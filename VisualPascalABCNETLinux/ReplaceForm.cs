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
    public partial class FindReplaceForm : Form
    {
        private Form1 Form1;
        public enum FormType { Find, Replace };
        public int resOffset=-1;
        public int resLength=-1;
        public bool findSuccess=false;
        FormType formType;
        public ReplaceConfirmDialog ReplaceConfirmDlg;
        public bool ReplaceAllExec = false;
        public System.Text.RegularExpressions.Match Match;
        public System.Text.RegularExpressions.MatchCollection Matches;
        private string lastSearched;

        public FindReplaceForm(FormType formType)
        {
            InitializeComponent();
            this.formType = formType;
            this.ActiveControl = this.tbTextToFind;
            if (formType == FormType.Find)
            {
                this.AcceptButton = this.btFindNext;
                this.tbTextToReplace.Visible = false;
                this.lReplaceTo.Visible = false;
                this.btReplace.Visible = false;
                this.btReplaceAll.Visible = false;
            }
            else
            {
                this.AcceptButton = this.btReplace;
                AddOwnedForm(ReplaceConfirmDlg = new ReplaceConfirmDialog());
                PascalABCCompiler.StringResources.SetTextForAllObjects(ReplaceConfirmDlg, "VP_REPLACECONFIRMDLGFORM_");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            resOffset = -1;
            resLength = -1;
            ReplaceAllExec = false;
            Close();
        }

        private void UpdateLastWordsList(System.Windows.Forms.ComboBox cb)
        {
            if (cb.Items.Count > 0)
            {
                if ((string)cb.Items[0] != cb.Text)
                    cb.Items.Insert(0, cb.Text);
            }
            else
                cb.Items.Add(cb.Text);
        }
        
        int startOffset;

        public bool FindNext(bool exec_goto_action=true)
        {
            Form1 = (Form1)this.Owner;
            UpdateLastWordsList(this.tbTextToFind);
            if (exec_goto_action)
                startOffset = Form1.CurrentSyntaxEditor.TextEditor.ActiveTextAreaControl.TextArea.Caret.Offset;
            if (cbSearchUp.Checked && resLength >= 0)
                startOffset -= resLength;
            resOffset = -1;
            //resLength = -1;
            System.Text.RegularExpressions.RegexOptions options = System.Text.RegularExpressions.RegexOptions.None;
            if (!cbMatchCase.Checked)
                options = options | System.Text.RegularExpressions.RegexOptions.IgnoreCase;
            if (cbSearchUp.Checked)
                options = options | System.Text.RegularExpressions.RegexOptions.RightToLeft;
            string pattern = this.tbTextToFind.Text;
            if (pattern == "")
            {
                findSuccess = false;
                return findSuccess;
            }
            if (!cbUseRegex.Checked)
                pattern = System.Text.RegularExpressions.Regex.Escape(pattern);
            if (cbMathWord.Checked)
                pattern = @"\b" + pattern + @"\b";
            System.Text.RegularExpressions.Regex Regex;
            try
            {
                Regex = new System.Text.RegularExpressions.Regex(pattern, options);
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format(PascalABCCompiler.StringResources.Get("VP_REPLACEFORM_REGEXPERR{0}"),e.Message), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                findSuccess = false;
                return findSuccess;
            }
            if (cbSearchUp.Checked)
                Match = Regex.Match(Form1.CurrentSyntaxEditor.TextEditor.Document.TextContent, 0, Math.Max(startOffset, 0));
            else
                Match = Regex.Match(Form1.CurrentSyntaxEditor.TextEditor.Document.TextContent, startOffset);
            
            if (findSuccess = Match.Success)
            {
                resOffset = Match.Index;
                resLength = Match.Length;
                //Point p = Form1.CurrentSyntaxEditor.TextEditor.Document.OffsetToPosition(resOffset);
                if (exec_goto_action)
                {
                    ICSharpCode.TextEditor.TextLocation p = Form1.CurrentSyntaxEditor.TextEditor.Document.OffsetToPosition(resOffset);
                    Form1.ExecuteSourceLocationAction(new PascalABCCompiler.SourceLocation(null, p.Y + 1, p.X + 1, p.Y + 1, p.X + resLength), VisualPascalABCPlugins.SourceLocationAction.FindSelection);
                }
                else
                    startOffset = resOffset + resLength;
            }
            return findSuccess;
        }
        
        public bool Replace(bool exec_goto_action=true)
        {
            UpdateLastWordsList(this.tbTextToReplace);
            if (!findSuccess)
                FindNext(exec_goto_action);
            if (findSuccess)
            {
                /*Form1.CurrentSyntaxEditor.TextEditor.Document.Remove(resOffset, resLength);
                Form1.CurrentSyntaxEditor.TextEditor.Document.Insert(resOffset, tbTextToReplace.Text);*/
                Form1.CurrentSyntaxEditor.TextEditor.Document.Replace(resOffset, resLength, tbTextToReplace.Text);
                resLength = tbTextToReplace.Text.Length;
                //Point p = Form1.CurrentSyntaxEditor.TextEditor.Document.OffsetToPosition(resOffset);
                if (exec_goto_action)
                {
                    ICSharpCode.TextEditor.TextLocation p = Form1.CurrentSyntaxEditor.TextEditor.Document.OffsetToPosition(resOffset);
                    Form1.ExecuteSourceLocationAction(new PascalABCCompiler.SourceLocation(null, p.Y + 1, p.X + 1, p.Y + 1, p.X + resLength), VisualPascalABCPlugins.SourceLocationAction.SelectAndGotoEnd);
                }
                else
                    startOffset = resOffset + resLength;
                resOffset = -1;
                findSuccess = false;
                return true;
            }
            else
                return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void btReplace_Click(object sender, EventArgs e)
        {
            lastSearched = this.tbTextToFind.Text;
            Replace();
        }

        private void btFindNext_Click(object sender, EventArgs e)
        {
            FindNext();
        }

        private void btReplaceAll_Click(object sender, EventArgs e)
        {
            startOffset = 0;
            if (FindNext(false))
            {
                Close();
                ReplaceAll();
                /*while (findSuccess)
                {
                    ReplaceConfirmDlg.lMessage.Text = String.Format(PascalABCCompiler.StringResources.Get("VP_REPLACECONFIRMDLGFORM_REPLACE{0}TO{1}"), Match.Value, tbTextToReplace.Text);
                    ReplaceConfirmDlg.ShowDialog();
                    if (ReplaceConfirmDlg.Result == ReplaceConfirmDialog.ModalResult.Yes)
                        Replace();
                    else
                        if (ReplaceConfirmDlg.Result == ReplaceConfirmDialog.ModalResult.Cancel)
                            return;
                        else
                            if (ReplaceConfirmDlg.Result == ReplaceConfirmDialog.ModalResult.All)
                            {
                                
                                ReplaceAll();
                                return;
                            }
                    FindNext();
                }*/
            }
        }

        internal void ReplaceAll()
        {
            UpdateLastWordsList(this.tbTextToReplace);
            if (findSuccess)
            {
                System.Text.RegularExpressions.RegexOptions options = System.Text.RegularExpressions.RegexOptions.None;
                if (!cbMatchCase.Checked)
                    options = options | System.Text.RegularExpressions.RegexOptions.IgnoreCase;
                if (cbSearchUp.Checked)
                    options = options | System.Text.RegularExpressions.RegexOptions.RightToLeft;
                string pattern = this.tbTextToFind.Text;
                if (pattern == "")
                {
                    findSuccess = false;
                    return;
                }
                if (!cbUseRegex.Checked)
                    pattern = System.Text.RegularExpressions.Regex.Escape(pattern);
                if (cbMathWord.Checked)
                    pattern = @"\b" + pattern + @"\b";

                try
                {
                    if (lastSearched == this.tbTextToFind.Text)
                    {
                        int offset = Form1.CurrentSyntaxEditor.TextEditor.ActiveTextAreaControl.Caret.Offset;
                        Form1.CurrentSyntaxEditor.TextEditor.Document.Replace(offset,
                            Form1.CurrentSyntaxEditor.TextEditor.Document.TextContent.Length - offset,
                            System.Text.RegularExpressions.Regex.Replace(Form1.CurrentSyntaxEditor.TextEditor.Document.TextContent.Substring(offset), pattern, tbTextToReplace.Text));
                    }
                    else
                        Form1.CurrentSyntaxEditor.TextEditor.Document.Replace(0, Form1.CurrentSyntaxEditor.TextEditor.Document.TextContent.Length, System.Text.RegularExpressions.Regex.Replace(Form1.CurrentSyntaxEditor.TextEditor.Document.TextContent, pattern, tbTextToReplace.Text));
                    //Form1.CurrentSyntaxEditor.TextEditor.Document.CommitUpdate();
                }
                catch (Exception e)
                {
                    MessageBox.Show(string.Format(PascalABCCompiler.StringResources.Get("VP_REPLACEFORM_REGEXPERR{0}"), e.Message), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    findSuccess = false;
                    lastSearched = null;
                    return;
                }
                lastSearched = null;
                findSuccess = false;
                
            }
            
        }

        private void FindReplaceForm_Shown(object sender, EventArgs e)
        {
            Form1 = (Form1)this.Owner; 
            this.ActiveControl = tbTextToFind;
            startOffset = Form1.CurrentSyntaxEditor.TextEditor.ActiveTextAreaControl.TextArea.Caret.Offset;
            
            /*string selt = Form1.CurrentSyntaxEditor.SelectedText;
            if (selt != string.Empty)
                tbTextToFind.Text = selt;*/
        }

        private void FindReplaceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void FindReplaceForm_Activated(object sender, EventArgs e)
        {
            FindReplaceForm_Shown(sender, e);
        }
    }
}