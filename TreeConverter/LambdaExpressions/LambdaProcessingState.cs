// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
namespace TreeConverter.LambdaExpressions
{
    public enum LambdaProcessingState
    {
        None,
        TypeInferencePhase,
        ClosuresProcessingPhase,
        ClosuresProcessingVisitGeneratedClassesPhase,
        FinishPhase
    }
}
