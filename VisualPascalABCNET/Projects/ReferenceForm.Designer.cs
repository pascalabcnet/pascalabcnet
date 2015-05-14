
namespace VisualPascalABC
{
	partial class ReferenceForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpGAC = new System.Windows.Forms.TabPage();
            this.lvGac = new System.Windows.Forms.ListView();
            this.chComponentName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpAssemblies = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.tpGAC.SuspendLayout();
            this.tpAssemblies.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpGAC);
            this.tabControl1.Controls.Add(this.tpAssemblies);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(448, 258);
            this.tabControl1.TabIndex = 0;
            // 
            // tpGAC
            // 
            this.tpGAC.Controls.Add(this.lvGac);
            this.tpGAC.Location = new System.Drawing.Point(4, 22);
            this.tpGAC.Name = "tpGAC";
            this.tpGAC.Padding = new System.Windows.Forms.Padding(3);
            this.tpGAC.Size = new System.Drawing.Size(440, 232);
            this.tpGAC.TabIndex = 0;
            this.tpGAC.Text = "PRJ_GAC";
            this.tpGAC.UseVisualStyleBackColor = true;
            // 
            // lvGac
            // 
            this.lvGac.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chComponentName,
            this.chVersion});
            this.lvGac.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvGac.FullRowSelect = true;
            this.lvGac.Location = new System.Drawing.Point(3, 3);
            this.lvGac.Name = "lvGac";
            this.lvGac.Size = new System.Drawing.Size(434, 226);
            this.lvGac.TabIndex = 0;
            this.lvGac.UseCompatibleStateImageBehavior = false;
            this.lvGac.View = System.Windows.Forms.View.Details;
            // 
            // chComponentName
            // 
            this.chComponentName.Text = "PRJ_COMPONENT_NAME";
            this.chComponentName.Width = 314;
            // 
            // chVersion
            // 
            this.chVersion.Text = "PRJ_VERSION";
            this.chVersion.Width = 103;
            // 
            // tpAssemblies
            // 
            this.tpAssemblies.Controls.Add(this.button1);
            this.tpAssemblies.Location = new System.Drawing.Point(4, 22);
            this.tpAssemblies.Name = "tpAssemblies";
            this.tpAssemblies.Padding = new System.Windows.Forms.Padding(3);
            this.tpAssemblies.Size = new System.Drawing.Size(440, 232);
            this.tpAssemblies.TabIndex = 1;
            this.tpAssemblies.Text = "PRJ_ASSEMBLIES";
            this.tpAssemblies.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(19, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "PRJ_SEARCH";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(277, 264);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(79, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "RF_OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(373, 264);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "RF_CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenFileDialog1FileOk);
            // 
            // ReferenceForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(453, 292);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReferenceForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PRJ_REFERENCES";
            this.tabControl1.ResumeLayout(false);
            this.tpGAC.ResumeLayout(false);
            this.tpAssemblies.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ColumnHeader chVersion;
		private System.Windows.Forms.ColumnHeader chComponentName;
		private System.Windows.Forms.ListView lvGac;
		private System.Windows.Forms.TabPage tpAssemblies;
		private System.Windows.Forms.TabPage tpGAC;
		private System.Windows.Forms.TabControl tabControl1;
	}
}
