using System;
using System.Threading;
using System.Threading.Tasks;

namespace PascalABCCompiler.Contracts.CodeCompletion
{
    public interface IInlineCompletionService
    {
        event Action OnReady;
        bool Ready { get; }
        void RegisterProvider(IInlineCompletionProvider provider);
        Task<string> TriggerCompletionAsync(CodeCompletionContext context, CancellationToken cancellationToken);
    }
}
