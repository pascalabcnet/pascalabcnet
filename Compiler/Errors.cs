// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.Errors
{

    /// <summary>
    /// Базовый класс для ошибок, бросаемых компилятором
    /// </summary>
    public class CompilerThrownError : LocatedError
    {
        public CompilerThrownError(string message)
            : base(message)
        {
        }
        public CompilerThrownError(string message, string FileName)
            : base(message, FileName)
        {
        }
        public override string ToString()
        {
            return Message;
        }
    }

    /// <summary>
    /// Бросается в случае невозможности чтения pcu
    /// </summary>
    public class ReadPCUError : CompilerThrownError
    {
        public ReadPCUError(string FileName)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_READ_PCU{0}_ERROR"), FileName))
        {
        }
    }

    /// <summary>
    /// Бросается в случае использования uses in в пространстве имен
    /// </summary>
    public class NamespaceCannotHaveInSection : CompilerThrownError
    {
        public NamespaceCannotHaveInSection(SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_NAMESPACE_CANNOT_HAVE_IN_SECTION")))
        {
            this.source_context = sc;
        }
    }

    /// <summary>
    /// Бросается, если встречена не основная программа (там где она должна быть)
    /// </summary>
    public class ProgramModuleExpected : CompilerThrownError
    {
        public ProgramModuleExpected(string FileName, SyntaxTree.SourceContext sc)
            : base(StringResources.Get("COMPILATIONERROR_PROGRAM_MODULE_EXPECTED"), FileName)
        {
            this.source_context = sc;
        }
    }

    /// <summary>
    /// Бросается, если встречен не модуль (там где он должна быть)
    /// </summary>
    public class UnitModuleExpected : CompilerThrownError
    {
        public UnitModuleExpected(string FileName, SyntaxTree.SourceContext sc)
            : base(StringResources.Get("COMPILATIONERROR_UNIT_MODULE_EXPECTED"), FileName)
        {
            this.source_context = sc;
        }
    }

    /// <summary>
    /// Бросается при обнаружении директивы {$apptype dll} не в библиотеке
    /// </summary>
    public class AppTypeDllIsAllowedOnlyForLibraries : CompilerThrownError
    {
        public AppTypeDllIsAllowedOnlyForLibraries(string FileName, SyntaxTree.SourceContext sc)
            : base(StringResources.Get("COMPILATIONERROR_APPTYPE_DLL_IS_ALLOWED_ONLY_FOR_LIBRARIES"), FileName)
        {
            this.source_context = sc;
        }
    }

    /// <summary>
    /// Бросается при компиляции библиотеки не первой
    /// </summary>
    public class UnitModuleExpectedLibraryFound : CompilerThrownError
    {
        public UnitModuleExpectedLibraryFound(string FileName, SyntaxTree.SourceContext sc)
            : base(StringResources.Get("COMPILATIONERROR_UNIT_MODULE_EXPECTED_LIBRARY_FOUND"), FileName)
        {
            this.source_context = sc;
        }
    }

    /// <summary>
    /// Бросается, если файл сборки не найден
    /// </summary>
    public class AssemblyNotFound : CompilerThrownError
    {
        public string AssemblyFileName;
        public AssemblyNotFound(string FileName, string AssemblyFileName, SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_ASSEMBLY_{0}_NOT_FOUND"), AssemblyFileName), FileName)
        {
            this.AssemblyFileName = AssemblyFileName;
            this.source_context = sc;
        }

    }

    /// <summary>
    /// Бросается при невозможности чтения сборки
    /// </summary>
    public class AssemblyReadingError : CompilerThrownError
    {
        public string AssemblyFileName;
        public AssemblyReadingError(string FileName, string AssemblyFileName, SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_ASSEMBLY_{0}_READING_ERROR"), AssemblyFileName), FileName)
        {
            this.AssemblyFileName = AssemblyFileName;
            this.source_context = sc;
        }
    }

    /// <summary>
    /// Бросается при попытке обработки неправильного пути к сборке
    /// </summary>
    public class InvalidAssemblyPathError : CompilerThrownError
    {
        public InvalidAssemblyPathError(string FileName, SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_INVALID_ASSEMBLY_PATH")), FileName)
        {
            this.source_context = sc;
        }
    }

    /// <summary>
    /// Бросается при попытке обработки неправильного пути к файлу
    /// </summary>
    public class InvalidPathError : CompilerThrownError
    {
        public InvalidPathError(SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_INVALID_PATH")))
        {
            this.source_context = sc;
            this.fileName = sc.FileName;
        }
    }

    /// <summary>
    /// Бросается при неудаче в нахождении файла ресурсов
    /// </summary>
    public class ResourceFileNotFound : CompilerThrownError
    {
        public ResourceFileNotFound(string fileName, string ResFileName, SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_RESOURCEFILE_{0}_NOT_FOUND"), ResFileName), fileName)
        {
            source_context = sc;
        }
    }

    /// <summary>
    /// Бросается при подключении явного пространства имен в модуле
    /// </summary>
    public class IncludeNamespaceInUnitError : CompilerThrownError
    {
        public IncludeNamespaceInUnitError(string FileName, SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_INCLUDE_NAMESPACE_IN_UNIT")), FileName)
        {
            this.source_context = sc;
        }
    }

    /// <summary>
    /// Бросается, если встречено не явное пространство имен
    /// </summary>
    public class NamespaceModuleExpected : CompilerThrownError
    {
        public NamespaceModuleExpected(SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_NAMESPACE_MODULE_EXPECTED")))
        {
            this.source_context = sc;
        }
    }

    /// <summary>
    /// Бросается в случае некорректного использования директивы {$mainresource ...}
    /// </summary>
    public class MainResourceNotAllowed : CompilerThrownError
    {
        public MainResourceNotAllowed(string fileName, SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_MAINRESOURCE_NOT_ALLOWED")), fileName)
        {
            source_context = sc;
        }

    }

    /// <summary>
    /// Бросается при нахождении дубликатов в секции uses
    /// </summary>
    public class DuplicateUsesUnit : CompilerThrownError
    {
        public string UnitName;
        public DuplicateUsesUnit(string FileName, string UnitName, SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_DUPLICATE_USES_UNIT{0}"), UnitName), FileName)
        {
            this.UnitName = UnitName;
            this.source_context = sc;
        }
    }

    /// <summary>
    /// Бросается при нахождении дубликатов директив, не поддерживающих многократное использование в рамках некоторого контекста
    /// </summary>
    public class DuplicateDirective : CompilerThrownError
    {
        public string DirectiveName;
        public DuplicateDirective(string FileName, string DirectiveName, SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_DUPLICATE_DIRECTIVE{0}"), DirectiveName), FileName)
        {
            this.DirectiveName = DirectiveName;
            this.source_context = sc;
        }
    }

    /// <summary>
    /// Legacy, бросается, если unitModule.unitName.HeaderKeyword == SyntaxTree.UnitHeaderKeyword.Namespace
    /// </summary>
    public class NamespacesCanBeCompiledOnlyInProjects : CompilerThrownError
    {
        public NamespacesCanBeCompiledOnlyInProjects(SyntaxTree.SourceContext sc)
            : base(StringResources.Get("COMPILATIONERROR_NAMESPACE_CAN_BE_COMPILED_ONLY_IN_PROJECTS"))
        {
            this.source_context = sc;
        }
    }

    /// <summary>
    /// Бросается, если модуль (unit) не найден по некоторому пути
    /// </summary>
    public class UnitNotFound : CompilerThrownError
    {
        public string UnitName;
        public UnitNotFound(string FileName, string UnitName, SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_UNIT_{0}_NOT_FOUND"), UnitName), FileName)
        {
            this.UnitName = UnitName;
            this.source_context = sc;
        }
    }

    /// <summary>
    /// Бросается при некорректном пути в uses in
    /// </summary>
    public class UsesInWrongName : CompilerThrownError
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

    /// <summary>
    /// Бросается при невозможности найти исходник по некотрому пути
    /// </summary>
    public class SourceFileNotFound : CompilerThrownError
    {
        public SourceFileNotFound(string FileName)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_SOURCE_FILE_{0}_NOT_FOUND"), FileName))
        {
        }
    }

    /// <summary>
    /// Бросается при попытке файловой операции с недостаточными правами
    /// </summary>
    public class UnauthorizedAccessToFile : CompilerThrownError
    {
        public UnauthorizedAccessToFile(string FileName)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_NO_ACCESS_TO_FILE{0}"), FileName))
        {
        }
    }

    /// <summary>
    /// Бросается в случае обнаружения циклической зависимости модулей
    /// </summary>
    public class CycleUnitReference : CompilerThrownError
    {
        public CycleUnitReference(string FileName, SyntaxTree.unit_or_namespace SyntaxUsesUnit)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_CYCLIC_UNIT_REFERENCE_WITH_UNIT_{0}"), SyntaxTree.Utils.IdentListToString(SyntaxUsesUnit.name.idents, ".")), FileName)
        {
            this.source_context = SyntaxUsesUnit.source_context;
        }
    }

    /// <summary>
    /// Бросается, если пользователь указывает неподдерживаемый целевой framework
    /// </summary>
    public class UnsupportedTargetFramework : CompilerThrownError
    {
        public UnsupportedTargetFramework(string FrameworkName, SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_UNSUPPORTED_TARGETFRAMEWORK_{0}"), FrameworkName))
        {
            this.source_context = sc;
        }
    }

    /// <summary>
    /// Бросается, если пользователь указывает неподдерживаемую целевую платформу
    /// </summary>
    public class UnsupportedTargetPlatform : CompilerThrownError
    {
        public UnsupportedTargetPlatform(string platformName, SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_UNSUPPORTED_TARGET_PLATFORM{0}"), platformName))
        {
            source_context = sc;
        }
    }

    /// <summary>
    /// Бросается, если пользователь указывает неподдерживаемый тип выходного файла (например, в директиве {$apptype ...})
    /// </summary>
    public class UnsupportedOutputFileType : CompilerThrownError
    {
        public UnsupportedOutputFileType(string outputFileType, SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_UNSUPPORTED_OUTPUT_FILE_TYPE{0}"), outputFileType))
        {
            source_context = sc;
        }
    }

    /// <summary>
    /// Бросается при отсутствии некоторого файла по некоторому пути
    /// </summary>
    public class FileNotFound : CompilerThrownError
    {
        public FileNotFound(string fileName, SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_FILE_{0}_NOT_FOUND"), fileName))
        {
            this.source_context = sc;
        }
    }

    /*public class DLLReadingError : TreeConverter.CompilationError
    {
        private string _dll_name;

        public DLLReadingError(string dll_name)
        {
            _dll_name = dll_name;
        }

        public string dll_name
        {
            get
            {
                return _dll_name;
            }
        }

        public override string ToString()
        {
            //return ("Dll: "+dll_name+" not found");
            return string.Format(StringResources.Get("COMPILATIONERROR_ASSEMBLY_{0}_READING_ERROR"), dll_name);
        }
    }

    public class InvalidUnit : TreeConverter.CompilationError
    {
        private string _unit_name;

        public InvalidUnit(string unit_name)
        {
            _unit_name = unit_name;
        }

        public string unit_name
        {
            get
            {
                return _unit_name;
            }
        }

        public override string ToString()
        {
            return ("Invalid unit: " + _unit_name);
        }
    }

    public class UnitCompilationError : TreeConverter.CompilationError
    {
        private SyntaxTree.unit_or_namespace _SyntaxUsesUnit;
        private string _file_name;

        public UnitCompilationError(string fileName, SyntaxTree.unit_or_namespace syntaxUsesUnit)
        {
            _SyntaxUsesUnit = syntaxUsesUnit;
            _file_name = fileName;
        }

        public SyntaxTree.unit_or_namespace SyntaxUsesUnit
        {
            get
            {
                return _SyntaxUsesUnit;
            }
        }

        public string file_name
        {
            get
            {
                return _file_name;
            }
        }
    }*/
}
