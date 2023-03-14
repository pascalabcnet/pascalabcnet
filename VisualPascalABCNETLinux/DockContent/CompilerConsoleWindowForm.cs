// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace VisualPascalABC
{
    public partial class CompilerConsoleWindowForm : BottomDockContentForm, VisualPascalABCPlugins.ICompilerConsoleWindow
    {
        public CompilerConsoleWindowForm(Form1 MainForm)
            :base(MainForm)
        {
            InitializeComponent();
        }
        public void CompilerConsoleScrolToEnd()
        {
            CompilerConsole.SelectionStart = CompilerConsole.Text.Length;
            CompilerConsole.ScrollToCaret();
        }
        public void AppendTextToConsoleCompiler(object text)
        {
            CompilerConsole.AppendText(text as string);
        }

        public void AddTextToCompilerMessages(string text)
        {
            //CompilerConsole.Invoke(new ParameterizedThreadStart(this.AppendTextToConsoleCompiler),new object[1]{text});
            AppendTextToConsoleCompiler(text);
            CompilerConsoleScrolToEnd();
            //this.CompilerConsole.g
        }
        delegate void _noparamsdegeate();
        public void ClearConsole()
        {
            if (Visible)
                this.BeginInvoke(new _noparamsdegeate(CompilerConsole.Clear));
            else
                CompilerConsole.Clear();
        }
    }
}