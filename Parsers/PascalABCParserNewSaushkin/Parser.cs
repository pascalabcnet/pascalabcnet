//#define _ERR

using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCSavParser;
using PascalABCCompiler.Parsers;
using GPPGParserScanner;
using PascalABCCompiler.ParserTools;
using GPPGPreprocessor3;

namespace PascalABCCompiler.PascalABCNewParser
{
    // SSM: класс, являющийся обёрткой над GPPG парсером
    public class GPPGParserHelper
    {
        private List<Errors.Error> Errs;
        private List<Errors.CompilerWarning> Warnings;
        private string FileName;
        public bool build_tree_for_formatter = false;
        public List<string> DefinesList = null;

        public GPPGParserHelper(List<Errors.Error> Errs, List<Errors.CompilerWarning> Warnings, string FileName)
        {
            this.Errs = Errs;
            this.Warnings = Warnings;
            this.FileName = FileName;
        }

        public syntax_tree_node Parse(string Text, List<compiler_directive> compilerDirectives=null)
        {
#if DEBUG
#if _ERR
            FileInfo f = new FileInfo(FileName);
            var sv = Path.ChangeExtension(FileName,".grmtrack1");
            var sw = new StreamWriter(sv);
            Console.SetError(sw);
#endif
#endif
            PT parsertools = new PT(); // контекст сканера и парсера
            parsertools.errors = Errs;
            parsertools.warnings = Warnings;
            parsertools.compilerDirectives = compilerDirectives;
            parsertools.CurrentFileName = Path.GetFullPath(FileName);


            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            scanner.parsertools = parsertools;// передали parsertools в объект сканера
            if (DefinesList != null)
                scanner.Defines.AddRange(DefinesList);
            GPPGParser parser = new GPPGParser(scanner);
            parsertools.build_tree_for_formatter = build_tree_for_formatter;
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

    // SSM: класс, являющийся обёрткой над парсером препроцессора
    public class PreprocessorParserHelper
    {
        private List<Errors.Error> Errs;
        private string FileName;
        public List<compiler_directive> compilerDirectives;

        public PreprocessorParserHelper(List<Errors.Error> Errs, string FileName)
        {
            this.Errs = Errs;
            this.FileName = FileName;
        }

        public bool Parse(string Text)
        {
            compilerDirectives = new List<compiler_directive>();

            PT parsertools = new PT(); // контекст сканера и парсера
            parsertools.errors = Errs;
            parsertools.CurrentFileName = Path.GetFullPath(FileName);

            var scanner = new PreprocessorScanner();
            scanner.SetSource(Text, 0);
            //scanner.parsertools = parsertools;// передали parsertools в объект сканера

            var parser = new PreprocessorParser(scanner);
            parser.compilerDirectives = compilerDirectives;
            parsertools.compilerDirectives = compilerDirectives;
            //parser.parsertools = parsertools; // передали parsertools в объект парсера

            if (!parser.Parse())
            {
                parsertools.AddError("Неопознанная синтаксическая ошибка препроцессора!", null);
                return false;
            }
            return true;
        }
    }

    // SSM: Наш основной парсер + препроцессор (реализует НАШ интерфейс IParser)
    public class PascalABCNewLanguageParser : BaseParser, IParser
    {
        private GPPGParserHelper localparserhelper;
        //public Preprocessor2.Preprocessor2 preprocessor2 = new PascalABCCompiler.Preprocessor2.Preprocessor2(null);

        public PascalABCNewLanguageParser()
            : base("PascalABC.NET", "1.2", "Copyright © 2005-2024 by Ivan Bondarev, Stanislav Mikhalkovich", false, new string[] { ".pas" }, new string[] { "PABCSystem", "PABCExtensions" })
        {
        }

        public override void Reset()
        {
            CompilerDirectives = new List<compiler_directive>();
            Errors.Clear();
        }

        public override SourceFilesProviderDelegate SourceFilesProvider
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
            syntax_tree_node root = null;

            PreBuildTree(FileName);
            switch (ParseMode)
            {
                case ParseMode.Normal:
                    root = BuildTreeInNormalMode(FileName, Text, DefinesList);
                    break;
                case ParseMode.Expression:
                    root = BuildTreeInExprMode(FileName, Text);
                    break;
                case ParseMode.TypeAsExpression:
                    root = BuildTreeInTypeExprMode(FileName, Text);
                    break;
                case ParseMode.Special:
                    root = BuildTreeInSpecialMode(FileName, Text);
                    break;
                case ParseMode.ForFormatter:
                    root = BuildTreeInFormatterMode(FileName, Text);
                    break;
                case ParseMode.Statement:
                    root = BuildTreeInStatementMode(FileName, Text);
                    break;
                default:
                    break;
            }

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
            file_names[0] = file_name;

            preprocessor2.Build(file_names, Errors, null);*/

            PreprocessorParserHelper preprocessor3 = new PreprocessorParserHelper(Errors, FileName);
            var b = preprocessor3.Parse(Text);
            
            if (Errors.Count > 0)
                return null;

            localparserhelper = new GPPGParserHelper(Errors, Warnings, FileName);
            localparserhelper.DefinesList = DefinesList;
            syntax_tree_node root = localparserhelper.Parse(Text, preprocessor3.compilerDirectives);

            if (Errors.Count > 0)
                return null;

            if (root != null && root is compilation_unit)
                (root as compilation_unit).file_name = FileName;

            /*if (preprocessor2.CompilerDirectives != null && preprocessor2.CompilerDirectives.Count != 0)
                CompilerDirectives.AddRange(preprocessor2.CompilerDirectives);*/

            if (preprocessor3.compilerDirectives != null && preprocessor3.compilerDirectives.Count != 0)
                CompilerDirectives.AddRange(preprocessor3.compilerDirectives);

            return root;
        }

        public override syntax_tree_node BuildTreeInExprMode(string FileName, string Text)
        {
            if (Text == string.Empty)
                return null;
                // LineCorrection = -1 не забыть
            string origText = Text;
            Text = String.Concat("<<expression>>", Environment.NewLine, Text);
            localparserhelper = new GPPGParserHelper(Errors, Warnings, FileName);
            // localparser.parsertools.LineCorrection = -1;
            syntax_tree_node root = localparserhelper.Parse(Text);
            if (root == null && origText != null && origText.Contains("<"))
            {
                Errors.Clear();
                root = localparserhelper.Parse(String.Concat("<<expression>>", Environment.NewLine, origText.Replace("<","&<")));
            }
            return root as expression;
        }

        public override syntax_tree_node BuildTreeInTypeExprMode(string FileName, string Text)
        {
            // LineCorrection = -1 не забыть
            Text = String.Concat("<<type>>", Environment.NewLine, Text);
            localparserhelper = new GPPGParserHelper(Errors, Warnings, FileName);
            // localparser.parsertools.LineCorrection = -1;
            syntax_tree_node root = localparserhelper.Parse(Text);
            return root as expression;
        }

        public override syntax_tree_node BuildTreeInStatementMode(string FileName, string Text)
        {
            Text = String.Concat("<<statement>>", Environment.NewLine, Text);
            localparserhelper = new GPPGParserHelper(Errors, Warnings, FileName);
            // localparser.parsertools.LineCorrection = -1;
            syntax_tree_node root = localparserhelper.Parse(Text);
            return root as statement;
        }

        public override syntax_tree_node BuildTreeInSpecialMode(string FileName, string Text)
        {
            Errors.Clear();
            localparserhelper = new GPPGParserHelper(Errors, Warnings, FileName);
            syntax_tree_node root = localparserhelper.Parse(Text);
            return root;
        }

        public override syntax_tree_node BuildTreeInFormatterMode(string FileName, string Text)
        {
            Errors.Clear();
            localparserhelper = new GPPGParserHelper(Errors, Warnings, FileName);
            localparserhelper.build_tree_for_formatter = true;
            syntax_tree_node root = localparserhelper.Parse(Text);
            return root;
        }

        // SSM: Зачем это - не знаю. Я бы убрал
        public override IPreprocessor Preprocessor
        {
            get
            {
                return null;
            }
        }
    }
}
