using System;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;

namespace PascalABCCompiler.Parsers
{
    public interface IPreprocessor
    {
        List<compiler_directive> CompilerDirectives
        {
            get;
        }
        /*SourceFilesProviderDelegate SourceFilesProvider
        {
            get;
            set;
        }*/
        //string Build(string[] fileNames, List<Error> errors, ParserTools.SourceContextMap sourceContextMap);
    }
}
