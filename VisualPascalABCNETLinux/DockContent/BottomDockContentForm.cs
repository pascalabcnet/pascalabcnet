// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;


namespace VisualPascalABC
{
    public partial class BottomDockContentForm : DockContent
    {
        protected Form1 MainForm;
        public BottomDockContentForm()
        {
        }
        public BottomDockContentForm(Form1 MainForm)
        {
            this.MainForm = MainForm;
            InitializeComponent();
            HideOnClose = true;
            DockAreas |= DockAreas.DockBottom;
            MainForm.BottomDockContent.Add(this);
        }

        private void BottomDockContentForm_VisibleChanged(object sender, EventArgs e)
        {
            MainForm.UpdateOutputWindowVisibleButtons();
        }

        private void BottomDockContentForm_Activated(object sender, EventArgs e)
        {
            MainForm.BottomActiveContent = this;
        }
    }
}