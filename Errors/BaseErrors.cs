// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.IO;
using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.Errors
{
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
}
