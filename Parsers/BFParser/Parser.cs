using System;
using System.IO;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using System.Resources;
using System.Reflection;
using System.Collections.Generic;
using PascalABCCompiler.Errors;



namespace PascalABCCompiler.BFParser
{
	
	public class BFLanguageParser: IParser
	{
		private GPBParser_BF parser;

        public BFLanguageParser()
		{
            filesExtensions = new string[1];
            filesExtensions[0] = ".bf";
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

        public string Name
        {
            get
            {
                return "BF";
            }
        }
        public string Version
        {
            get
            {
                return "1.1";
            }
        }
        public  string Copyright
        {
            get
            {
                return "(c) Ткачук А.В. 2006";
            }
        }
        public void Reset()
        {
            parser = new GPBParser_BF(CGTResourceExtractor.Extract(new ResourceManager("BFParser.BFLang", Assembly.GetExecutingAssembly()), "BFLanguage"));
            //parser.Parse("+[.,]");
            parser.max_errors = 5;
        }
		
        public syntax_tree_node BuildTree(string FileName, string Text,string[] SearchPatchs, ParseMode ParseMode)
        {
            if (parser == null)
                Reset();
            parser.errors = Errors;
            parser.current_file_name = FileName;
            switch (ParseMode)
            {
                case ParseMode.Expression:
                case ParseMode.Statement:
                    return null;
            }
            syntax_tree_node cu = (syntax_tree_node)parser.Parse(Text);
            if (cu != null && cu is compilation_unit)
                (cu as compilation_unit).file_name = FileName;
            return cu;
        }
        
        public override string ToString()
        {
            return "BF Language Parser v1.1";
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
