using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace VisualPascalABC
{
    public partial class PropertiesForm : DockContent
    {
        public PropertiesForm()
        {
            InitializeComponent();
            TabText = PascalABCCompiler.StringResources.Get("VP_MF_M_PROPERTIES");
        }

        private void PropertiesForm_Load(object sender, EventArgs e)
        {

        }

        private void PropertiesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
    }
}
