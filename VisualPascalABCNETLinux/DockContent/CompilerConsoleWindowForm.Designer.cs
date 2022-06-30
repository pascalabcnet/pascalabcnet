namespace VisualPascalABC
{
    partial class CompilerConsoleWindowForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompilerConsoleWindowForm));
            this.CompilerConsole = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // CompilerConsole
            // 
            this.CompilerConsole.BackColor = System.Drawing.Color.White;
            this.CompilerConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CompilerConsole.Font = new System.Drawing.Font("Courier New", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CompilerConsole.Location = new System.Drawing.Point(0, 0);
            this.CompilerConsole.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.CompilerConsole.Multiline = true;
            this.CompilerConsole.Name = "CompilerConsole";
            this.CompilerConsole.ReadOnly = true;
            this.CompilerConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.CompilerConsole.Size = new System.Drawing.Size(584, 525);
            this.CompilerConsole.TabIndex = 1;
            // 
            // CompilerConsoleWindowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 525);
            this.Controls.Add(this.CompilerConsole);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "CompilerConsoleWindowForm";
            this.TabText = "TP_COMPILERMESSAGES";
            this.Text = "TP_COMPILERMESSAGES";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox CompilerConsole;
    }
}