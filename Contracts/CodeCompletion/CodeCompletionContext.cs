namespace PascalABCCompiler.Contracts
{
    public class CodeCompletionContext
    {
        public string FileName { get; }
        public string FullText { get; }
        public int CaretOffset { get; }
        public int Line { get; }
        public int Column { get; }

        public CodeCompletionContext(string fileName, string fullText, int caretOffset, int line, int column)
        {
            FileName = fileName;
            FullText = fullText;
            CaretOffset = caretOffset;
            Line = line;
            Column = column;
        }

        public string Prefix => FullText.Substring(0, CaretOffset);
        public string Suffix => FullText.Substring(CaretOffset);
    }
}
