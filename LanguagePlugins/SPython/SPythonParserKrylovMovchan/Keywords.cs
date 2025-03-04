using System.Collections.Generic;
using System.Linq;
using SPythonParserYacc;

namespace SPythonParser
{

    public class SPythonKeywords : PascalABCCompiler.Parsers.BaseKeywords
    {
        protected override Dictionary<string, int> KeywordsToTokens { get; set; }

        protected override string FileName => "keywordsmap.spy";

        public SPythonKeywords() : base()
        {
            KeywordsToTokens = new Dictionary<string, int>
            {
                ["if"] = (int)Tokens.IF,
                ["elif"] = (int)Tokens.ELIF,
                ["else"] = (int)Tokens.ELSE,
                ["while"] = (int)Tokens.WHILE,
                ["for"] = (int)Tokens.FOR,
                ["in"] = (int)Tokens.IN,
                ["def"] = (int)Tokens.DEF,
                ["return"] = (int)Tokens.RETURN,
                ["break"] = (int)Tokens.BREAK,
                ["continue"] = (int)Tokens.CONTINUE,
                ["and"] = (int)Tokens.AND,
                ["or"] = (int)Tokens.OR,
                ["not"] = (int)Tokens.NOT,
                ["import"] = (int)Tokens.IMPORT,
                ["from"] = (int)Tokens.FROM,
                ["global"] = (int)Tokens.GLOBAL,
                ["True"] = (int)Tokens.TRUE,
                ["False"] = (int)Tokens.FALSE,
                ["as"] = (int)Tokens.AS,
                ["pass"] = (int)Tokens.PASS
            }
            .ToDictionary(kv => ConvertKeyword(kv.Key), kv => kv.Value);
        }

        protected override int GetIdToken() => (int)Tokens.ID;
    }

}
