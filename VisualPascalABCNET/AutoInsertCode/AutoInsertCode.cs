using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor;


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

        private bool TextArea_KeyEventHandler(char ch)
        {
            if (!tsAutoInsertCode.Checked)
                return false;
            try
            {
                var editor = VisualPascalABCProgram.MainForm.CurrentCodeFileDocument.TextEditor;
                var ta = editor.ActiveTextAreaControl.TextArea;
                var doc = ta.Document;
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
                        if (pst.StartsWith("if") || pst.StartsWith("for") || pst.StartsWith("loop") || pst.StartsWith("with") || pst.StartsWith("on")) // потом улучшу - для нескольких выборов
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
                if (Text.ToLower() == "repeat")
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
                if (Text.ToLower() == "then" || Text.ToLower() == "else" || Text.ToLower() == "do")
                {
                    string cur, next, prev;
                    GetCurNextLines(out cur, out next, out prev);
                    var icur = Indent(cur);
                    ta.InsertString("\n" + Spaces(icur + 2));
                    return true;
                }
                /*if (Text.ToLower() == "do")
                {
                    var pos = doc.OffsetToPosition(start);
                    var ls = doc.GetLineSegment(pos.Line);
                    //var beglineoffset = ls.Offset;
                    var tttt = doc.GetText(ls);
                    var n = Array.FindIndex(tttt.ToCharArray(), c => c != ' ');
                    ta.InsertString("\n" + Spaces(n) + "begin\n" + Spaces(n + 2));
                    var tl_beg = new TextLocation(ta.Caret.Column, ta.Caret.Line);
                    int offset = doc.PositionToOffset(tl_beg);
                    doc.Insert(offset, "\n" + Spaces(n) + "end;");
                    return true;
                }*/

                return false;
            }
            catch (Exception e)
            {

            }
            return false;
        }
    }
}
