// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.SemanticTreeConverters;
using PascalABCCompiler.Errors;

///В разработке DarkStar

namespace PascalABCCompiler
{
    public enum CompilerType { Standart, Remote }
    public interface ICompiler
    {
        SemanticTreeConvertersController SemanticTreeConvertersController
        {
            get;
        }
        
        Parsers.Controller ParsersController
        {
            get;
        }

        SemanticTree.IProgramNode SemanticTree
        {
            get;
        }
        
        uint LinesCompiled
        {
            get;
        }

        CompilerInternalDebug InternalDebug
        {
            get;
            set;
        }

        CompilerState State
        {
            get;
        }

        SupportedSourceFile[] SupportedSourceFiles
        {
            get;
        }
		
        SupportedSourceFile[] SupportedProjectFiles
        {
        	get;
        }
        
        CompilerOptions CompilerOptions
        {
            get;
            set;
        }

        List<Errors.Error> ErrorsList
        {
            get;
        }

        List<CompilerWarning> Warnings
        {
            get;
        }

        int BeginOffset
        {
            get;
        }
        
        int VarBeginOffset
        {
        	get;
        }
        
        CompilationUnitHashTable UnitTable
        {
            get;
        }

        SourceFilesProviderDelegate SourceFilesProvider
        {
            get;
        }

        event ChangeCompilerStateEventDelegate OnChangeCompilerState;

        void Free();

        void Reload();

        string Compile();

        void StartCompile();

        void AddWarnings(List<CompilerWarning> WarningList);

        SyntaxTree.compilation_unit ParseText(string FileName, string Text, List<Error> ErrorList);

        string GetSourceFileText(string FileName);

        CompilerType CompilerType
        {
            get;
        }


    }

}
