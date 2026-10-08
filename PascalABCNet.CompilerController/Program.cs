using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
#if NETFRAMEWORK
using System.Web.Script.Serialization;
#else
using System.Text.Json;
#endif

namespace PascalABCNet.CompilerController;

internal static class Program
{
    private sealed class ControllerRequest
    {
        public int id { get; set; }
        public string? command { get; set; }
        public string? fileName { get; set; }
        public string? outputDirectory { get; set; }
        public string? runtimeModule { get; set; }
        public SourceFileRequest[]? sourceFiles { get; set; }
        public bool emitEvents { get; set; }
        public bool rebuild { get; set; }
    }

    private sealed class SourceFileRequest
    {
        public string? fileName { get; set; }
        public string? text { get; set; }
    }

    private sealed class WorkerCompileRequest
    {
        public string? fileName { get; set; }
        public string? outputDirectory { get; set; }
        public string? runtimeModule { get; set; }
        public SourceFileRequest[]? sourceFiles { get; set; }
        public bool rebuild { get; set; }
    }

    private sealed class WorkerTransportRequest
    {
        public string? command { get; set; }
        public string? payload { get; set; }
        public bool emitEvents { get; set; }
    }

    private sealed class WorkerTransportResponse
    {
        public bool success { get; set; }
        public string? response { get; set; }
        public string? error { get; set; }
        public CompilerEvent? compilerEvent { get; set; }
    }

    private sealed class CompilerEvent
    {
        public string? state { get; set; }
        public string? fileName { get; set; }
        public uint linesCompiled { get; set; }
        public int errorCount { get; set; }
        public int warningCount { get; set; }
        public double elapsedMilliseconds { get; set; }
    }

    private sealed class WorkerConnection : IDisposable
    {
        public WorkerConnection(Process process)
        {
            Process = process;
            Input = new StreamWriter(
                process.StandardInput.BaseStream,
                new UTF8Encoding(false));
            Output = process.StandardOutput;
        }

        public Process Process { get; }
        public StreamWriter Input { get; }
        public StreamReader Output { get; }
        public bool IsUsable { get; set; } = true;

        public void Dispose()
        {
            Input.Dispose();
            Output.Dispose();
            Process.Dispose();
        }
    }

    private static void Log(string message)
    {
        Console.Error.WriteLine(message);
        Console.Error.Flush();
    }

    private static void WriteJson(Dictionary<string, object?> response)
    {
        Console.WriteLine(SerializeJson(response));
        Console.Out.Flush();
    }

    private static string SerializeJson(object value)
    {
#if NETFRAMEWORK
        return CreateJsonSerializer().Serialize(value);
#else
        return JsonSerializer.Serialize(value);
#endif
    }

#if NETFRAMEWORK
    private static JavaScriptSerializer CreateJsonSerializer() =>
        new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
#endif

    private static ControllerRequest DeserializeRequest(string json)
    {
#if NETFRAMEWORK
        return CreateJsonSerializer().Deserialize<ControllerRequest>(json)
               ?? throw new InvalidDataException("JSON-запрос не содержит объекта");
#else
        return JsonSerializer.Deserialize<ControllerRequest>(json)
               ?? throw new InvalidDataException("JSON-запрос не содержит объекта");
#endif
    }

    private static WorkerTransportResponse DeserializeWorkerResponse(string json)
    {
#if NETFRAMEWORK
        return CreateJsonSerializer().Deserialize<WorkerTransportResponse>(json)
               ?? throw new InvalidDataException("Worker вернул пустой JSON-ответ");
#else
        return JsonSerializer.Deserialize<WorkerTransportResponse>(json)
               ?? throw new InvalidDataException("Worker вернул пустой JSON-ответ");
#endif
    }

    private static Dictionary<string, object?> CreateResponse(int requestId, bool success)
    {
        return new Dictionary<string, object?>
        {
            ["id"] = requestId,
            ["success"] = success
        };
    }

    private static string QuoteArgument(string value) =>
        "\"" + value.Replace("\"", "\\\"") + "\"";

    private static WorkerConnection StartWorker(string workerFileName)
    {
        if (!File.Exists(workerFileName))
            throw new FileNotFoundException(
                "Не найден CompilerWorker: " + workerFileName,
                workerFileName);

        var workerIsDotNetAssembly = string.Equals(
            Path.GetExtension(workerFileName),
            ".dll",
            StringComparison.OrdinalIgnoreCase);

        var startInfo = new ProcessStartInfo
        {
            FileName = workerIsDotNetAssembly ? "dotnet" : workerFileName,
            Arguments = workerIsDotNetAssembly
                ? QuoteArgument(workerFileName)
                : "",
            WorkingDirectory = Path.GetDirectoryName(workerFileName) ??
                               AppDomain.CurrentDomain.BaseDirectory,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = new UTF8Encoding(false),
            StandardErrorEncoding = new UTF8Encoding(false)
        };

        var worker = new Process
        {
            StartInfo = startInfo,
            EnableRaisingEvents = true
        };

        worker.ErrorDataReceived += (_, eventArgs) =>
        {
            if (eventArgs.Data != null)
                Log("[worker error] " + eventArgs.Data);
        };

        if (!worker.Start())
        {
            worker.Dispose();
            throw new InvalidOperationException("Не удалось запустить CompilerWorker");
        }

        worker.BeginErrorReadLine();
        return new WorkerConnection(worker);
    }

    private static void WaitUntilReady(WorkerConnection connection)
    {
        if (TrySendWorkerRequest(
                connection,
                new WorkerTransportRequest { command = "ping" },
                TimeSpan.FromSeconds(10),
                out var response) && response == "PONG")
            return;

        throw new TimeoutException("CompilerWorker не ответил на ping за 10 секунд");
    }

    private static void StartWorkerAndConnect(
        string workerFileName,
        ref WorkerConnection? connection)
    {
        connection = StartWorker(workerFileName);
        try
        {
            WaitUntilReady(connection);
        }
        catch
        {
            var worker = connection.Process;
            if (!worker.HasExited)
                worker.Kill();
            connection.Dispose();
            connection = null;
            throw;
        }

        Log("CompilerWorker запущен, PID = " + connection.Process.Id);
    }

    private static void StopWorker(ref WorkerConnection? connection)
    {
        if (connection == null)
            return;

        var worker = connection.Process;
        if (!worker.HasExited)
        {
            if (connection.IsUsable)
            {
                TrySendWorkerRequest(
                    connection,
                    new WorkerTransportRequest { command = "shutdown" },
                    TimeSpan.FromMilliseconds(500),
                    out _);
            }

            if (!worker.WaitForExit(1500))
                worker.Kill();
        }

        connection.Dispose();
        connection = null;
    }

    private static void RestartWorker(
        string workerFileName,
        ref WorkerConnection? connection, string reason = "запрошен перезапуск")
    {
        Log("Перезапуск CompilerWorker");
        StopWorker(ref connection);
        Thread.Sleep(100);
        StartWorkerAndConnect(workerFileName, ref connection);
        Log("[CompilerRestarted]Компилятор .NET перезагружен: " + reason + ".");
    }

    private static bool WaitForTask(
        Task task,
        Process worker,
        Stopwatch stopwatch,
        TimeSpan timeout)
    {
        while (!task.Wait(50))
        {
            if (worker.HasExited || stopwatch.Elapsed >= timeout)
                return false;
        }
        return true;
    }

    private static bool TrySendWorkerRequest(
        WorkerConnection connection,
        WorkerTransportRequest request,
        TimeSpan timeout,
        out string response, Action<CompilerEvent>? progress = null)
    {
        try
        {
            if (!connection.IsUsable || connection.Process.HasExited)
                throw new EndOfStreamException("CompilerWorker уже завершён");

            var stopwatch = Stopwatch.StartNew();
            var writeTask = connection.Input.WriteLineAsync(SerializeJson(request));
            if (!WaitForTask(writeTask, connection.Process, stopwatch, timeout))
                throw new TimeoutException("Тайм-аут записи запроса CompilerWorker");
            var flushTask = connection.Input.FlushAsync();
            if (!WaitForTask(flushTask, connection.Process, stopwatch, timeout))
                throw new TimeoutException("Тайм-аут отправки запроса CompilerWorker");

            while (true)
            {
                // Progress must not reset or bypass the total request deadline.
                if (stopwatch.Elapsed >= timeout)
                    throw new TimeoutException("Тайм-аут ответа CompilerWorker");
                var readTask = connection.Output.ReadLineAsync();
                if (!WaitForTask(readTask, connection.Process, stopwatch, timeout))
                    throw new TimeoutException("Тайм-аут ответа CompilerWorker");
                var line = readTask.GetAwaiter().GetResult();
                if (line == null)
                    throw new EndOfStreamException("CompilerWorker закрыл stdout");

                var transportResponse = DeserializeWorkerResponse(line);
                if (transportResponse.compilerEvent != null && transportResponse.response == null && transportResponse.error == null)
                {
                    progress?.Invoke(transportResponse.compilerEvent);
                    continue;
                }
                if (!transportResponse.success)
                    throw new InvalidDataException(
                        transportResponse.error ?? "CompilerWorker вернул ошибку");

                response = transportResponse.response ?? "";
                return true;
            }
        }
        catch (Exception exception)
        {
            connection.IsUsable = false;
            Log("Ошибка обмена с CompilerWorker: " + exception.Message);
            response = "";
            return false;
        }
    }

    private static string SendRequest(
        string command,
        string? payload,
        string workerFileName,
        ref WorkerConnection? connection, Action<CompilerEvent, int>? progress = null)
    {
        var requestTimeout = GetWorkerRequestTimeout();
        for (var attempt = 1; attempt <= 2; attempt++)
        {
            if (connection != null && TrySendWorkerRequest(
                    connection,
                    new WorkerTransportRequest
                    {
                        command = command,
                        payload = payload,
                        emitEvents = progress != null
                    },
                    requestTimeout,
                    out var response, value => progress?.Invoke(value, attempt)))
                return response;

            if (attempt == 1)
            {
                Log("CompilerWorker не ответил. Выполняется перезапуск");
                RestartWorker(workerFileName, ref connection, "Worker завершился или не ответил вовремя");
            }
        }

        throw new InvalidOperationException("Не удалось получить ответ от CompilerWorker");
    }

    private static TimeSpan GetWorkerRequestTimeout()
    {
        const int defaultTimeoutMilliseconds = 30 * 1000;
        var value = Environment.GetEnvironmentVariable(
            "PABC_COMPILER_WORKER_REQUEST_TIMEOUT_MS");
        return int.TryParse(value, out var milliseconds) && milliseconds > 0
            ? TimeSpan.FromMilliseconds(milliseconds)
            : TimeSpan.FromMilliseconds(defaultTimeoutMilliseconds);
    }

    private static long GetWorkingSetMb(WorkerConnection? connection)
    {
        var worker = connection?.Process;
        if (worker == null || worker.HasExited)
            return 0;
        worker.Refresh();
        return worker.WorkingSet64 / (1024 * 1024);
    }

    private static void ParseCompileResponse(
        string workerResponse,
        string sourceFileName,
        Dictionary<string, object?> response)
    {
        var normalized = workerResponse.Replace("\r\n", "\n").Replace("\r", "\n");
        var lines = normalized.Split('\n');
        var diagnostics = new List<Dictionary<string, object?>>();
        response["diagnostics"] = diagnostics;

        var success = lines.Length > 0 && lines[0] == "OK";
        if (success)
        {
            response["success"] = true;
            response["outputFile"] = lines.Length > 1 ? lines[1].Trim() : "";
            response["message"] = "";
        }
        else
        {
            response["success"] = false;
            response["outputFile"] = "";
        }
        var firstLine = success ? 2 : lines.Length > 0 &&
                        (lines[0] == "ERROR" || lines[0] == "FATAL") ? 1 : 0;
        var message = new StringBuilder();
        var errorPattern = new Regex(
            @"^\s*\((\d+),\s*(\d+)\):\s*(.*)$",
            RegexOptions.CultureInvariant);

        for (var index = firstLine; index < lines.Length; index++)
        {
            var line = lines[index].Trim();
            if (line.Length == 0)
                continue;

            bool warning = line.StartsWith("WARNING\t", StringComparison.Ordinal);
            if (warning || line.StartsWith("DIAGNOSTIC\t", StringComparison.Ordinal))
            {
                var parts = line.Split('\t');
                if (parts.Length == 5 &&
                    int.TryParse(parts[2], out var diagnosticLine) &&
                    int.TryParse(parts[3], out var diagnosticColumn))
                {
                    var diagnosticFileName = DecodeBase64(parts[1]);
                    var diagnosticMessage = DecodeBase64(parts[4]);
                    diagnostics.Add(new Dictionary<string, object?>
                    {
                        ["fileName"] = diagnosticFileName,
                        ["line"] = diagnosticLine,
                        ["column"] = diagnosticColumn,
                        ["severity"] = warning ? "warning" : "error",
                        ["message"] = diagnosticMessage
                    });
                    if (!warning)
                    {
                        if (message.Length > 0)
                            message.AppendLine();
                        message.Append(diagnosticMessage);
                    }
                    continue;
                }
            }

            if (message.Length > 0)
                message.AppendLine();
            message.Append(line);

            var match = errorPattern.Match(line);
            var diagnostic = new Dictionary<string, object?>
            {
                ["fileName"] = sourceFileName,
                ["severity"] = "error"
            };
            if (match.Success)
            {
                diagnostic["line"] = int.Parse(match.Groups[1].Value);
                diagnostic["column"] = int.Parse(match.Groups[2].Value);
                diagnostic["message"] = match.Groups[3].Value;
            }
            else
            {
                diagnostic["line"] = 1;
                diagnostic["column"] = 1;
                diagnostic["message"] = line;
            }
            diagnostics.Add(diagnostic);
        }

        response["message"] = success ? "" : message.Length == 0
            ? workerResponse
            : message.ToString();
    }

    private static string DecodeBase64(string value) =>
        Encoding.UTF8.GetString(Convert.FromBase64String(value));

    private static string GetDefaultWorkerFileName()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
#if NET10_0
        var dotNetWorker = Path.Combine(baseDirectory, "PABCCompilerWorker.dll");
        if (File.Exists(dotNetWorker))
            return dotNetWorker;
#endif
        return Path.Combine(baseDirectory, "PABCCompilerWorker.exe");
    }

    private static int Main(string[] args)
    {
        Console.InputEncoding = new UTF8Encoding(false);
        Console.OutputEncoding = new UTF8Encoding(false);

        var workerFileName = Path.GetFullPath(
            args.Length >= 1 ? args[0] : GetDefaultWorkerFileName());
        var maxCompilations = args.Length >= 2 ? int.Parse(args[1]) : 0;
        var maxWorkingSetMb = args.Length >= 3 ? long.Parse(args[2]) : 0;

        WorkerConnection? connection = null;
        var compilationCount = 0;
        var running = true;

        try
        {
            StartWorkerAndConnect(workerFileName, ref connection);
            Log("PABCCompilerController готов");

            while (running)
            {
                var inputLine = Console.ReadLine();
                if (inputLine == null)
                    break;
                if (string.IsNullOrWhiteSpace(inputLine))
                    continue;

                inputLine = inputLine.TrimStart('\uFEFF');

                var requestId = 0;
                try
                {
                    var request = DeserializeRequest(inputLine);
                    requestId = request.id;
                    var command = (request.command ?? "").ToLowerInvariant();

                    switch (command)
                    {
                        case "ping":
                        {
                            var workerResponse = SendRequest(
                                "ping", null, workerFileName, ref connection);
                            var response = CreateResponse(
                                requestId, workerResponse == "PONG");
                            response["result"] = workerResponse;
                            response["workerPid"] = connection!.Process.Id;
                            response["workingSetMB"] = GetWorkingSetMb(connection);
                            WriteJson(response);
                            break;
                        }
                        case "compile":
                        {
                            var fileName = request.fileName ?? "";
                            if (fileName.Length == 0)
                            {
                                var missingFileResponse = CreateResponse(requestId, false);
                                missingFileResponse["outputFile"] = "";
                                missingFileResponse["message"] = "Не задано поле fileName";
                                WriteJson(missingFileResponse);
                                continue;
                            }

                            fileName = Path.GetFullPath(fileName);
                            var sourceFiles = request.sourceFiles;
                            if (sourceFiles != null)
                            {
                                foreach (var sourceFile in sourceFiles)
                                {
                                    if (string.IsNullOrWhiteSpace(sourceFile.fileName))
                                        throw new InvalidDataException(
                                            "В sourceFiles не задано поле fileName");
                                    if (!Path.IsPathRooted(sourceFile.fileName))
                                        throw new InvalidDataException(
                                            "sourceFiles.fileName должен быть абсолютным путём: " +
                                            sourceFile.fileName);
                                    sourceFile.fileName = Path.GetFullPath(sourceFile.fileName);
                                    sourceFile.text ??= "";
                                }
                            }
                            var outputDirectory = request.outputDirectory;
                            if (!string.IsNullOrWhiteSpace(outputDirectory))
                            {
                                outputDirectory = Path.GetFullPath(outputDirectory);
                                Directory.CreateDirectory(outputDirectory);
                            }

                            var workerRequest = SerializeJson(
                                new WorkerCompileRequest
                                {
                                    fileName = fileName,
                                    outputDirectory = outputDirectory,
                                    runtimeModule = request.runtimeModule,
                                    sourceFiles = sourceFiles,
                                    rebuild = request.rebuild
                                });
                            var workerResponse = SendRequest(
                                "compile", workerRequest, workerFileName,
                                ref connection, request.emitEvents ? (Action<CompilerEvent, int>)((value, attempt) =>
                                {
                                    WriteJson(new Dictionary<string, object?>
                                    {
                                        ["id"] = requestId, ["event"] = "compilerState", ["attempt"] = attempt,
                                        ["state"] = value.state, ["fileName"] = value.fileName,
                                        ["linesCompiled"] = value.linesCompiled, ["errorCount"] = value.errorCount,
                                        ["warningCount"] = value.warningCount,
                                        ["elapsedMilliseconds"] = value.elapsedMilliseconds
                                    });
                                }) : null);
                            compilationCount++;
                            var workingSetMb = GetWorkingSetMb(connection);
                            var response = CreateResponse(requestId, false);
                            ParseCompileResponse(workerResponse, fileName, response);
                            response["fileName"] = fileName;
                            response["compilationCount"] = compilationCount;
                            response["workerPid"] = connection!.Process.Id;
                            response["workingSetMB"] = workingSetMb;
                            WriteJson(response);

                            Log("Компиляций: " + compilationCount +
                                ", память: " + workingSetMb + " MB");

                            var restartByCount = maxCompilations > 0 &&
                                                 compilationCount >= maxCompilations;
                            var restartByMemory = maxWorkingSetMb > 0 &&
                                                  workingSetMb >= maxWorkingSetMb;
                            if (restartByCount || restartByMemory)
                            {
                                string reason = restartByMemory
                                    ? "превышен порог памяти " + maxWorkingSetMb + " МБ (" + workingSetMb + " МБ)"
                                    : "достигнут лимит " + maxCompilations + " компиляций";
                                RestartWorker(workerFileName, ref connection, reason);
                                compilationCount = 0;
                            }
                            break;
                        }
                        case "restart":
                        {
                            RestartWorker(workerFileName, ref connection);
                            compilationCount = 0;
                            var response = CreateResponse(requestId, true);
                            response["result"] = "restarted";
                            response["workerPid"] = connection!.Process.Id;
                            WriteJson(response);
                            break;
                        }
                        case "shutdown":
                        {
                            var response = CreateResponse(requestId, true);
                            response["result"] = "shutdown";
                            WriteJson(response);
                            running = false;
                            break;
                        }
                        default:
                        {
                            var response = CreateResponse(requestId, false);
                            response["message"] = "Неизвестная команда: " + command;
                            WriteJson(response);
                            break;
                        }
                    }
                }
                catch (Exception exception)
                {
                    var response = CreateResponse(requestId, false);
                    response["message"] = exception.Message;
                    response["errorType"] = exception.GetType().FullName;
                    WriteJson(response);
                    Log(exception.ToString());
                }
            }
        }
        catch (Exception exception)
        {
            Log("Критическая ошибка контроллера:");
            Log(exception.ToString());
        }
        finally
        {
            StopWorker(ref connection);
        }

        return 0;
    }
}
