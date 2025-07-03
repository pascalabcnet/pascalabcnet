// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeCompletion;

namespace LanguageServerEngine
{
    internal class CompletionHandler : ICompletionHandler
    {
        private readonly DocumentStorage documentStorage;

        private CompletionCapability capability;

        private readonly string[] specialTriggerCharacters = new string[] { "." };

        private CodeCompletionProvider completionProvider;

        public CompletionHandler(DocumentStorage documentStorage)
        {
            completionProvider = new CodeCompletionProvider();
            CodeCompletion.CodeCompletionController.comp = new PascalABCCompiler.Compiler();
            
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

            var documentPath = LspDataConvertor.GetNormalizedPathFromUri(request.TextDocument.Uri);

            Position pos = request.Position;

            var docText = documentStorage.GetDocumentText(documentPath);

            var textInfoProvider = documentStorage.GetTextInfoProvider(documentPath);

            int col = (int)pos.Character;

            char triggerChar = col > 0 ? textInfoProvider.GetSymbolAtPos((int)pos.Line, col - 1) : '_';

            // отслеживание ctrl + space "на пустом месте" (также сюда относится вставка пробельных символов)
            if (char.IsWhiteSpace(triggerChar))
            {
                triggerChar = '_';
            }

            UserDefaultCompletionData[] completionList = null;

            int symbolIndex = textInfoProvider.GetOffsetFromPosition((int)pos.Line, (int)pos.Character) - 1;

            try
            {
                // подсказка по точке, либо ctrl + space
                if (specialTriggerCharacters.Contains(triggerChar.ToString()) || triggerChar == '_')
                {
                    completionList = completionProvider.GenerateCompletionDataWithKeyword(documentPath, docText,
                            symbolIndex,
                            (int)pos.Line, (int)pos.Character, triggerChar, PascalABCCompiler.Parsers.KeywordKind.None);
                }
                // дополнение первого символа
                else
                {
                    PascalABCCompiler.Parsers.KeywordKind keyw = KeywordChecker.TestForKeyword(docText, symbolIndex);

                    if (CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.IsDefinitionIdentifierAfterKeyword(keyw))
                        return Task.FromResult(new CompletionList());

                    completionList = completionProvider.GenerateCompletionDataByFirstChar(documentPath, docText, symbolIndex, (int)pos.Line, (int)pos.Character, triggerChar, keyw);
                }
            }
            catch (Exception) { }

            if (completionList != null)
            {
                var resultList = new CompletionList(completionList.Select(item => new CompletionItem() { 
                    Label = item.Text, 
                    Detail = item.Description, 
                    InsertText = GetInsertedTextForCompletionItem(item.Text)
                }));

                return Task.FromResult(resultList);
            }
            
            return Task.FromResult(new CompletionList());
            
        }

        private string GetInsertedTextForCompletionItem(string text)
        {
            return !text.EndsWith("<>") ? null : text.Substring(0, text.Length - 2);
        }


        public void SetCapability(CompletionCapability capability)
        {
            this.capability = capability;
        }
    }
}
