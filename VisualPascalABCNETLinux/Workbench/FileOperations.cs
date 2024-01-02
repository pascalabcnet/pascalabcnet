// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
//using ICSharpCode.FormsDesigner;
using VisualPascalABCPlugins;
using WeifenLuo.WinFormsUI.Docking;

namespace VisualPascalABC
{
    public partial class Form1 : IWorkbenchFileService
    {
        List<string> temp_open_files = new List<string>();
        public List<string> LastOpenFiles;
        private VisualStates visualStates = new VisualStates();
        internal Dictionary<string, CodeFileDocumentControl> OpenDocuments = new Dictionary<string, CodeFileDocumentControl>();
        internal Dictionary<string, WebBrowserControl> OpenBrowserDocuments = new Dictionary<string, WebBrowserControl>();
        private int MaxLastOpenFiles = 10;
        private int MaxCharsInLastOpenFileName = 80;

        public CodeFileDocumentControl OpenFileAndNoSelection(string FileName)
        {
            CodeFileDocumentControl tp = FindTab(FileName);
            if (tp != null)
                return tp;

            MainDockPanel.Size = new Size(1920, 1080);
            tp = AddNewProgramToTab(MainDockPanel, FileName);
            MPanel_Resize(null, EventArgs.Empty);

            AddLastFile(FileName);
            tp.LoadFromFile(FileName);
            tp.DocumentSavedToDisk = true;
            ShowAddedBreakpoints(tp);
            return tp;
        }

        public bool OpenFile(string FileName, string PreferedFileName, bool not_open_designer=false)
        {
            CodeFileDocumentControl tp = FindTab(FileName);
            bool IsNewFile = FileName == null;
            if (tp == null)
            {
                if (!IsNewFile && !File.Exists(FileName))
                {
                    MessageBox.Show(string.Format(PascalABCCompiler.StringResources.Get("!FILE_NOT_FOUND{0}"), FileName), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                visualStates.FileOpened = true;//?????
                if (!IsNewFile)
                    openFileDialog1.InitialDirectory = Path.GetDirectoryName(FileName);
                if (!IsBlankNewProgram(CurrentCodeFileDocument))
                {
                    if (IsNewFile)
                        if (PreferedFileName == null)
                            FileName = InstNameNewProgramm(MainDockPanel);
                        else
                            FileName = PreferedFileName;
                    MainDockPanel.Size = new Size(1920, 1080);
                    tp = AddNewProgramToTab(MainDockPanel, FileName);
                    MPanel_Resize(null, EventArgs.Empty);

                }
                else
                    if (!IsNewFile)
                    {
                        CheckErrorListAndClear(CurrentCodeFileDocument.FileName);
                        WorkbenchServiceFactory.CodeCompletionParserController.CloseFile(CurrentCodeFileDocument.FileName);
                        OpenDocuments.Remove(Tools.FileNameToLower(CurrentCodeFileDocument.FileName));
                        CurrentCodeFileDocument.FileName = FileName;
                        SetTabPageText(CurrentCodeFileDocument);
                        if (!WorkbenchServiceFactory.RunService.HasRunArgument(FileName.ToLower()))
                            WorkbenchServiceFactory.RunService.AddRunArgument(FileName.ToLower(), "");
                        tp = CurrentCodeFileDocument;
                    }
                if (!IsNewFile)
                {
                    AddLastFile(FileName);
                    WatcherService.AddWatcher(FileName);
                    int ticks = Environment.TickCount;
                    tp.LoadFromFile(FileName);
                    //MessageBox.Show((Environment.TickCount-ticks).ToString());
                    
                    tp.DocumentSavedToDisk = true;
                    //ivan added
                    ShowAddedBreakpoints(tp);
                    //ShowAddedBreakpoints(tabControl1.TabPages[TabIndex]);
                }
                //CloseButtonsEnabled = OpenDocuments.Count > 1;
                ChangedSelectedTab();
                if (FileName != null) // SS 09.08.08
                    WorkbenchServiceFactory.CodeCompletionParserController.RegisterFileForParsing(FileName);

            }
            if (tp != null)
            {
                if (MainDockPanel.ActiveDocument != tp)
                {
                    CurrentCodeFileDocument = tp;
                }
            }
            if (FileName != null)
            {
                string filename = Tools.FileNameToLower(FileName);
                if (OpenDocuments.ContainsKey(filename))
                    OpenDocuments[filename] = tp;
                else
                    OpenDocuments.Add(filename, tp);
                CloseButtonsEnabled = OpenDocuments.Count > 1;//Ivan 09.08.08 err 195 
            }
            visualStates.FileOpened = false;

            //ssyy
            if (!IsNewFile)
            {
                if (Path.GetExtension(FileName) == ".pas")
                {
                    /*string XMLFile = Path.ChangeExtension(file_name, string_consts.xml_form_extention);
                    if (File.Exists(XMLFile))
                    {
                        if (not_open_designer)
                        {
                            if (VisualPascalABC.VisualPABCSingleton.MainForm._currentCodeFileDocument.DesignerAndCodeTabs != null)
                            {
                                //VisualPascalABC.Form1.Form1_object.CahngedSelectedTab();
                                VisualPascalABC.VisualPABCSingleton.MainForm._currentCodeFileDocument.DesignerAndCodeTabs.SelectedTab =
                                           VisualPascalABC.VisualPABCSingleton.MainForm._currentCodeFileDocument.TextPage;

                            }
                        }
                        else
                        _currentCodeFileDocument.AddDesigner(XMLFile);
                    }*/
                }
            }

            return true;
        }

        public void OpenFileWithForm()
        {
            OpenFile(null, null);
            CurrentCodeFileDocument.AddDesigner(null);
        }

        public delegate CodeFileDocumentControl OpenFileForDebugDelegate(string FileName);

        public ICodeFileDocument OpenFileForDebug(string FileName)
        {
            return this.Invoke(new OpenFileForDebugDelegate(OpenFileForDebugInvoke), FileName) as ICodeFileDocument;
        }

        public CodeFileDocumentControl OpenFileForDebugInvoke(string FileName)
        {
            CodeFileDocumentControl tp = FindTab(FileName);
            bool IsNewFile = FileName == null;
            if (tp == null)
            {
                visualStates.FileOpened = true;//?????
                if (!IsNewFile && !File.Exists(FileName))
                {
                    //MessageBox.Show(string.Format(PascalABCCompiler.StringResources.Get("!FILE_NOT_FOUND{0}"), file_name), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    bool exists = false;
                    foreach (string s in search_debug_paths)
                    {
                        FileName = String.Concat(s, Path.DirectorySeparatorChar, Path.GetFileName(FileName));
                        if (File.Exists(FileName))
                        {
                            tp = FindTab(FileName);
                            if (tp != null)
                            {
                                CurrentCodeFileDocument = tp;
                                //(CurrentTabPage.ag as CodeFileDocumentControl).DocumentSavedToDisk = !IsNewFile;
                                visualStates.FileOpened = false;
                                AddDebugPage(CurrentCodeFileDocument);
                                return CurrentCodeFileDocument;
                            }
                            else
                            {
                                exists = true;
                                break;
                            }
                        }
                    }
                    if (!exists)
                    {
                        visualStates.FileOpened = false;
                        return null;
                    }
                }
                openFileDialog1.InitialDirectory = Path.GetDirectoryName(FileName);
                if (!IsBlankNewProgram(CurrentCodeFileDocument))
                {
                    if (IsNewFile)
                        FileName = InstNameNewProgramm(MainDockPanel);
                    MainDockPanel.Size = new Size(1920, 1080);
                    tp = AddNewProgramToTab(MainDockPanel, FileName);
                    MPanel_Resize(null, EventArgs.Empty);
                }
                else
                    if (!IsNewFile)
                    {
                        //CheckErrorListAndClear((tabControl1.TabPages[TabIndex].ag as CodeFileDocumentControl).file_name);
                        CurrentCodeFileDocument.FileName = FileName;
                        if (!ProjectFactory.Instance.ProjectLoaded)
                            SetTabPageText(CurrentCodeFileDocument);
                        tp = CurrentCodeFileDocument;
                    }
                if (!IsNewFile)
                {
                    AddLastFile(FileName);
                    WatcherService.AddWatcher(FileName);
                    tp.LoadFromFile(FileName);
                    tp.DocumentSavedToDisk = true;
                    //ivan added
                    ShowAddedBreakpoints(tp);
                    //ShowAddedBreakpoints(tabControl1.TabPages[TabIndex]);
                }
                CloseButtonsEnabled = OpenDocuments.Count > 1;
            }
            if (tp != null)
            {
                CurrentCodeFileDocument = tp;
                if (tp.DesignerAndCodeTabs != null)
                    tp.DesignerAndCodeTabs.SelectedTab = tp.TextPage;
                //(CurrentTabPage.ag as CodeFileDocumentControl).DocumentSavedToDisk = !IsNewFile;
            }
            visualStates.FileOpened = false;
            AddDebugPage(CurrentCodeFileDocument);
            //CahngedSelectedTab();
            return CurrentCodeFileDocument;
        }

        ICodeFileDocument VisualPascalABCPlugins.IWorkbenchFileService.OpenFileForDebug(string FileName)
        {
            return OpenFileForDebug(FileName);
        }

        public void RenameFile(string OldFileName, string NewFileName)
        {
            CodeFileDocumentControl cfdc = FindTab(OldFileName);
            if (cfdc == null)
            {
                try
                {
                    File.Copy(OldFileName, NewFileName);
                    File.Delete(OldFileName);
                }
                catch
                {
                }
                return;
            }
            SaveFileAs(cfdc, NewFileName);
            try
            {
                File.Delete(OldFileName);
            }
            catch
            {
            }
        }

        public void CloseFile(CodeFileDocumentControl tp, bool ask_for_save = true)
        {
            string FileName = tp.FileName.ToLower();
            string exeName = tp.EXEFileName;
            if (WorkbenchServiceFactory.RunService.IsRun(exeName))
                WorkbenchServiceFactory.RunService.Stop(exeName);
            if (ask_for_save)
                if (tp.DocumentChanged && !tp.FromMetadata)
                    if (!QuestionAndSaveFile(tp))
                    {
                        SaveCanceled = true;
                        return;
                    }
            if (tp.DocumentSavedToDisk)
                AddLastFile(CurrentSourceFileName);
            if (LastSelectedTab != null && !LastSelectedTab.IsDisposed)
                CurrentCodeFileDocument = LastSelectedTab;
            if (tp == ActiveCodeFileDocument)
                ActiveCodeFileDocument = null;
            OutputTextBoxs.Remove(tp);
            OpenDocuments.Remove(Tools.FileNameToLower(tp.FileName));
            //RunArgumentsTable.Remove(tp);
            tp.Dispose();
            CheckErrorListAndClear(FileName);
            WatcherService.RemoveWatcher(FileName);
            if (OpenDocuments.Count == 1)
            {
                CloseButtonsEnabled = false;
            }
            SaveAllButtonsEnabled = !AllSaved();
            WorkbenchServiceFactory.CodeCompletionParserController.CloseFile(FileName);
            WorkbenchServiceFactory.CodeCompletionParserController.ParseInformationUpdated -= tp.TextEditor.UpdateFolding;
            CodeCompletionKeyHandler.Detach(tp.TextEditor);
            tp.TextEditor.Document.DocumentChanged -= tp.Document_DocumentChanged;
            tp.TextEditor.Document.TextContent = "";
            tp.TextEditor.Document.DocumentChanged += tp.Document_DocumentChanged;
            GC.Collect();
        }

        public void CloseFile(string FileName)
        {
            CodeFileDocumentControl tp = FindTab(FileName);
            if (tp != null)
            {
                CloseFile(tp);
                if (OpenDocuments.Count == 0)
                {
                    OpenFile(null, null);
                }
            }
        }

        private void CloseFilesAndSaveState()
        {
            foreach (CodeFileDocumentControl cfdc in this.OpenDocuments.Values)
            {
                temp_open_files.Add(cfdc.FileName);
            }

            CloseAllButThis(null);
        }

        public void ReloadFile(string FileName)
        {
            CodeFileDocumentControl tab = FindTab(FileName);
            if (tab != null)
            {
                tab.LoadFromFile(FileName);
                tab.DocumentChanged = false;
                SetTabPageText(tab);
            }
        }

        private void RestoreFiles()
        {
            foreach (string s in temp_open_files)
            {
                if (File.Exists(s))
                    OpenFile(s, null);
                else
                    OpenFile(null, null);
            }
            temp_open_files.Clear();
        }

        public void SetFileAsChanged(string FileName)
        {
            CodeFileDocumentControl tab = FindTab(FileName);
            if (tab != null)
            {
                tab.DocumentChanged = true;
                SetTabPageText(tab);
            }
        }

        private void SaveSelFile(CodeFileDocumentControl TbPage)
        {
            SaveSelFileAs(TbPage, TbPage.FileName);
        }

        private void SaveSelFileAs(CodeFileDocumentControl TbPage, string FileName)
        {
            if (!TbPage.DocumentSavedToDisk)
                ExecuteSaveAs(TbPage);
            else
                SaveFileAs(TbPage, FileName);
        }

        internal void SaveFileAs(CodeFileDocumentControl TbPage, string FileName)
        {
            try
            {
                /*if (TbPage.DesignerAndCodeTabs != null)
                {
                    TbPage.GenerateDesignerCode(null);
                }*/
                CodeFileDocumentControl dt = TbPage;
                WatcherService.DisableWatcher(FileName);
                //dt.TextEditor.Encoding = VisualEnvironmentCompiler.DefaultFileEncoding;
                if (!dt.TextEditor.CanSaveWithCurrentEncoding() || true)
                {
                    dt.TextEditor.Encoding = Encoding.UTF8;
                    dt.TextEditor.SaveFile(FileName);
                    dt.TextEditor.Encoding = VisualEnvironmentCompiler.DefaultFileEncoding;
                }
                else
                    dt.TextEditor.SaveFile(FileName);
                WatcherService.EnableWatcher(FileName);
                OpenDocuments.Remove(Tools.FileNameToLower(dt.FileName));
                OpenDocuments.Add(Tools.FileNameToLower(FileName), TbPage);
                WorkbenchServiceFactory.CodeCompletionParserController.RenameFile(dt.FileName, FileName);
                //TbPage.SaveFormFile(file_name);
                dt.DocumentChanged = false;
                dt.FileName = FileName;
                dt.DocumentSavedToDisk = true;
                dt.SetHighlightingStrategyForFile(FileName);
                SetTabPageText(TbPage);
                if (!WorkbenchServiceFactory.RunService.HasRunArgument(FileName.ToLower()))
                    WorkbenchServiceFactory.RunService.AddRunArgument(FileName.ToLower(), "");
                if (TbPage == CurrentCodeFileDocument)
                    UpdateSaveButtonsEnabled();
            }
            catch (Exception)
            {
                MessageBox.Show(String.Format(Form1StringResources.Get("SAVE_FILE_ERROR_TEXT{0}"), FileName), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public bool SaveAll(bool Question)
        {
            if (ProjectFactory.Instance.ProjectLoaded && Question)
            {
                return SaveAllInProject() != 2;
            }
            SaveCanceled = false;
            CodeFileDocumentControl[] l = new CodeFileDocumentControl[OpenDocuments.Values.Count];
            OpenDocuments.Values.CopyTo(l, 0);
            foreach (CodeFileDocumentControl tp in l)
                if (tp.DocumentChanged && !tp.FromMetadata)
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
            UpdateSaveButtonsEnabled();
            return true;
        }

        void IWorkbenchFileService.CloseAllButThis(ICodeFileDocument nonCloseTab)
        {
            this.CloseAllButThis(nonCloseTab as CodeFileDocumentControl);
        }

        void CloseAllButThis(CodeFileDocumentControl nonCloseTab)
        {
            SaveCanceled = false;
            CodeFileDocumentControl bakTab = CurrentCodeFileDocument;
            /*foreach (TabPage tbPage in tabControl1.TabPages)
                if (tbPage != tabControl1.SelectedTab)
                    CloseFile(tbPage);*/
            List<CodeFileDocumentControl> to_close = new List<CodeFileDocumentControl>();
            foreach (CodeFileDocumentControl tbPage in OpenDocuments.Values)
            {
                if (SaveCanceled) break;
                if (tbPage != nonCloseTab)
                {
                    if (tbPage == bakTab)
                        bakTab = null;
                    to_close.Add(tbPage);
                }
            }
            if (!SaveCanceled)
                foreach (CodeFileDocumentControl tbPage in to_close)
                {
                    tbPage.Close();
                }
            if (bakTab != null)
                if (bakTab != CurrentCodeFileDocument)
                    CurrentCodeFileDocument = bakTab;
            List<WebBrowserControl> todelete = new List<WebBrowserControl>();
            foreach (WebBrowserControl browserTab in OpenBrowserDocuments.Values)
                todelete.Add(browserTab);
            foreach (WebBrowserControl browserTab in todelete)
                browserTab.Close();
            //OpenBrowserDocuments.Clear();
        }

        bool QuestionAndSaveFile(CodeFileDocumentControl tp)
        {
            CurrentCodeFileDocument = tp;
            DialogResult result = MessageBox.Show(string.Format(Form1StringResources.Get("SAVE_CHANGES_IN_FILE{0}"), Path.GetFileName(tp.FileName)), PascalABCCompiler.StringResources.Get("!CONFIRM"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                SaveSelFile(CurrentCodeFileDocument);
            else
                if (result == DialogResult.Cancel)
                    return false;
            return true;
        }

        private void ExecuteSaveAs(CodeFileDocumentControl TbPage)
        {
            //if (TbPage.DesignerAndCodeTabs != null)
            //{
            //    TbPage.GenerateDesignerCode();
            //}
            CodeFileDocumentControl bakTab = CurrentCodeFileDocument;
            CurrentCodeFileDocument = TbPage;
            var s = Path.GetFileName(CurrentSourceFileName);
            if (s.ToLower().StartsWith("program"))
                s = "";
            saveFileDialog1.FileName = s;
            string id = Path.GetDirectoryName(CurrentSourceFileName);
            if (id == "")
            {
                if (WorkbenchStorage.WorkingDirectoryExsist)
                    saveFileDialog1.InitialDirectory = WorkbenchStorage.WorkingDirectory;
            }
            else
                saveFileDialog1.InitialDirectory = Path.GetDirectoryName(CurrentSourceFileName);
            DialogResult dr = saveFileDialog1.ShowDialog();
            CurrentCodeFileDocument = bakTab;

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
            LastOpenFiles.Insert(0, FileName);
            mI = new ToolStripMenuItem();
            mI.Text = GetLastFileCaption(FileName);
            mI.Tag = FileName;
            miRecentFiles.DropDownItems.Insert(0, mI);
            mI.Click += new System.EventHandler(this.OpenLastFile_ToolStripMenuItem_Click);
            // mI.ToolTipText = mI.Text;
        }

        private string InstNameNewProgramm(DockPanel tabControl)
        {
            int NumbNewProg = 1;
            string FileNameFormat = Path.Combine(WorkbenchStorage.WorkingDirectory, UserOptions.DefaultSourceFileNameFormat), FileName;
            while (true)
            {
                FileName = string.Format(FileNameFormat, NumbNewProg);
                if (FindTab(FileName) == null && !File.Exists(FileName))
                    return FileName;
                NumbNewProg++;
            }
        }

        bool IsBlankNewProgram(CodeFileDocumentControl tp)
        {
            return (!tp.DocumentSavedToDisk && !tp.DocumentChanged);
        }

        internal bool IsForm(string FileName)
        {
            //string XMLFile = Path.ChangeExtension(file_name, string_consts.xml_form_extention);
            // return File.Exists(XMLFile);
            return false;
        }

        internal bool AllSaved()
        {
            foreach (CodeFileDocumentControl tp in OpenDocuments.Values)
                if (tp.DocumentChanged) return false;
            return true;
        }

        void IWorkbenchFileService.PrintActiveDocument()
        {
            ExecPrint();
        }

        public void ExecPrint()
        {
            printDialog1.AllowSelection = true;
            printDialog1.Document = this.CurrentCodeFileDocument.TextEditor.PrintDocument;
            if (printDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                printDialog1.Document.Print();
            }
        }
    }
}
