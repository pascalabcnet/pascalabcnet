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

using ICSharpCode.NRefactory;
using Microsoft.Scripting;
//using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using PascalABCCompiler.Errors;
using PascalABCCompiler.PCU;
using PascalABCCompiler.SemanticTreeConverters;
//using SyntaxTreeChanger;
using PascalABCCompiler.SyntaxTreeConverters;
using PascalABCCompiler.TreeRealization;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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
            : base(string.Format(StringResources.Get("COMPILATIONERROR_READ_PCU{0}_ERROR"), FileName))
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
        public AssemblyNotFound(string FileName, string AssemblyFileName, SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_ASSEMBLY_{0}_NOT_FOUND"), AssemblyFileName), FileName)
        {
            this.AssemblyFileName = AssemblyFileName;
            this.source_context = sc;
        }

    }

    public class AssemblyReadingError : CompilerCompilationError
    {
        public string AssemblyFileName;
        public AssemblyReadingError(string FileName, string AssemblyFileName, SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_ASSEMBLY_{0}_READING_ERROR"), AssemblyFileName), FileName)
        {
            this.AssemblyFileName = AssemblyFileName;
            this.source_context = sc;
        }
    }

    public class InvalidAssemblyPathError : CompilerCompilationError
    {
        public InvalidAssemblyPathError(string FileName, SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_INVALID_ASSEMBLY_PATH")), FileName)
        {
            this.source_context = sc;
        }
    }

    public class InvalidPathError : CompilerCompilationError
    {
        public InvalidPathError(SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_INVALID_PATH")))
        {
            this.source_context = sc;
            this.fileName = sc.FileName;
        }
    }

    public class ResourceFileNotFound : CompilerCompilationError
    {
        public ResourceFileNotFound(string ResFileName, TreeRealization.location sl)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_RESOURCEFILE_{0}_NOT_FOUND"), ResFileName), sl.doc.file_name)
        {
            this.sourceLocation = new SourceLocation(sl.doc.file_name, sl.begin_line_num, sl.begin_column_num, sl.end_line_num, sl.end_column_num);
        }

        public ResourceFileNotFound(string ResFileName)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_RESOURCEFILE_{0}_NOT_FOUND"), ResFileName), null)
        {
            //this.sourceLocation = new SourceLocation(sl.doc.file_name, sl.begin_line_num, sl.begin_column_num, sl.end_line_num, sl.end_column_num);
        }
    }

    public class IncludeNamespaceInUnit : CompilerCompilationError
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

    public class NamespacesCanBeCompiledOnlyInProjects : CompilerCompilationError
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
            : base(string.Format(StringResources.Get("COMPILATIONERROR_SOURCE_FILE_{0}_NOT_FOUND"), FileName))
        {
        }
    }

    public class UnauthorizedAccessToFile : CompilerCompilationError
    {
        public UnauthorizedAccessToFile(string FileName)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_NO_ACCESS_TO_FILE{0}"), FileName))
        {
        }
    }
    public class CycleUnitReference : CompilerCompilationError
    {
        public SyntaxTree.unit_or_namespace SyntaxUsesUnit;
        public CycleUnitReference(string FileName, SyntaxTree.unit_or_namespace SyntaxUsesUnit)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_CYCLIC_UNIT_REFERENCE_WITH_UNIT_{0}"), SyntaxTree.Utils.IdentListToString(SyntaxUsesUnit.name.idents, ".")), FileName)
        {
            this.SyntaxUsesUnit = SyntaxUsesUnit;
            this.source_context = SyntaxUsesUnit.source_context;
        }
    }

    public class UnsupportedTargetFramework : CompilerCompilationError
    {
        public UnsupportedTargetFramework(string FrameworkName, TreeRealization.location sl)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_UNSUPPORTED_TARGETFRAMEWORK_{0}"), FrameworkName))
        {
            this.sourceLocation = new SourceLocation(sl.doc.file_name, sl.begin_line_num, sl.begin_column_num, sl.end_line_num, sl.end_column_num);
        }
    }

    public enum UnitState { BeginCompilation, InterfaceCompiled, Compiled }

    public class CompilationUnit
    {
        internal bool CaseSensitive = false;
        public string CurrentUsesUnitId;
        public PascalABCCompiler.Errors.SyntaxError syntax_error;
        public string UnitFileName;
        public List<Errors.Error> ErrorList = new List<Errors.Error>();
        public bool Documented;
        internal List<SyntaxTree.unit_or_namespace> possibleNamespaces = new List<PascalABCCompiler.SyntaxTree.unit_or_namespace>();
        //internal List<CompilationUnit> AssemblyReferences = new List<CompilationUnit>();

        //private SemanticTree.compilation_unitArrayList _interfaceUsedUnits=new SemanticTree.compilation_unitArrayList();

        /// <summary>
        /// Только "реальные" паскалевские юниты (не dll и namespace)
        /// </summary>
        public Dictionary<unit_node, CompilationUnit> DirectInterfaceCompilationUnits { get; } = new Dictionary<unit_node, CompilationUnit>();

        public PascalABCCompiler.TreeRealization.unit_node_list InterfaceUsedUnits { get; } = new PascalABCCompiler.TreeRealization.unit_node_list();
        /// <summary>
        /// Только "реальные" паскалевские юниты (не dll и namespace)
        /// </summary>
        public Dictionary<unit_node, CompilationUnit> DirectImplementationCompilationUnits { get; } = new Dictionary<unit_node, CompilationUnit>();
        public PascalABCCompiler.TreeRealization.unit_node_list ImplementationUsedUnits { get; } = new PascalABCCompiler.TreeRealization.unit_node_list();

        public bool ForEachDirectCompilationUnit(Func<CompilationUnit, string, bool> on_unit)
        {
            foreach (var kvp in DirectInterfaceCompilationUnits)
                if (!on_unit(kvp.Value, InterfaceUsedUnits.unit_uses_paths[kvp.Key]))
                    return false;
            foreach (var kvp in DirectImplementationCompilationUnits)
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

        private SyntaxTree.unit_or_namespace _syntaxUnitName = null;
        public SyntaxTree.unit_or_namespace SyntaxUnitName
        {
            get { return _syntaxUnitName; }
            set { _syntaxUnitName = value; }
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

        public UnitState State = UnitState.BeginCompilation;
    }

    public class CompilationUnitHashTable : Hashtable
    {
        public CompilationUnitHashTable()
            //DarkStar: fixed error in test_standartuses.pas at rebuild
            : base(StringComparer.InvariantCultureIgnoreCase)
        {
        }
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
        public bool SavePCUInThreadPull = false;
        public bool RunWithEnvironment = false;
        public string CompiledUnitExtension = ".pcu";
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

        //
        public string SystemDirectory;

        public List<string> ForceDefines = new List<string>();

        public List<string> SearchDirectory;

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

        public enum StandartModuleAddMethod { LeftToAll, RightToMain };
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
            StandartModule module_to_del = null;
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
            if (index < StandartModules.Count)
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
            StandartModules = new List<StandartModule>();
            StandartModules.Add(new StandartModule("PABCSystem", SyntaxTree.LanguageId.PascalABCNET | SyntaxTree.LanguageId.C));
            StandartModules.Add(new StandartModule("PABCExtensions", SyntaxTree.LanguageId.PascalABCNET | SyntaxTree.LanguageId.C));
        }

        private void SetDirectories()
        {
            SystemDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);

            StandartDirectories = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
            StandartDirectories.Add("%PABCSYSTEM%", SystemDirectory);

            ParserSearchPatchs = new string[] { Path.Combine(SystemDirectory, "Lib") };
            SearchDirectory = ParserSearchPatchs.ToList();
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
        public bool AlwaysGenerateXMLDoc = false;
        public bool SkipInternalErrorsIfSyntaxTreeIsCorrupt = true;
        public bool UseStandarParserForIntellisense = true;
        public bool RunOnMono = false;
        public List<string> DocumentedUnits = new List<string>();

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

        public program_node semantic_tree
        {
            get
            {
                return _semantic_tree;
            }
        }

        public List<var_definition_node> CompiledVariables = new List<var_definition_node>();

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
            supportedProjectFilesList.Add(new SupportedSourceFile(new string[1] { ".pabcproj" }, "PascalABC.NET"));
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

        private CompilationUnitHashTable unitTable = new CompilationUnitHashTable();
        public CompilationUnitHashTable UnitTable { get { return unitTable; } }
        public List<CompilationUnit> UnitsLogicallySortedList = new List<CompilationUnit>();

        private List<string> StandardModules = new List<string>();
        public CompilerOptions CompilerOptions { get; set; } = new CompilerOptions();

        internal Dictionary<string, CompilationUnit> DLLCache = new Dictionary<string, CompilationUnit>();

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
        public SyntaxTree.unit_or_namespace usesSection;
        /// <summary>
        /// список отложенной компиляции реализации (она будет откомпилирована в GenerateILCodeAndSaveAssembly, а не в СompileUnit)
        /// </summary>
		private List<CompilationUnit> UnitsToCompileDelayedList = new List<CompilationUnit>();
        public Hashtable RecompileList = new Hashtable(StringComparer.OrdinalIgnoreCase);
        private Hashtable CycleUnits = new Hashtable();
        public CompilationUnit currentCompilationUnit = null;
        private CompilationUnit firstCompilationUnit = null;
        private bool PCUReadersAndWritersClosed;
        /// <summary>
        /// Начало основной программы
        /// </summary>
        public int beginOffset;
        /// <summary>
        /// Положение первых переменных в пространстве имен основной программы
        /// </summary>
        public int varBeginOffset;
        private bool _clear_after_compilation = true;
        private static Dictionary<string, CompilationUnit> pcuCompilationUnits = new Dictionary<string, CompilationUnit>();

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

        private Dictionary<string, List<compiler_directive>> GetCompilerDirectives(List<CompilationUnit> Units)
        {
            Dictionary<string, List<compiler_directive>> Directives = new Dictionary<string, List<compiler_directive>>(StringComparer.CurrentCultureIgnoreCase);

            for (int i = 0; i < Units.Count; i++)
            {
                common_unit_node cun = Units[i].SemanticTree as common_unit_node;
                if (cun != null)
                    foreach (compiler_directive cd in cun.compiler_directives)
                    {
                        if (!Directives.ContainsKey(cd.name))
                            Directives.Add(cd.name, new List<compiler_directive>());
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
                list.Add(new TreeRealization.compiler_directive(sncd.Name.text, sncd.Directive != null ? sncd.Directive.text : "", get_location_from_treenode(sncd, cu.file_name), cu.file_name));
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
            ParsersController.Reset();
            SyntaxTreeToSemanticTreeConverter.Reset();
            CodeGeneratorsController.Reset();
            //PABCToCppCodeGeneratorsController.Reset();
            UnitsToCompileDelayedList.Clear();
            DLLCache.Clear();
            project = null;
        }

        void CheckErrorsAndThrowTheFirstOne()
        {
            if (CompilerOptions.ForIntellisense)
                return;
            if (ErrorsList.Count > 0)
                throw ErrorsList[0];
        }

        private void MoveSystemUnitForwardInUnitLogicallySortedList()
        {
            if (CompilerOptions.StandartModules.Count == 0)
                return;

            CompilationUnit system_unit = null;
            foreach (CompilationUnit unit in UnitsLogicallySortedList)
            {
                if (unit.SemanticTree != null && unit.SemanticTree is common_unit_node)
                {
                    if ((unit.SemanticTree as common_unit_node).unit_name == CompilerOptions.StandartModules[0].Name)
                    {
                        system_unit = unit;
                        break;
                    }
                }
            }

            if (system_unit != null && system_unit != UnitsLogicallySortedList[0])
            {
                UnitsLogicallySortedList.Remove(system_unit);
                UnitsLogicallySortedList.Insert(0, system_unit);
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
            public int using_pos = -1;
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
                        info.addit_imports.Add(name + "." + name);
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
                    string source_file = FindFileInDirs(s + ".vb", out _, Path.GetDirectoryName(CompilerOptions.SourceFileName), Path.Combine(this.CompilerOptions.SystemDirectory, "lib"),
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
        }

        public string CompileWithProvider(string[] sources, System.CodeDom.Compiler.CodeDomProvider cp, params string[] RefAssemblies)
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
            public PyErrorHandler(Compiler cc) { c = cc; }
            public override void ErrorReported(ScriptSource source, string message, SourceSpan span, int errorCode, Severity severity)
            {
                if (severity == Severity.Warning)
                    c.warnings.Add(new Errors.CommonWarning(message, c.CompilerOptions.SourceFileName, span.Start.Line, span.Start.Column));
                else
                    c.errorsList.Add(new Errors.CommonCompilerError(message, c.CompilerOptions.SourceFileName, span.Start.Line, span.Start.Column));
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
            string redirect_base_fname = FindFileInDirs("__RedirectIOMode.vb", out _, Path.Combine(this.CompilerOptions.SystemDirectory, "Lib"), Path.Combine(this.CompilerOptions.SystemDirectory, "LibSource"));
            string system_unit_name = FindFileInDirs("VBSystem.vb", out _, Path.Combine(this.CompilerOptions.SystemDirectory, "lib"), Path.Combine(this.CompilerOptions.SystemDirectory, "LibSource"));
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
                sw.WriteLine("Module _RedirectIOMode");
                sw.WriteLine("End Module");
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
                comp_opt.ReferencedAssemblies.Add(Path.Combine(Path.GetDirectoryName(CompilerOptions.SourceFileName), PascalABCCompiler.TreeConverter.compiler_string_consts.pabc_rtl_dll_name));
                string mod_file_name = FindFileInDirs("PABCRtl.dll", out _, Path.Combine(this.CompilerOptions.SystemDirectory, "Lib"));
                File.Copy(mod_file_name, Path.Combine(Path.GetDirectoryName(CompilerOptions.SourceFileName), "PABCRtl.dll"), true);
                /*foreach (string mod in info.modules)
				{
					comp_opt.ReferencedAssemblies.Add(Path.Combine(Path.GetDirectoryName(CompilerOptions.SourceFileName),mod+".dll"));
					string mod_file_name = FindSourceFileInDirectories(mod+".mod",Path.Combine(this.CompilerOptions.SystemDirectory,"lib"));
					File.Copy(mod_file_name,Path.Combine(Path.GetDirectoryName(CompilerOptions.SourceFileName),mod+".dll"),true);
				}*/
            }
            sources.Add(redirect_fname);
            sources.Add(system_unit_name);
            System.CodeDom.Compiler.CompilerResults res = vbcp.CompileAssemblyFromFile(comp_opt, sources.ToArray());
            if (res.Errors.Count > 0)
            {
                for (int i = 0; i < res.Errors.Count; i++)
                {
                    if (!res.Errors[i].IsWarning && errorsList.Count == 0 /*&& res.Errors[i].FileName != redirect_fname*/)
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
                                CompileUnit(CurrentUnit.ImplementationUsedUnits, CurrentUnit.DirectImplementationCompilationUnits, implementationUsesList[i], Path.GetDirectoryName(unitFileName));
                            }
                            else
                            {
                                // добавление в списки только пространств имен
                                CurrentUnit.ImplementationUsedUnits.AddElement(new namespace_unit_node(GetNamespace(implementationUsesList[i])), null);
                                CurrentUnit.possibleNamespaces.Add(implementationUsesList[i]);
                            }
                        }

                    }
                    
                    //TODO: Избавиться от преобразования типа.
                    AddNamespaces(CurrentUnit.ImplementationUsingNamespaceList, CurrentUnit.possibleNamespaces, true, null);

                    CompileCurrentUnitImplementation(unitFileName, CurrentUnit, null);

                    CurrentUnit.State = UnitState.Compiled; // отметка о скомпилированности
                    OnChangeCompilerState(this, CompilerState.EndCompileFile, unitFileName); // состояние конец компиляции
                    //SavePCU(currentUnit, unitFileName);
                    CurrentUnit.UnitFileName = unitFileName;
                }
            }  
        }

        private void SetOutputFileType()
        {
            if (compilerDirectives.ContainsKey(TreeConverter.compiler_string_consts.compiler_directive_apptype))
            {
                string directive = compilerDirectives[TreeConverter.compiler_string_consts.compiler_directive_apptype][0].directive.ToLower();
                switch (directive)
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
                        throw new Exception("No possible OutputFileType!");
                }
            }
        }

        private void SetOutputPlatform(NETGenerator.CompilerOptions compilerOptions)
        {
            List<compiler_directive> compilerDirectivesList = new List<compiler_directive>();
            if (compilerDirectives.TryGetValue(TreeConverter.compiler_string_consts.compiler_directive_platformtarget, out compilerDirectivesList))
            {
                string plt = compilerDirectivesList[0].directive.ToLower();
                switch (plt)
                {
                    case "x86":
                        compilerOptions.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.x86;
                        break;
                    case "x64":
                        compilerOptions.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.x64;
                        break;
                    case "anycpu":
                        compilerOptions.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.AnyCPU;
                        break;
                    case "dotnet5win":
                        compilerOptions.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.dotnet5win;
                        break;
                    case "dotnet5linux":
                        compilerOptions.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.dotnet5linux;
                        break;
                    case "dotnet5macos":
                        compilerOptions.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.dotnet5macos;
                        break;
                    case "native":
                        if (Environment.OSVersion.Platform == PlatformID.Unix)
                        {
                            compilerOptions.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.dotnetlinuxnative;
                        }
                        else if (Environment.OSVersion.Platform == PlatformID.MacOSX)
                        {
                            compilerOptions.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.dotnetmacosnative;
                        }
                        else
                        {
                            compilerOptions.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.dotnetwinnative;
                        }
                        break;
                    default:
                        throw new Exception("Unknown platform!");
                }
                if (CompilerOptions.Only32Bit)
                    compilerOptions.platformtarget = NETGenerator.CompilerOptions.PlatformTarget.x86;

                // целевой framework
                if (this.compilerDirectives.TryGetValue(TreeConverter.compiler_string_consts.compiler_directive_targetframework, out compilerDirectivesList))
                {
                    compilerOptions.TargetFramework = compilerDirectivesList[0].directive;
                    if (!(new string[] { "net40", "net403", "net45", "net451", "net452", "net46", "net461", "net462", "net47", "net471", "net472", "net48", "net481" })
                        .Contains(compilerOptions.TargetFramework))
                        ErrorsList.Add(new UnsupportedTargetFramework(compilerOptions.TargetFramework, compilerDirectivesList[0].location));
                }
            }
        }

        private void FillOtherCompilerOptions(NETGenerator.CompilerOptions compilerOptions)
        {
            var compilerDirectives = new List<compiler_directive>();

            if (this.compilerDirectives.TryGetValue(TreeConverter.compiler_string_consts.product_string, out compilerDirectives))
            {
                compilerOptions.Product = compilerDirectives[0].directive;
            }
            if (this.compilerDirectives.TryGetValue(TreeConverter.compiler_string_consts.version_string, out compilerDirectives))
            {
                compilerOptions.ProductVersion = compilerDirectives[0].directive;
            }
            if (this.compilerDirectives.TryGetValue(TreeConverter.compiler_string_consts.company_string, out compilerDirectives))
            {
                compilerOptions.Company = compilerDirectives[0].directive;
            }
            if (this.compilerDirectives.TryGetValue(TreeConverter.compiler_string_consts.trademark_string, out compilerDirectives))
            {
                compilerOptions.TradeMark = compilerDirectives[0].directive;
            }
            if (this.compilerDirectives.TryGetValue(TreeConverter.compiler_string_consts.copyright_string, out compilerDirectives))
            {
                compilerOptions.Copyright = compilerDirectives[0].directive;
            }
            if (this.compilerDirectives.TryGetValue(TreeConverter.compiler_string_consts.title_string, out compilerDirectives))
            {
                compilerOptions.Title = compilerDirectives[0].directive;
            }
            if (this.compilerDirectives.TryGetValue(TreeConverter.compiler_string_consts.description_string, out compilerDirectives))
            {
                compilerOptions.Description = compilerDirectives[0].directive;
            }
            if (this.compilerDirectives.TryGetValue(TreeConverter.compiler_string_consts.main_resource_string, out compilerDirectives))
            {
                if (this.compilerDirectives.ContainsKey(TreeConverter.compiler_string_consts.product_string) ||
                    this.compilerDirectives.ContainsKey(TreeConverter.compiler_string_consts.version_string) ||
                    this.compilerDirectives.ContainsKey(TreeConverter.compiler_string_consts.company_string) ||
                    this.compilerDirectives.ContainsKey(TreeConverter.compiler_string_consts.trademark_string) ||
                    this.compilerDirectives.ContainsKey(TreeConverter.compiler_string_consts.title_string) ||
                    this.compilerDirectives.ContainsKey(TreeConverter.compiler_string_consts.description_string) ||
                    this.compilerDirectives.ContainsKey(TreeConverter.compiler_string_consts.copyright_string))
                {
                    ErrorsList.Add(new MainResourceNotAllowed(compilerDirectives[0].location));
                }
                TryThrowInvalidPath(compilerDirectives[0].directive, compilerDirectives[0].location);
                // Тут не обязательно нормализовывать путь
                // И если он слишком длинный - File.Exists вернёт false
                compilerOptions.MainResourceFileName = Path.Combine(Path.GetDirectoryName(compilerDirectives[0].source_file), compilerDirectives[0].directive);
                if (!File.Exists(compilerOptions.MainResourceFileName))
                    ErrorsList.Add(new ResourceFileNotFound(compilerDirectives[0].directive, compilerDirectives[0].location));
            }

        }

        private void FillCompilerOptionsFromProject(NETGenerator.CompilerOptions compilerOptions)
        {
            if (project != null)
            {
                if (!(project.major_version == 0 && project.minor_version == 0 && project.build_version == 0 && project.revision_version == 0))
                    compilerOptions.ProductVersion = project.major_version + "." + project.minor_version + "." + project.build_version + "." + project.revision_version;
                if (!string.IsNullOrEmpty(project.product))
                    compilerOptions.Product = project.product;
                if (!string.IsNullOrEmpty(project.company))
                    compilerOptions.Company = project.company;
                if (!string.IsNullOrEmpty(project.trademark))
                    compilerOptions.TradeMark = project.trademark;
                if (!string.IsNullOrEmpty(project.copyright))
                    compilerOptions.Copyright = project.copyright;
                if (!string.IsNullOrEmpty(project.title))
                    compilerOptions.Title = project.title;
                if (!string.IsNullOrEmpty(project.description))
                    compilerOptions.Description = project.description;
                // TODO: разобраться, что за условие
                if (!string.IsNullOrEmpty(project.app_icon) && false) // ???
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
                        /*sw.WriteLine("FILEFLAGSMASK VS_FFI_FILEFLAGSMASK");
                        sw.WriteLine("FILEFLAGS VER_DEBUG");
                        sw.WriteLine("FILEOS VOS__WINDOWS32");
                        if (project.project_type != ProjectType.Library)
                            sw.WriteLine("FILETYPE VFT_APP");
                        else
                            sw.WriteLine("FILETYPE VFT_DLL");
                        sw.WriteLine("FILESUBTYPE VFT2_UNKNOWN");*/
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
            }
        }

        private void SetTargetType(NETGenerator.CompilerOptions compilerOptions)
        {
            compilerOptions.ForRunningWithEnvironment = CompilerOptions.RunWithEnvironment;
            // тип выходного файла
            switch (CompilerOptions.OutputFileType)
            {
                case CompilerOptions.OutputType.ClassLibrary: compilerOptions.target = NETGenerator.TargetType.Dll; break;
                case CompilerOptions.OutputType.ConsoleApplicaton: compilerOptions.target = NETGenerator.TargetType.Exe; break;
                case CompilerOptions.OutputType.WindowsApplication: compilerOptions.target = NETGenerator.TargetType.WinExe; break;
            }
            // TODO: вынести связанное с project отдельно (или хотя бы отметить это отдельно)
            if (project != null && project.ProjectType == ProjectType.WindowsApp)
                compilerOptions.target = PascalABCCompiler.NETGenerator.TargetType.WinExe;
            // Debug / Release
            switch (CompilerOptions.Debug)
            {
                case true: compilerOptions.dbg_attrs = NETGenerator.DebugAttributes.Debug; break;
                case false: compilerOptions.dbg_attrs = NETGenerator.DebugAttributes.Release; break;
            }
            if (CompilerOptions.ForDebugging)
                compilerOptions.dbg_attrs = NETGenerator.DebugAttributes.ForDebugging;
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
                // событие имеет много потенциальных обработчиков
                OnChangeCompilerState(this, CompilerState.CompilationStarting, CompilerOptions.SourceFileName);

                // очистка всех переменных и списков, используемых в процессе
                Reset();

                // если проект скомпилирован, то заполнение информации о проекте в опциях компилятора
                if (CompilerOptions.ProjectCompiled)
                {
                    PrepareCompileOptionsForProject();
                }
                
                // модули и пространства имен из секции uses - возможно стоит перенести это в CompileUnit (только аккуратно)   EVA
                usesSection = new SyntaxTree.uses_unit_in(null, new SyntaxTree.string_const(Path.GetFullPath(CompilerOptions.SourceFileName)));

                // компиляция всех юнитов произойдет рекурсивно (кроме отложенных)
                CompileUnit(
                    new unit_node_list(),
                    new Dictionary<unit_node, CompilationUnit>(),
                    usesSection, null);

                // компиляция юнитов из списка отложенной компиляции, если он не пуст
                CompileUnitsFromDelayedList();

                // --------------------- далее завершение компиляции (или перекомпиляция)

                // Закрытие чтения и записи .pcu файлов
                ClosePCUReadersAndWriters();

                if (CompilerOptions.SaveDocumentation)
                {
                    SaveDocumentationsForUnits();
                }

                compilerDirectives = GetCompilerDirectives(UnitsLogicallySortedList);

                // выяснение типа выходного файла по соотв. директиве компилятора
                SetOutputFileType();

                // перемещаем PABCSystem в начало списка
                MoveSystemUnitForwardInUnitLogicallySortedList();

                // передача информации о типе выходного файла системному юниту
                if (UnitsLogicallySortedList.Count > 0)
                {
                    bool isConsoleApplication = CompilerOptions.OutputFileType == CompilerOptions.OutputType.ConsoleApplicaton;
                    common_unit_node systemUnit = UnitsLogicallySortedList[0].SemanticTree as common_unit_node;
                    systemUnit.IsConsoleApplicationVariable = isConsoleApplication;
                }

                NETGenerator.CompilerOptions compilerOptions = new NETGenerator.CompilerOptions();

                // выяснение целевой платформы
                SetOutputPlatform(compilerOptions);

                // остальные директивы
                FillOtherCompilerOptions(compilerOptions);

                // заполнение опций компилятора из заголовка проекта
                FillCompilerOptionsFromProject(compilerOptions);

                if (ErrorsList.Count == 0)
                {
                    // Устанавливает опции компилятора, связанные с типом выходного файла
                    SetTargetType(compilerOptions);

                    //TODO: Разобратся c location для program_node и правильно передавать main_function. Добавить генератор main_function в SyntaxTreeToSemanticTreeConverter.
                    program_node programRoot = BuildFullSemanticTree(compilerOptions); // получние полного семантического дерева, включающего все зависимости

                    if (firstCompilationUnit.SyntaxTree is SyntaxTree.unit_module && CompilerOptions.OutputFileType != CompilerOptions.OutputType.ClassLibrary)
                    {
                        // если мы комилируем PCU
                        CompilerOptions.OutputFileType = CompilerOptions.OutputType.PascalCompiledUnit;
                    }
                    // генерация IL кода
                    else if (CompilerOptions.GenerateCode)
                    {
                        if (CompilerOptions.UseDllForSystemUnits)
                            compilerOptions.RtlPABCSystemType = NetHelper.NetHelper.FindRtlType("PABCSystem.PABCSystem");

                        GenerateILCode(programRoot, compilerOptions);
                    }
                }
            }
            // TODO: просмотреть возможные ParserError   EVA
            catch (TreeConverter.ParserError err)
            {
                //конвертор уткнулся в ошибку. ничего не делаем
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
                AddCodeGenerationErrorToErrorsList(err);
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
                ErrorsList.Add(new CompilerInternalError("Compiler.ClosePCUReadersAndWriters", e));
            }

            OnChangeCompilerState(this, CompilerState.CompilationFinished, CompilerOptions.SourceFileName); // compilation finished
            if (ClearAfterCompilation)
                ClearAll();

            // на случай ошибки в самом .pcu формате / здесь могут быть не только pcu errors   EVA
            bool recompilationNeeded = CheckForInternalErrors();

            if (!recompilationNeeded)
                OnChangeCompilerState(this, CompilerState.Ready, null); // компилятор окончательно завершил работу

            if (ErrorsList.Count > 0)
            {
                return null;
            }
            else if (recompilationNeeded)
            {
                //Compiler c = new Compiler(sourceFilesProvider,OnChangeCompilerState);
                //return c.GenerateILCodeAndSaveAssembly(this.compilerOptions);
                return Compile();
            }
            else return CompilerOptions.OutputFileName;
        }

        private program_node BuildFullSemanticTree(NETGenerator.CompilerOptions compilerOptions)
        {
            program_node programRoot = new program_node(null, null);
            
            for (int i = 0; i < UnitsLogicallySortedList.Count; i++)
                programRoot.units.AddElement(UnitsLogicallySortedList[i].SemanticTree as common_unit_node);

            CreateMainFunction(programRoot, compilerOptions); // планируется вынесение в другой класс, поэтому рефакторинг функции не проведен  EVA

            if (CompilerOptions.GenerateCode) // семантические преобразования с оптимизацией кода
                programRoot = semanticTreeConvertersController.Convert(programRoot) as program_node;

            _semantic_tree = programRoot;
            return programRoot;
        }

        private void CreateMainFunction(program_node programNode, NETGenerator.CompilerOptions compilerOptions)
        {
            // если компилируем exe или WinExe (первый модуль - основная программа)
            if (firstCompilationUnit.SyntaxTree is SyntaxTree.program_module)
            {
                if ((compilerOptions.target == NETGenerator.TargetType.Exe) || (compilerOptions.target == NETGenerator.TargetType.WinExe))
                {
                    if (UnitsLogicallySortedList.Count > 0)
                    {
                        programNode.main_function = ((common_unit_node)UnitsLogicallySortedList[UnitsLogicallySortedList.Count - 1].SemanticTree).main_function;

                        /***************************Ivan added*******************************/
                        if (programNode.main_function.function_code.location != null)
                        {
                            bool flag = false;
                            common_namespace_node main_ns = programNode.main_function.namespace_node;
                            for (int i = 0; i < main_ns.variables.Count; i++)
                            {
                                namespace_variable nv = main_ns.variables[i];
                                if (nv.inital_value != null && nv.inital_value.location != null && !(nv.inital_value is constant_node)
                                    && !(nv.inital_value is record_initializer) && !(nv.inital_value is array_initializer))
                                {
                                    varBeginOffset = main_ns.variables[i].inital_value.location.begin_line_num;
                                    flag = true;
                                    break;
                                }
                            }
                            beginOffset = programNode.main_function.function_code.location.begin_line_num;
                        }
                        /*******************************************************************/
                        // локализация
                        Dictionary<string, object> config_dic = new Dictionary<string, object>();
                        if (CompilerOptions.Locale != null && PascalABCCompiler.StringResourcesLanguage.GetLCIDByTwoLetterISO(CompilerOptions.Locale) != null)
                        {
                            config_dic["locale"] = CompilerOptions.Locale;
                            config_dic["full_locale"] = PascalABCCompiler.StringResourcesLanguage.GetLCIDByTwoLetterISO(CompilerOptions.Locale);
                        }
                        programNode.create_main_function(StandardModules.ToArray(), config_dic);

                    }
                }
            }
            // если мы компилируем dll
            else if (firstCompilationUnit.SyntaxTree is SyntaxTree.unit_module && compilerOptions.target == NETGenerator.TargetType.Dll)
            {
                // TODO: посмотреть инициализирующий код .dll  EVA
                programNode.create_main_function_as_in_module();
            }
        }

        private void AddErrorToErrorListConsideringPosition(Error err)
        {
            if (ErrorsList.Count == 0)
                ErrorsList.Add(err);
            else if (err != ErrorsList[0])
            {
                // TODO: переделать insert в ErrorsList
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

        private void AddCodeGenerationErrorToErrorsList(Exception err)
        {
            string fileName = Path.GetFileName(currentCompilationUnit?.SyntaxTree?.file_name) ?? "Compiler";
            CompilerInternalError compilationError = new CompilerInternalError(string.Format("Compiler.GenerateILCodeAndSaveAssembly[{0}]", fileName), err);
            
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

        // TODO: перепроверить этот метод, возможно, он может найти не те ошибки и перекомпиляция ничего не даст   EVA
        private bool CheckForInternalErrors()
        {
            bool anyExternalErrors = false;
            
            if (ErrorsList.Count > 0)
            {
                // rtl - та самая dll со всей паскалевской системой (PABCSystem)
                if (CompilerOptions.UseDllForSystemUnits && !HasOnlySyntaxErrors(ErrorsList) && CompilerOptions.IgnoreRtlErrors)
                {
                    CompilerOptions.UseDllForSystemUnits = false;
                    ErrorsList.Clear();

                    anyExternalErrors = true;

                }
            }

            return anyExternalErrors;
        }

        private void GenerateILCode(program_node programNode, NETGenerator.CompilerOptions compilerOptions)
        {
            
            if (CompilerOptions.OutputFileType != CompilerOptions.OutputType.SemanticTree)
#if DEBUG
                if (InternalDebug.CodeGeneration)
#endif
                {
                    // генерация файла .pdb для дебага
                    DebugOutputFileCreationUsingPDB();

                    OnChangeCompilerState(this, CompilerState.CodeGeneration, CompilerOptions.OutputFileName); // состояние генерации кода

                    // трансляция в IL-код | В programRoot находится ЕДИНСТВЕННОЕ семантическое дерево, содержащее программу и все семантические модули
                    CodeGeneratorsController.GenerateILCodeAndSaveAssembly(programNode, CompilerOptions.OutputFileName,
                        CompilerOptions.SourceFileName, compilerOptions, CompilerOptions.StandartDirectories,
                        GetResourceFilesFromCompilerDirectives()?.ToArray());

                    CodeGeneratorsController.EmitAssemblyRedirects(
                        assemblyResolveScope,
                        CompilerOptions.OutputFileName);

                    if (compilerOptions.MainResourceFileName != null)
                        File.Delete(compilerOptions.MainResourceFileName);
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

        private List<string> GetResourceFilesFromCompilerDirectives()
        {
            List<string> ResourceFiles = null;
            if (compilerDirectives.ContainsKey(TreeConverter.compiler_string_consts.compiler_directive_resource))
            {
                ResourceFiles = new List<string>();
                List<compiler_directive> ResourceDirectives = compilerDirectives[TreeConverter.compiler_string_consts.compiler_directive_resource];
                
                foreach (compiler_directive cd in ResourceDirectives)
                {
                    TryThrowInvalidPath(cd.directive, cd.location);
                    var resourceFileName = Path.Combine(Path.GetDirectoryName(cd.source_file), cd.directive);

                    // Так же как с main_resource
                    if (File.Exists(resourceFileName))
                        ResourceFiles.Add(resourceFileName);
                    else
                        ErrorsList.Add(new ResourceFileNotFound(cd.directive, cd.location));

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
            foreach (CompilationUnit cu in UnitsLogicallySortedList)
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
            foreach (CompilationUnit cu in UnitsLogicallySortedList)
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
        // Возвращает список зависимостей из интерфейсной части currentSyntaxUnit
        public List<SyntaxTree.unit_or_namespace> GetInterfaceUsesSection(SyntaxTree.compilation_unit currentSyntaxUnit)
        {
            List<SyntaxTree.unit_or_namespace> result = null;
            if (currentSyntaxUnit is SyntaxTree.unit_module)
            {
                SyntaxTree.interface_node intp = (currentSyntaxUnit as SyntaxTree.unit_module).interface_part;
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
            if (currentSyntaxUnit is SyntaxTree.program_module)
            {
                SyntaxTree.program_module pm = (currentSyntaxUnit as SyntaxTree.program_module);
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
            CheckForDuplicatesInUsesSection(result);
            return result;
        }

        private List<SyntaxTree.unit_or_namespace> GetImplementationUsesSection(SyntaxTree.compilation_unit currentSyntaxUnit)
        {

            List<SyntaxTree.unit_or_namespace> usesSection = (currentSyntaxUnit as SyntaxTree.unit_module)?.implementation_part?.uses_modules?.units;

            CheckForDuplicatesInUsesSection(usesSection);

            return usesSection;

            /*List<SyntaxTree.unit_or_namespace> result = null;
            if (currentSyntaxUnit is SyntaxTree.unit_module)
				if ((currentSyntaxUnit as SyntaxTree.unit_module).implementation_part != null)
                    if ((currentSyntaxUnit as SyntaxTree.unit_module).implementation_part.uses_modules != null)
                    {
                        result = (currentSyntaxUnit as SyntaxTree.unit_module).implementation_part.uses_modules.units;
                        checkDuplicateUsesUnit(result);
                        return result;
                    }
			if(currentSyntaxUnit is SyntaxTree.program_module)
				return null;
			return null;*/
        }

        public string FindPCUFileName(string fname, string curr_path, out int folder_priority)
        {
            if (string.IsNullOrEmpty(Path.GetExtension(fname)))
                fname += CompilerOptions.CompiledUnitExtension;
            var cache_key = Tuple.Create(fname.ToLower(), curr_path?.ToLower());

            if (!PCUFileNamesDictionary.TryGetValue(cache_key, out var res))
            {

                if (FindFileInDirs(fname, out _, curr_path) is string res_s1)
                    res = Tuple.Create(res_s1, 1);
                else if (FindFileInDirs(Path.GetFileName(fname), out _, CompilerOptions.OutputDirectory) is string res_s2)
                    res = Tuple.Create(res_s2, 2);
                else if (FindFileInDirs(fname, out var dir_ind, CompilerOptions.SearchDirectory.ToArray()) is string res_s3)
                    res = Tuple.Create(res_s3, 3 + dir_ind);
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

                if (FindSourceFileNameInDirs(fname, out _, curr_path) is string res_s1)
                    res = Tuple.Create(res_s1, 1);
                else if (FindSourceFileNameInDirs(fname, out var dir_ind, CompilerOptions.SearchDirectory.ToArray()) is string res_s3)
                    res = Tuple.Create(res_s3, 3 + dir_ind);
                else
                    res = null;

                SourceFileNamesDictionary[cache_key] = res;
            }

            folder_priority = res == null ? 0 : res.Item2;
            return res?.Item1;
        }

        public string FindSourceFileNameInDirs(string fname, out int found_dir_ind, params string[] Dirs)
        {
            var fname_ext = Path.GetExtension(fname);
            var need_ext = string.IsNullOrEmpty(fname_ext);

            foreach (SupportedSourceFile sf in SupportedSourceFiles)
                foreach (string ext in sf.Extensions)
                    if (need_ext || fname_ext == ext)
                    {
                        var res = FindFileInDirs(need_ext ? fname + ext : fname, out found_dir_ind, Dirs);
                        //if (!(CompilerOptions.UseDllForSystemUnits && Path.GetDirectoryName(res) == CompilerOptions.SearchDirectory))
                        if (res != null)
                            return res;
                    }

            found_dir_ind = 0;
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

        private string FindFileInDirs(string FileName, out int found_dir_ind, params string[] Dirs)
        {
            if (Path.IsPathRooted(FileName))
            {
                found_dir_ind = 0;
                return File.Exists(FileName) ? FileName : null;
            }

            for (int dir_i = 0; dir_i < Dirs.Length; ++dir_i)
                try
                {
                    var Dir = Dirs[dir_i];
                    var res = Path.Combine(Dir, FileName);
                    if (File.Exists(res))
                    {
                        found_dir_ind = dir_i;
                        // Path.GetFullPath чтобы нормализовать
                        // File.Exists не может кинуть исключение или дать true
                        // если путь слишком длинный или содержит нерпавильные знаки
                        return Path.GetFullPath(res);
                    }
                }
                catch (PathTooLongException)
                {
                    continue;
                }

            found_dir_ind = 0;
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
            if (FileName == TreeConverter.compiler_string_consts.pabc_rtl_dll_name)
            {

                string name = get_assembly_path(FileName, true);
                if ((name != null) && (File.Exists(name)))
                    return name;

            }
            //\MikhailoMMX

            var FullFileName = Path.Combine(curr_path, FileName);
            if (System.IO.File.Exists(FullFileName))
            {
                var NewFileName = Path.Combine(CompilerOptions.OutputDirectory, Path.GetFileName(FullFileName));
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
                    throw new AssemblyNotFound(this.currentCompilationUnit.SyntaxTree.file_name, FileName, sc);
                else
                    if (File.Exists(name))
                    return name;
                else
                    throw new AssemblyNotFound(this.currentCompilationUnit.SyntaxTree.file_name, FileName, sc);
            }
        }

        public string GetUnitFileName(SyntaxTree.unit_or_namespace SyntaxUsesUnit, string curr_path)
        {
            //ToDo В корневом GenerateILCodeAndSaveAssembly() создаётся uses_unit_in без name. Выглядит как костыль
            if (SyntaxUsesUnit is SyntaxTree.uses_unit_in && (SyntaxUsesUnit as SyntaxTree.uses_unit_in).name == null) return (SyntaxUsesUnit as SyntaxTree.uses_unit_in).in_file.Value;
            if (curr_path == null) throw new InvalidOperationException(SyntaxUsesUnit.UsesPath());
            var UnitName = SyntaxUsesUnit.name.idents[0].name;

            if (SyntaxUsesUnit is SyntaxTree.uses_unit_in uui)
            {

                TryThrowInvalidPath(uui.in_file.Value, uui.in_file.source_context);

                if (UnitName.ToLower() != Path.GetFileNameWithoutExtension(uui.in_file.Value).ToLower())
                    throw new UsesInWrongName(SyntaxUsesUnit.source_context.FileName, UnitName, Path.GetFileNameWithoutExtension(uui.in_file.Value), uui.in_file.source_context);

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
            var PCUFileExists = (!CompilerOptions.Rebuild || !SourceFileExists) && PCUFileName != null;

            if (!PCUFileExists && !SourceFileExists)
                if (UnitName == null)
                    // вызов с "unitFileName == null" должен быть только там, где уже известно что хотя бы какой то файл есть
                    // если где то ещё будет исопльзоваться unitFileName или source_context - надо будет добавить такую же проверку
                    throw new InvalidOperationException(nameof(UnitName));
                else
                    throw new UnitNotFound(source_context.FileName, UnitName, source_context);

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

        public void AddStandartUnitsToUsesSection(SyntaxTree.compilation_unit cu)
        {
            //if (FileInSearchDirectory(cu.file_name)) return;

            string ModuleName = null;
            SyntaxTree.uses_unit_in uses_unit_in = null;
            SyntaxTree.unit_or_namespace uses_unit = null;
            List<SyntaxTree.unit_or_namespace> UsesList = GetInterfaceUsesSection(cu);
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
                if ((Module.AddToLanguages & firstCompilationUnit.SyntaxTree.Language) != firstCompilationUnit.SyntaxTree.Language
                    && (Module.AddToLanguages & currentCompilationUnit.SyntaxTree.Language) != currentCompilationUnit.SyntaxTree.Language)
                    continue;
                ModuleName = Path.GetFileNameWithoutExtension(Module.Name);
                if (Module.AddMethod == CompilerOptions.StandartModuleAddMethod.RightToMain && currentCompilationUnit != firstCompilationUnit)
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

        private Assembly PreloadReference(TreeRealization.compiler_directive reference)
        {
            var sc = GetSourceContext(reference);
            var fileName = GetReferenceFileName(reference.directive, sc, Path.GetDirectoryName(reference.source_file), true);
            return assemblyResolveScope.PreloadAssembly(fileName);
        }

        private CompilationUnit CompileReference(PascalABCCompiler.TreeRealization.unit_node_list Units, TreeRealization.compiler_directive cd)
        {
            var sc = GetSourceContext(cd);
            string UnitName = null;
            try
            {
                UnitName = GetReferenceFileName(cd.directive, sc, Path.GetDirectoryName(cd.source_file), false);
            }
            catch (AssemblyNotFound ex)
            {
                throw;
            }
            //ToDo плохо, пока дебажил - тут постоянно ловились другие исключения, не связанные с неправильным знаками в пути к сборке
            catch (Exception ex)
            {
                throw new InvalidAssemblyPathError(currentCompilationUnit.SyntaxTree.file_name, sc);
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
                //throw new DLLReadingError(unitFileName);
                throw new AssemblyReadingError(currentCompilationUnit.SyntaxTree.file_name, UnitName, sc);
        }

        private SyntaxTree.SourceContext GetSourceContext(TreeRealization.compiler_directive cd)
        {
            var loc = cd.location;
            if (loc == null) return null;
            return new SyntaxTree.SourceContext(loc.begin_line_num, loc.begin_column_num, loc.end_line_num,
                loc.end_column_num, 0, 0);
        }

        private bool HasIncludeNamespaceDirective(CompilationUnit Unit)
        {
            var directives = ConvertDirectives(Unit.SyntaxTree);

            return directives.Any(directive => directive.name.ToLower() == TreeConverter.compiler_string_consts.include_namespace_directive);

            /*foreach (compiler_directive cd in directives)
            {
                if (cd.name.ToLower() == TreeConverter.compiler_string_consts.include_namespace_directive)
                {
                    return true;
                }
            }
            return false;*/
        }


        private Dictionary<string, SyntaxTree.syntax_namespace_node> IncludeNamespaces(CompilationUnit Unit)
        {
            if (HasIncludeNamespaceDirective(Unit) && Unit.SyntaxTree is SyntaxTree.unit_module && (Unit.SyntaxTree as SyntaxTree.unit_module).unit_name.HeaderKeyword != SyntaxTree.UnitHeaderKeyword.Library)
                throw new IncludeNamespaceInUnit(currentCompilationUnit.SyntaxTree.file_name, currentCompilationUnit.SyntaxTree.source_context);
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
                        CheckForDuplicatesInUsesSection(unit.interface_part.uses_modules.units);
                        foreach (SyntaxTree.unit_or_namespace name_space in unit.interface_part.uses_modules.units)
                        {
                            if (IsPossibleNetNamespaceOrStandardPasFile(name_space, false, Path.GetDirectoryName(file)))
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
            if (!CompilerOptions.Debug && !CompilerOptions.ForDebugging)
                DefinesList.Add("RELEASE");
            else
                DefinesList.Add("DEBUG");
            DefinesList.AddRange(CompilerOptions.ForceDefines);
            SyntaxTree.compilation_unit SyntaxTree = InternalParseText(FileName, SourceText, errorsList, warnings, DefinesList);
            if (errorsList.Count > 0)
                throw errorsList[0];
            SyntaxTree = syntaxTreeConvertersController.Convert(SyntaxTree) as SyntaxTree.compilation_unit;
            return SyntaxTree;
        }

        public unit_node_list GetReferences(CompilationUnit Unit)
        {
            //TODO переделать, ConvertDirectives определена дважды и вызывается дважды!
            TreeRealization.unit_node_list res = new TreeRealization.unit_node_list();
            List<TreeRealization.compiler_directive> directives;
            if (Unit.SemanticTree is TreeRealization.common_unit_node)
                directives = (Unit.SemanticTree as TreeRealization.common_unit_node).compiler_directives;
            else
                directives = ConvertDirectives(Unit.SyntaxTree);
            foreach (TreeRealization.compiler_directive cd in directives)
            {
                if (cd.name.ToLower() == TreeConverter.compiler_string_consts.compiler_directive_platformtarget && !string.IsNullOrEmpty(cd.directive) && cd.directive.IndexOf("dotnet5") != -1)
                    CompilerOptions.UseDllForSystemUnits = false;
            }
            if (CompilerOptions.UseDllForSystemUnits)
            {
                directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\PABCRtl.dll", null, "."));
                directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\mscorlib.dll", null, "."));
                directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\System.dll", null, "."));
                directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\System.Core.dll", null, "."));
                directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\System.Numerics.dll", null, "."));
                directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\System.Windows.Forms.dll", null, "."));
                directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\System.Drawing.dll", null, "."));
                if (Unit.SyntaxTree is SyntaxTree.program_module && (Unit.SyntaxTree as SyntaxTree.program_module).used_units != null)
                    foreach (SyntaxTree.unit_or_namespace uui in (Unit.SyntaxTree as SyntaxTree.program_module).used_units.units)
                    {
                        if (uui.name.ToString() == "Graph3D")
                        {
                            directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\PresentationFramework.dll", null, "."));
                            directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\WindowsBase.dll", null, "."));
                            directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\PresentationCore.dll", null, "."));
                            directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\HelixToolkit.Wpf.dll", null, "."));
                            directives.Add(new TreeRealization.compiler_directive("reference", "%GAC%\\HelixToolkit.dll", null, "."));
                        }
                    }

            }

            var referenceDirectives = new List<TreeRealization.compiler_directive>();
            foreach (TreeRealization.compiler_directive cd in directives)
            {
                if (cd.name.ToLower() == TreeConverter.compiler_string_consts.compiler_directive_reference)
                {
                    if (string.IsNullOrEmpty(cd.directive))
                        throw new TreeConverter.SimpleSemanticError(cd.location, "EXPECTED_ASSEMBLY_NAME");
                    else
                        referenceDirectives.Add(cd);
                }
            }
            if (CompilerOptions.ProjectCompiled)
            {
                foreach (ReferenceInfo ri in project.references)
                {
                    referenceDirectives.Add(new TreeRealization.compiler_directive("reference", ri.full_assembly_name, null, project.MainFile));
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
                PreloadReference(reference);

            foreach (var reference in referenceDirectives)
                CompileReference(res, reference);

            return res;
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

            string[] standardFiles = new string[] { "PT4", "CRT", "Arrays", "MPI", "LibForHaskell", "Collections", "Core", "Oberon00System" };

            bool isStandardFile = standardFiles.Any(standardFile => standardFile.Equals(fileNameWithoutExtension, StringComparison.CurrentCultureIgnoreCase)); // current culture ?

            // если это исходный файл из папки Lib (стандартные паскалевские библиотеки)
            if (CompilerOptions.UseDllForSystemUnits
                && Path.GetDirectoryName(fileName).Equals(Path.Combine(CompilerOptions.SystemDirectory, "Lib"), StringComparison.CurrentCultureIgnoreCase)
                && isStandardFile)
            {
                string s = Path.GetFileNameWithoutExtension(fileName).ToLower();
                if (addToStandardModules && !StandardModules.Contains(s))
                    StandardModules.Add(s);
                return true;
            }

            /*if (CompilerOptions.UseDllForSystemUnits && pcuFileName != null && string.Compare(Path.GetDirectoryName(pcuFileName), Path.Combine(CompilerOptions.SystemDirectory, "Lib"), true) == 0
                && string.Compare(Path.GetFileNameWithoutExtension(pcuFileName), "PT4", true) != 0 
                && string.Compare(Path.GetFileNameWithoutExtension(pcuFileName), "CRT", true) != 0 
                && string.Compare(Path.GetFileNameWithoutExtension(pcuFileName), "Arrays", true) != 0
                //&& string.Compare(Path.GetFileNameWithoutExtension(pcu), "FormsABC", true) != 0
                && string.Compare(Path.GetFileNameWithoutExtension(pcuFileName), "MPI", true) != 0
                && string.Compare(Path.GetFileNameWithoutExtension(pcuFileName), "LibForHaskell", true) != 0
                && string.Compare(Path.GetFileNameWithoutExtension(pcuFileName), "Collections", true) != 0
                && string.Compare(Path.GetFileNameWithoutExtension(pcuFileName), "Core", true) != 0
                && string.Compare(Path.GetFileNameWithoutExtension(pcuFileName), "Oberon00System", true) != 0
                )
            {
                string s = Path.GetFileNameWithoutExtension(pcuFileName).ToLower();
                if (addToStandardModules && !StandardModules.Contains(s))
                    StandardModules.Add(s);
                return true;
            }*/
            return false;
        }

        public using_namespace GetNamespace(SyntaxTree.unit_or_namespace _name_space)
        {
            return new using_namespace(SyntaxTree.Utils.IdentListToString(_name_space.name.idents, "."));
        }

        private TreeRealization.using_namespace GetNamespace(TreeRealization.using_namespace_list using_list, string full_namespace_name, SyntaxTree.unit_or_namespace _name_space, bool possible_is_unit, Dictionary<string, SyntaxTree.syntax_namespace_node> pabc_namespaces)
        {
            if (!NetHelper.NetHelper.NamespaceExists(full_namespace_name) && !(pabc_namespaces != null && pabc_namespaces.ContainsKey(full_namespace_name)))
            {
                if (possible_is_unit)
                    if (!full_namespace_name.Contains("."))
                        throw new UnitNotFound(currentCompilationUnit.SyntaxTree.file_name, full_namespace_name, _name_space.source_context);
                throw new TreeConverter.NamespaceNotFound(full_namespace_name, get_location_from_treenode(_name_space.name, currentCompilationUnit.SyntaxTree.file_name));
            }
            return new TreeRealization.using_namespace(full_namespace_name);
        }

        public void AddNamespaces(using_namespace_list using_list, List<SyntaxTree.unit_or_namespace> namespaces, bool possible_is_units, Dictionary<string, SyntaxTree.syntax_namespace_node> pabc_namespaces)
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
                if ((cu as SyntaxTree.unit_module).implementation_part != null)
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
            if (ErrorList.Count == 0 && cu != null && cu.source_context != null)
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

        public static bool IsDll(SyntaxTree.compilation_unit cu)
        {
            foreach (SyntaxTree.compiler_directive cd in cu.compiler_directives)
                if (string.Compare(cd.Name.text, "apptype", true) == 0 && string.Compare(cd.Directive.text, "dll", true) == 0)
                    return true;
            return false;
        }

        public CompilationUnit CompileUnit(unit_node_list unitsFromUsesSection, Dictionary<unit_node, CompilationUnit> DirectCompilationUnits, SyntaxTree.unit_or_namespace unitToCompile, string prev_path)
        {
            string UnitFileName = GetUnitFileName(unitToCompile, prev_path);
            string UnitId = Path.ChangeExtension(UnitFileName, null);
            // Имя папки, в которой лежит текущий модуль
            // Используется для подключения модулей, $include и т.п. из модуля, подключённого с uses-in
            var currentDirectory = Path.GetDirectoryName(UnitFileName);

            // ошибка - пространство имен не может содержать in секцию (для указания файла
            // TODO: скинуть семантические проверки в кучу (если получится)
            CompilationUnit currentUnit = UnitTable[UnitId];

            SemanticCheckUsesInIsNotNamespace(unitToCompile, currentUnit);

            // если модуль уже скомпилирован - возвращаем (возможно, только интерфейс модуля)   EVA
            if (IsUnitCompiled(unitsFromUsesSection, DirectCompilationUnits, unitToCompile, currentUnit))
                return currentUnit;

            // если есть pcu - возврат  EVA
            if (IsUnitHasPCU(unitsFromUsesSection, DirectCompilationUnits, unitToCompile, ref UnitFileName, UnitId, ref currentUnit))
                return currentUnit;

            string SourceText = null;
            Dictionary<SyntaxTree.syntax_tree_node, string> docs = null;
            // нет pcu и модуль не откомпилирован => новый модуль   EVA
            InitializeNewUnit(unitToCompile, UnitFileName, UnitId,
                ref currentUnit, ref SourceText, ref docs);


            //TODO переделать, слишком сложно, некоторый код дублируется
            //WORK IN PROGRESS  (EVA)
            
            // TODO: функцию можно упростить
            List<SyntaxTree.unit_or_namespace> interfaceUsesList = GetInterfaceUsesSection(currentUnit.SyntaxTree);

            SetUseDLLForSystemUnits(currentDirectory, interfaceUsesList, interfaceUsesList.Count - 1 - currentUnit.InterfaceUsedUnits.Count);

            unit_node_list References = GetReferences(currentUnit); // получение dll
            var namespaces = IncludeNamespaces(currentUnit);
            if (interfaceUsesList != null)
                if (CompileInterfaceDependencies(unitsFromUsesSection, DirectCompilationUnits, unitToCompile, UnitFileName, currentDirectory, currentUnit, interfaceUsesList, References, namespaces))
                    return currentUnit;

            currentCompilationUnit = currentUnit;

            currentUnit.InterfaceUsedUnits.AddRange(References);

            AddNamespaces(currentUnit.InterfaceUsingNamespaceList, currentUnit.possibleNamespaces, true, namespaces);
            AddNamespaces(currentUnit.InterfaceUsingNamespaceList, GetInterfaceSyntaxUsingList(currentUnit.SyntaxTree));

            //Console.WriteLine("GenerateILCodeAndSaveAssembly Interface "+unitFileName);//DEBUG

            // компилируем интерфейс текущего модуля EVA
            CompileCurrentUnitInterface(UnitFileName, currentUnit, docs);

            common_unit_node commonUnitNode = currentUnit.SemanticTree as common_unit_node;
            /*if (cun != null)
            {
            	if (!UnitsLogicallySortedList.Contains(currentUnit))//vnimanie zdes inogda pri silnoj zavisimosti modulej moduli popadajut neskolko raz
            	UnitsLogicallySortedList.Add(currentUnit);
            }*/
            // интерфейс скомпилирован - переходим к секции реализации
            currentUnit.State = UnitState.InterfaceCompiled;
            if (unitsFromUsesSection != null)
            {
                if (unitsFromUsesSection.AddElement(currentUnit.SemanticTree, unitToCompile.UsesPath()))
                    DirectCompilationUnits.Add(currentUnit.SemanticTree, currentUnit);
                unitsFromUsesSection.AddRange(References);
            }

            // берем модули из секции uses в реализации EVA
            interfaceUsesList = GetImplementationUsesSection(currentUnit.SyntaxTree);

            CompilationUnit compilationUnit = null;
            bool interfcompile = true;

            currentUnit.ImplementationUsedUnits.Clear();
            currentUnit.possibleNamespaces.Clear();

            // Компиляция зависимостей в области реализации    EVA
            if (CompileImplementationDependencies(currentDirectory, currentUnit, interfaceUsesList, namespaces, commonUnitNode, ref compilationUnit, ref interfcompile))
                return currentUnit;

            //Console.WriteLine("GenerateILCodeAndSaveAssembly Implementation "+unitFileName);//DEBUG

            // компилируем реализацию текущего модуля EVA
            CompileCurrentUnitImplementation(UnitFileName, currentUnit, docs);

            currentUnit.State = UnitState.Compiled;
            if (commonUnitNode != null)
            {
                if (!UnitsLogicallySortedList.Contains(currentUnit))//vnimanie zdes inogda pri silnoj zavisimosti modulej moduli popadajut neskolko raz
                    UnitsLogicallySortedList.Add(currentUnit);
            }
            OnChangeCompilerState(this, CompilerState.EndCompileFile, UnitFileName);
            //SavePCU(currentUnit, unitFileName);
            currentUnit.UnitFileName = UnitFileName;
            return currentUnit;
            /*if(currentUnit.State!=UnitState.Compiled)
            { 
                //Console.WriteLine("GenerateILCodeAndSaveAssembly Interface "+unitFileName);//DEBUG
                currentUnit.SemanticTree=SyntaxTreeToSemanticTreeConverter.CompileInterface(currentUnit.SyntaxTree,
                    currentUnit.InterfaceUsedUnits,currentUnit.syntax_error);
                currentUnit.State=UnitState.InterfaceCompiled;
                implementationUsesList=GetSemanticImplementationUsesList(currentUnit.SyntaxTree);
                if(implementationUsesList!=null)
                    for(int i=implementationUsesList.Count-1;i>=0;i--)
                        CompileUnit(currentUnit.ImplementationUsedUnits,implementationUsesList[i]);        
                //Console.WriteLine("GenerateILCodeAndSaveAssembly Implementation "+unitFileName);//DEBUG
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

        private void SemanticCheckUsesInIsNotNamespace(SyntaxTree.unit_or_namespace unitToCompile, CompilationUnit currentUnit)
        {
            if (currentUnit != null && currentUnit.SemanticTree is dot_net_unit_node
                            && unitToCompile is SyntaxTree.uses_unit_in ui && ui.in_file != null) // значит, это пространство имен и секция in у него должна отсутствовать
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
                    
                    TreeConverter.SemanticRules.SymbolTableCaseSensitive = currentUnit.CaseSensitive;
                    
                    SyntaxTreeToSemanticTreeConverter.CompileImplementation(
                        (common_unit_node)currentUnit.SemanticTree,
                        currentUnit.SyntaxTree,
                        buildImplementationUsesList(currentUnit),
                        ErrorsList, Warnings,
                        currentUnit.syntax_error,
                        BadNodesInSyntaxTree,
                        currentUnit.InterfaceUsingNamespaceList,
                        currentUnit.ImplementationUsingNamespaceList,
                        docs,
                        CompilerOptions.Debug,
                        CompilerOptions.ForDebugging,
                        CompilerOptions.ForIntellisense,
                        CompiledVariables
                        );
                    CheckErrorsAndThrowTheFirstOne(); // выбрасывание ошибок из ErrorList при необходимости
                }
            }
        }

        private bool CompileImplementationDependencies(string curr_path, CompilationUnit currentUnit, List<SyntaxTree.unit_or_namespace> implementationUsesList, Dictionary<string, SyntaxTree.syntax_namespace_node> namespaces, common_unit_node commonUnitNode, ref CompilationUnit compilationUnit, ref bool interfereCompile)
        {
            if (implementationUsesList != null)
            {
                for (int i = implementationUsesList.Count - 1; i >= 0; i--)
                {
                    if (!IsPossibleNetNamespaceOrStandardPasFile(implementationUsesList[i], true, curr_path))
                    {
                        compilationUnit = UnitTable[Path.ChangeExtension(GetUnitFileName(implementationUsesList[i], curr_path), null)];
                        if (compilationUnit != null && compilationUnit.State == UnitState.BeginCompilation) // состояние BeginCompilation стоит изначально
                        {
                            UnitsToCompileDelayedList.Add(compilationUnit);
                            interfereCompile = false; // обрубаем компиляцию реализации в CompileUnit - не все интерфейсы еще откомпилированы !!!
#if DEBUG
                            // Console.WriteLine("[DEBUGINFO]Send compile to end " + Path.GetFileName(GetUnitFileName(implementationUsesList[i])));//DEBUG
#endif
                        }
                        else
                        {
                            CompileUnit(currentUnit.ImplementationUsedUnits, currentUnit.DirectImplementationCompilationUnits, implementationUsesList[i], curr_path);
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

            AddNamespaces(currentUnit.ImplementationUsingNamespaceList, currentUnit.possibleNamespaces, true, namespaces);
            AddNamespaces(currentUnit.ImplementationUsingNamespaceList, GetImplementationSyntaxUsingList(currentUnit.SyntaxTree));

            if (!interfereCompile)
            {
                UnitsToCompileDelayedList.Add(currentUnit);
                if (commonUnitNode != null)
                {
                    if (!UnitsLogicallySortedList.Contains(currentUnit))//vnimanie zdes inogda pri silnoj zavisimosti modulej moduli popadajut neskolko raz
                        UnitsLogicallySortedList.Add(currentUnit);
                }
                //Console.WriteLine("Send compile to end "+unitFileName);//DEBUG
                return true;
            }

            return false;
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
                    PascalABCCompiler.TreeConverter.SemanticRules.SymbolTableCaseSensitive = currentUnit.CaseSensitive;
                    currentUnit.SemanticTree = SyntaxTreeToSemanticTreeConverter.CompileInterface(
                        currentUnit.SyntaxTree,
                        currentUnit.InterfaceUsedUnits,
                        ErrorsList, Warnings,
                        currentUnit.syntax_error,
                        BadNodesInSyntaxTree,
                        currentUnit.InterfaceUsingNamespaceList,
                        docs,
                        CompilerOptions.Debug,
                        CompilerOptions.ForDebugging,
                        CompilerOptions.ForIntellisense,
                        CompiledVariables
                        );
                    CheckErrorsAndThrowTheFirstOne();
                }
            }
        }

        private bool CompileInterfaceDependencies(unit_node_list unitsFromUsesSection, Dictionary<unit_node, CompilationUnit> DirectCompilationUnits, SyntaxTree.unit_or_namespace unitToCompile, string UnitFileName, string curr_path, CompilationUnit currentUnit, List<SyntaxTree.unit_or_namespace> interfaceUsesList, unit_node_list References, Dictionary<string, SyntaxTree.syntax_namespace_node> namespaces)
        {
            // TODO: выяснить, что за условие в for
            for (int i = interfaceUsesList.Count - 1 - currentUnit.InterfaceUsedUnits.Count; i >= 0; i--) // здесь откидываются модули с уже откомпилированными интерфейсами из секции uses (см. комментарий, обозначенный #1710)
            {
                if (IsPossibleNetNamespaceOrStandardPasFile(interfaceUsesList[i], true, curr_path) || namespaces.ContainsKey(interfaceUsesList[i].name.idents[0].name))
                {
                    currentUnit.InterfaceUsedUnits.AddElement(new namespace_unit_node(GetNamespace(interfaceUsesList[i])), null);
                    currentUnit.possibleNamespaces.Add(interfaceUsesList[i]);
                }
                else
                {
                    var CurrentSyntaxUnitFileName = GetUnitFileName(interfaceUsesList[i], curr_path);
                    var CurrentSyntaxUnitId = Path.ChangeExtension(CurrentSyntaxUnitFileName, null);
                    currentUnit.CurrentUsesUnitId = CurrentSyntaxUnitId;

                    if (UnitTable[CurrentSyntaxUnitId] != null)
                        if (UnitTable[CurrentSyntaxUnitId].State == UnitState.BeginCompilation) // компиляция модуля
                        {
                            string CurrentSyntaxUnit_CurrentUsesUnitId = UnitTable[CurrentSyntaxUnitId].CurrentUsesUnitId;
                            if (CurrentSyntaxUnit_CurrentUsesUnitId != null)
                            {
                                //если сначала взяли pcu а потом решили его перекомпилировать, поэтому в таблице его нет
                                if (UnitTable[CurrentSyntaxUnit_CurrentUsesUnitId] == null)
                                    UnitTable[CurrentSyntaxUnitId].CurrentUsesUnitId = CurrentSyntaxUnit_CurrentUsesUnitId;
                                //далее финальная поверка на зацикливание
                                if (UnitTable[CurrentSyntaxUnitId].CurrentUsesUnitId != null && UnitTable[UnitTable[CurrentSyntaxUnitId].CurrentUsesUnitId].State == UnitState.BeginCompilation)
                                    throw new CycleUnitReference(UnitFileName, interfaceUsesList[i]);
                            }
                        }

                    // компиляция модулей из интерфейса текущего модуля 
                    CompileUnit(currentUnit.InterfaceUsedUnits, currentUnit.DirectInterfaceCompilationUnits, interfaceUsesList[i], curr_path);

                    // если модуль был откомпилирован в другом рекурсивном вызове 
                    if (currentUnit.State == UnitState.Compiled)
                    {
                        if (unitsFromUsesSection.AddElement(currentUnit.SemanticTree, unitToCompile.UsesPath())) // #1710 добавление в список модулей из uses происходит только в конце компиляции интерфейса юнита или позже во всех случаях 
                            DirectCompilationUnits.Add(currentUnit.SemanticTree, currentUnit);
                        unitsFromUsesSection.AddRange(References);
                        return true;
                    }
                }

            }

            return false;
        }

        private void SetUseDLLForSystemUnits(string currentDirectory, List<SyntaxTree.unit_or_namespace> usesList, int lastUnitIndex)
        {
            if (usesList != null)
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

        // TODO: Выделить из метода чистое составление синтаксического дерева с параметрами     EVA
        // Получение исходного кода модуля, заполнение документации,
        // генерация синтаксического дерева,
        // обработка синтаксических ошибок
        private void InitializeNewUnit(SyntaxTree.unit_or_namespace unitToCompile, string UnitFileName, string UnitId, ref CompilationUnit currentUnit, ref string SourceText, ref Dictionary<SyntaxTree.syntax_tree_node, string> docs)
        {
            if (currentUnit == null)
            {

                currentUnit = new CompilationUnit();
                if (firstCompilationUnit == null)
                    firstCompilationUnit = currentUnit;
                OnChangeCompilerState(this, CompilerState.BeginCompileFile, UnitFileName); // начало компиляции модуля

                // получение текста исходного кода модуля (если нет синтаксического дерева) EVA
                GetSourceCode(unitToCompile, UnitFileName, currentUnit, ref SourceText);

                // разбор синтаксического дерева EVA
                GenSyntaxTree(UnitFileName, currentUnit, SourceText);

                if (errorsList.Count == 0) // SSM 2/05/16 - для преобразования синтаксических деревьев извне (синтаксический сахар)
                {
                    currentUnit.SyntaxTree = syntaxTreeConvertersController.Convert(currentUnit.SyntaxTree) as SyntaxTree.compilation_unit;
                }

                // генерация документации к узлам синтаксического дерева EVA
                GenUnitDocumentation(currentUnit, SourceText, ref docs);

                // legacy 
                if (currentUnit.SyntaxTree is SyntaxTree.unit_module)
                {
                    // Проверка на явный namespace (паскалевский)
                    if ((currentUnit.SyntaxTree as SyntaxTree.unit_module).unit_name.HeaderKeyword == SyntaxTree.UnitHeaderKeyword.Namespace)
                        throw new NamespacesCanBeCompiledOnlyInProjects(currentUnit.SyntaxTree.source_context);
                    CompilerOptions.UseDllForSystemUnits = false;
                }

                // SSM 21/05/20 Проверка, что мы не записали apptype dll в небиблиотеку

                var unitSyntaxTree = currentUnit.SyntaxTree;
                // TODO: пофиксить разбор ошибок в директивах компилятора и разбросанные директивы компилятора собрать
                CheckDLLDirectiveOnlyForLibraries(unitSyntaxTree);

                // TODO: возможно, имплементировать переключение парсеров
                if (IsDll(currentUnit.SyntaxTree))
                    CompilerOptions.OutputFileType = CompilerOptions.OutputType.ClassLibrary;
                if (ParsersController.LastParser != null)
                    currentUnit.CaseSensitive = ParsersController.LastParser.CaseSensitive;
                currentCompilationUnit = currentUnit;

                currentUnit.SyntaxUnitName = unitToCompile;

                //BadNodesInSyntaxTree.Clear();
                // сопоставление нодам ошибок EVA
                
                
                MatchErrorsToBadNodes(currentUnit);
                CheckErrorsAndThrowTheFirstOne();

                //if (currentUnit.SyntaxTree == null)
                //if (errorsList.Count > 0)
                //{
                //    //if (errorsList.Count == 0)
                //    //    throw new Errors.SyntaxError("Internal parser error: Parser not create syntax tree", unitFileName,null,null);
                //    throw errorsList[0];
                //}

                UnitTable[UnitId] = currentUnit;

                // ошибки не UnitModule EVA
                CheckCurrentUnitIsUnit(UnitFileName, currentUnit);

                // здесь в начало uses добавляем стандартные модули
#if DEBUG
                if (InternalDebug.AddStandartUnits)
#endif
                    AddStandartUnitsToUsesSection(currentUnit.SyntaxTree);

            }

            this.usesSection = unitToCompile;
            currentCompilationUnit = currentUnit;

            currentUnit.possibleNamespaces.Clear();
        }
        // Синтактико-семантическая ошибка - проверка, что currentUnit является модулем,
        // а не основной программой и не dll         EVA
        private void CheckCurrentUnitIsUnit(string UnitFileName, CompilationUnit currentUnit)
        {
            if (UnitTable.Count > 1) // если это не главный модуль (программа в unittable всегда идет первой)
                if (currentUnit.SyntaxTree is SyntaxTree.program_module)
                    throw new UnitModuleExpected(UnitFileName, currentUnit.SyntaxTree.source_context.LeftSourceContext);
                else if (IsDll(currentUnit.SyntaxTree))
                    throw new UnitModuleExpectedLibraryFound(UnitFileName, currentUnit.SyntaxTree.source_context.LeftSourceContext);
        }

        private void MatchErrorsToBadNodes(CompilationUnit currentUnit)
        {
            if (errorsList.Count > 0)
            {
                currentUnit.syntax_error = errorsList[0] as SyntaxError;
                foreach (Error er in errorsList)
                    if (er is SyntaxError && (er as SyntaxError).bad_node != null)
                        BadNodesInSyntaxTree[(er as SyntaxError).bad_node] = er;
            }
        }

        private void CheckDLLDirectiveOnlyForLibraries(SyntaxTree.compilation_unit unitSyntaxTree)
        {
            // Странно, почему именно такая проверка (а если library и не dll?)
            if (unitSyntaxTree != null) // SSM 06.06.22 - здесь была ошибка - unitSyntaxTree может быть null и тогда исключение!!! Добавил if
                foreach (SyntaxTree.compiler_directive cd in unitSyntaxTree.compiler_directives)
                    if (string.Compare(cd.Name.text, "apptype", true) == 0 && string.Compare(cd.Directive.text, "dll", true) == 0)
                    {
                        if (!(unitSyntaxTree is SyntaxTree.unit_module) ||
                            (unitSyntaxTree is SyntaxTree.unit_module um && um.unit_name.HeaderKeyword != SyntaxTree.UnitHeaderKeyword.Library))
                        {
                            ErrorsList.Add(new AppTypeDllIsAllowedOnlyForLibraries(unitSyntaxTree.file_name, cd.source_context)); // если в директивах появилось dll, но это не Library
                            break;
                        }
                    }
        }

        private void GenSyntaxTree(string UnitFileName, CompilationUnit currentUnit, string SourceText)
        {
            List<string> DefinesList = new List<string>();
            DefinesList.Add("PASCALABC");
            if (!CompilerOptions.Debug && !CompilerOptions.ForDebugging)
                DefinesList.Add("RELEASE");
            else
                DefinesList.Add("DEBUG");
            DefinesList.AddRange(CompilerOptions.ForceDefines);
            if (CompilerOptions.UnitSyntaxTree != null)
            {
                currentUnit.SyntaxTree = CompilerOptions.UnitSyntaxTree;
                CompilerOptions.UnitSyntaxTree = null;
            }
            // синтаксический анализ
            else
                currentUnit.SyntaxTree = InternalParseText(UnitFileName, SourceText, errorsList, warnings, DefinesList);
        }

        private void GetSourceCode(SyntaxTree.unit_or_namespace unitToCompile, string UnitFileName, CompilationUnit currentUnit, ref string SourceText)
        {
            if (CompilerOptions.UnitSyntaxTree == null)
            {
                // TODO: подебажить GetSourceFileText
                SourceText = GetSourceFileText(UnitFileName);
                if (SourceText == null)
                    if (currentUnit == firstCompilationUnit)
                        throw new SourceFileNotFound(UnitFileName);
                    else
                        throw new UnitNotFound(currentCompilationUnit.SyntaxTree.file_name, UnitFileName, unitToCompile.source_context);
            }
        }

        private void GenUnitDocumentation(CompilationUnit currentUnit, string SourceText, ref Dictionary<SyntaxTree.syntax_tree_node, string> docs)
        {
            if (errorsList.Count == 0 && need_gen_doc(currentUnit.SyntaxTree))
            {
                if (SourceText != null)
                {
                    docs = AddDocumentationToNodes(currentUnit.SyntaxTree, SourceText);
                    if (docs != null)
                        currentUnit.Documented = true;
                }
            }
        }

        private bool IsUnitCompiled(unit_node_list unitsFromUsesSection, Dictionary<unit_node, CompilationUnit> DirectCompilationUnits, SyntaxTree.unit_or_namespace unitToCompile, CompilationUnit currentUnit)
        {
            if (currentUnit != null)
                if (currentUnit.State != UnitState.BeginCompilation || currentUnit.SemanticTree != null)  //ИЗБАВИТЬСЯ ОТ ВТОРОГО УСЛОВИЯ
                {
                    // TODO: подумать над названием DirectCompilationUnits
                    if (unitsFromUsesSection.AddElement(currentUnit.SemanticTree, unitToCompile.UsesPath()))
                        DirectCompilationUnits.Add(currentUnit.SemanticTree, currentUnit);
                    unitsFromUsesSection.AddRange(GetReferences(currentUnit));
                    return true;
                }
            return false;
        }

        private bool IsUnitHasPCU(unit_node_list unitsFromUsesSection, Dictionary<unit_node, CompilationUnit> DirectCompilationUnits, SyntaxTree.unit_or_namespace unitToCompile, ref string UnitFileName, string UnitId, ref CompilationUnit currentUnit)
        {
            if (currentUnit == null && Path.GetExtension(UnitFileName).ToLower() == CompilerOptions.CompiledUnitExtension)
                if (File.Exists(UnitFileName))
                {
                    if (UnitTable.Count == 0) throw new ProgramModuleExpected(UnitFileName, null);
                    try
                    {
                        if ((currentUnit = ReadPCU(UnitFileName)) != null)
                        {
                            if (unitsFromUsesSection.AddElement(currentUnit.SemanticTree, unitToCompile.UsesPath()))
                                DirectCompilationUnits.Add(currentUnit.SemanticTree, currentUnit);
                            unitsFromUsesSection.AddRange(GetReferences(currentUnit));
                            UnitTable[UnitId] = currentUnit;
                            return true;
                        }
                    }
                    catch (InvalidPCUFule)
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
            return false;
        }

        private Dictionary<SyntaxTree.syntax_tree_node, string> AddDocumentationToNodes(SyntaxTree.compilation_unit cu, string Text)
        {
            List<PascalABCCompiler.Errors.Error> errors = new List<PascalABCCompiler.Errors.Error>();
            List<PascalABCCompiler.Errors.CompilerWarning> warnings = new List<CompilerWarning>();
            string doctagsParserExtension = Path.GetExtension(cu.file_name) + "dt" + PascalABCCompiler.Parsers.Controller.HideParserExtensionPostfixChar;
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
                if (string.Compare(cd.Name.text, "gendoc", true) == 0 && string.Compare(cd.Directive.text, "true", true) == 0)
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
                //ErrorsList.Add(new Errors.CompilerInternalError(string.Format("Compiler.GenerateILCodeAndSaveAssembly[{0}]", Path.GetFileName(this.currentCompilationUnit.SyntaxTree.file_name)), err));
                OnChangeCompilerState(this, CompilerState.PCUWritingError, Unit.UnitFileName);
#if DEBUG
                if (!InternalDebug.SkipPCUErrors)
                    throw new Errors.CompilerInternalError(string.Format("Compiler.GenerateILCodeAndSaveAssembly[{0}]", Path.GetFileName(this.currentCompilationUnit.SyntaxTree.file_name)), err);
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
                    UnitsLogicallySortedList.Add(cu);
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



        public CompilationUnit ReadDLL(string FileName)
        {
            if (DLLCache.ContainsKey(FileName))
                return DLLCache[FileName];
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
                DLLCache[FileName] = cu;

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
            currentSyntaxUnit = new SyntaxTree.uses_unit_in(new SyntaxTree.string_const(program_folder + "\\" + unit_name+".pas"));
            CompileUnit(unitsFromUsesSection, currentSyntaxUnit);
            CompilationUnit cu = new CompilationUnit();
            if (unitsFromUsesSection.Count != 0)
                cu.SemanticTree = unitsFromUsesSection[unitsFromUsesSection.Count - 1];
            else
                return null;
            return cu;
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
            _semantic_tree = null;
            if (close_pcu)
            {
                PCUReader.CloseUnits();
                PCUWriter.Clear();
            }

            RecompileList.Clear();
            CycleUnits.Clear();
            UnitTable.Clear();
            UnitsLogicallySortedList.Clear();
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
