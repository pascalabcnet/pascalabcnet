using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using VisualPascalABC;

namespace LanguageServerEngine
{
    internal class CompletionHandler : ICompletionHandler
    {
        private readonly DocumentStorage documentStorage;

        private CompletionCapability capability;

        private string[] specialTriggerCharacters = new string[] { ".", "(" };

        private CodeCompletionProvider completionProvider;

        public CompletionHandler(DocumentStorage documentStorage)
        {
            completionProvider = new CodeCompletionProvider();
            this.documentStorage = documentStorage;
        }

        public CompletionRegistrationOptions GetRegistrationOptions()
        {
            return new CompletionRegistrationOptions
            {
                DocumentSelector = BaseConfiguration.TextDocumentSelector,
                TriggerCharacters = specialTriggerCharacters,
                ResolveProvider = false
            };
        }

        public Task<CompletionList> Handle(TextDocumentPositionParams request, CancellationToken cancellationToken)
        {

            var documentPath = request.TextDocument.Uri.ToString();

            Position pos = request.Position;

            var docText = documentStorage.GetDocument(documentPath).ToString();

            var changedLineText = LspDataConvertor.GetTextInRange(docText, new Range()
            {
                Start = new Position(pos.Line, 0),
                End = pos
            });

            string triggerChar = changedLineText.Length > 0 ? changedLineText[changedLineText.Length - 1].ToString() : "";

            UserDefaultCompletionData[] completionList = null;

            try
            {
                if (specialTriggerCharacters.Contains(triggerChar))
                {
                    completionList = completionProvider.GenerateCompletionDataWithKeyword(documentPath, docText,
                        LspDataConvertor.GetOffsetFromPosition(documentStorage.GetDocument(documentPath), pos) - 1,
                        (int)pos.Line, (int)pos.Character, triggerChar[0], PascalABCCompiler.Parsers.KeywordKind.None);
                }
                else
                {
                    
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Intellisense exception:");
                Console.Error.WriteLine(e.Message + "\n" + e.StackTrace);
            }

            if (completionList != null)
            {
                Console.Error.WriteLine("completions got");
                return Task.FromResult(new CompletionList(completionList.Select(item => new CompletionItem() { Label = item.Text, Detail = item.Description })));
            }
            else
            {
                return Task.FromResult(new CompletionList(new CompletionItem() { Label = "Console" }));
            }
        }

        public void SetCapability(CompletionCapability capability)
        {
            this.capability = capability;
        }
    }
}
