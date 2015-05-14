using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VisualPascalABCPlugins
{
    public partial class ErrorReport : Form
    {
        public ErrorReport()
        {
            InitializeComponent();
            PascalABCCompiler.StringResources.SetTextForAllObjects(this, InternalErrorReport_VisualPascalABCPlugin.StringsPrefix + "ERRORREPORT_");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(PascalABCCompiler.StringResources.Get("!PASCALABCNET_FORUM_LINK"));
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:"+((Label)sender).Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}