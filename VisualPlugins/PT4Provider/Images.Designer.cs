namespace PT4Provider
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Images));
            this.pb_pt4d = new System.Windows.Forms.PictureBox();
            this.pb_pt4l = new System.Windows.Forms.PictureBox();
            this.pb_pt4r = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pb_pt4d)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_pt4l)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_pt4r)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_pt4d
            // 
            this.pb_pt4d.BackColor = System.Drawing.Color.Fuchsia;
            this.pb_pt4d.Image = ((System.Drawing.Image)(resources.GetObject("pb_pt4d.Image")));
            this.pb_pt4d.Location = new System.Drawing.Point(17, 26);
            this.pb_pt4d.Name = "pb_pt4d";
            this.pb_pt4d.Size = new System.Drawing.Size(16, 16);
            this.pb_pt4d.TabIndex = 0;
            this.pb_pt4d.TabStop = false;
            // 
            // pb_pt4l
            // 
            this.pb_pt4l.BackColor = System.Drawing.Color.Fuchsia;
            this.pb_pt4l.Image = ((System.Drawing.Image)(resources.GetObject("pb_pt4l.Image")));
            this.pb_pt4l.Location = new System.Drawing.Point(39, 26);
            this.pb_pt4l.Name = "pb_pt4l";
            this.pb_pt4l.Size = new System.Drawing.Size(16, 16);
            this.pb_pt4l.TabIndex = 1;
            this.pb_pt4l.TabStop = false;
            // 
            // pb_pt4r
            // 
            this.pb_pt4r.BackColor = System.Drawing.Color.Fuchsia;
            this.pb_pt4r.Image = ((System.Drawing.Image)(resources.GetObject("pb_pt4r.Image")));
            this.pb_pt4r.Location = new System.Drawing.Point(61, 26);
            this.pb_pt4r.Name = "pb_pt4r";
            this.pb_pt4r.Size = new System.Drawing.Size(16, 16);
            this.pb_pt4r.TabIndex = 2;
            this.pb_pt4r.TabStop = false;
            // 
            // Images
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(142, 106);
            this.Controls.Add(this.pb_pt4r);
            this.Controls.Add(this.pb_pt4l);
            this.Controls.Add(this.pb_pt4d);
            this.Name = "Images";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pb_pt4d)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_pt4l)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_pt4r)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox pb_pt4d;
        public System.Windows.Forms.PictureBox pb_pt4l;
        public System.Windows.Forms.PictureBox pb_pt4r;

    }
}

