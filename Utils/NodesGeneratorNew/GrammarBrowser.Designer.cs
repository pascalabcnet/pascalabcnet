// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
namespace NodesGenerator
{
    partial class GrammarBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GrammarBrowser));
            this.scintilla1 = new ScintillaNET.Scintilla();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.saveHieararchy = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.searchText = new System.Windows.Forms.ToolStripTextBox();
            this.searchDown = new System.Windows.Forms.ToolStripButton();
            this.searchUp = new System.Windows.Forms.ToolStripButton();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.scintilla1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // scintilla1
            // 
            this.scintilla1.Cursor = System.Windows.Forms.Cursors.Default;
            this.scintilla1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla1.Folding.MarkerScheme = ScintillaNET.FoldMarkerScheme.Custom;
            this.scintilla1.Location = new System.Drawing.Point(0, 0);
            this.scintilla1.Markers.Folder.BackColor = System.Drawing.Color.Gray;
            this.scintilla1.Markers.Folder.ForeColor = System.Drawing.Color.White;
            this.scintilla1.Markers.Folder.Number = 30;
            this.scintilla1.Markers.Folder.Symbol = ScintillaNET.MarkerSymbol.BoxPlus;
            this.scintilla1.Markers.FolderEnd.BackColor = System.Drawing.Color.Gray;
            this.scintilla1.Markers.FolderEnd.ForeColor = System.Drawing.Color.White;
            this.scintilla1.Markers.FolderEnd.Number = 25;
            this.scintilla1.Markers.FolderEnd.Symbol = ScintillaNET.MarkerSymbol.BoxPlusConnected;
            this.scintilla1.Markers.FolderOpen.BackColor = System.Drawing.Color.Gray;
            this.scintilla1.Markers.FolderOpen.ForeColor = System.Drawing.Color.White;
            this.scintilla1.Markers.FolderOpen.Number = 31;
            this.scintilla1.Markers.FolderOpen.Symbol = ScintillaNET.MarkerSymbol.BoxMinus;
            this.scintilla1.Markers.FolderOpenMid.BackColor = System.Drawing.Color.Gray;
            this.scintilla1.Markers.FolderOpenMid.ForeColor = System.Drawing.Color.White;
            this.scintilla1.Markers.FolderOpenMid.Number = 26;
            this.scintilla1.Markers.FolderOpenMid.Symbol = ScintillaNET.MarkerSymbol.BoxMinusConnected;
            this.scintilla1.Markers.FolderOpenMidTail.BackColor = System.Drawing.Color.Gray;
            this.scintilla1.Markers.FolderOpenMidTail.ForeColor = System.Drawing.Color.White;
            this.scintilla1.Markers.FolderOpenMidTail.Number = 27;
            this.scintilla1.Markers.FolderOpenMidTail.Symbol = ScintillaNET.MarkerSymbol.TCorner;
            this.scintilla1.Markers.FolderSub.BackColor = System.Drawing.Color.Gray;
            this.scintilla1.Markers.FolderSub.ForeColor = System.Drawing.Color.White;
            this.scintilla1.Markers.FolderSub.Number = 29;
            this.scintilla1.Markers.FolderSub.Symbol = ScintillaNET.MarkerSymbol.VLine;
            this.scintilla1.Markers.FolderTail.BackColor = System.Drawing.Color.Gray;
            this.scintilla1.Markers.FolderTail.ForeColor = System.Drawing.Color.White;
            this.scintilla1.Markers.FolderTail.Number = 28;
            this.scintilla1.Markers.FolderTail.Symbol = ScintillaNET.MarkerSymbol.LCorner;
            this.scintilla1.Name = "scintilla1";
            this.scintilla1.Size = new System.Drawing.Size(649, 762);
            this.scintilla1.Styles.BraceBad.Size = 7F;
            this.scintilla1.Styles.BraceLight.Size = 7F;
            this.scintilla1.Styles.ControlChar.Size = 7F;
            this.scintilla1.Styles.Default.BackColor = System.Drawing.SystemColors.Window;
            this.scintilla1.Styles.Default.Size = 7F;
            this.scintilla1.Styles.IndentGuide.Size = 7F;
            this.scintilla1.Styles.LastPredefined.Size = 7F;
            this.scintilla1.Styles.LineNumber.Size = 7F;
            this.scintilla1.Styles.Max.Size = 7F;
            this.scintilla1.TabIndex = 0;
            this.scintilla1.HotspotClick += new System.EventHandler<ScintillaNET.HotspotClickEventArgs>(this.scintilla1_HotspotClick);
            this.scintilla1.SelectionChanged += new System.EventHandler(this.scintilla1_SelectionChanged);
            this.scintilla1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.scintilla1_MouseUp);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveHieararchy,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.searchText,
            this.searchDown,
            this.searchUp});
            this.toolStrip1.Location = new System.Drawing.Point(0, 737);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(649, 25);
            this.toolStrip1.TabIndex = 2;
            // 
            // saveHieararchy
            // 
            this.saveHieararchy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveHieararchy.Image = ((System.Drawing.Image)(resources.GetObject("saveHieararchy.Image")));
            this.saveHieararchy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveHieararchy.Name = "saveHieararchy";
            this.saveHieararchy.Size = new System.Drawing.Size(23, 22);
            this.saveHieararchy.Text = "Save As...";
            this.saveHieararchy.Click += new System.EventHandler(this.saveHieararchy_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(33, 22);
            this.toolStripLabel1.Text = "Find:";
            // 
            // searchText
            // 
            this.searchText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchText.Name = "searchText";
            this.searchText.Size = new System.Drawing.Size(200, 25);
            this.searchText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchText_KeyDown);
            // 
            // searchDown
            // 
            this.searchDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.searchDown.Image = ((System.Drawing.Image)(resources.GetObject("searchDown.Image")));
            this.searchDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.searchDown.Name = "searchDown";
            this.searchDown.Size = new System.Drawing.Size(23, 22);
            this.searchDown.Text = "Search Down";
            this.searchDown.Click += new System.EventHandler(this.searchDown_Click);
            // 
            // searchUp
            // 
            this.searchUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.searchUp.Image = ((System.Drawing.Image)(resources.GetObject("searchUp.Image")));
            this.searchUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.searchUp.Name = "searchUp";
            this.searchUp.Size = new System.Drawing.Size(23, 22);
            this.searchUp.Text = "Search Up";
            this.searchUp.Click += new System.EventHandler(this.searchUp_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "txt";
            this.saveFileDialog1.Filter = "Text Files|*.txt|All Files|*.*";
            // 
            // GrammarBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 762);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.scintilla1);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "GrammarBrowser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Hierarchy Browser";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GrammarBrowser_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.scintilla1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScintillaNET.Scintilla scintilla1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton saveHieararchy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox searchText;
        private System.Windows.Forms.ToolStripButton searchDown;
        private System.Windows.Forms.ToolStripButton searchUp;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}