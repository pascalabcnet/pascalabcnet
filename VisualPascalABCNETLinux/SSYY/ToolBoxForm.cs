using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using SampleDesignerApplication;

namespace VisualPascalABC
{
    public partial class ToolBoxForm : DockContent
    {
        public SampleDesignerApplication.ToolBoxPane ToolBox
        {
            get
            {
                return toolboxPane1;
            }
        }

        public ToolBoxForm()
        {
            //Hide();
            InitializeComponent();
            //ToolBox = new SampleDesignerApplication.ToolboxPane();
            //ToolBox.Hide();
            //this.Controls.Add(ToolBox);
            //ToolBox.Dock = DockStyle.Fill;
            //ToolBox.Show();
            TabText = PascalABCCompiler.StringResources.Get("VP_MF_M_TOOLBOX");
        }

    }
}