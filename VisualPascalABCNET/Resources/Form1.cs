using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using VisualPascalABCPlugins;
using VisualPascalABC.Utils;

namespace VisualPascalABC
{
    public partial class Form1 : Form
    {

        private string MainFormText = "PascalABC.NET";
        private string FTSFormat = "{0} [{1}]";

        string RedirectIOModeModuleName = "__RedirectIOMode.pas";
        string RunModeModuleName = "__RunMode.pas";
        string RedirectIOModeName = "[REDIRECTIOMODE]";
        string RunModeName = "[RUNMODE]";
        
        private System.Threading.Thread MainProgramThread = System.Threading.Thread.CurrentThread;
        private ICSharpCode.TextEditor.Document.FileSyntaxModeProvider FileSyntaxProvider;
        private RunManager RunnerManager;
        private Dictionary<string,TabPage> RunTabs = new Dictionary<string,TabPage>();
        private Dictionary<TabPage, string> ReadRequests = new Dictionary<TabPage, string>();
        public PascalABCCompiler.Errors.ErrorsStrategyManager ErrorsManager = new PascalABCCompiler.Errors.ErrorsStrategyManager(PascalABCCompiler.Errors.ErrorsStrategy.FirstSemanticAndSyntax);

        Dictionary<TabPage, TextBox> OutputTextBoxs = new Dictionary<TabPage, TextBox>();

        private int ErrorListNumCol = 1;
        private int ErrorListLineCol = 2;
        private int ErrorListDescrCol = 3;
        private int ErrorListFileCol = 4;
        private int ErrorListPathCol = 5;
        private int FormLeft, FormTop, FormWidth, FormHeight;

        private bool SkipStakTraceItemIfSourceFileInSystemDirectory = true;

        private bool LoadComplete = false;

        private bool TerminateAllPrograms = false;

        private int MaxLastOpenFiles = 10;
        private int MaxCharsInLastOpenFileName = 80;

        internal SourceLocationAction ErrorCursorPosStrategy = SourceLocationAction.GotoEnd;

        private TabPage activeTabPage = null;

        List<PascalABCCompiler.Errors.Error> ErrorsList = new List<PascalABCCompiler.Errors.Error>();

        private string WorkingDirectory = null;
        private bool WorkingDirectoryExsist
        {
            get { return Directory.Exists(WorkingDirectory); }
        }

        private Hashtable StandartDirectories;

        private string OptionsFileName = Path.ChangeExtension(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName, ".ini");

        private bool CurrentThreadIsMainThread()
        {
            return MainProgramThread == System.Threading.Thread.CurrentThread;
        }

        public VisualEnvironmentCompiler VisualEnvironmentCompiler = null;
        
        public CompileOptionsForm CompileOptionsForm1;
        public UserOptionsForm UserOptionsForm1;
        public UserOptions UserOptions;
        public CompilerForm CompilerForm1;      
        public PascalABCCompiler.CompilerOptions CompilerOptions1;
        public RunProcessOptions RunProcessOptions1;        
        public VisualStates visualStates;
        public AboutBox AboutBox1;
        public FindReplaceForm FindForm;
        public FindReplaceForm ReplaceForm;
        public List<string> LastOpenFiles;
        public string ConfirmText;

        public bool SaveCanceled = false;

        private TabPage LastSelectedTab,BakSelectedTab;

        public Form1()
        {
            PascalABCCompiler.StringResourcesLanguage.LoadDefaultConfig();
            if (PascalABCCompiler.StringResourcesLanguage.AccessibleLanguages.Count > 0)
                PascalABCCompiler.StringResourcesLanguage.CurrentLanguageName = PascalABCCompiler.StringResourcesLanguage.AccessibleLanguages[0];
           
            InitializeComponent();
            InitForm();
            AddOwnedForm(CompileOptionsForm1 = new CompileOptionsForm());
            AddOwnedForm(UserOptionsForm1 = new UserOptionsForm());
            AddOwnedForm(CompilerForm1 = new CompilerForm());
            AddOwnedForm(AboutBox1 = new AboutBox());
            AddOwnedForm(FindForm = new FindReplaceForm(FindReplaceForm.FormType.Find));
            AddOwnedForm(ReplaceForm = new FindReplaceForm(FindReplaceForm.FormType.Replace));
            CompilerOptions1 = new PascalABCCompiler.CompilerOptions();
            CompilerOptions1.Debug = true;
            LastOpenFiles = new List<string>();

            RunProcessOptions1 = new RunProcessOptions();
            

            CompilerOptions1.OutputFileType = PascalABCCompiler.CompilerOptions.OutputType.ConsoleApplicaton;


            Form1StringResources.SetTextForAllControls(this);
            Form1StringResources.SetTextForAllControls(this.contextMenuStrip1);
            Form1StringResources.SetTextForAllControls(this.cmEditor);
            PascalABCCompiler.StringResources.SetTextForAllObjects(UserOptionsForm1, "VP_OPTFORM_");
            PascalABCCompiler.StringResources.SetTextForAllObjects(CompileOptionsForm1, "VP_COMPOPTFORM_");
            PascalABCCompiler.StringResources.SetTextForAllObjects(AboutBox1, "VP_ABOUTBOXFORM_");
            PascalABCCompiler.StringResources.SetTextForAllObjects(FindForm, "VP_FINDFORM_");
            PascalABCCompiler.StringResources.SetTextForAllObjects(ReplaceForm, "VP_REPLACEFORM_");

            tsatConsoleApplication.Tag = PascalABCCompiler.CompilerOptions.OutputType.ConsoleApplicaton;
            tsatWindowsApplication.Tag = PascalABCCompiler.CompilerOptions.OutputType.WindowsApplication;
            tsatDll.Tag = PascalABCCompiler.CompilerOptions.OutputType.ClassLibrary;

            SelectAppType(CompilerOptions1.OutputFileType);

            //ivan
            dbgHelper = new DebugHelper(this);
            //\ivan
            //this.dataGridView1.MinimumSize = new Size(0, Screen.PrimaryScreen.WorkingArea.Height);

        }

        delegate void SetTextDelegate(string Text);
        delegate void SetFileNameAndTextDelegate(string ExeFileName, string Text);
        DateTime dt=DateTime.Now;
        void AddTextToOutputWindow(string ExeFileName, string Text)
        {
            //if ((DateTime.Now - dt).TotalSeconds >= 1)
              //  Application.DoEvents();
            if (!BottomTabsVisible)
                BottomTabsVisible = true;
            BottomTabControl.SelectedTab = tpInputOutput;
            TextBox textBox = OutputTextBoxs[RunTabs[ExeFileName]];
            textBox.AppendText(Text);
            if(textBox==outputTextBox)
                OutputTextBoxScrolToEnd();
        }

        void OutputTextBoxScrolToEnd()
        {
            outputTextBox.SelectionStart = outputTextBox.Text.Length;
            outputTextBox.ScrollToCaret();
        }
        void CompilerConsoleScrolToEnd()
        {
            CompilerConsole.SelectionStart = CompilerConsole.Text.Length;
            CompilerConsole.ScrollToCaret();
        }
        void InputTextBoxCursorToEnd()
        {
            InputTextBox.SelectionStart = InputTextBox.Text.Length;
        }

        TabPage FindTab(string FileName)
        {
            if (FileName==null) return null;
            foreach (TabPage tbPage in tabControl1.TabPages)
                if (((Data)tbPage.Tag).FullPathName.ToLower() == FileName.ToLower())
                    return tbPage;
            return null;
        }
        bool IsBlankNewProgram(TabPage tp)
        {
            return !(tp.Tag as Data).DocumentSavedToDisk && !(tp.Tag as Data).DocumentChanged;
        }
        public TabPage OpenFile(string FileName)
        {
            TabPage tp = FindTab(FileName);
            bool IsNewFile = FileName == null;
            if (tp == null)
            {
                visualStates.FileOpened = true;//?????
                if (!IsNewFile && !File.Exists(FileName))
                {
                    MessageBox.Show(string.Format(PascalABCCompiler.StringResources.Get("!FILE_NOT_FOUND{0}"), FileName), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                openFileDialog1.InitialDirectory = Path.GetDirectoryName(FileName);
                if (!IsBlankNewProgram(CurrentTabPage))
                {
                    if (IsNewFile)
                        FileName = InstNameNewProgramm(tabControl1);
                    tp = AddNewProgramToTab(tabControl1, FileName);
                }
                else
                    if (!IsNewFile)
                    {
                        CheckErrorListAndClear((tabControl1.TabPages[TabIndex].Tag as Data).FullPathName);
                        (CurrentTabPage.Tag as Data).FullPathName = FileName;
                        SetTabPageText(CurrentTabPage);
                        tp = CurrentTabPage;
                    }
                if (!IsNewFile)
                {
                    AddLastFile(FileName);
                    (tp.Tag as Data).OwnSyntaxEditor.LoadFromFile(FileName);
                    //ivan added
                    ShowAddedBreakpoints(tabControl1.TabPages[TabIndex]);
                }
                CloseButtonsEnabled = tabControl1.TabPages.Count > 1;
            }
            if (tp != null)
            {
                tabControl1.SelectedTab = tp;
                (CurrentTabPage.Tag as Data).DocumentSavedToDisk = !IsNewFile;
            }
            visualStates.FileOpened = false;
            return tabControl1.SelectedTab;;
        }

        string GetLastFileCaption(string FullFileName)
        {
            if (FullFileName.Length > MaxCharsInLastOpenFileName + 5)
            {
                string DriveName = Path.GetPathRoot(FullFileName);//FullFileName.Substring(0, 3);
                string FileName = Path.GetFileName(FullFileName);
                int FreeSpace = MaxCharsInLastOpenFileName - DriveName.Length - FileName.Length;
                string DirectoryName = Path.GetDirectoryName(FullFileName);
                if (FreeSpace > 0 && DirectoryName.Length > FreeSpace)
                    DirectoryName = DirectoryName.Substring(DirectoryName.Length - FreeSpace);
                else
                    return FullFileName;
                return DriveName + "..." + DirectoryName + "\\" + FileName;
            }
            return FullFileName;
        }
        bool ReplaceLastFile(string OldFileName, string NewFileName)
        {
            ToolStripMenuItem mI;
            for (int i = 0; i < LastOpenFiles.Count; i++)
                if (LastOpenFiles[i].ToLower() == OldFileName.ToLower())
                {
                    LastOpenFiles.RemoveAt(i);
                    LastOpenFiles.Insert(0, NewFileName);
                    mI = (ToolStripMenuItem)miRecentFiles.DropDownItems[i];
                    miRecentFiles.DropDownItems.RemoveAt(i);
                    mI.Text = GetLastFileCaption(NewFileName);
                    mI.Tag = NewFileName;
                    miRecentFiles.DropDownItems.Insert(0, mI);
                    return true;
                }
            return false;
        }
        void AddLastFile(string FileName)
        {
            if (Path.GetDirectoryName(FileName) == "") return;
            ToolStripMenuItem mI;
            if (ReplaceLastFile(FileName, FileName)) return;
            if (LastOpenFiles.Count >= MaxLastOpenFiles)
            {
                LastOpenFiles.RemoveAt(LastOpenFiles.Count - 1);
                miRecentFiles.DropDownItems.RemoveAt(miRecentFiles.DropDownItems.Count - 1);
            }
            LastOpenFiles.Insert(0,FileName);
            mI = new ToolStripMenuItem();
            mI.Text = GetLastFileCaption(FileName);
            mI.Tag = FileName;
            miRecentFiles.DropDownItems.Insert(0, mI);
            mI.Click += new System.EventHandler(this.OpenLastFile_ToolStripMenuItem_Click);
           // mI.ToolTipText = mI.Text;
        }
        public void ErrorListClear()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add();
            ErrorListAddRowsFill();
            dataGridView1.Update();
            ErrorsList.Clear();
        }

        public void ErrorListAddRowsFill()
        {
            if (dataGridView1.Height < dataGridView1.RowTemplate.Height)
                return;
            int RowCount;
            int c = dataGridView1.RowCount;
            if (c == 0) dataGridView1.Rows.Add();
            RowCount = (dataGridView1.Size.Height / dataGridView1.Rows[0].Height)+3;
            if (dataGridView1.Rows.Count < RowCount)
            {
                dataGridView1.Rows.Add(RowCount - dataGridView1.Rows.Count);
                dataGridView1.ScrollBars = ScrollBars.None;
            }
            else
            {
                bool FullRow = true;
                int i = 0;
                while (dataGridView1.Rows.Count > RowCount && FullRow)
                {
                    if (i >= dataGridView1.Rows.Count) return;
                    while (dataGridView1.Rows[i].Cells[0].Value != null && i < dataGridView1.Rows.Count - 2) i++;                    
                    if (dataGridView1.Rows[i].Cells[0].Value == null) dataGridView1.Rows.RemoveAt(i);
                    else FullRow = false;                    
                }
                if (dataGridView1.Rows[dataGridView1.RowCount-1].Cells[0].Value != null)
                    dataGridView1.ScrollBars = ScrollBars.Vertical;
            }
            
        }
        
        private void SaveSelFile(TabPage TbPage)
        {
            SaveSelFileAs(TbPage, (TbPage.Tag as Data).FullPathName);
        }
        private void SaveSelFileAs(TabPage TbPage,string FileName)
        {
            if (!(TbPage.Tag as Data).DocumentSavedToDisk)
                ExecuteSaveAs(TbPage);
            else
                SaveFileAs(TbPage, FileName);
        }
        void SaveFileAs(TabPage TbPage, string FileName)
        {
            TextWriter TWr = new StreamWriter(FileName, false, System.Text.Encoding.GetEncoding(1251));
            TWr.Write((TbPage.Tag as Data).OwnSyntaxEditor.OwnEdit.Document.TextContent);
            TWr.Close();
            (TbPage.Tag as Data).DocumentChanged = false;
            (TbPage.Tag as Data).FullPathName = FileName;
            (TbPage.Tag as Data).DocumentSavedToDisk = true;
            SetTabPageText(TbPage);
            UpdateSaveButtonsEnabled();
        }

        void UpdateSaveButtonsEnabled()
        {
            SaveButtonsEnabled = (tabControl1.SelectedTab.Tag as Data).DocumentChanged;
            SaveAllButtonsEnabled = !AllSaved();
        }

        private bool AllSaved()
        {
            foreach (TabPage tp in tabControl1.TabPages)
                if ((tp.Tag as Data).DocumentChanged) return false;
            return true;
        }

        private string InstNameNewProgramm(TabControl tabControl)
        {
            int NumbNewProg = 1;
            string FileNameFormat = WorkingDirectory + "\\Program{0}.pas", FileName;
            while (true)
            {
                FileName = string.Format(FileNameFormat, NumbNewProg);
                if (FindTab(FileName)==null && !File.Exists(FileName))
                    return FileName;
                NumbNewProg++;
            }
            /*int i = 0, NumbNewProg = 1;            
            bool pr = true;
            while (pr)
            {
                if (!File.Exists("Program" + NumbNewProg + ".pas"))
                {
                    while (((pr && tabControl.TabPages[i].Text != "Program" + NumbNewProg + ".pas") && (tabControl.TabPages[i].Text != "Program" + NumbNewProg + ".pas * ")) && tabControl.TabPages.Count != i)
                    {                        
                        if (tabControl.TabPages.Count - 1 == i) pr = false;
                        i++;
                    }
                    if (pr) NumbNewProg++;
                }
                else NumbNewProg++;
            }            
            return Directory.GetCurrentDirectory() + "\\" + "Program" + NumbNewProg + ".pas";
             * */
        }

        private TabPage AddNewProgramToTab(TabControl tabControl, string FileName)
        {
            SyntaxEditor edit = new SyntaxEditor(this);
            TabPage toAdd = AddNewTab(tabControl);
            edit.AddToTab(toAdd);
            toAdd.Tag = new Data(FileName, edit, false);
            SetTabPageText(toAdd);
            edit.SetHighlightingStrategyForFile(FileName);
            //ivan
            AddBreakPointHandler(toAdd);
            //\ivan
            return toAdd;
        }
        private TabPage AddNewTab(TabControl tabControl)
        {
            TabPage tp = new TabPage();           
            tp.BorderStyle=BorderStyle.Fixed3D;
            //tp.Font = new Font("MS Sans Serif", tp.Font.Size);
            TextBox tb = outputTextBox;
            if (tabControl.TabPages.Count > 0)
                tb = CopyTextBox(outputTextBox);
            tabControl.TabPages.Add(tp);
            OutputTextBoxs.Add(tp, tb);
            return tp;
            //tabControl.SelectedTab = tp;
        }

        TextBox CopyTextBox(TextBox orig)
        {
            TextBox res = new TextBox();
            res.Visible = false;
            res.Font = orig.Font;
            res.Size = orig.Size;
            res.Location = orig.Location;
            res.ScrollBars = orig.ScrollBars;
            res.ReadOnly = orig.ReadOnly;
            res.Dock = orig.Dock;
            res.Multiline = orig.Multiline;
            res.ForeColor = orig.ForeColor;
            res.BackColor = orig.BackColor;
            res.Parent = orig.Parent;
            return res;
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
        
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            foreach(string FileName in openFileDialog1.FileNames)
                OpenFile(FileName);
        }

        private void очиститьОкноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            outputTextBox.Clear();
        }

        private void statusStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            RunnerManager = new RunManager(ReadStringRequest);

            //this.Width = 800;
            //this.Height = 600;
            UserOptions = new UserOptions();

            visualStates = new VisualStates();

            

            string hdir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName) + "\\Highlighting\\";
            if (Directory.Exists(hdir))
            {
                FileSyntaxProvider = new ICSharpCode.TextEditor.Document.FileSyntaxModeProvider(hdir);
                ICSharpCode.TextEditor.Document.HighlightingManager.Manager.AddSyntaxModeFileProvider(FileSyntaxProvider);
                string Filter = "", AllFilter = "";
                foreach (ICSharpCode.TextEditor.Document.SyntaxMode sm in FileSyntaxProvider.SyntaxModes)
                {
                    Filter = Tools.MakeFilter(Filter, sm.Name, sm.Extensions);
                    AllFilter = Tools.MakeAllFilter(AllFilter, sm.Name, sm.Extensions);
                }
                saveFileDialog1.Filter = openFileDialog1.Filter = Tools.FinishMakeFilter(Filter, AllFilter);
            }
            RunnerManager.Exited += new RunManager.RunnerManagerActionDelegate(RunnerManager_Exited);
            RunnerManager.Started += new RunManager.RunnerManagerActionDelegate(RunnerManager_Started);
            RunnerManager.OutputStringReceived += new RunManager.TextRecivedDelegate(RunnerManager_OutputStringReceived);
            RunnerManager.RunnerManagerUnhanledRuntimeException += new RunManager.RunnerManagerUnhanledRuntimeExceptionDelegate(RunnerManager_RunnerManagerUnhanledRuntimeException);

            FormLeft = this.Left; FormTop = this.Top; FormWidth = this.Width; FormHeight = this.Height;

            LoadOptions(OptionsFileName);


            tsOutputWindow.Checked = BottomTabsVisible;

            StandartDirectories = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
            StandartDirectories.Add(Constants.SystemDirectoryIdent, System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName));
            if (WorkingDirectory == null)
                WorkingDirectory = PascalABCCompiler.Tools.ReplaceAllKeys(Constants.DefaultWorkingDirectory, StandartDirectories);
            StandartDirectories.Add(Constants.WorkingDirectoryIdent, WorkingDirectory);

            AddNewProgramToTab(tabControl1, InstNameNewProgramm(tabControl1));
            foreach (string FileName in VisualPascalABC.CommandLineArgs)
                OpenFile(FileName);
            dataGridView1.Rows.Add();
            ErrorListAddRowsFill();

            SetStopEnabled(false);
            CompilingButtonsEnabled = CloseButtonsEnabled = SaveAllButtonsEnabled = SaveButtonsEnabled = false;

            NavigBackButtonsEnabled = NavigForwButtonsEnabled = false;

            try
            {
                if (!Directory.Exists(WorkingDirectory))
                    Directory.CreateDirectory(WorkingDirectory);
            }
            catch (Exception exc)
            {
                AddTextToCompilerMessages(Form1StringResources.Get("MSGTYPE")+String.Format(Form1StringResources.Get("ERROR_CREATING_WORKDIR_{0}_{1}"), WorkingDirectory,exc.Message)+Environment.NewLine);
            }

        }

        void RunnerManager_RunnerManagerUnhanledRuntimeException(string id, string ExceptionType, string ExceptionMessage, string StackTraceData, List<RunManager.StackTraceItem> StackTrace)
        {
            string localiseMsg = RuntimeExceptionsStringResources.Get(ExceptionMessage);
            AddTextToOutputWindowSync(id, string.Format(Form1StringResources.Get("OW_RUNTIME_EXCEPTION{0}_MESSAGE{1}"), ExceptionType, localiseMsg) + Environment.NewLine);
            if (StackTraceData != null) 
                AddTextToOutputWindowSync(id, string.Format(Form1StringResources.Get("OW_RUNTIME_EXCEPTION_STACKTRACE{0}"), StackTraceData) + Environment.NewLine);
            RunManager.StackTraceItem ToSend = null;
            foreach (RunManager.StackTraceItem StackTraceItem in StackTrace)
            {
                if (StackTraceItem.SourceFileName != null)
                {
                    if (ToSend == null)
                        ToSend = StackTraceItem;
                    if (SkipStakTraceItemIfSourceFileInSystemDirectory && Path.GetDirectoryName(StackTraceItem.SourceFileName) == CompilerOptions1.SearchDirectory)
                        continue;
                    ToSend = StackTraceItem;
                    break;
                }
            }
            if (ToSend != null)
            {
                List<PascalABCCompiler.Errors.Error> list = new List<PascalABCCompiler.Errors.Error>();
                list.Add(new RuntimeException(
                    string.Format(
                    Form1StringResources.Get("ERRORLIST_RUNTIME_EXCEPTION_MESSAGE{0}"),
                    localiseMsg),
                    ToSend.SourceFileName,
                    0,
                    ToSend.LineNumber)
                    );
                ShowErrorsSync(list, false);
            }
        }

        string CurrentIdForReadRequest;
        void ReadStringRequest(string ForId)
        {
            if(!ReadRequests.ContainsKey(RunTabs[ForId]))
                ReadRequests.Add(RunTabs[ForId], "");
            BeginInvoke(new ReadStringRequestSyncDel(ReadStringRequestSync));
        }
        delegate void ReadStringRequestSyncDel();
        void ReadStringRequestSync()
        {
            UpdateReadRequest(false);
        }

        bool BottomTabsVisibleChekerEnabled
        {
            get
            {
                return miOutputWindow.Enabled;
            }
            set
            {
                miOutputWindow.Enabled = tsOutputWindow.Enabled = value;
            }
        }

        bool InputPanelVisible
        {
            get
            {
                return InputPanel.Visible;
            }
            set
            {
                if (value && !InputPanel.Visible)
                {
                    InputPanel.Visible = true;
                    if (!BottomTabsVisible)
                        BottomTabsVisible = true;
                    BottomTabControl.SelectedTab = tpInputOutput;
                    OutputTextBoxScrolToEnd();
                    SetFocusToInputEditor();
                    BottomTabsVisibleChekerEnabled = false;
                    return;
                }
                if (!value && InputPanel.Visible)
                {
                    InputPanel.Visible = false;
                    InputTextBox.Text = "";
                    lastInputText = "";
                    SetFocusToEditor();
                    BottomTabsVisibleChekerEnabled = true;
                    return;
                }
            }
        }

        void UpdateReadRequest(bool changeSelected)
        {
            if (changeSelected && InputPanelVisible)
            {
                ReadRequests[LastSelectedTab] = InputTextBox.Text;
            }
            if (ReadRequests.ContainsKey(CurrentTabPage))
            {
                InputTextBox.Text = ReadRequests[CurrentTabPage];
                InputPanelVisible = true;
                InputTextBoxCursorToEnd();
                return;
            }
            InputPanelVisible = false;
        }

        private string OptionsItemNameLanguage = "Language";
        private string OptionsItemNameMainFormLeft = "MainFormLeft";
        private string OptionsItemNameMainFormTop = "MainFormTop";
        private string OptionsItemNameMainFormHeight = "MainFormHeight";
        private string OptionsItemNameMainFormWidth = "MainFormWidth";
        private string OptionsItemNameMainFormMaximized = "MainFormMaximized";
        private string OptionsItemNameShowLinesNum = "ShowLinesNum";
        private string OptionsItemNameRedirectConsoleIO = "RedirectConsoleIO";
        private string OptionsItemNameGenerateDebugInfo = "GenerateDebugInfo";
        private string OptionsItemNameLastFile = "LastFile";
        private string OptionsItemNameWorkDirectory = "WorkDirectory";
        private string OptionsItemNameShowOutputWindow = "ShowOutputWindow";
        private string OptionsItemNameErrorsStrategy = "ErrorsStrategy";
        private string OptionsItemNameEditorFontSize = "EditorFontSize";
        private string OptionsItemNameErrorsCursorPosStrategy = "ErrorsCursorPosStrategy";
        private string OptionsItemNameShowMathBracket = "ShowMathBracket";
        private string OptionsItemNameConvertTabsToSpaces = "ConvertTabsToSpaces";
        private string OptionsItemNameTabIdent= "TabIdent";
        private string OptionsItemNameDeleteEXEAfterExecute = "DeleteEXEAfterExecute";
        private string OptionsItemNameDeletePDBAfterExecute = "DeletePDBAfterExecute";
        

        void LoadOptions(string FileName)
        {
            try
            {
                if (!File.Exists(FileName))
                {
                    PascalABCCompiler.StringResourcesLanguage.CurrentLanguageName = "Русский";
                    return;
                }
                Hashtable Options = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
                PascalABCCompiler.StringResources.ReadStringsFromStream(new StreamReader(FileName, System.Text.Encoding.GetEncoding(1251)), Options);
                string value;
                int val;
                if ((value = (string)Options[OptionsItemNameWorkDirectory]) != null)
                    WorkingDirectory = value;
                if ((value = (string)Options[OptionsItemNameLanguage]) != null)
                    PascalABCCompiler.StringResourcesLanguage.CurrentLanguageName = value;
                if ((value = (string)Options[OptionsItemNameMainFormLeft]) != null)
                {
                    val = Convert.ToInt32(value);
                    if (val < Screen.PrimaryScreen.WorkingArea.Size.Width && val >= 0)
                        FormLeft = this.Left = val;
                }
                if ((value = (string)Options[OptionsItemNameMainFormTop]) != null)
                {
                    val = Convert.ToInt32(value);
                    if (val < Screen.PrimaryScreen.WorkingArea.Size.Height && val >= 0)
                        FormTop = this.Top = val;
                }
                if ((value = (string)Options[OptionsItemNameMainFormWidth]) != null)
                {
                    val = Convert.ToInt32(value);
                    if (val < Screen.PrimaryScreen.WorkingArea.Size.Width && val >= 100)
                        FormWidth = this.Width = val;
                } 
                if ((value = (string)Options[OptionsItemNameMainFormHeight]) != null)
                {
                    val = Convert.ToInt32(value);
                    if (val < Screen.PrimaryScreen.WorkingArea.Size.Height && val >= 100)
                        FormHeight = this.Height = val;
                } 
                if ((value = (string)Options[OptionsItemNameMainFormMaximized]) != null)
                    if (Convert.ToBoolean(value))
                        this.WindowState = FormWindowState.Maximized;
                if ((value = (string)Options[OptionsItemNameShowLinesNum]) != null)
                    UserOptions.ShowLineNums = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameDeleteEXEAfterExecute]) != null)
                    UserOptions.DeleteEXEAfterExecute = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameDeletePDBAfterExecute]) != null)
                    UserOptions.DeletePDBAfterExecute = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameShowMathBracket]) != null)
                    UserOptions.ShowMathBraket = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameEditorFontSize]) != null)
                    UserOptions.EditorFontSize = Convert.ToInt32(value);
                if ((value = (string)Options[OptionsItemNameErrorsStrategy]) != null)
                    ErrorsManager.Strategy = (PascalABCCompiler.Errors.ErrorsStrategy)Convert.ToByte(value);
                if ((value = (string)Options[OptionsItemNameErrorsCursorPosStrategy]) != null)
                    ErrorCursorPosStrategy = (VisualPascalABCPlugins.SourceLocationAction)Convert.ToByte(value);
                if ((value = (string)Options[OptionsItemNameShowOutputWindow]) != null)
                    BottomTabsVisible = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameRedirectConsoleIO]) != null)
                    UserOptions.RedirectConsoleIO = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameConvertTabsToSpaces]) != null)
                    UserOptions.ConverTabsToSpaces = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameGenerateDebugInfo]) != null)
                    CompilerOptions1.Debug = Convert.ToBoolean(value);
                if ((value = (string)Options[OptionsItemNameTabIdent]) != null)
                    UserOptions.TabIndent = Convert.ToInt32(value);
                int i = 0;
                while ((value = (string)Options[OptionsItemNameLastFile + (i++).ToString()]) != null)
                    AddLastFile(value);
            }
            catch (Exception)
            {
            }
        }
        void SaveOptions(string FileName)
        {
            Hashtable Options = new Hashtable(StringComparer.CurrentCultureIgnoreCase);

            Options.Add(OptionsItemNameWorkDirectory, WorkingDirectory);
            Options.Add(OptionsItemNameLanguage, PascalABCCompiler.StringResourcesLanguage.CurrentLanguageName);
            Options.Add(OptionsItemNameMainFormLeft, FormLeft);
            Options.Add(OptionsItemNameMainFormTop, FormTop);
            Options.Add(OptionsItemNameMainFormHeight, FormHeight);
            Options.Add(OptionsItemNameMainFormWidth, FormWidth);
            Options.Add(OptionsItemNameMainFormMaximized, this.WindowState == FormWindowState.Maximized);
            Options.Add(OptionsItemNameShowOutputWindow, BottomTabsVisible);
            Options.Add(OptionsItemNameErrorsStrategy, (byte)ErrorsManager.Strategy);
            Options.Add(OptionsItemNameErrorsCursorPosStrategy, (byte)ErrorCursorPosStrategy);
            Options.Add(OptionsItemNameGenerateDebugInfo, CompilerOptions1.Debug);
            Options.Add(OptionsItemNameDeleteEXEAfterExecute, UserOptions.DeleteEXEAfterExecute);
            Options.Add(OptionsItemNameDeletePDBAfterExecute, UserOptions.DeletePDBAfterExecute);

            Options.Add(OptionsItemNameConvertTabsToSpaces, UserOptions.ConverTabsToSpaces);
            Options.Add(OptionsItemNameTabIdent, UserOptions.TabIndent);
            Options.Add(OptionsItemNameShowLinesNum, UserOptions.ShowLineNums);
            Options.Add(OptionsItemNameShowMathBracket, UserOptions.ShowMathBraket);
            Options.Add(OptionsItemNameEditorFontSize, UserOptions.EditorFontSize);
            Options.Add(OptionsItemNameRedirectConsoleIO, UserOptions.RedirectConsoleIO);
            for (int i = 0; i<LastOpenFiles.Count; i++)
                Options.Add(OptionsItemNameLastFile + (LastOpenFiles.Count-i-1).ToString(), LastOpenFiles[i]);

            try
            {
                PascalABCCompiler.StringResources.WriteStringsToStream(new StreamWriter(FileName, false, System.Text.Encoding.GetEncoding(1251)), Options);
            }
            catch (Exception)
            {
                //гасим исключения если не удалось записать файл. это не смертельно
            }

        }

        void RunnerManager_OutputStringReceived(string fileName, RunManager.StreamType streamType, string text)
        {
            AddTextToOutputWindowSync(fileName, text);
        }

        void AddTextToOutputWindowSync(string fileName, string text)
        {
            BeginInvoke(new SetFileNameAndTextDelegate(AddTextToOutputWindow), fileName, text);
        }
       
        private void открытьToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            //печатьToolStripMenuItem.Enabled = true;            
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UnhandledExceptionEventArgs a;
            
        }

        private void ExecuteSaveAs(TabPage TbPage)
        {
            TabPage bakTab = CurrentTabPage;
            CurrentTabPage = TbPage;
            saveFileDialog1.FileName = Path.GetFileName(CurrentSourceFileName);
            string id = Path.GetDirectoryName(CurrentSourceFileName);
            if (id == "")
            {
                if (WorkingDirectoryExsist)
                    saveFileDialog1.InitialDirectory = WorkingDirectory;
            }
            else
                saveFileDialog1.InitialDirectory = Path.GetDirectoryName(CurrentSourceFileName);
            saveFileDialog1.ShowDialog();
            CurrentTabPage = bakTab;

        }


        private void окноВыводаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
        bool BottomTabsVisible
        {
            get 
            { 
                return !splitContainer1.Panel2Collapsed; 
            }
            set 
            {
                miOutputWindow.Checked = value;
                tsOutputWindow.Checked = value;
                splitContainer1.Panel2Collapsed = !value;
                //if (value) 
                //    this.Show();
                if (!value && LoadComplete)
                    SetFocusToEditor();
            }
        }

        bool QuestionAndSaveFile(TabPage tp)
        {
            CurrentTabPage = tp;
            DialogResult result = MessageBox.Show(string.Format(Form1StringResources.Get("SAVE_CHANGES_IN_FILE{0}"), Path.GetFileName(tp.Tag.ToString())), PascalABCCompiler.StringResources.Get("!CONFIRM"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                SaveSelFile(tabControl1.SelectedTab);
            else
                if (result == DialogResult.Cancel)
                    return false;
            return true;
        }

        void CheckErrorListAndClear(string FileName)
        {
            if (ErrorsList.Count > 0)
            {
                PascalABCCompiler.Errors.LocatedError er = ErrorsList[0] as PascalABCCompiler.Errors.LocatedError;
                if (er != null && er.SourceLocation != null && er.SourceLocation.FileName.ToLower() == FileName.ToLower())
                    ErrorListClear();

            }
        }

        public void CloseFile(TabPage tp)
        {
            string FileName = (tp.Tag as Data).FullPathName.ToLower();
            string exeName = (tp.Tag as Data).EXEFileName;
            if (RunnerManager.IsRun(exeName))
                RunnerManager.Stop(exeName);
            if ((tp.Tag as Data).DocumentChanged)
                if (!QuestionAndSaveFile(tp))
                {
                    SaveCanceled = true;
                    return;
                }
            if (tabControl1.TabPages.Count != 1)
            {
                if((tp.Tag as Data).DocumentSavedToDisk)
                    AddLastFile(CurrentSourceFileName);
                if (LastSelectedTab != null && !LastSelectedTab.IsDisposed)
                    tabControl1.SelectedTab = LastSelectedTab;
                if (tp == ActiveTabPage) 
                    ActiveTabPage = null;
                OutputTextBoxs.Remove(tp);
                tp.Dispose();
            }
            CheckErrorListAndClear(FileName);
            if (tabControl1.TabPages.Count == 1)
            {
                CloseButtonsEnabled = false;
            }
            SaveAllButtonsEnabled = !AllSaved();
        }

        //меняем стратегию подсведки в соответсвии с расширением файла
        private void SetHighlightingStrategy(ICSharpCode.TextEditor.TextEditorControl edit, string FileName)
        {
            edit.Document.HighlightingStrategy = ICSharpCode.TextEditor.Document.HighlightingManager.Manager.FindHighlighterForFile(FileName);
        }


        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //ReplaceLastFile(CurrentSourceFileName, saveFileDialog1.FileName);
            SaveFileAs(tabControl1.SelectedTab, saveFileDialog1.FileName);
            AddLastFile(saveFileDialog1.FileName);
        }
        

        void SetTabPageText(TabPage tp)
        {
            Data fileInfo=tp.Tag as Data;
            tp.Text = Path.GetFileName(fileInfo.FullPathName);
            tp.ToolTipText = fileInfo.FullPathName;
            if (fileInfo.DocumentChanged)
                tp.Text += " * ";
            if (fileInfo.Run)
                tp.Text += string.Format(" [{0}]", PascalABCCompiler.StringResources.Get("VP_MF_TS_RUN"));
            if (tp == ActiveTabPage)
                tp.Text = Convert.ToChar(0x2022) + tp.Text;//25CF
        }
        public void SynEdit_ChangeText(object sender)
        {
            if (!((Data)tabControl1.SelectedTab.Tag).DocumentChanged && !visualStates.FileOpened)//!((Data)tabControl1.SelectedTab.Tag).FileOpened) 
            {
                ((Data)tabControl1.SelectedTab.Tag).DocumentChanged = true;
                SetTabPageText(tabControl1.SelectedTab);
                SaveAllButtonsEnabled = SaveButtonsEnabled = true;
            }
            UpdateUndoRedoEnabled();
        }

        public void SynEdit_SelectionChanged(object sender)
        {
            UpdateCutCopyButtonsEnabled();
        }
        
        void UpdateCutCopyButtonsEnabled()
        {
            CutButtonsEnabled = CopyButtonsEnabled = CurrentSyntaxEditor.TextSelected;
        }

        public SyntaxEditor CurrentSyntaxEditor
        {
            get             
            { 
                if (tabControl1.SelectedTab!=null)
                    return (tabControl1.SelectedTab.Tag as Data).OwnSyntaxEditor;
                return null;
            }
        }

        private void печатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDialog1.ShowDialog();
        }

        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (tabControl1.SelectedTab.Tag as Data).OwnSyntaxEditor.OwnEdit.Undo();
            UpdateUndoRedoEnabled();
        }

        private void восстановитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (tabControl1.SelectedTab.Tag as Data).OwnSyntaxEditor.OwnEdit.Redo();
            UpdateUndoRedoEnabled();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        
        public void SetStateText(string text)
        {
            toolStripStatusLabel5.Text = text;
            statusStrip1.Refresh();
        }

        public void AddTextToCompilerMessages(string text)
        {
            CompilerConsole.AppendText(text);
            CompilerConsoleScrolToEnd();
            //this.CompilerConsole.g
        }


        public void Reset()
        {
        }

        
        private void Form1_Shown(object sender, EventArgs e)
        {
            //PascalABCCompiler.StringResourcesLanguage.CurrentLanguageName = "Russi         

            CahngedSelectedTab();


            VisualEnvironmentCompiler = new VisualEnvironmentCompiler(
                this.BeginInvoke, tabControl1, SetCompilingButtonsEnabled, SetStateText,
                AddTextToCompilerMessages, miPlugins, toolStrip1,
                ExecuteSourceLocationAction, ExecuteVisualEnvironmentCompilerAction,ErrorsManager);

            VisualEnvironmentCompiler.ChangeVisualEnvironmentState += new ChangeVisualEnvironmentStateDelegate(VisualEnvironmentCompiler_ChangeVisualEnvironmentState);

            VisualEnvironmentCompiler.RunStartingThread();

            LoadComplete = true;
        }

        private object ExecuteVisualEnvironmentCompilerAction(VisualEnvironmentCompilerAction Action,object obj)
        {
            switch (Action)
            {
                case VisualEnvironmentCompilerAction.Run:
                    return Run(true);
                case VisualEnvironmentCompilerAction.Stop:
                    return Stop();
                case VisualEnvironmentCompilerAction.Build:
                    return Build();
                case VisualEnvironmentCompilerAction.Rebuild:
                    return Rebuild();
                case VisualEnvironmentCompilerAction.OpenFile:
                    return OpenFile((string)obj);
                case VisualEnvironmentCompilerAction.GetDirectory:
                    string s=VisualEnvironmentCompiler.Compiler.CompilerOptions.StandartDirectories[(string)obj] as string;
                    if (s != null) 
                        return s;
                    return StandartDirectories[(string)obj] as string;
                case VisualEnvironmentCompilerAction.GetCurrentSourceFileName:
                    return CurrentSourceFileName;
                case VisualEnvironmentCompilerAction.SetCurrentSourceFileText:
                    string text = (string)obj;
                    CurrentSyntaxEditor.SelectAll();
                    Tools.CopyTextToClipboard(text);
                    CurrentSyntaxEditor.Paste();
                    return true;
            }
            return false;
        }

        public void ExecuteSourceLocationAction(PascalABCCompiler.SourceLocation 
            SourceLocation,VisualPascalABCPlugins.SourceLocationAction Action)
        {
            if (SourceLocation.FileName!=null) 
                OpenFile(SourceLocation.FileName);
            ICSharpCode.TextEditor.TextEditorControl editor = CurrentSyntaxEditor.OwnEdit;
            
            Point Beg = new Point(SourceLocation.BeginPosition.Column - 1, SourceLocation.BeginPosition.Line - 1);
            Point End = new Point(SourceLocation.EndPosition.Column, SourceLocation.EndPosition.Line - 1);
            switch (Action)
            {
                case SourceLocationAction.SelectAndGotoBeg:
                case SourceLocationAction.SelectAndGotoEnd:
                    if (Action == SourceLocationAction.SelectAndGotoBeg) 
                        editor.ActiveTextAreaControl.Caret.Position = Beg;
                    else
                        editor.ActiveTextAreaControl.Caret.Position = End;
                    editor.ActiveTextAreaControl.SelectionManager.SetSelection(Beg,End);
                    SetFocusToEditor();
                    break;
                case SourceLocationAction.GotoBeg:
                case SourceLocationAction.GotoEnd:
                    if (Action == SourceLocationAction.GotoBeg)
                        editor.ActiveTextAreaControl.Caret.Position = Beg;
                    else
                        editor.ActiveTextAreaControl.Caret.Position = End;
                    SetFocusToEditor();
                    break;
            }
            CurrentSyntaxEditor.CenterView();
        }

        void VisualEnvironmentCompiler_ChangeVisualEnvironmentState(VisualEnvironmentState State, object obj)
        {
            switch (State)
            {
                case VisualEnvironmentState.StartCompilerLoading:
                    this.Text = String.Format(FTSFormat,MainFormText,Form1StringResources.Get("FTS_LOADING"));
                    break;
                case VisualEnvironmentState.FinishCompilerLoading:
                    //this.Text = String.Format("{0} v{1}",MainFormText,PascalABCCompiler.Compiler.ShortVersion);
                    this.Text = MainFormText;
                    openFileDialog1.Filter = saveFileDialog1.Filter = VisualEnvironmentCompiler.GetFilterForDialogs();
                    break;
            }
        }




        private void edit1_Load(object sender, EventArgs e)
        {

        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if ((tabControl1.SelectedTab.Tag as Data).DocumentChanged)
            {
                miSave.Enabled = true;
                miSaveAs.Enabled = true;
                miSaveAll.Enabled = true;
                miClose.Enabled = true;
                tbSave.Enabled = true;
                tbSaveAll.Enabled = true;
            }
            else
            {
                miSave.Enabled = false;
                miSaveAs.Enabled = false;
                miSaveAll.Enabled = false;
                miClose.Enabled = false;
                tbSave.Enabled = false;
//                toolStripButton4.Enabled = false;
            }
        }

        

        public string Compile(string FileName, bool rebuild, string RuntimeServicesModule)
        {
            CompilerConsole.Clear();

            CompilerOptions1.SourceFileName = FileName;

            if (Path.GetDirectoryName(FileName) == "" && WorkingDirectoryExsist)
                CompilerOptions1.OutputDirectory = WorkingDirectory;

            CompilerOptions1.Rebuild = rebuild;

            CompilerOptions1.RemoveStandartModuleAtIndex(1);
            string runtimeModuleFileName = CompilerOptions1.SearchDirectory + "\\" + RuntimeServicesModule;
            if (RuntimeServicesModule!=null && File.Exists(runtimeModuleFileName))
                CompilerOptions1.StandartModules.Add(new PascalABCCompiler.CompilerOptions.StandartModule(runtimeModuleFileName, PascalABCCompiler.CompilerOptions.StandartModuleAddMethod.RightToMain));


            ErrorsList.Clear();

            //CompilerOptions1.SavePCUInThreadPull = true;

            string ofn = VisualEnvironmentCompiler.Compile(CompilerOptions1);

            ShowWarnings(VisualEnvironmentCompiler.Compiler.Warnings);
            if (VisualEnvironmentCompiler.Compiler.ErrorsList.Count != 0)
            {
                ShowErrors(VisualEnvironmentCompiler.Compiler.ErrorsList,false);            
            }
            return ofn;
        }

        delegate void ShowErrorsDelegate(List<PascalABCCompiler.Errors.Error> errors, bool IgnoreErrorsManager);
        public void ShowErrorsSync(List<PascalABCCompiler.Errors.Error> errors, bool IgnoreErrorsManager)
        {
            BeginInvoke(new ShowErrorsDelegate(ShowErrors), errors, IgnoreErrorsManager);
        }
        public void ShowErrors(List<PascalABCCompiler.Errors.Error> errors, bool IgnoreErrorsManager)
        {
            PascalABCCompiler.Errors.LocatedError er;
            BottomTabControl.SelectedTab = BottomTabControl.TabPages[1];
            int i = 0;
            //ivan
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(8);
            //\ivan
            if (dataGridView1.Rows.Count < errors.Count)
                dataGridView1.Rows.Add(errors.Count - dataGridView1.Rows.Count);
            if (IgnoreErrorsManager)
                ErrorsList = errors;
            else
                ErrorsList = ErrorsManager.CreateErrorsList(errors);
            foreach (PascalABCCompiler.Errors.Error Err in ErrorsList)
            {
                dataGridView1.Rows[i].Cells[ErrorListNumCol].Value = i + 1;

                if (Err is PascalABCCompiler.Errors.LocatedError)
                {
                    er = Err as PascalABCCompiler.Errors.LocatedError;
                    dataGridView1.Rows[i].Cells[ErrorListDescrCol].Value = er.Message;
                    if ((Err as PascalABCCompiler.Errors.LocatedError).SourceLocation != null)
                    {
                        dataGridView1.Rows[i].Tag = (Err as PascalABCCompiler.Errors.LocatedError).SourceLocation;
                        //ivan
                        dataGridView1.Rows[i].Cells[0] = new DataGridViewImageCell();
                        //(dataGridView1.Rows[i].Cells[0] as DataGridViewImageCell).ValueIsIcon = false;
                        dataGridView1.Rows[i].Cells[0].ReadOnly = true;
                        dataGridView1.Rows[i].Cells[0].Value = new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VisualPascalABC.Resources.Icons.16x16.Error.png"));
                        //\ivan
                        dataGridView1.Rows[i].Cells[ErrorListFileCol].Value = Path.GetFileName(er.SourceLocation.FileName);
                        dataGridView1.Rows[i].Cells[ErrorListPathCol].Value = Path.GetDirectoryName(er.SourceLocation.FileName);
                        dataGridView1.Rows[i].Cells[ErrorListLineCol].Value = er.SourceLocation.BeginPosition.Line;
                        //dataGridView1.Rows[i].Cells[4].Value = (Err as PascalABCCompiler.Errors.CompilationError).SourceLocation.BeginPosition.Column;
                    }
                    /*else
                    {
                        dataGridView1.Rows[i].Cells[3].Value = "";
                        dataGridView1.Rows[i].Cells[4].Value = "";
                    }*/


                }
                if (Err is PascalABCCompiler.Errors.CompilerInternalError)
                {
                    dataGridView1.Rows[i].Cells[ErrorListDescrCol].Value = (Err as PascalABCCompiler.Errors.CompilerInternalError).ToString();
                }
                i++;
            }
            if (!BottomTabsVisible)
                BottomTabsVisible = true;
            if ((er = ErrorsList[0] as PascalABCCompiler.Errors.LocatedError) != null)
                if (er.SourceLocation != null)
                {
                    ExecuteErrorPos(er.SourceLocation);
                    //OpenFile(er.SourceLocation.FileName);
                    //(tabControl1.SelectedTab.Tag as Data).OwnSyntaxEditor.CaretPosition(er.SourceLocation.BeginPosition.Line,er.SourceLocation.BeginPosition.Column);
                }

        }

        internal void ExecuteErrorPos(PascalABCCompiler.SourceLocation sl)
        {
            ExecuteSourceLocationAction(sl, ErrorCursorPosStrategy);
        }

 
        
        public bool Run(bool Debug)
        {
            UserOptions.RedirectConsoleIO = Debug;
            return Run(CurrentTabPage);
        }

        bool RunActiveTabPage = false;
        public bool Run(TabPage tabPage)
        {
            outputTextBox.Clear();
            ErrorListClear();
            //this.Refresh();
//            new System.Threading.Thread(new System.Threading.ThreadStart(RunCompile)).Start();;
            string runtimeModule;
            string ModeName;
            if (UserOptions.RedirectConsoleIO)
            {
                runtimeModule = RedirectIOModeModuleName;
                ModeName = RedirectIOModeName;
            }
            else
            {
                runtimeModule = RunModeModuleName;
                ModeName = RunModeName;
            }
            string OutputFileName = Compile((tabPage.Tag as Data).FullPathName, false, runtimeModule);
            if (OutputFileName != null)
            {
                switch (VisualEnvironmentCompiler.Compiler.CompilerOptions.OutputFileType)
                {
                    case PascalABCCompiler.CompilerOptions.OutputType.ClassLibrary:
                        MessageBox.Show(Form1StringResources.Get("RUN_DLL_WARNING_TEXT"), PascalABCCompiler.StringResources.Get("!WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        RunActiveTabPage = false;
                        break;
                    case PascalABCCompiler.CompilerOptions.OutputType.PascalCompiledUnit:
                        if (ActiveTabPage != null && ActiveTabPage != CurrentTabPage && !(ActiveTabPage.Tag as Data).Run)
                            if (!RunActiveTabPage)
                            {
                                RunActiveTabPage = true;
                                return Run(ActiveTabPage);
                            }
                        RunActiveTabPage = false;
                        MessageBox.Show(Form1StringResources.Get("RUN_PCU_WARNING_TEXT"), PascalABCCompiler.StringResources.Get("!WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case PascalABCCompiler.CompilerOptions.OutputType.ConsoleApplicaton:
                    case PascalABCCompiler.CompilerOptions.OutputType.WindowsApplication:
                        //System.Diagnostics.Process.Start(Path.ChangeExtension((tabControl1.SelectedTab.Tag as Data).FullPathName,"exe"));
                        if (UserOptions.RedirectConsoleIO && BottomTabControl.SelectedTab != BottomTabControl.TabPages[2])
                            BottomTabControl.SelectedTab = BottomTabControl.TabPages[0];

                        //System.Diagnostics.ProcessStartInfo ps = new System.Diagnostics.ProcessStartInfo(VisualEnvironmentCompiler.Compiler.CompilerOptions.OutputFileName);
                        //ps.WorkingDirectory = Path.GetDirectoryName(VisualEnvironmentCompiler.Compiler.CompilerOptions.OutputFileName);
                        //ps.UseShellExecute = false;
                        // ps.CreateNoWindow = true;
                        /*if (UserOptions1.ConsoleOutput)
                        {
                            ps.RedirectStandardOutput = true;                        
                            ps.CreateNoWindow = true;
                        }
                        else
                        {
                            ps.RedirectStandardOutput = false;
                            ps.CreateNoWindow = false;
                        }
                        */
                        /*RunProcessOptions1.Process = new System.Diagnostics.Process();
                        RunProcessOptions1.Process.StartInfo=ps;
                        RunProcessOptions1.Process.EnableRaisingEvents = true;
                        RunProcessOptions1.Process.Exited += new EventHandler(Process_Exited);
                        if (UserOptions1.ConsoleOutput)
                        {
                            RunProcessOptions1.Process.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(Process_OutputDataReceived);
                            RunProcessOptions1.Process.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(Process_OutputDataReceived);
                            //textBox1.Text += RunProcessOptions1.Process.StandardOutput.ReadToEnd();
                            //RunProcessOptions1.Process.WaitForExit();
                            //RunProcessOptions1.Process = null;
                        }
                        RunProcessOptions1.Process.Start();*/

                        RunActiveTabPage = false;
                        RunnerManager.Run(VisualEnvironmentCompiler.Compiler.CompilerOptions.OutputFileName, UserOptions.RedirectConsoleIO, ModeName);
                        RunTabs[VisualEnvironmentCompiler.Compiler.CompilerOptions.OutputFileName] = tabPage;
                        ActiveTabPage = tabPage;
                        //if (UserOptions1.ConsoleOutput)
                            //AddTextToOutputWindow("Программа запущена");
                        return true;
                }
            }
            return false;
        }


        public string CurrentSourceFileName
        {
            get { return (tabControl1.SelectedTab.Tag as Data).FullPathName; }
        }
        string CurrentEXEFileName
        {
            get { return Path.ChangeExtension(CurrentSourceFileName, ".exe"); }
        }
        void RunnerManager_Started(string fileName)
        {
            BeginInvoke(new SetTextDelegate(RunnerManager_Started_Sync),fileName);
        }
        void RunnerManager_Started_Sync(string fileName)
        {
            if (CurrentEXEFileName == fileName)
            {
                SetStopEnabled(true);
                SetCompilingButtonsEnabled(false);
            }
            //(Tabs[fileName]).Text = (Tabs[fileName]).Text + string.Format(" [{0}]", PascalABCCompiler.StringResources.Get("VP_MF_TS_RUN"));
            ((RunTabs[fileName]).Tag as Data).Run = true;
            SetTabPageText(RunTabs[fileName]);
        }
        void RunnerManager_Exited(string fileName)
        {
            if (System.Threading.Thread.CurrentThread != MainProgramThread)
                BeginInvoke(new SetTextDelegate(RunnerManager_Exited_Sync), fileName);
            else
                RunnerManager_Exited_Sync(fileName);
        }
        void RunnerManager_Exited_Sync(string fileName)
        {
            if (CurrentEXEFileName == fileName)
            {
                SetStopEnabled(false);
                SetCompilingButtonsEnabled(true);
            }
            ((RunTabs[fileName]).Tag as Data).Run = false;
            SetTabPageText(RunTabs[fileName]);
            //Tabs.Remove(fileName);
            SetFocusToEditor();
            if (ReadRequests.ContainsKey(RunTabs[fileName]))
                ReadRequests.Remove(RunTabs[fileName]);
            UpdateReadRequest(false);
            if(TerminateAllPrograms)
                WaitCallback_DeleteEXEAndPDB(fileName);
            else
                System.Threading.ThreadPool.QueueUserWorkItem(WaitCallback_DeleteEXEAndPDB,fileName);
            
        }

        void WaitCallback_DeleteEXEAndPDB(object state)
        {
            int i = 0;
            string fileName = (string)state;
            while (i < 20)
            {
                try
                {
                    if (UserOptions.DeleteEXEAfterExecute)
                        File.Delete(fileName);
                    if (UserOptions.DeletePDBAfterExecute)
                    {
                        string pdbFileName = Path.ChangeExtension(fileName, ".pdb");
                        if (File.Exists(pdbFileName))
                            File.Delete(pdbFileName);
                    }
                    return;
                }
                catch
                {
                }
                System.Threading.Thread.Sleep(20);
                i++;
            }
        }

        void Process_Exited(object sender, EventArgs e)
        {
            
        }

        private delegate void SetBoolDelegate(bool b);
        private void SetStopEnabled(bool enabled)
        {
            stopButton.Enabled = miStop.Enabled = enabled;
        }

        void Process_Disposed(object sender, EventArgs e)
        {
            StartButton.Enabled = true;

            //throw new Exception("The method or operation is not implemented.");
        }

        private void OpenLastFile_ToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            OpenFile((sender as ToolStripMenuItem).Tag as string);         
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton19_Click(object sender, EventArgs e)
        {
            this.outputTextBox.Text = "";
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            
        }

        private void шрифтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {
            tabControl1.SelectedTab.Font = fontDialog1.Font;
            /*foreach (Control edit in tabControl1.SelectedTab.Controls)
                if (edit is Rsdn.Editor.Edit)
                {
                    (edit as Rsdn.Editor.Edit).Font = fontDialog1.Font;
                    //(edit as Rsdn.Editor.Edit).D
                }*/
        }

        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            //textEditorControl1.Document.TextEditorProperties.Font = fontDialog1.Font;
            //textEditorControl1.TextEditorProperties.
            //textEditorControl1.Text = " if(font); using namespace colors; ";
            
        }

        private void опцииКомпиляцииToolStripMenuItem_Click(object sender, EventArgs e)
        {           
            
        }

        private void видToolStripMenuItem1_Click(object sender, EventArgs e)
        {            
            
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_Resize(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_ClientSizeChanged(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_SizeChanged(object sender, EventArgs e)
        {
            ErrorListAddRowsFill();
        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            ErrorListAddRowsFill();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            Build();
        }

        private bool Build()
        {
            outputTextBox.Clear();
            ErrorListClear();
            return Compile((tabControl1.SelectedTab.Tag as Data).FullPathName, false, null)!=null;
        }
        private bool Rebuild()
        {
            outputTextBox.Clear();
            ErrorListClear();
            return Compile((tabControl1.SelectedTab.Tag as Data).FullPathName, true, null)!=null;
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            Rebuild();
        }
        
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                if (dataGridView1.Rows[e.RowIndex].Tag != null)
                    ExecuteErrorPos((PascalABCCompiler.SourceLocation)dataGridView1.Rows[e.RowIndex].Tag);
             /*if (e.RowIndex > -1)
              {
                  if ((dataGridView1.Rows[e.RowIndex].Cells[3].Value != null) && (dataGridView1.Rows[e.RowIndex].Cells[0].Value != null) && (dataGridView1.Rows[e.RowIndex].Cells[2].Value != null) && (dataGridView1.Rows[e.RowIndex].Cells[1].Value != null))
                  {
                      foreach (TabPage tbPage in tabControl1.TabPages)
                          if (((Data)tbPage.Tag).FullPathName == dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString())                              
                                  {
                                      (tabControl1.SelectedTab.Tag as Data).OwnSyntaxEditor.CaretPosition((int)dataGridView1.Rows[e.RowIndex].Cells[3].Value, (int)dataGridView1.Rows[e.RowIndex].Cells[4].Value);  
                                       toolStripStatusLabel3.Text = ((int)(dataGridView1.Rows[e.RowIndex].Cells[3].Value)).ToString();
                                       toolStripStatusLabel4.Text = ((int)(dataGridView1.Rows[e.RowIndex].Cells[4].Value)).ToString();
                                  }
                  }
              }*/             
        }

        private void tabPage1_Click_1(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {

        }
        public void CahngedSelectedTab()
        {
            LastSelectedTab = BakSelectedTab;
            BakSelectedTab = tabControl1.SelectedTab;

            if (OutputTextBoxs[tabControl1.SelectedTab] != outputTextBox)
            {
                OutputTextBoxs[tabControl1.SelectedTab].Visible = true;
                outputTextBox.Visible = false;
                outputTextBox = OutputTextBoxs[tabControl1.SelectedTab];
            }

            SetFocusToEditor();
            bool run = RunnerManager.IsRun(CurrentEXEFileName);
            SetStopEnabled(run);
            if (VisualEnvironmentCompiler!=null)
                SetCompilingButtonsEnabled(!run);
            else
                SetCompilingButtonsEnabled(false);
            SaveButtonsEnabled = (tabControl1.SelectedTab.Tag as Data).DocumentChanged;
            UpdateUndoRedoEnabled();
            UpdateCutCopyButtonsEnabled();
            UpdateLineColPosition();
            UpdateReadRequest(true);
        }
        void UpdateUndoRedoEnabled()
        {
            UndoButtonsEnabled = (tabControl1.SelectedTab.Tag as Data).OwnSyntaxEditor.CanUndo;
            RedoButtonsEnabled = (tabControl1.SelectedTab.Tag as Data).OwnSyntaxEditor.CanRedo;
        }
        private void tabControl1_Selected_1(object sender, TabControlEventArgs e)
        {
            CahngedSelectedTab();
        }
        private void ukvToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void компиляторToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CompilerForm1.ShowDialog();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1.ShowDialog();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string text=Text;
            //если запущены программы
            if (RunnerManager.Count > 0)
            {
                //завершаем все программы
                this.Text = string.Format(FTSFormat, MainFormText, Form1StringResources.Get("FTS_KILLPROCESS"));
                TerminateAllPrograms = true;
                RunnerManager.KillAll();
                TerminateAllPrograms = false;
                this.Text = text;
            }

            
            //Сохраняем Файлы
            if (!SaveAll(true))
            {
                e.Cancel = true;
                return;
            }

            //если стартовый поток запущен
            if (VisualEnvironmentCompiler.Starting())
            {
                //начинаем процедуру отмены старта компилятора
                this.Text = string.Format(FTSFormat, MainFormText, Form1StringResources.Get("FTS_CANCELLOADING"));
                VisualEnvironmentCompiler.AbortStaring();
            }
            else
                SaveOptions(OptionsFileName);

        }

        private void CompileButton_EnabledChanged(object sender, EventArgs e)
        {
            //CompileMenuItem.Enabled = CompileButton.Enabled;
        }

        private void StartButton_EnabledChanged(object sender, EventArgs e)
        {
            //StartMenuItem.Enabled = StartButton.Enabled;
        }

        private void ReCompileMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            
        }

        private void ReCompileButton_EnabledChanged(object sender, EventArgs e)
        {
            miRebuild.Enabled = ReCompileButton.Enabled;
        }

        private void splitContainer1_Panel2_SizeChanged(object sender, EventArgs e)
        {
            
        }

        private void заменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {

        }

        private void miCurt_Click(object sender, EventArgs e)
        {
            (tabControl1.SelectedTab.Tag as Data).OwnSyntaxEditor.Cut();
        }

        private void miCopy_Click(object sender, EventArgs e)
        {
            (tabControl1.SelectedTab.Tag as Data).OwnSyntaxEditor.Copy();
        }

        private void miPaste_Click(object sender, EventArgs e)
        {
            (tabControl1.SelectedTab.Tag as Data).OwnSyntaxEditor.Paste();
        }

        private void miCompilerOptions_Click(object sender, EventArgs e)
        {
            CompileOptionsForm1.ShowDialog();
        }

        private void miViewOptions_Click(object sender, EventArgs e)
        {
            UserOptionsForm1.ShowDialog();
        }

        private void miStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private bool Stop()
        {
            if (RunnerManager.IsRun(CurrentEXEFileName))
            {
                RunnerManager.Stop(CurrentEXEFileName);
                return true;
            }
            return false;
        }

        private void miSaveAll_Click(object sender, EventArgs e)
        {
        }

        private void miSave_Click(object sender, EventArgs e)
        {
            SaveSelFile(tabControl1.SelectedTab);
        }

        bool SaveAll(bool Question)
        {
            SaveCanceled = false;
            foreach (TabPage tp in tabControl1.TabPages)
                if ((tp.Tag as Data).DocumentChanged)
                    if (Question)
                    {
                        if (!QuestionAndSaveFile(tp))
                        {
                            SaveCanceled = true;
                            return false;
                        }
                    }
                    else
                        SaveSelFile(tp);
            return true;
        }
        private void miSaveAll_Click_1(object sender, EventArgs e)
        {
            SaveAll(false);
        }
        private void сохранитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void miClose_Click(object sender, EventArgs e)
        {
            CloseFile(tabControl1.SelectedTab);            
        }

        void CloseAllButThis(TabPage nonCloseTab)
        {
            SaveCanceled = false;
            TabPage bakTab = tabControl1.SelectedTab;
            /*foreach (TabPage tbPage in tabControl1.TabPages)
                if (tbPage != tabControl1.SelectedTab)
                    CloseFile(tbPage);*/
            int i = 0, c = tabControl1.TabPages.Count;
            while (i < c && !SaveCanceled)
            {
                TabPage tbPage = tabControl1.TabPages[i];
                if (tbPage != nonCloseTab)
                {
                    if (tbPage == bakTab)
                        bakTab = null;
                    CloseFile(tbPage);
                    if (c == tabControl1.TabPages.Count)
                        i++;
                    else
                        c = tabControl1.TabPages.Count;
                }
                else
                    i++;
            }
            if (bakTab!=null)
                if(bakTab != tabControl1.SelectedTab)
                    tabControl1.SelectedTab = bakTab;
        }
        private void miCloseNonActive_Click(object sender, EventArgs e)
        {
            CloseAllButThis(tabControl1.SelectedTab);
        }

        private void miNew_Click(object sender, EventArgs e)
        {
            /*bool FullTxt = false;
            string FileName;
            FullTxt = ((tabControl1.SelectedTab.Tag as Data).OwnSyntaxEditor.OwnEdit.Document.TextContent != "");
            if ((tabControl1.SelectedTab.Tag as Data).DocumentChanged || FullTxt)
            {
                tabControl1.TabPages.Add("");
                FileName = InstNameNewProgramm(tabControl1);
                //                tabControl1.TabPages[tabControl1.TabPages.Count - 1].Text = Path.GetFileName(FileName);
                //                tabControl1.TabPages[tabControl1.TabPages.Count - 1].Tag = new Data(FileName);
                AddNewProgramToTab(tabControl1, tabControl1.TabPages.Count - 1, Path.GetFileName(FileName));
                tabControl1.TabPages[tabControl1.TabPages.Count - 1].BorderStyle = BorderStyle.Fixed3D;
                tabControl1.SelectTab(tabControl1.TabPages.Count - 1);
                tabControl1.SelectedTab.ContextMenuStrip = contextMenuStrip1;
            }
            miClose.Enabled = true;
            miCloseAllButThis.Enabled = true;
            печатьToolStripMenuItem.Enabled = true;   */
            OpenFile(null);         

        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            if (InputPanelVisible)
                SetFocusToInputEditor();
            else
                SetFocusToEditor();
        }

        private void miSaveAs_Click(object sender, EventArgs e)
        {
            ExecuteSaveAs(tabControl1.SelectedTab);   
        }

        public void SelectAppType(PascalABCCompiler.CompilerOptions.OutputType outputType)
        {
            Image img=null;
            foreach (ToolStripMenuItem mi in tstaSelect.DropDownItems)
                if (outputType == (PascalABCCompiler.CompilerOptions.OutputType)mi.Tag)
                    img = mi.Image;
            tstaSelect.Image = img;
            CompilerOptions1.OutputFileType = outputType;
        }

        private void tsat_Click(object sender, EventArgs e)
        {
            SelectAppType((PascalABCCompiler.CompilerOptions.OutputType)((sender as ToolStripMenuItem).Tag));
        }

        private void StartMenuItem_Click(object sender, EventArgs e)
        {
            Run(true);
        }




        public void SetCompilingButtonsEnabled(bool Enabled)
        {
            StartNoDebugButton.Enabled = miRunNoDebug.Enabled = miBuild.Enabled = miRebuild.Enabled = miRun.Enabled = ReCompileButton.Enabled = CompileButton.Enabled = cmRun.Enabled = StartButton.Enabled = Enabled;
            toolStrip1.Refresh();
        }
        bool CompilingButtonsEnabled
        {
            get { return miRun.Enabled; }
            set { SetCompilingButtonsEnabled(value); }
        }
        bool SaveButtonsEnabled
        {
            set
            {
                cmSave.Enabled = tbSave.Enabled = miSave.Enabled = value;
            }
            get
            {
                return miSave.Enabled;
            }
        }
        bool SaveAllButtonsEnabled
        {
            set
            {
                tbSaveAll.Enabled = miSaveAll.Enabled = value;
            }
            get
            {
                return miSaveAll.Enabled;
            }
        }
        bool CloseButtonsEnabled
        {
            set
            {
                cmCloseAllButThis.Enabled = miCloseAllButThis.Enabled = miClose.Enabled = cmClose.Enabled = value;
            }
            get
            {
                return miClose.Enabled;
            }
        }
        bool UndoButtonsEnabled
        {
            get { return miUndo.Enabled; }
            set { miUndo.Enabled = tsUndo.Enabled = value; }
        }
        bool RedoButtonsEnabled
        {
            get { return miRedo.Enabled; }
            set { miRedo.Enabled = tsRedo.Enabled = value; }
        }
        bool CutButtonsEnabled
        {
            get { return miCut.Enabled; }
            set { cmCut.Enabled = miCut.Enabled = tsCut.Enabled = value; }
        }
        bool CopyButtonsEnabled
        {
            get { return miCopy.Enabled; }
            set { cmCopy.Enabled = miCopy.Enabled = tsCopy.Enabled = value; }
        }
        bool NavigBackButtonsEnabled
        {
            get { return miNavigBack.Enabled; }
            set { miNavigBack.Enabled = tsNavigBack.Enabled = value; }
        }
        bool NavigForwButtonsEnabled
        {
            get { return miNavigForw.Enabled; }
            set { miNavigForw.Enabled = tsNavigForw.Enabled = value; }
        }


        public void UpdateUserOptions()
        {
            foreach (SyntaxEditor si in AllSyntaxEditors)
            {
                if (si.OwnEdit.ShowLineNumbers != UserOptions.ShowLineNums)
                    si.OwnEdit.ShowLineNumbers = UserOptions.ShowLineNums;
                if (si.OwnEdit.ShowMatchingBracket != UserOptions.ShowMathBraket)
                    si.OwnEdit.ShowMatchingBracket = UserOptions.ShowMathBraket;
                if (si.FontSize != UserOptions.EditorFontSize)
                    si.FontSize = UserOptions.EditorFontSize;
                if (si.OwnEdit.ConvertTabsToSpaces != UserOptions.ConverTabsToSpaces)
                    si.OwnEdit.ConvertTabsToSpaces = UserOptions.ConverTabsToSpaces;
                if (si.OwnEdit.TabIndent != UserOptions.TabIndent)
                    si.OwnEdit.TabIndent = UserOptions.TabIndent;
            }
        }
        public SyntaxEditor[] AllSyntaxEditors
        {
            get
            {
                SyntaxEditor[] arr=new SyntaxEditor[tabControl1.TabPages.Count];
                for (int i = 0; i < tabControl1.TabPages.Count;i++ )
                {
                    arr[i] = (tabControl1.TabPages[i].Tag as Data).OwnSyntaxEditor;
                }
                return arr;
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyData == Keys.Escape && BottomTabsVisibleChekerEnabled)
            //    ButtomTabsVisible = false;

        }
        public void UpdateLineColPosition()
        {
            slLine.Text = CurrentSyntaxEditor.CaretPosition.Y.ToString();
            slCol.Text = CurrentSyntaxEditor.CaretPosition.X.ToString();
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void miFind_Click(object sender, EventArgs e)
        {
            FindForm.ShowDialog();
        }

        private void miReplace_Click(object sender, EventArgs e)
        {
            ReplaceForm.ShowDialog();
            if (ReplaceForm.ReplaceAllExec)
            while (ReplaceForm.findSuccess)
            {
                ReplaceForm.ReplaceConfirmDlg.lMessage.Text = String.Format(PascalABCCompiler.StringResources.Get("VP_REPLACECONFIRMDLGFORM_REPLACE{0}TO{1}"), ReplaceForm.Match.Value,ReplaceForm.tbTextToReplace.Text);
                ReplaceForm.ReplaceConfirmDlg.ShowDialog();
                if (ReplaceForm.ReplaceConfirmDlg.Result == ReplaceConfirmDialog.ModalResult.Yes)
                    ReplaceForm.Replace();
                else
                    if (ReplaceForm.ReplaceConfirmDlg.Result == ReplaceConfirmDialog.ModalResult.Cancel)
                        return;
                    else
                        if (ReplaceForm.ReplaceConfirmDlg.Result == ReplaceConfirmDialog.ModalResult.All)
                            ReplaceForm.ReplaceAll();
                ReplaceForm.FindNext();
            }
        }

        private void найтиДалееToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindForm.FindNext();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        public TabPage CurrentTabPage
        {
            get {return tabControl1.SelectedTab;}
            set { tabControl1.SelectedTab = value; }
        }
        public TabPage ActiveTabPage
        {
            get { return activeTabPage; }
            set
            {
                if (activeTabPage != value)
                {
                    TabPage tp = ActiveTabPage;
                    activeTabPage = CurrentTabPage;
                    if (tp != null)
                        SetTabPageText(tp);
                    SetTabPageText(activeTabPage);
                }
            }

        }
        private void cmSetActive_Click(object sender, EventArgs e)
        {
            ActiveTabPage = CurrentTabPage;
        }

        private void miOutputWindow_Click(object sender, EventArgs e)
        {
            BottomTabsVisible = !BottomTabsVisible;
        }

        internal void UpdateEditorFontSize()
        {
            foreach (SyntaxEditor si in AllSyntaxEditors)
            {
            }
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }


        void SendInputTextToProcess()
        {
            RunnerManager.WritelnStringToProcess(CurrentEXEFileName, InputTextBox.Text);
            outputTextBox.AppendText(Environment.NewLine);
            InputPanelVisible = false;
            ReadRequests.Remove(CurrentTabPage);
        }

        void SetFocusToEditor()
        {
            CurrentSyntaxEditor.OwnEdit.Focus();
        }
        void SetFocusToInputEditor()
        {
            InputTextBox.Focus();
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyData)
            {
                case Keys.Enter:
                    SendInputTextToProcess();
                    break;
            }
            if (!e.Alt && !e.Control)
                addThisChar = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendInputTextToProcess();
        }

        private void miRunNoDebug_Click(object sender, EventArgs e)
        {
            Run(false);
        }

        private void tabControl2_Click(object sender, EventArgs e)
        {
            if (InputPanelVisible)
                SetFocusToInputEditor();
            else
                SetFocusToEditor();
        }
        bool addThisChar = false;
        private void InputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!addThisChar)
                e.KeyChar = '\0';
            addThisChar = false;
        }

        Dictionary<TabPage, string> lastInputTexts = new Dictionary<TabPage, string>();
        string lastInputText
        {
            get
            {
                if (lastInputTexts.ContainsKey(CurrentTabPage))
                    return lastInputTexts[CurrentTabPage];
                else
                    return "";
            }
            set
            {
                if (value == "")
                    if (lastInputTexts.ContainsKey(CurrentTabPage))
                    {
                        lastInputTexts.Remove(CurrentTabPage);
                        return;
                    }
                if (lastInputTexts.ContainsKey(CurrentTabPage))
                    lastInputTexts[CurrentTabPage] = value;
                else
                    lastInputTexts.Add(CurrentTabPage, value);
            }
        }
        private void InputTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!InputPanelVisible)
                return;
            if (InputTextBox.Lines.Length > 1)
                InputTextBox.Text = InputTextBox.Lines[0];
            string ltext = lastInputText;
            string text = InputTextBox.Text;
            if ((text.Length > 0) && (ltext == text.Substring(0, text.Length - 1)))
            {
                outputTextBox.AppendText(text[text.Length - 1].ToString());
                OutputTextBoxScrolToEnd();
                lastInputText = text;
            }
            else
            {
                outputTextBox.Text = outputTextBox.Text.Substring(0, outputTextBox.Text.Length - ltext.Length) + InputTextBox.Text;
                OutputTextBoxScrolToEnd();
                lastInputText = text;
            }
            
        }
    }


}