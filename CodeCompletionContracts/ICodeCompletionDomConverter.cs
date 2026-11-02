namespace PascalABCCompiler.Contracts.CodeCompletion
{
    public interface ICodeCompletionDomConverter
    {
        bool IsCompiled
        {
            get;
        }
        IBaseScope EntryScope
        {
            get;
        }
    }
}
