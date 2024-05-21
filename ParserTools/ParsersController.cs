// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.IO;
using PascalABCCompiler.Errors;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace PascalABCCompiler.Parsers
{
	public class Controller
	{

        private static readonly Lazy<Controller> lazyInstanceWrapper = new Lazy<Controller>(() => new Controller(), true);
        public static Controller Instance { get { return lazyInstanceWrapper.Value; } }

        public const char HideParserExtensionPostfixChar = '_';

        public event Action<IParser> ParserConnected;
        public event Action<string> ParserLoadErrorOccured;

        public List<IParser> Parsers = new List<IParser>();
        
        public IParser LastParser;

        /// <summary>
        /// Загружает стандартные парсеры из папки бин (в частности PascalABCParser.dll)
        /// </summary>
        public void LoadStandardParsers()
        {
            string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);

            DirectoryInfo directory = new DirectoryInfo(directoryName);

            FileInfo[] dllfiles = directory.GetFiles("*Parser.dll");

            foreach (FileInfo parserFile in dllfiles)
            {
                if (Path.GetFileName(parserFile.FullName) == "VBNETParser.dll" || Path.GetFileName(parserFile.FullName) == "PascalABCPartParser.dll")
                    continue;

                IntegrateParsersFromAssembly(parserFile);
            }
        }

        /// <summary>
        /// Загружает все парсеры языков из переданной сборки, используя рефлексию
        /// </summary>
        public void IntegrateParsersFromAssembly(FileInfo parserFile)
        {
            Assembly assembly = Assembly.LoadFile(parserFile.FullName);
            try
            {
                Type[] types = assembly.GetTypes();
                if (assembly != null)
                {
                    foreach (Type type in types)
                    {
                        if (type.Name.IndexOf("LanguageParser") >= 0)
                        {
                            object obj = Activator.CreateInstance(type);
                            if (obj is IParser parser)
                            {
                                SaveParser(parser);
                            }
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException e)
            {
#if DEBUG
                string errorMessage = e + Environment.NewLine;
                errorMessage += "<Loader exceptions>:" + Environment.NewLine;
                errorMessage += string.Join(Environment.NewLine, e.LoaderExceptions.Select(error => error.ToString())) + Environment.NewLine;
                File.AppendAllText("log.txt", errorMessage);
#endif
                string errorLocalized = string.Format(ParserErrorsStringResources.Get("LOAD_ERROR{0}"), Path.GetFileName(parserFile.FullName));
                ParserLoadErrorOccured?.Invoke(errorLocalized + Environment.NewLine);
            }
            catch (Exception e)
            {
#if DEBUG
                string errorMessage = e + Environment.NewLine;
                File.AppendAllText("log.txt", errorMessage);
#endif
                string errorLocalized = string.Format(ParserErrorsStringResources.Get("LOAD_ERROR{0}"), Path.GetFileName(parserFile.FullName));
                ParserLoadErrorOccured?.Invoke(errorLocalized + Environment.NewLine);
            }
        }

        private Controller()
        {
            sourceFilesProvider = SourceFilesProviders.DefaultSourceFilesProvider;
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
                foreach (IParser parser in Parsers)
                {
                    parser.SourceFilesProvider = value;
                }
            }
        }

        /// <summary>
        /// Добавляет парсер в общий список парсеров (с вызовом инициализации парсера)  
        /// </summary>
        private void SaveParser(IParser parser)
        {
            Parsers.Add(parser);
            parser.Reset();
            parser.SourceFilesProvider = SourceFilesProvider;
            ParserConnected?.Invoke(parser);
        }
        public void SendUnitCheckToParsers(Func<bool> callback)
        {
            foreach (IParser parser in Parsers)
            {
                if (parser is BaseParser baseParser)
                    baseParser.CheckIfParsingUnit = callback;
            }
        }

        /*public void SendUnitCheckToParsers(Func<bool> callback)
        {
            foreach (IParser parser in Parsers)
            {
                if (parser is BaseParser baseParser)
                    baseParser.CheckIfParsingUnit = callback;
            }
        }
        */
        /// <summary>
        /// Возвращает подходящий для переданного расширения парсер из имеющихся 
        /// </summary>
        public IParser SelectParser(string extension)
        {
            foreach (IParser parser in Parsers)
                foreach (string parserExtension in parser.FilesExtensions)
                    if (parserExtension.ToLower() == extension)
                        return parser;
            return null;
        }
        

        public bool ParserExists(string ext)
        {
            return SelectParser(ext) != null;
        }

        /// <summary>
        /// Строит синтаксическое дерево с автоматическим выбором подходящего парсера
        /// </summary>
        /// <exception cref="ParserBadFileExtension"></exception>
        public SyntaxTree.syntax_tree_node SelectParserForUnitAndBuildTree(string FileName, string Text, List<Error> Errors, List<CompilerWarning> Warnings, ParseMode ParseMode, List<string> DefinesList = null)
        {
            LastParser = SelectParser(Path.GetExtension(FileName).ToLower());
            
            if (LastParser == null)
                throw new ParserBadFileExtension(FileName);
            
            LastParser.Errors = Errors;
            LastParser.Warnings = Warnings;
            
            return LastParser.BuildTree(FileName, Text, ParseMode, DefinesList);
        }

        /// <summary>
        /// Возвращеает синтаксическое дерево модуля
        /// </summary>
        public SyntaxTree.compilation_unit GetCompilationUnit(string FileName, string Text, List<Error> Errors, List<CompilerWarning> Warnings, ParseMode parseMode, List<string> DefinesList = null)
        {
            return GetSyntaxTree<SyntaxTree.compilation_unit>(FileName, Text, Errors, Warnings, parseMode, DefinesList);
        }

        public SyntaxTree.compilation_unit GetCompilationUnitForFormatter(string FileName, string Text, List<Error> Errors, List<CompilerWarning> Warnings)
        {
            try // SSM 06.09.18
            {
                return GetSyntaxTree<SyntaxTree.compilation_unit>(FileName, Text, Errors, Warnings, ParseMode.ForFormatter);
            }
            catch (ParserBadFileExtension e)
            {
                Errors.Add(e);
                return null;
                // Погасить исключение если оно не погашено ранее
            }
        }

        public SyntaxTree.expression GetExpression(string FileName, string Text, List<Error> Errors, List<CompilerWarning> Warnings)
        {
            try // SSM 06.09.18
            {
                return GetSyntaxTree<SyntaxTree.expression>(FileName, Text, Errors, Warnings, ParseMode.Expression);
            }
            catch
            {
                return null;
            }
        }

        public SyntaxTree.expression GetTypeAsExpression(string FileName, string Text, List<Error> Errors, List<CompilerWarning> Warnings)
        {
            return GetSyntaxTree<SyntaxTree.expression> (FileName, Text, Errors, Warnings, ParseMode.TypeAsExpression);
        }

        public SyntaxTree.statement GetStatement(string FileName, string Text, List<Error> Errors, List<CompilerWarning> Warnings)
        {
            return GetSyntaxTree<SyntaxTree.statement>(FileName, Text, Errors, Warnings, ParseMode.Statement);
        }

        /// <summary>
        /// Обобщенная функция для получения различных синтаксических узлов
        /// </summary>
        private T GetSyntaxTree<T>(string FileName, string Text, List<Error> Errors, List<CompilerWarning> Warnings, ParseMode parseMode, List<string> DefinesList = null) where T : SyntaxTree.syntax_tree_node
        {
            SyntaxTree.syntax_tree_node unitNode = SelectParserForUnitAndBuildTree(FileName, Text, Errors, Warnings, parseMode, DefinesList);

            if (unitNode == null)
                return null;

            if (unitNode is T)
                return unitNode as T;

            Errors.Add(new UnexpectedNodeType(FileName, unitNode.source_context, null));

            return null;
        }

        public void Reset()
		{
		
        }

	}
}
