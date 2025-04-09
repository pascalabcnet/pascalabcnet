using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using VisualPascalABCPlugins.DBAccess;
using PascalABCCompiler;

namespace VisualPascalABCPlugins
{
    //имя класса *_VisualPascalABCPlugin
    public class VisualPascalABCPlugin_TeacherControlPlugin : IExtendedVisualPascalABCPlugin
    {
        private const string LabelEntered = "Авторизация: ";
        private const string LabelNotEntered = "Авторизация: вход не выполнен";
        public static string StringsPrefix = "VPP_REGISTER_AND_CONTROL_PLUGIN_";
        public static string ServerAddress = "https://air.mmcs.sfedu.ru/pascalabc";
        IVisualEnvironmentCompiler VisualEnvironmentCompiler;

        IWorkbench Workbench;
        //private TeacherControlForm RegisterForm = new TeacherControlForm(); // старая форма авторизации
        //private LoginForm RegisterFormNew = new LoginForm(); // новая форма авторизации
        // User
        SiteAccessProvider User;

        private PluginGUIItem Item;
        public string Name { get => "Teacher Control Plugin"; }
        public string Version { get => "0.2"; }
        public string Copyright { get => "Copyright © 2021-2025 by Stanislav Mikhalkovich"; }

        public string Login = null;
        public string Password = null;

        public LoginForm loginForm;

        public ToolStripMenuItem menuItem = null;
        public ToolStripButton toolStripButton = null;

        // public string AuthFileFullName = "";

        public bool IsMechmath;

        public VisualPascalABCPlugin_TeacherControlPlugin(IWorkbench Workbench)
        {
            this.Workbench = Workbench;
            VisualEnvironmentCompiler = Workbench.VisualEnvironmentCompiler;            

            User = new SiteAccessProvider();

            if (IsLightPTInWorkingDirectory())
            {
                IsMechmath = System.Environment.MachineName.ToLower().StartsWith("mil8a-");
                // Попытаемся изменить ServerAddress если в папке есть server.dat
                TryChangeServerAddress(ref ServerAddress);
                User.ServAddr = ServerAddress;
                loginForm = new LoginForm(this);
            }
            // RegisterForm.VisualEnvironmentCompiler = VisualEnvironmentCompiler; // Пока форма регистрации никак не связана с компилятором

            // Регистрация обработчика
            this.Workbench.ServiceContainer.RunService.Starting += RunStartingHandler;
            this.Workbench.ServiceContainer.RunService.ChangeArgsBeforeRun += ChangeArgsBeforeRunHandler;
            //VisualEnvironmentCompiler.Compiler.SourceFilesProvider = TeacherSourceFilesProvider;
            //Workbench.ServiceContainer.BuildService.BeforeCompile += BeforeCompileHandler;
        }

        public object TeacherSourceFilesProvider(string FileName, SourceFileOperation FileOperation)
        {
            switch (FileOperation)
            {
                case SourceFileOperation.GetText:
                    if (!File.Exists(FileName)) return null;
                    string Text = FileReader.ReadFileContent(FileName, null);
                    // Здесь можно дешифровать когда надо
                    //File.AppendAllText("d:\\aaaa.txt", FileName + "\n");
                    return Text;
                case SourceFileOperation.Exists:
                    return File.Exists(FileName);
                case SourceFileOperation.GetLastWriteTime:
                    return File.GetLastWriteTime(FileName);
            }
            return null;
        }

        public void Execute()
        {
            loginForm.SiteProvider = User;
            loginForm.ShowDialog(); // OK никогда не будет
        }

        public void AddMessage(string text)
        {
            Workbench.CompilerConsoleWindow.AddTextToCompilerMessages(text + Environment.NewLine);
        }
        public void AddMessage(Exception e) => AddMessage(e.ToString());

        public string WorkingDirectory()
        {
            var FileName = Workbench.ServiceContainer.DocumentService.CurrentCodeFileDocument.FileName;
            return System.IO.Path.GetDirectoryName(FileName);
        }

        public string FullLightPTName()
        {
            var WorkingDir = WorkingDirectory();
            var lightptname = Path.Combine(WorkingDir, "lightpt.dat");
            if (File.Exists(lightptname))
                return lightptname;
            return "";
        }

        public bool IsLightPTInWorkingDirectory()
        {
            var WorkingDir = WorkingDirectory();
            var lightptname = Path.Combine(WorkingDir, "lightpt.dat");
            return File.Exists(lightptname);
        }

        public bool TryChangeServerAddress(ref string ServerAddress)
        {
            var WorkingDir = WorkingDirectory();
            var server_dat_name = Path.Combine(WorkingDir, "server.dat");
            if (File.Exists(server_dat_name))
            {
                try
                {
                    ServerAddress = System.IO.File.ReadAllText(server_dat_name).Trim();
                    return true;
                }
                catch
                {
                    return false; // не поменяли ServerName
                    // погасить исключение
                }
            }
            return false;
        }

        public /*async*/ void TryLogin(string login, string pass)
        {
            // Гасим все исключения
            try
            {
                var answer = User.Login("", login, pass);
                if (answer.Result == "Success")
                {
                    loginForm.ChangeControlsAfterLogin(login);
                    var Rating = User.GetRating("", login, pass);
                    loginForm.SetRating(Rating.Result);
                }
            }
            catch (Exception e)
            {
                AddMessage(e);
            }
        }

        public string FullAuthNameFromNETDisk()
        {
            var AuthFileFullName = "";
            try
            {
                foreach (var drive in System.IO.DriveInfo.GetDrives())
                {
                    if (drive.DriveType != System.IO.DriveType.Network)
                        continue;
                    // Проверять, что диск сетевой!!! Для несетевых - нет!
                    var auth = System.IO.Path.Combine(drive.Name, "auth.dat");
                    if (System.IO.File.Exists(auth))
                        AuthFileFullName = auth;
                }
            }
            catch (Exception)
            { } // Тут исключение быть не может и даже если, то неинформативно

            return AuthFileFullName;
        }

        public string FullAuthNameFromWorkingDirOrParent()
        {
            var AuthFileFullName = "";
            try
            {
                var WorkingDir = WorkingDirectory();
                var auth = Path.Combine(WorkingDir, "auth.dat");
                var exists = false;
                if (File.Exists(auth)) // искать auth в текущей папке. Если нет, то 
                    exists = true;
                else
                {
                    auth = Path.Combine(WorkingDir, "..", "auth.dat"); // искать auth в папке на уровень выше
                    if (File.Exists(auth))
                        exists = true;
                }
                if (exists) // то в auth - его путь Преобразуем его в полный путь
                {
                    var fi = new System.IO.FileInfo(auth);
                    AuthFileFullName = fi.FullName;
                }
            }
            catch (Exception)
            { } 

            return AuthFileFullName;
        }

        public string CalcAuthFullName()
        {
            var AuthFileFullName = "";

            // На мехмате - проверяю все диски на предмет существования в корне файла auth.dat
            if (IsMechmath)
                AuthFileFullName = FullAuthNameFromNETDisk();
            else // Это домашний компьютер
                AuthFileFullName = FullAuthNameFromWorkingDirOrParent();

            return AuthFileFullName;
        }

        public void InitItems(List<IPluginGUIItem> MenuItems, List<IPluginGUIItem> ToolBarItems)
        {
            Item = new PluginGUIItem(StringsPrefix + "NAME", StringsPrefix + "DESCRIPTION", loginForm.PluginImage.Image, loginForm.PluginImage.BackColor, Execute);
            MenuItems.Add(Item);
            ToolBarItems.Add(Item);
        }

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

            // При ручной авторизации - сохранение
            //  Мехмат: auth сохраняется в корень сетевого диска
            //  Дома: auth сохраняется в папке на уровень выше текущей. Если это невозможно, то в текущей папке. 
            //   Если это диск C, то тоже не сохранять и тогда сохранять в текущей

            // При запуске кнопки плагина показываются
            //  Дома: всегда у тех, у кого установлен плагин
            //  Мехмат: при наличии lightpt в текущей папке или при наличии auth.dat в корне сетевого диска !!!
            //if (!IsMechmath) // дома - всегда
            //    InitItems(MenuItems, ToolBarItems);
            //else // это мехмат

            // SSM 20/06/22 Решил включить плагин в инсталлят и показывать кнопки только если есть lightpt.dat в текущем 
            // или auth.dat в корне сетевого - убрал это 11.07.23 - только если при запуске lightpt.dat в текущем!!!
            {
                if (IsLightPTInWorkingDirectory()) // если lightpt.dat существует в текущем
                    InitItems(MenuItems, ToolBarItems);
                /*else
                {
                    var AuthFileFullName = FullAuthNameFromNETDisk();
                    if (AuthFileFullName != "") // если auth.dat существует в корне сетевого диска
                        InitItems(MenuItems, ToolBarItems);
                }*/
            }
        }

        public void AfterAddInGUI()
        {
            if (Item == null) // Если не показали кнопки, то нет смысла в дальнейших действиях - плагин неактивен
            {
                AddMessage("TeacherControlPlugin deactivated"); // Не выполнено условие LightPTExists || AuthExists
                return;
            }
            menuItem = (ToolStripMenuItem)Item.menuItem;
            toolStripButton = (ToolStripButton)Item.toolStripButton;

            // При запуске auth.dat ищется и делается попытка авторизации:
            //  Мехмат: в корне сетевого диска (только!). Если находится, делается попытка авторизации
            //  Дома: в текущей папке. Если нет, ищется в папке на уровень выше. Если находится, делается попытка авторизации
            //  При этом на мехмате не ищется lightpt.dat в текущем

            // Версия. Разрешить на мехмате автоматически авторизоваться если заходим из папки с lightpt.dat 
            // А если его нет, то ищем auth в корне сетевого
            // Последовательность поиска auth.dat на мехмате:
            //  В текущем, на уровень выше (только если в текущем есть lightpt) и на сетевом
            try
            {
                // Попытка автоматического входа 
                // AuthFileFullName для мехмата был запомнен в GetGUI
                // SSM 11/07/23 - нет, AuthFileFullName для мехмата отдельно теперь не перевычисляется для простоты!!!
                // Нет - только на мехмате. Заполнить если дома!!!
                // Думаю, здесь всё надо перевычислить. Начать с текущего, потом на уровень выше и потом в корень сетевого диска

                // !!! НАДО ТУТ ИСКАТЬ lightpt.dat в текущем!!!
                var AuthFileFullName = "";
                if (IsLightPTInWorkingDirectory()) // Только если lightPT.dat в текущем, ищем auth.dat в текущем или выше
                    AuthFileFullName = FullAuthNameFromWorkingDirOrParent();
                /*if (AuthFileFullName == "" && IsMechmath)
                {
                    AuthFileFullName = FullAuthNameFromNETDisk(); 
                }*/

                if (AuthFileFullName != "")
                {
                    //var ss = File.ReadAllLines(AuthFileFullName);
                    var ss = TeacherPluginUtils.ReadLoginPassFromAuth(AuthFileFullName);
                    if (ss.Length >= 2)
                    {
                        var login = ss[0];
                        var pass = ss[1];
                        // В TryLogin может быть исключение, которое не перехватывается этим кодом, т.к.в другом потоке
                        System.Threading.Tasks.Task.Run(() => TryLogin(login, pass));
                    }
                }
            }
            catch (Exception e)
            {
                AddMessage(e); // И тут уже вывести причину. Она только в ReadLoginPassFromAuth может быть
            }
        }

        private void RunStartingHandler(string filename)
        {
        }

        private void ChangeArgsBeforeRunHandler(ref string args)
        {
            if (Item == null) // если нет кнопок с человечком, то в глобальную базу писаться не будет в принципе!
                return;
            args += " " + loginForm.Authorized.ToString(); 
            if (loginForm.Authorized) // Параметр Текст программы добавляется только если зарегистрирован
            {
                var FileName = Workbench.ServiceContainer.DocumentService.CurrentCodeFileDocument.FileName;
                var text = Workbench.VisualEnvironmentCompiler.StandartCompiler.GetSourceFileText(FileName);
                if (text.Length > 5000)
                    text = text.Substring(0, 5000);
                text = text.Replace("\\\"", "\\ \"");
                text = text.Replace("\"", "\\\"");
                args += " " + "\"" + text + "\"";
            }
        }
    }
}

