using System;
using System.IO;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using SPythonParserYacc;
using PascalABCCompiler.Errors;



namespace SPythonParser
{
    public class SPythonGPPGParserHelper
    {
        private List<Error> Errs;
        private List<CompilerWarning> Warnings;
        private string FileName;
        public bool build_tree_for_formatter = false;
        public List<string> DefinesList = null;

        public static Func<bool> CheckIfParsingUnit;

        public SPythonGPPGParserHelper(List<Error> Errs, List<CompilerWarning> Warnings, string FileName)
        {
            this.Errs = Errs;
            this.Warnings = Warnings;
            this.FileName = FileName;
        }

        public syntax_tree_node Parse(string Text, List<compiler_directive> compilerDirectives = null)
        {
#if DEBUG
#if _ERR
            FileInfo f = new FileInfo(FileName);
            var sv = Path.ChangeExtension(FileName,".grmtrack1");
            var sw = new StreamWriter(sv);
            Console.SetError(sw);
#endif
#endif
            SPythonParserTools parsertools = new SPythonParserTools(); // контекст сканера и парсера
            parsertools.errors = Errs;
            parsertools.warnings = Warnings;
            parsertools.compilerDirectives = compilerDirectives;
            parsertools.CurrentFileName = Path.GetFullPath(FileName);
            

            IndentArranger ia = new IndentArranger();
            ia.ProcessSourceText(ref Text);

            var scanner = new Scanner();
            scanner.SetSource(Text, 0);
            scanner.parsertools = parsertools;// передали parsertools в объект сканера
            if (DefinesList != null)
                scanner.Defines.AddRange(DefinesList);

            SPythonGPPGParser parser = new SPythonGPPGParser(scanner);
            parsertools.build_tree_for_formatter = build_tree_for_formatter;

            parser.is_unit_to_be_parsed = CheckIfParsingUnit.Invoke();

            parser.parsertools = parsertools; // передали parsertools в объект парсера

            if (!parser.Parse())
                if (Errs.Count == 0)
                    parsertools.AddErrorFromResource("UNEXPECTED_SYNTAX_ERROR", null);
#if DEBUG
#if _ERR
            sw.Close();
#endif
#endif
            return parser.root;
        }
    }

    public class SPythonLanguageParser : BaseParser
    {
        private SPythonGPPGParserHelper localparserhelper;

        public SPythonLanguageParser()
        {
            SPythonGPPGParserHelper.CheckIfParsingUnit = CallCheckIfParsingUnit;
        }

        public bool CallCheckIfParsingUnit()
        {
            return CheckIfParsingUnit.Invoke();
        }

        public override void Reset()
        {
            CompilerDirectives = new List<compiler_directive>();

            Errors.Clear();
        }

        protected override void PreBuildTree(string FileName)
        {
            CompilerDirectives = new List<compiler_directive>();

            // чтобы строки во всех программах индексировались с нуля | Movchan 18.05.24
            CompilerDirectives.Add(new compiler_directive(new token_info("zerobasedstrings"), new token_info("")));

        }

        protected override syntax_tree_node BuildTreeInNormalMode(string FileName, string Text, List<string> DefinesList = null)
        {
            Errors.Clear();
            Warnings.Clear();

            localparserhelper = new SPythonGPPGParserHelper(Errors, Warnings, FileName);
            localparserhelper.DefinesList = DefinesList;
            syntax_tree_node root = localparserhelper.Parse(Text);

            if (Errors.Count > 0)
                return null;

            if (root != null && root is compilation_unit)
                (root as compilation_unit).file_name = FileName;

            return root;
        }

        protected override syntax_tree_node BuildTreeInTypeExprMode(string FileName, string Text)
        {
            return null;
        }

        protected override syntax_tree_node BuildTreeInExprMode(string FileName, string Text)
        {
            throw null;
        }

        protected override syntax_tree_node BuildTreeInSpecialMode(string FileName, string Text)
        {
            throw null;
        }

        protected override syntax_tree_node BuildTreeInFormatterMode(string FileName, string Text)
        {
            throw null;
        }

        protected override syntax_tree_node BuildTreeInStatementMode(string FileName, string Text)
        {
            return null;
        }
    }
}
