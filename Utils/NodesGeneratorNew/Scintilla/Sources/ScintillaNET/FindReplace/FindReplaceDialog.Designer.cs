namespace ScintillaNET
{
    partial class FindReplaceDialog
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
            this.tabAll = new System.Windows.Forms.TabControl();
            this.tpgFind = new System.Windows.Forms.TabPage();
            this.grpFindAll = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnFindAll = new System.Windows.Forms.Button();
            this.chkHighlightMatches = new System.Windows.Forms.CheckBox();
            this.chkMarkLine = new System.Windows.Forms.CheckBox();
            this.chkSearchSelectionF = new System.Windows.Forms.CheckBox();
            this.chkWrapF = new System.Windows.Forms.CheckBox();
            this.btnFindPrevious = new System.Windows.Forms.Button();
            this.btnFindNext = new System.Windows.Forms.Button();
            this.cboFindF = new System.Windows.Forms.ComboBox();
            this.grpOptionsF = new System.Windows.Forms.GroupBox();
            this.pnlStandardOptionsF = new System.Windows.Forms.Panel();
            this.chkWordStartF = new System.Windows.Forms.CheckBox();
            this.chkWholeWordF = new System.Windows.Forms.CheckBox();
            this.chkMatchCaseF = new System.Windows.Forms.CheckBox();
            this.pnlRegexpOptionsF = new System.Windows.Forms.Panel();
            this.chkSinglelineF = new System.Windows.Forms.CheckBox();
            this.chkRightToLeftF = new System.Windows.Forms.CheckBox();
            this.chkMultilineF = new System.Windows.Forms.CheckBox();
            this.chkIgnorePatternWhitespaceF = new System.Windows.Forms.CheckBox();
            this.chkIgnoreCaseF = new System.Windows.Forms.CheckBox();
            this.chkExplicitCaptureF = new System.Windows.Forms.CheckBox();
            this.chkEcmaScriptF = new System.Windows.Forms.CheckBox();
            this.chkCultureInvariantF = new System.Windows.Forms.CheckBox();
            this.chkCompiledF = new System.Windows.Forms.CheckBox();
            this.lblSearchTypeF = new System.Windows.Forms.Label();
            this.rdoRegexF = new System.Windows.Forms.RadioButton();
            this.rdoStandardF = new System.Windows.Forms.RadioButton();
            this.lblFindF = new System.Windows.Forms.Label();
            this.tpgReplace = new System.Windows.Forms.TabPage();
            this.btnReplaceAll = new System.Windows.Forms.Button();
            this.cboReplace = new System.Windows.Forms.ComboBox();
            this.lblReplace = new System.Windows.Forms.Label();
            this.chkSearchSelectionR = new System.Windows.Forms.CheckBox();
            this.chkWrapR = new System.Windows.Forms.CheckBox();
            this.btnReplacePrevious = new System.Windows.Forms.Button();
            this.btnReplaceNext = new System.Windows.Forms.Button();
            this.cboFindR = new System.Windows.Forms.ComboBox();
            this.grdOptionsR = new System.Windows.Forms.GroupBox();
            this.pnlStandardOptionsR = new System.Windows.Forms.Panel();
            this.chkWordStartR = new System.Windows.Forms.CheckBox();
            this.chkWholeWordR = new System.Windows.Forms.CheckBox();
            this.chkMatchCaseR = new System.Windows.Forms.CheckBox();
            this.pnlRegexpOptionsR = new System.Windows.Forms.Panel();
            this.chkSinglelineR = new System.Windows.Forms.CheckBox();
            this.chkRightToLeftR = new System.Windows.Forms.CheckBox();
            this.chkMultilineR = new System.Windows.Forms.CheckBox();
            this.chkIgnorePatternWhitespaceR = new System.Windows.Forms.CheckBox();
            this.chkIgnoreCaseR = new System.Windows.Forms.CheckBox();
            this.chkExplicitCaptureR = new System.Windows.Forms.CheckBox();
            this.chkEcmaScriptR = new System.Windows.Forms.CheckBox();
            this.chkCultureInvariantR = new System.Windows.Forms.CheckBox();
            this.chkCompiledR = new System.Windows.Forms.CheckBox();
            this.lblSearchTypeR = new System.Windows.Forms.Label();
            this.rdoRegexR = new System.Windows.Forms.RadioButton();
            this.rdoStandardR = new System.Windows.Forms.RadioButton();
            this.lblFindR = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabAll.SuspendLayout();
            this.tpgFind.SuspendLayout();
            this.grpFindAll.SuspendLayout();
            this.grpOptionsF.SuspendLayout();
            this.pnlStandardOptionsF.SuspendLayout();
            this.pnlRegexpOptionsF.SuspendLayout();
            this.tpgReplace.SuspendLayout();
            this.grdOptionsR.SuspendLayout();
            this.pnlStandardOptionsR.SuspendLayout();
            this.pnlRegexpOptionsR.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabAll
            // 
            this.tabAll.Controls.Add(this.tpgFind);
            this.tabAll.Controls.Add(this.tpgReplace);
            this.tabAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabAll.Location = new System.Drawing.Point(0, 0);
            this.tabAll.Name = "tabAll";
            this.tabAll.SelectedIndex = 0;
            this.tabAll.Size = new System.Drawing.Size(499, 291);
            this.tabAll.TabIndex = 0;
            this.tabAll.SelectedIndexChanged += new System.EventHandler(this.tabAll_SelectedIndexChanged);
            // 
            // tpgFind
            // 
            this.tpgFind.Controls.Add(this.grpFindAll);
            this.tpgFind.Controls.Add(this.chkSearchSelectionF);
            this.tpgFind.Controls.Add(this.chkWrapF);
            this.tpgFind.Controls.Add(this.btnFindPrevious);
            this.tpgFind.Controls.Add(this.btnFindNext);
            this.tpgFind.Controls.Add(this.cboFindF);
            this.tpgFind.Controls.Add(this.grpOptionsF);
            this.tpgFind.Controls.Add(this.lblSearchTypeF);
            this.tpgFind.Controls.Add(this.rdoRegexF);
            this.tpgFind.Controls.Add(this.rdoStandardF);
            this.tpgFind.Controls.Add(this.lblFindF);
            this.tpgFind.Location = new System.Drawing.Point(4, 22);
            this.tpgFind.Name = "tpgFind";
            this.tpgFind.Padding = new System.Windows.Forms.Padding(3);
            this.tpgFind.Size = new System.Drawing.Size(491, 265);
            this.tpgFind.TabIndex = 0;
            this.tpgFind.Text = "Find";
            this.tpgFind.UseVisualStyleBackColor = true;
            // 
            // grpFindAll
            // 
            this.grpFindAll.Controls.Add(this.btnClear);
            this.grpFindAll.Controls.Add(this.btnFindAll);
            this.grpFindAll.Controls.Add(this.chkHighlightMatches);
            this.grpFindAll.Controls.Add(this.chkMarkLine);
            this.grpFindAll.Location = new System.Drawing.Point(5, 176);
            this.grpFindAll.Name = "grpFindAll";
            this.grpFindAll.Size = new System.Drawing.Size(209, 65);
            this.grpFindAll.TabIndex = 8;
            this.grpFindAll.TabStop = false;
            this.grpFindAll.Text = "Find All";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(116, 37);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(88, 23);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "C&lear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnFindAll
            // 
            this.btnFindAll.Location = new System.Drawing.Point(116, 13);
            this.btnFindAll.Name = "btnFindAll";
            this.btnFindAll.Size = new System.Drawing.Size(88, 23);
            this.btnFindAll.TabIndex = 2;
            this.btnFindAll.Text = "Find &All";
            this.btnFindAll.UseVisualStyleBackColor = true;
            this.btnFindAll.Click += new System.EventHandler(this.btnFindAll_Click);
            // 
            // chkHighlightMatches
            // 
            this.chkHighlightMatches.AutoSize = true;
            this.chkHighlightMatches.Location = new System.Drawing.Point(6, 37);
            this.chkHighlightMatches.Name = "chkHighlightMatches";
            this.chkHighlightMatches.Size = new System.Drawing.Size(111, 17);
            this.chkHighlightMatches.TabIndex = 1;
            this.chkHighlightMatches.Text = "&Highlight Matches";
            this.chkHighlightMatches.UseVisualStyleBackColor = true;
            // 
            // chkMarkLine
            // 
            this.chkMarkLine.AutoSize = true;
            this.chkMarkLine.Location = new System.Drawing.Point(6, 20);
            this.chkMarkLine.Name = "chkMarkLine";
            this.chkMarkLine.Size = new System.Drawing.Size(73, 17);
            this.chkMarkLine.TabIndex = 0;
            this.chkMarkLine.Text = "&Mark Line";
            this.chkMarkLine.UseVisualStyleBackColor = true;
            // 
            // chkSearchSelectionF
            // 
            this.chkSearchSelectionF.AutoSize = true;
            this.chkSearchSelectionF.Location = new System.Drawing.Point(251, 72);
            this.chkSearchSelectionF.Name = "chkSearchSelectionF";
            this.chkSearchSelectionF.Size = new System.Drawing.Size(105, 17);
            this.chkSearchSelectionF.TabIndex = 6;
            this.chkSearchSelectionF.Text = "Search Selection";
            this.chkSearchSelectionF.UseVisualStyleBackColor = true;
            // 
            // chkWrapF
            // 
            this.chkWrapF.AutoSize = true;
            this.chkWrapF.Checked = true;
            this.chkWrapF.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWrapF.Location = new System.Drawing.Point(251, 55);
            this.chkWrapF.Name = "chkWrapF";
            this.chkWrapF.Size = new System.Drawing.Size(52, 17);
            this.chkWrapF.TabIndex = 5;
            this.chkWrapF.Text = "&Wrap";
            this.chkWrapF.UseVisualStyleBackColor = true;
            // 
            // btnFindPrevious
            // 
            this.btnFindPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindPrevious.Location = new System.Drawing.Point(375, 188);
            this.btnFindPrevious.Name = "btnFindPrevious";
            this.btnFindPrevious.Size = new System.Drawing.Size(107, 23);
            this.btnFindPrevious.TabIndex = 9;
            this.btnFindPrevious.Text = "Find &Previous";
            this.btnFindPrevious.UseVisualStyleBackColor = true;
            this.btnFindPrevious.Click += new System.EventHandler(this.btnFindPrevious_Click);
            // 
            // btnFindNext
            // 
            this.btnFindNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNext.Location = new System.Drawing.Point(375, 212);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new System.Drawing.Size(107, 23);
            this.btnFindNext.TabIndex = 10;
            this.btnFindNext.Text = "Find &Next";
            this.btnFindNext.UseVisualStyleBackColor = true;
            this.btnFindNext.Click += new System.EventHandler(this.btnFindNext_Click);
            // 
            // cboFindF
            // 
            this.cboFindF.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFindF.FormattingEnabled = true;
            this.cboFindF.Location = new System.Drawing.Point(59, 6);
            this.cboFindF.Name = "cboFindF";
            this.cboFindF.Size = new System.Drawing.Size(424, 21);
            this.cboFindF.TabIndex = 1;
            // 
            // grpOptionsF
            // 
            this.grpOptionsF.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpOptionsF.Controls.Add(this.pnlStandardOptionsF);
            this.grpOptionsF.Controls.Add(this.pnlRegexpOptionsF);
            this.grpOptionsF.Location = new System.Drawing.Point(4, 94);
            this.grpOptionsF.Name = "grpOptionsF";
            this.grpOptionsF.Size = new System.Drawing.Size(481, 77);
            this.grpOptionsF.TabIndex = 7;
            this.grpOptionsF.TabStop = false;
            this.grpOptionsF.Text = "Options";
            // 
            // pnlStandardOptionsF
            // 
            this.pnlStandardOptionsF.Controls.Add(this.chkWordStartF);
            this.pnlStandardOptionsF.Controls.Add(this.chkWholeWordF);
            this.pnlStandardOptionsF.Controls.Add(this.chkMatchCaseF);
            this.pnlStandardOptionsF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStandardOptionsF.Location = new System.Drawing.Point(3, 17);
            this.pnlStandardOptionsF.Name = "pnlStandardOptionsF";
            this.pnlStandardOptionsF.Size = new System.Drawing.Size(475, 57);
            this.pnlStandardOptionsF.TabIndex = 0;
            // 
            // chkWordStartF
            // 
            this.chkWordStartF.AutoSize = true;
            this.chkWordStartF.Location = new System.Drawing.Point(10, 37);
            this.chkWordStartF.Name = "chkWordStartF";
            this.chkWordStartF.Size = new System.Drawing.Size(79, 17);
            this.chkWordStartF.TabIndex = 2;
            this.chkWordStartF.Text = "W&ord Start";
            this.chkWordStartF.UseVisualStyleBackColor = true;
            // 
            // chkWholeWordF
            // 
            this.chkWholeWordF.AutoSize = true;
            this.chkWholeWordF.Location = new System.Drawing.Point(10, 20);
            this.chkWholeWordF.Name = "chkWholeWordF";
            this.chkWholeWordF.Size = new System.Drawing.Size(85, 17);
            this.chkWholeWordF.TabIndex = 1;
            this.chkWholeWordF.Text = "Whole Wor&d";
            this.chkWholeWordF.UseVisualStyleBackColor = true;
            // 
            // chkMatchCaseF
            // 
            this.chkMatchCaseF.AutoSize = true;
            this.chkMatchCaseF.Location = new System.Drawing.Point(10, 3);
            this.chkMatchCaseF.Name = "chkMatchCaseF";
            this.chkMatchCaseF.Size = new System.Drawing.Size(82, 17);
            this.chkMatchCaseF.TabIndex = 0;
            this.chkMatchCaseF.Text = "Match &Case";
            this.chkMatchCaseF.UseVisualStyleBackColor = true;
            // 
            // pnlRegexpOptionsF
            // 
            this.pnlRegexpOptionsF.Controls.Add(this.chkSinglelineF);
            this.pnlRegexpOptionsF.Controls.Add(this.chkRightToLeftF);
            this.pnlRegexpOptionsF.Controls.Add(this.chkMultilineF);
            this.pnlRegexpOptionsF.Controls.Add(this.chkIgnorePatternWhitespaceF);
            this.pnlRegexpOptionsF.Controls.Add(this.chkIgnoreCaseF);
            this.pnlRegexpOptionsF.Controls.Add(this.chkExplicitCaptureF);
            this.pnlRegexpOptionsF.Controls.Add(this.chkEcmaScriptF);
            this.pnlRegexpOptionsF.Controls.Add(this.chkCultureInvariantF);
            this.pnlRegexpOptionsF.Controls.Add(this.chkCompiledF);
            this.pnlRegexpOptionsF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRegexpOptionsF.Location = new System.Drawing.Point(3, 17);
            this.pnlRegexpOptionsF.Name = "pnlRegexpOptionsF";
            this.pnlRegexpOptionsF.Size = new System.Drawing.Size(475, 57);
            this.pnlRegexpOptionsF.TabIndex = 1;
            // 
            // chkSinglelineF
            // 
            this.chkSinglelineF.AutoSize = true;
            this.chkSinglelineF.Location = new System.Drawing.Point(279, 37);
            this.chkSinglelineF.Name = "chkSinglelineF";
            this.chkSinglelineF.Size = new System.Drawing.Size(70, 17);
            this.chkSinglelineF.TabIndex = 8;
            this.chkSinglelineF.Text = "Singleline";
            this.chkSinglelineF.UseVisualStyleBackColor = true;
            // 
            // chkRightToLeftF
            // 
            this.chkRightToLeftF.AutoSize = true;
            this.chkRightToLeftF.Location = new System.Drawing.Point(279, 20);
            this.chkRightToLeftF.Name = "chkRightToLeftF";
            this.chkRightToLeftF.Size = new System.Drawing.Size(88, 17);
            this.chkRightToLeftF.TabIndex = 5;
            this.chkRightToLeftF.Text = "Right To Left";
            this.chkRightToLeftF.UseVisualStyleBackColor = true;
            // 
            // chkMultilineF
            // 
            this.chkMultilineF.AutoSize = true;
            this.chkMultilineF.Location = new System.Drawing.Point(279, 3);
            this.chkMultilineF.Name = "chkMultilineF";
            this.chkMultilineF.Size = new System.Drawing.Size(64, 17);
            this.chkMultilineF.TabIndex = 2;
            this.chkMultilineF.Text = "Multiline";
            this.chkMultilineF.UseVisualStyleBackColor = true;
            // 
            // chkIgnorePatternWhitespaceF
            // 
            this.chkIgnorePatternWhitespaceF.AutoSize = true;
            this.chkIgnorePatternWhitespaceF.Location = new System.Drawing.Point(113, 37);
            this.chkIgnorePatternWhitespaceF.Name = "chkIgnorePatternWhitespaceF";
            this.chkIgnorePatternWhitespaceF.Size = new System.Drawing.Size(156, 17);
            this.chkIgnorePatternWhitespaceF.TabIndex = 7;
            this.chkIgnorePatternWhitespaceF.Text = "I&gnore Pattern Whitespace";
            this.chkIgnorePatternWhitespaceF.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreCaseF
            // 
            this.chkIgnoreCaseF.AutoSize = true;
            this.chkIgnoreCaseF.Location = new System.Drawing.Point(113, 20);
            this.chkIgnoreCaseF.Name = "chkIgnoreCaseF";
            this.chkIgnoreCaseF.Size = new System.Drawing.Size(85, 17);
            this.chkIgnoreCaseF.TabIndex = 4;
            this.chkIgnoreCaseF.Text = "&Ignore Case";
            this.chkIgnoreCaseF.UseVisualStyleBackColor = true;
            // 
            // chkExplicitCaptureF
            // 
            this.chkExplicitCaptureF.AutoSize = true;
            this.chkExplicitCaptureF.Location = new System.Drawing.Point(113, 3);
            this.chkExplicitCaptureF.Name = "chkExplicitCaptureF";
            this.chkExplicitCaptureF.Size = new System.Drawing.Size(101, 17);
            this.chkExplicitCaptureF.TabIndex = 1;
            this.chkExplicitCaptureF.Text = "E&xplicit Capture";
            this.chkExplicitCaptureF.UseVisualStyleBackColor = true;
            // 
            // chkEcmaScriptF
            // 
            this.chkEcmaScriptF.AutoSize = true;
            this.chkEcmaScriptF.Location = new System.Drawing.Point(3, 37);
            this.chkEcmaScriptF.Name = "chkEcmaScriptF";
            this.chkEcmaScriptF.Size = new System.Drawing.Size(84, 17);
            this.chkEcmaScriptF.TabIndex = 6;
            this.chkEcmaScriptF.Text = "ECMA Script";
            this.chkEcmaScriptF.UseVisualStyleBackColor = true;
            this.chkEcmaScriptF.CheckedChanged += new System.EventHandler(this.chkEcmaScript_CheckedChanged);
            // 
            // chkCultureInvariantF
            // 
            this.chkCultureInvariantF.AutoSize = true;
            this.chkCultureInvariantF.Location = new System.Drawing.Point(3, 20);
            this.chkCultureInvariantF.Name = "chkCultureInvariantF";
            this.chkCultureInvariantF.Size = new System.Drawing.Size(108, 17);
            this.chkCultureInvariantF.TabIndex = 3;
            this.chkCultureInvariantF.Text = "C&ulture Invariant";
            this.chkCultureInvariantF.UseVisualStyleBackColor = true;
            // 
            // chkCompiledF
            // 
            this.chkCompiledF.AutoSize = true;
            this.chkCompiledF.Location = new System.Drawing.Point(3, 3);
            this.chkCompiledF.Name = "chkCompiledF";
            this.chkCompiledF.Size = new System.Drawing.Size(69, 17);
            this.chkCompiledF.TabIndex = 0;
            this.chkCompiledF.Text = "&Compiled";
            this.chkCompiledF.UseVisualStyleBackColor = true;
            // 
            // lblSearchTypeF
            // 
            this.lblSearchTypeF.AutoSize = true;
            this.lblSearchTypeF.Location = new System.Drawing.Point(8, 52);
            this.lblSearchTypeF.Name = "lblSearchTypeF";
            this.lblSearchTypeF.Size = new System.Drawing.Size(67, 13);
            this.lblSearchTypeF.TabIndex = 2;
            this.lblSearchTypeF.Text = "Search Type";
            // 
            // rdoRegexF
            // 
            this.rdoRegexF.AutoSize = true;
            this.rdoRegexF.Location = new System.Drawing.Point(102, 71);
            this.rdoRegexF.Name = "rdoRegexF";
            this.rdoRegexF.Size = new System.Drawing.Size(117, 17);
            this.rdoRegexF.TabIndex = 4;
            this.rdoRegexF.Text = "Regular &Expression";
            this.rdoRegexF.UseVisualStyleBackColor = true;
            this.rdoRegexF.CheckedChanged += new System.EventHandler(this.rdoStandardF_CheckedChanged);
            // 
            // rdoStandardF
            // 
            this.rdoStandardF.AutoSize = true;
            this.rdoStandardF.Checked = true;
            this.rdoStandardF.Location = new System.Drawing.Point(27, 71);
            this.rdoStandardF.Name = "rdoStandardF";
            this.rdoStandardF.Size = new System.Drawing.Size(69, 17);
            this.rdoStandardF.TabIndex = 3;
            this.rdoStandardF.TabStop = true;
            this.rdoStandardF.Text = "&Standard";
            this.rdoStandardF.UseVisualStyleBackColor = true;
            this.rdoStandardF.CheckedChanged += new System.EventHandler(this.rdoStandardF_CheckedChanged);
            // 
            // lblFindF
            // 
            this.lblFindF.AutoSize = true;
            this.lblFindF.Location = new System.Drawing.Point(8, 10);
            this.lblFindF.Name = "lblFindF";
            this.lblFindF.Size = new System.Drawing.Size(27, 13);
            this.lblFindF.TabIndex = 0;
            this.lblFindF.Text = "&Find";
            // 
            // tpgReplace
            // 
            this.tpgReplace.Controls.Add(this.btnReplaceAll);
            this.tpgReplace.Controls.Add(this.cboReplace);
            this.tpgReplace.Controls.Add(this.lblReplace);
            this.tpgReplace.Controls.Add(this.chkSearchSelectionR);
            this.tpgReplace.Controls.Add(this.chkWrapR);
            this.tpgReplace.Controls.Add(this.btnReplacePrevious);
            this.tpgReplace.Controls.Add(this.btnReplaceNext);
            this.tpgReplace.Controls.Add(this.cboFindR);
            this.tpgReplace.Controls.Add(this.grdOptionsR);
            this.tpgReplace.Controls.Add(this.lblSearchTypeR);
            this.tpgReplace.Controls.Add(this.rdoRegexR);
            this.tpgReplace.Controls.Add(this.rdoStandardR);
            this.tpgReplace.Controls.Add(this.lblFindR);
            this.tpgReplace.Location = new System.Drawing.Point(4, 22);
            this.tpgReplace.Name = "tpgReplace";
            this.tpgReplace.Padding = new System.Windows.Forms.Padding(3);
            this.tpgReplace.Size = new System.Drawing.Size(491, 265);
            this.tpgReplace.TabIndex = 1;
            this.tpgReplace.Text = "Replace";
            this.tpgReplace.UseVisualStyleBackColor = true;
            // 
            // btnReplaceAll
            // 
            this.btnReplaceAll.Location = new System.Drawing.Point(7, 212);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new System.Drawing.Size(107, 23);
            this.btnReplaceAll.TabIndex = 10;
            this.btnReplaceAll.Text = "Replace &All";
            this.btnReplaceAll.UseVisualStyleBackColor = true;
            this.btnReplaceAll.Click += new System.EventHandler(this.btnReplaceAll_Click);
            // 
            // cboReplace
            // 
            this.cboReplace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboReplace.FormattingEnabled = true;
            this.cboReplace.Location = new System.Drawing.Point(59, 28);
            this.cboReplace.Name = "cboReplace";
            this.cboReplace.Size = new System.Drawing.Size(424, 21);
            this.cboReplace.TabIndex = 3;
            // 
            // lblReplace
            // 
            this.lblReplace.AutoSize = true;
            this.lblReplace.Location = new System.Drawing.Point(8, 32);
            this.lblReplace.Name = "lblReplace";
            this.lblReplace.Size = new System.Drawing.Size(45, 13);
            this.lblReplace.TabIndex = 2;
            this.lblReplace.Text = "&Replace";
            // 
            // chkSearchSelectionR
            // 
            this.chkSearchSelectionR.AutoSize = true;
            this.chkSearchSelectionR.Location = new System.Drawing.Point(251, 72);
            this.chkSearchSelectionR.Name = "chkSearchSelectionR";
            this.chkSearchSelectionR.Size = new System.Drawing.Size(107, 17);
            this.chkSearchSelectionR.TabIndex = 8;
            this.chkSearchSelectionR.Text = "Search Selection";
            this.chkSearchSelectionR.UseVisualStyleBackColor = true;
            // 
            // chkWrapR
            // 
            this.chkWrapR.AutoSize = true;
            this.chkWrapR.Checked = true;
            this.chkWrapR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWrapR.Location = new System.Drawing.Point(251, 55);
            this.chkWrapR.Name = "chkWrapR";
            this.chkWrapR.Size = new System.Drawing.Size(52, 17);
            this.chkWrapR.TabIndex = 7;
            this.chkWrapR.Text = "&Wrap";
            this.chkWrapR.UseVisualStyleBackColor = true;
            // 
            // btnReplacePrevious
            // 
            this.btnReplacePrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReplacePrevious.Location = new System.Drawing.Point(375, 188);
            this.btnReplacePrevious.Name = "btnReplacePrevious";
            this.btnReplacePrevious.Size = new System.Drawing.Size(107, 23);
            this.btnReplacePrevious.TabIndex = 11;
            this.btnReplacePrevious.Text = "Replace &Previous";
            this.btnReplacePrevious.UseVisualStyleBackColor = true;
            this.btnReplacePrevious.Click += new System.EventHandler(this.btnReplacePrevious_Click);
            // 
            // btnReplaceNext
            // 
            this.btnReplaceNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReplaceNext.Location = new System.Drawing.Point(375, 212);
            this.btnReplaceNext.Name = "btnReplaceNext";
            this.btnReplaceNext.Size = new System.Drawing.Size(107, 23);
            this.btnReplaceNext.TabIndex = 12;
            this.btnReplaceNext.Text = "Replace &Next";
            this.btnReplaceNext.UseVisualStyleBackColor = true;
            this.btnReplaceNext.Click += new System.EventHandler(this.btnReplaceNext_Click);
            // 
            // cboFindR
            // 
            this.cboFindR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFindR.FormattingEnabled = true;
            this.cboFindR.Location = new System.Drawing.Point(59, 6);
            this.cboFindR.Name = "cboFindR";
            this.cboFindR.Size = new System.Drawing.Size(424, 21);
            this.cboFindR.TabIndex = 1;
            // 
            // grdOptionsR
            // 
            this.grdOptionsR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdOptionsR.Controls.Add(this.pnlStandardOptionsR);
            this.grdOptionsR.Controls.Add(this.pnlRegexpOptionsR);
            this.grdOptionsR.Location = new System.Drawing.Point(4, 94);
            this.grdOptionsR.Name = "grdOptionsR";
            this.grdOptionsR.Size = new System.Drawing.Size(481, 77);
            this.grdOptionsR.TabIndex = 9;
            this.grdOptionsR.TabStop = false;
            this.grdOptionsR.Text = "Options";
            // 
            // pnlStandardOptionsR
            // 
            this.pnlStandardOptionsR.Controls.Add(this.chkWordStartR);
            this.pnlStandardOptionsR.Controls.Add(this.chkWholeWordR);
            this.pnlStandardOptionsR.Controls.Add(this.chkMatchCaseR);
            this.pnlStandardOptionsR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStandardOptionsR.Location = new System.Drawing.Point(3, 17);
            this.pnlStandardOptionsR.Name = "pnlStandardOptionsR";
            this.pnlStandardOptionsR.Size = new System.Drawing.Size(475, 57);
            this.pnlStandardOptionsR.TabIndex = 0;
            // 
            // chkWordStartR
            // 
            this.chkWordStartR.AutoSize = true;
            this.chkWordStartR.Location = new System.Drawing.Point(10, 37);
            this.chkWordStartR.Name = "chkWordStartR";
            this.chkWordStartR.Size = new System.Drawing.Size(77, 17);
            this.chkWordStartR.TabIndex = 2;
            this.chkWordStartR.Text = "W&ord Start";
            this.chkWordStartR.UseVisualStyleBackColor = true;
            // 
            // chkWholeWordR
            // 
            this.chkWholeWordR.AutoSize = true;
            this.chkWholeWordR.Location = new System.Drawing.Point(10, 20);
            this.chkWholeWordR.Name = "chkWholeWordR";
            this.chkWholeWordR.Size = new System.Drawing.Size(86, 17);
            this.chkWholeWordR.TabIndex = 1;
            this.chkWholeWordR.Text = "Whole Wor&d";
            this.chkWholeWordR.UseVisualStyleBackColor = true;
            // 
            // chkMatchCaseR
            // 
            this.chkMatchCaseR.AutoSize = true;
            this.chkMatchCaseR.Location = new System.Drawing.Point(10, 3);
            this.chkMatchCaseR.Name = "chkMatchCaseR";
            this.chkMatchCaseR.Size = new System.Drawing.Size(83, 17);
            this.chkMatchCaseR.TabIndex = 0;
            this.chkMatchCaseR.Text = "Match &Case";
            this.chkMatchCaseR.UseVisualStyleBackColor = true;
            // 
            // pnlRegexpOptionsR
            // 
            this.pnlRegexpOptionsR.Controls.Add(this.chkSinglelineR);
            this.pnlRegexpOptionsR.Controls.Add(this.chkRightToLeftR);
            this.pnlRegexpOptionsR.Controls.Add(this.chkMultilineR);
            this.pnlRegexpOptionsR.Controls.Add(this.chkIgnorePatternWhitespaceR);
            this.pnlRegexpOptionsR.Controls.Add(this.chkIgnoreCaseR);
            this.pnlRegexpOptionsR.Controls.Add(this.chkExplicitCaptureR);
            this.pnlRegexpOptionsR.Controls.Add(this.chkEcmaScriptR);
            this.pnlRegexpOptionsR.Controls.Add(this.chkCultureInvariantR);
            this.pnlRegexpOptionsR.Controls.Add(this.chkCompiledR);
            this.pnlRegexpOptionsR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRegexpOptionsR.Location = new System.Drawing.Point(3, 17);
            this.pnlRegexpOptionsR.Name = "pnlRegexpOptionsR";
            this.pnlRegexpOptionsR.Size = new System.Drawing.Size(475, 57);
            this.pnlRegexpOptionsR.TabIndex = 1;
            // 
            // chkSinglelineR
            // 
            this.chkSinglelineR.AutoSize = true;
            this.chkSinglelineR.Location = new System.Drawing.Point(279, 37);
            this.chkSinglelineR.Name = "chkSinglelineR";
            this.chkSinglelineR.Size = new System.Drawing.Size(71, 17);
            this.chkSinglelineR.TabIndex = 8;
            this.chkSinglelineR.Text = "Singleline";
            this.chkSinglelineR.UseVisualStyleBackColor = true;
            // 
            // chkRightToLeftR
            // 
            this.chkRightToLeftR.AutoSize = true;
            this.chkRightToLeftR.Location = new System.Drawing.Point(279, 20);
            this.chkRightToLeftR.Name = "chkRightToLeftR";
            this.chkRightToLeftR.Size = new System.Drawing.Size(88, 17);
            this.chkRightToLeftR.TabIndex = 7;
            this.chkRightToLeftR.Text = "Right To Left";
            this.chkRightToLeftR.UseVisualStyleBackColor = true;
            // 
            // chkMultilineR
            // 
            this.chkMultilineR.AutoSize = true;
            this.chkMultilineR.Location = new System.Drawing.Point(279, 3);
            this.chkMultilineR.Name = "chkMultilineR";
            this.chkMultilineR.Size = new System.Drawing.Size(64, 17);
            this.chkMultilineR.TabIndex = 6;
            this.chkMultilineR.Text = "Multiline";
            this.chkMultilineR.UseVisualStyleBackColor = true;
            // 
            // chkIgnorePatternWhitespaceR
            // 
            this.chkIgnorePatternWhitespaceR.AutoSize = true;
            this.chkIgnorePatternWhitespaceR.Location = new System.Drawing.Point(113, 37);
            this.chkIgnorePatternWhitespaceR.Name = "chkIgnorePatternWhitespaceR";
            this.chkIgnorePatternWhitespaceR.Size = new System.Drawing.Size(153, 17);
            this.chkIgnorePatternWhitespaceR.TabIndex = 5;
            this.chkIgnorePatternWhitespaceR.Text = "I&gnore Pattern Whitespace";
            this.chkIgnorePatternWhitespaceR.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreCaseR
            // 
            this.chkIgnoreCaseR.AutoSize = true;
            this.chkIgnoreCaseR.Location = new System.Drawing.Point(113, 20);
            this.chkIgnoreCaseR.Name = "chkIgnoreCaseR";
            this.chkIgnoreCaseR.Size = new System.Drawing.Size(83, 17);
            this.chkIgnoreCaseR.TabIndex = 4;
            this.chkIgnoreCaseR.Text = "&Ignore Case";
            this.chkIgnoreCaseR.UseVisualStyleBackColor = true;
            // 
            // chkExplicitCaptureR
            // 
            this.chkExplicitCaptureR.AutoSize = true;
            this.chkExplicitCaptureR.Location = new System.Drawing.Point(113, 3);
            this.chkExplicitCaptureR.Name = "chkExplicitCaptureR";
            this.chkExplicitCaptureR.Size = new System.Drawing.Size(99, 17);
            this.chkExplicitCaptureR.TabIndex = 3;
            this.chkExplicitCaptureR.Text = "E&xplicit Capture";
            this.chkExplicitCaptureR.UseVisualStyleBackColor = true;
            // 
            // chkEcmaScriptR
            // 
            this.chkEcmaScriptR.AutoSize = true;
            this.chkEcmaScriptR.Location = new System.Drawing.Point(3, 37);
            this.chkEcmaScriptR.Name = "chkEcmaScriptR";
            this.chkEcmaScriptR.Size = new System.Drawing.Size(86, 17);
            this.chkEcmaScriptR.TabIndex = 2;
            this.chkEcmaScriptR.Text = "ECMA Script";
            this.chkEcmaScriptR.UseVisualStyleBackColor = true;
            this.chkEcmaScriptR.CheckedChanged += new System.EventHandler(this.chkEcmaScript_CheckedChanged);
            // 
            // chkCultureInvariantR
            // 
            this.chkCultureInvariantR.AutoSize = true;
            this.chkCultureInvariantR.Location = new System.Drawing.Point(3, 20);
            this.chkCultureInvariantR.Name = "chkCultureInvariantR";
            this.chkCultureInvariantR.Size = new System.Drawing.Size(103, 17);
            this.chkCultureInvariantR.TabIndex = 1;
            this.chkCultureInvariantR.Text = "C&ulture Invariant";
            this.chkCultureInvariantR.UseVisualStyleBackColor = true;
            // 
            // chkCompiledR
            // 
            this.chkCompiledR.AutoSize = true;
            this.chkCompiledR.Location = new System.Drawing.Point(3, 3);
            this.chkCompiledR.Name = "chkCompiledR";
            this.chkCompiledR.Size = new System.Drawing.Size(69, 17);
            this.chkCompiledR.TabIndex = 0;
            this.chkCompiledR.Text = "&Compiled";
            this.chkCompiledR.UseVisualStyleBackColor = true;
            // 
            // lblSearchTypeR
            // 
            this.lblSearchTypeR.AutoSize = true;
            this.lblSearchTypeR.Location = new System.Drawing.Point(8, 52);
            this.lblSearchTypeR.Name = "lblSearchTypeR";
            this.lblSearchTypeR.Size = new System.Drawing.Size(67, 13);
            this.lblSearchTypeR.TabIndex = 4;
            this.lblSearchTypeR.Text = "Search Type";
            // 
            // rdoRegexR
            // 
            this.rdoRegexR.AutoSize = true;
            this.rdoRegexR.Location = new System.Drawing.Point(102, 71);
            this.rdoRegexR.Name = "rdoRegexR";
            this.rdoRegexR.Size = new System.Drawing.Size(116, 17);
            this.rdoRegexR.TabIndex = 6;
            this.rdoRegexR.Text = "Regular &Expression";
            this.rdoRegexR.UseVisualStyleBackColor = true;
            this.rdoRegexR.CheckedChanged += new System.EventHandler(this.rdoStandardR_CheckedChanged);
            // 
            // rdoStandardR
            // 
            this.rdoStandardR.AutoSize = true;
            this.rdoStandardR.Checked = true;
            this.rdoStandardR.Location = new System.Drawing.Point(27, 71);
            this.rdoStandardR.Name = "rdoStandardR";
            this.rdoStandardR.Size = new System.Drawing.Size(68, 17);
            this.rdoStandardR.TabIndex = 5;
            this.rdoStandardR.TabStop = true;
            this.rdoStandardR.Text = "&Standard";
            this.rdoStandardR.UseVisualStyleBackColor = true;
            this.rdoStandardR.CheckedChanged += new System.EventHandler(this.rdoStandardR_CheckedChanged);
            // 
            // lblFindR
            // 
            this.lblFindR.AutoSize = true;
            this.lblFindR.Location = new System.Drawing.Point(8, 10);
            this.lblFindR.Name = "lblFindR";
            this.lblFindR.Size = new System.Drawing.Size(27, 13);
            this.lblFindR.TabIndex = 0;
            this.lblFindR.Text = "&Find";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 269);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(499, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "xxx";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // FindReplaceDialog
            // 
            this.AcceptButton = this.btnFindNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 291);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabAll);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindReplaceDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Find and Replace";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FindReplaceDialog_FormClosing);
            this.tabAll.ResumeLayout(false);
            this.tpgFind.ResumeLayout(false);
            this.tpgFind.PerformLayout();
            this.grpFindAll.ResumeLayout(false);
            this.grpFindAll.PerformLayout();
            this.grpOptionsF.ResumeLayout(false);
            this.pnlStandardOptionsF.ResumeLayout(false);
            this.pnlStandardOptionsF.PerformLayout();
            this.pnlRegexpOptionsF.ResumeLayout(false);
            this.pnlRegexpOptionsF.PerformLayout();
            this.tpgReplace.ResumeLayout(false);
            this.tpgReplace.PerformLayout();
            this.grdOptionsR.ResumeLayout(false);
            this.pnlStandardOptionsR.ResumeLayout(false);
            this.pnlStandardOptionsR.PerformLayout();
            this.pnlRegexpOptionsR.ResumeLayout(false);
            this.pnlRegexpOptionsR.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tpgFind;
        private System.Windows.Forms.TabPage tpgReplace;
        internal System.Windows.Forms.TabControl tabAll;
        private System.Windows.Forms.Label lblFindF;
        private System.Windows.Forms.Label lblSearchTypeF;
        private System.Windows.Forms.RadioButton rdoRegexF;
        private System.Windows.Forms.RadioButton rdoStandardF;
        private System.Windows.Forms.GroupBox grpOptionsF;
        private System.Windows.Forms.Panel pnlRegexpOptionsF;
        private System.Windows.Forms.Panel pnlStandardOptionsF;
        private System.Windows.Forms.CheckBox chkWordStartF;
        private System.Windows.Forms.CheckBox chkWholeWordF;
        private System.Windows.Forms.CheckBox chkMatchCaseF;
        private System.Windows.Forms.CheckBox chkEcmaScriptF;
        private System.Windows.Forms.CheckBox chkCultureInvariantF;
        private System.Windows.Forms.CheckBox chkCompiledF;
        private System.Windows.Forms.CheckBox chkRightToLeftF;
        private System.Windows.Forms.CheckBox chkMultilineF;
        private System.Windows.Forms.CheckBox chkIgnorePatternWhitespaceF;
        private System.Windows.Forms.CheckBox chkIgnoreCaseF;
        private System.Windows.Forms.CheckBox chkExplicitCaptureF;
        private System.Windows.Forms.CheckBox chkSinglelineF;
        private System.Windows.Forms.Button btnFindPrevious;
        private System.Windows.Forms.Button btnFindNext;
        private System.Windows.Forms.CheckBox chkWrapF;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnFindAll;
        private System.Windows.Forms.CheckBox chkHighlightMatches;
        private System.Windows.Forms.CheckBox chkMarkLine;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ComboBox cboReplace;
        private System.Windows.Forms.Label lblReplace;
        private System.Windows.Forms.CheckBox chkWrapR;
        private System.Windows.Forms.Button btnReplacePrevious;
        private System.Windows.Forms.Button btnReplaceNext;
        private System.Windows.Forms.GroupBox grdOptionsR;
        private System.Windows.Forms.Panel pnlRegexpOptionsR;
        private System.Windows.Forms.CheckBox chkSinglelineR;
        private System.Windows.Forms.CheckBox chkRightToLeftR;
        private System.Windows.Forms.CheckBox chkMultilineR;
        private System.Windows.Forms.CheckBox chkIgnorePatternWhitespaceR;
        private System.Windows.Forms.CheckBox chkIgnoreCaseR;
        private System.Windows.Forms.CheckBox chkExplicitCaptureR;
        private System.Windows.Forms.CheckBox chkEcmaScriptR;
        private System.Windows.Forms.CheckBox chkCultureInvariantR;
        private System.Windows.Forms.CheckBox chkCompiledR;
        private System.Windows.Forms.Panel pnlStandardOptionsR;
        private System.Windows.Forms.CheckBox chkWordStartR;
        private System.Windows.Forms.CheckBox chkWholeWordR;
        private System.Windows.Forms.CheckBox chkMatchCaseR;
        private System.Windows.Forms.Label lblSearchTypeR;
        private System.Windows.Forms.RadioButton rdoRegexR;
        private System.Windows.Forms.RadioButton rdoStandardR;
        private System.Windows.Forms.Label lblFindR;
        private System.Windows.Forms.Button btnReplaceAll;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        public System.Windows.Forms.GroupBox grpFindAll;
        internal System.Windows.Forms.CheckBox chkSearchSelectionF;
        internal System.Windows.Forms.CheckBox chkSearchSelectionR;
        internal System.Windows.Forms.ComboBox cboFindF;
        internal System.Windows.Forms.ComboBox cboFindR;
    }
}