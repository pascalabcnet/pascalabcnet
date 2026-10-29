using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

internal static class Program
{
    private static void Check(bool value, string message)
    {
        if (!value) throw new Exception(message);
        Console.WriteLine("PASS " + message);
    }
    private static object Call(object target, string name, params object[] args) => target.GetType().GetMethod(name).Invoke(target, args);
    private static void Flag(object target, string name, object value) => target.GetType().GetField(name).SetValue(target, value);
    [STAThread]
    private static int Main(string[] args)
    {
        try
        {
            string path = Path.GetFullPath(args.Length == 0 ? "bin/PascalABCNETLinux.exe" : args[0]);
            AppDomain.CurrentDomain.AssemblyResolve += (sender, e) => {
                string file = Path.Combine(Path.GetDirectoryName(path), new AssemblyName(e.Name).Name + ".dll");
                return File.Exists(file) ? Assembly.LoadFrom(file) : null;
            };
            Assembly ide = Assembly.LoadFrom(path);
            object defaults = Activator.CreateInstance(ide.GetType("VisualPascalABC.UserOptions", true));
            Check((int)defaults.GetType().GetField("EditorFontSize").GetValue(defaults) == 11,
                "shipped IDE defaults to editor font size 11");
            Assembly editorAssembly = Assembly.LoadFrom(Path.Combine(Path.GetDirectoryName(path), "ICSharpCode.TextEditorLinux.dll"));
            object editorDefaults = Activator.CreateInstance(editorAssembly.GetType("ICSharpCode.TextEditor.Document.DefaultTextEditorProperties", true));
            var defaultFont = (Font)editorDefaults.GetType().GetProperty("Font").GetValue(editorDefaults, null);
            Check(Math.Abs(defaultFont.SizeInPoints - 11f) < 0.01f, "shipped editor library defaults to 11 points");
            if (args.Length > 1 && args[1] == "--font-defaults-only") return 0;
            if (args.Length > 1 && args[1] == "--confirmation-only") { TestConfirmation(ide); return 0; }
            TestToolbarTips(ide);
            TestRunButton(ide);
            TestTooltipBitmap(ide);
            TestErrorIcons(ide);
            Type textType = ide.GetType("ICSharpCode.TextEditor.Util.TipText", true);
            Type sectionType = ide.GetType("ICSharpCode.TextEditor.Util.TipSection", true);
            Type splitterType = ide.GetType("ICSharpCode.TextEditor.Util.TipSplitter", true);
            Type painter = ide.GetType("ICSharpCode.TextEditor.Util.TipPainter", true);
            var measure = painter.GetMethods().Single(m => m.Name == "GetTipSize" && m.GetParameters().Length == 3);
            var draw = painter.GetMethods().Single(m => m.Name == "DrawTip" && m.GetParameters().Length == 3);
            foreach (float fontSize in new[] { 9f, 11f, 18f })
            using (var font = new Font(FontFamily.GenericSansSerif, fontSize))
            using (var bitmap = new Bitmap(700, 150))
            using (var graphics = Graphics.FromImage(bitmap))
            using (var control = new Control())
            {
                foreach (string label in new[] { "x", "integer", "procedure Down;", "Сохранить все" })
                {
                    object text = Activator.CreateInstance(textType, graphics, font, label);
                    Call(text, "SetMaximumSize", new SizeF(600, 120));
                    SizeF required = (SizeF)Call(text, "GetRequiredSize");
                    Array children = Array.CreateInstance(sectionType, 1); children.SetValue(text, 0);
                    object stack = Activator.CreateInstance(splitterType, graphics, false, children);
                    Call(stack, "SetMaximumSize", new SizeF(600, 120));
                    SizeF stacked = (SizeF)Call(stack, "GetRequiredSize");
                    Check(Math.Abs(stacked.Width - required.Width) < 0.1f, "vertical layout adds no right-only padding: " + label);
                    Size size = (Size)measure.Invoke(null, new[] { control, graphics, stack });
                    Check(size.Width == (int)Math.Ceiling(required.Width + 12), "popup reserves equal six-pixel side margins: " + label);
                    graphics.Clear(SystemColors.Info);
                    draw.Invoke(null, new[] { control, graphics, stack });
                    int left = size.Width, right = -1;
                    for (int x = 1; x < size.Width - 1; x++)
                    for (int y = 1; y < size.Height - 1; y++)
                        if (bitmap.GetPixel(x, y).ToArgb() != SystemColors.Info.ToArgb()) { left = Math.Min(left, x); right = Math.Max(right, x); }
                    Check(right >= left && left >= 5 && size.Width - 1 - right >= 5,
                        "painted text stays clear of both borders: " + label);
                }
                object ordinary = Activator.CreateInstance(textType, graphics, font, "Description");
                object indented = Activator.CreateInstance(textType, graphics, font, "Description");
                Flag(indented, "need_tab", true);
                Call(ordinary, "SetMaximumSize", new SizeF(600, 120));
                Call(indented, "SetMaximumSize", new SizeF(600, 120));
                Check(Math.Abs(((SizeF)Call(indented, "GetRequiredSize")).Width - ((SizeF)Call(ordinary, "GetRequiredSize")).Width - 10) < 0.1f,
                    "description measurement matches its actual ten-pixel indent");
                Flag(indented, "param_desc", true);
                Call(indented, "SetMaximumSize", new SizeF(599, 120));
                Check(Math.Abs(((SizeF)Call(indented, "GetRequiredSize")).Width - ((SizeF)Call(ordinary, "GetRequiredSize")).Width - 20) < 0.1f,
                    "parameter documentation measurement matches its twenty-pixel indent");
                const string signature = "procedure Foo(x: integer);";
                object mixed = Activator.CreateInstance(textType, graphics, font, signature);
                int selected = signature.IndexOf('x');
                Flag(mixed, "beg_bold", selected); Flag(mixed, "len_bold", 1);
                Call(mixed, "SetMaximumSize", new SizeF(600, 120));
                var format = (StringFormat)Call(mixed, "GetInternalStringFormat");
                using (var bold = new Font(font, FontStyle.Bold))
                {
                    float expected = graphics.MeasureString(signature.Substring(0, selected), font, new SizeF(600, 120), format).Width +
                        graphics.MeasureString("x", bold, new SizeF(600, 120), format).Width +
                        graphics.MeasureString(signature.Substring(selected + 1), font, new SizeF(600, 120), format).Width;
                    Check(Math.Abs(((SizeF)Call(mixed, "GetRequiredSize")).Width - expected) < 1,
                        "selected parameter reserves the actual mixed-font width, not an all-bold signature");
                }
            }
            return 0;
        }
        catch (Exception e) { Console.Error.WriteLine(e); return 1; }
    }

    private static void TestToolbarTips(Assembly ide)
    {
        Type tips = ide.GetType("VisualPascalABC.ToolbarToolTips", true);
        const BindingFlags methods = BindingFlags.Static | BindingFlags.NonPublic;
        MethodInfo getText = tips.GetMethod("GetText", methods);
        foreach (string caption in new[] { "Открыть", "Сохранить", "Форматировать код", "Здоровье кода", "Окно вывода" })
        using (var button = new ToolStripButton { Text = caption, AutoToolTip = true })
            Check((string)getText.Invoke(null, new object[] { button }) == caption,
                "automatic toolbar tooltip uses the command caption: " + caption);
        using (var button = new ToolStripButton { Text = "Открыть", ToolTipText = "Открыть файл" })
            Check((string)getText.Invoke(null, new object[] { button }) == "Открыть файл", "explicit toolbar tooltip takes precedence");
        using (var button = new ToolStripButton { Text = "Открыть", AutoToolTip = false })
            Check(string.IsNullOrEmpty((string)getText.Invoke(null, new object[] { button })), "disabled automatic captions remain disabled");
        foreach (float points in new[] { 9f, 11f, 18f })
        using (var font = new Font(FontFamily.GenericSansSerif, points))
        foreach (string text in new[] { "x", "Сохранить все", "Открыть", "Компилировать" })
        using (var bitmap = new Bitmap(600, 100))
        using (var graphics = Graphics.FromImage(bitmap))
        using (var format = new StringFormat(StringFormat.GenericTypographic))
        {
            Size size = (Size)tips.GetMethod("Measure", methods).Invoke(null, new object[] { graphics, font, text });
            Size expected = Size.Ceiling(graphics.MeasureString(text, font, int.MaxValue, format));
            Check(size.Width == expected.Width + 12, "toolbar tooltip has equal six-pixel side margins: " + text);
            tips.GetMethod("Paint", methods).Invoke(null, new object[] { graphics, new Rectangle(Point.Empty, size), font, text });
            int left = size.Width, right = -1;
            for (int x = 1; x < size.Width - 1; x++)
            for (int y = 1; y < size.Height - 1; y++)
                if (bitmap.GetPixel(x, y).ToArgb() != SystemColors.Info.ToArgb()) { left = Math.Min(left, x); right = Math.Max(right, x); }
            Check(right >= left && left >= 5 && size.Width - 1 - right >= 4,
                "toolbar tooltip text fits inside both margins: " + text);
        }
        using (var container = new System.ComponentModel.Container())
        using (var owner = new Form())
        using (var toolbar = new ToolStrip())
        {
            owner.Controls.Add(toolbar);
            owner.Show();
            var component = (IDisposable)Activator.CreateInstance(tips, container, toolbar, owner);
            Check(!toolbar.ShowItemToolTips, "toolbar uses only one tooltip provider");
            toolbar.Items.Add(new ToolStripButton { Text = "Save", ToolTipText = "Сохранить все" });
            toolbar.PerformLayout();
            typeof(ToolStripItem).GetMethod("HandleMouseEnter", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(toolbar.Items[0],
                new object[] { EventArgs.Empty });
            var timer = (Timer)tips.GetField("delay", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(component);
            Check(timer.Enabled, "hovering a toolbar button starts the tooltip delay");
            typeof(Control).GetMethod("OnMouseLeave", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(toolbar,
                new object[] { EventArgs.Empty });
            Check(!timer.Enabled, "leaving the toolbar cancels the pending tooltip");
            component.Dispose();
            Check(toolbar.ShowItemToolTips, "disposing toolbar tooltip restores standard behavior");
        }
    }

    private static void TestRunButton(Assembly ide)
    {
        Type buttonType = ide.GetType("VisualPascalABC.RunToolStripButton", true);
        using (var owner = new Form())
        using (var toolbar = new ToolStrip())
        using (var editor = new TextBox())
        using (var button = (ToolStripButton)Activator.CreateInstance(buttonType))
        {
            owner.StartPosition = FormStartPosition.Manual;
            // Keep this test window away from the real pointer, without moving it.
            Point pointer = Cursor.Position;
            owner.Location = new Point(pointer.X + 100, pointer.Y + 100);
            owner.Controls.Add(editor);
            owner.Controls.Add(toolbar);
            toolbar.Items.Add(button);
            owner.Show();
            editor.Focus();
            for (int i = 0; i < 3; i++)
            {
                button.Select();
                Check(button.Selected, "run button can still highlight normally");
                button.Enabled = false;
                button.Enabled = true;
                Check(!button.Selected && !button.Pressed, "run button clears stale highlight after disable/enable");
                Check(editor.Focused, "clearing run highlight keeps focus in the editor");
            }
        }
    }

    private static void TestTooltipBitmap(Assembly ide)
    {
        Type tools = ide.GetType("ICSharpCode.TextEditor.Util.TipPainterTools", true);
        MethodInfo measure = tools.GetMethod("GetDrawingSizeHelpTipFromCombinedDescription");
        MethodInfo draw = tools.GetMethod("DrawHelpTipFromCombinedDescription");
        foreach (float dpi in new[] { 96f, 144f, 192f })
        using (var layout = new Control())
        using (var font = new Font(FontFamily.GenericSansSerif, 9f))
        using (var probe = new Bitmap(1, 1))
        {
            probe.SetResolution(dpi, dpi);
            const string description = "procedure TurtleABC.Down();\nОпускает перо Черепахи";
            Size size;
            using (Graphics graphics = Graphics.FromImage(probe))
                size = (Size)measure.Invoke(null, new object[] { layout, graphics, font, null, description, -1, -1, false });
            using (var bitmap = new Bitmap(size.Width, size.Height))
            {
            bitmap.SetResolution(dpi, dpi);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(SystemColors.Info);
                Size painted = (Size)draw.Invoke(null, new object[] { layout, graphics, font, null, description, -1, -1, false });
                Check(size == painted, "documentation tooltip measures and draws at the same size, DPI " + dpi);
            }
            int border = SystemColors.WindowFrame.ToArgb();
            for (int x = 0; x < bitmap.Width; x++)
                CheckPixel(bitmap.GetPixel(x, 0).ToArgb() == border && bitmap.GetPixel(x, bitmap.Height - 1).ToArgb() == border, "horizontal x=" + x + " dpi=" + dpi);
            for (int y = 0; y < bitmap.Height; y++)
                CheckPixel(bitmap.GetPixel(0, y).ToArgb() == border && bitmap.GetPixel(bitmap.Width - 1, y).ToArgb() == border, "vertical y=" + y + " dpi=" + dpi);
            Check(true, "documentation tooltip has four complete borders at DPI " + dpi);
            for (int y = bitmap.Height - 3; y < bitmap.Height - 1; y++)
            for (int x = 1; x < bitmap.Width - 1; x++)
                CheckPixel(bitmap.GetPixel(x, y).ToArgb() == SystemColors.Info.ToArgb(), "bottom x=" + x + " y=" + y + " dpi=" + dpi);
            Check(true, "documentation tooltip keeps a clear bottom margin at DPI " + dpi);
            }
        }
    }

    private static void CheckPixel(bool valid, string position)
    {
        if (!valid) throw new Exception("Tooltip border or bottom margin is clipped: " + position);
    }

    private static void TestErrorIcons(Assembly ide)
    {
        Type type = ide.GetType("VisualPascalABC.ErrorsListWindowForm", true);
        using (var window = (Form)Activator.CreateInstance(type))
        {
            var list = (ListView)type.GetField("lvErrorsList", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(window);
            Size originalSize = list.SmallImageList.ImageSize;
            int count = list.SmallImageList.Images.Count;
            type.GetMethod("AddErrorIconPadding", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(window, null);
            Check(list.SmallImageList.ImageSize == originalSize + new Size(6, 6), "error icons reserve three-pixel margins");
            Check(list.SmallImageList.Images.Count == count, "error, warning and runtime error icon indices are preserved");
            foreach (Image image in list.SmallImageList.Images)
            using (var bitmap = new Bitmap(image))
                Check(bitmap.Width == originalSize.Width + 6, "padded error icon can be rendered after initialization");
            var item = new ListViewItem("") { ImageIndex = 1 };
            list.Items.Add(item);
            MethodInfo paint = type.GetMethod("DrawErrorCell", BindingFlags.NonPublic | BindingFlags.Instance);
            using (var bitmap = new Bitmap(list.Columns[0].Width, 50))
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                for (int repeat = 0; repeat < 3; repeat++)
                {
                    graphics.Clear(Color.White);
                    var e = new DrawListViewSubItemEventArgs(graphics, new Rectangle(Point.Empty, bitmap.Size),
                        item, item.SubItems[0], 0, 0, list.Columns[0], ListViewItemStates.Default);
                    paint.Invoke(window, new object[] { list, e });
                    int left = bitmap.Width, right = -1, top = bitmap.Height, bottom = -1;
                    int background = bitmap.GetPixel(0, 0).ToArgb();
                    for (int y = 0; y < bitmap.Height; y++)
                    for (int x = 0; x < bitmap.Width; x++)
                        if (bitmap.GetPixel(x, y).ToArgb() != background)
                        { left = Math.Min(left, x); right = Math.Max(right, x); top = Math.Min(top, y); bottom = Math.Max(bottom, y); }
                    Check(right >= left && left >= 3 && top >= 3 && right < bitmap.Width - 3 && bottom < bitmap.Height - 3,
                        "error glyph has clear margins on all four sides on repeated paints");
                }
            }
        }
    }

    private static void TestConfirmation(Assembly ide)
    {
        MethodInfo create = ide.GetType("VisualPascalABC.ConfirmationDialog", true)
            .GetMethod("Create", BindingFlags.NonPublic | BindingFlags.Static);
        foreach (string action in new[] { "Да", "Нет", "Отмена", "Enter", "Escape", "Close" })
        using (var owner = new Form())
        using (var dialog = (Form)create.Invoke(null, new object[] { "Сохранить изменения в файле 'dh.pas'?", "Подтверждение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question }))
        using (var timer = new Timer { Interval = 100 })
        {
            Button[] buttons = dialog.Controls[0].Controls.OfType<FlowLayoutPanel>().Single().Controls.OfType<Button>().ToArray();
            Check(string.Join(",", buttons.Select(b => b.Text)) == "Да,Нет,Отмена", "confirmation buttons have Russian captions");
            owner.Show();
            Exception failure = null;
            timer.Tick += (sender, e) => {
                timer.Stop();
                try
                {
                    if (action == "Close") dialog.Close();
                    else if (action == "Enter" || action == "Escape")
                        typeof(Form).GetMethod("ProcessDialogKey", BindingFlags.NonPublic | BindingFlags.Instance)
                            .Invoke(dialog, new object[] { action == "Enter" ? Keys.Enter : Keys.Escape });
                    else buttons.Single(b => b.Text == action).PerformClick();
                }
                catch (Exception error) { failure = error; dialog.Close(); }
            };
            timer.Start();
            DialogResult result = dialog.ShowDialog(owner);
            if (failure != null) throw failure;
            DialogResult expected = action == "Да" || action == "Enter" ? DialogResult.Yes : action == "Нет" ? DialogResult.No : DialogResult.Cancel;
            Check(result == expected && owner.Enabled, "confirmation preserves result and restores owner: " + action);
        }
    }
}
