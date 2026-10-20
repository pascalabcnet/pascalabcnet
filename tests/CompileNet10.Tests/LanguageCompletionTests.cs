using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using VisualPascalABC;
using VisualPascalABCPlugins;

internal static partial class Program
{
    private static Dictionary<string, object> CompletionResponse() => new Dictionary<string, object>
    {
        ["items"] = new object[]
        {
            new Dictionary<string, object> { ["label"] = "Substring", ["insertText"] = "Substring", ["kind"] = 2,
                ["detail"] = "function Substring(startIndex: integer): string",
                ["documentation"] = new Dictionary<string, object> { ["kind"] = "plaintext", ["value"] = "Описание кириллицей" } },
            new Dictionary<string, object> { ["label"] = "Generic<>", ["insertText"] = "Generic", ["kind"] = 2 },
            new Dictionary<string, object> { ["label"] = "Length", ["kind"] = 10 },
            new Dictionary<string, object> { ["label"] = "Snippet", ["insertTextFormat"] = 2 },
            new Dictionary<string, object> { ["label"] = "ComplexEdit", ["textEdit"] = new { } }
        }
    };

    private static void CheckLanguageCompletion(string runtime)
    {
        var images = new CodeCompletionImagesProvider();
        try
        {
            var provider = new Net10CompletionProvider(CompletionResponse(), images, "Su");
            Check(provider.Items.Length == 3 && provider.PreSelection == "Su" &&
                provider.Items[0].Description.Contains("Описание кириллицей") &&
                provider.Items[2].ImageIndex == images.IconNumberProperty,
                "LSP completion adapts labels, documentation, prefix and icons; unsupported edits/snippets are not inserted");
            using (var document = new EditorDocument { FileName = System.IO.Path.GetFullPath("Completion.pas") })
            {
                var editor = document.TextEditor;
                editor.Document.TextContent = "s.";
                editor.ActiveTextAreaControl.Caret.Position = editor.Document.OffsetToPosition(2);
                var context = new Net10CompletionContext(document);
                Check(context.IsCurrent(out var prefix) && prefix == "", "dot completion accepts original editor context");
                editor.Document.Insert(2, "Su"); editor.ActiveTextAreaControl.Caret.Position = editor.Document.OffsetToPosition(4);
                Check(context.IsCurrent(out prefix) && prefix == "Su", "typing a prefix while LSP answers does not discard the result");
                editor.ActiveTextAreaControl.Caret.Position = editor.Document.OffsetToPosition(1);
                Check(!context.IsCurrent(out prefix), "moved caret rejects stale completion");
                editor.ActiveTextAreaControl.Caret.Position = editor.Document.OffsetToPosition(4);
                editor.Document.TextContent = "x.Su";
                Check(!context.IsCurrent(out prefix), "changed member receiver rejects stale completion");
                editor.Document.TextContent = "s.Su"; editor.ActiveTextAreaControl.Caret.Position = editor.Document.OffsetToPosition(4);
                document.FileName = System.IO.Path.GetFullPath("AfterRename.pas");
                Check(!context.IsCurrent(out prefix), "Save As rejects old completion URI");
                document.FileName = context.FileName;
                editor.Document.TextContent = "s."; editor.ActiveTextAreaControl.Caret.Position = editor.Document.OffsetToPosition(2);
                provider.InsertAction(provider.Items[1], editor.ActiveTextAreaControl.TextArea, 2, '\0');
                Check(editor.Document.TextContent == "s.Generic", "completion inserts server insertText, not the generic display label");
            }
            CheckCompletionWindow(provider, images);
        }
        finally { images.ImageList.Dispose(); }

        using (var main = new EditorDocument { FileName = System.IO.Path.GetFullPath("CompletionMain.pas") })
        using (var unit = new EditorDocument { FileName = System.IO.Path.GetFullPath("CompletionUnit.pas") })
        using (var parent = new Form())
        {
            var open = new List<ICodeFileDocument> { main, unit };
            var events = new WorkbenchDocumentEventSource(() => open);
            var fake = new FakeLanguageClient();
            var sync = new Net10LanguageDocuments(events, () => fake, text => { });
            var ui = new Net10Completion(parent, events, sync, () => true);
            try
            {
                sync.SetActive(true); sync.WhenIdleAsync().GetAwaiter().GetResult();
                main.TextEditor.Document.TextContent = "uses CompletionUnit; begin value. end.";
                unit.TextEditor.Document.TextContent = "unit CompletionUnit; interface var value: string; implementation end.";
                sync.CompleteAsync(main, 0, 32, CancellationToken.None).GetAwaiter().GetResult();
                var messages = fake.Messages.ToArray();
                int request = Array.FindLastIndex(messages, item => item.Item1 == "textDocument/completion");
                Check(messages[request - 1].Item1 == "textDocument/didChange" && messages[request - 2].Item1 == "textDocument/didChange" &&
                    messages.Take(request).Any(item => item.Item1 == "textDocument/didChange" &&
                        (string)ObjectValue(ArrayValue(item.Item2["contentChanges"])[0])["text"] == unit.TextEditor.Document.TextContent),
                    "completion flushes unsaved main and dependent unit before the LSP request, without waiting for debounce");

                var entered = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
                fake.Completion = (parameters, cancellation) =>
                {
                    var reply = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
                    cancellation.Register(() => reply.TrySetCanceled()); entered.TrySetResult(true); return reply.Task;
                };
                using (var cancel = new CancellationTokenSource())
                {
                    var query = sync.CompleteAsync(main, 0, 32, cancel.Token);
                    entered.Task.GetAwaiter().GetResult(); cancel.Cancel();
                    bool cancelled = false;
                    try { query.GetAwaiter().GetResult(); } catch (OperationCanceledException) { cancelled = true; }
                    Check(cancelled && !fake.Disposed, "cancelled completion releases the queue but keeps shared LanguageServer alive");
                }
                fake.Completion = null;
                sync.SetActive(false); sync.WhenIdleAsync().GetAwaiter().GetResult();
                int count = fake.Messages.Count;
                sync.CompleteAsync(main, 0, 32, CancellationToken.None).GetAwaiter().GetResult();
                Check(fake.Messages.Count == count, "legacy mode sends no completion requests");

                var area = main.TextEditor.ActiveTextAreaControl.TextArea;
                ICSharpCode.TextEditor.KeyEventHandler legacy = ch => true;
                area.KeyEventHandler += legacy;
                var handleKey = area.GetType().GetMethod("HandleKeyPress", BindingFlags.NonPublic | BindingFlags.Instance);
                Check((bool)handleKey.Invoke(area, new object[] { 'x' }), "legacy editor handler retains its handled key result");
                ui.SetActive(true);
                Check(VisualPascalABC.CodeCompletionAllNamesAction.PackageCompletion != null &&
                    VisualPascalABC.CodeCompletionAllNamesAction.PackageDirectiveAtCaret != null,
                    "modern editor installs package-completion routing");
                var subscriptions = (System.Collections.IDictionary)typeof(Net10Completion)
                    .GetField("subscriptions", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(ui);
                Check(subscriptions.Count == 2 && (bool)handleKey.Invoke(area, new object[] { 'x' }),
                    "modern completion attaches without losing the legacy handler's consumed key");
                ui.SetActive(false);
                Check(VisualPascalABC.CodeCompletionAllNamesAction.PackageCompletion == null &&
                    VisualPascalABC.CodeCompletionAllNamesAction.PackageDirectiveAtCaret == null,
                    "return to net472 removes package-completion routing");
                Check(subscriptions.Count == 0 && (bool)handleKey.Invoke(area, new object[] { 'x' }),
                    "switching back detaches modern handler and preserves legacy result");
                ui.SetActive(true); open.Remove(main); events.Closed(main);
                Check((bool)handleKey.Invoke(area, new object[] { 'x' }), "closed tab detaches modern key handler before disposing the editor");
                open.Add(main); events.Opened(main);
                CheckCompletionInvalidation(ui, main, unit, events);
                area.KeyEventHandler -= legacy;
            }
            finally { ui.Dispose(); sync.ShutdownAsync().GetAwaiter().GetResult(); }
        }
    }

    private static void CheckCompletionInvalidation(Net10Completion ui, EditorDocument main, EditorDocument unit,
        WorkbenchDocumentEventSource events)
    {
        var flags = BindingFlags.NonPublic | BindingFlags.Instance;
        var pending = typeof(Net10Completion).GetField("pending", flags);
        var cancellation = typeof(Net10Completion).GetField("cancellation", flags);
        Action<CancellationTokenSource> begin = source =>
        {
            main.TextEditor.Document.TextContent = "s.";
            main.TextEditor.ActiveTextAreaControl.Caret.Position = main.TextEditor.Document.OffsetToPosition(2);
            pending.SetValue(ui, new Net10CompletionContext(main));
            cancellation.SetValue(ui, source);
        };
        var prefixSource = new CancellationTokenSource();
        var prefixToken = prefixSource.Token;
        begin(prefixSource);
        main.TextEditor.Document.Insert(2, "Su");
        main.TextEditor.ActiveTextAreaControl.Caret.Position = main.TextEditor.Document.OffsetToPosition(4);
        Check(!prefixToken.IsCancellationRequested && pending.GetValue(ui) != null,
            "pending UI request survives identifier typing and caret advancing inside the prefix");
        unit.TextEditor.Document.TextContent += "{changed}";
        Check(prefixToken.IsCancellationRequested && pending.GetValue(ui) == null,
            "editing another unsaved unit cancels the pending UI response");
        foreach (string action in new[] { "caret", "escape", "rename", "switch" })
        {
            var source = new CancellationTokenSource(); var token = source.Token; begin(source);
            switch (action)
            {
                case "caret": main.TextEditor.ActiveTextAreaControl.Caret.Position = main.TextEditor.Document.OffsetToPosition(0); break;
                case "escape": main.TextEditor.ActiveTextAreaControl.TextArea.ExecuteDialogKey(Keys.Escape); break;
                case "rename":
                    string old = main.FileName; main.FileName += ".pas"; events.Renamed(main, old); break;
                case "switch": ui.SetActive(false); break;
            }
            Check(token.IsCancellationRequested && pending.GetValue(ui) == null, "pending UI completion cancelled on " + action);
        }
    }

    private static void CheckCompletionWindow(Net10CompletionProvider provider, CodeCompletionImagesProvider images)
    {
        // Test the real host widget without showing windows or starting the IDE/compiler.
        var singleton = typeof(VisualPABCSingleton).GetField("mainForm", BindingFlags.NonPublic | BindingFlags.Static);
        var previous = singleton.GetValue(null);
        var shell = (Form1)FormatterServices.GetUninitializedObject(typeof(Form1));
        shell.UserOptions = new UserOptions { ShowCompletionInfoByGroup = true, EnableSmartIntellisense = true };
        singleton.SetValue(null, shell);
        try
        {
            using (var parent = new Form())
            using (var document = new EditorDocument { FileName = System.IO.Path.GetFullPath("Popup.pas") })
            {
                document.TextEditor.Document.TextContent = "s.Su";
                document.TextEditor.ActiveTextAreaControl.Caret.Position = document.TextEditor.Document.OffsetToPosition(4);
                var ctor = typeof(PABCNETCodeCompletionWindow).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Single(item => item.GetParameters().Length == 8);
                using (var window = (PABCNETCodeCompletionWindow)ctor.Invoke(new object[]
                    { provider, provider.Items, parent, document.TextEditor, false, true, images, false }))
                {
                    Check(window.Width == Constants.CompletionWindowWidth,
                        "short ordinary completion keeps the minimum popup width");
                    var list = (PABCNETCodeCompletionListView)typeof(PABCNETCodeCompletionWindow)
                        .GetField("codeCompletionListView", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(window);
                    Check(list.SelectedCompletionData.Text == "Substring", "standard completion window selects and filters LSP prefix with grouping enabled");
                    var insert = typeof(PABCNETCodeCompletionWindow).GetMethod("InsertSelectedItem", BindingFlags.NonPublic | BindingFlags.Instance);
                    insert.Invoke(window, new object[] { '\0' });
                    Check(document.TextEditor.Document.TextContent == "s.Substring",
                        "standard window replaces typed prefix once without invoking legacy smart-semantic caches");
                }
                var longResponse = new Dictionary<string, object> { ["items"] = new object[] {
                    new Dictionary<string, object> { ["label"] = "Symbol" + new string('X', 300), ["kind"] = 2 }
                } };
                var longProvider = new Net10CompletionProvider(longResponse, images, "");
                using (var window = (PABCNETCodeCompletionWindow)ctor.Invoke(new object[]
                    { longProvider, longProvider.Items, parent, document.TextEditor, false, true, images, false }))
                    Check(window.Width > Constants.CompletionWindowWidth &&
                        window.Width <= Math.Max(Constants.CompletionWindowWidth, Screen.GetWorkingArea(window).Width / 4),
                        "long ordinary completion grows only up to a quarter of the working area");
            }
        }
        finally { singleton.SetValue(null, previous); }
    }
}
