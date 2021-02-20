#region Using Directives

using System;
using System.Collections;
using System.Collections.Generic;

#endregion Using Directives


namespace ScintillaNET
{
    public class KeywordCollection : TopLevelHelper
    {
        #region Fields

        private string[] _keywords = new string[] { "", "", "", "", "", "", "", "", "" };
        private Dictionary<string, Lexer> _lexerAliasMap;
        private Dictionary<string, string[]> _lexerKeywordListMap;
        private Dictionary<string, Dictionary<string, int>> _lexerStyleMap;

        #endregion Fields


        #region Methods

        private int GetKeyowrdSetIndex(string name)
        {
            string lexerName = Scintilla.Lexing.Lexer.ToString().ToLower();
            if (_lexerKeywordListMap.ContainsKey(lexerName))
                throw new ApplicationException("lexer " + lexerName + " does not support named keyword lists");

            int index = ((IList)_lexerKeywordListMap[lexerName]).IndexOf(name);

            if (index < 0)
                throw new ArgumentException("Keyword Set name does not exist for lexer " + lexerName, "keywordSetName");

            return index;
        }

        #endregion Methods


        #region Indexers

        public string this[int keywordSet]
        {
            get
            {
                return _keywords[keywordSet];
            }
            set
            {
                _keywords[keywordSet] = value;
                NativeScintilla.SetKeywords(keywordSet, value);
            }
        }


        public string this[string keywordSetName]
        {
            get
            {
                return this[GetKeyowrdSetIndex(keywordSetName)];
            }
            set
            {
                this[GetKeyowrdSetIndex(keywordSetName)] = value;
            }
        }

        #endregion Indexers


        #region Constructors

        internal KeywordCollection(Scintilla scintilla) : base(scintilla)
        {
            // Auugh, this plagued me for a while. Each of the lexers cna define their own "Name"
            // and also asign themsleves to a Scintilla Lexer Constant. Most of the time these
            // match the defined constant, but sometimes they don't. We'll always use the constant
            // name since it's easier to use, consistent and will always have valid characters.
            // However its still valid to access the lexers by this name (as SetLexerLanguage
            // uses this value) so we'll create a lookup.
            _lexerAliasMap = new Dictionary<string, Lexer>(StringComparer.OrdinalIgnoreCase);

            // I have no idea how Progress fits into this. It's defined with the PS lexer const
            // and a name of "progress"

            _lexerAliasMap.Add("PL/M", Lexer.Plm);
            _lexerAliasMap.Add("props", Lexer.Properties);
            _lexerAliasMap.Add("inno", Lexer.InnoSetup);
            _lexerAliasMap.Add("clarion", Lexer.Clw);
            _lexerAliasMap.Add("clarionnocase", Lexer.ClwNoCase);


            //_lexerKeywordListMap = new Dictionary<string,string[]>(StringComparer.OrdinalIgnoreCase);


            //_lexerKeywordListMap.Add("xml", new string[] { "HTML elements and attributes", "JavaScript keywords", "VBScript keywords", "Python keywords", "PHP keywords", "SGML and DTD keywords" });
            //_lexerKeywordListMap.Add("yaml", new string[] { "Keywords" });
            // baan, kix, ave, scriptol, diff, props, makefile, errorlist, latex, null, lot, haskell
            // lexers don't have keyword list names

        }

        #endregion Constructors
    }
}
