// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VisualPascalABCPlugins;
using WeifenLuo.WinFormsUI.Docking;

namespace VisualPascalABC
{
    public partial class Form1: VisualPascalABCPlugins.IWidgetStatusContoller
    {
        IWidgetStatusContoller VisualPascalABCPlugins.IWorkbench.WidgetController
        {
            get
            {
                return this;
            }
        }

        internal List<DockContent> BottomDockContent = new List<DockContent>();
        List<DockContent> VisibleBottomContent = new List<DockContent>();

        public bool CompilingButtonsEnabled
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

        internal bool SaveAllButtonsEnabled
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

        public void SetDebugButtonsEnabled(bool Enabled)
        {
            if (!Enabled || CodeCompletion.CodeCompletionController.IntellisenseAvailable())
            {
                StepIntoButton.Enabled = StepOverButton.Enabled = StartDebugButton.Enabled =
                mDEBUGSTARTToolStripMenuItem.Enabled = mSTEPOVERToolStripMenuItem.Enabled =
                mSTEPINToolStripMenuItem.Enabled = mRUNTOCURToolStripMenuItem.Enabled = Enabled;
            }
            //toolStrip1.Refresh();
        }



        bool BottomDockContentVisible
        {
            get
            {
                foreach (DockContent dc in BottomDockContent)
                    if (!dc.IsHidden)
                        return true;
                return false;
            }
        }

        internal bool BottomTabsVisibleChekerEnabled
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

        internal bool BrowserTabSelected;

        internal bool BottomTabsVisible
        {
            get
            {
                //return BottomDockContentVisible;
                return BPanel.Visible;
            }
            set
            {
                if (value == BottomTabsVisible)
                    return;
                SetVisibilityBottomPanel(value);
                /*if (value)
                {
                    if (VisibleBottomContent.Count > 0)
                    {
                        foreach (DockContent dc in VisibleBottomContent)
                        {
                            dc.Show();
                        }
                            
                        VisibleBottomContent.Clear();
                    }
                    else
                        if (BottomDockContent.Count > 0)
                            BottomDockContent[0].Show();
                }
                else
                {
                    VisibleBottomContent.Clear();
                    DockContent dca = BottomDockContent[0].Pane.ActiveContent as DockContent;
                    foreach (DockContent dc in BottomDockContent)
                        if (!dc.IsHidden && !BottomPane.IsAutoHide)
                        {
                            if (dc != dca)
                                VisibleBottomContent.Add(dc);
                            dc.Hide();
                        }
                    VisibleBottomContent.Add(dca);
                }*/

                UpdateOutputWindowVisibleButtons();
                if (!value && LoadComplete)
                    SetFocusToEditor();
            }
        }

        private bool ToolBoxVisible_ = false;

        public bool ToolBoxVisible
        {
            get
            {
                return ToolBoxVisible_;
            }
            set
            {
                /*if (value && ToolBoxVisible_ && !ToolBoxWindow.IsActivated)
                {
                    ToolBoxWindow.Show();
                    ToolBoxWindow.Activate();
                    return;
                }*/
                if (ToolBoxVisible_ == value) return;
                ToolBoxVisible_ = value;
                /*if (value)
                {
                    if (ToolBoxWindow == null)
                    {
                        AddToolBox();
                    }
                    else
                    {
                        ToolBoxWindow.Show();
                    }
                }
                else
                {
                    ToolBoxWindow.Hide();
                }*/
            }
        }

        public bool ProjectExplorerWindowVisible
        {
            get
            {
                return ProjectExplorerWindow != null && ProjectExplorerWindow.Visible;
            }
            set
            {
                if (value)
                {
                    if (ProjectExplorerWindow == null)
                    {
                        AddProjectExplorerWindow();
                    }
                    else
                    {
                        //ShowContent(PropertiesWindow, false);
                        ProjectExplorerWindow.Show();
                        //PropertiesWindow.IsHidden = false; //Show(MainDockPanel, DockState.Hidden);
                    }
                }
                else
                {
                    if (ProjectExplorerWindow != null)
                    {
                        //PropertiesWindow.IsHidden = true;
                    }
                }
            }

        }

        public bool DisassemblyWindowVisible
        {
            get
            {
                return DisassemblyWindow != null && DisassemblyWindow.Visible;
            }
            set
            {
                if (value)
                {
                    if (DisassemblyWindow == null)
                    {
                        AddDisassemblyWindow();
                    }
                    else
                    {
                        //ShowContent(PropertiesWindow, false);
                        DisassemblyWindow.Show();
                        DisassemblyWindow.ShowDisassembly();
                        //PropertiesWindow.IsHidden = false; //Show(MainDockPanel, DockState.Hidden);
                    }
                }
            }

        }

        public bool PropertiesWindowVisible
        {
            get
            {
                return false; // PropertiesWindow != null && PropertiesWindow.Visible;
            }
            set
            {
                if (value)
                {
                    //if (PropertiesWindow == null)
                    {
                        //AddPropertiesWindow();
                    }
                    //else
                    {
                        //ShowContent(PropertiesWindow, false);
                        //PropertiesWindow.Show();
                        //PropertiesWindow.IsHidden = false; //Show(MainDockPanel, DockState.Hidden);
                    }
                }
                //else
                {
                    //if (PropertiesWindow != null)
                    {
                        //PropertiesWindow.IsHidden = true;
                    }
                }
            }
        }

        internal bool FindAllReferncesEnabled
        {
            get
            {
                return cmFindAllReferences.Enabled;
            }
            set
            {
                cmFindAllReferences.Enabled = tsFindAllReferences.Enabled = value;
            }
        }

        public void SetCompilingButtonsEnabled(bool Enabled)
        {
            if (Enabled)
            {
                miNewProject.Enabled = true;
            }
            //StepIntoButton.Enabled = StepOverButton.Enabled = 
            //   mDEBUGSTARTToolStripMenuItem.Enabled = mSTEPOVERToolStripMenuItem.Enabled=
            //   StartDebugButton.Enabled =    
            StartNoDebugButton.Enabled = miRunNoDebug.Enabled =
            miBuild.Enabled = miRebuild.Enabled = miRun.Enabled = ReCompileButton.Enabled =
            CompileButton.Enabled = cmRun.Enabled = StartButton.Enabled = Enabled;

            toolStrip1.Refresh();
        }

        public void UpdateUndoRedoEnabled() //roman// public
        {
            CodeFileDocumentControl d = CurrentCodeFileDocument; //roman//
            if (d.DesignerAndCodeTabs != null)
            {
                //if (d.DesignerPage != null && d.Designer != null && d.DesignerAndCodeTabs.SelectedTab == d.DesignerPage)
                {
                //    UndoButtonsEnabled = d.Designer.EnableUndo;
                //    RedoButtonsEnabled = d.Designer.EnableRedo;
                }
                //else
                {
                    UndoButtonsEnabled = d.CanUndo;
                    RedoButtonsEnabled = d.CanRedo;
                }
            }
            else
            {
                UndoButtonsEnabled = d.CanUndo;
                RedoButtonsEnabled = d.CanRedo;
            }
        }

        void UpdateCutCopyButtonsEnabled()
        {
            CutButtonsEnabled = CopyButtonsEnabled = CurrentSyntaxEditor.TextSelected;
        }

        internal void UpdateOutputWindowVisibleButtons()
        {
            tsOutputWindow.Checked = miOutputWindow.Checked = BottomTabsVisible;
        }

        public void UpdateSaveButtonsEnabled()
        {
            SaveButtonsEnabled = CurrentCodeFileDocument != null && CurrentCodeFileDocument.DocumentChanged;
            SaveAllButtonsEnabled = !AllSaved();
        }

        public void SetDebugPausedEnabled()
        {
            this.mDEBUGSTOPToolStripMenuItem.Enabled = true;
            this.StopDebugButton.Enabled = true;
            SaveDebugContext();
        }

        public void SetDebugPausedDisabled()
        {
            this.mDEBUGSTOPToolStripMenuItem.Enabled = false;
            this.StopDebugButton.Enabled = false;
            SaveDebugContext();
        }

        public void SetDebugStopEnabled()
        {
            SetCompilingButtonsEnabled(false);
            this.mDEBUGSTOPToolStripMenuItem.Enabled = true;
            this.mDEBUGENDToolStripMenuItem.Enabled = true;
            this.mSTEPToolStripMenuItem.Enabled = true;
            this.StopDebugButton.Enabled = true;
            this.StepOutButton.Enabled = true;
            this.stopButton.Enabled = true;
            this.miStop.Enabled = true;
            ChangeDebugButtons(true);
            SaveDebugContext();
        }

        public void ChangeStartDebugNameOnContinue()
        {
            this.StartDebugButton.ToolTipText = Form1StringResources.Get("M_DEBUGCONT");
            this.mDEBUGSTARTToolStripMenuItem.Text = Form1StringResources.Get("M_DEBUGCONT");
        }

        public void ChangeContinueDebugNameOnStart()
        {
            this.StartDebugButton.ToolTipText = Form1StringResources.Get("M_DEBUGSTART");
            this.mDEBUGSTARTToolStripMenuItem.Text = Form1StringResources.Get("M_DEBUGSTART");
        }

        public void SetStartDebugAndRunDisabled()
        {
            this.mDEBUGSTARTToolStripMenuItem.Enabled = false;
            this.StartDebugButton.Enabled = false;
            this.mSTEPINToolStripMenuItem.Enabled = false;
            this.StepIntoButton.Enabled = false;
            this.mSTEPOVERToolStripMenuItem.Enabled = false;
            this.StepOverButton.Enabled = false;
            this.mRUNTOCURToolStripMenuItem.Enabled = false;
            this.mSTEPToolStripMenuItem.Enabled = false;
            this.miRun.Enabled = false;
            this.StartButton.Enabled = false;
            SetDebugPausedEnabled();
            ChangeDebugButtons(true);
            SaveDebugContext();
        }

        /// <summary>
        /// Активирует кнопки для Debug только в случае, если Intellisense доступен
        /// </summary>
        public void SetStartDebugAndRunEnabled()
        {
            if (CodeCompletion.CodeCompletionController.IntellisenseAvailable())
            {
                this.mDEBUGSTARTToolStripMenuItem.Enabled = true;
                this.StartDebugButton.Enabled = true;
                this.mSTEPINToolStripMenuItem.Enabled = true;
                this.StepIntoButton.Enabled = true;
                this.mSTEPOVERToolStripMenuItem.Enabled = true;
                this.StepOverButton.Enabled = true;
                this.mRUNTOCURToolStripMenuItem.Enabled = true;
                this.mSTEPToolStripMenuItem.Enabled = true;
            }

            this.miRun.Enabled = true;
            this.StartButton.Enabled = true;
            ChangeDebugButtons(false);
            SaveDebugContext();
        }

        public void SetDebugStopDisabled()
        {
            this.mDEBUGSTOPToolStripMenuItem.Enabled = false;
            this.mSTEPToolStripMenuItem.Enabled = false;
            this.StopDebugButton.Enabled = false;
            this.StepOutButton.Enabled = false;
            if (!(clicked_stop_debug_in_menu && WorkbenchServiceFactory.RunService.IsRun() && debuggedPage != ActiveCodeFileDocument))
            {
                SetCompilingButtonsEnabled(true);
                this.stopButton.Enabled = false;
                this.miStop.Enabled = false;
            }
            clicked_stop_debug_in_menu = false;
            this.mDEBUGENDToolStripMenuItem.Enabled = false;
            //Data fileInfo = debuggedPage.ag as CodeFileDocumentControl;
            //debuggedPage.Text = Path.GetFileName(fileInfo.file_name);
            //debuggedPage.ToolTipText = fileInfo.file_name;
            SetTabPageText(debuggedPage);
            ChangeDebugButtons(false);
            SaveDebugContext();
            //this.mDEBUGSTARTToolStripMenuItem.Text = PascalABCCompiler.StringResources.Get("M_DEBUGSTART");
        }

        private bool play_pause_buttons_visible = true;

        public bool PlayPauseButtonsVisibleInPanel
        {
            get
            {
                return play_pause_buttons_visible;
            }
            set
            {
                play_pause_buttons_visible = value;
                /*StopDebugButton.Visible = */
                if (DebuggerVisible)
                    StartDebugButton.Visible = value;
            }
        }

        public void SetAddExprMenuVisible(bool flag)
        {
        }

        public void SetDisassemblyMenuVisible(bool flag)
        {
            if (tsDisassembly == null)
                return;
            tsDisassembly.Visible = flag;
        }

        public void SetPlayButtonsVisible(bool start)
        {
            if (play_pause_buttons_visible)
            {
                if (start)
                {
                    this.StartDebugButton.Visible = false;
                    this.StopDebugButton.Visible = false;
                }
                else
                {
                    this.StartDebugButton.Visible = true;
                    this.StopDebugButton.Visible = false;
                }
            }
        }

        /// <summary>
        ///  Активировать/Деактивировать все кнопки, относящиеся к форматированию кода
        /// </summary>
        private void SetFormatButtonsEnabled(bool enabled)
        {
            tsFormat.Enabled = mFORMATToolStripMenuItem.Enabled = cmFormat.Enabled = enabled;
        }

        /// <summary>
        /// Активировать/Деактивировать все кнопки, относящиеся к дебагу и кнопки запуска. 
        /// ВАЖНО: если Intellisense не поддерживается, то активация кнопок дебага не произойдет
        /// </summary>
        // Здесь в том числе активируюется кнопка запуска, нужен рефакторинг EVA
        public void SetDebugAndRunButtonsEnabled(bool val)
        {
            if (!DebuggerVisible)
                return;
            if (val)
            {
                this.mDEBUGSTOPToolStripMenuItem.Enabled = false;
                this.mDEBUGSTARTToolStripMenuItem.Enabled = false;
                this.StepOutButton.Enabled = false;
                this.mSTEPToolStripMenuItem.Enabled = false;

                if (CodeCompletion.CodeCompletionController.IntellisenseAvailable())
                {
                    this.StartDebugButton.Enabled = true;
                    this.StopDebugButton.Enabled = true;
                    this.StepOverButton.Enabled = true;
                    this.StepIntoButton.Enabled = true;
                    this.mDEBUGSTARTToolStripMenuItem.Enabled = true;
                    this.mSTEPINToolStripMenuItem.Enabled = true;
                    this.mSTEPOVERToolStripMenuItem.Enabled = true;
                    this.mRUNTOCURToolStripMenuItem.Enabled = true;
                }

                this.miRun.Enabled = true;
                this.StartButton.Enabled = true;
            }
            else
            {
                this.mDEBUGSTOPToolStripMenuItem.Enabled = false;
                this.mSTEPToolStripMenuItem.Enabled = false;
                this.StartDebugButton.Enabled = false;
                this.StopDebugButton.Enabled = false;
                this.StepOutButton.Enabled = false;
                this.StepOverButton.Enabled = false;
                this.StepIntoButton.Enabled = false;
                this.mDEBUGSTARTToolStripMenuItem.Enabled = false;
                this.mSTEPINToolStripMenuItem.Enabled = false;
                this.mSTEPOVERToolStripMenuItem.Enabled = false;
                this.mRUNTOCURToolStripMenuItem.Enabled = false;
                this.mSTEPToolStripMenuItem.Enabled = false;
                this.miRun.Enabled = false;
                this.StartButton.Enabled = false;
            }
        }

        bool mDEBUGSTOPToolStripMenuItem_Enabled;
        bool mDEBUGSTARTToolStripMenuItem_Enabled;
        bool mSTEPToolStripMenuItem_Enabled;
        bool StartDebugButton_Enabled;
        bool StopDebugButton_Enabled;
        bool StepOutButton_Enabled;
        bool StepOverButton_Enabled;
        bool StepIntoButton_Enabled;
        bool mSTEPINToolStripMenuItem_Enabled;
        bool mSTEPOVERToolStripMenuItem_Enabled;
        bool mRUNTOCURToolStripMenuItem_Enabled;
        bool miRun_Enabled;

        private void SaveDebugContext()
        {
            mDEBUGSTOPToolStripMenuItem_Enabled = this.mDEBUGSTOPToolStripMenuItem.Enabled;
            mDEBUGSTARTToolStripMenuItem_Enabled = this.mDEBUGSTARTToolStripMenuItem.Enabled;
            mSTEPToolStripMenuItem_Enabled = this.mSTEPToolStripMenuItem.Enabled;
            StartDebugButton_Enabled = this.StartDebugButton.Enabled;
            StopDebugButton_Enabled = this.StopDebugButton.Enabled;
            StepOutButton_Enabled = this.StepOutButton.Enabled;
            StepOverButton_Enabled = this.StepOverButton.Enabled;
            StepIntoButton_Enabled = this.StepIntoButton.Enabled;
            mSTEPINToolStripMenuItem_Enabled = this.mSTEPINToolStripMenuItem.Enabled;
            mSTEPOVERToolStripMenuItem_Enabled = this.mSTEPOVERToolStripMenuItem.Enabled;
            mRUNTOCURToolStripMenuItem_Enabled = this.mRUNTOCURToolStripMenuItem.Enabled;
            mSTEPToolStripMenuItem_Enabled = this.mSTEPToolStripMenuItem.Enabled;
            miRun_Enabled = this.miRun.Enabled;
        }

        private void RestoreDebugContext()
        {
            this.mDEBUGSTOPToolStripMenuItem.Enabled = mDEBUGSTOPToolStripMenuItem_Enabled;
            this.mDEBUGSTARTToolStripMenuItem.Enabled = mDEBUGSTARTToolStripMenuItem_Enabled;
            this.mSTEPToolStripMenuItem.Enabled = mSTEPToolStripMenuItem_Enabled;
            this.StartDebugButton.Enabled = StartDebugButton_Enabled;
            this.StopDebugButton.Enabled = StopDebugButton_Enabled;
            this.StepOutButton.Enabled = StepOutButton_Enabled;
            this.StepOverButton.Enabled = StepOverButton_Enabled;
            this.StepIntoButton.Enabled = StepIntoButton_Enabled;
            this.mDEBUGSTARTToolStripMenuItem.Enabled = mDEBUGSTARTToolStripMenuItem_Enabled;
            this.mSTEPINToolStripMenuItem.Enabled = mSTEPINToolStripMenuItem_Enabled;
            this.mSTEPOVERToolStripMenuItem.Enabled = mSTEPOVERToolStripMenuItem_Enabled;
            this.mRUNTOCURToolStripMenuItem.Enabled = mRUNTOCURToolStripMenuItem_Enabled;
            this.mSTEPToolStripMenuItem.Enabled = mSTEPToolStripMenuItem_Enabled;
            this.miRun.Enabled = miRun_Enabled;
            this.StartButton.Enabled = miRun_Enabled;
        }

        private void SetDebugButtonsAsByDebug()
        {
            RestoreDebugContext();
        }

        public void EnableCodeCompletionToolTips(bool flag)
        {
            if (flag)
            {
                foreach (CodeFileDocumentControl tp in this.OpenDocuments.Values)
                {
                    AddEditorHandlers(tp);
                }
            }
            else
            {
                foreach (CodeFileDocumentControl tp in this.OpenDocuments.Values)
                {
                    RemoveEditorHandlers(tp);
                }
            }
        }

        private bool watch_vis = false;
        public void SetDebugTabsVisible(bool flag)
        {
            if (flag)
            {
                if (DebugWatchListWindow != null && !DebugWatchListWindow.IsHidden)
                {
                    watch_vis = true;
                }
                if (WorkbenchServiceFactory.DebuggerManager.ShowDebugTabs)
                {
                    if (!BottomTabsVisible)
                        BottomTabsVisible = true;
                    //BPanel_Resize(null, EventArgs.Empty);
                    BottomDockPanel.Size = new System.Drawing.Size(1920, 1080);
                    if (DebugWatchListWindow != null)
                        ShowContent(DebugWatchListWindow, false);
                    ShowContent(DebugVariablesListWindow, false);
                    this.Width = this.Width + 1;
                    this.Width = this.Width - 1;
                }
            }
            else
            {
                //HideContent(DebugWatchListWindow);
                if (!watch_vis && DebugWatchListWindow != null)
                {
                    HideContent(DebugWatchListWindow);
                }
                watch_vis = false;
                HideContent(DebugVariablesListWindow);
            }
        }

        private void ChangeDebugButtons(bool stop_as_visible)
        {
            if (PlayPauseButtonsVisibleInPanel)
                if (stop_as_visible)
                {
                    this.StartDebugButton.Visible = false;
                    this.StopDebugButton.Visible = true;
                }
                else
                {
                    this.StartDebugButton.Visible = true;
                    this.StopDebugButton.Visible = false;
                }
        }

        public void SetStopEnabled(bool enabled)
        {
            stopButton.Enabled = miStop.Enabled = enabled;
        }

        public void SetOptionsEnabled(bool val)
        {
            foreach (OptionsContent.IOptionsContent content in this.optionsContentEngine.ContentList)
                content.Content.Enabled = val;
        }
    }
}
