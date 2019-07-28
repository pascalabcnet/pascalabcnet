using System;
using System.Collections;
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
        public VisualPascalABC.Form1 MainForm;

        public CodeTemplatesForm()
        {
            InitializeComponent();
            try
            {
                schoolManager = new CodeTemplateManager("school.pct");
                listBox1.Items.Clear();
                listBox1.Items.AddRange(schoolManager.ht.Keys.ToArray());
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

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var s = listBox1.SelectedItem.ToString();
            ICSharpCode.TextEditor.TextArea ta = MainForm.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.TextArea;
            ta.Focus();
            if (schoolManager.ht.ContainsKey(s))
            {
                CodeCompletionActionsManager.GenerateTemplate(s, ta, schoolManager, false);
            }
            else
            {
                ta.InsertString(s);
            }
            ta.Focus();
        }
    }
}
