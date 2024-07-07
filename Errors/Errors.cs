// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.IO;
using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.Errors
{
    // Базовые классы ошибок
    #region BASE ERRORS
    public class Error : Exception
    {
        public Error(string Message)
            : base(Message)
        {
        }

        public virtual bool MustThrow
        {
            get
            {
                return true;
            }
        }

        public Error()
        {
        }
    }

    public class LocatedError : Error
    {
        protected SourceContext source_context = null;
        protected SourceLocation sourceLocation = null;
        public string fileName = null;
        public LocatedError(string Message, string fileName)
            : base(Message)
        {
            this.fileName = fileName;
        }

        public LocatedError(string Message)
            : base(Message)
        {
        }

        public LocatedError()
        {
        }

        public override string ToString()
        {
            /*string pos=null;
            if (source_context != null)
                pos = "(" + source_context.begin_position.line_num + "," + source_context.begin_position.column_num + ")";
            string fn = null;
            if (file_name != null)
                fn = Path.GetFileName(file_name);
            return string.Format(StringResources.Get("PARSER_ERRORS_COMPILATION_ERROR{0}{1}{2}"), fn, pos, Message);
             */
            return (new CompilerInternalError("Errors.ToString", new Exception(string.Format("Не переопеределена {0}.ToString", this.GetType())))).ToString();
        }

        public virtual SourceLocation SourceLocation
        {
            get
            {
                if (sourceLocation != null)
                    return sourceLocation;
                if (SourceContext != null)
                    return new SourceLocation(FileName, SourceContext.begin_position.line_num, SourceContext.begin_position.column_num, SourceContext.end_position.line_num, SourceContext.end_position.column_num);
                return null;
            }
        }


        public virtual SourceContext SourceContext
        {
            get
            {
                return source_context;
            }
        }

        public virtual string FileName
        {
            get
            {
                return fileName;
            }
        }
    }

    public class CommonCompilerError : LocatedError
    {
        public CommonCompilerError(string mes, string fileName, int line, int col) : base(mes, fileName)
        {
            this.sourceLocation = new SourceLocation(fileName, line, col, line, col);
            this.source_context = new SyntaxTree.SourceContext(line, col, line, col);
        }
        public override string ToString()
        {
            return string.Format("{0} ({1},{2}): {3}", Path.GetFileName(sourceLocation.FileName), sourceLocation.BeginPosition.Line, sourceLocation.BeginPosition.Column, this.Message);
        }
    }

    public class CompilerWarning : LocatedError
    {
        public CompilerWarning() { }

        public CompilerWarning(string Message, string fileName)
            : base(Message)
        {
            this.fileName = fileName;
        }

        public override string Message
        {
            get
            {
                return (this.ToString());
            }
        }
    }

    public class CommonWarning : CompilerWarning
    {
        string _mes;

        public CommonWarning(string mes, string fileName, int line, int col) : base(mes, fileName)
        {
            this.sourceLocation = new SourceLocation(fileName, line, col, line, col);
            this.source_context = new SyntaxTree.SourceContext(line, col, line, col);
            _mes = mes;
        }

        public override string Message
        {
            get
            {
                return _mes;
            }
        }
    }

    public class SyntaxError : LocatedError
    {
        public syntax_tree_node bad_node;
        public SyntaxError(string Message, string fileName, PascalABCCompiler.SyntaxTree.SourceContext _source_context, PascalABCCompiler.SyntaxTree.syntax_tree_node _bad_node)
            : base(Message, fileName)
        {
            source_context = _source_context;
            if (source_context != null && source_context.FileName != null)
                base.fileName = source_context.FileName;
            if (source_context == null && _bad_node != null) // искать source_context у Parentов
            {
                var bn = _bad_node;

                do
                {
                    bn = bn.Parent;
                    if (bn != null && bn.source_context != null)
                    {
                        source_context = bn.source_context;
                        if (source_context.FileName != null)
                            base.fileName = source_context.FileName;
                        break;
                    }
                } while (bn != null);

            }
            bad_node = _bad_node;
        }
        public SyntaxError(string Message, string fileName, PascalABCCompiler.SyntaxTree.syntax_tree_node bad_node)
            : this(Message, fileName, bad_node.source_context, bad_node) { }
        public SyntaxError(string Message, PascalABCCompiler.SyntaxTree.syntax_tree_node bad_node)
            : this(Message, null, bad_node.source_context, bad_node) { }

        public override string ToString()
        {
            string snode = "";
            /*if (bad_node == null)
                snode = " (bad_node==null)";
            else
                snode = " (bad_node==" + bad_node.ToString() + ")";*/
            string pos;
            if (source_context == null)
                pos = "(?,?)";
            else
                pos = "(" + source_context.begin_position.line_num + "," + source_context.begin_position.column_num + ")";
            return Path.GetFileName(FileName) + pos + ": Синтаксическая ошибка :  " + Message + snode;
        }

        public override SourceContext SourceContext
        {
            get
            {
                return source_context;
            }
        }
        public override SourceLocation SourceLocation
        {
            get
            {
                if (source_context == null)
                    return null;
                return new SourceLocation(fileName,
                    source_context.begin_position.line_num, source_context.begin_position.column_num,
                    source_context.end_position.line_num, source_context.end_position.column_num);
            }
        }

    }

    public class SemanticError : LocatedError
    {
        public SemanticError(string Message, string fileName)
            : base(Message, fileName)
        {
            this.fileName = fileName;
        }

        public virtual SemanticTree.ILocation Location
        {
            get
            {
                return null;
            }
        }

        public SemanticError()
        {
        }
        public override string ToString()
        {
            return String.Format(StringResources.Get("COMPILATIONERROR_UNDEFINED_SEMANTIC_ERROR{0}"), this.GetType().ToString());
        }
        public override SourceLocation SourceLocation
        {
            get
            {
                if (sourceLocation != null)
                    return sourceLocation;
                if (Location != null)
                {
                    return new SourceLocation(Location.document.file_name,
                        Location.begin_line_num, Location.begin_column_num, Location.end_line_num, Location.end_column_num);
                }
                return null;
            }
        }
        public override string Message
        {
            get
            {
                return (this.ToString());
            }
        }

    }

    public class SemanticNonSupportedError : SemanticError
    {
        public SemanticNonSupportedError(string fileName)
            : base("", fileName)
        {
        }

    }
    #endregion

    // Ошибки, бросаемые в компиляторе
    #region ERRORS THROWN BY COMPILER

    /// <summary>
    /// Внутренняя ошибка компилятора (нештатная)
    /// </summary>
    public class CompilerInternalError : Error
    {
        public Exception exception = null;
        public string Module;
        public CompilerInternalError(string module, Exception exc)
            : base(exc.ToString())
        {
            Module = module;
            exception = exc;
        }
        public override string ToString()
        {
            return string.Format(StringResources.Get("COMPILER_INTERNAL_ERROR_IN_UNIT_{0}_:{1}"), Module, Message + ' ' + exception.ToString());
        }
    }

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

    public class ReadPCUError : CompilerThrownError
    {
        public ReadPCUError(string FileName)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_READ_PCU{0}_ERROR"), FileName))
        {
        }
    }

    public class NamespaceCannotHaveInSection : CompilerThrownError
    {
        public NamespaceCannotHaveInSection(SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_NAMESPACE_CANNOT_HAVE_IN_SECTION")))
        {
            this.source_context = sc;
        }
    }

    public class ProgramModuleExpected : CompilerThrownError
    {
        public ProgramModuleExpected(string FileName, SyntaxTree.SourceContext sc)
            : base(StringResources.Get("COMPILATIONERROR_PROGRAM_MODULE_EXPECTED"), FileName)
        {
            this.source_context = sc;
        }
    }

    public class UnitModuleExpected : CompilerThrownError
    {
        public UnitModuleExpected(string FileName, SyntaxTree.SourceContext sc)
            : base(StringResources.Get("COMPILATIONERROR_UNIT_MODULE_EXPECTED"), FileName)
        {
            this.source_context = sc;
        }
    }

    public class AppTypeDllIsAllowedOnlyForLibraries : CompilerThrownError
    {
        public AppTypeDllIsAllowedOnlyForLibraries(string FileName, SyntaxTree.SourceContext sc)
            : base(StringResources.Get("COMPILATIONERROR_APPTYPE_DLL_IS_ALLOWED_ONLY_FOR_LIBRARIES"), FileName)
        {
            this.source_context = sc;
        }
    }

    public class UnitModuleExpectedLibraryFound : CompilerThrownError
    {
        public UnitModuleExpectedLibraryFound(string FileName, SyntaxTree.SourceContext sc)
            : base(StringResources.Get("COMPILATIONERROR_UNIT_MODULE_EXPECTED_LIBRARY_FOUND"), FileName)
        {
            this.source_context = sc;
        }
    }

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

    public class InvalidAssemblyPathError : CompilerThrownError
    {
        public InvalidAssemblyPathError(string FileName, SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_INVALID_ASSEMBLY_PATH")), FileName)
        {
            this.source_context = sc;
        }
    }

    public class InvalidPathError : CompilerThrownError
    {
        public InvalidPathError(SyntaxTree.SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_INVALID_PATH")))
        {
            this.source_context = sc;
            this.fileName = sc.FileName;
        }
    }

    public class ResourceFileNotFound : CompilerThrownError
    {
        public ResourceFileNotFound(string fileName, string ResFileName, SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_RESOURCEFILE_{0}_NOT_FOUND"), ResFileName), fileName)
        {
            source_context = sc;
        }
    }

    public class IncludeNamespaceInUnitError : CompilerThrownError
    {
        public IncludeNamespaceInUnitError(string FileName, SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_INCLUDE_NAMESPACE_IN_UNIT")), FileName)
        {
            this.source_context = sc;
        }
    }

    public class NamespaceModuleExpected : CompilerThrownError
    {
        public NamespaceModuleExpected(SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_NAMESPACE_MODULE_EXPECTED")))
        {
            this.source_context = sc;
        }
    }

    public class MainResourceNotAllowed : CompilerThrownError
    {
        public MainResourceNotAllowed(string fileName, SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_MAINRESOURCE_NOT_ALLOWED")), fileName)
        {
            source_context = sc;
        }

    }

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

    public class NamespacesCanBeCompiledOnlyInProjects : CompilerThrownError
    {
        public NamespacesCanBeCompiledOnlyInProjects(SyntaxTree.SourceContext sc)
            : base(StringResources.Get("COMPILATIONERROR_NAMESPACE_CAN_BE_COMPILED_ONLY_IN_PROJECTS"))
        {
            this.source_context = sc;
        }
    }

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

    public class SourceFileNotFound : CompilerThrownError
    {
        public SourceFileNotFound(string FileName)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_SOURCE_FILE_{0}_NOT_FOUND"), FileName))
        {
        }
    }

    public class UnauthorizedAccessToFile : CompilerThrownError
    {
        public UnauthorizedAccessToFile(string FileName)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_NO_ACCESS_TO_FILE{0}"), FileName))
        {
        }
    }
    public class CycleUnitReference : CompilerThrownError
    {
        public CycleUnitReference(string FileName, SyntaxTree.unit_or_namespace SyntaxUsesUnit)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_CYCLIC_UNIT_REFERENCE_WITH_UNIT_{0}"), SyntaxTree.Utils.IdentListToString(SyntaxUsesUnit.name.idents, ".")), FileName)
        {
            this.source_context = SyntaxUsesUnit.source_context;
        }
    }

    public class UnsupportedTargetFramework : CompilerThrownError
    {
        public UnsupportedTargetFramework(string FrameworkName, SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_UNSUPPORTED_TARGETFRAMEWORK_{0}"), FrameworkName))
        {
            this.source_context = sc;
        }
    }

    public class UnsupportedTargetPlatform : CompilerThrownError
    {
        public UnsupportedTargetPlatform(string platformName, SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_UNSUPPORTED_TARGET_PLATFORM{0}"), platformName))
        {
            source_context = sc;
        }
    }

    public class UnsupportedOutputFileType : CompilerThrownError
    {
        public UnsupportedOutputFileType(string outputFileType, SourceContext sc)
            : base(string.Format(StringResources.Get("COMPILATIONERROR_UNSUPPORTED_OUTPUT_FILE_TYPE{0}"), outputFileType))
        {
            source_context = sc;
        }
    }

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
    #endregion
}
