namespace VisualPascalABC
{
    partial class FindReplaceForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.lReplaceTo = new System.Windows.Forms.Label();
            this.btFindNext = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbUseRegex = new System.Windows.Forms.CheckBox();
            this.cbSearchUp = new System.Windows.Forms.CheckBox();
            this.cbMathWord = new System.Windows.Forms.CheckBox();
            this.cbMatchCase = new System.Windows.Forms.CheckBox();
            this.btReplace = new System.Windows.Forms.Button();
            this.btReplaceAll = new System.Windows.Forms.Button();
            this.tbTextToFind = new System.Windows.Forms.ComboBox();
            this.tbTextToReplace = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "L_FIND";
            // 
            // lReplaceTo
            // 
            this.lReplaceTo.AutoSize = true;
            this.lReplaceTo.Location = new System.Drawing.Point(27, 38);
            this.lReplaceTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lReplaceTo.Name = "lReplaceTo";
            this.lReplaceTo.Size = new System.Drawing.Size(107, 17);
            this.lReplaceTo.TabIndex = 3;
            this.lReplaceTo.Text = "L_REPLACETO";
            // 
            // btFindNext
            // 
            this.btFindNext.Location = new System.Drawing.Point(313, 155);
            this.btFindNext.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btFindNext.Name = "btFindNext";
            this.btFindNext.Size = new System.Drawing.Size(135, 33);
            this.btFindNext.TabIndex = 4;
            this.btFindNext.Text = "FINDNEXT";
            this.btFindNext.UseVisualStyleBackColor = true;
            this.btFindNext.Click += new System.EventHandler(this.btFindNext_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(456, 155);
            this.btCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(100, 33);
            this.btCancel.TabIndex = 5;
            this.btCancel.Text = "!CANCEL";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbUseRegex);
            this.groupBox1.Controls.Add(this.cbSearchUp);
            this.groupBox1.Controls.Add(this.cbMathWord);
            this.groupBox1.Controls.Add(this.cbMatchCase);
            this.groupBox1.Location = new System.Drawing.Point(16, 64);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(540, 84);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "G_OPTIONS";
            // 
            // cbUseRegex
            // 
            this.cbUseRegex.AutoSize = true;
            this.cbUseRegex.Location = new System.Drawing.Point(248, 52);
            this.cbUseRegex.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbUseRegex.Name = "cbUseRegex";
            this.cbUseRegex.Size = new System.Drawing.Size(111, 21);
            this.cbUseRegex.TabIndex = 3;
            this.cbUseRegex.Text = "USE_REGEX";
            this.cbUseRegex.UseVisualStyleBackColor = true;
            // 
            // cbSearchUp
            // 
            this.cbSearchUp.AutoSize = true;
            this.cbSearchUp.Location = new System.Drawing.Point(248, 23);
            this.cbSearchUp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbSearchUp.Name = "cbSearchUp";
            this.cbSearchUp.Size = new System.Drawing.Size(110, 21);
            this.cbSearchUp.TabIndex = 2;
            this.cbSearchUp.Text = "SEARCH_UP";
            this.cbSearchUp.UseVisualStyleBackColor = true;
            // 
            // cbMathWord
            // 
            this.cbMathWord.AutoSize = true;
            this.cbMathWord.Location = new System.Drawing.Point(8, 52);
            this.cbMathWord.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbMathWord.Name = "cbMathWord";
            this.cbMathWord.Size = new System.Drawing.Size(177, 21);
            this.cbMathWord.TabIndex = 1;
            this.cbMathWord.Text = "MATCH_WHOLE_WORD";
            this.cbMathWord.UseVisualStyleBackColor = true;
            // 
            // cbMatchCase
            // 
            this.cbMatchCase.AutoSize = true;
            this.cbMatchCase.Location = new System.Drawing.Point(8, 23);
            this.cbMatchCase.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbMatchCase.Name = "cbMatchCase";
            this.cbMatchCase.Size = new System.Drawing.Size(110, 21);
            this.cbMatchCase.TabIndex = 0;
            this.cbMatchCase.Text = "MATCH_CASE";
            this.cbMatchCase.UseVisualStyleBackColor = true;
            // 
            // btReplace
            // 
            this.btReplace.Location = new System.Drawing.Point(159, 155);
            this.btReplace.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btReplace.Name = "btReplace";
            this.btReplace.Size = new System.Drawing.Size(135, 33);
            this.btReplace.TabIndex = 7;
            this.btReplace.Text = "REPLACE";
            this.btReplace.UseVisualStyleBackColor = true;
            this.btReplace.Click += new System.EventHandler(this.btReplace_Click);
            // 
            // btReplaceAll
            // 
            this.btReplaceAll.Location = new System.Drawing.Point(16, 155);
            this.btReplaceAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btReplaceAll.Name = "btReplaceAll";
            this.btReplaceAll.Size = new System.Drawing.Size(135, 33);
            this.btReplaceAll.TabIndex = 8;
            this.btReplaceAll.Text = "REPLACEALL";
            this.btReplaceAll.UseVisualStyleBackColor = true;
            this.btReplaceAll.Click += new System.EventHandler(this.btReplaceAll_Click);
            // 
            // tbTextToFind
            // 
            this.tbTextToFind.FormattingEnabled = true;
            this.tbTextToFind.Location = new System.Drawing.Point(144, 10);
            this.tbTextToFind.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbTextToFind.Name = "tbTextToFind";
            this.tbTextToFind.Size = new System.Drawing.Size(411, 24);
            this.tbTextToFind.TabIndex = 9;
            // 
            // tbTextToReplace
            // 
            this.tbTextToReplace.FormattingEnabled = true;
            this.tbTextToReplace.Location = new System.Drawing.Point(144, 38);
            this.tbTextToReplace.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbTextToReplace.Name = "tbTextToReplace";
            this.tbTextToReplace.Size = new System.Drawing.Size(411, 24);
            this.tbTextToReplace.TabIndex = 10;
            // 
            // FindReplaceForm
            // 
            this.AcceptButton = this.btFindNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(572, 203);
            this.Controls.Add(this.tbTextToReplace);
            this.Controls.Add(this.tbTextToFind);
            this.Controls.Add(this.btReplaceAll);
            this.Controls.Add(this.btReplace);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btFindNext);
            this.Controls.Add(this.lReplaceTo);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindReplaceForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FORMTEXT";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.FindReplaceForm_Shown);
            this.Activated += new System.EventHandler(this.FindReplaceForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FindReplaceForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lReplaceTo;
        private System.Windows.Forms.Button btFindNext;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbMathWord;
        private System.Windows.Forms.CheckBox cbMatchCase;
        private System.Windows.Forms.CheckBox cbSearchUp;
        private System.Windows.Forms.CheckBox cbUseRegex;
        private System.Windows.Forms.Button btReplace;
        private System.Windows.Forms.Button btReplaceAll;
        public System.Windows.Forms.ComboBox tbTextToFind;
        public System.Windows.Forms.ComboBox tbTextToReplace;
    }
}