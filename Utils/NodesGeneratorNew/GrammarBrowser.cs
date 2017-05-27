// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NodesGenerator
{
    public partial class GrammarBrowser : Form
    {
        private Form1 main_form; //link to the main form for the backwards synch
        private Dictionary<string, int> keywords = new Dictionary<string,int>();
        private int definition_style;
        private int heritage_style;
        private int parent_style;
        private int hotspot_style;

        private int move_caret_to_line = -1;

        public GrammarBrowser()
        {
            InitializeComponent();            
        }

        private void move_sci_to_line(int line_num)
        {
            scintilla1.Caret.LineNumber = line_num;
            scintilla1.Lines.FirstVisibleIndex = Math.Max(0, line_num - scintilla1.Lines.VisibleCount / 2 + 2);
        }

        public void set_sci_styles()
        {
            definition_style = scintilla1.Styles.LastPredefined.Index + 1;
            heritage_style = definition_style + 1;
            parent_style = heritage_style + 1;
            hotspot_style = parent_style + 1;

            scintilla1.Styles.Default.Font = new Font(FontFamily.GenericMonospace, 10);

            scintilla1.Styles[definition_style].ForeColor = Color.Blue;            
            scintilla1.Styles[definition_style].Font = new Font(FontFamily.GenericMonospace, 10);

            scintilla1.Styles[heritage_style].ForeColor = Color.Red;            
            scintilla1.Styles[heritage_style].Font = new Font(FontFamily.GenericMonospace, 10);

            scintilla1.Styles[parent_style].ForeColor = Color.Green;
            scintilla1.Styles[parent_style].Font = new Font(FontFamily.GenericMonospace, 10);
            scintilla1.Styles[parent_style].IsHotspot = true;
            
            scintilla1.Styles[hotspot_style].Font = new Font(FontFamily.GenericMonospace, 10);
            scintilla1.Styles[hotspot_style].IsHotspot = true;
        }

        public void SetDialogPosition(Point loc)
        {
            StartPosition = FormStartPosition.Manual;
            Location = loc;
        }

        public GrammarBrowser(string text, List<string> kwords, string start_pos, Form1 main_f)
        {
            InitializeComponent();

            main_form = main_f;

            saveFileDialog1.InitialDirectory = Application.StartupPath;
        
            scintilla1.Text = text;
            scintilla1.IsReadOnly = true;

            //setting styles
            set_sci_styles();

            scintilla1.GetRange().SetStyle(scintilla1.Styles.Default.Index);

            //Lexing and coloring our view
            for (int i = 0; i < scintilla1.Lines.Count; i++)
            {
                if (scintilla1.Lines[i].Text == "") continue;
                
                string word = scintilla1.Lines[i].Text.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
                if (keywords.ContainsKey(word) || kwords.Contains(word))
                {
                    var x = scintilla1.Lines[i].Text.IndexOf(word);
                    var range = new ScintillaNET.Range(scintilla1.Lines[i].Range.Start + x, scintilla1.Lines[i].Range.Start + x + word.Length, scintilla1);
                    range.SetStyle(hotspot_style);
                }

                if (scintilla1.Lines[i].Text[0] == '\t' || scintilla1.Lines[i].Text[0] == ' ') continue;
                for (int j = 0; j < kwords.Count; j++)
                {
                    if (scintilla1.Lines[i].Text.StartsWith(kwords[j])) //if starts with a keyword and NO tabs, means, that this is a definition
                    {
                        //adding keyword
                        keywords.Add(kwords[j], i);

                        //applying styles
                        var range = new ScintillaNET.Range(scintilla1.Lines[i].Range.Start, scintilla1.Lines[i].Range.Start + kwords[j].Length, scintilla1);
                        range.SetStyle(definition_style);

                        int x = scintilla1.Lines[i].Text.IndexOf("->");
                        if (x >= 0)
                        {
                            x += scintilla1.Lines[i].Range.Start;
                            range.Start = x;
                            range.End = x + 2;
                            range.SetStyle(heritage_style);

                            range.Start = x + 3;
                            range.End = scintilla1.Lines[i].Range.End;
                            range.SetStyle(parent_style);
                        }

                        kwords.RemoveAt(j);
                        break;
                    }
                }
            }

            //moving to the starting pos
            if (keywords.ContainsKey(start_pos))
                move_sci_to_line(keywords[start_pos]);
        }

        private void saveHieararchy_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                System.IO.File.WriteAllText(saveFileDialog1.FileName, scintilla1.Text);
        }

        private bool search_internal(string text, int line_num)
        {
            int ind = scintilla1.Lines[line_num].Text.IndexOf(text);
            if (ind >= 0)
            {
                move_sci_to_line(line_num);
                scintilla1.Selection.Start = scintilla1.Lines[line_num].StartPosition + ind;
                scintilla1.Selection.End = scintilla1.Selection.Start + searchText.Text.Length;
                return true;
            }
            return false;
        }

        private void searchDown_do(string text)
        {
            for (int i = scintilla1.Caret.LineNumber + 1; i < scintilla1.Lines.Count; i++)
            {
                if (search_internal(text, i)) break;
            }
        }

        private void searchUp_do(string text)
        {
            for (int i = scintilla1.Caret.LineNumber - 1; i >= 0; i--)
            {
                if (search_internal(text, i)) break;
            }
        }

        private void searchDown_Click(object sender, EventArgs e)
        {
            searchDown_do(searchText.Text);
        }

        private void searchUp_Click(object sender, EventArgs e)
        {
            searchUp_do(searchText.Text);
        }

        private void GrammarBrowser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                if (e.Control)
                    searchUp_do(searchText.Text);
                else
                    searchDown_do(searchText.Text);
        }

        private void searchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (e.Control)
                    searchUp_do(searchText.Text);
                else
                    searchDown_do(searchText.Text);
        }

        private void scintilla1_HotspotClick(object sender, ScintillaNET.HotspotClickEventArgs e)
        {
            move_caret_to_line = keywords[scintilla1.GetWordFromPosition(e.Position)];
            move_sci_to_line(move_caret_to_line);
        }     

        private void scintilla1_SelectionChanged(object sender, EventArgs e)
        {
            //navigation handling
            if (move_caret_to_line >= -1)
            {
                scintilla1.Selection.Length = 0;                
            }
            if (move_caret_to_line == -1)
                move_caret_to_line = -2;

            //main form synch            
            string w = scintilla1.GetWordFromPosition(scintilla1.CurrentPos);
            foreach (var node in main_form.nodes_list.Items)
                if (node.ToString() == w)
                {
                    main_form.nodes_list.SelectedItem = node;
                    break;
                }
        }

        private void scintilla1_MouseUp(object sender, MouseEventArgs e)
        {
            move_caret_to_line = -1;
        }

    }
}
