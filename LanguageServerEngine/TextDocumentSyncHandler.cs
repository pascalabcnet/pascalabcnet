using MediatR;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using CodeCompletion;

namespace LanguageServerEngine
{
    internal class TextDocumentSyncHandler : ITextDocumentSyncHandler
    {
        private readonly ILanguageServerFacade router;

        private readonly DocumentStorage documentStorage;

        private readonly Dictionary<string, bool> openFiles = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

        public TextDocumentSyncHandler(ILanguageServerFacade router, DocumentStorage documentStorage)
        {
            this.router = router;
            this.documentStorage = documentStorage;
        }

        private TextDocumentSelector documentSelector => new TextDocumentSelector(
            TextDocumentFilter.ForPattern("**/*.pas"
                //"**/*.{" +
                //string.Join(
                //    ",",
                //    Languages.Facade.LanguageProvider.Instance.Languages
                //    .SelectMany(lang => lang.FilesExtensions)
                //    .Select(ext => ext.Substring(1))
                //    )
                //+ "}"
            )
        );

        public TextDocumentSyncKind Change { get; } = TextDocumentSyncKind.Incremental;

        public TextDocumentChangeRegistrationOptions GetRegistrationOptions(TextSynchronizationCapability capability, ClientCapabilities clientCapabilities)
        {
            return new TextDocumentChangeRegistrationOptions()
            {
                DocumentSelector = documentSelector,
                SyncKind = Change
            };
        }

        public TextDocumentAttributes GetTextDocumentAttributes(DocumentUri uri)
        {
            return new TextDocumentAttributes(uri, "PascalABC.NET");
        }

        public Task<Unit> Handle(DidChangeTextDocumentParams request, CancellationToken cancellationToken)
        {
            var documentUri = request.TextDocument.Uri.ToString();

            var contentChange = request.ContentChanges.First(); // берем пока одно изменение

            documentStorage.UpdateDocument(documentUri, contentChange);

            SetAsChanged(documentUri);

            ParseInThread();

            return Unit.Task;
        }

        public Task<Unit> Handle(DidOpenTextDocumentParams request, CancellationToken cancellationToken)
        {
            

            string documentUri = request.TextDocument.Uri.ToString();

            documentStorage.AddDocument(documentUri, request.TextDocument.Text);

            RegisterFileForParsing(documentUri);

            ParseInThread();

            // Console.Error.WriteLine((CodeCompletion.CodeCompletionController.comp_modules[documentUri] as DomConverter).visitor.cur_scope);

            return Unit.Task;
        }

        public Task<Unit> Handle(DidCloseTextDocumentParams request, CancellationToken cancellationToken)
        {
            string documentUri = request.TextDocument.Uri.ToString();

            documentStorage.RemoveDocument(documentUri);

            CloseFile(documentUri);

            return Unit.Task;
        }

        public Task<Unit> Handle(DidSaveTextDocumentParams request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }

        TextDocumentOpenRegistrationOptions IRegistration<TextDocumentOpenRegistrationOptions, TextSynchronizationCapability>.GetRegistrationOptions(TextSynchronizationCapability capability, ClientCapabilities clientCapabilities)
        {
            return new TextDocumentOpenRegistrationOptions()
            {
                DocumentSelector = documentSelector
            };
        }

        TextDocumentCloseRegistrationOptions IRegistration<TextDocumentCloseRegistrationOptions, TextSynchronizationCapability>.GetRegistrationOptions(TextSynchronizationCapability capability, ClientCapabilities clientCapabilities)
        {
            return new TextDocumentCloseRegistrationOptions()
            {
                DocumentSelector = documentSelector
            };
        }

        TextDocumentSaveRegistrationOptions IRegistration<TextDocumentSaveRegistrationOptions, TextSynchronizationCapability>.GetRegistrationOptions(TextSynchronizationCapability capability, ClientCapabilities clientCapabilities)
        {
            return new TextDocumentSaveRegistrationOptions()
            {
                DocumentSelector = documentSelector
            };
        }

        private void RegisterFileForParsing(string FileName)
        {
            openFiles[FileName] = true;
            CodeCompletion.CodeCompletionController.SetParser(System.IO.Path.GetExtension(FileName));
        }

        private void CloseFile(string FileName)
        {
            if (CodeCompletion.CodeCompletionController.comp_modules[FileName] != null)
                CodeCompletion.CodeCompletionController.comp_modules.Remove(FileName);
            openFiles.Remove(FileName);
        }

        private void SetAsChanged(string FileName)
        {
            openFiles[FileName] = true;
        }

        private void ParseInThread()
        {
            try
            {
                long mem_delta = 0;

                Dictionary<string, string> recomp_files = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                bool is_comp = false;
                foreach (string FileName in openFiles.Keys.ToArray()) // копирование ключей обязательно, иначе будет InvalidOperationException EVA
                {

                    if (openFiles[FileName])
                    {
                        is_comp = true;
                        CodeCompletion.CodeCompletionController controller = new CodeCompletion.CodeCompletionController();
                        string text = documentStorage.GetDocument(FileName).ToString();
                        if (string.IsNullOrEmpty(text))
                            text = "begin end.";
                        CodeCompletion.DomConverter tmp = CodeCompletion.CodeCompletionController.comp_modules[FileName] as CodeCompletion.DomConverter;
                        long cur_mem = Environment.WorkingSet;
                        CodeCompletion.DomConverter dc = controller.Compile(FileName, text);
                        mem_delta += Environment.WorkingSet - cur_mem;
                        openFiles[FileName] = false;
                        if (dc.is_compiled)
                        {
                            //CodeCompletion.CodeCompletionController.comp_modules.Remove(file_name);
                            if (tmp != null && tmp.visitor.entry_scope != null)
                            {
                                tmp.visitor.entry_scope.Clear();
                                if (tmp.visitor.cur_scope != null)
                                    tmp.visitor.cur_scope.Clear();
                            }
                            CodeCompletion.CodeCompletionController.comp_modules[FileName] = dc;
                            recomp_files[FileName] = FileName;
                            openFiles[FileName] = false;
                            //if (ParseInformationUpdated != null)
                            //    ParseInformationUpdated(dc.visitor.entry_scope, FileName);
                        }
                        else if (CodeCompletion.CodeCompletionController.comp_modules[FileName] == null)
                            CodeCompletion.CodeCompletionController.comp_modules[FileName] = dc;
                    }
                }
                foreach (string FileName in openFiles.Keys.ToArray()) // копирование ключей обязательно, иначе будет InvalidOperationException EVA
                {
                    CodeCompletion.DomConverter dc = CodeCompletion.CodeCompletionController.comp_modules[FileName] as CodeCompletion.DomConverter;
                    CodeCompletion.SymScope ss = null;
                    if (dc != null)
                    {
                        if (dc.visitor.entry_scope != null) ss = dc.visitor.entry_scope;
                        else if (dc.visitor.impl_scope != null) ss = dc.visitor.impl_scope;
                        int j = 0;
                        while (j < 2)
                        {
                            if (j == 0)
                            {
                                ss = dc.visitor.entry_scope;
                                j++;
                            }
                            else
                            {
                                ss = dc.visitor.impl_scope;
                                j++;
                            }
                            if (ss != null)
                            {
                                for (int i = 0; i < ss.used_units.Count; i++)
                                {
                                    string s = ss.used_units[i].file_name;
                                    if (s != null && openFiles.ContainsKey(s) && recomp_files.ContainsKey(s))
                                    {
                                        is_comp = true;
                                        CodeCompletion.CodeCompletionController controller = new CodeCompletion.CodeCompletionController();
                                        string text = documentStorage.GetDocument(FileName).ToString();
                                        CodeCompletion.DomConverter tmp = CodeCompletion.CodeCompletionController.comp_modules[FileName] as CodeCompletion.DomConverter;
                                        long cur_mem = Environment.WorkingSet;
                                        dc = controller.Compile(FileName, text);
                                        mem_delta += Environment.WorkingSet - cur_mem;
                                        openFiles[FileName] = false;
                                        CodeCompletion.CodeCompletionController.comp_modules[FileName] = dc;
                                        if (dc.is_compiled)
                                        {
                                            /*if (tmp != null && tmp.stv.entry_scope != null)
                                            {
                                                tmp.stv.entry_scope.Clear();
                                                if (tmp.stv.cur_scope != null) tmp.stv.cur_scope.Clear();
                                            }*/
                                            CodeCompletion.CodeCompletionController.comp_modules[FileName] = dc;
                                            recomp_files[FileName] = FileName;
                                            ss.used_units[i] = dc.visitor.entry_scope;
                                            //if (ParseInformationUpdated != null)
                                            //    ParseInformationUpdated(dc.visitor.entry_scope, FileName);
                                        }
                                        else if (CodeCompletion.CodeCompletionController.comp_modules[FileName] == null)
                                            CodeCompletion.CodeCompletionController.comp_modules[FileName] = dc;
                                    }
                                }
                            }
                        }
                    }
                }
                if (is_comp && mem_delta > 20000000 /*&& mem_delta > 10000000*/)
                //postavil delta dlja pamjati, posle kototoj delaetsja sborka musora
                {
                    GC.Collect();
                }
            }
            catch (Exception e) {

                Console.Error.WriteLine("Intellisense exception:");
                Console.Error.WriteLine(e.Message + "\n" + e.StackTrace);
            }

        }

    }
}
