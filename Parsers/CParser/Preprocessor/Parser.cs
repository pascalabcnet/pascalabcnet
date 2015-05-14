using System;
using System.IO;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.SyntaxTree;

using System.Resources;
using System.Reflection;
using PascalABCCompiler.Parsers;
using System.Collections.Generic;
using PascalABCCompiler.Errors;


namespace PascalABCCompiler.CPreprocessor
{
	
	public class CPreprocessorLanguageParser:IParser
	{
		private GPBPreprocessor_C parser;

        public CPreprocessorLanguageParser()
		{
            this.filesExtensions = new string[1];
            this.filesExtensions[0] = ".cpr" + Parsers.Controller.HideParserExtensionPostfixChar;
            parser = null;
		}

        string[] filesExtensions;
        public string[] FilesExtensions
        {
            get
            {
                return filesExtensions;
            }
        }
		
        public ILanguageInformation LanguageInformation {
			get {
				return null;
			}
		}
        
        public bool CaseSensitive
        {
            get
            {
                return parser.LanguageGrammar.CaseSensitive;
            }
        }

        public Keyword[] Keywords
        {
            get
            {
                return new Keyword[0];
            }
        }

        public List<compiler_directive> compilerDirectives = new List<compiler_directive>();
        public List<compiler_directive> CompilerDirectives
        {
            get
            {
                return compilerDirectives;
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

        public void Reset()
        {
            parser = new GPBPreprocessor_C(CGTResourceExtractor.Extract(new ResourceManager("CParser.Preprocessor.CPreprocessorLang", Assembly.GetExecutingAssembly()), "CPreprocessorLanguage"));
            //parser.Parse("+[.,]");
            max_errors = 5;
        }
		public virtual int max_errors
		{
			get
			{
				return parser.max_errors;
			}
			set
			{
				parser.max_errors=value;
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

        public syntax_tree_node BuildTree(string FileName, string Text, string[] SearchPatchs, ParseMode parseMode)
        {
            if (parser == null)
                Reset();
            parser.errors = Errors;
            parser.current_file_name = FileName;
            compilerDirectives = new List<compiler_directive>();
            parser.CompilerDirectives = compilerDirectives;
            switch (parseMode)
            {
                case ParseMode.Expression:
                case ParseMode.Statement:
                    return null;
            }
            syntax_tree_node cu = (syntax_tree_node)parser.Parse(Text);
            if (cu != null && cu is compilation_unit)
            {
                (cu as compilation_unit).file_name = FileName;
                (cu as compilation_unit).compiler_directives = compilerDirectives;
            }
            return cu;
        }

        public  string Name
        {
            get
            {
                return "C Preprocessor";
            }
        }
        public  string Version
        {
            get
            {
                return "0.1";
            }
        }
        public  string Copyright
        {
            get
            {
                return "(c) Ткачук А.В. 2007";
            }
        }
        public override string ToString()
        {
            return "C Preprocessor v0.1";
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
