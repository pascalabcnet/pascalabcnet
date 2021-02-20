namespace VisualPascalABC
{
    partial class DisassemblyWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImmediateWindow));
            this.DisassemblyEditor = new VisualPascalABC.CodeFileDocumentTextEditorControl();
            this.SuspendLayout();
            // 
            // DisassemblyEditor
            // 
            this.DisassemblyEditor.ConvertTabsToSpaces = true;
            this.DisassemblyEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DisassemblyEditor.IsReadOnly = false;
            this.DisassemblyEditor.Location = new System.Drawing.Point(0, 0);
            this.DisassemblyEditor.Name = "DisassemblyEditor";
            this.DisassemblyEditor.ShowVRuler = false;
            this.DisassemblyEditor.ShowLineNumbers = false;
            this.DisassemblyEditor.Size = new System.Drawing.Size(472, 271);
            this.DisassemblyEditor.TabIndex = 0;
            // 
            // ImmediateWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 271);
            this.Controls.Add(this.DisassemblyEditor);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom;
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DisassemblyWindow";
            this.TabText = "TP_DISASSEMBLY";
            this.Text = "TP_DISASSEMBLY";
            this.ResumeLayout(false);
        }

        #endregion

        private VisualPascalABC.CodeFileDocumentTextEditorControl DisassemblyEditor;
    }
}