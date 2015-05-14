using System;
using System.Collections.Generic;
using System.Text;

namespace VisualPascalABCPlugins
{
    public class AspectTree_VisualPascalABCPlugin : IVisualPascalABCPlugin
    {
        public static string StringsPrefix = "VPP_ASPTRV_";
        IVisualEnvironmentCompiler VisualEnvironmentCompiler;
        private AspectTreeForm AspectTreeForm = new AspectTreeForm();
        public string Name
        {
            get
            {
                return "AOP Plugin";
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
                return "(c)ZM 2010";
            }
        }

        public void Execute()
        {
            AspectTreeForm.Show();
        }

        public AspectTree_VisualPascalABCPlugin(IVisualEnvironmentCompiler VisualEnvironmentCompiler)
        {
            this.VisualEnvironmentCompiler = VisualEnvironmentCompiler;
            AspectTreeForm.VisualEnvironmentCompiler = VisualEnvironmentCompiler;
            

        }
        public void GetGUI(List<IPluginGUIItem> MenuItems, List<IPluginGUIItem> ToolBarItems)
        {
            PluginGUIItem Item = new PluginGUIItem(StringsPrefix + "NAME", StringsPrefix + "DESCRIPTION", AspectTreeForm.PluginImage.Image, AspectTreeForm.PluginImage.BackColor, Execute);            
            MenuItems.Add(Item);
            ToolBarItems.Add(Item);
        }

    }
}

