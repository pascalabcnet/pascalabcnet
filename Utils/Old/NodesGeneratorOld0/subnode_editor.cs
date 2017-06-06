using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using NodeGenerator;

namespace NodesGenerator
{
	/// <summary>
	/// Summary description for subnode_editor.
	/// </summary>
	public class subnode_editor : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox subnodes_types;
		private System.Windows.Forms.TextBox field_name;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.Button ok;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox help_context;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public subnode_editor()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		private NodeGenerator.NodeGenerator ngen;
		private node_field_info nfi;
		private static node_field_info snfi;
		private node_info ninf;

		public subnode_editor(NodeGenerator.NodeGenerator ng,node_field_info nfi,node_info this_node)
		{
			InitializeComponent();
			ngen=ng;
			this.nfi=nfi;

			subnodes_types.Items.Clear();
			subnodes_types.Items.AddRange((object[])ngen.all_nodes.ToArray(typeof(object)));

			if (ngen.all_nodes.IndexOf(this_node)<0)
			{
				subnodes_types.Items.Insert(0,this_node);
			}

			if (this.nfi==null)
			{
				this.nfi=new node_field_info();
				this.nfi.field_type=(node_info)ng.all_nodes[0];
			}

			ninf=this_node;
			snfi=this.nfi;
			set_node_info();
		}

		private void set_node_info()
		{
			if (nfi==null)
			{
				return;
			}

			field_name.Text=nfi.field_name;
			
			int t=subnodes_types.Items.IndexOf(nfi.field_type);
			if (t<0)
			{
				return;
			}

			subnodes_types.SelectedIndex=t;

			this.help_context.Text=ngen.help_storage.get_help_context(ninf.node_name+"."+nfi.field_name).help_context;

		}

		private void get_node_info()
		{
			if (nfi==null)
			{
				nfi=new node_field_info();
			}
			nfi.field_name=field_name.Text;
			
			int t=subnodes_types.SelectedIndex;
			if (t<0)
			{
				return;
			}
			nfi.field_type=(node_info)subnodes_types.Items[t];

			ngen.help_storage.get_help_context(ninf.node_name+"."+nfi.field_name).help_context=this.help_context.Text;

		}

		public static node_field_info edit_field(node_field_info nfi,NodeGenerator.NodeGenerator ng,node_info this_node)
		{
			subnode_editor se = new subnode_editor(ng,nfi,this_node);
			if (se.ShowDialog() == DialogResult.Cancel)
			{
                snfi.field_name = "";
			}
			return snfi;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.subnodes_types = new System.Windows.Forms.ListBox();
            this.field_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cancel = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.help_context = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // subnodes_types
            // 
            this.subnodes_types.ItemHeight = 16;
            this.subnodes_types.Location = new System.Drawing.Point(19, 102);
            this.subnodes_types.Name = "subnodes_types";
            this.subnodes_types.Size = new System.Drawing.Size(231, 324);
            this.subnodes_types.TabIndex = 0;
            // 
            // field_name
            // 
            this.field_name.Location = new System.Drawing.Point(19, 37);
            this.field_name.Name = "field_name";
            this.field_name.Size = new System.Drawing.Size(231, 22);
            this.field_name.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Field name";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(29, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Field type";
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(154, 582);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(90, 26);
            this.cancel.TabIndex = 4;
            this.cancel.Text = "Cancel";
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // ok
            // 
            this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok.Location = new System.Drawing.Point(19, 582);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(90, 26);
            this.ok.TabIndex = 5;
            this.ok.Text = "OK";
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 462);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 18);
            this.label3.TabIndex = 6;
            this.label3.Text = "Help context";
            // 
            // help_context
            // 
            this.help_context.Location = new System.Drawing.Point(10, 480);
            this.help_context.Multiline = true;
            this.help_context.Name = "help_context";
            this.help_context.Size = new System.Drawing.Size(249, 92);
            this.help_context.TabIndex = 7;
            // 
            // subnode_editor
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(274, 618);
            this.Controls.Add(this.help_context);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.field_name);
            this.Controls.Add(this.subnodes_types);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "subnode_editor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Subnode editor";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void ok_Click(object sender, System.EventArgs e)
		{
			get_node_info();
			snfi=nfi;
		}

        private void cancel_Click(object sender, EventArgs e)
        {

        }
	}
}
