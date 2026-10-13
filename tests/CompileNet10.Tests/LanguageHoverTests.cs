using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using VisualPascalABCPlugins;

internal static partial class Program
{
    private static void CheckMetadataHover(Net10LanguageDocuments sync, Net10NavigationTarget target,
        List<ICodeFileDocument> open, WorkbenchDocumentEventSource events)
    {
        using (var metadata = new EditorDocument { FileName = "String [.NET 10].pas", FromMetadata = true })
        using (var parent = new Form())
        using (var ui = new Net10Hover(parent, events, sync, () => true))
        {
            metadata.TextEditor.Document.TextContent = target.MetadataText;
            open.Add(metadata); events.Opened(metadata); sync.WhenIdleAsync().GetAwaiter().GetResult();
            ui.SetActive(true);
            var subscriptions = (System.Collections.IDictionary)typeof(Net10Hover).GetField("subscriptions",
                BindingFlags.Instance | BindingFlags.NonPublic).GetValue(ui);
            Check(subscriptions.Contains(metadata), "metadata tab receives the modern hover subscription");
            int offset = target.MetadataText.IndexOf("integer", StringComparison.Ordinal);
            Check(offset >= 0, "real CLR metadata has a Pascal integer type to inspect");
            var point = metadata.TextEditor.Document.OffsetToPosition(offset + 2);
            var response = sync.HoverAsync(metadata, point.Y, point.X, CancellationToken.None).GetAwaiter().GetResult();
            string description = Net10HoverContext.Description(response);
            Check(description != null && description.Contains("integer"), "hover describes integer inside real .NET 10 String metadata");
            offset = target.MetadataText.IndexOf("Length :", StringComparison.Ordinal);
            point = metadata.TextEditor.Document.OffsetToPosition(offset + 2);
            response = sync.HoverAsync(metadata, point.Y, point.X, CancellationToken.None).GetAwaiter().GetResult();
            description = Net10HoverContext.Description(response);
            Check(description != null && description.Contains("Length") && description.Contains("integer"),
                "metadata hover also describes a property, not just built-in type names");
            Check(metadata.TextEditor.Document.TextContent == target.MetadataText && target.MetadataText.Contains("System.Char&"),
                "metadata analysis leaves the displayed text and CLR by-ref signature unchanged");
            Check(sync.CompleteAsync(metadata, point.Y, point.X, CancellationToken.None).GetAwaiter().GetResult() == null,
                "metadata remains a generated view, not a user source with editing completion");
            sync.Flush(); sync.WhenIdleAsync().GetAwaiter().GetResult();
            open.Remove(metadata); events.Closed(metadata); sync.WhenIdleAsync().GetAwaiter().GetResult();
            Check(!subscriptions.Contains(metadata) &&
                sync.HoverAsync(metadata, point.Y, point.X, CancellationToken.None).GetAwaiter().GetResult() == null,
                "closing metadata detaches hover and prevents further requests");
            Check(!File.Exists(metadata.FileName), "metadata hover creates no source file on disk");
        }
    }

    private static void CheckLanguageHover()
    {
        Check(Net10HoverContext.Description(null) == null &&
            Net10HoverContext.Description(new Dictionary<string, object> { ["contents"] = new { unknown = true } }) == null,
            "empty and unsupported hover responses do not open a hint");
        Check(Net10HoverContext.Description(new Dictionary<string, object>
            { ["contents"] = new Dictionary<string, object> { ["kind"] = "plaintext", ["value"] = "var имя: string" } }) == "var имя: string",
            "hover preserves plaintext signature and Cyrillic from the real LSP format");
        Check(Net10HoverContext.Description(new Dictionary<string, object> { ["contents"] = new ArrayList
            { "first", new Dictionary<string, object> { ["language"] = "pascal", ["value"] = "second" } } }) == "first" + Environment.NewLine + "second",
            "hover also accepts legacy LSP string and MarkedString array formats");

        using (var main = new EditorDocument { FileName = Path.GetFullPath("HoverMain.pas") })
        using (var unit = new EditorDocument { FileName = Path.GetFullPath("HoverUnit.pas") })
        using (var parent = new Form())
        {
            main.TextEditor.Document.TextContent = "{😀}\r\nbegin var имя := 'abc'; имя.Substring(1); end.";
            unit.TextEditor.Document.TextContent = "unit HoverUnit; interface implementation end.";
            var position = new TextLocation(11, 1); // UTF-16 editor coordinates, including prior surrogate pair.
            var context = Net10HoverContext.Capture(main, position);
            Check(context != null && context.Contains(new TextLocation(12, 1)) &&
                !context.Contains(new TextLocation(14, 1)) && !context.Contains(new TextLocation(11, 0)),
                "hover remains valid inside one identifier, not across whitespace or lines");
            Check(Net10HoverContext.Capture(main, new TextLocation(5, 1)) == null &&
                Net10HoverContext.Capture(main, new TextLocation(1000, 1)) == null &&
                Net10HoverContext.Capture(main, new TextLocation(0, -1)) == null,
                "hover ignores whitespace, virtual columns and invalid lines");
            string previousName = main.FileName;
            main.FileName += ".pas";
            Check(!context.IsCurrent, "Save As invalidates captured hover URI");
            main.FileName = previousName;
            main.TextEditor.Document.TextContent += " ";
            Check(!context.IsCurrent, "unsaved edits invalidate a captured hover description");

            var open = new List<ICodeFileDocument> { main, unit };
            var events = new WorkbenchDocumentEventSource(() => open);
            var fake = new FakeLanguageClient();
            var sync = new Net10LanguageDocuments(events, () => fake, text => { });
            bool option = true;
            var ui = new Net10Hover(parent, events, sync, () => option);
            var area = main.TextEditor.ActiveTextAreaControl.TextArea;
            var raise = typeof(TextArea).GetMethod("OnToolTipRequest", BindingFlags.NonPublic | BindingFlags.Instance);
            Action<ToolTipRequestEventArgs> request = args => raise.Invoke(area, new object[] { args });
            try
            {
                sync.SetActive(true); sync.WhenIdleAsync().GetAwaiter().GetResult();
                main.TextEditor.Document.TextContent += "{новый текст}";
                unit.TextEditor.Document.TextContent += "{несохранённый модуль}";
                sync.HoverAsync(main, 1, 11, CancellationToken.None).GetAwaiter().GetResult();
                var messages = fake.Messages.ToArray();
                int index = Array.FindLastIndex(messages, item => item.Item1 == "textDocument/hover");
                var parameters = messages[index].Item2;
                Check(messages[index - 1].Item1 == "textDocument/didChange" && messages[index - 2].Item1 == "textDocument/didChange" &&
                    (int)ObjectValue(parameters["position"])["line"] == 1 && (int)ObjectValue(parameters["position"])["character"] == 11 &&
                    !parameters.ContainsKey("context"),
                    "hover flushes ALL unsaved editors and sends the UTF-16 position without completion-specific parameters");

                int count = fake.Messages.Count;
                request(new ToolTipRequestEventArgs(Point.Empty, position, true));
                sync.WhenIdleAsync().GetAwaiter().GetResult();
                Check(fake.Messages.Count == count, "legacy mode has no modern hover handler or requests");
                ui.SetActive(true);
                var claimed = new ToolTipRequestEventArgs(Point.Empty, position, true); claimed.ShowToolTip("error marker");
                request(claimed);
                Check(claimed.ToolTipShown && fake.Messages.Count == count, "existing marker/folding/debugger tooltip retains priority");
                option = false;
                request(new ToolTipRequestEventArgs(Point.Empty, position, true));
                Check(fake.Messages.Count == count, "disabled hover setting sends no request immediately, before the settings timer");
                option = true;
                CheckHoverInvalidation(ui, main, unit, events, request, position);
                ui.SetActive(false);
                count = fake.Messages.Count;
                request(new ToolTipRequestEventArgs(Point.Empty, position, true));
                Check(fake.Messages.Count == count, "return to net472 detaches hover and leaves legacy event dispatch intact");
                sync.SetActive(false); sync.WhenIdleAsync().GetAwaiter().GetResult();
                count = fake.Messages.Count;
                sync.HoverAsync(main, 1, 11, CancellationToken.None).GetAwaiter().GetResult();
                Check(fake.Messages.Count == count && !fake.Disposed, "suspended hover keeps the shared server without sending queries");
            }
            finally { ui.Dispose(); sync.ShutdownAsync().GetAwaiter().GetResult(); }
        }
    }

    private static void CheckHoverInvalidation(Net10Hover ui, EditorDocument main, EditorDocument unit,
        WorkbenchDocumentEventSource events, Action<ToolTipRequestEventArgs> request, TextLocation position)
    {
        var flags = BindingFlags.NonPublic | BindingFlags.Instance;
        var captured = typeof(Net10Hover).GetField("context", flags);
        var cancellation = typeof(Net10Hover).GetField("cancellation", flags);
        var area = main.TextEditor.ActiveTextAreaControl.TextArea;
        foreach (string action in new[] { "edit", "dependent edit", "caret", "key", "mouse", "leave", "rename", "close", "switch" })
        {
            main.TextEditor.Document.TextContent = "\r\nbegin var имя := 'abc'; end.";
            area.Caret.Position = main.TextEditor.Document.OffsetToPosition(0);
            var source = new CancellationTokenSource(); var token = source.Token;
            captured.SetValue(ui, Net10HoverContext.Capture(main, position));
            cancellation.SetValue(ui, source);
            if (action == "edit") main.TextEditor.Document.Insert(0, " ");
            else if (action == "dependent edit") unit.TextEditor.Document.TextContent += " ";
            else if (action == "caret") area.Caret.Position = main.TextEditor.Document.OffsetToPosition(1);
            else if (action == "key") typeof(TextArea).GetMethod("OnKeyDown", flags).Invoke(area, new object[] { new KeyEventArgs(Keys.Escape) });
            else if (action == "mouse") request(new ToolTipRequestEventArgs(Point.Empty, new TextLocation(5, 1), true));
            else if (action == "leave") typeof(TextArea).GetMethod("OnMouseLeave", flags).Invoke(area, new object[] { EventArgs.Empty });
            else if (action == "rename") { string old = main.FileName; main.FileName += ".pas"; events.Renamed(main, old); }
            else if (action == "close") events.Closed(main);
            else if (action == "switch") ui.SetActive(false);
            Check(token.IsCancellationRequested && captured.GetValue(ui) == null, "late hover response cancelled on " + action);
            if (action == "close") events.Opened(main);
        }
    }
}
