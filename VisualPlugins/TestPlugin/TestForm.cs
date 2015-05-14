using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PascalABCCompiler.Errors;
using System.IO;

namespace VisualPascalABCPlugins
{
    public partial class TestForm : Form
    {
        public IVisualEnvironmentCompiler VisualEnvironmentCompiler;
        
        public TestForm()
        {
            InitializeComponent();
            PascalABCCompiler.StringResources.SetTextForAllObjects(this, TestPlugin_VisualPascalABCPlugin.StringsPrefix);
        }

        private void SyntaxTreeVisualisatorForm_Load(object sender, EventArgs e)
        {

        }

        private void SyntaxTreeVisualisatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void SyntaxTreeVisualisatorForm_Shown(object sender, EventArgs e)
        {
            VisualEnvironmentCompiler.Compiler.OnChangeCompilerState += new PascalABCCompiler.ChangeCompilerStateEventDelegate(Compiler_OnChangeCompilerState);
        }

        void Compiler_OnChangeCompilerState(PascalABCCompiler.ICompiler sender, PascalABCCompiler.CompilerState State, string FileName)
        {
            //compiler states
        }
    }
}
