using System;
using System.Collections.Generic;
using System.Text;

namespace VisualPascalABCPlugins
{
    //имя класса *_VisualPascalABCPlugin
    public class TestPlugin_VisualPascalABCPlugin : IVisualPascalABCPlugin
    {
        public static string StringsPrefix = "VPP_TESTPLUGIN_";
        IVisualEnvironmentCompiler VisualEnvironmentCompiler;
        IWorkbench Workbench;
        private TestForm testForm = new TestForm();
        public string Name
        {
            get
            {
                return "Test Plugin";
            }
        }
        public string Version
        {
            get
            {
                return "1.0";
            }
        }
        public string Copyright
        {
            get
            {
                return "Copyright © 2005-2016 by Ivan Bondarev, Stanislav Mihalkovich";
            }
        }

        public void Execute()
        {
            testForm.Show();
        }

        public TestPlugin_VisualPascalABCPlugin(IWorkbench Workbench)
        {
            this.Workbench = Workbench;
            VisualEnvironmentCompiler = Workbench.VisualEnvironmentCompiler;
            testForm.VisualEnvironmentCompiler = VisualEnvironmentCompiler;
            

        }
        public void GetGUI(List<IPluginGUIItem> MenuItems, List<IPluginGUIItem> ToolBarItems)
        {
            PluginGUIItem Item = new PluginGUIItem(StringsPrefix + "NAME", StringsPrefix + "DESCRIPTION", testForm.PluginImage.Image, testForm.PluginImage.BackColor, Execute);
            MenuItems.Add(Item);
            ToolBarItems.Add(Item);
        }

    }
}

