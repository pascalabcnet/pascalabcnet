// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using System;

namespace PascalABCCompiler.Parsers
{
    public enum ParseMode { Normal, Expression, Statement, Special, ForFormatter, TypeAsExpression };
    public interface IParser
    {
        List<Error> Errors { get; }

        List<CompilerWarning> Warnings { get; }

        List<compiler_directive> CompilerDirectives
        {
            get;
        }

        ILanguageInformation LanguageInformation { get; set; }

        compilation_unit GetCompilationUnit(string FileName, string Text, List<Error> Errors, List<CompilerWarning> Warnings, ParseMode parseMode, bool compilingNotMainProgram, List<string> DefinesList = null);

        compilation_unit GetCompilationUnitForFormatter(string FileName, string Text, List<Error> Errors, List<CompilerWarning> Warnings);

        expression GetExpression(string FileName, string Text, List<Error> Errors, List<CompilerWarning> Warnings);

        statement GetStatement(string FileName, string Text, List<Error> Errors, List<CompilerWarning> Warnings);

        expression GetTypeAsExpression(string FileName, string Text, List<Error> Errors, List<CompilerWarning> Warnings);
    }

    
    
}
