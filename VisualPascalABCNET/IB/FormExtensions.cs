// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
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
using Debugger;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
//using Aga.Controls.Tree;
//using Aga.Controls.Tree.NodeControls;

namespace VisualPascalABC
{
    public class TextAreaHelper
    {
        public static Hashtable TextAreas = new Hashtable();

        public static string GetFileNameByTextArea(TextArea ta)
        {
            return (string)TextAreas[ta];
        }
    }
    public partial class Form1 : Form, VisualPascalABCPlugins.IWorkbenchDebuggerOperationsService
    {
        //public DebugHelper WorkbenchServiceFactory.DebuggerManager;
        private Hashtable DebugTabs = new Hashtable();

        private ICodeFileDocument debuggedPage = null;

        IDebuggerManager IWorkbench.DebuggerManager
        {
            get
            {
                return WorkbenchServiceFactory.DebuggerManager;
            }
        }

        private void SetDebugTextOnPage(CodeFileDocumentControl tp)
        {
            SetTabPageText(tp);
        }

        private void AddBreakPointHandler(CodeFileDocumentControl tp, string FileName)
        {
            string name = tp.FileName;
            TextAreaHelper.TextAreas[tp.TextEditor.ActiveTextAreaControl.TextArea] = FileName;
            tp.TextEditor.ActiveTextAreaControl.TextArea.IconBarMargin.MouseDown += BreakPointFactory.IconBarMouseDown;
        }

        private void ShowAddedBreakpoints(CodeFileDocumentControl tp)
        {
            string name = tp.FileName;
            BreakPointFactory.AddBreakpoints(name, tp.TextEditor.ActiveTextAreaControl.TextArea);
        }

        private void mDEBUGSTARTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!WorkbenchServiceFactory.DebuggerManager.IsRunning)
                //Start(false,CurrentCodeFileDocument);
                WorkbenchServiceFactory.RunService.Run(true, false, false);
            else
                WorkbenchServiceFactory.DebuggerManager.Continue();
        }

        public void StartTimer()
        {
        	//timer.Start();
        }
        
        public void StopTimer()
        {
        	//timer.Stop();
        }

        private bool row_exists(string expr)
        {
        	for (int i=0; i<this.WdataGridView1.Rows.Count; i++)
        	{
        		if ((string)this.WdataGridView1.Rows[i].Cells[0].Value == expr) 
        			return true;
        	}
        	return false;
        }
        
        public void AddVariable(string expr, bool show_tab)
        {
        	if (row_exists(expr)) return;
        	RetValue rv = WorkbenchServiceFactory.DebuggerManager.Evaluate(expr);
        	int i = this.WdataGridView1.Rows.Count-1;
        	BottomTabsVisible = true;
        	if (show_tab)
            SelectContent(DebugWatchListWindow, false);
            //this.BottomTabControl.SelectTab(this.tpWarningList);
        	//if (this.WdataGridView1.Rows.Count == 1 && this.WdataGridView1.Rows[0].Cells[0].Value == null) i = 0;
        	if (this.WdataGridView1.Rows[0].Cells[0].Value == null) i = 0;
        	else //if (this.WdataGridView1.Rows[1].Cells[0].Value != null)
        	{
        		this.WdataGridView1.Rows.Insert(0,1); i=0;
        		//i = this.WdataGridView1.Rows.Count-1;
        	}
        	if (this.WdataGridView1.Rows.Count == 1) this.WdataGridView1.Rows.Insert(0,1);
        	this.WdataGridView1.Rows[i].Cells[0].Value = expr;
        	DebugWatchListWindow.RefreshRow(i);
        }
        
        public void AddVariable(ListItem nv)
        {
        	if (row_exists(nv.Name)) return;
        	//this.WdataGridView1.Rows.Add();
        	int i = this.WdataGridView1.Rows.Count-1;
        	//this.BottomTabControl.SelectTab(this.tpWarningList);
        	BottomTabsVisible = true;
            SelectContent(DebugWatchListWindow, false);            
        	//if (this.WdataGridView1.Rows.Count == 1 && this.WdataGridView1.Rows[0].Cells[0].Value == null) i = 0;
        	if (this.WdataGridView1.Rows[0].Cells[0].Value == null) i = 0;
        	else //if (this.WdataGridView1.Rows[1].Cells[0].Value != null)
        	{
        		this.WdataGridView1.Rows.Insert(0,1); i=0;
        		//i = this.WdataGridView1.Rows.Count-1;
        	}
        	if (this.WdataGridView1.Rows.Count == 1) this.WdataGridView1.Rows.Insert(0,1);
        	this.WdataGridView1.Rows[i].Cells[0].Value = nv.Name;
        	DebugWatchListWindow.RefreshRow(i);
        }

        public void AddObjectVariable(NamedValue lv)
        {
            if (lv.IsArray)
            {
                AddArrayVariable(lv);
                return;
            }
            NamedValueCollection nvc = lv.GetMembers();
            //System.Windows.Forms.TreeView obj = new System.Windows.Forms.TreeView();
            //obj.Nodes.Add(
            // dgLocalVars.Rows.Add(lv.Name, "<object>");
        }

        public void AddArrayVariable(NamedValue lv)
        {
            //dgLocalVars.Rows.Add(lv.Name, lv.AsString);
        }

        void IWorkbenchDebuggerOperationsService.AddDebugPage(ICodeFileDocument tabPage)
        {
            debuggedPage = tabPage;
            AddDebugPage(debuggedPage as CodeFileDocumentControl);
            WorkbenchServiceFactory.DebuggerManager.SetAsDebugPage(debuggedPage);
        }

        private bool DebugStop()
        {
            WorkbenchServiceFactory.DebuggerManager.Stop(CurrentEXEFileName);
            return true;
        }

        private void mDEBUGSTOPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DebugStop();
            WorkbenchServiceFactory.DebuggerManager.DebugBreak();
        }

        private bool clicked_stop_debug_in_menu = false;

        private void mDEBUGENDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clicked_stop_debug_in_menu = true;
            DebugStop();
        }

        private void mSTEPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkbenchServiceFactory.DebuggerManager.StepOut();
        }

        private void mSTEPINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WorkbenchServiceFactory.DebuggerManager.IsRunning)
            {
                WorkbenchServiceFactory.DebuggerManager.Status = DebugStatus.StepIn;
                WorkbenchServiceFactory.DebuggerManager.StepInto();
            }
            else
                //Start(true,CurrentCodeFileDocument);
                WorkbenchServiceFactory.RunService.Run(true, false, true);
        }

        private void mRUNTOCURToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkbenchServiceFactory.DebuggerManager.Status = DebugStatus.None;
            if (WorkbenchServiceFactory.DebuggerManager.IsRunning)
                WorkbenchServiceFactory.DebuggerManager.RunToCursor();
            else
            {
                //StartWithGoto(CurrentCodeFileDocument);
                WorkbenchServiceFactory.RunService.Run(true, true, false);
            }
        }

        private void mSTEPOVERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WorkbenchServiceFactory.DebuggerManager.IsRunning)
            {
                WorkbenchServiceFactory.DebuggerManager.Status = DebugStatus.StepOver;
                WorkbenchServiceFactory.DebuggerManager.StepOver();
            }
            else
                WorkbenchServiceFactory.RunService.Run(true, false, true);
        }

        private List<string> search_debug_paths = new List<string>();

        public void AddSearchDebugPath(string path)
        {
            search_debug_paths.Add(path);
        }
		
        internal List<CodeFileDocumentControl> TabStack = new List<CodeFileDocumentControl>();
        internal List<TextBoxBase> OutputBoxStack = new List<TextBoxBase>();

        delegate void RefreshWatchDelegate (object sender, DataGridViewCellEventArgs e);
                
        private void InitForm()
        {
            /*this.PictColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.PictColumn.ValuesAreIcons = false;
            this.PictColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.PictColumn.FillWeight = 20F;
            this.PictColumn.Frozen = true;
            //this.PictColumn.Name = "PictColumn";
            this.PictColumn.ReadOnly = true;
            this.PictColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PictColumn.Width = 25;
            this.PictColumn.DefaultCellStyle.NullValue = null;*/
            
            
            //this.tpVarList.SuspendLayout();
            //this.BottomTabControl.TabPages.Add(this.tpWarningList);
            //this.BottomTabControl.TabPages.Add(this.tpVarList);
			
            this.mShowVarsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mShowVarsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mDEBUGSTARTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mDEBUGSTOPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
            this.mSTEPOVERToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSTEPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSTEPINToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mRUNTOCURToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            int ind = this.MainMenuStrip.Items.IndexOf(this.mrProgram);
            //this.MainMenuStrip.Items.Insert(ind + 1, this.mShowVarsMenuItem);
            this.mDEBUGSTARTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mDEBUGSTOPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
            this.mSTEPOVERToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSTEPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSTEPINToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mRUNTOCURToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mDEBUGENDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mShowVarsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mDEBUGSTARTToolStripMenuItem,
            this.mDEBUGSTOPToolStripMenuItem,
            this.mDEBUGENDToolStripMenuItem,
            this._toolStripMenuItem10,
            this.mSTEPOVERToolStripMenuItem,
            this.mSTEPINToolStripMenuItem,
            this.mSTEPToolStripMenuItem,
            this.mRUNTOCURToolStripMenuItem});
            this.mrProgram.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._toolStripMenuItem10,
            this.mSTEPOVERToolStripMenuItem,
            this.mSTEPINToolStripMenuItem,
            this.mSTEPToolStripMenuItem,
            this.mRUNTOCURToolStripMenuItem});
            
            this.mShowVarsMenuItem.Name = "mShowVarsMenuItem";
            this.mShowVarsMenuItem.Size = new System.Drawing.Size(73, 24);
            this.mShowVarsMenuItem.Text = "MR_DEBUG";
            // 
            // mDEBUGSTARTToolStripMenuItem
            // 
            this.mDEBUGSTARTToolStripMenuItem.Name = "mDEBUGSTARTToolStripMenuItem";
            this.mDEBUGSTARTToolStripMenuItem.Visible = false;
            this.mDEBUGSTARTToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F8)));
            this.mDEBUGSTARTToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.mDEBUGSTARTToolStripMenuItem.Text = "M_DEBUGSTART";
            this.mDEBUGSTARTToolStripMenuItem.Click += new System.EventHandler(this.mDEBUGSTARTToolStripMenuItem_Click);
            this.mDEBUGSTARTToolStripMenuItem.Image = new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VisualPascalABC.Resources.Icons.16x16.Debug.Continue.png"));
            // 
            // mDEBUGSTOPToolStripMenuItem
            // 
            this.mDEBUGSTOPToolStripMenuItem.Enabled = false;
            this.mDEBUGSTOPToolStripMenuItem.Visible = false;
            this.mDEBUGSTOPToolStripMenuItem.Name = "mDEBUGSTOPToolStripMenuItem";
            this.mDEBUGSTOPToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.mDEBUGSTOPToolStripMenuItem.Text = "M_DEBUGSTOP";
            this.mDEBUGSTOPToolStripMenuItem.Click += new System.EventHandler(this.mDEBUGSTOPToolStripMenuItem_Click);
            this.mDEBUGSTOPToolStripMenuItem.Image = new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VisualPascalABC.Resources.Icons.16x16.Debug.Break.png"));

            this.mDEBUGENDToolStripMenuItem.Enabled = false;
            this.mDEBUGENDToolStripMenuItem.Visible = false;
            this.mDEBUGENDToolStripMenuItem.Name = "mDEBUGENDToolStripMenuItem";
            this.mDEBUGENDToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.mDEBUGENDToolStripMenuItem.Text = "M_ENDDEBUG";
            this.mDEBUGENDToolStripMenuItem.Click += new System.EventHandler(this.mDEBUGENDToolStripMenuItem_Click);
            this.mDEBUGENDToolStripMenuItem.Image = global::VisualPascalABC.Properties.Resources.stop;
            this.mDEBUGENDToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Maroon;
            // 
            // toolStripMenuItem10
            // 
            this._toolStripMenuItem10.Name = "toolStripMenuItem10";
            this._toolStripMenuItem10.Size = new System.Drawing.Size(197, 6);
            // 
            // mSTEPOVERToolStripMenuItem
            // 
            this.mSTEPOVERToolStripMenuItem.Name = "mSTEPOVERToolStripMenuItem";
            this.mSTEPOVERToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.mSTEPOVERToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.mSTEPOVERToolStripMenuItem.Text = "M_STEPOVER";
            this.mSTEPOVERToolStripMenuItem.Click += new System.EventHandler(this.mSTEPOVERToolStripMenuItem_Click);
            this.mSTEPOVERToolStripMenuItem.Image = new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VisualPascalABC.Resources.Icons.16x16.Debug.StepOver.png"));
            // 
            // mSTEPToolStripMenuItem
            // 
            this.mSTEPToolStripMenuItem.Name = "mSTEPToolStripMenuItem";
            this.mSTEPToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.mSTEPToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.mSTEPToolStripMenuItem.Text = "M_STEPOUT";
            this.mSTEPToolStripMenuItem.Click += new System.EventHandler(this.mSTEPToolStripMenuItem_Click);
            this.mSTEPToolStripMenuItem.Enabled = false;
            this.mSTEPToolStripMenuItem.Image = new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VisualPascalABC.Resources.Icons.16x16.Debug.StepOut.png"));
            // 
            // mSTEPINToolStripMenuItem
            // 
            this.mSTEPINToolStripMenuItem.Name = "mSTEPINToolStripMenuItem";
            this.mSTEPINToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.mSTEPINToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.mSTEPINToolStripMenuItem.Text = "M_STEPIN";
            this.mSTEPINToolStripMenuItem.Click += new System.EventHandler(this.mSTEPINToolStripMenuItem_Click);
            this.mSTEPINToolStripMenuItem.Image = new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VisualPascalABC.Resources.Icons.16x16.Debug.StepInto.png"));
            // 
            // mRUNTOCURToolStripMenuItem
            // 
            this.mRUNTOCURToolStripMenuItem.Name = "mRUNTOCURToolStripMenuItem";
            this.mRUNTOCURToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.mRUNTOCURToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.mRUNTOCURToolStripMenuItem.Text = "M_RUNTOCUR";
            this.mRUNTOCURToolStripMenuItem.Enabled = true;
            this.mRUNTOCURToolStripMenuItem.Click += new System.EventHandler(this.mRUNTOCURToolStripMenuItem_Click);
            //this.tpVarList.ResumeLayout(false);
            //this.ResumeLayout(false);
            //this.PerformLayout();
            this.StartDebugButton = new System.Windows.Forms.ToolStripButton();
            this.StartDebugButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StartDebugButton.Visible = true;
            this.StartDebugButton.Enabled = true;
            this.StartDebugButton.Image = new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VisualPascalABC.Resources.Icons.16x16.Debug.Continue.png"));
            this.StartDebugButton.ImageTransparentColor = System.Drawing.Color.Red;
            this.StartDebugButton.Name = "StartDebugButton";
            this.StartDebugButton.Size = new System.Drawing.Size(23, 22);
            //this.StartDebugButton.Text = "Start Debug";
            this.StartDebugButton.ToolTipText = "M_DEBUGSTART";
            //this.StartDebugButton.EnabledChanged += new System.EventHandler(this.ReCompileButton_EnabledChanged);
            this.StartDebugButton.Click += new System.EventHandler(this.mDEBUGSTARTToolStripMenuItem_Click);

            this.StopDebugButton = new System.Windows.Forms.ToolStripButton();
            this.StopDebugButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StopDebugButton.Enabled = false;
            this.StopDebugButton.Visible = false;
            this.StopDebugButton.Image = new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VisualPascalABC.Resources.Icons.16x16.Debug.Break.png"));
            this.StopDebugButton.ImageTransparentColor = System.Drawing.Color.Red;
            this.StopDebugButton.Name = "StopDebugButton";
            this.StopDebugButton.Size = new System.Drawing.Size(23, 22);
            //this.StopDebugButton.Text = "Stop Debug";
            this.StopDebugButton.ToolTipText = "M_DEBUGSTOP";
            //this.StartDebugButton.EnabledChanged += new System.EventHandler(this.ReCompileButton_EnabledChanged);
            this.StopDebugButton.Click += new System.EventHandler(this.mDEBUGSTOPToolStripMenuItem_Click);

            this.StepOverButton = new System.Windows.Forms.ToolStripButton();
            this.StepOverButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StepOverButton.Enabled = true;
            this.StepOverButton.Visible = true;
            this.StepOverButton.Image = new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VisualPascalABC.Resources.Icons.16x16.Debug.StepOver.png"));
            this.StepOverButton.ImageTransparentColor = System.Drawing.Color.Red;
            this.StepOverButton.Name = "StepOverButton";
            this.StepOverButton.Size = new System.Drawing.Size(23, 22);
            //this.StopDebugButton.Text = "Stop Debug";
            this.StepOverButton.ToolTipText = "M_STEPOVER";
            //this.StartDebugButton.EnabledChanged += new System.EventHandler(this.ReCompileButton_EnabledChanged);
            this.StepOverButton.Click += new System.EventHandler(this.mSTEPOVERToolStripMenuItem_Click);

            this.StepIntoButton = new System.Windows.Forms.ToolStripButton();
            this.StepIntoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StepIntoButton.Enabled = true;
            this.StepIntoButton.Visible = true;
            this.StepIntoButton.Image = new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VisualPascalABC.Resources.Icons.16x16.Debug.StepInto.png"));
            this.StepIntoButton.ImageTransparentColor = System.Drawing.Color.Red;
            this.StepIntoButton.Name = "StepIntoButton";
            this.StepIntoButton.Size = new System.Drawing.Size(23, 22);
            //this.StopDebugButton.Text = "Stop Debug";
            this.StepIntoButton.ToolTipText = "M_STEPIN";
            //this.StartDebugButton.EnabledChanged += new System.EventHandler(this.ReCompileButton_EnabledChanged);
            this.StepIntoButton.Click += new System.EventHandler(this.mSTEPINToolStripMenuItem_Click);

            this.StepOutButton = new System.Windows.Forms.ToolStripButton();
            this.StepOutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StepOutButton.Enabled = false;
            this.StepOutButton.Visible = false;
            this.StepOutButton.Image = new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VisualPascalABC.Resources.Icons.16x16.Debug.StepOut.png"));
            this.StepOutButton.ImageTransparentColor = System.Drawing.Color.Red;
            this.StepOutButton.Name = "StepOutButton";
            this.StepOutButton.Size = new System.Drawing.Size(23, 22);
            //this.StopDebugButton.Text = "Stop Debug";
            this.StepOutButton.ToolTipText = "M_STEPOUT";
            //this.StartDebugButton.EnabledChanged += new System.EventHandler(this.ReCompileButton_EnabledChanged);
            this.StepOutButton.Click += new System.EventHandler(this.mSTEPToolStripMenuItem_Click);
            this.toolStrip1.Items.Insert(this.toolStrip1.Items.IndexOf(this.ReCompileButton), StepOutButton);
            this.toolStrip1.Items.Insert(this.toolStrip1.Items.IndexOf(this.StepOutButton), StepIntoButton);
            this.toolStrip1.Items.Insert(this.toolStrip1.Items.IndexOf(this.StepIntoButton), StepOverButton);
            this.toolStrip1.Items.Insert(this.toolStrip1.Items.IndexOf(this.StepOverButton), StopDebugButton);
            this.toolStrip1.Items.Insert(this.toolStrip1.Items.IndexOf(this.StopDebugButton), StartDebugButton);
            this.toolStrip1.Items.Insert(this.toolStrip1.Items.IndexOf(this.StartDebugButton), new ToolStripSeparator());
            /*timer = new Timer();
            timer.Interval = 2000;
            timer.Tick += TimerTicked;*/
            //timer.Start();
            WorkbenchServiceFactory.CodeCompletionParserController.SwitchOnIntellisence();
            /*this.tsGotoRealization = new System.Windows.Forms.ToolStripMenuItem();
            this.tsGotoRealization.Enabled = false;
            //this.tsGotoRealization.Image = ((System.Drawing.Image)(resources.GetObject("tsGotoDefinition.Image")));
            this.tsGotoRealization.Name = "tsGotoRealization";
            this.tsGotoRealization.Size = new System.Drawing.Size(193, 22);
            this.tsGotoRealization.Text = "M_GOTO_REALIZATION";
            */
            this.cmGotoDefinition.Click += GotoDefinitionClicked;
            this.cmGotoRealization.Click += GotoRealizationClicked;
            this.tsGotoDefinition.Click += GotoDefinitionClicked;
            this.tsGotoRealization.Click += GotoRealizationClicked;
            //this.cmFindAllReferences.Click += FindReferencesClicked;
            //this.tsGotoDefinition.Enabled = true;
            this.cmEditor.Opening += OpeningContextMenu;
            this.cmEditor.Closing += ClosingContextMenu;
            this.mrService.DropDownOpened += new EventHandler(tsIntellisense_DropDownOpened);
            this.cmEditor.Items.Insert(1,cmGotoRealization);
            this.cmRename = new ToolStripMenuItem();
            this.cmRename.Enabled = true;
            //this.cmRename.Image = ((System.Drawing.Image)(resources.GetObject("cmGotoRealization.Image")));
            this.cmRename.Name = "cmRename";
            this.cmRename.Size = new System.Drawing.Size(224, 22);
            this.cmRename.Text = "M_RENAME";
            this.cmRename.Click += RenameClicked;
            //this.cmRename.ShortcutKeys = Keys.F2;
            this.cmEditor.Items.Insert(3,cmRename);
        }

        void tsIntellisense_DropDownOpened(object sender, EventArgs e)
        {
            if (UserOptions.AllowCodeCompletion && CodeCompletion.CodeCompletionController.IntellisenseAvailable())
            {
                //GotoAction ga = new GotoAction();
                TextArea ta = CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.TextArea;
                this.tsGotoDefinition.Enabled = CodeCompletionActionsManager.CanGoTo(ta);
                this.tsGotoRealization.Enabled = CodeCompletionActionsManager.CanGoToRealization(ta);
                this.tsFindAllReferences.Enabled = CodeCompletionActionsManager.CanFindReferences(ta);
                //this.cmFindAllReferences.Enabled = ga.CanFindReferences(ta);
        		this.miGenerateRealization.Enabled = CodeCompletionActionsManager.CanGenerateRealization(ta);
            }
            else
            {
                DisableIntellisenseDropDownOptions();
            }
        }

        private void OpeningContextMenu(object sender, CancelEventArgs args)
        {
            if (CodeCompletion.CodeCompletionController.IntellisenseAvailable())
            {
                // GotoAction ga = new GotoAction();
                TextArea ta = CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.TextArea;
            
                this.cmGotoDefinition.Enabled = CodeCompletionActionsManager.CanGoTo(ta);
                this.cmGotoRealization.Enabled = CodeCompletionActionsManager.CanGoToRealization(ta);
                this.cmFindAllReferences.Enabled = CodeCompletionActionsManager.CanFindReferences(ta);
                this.cmGenerateRealization.Enabled = CodeCompletionActionsManager.CanGenerateRealization(ta);
                this.cmRename.Enabled = CodeCompletionActionsManager.CanGoTo(ta);
            }
            else
            {
                DisableIntellisenseContextMenuOptions();
            }
        }

        private void DisableIntellisenseContextMenuOptions()
        {
            this.cmGotoDefinition.Enabled = false;
            this.cmGotoRealization.Enabled = false;
            this.cmFindAllReferences.Enabled = false;
            this.cmGenerateRealization.Enabled = false;
            this.cmRename.Enabled = false;
        }

        private void DisableIntellisenseDropDownOptions()
        {
            this.tsGotoDefinition.Enabled = false;
            this.tsGotoRealization.Enabled = false;
            this.tsFindAllReferences.Enabled = false;
            this.miGenerateRealization.Enabled = false;
        }
        
        private void ClosingContextMenu(object sender, CancelEventArgs args)
        {
        	this.cmRename.Enabled = true;
        }
        private void GotoDefinitionClicked(object sender, EventArgs args)
        {
            //GotoAction ga = new GotoAction();
            CodeCompletionActionsManager.GotoDefinition(CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.TextArea);
        }
        
        private void GotoRealizationClicked(object sender, EventArgs args)
        {
        	//GotoAction ga = new GotoAction();
            CodeCompletionActionsManager.GotoRealization(CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.TextArea);
        }
        
        private void FindReferencesClicked(object sender, EventArgs args)
        {
        	//GotoAction ga = new GotoAction();
            List<SymbolsViewerSymbol> syms = CodeCompletionActionsManager.FindReferences(CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.TextArea);
        	FindSymbolsResultWindow.FindSymbolResults.Show(syms);
        }
        
        private void RenameClicked(object sender, EventArgs args)
        {
         	//GotoAction ga = new GotoAction();
         	if (CodeCompletionActionsManager.CanGoTo(CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.TextArea))
         	    CodeCompletionActionsManager.Rename(CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.TextArea);
        }
        

        private System.Windows.Forms.ToolStripMenuItem mShowVarsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mDEBUGSTARTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mDEBUGSTOPToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator _toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem mSTEPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mSTEPINToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mRUNTOCURToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mSTEPOVERToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mDEBUGENDToolStripMenuItem;
        private System.Windows.Forms.DataGridView localVarList
        {
            get
            {
                return DebugVariablesListWindow.watchList;
            }
        }

        private System.Windows.Forms.DataGridView WdataGridView1
        {
            get
            {
                return DebugWatchListWindow.watchList;
            }
        }
        //private System.Windows.Forms.DataGridViewImageColumn PictColumn;
        //private System.Windows.Forms.TabPage tpVarList;

        private System.Windows.Forms.ToolStripButton StartDebugButton;
        private System.Windows.Forms.ToolStripButton StopDebugButton;
        private System.Windows.Forms.ToolStripButton StepOverButton;
        private System.Windows.Forms.ToolStripButton StepIntoButton;
        private System.Windows.Forms.ToolStripButton StepOutButton;

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripMenuItem cmRename;
        //private System.Windows.Forms.ToolStripMenuItem tsGotoRealization;
        //internal ICSharpCode.SharpDevelop.Dom.ProjectContentRegistry pcRegistry;
        //internal ICSharpCode.SharpDevelop.Dom.DefaultProjectContent myProjectContent;
        //internal ICSharpCode.SharpDevelop.Dom.ICompilationUnit lastCompilationUnit;

    }
    
    public class VisualPABCSingleton
    {
    	private static Form1 mainForm=null;

    	public static Form1 MainForm
    	{
    		get
    		{
    			return mainForm;
    		}
    		set
    		{
    			if (mainForm == null)
    				mainForm = value;
    		}
    	}
    }
    
}
