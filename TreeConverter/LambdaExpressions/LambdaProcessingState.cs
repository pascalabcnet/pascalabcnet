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
