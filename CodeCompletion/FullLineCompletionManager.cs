using System;
using System.Threading;
using System.Threading.Tasks;
using PascalABCCompiler.Contracts;

namespace CodeCompletion
{
    public sealed class FullLineCompletionManager : IInlineCompletionService
    {
        public event Action OnReady;
        private IInlineCompletionProvider provider;

        private const int COMPLETION_REQUEST_DELAY = 250;

        public bool Ready => provider != null;

        public void RegisterProvider(IInlineCompletionProvider provider)
        {
            var previousProvider = this.provider;

            this.provider = provider;

            if (previousProvider == null)
                OnReady?.Invoke();
        }

        public async Task<string> TriggerCompletionAsync(CodeCompletionContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(COMPLETION_REQUEST_DELAY, cancellationToken);

            if (provider == null || !provider.IsEnabled)
                return string.Empty;

            return await provider.GetCompletionAsync(context, cancellationToken);
        }
    }
}

