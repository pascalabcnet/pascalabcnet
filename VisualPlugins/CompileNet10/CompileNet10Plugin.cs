using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace VisualPascalABCPlugins
{
    // The legacy IDE discovers plugins by this class-name suffix and IWorkbench constructor.
    public sealed class CompileNet10_VisualPascalABCPlugin : IExtendedVisualPascalABCPlugin
    {
        private readonly IWorkbench workbench;
        private readonly PluginGUIItem compileItem;
        private readonly PluginGUIItem runItem;
        private readonly PluginGUIItem stopItem;
        private readonly Net10StatusDisplay status;
        private Net10ControllerClient client;
        private Net10ProgramRunner runner;
        private Net10OutputSession outputSession;
        private string runningOutputFile;
        private string dotnetPath = "dotnet";
        private bool compiling;

        public CompileNet10_VisualPascalABCPlugin(IWorkbench workbench)
        {
            if (workbench == null)
                throw new ArgumentNullException(nameof(workbench));

            this.workbench = workbench;
            status = new Net10StatusDisplay(workbench.MainForm);
            compileItem = new PluginGUIItem(
                "Compile10",
                "Скомпилировать текущий текст под .NET 10",
                null,
                Color.Transparent,
                () => Execute(false));
            runItem = new PluginGUIItem(
                "Run10",
                "Скомпилировать и запустить текущий текст под .NET 10",
                null,
                Color.Transparent,
                () => Execute(true));
            stopItem = new PluginGUIItem("Stop10", "Остановить программу .NET 10",
                null, Color.Transparent, () => runner?.Stop());
            workbench.MainForm.FormClosed += (sender, e) =>
            {
                runner?.Stop();
                outputSession?.Dispose();
                client?.Dispose();
                status.Dispose();
            };
            // Do not compete with a normal IDE run for the shared console/input.
            workbench.ServiceContainer.RunService.Starting += fileName =>
            {
                if (string.Equals(fileName, runningOutputFile, StringComparison.OrdinalIgnoreCase)) return;
                runner?.Stop();
                // Release ReadRequests before the ordinary runner starts reading input.
                outputSession?.Dispose();
            };
        }

        public string Name => "Compile .NET 10 (experimental)";
        public string Version => "0.5";
        public string Copyright => "PascalABC.NET";

        public void GetGUI(List<IPluginGUIItem> menuItems, List<IPluginGUIItem> toolBarItems)
        {
            menuItems.Add(compileItem);
            toolBarItems.Add(compileItem);
            menuItems.Add(runItem);
            toolBarItems.Add(runItem);
            menuItems.Add(stopItem);
            toolBarItems.Add(stopItem);
        }

        public void AfterAddInGUI()
        {
            // The host normally displays icon-only plugin buttons; this prototype uses a label.
            ConfigureButton(compileItem);
            ConfigureButton(runItem);
            ConfigureButton(stopItem);
            SetItemEnabled(stopItem, false);
        }

        private static void ConfigureButton(PluginGUIItem item)
        {
            var button = item.toolStripButton as ToolStripButton;
            if (button != null)
            {
                button.Text = item.Text;
                button.DisplayStyle = ToolStripItemDisplayStyle.Text;
                button.AutoToolTip = false;
                button.ToolTipText = item.Hint;
            }
        }

        private async void Execute(bool run)
        {
            if (compiling) return;
            IDisposable preparation = null;
            status.Start("Подготовка компиляции .NET 10…");
            try
            {
                var document = workbench.ServiceContainer.DocumentService.CurrentCodeFileDocument;
                if (document == null || document.FromMetadata)
                    throw new InvalidOperationException("Выберите исходный файл.");
                var externalRun = workbench.ServiceContainer.RunService as IExternalRunService;
                if (externalRun == null)
                    throw new InvalidOperationException("Для событий Run10 пересоберите IDE и PluginsSupport.dll.");
                preparation = externalRun.PrepareExternalCompile(document.FileName);
                var sources = Net10SourceSnapshot.Capture(Net10EditorDocument.Read(document), OpenEditorSources());
                string fileName = sources[0].fileName;
                string outputDirectory = Path.Combine(Path.GetDirectoryName(fileName), "net10-output");
                var runService = workbench.ServiceContainer.RunService;
                if (run && runService.IsRun())
                    throw new InvalidOperationException("Сначала остановите обычную программу IDE.");
                if (run) workbench.ServiceContainer.OperationsService.ClearOutputTextBoxForTabPage(document);
                string arguments = run && runService.HasRunArgument(fileName)
                    ? runService.GetRunArgument(fileName) : "";
                workbench.CompilerConsoleWindow.ClearConsole();
                if (client == null) client = CreateClient();
                compiling = true;
                SetEnabled(false);
                workbench.ErrorsListWindow.ClearErrorList();
                status.Update("Компиляция .NET 10: " + Path.GetFileName(fileName) + "…");
                var display = new Net10CompilerDisplay(key => PascalABCCompiler.StringResources.Get("VP_VEC_" + key));
                // Await each UI delivery so Ready cannot arrive after Run/exception status.
                var response = await client.CompileAsync(fileName, outputDirectory, "__RedirectIOMode", sources,
                    value => DisplayCompilerEventAsync(display, value));
                if (run && response.success)
                {
                    var target = Net10EditorDocument.ResolveRunTarget(document, response.outputFile,
                        workbench.ServiceContainer.DocumentService);
                    if (target != document)
                    {
                        document = target;
                        workbench.ServiceContainer.DocumentService.CurrentCodeFileDocument = document;
                        sources = Net10SourceSnapshot.Capture(Net10EditorDocument.Read(document), OpenEditorSources());
                        fileName = sources[0].fileName;
                        outputDirectory = Path.Combine(Path.GetDirectoryName(fileName), "net10-output");
                        arguments = runService.HasRunArgument(fileName) ? runService.GetRunArgument(fileName) : "";
                        workbench.ServiceContainer.OperationsService.ClearOutputTextBoxForTabPage(document);
                        display = new Net10CompilerDisplay(key => PascalABCCompiler.StringResources.Get("VP_VEC_" + key));
                        status.Update("Компиляция .NET 10: " + Path.GetFileName(fileName) + "…");
                        response = await client.CompileAsync(fileName, outputDirectory, "__RedirectIOMode", sources,
                            value => DisplayCompilerEventAsync(display, value));
                    }
                }
                if (response.success)
                {
                    if (!display.HasResult)
                    {
                        WriteMessage("Готово: " + response.outputFile);
                        status.Update("Компиляция .NET 10 прошла успешно");
                    }
                    if (run && !workbench.MainForm.IsDisposed)
                    {
                        string outputFile = response.outputFile;
                        if (string.IsNullOrWhiteSpace(outputFile))
                            throw new IOException("Компилятор не сообщил имя выходного файла.");
                        if (!Path.IsPathRooted(outputFile))
                            outputFile = Path.Combine(outputDirectory, outputFile);
                        if (runService.IsRun())
                            throw new InvalidOperationException("Уже запущена обычная программа IDE.");
                        using (var program = new Net10ProgramRunner(dotnetPath, Path.GetFullPath(outputFile),
                            Path.GetDirectoryName(fileName), arguments, externalRun.PrepareExternalArguments))
                        using (var lifecycle = externalRun.RegisterExternalRun(document, outputFile, program.Stop))
                        using (var console = new Net10OutputSession(workbench, document,
                            text =>
                            {
                                status.Update("Программа .NET 10 выполняется");
                                return program.SendInputAsync(text);
                            }, program.Stop, lifecycle.WriteOutput))
                        {
                            runner = program;
                            runningOutputFile = Path.GetFullPath(outputFile);
                            outputSession = console;
                            SetItemEnabled(stopItem, true);
                            WriteMessage("Запущено .NET 10: " + outputFile);
                            status.Update("Программа .NET 10 выполняется");
                            int exitCode = await program.RunAsync(console.Append, () =>
                            {
                                status.Update("Программа .NET 10 ожидает ввода");
                                console.RequestInput();
                            }, lifecycle.Started, error => console.ReportException(error, lifecycle));
                            await console.FlushAsync();
                            status.Update(program.WasStopped ? "Программа .NET 10 остановлена" :
                                exitCode == 0 ? "Выполнение .NET 10 завершено" :
                                "Программа .NET 10 завершилась с ошибкой (код " + exitCode + ")");
                            // Both output streams were queued before this UI continuation.
                            if (exitCode != 0 && !program.WasStopped)
                                WriteMessage("Программа .NET 10 завершилась с кодом " + exitCode + ".");
                        }
                    }
                }
                else
                {
                    if (!display.HasResult) status.Update("Ошибка компиляции .NET 10");
                    ShowDiagnostics(response, fileName);
                }
            }
            catch (Exception error)
            {
                status.Update("Ошибка .NET 10: " + error.Message);
                WriteMessage((run ? "Run .NET 10: " : "Compile .NET 10: ") + error.Message);
            }
            finally
            {
                compiling = false;
                runner = null;
                runningOutputFile = null;
                outputSession = null;
                if (!workbench.MainForm.IsDisposed) SetItemEnabled(stopItem, false);
                SetEnabled(true);
                preparation?.Dispose();
            }
        }

        private System.Threading.Tasks.Task DisplayCompilerEventAsync(Net10CompilerDisplay display, Net10CompilerEvent value)
        {
            var completed = new System.Threading.Tasks.TaskCompletionSource<bool>();
            var form = workbench.MainForm;
            if (form.IsDisposed || form.Disposing) return System.Threading.Tasks.Task.CompletedTask;
            FormClosedEventHandler closed = null;
            closed = (sender, e) => { form.FormClosed -= closed; completed.TrySetResult(true); };
            form.FormClosed += closed;
            try
            {
                form.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        if (!form.IsDisposed) display.Handle(value, WriteMessage, status.Update);
                        completed.TrySetResult(true);
                    }
                    catch (Exception error) { completed.TrySetException(error); }
                    finally { form.FormClosed -= closed; }
                }));
            }
            catch (InvalidOperationException)
            {
                form.FormClosed -= closed;
                completed.TrySetResult(true);
            }
            return completed.Task;
        }

        private void ShowDiagnostics(Net10CompileResponse response, string sourceFileName)
        {
            var errors = new List<PascalABCCompiler.Errors.Error>();
            if (response.diagnostics != null)
                foreach (var diagnostic in response.diagnostics)
                {
                    string fileName = string.IsNullOrWhiteSpace(diagnostic.fileName)
                        ? sourceFileName : diagnostic.fileName;
                    if (!Path.IsPathRooted(fileName))
                        fileName = Path.Combine(Path.GetDirectoryName(sourceFileName), fileName);
                    var error = new PascalABCCompiler.Errors.CommonCompilerError(
                        diagnostic.message ?? "Ошибка компиляции .NET 10",
                        Path.GetFullPath(fileName),
                        Math.Max(1, diagnostic.line), Math.Max(1, diagnostic.column));
                    errors.Add(error);
                    WriteMessage(error.ToString());
                }
            if (errors.Count == 0)
            {
                string message = string.IsNullOrWhiteSpace(response.message)
                    ? "Компилятор .NET 10 не создал выходной файл." : response.message;
                errors.Add(new PascalABCCompiler.Errors.Error(message));
                WriteMessage("Ошибка: " + message);
            }
            if (!workbench.MainForm.IsDisposed)
                workbench.ErrorsListWindow.ShowErrorsSync(errors, true);
        }

        private List<Net10EditorSource> OpenEditorSources()
        {
            var documents = new HashSet<ICodeFileDocument>();
            FindDocuments(workbench.MainForm, documents);
            // Floating docking windows are separate Forms, not MainForm children.
            foreach (Form form in Application.OpenForms) FindDocuments(form, documents);
            return Net10EditorDocument.ReadOpen(documents, workbench.ServiceContainer.DocumentService);
        }

        private static void FindDocuments(Control root, HashSet<ICodeFileDocument> documents)
        {
            if (root.IsDisposed) return;
            var document = root as ICodeFileDocument;
            if (document != null) documents.Add(document);
            foreach (Control child in root.Controls) FindDocuments(child, documents);
        }

        private Net10ControllerClient CreateClient()
        {
            var settings = Net10RuntimeSettings.Load(AppDomain.CurrentDomain.BaseDirectory);
            dotnetPath = settings.DotnetPath;
            var result = new Net10ControllerClient(settings.RuntimeDirectory, dotnetPath);
            result.WorkerRestarted += message =>
            {
                var form = workbench.MainForm;
                if (form.IsDisposed || form.Disposing) return;
                try
                {
                    form.BeginInvoke(new Action(() => WriteMessage("[.NET 10]" + message)));
                }
                catch (InvalidOperationException) { }
            };
            return result;
        }

        private void WriteMessage(string message)
        {
            if (workbench.MainForm.IsDisposed) return;
            workbench.ServiceContainer.OperationsService.AddTextToCompilerMessagesSync(message + Environment.NewLine);
        }

        private void SetEnabled(bool enabled)
        {
            if (workbench.MainForm.IsDisposed) return;
            SetItemEnabled(compileItem, enabled);
            SetItemEnabled(runItem, enabled);
        }

        private static void SetItemEnabled(PluginGUIItem item, bool enabled)
        {
            var button = item.toolStripButton as ToolStripButton;
            if (button != null) button.Enabled = enabled;
            var menu = item.menuItem as ToolStripMenuItem;
            if (menu != null) menu.Enabled = enabled;
        }
    }
}
