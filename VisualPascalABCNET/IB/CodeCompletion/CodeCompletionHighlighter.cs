// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using System.IO;
using CodeCompletion;

namespace VisualPascalABC
{
    public class CodeCompletionHighlighter
    {
        private static Hashtable markers = new Hashtable();
        private static Hashtable highlighted_keywords = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
        private static Hashtable ignored_keywords = new Hashtable(StringComparer.CurrentCultureIgnoreCase);

        static CodeCompletionHighlighter()
        {
            highlighted_keywords["try"] = "try";
            highlighted_keywords["case"] = "case";
            highlighted_keywords["class"] = "class";
            highlighted_keywords["record"] = "record";
            //highlighted_keywords["interface"] = "interface";
            //ignored_keywords["class"] = "class";
            //ignored_keywords["record"] = "record";
            //ignored_keywords["interface"] = "interface";
        }

        public static void Highlight(TextArea textArea)
        {
            try
            {

                int beg_off = -1;
                int end_off = -1;
                List<TextMarker> marks = markers[textArea] as List<TextMarker>;
                if (marks == null)
                {
                    marks = new List<TextMarker>();
                    markers[textArea] = marks;
                }
                RemoveMarkers(textArea);
                string word = GetWordAtOffset(textArea, out beg_off, out end_off);
                if (string.Compare(word, "begin", true) == 0 || highlighted_keywords[word] != null)
                {
                    if (string.Compare(word, "class", true) == 0)
                    {
                        if (isClassMember(beg_off, textArea))
                            return;
                    }
                    TmpPos end_pos = GetPositionOfEnd(textArea, end_off);
                    if (end_pos != null)
                    {
                        TextLocation Beg = textArea.Document.OffsetToPosition(beg_off);
                        TextLocation End = textArea.Document.OffsetToPosition(end_off);
                        TextMarker marker = new TextMarker(beg_off, word.Length, TextMarkerType.SolidBlock, System.Drawing.Color.FromArgb(219, 224, 204));
                        marker.WholeLine = false;
                        textArea.Document.MarkerStrategy.AddMarker(marker);
                        marks.Add(marker);
                        textArea.Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.SingleLine, textArea.Document.GetLineNumberForOffset(beg_off)));
                        textArea.EndUpdate();
                        Beg = textArea.Document.OffsetToPosition(end_pos.start_pos);
                        End = textArea.Document.OffsetToPosition(end_pos.start_pos + end_pos.len);
                        marker = new TextMarker(end_pos.start_pos, end_pos.len, TextMarkerType.SolidBlock, System.Drawing.Color.FromArgb(219, 224, 204));
                        marker.WholeLine = false;
                        textArea.Document.MarkerStrategy.AddMarker(marker);
                        marks.Add(marker);
                        textArea.Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.SingleLine, textArea.Document.GetLineNumberForOffset(end_pos.start_pos)));
                        textArea.EndUpdate();
                        textArea.Document.CommitUpdate();
                    }
                }
                else if (string.Compare(word, "end", true) == 0)
                {
                    TmpPos beg_pos = GetPositionOfBegin(textArea, beg_off);
                    if (beg_pos != null)
                    {
                        TextLocation Beg = textArea.Document.OffsetToPosition(beg_off);
                        TextLocation End = textArea.Document.OffsetToPosition(end_off);
                        TextMarker marker = new TextMarker(beg_off, 3, TextMarkerType.SolidBlock, System.Drawing.Color.FromArgb(219, 224, 204));
                        marker.WholeLine = false;
                        textArea.Document.MarkerStrategy.AddMarker(marker);
                        marks.Add(marker);
                        textArea.Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.SingleLine, textArea.Document.GetLineNumberForOffset(beg_off)));
                        textArea.EndUpdate();
                        Beg = textArea.Document.OffsetToPosition(beg_pos.start_pos);
                        End = textArea.Document.OffsetToPosition(beg_pos.start_pos + beg_pos.len);
                        marker = new TextMarker(beg_pos.start_pos, beg_pos.len, TextMarkerType.SolidBlock, System.Drawing.Color.FromArgb(219, 224, 204));
                        marker.WholeLine = false;
                        textArea.Document.MarkerStrategy.AddMarker(marker);
                        marks.Add(marker);
                        textArea.Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.SingleLine, textArea.Document.GetLineNumberForOffset(beg_pos.start_pos)));
                        textArea.EndUpdate();
                        textArea.Document.CommitUpdate();
                    }
                }
            }
            catch (Exception e)
            {
#if DEBUG
                File.AppendAllText("log.txt", e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
#endif
            }
        }

        private static bool remove_pred(TextMarker marker)
        {
            return true;
        }

        private static bool isClassMember(int beg_off, TextArea textArea)
        {
            int off = beg_off - 1;
            if (CheckForCommentOrKav(textArea.Document.TextContent, beg_off))
                return false;
            char c = textArea.Document.TextContent[off];
            while (char.IsWhiteSpace(c))
            {
                off--;
                if (off < 0)
                    return false;
                c = textArea.Document.TextContent[off];
            }
            c = char.ToLower(c);
            if (c != '=' && c != 't' && c != 'd' && c != 'o')
            {
                return true;
            }
            if (c == 't' || c == 'd' || c == 'o')
            {
                StringBuilder keyword = new StringBuilder();
                
                while (char.IsLetter(c))
                {
                    keyword.Insert(0, c);
                    off--;
                    if (off < 0)
                        break;
                    c = textArea.Document.TextContent[off];
                }
                if (keyword.ToString().ToLower() == "sealed" || keyword.ToString().ToLower() == "abstract" || keyword.ToString().ToLower() == "auto")
                {
                    return isClassMember(off, textArea);
                }
                return false;
            }
            return false;
        }

        private static void RemoveMarkers(TextArea textArea)
        {
            //textArea.Document.MarkerStrategy.RemoveAll(remove_pred);
            List<TextMarker> marks = markers[textArea] as List<TextMarker>;
            if (marks != null)
            {
                foreach (TextMarker marker in marks)
                {
                    try
                    {
                        textArea.Document.MarkerStrategy.RemoveMarker(marker);
                        textArea.Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.SingleLine, textArea.Document.GetLineNumberForOffset(marker.Offset)));
                        textArea.EndUpdate();
                    }
                    catch
                    {

                    }
                }
                marks.Clear();
            }
            textArea.Document.CommitUpdate();
        }

        class TmpPos
        {
            public int start_pos;
            public int len;
            public TmpPos(int start_pos, int len)
            {
                this.start_pos = start_pos;
                this.len = len;
            }
        }

        private static TmpPos GetPositionOfEnd(TextArea textArea, int end_off)
        {
            string text = textArea.Document.TextContent;
            int off = textArea.Caret.Offset;
            if (CheckForCommentOrKavAhead(text, off - 1)) //proverka esli my v kommentarii ili v kavychkah
                return null;
            Stack<string> beg_stack = new Stack<string>();
            beg_stack.Push("begin");
            StringBuilder sb = new StringBuilder();
            while (end_off < text.Length)
            {
                char c = text[end_off];
                if (char.IsLetterOrDigit(c) || c == '_' || c == '&')
                {
                    sb.Append(c);
                }
                else if (c == '}')//okazalis vnutri kommentarija
                {
                    return null;
                }
                else if (c == '{')//nachalo kommentarija, propuskaem ego
                {
                    end_off++;
                    while (end_off < text.Length && text[end_off] != '}')
                        end_off++;
                }
                else if (c == '\'')//nachalo kavychek, propuskaem
                {
                    end_off++;
                    while (end_off < text.Length && text[end_off] != '\'')
                        end_off++;
                }
                else if (c == '/' && end_off < text.Length - 1 && text[end_off + 1] == '/')
                {
                    while (end_off < text.Length && text[end_off] != '\n')
                        end_off++;
                }
                else
                {
                    string word = sb.ToString();
                    if (string.Compare(word, "begin", true) == 0 || highlighted_keywords[word] != null || ignored_keywords[word] != null)
                    {
                        //if (!CheckForCommentOrKavAhead(text, end_off))
                        if (string.Compare(word, "class", true) != 0 || !isClassMember(end_off, textArea))
                            beg_stack.Push(word);
                    }
                    else if (string.Compare(word, "end", true) == 0)
                    {
                        //if (!CheckForCommentOrKavAhead(text, end_off))
                        {
                            string s = beg_stack.Pop();
                            if (beg_stack.Count == 0 && (string.Compare(s, "begin", true) == 0 || highlighted_keywords[s] != null))
                                return new TmpPos(end_off - 3, word.Length);
                            if (beg_stack.Count == 0 && ignored_keywords[s] != null)
                                return null;
                        }
                    }
                    sb.Remove(0, sb.Length);
                }
                end_off++;
            }
            if (string.Compare(sb.ToString(), "end", true) == 0 && beg_stack.Count > 0)
            {
                return new TmpPos(end_off - 3, 3);
            }
            return null;
        }

        private static TmpPos GetPositionOfBegin(TextArea textArea, int beg_off)
        {
            string text = textArea.Document.TextContent;
            int off = textArea.Caret.Offset;
            if (CheckForCommentOrKav(text, off))//zdes nado proverjat vpered i nazad
                return null;
            Stack<string> beg_stack = new Stack<string>();
            beg_stack.Push("end");
            StringBuilder sb = new StringBuilder();
            bool was_comment = false;
            while (beg_off >= 0)
            {
                char c = text[beg_off];
                if (char.IsLetterOrDigit(c) || c == '_' || c == '&')
                {
                    sb.Insert(0, c);
                }
                else if (c == '{')//v kommentarii, vyhodim
                {
                    if (!was_comment)
                        return null;
                }
                else if (c == '}')//nachalo kommenta
                {
                    beg_off--;
                    was_comment = true;
                    while (beg_off >= 0 && text[beg_off] != '{')
                        beg_off--;
                }
                else if (c == '\'')
                {
                    beg_off--;
                    while (beg_off >= 0 && text[beg_off] != '\'')
                        beg_off--;
                }
                else
                {
                    string word = sb.ToString();
                    if (string.Compare(word, "end", true) == 0)
                    {
                        if (!CheckForCommentOrKavAhead(text, beg_off))//esli end ne v kavychkah i ne v kommentarii
                            beg_stack.Push("end");
                    }
                    else if (string.Compare(word, "begin", true) == 0 || highlighted_keywords[word] != null || ignored_keywords[word] != null)
                    {
                        if (!CheckForCommentOrKavAhead(text, beg_off))
                        {
                            if (string.Compare(word, "class", true) == 0)
                            {
                                if (isClassMember(beg_off, textArea))
                                {
                                    sb.Remove(0, sb.Length);
                                    beg_off--;
                                    continue;
                                }
                            }
                            beg_stack.Pop();
                            if (beg_stack.Count == 0 && (string.Compare(word, "begin", true) == 0 || highlighted_keywords[word] != null))
                            {
                                return new TmpPos(beg_off + 1, word.Length);
                            }
                            if (beg_stack.Count == 0 && ignored_keywords[word] != null)
                                return null;
                        }
                    }
                    sb.Remove(0, sb.Length);
                }
                beg_off--;
            }
            string s = sb.ToString();
            if ((string.Compare(s, "begin", true) == 0 || highlighted_keywords[s] != null) && beg_stack.Count > 0)
            {
                return new TmpPos(beg_off + 1, s.Length);
            }
            return null;
        }

        private static bool CheckForCommentOrKavAhead(string Text, int off)
        {
            int i = off;
            Stack<char> kav = new Stack<char>();
            bool is_comm = false;
            while (i >= 0 && !is_comm && Text[i] != '\n')
            {
                if (Text[i] == '\'')
                {
                    if (kav.Count == 0) kav.Push('\'');
                    else kav.Pop();
                }
                else if (Text[i] == '{')
                {
                    if (kav.Count == 0) is_comm = true;
                }
                else if (Text[i] == '}')
                {
                    return false;
                }
                else if (Text[i] == '/')
                    if (i > 0 && Text[i - 1] == '/' && kav.Count == 0)
                        is_comm = true;
                i--;
            }
            return is_comm || kav.Count > 0;
        }

        private static bool CheckForCommentOrKav(string Text, int off)
        {
            int i = off;
            Stack<char> kav = new Stack<char>();
            bool is_comm = false;
            while (i >= 0 && !is_comm && Text[i] != '\n')
            {
                if (Text[i] == '\'')
                {
                    if (kav.Count == 0) kav.Push('\'');
                    else kav.Pop();
                }
                else if (Text[i] == '{')
                {
                    if (kav.Count == 0) is_comm = true;
                }
                else if (Text[i] == '}')
                {
                    return false;
                }
                else if (Text[i] == '/')
                    if (i > 0 && Text[i - 1] == '/' && kav.Count == 0)
                        is_comm = true;
                i--;
            }
            if (is_comm || kav.Count > 0)
                return true;
            i = off;
            while (i < Text.Length && !is_comm && Text[i] != '\n')
            {
                if (Text[i] == '}')
                {
                    return true;
                }
                i++;
            }
            return false;
        }

        private static string GetWordAtOffset(TextArea textArea, out int beg_off, out int end_off)
        {
            string text = textArea.Document.TextContent;
            int off = textArea.Caret.Offset;
            StringBuilder sb = new StringBuilder();
            if (off < 0 || off >= text.Length /*|| !(char.IsLetterOrDigit(text[off]) || text[off]=='&' || text[off]=='_')*/)
            {
                beg_off = -1;
                end_off = -1;
                return "";
            }
            if (off >= 1 && (off >= text.Length || !(char.IsLetterOrDigit(text[off]) || text[off] == '&' || text[off] == '_')) && (char.IsLetterOrDigit(text[off - 1]) || text[off - 1] == '&' || text[off - 1] == '_'))
                off--;
            if (off < 0 || !(char.IsLetterOrDigit(text[off]) || text[off] == '&' || text[off] == '_'))
            {
                beg_off = -1;
                end_off = -1;
                return "";
            }
            while (off >= 0 && off < text.Length && (char.IsLetterOrDigit(text[off]) || text[off] == '&' || text[off] == '_'))
            {
                off--;
            }
            off++;
            beg_off = off;
            while (off < text.Length && (char.IsLetterOrDigit(text[off]) || text[off] == '&' || text[off] == '_'))
            {
                sb.Append(text[off]);
                off++;
            }
            end_off = off;
            return sb.ToString();
        }
    }
}

