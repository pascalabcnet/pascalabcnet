using System;
using System.Collections.Generic;

namespace VisualPascalABCPlugins
{
    // Text-only lifetime tracking, NOT expression/type/overload analysis (that stays in LSP).
    internal sealed class Net10SignatureContext
    {
        internal readonly ICodeFileDocument Document;
        internal readonly string FileName, Text, CallPrefix;
        internal readonly int Offset, OpenOffset;

        private Net10SignatureContext(ICodeFileDocument document, int offset, int open)
        {
            Document = document; FileName = document.FileName;
            Text = document.TextEditor.Document.TextContent;
            Offset = offset; OpenOffset = open; CallPrefix = Text.Substring(0, open + 1);
        }

        internal static bool IsTrigger(char ch) => ch == '(' || ch == '[' || ch == ',';
        internal static Net10SignatureContext Capture(ICodeFileDocument document, int offset)
        {
            string text = document.TextEditor.Document.TextContent;
            if (offset < 1 || offset > text.Length || !IsTrigger(text[offset - 1])) return null;
            var opens = OpenBrackets(text, offset, out bool inCode);
            return !inCode || opens.Count == 0 ? null : new Net10SignatureContext(document, offset, opens[opens.Count - 1]);
        }

        internal bool TryText(string text)
        {
            int length = text.Length - Text.Length;
            return length >= 0 && text.StartsWith(Text.Substring(0, Offset), StringComparison.Ordinal) &&
                text.EndsWith(Text.Substring(Offset), StringComparison.Ordinal);
        }

        internal bool IsCurrent => string.Equals(FileName, Document.FileName, StringComparison.Ordinal) &&
            TryText(Document.TextEditor.Document.TextContent) &&
            Document.TextEditor.ActiveTextAreaControl.Caret.Offset == Offset + Document.TextEditor.Document.TextLength - Text.Length &&
            IsInsideCall(Document.TextEditor.ActiveTextAreaControl.Caret.Offset);

        internal bool IsInsideCall(int caret)
        {
            if (Document.TextEditor.IsDisposed || !string.Equals(FileName, Document.FileName, StringComparison.Ordinal)) return false;
            string text = Document.TextEditor.Document.TextContent;
            return caret > OpenOffset && caret <= text.Length && text.StartsWith(CallPrefix, StringComparison.Ordinal) &&
                OpenBrackets(text, caret, out _).Contains(OpenOffset);
        }

        internal bool RequestPrefixUnchanged => Document.TextEditor.Document.TextContent.StartsWith(Text.Substring(0, Offset), StringComparison.Ordinal);
        internal bool IsRequestPrefixCurrent(int caret) => caret >= Offset && RequestPrefixUnchanged && IsInsideCall(caret);

        private static List<int> OpenBrackets(string text, int end, out bool inCode)
        {
            var opens = new List<int>();
            var comments = new Stack<char>();
            bool quoted = false, lineComment = false;
            for (int i = 0; i < end; i++)
            {
                char ch = text[i]; char next = i + 1 < end ? text[i + 1] : '\0';
                if (lineComment) { if (ch == '\r' || ch == '\n') lineComment = false; continue; }
                if (quoted)
                {
                    if (ch == '\'') { if (next == '\'') i++; else quoted = false; }
                    continue;
                }
                if (comments.Count > 0)
                {
                    if (ch == '{') comments.Push('{');
                    else if (ch == '(' && next == '*') { comments.Push('*'); i++; }
                    else if (ch == '}' && comments.Peek() == '{') comments.Pop();
                    else if (ch == '*' && next == ')' && comments.Peek() == '*') { comments.Pop(); i++; }
                    continue;
                }
                if (ch == '\'') quoted = true;
                else if (ch == '/' && next == '/') { lineComment = true; i++; }
                else if (ch == '{') comments.Push('{');
                else if (ch == '(' && next == '*') { comments.Push('*'); i++; }
                else if (ch == '(' || ch == '[') opens.Add(i);
                else if (ch == ')' || ch == ']')
                {
                    if (opens.Count > 0 && text[opens[opens.Count - 1]] == (ch == ')' ? '(' : '[')) opens.RemoveAt(opens.Count - 1);
                    else opens.Clear();
                }
                else if (ch == ';') opens.Clear();
            }
            inCode = !quoted && !lineComment && comments.Count == 0;
            return opens;
        }
    }
}
