using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace VisualPascalABCPlugins
{
    // Data adapter only. The host owns the popup, keyboard navigation and prefix replacement.
    internal sealed class Net10CompletionProvider : ICompletionDataProvider
    {
        private sealed class Item : DefaultCompletionData
        {
            private readonly string insertion;
            public Item(string label, string insertion, string description, int image)
                : base(label, description, image) { this.insertion = insertion; }
            public override bool InsertAction(TextArea area, char ch) { area.InsertString(insertion); return false; }
        }

        internal readonly ICompletionData[] Items;
        public ImageList ImageList { get; }
        public string PreSelection { get; }
        public int DefaultIndex => 0;

        public Net10CompletionProvider(object response, IExternalIntelliSenseSession ui, string prefix)
        {
            ImageList = ui.CompletionImages;
            PreSelection = string.IsNullOrEmpty(prefix) ? null : prefix;
            var list = response as IEnumerable;
            if (response is Dictionary<string, object> completion && completion.TryGetValue("items", out var items))
                list = items as IEnumerable;
            var result = new List<ICompletionData>();
            foreach (var value in list ?? new object[0])
            {
                if (!(value is Dictionary<string, object> item)) continue;
                string label = Text(item, "label");
                if (string.IsNullOrEmpty(label)) continue;
                // Current server supplies plain insertText, not snippets or cross-document edits.
                if (item.ContainsKey("textEdit") || item.ContainsKey("additionalTextEdits") ||
                    (item.TryGetValue("insertTextFormat", out var format) && Convert.ToInt32(format) == 2)) continue;
                string insertion = Text(item, "insertText") ?? label;
                string description = Text(item, "detail") ?? "";
                string documentation = Text(item, "documentation");
                if (!string.IsNullOrEmpty(documentation)) description += (description.Length > 0 ? "\n" : "") + documentation;
                int kind = item.TryGetValue("kind", out var kindValue) ? Convert.ToInt32(kindValue) : 1;
                bool extension = item.TryGetValue("data", out var data) && data is Dictionary<string, object> metadata &&
                    metadata.TryGetValue("isExtensionMethod", out var flag) && flag is bool isExtension && isExtension;
                int image = extension && (kind == 2 || kind == 3 || kind == 4)
                    ? ui.GetImageIndex(CompletionItemKind.ExtensionMethod) : Icon(ui, kind);
                result.Add(new Item(label, insertion, description, image));
            }
            Items = result.ToArray();
        }

        private static string Text(Dictionary<string, object> item, string key)
        {
            if (!item.TryGetValue(key, out var value)) return null;
            if (value is Dictionary<string, object> markup) return Text(markup, "value");
            return value as string;
        }

        private static int Icon(IExternalIntelliSenseSession ui, int kind)
        {
            switch (kind)
            {
                case 2: case 3: case 4: return ui.GetImageIndex(CompletionItemKind.Method);
                case 5: return ui.GetImageIndex(CompletionItemKind.Field);
                case 6: return ui.GetImageIndex(CompletionItemKind.Local);
                case 7: return ui.GetImageIndex(CompletionItemKind.Class);
                case 8: return ui.GetImageIndex(CompletionItemKind.Interface);
                case 9: return ui.GetImageIndex(CompletionItemKind.Namespace);
                case 10: return ui.GetImageIndex(CompletionItemKind.Property);
                case 13: return ui.GetImageIndex(CompletionItemKind.Enum);
                case 14: return ui.GetImageIndex(CompletionItemKind.Keyword);
                case 20: case 21: return ui.GetImageIndex(CompletionItemKind.Constant);
                case 22: return ui.GetImageIndex(CompletionItemKind.Struct);
                case 23: return ui.GetImageIndex(CompletionItemKind.Event);
                default: return ui.GetImageIndex(CompletionItemKind.Class);
            }
        }

        public CompletionDataProviderKeyResult ProcessKey(char key) => char.IsLetterOrDigit(key) || key == '_'
            ? CompletionDataProviderKeyResult.NormalKey : CompletionDataProviderKeyResult.InsertionKey;
        public bool InsertAction(ICompletionData data, TextArea area, int offset, char key)
        {
            area.Caret.Position = area.Document.OffsetToPosition(offset);
            return data.InsertAction(area, key);
        }
        public ICompletionData[] GenerateCompletionData(string filename, TextArea area, char typed) => Items;
    }
}
