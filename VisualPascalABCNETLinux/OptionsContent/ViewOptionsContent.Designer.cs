namespace VisualPascalABC.OptionsContent
{
    partial class ViewOptionsContent
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
            this.cbSaveFilesIfComilationOk = new System.Windows.Forms.CheckBox();
            this.cbPauseInRunModeIfConsole = new System.Windows.Forms.CheckBox();
            this.cbShowDebugPlayPauseButtons = new System.Windows.Forms.CheckBox();
            this.cbErrorsStrategy = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.languageSelect = new System.Windows.Forms.ComboBox();
            this.cbAutoInsertCodeIsEnabledOnStartup = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbSaveFilesIfComilationOk
            // 
            this.cbSaveFilesIfComilationOk.AutoSize = true;
            this.cbSaveFilesIfComilationOk.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbSaveFilesIfComilationOk.Checked = true;
            this.cbSaveFilesIfComilationOk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSaveFilesIfComilationOk.Location = new System.Drawing.Point(7, 128);
            this.cbSaveFilesIfComilationOk.Name = "cbSaveFilesIfComilationOk";
            this.cbSaveFilesIfComilationOk.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbSaveFilesIfComilationOk.Size = new System.Drawing.Size(204, 17);
            this.cbSaveFilesIfComilationOk.TabIndex = 25;
            this.cbSaveFilesIfComilationOk.Text = "SAVE_FILES_IF_COMPILATOIN_OK";
            this.cbSaveFilesIfComilationOk.UseVisualStyleBackColor = true;
            // 
            // cbPauseInRunModeIfConsole
            // 
            this.cbPauseInRunModeIfConsole.AutoSize = true;
            this.cbPauseInRunModeIfConsole.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbPauseInRunModeIfConsole.Checked = true;
            this.cbPauseInRunModeIfConsole.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPauseInRunModeIfConsole.Location = new System.Drawing.Point(7, 105);
            this.cbPauseInRunModeIfConsole.Name = "cbPauseInRunModeIfConsole";
            this.cbPauseInRunModeIfConsole.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbPauseInRunModeIfConsole.Size = new System.Drawing.Size(220, 17);
            this.cbPauseInRunModeIfConsole.TabIndex = 24;
            this.cbPauseInRunModeIfConsole.Text = "PASUSE_IN_RUNMODE_IF_CONSOLE";
            this.cbPauseInRunModeIfConsole.UseVisualStyleBackColor = true;
            // 
            // cbShowDebugPlayPauseButtons
            // 
            this.cbShowDebugPlayPauseButtons.AutoSize = true;
            this.cbShowDebugPlayPauseButtons.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbShowDebugPlayPauseButtons.Checked = true;
            this.cbShowDebugPlayPauseButtons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowDebugPlayPauseButtons.Location = new System.Drawing.Point(7, 174);
            this.cbShowDebugPlayPauseButtons.Name = "cbShowDebugPlayPauseButtons";
            this.cbShowDebugPlayPauseButtons.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbShowDebugPlayPauseButtons.Size = new System.Drawing.Size(237, 17);
            this.cbShowDebugPlayPauseButtons.TabIndex = 23;
            this.cbShowDebugPlayPauseButtons.Text = "SHOW_DEBUG_PLAY_PAUSE_BUTTONS";
            this.cbShowDebugPlayPauseButtons.UseVisualStyleBackColor = true;
            this.cbShowDebugPlayPauseButtons.Visible = false;
            this.cbShowDebugPlayPauseButtons.CheckedChanged += new System.EventHandler(this.cbShowDebugPlayPauseButtons_CheckedChanged);
            // 
            // cbErrorsStrategy
            // 
            this.cbErrorsStrategy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbErrorsStrategy.FormattingEnabled = true;
            this.cbErrorsStrategy.Location = new System.Drawing.Point(7, 66);
            this.cbErrorsStrategy.Name = "cbErrorsStrategy";
            this.cbErrorsStrategy.Size = new System.Drawing.Size(284, 21);
            this.cbErrorsStrategy.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "ERRORS_STRATEGY";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "LANGUAGE";
            // 
            // languageSelect
            // 
            this.languageSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.languageSelect.FormattingEnabled = true;
            this.languageSelect.Location = new System.Drawing.Point(7, 21);
            this.languageSelect.Name = "languageSelect";
            this.languageSelect.Size = new System.Drawing.Size(195, 21);
            this.languageSelect.TabIndex = 18;
            this.languageSelect.SelectedIndexChanged += new System.EventHandler(this.languageSelect_SelectedIndexChanged);
            // 
            // cbAutoInsertCodeIsEnabledOnStartup
            // 
            this.cbAutoInsertCodeIsEnabledOnStartup.AutoSize = true;
            this.cbAutoInsertCodeIsEnabledOnStartup.Checked = true;
            this.cbAutoInsertCodeIsEnabledOnStartup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoInsertCodeIsEnabledOnStartup.Location = new System.Drawing.Point(7, 151);
            this.cbAutoInsertCodeIsEnabledOnStartup.Name = "cbAutoInsertCodeIsEnabledOnStartup";
            this.cbAutoInsertCodeIsEnabledOnStartup.Size = new System.Drawing.Size(353, 17);
            this.cbAutoInsertCodeIsEnabledOnStartup.TabIndex = 26;
            this.cbAutoInsertCodeIsEnabledOnStartup.Text = "AUTO_INSERT_CODE_IS_ENABLED_ON_STARTUP";
            this.cbAutoInsertCodeIsEnabledOnStartup.UseVisualStyleBackColor = true;
            // 
            // ViewOptionsContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbAutoInsertCodeIsEnabledOnStartup);
            this.Controls.Add(this.cbSaveFilesIfComilationOk);
            this.Controls.Add(this.cbPauseInRunModeIfConsole);
            this.Controls.Add(this.cbShowDebugPlayPauseButtons);
            this.Controls.Add(this.cbErrorsStrategy);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.languageSelect);
            this.Name = "ViewOptionsContent";
            this.Size = new System.Drawing.Size(495, 246);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbSaveFilesIfComilationOk;
        private System.Windows.Forms.CheckBox cbPauseInRunModeIfConsole;
        private System.Windows.Forms.CheckBox cbShowDebugPlayPauseButtons;
        private System.Windows.Forms.ComboBox cbErrorsStrategy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox languageSelect;
        private System.Windows.Forms.CheckBox cbAutoInsertCodeIsEnabledOnStartup;
    }
}
