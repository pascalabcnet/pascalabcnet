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
            ((System.ComponentModel.ISupportInitialize)(this.nuNamespaceVisibleRange)).BeginInit();
            this.SuspendLayout();
            // 
            // cbCodeCompletionKeyPressed
            // 
            this.cbCodeCompletionKeyPressed.AutoSize = true;
            this.cbCodeCompletionKeyPressed.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbCodeCompletionKeyPressed.Checked = true;
            this.cbCodeCompletionKeyPressed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCodeCompletionKeyPressed.Location = new System.Drawing.Point(23, 110);
            this.cbCodeCompletionKeyPressed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbCodeCompletionKeyPressed.Name = "cbCodeCompletionKeyPressed";
            this.cbCodeCompletionKeyPressed.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbCodeCompletionKeyPressed.Size = new System.Drawing.Size(264, 21);
            this.cbCodeCompletionKeyPressed.TabIndex = 38;
            this.cbCodeCompletionKeyPressed.Text = "CODE_COMPLETION_KEYPRESSED";
            this.cbCodeCompletionKeyPressed.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(91, 170);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(207, 17);
            this.label5.TabIndex = 37;
            this.label5.Text = "NAMESPACE_VISIBLE_RANGE";
            // 
            // nuNamespaceVisibleRange
            // 
            this.nuNamespaceVisibleRange.Location = new System.Drawing.Point(23, 165);
            this.nuNamespaceVisibleRange.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nuNamespaceVisibleRange.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.nuNamespaceVisibleRange.Name = "nuNamespaceVisibleRange";
            this.nuNamespaceVisibleRange.Size = new System.Drawing.Size(64, 22);
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
            this.cbAllowCodeCompletion.Location = new System.Drawing.Point(7, 6);
            this.cbAllowCodeCompletion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbAllowCodeCompletion.Name = "cbAllowCodeCompletion";
            this.cbAllowCodeCompletion.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbAllowCodeCompletion.Size = new System.Drawing.Size(221, 21);
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
            this.cbCodeCompletionParams.Location = new System.Drawing.Point(23, 84);
            this.cbCodeCompletionParams.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbCodeCompletionParams.Name = "cbCodeCompletionParams";
            this.cbCodeCompletionParams.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbCodeCompletionParams.Size = new System.Drawing.Size(229, 21);
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
            this.cbCodeCompletionDot.Location = new System.Drawing.Point(23, 58);
            this.cbCodeCompletionDot.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbCodeCompletionDot.Name = "cbCodeCompletionDot";
            this.cbCodeCompletionDot.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbCodeCompletionDot.Size = new System.Drawing.Size(202, 21);
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
            this.cbCodeCompletionHint.Location = new System.Drawing.Point(23, 32);
            this.cbCodeCompletionHint.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbCodeCompletionHint.Name = "cbCodeCompletionHint";
            this.cbCodeCompletionHint.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbCodeCompletionHint.Size = new System.Drawing.Size(204, 21);
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
            this.cbIntellisencePanel.Location = new System.Drawing.Point(23, 136);
            this.cbIntellisencePanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbIntellisencePanel.Name = "cbIntellisencePanel";
            this.cbIntellisencePanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbIntellisencePanel.Size = new System.Drawing.Size(227, 21);
            this.cbIntellisencePanel.TabIndex = 39;
            this.cbIntellisencePanel.Text = "SHOW_INTELLISENCE_PANEL";
            this.cbIntellisencePanel.UseVisualStyleBackColor = true;
            // 
            // IntelliseseOptionsContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbIntellisencePanel);
            this.Controls.Add(this.cbCodeCompletionKeyPressed);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nuNamespaceVisibleRange);
            this.Controls.Add(this.cbAllowCodeCompletion);
            this.Controls.Add(this.cbCodeCompletionParams);
            this.Controls.Add(this.cbCodeCompletionDot);
            this.Controls.Add(this.cbCodeCompletionHint);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "IntelliseseOptionsContent";
            this.Size = new System.Drawing.Size(701, 260);
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
    }
}
