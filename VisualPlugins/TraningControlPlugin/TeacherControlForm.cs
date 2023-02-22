using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PascalABCCompiler.Errors;
using System.IO;

namespace VisualPascalABCPlugins
{
    public partial class TeacherControlForm : Form
    {
        private const string MessageEmptyLogin = "������ �����";
        private const string MessageEmptyPassword = "������ ������";
        private const string BadLoginOrPassword = "�������� ����� ��� ������";
        private const string NoConnection = "�� ������� ����������� � ��������. ��������� ������� �����";
        private const string LoginText = "����� (������� ��� ��������)";

        public IVisualEnvironmentCompiler VisualEnvironmentCompiler;

        public TeacherControlForm()
        {
            InitializeComponent();
            PascalABCCompiler.StringResources.SetTextForAllObjects(this, VisualPascalABCPlugin_TeacherControlPlugin.StringsPrefix);
        }

        public void SetRegistered()
        {
            LoginTextBox.Enabled = false;
            LoginTextBox2.Text = LoginTextBox.Text;

            buttonOK.Text = "�����";
            PasswordTextBox.Text = "";

            panelLoginPassword.Visible = false;

            //this.Text = LabelEntered;
        }
        public void SetUnRegistered()
        {
            buttonOK.Text = "����";
            LoginTextBox.Enabled = true;
            LoginTextBox2.Text = LoginTextBox.Text;

            panelLoginPassword.Visible = true;

            LoginTextBox.Text = "";
            PasswordTextBox.Text = "";
            //this.Text = LabelNotEntered;
        }

        public string Login { get; private set; }
        public string Password { get; private set; }
        public bool Registered { get; private set; }

        public bool CheckConnection()
        {
            return true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!Registered)
            {
                if (LoginTextBox.Text.Trim() == "")
                {
                    labelMessage.Text = MessageEmptyLogin;
                    return;
                }
                if (PasswordTextBox.Text.Trim() == "")
                {
                    labelMessage.Text = MessageEmptyPassword;
                    return;
                }
                if (CheckConnection())
                {
                    labelMessage.Text = "";
                    Login = LoginTextBox.Text;
                    Password = PasswordTextBox.Text;
                    Registered = true;
                    SetRegistered();
                }
            }
            else
            {
                Login = null;
                Password = null;
                Registered = false;
                SetUnRegistered();
            }
        }
    }
}
