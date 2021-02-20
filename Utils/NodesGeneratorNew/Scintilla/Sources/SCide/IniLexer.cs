#region Using Directives

using System;
using System.Drawing;
using ScintillaNET;

#endregion Using Directives


namespace SCide
{
    // A helper class to use the Scintilla container as an INI lexer.
    // We'll ignore the fact that SciLexer.DLL already has an INI capable lexer. ;)
    internal sealed class IniLexer
    {
        #region Constants

        private const int EOL = -1;

        // SciLexer's weird choice for a default style _index
        private const int DEFAULT_STYLE = 32;
        
        // Our custom styles (indexes chosen not to conflict with anything else)
        private const int KEY_STYLE = 11;
        private const int VALUE_STYLE = 12;
        private const int ASSIGNMENT_STYLE = 13;
        private const int SECTION_STYLE = 14;
        private const int COMMENT_STYLE = 15;
        private const int QUOTED_STYLE = 16;

        #endregion Constants


        #region Fields

        private Scintilla _scintilla;
        private int _startPos;

        private int _index;
        private string _text;

        #endregion Fields


        #region Methods

        public static void Init(Scintilla scintilla)
        {
            // Reset any current language and enable the StyleNeeded
            // event by setting the lexer to container.
            scintilla.Indentation.SmartIndentType = SmartIndent.None;
            scintilla.ConfigurationManager.Language = String.Empty;
            scintilla.Lexing.LexerName = "container";
            scintilla.Lexing.Lexer = Lexer.Container;

            // Add our custom styles to the collection
            scintilla.Styles[QUOTED_STYLE].ForeColor = Color.FromArgb(153, 51, 51);
            scintilla.Styles[KEY_STYLE].ForeColor = Color.FromArgb(0, 0, 153);
            scintilla.Styles[ASSIGNMENT_STYLE].ForeColor = Color.OrangeRed;
            scintilla.Styles[VALUE_STYLE].ForeColor = Color.FromArgb(102, 0, 102);
            scintilla.Styles[COMMENT_STYLE].ForeColor = Color.FromArgb(102, 102, 102);
            scintilla.Styles[SECTION_STYLE].ForeColor = Color.FromArgb(0, 0, 102);
            scintilla.Styles[SECTION_STYLE].Bold = true;
        }


        private int Read()
        {
            if (_index < _text.Length)
                return _text[_index];

            return EOL;
        }


        private void SetStyle(int style, int length)
        {
            if (length > 0)
            {
                // TODO Still using old API
                // This will style the _length of chars and advance the style pointer.
                ((INativeScintilla)_scintilla).SetStyling(length, style);
            }
        }


        public void Style()
        {
            // TODO Still using the old API
            // Signals that we're going to begin styling from this point.
            ((INativeScintilla)_scintilla).StartStyling(_startPos, 0x1F);

            // Run our humble lexer...
            StyleWhitespace();
            switch(Read())
            {
                case '[':

                    // Section, default, comment
                    StyleUntilMatch(SECTION_STYLE, new char[] { ']' });
                    StyleCh(SECTION_STYLE);
                    StyleUntilMatch(DEFAULT_STYLE, new char[] { ';' });
                    goto case ';';
                
                case ';':

                    // Comment
                    SetStyle(COMMENT_STYLE, _text.Length - _index);
                    break;
                
                default:

                    // Key, assignment, quote, value, comment
                    StyleUntilMatch(KEY_STYLE, new char[] { '=', ';' });
                    switch (Read())
                    {
                        case '=':

                            // Assignment, quote, value, comment
                            StyleCh(ASSIGNMENT_STYLE);
                            switch (Read())
                            {
                                case '"':

                                    // Quote
                                    StyleCh(QUOTED_STYLE);  // '"'
                                    StyleUntilMatch(QUOTED_STYLE, new char[] { '"' });
                                    
                                    // Make sure it wasn't an escaped quote
                                    if (_index > 0 && _index < _text.Length && _text[_index - 1] == '\\')
                                        goto case '"';

                                    StyleCh(QUOTED_STYLE); // '"'
                                    goto default;

                                default:

                                    // Value, comment
                                    StyleUntilMatch(VALUE_STYLE, new char[] { ';' });
                                    SetStyle(COMMENT_STYLE, _text.Length - _index);
                                    break;
                            }
                            break;

                        default: // ';', EOL

                            // Comment
                            SetStyle(COMMENT_STYLE, _text.Length - _index);
                            break;
                    }
                    break;
            }
        }


        private void StyleCh(int style)
        {
            // Style just one char and advance
            SetStyle(style, 1);
            _index++;
        }


        public static void StyleNeeded(Scintilla scintilla, Range range)
        {
            // Create an instance of our lexer and bada-bing the line!
            IniLexer lexer = new IniLexer(scintilla, range.Start, range.StartingLine.Length);
            lexer.Style();
        }


        private void StyleUntilMatch(int style, char[] chars)
        {
            // Advance until we match a char in the array
            int startIndex = _index;
            while (_index < _text.Length && Array.IndexOf<char>(chars, _text[_index]) < 0)
                _index++;

            if (startIndex != _index)
                SetStyle(style, _index - startIndex);
        }


        private void StyleWhitespace()
        {
            // Advance the _index until non-whitespace character
            int startIndex = _index;
            while (_index < _text.Length && Char.IsWhiteSpace(_text[_index]))
                _index++;

            SetStyle(DEFAULT_STYLE, _index - startIndex);
        }

        #endregion Methods


        #region Constructors

        private IniLexer(Scintilla scintilla, int startPos, int length)
        {
            this._scintilla = scintilla;
            this._startPos = startPos;

            // One line of _text
            this._text = scintilla.GetRange(startPos, startPos + length).Text;
        }

        #endregion Constructors
    }
}
