
namespace VisualPascalABC
{
	partial class ProjectExplorerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectExplorerForm));
            this.tvProjectExplorer = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.AddReferenceMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pRJADDREFERENCEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SouceFileMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pRJOPENToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pRJToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pRJVIEWCODEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pRJEXCLUDEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pRJRENAMEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ProjectMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pRJBUILDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pRJBUILDALLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pRJRUNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.pRJToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pRJNEWFILEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pRJEXISTFILEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pRJFORMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pRJToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.pRJPROPERTIESToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReferenceMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pRJREMOVEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.AddReferenceMenuStrip.SuspendLayout();
            this.SouceFileMenuStrip.SuspendLayout();
            this.ProjectMenuStrip.SuspendLayout();
            this.ReferenceMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvProjectExplorer
            // 
            this.tvProjectExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvProjectExplorer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvProjectExplorer.ImageIndex = 0;
            this.tvProjectExplorer.ImageList = this.imageList1;
            this.tvProjectExplorer.Location = new System.Drawing.Point(0, 0);
            this.tvProjectExplorer.Name = "tvProjectExplorer";
            this.tvProjectExplorer.SelectedImageIndex = 0;
            this.tvProjectExplorer.Size = new System.Drawing.Size(209, 332);
            this.tvProjectExplorer.StateImageList = this.imageList1;
            this.tvProjectExplorer.TabIndex = 0;
            this.tvProjectExplorer.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.TvProjectExplorerBeforeSelect);
            this.tvProjectExplorer.DoubleClick += new System.EventHandler(this.TvProjectExplorerDoubleClick);
            this.tvProjectExplorer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TvProjectExplorerMouseClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ProjectBrowser.ReferenceFolder.Closed.png");
            this.imageList1.Images.SetKeyName(1, "ProjectBrowser.ReferenceFolder.Open.png");
            this.imageList1.Images.SetKeyName(2, "Icons.16x16.Project.png");
            this.imageList1.Images.SetKeyName(3, "Icons.16x16.Reference.png");
            this.imageList1.Images.SetKeyName(4, "pabcfile.png");
            this.imageList1.Images.SetKeyName(5, "Icons.16x16.Form.png");
            // 
            // AddReferenceMenuStrip
            // 
            this.AddReferenceMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pRJADDREFERENCEToolStripMenuItem});
            this.AddReferenceMenuStrip.Name = "AddReferenceMenuStrip";
            this.AddReferenceMenuStrip.Size = new System.Drawing.Size(188, 26);
            // 
            // pRJADDREFERENCEToolStripMenuItem
            // 
            this.pRJADDREFERENCEToolStripMenuItem.Name = "pRJADDREFERENCEToolStripMenuItem";
            this.pRJADDREFERENCEToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.pRJADDREFERENCEToolStripMenuItem.Text = "PRJ_ADD_REFERENCE";
            this.pRJADDREFERENCEToolStripMenuItem.Click += new System.EventHandler(this.PRJADDREFERENCEToolStripMenuItemClick);
            // 
            // SouceFileMenuStrip
            // 
            this.SouceFileMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pRJOPENToolStripMenuItem,
            this.pRJToolStripMenuItem,
            this.pRJVIEWCODEToolStripMenuItem,
            this.pRJEXCLUDEToolStripMenuItem,
            this.pRJRENAMEToolStripMenuItem});
            this.SouceFileMenuStrip.Name = "SouceFileMenuStrip";
            this.SouceFileMenuStrip.Size = new System.Drawing.Size(171, 114);
            // 
            // pRJOPENToolStripMenuItem
            // 
            this.pRJOPENToolStripMenuItem.Name = "pRJOPENToolStripMenuItem";
            this.pRJOPENToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.pRJOPENToolStripMenuItem.Text = "PRJ_OPEN_FILE";
            this.pRJOPENToolStripMenuItem.Click += new System.EventHandler(this.pRJOPENToolStripMenuItem_Click);
            // 
            // pRJToolStripMenuItem
            // 
            this.pRJToolStripMenuItem.Name = "pRJToolStripMenuItem";
            this.pRJToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.pRJToolStripMenuItem.Text = "PRJ_SHOW_FORM";
            this.pRJToolStripMenuItem.Visible = false;
            this.pRJToolStripMenuItem.Click += new System.EventHandler(this.pRJToolStripMenuItem_Click);
            // 
            // pRJVIEWCODEToolStripMenuItem
            // 
            this.pRJVIEWCODEToolStripMenuItem.Name = "pRJVIEWCODEToolStripMenuItem";
            this.pRJVIEWCODEToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.pRJVIEWCODEToolStripMenuItem.Text = "PRJ_VIEW_CODE";
            this.pRJVIEWCODEToolStripMenuItem.Click += new System.EventHandler(this.pRJVIEWCODEToolStripMenuItem_Click);
            // 
            // pRJEXCLUDEToolStripMenuItem
            // 
            this.pRJEXCLUDEToolStripMenuItem.Name = "pRJEXCLUDEToolStripMenuItem";
            this.pRJEXCLUDEToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.pRJEXCLUDEToolStripMenuItem.Text = "PRJ_EXCLUDE";
            this.pRJEXCLUDEToolStripMenuItem.Click += new System.EventHandler(this.pRJEXCLUDEToolStripMenuItem_Click);
            // 
            // pRJRENAMEToolStripMenuItem
            // 
            this.pRJRENAMEToolStripMenuItem.Name = "pRJRENAMEToolStripMenuItem";
            this.pRJRENAMEToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.pRJRENAMEToolStripMenuItem.Text = "PRJ_RENAME";
            this.pRJRENAMEToolStripMenuItem.Click += new System.EventHandler(this.pRJRENAMEToolStripMenuItem_Click);
            // 
            // ProjectMenuStrip
            // 
            this.ProjectMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pRJBUILDToolStripMenuItem,
            this.pRJBUILDALLToolStripMenuItem,
            this.pRJRUNToolStripMenuItem,
            this.toolStripMenuItem1,
            this.pRJToolStripMenuItem1,
            this.pRJToolStripMenuItem2,
            this.toolStripMenuItem2,
            this.pRJPROPERTIESToolStripMenuItem});
            this.ProjectMenuStrip.Name = "ProjectMenuStrip";
            this.ProjectMenuStrip.Size = new System.Drawing.Size(188, 148);
            // 
            // pRJBUILDToolStripMenuItem
            // 
            this.pRJBUILDToolStripMenuItem.Name = "pRJBUILDToolStripMenuItem";
            this.pRJBUILDToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.pRJBUILDToolStripMenuItem.Text = "PRJ_BUILD";
            this.pRJBUILDToolStripMenuItem.Click += new System.EventHandler(this.pRJBUILDToolStripMenuItem_Click);
            // 
            // pRJBUILDALLToolStripMenuItem
            // 
            this.pRJBUILDALLToolStripMenuItem.Name = "pRJBUILDALLToolStripMenuItem";
            this.pRJBUILDALLToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.pRJBUILDALLToolStripMenuItem.Text = "PRJ_BUILD_ALL";
            this.pRJBUILDALLToolStripMenuItem.Click += new System.EventHandler(this.pRJBUILDALLToolStripMenuItem_Click);
            // 
            // pRJRUNToolStripMenuItem
            // 
            this.pRJRUNToolStripMenuItem.Name = "pRJRUNToolStripMenuItem";
            this.pRJRUNToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.pRJRUNToolStripMenuItem.Text = "PRJ_RUN";
            this.pRJRUNToolStripMenuItem.Click += new System.EventHandler(this.pRJRUNToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(184, 6);
            // 
            // pRJToolStripMenuItem1
            // 
            this.pRJToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pRJNEWFILEToolStripMenuItem,
            this.pRJEXISTFILEToolStripMenuItem,
            this.pRJFORMToolStripMenuItem});
            this.pRJToolStripMenuItem1.Name = "pRJToolStripMenuItem1";
            this.pRJToolStripMenuItem1.Size = new System.Drawing.Size(187, 22);
            this.pRJToolStripMenuItem1.Text = "PRJ_ADD";
            // 
            // pRJNEWFILEToolStripMenuItem
            // 
            this.pRJNEWFILEToolStripMenuItem.Name = "pRJNEWFILEToolStripMenuItem";
            this.pRJNEWFILEToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pRJNEWFILEToolStripMenuItem.Text = "PRJ_NEW_FILE";
            this.pRJNEWFILEToolStripMenuItem.Click += new System.EventHandler(this.pRJNEWFILEToolStripMenuItem_Click);
            // 
            // pRJEXISTFILEToolStripMenuItem
            // 
            this.pRJEXISTFILEToolStripMenuItem.Name = "pRJEXISTFILEToolStripMenuItem";
            this.pRJEXISTFILEToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pRJEXISTFILEToolStripMenuItem.Text = "PRJ_EXIST_FILE";
            this.pRJEXISTFILEToolStripMenuItem.Click += new System.EventHandler(this.pRJEXISTFILEToolStripMenuItem_Click);
            // 
            // pRJFORMToolStripMenuItem
            // 
            this.pRJFORMToolStripMenuItem.Name = "pRJFORMToolStripMenuItem";
            this.pRJFORMToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pRJFORMToolStripMenuItem.Text = "PRJ_FORM";
            this.pRJFORMToolStripMenuItem.Click += new System.EventHandler(this.pRJFORMToolStripMenuItem_Click);
            // 
            // pRJToolStripMenuItem2
            // 
            this.pRJToolStripMenuItem2.Name = "pRJToolStripMenuItem2";
            this.pRJToolStripMenuItem2.Size = new System.Drawing.Size(187, 22);
            this.pRJToolStripMenuItem2.Text = "PRJ_ADD_REFERENCE";
            this.pRJToolStripMenuItem2.Click += new System.EventHandler(this.pRJToolStripMenuItem2_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(184, 6);
            // 
            // pRJPROPERTIESToolStripMenuItem
            // 
            this.pRJPROPERTIESToolStripMenuItem.Name = "pRJPROPERTIESToolStripMenuItem";
            this.pRJPROPERTIESToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.pRJPROPERTIESToolStripMenuItem.Text = "PRJ_PROPERTIES";
            this.pRJPROPERTIESToolStripMenuItem.Click += new System.EventHandler(this.pRJPROPERTIESToolStripMenuItem_Click);
            // 
            // ReferenceMenuStrip
            // 
            this.ReferenceMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pRJREMOVEToolStripMenuItem});
            this.ReferenceMenuStrip.Name = "ReferenceMenuStrip";
            this.ReferenceMenuStrip.Size = new System.Drawing.Size(144, 26);
            // 
            // pRJREMOVEToolStripMenuItem
            // 
            this.pRJREMOVEToolStripMenuItem.Name = "pRJREMOVEToolStripMenuItem";
            this.pRJREMOVEToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.pRJREMOVEToolStripMenuItem.Text = "PRJ_REMOVE";
            this.pRJREMOVEToolStripMenuItem.Click += new System.EventHandler(this.pRJREMOVEToolStripMenuItem_Click);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "ProjectBrowser.ReferenceFolder.Closed.png");
            this.imageList2.Images.SetKeyName(1, "ProjectBrowser.ReferenceFolder.Open.png");
            this.imageList2.Images.SetKeyName(2, "Icons.16x16.Project.png");
            this.imageList2.Images.SetKeyName(3, "Icons.16x16.Reference.png");
            this.imageList2.Images.SetKeyName(4, "pabcfile.png");
            this.imageList2.Images.SetKeyName(5, "Icons.16x16.Form.png");
            // 
            // ProjectExplorerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(209, 332);
            this.Controls.Add(this.tvProjectExplorer);
            this.Name = "ProjectExplorerForm";
            this.TabText = "PRJ_PROJECT_EXPLORER";
            this.Text = "PRJ_PROJECT_EXPLORER";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProjectExplorerForm_FormClosing);
            this.AddReferenceMenuStrip.ResumeLayout(false);
            this.SouceFileMenuStrip.ResumeLayout(false);
            this.ProjectMenuStrip.ResumeLayout(false);
            this.ReferenceMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.ToolStripMenuItem pRJADDREFERENCEToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip AddReferenceMenuStrip;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.TreeView tvProjectExplorer;
        private System.Windows.Forms.ContextMenuStrip SouceFileMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem pRJOPENToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pRJVIEWCODEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pRJEXCLUDEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pRJToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pRJRENAMEToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ProjectMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem pRJBUILDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pRJBUILDALLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pRJRUNToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem pRJToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem pRJNEWFILEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pRJEXISTFILEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pRJFORMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pRJToolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem pRJPROPERTIESToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ReferenceMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem pRJREMOVEToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList2;
    }
}
