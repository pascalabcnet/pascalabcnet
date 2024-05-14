/*
using System;
using System.IO;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using System.Resources;
using System.Reflection;
using System.Collections.Generic;
using PascalABCCompiler.Errors;

namespace PascalABCCompiler.VBNETParser
{
	public class VBNETLanguageParser : IParser
    {
		
		public VBNETLanguageParser()
		{
			this.filesExtensions = new string[1];
			this.filesExtensions[0] = ".vb";
			initKeywords();
		}
		
		public List<Error> Errors {
			get {
				return new List<Error>();
			}
			set {
				
			}
		}
		
		public List<compiler_directive> CompilerDirectives {
			get {
				return new List<compiler_directive>();
			}
		}
		
		Keyword[] _keywords;
        void initKeywords()
        {
            List<Keyword> key_words = new List<Keyword>();
            key_words.Add(new Keyword("Module", KeywordKind.Unit));
            key_words.Add(new Keyword("Or", KeywordKind.BinaryOperator));
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
            key_words.Add(new Keyword("until"));
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
		
		public bool CaseSensitive {
			get {
				return false;
			}
		}
		
		string[] filesExtensions;
		public string[] FilesExtensions {
			get {
				return filesExtensions;
			}
		}
		
		public string Name {
			get {
				return "VB.NET";
			}
		}
		
		public string Version {
			get {
				return "0.1";
			}
		}
		
		public string Copyright {
			get {
				return "";
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
                //preprocessor.SourceFilesProvider = value;
            }
        }
		
		ILanguageInformation languageInformation = null;
		
		public ILanguageInformation LanguageInformation {
			get {
				if (languageInformation == null)
        		languageInformation = new VBNETLanguageInformation(this);
        		return languageInformation;
			}
		}
		

        public List<CompilerWarning> Warnings
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public syntax_tree_node BuildTree(string FileName, string Text, ParseMode ParseMode, List<string> DefinesList = null)
		{
			if (string.IsNullOrEmpty(Text))
				return null;
			ICSharpCode.NRefactory.IParser parser = ICSharpCode.NRefactory.ParserFactory.CreateParser(ICSharpCode.NRefactory.SupportedLanguage.VBNet,new StringReader(Text));
			syntax_tree_node cu = null;
			ASTConverter conv = new ASTConverter();
			if (ParseMode == ParseMode.Expression)
			{
				ICSharpCode.NRefactory.Ast.Expression expr = parser.ParseExpression();
				cu = conv.get_expression(expr);
			}
			else
			{
				parser.Parse();
				cu = conv.get_syntax_tree(parser.CompilationUnit, FileName);
			}
			parser.Dispose();
			return cu;
		}
		
		
		
		public void Reset()
		{
			
		}
		
		public override string ToString()
        {
            return "VB.NET Language Parser Wrapper v0.1";
        }
	}
}
*/