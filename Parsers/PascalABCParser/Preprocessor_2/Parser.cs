using System;
using System.IO;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.SyntaxTree;
using System.Resources;
using System.Reflection;
using PascalABCCompiler.Parsers;
using System.Collections.Generic;
using PascalABCCompiler.Errors;


namespace PascalABCCompiler.Preprocessor2
{
	
	public class PascalPreprocessor2LanguageParser:IParser
	{
        public GPB_PABCPreprocessor2 parser;

        public PascalPreprocessor2LanguageParser()
		{         
            this.filesExtensions = new string[1];
            this.filesExtensions[0] = ".paspr2" + Parsers.Controller.HideParserExtensionPostfixChar;
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

        ILanguageInformation lnginf = new DefaultLanguageInformation();
        public ILanguageInformation LanguageInformation {
			get {
                return lnginf;
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
		
        private Stream grammar_stream = null;
        public void Reset()
        {
            grammar_stream = CGTResourceExtractor.Extract(new ResourceManager("PascalABCParser.Preprocessor_2.PABCPreprocessor2Lang", Assembly.GetExecutingAssembly()), "PABCPreprocessor2Language");
        	parser = new GPB_PABCPreprocessor2(grammar_stream,null);
                        
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

        public syntax_tree_node BuildTree(string FileName, string Text, ParseMode parseMode)
        {
            if (this.parser == null)
                Reset();
            GPB_PABCPreprocessor2 parser = new GPB_PABCPreprocessor2(this.grammar_stream, this.parser.LanguageGrammar, this.parser.prepr);
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
                return "PABCPreprocessor2";
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
                return "(c) Зарубин М.А. 2009";
            }
        }
        public override string ToString()
        {
            return "PABCPreprocessor2 v0.1";
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
