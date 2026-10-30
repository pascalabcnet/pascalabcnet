using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

// Exercise the actual Linux IDE popup, provider, editor and Closed handler from
// a built distribution without starting the IDE/compiler or loading its settings.
internal static class CompletionInsertionTests
{
    private static object Read(object target, string name)
    {
        return target.GetType().GetProperty(name).GetValue(target, null);
    }

    private static void Write(object target, string name, object value)
    {
        target.GetType().GetProperty(name).SetValue(target, value, null);
    }

    internal static void Run(string idePath, Action<bool, string> check, Action pump, bool listOnly = false)
    {
        string directory = Path.GetDirectoryName(Path.GetFullPath(idePath));
        ResolveEventHandler resolve = (sender, args) => {
            string file = Path.Combine(directory, new AssemblyName(args.Name).Name + ".dll");
            return File.Exists(file) ? Assembly.LoadFrom(file) : null;
        };
        AppDomain.CurrentDomain.AssemblyResolve += resolve;
        try
        {
            Assembly ide = Assembly.LoadFrom(idePath);
            Assembly editorAssembly = Assembly.LoadFrom(Path.Combine(directory, "ICSharpCode.TextEditorLinux.dll"));
            if (Environment.GetEnvironmentVariable("PABC_TEST_MONO_EDITOR_BRANCH") == "1")
            {
                // Test process only: exercise the common editor's Mono paths on
                // Windows before the real ALT check. No production override exists.
                editorAssembly.GetType("ICSharpCode.TextEditor.EditorPlatform", true)
                    .GetField("IsMonoUnix", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, true);
            }
            Type mainType = ide.GetType("VisualPascalABC.Form1", true);
            // Only the options field is read by the popup. Do not run Form1's
            // constructor: it starts IDE services and loads real user settings.
            object optionsHost = FormatterServices.GetUninitializedObject(mainType);
            GC.SuppressFinalize(optionsHost);
            mainType.GetField("UserOptions").SetValue(optionsHost,
                Activator.CreateInstance(ide.GetType("VisualPascalABC.UserOptions", true)));
            FieldInfo singleton = ide.GetType("VisualPascalABC.VisualPABCSingleton", true)
                .GetField("mainForm", BindingFlags.Static | BindingFlags.NonPublic);
            object previous = singleton.GetValue(null);
            singleton.SetValue(null, optionsHost);
            try
            {
                if (listOnly) { CompletionListInteractionTests.Run(ide, editorAssembly, check, pump); return; }
                WindowTraceTests.Run(ide, check);
                RunCase(ide, editorAssembly, "uses School", false, null, check, pump);
                RunCase(ide, editorAssembly, "uses School", true, null, check, pump);
                RunCase(ide, editorAssembly, "uses Sch", true, null, check, pump);
                RunCase(ide, editorAssembly, "uses Sch", true, Keys.Enter, check, pump);
                RunCase(ide, editorAssembly, "uses Sch", true, Keys.Tab, check, pump);
                RunCase(ide, editorAssembly, "uses Sch", true, null, check, pump, true);
                RunCase(ide, editorAssembly, "uses Sch", true, Keys.Enter, check, pump, true);
                RunCase(ide, editorAssembly, "uses Sch", true, Keys.Tab, check, pump, true);
                DocumentationSnapshotTests.Run(directory, check);
                DocumentationPublicationTests.Run(ide, editorAssembly, check, pump);
                DescriptionWindowTests.Run(ide, editorAssembly, check, pump);
                HoverEventTests.Run(ide, editorAssembly, check, pump);
                EditorToolTipTests.Run(ide, editorAssembly, check, pump);
                InsightWindowTests.Run(ide, editorAssembly, check, pump);
                CompletionListInteractionTests.Run(ide, editorAssembly, check, pump);
            }
            finally { singleton.SetValue(null, previous); }
        }
        finally { AppDomain.CurrentDomain.AssemblyResolve -= resolve; }
    }

    private static void RunCase(Assembly ide, Assembly editorAssembly, string source,
        bool showCompletion, Keys? dialogKey, Action<bool, string> check, Action pump, bool blockDocumentation = false)
    {
        using (var owner = new Form { Text = "Linux IDE semicolon regression test", Size = new Size(700, 350), ShowInTaskbar = false })
        using (var editor = (Control)Activator.CreateInstance(editorAssembly.GetType("ICSharpCode.TextEditor.TextEditorControl", true)))
        {
            editor.Dock = DockStyle.Fill;
            owner.Controls.Add(editor);
            object document = Read(editor, "Document");
            Write(document, "TextContent", source);
            object activeEditor = Read(editor, "ActiveTextAreaControl");
            var textArea = (Control)Read(activeEditor, "TextArea");
            object caret = Read(activeEditor, "Caret");
            owner.Show(); owner.Activate(); textArea.Focus();
            Write(caret, "Position", document.GetType().GetMethod("OffsetToPosition").Invoke(document, new object[] { source.Length }));
            pump();
            int lostFocus = 0;
            textArea.LostFocus += (sender, args) => lostFocus++;
            ToolStripDropDown popup = null;
            dynamic description = null;
            Delegate keyHandler = null;
            object documentation = null;
            using (var readStarted = new ManualResetEvent(false))
            using (var readRelease = new ManualResetEvent(false))
            using (var readFinished = new ManualResetEvent(false))
            {
            int published = 0;
            EventInfo keyEvent = textArea.GetType().GetEvent("KeyEventHandler");
            try
            {
                if (showCompletion)
                {
                    Type providerType = ide.GetType("VisualPascalABC.CodeCompletionProvider", true);
                    object provider = Activator.CreateInstance(providerType);
                    if (blockDocumentation)
                    {
                        Type documentationType = ide.GetType("ICSharpCode.TextEditor.Gui.CompletionWindow.CompletionDocumentation", true);
                        documentation = documentationType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0]
                            .Invoke(new object[] { textArea });
                        providerType.GetField("documentation", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(provider, documentation);
                        documentationType.GetMethod("Start", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(documentation, new object[] {
                            new Action(() => { readStarted.Set(); try { readRelease.WaitOne(5000); } finally { readFinished.Set(); } }),
                            new Action(() => published++) });
                        if (!readStarted.WaitOne(3000)) throw new Exception("Actual IDE documentation reader did not start.");
                    }
                    providerType.GetField("preSelection").SetValue(provider, source.Substring(5));
                    Type dataType = ide.GetType("ICSharpCode.TextEditor.Gui.CompletionWindow.UserDefaultCompletionData", true);
                    object item = Activator.CreateInstance(dataType,
                        "School", "School unit documentation", 0, false, false);
                    Array items = Array.CreateInstance(editorAssembly.GetType("ICSharpCode.TextEditor.Gui.CompletionWindow.ICompletionData", true), 1);
                    items.SetValue(item, 0);
                    Type popupType = ide.GetType("ICSharpCode.TextEditor.Gui.CompletionWindow.PABCNETCodeCompletionWindow", true);
                    popup = (ToolStripDropDown)popupType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0]
                        .Invoke(new object[] { provider, items, owner, editor, false, false });
                    description = popupType.GetField("declarationViewWindow", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(popup);
                    check(!description.Visible, "list description waits until the completion list has been shown");
                    popupType.GetMethod("ShowCompletionWindow", BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null).Invoke(popup, null);
                    popupType.GetMethod("SelectIndex").Invoke(popup, new object[] { 0 });

                    // The same forwarding used by CodeCompletionKeyHandler for an
                    // already-open popup, plus its real documentation cleanup on Closed.
                    keyHandler = Delegate.CreateDelegate(keyEvent.EventHandlerType, popup, popupType.GetMethod("ProcessKeyEvent"));
                    keyEvent.AddEventHandler(textArea, keyHandler);
                    Type handlerType = ide.GetType("VisualPascalABC.CodeCompletionKeyHandler", true);
                    object handler = handlerType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0].Invoke(new object[] { editor });
                    handlerType.GetField("codeCompletionWindow").SetValue(handler, popup);
                    popup.Closed += (ToolStripDropDownClosedEventHandler)Delegate.CreateDelegate(typeof(ToolStripDropDownClosedEventHandler), handler,
                        handlerType.GetMethod("CloseCodeCompletionWindow", BindingFlags.Instance | BindingFlags.NonPublic));
                    pump();
                    check(popup.Visible && description.Visible, "actual Linux completion list and documentation are visible");
                    check(textArea.Focused && lostFocus == 0 && !popup.ContainsFocus && !popup.Capture,
                        "completion list appears without even a transient loss of editor focus");
                }
                var closeWatch = Stopwatch.StartNew();
                if (dialogKey.HasValue)
                {
                    textArea.GetType().GetMethod("ExecuteDialogKey").Invoke(textArea, new object[] { dialogKey.Value });
                }
                else
                {
                    textArea.GetType().GetMethod("SimulateKeyPress").Invoke(textArea, new object[] { ';' });
                }
                closeWatch.Stop();
                if (keyHandler != null) { keyEvent.RemoveEventHandler(textArea, keyHandler); keyHandler = null; }
                pump();
                string expected = "uses School" + (dialogKey.HasValue ? "" : ";");
                check((string)Read(document, "TextContent") == expected && textArea.Focused &&
                    (popup == null || popup.IsDisposed) && (description == null || description.IsDisposed),
                    "actual Linux " + (dialogKey.HasValue ? dialogKey.Value.ToString() : ";") +
                    " after '" + source + "' closes both popups and restores editor focus");
                textArea.GetType().GetMethod("SimulateKeyPress").Invoke(textArea, new object[] { 'x' });
                pump();
                check((string)Read(document, "TextContent") == expected + "x" && textArea.Focused,
                    "editor accepts the next character after completion");
                if (blockDocumentation)
                    check(closeWatch.ElapsedMilliseconds < 250 && published == 0 && !readFinished.WaitOne(0) &&
                        (bool)documentation.GetType().GetProperty("IsCancelled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(documentation, null),
                        "actual IDE " + (dialogKey.HasValue ? dialogKey.Value.ToString() : ";") +
                        " cancels pending documentation without waiting for the blocked reader");
            }
            finally
            {
                readRelease.Set();
                if (documentation != null && !readFinished.WaitOne(3000)) throw new Exception("Actual IDE test reader did not stop.");
                if (keyHandler != null) keyEvent.RemoveEventHandler(textArea, keyHandler);
                popup?.Dispose(); description?.Dispose();
            }
            }
        }
        pump();
    }
}
