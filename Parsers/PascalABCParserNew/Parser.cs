using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCNewParser;
using PascalABCCompiler.Parsers;
using GPPGParserScanner;
using PascalABCCompiler.ParserTools;

namespace PascalABCCompiler.PascalABCNewParser
{
    // SSM: класс, 
    public class GPPGParserHelper
    {
        private List<Errors.Error> Errs;
        private string FileName;
        public SourceContextMap scm;
        public bool build_tree_for_brackets = false;

        public GPPGParserHelper(List<Errors.Error> Errs, string FileName)
        {
            this.Errs = Errs;
            this.FileName = FileName;
        }

        public syntax_tree_node Parse(string Text)
        {
            PT parsertools = new PT();
            parsertools.errors = Errs;
            parsertools.CurrentFileName = FileName;

            Scanner scanner = new Scanner();
            scanner.SetSource(Text, 0);
            scanner.parsertools = parsertools;

            GPPGParser parser = new GPPGParser(scanner);
            parser.parsertools = parsertools; // передали parsertools в объект парсера

            if (!parser.Parse())
                if (Errs.Count == 0)
                    parsertools.AddError("Неопознанная синтаксическая ошибка!", null);
            return parser.root;
        }

    }

    public class PascalABCNewLanguageParser : BaseParser, IParser
    {
        private GPPGParserHelper localparserhelper;
        public Preprocessor2.Preprocessor2 preprocessor2 = new PascalABCCompiler.Preprocessor2.Preprocessor2(null);

        public PascalABCNewLanguageParser()
            : base("PascalABCNewParser", "1.0", "(c) Mikhalkovich S.S., 2012", false, new string[] { ".dpas" })
        {
        }

        public override void Reset()
        {
            CompilerDirectives.Clear();
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
                preprocessor2.SourceFilesProvider = value;
            }
        }

        public override void PreBuildTree(string FileName)
        {
            CompilerDirectives.Clear();
            preprocessor2 = new Preprocessor2.Preprocessor2(SourceFilesProvider);
            /*localparserhelper = new GPBParser_PascalABC(this.grammar_stream, this.parser.LanguageGrammar, preprocessor2);
            localparserhelper.CompilerDirectives = CompilerDirectives;
            localparserhelper.errors = Errors;
            localparserhelper.current_file_name = FileName;
            localparserhelper.parsertools.LineCorrection = 0;
            localparserhelper.build_tree_for_brackets = false;*/
        }

        public override syntax_tree_node BuildTreeInNormalMode(string FileName, string Text)
        {
            Errors.Clear();

            string[] file_names = new string[1];
            file_names[0] = FileName;
            SourceContextMap scm = new SourceContextMap();
            //Text = 
                preprocessor2.Build(file_names, Errors, scm);

            localparserhelper = new GPPGParserHelper(Errors, FileName);

            localparserhelper.scm = scm;
            //((GPPGhelper.parsertools) as pascalabc_parsertools).scm = scm;

            if (Errors.Count > 0)
                return null;

            syntax_tree_node root = localparserhelper.Parse(Text);
            if (root != null && root is compilation_unit)
                (root as compilation_unit).file_name = FileName;
            if (preprocessor2.CompilerDirectives != null && preprocessor2.CompilerDirectives.Count != 0)
                /*localparserhelper.*/CompilerDirectives.AddRange(preprocessor2.CompilerDirectives);

            return root;
        }

        public override syntax_tree_node BuildTreeInExprMode(string FileName, string Text)
        {
            // LineCorrection = -1 не забыть
            Text = String.Concat("<<expression>>", Environment.NewLine, Text);
            localparserhelper = new GPPGParserHelper(Errors, FileName);
            // localparser.parsertools.LineCorrection = -1;
            syntax_tree_node root = localparserhelper.Parse(Text);
            return root as expression;
        }

        public override syntax_tree_node BuildTreeInStatementMode(string FileName, string Text)
        {
            Text = String.Concat("<<statement>>", Environment.NewLine, Text);
            localparserhelper = new GPPGParserHelper(Errors, FileName);
            // localparser.parsertools.LineCorrection = -1;
            syntax_tree_node root = localparserhelper.Parse(Text);
            return root as expression;
        }

        public override syntax_tree_node BuildTreeInSpecialMode(string FileName, string Text)
        {
            Errors.Clear();
            localparserhelper = new GPPGParserHelper(Errors, FileName);
            syntax_tree_node root = localparserhelper.Parse(Text);
            return root;
        }

        public override syntax_tree_node BuildTreeInFormatterMode(string FileName, string Text)
        {
            Errors.Clear();
            localparserhelper = new GPPGParserHelper(Errors, FileName);
            localparserhelper.build_tree_for_brackets = true;
            syntax_tree_node root = localparserhelper.Parse(Text);
            return root;
        }

        public override IPreprocessor Preprocessor
        {
            get
            {
                return preprocessor2;
            }
        }
    }
}
