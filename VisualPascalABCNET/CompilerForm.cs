using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VisualPascalABC
{
    public partial class CompilerForm : Form
    {
        public CompilerForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (Owner as Form1).Reset();
        }
    }
}