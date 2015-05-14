using System;
using System.IO;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using System.Resources;
using System.Reflection;
using System.Collections.Generic;
using PascalABCCompiler.Errors;
using PascalABCCompiler.Preprocessor2;


namespace PascalABCCompiler.PascalABCParser
{

    public class PascalABCLanguageParser : BaseParser, IParser
    {
        private GPBParser_PascalABC parser;
        private GPBParser_PascalABC localparser; // Вынес localparser сюда для передачи между методами. После избавления от GPB должен остаться только этот парсер

        public Preprocessor2.Preprocessor2 preprocessor2 = new PascalABCCompiler.Preprocessor2.Preprocessor2(null);

        private Stream grammar_stream;

        public PascalABCLanguageParser()
            : base("PascalABC.NET", "2.2", "(c) Ткачук А.В. 2005,2008; Михалкович С.С. 2012", false, new string[1]{".gpas"})
        {
            parser = null;
        }

        public override void Reset()
        {
            CompilerDirectives.Clear();
            grammar_stream = CGTResourceExtractor.Extract(new ResourceManager("PascalABCParser.PascalABCLang", Assembly.GetExecutingAssembly()),"PascalABCLanguage");
            parser = new GPBParser_PascalABC(grammar_stream, preprocessor2);
            parser.Parse("begin end.");
            max_errors = 30;
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

        int max_errors
        {
            get
            {
                return parser.max_errors;
            }
            set
            {
                parser.max_errors = value;
            }
        }

        public override void PreBuildTree(string FileName)
        {
            if (this.parser == null)
                Reset();
            CompilerDirectives = new List<compiler_directive>();
            localparser = new GPBParser_PascalABC(this.grammar_stream, this.parser.LanguageGrammar, preprocessor2);
            localparser.CompilerDirectives = CompilerDirectives;
            localparser.errors = Errors;
            localparser.current_file_name = FileName;
            localparser.parsertools.LineCorrection = 0;
            localparser.build_tree_for_brackets = false;
            preprocessor2 = new Preprocessor2.Preprocessor2(SourceFilesProvider);
        }

        public override syntax_tree_node BuildTreeInNormalMode(string FileName, string Text)
        {
            Errors.Clear();

            string[] file_names = new string[1];
            file_names[0] = FileName;
            SourceContextMap scm = new SourceContextMap();
            Text = preprocessor2.Build(file_names, Errors, scm);

            localparser.scm = scm;
            ((localparser.parsertools) as pascalabc_parsertools).scm = scm;

            if (Errors.Count > 0)
                return null;

            syntax_tree_node  root = (syntax_tree_node)localparser.Parse(Text);
            if (root != null && root is compilation_unit)
                (root as compilation_unit).file_name = FileName;
            if (preprocessor2.CompilerDirectives != null && preprocessor2.CompilerDirectives.Count != 0)
                localparser.CompilerDirectives.AddRange(preprocessor2.CompilerDirectives);

            return root;
        }

        public override syntax_tree_node BuildTreeInExprMode(string FileName, string Text)
        {
            Text = String.Concat("<<expression>>", Environment.NewLine, Text);
            localparser.parsertools.LineCorrection = -1;
            syntax_tree_node root = (syntax_tree_node)localparser.Parse(Text);
            return root as expression;
        }

        public override syntax_tree_node BuildTreeInStatementMode(string FileName, string Text)
        {
            Text = String.Concat("<<statement>>", Environment.NewLine, Text);
            localparser.parsertools.LineCorrection = -1;
            syntax_tree_node root = (syntax_tree_node)localparser.Parse(Text);
            return root as statement;
        }

        public override syntax_tree_node BuildTreeInSpecialMode(string FileName, string Text)
        {
            Errors.Clear();
            syntax_tree_node root = (syntax_tree_node)localparser.Parse(Text);
            return root;
        }

        public override syntax_tree_node BuildTreeInFormatterMode(string FileName, string Text)
        {
            localparser.build_tree_for_brackets = true;
            Errors.Clear();
            syntax_tree_node root = (syntax_tree_node)localparser.Parse(Text);
            return root;
        }

        public override string ToString()
        {
            return "PascalABC.NET Language Parser v2.2";
        }
        public override IPreprocessor Preprocessor
        {
            get
            {
                //MZ
                return preprocessor2;
            }
        }

    }



}
