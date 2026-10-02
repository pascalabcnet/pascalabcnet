using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VisualPascalABCPlugins;

internal static class Program
{
    private sealed class EditorDocument : System.Windows.Forms.Control, ICodeFileDocument
    {
        public string FileName { get; set; }
        public string EXEFileName => null;
        public int LinesCount => 1;
        public ICSharpCode.TextEditor.TextEditorControl TextEditor { get; } = new ICSharpCode.TextEditor.TextEditorControl();
        public bool FromMetadata => false;
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

    [STAThread]
    private static int Main(string[] args) => RunTestsAsync(args).GetAwaiter().GetResult();

    private static async Task<int> RunTestsAsync(string[] args)
    {
        try
        {
            CheckRuntimeSettings();
            CheckExternalInputRouting();
            using (var document = new EditorDocument { FileName = Path.GetFullPath("a1-2.pas"), Text = "a1-2.pas" })
            {
                document.TextEditor.Document.TextContent = "begin Write(42); end.";
                var snapshot = Net10EditorDocument.Read(document);
                Check(snapshot.Text == "begin Write(42); end." && snapshot.Text != document.Text,
                    "editor adapter reads source, not tab caption");
                document.TextEditor.Document.TextContent = "unit U; interface implementation end.";
                Check(Net10EditorDocument.Read(document).Text.StartsWith("unit U;"),
                    "editor adapter reads current unit changes");
            }
            using (var form = new System.Windows.Forms.Form())
            using (var strip = new System.Windows.Forms.StatusStrip { Name = "statusStrip1" })
            {
                var label = new System.Windows.Forms.ToolStripStatusLabel { Name = "toolStripStatusLabel5", Text = "Готово" };
                strip.Items.Add(label);
                form.Controls.Add(strip);
                using (var status = new Net10StatusDisplay(form))
                {
                    status.Start("Компиляция .NET 10");
                    status.Update("Программа .NET 10 ожидает ввода");
                    Check(label.Text == "Программа .NET 10 ожидает ввода", "net10 status updates shared status line");
                    label.Text = "Обычная компиляция";
                    status.Update("Выполнение .NET 10 завершено");
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

    private static async Task CheckSnapshots(Net10ControllerClient compiler, string root)
    {
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
