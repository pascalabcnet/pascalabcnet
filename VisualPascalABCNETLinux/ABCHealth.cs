using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualPascalABC
{
    public partial class ABCHealth : Form
    {
        public ABCHealth()
        {
            InitializeComponent();
            this.Font = new Font(this.Font.FontFamily, 10);
            this.Activated += OnActivated;
            //this.Paint += OnPaint;
        }

        private int i = 0;
        private void OnActivated(object sender, EventArgs e)
        {
        }
    }
}
