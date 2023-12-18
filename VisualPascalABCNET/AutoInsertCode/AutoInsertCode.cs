using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;

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
            Current = GetLine(ta.Caret.Line);

            var i = ta.Caret.Line + 1;
            do
            {
                Next = GetLine(i);
                i++;
            } while (Next != null && Next.Trim() == "");

            Prev = null;
            if (ta.Caret.Line == 0)
                return;
            var j = ta.Caret.Line;
            do
            {
                j--;
                Prev = GetLine(j);
            } while (j > 0 && Prev.Trim() == "");
            if (Prev.Trim() == "")
                Prev = null;
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

                /*if (ch == ' ')
                {
                    var caret1 = editor.ActiveTextAreaControl.Caret;
                    int start1 = TextUtilities.FindPrevWordStart(editor.Document, caret1.Offset);
                    var Text1 = editor.Document.GetText(start1, caret1.Offset - start1).TrimEnd();

                    if (Text1.ToLower().StartsWith("for") || Text1.ToLower().StartsWith("foreach"))
                    {
                        var curline = TextUtilities.GetLineAsString(editor.Document, caret1.Line);
                        var len = curline.TrimEnd().Length;
                        if (ta.Caret.Column == len)
                        {
                            doc.Replace(CurrentOffset(), curline.Length - len, " var");
                            ta.Caret.Column = ta.Caret.Column + 4;
                        }
                    }
                    return false;
                }*/
                /*if (ch == '(')
                {
                    var tl_beg = new TextLocation(ta.Caret.Column, ta.Caret.Line);
                    int offset = doc.PositionToOffset(tl_beg);
                    doc.Insert(offset, ")");
                    return false;
                }*/
                if (ch != '\n')
                    return false;
                var caret = editor.ActiveTextAreaControl.Caret;
                int start = TextUtilities.FindPrevWordStart(editor.Document, caret.Offset);
                // Нужно, чтобы в Text было последнее слово в строке !!! Исключение - когда в следующей надо сделать просто сдвиг
               
                var Text = editor.Document.GetText(start, caret.Offset - start).TrimEnd();
                var TextToLower = Text.ToLower();
                //var curStrEnd = TextUtilities.GetLineAsString(editor.Document, caret.Line).Substring(caret.Column);

                if (TextToLower == "begin")
                {
                    string cur, next, prev;
                    GetCurNextLines(out cur, out next, out prev);
                    // Ничего не надо делать если begin - не последнее слово в строке
                    var curStrEnd = cur.Substring(caret.Column);
                    if (!string.IsNullOrWhiteSpace(curStrEnd))
                        return false;
                    // анализ предыдущей
                    var icur = Indent(cur);
                    if (cur.Trim().ToLower() == "begin" && (next == null || Indent(next) < icur || Indent(next) == icur && !next.TrimStart().ToLower().StartsWith("end"))) // значит, это единственное слово begin в строке
                    {
                        // Проанализируем предыдущий оператор по первому слову
                        var pst = prev?.TrimStart().ToLower();
                        if (pst != null)
                            //if (pst.StartsWith("if") || pst.StartsWith("for") || pst.StartsWith("loop") || pst.StartsWith("with") || pst.StartsWith("on") || pst.StartsWith("while") || pst.StartsWith("else") || pst.StartsWith("foreach")) // потом улучшу - для нескольких выборов
                            if (Regex.IsMatch(pst, "^(if|else|for|loop|while|foreach|with|on)"))
                            {
                                // Надо удалить в текущей строке пробелы чтобы выровнять begin по if
                                var iprev = Indent(prev);
                                var tl_beg = new TextLocation(ta.Caret.Column, ta.Caret.Line);
                                int offset = doc.PositionToOffset(tl_beg);

                                var cls = doc.GetLineSegment(ta.Caret.Line);

                                // надо удалить всю строчку с begin и сформировать новую
                                doc.Replace(cls.Offset, cls.Length , Spaces(iprev)+"begin");
                                ta.Caret.Column = iprev + 5;
                                icur = iprev;
                            }
                    }

                    ta.InsertString("\n" + Spaces(icur + 2));
                    if (next == null || Indent(next) < icur || Indent(next) == icur && !next.TrimStart().ToLower().StartsWith("end"))
                    {
                        string whole_text = editor.Document.TextContent.Trim();
                        var tl_beg = new TextLocation(ta.Caret.Column, ta.Caret.Line);
                        int offset = doc.PositionToOffset(tl_beg);
                        var send = "\n" + Spaces(icur) + "end";
                        bool is_short = false;
                        int ind = whole_text.IndexOf("##");
                        if (ind != -1 && ind + 2 < whole_text.Length - 1 && (whole_text[ind + 2] == '\r' || whole_text[ind + 2] == '\n' || whole_text[ind + 2] == ' '))
                            is_short = true;
                        if (!is_short && next == null && (prev == null || !(prev.TrimStart().ToLower().StartsWith("procedure") || prev.TrimStart().ToLower().StartsWith("function"))))
                            send += ".";
                        else send += ";";
                        doc.Insert(offset, send);
                    }
                    return true;
                }

                else if (TextToLower == "repeat") // repeat .. until
                {
                    string cur, next, prev;
                    GetCurNextLines(out cur, out next, out prev);
                    if (!string.IsNullOrWhiteSpace(cur.Substring(caret.Column)))
                        return false;
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

                else if (Regex.IsMatch(TextToLower,"class|record")) // class .. end, record .. end
                {
                    string cur, next, prev;
                    GetCurNextLines(out cur, out next, out prev);
                    if (!string.IsNullOrWhiteSpace(cur.Substring(caret.Column)))
                        return false;
                    var icur = Indent(cur);
                    ta.InsertString("\n" + Spaces(icur + 2));
                    if (next == null || Indent(next) < icur || Indent(next) == icur && !next.TrimStart().ToLower().StartsWith("until"))
                    {
                        var tl_beg = new TextLocation(ta.Caret.Column, ta.Caret.Line);
                        int offset = doc.PositionToOffset(tl_beg);
                        var send = "\n" + Spaces(icur) + "end;";
                        doc.Insert(offset, send);
                    }
                    return true;
                }

                else if (TextToLower == "of")
                {
                    string cur, next, prev;
                    GetCurNextLines(out cur, out next, out prev);
                    if (!string.IsNullOrWhiteSpace(cur.Substring(caret.Column)))
                        return false;
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

                else if (TextToLower == "with")
                {
                    string cur, next, prev;
                    GetCurNextLines(out cur, out next, out prev);
                    if (!string.IsNullOrWhiteSpace(cur.Substring(caret.Column)))
                        return false;
                    var icur = Indent(cur);
                    ta.InsertString("\n" + Spaces(icur + 2));
                    if (cur.TrimStart().ToLower().StartsWith("match") && next == null || Indent(next) < icur || Indent(next) == icur && !next.TrimStart().ToLower().StartsWith("end"))
                    {
                        var tl_beg = new TextLocation(ta.Caret.Column, ta.Caret.Line);
                        int offset = doc.PositionToOffset(tl_beg);
                        var send = "\n" + Spaces(icur) + "end;";
                        doc.Insert(offset, send);
                    }
                    return true;
                }

                else if (Regex.IsMatch(TextToLower, "then|else|do|try|except|finally|var"))
                {
                    if (ta.Caret.Line > 0 && TextToLower == "else")
                    {
                        var prevSeg = doc.GetLineSegment(ta.Caret.Line - 1);
                        var prev = GetLine(ta.Caret.Line - 1).TrimEnd();

                        if (prev[prev.Length-1] == ';')
                        {
                            doc.Replace(prevSeg.Offset + prevSeg.Length - 1, 1, "");
                        }
                    }

                    var cur = GetLine(ta.Caret.Line);
                    var icur = Indent(cur);
                    ta.InsertString("\n" + Spaces(icur + 2));

                    return true;
                }

                else
                {
                    string cur, next, prev;
                    GetCurNextLines(out cur, out next, out prev);
                    if (ta.Caret.Column >= cur.Length && prev != null)
                    {
                        var prevTrimEnd = prev.TrimEnd();
                        // если prev заканчивается на then, а cur не начинается с begin, то отступ назад
                        if ((prevTrimEnd.EndsWith("then",StringComparison.InvariantCultureIgnoreCase) ||
                            prevTrimEnd.EndsWith("else", StringComparison.InvariantCultureIgnoreCase) ||
                            prevTrimEnd.EndsWith("do", StringComparison.InvariantCultureIgnoreCase))
                            &&
                            !(cur.TrimStart().StartsWith("begin", StringComparison.InvariantCultureIgnoreCase)))
                        {
                            var iprev = Indent(prev);
                            ta.InsertString("\n" + Spaces(iprev));
                            return true;
                        }

                        /* if (Regex.IsMatch(curline, @"^\s*\w+\s*:=", RegexOptions.IgnoreCase)) // пока тут добавление ; после присваивания. Плохо перед else! И плохо если выражение продолжается на следующей строке! Убираем.
                        {
                            if (curline.Length > 0)
                            {
                                var last = curline[curline.Length - 1];
                                if (!char.IsLetterOrDigit(last) && last != ']' && last != ')')
                                    return false;
                            }

                            var curlinenew = Regex.Replace(curline, @"(\s*)(\S+)(\s*):=(\s*)([^;]+)(\s*)(;?)$", @"$1$2 := $5;", RegexOptions.IgnoreCase);
                            while (curlinenew.EndsWith(";;"))
                                curlinenew = curlinenew.Remove(curlinenew.Length - 1);
                            doc.Replace(seg.Offset, curline.Length, curlinenew);
                            ta.Caret.Column = curlinenew.Length;
                            return false;
                        } */
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
