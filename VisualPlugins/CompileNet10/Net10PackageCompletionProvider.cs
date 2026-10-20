using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using VisualPascalABC;

namespace VisualPascalABCPlugins
{
    internal sealed class Net10PackageCompletionProvider : ICompletionDataProvider
    {
        private sealed class Item : DefaultCompletionData
        {
            private readonly string insertion;
            internal Item(string label, string insertion, string description, int icon)
                : base(label, description, icon) { this.insertion = insertion; }
            public override bool InsertAction(TextArea area, char ch)
            {
                area.InsertString(insertion);
                return false;
            }
        }

        internal readonly ICompletionData[] Items;
        public ImageList ImageList { get; }
        public string PreSelection { get; }
        public int DefaultIndex => 0;

        internal Net10PackageCompletionProvider(object response, CodeCompletionImagesProvider images,
            Net10PackageDirectiveContext context)
        {
            ImageList = images.ImageList;
            PreSelection = context.Prefix.Length == 0 ? null : context.Prefix;
            var values = response as IEnumerable;
            var result = new List<ICompletionData>();
            foreach (var value in values ?? new object[0])
            {
                if (context.IsVersion)
                {
                    string version = value as string;
                    if (string.IsNullOrWhiteSpace(version)) continue;
                    result.Add(new Item(version, version + (context.HasClosingBrace ? "" : "}"),
                        null, images.IconNumberConstant));
                }
                else if (value is Dictionary<string, object> package)
                {
                    string id = Text(package, "Id");
                    string version = Text(package, "Version");
                    if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(version)) continue;
                    string description = Text(package, "Description") ?? "";
                    if (description.Length > 500) description = description.Substring(0, 500) + "…";
                    string source = Text(package, "Source");
                    if (!string.IsNullOrWhiteSpace(source))
                        description += (description.Length > 0 ? "\n" : "") + source;
                    result.Add(new Item(id + "    " + version,
                        id + " " + version + (context.HasClosingBrace ? "" : "}"),
                        description, images.IconNumberClass));
                }
            }
            Items = result.ToArray();
        }

        private static string Text(Dictionary<string, object> item, string key)
        {
            foreach (var field in item)
                if (string.Equals(field.Key, key, StringComparison.OrdinalIgnoreCase))
                    return field.Value as string;
            return null;
        }

        public CompletionDataProviderKeyResult ProcessKey(char key) =>
            char.IsLetterOrDigit(key) || key == '_' || key == '.' || key == '-' || key == '+'
                ? CompletionDataProviderKeyResult.NormalKey : CompletionDataProviderKeyResult.InsertionKey;

        public bool InsertAction(ICompletionData data, TextArea area, int offset, char key)
        {
            area.Caret.Position = area.Document.OffsetToPosition(offset);
            return data.InsertAction(area, key);
        }

        public ICompletionData[] GenerateCompletionData(string filename, TextArea area, char typed) => Items;
    }
}
