using System.Diagnostics;
using System.Text;
using System.Text.Json;

var options = ParseArguments(args);
var runtimeRoot = Path.GetFullPath(options.RuntimeRoot);
var isNet10 = options.Target == "net10";
var controllerPath = Path.Combine(
    runtimeRoot,
    isNet10 ? "PABCCompilerController.dll" : "PABCCompilerController.exe");
var workerPath = Path.Combine(
    runtimeRoot,
    isNet10 ? "ZMQServerPas.dll" : "ZMQServerPas.exe");

Check(File.Exists(controllerPath), $"Controller exists: {controllerPath}");
Check(File.Exists(workerPath), $"Worker exists: {workerPath}");

var testRoot = Path.Combine(
    Path.GetTempPath(),
    "pabc-tooling-controller-" + Guid.NewGuid().ToString("N"));
var sourceRoot = Path.Combine(testRoot, "папка с пробелами");
var outputRoot = Path.Combine(testRoot, "output");
Directory.CreateDirectory(sourceRoot);
Directory.CreateDirectory(outputRoot);
var sourcePath = Path.Combine(sourceRoot, "Проверка.pas");
var workerControlPath = Path.Combine(testRoot, "worker-control.txt");

var startInfo = new ProcessStartInfo
{
    FileName = isNet10 ? ResolveDotnet() : controllerPath,
    WorkingDirectory = runtimeRoot,
    UseShellExecute = false,
    CreateNoWindow = true,
    RedirectStandardInput = true,
    RedirectStandardOutput = true,
    RedirectStandardError = true,
    StandardInputEncoding = new UTF8Encoding(false),
    StandardOutputEncoding = new UTF8Encoding(false),
    StandardErrorEncoding = new UTF8Encoding(false)
};
if (isNet10)
    startInfo.ArgumentList.Add(controllerPath);
startInfo.ArgumentList.Add(workerPath);
startInfo.ArgumentList.Add("2");
startInfo.ArgumentList.Add("0");
startInfo.Environment["PABC_COMPILER_WORKER_REQUEST_TIMEOUT_MS"] = "10000";
startInfo.Environment["PABC_COMPILER_WORKER_TEST_CONTROL_FILE"] = workerControlPath;

using var process = new Process { StartInfo = startInfo };
var stderr = new StringBuilder();
var stderrLock = new object();
var processStarted = false;
process.ErrorDataReceived += (_, eventArgs) =>
{
    if (eventArgs.Data is not null)
    {
        lock (stderrLock)
            stderr.AppendLine(eventArgs.Data);
    }
};

try
{
    processStarted = process.Start();
    Check(processStarted, "Controller process started");
    process.BeginErrorReadLine();

    var ping = await SendAsync(process, new { id = 1, command = "ping" });
    CheckSuccess(ping, "ping");
    Check(ping.GetProperty("result").GetString() == "PONG", "Worker answered PONG");
    var firstWorkerPid = ping.GetProperty("workerPid").GetInt32();

    await File.WriteAllTextAsync(
        sourcePath,
        "begin\n  Println('first compilation');\nend.\n",
        new UTF8Encoding(false));
    var firstCompile = await CompileAsync(process, 2, sourcePath, outputRoot);
    CheckSuccess(firstCompile, "first compilation");
    Check(firstCompile.GetProperty("diagnostics").GetArrayLength() == 0,
        "Successful compilation has no diagnostics");
    var firstOutput = firstCompile.GetProperty("outputFile").GetString();
    Check(!string.IsNullOrWhiteSpace(firstOutput) && File.Exists(firstOutput),
        "First compilation produced an output file");

    await File.WriteAllTextAsync(
        sourcePath,
        "begin\n  Println('second compilation');\nend.\n",
        new UTF8Encoding(false));
    var secondCompile = await CompileAsync(process, 3, sourcePath, outputRoot);
    CheckSuccess(secondCompile, "second compilation in the same controller");

    var pingAfterRestart = await SendAsync(process, new { id = 4, command = "ping" });
    CheckSuccess(pingAfterRestart, "ping after automatic restart");
    Check(pingAfterRestart.GetProperty("workerPid").GetInt32() != firstWorkerPid,
        "Worker restarted after the configured compilation limit");

    await File.WriteAllTextAsync(
        sourcePath,
        "begin\n  this is not valid Pascal\nend.\n",
        new UTF8Encoding(false));
    var invalidCompile = await CompileAsync(process, 5, sourcePath, outputRoot);
    Check(!invalidCompile.GetProperty("success").GetBoolean(),
        "Invalid source was rejected");
    Check(invalidCompile.GetProperty("diagnostics").GetArrayLength() > 0,
        "Invalid source returned diagnostics");

    await File.WriteAllTextAsync(
        sourcePath,
        "begin\n  Println('recovered');\nend.\n",
        new UTF8Encoding(false));
    var recoveredCompile = await CompileAsync(process, 6, sourcePath, outputRoot);
    CheckSuccess(recoveredCompile, "compilation after an error");

    await CheckSourceSnapshotsAsync(
        process, sourceRoot, outputRoot, isNet10, firstRequestId: 7);

    await File.WriteAllTextAsync(
        sourcePath,
        "begin\n  Write('Без перевода строки: ');\n  var value: string;\n" +
        "  Readln(value);\n  Writeln('Ответ: ', value);\nend.\n",
        new UTF8Encoding(false));
    var redirectedCompile = await CompileAsync(
        process, 20, sourcePath, outputRoot, "__RedirectIOMode",
        new[]
        {
            new SourceFileInput(
                sourcePath,
                "begin\n  Write('Без перевода строки: ');\n" +
                "  var value: string;\n  Readln(value);\n" +
                "  Writeln('Ответ: ', value);\nend.\n")
        });
    CheckSuccess(redirectedCompile, "compilation with __RedirectIOMode");
    var redirectedOutput = redirectedCompile.GetProperty("outputFile").GetString();
    Check(!string.IsNullOrWhiteSpace(redirectedOutput) && File.Exists(redirectedOutput),
        "Redirected-I/O compilation produced an output file");

    if (isNet10)
    {
        await CheckRedirectedInputOutputAsync(redirectedOutput!, sourceRoot);

        await File.WriteAllTextAsync(
            sourcePath,
            "begin\n  raise new Exception('Тестовая ошибка выполнения');\nend.\n",
            new UTF8Encoding(false));
        var exceptionCompile = await CompileAsync(
            process, 21, sourcePath, outputRoot, "__RedirectIOMode");
        CheckSuccess(exceptionCompile, "exception sample with __RedirectIOMode");
        var exceptionOutput = exceptionCompile.GetProperty("outputFile").GetString();
        Check(!string.IsNullOrWhiteSpace(exceptionOutput) && File.Exists(exceptionOutput),
            "Exception sample produced an output file");
        await CheckRedirectedExceptionAsync(exceptionOutput!, sourceRoot);
    }

    var largeComment = new string('Ж', 1_000_000);
    var largeSource = "begin\n  Println('Большой кириллический запрос');\nend.\n//" +
                      largeComment;
    var largeCompile = await CompileAsync(
        process, 23, sourcePath, outputRoot,
        sourceFiles: new[] { new SourceFileInput(sourcePath, largeSource) });
    CheckSuccess(largeCompile, "large Cyrillic request");

    const string stderrStart = "WORKER_STDERR_НАЧАЛО_";
    const string stderrEnd = "_WORKER_STDERR_КОНЕЦ";
    var stderrMessage = stderrStart + new string('Я', 128_000) + stderrEnd;
    await File.WriteAllTextAsync(
        workerControlPath,
        "stderr-base64:" + Convert.ToBase64String(Encoding.UTF8.GetBytes(stderrMessage)),
        new UTF8Encoding(false));
    var stderrPing = await SendAsync(process, new { id = 24, command = "ping" });
    CheckSuccess(stderrPing, "ping while worker writes stderr");
    await WaitForTextAsync(stderr, stderrLock, stderrStart, stderrEnd);
    Check(true, "worker stderr did not corrupt controller JSON Lines");

    var crashedWorkerPid = stderrPing.GetProperty("workerPid").GetInt32();
    using (var crashedWorker = Process.GetProcessById(crashedWorkerPid))
    {
        crashedWorker.Kill(entireProcessTree: true);
        Check(crashedWorker.WaitForExit(10_000), "crashed worker process exited");
    }
    var pingAfterCrash = await SendAsync(process, new { id = 25, command = "ping" });
    CheckSuccess(pingAfterCrash, "automatic restart after worker crash");
    Check(pingAfterCrash.GetProperty("workerPid").GetInt32() != crashedWorkerPid,
        "worker PID changed after crash");

    var hungWorkerPid = pingAfterCrash.GetProperty("workerPid").GetInt32();
    await File.WriteAllTextAsync(
        workerControlPath, "hang:30000", new UTF8Encoding(false));
    var hangStopwatch = Stopwatch.StartNew();
    var pingAfterHang = await SendAsync(process, new { id = 26, command = "ping" });
    hangStopwatch.Stop();
    CheckSuccess(pingAfterHang, "automatic restart after worker timeout");
    Check(pingAfterHang.GetProperty("workerPid").GetInt32() != hungWorkerPid,
        "worker PID changed after timeout");
    Check(hangStopwatch.Elapsed < TimeSpan.FromSeconds(25),
        "worker timeout remained bounded");

    var finalWorkerPid = pingAfterHang.GetProperty("workerPid").GetInt32();

    var shutdown = await SendAsync(process, new { id = 27, command = "shutdown" });
    CheckSuccess(shutdown, "shutdown");
    Check(shutdown.GetProperty("result").GetString() == "shutdown",
        "Controller acknowledged shutdown");
    Check(process.WaitForExit(10_000), "Controller exited after shutdown");
    Check(process.ExitCode == 0, "Controller exit code is zero");
    Check(!IsProcessRunning(finalWorkerPid), "Worker exited with Controller");

    Console.WriteLine($"All {options.Target} compiler-controller smoke checks passed.");
}
catch (Exception exception)
{
    Console.Error.WriteLine(exception);
    lock (stderrLock)
    {
        if (stderr.Length > 0)
        {
            Console.Error.WriteLine("Controller stderr:");
            Console.Error.WriteLine(stderr);
        }
    }
    Environment.ExitCode = 1;
}
finally
{
    if (processStarted && !process.HasExited)
    {
        process.Kill(entireProcessTree: true);
        process.WaitForExit();
    }

    try
    {
        Directory.Delete(testRoot, recursive: true);
    }
    catch (IOException)
    {
    }
    catch (UnauthorizedAccessException)
    {
    }
}

static async Task<JsonElement> CompileAsync(
    Process process,
    int id,
    string sourcePath,
    string outputRoot,
    string? runtimeModule = null,
    IReadOnlyList<SourceFileInput>? sourceFiles = null)
{
    var request = new Dictionary<string, object?>
    {
        ["id"] = id,
        ["command"] = "compile",
        ["fileName"] = sourcePath,
        ["outputDirectory"] = outputRoot
    };
    if (runtimeModule is not null)
        request["runtimeModule"] = runtimeModule;
    if (sourceFiles is not null)
        request["sourceFiles"] = sourceFiles;
    return await SendAsync(process, request);
}

static async Task CheckSourceSnapshotsAsync(
    Process process,
    string sourceRoot,
    string outputRoot,
    bool isNet10,
    int firstRequestId)
{
    var mainPath = Path.Combine(sourceRoot, "SnapshotMain.pas");
    await File.WriteAllTextAsync(
        mainPath, "begin Println('old disk main') end.", new UTF8Encoding(false));
    var changedMain = await CompileAsync(
        process, firstRequestId, mainPath, outputRoot, sourceFiles: new[]
        {
            new SourceFileInput(mainPath, "begin Println('new snapshot main') end.")
        });
    CheckSuccess(changedMain, "changed main source from snapshot");
    Check(await RunAndCaptureAsync(
            changedMain.GetProperty("outputFile").GetString()!, sourceRoot, isNet10) ==
          "new snapshot main",
        "snapshot text overrides older main file on disk");

    var virtualMainPath = Path.Combine(sourceRoot, "VirtualMain.pas");
    var virtualMain = await CompileAsync(
        process, firstRequestId + 1, virtualMainPath, outputRoot,
        sourceFiles: new[]
        {
            new SourceFileInput(
                virtualMainPath, "begin Println('virtual main') end.")
        });
    CheckSuccess(virtualMain, "fully virtual main source");
    Check(await RunAndCaptureAsync(
            virtualMain.GetProperty("outputFile").GetString()!, sourceRoot, isNet10) ==
          "virtual main",
        "fully virtual main produced the snapshot program");

    var diskUnitPath = Path.Combine(sourceRoot, "DiskSnapshotUnit.pas");
    var virtualUnitPath = Path.Combine(sourceRoot, "VirtualSnapshotUnit.pas");
    await File.WriteAllTextAsync(
        diskUnitPath,
        "unit DiskSnapshotUnit; interface function DiskValue: integer; " +
        "implementation function DiskValue := 3; end.",
        new UTF8Encoding(false));
    await File.WriteAllTextAsync(
        mainPath,
        "uses DiskSnapshotUnit, VirtualSnapshotUnit; " +
        "begin var result := DiskValue + VirtualValue end.",
        new UTF8Encoding(false));

    var mixedCompile = await CompileAsync(
        process, firstRequestId + 2, mainPath, outputRoot,
        sourceFiles: new[]
        {
            new SourceFileInput(
                virtualUnitPath,
                "unit VirtualSnapshotUnit; interface function VirtualValue: integer; " +
                "implementation function VirtualValue := 7; end.")
        });
    CheckSuccess(mixedCompile, "mixed virtual and on-disk modules");
    Check(true, "virtual module and disk module were compiled together");

    await File.WriteAllTextAsync(
        diskUnitPath,
        "unit DiskSnapshotUnit; interface function DiskValue: integer; " +
        "implementation function DiskValue := 1; end.",
        new UTF8Encoding(false));
    await File.WriteAllTextAsync(
        mainPath,
        "uses DiskSnapshotUnit; " +
        "begin var result := DiskValue end.",
        new UTF8Encoding(false));

    var diskPcuCompile = await CompileAsync(
        process, firstRequestId + 3, mainPath, outputRoot);
    CheckSuccess(diskPcuCompile, "baseline disk module before snapshots");

    foreach (var testCase in new[] { (Id: firstRequestId + 4, Value: 11), (Id: firstRequestId + 5, Value: 22) })
    {
        var snapshotFunction = "SnapshotValue" + testCase.Value;
        var compile = await CompileAsync(
            process, testCase.Id, mainPath, outputRoot,
            sourceFiles: new[]
            {
                new SourceFileInput(
                    mainPath,
                    "uses DiskSnapshotUnit; " +
                    $"begin var result := {snapshotFunction} end."),
                new SourceFileInput(
                    diskUnitPath,
                    "unit DiskSnapshotUnit; interface " +
                    $"function {snapshotFunction}: integer; implementation " +
                    $"function {snapshotFunction} := {testCase.Value}; end.")
            });
        CheckSuccess(compile, $"module snapshot value {testCase.Value}");
        Check(true, "new module snapshot overrides disk and stale PCU");
    }

    var diskFallback = await CompileAsync(
        process, firstRequestId + 6, mainPath, outputRoot);
    CheckSuccess(diskFallback, "request after snapshot without sourceFiles");
    Check(true, "snapshot does not leak into the next request");

    var invalidUnitPath = Path.Combine(sourceRoot, "InvalidSnapshotUnit.pas");
    await File.WriteAllTextAsync(
        mainPath,
        "uses InvalidSnapshotUnit; begin end.",
        new UTF8Encoding(false));
    var invalidModule = await CompileAsync(
        process, firstRequestId + 7, mainPath, outputRoot,
        sourceFiles: new[]
        {
            new SourceFileInput(
                invalidUnitPath,
                "unit InvalidSnapshotUnit;\ninterface\nprocedure Broken;\n" +
                "implementation\nprocedure Broken;\nbegin\n  this is invalid\nend;\nend.")
        });
    Check(!invalidModule.GetProperty("success").GetBoolean(),
        "error in virtual module was rejected");
    var diagnostic = invalidModule.GetProperty("diagnostics")[0];
    Check(Path.GetFullPath(diagnostic.GetProperty("fileName").GetString()!) ==
          Path.GetFullPath(invalidUnitPath),
        "diagnostic identifies the virtual module");
    Check(diagnostic.GetProperty("line").GetInt32() >= 6 &&
          diagnostic.GetProperty("column").GetInt32() >= 1,
        "diagnostic contains virtual-module coordinates");
}

static async Task<string> RunAndCaptureAsync(
    string outputFile,
    string workingDirectory,
    bool isNet10)
{
    var startInfo = new ProcessStartInfo
    {
        FileName = isNet10 ? ResolveDotnet() : outputFile,
        WorkingDirectory = workingDirectory,
        UseShellExecute = false,
        CreateNoWindow = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        StandardOutputEncoding = new UTF8Encoding(false),
        StandardErrorEncoding = new UTF8Encoding(false)
    };
    if (isNet10)
        startInfo.ArgumentList.Add(outputFile);
    using var program = Process.Start(startInfo) ??
                        throw new InvalidOperationException("Could not start compiled program.");
    var stdoutTask = program.StandardOutput.ReadToEndAsync();
    var stderrTask = program.StandardError.ReadToEndAsync();
    await program.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(10));
    var stdout = await stdoutTask;
    var stderr = await stderrTask;
    Check(program.ExitCode == 0, "compiled snapshot program exited successfully: " + stderr);
    return stdout.Trim();
}

static async Task CheckRedirectedInputOutputAsync(
    string outputFile,
    string workingDirectory)
{
    using var program = StartRedirectedProgram(outputFile, workingDirectory);
    try
    {
        var stdout = new StringBuilder();
        var stderr = new StringBuilder();
        var outputSeen = new TaskCompletionSource<bool>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var readSignalSeen = new TaskCompletionSource<bool>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var stdoutTask = CaptureAsync(
            program.StandardOutput,
            stdout,
            "Без перевода строки: ",
            outputSeen);
        var stderrTask = CaptureAsync(
            program.StandardError,
            stderr,
            "[READLNSIGNAL]",
            readSignalSeen);

        await program.StandardInput.WriteLineAsync("GO");
        await program.StandardInput.FlushAsync();
        await outputSeen.Task.WaitAsync(TimeSpan.FromSeconds(10));
        await readSignalSeen.Task.WaitAsync(TimeSpan.FromSeconds(10));
        Check(true, "redirected output without a newline arrived before input");
        Check(true, "runtime emitted [READLNSIGNAL]");

        await program.StandardInput.WriteLineAsync("Привет из теста");
        await program.StandardInput.FlushAsync();
        await program.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(10));
        await Task.WhenAll(stdoutTask, stderrTask);

        Check(program.ExitCode == 0, "redirected-I/O sample exited successfully");
        Check(stdout.ToString().Contains("Ответ: Привет из теста", StringComparison.Ordinal),
            "redirected stdin and Cyrillic stdout round-trip");
    }
    finally
    {
        StopTestProcess(program);
    }
}

static async Task CheckRedirectedExceptionAsync(
    string outputFile,
    string workingDirectory)
{
    using var program = StartRedirectedProgram(outputFile, workingDirectory);
    try
    {
        var stdoutTask = program.StandardOutput.ReadToEndAsync();
        var stderrTask = program.StandardError.ReadToEndAsync();

        await program.StandardInput.WriteLineAsync("GO");
        await program.StandardInput.FlushAsync();
        await program.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(10));
        var stderr = await stderrTask;
        await stdoutTask;

        Check(stderr.Contains("[EXCEPTION]", StringComparison.Ordinal),
            "runtime emitted [EXCEPTION]");
        Check(stderr.Contains("[MESSAGE]Тестовая ошибка выполнения", StringComparison.Ordinal),
            "runtime exception preserved its Cyrillic message");
        Check(stderr.Contains("[STACK]", StringComparison.Ordinal) &&
              stderr.Contains("[END]", StringComparison.Ordinal),
            "runtime exception included stack and end markers");
    }
    finally
    {
        StopTestProcess(program);
    }
}

static void StopTestProcess(Process process)
{
    if (process.HasExited)
        return;
    process.Kill(entireProcessTree: true);
    process.WaitForExit();
}

static Process StartRedirectedProgram(string outputFile, string workingDirectory)
{
    var startInfo = new ProcessStartInfo
    {
        FileName = ResolveDotnet(),
        WorkingDirectory = workingDirectory,
        UseShellExecute = false,
        CreateNoWindow = true,
        RedirectStandardInput = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        StandardInputEncoding = new UTF8Encoding(false),
        StandardOutputEncoding = new UTF8Encoding(false),
        StandardErrorEncoding = new UTF8Encoding(false)
    };
    startInfo.ArgumentList.Add(outputFile);
    startInfo.ArgumentList.Add("[REDIRECTIOMODE]");

    var process = new Process { StartInfo = startInfo };
    if (!process.Start())
    {
        process.Dispose();
        throw new InvalidOperationException("Could not start compiled .NET 10 program.");
    }
    return process;
}

static async Task CaptureAsync(
    StreamReader reader,
    StringBuilder destination,
    string marker,
    TaskCompletionSource<bool> markerSeen)
{
    var buffer = new char[256];
    while (true)
    {
        var count = await reader.ReadAsync(buffer);
        if (count == 0)
            break;
        destination.Append(buffer, 0, count);
        if (destination.ToString().Contains(marker, StringComparison.Ordinal))
            markerSeen.TrySetResult(true);
    }
}

static async Task<JsonElement> SendAsync(Process process, object request)
{
    var json = JsonSerializer.Serialize(request);
    await process.StandardInput.WriteLineAsync(json);
    await process.StandardInput.FlushAsync();

    using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(60));
    var line = await process.StandardOutput.ReadLineAsync(timeout.Token);
    if (line is null)
        throw new EndOfStreamException("Controller closed stdout before returning JSON.");

    using var document = JsonDocument.Parse(line);
    return document.RootElement.Clone();
}

static async Task WaitForTextAsync(
    StringBuilder text,
    object syncRoot,
    string firstMarker,
    string secondMarker)
{
    var finishTime = Stopwatch.GetTimestamp() + Stopwatch.Frequency * 10;
    while (Stopwatch.GetTimestamp() < finishTime)
    {
        lock (syncRoot)
        {
            var value = text.ToString();
            if (value.Contains(firstMarker, StringComparison.Ordinal) &&
                value.Contains(secondMarker, StringComparison.Ordinal))
                return;
        }
        await Task.Delay(25);
    }
    throw new TimeoutException("Worker stderr markers were not received.");
}

static bool IsProcessRunning(int processId)
{
    try
    {
        using var process = Process.GetProcessById(processId);
        return !process.HasExited;
    }
    catch (ArgumentException)
    {
        return false;
    }
}

static void CheckSuccess(JsonElement response, string operation)
{
    Check(response.GetProperty("success").GetBoolean(),
        $"{operation} succeeded: {response}");
}

static string ResolveDotnet()
{
    var hostPath = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH");
    return string.IsNullOrWhiteSpace(hostPath) ? "dotnet" : hostPath;
}

static (string RuntimeRoot, string Target) ParseArguments(string[] arguments)
{
    string? runtimeRoot = null;
    string? target = null;
    for (var index = 0; index < arguments.Length; index++)
    {
        switch (arguments[index])
        {
            case "--runtime" when index + 1 < arguments.Length:
                runtimeRoot = arguments[++index];
                break;
            case "--target" when index + 1 < arguments.Length:
                target = arguments[++index];
                break;
            default:
                throw new ArgumentException($"Unknown or incomplete argument: {arguments[index]}");
        }
    }

    if (string.IsNullOrWhiteSpace(runtimeRoot))
        throw new ArgumentException("--runtime is required.");
    if (target is not ("net10" or "net-framework"))
        throw new ArgumentException("--target must be net10 or net-framework.");
    return (runtimeRoot, target);
}

static void Check(bool condition, string message)
{
    if (!condition)
        throw new InvalidOperationException("Smoke check failed: " + message);
    Console.WriteLine("PASS " + message);
}

internal sealed record SourceFileInput(string fileName, string text);
