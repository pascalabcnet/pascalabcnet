namespace VisualPascalABC.OptionsContent
{
    partial class EditorOptionsContent
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.nudTabIndent = new System.Windows.Forms.NumericUpDown();
            this.cbConvertTabsToSpaces = new System.Windows.Forms.CheckBox();
            this.cbShowMathBracket = new System.Windows.Forms.CheckBox();
            this.cbEditorFontSize = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbShowLinesNum = new System.Windows.Forms.CheckBox();
            this.cbEnableFolding = new System.Windows.Forms.CheckBox();
            this.cbSkipStakTraceItemIfSourceFileInSystemDirectory = new System.Windows.Forms.CheckBox();
            this.fcbFont = new VisualPascalABC.FontComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbEnableMatchOperBrackets = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudTabIndent)).BeginInit();
            this.SuspendLayout();
            // 
            // nudTabIndent
            // 
            this.nudTabIndent.Location = new System.Drawing.Point(272, 187);
            this.nudTabIndent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudTabIndent.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.nudTabIndent.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTabIndent.Name = "nudTabIndent";
            this.nudTabIndent.Size = new System.Drawing.Size(60, 22);
            this.nudTabIndent.TabIndex = 21;
            this.nudTabIndent.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbConvertTabsToSpaces
            // 
            this.cbConvertTabsToSpaces.AutoSize = true;
            this.cbConvertTabsToSpaces.Location = new System.Drawing.Point(7, 186);
            this.cbConvertTabsToSpaces.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbConvertTabsToSpaces.Name = "cbConvertTabsToSpaces";
            this.cbConvertTabsToSpaces.Size = new System.Drawing.Size(231, 21);
            this.cbConvertTabsToSpaces.TabIndex = 26;
            this.cbConvertTabsToSpaces.Text = "CONVERT_TABS_TO_SPACES";
            this.cbConvertTabsToSpaces.UseVisualStyleBackColor = true;
            // 
            // cbShowMathBracket
            // 
            this.cbShowMathBracket.AutoSize = true;
            this.cbShowMathBracket.Location = new System.Drawing.Point(7, 102);
            this.cbShowMathBracket.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbShowMathBracket.Name = "cbShowMathBracket";
            this.cbShowMathBracket.Size = new System.Drawing.Size(192, 21);
            this.cbShowMathBracket.TabIndex = 25;
            this.cbShowMathBracket.Text = "SHOW_MATH_BRACKET";
            this.cbShowMathBracket.UseVisualStyleBackColor = true;
            // 
            // cbEditorFontSize
            // 
            this.cbEditorFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEditorFontSize.FormattingEnabled = true;
            this.cbEditorFontSize.Location = new System.Drawing.Point(273, 26);
            this.cbEditorFontSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbEditorFontSize.Name = "cbEditorFontSize";
            this.cbEditorFontSize.Size = new System.Drawing.Size(143, 24);
            this.cbEditorFontSize.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(272, 6);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 17);
            this.label3.TabIndex = 22;
            this.label3.Text = "FONT_SIZE";
            // 
            // cbShowLinesNum
            // 
            this.cbShowLinesNum.AutoSize = true;
            this.cbShowLinesNum.Location = new System.Drawing.Point(7, 74);
            this.cbShowLinesNum.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbShowLinesNum.Name = "cbShowLinesNum";
            this.cbShowLinesNum.Size = new System.Drawing.Size(159, 21);
            this.cbShowLinesNum.TabIndex = 20;
            this.cbShowLinesNum.Text = "SHOW_LINES_NUM";
            this.cbShowLinesNum.UseVisualStyleBackColor = true;
            // 
            // cbEnableFolding
            // 
            this.cbEnableFolding.AutoSize = true;
            this.cbEnableFolding.Location = new System.Drawing.Point(7, 158);
            this.cbEnableFolding.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbEnableFolding.Name = "cbEnableFolding";
            this.cbEnableFolding.Size = new System.Drawing.Size(153, 21);
            this.cbEnableFolding.TabIndex = 27;
            this.cbEnableFolding.Text = "ENABLE_FOLDING";
            this.cbEnableFolding.UseVisualStyleBackColor = true;
            // 
            // cbSkipStakTraceItemIfSourceFileInSystemDirectory
            // 
            this.cbSkipStakTraceItemIfSourceFileInSystemDirectory.AutoSize = true;
            this.cbSkipStakTraceItemIfSourceFileInSystemDirectory.Location = new System.Drawing.Point(7, 214);
            this.cbSkipStakTraceItemIfSourceFileInSystemDirectory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbSkipStakTraceItemIfSourceFileInSystemDirectory.Name = "cbSkipStakTraceItemIfSourceFileInSystemDirectory";
            this.cbSkipStakTraceItemIfSourceFileInSystemDirectory.Size = new System.Drawing.Size(339, 21);
            this.cbSkipStakTraceItemIfSourceFileInSystemDirectory.TabIndex = 28;
            this.cbSkipStakTraceItemIfSourceFileInSystemDirectory.Text = "GOTO_ERRORPOS_IN_STANDART_MODULES";
            this.cbSkipStakTraceItemIfSourceFileInSystemDirectory.UseVisualStyleBackColor = true;
            // 
            // fcbFont
            // 
            this.fcbFont.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.fcbFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fcbFont.FormattingEnabled = true;
            this.fcbFont.IntegralHeight = false;
            this.fcbFont.Location = new System.Drawing.Point(7, 26);
            this.fcbFont.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.fcbFont.MaxDropDownItems = 20;
            this.fcbFont.Name = "fcbFont";
            this.fcbFont.Size = new System.Drawing.Size(257, 23);
            this.fcbFont.TabIndex = 29;
            this.fcbFont.SelectedIndexChanged += new System.EventHandler(this.FcbFontSelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 17);
            this.label1.TabIndex = 30;
            this.label1.Text = "FONT";
            // 
            // cbEnableMatchOperBrackets
            // 
            this.cbEnableMatchOperBrackets.AutoSize = true;
            this.cbEnableMatchOperBrackets.Location = new System.Drawing.Point(7, 130);
            this.cbEnableMatchOperBrackets.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbEnableMatchOperBrackets.Name = "cbEnableMatchOperBrackets";
            this.cbEnableMatchOperBrackets.Size = new System.Drawing.Size(287, 21);
            this.cbEnableMatchOperBrackets.TabIndex = 31;
            this.cbEnableMatchOperBrackets.Text = "SHOW_MATCH_OPERATOR_BRACKET";
            this.cbEnableMatchOperBrackets.UseVisualStyleBackColor = true;
            // 
            // EditorOptionsContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbEnableMatchOperBrackets);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fcbFont);
            this.Controls.Add(this.cbSkipStakTraceItemIfSourceFileInSystemDirectory);
            this.Controls.Add(this.cbEnableFolding);
            this.Controls.Add(this.nudTabIndent);
            this.Controls.Add(this.cbConvertTabsToSpaces);
            this.Controls.Add(this.cbShowMathBracket);
            this.Controls.Add(this.cbEditorFontSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbShowLinesNum);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "EditorOptionsContent";
            this.Size = new System.Drawing.Size(431, 287);
            this.Load += new System.EventHandler(this.EditorOptionsContentLoad);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.EditorOptionsContentPaint);
            ((System.ComponentModel.ISupportInitialize)(this.nudTabIndent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.CheckBox cbEnableMatchOperBrackets;
        private System.Windows.Forms.Label label1;
        private VisualPascalABC.FontComboBox fcbFont;

        #endregion

        private System.Windows.Forms.NumericUpDown nudTabIndent;
        private System.Windows.Forms.CheckBox cbConvertTabsToSpaces;
        private System.Windows.Forms.CheckBox cbShowMathBracket;
        private System.Windows.Forms.ComboBox cbEditorFontSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbShowLinesNum;
        private System.Windows.Forms.CheckBox cbEnableFolding;
        private System.Windows.Forms.CheckBox cbSkipStakTraceItemIfSourceFileInSystemDirectory;
    }
}
