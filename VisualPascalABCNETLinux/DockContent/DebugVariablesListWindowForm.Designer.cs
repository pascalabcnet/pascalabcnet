namespace VisualPascalABC
{
    partial class DebugVariablesListWindowForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugVariablesListWindowForm));
            this.watchList = new AdvancedDataGridView.TreeGridView();
            ((System.ComponentModel.ISupportInitialize)(this.watchList)).BeginInit();
            this.SuspendLayout();
            // 
            // watchList
            // 
            this.watchList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.watchList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.watchList.ImageList = null;
            this.watchList.Location = new System.Drawing.Point(0, 0);
            this.watchList.Name = "watchList";
            this.watchList.Size = new System.Drawing.Size(292, 273);
            this.watchList.TabIndex = 0;
            // 
            // DebugVariablesListWindowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.watchList);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DebugVariablesListWindowForm";
            this.TabText = "TP_VARLIST";
            this.Text = "TP_VARLIST";
            ((System.ComponentModel.ISupportInitialize)(this.watchList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal AdvancedDataGridView.TreeGridView watchList;


    }
}