using System;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using System.Resources;
using System.Reflection;
using PascalABCCompiler.Errors;
using System.IO;

namespace PascalABCCompiler.HaskellParser
{
    public class HaskellLanguageParser : IParser
    {
        GPBParser_Haskell parser;

        public HaskellLanguageParser()
        {
            filesExtensions = new string[1];
            filesExtensions[0] = ".hs";
        }

        string[] filesExtensions;
        public string[] FilesExtensions
        {
            get
            {
                return filesExtensions;
            }
        }

        ILanguageInformation languageInformation;
        public ILanguageInformation LanguageInformation
        {
            get
            {
                if (languageInformation == null)
                    languageInformation = new DefaultLanguageInformation(this);
                return languageInformation;
            }
        }

        Keyword[] _keywords;
        void initKeywords()
        {
            List<Keyword> key_words = new List<Keyword>();
            key_words.Add(new Keyword("module"));
            key_words.Add(new Keyword("where"));
            _keywords = key_words.ToArray();
        }
        public Keyword[] Keywords
        {
            get
            {
                return _keywords;
            }
        }

        public void Reset()
        {
            ResourceManager rm = new ResourceManager("HaskellParser.HaskellLang", Assembly.GetExecutingAssembly());
            parser = new GPBParser_Haskell(CGTResourceExtractor.Extract(rm, "HaskellLanguage"));
            parser.max_errors = 30;
        }

        public bool CaseSensitive
        {
            get
            {
                return parser.LanguageGrammar.CaseSensitive;
            }
        }

        SourceFilesProviderDelegate sourceFilesProvider = null;
        public SourceFilesProviderDelegate SourceFilesProvider
        {
            get
            {
                return sourceFilesProvider;
            }
            set
            {
                sourceFilesProvider = value;
            }
        }

        public List<compiler_directive> compilerDirectives = new List<compiler_directive>();
        public List<compiler_directive> CompilerDirectives
        {
            get
            {
                return new List<compiler_directive>();
            }
        }

        List<Error> errors = new List<Error>();
        public List<Error> Errors
        {
            get
            {
                return errors;
            }
            set
            {
                errors = value;
            }
        }
        public syntax_tree_node BuildTree(string FileName, string Text, string[] SearchPatchs, ParseMode ParseMode)
        {
            if (parser == null)
                Reset();
            compilerDirectives = new System.Collections.Generic.List<compiler_directive>();
            parser.CompilerDirectives = compilerDirectives;
            parser.errors = Errors;
            parser.current_file_name = FileName;
            parser.parsertools.LineCorrection = 0;
            syntax_tree_node cu = null;
            switch (ParseMode)
            {
                case ParseMode.Normal:
                    cu = (syntax_tree_node)parser.Parse(Text);
                    break;
                case ParseMode.Expression:
                case ParseMode.Statement:
                    return null;
            }
            if (cu != null && cu is compilation_unit)
            {
                (cu as compilation_unit).file_name = FileName;
                (cu as compilation_unit).compiler_directives = parser.CompilerDirectives;
            }
            return cu;
        }

        public string Name
        {
            get
            {
                return "Haskell";
            }
        }
        public string Version
        {
            get
            {
                return "1.0";
            }
        }
        public string Copyright
        {
            get
            {
                return "(c) Бушманова Н. 2007";
            }
        }

        public override string ToString()
        {
            return "Haskell Language Parser v1.0";
        }
        public IPreprocessor Preprocessor
        {
            get
            {
                return null;
            }
        }

    }
}
