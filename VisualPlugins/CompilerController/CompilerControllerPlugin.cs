using System;
using System.Collections.Generic;
using System.Text;

namespace VisualPascalABCPlugins
{
    public class CompilerController_VisualPascalABCPlugin : IVisualPascalABCPlugin
    {
        public static string StringsPrefix = "VPP_COMPCONTR_";
        IVisualEnvironmentCompiler VisualEnvironmentCompiler;
        private CompilerInformation CompilerInformation = new CompilerInformation();
        private string ILDASMFileName=null;
        private string DbgCLRFileName = null; 
        private string LastGeneratedFile = null;
        private System.Windows.Forms.ToolStripButton RunILDasmButton=null,RunDbgCLRButton=null;
        
        public string Name
        {
            get
            {
                return "Compiler Controller";
            }
        }
        public string Version
        {
            get
            {
                return "1.3";
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
            CompilerInformation.Show();
        }
        public CompilerController_VisualPascalABCPlugin(IWorkbench Workbench)
        {
            IVisualEnvironmentCompiler VisualEnvironmentCompiler = Workbench.VisualEnvironmentCompiler;
            this.VisualEnvironmentCompiler=VisualEnvironmentCompiler;
            CompilerInformation.VisualEnvironmentCompiler = VisualEnvironmentCompiler;
            CompilerInformation.LoadOptions(CompilerInformation.OptionsFileName);
            VisualEnvironmentCompiler.StandartCompiler.InternalDebug.UseStandarParserForIntellisense = CompilerInformation.cbUseStandarParserForInellisense.Checked;
            VisualEnvironmentCompiler.StandartCompiler.InternalDebug.RunOnMono = CompilerInformation.cbRunMono.Checked;
        	VisualEnvironmentCompiler.RemoteCompiler.InternalDebug.RunOnMono = CompilerInformation.cbRunMono.Checked;
            VisualEnvironmentCompiler.StandartCompiler.OnChangeCompilerState += Compiler_OnChangeCompilerState;
            if (VisualEnvironmentCompiler.RemoteCompiler != null)
                VisualEnvironmentCompiler.RemoteCompiler.OnChangeCompilerState += Compiler_OnChangeCompilerState;
        }

        public void GetGUI(List<IPluginGUIItem> MenuItems, List<IPluginGUIItem> ToolBarItems)
        {
            PluginGUIItem Item = new PluginGUIItem(StringsPrefix + "NAME", StringsPrefix + "DESCRIPTION", CompilerInformation.PluginImage.Image, CompilerInformation.PluginImage.BackColor, Execute);
            MenuItems.Add(Item);
            ToolBarItems.Add(Item);
            if ((ILDASMFileName = GetILDASMFileName()) != null)
            {
                Item = new PluginGUIItem(StringsPrefix + "RUN_ILDASM", StringsPrefix + "RUN_ILDASM", CompilerInformation.RunILDASMImage.Image, CompilerInformation.RunILDASMImage.BackColor, RunILDasm);
                MenuItems.Add(Item);
                ToolBarItems.Add(Item);
            }
            if ((DbgCLRFileName = GetDbgCLRFileName()) != null)
            {
                Item = new PluginGUIItem(StringsPrefix + "RUN_DBGCLR", StringsPrefix + "RUN_DBGCLR", CompilerInformation.RunDbgCLRImage.Image, CompilerInformation.RunDbgCLRImage.BackColor, RunDbgCLR);
                MenuItems.Add(Item);
                ToolBarItems.Add(Item);
            }
        }

        void RunDbgCLR()
        {
            System.Diagnostics.Process Process = new System.Diagnostics.Process();
            Process.StartInfo.FileName = DbgCLRFileName;
            if (LastGeneratedFile != null && System.IO.File.Exists(LastGeneratedFile))
                Process.StartInfo.Arguments = LastGeneratedFile;
            Process.Start();            
        }

        void RunILDasm()
        {
            System.Diagnostics.Process ILDASMProcess = new System.Diagnostics.Process();
            ILDASMProcess.StartInfo.FileName = ILDASMFileName;
            ILDASMProcess.StartInfo.UseShellExecute = false;
            ILDASMProcess.StartInfo.CreateNoWindow = true;
            if (LastGeneratedFile != null && System.IO.File.Exists(LastGeneratedFile))
                ILDASMProcess.StartInfo.Arguments = LastGeneratedFile;
            ILDASMProcess.Start();
        }

        private static string GetVS8Dir()
        {
            try
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;
                key = key.OpenSubKey("SOFTWARE");
                key = key.OpenSubKey("Microsoft");
                key = key.OpenSubKey("VisualStudio");
                key = key.OpenSubKey("8.0");
                string s = (string)key.GetValue("InstallDir");
                key.Close();
                s = System.IO.Path.GetDirectoryName(s);
                s = System.IO.Path.GetDirectoryName(s);
                s = System.IO.Path.GetDirectoryName(s);
                return s;
            }
            catch (Exception)
            {
            }
            return null;
        }
        private static string GetILDASMFileName()
        {
            try
            {
                string s;
                if((s= GetVS8Dir())==null) return null;
                s = s + "\\SDK\\v2.0\\bin\\ildasm.exe";
                if (System.IO.File.Exists(s))
                    return s;
            }
            catch (Exception)
            {
            }
            return null;
        }
        private static string GetDbgCLRFileName()
        {
            try
            {
                string s;
                if ((s = GetVS8Dir()) == null) return null;
                s = s + "\\SDK\\v2.0\\GuiDebug\\DbgCLR.exe";
                if (System.IO.File.Exists(s))
                    return s;
            }
            catch (Exception)
            {
            }
            return null;
        }        
        void Compiler_OnChangeCompilerState(PascalABCCompiler.ICompiler sender, PascalABCCompiler.CompilerState State, string FileName)
        {
            if (State == PascalABCCompiler.CompilerState.CodeGeneration)
                LastGeneratedFile = FileName;
            
        }


    }
}
