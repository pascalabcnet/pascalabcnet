// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VisualPascalABC
{
    public partial class GotoLineForm : Form
    {
        Form1 MainForm;
        public GotoLineForm()
        {
            InitializeComponent();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void GotoLineForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = tbLineNumber;
            MainForm = (Form1)this.Owner;
            label1.Text = string.Format(PascalABCCompiler.StringResources.Get("VP_GOTOLINEFORM_GOTO_LINE_{0}_{1}"),
                1, MainForm.CurrentSyntaxEditor.LinesCount);
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            int line;
            if (int.TryParse(tbLineNumber.Text, out line))
                if (line <= MainForm.CurrentSyntaxEditor.LinesCount)
                {
                    Close();
                    MainForm.ExecuteSourceLocationAction(new PascalABCCompiler.SourceLocation(MainForm.CurrentSourceFileName, line, 1, line, 1), VisualPascalABCPlugins.SourceLocationAction.GotoBeg);
                    return;
                }
            this.ActiveControl = tbLineNumber;
        }
    }
}
