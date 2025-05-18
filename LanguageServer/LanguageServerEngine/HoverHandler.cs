// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol;
using System;
using System.Threading;
using System.Threading.Tasks;
using CodeCompletion;

namespace LanguageServerEngine
{
    internal class HoverHandler : IHoverHandler
    {
        private readonly DocumentStorage documentStorage;

        private HoverCapability capability;

        public HoverHandler(DocumentStorage documentStorage)
        {
            this.documentStorage = documentStorage;
        }

        Task<Hover> IRequestHandler<TextDocumentPositionParams, Hover>.Handle(TextDocumentPositionParams request, CancellationToken token)
        {
            var documentPath = LspDataConvertor.GetNormalizedPathFromUri(request.TextDocument.Uri);

            Position pos = request.Position;

            var docText = documentStorage.GetDocumentText(documentPath);

            var buffer = documentStorage.GetDocumentBuffer(documentPath);

            int offset = buffer.GetOffsetFromPosition((int)pos.Line, (int)pos.Character);

            string hintText = null;

            try
            {
                hintText = TooltipServiceManager.GetPopupHintText(offset, (int)pos.Line, (int)pos.Character, docText, documentPath);
            }
            catch (Exception) { }
            
            if (hintText != null)
            {
                var markedContent = new MarkupContent()
                {
                    Kind = MarkupKind.Plaintext,
                    Value = hintText
                };

                return Task.FromResult(
                    new Hover()
                    {
                        Contents = new MarkedStringsOrMarkupContent(markedContent),
                        Range = LspDataConvertor.GetRangeForSymbolAtPos(buffer.GetLine((int)pos.Line), pos)
                    }
                    );
            }

            return Task.FromResult(new Hover());
        }

        TextDocumentRegistrationOptions IRegistration<TextDocumentRegistrationOptions>.GetRegistrationOptions()
        {
            return new TextDocumentRegistrationOptions()
            {
                DocumentSelector = BaseConfiguration.TextDocumentSelector
            };
        }

        public void SetCapability(HoverCapability capability)
        {
            this.capability = capability;
        }

    }
}
