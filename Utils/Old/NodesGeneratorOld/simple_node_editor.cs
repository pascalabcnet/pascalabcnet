using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using NodeGenerator;

namespace NodesGenerator
{
	/// <summary>
	/// Summary description for simple_node_editor.
	/// </summary>
	public class simple_node_editor : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox field_name;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox field_type;
		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.Button ok;
		private System.Windows.Forms.CheckBox create_var;
		private System.Windows.Forms.CheckBox delete_var;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox help_context;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public simple_node_editor()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		private extended_simple_element se;
		private static extended_simple_element sse;
		private NodeGenerator.NodeGenerator ng;
		private NodeGenerator.node_info ninf;

		public simple_node_editor(simple_element se,node_info ninf,NodeGenerator.NodeGenerator _ng)
		{
			InitializeComponent();
			if (se!=null)
			{
				extended_simple_element ese=se as extended_simple_element;
				if (ese!=null)
				{
					this.se=ese;
				}
				else
				{
					this.se=new extended_simple_element();
					this.se.val_field_type_name=se.val_field_type_name;
					this.se.field_name=se.field_name;
					this.se.field_type=se.field_type;
				}
			}
			if (this.se==null)
			{
				this.se=new extended_simple_element();
			}
			this.ninf=ninf;
			this.ng=_ng;

			set_element();
		}

		private string help_context_name()
		{
			return ninf.node_name+"."+se.field_name;
		}

		private void set_element()
		{
			this.field_name.Text=se.field_name;
			this.field_type.Text=se.field_type_name;
			this.create_var.Checked=se.create_var;
			this.delete_var.Checked=se.delete_var;
			this.help_context.Text=ng.help_storage.get_help_context(help_context_name()).help_context;
		}

		private void get_element()
		{
			se.field_name=field_name.Text;
			se.val_field_type_name=field_type.Text;
			se.create_var=this.create_var.Checked;
			se.delete_var=this.delete_var.Checked;
			ng.help_storage.get_help_context(help_context_name()).help_context=help_context.Text;
		}

		public static simple_element edit_element(simple_element se,node_info ninf,NodeGenerator.NodeGenerator _ng)
		{
			simple_node_editor sne=new simple_node_editor(se,ninf,_ng);
			sne.ShowDialog();
			sse=sne.se;
			return sse;
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
            this.field_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.field_type = new System.Windows.Forms.TextBox();
            this.cancel = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.create_var = new System.Windows.Forms.CheckBox();
            this.delete_var = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.help_context = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // field_name
            // 
            this.field_name.Location = new System.Drawing.Point(19, 37);
            this.field_name.Name = "field_name";
            this.field_name.Size = new System.Drawing.Size(269, 22);
            this.field_name.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Field name";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(19, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Field type";
            // 
            // field_type
            // 
            this.field_type.Location = new System.Drawing.Point(19, 102);
            this.field_type.Name = "field_type";
            this.field_type.Size = new System.Drawing.Size(269, 22);
            this.field_type.TabIndex = 3;
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(192, 166);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(90, 27);
            this.cancel.TabIndex = 4;
            this.cancel.Text = "Cancel";
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(67, 166);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(90, 27);
            this.ok.TabIndex = 5;
            this.ok.Text = "OK";
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // create_var
            // 
            this.create_var.Location = new System.Drawing.Point(19, 138);
            this.create_var.Name = "create_var";
            this.create_var.Size = new System.Drawing.Size(125, 19);
            this.create_var.TabIndex = 6;
            this.create_var.Text = "create variable";
            // 
            // delete_var
            // 
            this.delete_var.Location = new System.Drawing.Point(163, 138);
            this.delete_var.Name = "delete_var";
            this.delete_var.Size = new System.Drawing.Size(125, 19);
            this.delete_var.TabIndex = 7;
            this.delete_var.Text = "delete variable";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 203);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 19);
            this.label3.TabIndex = 8;
            this.label3.Text = "Help context";
            // 
            // help_context
            // 
            this.help_context.Location = new System.Drawing.Point(10, 231);
            this.help_context.Multiline = true;
            this.help_context.Name = "help_context";
            this.help_context.Size = new System.Drawing.Size(288, 83);
            this.help_context.TabIndex = 9;
            // 
            // simple_node_editor
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(310, 332);
            this.Controls.Add(this.help_context);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.delete_var);
            this.Controls.Add(this.create_var);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.field_type);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.field_name);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "simple_node_editor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактор узлов";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void ok_Click(object sender, System.EventArgs e)
		{
			get_element();
			//sse=se;
			Close();
		}
	}
}
