// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System.Collections.Generic;

namespace PascalABCCompiler.Errors
{
    public enum ErrorsStrategy { All=0, FirstOnly=1, FirstSemanticAndSyntax=2 };
    public class ErrorsStrategyManager
    {
        public ErrorsStrategy Strategy;

        public ErrorsStrategyManager(ErrorsStrategy Strategy)
        {
            this.Strategy = Strategy;
        }

        public List<PascalABCCompiler.Errors.Error> CreateErrorsList(List<PascalABCCompiler.Errors.Error> CompilerErrorsList)
        {
            List<PascalABCCompiler.Errors.Error> ErrorsList = new List<PascalABCCompiler.Errors.Error>();
            switch (Strategy)
            {
                case ErrorsStrategy.All:
                    ErrorsList = CompilerErrorsList;
                    break;
                case ErrorsStrategy.FirstOnly:
                    if (CompilerErrorsList.Count > 0)
                        ErrorsList.Add(CompilerErrorsList[0]);
                    break;
                case ErrorsStrategy.FirstSemanticAndSyntax:
                    if (CompilerErrorsList.Count > 0)
                    {
                        ErrorsList.Add(CompilerErrorsList[0]);
                        bool syntax_add = ErrorsList[0] is PascalABCCompiler.Errors.SyntaxError;
                        bool semantic_add = ErrorsList[0] is PascalABCCompiler.Errors.SemanticError;
                        int i = 0;
                        while (i < CompilerErrorsList.Count && (!syntax_add || !semantic_add))
                        {
                            if (!syntax_add && CompilerErrorsList[i] is PascalABCCompiler.Errors.SyntaxError)
                            {
                                ErrorsList.Add(CompilerErrorsList[i]);
                                syntax_add = true;
                            }
                            if (!semantic_add && CompilerErrorsList[i] is PascalABCCompiler.Errors.SemanticError)
                            {
                                ErrorsList.Add(CompilerErrorsList[i]);
                                semantic_add = true;
                            }
                            i++;
                        }
                    }
                    break;                            
            }
            return ErrorsList;
        }
        

    }
}
