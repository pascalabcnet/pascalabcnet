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
    public class VisualPascalABCPlugin_TeacherControlPlugin : IVisualPascalABCPlugin
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

        bool firstTime = true;
        public ToolStripMenuItem menuItem = null;
        public ToolStripButton toolStripButton = null;

        public VisualPascalABCPlugin_TeacherControlPlugin(IWorkbench Workbench)
        {
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
            if (firstTime)
            {
                firstTime = false;
                menuItem = (ToolStripMenuItem)Item.menuItem;
                toolStripButton = (ToolStripButton)Item.toolStripButton;
            }

            User.ServAddr = "https://air.mmcs.sfedu.ru/pascalabc";
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

        public void GetGUI(List<IPluginGUIItem> MenuItems, List<IPluginGUIItem> ToolBarItems)
        {
            Item = new PluginGUIItem(StringsPrefix + "NAME", StringsPrefix + "DESCRIPTION", loginForm.PluginImage.Image, loginForm.PluginImage.BackColor, Execute);
            MenuItems.Add(Item);
            ToolBarItems.Add(Item);
        }

        /*private void BeforeCompileHandler(string filename) // filename - pas-файл
        {
            // Проверяем присутствие файла lightpt.dat
            var fi = new System.IO.FileInfo(filename);
            var SourceFileDirectory = fi.DirectoryName;

            var FullLightPTName = System.IO.Path.Combine(SourceFileDirectory, "lightpt.dat");

            // VisualEnvironmentCompiler.Compiler.SyntaxTreeConvertersController возвращает NotSupportedException для RemoteCompiler. Странно... И как это вообще срабатывает?

            var SyntaxTreeConverters = VisualEnvironmentCompiler.Compiler.SyntaxTreeConvertersController.SyntaxTreeConverters;
            // Если осталось с прошлого раза
            SyntaxTreeConverters.RemoveAll(st => st.Name == "TeacherContolConverter");
            //if (System.IO.File.Exists(FullLightPTName)) // Если в папке есть файл lightpt.dat
            {
                //SyntaxTreeConverters.Add(new TeacherContolConverter());
            }

            //System.IO.File.AppendAllText("d:\\Stmd.txt", string.Join(" ", StandartModules.Select(sm => sm.Name)) + '\n');
            //System.IO.File.AppendAllText("d:\\Stmd.txt", FullLightPTName + " " + DateTime.Now + '\n');
        }*/

        private void RunStartingHandler(string filename)
        {

            /*if (RegisterForm.Registered)
            {
                System.IO.File.AppendAllText("d:\\runs.txt", filename + " " + DateTime.Now + '\n');
            }*/
        }
    }
}

