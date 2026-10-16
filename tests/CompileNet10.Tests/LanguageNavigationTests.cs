using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using VisualPascalABCPlugins;

internal static partial class Program
{
    private static object NavigationResponse(string file, int line = 4, int column = 10) => new ArrayList
    {
        new Dictionary<string, object> { ["uri"] = new Uri(file).AbsoluteUri,
            ["range"] = new Dictionary<string, object> { ["start"] = new Dictionary<string, object> { ["line"] = line, ["character"] = column } } }
    };
    private static void PumpNavigation(Func<bool> completed)
    {
        var timeout = Stopwatch.StartNew();
        while (!completed() && timeout.ElapsedMilliseconds < 10000) { Application.DoEvents(); Thread.Sleep(1); }
        Check(completed(), "navigation UI callback completes within timeout");
    }
    private static void CheckLanguageNavigation(string runtime)
    {
        string unitName = Path.GetFullPath("Навигация A.pas");
        var parsed = Net10NavigationTarget.Read(NavigationResponse(unitName));
        Check(parsed.Count == 1 && parsed[0].FileName == unitName && parsed[0].Line == 5 && parsed[0].Column == 11,
            "navigation converts encoded Cyrillic file URI and UTF-16 LSP coordinates to IDE coordinates once");
        Check(Net10NavigationTarget.Read(NavigationResponse("https://example.com/Bad.pas")).Count == 0 &&
            Net10NavigationTarget.Read(NavigationResponse(unitName, -1)).Count == 0,
            "navigation rejects external URLs and invalid coordinates instead of opening them");
        CheckNavigationClick(null);
        CheckNavigationClick(runtime);
    }

    private static void CheckNavigationClick(string runtime)
    {
        string folder = Path.Combine(Path.GetTempPath(), "Навигация-" + Guid.NewGuid().ToString("N"));
        using (var main = new EditorDocument { FileName = Path.Combine(folder, "Main.pas") })
        using (var unit = new EditorDocument { FileName = Path.Combine(folder, "NavUnit.pas") })
        using (var parent = new Form { ShowInTaskbar = false, Opacity = 0, Size = new Size(800, 500) })
        {
            const string text = "uses NavUnit;\r\nbegin\r\n  p1;\r\nend.";
            const string unitText = "unit NavUnit;\r\ninterface\r\nprocedure p1;\r\nimplementation\r\n\r\nprocedure p1;\r\nbegin Print(3) end;\r\nend.";
            Directory.CreateDirectory(folder);
            const string savedUnit = "unit NavUnit; interface procedure p1; implementation procedure p1; begin Print(2) end; end.";
            File.WriteAllText(unit.FileName, savedUnit);
            main.TextEditor.Document.TextContent = text;
            unit.TextEditor.Document.TextContent = unitText;
            parent.Controls.Add(main.TextEditor); main.TextEditor.Dock = DockStyle.Fill;
            var open = new List<ICodeFileDocument> { main, unit };
            var events = new WorkbenchDocumentEventSource(() => open);
            var fake = new FakeLanguageClient { Request = (method, value, token) => Task.FromResult(
                method == "textDocument/implementation" ? (object)new ArrayList() : NavigationResponse(unit.FileName, 5, 0)) };
            var sync = new Net10LanguageDocuments(events, () => runtime == null ? (INet10LanguageClient)fake :
                new Net10LanguageClient(runtime, "dotnet", "ru"), value => { });
            var targets = new List<Net10NavigationTarget>(); int jumps = 0; Keys modifiers = Keys.Control;
            using (var ui = new Net10Navigation(events, sync, () => true, result => { targets = result; jumps++; }, () => modifiers))
            {
                try
                {
                    sync.SetActive(true); sync.WhenIdleAsync().GetAwaiter().GetResult(); ui.SetActive(true);
                    parent.Show(); var area = main.TextEditor.ActiveTextAreaControl.TextArea; area.Focus(); Application.DoEvents();
                    var flags = BindingFlags.Instance | BindingFlags.NonPublic;
                    Action<MouseButtons> click = button =>
                    {
                        var point = main.TextEditor.Document.OffsetToPosition(main.TextEditor.Document.TextContent.IndexOf("p1;", StringComparison.Ordinal) + 1);
                        area.Caret.Position = point;
                        int x = area.TextView.DrawingPosition.X + area.TextView.GetDrawingXPos(point.Y, point.X);
                        int y = area.TextView.DrawingPosition.Y + point.Y * area.TextView.FontHeight + area.TextView.FontHeight / 2;
                        var args = new MouseEventArgs(button, 1, x, y, 0);
                        typeof(TextArea).GetMethod("OnMouseDown", flags).Invoke(area, new object[] { args });
                        typeof(TextArea).GetMethod("OnMouseClick", flags).Invoke(area, new object[] { args });
                    };
                    modifiers = Keys.None; click(MouseButtons.Left); Application.DoEvents();
                    modifiers = Keys.Control | Keys.Shift; click(MouseButtons.Left); Application.DoEvents();
                    modifiers = Keys.Control; click(MouseButtons.Right); Application.DoEvents();
                    Check(jumps == 0, "ordinary click, Ctrl-Shift-click and right-click do not navigate");
                    var movePosition = area.Caret.Position;
                    var move = new MouseEventArgs(MouseButtons.None, 0,
                        area.TextView.DrawingPosition.X + area.TextView.GetDrawingXPos(movePosition.Y, movePosition.X),
                        area.TextView.DrawingPosition.Y + movePosition.Y * area.TextView.FontHeight + area.TextView.FontHeight / 2, 0);
                    var legacy = new TextMarker(0, 1, TextMarkerType.Underlined, Color.Red);
                    area.Document.MarkerStrategy.AddMarker(legacy);
                    typeof(TextArea).GetMethod("OnMouseMove", flags).Invoke(area, new object[] { move });
                    var firstUnderline = area.Document.MarkerStrategy.GetMarkers(area.Caret.Offset)
                        .FirstOrDefault(marker => marker.Color == Color.Blue);
                    Check(firstUnderline != null,
                        "Ctrl-hover underlines the source identifier with the navigation-owned marker");
                    if (runtime == null)
                    {
                        for (int repeat = 0; repeat < 20; repeat++)
                            typeof(TextArea).GetMethod("OnMouseMove", flags).Invoke(area, new object[] { move });
                        typeof(TextArea).GetMethod("OnKeyDown", flags).Invoke(area,
                            new object[] { new KeyEventArgs(Keys.ControlKey) });
                        Check(ReferenceEquals(firstUnderline, area.Document.MarkerStrategy.GetMarkers(area.Caret.Offset)
                            .FirstOrDefault(marker => marker.Color == Color.Blue)),
                            "held Ctrl and repeated hover events keep one stable underline instead of recreating it");
                    }
                    click(MouseButtons.Left); PumpNavigation(() => jumps == 1);
                    Check(targets.Count == 1 && targets[0].FileName == unit.FileName && targets[0].Line == 6,
                        "real Ctrl-left-click finds the editor's unsaved module implementation, not its older disk version" + (runtime == null ? " via definition fallback" : " through LanguageServer"));
                    if (runtime == null)
                    {
                        string[] methods = fake.Messages.Select(message => message.Item1).ToArray();
                        int implementation = Array.LastIndexOf(methods, "textDocument/implementation");
                        Check(implementation > 0 && methods[implementation + 1] == "textDocument/definition" &&
                            fake.Messages.Any(message => message.Item1 == "textDocument/didOpen" && (string)DocumentValue(message, "text") == unitText),
                            "implementation fallback shares the queue and includes unsaved dependency text");
                        var reply = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
                        CancellationToken pendingToken = default(CancellationToken);
                        fake.Request = (method, value, token) => { pendingToken = token; return reply.Task; };
                        int count = fake.Messages.Count;
                        click(MouseButtons.Left);
                        PumpNavigation(() => fake.Messages.Count > count && pendingToken.CanBeCanceled);
                        typeof(TextArea).GetMethod("OnKeyUp", flags).Invoke(area, new object[] { new KeyEventArgs(Keys.ControlKey) });
                        Check(!pendingToken.IsCancellationRequested, "releasing Ctrl after the click does not cancel navigation");
                        unit.TextEditor.Document.TextContent += " ";
                        Check(pendingToken.IsCancellationRequested, "dependency edit cancels the in-flight navigation token");
                        reply.SetResult(NavigationResponse(unit.FileName));
                        Application.DoEvents();
                        Check(jumps == 1, "editing a dependent unsaved module cancels a late navigation response");
                    }
                    else
                    {
                        main.TextEditor.Document.TextContent = "begin var s := 'abc'; s.Substring(1); end.";
                        int offset = main.TextEditor.Document.TextContent.IndexOf("Substring", StringComparison.Ordinal) + 2;
                        var position = main.TextEditor.Document.OffsetToPosition(offset);
                        var response = sync.NavigationAsync(main, position.Y, position.X, CancellationToken.None).GetAwaiter().GetResult();
                        var metadata = Net10NavigationTarget.Read(response);
                        Check(metadata.Count == 1 && metadata[0].FileName == null && metadata[0].MetadataText.Contains("Substring"),
                            "CLR member definition returns on-demand metadata for the standard IDE metadata tab");
                        CheckMetadataHover(sync, metadata[0], open, events);
                        main.TextEditor.Document.TextContent = "begin var local := 1; Print(local); end.";
                        offset = main.TextEditor.Document.TextContent.LastIndexOf("local", StringComparison.Ordinal) + 2;
                        position = main.TextEditor.Document.OffsetToPosition(offset);
                        response = sync.NavigationAsync(main, position.Y, position.X, CancellationToken.None).GetAwaiter().GetResult();
                        var local = Net10NavigationTarget.Read(response);
                        Check(local.Count == 1 && local[0].FileName == main.FileName && local[0].Line == 1,
                            "local variable falls back to source definition rather than implementation");
                    }
                    // Replacing the document above legitimately clears all old markers.
                    if (!area.Document.MarkerStrategy.GetMarkers(0).Contains(legacy))
                        area.Document.MarkerStrategy.AddMarker(legacy);
                    ui.SetActive(false);
                    click(MouseButtons.Left); Application.DoEvents();
                    Check(jumps == 1 && area.Document.MarkerStrategy.GetMarkers(0).Contains(legacy),
                        "legacy mode detaches Ctrl-click and preserves markers owned by the IDE");
                    Check(!File.Exists(main.FileName) && File.ReadAllText(unit.FileName) == savedUnit, "navigation never saves or overwrites editor sources");
                }
                finally { parent.Hide(); sync.ShutdownAsync().GetAwaiter().GetResult(); File.Delete(unit.FileName); Directory.Delete(folder); }
            }
        }
    }
}
