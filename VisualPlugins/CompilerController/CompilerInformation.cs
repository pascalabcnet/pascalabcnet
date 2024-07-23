using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace VisualPascalABCPlugins
{
    public partial class CompilerInformation : Form
    {
        public IVisualEnvironmentCompiler VisualEnvironmentCompiler;
        public string ILDASMFileName = null;
        private DateTime dt;
        public string LastGeneratedFile = null;

        public CompilerInformation()
        {
            InitializeComponent();
            PascalABCCompiler.StringResources.SetTextForAllObjects(this, CompilerController_VisualPascalABCPlugin.StringsPrefix);
        }

        private delegate void OnChangeCompilerStateDelegate(PascalABCCompiler.ICompiler sender, PascalABCCompiler.CompilerState State, string FileName);
        void Compiler_OnChangeCompilerStateSync(PascalABCCompiler.ICompiler sender, PascalABCCompiler.CompilerState State, string FileName)
        {
            if(this.Visible)
                BeginInvoke(new OnChangeCompilerStateDelegate(Compiler_OnChangeCompilerState), sender, State, FileName);
        }
        void Compiler_OnChangeCompilerState(PascalABCCompiler.ICompiler sender, PascalABCCompiler.CompilerState State, string FileName)
        {
            string text;
            if (State == PascalABCCompiler.CompilerState.CompilationStarting || State == PascalABCCompiler.CompilerState.Reloading)
            {
                //CompilerConsole.Clear();
                dt = DateTime.Now;
                if (OnRebuld.Checked)
                    VisualEnvironmentCompiler.Compiler.CompilerOptions.Rebuild = true;
            }
            text = State.ToString();
            if (FileName != null)
                text += " " + System.IO.Path.GetFileName(FileName) + "...";
            if (State == PascalABCCompiler.CompilerState.Ready)
            {
                if (VisualEnvironmentCompiler.Compiler.ErrorsList.Count > 0)
                    text += string.Format(" [{0} errors]", VisualEnvironmentCompiler.Compiler.ErrorsList.Count);
                text += string.Format(" [{0}ms]", (int)(DateTime.Now - dt).TotalMilliseconds);
                if (sender == VisualEnvironmentCompiler.RemoteCompiler)
                    text += string.Format(Environment.NewLine+"WorkingSet {0}", VisualEnvironmentCompiler.RemoteCompiler.RemoteCompilerWorkingSet/1024/1024);
                NoSavePCU.Checked = !VisualEnvironmentCompiler.Compiler.InternalDebug.PCUGenerate;
                NoSemantic.Checked = !VisualEnvironmentCompiler.Compiler.InternalDebug.SemanticAnalysis;
                NoCodeGeneration.Checked = !VisualEnvironmentCompiler.Compiler.InternalDebug.CodeGeneration;
                NoAddStandartUnits.Checked = VisualEnvironmentCompiler.Compiler.CompilerOptions.DisableStandardUnits;
                NoSkipPCUErrors.Checked = !VisualEnvironmentCompiler.Compiler.InternalDebug.SkipPCUErrors;
                NoSkipInternalErrorsIfSyntaxTreeIsCorrupt.Checked = !VisualEnvironmentCompiler.Compiler.InternalDebug.SkipInternalErrorsIfSyntaxTreeIsCorrupt;
                NoIncludeDebugInfoInPCU.Checked = !VisualEnvironmentCompiler.Compiler.InternalDebug.IncludeDebugInfoInPCU;
                cbUseStandarParserForInellisense.Checked = VisualEnvironmentCompiler.Compiler.InternalDebug.UseStandarParserForIntellisense;
                //OnRebuld.Checked = VisualEnvironmentCompiler.Compiler.CompilerOptions.Rebuild;
            }
            text += Environment.NewLine;
            if (sender.CompilerType == PascalABCCompiler.CompilerType.Remote)
                text = "[remote]" + text;
            CompilerConsole.AppendText(text);
            CompilerConsole.SelectionStart = CompilerConsole.Text.Length;
            CompilerConsole.ScrollToCaret();
        }

        private void CompilerInformation_Load(object sender, EventArgs e)
        {
            VisualEnvironmentCompiler.StandartCompiler.OnChangeCompilerState += new PascalABCCompiler.ChangeCompilerStateEventDelegate(Compiler_OnChangeCompilerStateSync);
            CompilerVersion.Text = "Core "+PascalABCCompiler.Compiler.Version +Environment.NewLine;
            if (VisualEnvironmentCompiler.Compiler.InternalDebug.DebugVersion)
                CompilerVersion.Text += "debug version";
            else
                CompilerVersion.Text += "relase version";
            cbUseStandarParserForInellisense.Enabled = NoAddStandartUnits.Enabled=this.NoCodeGeneration.Enabled = this.NoSavePCU.Enabled = this.NoSemantic.Enabled = this.NoSkipPCUErrors.Enabled = this.NoSkipInternalErrorsIfSyntaxTreeIsCorrupt.Enabled = VisualEnvironmentCompiler.Compiler.InternalDebug.DebugVersion;
        	
        }

        private void CompilerInformation_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void CompilerInformation_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
            SaveOptions(OptionsFileName);            
        }

        private void ResetCompilerButton_Click(object sender, EventArgs e)
        {
            ResetCompilerButton.Enabled = false;
            VisualEnvironmentCompiler.StandartCompiler.Reload();
            if (VisualEnvironmentCompiler.RemoteCompiler!=null)
                VisualEnvironmentCompiler.RemoteCompiler.Reload();
            ResetCompilerButton.Enabled = true;
        }

        private void NoSemantic_CheckedChanged(object sender, EventArgs e)
        {
            VisualEnvironmentCompiler.Compiler.InternalDebug.SemanticAnalysis = !NoSemantic.Checked;
            NoCodeGeneration.Enabled = !NoSemantic.Checked;
            NoCodeGeneration.Checked = NoSemantic.Checked;
            NoSavePCU.Enabled = !NoSemantic.Checked;
            NoSavePCU.Checked = NoSemantic.Checked;
            
        }

        private void NoSavePCU_CheckedChanged(object sender, EventArgs e)
        {
            VisualEnvironmentCompiler.Compiler.InternalDebug.PCUGenerate = !NoSavePCU.Checked;
        }

        private void NoCodeGeneration_CheckedChanged(object sender, EventArgs e)
        {
            VisualEnvironmentCompiler.Compiler.InternalDebug.CodeGeneration = !NoCodeGeneration.Checked;
        }

        private void NoAddStandartUnits_CheckedChanged(object sender, EventArgs e)
        {
            VisualEnvironmentCompiler.Compiler.CompilerOptions.DisableStandardUnits = NoAddStandartUnits.Checked;
        }

        private void OnRebuld_CheckedChanged(object sender, EventArgs e)
        {
            VisualEnvironmentCompiler.Compiler.CompilerOptions.Rebuild = OnRebuld.Checked;
        }

        private void PluginImage_Click(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            GC.Collect();
        }

        private void NoSkipPCUReadingErrors_CheckedChanged(object sender, EventArgs e)
        {
            VisualEnvironmentCompiler.Compiler.InternalDebug.SkipPCUErrors = !NoSkipPCUErrors.Checked;
        }

        private void NoSkipInternalErrorsIfSyntaxTreeIsCorrupt_CheckedChanged(object sender, EventArgs e)
        {
            VisualEnvironmentCompiler.Compiler.InternalDebug.SkipInternalErrorsIfSyntaxTreeIsCorrupt = !NoSkipInternalErrorsIfSyntaxTreeIsCorrupt.Checked;
        }

        private void cbIncludeDebugInfoInPCU_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void NoIncludeDebugInfoInPCU_CheckedChanged(object sender, EventArgs e)
        {
            VisualEnvironmentCompiler.Compiler.InternalDebug.IncludeDebugInfoInPCU = !NoIncludeDebugInfoInPCU.Checked;
        }

        string oiNoCodeGeneration = "NoCodeGeneration";
        string oiUseStandarParserForInellisense = "UseStandarParserForInellisense";
        string oiNotUseRemoteCompiler = "NotUseRemoteCompiler";
		string oiRunMono = "RunOnMono";
        string oiUseSemanticForIntellisense = "UseSemanticForIntellisense";

        public string OptionsFileName = Path.ChangeExtension(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName, ".ini");
        public void SaveOptions(string FileName)
        {
            Hashtable Options = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
            Options.Add(oiNoCodeGeneration, NoCodeGeneration.Checked);
            Options.Add(oiUseStandarParserForInellisense, cbUseStandarParserForInellisense.Checked);
            Options.Add(oiNotUseRemoteCompiler, cbNotUseRemoteCompiler.Checked);
            Options.Add(oiRunMono, cbRunMono.Checked);
            Options.Add(oiUseSemanticForIntellisense, cbUseSemanticForIntellisense.Checked);
            try
            {
                PascalABCCompiler.StringResources.WriteStringsToStream(new StreamWriter(FileName, false, System.Text.Encoding.GetEncoding(1251)), Options);
            }
            catch (Exception)
            {
            }
        }
        public void LoadOptions(string FileName)
        {
            cbNotUseRemoteCompiler.Checked = VisualEnvironmentCompiler.DefaultCompilerType == PascalABCCompiler.CompilerType.Standart;
            try
            {
                if (!File.Exists(FileName))
                    return;
                Hashtable Options = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
                PascalABCCompiler.StringResources.ReadStringsFromStream(new StreamReader(FileName, System.Text.Encoding.GetEncoding(1251)), Options);
                string value;
                if ((value = (string)Options[oiNoCodeGeneration]) != null)
                    NoCodeGeneration.Checked = Convert.ToBoolean(value);
                if ((value = (string)Options[oiUseStandarParserForInellisense]) != null)
                    cbUseStandarParserForInellisense.Checked = Convert.ToBoolean(value);
                if ((value = (string)Options[oiNotUseRemoteCompiler]) != null)
                    cbNotUseRemoteCompiler.Checked = Convert.ToBoolean(value);
                if ((value = (string)Options[oiRunMono]) != null)
                    cbRunMono.Checked = Convert.ToBoolean(value);
                if ((value = (string)Options[oiUseSemanticForIntellisense]) != null)
                    cbUseSemanticForIntellisense.Checked = Convert.ToBoolean(value);
            }
            catch (Exception)
            {
            }
        }

        private void cbUseStandarParserForInellisense_CheckedChanged(object sender, EventArgs e)
        {
            VisualEnvironmentCompiler.Compiler.InternalDebug.UseStandarParserForIntellisense = cbUseStandarParserForInellisense.Checked;
        }

        private void CompilerInformation_Shown(object sender, EventArgs e)
        {
            //cbUseStandarParserForInellisense.Checked = VisualEnvironmentCompiler.Compiler.InternalDebug.UseStandarParserForIntellisense;
        }

        private void cbNotUseRemoteCompiler_CheckedChanged(object sender, EventArgs e)
        {
            if (cbNotUseRemoteCompiler.Checked)
                VisualEnvironmentCompiler.DefaultCompilerType = PascalABCCompiler.CompilerType.Standart;
            else
            {
                VisualEnvironmentCompiler.DefaultCompilerType = PascalABCCompiler.CompilerType.Remote;
                VisualEnvironmentCompiler.RemoteCompiler.OnChangeCompilerState += Compiler_OnChangeCompilerStateSync;
                VisualEnvironmentCompiler.RemoteCompiler.InternalDebug = VisualEnvironmentCompiler.StandartCompiler.InternalDebug;
            }
        }
        
        void Button2Click(object sender, EventArgs e)
        {
        	CodeCompletion.CodeCompletionTester.Test();
            
            CodeCompletion.CodeCompletionTester.TestIntellisense(Path.Combine(@"c:\Work\Miks\_PABCNETGitHub\pascalabcnet\TestSuite", "intellisense_tests"));
            CodeCompletion.CodeCompletionTester.TestRename(@"c:\Work\Miks\_PABCNETGitHub\pascalabcnet\TestSuite\refactoring_tests\rename_tests");
            MessageBox.Show("Done");
        }
        
        void CbRunMonoCheckedChanged(object sender, EventArgs e)
        {
        	VisualEnvironmentCompiler.StandartCompiler.InternalDebug.RunOnMono = cbRunMono.Checked;
        	VisualEnvironmentCompiler.RemoteCompiler.InternalDebug.RunOnMono = cbRunMono.Checked;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CodeCompletion.FormatterTester.Test();
            MessageBox.Show("Done");
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            CodeCompletion.CodeCompletionTester.TestRename(@"c:\Work\Miks\_PABCNETGitHub\pascalabcnet\TestSuite\refactoring_tests\rename_tests");
            MessageBox.Show("Done");
        }

        private void cbUseSemanticForIntellisense_CheckedChanged(object sender, EventArgs e)
        {
            //CodeCompletion.DomSyntaxTreeVisitor.use_semantic_for_intellisense = cbUseSemanticForIntellisense.Checked;
        }
    }
}
