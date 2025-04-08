using PascalABCCompiler.Parsers;
using SPythonParserYacc;

namespace SPythonParser
{

    public class SPythonKeywords : BaseKeywords
    {

        protected override string FileName => "keywordsmap.pys";

        public SPythonKeywords() : base(true)
        {
            CreateNewKeyword("if", Tokens.IF);
            CreateNewKeyword("elif", Tokens.ELIF);
            CreateNewKeyword("else", Tokens.ELSE);
            CreateNewKeyword("while", Tokens.WHILE);
            CreateNewKeyword("for", Tokens.FOR);
            CreateNewKeyword("in", Tokens.IN);
            CreateNewKeyword("def", Tokens.DEF, KeywordKind.Function, true);
            CreateNewKeyword("return", Tokens.RETURN);
            CreateNewKeyword("break", Tokens.BREAK);
            CreateNewKeyword("continue", Tokens.CONTINUE);
            CreateNewKeyword("and", Tokens.AND);
            CreateNewKeyword("or", Tokens.OR);
            CreateNewKeyword("not", Tokens.NOT);
            CreateNewKeyword("import", Tokens.IMPORT, KeywordKind.Uses);
            CreateNewKeyword("from", Tokens.FROM);
            CreateNewKeyword("global", Tokens.GLOBAL);
            CreateNewKeyword("True", Tokens.TRUE);
            CreateNewKeyword("False", Tokens.FALSE);
            CreateNewKeyword("as", Tokens.AS);
            CreateNewKeyword("pass", Tokens.PASS);
            CreateNewKeyword("class", Tokens.CLASS, isTypeKeyword: true);
            CreateNewKeyword("lambda", Tokens.LAMBDA);
            CreateNewKeyword("exit", Tokens.EXIT);
            CreateNewKeyword("new", Tokens.NEW, KeywordKind.New);
            CreateNewKeyword("is", Tokens.IS);
        }

        protected override int GetIdToken() => (int)Tokens.ID;
    }

}
