#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

#endregion Using Directives


namespace ScintillaNET
{
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class Lexing : TopLevelHelper
    {
        #region Constants

        private const string DEFAULT_WHITECHARS = " \t\r\n\0";
        private const string DEFAULT_WORDCHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";

        #endregion Constants


        #region Fields

        private KeywordCollection _keywords;
        private Dictionary<string, string> _lexerLanguageMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private string _lexerName = "container";
        private string _lineCommentPrefix = string.Empty;
        private string _streamCommentPrefix = string.Empty;
        private string _streamCommentSufix = string.Empty;
        private Dictionary<string, int> _styleNameMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private string _whitespaceChars;
        private string _wordChars;
        internal char[] WhitespaceCharsArr;
        internal char[] WordCharsArr = null;

        #endregion Fields


        #region Methods

        public void Colorize()
        {
            Colorize(0, -1);
        }


        public void Colorize(int startPos, int endPos)
        {
            NativeScintilla.Colourise(startPos, endPos);
        }


        private int FindFirstNonWhitespaceChar(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].ToString().IndexOfAny(WhitespaceCharsArr) == -1)
                    return i;
            }

            return -1;
        }


        public string GetProperty(string name)
        {
            string s;
            NativeScintilla.GetProperty(name, out s);
            return s;
        }


        public string GetPropertyExpanded(string name)
        {
            string s;
            NativeScintilla.GetPropertyExpanded(name, out s);
            return s;
        }


        public int GetPropertyInt(string name)
        {
            return GetPropertyInt(name, 0);
        }


        public int GetPropertyInt(string name, int defaultValue)
        {
            return NativeScintilla.GetPropertyInt(name, defaultValue);
        }


        public void LineComment()
        {
            if (string.IsNullOrEmpty(_lineCommentPrefix))
                return;

            // So the theory behind line commenting is that for every selected line
            // we look for the first non-whitespace character and insert the line
            // comment prefix. Lines without non-whitespace are skipped.
            NativeScintilla.BeginUndoAction();

            Range selRange = Scintilla.Selection.Range;
            int start = selRange.StartingLine.Number;
            int end = selRange.EndingLine.Number;

            // We're tracking the new _end of the selection range including
            // the amount it expands because we're inserting new text.
            int offset = _lineCommentPrefix.Length;

            for (int i = start; i <= end; i++)
            {
                Line l = Scintilla.Lines[i];
                int firstWordChar = FindFirstNonWhitespaceChar(l.Text);
                if (firstWordChar >= 0)
                {
                    Scintilla.InsertText(l.StartPosition + firstWordChar, _lineCommentPrefix);
                    selRange.End += offset;
                }
            }

            NativeScintilla.EndUndoAction();

            // An odd side-effect of InsertText is that we lose the current
            // selection. This is undesirable. This is why we were tracking
            // the _end position offset.
            selRange.Select();
        }


        public void LineUncomment()
        {
            if (string.IsNullOrEmpty(_lineCommentPrefix))
                return;

            NativeScintilla.BeginUndoAction();

            // Uncommenting is a lot like line commenting. However in addition
            // to looking for a non-whitespace character, the string that follows
            // it MUST be our line Comment Prefix. If this is the case the prefex
            // is removed from the line at its position.
            Range selRange = Scintilla.Selection.Range;
            int start = selRange.StartingLine.Number;
            int end = selRange.EndingLine.Number;

            int offset = _lineCommentPrefix.Length;

            for (int i = start; i <= end; i++)
            {
                Line l = Scintilla.Lines[i];
                int firstWordChar = FindFirstNonWhitespaceChar(l.Text);
                if (firstWordChar >= 0)
                {
                    int startPos = l.StartPosition + firstWordChar;
                    Range commentRange = Scintilla.GetRange(startPos, startPos + offset);
                    if (commentRange.Text == _lineCommentPrefix)
                        commentRange.Text = string.Empty;
                }
            }

            NativeScintilla.EndUndoAction();
        }


        public void LoadLexerLibrary(string path)
        {
            NativeScintilla.LoadLexerLibrary(path);
        }


        private void loadStyleMap()
        {
            if (Scintilla.IsDesignMode)
                return;

            _styleNameMap.Clear();

            // These are global constants that always apply
            _styleNameMap.Add("BRACEBAD",Constants.STYLE_BRACEBAD);
            _styleNameMap.Add("BRACELIGHT",Constants.STYLE_BRACELIGHT);
            _styleNameMap.Add("CALLTIP",Constants.STYLE_CALLTIP);
            _styleNameMap.Add("CONTROLCHAR",Constants.STYLE_CONTROLCHAR);
            _styleNameMap.Add("DEFAULT",Constants.STYLE_DEFAULT);
            _styleNameMap.Add("LINENUMBER",Constants.STYLE_LINENUMBER);

            string lexname = this.Lexer.ToString().ToLower();

            using (Stream s = GetType().Assembly.GetManifestResourceStream("ScintillaNET.Configuration.Builtin.LexerStyleNames." + lexname + ".txt"))
            {
                if (s == null)
                    return;

                using (StreamReader sr = new StreamReader(s))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] arr = sr.ReadLine().Split('=');
                        if (arr.Length != 2)
                            continue;

                        string key = arr[0].Trim();
                        int value = int.Parse(arr[1].Trim());
                        
                        _styleNameMap.Remove(key);
                        _styleNameMap.Add(key, value);
                    }
                }
            }

        }


        private void ResetLexer()
        {
            Lexer = Lexer.Container;
        }


        private void ResetLexerName()
        {
            LexerName = "container";
        }


        private void ResetWhitespaceChars()
        {
            _whitespaceChars = DEFAULT_WHITECHARS;
        }


        private void ResetWordChars()
        {
            WordChars = DEFAULT_WORDCHARS;
        }


        public void SetKeywords(int keywordSet, string list)
        {
            NativeScintilla.SetKeywords(keywordSet, list);
        }


        public void SetProperty(string name, string value)
        {
            NativeScintilla.SetProperty(name, value);
        }


        internal bool ShouldSerialize()
        {
            return ShouldSerializeLexerName() ||
                ShouldSerializeLexer() ||
                ShouldSerializeWhitespaceChars() ||
                ShouldSerializeWordChars();
        }


        private bool ShouldSerializeLexer()
        {
            return Lexer != Lexer.Container;
        }


        private bool ShouldSerializeLexerName()
        {
            return LexerName != "container";
        }


        private bool ShouldSerializeWhitespaceChars()
        {
            return _whitespaceChars != DEFAULT_WHITECHARS;
        }


        private bool ShouldSerializeWordChars()
        {
            return _wordChars != DEFAULT_WORDCHARS;
        }


        public void StreamComment()
        {
            if (string.IsNullOrEmpty(_streamCommentPrefix) || string.IsNullOrEmpty(_streamCommentSufix))
                return;

            NativeScintilla.BeginUndoAction();
            
            Range selRange = Scintilla.Selection.Range;
            Scintilla.InsertText(selRange.Start, _streamCommentPrefix);
            Scintilla.InsertText(selRange.End+ _streamCommentPrefix.Length, _streamCommentSufix);
            selRange.End += _streamCommentPrefix.Length + _streamCommentSufix.Length;
            selRange.Select();

            NativeScintilla.EndUndoAction();
        }


        public void ToggleLineComment()
        {
            if (string.IsNullOrEmpty(_lineCommentPrefix))
                return;

            NativeScintilla.BeginUndoAction();

            Range selRange = Scintilla.Selection.Range;
            int start = selRange.StartingLine.Number;
            int end = selRange.EndingLine.Number;

            int offset = _lineCommentPrefix.Length;

            for (int i = start; i <= end; i++)
            {
                Line l = Scintilla.Lines[i];
                int firstWordChar = FindFirstNonWhitespaceChar(l.Text);
                if (firstWordChar >= 0)
                {
                    int startPos = l.StartPosition + firstWordChar;
                    Range commentRange = Scintilla.GetRange(startPos, startPos + offset);
                    if (commentRange.Text == _lineCommentPrefix)
                    {
                        commentRange.Text = string.Empty;
                        selRange.End -= offset;
                    }
                    else
                    {
                        Scintilla.InsertText(l.StartPosition + firstWordChar, _lineCommentPrefix);
                        selRange.End += offset;
                    }
                }
            }

            NativeScintilla.EndUndoAction();
            selRange.Select();
        }

        #endregion Methods


        #region Properties

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public KeywordCollection Keywords
        {
            get
            {
                return _keywords;
            }
        }


        public Lexer Lexer
        {
            get
            {
                return (Lexer)NativeScintilla.GetLexer();
            }
            set
            {
                NativeScintilla.SetLexer((int)value);
                _lexerName = value.ToString().ToLower();
                if (_lexerName == "null")
                    _lexerName = "";

                loadStyleMap();

                if (!Scintilla.IsDesignMode)
                {
                    // Reapply folding
                    Scintilla.Folding.IsEnabled = Scintilla.Folding.IsEnabled;
                    Colorize();
                }
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<string, string> LexerLanguageMap
        {
            get
            {
                return _lexerLanguageMap;
            }
        }


        public string LexerName
        {
            get
            {
                return _lexerName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "null";
                    
                NativeScintilla.SetLexerLanguage(value.ToLower());

                _lexerName = value;

                loadStyleMap();

                if (!Scintilla.IsDesignMode)
                {
                    // Reapply folding
                    Scintilla.Folding.IsEnabled = Scintilla.Folding.IsEnabled;
                    Colorize();
                }
            }
        }


        public string LineCommentPrefix
        {
            get
            {
                return _lineCommentPrefix;
            }
            set
            {
                if (value == null)
                    value = string.Empty;

                _lineCommentPrefix = value;
            }
        }


        public string StreamCommentPrefix
        {
            get
            {
                return _streamCommentPrefix;
            }
            set
            {
                if (value == null)
                    value = string.Empty;

                _streamCommentPrefix = value;
            }
        }


        public string StreamCommentSufix
        {
            get
            {
                return _streamCommentSufix;
            }
            set
            {
                if (value == null)
                    value = string.Empty;

                _streamCommentSufix = value;
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<string, int> StyleNameMap
        {
            get
            {
                return _styleNameMap;
            }
        }


        [TypeConverter(typeof(ScintillaNET.WhitespaceStringConverter))]
        public string WhitespaceChars
        {
            get
            {
                return _whitespaceChars;
            }
            set
            {
                _whitespaceChars = value;
                WhitespaceCharsArr = _whitespaceChars.ToCharArray();
                NativeScintilla.SetWhitespaceChars(value);
            }
        }


        public string WordChars
        {
            get
            {
                return _wordChars;
            }
            set
            {
                _wordChars = value;
                WordCharsArr = _wordChars.ToCharArray();
                NativeScintilla.SetWordChars(value);
            }
        }

        #endregion Properties


        #region Constructors

        internal Lexing(Scintilla scintilla) : base(scintilla)
        {
            WhitespaceChars = DEFAULT_WHITECHARS;
            WordChars = DEFAULT_WORDCHARS;
            _keywords = new KeywordCollection(scintilla);

            // Language names are a superset lexer names. For instance the c and cs (c#)
            // langauges both use the cpp lexer (by default). Languages are kind of a 
            // SCite concept, while Scintilla only cares about Lexers. However we don't
            // need to explicetly map a language to a lexer if they are the same name
            // like cpp.
            _lexerLanguageMap.Add("cs", "cpp");
            _lexerLanguageMap.Add("html", "hypertext");
            _lexerLanguageMap.Add("xml", "hypertext");
        }

        #endregion Constructors
    }
}
