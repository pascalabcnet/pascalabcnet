// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;

namespace VisualPascalABC
{
    public partial class ErrorsListWindowForm : BottomDockContentForm, VisualPascalABCPlugins.IErrorListWindow
    {
        private int ErrorListPict = 0;
        private int ErrorListNumCol = 1;
        private int ErrorListLineCol = 2;
        private int ErrorListDescrCol = 3;
        private int ErrorListFileCol = 4;
        private int ErrorListPathCol = 5;

        public ErrorsListWindowForm(Form1 MainForm)
            : base(MainForm)
        {
            InitializeComponent();
            Form1StringResources.SetTextForAllControls(this.contextMenuStrip1);
        }

        private void ErrorsListWindowForm_VisibleChanged(object sender, EventArgs e)
        {
        }

        private void ErrorsListWindowForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Pane = null;
            MainForm.UpdateOutputWindowVisibleButtons();

        }
        internal void Resized()
        {
            lvErrorsList.Columns[ErrorListDescrCol].Width = lvErrorsList.ClientSize.Width -
    (lvErrorsList.Columns[ErrorListPict].Width +
    lvErrorsList.Columns[ErrorListNumCol].Width +
    lvErrorsList.Columns[ErrorListLineCol].Width +
    lvErrorsList.Columns[ErrorListFileCol].Width +
    lvErrorsList.Columns[ErrorListPathCol].Width + 1);
        }
        private void lvErrorsList_Resize(object sender, EventArgs e)
        {
            Resized();
        }

        delegate void ShowErrorsDelegate(List<PascalABCCompiler.Errors.Error> errors, bool ChangeViewTab);
        public void ShowErrorsSync(List<PascalABCCompiler.Errors.Error> errors, bool ChangeViewTab)
        {
            BeginInvoke(new ShowErrorsDelegate(ShowErrors), errors, ChangeViewTab);
        }

        public void ShowErrors(List<PascalABCCompiler.Errors.Error> errors, bool ChangeViewTab)
        {
            if (ChangeViewTab)
            {
                MainForm.BottomTabsVisible = true;

                MainForm.SelectContent(this, true);
            }
            PascalABCCompiler.Errors.LocatedError er;
            int i = lvErrorsList.Items.Count;
            //ivan
            //dataGridView1.Rows.Clear();
            //dataGridView1.Rows.Add(8);
            //\ivan
            List<PascalABCCompiler.Errors.Error> ErrorsList = errors;
            foreach (PascalABCCompiler.Errors.Error Err in ErrorsList)
            {
                lvErrorsList.Items.Add("");
                if (Err is PascalABCCompiler.Errors.CompilerWarning)
                    lvErrorsList.Items[i].ImageIndex = 0;
                else
                    if (Err is RuntimeException)
                        lvErrorsList.Items[i].ImageIndex = 2;
                    else
                        lvErrorsList.Items[i].ImageIndex = 1;
                lvErrorsList.Items[i].SubItems.Add("");//num
                lvErrorsList.Items[i].SubItems.Add("");//linenum
                lvErrorsList.Items[i].SubItems.Add("");//descr
                lvErrorsList.Items[i].SubItems.Add("");//file
                lvErrorsList.Items[i].SubItems.Add("");//patch
                lvErrorsList.Items[i].SubItems[ErrorListNumCol].Text = (i + 1).ToString();
                if (Err is PascalABCCompiler.Errors.LocatedError)
                {
                    er = Err as PascalABCCompiler.Errors.LocatedError;
                    lvErrorsList.Items[i].SubItems[ErrorListDescrCol].Text = er.Message;
                    if ((Err as PascalABCCompiler.Errors.LocatedError).SourceLocation != null)
                    {
                        lvErrorsList.Items[i].Tag = (Err as PascalABCCompiler.Errors.LocatedError).SourceLocation;
                        lvErrorsList.Items[i].SubItems[ErrorListFileCol].Text = Path.GetFileName(er.SourceLocation.FileName);
                        lvErrorsList.Items[i].SubItems[ErrorListPathCol].Text = Path.GetDirectoryName(er.SourceLocation.FileName);
                        lvErrorsList.Items[i].SubItems[ErrorListLineCol].Text = er.SourceLocation.BeginPosition.Line.ToString();
                    }
                }
                if (Err is PascalABCCompiler.Errors.CompilerInternalError)
                {
                    lvErrorsList.Items[i].SubItems[ErrorListDescrCol].Text = (Err as PascalABCCompiler.Errors.CompilerInternalError).ToString();
                }
                i++;
            }
            if (ErrorsList.Count > 0 && !(ErrorsList[0] is PascalABCCompiler.Errors.CompilerWarning))
            {
                MainForm.BottomTabsVisible = true;

                if ((er = ErrorsList[0] as PascalABCCompiler.Errors.LocatedError) != null)
                    if (er.SourceLocation != null)
                    {
                        if (ErrorsList[0] is VisualPascalABC.RuntimeException)
                            MainForm.ExecuteErrorPos(er.SourceLocation, 2);
                        else // ImageIndex==2 - ошибка выполнения
                            MainForm.ExecuteErrorPos(er.SourceLocation, 1);
                        //OpenFile(er.SourceLocation.FileName);
                        //(tabControl1.SelectedTab.ag as CodeFileDocumentControl).TextEditor.CaretPosition(er.SourceLocation.BeginPosition.Line,er.SourceLocation.BeginPosition.Column);
                    }
            }
            else
                MainForm.SetFocusToEditor();
        }

        private void lvErrorsList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Point clickPoint = lvErrorsList.PointToClient(Cursor.Position);
            ListViewItem item = lvErrorsList.GetItemAt(clickPoint.X, clickPoint.Y);
            if (item != null)
            {
                if (item.Tag != null)
                    MainForm.ExecuteErrorPos((PascalABCCompiler.SourceLocation)item.Tag, item.ImageIndex);
            }
        }

        private void lvErrorsList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && lvErrorsList.SelectedItems.Count > 0)
            {
                ListViewItem item = lvErrorsList.SelectedItems[0];
                if (item != null)
                {
                    if (item.Tag != null)
                        MainForm.ExecuteErrorPos((PascalABCCompiler.SourceLocation)item.Tag, item.ImageIndex);
                }
            }
        }

        private void tPCOPYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvErrorsList.SelectedItems.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(lvErrorsList.SelectedItems[0].SubItems[ErrorListFileCol].Text + "(" + lvErrorsList.SelectedItems[0].SubItems[ErrorListLineCol].Text + ") : ");
                sb.Append(lvErrorsList.SelectedItems[0].SubItems[ErrorListDescrCol].Text);
                Clipboard.SetText(sb.ToString());
            }
        }

        public void ClearErrorList()
        {
            lvErrorsList.Items.Clear();
            WorkbenchServiceFactory.BuildService.ErrorsList.Clear();
        }

    }
}
