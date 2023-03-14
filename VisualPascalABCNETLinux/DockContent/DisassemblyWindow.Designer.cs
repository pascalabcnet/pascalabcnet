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
            this.DisassemblyEditor = new VisualPascalABC.CodeFileDocumentTextEditorControl();
            this.SuspendLayout();
            // 
            // DisassemblyEditor
            // 
            this.DisassemblyEditor.CaretColumn = 0;
            this.DisassemblyEditor.CaretLine = 0;
            this.DisassemblyEditor.ConvertTabsToSpaces = true;
            this.DisassemblyEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DisassemblyEditor.Location = new System.Drawing.Point(0, 0);
            this.DisassemblyEditor.Name = "DisassemblyEditor";
            this.DisassemblyEditor.ShowLineNumbers = false;
            this.DisassemblyEditor.ShowVRuler = false;
            this.DisassemblyEditor.Size = new System.Drawing.Size(472, 271);
            this.DisassemblyEditor.TabIndent = 4;
            this.DisassemblyEditor.TabIndex = 0;
            // 
            // DisassemblyWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 271);
            this.Controls.Add(this.DisassemblyEditor);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.Name = "DisassemblyWindow";
            this.TabText = "TP_DISASSEMBLY";
            this.Text = "TP_DISASSEMBLY";
            this.ResumeLayout(false);

        }

        #endregion

        private VisualPascalABC.CodeFileDocumentTextEditorControl DisassemblyEditor;
    }
}