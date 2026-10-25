using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using VisualPascalABCPlugins;

internal static partial class Program
{
    private static async Task CheckRuntimeLocation(string runtime)
    {
        string folder = Path.Combine(Path.GetTempPath(), "Ошибка выполнения PDB " + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(folder);
        string file = Path.Combine(folder, "ReadLines.pas");
        const string source = "begin\n  foreach var s in ReadLines('missing-23.txt') do\n  begin\n  end;\nend.";
        File.WriteAllText(file, source);
        RuntimeExceptionInfo error = null;
        using (var compiler = new Net10ControllerClient(runtime, "dotnet"))
        {
            var result = await compiler.CompileAsync(file, folder, "__RedirectIOMode", debug: true);
            Check(result.success, "runtime location fixture compiles");
            using (var runner = new Net10ProgramRunner("dotnet", result.outputFile, folder, ""))
            {
                var running = runner.RunAsync(_ => { }, () => { }, exception: value => error = value);
                if (await Task.WhenAny(running, Task.Delay(30000)) != running)
                {
                    runner.Stop();
                    throw new TimeoutException("Runtime location fixture timed out");
                }
                Check(await running != 0 && error != null, "real runtime exception reaches the plugin");
            }
        }
        var frame = error.Frames.FirstOrDefault(value => value.FileName == file);
        Check(frame != null && frame.Line == 2, "runtime protocol preserves Pascal source path and line 2");
        Exception failure = null;
        var thread = new Thread(() =>
        {
            try
            {
                using (var editor = new TextEditorControl())
                {
                    editor.FileName = file;
                    editor.Document.TextContent = source;
                    // Same production editor bookmark used by Form1.ExecuteErrorPos(imageindex: 2).
                    VisualPascalABC.RuntimeErrorBookmark.SetPosition(editor, frame.Line);
                    var marker = editor.Document.MarkerStrategy.TextMarker.Single();
                    var line = editor.Document.GetLineSegment(1);
                    Check(marker.Offset == line.Offset && marker.Length == line.Length &&
                        marker.TextMarkerType == TextMarkerType.SolidBlock &&
                        marker.Color.ToArgb() == Color.OrangeRed.ToArgb() && marker.ForeColor.ToArgb() == Color.White.ToArgb(),
                        "real IDE editor highlights exactly Pascal line 2 in OrangeRed with white text");
                    Check(editor.Document.BookmarkManager.Marks.OfType<VisualPascalABC.RuntimeErrorBookmark>().Count() == 1,
                        "real IDE runtime bookmark is registered");
                    VisualPascalABC.RuntimeErrorBookmark.Remove();
                    Check(!editor.Document.MarkerStrategy.TextMarker.Any(), "runtime highlight can be removed for next run");
                }
            }
            catch (Exception exception) { failure = exception; }
            finally { VisualPascalABC.RuntimeErrorBookmark.Remove(); }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        if (!thread.Join(10000)) throw new TimeoutException("Runtime marker UI test timed out");
        if (failure != null) throw new Exception("Runtime marker UI test failed", failure);
    }
}
