using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualPascalABCPlugins;

namespace DBAccessPluginNamespace
{
    public partial class LoginForm : Form
    {
        public SiteAccessProvider SiteProvider = null;
        public VisualPascalABCPlugin_TeacherControlPlugin Plugin = null;

        public LoginForm(VisualPascalABCPlugin_TeacherControlPlugin Plugin)
        {
            this.Plugin = Plugin;
            InitializeComponent();
        }

        public bool Authorized = false;
        private async void enterButton_Click(object sender, EventArgs e)
        {
            if (SiteProvider == null)
                throw new Exception("Error in Login form! Site credentials cannot be empty!");

            if (!Authorized)
            {
                var answer = await SiteProvider.Login("", usersNamesBox.Text, passwordBox.Text);
                if(answer == "Success")
                {
                    //DialogResult = DialogResult.OK;
                    //Close();
                    Text = "Авторизация: вход выполнен";
                    passwordBox.Text = "";
                    enterButton.Text = "Выход";
                    passwordBox.Visible = false;
                    labelPassword.Visible = false;
                    groupNamesBox.Enabled = false;
                    usersNamesBox.Enabled = false;
                    Authorized = true;
                    Plugin.toolStripButton.ToolTipText = "Авторизация выполнена: " + SiteProvider.FullFIO;
                    Plugin.toolStripButton.Image = PluginImageAuthorized.Image;
                    Plugin.menuItem.Image = PluginImageAuthorized.Image;
                    this.Icon = VisualPascalABCPlugins.Properties.Resources.IconAuthorized;
                    closeButton.Focus();
                }
                else MessageBox.Show(answer,"Ошибка");
            }
            else
            {
                SiteProvider.Logout();
                Text = "Авторизация";
                passwordBox.Text = "";
                enterButton.Text = "Вход";
                groupNamesBox.SelectedIndex = -1;
                usersNamesBox.Items.Clear();
                usersNamesBox.SelectedIndex = -1;
                groupNamesBox.Enabled = true;
                usersNamesBox.Enabled = true;
                passwordBox.Visible = true;
                labelPassword.Visible = true;
                Authorized = false;
                Plugin.toolStripButton.ToolTipText = "Авторизация: вход не выполнен";
                Plugin.toolStripButton.Image = PluginImage.Image;
                Plugin.menuItem.Image = PluginImage.Image;
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
                this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            }

        }

        private async void LoginForm_Load(object sender, EventArgs e)
        {
            //  Получить список групп и заполнить выпадающий список значениями
            if (SiteProvider == null)
                throw new Exception("Error in Login form! Site credentials cannot be empty!");
            if (groupNamesBox.Items.Count > 0) // Группы уже загружены
                return;
            //groupImage.Image = Properties.Resources.LoadingImg;
            var groups = await SiteProvider.GetGroupsNames();
            groupNamesBox.Items.Clear();
            groupNamesBox.Items.AddRange(groups.Split(';').Where(Item => Item != "").ToArray());
            //groupImage.Image = null;
        }

        private async void GroupNamesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  Получить список участников группы
            if (groupNamesBox.Text == "") return;

            if (SiteProvider == null)
                throw new Exception("Error in Login form! Site credentials cannot be empty!");
            //userImage.Image = Properties.Resources.LoadingImg;
            var users = await SiteProvider.GetUsersNames(groupNamesBox.Text);
            usersNamesBox.Items.Clear();
            usersNamesBox.Items.AddRange(users.Split(';').Where(Item => Item != "").ToArray());
            //userImage.Image = null;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
            passwordBox.Text = "";
        }
    }
}
