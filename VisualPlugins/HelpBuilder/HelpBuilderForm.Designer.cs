
namespace VisualPascalABCPlugins
{
	partial class HelpBuilderForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpBuilderForm));
			this.lbFiles = new System.Windows.Forms.ListBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsbNew = new System.Windows.Forms.ToolStripButton();
			this.tsbOpen = new System.Windows.Forms.ToolStripButton();
			this.tsbSave = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbRun = new System.Windows.Forms.ToolStripButton();
			this.tsbBuild = new System.Windows.Forms.ToolStripButton();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnAddCurrent = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
			this.toolStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbFiles
			// 
			this.lbFiles.FormattingEnabled = true;
			this.lbFiles.Location = new System.Drawing.Point(6, 23);
			this.lbFiles.Name = "lbFiles";
			this.lbFiles.Size = new System.Drawing.Size(207, 160);
			this.lbFiles.TabIndex = 0;
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(222, 23);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(124, 23);
			this.btnAdd.TabIndex = 2;
			this.btnAdd.Text = "ADD_FILES";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAddClick);
			// 
			// btnRemove
			// 
			this.btnRemove.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnRemove.Location = new System.Drawing.Point(222, 122);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(124, 23);
			this.btnRemove.TabIndex = 3;
			this.btnRemove.Text = "REMOVE";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.BtnRemoveClick);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.tsbNew,
									this.tsbOpen,
									this.tsbSave,
									this.toolStripSeparator1,
									this.tsbRun,
									this.tsbBuild});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(358, 25);
			this.toolStrip1.TabIndex = 5;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// tsbNew
			// 
			this.tsbNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbNew.Image = ((System.Drawing.Image)(resources.GetObject("tsbNew.Image")));
			this.tsbNew.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbNew.Name = "tsbNew";
			this.tsbNew.Size = new System.Drawing.Size(23, 22);
			this.tsbNew.Text = "NEW";
			this.tsbNew.Click += new System.EventHandler(this.TsbNewClick);
			// 
			// tsbOpen
			// 
			this.tsbOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbOpen.Image = ((System.Drawing.Image)(resources.GetObject("tsbOpen.Image")));
			this.tsbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbOpen.Name = "tsbOpen";
			this.tsbOpen.Size = new System.Drawing.Size(23, 22);
			this.tsbOpen.Text = "OPEN";
			this.tsbOpen.Click += new System.EventHandler(this.TsbOpenClick);
			// 
			// tsbSave
			// 
			this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbSave.Image = ((System.Drawing.Image)(resources.GetObject("tsbSave.Image")));
			this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbSave.Name = "tsbSave";
			this.tsbSave.Size = new System.Drawing.Size(23, 22);
			this.tsbSave.Text = "SAVE";
			this.tsbSave.Click += new System.EventHandler(this.TsbSaveClick);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// tsbRun
			// 
			this.tsbRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbRun.Image = ((System.Drawing.Image)(resources.GetObject("tsbRun.Image")));
			this.tsbRun.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbRun.Name = "tsbRun";
			this.tsbRun.Size = new System.Drawing.Size(23, 22);
			this.tsbRun.Text = "RUN";
			this.tsbRun.Click += new System.EventHandler(this.ToolStripButton1Click);
			// 
			// tsbBuild
			// 
			this.tsbBuild.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbBuild.Image = ((System.Drawing.Image)(resources.GetObject("tsbBuild.Image")));
			this.tsbBuild.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbBuild.Name = "tsbBuild";
			this.tsbBuild.Size = new System.Drawing.Size(23, 22);
			this.tsbBuild.Text = "BUILD";
			this.tsbBuild.Click += new System.EventHandler(this.TsbBuildClick);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(304, 189);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(32, 32);
			this.pictureBox1.TabIndex = 7;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Visible = false;
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			this.openFileDialog1.Multiselect = true;
			this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenFileDialog1FileOk);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnAddCurrent);
			this.groupBox1.Controls.Add(this.lbFiles);
			this.groupBox1.Controls.Add(this.btnAdd);
			this.groupBox1.Controls.Add(this.btnRemove);
			this.groupBox1.Location = new System.Drawing.Point(0, 28);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(358, 193);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "FILES";
			// 
			// btnAddCurrent
			// 
			this.btnAddCurrent.Location = new System.Drawing.Point(222, 71);
			this.btnAddCurrent.Name = "btnAddCurrent";
			this.btnAddCurrent.Size = new System.Drawing.Size(124, 23);
			this.btnAddCurrent.TabIndex = 4;
			this.btnAddCurrent.Text = "ADD_CURRENT_FILE";
			this.btnAddCurrent.UseVisualStyleBackColor = true;
			this.btnAddCurrent.Click += new System.EventHandler(this.BtnAddCurrentClick);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.propertyGrid1);
			this.groupBox2.Location = new System.Drawing.Point(0, 227);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(358, 257);
			this.groupBox2.TabIndex = 9;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "OPTIONS";
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid1.Location = new System.Drawing.Point(3, 16);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(352, 238);
			this.propertyGrid1.TabIndex = 0;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 486);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(358, 22);
			this.statusStrip1.TabIndex = 10;
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.Filter = "hbp projects|*.hbp";
			// 
			// openFileDialog2
			// 
			this.openFileDialog2.FileName = "openFileDialog2";
			this.openFileDialog2.Filter = "hbp projects|*.hbp";
			// 
			// HelpBuilderForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(358, 508);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.ForeColor = System.Drawing.SystemColors.ControlText;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "HelpBuilderForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FORM_NAME";
			this.TopMost = true;
			this.Shown += new System.EventHandler(this.HelpBuilderFormShown);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HelpBuilderFormFormClosing);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button btnAddCurrent;
		private System.Windows.Forms.OpenFileDialog openFileDialog2;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripButton tsbSave;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton tsbOpen;
		private System.Windows.Forms.ToolStripButton tsbNew;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ToolStripButton tsbRun;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.ToolStripButton tsbBuild;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.ListBox lbFiles;
	}
}
