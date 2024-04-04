namespace VisualPascalABC
{
    partial class FindSymbolsResultWindowForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindSymbolsResultWindowForm));
            this.lvFindSymbolResults = new System.Windows.Forms.ListView();
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvFindSymbolResults
            // 
            this.lvFindSymbolResults.AutoArrange = false;
            this.lvFindSymbolResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader12,
            this.columnHeader13});
            this.lvFindSymbolResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFindSymbolResults.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lvFindSymbolResults.FullRowSelect = true;
            this.lvFindSymbolResults.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvFindSymbolResults.HideSelection = false;
            this.lvFindSymbolResults.Location = new System.Drawing.Point(0, 0);
            this.lvFindSymbolResults.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.lvFindSymbolResults.MultiSelect = false;
            this.lvFindSymbolResults.Name = "lvFindSymbolResults";
            this.lvFindSymbolResults.ShowItemToolTips = true;
            this.lvFindSymbolResults.Size = new System.Drawing.Size(584, 525);
            this.lvFindSymbolResults.TabIndex = 1;
            this.lvFindSymbolResults.UseCompatibleStateImageBehavior = false;
            this.lvFindSymbolResults.View = System.Windows.Forms.View.Details;
            this.lvFindSymbolResults.Resize += new System.EventHandler(this.lvFindSymbolResults_Resize);
            // 
            // columnHeader12
            // 
            this.columnHeader12.Width = 20;
            // 
            // FindSymbolsResultWindowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 525);
            this.Controls.Add(this.lvFindSymbolResults);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "FindSymbolsResultWindowForm";
            this.TabText = "TP_FIND_SYMBOL_RESULTS";
            this.Text = "TP_FIND_SYMBOL_RESULTS";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvFindSymbolResults;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
    }
}