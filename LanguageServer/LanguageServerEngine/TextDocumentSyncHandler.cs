// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using CodeCompletion;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageServerEngine
{
    internal class TextDocumentSyncHandler : ITextDocumentSyncHandler
    {

        private SynchronizationCapability capability;

        private readonly DocumentStorage documentStorage;

        private readonly CodeCompletionParserController codeCompletionController;

        private bool syncKindChosen = false;

        public TextDocumentSyncHandler(DocumentStorage documentStorage)
        {
            this.documentStorage = documentStorage;

            codeCompletionController = new CodeCompletionParserController(documentStorage);

            codeCompletionController.SwitchOnIntellisense();
        }

        public TextDocumentSyncKind Change { get; } = TextDocumentSyncKind.Incremental;

        public TextDocumentSyncOptions Options => new TextDocumentSyncOptions() { OpenClose = true, Change = Change };

        public TextDocumentAttributes GetTextDocumentAttributes(Uri uri)
        {
            return new TextDocumentAttributes(uri, PascalABCCompiler.StringConstants.pascalLanguageName);
        }

        public Task Handle(DidChangeTextDocumentParams request)
        {
            string documentPath = LspDataConvertor.GetNormalizedPathFromUri(request.TextDocument.Uri);

            // используется ли инкрементальный режим клиентом
            if (!syncKindChosen)
            {
                if (request.ContentChanges.First().Range != null)
                    documentStorage.SwitchOnIncrementalMode();

                syncKindChosen = true;
            }

            foreach (var contentChange in request.ContentChanges)
            {
                documentStorage.UpdateDocument(documentPath, contentChange);
            }

            codeCompletionController.SetAsChanged(documentPath);

            return Task.CompletedTask;
        }

        public Task Handle(DidOpenTextDocumentParams request)
        {
            string documentPath = LspDataConvertor.GetNormalizedPathFromUri(request.TextDocument.Uri);

            documentStorage.AddDocument(documentPath, request.TextDocument.Text);

            codeCompletionController.RegisterFileForParsing(documentPath);

            return Task.CompletedTask;
        }

        public Task Handle(DidCloseTextDocumentParams request)
        {
            string documentPath = LspDataConvertor.GetNormalizedPathFromUri(request.TextDocument.Uri);

            documentStorage.RemoveDocument(documentPath);

            codeCompletionController.CloseFile(documentPath);

            return Task.CompletedTask;
        }

        public Task Handle(DidSaveTextDocumentParams request)
        {
            return Task.CompletedTask;
        }

        TextDocumentRegistrationOptions IRegistration<TextDocumentRegistrationOptions>.GetRegistrationOptions()
        {
            return new TextDocumentRegistrationOptions()
            {
                DocumentSelector = BaseConfiguration.TextDocumentSelector
            };
        }

        TextDocumentSaveRegistrationOptions IRegistration<TextDocumentSaveRegistrationOptions>.GetRegistrationOptions()
        {
            return new TextDocumentSaveRegistrationOptions()
            {
                DocumentSelector = BaseConfiguration.TextDocumentSelector
            };
        }

        public TextDocumentChangeRegistrationOptions GetRegistrationOptions()
        {
            return new TextDocumentChangeRegistrationOptions()
            {
                DocumentSelector = BaseConfiguration.TextDocumentSelector,
                SyncKind = Change
            };
        }

        public void SetCapability(SynchronizationCapability capability)
        {
            this.capability = capability;
        }
    }
}
