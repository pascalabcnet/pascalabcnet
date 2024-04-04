namespace VisualPascalABC
{
    partial class DebugWatchListWindowForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugWatchListWindowForm));
            this.watchList = new AdvancedDataGridView.TreeGridView();
            this.cntxtWatch = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mADDEXPRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mDELETEEXPRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mCLEARALLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSELECTALLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.watchList)).BeginInit();
            this.cntxtWatch.SuspendLayout();
            this.SuspendLayout();
            // 
            // watchList
            // 
            this.watchList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.watchList.ContextMenuStrip = this.cntxtWatch;
            this.watchList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.watchList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.watchList.ImageList = null;
            this.watchList.Location = new System.Drawing.Point(0, 0);
            this.watchList.Name = "watchList";
            this.watchList.Size = new System.Drawing.Size(292, 273);
            this.watchList.TabIndex = 0;
            // 
            // cntxtWatch
            // 
            this.cntxtWatch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mADDEXPRToolStripMenuItem,
            this.mDELETEEXPRToolStripMenuItem,
            this.mCLEARALLToolStripMenuItem,
            this.mSELECTALLToolStripMenuItem});
            this.cntxtWatch.Name = "cntxtWatch";
            this.cntxtWatch.Size = new System.Drawing.Size(161, 92);
            // 
            // mADDEXPRToolStripMenuItem
            // 
            this.mADDEXPRToolStripMenuItem.Name = "mADDEXPRToolStripMenuItem";
            this.mADDEXPRToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.mADDEXPRToolStripMenuItem.Text = "M_ADD_EXPR";
            this.mADDEXPRToolStripMenuItem.Click += new System.EventHandler(this.mADDEXPRToolStripMenuItem_Click);
            // 
            // mDELETEEXPRToolStripMenuItem
            // 
            this.mDELETEEXPRToolStripMenuItem.Name = "mDELETEEXPRToolStripMenuItem";
            this.mDELETEEXPRToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.mDELETEEXPRToolStripMenuItem.Text = "M_DELETE_EXPR";
            this.mDELETEEXPRToolStripMenuItem.Click += new System.EventHandler(this.mDELETEEXPRToolStripMenuItem_Click);
            // 
            // mCLEARALLToolStripMenuItem
            // 
            this.mCLEARALLToolStripMenuItem.Name = "mCLEARALLToolStripMenuItem";
            this.mCLEARALLToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.mCLEARALLToolStripMenuItem.Text = "M_CLEAR_ALL";
            this.mCLEARALLToolStripMenuItem.Click += new System.EventHandler(this.mCLEARALLToolStripMenuItem_Click);
            // 
            // mSELECTALLToolStripMenuItem
            // 
            this.mSELECTALLToolStripMenuItem.Name = "mSELECTALLToolStripMenuItem";
            this.mSELECTALLToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.mSELECTALLToolStripMenuItem.Text = "M_SELECT_ALL";
            this.mSELECTALLToolStripMenuItem.Click += new System.EventHandler(this.mSELECTALLToolStripMenuItem_Click);
            // 
            // DebugWatchListWindowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.watchList);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DebugWatchListWindowForm";
            this.TabText = "TP_WATCHLIST";
            this.Text = "TP_WATCHLIST";
            this.Load += new System.EventHandler(this.DebugWathListWindowForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.watchList)).EndInit();
            this.cntxtWatch.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal AdvancedDataGridView.TreeGridView watchList;
        private System.Windows.Forms.ContextMenuStrip cntxtWatch;
        private System.Windows.Forms.ToolStripMenuItem mADDEXPRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mDELETEEXPRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mCLEARALLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mSELECTALLToolStripMenuItem;

    }
}
