using System;
using System.IO;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using System.Resources;
using System.Reflection;
using PascalABCCompiler.Errors;
using System.Collections.Generic;


namespace PascalABCCompiler.PL0Parser
{
	
	public class PL0LanguageParser:IParser
	{
		GPBParser_PL0 parser;
        
        public PL0LanguageParser()
		{
            filesExtensions = new string[1];
            filesExtensions[0] = ".pl0";
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
        public ILanguageInformation LanguageInformation {
			get {
        		if (languageInformation == null)
        		languageInformation = new DefaultLanguageInformation(this);
        		return languageInformation;
			}
		}
        
        Keyword[] _keywords;
        void initKeywords()
        {
            List<Keyword> key_words = new List<Keyword>();
            key_words.Add(new Keyword("CONST"));
            key_words.Add(new Keyword("VAR", KeywordKind.Var));
            key_words.Add(new Keyword("CALL"));
            key_words.Add(new Keyword("BEGIN"));
            key_words.Add(new Keyword("END")); 
            key_words.Add(new Keyword("IF"));
            key_words.Add(new Keyword("THEN"));
            key_words.Add(new Keyword("WHILE"));
            key_words.Add(new Keyword("DO"));
            key_words.Add(new Keyword("PROCEDURE", KeywordKind.Function));
            key_words.Add(new Keyword("ODD"));
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
            parser = new GPBParser_PL0(CGTResourceExtractor.Extract(new ResourceManager("PL0Parser.PL0Lang", Assembly.GetExecutingAssembly()), "PL0Language"));
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
                (cu as compilation_unit).compiler_directives = CompilerDirectives;
            }
            return cu;
        }

        public string Name
        {
            get
            {
                return "PL/0";
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
                return "(c) Ткачук А.В., 2007";
            }
        }

        public override string ToString()
        {
            return "PL/0 Language Parser v1.0";
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
