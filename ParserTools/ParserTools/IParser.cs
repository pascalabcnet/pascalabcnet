using System;
using System.Text;
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
        
        List<compiler_directive> CompilerDirectives
        {
            get;
        }

        

        bool CaseSensitive
        {
            get;
        }

        string[] FilesExtensions
        {
            get;
        }

        string Name
        {
            get;
        }

        string Version
        {
            get;
        }
        string Copyright
        {
            get;
        }
        SourceFilesProviderDelegate SourceFilesProvider
        {
            get;
            set;
        }
        ILanguageInformation LanguageInformation
        {
        	get;
        }
        /*ICodeFormatter CodeFormatter
        {
            get;
        }*/
        syntax_tree_node BuildTree(string FileName, string Text, ParseMode ParseMode);

        void Reset();

        IPreprocessor Preprocessor
        {
            get;
        }
    }

    
    
}
