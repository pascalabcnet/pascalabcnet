// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
 /***************************************************************************
 *   
 *   Управляющий блок компилятора, алгоритм компиляции модулей
 *   Зависит от Errors,SyntaxTree,SemanticTree,Parsers,TreeConvertor,
 *              CodeGenerators
 * 
 ***************************************************************************/

//TODO разобраться с ToLower()!!!

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
 *   5.СинтаксическийСписокМодулей=ТекущийМодуль.СинтаксическоеДерево.Interface.UsesList;
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
 *     СинтаксическийСписокМодулей=ТекущийМодуль.СинтаксическоеДерево.Implementation.UsesList;
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
//#undef DEBUG

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

using PascalABCCompiler.Errors;
using PascalABCCompiler.PCU;
using PascalABCCompiler.SemanticTreeConverters;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Parser.VB;
using System.CodeDom.Compiler;

//using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting;
using System.Reflection;
//using SyntaxTreeChanger;
using PascalABCCompiler.SyntaxTreeConverters;
using System.Text;
using PascalABCCompiler.TreeRealization;
using System.Linq;

namespace PascalABCCompiler
{
    public class CompilerCompilationError : LocatedError
    {
        public CompilerCompilationError(string message)
            : base(message)
        {
        }
        public CompilerCompilationError(string message, string FileName)
            : base(message, FileName)
        {
        }
        public override string ToString()
        {
            return Message;
        }
    }

    public class ReadPCUError : CompilerCompilationError
    {
        public ReadPCUError(string FileName)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_READ_PCU{0}_ERROR"),FileName))
        {
        }
    }

    public class NamespaceCannotHaveInSection : CompilerCompilationError
    {
        public NamespaceCannotHaveInSection(SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_NAMESPACE_CANNOT_HAVE_IN_SECTION")))
        {
            this.source_context = sc;
        }
    }

    public class ProgramModuleExpected : CompilerCompilationError
    {
        public ProgramModuleExpected(string FileName, SyntaxTree.SourceContext sc)
            : base(StringResources.Get("COMPILATIONERROR_PROGRAM_MODULE_EXPECTED"), FileName)
        {
            this.source_context = sc;
        }
    }

    public class UnitModuleExpected : CompilerCompilationError
    {
        public UnitModuleExpected(string FileName, SyntaxTree.SourceContext sc)
            : base(StringResources.Get("COMPILATIONERROR_UNIT_MODULE_EXPECTED"), FileName)
        {
            this.source_context = sc;
        }
    }

    public class AppTypeDllIsAllowedOnlyForLibraries : CompilerCompilationError
    {
        public AppTypeDllIsAllowedOnlyForLibraries(string FileName, SyntaxTree.SourceContext sc)
            : base(StringResources.Get("COMPILATIONERROR_APPTYPE_DLL_IS_ALLOWED_ONLY_FOR_LIBRARIES"), FileName)
        {
            this.source_context = sc;
        }
    }

    public class UnitModuleExpectedLibraryFound : CompilerCompilationError
    {
        public UnitModuleExpectedLibraryFound(string FileName, SyntaxTree.SourceContext sc)
            : base(StringResources.Get("COMPILATIONERROR_UNIT_MODULE_EXPECTED_LIBRARY_FOUND"), FileName)
        {
            this.source_context = sc;
        }
    }
    
    public class AssemblyNotFound : CompilerCompilationError
    {
        public string AssemblyFileName;
        public AssemblyNotFound(string FileName,string AssemblyFileName, SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_ASSEMBLY_{0}_NOT_FOUND"), AssemblyFileName), FileName)
        {
            this.AssemblyFileName = AssemblyFileName;
            this.source_context = sc;
        }
        
    }

    public class AssemblyReadingError : CompilerCompilationError
    {
    	public string AssemblyFileName;
    	public AssemblyReadingError(string FileName,string AssemblyFileName, SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_ASSEMBLY_{0}_READING_ERROR"), AssemblyFileName), FileName)
        {
            this.AssemblyFileName = AssemblyFileName;
            this.source_context = sc;
        }
    }

    public class InvalidAssemblyPathError : CompilerCompilationError
    {
        public InvalidAssemblyPathError(string FileName,SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_INVALID_ASSEMBLY_PATH")), FileName)
        {
            this.source_context = sc;
        }
    }

    public class ResourceFileNotFound : CompilerCompilationError
    {
        public ResourceFileNotFound(string ResFileName, TreeRealization.location sl)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_RESOURCEFILE_{0}_NOT_FOUND"), ResFileName), sl.doc.file_name)
        {
            this.sourceLocation = new SourceLocation(sl.doc.file_name, sl.begin_line_num, sl.begin_column_num, sl.end_line_num,sl.end_column_num);
        }

        public ResourceFileNotFound(string ResFileName)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_RESOURCEFILE_{0}_NOT_FOUND"), ResFileName), null)
        {
            //this.sourceLocation = new SourceLocation(sl.doc.file_name, sl.begin_line_num, sl.begin_column_num, sl.end_line_num, sl.end_column_num);
        }
    }

    public class IncludeNamespaceInUnit: CompilerCompilationError
    {
        public IncludeNamespaceInUnit(string FileName, SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_INCLUDE_NAMESPACE_IN_UNIT")), FileName)
        {
            this.source_context = sc;
        }
    }

    public class NamespaceModuleExpected : CompilerCompilationError
    {
        public NamespaceModuleExpected(SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_NAMESPACE_MODULE_EXPECTED")))
        {
            this.source_context = sc;
        }
    }

    public class MainResourceNotAllowed : CompilerCompilationError
    {
        public MainResourceNotAllowed(TreeRealization.location sl)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_MAINRESOURCE_NOT_ALLOWED")), sl.doc.file_name)
        {
            this.sourceLocation = new SourceLocation(sl.doc.file_name, sl.begin_line_num, sl.begin_column_num, sl.end_line_num, sl.end_column_num);
        }

    }

    public class DuplicateUsesUnit : CompilerCompilationError
    {
        public string UnitName;
        public DuplicateUsesUnit(string FileName, string UnitName, SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_DUPLICATE_USES_UNIT{0}"), UnitName), FileName)
        {
            this.UnitName = UnitName;
            this.source_context = sc;
        }
    }
    public class DuplicateDirective : CompilerCompilationError
    {
        public string DirectiveName;
        public DuplicateDirective(string FileName, string DirectiveName, SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_DUPLICATE_DIRECTIVE{0}"), DirectiveName), FileName)
        {
            this.DirectiveName = DirectiveName;
            this.source_context = sc;
        }
    }

    public class NamespacesCanBeCompiledOnlyInProjects: CompilerCompilationError
    {
        public NamespacesCanBeCompiledOnlyInProjects(SyntaxTree.SourceContext sc)
            : base(StringResources.Get("COMPILATIONERROR_NAMESPACE_CAN_BE_COMPILED_ONLY_IN_PROJECTS"))
        {
            this.source_context = sc;
        }
    }

    public class UnitNotFound : CompilerCompilationError
    {
        public string UnitName;
        public UnitNotFound(string FileName, string UnitName, SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_UNIT_{0}_NOT_FOUND"), UnitName), FileName)
        {
            this.UnitName = UnitName;
            this.source_context = sc;
        }
    }

    public class UsesInWrongName : CompilerCompilationError
    {
        public string UnitName1;
        public string UnitName2;
        public UsesInWrongName(string FileName, string UnitName1, string UnitName2, SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_USES_IN_WRONG_NAME"), UnitName1, UnitName2), FileName)
        {
            this.UnitName1 = UnitName1;
            this.UnitName2 = UnitName2;
            this.source_context = sc;
        }
    }

    public class SourceFileNotFound : CompilerCompilationError
    {
        public SourceFileNotFound(string FileName)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_SOURCE_FILE_{0}_NOT_FOUND"),FileName))
        {
        }
    }

    public class UnauthorizedAccessToFile : CompilerCompilationError
    {
        public UnauthorizedAccessToFile(string FileName)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_NO_ACCESS_TO_FILE{0}"),FileName))
        {
        }
    }
    public class CycleUnitReference : CompilerCompilationError
    {
        public SyntaxTree.unit_or_namespace SyntaxUsesUnit;
        public CycleUnitReference(string FileName, SyntaxTree.unit_or_namespace SyntaxUsesUnit)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_CYCLIC_UNIT_REFERENCE_WITH_UNIT_{0}"), SyntaxTree.Utils.IdentListToString(SyntaxUsesUnit.name.idents,".")), FileName)
        {
            this.SyntaxUsesUnit = SyntaxUsesUnit;
            this.source_context = SyntaxUsesUnit.source_context;
        }
    }


    public enum UnitState { BeginCompilation, InterfaceCompiled, Compiled }

	public class CompilationUnit
	{
        internal bool CaseSensitive = false;
		public string CurrentUsesUnit;
        public PascalABCCompiler.Errors.SyntaxError syntax_error;
        public string UnitName;
		public List<Errors.Error> ErrorList=new List<Errors.Error>();
		public bool Documented;
        internal List<SyntaxTree.unit_or_namespace> PossibleNamespaces = new List<PascalABCCompiler.SyntaxTree.unit_or_namespace>();
        //internal List<CompilationUnit> AssemblyReferences = new List<CompilationUnit>();

        //private SemanticTree.compilation_unitArrayList _interfaceUsedUnits=new SemanticTree.compilation_unitArrayList();

        public Dictionary<unit_node, CompilationUnit> DirectInterfaceCompilationUnits { get; } = new Dictionary<unit_node, CompilationUnit>();
        public PascalABCCompiler.TreeRealization.unit_node_list InterfaceUsedUnits { get; } = new PascalABCCompiler.TreeRealization.unit_node_list();

        public Dictionary<unit_node, CompilationUnit> DirectImplementationCompilationUnits { get; } = new Dictionary<unit_node, CompilationUnit>();
        public PascalABCCompiler.TreeRealization.unit_node_list ImplementationUsedUnits { get; } = new PascalABCCompiler.TreeRealization.unit_node_list();

        private SyntaxTree.compilation_unit _syntaxTree=null;
		public SyntaxTree.compilation_unit SyntaxTree
		{
			get {return _syntaxTree;}
			set {_syntaxTree=value;}
		}

        private PascalABCCompiler.TreeRealization.unit_node _semanticTree = null;
        public PascalABCCompiler.TreeRealization.unit_node SemanticTree
		{
			get {return _semanticTree;}
			set {_semanticTree=value;}
		}

		private SyntaxTree.unit_or_namespace _syntaxUnitName=null;
		public SyntaxTree.unit_or_namespace SyntaxUnitName
		{
			get {return _syntaxUnitName;}
			set {_syntaxUnitName=value;}
		}

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

		public UnitState State=UnitState.BeginCompilation;
	}
    
	public class CompilationUnitHashTable:Hashtable
	{
        public CompilationUnitHashTable()
            //DarkStar: fixed error in test_standartuses.pas at rebuild
            :base(StringComparer.InvariantCultureIgnoreCase)
        {
        }
		public CompilationUnit this [string key]
		{
			get
			{
                if (key != null)
                    return (base[key] as CompilationUnit);
                return null;
			}
			set
			{
				base[key]=value;
			}
		}
	}
    
    /// <summary>
    /// Опции компиляции
    /// </summary>
    //[Serializable()]
    public class CompilerOptions : MarshalByRefObject
    {
        public enum OutputType {ClassLibrary=0, ConsoleApplicaton=1, WindowsApplication=2, PascalCompiledUnit=3, SemanticTree=4 }

        public bool Debug = false;
        public bool ForDebugging = false;
        public bool Rebuild = false;
        public bool Optimise = false;
        public bool SavePCUInThreadPull = false;
		public bool RunWithEnvironment = false;
        public string CompiledUnitExtension=".pcu";
		public bool ProjectCompiled = false;
		public IProjectInfo CurrentProject = null;
        public OutputType OutputFileType = OutputType.ConsoleApplicaton;
        public bool GenerateCode = true;
        public bool SaveDocumentation = true;
        public bool SavePCU = true;
        public bool IgnoreRtlErrors = true;
        public bool Only32Bit = false;
        public string Locale = "ru";
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
        
        private string outputFileName=null;
        //имя выходного файла без расширения
        //ели имя указано без пути то в качестве пути используетя OutputDirectory
        public string OutputFileName
        {
            get
            {
                string FileName = outputFileName;
                switch (OutputFileType)
                {
                    case OutputType.ConsoleApplicaton:
                    case OutputType.WindowsApplication: FileName = FileName + ".exe"; break;
                    case OutputType.ClassLibrary:       FileName = FileName + ".dll"; break;
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

        //
        public string SystemDirectory;

        public string SearchDirectory;

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
                PascalABCCompiler.NetHelper.NetHelper.UsePABCRtl = value;
            }
        }

        public string[] ParserSearchPatchs;

        public enum StandartModuleAddMethod {LeftToAll, RightToMain};
        [Serializable()]
        public class StandartModule : MarshalByRefObject
        {
            public string Name = null;
            public StandartModuleAddMethod AddMethod = StandartModuleAddMethod.LeftToAll;
            public SyntaxTree.LanguageId AddToLanguages = SyntaxTree.LanguageId.PascalABCNET;
            public StandartModule(string Name, StandartModuleAddMethod AddMethod)
            {
                this.Name = Name;
                this.AddMethod = AddMethod;
            }
            public StandartModule(string Name, StandartModuleAddMethod AddMethod, SyntaxTree.LanguageId AddToLanguages)
            {
                this.Name = Name;
                this.AddToLanguages = AddToLanguages;
                this.AddMethod = AddMethod;
            }
            public StandartModule(string Name)
            {
                this.Name = Name;
            }
            public StandartModule(string Name, SyntaxTree.LanguageId AddToLanguages)
            {
                this.AddToLanguages = AddToLanguages;
                this.Name = Name;
            }
        }
        /// <summary>
        /// Module at index 0 is System module
        /// </summary>
        public List<StandartModule> StandartModules;
        public void RemoveStandartModule(string Name)
        {
            StandartModule module_to_del=null;
            foreach (StandartModule module in StandartModules)
                if (module.Name == Name)
                {
                    module_to_del = module;
                    break;
                }
            if (module_to_del != null)
                StandartModules.Remove(module_to_del);
        }
        public void RemoveStandartModuleAtIndex(int index)
        {
            if(index<StandartModules.Count)
                StandartModules.RemoveAt(index);
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

        public Hashtable StandartDirectories;
        
        private void SetStandartModules()
        {
            StandartModules=new List<StandartModule>();
            StandartModules.Add(new StandartModule("PABCSystem", SyntaxTree.LanguageId.PascalABCNET | SyntaxTree.LanguageId.C));
            StandartModules.Add(new StandartModule("PABCExtensions", SyntaxTree.LanguageId.PascalABCNET | SyntaxTree.LanguageId.C));
        }

        private void SetDirectories()
        {
            SystemDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);
            SearchDirectory = Path.Combine(SystemDirectory,"Lib");
            StandartDirectories = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
            StandartDirectories.Add("%PABCSYSTEM%", SystemDirectory);
            ParserSearchPatchs = new string[1];
            ParserSearchPatchs[0] = SearchDirectory;
        }


        public CompilerOptions()
        {
            SetDirectories();
            SetStandartModules();
        }

        public CompilerOptions(string SourceFileName, OutputType OutputFileType)
        {
            SetDirectories();
            SetStandartModules();
            this.SourceFileName = SourceFileName;
            this.OutputFileType = OutputFileType;
        }
        
    }

    public enum CompilerState
    {
        Ready, CompilationStarting, Reloading, ParserConnected,
        BeginCompileFile, BeginParsingFile, EndParsingFile, CompileInterface, CompileImplementation, EndCompileFile,
        ReadDLL, ReadPCUFile, SavePCUFile, CodeGeneration, CompilationFinished, PCUReadingError, PCUWritingError,
        SemanticTreeConverterConnected, SemanticTreeConversion, SyntaxTreeConversion, SyntaxTreeConverterConnected
    }

    [Serializable()]
    public class CompilerInternalDebug
    {
        public bool CodeGeneration = true;
        public bool SemanticAnalysis = true;
        public bool PCUGenerate = true;
        public bool AddStandartUnits = true;
        public bool SkipPCUErrors = true;
        public bool IncludeDebugInfoInPCU = true;
        public bool AlwaysGenerateXMLDoc=false;
        public bool SkipInternalErrorsIfSyntaxTreeIsCorrupt = true;
        public bool UseStandarParserForIntellisense = true;
        public bool RunOnMono = false;
        public List<string> DocumentedUnits=new List<string>();
        
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
        private string[] extensions;
        public string[] Extensions
        {
            get { return extensions; }
        }
        private string languageName;
        public string LanguageName
        {
            get { return languageName; }
        }
        public SupportedSourceFile(string[] extensions, string lname)
        {
            this.extensions = extensions; languageName = lname;
        }
        public static SupportedSourceFile Make(Parsers.IParser parser)
        {
            List<string> ext = new List<string>();
            foreach (string ex in parser.FilesExtensions)
                if (ex[ex.Length - 1] != Parsers.Controller.HideParserExtensionPostfixChar)
                    ext.Add(ex);
            if (ext.Count > 0)
                return new SupportedSourceFile(ext.ToArray(), parser.Name);
            return null;
        }
        public override string ToString()
        {
            return string.Format("{0} ({1})", LanguageName, FormatTools.ExtensionsToString(Extensions, "*", ";"));
        }
    }

    public delegate void ChangeCompilerStateEventDelegate(ICompiler sender, CompilerState State, string FileName);
        
    public class Compiler : MarshalByRefObject, ICompiler
	{
        //public ISyntaxTreeChanger SyntaxTreeChanger = null; // SSM 17/08/15 - для операций над синтаксическим деревом после его построения
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
                else return RevisionClass.MainVersion + "." + RevisionClass.Build;
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

        private SyntaxTreeConvertersController syntaxTreeConvertersController = null;
        public SyntaxTreeConvertersController SyntaxTreeConvertersController
        {
            get
            {
                return syntaxTreeConvertersController;
            }
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

        TreeRealization.program_node _semantic_tree = null;
        public SemanticTree.IProgramNode SemanticTree
        {
            get
            {
                return _semantic_tree;
            }
        }

        private Dictionary<string, List<TreeRealization.compiler_directive>> compilerDirectives = null;

        //public Hashtable CompilerDirectives
        //{
        //    get { return compilerDirectives; }
        //}

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
            List<SupportedSourceFile> supportedSourceFilesList = new List<SupportedSourceFile>();
            for (int i = 0; i < ParsersController.Parsers.Count; i++)
            {
                SupportedSourceFile sf = SupportedSourceFile.Make(ParsersController.Parsers[i]);
                if (sf != null)
                    supportedSourceFilesList.Add(sf);
            }
            supportedSourceFiles = supportedSourceFilesList.ToArray();
        }
        
        private void SetSupportedProjectFiles()
        {
        	List<SupportedSourceFile> supportedProjectFilesList = new List<SupportedSourceFile>();
            /*for (int i = 0; i < ParsersController.Parsers.Count; i++)
            {
                SupportedSourceFile sf = SupportedSourceFile.Make(ParsersController.Parsers[i]);
                if (sf != null)
                    supportedSourceFilesList.Add(sf);
            }*/
            //vremenno
            supportedProjectFilesList.Add(new SupportedSourceFile(new string[1]{".pabcproj"},"PascalABC.NET"));
            supportedProjectFiles = supportedProjectFilesList.ToArray();
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
        
		private CompilationUnitHashTable unitTable=new CompilationUnitHashTable();
        public CompilationUnitHashTable UnitTable{ get { return unitTable; } }
        public List<CompilationUnit> UnitsSortedList = new List<CompilationUnit>();

        private List<string> StandarModules = new List<string>();

        private CompilerOptions compilerOptions = new CompilerOptions();
        public CompilerOptions CompilerOptions
        {
            get
            {
                return compilerOptions;
            }
            set
            {
                compilerOptions = value;
            }
        }

        internal Dictionary<string,CompilationUnit> DLLCashe = new Dictionary<string,CompilationUnit>();

        private Parsers.Controller parsersController = null;
        public Parsers.Controller ParsersController
        {
            get
            {
                return parsersController;
            }
            set
            {
                parsersController = value;
            }
        }
        public TreeConverter.SyntaxTreeToSemanticTreeConverter SyntaxTreeToSemanticTreeConverter = null;
        public CodeGenerators.Controller CodeGeneratorsController = null;
        //public LLVMConverter.Controller LLVMCodeGeneratorsController = null;
        //public PascalToCppConverter.Controller PABCToCppCodeGeneratorsController = null;
        public SyntaxTree.unit_or_namespace CurrentSyntaxUnit;
		private List<CompilationUnit> UnitsToCompile = new List<CompilationUnit>();
        public Hashtable RecompileList = new Hashtable(StringComparer.OrdinalIgnoreCase);
        private Hashtable CycleUnits = new Hashtable();
        public CompilationUnit CurrentCompilationUnit = null;
        private CompilationUnit FirstCompilationUnit = null;
        private bool PCUReadersAndWritersClosed;
        public int beginOffset;
        public int varBeginOffset;
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

        public int BeginOffset
        {
            get
            {
                return beginOffset;
            }
        }
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
            this.state=State;
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
			get{return errorsList;}
		}

        public Compiler()
        {
            //SyntaxTreeChanger = new SyntaxTreeChanger.SyntaxTreeChange();

            OnChangeCompilerState += ChangeCompilerStateEvent;
            Reload();            
        }
        
        public Compiler(ICompiler comp, SourceFilesProviderDelegate SourceFilesProvider, ChangeCompilerStateEventDelegate ChangeCompilerState)
        {
            //SyntaxTreeChanger = new SyntaxTreeChanger.SyntaxTreeChange(); // SSM 01/05/16 - подключение изменяльщика синтаксического дерева

            this.ParsersController = comp.ParsersController;
        	this.internalDebug = comp.InternalDebug;
        	OnChangeCompilerState += ChangeCompilerStateEvent;
        	if (SourceFilesProvider != null)
                this.sourceFilesProvider = SourceFilesProvider;
            if (ChangeCompilerState != null)
                OnChangeCompilerState += ChangeCompilerState;
            this.supportedSourceFiles = comp.SupportedSourceFiles;
            this.supportedProjectFiles = comp.SupportedProjectFiles;
        }
        
        public Compiler(SourceFilesProviderDelegate SourceFilesProvider, ChangeCompilerStateEventDelegate ChangeCompilerState)
		{
            //SyntaxTreeChanger = new SyntaxTreeChanger.SyntaxTreeChange(); // SSM 01/05/16 - подключение изменяльщика синтаксического дерева
            OnChangeCompilerState += ChangeCompilerStateEvent;
            if (SourceFilesProvider != null)
                this.sourceFilesProvider = SourceFilesProvider;
            if (ChangeCompilerState != null)
                OnChangeCompilerState += ChangeCompilerState;
            Reload();
        }
        
        public void Reload()
        {
            OnChangeCompilerState(this, CompilerState.Reloading, null);

            pABCCodeHealth = 0;

            //А это что?
            TreeRealization.type_node tn = SystemLibrary.SystemLibrary.void_type;

            ClearAll();
            errorsList.Clear();
            Warnings.Clear();
            InternalDebug = new CompilerInternalDebug();
            ParsersController = new Parsers.Controller();
            ParsersController.ParserConnected += new PascalABCCompiler.Parsers.Controller.ParserConnectedDeleagte(ParsersController_ParserConnected);
            ParsersController.SourceFilesProvider = sourceFilesProvider;
            ParsersController.Reload();
            SyntaxTreeToSemanticTreeConverter = new TreeConverter.SyntaxTreeToSemanticTreeConverter();
            CodeGeneratorsController = new CodeGenerators.Controller();
            //PABCToCppCodeGeneratorsController = new PascalToCppConverter.Controller();
            SetSupportedSourceFiles();
            SetSupportedProjectFiles();

            syntaxTreeConvertersController = new SyntaxTreeConvertersController(this);
            syntaxTreeConvertersController.ChangeState += syntaxTreeConvertersController_ChangeState;
            syntaxTreeConvertersController.AddConverters();

            semanticTreeConvertersController = new SemanticTreeConvertersController(this);
            semanticTreeConvertersController.ChangeState += semanticTreeConvertersController_ChangeState;
            semanticTreeConvertersController.AddConverters();

            OnChangeCompilerState(this, CompilerState.Ready, null);    
        }

        void ParsersController_ParserConnected(PascalABCCompiler.Parsers.IParser Parser)
        {
            if (OnChangeCompilerState != null)
                OnChangeCompilerState(this, CompilerState.ParserConnected, Parser.GetType().Assembly.ManifestModule.FullyQualifiedName);
        }

        void syntaxTreeConvertersController_ChangeState(SyntaxTreeConvertersController.State State, ISyntaxTreeConverter SyntaxTreeConverter)
        {
            switch (State)
            {
                case SyntaxTreeConvertersController.State.Convert:
                    OnChangeCompilerState(this, CompilerState.SyntaxTreeConversion, SyntaxTreeConverter.Name);
                    break;
                case SyntaxTreeConvertersController.State.ConnectConverter:
                    OnChangeCompilerState(this, CompilerState.SyntaxTreeConverterConnected, SyntaxTreeConverter.Name);
                    break;
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

        private Dictionary<string, List<TreeRealization.compiler_directive>> GetCompilerDirectives(List<CompilationUnit> Units)
        {
            Dictionary<string, List<TreeRealization.compiler_directive>> Directives = new Dictionary<string, List<TreeRealization.compiler_directive>>(StringComparer.CurrentCultureIgnoreCase);

            for (int i = 0; i < Units.Count; i++)
            {
                TreeRealization.common_unit_node cun = Units[i].SemanticTree as TreeRealization.common_unit_node;
                if (cun != null)
                    foreach (TreeRealization.compiler_directive cd in cun.compiler_directives)
                    {
                        if (!Directives.ContainsKey(cd.name))
                            Directives.Add(cd.name, new List<TreeRealization.compiler_directive>());
                        else if (string.Compare(cd.name, "mainresource", true) == 0)
                            throw new DuplicateDirective(cd.location.doc.file_name, "mainresource", cd.location);
                        Directives[cd.name].Insert(0, cd);
                    }
            }
            return Directives;
        }
        
        private Hashtable GetCompilerDirectives(CompilationUnit Unit)
        {
            Hashtable Directives = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
            TreeRealization.common_unit_node cun = Unit.SemanticTree as TreeRealization.common_unit_node;
            if (cun != null)
                foreach (TreeRealization.compiler_directive cd in cun.compiler_directives)
                    Directives[cd.name] = cd;
            return Directives;
        }
        
        private TreeRealization.location get_location_from_treenode(SyntaxTree.syntax_tree_node tn, string FileName)
        {
            if (tn.source_context == null)
            {
                return null;
            }
            return new TreeRealization.location(tn.source_context.begin_position.line_num, tn.source_context.begin_position.column_num,
                tn.source_context.end_position.line_num, tn.source_context.end_position.column_num, new TreeRealization.document(FileName));
        }
        
        private System.Collections.Generic.List<TreeRealization.compiler_directive> ConvertDirectives(SyntaxTree.compilation_unit cu)
        {
            System.Collections.Generic.List<TreeRealization.compiler_directive> list = new System.Collections.Generic.List<TreeRealization.compiler_directive>();
            foreach (SyntaxTree.compiler_directive sncd in cu.compiler_directives)
            {
            	list.Add(new TreeRealization.compiler_directive(sncd.Name.text, sncd.Directive!=null?sncd.Directive.text:"", get_location_from_treenode(sncd,cu.file_name), cu.file_name));
            }
            return list;
        }

        void syncStartCompile()
        {
            Compile();
        }
        
        public void StartCompile()
        {
            System.Threading.Thread th = new System.Threading.Thread(syncStartCompile);
            th.SetApartmentState(System.Threading.ApartmentState.STA);
            th.Start();
        }

        public string Compile(CompilerOptions CompilerOptions)
        {
            this.CompilerOptions = CompilerOptions;
            return Compile();
        }

        internal void Reset()
        {
            SourceFileNamesDictionary.Clear();
            PCUFileNamesDictionary.Clear();
            GetUnitFileNameCache.Clear();
            Warnings.Clear();
            errorsList.Clear();
            //if (!File.Exists(CompilerOptions.SourceFileName)) throw new SourceFileNotFound(CompilerOptions.SourceFileName);
            CurrentCompilationUnit = null;
            FirstCompilationUnit = null;
            linesCompiled = 0;
            pABCCodeHealth = 0;
            PCUReadersAndWritersClosed = false;
            ParsersController.Reset();
            SyntaxTreeToSemanticTreeConverter.Reset();
            CodeGeneratorsController.Reset();
            //PABCToCppCodeGeneratorsController.Reset();
            UnitsToCompile.Clear();
            DLLCashe.Clear();
            project = null;
        }

        void CheckErrors()
        {
            if (ErrorsList.Count > 0)
                throw ErrorsList[0];
        }

        private void moveSystemUnitToForwardUnitSortedList()
        {
            if(CompilerOptions.StandartModules.Count==0)
                return;
            CompilationUnit system_unit = null;
            foreach (CompilationUnit cu in UnitsSortedList)
                if (cu.SemanticTree != null && cu.SemanticTree is PascalABCCompiler.TreeRealization.common_unit_node)
                    if ((cu.SemanticTree as PascalABCCompiler.TreeRealization.common_unit_node).unit_name == CompilerOptions.StandartModules[0].Name)
                    {
                        system_unit = cu;
                        break;
                    }
            if (system_unit != null && system_unit != UnitsSortedList[0])
            {
                UnitsSortedList.Remove(system_unit);
                UnitsSortedList.Insert(0, system_unit);
            }

        }
		
        private uint get_compiled_lines(string FileName)
        {
        	StreamReader sr = File.OpenText(FileName);
        	uint line = 0;
        	while (!sr.EndOfStream)
        	{
        		sr.ReadLine();
        		line++;
        	}
        	sr.Close();
        	return line;
        }
        
       	class ProgInfo
       	{
       		public string entry_module;
       		public int entry_method_name_pos;
       		public int entry_method_line;
       		public int using_pos=-1;
       		public List<string> modules = new List<string>();
       		public List<string> addit_imports = new List<string>();
       		public List<string> addit_project_files = new List<string>();
       	}
       	
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
       					info.addit_imports.Add(name+"."+name);
       				}
       			}
       			/*if (!string.IsNullOrEmpty(mod_file))
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
       			}*/
       			else
       			{
       				string source_file = FindFileInDirs(s+".vb",Path.GetDirectoryName(CompilerOptions.SourceFileName),Path.Combine(this.CompilerOptions.SystemDirectory,"lib"),
       				                                                 Path.Combine(this.CompilerOptions.SystemDirectory,"LibSource"));
       				if (!string.IsNullOrEmpty(source_file))
       				{
       					if (!info.addit_project_files.Contains(source_file))
       					{
       						info.addit_project_files.Add(source_file);
       					}
       				}
       			}
       		}
       		/*List<string> mods = new List<string>();
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
       		}*/
       		
       	}
       	
       	private int find_pos(string s, int line, int col, bool search_main)
       	{
       		int ind_ln=1;
       		int ind_col=1;
       		int pos = 0;
       		for (int i=0; i<s.Length; i++)
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
       	}
       	
        private ProgInfo get_programm_info(ICSharpCode.NRefactory.Ast.CompilationUnit cu, string source)
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
        						if (string.Compare(meth.Name,"Main",true)==0 && (meth.Parameters == null || meth.Parameters.Count == 0))
        						{
        							//info = new ProgInfo();
        							info.entry_module = (meth.Parent as ICSharpCode.NRefactory.Ast.TypeDeclaration).Name;
        							//info.entry_method_name_line = meth.StartLocation.Line;
        							//info.entry_method_name_col = meth.StartLocation.Column;
        							info.entry_method_name_pos = find_pos(source,meth.StartLocation.Line,meth.StartLocation.Column,true);
        							if (meth.Body.Children.Count > 0)
        							{
        								for (int i=0; i<meth.Body.Children.Count; i++)
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
        			info.using_pos = find_pos(source,using_node.EndLocation.Line,using_node.EndLocation.Column,false);
        		}
        	}
        	if (info.entry_module != null)
        	{
        		add_import_info(info,usings);
        		return info;
        	}
        	return null;
        }

        public string CompileWithProvider(string[] sources, System.CodeDom.Compiler.CodeDomProvider cp, params string [] RefAssemblies)
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
            if (RefAssemblies!=null)
                comp_opt.ReferencedAssemblies.AddRange(RefAssemblies);

            //string source = GetSourceFileText(CompilerOptions.SourceFileName);
            var res = cp.CompileAssemblyFromSource(comp_opt, sources);
            if (res.Errors.Count > 0)
            {
                for (int i = 0; i < res.Errors.Count; i++)
                {
                    if (!res.Errors[i].IsWarning && errorsList.Count == 0 /*&& res.Errors[i].FileName != redirect_fname*/)
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
        }

        class PyErrorHandler : ErrorListener
        {
            Compiler c;
            public PyErrorHandler(Compiler cc) {c=cc;}
            public override void ErrorReported(ScriptSource source, string message, SourceSpan span, int errorCode, Severity severity)
            {
                if (severity==Severity.Warning)
                    c.warnings.Add(new Errors.CommonWarning(message, c.compilerOptions.SourceFileName, span.Start.Line, span.Start.Column));
                else
                    c.errorsList.Add(new Errors.CommonCompilerError(message, c.compilerOptions.SourceFileName, span.Start.Line, span.Start.Column));
            }
        }

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
            //else return res.PathToAssembly;
        }

        public string CompileCS()
        {
            OnChangeCompilerState(this, CompilerState.CompilationStarting, CompilerOptions.SourceFileName);
            OnChangeCompilerState(this, CompilerState.BeginCompileFile, CompilerOptions.SourceFileName);
            Reset();
            var d = new Dictionary<string,string>();
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
                    if (!res.Errors[i].IsWarning && errorsList.Count == 0 /*&& res.Errors[i].FileName != redirect_fname*/)
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

            linesCompiled = get_compiled_lines(CompilerOptions.SourceFileName);

            OnChangeCompilerState(this, CompilerState.CompilationFinished, CompilerOptions.SourceFileName);
            ClearAll();
            OnChangeCompilerState(this, CompilerState.Ready, null);

            if (errorsList.Count > 0)
                return null;
            else
                return res.PathToAssembly;
        }

        public string CompileVB()
        {
        	OnChangeCompilerState(this, CompilerState.CompilationStarting, CompilerOptions.SourceFileName);
        	OnChangeCompilerState(this,CompilerState.BeginCompileFile,CompilerOptions.SourceFileName);
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
			string fname = Path.Combine(Path.GetDirectoryName(CompilerOptions.SourceFileName),"_temp$"+Path.GetFileName(CompilerOptions.SourceFileName));
			File.Copy(CompilerOptions.SourceFileName,fname,true);
			string source = GetSourceFileText(CompilerOptions.SourceFileName);
			
			IParser parser = ICSharpCode.NRefactory.ParserFactory.CreateParser(ICSharpCode.NRefactory.SupportedLanguage.VBNet,new StringReader(source));
			parser.Parse();
			ProgInfo info = get_programm_info(parser.CompilationUnit, source);
			parser.Dispose();
			if (info != null)
			sources.AddRange(info.addit_project_files);
			string redirect_base_fname = FindFileInDirs("__RedirectIOMode.vb",Path.Combine(this.CompilerOptions.SystemDirectory,"Lib"),Path.Combine(this.CompilerOptions.SystemDirectory,"LibSource"));
			string system_unit_name = FindFileInDirs("VBSystem.vb",Path.Combine(this.CompilerOptions.SystemDirectory,"lib"),Path.Combine(this.CompilerOptions.SystemDirectory,"LibSource"));
			string redirect_fname = Path.Combine(Path.GetDirectoryName(CompilerOptions.SourceFileName),"_RedirectIOMode.vb");
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
				for (int i=0; i<info.modules.Count; i++)
				{
					tmp_sb.AppendLine("GetType(Fictive.Fictive).Assembly.GetType(\""+info.modules[i]+"_implementation$."+info.modules[i]+
					              "_implementation$\").GetMethod(\"$Initialization\").Invoke(Nothing,Nothing)");
					
				}
				tmp_sb.AppendLine(info.entry_module+"."+"___Main()");
				for (int i=0; i<info.modules.Count; i++)
				{
					tmp_sb.AppendLine("GetType(Fictive.Fictive).Assembly.GetType(\""+info.modules[i]+"_implementation$."+info.modules[i]+
					              "_implementation$\").GetMethod(\"$Finalization\").Invoke(Nothing,Nothing)");
					
				}
				redirect_module = redirect_module.Replace("%MAIN%",tmp_sb.ToString());
			}
			StreamWriter sw = new StreamWriter(redirect_fname, false);
			if (info != null)
			{
				sw.WriteLine(redirect_module);
			}
			else
			{
				sw.WriteLine("Module _RedirectIOMode");
				sw.WriteLine("End Module");
			}
			sw.Close();
			
			sw = new StreamWriter(CompilerOptions.SourceFileName,false);
			if (info != null && info.entry_method_name_pos != -1)
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				int start_pos = 0;
				if (info.using_pos != -1)
				{
					start_pos = info.using_pos;
					sb.Append(source.Substring(0,info.using_pos));
					for (int i=0; i<info.addit_imports.Count; i++)
					{
						sb.Append(","+info.addit_imports[i]);
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
				sb.Append(source.Substring(start_pos,info.entry_method_name_pos-start_pos));
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
				comp_opt.ReferencedAssemblies.Add(Path.Combine(Path.GetDirectoryName(CompilerOptions.SourceFileName),PascalABCCompiler.TreeConverter.compiler_string_consts.pabc_rtl_dll_name));
				string mod_file_name = FindFileInDirs("PABCRtl.dll",Path.Combine(this.CompilerOptions.SystemDirectory,"Lib"));
				File.Copy(mod_file_name,Path.Combine(Path.GetDirectoryName(CompilerOptions.SourceFileName),"PABCRtl.dll"),true);
				/*foreach (string mod in info.modules)
				{
					comp_opt.ReferencedAssemblies.Add(Path.Combine(Path.GetDirectoryName(CompilerOptions.SourceFileName),mod+".dll"));
					string mod_file_name = FindSourceFileInDirectories(mod+".mod",Path.Combine(this.CompilerOptions.SystemDirectory,"lib"));
					File.Copy(mod_file_name,Path.Combine(Path.GetDirectoryName(CompilerOptions.SourceFileName),mod+".dll"),true);
				}*/
			}
			sources.Add(redirect_fname);
			sources.Add(system_unit_name);
			System.CodeDom.Compiler.CompilerResults res = vbcp.CompileAssemblyFromFile(comp_opt,sources.ToArray());
			if (res.Errors.Count > 0)
            {
            	for (int i=0; i<res.Errors.Count; i++)
            	{
            		if (!res.Errors[i].IsWarning && errorsList.Count == 0 /*&& res.Errors[i].FileName != redirect_fname*/)
            		{
            			if (File.Exists(res.Errors[i].FileName))
            				errorsList.Add(new Errors.CommonCompilerError(res.Errors[i].ErrorText, res.Errors[i].FileName,res.Errors[i].Line!=0?res.Errors[i].Line:1,1));
            			else
            				errorsList.Add(new Errors.CommonCompilerError(res.Errors[i].ErrorText, CompilerOptions.SourceFileName,res.Errors[i].Line!=0?res.Errors[i].Line:1,1));
            		}
            		else if (res.Errors[i].IsWarning)
            		{
            			warnings.Add(new Errors.CommonWarning(res.Errors[i].ErrorText, res.Errors[i].FileName, res.Errors[i].Line,1));
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
			File.Copy(fname,CompilerOptions.SourceFileName,true);
			File.Delete(fname);
			File.Delete(redirect_fname);
            OnChangeCompilerState(this, CompilerState.Ready, null);
            if (errorsList.Count > 0)
            	return null;
            else
				return res.PathToAssembly;
        }
        
        private ProjectInfo project;

        private void PrepareCompileOptionsForProject()
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
        
		public string Compile()
        {
			
			try
            {
                //var timer = System.Diagnostics.Stopwatch.StartNew(); //////

                if (Path.GetExtension(CompilerOptions.SourceFileName) == ".vb")
                {
                    return CompileVB();
                }

                if (Path.GetExtension(CompilerOptions.SourceFileName) == ".cs")
                {
                    return CompileCS();
                }
                
				OnChangeCompilerState(this, CompilerState.CompilationStarting, CompilerOptions.SourceFileName);

                Reset();
                //Console.WriteLine(timer.ElapsedMilliseconds / 1000.0);  //////
                if (CompilerOptions.ProjectCompiled)
                {
                	PrepareCompileOptionsForProject();
                }
                CurrentSyntaxUnit = new SyntaxTree.uses_unit_in(null, new SyntaxTree.string_const(Path.GetFullPath(CompilerOptions.SourceFileName)));
                
                CompileUnit(
                    new PascalABCCompiler.TreeRealization.unit_node_list(),
                    new Dictionary<unit_node, CompilationUnit>(),
                    CurrentSyntaxUnit, null);
                
                //Console.WriteLine(timer.ElapsedMilliseconds / 1000.0);  //////
                foreach (CompilationUnit CurrentUnit in UnitsToCompile)
                    if (CurrentUnit.State != UnitState.Compiled)
                    {
                        CurrentCompilationUnit = CurrentUnit;
                        string UnitName = CurrentCompilationUnit.SyntaxTree.file_name;
                        //if(CurrentUnit.State!=UnitState.InterfaceCompiled)													//DEBUG
                        //Console.WriteLine("ERROR! interface not compiled "+GetUnitFileName(CurrentUnit.SyntaxUnitName));//DEBUG
                        System.Collections.Generic.List<SyntaxTree.unit_or_namespace> SyntaxUsesList = GetSyntaxImplementationUsesList(CurrentUnit.SyntaxTree);
                        CurrentUnit.PossibleNamespaces.Clear();
                        if (HasIncludeNamespacesDirective(CurrentUnit))
                            compilerOptions.UseDllForSystemUnits = false;
                        if (SyntaxUsesList != null)
                        {
                            for (int i = SyntaxUsesList.Count - 1; i >= 0; i--)
                                if (!IsPossibleNamespace(SyntaxUsesList[i], false, Path.GetDirectoryName(UnitName)))
                                {
                                    compilerOptions.UseDllForSystemUnits = false;
                                    break;
                                }
                            for (int i = SyntaxUsesList.Count - 1; i >= 0; i--)
                                if (!IsPossibleNamespace(SyntaxUsesList[i], true, Path.GetDirectoryName(UnitName)))
                                {
                                    CompileUnit(CurrentUnit.ImplementationUsedUnits, CurrentUnit.DirectImplementationCompilationUnits, SyntaxUsesList[i], Path.GetDirectoryName(UnitName));
                                }
                                else
                                {
                                    CurrentUnit.ImplementationUsedUnits.AddElement(new TreeRealization.namespace_unit_node(GetNamespace(SyntaxUsesList[i])), null);
                                    CurrentUnit.PossibleNamespaces.Add(SyntaxUsesList[i]);
                                }
                        }
                        //Console.WriteLine("Compile Implementation "+UnitName);//DEBUG
                        //TODO: Избавиться от преобразования типа.

                        AddNamespaces(CurrentUnit.ImplementationUsingNamespaceList, CurrentUnit.PossibleNamespaces, true, null);

#if DEBUG
                        if (InternalDebug.SemanticAnalysis)
#endif
                        {
                            OnChangeCompilerState(this, CompilerState.CompileImplementation, UnitName);
                            PascalABCCompiler.TreeConverter.SemanticRules.SymbolTableCaseSensitive = CurrentUnit.CaseSensitive;
                            SyntaxTreeToSemanticTreeConverter.CompileImplementation(
                                (PascalABCCompiler.TreeRealization.common_unit_node)CurrentUnit.SemanticTree,
                                CurrentUnit.SyntaxTree,
                                buildImplementationUsesList(CurrentUnit),
                                ErrorsList,Warnings,
                                CurrentUnit.syntax_error, BadNodesInSyntaxTree,
                                CurrentUnit.InterfaceUsingNamespaceList,
                                CurrentUnit.ImplementationUsingNamespaceList,
                                null,
                                CompilerOptions.Debug,
                                CompilerOptions.ForDebugging
                                );
                            CheckErrors();
                        }
                        CurrentUnit.State = UnitState.Compiled;
                        OnChangeCompilerState(this, CompilerState.EndCompileFile, UnitName);
                        //SavePCU(CurrentUnit, UnitName);
                        CurrentUnit.UnitName = UnitName;
                    }

                ClosePCUReadersAndWriters();
                if (CompilerOptions.SaveDocumentation)
                SaveDocumentations();
                
                compilerDirectives = GetCompilerDirectives(UnitsSortedList);

                if (compilerDirectives.ContainsKey(TreeConverter.compiler_string_consts.compiler_directive_apptype))
                {
                    string directive = compilerDirectives[TreeConverter.compiler_string_consts.compiler_directive_apptype][0].directive;
                    if (string.Compare(directive,"console",true)==0)
                        CompilerOptions.OutputFileType = CompilerOptions.OutputType.ConsoleApplicaton;
                    else
                        if (string.Compare(directive,"windows",true)==0)
                            CompilerOptions.OutputFileType = CompilerOptions.OutputType.WindowsApplication;
                        else
                            if (string.Compare(directive,"dll",true)==0)
                                CompilerOptions.OutputFileType = CompilerOptions.OutputType.ClassLibrary;
                            else
                                if (string.Compare(directive,"pcu",true)==0)
                                    CompilerOptions.OutputFileType = CompilerOptions.OutputType.PascalCompiledUnit;
                }

                moveSystemUnitToForwardUnitSortedList();
                PascalABCCompiler.TreeRealization.common_unit_node system_unit = null;
                if (UnitsSortedList.Count>0) 
                    system_unit = UnitsSortedList[0].SemanticTree as PascalABCCompiler.TreeRealization.common_unit_node;
                if (system_unit != null)
                    system_unit.IsConsoleApplicationVariable = CompilerOptions.OutputFileType == CompilerOptions.OutputType.ConsoleApplicaton;

                TreeRealization.program_node pn = null;
                NETGenerator.CompilerOptions cdo = new NETGenerator.CompilerOptions();
                List<TreeRealization.compiler_directive> cds;
                if (compilerDirectives.TryGetValue(TreeConverter.compiler_string_consts.compiler_directive_platformtarget, out cds))
                {
                    string plt = cds[0].directive.ToLower();
                    if (plt.Equals("x86"))
                        cdo.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.x86;
                    else if (plt.Equals("x64"))
                        cdo.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.x64;
                    else if (plt.Equals("anycpu"))
                        cdo.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.AnyCPU;
                }
                if (this.compilerOptions.Only32Bit)
                    cdo.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.x86;
                if (compilerDirectives.TryGetValue(TreeConverter.compiler_string_consts.product_string, out cds))
                {
                    cdo.Product = cds[0].directive;
                }
                if (compilerDirectives.TryGetValue(TreeConverter.compiler_string_consts.version_string, out cds))
                {
                    cdo.ProductVersion = cds[0].directive;
                }
                if (compilerDirectives.TryGetValue(TreeConverter.compiler_string_consts.company_string, out cds))
                {
                    cdo.Company = cds[0].directive;
                }
                if (compilerDirectives.TryGetValue(TreeConverter.compiler_string_consts.trademark_string, out cds))
                {
                    cdo.TradeMark = cds[0].directive;
                }
                if (compilerDirectives.TryGetValue(TreeConverter.compiler_string_consts.copyright_string, out cds))
                {
                    cdo.Copyright = cds[0].directive;
                }
                if (compilerDirectives.TryGetValue(TreeConverter.compiler_string_consts.title_string, out cds))
                {
                    cdo.Title = cds[0].directive;
                }
                if (compilerDirectives.TryGetValue(TreeConverter.compiler_string_consts.description_string, out cds))
                {
                    cdo.Description = cds[0].directive;
                }
                if (compilerDirectives.TryGetValue(TreeConverter.compiler_string_consts.main_resource_string, out cds))
                {
                    if (compilerDirectives.ContainsKey(TreeConverter.compiler_string_consts.product_string) ||
                        compilerDirectives.ContainsKey(TreeConverter.compiler_string_consts.version_string) ||
                        compilerDirectives.ContainsKey(TreeConverter.compiler_string_consts.company_string) ||
                        compilerDirectives.ContainsKey(TreeConverter.compiler_string_consts.trademark_string) ||
                        compilerDirectives.ContainsKey(TreeConverter.compiler_string_consts.title_string) ||
                        compilerDirectives.ContainsKey(TreeConverter.compiler_string_consts.description_string))
                    {
                        ErrorsList.Add(new MainResourceNotAllowed(cds[0].location));
                    }
                    cdo.MainResourceFileName = cds[0].directive;
                    if (!File.Exists(cdo.MainResourceFileName))
                    {
                        ErrorsList.Add(new ResourceFileNotFound(cds[0].directive, cds[0].location));
                    }
                }

                List<string> ResourceFiles = null;
                if (compilerDirectives.ContainsKey(TreeConverter.compiler_string_consts.compiler_directive_resource))
                {
                    ResourceFiles = new List<string>();
                    List<TreeRealization.compiler_directive> ResourceDirectives = compilerDirectives[TreeConverter.compiler_string_consts.compiler_directive_resource];
                    foreach (TreeRealization.compiler_directive cd in ResourceDirectives)
                    {
                        var resource_fname = Path.Combine(Path.GetDirectoryName(cd.source_file), cd.directive);
                        
                        if (File.Exists(resource_fname))
                            ResourceFiles.Add(resource_fname);
                        else
                            ErrorsList.Add(new ResourceFileNotFound(cd.directive, cd.location));

                    }    
                }
                string res_file = null;
                if (project != null)
                {
                    if (!(project.major_version == 0 && project.minor_version == 0 && project.build_version == 0 && project.revision_version == 0))
                        cdo.ProductVersion = project.major_version + "." + project.minor_version + "." + project.build_version + "." + project.revision_version;
                    if (!string.IsNullOrEmpty(project.product))
                        cdo.Product = project.product;
                    if (!string.IsNullOrEmpty(project.company))
                        cdo.Company = project.company;
                    if (!string.IsNullOrEmpty(project.trademark))
                        cdo.TradeMark = project.trademark;
                    if (!string.IsNullOrEmpty(project.copyright))
                        cdo.Copyright = project.copyright;
                    if (!string.IsNullOrEmpty(project.title))
                        cdo.Title = project.title;
                    if (!string.IsNullOrEmpty(project.description))
                        cdo.Description = project.description;
                    if (!string.IsNullOrEmpty(project.app_icon) && false)
                    {
                        //cdo.MainResourceFileName = project.app_icon;
                        string rc_file = Path.GetFileNameWithoutExtension(project.app_icon) + ".rc";
                        StreamWriter sw = File.CreateText(rc_file);
                        sw.WriteLine("1 ICON \"" + project.app_icon.Replace("\\", "\\\\") + "\"");
                        if (cdo.NeedDefineVersionInfo)
                        {
                            cdo.NeedDefineVersionInfo = false;
                            sw.WriteLine("1 VERSIONINFO");
                            string ver = project.major_version + "," + project.minor_version + "," + project.build_version + "," + project.revision_version;
                            sw.WriteLine("FILEVERSION " + ver);
                            /*sw.WriteLine("FILEFLAGSMASK VS_FFI_FILEFLAGSMASK");
                            sw.WriteLine("FILEFLAGS VER_DEBUG");
                            sw.WriteLine("FILEOS VOS__WINDOWS32");
                            if (project.project_type != ProjectType.Library)
                                sw.WriteLine("FILETYPE VFT_APP");
                            else
                                sw.WriteLine("FILETYPE VFT_DLL");
                            sw.WriteLine("FILESUBTYPE VFT2_UNKNOWN");*/
                            sw.WriteLine("BEGIN \r\n BLOCK \"StringFileInfo\"\r\n BEGIN \r\n BLOCK \"041904E3\"\r\nBEGIN");
                            sw.WriteLine("VALUE \"ProductName\"," + "\"" + cdo.Product + "\"");
                            sw.WriteLine("VALUE \"FileVersion\"," + "\"" + ver + "\"");
                            sw.WriteLine("VALUE \"ProductVersion\"," + "\"" + ver + "\"");
                            sw.WriteLine("VALUE \"FileDescription\"," + "\"" + cdo.Description + "\"");
                            sw.WriteLine("VALUE \"OriginalFileName\"," + "\"" + Path.GetFileName(CompilerOptions.OutputFileName) + "\"");
                            sw.WriteLine("VALUE \"InternalName\"," + "\"" + Path.GetFileNameWithoutExtension(CompilerOptions.OutputFileName) + "\"");
                            sw.WriteLine("VALUE \"CompanyName\"," + "\"" + cdo.Company + "\"");
                            sw.WriteLine("VALUE \"LegalTrademarks1\"," + "\"" + cdo.TradeMark + "\"");
                            sw.WriteLine("VALUE \"LegalCopyright\"," + "\"" + cdo.Copyright + "\"");
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
                        res_file = Path.Combine(Path.GetDirectoryName(project.app_icon), Path.GetFileNameWithoutExtension(project.app_icon) + ".res");
                        if (File.Exists(res_file))
                        {
                            cdo.MainResourceFileName = res_file;
                        }
                        File.Delete(rc_file);
                    }
                }

                if (ErrorsList.Count == 0 && compilerOptions.GenerateCode)
                {
                    cdo.ForRunningWithEnvironment = CompilerOptions.RunWithEnvironment;
                	switch (CompilerOptions.OutputFileType)
                    {
                        case CompilerOptions.OutputType.ClassLibrary: cdo.target = NETGenerator.TargetType.Dll; break;
                        case CompilerOptions.OutputType.ConsoleApplicaton: cdo.target = NETGenerator.TargetType.Exe; break;
                        case CompilerOptions.OutputType.WindowsApplication: cdo.target = NETGenerator.TargetType.WinExe; break;

                    }
                    if (project != null && project.ProjectType == ProjectType.WindowsApp)
                        cdo.target = PascalABCCompiler.NETGenerator.TargetType.WinExe;
                    switch (CompilerOptions.Debug)
                    {
                        case true: cdo.dbg_attrs = NETGenerator.DebugAttributes.Debug; break;
                        case false: cdo.dbg_attrs = NETGenerator.DebugAttributes.Release; break;
                    }
                    if (CompilerOptions.ForDebugging)
                        cdo.dbg_attrs = NETGenerator.DebugAttributes.ForDebugging;


                    //TODO: Разобратся c location для program_node и правильно передавать main_function. Добавить генератор main_function в SyntaxTreeToSemanticTreeConverter.
                    pn = new PascalABCCompiler.TreeRealization.program_node(null, null);
                    
                    for (int i = 0; i < UnitsSortedList.Count; i++)
                        pn.units.AddElement(UnitsSortedList[i].SemanticTree as TreeRealization.common_unit_node);

                    //(ssyy) Добавил в условие c_module
                    if (FirstCompilationUnit.SyntaxTree is SyntaxTree.program_module ||
                        FirstCompilationUnit.SyntaxTree is SyntaxTree.c_module)
                    {
                        if ((cdo.target == NETGenerator.TargetType.Exe) || (cdo.target == NETGenerator.TargetType.WinExe))
                        {
                            if (UnitsSortedList.Count > 0)
                            {
                                pn.main_function = ((PascalABCCompiler.TreeRealization.common_unit_node)UnitsSortedList[UnitsSortedList.Count - 1].SemanticTree).main_function;
                                /***************************Ivan added*******************************/
                                if (pn.main_function.function_code.location != null)
                                {
                                    bool flag = false;
                                    PascalABCCompiler.TreeRealization.common_namespace_node main_ns = pn.main_function.namespace_node;
                                    for (int i = 0; i < main_ns.variables.Count; i++)
                                    {
                                        PascalABCCompiler.TreeRealization.namespace_variable nv = main_ns.variables[i];
                                        if (nv.inital_value != null && nv.inital_value.location != null && !(nv.inital_value is PascalABCCompiler.TreeRealization.constant_node)
                                            && !(nv.inital_value is PascalABCCompiler.TreeRealization.record_initializer) && !(nv.inital_value is PascalABCCompiler.TreeRealization.array_initializer))
                                        {
                                            varBeginOffset = main_ns.variables[i].inital_value.location.begin_line_num;
                                            flag = true;
                                            break;
                                        }
                                    }
                                    beginOffset = pn.main_function.function_code.location.begin_line_num;
                                }
                                /*******************************************************************/
                                Dictionary<string, object> config_dic = new Dictionary<string, object>();
                                if (CompilerOptions.Locale != null && PascalABCCompiler.StringResourcesLanguage.GetLCIDByTwoLetterISO(CompilerOptions.Locale) != null)
                                { 
                                    config_dic["locale"] = CompilerOptions.Locale;
                                    config_dic["full_locale"] = PascalABCCompiler.StringResourcesLanguage.GetLCIDByTwoLetterISO(CompilerOptions.Locale);
                                }
                                pn.create_main_function(StandarModules.ToArray(), config_dic);
                                
                            }
                        }
                    }
                    else if (FirstCompilationUnit.SyntaxTree is SyntaxTree.unit_module && cdo.target == NETGenerator.TargetType.Dll)
                    {
                    	pn.create_main_function_as_in_module();
                    }
                    pn = semanticTreeConvertersController.Convert(pn) as TreeRealization.program_node;

                    _semantic_tree = pn;

                    if (FirstCompilationUnit.SyntaxTree is SyntaxTree.unit_module && CompilerOptions.OutputFileType != CompilerOptions.OutputType.ClassLibrary)
                    {
                        //если мы комилируем PCU
                        CompilerOptions.OutputFileType = CompilerOptions.OutputType.PascalCompiledUnit;
                    }
                    else
                    {
                        if( CompilerOptions.OutputFileType!= CompilerOptions.OutputType.SemanticTree)
#if DEBUG
                        if (InternalDebug.CodeGeneration)
#endif
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
                                string pdb_file_name=Path.ChangeExtension(CompilerOptions.OutputFileName, ".pdb");
                                if (File.Exists(pdb_file_name))
                                    File.Delete(pdb_file_name);
                                    n = 5;
                            }
                            catch (Exception e)
                            {
                                    throw new UnauthorizedAccessToFile(CompilerOptions.OutputFileName + " -- " + n + "  " + e.ToString());
                                    //throw e;
                            }
                            OnChangeCompilerState(this, CompilerState.CodeGeneration, CompilerOptions.OutputFileName);
                            string[] ResourceFilesArray = null;
                            if (ResourceFiles != null)
                                ResourceFilesArray = ResourceFiles.ToArray();
                            cds = null;
                            /*if (compilerDirectives.TryGetValue("platform", out cds) && cds[0].directive.ToLower() == "native")
                            {
                                //LLVMCodeGeneratorsController.Compile(pn, CompilerOptions.OutputFileName, CompilerOptions.SourceFileName, ResourceFilesArray);
                                PABCToCppCodeGeneratorsController.Compile(pn, CompilerOptions.OutputFileName, CompilerOptions.SourceFileName, ResourceFilesArray);
                            }
                            else*/
                            if (compilerOptions.UseDllForSystemUnits)
                                cdo.RtlPABCSystemType = NetHelper.NetHelper.FindRtlType("PABCSystem.PABCSystem");
                            CodeGeneratorsController.Compile(pn, CompilerOptions.OutputFileName, CompilerOptions.SourceFileName, cdo, CompilerOptions.StandartDirectories, ResourceFilesArray);
                            if (res_file != null)
                                File.Delete(res_file);
                        }
                    }
                }
            }
            catch (TreeConverter.ParserError err)
            {
                //конвертор уткнулся в ошибку. ничего не делаем
            }
            catch (Errors.CompilerInternalError err)
            {
                if (ErrorsList.Count == 0)
                    ErrorsList.Add(err);
                else
                {
#if DEBUG
                    if (!InternalDebug.SkipInternalErrorsIfSyntaxTreeIsCorrupt)
                        ErrorsList.Add(err);
#endif
                }
            }
            catch (Errors.Error err)
            {
                if (ErrorsList.Count == 0)
                    ErrorsList.Add(err);
                else
                    if (err != ErrorsList[0])
                    {
                        if (err is SemanticError)
                        {
                            int pos = ErrorsList.Count;
                            SourceLocation loc = (err as SemanticError).SourceLocation, loctmp;
                            if (loc != null)
                                for (int i = 0; i < ErrorsList.Count; i++)
                                    if (ErrorsList[i] is LocatedError)
                                        if ((loctmp = (ErrorsList[i] as LocatedError).SourceLocation) != null)
                                            if (loctmp > loc)
                                            {
                                                pos = i;
                                                break;
                                            }

                            ErrorsList.Insert(pos, err);
                        }
                        else
                            ErrorsList.Add(err);
                    }
            }
            catch (Exception err)
            {
            	string fn = "Compiler";
                if (CurrentCompilationUnit != null && this.CurrentCompilationUnit.SyntaxTree != null) fn = Path.GetFileName(this.CurrentCompilationUnit.SyntaxTree.file_name);
                Errors.CompilerInternalError comp_err = new Errors.CompilerInternalError(string.Format("Compiler.Compile[{0}]", fn), err);
                if (ErrorsList.Count == 0)
                    ErrorsList.Add(comp_err);
                else
                {
#if DEBUG
                    if (!InternalDebug.SkipInternalErrorsIfSyntaxTreeIsCorrupt)
                        ErrorsList.Add(comp_err);
#endif
                }
            }
            
            //удаляем лишние ошибки
            /*foreach(Error er in errorsList)
            {

            }*/

            //на случай если мы вывалились по исключению но у нас есть откомпилированные модули
            try
            {
                ClosePCUReadersAndWriters();
            }
            catch (Exception e)
            {
                ErrorsList.Add(new Errors.CompilerInternalError("Compiler.ClosePCUReadersAndWriters", e));
            }
            bool need_recompiled = false;
            if (ErrorsList.Count > 0)
            {
                if (compilerOptions.UseDllForSystemUnits && !has_only_syntax_errors(ErrorsList) && compilerOptions.IgnoreRtlErrors)
                {
                    compilerOptions.UseDllForSystemUnits = false;
                    ErrorsList.Clear();
                    
                    need_recompiled = true;

                }
            }
            OnChangeCompilerState(this, CompilerState.CompilationFinished, CompilerOptions.SourceFileName);
            if (ClearAfterCompilation)
            ClearAll();
            
            
            OnChangeCompilerState(this, CompilerState.Ready, null);
            if (ErrorsList.Count > 0)
            {
                return null;
            }
            else if (need_recompiled)
            {
                //Compiler c = new Compiler(sourceFilesProvider,OnChangeCompilerState);
                //return c.Compile(this.compilerOptions);
                return Compile();
            }
            else
                return CompilerOptions.OutputFileName;
        }

        private bool has_only_syntax_errors(List<Error> errors)
        {
            foreach (Error err in errors)
            {
                if (!(err is SyntaxError))
                    return false;
            }
            return true;
        }

		private void SaveDocumentations()
		{
			DocXmlManager dxm = new DocXmlManager();
			foreach (CompilationUnit cu in UnitsSortedList)
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
            	}
            	bool rest = true;
            	for (int i=0; i<PCUReader.AllReaders.Count; i++)
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
        
        void WaitCallback_ClosePCUWriters(object state)
        {
            ClosePCUWriters();
        }
        
        private void AsyncClosePCUWriters()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(WaitCallback_ClosePCUWriters);
        }
        
        private void ClosePCUWriters()
        {
            foreach (CompilationUnit cu in UnitsSortedList)
            {
                SavePCU(cu);
            }
            foreach (PCUWriter pw in PCUWriter.AllWriters)
            {
                pw.CloseWriter();
            }
        }
		
        void checkDuplicateUsesUnit(List<SyntaxTree.unit_or_namespace> usesList)
        {
            if(usesList==null)
                return;
            List<string> names = new List<string>();
            foreach (SyntaxTree.unit_or_namespace un in usesList)
            {
                string name=SyntaxTree.Utils.IdentListToString(un.name.idents,".").ToLower();
                if (un.source_context != null)
                {
                    if (names.Contains(name))
                        throw new DuplicateUsesUnit(CurrentCompilationUnit.SyntaxTree.file_name, name, un.source_context);
                    else
                        names.Add(name);
                }
            }
        }
		
        public List<SyntaxTree.unit_or_namespace> GetSyntaxInterfaceUsesList(SyntaxTree.compilation_unit CurrentSyntaxUnit)
		{
            List<SyntaxTree.unit_or_namespace> result = null;
			if(CurrentSyntaxUnit is SyntaxTree.unit_module)
            {
                SyntaxTree.interface_node intp = (CurrentSyntaxUnit as SyntaxTree.unit_module).interface_part;
                if (intp.uses_modules == null)
                    if (CompilerOptions.StandartModules.Count > 0)//если есть стандартые модули то создать список
                    {
                        intp.uses_modules = new SyntaxTree.uses_list();
                        intp.uses_modules.source_context = new SyntaxTree.SourceContext(1, 1, 1, 1, 1, 1);
                    }
                    else
                        return null;
                result = intp.uses_modules.units;
            }
            if (CurrentSyntaxUnit is SyntaxTree.program_module)
            {
                SyntaxTree.program_module pm = (CurrentSyntaxUnit as SyntaxTree.program_module);
                if (pm.used_units == null)
                    if (CompilerOptions.StandartModules.Count > 0)//если есть стандартые модули то создать список
                    {
                        pm.used_units = new SyntaxTree.uses_list();
                        pm.used_units.source_context = new SyntaxTree.SourceContext(1, 1, 1, 1, 1, 1);
                    }
                    else
                        return null;
                result = pm.used_units.units;
            }
            if (CurrentSyntaxUnit is SyntaxTree.c_module)
            {
                SyntaxTree.c_module pm = (CurrentSyntaxUnit as SyntaxTree.c_module);
                if (pm.used_units == null)
                    if (CompilerOptions.StandartModules.Count > 0)//если есть стандартые модули то создать список
                    {
                        pm.used_units = new SyntaxTree.uses_list();
                        pm.used_units.source_context = new SyntaxTree.SourceContext(1, 1, 1, 1, 1, 1);
                    }
                    else
                        return null;
                result = pm.used_units.units;
            }
            checkDuplicateUsesUnit(result);
            return result;
		}

        private List<SyntaxTree.unit_or_namespace> GetSyntaxImplementationUsesList(SyntaxTree.compilation_unit CurrentSyntaxUnit)
		{
            List<SyntaxTree.unit_or_namespace> result = null;
			if(CurrentSyntaxUnit is SyntaxTree.unit_module)
				if ((CurrentSyntaxUnit as SyntaxTree.unit_module).implementation_part!=null)
                    if ((CurrentSyntaxUnit as SyntaxTree.unit_module).implementation_part.uses_modules != null)
                    {
                        result = (CurrentSyntaxUnit as SyntaxTree.unit_module).implementation_part.uses_modules.units;
                        checkDuplicateUsesUnit(result);
                        return result;
                    }
			if(CurrentSyntaxUnit is SyntaxTree.program_module)
				return null;
			return null;
		}
        
        public string FindPCUFileName(string fname, string curr_path, out int folder_priority)
        {
            if (string.IsNullOrEmpty(Path.GetExtension(fname)))
                fname += CompilerOptions.CompiledUnitExtension;
            var cache_key = Tuple.Create(fname.ToLower(), curr_path?.ToLower());

            if (!PCUFileNamesDictionary.TryGetValue(cache_key, out var res))
            {

                if (FindFileInDirs(fname, curr_path) is string res_s1)
                    res = Tuple.Create(res_s1, 1);
                else if (FindFileInDirs(Path.GetFileName(fname), CompilerOptions.OutputDirectory) is string res_s2)
                    res = Tuple.Create(res_s2, 2);
                else if (FindFileInDirs(fname, CompilerOptions.SearchDirectory) is string res_s3)
                    res = Tuple.Create(res_s3, 3);
                else
                    res = null;

                PCUFileNamesDictionary[cache_key] = res;
            }

            folder_priority = res == null ? 0 : res.Item2;
            return res?.Item1;
        }

        public string FindSourceFileName(string fname, string curr_path, out int folder_priority)
        {
            var cache_key = Tuple.Create(fname.ToLower(), curr_path?.ToLower());

            if (!SourceFileNamesDictionary.TryGetValue(cache_key, out var res))
            {

                if (FindSourceFileNameInDirs(fname, curr_path) is string res_s1)
                    res = Tuple.Create(res_s1, 1);
                else if (FindSourceFileNameInDirs(fname, CompilerOptions.SearchDirectory) is string res_s3)
                    res = Tuple.Create(res_s3, 3);
                else
                    res = null;

                SourceFileNamesDictionary[cache_key] = res;
            }

            folder_priority = res == null ? 0 : res.Item2;
            return res?.Item1;
        }
        
        public string FindSourceFileNameInDirs(string fname, params string[] Dirs)
        {
            var fname_ext = Path.GetExtension(fname);
            var need_ext = string.IsNullOrEmpty(fname_ext);

            foreach (SupportedSourceFile sf in SupportedSourceFiles)
                foreach (string ext in sf.Extensions)
                    if (need_ext || fname_ext==ext)
                    {
                        var res = FindFileInDirs(need_ext ? fname+ext : fname, Dirs);
                        //if (!(CompilerOptions.UseDllForSystemUnits && Path.GetDirectoryName(res) == CompilerOptions.SearchDirectory))
                        if (res != null)
                            return res;
                    }
            
            return null;
        }

        public static string CombinePath(string dir, string path)
        {
            if (Path.IsPathRooted(path)) return path;
            int i = 0;

            for (; dir!="" && i < path.Length && path[i] == '.' && path[i + 1] == '.'; )
            {
                dir = Path.GetDirectoryName(dir);
                if (string.IsNullOrEmpty(dir)) return null;
                i += 2;
                if (path[i] == Path.DirectorySeparatorChar || path[i] == Path.AltDirectorySeparatorChar) i += 1;
            }

            return Path.Combine(dir, path.Substring(i));
        }

        public static string GetUnitPath(CompilationUnit u1, CompilationUnit u2)
        {
            if (u1 == null) throw new ArgumentException(nameof(u1));
            if (u2 == null) throw new ArgumentException(nameof(u2));

            if (u1.DirectInterfaceCompilationUnits      .Values.Contains(u2)) return u1.InterfaceUsedUnits      .unit_uses_paths[u2.SemanticTree];
            if (u1.DirectImplementationCompilationUnits .Values.Contains(u2)) return u1.ImplementationUsedUnits .unit_uses_paths[u2.SemanticTree];

            var last = new Dictionary<CompilationUnit, string>();
            var done = new HashSet<CompilationUnit>();

            foreach (var u in u1.DirectInterfaceCompilationUnits.Values.Concat(u1.DirectImplementationCompilationUnits.Values))
            {
                last.Add(u, u1.InterfaceUsedUnits.unit_uses_paths[u.SemanticTree]);
                done.Add(u);
            }

            while (last.Count != 0)
            {
                var next = new Dictionary<CompilationUnit, string>();

                foreach (var u in last.Keys)
                {
                    foreach (var used_u in u.DirectInterfaceCompilationUnits.Values)
                    {
                        if (!done.Add(used_u)) continue;
                        var path = CombinePath(Path.GetDirectoryName(last[u]), u.InterfaceUsedUnits.unit_uses_paths[used_u.SemanticTree]);
                        if (used_u == u2) return path;
                        next.Add(used_u, path);
                    }

                    foreach (var used_u in u.DirectImplementationCompilationUnits.Values)
                    {
                        if (!done.Add(used_u)) continue;
                        var path = CombinePath(Path.GetDirectoryName(last[u]), u.ImplementationUsedUnits.unit_uses_paths[used_u.SemanticTree]);
                        if (used_u == u2) return path;
                        next.Add(used_u, path);
                    }
                }

                last = next;
            }

            throw new InvalidOperationException($"Could not find path to \"{u2.UnitName}\" relative to \"{u1.UnitName}\"");
        }

        private string FindFileInDirs(string FileName, params string[] Dirs)
        {
            foreach (string Dir in Dirs)
            {
                var res = CombinePath(Dir, FileName);
                if (File.Exists(res))
                    return res;
            }
            return null;
        }

        public static string GetReferenceFileName(string FileName)
        {
            // Вначале - кешированные стандартные dll
            if (standart_assembly_dict.ContainsKey(FileName))
                return standart_assembly_dict[FileName];

            if (System.IO.File.Exists(FileName))
            {
                return FileName;//.ToLower();//? а надо ли tolover?
            }
            else
            {
            	return get_assembly_path(FileName,false);
            }
        }
        
        private string GetReferenceFileName(string FileName, SyntaxTree.SourceContext sc, string curr_path)
        {
            if (standart_assembly_dict.ContainsKey(FileName))
                return standart_assembly_dict[FileName];

            // Наверное, этот код MikhailoMMX лишний
            //MikhailoMMX PABCRtl.dll будем искать сначала в GAC, а потом в папке с программой
            if (FileName == TreeConverter.compiler_string_consts.pabc_rtl_dll_name)
            {

                string name = get_assembly_path(FileName, true);
                if ((name != null) && (File.Exists(name)))
                    return name;

            }
            //\MikhailoMMX

            var FullFileName = Path.Combine(curr_path, FileName);
            if (System.IO.File.Exists(FullFileName)) // для отладки с *.inc файлами
            {
                var NewFileName = Path.Combine(compilerOptions.OutputDirectory, Path.GetFileName(FullFileName));
                if (FullFileName != NewFileName) File.Copy(FullFileName, NewFileName, true);
                return NewFileName;
            }
            else
            {
                string name = get_assembly_path(FileName,false);//? а надо ли tolover?
                if (name == null)
                    throw new AssemblyNotFound(this.CurrentCompilationUnit.SyntaxTree.file_name, FileName, sc);
                else
                    if (File.Exists(name))
                        return name;
                    else
                        throw new AssemblyNotFound(this.CurrentCompilationUnit.SyntaxTree.file_name, FileName, sc);
            }
        }
        
        public string GetUnitFileName(SyntaxTree.unit_or_namespace SyntaxUsesUnit, string curr_path)
		{
            //ToDo В корневом Compile() создаётся uses_unit_in без name. Выглядит как костыль
            if (SyntaxUsesUnit is SyntaxTree.uses_unit_in && (SyntaxUsesUnit as SyntaxTree.uses_unit_in).name == null) return (SyntaxUsesUnit as SyntaxTree.uses_unit_in).in_file.Value;
            var UnitName = SyntaxUsesUnit.name.idents[0].name;

            if (SyntaxUsesUnit is SyntaxTree.uses_unit_in uui)
            {
                
                // Подключение .dll в uses-in. Зачем - не понятно, но было до меня, поэтому просто оставил
                switch (Path.GetExtension(uui.in_file.Value).ToLower())
                {
                    case ".dll":
                    case ".exe":
                        return GetReferenceFileName(uui.in_file.Value, uui.in_file.source_context, curr_path);
                }

                if (UnitName.ToLower() != Path.GetFileNameWithoutExtension(uui.in_file.Value).ToLower())
                    throw new UsesInWrongName(CurrentCompilationUnit.SyntaxTree.file_name, UnitName, Path.GetFileNameWithoutExtension(uui.in_file.Value), uui.in_file.source_context);

            }

            return GetUnitFileName(UnitName, SyntaxUsesUnit.UsesPath(), curr_path, SyntaxUsesUnit.source_context);
        }
        
        public string GetUnitFileName(string UnitName, string path, string curr_path, SyntaxTree.SourceContext source_context)
        {
            var cache_key = Tuple.Create(path.ToLower(), curr_path?.ToLower());
            string res;
            if (GetUnitFileNameCache.TryGetValue(cache_key, out res))
                return res;

            // число приоритета меньше = более важная папка
            // может выглядеть задом-наперёд, но так должно быть проще в будущем добавлять папки...
            var SourceFileName = FindSourceFileName(path, curr_path, out var SourceFilePriority);
            var PCUFileName = FindPCUFileName(path, curr_path, out var PCUFilePriority);

            var SourceFileExists = SourceFileName != null;
            //ToDo то есть Rebuild режим игнорирует .pcu, даже если нет .pas файла?
            // а ещё ниже стоит ещё 1 проверка Rebuild...
            var PCUFileExists = !CompilerOptions.Rebuild && PCUFileName != null;

            if (!PCUFileExists && !SourceFileExists)
                if (UnitName == null)
                    // вызов с "UnitName == null" должен быть только там, где уже известно что хотя бы какой то файл есть
                    // если где то ещё будет исопльзоваться UnitName или source_context - надо будет добавить такую же проверку
                    throw new InvalidOperationException(nameof(UnitName));
                else
                    throw new UnitNotFound(CurrentCompilationUnit.SyntaxTree.file_name, UnitName, source_context);

            if (PCUFileExists && SourceFileExists)
            {

                //ToDo из за проверки Rebuild выше - тут всегда будет false
                // но я не понимаю какая из этих 2 проверок правильная
                if (CompilerOptions.Rebuild && !RecompileList.ContainsKey(PCUFileName))
                    PCUFileExists = false;

                else if (SourceFilePriority != PCUFilePriority)
                {
                    if (SourceFilePriority < PCUFilePriority)
                        PCUFileExists = false;
                    else
                        SourceFileExists = false;
                }
                else if (Path.GetDirectoryName(SourceFileName) != Path.GetDirectoryName(PCUFileName))
                    throw new InvalidOperationException("priority"); // не должно происходить, раз приоритет одинаковый

                else if (File.GetLastWriteTime(PCUFileName) < File.GetLastWriteTime(SourceFileName))
                    PCUFileExists = false;

            }

            if (PCUFileExists)
                res = Path.Combine(curr_path, PCUFileName);
            else if (SourceFileExists)
                res = Path.Combine(curr_path, SourceFileName);
            else
                throw new InvalidOperationException(nameof(SourceFileExists)); // тело "if (PCUFileExists && SourceFileExists)" не должно присваивать false обоим переменным

            GetUnitFileNameCache[cache_key] = res;
            return res;
        }

        private bool FileInSearchDirectory(string FileName)
        {
            return Path.GetDirectoryName(FileName).ToLower() == CompilerOptions.SearchDirectory.ToLower();
        }
        
        public void AddStandartUnitsToUsesSection(SyntaxTree.compilation_unit cu)
        {
            //if (FileInSearchDirectory(cu.file_name)) return;
            
            string ModuleName = null;
            SyntaxTree.uses_unit_in uses_unit_in = null;
            SyntaxTree.unit_or_namespace uses_unit = null;
            List<SyntaxTree.unit_or_namespace> UsesList = GetSyntaxInterfaceUsesList(cu);
            if (UsesList == null) 
                return;
            string cu_module_name = Path.GetFileNameWithoutExtension(cu.file_name).ToLower();
            foreach (CompilerOptions.StandartModule Module in CompilerOptions.StandartModules)
            {
                //Я стандартный??
                ModuleName = Path.GetFileNameWithoutExtension(Module.Name);
                if (ModuleName.ToLower() == cu_module_name)
                    return;             
            }
            foreach (CompilerOptions.StandartModule Module in CompilerOptions.StandartModules)
            {
                if ((Module.AddToLanguages & FirstCompilationUnit.SyntaxTree.Language) != FirstCompilationUnit.SyntaxTree.Language
                    && (Module.AddToLanguages & CurrentCompilationUnit.SyntaxTree.Language) != CurrentCompilationUnit.SyntaxTree.Language)
                    continue;
                ModuleName = Path.GetFileNameWithoutExtension(Module.Name);
                if (Module.AddMethod == CompilerOptions.StandartModuleAddMethod.RightToMain && CurrentCompilationUnit != FirstCompilationUnit)
                    continue;
                foreach (SyntaxTree.unit_or_namespace curunit in UsesList)
                {
                    if (curunit.name.idents.Count == 1 && curunit.name.idents[0].name.ToLower() == ModuleName.ToLower())
                        continue;
                }

                PascalABCCompiler.SyntaxTree.unit_or_namespace to_add;
                if (Path.GetExtension(Module.Name) != "" /*&& Path.GetExtension(ModuleFileName).ToLower() != ".dll"*/)
                {
                    uses_unit_in = new SyntaxTree.uses_unit_in(
                        _name: new SyntaxTree.ident_list(new SyntaxTree.ident(ModuleName)),
                        _in_file: new SyntaxTree.string_const(Module.Name));
                    //uses_unit_in.source_context = uses_unit_in.in_file.source_context = uses_unit_in.name.source_context = new SyntaxTree.SourceContext(1, 1, 1, 1);
                    to_add = uses_unit_in;
                }
                else
                {
                    uses_unit = new SyntaxTree.unit_or_namespace(new SyntaxTree.ident_list(new SyntaxTree.ident(ModuleName)));
                    //uses_unit.source_context = uses_unit.name.source_context = new SyntaxTree.SourceContext(1, 1, 1, 1);
                    to_add = uses_unit;
                }
                switch (Module.AddMethod)
                {
                    case CompilerOptions.StandartModuleAddMethod.RightToMain:
                        UsesList.Add(to_add);
                        break;
                    case CompilerOptions.StandartModuleAddMethod.LeftToAll:
                        UsesList.Insert(0, to_add);
                        break;
                }
                
            }
        }

        private CompilationUnit CompileReference(PascalABCCompiler.TreeRealization.unit_node_list Units, TreeRealization.compiler_directive cd)
        {
            TreeRealization.location loc = cd.location;
            SyntaxTree.SourceContext sc = null;
            if (loc != null)
                sc = new SyntaxTree.SourceContext(loc.begin_line_num, loc.begin_column_num, loc.end_line_num, loc.end_column_num, 0, 0);
            string UnitName = null;
            try
            {
                // "loc == null" для стандартных .dll, как "System.dll"
                //ToDo а loc.doc было null в тестранере, но я так и не понял почему. не смог воспроизвести в дебаг режиме
                UnitName = GetReferenceFileName(cd.directive, sc, loc?.doc?.file_name == null ? CompilerOptions.OutputDirectory : Path.GetDirectoryName(loc.doc.file_name));
            }
            catch (AssemblyNotFound ex)
            {
                throw;
            }
            //ToDo плохо, пока дебажил - тут постоянно ловились другие исключения, не связанные с неправильным знаками в путик сборке
            catch (Exception ex)
            {
                throw new InvalidAssemblyPathError(CurrentCompilationUnit.SyntaxTree.file_name, sc);
            }
            CompilationUnit CurrentUnit = null;
            if (UnitTable.Count == 0) throw new ProgramModuleExpected(UnitName, null);
            if ((CurrentUnit = ReadDLL(UnitName)) != null)
            {
                Units.AddElement(CurrentUnit.SemanticTree, null);
                UnitTable[UnitName] = CurrentUnit;
                return CurrentUnit;
            }
            else
                //throw new DLLReadingError(UnitName);
                throw new AssemblyReadingError(CurrentCompilationUnit.SyntaxTree.file_name, UnitName, sc);
        }

        private bool HasIncludeNamespacesDirective(CompilationUnit Unit)
        {
            var directives = ConvertDirectives(Unit.SyntaxTree);
            foreach (TreeRealization.compiler_directive cd in directives)
            {
                if (cd.name.ToLower() == TreeConverter.compiler_string_consts.include_namespace_directive)
                {
                    return true;
                }
            }
            return false;
        }
     

        private Dictionary<string, SyntaxTree.syntax_namespace_node> IncludeNamespaces(CompilationUnit Unit)
        {
            if (HasIncludeNamespacesDirective(Unit) && Unit.SyntaxTree is SyntaxTree.unit_module && (Unit.SyntaxTree as SyntaxTree.unit_module).unit_name.HeaderKeyword != SyntaxTree.UnitHeaderKeyword.Library)
                throw new IncludeNamespaceInUnit(CurrentCompilationUnit.SyntaxTree.file_name, CurrentCompilationUnit.SyntaxTree.source_context);
            var directives = ConvertDirectives(Unit.SyntaxTree);
            SyntaxTree.unit_module main_library = Unit.SyntaxTree as SyntaxTree.unit_module;
            SyntaxTree.program_module main_program = Unit.SyntaxTree as SyntaxTree.program_module;
            List<string> files = new List<string>();
            foreach (TreeRealization.compiler_directive cd in directives)
            {
                if (cd.name.ToLower() == TreeConverter.compiler_string_consts.include_namespace_directive)
                {
                    string directive = cd.directive.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);

                    if (directive == "*.pas" || directive.EndsWith(Path.DirectorySeparatorChar + "*.pas"))
                    {
                        string dir = Path.Combine(Path.GetDirectoryName(Unit.SyntaxTree.file_name), directive.Replace(Path.DirectorySeparatorChar + "*.pas", ""));
                        foreach (string file in Directory.EnumerateFiles(dir, "*.pas"))
                        {
                            if (!File.Exists(file))
                                throw new FileNotFound(file, cd.location);
                            files.Add(file);
                        }
                            
                    }
                    else
                    {
                        string file = Path.Combine(Path.GetDirectoryName(Unit.SyntaxTree.file_name), directive);
                        if (!File.Exists(file))
                            throw new FileNotFound(file, cd.location);
                        files.Add(file);
                    }
                       


                }
            }
            Dictionary<string, SyntaxTree.syntax_namespace_node> namespaces = new Dictionary<string, SyntaxTree.syntax_namespace_node>(StringComparer.OrdinalIgnoreCase);
            List<SyntaxTree.unit_or_namespace> namespace_modules = new List<SyntaxTree.unit_or_namespace>();
            foreach (string file in files)
            {
                
                SyntaxTree.compilation_unit tree = GetNamespaceSyntaxTree(file);
                if (!(tree is SyntaxTree.unit_module))
                    throw new NamespaceModuleExpected(tree.source_context);
                SyntaxTree.unit_module unit = tree as SyntaxTree.unit_module;
                if (unit.unit_name.HeaderKeyword != SyntaxTree.UnitHeaderKeyword.Namespace)
                    throw new NamespaceModuleExpected(unit.unit_name.source_context);
                if (unit.implementation_part != null)
                    throw new NamespaceModuleExpected(unit.implementation_part.source_context);
                if (unit.finalization_part != null)
                    throw new NamespaceModuleExpected(unit.finalization_part.source_context);
                if (unit.initialization_part != null && unit.initialization_part.list.Count > 0)
                    throw new NamespaceModuleExpected(unit.initialization_part.source_context);
                SyntaxTree.syntax_namespace_node ns = null;
                if (!namespaces.TryGetValue(unit.unit_name.idunit_name.name, out ns))
                {
                    ns = new SyntaxTree.syntax_namespace_node(unit.unit_name.idunit_name.name);
                    ns.referenced_units = new TreeRealization.unit_node_list();
                    namespaces[unit.unit_name.idunit_name.name] = ns;
                }
                if (unit.interface_part.interface_definitions != null)
                {
                    foreach (SyntaxTree.declaration decl in unit.interface_part.interface_definitions.defs)
                    {
                        ns.defs.Add(decl);
                    }
                    if (unit.interface_part.uses_modules != null)
                    {
                        checkDuplicateUsesUnit(unit.interface_part.uses_modules.units);
                        foreach (SyntaxTree.unit_or_namespace name_space in unit.interface_part.uses_modules.units)
                        {
                            if (IsPossibleNamespace(name_space, false, Path.GetDirectoryName(file)))
                            {
                                ns.referenced_units.AddElement(new TreeRealization.namespace_unit_node(GetNamespace(name_space), get_location_from_treenode(name_space, tree.file_name)), null);
                            }
                            else
                            {
                                namespace_modules.Add(name_space);
                            }
                        }
                    }
                }
            }
            foreach (string s in namespaces.Keys)
            {
                if (main_library != null)
                    main_library.interface_part.interface_definitions.Insert(0, namespaces[s]);
                else
                    main_program.program_block.defs.Insert(0, namespaces[s]);
            }
            SyntaxTree.uses_list main_uses = null;
            if (main_library != null)
            {
                if (main_library.interface_part.uses_modules != null)
                    main_uses = main_library.interface_part.uses_modules;
            }
            else if (main_program.used_units != null)
                main_uses = main_program.used_units;
            if (main_uses == null)
                main_uses = new SyntaxTree.uses_list();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (SyntaxTree.unit_or_namespace name_space in namespace_modules)
            {
                string name = SyntaxTree.Utils.IdentListToString(name_space.name.idents, ".").ToLower();
                if (!dict.ContainsKey(name))
                {
                    main_uses.Add(name_space);
                    dict.Add(name, name);
                }
            }
            if (main_library != null)
                main_library.interface_part.uses_modules = main_uses;
            else
                main_program.used_units = main_uses;
            return namespaces;
        }

        private SyntaxTree.compilation_unit GetNamespaceSyntaxTree(string FileName)
        {
            string SourceText = GetSourceFileText(FileName);
            List<string> DefinesList = new List<string>();
            DefinesList.Add("PASCALABC"); // SSM 11/07/20
            if (!compilerOptions.Debug && !compilerOptions.ForDebugging)
                DefinesList.Add("RELEASE");
            else
                DefinesList.Add("DEBUG");
            SyntaxTree.compilation_unit SyntaxTree = InternalParseText(FileName, SourceText, errorsList, warnings, DefinesList);
            if (errorsList.Count > 0)
                throw errorsList[0];
            SyntaxTree = syntaxTreeConvertersController.Convert(SyntaxTree) as SyntaxTree.compilation_unit;
            return SyntaxTree;
        }

        public TreeRealization.unit_node_list GetReferences(CompilationUnit Unit)
        {
            //TODO переделать, ConvertDirectives определена дважды и вызывается дважды!
            TreeRealization.unit_node_list res = new TreeRealization.unit_node_list();
            List<TreeRealization.compiler_directive> directives;
            if (Unit.SemanticTree is TreeRealization.common_unit_node)
                directives = (Unit.SemanticTree as TreeRealization.common_unit_node).compiler_directives;
            else
                directives = ConvertDirectives(Unit.SyntaxTree);
            if (CompilerOptions.UseDllForSystemUnits)
            {
                directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\PABCRtl.dll", null, null));
                directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\mscorlib.dll", null, null));
                directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\System.dll", null, null));
                directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\System.Core.dll", null, null));
                directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\System.Numerics.dll", null, null));
                directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\System.Windows.Forms.dll", null, null));
                directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\System.Drawing.dll", null, null));
                if (Unit.SyntaxTree is SyntaxTree.program_module && (Unit.SyntaxTree as SyntaxTree.program_module).used_units != null)
                foreach (SyntaxTree.unit_or_namespace uui in (Unit.SyntaxTree as SyntaxTree.program_module).used_units.units)
                {
                    if (uui.name.ToString() == "Graph3D")
                    {
                        directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\PresentationFramework.dll", null, null));
                        directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\WindowsBase.dll", null, null));
                        directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\PresentationCore.dll", null, null));
                        directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\HelixToolkit.Wpf.dll", null, null));
                        directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\HelixToolkit.dll", null, null));
                    }
                }
            }
            foreach (TreeRealization.compiler_directive cd in directives)
            {
                if (cd.name.ToLower() == TreeConverter.compiler_string_consts.compiler_directive_reference)
                {
                    if (string.IsNullOrEmpty(cd.directive))
                        throw new TreeConverter.SimpleSemanticError(cd.location, "EXPECTED_ASSEMBLY_NAME");
                    else
                        CompileReference(res, cd);
                }
            }
            if (CompilerOptions.ProjectCompiled)
            {
            	foreach (ReferenceInfo ri in project.references)
            	{
            		CompileReference(res,new TreeRealization.compiler_directive("reference",ri.full_assembly_name,null,null));
            	}
            }
            return res;
        }

        private bool IsPossibleNamespace(SyntaxTree.unit_or_namespace name_space, bool add_to_standard_modules, string curr_path)
        {
            if (name_space is SyntaxTree.uses_unit_in)
                return false;
            if (name_space.name.idents.Count > 1)
                return true;
            string src = FindSourceFileName(name_space.name.idents[0].name, curr_path, out _);
            string pcu = FindPCUFileName(name_space.name.idents[0].name, curr_path, out _);
            if (src == null && pcu == null)
                return true;
            if (CompilerOptions.UseDllForSystemUnits && src != null && string.Compare(Path.GetDirectoryName(src), Path.Combine(CompilerOptions.SystemDirectory, "Lib"), true) == 0
                && string.Compare(Path.GetFileNameWithoutExtension(src), "PT4", true) != 0 
                && string.Compare(Path.GetFileNameWithoutExtension(src), "CRT", true) != 0 
                && string.Compare(Path.GetFileNameWithoutExtension(src), "Arrays", true) != 0
                //&& string.Compare(Path.GetFileNameWithoutExtension(src), "FormsABC", true) != 0
                && string.Compare(Path.GetFileNameWithoutExtension(src), "MPI", true) != 0
                && string.Compare(Path.GetFileNameWithoutExtension(src), "LibForHaskell", true) != 0
                && string.Compare(Path.GetFileNameWithoutExtension(src), "Collections", true) != 0
                && string.Compare(Path.GetFileNameWithoutExtension(src), "Core", true) != 0
                && string.Compare(Path.GetFileNameWithoutExtension(src), "Oberon00System", true) != 0)
            {
                string s = Path.GetFileNameWithoutExtension(src).ToLower();
                if (add_to_standard_modules && !StandarModules.Contains(s))
                    StandarModules.Add(s);
                return true;
            }
            if (CompilerOptions.UseDllForSystemUnits && pcu != null && string.Compare(Path.GetDirectoryName(pcu), Path.Combine(CompilerOptions.SystemDirectory, "Lib"), true) == 0
                && string.Compare(Path.GetFileNameWithoutExtension(pcu), "PT4", true) != 0 
                && string.Compare(Path.GetFileNameWithoutExtension(pcu), "CRT", true) != 0 
                && string.Compare(Path.GetFileNameWithoutExtension(pcu), "Arrays", true) != 0
                //&& string.Compare(Path.GetFileNameWithoutExtension(pcu), "FormsABC", true) != 0
                && string.Compare(Path.GetFileNameWithoutExtension(pcu), "MPI", true) != 0
                && string.Compare(Path.GetFileNameWithoutExtension(pcu), "LibForHaskell", true) != 0
                && string.Compare(Path.GetFileNameWithoutExtension(pcu), "Collections", true) != 0
                && string.Compare(Path.GetFileNameWithoutExtension(pcu), "Core", true) != 0
                && string.Compare(Path.GetFileNameWithoutExtension(pcu), "Oberon00System", true) != 0
                )
            {
                string s = Path.GetFileNameWithoutExtension(pcu).ToLower();
                if (add_to_standard_modules && !StandarModules.Contains(s))
                    StandarModules.Add(s);
                return true;
            }
            return false;
        }

        public TreeRealization.using_namespace GetNamespace(SyntaxTree.unit_or_namespace _name_space)
        {
            return new TreeRealization.using_namespace(SyntaxTree.Utils.IdentListToString(_name_space.name.idents, "."));
        }

        private TreeRealization.using_namespace GetNamespace(TreeRealization.using_namespace_list using_list, string full_namespace_name, SyntaxTree.unit_or_namespace _name_space, bool possible_is_unit, Dictionary<string, SyntaxTree.syntax_namespace_node> pabc_namespaces)
        {
            if (!NetHelper.NetHelper.NamespaceExists(full_namespace_name) && !(pabc_namespaces != null && pabc_namespaces.ContainsKey(full_namespace_name)))
            {
            	if (possible_is_unit)
                    if (!full_namespace_name.Contains("."))
                        throw new UnitNotFound(CurrentCompilationUnit.SyntaxTree.file_name, full_namespace_name, _name_space.source_context);
                throw new TreeConverter.NamespaceNotFound(full_namespace_name, get_location_from_treenode(_name_space.name, CurrentCompilationUnit.SyntaxTree.file_name));
            }
            return new TreeRealization.using_namespace(full_namespace_name);
        }

        public void AddNamespaces(TreeRealization.using_namespace_list using_list, List<SyntaxTree.unit_or_namespace> namespaces, bool possible_is_units, Dictionary<string, SyntaxTree.syntax_namespace_node> pabc_namespaces)
        {
            foreach (SyntaxTree.unit_or_namespace ns in namespaces)
                using_list.AddElement(GetNamespace(using_list, SyntaxTree.Utils.IdentListToString(ns.name.idents, "."), ns, possible_is_units, pabc_namespaces));
        }
        
        public void AddNamespaces(TreeRealization.using_namespace_list using_list, SyntaxTree.using_list ul)
        {
            if (ul != null) 
                AddNamespaces(using_list, ul.namespaces, false, null);
        }

        public SyntaxTree.using_list GetInterfaceSyntaxUsingList(SyntaxTree.compilation_unit cu)
        {
            if (cu is SyntaxTree.unit_module)
                return (cu as SyntaxTree.unit_module).interface_part.using_namespaces;
            if (cu is SyntaxTree.program_module)
                return (cu as SyntaxTree.program_module).using_namespaces;
            return null;
        }
        
        private SyntaxTree.using_list GetImplementationSyntaxUsingList(SyntaxTree.compilation_unit cu)
        {
            if (cu is SyntaxTree.unit_module)
                if ((cu as SyntaxTree.unit_module).implementation_part!=null)
                    return (cu as SyntaxTree.unit_module).implementation_part.using_namespaces;
            return null;
        }

        public string GetSourceFileText(string FileName)
        {
            return (string)SourceFilesProvider(FileName, SourceFileOperation.GetText);
        }


        public SyntaxTree.compilation_unit ParseText(string FileName, string Text, List<Error> ErrorList, List<CompilerWarning> Warnings)
        {
            Reset();
            OnChangeCompilerState(this, CompilerState.CompilationStarting, FileName);
            SyntaxTree.compilation_unit cu = InternalParseText(FileName, Text, ErrorsList, Warnings);
            OnChangeCompilerState(this, CompilerState.Ready, FileName);
            return cu;
        }

        private SyntaxTree.compilation_unit InternalParseText(string FileName, string Text, List<Error> ErrorList, List<CompilerWarning> Warnings, List<string> DefinesList = null)
        {
            OnChangeCompilerState(this, CompilerState.BeginParsingFile, FileName);
            SyntaxTree.compilation_unit cu = ParsersController.GetCompilationUnit(FileName, Text, ErrorsList, Warnings, DefinesList);
            OnChangeCompilerState(this, CompilerState.EndParsingFile, FileName);
            //Вычисляем сколько строк скомпилировали
            if (ErrorList.Count == 0 && cu != null && cu.source_context!=null)
            {
                linesCompiled += (uint)(cu.source_context.end_position.line_num - cu.source_context.begin_position.line_num + 1);
                // 500 - это наибольшая программа для начинающих. БОльшая программа - здоровье кода только по кнопке (чтобы не замедлять)
                if (linesCompiled <= 500)
                {
                    // Это только для локального компилятора?
                    var stat = new SyntaxVisitors.ABCStatisticsVisitor();
                    stat.ProcessNode(cu);
                    pABCCodeHealth = stat.CalcHealth(out int neg, out int pos);
                }
            }
            return cu;            
        }

        /*private bool check_for_library(List<compiler_directive> directives)
        {
        	foreach (compiler_directive cd in directives)
        	{
        		if (string.Compare(cd.name,"apptype")==0 && string.Compare(cd.directive,"dll")==0)
        			return true;
        	}
        	return false;
        }*/
        
        public static bool is_dll(SyntaxTree.compilation_unit cu)
        {
            foreach (SyntaxTree.compiler_directive cd in cu.compiler_directives)
                if (string.Compare(cd.Name.text, "apptype", true) == 0 && string.Compare(cd.Directive.text, "dll", true) == 0)
                    return true;
            return false;
        }

        public CompilationUnit CompileUnit(PascalABCCompiler.TreeRealization.unit_node_list Units, Dictionary<unit_node, CompilationUnit> DirectCompilationUnits, SyntaxTree.unit_or_namespace SyntaxUsesUnit, string prev_path)
        {
            string UnitName = GetUnitFileName(SyntaxUsesUnit, prev_path);
            // имя папки, в которой лежит текущий модуль
            // используется для подключения модулей, $include и т.п. из модуля, подключённого с uses-in
            var curr_path = Path.GetDirectoryName(UnitName);

            CompilationUnit CurrentUnit = UnitTable[UnitName];
            if (CurrentUnit != null && CurrentUnit.SemanticTree is PascalABCCompiler.TreeRealization.dot_net_unit_node 
                && SyntaxUsesUnit is PascalABCCompiler.SyntaxTree.uses_unit_in ui && ui.in_file != null) // значит, это пространство имен и секция in у него должна отсутствовать
            {
                ErrorsList.Add(new NamespaceCannotHaveInSection(ui.in_file.source_context));
            }

            if (Path.GetExtension(UnitName).ToLower() == CompilerOptions.CompiledUnitExtension)
            {
                string sfn = FindSourceFileName(Path.ChangeExtension(UnitName, null), null, out _);
                if (sfn != null && UnitTable[sfn] != null)
                    CurrentUnit = UnitTable[sfn];
            }

            if (CurrentUnit != null)
                if (CurrentUnit.State != UnitState.BeginCompilation || CurrentUnit.SemanticTree != null)  //ИЗБАВИТЬСЯ ОТ ВТОРОГО УСЛОВИЯ
                {
                    if (Units.AddElement(CurrentUnit.SemanticTree, SyntaxUsesUnit.UsesPath()))
                        DirectCompilationUnits.Add(CurrentUnit.SemanticTree, CurrentUnit);
                    Units.AddRange(GetReferences(CurrentUnit));
                    return CurrentUnit;
                }

            if (UnitName.ToLower().LastIndexOf(".dll") >= 0 || UnitName.ToLower().LastIndexOf(".exe") >= 0)
                if (File.Exists(UnitName))
                {
                    if (UnitTable.Count == 0) throw new ProgramModuleExpected(UnitName, null);
                    if ((CurrentUnit = ReadDLL(UnitName)) != null)
                    {
                        if (Units.AddElement(CurrentUnit.SemanticTree, SyntaxUsesUnit.UsesPath()))
                            DirectCompilationUnits.Add(CurrentUnit.SemanticTree, CurrentUnit);
                        UnitTable[UnitName] = CurrentUnit;
                        return CurrentUnit;
                    }
                    else
                        //throw new DLLReadingError(UnitName);
                        throw new AssemblyReadingError(CurrentCompilationUnit.SyntaxTree.file_name, UnitName, SyntaxUsesUnit.source_context);
                }
            if (Path.GetExtension(UnitName).ToLower() == CompilerOptions.CompiledUnitExtension)
                if (File.Exists(UnitName))
                {
                    if (UnitTable.Count == 0) throw new ProgramModuleExpected(UnitName, null);
                    try
                    {
                        if ((CurrentUnit = ReadPCU(UnitName)) != null)
                        {
                            if (Units.AddElement(CurrentUnit.SemanticTree, SyntaxUsesUnit.UsesPath()))
                                DirectCompilationUnits.Add(CurrentUnit.SemanticTree, CurrentUnit);
                            Units.AddRange(GetReferences(CurrentUnit));
                            UnitTable[UnitName] = CurrentUnit;
                            return CurrentUnit;
                        }
                    }
                    catch (InvalidPCUFule)
                    {
                        //Перекомпилируем....
                    }
                    catch (Error)
                    {
                        throw;
                    }
                    catch (Exception e)
                    {
                        OnChangeCompilerState(this, CompilerState.PCUReadingError, UnitName);
#if DEBUG
                        if (!InternalDebug.SkipPCUErrors)
                            throw new Errors.CompilerInternalError("PCUReader", e);
#endif
                    }
                    string SourceFileName = FindSourceFileName(Path.ChangeExtension(UnitName, null), null, out _);
                    if (SourceFileName == null)
                        throw new ReadPCUError(UnitName);
                    else
                        UnitName = SourceFileName;
                }
            string SourceText = null;
            Dictionary<SyntaxTree.syntax_tree_node, string> docs = null;
            if (CurrentUnit == null)
            {

                CurrentUnit = new CompilationUnit();
                if (FirstCompilationUnit == null)
                    FirstCompilationUnit = CurrentUnit;
                OnChangeCompilerState(this, CompilerState.BeginCompileFile, UnitName);
                SourceText = GetSourceFileText(UnitName);
                if (SourceText == null)
                    if (CurrentUnit == FirstCompilationUnit)
                        throw new SourceFileNotFound(UnitName);
                    else
                        throw new UnitNotFound(CurrentCompilationUnit.SyntaxTree.file_name, UnitName, SyntaxUsesUnit.source_context);
                List<string> DefinesList = new List<string>();
                DefinesList.Add("PASCALABC");
                if (!compilerOptions.Debug && !compilerOptions.ForDebugging)
                    DefinesList.Add("RELEASE");
                else
                    DefinesList.Add("DEBUG");
                CurrentUnit.SyntaxTree = InternalParseText(UnitName, SourceText, errorsList, warnings, DefinesList);
                
                if (errorsList.Count == 0) // SSM 2/05/16 - для преобразования синтаксических деревьев извне
                {
                    CurrentUnit.SyntaxTree = syntaxTreeConvertersController.Convert(CurrentUnit.SyntaxTree) as SyntaxTree.compilation_unit;
                    
                }

                if (errorsList.Count == 0 && need_gen_doc(CurrentUnit.SyntaxTree))
                {
                    if (SourceText != null)
                    {
                        docs = AddDocumentationToNodes(CurrentUnit.SyntaxTree, SourceText);
                        if (docs != null)
                            CurrentUnit.Documented = true;
                    }
                }
                if (CurrentUnit.SyntaxTree is SyntaxTree.unit_module)
                {
                    if ((CurrentUnit.SyntaxTree as SyntaxTree.unit_module).unit_name.HeaderKeyword == SyntaxTree.UnitHeaderKeyword.Namespace)
                        throw new NamespacesCanBeCompiledOnlyInProjects(CurrentUnit.SyntaxTree.source_context);
                    compilerOptions.UseDllForSystemUnits = false;
                }

                // SSM 21/05/20 Проверка, что мы не записали apptype dll в небиблиотеку

                var ccu = CurrentUnit.SyntaxTree;
                foreach (SyntaxTree.compiler_directive cd in ccu.compiler_directives)
                    if (string.Compare(cd.Name.text, "apptype", true) == 0 && string.Compare(cd.Directive.text, "dll", true) == 0)
                    {
                        if (!(ccu is SyntaxTree.unit_module) ||
                            (ccu is SyntaxTree.unit_module um && um.unit_name.HeaderKeyword != SyntaxTree.UnitHeaderKeyword.Library))
                        {
                            ErrorsList.Add(new AppTypeDllIsAllowedOnlyForLibraries(ccu.file_name, cd.source_context));
                            break;
                        }
                    }
                //

                if (is_dll(CurrentUnit.SyntaxTree))
                    compilerOptions.OutputFileType = PascalABCCompiler.CompilerOptions.OutputType.ClassLibrary;
                CurrentUnit.CaseSensitive = ParsersController.LastParser.CaseSensitive;
                CurrentCompilationUnit = CurrentUnit;

                CurrentUnit.SyntaxUnitName = SyntaxUsesUnit;

                //BadNodesInSyntaxTree.Clear();
                if (errorsList.Count > 0)
                {
                    CurrentUnit.syntax_error = errorsList[0] as PascalABCCompiler.Errors.SyntaxError;
                    foreach (Errors.Error er in errorsList)
                        if (er is SyntaxError && (er as SyntaxError).bad_node != null)
                            BadNodesInSyntaxTree[(er as SyntaxError).bad_node] = er;
                }

                //if (CurrentUnit.SyntaxTree == null)
                if (errorsList.Count > 0)
                {
                    //if (errorsList.Count == 0)
                    //    throw new Errors.SyntaxError("Internal parser error: Parser not create syntax tree", UnitName,null,null);
                    throw errorsList[0];
                }

                UnitTable[UnitName] = CurrentUnit;

                if (UnitTable.Count > 1)//если это не главный модуль
                    if (CurrentUnit.SyntaxTree is SyntaxTree.program_module)
                        throw new UnitModuleExpected(UnitName, CurrentUnit.SyntaxTree.source_context.LeftSourceContext);
                    else if (is_dll(CurrentUnit.SyntaxTree))
                        throw new UnitModuleExpectedLibraryFound(UnitName, CurrentUnit.SyntaxTree.source_context.LeftSourceContext);
                //здесь в начало uses добавляем стандартные модули
#if DEBUG
                if (InternalDebug.AddStandartUnits)
#endif
                    AddStandartUnitsToUsesSection(CurrentUnit.SyntaxTree);

            }
            CurrentSyntaxUnit = SyntaxUsesUnit;
            CurrentCompilationUnit = CurrentUnit;

            CurrentUnit.PossibleNamespaces.Clear();

            //TODO переделать, слишком сложно, некоторый код дублируется
            

            System.Collections.Generic.List<SyntaxTree.unit_or_namespace> SyntaxUsesList;
            SyntaxUsesList = GetSyntaxInterfaceUsesList(CurrentUnit.SyntaxTree);

            if (SyntaxUsesList != null)
            {
                for (int i = SyntaxUsesList.Count - 1 - CurrentUnit.InterfaceUsedUnits.Count; i >= 0; i--)
                {
                    if (!IsPossibleNamespace(SyntaxUsesList[i], false, curr_path))
                    {
                        compilerOptions.UseDllForSystemUnits = false;
                        break;
                    }
                }
            }
            TreeRealization.unit_node_list References = GetReferences(CurrentUnit);
            var namespaces = IncludeNamespaces(CurrentUnit);
            if (SyntaxUsesList != null)
            {
                
                for (int i = SyntaxUsesList.Count - 1 - CurrentUnit.InterfaceUsedUnits.Count; i >= 0; i--)
                {
                    if (IsPossibleNamespace(SyntaxUsesList[i], true, curr_path) || namespaces.ContainsKey(SyntaxUsesList[i].name.idents[0].name))
                    {
                        CurrentUnit.InterfaceUsedUnits.AddElement(new TreeRealization.namespace_unit_node(GetNamespace(SyntaxUsesList[i])), null);
                        CurrentUnit.PossibleNamespaces.Add(SyntaxUsesList[i]);
                    }
                    else
                    {
                        string CurrentSyntaxUnitName = GetUnitFileName(SyntaxUsesList[i], curr_path);
                        CurrentUnit.CurrentUsesUnit = CurrentSyntaxUnitName;
                        if (UnitTable[CurrentSyntaxUnitName] != null)
                            if (UnitTable[CurrentSyntaxUnitName].State == UnitState.BeginCompilation)
                            {
                                string CurrentSyntaxUnitNameCurrentUsesUnit = UnitTable[CurrentSyntaxUnitName].CurrentUsesUnit;
                                if (CurrentSyntaxUnitNameCurrentUsesUnit != null)
                                {
                                    //если сначало взали pcu а потом решили его перекомпилировать, поэтому в таблице его нет
                                    if (UnitTable[CurrentSyntaxUnitNameCurrentUsesUnit] == null)
                                        UnitTable[CurrentSyntaxUnitName].CurrentUsesUnit = FindSourceFileName(Path.ChangeExtension(CurrentSyntaxUnitNameCurrentUsesUnit, null), null, out _);
                                    //далее финальная поверка на зацикливание
                                    if (UnitTable[CurrentSyntaxUnitName].CurrentUsesUnit != null && UnitTable[UnitTable[CurrentSyntaxUnitName].CurrentUsesUnit].State == UnitState.BeginCompilation)
                                        throw new CycleUnitReference(UnitName, SyntaxUsesList[i]);
                                }
                            }
                        CompileUnit(CurrentUnit.InterfaceUsedUnits, CurrentUnit.DirectInterfaceCompilationUnits, SyntaxUsesList[i], curr_path);
                        if (CurrentUnit.State == UnitState.Compiled)
                        {
                            if (Units.AddElement(CurrentUnit.SemanticTree, SyntaxUsesUnit.UsesPath()))
                                DirectCompilationUnits.Add(CurrentUnit.SemanticTree, CurrentUnit);
                            Units.AddRange(References);
                            return CurrentUnit;
                        }
                    }

                }
            }

            CurrentCompilationUnit = CurrentUnit;

            CurrentUnit.InterfaceUsedUnits.AddRange(References);

            AddNamespaces(CurrentUnit.InterfaceUsingNamespaceList, CurrentUnit.PossibleNamespaces, true, namespaces);
            AddNamespaces(CurrentUnit.InterfaceUsingNamespaceList, GetInterfaceSyntaxUsingList(CurrentUnit.SyntaxTree));
            
            //Console.WriteLine("Compile Interface "+UnitName);//DEBUG
#if DEBUG
            if (InternalDebug.SemanticAnalysis)
#endif
            {
                if (CurrentUnit.State != UnitState.InterfaceCompiled)
                {
                    OnChangeCompilerState(this, CompilerState.CompileInterface, UnitName);
                    PascalABCCompiler.TreeConverter.SemanticRules.SymbolTableCaseSensitive = CurrentUnit.CaseSensitive;
                    CurrentUnit.SemanticTree = SyntaxTreeToSemanticTreeConverter.CompileInterface(
                        CurrentUnit.SyntaxTree,
                        CurrentUnit.InterfaceUsedUnits,
                        ErrorsList, Warnings,
                        CurrentUnit.syntax_error,
                        BadNodesInSyntaxTree,
                        CurrentUnit.InterfaceUsingNamespaceList,
                        docs,
                        CompilerOptions.Debug,
                        CompilerOptions.ForDebugging
                        );
                    CheckErrors();
                }
            }
            TreeRealization.common_unit_node cun = CurrentUnit.SemanticTree as TreeRealization.common_unit_node;
            /*if (cun != null)
            {
            	if (!UnitsSortedList.Contains(CurrentUnit))//vnimanie zdes inogda pri silnoj zavisimosti modulej moduli popadajut neskolko raz
            	UnitsSortedList.Add(CurrentUnit);
            }*/
            CurrentUnit.State = UnitState.InterfaceCompiled;
            if (Units != null)
            {
                if (Units.AddElement(CurrentUnit.SemanticTree, SyntaxUsesUnit.UsesPath()))
                    DirectCompilationUnits.Add(CurrentUnit.SemanticTree, CurrentUnit);
                Units.AddRange(References);
            }
            SyntaxUsesList = GetSyntaxImplementationUsesList(CurrentUnit.SyntaxTree);
            CompilationUnit cu = null; bool interfcompile = true;
            CurrentUnit.ImplementationUsedUnits.clear();
            CurrentUnit.PossibleNamespaces.Clear();
            if (SyntaxUsesList != null)
            {
                for (int i = SyntaxUsesList.Count - 1; i >= 0; i--)
                    if (!IsPossibleNamespace(SyntaxUsesList[i], true, curr_path))
                    {
                        cu = UnitTable[GetUnitFileName(SyntaxUsesList[i], curr_path)];
                        if (cu != null && cu.State == UnitState.BeginCompilation)
                        {
                            UnitsToCompile.Add(cu);
                            interfcompile = false;
#if DEBUG
                            //                            Console.WriteLine("[DEBUGINFO]Send compile to end " + Path.GetFileName(GetUnitFileName(SyntaxUsesList[i])));//DEBUG
#endif
                        }
                        else
                        {
                            CompileUnit(CurrentUnit.ImplementationUsedUnits, CurrentUnit.DirectImplementationCompilationUnits, SyntaxUsesList[i], curr_path);
                        }
                    }
                    else
                    {
                        CurrentUnit.ImplementationUsedUnits.AddElement(new TreeRealization.namespace_unit_node(GetNamespace(SyntaxUsesList[i])), null);
                        CurrentUnit.PossibleNamespaces.Add(SyntaxUsesList[i]);
                    }
            }

            CurrentCompilationUnit = CurrentUnit;

            AddNamespaces(CurrentUnit.ImplementationUsingNamespaceList, CurrentUnit.PossibleNamespaces, true, namespaces);
            AddNamespaces(CurrentUnit.ImplementationUsingNamespaceList, GetImplementationSyntaxUsingList(CurrentUnit.SyntaxTree));

            if (!interfcompile)
            {
                UnitsToCompile.Add(CurrentUnit);
                if (cun != null)
                {
                    if (!UnitsSortedList.Contains(CurrentUnit))//vnimanie zdes inogda pri silnoj zavisimosti modulej moduli popadajut neskolko raz
                        UnitsSortedList.Add(CurrentUnit);
                }
                //Console.WriteLine("Send compile to end "+UnitName);//DEBUG
                return CurrentUnit;
            }

            //Console.WriteLine("Compile Implementation "+UnitName);//DEBUG
            if (CurrentUnit.SyntaxTree is SyntaxTree.unit_module)
            {
#if DEBUG
                if (InternalDebug.SemanticAnalysis)
#endif
                {
                    OnChangeCompilerState(this, CompilerState.CompileImplementation, UnitName);
                    PascalABCCompiler.TreeConverter.SemanticRules.SymbolTableCaseSensitive = CurrentUnit.CaseSensitive;
                    SyntaxTreeToSemanticTreeConverter.CompileImplementation(
                        (PascalABCCompiler.TreeRealization.common_unit_node)CurrentUnit.SemanticTree,
                        CurrentUnit.SyntaxTree,
                        buildImplementationUsesList(CurrentUnit),
                        ErrorsList, Warnings,
                        CurrentUnit.syntax_error,
                        BadNodesInSyntaxTree,
                        CurrentUnit.InterfaceUsingNamespaceList,
                        CurrentUnit.ImplementationUsingNamespaceList,
                        docs,
                        CompilerOptions.Debug,
                        CompilerOptions.ForDebugging
                        );
                    CheckErrors();
                }
            }
            CurrentUnit.State = UnitState.Compiled;
            if (cun != null)
            {
                if (!UnitsSortedList.Contains(CurrentUnit))//vnimanie zdes inogda pri silnoj zavisimosti modulej moduli popadajut neskolko raz
                    UnitsSortedList.Add(CurrentUnit);
            }
            OnChangeCompilerState(this, CompilerState.EndCompileFile, UnitName);
            //SavePCU(CurrentUnit, UnitName);
            CurrentUnit.UnitName = UnitName;
            return CurrentUnit;
            /*if(CurrentUnit.State!=UnitState.Compiled)
            { 
                //Console.WriteLine("Compile Interface "+UnitName);//DEBUG
                CurrentUnit.SemanticTree=SyntaxTreeToSemanticTreeConverter.CompileInterface(CurrentUnit.SyntaxTree,
                    CurrentUnit.InterfaceUsedUnits,CurrentUnit.syntax_error);
                CurrentUnit.State=UnitState.InterfaceCompiled;
                SyntaxUsesList=GetSemanticImplementationUsesList(CurrentUnit.SyntaxTree);
                if(SyntaxUsesList!=null)
                    for(int i=SyntaxUsesList.Count-1;i>=0;i--)
                        CompileUnit(CurrentUnit.ImplementationUsedUnits,SyntaxUsesList[i]);        
                //Console.WriteLine("Compile Implementation "+UnitName);//DEBUG
                if (CurrentUnit.SyntaxTree is SyntaxTree.unit_module)
                {
                    SyntaxTreeToSemanticTreeConverter.CompileImplementation(CurrentUnit.SemanticTree,
                        CurrentUnit.SyntaxTree,CurrentUnit.ImplementationUsedUnits,CurrentUnit.syntax_error);
                }
                CurrentUnit.State=UnitState.Compiled;
                Units.Add(CurrentUnit.SemanticTree);
                SaveSemanticTreeToFile(CurrentUnit,UnitName);
            }*/
        }
		
        private Dictionary<SyntaxTree.syntax_tree_node,string> AddDocumentationToNodes(SyntaxTree.compilation_unit cu, string Text)
        {
        	List<PascalABCCompiler.Errors.Error> errors = new List<PascalABCCompiler.Errors.Error>();
            List<PascalABCCompiler.Errors.CompilerWarning> warnings = new List<CompilerWarning>();
        	string doctagsParserExtension = Path.GetExtension(cu.file_name)+"dt"+PascalABCCompiler.Parsers.Controller.HideParserExtensionPostfixChar;
        	PascalABCCompiler.SyntaxTree.documentation_comment_list dt = ParsersController.Compile(System.IO.Path.ChangeExtension(cu.file_name, doctagsParserExtension), Text, errors, warnings, PascalABCCompiler.Parsers.ParseMode.Normal) as PascalABCCompiler.SyntaxTree.documentation_comment_list;
        	if (errors.Count > 0) return null;
        	PascalABCCompiler.DocumentationConstructor docconst = new PascalABCCompiler.DocumentationConstructor();
            return docconst.Construct(cu, dt);
        }
        
        private bool need_gen_doc(SyntaxTree.compilation_unit cu)
        {
            if (project != null && project.generate_xml_doc)
                return true;
            if (cu == null)
        		return false;
        	if (cu.file_name != null && internalDebug.DocumentedUnits.Contains(cu.file_name.ToLower()))
        		return true;
        	foreach (SyntaxTree.compiler_directive cd in cu.compiler_directives)
        		if (string.Compare(cd.Name.text, "gendoc",true)==0 && string.Compare(cd.Directive.text,"true",true)==0)
        		return true;
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
                        if (cd.Name.text.ToLower() == TreeConverter.compiler_string_consts.compiler_savepcu)
                            if (!Convert.ToBoolean(cd.Directive.text))
                                return;
                }
            }
            catch
            {
            }
//#endif
            PCUWriter writer=null;
            try
            {
#if DEBUG
                if (InternalDebug.PCUGenerate)
#endif
                if (compilerOptions.SavePCU)
                if ((Unit.SemanticTree as TreeRealization.common_unit_node).namespaces.Count > 1 && 
                    Unit.SyntaxTree!=null && 
                    Unit.State == UnitState.Compiled)
                {
                    writer = new PCUWriter(this, pr_ChangeState);
                    
                    bool dbginfo = true;/*CompilerOptions.Debug*/
#if DEBUG
                    dbginfo = InternalDebug.IncludeDebugInfoInPCU;
#endif
                    
                    writer.SaveSemanticTree(Unit, Path.ChangeExtension(Unit.UnitName, CompilerOptions.CompiledUnitExtension), dbginfo);
                    
                }
            }
            catch (Exception err)
            {
                //ErrorsList.Add(new Errors.CompilerInternalError(string.Format("Compiler.Compile[{0}]", Path.GetFileName(this.CurrentCompilationUnit.SyntaxTree.file_name)), err));
                OnChangeCompilerState(this, CompilerState.PCUWritingError, Unit.UnitName);
#if DEBUG
                if (!InternalDebug.SkipPCUErrors)
                    throw new Errors.CompilerInternalError(string.Format("Compiler.Compile[{0}]", Path.GetFileName(this.CurrentCompilationUnit.SyntaxTree.file_name)), err);
                writer.RemoveSelf();
#endif
            }
        }

		public CompilationUnit ReadPCU(string FileName)
		{
            PCUReader pr = new PCUReader(this,pr_ChangeState);
            return pr.GetCompilationUnit(FileName,CompilerOptions.Debug);
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
                    UnitsSortedList.Add(cu);
                    GetReferences(cu);
                    break;
                case PCUReaderWriterState.EndSaveTree:
                    OnChangeCompilerState(this, CompilerState.SavePCUFile, (Sender as PCUWriter).FileName);
                    break;

            }
        }
        /*                            TreeRealization.common_unit_node cun11 = CurrentUnit.SemanticTree as TreeRealization.common_unit_node;
                            if (cun11 != null)
                                UnitsSortedList.AddElement(cun11);*/

        static string standartAssemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(string)).ManifestModule.FullyQualifiedName);

        public static Dictionary<string, string> standart_assembly_dict = new Dictionary<string, string>();
        static Compiler()
        {
            string[] ss = new string[] { "mscorlib.dll","System.dll", "System.Core.dll", "System.Numerics.dll", "System.Windows.Forms.dll", "PABCRtl.dll" };
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
		
		

		public CompilationUnit ReadDLL(string FileName)
		{
            if (DLLCashe.ContainsKey(FileName))
                return DLLCashe[FileName];
            OnChangeCompilerState(this, CompilerState.ReadDLL, FileName);
            TreeRealization.using_namespace_list using_namespaces = new TreeRealization.using_namespace_list();
            try
            {
                TreeRealization.dot_net_unit_node un =
                    new TreeRealization.dot_net_unit_node(new NetHelper.NetScope(using_namespaces,
                    /*System.Reflection.Assembly.LoadFrom(FileName)*/
                    PascalABCCompiler.NetHelper.NetHelper.LoadAssembly(FileName), SyntaxTreeToSemanticTreeConverter.SymbolTable));
                CompilationUnit cu = new CompilationUnit();
                cu.SemanticTree = un;

                //un.dotNetScope=new PascalABCCompiler.NetHelper.NetScope(using_namespaces,
                //	System.Reflection.Assembly.LoadFrom(FileName),SyntaxTreeToSemanticTreeConverter.SymbolTable);
                DLLCashe[FileName] = cu;

                return cu;
            }
            catch (ReflectionTypeLoadException e)
            {
                Console.Error.WriteLine(e.Message);
                foreach (var eLoaderException in e.LoaderExceptions)
                {
                    Console.Error.WriteLine(eLoaderException.Message);
                }

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
            CurrentSyntaxUnit = new SyntaxTree.uses_unit_in(new SyntaxTree.string_const(program_folder + "\\" + unit_name+".pas"));
            CompileUnit(Units, CurrentSyntaxUnit);
            CompilationUnit cu = new CompilationUnit();
            if (Units.Count != 0)
                cu.SemanticTree = Units[Units.Count - 1];
            else
                return null;
            return cu;
        }*/
        public bool NeedRecompiled(string pcu_name, string[] included, PCUReader pr)
        {
            if (!Path.IsPathRooted(pcu_name)) throw new InvalidOperationException();
            string pas_name = FindSourceFileName(Path.ChangeExtension(pcu_name, null), null, out _);
            var dir = Path.GetDirectoryName(pcu_name);

            if (UnitTable[pas_name] != null)
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
                    if (UnitTable[used_unit_fname] != null) return true;
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

        public void ClearAll()
        {
            _semantic_tree = null;
            PCUReader.CloseUnits(); 
            PCUWriter.Clear();
            RecompileList.Clear();
            CycleUnits.Clear();
            UnitTable.Clear();
            UnitsSortedList.Clear();
            //TreeRealization.PCUReturner.Clear();
            BadNodesInSyntaxTree.Clear();
            PCUReader.AllReaders.Clear();
            project = null;
            StandarModules.Clear();
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
