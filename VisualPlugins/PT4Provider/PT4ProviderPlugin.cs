using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace VisualPascalABCPlugins
{
    public class PT4Provider_VisualPascalABCPlugin : IVisualPascalABCPlugin
    {
        private string PT4Directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName) + "\\PT4";
        private string PABCWorkDirectory;

        public static string StringsPrefix = "VPP_PT4PROVIDER_";
        private PT4Provider.Images Images = new PT4Provider.Images();
        IVisualEnvironmentCompiler VisualEnvironmentCompiler;
        PT4.Tools tools;

        public string Name
        {
            get
            {
                return "PT4 Provider";
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

        /*[DllImport("PT4\\PT4PABC.dll", CharSet = CharSet.Ansi, EntryPoint = "pt4demo")]
        static extern int pt4demo(string startdir,
          string ptdir, string dllname,
          int dlloptions, int usedlltopics, int usedemodat,
          string demodatname, string demodatdir,
          StringBuilder topic, ref int number);

        [DllImport("PT4\\PT4PABC.dll", CharSet = CharSet.Ansi, EntryPoint = "pt4load")]
        static extern int pt4load(string startdir,
          string ptdir, string envlist, string dirlist,
          int startenv, StringBuilder edittext, StringBuilder filename);

        [DllImport("PT4\\PT4PABC.dll", CharSet = CharSet.Ansi, EntryPoint = "pt4results")]
        static extern int pt4results(string startdir,
          string ptdir, int showbrowsedir, int browsesubdirs,
          ref int fontsize, ref int showinffile, ref int showtxtfile);

        [DllImport("PT4\\PT4PABC.dll", CharSet = CharSet.Ansi, EntryPoint = "pt4getdatinfo")]
        static extern int pt4getdatinfo(string startdir,
          StringBuilder studentname, ref int fontsize,
          StringBuilder envir, StringBuilder ptdir);

        [DllImport("PT4\\PT4PABC.dll", CharSet = CharSet.Ansi, EntryPoint = "pt4setdatinfo")]
        public static extern int pt4setdatinfo(string startdir,
          int fontsize, string envir);*/


        public int Demo(StringBuilder topic, ref int number)
        {
//MABR 26.12.2015>
            string CurrentSourceFileName = VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.GetCurrentSourceFileName, "") as string;
            tools.Demo(Path.GetDirectoryName(CurrentSourceFileName));
//            tools.Demo(PABCWorkDirectory);
//MABR 26.12.2015<
            return 0;
//            return pt4demo(PABCWorkDirectory, PT4Directory, "pt4", 0, 1, 2, "", "", topic, ref number);
        }

        /*public int Load(StringBuilder edittext, StringBuilder filename)
        {
            filename = tools.Load(PABCWorkDirectory);
            return 0;
            //return pt4load(PABCWorkDirectory, PT4Directory, "PABC2", "", 1, edittext, filename);
        }*/

        public void Results()
        {
//MABR 26.12.2015>
            string CurrentSourceFileName = VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.GetCurrentSourceFileName, "") as string;
            tools.Res(Path.GetDirectoryName(CurrentSourceFileName));
//            tools.Res(PABCWorkDirectory);
//MABR 26.12.2015<
            /*StringBuilder
              s1 = new StringBuilder(50),
              s2 = new StringBuilder(20),
              s3 = new StringBuilder(100);
            int fsize = 0, fontsize = 0,
              showinf = 1, showtxt = 1,
              i = pt4getdatinfo("", s1, ref fsize, s2, s3);
            if (i == 0)
                fontsize = fsize;
            pt4results(PABCWorkDirectory, PT4Directory, 0, 0, ref fontsize,
              ref showinf, ref showtxt);
            if (i == 0 && fsize != fontsize)
                pt4setdatinfo("", fontsize, "");*/
        }
        
        StringBuilder topic = new StringBuilder(20),edittext = new StringBuilder(100);
        int number = 0;

        public void ExecuteB_D()
        {
            Demo(topic, ref number);
        }
        public void ExecuteB_L()
        {
            // SSM 30.12.2010

            //StringBuilder filename = new StringBuilder(250);
            string filename = tools.Load(PABCWorkDirectory);
            if (filename == "")
                return;
            /*int i = Load(edittext, filename);
            if (i < 1 || filename.ToString() == "") 
                return;*/
            VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.OpenFile, filename.ToString());
            VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.PT4PositionCursorAfterTask, null);
        }
        public void ExecuteB_R()
        {
            Results();
        }

        public void ExecuteB_S()
        {

            string SystemDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);
            var PT4Dir = SystemDir + "\\PT4";

            var pt4setup = new System.Diagnostics.Process();
            pt4setup.StartInfo.FileName = PT4Dir + "\\PT4Setup.exe";

            if (System.IO.File.Exists(pt4setup.StartInfo.FileName))
                try
                {
                    pt4setup.Start();
                }
                catch(Exception ex)
                {
                    VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.AddMessageToErrorListWindow, 
                    new List<PascalABCCompiler.Errors.Error>(new PascalABCCompiler.Errors.Error[] { new PascalABCCompiler.Errors.Error(ex.Message) }));
                }
        }
        public void GetGUI(List<IPluginGUIItem> MenuItems, List<IPluginGUIItem> ToolBarItems)
        {
            // SSM 30.12.2010
            string SystemDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);
            tools = new PT4.Tools(SystemDir);
            if (!tools.AreAvailable)
                return;

            PluginGUIItem Item;
            Item = new PluginGUIItem(StringsPrefix + "D_NAME", StringsPrefix + "D_DESCRIPTION", Images.pb_pt4d.Image, Images.pb_pt4d.BackColor, ExecuteB_D, Keys.Control | Keys.Shift | Keys.D, "Ctrl+Shift+D");
            MenuItems.Add(Item);ToolBarItems.Add(Item);
            Item = new PluginGUIItem(StringsPrefix + "L_NAME", StringsPrefix + "L_DESCRIPTION", Images.pb_pt4l.Image, Images.pb_pt4l.BackColor, ExecuteB_L, Keys.Control | Keys.Shift | Keys.L, "Ctrl+Shift+L");
            MenuItems.Add(Item); ToolBarItems.Add(Item);
            Item = new PluginGUIItem(StringsPrefix + "R_NAME", StringsPrefix + "R_DESCRIPTION", Images.pb_pt4r.Image, Images.pb_pt4r.BackColor, ExecuteB_R, Keys.Control | Keys.Shift | Keys.R, "Ctrl+Shift+R");
            MenuItems.Add(Item); ToolBarItems.Add(Item);

            var PT4Dir = SystemDir + "\\PT4";
            Item = new PluginGUIItem(StringsPrefix + "S_NAME", StringsPrefix + "PT4_Setup", null, Images.pb_pt4r.BackColor, ExecuteB_S, Keys.None, "");
            MenuItems.Add(Item);

        }

        /*private void CheckAndCopyPT4Files(string FileName)
        {
            
            string fn=PABCWorkDirectory+"\\"+FileName;
            string instfn=PT4Directory+"\\"+FileName;
            try
            {
                if (!File.Exists(fn))
                    if (File.Exists(instfn))
                        File.Copy(instfn, fn);
            }
            catch (Exception)
            {
            }

        }*/

        public PT4Provider_VisualPascalABCPlugin(IWorkbench Workbench)
        {
            VisualEnvironmentCompiler = Workbench.VisualEnvironmentCompiler;
            VisualEnvironmentCompiler.ChangeVisualEnvironmentState += new ChangeVisualEnvironmentStateDelegate(VisualEnvironmentCompiler_ChangeVisualEnvironmentState);
            VisualEnvironmentCompiler.Compiler.OnChangeCompilerState += Compiler_OnChangeCompilerState;
            PABCWorkDirectory = VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.GetDirectory, "%WORKINGDIRECTORY%") as string;
            /*if (PABCWorkDirectory == null)
                PABCWorkDirectory = "";
            else
                if (Directory.Exists(PABCWorkDirectory))
                {
                    CheckAndCopyPT4Files(PT4Provider.Constants.ResultsABCFileName);
                    //CheckAndCopyPT4Files(PT4Provider.Constants.ResultsDATFileName);
                }
             */
            /*string PABCOutputDirectory = VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.GetDirectory, "%OUTPUTDIRECTORY%") as string;
            if (PABCOutputDirectory != null)
                if (Directory.Exists(PABCOutputDirectory))
                {
                    PABCWorkDirectory = PABCOutputDirectory;
                    CheckAndCopyPT4Files(PT4Provider.Constants.ResultsABCFileName);
                    //CheckAndCopyPT4Files(PT4Provider.Constants.ResultsDATFileName);
                }
            */
        }

        void CopyResultsABC(string FromDir, string ToDir)
        {
            try
            {
                if (File.Exists(FromDir + "\\" + PT4Provider.Constants.ResultsABCFileName))
                    File.Copy(FromDir + "\\" + PT4Provider.Constants.ResultsABCFileName, ToDir + "\\" + PT4Provider.Constants.ResultsABCFileName);
                //if (File.Exists(FromDir + "\\" + PT4Provider.Constants.ResultsDATFileName))
                //    File.Copy(FromDir + "\\" + PT4Provider.Constants.ResultsDATFileName, ToDir + "\\" + PT4Provider.Constants.ResultsABCFileName);
            }
            catch (Exception)
            {
            }
        }

        void VisualEnvironmentCompiler_ChangeVisualEnvironmentState(VisualEnvironmentState State, object obj)
        {
            switch (State)
            {
                case VisualEnvironmentState.ProcessStarting:
                case VisualEnvironmentState.ProcessStartingDebug:
                case VisualEnvironmentState.ProcessExited:
                    string FileName = (string)obj;
                    if(CompiledProgrammsWithPT4Unit.ContainsKey(FileName))
                    {
                        string PasDir = Path.GetDirectoryName(CompiledProgrammsWithPT4Unit[FileName]);
                        string ExeDir = Path.GetDirectoryName(FileName);
                        if (PasDir.ToLower() != ExeDir.ToLower())
                        {
                            if (State == VisualEnvironmentState.ProcessExited)
                            {
                                CopyResultsABC(ExeDir, PasDir);
                                CompiledProgrammsWithPT4Unit.Remove(FileName);
                            }
                            else
                            {
                                if (File.Exists(PasDir + "\\" + PT4Provider.Constants.ResultsABCFileName))
                                    CopyResultsABC(PasDir, ExeDir);
                                else
                                    CopyResultsABC(PT4Directory, ExeDir);
                            }
                        }
                        else
                        {
                            if (State != VisualEnvironmentState.ProcessExited)
                                if (!File.Exists(PasDir + "\\" + PT4Provider.Constants.ResultsABCFileName))
                                    CopyResultsABC(PT4Directory, ExeDir);
                        }
                    }
                    break;
            }
        }

        //exe->pas
        private Dictionary<string, string> CompiledProgrammsWithPT4Unit = new Dictionary<string, string>();
        private string FirstCompiledFileName = null;
        private bool PT4UnitUsed = false;

        void Compiler_OnChangeCompilerState(PascalABCCompiler.ICompiler sender, PascalABCCompiler.CompilerState State, string FileName)
        {
            switch (State)
            {
                case PascalABCCompiler.CompilerState.CompilationStarting:
                    FirstCompiledFileName = FileName;
                    break;
                
                case PascalABCCompiler.CompilerState.ReadPCUFile:
                case PascalABCCompiler.CompilerState.BeginCompileFile:
                    if (Path.GetFileNameWithoutExtension(FileName.ToLower()) == PT4Provider.Constants.PT4UnitName)
                        PT4UnitUsed = true;
                    break;
                
                case PascalABCCompiler.CompilerState.CodeGeneration:
                    if (CompiledProgrammsWithPT4Unit.ContainsKey(FileName))
                        CompiledProgrammsWithPT4Unit.Remove(FileName);
                    if (FirstCompiledFileName != null && PT4UnitUsed)
                        CompiledProgrammsWithPT4Unit.Add(FileName, FirstCompiledFileName);
                    break;

                case PascalABCCompiler.CompilerState.CompilationFinished:
                    FirstCompiledFileName = null;
                    PT4UnitUsed = false;
                    break;

            }
        }

    }

}
