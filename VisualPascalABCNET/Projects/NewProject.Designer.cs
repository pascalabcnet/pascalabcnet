
namespace VisualPascalABC
{
	partial class NewProjectForm
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("PRJ_WINFORMS_APP", 1);
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("PRJ_CONSOLE_APP", 2);
			System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("PRJ_CLASS_LIBRARY", 3);
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewProjectForm));
			this.lvTemplates = new System.Windows.Forms.ListView();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.tbProjectName = new System.Windows.Forms.TextBox();
			this.tbProjectDir = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnProjectDirSelect = new System.Windows.Forms.Button();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.SuspendLayout();
			// 
			// lvTemplates
			// 
			this.lvTemplates.HideSelection = false;
			listViewItem1.StateImageIndex = 0;
			this.lvTemplates.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
									listViewItem1,
									listViewItem2,
									listViewItem3});
			this.lvTemplates.LargeImageList = this.imageList1;
			this.lvTemplates.Location = new System.Drawing.Point(1, 39);
			this.lvTemplates.MultiSelect = false;
			this.lvTemplates.Name = "lvTemplates";
			this.lvTemplates.Size = new System.Drawing.Size(425, 222);
			this.lvTemplates.TabIndex = 0;
			this.lvTemplates.UseCompatibleStateImageBehavior = false;
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "pas.ico");
			this.imageList1.Images.SetKeyName(1, "Icons.32x32.PABCProject.WinForms.png");
			this.imageList1.Images.SetKeyName(2, "Icons.32x32.PABCProject.ConsoleSimple.png");
			this.imageList1.Images.SetKeyName(3, "Icons.32x32.PABCProject.Library.png");
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(1, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(137, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "PRJ_PROJECT_TYPES";
			// 
			// tbProjectName
			// 
			this.tbProjectName.Location = new System.Drawing.Point(114, 288);
			this.tbProjectName.Name = "tbProjectName";
			this.tbProjectName.Size = new System.Drawing.Size(312, 20);
			this.tbProjectName.TabIndex = 1;
			this.tbProjectName.TextChanged += new System.EventHandler(this.TbProjectNameTextChanged);
			// 
			// tbProjectDir
			// 
			this.tbProjectDir.Location = new System.Drawing.Point(114, 314);
			this.tbProjectDir.Name = "tbProjectDir";
			this.tbProjectDir.Size = new System.Drawing.Size(280, 20);
			this.tbProjectDir.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 288);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 23);
			this.label2.TabIndex = 5;
			this.label2.Text = "PRJ_PROJECT_NAME";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 314);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 23);
			this.label3.TabIndex = 6;
			this.label3.Text = "PRJ_PROJECT_DIR";
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(243, 346);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(82, 23);
			this.btnOk.TabIndex = 7;
			this.btnOk.Text = "RF_OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.BtnOkClick);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(346, 346);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 23);
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "RF_CANCEL";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnProjectDirSelect
			// 
			this.btnProjectDirSelect.Location = new System.Drawing.Point(402, 314);
			this.btnProjectDirSelect.Name = "btnProjectDirSelect";
			this.btnProjectDirSelect.Size = new System.Drawing.Size(24, 20);
			this.btnProjectDirSelect.TabIndex = 9;
			this.btnProjectDirSelect.Text = "...";
			this.btnProjectDirSelect.UseVisualStyleBackColor = true;
			this.btnProjectDirSelect.Click += new System.EventHandler(this.BtnProjectDirSelectClick);
			// 
			// NewProjectForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(434, 381);
			this.Controls.Add(this.btnProjectDirSelect);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.tbProjectDir);
			this.Controls.Add(this.tbProjectName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lvTemplates);
			this.Controls.Add(this.label2);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewProjectForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "PRJ_NEW_PROJECT";
			this.Activated += new System.EventHandler(this.NewProjectFormActivated);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewProjectFormFormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Button btnProjectDirSelect;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbProjectDir;
		private System.Windows.Forms.TextBox tbProjectName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView lvTemplates;
	}
}
