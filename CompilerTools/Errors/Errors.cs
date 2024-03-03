// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.IO;
using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.Errors
{
	
    public class SemanticError : LocatedError
    {
        public SemanticError(string Message, string fileName)
            : base(Message,fileName)
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
    
    public class SemanticErrorFixed : SemanticError { }
    
    
	
   



}
