using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Linq;
using PascalABCCompiler.SyntaxTreeConverters;
using PascalABCCompiler.SyntaxTree;
using DBAccessPluginNamespace;

namespace VisualPascalABCPlugins
{
    //имя класса *_VisualPascalABCPlugin
    public class VisualPascalABCPlugin_TeacherControlPlugin : IExtendedVisualPascalABCPlugin
    {
        private const string LabelEntered = "Авторизация: ";
        private const string LabelNotEntered = "Авторизация: вход не выполнен";
        public static string StringsPrefix = "VPP_REGISTER_AND_CONTROL_PLUGIN_";
        IVisualEnvironmentCompiler VisualEnvironmentCompiler;

        IWorkbench Workbench;
        //private TeacherControlForm RegisterForm = new TeacherControlForm(); // старая форма авторизации
        //private LoginForm RegisterFormNew = new LoginForm(); // новая форма авторизации
        // User
        SiteAccessProvider User = new SiteAccessProvider();

        private PluginGUIItem Item;
        public string Name { get => "Teacher Control Plugin"; }
        public string Version { get => "0.1"; }
        public string Copyright { get => "Copyright © 2021-2022 by Stanislav Mikhalkovich"; }

        public string Login = null;
        public string Password = null;

        public LoginForm loginForm;

        public ToolStripMenuItem menuItem = null;
        public ToolStripButton toolStripButton = null;

        public string AuthFileFullName = "";

        public VisualPascalABCPlugin_TeacherControlPlugin(IWorkbench Workbench)
        {
            User.ServAddr = "https://air.mmcs.sfedu.ru/pascalabc";
            loginForm = new LoginForm(this);
            this.Workbench = Workbench;
            VisualEnvironmentCompiler = Workbench.VisualEnvironmentCompiler;
            //var tbitem = Workbench.MainForm.Controls.Find("", true);
            // RegisterForm.VisualEnvironmentCompiler = VisualEnvironmentCompiler; // Пока форма регистрации никак не связана с компилятором

            // Регистрация обработчика
            this.Workbench.ServiceContainer.RunService.Starting += RunStartingHandler;
            //Workbench.ServiceContainer.BuildService.BeforeCompile += BeforeCompileHandler;
        }
        public void Execute()
        {
            loginForm.SiteProvider = User;

            loginForm.ShowDialog(); // OK никогда не будет
            /*if (loginForm.Authorized)
            {
                toolStripButton.ToolTipText = "Авторизация выполнена";
                toolStripButton.Image = loginForm.PluginImageAuthorized.Image;
            }
            else
            {
                toolStripButton.ToolTipText = "Авторизация: вход не выполнен";
                toolStripButton.Image = loginForm.PluginImage.Image;
            }*/
        }

        public string FullAuthNameFromNETDisk()
        {
            string authName = "";
            try
            {
                foreach (var drive in System.IO.DriveInfo.GetDrives())
                {
                    if (drive.DriveType != System.IO.DriveType.Network)
                        continue;
                    // Проверять, что диск сетевой!!! Для несетевых - нет!
                    var auth = System.IO.Path.Combine(drive.Name, "auth.dat");
                    if (System.IO.File.Exists(auth))
                        authName = auth;
                }
            }
            catch (Exception)
            {
            }
            return authName;
        }

        public string WorkingDirectory()
        {
            var FileName = Workbench.ServiceContainer.DocumentService.CurrentCodeFileDocument.FileName;
            return System.IO.Path.GetDirectoryName(FileName);
        }

        public string FullLightPTName()
        {
            var WorkingDir = WorkingDirectory();
            var lightptname = System.IO.Path.Combine(WorkingDir, "lightpt.dat");
            if (System.IO.File.Exists(lightptname))
                return lightptname;
            return "";
        }

        public /*async*/ void TryLogin(string login, string pass)
        {
            var answer = User.Login("", login, pass);
            if (answer.Result == "Success")
            {
                loginForm.ChangeControlsAfterLogin(login);
            }
        }

        public bool IsMechmath() => System.Environment.MachineName.ToLower().StartsWith("mil8a-");

        public void GetGUI(List<IPluginGUIItem> MenuItems, List<IPluginGUIItem> ToolBarItems)
        {
            // Для студентов: если в рабочем каталоге нет lightpt.dat, то кнопки не появляются
            // На самом деле надо еще проверять диск P: - точнее, сетевой диск с определенной меткой (например, в его корне лежит файл auth.dat
            // if (!File.Exists(Path.Combine(WorkingDir,'lightpt.dat'))
            //   return;
            // Scan disks
            // Find NET disk with auth.dat in root
            // if not - return
            //var WorkingDirectory = Workbench.VisualEnvironmentCompiler.Compiler.CompilerOptions.SourceFileDirectory;
            var LightPTExists = false;
            var AuthExists = false;
            try
            {
                // Только на мехмате - проверяю все диски на предмет существования в корне файла auth.dat
                if (IsMechmath())
                    AuthFileFullName = FullAuthNameFromNETDisk();

                LightPTExists = FullLightPTName() != "";
                AuthExists = AuthFileFullName != "";
            }
            catch (Exception)
            {
                // Погасили исключение
            }

            // При запуске иконка плагина показывается
            // Мехмат: при наличии auth.dat в корне сетевого диска или при наличии lightpt в текущей папке !!!
            // Дома: всегда у тех, у кого установлен плагин

            // При запуске auth.dat ищется 
            // Мехмат: в корне сетевого диска (только!). Если находится, делается попытка авторизации
            // Дома: в текущей папке. Если нет, ищется в папке на уровень выше. Если находится, делается попытка авторизации

            // При ручной авторизации - сохранение
            // Мехмат: auth сохраняется в корень сетевого диска
            // Дома: auth сохраняется в папке на уровень выше текущей. Если это невозможно, то в текущей папке. 
            // Если это диск C, то тоже не сохранять и тогда сохранять в текущей

            if (
                (IsMechmath() && (LightPTExists || AuthExists)) ||
                !IsMechmath() // дома - всегда
                )
            {
                Item = new PluginGUIItem(StringsPrefix + "NAME", StringsPrefix + "DESCRIPTION", loginForm.PluginImage.Image, loginForm.PluginImage.BackColor, Execute);
                MenuItems.Add(Item);
                ToolBarItems.Add(Item);
            }
        }

        public void AfterAddInGUI()
        {
            menuItem = (ToolStripMenuItem)Item.menuItem;
            toolStripButton = (ToolStripButton)Item.toolStripButton;
            // Попытка выполнить автоматический вход из информации в auth.dat
            // Прочитать логин и хеш пароля в auth.dat
            // По идее в БД надо хранить хеш пароля. 
            // Авторизоваться по этому паролю через БД
            // Непонятно, как сделать эту часть асинхронно

            // В AuthFileFullName только auth с сетевого диска. Если он пустой, то для дома попытаться добавить сюда 
            // текущую папку и потом корневую папку
            try { 
                if (!IsMechmath()) // если дома, то искать auth в текущей папке
                {
                    var WorkingDir = WorkingDirectory();

                    var auth = System.IO.Path.Combine(WorkingDir,"auth.dat");
                    // искать auth в текущей папке
                    if (System.IO.File.Exists(auth))
                        AuthFileFullName = auth;
                    else
                    {
                        // искать auth в папке на уровень выше
                        auth = System.IO.Path.Combine(WorkingDir, "..", "auth.dat");
                        if (System.IO.File.Exists(auth))
                            AuthFileFullName = auth;
                    }
                }
                if (AuthFileFullName != "")
                {
                    var ss = System.IO.File.ReadAllLines(AuthFileFullName);
                    if (ss.Length >= 2)
                    {
                        var login = ss[0];
                        var pass = ss[1];
                        System.Threading.Tasks.Task.Run(() => TryLogin(login, pass));
                    }
                }
            }
            catch (Exception)
            { }
        }

        private void RunStartingHandler(string filename)
        {
            /*if (RegisterForm.Registered)
            {
                System.IO.File.AppendAllText("d:\\runs.txt", filename + " " + DateTime.Now + '\n');
            }*/
        }
    }
}

