using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VisualPascalABC
{
    public partial class RunArguments : Form
    {
        public RunArguments()
        {
            InitializeComponent();
        }

        public string CommandLineArguments
        {
            get
            {
                return this.tbRunArguments.Text;
            }
            set
            {
                this.tbRunArguments.Text = value;
            }
        }
    }
}
