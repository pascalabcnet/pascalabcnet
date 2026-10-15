using System;
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace VisualPascalABCPlugins
{
    internal static class Net10OutputCleanup
    {
        // null means "next to the source"; a configured root is captured before compilation.
        public static string GetDirectory(string sourceFile, string configuredOutputDirectory = null)
        {
            if (!Path.IsPathRooted(sourceFile)) throw new ArgumentException("Source path must be absolute.", nameof(sourceFile));
            if (configuredOutputDirectory != null && !Path.IsPathRooted(configuredOutputDirectory))
                throw new ArgumentException("Output path must be absolute.", nameof(configuredOutputDirectory));
            return Path.GetFullPath(Path.Combine(configuredOutputDirectory ??
                Path.GetDirectoryName(Path.GetFullPath(sourceFile)), "net10-output"));
        }

        // Called only after the launched process has exited and its streams have drained.
        // Never accept an arbitrary directory from a compiler response as a deletion target.
        public static bool TryDelete(string sourceFile, string outputFile, bool deleteExe, bool deletePdb,
            string configuredOutputDirectory = null)
        {
            if (!deleteExe || !deletePdb || string.IsNullOrWhiteSpace(sourceFile) ||
                string.IsNullOrWhiteSpace(outputFile)) return false;
            try
            {
                if (!Path.IsPathRooted(sourceFile) || !Path.IsPathRooted(outputFile)) return false;
                string source = Path.GetFullPath(sourceFile);
                string output = Path.GetFullPath(outputFile);
                string directory = GetDirectory(source, configuredOutputDirectory);
                // A user may open a source from inside the selected output directory itself.
                if (source.StartsWith(directory + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)) return false;
                if (!string.Equals(Path.GetDirectoryName(output), directory, StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(Path.GetExtension(output), ".exe", StringComparison.OrdinalIgnoreCase)) return false;
                if (!Directory.Exists(directory)) return false;

                // Reject junctions/symlinks in the target, its ancestors and its contents.
                // They could otherwise redirect recursive deletion outside the generated directory.
                for (var ancestor = new DirectoryInfo(directory); ancestor != null; ancestor = ancestor.Parent)
                    if ((ancestor.Attributes & FileAttributes.ReparsePoint) != 0) return false;
                var pending = new Stack<string>();
                pending.Push(directory);
                while (pending.Count != 0)
                    foreach (string entry in Directory.EnumerateFileSystemEntries(pending.Pop()))
                    {
                        var attributes = File.GetAttributes(entry);
                        if ((attributes & FileAttributes.ReparsePoint) != 0) return false;
                        if ((attributes & FileAttributes.Directory) != 0) pending.Push(entry);
                    }

                Directory.Delete(directory, true);
                return true;
            }
            // Cleanup is best-effort: a locked file must not turn a completed run into an error.
            catch (IOException) { return false; }
            catch (UnauthorizedAccessException) { return false; }
            catch (SecurityException) { return false; }
            catch (ArgumentException) { return false; }
            catch (NotSupportedException) { return false; }
        }
    }
}
