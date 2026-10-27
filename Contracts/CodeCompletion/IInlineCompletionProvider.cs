using System.Threading;
using System.Threading.Tasks;

namespace PascalABCCompiler.Contracts
{
    public interface IInlineCompletionProvider
    {
        bool IsEnabled { get; }
        Task<string> GetCompletionAsync(CodeCompletionContext context, CancellationToken cancellationToken);
    }
}
