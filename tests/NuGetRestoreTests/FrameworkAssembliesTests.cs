using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using NuGet.Packaging;
using NuGet.Versioning;
using PascalABCNet.PackageServices;

internal static partial class Program
{
    private static async Task CheckFrameworkAssemblies(string work)
    {
        string directory = Path.Combine(work, "framework-assemblies");
        string feed = Path.Combine(directory, "feed");
        Directory.CreateDirectory(feed);
        string framework = typeof(ImmutableArray<>).Assembly.Location;
        string fileName = Path.GetFileName(framework);
        var identity = AssemblyName.GetAssemblyName(framework);
        string fixture = Path.Combine(AppContext.BaseDirectory, "fixtures", "lib", "TestDependency.dll");

        // Synthetic files test identity/version policy only; none is loaded/executed.
        string MakeAssembly(string name, Version version, byte[]? publicKey, string stem, string culture = "")
        {
            var metadata = new MetadataBuilder();
            metadata.AddModule(0, metadata.GetOrAddString(fileName), metadata.GetOrAddGuid(Guid.NewGuid()), default, default);
            metadata.AddAssembly(metadata.GetOrAddString(name), version, metadata.GetOrAddString(culture),
                publicKey is null ? default : metadata.GetOrAddBlob(publicKey),
                publicKey is null ? 0 : AssemblyFlags.PublicKey, AssemblyHashAlgorithm.None);
            metadata.AddTypeDefinition(TypeAttributes.NotPublic, default, metadata.GetOrAddString("<Module>"),
                default, MetadataTokens.FieldDefinitionHandle(1), MetadataTokens.MethodDefinitionHandle(1));
            var pe = new ManagedPEBuilder(new PEHeaderBuilder(imageCharacteristics: Characteristics.ExecutableImage | Characteristics.Dll),
                new MetadataRootBuilder(metadata), new BlobBuilder());
            var blob = new BlobBuilder();
            pe.Serialize(blob);
            string path = Path.Combine(directory, stem + ".dll");
            File.WriteAllBytes(path, blob.ToArray());
            return path;
        }
        string old = MakeAssembly(identity.Name!, new Version(9, 0, 0, 0), identity.GetPublicKey(), "older");
        string newer = MakeAssembly(identity.Name!, new Version(11, 0, 0, 0), identity.GetPublicKey(), "newer");
        string unsigned = MakeAssembly(identity.Name!, identity.Version!, null, "unsigned");
        string otherKey = MakeAssembly(identity.Name!, identity.Version!,
            typeof(NuGetVersion).Assembly.GetName().GetPublicKey(), "other-key");
        string otherCulture = MakeAssembly(identity.Name!, identity.Version!, identity.GetPublicKey(), "other-culture", "fr");
        string otherIdentity = MakeAssembly("Unrelated.Assembly", identity.Version!, identity.GetPublicKey(), "unrelated");
        string invalid = Path.Combine(directory, "invalid.dll");
        File.WriteAllText(invalid, "Not a managed assembly");
        var cases = new List<(string Name, string File, bool Provided)> {
            (Name: "Same", File: framework, Provided: true),
            (Name: "Older", File: old, Provided: true),
            (Name: "Newer", File: newer, Provided: false),
            (Name: "Unsigned", File: unsigned, Provided: false),
            (Name: "OtherKey", File: otherKey, Provided: false),
            (Name: "OtherCulture", File: otherCulture, Provided: false),
            (Name: "Renamed", File: fixture, Provided: false),
            (Name: "OtherIdentity", File: otherIdentity, Provided: false),
            (Name: "Invalid", File: invalid, Provided: false)
        };
        string desktopRoot = Path.GetFullPath(Path.Combine(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory(),
            "../../Microsoft.WindowsDesktop.App"));
        string? desktop = Directory.Exists(desktopRoot) ? Directory.EnumerateDirectories(desktopRoot)
            .Select(dir => Path.Combine(dir, "System.IO.Packaging.dll")).FirstOrDefault(File.Exists) : null;
        if (desktop is not null) cases.Add(("WindowsDesktop", desktop, false));
        var service = new PackageRestoreService();
        foreach (var item in cases)
        {
            string id = "PascalABC.FrameworkPolicy." + item.Name;
            var package = new PackageBuilder { Id = id, Version = NuGetVersion.Parse("1.0.0"), Description = "Framework assembly selection" };
            package.Authors.Add("PascalABC.NET tests");
            string targetName = item.Name == "WindowsDesktop" ? Path.GetFileName(item.File) : fileName;
            package.Files.Add(new PhysicalPackageFile { SourcePath = item.File, TargetPath = "lib/net10.0/" + targetName });
            package.Files.Add(new PhysicalPackageFile { SourcePath = fixture, TargetPath = "lib/net10.0/TestDependency.dll" });
            // Same bytes/name as a core assembly, but in native assets: never filter it.
            package.Files.Add(new PhysicalPackageFile { SourcePath = framework, TargetPath = "runtimes/win-x64/native/" + fileName });
            using (var file = File.Create(Path.Combine(feed, id + ".1.0.0.nupkg"))) package.Save(file);
            var request = new PackageRestoreRequest {
                Packages = new[] { new PackageRequirement(id, "1.0.0") }, Sources = new[] { feed }, RuntimeIdentifier = "win-x64",
                PackagesDirectory = Path.Combine(directory, "packages"), RestoreDirectory = Path.Combine(directory, item.Name)
            };
            var graph = await service.RestoreAsync(request);
            var selected = graph.Packages.Single();
            int count = item.Provided ? 1 : 2;
            Check(selected.CompileAssemblies.Count == count && selected.RuntimeAssemblies.Count == count &&
                selected.RuntimeAssemblies.Any(path => Path.GetFileName(path) == "TestDependency.dll") &&
                selected.NativeLibraries.Count == 1, "Wrong framework assembly choice: " + item.Name);
            var cached = await service.RestoreAsync(request);
            Check(cached.UsedCachedRestore && cached.Packages.Single().RuntimeAssemblies.SequenceEqual(selected.RuntimeAssemblies),
                "No-op changed the framework assembly choice: " + item.Name);
            string output = Path.Combine(directory, item.Name + ".exe");
            File.Copy(fixture, output);
            File.WriteAllText(Path.ChangeExtension(output, ".runtimeconfig.json"),
                "{\"runtimeOptions\":{\"tfm\":\"net10.0\",\"framework\":{\"name\":\"Microsoft.NETCore.App\",\"version\":\"10.0.0\"}}}");
            PackageRuntimeManifestWriter.WriteForExecutable(graph, output);
            using var deps = System.Text.Json.JsonDocument.Parse(File.ReadAllText(Path.ChangeExtension(output, ".deps.json")));
            var target = deps.RootElement.GetProperty("targets").EnumerateObject().Single().Value;
            var runtime = target.EnumerateObject().Single().Value.GetProperty("runtime").EnumerateObject().ToArray();
            Check(runtime.Length == count, "Manifest disagrees with semantic references: " + item.Name);
        }
        Console.WriteLine("PASS: core framework replaces only matching signed older/equal package DLLs; newer, foreign and native files remain; no-op and manifest agree.");
    }
}
