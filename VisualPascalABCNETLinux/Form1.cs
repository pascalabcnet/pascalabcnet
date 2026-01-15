// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using VisualPascalABCPlugins;
using System.Threading;
using System.Runtime.InteropServices;
using VisualPascalABC.OptionsContent;
using System.Security.Permissions;

//using ICSharpCode.FormsDesigner;
//using ICSharpCode.SharpDevelop.Gui;
//using ICSharpCode.Core;
//using ICSharpCode.SharpDevelop;

namespace VisualPascalABC
{
    delegate void SetTextDelegate(string Text);
    delegate void SetFileNameAndTextDelegate(string ExeFileName, string Text);

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    
    public partial class Form1 : Form, VisualPascalABCPlugins.IWorkbench, VisualPascalABCPlugins.IWorkbenchDocumentService
    {
        static string VersionInTitle(string s)
        {
            // VersionInTitle(RevisionClass.FullVersion)
            var i = s.LastIndexOf('.');
            return s.Substring(0, i);
        }

        private const string MainFormTitle = "PascalABC.NET";
        private static bool DesignerUseable = true;
		private static bool ProjectsUseable = true;
#if (DEBUG)
        private static bool UseImmediate = true;
#else
		private static bool UseImmediate = false;
#endif

#if (DEBUG)
        private const bool DebugModus = true;
#else
        private const bool DebugModus = false;
#endif
		private string MainFormText = MainFormTitle;
        private string FTSFormat = "{0} [{1}]";

        internal BottomDockContentForm BottomActiveContent = null;

       
        private ICSharpCode.TextEditor.Document.FileSyntaxModeProvider FileSyntaxProvider;
        
        public PascalABCCompiler.Errors.ErrorsStrategyManager ErrorsManager = new PascalABCCompiler.Errors.ErrorsStrategyManager(PascalABCCompiler.Errors.ErrorsStrategy.FirstSemanticAndSyntax);

        internal bool DebuggerVisible = true; // почему то влияет на Intellisense!!!!! Не отключать!

        private int FormLeft, FormTop, FormWidth, FormHeight;

        //public ToolBoxForm ToolBoxWindow = null;
        //public PropertiesForm PropertiesWindow = null;
        internal OutputWindowForm OutputWindow;
        internal ErrorsListWindowForm ErrorsListWindow;
        internal FindSymbolsResultWindowForm FindSymbolsResultWindow;
        internal ImmediateWindow ImmediateWindow;
        internal CompilerConsoleWindowForm CompilerConsoleWindow;
        internal DebugVariablesListWindowForm DebugVariablesListWindow;
        internal DebugWatchListWindowForm DebugWatchListWindow;
        internal DisassemblyWindow DisassemblyWindow;
        internal ProjectExplorerForm ProjectExplorerWindow = null;
        public WeifenLuo.WinFormsUI.Docking.DockPane BottomPane;
        public WeifenLuo.WinFormsUI.Docking.DockPane ProjectPane;
        FSWatcherService WatcherService = new FSWatcherService();

        private bool LoadComplete = false;
        internal SourceLocationAction ErrorCursorPosStrategy = SourceLocationAction.GotoBeg;

        private CodeFileDocumentControl activeTabPage = null;

        PascalABCCompiler.Errors.ErrorsStrategyManager VisualPascalABCPlugins.IWorkbench.ErrorsManager
        {
            get
            {
                return ErrorsManager;
            }
        }

        VisualPascalABCPlugins.IOutputWindow VisualPascalABCPlugins.IWorkbench.OutputWindow
        {
            get
            {
                return OutputWindow;
            }
        }

        VisualPascalABCPlugins.IDisassemblyWindow VisualPascalABCPlugins.IWorkbench.DisassemblyWindow
        {
            get
            {
                return DisassemblyWindow;
            }
        }

        VisualPascalABCPlugins.ICompilerConsoleWindow VisualPascalABCPlugins.IWorkbench.CompilerConsoleWindow
        {
            get
            {
                return CompilerConsoleWindow;
            }
        }

        VisualPascalABCPlugins.IErrorListWindow VisualPascalABCPlugins.IWorkbench.ErrorsListWindow
        {
            get
            {
                return ErrorsListWindow;
            }
        }

        ICodeFileDocument VisualPascalABCPlugins.IWorkbenchDocumentService.ActiveCodeFileDocument
        {
            get
            {
                return ActiveCodeFileDocument;
            }
            set
            {
                ActiveCodeFileDocument = value as CodeFileDocumentControl;
            }
        }

        ICodeFileDocument VisualPascalABCPlugins.IWorkbenchDocumentService.LastSelectedTab
        {
            get
            {
                return LastSelectedTab;
            }
            
        }

        void VisualPascalABCPlugins.IWorkbench.BeginInvoke(Delegate del, params object[] args)
        {
            this.BeginInvoke(del, args);
        }

        internal System.Drawing.Image BuildImage
        {
        	get
        	{
        		return miBuild.Image;
        	}
        }
        
        internal System.Drawing.Image RebuildImage
        {
        	get
        	{
        		return miRebuild.Image;
        	}
        }
        
        internal System.Drawing.Image RunImage
        {
        	get
        	{
        		return miRun.Image;
        	}
        }

        internal System.Drawing.Image NewFileImage
        {
            get
            {
                return miNew.Image;
            }
        }

        internal System.Drawing.Image OpenFileImage
        {
            get
            {
                return miOpen.Image;
            }
        }

        internal System.Drawing.Image NewFormImage
        {
            get
            {
                return mADDFORMToolStripMenuItem.Image;
            }
        }       

        public VisualEnvironmentCompiler VisualEnvironmentCompiler = null;
        
        public CompilerForm CompilerForm1;      
               
        
        public AboutBox AboutBox1;
        public FindReplaceForm FindForm;
        public GotoLineForm GotoLineForm;
        public FindReplaceForm ReplaceForm;
        
        internal NavigationManager NavigationManager;
        public bool SaveCanceled = false;
        private WorkbenchServiceContainer serviceContainer;

        //private System.Windows.Forms.DataGridViewImageColumn PictColumn;

        OptionsContentEngine optionsContentEngine;

        // Для Linux сделать все Debug-кнопки неактивными
        public void SetDebugButtonsInvisible()
        {
            this.SuspendLayout();
            this.toolStripPanel.SuspendLayout();
            this.menuStrip1.SuspendLayout();

            this._toolStripMenuItem10.Visible = false;
            this.mSTEPOVERToolStripMenuItem.Visible = false;
            this.mSTEPToolStripMenuItem.Visible = false;
            this.mSTEPINToolStripMenuItem.Visible = false;
            this.mRUNTOCURToolStripMenuItem.Visible = false;
            this.StepOverButton.Visible = false;
            this.StepIntoButton.Visible = false;
            this.tsShowDebugVariablesListWindow.Visible = false;
            this.tsShowDebugWatchListWindow.Visible = false;
            this.tsImmediateWindow.Visible = false;
            this.menuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        public Form1()
        {
            if (Tools.IsUnix())
            {
                ProjectsUseable = false;
                DesignerUseable = false;
                //DebuggerVisible = false;
            }

            PascalABCCompiler.StringResourcesLanguage.LoadDefaultConfig();
            
            //if (PascalABCCompiler.StringResourcesLanguage.AccessibleLanguages.Count > 0)
            //    PascalABCCompiler.StringResourcesLanguage.CurrentLanguageName = PascalABCCompiler.StringResourcesLanguage.AccessibleLanguages[0];
           
            InitializeComponent();
            ScreenScale.FirstCalcScale(this); // !!
            ICSharpCode.TextEditor.ScreenScale.FirstCalcScale(this);

            if (!DebuggerVisible)
            {
                tsShowDebugVariablesListWindow.Visible = false;
                tsShowDebugWatchListWindow.Visible = false;
                tsImmediateWindow.Visible = false;
            }

            //toolStrip1.Visible = false;
            //menuStrip1.Visible = false;
            tsAutoInsertCode.Visible = false;
            MainDockPanel.Theme = new WeifenLuo.WinFormsUI.Docking.VS2005Theme();
            BottomDockPanel.Theme = new WeifenLuo.WinFormsUI.Docking.VS2005Theme();
            //MainDockPanel.ActiveDocumentChanged += (oo, ee) => MessageBox.Show("MainDockPanel.ActiveDocumentChanged");
            //MainDockPanel.Click += (oo, ee) => MessageBox.Show("MainDockPanel.Click");


            VisualPABCSingleton.MainForm = this;
            WorkbenchStorage.MainProgramThread = System.Threading.Thread.CurrentThread;
            //images init
            this.miNewProject.Image = new System.Drawing.Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VisualPascalABC.Resources.Icons.16x16.NewProjectIcon.png"));
            this.mADDFILEToolStripMenuItem.Image = miNew.Image;
            this.mADDEXISTFILEToolStripMenuItem.Image = miOpen.Image;
            this.mADDFORMToolStripMenuItem.Image = new System.Drawing.Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VisualPascalABC.Resources.Icons.16x16.Form.png"));

            //toolStripPanel.Size = new Size(toolStripPanel.Size.Width, toolStrip1.Height);
            toolStripPanel.AutoSize = true;
            var gr = CreateGraphics();
            if (gr.DpiX >= 96*2)
            {
                this.SuspendLayout();
                this.toolStripPanel.SuspendLayout();
                this.menuStrip1.SuspendLayout();

                toolStrip1.ImageScalingSize = new Size(32, 32);
                menuStrip1.ImageScalingSize = new Size(32, 32);
                toolStripPanel.Size = new Size(toolStripPanel.Size.Width, 38);
                this.toolStripPanel.ResumeLayout(false);
                this.menuStrip1.ResumeLayout(false);
                this.ResumeLayout(false);
                this.PerformLayout();
            }
            else if (gr.DpiX >= 144)
            {
                this.SuspendLayout();
                this.toolStripPanel.SuspendLayout();
                this.menuStrip1.SuspendLayout();

                toolStrip1.ImageScalingSize = new Size(24, 24);
                menuStrip1.ImageScalingSize = new Size(24, 24);
                toolStripPanel.Size = new Size(toolStripPanel.Size.Width, 30);
                this.toolStripPanel.ResumeLayout(false);
                this.menuStrip1.ResumeLayout(false);
                this.ResumeLayout(false);
                this.PerformLayout();
            }
            // !!
            gr.Dispose();
            serviceContainer = new WorkbenchServiceContainer();

            if (DebuggerVisible)
            {
            	InitForm();
                this.StepOutButton.Visible = false;
                PlayPauseButtonsVisibleInPanel = PlayPauseButtonsVisibleInPanel;
                SetDebugButtonsEnabled(false);
            }

            // Для Linux сделать все Debug-кнопки неактивными
            // SetDebugButtonsInvisible();

            AddOwnedForm(CompilerForm1 = new CompilerForm());
            AddOwnedForm(AboutBox1 = new AboutBox());
            AddOwnedForm(FindForm = new FindReplaceForm(FindReplaceForm.FormType.Find));
            AddOwnedForm(ReplaceForm = new FindReplaceForm(FindReplaceForm.FormType.Replace));
            AddOwnedForm(GotoLineForm = new GotoLineForm());
           
            LastOpenFiles = new List<string>();
            LastOpenProjects = new List<string>();

            WorkbenchServiceFactory.BuildService.CompilerOptions.Debug = true;
            WorkbenchServiceFactory.BuildService.CompilerOptions.OutputFileType = PascalABCCompiler.CompilerOptions.OutputType.ConsoleApplicaton;

            LocalizeControls();

            tsatConsoleApplication.Tag = PascalABCCompiler.CompilerOptions.OutputType.ConsoleApplicaton;
            tsatWindowsApplication.Tag = PascalABCCompiler.CompilerOptions.OutputType.WindowsApplication;
            tsatDll.Tag = PascalABCCompiler.CompilerOptions.OutputType.ClassLibrary;

            SelectAppType(WorkbenchServiceFactory.BuildService.CompilerOptions.OutputFileType);

            //this.Width = 800;
            //this.Height = 600;
            UserOptions = new UserOptions();
            SetFiltersAndHighlighting();
            
            FormLeft = this.Left; FormTop = this.Top; FormWidth = this.Width; FormHeight = this.Height;
			//MainDockPanel.DockRightPortion = 0.22;
            //MainDockPanel.DockLeftPortion = 0.22;
                
            //UserOptions.OutputDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName)+"\\Output";
            UserOptions.OutputDirectory = Constants.DefaultOutputDirectory;

            PlayPauseButtonsVisibleInPanel = false;

            WorkbenchStorage.StandartDirectories = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            WorkbenchStorage.StandartDirectories.Add(Constants.SystemDirectoryIdent, System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName));

            //---------------------------------------------
            BottomDockPanel.Size = new Size(1920, 1080);
            RestoreDesktop();
            //---------------------------------------------

            OutputWindow.CloseButton = false;
            ErrorsListWindow.CloseButton = false;
            CompilerConsoleWindow.CloseButton = false;

            menuStrip1.Font = new Font(menuStrip1.Font.Name, 9.0f);

            // SSM 06.06.22 - это только для Линукса!!!
            MainDockPanel.AllowEndUserDocking = false;
            BottomDockPanel.AllowEndUserDocking = false; 

            LoadOptions();

            HelpExamplesDirectory = PascalABCCompiler.Tools.ReplaceAllKeys(Constants.HelpExamplesDirectory, WorkbenchStorage.StandartDirectories);
            HelpTutorialExamplesDirectory = PascalABCCompiler.Tools.ReplaceAllKeys(Constants.HelpTutorialExamplesDirectory, WorkbenchStorage.StandartDirectories);

            UpdateUserOptions();
            //ErrorsListWindow.Resized();
            FindSymbolsResultWindow.Resized();

            UpdateOutputWindowVisibleButtons();

            if (WorkbenchStorage.WorkingDirectory == null || true)
            {
                WorkbenchStorage.WorkingDirectoryInOptionsFile = Constants.DefaultWorkingDirectory;
                WorkbenchStorage.WorkingDirectory = PascalABCCompiler.Tools.ReplaceAllKeys(Constants.DefaultWorkingDirectory, WorkbenchStorage.StandartDirectories);
            }
            if (Path.GetDirectoryName(Application.ExecutablePath).ToLower() != Environment.CurrentDirectory.ToLower())
                WorkbenchStorage.WorkingDirectory = Environment.CurrentDirectory;
            WorkbenchStorage.StandartDirectories.Add(Constants.WorkingDirectoryIdent, WorkbenchStorage.WorkingDirectory);
            openFileDialog1.InitialDirectory = WorkbenchStorage.WorkingDirectory;

            if (WorkbenchStorage.LibSourceDirectory == null)
                WorkbenchStorage.LibSourceDirectory = PascalABCCompiler.Tools.ReplaceAllKeys(Constants.DefaultLibSourceDirectory, WorkbenchStorage.StandartDirectories);
            WorkbenchStorage.StandartDirectories.Add(Constants.LibSourceDirectoryIdent, WorkbenchStorage.LibSourceDirectory);
            AddSearchDebugPath(WorkbenchStorage.LibSourceDirectory);

            if (UserOptions.UseOutputDirectory)
                WorkbenchStorage.StandartDirectories.Add(Constants.OutputDirectoryIdent, UserOptions.OutputDirectory);
            else
                WorkbenchStorage.StandartDirectories.Add(Constants.OutputDirectoryIdent, null);

            RunManager RunnerManager = (WorkbenchServiceFactory.RunService as WorkbenchRunService).RunnerManager;
            VisualEnvironmentCompiler = new VisualEnvironmentCompiler(
                this.BeginInvoke, SetCompilingButtonsEnabled, SetDebugButtonsEnabled2, SetStateText,
                AddTextToCompilerMessagesSync, miPlugins, toolStrip1,
                ExecuteSourceLocationAction, ExecuteVisualEnvironmentCompilerAction, ErrorsManager, RunnerManager,
                WorkbenchServiceFactory.DebuggerManager, UserOptions, WorkbenchStorage.StandartDirectories, OpenDocuments, this);
            if (OnEnvorimentEvent != null)
                OnEnvorimentEvent(EnvorimentEvent.VisualEnvironmentCompilerCreated);

            NavigationManager = new NavigationManager(ExecuteSourceLocationAction);
            NavigationManager.StateChanged += new NavigationManager.NavigationManagerStateChanged(NavigationManager_StateChanged);

            // загрузка всех парсеров и других составляющих языков  EVA
            Languages.Integration.LanguageIntegrator.LoadAllLanguages();

            string newFileName = InstNameNewProgramm(MainDockPanel);
            //---------------------------------------------
            MainDockPanel.Size = new Size(1920, 1080);
            BottomDockPanel.Size = new Size(1920, 1080);
            AddNewProgramToTab(MainDockPanel, newFileName);
            //AddNewProgramToTab(BottomDockPanel, "gdsfj");
            //---------------------------------------------

            AddOptionsContent();
            Application.AddMessageFilter(this);

            if (DesignerUseable)
            {
                //miProperties.Visible = true;
                //miToolBox.Visible = true;
                //miNewAdv.Visible = true;
                miUndo.Visible = true;
            }
			this.mRPROJECTToolStripMenuItem.Visible = false;
            if (ProjectsUseable)
            {
            	miNewProject.Visible = true;
            	miRecentProjects.Visible = true;
            	//miCloseProject.Visible = true;
            	miOpenProject.Visible = true;
            }
            if (!UseImmediate)
            {
            	tsImmediateWindow.Visible = false;
            }
            this.mNEWASPToolStripMenuItem.Visible = DebugModus;
            //if (!Tools.IsUnix())
            //    AddDesignerSidebars();
            WorkbenchServiceFactory.CodeCompletionParserController.RegisterFileForParsing(newFileName);
        }

        public class ProbaBForm : BottomDockContentForm { }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Куча констант - VS2005DockPaneStrip.cs строка 98
            // _DocumentIconGapBottom сделать 5 вместо 2
            // _DocumentTabMaxWidth 600 сделать и в Win версии!!!

            MainDockPanel.Theme.Skin.DockPaneStripSkin.TextFont = menuStrip1.Font;
            BottomDockPanel.Theme.Skin.DockPaneStripSkin.TextFont = menuStrip1.Font;
            statusStrip1.Font = menuStrip1.Font;
            //var name = BottomDockPanel.Theme.Skin.DockPaneStripSkin.TextFont.Name;
            //var sz = (float)9.0;
            //MainDockPanel.Theme.Skin.DockPaneStripSkin.TextFont = new Font(name, sz);
            //BottomDockPanel.Theme.Skin.DockPaneStripSkin.TextFont = new Font(name, sz);

            BottomDockPanel.AllowEndUserDocking = false;

            //CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.TextArea.KeyEventHandler += TextArea_KeyEventHandler;

            init = true;
            foreach (string FileName in VisualPascalABCProgram.CommandLineArgs)
            {
                if (Path.GetExtension(FileName) == ".pabcproj")
                    WorkbenchServiceFactory.ProjectService.OpenProject(FileName);
                else
                    WorkbenchServiceFactory.FileService.OpenFile(FileName, null);
            }
            if (FileNameToWait != null)
            {
                if (Path.GetExtension(FileNameToWait) == ".pabcproj")
                    WorkbenchServiceFactory.ProjectService.OpenProject(FileNameToWait);
                else
                    WorkbenchServiceFactory.FileService.OpenFile(FileNameToWait, null);
            }
            SetStopEnabled(false);
            CompilingButtonsEnabled = CloseButtonsEnabled = SaveAllButtonsEnabled = SaveButtonsEnabled = false;
            if (DebuggerVisible)
                SetDebugButtonsEnabled2(false);
            SetCompilingButtonsEnabled(false);

            HelpFileName = PascalABCCompiler.Tools.ReplaceAllKeys(Constants.HelpFileName, WorkbenchStorage.StandartDirectories);
            DotNetHelpFileName = PascalABCCompiler.Tools.ReplaceAllKeys(Constants.DotNetHelpFileName, WorkbenchStorage.StandartDirectories);
            if (!File.Exists(HelpFileName))
                tsHelp.Visible = false;

            HelpExamplesFileName = PascalABCCompiler.Tools.ReplaceAllKeys(Constants.HelpExamplesFileName, WorkbenchStorage.StandartDirectories);
            HelpExamplesMapFileName = PascalABCCompiler.Tools.ReplaceAllKeys(Constants.HelpExamplesMapFileName, WorkbenchStorage.StandartDirectories);

            InitHelpProgramsDictionary(); // SSM

            NavigBackButtonsEnabled = NavigForwButtonsEnabled = false;

            try
            {
                if (!Directory.Exists(WorkbenchStorage.WorkingDirectory))
                    Directory.CreateDirectory(WorkbenchStorage.WorkingDirectory);
            }
            catch (Exception exc)
            {
                AddTextToCompilerMessages(Form1StringResources.Get("MSGTYPE") + String.Format(Form1StringResources.Get("ERROR_CREATING_WORKDIR_{0}_{1}"), WorkbenchStorage.WorkingDirectory, exc.Message) + Environment.NewLine);
            }
            try
            {
                if (!Directory.Exists(UserOptions.OutputDirectory))
                    Directory.CreateDirectory(UserOptions.OutputDirectory);
            }
            catch (Exception exc)
            {
                AddTextToCompilerMessages(Form1StringResources.Get("MSGTYPE") + String.Format(Form1StringResources.Get("ERROR_CREATING_WORKDIR_{0}_{1}"), UserOptions.OutputDirectory, exc.Message) + Environment.NewLine);
            }
            CodeCompletionActionsManager.templateManager = new CodeTemplateManager();
        }

        private void LocalizeControls()
        {
            Form1StringResources.SetTextForAllControls(this);
            Form1StringResources.SetTextForAllControls(this.contextMenuStrip1);
            Form1StringResources.SetTextForAllControls(this.cm_Designer);
            Form1StringResources.SetTextForAllControls(this.cmEditor);
            Form1StringResources.SetTextForAllControls(this.cmBreakpointCondition);
            Form1StringResources.SetTextForAllControls(this.RunArgumentsForm);
            PascalABCCompiler.StringResources.SetTextForAllObjects(AboutBox1, "VP_ABOUTBOXFORM_");
            PascalABCCompiler.StringResources.SetTextForAllObjects(FindForm, "VP_FINDFORM_");
            PascalABCCompiler.StringResources.SetTextForAllObjects(ReplaceForm, "VP_REPLACEFORM_");
            PascalABCCompiler.StringResources.SetTextForAllObjects(GotoLineForm, "VP_GOTOLINEFORM_");
        }

        private void SetFiltersAndHighlighting()
        {
            string hdir = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName), "Highlighting");
            if (Directory.Exists(hdir))
            {
                FileSyntaxProvider = new ICSharpCode.TextEditor.Document.FileSyntaxModeProvider(hdir+Path.DirectorySeparatorChar);
                ICSharpCode.TextEditor.Document.HighlightingManager.Manager.AddSyntaxModeFileProvider(FileSyntaxProvider);
                string Filter = "", AllFilter = "";
                foreach (ICSharpCode.TextEditor.Document.SyntaxMode sm in FileSyntaxProvider.SyntaxModes)
                {
                    Filter = Tools.MakeFilter(Filter, sm.Name, sm.Extensions);
                    AllFilter = Tools.MakeAllFilter(AllFilter, sm.Name, sm.Extensions);
                }
                saveFileDialog1.Filter = openFileDialog1.Filter = Tools.FinishMakeFilter(Filter, AllFilter);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            WorkbenchServiceFactory.CodeCompletionParserController.StopParseThread();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string text = Text;
            //если запущены программы
            if (WorkbenchServiceFactory.RunService.IsRun())
            {
                //завершаем все программы
                this.Text = string.Format(FTSFormat, MainFormText, Form1StringResources.Get("FTS_KILLPROCESS"));
                WorkbenchServiceFactory.RunService.KillAll();
                this.Text = text;
            }

            WorkbenchServiceFactory.CodeCompletionParserController.StopParsing();

            //Сохраняем Файлы
            if (ProjectFactory.Instance.ProjectLoaded)
            {
                if (!WorkbenchServiceFactory.ProjectService.CloseProject())
                {
                    e.Cancel = true;
                    return;
                }
            }
            else if (!SaveAll(true))
            {
                e.Cancel = true;
                return;
            }


            //если стартовый поток запущен
            try
            {
                if (VisualEnvironmentCompiler.Starting())
                {
                    //начинаем процедуру отмены старта компилятора
                    this.Text = string.Format(FTSFormat, MainFormText, Form1StringResources.Get("FTS_CANCELLOADING"));
                    VisualEnvironmentCompiler.AbortStaring();
                }
                else
                    SaveOptions();
            }
            catch
            {

            }
            e.Cancel = false;
        }

        
        
        private void miClearOutputWindow_Click(object sender, EventArgs e)
        {
            var zoomf = OutputWindow.outputTextBox.ZoomFactor;
            OutputWindow.outputTextBox.Clear();
            OutputWindow.outputTextBox.ZoomFactor = zoomf;
        }

        public enum EnvorimentEvent {VisualEnvironmentCompilerCreated}
        public delegate void EnvorimentEventDelegate(EnvorimentEvent EnvorimentEvent);
        public event EnvorimentEventDelegate OnEnvorimentEvent;
        bool init = false;
        string FileNameToWait=null;
        
        void NavigationManager_StateChanged(NavigationManager sender)
        {
            NavigBackButtonsEnabled = NavigationManager.CanNavigateBackward;
            NavigForwButtonsEnabled = NavigationManager.CanNavigateForward;
        }
        
        private void OpenToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog(Path.GetDirectoryName(CurrentSourceFileName));
        }

        VisualPascalABCPlugins.IVisualEnvironmentCompiler VisualPascalABCPlugins.IWorkbench.VisualEnvironmentCompiler
        {
            get
            {
                return VisualEnvironmentCompiler;
            }
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e) //roman//
        {
            ExecUndo();
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e) //roman//
        {
            ExecRedo();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void SetStateText(string text)
        {
            toolStripStatusLabel5.Text = text;
            statusStrip1.Refresh();
        }
		
        public void Reset()
        {
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            MPanel_Resize(null, EventArgs.Empty);
            BPanel_Resize(null, EventArgs.Empty);

            if (_mainFormWindowStateMaximized)
                this.WindowState = FormWindowState.Maximized;

            ChangedSelectedTab();
            VisualEnvironmentCompiler.ChangeVisualEnvironmentState += new ChangeVisualEnvironmentStateDelegate(VisualEnvironmentCompiler_ChangeVisualEnvironmentState);
            VisualEnvironmentCompiler.RunStartingThread();

            LoadComplete = true;
        }

        private object ExecuteVisualEnvironmentCompilerAction(VisualEnvironmentCompilerAction Action,object obj)
        {
            switch (Action)
            {
                case VisualEnvironmentCompilerAction.Run:
                    return WorkbenchServiceFactory.RunService.Run(true);
                case VisualEnvironmentCompilerAction.Stop:
                    return WorkbenchServiceFactory.RunService.Stop();
                case VisualEnvironmentCompilerAction.Build:
                    return WorkbenchServiceFactory.BuildService.Build();
                case VisualEnvironmentCompilerAction.BuildUnit:
                    return WorkbenchServiceFactory.BuildService.Build(obj as string);
                case VisualEnvironmentCompilerAction.Rebuild:
                    return WorkbenchServiceFactory.BuildService.Rebuild();
                case VisualEnvironmentCompilerAction.OpenFile:
                    return WorkbenchServiceFactory.FileService.OpenFile((string)obj, null);
                case VisualEnvironmentCompilerAction.GetDirectory:
                    VisualEnvironmentCompiler.Compiler.CompilerOptions.StandardDirectories.TryGetValue((string)obj, out var s);
                    if (s != null)
                        return s;
                    WorkbenchStorage.StandartDirectories.TryGetValue((string)obj, out s);
                    return s;
                case VisualEnvironmentCompilerAction.PT4PositionCursorAfterTask: // SSM 09.11.19
                    {
                        var ta = CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.TextArea;
                        var d = ta.Document;
                        for (var i = 0; i<d.TotalNumberOfLines; i++)
                        {
                            var line = ICSharpCode.TextEditor.Document.TextUtilities.GetLineAsString(d, i);
                            if (line.Equals("  "))
                            {
                                var p = 1;
                                ta.Caret.Line = i;
                                ta.Caret.Column = 2;
                                return true;
                            }
                        }
                        return true;
                    }
                case VisualEnvironmentCompilerAction.GetCurrentSourceFileName:
                    return CurrentSourceFileName;
                case VisualEnvironmentCompilerAction.SetCurrentSourceFileTextFormatting:
                case VisualEnvironmentCompilerAction.SetCurrentSourceFileText:
                    {
                        string text = (string)obj;
                        string clipboard_text = Tools.GetTextFromClipboard();
                        CurrentSyntaxEditor.SelectAll();
                        Tools.CopyTextToClipboard(text);
                        CurrentSyntaxEditor.Paste(true);
                        Tools.CopyTextToClipboard(clipboard_text);
                        CurrentSyntaxEditor.SetText(text);
                    }
                    return true;
                case VisualEnvironmentCompilerAction.AddTextToCompilerMessages:
                    AddTextToCompilerMessagesSync((string)obj);
                    return true;
                case VisualEnvironmentCompilerAction.AddMessageToErrorListWindow:
                    ErrorsListWindow.ShowErrorsSync((List<PascalABCCompiler.Errors.Error>)obj,true);
                    return true;
                case VisualEnvironmentCompilerAction.SaveFile:
                    string FileName = Tools.FileNameToLower((string)obj);
                    if (!OpenDocuments.ContainsKey(FileName))
                        return false;
                    var doc = OpenDocuments[FileName];
                    if (doc.DocumentSavedToDisk)
                        SaveFileAs(doc, (string)obj);
                    //else
                    //    ExecuteSaveAs(doc);
                    return true;
            }
            return false;
        }

        void VisualEnvironmentCompiler_ChangeVisualEnvironmentState(VisualEnvironmentState State, object obj)
        {
            switch (State)
            {
                case VisualEnvironmentState.StartCompilerLoading:
                    this.Text = String.Format(FTSFormat,MainFormText,Form1StringResources.Get("FTS_LOADING"));
                    break;
                case VisualEnvironmentState.FinishCompilerLoading:
                    this.Text = String.Format("{0} {1}",MainFormText,PascalABCCompiler.Compiler.ShortVersion);
                    //this.Text = MainFormText;
                    openFileDialog1.Filter = saveFileDialog1.Filter = VisualEnvironmentCompiler.GetFilterForDialogs();
                    openProjectDialog.Filter = VisualEnvironmentCompiler.GetProjectFilterForDialogs();
                    VisualEnvironmentCompiler.Compiler.CompilerOptions = WorkbenchServiceFactory.BuildService.CompilerOptions;
                    break;
            }
        }
        
        public string CurrentSourceFileName
        {
            get { return CurrentCodeFileDocument.FileName; }
        }

        public string CurrentEXEFileName
        {
            get
            {
                string path = "";

                if (ProjectFactory.Instance.CurrentProject != null)
                {
                    if (!string.IsNullOrEmpty(ProjectFactory.Instance.CurrentProject.OutputDirectory))
                        return Path.Combine(ProjectFactory.Instance.CurrentProject.OutputDirectory, ProjectFactory.Instance.CurrentProject.OutputFileName);
                    return Path.Combine(ProjectFactory.Instance.CurrentProject.ProjectDirectory, ProjectFactory.Instance.CurrentProject.OutputFileName);
                }
                    

                if (UserOptions.UseOutputDirectory)
                    path = Path.Combine(UserOptions.OutputDirectory, Path.GetFileNameWithoutExtension(CurrentSourceFileName)) + ".exe";
                else
                    path = Path.ChangeExtension(CurrentSourceFileName, ".exe");
                if (!File.Exists(path) && ActiveCodeFileDocument != null)
                {
                    if (UserOptions.UseOutputDirectory)
                        path = Path.Combine(UserOptions.OutputDirectory, Path.GetFileNameWithoutExtension(ActiveCodeFileDocument.FileName)) + ".exe";
                    else
                        path = Path.ChangeExtension(ActiveCodeFileDocument.FileName, ".exe");
                }
                return path;
            }
        }

        private void OpenLastFile_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkbenchServiceFactory.FileService.OpenFile((sender as ToolStripMenuItem).Tag as string, null);         
        }

		private void OpenLastProject_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkbenchServiceFactory.ProjectService.OpenProject((sender as ToolStripMenuItem).Tag as string);         
        }
		
        private void toolStripButton19_Click(object sender, EventArgs e)
        {
            OutputWindow.outputTextBox.Text = "";
        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {
            CurrentCodeFileDocument.Font = fontDialog1.Font;
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            WorkbenchServiceFactory.BuildService.StartCompile(true);
        }
        
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            WorkbenchServiceFactory.BuildService.StartCompile(false);
        }
        
        private void miAboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1.ShowDialog();
        }

        private void CompileButton_EnabledChanged(object sender, EventArgs e)
        {
            //CompileMenuItem.Enabled = CompileButton.Enabled;
        }

        private void StartButton_EnabledChanged(object sender, EventArgs e)
        {
            //StartMenuItem.Enabled = StartButton.Enabled;
        }

        private void ReCompileButton_EnabledChanged(object sender, EventArgs e)
        {
            miRebuild.Enabled = ReCompileButton.Enabled;
        }

        private void miCurt_Click(object sender, EventArgs e)
        {
            ExecCut();
        }

        private void miCopy_Click(object sender, EventArgs e)
        {
            ExecCopy();
        }

        private void miPaste_Click(object sender, EventArgs e)
        {
            ExecPaste();
        }

        private void miStop_Click(object sender, EventArgs e)
        {
            WorkbenchServiceFactory.RunService.Stop();
        }

        private void miSave_Click(object sender, EventArgs e)
        {
            SaveSelFile(CurrentCodeFileDocument);                         
        }

        private void miSaveAll_Click_1(object sender, EventArgs e)
        {
            SaveAll(false);
        }

        private void miClose_Click(object sender, EventArgs e)
        {
            if (CurrentWebBrowserControl != null)
                CurrentWebBrowserControl.Close();
            else
                CurrentCodeFileDocument.Close();            
        }
        
        private void miCloseNonActive_Click(object sender, EventArgs e)
        {
            CloseAllButThis(CurrentCodeFileDocument);
        }

        private void miNew_Click(object sender, EventArgs e)
        {
            // SSM 26/04/22 - новый файл создается в папке текущего открытого файла
            var CurrentFileNameDirectory = Path.GetDirectoryName(CurrentSourceFileName);
            if (CurrentFileNameDirectory != "")
            {
                WorkbenchStorage.WorkingDirectory = CurrentFileNameDirectory;
                Environment.CurrentDirectory = CurrentFileNameDirectory;
            }

            WorkbenchServiceFactory.FileService.OpenFile(null, null);
        }

        private void miSaveAs_Click(object sender, EventArgs e)
        {
            ExecuteSaveAs(CurrentCodeFileDocument);   
        }

        

        private void tsat_Click(object sender, EventArgs e)
        {
            SelectAppType((PascalABCCompiler.CompilerOptions.OutputType)((sender as ToolStripMenuItem).Tag));
        }

        public void SelectAppType(PascalABCCompiler.CompilerOptions.OutputType outputType)
        {
            Image img = null;
            foreach (ToolStripMenuItem mi in tstaSelect.DropDownItems)
                if (outputType == (PascalABCCompiler.CompilerOptions.OutputType)mi.Tag)
                    img = mi.Image;
            tstaSelect.Image = img;
            WorkbenchServiceFactory.BuildService.CompilerOptions.OutputFileType = outputType;
        }

        private void StartMenuItem_Click(object sender, EventArgs e)
        {
            WorkbenchServiceFactory.RunService.Run(true);
            GC.Collect();
            if (VisualEnvironmentCompiler.Compiler != null && VisualEnvironmentCompiler.Compiler.ErrorsList.Count == 0)
            {
                var Percent = VisualEnvironmentCompiler.Compiler.PABCCodeHealth;
                if (Percent > 0)
                {
                    if (Percent >= 100)
                    {
                        HealthLabel.Text = "";
                        return;
                    }
                    HealthLabel.Text = $"{Percent}%";

                    var c = SystemColors.Control;
                    var PM100 = 100 - Percent;
                    HealthLabel.BackColor = Color.FromArgb(c.R - PM100 - 20, c.G - PM100 - 20, c.B - PM100 - 20);
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F1)
            {
                ExecShowHelpF1();
            }
        }
        
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                FormWidth = Width;
                FormHeight = Height;
            }
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                FormLeft = Left;
                FormTop = Top;
            }
        }

        private void miFind_Click(object sender, EventArgs e)
        {
            ExecFind();
        }

        private void miReplace_Click(object sender, EventArgs e)
        {
            ExecReplace();
        }

        private void miFindNext_Click(object sender, EventArgs e)
        {
            ExecFindNext();
        }

        internal WebBrowserControl _currentWebBrowserControl = null;

        internal CodeFileDocumentControl _currentCodeFileDocument = null;

        public WebBrowserControl CurrentWebBrowserControl
        {
            get { return _currentWebBrowserControl; }
            set
            {
                _currentWebBrowserControl = value;
                if (value != null)
                value.Activate();
            }
        }

        public CodeFileDocumentControl CurrentCodeFileDocument
        {
            get { return _currentCodeFileDocument; }
            set { 
                _currentCodeFileDocument = value; 
                value.Activate(); 
            }
        }

        VisualPascalABCPlugins.ICodeFileDocument VisualPascalABCPlugins.IWorkbenchDocumentService.CurrentCodeFileDocument
        {
            get
            {
                return CurrentCodeFileDocument;
            }
            set
            {
                CurrentCodeFileDocument = value as CodeFileDocumentControl;
            }
        }

        public CodeFileDocumentControl ActiveCodeFileDocument
        {
            get { return activeTabPage; }
            set
            {
                if (activeTabPage != value)
                {
                    CodeFileDocumentControl tp = ActiveCodeFileDocument;
                    activeTabPage = value;
                    if (tp != null)
                        SetTabPageText(tp);
                    if (activeTabPage != null)
                        SetTabPageText(activeTabPage);
                }
            }

        }

        Form VisualPascalABCPlugins.IWorkbench.MainForm
        {
            get
            {
                return this;
            }
        }

        private void cmSetActive_Click(object sender, EventArgs e)
        {
            ActiveCodeFileDocument = CurrentCodeFileDocument;
        }

        private void miOutputWindow_Click(object sender, EventArgs e)
        {
            SetVisibilityBottomPanel(!BPanel.Visible);
            //BPanel.Visible = !BPanel.Visible;
            //BottomDockPanel.Height = 0;
            //BottomTabsVisible = !BottomTabsVisible;
            /*OutputWindow.outputTextBox.BringToFront();
            var s = OutputWindow.outputTextBox.Height.ToString() + " " + OutputWindow.outputTextBox.Width.ToString() + "\n";
            s += OutputWindow.outputTextBox.Dock.ToString()+"\n";
            MessageBox.Show(s);*/
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            CurrentSyntaxEditor.ToggleBookmark();
        }

        private void mGOTONEXTBOOKMARKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentSyntaxEditor.NextBookmark();
        }

        private void mGOTOPREVBOOKMARKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentSyntaxEditor.PrevBookmark();
        }

        private void mCLEARBOOKMARKSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentSyntaxEditor.ClearAllBookmarks();
        }

        private void miRunNoDebug_Click(object sender, EventArgs e)
        {
            WorkbenchServiceFactory.RunService.Run(false);
        }
        
        Dictionary<CodeFileDocumentControl, string> lastInputTexts = new Dictionary<CodeFileDocumentControl, string>();
        internal string lastInputText
        {
            get
            {
                if (lastInputTexts.ContainsKey(CurrentCodeFileDocument))
                    return lastInputTexts[CurrentCodeFileDocument];
                else
                    return "";
            }
            set
            {
                if (value == "")
                    if (lastInputTexts.ContainsKey(CurrentCodeFileDocument))
                    {
                        lastInputTexts.Remove(CurrentCodeFileDocument);
                        return;
                    }
                if (lastInputTexts.ContainsKey(CurrentCodeFileDocument))
                    lastInputTexts[CurrentCodeFileDocument] = value;
                else
                    lastInputTexts.Add(CurrentCodeFileDocument, value);
            }
        }

        private void cmFindAllReferences_Click(object sender, EventArgs e)
        {
            ExecFindReferences();
        }

        private void tsNavigBack_Click(object sender, EventArgs e)
        {
            ExecNavigateBackward();
        }

        private void tsNavigForw_Click(object sender, EventArgs e)
        {
            ExecNavigateForward();
        }

        // SSM 15.06.22
        public void SetVisibilityBottomPanel(bool value)
        {
            tsOutputWindow.Checked = value;
            miOutputWindow.Checked = value;
            BPanel.Visible = value;
        }
        public void HideBottomPanel()
        {
            SetVisibilityBottomPanel(false);
        }
        public void ShowBottomPanel()
        {
            SetVisibilityBottomPanel(true);
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape /*&& BottomTabsVisibleChekerEnabled*/)
            {
                tsOutputWindow.Checked = false;
                BPanel.Visible = false;
                //BottomTabsVisible = false;
                e.Handled = true;
            }
            /*if (e.KeyData == Keys.Delete)
            {
                if (_designer_is_active && MainDockPanel.ActiveContent is CodeFileDocumentControl)
                {
                    ExecDelete();
                }
            }*/
        }

        private void mGENERATEREALIZATIONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new ClassOrMethodRealizationAction()).Execute(CurrentSyntaxEditor.TextEditor.ActiveTextAreaControl.TextArea);
        }

        private void tsSelectAll_Click(object sender, EventArgs e)
        {
            ExecSelectAll();
        }

        private void tsGotoLine_Click(object sender, EventArgs e)
        {
            ExecGotoLine();
        }

        public SymbolsViwer FindSymbolResults
        {
            get
            {
                return FindSymbolsResultWindow.FindSymbolResults;
            }
        }
        
        public void ShowFindResults(List<SymbolsViewerSymbol> syms)
        {
            FindSymbolsResultWindow.ShowFindResults(syms);
        }

        private void tsShowOutputWindow_Click(object sender, EventArgs e)
        {
            ShowContent(OutputWindow, true);
        }

        private void tsShowErrorsListWindow_Click(object sender, EventArgs e)
        {
            ShowContent(ErrorsListWindow, true);
        }

        private void tsShowCompilerConsoleWindow_Click(object sender, EventArgs e)
        {
            ShowContent(CompilerConsoleWindow, true);
        }

        private void tsShowFindSymbolsResultWindow_Click(object sender, EventArgs e)
        {
            if (!BottomTabsVisible)
        		BottomTabsVisible = true;
        	ShowContent(FindSymbolsResultWindow, true);
        }

        private void tsShowDebugVariablesListWindow_Click(object sender, EventArgs e)
        {
            if (!BottomTabsVisible)
        		BottomTabsVisible = true;
        	ShowContent(DebugVariablesListWindow, true);
        }

        private void tsShowDebugWatchListWindow_Click(object sender, EventArgs e)
        {
        	if (!BottomTabsVisible)
        		BottomTabsVisible = true;
        	ShowContent(DebugWatchListWindow, true);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (!(MainDockPanel.ActiveContent is CodeFileDocumentControl))
            {
                this.cmSave.Visible = false;
                this.cmSetActive.Visible = false;
                this.cmCloseAllButThis.Visible = false;
                CloseButtonsEnabled = true;
            }
            else
            {
                this.cmSave.Visible = true;
                this.cmSetActive.Visible = true;
                this.cmCloseAllButThis.Visible = true;
                CloseButtonsEnabled = OpenDocuments.Count > 1;
            }
        }

        private void mOPTIONSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            optionsContentEngine.ShowDialog();
        }

        private void tsViewIntellisensePanel_CheckedChanged(object sender, EventArgs e)
        {
            if (tsViewIntellisensePanel.Checked != UserOptions.ShowQuickClassBrowserPanel)
            {
                UserOptions.ShowQuickClassBrowserPanel = tsViewIntellisensePanel.Checked;
                UpdateUserOptions();
            }
        }

        private void mEXAMPLESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(null, HelpExamplesFileName);
        }

        private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog(HelpExamplesDirectory);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            OpenFileDialog(HelpTutorialExamplesDirectory);
        }

        private void miAddExpr_Click(object sender, EventArgs e)
        {
        	AddToWatchAction wact = new AddToWatchAction();
        	wact.Execute(this.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.TextArea);
        }
        
        public ContextMenuStrip BreakpointMenuStrip
        {
        	get
        	{
        		return cmBreakpointCondition;
        	}
        }
        
        private void mDELETEBREAKPOINTToolStripMenuItem_Click(object sender, EventArgs e)
        {
        	this.CurrentCodeFileDocument.TextEditor.ContextMenuStrip = this.cmEditor;
        	BreakPointFactory.DeleteCurrentBreakpoint();
        }
        
        private void mBREAKPOINTCONDITIONToolStripMenuItem1_Click(object sender, EventArgs e)
        {
			this.CurrentCodeFileDocument.TextEditor.ContextMenuStrip = this.cmEditor;
			BreakPointFactory.InvokeAddCondition();
        }
        
        private void miToolBox_Click(object sender, EventArgs e)
        {
            ToolBoxVisible = true;
        }

        private void miProperties_Click(object sender, EventArgs e)
        {
            PropertiesWindowVisible = true;
        }

        private void miUnitWithForm_Click(object sender, EventArgs e)
        {
            OpenFileWithForm();
        }

        private void mMAINFEATURESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ThreadPool.QueueUserWorkItem(__showgettingstarted))
                __showgettingstarted(null);
        }
        
		[System.ComponentModel.EditorBrowsableAttribute()]
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 74)//WM_COPYDATA
			{
				try
				{
					COPYDATASTRUCT cds = (COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam,typeof(COPYDATASTRUCT));
					if (cds.dwData.ToInt32() == 32)//open file
					{
						string file_name = cds.lpData;//Marshal.PtrToStringUni(cds.lpData,(int)cds.cbData);
						//MessageBox.Show(cds.cbData.ToString());
                        if (init)
                        {
                            if (System.IO.Path.GetExtension(file_name) != ".pabcproj")
                                WorkbenchServiceFactory.FileService.OpenFile(file_name, null);
                            else
                                WorkbenchServiceFactory.ProjectService.OpenProject(file_name);
                        }
                        else
                        {
                            FileNameToWait = file_name;
                        }
					}
				}
				catch
				{
					
				}
			}
			base.WndProc(ref m);
		}

		
		
		private void miNewProject_Click(object sender, EventArgs e)
        {
			WorkbenchServiceFactory.ProjectService.NewProject();
        }
		
		private void miOpenProject_Click(object sender, EventArgs e)
        {
			OpenProjectDialog(Constants.DefaultWorkingDirectory);
        }
		
		List<string> watches = new List<string>();

		private void miCloseProject_Click(object sender, EventArgs e)
        {
			//CloseProjectFiles();
            WorkbenchServiceFactory.ProjectService.CloseProject();
        }
		
		private void mADDFILEToolStripMenuItem_Click(object sender, EventArgs e)
        {
			ProjectTask.NewFile(ProjectExplorerWindow);
        }

        private void mADDEXISTFILEToolStripMenuItem_Click(object sender, EventArgs e)
        {
        	ProjectTask.AddFile(ProjectExplorerWindow);
        }

        private void mADDFORMToolStripMenuItem_Click(object sender, EventArgs e)
        {
        	ProjectTask.NewForm(ProjectExplorerWindow, true);//roman//
        }

        private void mADDREFERENCEToolStripMenuItem_Click(object sender, EventArgs e)
        {
        	ProjectTask.AddReferencesToProject(ProjectExplorerWindow);
        }

        private void mPROPERTIESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectTask.ProjectProperties();
        }
        
        void TsImmediateWindowClick(object sender, EventArgs e)
        {
        	if (!BottomTabsVisible)
        		BottomTabsVisible = true;
        	ShowContent(ImmediateWindow,true);
        }

        private RunArguments RunArgumentsForm = new RunArguments();

        private void mRUNPARAMETERSToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ExecRunParameters();
        } 

        private void cmiCopy_Click(object sender, EventArgs e)
        {
            //ExecDesignerCopy();
        }

        private void cmiPaste_Click(object sender, EventArgs e)
        {
            //ExecDesignerPaste();
        }

        private void miDelete_Click(object sender, EventArgs e)
        {
            //ExecDelete();
        }

        private void cmiDelete_Click(object sender, EventArgs e)
        {
            //ExecDelete();
        }

        private void cmiCut_Click(object sender, EventArgs e)
        {
            //ExecDesignerCut();
        }

        private void miTabOrder_Click(object sender, EventArgs e)
        {
            //ExecTabOrder();
        }

        private void miShowGrid_Click(object sender, EventArgs e)
        {
            //ExecShowGrid();
        }

        private void miSnapToGrid_Click(object sender, EventArgs e)
        {
            //ExecSnapToGrid();
        }

        private void cmiAlignLefts_Click(object sender, EventArgs e)
        {
           // ExecAlignLefts();
        }

        private void cmiAlignRights_Click(object sender, EventArgs e)
        {
            //ExecAlignRights();
        }

        private void cmiAlignTops_Click(object sender, EventArgs e)
        {
           // ExecAlignTops();
        }

        private void cmiAlignBottoms_Click(object sender, EventArgs e)
        {
            //ExecAlignBottoms();
        }

        private void cmiAlignMiddles_Click(object sender, EventArgs e)
        {
            //ExecAlignMiddles();
        }

        private void cmiAlignCenters_Click(object sender, EventArgs e)
        {
            //ExecAlignCenters();
        }

        private void cmiAlignToGrid_Click(object sender, EventArgs e)
        {
            //ExecAlignToGrid();
        }

        private void cmiCenterHorizontally_Click(object sender, EventArgs e)
        {
            //ExecCenterHorizontally();
        }

        private void cmiCenterVertically_Click(object sender, EventArgs e)
        {
           // ExecCenterVertically();
        }

        private void cmiSizeToControl_Click(object sender, EventArgs e)
        {
            //ExecSizeToControl();
        }

        private void cmiSizeToControlWidth_Click(object sender, EventArgs e)
        {
            //ExecSizeToControlWidth();
        }

        private void cmiSizeToControlHeight_Click(object sender, EventArgs e)
        {
            //ExecSizeToControlHeight();
        }

        private void cmiSizeToGrid_Click(object sender, EventArgs e)
        {
            //ExecSizeToGrid();
        }

        private void cmiBringToFront_Click(object sender, EventArgs e)
        {
            //ExecBringToFront();
        }

        private void cmiSendToBack_Click(object sender, EventArgs e)
        {
            //ExecSendToBack();
        }

        private void cmiTabOrder_Click(object sender, EventArgs e)
        {
            cmiTabOrder.Checked = !cmiTabOrder.Checked;
            //ExecTabOrder();
        }

        private void mFORMATToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new CodeFormattingAction()).Execute(CurrentSyntaxEditor.TextEditor.ActiveTextAreaControl.TextArea);
        }

        private void cmFormat_Click(object sender, EventArgs e)
        {
            (new CodeFormattingAction()).Execute(CurrentSyntaxEditor.TextEditor.ActiveTextAreaControl.TextArea);
        }

        private void tsFormat_Click(object sender, EventArgs e)
        {
            (new CodeFormattingAction()).Execute(CurrentSyntaxEditor.TextEditor.ActiveTextAreaControl.TextArea);
        }

        
        //ZM
        private void mNEWASPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = "{$asp newaspect}\n  {$aspdata newaspect author 1.0}\n{$endasp newaspect}";
            ICSharpCode.TextEditor.TextArea ta = CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.TextArea;
            ta.InsertString(text);

        }
       
        public void MakeTemplate()
        {
            string text = "{$asp declarations}\n  {$aspdata declarations author 1.0}\n{$endasp declarations}\n \n{$asp classes}\n{$endasp classes}\n \n{$asp main}\nbegin\nend.\n{$endasp main}\n";
            ICSharpCode.TextEditor.TextArea ta = CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.TextArea;
            ta.InsertString(text);
        }

        public void SaveAspPlugin(CodeFileDocumentControl dc)
        {
            SaveSelFile(dc);
        }

        public void CloseAspPlugin(CodeFileDocumentControl dc)
        {
            //CloseFile(dc);
        }

        private void mDOTNETHELPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMSDN();
        }

        void ExecViewCode()
        {
            //ExecuteDesignerCommand(StandardCommands.ViewCode);//roman//
        }
        private void cmViewCode_Click(object sender, EventArgs e)
        {
            ExecViewCode();
        }

        void ExecShowProperties()
        {
            //ExecuteDesignerCommand(StandardCommands.PropertiesWindow);//roman//
        }
        private void cmiProperties_Click(object sender, EventArgs e)
        {
            ExecShowProperties();
        }

        private void miPrint_Click(object sender, EventArgs e)
        {
            ExecPrint();
        }

        private void tsFindAllReferences_Click(object sender, EventArgs e)
        {
            ExecFindReferences();
        }
        //end ZM

        void ExecRunParameters()
        {
            RunArgumentsForm.CommandLineArguments = WorkbenchServiceFactory.RunService.GetRunArgument(this.CurrentCodeFileDocument.FileName.ToLower());
            if (RunArgumentsForm.ShowDialog() == DialogResult.OK)
            {
                WorkbenchServiceFactory.RunService.AddRunArgument(this.CurrentCodeFileDocument.FileName.ToLower(), RunArgumentsForm.CommandLineArguments);
            }
        }

        private void miProjectExplorer_Click(object sender, EventArgs e)
        {
            ProjectExplorerWindowVisible = true;
        }

        private void cmSamples_Click(object sender, EventArgs e)
        {

        }

        private void cmHelp_Click(object sender, EventArgs e)
        {
            __showhelpinqueue();
        }

        /*private void __checkforupdate(object state)
        {
            WorkbenchServiceFactory.UpdateService.CheckForUpdates();
        }*/

        private void miCheckUpdates_Click(object sender, EventArgs e)
        {
            WorkbenchServiceFactory.UpdateService.CheckForUpdates();
            //if (!ThreadPool.QueueUserWorkItem(__checkforupdate))
            //    __checkforupdate(null);
        }

        private void cmCollapseRegions_Click(object sender, EventArgs e)
        {
            WorkbenchServiceFactory.EditorService.CollapseRegions();
        }

        private void tsDisassembly_Click(object sender, EventArgs e)
        {
            DisassemblyWindowVisible = true;
        }

        private void tsAutoInsertCode_Click(object sender, EventArgs e)
        {
            tsAutoInsertCode.Checked = !tsAutoInsertCode.Checked;
            mAUTOINSERTToolStripMenuItem.Checked = !mAUTOINSERTToolStripMenuItem.Checked;
        }

        private void mAUTOINSERTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsAutoInsertCode.Checked = !tsAutoInsertCode.Checked;
            mAUTOINSERTToolStripMenuItem.Checked = !mAUTOINSERTToolStripMenuItem.Checked;
        }

        private void mUNITTESTSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WorkbenchServiceFactory.Workbench.ErrorsListWindow.ClearErrorList();
                var OutputFileName = WorkbenchServiceFactory.BuildService.Compile(CurrentSourceFileName, false, "__RunMode", true, true);
                if (OutputFileName != null)
                {
                    var process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "RunUnitTests.exe");
                    process.StartInfo.Arguments = "\""+OutputFileName+"\"";
                    process.Start();
                }
                /*WorkbenchServiceFactory.RunService.Run(@"D:\PABC_Git\bin\RunUnitTests.exe",
                     WorkbenchServiceFactory.Workbench.UserOptions.RedirectConsoleIO, "", false,
                     WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.Compiler.CompilerOptions.SourceFileDirectory,
                     WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.Compiler.CompilerOptions.SourceFileName, false, false);*/
            }
            catch (System.Exception ee)
            {
                ee = ee;
                // SSM 22/04/19 - исправляю вылет оболочки при отсутствии exe файла
                // this.RunnerManager_Exited(OutputFileName); // - это всё равно не срабатывает. Кнопки оказываются в заблокированном состоянии
                //var OutputFileName = @"D:\PABC_Git\bin\RunUnitTests.exe";
                //WorkbenchServiceFactory.OperationsService.AddTextToOutputWindowSync(OutputFileName, "Произошла непредвиденная ошибка" + ee.StackTrace);
                //throw;
            }
        }

        public ABCHealth ABCHealthForm = null;

        private Label AddString(double x, double y, string s, bool link = false, bool goodfeatures = false)
        {
            Label l;
            if (link)
                l = new LinkLabel();
            else l = new Label();
            l.Text = s;
            l.Left = (int)Math.Round(x);
            l.Top = (int)Math.Round(y);
            //if (link)
                if (!goodfeatures)
                    l.ForeColor = System.Drawing.Color.Red;
                else l.ForeColor = System.Drawing.Color.Green;
            l.AutoSize = true;
            ABCHealthForm.Controls.Add(l);
            return l;
        }

        private string Raz(int n)
        {
            if ((n % 10 == 2 || n % 10 == 3 || n % 10 == 4) && (n / 10 % 10 != 1))
                return n + " раза";
            else return n + " раз";
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // Пока что такая временная мера для всех языков, кроме Паскаля  EVA
            if (Languages.Facade.LanguageProvider.Instance.SelectLanguageByExtensionSafe(CurrentSourceFileName)?.Name != PascalABCCompiler.StringConstants.pascalLanguageName)
            {
                ErrorsListWindow.ShowErrorsSync(new List<PascalABCCompiler.Errors.Error>() { new PascalABCCompiler.Errors.Error(Form1StringResources.Get("PABCHEALTH_NOT_SUPPORTED")) }, true);
                return;
            }

            if (ABCHealthForm == null)
            {
                ABCHealthForm = new ABCHealth();
                ABCHealthForm.FormBorderStyle = FormBorderStyle.Fixed3D;
            }
            try
            {
                var gr = Graphics.FromHwnd(Handle);
                var scale = gr.DpiX / 96;

                var aw = ABCHealthForm.Width;
                var ah = ABCHealthForm.Height;
                var c = new PascalABCCompiler.Compiler();
                var errors = new List<PascalABCCompiler.Errors.Error>();
                var warnings = new List<PascalABCCompiler.Errors. CompilerWarning>();

                var text = CurrentSyntaxEditor.TextEditor.ActiveTextAreaControl.TextArea.Document.TextContent;

                WorkbenchServiceFactory.Workbench.ErrorsListWindow.ClearErrorList();
                var res = c.ParseText(CurrentSourceFileName, text, errors, warnings);
                WorkbenchServiceFactory.Workbench.ErrorsListWindow.ShowErrorsSync(c.ErrorsList, c.ErrorsList.Count != 0);
                if (res != null)
                {
                    var y = 30/2*scale;
                    var stat = new SyntaxVisitors.ABCStatisticsVisitor();
                    stat.ProcessNode(res);
                    //stat.
                    var h = 40 / 2 * scale;
                    var x = 50 / 2 * scale;
                    ABCHealthForm.Controls.Clear();

                    var pp = new Panel();

                    ToolTip ToolTip1 = new ToolTip();
                    ToolTip1.SetToolTip(pp, "Здоровье PascalABC.NET программы");

                    pp.Width = (int)Math.Round(220 / 2 * scale);
                    pp.Height = (int)Math.Round(100 / 2 * scale);
                    pp.Left = ABCHealthForm.ClientSize.Width - pp.Width - (int)Math.Round(24 / 2 * scale);
                    pp.Top = (int)Math.Round(24 / 2 * scale);
                    pp.BackColor = Color.Gray;
                    
                    ABCHealthForm.Controls.Add(pp);


                    var l0 = AddString(x, y, "О здоровье кода",true) as LinkLabel;
                    l0.LinkClicked += (o, e1) =>
                    {
                        System.Diagnostics.Process.Start("https://pascalabcnet.github.io/program_health.html");
                    };
                    y += 48 / 2 * scale;

                    var l1 = AddString(x, y, "В программе найдены следующие конструкции базового Паскаля,");
                    y += 36 / 2 * scale;
                    var l2 = AddString(x, y, "считающиеся устаревшими в PascalABC.NET:");
                    l1.ForeColor = Color.Black;
                    l2.ForeColor = Color.Black;
                    y += h;
                    y += 8 / 2 * scale;
                    x += 24 / 2 * scale;

                    // Проценты в минус
                    // 1. Количество var вне - количество var внутри. За первую - -10%, за остальные - -2% пока не достигнет 25%
                    // 2. Количество  for i - за первую - -15%, за каждую следующую - -3% пока не достигнет 25%
                    // 3. Read(i,j) - за первую -15%, за последующие - -2% пока не достигнет -20%
                    // 4. program - -10%
                    // 5. Статические массивы - -10%, за каждый последующий - -2% пока не достигнет -15%
                    // 6. Write(a,' ',b) - за каждую - -1% пока не достгнет -5%  
                    // 7. string[10] - за каждую - -1% . Если минусуется > 100%, то здоровье делать 0% 


                    var ShowNegativeInfo = false;
                    if (stat.OutBlockVarDefs > 0 && stat.OutBlockVarDefs > stat.InBlockVarDefs)
                    {
                        AddString(x, y, "Переменные описаны вне блоков begin-end: " + Raz(stat.OutBlockVarDefs), false);
                        y += h;
                        ShowNegativeInfo = true;
                    }
                    if (stat.ForsWithoutVar != 0)
                    {
                        AddString(x, y, "Переменная цикла for не описана в заголовке цикла: " + Raz(stat.ForsWithoutVar), false);
                        y += h;
                        ShowNegativeInfo = true;
                    }
                    if (stat.ReadProc > 0)
                    {
                        AddString(x, y, "Для ввода использована процедура Read: " + Raz(stat.ReadProc), false);
                        y += h;
                        ShowNegativeInfo = true;
                    }
                    if (stat.ProgramKeyword)
                    {
                        AddString(x, y, "Использовано ключевое слово program", false);
                        y += h;
                        ShowNegativeInfo = true;
                    }
                    if (stat.WriteProcWithSpace > 0)
                    {
                        AddString(x, y, "Использована процедура Write с пробелом в качестве разделителя: " + Raz(stat.WriteProcWithSpace), false);
                        y += h;
                        ShowNegativeInfo = true;
                    }
                    if (stat.StaticArrays > 0)
                    {
                        AddString(x, y, "Используются статические массивы вместо динамических: " + Raz(stat.StaticArrays), false);
                        y += h;
                        ShowNegativeInfo = true;
                    }
                    if (stat.OldStrings > 0)
                    {
                        AddString(x, y, "Используются устаревшие строки вида string[10]: " + Raz(stat.OldStrings), false);
                        y += h;
                        ShowNegativeInfo = true;
                    }

                    ///----------------------------------------------------
                    y += 12 / 2 * scale;
                    x -= 24 / 2 * scale;
                    var l3 = AddString(x, y, "В программе используются следующие рекомендованные конструкции");
                    y += 36 / 2 * scale;
                    var l4 = AddString(x, y, "PascalABC.NET:");
                    l3.ForeColor = Color.Black;
                    l4.ForeColor = Color.Black;
                    x += 24 / 2 * scale;
                    y += h;
                    y += 8 / 2 * scale;
                    if (stat.InBlockVarDefs > 0)
                    {
                        AddString(x, y, "Используются внутриблочные описания переменных: " + Raz(stat.InBlockVarDefs), false, true);
                        y += h;
                    }
                    if (stat.ForsWithVar != 0)
                    {
                        AddString(x, y, "Переменная цикла for описана в заголовке цикла: " + Raz(stat.ForsWithVar), false, true);
                        y += h;
                    }
                    if (stat.InitVarInDef > 0)
                    {
                        AddString(x, y, "Используется инициализация при описании: " + Raz(stat.InitVarInDef), false, true);
                        y += h;
                    }
                    if (stat.ReadFuncCount > 0)
                    {
                        AddString(x, y, "Для ввода используется функция вида ReadInteger, ReadReal: " + Raz(stat.ReadFuncCount), false, true);
                        y += h;
                    }
                    if (stat.ExtAssignCount > 0)
                    {
                        AddString(x, y, "Используется расширенное присваивание: " + Raz(stat.ExtAssignCount), false, true);
                        y += h;
                    }
                    if (stat.PrintCount > 0)
                    {
                        AddString(x, y, "Для вывода использована Print: " + Raz(stat.PrintCount), false, true);
                        y += h;
                    }
                    if (stat.TuplesCount > 0)
                    {
                        AddString(x, y, "Используются кортежи: " + Raz(stat.TuplesCount), false, true);
                        y += h;
                    }
                    if (stat.DynamicArrays > 0)
                    {
                        AddString(x, y, "Используются динамические массивы: " + Raz(stat.DynamicArrays), false, true);
                        y += h;
                    }
                    if (stat.UnpackingAssign > 0)
                    {
                        AddString(x, y, "Используется распаковка значения в переменные: " + Raz(stat.UnpackingAssign), false, true);
                        y += h;
                    }
                    if (stat.LoopsCount > 0)
                    {
                        AddString(x, y, "Используется цикл loop: " + Raz(stat.LoopsCount), false, true);
                        y += h;
                    }
                    if (stat.ForeachCount > 0)
                    {
                        AddString(x, y, "Используется цикл foreach: " + Raz(stat.ForeachCount), false, true);
                        y += h;
                    }
                    if (stat.LambdasCount > 0)
                    {
                        AddString(x, y, "Используются лямбда-выражения: " + Raz(stat.LambdasCount), false, true);
                        y += h;
                    }

                    var Percent = stat.CalcHealth(out int NegativePercent, out int PositivePercent);
                    // Процент здоровья
                    // VarDefs

                    if (!ShowNegativeInfo)
                    {
                        l1.Text = "В программе отсутствуют устаревшие в PascalABC.NET конструкции";
                        l2.Text = "";
                    }
                    if (PositivePercent == 0)
                    {
                        l3.Text = "В программе отсутствуют рекомендованные в PascalABC.NET конструкции";
                        l4.Text = "";
                    }

                    // Цвет панели
                    if (Percent < 25)
                        pp.BackColor = Color.FromArgb(255, 0, 0);
                    else if (Percent < 50)
                        pp.BackColor = Color.FromArgb(255, 64, 64);
                    else if (Percent < 75)
                        pp.BackColor = Color.FromArgb(128, 128, 128);
                    else if (Percent < 100)
                        pp.BackColor = Color.FromArgb(64 + 16 + 16, 128, 64 + 16 + 16);
                    else pp.BackColor = Color.FromArgb(0, 128, 0);

                    pp.Paint += (o, ea) =>
                    {
                        Font drawFont = new Font("Arial", 20);
                        SolidBrush drawBrush = new SolidBrush(Color.White);
                        StringFormat format = new StringFormat(StringFormatFlags.NoClip);
                        format.LineAlignment = StringAlignment.Center;
                        format.Alignment = StringAlignment.Center;
                        ea.Graphics.DrawString(Percent + "%", drawFont, drawBrush, new RectangleF(0, 0, pp.Width, pp.Height),format);
                    };

                    Button b = new Button();
                    b.Text = "OK";
                    b.Width = (int)Math.Round(156 / 2 * scale);
                    b.Height = (int)Math.Round(48 / 2 * scale);
                    y += h;
                    b.Top = (int)Math.Round(y);
                    b.Left = (int)Math.Round((double)(ABCHealthForm.Width - b.Width) / 2);
                    b.Click += (o, ee) => { ABCHealthForm.Close(); };
                    ABCHealthForm.Controls.Add(b);
                    y += b.Height + h;
                    var hh = ABCHealthForm.Height - ABCHealthForm.ClientSize.Height;
                    ABCHealthForm.Height = (int)Math.Round(y+hh);

                    ABCHealthForm.StartPosition = FormStartPosition.Manual;
                    ABCHealthForm.Left = this.Left + Width - aw;
                    ABCHealthForm.Top = this.Top;
                    b.PreviewKeyDown += (o, eee) =>
                    {
                        if (eee.KeyCode == Keys.Escape)
                            ABCHealthForm.Close();
                    };
                    ABCHealthForm.ShowDialog();
                }
            }
            catch (System.Exception ee)
            {
            }
        }

        private void HealthLabelClear()
        {
            HealthLabel.Text = "";
            HealthLabel.BackColor = System.Drawing.SystemColors.Control;
        }

        private void HealthLabel_Click(object sender, EventArgs e)
        {
            toolStripButton1_Click(this, null);
            //System.Diagnostics.Process.Start("https://pascalabcnet.github.io/program_health.html");
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            MainDockPanel.Size = new Size(MPanel.Width, MPanel.Height);
            BottomDockPanel.Size = new Size(BPanel.Width, BPanel.Height - 3);
        }

        private void MPanel_Resize(object sender, EventArgs e)
        {
            MainDockPanel.Size = new Size(MPanel.Width, MPanel.Height);
        }

        private void BPanel_Resize(object sender, EventArgs e)
        {
            BottomDockPanel.Size = new Size(BPanel.Width-3, BPanel.Height - 3);
        }

        public bool MenuActive { get; set; } = false;

        private void menuStrip1_MenuActivate(object sender, EventArgs e)
        {
            MenuActive = true;
            //Text = "MenuActive = " + MenuActive.ToString();
        }

        private void menuStrip1_MenuDeactivate(object sender, EventArgs e)
        {
            MenuActive = false;
            //Text = "MenuActive = " + MenuActive.ToString();
        }

        private void menuStrip1_Enter(object sender, EventArgs e)
        {
            MenuActive = true;
            //Text = "MenuActive = " + MenuActive.ToString();
        }

        private void menuStrip1_Leave(object sender, EventArgs e)
        {
            MenuActive = false;
            //Text = "MenuActive = " + MenuActive.ToString();
        }

        private void cmEditor_Opened(object sender, EventArgs e)
        {
            MenuActive = true;
            //Text = "ContextMenuActive = " + MenuActive.ToString();
        }

        private void cmEditor_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            MenuActive = false;
            //Text = "ContextMenuActive = " + MenuActive.ToString();
        }

        private void tsHelp_Click(object sender, EventArgs e)
        {
            __showhelpinqueue();
        }

        private void cmEditor_Opening(object sender, CancelEventArgs e)
        {
            ICSharpCode.TextEditor.TextLocation cp = CurrentSyntaxEditor.TextEditor.ActiveTextAreaControl.Caret.Position;
            ICSharpCode.TextEditor.Document.TextWord tw = CurrentSyntaxEditor.TextEditor.ActiveTextAreaControl.TextArea.Document.GetLineSegment(cp.Line).GetWord(cp.Column);
            this.cmSamples.DropDownItems.Clear();
            try
            {
                if (tw != null)
                {
                    var header = sampleManager.GetSampleHeader(tw.Word);
                    if (header != null)
                    {
                        var samples = sampleManager.GetSamples(header);
                        foreach (string pattern in samples)
                        {
                            var files = Directory.GetFiles(HelpExamplesDirectory, pattern);
                            foreach (string file in files)
                            {
                                if (File.Exists(file))
                                {
                                    var sample_mi = new ToolStripMenuItem();
                                    sample_mi.Text = file.Replace(HelpExamplesDirectory + Path.DirectorySeparatorChar, "");
                                    sample_mi.Click += delegate(object _sender, EventArgs _e)
                                    {
                                        VisualPascalABC.VisualPascalABCProgram.MainForm.Activate();
                                        ExecuteVisualEnvironmentCompilerAction(VisualEnvironmentCompilerAction.OpenFile, file);
                                    };
                                    this.cmSamples.DropDownItems.Add(sample_mi);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            this.cmSamples.Enabled = this.cmSamples.DropDownItems.Count > 0;
        }

        IWorkbenchServiceContainer VisualPascalABCPlugins.IWorkbench.ServiceContainer
        {
            get { return serviceContainer; }
        }

    }

}
