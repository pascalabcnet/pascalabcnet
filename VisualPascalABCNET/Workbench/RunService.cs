// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
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
        internal RunManager RunnerManager;
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
        //IUserOptions UserOptions;
        //IVisualEnvironmentCompiler VisualEnvironmentCompiler;

        public WorkbenchRunService()
        {
            //UserOptions = WorkbenchServiceFactory.Workbench.UserOptions;
            Workbench = WorkbenchServiceFactory.Workbench;
            //VisualEnvironmentCompiler = WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler;
            RunnerManager = new RunManager(ReadStringRequest);
            RunnerManager.Exited += new RunManager.RunnerManagerActionDelegate(RunnerManager_Exited);
            RunnerManager.Starting += new RunManager.RunnerManagerActionDelegate(RunnerManager_Started);
            RunnerManager.OutputStringReceived += new RunManager.TextRecivedDelegate(RunnerManager_OutputStringReceived);
            RunnerManager.RunnerManagerUnhanledRuntimeException += new RunManager.RunnerManagerUnhanledRuntimeExceptionDelegate(RunnerManager_RunnerManagerUnhanledRuntimeException);
            DesignerService = WorkbenchServiceFactory.DesignerService;
            this.DebuggerManager = WorkbenchServiceFactory.DebuggerManager;
            BuildService = WorkbenchServiceFactory.BuildService;
            DebuggerOperationsService = WorkbenchServiceFactory.DebuggerOperationsService;
            DocumentService = WorkbenchServiceFactory.DocumentService;
        }

        public bool Run(bool Debug)
        {
            Workbench.UserOptions.RedirectConsoleIO = Debug;

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

        public bool Run(ICodeFileDocument tabPage, bool forDebugging, bool startWithGoto, bool needFirstBreakpoint)
        {
            bool attachdbg = forDebugging || startWithGoto || needFirstBreakpoint; //|| WorkbenchServiceFactory.DebuggerManager.HasBreakpoints();
            bool fictive_attach = false;
            BuildService.CompilerOptions.UseDllForSystemUnits = false;
            BuildService.CompilerOptions.OutputFileType = PascalABCCompiler.CompilerOptions.OutputType.ConsoleApplicaton;
            if (ProjectFactory.Instance.ProjectLoaded)
                tabPage = DocumentService.GetTabPageForMainFile();
            if (attachdbg && !Workbench.UserOptions.AlwaysAttachDebuggerAtStart)
                fictive_attach = !startWithGoto && !needFirstBreakpoint && !DebuggerManager.HasBreakpoints();
            WorkbenchServiceFactory.OperationsService.ClearOutputTextBoxForTabPage(tabPage);
            Workbench.ErrorsListWindow.ClearErrorList();
            DesignerService.GenerateAllDesignersCode();
            //this.Refresh();
            //            new System.Threading.Thread(new System.Threading.ThreadStart(RunCompile)).Start();;
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
                if (Workbench.UserOptions.DeleteEXEAfterExecute && Workbench.UserOptions.UseDllForSystemUnits && !startWithGoto && !needFirstBreakpoint)
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
                                DebuggerManager.show_debug_tabs = true;
                                DebuggerManager.CurrentBreakpoint = DebuggerManager.AddBreakPoint(DocumentService.CurrentCodeFileDocument.FileName, DocumentService.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.Caret.Line + 1, false);
                                DebuggerManager.AddGoToBreakPoint(DebuggerManager.CurrentBreakpoint);
                            }
                            else if (needFirstBreakpoint)
                            {
                                DebuggerManager.show_debug_tabs = true;
                                if (Workbench.VisualEnvironmentCompiler.Compiler.VarBeginOffset != 0)
                                {
                                    DebuggerManager.SetFirstBreakpoint(tabPage.FileName, Workbench.VisualEnvironmentCompiler.Compiler.VarBeginOffset);
                                    DebuggerManager.AddGoToBreakPoint(tabPage.FileName, Workbench.VisualEnvironmentCompiler.Compiler.BeginOffset);
                                }
                                else
                                    DebuggerManager.SetFirstBreakpoint(tabPage.FileName, Workbench.VisualEnvironmentCompiler.Compiler.BeginOffset);
                            }
                            else
                                DebuggerManager.show_debug_tabs = false;
                        }
                        RunActiveTabPage = false;
                        try
                        {
                            RunTabsAdd(Workbench.VisualEnvironmentCompiler.Compiler.CompilerOptions.OutputFileName, tabPage);
                            RunnerManager.Run(Workbench.VisualEnvironmentCompiler.Compiler.CompilerOptions.OutputFileName, Workbench.UserOptions.RedirectConsoleIO, ModeName, RunWithPause, Path.GetDirectoryName(Workbench.VisualEnvironmentCompiler.Compiler.CompilerOptions.OutputFileName), RunArgumentsTable[tabPage.FileName.ToLower()], attachdbg, fictive_attach);
                        }
                        catch (System.Exception e)
                        {
                            throw;
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
            return false;
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
            /*else
            {
                DebugStop();
                return true;
            }*/
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
