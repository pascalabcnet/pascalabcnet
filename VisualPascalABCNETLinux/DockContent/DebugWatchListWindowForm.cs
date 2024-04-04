// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace VisualPascalABC
{
    public partial class DebugWatchListWindowForm : BottomDockContentForm
    {
//        private System.Windows.Forms.DataGridViewTextBoxColumn WColumn4;
//        private System.Windows.Forms.DataGridViewTextBoxColumn WColumn1;
//        private System.Windows.Forms.DataGridViewTextBoxColumn WColumn2;
//        private System.Windows.Forms.DataGridViewTextBoxColumn WColumn3;
//        private System.Windows.Forms.DataGridViewTextBoxColumn WColumn5;
		
		private System.Windows.Forms.DataGridViewTextBoxColumn WColumn4;
        private AdvancedDataGridView.TreeGridColumn WColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn WColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn WColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn WColumn5;
        
        private System.Windows.Forms.DataGridViewTextBoxColumn WColumn6Empty;

        private System.Threading.Thread th;
        public void RefreshWatch()
        {
            //if (!dbgHelper.IsRunning) return;
            /*if (th != null && th.ThreadState == System.Threading.ThreadState.Running)
            {
                System.Threading.Thread.Sleep(50);
                if (th.ThreadState == System.Threading.ThreadState.Running) th.Suspend();
            }*/
//            if (th == null || th.ThreadState != System.Threading.ThreadState.Running)
//            {
//            	th = new System.Threading.Thread(new System.Threading.ThreadStart(RefreshWatchInThread));
//            	th.Start();
//            }
            RefreshWatchInThread();
        }
        
        private string WrapPrimValue(object val)
        {
            if (val is string || val is char) return "'" + val.ToString() + "'";
            return val.ToString();
        }

        public void ClearAllSubTrees()
        {
        	for (int i = 0; i < this.watchList.Rows.Count; i++)
            {
            	if (this.watchList.Rows[i].Cells[0].Value != null)
                {
            		ClearRow(i);
            	}
        	}
        }
        
        public void ClearRow(int i)
        {
            AdvancedDataGridView.TreeGridNode node = this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode;
        	if (node.IsExpanded)
            node.Collapse();
        	node.populated = false;
            node.IsExpanded = false;
            node.InExpanding = false;
            node.HasChildren = false;
            node._grid= null;
        	node.Nodes.Clear();
        	this.watchList.InvalidateRow(i);
        }
        
        public void SetUndefinedValue(int i)
        {
        	if (this.watchList.Rows[i].Cells[0].Value != null)
            {
        		this.ClearRow(i);
                //FixedItem fi = new FixedItem(this.watchList.Rows[i].Cells[0].Value as string,PascalABCCompiler.StringResources.Get("EXPR_VALUE_UNDEFINED_VALUE"),PascalABCCompiler.StringResources.Get("EXPR_VALUE_UNDEFINED_TYPE"));
                FixedItem fi = new FixedItem(this.watchList.Rows[i].Cells[0].Value as string,"","");
                //fi.imageIndex = CodeCompletionProvider.ImagesProvider.IconNumberEvalError;
                this.watchList.Rows[i].Cells[1].Value = fi.Text;
                this.watchList.Rows[i].Cells[2].Value = fi.Type;
                (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode)._grid = this.watchList;
                (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode).Content = fi;
                this.watchList.InvalidateCell(0,i);
        	}
        }
        
        public string[] GetExpressions()
        {
        	List<string> exprs = new List<string>();
        	for (int i=0; i<watchList.Rows.Count-1; i++)
        	{
        		exprs.Add(this.watchList.Rows[i].Cells[0].Value as string);
        	}
        	return exprs.ToArray();
        }
        
        public void RefreshRow(int i)
        {
            if (this.watchList.Rows[i].Cells[0].Value != null)
            {
                if ((this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode).UserRow)
                {
                    string s = this.watchList.Rows[i].Cells[0].Value as string;
                    
                    RetValue rv = WorkbenchServiceFactory.DebuggerManager.Evaluate(s);
                    if (rv.syn_err)
                    {
                        this.ClearRow(i);
                        FixedItem fi = new FixedItem(this.watchList.Rows[i].Cells[0].Value as string, PascalABCCompiler.StringResources.Get("EXPR_VALUE_SYNTAX_ERROR_IN_EXPR"), "");
                        fi.imageIndex = CodeCompletionProvider.ImagesProvider.IconNumberEvalError;
                        this.watchList.Rows[i].Cells[1].Value = fi.Text;
                        this.watchList.Rows[i].Cells[2].Value = fi.Type;
                        (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode)._grid = this.watchList;
                        (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode).Content = fi;
                    }
                    else
                        if (rv.monoValue != null)
                        {
                            //this.ClearRow(i);
                            try
                            {
                                ValueItem vi = new ValueItem(rv.monoValue, s, WorkbenchServiceFactory.DebuggerManager.evaluator.declaringType);
                                (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode)._grid = this.watchList;
                                (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode).Content = vi;
                                this.watchList.Rows[i].Cells[1].Value = rv.monoValue.Value;//rv.obj_val.AsString;
                                this.watchList.Rows[i].Cells[2].Value = rv.monoValue.TypeName;
                                this.watchList.InvalidateCell(0, i);
                            }
                            catch (System.Exception e)
                            {
                                
                            }
                        }
                        else if (rv.prim_val != null)
                        {
                            //this.ClearRow(i);
                            FixedItem fi = new FixedItem(s, WrapPrimValue(rv.prim_val), DebugUtils.WrapTypeName(rv.prim_val.GetType()));
                            this.watchList.Rows[i].Cells[1].Value = fi.Text;
                            this.watchList.Rows[i].Cells[2].Value = fi.Type;
                            (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode)._grid = this.watchList;
                            (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode).Content = fi;
                            this.watchList.InvalidateCell(0, i);
                        }
                        else if (rv.monoType != null)
                        {
                            //this.ClearRow(i);
                            BaseTypeItem bti = new BaseTypeItem(rv.monoType, rv.managed_type);
                            (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode)._grid = this.watchList;
                            (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode).Content = bti;
                        }
                        else
                        {
                            this.ClearRow(i);
                            FixedItem fi = null;
                            if (WorkbenchServiceFactory.DebuggerManager.IsRunning)
                            {
                                fi = new FixedItem(s, rv.err_mes != null ? rv.err_mes : PascalABCCompiler.StringResources.Get("EXPR_VALUE_UNDEFINED_VALUE"),/*PascalABCCompiler.StringResources.Get("EXPR_VALUE_UNDEFINED_TYPE")*/"");
                                fi.imageIndex = CodeCompletionProvider.ImagesProvider.IconNumberEvalError;
                            }
                            else
                            {
                                fi = new FixedItem(s, "", "");
                            }
                            this.watchList.Rows[i].Cells[1].Value = fi.Text;
                            this.watchList.Rows[i].Cells[2].Value = fi.Type;
                            (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode)._grid = this.watchList;
                            (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode).Content = fi;
                            this.watchList.InvalidateCell(0, i);
                        }
                }
                else
                {

                }
            }
            else
            {
                //this.watchList.Rows[i].Cells[1].Value = null;
                //this.watchList.Rows[i].Cells[2].Value = null;
                if (i != this.watchList.Rows.Count - 1)
                {
                    this.ClearRow(i);
                    FixedItem fi = new FixedItem("", "", "");
                    this.watchList.Rows[i].Cells[1].Value = "";
                    this.watchList.Rows[i].Cells[2].Value = "";
                    (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode)._grid = this.watchList;
                    (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode).Content = fi;
                    this.watchList.InvalidateCell(0, i);
                    this.watchList.Rows.RemoveAt(i);
                }
                //this.watchList.InvalidateCell(0,i);
            }
        }
        
        private void RefreshWatchInThread()
        {
        	for (int i = 0; i < this.watchList.Rows.Count; i++)
            {
        		RefreshRow(i);
            }
        }
        
        private void RefreshWatchDel(object sender, DataGridViewCellEventArgs e)
        {
            //RefreshWatch();
            
        }

        private void CellClickDel(object sender, DataGridViewCellEventArgs e)
        {
        	try
        	{
        		if (e.ColumnIndex == 0 && this.watchList.Rows[e.RowIndex].Cells[0].Value == null)
            	{
                	this.watchList.Rows[e.RowIndex].Selected = false;
                	this.watchList.BeginEdit(false);
            	}
        	}
        	catch(Exception ex)
        	{
        		
        	}
        }
        
        private void KeyDownDel(object sender, KeyEventArgs e)
        {
        	if (e.KeyCode == Keys.Enter && !watchList.IsCurrentCellInEditMode && watchList.SelectedRows.Count == 1)
        		AddNewEntry(watchList.Rows[watchList.SelectedRows[0].Index].Cells[0].Value as string);
        }
        
        private void CellDblClickDel(object sender, DataGridViewCellEventArgs e)
        {
        	try
        	{
//        		if (e.ColumnIndex == 0 && this.watchList.Rows[e.RowIndex].Cells[0].Value == null)
				if ((this.watchList.Rows[e.RowIndex] as AdvancedDataGridView.TreeGridNode).UserRow && (this.watchList.Rows[e.RowIndex].Cells[0] as AdvancedDataGridView.TreeGridCell).on_name_click)
            	{
                	this.watchList.Rows[e.RowIndex].Selected = false;
                	this.watchList.BeginEdit(false);
            	}
        	}
        	catch(Exception ex)
        	{
        		
        	}
        }
		
        public static DebugWatchListWindowForm WatchWindow
        {
        	get
        	{
        		return watchWindow;
        	}
        }
        
        private static DebugWatchListWindowForm watchWindow;
        
        public DebugWatchListWindowForm(Form1 MainForm)
            :base(MainForm)
        {
           
            InitializeComponent();
//            this.WColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
//            this.WColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
//            this.WColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
//            this.WColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
//            this.WColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			watchWindow = this;
			Form1StringResources.SetTextForAllControls(this.cntxtWatch);
            this.WColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WColumn1 = new AdvancedDataGridView.TreeGridColumn();
            this.WColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WColumn6Empty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            
            this.watchList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.watchList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.watchList.BackgroundColor = System.Drawing.Color.White;
            this.watchList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.watchList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
				this.WColumn1,
				this.WColumn3,
				this.WColumn2,
				this.WColumn6Empty
			});
            this.watchList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.watchList.GridColor = System.Drawing.SystemColors.Control;
            this.watchList.Location = new System.Drawing.Point(1, 1);
            this.watchList.MultiSelect = true;
            this.watchList.Font = new Font("Tahoma",8);
            this.watchList.BorderStyle = BorderStyle.None;
            this.watchList.ReadOnly = false;
            this.watchList.RowHeadersVisible = false;
            this.watchList.RowTemplate.Height = 24;
            this.watchList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.watchList.Size = new System.Drawing.Size(782, 121);
            this.watchList.TabIndex = 0;
            //this.watchList.CellValueChanged += RefreshWatchDel;//delegate(object sender, DataGridViewCellEventArgs e) { RefreshWatch(); };
            this.watchList.CellEndEdit += delegate(object sender, DataGridViewCellEventArgs e) { RefreshRow(e.RowIndex); };
            this.watchList.CellClick += CellClickDel;
            this.watchList.CellDoubleClick += CellDblClickDel;
            this.watchList.KeyDown += KeyDownDel;
            this.watchList.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.watchList_UserDeletingRow);
            this.watchList._imageList = CodeCompletionProvider.ImagesProvider.ImageList;
            this.watchList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.watchList.ShowLines = true;
            this.watchList.Nodes.Add(new AdvancedDataGridView.TreeGridNode());
            //this.dataGridView1.ClientSizeChanged += new System.EventHandler(this.dataGridView1_ClientSizeChanged);
            //this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            //this.dataGridView1.SizeChanged += new System.EventHandler(this.dataGridView1_SizeChanged);
            //this.dataGridView1.Paint += new System.Windows.Forms.PaintEventHandler(this.dataGridView1_Paint);
            //this.dataGridView1.Resize += new System.EventHandler(this.dataGridView1_Resize);
    

            this.WColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.WColumn4.FillWeight = 20F;
            this.WColumn4.Frozen = true;
            this.WColumn4.HeaderText = "ER_NUM";
            this.WColumn4.Name = "WColumn4";
            this.WColumn4.ReadOnly = true;
            this.WColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.WColumn4.Width = 25;
            // 
            // Column1
            // 
            this.WColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.WColumn1.FillWeight = 20F;
            this.WColumn1.Frozen = false;
            this.WColumn1.HeaderText = "WT_EXPR";
            this.WColumn1.Name = "WColumn1";
            this.WColumn1.ReadOnly = false;
            this.WColumn1.Width = 250;
            // 
            // Column2
            // 
            this.WColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.WColumn2.FillWeight = 221.0297F;
            this.WColumn2.Frozen = false;
            this.WColumn2.HeaderText = "WT_TYPE";
            this.WColumn2.Name = "WColumn2";
            this.WColumn2.ReadOnly = true;
            this.WColumn2.Width = 200;
            // 
            // Column3
            // 
            this.WColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.WColumn3.FillWeight = 221.0297F;
            this.WColumn3.MinimumWidth = 221;
            this.WColumn3.Frozen = false;
            this.WColumn3.HeaderText = "WT_VALUE";
            this.WColumn3.Name = "WColumn3";
            this.WColumn3.ReadOnly = true;

            // 
            // Column6Empty
            // 
            this.WColumn6Empty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.WColumn6Empty.FillWeight = 101.5228F;
            this.WColumn6Empty.Frozen = false;
            this.WColumn6Empty.HeaderText = "";
            this.WColumn6Empty.Name = "Empty";
            this.WColumn6Empty.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.WColumn6Empty.ReadOnly = true;

            // 
            // Column5
            // 
            this.WColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.WColumn5.HeaderText = "ER_PATH";
            this.WColumn5.Name = "WColumn5";
            this.WColumn5.ReadOnly = true;
            this.WColumn5.Width = 200;

			this.watchList.AllowUserToAddRows = true;
            this.watchList.AllowUserToDeleteRows = true;
            this.watchList.AllowUserToResizeColumns = true;
            this.watchList.AllowUserToResizeRows = false;
            
        }

        private void DebugWathListWindowForm_Load(object sender, EventArgs e)
        {

        }
		
        private AddWatchForm frm;
        
        public void AddNewEntry(string entry)
        {
        	//this.watchList.ClearSelection();
        	if (frm == null)
        	{
        		frm = new AddWatchForm();
        		Form1StringResources.SetTextForAllControls(frm);
        	}
        	frm.EditValue = entry;
        	int i=0;
        	if (frm.ShowDialog() == DialogResult.OK)
        	{
        		if (string.IsNullOrEmpty(entry))
        		{
        		if (this.watchList.Rows[0].Cells[0].Value == null) i = 0;
        		else 
        		{
        			this.watchList.Rows.Insert(0,1); i=0;
        			//i = this.WdataGridView1.Rows.Count-1;
        		}
        		if (this.watchList.Rows.Count == 1) 
        			this.watchList.Rows.Insert(0,1);
        		}
        		else 
        			i = this.watchList.SelectedRows[0].Index;
        		if (frm.EditValue != "")
        		this.watchList.Rows[i].Cells[0].Value = frm.EditValue;
        		else
        		this.watchList.Rows[i].Cells[0].Value = null;
        		RefreshRow(i);
        	}
        }
        
        private void mADDEXPRToolStripMenuItem_Click(object sender, EventArgs e)
        {
        	AddNewEntry("");
        }
		
        public void RemoveAll()
        {
        	this.ClearAllSubTrees();
        	this.watchList.Rows.Clear();
        }
        
        public void AddExpressions(string[] exprs)
        {
        	foreach (string s in exprs)
        		AddNewEntry(s);
        }
        
        private void mDELETEEXPRToolStripMenuItem_Click(object sender, EventArgs e)
        {
        	try
        	{
        		for (int i=0; i<this.watchList.SelectedRows.Count; i++)
        		{
        			this.watchList.Rows.Remove(this.watchList.SelectedRows[i]);
        		}
        		this.watchList.Update();
        	}
        	catch
        	{
        		
        	}
        }

        private void mCLEARALLToolStripMenuItem_Click(object sender, EventArgs e)
        {
        	this.watchList.Rows.Clear();
        	this.watchList.Update();
        }

        private void mSELECTALLToolStripMenuItem_Click(object sender, EventArgs e)
        {
        	this.watchList.SelectAll();
        }
        
        private void watchList_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
        	if (e.Row is AdvancedDataGridView.TreeGridNode && !(e.Row as AdvancedDataGridView.TreeGridNode).UserRow)
        		e.Cancel = true;
        	else
        	{
        		e.Cancel = false;
        		if (e.Row is AdvancedDataGridView.TreeGridNode && (e.Row as AdvancedDataGridView.TreeGridNode).IsExpanded)
        			ClearRow(e.Row.Index);
        	}
        }
    }
}
