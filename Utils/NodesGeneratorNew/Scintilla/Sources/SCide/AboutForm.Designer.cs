namespace SCide
{
    partial class AboutForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
			this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.logoPictureBox = new System.Windows.Forms.PictureBox();
			this.productNameLabel = new System.Windows.Forms.Label();
			this.versionLabel = new System.Windows.Forms.Label();
			this.copyrightLabel = new System.Windows.Forms.Label();
			this.companyNameLabel = new System.Windows.Forms.Label();
			this.descriptionTextBox = new System.Windows.Forms.TextBox();
			this.okButton = new System.Windows.Forms.Button();
			this.tableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.ColumnCount = 2;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67F));
			this.tableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.productNameLabel, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.versionLabel, 1, 1);
			this.tableLayoutPanel.Controls.Add(this.copyrightLabel, 1, 2);
			this.tableLayoutPanel.Controls.Add(this.companyNameLabel, 1, 3);
			this.tableLayoutPanel.Controls.Add(this.descriptionTextBox, 1, 4);
			this.tableLayoutPanel.Controls.Add(this.okButton, 1, 5);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel.Location = new System.Drawing.Point(9, 9);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 6;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel.Size = new System.Drawing.Size(417, 265);
			this.tableLayoutPanel.TabIndex = 0;
			// 
			// logoPictureBox
			// 
			this.logoPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
			this.logoPictureBox.Location = new System.Drawing.Point(3, 3);
			this.logoPictureBox.Name = "logoPictureBox";
			this.tableLayoutPanel.SetRowSpan(this.logoPictureBox, 6);
			this.logoPictureBox.Size = new System.Drawing.Size(131, 259);
			this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.logoPictureBox.TabIndex = 12;
			this.logoPictureBox.TabStop = false;
			// 
			// productNameLabel
			// 
			this.productNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.productNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.productNameLabel.Location = new System.Drawing.Point(143, 0);
			this.productNameLabel.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			this.productNameLabel.MaximumSize = new System.Drawing.Size(0, 17);
			this.productNameLabel.Name = "productNameLabel";
			this.productNameLabel.Size = new System.Drawing.Size(271, 17);
			this.productNameLabel.TabIndex = 19;
			this.productNameLabel.Text = "Product Name";
			this.productNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// versionLabel
			// 
			this.versionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.versionLabel.Location = new System.Drawing.Point(143, 26);
			this.versionLabel.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			this.versionLabel.MaximumSize = new System.Drawing.Size(0, 17);
			this.versionLabel.Name = "versionLabel";
			this.versionLabel.Size = new System.Drawing.Size(271, 17);
			this.versionLabel.TabIndex = 0;
			this.versionLabel.Text = "Version";
			this.versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// copyrightLabel
			// 
			this.copyrightLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.copyrightLabel.Location = new System.Drawing.Point(143, 52);
			this.copyrightLabel.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			this.copyrightLabel.MaximumSize = new System.Drawing.Size(0, 17);
			this.copyrightLabel.Name = "copyrightLabel";
			this.copyrightLabel.Size = new System.Drawing.Size(271, 17);
			this.copyrightLabel.TabIndex = 21;
			this.copyrightLabel.Text = "Copyright";
			this.copyrightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// companyNameLabel
			// 
			this.companyNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.companyNameLabel.Location = new System.Drawing.Point(143, 78);
			this.companyNameLabel.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			this.companyNameLabel.MaximumSize = new System.Drawing.Size(0, 17);
			this.companyNameLabel.Name = "companyNameLabel";
			this.companyNameLabel.Size = new System.Drawing.Size(271, 17);
			this.companyNameLabel.TabIndex = 22;
			this.companyNameLabel.Text = "Company Name";
			this.companyNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// descriptionTextBox
			// 
			this.descriptionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.descriptionTextBox.Location = new System.Drawing.Point(143, 107);
			this.descriptionTextBox.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
			this.descriptionTextBox.Multiline = true;
			this.descriptionTextBox.Name = "descriptionTextBox";
			this.descriptionTextBox.ReadOnly = true;
			this.descriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.descriptionTextBox.Size = new System.Drawing.Size(271, 126);
			this.descriptionTextBox.TabIndex = 23;
			this.descriptionTextBox.TabStop = false;
			this.descriptionTextBox.Text = "Description";
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.okButton.Location = new System.Drawing.Point(339, 239);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 24;
			this.okButton.Text = "&OK";
			// 
			// frmAbout
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(435, 283);
			this.Controls.Add(this.tableLayoutPanel);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAbout";
			this.Padding = new System.Windows.Forms.Padding(9);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About SCide";
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Label productNameLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.Label companyNameLabel;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Button okButton;
    }
}
