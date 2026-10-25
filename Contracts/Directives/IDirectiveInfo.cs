using System;
using System.Collections.Generic;

namespace PascalABCCompiler.Contracts
{
    public interface IDirectiveInfo
    {
        DirectiveProcessingStage ProcessingStage { get; }

        bool QuotesAreSpecialSymbols { get; }

        bool CheckParamsNumNeeded { get; }

        int[] ParamsNums { get; }

        bool ParamsValid(List<string> directiveParams, out int indexOfMismatch, out string errorMessage);
    }
}
