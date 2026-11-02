using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using Timer = System.Windows.Forms.Timer;
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
        private bool restoringLanguagePackages;
        private CancellationTokenSource preparationCancellation;
        private IWorkbenchCommandService commandService;
        private IDisposable commandRegistration;
        private ToolStripMenuItem platformItem;
        private ToolStripMenuItem publishItem;
        private Net10LanguageDocuments languageDocuments;
        private Net10Completion languageCompletion;
        private Net10Hover languageHover;
        private Net10Signature languageSignature;
        private Net10Navigation languageNavigation;
        private Net10DocumentSymbols languageSymbols;
        private readonly Timer languageGate = new Timer { Interval = 500 };

        public CompileNet10_VisualPascalABCPlugin(IWorkbench workbench)
        {
            if (workbench == null)
                throw new ArgumentNullException(nameof(workbench));

            this.workbench = workbench;
            Application.ApplicationExit += (sender, e) => languageDocuments?.AbortOnApplicationExit();
            status = new Net10StatusDisplay(workbench.MainForm,
                key => PascalABCCompiler.StringResources.Get("VP_VEC_" + key));
            status.SetPackageRestoreRequired(() => platformItem?.Checked == true &&
                workbench.UserOptions.AllowCodeCompletion && languageDocuments?.NeedsPackageRestore(
                    workbench.ServiceContainer.DocumentService.CurrentCodeFileDocument) == true, RestoreLanguagePackages);
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
                languageSymbols?.Dispose();
                languageDocuments?.Dispose();
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
                SelectPlatform(!platformItem.Checked);
            };
            commandRegistration = commandService.RegisterCommandHandler(this);
            programMenu.DropDownItems.Add(new ToolStripSeparator());
            programMenu.DropDownItems.Add(platformItem);
            publishItem = Net10MenuItems.CreatePublishItem();
            publishItem.Click += (sender, e) => Execute(false, publish: true);
            programMenu.DropDownItems.Add(publishItem);
            programMenu.DropDownOpening += (sender, e) =>
            {
                bool available = !compiling && !restoringLanguagePackages && runner == null &&
                    !workbench.ServiceContainer.RunService.IsRun() && !workbench.DebuggerManager.IsRunning;
                platformItem.Enabled = available;
                publishItem.Enabled = available && platformItem.Checked;
            };
            foreach (var item in new[] { compileItem, runItem, stopItem })
            {
                if (item.toolStripButton is ToolStripItem button) button.Visible = false;
                if (item.menuItem is ToolStripItem menu) menu.Visible = false;
            }
            var documentEvents = workbench.ServiceContainer.DocumentService as IWorkbenchDocumentEvents;
            if (documentEvents != null)
            {
                languageDocuments = new Net10LanguageDocuments(documentEvents, () =>
                {
                    var settings = Net10RuntimeSettings.Load(AppDomain.CurrentDomain.BaseDirectory);
                    return new Net10LanguageClient(settings.RuntimeDirectory, settings.DotnetPath,
                        PascalABCCompiler.StringResourcesLanguage.CurrentTwoLetterISO, manualNuGetRestore: true);
                }, ShowLanguageStatus);
                languageDocuments.RestoreProgressChanged += value =>
                {
                    var form = workbench.MainForm;
                    if (form.IsDisposed || form.Disposing) return;
                    try { form.BeginInvoke(new Action(() =>
                    {
                        if (platformItem.Checked && workbench.UserOptions.AllowCodeCompletion)
                            status.ShowRestoreProgress("language", value, CancelLanguageRestore);
                    })); }
                    catch (InvalidOperationException) { }
                };
                languageCompletion = new Net10Completion(workbench.MainForm, documentEvents, languageDocuments,
                    () => platformItem.Checked && workbench.UserOptions.AllowCodeCompletion && workbench.UserOptions.CodeCompletionDot);
                languageHover = new Net10Hover(workbench.MainForm, documentEvents, languageDocuments,
                    () => platformItem.Checked && workbench.UserOptions.AllowCodeCompletion && workbench.UserOptions.CodeCompletionHint);
                languageSignature = new Net10Signature(workbench.MainForm, documentEvents, languageDocuments,
                    () => platformItem.Checked && workbench.UserOptions.AllowCodeCompletion && workbench.UserOptions.CodeCompletionParams);
                languageCompletion.Opening += languageSignature.CloseHints;
                languageNavigation = new Net10Navigation(documentEvents, languageDocuments,
                    () => platformItem.Checked && workbench.UserOptions.AllowCodeCompletion, Navigate);
                languageSymbols = new Net10DocumentSymbols(documentEvents,
                    workbench.ServiceContainer.DocumentService, languageDocuments);
                // Only check the user's enable setting; document text is event-driven, never polled.
                languageGate.Tick += (sender, e) => UpdateLanguageActivity();
                languageGate.Start();
            }
            // Only the modern distribution ships this plugin. The ordinary IDE is unchanged.
            // Apply once, after all modern services are connected; manual changes remain session-only.
            try
            {
                var settings = Net10RuntimeSettings.Load(AppDomain.CurrentDomain.BaseDirectory);
                if (File.Exists(Path.Combine(settings.RuntimeDirectory, "PABCCompilerController.dll")) &&
                    File.Exists(Path.Combine(settings.RuntimeDirectory, "PABCCompilerWorker.dll")) &&
                    File.Exists(Path.Combine(settings.RuntimeDirectory, "PascalABCNet.LanguageServer.dll")) &&
                    Net10RuntimeAvailability.IsAvailable(settings.DotnetPath))
                    SelectPlatform(true);
            }
            catch (IOException) { } // A locked INI must not prevent startup in the ordinary mode.
            catch (UnauthorizedAccessException) { }
        }

        private void SelectPlatform(bool modern)
        {
            platformItem.Checked = modern;
            commandService.LegacyServicesSuspended = modern;
            status.StartReady(modern, PascalABCCompiler.StringResources.Get("VP_VEC_STATETEXT_READY"));
            UpdateLanguageActivity();
        }

        private void UpdateLanguageActivity()
        {
            bool selected = platformItem?.Checked == true;
            bool enabled = selected && workbench.UserOptions.AllowCodeCompletion;
            status.SetLanguageRestoreVisible(enabled && runner == null &&
                !workbench.ServiceContainer.RunService.IsRun() && !workbench.DebuggerManager.IsRunning);
            languageDocuments?.SetActive(enabled);
            if (!enabled) status.ClearRestores("language");
            languageCompletion?.SetActive(enabled && workbench.UserOptions.CodeCompletionDot);
            languageHover?.SetActive(enabled && workbench.UserOptions.CodeCompletionHint);
            languageSignature?.SetActive(enabled && workbench.UserOptions.CodeCompletionParams);
            languageNavigation?.SetActive(enabled);
            languageSymbols?.SetActive(selected, enabled && workbench.UserOptions.ShowQuickClassBrowserPanel);
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
                    if (platformItem?.Checked == true && workbench.UserOptions.AllowCodeCompletion)
                    {
                        if (text.StartsWith("LSP: ошибка", StringComparison.Ordinal) ||
                            text.StartsWith("LSP: связь потеряна", StringComparison.Ordinal))
                            status.ClearRestores("language");
                        status.ShowLanguageStatus(text);
                    }
                }));
            }
            catch (InvalidOperationException) { }
        }

        private async void CancelLanguageRestore()
        {
            try { if (languageDocuments != null) await languageDocuments.CancelPackageRestoreAsync(); }
            catch (Exception error) { ShowLanguageStatus("LSP: ошибка — " + error.Message); }
        }

        private async void RestoreLanguagePackages()
        {
            if (restoringLanguagePackages || compiling || runner != null || languageDocuments == null) return;
            var document = workbench.ServiceContainer.DocumentService.CurrentCodeFileDocument;
            if (document == null || !languageDocuments.NeedsPackageRestore(document)) return;
            restoringLanguagePackages = true;
            status.SetManualRestoreActive(true);
            SetEnabled(false);
            try
            {
                var result = await languageDocuments.RestorePackagesAsync(document) as System.Collections.Generic.IDictionary<string, object>;
                bool success = result != null && (result.TryGetValue("success", out var value) || result.TryGetValue("Success", out value)) &&
                    value is bool restored && restored;
                status.ClearRestores("language");
                string detail = result != null && (result.TryGetValue("message", out var message) || result.TryGetValue("Message", out message))
                    ? message as string : null;
                status.Start(success ? PascalABCCompiler.StringResources.Get("VP_VEC_NUGET_RESTORED") :
                    string.Format(PascalABCCompiler.StringResources.Get("VP_VEC_NUGET_RESTORE_FAILED{0}"), detail ?? ""));
            }
            catch (OperationCanceledException)
            {
                status.ClearRestores("language");
                status.Start(PascalABCCompiler.StringResources.Get("VP_VEC_NUGET_RESTORE_CANCELLED"));
            }
            catch (Exception error)
            {
                status.ClearRestores("language");
                status.Start(string.Format(PascalABCCompiler.StringResources.Get("VP_VEC_NUGET_RESTORE_FAILED{0}"), error.Message));
            }
            finally
            {
                restoringLanguagePackages = false;
                status.SetManualRestoreActive(false);
                SetEnabled(true);
                UpdateLanguageActivity();
            }
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
                action == VisualEnvironmentCompilerAction.Build && context is bool rebuild && rebuild,
                redirectConsoleIO: !(context is IdeRunCommand runCommand) || runCommand.RedirectConsoleIO);
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

        private async void Execute(bool run, bool rebuild = false, bool publish = false,
            bool redirectConsoleIO = true)
        {
            if (compiling || restoringLanguagePackages) return;
            IDisposable preparation = null;
            string cleanupSource = null, cleanupOutput = null;
            string cleanupOutputRoot = null;
            string publishOutputDirectory = null;
            bool deleteExe = false, deletePdb = false;
            bool standalone = run && !redirectConsoleIO;
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
                string outputRoot = workbench.UserOptions.UseOutputDirectory
                    ? Path.GetFullPath(workbench.UserOptions.OutputDirectory) : null;
                string outputDirectory = publish
                    ? publishOutputDirectory = Net10OutputCleanup.CreatePublishDirectory()
                    : Net10OutputCleanup.GetDirectory(fileName, outputRoot);
                var runService = workbench.ServiceContainer.RunService;
                if (publish && runService.IsRun())
                    throw new InvalidOperationException("Сначала остановите обычную программу IDE.");
                if (run && runService.IsRun())
                    throw new InvalidOperationException("Сначала остановите обычную программу IDE.");
                if (run && !standalone)
                    workbench.ServiceContainer.OperationsService.ClearOutputTextBoxForTabPage(document);
                string arguments = run && runService.HasRunArgument(fileName)
                    ? runService.GetRunArgument(fileName) : "";
                if (!standalone) workbench.CompilerConsoleWindow.ClearConsole();
                if (client == null) client = CreateClient();
                compiling = true;
                preparationCancellation = new CancellationTokenSource();
                status.SetCompilationActive(true);
                SetEnabled(false);
                workbench.ErrorsListWindow.ClearErrorList();
                var display = new Net10CompilerDisplay(key => PascalABCCompiler.StringResources.Get("VP_VEC_" + key));
                // Await each UI delivery so Ready cannot arrive after Run/exception status.
                var response = await client.CompileAsync(fileName, outputDirectory,
                    publish ? null : standalone ? "__RunMode" : "__RedirectIOMode", sources,
                    value => DisplayCompilerEventAsync(display, value), rebuild,
                    libraryOutputDirectory: outputRoot ?? Path.GetDirectoryName(fileName),
                    debug: !publish && workbench.ServiceContainer.BuildService.CompilerOptions.Debug,
                    publish: publish,
                    language: PascalABCCompiler.StringResourcesLanguage.CurrentTwoLetterISO,
                    cancellation: preparationCancellation.Token);
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
                        outputDirectory = Net10OutputCleanup.GetDirectory(fileName, outputRoot);
                        arguments = runService.HasRunArgument(fileName) ? runService.GetRunArgument(fileName) : "";
                        if (!standalone)
                            workbench.ServiceContainer.OperationsService.ClearOutputTextBoxForTabPage(document);
                        display = new Net10CompilerDisplay(key => PascalABCCompiler.StringResources.Get("VP_VEC_" + key));
                        response = await client.CompileAsync(fileName, outputDirectory,
                            standalone ? "__RunMode" : "__RedirectIOMode", sources,
                            value => DisplayCompilerEventAsync(display, value), rebuild,
                            libraryOutputDirectory: outputRoot ?? Path.GetDirectoryName(fileName),
                            debug: workbench.ServiceContainer.BuildService.CompilerOptions.Debug,
                            language: PascalABCCompiler.StringResourcesLanguage.CurrentTwoLetterISO,
                            cancellation: preparationCancellation.Token);
                    }
                }
                if (response.success)
                {
                    if (languageDocuments?.NeedsPackageRestore(document) == true)
                        await languageDocuments.RefreshPackagesAsync(document);
                    ShowDiagnostics(response, fileName, run);
                    if (publish)
                    {
                        WriteMessage(string.Format(PascalABCCompiler.StringResources.Get("VP_VEC_PUBLISH_SUCCESS{0}"),
                            response.outputFile));
                        status.Update(PascalABCCompiler.StringResources.Get("VP_VEC_PUBLISH_READY"));
                    }
                    else if (!display.HasResult)
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
                            Path.GetDirectoryName(fileName), arguments, externalRun.PrepareExternalArguments,
                            redirectConsoleIO, standalone && workbench.UserOptions.PauseInRunModeIfConsole,
                            PascalABCCompiler.StringResourcesLanguage.CurrentLCID))
                        using (var lifecycle = externalRun.RegisterExternalRun(document, outputFile, program.Stop))
                        {
                            runner = program;
                            runningOutputFile = Path.GetFullPath(outputFile);
                            SetItemEnabled(stopItem, true);
                            int exitCode;
                            bool runtimeExceptionReported = false;
                            if (standalone)
                                exitCode = await program.RunAsync(null, null, lifecycle.Started);
                            else
                            {
                                using (var console = new Net10OutputSession(workbench, document,
                                    program.SendInputAsync, program.Stop, lifecycle.WriteOutput))
                                {
                                    outputSession = console;
                                    exitCode = await program.RunAsync(console.Append, console.RequestInput,
                                        lifecycle.Started, error =>
                                        {
                                            runtimeExceptionReported = true;
                                            console.ReportException(error, lifecycle);
                                        });
                                    await console.FlushAsync();
                                }
                                outputSession = null;
                            }
                            // Only a completed run (including Stop/runtime errors), never Compile or a failed launch.
                            cleanupSource = fileName;
                            cleanupOutput = Path.GetFullPath(outputFile);
                            cleanupOutputRoot = outputRoot;
                            deleteExe = workbench.UserOptions.DeleteEXEAfterExecute;
                            deletePdb = workbench.UserOptions.DeletePDBAfterExecute;
                            if (exitCode != 0 && !program.WasStopped)
                            {
                                status.Update("Ошибка выполнения .NET 10");
                                if (!runtimeExceptionReported)
                                    WriteMessage("Программа .NET 10 завершилась с кодом " + exitCode +
                                        ". Сообщение об исключении не получено.");
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
            catch (OperationCanceledException) when (preparationCancellation?.IsCancellationRequested == true)
            {
                status.ClearRestores("compiler");
                status.Update(PascalABCCompiler.StringResources.Get("VP_VEC_NUGET_CANCELLED"));
            }
            catch (Exception error)
            {
                status.Update("Ошибка .NET 10: " + error.Message);
                WriteMessage((publish ? "Publish .NET 10: " : run ? "Run .NET 10: " : "Compile .NET 10: ") + error.Message);
            }
            finally
            {
                status.ClearRestores("compiler");
                status.SetCompilationActive(false);
                preparationCancellation?.Dispose();
                preparationCancellation = null;
                // Process/session handles are disposed by now. Keep commands disabled until cleanup finishes.
                if (cleanupOutput != null && deleteExe && deletePdb)
                    await System.Threading.Tasks.Task.Run(() =>
                        Net10OutputCleanup.TryDelete(cleanupSource, cleanupOutput, deleteExe, deletePdb, cleanupOutputRoot));
                if (publishOutputDirectory != null)
                    await System.Threading.Tasks.Task.Run(() =>
                        Net10OutputCleanup.TryDeletePublishDirectory(publishOutputDirectory));
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
                        if (!form.IsDisposed)
                        {
                            if (value.state == "NuGetRestoreProgress")
                                status.ShowRestoreProgress("compiler", new Net10RestoreProgress {
                                    operationId = value.nuGetOperationId, stage = value.nuGetStage,
                                    packageId = value.fileName, installedCount = value.nuGetInstalledCount,
                                    elapsedMilliseconds = (long)value.elapsedMilliseconds
                                }, () => preparationCancellation?.Cancel());
                            display.Handle(value, WriteMessage, status.Update);
                        }
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
            if (publishItem != null) publishItem.Enabled = enabled && platformItem?.Checked == true;
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
