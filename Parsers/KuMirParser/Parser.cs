using System;
using System.IO;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using System.Resources;
using System.Reflection;
using PascalABCCompiler.Errors;
using System.Collections.Generic;


namespace PascalABCCompiler.KuMirParser
{

    public class KuMirLanguageParser : IParser
    {
        GPBParser_KuMir parser;

        public KuMirLanguageParser()
        {
            filesExtensions = new string[1];
            filesExtensions[0] = ".alg";
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
        		return new DefaultLanguageInformation(this);
			}
		}
        
        public void Reset()
        {
            parser = new GPBParser_KuMir(CGTResourceExtractor.Extract(new ResourceManager("KuMirParser.KuMirLang", Assembly.GetExecutingAssembly()), "KuMirLanguage"));
            parser.max_errors = 30;
        }

        public bool CaseSensitive
        {
            get
            {
                return parser.LanguageGrammar.CaseSensitive;
            }
        }


        public List<compiler_directive> CompilerDirectives
        {
            get
            {
                return new List<compiler_directive>();
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
        public Keyword[] Keywords
        {
            get
            {
                return new Keyword[0];
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
                return "KuMir";
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
                return "(c) 3 course";
            }
        }

        public override string ToString()
        {
            return "KuMir Language Parser v1.0";
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
