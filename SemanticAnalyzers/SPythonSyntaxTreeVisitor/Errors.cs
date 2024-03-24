using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PascalABCCompiler.Errors;
using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.TreeRealization;

namespace SPythonSyntaxTreeVisitor
{
    class SPythonSemanticError : SimpleSemanticError
    {
        public SPythonSemanticError(location loc, string ErrResourceString, params object[] values) : base(loc, ErrResourceString, values) { }
        public override string ToString()
        {
            if (values != null)
                return string.Format(PascalABCCompiler.StringResources.Get(ErrorResourceString), values);
            else
                return string.Format(PascalABCCompiler.StringResources.Get(ErrorResourceString), "");
        }
    }
}
