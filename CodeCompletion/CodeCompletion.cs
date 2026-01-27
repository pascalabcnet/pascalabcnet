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
using Languages.Facade;
using System.Linq;

namespace CodeCompletion
{
    public class CodeCompletionController
    {
        private static readonly LanguageProvider LanguageProvider = LanguageProvider.Instance;

        public Dictionary<syntax_tree_node, string> docs = new Dictionary<syntax_tree_node, string>();
		// static bool parsers_loaded=false;

        static CodeCompletionController()
        {
            //ParsersController.Reload();
        }
		
        
        public CodeCompletionController()
        {
        	/*if (!parsers_loaded)
        	{
        		//ParsersController.Reload();
        		parsers_loaded = true;
        	}*/
        	//dconv = new DomConverter(this);
        }

        public static PascalABCCompiler.Compiler comp;// = new PascalABCCompiler.Compiler();
        public static Dictionary<string, string> StandartDirectories = new Dictionary<string, string>();
        public static Hashtable comp_modules = new Hashtable(StringComparer.OrdinalIgnoreCase);
        // public static Hashtable parsers = new Hashtable(StringComparer.OrdinalIgnoreCase);
        public static Dictionary<string, InterfaceUnitScope> pabcNamespaces = new Dictionary<string, InterfaceUnitScope>();
         	
        public static string currentLanguageISO;
		// public static PascalABCCompiler.Parsers.IParser currentParser;
		// static string cur_ext = ".pas";
        private static ILanguage currentLanguage;

        /// <summary>
        /// Запоминает язык текущего открытого файла.
        /// Если язык файла не поддерживается в системе, то ошибка не выбрасывается,
        /// текущему языку присваивается null.
        /// </summary>
		public static void SetLanguage(string fileName)
		{
            currentLanguage = LanguageProvider.SelectLanguageByExtensionSafe(fileName);
		}
		
        public void ResetNamespaces()
        {
            pabcNamespaces.Clear();
        }
		
		public static ILanguage CurrentLanguage
		{
			get
			{
                return currentLanguage;
			}
		}

        /// <summary>
        /// Поддерживается ли Intellisense для текущего языка
        /// </summary>
        public static bool IntellisenseAvailable()
        {
            return CurrentLanguage != null && CurrentLanguage.LanguageIntellisenseSupport != null;
        }
        
        public DomConverter Compile(string FileName, string Text)
        {
            List<Error> ErrorsList = new List<Error>();
            List<CompilerWarning> Warnings = new List<CompilerWarning>();
            compilation_unit cu = null;

            ILanguage currentLanguage = LanguageProvider.SelectLanguageByExtension(FileName);

            try
            {
                cu = currentLanguage.Parser.GetCompilationUnit(FileName, Text, ErrorsList, Warnings, ParseMode.Normal, false);
                
                ErrorsList.Clear();

                if (currentLanguage.DocParser != null)
                    BuildDocs(Text, cu, currentLanguage.DocParser);

            }
            catch (Exception e)
            {
#if DEBUG
                File.AppendAllText("log.txt", e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
#endif
            }
            
            TypeScope.instance_cache.Clear();

            // очистка кэша и данных от старых компиляций, чтобы при новой компиляции не появились ссылки на старые данные
            TypeTable.Clear();

            DomConverter dconv = new DomConverter(this);
            if (cu != null)
            {
                PascalABCCompiler.NetHelper.NetHelper.reset();
                
                dconv.ConvertToDom(cu);
            }
            // Попытка поменять текст программы ниже сработает только для PascalABC.NET
            else if (currentLanguage == LanguageProvider.MainLanguage)
            {

                ErrorsList.Clear();
                Warnings.Clear();
                try
                {
                    string tmp = ParsersHelper.GetModifiedProgramm(Text);
                    if (tmp != null)
                    {
                        cu = currentLanguage.Parser.GetCompilationUnit(FileName, Text, ErrorsList, Warnings, ParseMode.Special, false);
                    }

                    ErrorsList.Clear();
                    Warnings.Clear();

                    if (currentLanguage.DocParser != null)
                        BuildDocs(Text, cu, currentLanguage.DocParser);

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

        private void BuildDocs(string Text, compilation_unit cu, IDocParser docParser)
        {
            documentation_comment_list dt = docParser.BuildTree(Text);
            PascalABCCompiler.DocumentationConstructor docconst = new PascalABCCompiler.DocumentationConstructor();
            if (cu != null)
                docs = docconst.Construct(cu, dt);
        }
        
        public compilation_unit ParseOnlySyntaxTree(string FileName, string Text)
        {
            var currentLanguage = LanguageProvider.SelectLanguageByExtensionSafe(FileName);

            if (currentLanguage == null)
                return null;

            var cu = currentLanguage.Parser.GetCompilationUnit(FileName, Text, new List<Error>(), new List<CompilerWarning>(), ParseMode.Normal, false);
            
            return cu;
        }

        public DomConverter CompileAllIfNeed(string FileName, bool parse_only_interface=false)
        {
            string Text = comp.GetSourceFileText(FileName);
            List<Error> ErrorsList = new List<Error>();
            List<CompilerWarning> Warnings = new List<CompilerWarning>();
            compilation_unit cu = null;

            DomConverter dconv = new DomConverter(this);

            ILanguage currentLanguage = LanguageProvider.SelectLanguageByExtensionSafe(FileName);
            
            if (currentLanguage == null)
                return dconv;
            
            if (Text != null)
            {
                cu = currentLanguage.Parser.GetCompilationUnit(FileName, Text, ErrorsList, Warnings, ParseMode.Normal, true);
            }
            ErrorsList.Clear();
            Warnings.Clear();

            if (currentLanguage.DocParser != null)
                BuildDocs(Text, cu, currentLanguage.DocParser);
            
            dconv.visitor.parse_only_interface = parse_only_interface;
            if (CodeCompletionTools.XmlDoc.LookupLocalizedXmlDocForUnitWithSources(FileName, CodeCompletionController.currentLanguageISO) != null)
            {
                dconv.visitor.add_doc_from_text = false;
            }

            if (cu != null)
            {
                dconv.ConvertToDom(cu);
            }
            // Попытка поменять текст программы ниже сработает только для PascalABC.NET
            else if (currentLanguage == LanguageProvider.MainLanguage)
            {
                ErrorsList.Clear();
                Warnings.Clear();

                if (comp_modules[FileName] == null)
                {
                    string tmp = ParsersHelper.GetModifiedProgramm(Text);
                    if (tmp != null)
                    {
                        cu = currentLanguage.Parser.GetCompilationUnit(FileName, Text, ErrorsList, Warnings, ParseMode.Special, true);
                    }

                    ErrorsList.Clear();
                    Warnings.Clear();
                }

                if (currentLanguage.DocParser != null)
                    BuildDocs(Text, cu, currentLanguage.DocParser);

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
        public static readonly string system_unit_file_name = PascalABCCompiler.StringConstants.pascalSystemUnitName;
        public static string system_unit_file_full_name;
        private static CodeCompletionNameHelper helper;

        private CodeCompletionNameHelper()
        {
        }

        public PascalABCCompiler.Parsers.KeywordKind GetKeywordKind(string name)
        {
            if (CodeCompletionController.IntellisenseAvailable())
            	return CodeCompletionController.CurrentLanguage.LanguageIntellisenseSupport.GetKeywordKind(name);
            return PascalABCCompiler.Parsers.KeywordKind.None;
        }

        public bool IsKeyword(string name)
        {
            if (CodeCompletionController.IntellisenseAvailable())
            {
                return CodeCompletionController.CurrentLanguage.LanguageIntellisenseSupport.IsKeyword(name);
            }
            return false;
        }

        public List<string> GetKeywords()
        {
            if (CodeCompletionController.IntellisenseAvailable())
                return CodeCompletionController.CurrentLanguage.LanguageInformation.KeywordsStorage.KeywordsForIntellisenseList;
            return new List<string>();
        }

        public List<string> GetTypeKeywords()
        {
            if (CodeCompletionController.IntellisenseAvailable())
                return CodeCompletionController.CurrentLanguage.LanguageInformation.KeywordsStorage.TypeKeywords;
            return new List<string>();
        }

        const string LibSourceDirectoryIdent = "%LIBSOURCEDIRECTORY%";
        public static string FindSourceFileName(string unit_name, out int found_dir_ind, ILanguage currentUnitLanguage, params string[] ddirs)
        {
            // TODO: check error in older version
            List<string> Dirs = new List<string>();
            Dirs.AddRange(ddirs);
            if (CodeCompletionController.comp != null)
                Dirs.AddRange(CodeCompletionController.comp.GetCurrentSearchDirectories(currentUnitLanguage));
            // Надо как-то проверять, что мы не в инсталированной версии EVA
            if (CodeCompletionController.StandartDirectories.ContainsKey(LibSourceDirectoryIdent) && Directory.Exists(CodeCompletionController.StandartDirectories[LibSourceDirectoryIdent]))
                Dirs.AddRange(GetLibSourceDirectories(currentUnitLanguage));
            return CodeCompletionController.comp.FindSourceFileNameInDirs(unit_name, out found_dir_ind, currentUnitLanguage, Dirs.ToArray());
        }

        private static string[] GetLibSourceDirectories(ILanguage currentUnitLanguage)
        {
            return LanguageProvider.Instance.Languages
                                        .Where(lang => lang == currentUnitLanguage)
                                        .Concat(LanguageProvider.Instance.Languages.Where(lang => lang != currentUnitLanguage))
                                        .Select(lang => Path.Combine(CodeCompletionController.StandartDirectories[LibSourceDirectoryIdent],
                                                lang.Name.Replace(PascalABCCompiler.StringConstants.pascalLanguageName, ""))) // для PascalABC.NET прямо в LibSource, остальные во внутренних папках
                                        .Where(dir => Directory.Exists(dir))
                                        .ToArray();
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
    		if (!src.TrimEnd().EndsWith("end."))
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
