// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
/***************************************************************************
*   
*   Управляющий блок компилятора, алгоритм компиляции модулей
*   Зависит от Errors,SyntaxTree,SemanticTree,Parsers,TreeConvertor,
*              CodeGenerators
* 
***************************************************************************/

#region algorithm
/***************************************************************************
 *               Рекурсивный алгоритм компиляции модулей
 *						        версия 1.4
 *   
 *   CompileUnit(ИмяФайла)  
 *   1.CompileUnit(new СписокМодулей,ИмяФайла)
 *   2.Докомпилировать модули из СписокОтложенойКомпиляции; 
 * 
 *   CompileUnit(СписокМодулей,ИмяФайла)
 *   1.ТекущийМодуль=ТаблицаМодулей[ИмяФайла];
 *     Если (ТекущийМодуль!=0) то 
 *	     Если (ТекущийМодуль.Состояние==BeginCompilation)
 *         СписокМодулей.Добавить(ТекущийМодуль);
 *         Выход;    
 *       иначе перейти к пункту 5
 *   
 *   2.Если ЭтоФайлDLL(ИмяФайла) то
 *       Если ((ТекущийМодуль=СчитатьDLL(ИмяФайла))!=0) то
 *         СписокМодулей.Добавить(ТекущийМодуль);
 *         ТаблицаМодулей.Добавить(ТекущийМодуль);
 *         Выход;
 *       иначе
 *         Ошибка("Не могу подключить сборку");
 *         Выход;
 *   
 *   3.Если ЭтоФайлPCU(ИмяФайла) то
 *       Если ((ТекущийМодуль=СчитатьPCU(ИмяФайла))!=0) то
 *         СписокМодулей.Добавить(ТекущийМодуль);
 *         ТаблицаМодулей.Добавить(ТекущийМодуль);
 *         Выход;
 *       иначе
 *         иначе перейти к пункту 4;
 *   
 *   4.ТекущийМодуль=новыйМодуль();
 *     ТекущийМодуль.СинтаксическоеДерево=Парасеры.Парсить(ИмяФайла,ТекущийМодуль.СписокОшибок);
 *     Если (ТекущийМодуль.СинтаксическоеДерево==0) то 
 *        Если (ТекущийМодуль.СписокОшибок.Количество==0) то 
 *          Ошибка("Модуль не неайден");
 *        иначе
 *          Ошибка(ТекущийМодуль.СписокОшибок[0]);
 *     ТаблицаМодулей[ИмяФайла]=ТекущийМодуль;
 *     ТекущийМодуль.Состояние=BeginCompilation;
 *   
 *   5.СинтаксическийСписокМодулей=ТекущийМодуль.СинтаксическоеДерево.Interface.usesList;
 *     Для(i=СинтаксическийСписокМодулей.Количество-1-ТекущийМодуль.КомпилированыеВInterface.Количество;i>=0;i--)
 *        ТекушийМодуль.ТекущийUsesМодуль=СинтаксическийСписокМодулей[i].ИмяФайла;
 *        ИмяUsesФайла=СинтаксическийСписокМодулей[i].ИмяФайла;
 *        Если (ТаблицаМодулей[ИмяUsesФайла]!=0)
 *          Если (ТаблицаМодулей[ИмяUsesФайла].Состояние==BeginCompilation)
 *            Если (ТаблицаМодулей[ТаблицаМодулей[ИмяUsesФайла].ТекущийUsesМодуль].Состояние=BeginCompilation)
 *               Ошибка("Циклическая связь модулей");
 *        CompileUnit(ТекущийМодуль.КомпилированыеВInterface,ИмяUsesФайла);
 *        Если (ТекушийМодуль.Состояние==Compiled) то
 *          СписокМодулей.Добавить(ТекушийМодуль);
 *          Выход; 
 * 
 *   6.ТекущийМодуль.СемантическоеДерево=КонверторДерева.КонвертироватьInterfaceЧасть(
 *                                         ТекущийМодуль.СинтаксическоеДерево,
 *                                         ТекущийМодуль.КомпилированыеВInterface,
 *                                         ТекущийМодуль.СписокОшибок);
 *     СписокМодулей.Добавить(ТекущийМодуль);    
 *     СинтаксическийСписокМодулей=ТекущийМодуль.СинтаксическоеДерево.Implementation.usesList;
 *     Для(i=СинтаксическийСписокМодулей.Количество-1;i>=0;i--)
 *       Если (ТаблицаМодулей[СинтаксическийСписокМодулей[i].ИмяФайла].Состояние=BeginCompilation)
 *         СписокОтложенойКомпиляции.Добавить(ТаблицаМодулей[СинтаксическийСписокМодулей[i].ИмяФайла]);
 *       иначе
 *         CompileUnit(ТекущийМодуль.КомпилированыеВImplementation,СинтаксическийСписокМодулей[i].ИмяФайла);
 *     Если(ДобавлялиХотябыОдинВСписокОтложенойКомпиляции)
 *       СписокОтложенойКомпиляции.Добавить(ТекущийМодуль);
 *       выход;
 *     иначе
 *       КонверторДерева.КонвертироватьImplementationЧасть(
 *                                         ТекущийМодуль.СинтаксическоеДерево,
 *                                         ТекущийМодуль.СемантическоеДерево,
 *                                         ТекущийМодуль.КомпилированыеВImplementation 
 *                                         ТекущийМодуль.СписокОшибок);
 *     ТекущийМодуль.Состояние=Compiled;
 *     СохранитьPCU(ТекущийМодуль);
 *     
 * 
 *     
 *     [краткая верcия алгоритма компиляции модулей]
 *     CompileUnit(ИмяФайла)  
 *     1.CompileUnit(new СписокМодулей,ИмяФайла)
 *     2.Докомпилировать модули из СписокОтложенойКомпиляции; 
 *     
 *     CompileUnit(СписокМодулей,ИмяФайла);
 *     1.Если у этого модуля откомпилирован хотябы интерфейс то 
 *         добавить его в СписокМодулей
 *         выход
 *     2.Если это DLL то 
 *         считать
 *         добавить его в СписокМодулей
 *         выход
 *     3.Если это PCU то 
 *         считать
 *         добавить его в СписокМодулей
 *         выход
 *     4.создать новый компилируемыйМодуль
 *       РаспарситьТекст(ИмяФайла)
 *       Состояние компилируемогоМодуля установить на BeginCompilation
 *     5.Для всех модулей из Interface части компилируемогоМодуля справа налево
 *         Если мы уже начаинали компилировать этот модуль  
 *           Если состояние модуля BeginCompilation
 *             Если состояние последнего компилируемого им модуля BeginCompilation
 *               ошибка("Циклическая связь модулей")
 *               выход 
 *         CompileUnit(Список из Interface части компилируемогоМодуля,модуль.имя)
 *         Если компилируемыйМодуль.Состояние Compiled то
 *           добавить его в СписокМодулей
 *           выход
 *     6.Откомпилировать Interface часть компилируемогоМодуля
 *       Для всех модулей из Implementation части компилируемогоМодуля справа налево
 *         Если состояние очередного модуля BeginCompilation то
 *           добавить его в список отложеной компиляции;
 *         иначе
 *           CompileUnit(Список из Implementation части компилируемогоМодуля,модуль.имя)
 *       Если Добавляли Хотябы Один В Список Отложеной Компиляции то
 *         добавить компилируемыйМодуль в список отложеной компиляции
 *         выход
 *       Откомпилировать Implementation часть компилируемогоМодуля
 *       Состояние компилируемогоМодуля установить на Compiled
 *       добавить его в СписокМодулей
 *       Сохранить компилируемыйМодуль в виде PCU файла на диск
 * 
 * 
 *     
 ***************************************************************************/
#endregion

#define DEBUG

using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

using PascalABCCompiler.Errors;
using PascalABCCompiler.PCU;
using PascalABCCompiler.SemanticTreeConverters;
using PascalABCCompiler.SyntaxTreeConverters;
using PascalABCCompiler.TreeRealization;

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Languages.Facade;

namespace PascalABCCompiler
{
    public enum UnitState { BeginCompilation, InterfaceCompiled, Compiled }

    public class CompilationUnit
    {
        internal bool CaseSensitive = false;
        /// <summary>
        /// поле для проверки на циклическую зависимость интерфейсов модулей
        /// </summary>
        public string currentUsedUnitId;
        public SyntaxError syntax_error;
        public string UnitFileName;
        public List<Error> ErrorList = new List<Error>();
        public bool Documented;
        internal List<SyntaxTree.unit_or_namespace> possibleNamespaces = new List<PascalABCCompiler.SyntaxTree.unit_or_namespace>();
        //internal List<CompilationUnit> AssemblyReferences = new List<CompilationUnit>();

        //private SemanticTree.compilation_unitArrayList _interfaceUsedUnits=new SemanticTree.compilation_unitArrayList();

        // название языка модуля
        public ILanguage Language { get; set; }

        /// <summary>
        /// Только "реальные" юниты (не dll и namespace)
        /// </summary>
        public Dictionary<unit_node, CompilationUnit> InterfaceUsedDirectUnits { get; } = new Dictionary<unit_node, CompilationUnit>();

        public unit_node_list InterfaceUsedUnits { get; } = new unit_node_list();
        /// <summary>
        /// Только "реальные" юниты (не dll и namespace)
        /// </summary>
        public Dictionary<unit_node, CompilationUnit> ImplementationUsedDirectUnits { get; } = new Dictionary<unit_node, CompilationUnit>();
        public unit_node_list ImplementationUsedUnits { get; } = new unit_node_list();

        public bool ForEachDirectCompilationUnit(Func<CompilationUnit, string, bool> on_unit)
        {
            foreach (var kvp in InterfaceUsedDirectUnits)
                if (!on_unit(kvp.Value, InterfaceUsedUnits.unit_uses_paths[kvp.Key]))
                    return false;
            foreach (var kvp in ImplementationUsedDirectUnits)
                if (!on_unit(kvp.Value, ImplementationUsedUnits.unit_uses_paths[kvp.Key]))
                    return false;
            return true;
        }

        private SyntaxTree.compilation_unit _syntaxTree = null;
        public SyntaxTree.compilation_unit SyntaxTree
        {
            get { return _syntaxTree; }
            set { _syntaxTree = value; }
        }

        private PascalABCCompiler.TreeRealization.unit_node _semanticTree = null;
        public PascalABCCompiler.TreeRealization.unit_node SemanticTree
        {
            get { return _semanticTree; }
            set { _semanticTree = value; }
        }

        /*private SyntaxTree.unit_or_namespace _syntaxUnitName = null;
        public SyntaxTree.unit_or_namespace SyntaxUnitName
        {
            get { return _syntaxUnitName; }
            set { _syntaxUnitName = value; }
        }*/

        private TreeRealization.using_namespace_list _interface_using_namespace_list = new TreeRealization.using_namespace_list();
        public TreeRealization.using_namespace_list InterfaceUsingNamespaceList
        {
            get { return _interface_using_namespace_list; }
            set { _interface_using_namespace_list = value; }
        }

        private TreeRealization.using_namespace_list _implementation_using_namespace_list = new TreeRealization.using_namespace_list();
        public TreeRealization.using_namespace_list ImplementationUsingNamespaceList
        {
            get { return _implementation_using_namespace_list; }
            set { _implementation_using_namespace_list = value; }
        }

        public UnitState State = UnitState.BeginCompilation;
    }

    public class CompilationUnitHashTable : Hashtable
    {
        public CompilationUnitHashTable() : base(StringComparer.InvariantCultureIgnoreCase) { }

        public CompilationUnit this[string key]
        {
            get
            {
                if (key != null)
                    return (base[key] as CompilationUnit);
                return null;
            }
            set
            {
                base[key] = value;
            }
        }
    }

    /// <summary>
    /// Опции компиляции
    /// </summary>
    //[Serializable()]
    public class CompilerOptions : MarshalByRefObject
    {
        public enum OutputType { ClassLibrary = 0, ConsoleApplicaton = 1, WindowsApplication = 2, PascalCompiledUnit = 3, SemanticTree = 4 }

        public bool Debug = false;
        public bool ForDebugging = false;
        public bool ForIntellisense = false;
        public bool Rebuild = false;
        public bool Optimise = false;
        public bool DisableStandardUnits = false; // true устанавливается соответствующей директивой
        public bool SavePCUInThreadPull = false;
        public bool RunWithEnvironment = false;
        public string CompiledUnitExtension = StringConstants.pascalCompiledUnitExtension;
        public bool ProjectCompiled = false;
        public IProjectInfo CurrentProject = null;
        public OutputType OutputFileType = OutputType.ConsoleApplicaton;
        public bool GenerateCode = true;
        public bool SaveDocumentation = true;
        public bool SavePCU = true;
        public bool IgnoreRtlErrors = true;
        public bool Only32Bit = false;
        public string Locale = "ru";
        public SyntaxTree.compilation_unit UnitSyntaxTree = null;

        private string sourceFileName = null;
        ///имя исходного файла
        ///при измененнии меняется OutputFileName,OutputDirectory,SourceFileDirectory
        public string SourceFileName
        {
            get { return sourceFileName; }
            set
            {
                sourceFileName = value;
                OutputFileName = value;
                sourceFileDirectory = Path.GetDirectoryName(value);
                if (sourceFileDirectory == "") sourceFileDirectory = Environment.CurrentDirectory;
                outputDirectory = SourceFileDirectory;
                useOutputDirectory = false;
            }
        }


        private string sourceFileDirectory = null;
        public string SourceFileDirectory
        {
            get { return sourceFileDirectory; }
        }

        private string outputFileName = null;
        
        // имя выходного файла без расширения
        // если имя указано без пути то в качестве пути используетя OutputDirectory
        public string OutputFileName
        {
            get
            {
                string FileName = outputFileName;
                switch (OutputFileType)
                {
                    case OutputType.ConsoleApplicaton:
                    case OutputType.WindowsApplication: FileName = FileName + ".exe"; break;
                    case OutputType.ClassLibrary: FileName = FileName + ".dll"; break;
                    case OutputType.PascalCompiledUnit: FileName = FileName + CompiledUnitExtension; break;
                }
                if (Path.GetDirectoryName(FileName) == "") FileName = Path.Combine(OutputDirectory, FileName);
                return FileName;
            }
            set
            {
                outputFileName = Path.GetFileNameWithoutExtension(value);
            }
        }

        public string SystemDirectory;

        public List<string> ForceDefines = new List<string>();

        public List<string> SearchDirectories;

        private bool useDllForSystemUnits = false;

        public bool UseDllForSystemUnits
        {
            get
            {
                return useDllForSystemUnits;
            }
            set
            {
                useDllForSystemUnits = value;
                NetHelper.NetHelper.UsePABCRtl = value;
            }
        }

        public string[] ParserSearchPaths;

        // LeftToAll - слева во все модули, RightToMain - справа, только в основную программу
        public enum StandardModuleAddMethod { LeftToAll, RightToMain };
        
        [Serializable()]
        public class StandardModule : MarshalByRefObject
        {
            public string name;
            public StandardModuleAddMethod addMethod;
            public string languageToAdd;

            public StandardModule(string name, StandardModuleAddMethod addMethod = StandardModuleAddMethod.LeftToAll, string languageToAdd = StringConstants.pascalLanguageName)
            {
                this.name = name;
                this.languageToAdd = languageToAdd;
                this.addMethod = addMethod;
            }
        }

        private Dictionary<string, List<StandardModule>> standardModules = new Dictionary<string, List<StandardModule>>();

        /// <summary>
        /// Списки стандартных модулей для поддерживаемых языков (первым в списке должен быть модуль "System")
        /// </summary>
        public Dictionary<string, List<StandardModule>> StandardModules
        {
            get
            {
                if (standardModules.Count == 0)
                    LoadStandardModules();
                return standardModules;
            }
            set
            {
                standardModules = value;
            }
        }

        /// <summary>
        /// Заполняет словарь стандартных модулей для всех поддерживаемых языков
        /// </summary>
        private void LoadStandardModules()
        {
            foreach (ILanguage language in LanguageProvider.Instance.Languages)
            {
                standardModules[language.Name] = language.SystemUnitNames.Select(unitName => new StandardModule(unitName, StandardModuleAddMethod.LeftToAll,  language.Name)).ToList();
            }
        }

        public void RemoveStandardModule(string language, string name)
        {
            int moduleIndex = StandardModules[language].FindIndex(module => module.name == name);
            
            if (moduleIndex != -1)
                StandardModules[language].RemoveAt(moduleIndex);
        }

        public void RemoveStandardModuleAtIndex(string language, int index)
        {
            if (index < StandardModules[language].Count)
                StandardModules[language].RemoveAt(index);
        }


        internal string outputDirectory = null;
        internal bool useOutputDirectory = false;

        //для поиска pcu во вторую очередь
        //для сохранения сюда .exe, .dll и .pdb файлов
        public string OutputDirectory
        {
            get
            {
                return outputDirectory;
            }
            set
            {
                outputDirectory = value;
                useOutputDirectory = outputDirectory != null;
            }
        }
        //для поиска pcu в третью очередь исп. путь к pas файлу

        public Hashtable StandardDirectories;

        private void SetDirectories()
        {
            SystemDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);

            StandardDirectories = new Hashtable(StringComparer.InvariantCultureIgnoreCase)
            {
                { "%PABCSYSTEM%", SystemDirectory }
            };

            ParserSearchPaths = new string[] { Path.Combine(SystemDirectory, "Lib") };
            SearchDirectories = ParserSearchPaths.ToList();
        }


        public CompilerOptions()
        {
            SetDirectories();
        }

        public CompilerOptions(string SourceFileName, OutputType OutputFileType)
        {
            SetDirectories();
            this.SourceFileName = SourceFileName;
            this.OutputFileType = OutputFileType;
        }

    }

    public enum CompilerState
    {
        Ready, CompilationStarting, Reloading, ParserConnected,
        BeginCompileFile, BeginParsingFile, EndParsingFile, CompileInterface, CompileImplementation, EndCompileFile,
        ReadDLL, ReadPCUFile, SavePCUFile, CodeGeneration, CompilationFinished, PCUReadingError, PCUWritingError,
        SemanticTreeConverterConnected, SemanticTreeConversion, SyntaxTreeConversion
    }

    [Serializable()]
    public class CompilerInternalDebug
    {
        public bool CodeGeneration = true;
        public bool SemanticAnalysis = true;
        public bool PCUGenerate = true;
        public bool SkipPCUErrors = true;
        public bool IncludeDebugInfoInPCU = true;
        public bool AlwaysGenerateXMLDoc = false;
        public bool SkipInternalErrorsIfSyntaxTreeIsCorrupt = true;
        public bool UseStandarParserForIntellisense = true;
        public bool RunOnMono = false;

#if DEBUG
        public bool DebugVersion
        {
            get { return true; }
        }
#elif !DEBUG
        public bool DebugVersion
        {
            get { return false; }
        }
#endif
    }

    public class SupportedSourceFile
    {
        private readonly string[] extensions;

        public string[] Extensions
        {
            get { return extensions; }
        }

        private readonly string languageName;

        public string LanguageName
        {
            get { return languageName; }
        }

        public SupportedSourceFile(string[] extensions, string lname)
        {
            this.extensions = extensions; 
            languageName = lname;
        }

        public static SupportedSourceFile Make(ILanguage language)
        { 
            return new SupportedSourceFile(language.FilesExtensions, language.Name);
        }
        
        public override string ToString()
        {
            return string.Format("{0} ({1})", LanguageName, FormatTools.ExtensionsToString(Extensions, "*", ";"));
        }
    }

    public delegate void ChangeCompilerStateEventDelegate(ICompiler sender, CompilerState State, string FileName);

    public class Compiler : MarshalByRefObject, ICompiler
    {
        int pABCCodeHealth = 0;
        public int PABCCodeHealth { get { return pABCCodeHealth; } }

        public static string Version
        {
            get
            {
                return RevisionClass.FullVersion;
            }
        }
        public static string ShortVersion
        {
            get
            {
                if (RevisionClass.Build == "0")
                    return RevisionClass.MainVersion;
                else
                    return RevisionClass.MainVersion + "." + RevisionClass.Build;
            }
        }
        public static DateTime VersionDateTime
        {
            get
            {
                return File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);
            }
        }
        public static string Banner
        {
            get
            {
                return "PascalABCCompiler.Core v" + Version;
            }
        }
        public override string ToString()
        {
            return Banner;
        }

        private SemanticTreeConvertersController semanticTreeConvertersController = null;
        public SemanticTreeConvertersController SemanticTreeConvertersController
        {
            get
            {
                return semanticTreeConvertersController;
            }
        }

        private Hashtable BadNodesInSyntaxTree = new Hashtable();

        program_node semanticTree = null;
        public SemanticTree.IProgramNode SemanticTree
        {
            get
            {
                return semanticTree;
            }
        }

        public List<var_definition_node> CompiledVariables = new List<var_definition_node>();

        private uint linesCompiled;
        public uint LinesCompiled
        {
            get { return linesCompiled; }
        }

        private CompilerInternalDebug internalDebug;
        public CompilerInternalDebug InternalDebug
        {
            get
            {
                return internalDebug;
            }
            set
            {
                internalDebug = value;
            }
        }

        private CompilerState state = CompilerState.Ready;
        public CompilerState State
        {
            get { return state; }
        }

        private void SetSupportedSourceFiles()
        {
            supportedSourceFiles = LanguageProvider.Languages.Select(language => SupportedSourceFile.Make(language)).ToArray();
        }

        private void SetSupportedProjectFiles()
        {
            // проекты только на Паскале пока   EVA
            supportedProjectFiles = new SupportedSourceFile[] { new SupportedSourceFile(new string[1] { ".pabcproj" }, "PascalABC.NET") };
        }

        private SupportedSourceFile[] supportedSourceFiles = null;
        public SupportedSourceFile[] SupportedSourceFiles
        {
            get { return supportedSourceFiles; }
            set { supportedSourceFiles = value; }
        }

        private SupportedSourceFile[] supportedProjectFiles = null;
        public SupportedSourceFile[] SupportedProjectFiles
        {
            get { return supportedProjectFiles; }
        }

        private CompilationUnitHashTable unitTable = new CompilationUnitHashTable();
        public CompilationUnitHashTable UnitTable { get { return unitTable; } }
        public List<CompilationUnit> UnitsTopologicallySortedList = new List<CompilationUnit>();

        private List<string> StandardModules = new List<string>();
        public CompilerOptions CompilerOptions { get; set; }

        private Dictionary<string, CompilationUnit> DLLCache = new Dictionary<string, CompilationUnit>();

        private LanguageProvider LanguageProvider => LanguageProvider.Instance;

        public TreeConverter.SyntaxTreeToSemanticTreeConverter SyntaxTreeToSemanticTreeConverter = null;
        public CodeGenerators.Controller CodeGeneratorsController = null;
        //public LLVMConverter.Controller LLVMCodeGeneratorsController = null;
        //public PascalToCppConverter.Controller PABCToCppCodeGeneratorsController = null;

        /// <summary>
        /// список отложенной компиляции реализации (она будет откомпилирована в Compile, а не в СompileUnit)
        /// </summary>
        private List<CompilationUnit> UnitsToCompileDelayedList = new List<CompilationUnit>();
        
        public Hashtable RecompileList = new Hashtable(StringComparer.OrdinalIgnoreCase);

        private CompilationUnit currentCompilationUnit = null;
        
        private CompilationUnit firstCompilationUnit = null;
        
        private bool PCUReadersAndWritersClosed;

        private static Dictionary<string, CompilationUnit> pcuCompilationUnits = new Dictionary<string, CompilationUnit>();

        private bool _clear_after_compilation = true;

        public bool ClearAfterCompilation
        {
            get
            {
                return _clear_after_compilation;
            }
            set
            {
                _clear_after_compilation = value;
            }
        }

        /// <summary>
        /// Начало основной программы
        /// </summary>
        public int beginOffset;

        public int BeginOffset
        {
            get
            {
                return beginOffset;
            }
        }

        /// <summary>
        /// Положение первых переменных в пространстве имен основной программы
        /// </summary>
        public int varBeginOffset;

        public int VarBeginOffset
        {
            get
            {
                return varBeginOffset;
            }
        }

        private List<CompilerWarning> warnings = new List<CompilerWarning>();
        
        public List<CompilerWarning> Warnings
        {
            get
            {
                return warnings;
            }
        }

        public Dictionary<Tuple<string, string>, Tuple<string, int>> SourceFileNamesDictionary { get; } = new Dictionary<Tuple<string, string>, Tuple<string, int>>();

        public Dictionary<Tuple<string, string>, Tuple<string, int>> PCUFileNamesDictionary { get; } = new Dictionary<Tuple<string, string>, Tuple<string, int>>();

        public Dictionary<Tuple<string, string>, string> GetUnitFileNameCache { get; } = new Dictionary<Tuple<string, string>, string>();

        public void AddWarnings(List<CompilerWarning> WarningList)
        {
            foreach (CompilerWarning cw in WarningList)
                warnings.Add(cw);
        }

        public event ChangeCompilerStateEventDelegate OnChangeCompilerState;
        private void ChangeCompilerStateEvent(ICompiler sender, CompilerState State, string FileName)
        {
            this.state = State;
        }

        private bool SourceFileExists(string FileName)
        {
            if (FileName == null) return false;
            return (bool)SourceFilesProvider(FileName, SourceFileOperation.Exists);
        }

        private DateTime SourceFileGetLastWriteTime(string FileName)
        {
            return (DateTime)SourceFilesProvider(FileName, SourceFileOperation.GetLastWriteTime);
        }

        private SourceFilesProviderDelegate sourceFilesProvider = SourceFilesProviders.DefaultSourceFilesProvider;
        
        public SourceFilesProviderDelegate SourceFilesProvider
        {
            get
            {
                return sourceFilesProvider;
            }
        }

        private List<Errors.Error> errorsList = new List<Errors.Error>();
        public List<Errors.Error> ErrorsList
        {
            get { return errorsList; }
        }

        public Compiler()
        {
            OnChangeCompilerState += ChangeCompilerStateEvent;
            Reload();
        }

        public Compiler(ICompiler comp, SourceFilesProviderDelegate SourceFilesProvider, ChangeCompilerStateEventDelegate ChangeCompilerState)
        {
            internalDebug = comp.InternalDebug;
            OnChangeCompilerState += ChangeCompilerStateEvent;
           
            if (SourceFilesProvider != null)
                sourceFilesProvider = SourceFilesProvider;
            
            if (ChangeCompilerState != null)
                OnChangeCompilerState += ChangeCompilerState;
            
            supportedSourceFiles = comp.SupportedSourceFiles;
            supportedProjectFiles = comp.SupportedProjectFiles;

            // 29.07.2024  EVA
            CompilerOptions = new CompilerOptions();
        }

        public Compiler(SourceFilesProviderDelegate SourceFilesProvider, ChangeCompilerStateEventDelegate ChangeCompilerState)
        {
            OnChangeCompilerState += ChangeCompilerStateEvent;
            
            if (SourceFilesProvider != null)
                sourceFilesProvider = SourceFilesProvider;
            
            if (ChangeCompilerState != null)
                OnChangeCompilerState += ChangeCompilerState;
            
            Reload();
        }

        public void Reload()
        {
            OnChangeCompilerState(this, CompilerState.Reloading, null);

            pABCCodeHealth = 0;

            ClearAll();
            errorsList.Clear();
            Warnings.Clear();
            InternalDebug = new CompilerInternalDebug();

            // 29.07.2024  EVA
            CompilerOptions = new CompilerOptions();

            SaveUnitCheckInParsers();

            SyntaxTreeToSemanticTreeConverter = new TreeConverter.SyntaxTreeToSemanticTreeConverter();
            CodeGeneratorsController = new CodeGenerators.Controller();

            SetSupportedSourceFiles();
            SetSupportedProjectFiles();

            semanticTreeConvertersController = new SemanticTreeConvertersController(this);
            semanticTreeConvertersController.ChangeState += semanticTreeConvertersController_ChangeState;
            semanticTreeConvertersController.AddConverters();

            OnChangeCompilerState(this, CompilerState.Ready, null);
        }

        /// <summary>
        /// Передаем парсерам возможность проверить, компилируется ли в данный момент модуль 
        /// (нужно, если нет ключевого слова unit или подобного в языке)
        /// </summary>
        private void SaveUnitCheckInParsers()
        {
            foreach (var parser in LanguageProvider.Languages.Select(language => language.Parser))
            {
                parser.CheckIfParsingUnit = CurrentUnitIsNotMainProgram;
            }
        }

        void semanticTreeConvertersController_ChangeState(SemanticTreeConvertersController.State State, ISemanticTreeConverter SemanticTreeConverter)
        {
            switch (State)
            {
                case SemanticTreeConvertersController.State.Convert:
                    OnChangeCompilerState(this, CompilerState.SemanticTreeConversion, SemanticTreeConverter.Name);
                    break;
                case SemanticTreeConvertersController.State.ConnectConverter:
                    OnChangeCompilerState(this, CompilerState.SemanticTreeConverterConnected, SemanticTreeConverter.Name);
                    break;
            }
        }

        #region COMPILER DIRECTIVES

        /// <summary>
        /// Формирует словарь директив компилятора, собирая их из всех переданных модулей
        /// </summary>
        /// <param name="Units"></param>
        /// <returns></returns>
        /// <exception cref="DuplicateDirective"></exception>
        private Dictionary<string, List<compiler_directive>> GetCompilerDirectives(List<CompilationUnit> Units)
        {
            Dictionary<string, List<compiler_directive>> directives = new Dictionary<string, List<compiler_directive>>(StringComparer.CurrentCultureIgnoreCase);

            for (int i = 0; i < Units.Count; i++)
            {
                common_unit_node unitNode = Units[i].SemanticTree as common_unit_node;
                if (unitNode != null)
                {
                    foreach (compiler_directive cd in unitNode.compiler_directives)
                    {
                        if (!directives.ContainsKey(cd.name))
                            directives.Add(cd.name, new List<compiler_directive>());
                        // TODO: сделать проверку на дубликаты централизованной (в другом месте)  EVA
                        else if (cd.name.Equals("mainresource", StringComparison.CurrentCultureIgnoreCase))
                            throw new DuplicateDirective(cd.location.doc.file_name, "mainresource", cd.location);
                        directives[cd.name].Insert(0, cd);
                    }
                }
            }
            return directives;
        }

        /*private Hashtable GetCompilerDirectives(CompilationUnit Unit)
        {
            Hashtable Directives = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
            TreeRealization.common_unit_node cun = Unit.SemanticTree as TreeRealization.common_unit_node;
            if (cun != null)
                foreach (TreeRealization.compiler_directive cd in cun.compiler_directives)
                    Directives[cd.name] = cd;
            return Directives;
        }*/

        /// <summary>
        /// преобразует в директивы семантического уровня | в syntax_tree_visitor такая же функция  EVA
        /// </summary>
        private List<compiler_directive> GetDirectivesAsSemanticNodes(List<SyntaxTree.compiler_directive> compilerDirectives, string unitFileName)
        {
            List<compiler_directive> list = new List<compiler_directive>();
            foreach (SyntaxTree.compiler_directive directive in compilerDirectives)
            {
                list.Add(new compiler_directive(directive.Name.text,
                    directive.Directive?.text ?? "",
                    GetLocationFromTreenode(directive, unitFileName),
                    unitFileName));
            }
            return list;
        }

        #endregion

        private TreeRealization.location GetLocationFromTreenode(SyntaxTree.syntax_tree_node tn, string FileName)
        {
            if (tn.source_context == null)
            {
                return null;
            }
            return new TreeRealization.location(tn.source_context.begin_position.line_num, tn.source_context.begin_position.column_num,
                tn.source_context.end_position.line_num, tn.source_context.end_position.column_num, new TreeRealization.document(FileName));
        }

        public void StartCompile()
        {
            System.Threading.Thread th = new System.Threading.Thread(() => Compile());
            th.SetApartmentState(System.Threading.ApartmentState.STA);
            th.Start();
        }

        public string Compile(CompilerOptions CompilerOptions)
        {
            this.CompilerOptions = CompilerOptions;
            return Compile();
        }

        private void Reset()
        {
            SourceFileNamesDictionary.Clear();
            PCUFileNamesDictionary.Clear();
            GetUnitFileNameCache.Clear();
            Warnings.Clear();
            errorsList.Clear();
            //if (!File.Exists(CompilerOptions.SourceFileName)) throw new SourceFileNotFound(CompilerOptions.SourceFileName);
            currentCompilationUnit = null;
            firstCompilationUnit = null;
            linesCompiled = 0;
            pABCCodeHealth = 0;
            PCUReadersAndWritersClosed = false;
            SyntaxTreeToSemanticTreeConverter.Reset();
            CodeGeneratorsController.Reset();
            //PABCToCppCodeGeneratorsController.Reset();
            UnitsToCompileDelayedList.Clear();
            DLLCache.Clear();
            project = null;

            // обнуляем здесь, чтобы значение не сохранялось между запусками  EVA
            CompilerOptions.DisableStandardUnits = false;
        }

        void CheckErrorsAndThrowTheFirstOne()
        {
            if (CompilerOptions.ForIntellisense)
                return;
            if (ErrorsList.Count > 0)
                throw ErrorsList[0];
        }

        /*private void MoveSystemUnitForwardInUnitsTopologicallySortedList()
        {
            if (CompilerOptions.StandardModules.Count == 0)
                return;

            CompilationUnit systemUnit = null;
            foreach (CompilationUnit unit in UnitsTopologicallySortedList)
            {
                if (unit.SemanticTree == null || !(unit.SemanticTree is common_unit_node))
                    continue;

                string unitName = (unit.SemanticTree as common_unit_node).unit_name;

                // Пока что сделана проверка для всех языков   |    Вопрос  EVA
                if (CompilerOptions.StandardModules.Select(kv => kv.Value[0].name).Contains(unitName))
                {
                    systemUnit = unit;
                    break;
                }
            }

            if (systemUnit != null && systemUnit != UnitsTopologicallySortedList[0])
            {
                UnitsTopologicallySortedList.Remove(systemUnit);
                UnitsTopologicallySortedList.Insert(0, systemUnit);
            }

        }*/

        #region LEGACY - Visual Basic and IronPython

        /*class ProgInfo
        {
            public string entry_module;
            public int entry_method_name_pos;
            public int entry_method_line;
            public int using_pos = -1;
            public List<string> modules = new List<string>();
            public List<string> addit_imports = new List<string>();
            public List<string> addit_project_files = new List<string>();
        }*/

        /*
        private void add_import_info(ProgInfo info, List<string> imports)
        {
            Hashtable ht = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
            ht["GraphABC"] = "GraphABC";
            ht["GraphABCHelper"] = "GraphABCHelper";
            ht["ABCObjects"] = "ABCObjects";
            ht["Robot"] = "Robot";
            ht["Drawman"] = "Drawman";
            ht["DrawManField"] = "DrawManField";
            foreach (string s in imports)
            {
                string low_s = s.ToLower();
                //string mod_file = FindSourceFileInDirectories(s+".mod",Path.Combine(this.CompilerOptions.SystemDirectory,"lib"));
                if (ht[s] != null)
                {
                    string name = ht[s] as string;
                    if (!info.modules.Contains(name))
                    {
                        info.modules.Add(name);
                        info.addit_imports.Add(name + "." + name);
                    }
                }
                // Этот if был закомментирован   EVA
                if (!string.IsNullOrEmpty(mod_file))
                {
                    switch (low_s)
                    {
                            case "graphabc" : 
                            if (!info.modules.Contains("GraphABC") && !info.modules.Contains("ABCObjects") && !info.modules.Contains("ABCButtons") && !info.modules.Contains("ABCHouse")
                                                 && !info.modules.Contains("ABCSprites") && !info.modules.Contains("RobotField"))
                            {
                                info.modules.Add("GraphABC");
                                info.addit_imports.Add("GraphABC.GraphABC");
                            }
                            else if (!info.addit_imports.Contains("GraphABC.GraphABC"))
                            {
                                info.addit_imports.Add("GraphABC.GraphABC");
                            }
                            break;
                            case "abcobjects" :
                            if (!info.modules.Contains("ABCObjects") && !info.modules.Contains("ABCButtons") && !info.modules.Contains("ABCHouse")
                                                 && !info.modules.Contains("ABCSprites") && !info.modules.Contains("RobotField"))
                            {
                                info.modules.Add("ABCObjects");
                                info.addit_imports.Add("ABCObjects.ABCObjects");
                                if (info.modules.Contains("GraphABC"))
                                {
                                    info.modules.Remove("GraphABC");
                                    //info.addit_imports.Remove("GraphABC.GraphABC");
                                }
                            }
                            break;
                            default:
                            if (!info.modules.Contains(s))
                            {
                                info.modules.Add(s);
                                info.addit_imports.Add(s+"."+s);
                            }
                            break;
                    }
                }
                else
                {
                    string source_file = FindFileWithExtensionInDirs(s + ".vb", out _, Path.GetDirectoryName(CompilerOptions.SourceFileName), Path.Combine(this.CompilerOptions.SystemDirectory, "lib"),
                                                                     Path.Combine(this.CompilerOptions.SystemDirectory, "LibSource"));
                    if (!string.IsNullOrEmpty(source_file))
                    {
                        if (!info.addit_project_files.Contains(source_file))
                        {
                            info.addit_project_files.Add(source_file);
                        }
                    }
                }
            }
            // весь код ниже был закомментирован  EVA
            List<string> mods = new List<string>();
            List<int> inds = new List<int>();
            for (int i=0; i<info.modules.Count; i++)
            {
                switch (info.modules[i])
                {
                    case "GraphABCHelper" : inds.Add(0); break;
                    case "GraphABC" : inds.Add(1); break;
                    case "ABCObjects" : inds.Add(2); break;
                    case "ABCHouse" : inds.Add(3); break;
                    case "ABCSprites" : inds.Add(3); break;
                    case "ABCButtons" : inds.Add(3); break;
                    case "DMCollect" : inds.Add(0); break;
                    case "DrawManField" : inds.Add(2); break;
                    case "DMZadan" : inds.Add(4); break;
                    case "DMTaskMaker" : inds.Add(3); break;
                    case "RobotField" : inds.Add(2); break;
                    case "RobotTaskMaker" : inds.Add(3); break;
                    case "RobotZadan" : inds.Add(4); break;
                    case "Robot" : inds.Add(5); break;
                }
            }
        } */

        /*private int find_pos(string s, int line, int col, bool search_main)
        {
            int ind_ln = 1;
            int ind_col = 1;
            int pos = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (ind_col == col && ind_ln == line)
                {
                    if (search_main)
                    {
                        pos += 3;
                        while (s[pos] != 'M' && s[pos] != 'm')
                            pos++;
                    }
                    return pos;
                }
                if (s[i] == '\n')
                {
                    ind_col = 1;
                    ind_ln++;
                }
                else
                {
                    ind_col++;
                }
                pos++;

            }
            return -1;
        }*/

        /*private ProgInfo get_programm_info(ICSharpCode.NRefactory.Ast.CompilationUnit cu, string source)
        {
            ProgInfo info = new ProgInfo();
            List<string> usings = new List<string>();
            foreach (ICSharpCode.NRefactory.Ast.INode node in cu.Children)
            {
                if (node is ICSharpCode.NRefactory.Ast.TypeDeclaration)
                {
                    ICSharpCode.NRefactory.Ast.TypeDeclaration td = node as ICSharpCode.NRefactory.Ast.TypeDeclaration;
                    if (td.Type == ICSharpCode.NRefactory.Ast.ClassType.Module)
                    {
                        foreach (ICSharpCode.NRefactory.Ast.INode node2 in td.Children)
                        {
                            if (node2 is ICSharpCode.NRefactory.Ast.MethodDeclaration)
                            {
                                ICSharpCode.NRefactory.Ast.MethodDeclaration meth = node2 as ICSharpCode.NRefactory.Ast.MethodDeclaration;
                                if (string.Compare(meth.Name, "Main", true) == 0 && (meth.Parameters == null || meth.Parameters.Count == 0))
                                {
                                    //info = new ProgInfo();
                                    info.entry_module = (meth.Parent as ICSharpCode.NRefactory.Ast.TypeDeclaration).Name;
                                    //info.entry_method_name_line = meth.StartLocation.Line;
                                    //info.entry_method_name_col = meth.StartLocation.Column;
                                    info.entry_method_name_pos = find_pos(source, meth.StartLocation.Line, meth.StartLocation.Column, true);
                                    if (meth.Body.Children.Count > 0)
                                    {
                                        for (int i = 0; i < meth.Body.Children.Count; i++)
                                        {
                                            if (!(meth.Body.Children[i] is ICSharpCode.NRefactory.Ast.LocalVariableDeclaration))
                                            {
                                                info.entry_method_line = meth.Body.Children[i].StartLocation.Line;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        info.entry_method_line = meth.Body.EndLocation.Line;
                                }
                            }
                        }
                    }
                }
                else if (node is ICSharpCode.NRefactory.Ast.UsingDeclaration)
                {
                    ICSharpCode.NRefactory.Ast.UsingDeclaration using_node = node as ICSharpCode.NRefactory.Ast.UsingDeclaration;

                    foreach (ICSharpCode.NRefactory.Ast.Using us in using_node.Usings)
                        if (!string.IsNullOrEmpty(us.Name))
                            usings.Add(us.Name);
                    if (info.using_pos == -1)
                        info.using_pos = find_pos(source, using_node.EndLocation.Line, using_node.EndLocation.Column, false);
                }
            }
            if (info.entry_module != null)
            {
                add_import_info(info, usings);
                return info;
            }
            return null;
        }*/

        /*public string CompileWithProvider(string[] sources, System.CodeDom.Compiler.CodeDomProvider cp, params string[] RefAssemblies)
        {
            OnChangeCompilerState(this, CompilerState.CompilationStarting, CompilerOptions.SourceFileName);
            OnChangeCompilerState(this, CompilerState.BeginCompileFile, CompilerOptions.SourceFileName);
            Reset();

            //var cscp = new Microsoft.CSharp.CSharpCodeProvider();
            var comp_opt = new CompilerParameters();
            comp_opt.IncludeDebugInformation = CompilerOptions.Debug;
            comp_opt.GenerateExecutable = true;
            comp_opt.WarningLevel = 3;
            comp_opt.CompilerOptions = "/platform:x86 ";
            comp_opt.OutputAssembly = CompilerOptions.OutputFileName;
            if (RefAssemblies != null)
                comp_opt.ReferencedAssemblies.AddRange(RefAssemblies);

            //string source = GetSourceFileText(CompilerOptions.SourceFileName);
            var res = cp.CompileAssemblyFromSource(comp_opt, sources);
            if (res.Errors.Count > 0)
            {
                for (int i = 0; i < res.Errors.Count; i++)
                {
                    if (!res.Errors[i].IsWarning && errorsList.Count == 0 *//*&& dlls.Errors[i].file_name != redirect_fname*//*)
                    {
                        if (File.Exists(res.Errors[i].FileName))
                            errorsList.Add(new Errors.CommonCompilerError(res.Errors[i].ErrorText, res.Errors[i].FileName, res.Errors[i].Line != 0 ? res.Errors[i].Line : 1, res.Errors[i].Column != 0 ? res.Errors[i].Column : 1));
                        else
                            errorsList.Add(new Errors.CommonCompilerError(res.Errors[i].ErrorText, CompilerOptions.SourceFileName, res.Errors[i].Line != 0 ? res.Errors[i].Line : 1, res.Errors[i].Column != 0 ? res.Errors[i].Column : 1));
                    }
                    else if (res.Errors[i].IsWarning)
                    {
                        warnings.Add(new Errors.CommonWarning(res.Errors[i].ErrorText, res.Errors[i].FileName, res.Errors[i].Line, res.Errors[i].Column));
                    }
                }
            }

            //linesCompiled = get_compiled_lines(CompilerOptions.SourceFileName);

            OnChangeCompilerState(this, CompilerState.CompilationFinished, CompilerOptions.SourceFileName);
            ClearAll();
            OnChangeCompilerState(this, CompilerState.Ready, null);

            if (errorsList.Count > 0)
                return null;
            else
                return res.PathToAssembly;
        }*/

        /*class PyErrorHandler : ErrorListener
        {
            Compiler c;
            public PyErrorHandler(Compiler cc) { c = cc; }
            public override void ErrorReported(ScriptSource source, string message, SourceSpan span, int errorCode, Severity severity)
            {
                if (severity == Severity.Warning)
                    c.warnings.Add(new Errors.CommonWarning(message, c.CompilerOptions.SourceFileName, span.Start.Line, span.Start.Column));
                else
                    c.errorsList.Add(new Errors.CommonCompilerError(message, c.CompilerOptions.SourceFileName, span.Start.Line, span.Start.Column));
            }
        }*/

        /*
        private Assembly IronPythonAssembly;
        private MethodInfo PythonCreateEngineMethod;
        public string CompilePy()
        {
            OnChangeCompilerState(this, CompilerState.CompilationStarting, CompilerOptions.SourceFileName);
            OnChangeCompilerState(this, CompilerState.BeginCompileFile, CompilerOptions.SourceFileName);
            Reset();

            if (IronPythonAssembly == null)
            {
                IronPythonAssembly = Assembly.LoadFrom("IronPython.dll");
                PythonCreateEngineMethod = IronPythonAssembly.GetType("IronPython.Hosting.Python").GetMethod("CreateEngine");
            }
            string source = GetSourceFileText(CompilerOptions.SourceFileName);

            ScriptEngine pyEngine = (ScriptEngine)PythonCreateEngineMethod.Invoke(null, new object[0]); ;

            ScriptSource src = pyEngine.CreateScriptSourceFromString(source);
            CompiledCode compiled = src.Compile(new PyErrorHandler(this));
            if (compiled != null)
                compiled.Execute();

            linesCompiled = get_compiled_lines(CompilerOptions.SourceFileName);

            OnChangeCompilerState(this, CompilerState.CompilationFinished, CompilerOptions.SourceFileName);
            ClearAll();
            OnChangeCompilerState(this, CompilerState.Ready, null);

            //if (errorsList.Count > 0)
            return null;
            //else return dlls.PathToAssembly;
        }*/

        /*public string CompileVB()
        {
            OnChangeCompilerState(this, CompilerState.CompilationStarting, CompilerOptions.SourceFileName);
            OnChangeCompilerState(this, CompilerState.BeginCompileFile, CompilerOptions.SourceFileName);
            Reset();
            Microsoft.VisualBasic.VBCodeProvider vbcp = new Microsoft.VisualBasic.VBCodeProvider();
            List<string> sources = new List<string>();
            sources.Add(CompilerOptions.SourceFileName);
            System.CodeDom.Compiler.CompilerParameters comp_opt = new System.CodeDom.Compiler.CompilerParameters();
            comp_opt.OutputAssembly = CompilerOptions.OutputFileName;
            comp_opt.WarningLevel = 3;
            comp_opt.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            comp_opt.ReferencedAssemblies.Add("System.Drawing.dll");
            comp_opt.ReferencedAssemblies.Add("Microsoft.VisualBasic.dll");
            comp_opt.IncludeDebugInformation = CompilerOptions.Debug;
            comp_opt.GenerateExecutable = true;
            string fname = Path.Combine(Path.GetDirectoryName(CompilerOptions.SourceFileName), "_temp$" + Path.GetFileName(CompilerOptions.SourceFileName));
            File.Copy(CompilerOptions.SourceFileName, fname, true);
            string source = GetSourceFileText(CompilerOptions.SourceFileName);

            IParser parser = ICSharpCode.NRefactory.ParserFactory.CreateParser(ICSharpCode.NRefactory.SupportedLanguage.VBNet, new StringReader(source));
            parser.Parse();
            ProgInfo info = get_programm_info(parser.CompilationUnit, source);
            parser.Dispose();
            if (info != null)
                sources.AddRange(info.addit_project_files);
            string redirect_base_fname = FindFileWithExtensionInDirs("__RedirectIOMode.vb", out _, Path.Combine(this.CompilerOptions.SystemDirectory, "Lib"), Path.Combine(this.CompilerOptions.SystemDirectory, "LibSource"));
            string system_unit_name = FindFileWithExtensionInDirs("VBSystem.vb", out _, Path.Combine(this.CompilerOptions.SystemDirectory, "lib"), Path.Combine(this.CompilerOptions.SystemDirectory, "LibSource"));
            string redirect_fname = Path.Combine(Path.GetDirectoryName(CompilerOptions.SourceFileName), "_RedirectIOMode.vb");
            StreamReader sr = File.OpenText(redirect_base_fname);
            string redirect_module = sr.ReadToEnd();
            sr.Close();
            if (info != null)
            {
                System.Text.StringBuilder tmp_sb = new System.Text.StringBuilder();
                if (info.modules.Count > 0)
                {
                    tmp_sb.AppendLine("GetType(Fictive.Fictive).Assembly.GetType(\"PABCSystem_implementation$.PABCSystem_implementation$\").GetMethod(\"$Initialization\").Invoke(Nothing,Nothing)");
                }
                for (int i = 0; i < info.modules.Count; i++)
                {
                    tmp_sb.AppendLine("GetType(Fictive.Fictive).Assembly.GetType(\"" + info.modules[i] + "_implementation$." + info.modules[i] +
                                  "_implementation$\").GetMethod(\"$Initialization\").Invoke(Nothing,Nothing)");

                }
                tmp_sb.AppendLine(info.entry_module + "." + "___Main()");
                for (int i = 0; i < info.modules.Count; i++)
                {
                    tmp_sb.AppendLine("GetType(Fictive.Fictive).Assembly.GetType(\"" + info.modules[i] + "_implementation$." + info.modules[i] +
                                  "_implementation$\").GetMethod(\"$Finalization\").Invoke(Nothing,Nothing)");

                }
                redirect_module = redirect_module.Replace("%MAIN%", tmp_sb.ToString());
            }
            StreamWriter sw = new StreamWriter(redirect_fname, false);
            if (info != null)
            {
                sw.WriteLine(redirect_module);
            }
            else
            {
                sw.WriteLine("module _RedirectIOMode");
                sw.WriteLine("End module");
            }
            sw.Close();

            sw = new StreamWriter(CompilerOptions.SourceFileName, false);
            if (info != null && info.entry_method_name_pos != -1)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                int start_pos = 0;
                if (info.using_pos != -1)
                {
                    start_pos = info.using_pos;
                    sb.Append(source.Substring(0, info.using_pos));
                    for (int i = 0; i < info.addit_imports.Count; i++)
                    {
                        sb.Append("," + info.addit_imports[i]);
                    }
                    sb.Append(",System.Collections.Generic");

                    sb.Append(",Microsoft.VisualBasic.Strings");
                    sb.Append(",Microsoft.VisualBasic.Constants");
                    sb.Append(",Microsoft.VisualBasic.VBMath");
                    sb.Append(",Microsoft.VisualBasic.Information");
                    sb.Append(",Microsoft.VisualBasic.Interaction");
                    sb.Append(",Microsoft.VisualBasic.FileSystem");
                    sb.Append(",Microsoft.VisualBasic.Financial");
                    sb.Append(",Microsoft.VisualBasic.DateAndTime");
                }
                else
                {
                    start_pos = 0;
                    sb.Append("Imports System");
                    sb.Append(",System.Collections.Generic");
                    sb.Append(",Microsoft.VisualBasic.Strings");
                    sb.Append(",Microsoft.VisualBasic.Constants");
                    sb.Append(",Microsoft.VisualBasic.VBMath");
                    sb.Append(",Microsoft.VisualBasic.Information");
                    sb.Append(",Microsoft.VisualBasic.Interaction");
                    sb.Append(",Microsoft.VisualBasic.FileSystem");
                    sb.Append(",Microsoft.VisualBasic.Financial");
                    sb.Append(",Microsoft.VisualBasic.DateAndTime");
                    sb.Append(":");
                }
                sb.Append(source.Substring(start_pos, info.entry_method_name_pos - start_pos));
                sb.Append("___");
                sb.Append(source.Substring(info.entry_method_name_pos));
                sw.Write(sb.ToString());
            }
            else
            {
                sw.Write(source);
            }
            sw.Close();

            if (info != null && info.modules.Count > 0)
            {
                comp_opt.ReferencedAssemblies.Add(Path.Combine(Path.GetDirectoryName(CompilerOptions.SourceFileName), StringConstants.pabc_rtl_dll_name));
                string mod_file_name = FindFileWithExtensionInDirs("PABCRtl.dll", out _, Path.Combine(this.CompilerOptions.SystemDirectory, "Lib"));
                File.Copy(mod_file_name, Path.Combine(Path.GetDirectoryName(CompilerOptions.SourceFileName), "PABCRtl.dll"), true);
                *//*foreach (string mod in info.modules)
                {
                    comp_opt.ReferencedAssemblies.Add(Path.Combine(Path.GetDirectoryName(CompilerOptions.SourceFileName),mod+".dll"));
                    string mod_file_name = FindSourceFileInDirectories(mod+".mod",Path.Combine(this.CompilerOptions.SystemDirectory,"lib"));
                    File.Copy(mod_file_name,Path.Combine(Path.GetDirectoryName(CompilerOptions.SourceFileName),mod+".dll"),true);
                }*//*
            }
            sources.Add(redirect_fname);
            sources.Add(system_unit_name);
            System.CodeDom.Compiler.CompilerResults res = vbcp.CompileAssemblyFromFile(comp_opt, sources.ToArray());
            if (res.Errors.Count > 0)
            {
                for (int i = 0; i < res.Errors.Count; i++)
                {
                    if (!res.Errors[i].IsWarning && errorsList.Count == 0 *//*&& dlls.Errors[i].file_name != redirect_fname*//*)
                    {
                        if (File.Exists(res.Errors[i].FileName))
                            errorsList.Add(new Errors.CommonCompilerError(res.Errors[i].ErrorText, res.Errors[i].FileName, res.Errors[i].Line != 0 ? res.Errors[i].Line : 1, 1));
                        else
                            errorsList.Add(new Errors.CommonCompilerError(res.Errors[i].ErrorText, CompilerOptions.SourceFileName, res.Errors[i].Line != 0 ? res.Errors[i].Line : 1, 1));
                    }
                    else if (res.Errors[i].IsWarning)
                    {
                        warnings.Add(new Errors.CommonWarning(res.Errors[i].ErrorText, res.Errors[i].FileName, res.Errors[i].Line, 1));
                    }
                }
            }
            linesCompiled = get_compiled_lines(CompilerOptions.SourceFileName);
            if (info != null)
            {
                beginOffset = info.entry_method_line;
                //if (info.using_pos == -1)
                //	beginOffset -= 1;
            }
            OnChangeCompilerState(this, CompilerState.CompilationFinished, CompilerOptions.SourceFileName);
            ClearAll();
            File.Copy(fname, CompilerOptions.SourceFileName, true);
            File.Delete(fname);
            File.Delete(redirect_fname);
            OnChangeCompilerState(this, CompilerState.Ready, null);
            if (errorsList.Count > 0)
                return null;
            else
                return res.PathToAssembly;
        }*/

        #endregion

        public string CompileCS()
        {
            OnChangeCompilerState(this, CompilerState.CompilationStarting, CompilerOptions.SourceFileName);
            OnChangeCompilerState(this, CompilerState.BeginCompileFile, CompilerOptions.SourceFileName);
            Reset();
            var d = new Dictionary<string, string>();
            d["CompilerVersion"] = "v4.0";
            var cscp = new Microsoft.CSharp.CSharpCodeProvider(d);

            //var cscp = new Microsoft.CSharp.CSharpCodeProvider();
            var comp_opt = new CompilerParameters();
            comp_opt.IncludeDebugInformation = CompilerOptions.Debug;
            comp_opt.GenerateExecutable = true;
            comp_opt.WarningLevel = 3;
            comp_opt.OutputAssembly = CompilerOptions.OutputFileName;

            //comp_opt.ReferencedAssemblies.Add()

            string source = GetSourceFileText(CompilerOptions.SourceFileName);

            using (StringReader sr = new StringReader(source))
            {
                do
                {
                    var s = sr.ReadLine();

                    if (s == null)
                        break;

                    if (s.ToLower().StartsWith("//#reference "))
                    {
                        s = s.Remove(0, 13);
                        s = s.Trim();
                        comp_opt.ReferencedAssemblies.Add(s);
                    }
                    else break;

                } while (true);
            }


            var res = cscp.CompileAssemblyFromSource(comp_opt, source);
            if (res.Errors.Count > 0)
            {
                for (int i = 0; i < res.Errors.Count; i++)
                {
                    if (!res.Errors[i].IsWarning && errorsList.Count == 0 /*&& dlls.Errors[i].file_name != redirect_fname*/)
                    {
                        if (File.Exists(res.Errors[i].FileName))
                            errorsList.Add(new Errors.CommonCompilerError(res.Errors[i].ErrorText, res.Errors[i].FileName, res.Errors[i].Line != 0 ? res.Errors[i].Line : 1, res.Errors[i].Column != 0 ? res.Errors[i].Column : 1));
                        else
                            errorsList.Add(new Errors.CommonCompilerError(res.Errors[i].ErrorText, CompilerOptions.SourceFileName, res.Errors[i].Line != 0 ? res.Errors[i].Line : 1, res.Errors[i].Column != 0 ? res.Errors[i].Column : 1));
                    }
                    else if (res.Errors[i].IsWarning)
                    {
                        warnings.Add(new Errors.CommonWarning(res.Errors[i].ErrorText, res.Errors[i].FileName, res.Errors[i].Line, res.Errors[i].Column));
                    }
                }
            }

            linesCompiled = (uint)File.ReadAllLines(CompilerOptions.SourceFileName).Length;

            OnChangeCompilerState(this, CompilerState.CompilationFinished, CompilerOptions.SourceFileName);
            ClearAll();
            OnChangeCompilerState(this, CompilerState.Ready, null);

            if (errorsList.Count > 0)
                return null;
            else
                return res.PathToAssembly;
        }

        private ProjectInfo project;

        private void InitializeProjectInfoAndFillCompilerOptionsFromIt()
        {
            project = new ProjectInfo();
            project.Load(CompilerOptions.SourceFileName);
            //LoadProject(CompilerOptions.SourceFileName);
            switch (project.ProjectType)
            {
                case ProjectType.ConsoleApp: CompilerOptions.OutputFileType = CompilerOptions.OutputType.ConsoleApplicaton; break;
                case ProjectType.WindowsApp: CompilerOptions.OutputFileType = CompilerOptions.OutputType.WindowsApplication; break;
                case ProjectType.Library: CompilerOptions.OutputFileType = CompilerOptions.OutputType.ClassLibrary; break;
            }
            CompilerOptions.SourceFileName = project.main_file;
            CompilerOptions.Debug = project.include_debug_info;
            CompilerOptions.OutputFileName = project.output_file_name;

            CompilerOptions.OutputDirectory = project.output_directory;


        }

        public static bool CheckPathValid(string path)
        {
            return !Path.GetInvalidPathChars().Any(path.Contains);
        }

        public static void TryThrowInvalidPath(string path, SyntaxTree.SourceContext loc)
        {
            if (CheckPathValid(path)) return;
            throw new InvalidPathError(loc);
        }

        private void CompileUnitsFromDelayedList()
        {
            // проход по всем юнитам из списка отложенной компиляции
            foreach (CompilationUnit CurrentUnit in UnitsToCompileDelayedList)
            {
                if (CurrentUnit.State != UnitState.Compiled)
                {
                    currentCompilationUnit = CurrentUnit;
                    string unitFileName = currentCompilationUnit.SyntaxTree.file_name;

                    // получение списка используемых модулей в файле (uses 1, 2, 3...)
                    List<SyntaxTree.unit_or_namespace> implementationUsesList = GetImplementationUsesSection(CurrentUnit.SyntaxTree);

                    CurrentUnit.possibleNamespaces.Clear();
                    if (HasIncludeNamespaceDirective(CurrentUnit))
                        CompilerOptions.UseDllForSystemUnits = false;

                    if (implementationUsesList != null)
                    {
                        SetUseDLLForSystemUnits(Path.GetDirectoryName(unitFileName), implementationUsesList, implementationUsesList.Count - 1);

                        for (int i = implementationUsesList.Count - 1; i >= 0; i--)
                        {
                            if (!IsPossibleNetNamespaceOrStandardPasFile(implementationUsesList[i], true, Path.GetDirectoryName(unitFileName)))
                            {
                                // докомпилируем юнит, если он не является пространством имен или стандартным pas файлом из Lib
                                CompileUnit(CurrentUnit.ImplementationUsedUnits, CurrentUnit.ImplementationUsedDirectUnits, implementationUsesList[i], Path.GetDirectoryName(unitFileName));
                            }
                            else
                            {
                                // добавление в списки только пространств имен
                                CurrentUnit.ImplementationUsedUnits.AddElement(new namespace_unit_node(GetNamespace(implementationUsesList[i])), null);
                                CurrentUnit.possibleNamespaces.Add(implementationUsesList[i]);
                            }
                        }

                    }

                    AddNamespacesToUsingList(CurrentUnit.ImplementationUsingNamespaceList, CurrentUnit.possibleNamespaces, true, null);

                    // Console.WriteLine("Compiling implementation delayed " + unitFileName);

                    CompileCurrentUnitImplementation(unitFileName, CurrentUnit, null);

                    CurrentUnit.State = UnitState.Compiled; // отметка о скомпилированности
                    OnChangeCompilerState(this, CompilerState.EndCompileFile, unitFileName); // состояние конец компиляции
                    //SavePCU(compilationUnit, unitFileName);
                    CurrentUnit.UnitFileName = unitFileName;
                }
            }
        }

        private void SetOutputFileTypeOption(Dictionary<string, List<TreeRealization.compiler_directive>> compilerDirectives)
        {
            if (compilerDirectives.ContainsKey(StringConstants.compiler_directive_apptype))
            {
                string outputFileType = compilerDirectives[StringConstants.compiler_directive_apptype][0].directive.ToLower();
                switch (outputFileType)
                {
                    case "console":
                        CompilerOptions.OutputFileType = CompilerOptions.OutputType.ConsoleApplicaton;
                        break;
                    case "windows":
                        CompilerOptions.OutputFileType = CompilerOptions.OutputType.WindowsApplication;
                        break;
                    case "dll":
                        CompilerOptions.OutputFileType = CompilerOptions.OutputType.ClassLibrary;
                        break;
                    case "pcu":
                        CompilerOptions.OutputFileType = CompilerOptions.OutputType.PascalCompiledUnit;
                        break;
                    default:
                        ErrorsList.Add(new UnsupportedOutputFileType(outputFileType, compilerDirectives[StringConstants.compiler_directive_apptype][0].location));
                        break;
                }
            }

            // передача информации о типе выходного файла системному юниту
            if (UnitsTopologicallySortedList.Count > 0)
            {
                bool isConsoleApplication = CompilerOptions.OutputFileType == CompilerOptions.OutputType.ConsoleApplicaton;
                common_unit_node systemUnit = UnitsTopologicallySortedList[0].SemanticTree as common_unit_node;
                systemUnit.IsConsoleApplicationVariable = isConsoleApplication;
            }
        }

        private void SetOutputPlatformOption(NETGenerator.CompilerOptions netCompilerOptions, Dictionary<string, List<TreeRealization.compiler_directive>> compilerDirectives)
        {
            List<compiler_directive> compilerDirectivesList = new List<compiler_directive>();
            if (compilerDirectives.TryGetValue(StringConstants.compiler_directive_platformtarget, out compilerDirectivesList))
            {
                string platformName = compilerDirectivesList[0].directive.ToLower();
                switch (platformName)
                {
                    case "x86":
                        netCompilerOptions.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.x86;
                        break;
                    case "x64":
                        netCompilerOptions.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.x64;
                        break;
                    case "anycpu":
                        netCompilerOptions.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.AnyCPU;
                        break;
                    case "dotnet5win":
                        netCompilerOptions.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.dotnet5win;
                        break;
                    case "dotnet5linux":
                        netCompilerOptions.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.dotnet5linux;
                        break;
                    case "dotnet5macos":
                        netCompilerOptions.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.dotnet5macos;
                        break;
                    case "native":
                        if (Environment.OSVersion.Platform == PlatformID.Unix)
                        {
                            netCompilerOptions.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.dotnetlinuxnative;
                        }
                        else if (Environment.OSVersion.Platform == PlatformID.MacOSX)
                        {
                            netCompilerOptions.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.dotnetmacosnative;
                        }
                        else
                        {
                            netCompilerOptions.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.dotnetwinnative;
                        }
                        break;
                    default:
                        ErrorsList.Add(new UnsupportedTargetPlatform(platformName, compilerDirectivesList[0].location));
                        break;
                }
                if (CompilerOptions.Only32Bit)
                    netCompilerOptions.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.x86;
            }
            if (compilerDirectives.TryGetValue(StringConstants.compiler_directive_targetframework, out compilerDirectivesList))
            {
                netCompilerOptions.TargetFramework = compilerDirectivesList[0].directive;
                if (!(new string[] { "net40", "net403", "net45", "net451", "net452", "net46", "net461", "net462", "net47", "net471", "net472", "net48", "net481" })
                    .Contains(netCompilerOptions.TargetFramework))
                {
                    ErrorsList.Add(new UnsupportedTargetFramework(netCompilerOptions.TargetFramework, compilerDirectivesList[0].location));
                }
            }
        }

        private void FillNetCompilerOptionsFromCompilerDirectives(NETGenerator.CompilerOptions netCompilerOptions, Dictionary<string, List<TreeRealization.compiler_directive>> compilerDirectives)
        {
            List<compiler_directive> compilerDirectivesList;
            
            if (compilerDirectives.TryGetValue(StringConstants.compiler_directive_product_string, out compilerDirectivesList))
            {
                netCompilerOptions.Product = compilerDirectivesList[0].directive;
            }
            if (compilerDirectives.TryGetValue(StringConstants.compiler_directive_version_string, out compilerDirectivesList))
            {
                netCompilerOptions.ProductVersion = compilerDirectivesList[0].directive;
            }
            if (compilerDirectives.TryGetValue(StringConstants.compiler_directive_company_string, out compilerDirectivesList))
            {
                netCompilerOptions.Company = compilerDirectivesList[0].directive;
            }
            if (compilerDirectives.TryGetValue(StringConstants.compiler_directive_trademark_string, out compilerDirectivesList))
            {
                netCompilerOptions.TradeMark = compilerDirectivesList[0].directive;
            }
            if (compilerDirectives.TryGetValue(StringConstants.compiler_directive_copyright_string, out compilerDirectivesList))
            {
                netCompilerOptions.Copyright = compilerDirectivesList[0].directive;
            }
            if (compilerDirectives.TryGetValue(StringConstants.compiler_directive_title_string, out compilerDirectivesList))
            {
                netCompilerOptions.Title = compilerDirectivesList[0].directive;
            }
            if (compilerDirectives.TryGetValue(StringConstants.compiler_directive_description_string, out compilerDirectivesList))
            {
                netCompilerOptions.Description = compilerDirectivesList[0].directive;
            }
            if (compilerDirectives.TryGetValue(StringConstants.compiler_directive_main_resource_string, out compilerDirectivesList))
            {
                if (compilerDirectives.ContainsKey(StringConstants.compiler_directive_product_string) ||
                    compilerDirectives.ContainsKey(StringConstants.compiler_directive_version_string) ||
                    compilerDirectives.ContainsKey(StringConstants.compiler_directive_company_string) ||
                    compilerDirectives.ContainsKey(StringConstants.compiler_directive_trademark_string) ||
                    compilerDirectives.ContainsKey(StringConstants.compiler_directive_title_string) ||
                    compilerDirectives.ContainsKey(StringConstants.compiler_directive_description_string) ||
                    compilerDirectives.ContainsKey(StringConstants.compiler_directive_copyright_string))
                {
                    ErrorsList.Add(new MainResourceNotAllowed(compilerDirectivesList[0].location.doc.file_name, compilerDirectivesList[0].location));
                }
                TryThrowInvalidPath(compilerDirectivesList[0].directive, compilerDirectivesList[0].location);
                // Тут не обязательно нормализовывать путь
                // И если он слишком длинный - File.Exists вернёт false
                netCompilerOptions.MainResourceFileName = Path.Combine(Path.GetDirectoryName(compilerDirectivesList[0].source_file), compilerDirectivesList[0].directive);
                if (!File.Exists(netCompilerOptions.MainResourceFileName))
                    ErrorsList.Add(new ResourceFileNotFound(compilerDirectivesList[0].location.doc.file_name, compilerDirectivesList[0].directive, compilerDirectivesList[0].location));
            }

        }

        private void FillNetCompilerOptionsFromProject(NETGenerator.CompilerOptions netCompilerOptions)
        {
            if (project != null)
            {
                if (!(project.major_version == 0 && project.minor_version == 0 && project.build_version == 0 && project.revision_version == 0))
                    netCompilerOptions.ProductVersion = project.major_version + "." + project.minor_version + "." + project.build_version + "." + project.revision_version;

                if (!string.IsNullOrEmpty(project.product))
                    netCompilerOptions.Product = project.product;

                if (!string.IsNullOrEmpty(project.company))
                    netCompilerOptions.Company = project.company;

                if (!string.IsNullOrEmpty(project.trademark))
                    netCompilerOptions.TradeMark = project.trademark;

                if (!string.IsNullOrEmpty(project.copyright))
                    netCompilerOptions.Copyright = project.copyright;

                if (!string.IsNullOrEmpty(project.title))
                    netCompilerOptions.Title = project.title;

                if (!string.IsNullOrEmpty(project.description))
                    netCompilerOptions.Description = project.description;

                if (project.ProjectType == ProjectType.WindowsApp)
                    netCompilerOptions.target = NETGenerator.TargetType.WinExe;

                // при использовании учесть удаление res_file из этой функции при кодогенерации  EVA
                // CreateRCFile(compilerOptions);
            }
        }

        /*private void CreateRCFile(NETGenerator.CompilerOptions compilerOptions)
        {
            if (!string.IsNullOrEmpty(project.app_icon))
            {
                //cdo.MainResourceFileName = project.app_icon;
                string rc_file = Path.GetFileNameWithoutExtension(project.app_icon) + ".rc";
                StreamWriter sw = File.CreateText(rc_file);
                sw.WriteLine("1 ICON \"" + project.app_icon.Replace("\\", "\\\\") + "\"");
                if (compilerOptions.NeedDefineVersionInfo)
                {
                    compilerOptions.NeedDefineVersionInfo = false;
                    sw.WriteLine("1 VERSIONINFO");
                    string ver = project.major_version + "," + project.minor_version + "," + project.build_version + "," + project.revision_version;
                    sw.WriteLine("FILEVERSION " + ver);

                    // это было закомментировано *****   EVA
                    sw.WriteLine("FILEFLAGSMASK VS_FFI_FILEFLAGSMASK");
                    sw.WriteLine("FILEFLAGS VER_DEBUG");
                    sw.WriteLine("FILEOS VOS__WINDOWS32");
                    if (project.project_type != ProjectType.Library)
                        sw.WriteLine("FILETYPE VFT_APP");
                    else
                        sw.WriteLine("FILETYPE VFT_DLL");
                    sw.WriteLine("FILESUBTYPE VFT2_UNKNOWN");
                    // до сюда *******

                    sw.WriteLine("BEGIN \r\n BLOCK \"StringFileInfo\"\r\n BEGIN \r\n BLOCK \"041904E3\"\r\nBEGIN");
                    sw.WriteLine("VALUE \"ProductName\"," + "\"" + compilerOptions.Product + "\"");
                    sw.WriteLine("VALUE \"FileVersion\"," + "\"" + ver + "\"");
                    sw.WriteLine("VALUE \"ProductVersion\"," + "\"" + ver + "\"");
                    sw.WriteLine("VALUE \"FileDescription\"," + "\"" + compilerOptions.Description + "\"");
                    sw.WriteLine("VALUE \"OriginalFileName\"," + "\"" + Path.GetFileName(CompilerOptions.OutputFileName) + "\"");
                    sw.WriteLine("VALUE \"InternalName\"," + "\"" + Path.GetFileNameWithoutExtension(CompilerOptions.OutputFileName) + "\"");
                    sw.WriteLine("VALUE \"CompanyName\"," + "\"" + compilerOptions.Company + "\"");
                    sw.WriteLine("VALUE \"LegalTrademarks1\"," + "\"" + compilerOptions.TradeMark + "\"");
                    sw.WriteLine("VALUE \"LegalCopyright\"," + "\"" + compilerOptions.Copyright + "\"");
                    sw.WriteLine("END");
                    sw.WriteLine("END");

                    sw.WriteLine("BLOCK \"VarFileInfo\"\r\nBEGIN");
                    sw.WriteLine("VALUE \"Translation\", 0x0419, 1251");
                    sw.WriteLine("END");
                    sw.WriteLine("END");
                }
                sw.Close();
                System.Diagnostics.Process prc = new System.Diagnostics.Process();
                prc.StartInfo.FileName = Path.Combine(this.CompilerOptions.SystemDirectory, "rc.exe");
                prc.StartInfo.Arguments = Path.Combine(Path.GetDirectoryName(project.app_icon), Path.GetFileNameWithoutExtension(project.app_icon) + ".rc");
                prc.StartInfo.CreateNoWindow = true;
                prc.StartInfo.UseShellExecute = false;
                prc.StartInfo.RedirectStandardOutput = true;
                prc.StartInfo.RedirectStandardError = true;
                prc.Start();
                prc.WaitForExit();
                string res_file = Path.Combine(Path.GetDirectoryName(project.app_icon), Path.GetFileNameWithoutExtension(project.app_icon) + ".res");
                if (File.Exists(res_file))
                {
                    compilerOptions.MainResourceFileName = res_file; // !!! главный ресурсный файл
                }
                File.Delete(rc_file);
            }
        }*/

        private void SetTargetTypeOption(NETGenerator.CompilerOptions netCompilerOptions)
        {
            netCompilerOptions.ForRunningWithEnvironment = CompilerOptions.RunWithEnvironment;

            // тип выходного файла
            if (netCompilerOptions.target == NETGenerator.TargetType.Exe) // если еще не установлен согласно проекту
            {
                switch (CompilerOptions.OutputFileType)
                {
                    case CompilerOptions.OutputType.ClassLibrary: netCompilerOptions.target = NETGenerator.TargetType.Dll; break;
                    case CompilerOptions.OutputType.ConsoleApplicaton: netCompilerOptions.target = NETGenerator.TargetType.Exe; break;
                    case CompilerOptions.OutputType.WindowsApplication: netCompilerOptions.target = NETGenerator.TargetType.WinExe; break;
                }
            }

            // Debug / Release
            netCompilerOptions.dbg_attrs = CompilerOptions.Debug ? NETGenerator.DebugAttributes.Debug : NETGenerator.DebugAttributes.Release;
            if (CompilerOptions.ForDebugging)
                netCompilerOptions.dbg_attrs = NETGenerator.DebugAttributes.ForDebugging;
        }

        public string Compile()
        {
            try
            {
                // компиляция C#
                if (Path.GetExtension(CompilerOptions.SourceFileName) == ".cs")
                {
                    return CompileCS();
                }

                // вызов события смены состояния компилятора - начало компиляции
                // информация о состояниях выводится в сообщениях компилятора
                OnChangeCompilerState(this, CompilerState.CompilationStarting, CompilerOptions.SourceFileName);

                // очистка всех переменных и списков, используемых в процессе
                Reset();

                // если проект скомпилирован, то заполнение информации о проекте в опциях компилятора
                if (CompilerOptions.ProjectCompiled)
                {
                    InitializeProjectInfoAndFillCompilerOptionsFromIt();
                }

                #region CONSTRUCTING SYNTAX AND SEMANTIC TREES

                // компиляция всех юнитов произойдет рекурсивно (кроме отложенных)
                CompileUnit(
                    new unit_node_list(),
                    new Dictionary<unit_node, CompilationUnit>(),
                    new SyntaxTree.uses_unit_in(null, new SyntaxTree.string_const(Path.GetFullPath(CompilerOptions.SourceFileName))),
                    null);

                // компиляция юнитов из списка отложенной компиляции, если он не пуст
                CompileUnitsFromDelayedList();
                #endregion

                // Закрытие чтения и записи .pcu файлов
                ClosePCUReadersAndWriters();

                PrebuildMainSemanticTreeActions(out var netCompilerOptions, out var resourceFiles);

                #region GENERATING CODE
                if (ErrorsList.Count == 0)
                {

                    //TODO: Разобратся c location для program_node и правильно передавать main_function. Добавить генератор main_function в SyntaxTreeToSemanticTreeConverter. | Отложено на потом  EVA
                    // получние полного семантического дерева, включающего все зависимости
                    program_node semanticTree = ConstructMainSemanticTree(netCompilerOptions); 

                    if (firstCompilationUnit.SyntaxTree is SyntaxTree.unit_module && CompilerOptions.OutputFileType != CompilerOptions.OutputType.ClassLibrary)
                    {
                        // если мы комилируем PCU
                        CompilerOptions.OutputFileType = CompilerOptions.OutputType.PascalCompiledUnit;
                    }
                    // генерация IL кода
                    else if (CompilerOptions.GenerateCode)
                    {
                        if (CompilerOptions.UseDllForSystemUnits)
                            netCompilerOptions.RtlPABCSystemType = NetHelper.NetHelper.FindRtlType("PABCSystem.PABCSystem");

                        GenerateILCode(semanticTree, netCompilerOptions, resourceFiles);
                    }
                }
                #endregion

            }
            // TODO: просмотреть возможные ParserError   EVA
            catch (TreeConverter.ParserError)
            {
                // конвертор уткнулся в ошибку. ничего не делаем
            }
            catch (CompilerInternalError err)
            {
                AddInternalErrorToErrorList(err);
            }
            catch (Error err)
            {
                // здесь учитывается позиция Located error
                AddErrorToErrorListConsideringPosition(err);
            }
            catch (Exception err)
            {
                // здесь добавляются только ошибки генерации кода
                AddCodeGenerationErrorToErrorList(err);
            }

            // на случай если мы вывалились по исключению, но у нас есть откомпилированные модули
            try
            {
                ClosePCUReadersAndWriters();
            }
            catch (Exception e)
            {
                ErrorsList.Add(new CompilerInternalError("Compiler.ClosePCUReadersAndWriters", e));
            }

            // если есть семантические ошибки в RTL, то очистить ошибки и повторно перекомпилировать без RTL
            bool recompilationNeeded = CheckForRTLErrorsAndClearAllErrorsIfFound();

            OnChangeCompilerState(this, CompilerState.CompilationFinished, CompilerOptions.SourceFileName); // compilation finished state

            if (ClearAfterCompilation)
                ClearAll();

            if (!recompilationNeeded)
                OnChangeCompilerState(this, CompilerState.Ready, null); // компилятор окончательно завершил работу

            if (ErrorsList.Count > 0)
            {
                return null;
            }
            else if (recompilationNeeded)
            {
                //Compiler c = new Compiler(sourceFilesProvider,OnChangeCompilerState);
                //return c.Compile(this.compilerOptions);
                return Compile();
            }
            else return CompilerOptions.OutputFileName;
        }

        /// <summary>
        /// Сохраняет документацию для модулей;
        /// Выясняет тип выходного файла, целевой фреймворк, платформу;
        /// Заполняет опции .NET компиляции согласно директивам и/или информации из проекта;
        /// Находит ресурсные файлы из директив
        /// </summary>
        private void PrebuildMainSemanticTreeActions(out NETGenerator.CompilerOptions netCompilerOptions, out List<string> resourceFiles)
        {
            if (CompilerOptions.SaveDocumentation)
            {
                SaveDocumentationsForUnits();
            }

            Dictionary<string, List<TreeRealization.compiler_directive>> compilerDirectives = GetCompilerDirectives(UnitsTopologicallySortedList);

            // выяснение типа выходного файла по соотв. директиве компилятора
            SetOutputFileTypeOption(compilerDirectives);

            // перемещаем PABCSystem в начало списка
            // MoveSystemUnitForwardInUnitsTopologicallySortedList();

            netCompilerOptions = new NETGenerator.CompilerOptions();

            // выяснение TargetFramework и целевой платформы
            SetOutputPlatformOption(netCompilerOptions, compilerDirectives);

            // заполнение опций компилятора из директив
            FillNetCompilerOptionsFromCompilerDirectives(netCompilerOptions, compilerDirectives);

            // получние путей к файлам ресурсов из директив
            resourceFiles = GetResourceFilesFromCompilerDirectives(compilerDirectives);

            // заполнение опций компилятора из заголовка проекта
            FillNetCompilerOptionsFromProject(netCompilerOptions);

            // Устанавливает опции компилятора, связанные с типом выходного файла
            SetTargetTypeOption(netCompilerOptions);
        }

        private program_node ConstructMainSemanticTree(NETGenerator.CompilerOptions compilerOptions)
        {
            program_node mainSemanticTree = new program_node(null, null);

            for (int i = 0; i < UnitsTopologicallySortedList.Count; i++)
                mainSemanticTree.units.AddElement(UnitsTopologicallySortedList[i].SemanticTree as common_unit_node);

            bool targetTypeIsExe = compilerOptions.target == NETGenerator.TargetType.Exe || compilerOptions.target == NETGenerator.TargetType.WinExe;

            // если компилируем exe или WinExe (первый модуль - основная программа)
            if (firstCompilationUnit.SyntaxTree is SyntaxTree.program_module && targetTypeIsExe && UnitsTopologicallySortedList.Count > 0)
            {
                mainSemanticTree.main_function = ((common_unit_node)UnitsTopologicallySortedList.Last().SemanticTree).main_function;

                PrepareFinalMainFunctionForExe(mainSemanticTree);
            }
            // если мы компилируем dll
            else if (firstCompilationUnit.SyntaxTree is SyntaxTree.unit_module)
            {
                // TODO: посмотреть инициализирующий код .dll  EVA
                mainSemanticTree.create_main_function_as_in_module();
            }

            if (CompilerOptions.GenerateCode) // семантические преобразования с оптимизацией кода
                mainSemanticTree = semanticTreeConvertersController.Convert(mainSemanticTree) as program_node;

            semanticTree = mainSemanticTree;
            return mainSemanticTree;
        }

        public void PrepareFinalMainFunctionForExe(program_node mainSemanticTree)
        {
            // вычисляем номер строку первой переменной и строки с началом основной программы
            if (mainSemanticTree.main_function.function_code.location != null)
            {
                common_namespace_node main_ns = mainSemanticTree.main_function.namespace_node;

                foreach (namespace_variable variable in main_ns.variables)
                {
                    if (variable.inital_value?.location != null && !(variable.inital_value is constant_node)
                        && !(variable.inital_value is record_initializer) && !(variable.inital_value is array_initializer))
                    {
                        varBeginOffset = variable.inital_value.location.begin_line_num;
                        break;
                    }
                }
                beginOffset = mainSemanticTree.main_function.function_code.location.begin_line_num;
            }

            // локализация
            Dictionary<string, object> config_dict = new Dictionary<string, object>();
            if (CompilerOptions.Locale != null && StringResourcesLanguage.GetLCIDByTwoLetterISO(CompilerOptions.Locale) != null)
            {
                config_dict["locale"] = CompilerOptions.Locale;
                config_dict["full_locale"] = StringResourcesLanguage.GetLCIDByTwoLetterISO(CompilerOptions.Locale);
            }
            mainSemanticTree.create_main_function(StandardModules.ToArray(), config_dict);
        }

        private void AddErrorToErrorListConsideringPosition(Error err)
        {
            if (ErrorsList.Count == 0)
                ErrorsList.Add(err);
            else if (err != ErrorsList[0])
            {
                if (err is SemanticError)
                {
                    int position = FindPositionForSemanticErrorInTheErrorList(err); // семантические ошибки отсортированы по location

                    ErrorsList.Insert(position, err);
                }
                else ErrorsList.Add(err);
            }
        }

        private int FindPositionForSemanticErrorInTheErrorList(Error err)
        {
            int position = ErrorsList.Count;

            SourceLocation location = (err as SemanticError).SourceLocation;
            SourceLocation locationTemp;
            if (location != null)
            {
                for (int i = 0; i < ErrorsList.Count; i++)
                {
                    if ((locationTemp = (ErrorsList[i] as LocatedError)?.SourceLocation) != null)
                    {
                        if (locationTemp > location)
                        {
                            position = i;
                            break;
                        }
                    }
                }
            }

            return position;
        }

        private void AddCodeGenerationErrorToErrorList(Exception err)
        {
            string fileName = Path.GetFileName(currentCompilationUnit?.SyntaxTree?.file_name) ?? "Compiler";
            CompilerInternalError compilationError = new CompilerInternalError(string.Format("Compiler.Compile[{0}]", fileName), err);

            AddInternalErrorToErrorList(compilationError);
        }

        private void AddInternalErrorToErrorList(CompilerInternalError internalError)
        {
            if (ErrorsList.Count == 0)
                ErrorsList.Add(internalError);
            else
            {
#if DEBUG
                if (!InternalDebug.SkipInternalErrorsIfSyntaxTreeIsCorrupt)
                    ErrorsList.Add(internalError);
#endif
            }
        }


        private bool CheckForRTLErrorsAndClearAllErrorsIfFound()
        {
            bool anyRTLErrors = false;

            if (ErrorsList.Count > 0)
            {
                if (CompilerOptions.UseDllForSystemUnits && !HasOnlySyntaxErrors(ErrorsList) && CompilerOptions.IgnoreRtlErrors)
                {
                    CompilerOptions.UseDllForSystemUnits = false;
                    ErrorsList.Clear();

                    anyRTLErrors = true;

                }
            }

            return anyRTLErrors;
        }

        private void GenerateILCode(program_node programNode, NETGenerator.CompilerOptions compilerOptions, List<string> resourceFiles)
        {

            if (CompilerOptions.OutputFileType != CompilerOptions.OutputType.SemanticTree)
#if DEBUG
                if (InternalDebug.CodeGeneration)
#endif
                {
                    // генерация файла .pdb для дебага
                    DebugOutputFileCreationUsingPDB();

                    OnChangeCompilerState(this, CompilerState.CodeGeneration, CompilerOptions.OutputFileName); // состояние генерации кода

                    // трансляция в IL-код | В semanticTree находится ЕДИНСТВЕННОЕ семантическое дерево, содержащее программу и все семантические модули
                    CodeGeneratorsController.GenerateILCodeAndSaveAssembly(programNode, CompilerOptions.OutputFileName,
                        CompilerOptions.SourceFileName, compilerOptions, CompilerOptions.StandardDirectories,
                        resourceFiles?.ToArray());

                    CodeGeneratorsController.EmitAssemblyRedirects(
                        assemblyResolveScope,
                        CompilerOptions.OutputFileName);
                }
        }

        private void DebugOutputFileCreationUsingPDB()
        {
            int n = 1;
            try
            {
                n = 2;
                var fs = File.Create(CompilerOptions.OutputFileName);
                n = 3;
                fs.Close();
                n = 4;
                ///////File.Delete(CompilerOptions.OutputFileName);
                string pdb_file_name = Path.ChangeExtension(CompilerOptions.OutputFileName, ".pdb");
                if (File.Exists(pdb_file_name))
                    File.Delete(pdb_file_name);
                n = 5; // PVS 01/2022
            }
            catch (Exception e)
            {
                throw new UnauthorizedAccessToFile(CompilerOptions.OutputFileName + " -- " + n + "  " + e.ToString());
                //throw e;
            }
        }

        private List<string> GetResourceFilesFromCompilerDirectives(Dictionary<string, List<TreeRealization.compiler_directive>> compilerDirectives)
        {
            List<string> ResourceFiles = null;
            if (compilerDirectives.ContainsKey(StringConstants.compiler_directive_resource))
            {
                ResourceFiles = new List<string>();
                List<compiler_directive> ResourceDirectives = compilerDirectives[StringConstants.compiler_directive_resource];

                foreach (compiler_directive cd in ResourceDirectives)
                {
                    TryThrowInvalidPath(cd.directive, cd.location);
                    var resourceFileName = Path.Combine(Path.GetDirectoryName(cd.source_file), cd.directive);

                    // Так же как с main_resource
                    if (File.Exists(resourceFileName))
                        ResourceFiles.Add(resourceFileName);
                    else
                        ErrorsList.Add(new ResourceFileNotFound(cd.location.doc.file_name, cd.directive, cd.location));

                }
            }

            return ResourceFiles;
        }

        private bool HasOnlySyntaxErrors(List<Error> errors)
        {
            foreach (Error err in errors)
            {
                if (!(err is SyntaxError))
                    return false;
            }
            return true;
        }

        private void SaveDocumentationsForUnits()
        {
            DocXmlManager dxm = new DocXmlManager();
            foreach (CompilationUnit cu in UnitsTopologicallySortedList)
            {
                if (cu.Documented)
                    dxm.SaveXml(cu);
            }
        }

        private void ClosePCUReadersAndWriters()
        {
            if (PCUReadersAndWritersClosed)
                return;

            PCUReadersAndWritersClosed = true;

            bool all_restored = false;
            while (!all_restored)
            {
                foreach (PCUReader p in PCUReader.AllReaders)
                {
                    p.AddInitFinalMethods();
                    SystemLibrary.SystemLibInitializer.RestoreStandardFunctions();
                    p.ProcessWaitedToRestoreFields();
                    p.RestoreWaitedMethodCodes();
                }
                bool rest = true;
                for (int i = 0; i < PCUReader.AllReaders.Count; i++)
                    if (PCUReader.AllReaders[i].waited_types_to_restore_fields.Count != 0)
                    {
                        rest = false;
                        break;
                    }
                all_restored = rest;
            }
            PCUReader.AddUsedMembersInAllUnits();

            if (CompilerOptions.SavePCUInThreadPull)
                AsyncClosePCUWriters();
            else
                ClosePCUWriters();
        }

        private void WaitCallback_ClosePCUWriters(object state)
        {
            ClosePCUWriters();
        }

        private void AsyncClosePCUWriters()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(WaitCallback_ClosePCUWriters);
        }

        private void ClosePCUWriters()
        {
            foreach (CompilationUnit cu in UnitsTopologicallySortedList)
            {
                SavePCU(cu);
            }
            foreach (PCUWriter pw in PCUWriter.AllWriters)
            {
                pw.CloseWriter();
            }
        }

        /// <summary>
        /// Бросает ошибку если находит дупликаты в секции uses
        /// </summary>
        void CheckForDuplicatesInUsesSection(List<SyntaxTree.unit_or_namespace> usesList)
        {
            if (usesList == null)
                return;
            List<string> names = new List<string>();
            foreach (SyntaxTree.unit_or_namespace un in usesList)
            {
                string name = SyntaxTree.Utils.IdentListToString(un.name.idents, ".").ToLower();
                if (un.source_context != null)
                {
                    if (names.Contains(name))
                        throw new DuplicateUsesUnit(currentCompilationUnit.SyntaxTree.file_name, name, un.source_context);
                    else
                        names.Add(name);
                }
            }
        }

        /// <summary>
        /// Возвращает список зависимостей из интерфейсной части модуля (или основной программы)
        /// </summary>
        public List<SyntaxTree.unit_or_namespace> GetInterfaceUsesSection(SyntaxTree.compilation_unit unitSyntaxTree)
        {
            List<SyntaxTree.unit_or_namespace> usesList = null;

            if (unitSyntaxTree is SyntaxTree.unit_module unitModule)
            {
                if (unitModule.interface_part.uses_modules == null)
                {
                    if (CompilerOptions.StandardModules[currentCompilationUnit.Language.Name].Count > 0)
                    {
                        unitModule.interface_part.uses_modules = new SyntaxTree.uses_list();
                        unitModule.interface_part.uses_modules.source_context = new SyntaxTree.SourceContext();
                    }
                    else return null;
                }

                usesList = unitModule.interface_part.uses_modules.units;
            }
            else if (unitSyntaxTree is SyntaxTree.program_module programModule)
            {
                if (programModule.used_units == null)
                {
                    if (CompilerOptions.StandardModules[currentCompilationUnit.Language.Name].Count > 0)
                    {
                        programModule.used_units = new SyntaxTree.uses_list();
                        programModule.used_units.source_context = new SyntaxTree.SourceContext();
                    }
                    else return null;
                }

                usesList = programModule.used_units.units;
            }
            CheckForDuplicatesInUsesSection(usesList);
            return usesList;
        }

        private List<SyntaxTree.unit_or_namespace> GetImplementationUsesSection(SyntaxTree.compilation_unit unitSyntaxTree)
        {

            List<SyntaxTree.unit_or_namespace> usesSection = (unitSyntaxTree as SyntaxTree.unit_module)?.implementation_part?.uses_modules?.units;

            CheckForDuplicatesInUsesSection(usesSection);

            return usesSection;
        }

        public string FindPCUFileName(string fileName, string currentPath, out int folderPriority)
        {
            if (string.IsNullOrEmpty(Path.GetExtension(fileName)))
                fileName += CompilerOptions.CompiledUnitExtension;

            var cacheKey = Tuple.Create(fileName.ToLower(), currentPath?.ToLower());

            if (!PCUFileNamesDictionary.TryGetValue(cacheKey, out var fileNameWithPriority))
            {
                if (Path.GetExtension(fileName) != CompilerOptions.CompiledUnitExtension)
                    fileNameWithPriority = null;

                else if (FindFileWithExtensionInDirs(fileName, out _, currentPath) is string resultFileName1)
                    fileNameWithPriority = Tuple.Create(resultFileName1, 1);
                else if (CompilerOptions.OutputDirectory != CompilerOptions.SourceFileDirectory && FindFileWithExtensionInDirs(Path.GetFileName(fileName), out _, CompilerOptions.OutputDirectory) is string resultFileName2)
                    fileNameWithPriority = Tuple.Create(resultFileName2, 2);
                else if (FindFileWithExtensionInDirs(fileName, out var dirIndex, CompilerOptions.SearchDirectories.ToArray()) is string resultFileName3)
                    fileNameWithPriority = Tuple.Create(resultFileName3, 3 + dirIndex);
                else
                    fileNameWithPriority = null;

				PCUFileNamesDictionary[cacheKey] = fileNameWithPriority;
            }

            folderPriority = fileNameWithPriority?.Item2 ?? 0;
            return fileNameWithPriority?.Item1;
        }

        public string FindSourceFileName(string fileName, string currentPath, out int folderPriority)
        {
            var cacheKey = Tuple.Create(fileName.ToLower(), currentPath?.ToLower());

            if (!SourceFileNamesDictionary.TryGetValue(cacheKey, out var fileNameWithPriority))
			{

				if (FindSourceFileNameInDirs(fileName, out _, currentPath) is string resultFileName1)
                    fileNameWithPriority = Tuple.Create(resultFileName1, 1);
                else if (FindSourceFileNameInDirs(fileName, out var dirIndex, CompilerOptions.SearchDirectories.ToArray()) is string resultFileName2)
                    fileNameWithPriority = Tuple.Create(resultFileName2, 3 + dirIndex);
                else
                    fileNameWithPriority = null;

                SourceFileNamesDictionary[cacheKey] = fileNameWithPriority;
            }

            folderPriority = fileNameWithPriority?.Item2 ?? 0;
            return fileNameWithPriority?.Item1;
        }

        public string FindSourceFileNameInDirs(string fileName, out int foundDirIndex, params string[] Dirs)
        {
            var fileNameExtension = Path.GetExtension(fileName);
            var isExtensionEmpty = string.IsNullOrEmpty(fileNameExtension);

            // TODO: ищем сперва для расширения текущего языка  EVA

            foreach (SupportedSourceFile sf in SupportedSourceFiles)
            {
                foreach (string extension in sf.Extensions)
                {
                    if (isExtensionEmpty || fileNameExtension == extension)
                    {
                        var resultFileName = FindFileWithExtensionInDirs(isExtensionEmpty ? fileName + extension : fileName, out foundDirIndex, Dirs);
                        if (resultFileName != null)
                            return resultFileName;
                    }
                }  
            }
                

            foundDirIndex = 0;
            return null;
        }

        public static string CombinePathsRelatively(string path1, string path2)
        {
            if (Path.IsPathRooted(path2)) return path2;
            int i = 0;

            foreach (var s in path2.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }))
            {
                if (s == ".") continue;
                if (s == ".." && !string.IsNullOrWhiteSpace(path1) && !path1.EndsWith(".."))
                {
                    path1 = Path.GetDirectoryName(path1);
                    if (path1 == null) return null; // Path.GetDirectoryName("C:\") возвращает null
                }
                else
                    path1 = Path.Combine(path1, s);
            }

            return path1;
        }

        public static string GetUnitPath(CompilationUnit u1, CompilationUnit u2)
        {
            if (u1 == null) throw new ArgumentNullException(nameof(u1));
            if (u2 == null) throw new ArgumentNullException(nameof(u2));

            var curr = new Dictionary<CompilationUnit, string>();
            var done = new HashSet<CompilationUnit>();

            var res_path = default(string);
            Func<CompilationUnit, string, bool> register_unit = (CompilationUnit u, string path) =>
            {
                if (!done.Add(u))
                    return true;
                if (u == u2)
                {
                    res_path = path;
                    return false;
                }
                curr.Add(u, path);
                return true;
            };

            if (!u1.ForEachDirectCompilationUnit(register_unit))
                return res_path;

            while (curr.Count != 0)
            {
                var prev = curr;
                curr = new Dictionary<CompilationUnit, string>();

                foreach (var kvp in prev)
                    if (!kvp.Key.ForEachDirectCompilationUnit((u, path) =>
                    {
                        // Важно "..\a\b" + "..\c\d" превращать в "..\a\c\d", а не полный путь
                        return register_unit(u, CombinePathsRelatively(Path.GetDirectoryName(kvp.Value), path));
                    })) return res_path;

            }

            throw new InvalidOperationException($"Could not find path to \"{u2.UnitFileName}\" relative to \"{u1.UnitFileName}\"");
        }

        private string FindFileWithExtensionInDirs(string fileName, out int foundDirIndex, params string[] dirs)
        {
            if (Path.IsPathRooted(fileName))
            {
                foundDirIndex = 0;
                return File.Exists(fileName) ? fileName : null;
            }

            for (int dirIndex = 0; dirIndex < dirs.Length; ++dirIndex)
            {
                try
                {
                    var dir = dirs[dirIndex];
                    var resultFileName = Path.Combine(dir, fileName);
                    if (File.Exists(resultFileName))
                    {
                        foundDirIndex = dirIndex;
                        // Path.GetFullPath чтобы нормализовать
                        // File.Exists не может кинуть исключение или дать true
                        // если путь слишком длинный или содержит неправильные знаки
                        return Path.GetFullPath(resultFileName);
                    }
                }
                catch (PathTooLongException) { }
            }   

            foundDirIndex = 0;
            return null;
        }

        public static string GetReferenceFileName(string FileName, string curr_path = null)
        {
            // Вначале - кешированные стандартные dll
            if (standart_assembly_dict.ContainsKey(FileName))
                return standart_assembly_dict[FileName];
            if (curr_path != null && System.IO.File.Exists(Path.Combine(curr_path, FileName)))
                return Path.Combine(curr_path, FileName);
            if (System.IO.File.Exists(FileName))
            {
                return FileName;//.ToLower();//? а надо ли tolover?
            }
            else
            {
                return get_assembly_path(FileName, false);
            }
        }

        private string GetReferenceFileName(string FileName, SyntaxTree.SourceContext sc, string curr_path, bool overwrite)
        {
            FileName = FileName.Trim();
            if (standart_assembly_dict.ContainsKey(FileName))
                return standart_assembly_dict[FileName];

            // Наверное, этот код MikhailoMMX лишний
            //MikhailoMMX PABCRtl.dll будем искать сначала в GAC, а потом в папке с программой
            if (FileName == StringConstants.pabc_rtl_dll_name)
            {

                string name = get_assembly_path(FileName, true);
                if ((name != null) && (File.Exists(name)))
                    return name;

            }
            //\MikhailoMMX
            try
            {
                var FullFileName = Path.Combine(curr_path, FileName);
                if (File.Exists(FullFileName))
                {
                    var NewFileName = Path.GetFullPath(Path.Combine(CompilerOptions.OutputDirectory, Path.GetFileName(FullFileName)));
                    if (FullFileName != NewFileName)
                    {
                        if (overwrite)
                            File.Copy(FullFileName, NewFileName, true);
                        else if (!File.Exists(NewFileName))
                            File.Copy(FullFileName, NewFileName, false);
                    }

                    return NewFileName;
                }
                else
                {
                    string name = get_assembly_path(FileName, false);//? а надо ли tolover?
                    if (name == null)
                        throw new AssemblyNotFound(currentCompilationUnit.SyntaxTree.file_name, FileName, sc);
                    else
                        if (File.Exists(name))
                        return name;
                    else
                        throw new AssemblyNotFound(currentCompilationUnit.SyntaxTree.file_name, FileName, sc);
                }
            }
            catch (ArgumentException ex)
            {
                throw new InvalidAssemblyPathError(currentCompilationUnit.SyntaxTree.file_name, sc);
            }

        }

        public string GetUnitFileName(SyntaxTree.unit_or_namespace unitNode, string currentPath)
        {
            if (unitNode is SyntaxTree.uses_unit_in unitNodeCasted && unitNodeCasted.name == null)
                return unitNodeCasted.in_file.Value;

            if (currentPath == null) throw new InvalidOperationException(unitNode.UsesPath());

            var unitName = unitNode.name.idents[0].name;

            if (unitNode is SyntaxTree.uses_unit_in uui)
            {

                TryThrowInvalidPath(uui.in_file.Value, uui.in_file.source_context);

                if (unitName.ToLower() != Path.GetFileNameWithoutExtension(uui.in_file.Value).ToLower())
                    throw new UsesInWrongName(unitNode.source_context.FileName, unitName, Path.GetFileNameWithoutExtension(uui.in_file.Value), uui.in_file.source_context);

            }

            return GetUnitFileName(unitName, unitNode.UsesPath(), currentPath, unitNode.source_context);
        }

        public string GetUnitFileName(string unitName, string usesPath, string currentPath, SyntaxTree.SourceContext sourceContext)
        {
            var cacheKey = Tuple.Create(usesPath.ToLower(), currentPath?.ToLower());
            
            if (GetUnitFileNameCache.TryGetValue(cacheKey, out var unitFileName))
                return unitFileName;

            // число приоритета меньше означает, что папка более важная
            var sourceFileName = FindSourceFileName(usesPath, currentPath, out var sourceFilePriority);
            var pcuFileName = FindPCUFileName(usesPath, currentPath, out var pcuFilePriority);

            bool sourceFileExists = sourceFileName != null;
            bool pcuFileExists = pcuFileName != null;

            if (CompilerOptions.Rebuild && sourceFileExists)
                pcuFileExists = false;

            // если нет ни одного типа файла или нет исходника и режим Rebuild, то ошибка 
            if (!sourceFileExists && !pcuFileExists)
            {
                if (unitName == null)
                    // вызов с "unitFileName == null" должен быть только там, где уже известно что хотя бы какой то файл есть
                    // если где то ещё будет исопльзоваться unitFileName или source_context - надо будет добавить такую же проверку
                    throw new InvalidOperationException(nameof(unitName));
                else
                    throw new UnitNotFound(sourceContext.FileName, unitName, sourceContext);
            }
                

            if (pcuFileExists && sourceFileExists)
            {
                // если приоритет папки исходника выше, то берем исходник
                if (sourceFilePriority < pcuFilePriority)
                    pcuFileExists = false;
                // проверка на правильность установки приоритета - для удобства
                else if (sourceFilePriority == pcuFilePriority && Path.GetDirectoryName(sourceFileName) != Path.GetDirectoryName(pcuFileName))
                    throw new InvalidOperationException("Не верно задан приоритет папок!");
                // также если время модификации pcu раньше, чем исходника, то нужна перекомпиляция (при этом они должны быть в одной папке)
                else if (sourceFilePriority == pcuFilePriority && File.GetLastWriteTime(pcuFileName) < File.GetLastWriteTime(sourceFileName))
                    pcuFileExists = false;
            }

            if (pcuFileExists)
                unitFileName = Path.Combine(currentPath, pcuFileName);
            else if (sourceFileExists)
                unitFileName = Path.Combine(currentPath, sourceFileName);
            else
                // значит в предыдущем блоке кода ошибка - проверка для удобства
                throw new InvalidOperationException("Сброшено значение pcuFileExists и sourceFileExists. Такого здесь быть не должно."); 

            GetUnitFileNameCache[cacheKey] = unitFileName;
            return unitFileName;
        }

        public void AddStandardUnitsToInterfaceUsesSection(CompilationUnit currentUnit)
        {
            if (CompilerOptions.StandardModules[currentUnit.Language.Name].Count == 0)
                return;

            List<SyntaxTree.unit_or_namespace> usesList = GetInterfaceUsesSection(currentUnit.SyntaxTree);

            string currentModuleName = Path.GetFileNameWithoutExtension(currentUnit.SyntaxTree.file_name).ToLower();

            foreach (CompilerOptions.StandardModule module in CompilerOptions.StandardModules[currentUnit.Language.Name])
            {
                string moduleName = Path.GetFileNameWithoutExtension(module.name);
                if (moduleName.ToLower() == currentModuleName)
                    return;
            }

            foreach (CompilerOptions.StandardModule module in CompilerOptions.StandardModules[currentUnit.Language.Name])
            {
                // если мы компилируем не основную программу, а добавлять нужно в основную программу, то пропускаем
                if (module.addMethod == CompilerOptions.StandardModuleAddMethod.RightToMain && currentUnit != firstCompilationUnit)
                    continue;

                string moduleName = Path.GetFileNameWithoutExtension(module.name);

                // если стандартный модуль уже подключен
                bool isModuleAlreadyInUsesSection = false;
                foreach (SyntaxTree.unit_or_namespace currentUnitNode in usesList)
                {
                    if (currentUnitNode.name.idents.Count == 1 && currentUnitNode.name.idents[0].name.ToLower() == moduleName.ToLower())
                    {
                        isModuleAlreadyInUsesSection = true;
                        break;
                    }

                }
                if (isModuleAlreadyInUsesSection) continue;

                // здесь присвоится либо юнит, либо пространство имен
                SyntaxTree.unit_or_namespace unitToAdd;

                if (Path.GetExtension(module.name) != "" /*&& Path.GetExtension(ModuleFileName).ToLower() != ".dll"*/)
                {
                    unitToAdd = new SyntaxTree.uses_unit_in(
                        new SyntaxTree.ident_list(new SyntaxTree.ident(moduleName)),
                        new SyntaxTree.string_const(module.name));
                    //uses_unit_in.source_context = uses_unit_in.in_file.source_context = uses_unit_in.name.source_context = new SyntaxTree.SourceContext(1, 1, 1, 1);
                }
                else
                {
                    unitToAdd = new SyntaxTree.unit_or_namespace(new SyntaxTree.ident_list(new SyntaxTree.ident(moduleName)));
                    //uses_unit.source_context = uses_unit.name.source_context = new SyntaxTree.SourceContext(1, 1, 1, 1);
                }

                // добавление
                if (module.addMethod == CompilerOptions.StandardModuleAddMethod.RightToMain)
                {
                    usesList.Add(unitToAdd);
                }
                else if (module.addMethod == CompilerOptions.StandardModuleAddMethod.LeftToAll)
                {
                    usesList.Insert(0, unitToAdd);
                }

            }
        }

        private Assembly PreloadReference(compiler_directive reference)
        {
            var sc = GetSourceContext(reference);
            var fileName = GetReferenceFileName(reference.directive, sc, Path.GetDirectoryName(reference.source_file), true);
            return assemblyResolveScope.PreloadAssembly(fileName);
        }

        private CompilationUnit CompileReference(unit_node_list dlls, compiler_directive reference)
        {
            var sourceContext = GetSourceContext(reference);
            string unitName;
            try
            {
                unitName = GetReferenceFileName(reference.directive, sourceContext, Path.GetDirectoryName(reference.source_file), false);
            }
            catch (AssemblyNotFound)
            {
                throw;
            }
            // ToDo плохо, пока дебажил - тут постоянно ловились другие исключения, не связанные с неправильным знаками в пути к сборке |
            // EVA  (проверить)
            catch (Exception)
            {
                throw new InvalidAssemblyPathError(currentCompilationUnit.SyntaxTree.file_name, sourceContext);
            }

            CompilationUnit currentUnit = null;
            if (UnitTable.Count == 0) throw new ProgramModuleExpected(unitName, null);
            if ((currentUnit = ReadDLL(unitName, sourceContext)) != null)
            {
                dlls.AddElement(currentUnit.SemanticTree, null);
                UnitTable[unitName] = currentUnit;
                return currentUnit;
            }
            else throw new AssemblyReadingError(currentCompilationUnit.SyntaxTree.file_name, unitName, sourceContext);
        }

        private SyntaxTree.SourceContext GetSourceContext(compiler_directive directive)
        {
            var loc = directive.location;
            if (loc == null) return null;
            return new SyntaxTree.SourceContext(loc.begin_line_num, loc.begin_column_num, loc.end_line_num,
                loc.end_column_num, 0, 0);
        }

        private bool HasIncludeNamespaceDirective(CompilationUnit unit)
        {
            var directives = GetDirectivesAsSemanticNodes(unit.SyntaxTree.compiler_directives, unit.SyntaxTree.file_name);

            return directives.Any(directive => directive.name.ToLower() == StringConstants.compiler_directive_include_namespace);
        }


        private Dictionary<string, SyntaxTree.syntax_namespace_node> PrepareUserNamespacesUsedInTheCurrentUnit(CompilationUnit compilationUnit)
        {
            var directives = GetDirectivesAsSemanticNodes(compilationUnit.SyntaxTree.compiler_directives, compilationUnit.SyntaxTree.file_name);

            List<string> files = GetIncludedFilesFromDirectives(compilationUnit, directives);

            Dictionary<string, SyntaxTree.syntax_namespace_node> namespaces = new Dictionary<string, SyntaxTree.syntax_namespace_node>(StringComparer.OrdinalIgnoreCase);

            List<SyntaxTree.unit_or_namespace> namespaceModules = new List<SyntaxTree.unit_or_namespace>();

            foreach (string file in files)
            {
                SyntaxTree.compilation_unit syntaxTree = GetNamespaceSyntaxTree(file);

                #region SEMANTIC CHECKS : PASCAL NAMESPACE

                SemanticCheckIsUserNamespace(syntaxTree);

                #endregion

                SyntaxTree.unit_module unitModule = syntaxTree as SyntaxTree.unit_module;

                SyntaxTree.syntax_namespace_node namespaceNode = null;

                if (!namespaces.TryGetValue(unitModule.unit_name.idunit_name.name, out namespaceNode))
                {
                    namespaceNode = new SyntaxTree.syntax_namespace_node(unitModule.unit_name.idunit_name.name);
                    namespaceNode.referenced_units = new unit_node_list();
                    namespaces[unitModule.unit_name.idunit_name.name] = namespaceNode;
                }

                AddDeclarationsAndReferencedUnitsToNamespaces(namespaceModules, file, unitModule, namespaceNode);
            }

            // корневой модуль является чем-то одним из этого
            SyntaxTree.unit_module mainLibrary = compilationUnit.SyntaxTree as SyntaxTree.unit_module;
            SyntaxTree.program_module mainProgram = compilationUnit.SyntaxTree as SyntaxTree.program_module;

            AddNamespacesToMainDefinitions(mainLibrary, mainProgram, namespaces);

            AddNamespacesToMainUsesList(mainLibrary, mainProgram, namespaceModules);

            return namespaces;
        }

        private void AddNamespacesToMainDefinitions(SyntaxTree.unit_module mainLibrary, SyntaxTree.program_module main_program, Dictionary<string, SyntaxTree.syntax_namespace_node> namespaces)
        {
            foreach (string moduleName in namespaces.Keys)
            {
                if (mainLibrary != null)
                    mainLibrary.interface_part.interface_definitions.Insert(0, namespaces[moduleName]);
                else
                    main_program.program_block.defs.Insert(0, namespaces[moduleName]);
            }
        }

        private void AddNamespacesToMainUsesList(SyntaxTree.unit_module mainLibrary, SyntaxTree.program_module main_program, List<SyntaxTree.unit_or_namespace> namespaceModules)
        {
            SyntaxTree.uses_list mainUsesList;

            if (mainLibrary != null)
                mainUsesList = mainLibrary.interface_part.uses_modules;
            else
                mainUsesList = main_program.used_units;


            if (mainUsesList == null)
                mainUsesList = new SyntaxTree.uses_list();

            HashSet<string> set = new HashSet<string>();
            foreach (SyntaxTree.unit_or_namespace name_space in namespaceModules)
            {
                string name = SyntaxTree.Utils.IdentListToString(name_space.name.idents, ".").ToLower();
                if (!set.Contains(name))
                {
                    mainUsesList.Add(name_space);
                    set.Add(name);
                }
            }

            if (mainLibrary != null)
                mainLibrary.interface_part.uses_modules = mainUsesList;
            else
                main_program.used_units = mainUsesList;
        }

        private void AddDeclarationsAndReferencedUnitsToNamespaces(List<SyntaxTree.unit_or_namespace> namespace_modules, string file,
            SyntaxTree.unit_module unitModule, SyntaxTree.syntax_namespace_node namespaceNode)
        {
            if (unitModule.interface_part.interface_definitions != null)
            {
                foreach (SyntaxTree.declaration decl in unitModule.interface_part.interface_definitions.defs)
                {
                    namespaceNode.defs.Add(decl);
                }
                if (unitModule.interface_part.uses_modules != null)
                {
                    CheckForDuplicatesInUsesSection(unitModule.interface_part.uses_modules.units);
                    foreach (SyntaxTree.unit_or_namespace name_space in unitModule.interface_part.uses_modules.units)
                    {
                        if (IsPossibleNetNamespaceOrStandardPasFile(name_space, false, Path.GetDirectoryName(file)))
                        {
                            namespaceNode.referenced_units.AddElement(new namespace_unit_node(GetNamespace(name_space), GetLocationFromTreenode(name_space, unitModule.file_name)), null);
                        }
                        else
                        {
                            namespace_modules.Add(name_space);
                        }
                    }
                }
            }
        }

        private void SemanticCheckIsUserNamespace(SyntaxTree.compilation_unit unitSyntaxTree)
        {
            if (!(unitSyntaxTree is SyntaxTree.unit_module))
                throw new NamespaceModuleExpected(unitSyntaxTree.source_context);

            SyntaxTree.unit_module unitModule = unitSyntaxTree as SyntaxTree.unit_module;

            if (unitModule.unit_name.HeaderKeyword != SyntaxTree.UnitHeaderKeyword.Namespace)
                throw new NamespaceModuleExpected(unitModule.unit_name.source_context);
            if (unitModule.implementation_part != null)
                throw new NamespaceModuleExpected(unitModule.implementation_part.source_context);
            if (unitModule.finalization_part != null)
                throw new NamespaceModuleExpected(unitModule.finalization_part.source_context);
            if (unitModule.initialization_part != null && unitModule.initialization_part.list.Count > 0)
                throw new NamespaceModuleExpected(unitModule.initialization_part.source_context);
        }

        private static List<string> GetIncludedFilesFromDirectives(CompilationUnit compilationUnit, List<compiler_directive> directives)
        {
            List<string> files = new List<string>();
            foreach (compiler_directive cd in directives)
            {
                if (cd.name.ToLower() == StringConstants.compiler_directive_include_namespace)
                {
                    string directive = cd.directive.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);

                    if (directive == "*.pas" || directive.EndsWith(Path.DirectorySeparatorChar + "*.pas"))
                    {
                        string dir = Path.Combine(Path.GetDirectoryName(compilationUnit.SyntaxTree.file_name), directive.Replace(Path.DirectorySeparatorChar + "*.pas", ""));
                        foreach (string file in Directory.EnumerateFiles(dir, "*.pas"))
                        {
                            if (!File.Exists(file))
                                throw new FileNotFound(file, cd.location);
                            files.Add(file);
                        }

                    }
                    else
                    {
                        string file = Path.Combine(Path.GetDirectoryName(compilationUnit.SyntaxTree.file_name), directive);
                        if (!File.Exists(file))
                            throw new FileNotFound(file, cd.location);
                        files.Add(file);
                    }
                }
            }

            return files;
        }

        private void SemanticCheckNoIncludeNamespaceDirectivesInUnit(CompilationUnit currentUnit)
        {
            if (currentUnit.SyntaxTree is SyntaxTree.unit_module unitModule && HasIncludeNamespaceDirective(currentUnit)
                && unitModule.unit_name.HeaderKeyword != SyntaxTree.UnitHeaderKeyword.Library)
            {
                throw new IncludeNamespaceInUnitError(currentUnit.SyntaxTree.file_name, currentUnit.SyntaxTree.source_context);
            }
        }

        private SyntaxTree.compilation_unit GetNamespaceSyntaxTree(string fileName)
        {
            string sourceText = GetSourceFileText(fileName);
            List<string> definesList = new List<string> { "PASCALABC" }; // SSM 11/07/20
            
            if (!CompilerOptions.Debug && !CompilerOptions.ForDebugging)
                definesList.Add("RELEASE");
            else
                definesList.Add("DEBUG");
            
            definesList.AddRange(CompilerOptions.ForceDefines);

            ILanguage language = LanguageProvider.SelectLanguageByExtension(fileName);

            SyntaxTree.compilation_unit syntaxTree = InternalParseText(language, fileName, sourceText, errorsList, warnings, definesList);
            
            if (errorsList.Count > 0)
                throw errorsList[0];

            syntaxTree = ConvertSyntaxTree(syntaxTree, language.SyntaxTreeConverters);
            
            
            return syntaxTree;
        }

        public unit_node_list GetReferences(CompilationUnit compilationUnit)
        {
            unit_node_list dlls = new unit_node_list();
            List<compiler_directive> directives;
            if (compilationUnit.SemanticTree is common_unit_node)
                directives = (compilationUnit.SemanticTree as common_unit_node).compiler_directives;
            else
                directives = GetDirectivesAsSemanticNodes(compilationUnit.SyntaxTree.compiler_directives, compilationUnit.SyntaxTree.file_name);
            
            DisablePABCRtlIfUsingDotnet5(directives);

            if (CompilerOptions.UseDllForSystemUnits)
            {
                directives.Add(new compiler_directive("reference", "%GAC%\\PABCRtl.dll", null, "."));
                AddReferencesToNetSystemLibraries(compilationUnit, directives);
            }

            var referenceDirectives = new List<compiler_directive>();
            foreach (compiler_directive directive in directives)
            {
                if (directive.name.ToLower() == StringConstants.compiler_directive_reference)
                {

                    referenceDirectives.Add(directive);
                }
            }

            if (CompilerOptions.ProjectCompiled)
            {
                foreach (ReferenceInfo ri in project.references)
                {
                    referenceDirectives.Add(new compiler_directive("reference", ri.full_assembly_name, null, project.MainFile));
                }
            }

            if (assemblyResolveScope == null)
                assemblyResolveScope = new NetHelper.AssemblyResolveScope(AppDomain.CurrentDomain);

            // It's important to preload all the referenced assemblies before starting the compilation. During the
            // compilation, we need to access types from every referenced assembly. An attempt to access them could fail
            // if a transitively dependent assembly is not loaded, yet.
            //
            // It's not always possible to solve by re-ordering the references, since there are cases of
            // mutually-dependent assemblies (i.e. dependency loops) in the wild.
            foreach (var reference in referenceDirectives)
            {
                try
                {
                    PreloadReference(reference);
                }
                catch (FileLoadException ex)
                {
                    throw new CommonCompilerError(ex.Message, compilationUnit.SyntaxTree.file_name, reference.location.begin_line_num, reference.location.end_line_num);
                }
            }


            foreach (var reference in referenceDirectives)
                CompileReference(dlls, reference);

            return dlls;
        }

        private void DisablePABCRtlIfUsingDotnet5(List<compiler_directive> directives)
        {
            foreach (compiler_directive cd in directives)
            {
                if (cd.name.ToLower() == StringConstants.compiler_directive_platformtarget
                    && !string.IsNullOrEmpty(cd.directive) && cd.directive.IndexOf("dotnet5") != -1)
                {
                    CompilerOptions.UseDllForSystemUnits = false;
                }
            }
        }

        /// <summary>
        /// Добавляет ссылки на стандартные системные dll .NET - версия с директивами уровня семантики
        /// </summary>
        /// <param name="compilationUnit"></param>
        /// <param name="directives"></param>
        private void AddReferencesToNetSystemLibraries(CompilationUnit compilationUnit, List<TreeRealization.compiler_directive> directives)
        {
            IEnumerable<string> librariesToAdd = StringConstants.netSystemLibraries.Select(dll => $"%GAC%\\{dll}")
                .Except(directives.Where(directive => directive.name.Equals("reference", StringComparison.CurrentCultureIgnoreCase))
                .Select(directive => directive.directive), StringComparer.CurrentCultureIgnoreCase);

            directives.AddRange(librariesToAdd.Select(dll => new compiler_directive("reference", dll, null, ".")));

            if (compilationUnit.SyntaxTree is SyntaxTree.program_module program && program.used_units != null)
            {
                var graph3DUnit = program.used_units.units.FirstOrDefault(u => u.name.ToString() == "Graph3D");
                if (graph3DUnit != null)
                {
                    IEnumerable<string> graphLibrariesToAdd = StringConstants.graph3DDependencies.Select(dll => $"%GAC%\\{dll}")
                        .Except(directives.Where(directive => directive.name.Equals("reference", StringComparison.CurrentCultureIgnoreCase))
                        .Select(directive => directive.directive), StringComparer.CurrentCultureIgnoreCase);

                    directives.AddRange(graphLibrariesToAdd.Select(dll => new compiler_directive("reference", dll, null, ".")));
                }
            }
        }

        /// <summary>
        /// Добавляет ссылки на стандартные системные dll .NET - версия с директивами уровня синтаксиса
        /// </summary>
        /// <param name="compilationUnit"></param>
        /// <param name="directives"></param>
        private void AddReferencesToNetSystemLibraries(CompilationUnit compilationUnit, List<SyntaxTree.compiler_directive> directives)
        {
            IEnumerable<string> librariesToAdd = StringConstants.netSystemLibraries.Select(dll => $"%GAC%\\{dll}")
                .Except(directives.Where(directive => directive.Name.text.Equals("reference", StringComparison.CurrentCultureIgnoreCase))
                .Select(directive => directive.Directive.text), StringComparer.CurrentCultureIgnoreCase);

            directives.AddRange(librariesToAdd.Select(dll => new SyntaxTree.compiler_directive(new SyntaxTree.token_info("reference"), new SyntaxTree.token_info(dll))));

            if (compilationUnit.SyntaxTree is SyntaxTree.program_module program && program.used_units != null)
            {
                var graph3DUnit = program.used_units.units.FirstOrDefault(u => u.name.ToString() == "Graph3D");
                if (graph3DUnit != null)
                {
                    IEnumerable<string> graphLibrariesToAdd = StringConstants.graph3DDependencies.Select(dll => $"%GAC%\\{dll}")
                        .Except(directives.Where(directive => directive.Name.text.Equals("reference", StringComparison.CurrentCultureIgnoreCase))
                        .Select(directive => directive.Directive.text), StringComparer.CurrentCultureIgnoreCase);

                    directives.AddRange(graphLibrariesToAdd.Select(dll => new SyntaxTree.compiler_directive(new SyntaxTree.token_info("reference"), new SyntaxTree.token_info(dll))));
                }
            }
        }

        NetHelper.AssemblyResolveScope assemblyResolveScope;

        private bool IsPossibleNetNamespaceOrStandardPasFile(SyntaxTree.unit_or_namespace name_space, bool addToStandardModules, string currentPath)
        {
            if (name_space is SyntaxTree.uses_unit_in)
                return false;

            // если это "что-то"."что-то"... (полный путь к пространству имен)
            if (name_space.name.idents.Count > 1)
                return true;

            string sourceFileName = FindSourceFileName(name_space.name.idents[0].name, currentPath, out _);
            string pcuFileName = FindPCUFileName(name_space.name.idents[0].name, currentPath, out _);

            // если нет исходников и pcu
            if (sourceFileName == null && pcuFileName == null)
                return true;

            // если есть что-то одно
            string fileName = sourceFileName ?? pcuFileName;

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

            // если в программе используются эти модули, то RTL не используется
            string[] standardFilesExcludedFromRTL = new string[] { "PT4", "School", "CRT", "Arrays", "MPI", "Collections", "Core"};

            bool includeInRTL = standardFilesExcludedFromRTL.All(file => !file.Equals(fileNameWithoutExtension, StringComparison.CurrentCultureIgnoreCase));

            // если это исходный файл из папки Lib (стандартные паскалевские модули)
            if (CompilerOptions.UseDllForSystemUnits
                && Path.GetDirectoryName(fileName).Equals(Path.Combine(CompilerOptions.SystemDirectory, "Lib"), StringComparison.CurrentCultureIgnoreCase)
                && includeInRTL)
            {
                string s = Path.GetFileNameWithoutExtension(fileName).ToLower();
                if (addToStandardModules && !StandardModules.Contains(s))
                    StandardModules.Add(s);
                return true;
            }
            
            return false;
        }

        public using_namespace GetNamespace(SyntaxTree.unit_or_namespace _name_space)
        {
            return new using_namespace(SyntaxTree.Utils.IdentListToString(_name_space.name.idents, "."));
        }

        /// <summary>
        /// Формирует узел семантического дерева, соответствующий пространству имен (.NET или пользовательскому)
        /// </summary>
        /// <exception cref="UnitNotFound"></exception>
        /// <exception cref="TreeConverter.NamespaceNotFound"></exception>
        private using_namespace GetNamespace(using_namespace_list usingList, string fullNamespaceName, SyntaxTree.unit_or_namespace name_space, bool mightBeUnit, Dictionary<string, SyntaxTree.syntax_namespace_node> namespaces)
        {
            if (!NetHelper.NetHelper.NamespaceExists(fullNamespaceName) && (namespaces == null || !namespaces.ContainsKey(fullNamespaceName)))
            {
                if (mightBeUnit && !fullNamespaceName.Contains("."))
                    throw new UnitNotFound(currentCompilationUnit.SyntaxTree.file_name, fullNamespaceName, name_space.source_context);

                throw new TreeConverter.NamespaceNotFound(fullNamespaceName, GetLocationFromTreenode(name_space.name, currentCompilationUnit.SyntaxTree.file_name));
            }
            return new using_namespace(fullNamespaceName);
        }

        public void AddNamespacesToUsingList(using_namespace_list usingList, List<SyntaxTree.unit_or_namespace> possibleNamespaces, bool mightContainUnits, Dictionary<string, SyntaxTree.syntax_namespace_node> namespaces)
        {
            foreach (SyntaxTree.unit_or_namespace name_space in possibleNamespaces)
            {
                usingList.AddElement(GetNamespace(usingList, SyntaxTree.Utils.IdentListToString(name_space.name.idents, "."),
                    name_space, mightContainUnits, namespaces));
            }
        }

        public void AddNamespacesToUsingList(using_namespace_list using_list, SyntaxTree.using_list ul)
        {
            if (ul != null)
                AddNamespacesToUsingList(using_list, ul.namespaces, false, null);
        }

        /// <summary>
        /// получение списка using - legacy code !!!
        /// </summary>
        public SyntaxTree.using_list GetInterfaceUsingList(SyntaxTree.compilation_unit cu)
        {
            if (cu is SyntaxTree.unit_module)
                return (cu as SyntaxTree.unit_module).interface_part.using_namespaces;
            if (cu is SyntaxTree.program_module)
                return (cu as SyntaxTree.program_module).using_namespaces;
            return null;
        }

        /// <summary>
        /// получение списка using - legacy code !!!
        /// </summary>
        private SyntaxTree.using_list GetImplementationSyntaxUsingList(SyntaxTree.compilation_unit cu)
        {
            if (cu is SyntaxTree.unit_module)
                if ((cu as SyntaxTree.unit_module).implementation_part != null)
                    return (cu as SyntaxTree.unit_module).implementation_part.using_namespaces;
            return null;
        }

        public string GetSourceFileText(string FileName)
        {
            return (string)SourceFilesProvider(FileName, SourceFileOperation.GetText);
        }


        public SyntaxTree.compilation_unit ParseText(string fileName, string text, List<Error> errorList, List<CompilerWarning> warnings)
        {
            Reset();
            ILanguage language = LanguageProvider.SelectLanguageByExtension(fileName);
            
            OnChangeCompilerState(this, CompilerState.CompilationStarting, fileName);
            SyntaxTree.compilation_unit cu = InternalParseText(language, fileName, text, ErrorsList, warnings, null, false);
            OnChangeCompilerState(this, CompilerState.Ready, fileName);
            
            return cu;
        }

        private void CalculateLinesCompiled(List<Error> errorList, SyntaxTree.compilation_unit unitSyntaxTree)
        {
            if (errorList.Count == 0 && unitSyntaxTree != null && unitSyntaxTree.source_context != null)
            {
                linesCompiled += (uint)(unitSyntaxTree.source_context.end_position.line_num - unitSyntaxTree.source_context.begin_position.line_num + 1);
            }
        }

        private void CalculatePascalProgramHealth(SyntaxTree.compilation_unit unitSyntaxTree)
        {
            // Это только для локального компилятора?
            var stat = new SyntaxVisitors.ABCStatisticsVisitor();
            stat.ProcessNode(unitSyntaxTree);
            pABCCodeHealth = stat.CalcHealth(out int _, out int _);
        }

        private SyntaxTree.compilation_unit InternalParseText(ILanguage language, string fileName, string text, List<Error> errorList, List<CompilerWarning> warnings, List<string> definesList = null, bool calculateHealth = true)
        {
            OnChangeCompilerState(this, CompilerState.BeginParsingFile, fileName);
            SyntaxTree.compilation_unit unitSyntaxTree = language.Parser.GetCompilationUnit(fileName, text, ErrorsList, warnings, Parsers.ParseMode.Normal, definesList);
            OnChangeCompilerState(this, CompilerState.EndParsingFile, fileName);

            // Вычисляем сколько строк скомпилировали
            CalculateLinesCompiled(errorList, unitSyntaxTree);
            
            // 500 - это наибольшая программа для начинающих. БОльшая программа - здоровье кода только по кнопке (чтобы не замедлять)
            if (calculateHealth && language.Name == StringConstants.pascalLanguageName && linesCompiled <= 500)
            {
                // TODO: при подсчете здоровья программы на других языках необходимо вынести StatisticsVisitor в интерфейс ILanguage  EVA
                CalculatePascalProgramHealth(unitSyntaxTree);
            }
            return unitSyntaxTree;
        }

        /// <summary>
        /// Проверяет, является ли модуль dll по соответствующей директиве
        /// </summary>
        public static bool IsDll(SyntaxTree.compilation_unit unitSyntaxTree)
        {
            foreach (SyntaxTree.compiler_directive directive in unitSyntaxTree.compiler_directives)
            {
                if (string.Equals(directive.Name.text, "apptype", StringComparison.CurrentCultureIgnoreCase)
                    && string.Equals(directive.Directive.text, "dll", StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Проверяет, является ли модуль dll по соответствующей директиве и возвращает эту директиву выходным параметром
        /// </summary>
        public static bool IsDll(SyntaxTree.compilation_unit unitSyntaxTree, out SyntaxTree.compiler_directive dllDirective)
        {
            foreach (SyntaxTree.compiler_directive directive in unitSyntaxTree.compiler_directives)
            {
                if (string.Equals(directive.Name.text, StringConstants.compiler_directive_apptype, StringComparison.CurrentCultureIgnoreCase)
                                    && string.Equals(directive.Directive.text, "dll", StringComparison.CurrentCultureIgnoreCase))
                {
                    dllDirective = directive;
                    return true;
                }
            }

            dllDirective = null;
            return false;
        }

        /// <summary>
        /// Компилирует основную программу и все используемые ей юниты рекурсивно
        /// </summary>
        /// <param name="unitsFromUsesSection"> Вспомогательная переменная для заполнения CompilationUnit.interfaceUsedUnits и 
        /// CompilationUnit.implementationUsedUnits (здесь могут содержаться юниты и dll) </param>
        /// 
        /// <param name="directUnitsFromUsesSection">Вспомогательная переменная для заполнения CompilationUnit.interfaceUsedDirectUnits и 
        /// CompilationUnit.implementationUsedDirectUnits</param>
        /// 
        /// <param name="currentUnitNode">Синтаксический узел текущего модуля (или пространства имен)</param>
        /// <param name="previousPath">Директория родительского модуля</param>
        /// <returns>Скомпилированный юнит</returns>
        public CompilationUnit CompileUnit(unit_node_list unitsFromUsesSection, Dictionary<unit_node, CompilationUnit> directUnitsFromUsesSection, SyntaxTree.unit_or_namespace currentUnitNode, string previousPath)
        {
            string unitFileName = GetUnitFileName(currentUnitNode, previousPath);
            string unitId = Path.ChangeExtension(unitFileName, null);
            // Имя папки, в которой лежит текущий модуль
            // Используется для подключения модулей, $include и т.п. из модуля, подключённого с uses-in
            var currentDirectory = Path.GetDirectoryName(unitFileName);

            // вернет null, если юнит еще не был инициализирован
            CompilationUnit currentUnit = UnitTable[unitId];

            Dictionary<SyntaxTree.syntax_tree_node, string> docs = null;

            if (currentUnit != null)
            {
                #region SEMANTIC CHECKS : USES IN SECTION LOGIC

                // ошибка - пространство имен не может содержать in секцию (для указания файла)
                SemanticCheckUsesInIsNotNamespace(currentUnitNode, currentUnit);
                #endregion

                // если модуль уже скомпилирован - возвращаем (возможно, только интерфейс модуля и тогда он докомпилируется в другом рекурсивном вызове)   EVA
                if (currentUnit.State != UnitState.BeginCompilation || currentUnit.SemanticTree != null)  //TODO: ИЗБАВИТЬСЯ ОТ ВТОРОГО УСЛОВИЯ
                {
                    AddCurrentUnitAndItsReferencesToUsesLists(unitsFromUsesSection, directUnitsFromUsesSection, 
                                                              currentUnitNode, currentUnit, GetReferences(currentUnit));
                    return currentUnit;
                }
            }
            else
            {
                // если есть pcu - возврат  EVA
                if (UnitHasPCU(unitsFromUsesSection, directUnitsFromUsesSection, currentUnitNode, ref unitFileName, ref currentUnit))
                    return currentUnit;

                // нет pcu и модуль не откомпилирован => новый модуль   EVA
                InitializeNewUnit(unitFileName, unitId, ref currentUnit, out docs);
            }

            // формирование списков зависимостей текущего модуля (uses list, dll, пространства имен)
            CreateDependencyListsForCurrentUnit(currentUnit, currentDirectory, out var interfaceUsesList, out var references, out var namespaces);

            #region INTERFACE PART
            // комплируем зависимости из интерфейса  EVA
            if (interfaceUsesList != null)
            {
                CompileInterfaceDependencies(unitsFromUsesSection, directUnitsFromUsesSection, currentUnitNode,
                    unitFileName, currentDirectory, currentUnit, interfaceUsesList, references, namespaces, out bool shouldReturnCurUnit);

                if (shouldReturnCurUnit)
                    return currentUnit;
            }

            currentCompilationUnit = currentUnit;

            currentUnit.InterfaceUsedUnits.AddRange(references);

            // Добавление пространств имен из uses list (могут быть разных видов)
            AddNamespacesToUsingList(currentUnit.InterfaceUsingNamespaceList, currentUnit.possibleNamespaces, true, namespaces);

            #region USING LIST LEGACY CODE
            // Добавление пространств имен NET из using list - устаревшее ключевое слово using
            AddNamespacesToUsingList(currentUnit.InterfaceUsingNamespaceList, GetInterfaceUsingList(currentUnit.SyntaxTree));

            #endregion

            //Console.WriteLine("Compiling Interface "+ unitFileName);//DEBUG

            // компилируем интерфейс текущего модуля EVA
            CompileCurrentUnitInterface(unitFileName, currentUnit, docs);

            // интерфейс скомпилирован - переходим к секции реализации 
            currentUnit.State = UnitState.InterfaceCompiled;

            // заполнение списков uses семантического уровня
            AddCurrentUnitAndItsReferencesToUsesLists(unitsFromUsesSection, directUnitsFromUsesSection, currentUnitNode, currentUnit, references);
            #endregion

            #region IMPLEMENTATION PART
            // берем модули из секции uses в реализации
            List<SyntaxTree.unit_or_namespace> implementationUsesList = GetImplementationUsesSection(currentUnit.SyntaxTree);

            currentUnit.ImplementationUsedUnits.Clear();
            currentUnit.possibleNamespaces.Clear();

            common_unit_node semanticTreeAsCommonNode = currentUnit.SemanticTree as common_unit_node;

            // Компиляция зависимостей в области реализации    EVA
            CompileImplementationDependencies(currentDirectory, currentUnit, implementationUsesList, namespaces, semanticTreeAsCommonNode, out bool shouldReturnCurrentUnit);

            if (shouldReturnCurrentUnit)
                return currentUnit;

            // Console.WriteLine("Compiling Implementation "+ unitFileName);//DEBUG

            // компилируем реализацию текущего модуля EVA
            CompileCurrentUnitImplementation(unitFileName, currentUnit, docs);
            #endregion

            currentUnit.State = UnitState.Compiled;

            if (semanticTreeAsCommonNode != null)
            {
                if (!UnitsTopologicallySortedList.Contains(currentUnit))//vnimanie zdes inogda pri silnoj zavisimosti modulej moduli popadajut neskolko raz
                    UnitsTopologicallySortedList.Add(currentUnit);
            }

            OnChangeCompilerState(this, CompilerState.EndCompileFile, unitFileName);

            //SavePCU(currentUnit, unitFileName);
            currentUnit.UnitFileName = unitFileName;

            return currentUnit;


            /*if(currentUnit.State!=UnitState.Compiled)
            { 
                //Console.WriteLine("Compile Interface "+unitFileName);//DEBUG
                currentUnit.SemanticTree=SyntaxTreeToSemanticTreeConverter.CompileInterface(currentUnit.SyntaxTree,
                    currentUnit.InterfaceUsedUnits,currentUnit.syntax_error);
                currentUnit.State=UnitState.InterfaceCompiled;
                implementationUsesList=GetSemanticImplementationUsesList(currentUnit.SyntaxTree);
                if(implementationUsesList!=null)
                    for(int i=implementationUsesList.Count-1;i>=0;i--)
                        CompileUnit(currentUnit.ImplementationUsedUnits,implementationUsesList[i]);        
                //Console.WriteLine("Compile Implementation "+unitFileName);//DEBUG
                if (currentUnit.SyntaxTree is SyntaxTree.unit_module)
                {
                    SyntaxTreeToSemanticTreeConverter.CompileImplementation(currentUnit.SemanticTree,
                        currentUnit.SyntaxTree,currentUnit.ImplementationUsedUnits,currentUnit.syntax_error);
                }
                currentUnit.State=UnitState.Compiled;
                unitsFromUsesSection.Add(currentUnit.SemanticTree);
                SaveSemanticTreeToFile(currentUnit,unitFileName);
            }*/
        }

        private void CreateDependencyListsForCurrentUnit(CompilationUnit currentUnit, string currentDirectory, out List<SyntaxTree.unit_or_namespace> interfaceUsesList,
            out unit_node_list references, out Dictionary<string, SyntaxTree.syntax_namespace_node> namespaces)
        {
            interfaceUsesList = GetInterfaceUsesSection(currentUnit.SyntaxTree);

            SetUseDLLForSystemUnits(currentDirectory, interfaceUsesList, interfaceUsesList.Count - 1 - currentUnit.InterfaceUsedUnits.Count);

            references = GetReferences(currentUnit);

            // Надо подумать, как мы будем подключать про-ва имен из других языков  EVA
            namespaces = PrepareUserNamespacesUsedInTheCurrentUnit(currentUnit);
        }

        private void AddCurrentUnitAndItsReferencesToUsesLists(unit_node_list unitsFromUsesSection, Dictionary<unit_node, CompilationUnit> directUnitsFromUsesSection,
            SyntaxTree.unit_or_namespace currentUnitNode, CompilationUnit currentUnit, unit_node_list references)
        {
            if (unitsFromUsesSection != null)
            {
                if (unitsFromUsesSection.AddElement(currentUnit.SemanticTree, currentUnitNode.UsesPath()))
                    directUnitsFromUsesSection.Add(currentUnit.SemanticTree, currentUnit);
                unitsFromUsesSection.AddRange(references);
            }
        }

        private void SemanticCheckUsesInIsNotNamespace(SyntaxTree.unit_or_namespace currentUnitNode, CompilationUnit currentUnit)
        {
            if (currentUnit.SemanticTree is dot_net_unit_node
                            && currentUnitNode is SyntaxTree.uses_unit_in ui && ui.in_file != null) // значит, это пространство имен и секция in у него должна отсутствовать
            {
                ErrorsList.Add(new NamespaceCannotHaveInSection(ui.in_file.source_context));
            }
        }

        private void CompileCurrentUnitImplementation(string UnitFileName, CompilationUnit currentUnit, Dictionary<SyntaxTree.syntax_tree_node, string> docs)
        {
            if (currentUnit.SyntaxTree is SyntaxTree.unit_module)
            {
#if DEBUG
                if (InternalDebug.SemanticAnalysis)
#endif
                {
                    OnChangeCompilerState(this, CompilerState.CompileImplementation, UnitFileName);

                    TreeConverter.SemanticRulesConstants.SymbolTableCaseSensitive = currentUnit.CaseSensitive;

                    var data = new TreeConverter.TreeConversion.InitializationDataForCompilingImplementation(
                        currentUnit.syntax_error,
                        BadNodesInSyntaxTree,
                        buildImplementationUsesList(currentUnit),
                        currentUnit.InterfaceUsingNamespaceList,
                        currentUnit.ImplementationUsingNamespaceList,
                        currentUnit.SyntaxTree,
                        (common_unit_node)currentUnit.SemanticTree,
                        ErrorsList, Warnings,
                        docs,
                        CompilerOptions.Debug,
                        CompilerOptions.ForDebugging,
                        CompilerOptions.ForIntellisense
                        );

                    SyntaxTreeToSemanticTreeConverter.CompileImplementation(currentUnit.Language, data, CompiledVariables);
                    CheckErrorsAndThrowTheFirstOne();
                }
            }
        }

        /// <summary>
        /// Компилирует модули из секции uses текущего модуля реализации рекурсивно
        /// </summary>
        private void CompileImplementationDependencies(string currentPath, CompilationUnit currentUnit, List<SyntaxTree.unit_or_namespace> implementationUsesList,
            Dictionary<string, SyntaxTree.syntax_namespace_node> namespaces, common_unit_node commonUnitNode, out bool shouldReturnCurrentUnit)

        {
            shouldReturnCurrentUnit = false;

            if (implementationUsesList != null)
            {
                for (int i = implementationUsesList.Count - 1; i >= 0; i--)
                {
                    if (!IsPossibleNetNamespaceOrStandardPasFile(implementationUsesList[i], true, currentPath))
                    {
                        CompilationUnit unitFromUsesSection = UnitTable[Path.ChangeExtension(GetUnitFileName(implementationUsesList[i], currentPath), null)];

                        // защита от попадания в бесконечный цикл (когда мы вернемся в тот юнит, в котором уже были (еще не скомпилированный), а затем спустимся по дереву зависимостей сюда же и т.д.)
                        // первая часть условия - если мы встречаем юнит не первый раз, вторая часть - если интерфейс еще не скомпилирован
                        if (unitFromUsesSection != null && unitFromUsesSection.State == UnitState.BeginCompilation)
                        {
                            UnitsToCompileDelayedList.Add(unitFromUsesSection);
                            shouldReturnCurrentUnit = true; // обрубаем компиляцию реализации в CompileUnit - не все интерфейсы еще откомпилированы !!!
#if DEBUG
                            // Console.WriteLine("[DEBUGINFO]Send compile to end " + Path.GetFileName(GetUnitFileName(implementationUsesList[i])));//DEBUG
#endif
                        }
                        else
                        {
                            CompileUnit(currentUnit.ImplementationUsedUnits, currentUnit.ImplementationUsedDirectUnits, implementationUsesList[i], currentPath);
                        }
                    }
                    else
                    {
                        currentUnit.ImplementationUsedUnits.AddElement(new TreeRealization.namespace_unit_node(GetNamespace(implementationUsesList[i])), null);
                        currentUnit.possibleNamespaces.Add(implementationUsesList[i]);
                    }
                }

            }

            currentCompilationUnit = currentUnit;

            AddNamespacesToUsingList(currentUnit.ImplementationUsingNamespaceList, currentUnit.possibleNamespaces, true, namespaces);

            #region USING LIST LEGACY CODE
            AddNamespacesToUsingList(currentUnit.ImplementationUsingNamespaceList, GetImplementationSyntaxUsingList(currentUnit.SyntaxTree));
            #endregion

            if (shouldReturnCurrentUnit)
            {
                // помещаем текущий модуль в список отложенной компиляции
                UnitsToCompileDelayedList.Add(currentUnit);

                if (commonUnitNode != null)
                {
                    if (!UnitsTopologicallySortedList.Contains(currentUnit))//vnimanie zdes inogda pri silnoj zavisimosti modulej moduli popadajut neskolko raz
                        UnitsTopologicallySortedList.Add(currentUnit);
                }
                //Console.WriteLine("Send compile to end "+unitFileName);//DEBUG
            }
        }

        private void CompileCurrentUnitInterface(string UnitFileName, CompilationUnit currentUnit, Dictionary<SyntaxTree.syntax_tree_node, string> docs)
        {
#if DEBUG
            if (InternalDebug.SemanticAnalysis)
#endif
            {
                if (currentUnit.State != UnitState.InterfaceCompiled)
                {
                    OnChangeCompilerState(this, CompilerState.CompileInterface, UnitFileName);
                    TreeConverter.SemanticRulesConstants.SymbolTableCaseSensitive = currentUnit.CaseSensitive;

                    var data = new TreeConverter.TreeConversion.InitializationDataForCompilingInterface(
                        currentUnit.syntax_error,
                        BadNodesInSyntaxTree,
                        currentUnit.InterfaceUsedUnits,
                        currentUnit.InterfaceUsingNamespaceList,
                        currentUnit.SyntaxTree,
                        ErrorsList, Warnings,
                        docs,
                        CompilerOptions.Debug,
                        CompilerOptions.ForDebugging,
                        CompilerOptions.ForIntellisense
                        );
                    
                    currentUnit.SemanticTree = SyntaxTreeToSemanticTreeConverter.CompileInterface(currentUnit.Language, data, CompiledVariables);
                    
                    CheckErrorsAndThrowTheFirstOne();
                }
            }
        }

        /// <summary>
        /// Компилирует модули из секции uses интерфейса текущего модуля рекурсивно
        /// </summary>
        /// <exception cref="CycleUnitReference"></exception>
        private void CompileInterfaceDependencies(unit_node_list unitsFromUsesSection, Dictionary<unit_node, CompilationUnit> directUnitsFromUsesSection, SyntaxTree.unit_or_namespace currentUnitNode,
            string unitFileName, string currentPath, CompilationUnit currentUnit, List<SyntaxTree.unit_or_namespace> interfaceUsesList, unit_node_list references,
            Dictionary<string, SyntaxTree.syntax_namespace_node> namespaces, out bool shouldReturnCurrentUnit)
        {
            shouldReturnCurrentUnit = false;
            for (int i = interfaceUsesList.Count - 1 - currentUnit.InterfaceUsedUnits.Count; i >= 0; i--) // здесь откидываются модули с уже откомпилированными интерфейсами из секции uses (см. комментарий, обозначенный #1710)
            {
                if (IsPossibleNetNamespaceOrStandardPasFile(interfaceUsesList[i], true, currentPath) || namespaces.ContainsKey(interfaceUsesList[i].name.idents[0].name))
                {
                    currentUnit.InterfaceUsedUnits.AddElement(new namespace_unit_node(GetNamespace(interfaceUsesList[i])), null);
                    currentUnit.possibleNamespaces.Add(interfaceUsesList[i]);
                }
                else
                {
                    #region SEMANTIC CHECKS : CYCLE DEPENDENCY OF INTERFACES

                    SemanticCheckNoLoopDependenciesOfInterfaces(currentUnit, unitFileName, interfaceUsesList[i], currentPath);

                    #endregion

                    // компиляция модулей из интерфейса текущего модуля 
                    CompileUnit(currentUnit.InterfaceUsedUnits, currentUnit.InterfaceUsedDirectUnits, interfaceUsesList[i], currentPath);

                    // если текущий модуль был откомпилирован в другом рекурсивном вызове 
                    if (currentUnit.State == UnitState.Compiled)
                    {
                        AddCurrentUnitAndItsReferencesToUsesLists(unitsFromUsesSection, directUnitsFromUsesSection, currentUnitNode, currentUnit, references); // #1710 добавление в список модулей из uses происходит только в конце компиляции интерфейса юнита или позже во всех случаях
                        shouldReturnCurrentUnit = true;
                    }
                }

            }
        }

        private void SemanticCheckNoLoopDependenciesOfInterfaces(CompilationUnit currentUnit, string unitFileName, SyntaxTree.unit_or_namespace usedUnitNode, string currentPath)
        {
            var usedUnitFileName = GetUnitFileName(usedUnitNode, currentPath);
            var usedUnitId = Path.ChangeExtension(usedUnitFileName, null);

            // когда образуется цикл здесь сохранится смежная вершина графа (используемый юнит), которая тоже принадлежит циклу
            currentUnit.currentUsedUnitId = usedUnitId;

            CompilationUnit usedUnit = UnitTable[usedUnitId];

            // если используемый юнит имеет не скомпилированный интерфейс, но был инициализирован (то есть мы попали в цикл)
            if (usedUnit != null && usedUnit.State == UnitState.BeginCompilation)
            {
                // если в цикле где-то присутствует дуга из implementation (тогда интерфейс этого модуля будет откомпилирован), то такой цикл допускается
                while (usedUnit != currentUnit)
                {
                    string nextUnitId = usedUnit.currentUsedUnitId;
                    usedUnit = UnitTable[nextUnitId];
                    if (usedUnit.State != UnitState.BeginCompilation)
                        return;
                }

                throw new CycleUnitReference(unitFileName, usedUnitNode);
            }
        }

        /// <summary>
        /// Если в программе в секции uses есть не про-во имен и не стандартный модуль, то использование PABCRtl.dll отменяется
        /// </summary>
        private void SetUseDLLForSystemUnits(string currentDirectory, List<SyntaxTree.unit_or_namespace> usesList, int lastUnitIndex)
        {
            if (usesList != null && CompilerOptions.UseDllForSystemUnits)
            {
                for (int i = lastUnitIndex; i >= 0; i--)
                {
                    if (!IsPossibleNetNamespaceOrStandardPasFile(usesList[i], false, currentDirectory))
                    {
                        CompilerOptions.UseDllForSystemUnits = false;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Получение исходного кода модуля, заполнение документации,
        /// генерация синтаксического дерева,
        /// обработка синтаксических ошибок
        /// </summary>
        private void InitializeNewUnit(string unitFileName, string UnitId, ref CompilationUnit currentUnit, out Dictionary<SyntaxTree.syntax_tree_node, string> docs)
        {
            currentUnit = new CompilationUnit();
            if (firstCompilationUnit == null)
                firstCompilationUnit = currentUnit;

            currentCompilationUnit = currentUnit;

            // запоминание языка
            currentUnit.Language = LanguageProvider.SelectLanguageByExtension(unitFileName);

            currentUnit.CaseSensitive = currentUnit.Language.CaseSensitive;
            
            // получение итогового синтаксического дерева после сахарных преобразований
            ConstructSyntaxTreeAndRunSugarConversions(unitFileName, currentUnit, out docs);

            InitializeCompilerOptionsRelatedToStandardUnits(currentUnit.SyntaxTree);

            RunSemanticChecks(unitFileName, currentUnit);

            // местоположение этой строчки важно, потому что проверяется UnitTable.Count > 0 выше  EVA
            UnitTable[UnitId] = currentUnit;

            // здесь добавляем стандартные модули в секцию uses интерфейса
            if (!CompilerOptions.DisableStandardUnits)
                AddStandardUnitsToInterfaceUsesSection(currentUnit);
            else
            {
                 AddReferencesToNetSystemLibraries(currentUnit, currentUnit.SyntaxTree.compiler_directives);
            }
        }


        /// <summary>
        /// Строит синтаксическое дерево, бросает первую из найденных ошибок (если они есть) и запускает сахарные преобразования
        /// </summary>
        private void ConstructSyntaxTreeAndRunSugarConversions(string unitFileName, CompilationUnit currentUnit, out Dictionary<SyntaxTree.syntax_tree_node, string> docs)
        {
            OnChangeCompilerState(this, CompilerState.BeginCompileFile, unitFileName); // начало компиляции модуля
            
            // получение синтаксического дерева
            string sourceText = GetSourceCode(unitFileName, currentUnit);

            currentUnit.SyntaxTree = ConstructSyntaxTree(unitFileName, currentUnit, sourceText);

            // сопоставление нодам ошибок     EVA
            MatchSyntaxErrorsToBadNodes(currentUnit);

            CheckErrorsAndThrowTheFirstOne();

            // SSM 2/05/16 - для преобразования синтаксических деревьев извне (синтаксический сахар)
            currentUnit.SyntaxTree = ConvertSyntaxTree(currentUnit.SyntaxTree, currentUnit.Language.SyntaxTreeConverters);

            // генерация документации к узлам синтаксического дерева EVA
            docs = GenUnitDocumentation(currentUnit, sourceText);
        }

        /// <summary>
        /// Устанавливает значения опций DisableStandardUnits и UseDllForSystemUnits
        /// </summary>
        private void InitializeCompilerOptionsRelatedToStandardUnits(SyntaxTree.compilation_unit unitSyntaxTree)
        {
            // проверяем только для основной программы или dll
            if (UnitTable.Count == 0)
            {
                var disableStandardUnitsDirective = unitSyntaxTree.compiler_directives.Find(directive =>
                            directive.Name.text.Equals(StringConstants.compiler_directive_disable_standard_units, StringComparison.CurrentCultureIgnoreCase));

                if (disableStandardUnitsDirective != null)
                    CompilerOptions.DisableStandardUnits = true;
            }

            if (unitSyntaxTree is SyntaxTree.unit_module)
                CompilerOptions.UseDllForSystemUnits = false;
        }

        /// <summary>
        /// Семантические проверки по директивам и по типу файла
        /// </summary>
        private void RunSemanticChecks(string unitFileName, CompilationUnit currentUnit)
        {
            // SSM 21/05/20 Проверка, что мы не записали apptype dll в небиблиотеку
            bool isDll = IsDll(currentUnit.SyntaxTree, out var dllDirective);
            SemanticCheckDLLDirectiveOnlyForLibraries(currentUnit.SyntaxTree, isDll, dllDirective);

            // ошибка - компилируем вторую основную программу или вторую dll вместо юнита
            SemanticCheckCurrentUnitMustBeUnitModule(unitFileName, currentUnit, isDll);

            // ошибка директива include в паскалевском юните
            SemanticCheckNoIncludeNamespaceDirectivesInUnit(currentUnit);

            SemanticCheckDisableStandardUnitsDirectiveInUnit(currentUnit.SyntaxTree);

            CheckErrorsAndThrowTheFirstOne();
        }

        private SyntaxTree.compilation_unit ConvertSyntaxTree(SyntaxTree.compilation_unit syntaxTree, List<ISyntaxTreeConverter> converters)
        {
            foreach (ISyntaxTreeConverter converter in converters)
            {
                OnChangeCompilerState(this, CompilerState.SyntaxTreeConversion, converter.Name);
                syntaxTree = converter.Convert(syntaxTree) as SyntaxTree.compilation_unit;
            }

            return syntaxTree;
        }

        private void SemanticCheckNamespacesOnlyInProjects(CompilationUnit currentUnit)
        {
            // legacy 
            if (currentUnit.SyntaxTree is SyntaxTree.unit_module)
            {
                // Проверка на явный namespace (паскалевский)
                if ((currentUnit.SyntaxTree as SyntaxTree.unit_module).unit_name.HeaderKeyword == SyntaxTree.UnitHeaderKeyword.Namespace)
                    throw new NamespacesCanBeCompiledOnlyInProjects(currentUnit.SyntaxTree.source_context);
            }
        }

        // Синтактико-семантическая ошибка - проверка, что compilationUnit является модулем,
        // а не основной программой и не dll EVA
        private void SemanticCheckCurrentUnitMustBeUnitModule(string UnitFileName, CompilationUnit currentUnit, bool isDll)
        {
            if (UnitTable.Count > 0) // если это не главный модуль (программа в unittable всегда идет первой)
            {
                if (currentUnit.SyntaxTree is SyntaxTree.program_module)
                    throw new UnitModuleExpected(UnitFileName, currentUnit.SyntaxTree.source_context.LeftSourceContext);
                else if (isDll)
                    throw new UnitModuleExpectedLibraryFound(UnitFileName, currentUnit.SyntaxTree.source_context.LeftSourceContext);
            }
        }

        private void MatchSyntaxErrorsToBadNodes(CompilationUnit currentUnit)
        {
            if (errorsList.Count > 0)
            {
                currentUnit.syntax_error = errorsList[0] as SyntaxError;
                foreach (Error er in errorsList)
                    if (er is SyntaxError && (er as SyntaxError).bad_node != null)
                        BadNodesInSyntaxTree[(er as SyntaxError).bad_node] = er;
            }
        }


        /// <summary>
        /// Проверка, что директива dll только в Library - требует передачи директивы dll
        /// </summary>
        private void SemanticCheckDLLDirectiveOnlyForLibraries(SyntaxTree.compilation_unit unitSyntaxTree, bool isDll, SyntaxTree.compiler_directive dllDirective)
        {
            // Если Library и apptype dll не указано, то никакой ошибки нет  EVA
            if (isDll)
            {
                if (!(unitSyntaxTree is SyntaxTree.unit_module) ||
                            (unitSyntaxTree is SyntaxTree.unit_module unitNode && unitNode.unit_name.HeaderKeyword != SyntaxTree.UnitHeaderKeyword.Library))
                {
                    // если в директивах появилось {$apptype dll}, но это не Library
                    ErrorsList.Add(new AppTypeDllIsAllowedOnlyForLibraries(unitSyntaxTree.file_name, dllDirective.source_context));
                }
            }
        }

        /// <summary>
        /// Ошибка указания директивы DisableStandardUnits в подключенном модулей
        /// </summary>
        /// 
        private void SemanticCheckDisableStandardUnitsDirectiveInUnit(SyntaxTree.compilation_unit unitSyntaxTree)
        {
            // проверяем для используемых модулей
            if (UnitTable.Count > 0)
            {
                var foundDirective = unitSyntaxTree.compiler_directives.Find(directive =>
                            directive.Name.text.Equals(StringConstants.compiler_directive_disable_standard_units, StringComparison.CurrentCultureIgnoreCase));

                if (foundDirective != null)
                {
                    ErrorsList.Add(new DisableStandardUnitsDirectiveDisallowedInUsedUnits(unitSyntaxTree.file_name, foundDirective.source_context));
                }
            }
        }

        private SyntaxTree.compilation_unit ConstructSyntaxTree(string unitFileName, CompilationUnit currentUnit, string sourceText)
        {
            List<string> DefinesList = new List<string> { "PASCALABC" };
            if (!CompilerOptions.Debug && !CompilerOptions.ForDebugging)
                DefinesList.Add("RELEASE");
            else
                DefinesList.Add("DEBUG");
            DefinesList.AddRange(CompilerOptions.ForceDefines);

            SyntaxTree.compilation_unit syntaxTree;

            if (CompilerOptions.UnitSyntaxTree != null)
            {
                syntaxTree = CompilerOptions.UnitSyntaxTree;
                CompilerOptions.UnitSyntaxTree = null;
            }
            // синтаксический анализ
            else
                syntaxTree = InternalParseText(currentUnit.Language, unitFileName, sourceText, errorsList, warnings, DefinesList);

            // проверка, что пространства имен только в проектах
            SemanticCheckNamespacesOnlyInProjects(currentUnit);

            return syntaxTree;
        }

        private string GetSourceCode(string UnitFileName, CompilationUnit currentUnit)
        {
            string SourceText = null;
            if (CompilerOptions.UnitSyntaxTree == null)
            {
                SourceText = GetSourceFileText(UnitFileName);
                if (SourceText == null)
                {
                    if (currentUnit == firstCompilationUnit)
                        throw new SourceFileNotFound(UnitFileName);
                    else
                        throw new UnitNotFound(currentUnit.SyntaxTree.file_name, UnitFileName, currentUnit.SyntaxTree.source_context);
                }
            }
            return SourceText;
        }

        /// <summary>
        /// Возвращает true, если текущий компилируемый модуль не является основной программой (program_module)
        /// 
        /// </summary>
        private bool CurrentUnitIsNotMainProgram()
        {
            return UnitTable.Count > 0;
        }


        private Dictionary<SyntaxTree.syntax_tree_node, string> GenUnitDocumentation(CompilationUnit currentUnit, string SourceText)
        {
            Dictionary<SyntaxTree.syntax_tree_node, string> docs = null;

            if (IsDocumentationNeeded(currentUnit.SyntaxTree))
            {
                if (SourceText != null)
                {
                    docs = AddDocumentationToNodes(currentUnit, SourceText);
                    if (docs != null)
                        currentUnit.Documented = true;
                }
            }
            return docs;
        }

        private bool UnitHasPCU(unit_node_list unitsFromUsesSection, Dictionary<unit_node, CompilationUnit> directUnitsFromUsesSection, SyntaxTree.unit_or_namespace currentUnitNode, ref string UnitFileName, ref CompilationUnit currentUnit)
        {
            if (Path.GetExtension(UnitFileName).ToLower() == CompilerOptions.CompiledUnitExtension)
            {
                if (File.Exists(UnitFileName))
                {
                    if (UnitTable.Count == 0) throw new ProgramModuleExpected(UnitFileName, null);
                    try
                    {
                        if ((currentUnit = ReadPCU(UnitFileName)) != null)
                        {
                            AddCurrentUnitAndItsReferencesToUsesLists(unitsFromUsesSection, directUnitsFromUsesSection, 
                                                                      currentUnitNode, currentUnit, GetReferences(currentUnit));

                            UnitTable[Path.ChangeExtension(UnitFileName, null)] = currentUnit;
                            return true;
                        }
                    }
                    catch (InvalidPCUFile)
                    {
                        //Перекомпилируем....
                    }
                    // Так надо (для дебага)
                    catch (Error)
                    {
                        throw;
                    }
                    catch (Exception e)
                    {
                        OnChangeCompilerState(this, CompilerState.PCUReadingError, UnitFileName); // ошибка чтения PCU
#if DEBUG
                        if (!InternalDebug.SkipPCUErrors)
                            throw new CompilerInternalError("PCUReader", e);
#endif
                    }
                    string SourceFileName = FindSourceFileName(Path.ChangeExtension(UnitFileName, null), null, out _);
                    if (SourceFileName == null)
                        throw new ReadPCUError(UnitFileName);
                    else
                        UnitFileName = SourceFileName;
                }
            }

            return false;
        }

        private Dictionary<SyntaxTree.syntax_tree_node, string> AddDocumentationToNodes(CompilationUnit currentUnit, string text)
        {
            SyntaxTree.documentation_comment_list docCommentList = currentUnit.Language.DocParser.BuildTree(text);
            
            return new DocumentationConstructor().Construct(currentUnit.SyntaxTree, docCommentList);
        }

        private bool IsDocumentationNeeded(SyntaxTree.compilation_unit unitSyntaxTree)
        {
            if (project != null && project.generate_xml_doc)
                return true;
            
            if (unitSyntaxTree == null)
                return false;
            
            foreach (SyntaxTree.compiler_directive directive in unitSyntaxTree.compiler_directives)
            {
                if (string.Equals(directive.Name.text, "gendoc", StringComparison.CurrentCultureIgnoreCase)
                    && string.Equals(directive.Directive.text, "true", StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private TreeRealization.unit_node_list buildImplementationUsesList(CompilationUnit cu)
        {
            TreeRealization.unit_node_list unl = new PascalABCCompiler.TreeRealization.unit_node_list();
            unl.AddRange(cu.ImplementationUsedUnits);
            foreach (TreeRealization.unit_node un in cu.InterfaceUsedUnits)
            {
                if (un is TreeRealization.dot_net_unit_node)
                    unl.AddElement(un, null);
            }
            return unl;
        }

        public void SavePCU(CompilationUnit Unit)
        {
            //#if DEBUG
            try
            {
                if (Unit.SyntaxTree != null && Unit.SyntaxTree is SyntaxTree.unit_module)
                {
                    if (((SyntaxTree.unit_module)Unit.SyntaxTree).unit_name.HeaderKeyword == PascalABCCompiler.SyntaxTree.UnitHeaderKeyword.Library)
                        return;
                    foreach (SyntaxTree.compiler_directive cd in Unit.SyntaxTree.compiler_directives)
                        if (cd.Name.text.ToLower() == StringConstants.compiler_directive_savepcu)
                            if (!Convert.ToBoolean(cd.Directive.text))
                                return;
                }
            }
            catch
            {
            }
            //#endif
            PCUWriter writer = null;
            try
            {
#if DEBUG
                if (InternalDebug.PCUGenerate)
#endif
                    if (CompilerOptions.SavePCU)
                        if ((Unit.SemanticTree as TreeRealization.common_unit_node).namespaces.Count > 1 &&
                            Unit.SyntaxTree != null &&
                            Unit.State == UnitState.Compiled)
                        {
                            writer = new PCUWriter(this, pr_ChangeState);

                            bool dbginfo = true;/*CompilerOptions.Debug*/
#if DEBUG
                            dbginfo = InternalDebug.IncludeDebugInfoInPCU;
#endif

                            writer.SaveSemanticTree(Unit, Path.ChangeExtension(Unit.UnitFileName, CompilerOptions.CompiledUnitExtension), dbginfo);

                        }
            }
            catch (Exception err)
            {
                //ErrorsList.Add(new Errors.CompilerInternalError(string.Format("Compiler.Compile[{0}]", Path.GetFileName(this.currentCompilationUnit.SyntaxTree.file_name)), err));
                OnChangeCompilerState(this, CompilerState.PCUWritingError, Unit.UnitFileName);
#if DEBUG
                if (!InternalDebug.SkipPCUErrors)
                    throw new CompilerInternalError(string.Format("Compiler.Compile[{0}]", Path.GetFileName(this.currentCompilationUnit.SyntaxTree.file_name)), err);
                writer.RemoveSelf();
#endif
            }
        }

        public CompilationUnit ReadPCU(string FileName)
        {
            if (CompilerOptions.ForIntellisense && false)
            {
                CompilationUnit unit = null;
                if (pcuCompilationUnits.ContainsKey(FileName))
                {
                    unit = pcuCompilationUnits[FileName];
                    return unit;
                }
                PCUReader pr = new PCUReader(this, pr_ChangeState);
                unit = pr.GetCompilationUnit(FileName, CompilerOptions.Debug);
                pcuCompilationUnits[FileName] = unit;
                return unit;
            }
            else
            {
                PCUReader pr = new PCUReader(this, pr_ChangeState);
                return pr.GetCompilationUnit(FileName, CompilerOptions.Debug);
            }

        }

        void pr_ChangeState(object Sender, PCUReaderWriterState State, object obj)
        {
            switch (State)
            {
                case PCUReaderWriterState.BeginReadTree:
                    OnChangeCompilerState(this, CompilerState.ReadPCUFile, (Sender as PCUReader).FileName);
                    break;
                case PCUReaderWriterState.EndReadTree:
                    CompilationUnit cu = obj as CompilationUnit;
                    cu.State = UnitState.Compiled;
                    unitTable[(Sender as PCUReader).FileName] = cu;
                    UnitsTopologicallySortedList.Add(cu);
                    GetReferences(cu);
                    break;
                case PCUReaderWriterState.EndSaveTree:
                    OnChangeCompilerState(this, CompilerState.SavePCUFile, (Sender as PCUWriter).FileName);
                    break;

            }
        }
        /*                            TreeRealization.common_unit_node cun11 = currentUnit.SemanticTree as TreeRealization.common_unit_node;
                            if (cun11 != null)
                                UnitsLogicallySortedList.AddElement(cun11);*/

        static string standartAssemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(string)).ManifestModule.FullyQualifiedName);

        public static Dictionary<string, string> standart_assembly_dict = new Dictionary<string, string>();
        static Compiler()
        {
            string[] ss = new string[] { "mscorlib.dll", "System.dll", "System.Core.dll", "System.Numerics.dll", "System.Windows.Forms.dll", "PABCRtl.dll" };
            foreach (var x in ss)
                standart_assembly_dict[x] = get_standart_assembly_path(x);
        }
        public static string get_standart_assembly_path(string name)
        {
            name = name.Replace("%GAC%\\", "");
            string ttn = System.IO.Path.GetFileNameWithoutExtension(name);
            string tn = Path.Combine(standartAssemblyPath, name);
            if (File.Exists(tn))
                return tn;
            if (Environment.OSVersion.Platform != PlatformID.Unix && Environment.OSVersion.Platform != PlatformID.MacOSX)
            {
                string windir = Path.Combine(Environment.GetEnvironmentVariable("windir"), "Microsoft.NET");
                tn = windir + @"\assembly\GAC_MSIL\";
                tn += ttn + "\\";
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(tn);
                if (!di.Exists)
                {
                    tn = windir + @"\assembly\GAC_64\";
                    tn += ttn + "\\";
                    di = new System.IO.DirectoryInfo(tn);
                    if (!di.Exists)
                    {
                        tn = windir + @"\assembly\GAC_32\";
                        tn += ttn + "\\";
                        di = new System.IO.DirectoryInfo(tn);
                        if (!di.Exists)
                        {
                            tn = windir + @"\assembly\GAC\";
                            tn += ttn + "\\";
                            di = new System.IO.DirectoryInfo(tn);
                            if (!di.Exists)
                            {
                                windir = Environment.GetEnvironmentVariable("windir");
                                tn = windir + @"\assembly\GAC_MSIL\";
                                tn += ttn + "\\";
                                di = new System.IO.DirectoryInfo(tn);
                                if (!di.Exists)
                                {
                                    tn = windir + @"\assembly\GAC_64\";
                                    tn += ttn + "\\";
                                    di = new System.IO.DirectoryInfo(tn);
                                    if (!di.Exists)
                                    {
                                        tn = windir + @"\assembly\GAC_32\";
                                        tn += ttn + "\\";
                                        di = new System.IO.DirectoryInfo(tn);
                                        if (!di.Exists)
                                        {
                                            tn = windir + @"\assembly\GAC\";
                                            tn += ttn + "\\";
                                            di = new System.IO.DirectoryInfo(tn);
                                            if (!di.Exists)
                                            {
                                                return null;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                System.IO.DirectoryInfo[] diarr = di.GetDirectories();
                tn = Path.Combine((diarr[0]).FullName, name);
            }
            else
            {
                string gac_path = "/usr/lib/mono/4.0/gac";
                DirectoryInfo di = new DirectoryInfo(Path.Combine(gac_path, ttn));
                if (di.Exists)
                {
                    System.IO.DirectoryInfo[] diarr = di.GetDirectories();
                    tn = Path.Combine((diarr[diarr.Length - 1]).FullName, name);
                }
                else
                    return null;

            }
            return tn;

        }
        public static string get_assembly_path(string name, bool search_for_intellisense)
        {
            //если явно задан каталог то ищем только там
            if (Environment.OSVersion.Platform != PlatformID.Unix && Environment.OSVersion.Platform != PlatformID.MacOSX && !name.StartsWith("%GAC%\\"))
            {
                if (Path.GetDirectoryName(name) != string.Empty)
                    if (File.Exists(name))
                        return name;
                    else
                        return null;
            }
            if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                string SystemDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);
                string SearchDirectory = Path.Combine(SystemDirectory, "Lib");
                if (File.Exists(Path.Combine(Environment.CurrentDirectory, name)))
                    return Path.Combine(Environment.CurrentDirectory, name);
                if (File.Exists(Path.Combine(SearchDirectory, name)))
                {
                    File.Copy(Path.Combine(SearchDirectory, name), Path.Combine(Environment.CurrentDirectory, name), true);
                    return Path.Combine(Environment.CurrentDirectory, name);
                }

            }
            if (!search_for_intellisense && !name.StartsWith("%GAC%\\"))
            {
                string dir = Environment.CurrentDirectory;

                if (File.Exists(Path.Combine(dir, name)))
                {
                    return Path.Combine(dir, name);
                }
            }

            return get_standart_assembly_path(name);
        }

        public CompilationUnit ReadDLL(string FileName, SyntaxTree.SourceContext sc = null)
        {
            if (DLLCache.ContainsKey(FileName))
                return DLLCache[FileName];
            OnChangeCompilerState(this, CompilerState.ReadDLL, FileName);
            TreeRealization.using_namespace_list using_namespaces = new TreeRealization.using_namespace_list();
            try
            {
                TreeRealization.dot_net_unit_node un =
                    new TreeRealization.dot_net_unit_node(new NetHelper.NetScope(using_namespaces,
                    /*System.Reflection.Assembly.LoadFrom(file_name)*/
                    PascalABCCompiler.NetHelper.NetHelper.LoadAssembly(FileName), SyntaxTreeToSemanticTreeConverter.SymbolTable));
                CompilationUnit cu = new CompilationUnit();
                cu.SemanticTree = un;

                //un.dotNetScope=new PascalABCCompiler.NetHelper.NetScope(using_namespaces,
                //	System.Reflection.Assembly.LoadFrom(file_name),SyntaxTreeToSemanticTreeConverter.SymbolTable);
                DLLCache[FileName] = cu;

                return cu;
            }
            catch (ReflectionTypeLoadException e)
            {
                foreach (var assm in assemblyResolveScope.missingAssemblies)
                    errorsList.Add(new AssemblyNotFound(currentCompilationUnit.UnitFileName, assm, sc));
                /*Console.Error.WriteLine(e.Message);
                foreach (var eLoaderException in e.LoaderExceptions)
                {
                    Console.Error.WriteLine(eLoaderException.Message);
                }*/

                return null;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return null;
            }
        }

        /*public CompilationUnit RecompileUnit(string unit_name)
        {
            Console.WriteLine("recompile {0}", unit_name);
            currentUnit = new SyntaxTree.uses_unit_in(new SyntaxTree.string_const(program_folder + "\\" + unit_name+".pas"));
            CompileUnit(unitsFromUsesSection, compilationUnit);
            CompilationUnit compilationUnit = new CompilationUnit();
            if (unitsFromUsesSection.Count != 0)
                compilationUnit.SemanticTree = unitsFromUsesSection[unitsFromUsesSection.Count - 1];
            else
                return null;
            return compilationUnit;
        }*/
        public bool NeedRecompiled(string pcu_name, string[] included, PCUReader pr)
        {
            if (!Path.IsPathRooted(pcu_name)) throw new InvalidOperationException();
            string pas_name = FindSourceFileName(Path.ChangeExtension(pcu_name, null), null, out _);
            var dir = Path.GetDirectoryName(pcu_name);

            if (UnitTable[Path.ChangeExtension(pas_name, null)] != null)
                return true;

            bool need = false;
            for (int i = 0; i < included.Length; i++)
            {
                //if (included[i].Contains("$"))
                //	continue;
                var used_unit_fname = GetUnitFileName(Path.GetFileNameWithoutExtension(included[i]), included[i], dir, null);
                var used_unit_is_pcu = Path.GetExtension(used_unit_fname) == CompilerOptions.CompiledUnitExtension;

                if (!used_unit_is_pcu)
                {
                    if (UnitTable[Path.ChangeExtension(used_unit_fname, null)] != null) return true;
                    need = true;
                    RecompileList[pcu_name] = pcu_name;
                }
            }
            if (need) return true;

            if (!SourceFileExists(pas_name)) return false;

            // NeedRecompiled вызывается уже после успешного прочтения заголовка модуля (иначе откуда included)
            //if (!File.Exists(pcu_name)) return true;

            if (File.GetLastWriteTime(pcu_name) < SourceFileGetLastWriteTime(pas_name)) return true;
            //Console.WriteLine("{0} {1}",name,RecompileList.Count);
            for (int i = 0; i < included.Length; i++)
            {
                string pcu_name2 = FindPCUFileName(included[i], dir, out _);
                //TODO: Спросить у Сащи насчет < и <=.

                if ((File.Exists(pcu_name2) && File.GetLastWriteTime(pcu_name) < File.GetLastWriteTime(pcu_name2) && !pr.AlreadyCompiled(pcu_name2)))
                {
                    pr.AddAlreadyCompiledUnit(pcu_name2);
                    return true;
                }
                if (RecompileList.ContainsKey(pcu_name2))
                {
                    return true;
                }
            }
            return false;
        }

        public void ClearAll(bool close_pcu = true)
        {
            semanticTree = null;
            if (close_pcu)
            {
                PCUReader.CloseUnits();
                PCUWriter.Clear();
            }

            RecompileList.Clear();
            UnitTable.Clear();
            UnitsTopologicallySortedList.Clear();
            //TreeRealization.PCUReturner.Clear();
            BadNodesInSyntaxTree.Clear();
            if (close_pcu)
                PCUReader.AllReaders.Clear();
            project = null;
            StandardModules.Clear();
            CompiledVariables.Clear();
            if (assemblyResolveScope != null)
                assemblyResolveScope.Dispose();
            assemblyResolveScope = null;
            if (!close_pcu)
            {
                SyntaxTreeToSemanticTreeConverter = new TreeConverter.SyntaxTreeToSemanticTreeConverter();
            }
            //SystemLibrary.SystemLibrary.RestoreStandartNames();
        }

        public CompilerType CompilerType
        {
            get
            {
                return CompilerType.Standart;
            }
        }

        public void Free()
        {
        }
    }
}
