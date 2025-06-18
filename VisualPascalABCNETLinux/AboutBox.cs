// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace VisualPascalABC
{
    public partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            //  Initialize the AboutBox to display the product information from the assembly information.
            //  Change assembly information settings for your application through either:
            //  - Project->Properties->Application->Assembly Information
            //  - AssemblyInfo.cs
            /*this.Text = String.Format("About {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            this.textBoxDescription.Text = AssemblyDescription;*/
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                // Get all Title attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                // If there is at least one Title attribute
                if (attributes.Length > 0)
                {
                    // Select the first one
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    // If it is not an empty string, return it
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                // Get all Description attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                // If there aren't any Description attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Description attribute, return its value
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                // Get all Product attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                // If there aren't any Product attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Product attribute, return its value
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright(Assembly a)
        {
            
                // Get all Copyright attributes on this assembly
                object[] attributes = a.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                // If there aren't any Copyright attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Copyright attribute, return its value
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
        }

        public string AssemblyCompany
        {
            get
            {
                // Get all Company attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                // If there aren't any Company attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Company attribute, return its value
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void AboutBox_Load(object sender, EventArgs e)
        {
			
        }

        private void AboutBox_Shown(object sender, EventArgs e)
        {
            try
            {
                lVersion.Text = string.Format("{0}, сборка {1} ({2})", PascalABCCompiler.Compiler.ShortVersion, RevisionClass.Revision, PascalABCCompiler.Compiler.VersionDateTime.ToShortDateString());
                dgvModules.Items.Clear();

                string apppatch = Path.GetDirectoryName(Application.ExecutablePath);

                VisualEnvironmentCompiler vec = (Owner as Form1).VisualEnvironmentCompiler;
                PascalABCCompiler.ICompiler comp = vec.StandartCompiler;
                if (comp != null)
                {
                    Assembly a = Assembly.GetAssembly(comp.GetType());
                    dgvModules.Items.Add(MakeItem("Core", "PascalABCCompiler.Core", a.GetName().Version.ToString(), "Copyright © 2005-2025 by Ivan Bondarev, Stanislav Mikhalkovich"));
                    foreach (Languages.Facade.ILanguage language in Languages.Facade.LanguageProvider.Instance.Languages)
                        dgvModules.Items.Add(MakeItem("Language", language.Name, language.Version, language.Copyright));
                    foreach (PascalABCCompiler.SemanticTreeConverters.ISemanticTreeConverter conv in comp.SemanticTreeConvertersController.SemanticTreeConverters)
                        dgvModules.Items.Add(MakeItem("Converter", conv.Name, conv.Version, conv.Copyright));
                    foreach (VisualPascalABCPlugins.IVisualPascalABCPlugin plugin in vec.PluginsController.Plugins)
                        dgvModules.Items.Add(MakeItem("Plugin", plugin.Name, plugin.Version, plugin.Copyright));

                }

                //dgvModules.Items.Add(MakeItem(Assembly.LoadFile(Path.Combine(apppatch,"ICSharpCode.TextEditor.dll"))));
                //dgvModules.Items.Add(MakeItem(Assembly.LoadFile(Path.Combine(apppatch,"Debugger.Core.dll"))));
                //dgvModules.Items.Add(MakeItem(Assembly.LoadFile(Path.Combine(apppatch,"WeifenLuo.WinFormsUI.Docking.dll")))); 
                ActiveControl = button1;
            }
            catch
            {

            }
            /*lbComponents.Items.Clear();
            PascalABCCompiler.Compiler comp = (Owner as Form1).VisualEnvironmentCompiler.Compiler;
            if (comp != null)
            {
                lbComponents.Items.Add(BuildComponentString(comp));
                foreach (PascalABCCompiler.ParserTools.BaseParser parser in comp.ParsersController.Parsers)
                    lbComponents.Items.Add("Language: "+BuildComponentString(parser));
                foreach (VisualPascalABCPlugins.IVisualPascalABCPlugin plugin in (Owner as Form1).VisualEnvironmentCompiler.PluginsController.Plugins)
                    lbComponents.Items.Add("Plugin: "+BuildComponentString(plugin));
                foreach (PascalABCCompiler.SemanticTreeConverters.ISemanticTreeConverter conv in (Owner as Form1).VisualEnvironmentCompiler.Compiler.SemanticTreeConvertersController.SemanticTreeConverters)
                    lbComponents.Items.Add("Converter: " + BuildComponentString(conv));

            }*/

        }

        ListViewItem MakeItem(Assembly a)
        {
            return MakeItem("Assembly", Path.GetFileName(a.ManifestModule.FullyQualifiedName), a.GetName().Version.ToString(), AssemblyCopyright(a));
        }
        
        ListViewItem MakeItem(params string[] names)
        {
            ListViewItem li = new ListViewItem();
            li.Text = names[0];
            for (int i = 1; i < names.Length; i++)
                li.SubItems.Add(names[i]);
            return li;
        }

        private string BuildComponentString(object Component)
        {
            return string.Format("{0}",Component);
        }

        private string GetAssamlyFileName(object obj)
        {
            return Path.GetFileName(Assembly.GetAssembly(obj.GetType()).ManifestModule.FullyQualifiedName);
        }
        private void AboutBox_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void lbComponents_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(PascalABCCompiler.StringResources.Get("!PASCALABCNET_SITE_LINK"));
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(PascalABCCompiler.StringResources.Get("!PASCALABCNET_FORUM_LINK"));
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(PascalABCCompiler.StringResources.Get("!PASCALABCNET_DEVELOPERS_LINK"));
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(PascalABCCompiler.StringResources.Get("!PASCALABCNET_TELEGRAM_CHANNEL_LINK"));
        }
    }
}
