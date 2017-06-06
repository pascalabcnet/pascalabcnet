using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace NodesGenerator
{
	/// <summary>
	/// Summary description for template_form.
	/// </summary>
	public class template_form : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox visitor_name;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox file_name;
		private System.Windows.Forms.Button browse;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Button generate_visitor_template;
		private System.Windows.Forms.Button close;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public template_form()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		private NodeGenerator.NodeGenerator node_gen;

		public template_form(NodeGenerator.NodeGenerator ng)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			node_gen=ng;
		}

		public static void generate(NodeGenerator.NodeGenerator ng)
		{
			template_form tf=new template_form(ng);
			tf.ShowDialog();
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
            this.visitor_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.file_name = new System.Windows.Forms.TextBox();
            this.browse = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.generate_visitor_template = new System.Windows.Forms.Button();
            this.close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // visitor_name
            // 
            this.visitor_name.Location = new System.Drawing.Point(10, 28);
            this.visitor_name.Name = "visitor_name";
            this.visitor_name.Size = new System.Drawing.Size(220, 22);
            this.visitor_name.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Visitor name";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "File name";
            // 
            // file_name
            // 
            this.file_name.Location = new System.Drawing.Point(10, 83);
            this.file_name.Name = "file_name";
            this.file_name.Size = new System.Drawing.Size(220, 22);
            this.file_name.TabIndex = 3;
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(240, 83);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(90, 27);
            this.browse.TabIndex = 4;
            this.browse.Text = "Browse";
            this.browse.Click += new System.EventHandler(this.browse_Click);
            // 
            // generate_visitor_template
            // 
            this.generate_visitor_template.Location = new System.Drawing.Point(19, 120);
            this.generate_visitor_template.Name = "generate_visitor_template";
            this.generate_visitor_template.Size = new System.Drawing.Size(183, 46);
            this.generate_visitor_template.TabIndex = 5;
            this.generate_visitor_template.Text = "Generate visitor template";
            this.generate_visitor_template.Click += new System.EventHandler(this.button1_Click);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(221, 129);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(115, 37);
            this.close.TabIndex = 6;
            this.close.Text = "Close";
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // template_form
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(347, 183);
            this.Controls.Add(this.close);
            this.Controls.Add(this.generate_visitor_template);
            this.Controls.Add(this.browse);
            this.Controls.Add(this.file_name);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.visitor_name);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "template_form";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Template generator";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void browse_Click(object sender, System.EventArgs e)
		{
			if (openFileDialog1.ShowDialog()==DialogResult.OK)
			{
				file_name.Text=openFileDialog1.FileName;
			}
		}

		private void close_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			node_gen.generate_visitor_template(visitor_name.Text,file_name.Text);
		}
	}
}
