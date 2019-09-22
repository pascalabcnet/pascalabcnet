using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor;
using System.Text.RegularExpressions;

namespace VisualPascalABC
{
    public partial class Form1
    {
        string Spaces(int n) => new string(' ', n);
        string GetLine(int lineNum)
        {
            var ta = CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.TextArea;
            var doc = ta.Document;
            if (lineNum < doc.TotalNumberOfLines)
                return TextUtilities.GetLineAsString(doc, lineNum); 
            else return null;
        }

        void GetCurNextLines(out string Current, out string Next, out string Prev) // Next - следующая непустая строка
        {
            var ta = CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.TextArea;
            var doc = ta.Document;
            var tl_beg = new TextLocation(ta.Caret.Column, ta.Caret.Line);
            var cls = doc.GetLineSegment(ta.Caret.Line);
            LineSegment nls = null;
            if (ta.Caret.Line + 1 < doc.TotalNumberOfLines)
                nls = doc.GetLineSegment(ta.Caret.Line + 1);
            Current = GetLine(ta.Caret.Line);
            var i = ta.Caret.Line + 1;
            do
            {
                Next = GetLine(i);
                i++;
            } while (Next != null && Next.Trim() == "");
            Prev = null;
            if (ta.Caret.Line > 0)
                Prev = GetLine(ta.Caret.Line - 1);
        }

        int Indent(string s)
        {
            return Array.FindIndex(s.ToCharArray(), c => c != ' ');
        }

        int CurrentOffset()
        {
            var editor = VisualPascalABCProgram.MainForm.CurrentCodeFileDocument.TextEditor;
            var ta = editor.ActiveTextAreaControl.TextArea;
            var tl_beg = new TextLocation(ta.Caret.Column, ta.Caret.Line);
            return editor.ActiveTextAreaControl.TextArea.Document.PositionToOffset(tl_beg);
        }
        private bool TextArea_KeyEventHandler(char ch)
        {
            if (!tsAutoInsertCode.Checked)
                return false;
            try
            {
                var editor = VisualPascalABCProgram.MainForm.CurrentCodeFileDocument.TextEditor;
                var ta = editor.ActiveTextAreaControl.TextArea;
                var doc = ta.Document;

                if (ch == ' ')
                {
                    var caret1 = editor.ActiveTextAreaControl.Caret;
                    int start1 = TextUtilities.FindPrevWordStart(editor.Document, caret1.Offset);
                    var Text1 = editor.Document.GetText(start1, caret1.Offset - start1).TrimEnd();

                    if (Text1.ToLower().StartsWith("for"))
                    {
                        doc.Insert(CurrentOffset(), " var");
                        ta.Caret.Column = ta.Caret.Column + 5;
                    }
                    return false;
                }
                /*if (ch == '(')
                {
                    var tl_beg = new TextLocation(ta.Caret.Column, ta.Caret.Line);
                    int offset = doc.PositionToOffset(tl_beg);
                    doc.Insert(offset, ")");
                    return false;
                }
                if (ch == '[')
                {
                    var tl_beg = new TextLocation(ta.Caret.Column, ta.Caret.Line);
                    int offset = doc.PositionToOffset(tl_beg);
                    doc.Insert(offset, "]");
                    return false;
                }
                if (ch == '{')
                {
                    var tl_beg = new TextLocation(ta.Caret.Column, ta.Caret.Line);
                    int offset = doc.PositionToOffset(tl_beg);
                    doc.Insert(offset, "}");
                    return false;
                }*/
                if (ch != '\n')
                    return false;
                var caret = editor.ActiveTextAreaControl.Caret;
                int start = TextUtilities.FindPrevWordStart(editor.Document, caret.Offset);
                var Text = editor.Document.GetText(start, caret.Offset - start).TrimEnd();

                if (Text.ToLower() == "begin")
                {
                    string cur, next, prev;
                    GetCurNextLines(out cur, out next, out prev);
                    // анализ предыдущей
                    var icur = Indent(cur);
                    if (cur.Trim().ToLower() == "begin" && (next == null || Indent(next) < icur || Indent(next) == icur && !next.TrimStart().ToLower().StartsWith("end"))) // значит, это единственное слово begin в строке
                    {
                        // Проанализируем предыдущий оператор по первому слову
                        var pst = prev?.TrimStart().ToLower();
                        if (pst != null)
                        if (pst.StartsWith("if") || pst.StartsWith("for") || pst.StartsWith("loop") || pst.StartsWith("with") || pst.StartsWith("on") || pst.StartsWith("while") || pst.StartsWith("else") || pst.StartsWith("foreach")) // потом улучшу - для нескольких выборов
                        {
                            // Надо удалить в текущей строке пробелы чтобы выровнять begin по if
                            var iprev = Indent(prev);
                            var tl_beg = new TextLocation(ta.Caret.Column, ta.Caret.Line);
                            int offset = doc.PositionToOffset(tl_beg);
                            doc.Remove(offset - 7, 2);
                            icur -= 2;
                        }
                    }

                    ta.InsertString("\n" + Spaces(icur + 2));
                    if (next == null || Indent(next) < icur || Indent(next) == icur && !next.TrimStart().ToLower().StartsWith("end"))
                    {
                        var tl_beg = new TextLocation(ta.Caret.Column, ta.Caret.Line);
                        int offset = doc.PositionToOffset(tl_beg);
                        var send = "\n" + Spaces(icur) + "end";
                        if (next == null)
                            send += ".";
                        else send += ";";
                        doc.Insert(offset, send);
                    }
                    return true;
                }

                else if (Text.ToLower() == "repeat")
                {
                    string cur, next, prev;
                    GetCurNextLines(out cur, out next, out prev);
                    var icur = Indent(cur);
                    ta.InsertString("\n" + Spaces(icur + 2));
                    if (next == null || Indent(next) < icur || Indent(next) == icur && !next.TrimStart().ToLower().StartsWith("until"))
                    {
                        var tl_beg = new TextLocation(ta.Caret.Column, ta.Caret.Line);
                        int offset = doc.PositionToOffset(tl_beg);
                        var send = "\n" + Spaces(icur) + "until ;";
                        doc.Insert(offset, send);
                    }
                    return true;
                }

                else if (Text.ToLower() == "of")
                {
                    string cur, next, prev;
                    GetCurNextLines(out cur, out next, out prev);
                    var icur = Indent(cur);
                    ta.InsertString("\n" + Spaces(icur + 2));
                    if (cur.TrimStart().ToLower().StartsWith("case") && next == null || Indent(next) < icur || Indent(next) == icur && !next.TrimStart().ToLower().StartsWith("end"))
                    {
                        var tl_beg = new TextLocation(ta.Caret.Column, ta.Caret.Line);
                        int offset = doc.PositionToOffset(tl_beg);
                        var send = "\n" + Spaces(icur) + "end;";
                        doc.Insert(offset, send);
                    }
                    return true;
                }

                else if (Text.ToLower() == "then" || Text.ToLower() == "else" || Text.ToLower() == "do")
                {
                    var cur = GetLine(ta.Caret.Line);
                    var icur = Indent(cur);
                    ta.InsertString("\n" + Spaces(icur + 2));
                    return true;
                }
                
                else
                {
                    var seg = doc.GetLineSegment(ta.Caret.Line);
                    var curline = doc.GetText(seg);
                    if (curline.Contains(":="))
                    {
                        var curlinenew = Regex.Replace(curline, @"(\s*)(\S+)(\s*):=(\s*)(.+)(;?)(\s*)", @"$1$2 := $5;");
                        doc.Replace(seg.Offset, curline.Length, curlinenew);
                        ta.Caret.Column = curlinenew.Length;
                        return false;
                    }
                }

                return false;
            }
            catch (Exception e)
            {

            }
            return false;
        }
    }
}
