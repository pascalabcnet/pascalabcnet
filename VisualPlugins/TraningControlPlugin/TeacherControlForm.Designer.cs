namespace VisualPascalABCPlugins
{
    partial class TeacherControlForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TeacherControlForm));
            this.PluginImage = new System.Windows.Forms.PictureBox();
            this.labelLogin = new System.Windows.Forms.Label();
            this.LoginTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.checkBoxMemory = new System.Windows.Forms.CheckBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelMessage = new System.Windows.Forms.Label();
            this.panelLoginPassword = new System.Windows.Forms.Panel();
            this.panelAfterLogin = new System.Windows.Forms.Panel();
            this.labelEntered = new System.Windows.Forms.Label();
            this.LoginTextBox2 = new System.Windows.Forms.TextBox();
            this.labelAdditional = new System.Windows.Forms.Label();
            this.textBoxAdditional = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.PluginImage)).BeginInit();
            this.panelLoginPassword.SuspendLayout();
            this.panelAfterLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // PluginImage
            // 
            this.PluginImage.BackColor = System.Drawing.Color.Magenta;
            this.PluginImage.Image = ((System.Drawing.Image)(resources.GetObject("PluginImage.Image")));
            this.PluginImage.Location = new System.Drawing.Point(557, 306);
            this.PluginImage.Margin = new System.Windows.Forms.Padding(12);
            this.PluginImage.Name = "PluginImage";
            this.PluginImage.Size = new System.Drawing.Size(50, 50);
            this.PluginImage.TabIndex = 9;
            this.PluginImage.TabStop = false;
            this.PluginImage.Visible = false;
            // 
            // labelLogin
            // 
            this.labelLogin.AutoSize = true;
            this.labelLogin.Location = new System.Drawing.Point(39, 31);
            this.labelLogin.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelLogin.Name = "labelLogin";
            this.labelLogin.Size = new System.Drawing.Size(329, 25);
            this.labelLogin.TabIndex = 10;
            this.labelLogin.Text = "Логин (Фамилия Имя Отчество)";
            // 
            // LoginTextBox
            // 
            this.LoginTextBox.Location = new System.Drawing.Point(45, 61);
            this.LoginTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.LoginTextBox.Name = "LoginTextBox";
            this.LoginTextBox.Size = new System.Drawing.Size(538, 31);
            this.LoginTextBox.TabIndex = 11;
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(44, 141);
            this.PasswordTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(538, 31);
            this.PasswordTextBox.TabIndex = 13;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(38, 110);
            this.labelPassword.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(86, 25);
            this.labelPassword.TabIndex = 12;
            this.labelPassword.Text = "Пароль";
            // 
            // checkBoxMemory
            // 
            this.checkBoxMemory.AutoSize = true;
            this.checkBoxMemory.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxMemory.Location = new System.Drawing.Point(378, 202);
            this.checkBoxMemory.Margin = new System.Windows.Forms.Padding(6);
            this.checkBoxMemory.Name = "checkBoxMemory";
            this.checkBoxMemory.Size = new System.Drawing.Size(205, 29);
            this.checkBoxMemory.TabIndex = 15;
            this.checkBoxMemory.Text = "Запомнить вход";
            this.checkBoxMemory.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(94, 304);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(179, 50);
            this.buttonOK.TabIndex = 16;
            this.buttonOK.Text = "Вход";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(346, 304);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(179, 50);
            this.buttonCancel.TabIndex = 17;
            this.buttonCancel.Text = "Закрыть";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.ForeColor = System.Drawing.Color.OrangeRed;
            this.labelMessage.Location = new System.Drawing.Point(40, 242);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(0, 25);
            this.labelMessage.TabIndex = 18;
            // 
            // panelLoginPassword
            // 
            this.panelLoginPassword.Controls.Add(this.labelLogin);
            this.panelLoginPassword.Controls.Add(this.labelMessage);
            this.panelLoginPassword.Controls.Add(this.LoginTextBox);
            this.panelLoginPassword.Controls.Add(this.labelPassword);
            this.panelLoginPassword.Controls.Add(this.checkBoxMemory);
            this.panelLoginPassword.Controls.Add(this.PasswordTextBox);
            this.panelLoginPassword.Location = new System.Drawing.Point(0, 0);
            this.panelLoginPassword.Name = "panelLoginPassword";
            this.panelLoginPassword.Size = new System.Drawing.Size(629, 291);
            this.panelLoginPassword.TabIndex = 19;
            // 
            // panelAfterLogin
            // 
            this.panelAfterLogin.Controls.Add(this.labelEntered);
            this.panelAfterLogin.Controls.Add(this.LoginTextBox2);
            this.panelAfterLogin.Controls.Add(this.labelAdditional);
            this.panelAfterLogin.Controls.Add(this.textBoxAdditional);
            this.panelAfterLogin.Location = new System.Drawing.Point(0, 0);
            this.panelAfterLogin.Name = "panelAfterLogin";
            this.panelAfterLogin.Size = new System.Drawing.Size(629, 291);
            this.panelAfterLogin.TabIndex = 20;
            // 
            // labelEntered
            // 
            this.labelEntered.AutoSize = true;
            this.labelEntered.Location = new System.Drawing.Point(39, 31);
            this.labelEntered.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelEntered.Name = "labelEntered";
            this.labelEntered.Size = new System.Drawing.Size(171, 25);
            this.labelEntered.TabIndex = 10;
            this.labelEntered.Text = "Вход выполнен:";
            // 
            // LoginTextBox2
            // 
            this.LoginTextBox2.Enabled = false;
            this.LoginTextBox2.Location = new System.Drawing.Point(45, 61);
            this.LoginTextBox2.Margin = new System.Windows.Forms.Padding(6);
            this.LoginTextBox2.Name = "LoginTextBox2";
            this.LoginTextBox2.Size = new System.Drawing.Size(538, 31);
            this.LoginTextBox2.TabIndex = 11;
            // 
            // labelAdditional
            // 
            this.labelAdditional.AutoSize = true;
            this.labelAdditional.Location = new System.Drawing.Point(38, 110);
            this.labelAdditional.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelAdditional.Name = "labelAdditional";
            this.labelAdditional.Size = new System.Drawing.Size(319, 25);
            this.labelAdditional.TabIndex = 12;
            this.labelAdditional.Text = "Дополнительная информация:";
            // 
            // textBoxAdditional
            // 
            this.textBoxAdditional.Enabled = false;
            this.textBoxAdditional.Location = new System.Drawing.Point(44, 141);
            this.textBoxAdditional.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxAdditional.Multiline = true;
            this.textBoxAdditional.Name = "textBoxAdditional";
            this.textBoxAdditional.Size = new System.Drawing.Size(538, 132);
            this.textBoxAdditional.TabIndex = 13;
            // 
            // TeacherControlForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(628, 393);
            this.Controls.Add(this.panelLoginPassword);
            this.Controls.Add(this.panelAfterLogin);
            this.Controls.Add(this.PluginImage);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(12);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TeacherControlForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FORMNAME";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.PluginImage)).EndInit();
            this.panelLoginPassword.ResumeLayout(false);
            this.panelLoginPassword.PerformLayout();
            this.panelAfterLogin.ResumeLayout(false);
            this.panelAfterLogin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox PluginImage;
        private System.Windows.Forms.Label labelLogin;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.CheckBox checkBoxMemory;
        public System.Windows.Forms.TextBox LoginTextBox;
        public System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Panel panelLoginPassword;
        private System.Windows.Forms.Panel panelAfterLogin;
        private System.Windows.Forms.Label labelEntered;
        public System.Windows.Forms.TextBox LoginTextBox2;
        private System.Windows.Forms.Label labelAdditional;
        public System.Windows.Forms.TextBox textBoxAdditional;
    }
}