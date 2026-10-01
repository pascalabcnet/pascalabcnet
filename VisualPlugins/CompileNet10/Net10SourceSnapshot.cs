using System;
using System.Collections.Generic;
using System.IO;

namespace VisualPascalABCPlugins
{
    internal sealed class Net10EditorSource
    {
        public string FileName;
        public string Text;
        public bool Changed;
        public bool FromMetadata;
    }

    internal static class Net10SourceSnapshot
    {
        // Capture synchronously on the UI thread, before awaiting the compiler.
        // These strings belong to this request only; editor documents are untouched.
        public static List<Net10SourceFile> Capture(Net10EditorSource main, IEnumerable<Net10EditorSource> documents)
        {
            if (main.FromMetadata || string.IsNullOrWhiteSpace(main.FileName) || !IsPascal(main.FileName))
                throw new InvalidOperationException("Выберите Pascal-программу с назначенным именем файла.");
            var result = new List<Net10SourceFile>();
            var paths = new HashSet<string>(Path.DirectorySeparatorChar == '\\'
                ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
            Add(main, paths, result);
            foreach (var document in documents)
            {
                if (document.FromMetadata || string.IsNullOrWhiteSpace(document.FileName) || !IsPascal(document.FileName)) continue;
                if (document.Changed || !File.Exists(Path.GetFullPath(document.FileName)))
                    Add(document, paths, result);
            }
            return result;
        }

        private static bool IsPascal(string path)
        {
            return string.Equals(Path.GetExtension(path), ".pas", StringComparison.OrdinalIgnoreCase);
        }

        private static void Add(Net10EditorSource document, HashSet<string> paths, List<Net10SourceFile> result)
        {
            string path = Path.GetFullPath(document.FileName);
            if (paths.Add(path)) result.Add(new Net10SourceFile { fileName = path, text = document.Text ?? "" });
        }
    }
}
