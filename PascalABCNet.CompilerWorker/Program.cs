using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Text;
using System.Threading;
using Languages.Integration;
using PascalABCCompiler;
using PascalABCCompiler.Errors;
#if !NETFRAMEWORK
using PascalABCNet.PackageServices;
#endif
#if NETFRAMEWORK
using System.Web.Script.Serialization;
#else
using System.Text.Json;
#endif

namespace PascalABCNet.CompilerWorker;

internal static class Program
{
    private static string? defaultLanguageIso;
    private sealed class CompileRequest
    {
        public string? fileName { get; set; }
        public string? outputDirectory { get; set; }
        public string? libraryOutputDirectory { get; set; }
        public string? runtimeModule { get; set; }
        public string? language { get; set; }
        public SourceFileRequest[]? sourceFiles { get; set; }
        public bool rebuild { get; set; }
        public bool debug { get; set; }
        public bool publish { get; set; }
    }

    private sealed class SourceFileRequest
    {
        public string? fileName { get; set; }
        public string? text { get; set; }
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
        public string? nuGetOperationId { get; set; }
        public string? nuGetStage { get; set; }
        public int nuGetInstalledCount { get; set; }
    }

    private sealed class SourceFileSnapshot
    {
        private readonly Dictionary<string, string> files;

        public SourceFileSnapshot(SourceFileRequest[] sourceFiles)
        {
            files = new Dictionary<string, string>(PathComparer);
            foreach (var sourceFile in sourceFiles)
            {
                if (string.IsNullOrWhiteSpace(sourceFile.fileName))
                    throw new InvalidDataException(
                        "В sourceFiles не задано поле fileName");
                if (!Path.IsPathRooted(sourceFile.fileName))
                    throw new InvalidDataException(
                        "sourceFiles.fileName должен быть абсолютным путём: " +
                        sourceFile.fileName);
                files[NormalizePath(sourceFile.fileName!)] = sourceFile.text ?? "";
            }
        }

        public IEnumerable<string> FileNames => files.Keys;

        public bool Contains(string fileName) =>
            files.ContainsKey(NormalizePath(fileName));

        public IEnumerable<string> GetChangedDiskFiles()
        {
            foreach (var file in files)
            {
                // Virtual modules still produce standalone PCU artifacts. Only
                // an existing disk source has a conflicting cache version.
                if (File.Exists(file.Key) && !string.Equals(file.Value,
                        PascalABCCompiler.CoreUtils.SourceFilesProviders.DefaultSourceFilesProvider(
                            file.Key, PascalABCCompiler.CoreUtils.SourceFileOperation.GetText) as string,
                        StringComparison.Ordinal))
                    yield return file.Key;
            }
        }

        public object? Provide(string fileName, PascalABCCompiler.CoreUtils.SourceFileOperation operation)
        {
            var normalized = NormalizePath(fileName);
            if (files.TryGetValue(normalized, out var text))
            {
                switch (operation)
                {
                    case PascalABCCompiler.CoreUtils.SourceFileOperation.GetText:
                        return text;
                    case PascalABCCompiler.CoreUtils.SourceFileOperation.Exists:
                        return true;
                    case PascalABCCompiler.CoreUtils.SourceFileOperation.GetLastWriteTime:
                        return DateTime.MaxValue;
                    case PascalABCCompiler.CoreUtils.SourceFileOperation.FileEncoding:
                        return new UTF8Encoding(false);
                }
            }

            return PascalABCCompiler.CoreUtils.SourceFilesProviders
                .DefaultSourceFilesProvider(normalized, operation);
        }

        private static StringComparer PathComparer =>
            Path.DirectorySeparatorChar == '\\'
                ? StringComparer.OrdinalIgnoreCase
                : StringComparer.Ordinal;

        private static string NormalizePath(string fileName) =>
            Path.GetFullPath(fileName);
    }

    private sealed class SnapshotPcuFreshness
    {
        private static StringComparer PathComparer => Path.DirectorySeparatorChar == '\\'
            ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        private readonly HashSet<string> changedUnits;
        private readonly Dictionary<string, string> sourceFiles = new(PathComparer);
        private readonly Dictionary<string, HashSet<string>> dependencies = new(PathComparer);
        private readonly HashSet<string> writtenPcus = new(PathComparer);

        public SnapshotPcuFreshness(SourceFileSnapshot snapshot)
        {
            changedUnits = new HashSet<string>(PathComparer);
            foreach (var file in snapshot.GetChangedDiskFiles())
                changedUnits.Add(UnitKey(file));
        }

        private static string UnitKey(string fileName) =>
            Path.ChangeExtension(Path.GetFullPath(fileName), null);

        public void Observe(ICompiler compiler, CompilerState state, string? fileName)
        {
            if (changedUnits.Count == 0 || string.IsNullOrWhiteSpace(fileName)) return;
            if (state == CompilerState.SavePCUFile)
            {
                writtenPcus.Add(Path.GetFullPath(fileName!));
                return;
            }
            if (state != CompilerState.EndCompileFile) return;
            var key = UnitKey(fileName!);
            var unit = compiler.UnitTable[key];
            if (unit == null) return;
            sourceFiles[key] = Path.GetFullPath(fileName!);
            var usedUnits = new HashSet<string>(PathComparer);
            unit.ForEachDirectCompilationUnit((used, _) =>
            {
                if (!string.IsNullOrWhiteSpace(used.UnitFileName)) usedUnits.Add(UnitKey(used.UnitFileName));
                return true;
            });
            dependencies[key] = usedUnits;
        }

        public void MarkDiskCachesStale()
        {
            // Also invalidate PCUs whose own source is unchanged but whose
            // declarations/implementation were built against a changed module.
            var affected = new HashSet<string>(changedUnits, PathComparer);
            bool added;
            do
            {
                added = false;
                foreach (var pair in dependencies)
                    if (!affected.Contains(pair.Key) && pair.Value.Overlaps(affected))
                        added |= affected.Add(pair.Key);
            } while (added);

            foreach (var pcu in writtenPcus)
            {
                var key = UnitKey(pcu);
                if (!affected.Contains(key) || !sourceFiles.TryGetValue(key, out var source) ||
                    !File.Exists(source) || !File.Exists(pcu)) continue;
                // SavePCU remains enabled. Preserve the compiled artifact, but
                // make the normal source-newer-than-PCU check reject it when
                // the editor snapshot is no longer supplied (also after restart).
                File.SetLastWriteTimeUtc(pcu, File.GetLastWriteTimeUtc(source).AddSeconds(-2));
            }
        }
    }

    private static CompileRequest DeserializeCompileRequest(string json)
    {
#if NETFRAMEWORK
        return CreateJsonSerializer().Deserialize<CompileRequest>(json)
               ?? throw new InvalidDataException("JSON-запрос не содержит объекта");
#else
        return JsonSerializer.Deserialize<CompileRequest>(json)
               ?? throw new InvalidDataException("JSON-запрос не содержит объекта");
#endif
    }

    private static WorkerTransportRequest DeserializeTransportRequest(string json)
    {
#if NETFRAMEWORK
        return CreateJsonSerializer().Deserialize<WorkerTransportRequest>(json)
               ?? throw new InvalidDataException("JSON-запрос не содержит объекта");
#else
        return JsonSerializer.Deserialize<WorkerTransportRequest>(json)
               ?? throw new InvalidDataException("JSON-запрос не содержит объекта");
#endif
    }

    private static string SerializeTransportResponse(WorkerTransportResponse response)
    {
#if NETFRAMEWORK
        return CreateJsonSerializer().Serialize(response);
#else
        return JsonSerializer.Serialize(response);
#endif
    }

#if NETFRAMEWORK
    private static JavaScriptSerializer CreateJsonSerializer() =>
        new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
#endif

    private static string CompileFile(Compiler compiler, string requestJson, Action<CompilerEvent> progress)
    {
        var response = new StringBuilder();

        try
        {
            var request = DeserializeCompileRequest(requestJson);
            ApplyDiagnosticLanguage(request.language);
            var fileName = request.fileName ?? "";
            var fullFileName = Path.GetFullPath(fileName);
            var snapshot = request.sourceFiles != null
                ? new SourceFileSnapshot(request.sourceFiles)
                : null;
            if (!File.Exists(fullFileName) &&
                (snapshot == null || !snapshot.Contains(fullFileName)))
            {
                response.AppendLine("ERROR");
                response.AppendLine(string.Format(StringResources.Get("COMPILATIONERROR_SOURCE_FILE_{0}_NOT_FOUND"), fullFileName));
                return response.ToString();
            }

            var options = new CompilerOptions(
                fullFileName,
                CompilerOptions.OutputType.ConsoleApplicaton)
            {
#if NETFRAMEWORK
                UseDllForSystemUnits = true,
#else
                UseDllForSystemUnits = false,
#endif
                Debug = request.debug,
                Rebuild = request.rebuild,
                ForDebugging = false
            };

            if (!string.IsNullOrWhiteSpace(request.outputDirectory))
            {
                var outputDirectory = Path.GetFullPath(request.outputDirectory);
#if NETFRAMEWORK
                Directory.CreateDirectory(outputDirectory);
#endif
                options.OutputDirectory = outputDirectory;
            }
#if !NETFRAMEWORK
            if (!string.IsNullOrWhiteSpace(request.libraryOutputDirectory))
                options.LibraryOutputDirectory = Path.GetFullPath(request.libraryOutputDirectory);
#endif

            var runtimeModule = request.runtimeModule;
            if (!string.IsNullOrWhiteSpace(runtimeModule))
            {
                foreach (var languageModules in options.StandardModules)
                {
                    languageModules.Value.Add(new CompilerOptions.StandardModule(
                        runtimeModule!.Trim(),
                        CompilerOptions.StandardModuleAddMethod.RightToMain,
                        languageModules.Key));
                }
            }

            var requestCompiler = compiler;
            var snapshotPcus = snapshot != null ? new SnapshotPcuFreshness(snapshot) : null;
            if (snapshot != null)
            {
                requestCompiler = new Compiler();
                requestCompiler.SourceFilesProvider = snapshot.Provide;
                requestCompiler.OnChangeCompilerState += (_, state, currentFileName) =>
                {
                    if (state == CompilerState.BeginCompileFile)
                        RegisterSnapshotPaths(
                            requestCompiler, snapshot, currentFileName);
                };
            }

            var clock = new Stopwatch();
            ChangeCompilerStateEventDelegate handler = (sender, state, currentFileName) =>
            {
                snapshotPcus?.Observe(sender, state, currentFileName);
#if !NETFRAMEWORK
                // Per-DLL UI round trips are expensive even when assemblies are cached.
                // Keep DLL processing, but suppress these verbose progress messages.
                if (state == CompilerState.ReadDLL) return;
#endif
                if (state == CompilerState.CompilationStarting) clock.Restart();
                if (state == CompilerState.CompilationFinished) clock.Stop();
                progress(new CompilerEvent
                {
                    state = state.ToString(), fileName = currentFileName,
                    linesCompiled = sender.LinesCompiled, errorCount = sender.ErrorsList.Count,
                    warningCount = sender.Warnings.Count, elapsedMilliseconds = clock.Elapsed.TotalMilliseconds
                });
            };
            string? outputFileName;
            requestCompiler.OnChangeCompilerState += handler;
#if !NETFRAMEWORK
            Action<PackageRestoreProgress> packageProgress = value => progress(new CompilerEvent {
                state = "NuGetRestoreProgress", fileName = value.PackageId,
                nuGetOperationId = value.OperationId, nuGetStage = value.Stage,
                nuGetInstalledCount = value.InstalledCount, elapsedMilliseconds = value.ElapsedMilliseconds
            });
            PackageRestoreService.RestoreProgress += packageProgress;
#endif
            try
            {
                requestCompiler.Reload();
                outputFileName = requestCompiler.Compile(options);
            }
            finally
            {
                requestCompiler.OnChangeCompilerState -= handler;
#if !NETFRAMEWORK
                PackageRestoreService.RestoreProgress -= packageProgress;
#endif
                snapshotPcus?.MarkDiskCachesStale();
            }

            if (outputFileName != null)
            {
#if NETFRAMEWORK
                if (request.publish)
                    throw new NotSupportedException("Publishing is supported only by the .NET 10 worker.");
#else
                if (request.publish)
                {
                    if (!string.Equals(Path.GetExtension(outputFileName), ".exe", StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException("Only a compiled .NET 10 program can be published.");
                    outputFileName = PackagePublisher.Publish(outputFileName, fullFileName, AppContext.BaseDirectory);
                }
#endif
                response.AppendLine("OK");
                response.AppendLine(outputFileName);
            }
            else
            {
                response.AppendLine("ERROR");
                if (requestCompiler.ErrorsList.Count == 0)
                {
                    response.AppendLine("Компилятор не создал выходной файл");
                }
                else
                {
                    foreach (var error in requestCompiler.ErrorsList)
                        AppendDiagnostic(response, error, fullFileName);
                }
            }
            foreach (var warning in requestCompiler.Warnings)
                AppendDiagnostic(response, warning, fullFileName, warning: true);
        }
        catch (Exception exception)
        {
            response.Clear();
            response.AppendLine("FATAL");
            response.AppendLine(exception.ToString());
        }

        return response.ToString();
    }

    private static void RegisterSnapshotPaths(
        Compiler compiler,
        SourceFileSnapshot snapshot,
        string? currentFileName)
    {
        var currentDirectory = string.IsNullOrWhiteSpace(currentFileName)
            ? compiler.CompilerOptions.SourceFileDirectory
            : Path.GetDirectoryName(Path.GetFullPath(currentFileName));
        var normalizedCurrentDirectory = currentDirectory?.ToLowerInvariant();

        foreach (var snapshotFileName in snapshot.FileNames)
        {
            var names = new List<string>
            {
                Path.GetFileNameWithoutExtension(snapshotFileName),
                Path.GetFileName(snapshotFileName),
                Path.ChangeExtension(snapshotFileName, null),
                snapshotFileName
            };

            if (currentDirectory != null)
            {
                var relativeName = MakeRelativePath(currentDirectory, snapshotFileName);
                names.Add(relativeName);
                names.Add(Path.ChangeExtension(relativeName, null));
            }

            foreach (var name in names)
            {
                var key = Tuple.Create(
                    name.ToLowerInvariant(), normalizedCurrentDirectory);
                if (!compiler.SourceFileNamesDictionary.ContainsKey(key))
                {
                    compiler.SourceFileNamesDictionary[key] = Tuple.Create(
                        snapshotFileName, 0);
                }
            }
        }
    }

    private static string MakeRelativePath(string directory, string fileName)
    {
        var directoryUri = new Uri(AppendDirectorySeparator(directory));
        var fileUri = new Uri(fileName);
        if (!string.Equals(
                directoryUri.Scheme, fileUri.Scheme,
                StringComparison.OrdinalIgnoreCase))
            return fileName;
        return Uri.UnescapeDataString(
                directoryUri.MakeRelativeUri(fileUri).ToString())
            .Replace('/', Path.DirectorySeparatorChar);
    }

    private static string AppendDirectorySeparator(string directory) =>
        directory.EndsWith(Path.DirectorySeparatorChar.ToString(),
            StringComparison.Ordinal)
            ? directory
            : directory + Path.DirectorySeparatorChar;

    private static void AppendDiagnostic(
        StringBuilder response,
        object error,
        string defaultFileName,
        bool warning = false)
    {
        var locatedError = error as LocatedError;
        var location = locatedError?.SourceLocation;
        var fileName = location?.FileName ?? locatedError?.FileName ?? defaultFileName;
        var line = location?.BeginPosition.Line ?? 1;
        var column = location?.BeginPosition.Column ?? 1;
        // The IDE displays Message verbatim and receives source coordinates in
        // separate protocol fields, just as it does for legacy compiler errors.
        var message = error is PascalABCCompiler.Errors.Error compilerError
            ? compilerError.Message : error.ToString() ?? "";

        response.Append(warning ? "WARNING\t" : "DIAGNOSTIC\t");
        response.Append(EncodeBase64(fileName));
        response.Append('\t');
        response.Append(line);
        response.Append('\t');
        response.Append(column);
        response.Append('\t');
        response.AppendLine(EncodeBase64(message));
    }

    private static string EncodeBase64(string value) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes(value));

    private static void ApplyTestControl()
    {
        var fileName = Environment.GetEnvironmentVariable(
            "PABC_COMPILER_WORKER_TEST_CONTROL_FILE");
        if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
            return;

        var command = File.ReadAllText(fileName, Encoding.UTF8);
        File.Delete(fileName);

        const string hangPrefix = "hang:";
        if (command.StartsWith(hangPrefix, StringComparison.Ordinal) &&
            int.TryParse(command.Substring(hangPrefix.Length), out var milliseconds) &&
            milliseconds > 0)
        {
            Thread.Sleep(milliseconds);
            return;
        }

        const string stderrPrefix = "stderr-base64:";
        if (command.StartsWith(stderrPrefix, StringComparison.Ordinal))
        {
            var bytes = Convert.FromBase64String(
                command.Substring(stderrPrefix.Length));
            Console.Error.WriteLine(Encoding.UTF8.GetString(bytes));
            Console.Error.Flush();
        }
    }

    private static void ApplyDiagnosticLanguage(string? requestedLanguage)
    {
        string iso = string.IsNullOrWhiteSpace(requestedLanguage)
            ? defaultLanguageIso! : CultureInfo.GetCultureInfo(requestedLanguage).TwoLetterISOLanguageName;
        int index = StringResourcesLanguage.TwoLetterISOLanguages.FindIndex(
            value => string.Equals(value, iso, StringComparison.OrdinalIgnoreCase));
        // Older headless deployments may rely entirely on embedded resources.
        // An omitted language must continue to work without an external Lng folder.
        if (index < 0 && string.IsNullOrWhiteSpace(requestedLanguage) &&
            StringResourcesLanguage.AccessibleLanguages.Count == 0)
            return;
        if (index < 0)
            throw new ArgumentException(string.Format(StringResources.Get(
                "COMPILATIONERROR_UNSUPPORTED_DIAGNOSTIC_LANGUAGE{0}"), requestedLanguage));
        // Setting ISO alone does not load .dat files. Select the resource language
        // for every request, including switches in a long-lived Worker.
        StringResourcesLanguage.CurrentLanguageName = StringResourcesLanguage.AccessibleLanguages[index];
        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(iso);
    }

    private static int Main(string[] args)
    {
        Console.InputEncoding = new UTF8Encoding(false);
        Console.OutputEncoding = new UTF8Encoding(false);
        var protocolOutput = Console.Out;
        // Keep stdout protocol-only even if compiler code writes to Console.Out.
        Console.SetOut(Console.Error);

        try
        {
#if NET10_0
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            StringResourcesLanguage.LoadDefaultConfig();
            defaultLanguageIso = StringResourcesLanguage.CurrentTwoLetterISO;
            LanguageIntegrator.LoadAllLanguages();

            var compiler = new Compiler();
            Console.Error.WriteLine("PascalABC.NET compiler worker started");
            Console.Error.Flush();

            var running = true;
            while (running)
            {
                var inputLine = Console.ReadLine();
                if (inputLine == null)
                    break;
                if (string.IsNullOrWhiteSpace(inputLine))
                    continue;

                ApplyTestControl();
                var transportResponse = new WorkerTransportResponse();
                try
                {
                    var request = DeserializeTransportRequest(inputLine);
                    switch ((request.command ?? "").ToLowerInvariant())
                    {
                        case "ping":
                            transportResponse.success = true;
                            transportResponse.response = "PONG";
                            break;

                        case "shutdown":
                            transportResponse.success = true;
                            transportResponse.response = "BYE";
                            running = false;
                            break;

                        case "compile":
                            transportResponse.success = true;
                            transportResponse.response = CompileFile(
                                compiler, request.payload ?? "", progress =>
                                {
                                    transportResponse.compilerEvent = progress;
                                    // Even quiet clients need NuGet heartbeats for the Controller watchdog.
                                    if (!request.emitEvents && progress.state != "NuGetRestoreProgress") return;
                                    protocolOutput.WriteLine(SerializeTransportResponse(new WorkerTransportResponse
                                        { compilerEvent = progress }));
                                    protocolOutput.Flush();
                                });
                            break;

                        default:
                            throw new InvalidDataException(
                                "Неизвестная команда Worker: " + request.command);
                    }
                }
                catch (Exception exception)
                {
                    transportResponse.success = false;
                    transportResponse.error = exception.ToString();
                    Console.Error.WriteLine(exception);
                    Console.Error.Flush();
                }

                protocolOutput.WriteLine(
                    SerializeTransportResponse(transportResponse));
                protocolOutput.Flush();
            }

            Console.Error.WriteLine("PascalABC.NET compiler worker stopped");
            Console.Error.Flush();
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }
}
