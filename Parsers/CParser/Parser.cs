using System;
using System.IO;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using System.Resources;
using System.Reflection;
using PascalABCCompiler.Errors;
using System.Collections.Generic;


namespace PascalABCCompiler.CParser
{
	
	public class CLanguageParser:IParser
	{
		private GPBParser_C parser;

        public CLanguageParser()
		{
            filesExtensions = new string[2];
            filesExtensions[0] = ".c";
            filesExtensions[1] = ".h";
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
		
        ILanguageInformation languageInformation;
        public ILanguageInformation LanguageInformation {
			get {
        		if (languageInformation == null)
        		languageInformation = new CLanguageInformation(this);
        		return languageInformation;
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

        public void Reset()
        {
            parser = new GPBParser_C(CGTResourceExtractor.Extract(new ResourceManager("CParser.CLang", Assembly.GetExecutingAssembly()), "CLanguage"));
            //parser.Parse("+[.,]");
            max_errors = 5;
        }
		int max_errors
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


        public syntax_tree_node BuildTree(string FileName, string Text, string[] SearchPatchs, ParseMode ParseMode)
        {
            if (parser == null)
                Reset();
            parser.errors = Errors;
            parser.current_file_name = FileName;
            syntax_tree_node cu = null;
            switch (ParseMode)
            {
                case ParseMode.Expression:
                    Text = String.Concat("<<expression>>", Environment.NewLine, Text);
                    parser.parsertools.LineCorrection = -1;
                    cu = (syntax_tree_node)parser.Parse(Text);
                    return cu as expression;
                case ParseMode.Statement:
                    return null;
            }
            Preprocessor preprocessor = new Preprocessor(SourceFilesProvider);
            string[] file_names = new string[1];
            file_names[0] = FileName;
            SourceContextMap scm = new SourceContextMap();
            Text = preprocessor.Build(file_names, SearchPatchs, Errors, scm);

            if (Text != null)
            {
                StreamWriter sw = File.CreateText(Path.GetDirectoryName(FileName) + "\\" + Path.GetFileNameWithoutExtension(FileName) + "_preprocessor.c");
                sw.Write(Text);
                sw.Close();
            }

            if (Errors.Count > 0)
                return null;
            parser.scm = scm;
            cu = (syntax_tree_node)parser.Parse(Text);
            if (cu != null && cu is compilation_unit)
                (cu as compilation_unit).file_name = FileName;
            return cu;
        }
     

        public  string Name
        {
            get
            {
                return "C";
            }
        }
        public string Version
        {
            get
            {
                return "0.3";
            }
        }
        public string Copyright
        {
            get
            {
                return "(c) Ткачук А.В. 2007";
            }
        }
        public override string ToString()
        {
            return "C Language Parser v0.3";
        }
        public IPreprocessor Preprocessor
        {
            get
            {
                return new Preprocessor(SourceFilesProvider);
            }
        }

	}


}
