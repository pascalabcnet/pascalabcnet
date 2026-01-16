using PascalABCCompiler.Parsers;
using PascalABCCompiler.SyntaxTree;
using System.Collections.Generic;
using System.IO;
using Languages.Example.Frontend.Core;

namespace Languages.Example.Frontend.Wrapping
{
    public class ExampleLanguageParser : SimpleParser
    {

        protected override syntax_tree_node BuildTreeInNormalMode(string FileName, string Text, bool compilingNotMainProgram, List<string> DefinesList = null)
        {
            syntax_tree_node root = Parse(Text, FileName, false, compilingNotMainProgram, DefinesList);

            if (Errors.Count > 0)
                return null;

            return root;
        }

        public syntax_tree_node Parse(string Text, string fileName, bool buildTreeForFormatter = false, bool compilingNotMainProgram = false, List<string> definesList = null)
        {

            ExampleParserTools parserTools = new ExampleParserTools(Errors, Warnings, LanguageInformation.ValidDirectives, buildTreeForFormatter, false,
                Path.GetFullPath(fileName), CompilerDirectives); // контекст сканера и парсера

            var scanner = new Scanner(Text, parserTools, LanguageInformation.KeywordsStorage, definesList);

            ExampleGPPGParser parser = new ExampleGPPGParser(scanner, parserTools);

            if (!parser.Parse())
                if (Errors.Count == 0)
                    parserTools.AddErrorFromResource("UNEXPECTED_SYNTAX_ERROR", null);

            return parser.root;
        }
    }
}
