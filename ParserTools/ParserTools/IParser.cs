// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;

namespace PascalABCCompiler.Parsers
{
    public enum ParseMode { Normal, Expression, Statement, Special, ForFormatter, TypeAsExpression };
    public interface IParser
    {
        List<Error> Errors
        {
            get;
            set;
        }

        List<CompilerWarning> Warnings
        {
            get;
            set;
        }

        List<compiler_directive> CompilerDirectives
        {
            get;
        }

        /*SourceFilesProviderDelegate SourceFilesProvider
        {
            get;
            set;
        }*/

        ILanguageInformation LanguageInformation
        {
        	get;
        }
        syntax_tree_node BuildTree(string FileName, string Text, ParseMode ParseMode, List<string> DefinesList = null);

        void Reset();

    }

    
    
}
