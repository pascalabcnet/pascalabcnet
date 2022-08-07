namespace DBAccessPluginNamespace
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.groupNamesBox = new System.Windows.Forms.ComboBox();
            this.usersNamesBox = new System.Windows.Forms.ComboBox();
            this.labelGroup = new System.Windows.Forms.Label();
            this.labelUser = new System.Windows.Forms.Label();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.enterButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.userImage = new System.Windows.Forms.PictureBox();
            this.groupImage = new System.Windows.Forms.PictureBox();
            this.PluginImage = new System.Windows.Forms.PictureBox();
            this.PluginImageAuthorized = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.userImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PluginImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PluginImageAuthorized)).BeginInit();
            this.SuspendLayout();
            // 
            // groupNamesBox
            // 
            this.groupNamesBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.groupNamesBox.FormattingEnabled = true;
            this.groupNamesBox.Location = new System.Drawing.Point(114, 12);
            this.groupNamesBox.Name = "groupNamesBox";
            this.groupNamesBox.Size = new System.Drawing.Size(258, 21);
            this.groupNamesBox.TabIndex = 1;
            this.groupNamesBox.SelectedIndexChanged += new System.EventHandler(this.GroupNamesBox_SelectedIndexChanged);
            // 
            // usersNamesBox
            // 
            this.usersNamesBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.usersNamesBox.FormattingEnabled = true;
            this.usersNamesBox.Location = new System.Drawing.Point(114, 50);
            this.usersNamesBox.Name = "usersNamesBox";
            this.usersNamesBox.Size = new System.Drawing.Size(258, 21);
            this.usersNamesBox.TabIndex = 2;
            // 
            // labelGroup
            // 
            this.labelGroup.AutoSize = true;
            this.labelGroup.Location = new System.Drawing.Point(63, 15);
            this.labelGroup.Name = "labelGroup";
            this.labelGroup.Size = new System.Drawing.Size(45, 13);
            this.labelGroup.TabIndex = 3;
            this.labelGroup.Text = "Группа:";
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(25, 53);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(83, 13);
            this.labelUser.TabIndex = 4;
            this.labelUser.Text = "Пользователь:";
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(114, 92);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(258, 20);
            this.passwordBox.TabIndex = 5;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(60, 95);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(48, 13);
            this.labelPassword.TabIndex = 6;
            this.labelPassword.Text = "Пароль:";
            // 
            // enterButton
            // 
            this.enterButton.Location = new System.Drawing.Point(87, 131);
            this.enterButton.Name = "enterButton";
            this.enterButton.Size = new System.Drawing.Size(118, 34);
            this.enterButton.TabIndex = 8;
            this.enterButton.Text = "Вход";
            this.enterButton.UseVisualStyleBackColor = true;
            this.enterButton.Click += new System.EventHandler(this.enterButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(225, 131);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(118, 34);
            this.closeButton.TabIndex = 7;
            this.closeButton.Text = "Закрыть";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // userImage
            // 
            this.userImage.InitialImage = null;
            this.userImage.Location = new System.Drawing.Point(378, 45);
            this.userImage.Name = "userImage";
            this.userImage.Size = new System.Drawing.Size(32, 32);
            this.userImage.TabIndex = 10;
            this.userImage.TabStop = false;
            // 
            // groupImage
            // 
            this.groupImage.InitialImage = null;
            this.groupImage.Location = new System.Drawing.Point(378, 7);
            this.groupImage.Name = "groupImage";
            this.groupImage.Size = new System.Drawing.Size(32, 32);
            this.groupImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.groupImage.TabIndex = 9;
            this.groupImage.TabStop = false;
            // 
            // PluginImage
            // 
            this.PluginImage.BackColor = System.Drawing.Color.Magenta;
            this.PluginImage.Image = ((System.Drawing.Image)(resources.GetObject("PluginImage.Image")));
            this.PluginImage.Location = new System.Drawing.Point(379, 95);
            this.PluginImage.Margin = new System.Windows.Forms.Padding(6);
            this.PluginImage.Name = "PluginImage";
            this.PluginImage.Size = new System.Drawing.Size(25, 26);
            this.PluginImage.TabIndex = 11;
            this.PluginImage.TabStop = false;
            this.PluginImage.Visible = false;
            // 
            // PluginImageAuthorized
            // 
            this.PluginImageAuthorized.BackColor = System.Drawing.Color.Magenta;
            this.PluginImageAuthorized.Image = ((System.Drawing.Image)(resources.GetObject("PluginImageAuthorized.Image")));
            this.PluginImageAuthorized.InitialImage = null;
            this.PluginImageAuthorized.Location = new System.Drawing.Point(379, 131);
            this.PluginImageAuthorized.Name = "PluginImageAuthorized";
            this.PluginImageAuthorized.Size = new System.Drawing.Size(25, 26);
            this.PluginImageAuthorized.TabIndex = 12;
            this.PluginImageAuthorized.TabStop = false;
            this.PluginImageAuthorized.Visible = false;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(422, 175);
            this.Controls.Add(this.PluginImageAuthorized);
            this.Controls.Add(this.PluginImage);
            this.Controls.Add(this.userImage);
            this.Controls.Add(this.groupImage);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.enterButton);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.passwordBox);
            this.Controls.Add(this.labelUser);
            this.Controls.Add(this.labelGroup);
            this.Controls.Add(this.usersNamesBox);
            this.Controls.Add(this.groupNamesBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Авторизация";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.userImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PluginImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PluginImageAuthorized)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox groupNamesBox;
        private System.Windows.Forms.ComboBox usersNamesBox;
        private System.Windows.Forms.Label labelGroup;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Button enterButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.PictureBox groupImage;
        private System.Windows.Forms.PictureBox userImage;
        public System.Windows.Forms.PictureBox PluginImage;
        public System.Windows.Forms.PictureBox PluginImageAuthorized;
    }
}