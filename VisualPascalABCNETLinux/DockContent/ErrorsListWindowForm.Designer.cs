namespace VisualPascalABC
{
    partial class ErrorsListWindowForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorsListWindowForm));
            this.lvErrorsList = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tPCOPYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ilvlErrorList = new System.Windows.Forms.ImageList(this.components);
            this.ilvlErrorList32 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvErrorsList
            // 
            this.lvErrorsList.AutoArrange = false;
            this.lvErrorsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11});
            this.lvErrorsList.ContextMenuStrip = this.contextMenuStrip1;
            this.lvErrorsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvErrorsList.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lvErrorsList.FullRowSelect = true;
            this.lvErrorsList.GridLines = true;
            this.lvErrorsList.HideSelection = false;
            this.lvErrorsList.Location = new System.Drawing.Point(0, 0);
            this.lvErrorsList.Margin = new System.Windows.Forms.Padding(6);
            this.lvErrorsList.MultiSelect = false;
            this.lvErrorsList.Name = "lvErrorsList";
            this.lvErrorsList.RightToLeftLayout = true;
            this.lvErrorsList.ShowItemToolTips = true;
            this.lvErrorsList.Size = new System.Drawing.Size(1574, 525);
            this.lvErrorsList.SmallImageList = this.ilvlErrorList;
            this.lvErrorsList.TabIndex = 1;
            this.lvErrorsList.TileSize = new System.Drawing.Size(1, 1);
            this.lvErrorsList.UseCompatibleStateImageBehavior = false;
            this.lvErrorsList.View = System.Windows.Forms.View.Details;
            this.lvErrorsList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvErrorsList_KeyDown);
            this.lvErrorsList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvErrorsList_MouseDoubleClick);
            this.lvErrorsList.Resize += new System.EventHandler(this.lvErrorsList_Resize);
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "";
            this.columnHeader6.Width = 23;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "ER_NUM";
            this.columnHeader7.Width = 25;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "ER_LINE";
            this.columnHeader8.Width = 70;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "ER_DESCRIPTION";
            this.columnHeader9.Width = 300;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "ER_FILE";
            this.columnHeader10.Width = 150;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "ER_PATH";
            this.columnHeader11.Width = 200;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tPCOPYToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(185, 42);
            // 
            // tPCOPYToolStripMenuItem
            // 
            this.tPCOPYToolStripMenuItem.Name = "tPCOPYToolStripMenuItem";
            this.tPCOPYToolStripMenuItem.Size = new System.Drawing.Size(184, 38);
            this.tPCOPYToolStripMenuItem.Text = "ER_COPY";
            this.tPCOPYToolStripMenuItem.Click += new System.EventHandler(this.tPCOPYToolStripMenuItem_Click);
            // 
            // ilvlErrorList
            // 
            this.ilvlErrorList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilvlErrorList.ImageStream")));
            this.ilvlErrorList.TransparentColor = System.Drawing.Color.Transparent;
            this.ilvlErrorList.Images.SetKeyName(0, "Icons.16x16.Warning.png");
            this.ilvlErrorList.Images.SetKeyName(1, "Icons.16x16.Error.png");
            this.ilvlErrorList.Images.SetKeyName(2, "Icons.16x16.RuntimeError.png");
            // 
            // ilvlErrorList32
            // 
            this.ilvlErrorList32.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilvlErrorList32.ImageStream")));
            this.ilvlErrorList32.TransparentColor = System.Drawing.Color.Transparent;
            this.ilvlErrorList32.Images.SetKeyName(0, "Icons.16x16.Warning.png");
            this.ilvlErrorList32.Images.SetKeyName(1, "Icons.32x32.Error.png");
            this.ilvlErrorList32.Images.SetKeyName(2, "Icons.16x16.RuntimeError.png");
            // 
            // ErrorsListWindowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1574, 525);
            this.Controls.Add(this.lvErrorsList);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "ErrorsListWindowForm";
            this.ShowIcon = false;
            this.TabText = "TP_ERRORSLIST";
            this.Text = "TP_ERRORSLIST";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ErrorsListWindowForm_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.ErrorsListWindowForm_VisibleChanged);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader columnHeader6;
        public System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        internal System.Windows.Forms.ListView lvErrorsList;
        private System.Windows.Forms.ImageList ilvlErrorList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tPCOPYToolStripMenuItem;
        private System.Windows.Forms.ImageList ilvlErrorList32;
    }
}
