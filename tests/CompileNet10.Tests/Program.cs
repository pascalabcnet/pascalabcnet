using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VisualPascalABCPlugins;

internal static partial class Program
{
    private sealed class DocumentService : IWorkbenchDocumentService
    {
        public ICodeFileDocument CurrentCodeFileDocument { get; set; }
        public ICodeFileDocument ActiveCodeFileDocument { get; set; }
        public ICodeFileDocument LastSelectedTab => CurrentCodeFileDocument;
        public bool ContainsTab(ICodeFileDocument tab) => false; // Debugger stack was cleared.
        public bool ContainsTab(string fileName) => GetDocument(fileName) != null;
        public ICodeFileDocument GetDocument(string fileName) =>
            CurrentCodeFileDocument?.FileName == fileName ? CurrentCodeFileDocument :
            ActiveCodeFileDocument?.FileName == fileName ? ActiveCodeFileDocument : null;
        public ICodeFileDocument GetTabPageForMainFile() => CurrentCodeFileDocument;
        public void SetTabPageText(ICodeFileDocument tab) { }
    }

    private sealed class EditorDocument : System.Windows.Forms.Control, ICodeFileDocument
    {
        public string FileName { get; set; }
        public string EXEFileName => null;
        public int LinesCount => 1;
        public ICSharpCode.TextEditor.TextEditorControl TextEditor { get; } = new ICSharpCode.TextEditor.TextEditorControl();
        public bool FromMetadata { get; set; }
        public bool DocumentChanged => true;
        public string ToolTipText { get; set; }
        public bool Run { get; set; }
        public void LoadFromFile(string fileName) { throw new NotSupportedException(); }
        protected override void Dispose(bool disposing)
        {
            if (disposing) TextEditor.Dispose();
            base.Dispose(disposing);
        }
    }
    private static void Check(bool condition, string name)
    {
        if (!condition) throw new Exception(name);
        Console.WriteLine("PASS: " + name);
    }

    private sealed class CommandHandler : IIdeCommandHandler
    {
        public bool Enabled;
        public int Calls;
        public VisualEnvironmentCompilerAction Action;
        public object Context;
        public bool TryExecute(VisualEnvironmentCompilerAction action, object context)
        {
            Calls++;
            Action = action;
            Context = context;
            return Enabled;
        }
    }

    private static void CheckCommandRouting()
    {
        var router = new IdeCommandRouter();
        Check(!router.TryExecute(VisualEnvironmentCompilerAction.Build), "without plugin legacy compile remains available");
        var handler = new CommandHandler();
        var registration = router.Register(handler);
        Check(!router.TryExecute(VisualEnvironmentCompilerAction.Run), "unchecked platform falls through to legacy run");
        handler.Enabled = true;
        Check(router.TryExecute(VisualEnvironmentCompilerAction.Build, false) && (bool)handler.Context == false,
            "shared Compile routes ordinary compilation");
        Check(router.TryExecute(VisualEnvironmentCompilerAction.Build, true) && (bool)handler.Context,
            "shared Recompile routes the same action with rebuild flag");
        var request = new IdeRunCommand { DebugRequested = true, RedirectConsoleIO = true };
        Check(router.TryExecute(VisualEnvironmentCompilerAction.Run, request) && ReferenceEquals(handler.Context, request),
            "shared Run preserves run/debug context");
        Check(router.TryExecute(VisualEnvironmentCompilerAction.Stop), "shared Stop routes to plugin");
        int calls = handler.Calls;
        Check(!router.TryExecute(VisualEnvironmentCompilerAction.OpenFile) && handler.Calls == calls,
            "unrelated IDE actions are not intercepted");
        registration.Dispose();
        registration.Dispose();
        Check(!router.TryExecute(VisualEnvironmentCompilerAction.Run), "unregister restores legacy commands");
        using (router.Register(handler))
            Check(router.TryExecute(VisualEnvironmentCompilerAction.Run), "handler can register again after disposal");
    }

    private static void CheckAnalysisSuspension()
    {
        var assembly = System.Reflection.Assembly.Load("CodeCompletion");
        var completion = assembly.GetType("CodeCompletion.CodeCompletionController", true);
        var suspended = completion.GetField("LegacyAnalysisSuspended");
        var semantic = assembly.GetType("CodeCompletion.DomSyntaxTreeVisitor", true).GetField("use_semantic_for_intellisense");
        object previousSuspension = suspended.GetValue(null), previousSemantic = semantic.GetValue(null);
        try
        {
            suspended.SetValue(null, true);
            foreach (bool enabled in new[] { false, true })
            {
                var options = new VisualPascalABC.UserOptions
                {
                    AllowCodeCompletion = enabled, UseSemanticIntellisense = enabled, UseDllForSystemUnits = enabled
                };
                semantic.SetValue(null, enabled);
                Check(!(bool)completion.GetMethod("IntellisenseAvailable").Invoke(null, null) &&
                    (bool)semantic.GetValue(null) == enabled && options.AllowCodeCompletion == enabled &&
                    options.UseSemanticIntellisense == enabled && options.UseDllForSystemUnits == enabled,
                    "net10 session gate blocks IntelliSense without overwriting user preferences: " + enabled);
            }
            var parser = new VisualPascalABC.CodeCompletionParserController();
            var compile = typeof(VisualPascalABC.CodeCompletionParserController).GetMethod("CompileWatchedFile",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Check(!(bool)compile.Invoke(parser, new object[] { "Suspended.pas", "begin end.", true }),
                "suspended background parser does not access the legacy compiler");
            string file = Path.GetFullPath("ResumeIntellisense.pas");
            parser.RegisterFileForParsing(file);
            VisualPascalABC.CodeCompletionParserController.filesToParse[file] = false;
            suspended.SetValue(null, false);
            parser.InvalidateAllFiles();
            Check(VisualPascalABC.CodeCompletionParserController.filesToParse[file],
                "return to legacy mode schedules open standalone documents for analysis");
            parser.CloseFile(file);
        }
        finally
        {
            suspended.SetValue(null, previousSuspension);
            semantic.SetValue(null, previousSemantic);
        }
    }

    [STAThread]
    private static int Main(string[] args) => RunTestsAsync(args).GetAwaiter().GetResult();

    private static void CheckParserRename()
    {
        var completion = System.Reflection.Assembly.Load("CodeCompletion")
            .GetType("CodeCompletion.CodeCompletionController", true);
        var suspended = completion.GetField("LegacyAnalysisSuspended");
        var modules = (System.Collections.Hashtable)completion.GetField("comp_modules").GetValue(null);
        bool previousSuspension = (bool)suspended.GetValue(null);
        var parser = new VisualPascalABC.CodeCompletionParserController();
        string oldName = Path.GetFullPath("RenameOld-" + Guid.NewGuid().ToString("N") + ".pas");
        string newName = Path.GetFullPath("RenameNew-" + Guid.NewGuid().ToString("N") + ".pas");
        try
        {
            foreach (bool isSuspended in new[] { true, false })
            {
                suspended.SetValue(null, isSuspended);
                modules.Remove(oldName);
                modules.Remove(newName);
                VisualPascalABC.CodeCompletionParserController.filesToParse.Remove(oldName);
                VisualPascalABC.CodeCompletionParserController.filesToParse.Remove(newName);
                Console.WriteLine("CASE: Save As without legacy analysis entries; suspended=" + isSuspended);
                parser.RenameFile(oldName, newName);
                Check(!VisualPascalABC.CodeCompletionParserController.filesToParse.ContainsKey(oldName) &&
                    VisualPascalABC.CodeCompletionParserController.filesToParse[newName],
                    "Save As with no legacy analysis entry schedules new name without throwing: " + isSuspended);
                Check(!modules.ContainsKey(oldName) && !modules.ContainsKey(newName),
                    "Save As does not invent a compiled IntelliSense model");

                modules[newName] = new object(); // Stale model from a previously closed destination.
                VisualPascalABC.CodeCompletionParserController.filesToParse[oldName] = false;
                parser.RenameFile(oldName, newName);
                Check(!modules.ContainsKey(newName) &&
                    !VisualPascalABC.CodeCompletionParserController.filesToParse[newName],
                    "Save As handles an uncompiled but registered editor and discards stale destination model");

                // Existing net472 entries still move, with their original scheduled state.
                object converter = new object();
                modules[oldName] = converter;
                VisualPascalABC.CodeCompletionParserController.filesToParse[oldName] = false;
                parser.RenameFile(oldName, newName);
                Check(!modules.ContainsKey(oldName) && ReferenceEquals(modules[newName], converter) &&
                    !VisualPascalABC.CodeCompletionParserController.filesToParse.ContainsKey(oldName) &&
                    !VisualPascalABC.CodeCompletionParserController.filesToParse[newName],
                    "Save As preserves an existing legacy model and pending flag: " + isSuspended);
                parser.RenameFile(newName, newName.ToUpperInvariant());
                Check(ReferenceEquals(modules[newName], converter) &&
                    VisualPascalABC.CodeCompletionParserController.filesToParse.ContainsKey(newName),
                    "case-only rename does not remove the same document");
            }
        }
        finally
        {
            modules.Remove(oldName);
            modules.Remove(newName);
            VisualPascalABC.CodeCompletionParserController.filesToParse.Remove(oldName);
            VisualPascalABC.CodeCompletionParserController.filesToParse.Remove(newName);
            suspended.SetValue(null, previousSuspension);
        }
    }

    private static void CheckDocumentEvents()
    {
        Check(typeof(IWorkbenchDocumentEvents).IsAssignableFrom(typeof(VisualPascalABC.Form1)),
            "IDE document service exposes optional lifecycle notifications");
        Check(typeof(IWorkbenchDocumentService).GetEvents().Length == 0,
            "historical document contract remains unchanged");
        using (var document = new EditorDocument { FileName = Path.GetFullPath("Events.pas") })
        using (var second = new EditorDocument { FileName = Path.GetFullPath("Floating.pas") })
        {
            var documents = new List<ICodeFileDocument> { document, second, document };
            int failures = 0;
            var source = new WorkbenchDocumentEventSource(() => documents, error => failures++);
            var snapshot = source.GetOpenDocuments();
            Check(snapshot.Length == 2 && Array.IndexOf(snapshot, second) >= 0,
                "document snapshot includes inactive/floating editors without duplicate tabs");
            snapshot[0] = null;
            Check(source.GetOpenDocuments()[0] == document && documents.Count == 3,
                "returned snapshot cannot mutate the host's open collection");
            int opens = 0, closes = 0, renames = 0;
            int textChanges = 0;
            ICSharpCode.TextEditor.Document.DocumentEventHandler textChanged = (sender, e) => textChanges++;
            EventHandler<WorkbenchDocumentEventArgs> bad = (sender, e) => { throw new InvalidOperationException("test plugin"); };
            EventHandler<WorkbenchDocumentEventArgs> opened = (sender, e) =>
            {
                opens++;
                Check(e.Document == document && e.FileName == document.FileName && e.PreviousFileName == null &&
                    e.Document.TextEditor.Document.TextContent == "begin Print(42); end.",
                    "open notification exposes loaded editor text and its assigned name");
                e.Document.TextEditor.Document.DocumentChanged += textChanged;
            };
            source.DocumentOpened += bad;
            source.DocumentOpened += opened;
            document.TextEditor.Document.TextContent = "begin Print(42); end.";
            source.Opened(document);
            Check(opens == 1 && failures == 1, "failing plugin cannot interrupt open or other subscribers");
            document.TextEditor.Document.TextContent = "begin Print(43); end.";
            Check(textChanges > 0, "consumer can subscribe to existing editor changes after open");
            source.DocumentOpened -= bad;
            source.DocumentOpened -= opened;
            source.Opened(document);
            Check(opens == 1, "document event subscription can be detached");

            WorkbenchDocumentEventArgs rename = null;
            source.DocumentRenamed += (sender, e) => { renames++; rename = e; };
            string oldName = document.FileName;
            source.Renamed(document, oldName);
            Check(renames == 0, "ordinary save does not emit a rename");
            document.FileName = Path.GetFullPath("SavedAs.pas");
            source.Renamed(document, oldName);
            string renamedName = document.FileName;
            document.FileName = Path.GetFullPath("SavedAgain.pas");
            Check(renames == 1 && rename.PreviousFileName == oldName && rename.FileName == renamedName,
                "Save As notification captures both names for delayed LSP consumers");
            source.Renamed(document, renamedName);
            Check(renames == 2, "repeated Save As is observable on the same editor");

            source.DocumentClosed += (sender, e) =>
            {
                closes++;
                Check(Array.IndexOf(source.GetOpenDocuments(), e.Document) < 0 &&
                    !document.IsDisposed && e.Document.TextEditor.Document.TextContent.Length > 0,
                    "close notification follows removal, while editor is still available for detaching handlers");
                e.Document.TextEditor.Document.DocumentChanged -= textChanged;
            };
            documents.RemoveAll(value => value == document);
            source.Closed(document);
            Check(closes == 1 && source.GetOpenDocuments().Length == 1,
                "closed editor is removed from subsequent snapshots");
            int previousChanges = textChanges;
            document.TextEditor.Document.TextContent = "";
            Check(textChanges == previousChanges && failures == 1,
                "closed editor handlers are detached before cleanup changes its text");
            documents.Add(document);
            Check(source.GetOpenDocuments().Length == 2,
                "late subscribers see existing editors through the same collection");
        }
    }

    private static async Task<int> RunTestsAsync(string[] args)
    {
        try
        {
            if (args.Length > 0 && args[0] == "--navigation-ui")
            {
                CheckLanguageNavigation(args.Length > 1 ? args[1] : Path.GetFullPath("bin-net10"));
                return 0;
            }
            if (args.Length > 0 && args[0] == "--signature-ui")
            {
                CheckSignatureTypedTrigger();
                if (args.Length > 1) CheckSignatureTypedTrigger(args[1]);
                return 0;
            }
            CheckRuntimeSettings();
            CheckCommandRouting();
            CheckDocumentEvents();
            CheckParserRename();
            CheckAnalysisSuspension();
            // Keep subsequent WinForms checks on the STA test thread; there is no UI message pump here.
            CheckLanguageSynchronizationAsync(args.Length > 0 ? args[0] : Path.GetFullPath("bin-net10")).GetAwaiter().GetResult();
            CheckLanguageCompletion(args.Length > 0 ? args[0] : Path.GetFullPath("bin-net10"));
            CheckLanguageHover();
            CheckLanguageSignature(args.Length > 0 ? args[0] : Path.GetFullPath("bin-net10"));
            CheckLanguageNavigation(args.Length > 0 ? args[0] : Path.GetFullPath("bin-net10"));
            CheckExternalInputRouting();
            CheckExternalRunRouting();
            Check(Net10EditorDocument.GetRunWarningResourceKey("MissingUnit.PCU") == "RUN_PCU_WARNING_TEXT",
                "compiled unit warns before checking output file existence");
            Check(Net10EditorDocument.GetRunWarningResourceKey("Library.DLL") == "RUN_DLL_WARNING_TEXT",
                "compiled library uses the canonical legacy run warning");
            Check(Net10EditorDocument.GetRunWarningResourceKey("Program.exe") == null &&
                Net10EditorDocument.GetRunWarningResourceKey(null) == null &&
                Net10EditorDocument.GetRunWarningResourceKey("") == null,
                "application and missing output retain normal run/error handling");
            using (var document = new EditorDocument { FileName = Path.GetFullPath("a1-2.pas"), Text = "a1-2.pas" })
            {
                document.TextEditor.Document.TextContent = "begin Write(42); end.";
                var snapshot = Net10EditorDocument.Read(document);
                Check(snapshot.Text == "begin Write(42); end." && snapshot.Text != document.Text,
                    "editor adapter reads source, not tab caption");
                document.TextEditor.Document.TextContent = "unit U; interface implementation end.";
                Check(Net10EditorDocument.Read(document).Text.StartsWith("unit U;"),
                    "editor adapter reads current unit changes");
                var open = Net10EditorDocument.ReadOpen(new[] { document },
                    new DocumentService { CurrentCodeFileDocument = document });
                Check(open.Count == 1 && open[0].Changed && open[0].Text.StartsWith("unit U;"),
                    "unsaved open unit is included even after debugger tab stack was cleared");
                using (var main = new EditorDocument { FileName = Path.GetFullPath("Main.pas") })
                {
                    var service = new DocumentService { CurrentCodeFileDocument = document, ActiveCodeFileDocument = main };
                    Check(Net10EditorDocument.ResolveRunTarget(document, "U.pcu", service) == main,
                        "Run10 from unit selects marked main program");
                    Check(Net10EditorDocument.ResolveRunTarget(document, "U.dll", service) == main,
                        "Run10 from library selects marked main program");
                    Check(Net10EditorDocument.ResolveRunTarget(document, "Other.exe", service) == document,
                        "Run10 from another program still runs that program");
                    service.ActiveCodeFileDocument = document;
                    Check(Net10EditorDocument.ResolveRunTarget(document, "U.pcu", service) == document,
                        "unit without another marked program does not recurse");
                }
            }
            using (var form = new System.Windows.Forms.Form())
            using (var strip = new System.Windows.Forms.StatusStrip { Name = "statusStrip1" })
            {
                var label = new System.Windows.Forms.ToolStripStatusLabel { Name = "toolStripStatusLabel5", Text = "Готово" };
                strip.Items.Add(label);
                form.Controls.Add(strip);
                using (var status = new Net10StatusDisplay(form))
                {
                    status.Start(".NET 10");
                    status.BeginOperation();
                    Check(label.Text == ".NET 10", "starting compile adds no transient preparation status");
                    status.Update("Компиляция .NET 10 прошла успешно");
                    Check(label.Text == "Компиляция .NET 10 прошла успешно", "net10 compilation result updates shared status line");
                    label.Text = "Обычная компиляция";
                    status.Update("Поздний результат .NET 10");
                    Check(label.Text == "Обычная компиляция", "late net10 status preserves ordinary IDE messages");
                    status.Start("Новая компиляция .NET 10");
                    Check(label.Text == "Новая компиляция .NET 10", "new net10 action updates status again");
                }
            }
            // The editor test needs STA, but subsequent pipe tests have no UI loop.
            System.Threading.SynchronizationContext.SetSynchronizationContext(null);
            const string wire = "обычный stderr[READLNSIGNAL][CODEPAGE65001][EXCEPTION]System.Exception[MESSAGE]ошибка[STACK]stack[END]tail";
            for (int split = 0; split <= wire.Length; split++)
            {
                var result = new StringBuilder();
                int reads = 0;
                var parser = new Net10RuntimeProtocol(s => result.Append(s), () => reads++);
                parser.Feed(wire.Substring(0, split));
                parser.Feed(wire.Substring(split));
                parser.Complete();
                if (reads != 1 || result.ToString() != "обычный stderr" + Environment.NewLine +
                    "System.Exception: ошибка" + Environment.NewLine + "stack" + Environment.NewLine + "tail")
                    throw new Exception("Protocol split " + split);
            }
            Check(true, "protocol commands at every stream split");
            const string exceptionWire = "[EXCEPTION]System.Exception[MESSAGE]ошибка[STACK]   at Main() in C:\\Путь с пробелами\\a.pas:line 12[END]";
            for (int split = 0; split <= exceptionWire.Length; split++)
            {
                RuntimeExceptionInfo error = null;
                int calls = 0;
                var parser = new Net10RuntimeProtocol(s => { throw new Exception("Structured exception leaked to output"); },
                    () => { }, e => { error = e; calls++; });
                parser.Feed(exceptionWire.Substring(0, split));
                parser.Feed(exceptionWire.Substring(split));
                parser.Complete();
                if (calls != 1 || error.Message != "ошибка" || error.Frames[0].Line != 12 ||
                    error.Frames[0].FileName != "C:\\Путь с пробелами\\a.pas")
                    throw new Exception("Structured exception split " + split);
            }
            Check(true, "structured runtime exception and source location at every stream split");
            var chars = new StringBuilder();
            int charReads = 0;
            var fragmented = new Net10RuntimeProtocol(s => chars.Append(s), () => charReads++);
            foreach (char c in wire) fragmented.Feed(c.ToString());
            fragmented.Complete();
            Check(charReads == 1 && chars.ToString().EndsWith("tail"), "one-character stream reads");

            if (args.Length != 1) throw new ArgumentException("Usage: CompileNet10.Tests.exe <full net10 runtime directory>");
            string root = Path.Combine(Path.GetTempPath(), "pabc-net10-run-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(root);
            Console.WriteLine("Fixtures: " + root);
            await CheckPortableHost(Path.GetFullPath(args[0]), root);
            using (var compiler = new Net10ControllerClient(args[0], "dotnet"))
            {
                await CheckWarnings(compiler, root);
                await CheckWarnings(compiler, root);
                await CheckCompilerEvents(compiler, Path.GetFullPath(args[0]), root);
                await CheckSnapshots(compiler, root);
                string jsonSource = Path.Combine(root, "JsonReference.pas");
                var jsonResult = await compiler.CompileAsync(jsonSource, root, "__RedirectIOMode",
                    new List<Net10SourceFile> { new Net10SourceFile { fileName = jsonSource,
                        text = "uses System.Text.Json; begin Write(JsonSerializer.Serialize(42)); end." } });
                Check(jsonResult.success, "System.Text.Json is available in local host: " + jsonResult.message);
                Check(await RunOutput(jsonResult.outputFile, root) == "42", "JSON program runs via bundled host");
                string source = Path.Combine(root, "Ввод с пробелами.pas");
                var memory = new List<Net10SourceFile> { new Net10SourceFile { fileName = source,
                    text = "begin Write('Привет: '); var s := ReadString; Write('Ответ=' + s); end." } };
                var compiled = await compiler.CompileAsync(source, root, "__RedirectIOMode", memory);
                Check(compiled.success, "compile with runtimeModule: " + compiled.message);
                var output = new StringBuilder();
                int requests = 0;
                Task sent = Task.CompletedTask;
                using (var runner = new Net10ProgramRunner("dotnet", compiled.outputFile, root, ""))
                {
                    var running = runner.RunAsync(s => { lock (output) output.Append(s); }, () =>
                    {
                        requests++;
                        sent = runner.SendInputAsync("мир");
                    });
                    if (await Task.WhenAny(running, Task.Delay(30000)) != running)
                    {
                        runner.Stop();
                        throw new TimeoutException("Readln timed out");
                    }
                    Check(await running == 0, "redirected process exits successfully");
                    await sent;
                }
                Check(requests == 1, "Readln emits one request");
                Check(output.ToString() == "Привет: Ответ=мир", "UTF-8 output without final newline");

                memory[0].text = "begin raise new System.Exception('Проверка исключения'); end.";
                compiled = await compiler.CompileAsync(source, root, "__RedirectIOMode", memory);
                Check(compiled.success, "compile exception fixture");
                output.Clear();
                using (var runner = new Net10ProgramRunner("dotnet", compiled.outputFile, root, ""))
                {
                    var running = runner.RunAsync(s => { lock (output) output.Append(s); }, () => { });
                    if (await Task.WhenAny(running, Task.Delay(30000)) != running)
                    {
                        runner.Stop();
                        throw new TimeoutException("Exception process timed out");
                    }
                    await running;
                }
                Check(output.ToString().Contains("System.Exception: Проверка исключения") &&
                    !output.ToString().Contains("[EXCEPTION]"), "formatted runtime exception");
                RuntimeExceptionInfo structuredError = null;
                int started = 0, exited = 0;
                using (var runner = new Net10ProgramRunner("dotnet", compiled.outputFile, root, ""))
                using (var lifecycle = new ExternalRunSession(() => started++, () => exited++, e => structuredError = e))
                {
                    await runner.RunAsync(s => { }, () => { }, lifecycle.Started, lifecycle.ReportException);
                }
                Check(started == 1 && exited == 1 && structuredError != null &&
                    structuredError.Type == "System.Exception" && structuredError.Message == "Проверка исключения",
                    "real process delivers structured exception between Starting and Exited");

                memory[0].text = "begin var s := ReadString; end.";
                compiled = await compiler.CompileAsync(source, root, "__RedirectIOMode", memory);
                Check(compiled.success, "compile stop fixture");
                using (var runner = new Net10ProgramRunner("dotnet", compiled.outputFile, root, ""))
                {
                    var running = runner.RunAsync(s => { }, runner.Stop);
                    if (await Task.WhenAny(running, Task.Delay(30000)) != running)
                    {
                        runner.Stop();
                        throw new TimeoutException("Stop timed out");
                    }
                    await running;
                    Check(runner.WasStopped, "stop while waiting for Readln");
                }
                Check(!File.Exists(source), "virtual I/O and exception sources were not written to disk");

                File.WriteAllText(source, "begin Write('Готово'); end.", new UTF8Encoding(false));
                compiled = await compiler.CompileAsync(source, root);
                Check(compiled.success, "ordinary compile after redirected runs");
            }
            return 0;
        }
        catch (Exception error)
        {
            Console.Error.WriteLine(error);
            return 1;
        }
    }

    private static async Task CheckWarnings(Net10ControllerClient compiler, string root)
    {
        string source = Path.Combine(root, "UnusedReadInteger.pas");
        var events = new List<Net10CompilerEvent>();
        var result = await compiler.CompileAsync(source, root, "__RedirectIOMode",
            new List<Net10SourceFile> { new Net10SourceFile { fileName = source,
                text = "begin\n  var a := ReadInteger;\n  Print(2)\nend." } },
            value => { events.Add(value); return Task.CompletedTask; });
        var warning = result.diagnostics?.Find(value => value.severity == "warning");
        Check(result.success && File.Exists(result.outputFile) && string.IsNullOrEmpty(result.message) &&
            warning != null && warning.fileName == source && warning.line == 2 && warning.column > 0 &&
            !string.IsNullOrWhiteSpace(warning.message),
            "successful ReadInteger program preserves warning text, file, line and column through Worker/Controller/client");
        Check(events.Exists(value => value.warningCount > 0), "compiler events include warning count");
        var items = Net10Diagnostics.Create(result, source);
        var located = items[0] as PascalABCCompiler.Errors.CompilerWarning;
        Check(located != null && located.Message == warning.message && located.SourceLocation.FileName == source &&
            located.SourceLocation.BeginPosition.Line == warning.line && located.SourceLocation.BeginPosition.Column == warning.column,
            "IDE receives a real warning with warning icon and source-navigation coordinates");
        Check(Net10Diagnostics.ChangeViewTab(result, false) && !Net10Diagnostics.ChangeViewTab(result, true),
            "Compile shows warnings; Run records warnings without switching away from output");
        var mixed = new Net10CompileResponse { success = false, diagnostics = new List<Net10Diagnostic>
        {
            warning, new Net10Diagnostic { severity = "error", fileName = source, line = 3, column = 1, message = "test error" }
        } };
        items = Net10Diagnostics.Create(mixed, source);
        Check(items.Count == 2 && !(items[0] is PascalABCCompiler.Errors.CompilerWarning) &&
            items[1] is PascalABCCompiler.Errors.CompilerWarning && Net10Diagnostics.ChangeViewTab(mixed, true),
            "errors remain before warnings and retain automatic navigation on Run");
        mixed.diagnostics.RemoveAt(1);
        Check(!(Net10Diagnostics.Create(mixed, source)[0] is PascalABCCompiler.Errors.CompilerWarning),
            "failed response with warnings only still reports a compilation error");
        result.diagnostics.Clear();
        Check(Net10Diagnostics.Create(result, source).Count == 0,
            "successful compilation without diagnostics does not invent an error");
    }

    private static async Task CheckSnapshots(Net10ControllerClient compiler, string root)
    {
        string procedureUnit = Path.Combine(root, "A.pas");
        string procedureMain = Path.Combine(root, "ProcedureMain.pas");
        const string savedProcedure = "unit A; procedure p1; begin Print(2) end; end.";
        const string mainProcedure = "uses A; begin p1 end.";
        File.WriteAllText(procedureUnit, savedProcedure, new UTF8Encoding(false));
        File.WriteAllText(procedureMain, mainProcedure, new UTF8Encoding(false));
        var procedureResult = await compiler.CompileAsync(procedureMain, root, "__RedirectIOMode");
        Check(procedureResult.success && (await RunOutput(procedureResult.outputFile, root)).Trim() == "2",
            "saved procedure unit produces 2 and creates a PCU");
        var unitResult = await compiler.CompileAsync(procedureUnit, root, "__RedirectIOMode",
            new List<Net10SourceFile> { new Net10SourceFile { fileName = procedureUnit, text = savedProcedure } });
        Check(unitResult.success, "standalone unit compilation: " + unitResult.message);
        Check(string.Equals(Path.GetExtension(unitResult.outputFile), ".pcu", StringComparison.OrdinalIgnoreCase),
            "actual Worker unit response uses the PCU fallback tested by Run10 selection");
        Check(Net10EditorDocument.GetRunWarningResourceKey(unitResult.outputFile) == "RUN_PCU_WARNING_TEXT",
            "actual Worker unit result is reported as a run warning, not a missing executable");
        foreach (int value in new[] { 3, 4 })
        {
            procedureResult = await compiler.CompileAsync(procedureMain, root, "__RedirectIOMode",
                new List<Net10SourceFile> {
                    new Net10SourceFile { fileName = procedureMain, text = mainProcedure },
                    new Net10SourceFile { fileName = procedureUnit,
                        text = savedProcedure.Replace("Print(2)", "Print(" + value + ")") }
                });
            Check(procedureResult.success && (await RunOutput(procedureResult.outputFile, root)).Trim() == value.ToString(),
                "unsaved procedure body overrides existing PCU: " + value);
        }
        Check(File.ReadAllText(procedureUnit) == savedProcedure,
            "procedure unit source stays unchanged on disk");

        string main = Path.Combine(root, "SnapshotMain.pas");
        string edited = Path.Combine(root, "EditedUnit.pas");
        string virtualUnit = Path.Combine(root, "VirtualUnit.pas");
        string disk = Path.Combine(root, "DiskUnit.pas");
        const string savedMain = "begin Write('disk'); end.";
        const string savedUnit = "unit EditedUnit; interface const Value1 = 1; implementation end.";
        const string diskUnit = "unit DiskUnit; interface const Value3 = 30; implementation end.";
        File.WriteAllText(main, savedMain, new UTF8Encoding(false));
        File.WriteAllText(edited, savedUnit, new UTF8Encoding(false));
        File.WriteAllText(disk, diskUnit, new UTF8Encoding(false));
        var sources = Net10SourceSnapshot.Capture(new Net10EditorSource { FileName = main,
            Text = "uses EditedUnit, VirtualUnit, DiskUnit; begin Write(Value1 + Value2 + Value3); end." },
            new[] {
                new Net10EditorSource { FileName = edited, Changed = true,
                    Text = "unit EditedUnit; interface const Value1 = 10; implementation end." },
                new Net10EditorSource { FileName = virtualUnit,
                    Text = "unit VirtualUnit; interface const Value2 = 20; implementation end." },
                new Net10EditorSource { FileName = disk, Text = diskUnit },
                new Net10EditorSource { FileName = Path.Combine(root, "metadata.pas"), Changed = true, FromMetadata = true },
                new Net10EditorSource { FileName = Path.Combine(root, "other.txt"), Changed = true }
            });
        Check(sources.Count == 3 && sources[0].fileName == main && !sources.Exists(s => s.fileName == disk),
            "snapshot selection: main, edited unit, new unit; omit unchanged disk/metadata/non-Pascal");
        var response = await compiler.CompileAsync(main, root, "__RedirectIOMode", sources);
        Check(response.success, "compile mixed snapshot and disk units: " + response.message);
        Check(await RunOutput(response.outputFile, root) == "60", "unsaved main and units override disk");
        Check(File.Exists(Path.ChangeExtension(disk, ".pcu")) &&
            File.Exists(Path.ChangeExtension(edited, ".pcu")),
            "snapshot compilation retains normal PCU generation for disk and editor units");

        sources.Find(s => s.fileName == edited).text = "unit EditedUnit; interface const Value1 = 40; implementation end.";
        response = await compiler.CompileAsync(main, root, "__RedirectIOMode", sources);
        Check(response.success && await RunOutput(response.outputFile, root) == "90", "recompile after editing unit");

        sources.Find(s => s.fileName == virtualUnit).text = "unit VirtualUnit;\ninterface\nvar x: MissingSnapshotType;\nimplementation\nend.";
        response = await compiler.CompileAsync(main, root, "__RedirectIOMode", sources);
        var error = response.diagnostics?.Find(d => string.Equals(d.fileName, virtualUnit, StringComparison.OrdinalIgnoreCase));
        Check(!response.success && error != null && error.line == 3 && error.column > 0 && !string.IsNullOrWhiteSpace(error.message),
            "virtual unit diagnostic path, line, column and message");

        string newMain = Path.Combine(root, "NewUnsavedMain.pas");
        response = await compiler.CompileAsync(newMain, root, "__RedirectIOMode",
            new List<Net10SourceFile> {
                new Net10SourceFile { fileName = newMain, text = "uses VirtualUnit; begin Write(Value2); end." },
                new Net10SourceFile { fileName = virtualUnit, text = "unit VirtualUnit; interface const Value2 = 22; implementation end." }
            });
        Check(response.success && await RunOutput(response.outputFile, root) == "22", "fully virtual main and unit");
        Check(!File.Exists(newMain) && !File.Exists(virtualUnit) &&
            File.ReadAllText(main) == savedMain && File.ReadAllText(edited) == savedUnit,
            "snapshot does not create or overwrite source files");
        response = await compiler.CompileAsync(main, root, "__RedirectIOMode");
        Check(response.success && await RunOutput(response.outputFile, root) == "disk", "next request without snapshot reads disk");
    }

    private static void CheckRuntimeSettings()
    {
        string root = Path.Combine(Path.GetTempPath(), "pabc-runtime-path-" + Guid.NewGuid().ToString("N"));
        string previousDirectory = Environment.CurrentDirectory;
        Directory.CreateDirectory(root);
        try
        {
            foreach (string layout in new[] { "bin", "VS output", "installed IDE" })
            {
                string ide = Path.Combine(root, layout);
                Directory.CreateDirectory(ide);
                // Simulate VS launching with a working directory unrelated to the EXE.
                Environment.CurrentDirectory = root;
                var settings = Net10RuntimeSettings.Load(ide);
                string expected = Path.GetFullPath(Path.Combine(ide, "..", "bin-net10"));
                Check(settings.RuntimeDirectory == expected && settings.DotnetPath == "dotnet",
                    layout + ": no INI uses host beside IDE, independent of working directory");
                string ini = Path.Combine(ide, "CompileNet10Plugin.ini");
                File.WriteAllText(ini, "# bundled defaults\nRuntimeDirectory=..\\bin-net10\nDotnetPath=dotnet\n");
                Check(Net10RuntimeSettings.Load(ide).RuntimeDirectory == expected,
                    layout + ": bundled INI resolves the same host");
                File.WriteAllText(ini, "RuntimeDirectory=custom host\nDotnetPath=C:\\custom dotnet\\dotnet.exe\n");
                settings = Net10RuntimeSettings.Load(ide);
                Check(settings.RuntimeDirectory == Path.Combine(ide, "custom host") &&
                    settings.DotnetPath == "C:\\custom dotnet\\dotnet.exe", "explicit relative runtime and dotnet overrides");
                File.WriteAllText(ini, "RuntimeDirectory=" + expected + "\nDotnetPath=\n");
                settings = Net10RuntimeSettings.Load(ide);
                Check(settings.RuntimeDirectory == expected && settings.DotnetPath == "dotnet",
                    "absolute runtime override and empty dotnet default");
            }
        }
        finally
        {
            Environment.CurrentDirectory = previousDirectory;
            Directory.Delete(root, true); // Only fixtures created in this method.
        }
    }

    private static void CheckExternalInputRouting()
    {
        using (var net10 = new EditorDocument())
        using (var legacy = new EditorDocument())
        {
            string sent = null;
            int stops = 0, requests = 0, releases = 0;
            var pending = new Dictionary<ICodeFileDocument, string> { { legacy, "legacy draft" } };
            var session = new ExternalInputSession(net10, text => sent = text, () => stops++,
                () => { requests++; pending[net10] = ""; },
                () => { releases++; pending.Remove(net10); });
            Check(!session.TrySend(net10, "early"), "standard input sends only after READLNSIGNAL");
            session.RequestInput();
            session.RequestInput();
            Check(requests == 1 && session.IsWaiting, "duplicate input signal preserves pending request");
            Check(!session.TrySend(legacy, "wrong") && !session.TryStop(legacy) && sent == null && stops == 0,
                "net472/other tab is never routed to Run10");
            Check(session.TrySend(net10, "Кириллица") && sent == "Кириллица" && !session.IsWaiting,
                "standard input routes text to its owning Run10 process");
            Check(!session.TrySend(net10, "duplicate"), "duplicate Enter cannot send twice");
            session.RequestInput();
            Check(requests == 2 && session.IsWaiting, "next Readln reopens standard input");
            Check(session.TryStop(net10) && stops == 1, "standard Stop routes to owning Run10 process");
            session.Dispose();
            session.Dispose();
            session.RequestInput();
            Check(releases == 1 && !session.IsWaiting && !session.Owns(net10) &&
                !session.TrySend(net10, "late") && !session.TryStop(net10),
                "closing Run10 releases input and ignores late signals");
            Check(pending.Count == 1 && pending[legacy] == "legacy draft",
                "releasing Run10 preserves ordinary net472 input requests");
        }
    }

    private static async Task CheckPortableHost(string sourceRuntime, string root)
    {
        string ide = Path.Combine(root, "portable IDE with spaces");
        string runtime = Path.GetFullPath(Path.Combine(ide, "..", "bin-net10"));
        Directory.CreateDirectory(runtime);
        foreach (string source in Directory.GetFiles(sourceRuntime, "*", SearchOption.AllDirectories))
        {
            string destination = Path.Combine(runtime, source.Substring(sourceRuntime.Length + 1));
            Directory.CreateDirectory(Path.GetDirectoryName(destination));
            File.Copy(source, destination);
        }
        var settings = Net10RuntimeSettings.Load(ide);
        string previousDirectory = Environment.CurrentDirectory;
        try
        {
            Environment.CurrentDirectory = root; // Not the IDE directory or a checkout.
            using (var compiler = new Net10ControllerClient(settings.RuntimeDirectory, settings.DotnetPath))
            {
                string main = Path.Combine(root, "PortableMain.pas");
                var response = await compiler.CompileAsync(main, root, "__RedirectIOMode",
                    new List<Net10SourceFile> { new Net10SourceFile { fileName = main,
                        text = "begin Write('Переносимый host'); end." } });
                Check(response.success, "copied distribution host compiles without INI or checkout: " + response.message);
                Check(await RunOutput(response.outputFile, root) == "Переносимый host",
                    "copied distribution host runs with a different working directory");
            }
        }
        finally { Environment.CurrentDirectory = previousDirectory; }
    }

    private static async Task CheckCompilerEvents(Net10ControllerClient compiler, string runtime, string root)
    {
        string empty = Path.Combine(root, "EmptySnapshot.pas");
        var warmSources = new List<Net10SourceFile> { new Net10SourceFile { fileName = empty, text = "begin end." } };
        var warmup = await compiler.CompileAsync(empty, root, "__RedirectIOMode", warmSources);
        Check(warmup.success, "empty editor program compiles with normal PCU generation");
        var repeated = new List<Net10CompilerEvent>();
        warmup = await compiler.CompileAsync(empty, root, "__RedirectIOMode", warmSources,
            value => { repeated.Add(value); return Task.CompletedTask; });
        Check(warmup.success && repeated.FindAll(value => value.state == "CompilationStarting").Count == 1 &&
            repeated.FindAll(value => value.state == "CompilationFinished").Count == 1 &&
            !repeated.Exists(value => value.state == "ReadDLL" &&
                string.Equals(Path.GetFileName(value.fileName), "PABCRtl.dll", StringComparison.OrdinalIgnoreCase)),
            "net10 compilation has one pass without a PABCRtl fallback");
        Check(warmup.success && repeated.Exists(value => value.state == "ReadPCUFile" &&
                string.Equals(Path.GetFileName(value.fileName), "PABCSystem.pcu", StringComparison.OrdinalIgnoreCase)) &&
            !repeated.Exists(value => value.state == "CompileInterface" &&
                string.Equals(Path.GetFileName(value.fileName), "PABCSystem.pas", StringComparison.OrdinalIgnoreCase)),
            "repeated empty editor program reuses PABCSystem PCU instead of compiling its source");
        var rebuilt = new List<Net10CompilerEvent>();
        var rebuildResult = await compiler.CompileAsync(empty, root, "__RedirectIOMode", warmSources,
            value => { rebuilt.Add(value); return Task.CompletedTask; }, rebuild: true);
        Check(rebuildResult.success && rebuilt.Exists(value => value.state == "CompileInterface" &&
                string.Equals(Path.GetFileName(value.fileName), "PABCSystem.pas", StringComparison.OrdinalIgnoreCase)) &&
            !rebuilt.Exists(value => value.state == "ReadPCUFile" &&
                string.Equals(Path.GetFileName(value.fileName), "PABCSystem.pcu", StringComparison.OrdinalIgnoreCase)),
            "Recompile forwards rebuild through Controller and Worker and rebuilds cached system units from source");
        var events = new List<Net10CompilerEvent>();
        string source = Path.Combine(runtime, "Lib", "PABCSystem.pas");
        var result = await compiler.CompileAsync(source, root, null,
            new List<Net10SourceFile> { new Net10SourceFile { fileName = source, text = File.ReadAllText(source) } },
            value => { events.Add(value); return Task.CompletedTask; });
        Check(result.success, "PABCSystem compilation with real streamed events: " + result.message);
        Check(events.Exists(value => value.state == "CompileInterface") &&
            events.Exists(value => value.state == "CompileImplementation") &&
            !events.Exists(value => value.state == "ReadDLL"), "PABCSystem phase events preserved, verbose DLL-reading events suppressed");
        var messages = new List<string>();
        var statuses = new List<string>();
        var display = new Net10CompilerDisplay(key => key);
        foreach (var value in events) display.Handle(value, messages.Add, statuses.Add);
        Check(display.HasResult && events[events.Count - 1].linesCompiled > 0 &&
            events[events.Count - 1].elapsedMilliseconds > 0, "real line count and duration, Ready before final response");
        Check(messages.Exists(text => text.StartsWith("[.NET 10]STATE_COMPILEINTERFACE")) &&
            messages.Exists(text => text.StartsWith("CM_OK_")) &&
            statuses[statuses.Count - 1].Contains("STATETEXT_COMPILATION_SUCCESS"), "legacy resource keys format progress and success status");
        display.Handle(new Net10CompilerEvent { state = "CompilationStarting" }, messages.Add, statuses.Add);
        display.Handle(new Net10CompilerEvent { state = "Reloading", attempt = 2 }, messages.Add, statuses.Add);
        display.Handle(new Net10CompilerEvent { state = "Ready", attempt = 2 }, messages.Add, statuses.Add);
        Check(!display.HasResult && statuses[statuses.Count - 1] == ".NET 10: STATETEXT_RELOADING",
            "Ready during retry/reload adds no transient status or successful compilation");
        var quiet = new Net10CompilerDisplay(key => key);
        var quietStatuses = new List<string>();
        quiet.Handle(new Net10CompilerEvent { state = "Ready" }, messages.Add, quietStatuses.Add);
        quiet.Handle(new Net10CompilerEvent { state = "CompilationStarting" }, messages.Add, quietStatuses.Add);
        Check(quietStatuses.Count == 0, "startup and compilation bookkeeping do not flicker in status line");
        quiet.Handle(new Net10CompilerEvent { state = "BeginCompileFile", fileName = "Main.pas" }, messages.Add, quietStatuses.Add);
        quiet.Handle(new Net10CompilerEvent { state = "CodeGeneration", fileName = "Main.exe" }, messages.Add, quietStatuses.Add);
        quiet.Handle(new Net10CompilerEvent { state = "Ready", linesCompiled = 2 }, messages.Add, quietStatuses.Add);
        Check(quietStatuses.Count == 3 && quietStatuses[2].Contains("STATETEXT_COMPILATION_SUCCESS"),
            "status retains canonical compile/code-generation/result sequence");
    }

    private static void CheckExternalRunRouting()
    {
        var manager = new VisualPascalABC.RunManager(id => { });
        int starts = 0, exits = 0, stops = 0, errors = 0;
        string output = "";
        string file = Path.GetFullPath("external-net10.exe");
        manager.Starting += id => { Check(manager.IsRun(id), "run registered before Starting"); starts++; };
        manager.Exited += id => { Check(!manager.IsRun(id), "run removed before Exited"); exits++; };
        manager.OutputStringReceived += (id, stream, text) => output += text;
        manager.ChangeArgsBeforeRun += (ref string args) => args += " teacher-argument";
        Check(manager.PrepareExternalArguments("[REDIRECTIOMODE]") == "[REDIRECTIOMODE] teacher-argument",
            "shared ChangeArgsBeforeRun hook");
        using (var session = new ExternalRunSession(() => manager.ExternalStarted(file, () => stops++),
            () => manager.ExternalExited(file), error => errors++, text => manager.ExternalOutput(file, text)))
        {
            session.Started(); session.Started();
            Check(starts == 1 && manager.Count == 1 && manager.IsRun(), "single shared Starting and IsRun");
            manager.Stop(file.ToUpperInvariant());
            manager.KillAll();
            Check(stops == 2, "ordinary Stop and KillAll reach external process");
            session.ReportException(new RuntimeExceptionInfo());
            session.WriteOutput("Привет");
            session.Dispose(); session.Dispose();
            session.ReportException(new RuntimeExceptionInfo());
            session.WriteOutput("late");
            Check(exits == 1 && errors == 1 && manager.Count == 0, "single shared Exited and no late exception");
            Check(output == "Привет", "shared output handler and no output after Exited");
        }
        using (new ExternalRunSession(() => starts++, () => exits++, error => errors++)) { }
        Check(starts == 1 && exits == 1, "failed process start emits no lifecycle events");
    }

    private static async Task<string> RunOutput(string assembly, string root)
    {
        var output = new StringBuilder();
        using (var runner = new Net10ProgramRunner("dotnet", assembly, root, ""))
        {
            var running = runner.RunAsync(s => { lock (output) output.Append(s); }, () => runner.Stop());
            if (await Task.WhenAny(running, Task.Delay(30000)) != running)
            {
                runner.Stop();
                throw new TimeoutException("Snapshot program timed out");
            }
            if (await running != 0) throw new Exception("Snapshot run failed: " + output);
        }
        return output.ToString();
    }
}
