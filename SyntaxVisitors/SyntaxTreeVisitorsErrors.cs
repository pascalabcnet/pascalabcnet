// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;

namespace SyntaxVisitors
{
    public static class StringResources
    {
        public static string Get(string key)
        {
            return PascalABCCompiler.StringResources.Get("SYNTAXTREEVISITORSERROR_" + key);
        }

        public static string Get(string key, params object[] values)
        {
            return (string.Format(Get(key), values));
        }
    }

    public class SyntaxVisitorError: SyntaxError
    {
        public SyntaxVisitorError(string resourcestring, SourceContext sc, params object[] values): base(StringResources.Get(resourcestring),"",sc,null)
        {

        }
    }
}
