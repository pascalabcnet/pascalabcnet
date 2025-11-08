using PascalABCCompiler.Parsers;


namespace Languages.Example.Frontend.Core
{

    public class ExampleKeywords : BaseKeywords
    {

        protected override string FileName => "keywordsmap.example";

        public ExampleKeywords() : base(true)
        {
            CreateNewKeyword("var", Tokens.VAR, KeywordKind.Var);
            
        }

        protected override int GetIdToken() => (int)Tokens.ID;
    }

}

