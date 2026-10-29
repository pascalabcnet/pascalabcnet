using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

internal static class InsightWindowTests
{
    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr window, int message, IntPtr wParam, IntPtr lParam);
    private const BindingFlags PrivateInstance = BindingFlags.Instance | BindingFlags.NonPublic;
    private static object Read(object target, string name) { return target.GetType().GetProperty(name).GetValue(target, null); }
    private static void Write(object target, string name, object value) { target.GetType().GetProperty(name).SetValue(target, value, null); }
    private static object Call(object target, string name, params object[] args) { return target.GetType().GetMethod(name).Invoke(target, args); }
    private static object PreparedProvider(Assembly ide, Control area, object document, int offset, params string[] signatures)
    {
        Type type = ide.GetType("VisualPascalABC.DefaultInsightDataProvider", true);
        object provider = Activator.CreateInstance(type, -1, true, '(');
        type.GetField("textArea", PrivateInstance).SetValue(provider, area);
        type.GetField("document", PrivateInstance).SetValue(provider, document);
        type.GetField("initialOffset", PrivateInstance).SetValue(provider, offset);
        type.GetField("methods", PrivateInstance).SetValue(provider, signatures);
        type.GetField("param_count").SetValue(provider, 2);
        return provider;
    }

    internal static void Run(Assembly ide, Assembly editorAssembly, Action<bool, string> check, Action pump)
    {
        Type popupType = ide.GetType("ICSharpCode.TextEditor.Gui.InsightWindow.PABCNETInsightWindow", true);
        using (var owner = new Form { Text = "Linux passive parameter hint test", Size = new Size(800, 450), ShowInTaskbar = false })
        using (var editor = (Control)Activator.CreateInstance(editorAssembly.GetType("ICSharpCode.TextEditor.TextEditorControl", true)))
        {
            editor.Dock = DockStyle.Fill; owner.Controls.Add(editor); owner.Show();
            object active = Read(editor, "ActiveTextAreaControl"), document = Read(editor, "Document");
            var area = (Control)Read(active, "TextArea");
            object caret = Read(active, "Caret");
            Action<string> setSource = source => {
                Write(document, "TextContent", source);
                Write(caret, "Position", document.GetType().GetMethod("OffsetToPosition").Invoke(document, new object[] { source.Length }));
            };
            Action<char> typeChar = ch => Call(area, "SimulateKeyPress", ch);
            owner.Activate(); area.Focus(); pump();
            int lostFocus = 0;
            area.LostFocus += (sender, args) => lostFocus++;
            setSource("Sample(");
            object outer = PreparedProvider(ide, area, document, 6,
                "procedure Sample(a: integer; b: string);\nFirst overload.",
                "procedure Sample(a: string);\nSecond overload.");
            using (var popup = (ToolStripDropDown)Activator.CreateInstance(popupType, owner, editor))
            {
                Call(popup, "AddInsightDataProvider", outer, "sample.pas");
                var sharedFont = (Font)ide.GetType("VisualPascalABC.Constants", true).GetField("CompletionFont", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
                check(Equals(popup.Font, sharedFont), "parameter popup uses the same font and units as completion and hover descriptions");
                int forms = Application.OpenForms.Count;
                Call(popup, "ShowInsightWindow"); pump();
                check(popup.Visible && popup.ClientSize.Width > 0 && popup.ClientSize.Height > 0 &&
                    area.Focused && !popup.ContainsFocus && !popup.Capture && lostFocus == 0 && Application.OpenForms.Count == forms,
                    "parameter hint shows without a Form, native focus, menu capture or transient editor focus loss");
                object[] activation = { Message.Create(popup.Handle, 0x21, owner.Handle, new IntPtr(0x02010001)) };
                popupType.GetMethod("WndProc", PrivateInstance).Invoke(popup, activation);
                check(((Message)activation[0]).Result == new IntPtr(3), "clicking the parameter popup requests MA_NOACTIVATE");

                Call(area, "ExecuteDialogKey", Keys.Down); pump();
                check((int)Call(popup, "GetCurrentData") == 1 && area.Focused, "Down in the editor selects the next overload");
                Call(area, "ExecuteDialogKey", Keys.Up); pump();
                check((int)Call(popup, "GetCurrentData") == 0 && area.Focused, "Up in the editor selects the previous overload");

                Rectangle next = (Rectangle)popupType.GetField("nextArrow", PrivateInstance).GetValue(popup);
                check(next.Width > 0 && next.Height > 0, "native parameter popup paints its overload arrows");
                popupType.GetMethod("OnMouseDown", PrivateInstance).Invoke(popup,
                    new object[] { new MouseEventArgs(MouseButtons.Left, 1, next.X + 1, next.Y + 1, 0) });
                pump();
                check((int)Call(popup, "GetCurrentData") == 1 && area.Focused && lostFocus == 0,
                    "clicking the overload arrow changes the signature without taking editor focus");
                Call(popup, "HandleMouseWheel", new MouseEventArgs(MouseButtons.None, 0, 0, 0, 120)); pump();
                check((int)Call(popup, "GetCurrentData") == 0 && area.Focused, "mouse wheel still changes overloads");
                if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                    next = (Rectangle)popupType.GetField("nextArrow", PrivateInstance).GetValue(popup);
                    IntPtr point = new IntPtr(((next.Y + 1) << 16) | ((next.X + 1) & 0xffff));
                    IntPtr activationResult = SendMessage(popup.Handle, 0x21, owner.Handle, new IntPtr(0x02010001));
                    SendMessage(popup.Handle, 0x201, new IntPtr(1), point);
                    SendMessage(popup.Handle, 0x202, IntPtr.Zero, point);
                    pump();
                    check(activationResult == new IntPtr(3) && (int)Call(popup, "GetCurrentData") == 1 &&
                        area.Focused && lostFocus == 0 && !popup.Capture,
                        "native mouse-message routing changes the overload without focus loss or retained mouse capture");
                }

                Point initialLocation = popup.Location;
                typeChar('1'); pump();
                check((string)Read(document, "TextContent") == "Sample(1" && popup.Visible && area.Focused,
                    "typing an argument goes to the editor while the parameter hint remains visible");
                check(popup.Location.X == initialLocation.X, "typing arguments does not make the parameter hint follow each character horizontally");
                foreach (char ch in ", Inner(") typeChar(ch);
                object nested = PreparedProvider(ide, area, document, (int)Read(caret, "Offset") - 1,
                    "function Inner(value: integer): integer;\nNested call.");
                Call(popup, "AddInsightDataProvider", nested, "sample.pas");
                Call(popup, "ShowInsightWindow"); pump();
                check((int)Read(popup, "InsightProviderStackLength") == 2, "a nested call keeps both parameter providers");
                typeChar('2'); typeChar(')'); pump();
                check((int)Read(popup, "InsightProviderStackLength") == 1 && ReferenceEquals(Call(popup, "GetInsightProvider"), outer) && area.Focused,
                    "closing a nested call restores the outer signature without changing focus");
                typeChar(')'); pump();
                check(popup.IsDisposed && area.Focused && lostFocus == 0, "closing the outer call disposes the popup and preserves editor input");
                typeChar(';');
                check(((string)Read(document, "TextContent")).EndsWith(");"), "typing continues after the parameter popup closes");
            }

            setSource("Sample(");
            using (var popup = (ToolStripDropDown)Activator.CreateInstance(popupType, owner, editor))
            {
                Call(popup, "AddInsightDataProvider", PreparedProvider(ide, area, document, 6, "procedure Sample(a: integer);"), "sample.pas");
                // Use the actual shell's Closed handler, including its stored reference.
                Type handlerType = ide.GetType("VisualPascalABC.CodeCompletionKeyHandler", true);
                object handler = handlerType.GetConstructors(PrivateInstance)[0].Invoke(new object[] { editor });
                handlerType.GetField("insightWindow").SetValue(handler, popup);
                popup.Closed += (ToolStripDropDownClosedEventHandler)Delegate.CreateDelegate(typeof(ToolStripDropDownClosedEventHandler), handler,
                    handlerType.GetMethod("CloseInsightWindow", PrivateInstance));
                Call(popup, "ShowInsightWindow"); pump();
                Call(area, "ExecuteDialogKey", Keys.Escape); pump();
                check(popup.IsDisposed && handlerType.GetField("insightWindow").GetValue(handler) == null && area.Focused,
                    "Escape closes the hint through the real IDE handler and clears its reference");
                Call(area, "ExecuteDialogKey", Keys.Down);
                check(!area.IsDisposed && area.Focused, "closed parameter hints no longer intercept editor keys");
            }

            setSource("Sample(1");
            using (var popup = (ToolStripDropDown)Activator.CreateInstance(popupType, owner, editor))
            {
                Call(popup, "AddInsightDataProvider", PreparedProvider(ide, area, document, 6, "procedure Sample(a: integer);"), "sample.pas");
                Call(popup, "ShowInsightWindow"); pump();
                Write(caret, "Position", document.GetType().GetMethod("OffsetToPosition").Invoke(document, new object[] { 6 })); pump();
                check(popup.IsDisposed && area.Focused, "moving back across the opening bracket closes the parameter hint");
            }
            using (var popup = (ToolStripDropDown)Activator.CreateInstance(popupType, owner, editor))
            {
                Call(popup, "ShowInsightWindow");
                check(popup.IsDisposed && !popup.IsHandleCreated, "an unknown call creates no popup handle or lingering subscription");
            }
            RealProviderCase(ide, editorAssembly, popupType, owner, editor, area, document, caret, setSource, typeChar, check, pump);
            CompletionPriorityCase(ide, editorAssembly, popupType, owner, editor, area, document, setSource, check, pump);
            setSource("Sample("); area.Focus();
            using (var popup = (ToolStripDropDown)Activator.CreateInstance(popupType, owner, editor))
            using (var other = new TextBox { Dock = DockStyle.Bottom })
            {
                owner.Controls.Add(other); other.BringToFront();
                Call(popup, "AddInsightDataProvider", PreparedProvider(ide, area, document, 6, "procedure Sample(a: integer);"), "sample.pas");
                Call(popup, "ShowInsightWindow"); pump();
                other.Focus(); pump();
                check(popup.IsDisposed && other.Focused && !area.Focused, "leaving the editor closes parameter help without pulling focus back");
            }
        }
        pump();
    }

    private static void RealProviderCase(Assembly ide, Assembly editorAssembly, Type popupType, Form owner, Control editor,
        Control area, object document, object caret, Action<string> setSource, Action<char> typeChar, Action<bool, string> check, Action pump)
    {
        Assembly completion = Assembly.LoadFrom(Path.Combine(Path.GetDirectoryName(ide.Location), "CodeCompletion.dll"));
        Type controller = completion.GetType("CodeCompletion.CodeCompletionController", true);
        string file = Path.Combine(Path.GetDirectoryName(ide.Location), "insight-real-test.pas");
        const string source = "procedure Sample(a: integer; b: string); overload;\nbegin end;\nprocedure Sample(a: string); overload;\nbegin end;\nbegin\n  Sample(1, 'x');\nend.";
        object model = controller.GetMethod("Compile").Invoke(Activator.CreateInstance(controller), new object[] { file, source });
        check((bool)model.GetType().GetField("is_compiled").GetValue(model), "real Pascal analysis prepares overloaded methods for parameter hints");
        var modules = (IDictionary)controller.GetField("comp_modules").GetValue(null);
        modules[file] = model;
        try {
            Write(editor, "FileName", file);
            setSource(source.Substring(0, source.LastIndexOf("Sample(", StringComparison.Ordinal) + "Sample".Length));
            Type providerType = ide.GetType("VisualPascalABC.DefaultInsightDataProvider", true);
            object provider = Activator.CreateInstance(providerType, -1, false, '(');
            using (var popup = (ToolStripDropDown)Activator.CreateInstance(popupType, owner, editor)) {
                Call(popup, "AddInsightDataProvider", provider, file);
                check((int)Read(provider, "InsightDataCount") == 2, "the real bracket provider resolves both overloads");
                Call(popup, "ShowInsightWindow"); typeChar('('); typeChar('1');
                object second = Activator.CreateInstance(providerType, -1, false, ',');
                providerType.GetField("cur_param_num").SetValue(second, providerType.GetField("param_count").GetValue(provider));
                Call(popup, "AddInsightDataProvider", second, file); Call(popup, "ShowInsightWindow"); typeChar(','); pump();
                check((int)providerType.GetField("num_param").GetValue(second) == 2 && popup.Visible && area.Focused,
                    "a comma selects the second parameter through the real Pascal provider");
                string signature = (string)Call(second, "GetInsightData", (int)Call(popup, "GetCurrentData"));
                object[] parts = { signature, 2, (int)providerType.GetField("param_count").GetValue(second), null, null, 0, 0 };
                ide.GetType("ICSharpCode.TextEditor.Util.TipPainterTools", true).GetMethod("GetToolipParts", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, parts);
                string bold = ((string)parts[3]).Substring((int)parts[5], (int)parts[6]);
                check(bold.Contains("b") && bold.Contains("string"), "the existing painter still highlights the current parameter b: string");
                typeChar(')'); pump();
                check(popup.IsDisposed && area.Focused, "the real provider closes the hint at the closing bracket");
            }
        }
        finally { modules.Remove(file); }
    }

    private static void CompletionPriorityCase(Assembly ide, Assembly editorAssembly, Type insightType, Form owner, Control editor,
        Control area, object document, Action<string> setSource, Action<bool, string> check, Action pump)
    {
        setSource("Sample(");
        Type listType = ide.GetType("ICSharpCode.TextEditor.Gui.CompletionWindow.PABCNETCodeCompletionWindow", true);
        object provider = Activator.CreateInstance(ide.GetType("VisualPascalABC.CodeCompletionProvider", true));
        provider.GetType().GetField("preSelection").SetValue(provider, string.Empty);
        Type itemType = ide.GetType("ICSharpCode.TextEditor.Gui.CompletionWindow.UserDefaultCompletionData", true);
        Array items = Array.CreateInstance(editorAssembly.GetType("ICSharpCode.TextEditor.Gui.CompletionWindow.ICompletionData", true), 2);
        items.SetValue(Activator.CreateInstance(itemType, "Alpha", "Alpha documentation", 0, false, false), 0);
        items.SetValue(Activator.CreateInstance(itemType, "Beta", "Beta documentation", 0, false, false), 1);
        var registry = (IDictionary)ide.GetType("VisualPascalABC.CodeCompletionNamesOnlyInModuleAction", true)
            .GetField("comp_windows", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).GetValue(null);
        object previous = registry[area];
        using (var insight = (ToolStripDropDown)Activator.CreateInstance(insightType, owner, editor))
        using (var list = (ToolStripDropDown)listType.GetConstructors(PrivateInstance)[0].Invoke(new object[] { provider, items, owner, editor, false, true }))
        {
            Call(insight, "AddInsightDataProvider", PreparedProvider(ide, area, document, 6,
                "procedure Sample(a: integer);", "procedure Sample(a: string);"), "sample.pas");
            Call(insight, "ShowInsightWindow");
            listType.GetMethod("ShowCompletionWindow", PrivateInstance, null, Type.EmptyTypes, null).Invoke(list, null);
            Call(list, "SelectIndex", 0);
            registry[area] = list;
            list.Closed += (sender, args) => registry.Remove(area);
            try {
                pump();
                check(insight.Visible && list.Visible && area.Focused, "a completion list and parameter hint can coexist without activation");
                Call(area, "ExecuteDialogKey", Keys.Down); pump();
                object view = listType.GetField("codeCompletionListView", PrivateInstance).GetValue(list);
                check((string)Read(Read(view, "SelectedCompletionData"), "Text") == "Beta" && (int)Call(insight, "GetCurrentData") == 0,
                    "Down navigates completion rows without changing the underlying parameter overload");
                Call(area, "ExecuteDialogKey", Keys.Escape); pump();
                check(list.IsDisposed && !insight.IsDisposed && insight.Visible && area.Focused,
                    "first Escape closes only the completion list while leaving parameter help available");
                Call(area, "ExecuteDialogKey", Keys.Down); pump();
                check((int)Call(insight, "GetCurrentData") == 1 && area.Focused, "after list closure the editor arrows switch parameter overloads again");
                Call(area, "ExecuteDialogKey", Keys.Escape); pump();
                check(insight.IsDisposed && area.Focused, "second Escape closes parameter help and preserves editor focus");
            }
            finally { if (previous == null) registry.Remove(area); else registry[area] = previous; }
        }
    }
}
