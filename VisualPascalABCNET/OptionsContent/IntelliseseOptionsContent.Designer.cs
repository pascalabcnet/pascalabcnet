namespace VisualPascalABC.OptionsContent
{
    partial class IntelliseseOptionsContent
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
            this.cbCodeCompletionKeyPressed = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.nuNamespaceVisibleRange = new System.Windows.Forms.NumericUpDown();
            this.cbAllowCodeCompletion = new System.Windows.Forms.CheckBox();
            this.cbCodeCompletionParams = new System.Windows.Forms.CheckBox();
            this.cbCodeCompletionDot = new System.Windows.Forms.CheckBox();
            this.cbCodeCompletionHint = new System.Windows.Forms.CheckBox();
            this.cbIntellisencePanel = new System.Windows.Forms.CheckBox();
            this.cbUseSemanticIntellisense = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nuNamespaceVisibleRange)).BeginInit();
            this.SuspendLayout();
            // 
            // cbCodeCompletionKeyPressed
            // 
            this.cbCodeCompletionKeyPressed.AutoSize = true;
            this.cbCodeCompletionKeyPressed.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbCodeCompletionKeyPressed.Checked = true;
            this.cbCodeCompletionKeyPressed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCodeCompletionKeyPressed.Location = new System.Drawing.Point(17, 89);
            this.cbCodeCompletionKeyPressed.Name = "cbCodeCompletionKeyPressed";
            this.cbCodeCompletionKeyPressed.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbCodeCompletionKeyPressed.Size = new System.Drawing.Size(210, 17);
            this.cbCodeCompletionKeyPressed.TabIndex = 38;
            this.cbCodeCompletionKeyPressed.Text = "CODE_COMPLETION_KEYPRESSED";
            this.cbCodeCompletionKeyPressed.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(68, 138);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(163, 13);
            this.label5.TabIndex = 37;
            this.label5.Text = "NAMESPACE_VISIBLE_RANGE";
            // 
            // nuNamespaceVisibleRange
            // 
            this.nuNamespaceVisibleRange.Location = new System.Drawing.Point(17, 134);
            this.nuNamespaceVisibleRange.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.nuNamespaceVisibleRange.Name = "nuNamespaceVisibleRange";
            this.nuNamespaceVisibleRange.Size = new System.Drawing.Size(48, 20);
            this.nuNamespaceVisibleRange.TabIndex = 36;
            this.nuNamespaceVisibleRange.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // cbAllowCodeCompletion
            // 
            this.cbAllowCodeCompletion.AutoSize = true;
            this.cbAllowCodeCompletion.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbAllowCodeCompletion.Checked = true;
            this.cbAllowCodeCompletion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAllowCodeCompletion.Location = new System.Drawing.Point(5, 5);
            this.cbAllowCodeCompletion.Name = "cbAllowCodeCompletion";
            this.cbAllowCodeCompletion.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbAllowCodeCompletion.Size = new System.Drawing.Size(176, 17);
            this.cbAllowCodeCompletion.TabIndex = 35;
            this.cbAllowCodeCompletion.Text = "ALLOW_CODE_COMPLETION";
            this.cbAllowCodeCompletion.UseVisualStyleBackColor = true;
            this.cbAllowCodeCompletion.CheckedChanged += new System.EventHandler(this.cbAllowCodeCompletion_CheckedChanged);
            // 
            // cbCodeCompletionParams
            // 
            this.cbCodeCompletionParams.AutoSize = true;
            this.cbCodeCompletionParams.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbCodeCompletionParams.Checked = true;
            this.cbCodeCompletionParams.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCodeCompletionParams.Location = new System.Drawing.Point(17, 68);
            this.cbCodeCompletionParams.Name = "cbCodeCompletionParams";
            this.cbCodeCompletionParams.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbCodeCompletionParams.Size = new System.Drawing.Size(183, 17);
            this.cbCodeCompletionParams.TabIndex = 34;
            this.cbCodeCompletionParams.Text = "CODE_COMPLETION_PARAMS";
            this.cbCodeCompletionParams.UseVisualStyleBackColor = true;
            // 
            // cbCodeCompletionDot
            // 
            this.cbCodeCompletionDot.AutoSize = true;
            this.cbCodeCompletionDot.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbCodeCompletionDot.Checked = true;
            this.cbCodeCompletionDot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCodeCompletionDot.Location = new System.Drawing.Point(17, 47);
            this.cbCodeCompletionDot.Name = "cbCodeCompletionDot";
            this.cbCodeCompletionDot.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbCodeCompletionDot.Size = new System.Drawing.Size(161, 17);
            this.cbCodeCompletionDot.TabIndex = 33;
            this.cbCodeCompletionDot.Text = "CODE_COMPLETION_DOT";
            this.cbCodeCompletionDot.UseVisualStyleBackColor = true;
            // 
            // cbCodeCompletionHint
            // 
            this.cbCodeCompletionHint.AutoSize = true;
            this.cbCodeCompletionHint.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbCodeCompletionHint.Checked = true;
            this.cbCodeCompletionHint.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCodeCompletionHint.Location = new System.Drawing.Point(17, 26);
            this.cbCodeCompletionHint.Name = "cbCodeCompletionHint";
            this.cbCodeCompletionHint.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbCodeCompletionHint.Size = new System.Drawing.Size(164, 17);
            this.cbCodeCompletionHint.TabIndex = 32;
            this.cbCodeCompletionHint.Text = "CODE_COMPLETION_HINT";
            this.cbCodeCompletionHint.UseVisualStyleBackColor = true;
            // 
            // cbIntellisencePanel
            // 
            this.cbIntellisencePanel.AutoSize = true;
            this.cbIntellisencePanel.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbIntellisencePanel.Checked = true;
            this.cbIntellisencePanel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIntellisencePanel.Location = new System.Drawing.Point(17, 110);
            this.cbIntellisencePanel.Name = "cbIntellisencePanel";
            this.cbIntellisencePanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbIntellisencePanel.Size = new System.Drawing.Size(183, 17);
            this.cbIntellisencePanel.TabIndex = 39;
            this.cbIntellisencePanel.Text = "SHOW_INTELLISENCE_PANEL";
            this.cbIntellisencePanel.UseVisualStyleBackColor = true;
            // 
            // cbUseSemanticIntellisense
            // 
            this.cbUseSemanticIntellisense.AutoSize = true;
            this.cbUseSemanticIntellisense.Location = new System.Drawing.Point(17, 161);
            this.cbUseSemanticIntellisense.Name = "cbUseSemanticIntellisense";
            this.cbUseSemanticIntellisense.Size = new System.Drawing.Size(191, 17);
            this.cbUseSemanticIntellisense.TabIndex = 40;
            this.cbUseSemanticIntellisense.Text = "USE_SEMANTIC_INTELLISENSE";
            this.cbUseSemanticIntellisense.UseVisualStyleBackColor = true;
            this.cbUseSemanticIntellisense.CheckedChanged += new System.EventHandler(this.cbUseSemanticIntellisense_CheckedChanged);
            // 
            // IntelliseseOptionsContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbUseSemanticIntellisense);
            this.Controls.Add(this.cbIntellisencePanel);
            this.Controls.Add(this.cbCodeCompletionKeyPressed);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nuNamespaceVisibleRange);
            this.Controls.Add(this.cbAllowCodeCompletion);
            this.Controls.Add(this.cbCodeCompletionParams);
            this.Controls.Add(this.cbCodeCompletionDot);
            this.Controls.Add(this.cbCodeCompletionHint);
            this.Name = "IntelliseseOptionsContent";
            this.Size = new System.Drawing.Size(526, 211);
            ((System.ComponentModel.ISupportInitialize)(this.nuNamespaceVisibleRange)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbCodeCompletionKeyPressed;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nuNamespaceVisibleRange;
        private System.Windows.Forms.CheckBox cbAllowCodeCompletion;
        private System.Windows.Forms.CheckBox cbCodeCompletionParams;
        private System.Windows.Forms.CheckBox cbCodeCompletionDot;
        private System.Windows.Forms.CheckBox cbCodeCompletionHint;
        private System.Windows.Forms.CheckBox cbIntellisencePanel;
        private System.Windows.Forms.CheckBox cbUseSemanticIntellisense;
    }
}
