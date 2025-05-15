using CodeCompletion;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisualPascalABC;

namespace LanguageServerEngine
{
    internal class SignatureHelpHandler : ISignatureHelpHandler
    {
        private SignatureHelpCapability capability;

        private readonly DocumentStorage documentStorage;
        
        public SignatureHelpHandler(DocumentStorage documentStorage)
        {
            this.documentStorage = documentStorage;
        }

        public SignatureHelpRegistrationOptions GetRegistrationOptions()
        {
            return new SignatureHelpRegistrationOptions()
            {
                DocumentSelector = BaseConfiguration.TextDocumentSelector,
                TriggerCharacters = new string[] { "(", "[", "," }
            };
        }

        public Task<SignatureHelp> Handle(TextDocumentPositionParams request, CancellationToken token)
        {
            var documentPath = LspDataConvertor.GetNormalizedPathFromUri(request.TextDocument.Uri);

            Position pos = request.Position;

            var docText = documentStorage.GetDocumentText(documentPath);

            var buffer = documentStorage.GetDocumentBuffer(documentPath);

            var changedLineText = buffer.GetLine((int)pos.Line);
            
            char triggerChar = changedLineText.Substring(0, (int)pos.Character).Last();

            var insightProvider = new DefaultInsightDataProvider(-1, triggerChar);

            int symbolIndex = buffer.GetOffsetFromPosition((int)pos.Line, (int)pos.Character) - 1;

            try
            {
                insightProvider.SetupDataProvider(documentPath, docText, symbolIndex, (int)pos.Line, (int)pos.Character);
            }
            catch (Exception) { }

            if (insightProvider.methods == null)
            {
                return Task.FromResult(new SignatureHelp());
            }
            
            var resultMethods = new List<SignatureInformation>();

            foreach (var description in insightProvider.methods)
            {
                int index = description.IndexOf('\n');

                string label = description.Substring(0, index != -1 ? index : description.Length);

                string documentation = index != -1 ? description.Substring(index + 1) : null;

                string[] parameters;

                // подсказка для индексов
                if (triggerChar == '[')
                {
                    int startIndex = label.IndexOf('[') + 1;

                    parameters = label.Substring(startIndex, label.Substring(startIndex).IndexOf("]"))
                        .Split(new string[] { CodeCompletionController.CurrentParser.LanguageInformation.ParameterDelimiter }, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    // поиск с индекса 1 для учета обозначения расширения
                    int startIndex = label.Substring(1).IndexOf("(") + 2;

                    parameters = label.Substring(startIndex, label.Substring(startIndex).IndexOf(")"))
                        .Split(new string[] { CodeCompletionController.CurrentParser.LanguageInformation.ParameterDelimiter }, StringSplitOptions.RemoveEmptyEntries);
                }

                // Проверка нужна, потому что иначе Documentation сформируется неправильно при присвоении null
                if (documentation != null)
                {
                    var info = new SignatureInformation()
                    {
                        Documentation = documentation,
                        Label = label,
                        Parameters = parameters.Select(param => new ParameterInformation() { Label = param }).ToArray()
                    };
                    resultMethods.Add(info);
                }
                else
                {
                    var info = new SignatureInformation()
                    {
                        Label = label,
                        Parameters = parameters.Select(param => new ParameterInformation() { Label = param }).ToArray()
                    };
                    resultMethods.Add(info);
                }
            }

            int activeSignature = insightProvider.DefaultIndex;

            int activeParameter = -1;

            var paramsOfActiveSignature = resultMethods[activeSignature].Parameters;

            if (paramsOfActiveSignature.Count() > 0)
            {
                var lastParam = resultMethods[activeSignature].Parameters.Last().Label;

                // params случай
                if (lastParam.EndsWith("...") || lastParam.TrimStart().StartsWith("params"))
                {
                    activeParameter = Math.Min(insightProvider.num_param, paramsOfActiveSignature.Count()) - 1;
                }
                else
                {
                    activeParameter = insightProvider.num_param <= paramsOfActiveSignature.Count() ? insightProvider.num_param - 1 : -1;
                }
            }


            return Task.FromResult(new SignatureHelp()
                {
                    ActiveParameter = activeParameter,
                    ActiveSignature = activeSignature,
                    Signatures = resultMethods
                }
            );

        }

        public void SetCapability(SignatureHelpCapability capability)
        {
            this.capability = capability;
        }
    }
}
