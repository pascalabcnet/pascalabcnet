using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
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
        public bool Authorized { get; set; } = false;

        public LoginForm(VisualPascalABCPlugin_TeacherControlPlugin Plugin)
        {
            this.Plugin = Plugin;
            InitializeComponent();
        }

        public bool TryCreateAuthFile(string filename, string login, string pass)
        {
            var res = true;
            try
            {
                var encryptedstring = login + (char)10 + pass;
                var encr = TeacherPluginUtils.Encrypt(encryptedstring); // Здесь м.б. исключение
                using (var fs = new FileStream(filename, FileMode.Create)) // Здесь м.б. исключение
                {
                    fs.Write(encr, 0, encr.Length);
                }
            }
            catch (Exception e)
            {
                Plugin.AddMessage(e);
                res = false;
            }
            return res;
        }

        public bool IsRootDirectory(string path)
        {
            /*var isroot = false;
            try
            {
                var full = Path.GetFullPath(path);
                var root = Path.GetPathRoot(full);
                if (full == root)
                    isroot = true;
            }
            catch (Exception e)
            {
                Plugin.AddMessage(e);
            }
            return isroot;*/
            return Directory.GetParent(path) == null;
        }

        public bool CalcParentDirectory(string path, out string parent)
        {
            parent = "";
            if (IsRootDirectory(path))
                return false;
            var hasparent = false;

            var full = Path.GetFullPath(path);

            var di = Directory.GetParent(full);
            if (di != null)
                parent = di.FullName;

            hasparent = true;
            return hasparent;
        }

        public void WriteLoginPassToAuthDat(string login, string pass)
        {
            // SSM 04.08.22 Сохранить в auth.dat логин и пароль
            // Плохо вот что. Мы можем зайти из папки, в которой есть lightpt.dat, а потом перейти в папку, где его нет
            // Мы также можем скопировать lightpt.dat в корень

            // SSM 04.08.22 Поэтому надо проверить, что в текущей папке находится lightpt.dat и эта папка не корневая. В принципе, просто в корне этого диска можно
            // После этого перейти в корень диска и там создать этот файл

            // SSM 11.08.22 При ручной авторизации - сохранение
            // Мехмат: auth сохраняется в корень сетевого диска. Думаю, достаточно того же алгоритма что и дома!!!
            // Дома: auth сохраняется в папке на уровень выше текущей. Если это невозможно, то в текущей папке. 
            // Если это диск C, то тоже не сохранять и тогда сохранять в текущей

            // дома сохранять авторизацию на уровень выше текущего если это не корень. Если корень, то в текущий
            // на мехмате сохранять авторизацию в корень сетевого диска если он не защищен от записи
            // вопрос: если на мехмате текущий каталог - PABCWork.NET, то надо ли сохранять авторизацию?
            // это происходит тогда когда школьник смонтировал сетевой диск, но зашел в Паскаль по кнопке Пуск или с рабочего стола
            // можно попробовать поискать сетевой диск

            // Упрощаем. Дома сохраняем на уровень выше, а если нельзя или это корень, то в текущий
            // На мехмате если в текущем есть lightpt.dat - сохраняем на уровень выше, а если нельзя или это корень, то в текущий
            //            Если в текущем нет lightpt.dat - сохраняем в корень первого сетевого диска

            /*if (Plugin.IsMechmath)
            {
                // Найти корень сетевого диска
                // Если сетевой - текущий, то в него (это как правило)
                var workDir = Plugin.WorkingDirectory();
                var root = Path.GetPathRoot(workDir);
                System.IO.DriveInfo di = new System.IO.DriveInfo(root);
                bool b = false;
                if (di.DriveType == System.IO.DriveType.Network)
                {
                    var auth = Path.Combine(root, "auth.dat");
                    b = TryCreateAuthFile(auth, login, pass);
                }

                // Если нет, то попытаться найти первый сетевой, где в корне уже есть auth.dat
                if (!b) // Если не удалось записать
                {
                    string authName = "";
                    foreach (var drive in DriveInfo.GetDrives())
                    {
                        if (drive.DriveType != DriveType.Network)
                            continue;
                        // Проверять, что диск сетевой!!! Для несетевых - нет!
                        var auth = Path.Combine(drive.Name, "auth.dat");
                        if (File.Exists(auth))
                            authName = auth;
                    }
                    b = TryCreateAuthFile(authName, login, pass);
                }
                // Если нет, то попытаться найти первый сетевой (это первый раз когда нет auth
                if (!b) // Если не удалось записать
                {
                    string authName = "";
                    foreach (var drive in System.IO.DriveInfo.GetDrives())
                    {
                        if (drive.DriveType != System.IO.DriveType.Network)
                            continue;
                        // Найти первый сетевой куда можно записать
                        b = TryCreateAuthFile(authName, login, pass); // тут ошибка!!!
                        if (b)
                            break;
                    }
                }
                // Та редкая ситуация когда школьник заходит первый раз на первом занятии, не подключает диск и залогинивается в PABC
            }
            else // это домашний компьютер
            {
                var workDir = Plugin.WorkingDirectory();
                ///
                // сохранить пароль в каталоге на уровень выше WorkingDir. Если это корни дисков или уровнем выше нельзя, то в текущем
                // 
                //var auth = System.IO.Path.Combine(workDir, "auth.dat");
                var b1 = CalcParentDirectory(workDir, out string parent);
                bool b = false;
                if (b1 && !IsRootDirectory(parent)) // IsRootDirectory - parent = null !!
                {
                    // пытаемся в родительском
                    var auth = Path.Combine(parent, "auth.dat");
                    b = TryCreateAuthFile(auth, login, pass);
                    if (b)
                        AuthFileFullName = auth; // Эта информация нигде не используется - только для тестирования
                }
                if (!b)
                {
                    // пытаемся в текущем. Если он корень - ну, человек так хотел
                    var auth = Path.Combine(workDir, "auth.dat");
                    b = TryCreateAuthFile(auth, login, pass);
                    if (b)
                        AuthFileFullName = auth; // Эта информация нигде не используется - только для тестирования
                }
            }*/

            // ПО НОВОМУ SSM 19.08.22
            var AuthFileFullName = "";
            var workDir = Plugin.WorkingDirectory();
            var parentDir = "";
            var parentDirExists = false;
            var parentIsRoot = true;

            var fullworkDir = Path.GetFullPath(workDir);

            var di = Directory.GetParent(fullworkDir);
            if (di != null)
            {
                parentDir = di.FullName;
                parentDirExists = true;
                parentIsRoot = Directory.GetParent(parentDir) != null;
            }
            
            // На мехмате проверяем lightPT
            var isLightPT = Plugin.IsLightPTInWorkingDirectory();
            // Первый вариант - в родительский или текущий
            var authCreated = false;

            // Если в текущем есть lightpt.dat то мехмат или не мехмат - пытаемся создать файл авторизации в родительском или в текущем
            if (isLightPT)
            {
                // пытаемся создать в родительском
                if (parentDirExists)
                {
                    var auth = Path.Combine(parentDir, "auth.dat");
                    authCreated = TryCreateAuthFile(auth, login, pass);
                    if (authCreated)
                        AuthFileFullName = auth; // Эта информация нигде не используется - только для тестирования
                }
                // если не получилось, пытаемся создать в текущем
                if (!authCreated)
                {
                    var auth = Path.Combine(workDir, "auth.dat");
                    authCreated = TryCreateAuthFile(auth, login, pass);
                    if (authCreated)
                        AuthFileFullName = auth; // Эта информация нигде не используется - только для тестирования
                }
            }
            // Бывает так, что auth файл не создался. Обычно это означает, что в текущем нет lightPT.
            // Но может записать нельзя
            // Тогда остается последняя возможность. Если это мехмат, то ищем сетевой диск и пишем в корень туда
            if (!authCreated && Plugin.IsMechmath)
            {
                foreach (var drive in DriveInfo.GetDrives())
                {
                    if (drive.DriveType != System.IO.DriveType.Network)
                        continue;
                    var auth = Path.Combine(drive.Name, "auth.dat");
                    // Найти первый сетевой куда можно записать
                    authCreated = TryCreateAuthFile(auth, login, pass);
                    if (authCreated)
                    {
                        AuthFileFullName = auth; // Эта информация нигде не используется - только для тестирования
                        break;
                    }
                }
            }
        }

        public void SetRating(string rating)
        {
            var ss = rating.Split('|');
            labelRating.Text = $"Рейтинг: {ss[0]} баллов";
            labelDetails.Text = ss[1];
        }
        public void ChangeControlsAfterLogin(string login)
        {
            try
            {
                Text = "Авторизация: вход выполнен";
                usersNamesBox.Text = login;
                //var login = usersNamesBox.Text;
                //var pass = passwordBox.Text;
                passwordBox.Text = "";
                enterButton.Text = "Выход";
                passwordBox.Visible = false;
                labelPassword.Visible = false;
                groupNamesBox.Enabled = false;
                usersNamesBox.Enabled = false;
                Authorized = true;
                Plugin.toolStripButton.ToolTipText = "Авторизация выполнена: " + login;
                Plugin.toolStripButton.Image = PluginImageAuthorized.Image;
                Plugin.menuItem.Image = PluginImageAuthorized.Image;
                this.Icon = VisualPascalABCPlugins.Properties.Resources.IconAuthorized;
                closeButton.Focus();
                panelUnAuthorized.SendToBack();
                labelUserName.Text = login;
                //labelRating.Text = "Рейтинг: ";
            }
            catch (Exception e)
            {
                Plugin.AddMessage(e);
            }
        }

        public void ChangeControlsAfterLogout()
        {
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
            panelUnAuthorized.BringToFront();
            labelUserName.Text = "";
        }

        private async void enterButton_Click(object sender, EventArgs e)
        {
            if (SiteProvider == null)
                throw new Exception("Error in Login form! Site credentials cannot be empty!");

            if (!Authorized)
            {
                try
                {
                    if (Text.StartsWith("Авторизация: нет связи с сервером"))
                        Text = "Авторизация";


                    var answer = await SiteProvider.Login("", usersNamesBox.Text, passwordBox.Text);
                    if (answer == "Success")
                    {
                        WriteLoginPassToAuthDat(usersNamesBox.Text, passwordBox.Text);
                        ChangeControlsAfterLogin(usersNamesBox.Text);
                        var Rating = await SiteProvider.GetRating("", usersNamesBox.Text, passwordBox.Text);
                        SetRating(Rating);
                        //labelRating.Text = "Рейтинг: " + Rating.ToString();
                    }
                    else MessageBox.Show(answer, "Ошибка");
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    Text = "Авторизация: нет связи с сервером";
                    closeButton.Focus();
                }
                catch (Exception ex)
                {
                    Plugin.AddMessage(ex);
                }
            }
            else
            {
                SiteProvider.Logout();
                ChangeControlsAfterLogout();
            }

        }

        private async void LoginForm_Load(object sender, EventArgs e)
        {
            //  Получить список групп и заполнить выпадающий список значениями
            if (SiteProvider == null)
                throw new Exception("Error in Login form! Site credentials cannot be empty!");
            if (groupNamesBox.Items.Count > 0) // Группы уже загружены
                return;
            try
            {
                if (Text.StartsWith("Авторизация: нет связи с сервером"))
                    Text = "Авторизация";
                //groupImage.Image = Properties.Resources.LoadingImg;
                groupNamesBox.Items.Clear();
                var groups = await SiteProvider.GetGroupsNames();
                groupNamesBox.Items.AddRange(groups.Split(';').Where(Item => Item != "").ToArray());
                //groupImage.Image = null;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                Text = "Авторизация: нет связи с сервером";
                closeButton.Focus();
            }
            catch (Exception ex)
            {
                Plugin.AddMessage(ex);
            }
        }

        private async void GroupNamesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  Получить список участников группы
            if (groupNamesBox.Text == "") return;

            if (SiteProvider == null)
                throw new Exception("Error in Login form! Site credentials cannot be empty!");
            try
            {
                if (Text.StartsWith("Авторизация: нет связи с сервером"))
                    Text = "Авторизация";
                //var t = Text;
                //userImage.Image = Properties.Resources.LoadingImg;
                usersNamesBox.Items.Clear();
                var users = await SiteProvider.GetUsersNames(groupNamesBox.Text);
                usersNamesBox.Items.AddRange(users.Split(';').Where(Item => Item != "").ToArray());
                //userImage.Image = null;
                //Text = t;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                Text = "Авторизация: нет связи с сервером";
                closeButton.Focus();
            }
            catch (Exception ex)
            {
                Plugin.AddMessage(ex);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
            passwordBox.Text = "";
        }

        private void labelRating_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.labelRating.LinkVisited = true;
            if (string.IsNullOrEmpty(SiteProvider.Group))
                return;
            var group = SiteProvider.Group.Replace(' ', '+');

            System.Diagnostics.Process.Start("https://air.mmcs.sfedu.ru/pascalabc/index.php?groupName=" + group);
        }
    }
}
