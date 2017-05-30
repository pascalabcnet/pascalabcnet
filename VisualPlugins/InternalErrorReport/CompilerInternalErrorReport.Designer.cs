namespace VisualPascalABCPlugins
{
    partial class CompilerInternalErrorReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompilerInternalErrorReport));
            this.label1 = new System.Windows.Forms.Label();
            this.ErrorMessage = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.ReportTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(289, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IN_A_COMPILE_TIME_INTTERNAL_COMPILER_ERROR";
            // 
            // ErrorMessage
            // 
            this.ErrorMessage.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ErrorMessage.Location = new System.Drawing.Point(15, 34);
            this.ErrorMessage.Multiline = true;
            this.ErrorMessage.Name = "ErrorMessage";
            this.ErrorMessage.ReadOnly = true;
            this.ErrorMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ErrorMessage.Size = new System.Drawing.Size(484, 86);
            this.ErrorMessage.TabIndex = 3;
            // 
            // okButton
            // 
            this.okButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.okButton.Location = new System.Drawing.Point(209, 347);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(110, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "CLOSE";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // ReportTextBox
            // 
            this.ReportTextBox.Location = new System.Drawing.Point(15, 198);
            this.ReportTextBox.Multiline = true;
            this.ReportTextBox.Name = "ReportTextBox";
            this.ReportTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ReportTextBox.Size = new System.Drawing.Size(486, 108);
            this.ReportTextBox.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 312);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(140, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "SAVE_REPORT_AS";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(161, 317);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "SEND_TO_EMAIL";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(367, 317);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(129, 13);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "ibondarev@sfedu.ru";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "zip";
            this.saveFileDialog.Filter = "Zip files (*.zip)|*.zip";
            this.saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog_FileOk);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(487, 72);
            this.label3.TabIndex = 8;
            this.label3.Text = "SEND_RPEPORT_AUTORS";
            // 
            // CompilerInternalErrorReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 384);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ReportTextBox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.ErrorMessage);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CompilerInternalErrorReport";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "COMPILER_INTERNAL_ERROR";
            this.Load += new System.EventHandler(this.CompilerInternalErrorReport_Load);
            this.Shown += new System.EventHandler(this.CompilerInternalErrorReport_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button okButton;
        public System.Windows.Forms.TextBox ErrorMessage;
        private System.Windows.Forms.TextBox ReportTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Label label3;
    }
}