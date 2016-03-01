
using System;
using System.Collections.Generic;
using System.Text;

namespace VisualPascalABCPlugins
{
    public class HelpBuilder_VisualPascalABCPlugin : IVisualPascalABCPlugin
    {
		public static string StringsPrefix = "VPP_HLPBLDR_";
		IVisualEnvironmentCompiler VisualEnvironmentCompiler;
		private HelpBuilderForm HelpBuilderForm = new HelpBuilderForm();
		
		public string Name
        {
            get
            {
                return "HelpBuilder Plugin";
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
                return "Copyright © 2005-2016 by Ivan Bondarev, Stanislav Mihalkovich";
            }
        }
        
        public void Execute()
        {
            VisualEnvironmentCompiler.StandartCompiler.OnChangeCompilerState += new PascalABCCompiler.ChangeCompilerStateEventDelegate(HelpBuilderForm.Compiler_OnChangeCompilerState);
            HelpBuilderForm.Init();
            HelpBuilderForm.Show();
        }
        
        public HelpBuilder_VisualPascalABCPlugin(IWorkbench Workbench)
        {
            this.VisualEnvironmentCompiler = Workbench.VisualEnvironmentCompiler;
            HelpBuilderForm.VisualEnvironmentCompiler = Workbench.VisualEnvironmentCompiler;
            HelpBuilderForm.CreateBuilder();
        }
        
        public void GetGUI(List<IPluginGUIItem> MenuItems, List<IPluginGUIItem> ToolBarItems)
        {
            PluginGUIItem Item = new PluginGUIItem(StringsPrefix + "PLUGIN_NAME", StringsPrefix + "PLUGIN_DESCRIPTION", HelpBuilderForm.PluginImage.Image, HelpBuilderForm.PluginImage.BackColor, Execute);
            MenuItems.Add(Item);
            ToolBarItems.Add(Item);
        }
	}
}
