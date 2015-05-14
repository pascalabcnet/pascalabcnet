namespace VisualPascalABCPlugins
{
    partial class TestForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestForm));
            this.PluginImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PluginImage)).BeginInit();
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
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 361);
            this.Controls.Add(this.PluginImage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FORMNAME";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SyntaxTreeVisualisatorForm_Load);
            this.Shown += new System.EventHandler(this.SyntaxTreeVisualisatorForm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SyntaxTreeVisualisatorForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.PluginImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox PluginImage;


    }
}