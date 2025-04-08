// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.SemanticTreeConverters;
using PascalABCCompiler.SyntaxTreeConverters;
using PascalABCCompiler.Errors;

///В разработке DarkStar

namespace PascalABCCompiler
{
    public enum CompilerType { Standart, Remote }
    public interface ICompiler
    {
        /// Здоровье кода на всякий случай выносим в интерфейс компилятора
        /// Реально оно будет использоваться только при запуске из под оболочки (Remote Compiler)
        int PABCCodeHealth  
        {
            get;
        }

        /*SyntaxTreeConvertersController SyntaxTreeConvertersController
        {
            get; 
        }*/
        SemanticTreeConvertersController SemanticTreeConvertersController
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
            get; set;
        }

        event ChangeCompilerStateEventDelegate OnChangeCompilerState;

        void Free();

        void Reload();

        string Compile();

        void StartCompile();

        void AddWarnings(List<CompilerWarning> WarningList);

        SyntaxTree.compilation_unit ParseText(string FileName, string Text, List<Error> ErrorList, List<CompilerWarning> Warnings);

        string GetSourceFileText(string FileName);

        CompilerType CompilerType
        {
            get;
        }


    }

}
