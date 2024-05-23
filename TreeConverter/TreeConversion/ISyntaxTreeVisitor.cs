using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PascalABCCompiler.TreeConverter.TreeConversion
{
    public interface ISyntaxTreeVisitor
    {
        string[] FilesExtensions { get; }
    }
}
