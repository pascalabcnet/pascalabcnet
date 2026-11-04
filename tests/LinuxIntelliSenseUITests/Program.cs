using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using VisualPascalABC;
using VisualPascalABCPlugins;

internal static class Program
{
    private const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
    private static void Check(bool value, string message)
    { if (!value) throw new Exception(message); Console.WriteLine("PASS " + message); }
    private sealed class Document : ICodeFileDocument, IDisposable
    {
        public string FileName { get; set; } = Path.GetFullPath("ExternalHints.pas");
        public TextEditorControl TextEditor { get; } = new TextEditorControl();
        public string Text { get => TextEditor.Document.TextContent; set => TextEditor.Document.TextContent = value; }
        public string EXEFileName => null;
        public int LinesCount => TextEditor.Document.TotalNumberOfLines;
        public bool FromMetadata => false;
        public bool DocumentChanged => true;
        public string ToolTipText { get; set; }
        public bool Run { get; set; }
        public void LoadFromFile(string fileName) { throw new NotSupportedException(); }
        public void Dispose() => TextEditor.Dispose();
    }
    private sealed class Completion : ICompletionDataProvider
    {
        private sealed class Item : DefaultCompletionData
        {
            internal Item(string name, int icon) : base(name, "procedure " + name + ";", icon) { }
            public override bool InsertAction(TextArea area, char ch) { area.InsertString(Text); return false; }
        }
        private readonly ICompletionData[] items;
        public ImageList ImageList { get; }
        public string PreSelection { get; }
        public int DefaultIndex => 0;
        internal Completion(IExternalIntelliSenseSession ui, string prefix, params string[] names)
        {
            ImageList = ui.CompletionImages; PreSelection = prefix;
            items = Array.ConvertAll(names, name => (ICompletionData)new Item(name, ui.GetImageIndex(CompletionItemKind.ExtensionMethod)));
        }
        public ICompletionData[] GenerateCompletionData(string file, TextArea area, char ch) => (ICompletionData[])items.Clone();
        public CompletionDataProviderKeyResult ProcessKey(char ch) => char.IsLetterOrDigit(ch) ? CompletionDataProviderKeyResult.NormalKey : CompletionDataProviderKeyResult.InsertionKey;
        public bool InsertAction(ICompletionData data, TextArea area, int offset, char ch)
        { area.Caret.Position = area.Document.OffsetToPosition(offset); return data.InsertAction(area, ch); }
    }
    private sealed class Parameters : IExternalParameterInfo
    {
        private TextArea area;
        private readonly int anchor;
        internal Parameters(int anchor) { this.anchor = anchor; }
        public int CurrentParameter => 2;
        public int DefaultIndex => 0;
        public int InsightDataCount => 2;
        public string GetInsightData(int index) => "procedure Sample(a: integer; b: string);\nExternal documentation.";
        public void SetupDataProvider(string file, TextArea area) { this.area = area; }
        public bool CaretOffsetChanged()
        {
            int depth = 0;
            for (int i = anchor; i < area.Caret.Offset; i++)
            { char ch = area.Document.GetCharAt(i); if (ch == '(') depth++; else if (ch == ')') depth--; }
            return depth <= 0;
        }
        public bool GetParameterHighlight(int signature, out int start, out int length)
        { start = 29; length = 9; return true; }
    }
    [STAThread]
    private static int Main()
    {
        var singleton = typeof(VisualPABCSingleton).GetField("mainForm", BindingFlags.Static | BindingFlags.NonPublic);
        var previous = singleton.GetValue(null);
        var fake = (Form1)FormatterServices.GetUninitializedObject(typeof(Form1));
        GC.SuppressFinalize(fake);
        fake.UserOptions = new UserOptions { ShowCompletionInfoByGroup = true, EnableSmartIntellisense = true };
        singleton.SetValue(null, fake);
        try
        {
            Check(typeof(IWorkbenchIntelliSenseUI).IsAssignableFrom(typeof(WorkbenchServiceContainer)), "Linux exposes optional UI capability without enabling .NET 10");
            using (var parent = new Form { Width = 800, Height = 450, ShowInTaskbar = false, Opacity = 0 })
            using (var document = new Document())
            using (var ui = new ExternalIntelliSenseSession(parent))
            using (var newer = new ExternalIntelliSenseSession(parent))
            {
                parent.Controls.Add(document.TextEditor); document.TextEditor.Dock = DockStyle.Fill;
                var area = document.TextEditor.ActiveTextAreaControl.TextArea;
                Action<string> source = text => { document.TextEditor.Document.TextContent = text; area.Caret.Position = document.TextEditor.Document.OffsetToPosition(text.Length); };
                parent.Show(); area.Focus(); Application.DoEvents();
                int lostFocus = 0; area.LostFocus += (sender, e) => lostFocus++;
                Check(ui.GetImageIndex(CompletionItemKind.Method) != ui.GetImageIndex(CompletionItemKind.ExtensionMethod), "ordinary and extension methods keep different icons");
                foreach (var key in new[] { Keys.Enter, Keys.Tab, Keys.Escape })
                {
                    source("s.Su");
                    ui.ShowCompletion(document, new Completion(ui, "Su", "Substring"));
                    Check(area.Focused && lostFocus == 0, "external completion shows without taking focus: " + key);
                    Check(area.ExecuteDialogKey(key) && document.TextEditor.Document.TextContent == (key == Keys.Escape ? "s.Su" : "s.Substring"), "external completion replaces prefix once or cancels: " + key);
                    Check(!ui.ProcessCompletionKey('x'), "closed completion consumes no further text");
                }
                source("s.");
                ui.ShowCompletion(document, new Completion(ui, null, "Zebra", "Alpha"), true);
                area.ExecuteDialogKey(Keys.Enter);
                Check(document.TextEditor.Document.TextContent == "s.Zebra", "preserveOrder keeps provider ordering for package completion");
                source("Outer(Inner(");
                var outer = new Parameters(5); var inner = new Parameters(11);
                ui.ShowParameters(document, 5, outer); ui.ShowParameters(document, 11, inner);
                Check(ReferenceEquals(ui.ParameterProvider, inner) && area.Focused && lostFocus == 0, "external parameter stack shows without taking focus");
                area.SimulateKeyPress(')');
                Check(ReferenceEquals(ui.ParameterProvider, outer), "closing inner call restores outer external provider");
                var window = ui.GetType().GetField("parameters", Private).GetValue(ui);
                window.GetType().GetProperty("CurrentData", Private).SetValue(window, 1);
                ui.ShowParameters(document, 5, new Parameters(5));
                Check((int)window.GetType().GetMethod("GetCurrentData").Invoke(window, null) == 1, "parameter refresh preserves manually selected overload");
                Check(((Control)window).Font.Equals(typeof(Constants).GetMethod("GetCompletionFont", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { document.TextEditor.Font })), "external parameter font matches Linux completion font");
                ui.Hide(IntelliSenseHint.Parameters);
                ui.ShowHover(document, new TextLocation(0, 0), Point.Empty, "old description");
                newer.ShowHover(document, new TextLocation(0, 0), Point.Empty, "new description");
                ui.Hide(IntelliSenseHint.Hover);
                var hover = typeof(TooltipServiceManager).GetField("dvw", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
                Check((string)hover.GetType().GetProperty("Description").GetValue(hover) == "new description", "old session cannot hide a newer hover");
                ui.Dispose(); ui.Dispose();
                ui.ShowHover(document, new TextLocation(0, 0), Point.Empty, "late response");
                Check((string)hover.GetType().GetProperty("Description").GetValue(hover) == "new description" && area.Focused && lostFocus == 0, "disposed session ignores late hints; external hints never transfer focus");
                newer.Hide(IntelliSenseHint.Hover);
                foreach (string name in new[] { "X", new string('W', 100) })
                {
                    source("s.");
                    newer.ShowCompletion(document, new Completion(newer, null, name));
                    var popup = (Control)newer.GetType().GetField("completion", Private).GetValue(newer);
                    Check(popup.Width <= parent.ClientSize.Width / 3, "completion width is capped at one third of IDE");
                    Check(name.Length == 1 ? popup.Width < parent.ClientSize.Width / 3 : popup.Width == parent.ClientSize.Width / 3,
                        "completion width follows short and long content");
                    newer.Hide(IntelliSenseHint.Completion);
                }
                int originalWidth = parent.Width;
                parent.Width = 400;
                source("s.");
                newer.ShowCompletion(document, new Completion(newer, null, new string('W', 100)));
                var narrowPopup = (Control)newer.GetType().GetField("completion", Private).GetValue(newer);
                Check(narrowPopup.Width == Constants.CompletionWindowWidth, "narrow IDE keeps the previous minimum completion width");
                newer.Hide(IntelliSenseHint.Completion);
                parent.Width = originalWidth;
                var originalFont = document.TextEditor.Font;
                foreach (float editorSize in new[] { 11f, 14f, 11f })
                using (var font = new Font(originalFont.FontFamily, editorSize, FontStyle.Regular, GraphicsUnit.Point))
                {
                    document.TextEditor.Font = font;
                    source("s.");
                    newer.ShowCompletion(document, new Completion(newer, null, "Substring"));
                    var completionWindow = newer.GetType().GetField("completion", Private).GetValue(newer);
                    var list = (Control)completionWindow.GetType().GetField("codeCompletionListView", Private).GetValue(completionWindow);
                    Check(list.Font.SizeInPoints == editorSize - 0.5f, "completion follows editor font: " + editorSize);
                    newer.Hide(IntelliSenseHint.Completion);
                    source("Sample(");
                    newer.ShowParameters(document, 6, new Parameters(6));
                    var parameterWindow = (Control)newer.GetType().GetField("parameters", Private).GetValue(newer);
                    Check(parameterWindow.Font.Equals(list.Font), "yellow parameter hint matches white list");
                    newer.Hide(IntelliSenseHint.Parameters);
                    newer.ShowHover(document, new TextLocation(0, 0), Point.Empty, "font check");
                    Check(((Font)hover.GetType().GetProperty("Font").GetValue(hover)).Equals(list.Font), "reused yellow hover follows editor font");
                    newer.Hide(IntelliSenseHint.Hover);
                    document.TextEditor.Font = originalFont;
                }

            }
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
        finally { singleton.SetValue(null, previous); }
    }
}
