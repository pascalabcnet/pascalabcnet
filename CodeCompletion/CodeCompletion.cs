// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.Errors;
using System.IO;
//using ICSharpCode.SharpDevelop.Dom;

namespace CodeCompletion
{
    public class CodeCompletionController
    {
        public static Controller ParsersController = new Controller();
        string FileName;
        string Text;
        public Dictionary<PascalABCCompiler.SyntaxTree.syntax_tree_node, string> docs = new Dictionary<PascalABCCompiler.SyntaxTree.syntax_tree_node, string>();
		static bool parsers_loaded=false;
		public IParser Parser;

        static CodeCompletionController()
        {
            //ParsersController.Reload();
        }
		
        
        public CodeCompletionController()
        {
        	if (!parsers_loaded)
        	{
        		//ParsersController.Reload();
        		parsers_loaded = true;
        	}
        	//dconv = new DomConverter(this);
        }

        public static PascalABCCompiler.Compiler comp;// = new PascalABCCompiler.Compiler();
        public static Hashtable StandartDirectories = new Hashtable();
        public static Hashtable comp_modules = new Hashtable(StringComparer.OrdinalIgnoreCase);
        public static Hashtable parsers = new Hashtable(StringComparer.OrdinalIgnoreCase);
        public static Dictionary<string, InterfaceUnitScope> pabcNamespaces = new Dictionary<string, InterfaceUnitScope>();
         	
        public static string currentLanguageISO;
        static string doctagsParserExtension = ".pasdt" + PascalABCCompiler.Parsers.Controller.HideParserExtensionPostfixChar;
		//public static PascalABCCompiler.Parsers.IParser currentParser;
		static string cur_ext = ".pas";
        private static IParser currentParser;

		public static void SetParser(string ext)
		{
			cur_ext = ext;
            currentParser = null;
		}
		
        public void ResetNamespaces()
        {
            pabcNamespaces.Clear();
        }

		private static string get_doctagsParserExtension(string ext)
		{
			return ext + "dt" + PascalABCCompiler.Parsers.Controller.HideParserExtensionPostfixChar;
		}
		
		public static IParser CurrentParser
		{
			get
			{
                if (currentParser == null)
                    currentParser = parsers[cur_ext] as IParser;
				return currentParser;
			}
		}

        internal compilation_unit ParsersControllerGetCompilationUnit(string FileName, string Text, List<Error> ErrorsList, List<CompilerWarning> Warnings)
        {
            string ext = Path.GetExtension(FileName);
            Parser = ParsersController.selectParser(ext.ToLower());
            parsers[ext] = Parser;
            compilation_unit cu = null;
            cu = ParsersController.GetCompilationUnit(FileName, Text, ErrorsList, Warnings);
            //ParsersController.GetExpression("test.pas", "a+b", new List<PascalABCCompiler.Errors.Error>());
            return cu;
        }
		
        internal compilation_unit ParsersControllerGetCompilationUnitSpecial(string FileName, string Text, List<Error> ErrorsList, List<CompilerWarning> Warnings)
        {
            string ext = Path.GetExtension(FileName);
            Parser = ParsersController.selectParser(ext.ToLower());
            parsers[ext] = Parser;
            compilation_unit cu = null;
            cu = ParsersController.GetCompilationUnitSpecial(FileName, Text, ErrorsList, Warnings);
            //ParsersController.GetExpression("test.pas","a+b",new List<PascalABCCompiler.Errors.Error>());
            return cu;
        }
        
        public DomConverter Compile(string FileName, string Text)
        {
            this.Text = Text;
            this.FileName = FileName;
            List<PascalABCCompiler.Errors.Error> ErrorsList = new List<PascalABCCompiler.Errors.Error>();
            List<CompilerWarning> Warnings = new List<CompilerWarning>();
            PascalABCCompiler.SyntaxTree.compilation_unit cu = null;
            string ext = Path.GetExtension(FileName);
            try
            {
                cu = ParsersControllerGetCompilationUnit(FileName, Text, ErrorsList, Warnings);
                ErrorsList.Clear();
                PascalABCCompiler.SyntaxTree.documentation_comment_list dt = ParsersController.Compile(System.IO.Path.ChangeExtension(FileName, get_doctagsParserExtension(ext)), Text/*+")))));end."*/, ErrorsList, Warnings, PascalABCCompiler.Parsers.ParseMode.Normal) as PascalABCCompiler.SyntaxTree.documentation_comment_list;
                PascalABCCompiler.DocumentationConstructor docconst = new PascalABCCompiler.DocumentationConstructor();
                if (cu != null)
                    docs = docconst.Construct(cu, dt);
            }
            catch (Exception e)
            {
#if DEBUG
                File.AppendAllText("log.txt", e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
#endif
            }
            DomConverter dconv = new DomConverter(this);
            if (cu != null)
            {
                PascalABCCompiler.NetHelper.NetHelper.reset();
                
                dconv.ConvertToDom(cu);
            }
            else
            {
                ErrorsList.Clear();
                Warnings.Clear();
                try
                {
                    //cu = ParsersController.GetComilationUnit(file_name, Text+")))));end.",comp.CompilerOptions.ParserSearchPatchs,ErrorsList);
                    //cu = ParsersControllerGetComilationUnit(file_name, get_temp_text(Text), ErrorsList, true);
                    string tmp = ParsersHelper.GetModifiedProgramm(Text);
                    if (tmp != null)
                    {
                        cu = ParsersControllerGetCompilationUnitSpecial(FileName, tmp, ErrorsList, Warnings);
                    }
                    if (comp_modules[FileName] == null)
                    {
                        if (cu == null)
                            cu = get_fictive_unit(Text, FileName);
                    }
                    ErrorsList.Clear();
                    PascalABCCompiler.SyntaxTree.documentation_comment_list dt = ParsersController.Compile(System.IO.Path.ChangeExtension(FileName, get_doctagsParserExtension(ext)), Text + ")))));end.", ErrorsList, Warnings, PascalABCCompiler.Parsers.ParseMode.Normal) as PascalABCCompiler.SyntaxTree.documentation_comment_list;
                    PascalABCCompiler.DocumentationConstructor docconst = new PascalABCCompiler.DocumentationConstructor();
                    if (cu != null)
                        docs = docconst.Construct(cu, dt);
                }
                catch (Exception e)
                {
#if DEBUG
                    File.AppendAllText("log.txt", e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
#endif
                }
                if (cu != null)
                {
                    PascalABCCompiler.NetHelper.NetHelper.reset();
                    dconv.ConvertToDom(cu);
                }
            }
            if (docs != null) docs.Clear();
            //if (dconv.is_compiled) comp_modules[file_name]=dconv;
            return dconv;
            //ConvertToDom(cu);
        }

        //vremenno, potom ubrat
        private compilation_unit get_fictive_unit(string s, string FileName)
        {
            program_module prog = new program_module();
            int line = 1;
            int col = 1;
            for (int i = 0; i < s.Length; i++)
                if (s[i] == '\n')
                {
                    line++;
                    col = 1;
                }
                else
                {
                    col++;
                }
            prog.source_context = new SourceContext(1, 1, line + 3, 3);
            prog.program_block = new block();
            prog.file_name = FileName;
            statement_list sl = new statement_list();
            prog.program_block.program_code = sl;
            prog.program_block.program_code.left_logical_bracket = new token_info("begin");
            prog.program_block.program_code.left_logical_bracket.source_context = new SourceContext(1, 1, 1, 5);
            prog.program_block.program_code.right_logical_bracket = new token_info("end");
            prog.program_block.program_code.right_logical_bracket.source_context = new SourceContext(line + 3, 1, line + 3, 3);
            sl.subnodes.Add(new empty_statement());
            return prog;
        }
        
        public PascalABCCompiler.SyntaxTree.compilation_unit ParseOnlySyntaxTree(string FileName, string Text)
        {
            List<PascalABCCompiler.Errors.Error> ErrorsList = new List<PascalABCCompiler.Errors.Error>();
            List<CompilerWarning> Warnings = new List<CompilerWarning>();
            PascalABCCompiler.SyntaxTree.compilation_unit cu = ParsersControllerGetCompilationUnit(FileName, Text, ErrorsList, Warnings);
            return cu;
        }

        public DomConverter CompileAllIfNeed(string FileName, string Text)
        {
            DomConverter dconv = (DomConverter)comp_modules[FileName];
            if (dconv != null) return dconv;
            this.Text = Text;
            this.FileName = FileName;
            string ext = Path.GetExtension(FileName);
            List<PascalABCCompiler.Errors.Error> ErrorsList = new List<PascalABCCompiler.Errors.Error>();
            List<CompilerWarning> Warnings = new List<CompilerWarning>();
            PascalABCCompiler.SyntaxTree.compilation_unit cu = ParsersController.GetCompilationUnit(FileName, Text, ErrorsList, Warnings);
            Parser = ParsersController.selectParser(Path.GetExtension(FileName).ToLower());
            ErrorsList.Clear();
            PascalABCCompiler.SyntaxTree.documentation_comment_list dt = ParsersController.Compile(System.IO.Path.ChangeExtension(FileName, get_doctagsParserExtension(ext)), Text, ErrorsList, Warnings, PascalABCCompiler.Parsers.ParseMode.Normal) as PascalABCCompiler.SyntaxTree.documentation_comment_list;
            PascalABCCompiler.DocumentationConstructor docconst = new PascalABCCompiler.DocumentationConstructor();
            if (cu != null)
                docs = docconst.Construct(cu, dt);
            dconv = new DomConverter(this);
            if (CodeCompletionTools.XmlDoc.LookupLocalizedXmlDocForUnitWithSources(FileName, CodeCompletionController.currentLanguageISO) != null)
            {
                dconv.visitor.add_doc_from_text = false;
            }
            if (cu != null)
                dconv.ConvertToDom(cu);
            else
            {
                ErrorsList.Clear();
                Warnings.Clear();
                //cu = ParsersControllerGetComilationUnit(file_name, Text, ErrorsList, true);
                if (comp_modules[FileName] == null)
                {
                    string tmp = ParsersHelper.GetModifiedProgramm(Text);
                    if (tmp != null)
                    {
                    	cu = ParsersControllerGetCompilationUnitSpecial(FileName, tmp, ErrorsList, Warnings);
                    	ErrorsList.Clear();
                    }
                    if (cu == null)
                    cu = get_fictive_unit(Text, FileName);
                }
                ErrorsList.Clear();
                Warnings.Clear();
                dt = ParsersController.Compile(System.IO.Path.ChangeExtension(FileName, get_doctagsParserExtension(ext)), Text + ")))));end.", ErrorsList, Warnings, PascalABCCompiler.Parsers.ParseMode.Normal) as PascalABCCompiler.SyntaxTree.documentation_comment_list;
                //PascalABCCompiler.DocumentationConstructor docconst = new PascalABCCompiler.DocumentationConstructor();
                if (cu != null)
                    docs = docconst.Construct(cu, dt);
                if (CodeCompletionTools.XmlDoc.LookupLocalizedXmlDocForUnitWithSources(FileName, CodeCompletionController.currentLanguageISO) != null)
                {
                	dconv.visitor.add_doc_from_text = false;
                }
                if (cu != null)
                {
                    dconv.ConvertToDom(cu);
                }
            }
            //comp_modules[file_name] = dconv;
            if (dconv.is_compiled) comp_modules[FileName] = dconv;
            	
            if (docs != null) docs.Clear();
            //GC.Collect();
            return dconv;
        }

        public DomConverter CompileAllIfNeed(string FileName, bool parse_only_interface=false)
        {
            this.FileName = FileName;
            this.Text = comp.GetSourceFileText(FileName);
            string ext = Path.GetExtension(FileName);
            List<PascalABCCompiler.Errors.Error> ErrorsList = new List<PascalABCCompiler.Errors.Error>();
            List<CompilerWarning> Warnings = new List<CompilerWarning>();
            PascalABCCompiler.SyntaxTree.compilation_unit cu = null;
            if (Text != null)
            {
                cu = ParsersController.GetCompilationUnit(FileName, Text, ErrorsList, Warnings);
            }
            Parser = ParsersController.selectParser(Path.GetExtension(FileName).ToLower());
            ErrorsList.Clear();
            Warnings.Clear();
            PascalABCCompiler.SyntaxTree.documentation_comment_list dt = ParsersController.Compile(System.IO.Path.ChangeExtension(FileName, get_doctagsParserExtension(ext)), Text, ErrorsList, Warnings, PascalABCCompiler.Parsers.ParseMode.Normal) as PascalABCCompiler.SyntaxTree.documentation_comment_list;
            PascalABCCompiler.DocumentationConstructor docconst = new PascalABCCompiler.DocumentationConstructor();
            if (cu != null)
                docs = docconst.Construct(cu, dt);
            DomConverter dconv = new DomConverter(this);
            dconv.visitor.parse_only_interface = parse_only_interface;
            if (CodeCompletionTools.XmlDoc.LookupLocalizedXmlDocForUnitWithSources(FileName, CodeCompletionController.currentLanguageISO) != null)
            {
                dconv.visitor.add_doc_from_text = false;
            }
            if (cu != null)
                dconv.ConvertToDom(cu);
            else
            {
                ErrorsList.Clear();
                Warnings.Clear();
                //cu = ParsersControllerGetComilationUnit(file_name, Text, ErrorsList, true);
                if (comp_modules[FileName] == null)
                {
                    string tmp = ParsersHelper.GetModifiedProgramm(Text);
                    if (tmp != null)
                    {
                    	cu = ParsersControllerGetCompilationUnitSpecial(FileName, tmp, ErrorsList, Warnings);
                    	ErrorsList.Clear();
                    }
                    if (cu == null)
                    cu = get_fictive_unit(Text, FileName);
                }
                ErrorsList.Clear();
                Warnings.Clear();
                dt = ParsersController.Compile(System.IO.Path.ChangeExtension(FileName, get_doctagsParserExtension(ext)), Text, ErrorsList, Warnings, PascalABCCompiler.Parsers.ParseMode.Normal) as PascalABCCompiler.SyntaxTree.documentation_comment_list;
                if (cu != null)
                    docs = docconst.Construct(cu, dt);
                if (CodeCompletionTools.XmlDoc.LookupLocalizedXmlDocForUnitWithSources(FileName, CodeCompletionController.currentLanguageISO) != null)
                {
                	dconv.visitor.add_doc_from_text = false;
                }
                if (cu != null)
                {
                    dconv.ConvertToDom(cu);
                }
            }
            if (dconv.is_compiled) comp_modules[FileName] = dconv;
            if (docs != null) docs.Clear();
            //comp_modules[file_name] = dconv;
            // GC.Collect();
            return dconv;
        }
    }

    public class CodeCompletionNameHelper
    {
        public static readonly string system_unit_file_name = PascalABCCompiler.TreeConverter.compiler_string_consts.system_unit_file_name;
        public static string system_unit_file_full_name;
        private static CodeCompletionNameHelper helper;

        private CodeCompletionNameHelper()
        {
        }

        public PascalABCCompiler.Parsers.KeywordKind GetKeywordKind(string name)
        {
            if (CodeCompletionController.CurrentParser != null)
            	return CodeCompletionController.CurrentParser.LanguageInformation.GetKeywordKind(name);
            return PascalABCCompiler.Parsers.KeywordKind.None;
        }

        public bool IsKeyword(string name)
        {
            if (CodeCompletionController.CurrentParser != null)
            {
                return CodeCompletionController.CurrentParser.LanguageInformation.IsKeyword(name);
            }
            return false;
        }

        public string[] GetKeywords()
        {
            if (CodeCompletionController.CurrentParser != null)
            	return CodeCompletionController.CurrentParser.LanguageInformation.Keywords;
            return new string[0];
        }

        public string[] GetTypeKeywords()
        {
            if (CodeCompletionController.CurrentParser != null)
            	return CodeCompletionController.CurrentParser.LanguageInformation.TypeKeywords;
            return new string[0];
        }

        const string LibSourceDirectoryIdent = "%LIBSOURCEDIRECTORY%";
        public static string FindSourceFileName(string unit_name, out int found_dir_ind, params string[] ddirs)
        {
            // TODO: check error in older version
            List<string> Dirs = new List<string>();
            Dirs.AddRange(ddirs);
            if (CodeCompletionController.comp != null)
                Dirs.AddRange(CodeCompletionController.comp.CompilerOptions.SearchDirectory);
            if (CodeCompletionController.StandartDirectories.ContainsKey(LibSourceDirectoryIdent))
                Dirs.Add((string)CodeCompletionController.StandartDirectories[LibSourceDirectoryIdent]);
            return CodeCompletionController.comp.FindSourceFileNameInDirs(unit_name, out found_dir_ind, Dirs.ToArray());
        }

        public static CodeCompletionNameHelper Helper
        {
            get
            {
                if (helper == null) helper = new CodeCompletionNameHelper();
                return helper;
            }
        }
    }
    
    public class ParsersHelper
    {
    	string src;
    	static StringBuilder sb = new StringBuilder();
    	
    	public static string GetModifiedProgramm(string src)
    	{
    		sb.Remove(0,sb.Length);
    		if (!src.EndsWith("end."))
    		{
    			sb.AppendLine(src);
    			sb.AppendLine();
    			sb.Append("end.");
    			return sb.ToString();
    		}
    		return null;
    	}
    }
}
