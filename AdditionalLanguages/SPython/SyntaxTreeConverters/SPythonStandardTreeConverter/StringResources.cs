using PascalABCCompiler.Errors;
using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Languages.SPython.Frontend.Converters
{
    public static class SPythonStringResources
    {
        public static string Get(string key)
        {
            return PascalABCCompiler.StringResources.Get("SPYTHONSYNTAXTREEVISITORSERROR_" + key);
        }

        public static string Get(string key, params object[] values)
        {
            return (string.Format(Get(key), values));
        }
    }

    public class SPythonSyntaxVisitorError : SyntaxError
    {
        public SPythonSyntaxVisitorError(string resourcestring, SourceContext sc, params object[] values) : base(SPythonStringResources.Get(resourcestring, values), "", sc, null)
        {

        }
        public SPythonSyntaxVisitorError(string resourcestring, PascalABCCompiler.SyntaxTree.syntax_tree_node bad_node)
            : base(SPythonStringResources.Get(resourcestring), "", bad_node.source_context, bad_node)
        {

        }
    }
}
