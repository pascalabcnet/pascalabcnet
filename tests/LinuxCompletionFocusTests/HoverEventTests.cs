using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Windows.Forms;

internal static class HoverEventTests
{
    internal static void Run(Assembly ide, Assembly editorAssembly, Action<bool, string> check, Action pump)
    {
        string directory = Path.GetDirectoryName(ide.Location);
        Assembly language = Assembly.LoadFrom(Path.Combine(directory, "PascalABCLanguageInfo.dll"));
        language.GetType("Languages.Pascal.PascalLanguageRegistration", true).GetMethod("RegisterPascalLanguage").Invoke(null, null);
        Assembly completion = Assembly.LoadFrom(Path.Combine(directory, "CodeCompletion.dll"));
        Type controller = completion.GetType("CodeCompletion.CodeCompletionController", true);
        controller.GetMethod("SetLanguage").Invoke(null, new object[] { "hover-test.pas" });
        check((bool)controller.GetMethod("IntellisenseAvailable").Invoke(null, null), "hover test uses the real registered Pascal language");

        Type service = ide.GetType("VisualPascalABC.TooltipServiceManager", true);
        FieldInfo field = service.GetField("dvw", BindingFlags.Static | BindingFlags.NonPublic);
        object previous = field.GetValue(null);
        using (var owner = new Form { Text = "Linux hover event test", Size = new Size(600, 300), ShowInTaskbar = false })
        using (var editor = (Control)Activator.CreateInstance(editorAssembly.GetType("ICSharpCode.TextEditor.TextEditorControl", true)))
        using (var otherEditor = (Control)Activator.CreateInstance(editorAssembly.GetType("ICSharpCode.TextEditor.TextEditorControl", true)))
        using (dynamic window = Activator.CreateInstance(ide.GetType("ICSharpCode.TextEditor.Gui.CompletionWindow.DeclarationWindow", true), owner))
        {
            editor.Dock = DockStyle.Fill; owner.Controls.Add(editor); owner.Controls.Add(otherEditor);
            otherEditor.Visible = false; owner.Show(); pump();
            object active = editor.GetType().GetProperty("ActiveTextAreaControl").GetValue(editor, null);
            var area = (Control)active.GetType().GetProperty("TextArea").GetValue(active, null);
            object other = otherEditor.GetType().GetProperty("ActiveTextAreaControl").GetValue(otherEditor, null);
            var inactiveArea = (Control)other.GetType().GetProperty("TextArea").GetValue(other, null);
            // These tests inject timer requests explicitly. The physical desktop
            // pointer must not generate unrelated requests during the assertions.
            foreach (Control controlledArea in new[] { area, inactiveArea })
                ((Timer)controlledArea.GetType().GetField("HoverTimer", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(controlledArea)).Stop();
            window.GetType().GetProperty("Description").SetValue(window, "var counter: integer;", null);
            field.SetValue(null, window);
            FieldInfo ownerArea = service.GetField("hintTextArea", BindingFlags.Static | BindingFlags.NonPublic);
            FieldInfo visibleFlag = service.GetField("toolTipVisible", BindingFlags.Static | BindingFlags.NonPublic);
            bool previousVisible = (bool)visibleFlag.GetValue(null);
            visibleFlag.SetValue(null, true);
            ownerArea?.SetValue(null, area);
            try
            {
                Type eventType = editorAssembly.GetType("ICSharpCode.TextEditor.ToolTipRequestEventArgs", true);
                Type positionType = editorAssembly.GetType("ICSharpCode.TextEditor.TextLocation", true);
                object position = Activator.CreateInstance(positionType, 0, 0);
                object outside = Activator.CreateInstance(eventType, new Point(-1, -1), position, false);
                service.GetMethod("ToolTipService_TextAreaToolTipRequest").Invoke(null, new[] { inactiveArea, outside });
                pump();
                check(window.Visible && (string)window.GetType().GetProperty("Description").GetValue(window, null) == "var counter: integer;",
                    "hover timer from an inactive editor does not hide the visible editor's tooltip");
                object repeated = Activator.CreateInstance(eventType, new Point(40, 40), position, true);
                service.GetMethod("ToolTipService_TextAreaToolTipRequest").Invoke(null, new[] { area, repeated });
                pump();
                check(window.Visible, "a repeated hover request keeps the existing tooltip visible");
                service.GetMethod("ToolTipService_TextAreaMouseMove").Invoke(null,
                    new object[] { inactiveArea, new MouseEventArgs(MouseButtons.None, 0, 400, 200, 0) });
                check(window.Visible, "mouse movement reported by another editor does not hide the active tooltip");
                service.GetMethod("ToolTipService_TextAreaToolTipRequest").Invoke(null, new[] { area, outside });
                check(!window.Visible, "moving outside the owning editor's document still hides its tooltip");
                window.GetType().GetProperty("Description").SetValue(window, "var counter: integer;", null);
                ownerArea.SetValue(null, area);
                visibleFlag.SetValue(null, true);
                object main = ide.GetType("VisualPascalABC.VisualPABCSingleton", true).GetProperty("MainForm").GetValue(null, null);
                PropertyInfo menuActive = main.GetType().GetProperty("MenuActive");
                menuActive.SetValue(main, true, null);
                try
                {
                    service.GetMethod("ToolTipService_TextAreaToolTipRequest").Invoke(null, new[] { area, repeated });
                    check(!window.Visible, "opening a menu still hides the tooltip");
                }
                finally { menuActive.SetValue(main, false, null); }
                window.GetType().GetProperty("Description").SetValue(window, "var counter: integer;", null);
                ownerArea.SetValue(null, area);
                visibleFlag.SetValue(null, true);
                service.GetMethod("ToolTipService_TextAreaMouseMove").Invoke(null,
                    new object[] { area, new MouseEventArgs(MouseButtons.None, 0, 400, 200, 0) });
                check(!window.Visible, "moving away within the owning editor still hides the tooltip");

                // Exercise actual name analysis, the editor event, the shell handler
                // and showing the description. Earlier tests only supplied window text.
                const string source = "var counter: integer;\nbegin\n  counter := 1;\nend.";
                string fileName = Path.Combine(directory, "hover-pipeline-test.pas");
                completion.GetType("CodeCompletion.CodeCompletionBootstrap", true).GetMethod("Initialize", new[] { typeof(string) })
                    .Invoke(null, new object[] { "en" });
                object compiler = controller.GetField("comp").GetValue(null);
                object options = compiler.GetType().GetProperty("CompilerOptions").GetValue(compiler, null);
                string repositoryBin = Path.GetFullPath(Path.Combine(directory, "..", "..", "bin"));
                string libraryRoot = Directory.Exists(Path.Combine(repositoryBin, "Lib")) ? repositoryBin : directory;
                options.GetType().GetField("SystemDirectory").SetValue(options, libraryRoot);
                var directories = (IDictionary)controller.GetField("StandartDirectories").GetValue(null);
                directories["%PABCSYSTEM%"] = libraryRoot;
                directories["%LIBSOURCEDIRECTORY%"] = Path.Combine(libraryRoot, "Lib");
                object model = controller.GetMethod("Compile").Invoke(Activator.CreateInstance(controller), new object[] { fileName, source });
                check((bool)model.GetType().GetField("is_compiled").GetValue(model),
                    "real Pascal analysis compiles the hover sample [error=" + model.GetType().GetProperty("LastConversionError").GetValue(model, null) + "]");
                var modules = (IDictionary)controller.GetField("comp_modules").GetValue(null);
                modules[fileName] = model;
                try
                {
                    editor.GetType().GetProperty("FileName").SetValue(editor, fileName, null);
                    object document = editor.GetType().GetProperty("Document").GetValue(editor, null);
                    document.GetType().GetProperty("TextContent").SetValue(document, source, null);
                    object namePosition = Activator.CreateInstance(positionType, 3, 2);
                    object nameRequest = Activator.CreateInstance(eventType, new Point(40, 40), namePosition, true);
                    string description = (string)service.GetMethod("GetPopupHintText", BindingFlags.Static | BindingFlags.NonPublic)
                        .Invoke(null, new[] { area, nameRequest });
                    check(!string.IsNullOrEmpty(description) && description.Contains("counter"),
                        "real hover text is resolved for the name counter");
                    EventInfo hoverEvent = area.GetType().GetEvent("ToolTipRequest");
                    Delegate handler = Delegate.CreateDelegate(hoverEvent.EventHandlerType,
                        service.GetMethod("ToolTipService_TextAreaToolTipRequest"));
                    hoverEvent.AddEventHandler(area, handler);
                    try
                    {
                        area.GetType().GetMethod("OnToolTipRequest", BindingFlags.Instance | BindingFlags.NonPublic)
                            .Invoke(area, new[] { nameRequest });
                        pump();
                        check(window.Visible && window.ClientSize.Width > 0 && window.ClientSize.Height > 0 &&
                            (string)window.GetType().GetProperty("Description").GetValue(window, null) == description,
                            "editor hover event resolves a real Pascal name and displays its description");
                        window.Hide();
                        area.GetType().GetMethod("OnToolTipRequest", BindingFlags.Instance | BindingFlags.NonPublic)
                            .Invoke(area, new[] { nameRequest });
                        pump();
                        check(window.Visible, "hover can reappear after its native tooltip was hidden on deactivation");
                        service.GetMethod("ToolTipService_TextAreaKeyDown").Invoke(null,
                            new object[] { area, new KeyEventArgs(Keys.Back) });
                        check(!window.Visible, "Backspace hides the hover description before editing");
                    }
                    finally { hoverEvent.RemoveEventHandler(area, handler); }
                }
                finally { modules.Remove(fileName); }
            }
            finally
            {
                Type trace = ide.GetType("VisualPascalABC.HoverTrace");
                string log = (string)trace?.GetField("path", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null);
                if (log != null && File.Exists(log)) Console.WriteLine("HOVER TRACE:\n" + File.ReadAllText(log));
                ownerArea?.SetValue(null, null);
                field.SetValue(null, previous);
                visibleFlag.SetValue(null, previousVisible);
            }
        }
        pump();
    }
}
