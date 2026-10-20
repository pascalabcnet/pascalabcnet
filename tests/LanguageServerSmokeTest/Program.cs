using System.Diagnostics;
using System.Text;
using System.Text.Json;
using PascalABCNet.LanguageServer;

const string source = """
program HeadlessSmoke;

uses System.Collections.Generic;

var
  text: string;

begin
  text := 'PascalABC.NET';
  var result := text.Substring(1);
  Writeln(result);
  Print(result);
  var values := new List<integer>;
end.
""";

var repositoryRoot = FindRepositoryRoot(Environment.CurrentDirectory);
var serverAssembly = Environment.GetEnvironmentVariable("PABC_TOOLING_SERVER_ASSEMBLY") ??
    Path.Combine(
        repositoryRoot,
        "bin-net10",
        "PascalABCNet.LanguageServer.dll");

var firstSyntheticUri = new Uri("untitled:VirtualOne.pas");
var secondSyntheticUri = new Uri("untitled:VirtualTwo.pas");
var firstSyntheticFileName = DocumentConversions.GetFileName(firstSyntheticUri);
var secondSyntheticFileName = DocumentConversions.GetFileName(secondSyntheticUri);
Check(
    firstSyntheticFileName.EndsWith(".pas", StringComparison.OrdinalIgnoreCase) &&
    secondSyntheticFileName.EndsWith(".pas", StringComparison.OrdinalIgnoreCase),
    "Synthetic document names preserve the Pascal extension");
Check(
    firstSyntheticFileName == DocumentConversions.GetFileName(firstSyntheticUri),
    "Synthetic document names are stable for the same URI");
Check(
    !string.Equals(firstSyntheticFileName, secondSyntheticFileName, StringComparison.OrdinalIgnoreCase),
    "Different non-file URIs receive different synthetic document names");
Check(File.Exists(serverAssembly), $"Language server assembly exists: {serverAssembly}");

using var process = new Process
{
    StartInfo = new ProcessStartInfo
    {
        FileName = "dotnet",
        UseShellExecute = false,
        CreateNoWindow = true,
        RedirectStandardInput = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        WorkingDirectory = Path.GetDirectoryName(Path.GetFullPath(serverAssembly))!
    }
};
process.StartInfo.ArgumentList.Add(serverAssembly);
process.StartInfo.ArgumentList.Add("--stdio");
process.StartInfo.ArgumentList.Add("--documentation-language");
process.StartInfo.ArgumentList.Add("ru");
Check(process.Start(), "Language server process started");

// Drain stderr concurrently and terminate this test's child on every failure.
var stderrTask = process.StandardError.ReadToEndAsync();
try
{
    using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(60));
    var input = process.StandardInput.BaseStream;
    var output = process.StandardOutput.BaseStream;
    var documentUri = "file:///C:/HeadlessSmoke.pas";

    await WriteMessageAsync(input, new
    {
        jsonrpc = "2.0",
        id = 1,
        method = "initialize",
        @params = new
        {
            processId = Environment.ProcessId,
            rootUri = "file:///C:/",
            capabilities = new { }
        }
    }, timeout.Token);
    using var initialize = await ReadResponseAsync(output, 1, timeout.Token);
    Check(initialize.RootElement.GetProperty("result").GetProperty("capabilities")
        .GetProperty("completionProvider").GetProperty("triggerCharacters")[0].GetString() == ".",
        "initialize advertises completion after dot");
    Check(initialize.RootElement.GetProperty("result").GetProperty("capabilities")
        .GetProperty("textDocumentSync").GetProperty("change").GetInt32() == 2,
        "initialize advertises incremental document synchronization");
    Check(initialize.RootElement.GetProperty("result").GetProperty("capabilities")
        .GetProperty("documentSymbolProvider").GetBoolean(),
        "initialize advertises document symbols");
    Console.WriteLine("PASS initialize over stdio");

    await WriteMessageAsync(input, new
    {
        jsonrpc = "2.0", id = 951, method = "pascalabc/searchNuGetPackages",
        @params = new { uri = documentUri, prefix = "No" }
    }, timeout.Token);
    using var localPackages = await ReadResponseAsync(output, 951, timeout.Token);
    Check(localPackages.RootElement.GetProperty("result").ValueKind == JsonValueKind.Array,
        "short NuGet prefix is served by the local cache through LSP");

    const string changingUnitUri = "file:///C:/a.pas";
    const string fileBody = "\nbegin\n  var f := OpenWrite('xxx.txt');\n  f.Close;\nend.\n";
    await WriteDidOpenAsync(input, changingUnitUri, "unit A;" + fileBody, 1, timeout.Token);
    await WriteRequestAsync(input, 950, "textDocument/completion", changingUnitUri, (3, 4), timeout.Token);
    using var unitCompletion = await ReadResponseAsync(output, 950, timeout.Token);
    foreach (var change in new[] { (Version: 2, Text: "//unit A;" + fileBody),
                                 (Version: 3, Text: "unit A;" + fileBody),
                                 (Version: 4, Text: "//unit A;" + fileBody) })
    {
        await WriteMessageAsync(input, new { jsonrpc = "2.0", method = "textDocument/didChange",
            @params = new { textDocument = new { uri = changingUnitUri, version = change.Version },
                contentChanges = new[] { new { text = change.Text } } } }, timeout.Token);
        await WriteMessageAsync(input, new { jsonrpc = "2.0", method = "textDocument/didSave",
            @params = new { textDocument = new { uri = changingUnitUri } } }, timeout.Token);
        await WriteRequestAsync(input, 950 + change.Version, "textDocument/completion", changingUnitUri, (3, 4), timeout.Token);
        using var changedCompletion = await ReadResponseAsync(output, 950 + change.Version, timeout.Token);
        Check(!process.HasExited, "LanguageServer survives unit/program header edits and save notifications");
    }
    await WriteMessageAsync(input, new { jsonrpc = "2.0", method = "textDocument/didClose",
        @params = new { textDocument = new { uri = changingUnitUri } } }, timeout.Token);
    Console.WriteLine("PASS unit/program edits with OpenWrite over stdio");

    await WriteMessageAsync(input, new
    {
        jsonrpc = "2.0",
        method = "initialized",
        @params = new { }
    }, timeout.Token);

    await WriteMessageAsync(input, new
    {
        jsonrpc = "2.0",
        method = "textDocument/didOpen",
        @params = new
        {
            textDocument = new
            {
                uri = documentUri,
                languageId = "pascal",
                version = 1,
                text = source
            }
        }
    }, timeout.Token);

    await WriteMessageAsync(input, new { jsonrpc = "2.0", id = 999, method = "textDocument/documentSymbol",
        @params = new { textDocument = new { uri = documentUri } } }, timeout.Token);
    using var documentSymbols = await ReadResponseAsync(output, 999, timeout.Token);
    Check(documentSymbols.RootElement.GetProperty("result").EnumerateArray().Any(symbol =>
        symbol.GetProperty("name").GetString() == "text"), "document symbols include unsaved global variable");
    Console.WriteLine("PASS document symbols over stdio");

    const string symbolUri = "file:///C:/SymbolsSmoke.pas";
    const string symbolSource = "type\n  Person = class\n    public\n    auto property Name: string;\n  end;\nbegin\nend.";
    await WriteDidOpenAsync(input, symbolUri, symbolSource, 1, timeout.Token);
    await WriteMessageAsync(input, new { jsonrpc = "2.0", id = 998, method = "textDocument/documentSymbol",
        @params = new { textDocument = new { uri = symbolUri } } }, timeout.Token);
    using var nestedSymbols = await ReadResponseAsync(output, 998, timeout.Token);
    Check(nestedSymbols.RootElement.GetProperty("result").EnumerateArray().Any(symbol =>
        symbol.GetProperty("name").GetString() == "Person" &&
        symbol.GetProperty("children").EnumerateArray().Any(child => child.GetProperty("name").GetString() == "Name") &&
        symbol.GetProperty("range").GetProperty("end").GetProperty("line").GetInt32() >
            symbol.GetProperty("range").GetProperty("start").GetProperty("line").GetInt32()),
        "document symbols retain type members");
    await WriteMessageAsync(input, new { jsonrpc = "2.0", method = "textDocument/didChange",
        @params = new { textDocument = new { uri = symbolUri, version = 2 },
            contentChanges = new[] { new { text = symbolSource.Replace("Person", "Student") } } } }, timeout.Token);
    await WriteMessageAsync(input, new { jsonrpc = "2.0", id = 997, method = "textDocument/documentSymbol",
        @params = new { textDocument = new { uri = symbolUri } } }, timeout.Token);
    using var editedSymbols = await ReadResponseAsync(output, 997, timeout.Token);
    Check(editedSymbols.RootElement.GetProperty("result").EnumerateArray().Any(symbol =>
        symbol.GetProperty("name").GetString() == "Student"), "document symbols follow unsaved edits");
    await WriteMessageAsync(input, new { jsonrpc = "2.0", method = "textDocument/didClose",
        @params = new { textDocument = new { uri = symbolUri } } }, timeout.Token);

    const string memberAccess = "text.Substring";
    var memberStart = source.IndexOf(memberAccess, StringComparison.Ordinal);
    Check(memberStart >= 0, "Completion marker exists");
    var dotCaretOffset = memberStart + "text.".Length;
    var dotPosition = GetPosition(source, dotCaretOffset);

    await WriteRequestAsync(input, 2, "textDocument/completion", documentUri, dotPosition, timeout.Token);
    using var completion = await ReadResponseAsync(output, 2, timeout.Token);
    Check(completion.RootElement.GetProperty("result").GetProperty("items")
        .EnumerateArray().Any(item => item.GetProperty("label").GetString() == "Substring"),
        "completion contains Substring");
    Console.WriteLine("PASS completion over stdio");

    var printStart = source.IndexOf("Print(result)", StringComparison.Ordinal);
    Check(printStart >= 0, "Global completion marker exists");

    await WriteRequestAsync(input, 900, "textDocument/definition", documentUri,
        GetPosition(source, printStart + 1), timeout.Token);
    using var printDefinition = await ReadResponseAsync(output, 900, timeout.Token);
    var serverRoot = Path.GetDirectoryName(Path.GetFullPath(serverAssembly))!;
    var librarySourceRoot = Directory.Exists(Path.Combine(serverRoot, "LibSource"))
        ? Path.Combine(serverRoot, "LibSource") : Path.Combine(serverRoot, "Lib");
    var expectedSystemSource = Path.Combine(librarySourceRoot, "PABCSystem.pas");
    Check(printDefinition.RootElement.GetProperty("result").EnumerateArray().Any(location =>
            string.Equals(new Uri(location.GetProperty("uri").GetString()!).LocalPath,
                expectedSystemSource, StringComparison.OrdinalIgnoreCase)),
        "Print definition navigates to the server kit's standard-library source");
    Console.WriteLine("PASS standard-library source navigation over stdio");
    await WriteRequestAsync(input, 901, "textDocument/implementation", documentUri,
        GetPosition(source, printStart + 1), timeout.Token);
    using var printImplementation = await ReadResponseAsync(output, 901, timeout.Token);
    var implementationSectionLine = Array.FindIndex(File.ReadAllLines(expectedSystemSource),
        line => line.Trim().Equals("implementation", StringComparison.OrdinalIgnoreCase));
    Check(implementationSectionLine >= 0 &&
        printImplementation.RootElement.GetProperty("result").EnumerateArray().Any(location =>
            string.Equals(new Uri(location.GetProperty("uri").GetString()!).LocalPath,
                expectedSystemSource, StringComparison.OrdinalIgnoreCase) &&
            location.GetProperty("range").GetProperty("start").GetProperty("line").GetInt32() > implementationSectionLine),
        "Ctrl-click navigation targets the implementation section in the server kit's library source");
    Console.WriteLine("PASS standard-library implementation navigation over stdio");

    await WriteRequestAsync(
        input,
        30,
        "textDocument/completion",
        documentUri,
        GetPosition(source, printStart),
        timeout.Token);
    using var invokedCompletion = await ReadResponseAsync(output, 30, timeout.Token);
    Check(GetCompletionLabels(invokedCompletion).Contains("Print"),
        "invoked completion contains Print");

    await WriteRequestAsync(
        input,
        31,
        "textDocument/completion",
        documentUri,
        GetPosition(source, printStart + "Pri".Length),
        timeout.Token);
    using var prefixCompletion = await ReadResponseAsync(output, 31, timeout.Token);
    var prefixLabels = GetCompletionLabels(prefixCompletion);
    Check(prefixLabels.Contains("Print"),
        "prefix completion contains Print");
    Check(prefixLabels.All(label => label.StartsWith("Pri", StringComparison.OrdinalIgnoreCase)),
        "prefix completion excludes fuzzy non-prefix matches");

    var namespaceStart = source.IndexOf("System.Collections", StringComparison.Ordinal);
    await WriteRequestAsync(
        input,
        32,
        "textDocument/completion",
        documentUri,
        GetPosition(source, namespaceStart + "Sys".Length),
        timeout.Token);
    using var namespaceCompletion = await ReadResponseAsync(output, 32, timeout.Token);
    Check(GetCompletionLabels(namespaceCompletion).Contains("System"),
        "namespace completion contains System after uses Sys");

    var typeStart = source.IndexOf("List<integer>", StringComparison.Ordinal);
    await WriteRequestAsync(
        input,
        33,
        "textDocument/completion",
        documentUri,
        GetPosition(source, typeStart + "Lis".Length),
        timeout.Token);
    using var typeCompletion = await ReadResponseAsync(output, 33, timeout.Token);
    Check(GetCompletionLabels(typeCompletion).Contains("List<>"),
        "type completion contains List after new Lis");

    await WriteRequestAsync(
        input,
        34,
        "textDocument/completion",
        documentUri,
        (0, 0),
        timeout.Token);
    using var fileStartCompletion = await ReadResponseAsync(output, 34, timeout.Token);
    Check(GetCompletionLabels(fileStartCompletion).Contains("program"),
        "completion at file start contains program");
    Console.WriteLine("PASS invoked, prefix, namespace and type completion over stdio");

    var hoverOffset = memberStart + "text.".Length + 1;
    var hoverPosition = GetPosition(source, hoverOffset);
    await WriteRequestAsync(input, 3, "textDocument/hover", documentUri, hoverPosition, timeout.Token);
    using var hover = await ReadResponseAsync(output, 3, timeout.Token);
    Check(hover.RootElement.GetProperty("result").GetProperty("contents").GetProperty("value")
        .GetString()?.Contains("Substring", StringComparison.OrdinalIgnoreCase) == true,
        "hover describes Substring");
    Console.WriteLine("PASS hover over stdio");

    var signatureOffset = memberStart + memberAccess.Length + 1;
    var signaturePosition = GetPosition(source, signatureOffset);
    await WriteRequestAsync(input, 4, "textDocument/signatureHelp", documentUri, signaturePosition, timeout.Token);
    using var signature = await ReadResponseAsync(output, 4, timeout.Token);
    var substringSignatures = signature.RootElement.GetProperty("result").GetProperty("signatures");
    Check(substringSignatures.EnumerateArray().Any(item => item.GetProperty("label").GetString()
        ?.Contains("Substring", StringComparison.OrdinalIgnoreCase) == true),
        "signature help describes Substring");
    Check(substringSignatures.EnumerateArray().Any(item =>
            item.GetProperty("parameters").GetArrayLength() > 0),
        "signature help contains LSP parameters");
    Console.WriteLine("PASS signature help over stdio");

    const string printSource = """
    begin
      Print(1)
    end.
    """;
    const string printUri = "file:///C:/PrintSignature.pas";
    await WriteDidOpenAsync(input, printUri, printSource, version: 1, timeout.Token);
    var printSignatureOffset = printSource.IndexOf("Print(", StringComparison.Ordinal) + "Print(".Length;
    await WriteRequestAsync(
        input,
        26,
        "textDocument/signatureHelp",
        printUri,
        GetPosition(printSource, printSignatureOffset),
        timeout.Token);
    using var printSignature = await ReadResponseAsync(output, 26, timeout.Token);
    var printSignatures = printSignature.RootElement.GetProperty("result").GetProperty("signatures");
    Check(printSignatures.EnumerateArray().Any(item =>
            item.GetProperty("parameters").GetArrayLength() > 0),
        "Print signature contains LSP parameters");
    Check(printSignatures.EnumerateArray().Any(item =>
            item.TryGetProperty("documentation", out var documentation) &&
            documentation.ValueKind == JsonValueKind.String &&
            !string.IsNullOrWhiteSpace(documentation.GetString())),
        "Print signature contains Pascal documentation over LSP");
    Console.WriteLine("PASS Print parameters and documentation over stdio");

    const string documentedBeforeDot = """
    type TDocumented = class
      /// Возвращает переданное значение
      function Value(x: integer): integer;
    end;

    function TDocumented.Value(x: integer): integer;
    begin
      Result := x;
    end;

    begin
      var item := new TDocumented;
      item
    end.
    """;
    const string documentedAfterDot = """
    type TDocumented = class
      /// Возвращает переданное значение
      function Value(x: integer): integer;
    end;

    function TDocumented.Value(x: integer): integer;
    begin
      Result := x;
    end;

    begin
      var item := new TDocumented;
      item.
    end.
    """;
    const string documentedUri = "file:///C:/DocumentedCompletion.pas";
    await WriteDidOpenAsync(input, documentedUri, documentedBeforeDot, version: 1, timeout.Token);
    await WriteDidChangeAsync(input, documentedUri, documentedAfterDot, version: 2, timeout.Token);
    var documentedOffset = documentedAfterDot.IndexOf("item.", StringComparison.Ordinal) + "item.".Length;
    await WriteRequestAsync(
        input,
        27,
        "textDocument/completion",
        documentedUri,
        GetPosition(documentedAfterDot, documentedOffset),
        timeout.Token);
    using var documentedCompletion = await ReadResponseAsync(output, 27, timeout.Token);
    Check(documentedCompletion.RootElement.GetProperty("result").GetProperty("items")
            .EnumerateArray().Any(item =>
                item.GetProperty("label").GetString() == "Value" &&
                item.TryGetProperty("documentation", out var documentation) &&
                documentation.ValueKind == JsonValueKind.String &&
                documentation.GetString()?.Contains("Возвращает", StringComparison.Ordinal) == true),
        "Pascal completion contains source documentation over LSP");
    Console.WriteLine("PASS Pascal completion documentation over stdio");

    const string dateTimeBeforeDot = """
    begin
      DateTime
    end.
    """;
    const string dateTimeAfterDot = """
    begin
      DateTime.
    end.
    """;
    const string dateTimeUri = "file:///C:/DateTimeCompletion.pas";
    await WriteDidOpenAsync(input, dateTimeUri, dateTimeBeforeDot, version: 1, timeout.Token);
    await WriteDidChangeAsync(input, dateTimeUri, dateTimeAfterDot, version: 2, timeout.Token);
    var dateTimeCaretOffset = dateTimeAfterDot.IndexOf("DateTime.", StringComparison.Ordinal) + "DateTime.".Length;
    var dateTimePosition = GetPosition(dateTimeAfterDot, dateTimeCaretOffset);
    await WriteRequestAsync(input, 21, "textDocument/completion", dateTimeUri, dateTimePosition, timeout.Token);
    using var dateTimeCompletion = await ReadResponseAsync(output, 21, timeout.Token);
    var dateTimeLabels = GetCompletionLabels(dateTimeCompletion);
    Check(
        new[] { "Now", "Today", "UtcNow" }.All(dateTimeLabels.Contains),
        $"DateTime completion contains Now, Today and UtcNow; actual: {string.Join(", ", dateTimeLabels)}");
    Console.WriteLine("PASS DateTime completion over stdio");

    const string arrayBeforeDot = """
    begin
      var a := Arr(1,2,3,4,5,6);
      var s := a
    end.
    """;
    const string arrayAfterDot = """
    begin
      var a := Arr(1,2,3,4,5,6);
      var s := a.
    end.
    """;
    const string arrayUri = "file:///C:/ArrayAsSpanCompletion.pas";
    await WriteDidOpenAsync(input, arrayUri, arrayBeforeDot, version: 1, timeout.Token);
    await WriteDidChangeAsync(input, arrayUri, arrayAfterDot, version: 2, timeout.Token);
    var arrayCaretOffset = arrayAfterDot.IndexOf("a.", StringComparison.Ordinal) + "a.".Length;
    var arrayPosition = GetPosition(arrayAfterDot, arrayCaretOffset);
    await WriteRequestAsync(input, 23, "textDocument/completion", arrayUri, arrayPosition, timeout.Token);
    using var arrayCompletion = await ReadResponseAsync(output, 23, timeout.Token);
    var arrayLabels = GetCompletionLabels(arrayCompletion);
    Check(
        arrayLabels.Any(label => label.StartsWith("AsSpan", StringComparison.Ordinal)),
        $"Array completion contains AsSpan; actual: {string.Join(", ", arrayLabels)}");
    Console.WriteLine("PASS array AsSpan completion over stdio");

    const string arraySignatureSource = """
    begin
      var a := Arr(1,2,3,4,5,6);
      var s := a.AsSpan()
    end.
    """;
    const string arraySignatureUri = "file:///C:/ArrayAsSpanSignature.pas";
    await WriteDidOpenAsync(input, arraySignatureUri, arraySignatureSource, version: 1, timeout.Token);
    var arraySignatureOffset =
        arraySignatureSource.IndexOf("AsSpan(", StringComparison.Ordinal) + "AsSpan(".Length;
    var arraySignaturePosition = GetPosition(arraySignatureSource, arraySignatureOffset);
    await WriteRequestAsync(input, 25, "textDocument/signatureHelp", arraySignatureUri, arraySignaturePosition, timeout.Token);
    using var arraySignature = await ReadResponseAsync(output, 25, timeout.Token);
    Check(
        arraySignature.RootElement.GetProperty("result").GetProperty("signatures")
            .EnumerateArray().Any(item => item.GetProperty("label").GetString()
                ?.Contains("AsSpan", StringComparison.OrdinalIgnoreCase) == true),
        "Array AsSpan signature help contains AsSpan");
    Console.WriteLine("PASS array AsSpan signature help over stdio");

    const string firstVirtualSource = """
    program VirtualOne;
    type TFirstVirtual = class procedure FirstMember; end;
    procedure TFirstVirtual.FirstMember;
    begin
    end;
    var value: TFirstVirtual;
    begin
      value.FirstMember;
    end.
    """;

    const string secondVirtualSource = """
    program VirtualTwo;
    type TSecondVirtual = class procedure SecondMember; end;
    procedure TSecondVirtual.SecondMember;
    begin
    end;
    var value: TSecondVirtual;
    begin
      value.SecondMember;
    end.
    """;

    await WriteDidOpenAsync(
        input,
        firstSyntheticUri.AbsoluteUri,
        firstVirtualSource,
        version: 1,
        timeout.Token);
    await WriteDidOpenAsync(
        input,
        secondSyntheticUri.AbsoluteUri,
        secondVirtualSource,
        version: 1,
        timeout.Token);
    var firstVirtualOffset =
        firstVirtualSource.IndexOf("value.", StringComparison.Ordinal) + "value.".Length;
    var secondVirtualOffset =
        secondVirtualSource.IndexOf("value.", StringComparison.Ordinal) + "value.".Length;
    await WriteRequestAsync(
        input,
        18,
        "textDocument/completion",
        firstSyntheticUri.AbsoluteUri,
        GetPosition(firstVirtualSource, firstVirtualOffset),
        timeout.Token);
    using var firstVirtualCompletion = await ReadResponseAsync(output, 18, timeout.Token);
    await WriteRequestAsync(
        input,
        19,
        "textDocument/completion",
        secondSyntheticUri.AbsoluteUri,
        GetPosition(secondVirtualSource, secondVirtualOffset),
        timeout.Token);
    using var secondVirtualCompletion = await ReadResponseAsync(output, 19, timeout.Token);
    var firstVirtualLabels = GetCompletionLabels(firstVirtualCompletion);
    var secondVirtualLabels = GetCompletionLabels(secondVirtualCompletion);
    Check(
        firstVirtualLabels.Contains("FirstMember") && !firstVirtualLabels.Contains("SecondMember"),
        "First virtual document keeps its own semantic model");
    Check(
        secondVirtualLabels.Contains("SecondMember") && !secondVirtualLabels.Contains("FirstMember"),
        "Second virtual document keeps its own semantic model");
    Console.WriteLine("PASS simultaneous non-file documents over stdio");

    var dependencyDirectory = Path.Combine(Path.GetTempPath(), "PascalABCNet.LanguageServerSmokeTest." + Guid.NewGuid().ToString("N"));
    Directory.CreateDirectory(dependencyDirectory);
    var dependencyFileName = Path.Combine(dependencyDirectory, "LspDependencyA.pas");
    var consumerFileName = Path.Combine(dependencyDirectory, "LspDependencyB.pas");
    var dependencyUri = new Uri(dependencyFileName).AbsoluteUri;
    var consumerUri = new Uri(consumerFileName).AbsoluteUri;

    const string dependencySourceV1 = """
    unit LspDependencyA;
    interface
    type
      TLspDependency = class
        procedure Foo;
      end;
    implementation
    procedure TLspDependency.Foo;
    begin
    end;
    end.
    """;

    const string dependencyBrokenSource = """
    unit LspDependencyA;
    interface
    type
      TLspDependency = class
        procedure
      end;
    implementation
    end.
    """;

    const string dependencySourceV3 = """
    unit LspDependencyA;
    interface
    type
      TLspDependency = class
        procedure Bar;
      end;
    implementation
    procedure TLspDependency.Bar;
    begin
    end;
    end.
    """;

    const string dependencySourceV4 = """
    unit LspDependencyA;
    interface
    type
      TLspDependency = class
        procedure Baz;
      end;
    implementation
    procedure TLspDependency.Baz;
    begin
    end;
    end.
    """;

    const string dependencySourceV5 = """
    unit LspDependencyA;
    interface
    type
      TLspDependency = class
        procedure LongMember;
      end;
    implementation
    procedure TLspDependency.LongMember;
    begin
    end;
    end.
    """;

    const string dependencySourceV6 = """
    unit LspDependencyA;
    interface
    type
      TLspDependency = class
        procedure FinalMember;
      end;
    implementation
    procedure TLspDependency.FinalMember;
    begin
    end;
    end.
    """;

    const string consumerSource = """
    program LspDependencyB;
    uses LspDependencyA;
    var value: TLspDependency;
    begin
      value.Foo;
    end.
    """;

    File.WriteAllText(dependencyFileName, dependencySourceV1);
    File.WriteAllText(consumerFileName, consumerSource);
    await WriteDidOpenAsync(input, dependencyUri, dependencySourceV1, version: 1, timeout.Token);
    await WriteDidOpenAsync(input, consumerUri, consumerSource, version: 1, timeout.Token);

    var dependencyCaretOffset = consumerSource.IndexOf("value.", StringComparison.Ordinal) + "value.".Length;
    var dependencyPosition = GetPosition(consumerSource, dependencyCaretOffset);
    await WriteRequestAsync(input, 5, "textDocument/completion", consumerUri, dependencyPosition, timeout.Token);
    using var initialDependencyCompletion = await ReadResponseAsync(output, 5, timeout.Token);
    Check(
        GetCompletionLabels(initialDependencyCompletion).Contains("Foo"),
        "LSP consumer initially sees Foo from dependency A");

    File.WriteAllText(dependencyFileName, dependencyBrokenSource);
    await WriteDidChangeAsync(input, dependencyUri, dependencyBrokenSource, version: 2, timeout.Token);
    File.WriteAllText(dependencyFileName, dependencySourceV3);
    await WriteDidChangeAsync(input, dependencyUri, dependencySourceV3, version: 3, timeout.Token);

    await WriteRequestAsync(input, 6, "textDocument/completion", consumerUri, dependencyPosition, timeout.Token);
    using var refreshedDependencyCompletion = await ReadResponseAsync(output, 6, timeout.Token);
    var refreshedLabels = GetCompletionLabels(refreshedDependencyCompletion);
    Check(refreshedLabels.Contains("Bar"), "LSP consumer sees Bar after burst update of A");
    Check(!refreshedLabels.Contains("Foo"), "LSP consumer drops stale Foo after burst update of A");

    await WriteDidChangeAsync(input, dependencyUri, dependencyBrokenSource, version: 2, timeout.Token);
    await WriteRequestAsync(input, 7, "textDocument/completion", consumerUri, dependencyPosition, timeout.Token);
    using var completionAfterStaleVersion = await ReadResponseAsync(output, 7, timeout.Token);
    var labelsAfterStaleVersion = GetCompletionLabels(completionAfterStaleVersion);
    Check(labelsAfterStaleVersion.Contains("Bar") && !labelsAfterStaleVersion.Contains("Foo"),
        "Stale LSP document version cannot roll semantic state back");

    var barOffsets = FindAllOffsets(dependencySourceV3, "Bar");
    Check(barOffsets.Count == 2, "Dependency source contains two Bar ranges to update");
    File.WriteAllText(dependencyFileName, dependencySourceV4);
    await WriteDidChangeRangesAsync(
        input,
        dependencyUri,
        version: 4,
        barOffsets.Select(offset =>
        {
            var start = GetPosition(dependencySourceV3, offset);
            var end = GetPosition(dependencySourceV3, offset + "Bar".Length);
            return (start, end, "Baz");
        }).ToArray(),
        timeout.Token);
    await WriteRequestAsync(input, 10, "textDocument/completion", consumerUri, dependencyPosition, timeout.Token);
    using var completionAfterIncrementalChanges = await ReadResponseAsync(output, 10, timeout.Token);
    var labelsAfterIncrementalChanges = GetCompletionLabels(completionAfterIncrementalChanges);
    Check(labelsAfterIncrementalChanges.Contains("Baz"),
        "LSP consumer sees Baz after incremental range changes in A");
    Check(!labelsAfterIncrementalChanges.Contains("Foo") && !labelsAfterIncrementalChanges.Contains("Bar"),
        "Incremental range changes remove superseded dependency members");

    var firstBazOffset = dependencySourceV4.IndexOf("Baz", StringComparison.Ordinal);
    var sourceAfterFirstVariableLengthChange = string.Concat(
        dependencySourceV4.AsSpan(0, firstBazOffset),
        "LongMember",
        dependencySourceV4.AsSpan(firstBazOffset + "Baz".Length));
    var secondBazOffset = sourceAfterFirstVariableLengthChange.IndexOf("Baz", StringComparison.Ordinal);
    File.WriteAllText(dependencyFileName, dependencySourceV5);
    await WriteDidChangeRangesAsync(
        input,
        dependencyUri,
        version: 5,
        new[]
        {
            (
                GetPosition(dependencySourceV4, firstBazOffset),
                GetPosition(dependencySourceV4, firstBazOffset + "Baz".Length),
                "LongMember"),
            (
                GetPosition(sourceAfterFirstVariableLengthChange, secondBazOffset),
                GetPosition(sourceAfterFirstVariableLengthChange, secondBazOffset + "Baz".Length),
                "LongMember")
        },
        timeout.Token);
    await WriteRequestAsync(input, 11, "textDocument/completion", consumerUri, dependencyPosition, timeout.Token);
    using var completionAfterVariableLengthChanges = await ReadResponseAsync(output, 11, timeout.Token);
    var labelsAfterVariableLengthChanges = GetCompletionLabels(completionAfterVariableLengthChanges);
    Check(labelsAfterVariableLengthChanges.Contains("LongMember"),
        "Sequential incremental ranges are based on the text produced by the previous change");
    Check(!labelsAfterVariableLengthChanges.Contains("Baz"),
        "Variable-length incremental ranges remove the previous member");

    await WriteDidChangeRangesAsync(
        input,
        dependencyUri,
        version: 6,
        new[] { ((Line: 999, Character: 0), (Line: 999, Character: 1), "corrupt") },
        timeout.Token);
    await WriteRequestAsync(input, 12, "textDocument/completion", consumerUri, dependencyPosition, timeout.Token);
    using var completionAfterInvalidRange = await ReadResponseAsync(output, 12, timeout.Token);
    Check(GetCompletionLabels(completionAfterInvalidRange).Contains("LongMember"),
        "An invalid incremental range does not corrupt the current document snapshot");

    File.WriteAllText(dependencyFileName, dependencySourceV6);
    await WriteDidChangeAsync(input, dependencyUri, dependencySourceV6, version: 6, timeout.Token);
    await WriteRequestAsync(input, 13, "textDocument/completion", consumerUri, dependencyPosition, timeout.Token);
    using var completionAfterInvalidRangeRecovery = await ReadResponseAsync(output, 13, timeout.Token);
    var labelsAfterInvalidRangeRecovery = GetCompletionLabels(completionAfterInvalidRangeRecovery);
    Check(labelsAfterInvalidRangeRecovery.Contains("FinalMember") &&
          !labelsAfterInvalidRangeRecovery.Contains("LongMember"),
        "A valid update is accepted after an invalid range with the same version was rejected");
    Console.WriteLine("PASS dependency refresh and burst updates over stdio");
    Console.WriteLine("PASS incremental range synchronization over stdio");

    var chainAFileName = Path.Combine(dependencyDirectory, "LspChainA.pas");
    var chainBFileName = Path.Combine(dependencyDirectory, "LspChainB.pas");
    var chainCFileName = Path.Combine(dependencyDirectory, "LspChainC.pas");
    var chainAUri = new Uri(chainAFileName).AbsoluteUri;
    var chainBUri = new Uri(chainBFileName).AbsoluteUri;
    var chainCUri = new Uri(chainCFileName).AbsoluteUri;

    const string chainASourceV1 = """
    unit LspChainA;
    interface
    type TBase = class procedure OldMember; end;
    implementation
    procedure TBase.OldMember;
    begin
    end;
    end.
    """;

    const string chainASourceV2 = """
    unit LspChainA;
    interface
    type TBase = class procedure NewMember; end;
    implementation
    procedure TBase.NewMember;
    begin
    end;
    end.
    """;

    const string chainBSource = """
    unit LspChainB;
    interface
    uses LspChainA;
    type TDerived = class(TBase) end;
    implementation
    end.
    """;

    const string chainCSource = """
    program LspChainC;
    uses LspChainB;
    var value: TDerived;
    begin
      value.OldMember;
    end.
    """;

    File.WriteAllText(chainAFileName, chainASourceV1);
    File.WriteAllText(chainBFileName, chainBSource);
    File.WriteAllText(chainCFileName, chainCSource);
    await WriteDidOpenAsync(input, chainAUri, chainASourceV1, version: 1, timeout.Token);
    await WriteDidOpenAsync(input, chainBUri, chainBSource, version: 1, timeout.Token);
    await WriteDidOpenAsync(input, chainCUri, chainCSource, version: 1, timeout.Token);

    var chainCaretOffset = chainCSource.IndexOf("value.", StringComparison.Ordinal) + "value.".Length;
    var chainPosition = GetPosition(chainCSource, chainCaretOffset);
    await WriteRequestAsync(input, 8, "textDocument/completion", chainCUri, chainPosition, timeout.Token);
    using var initialChainCompletion = await ReadResponseAsync(output, 8, timeout.Token);
    Check(GetCompletionLabels(initialChainCompletion).Contains("OldMember"),
        "Transitive LSP consumer initially sees OldMember");

    File.WriteAllText(chainAFileName, chainASourceV2);
    await WriteDidChangeAsync(input, chainAUri, chainASourceV2, version: 2, timeout.Token);
    await WriteRequestAsync(input, 9, "textDocument/completion", chainCUri, chainPosition, timeout.Token);
    using var refreshedChainCompletion = await ReadResponseAsync(output, 9, timeout.Token);
    var refreshedChainLabels = GetCompletionLabels(refreshedChainCompletion);
    Check(refreshedChainLabels.Contains("NewMember"),
        "Transitive LSP consumer sees NewMember after A changes");
    Check(!refreshedChainLabels.Contains("OldMember"),
        "Transitive LSP consumer drops stale OldMember after A changes");
    Console.WriteLine("PASS transitive dependency refresh over stdio");

    var implementationAFileName = Path.Combine(dependencyDirectory, "LspImplementationA.pas");
    var implementationBFileName = Path.Combine(dependencyDirectory, "LspImplementationB.pas");
    var implementationAUri = new Uri(implementationAFileName).AbsoluteUri;
    var implementationBUri = new Uri(implementationBFileName).AbsoluteUri;

    const string implementationASourceV1 = """
    unit LspImplementationA;
    interface
    type TImplementationDependency = class procedure OldMember; end;
    implementation
    procedure TImplementationDependency.OldMember;
    begin
    end;
    end.
    """;

    const string implementationASourceV2 = """
    unit LspImplementationA;
    interface
    type TImplementationDependency = class procedure NewMember; end;
    implementation
    procedure TImplementationDependency.NewMember;
    begin
    end;
    end.
    """;

    const string implementationBSource = """
    unit LspImplementationB;
    interface
    procedure Touch;
    implementation
    uses LspImplementationA;
    procedure Touch;
    var value: TImplementationDependency;
    begin
      value.OldMember;
    end;
    end.
    """;

    File.WriteAllText(implementationAFileName, implementationASourceV1);
    File.WriteAllText(implementationBFileName, implementationBSource);
    await WriteDidOpenAsync(input, implementationAUri, implementationASourceV1, version: 1, timeout.Token);
    await WriteDidOpenAsync(input, implementationBUri, implementationBSource, version: 1, timeout.Token);

    var implementationCaretOffset =
        implementationBSource.IndexOf("value.", StringComparison.Ordinal) + "value.".Length;
    var implementationPosition = GetPosition(implementationBSource, implementationCaretOffset);
    await WriteRequestAsync(
        input,
        14,
        "textDocument/completion",
        implementationBUri,
        implementationPosition,
        timeout.Token);
    using var initialImplementationCompletion = await ReadResponseAsync(output, 14, timeout.Token);
    Check(
        GetCompletionLabels(initialImplementationCompletion).Contains("OldMember"),
        "Implementation-only LSP consumer initially sees OldMember");

    File.WriteAllText(implementationAFileName, implementationASourceV2);
    await WriteDidChangeAsync(input, implementationAUri, implementationASourceV2, version: 2, timeout.Token);
    await WriteRequestAsync(
        input,
        15,
        "textDocument/completion",
        implementationBUri,
        implementationPosition,
        timeout.Token);
    using var refreshedImplementationCompletion = await ReadResponseAsync(output, 15, timeout.Token);
    var refreshedImplementationLabels = GetCompletionLabels(refreshedImplementationCompletion);
    Check(
        refreshedImplementationLabels.Contains("NewMember"),
        "Implementation-only LSP consumer sees NewMember after A changes");
    Check(
        !refreshedImplementationLabels.Contains("OldMember"),
        "Implementation-only LSP consumer drops stale OldMember after A changes");
    Console.WriteLine("PASS implementation uses dependency refresh over stdio");

    File.WriteAllText(dependencyFileName, dependencySourceV1);
    await WriteDidChangeAsync(input, dependencyUri, dependencySourceV3, version: 7, timeout.Token);
    await WriteRequestAsync(input, 16, "textDocument/completion", consumerUri, dependencyPosition, timeout.Token);
    using var completionBeforeDependencyClose = await ReadResponseAsync(output, 16, timeout.Token);
    var labelsBeforeDependencyClose = GetCompletionLabels(completionBeforeDependencyClose);
    Check(
        labelsBeforeDependencyClose.Contains("Bar") && !labelsBeforeDependencyClose.Contains("Foo"),
        "LSP consumer uses the unsaved dependency model before close");

    await WriteMessageAsync(input, new
    {
        jsonrpc = "2.0",
        method = "textDocument/didClose",
        @params = new { textDocument = new { uri = dependencyUri } }
    }, timeout.Token);
    await WriteRequestAsync(input, 17, "textDocument/completion", consumerUri, dependencyPosition, timeout.Token);
    using var completionAfterDependencyClose = await ReadResponseAsync(output, 17, timeout.Token);
    var labelsAfterDependencyClose = GetCompletionLabels(completionAfterDependencyClose);
    Check(
        labelsAfterDependencyClose.Contains("Foo"),
        "LSP consumer returns to the dependency model stored on disk after close");
    Check(
        !labelsAfterDependencyClose.Contains("Bar"),
        "LSP consumer drops the unsaved dependency model after close");
    Console.WriteLine("PASS closing an unsaved dependency restores its disk model over stdio");

    foreach (var uri in new[] { implementationBUri, implementationAUri })
    {
        await WriteMessageAsync(input, new
        {
            jsonrpc = "2.0",
            method = "textDocument/didClose",
            @params = new { textDocument = new { uri } }
        }, timeout.Token);
    }

    foreach (var uri in new[] { chainCUri, chainBUri, chainAUri })
    {
        await WriteMessageAsync(input, new
        {
            jsonrpc = "2.0",
            method = "textDocument/didClose",
            @params = new { textDocument = new { uri } }
        }, timeout.Token);
    }

    await WriteMessageAsync(input, new
    {
        jsonrpc = "2.0",
        method = "textDocument/didClose",
        @params = new { textDocument = new { uri = consumerUri } }
    }, timeout.Token);

    await WriteMessageAsync(input, new
    {
        jsonrpc = "2.0",
        method = "textDocument/didClose",
        @params = new { textDocument = new { uri = documentUri } }
    }, timeout.Token);

    foreach (var uri in new[] { firstSyntheticUri.AbsoluteUri, secondSyntheticUri.AbsoluteUri })
    {
        await WriteMessageAsync(input, new
        {
            jsonrpc = "2.0",
            method = "textDocument/didClose",
            @params = new { textDocument = new { uri } }
        }, timeout.Token);
    }

    await WriteMessageAsync(input, new
    {
        jsonrpc = "2.0",
        id = 26,
        method = "shutdown"
    }, timeout.Token);
    using var shutdown = await ReadResponseAsync(output, 26, timeout.Token);
    Check(shutdown.RootElement.TryGetProperty("result", out var shutdownResult) &&
          shutdownResult.ValueKind == JsonValueKind.Null,
        "shutdown returned null result");

    await WriteMessageAsync(input, new { jsonrpc = "2.0", method = "exit" }, timeout.Token);
    await process.WaitForExitAsync(timeout.Token);
    var errorOutput = await stderrTask.WaitAsync(timeout.Token);
    Check(process.ExitCode == 0, $"Language server exited cleanly. stderr: {errorOutput}");
    Console.WriteLine("PASS shutdown and exit over stdio");
    Console.WriteLine("All LSP smoke checks passed.");
}
finally
{
    if (!process.HasExited)
    {
        process.Kill(entireProcessTree: true);
        await process.WaitForExitAsync();
    }
    var capturedErrors = await stderrTask;
    if (!string.IsNullOrWhiteSpace(capturedErrors)) Console.Error.WriteLine(capturedErrors);
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

static async Task WriteDidChangeAsync(
    Stream stream,
    string documentUri,
    string text,
    int version,
    CancellationToken cancellationToken)
{
    await WriteMessageAsync(stream, new
    {
        jsonrpc = "2.0",
        method = "textDocument/didChange",
        @params = new
        {
            textDocument = new { uri = documentUri, version },
            contentChanges = new[] { new { text } }
        }
    }, cancellationToken);
}

static async Task WriteDidChangeRangesAsync(
    Stream stream,
    string documentUri,
    int version,
    IReadOnlyList<((int Line, int Character) Start, (int Line, int Character) End, string Text)> changes,
    CancellationToken cancellationToken)
{
    var contentChanges = changes.Select(change => new
    {
        range = new
        {
            start = new { line = change.Start.Line, character = change.Start.Character },
            end = new { line = change.End.Line, character = change.End.Character }
        },
        text = change.Text
    }).ToArray();

    await WriteMessageAsync(stream, new
    {
        jsonrpc = "2.0",
        method = "textDocument/didChange",
        @params = new
        {
            textDocument = new { uri = documentUri, version },
            contentChanges
        }
    }, cancellationToken);
}

static IReadOnlyList<int> FindAllOffsets(string text, string value)
{
    var offsets = new List<int>();
    for (var offset = text.IndexOf(value, StringComparison.Ordinal);
         offset >= 0;
         offset = text.IndexOf(value, offset + value.Length, StringComparison.Ordinal))
    {
        offsets.Add(offset);
    }

    return offsets;
}

static HashSet<string> GetCompletionLabels(JsonDocument response) =>
    response.RootElement.GetProperty("result").GetProperty("items")
        .EnumerateArray()
        .Select(item => item.GetProperty("label").GetString())
        .Where(label => label is not null)
        .Select(label => label!)
        .ToHashSet(StringComparer.Ordinal);

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

static string FindRepositoryRoot(string startDirectory)
{
    for (var directory = new DirectoryInfo(startDirectory); directory is not null; directory = directory.Parent)
    {
        if (Directory.Exists(Path.Combine(directory.FullName, "PascalABCNet.LanguageServer")))
            return directory.FullName;
    }

    throw new DirectoryNotFoundException("Could not locate the PascalABC.NET repository root.");
}

static void Check(bool condition, string message)
{
    if (!condition)
        throw new InvalidOperationException($"Smoke check failed: {message}");
}
