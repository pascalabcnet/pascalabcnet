// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace VisualPascalABCPlugins
{
    public class PluginLoadingError : Exception
    {
        public Exception Error;
        public string FileName;
        public PluginLoadingError(string FileName, Exception e)
            :base(FileName)
        {
            Error = e;
            this.FileName = FileName;
        }
        public override string ToString()
        {
            return string.Format(PascalABCCompiler.StringResources.Get("VPPC_PLUGIN_{0}_LOADING_ERROR_{1}"), Path.GetFileName(FileName),Error);
        }
    }
    public class PluginsController
    {
        private IWorkbench workbench;
        private List<IVisualPascalABCPlugin> plugins = new List<IVisualPascalABCPlugin>();
        public List<IVisualPascalABCPlugin> Plugins
        {
            get { return plugins; }
        }
        private List<PluginLoadingError> errorList = new List<PluginLoadingError>();
        public List<PluginLoadingError> ErrorList
        {
            get { return errorList; }
        }
        public System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem = null;
        public System.Windows.Forms.ToolStrip ToolStrip = null;
        public IVisualEnvironmentCompiler VisualEnvironmentCompiler = null;
        public PluginsController(IVisualEnvironmentCompiler VisualEnvironmentCompiler)
        {
            this.VisualEnvironmentCompiler = VisualEnvironmentCompiler;
        }
        public PluginsController(IVisualEnvironmentCompiler VisualEnvironmentCompiler,System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem, System.Windows.Forms.ToolStrip ToolStrip, IWorkbench workbench)
        {
            this.ToolStripMenuItem = ToolStripMenuItem;
            this.ToolStrip = ToolStrip;
            this.VisualEnvironmentCompiler = VisualEnvironmentCompiler;
            this.workbench = workbench;
        }

        bool AddMenuSeparator = false;
        bool AddToolBarSeparator = false;
        delegate void AddInGuiDelegate(IVisualPascalABCPlugin Plugin);
        private void AddInGUI(IVisualPascalABCPlugin Plugin)
        {
            if (ToolStripMenuItem == null) return;
            List<IPluginGUIItem> MenuItems=new List<IPluginGUIItem>(),ToolBarItems=new List<IPluginGUIItem>();
            Plugin.GetGUI(MenuItems, ToolBarItems);
            if (MenuItems.Count > 1 && (ToolStripMenuItem.DropDownItems.Count > 0 && (ToolStripMenuItem.DropDownItems[ToolStripMenuItem.DropDownItems.Count - 1].Tag is IPluginGUIItem)))
            {
                ToolStripMenuItem.DropDownItems.Add(new System.Windows.Forms.ToolStripSeparator());
                AddMenuSeparator = false;
            }
            if (MenuItems.Count > 0)
            {
                if (AddMenuSeparator)
                {
                    AddMenuSeparator = false;
                    ToolStripMenuItem.DropDownItems.Add(new System.Windows.Forms.ToolStripSeparator());
                }
                foreach (IPluginGUIItem Item in MenuItems)
                {
                    System.Windows.Forms.ToolStripMenuItem menuItem = new System.Windows.Forms.ToolStripMenuItem();
                    menuItem.Image = Item.Image;
                    menuItem.ImageTransparentColor = Item.ImageTransparentColor;
                    menuItem.Text = Item.Text;
                    //menuItem.ToolTipText = Item.Hint;
                    menuItem.Tag = Item;
                    Item.menuItem = menuItem;
                    menuItem.ShortcutKeys = Item.ShortcutKeys;
                    if (Item.ShortcutKeyDisplayString != null)
                        menuItem.ShortcutKeyDisplayString = Item.ShortcutKeyDisplayString;
                    menuItem.Click += new EventHandler(item_Click);
                    ToolStripMenuItem.DropDownItems.Add(menuItem);
                    PascalABCCompiler.StringResources.SetTextForObject(menuItem, "");
                }
            }
            if (MenuItems.Count > 1)
                AddMenuSeparator = true;
            if (ToolStrip.Items.Count > 0 && !(ToolStrip.Items[ToolStrip.Items.Count - 1] is System.Windows.Forms.ToolStripSeparator) && ToolBarItems.Count > 1 && ((ToolStrip.Items[ToolStrip.Items.Count - 1].Tag is IPluginGUIItem)))
            {
                ToolStrip.Items.Add(new System.Windows.Forms.ToolStripSeparator());
                AddToolBarSeparator = false;
            }
            if (ToolBarItems.Count > 0)
            {
                if (AddToolBarSeparator)
                {
                    AddToolBarSeparator = false;
                    ToolStrip.Items.Add(new System.Windows.Forms.ToolStripSeparator());
                }
                foreach (IPluginGUIItem Item in ToolBarItems)
                {
                    System.Windows.Forms.ToolStripButton button = null;
                    button = new System.Windows.Forms.ToolStripButton();
                    button.Image = Item.Image;
                    button.ImageTransparentColor = Item.ImageTransparentColor;
                    //button.Text = Item.Text;
                    button.ToolTipText = Item.Hint;
                    button.Click += new EventHandler(button_Click);
                    button.Tag = Item;
                    Item.toolStripButton = button;
                    ToolStrip.Items.Add(button);
                    PascalABCCompiler.StringResources.SetTextForObject(button, "");
                }
            }
            if (ToolBarItems.Count > 1)
                AddToolBarSeparator = true;
            //ToolStrip.Refresh();
            if (ToolStripMenuItem.Visible == false) 
                ToolStripMenuItem.Visible = true;
        }
        void button_Click(object sender, EventArgs e)
        {
            ((IPluginGUIItem)((System.Windows.Forms.ToolStripButton)sender).Tag).Execute();
        }

        void item_Click(object sender, EventArgs e)
        {
            ((IPluginGUIItem)((System.Windows.Forms.ToolStripItem)sender).Tag).Execute();
        }
        public void AddPlugins()
        {
            AddPlugins(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName));
        }
        public void AddPlugins(string DirectoryName)
        {
            DirectoryInfo di = new DirectoryInfo(DirectoryName);
            FileInfo[] dllfiles = di.GetFiles("*.dll");
            System.Reflection.Assembly assembly = null;
            VisualPascalABCPlugins.IVisualPascalABCPlugin Plugin;
            foreach (FileInfo fi in dllfiles)
            {
                switch (fi.Name.ToLower())
                {
                    case "avalondock.dll":
                    case "codecompletion.dll":
                    case "compiler.dll":
                    case "compilertools.dll":
                    case "debuggercore.dll":
                    case "netgenerator.dll":
                    case "treeconverter.dll":
                    case "pascalabcparser.dll":
                    case "syntaxtree.dll":
                    case "semantictree.dll":
                    case "weifenluo.winformsui.docking.dll":
                    case "pluginssupport.dll":
                    case "localization.dll":
                        continue;
                }
                if (fi.Name.StartsWith("NETXP.", StringComparison.InvariantCultureIgnoreCase) || fi.Name.StartsWith("Mono.", StringComparison.InvariantCultureIgnoreCase) || fi.Name.StartsWith("ICSharpCode.", StringComparison.InvariantCultureIgnoreCase))
                    continue;
                try
                {
                    assembly = System.Reflection.Assembly.LoadFile(fi.FullName);
                    try
                    {
                        Type[] types = assembly.GetTypes();

                        foreach (Type type in types)
                        {
                            if (type.Name.IndexOf("VisualPascalABCPlugin") >= 0 && type.IsClass)
                            {
                                Object obj = Activator.CreateInstance(type, workbench);
                                if (obj is VisualPascalABCPlugins.IVisualPascalABCPlugin)
                                {
                                    Plugin = obj as VisualPascalABCPlugins.IVisualPascalABCPlugin;
                                    Plugins.Add(Plugin);
                                    VisualEnvironmentCompiler.BeginInvoke(new AddInGuiDelegate(AddInGUI), Plugin);
                                }
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        ErrorList.Add(new PluginLoadingError(fi.FullName, e));
                    }

                }
                catch (Exception e)
                {
                    //если dll не нетовская
                }
            }
            
        }
    }
}
