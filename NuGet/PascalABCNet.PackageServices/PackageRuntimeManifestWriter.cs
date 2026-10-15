using System.Text.Json;
using System.Text.Json.Nodes;

namespace PascalABCNet.PackageServices;

/// <summary>
/// Describes restored runtime assets to the .NET host without copying NuGet DLLs
/// beside a compiled Pascal program. Publishing a portable layout is separate.
/// </summary>
public static class PackageRuntimeManifestWriter
{
    private const string Target = ".NETCoreApp,Version=v10.0";
    private const string Marker = "pascalABCNET";

    public static void WriteForExecutable(ResolvedPackageGraph graph, string executablePath)
    {
        ArgumentNullException.ThrowIfNull(graph);
        ArgumentException.ThrowIfNullOrWhiteSpace(executablePath);
        if (!string.Equals(graph.TargetFramework, "net10.0", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Only net10.0 runtime manifests are supported.", nameof(graph));

        string executable = Path.GetFullPath(executablePath);
        string runtimeConfigPath = Path.ChangeExtension(executable, ".runtimeconfig.json");
        string depsPath = Path.ChangeExtension(executable, ".deps.json");
        if (!File.Exists(executable) || !File.Exists(runtimeConfigPath))
            throw new FileNotFoundException("The compiled EXE and its runtimeconfig.json must exist before writing NuGet runtime assets.");

        var versions = graph.Packages.ToDictionary(package => package.Id, package => package.Version,
            StringComparer.OrdinalIgnoreCase);
        var targets = new JsonObject();
        var libraries = new JsonObject();
        foreach (var package in graph.Packages.OrderBy(package => package.Id, StringComparer.OrdinalIgnoreCase))
        {
            string key = package.Id + "/" + package.Version;
            var runtime = new JsonObject();
            foreach (string asset in package.RuntimeAssemblies)
            {
                string relative = Path.GetRelativePath(package.Directory, asset);
                if (Path.IsPathRooted(relative) || relative == ".." ||
                    relative.StartsWith(".." + Path.DirectorySeparatorChar, StringComparison.Ordinal))
                    throw new InvalidDataException("NuGet runtime asset escapes its package: " + asset);
                runtime[relative.Replace('\\', '/')] = new JsonObject();
            }

            var targetLibrary = new JsonObject { ["runtime"] = runtime };
            if (package.Dependencies.Count != 0)
            {
                var dependencies = new JsonObject();
                foreach (var dependency in package.Dependencies)
                {
                    if (!versions.TryGetValue(dependency.Id, out string? version))
                        throw new InvalidDataException("NuGet runtime dependency is absent: " + dependency.Id);
                    dependencies[dependency.Id] = version;
                }
                targetLibrary["dependencies"] = dependencies;
            }
            targets[key] = targetLibrary;

            string normalizedId = package.Id.ToLowerInvariant();
            string normalizedVersion = package.Version.ToLowerInvariant();
            string hashPath = normalizedId + "." + normalizedVersion + ".nupkg.sha512";
            string hashFile = Path.Combine(package.Directory, hashPath);
            string hash = File.Exists(hashFile) ? "sha512-" + File.ReadAllText(hashFile).Trim() : "";
            libraries[key] = new JsonObject
            {
                ["type"] = "package",
                ["serviceable"] = true,
                ["sha512"] = hash,
                ["path"] = normalizedId + "/" + normalizedVersion,
                ["hashPath"] = hashPath
            };
        }

        var root = new JsonObject
        {
            ["runtimeTarget"] = new JsonObject { ["name"] = Target, ["signature"] = "" },
            ["compilationOptions"] = new JsonObject(),
            ["targets"] = new JsonObject { [Target] = targets },
            ["libraries"] = libraries,
            [Marker] = new JsonObject { ["generatedNuGetManifest"] = true }
        };

        var config = JsonNode.Parse(File.ReadAllText(runtimeConfigPath)) as JsonObject
            ?? throw new InvalidDataException("Invalid runtimeconfig.json: " + runtimeConfigPath);
        var options = config["runtimeOptions"] as JsonObject
            ?? throw new InvalidDataException("runtimeOptions is missing: " + runtimeConfigPath);
        var probing = options["additionalProbingPaths"] as JsonArray;
        if (probing == null)
        {
            probing = new JsonArray();
            options["additionalProbingPaths"] = probing;
        }
        string packagesDirectory = Path.GetFullPath(graph.Configuration.PackagesDirectory);
        if (!probing.Any(path => string.Equals((string?)path, packagesDirectory,
                OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)))
            probing.Add(packagesDirectory);

        WriteAtomically(depsPath, root.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
        WriteAtomically(runtimeConfigPath, config.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
    }

    /// <summary>Remove only a deps file previously generated by this writer.</summary>
    public static void RemoveGeneratedManifest(string executablePath)
    {
        string depsPath = Path.ChangeExtension(Path.GetFullPath(executablePath), ".deps.json");
        if (!File.Exists(depsPath)) return;
        try
        {
            using var document = JsonDocument.Parse(File.ReadAllText(depsPath));
            if (document.RootElement.ValueKind != JsonValueKind.Object ||
                !document.RootElement.TryGetProperty(Marker, out var marker) ||
                marker.ValueKind != JsonValueKind.Object ||
                !marker.TryGetProperty("generatedNuGetManifest", out var generated) ||
                generated.ValueKind != JsonValueKind.True)
                return;
        }
        catch (JsonException) { return; }
        File.Delete(depsPath);
    }

    private static void WriteAtomically(string path, string content)
    {
        string temporary = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            File.WriteAllText(temporary, content);
            File.Move(temporary, path, true);
        }
        finally
        {
            if (File.Exists(temporary)) File.Delete(temporary);
        }
    }
}
