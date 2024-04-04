namespace VisualPascalABC
{
    partial class ToolBoxForm
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
            this.toolboxPane1 = new SampleDesignerApplication.ToolBoxPane();
            this.SuspendLayout();
            // 
            // toolboxPane1
            // 
            this.toolboxPane1.AllowDrop = true;
            this.toolboxPane1.BackColor = System.Drawing.Color.Black;
            this.toolboxPane1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolboxPane1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolboxPane1.Host = null;
            this.toolboxPane1.Location = new System.Drawing.Point(0, 0);
            this.toolboxPane1.Margin = new System.Windows.Forms.Padding(2);
            this.toolboxPane1.Name = "toolboxPane1";
            this.toolboxPane1.Size = new System.Drawing.Size(292, 266);
            this.toolboxPane1.TabIndex = 0;
            // 
            // ToolBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.toolboxPane1);
            this.HideOnClose = true;
            this.Name = "ToolBoxForm";
            this.TabText = "ToolBoxForm";
            this.Text = "ToolBoxForm";
            this.ResumeLayout(false);

        }

        #endregion

        private SampleDesignerApplication.ToolBoxPane toolboxPane1;
    }
}