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
    public partial class DebugVariablesListWindowForm : BottomDockContentForm
    {
        private System.Windows.Forms.DataGridViewTextBoxColumn WColumn4;
        private AdvancedDataGridView.TreeGridColumn WColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn WColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn WColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn WColumn5;
		
        private System.Windows.Forms.DataGridViewTextBoxColumn WColumn6Empty;

        public DebugVariablesListWindowForm(Form1 MainForm)
            :base(MainForm)
        {
            InitializeComponent();
            localVarsWindow = this;

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
            this.watchList.MultiSelect = false;
            this.watchList.Font = new Font("Tahoma", 8);
            this.watchList.BorderStyle = BorderStyle.None;
            this.watchList.ReadOnly = true;
            this.watchList.RowHeadersVisible = false;
            this.watchList.RowTemplate.Height = 24;
            this.watchList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.watchList.Size = new System.Drawing.Size(782, 121);
            this.watchList.TabIndex = 0;
            //this.watchList.CellValueChanged += RefreshWatchDel;//delegate(object sender, DataGridViewCellEventArgs e) { RefreshWatch(); };
            //this.watchList.CellEndEdit += delegate(object sender, DataGridViewCellEventArgs e) { RefreshRow(e.RowIndex); };
            //this.watchList.CellClick += CellClickDel;
            //this.watchList.CellDoubleClick += CellDblClickDel;
            this.watchList._imageList = CodeCompletionProvider.ImagesProvider.ImageList;
            this.watchList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.watchList.ShowLines = true;
            //this.watchList.Nodes.Add(new AdvancedDataGridView.TreeGridNode());
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
            this.WColumn1.HeaderText = "CLMN_VAR";
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

            this.watchList.AllowUserToAddRows = false;
            this.watchList.AllowUserToDeleteRows = false;
            this.watchList.AllowUserToResizeColumns = true;
            this.watchList.AllowUserToResizeRows = false;
        }

        public static DebugVariablesListWindowForm LocalVarsWindow
        {
            get
            {
                return localVarsWindow;
            }
        }

        private static DebugVariablesListWindowForm localVarsWindow;

        public void RefreshList()
        {
            this.RefreshWatchInThread();
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
        	this.watchList.Nodes.Clear();
        	this.watchList.Rows.Clear();
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
            node._grid = null;
            node.Nodes.Clear();
            this.watchList.InvalidateRow(i);
        }

        public void RefreshRow(int i)
        {
            if (this.watchList.Rows[i].Cells[0].Value != null)
            {
                if ((this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode).UserRow)
                {
                    RetValue rv = WorkbenchServiceFactory.DebuggerManager.Evaluate(this.watchList.Rows[i].Cells[0].Value as string);
                    if (rv.syn_err)
                    {
                        this.watchList.Rows[i].Cells[1].Value = PascalABCCompiler.StringResources.Get("EXPR_VALUE_SYNTAX_ERROR_IN_EXPR");
                        this.watchList.Rows[i].Cells[2].Value = null;//PascalABCCompiler.StringResources.Get("EXPR_VALUE_UNDEFINED_TYPE");
                    }
                    else
                        if (rv.monoValue != null)
                        {
                            try
                            {
                                ValueItem vi = new ValueItem(rv.monoValue, this.watchList.Rows[i].Cells[0].Value as string, WorkbenchServiceFactory.DebuggerManager.evaluator.declaringType);
                                (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode)._grid = this.watchList;
                                (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode).Content = vi;
                                this.watchList.Rows[i].Cells[1].Value = rv.monoValue.Value;//rv.obj_val.AsString;
                                this.watchList.Rows[i].Cells[2].Value = rv.monoValue.TypeName;
                                this.watchList.InvalidateCell(0, i);
                            }
                            catch (System.Exception e)
                            {
                                this.watchList.Rows[i].Cells[1].Value = PascalABCCompiler.StringResources.Get("EXPR_VALUE_UNDEFINED_VALUE");
                                this.watchList.Rows[i].Cells[2].Value = PascalABCCompiler.StringResources.Get("EXPR_VALUE_UNDEFINED_TYPE");
                                //								  FixedItem fi = new FixedItem(this.watchList.Rows[i].Cells[0].Value as string,PascalABCCompiler.StringResources.Get("EXPR_VALUE_UNDEFINED_VALUE"),PascalABCCompiler.StringResources.Get("EXPR_VALUE_UNDEFINED_TYPE"));
                                //								  fi.imageIndex = CodeCompletionProvider.ImagesProvider.IconNumberEvalError;
                                //								  (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode)._grid = this.watchList;
                                //                            	  (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode).Content = fi;
                            }
                        }
                        else if (rv.prim_val != null)
                        {
                            FixedItem fi = new FixedItem(this.watchList.Rows[i].Cells[0].Value as string, WrapPrimValue(rv.prim_val), DebugUtils.WrapTypeName(rv.prim_val.GetType()));
                            this.watchList.Rows[i].Cells[1].Value = fi.Text;
                            this.watchList.Rows[i].Cells[2].Value = fi.Type;
                            (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode)._grid = this.watchList;
                            (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode).Content = fi;
                        }
                        else
                        {
                            this.watchList.Rows[i].Cells[1].Value = PascalABCCompiler.StringResources.Get("EXPR_VALUE_UNDEFINED_VALUE");
                            this.watchList.Rows[i].Cells[2].Value = PascalABCCompiler.StringResources.Get("EXPR_VALUE_UNDEFINED_TYPE");
                            //                            FixedItem fi = new FixedItem(this.watchList.Rows[i].Cells[0].Value as string,PascalABCCompiler.StringResources.Get("EXPR_VALUE_UNDEFINED_VALUE"),PascalABCCompiler.StringResources.Get("EXPR_VALUE_UNDEFINED_TYPE"));
                            //							fi.imageIndex = CodeCompletionProvider.ImagesProvider.IconNumberEvalError;
                            //							(this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode)._grid = this.watchList;
                            //                            (this.watchList.Rows[i] as AdvancedDataGridView.TreeGridNode).Content = fi;
                            this.ClearRow(i);
                        }
                }
                else
                {

                }
            }
            else
            {
                this.watchList.Rows[i].Cells[1].Value = null;
                this.watchList.Rows[i].Cells[2].Value = null;
            }
        }

        private void RefreshWatchInThread()
        {
            for (int i = 0; i < this.watchList.Rows.Count; i++)
            {
                RefreshRow(i);
            }
        }
    }
}
