using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace VisualPascalABCPlugins
{
    // The legacy IDE discovers plugins by this class-name suffix and IWorkbench constructor.
    public sealed class CompileNet10_VisualPascalABCPlugin : IExtendedVisualPascalABCPlugin, IIdeCommandHandler
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
        private IWorkbenchCommandService commandService;
        private IDisposable commandRegistration;
        private ToolStripMenuItem platformItem;
        private Net10LanguageDocuments languageDocuments;
        private Net10Completion languageCompletion;
        private Net10Hover languageHover;
        private Net10Signature languageSignature;
        private Net10Navigation languageNavigation;
        private ToolStripStatusLabel languageStatus;
        private readonly Timer languageGate = new Timer { Interval = 500 };

        public CompileNet10_VisualPascalABCPlugin(IWorkbench workbench)
        {
            if (workbench == null)
                throw new ArgumentNullException(nameof(workbench));

            this.workbench = workbench;
            Application.ApplicationExit += (sender, e) => languageDocuments?.AbortOnApplicationExit();
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
                languageGate.Dispose();
                languageCompletion?.Dispose();
                languageHover?.Dispose();
                languageSignature?.Dispose();
                languageNavigation?.Dispose();
                languageDocuments?.Dispose();
                languageStatus?.Dispose();
                commandRegistration?.Dispose();
                if (commandService != null) commandService.LegacyServicesSuspended = false;
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
            commandService = workbench.ServiceContainer as IWorkbenchCommandService;
            var programMenu = workbench.MainForm.MainMenuStrip?.Items["mrProgram"] as ToolStripMenuItem;
            if (commandService == null || programMenu == null) return; // Hosts without the standard menu retain prototype buttons.
            platformItem = new ToolStripMenuItem(".NET 10")
            {
                Name = "CompileNet10Platform",
                ToolTipText = "Компилировать и запускать на .NET 10. Выбор действует до закрытия среды."
            };
            platformItem.Click += (sender, e) =>
            {
                if (compiling || runner != null || workbench.ServiceContainer.RunService.IsRun() || workbench.DebuggerManager.IsRunning)
                {
                    WriteMessage("Остановите программу перед переключением платформы.");
                    return;
                }
                platformItem.Checked = !platformItem.Checked;
                commandService.LegacyServicesSuspended = platformItem.Checked;
                status.Start(platformItem.Checked ? ".NET 10" : ".NET 4.7.2");
                UpdateLanguageActivity();
            };
            commandRegistration = commandService.RegisterCommandHandler(this);
            programMenu.DropDownItems.Add(new ToolStripSeparator());
            programMenu.DropDownItems.Add(platformItem);
            programMenu.DropDownOpening += (sender, e) => platformItem.Enabled =
                !compiling && runner == null && !workbench.ServiceContainer.RunService.IsRun() && !workbench.DebuggerManager.IsRunning;
            foreach (var item in new[] { compileItem, runItem, stopItem })
            {
                if (item.toolStripButton is ToolStripItem button) button.Visible = false;
                if (item.menuItem is ToolStripItem menu) menu.Visible = false;
            }
            var documentEvents = workbench.ServiceContainer.DocumentService as IWorkbenchDocumentEvents;
            if (documentEvents != null)
            {
                var strips = workbench.MainForm.Controls.Find("statusStrip1", true);
                if (strips.Length == 1 && strips[0] is StatusStrip strip)
                {
                    languageStatus = new ToolStripStatusLabel { Name = "Net10LanguageServerStatus", Visible = false };
                    strip.Items.Add(languageStatus);
                }
                languageDocuments = new Net10LanguageDocuments(documentEvents, () =>
                {
                    var settings = Net10RuntimeSettings.Load(AppDomain.CurrentDomain.BaseDirectory);
                    return new Net10LanguageClient(settings.RuntimeDirectory, settings.DotnetPath,
                        PascalABCCompiler.StringResourcesLanguage.CurrentTwoLetterISO);
                }, ShowLanguageStatus);
                languageCompletion = new Net10Completion(workbench.MainForm, documentEvents, languageDocuments,
                    () => platformItem.Checked && workbench.UserOptions.AllowCodeCompletion && workbench.UserOptions.CodeCompletionDot);
                languageHover = new Net10Hover(workbench.MainForm, documentEvents, languageDocuments,
                    () => platformItem.Checked && workbench.UserOptions.AllowCodeCompletion && workbench.UserOptions.CodeCompletionHint);
                languageSignature = new Net10Signature(workbench.MainForm, documentEvents, languageDocuments,
                    () => platformItem.Checked && workbench.UserOptions.AllowCodeCompletion && workbench.UserOptions.CodeCompletionParams);
                languageCompletion.Opening += languageSignature.CloseHints;
                languageNavigation = new Net10Navigation(documentEvents, languageDocuments,
                    () => platformItem.Checked && workbench.UserOptions.AllowCodeCompletion, Navigate);
                // Only check the user's enable setting; document text is event-driven, never polled.
                languageGate.Tick += (sender, e) => UpdateLanguageActivity();
                languageGate.Start();
            }
        }

        private void UpdateLanguageActivity()
        {
            bool selected = platformItem?.Checked == true;
            if (languageStatus != null) languageStatus.Visible = selected;
            bool enabled = selected && workbench.UserOptions.AllowCodeCompletion;
            languageDocuments?.SetActive(enabled);
            languageCompletion?.SetActive(enabled && workbench.UserOptions.CodeCompletionDot);
            languageHover?.SetActive(enabled && workbench.UserOptions.CodeCompletionHint);
            languageSignature?.SetActive(enabled && workbench.UserOptions.CodeCompletionParams);
            languageNavigation?.SetActive(enabled);
            if (selected && !enabled && languageStatus != null) languageStatus.Text = "LSP: отключён в настройках";
        }

        private void Navigate(List<Net10NavigationTarget> targets)
        {
            languageSignature?.CloseHints();
            if (targets.Count == 1)
            {
                var target = targets[0];
                if (target.MetadataText != null)
                    workbench.ServiceContainer.FileService.OpenTabWithText(target.MetadataTitle + " [.NET 10]", target.MetadataText);
                workbench.VisualEnvironmentCompiler.ExecuteSourceLocationAction(
                    new PascalABCCompiler.SourceLocation(target.FileName, target.Line, target.Column, target.Line, target.Column), SourceLocationAction.GotoBeg);
            }
            else if (workbench.MainForm is VisualPascalABC.Form1 form)
            {
                var symbols = new List<VisualPascalABC.SymbolsViewerSymbol>();
                foreach (var target in targets)
                    if (target.FileName != null)
                        symbols.Add(new VisualPascalABC.SymbolsViewerSymbol(
                            new PascalABCCompiler.SourceLocation(target.FileName, target.Line, target.Column, target.Line, target.Column),
                            VisualPascalABC.CodeCompletionProvider.ImagesProvider.IconNumberGotoText));
                bool previous = form.FindSymbolResults.showInThread;
                try { form.FindSymbolResults.showInThread = false; form.ShowFindResults(symbols); }
                finally { form.FindSymbolResults.showInThread = previous; }
            }
        }

        private void ShowLanguageStatus(string text)
        {
            var form = workbench.MainForm;
            if (form.IsDisposed || form.Disposing) return;
            try
            {
                form.BeginInvoke(new Action(() =>
                {
                    if (languageStatus != null && !languageStatus.IsDisposed &&
                        platformItem?.Checked == true && workbench.UserOptions.AllowCodeCompletion)
                        languageStatus.Text = text;
                }));
            }
            catch (InvalidOperationException) { }
        }

        public bool TryExecute(VisualEnvironmentCompilerAction action, object context)
        {
            if (action == VisualEnvironmentCompilerAction.Stop && runner != null)
            {
                runner.Stop();
                return true;
            }
            if (platformItem?.Checked != true) return false;
            if (action == VisualEnvironmentCompilerAction.Stop) return false;
            if (action != VisualEnvironmentCompilerAction.Build && action != VisualEnvironmentCompilerAction.Run) return false;
            if (compiling || runner != null) return true; // Consumed: never fall back to the legacy compiler while busy.
            if (context is IdeRunCommand request)
            {
                if (request.DebugRequested)
                {
                    WriteMessage("Отладчик .NET 10 пока не подключён. Используйте обычный запуск.");
                    return true;
                }
                if (request.Document != null)
                    workbench.ServiceContainer.DocumentService.CurrentCodeFileDocument = request.Document;
            }
            Execute(action == VisualEnvironmentCompilerAction.Run,
                action == VisualEnvironmentCompilerAction.Build && context is bool rebuild && rebuild);
            return true;
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

        private async void Execute(bool run, bool rebuild = false)
        {
            if (compiling) return;
            IDisposable preparation = null;
            status.BeginOperation();
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
                var display = new Net10CompilerDisplay(key => PascalABCCompiler.StringResources.Get("VP_VEC_" + key));
                // Await each UI delivery so Ready cannot arrive after Run/exception status.
                var response = await client.CompileAsync(fileName, outputDirectory, "__RedirectIOMode", sources,
                    value => DisplayCompilerEventAsync(display, value), rebuild);
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
                        response = await client.CompileAsync(fileName, outputDirectory, "__RedirectIOMode", sources,
                            value => DisplayCompilerEventAsync(display, value), rebuild);
                    }
                }
                if (response.success)
                {
                    ShowDiagnostics(response, fileName, run);
                    if (!display.HasResult)
                    {
                        WriteMessage("Готово: " + response.outputFile);
                        status.Update("Компиляция .NET 10 прошла успешно");
                    }
                    if (run && !workbench.MainForm.IsDisposed)
                    {
                        string outputFile = response.outputFile;
                        // A PCU/DLL is a successful compilation, not a runnable
                        // application. Preserve the success status and use the
                        // same localized warning as the legacy Run command.
                        string warningKey = Net10EditorDocument.GetRunWarningResourceKey(outputFile);
                        if (warningKey != null)
                        {
                            MessageBox.Show(PascalABCCompiler.StringResources.Get("VP_MF_" + warningKey),
                                PascalABCCompiler.StringResources.Get("!WARNING"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
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
                            program.SendInputAsync, program.Stop, lifecycle.WriteOutput))
                        {
                            runner = program;
                            runningOutputFile = Path.GetFullPath(outputFile);
                            outputSession = console;
                            SetItemEnabled(stopItem, true);
                            int exitCode = await program.RunAsync(console.Append, console.RequestInput,
                                lifecycle.Started, error => console.ReportException(error, lifecycle));
                            await console.FlushAsync();
                            // Both output streams were queued before this UI continuation.
                            if (exitCode != 0 && !program.WasStopped)
                            {
                                status.Update("Программа .NET 10 завершилась с ошибкой (код " + exitCode + ")");
                                WriteMessage("Программа .NET 10 завершилась с кодом " + exitCode + ".");
                            }
                        }
                    }
                }
                else
                {
                    if (!display.HasResult) status.Update("Ошибка компиляции .NET 10");
                    ShowDiagnostics(response, fileName, run);
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

        private void ShowDiagnostics(Net10CompileResponse response, string sourceFileName, bool run)
        {
            var errors = Net10Diagnostics.Create(response, sourceFileName);
            foreach (var error in errors)
                if (!(error is PascalABCCompiler.Errors.CompilerWarning))
                    WriteMessage(error is PascalABCCompiler.Errors.CommonCompilerError ? error.ToString() : "Ошибка: " + error.Message);
            if (errors.Count > 0 && !workbench.MainForm.IsDisposed)
                workbench.ErrorsListWindow.ShowErrorsSync(errors, Net10Diagnostics.ChangeViewTab(response, run));
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
