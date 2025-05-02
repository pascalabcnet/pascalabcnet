using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using OmniSharp.Extensions.LanguageServer.Protocol;

namespace LanguageServerEngine
{
    internal class TextDocumentSyncHandler : ITextDocumentSyncHandler
    {
        // private readonly ILanguageServer router;

        private SynchronizationCapability capability;

        private readonly DocumentStorage documentStorage;

        private readonly Dictionary<string, bool> openFiles = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

        public TextDocumentSyncHandler(DocumentStorage documentStorage)
        {
            this.documentStorage = documentStorage;
        }

        public TextDocumentSyncKind Change { get; } = TextDocumentSyncKind.Incremental;

        public TextDocumentSyncOptions Options => new TextDocumentSyncOptions() { OpenClose = true, Change = Change };

        public TextDocumentAttributes GetTextDocumentAttributes(Uri uri)
        {
            return new TextDocumentAttributes(uri, "PascalABC.NET");
        }

        public Task Handle(DidChangeTextDocumentParams request)
        {
            var documentUri = request.TextDocument.Uri.ToString();

            var contentChange = request.ContentChanges.First(); // берем пока одно изменение

            documentStorage.UpdateDocument(documentUri, contentChange);

            SetAsChanged(documentUri);

            ParseInThread();

            return Task.CompletedTask;
        }

        public Task Handle(DidOpenTextDocumentParams request)
        {
            Console.Error.WriteLine("did open params received");

            string documentUri = request.TextDocument.Uri.ToString();

            documentStorage.AddDocument(documentUri, request.TextDocument.Text);

            RegisterFileForParsing(documentUri);

            ParseInThread();

            // Console.Error.WriteLine((CodeCompletion.CodeCompletionController.comp_modules[documentUri] as DomConverter).visitor.cur_scope);

            return Task.CompletedTask;
        }

        public Task Handle(DidCloseTextDocumentParams request)
        {
            string documentUri = request.TextDocument.Uri.ToString();

            documentStorage.RemoveDocument(documentUri);

            CloseFile(documentUri);

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
                            Console.Error.WriteLine("Compiled");
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
                                            //if (tmp != null && tmp.stv.entry_scope != null)
                                            //{
                                            //    tmp.stv.entry_scope.Clear();
                                            //    if (tmp.stv.cur_scope != null) tmp.stv.cur_scope.Clear();
                                            //}
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
                if (is_comp && mem_delta > 20000000 && mem_delta > 10000000)
                //postavil delta dlja pamjati, posle kototoj delaetsja sborka musora
                {
                    GC.Collect();
                }
            }
            catch (Exception e)
            {

                Console.Error.WriteLine("Intellisense exception:");
                Console.Error.WriteLine(e.Message + "\n" + e.StackTrace);
            }

        }
    }
}
