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
using System.Text.RegularExpressions;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor;

namespace CodeTemplatesPlugin
{
    public partial class CodeTemplatesForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public CodeTemplateManager schoolManager;
        public VisualPascalABC.Form1 MainForm;

        private Dictionary<string, string> help = new Dictionary<string, string>();

        public void LoadTemplates()
        {
            try
            {
                schoolManager = new CodeTemplateManager("school.pct",true); // искать вначале в локальном каталоге, потом на уровень выше и только потом в системном
                listBox1.Items.Clear();
                //listBox1.Items.AddRange(schoolManager.ht.Keys.ToArray());
                foreach (var x in schoolManager.ht.Keys)
                {
                    var a = x.Split('/');
                    listBox1.Items.Add(a[0]);
                    a[1] = a[1].Trim();
                    help[a[0]] = a[2];
                    var ind = -1;
                    for (var i = 0; i < listBox1.Groups.Count; i++)
                    {
                        if (listBox1.Groups[i].Name == a[1])
                        {
                            ind = i;
                            break;
                        }
                    }
                    if (ind == -1)
                    {
                        listBox1.Groups.Add(a[1], a[1]);
                        ind = listBox1.Groups.Count - 1;
                    }
                    listBox1.Items[listBox1.Items.Count - 1].Group = listBox1.Groups[ind];
                }
            }
            catch
            {
            }
        }
        public CodeTemplatesForm()
        {
            InitializeComponent();
            var scale = VisualPascalABC.ScreenScale.Calc();
            listBox1.TileSize = new Size(listBox1.TileSize.Width, System.Convert.ToInt32(16 * scale));
            LoadTemplates();
        }

        private void CodeTemplatesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        string GetLine(int lineNum)
        {
            var ta = MainForm.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.TextArea;
            var doc = ta.Document;
            if (lineNum < doc.TotalNumberOfLines && lineNum >= 0)
            {
                var cseg = doc.GetLineSegment(lineNum);
                return TextUtilities.GetLineAsString(doc, lineNum);
            }

            else return null;
        }

        string GetPrevLine(int lineNum) // предыдущая непустая строка
        {
            var ta = MainForm.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.TextArea;

            string Prev = null;
            if (ta.Caret.Line == 0)
                return null;
            var j = ta.Caret.Line;
            do
            {
                j--;
                Prev = GetLine(j);
            } while (j > 0 && Prev.Trim() == "");
            if (Prev.Trim() == "")
                return null;
            else return Prev;
        }

        string GetNextLine(int lineNum) // следущая непустая строка
        {
            var ta = MainForm.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.TextArea;
            string Next;
            var i = ta.Caret.Line + 1;
            do
            {
                Next = GetLine(i);
                i++;
            } while (Next != null && string.IsNullOrWhiteSpace(Next));
            return Next;
        }

        private void listBox1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedItems == null)
                return;
            var s = listBox1.SelectedItems[0].Text;
            ICSharpCode.TextEditor.TextArea ta = MainForm.CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.TextArea;
            ta.Focus();
            var full = schoolManager.ht.Keys.FirstOrDefault(st => st.StartsWith(s));
            //if (schoolManager.ht.Keys.Select(st=>st.Substring(0,s.IndexOf('/'))).Contains(s))
            if (full != null)
            {
                // Если шаблон начинается с begin и предыдущая конструкция с начала строки - управляющий оператор, то сдвинуть курсор, выровняв по предыдущей конструкции
                if (s.StartsWith("begin … end"))
                {
                    var Prev = GetPrevLine(ta.Caret.Line);
                    Match m = null;
                    if (Prev != null)
                    {
                        m = Regex.Match(Prev, @"^\s*(loop|for|while|if|else)", RegexOptions.IgnoreCase);
                        if (m.Groups[1].Value.Length > 0)
                        {
                            var Curr = GetLine(ta.Caret.Line);
                            if (Curr.Length > m.Groups[1].Index)
                            {
                                ta.Caret.Column = Curr.Length;
                                ta.InsertString(new string(' ', Curr.Length - m.Groups[1].Index));
                            }
                                
                            ta.Caret.Column = m.Groups[1].Index;

                            var doc = ta.Document;
                            var tl_beg = new TextLocation(ta.Caret.Column, ta.Caret.Line);
                            int offset = doc.PositionToOffset(tl_beg);

                            if (Curr.Length > ta.Caret.Column && Curr.Substring(ta.Caret.Column).TrimEnd().Length == 0)
                                doc.Remove(offset, Curr.Length - ta.Caret.Column);
                        }
                    }
                    var Next = GetNextLine(ta.Caret.Line);
                    if (m != null && m.Groups[1].Value.ToLower().Equals("if") && Next != null && Next.TrimStart().ToLower().StartsWith("else"))
                    {
                        CodeCompletionActionsManager.GenerateTemplate(s, ta, schoolManager, false, str => str.Remove(str.Length-1)); // Удалить ; в begin end перед else
                    }
                    else
                    {
                        CodeCompletionActionsManager.GenerateTemplate(s, ta, schoolManager, false);
                    }
                }
                else
                {
                    CodeCompletionActionsManager.GenerateTemplate(s, ta, schoolManager, false);
                }
            }
            else
            {
                ta.InsertString(s);
            }
            ta.Focus();
        }

        private void listBox1_SizeChanged(object sender, EventArgs e)
        {
            if (listBox1.Size.Width > 4)
                listBox1.TileSize = new Size(listBox1.Size.Width-4, listBox1.TileSize.Height);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 0)
                return;
            var s = listBox1.SelectedItems[0].Text;
            if (help.ContainsKey(s))
            {
                label1.Text = help[s];
            }
            
        }
    }
}
