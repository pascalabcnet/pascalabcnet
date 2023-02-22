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
    public partial class ReplaceConfirmDialog : Form
    {
        public enum ModalResult {Yes,No,All,Cancel}
        public ModalResult Result;
        public ReplaceConfirmDialog()
        {
            InitializeComponent();
        }

        private void btYes_Click(object sender, EventArgs e)
        {
            Result = ModalResult.Yes;
            Close();
        }

        private void btNo_Click(object sender, EventArgs e)
        {
            Result = ModalResult.No;
            Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Result = ModalResult.Cancel;
            Close();
        }

        private void btAll_Click(object sender, EventArgs e)
        {
            Result = ModalResult.All;
            Close();
        }
    }
}