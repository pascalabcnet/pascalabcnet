namespace VisualPascalABCPlugins
{
    partial class SemanticTreeVisualisatorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SemanticTreeVisualisatorForm));
            this.PluginImage = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbBuild = new System.Windows.Forms.ToolStripButton();
            this.tbRebuild = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.mylabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.PluginImage)).BeginInit();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PluginImage
            // 
            this.PluginImage.BackColor = System.Drawing.Color.Magenta;
            this.PluginImage.Image = ((System.Drawing.Image)(resources.GetObject("PluginImage.Image")));
            this.PluginImage.Location = new System.Drawing.Point(275, 2);
            this.PluginImage.Name = "PluginImage";
            this.PluginImage.Size = new System.Drawing.Size(16, 16);
            this.PluginImage.TabIndex = 9;
            this.PluginImage.TabStop = false;
            this.PluginImage.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(332, 25);
            this.panel1.TabIndex = 10;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbBuild,
            this.tbRebuild,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(332, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tbBuild
            // 
            this.tbBuild.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbBuild.Image = ((System.Drawing.Image)(resources.GetObject("tbBuild.Image")));
            this.tbBuild.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbBuild.Name = "tbBuild";
            this.tbBuild.Size = new System.Drawing.Size(23, 22);
            this.tbBuild.Text = "toolStripButton1";
            this.tbBuild.ToolTipText = "M_BUILD";
            this.tbBuild.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // tbRebuild
            // 
            this.tbRebuild.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbRebuild.Image = ((System.Drawing.Image)(resources.GetObject("tbRebuild.Image")));
            this.tbRebuild.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbRebuild.Name = "tbRebuild";
            this.tbRebuild.Size = new System.Drawing.Size(23, 22);
            this.tbRebuild.Text = "toolStripButton2";
            this.tbRebuild.ToolTipText = "M_REBUILD";
            this.tbRebuild.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "CHOSEN_SYNTAX_TREE";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(332, 348);
            this.panel2.TabIndex = 11;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.treeView);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(332, 324);
            this.panel4.TabIndex = 1;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(332, 324);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);            
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.statusStrip1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 324);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(332, 24);
            this.panel3.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStrip1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.toolStripStatusLabel1,
            this.mylabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 2);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(332, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // mylabel
            // 
            this.mylabel.Name = "mylabel";
            this.mylabel.Size = new System.Drawing.Size(0, 17);
            // 
            // SemanticTreeVisualisatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 373);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.PluginImage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SemanticTreeVisualisatorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FORMNAME";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SyntaxTreeVisualisatorForm_Load);
            this.Shown += new System.EventHandler(this.SyntaxTreeVisualisatorForm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SyntaxTreeVisualisatorForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.PluginImage)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox PluginImage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tbBuild;
        private System.Windows.Forms.ToolStripButton tbRebuild;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripStatusLabel mylabel;
    }
}