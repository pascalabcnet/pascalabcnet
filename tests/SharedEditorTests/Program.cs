using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Util;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using System.Windows.Forms;

class CheckEditor
{
    static void CheckTextAreaKeys()
    {
        using (var editor = new TextEditorControl())
        {
            var area = editor.ActiveTextAreaControl.TextArea;
            int laterCalls = 0;
            ICSharpCode.TextEditor.KeyEventHandler first = ch => true;
            ICSharpCode.TextEditor.KeyEventHandler later = ch => { laterCalls++; return false; };
            area.KeyEventHandler += first;
            area.KeyEventHandler += later;
            var handle = typeof(TextArea).GetMethod("HandleKeyPress", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Check((bool)handle.Invoke(area, new object[] { ';' }) && laterCalls == 0,
                "Windows key consumption stops at the first accepting handler");
            area.KeyEventHandler -= first;
            Check(!(bool)handle.Invoke(area, new object[] { ';' }) && laterCalls == 1,
                "unhandled Windows key reaches the remaining handler");
            area.KeyEventHandler -= later;
            Check(!(bool)handle.Invoke(area, new object[] { ';' }), "key without subscribers is not consumed");
            var timer = typeof(TextArea).GetField("HoverTimer", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Check(timer == null || timer.GetValue(area) == null, "Windows editor does not create the Mono hover timer");
        }
    }
    static void CheckScale(double requested)
    {
        ScreenScale.scale = requested;
        double actual = ScreenScale.Calc();
        Check(actual > 0 && ScreenScale.Calc() == actual, "scale is positive and cached");
        if (requested > 0) Check(actual == requested, "explicit scale is retained");
        using (var form = new Form())
        using (var editor = new TextEditorControl())
        {
            form.Controls.Add(editor); editor.CreateControl();
            var margin = editor.ActiveTextAreaControl.TextArea.IconBarMargin;
            int width = 18 + Convert.ToInt32(Math.Max(0, (actual - 1) * 16));
            Check(margin.Size.Width == width, "icon margin width at the selected scale");
            using (var bitmap = new Bitmap(120, 100))
            using (var graphics = Graphics.FromImage(bitmap))
            using (var stream = new MemoryStream())
            using (var sha = SHA256.Create())
            {
                graphics.Clear(Color.White);
                margin.DrawingPosition = new Rectangle(0, 0, width, 100);
                margin.Paint(graphics, margin.DrawingPosition);
                if (Environment.GetEnvironmentVariable("PABC_EDITOR_TEST_IMAGES") == "1")
                    bitmap.Save(Path.Combine(Path.GetDirectoryName(typeof(CheckEditor).Assembly.Location), "margin.png"), ImageFormat.Png);
                bitmap.Save(stream, ImageFormat.Png);
                Console.WriteLine("SCALE " + actual + " WIDTH " + width + " IMAGE " + BitConverter.ToString(sha.ComputeHash(stream.ToArray())));
            }
        }
    }
    sealed class CompletionProbe : AbstractCompletionWindow
    {
        public CompletionProbe(Form owner, TextEditorControl editor) : base(owner, editor)
        {
            drawingSize = new Size(120, 40);
            SetLocation();
        }
        public void Open() { ShowCompletionWindow(); }
        public bool Key(Keys key) { return ProcessTextAreaKey(key); }
        public void RaiseEditorLostFocus() { TextEditorLostFocus(this, EventArgs.Empty); }
    }

    static void CheckCompletionLifecycle()
    {
        using (var form = new Form())
        using (var editor = new TextEditorControl { Dock = DockStyle.Bottom, Height = 60 })
        using (var input = new TextBox { Dock = DockStyle.Top })
        {
            form.Controls.Add(editor); form.Controls.Add(input);
            form.Show(); form.Activate(); input.Focus(); Application.DoEvents();
            using (var popup = new CompletionProbe(form, editor))
            {
                int closed = 0;
                popup.Closed += (sender, e) => closed++;
                popup.Open(); Application.DoEvents();
                Check(popup.Visible && editor.ContainsFocus, "completion opens and restores editor focus");
                Check(!popup.Key(Keys.F8), "unhandled completion key is not consumed");
                Check(popup.Key(Keys.Escape), "Escape closes completion");
                Check(popup.IsDisposed && closed == 1, "completion closes once and disposes");
            }
            using (var popup = new CompletionProbe(form, editor))
            {
                popup.Open(); Application.DoEvents();
                input.Focus(); Application.DoEvents();
                popup.RaiseEditorLostFocus();
                Check(popup.IsDisposed && input.Focused, "Windows completion closes on focus loss without pulling focus back");
            }
            form.Close();
        }
    }
    sealed class DeclarationProbe : DeclarationViewWindow
    {
        public DeclarationProbe(Form owner) : base(owner) { }
        public void RaiseGotFocus() { base.OnGotFocus(EventArgs.Empty); }
        public bool IsPassive { get { return ShowWithoutActivation; } }
    }

    static void CheckDeclarationFocus()
    {
        using (var form = new Form())
        using (var editor = new TextEditorControl { Dock = DockStyle.Bottom, Height = 60 })
        using (var input = new TextBox { Dock = DockStyle.Top })
        using (var popup = new DeclarationProbe(form))
        {
            form.Controls.Add(editor); form.Controls.Add(input);
            form.Show(); form.Activate(); input.Focus(); Application.DoEvents();
            Check(input.Focused, "focus test starts in a separate input control");
            var target = typeof(DeclarationViewWindow).GetProperty("TextEditorControl");
            if (target != null) target.SetValue(popup, editor, null);
            int notifications = 0;
            popup.GotFocus += (sender, e) => notifications++;
            popup.RaiseGotFocus();
            Check(input.Focused && !editor.Focused, "Windows declaration GotFocus does not pull focus into the editor");
            Check(notifications == 1, "declaration focus notification is forwarded once");
            Check(popup.IsPassive && !popup.ShowInTaskbar && popup.FormBorderStyle == FormBorderStyle.None,
                "declaration passive window settings are preserved");
            form.Close();
        }
    }
    static void CheckFonts()
    {
        foreach (string name in new[] { "Courier New", "Consolas", "Arial" })
        using (var initial = new Font(name, 10))
        {
            var container = new FontContainer(initial);
            foreach (float size in new[] { 10f, 11f, 14f, 20f })
            using (var input = new Font(name, size))
            {
                container.DefaultFont = input;
                Font[] variants = { container.RegularFont, container.BoldFont, container.ItalicFont, container.BoldItalicFont };
                FontStyle[] styles = { FontStyle.Regular, FontStyle.Bold, FontStyle.Italic, FontStyle.Bold | FontStyle.Italic };
                using (var bitmap = new Bitmap(640, 240))
                using (var graphics = Graphics.FromImage(bitmap))
                using (var stream = new MemoryStream())
                using (var sha = SHA256.Create())
                {
                    graphics.Clear(Color.White);
                    for (int i = 0; i < variants.Length; i++)
                    {
                        Check(variants[i].Style == styles[i] && variants[i].FontFamily.Name == input.FontFamily.Name,
                            "font variant " + name + " " + size + " " + styles[i]);
                        graphics.DrawString("begin Пример 0123456789 end", variants[i], Brushes.Black, 0, i * 55);
                        Console.WriteLine("FONT " + variants[i].Name + " " + variants[i].SizeInPoints + " " + variants[i].Height);
                    }
                    bitmap.Save(stream, ImageFormat.Png);
                    Console.WriteLine("FONTIMAGE " + name + " " + size + " " + BitConverter.ToString(sha.ComputeHash(stream.ToArray())));
                }
            }
            container.RegularFont.Dispose(); container.BoldFont.Dispose();
            container.ItalicFont.Dispose(); container.BoldItalicFont.Dispose();
        }
    }
    static void CheckTipRendering()
    {
        string[] descriptions = {
            "integer", "procedure Print(a: integer);\nВыводит значение.",
            "function VeryLongFunctionName(argument: integer): string;\nПервая строка.\nВторая строка.",
            "procedure P;\n"
        };
        for (int i = 0; i < descriptions.Length; i++)
        using (var form = new Form { StartPosition = FormStartPosition.Manual, Location = new Point(20, 20) })
        using (var control = new Label { Size = new Size(400, 200) })
        using (var bitmap = new Bitmap(640, 240))
        using (var graphics = Graphics.FromImage(bitmap))
        using (var font = new Font("Arial", 10))
        {
            form.Controls.Add(control);
            control.CreateControl();
            graphics.Clear(Color.White);
            string count = i == 1 ? "1 из 2" : "";
            var painter = typeof(TextEditorControl).Assembly.GetType("ICSharpCode.TextEditor.Util.TipPainterTools", true);
            object[] arguments = { control, graphics, font, count, descriptions[i] };
            Size measured = (Size)painter.GetMethod("GetDrawingSizeHelpTipFromCombinedDescription").Invoke(null, arguments);
            Size drawn = (Size)painter.GetMethod("DrawHelpTipFromCombinedDescription").Invoke(null, arguments);
            Check(measured.Width > 0 && drawn.Width > 0 && drawn.Height > 0, "tooltip measure and render " + i);
            using (var stream = new MemoryStream())
            using (var sha = SHA256.Create())
            {
                bitmap.Save(stream, ImageFormat.Png);
                Console.WriteLine("TIP " + i + " " + measured + " " + drawn + " " +
                    BitConverter.ToString(sha.ComputeHash(stream.ToArray())));
            }
        }
    }
    static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
        Console.WriteLine("PASS " + message);
    }

    [STAThread]
    static int Main(string[] args)
    {
        try
        {
            if (args.Length == 2 && args[0] == "--scale")
            {
                CheckScale(double.Parse(args[1], System.Globalization.CultureInfo.InvariantCulture));
                return 0;
            }

            var properties = new DefaultTextEditorProperties();
            Check(properties.Font.SizeInPoints == 10f && properties.Font.Name == "Courier New", "Windows default font remains Courier New 10");
            var editor = new TextEditorControl();
            using (var form = new Form())
            {
                form.Controls.Add((Control)editor);
                ((Control)editor).CreateControl();
                editor.Document.TextContent = "abc\nпривет";
                Check(editor.Document.TotalNumberOfLines == 2, "multiline Unicode document");
                editor.Document.UndoStack.ClearAll();
                editor.Document.Insert(0, "X");
                Check(editor.Document.TextContent == "Xabc\nпривет", "insert text");
                editor.Document.UndoStack.Undo();
                Check(editor.Document.TextContent == "abc\nпривет", "undo");
                editor.Document.UndoStack.Redo();
                Check(editor.Document.TextContent == "Xabc\nпривет", "redo");
                var area = editor.ActiveTextAreaControl.TextArea;
                Check(area.TextView.GetVisualColumn(0, 3) == 3, "visual column in ordinary text");
                editor.Document.TextContent = "";
                Check(area.TextView.GetVisualColumn(0, 0) == 0, "visual column in cleared document");
                editor.Document.TextContent = "abc";
                ((Control)editor).Enabled = false;
                Check(area.TextView.GetVisualColumn(0, 2) == 2, "column calculation when disabled");
                ((Control)editor).Enabled = true;
                string file = Path.Combine(Path.GetTempPath(), "shared-editor-" + Guid.NewGuid().ToString("N") + ".pas");
                editor.SaveFile(file);
                editor.Document.TextContent = "";
                editor.LoadFile(file);
                Check(editor.Document.TextContent == "abc", "save and reload");
            }
            CheckTipRendering();
            CheckFonts();
            CheckDeclarationFocus();
            CheckCompletionLifecycle();
            CheckTextAreaKeys();
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
    }
}
