using System.Text;

namespace VisualPascalABC
{
    // Linux stderr only. Do not buffer ordinary diagnostics or runtime input signals.
    internal sealed class MonoX11WarningFilter
    {
        private const string Warning = "Could not set X locale modifiers";
        private readonly StringBuilder prefix = new StringBuilder();
        private bool lineStart = true;
        private bool skipLf;

        public string Feed(string text)
        {
            var output = new StringBuilder();
            foreach (char ch in text)
            {
                if (skipLf)
                {
                    skipLf = false;
                    if (ch == '\n') continue;
                }
                if (lineStart)
                {
                    if (prefix.Length < Warning.Length && ch == Warning[prefix.Length])
                    {
                        prefix.Append(ch);
                        continue;
                    }
                    if (prefix.Length == Warning.Length && (ch == '\n' || ch == '\r'))
                    {
                        prefix.Clear();
                        skipLf = ch == '\r';
                        continue;
                    }
                    output.Append(prefix);
                    prefix.Clear();
                }
                output.Append(ch);
                lineStart = ch == '\n' || ch == '\r';
            }
            return output.ToString();
        }

        public string Complete()
        {
            // Incomplete lines are not the known WriteLine diagnostic: preserve them.
            string tail = prefix.ToString();
            prefix.Clear();
            lineStart = true;
            skipLf = false;
            return tail;
        }
    }
}
