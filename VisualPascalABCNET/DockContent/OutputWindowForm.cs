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
    public partial class OutputWindowForm : BottomDockContentForm, VisualPascalABCPlugins.IOutputWindow
    {
        public OutputWindowForm(Form1 MainForm)
            :base(MainForm)
        {
            InitializeComponent();
            Form1StringResources.SetTextForAllControls(this.contextMenuStrip1);
            outputTextBox.DetectUrls = true;
            outputTextBox.LinkClicked += outputTextBox_LinkClicked;
        }

        public void outputTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
            catch (Exception ex)
            {

            }
        }

        private void OutputWindowForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Pane = null;
            MainForm.UpdateOutputWindowVisibleButtons();
        }

        private void OutputWindowForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void OutputWindowForm_VisibleChanged(object sender, EventArgs e)
        {
            
        }

        private void OutputTextBox_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                outputTextBox.Copy();
        }

        private void InputTextBox_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                InputTextBox.Copy();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WorkbenchServiceFactory.RunService.SendInputTextToProcess();
        }

        bool addThisChar = false;

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Enter:
                    WorkbenchServiceFactory.RunService.SendInputTextToProcess();
                    break;
            }
            if (!e.Alt && !e.Control)
                addThisChar = true;
        }

        private void InputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!addThisChar)
                e.KeyChar = '\0';
            addThisChar = false;
        }
        public void OutputTextBoxScrolToEnd()
        {
            outputTextBox.SelectionStart = outputTextBox.Text.Length;
            outputTextBox.ScrollToCaret();
        }
        public void InputTextBoxCursorToEnd()
        {
            InputTextBox.SelectionStart = InputTextBox.Text.Length;
            InputTextBox.Focus();
        }

        public bool InputPanelVisible
        {
            get
            {
                return InputPanel.Visible;
            }
            set
            {
                if (value && !InputPanel.Visible)
                {
                    InputPanel.Visible = true;
                    MainForm.BottomTabsVisible = true;
                    MainForm.SelectContent(this, false);
                    OutputTextBoxScrolToEnd();
                    InputTextBox.Focus();
                    MainForm.BottomTabsVisibleChekerEnabled = false;
                    return;
                }
                if (!value && InputPanel.Visible)
                {
                    InputPanel.Visible = false;
                    InputTextBox.Text = "";
                    MainForm.lastInputText = "";
                    InputTextBox.Focus();
                    MainForm.BottomTabsVisibleChekerEnabled = true;
                    return;
                }
            }
        }

        private void InputTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!InputPanelVisible)
                return;
            if (InputTextBox.Lines.Length > 1)
                InputTextBox.Text = InputTextBox.Lines[0];
            string ltext = MainForm.lastInputText;
            string text = InputTextBox.Text;
            if ((text.Length > 0) && (ltext == text.Substring(0, text.Length - 1)))
            {
                outputTextBox.AppendText(text[text.Length - 1].ToString());
                OutputTextBoxScrolToEnd();
                MainForm.lastInputText = text;
                if (MainForm.OutputBoxStack.Contains(outputTextBox))
                {
                	foreach (RichTextBox tb in MainForm.OutputBoxStack)
                	if (tb != outputTextBox)
                	{
                		tb.AppendText(text[text.Length - 1].ToString());
                		tb.SelectionStart = tb.Text.Length;
            			tb.ScrollToCaret();
                	}
                }
            }
            else
            {
                outputTextBox.Text = outputTextBox.Text.Substring(0, outputTextBox.Text.Length - ltext.Length) + InputTextBox.Text;
                OutputTextBoxScrolToEnd();
                MainForm.lastInputText = text;
                if (MainForm.OutputBoxStack.Contains(outputTextBox))
                {
                	foreach (RichTextBox tb in MainForm.OutputBoxStack)
                	if (tb != outputTextBox)
                	{
                		tb.Text = outputTextBox.Text;
                		tb.SelectionStart = tb.Text.Length;
            			tb.ScrollToCaret();
                	}
                }
            }
        }

        private void OutputWindowForm_Activated(object sender, EventArgs e)
        {
            MainForm.BottomActiveContent = this;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WorkbenchServiceFactory.RunService.Stop();
        }

        public void ClearTextBox()
        {
            this.outputTextBox.Clear();
        }

        string VisualPascalABCPlugins.IOutputWindow.InputTextBoxText
        {
            get { return InputTextBox.Text; }
            set { InputTextBox.Text = value; }
        }

        public void AppendTextToOutputBox(string Text)
        {
            this.outputTextBox.AppendText(Text);
        }

        private void mCOPYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.outputTextBox.Copy();
        }

        private void mSELECTALLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.outputTextBox.SelectAll();
        }
    }
}
