using System.Diagnostics;
using System.Text;
using System.Text.Json;

// Headless smoke test for the LSP plugin infrastructure and the inline
// completion protocol surface. A deterministic test plugin is installed into a
// temporary plugins directory; the test drives the language server over stdio
// with and without the plugin and checks capability advertisement and both
// inline completion requests (standard textDocument/inlineCompletion and the
// pascalabc/inlineCompletion mirror).

const string expectedSuggestion = "Println(stub);";

var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(2));

var serverAssembly = Environment.GetEnvironmentVariable("PABC_TOOLING_SERVER_ASSEMBLY") ??
    Path.Combine(AppContext.BaseDirectory, "PascalABCNet.LanguageServer.dll");
Check(File.Exists(serverAssembly), $"Language server assembly exists: {serverAssembly}");

var testPluginAssembly = Path.Combine(AppContext.BaseDirectory, "InlineCompletionTestLspPlugin.dll");
Check(File.Exists(testPluginAssembly), $"Test plugin assembly built: {testPluginAssembly}");

var pluginsDir = Path.Combine(Path.GetTempPath(), "pabc-lsp-plugins-" + Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(pluginsDir);
foreach (var name in new[] { "InlineCompletionTestLspPlugin.dll", "PascalABCNet.LanguageServer.PluginApi.dll" })
{
    File.Copy(Path.Combine(AppContext.BaseDirectory, name), Path.Combine(pluginsDir, name));
}

var emptyPluginsDir = Path.Combine(Path.GetTempPath(), "pabc-lsp-empty-" + Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(emptyPluginsDir);

var source = "program InlineSmoke;\nbegin\n  Pri\nend.\n";
var caretOffset = source.IndexOf("Pri", StringComparison.Ordinal) + "Pri".Length;
var position = GetPosition(source, caretOffset);
var documentUri = "file:///C:/InlineSmoke.pas";

// --- Scenario 1: server with the test plugin installed ----------------------

{
    var (process, input, output, stderrTask) = StartServer(serverAssembly, pluginsDir);
    try
    {
        await WriteMessageAsync(input, new
        {
            jsonrpc = "2.0",
            id = 1,
            method = "initialize",
            @params = new { capabilities = new { } }
        }, timeout.Token);
        using var initialize = await ReadResponseAsync(output, 1, timeout.Token);
        var capabilitiesJson = initialize.RootElement.GetProperty("result").GetProperty("capabilities")
            .GetRawText();
        Check(capabilitiesJson.Contains("\"inlineCompletionProvider\"", StringComparison.Ordinal),
            $"initialize advertises the inline completion capability when a plugin is loaded: {capabilitiesJson}");
        Console.WriteLine("PASS plugin capability advertisement over stdio");

        await WriteDidOpenAsync(input, documentUri, source, version: 1, timeout.Token);

        await WriteRequestAsync(input, 2, "pascalabc/inlineCompletion", documentUri, position, timeout.Token);
        using var customResponse = await ReadResponseAsync(output, 2, timeout.Token);
        var customText = GetSingleInsertText(customResponse);
        Check(customText == expectedSuggestion,
            $"pascalabc/inlineCompletion returns the plugin suggestion (got '{customText}')");
        Console.WriteLine("PASS pascalabc/inlineCompletion over stdio");

        await WriteRequestAsync(input, 3, "textDocument/inlineCompletion", documentUri, position, timeout.Token);
        using var standardResponse = await ReadResponseAsync(output, 3, timeout.Token);
        var standardText = GetSingleInsertText(standardResponse);
        Check(standardText == expectedSuggestion,
            $"textDocument/inlineCompletion returns the plugin suggestion (got '{standardText}')");
        Console.WriteLine("PASS textDocument/inlineCompletion over stdio");

        await WriteMessageAsync(input, new { jsonrpc = "2.0", id = 4, method = "shutdown" }, timeout.Token);
        using var shutdown = await ReadResponseAsync(output, 4, timeout.Token);
        await WriteMessageAsync(input, new { jsonrpc = "2.0", method = "exit" }, timeout.Token);
        await process.WaitForExitAsync(timeout.Token);
        var errorOutput = await stderrTask.WaitAsync(timeout.Token);
        Check(process.ExitCode == 0, $"Server with plugin exited cleanly. stderr: {errorOutput}");
        Check(errorOutput.Contains("Loaded LSP plugin", StringComparison.Ordinal),
            $"Server log reports the loaded plugin. stderr: {errorOutput}");
    }
    finally
    {
        if (!process.HasExited)
        {
            process.Kill(entireProcessTree: true);
            await process.WaitForExitAsync();
        }
    }
    Console.WriteLine("PASS shutdown and exit with plugin over stdio");
}

// --- Scenario 2: server without any plugin ----------------------------------

{
    var (process, input, output, stderrTask) = StartServer(serverAssembly, emptyPluginsDir);
    try
    {
        await WriteMessageAsync(input, new
        {
            jsonrpc = "2.0",
            id = 1,
            method = "initialize",
            @params = new { capabilities = new { } }
        }, timeout.Token);
        using var initialize = await ReadResponseAsync(output, 1, timeout.Token);
        var capabilitiesJson = initialize.RootElement.GetProperty("result").GetProperty("capabilities")
            .GetRawText();
        Check(!capabilitiesJson.Contains("\"inlineCompletionProvider\"", StringComparison.Ordinal),
            $"initialize stays silent about inline completion without plugins: {capabilitiesJson}");

        await WriteDidOpenAsync(input, documentUri, source, version: 1, timeout.Token);

        await WriteRequestAsync(input, 2, "pascalabc/inlineCompletion", documentUri, position, timeout.Token);
        using var customResponse = await ReadResponseAsync(output, 2, timeout.Token);
        Check(GetInsertTexts(customResponse).Count == 0,
            "pascalabc/inlineCompletion returns no items without a plugin");

        await WriteRequestAsync(input, 3, "textDocument/inlineCompletion", documentUri, position, timeout.Token);
        using var standardResponse = await ReadResponseAsync(output, 3, timeout.Token);
        Check(GetInsertTexts(standardResponse).Count == 0,
            "textDocument/inlineCompletion returns no items without a plugin");
        Console.WriteLine("PASS inline completion is inert without a plugin");

        await WriteMessageAsync(input, new { jsonrpc = "2.0", id = 4, method = "shutdown" }, timeout.Token);
        using var shutdown = await ReadResponseAsync(output, 4, timeout.Token);
        await WriteMessageAsync(input, new { jsonrpc = "2.0", method = "exit" }, timeout.Token);
        await process.WaitForExitAsync(timeout.Token);
        var errorOutput = await stderrTask.WaitAsync(timeout.Token);
        Check(process.ExitCode == 0, $"Server without plugin exited cleanly. stderr: {errorOutput}");
    }
    finally
    {
        if (!process.HasExited)
        {
            process.Kill(entireProcessTree: true);
            await process.WaitForExitAsync();
        }
    }
    Console.WriteLine("PASS shutdown and exit without plugin over stdio");
}

Console.WriteLine("All inline completion plugin smoke checks passed.");

// --- Helpers ----------------------------------------------------------------

static (Process process, Stream input, Stream output, Task<string> stderrTask) StartServer(
    string serverAssembly,
    string pluginsDirectory)
{
    var startInfo = new ProcessStartInfo
    {
        FileName = "dotnet",
        UseShellExecute = false,
        CreateNoWindow = true,
        RedirectStandardInput = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true
    };
    startInfo.ArgumentList.Add(serverAssembly);
    startInfo.ArgumentList.Add("--stdio");
    startInfo.ArgumentList.Add("--plugins-dir");
    startInfo.ArgumentList.Add(pluginsDirectory);

    var process = new Process { StartInfo = startInfo };
    process.Start();
    return (process, process.StandardInput.BaseStream, process.StandardOutput.BaseStream,
        process.StandardError.ReadToEndAsync());
}

static List<string> GetInsertTexts(JsonDocument response)
{
    var items = response.RootElement.GetProperty("result").GetProperty("items");
    return items.EnumerateArray()
        .Select(item => item.GetProperty("insertText").GetString())
        .Where(text => !string.IsNullOrEmpty(text))
        .Select(text => text!)
        .ToList();
}

static string? GetSingleInsertText(JsonDocument response)
{
    var texts = GetInsertTexts(response);
    return texts.Count == 1 ? texts[0] : null;
}

static async Task WriteDidOpenAsync(
    Stream stream,
    string documentUri,
    string text,
    int version,
    CancellationToken cancellationToken)
{
    await WriteMessageAsync(stream, new
    {
        jsonrpc = "2.0",
        method = "textDocument/didOpen",
        @params = new
        {
            textDocument = new { uri = documentUri, languageId = "pascal", version, text }
        }
    }, cancellationToken);
}

static async Task WriteRequestAsync(
    Stream stream,
    int id,
    string method,
    string documentUri,
    (int Line, int Character) position,
    CancellationToken cancellationToken)
{
    await WriteMessageAsync(stream, new
    {
        jsonrpc = "2.0",
        id,
        method,
        @params = new
        {
            textDocument = new { uri = documentUri },
            position = new { line = position.Line, character = position.Character }
        }
    }, cancellationToken);
}

static async Task WriteMessageAsync(Stream stream, object message, CancellationToken cancellationToken)
{
    var body = JsonSerializer.SerializeToUtf8Bytes(message);
    var header = Encoding.ASCII.GetBytes($"Content-Length: {body.Length}\r\n\r\n");
    await stream.WriteAsync(header, cancellationToken);
    await stream.WriteAsync(body, cancellationToken);
    await stream.FlushAsync(cancellationToken);
}

static async Task<JsonDocument> ReadResponseAsync(
    Stream stream,
    int expectedId,
    CancellationToken cancellationToken)
{
    var header = new List<byte>();
    while (header.Count < 4 ||
           header[^4] != '\r' || header[^3] != '\n' ||
           header[^2] != '\r' || header[^1] != '\n')
    {
        var oneByte = new byte[1];
        var read = await stream.ReadAsync(oneByte, cancellationToken);
        if (read == 0)
            throw new EndOfStreamException("Language server closed stdout before sending a response.");
        header.Add(oneByte[0]);
    }

    var headerText = Encoding.ASCII.GetString(header.ToArray());
    var contentLengthLine = headerText.Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
        .Single(line => line.StartsWith("Content-Length:", StringComparison.OrdinalIgnoreCase));
    var contentLength = int.Parse(contentLengthLine["Content-Length:".Length..].Trim());
    var body = new byte[contentLength];
    var offset = 0;
    while (offset < contentLength)
    {
        var read = await stream.ReadAsync(body.AsMemory(offset, contentLength - offset), cancellationToken);
        if (read == 0)
            throw new EndOfStreamException("Language server closed stdout inside a response.");
        offset += read;
    }

    var response = JsonDocument.Parse(body);
    if (response.RootElement.TryGetProperty("error", out var error))
        throw new InvalidOperationException($"LSP request {expectedId} failed: {error}");
    Check(response.RootElement.GetProperty("id").GetInt32() == expectedId,
        $"Received response for request {expectedId}");
    return response;
}

static (int Line, int Character) GetPosition(string text, int offset)
{
    var line = 0;
    var character = 0;
    for (var index = 0; index < offset; index++)
    {
        if (text[index] == '\n')
        {
            line++;
            character = 0;
        }
        else
        {
            character++;
        }
    }

    return (line, character);
}

static void Check(bool condition, string message)
{
    if (!condition)
        throw new InvalidOperationException($"Smoke check failed: {message}");
}
