namespace PascalABCCompiler.Contracts
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
