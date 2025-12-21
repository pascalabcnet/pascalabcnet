using System.IO;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using SPythonParserYacc;
using PascalABCCompiler.SyntaxTreeConverters;



namespace SPythonParser
{

    public class SPythonLanguageParser : BaseParser
    {
        public List<ISyntaxTreeConverter> SyntaxTreeConvertersForIntellisense { get; set; }

        public override void Reset()
        {
            CompilerDirectives = new List<compiler_directive>();

            Errors.Clear();
        }

        protected override void PreBuildTree(string FileName)
        {
            CompilerDirectives = new List<compiler_directive>();

        }

        protected override syntax_tree_node BuildTreeInNormalMode(string FileName, string Text, bool compilingNotMainProgram, List<string> DefinesList = null)
        {
            Errors.Clear();
            Warnings.Clear();

            syntax_tree_node root = Parse(Text, FileName, false, compilingNotMainProgram, DefinesList);

            if (Errors.Count > 0)
                return null;

            if (root != null && root is compilation_unit)
                (root as compilation_unit).file_name = FileName;

            return root;
        }

        public syntax_tree_node Parse(string Text, string fileName, bool buildTreeForFormatter = false, bool compilingNotMainProgram = false, List<string> definesList = null)
        {
#if DEBUG
#if _ERR
            FileInfo f = new FileInfo(FileName);
            var sv = Path.ChangeExtension(FileName,".grmtrack1");
            var sw = new StreamWriter(sv);
            Console.SetError(sw);
#endif
#endif
            SPythonParserTools parserTools = new SPythonParserTools(Errors, Warnings, LanguageInformation.ValidDirectives, buildTreeForFormatter, false,
                Path.GetFullPath(fileName), CompilerDirectives); // контекст сканера и парсера


            IndentArranger ia = new IndentArranger();
            ia.ProcessSourceText(ref Text);

            var scanner = new Scanner(Text, parserTools, LanguageInformation.KeywordsStorage, definesList);

            SPythonGPPGParser parser = new SPythonGPPGParser(scanner, parserTools, compilingNotMainProgram);

            if (!parser.Parse())
                if (Errors.Count == 0)
                    parserTools.AddErrorFromResource("UNEXPECTED_SYNTAX_ERROR", null);
#if DEBUG
#if _ERR
            sw.Close();
#endif
#endif
            return parser.root;
        }

        protected override syntax_tree_node BuildTreeInTypeExprMode(string FileName, string Text)
        {
            Text = string.Concat("<<type>>", Text);

            var expr = Parse(Text, FileName) as expression;

            if (expr == null)
                return null;

            foreach (ISyntaxTreeConverter converter in SyntaxTreeConvertersForIntellisense)
            {
                expr = (expression)converter.Convert(expr, true);
            }

            return expr;
        }

        protected override syntax_tree_node BuildTreeInExprMode(string FileName, string Text)
        {
            if (Text == string.Empty)
                return null;
            
            Text = string.Concat("<<expression>>", Text);

            var expr = Parse(Text, FileName) as expression;

            if (expr == null)
                return null;

            foreach (ISyntaxTreeConverter converter in SyntaxTreeConvertersForIntellisense)
            {
                expr = (expression)converter.Convert(expr, true);
            }
            
            return expr;
        }

        protected override syntax_tree_node BuildTreeInSpecialMode(string FileName, string Text, bool compilingNotMainProgram)
        {
            Errors.Clear();
            
            return Parse(Text, FileName, compilingNotMainProgram);
        }

        protected override syntax_tree_node BuildTreeInFormatterMode(string FileName, string Text)
        {
            Errors.Clear();
            
            return Parse(Text, FileName, true);
        }

        protected override syntax_tree_node BuildTreeInStatementMode(string FileName, string Text)
        {
            Text = string.Concat("<<statement>>", Text);

            var st = Parse(Text, FileName) as statement;

            if (st == null)
                return null;

            foreach (ISyntaxTreeConverter converter in SyntaxTreeConvertersForIntellisense)
            {
                st = (statement)converter.Convert(st, true);
            }

            return st;
        }
    }
}
