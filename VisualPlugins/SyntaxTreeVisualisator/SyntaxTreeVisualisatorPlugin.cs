using System;
using System.Collections.Generic;
using System.Text;

namespace VisualPascalABCPlugins
{
    public class SyntaxTreeVisualisator_VisualPascalABCPlugin : IVisualPascalABCPlugin
    {
        public static string StringsPrefix = "VPP_SYNTRV_";
        IVisualEnvironmentCompiler VisualEnvironmentCompiler;
        private SyntaxTreeVisualisatorForm SyntaxTreeVisualisatorForm = new SyntaxTreeVisualisatorForm();
        public string Name
        {
            get
            {
                return "SyntaxTree Visualisator";
            }
        }
        public string Version
        {
            get
            {
                return "1.1";
            }
        }
        public string Copyright
        {
            get
            {
                return "Copyright © 2005-2024 by Ivan Bondarev, Stanislav Mikhalkovich";
            }
        }

        public void Execute()
        {
            SyntaxTreeVisualisatorForm.Show();
        }

        public SyntaxTreeVisualisator_VisualPascalABCPlugin(IWorkbench Workbench)
        {
            VisualEnvironmentCompiler = Workbench.VisualEnvironmentCompiler;
            SyntaxTreeVisualisatorForm.VisualEnvironmentCompiler = VisualEnvironmentCompiler;
            

        }
        public void GetGUI(List<IPluginGUIItem> MenuItems, List<IPluginGUIItem> ToolBarItems)
        {
            PluginGUIItem Item = new PluginGUIItem(StringsPrefix + "NAME", StringsPrefix + "DESCRIPTION", SyntaxTreeVisualisatorForm.PluginImage.Image, SyntaxTreeVisualisatorForm.PluginImage.BackColor, Execute);
            MenuItems.Add(Item);
            ToolBarItems.Add(Item);
        }

    }
}

