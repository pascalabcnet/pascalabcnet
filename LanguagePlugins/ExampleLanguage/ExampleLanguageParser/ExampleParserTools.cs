using PascalABCCompiler.Errors;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.ParserTools.Directives;
using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;

namespace Languages.Example.Frontend.Core
{
    internal class ExampleParserTools : BaseParserTools
    {
        public override Dictionary<string, string> TokenNum { get; protected set; }

        private void InitializeTokenNum()
        {
            TokenNum = new Dictionary<string, string>();
        }

        public ExampleParserTools(List<Error> errors, List<CompilerWarning> warnings,
            Dictionary<string, DirectiveInfo> validDirectives, bool buildTreeForFormatter = false, bool buildTreeForFormatterStrings = false,
            string currentFileName = null, List<compiler_directive> compilerDirectives = null)
            : base(errors, warnings, validDirectives, buildTreeForFormatter, buildTreeForFormatterStrings, currentFileName, compilerDirectives)
        {
            InitializeTokenNum();
        }

        protected override string GetFromStringResources(string id)
        {
            return BaseStringResources.Get(id, "EXAMPLEPARSER_");
        }

        // методы ниже не понадобятся для минимальной реализации

        public override literal create_string_const(string text, SourceContext sc)
        {
            throw new NotImplementedException();
        }

        protected override string ExtractDirectiveTextWithoutSpecialSymbols(string directive)
        {
            throw new NotImplementedException();
        }

        protected override string ReplaceSpecialSymbols(string text)
        {
            throw new NotImplementedException();
        }
    }
}
