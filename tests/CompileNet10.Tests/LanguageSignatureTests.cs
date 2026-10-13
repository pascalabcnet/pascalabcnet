using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.InsightWindow;
using VisualPascalABCPlugins;

internal static partial class Program
{
    private static object SignatureResponse(int parameter = 0) => new Dictionary<string, object>
    {
        ["activeSignature"] = 1, ["activeParameter"] = parameter,
        ["signatures"] = new ArrayList
        {
            new Dictionary<string, object> { ["label"] = "function Substring(startIndex: integer): string;",
                ["parameters"] = new ArrayList { new Dictionary<string, object> { ["label"] = "startIndex: integer" } } },
            new Dictionary<string, object> { ["label"] = "function Substring(startIndex: integer; length: integer): string;",
                ["documentation"] = new Dictionary<string, object> { ["kind"] = "plaintext", ["value"] = "Описание кириллицей" },
                ["parameters"] = new ArrayList { new Dictionary<string, object> { ["label"] = "startIndex: integer" },
                    new Dictionary<string, object> { ["label"] = "length: integer" } } }
        }
    };

    private static void CheckLanguageSignature(string runtime)
    {
        CheckSignatureTypedTrigger();
        CheckSignatureTypedTrigger(runtime);
        using (var main = new EditorDocument { FileName = Path.GetFullPath("SignatureMain.pas") })
        using (var unit = new EditorDocument { FileName = Path.GetFullPath("SignatureUnit.pas") })
        using (var parent = new Form())
        {
            var area = main.TextEditor.ActiveTextAreaControl.TextArea;
            var handle = area.Handle; // Native message target, without showing any window.
            main.TextEditor.Document.TextContent = "begin s.Substring(";
            area.Caret.Position = main.TextEditor.Document.OffsetToPosition(main.TextEditor.Document.TextLength);
            var context = Net10SignatureContext.Capture(main, area.Caret.Offset);
            var provider = new Net10SignatureProvider(SignatureResponse(), context);
            Check(provider.InsightDataCount == 2 && provider.DefaultIndex == 1 && provider.CurrentParameter == 1 &&
                provider.GetInsightData(1).Contains("Описание кириллицей"), "signature adapter preserves overload selection and documentation");
            provider.GetParameterHighlight(1, out int start, out int length);
            Check(provider.GetInsightData(1).Substring(start, length) == "startIndex: integer", "first parameter uses zero-based LSP activeParameter");
            var second = new Net10SignatureProvider(SignatureResponse(1), context);
            second.GetParameterHighlight(1, out start, out length);
            Check(second.GetInsightData(1).Substring(start, length) == "length: integer" && second.CurrentParameter == 2,
                "comma selects the next parameter and provides exact UTF-16 bold range");
            Check(!second.GetParameterHighlight(0, out start, out length), "shorter overload does not bold an unrelated range");
            var ranges = new Net10SignatureProvider(new Dictionary<string, object>
            {
                ["activeParameter"] = 5,
                ["signatures"] = new object[] { new Dictionary<string, object>
                {
                    ["label"] = "Print(params values: object[])",
                    ["parameters"] = new object[] { new Dictionary<string, object> { ["label"] = new ArrayList { 6, 29 } } }
                } }
            }, context);
            Check(ranges.GetParameterHighlight(0, out start, out length) && start == 6 && length == 23,
                "LSP offset pair and extra variadic arguments highlight the params parameter");
            Check(new Net10SignatureProvider(null, context).InsightDataCount == 0, "null signature response opens no window");

            main.TextEditor.Document.Insert(area.Caret.Offset, "1");
            area.Caret.Position = main.TextEditor.Document.OffsetToPosition(main.TextEditor.Document.TextLength);
            Check(context.IsCurrent, "typing the current argument while awaiting a signature keeps the response valid");
            main.TextEditor.Document.Insert(area.Caret.Offset, ")");
            area.Caret.Position = main.TextEditor.Document.OffsetToPosition(main.TextEditor.Document.TextLength);
            Check(!context.IsCurrent, "closing call discards a late signature response");
            foreach (string text in new[] { "f('a,", "f('a''b,", "f({,", "f((* nested {x} ,", "f(//," })
            {
                main.TextEditor.Document.TextContent = text;
                Check(Net10SignatureContext.Capture(main, text.Length) == null, "no signature trigger in strings/comments: " + text);
            }
            main.TextEditor.Document.TextContent = "f({ nested (* , *) } 1,";
            Check(Net10SignatureContext.Capture(main, main.TextEditor.Document.TextLength)?.OpenOffset == 1,
                "signature resumes after nested Pascal comments");
            main.TextEditor.Document.TextContent = "f([1,";
            Check(Net10SignatureContext.Capture(main, main.TextEditor.Document.TextLength)?.OpenOffset == 2,
                "array comma is not mistaken for a parameter of the enclosing call");
            CheckSignatureWindow(parent, main);

            var open = new List<ICodeFileDocument> { main, unit };
            var events = new WorkbenchDocumentEventSource(() => open);
            var fake = new FakeLanguageClient();
            var sync = new Net10LanguageDocuments(events, () => fake, text => { });
            bool option = true;
            var ui = new Net10Signature(parent, events, sync, () => option);
            try
            {
                sync.SetActive(true); sync.WhenIdleAsync().GetAwaiter().GetResult();
                main.TextEditor.Document.TextContent = "begin s.Substring(";
                unit.TextEditor.Document.TextContent = "unit SignatureUnit; interface var s: string; implementation end.";
                sync.SignatureAsync(main, 0, main.TextEditor.Document.TextLength, CancellationToken.None).GetAwaiter().GetResult();
                var messages = fake.Messages.ToArray();
                int index = Array.FindLastIndex(messages, item => item.Item1 == "textDocument/signatureHelp");
                Check(messages[index - 1].Item1 == "textDocument/didChange" && messages[index - 2].Item1 == "textDocument/didChange" &&
                    !messages[index].Item2.ContainsKey("context"), "signature request flushes unsaved main and unit through the shared queue");
                ICSharpCode.TextEditor.KeyEventHandler other = ch => true;
                area.KeyEventHandler += other;
                ui.SetActive(true);
                var flags = BindingFlags.Instance | BindingFlags.NonPublic;
                Check((bool)typeof(TextArea).GetMethod("HandleKeyPress", flags).Invoke(area, new object[] { 'x' }),
                    "signature adds no boolean key handler that could overwrite completion's handled return");
                CheckSignatureInvalidation(ui, main, unit, events);
                ui.SetActive(false); area.KeyEventHandler -= other;
                sync.SetActive(false); sync.WhenIdleAsync().GetAwaiter().GetResult();
                int count = fake.Messages.Count;
                sync.SignatureAsync(main, 0, 1, CancellationToken.None).GetAwaiter().GetResult();
                Check(fake.Messages.Count == count && !fake.Disposed, "legacy mode detaches signature without another process or queries");
                option = false;
            }
            finally { ui.Dispose(); sync.ShutdownAsync().GetAwaiter().GetResult(); }
        }
    }

    // Exercise the real editor insertion, posted UI callback and visible native hint together.
    // The transparent fixture never displays source files or touches the user's IDE/settings.
    private static void CheckSignatureTypedTrigger(string runtime = null)
    {
        using (var document = new EditorDocument { FileName = Path.GetFullPath("SignatureTyped.pas") })
        using (var parent = new Form { ShowInTaskbar = false, Opacity = 0, Size = new Size(600, 400) })
        {
            parent.Controls.Add(document.TextEditor);
            document.TextEditor.Dock = DockStyle.Fill;
            var area = document.TextEditor.ActiveTextAreaControl.TextArea;
            document.TextEditor.Document.TextContent = "begin var s := 'hello'; s.Substring end.";
            area.Caret.Position = document.TextEditor.Document.OffsetToPosition("begin var s := 'hello'; s.Substring".Length);
            var events = new WorkbenchDocumentEventSource(() => new ICodeFileDocument[] { document });
            var fake = new FakeLanguageClient { Completion = (value, token) => System.Threading.Tasks.Task.FromResult(SignatureResponse()) };
            var sync = new Net10LanguageDocuments(events, () => runtime == null ? (INet10LanguageClient)fake :
                new Net10LanguageClient(runtime, "dotnet", "ru"), value => { });
            using (var ui = new Net10Signature(parent, events, sync, () => true))
            {
                try
                {
                    sync.SetActive(true); sync.WhenIdleAsync().GetAwaiter().GetResult();
                    ui.SetActive(true);
                    parent.Show(); area.Focus();
                    Check(area.Focused, "signature fixture uses the focused standard editor");
                    area.SimulateKeyPress('(');
                    var field = typeof(Net10Signature).GetField("window", BindingFlags.Instance | BindingFlags.NonPublic);
                    var timeout = Stopwatch.StartNew();
                    while (field.GetValue(ui) == null && timeout.ElapsedMilliseconds < 10000)
                    {
                        Application.DoEvents(); Thread.Sleep(1);
                    }
                    var window = (PABCNETInsightWindow)field.GetValue(ui);
                    Check(window != null && window.Visible && window.InsightProviderStackLength == 1,
                        "typing '(' opens the standard signature window through the complete UI pipeline" + (runtime == null ? "" : " with real LanguageServer"));
                    Check(document.TextEditor.Document.GetText(area.Caret.Offset - 1, 1) == "(" &&
                        (runtime != null || Array.Exists(fake.Messages.ToArray(), message => message.Item1 == "textDocument/signatureHelp")),
                        "signature trigger leaves the editor's inserted bracket intact");
                    Application.DoEvents();
                    Check(ReferenceEquals(field.GetValue(ui), window) && window.Visible && area.Focused,
                        "signature survives the native window's temporary focus transfer to the editor container");
                    fake.Completion = (value, token) => System.Threading.Tasks.Task.FromResult(SignatureResponse(1));
                    area.SimulateKeyPress('1'); area.SimulateKeyPress(',');
                    timeout.Restart();
                    while (window.Visible && ((IExternalInsightDataProvider)window.GetInsightProvider()).CurrentParameter != 2 && timeout.ElapsedMilliseconds < 10000)
                    {
                        Application.DoEvents(); Thread.Sleep(1);
                    }
                    Check(window.Visible && window.InsightProviderStackLength == 1 &&
                        ((IExternalInsightDataProvider)window.GetInsightProvider()).CurrentParameter == 2,
                        "typing comma refreshes the displayed signature and selects parameter two");
                    area.SimulateKeyPress('2'); area.SimulateKeyPress(')'); Application.DoEvents();
                    Check(field.GetValue(ui) == null && window.IsDisposed, "typing ')' closes the displayed signature");
                    area.SimulateKeyPress('(');
                    parent.Controls.Add(new TextBox { Dock = DockStyle.Bottom });
                    parent.Controls[parent.Controls.Count - 1].Focus();
                    Application.DoEvents();
                    Check(field.GetValue(ui) == null && typeof(Net10Signature).GetField("pending", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(ui) == null,
                        "genuine focus loss cancels a pending signature instead of reopening it");
                }
                finally { ui.CloseHints(); parent.Hide(); sync.ShutdownAsync().GetAwaiter().GetResult(); }
            }
        }
    }

    private static void CheckSignatureWindow(Form parent, EditorDocument document)
    {
        document.TextEditor.Document.TextContent = "begin Outer(Inner(";
        var area = document.TextEditor.ActiveTextAreaControl.TextArea;
        area.Caret.Position = document.TextEditor.Document.OffsetToPosition(document.TextEditor.Document.TextLength);
        var outer = Net10SignatureContext.Capture(document, "begin Outer(".Length);
        var inner = Net10SignatureContext.Capture(document, area.Caret.Offset);
        using (var window = new PABCNETInsightWindow(parent, document.TextEditor))
        {
            window.AddInsightDataProvider(new Net10SignatureProvider(SignatureResponse(), outer), document.FileName);
            window.AddInsightDataProvider(new Net10SignatureProvider(SignatureResponse(), inner), document.FileName);
            // Exercise the real standard renderer without showing any windows or using legacy DOM caches.
            using (var bitmap = new Bitmap(800, 600))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                var paint = typeof(PABCNETInsightWindow).GetMethod("OnPaint", BindingFlags.NonPublic | BindingFlags.Instance);
                paint.Invoke(window, new object[] { new PaintEventArgs(graphics, new Rectangle(0, 0, 800, 600)) });
                paint.Invoke(window, new object[] { new PaintEventArgs(graphics, new Rectangle(0, 0, 800, 600)) });
            }
            Check(window.InsightProviderStackLength == 2 && window.GetCurrentData() == 1,
                "standard insight renderer paints LSP signature and documentation, with nested overload stack");
            var caret = typeof(PABCNETInsightWindow).GetMethod("CaretOffsetChanged", BindingFlags.NonPublic | BindingFlags.Instance);
            document.TextEditor.Document.Insert(area.Caret.Offset, ")");
            area.Caret.Position = document.TextEditor.Document.OffsetToPosition(document.TextEditor.Document.TextLength);
            caret.Invoke(window, new object[] { area.Caret, EventArgs.Empty });
            Check(window.InsightProviderStackLength == 1 && ((Net10SignatureProvider)window.GetInsightProvider()).Context.OpenOffset == outer.OpenOffset,
                "closing nested call restores the outer signature in the native window stack");
            document.TextEditor.Document.Insert(area.Caret.Offset, ",");
            area.Caret.Position = document.TextEditor.Document.OffsetToPosition(document.TextEditor.Document.TextLength);
            window.ReplaceCurrentInsightDataProvider(new Net10SignatureProvider(SignatureResponse(1), Net10SignatureContext.Capture(document, area.Caret.Offset)), document.FileName);
            Check(window.InsightProviderStackLength == 1 && ((IExternalInsightDataProvider)window.GetInsightProvider()).CurrentParameter == 2,
                "comma updates the current signature without accumulating another provider on the stack");
            document.TextEditor.Document.Insert(area.Caret.Offset, "2)");
            area.Caret.Position = document.TextEditor.Document.OffsetToPosition(document.TextEditor.Document.TextLength);
            caret.Invoke(window, new object[] { area.Caret, EventArgs.Empty });
            Check(window.IsDisposed, "closing outer call closes the standard signature window");
        }
    }

    private static void CheckSignatureInvalidation(Net10Signature ui, EditorDocument main, EditorDocument unit, WorkbenchDocumentEventSource events)
    {
        var flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var pending = typeof(Net10Signature).GetField("pending", flags);
        var cancellation = typeof(Net10Signature).GetField("cancellation", flags);
        var area = main.TextEditor.ActiveTextAreaControl.TextArea;
        foreach (string action in new[] { "edit", "dependent edit", "caret", "escape", "rename", "close", "switch" })
        {
            main.TextEditor.Document.TextContent = "begin s.Substring(1";
            area.Caret.Position = main.TextEditor.Document.OffsetToPosition(main.TextEditor.Document.TextLength);
            var context = Net10SignatureContext.Capture(main, main.TextEditor.Document.TextLength - 1);
            var source = new CancellationTokenSource(); var token = source.Token;
            pending.SetValue(ui, context); cancellation.SetValue(ui, source);
            if (action == "edit") main.TextEditor.Document.Insert(0, " ");
            else if (action == "dependent edit") unit.TextEditor.Document.TextContent += " ";
            else if (action == "caret") area.Caret.Position = main.TextEditor.Document.OffsetToPosition(0);
            else if (action == "escape") typeof(TextArea).GetMethod("OnKeyDown", flags).Invoke(area, new object[] { new KeyEventArgs(Keys.Escape) });
            else if (action == "rename") { string old = main.FileName; main.FileName += ".pas"; events.Renamed(main, old); }
            else if (action == "close") events.Closed(main);
            else if (action == "switch") ui.SetActive(false);
            Check(token.IsCancellationRequested && pending.GetValue(ui) == null, "late signature cancelled on " + action);
            if (action == "close") events.Opened(main);
        }
    }
}
