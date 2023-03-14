namespace VisualPascalABC
{
    partial class GotoLineForm
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
        	this.btOk = new System.Windows.Forms.Button();
        	this.btCancel = new System.Windows.Forms.Button();
        	this.tbLineNumber = new System.Windows.Forms.TextBox();
        	this.label1 = new System.Windows.Forms.Label();
        	this.SuspendLayout();
        	// 
        	// btOk
        	// 
        	this.btOk.Location = new System.Drawing.Point(76, 56);
        	this.btOk.Name = "btOk";
        	this.btOk.Size = new System.Drawing.Size(75, 23);
        	this.btOk.TabIndex = 0;
        	this.btOk.Text = "!OK";
        	this.btOk.UseVisualStyleBackColor = true;
        	this.btOk.Click += new System.EventHandler(this.btOk_Click);
        	// 
        	// btCancel
        	// 
        	this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        	this.btCancel.Location = new System.Drawing.Point(157, 56);
        	this.btCancel.Name = "btCancel";
        	this.btCancel.Size = new System.Drawing.Size(75, 23);
        	this.btCancel.TabIndex = 1;
        	this.btCancel.Text = "!CANCEL";
        	this.btCancel.UseVisualStyleBackColor = true;
        	this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
        	// 
        	// tbLineNumber
        	// 
        	this.tbLineNumber.Location = new System.Drawing.Point(10, 30);
        	this.tbLineNumber.Name = "tbLineNumber";
        	this.tbLineNumber.Size = new System.Drawing.Size(222, 20);
        	this.tbLineNumber.TabIndex = 2;
        	// 
        	// label1
        	// 
        	this.label1.AutoSize = true;
        	this.label1.Location = new System.Drawing.Point(7, 9);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(30, 13);
        	this.label1.TabIndex = 3;
        	this.label1.Text = "_text";
        	// 
        	// GotoLineForm
        	// 
        	this.AcceptButton = this.btOk;
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.CancelButton = this.btCancel;
        	this.ClientSize = new System.Drawing.Size(244, 87);
        	this.Controls.Add(this.label1);
        	this.Controls.Add(this.tbLineNumber);
        	this.Controls.Add(this.btCancel);
        	this.Controls.Add(this.btOk);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        	this.MaximizeBox = false;
        	this.MinimizeBox = false;
        	this.Name = "GotoLineForm";
        	this.ShowInTaskbar = false;
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        	this.Text = "FORMTEXT";
        	this.Load += new System.EventHandler(this.GotoLineForm_Load);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.TextBox tbLineNumber;
        private System.Windows.Forms.Label label1;
    }
}