using System;
using System.IO;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using System.Resources;
using System.Reflection;
using System.Collections.Generic;
using PascalABCCompiler.Errors;


namespace PascalABCCompiler.PascalABCParser
{
	
	public class PascalABCLanguageParser:IParser
	{
		private GPBParser_PascalABC parser;
        public PascalABCLanguageParser()
		{
            filesExtensions = new string[1];
            filesExtensions[0] = ".paspart" + Parsers.Controller.HideParserExtensionPostfixChar;
            parser = null;   
		}
        public void Reset()
        {
            CompilerDirectives.Clear();
            parser = new GPBParser_PascalABC(CGTResourceExtractor.Extract(new ResourceManager("PascalABCPartParser.PascalABCLang", Assembly.GetExecutingAssembly()), "PascalABCLanguage"));
            //parser.Parse("begin end.");
            max_errors = 30;
        }
		
        Keyword[] _keywords;
        void initKeywords()
        {
            List<Keyword> key_words = new List<Keyword>();
            key_words.Add(new Keyword("library", KeywordKind.Unit));
            key_words.Add(new Keyword("unit", KeywordKind.Unit));
            key_words.Add(new Keyword("or", KeywordKind.BinaryOperator));
            key_words.Add(new Keyword("xor", KeywordKind.BinaryOperator));
            key_words.Add(new Keyword("and", KeywordKind.BinaryOperator));
            key_words.Add(new Keyword("div", KeywordKind.BinaryOperator));
            key_words.Add(new Keyword("mod", KeywordKind.BinaryOperator));
            key_words.Add(new Keyword("shl", KeywordKind.BinaryOperator));
            key_words.Add(new Keyword("shr", KeywordKind.BinaryOperator));
            key_words.Add(new Keyword("not", KeywordKind.UnaryOperator));
            key_words.Add(new Keyword("as", KeywordKind.As));
            key_words.Add(new Keyword("is", KeywordKind.BinaryOperator));
            key_words.Add(new Keyword("in", KeywordKind.BinaryOperator));
            key_words.Add(new Keyword("sizeof"));
            key_words.Add(new Keyword("typeof"));
            key_words.Add(new Keyword("where"));
            key_words.Add(new Keyword("array"));
            key_words.Add(new Keyword("begin"));
            key_words.Add(new Keyword("case"));
            key_words.Add(new Keyword("class"));
            key_words.Add(new Keyword("const"));
            key_words.Add(new Keyword("constructor"));
            key_words.Add(new Keyword("destructor"));
            key_words.Add(new Keyword("downto"));
            key_words.Add(new Keyword("do"));
            key_words.Add(new Keyword("else"));
            key_words.Add(new Keyword("end"));
            key_words.Add(new Keyword("except"));
            key_words.Add(new Keyword("file"));
            key_words.Add(new Keyword("finalization"));
            key_words.Add(new Keyword("finally"));
            key_words.Add(new Keyword("for"));
            key_words.Add(new Keyword("foreach"));
            key_words.Add(new Keyword("function", KeywordKind.Function));
            key_words.Add(new Keyword("if"));
            key_words.Add(new Keyword("implementation"));
            key_words.Add(new Keyword("inherited", KeywordKind.Inherited));
            key_words.Add(new Keyword("initialization"));
            key_words.Add(new Keyword("interface"));
            key_words.Add(new Keyword("procedure", KeywordKind.Function));
            key_words.Add(new Keyword("operator", KeywordKind.Function));
            key_words.Add(new Keyword("property"));
            key_words.Add(new Keyword("raise", KeywordKind.Raise));
            key_words.Add(new Keyword("record"));
            key_words.Add(new Keyword("repeat"));
            key_words.Add(new Keyword("set"));
            key_words.Add(new Keyword("try"));
            key_words.Add(new Keyword("type", KeywordKind.Type));
            key_words.Add(new Keyword("then"));
            key_words.Add(new Keyword("to"));
            key_words.Add(new Keyword("unitl"));
            key_words.Add(new Keyword("uses", KeywordKind.Uses));
            key_words.Add(new Keyword("var", KeywordKind.Var));
            key_words.Add(new Keyword("while"));
            key_words.Add(new Keyword("with"));
            key_words.Add(new Keyword("where"));
            key_words.Add(new Keyword("nil"));
            key_words.Add(new Keyword("goto"));
            key_words.Add(new Keyword("of", KeywordKind.Of));
            key_words.Add(new Keyword("label"));
            key_words.Add(new Keyword("lock"));
            key_words.Add(new Keyword("program"));
            key_words.Add(new Keyword("event"));
            key_words.Add(new Keyword("new", KeywordKind.New));
            key_words.Add(new Keyword("on"));
            key_words.Add(new Keyword("params"));
            key_words.Add(new Keyword("static", KeywordKind.StaticModificator));
            key_words.Add(new Keyword("forward", KeywordKind.MethodModificator));
            key_words.Add(new Keyword("reintroduce", KeywordKind.ReintroduceModificator));
            key_words.Add(new Keyword("virtual", KeywordKind.VirtualModificator));
            key_words.Add(new Keyword("override", KeywordKind.OverrideModificator));
            key_words.Add(new Keyword("abstract", KeywordKind.AbstractModificator));
            key_words.Add(new Keyword("public", KeywordKind.PublicModificator));
            key_words.Add(new Keyword("private", KeywordKind.PrivateModificator));
            key_words.Add(new Keyword("protected", KeywordKind.ProtectedModificator));
            key_words.Add(new Keyword("internal", KeywordKind.InternalModificator));
            _keywords = key_words.ToArray();
        }		  			
        public Keyword[] Keywords
        {
            get
            {
                return _keywords;
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
        
        string[] filesExtensions;
        public string[] FilesExtensions
        {
            get
            {
                return filesExtensions;
            }
        }

        public bool CaseSensitive
        {
            get
            {
                return parser.LanguageGrammar.CaseSensitive;
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
        public syntax_tree_node BuildTree(string FileName, string Text, string[] SearchPatchs, ParseMode parseMode)
        {
            if (parser == null)//my change
                Reset();
            
            compilerDirectives = new System.Collections.Generic.List<compiler_directive>();
            parser.CompilerDirectives = compilerDirectives;
            parser.errors = Errors;
            parser.current_file_name = FileName;
            parser.parsertools.LineCorrection = 0;
            parser.build_tree_for_brackets = false;
            syntax_tree_node cu = null;
            switch (parseMode)
            {
                case ParseMode.Normal:
            		errors.Clear();
                    cu = (syntax_tree_node)parser.Parse(Text);
                    break;
                case ParseMode.Special:
                    errors.Clear();
                   	cu = (syntax_tree_node)parser.Parse(Text);
                   	break;
                case ParseMode.ForFormatter:
                    errors.Clear();
                    parser.build_tree_for_brackets = true;
                    cu = (syntax_tree_node)parser.Parse(Text);
                    break;
                case ParseMode.Expression:
                   	errors.Clear();
                    Text = String.Concat("<<expression>>", Environment.NewLine, Text);
                    parser.parsertools.LineCorrection = -1;
                    cu = (syntax_tree_node)parser.Parse(Text);
                    return cu as expression;
                case ParseMode.Statement:
                    return null;
            }
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
                return "PABCNETTestPart";
            }
        }
        public string Version
        {
            get
            {
                return "2.0";
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
            return "PascalABC.NET Language Part Parser v0.0";
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
