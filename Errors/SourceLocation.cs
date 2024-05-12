// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

namespace PascalABCCompiler
{
    public class TextPosition
    {
        private int column;
        private int line;
        public int Column 
        { 
            get 
            { 
                return column; 
            }
            set
            {
                column = value;
            }
        }
        public int Line 
        { 
            get 
            { 
                return line; 
            }
            set
            {
                line = value;
            }
        }
        public TextPosition(int Line, int Column)
        {
            column = Column;
            line = Line;
        }
    }
    public class SourceLocation
    {
        private TextPosition beginPosition;
        public TextPosition BeginPosition
        {
            get
            {
                return beginPosition;
            }
        }
        private TextPosition endPosition;
        public TextPosition EndPosition
        {
            get
            {
                return endPosition;
            }
        }
        private string fileName;
        public string FileName
        {
            get
            {
                return fileName;
            }
        }
        public SourceLocation(string FileName, int BeginLine, int BeginColumn, int EndLine, int EndColumn)
        {
            fileName = FileName;
            beginPosition = new TextPosition(BeginLine, BeginColumn);
            endPosition = new TextPosition(EndLine, EndColumn);
        }
        public SourceLocation(SourceLocation Begin,SourceLocation End)
        {
            fileName = Begin.FileName;
            beginPosition = Begin.beginPosition;
            endPosition = End.EndPosition;
        }
        public static bool operator<(SourceLocation left, SourceLocation right)
        {
            if (left.beginPosition.Line != right.beginPosition.Line)
                return left.beginPosition.Line < right.beginPosition.Line;
            else
                return left.beginPosition.Column < right.beginPosition.Column; 
        }
        public static bool operator>(SourceLocation left, SourceLocation right)
        {
            if (left.beginPosition.Line != right.beginPosition.Line)
                return left.beginPosition.Line > right.beginPosition.Line;
            else
                return left.beginPosition.Column > right.beginPosition.Column;
        }
    }
}
