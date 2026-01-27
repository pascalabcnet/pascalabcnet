// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using VisualPascalABCPlugins;

namespace VisualPascalABC
{
    public partial class WorkbenchRunService : IWorkbenchRunService
    {
        string RedirectIOModeModuleName = "__RedirectIOMode";
        string RunModeModuleName = "__RunMode";
        string RedirectIOModeName = "[REDIRECTIOMODE]";
        string RunModeName = "[RUNMODE]";
        private bool TerminateAllPrograms = false;
        public RunManager RunnerManager;
        private IWorkbench Workbench;
        private IWorkbenchDesignerService DesignerService;
        private IWorkbenchBuildService BuildService;
        private IWorkbenchDocumentService DocumentService;
        private IWorkbenchDebuggerOperationsService DebuggerOperationsService;
        private DebugHelper DebuggerManager;
        private Dictionary<string, ICodeFileDocument> RunTabs = new Dictionary<string, ICodeFileDocument>();
        private Dictionary<ICodeFileDocument, string> ReadRequests = new Dictionary<ICodeFileDocument, string>();
        private Dictionary<string, string> RunArgumentsTable = new Dictionary<string, string>();
        bool RunActiveTabPage = false;

        public event ChangeArgsBeforeRunDelegate ChangeArgsBeforeRun
        {
            add
            {
                RunnerManager.ChangeArgsBeforeRun += value;
            }
            remove
            {
                RunnerManager.ChangeArgsBeforeRun -= value;
            }
        }
        // SSM 16/05/22 - добавил чтобы иметь возможность добавлять обработчики во внешнем плагине
        public event RunnerManagerActionDelegate Starting
        {
            add
            {
                RunnerManager.Starting += value;
            }
            remove
            {
                RunnerManager.Starting -= value;
            }
        }
        public event RunnerManagerActionDelegate Exited
        {
            add
            {
                RunnerManager.Exited += value;
            }
            remove
            {
                RunnerManager.Exited -= value;
            }
        }

        public WorkbenchRunService()
        {
            Workbench = WorkbenchServiceFactory.Workbench;
            RunnerManager = new RunManager(ReadStringRequest);
            RunnerManager.Exited += new RunnerManagerActionDelegate(RunnerManager_Exited);
            RunnerManager.Starting += new RunnerManagerActionDelegate(RunnerManager_Started);
            RunnerManager.OutputStringReceived += new RunManager.TextRecivedDelegate(RunnerManager_OutputStringReceived);
            RunnerManager.RunnerManagerUnhanledRuntimeException += new RunManager.RunnerManagerUnhanledRuntimeExceptionDelegate(RunnerManager_RunnerManagerUnhanledRuntimeException);
            DesignerService = WorkbenchServiceFactory.DesignerService;
            this.DebuggerManager = WorkbenchServiceFactory.DebuggerManager;
            BuildService = WorkbenchServiceFactory.BuildService;
            DebuggerOperationsService = WorkbenchServiceFactory.DebuggerOperationsService;
            DocumentService = WorkbenchServiceFactory.DocumentService;
        }

        public bool Run(bool RedirectConsoleIO)
        {
            Workbench.UserOptions.RedirectConsoleIO = RedirectConsoleIO;

            if (Workbench.UserOptions.RedirectConsoleIO)
                if (!DebuggerManager.IsRunning)
                    return Run(DocumentService.CurrentCodeFileDocument, true, false, false);
                else if (DebuggerManager.IsRunAndInThisTabPage())
                {
                    DebuggerManager.Continue();
                    return true;
                }
            return Run(DocumentService.CurrentCodeFileDocument);
        }

        public bool Run(bool forDebugging, bool startWithGoto, bool need_first_brpt)
        {
            return Run(DocumentService.CurrentCodeFileDocument, forDebugging, startWithGoto, need_first_brpt);
        }

        public void Run(string fileName, bool redirectIO, string ModeName, bool RunWithPause, string WorkingDirectory, string CommandLineArguments, bool attachDebugger, bool fictive_attach)
        {
            RunnerManager.Run(fileName,
                Workbench.UserOptions.RedirectConsoleIO, RedirectIOModeName, RunWithPause,
                Workbench.VisualEnvironmentCompiler.Compiler.CompilerOptions.SourceFileDirectory,
               CommandLineArguments, attachDebugger, fictive_attach);
        }

        public bool Run(ICodeFileDocument tabPage, bool forDebugging, bool startWithGoto, bool needFirstBreakpoint)
        {
            lock (o)
            {
                bool attachdbg = forDebugging || startWithGoto || needFirstBreakpoint; //|| WorkbenchServiceFactory.DebuggerManager.HasBreakpoints();
                bool fictive_attach = false;
                BuildService.CompilerOptions.UseDllForSystemUnits = false;
                BuildService.CompilerOptions.OutputFileType = PascalABCCompiler.CompilerOptions.OutputType.ConsoleApplicaton;
                if (ProjectFactory.Instance.ProjectLoaded)
                    tabPage = DocumentService.GetTabPageForMainFile();
                if (attachdbg && !Workbench.UserOptions.AlwaysAttachDebuggerAtStart)
                    fictive_attach = !startWithGoto && !needFirstBreakpoint && !DebuggerManager.HasBreakpoints() || !CodeCompletion.CodeCompletionController.IntellisenseAvailable();
                WorkbenchServiceFactory.OperationsService.ClearOutputTextBoxForTabPage(tabPage);
                Workbench.ErrorsListWindow.ClearErrorList();
                DesignerService.GenerateAllDesignersCode();
                string runtimeModule;
                string ModeName;
                bool RunWithPause = false;
                if (Workbench.UserOptions.RedirectConsoleIO || forDebugging)
                {
                    runtimeModule = RedirectIOModeModuleName;
                    ModeName = RedirectIOModeName;
                    if (Workbench.UserOptions.UseDllForSystemUnits && !Workbench.UserOptions.PABCDllChecked)
                    {
                        if (typeof(System.Runtime.CompilerServices.ExtensionAttribute).Assembly != typeof(int).Assembly || PascalABCCompiler.Compiler.get_assembly_path("PABCRtl.dll", false) == null)
                            Workbench.UserOptions.UseDllForSystemUnits = false;
                        Workbench.UserOptions.PABCDllChecked = true;
                    }
                    // SSM 09.02.20 - UseDllForSystemUnits включать когда флаг установлен - безо всяких доп. условий
                    if (/*Workbench.UserOptions.DeleteEXEAfterExecute &&*/ Workbench.UserOptions.UseDllForSystemUnits && !startWithGoto && !needFirstBreakpoint)
                        BuildService.CompilerOptions.UseDllForSystemUnits = true;
                }
                else
                {
                    runtimeModule = RunModeModuleName;
                    ModeName = RunModeName;
                    RunWithPause = Workbench.UserOptions.PauseInRunModeIfConsole;
                }
                bool debug = BuildService.CompilerOptions.Debug;
                if (forDebugging)
                {
                    BuildService.CompilerOptions.Debug = true;
                    BuildService.CompilerOptions.ForDebugging = true;
                    Workbench.UserOptions.RedirectConsoleIO = true;
                }
                string OutputFileName = null;

                if (BuildService.BeforeCompile != null)
                {
                    BuildService.BeforeCompile(tabPage.FileName);
                }
                
                if (!forDebugging)
                {
                    if (!ProjectFactory.Instance.ProjectLoaded)
                        OutputFileName = BuildService.Compile(tabPage.FileName, false, runtimeModule, true, Workbench.UserOptions.RedirectConsoleIO);
                    else
                        OutputFileName = BuildService.Compile(ProjectFactory.Instance.CurrentProject, false, runtimeModule, true, Workbench.UserOptions.RedirectConsoleIO);
                }
                else
                {
                    if (!ProjectFactory.Instance.ProjectLoaded)
                        OutputFileName = BuildService.Compile(tabPage.FileName, false, runtimeModule, true, true);
                    else
                        OutputFileName = BuildService.Compile(ProjectFactory.Instance.CurrentProject, false, runtimeModule, true, true);
                }
                if (RunWithPause)
                    RunWithPause = RunWithPause && Workbench.VisualEnvironmentCompiler.Compiler.CompilerOptions.OutputFileType == PascalABCCompiler.CompilerOptions.OutputType.ConsoleApplicaton;
                if (OutputFileName != null)
                {
                    switch (Workbench.VisualEnvironmentCompiler.Compiler.CompilerOptions.OutputFileType)
                    {
                        case PascalABCCompiler.CompilerOptions.OutputType.ClassLibrary:
                            if (DocumentService.ActiveCodeFileDocument != null && DocumentService.ActiveCodeFileDocument != DocumentService.CurrentCodeFileDocument && !DocumentService.ActiveCodeFileDocument.Run)
                                if (!RunActiveTabPage)
                                {
                                    RunActiveTabPage = true;
                                    return Run(DocumentService.ActiveCodeFileDocument, forDebugging, startWithGoto, needFirstBreakpoint);
                                }
                            MessageBox.Show(Form1StringResources.Get("RUN_DLL_WARNING_TEXT"), PascalABCCompiler.StringResources.Get("!WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            RunActiveTabPage = false;
                            break;
                        case PascalABCCompiler.CompilerOptions.OutputType.PascalCompiledUnit:
                            if (DocumentService.ActiveCodeFileDocument != null && DocumentService.ActiveCodeFileDocument != DocumentService.CurrentCodeFileDocument && !DocumentService.ActiveCodeFileDocument.Run)
                                if (!RunActiveTabPage)
                                {
                                    RunActiveTabPage = true;
                                    return Run(DocumentService.ActiveCodeFileDocument, forDebugging, startWithGoto, needFirstBreakpoint);
                                }
                            RunActiveTabPage = false;
                            MessageBox.Show(Form1StringResources.Get("RUN_PCU_WARNING_TEXT"), PascalABCCompiler.StringResources.Get("!WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        case PascalABCCompiler.CompilerOptions.OutputType.ConsoleApplicaton:
                        case PascalABCCompiler.CompilerOptions.OutputType.WindowsApplication:
                            if (forDebugging)
                            {
                                BuildService.CompilerOptions.Debug = debug;
                                BuildService.CompilerOptions.ForDebugging = false;
                                if (startWithGoto)
                                {
                                    DebuggerManager.ShowDebugTabs = true;
                                    DebuggerManager.UpdateBreakpoints();
                                    DebuggerManager.CurrentBreakpoint = DebuggerManager.AddBreakPoint(DocumentService.CurrentCodeFileDocument.FileName, DocumentService.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.Caret.Line + 1, false);
                                    DebuggerManager.AddGoToBreakPoint(DebuggerManager.CurrentBreakpoint);
                                }
                                else if (needFirstBreakpoint)
                                {
                                    DebuggerManager.ShowDebugTabs = true;
                                    DebuggerManager.UpdateBreakpoints();
                                    if (Workbench.VisualEnvironmentCompiler.Compiler.VarBeginOffset != 0)
                                    {
                                        DebuggerManager.SetFirstBreakpoint(tabPage.FileName, Workbench.VisualEnvironmentCompiler.Compiler.VarBeginOffset);
                                        DebuggerManager.AddGoToBreakPoint(tabPage.FileName, Workbench.VisualEnvironmentCompiler.Compiler.BeginOffset);
                                    }
                                    else
                                        DebuggerManager.SetFirstBreakpoint(tabPage.FileName, Workbench.VisualEnvironmentCompiler.Compiler.BeginOffset);
                                }
                                else
                                {
                                    DebuggerManager.UpdateBreakpoints();
                                    DebuggerManager.ShowDebugTabs = false;
                                }

                            }
                            RunActiveTabPage = false;
                            try
                            {
                                RunTabsAdd(Workbench.VisualEnvironmentCompiler.Compiler.CompilerOptions.OutputFileName, tabPage);
                                RunnerManager.Run(Workbench.VisualEnvironmentCompiler.Compiler.CompilerOptions.OutputFileName,
                                    Workbench.UserOptions.RedirectConsoleIO, ModeName, RunWithPause,
                                    // Path.GetDirectoryName(Workbench.VisualEnvironmentCompiler.Compiler.CompilerOptions.OutputFileName), // SSM 21.05.19 - исправил. Теперь текущим является каталог, в котором расположен исходник. 
                                    // Если компилировать программы на сетевом диске, а exe создавать локально, то работает шустро
                                    Workbench.VisualEnvironmentCompiler.Compiler.CompilerOptions.SourceFileDirectory,
                                    RunArgumentsTable[tabPage.FileName.ToLower()], attachdbg, fictive_attach);
                            }
                            catch (System.Exception e)
                            {
                                // SSM 22/04/19 - исправляю вылет оболочки при отсутствии exe файла
                                // this.RunnerManager_Exited(OutputFileName); // - это всё равно не срабатывает. Кнопки оказываются в заблокированном состоянии
                                
                                WorkbenchServiceFactory.OperationsService.AddTextToOutputWindowSync(OutputFileName, "Произошла непредвиденная ошибка. Вероятно, на диске отсутствует .exe-файл. Повторите запуск");
                                //throw;
                                return false;
                            }
                            if (!ProjectFactory.Instance.ProjectLoaded)
                                DocumentService.ActiveCodeFileDocument = tabPage;
                            if (forDebugging)
                            {
                                DebuggerOperationsService.AddDebugPage(tabPage);
                            }
                            WorkbenchStorage.SetCurrentTabPageIfWriteToOutputWindow = Workbench.UserOptions.RedirectConsoleIO;
                            //if (UserOptions1.ConsoleOutput)
                            //AddTextToOutputWindow("Ïðîãðàììà çàïóùåíà");
                            return true;
                    }
                }
                RunActiveTabPage = false;
                return false;
            }
        }

        public bool Run(ICodeFileDocument tabPage)
        {
            return Run(tabPage, false, false, false);
        }

        private bool __runInThreadDebug;
        private void __runInThread(object state)
        {
            Run(__runInThreadDebug);
        }

        private void runInThread(bool debug)
        {
            __runInThreadDebug = debug;
            if (!ThreadPool.QueueUserWorkItem(new WaitCallback(__runInThread)))
                Run(debug);
        }

        public bool Stop() // Èíòåðåñíî, çà÷åì ñäåëàëè private
        {
            if (RunnerManager.IsRun(Workbench.CurrentEXEFileName))
            {
                RunnerManager.Stop(Workbench.CurrentEXEFileName);
                return true;
            }
            return true;
        }

        void RunTabsAdd(string name, ICodeFileDocument p)
        {
            name = Tools.FileNameToLower(name);
            if (RunTabs.ContainsKey(name))
                RunTabs[name] = p;
            else
                RunTabs.Add(name, p);
        }

        public void SendInputTextToProcess()
        {
            ReadRequests.Remove(DocumentService.CurrentCodeFileDocument);
            RunnerManager.WritelnStringToProcess(Workbench.CurrentEXEFileName, Workbench.OutputWindow.InputTextBoxText);
            Workbench.OutputWindow.AppendTextToOutputBox(Environment.NewLine);
            WorkbenchServiceFactory.OperationsService.SendNewLineToInputTextBox();
            Workbench.OutputWindow.InputPanelVisible = false;
        }

        public bool IsRun()
        {
            return RunnerManager.IsRun();
        }
        
        public bool IsRun(string FileName)
        {
            return RunnerManager.IsRun(FileName);
        }

        public void Stop(string FileName)
        {
            RunnerManager.Stop(FileName);
        }

        public bool HasRunArgument(string FileName)
        {
            return RunArgumentsTable.ContainsKey(FileName);
        }

        public void AddRunArgument(string FileName, string argument)
        {
            RunArgumentsTable[FileName] = argument;
        }

        public ICodeFileDocument GetRunTab(string FileName)
        {
            return RunTabs[FileName];
        }

        public string GetRunArgument(string FileName)
        {
            return RunArgumentsTable[FileName];
        }

        public void KillAll()
        {
            TerminateAllPrograms = true;
            RunnerManager.KillAll();
            TerminateAllPrograms = false;
        }
    }
}
