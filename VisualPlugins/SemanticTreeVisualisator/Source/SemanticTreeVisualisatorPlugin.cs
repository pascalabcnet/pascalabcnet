using System;
using System.Collections.Generic;
using System.Text;

namespace VisualPascalABCPlugins
{
    public class SyntaxTreeVisualisator_VisualPascalABCPlugin : IVisualPascalABCPlugin
    {
        public static string StringsPrefix = "VPP_SEMTRV_";
        IVisualEnvironmentCompiler VisualEnvironmentCompiler;
        private SemanticTreeVisualisatorForm SemanticTreeVisualisatorForm = new SemanticTreeVisualisatorForm();
        public string Name
        {
            get
            {
                return "SemanticTree Visualisator";
            }
        }
        public string Version
        {
            get
            {
                return "0.1";
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
            SemanticTreeVisualisatorForm.Show();
        }

        public SyntaxTreeVisualisator_VisualPascalABCPlugin(IWorkbench Workbench)
        {
            VisualEnvironmentCompiler = Workbench.VisualEnvironmentCompiler;
            SemanticTreeVisualisatorForm.VisualEnvironmentCompiler = VisualEnvironmentCompiler;
            

        }
        public void GetGUI(List<IPluginGUIItem> MenuItems, List<IPluginGUIItem> ToolBarItems)
        {
            PluginGUIItem Item = new PluginGUIItem(StringsPrefix + "NAME", StringsPrefix + "DESCRIPTION", SemanticTreeVisualisatorForm.PluginImage.Image, SemanticTreeVisualisatorForm.PluginImage.BackColor, Execute);
            MenuItems.Add(Item);
            ToolBarItems.Add(Item);
        }

    }
}

