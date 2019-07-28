using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualPascalABC;

namespace CodeTemplatesPlugin
{
    public partial class CodeTemplatesForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public CodeTemplateManager schoolManager;

        public CodeTemplatesForm()
        {
            InitializeComponent();
            try
            {
                schoolManager = new CodeTemplateManager("school.pct");
            }
            catch
            {

            }

        }

        private void CodeTemplatesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }
    }
}
