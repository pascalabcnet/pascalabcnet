using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using NodeGenerator;

namespace NodesGenerator
{
	/// <summary>
	/// Summary description for node_def.
	/// </summary>
	public class node_def : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ListBox subnodes;
		private System.Windows.Forms.Button delete;
		private System.Windows.Forms.Button add_subnode;
		private System.Windows.Forms.Button add_variable;
		private System.Windows.Forms.TextBox node_name;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.Button ok;
		private System.Windows.Forms.ListBox base_class;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox help_context;
		private System.Windows.Forms.Label label3;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public node_def()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		private node_info ni;
		private static node_info sni;
		private NodeGenerator.NodeGenerator ng;

		public node_def(node_info ni,NodeGenerator.NodeGenerator ng)
		{
			InitializeComponent();
			this.ni=ni;
			sni=ni;
			
			base_class.Items.Clear();
			base_class.Items.AddRange((object[])ng.all_nodes.ToArray(typeof(object)));

			if (this.ni==null)
			{
				this.ni=new node_info();
			}
			this.ng=ng;
			set_node_info();
		}

		private void set_node_info()
		{
			this.node_name.Text=ni.node_name;
			this.subnodes.Items.Clear();
			this.subnodes.Items.AddRange(ni.subnodes);
			
			if (ni.node_name!=null)
			{
				if (ng.help_storage.get_help_context(ni.node_name)==null)
				{
					ng.help_storage.add_context(ni.node_name,new HelpContext());
				}
				this.help_context.Text=ng.help_storage.get_help_context(ni.node_name).help_context;
			}

			if (ni.base_class==null)
			{
				return;
			}
			int t=base_class.Items.IndexOf(ni.base_class);
			if (t<0)
			{
				return;
			}
			base_class.SelectedIndex=t;

		}

		public static node_info show_modal(node_info ni,NodeGenerator.NodeGenerator ng)
		{
			node_def nd=new node_def(ni,ng);
			nd.ShowDialog();
			return sni;
		}

		private void get_node_info()
		{
			ni.node_name=node_name.Text;
			ni.set_subnodes(subnodes.Items);

			ng.help_storage.get_help_context(ni.node_name).help_context=this.help_context.Text;

			int t=base_class.SelectedIndex;
			if (t<0)
			{
				return;
			}
			ni.base_class=base_class.Items[t] as node_info;

		}

		private void delete_item()
		{
			int t=subnodes.SelectedIndex;
			if (t<0)
			{
				return;
			}
			subnodes.Items.RemoveAt(t);
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.add_variable = new System.Windows.Forms.Button();
            this.delete = new System.Windows.Forms.Button();
            this.add_subnode = new System.Windows.Forms.Button();
            this.subnodes = new System.Windows.Forms.ListBox();
            this.node_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cancel = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.base_class = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.help_context = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.add_variable);
            this.panel1.Controls.Add(this.delete);
            this.panel1.Controls.Add(this.add_subnode);
            this.panel1.Controls.Add(this.subnodes);
            this.panel1.Location = new System.Drawing.Point(10, 92);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(422, 379);
            this.panel1.TabIndex = 0;
            // 
            // add_variable
            // 
            this.add_variable.Location = new System.Drawing.Point(317, 55);
            this.add_variable.Name = "add_variable";
            this.add_variable.Size = new System.Drawing.Size(96, 27);
            this.add_variable.TabIndex = 3;
            this.add_variable.Text = "Add variable";
            this.add_variable.Click += new System.EventHandler(this.add_variable_Click);
            // 
            // delete
            // 
            this.delete.Location = new System.Drawing.Point(317, 92);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(96, 27);
            this.delete.TabIndex = 2;
            this.delete.Text = "Delete";
            this.delete.Click += new System.EventHandler(this.delete_Click);
            // 
            // add_subnode
            // 
            this.add_subnode.Location = new System.Drawing.Point(317, 18);
            this.add_subnode.Name = "add_subnode";
            this.add_subnode.Size = new System.Drawing.Size(96, 27);
            this.add_subnode.TabIndex = 1;
            this.add_subnode.Text = "Add subnode";
            this.add_subnode.Click += new System.EventHandler(this.add_subnode_Click);
            // 
            // subnodes
            // 
            this.subnodes.ItemHeight = 16;
            this.subnodes.Location = new System.Drawing.Point(10, 9);
            this.subnodes.Name = "subnodes";
            this.subnodes.Size = new System.Drawing.Size(288, 324);
            this.subnodes.TabIndex = 0;
            this.subnodes.DoubleClick += new System.EventHandler(this.subnodes_DoubleClick);
            // 
            // node_name
            // 
            this.node_name.Location = new System.Drawing.Point(10, 55);
            this.node_name.Name = "node_name";
            this.node_name.Size = new System.Drawing.Size(422, 22);
            this.node_name.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Имя узла";
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(538, 618);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(90, 27);
            this.cancel.TabIndex = 3;
            this.cancel.Text = "Cancel";
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(432, 618);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(90, 27);
            this.ok.TabIndex = 4;
            this.ok.Text = "OK";
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // base_class
            // 
            this.base_class.ItemHeight = 16;
            this.base_class.Location = new System.Drawing.Point(442, 46);
            this.base_class.Name = "base_class";
            this.base_class.Size = new System.Drawing.Size(182, 404);
            this.base_class.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(442, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 19);
            this.label2.TabIndex = 6;
            this.label2.Text = "Базовый класс";
            // 
            // help_context
            // 
            this.help_context.Location = new System.Drawing.Point(10, 498);
            this.help_context.Multiline = true;
            this.help_context.Name = "help_context";
            this.help_context.Size = new System.Drawing.Size(624, 111);
            this.help_context.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(19, 480);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 18);
            this.label3.TabIndex = 8;
            this.label3.Text = "Help context";
            // 
            // node_def
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(638, 616);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.help_context);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.base_class);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.node_name);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "node_def";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Определение узла";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void ok_Click(object sender, System.EventArgs e)
		{
			get_node_info();
			sni=ni;
			Close();
		}

		private void add_subnode_Click(object sender, System.EventArgs e)
		{
			get_node_info();
			node_field_info nfi=subnode_editor.edit_field(null,ng,ni);
			if (nfi!=null)
			{
				subnodes.Items.Add(nfi);
			}
		}

		private void add_variable_Click(object sender, System.EventArgs e)
		{
			simple_element se=simple_node_editor.edit_element(null,ni,ng);
			if (se!=null)
			{
				subnodes.Items.Add(se);
			}
		}

		private void delete_Click(object sender, System.EventArgs e)
		{
			delete_item();
		}

		private void subnodes_DoubleClick(object sender, System.EventArgs e)
		{
			int t=subnodes.SelectedIndex;
			if (t<0)
			{
				return;
			}
			simple_element se=subnodes.Items[t] as simple_element;
			if (se!=null)
			{
				subnodes.Items[t]=simple_node_editor.edit_element(se,ni,ng);
				return;
			}
			node_field_info nfi=subnodes.Items[t] as node_field_info;
			if (nfi!=null)
			{
				subnode_editor.edit_field(nfi,ng,ni);
				return;
			}
			return;
		}
	}
}
