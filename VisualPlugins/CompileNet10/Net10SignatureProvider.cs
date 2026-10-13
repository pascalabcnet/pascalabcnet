using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.InsightWindow;

namespace VisualPascalABCPlugins
{
    internal sealed class Net10SignatureProvider : IExternalInsightDataProvider
    {
        private sealed class Signature
        {
            public string Label, Description;
            public int Active;
            public readonly List<Tuple<int, int>> Ranges = new List<Tuple<int, int>>();
            public bool Variadic;
        }
        private readonly Signature[] signatures;
        private TextArea area;
        internal readonly Net10SignatureContext Context;
        public int CurrentParameter { get; }
        public int DefaultIndex { get; internal set; }
        public int InsightDataCount => signatures.Length;

        internal Net10SignatureProvider(object response, Net10SignatureContext context)
        {
            Context = context;
            var items = new List<Signature>();
            if (response is IDictionary<string, object> help)
            {
                DefaultIndex = Number(help, "activeSignature");
                int active = Number(help, "activeParameter");
                CurrentParameter = active + 1;
                if (help.TryGetValue("signatures", out var values) && values is IEnumerable entries)
                    foreach (var entry in entries)
                    {
                        if (!(entry is IDictionary<string, object> signature) || !signature.TryGetValue("label", out var label) ||
                            !(label is string text) || string.IsNullOrWhiteSpace(text)) continue;
                        string documentation = signature.TryGetValue("documentation", out var doc) ? Documentation(doc) : null;
                        var item = new Signature { Label = text, Description = text + (string.IsNullOrWhiteSpace(documentation) ? "" : "\n" + documentation),
                            Active = signature.ContainsKey("activeParameter") ? Number(signature, "activeParameter") : active };
                        int from = Math.Max(0, text.IndexOf('(') + 1);
                        if (signature.TryGetValue("parameters", out var parameters) && parameters is IEnumerable arguments)
                            foreach (var argument in arguments)
                            {
                                int start = 0, length = 0;
                                if (argument is IDictionary<string, object> parameter && parameter.TryGetValue("label", out var name))
                                {
                                    if (name is string description)
                                    {
                                        int found = text.IndexOf(description, from, StringComparison.Ordinal);
                                        if (found >= 0) { start = found; length = description.Length; from = start + length; }
                                        item.Variadic = description.TrimStart().StartsWith("params ", StringComparison.OrdinalIgnoreCase);
                                    }
                                    else if (name is IEnumerable offsets)
                                    {
                                        object[] range = offsets.Cast<object>().ToArray();
                                        if (range.Length == 2 && range[0] is int begin && range[1] is int end && begin >= 0 && end >= begin && end <= text.Length)
                                        {
                                            start = begin; length = end - begin; from = end;
                                            item.Variadic = text.Substring(begin, length).TrimStart().StartsWith("params ", StringComparison.OrdinalIgnoreCase);
                                        }
                                    }
                                }
                                item.Ranges.Add(Tuple.Create(start, length));
                            }
                        items.Add(item);
                    }
            }
            signatures = items.ToArray();
            DefaultIndex = Math.Max(0, Math.Min(DefaultIndex, signatures.Length - 1));
        }

        private static int Number(IDictionary<string, object> data, string key) =>
            data.TryGetValue(key, out var value) && value is int number ? Math.Max(0, Math.Min(number, 100000)) : 0;
        private static string Documentation(object value) => value is string text ? text :
            value is IDictionary<string, object> markup && markup.TryGetValue("value", out var description) ? description as string : null;
        public void SetupDataProvider(string fileName, TextArea textArea) { area = textArea; }
        public bool CaretOffsetChanged() => area == null || !Context.IsRequestPrefixCurrent(area.Caret.Offset);
        public string GetInsightData(int number) => signatures[number].Description;
        public bool GetParameterHighlight(int signature, out int start, out int length)
        {
            start = length = 0;
            if (signature < 0 || signature >= signatures.Length) return false;
            var item = signatures[signature];
            int parameter = item.Active;
            if (parameter >= item.Ranges.Count && item.Variadic) parameter = item.Ranges.Count - 1;
            if (parameter < 0 || parameter >= item.Ranges.Count) return false;
            var range = item.Ranges[parameter]; start = range.Item1; length = range.Item2;
            return length > 0;
        }
    }
}
