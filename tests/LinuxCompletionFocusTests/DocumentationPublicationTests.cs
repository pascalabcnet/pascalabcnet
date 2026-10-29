using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Windows.Forms;

internal static class DocumentationPublicationTests
{
    internal static void Run(Assembly ide, Assembly editorAssembly, Action<bool, string> check, Action pump)
    {
        // Supply an in-memory XML document through the real documentation cache.
        // No dependence on installed Microsoft XML documentation or user files.
        Assembly parserTools = Assembly.LoadFrom(Path.Combine(Path.GetDirectoryName(ide.Location), "ParserTools.dll"));
        Type xmlType = parserTools.GetType("CodeCompletionTools.XmlDoc", true);
        using (var reader = XmlReader.Create(new StringReader("<doc><members><member name=\"T:System.String\"><summary>Assembly documentation.</summary></member></members></doc>")))
        using (var xml = (IDisposable)xmlType.GetMethod("Load", new[] { typeof(XmlReader) }).Invoke(null, new object[] { reader }))
        using (var owner = new Form { Text = "Linux documentation publication test", Size = new Size(500, 220), ShowInTaskbar = false })
        using (var editor = (Control)Activator.CreateInstance(editorAssembly.GetType("ICSharpCode.TextEditor.TextEditorControl", true)))
        {
            var xmlCache = (IDictionary)parserTools.GetType("CodeCompletionTools.AssemblyDocCache", true)
                .GetField("ht", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            Assembly system = typeof(string).Assembly;
            bool existed = xmlCache.Contains(system);
            object previous = xmlCache[system];
            xmlCache[system] = xml;
            object provider = null;
            try
            {
                editor.Dock = DockStyle.Fill; owner.Controls.Add(editor); owner.Show(); pump();
                object textArea = editor.GetType().GetProperty("ActiveTextAreaControl").GetValue(editor, null);
                textArea = textArea.GetType().GetProperty("TextArea").GetValue(textArea, null);
                Type providerType = ide.GetType("VisualPascalABC.CodeCompletionProvider", true);
                provider = Activator.CreateInstance(providerType);
                Type symbolType = parserTools.GetType("PascalABCCompiler.Parsers.SymInfo", true);
                Type symbolKind = symbolType.GetConstructors()[0].GetParameters()[1].ParameterType;
                object symbol = Activator.CreateInstance(symbolType, "String", Activator.CreateInstance(symbolKind), "type String");
                object item = Activator.CreateInstance(ide.GetType("ICSharpCode.TextEditor.Gui.CompletionWindow.UserDefaultCompletionData", true),
                    "String", "type String", 0, false, false);
                object dispatcher = providerType.GetField("documentationDispatcher", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(provider);
                dispatcher.GetType().GetMethod("Add").Invoke(dispatcher, new[] { symbol, item });
                Assembly completion = Assembly.LoadFrom(Path.Combine(Path.GetDirectoryName(ide.Location), "CodeCompletion.dll"));
                completion.GetType("CodeCompletion.AssemblyDocCache", true).GetMethod("AddDescribeToComplete", new[] { symbolType, typeof(Type) })
                    .Invoke(null, new object[] { symbol, typeof(string) });
                providerType.GetMethod("StartDocumentation", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(provider, new[] { textArea, (object)true });
                var watch = Stopwatch.StartNew();
                Func<string> description = () => (string)item.GetType().GetProperty("Description").GetValue(item, null);
                while (!description().Contains("Assembly documentation.") && watch.ElapsedMilliseconds < 5000) pump();
                check(description() == "type String\nAssembly documentation." && (bool)symbolType.GetField("has_doc").GetValue(symbol),
                    "actual Linux provider still attaches XML documentation to the matching completion item");
            }
            finally
            {
                provider?.GetType().GetMethod("CancelDocumentation", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(provider, null);
                if (existed) xmlCache[system] = previous; else xmlCache.Remove(system);
            }
        }
        pump();
    }
}
