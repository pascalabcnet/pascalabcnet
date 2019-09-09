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
            this.PluginImage.Image = global::CodeTemplatesPlugin.Properties.Resources.CodeTemplate3;
            this.PluginImage.InitialImage = global::CodeTemplatesPlugin.Properties.Resources.CodeTemplates2;
            this.PluginImage.Location = new System.Drawing.Point(78, 71);
            this.PluginImage.Margin = new System.Windows.Forms.Padding(6);
            this.PluginImage.Name = "PluginImage";
            this.PluginImage.Size = new System.Drawing.Size(16, 16);
            this.PluginImage.TabIndex = 1;
            this.PluginImage.TabStop = false;
            this.PluginImage.Visible = false;
            // 
            // Images
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 463);
            this.Controls.Add(this.PluginImage);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "Images";
            this.Text = "Images";
            ((System.ComponentModel.ISupportInitialize)(this.PluginImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox PluginImage;
    }
}