namespace VisualPascalABC
{
    partial class CodeFileDocumentControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeFileDocumentControl));
            this.basePanel = new System.Windows.Forms.Panel();
            this.editorPanel = new System.Windows.Forms.Panel();
            this.TextEditor = new VisualPascalABC.CodeFileDocumentTextEditorControl();
            this.basePanel.SuspendLayout();
            this.editorPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // basePanel
            // 
            this.basePanel.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.basePanel.Controls.Add(this.editorPanel);
            this.basePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basePanel.Location = new System.Drawing.Point(0, 1);
            this.basePanel.Margin = new System.Windows.Forms.Padding(4);
            this.basePanel.Name = "basePanel";
            this.basePanel.Size = new System.Drawing.Size(657, 586);
            this.basePanel.TabIndex = 0;
            // 
            // editorPanel
            // 
            this.editorPanel.BackColor = System.Drawing.SystemColors.Control;
            this.editorPanel.Controls.Add(this.TextEditor);
            this.editorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorPanel.Location = new System.Drawing.Point(0, 0);
            this.editorPanel.Margin = new System.Windows.Forms.Padding(4);
            this.editorPanel.Name = "editorPanel";
            this.editorPanel.Size = new System.Drawing.Size(657, 586);
            this.editorPanel.TabIndex = 5;
            // 
            // TextEditor
            // 
            this.TextEditor.CaretColumn = 0;
            this.TextEditor.CaretLine = 0;
            this.TextEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextEditor.Location = new System.Drawing.Point(0, 0);
            this.TextEditor.Margin = new System.Windows.Forms.Padding(4);
            this.TextEditor.Name = "TextEditor";
            this.TextEditor.Size = new System.Drawing.Size(657, 586);
            this.TextEditor.TabIndent = 4;
            this.TextEditor.TabIndex = 0;
            // 
            // CodeFileDocumentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(657, 587);
            this.Controls.Add(this.basePanel);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CodeFileDocumentControl";
            this.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.ShowIcon = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CodeFileDocumentControl_FormClosing);
            this.Shown += new System.EventHandler(this.CodeFileDocumentControl_Activated);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CodeFileDocumentControl_Paint);
            this.basePanel.ResumeLayout(false);
            this.editorPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel basePanel;
        private System.Windows.Forms.Panel editorPanel;
        public CodeFileDocumentTextEditorControl TextEditor;
    }
}
