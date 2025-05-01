using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System.Threading;
using System.Threading.Tasks;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;

namespace LanguageServerEngine
{
    internal class CompletionHandler : ICompletionHandler
    {
        private readonly DocumentStorage documentStorage;

        public CompletionHandler(ILanguageServerFacade router, DocumentStorage documentStorage)
        {
            this.documentStorage = documentStorage;
        }

        public CompletionRegistrationOptions GetRegistrationOptions(CompletionCapability capability, ClientCapabilities clientCapabilities)
        {
            return new CompletionRegistrationOptions
            {
                DocumentSelector = BaseConfiguration.TextDocumentSelector,
                ResolveProvider = false
            };
        }

        public Task<CompletionList> Handle(CompletionParams request, CancellationToken cancellationToken)
        {

            var testCompletionItem = new CompletionItem() { Label = "Console" };

            

            return Task.FromResult(new CompletionList(testCompletionItem));
        }
    }
}
