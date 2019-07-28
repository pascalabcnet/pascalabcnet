namespace CodeTemplatesPlugin
{
    partial class Images
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
            this.PluginImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PluginImage)).BeginInit();
            this.SuspendLayout();
            // 
            // PluginImage
            // 
            this.PluginImage.Image = global::CodeTemplatesPlugin.Properties.Resources.CodeTemplates;
            this.PluginImage.Location = new System.Drawing.Point(39, 37);
            this.PluginImage.Name = "PluginImage";
            this.PluginImage.Size = new System.Drawing.Size(16, 16);
            this.PluginImage.TabIndex = 1;
            this.PluginImage.TabStop = false;
            this.PluginImage.Visible = false;
            // 
            // Images
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 241);
            this.Controls.Add(this.PluginImage);
            this.Name = "Images";
            this.Text = "Images";
            ((System.ComponentModel.ISupportInitialize)(this.PluginImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox PluginImage;
    }
}