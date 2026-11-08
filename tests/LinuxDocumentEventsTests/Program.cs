using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using Mono.Cecil;
using Mono.Cecil.Cil;
using VisualPascalABCPlugins;

internal static class Program
{
    private const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
    private static void Check(bool value, string message)
    { if (!value) throw new Exception(message); Console.WriteLine("PASS " + message); }
    private static int CallIndex(MethodDefinition method, string name) =>
        method.Body.Instructions.Select((item, index) => new { item, index })
            .Where(pair => pair.item.Operand is MethodReference reference && reference.Name == name)
            .Select(pair => pair.index).DefaultIfEmpty(-1).First();
    private static void CheckNotificationSites(string path)
    {
        var assembly = AssemblyDefinition.ReadAssembly(path);
        var form = assembly.MainModule.Types.Single(type => type.FullName == "VisualPascalABC.Form1");
        foreach (string name in new[] { "OpenFile", "OpenFileAndNoSelection", "OpenFileForDebugInvoke" })
        {
            var method = form.Methods.Single(item => item.Name == name);
            Check(CallIndex(method, "NotifyDocumentOpened") > CallIndex(method, "LoadFromFile"),
                name + " notifies after loading the source, not while the editor is empty");
        }
        var open = form.Methods.Single(item => item.Name == "OpenFile");
        Check(CallIndex(open, "NotifyDocumentRenamed") > CallIndex(open, "LoadFromFile"), "reused blank editor is renamed after successful load");
        var save = form.Methods.Single(item => item.Name == "SaveFileAs");
        int saved = CallIndex(save, "SaveFile"), renamed = CallIndex(save, "NotifyDocumentRenamed");
        Check(saved >= 0 && renamed > saved && renamed > CallIndex(save, "set_FileName"), "Save As notifies after writing the file and assigning its final name");
        Check(save.Body.ExceptionHandlers.Any(handler => handler.HandlerType == ExceptionHandlerType.Catch &&
            save.Body.Instructions.IndexOf(handler.TryStart) <= saved && renamed < save.Body.Instructions.IndexOf(handler.TryEnd)),
            "a failed save exits through catch before the rename notification");
        var close = form.Methods.Single(item => item.Name == "CloseFile" && item.Parameters.Count == 2);
        int question = CallIndex(close, "QuestionAndSaveFile"), remove = CallIndex(close, "Remove"), notify = CallIndex(close, "NotifyDocumentClosed"), dispose = CallIndex(close, "Dispose");
        var code = close.Body.Instructions;
        Check(question >= 0 && question < remove && remove < notify && notify < dispose, "Close removes the document before notification, and disposes it afterwards");
        var accepted = code[question + 1];
        Check((accepted.OpCode == OpCodes.Brtrue || accepted.OpCode == OpCodes.Brtrue_S) &&
            code.Skip(question + 2).Take(code.IndexOf((Instruction)accepted.Operand) - question - 2).Any(item => item.OpCode == OpCodes.Ret),
            "cancelled close returns before removal, notification and disposal");
        Check(CallIndex(form.Methods.Single(item => item.Name == "OpenTabWithText"), "NotifyDocumentOpened") >= 0,
            "generated metadata tabs also report opening");
    }
    [STAThread]
    private static int Main(string[] args)
    {
        string path = args.Length > 0 ? Path.GetFullPath(args[0]) : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PascalABCNETLinux.exe");
        string temp = Path.Combine(Path.GetTempPath(), "LinuxDocumentEvents-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(temp);
        TextEditorControl editor = null;
        try
        {
            CheckNotificationSites(path);
            var assembly = Assembly.LoadFrom(path);
            var type = assembly.GetType("VisualPascalABC.Form1", true);
            var host = FormatterServices.GetUninitializedObject(type); GC.SuppressFinalize(host);
            var registryField = type.GetField("OpenDocuments", Private);
            var registry = (IDictionary)Activator.CreateInstance(registryField.FieldType); registryField.SetValue(host, registry);
            var documentType = assembly.GetType("VisualPascalABC.CodeFileDocumentControl", true);
            var document = (ICodeFileDocument)FormatterServices.GetUninitializedObject(documentType); GC.SuppressFinalize(document);
            editor = (TextEditorControl)Activator.CreateInstance(assembly.GetType("VisualPascalABC.CodeFileDocumentTextEditorControl", true));
            // This fixture never loads the shell/navigation panel or starts its
            // analysis thread. Keep that unloaded legacy panel out of editor
            // disposal (its production Dispose assumes OnLoad started the thread).
            var browser = (Control)editor.GetType().GetField("quickClassBrowserPanel", Private).GetValue(editor);
            browser.Parent.Controls.Remove(browser);
            GC.SuppressFinalize(browser);
            documentType.GetField("TextEditor", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).SetValue(document, editor);
            var name = documentType.GetProperty("FileName");
            string first = Path.Combine(temp, "First.pas"), second = Path.Combine(temp, "SavedAs.pas");
            File.WriteAllText(first, "begin Print(42) end.");
            editor.LoadFile(first); name.SetValue(document, first);
            registry.Add(first, document);
            var events = (IWorkbenchDocumentEvents)host;
            int opened = 0, closed = 0, renamed = 0;
            events.DocumentOpened += (sender, e) => { throw new InvalidOperationException("test plugin failure"); };
            events.DocumentOpened += (sender, e) => { opened++; Check(ReferenceEquals(e.Document, document) && e.FileName == first && e.Document.TextEditor.Document.TextContent.Contains("42"), "open event has the loaded text and final name"); };
            events.DocumentRenamed += (sender, e) => { renamed++; Check(e.PreviousFileName == first && e.FileName == second && File.ReadAllText(second).Contains("43"), "Save As event captures both names after saved contents are available"); };
            events.DocumentClosed += (sender, e) => { closed++; Check(events.GetOpenDocuments().Length == 0 && !editor.IsDisposed, "close notification exposes live editor after registry removal"); };
            Action<string, object[]> notifyCall = (method, values) => type.GetMethod(method, Private).Invoke(host, values);
            notifyCall("NotifyDocumentOpened", new object[] { document });
            Check(opened == 1 && events.GetOpenDocuments().Single() == document, "subscriber failure cannot break another subscriber or the open registry");
            var snapshot = events.GetOpenDocuments(); snapshot[0] = null;
            Check(events.GetOpenDocuments()[0] == document, "snapshot cannot mutate the host registry");
            editor.Document.TextContent = "begin Print(43) end.";
            editor.SaveFile(second); registry.Remove(first); registry.Add(second, document); name.SetValue(document, second);
            notifyCall("NotifyDocumentRenamed", new object[] { document, first });
            notifyCall("NotifyDocumentRenamed", new object[] { document, second });
            Check(renamed == 1, "ordinary save emits no rename");
            Check(closed == 0 && events.GetOpenDocuments().Length == 1 && editor.Document.TextContent.Contains("43"), "retaining a tab leaves its unsaved text and subscriptions intact");
            registry.Remove(second); notifyCall("NotifyDocumentClosed", new object[] { document });
            Check(closed == 1, "successful close emits one event");
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
        finally { editor?.Dispose(); Directory.Delete(temp, true); }
    }
}
