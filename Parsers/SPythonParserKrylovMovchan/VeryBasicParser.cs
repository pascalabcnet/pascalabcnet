using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.ParserTools;
using VeryBasicParser;
using VeryBasicParserYacc;
using PascalABCCompiler.Errors;



namespace VeryBasicParser
{
    public class VeryBasicGPPGParserHelper
    {
        private List<Error> Errs;
        private List<CompilerWarning> Warnings;
        private string FileName;
        public bool build_tree_for_formatter = false;
        public List<string> DefinesList = null;

        public VeryBasicGPPGParserHelper(List<Error> Errs, List<CompilerWarning> Warnings, string FileName)
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
            VeryBasicParserTools parsertools = new VeryBasicParserTools(); // контекст сканера и парсера
            parsertools.errors = Errs;
            parsertools.warnings = Warnings;
            parsertools.compilerDirectives = compilerDirectives;
            parsertools.CurrentFileName = Path.GetFullPath(FileName);

            bool isUnit = false;
            if (Text.EndsWith("unit"))
            {
                Text = Text.Substring(0, Text.Length - 4);
                isUnit = true;
            }

            var scanner = new Scanner();
            scanner.SetSource(Text, 0);
            scanner.parsertools = parsertools;// передали parsertools в объект сканера
            if (DefinesList != null)
                scanner.Defines.AddRange(DefinesList);

            VeryBasicGPPGParser parser = new VeryBasicGPPGParser(scanner);
            parsertools.build_tree_for_formatter = build_tree_for_formatter;

            parser.is_unit_to_be_parsed = isUnit;

            parser.parsertools = parsertools; // передали parsertools в объект парсера

            if (!parser.Parse())
                if (Errs.Count == 0)
                    parsertools.AddError("Неопознанная синтаксическая ошибка!", null);
#if DEBUG
#if _ERR
            sw.Close();
#endif
#endif
            return parser.root;
        }
    }
    public class VeryBasicLanguageParser: BaseParser, IParser
    {
        private VeryBasicGPPGParserHelper localparserhelper;
        //public Preprocessor2.Preprocessor2 preprocessor2 = new PascalABCCompiler.Preprocessor2.Preprocessor2(null);

        public VeryBasicLanguageParser()
            : base("VeryBasic", "0.0.1", "Copyright © 2023-2023 by Vladislav Krylov, Egor Movchan", new string[] { "SpythonSystem" }, false, new string[] { ".yavb" })
        {
        }

        public override void Reset()
        {
            CompilerDirectives = new List<compiler_directive>();

            Errors.Clear();
        }

        public override PascalABCCompiler.SourceFilesProviderDelegate SourceFilesProvider
        {
            get
            {
                return sourceFilesProvider;
            }
            set
            {
                sourceFilesProvider = value;
                //preprocessor2.SourceFilesProvider = value;
            }
        }

        public override void PreBuildTree(string FileName)
        {
            CompilerDirectives = new List<compiler_directive>();
            //preprocessor2 = new Preprocessor2.Preprocessor2(SourceFilesProvider);
        }

        public override PascalABCCompiler.SyntaxTree.syntax_tree_node BuildTree(string FileName, string Text, ParseMode ParseMode, List<string> DefinesList = null)
        {
            PreBuildTree(FileName);
            syntax_tree_node root = BuildTreeInNormalMode(FileName, Text, DefinesList);

            if (root != null && root is compilation_unit)
            {
                (root as compilation_unit).file_name = FileName;
                (root as compilation_unit).compiler_directives = CompilerDirectives;
                if (root is unit_module)
                    if ((root as unit_module).unit_name.HeaderKeyword == UnitHeaderKeyword.Library)
                        (root as compilation_unit).compiler_directives.Add(new compiler_directive(new token_info("apptype"), new token_info("dll")));
            }

            return root;
        }
        public override syntax_tree_node BuildTreeInNormalMode(string FileName, string Text, List<string> DefinesList = null)
        {
            Errors.Clear();
            Warnings.Clear();
            /*string[] file_names = new string[1];
            file_names[0] = FileName;

            preprocessor2.Build(file_names, Errors, null);*/

            localparserhelper = new VeryBasicGPPGParserHelper(Errors, Warnings, FileName);
            localparserhelper.DefinesList = DefinesList;
            syntax_tree_node root = localparserhelper.Parse(Text);

            if (Errors.Count > 0)
                return null;

            if (root != null && root is compilation_unit)
                (root as compilation_unit).file_name = FileName;

            return root;
        }

    }
}
