// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ICSharpCode.FormsDesigner;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.TextEditor;
using VisualPascalABC.OptionsContent;
using VisualPascalABCPlugins;
using WeifenLuo.WinFormsUI.Docking;

namespace VisualPascalABC
{
    public partial class Form1 : Form, VisualPascalABCPlugins.IWorkbench, VisualPascalABCPlugins.IWorkbenchOperationsService
    {
        internal CodeFileDocumentControl LastSelectedTab, BakSelectedTab;
        Dictionary<ICodeFileDocument, RichTextBox> OutputTextBoxs = new Dictionary<ICodeFileDocument, RichTextBox>();

        private void AddOutputWindow()
        {
            if (OutputWindow == null)
            {
                OutputWindow = new OutputWindowForm(this);
                Form1StringResources.SetTextForAllControls(OutputWindow);
            }
            BottomPane = null;
            AddWindowToDockPanel(OutputWindow, MainDockPanel, OutputWindow.Dock, DockState.DockBottom, OutputWindow.IsFloat, BottomPane, -1);
            BottomPane = OutputWindow.Pane;
        }

        private void AddErrorsListWindow()
        {
            if (ErrorsListWindow == null)
            {
                ErrorsListWindow = new ErrorsListWindowForm(this);
                Form1StringResources.SetTextForAllControls(ErrorsListWindow);
            }
            AddWindowToDockPanel(ErrorsListWindow, MainDockPanel, OutputWindow.Dock, DockState.DockBottom, OutputWindow.IsFloat, BottomPane, int.MaxValue);
        }

        private void AddFindSymbolsResultWindow()
        {
            if (FindSymbolsResultWindow == null)
            {
                FindSymbolsResultWindow = new FindSymbolsResultWindowForm(this);
                Form1StringResources.SetTextForAllControls(FindSymbolsResultWindow);
            }
            AddWindowToDockPanel(FindSymbolsResultWindow, MainDockPanel, OutputWindow.Dock, DockState.DockBottom, OutputWindow.IsFloat, BottomPane, int.MaxValue);
        }

        private void AddImmediateWindow()
        {
            if (ImmediateWindow == null)
            {
                ImmediateWindow = new ImmediateWindow(this);
                Form1StringResources.SetTextForAllControls(ImmediateWindow);
            }
            AddWindowToDockPanel(ImmediateWindow, MainDockPanel, OutputWindow.Dock, DockState.DockBottom, OutputWindow.IsFloat, BottomPane, int.MaxValue);
        }

        private void AddCompilerConsoleWindow()
        {
            if (CompilerConsoleWindow == null)
            {
                CompilerConsoleWindow = new CompilerConsoleWindowForm(this);
                Form1StringResources.SetTextForAllControls(CompilerConsoleWindow);
            }
            AddWindowToDockPanel(CompilerConsoleWindow, MainDockPanel, OutputWindow.Dock, DockState.DockBottom, OutputWindow.IsFloat, BottomPane, int.MaxValue);
        }

        void AddDebugVariablesListWindow()
        {
            if (DebugVariablesListWindow == null)
            {
                DebugVariablesListWindow = new DebugVariablesListWindowForm(this);
                Form1StringResources.SetTextForAllControls(DebugVariablesListWindow);
            }
            AddWindowToDockPanel(DebugVariablesListWindow, MainDockPanel, OutputWindow.Dock, DockState.DockBottom, OutputWindow.IsFloat, BottomPane, int.MaxValue);
        }

        void AddDisassemblyWindow()
        {
            if (DisassemblyWindow == null)
            {
                DisassemblyWindow = new DisassemblyWindow(this);
                Form1StringResources.SetTextForAllControls(DisassemblyWindow);
            }

            AddWindowToDockPanel(DisassemblyWindow, MainDockPanel, OutputWindow.Dock, DockState.DockBottom, OutputWindow.IsFloat, BottomPane, int.MaxValue);
        }

        void AddDebugWatchListWindow()
        {
            if (DebugWatchListWindow == null)
            {
                DebugWatchListWindow = new DebugWatchListWindowForm(this);
                Form1StringResources.SetTextForAllControls(DebugWatchListWindow);
            }

            AddWindowToDockPanel(DebugWatchListWindow, MainDockPanel, OutputWindow.Dock, DockState.DockBottom, OutputWindow.IsFloat, BottomPane, int.MaxValue);
        }

        void AddProjectExplorerWindow()
        {
            if (ProjectExplorerWindow == null)
            {
                ProjectExplorerWindow = new ProjectExplorerForm();
                Form1StringResources.SetTextForAllControls(ProjectExplorerWindow);
            }
            ProjectPane = null;
            AddWindowToDockPanel(ProjectExplorerWindow, MainDockPanel, ProjectExplorerWindow.Dock, DockState.DockLeft, OutputWindow.IsFloat, ProjectPane, -1);
            ProjectPane = ProjectExplorerWindow.Pane;
        }

        private void AddOptionsContent()
        {
            optionsContentEngine = new OptionsContentEngine(this);
            optionsContentEngine.AddContent(new ViewOptionsContent(this));
            optionsContentEngine.AddContent(new EditorOptionsContent(this));
            optionsContentEngine.AddContent(new CompilerOptionsContent(this));
            optionsContentEngine.AddContent(new IntelliseseOptionsContent(this));
        }

        public void UpdateOptionsForm()
        {
            if (optionsContentEngine != null)
                optionsContentEngine.UpdateOptionsForm();
        }

        public void AddWindowToDockPanel(DockContent dc, DockPanel dp, DockStyle dockStyle, DockState dockState, bool isFloat, DockPane dockToPane, int ind)
        {
            if (dc.Visible && dc.Pane != null)
                return;
            if (dp.DocumentStyle == DocumentStyle.SystemMdi)
            {
                dc.MdiParent = this;
                dc.Show();
            }
            else
            {
                dc.Show(dp);
            }

            if (dockToPane != null)
                if (ind == int.MaxValue)
                    dc.Pane.DockTo(dockToPane, dockStyle, dockToPane.Contents.Count - 1);
                else
                    dc.Pane.DockTo(dockToPane, dockStyle, ind);
            dc.DockState = dockState;
            dc.IsFloat = isFloat;
        }

        public void AddToolBox()
        {
            if (ToolBoxWindow == null)
            {
                ToolBoxWindow = new ToolBoxForm();
                SharpDevelopSideBar sideBar = FormsDesignerViewContent.FormsDesignerToolBox;
                sideBar.Dock = DockStyle.Fill;
                sideBar.Parent = ToolBoxWindow;
                AddWindowToDockPanel(ToolBoxWindow, MainDockPanel, OutputWindow.Dock, DockState.DockLeft, OutputWindow.IsFloat, null, -1);
                ToolBoxWindow.Show();
            }
            else
                ToolBoxWindow.Show();
        }

        public void AddPropertiesWindow()
        {
            if (PropertiesWindow == null)
            {
                PropertiesWindow = new PropertiesForm();
                Form1StringResources.SetTextForAllControls(PropertiesWindow);
                Panel properties = FormsDesignerViewContent.PropertyPad.PropertyPadPanel;
                properties.Dock = DockStyle.Fill;
                properties.Parent = PropertiesWindow;
                AddWindowToDockPanel(PropertiesWindow, MainDockPanel, DockStyle.Fill, DockState.DockRight, OutputWindow.IsFloat, ProjectPane, int.MaxValue);
                //PropertiesWindow.Show();
            }
            else PropertiesWindowVisible = true;
        }

        private CodeFileDocumentControl AddNewTab(DockPanel tabControl, DockStyle dockStyle = DockStyle.Fill)
        {
            CodeFileDocumentControl tp = new CodeFileDocumentControl(this);
            //tp.BorderStyle=BorderStyle.Fixed3D;
            //tp.Font = new System.Drawing.Font("MS Sans Serif", tp.Font.Size);
            RichTextBox tb = OutputWindow.outputTextBox;
            if (OpenDocuments.Count > 0)
                tb = CopyTextBox(OutputWindow.outputTextBox);
            AddWindowToDockPanel(tp, tabControl, tp.Dock != dockStyle ? dockStyle : tp.Dock, DockState.Document, tp.IsFloat, null, 0);
            tb.Font = tp.TextEditor.Font;
            OutputTextBoxs.Add(tp, tb);

            WorkbenchServiceFactory.CodeCompletionParserController.ParseInformationUpdated += tp.TextEditor.UpdateFolding;
            return tp;
            //tabControl.SelectedTab = tp;
        }

        public CodeFileDocumentControl FindTab(string FileName)
        {
            if (FileName == null) return null;
            CodeFileDocumentControl tp = null;
            OpenDocuments.TryGetValue(Tools.FileNameToLower(FileName), out tp);
            return tp;
        }

        public ICodeFileDocument GetDocument(string FileName)
        {
            return FindTab(FileName);
        }

        bool VisualPascalABCPlugins.IWorkbenchDocumentService.ContainsTab(string FileName)
        {
            return FindTab(FileName) != null;
        }

        public void CloseBrowserTab(string text)
        {
            if (OpenBrowserDocuments.ContainsKey(text))
                OpenBrowserDocuments.Remove(text);
        }

        public void AddTabWithUrl(string title, string url)
        {
            AddTabWithUrl(MainDockPanel, title, url);
        }

        public void AddTabWithUrl(DockPanel tabControl, string title, string url)
        {
            WebBrowserControl tp = null;//new WebBrowserControl();
            if (!OpenBrowserDocuments.TryGetValue(title, out tp))
            {
                tp = new WebBrowserControl();
                tp.OpenPage(title, url);
                AddWindowToDockPanel(tp, tabControl, tp.Dock, DockState.Document, tp.IsFloat, null, 0);
                OpenBrowserDocuments.Add(title, tp);
            }
            else
                tp.Activate();
        }

        private CodeFileDocumentControl AddNewProgramToTab(DockPanel tabControl, string FileName)
        {
            CodeFileDocumentControl edit = AddNewTab(tabControl);

            edit.FileName = FileName;
            SetTabPageText(edit);
            edit.SetHighlightingStrategyForFile(FileName);
            OpenDocuments.Add(Tools.FileNameToLower(FileName), edit);
            //this.codeCompletionParserController.SetAsChanged(file_name);
            //ivan
            AddBreakPointHandler(edit, FileName);
            var RunService = WorkbenchServiceFactory.RunService;
            if (!RunService.HasRunArgument(FileName.ToLower()))
                RunService.AddRunArgument(FileName.ToLower(), "");
            if (!WorkbenchServiceFactory.DebuggerManager.IsRunning)
            {
                AddEditorHandlers(edit);
            }
            CodeCompletionKeyHandler.Attach(edit.TextEditor);
            edit.TextEditor.ActiveTextAreaControl.TextArea.KeyEventHandler += TextArea_KeyEventHandler;

            //HostCallbackImplementation.Register(this);

            //\ivan
            return edit;
        }

        private void AddDebugPage(CodeFileDocumentControl tp)
        {
            if (TabStack.Count > 0 && !TabStack.Contains(tp))
            {
                CodeFileDocumentControl cfdc = TabStack[TabStack.Count - 1];
                //TabStack.RemoveAt(TabStack.Count-1);
                RichTextBox tb = null;
                if (OutputTextBoxs.ContainsKey(cfdc))
                {
                    tb = OutputTextBoxs[cfdc];
                    RichTextBox new_tb = OutputTextBoxs[tp];
                    new_tb.Text = tb.Text;
                }
                OutputWindow.OutputTextBoxScrolToEnd();
            }
            if (!TabStack.Contains(tp))
            {
                TabStack.Add(tp);
                OutputBoxStack.Add(OutputTextBoxs[tp]);
            }
            if (DebugTabs[tp] != null) return;
            DebugTabs[tp] = tp.TextEditor.ActiveTextAreaControl.TextArea;
            TextAreaHelper.TextAreas[tp.TextEditor.ActiveTextAreaControl.TextArea] = tp.FileName;
            tp.TextEditor.ActiveTextAreaControl.TextArea.ToolTipRequest += WorkbenchServiceFactory.DebuggerManager.TextAreaToolTipRequest;

        }

        private RichTextBox CopyTextBox(RichTextBox orig)
        {
            RichTextBox res = new RichTextBox();
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
            res.ContextMenuStrip = orig.ContextMenuStrip;
            res.DetectUrls = true;
            res.LinkClicked += OutputWindow.outputTextBox_LinkClicked;
            return res;
        }

        private void RestoreWatch()
        {
            DebugWatchListWindow.RemoveAll();
            foreach (string s in watches)
            {
                AddVariable(s, false);
            }
            watches.Clear();
        }

        private void RestoreDesktop()
        {
            AddOutputWindow();
            AddErrorsListWindow();
            AddCompilerConsoleWindow();
            AddFindSymbolsResultWindow();
            AddImmediateWindow();
            AddDisassemblyWindow();
            AddDebugVariablesListWindow();
            AddDebugWatchListWindow();
            if (!Tools.IsUnix())
            {
                AddProjectExplorerWindow();
                AddPropertiesWindow();
            }
            SelectContent(OutputWindow, false);
            if (!Tools.IsUnix())
                SelectContent(ProjectExplorerWindow, false);

            HideContent(ImmediateWindow);
            HideContent(DebugVariablesListWindow);
            HideContent(DebugWatchListWindow);
            HideContent(FindSymbolsResultWindow);
            HideContent(DisassemblyWindow);
            if (!Tools.IsUnix())
            {
                HideContent(PropertiesWindow);
                HideContent(ProjectExplorerWindow);
            }
        }

        bool VisualPascalABCPlugins.IWorkbenchDocumentService.ContainsTab(VisualPascalABCPlugins.ICodeFileDocument tab)
        {
            return TabStack.Contains(tab as CodeFileDocumentControl);
        }

        internal void HideContent(DockContent dc)
        {
            if (!dc.IsDisposed)
                dc.Hide();
        }

        internal void ShowContent(DockContent dc, bool activate)
        {
            if (activate || dc.Pane == null)
                dc.Show();
            else
            {
                IDockContent a = dc.Pane.ActiveContent;
                dc.Show();
                if (a != null)
                    dc.Pane.ActiveContent = a;
            }
        }

        internal void SelectContent(DockContent dc, bool Activate)
        {
            dc.Activate();
        }

        public void ChangedSelectedTab()
        {
            if (HealthLabel.Text != "")
                HealthLabel.Text = "";
            if (BrowserTabSelected)
            {
                BottomTabsVisible = true;
                BrowserTabSelected = false;
                CurrentWebBrowserControl = null;
                SetFocusToEditor();
            }
            if (BakSelectedTab == CurrentCodeFileDocument)
                return;
            LastSelectedTab = BakSelectedTab;
            BakSelectedTab = CurrentCodeFileDocument;

            if (LastSelectedTab != null && LastSelectedTab.DesignerAndCodeTabs != null)
            {
                //LastSelectedTab.Designer.host.HideCombo();
            }

            if (CurrentCodeFileDocument != null && CurrentCodeFileDocument.DesignerAndCodeTabs != null)
            {
                // CurrentCodeFileDocument.Designer.host.ShowCombo();
            }

            if (OutputTextBoxs[CurrentCodeFileDocument] != OutputWindow.outputTextBox)
            {
                OutputTextBoxs[CurrentCodeFileDocument].Visible = true;
                OutputWindow.outputTextBox.Visible = false;
                OutputWindow.outputTextBox = OutputTextBoxs[CurrentCodeFileDocument];
            }

            if (Languages.Facade.LanguageProvider.Instance.HasLanguageForExtension(CurrentCodeFileDocument.FileName))
            {
                CodeCompletion.CodeCompletionController.SetLanguage(CurrentCodeFileDocument.FileName);
            }
            
            SetFocusToEditor();
            bool run = WorkbenchServiceFactory.RunService.IsRun(CurrentEXEFileName);
            WorkbenchServiceFactory.DebuggerManager.SetAsPossibleDebugPage(CurrentCodeFileDocument);
            bool debug = WorkbenchServiceFactory.DebuggerManager.IsRunAndInThisTabPage(CurrentEXEFileName, CurrentCodeFileDocument);
            if (!debug)
            {
                SetStopEnabled(run);
            }
            else SetStopEnabled(debug);
            //if (debug) SetStopEnabled(debug);
            if (VisualEnvironmentCompiler != null)
                if (!debug)
                {
                    SetCompilingButtonsEnabled(!run && VisualEnvironmentCompiler.compilerLoaded);
                    SetDebugButtonsEnabled(!run && VisualEnvironmentCompiler.compilerLoaded);
                }
                else
                {
                    SetCompilingButtonsEnabled(!debug);
                    SetDebugButtonsAsByDebug();
                }
            else
                SetCompilingButtonsEnabled(false);
            SaveButtonsEnabled = CurrentCodeFileDocument.DocumentChanged;
            UpdateUndoRedoEnabled();
            UpdateCutCopyButtonsEnabled();
            UpdateLineColPosition();
            WorkbenchServiceFactory.RunService.UpdateReadRequest(true);
            UpdateDesignerIsActive();
        }

        public void OpenTabWithText(string tabName, string Text)
        {
            CodeFileDocumentControl edit = FindTab(tabName + ".pas");
            if (edit != null)
            {
                CurrentCodeFileDocument = edit;
                return;
            }
            edit = AddNewTab(MainDockPanel);
            edit.FromMetadata = true;
            OpenDocuments.Add(Tools.FileNameToLower(tabName + ".pas"), edit);
            CloseButtonsEnabled = OpenDocuments.Count > 1;
            edit.FileName = tabName + ".pas";
            edit.Text = tabName;
            edit.SetHighlightingStrategyForFile("prog.pas");
            edit.TextEditor.Document.Insert(0, Text);
            if (!WorkbenchServiceFactory.DebuggerManager.IsRunning)
            {
                AddEditorHandlers(edit);
            }
            WorkbenchServiceFactory.CodeCompletionParserController.RegisterFileForParsing(tabName + ".pas");
            //edit.file_name = file_name;
            //SetTabPageText(edit);
        }

        public ICodeFileDocument GetTabPageForMainFile()
        {
            return OpenFileAndNoSelection(ProjectFactory.Instance.CurrentProject.MainFile);
        }

        public void SetTabPageText(ICodeFileDocument tp)
        {
            //(ssyy) TODO: ðàçîáðàòüñÿ ñ ýòèì.
            tp.Text = Path.GetFileName(tp.FileName);
            tp.ToolTipText = tp.FileName;
            if (tp.DocumentChanged && !tp.FromMetadata)
                tp.Text += "*";
            if (tp.FromMetadata)
                tp.Text += string.Format(" [{0}]", PascalABCCompiler.StringResources.Get("VP_MF_FROM_METADATA"));
            if (!ProjectFactory.Instance.ProjectLoaded && !tp.FromMetadata)
            {
                if (tp.Run && (!WorkbenchServiceFactory.DebuggerManager.IsRun(tp.EXEFileName) || !WorkbenchServiceFactory.DebuggerManager.ShowDebugTabs))
                    tp.Text += string.Format(" [{0}]", PascalABCCompiler.StringResources.Get("VP_MF_TS_RUN"));
                else
                    if (WorkbenchServiceFactory.DebuggerManager.IsRun(tp.EXEFileName, tp.FileName))
                    tp.Text += string.Format(" [{0}]", PascalABCCompiler.StringResources.Get("VP_MF_MR_DEBUG"));
            }
            if (tp == ActiveCodeFileDocument && !tp.FromMetadata)
                tp.Text = Convert.ToChar(0x25CF) + tp.Text;//25CF //2022
        }

        void AddTextToOutputWindow(string ExeFileName, string Text)
        {
            //if ((DateTime.Now - dt).TotalSeconds >= 1)
            //  Application.DoEvents()
            ExeFileName = Tools.FileNameToLower(ExeFileName);
            if (WorkbenchStorage.SetCurrentTabPageIfWriteToOutputWindow)
            {
                WorkbenchStorage.SetCurrentTabPageIfWriteToOutputWindow = false;
                CurrentCodeFileDocument = WorkbenchServiceFactory.RunService.GetRunTab(ExeFileName) as CodeFileDocumentControl;
            }
            BottomTabsVisible = true;
            if (BottomActiveContent != OutputWindow)
            {
                OutputWindow.Activate();
                CurrentCodeFileDocument.Activate();
                OutputWindow.Activate();
            }
            TextBoxBase textBox = OutputTextBoxs[WorkbenchServiceFactory.RunService.GetRunTab(ExeFileName) as CodeFileDocumentControl];
            textBox.BeginInvoke(new AppendTextInternalAsyncDelegate(AppendTextInternalAsync), textBox, Text);
            if (textBox == OutputWindow.outputTextBox)
                OutputWindow.OutputTextBoxScrolToEnd();
            if (WorkbenchServiceFactory.DebuggerManager.IsRun(ExeFileName))
            {
                for (int i = 0; i < TabStack.Count; i++)
                {
                    TextBoxBase tb = OutputTextBoxs[TabStack[i]];
                    if (textBox != tb)
                        tb.BeginInvoke(new AppendTextInternalAsyncDelegate(AppendTextInternalAsync), tb, Text);
                }
            }
            if (!WorkbenchServiceFactory.RunService.IsRun(ExeFileName))
                SetFocusToEditor();
        }

        delegate void AppendTextInternalAsyncDelegate(TextBoxBase textBox, string Text);

        private System.Drawing.Color ConvertCodeToColor(char c)
        {
            switch ((UInt32)c)
            {
                case 65535: return System.Drawing.Color.Green;
                case 65534: return System.Drawing.Color.Red;
                case 65533: return System.Drawing.Color.OrangeRed;
                case 65532: return System.Drawing.Color.Magenta;
                case 65531: return System.Drawing.Color.Gray;
                default: return System.Drawing.Color.Black;
            }
        }

        private const int lastColorCharCode = 65531;
        private void AppendTextInternalAsync(TextBoxBase textBox, string Text)
        {
            var IsColored = Text.Any(c => (UInt32)c >= lastColorCharCode);
            if (textBox is RichTextBox rtextBox && IsColored)
            {
                var lst = Text.ToList();

                var ind0 = 0;
                var ind = lst.FindIndex(c => (UInt32)c >= lastColorCharCode);
                while (ind != -1)
                {
                    var ss = Text.Substring(ind0, ind - ind0);
                    if (ss.Length > 0)
                        rtextBox.AppendText(ss);

                    rtextBox.SelectionColor = ConvertCodeToColor(lst[ind]);
                    ind0 = ind + 1;
                    if (ind0 == lst.Count)
                        return;
                    ind = lst.FindIndex(ind0, c => (UInt32)c >= lastColorCharCode);
                } 
                var ss1 = Text.Substring(ind0);
                if (ss1.Length > 0)
                    rtextBox.AppendText(ss1);
            }
            else
                textBox.AppendText(Text);
        }

        delegate void InternalWriteToOutputBoxDel(string exc);

        private void InternalWriteToOutputBox(string exc)
        {
            OutputWindow.outputTextBox.AppendText(exc);
        }

        public void WriteToOutputBox(string excep, bool is_exc)
        {
            if (is_exc)
                SelectContent(OutputWindow, false);
            OutputWindow.outputTextBox.Invoke(new InternalWriteToOutputBoxDel(InternalWriteToOutputBox), excep);//AppendText(excep);
        }

        public void AddTextToOutputWindowSync(string fileName, string text)
        {
            //BeginInvoke(new SetFileNameAndTextDelegate(AddTextToOutputWindow), file_name, text);
            Invoke(new SetFileNameAndTextDelegate(AddTextToOutputWindow), fileName, text);
        }

        private void AppendTextToConsoleCompiler(object text)
        {
            CompilerConsoleWindow.AppendTextToConsoleCompiler(text);
        }

        public void AddTextToCompilerMessages(string text)
        {
            CompilerConsoleWindow.AddTextToCompilerMessages(text);
        }

        public void AddTextToCompilerMessagesSync(string text)
        {
            BeginInvoke(new SetTextDelegate(AddTextToCompilerMessages), text);
        }

        public void ClearErrorList()
        {
            ErrorsListWindow.lvErrorsList.Items.Clear();
            //lvErrorsList.Update();
            WorkbenchServiceFactory.BuildService.ErrorsList.Clear();
        }

        void CheckErrorListAndClear(string FileName)
        {
            if (WorkbenchServiceFactory.BuildService.ErrorsList.Count > 0)
            {
                PascalABCCompiler.Errors.LocatedError er = WorkbenchServiceFactory.BuildService.ErrorsList[0] as PascalABCCompiler.Errors.LocatedError;
                if (er != null && er.SourceLocation != null && er.SourceLocation.FileName.ToLower() == FileName.ToLower())
                    ClearErrorList();

            }
        }

        public void ClearOutputTextBoxForTabPage(ICodeFileDocument tabPage)
        {
            ClearTextBox(OutputTextBoxs[tabPage]);
        }

        public void SendNewLineToInputTextBox()
        {
            if (WorkbenchServiceFactory.DebuggerManager.IsRun(CurrentEXEFileName) && OutputBoxStack.Contains(OutputWindow.outputTextBox))
            {

                foreach (TextBoxBase tb in OutputBoxStack)
                    if (tb != OutputWindow.outputTextBox)
                    {
                        tb.AppendText(Environment.NewLine);
                    }
            }
        }

        private void ClearAndSaveWatch()
        {
            watches.AddRange(DebugWatchListWindow.GetExpressions());
            DebugWatchListWindow.RemoveAll();
        }

        private void ClearTextBox(RichTextBox rtb)
        {
            var zoomf = rtb.ZoomFactor;
            rtb.Clear();
            rtb.ZoomFactor = zoomf;
        }

        public void ClearTabStack()
        {
            foreach (CodeFileDocumentControl cfdc in TabStack)
            {
                if (cfdc != debuggedPage)
                {
                    if (OutputTextBoxs.ContainsKey(cfdc))
                    {
                        ClearTextBox(OutputTextBoxs[cfdc]);
                    }
                }
            }
            OutputBoxStack.Clear();
            TabStack.Clear();
        }

        public void ClearWatch()
        {
            for (int i = 0; i < this.WdataGridView1.Rows.Count; i++)
            {
                DebugWatchListWindow.SetUndefinedValue(i);
            }
        }

        public void DisplayDisassembledCode(string code)
        {
            DisassemblyWindow.SetDisassembledCode(code);
        }

        public void ClearDebugTabs()
        {
            foreach (TextArea ta in DebugTabs.Values)
            {
                ta.ToolTipRequest -= WorkbenchServiceFactory.DebuggerManager.TextAreaToolTipRequest;
            }
            DebugTabs.Clear();
        }

        public void ClearLocalVarTree()
        {
            DebugVariablesListWindow.ClearAllSubTrees();
        }

        public void RefreshPad(IList<IListItem> items)
        {
            try
            {
                DebugWatchListWindow.RefreshWatch();
                AdvancedDataGridView.TreeGridNode.UpdateNodesForLocalList(DebugVariablesListWindow.watchList, DebugVariablesListWindow.watchList.Nodes, items);
            }
            catch (System.Exception)
            {

            }
        }

        public void GotoWatch()
        {
            BottomTabsVisible = true;
            SelectContent(DebugWatchListWindow, true);
            DebugWatchListWindow.AddNewEntry("");
        }
    }
}
