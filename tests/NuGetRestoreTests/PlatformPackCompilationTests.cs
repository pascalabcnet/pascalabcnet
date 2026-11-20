using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using PascalABCNet.PackageServices;

internal static partial class Program
{
    // The existing Windows-target test must remain local-feed-only now that the
    // compiler also resolves platform references. Use a distinct empty assembly
    // so this fixture cannot collide with ordinary TestRoot/TestDependency packs.
    private static void CreateEmptyWindowsPlatformFixture(string feed, string directory)
    {
        const string name = "WindowsPlatformFixture";
        var metadata = new MetadataBuilder();
        metadata.AddModule(0, metadata.GetOrAddString(name + ".dll"),
            metadata.GetOrAddGuid(Guid.NewGuid()), default, default);
        metadata.AddAssembly(metadata.GetOrAddString(name), new Version(1, 0, 0, 0),
            default, default, (AssemblyFlags)0, AssemblyHashAlgorithm.None);
        metadata.AddTypeDefinition(TypeAttributes.NotPublic, default, metadata.GetOrAddString("<Module>"),
            default, MetadataTokens.FieldDefinitionHandle(1), MetadataTokens.MethodDefinitionHandle(1));
        var pe = new ManagedPEBuilder(new PEHeaderBuilder(
            imageCharacteristics: Characteristics.ExecutableImage | Characteristics.Dll),
            new MetadataRootBuilder(metadata), new BlobBuilder());
        var blob = new BlobBuilder();
        pe.Serialize(blob);
        string fixture = Path.Combine(directory, name + ".dll");
        File.WriteAllBytes(fixture, blob.ToArray());
        CreateWindowsPlatformFixture(feed, directory, fixture, name);
    }

    private static SelectedPlatformPackage CreateWindowsPlatformFixture(string feed, string directory,
        string fixture, string assemblyName)
    {
        var selected = PlatformPackSelector.Select(PlatformPackCatalogReader.ReadNet10(),
            "net10.0-windows10.0.19041.0").Packages.Single();
        string asset = "lib/net10.0/" + assemblyName + ".dll";
        string runtimeList = Path.Combine(directory, "RuntimeList.xml");
        new XDocument(new XElement("FileList", new XElement("File",
            new XAttribute("Type", "Managed"), new XAttribute("Profile", selected.Profile),
            new XAttribute("Path", asset), new XAttribute("AssemblyName", assemblyName)))).Save(runtimeList);
        var pack = new PackageBuilder { Id = selected.Id, Version = NuGetVersion.Parse(selected.Version),
            Description = "Isolated Windows platform compiler fixture" };
        pack.Authors.Add("PascalABC.NET tests");
        pack.PackageTypes.Add(new PackageType("DotnetPlatform", new Version(1, 0)));
        pack.Files.Add(new PhysicalPackageFile { SourcePath = fixture, TargetPath = asset });
        pack.Files.Add(new PhysicalPackageFile { SourcePath = runtimeList, TargetPath = "data/RuntimeList.xml" });
        using (var stream = File.Create(Path.Combine(feed, selected.Id + "." + selected.Version + ".nupkg")))
            pack.Save(stream);
        return selected;
    }

    private static async Task CheckPlatformPackCompilation(string work, bool runRuntime = false)
    {
        string directory = Path.Combine(work, "platform-compilation");
        string feed = Path.Combine(directory, "feed");
        string cache = Path.Combine(directory, "packages");
        string output = Path.Combine(directory, "output");
        Directory.CreateDirectory(feed);
        Directory.CreateDirectory(output);
        WriteConfiguration(Path.Combine(directory, "NuGet.Config"), feed, cache);
        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
        string compiler = Path.Combine(Environment.GetEnvironmentVariable("PABC_NUGET_TEST_COMPILER_DIR")
            ?? Path.Combine(repository, "bin-net10"), "pabcnetcclear.exe");
        Check(File.Exists(compiler), "Build the modern console compiler before --platform-pack-compilation.");

        string fixture = Path.Combine(AppContext.BaseDirectory, "fixtures", "lib", "TestDependency.dll");
        Check(File.Exists(fixture), "TestDependency fixture must be built.");
        var selected = CreateWindowsPlatformFixture(feed, directory, fixture, "TestDependency");

        string source = Path.Combine(directory, "platform-main.pas");
        HiddenRestoreDirectories.Add(new PackageRestoreService().GetConfiguration(new PackageRestoreRequest {
            Packages = Array.Empty<PackageRequirement>(), ProgramFilePath = source,
            Sources = new[] { feed }, PackagesDirectory = cache
        }).RestoreDirectory);
        File.WriteAllText(source, "{$target windows}\nuses NuGetRestoreFixture;\nbegin Writeln(Dependency.Value); end.");
        var first = await RunCompiler(compiler, source, cache, output);
        Check(first.ExitCode == 0, "A type from the platform package must be available to semantics: " + first.Output + first.Error);
        string installedDll = Path.Combine(cache, selected.Id.ToLowerInvariant(), selected.Version,
            "lib", "net10.0", "TestDependency.dll");
        Check(File.Exists(installedDll), "The platform DLL must remain in the NuGet cache.");
        var configuration = new PackageRestoreService().GetConfiguration(new PackageRestoreRequest {
            Packages = Array.Empty<PackageRequirement>(), ProgramFilePath = source,
            Sources = new[] { feed }, PackagesDirectory = cache
        });
        var platformGraph = PlatformPackRuntimeGraph.FromPacks(
            PlatformPackSelector.Select(PlatformPackCatalogReader.ReadNet10(), "net10.0-windows10.0.19041.0"),
            new[] { new CachedPlatformPackage(selected.Id, selected.Version,
                Path.Combine(cache, selected.Id.ToLowerInvariant(), selected.Version), true) }, configuration);
        Check(PlatformPackRuntimeGraph.Merge(null, platformGraph).Packages.Count == 1,
            "Platform-only applications need a runtime graph even without {$package}.");
        void RejectMerge(ResolvedPackageGraph ordinary)
        {
            try { PlatformPackRuntimeGraph.Merge(ordinary, platformGraph); }
            catch (InvalidDataException) { return; }
            throw new Exception("Incompatible platform runtime graphs were silently merged.");
        }
        RejectMerge(platformGraph with { TargetFramework = "net10.0" });
        RejectMerge(platformGraph with { Configuration = configuration with { PackagesDirectory = cache + "-other" } });
        RejectMerge(platformGraph with { Packages = new[] { platformGraph.Packages.Single() with { Version = "999.0.0" } } });
        Check(!File.Exists(Path.Combine(directory, "TestDependency.dll")) &&
            !File.Exists(Path.Combine(output, "TestDependency.dll")),
            "Platform compilation must not copy DLLs beside source or EXE.");
        string executable = Path.Combine(output, "platform-main.exe");
        using (var deps = System.Text.Json.JsonDocument.Parse(File.ReadAllText(Path.ChangeExtension(executable, ".deps.json"))))
        {
            var target = deps.RootElement.GetProperty("targets").EnumerateObject().Single().Value;
            var assets = target.GetProperty(selected.Id + "/" + selected.Version).GetProperty("runtime");
            Check(assets.TryGetProperty("lib/net10.0/TestDependency.dll", out _),
                "The runtime manifest must include the selected platform implementation.");
        }
        using (var config = System.Text.Json.JsonDocument.Parse(File.ReadAllText(Path.ChangeExtension(executable, ".runtimeconfig.json"))))
            Check(config.RootElement.GetProperty("runtimeOptions").GetProperty("additionalProbingPaths")
                .EnumerateArray().Any(p => p.GetString() == cache), "Runtime probing must point to the NuGet cache.");
        async Task CheckLaunch(string expected)
        {
            if (!runRuntime) return;
            var run = await RunDotnet(executable, directory);
            Check(run.ExitCode == 0 && run.Output.Trim() == expected,
                "Platform program failed to run directly from cache: " + run.Output + run.Error);
            Check(!File.Exists(Path.Combine(directory, "TestDependency.dll")) &&
                !File.Exists(Path.Combine(output, "TestDependency.dll")) &&
                !File.Exists(Path.Combine(output, "TestRoot.dll")), "Launching must not copy package DLLs.");
        }
        await CheckLaunch("42");

        // A second build must work with the already downloaded pack and no feed access.
        Directory.Move(feed, feed + "-offline");
        var repeat = await RunCompiler(compiler, source, cache, output);
        Check(repeat.ExitCode == 0, "Repeated compilation must use cached platform DLLs: " + repeat.Output + repeat.Error);
        await CheckLaunch("42");
        Directory.Move(feed + "-offline", feed);

        string unit = Path.Combine(directory, "PlatformConsumer.pas");
        HiddenRestoreDirectories.Add(new PackageRestoreService().GetConfiguration(new PackageRestoreRequest {
            Packages = Array.Empty<PackageRequirement>(), ProgramFilePath = unit,
            Sources = new[] { feed }, PackagesDirectory = cache
        }).RestoreDirectory);
        File.WriteAllText(unit, """
unit PlatformConsumer;
interface
uses NuGetRestoreFixture;
function ReadPlatformValue: integer;
implementation
function ReadPlatformValue: integer := Dependency.Value;
end.
""");
        File.WriteAllText(source, "{$target windows}\nuses PlatformConsumer;\nbegin Writeln(ReadPlatformValue); end.");
        var fromSource = await RunCompiler(compiler, source, cache, output);
        Check(fromSource.ExitCode == 0, "Platform references must also be visible in units: " + fromSource.Output + fromSource.Error);
        Check(File.Exists(Path.Combine(directory, "PlatformConsumer.pcu")), "The consumer PCU must be generated.");
        File.Move(unit, unit + ".hidden");
        var fromPcu = await RunCompiler(compiler, source, cache, output);
        Check(fromPcu.ExitCode == 0, "PCU assembly lookup must resolve cached platform DLLs: " + fromPcu.Output + fromPcu.Error);
        await CheckLaunch("42");

        // An ordinary NuGet assembly depends on the platform implementation.
        // Both must remain in the same host manifest, without copying either DLL.
        const string rootId = "PascalABC.PlatformRuntimeConsumer";
        var consumer = new PackageBuilder { Id = rootId, Version = NuGetVersion.Parse("1.0.0"),
            Description = "Ordinary package consuming a platform runtime assembly" };
        consumer.Authors.Add("PascalABC.NET tests");
        consumer.Files.Add(new PhysicalPackageFile {
            SourcePath = Path.Combine(AppContext.BaseDirectory, "fixtures", "lib", "TestRoot.dll"),
            TargetPath = "lib/net10.0/TestRoot.dll" });
        using (var stream = File.Create(Path.Combine(feed, rootId + ".1.0.0.nupkg"))) consumer.Save(stream);
        File.WriteAllText(source, "{$target windows}\n{$package " + rootId + " 1.0.0}\n" +
            "uses NuGetRestoreFixture;\nbegin Writeln(Root.Value); end.");
        var combined = await RunCompiler(compiler, source, cache, output);
        Check(combined.ExitCode == 0, "Ordinary and platform references must coexist: " + combined.Output + combined.Error);
        using (var deps = System.Text.Json.JsonDocument.Parse(File.ReadAllText(Path.ChangeExtension(executable, ".deps.json"))))
        {
            var libraries = deps.RootElement.GetProperty("libraries");
            Check(libraries.TryGetProperty(rootId + "/1.0.0", out _) &&
                libraries.TryGetProperty(selected.Id + "/" + selected.Version, out _), "Both runtime packages must be retained.");
        }
        await CheckLaunch("43");

        // Normal NuGet references still participate in the existing conflict checks.
        const string conflictId = "PascalABC.PlatformConflict";
        var conflict = new PackageBuilder { Id = conflictId, Version = NuGetVersion.Parse("1.0.0"),
            Description = "Different TestDependency implementation" };
        conflict.Authors.Add("PascalABC.NET tests");
        conflict.Files.Add(new PhysicalPackageFile {
            SourcePath = Path.Combine(AppContext.BaseDirectory, "fixtures", "v2", "lib", "TestDependency.dll"),
            TargetPath = "lib/net10.0/TestDependency.dll" });
        using (var stream = File.Create(Path.Combine(feed, conflictId + ".1.0.0.nupkg")))
            conflict.Save(stream);
        File.WriteAllText(source, "{$target windows}\n{$package " + conflictId + " 1.0.0}\nbegin end.");
        var incompatible = await RunCompiler(compiler, source, cache, output);
        Check(incompatible.ExitCode != 0 && (incompatible.Output + incompatible.Error).Contains("TestDependency"),
            "Conflicting normal/package platform DLLs must not be silently accepted.");

        string untouchedCache = Path.Combine(directory, "core-packages-must-not-be-created");
        File.WriteAllText(source, "begin var n := 1; end.");
        var core = await RunCompiler(compiler, source, untouchedCache, output);
        Check(core.ExitCode == 0 && !Directory.Exists(untouchedCache),
            "The plain net10.0 path must not acquire a Windows platform package.");
        Check(!File.Exists(Path.ChangeExtension(executable, ".deps.json")),
            "Removing the Windows target must remove the generated platform manifest.");
        Console.WriteLine(runRuntime
            ? "PASS: platform runtime from cache, source/PCU, offline reuse, NuGet coexistence and no DLL copying."
            : "PASS: platform compilation, runtime manifests, cache reuse, source/PCU and conflicts.");
    }
}
