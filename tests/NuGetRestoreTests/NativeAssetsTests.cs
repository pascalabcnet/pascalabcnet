using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using PascalABCNet.PackageServices;

internal static partial class Program
{
    private static async Task CheckNativeAssets(string work)
    {
        string feed = Path.Combine(work, "feed");
        const string id = "PascalABC.TestNativeAssets";
        const string fallbackId = "PascalABC.TestNativeFallback";
        string original = Path.Combine(AppContext.BaseDirectory, "fixtures", "lib", "TestDependency.dll");
        string platform = Path.Combine(AppContext.BaseDirectory, "fixtures", "v2", "lib", "TestDependency.dll");
        // These payloads exercise NuGet selection only, never native execution.
        var builder = new PackageBuilder { Id = id, Version = NuGetVersion.Parse("1.0.0"), Description = "Local native asset selection" };
        builder.Authors.Add("PascalABC.NET tests");
        foreach (var item in new[] {
            (original, "lib/net10.0/TestDependency.dll"),
            (platform, "runtimes/win-x64/lib/net10.0/TestDependency.dll"),
            (original, "runtimes/win-x64/native/native-fixture.dll"),
            (platform, "runtimes/win-x86/native/native-fixture.dll"),
            (platform, "runtimes/linux-x64/native/libnative-fixture.so") })
            builder.Files.Add(new PhysicalPackageFile { SourcePath = item.Item1, TargetPath = item.Item2 });
        builder.DependencyGroups.Add(new PackageDependencyGroup(NuGetFramework.ParseFolder("net10.0"),
            new[] { new PackageDependency(fallbackId, VersionRange.Parse("[1.0.0]")) }));
        using (var file = File.Create(Path.Combine(feed, id + ".1.0.0.nupkg"))) builder.Save(file);
        var fallback = new PackageBuilder { Id = fallbackId, Version = NuGetVersion.Parse("1.0.0"), Description = "Portable RID fallback" };
        fallback.Authors.Add("PascalABC.NET tests");
        fallback.Files.Add(new PhysicalPackageFile { SourcePath = original, TargetPath = "runtimes/win/native/fallback-fixture.dll" });
        using (var file = File.Create(Path.Combine(feed, fallbackId + ".1.0.0.nupkg"))) fallback.Save(file);
        var request = new PackageRestoreRequest {
            Packages = new[] { new PackageRequirement(id, "1.0.0") },
            RuntimeIdentifier = "win-x64", Sources = new[] { feed },
            PackagesDirectory = Path.Combine(work, "native-packages"), RestoreDirectory = Path.Combine(work, "native-restore")
        };
        var service = new PackageRestoreService();
        var managedOnly = await service.RestoreAsync(request with {
            RuntimeIdentifier = null, Packages = new[] { new PackageRequirement(RootId, "1.0.0") },
            RestoreDirectory = Path.Combine(work, "managed-portability") });
        Check(managedOnly.RuntimeIdentifier is null, "An ordinary managed package graph became platform-specific.");
        var graph = await service.RestoreAsync(request);
        var selected = graph.Packages.Single(p => p.Id == id);
        Check(graph.RuntimeIdentifier == "win-x64", "Selected RID was lost.");
        Check(selected.RuntimeAssemblies.Single().Replace('\\', '/').EndsWith("/runtimes/win-x64/lib/net10.0/TestDependency.dll"),
            "A generic implementation was used instead of the RID-specific managed assembly.");
        Check(Hash(selected.RuntimeAssemblies.Single()) == Hash(platform), "Wrong managed implementation was selected.");
        Check(selected.CompileAssemblies.Single().Replace('\\', '/').EndsWith("/lib/net10.0/TestDependency.dll"),
            "RID selection changed the compile-time API.");
        Check(selected.NativeLibraries.Single().Replace('\\', '/').EndsWith("/runtimes/win-x64/native/native-fixture.dll"),
            "Native files for a different architecture were selected.");
        Check(graph.Packages.Single(p => p.Id == fallbackId).NativeLibraries.Single()
            .Replace('\\', '/').EndsWith("/runtimes/win/native/fallback-fixture.dll"), "Official RID fallback was not used.");
        Check(graph.Packages.All(p => !p.RuntimeAssemblies.Intersect(p.NativeLibraries).Any()),
            "A native DLL became a semantic assembly reference.");
        var again = await service.RestoreAsync(request);
        Check(again.UsedCachedRestore && again.InstalledPackageCount == 0, "An unchanged RID restore was not cached.");
        try { await service.RestoreAsync(request with { RuntimeIdentifier = "win-arm64" }); throw new Exception("Unsupported RID was silently accepted."); }
        catch (ArgumentException) { }
        string manifestDirectory = Path.Combine(work, "native-manifest");
        Directory.CreateDirectory(manifestDirectory);
        string executable = Path.Combine(manifestDirectory, "NativeManifest.exe");
        File.Copy(original, executable);
        File.WriteAllText(Path.ChangeExtension(executable, ".runtimeconfig.json"),
            "{\"runtimeOptions\":{\"tfm\":\"net10.0\",\"framework\":{\"name\":\"Microsoft.NETCore.App\",\"version\":\"10.0.0\"}}}");
        PackageRuntimeManifestWriter.WriteForExecutable(graph, executable);
        string depsPath = Path.ChangeExtension(executable, ".deps.json");
        string originalManifest = Hash(depsPath);
        var escaping = graph with { Packages = graph.Packages.Select(package => package.Id == id
            ? package with { NativeLibraries = new[] { original } } : package).ToArray() };
        try { PackageRuntimeManifestWriter.WriteForExecutable(escaping, executable); throw new Exception("Native asset escaped its package directory."); }
        catch (InvalidDataException) { }
        Check(Hash(depsPath) == originalManifest, "Invalid native assets overwrote the last valid manifest.");
        using (var document = System.Text.Json.JsonDocument.Parse(File.ReadAllText(Path.ChangeExtension(executable, ".deps.json"))))
        {
            string target = document.RootElement.GetProperty("runtimeTarget").GetProperty("name").GetString()!;
            Check(target.EndsWith("/win-x64"), "The executable's native RID is missing.");
            Check(document.RootElement.GetProperty("targets").GetProperty(target).GetProperty(id + "/1.0.0")
                .GetProperty("native").EnumerateObject().Single().Name == "runtimes/win-x64/native/native-fixture.dll",
                "The deps manifest lost selected native assets.");
        }
        string published = PackagePublisher.Publish(executable, Path.Combine(manifestDirectory, "NativeManifest.pas"), manifestDirectory);
        string publishedNative = Path.Combine(published, "native-fixture.dll");
        Check(File.Exists(publishedNative) && File.Exists(Path.Combine(published, "fallback-fixture.dll")),
            "Publishing omitted a native dependency.");
        Check(!File.Exists(Path.Combine(published, "NativeManifest.deps.json")), "Publishing retained a cache-bound manifest.");
        string nativePath = selected.NativeLibraries.Single();
        File.Move(nativePath, nativePath + ".hidden");
        try
        {
            try { await service.RestoreAsync(request); throw new Exception("Cached restore ignored a missing native file."); }
            catch (FileNotFoundException) { }
            try { PackagePublisher.Publish(executable, Path.Combine(manifestDirectory, "NativeManifest.pas"), manifestDirectory);
                throw new Exception("Publishing silently omitted a missing native library."); }
            catch (FileNotFoundException) { }
            Check(Hash(publishedNative) == Hash(original), "Failed publishing damaged the previous publication.");
        }
        finally { File.Move(nativePath + ".hidden", nativePath); }
        Console.WriteLine("PASS: native assets: win-x64 selection, managed override, RID fallback, separate native files and cached restore; local feed only.");
        Console.WriteLine("PASS: native manifest, publication and preservation of previous output on missing native files.");
    }
}
