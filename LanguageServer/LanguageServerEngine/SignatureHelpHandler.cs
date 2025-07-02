// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeCompletion;

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

            var textInfoProvider = documentStorage.GetTextInfoProvider(documentPath);

            char triggerChar = textInfoProvider.GetSymbolAtPos((int)pos.Line, (int)pos.Character - 1);

            var insightProvider = new DefaultInsightDataProvider(-1, triggerChar);

            int symbolIndex = textInfoProvider.GetOffsetFromPosition((int)pos.Line, (int)pos.Character) - 1;

            try
            {
                insightProvider.SetupDataProvider(documentPath, docText, symbolIndex, (int)pos.Line, (int)pos.Character);
            }
            catch (Exception) { }

            if (insightProvider.methods == null)
            {
                return Task.FromResult(new SignatureHelp());
            }

            List<SignatureInformation> resultMethods = LspDataConvertor.GetLspMethodsInfoFromStringDescriptions(insightProvider.methods, triggerChar == '[');

            int activeSignature = insightProvider.DefaultIndex;

            int activeParameter = -1;

            var paramsOfActiveSignature = resultMethods[activeSignature].Parameters;

            if (paramsOfActiveSignature.Count() > 0)
            {
                var lastParam = resultMethods[activeSignature].Parameters.Last().Label;

                // params случай
                if (CodeCompletionController.CurrentParser.LanguageInformation.IsParams(lastParam))
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
