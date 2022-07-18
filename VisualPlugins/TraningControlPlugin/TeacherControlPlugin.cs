using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Linq;
using PascalABCCompiler.SyntaxTreeConverters;
using PascalABCCompiler.SyntaxTree;

namespace VisualPascalABCPlugins
{
    /*public class TeacherContolConverter: ISyntaxTreeConverter
    {
        public string Name { get => "TeacherContolConverter"; }
        public syntax_tree_node Convert(syntax_tree_node root)
        {
            var program = root as program_module;
            if (program == null) // если это не главная программа, то не преобразовывать
                return root;
            var FileName = program.file_name;
            var fi = new System.IO.FileInfo(FileName);
            var SourceFileDirectory = fi.DirectoryName;

            var FullLightPTName = System.IO.Path.Combine(SourceFileDirectory, "lightpt.dat");
            if (!System.IO.File.Exists(FullLightPTName))
                return root;

            if (program.used_units == null)
                program.used_units = new uses_list();
            program.used_units.Add(new unit_or_namespace("LightPT", null));
            program.used_units.Add(new unit_or_namespace("Tasks", null));
            return root;
        }
    }*/

    //имя класса *_VisualPascalABCPlugin
    public class VisualPascalABCPlugin_TeacherControlPlugin : IVisualPascalABCPlugin
    {
        private const string LabelEntered = "Личный кабинет: ";
        private const string LabelNotEntered = "Личный кабинет: вход не выполнен";
        public static string StringsPrefix = "VPP_REGISTER_AND_CONTROL_PLUGIN_";
        IVisualEnvironmentCompiler VisualEnvironmentCompiler;

        IWorkbench Workbench;
        private TeacherControlForm RegisterForm = new TeacherControlForm();
        private PluginGUIItem Item;
        public string Name { get => "Teacher Control Plugin"; }
        public string Version { get => "0.1"; }
        public string Copyright { get => "Copyright © 2021-2022 by Stanislav Mikhalkovich"; }

        public string Login = null;
        public string Password = null;
        public void Execute()
        {
            var dr = RegisterForm.ShowDialog();
            if (RegisterForm.Registered)
                Item.Hint = LabelEntered + RegisterForm.Login;
            else Item.Hint = LabelNotEntered;
        }

        public VisualPascalABCPlugin_TeacherControlPlugin(IWorkbench Workbench)
        {
            this.Workbench = Workbench;
            VisualEnvironmentCompiler = Workbench.VisualEnvironmentCompiler;
            RegisterForm.VisualEnvironmentCompiler = VisualEnvironmentCompiler;

            // Регистрация обработчика
            this.Workbench.ServiceContainer.RunService.Starting += RunStartingHandler;
            //Workbench.ServiceContainer.BuildService.BeforeCompile += BeforeCompileHandler;
        }
        public void GetGUI(List<IPluginGUIItem> MenuItems, List<IPluginGUIItem> ToolBarItems)
        {
            Item = new PluginGUIItem(StringsPrefix + "NAME", StringsPrefix + "DESCRIPTION", RegisterForm.PluginImage.Image, RegisterForm.PluginImage.BackColor, Execute);
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

            if (RegisterForm.Registered)
            {
                System.IO.File.AppendAllText("d:\\runs.txt", filename + " " + DateTime.Now + '\n');
            }
        }
    }
}

