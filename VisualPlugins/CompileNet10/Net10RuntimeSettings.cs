using System;
using System.IO;

namespace VisualPascalABCPlugins
{
    internal sealed class Net10RuntimeSettings
    {
        public const string DefaultRuntimeDirectory = "CompilerHost\\net10";
        public string RuntimeDirectory { get; private set; }
        public string DotnetPath { get; private set; }

        public static Net10RuntimeSettings Load(string ideDirectory)
        {
            // Resolve against the executable, never VS's/current working directory.
            string baseDirectory = Path.GetFullPath(ideDirectory);
            string runtimeDirectory = DefaultRuntimeDirectory;
            string dotnetPath = "dotnet";
            string ini = Path.Combine(baseDirectory, "CompileNet10Plugin.ini");
            if (File.Exists(ini))
                foreach (string line in File.ReadAllLines(ini))
                {
                    int separator = line.IndexOf('=');
                    if (separator < 0 || line.TrimStart().StartsWith("#")) continue;
                    string key = line.Substring(0, separator).Trim();
                    string value = line.Substring(separator + 1).Trim();
                    if (value.Length == 0) continue;
                    if (key == "RuntimeDirectory") runtimeDirectory = value;
                    if (key == "DotnetPath") dotnetPath = value;
                }
            return new Net10RuntimeSettings
            {
                RuntimeDirectory = Path.GetFullPath(Path.IsPathRooted(runtimeDirectory)
                    ? runtimeDirectory : Path.Combine(baseDirectory, runtimeDirectory)),
                DotnetPath = dotnetPath
            };
        }
    }
}
