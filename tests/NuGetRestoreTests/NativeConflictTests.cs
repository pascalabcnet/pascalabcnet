using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using PascalABCNet.PackageServices;

internal static partial class Program
{
    private static async Task CheckNativeConflicts(string work)
    {
        string directory = Path.Combine(work, "native-conflicts");
        Directory.CreateDirectory(directory);
        string left = Path.Combine(directory, "left", "collision.dll");
        string right = Path.Combine(directory, "right", "COLLISION.dll");
        Directory.CreateDirectory(Path.GetDirectoryName(left)!);
        Directory.CreateDirectory(Path.GetDirectoryName(right)!);
        byte[] bytes = Enumerable.Repeat((byte)7, 200001).ToArray();
        File.WriteAllBytes(left, bytes);
        File.WriteAllBytes(right, bytes);
        ResolvedPackage Package(string id, string path) => new(id, "1.0.0", Path.GetDirectoryName(path)!,
            Array.Empty<ResolvedPackageDependency>(), Array.Empty<string>(), Array.Empty<string>()) {
                NativeLibraries = new[] { path }
            };
        var a = Package("Native.A", left);
        var b = Package("Native.B", right);
        var graph = new ResolvedPackageGraph("net10.0", "", new[] { a, b }, false, 0,
            Array.Empty<PackageDiagnostic>()) {
                RuntimeIdentifier = "win-x64",
                Configuration = new PackageRestoreConfiguration(directory, directory,
                    Array.Empty<PackageSourceInfo>(), Array.Empty<string>())
            };
        NativePackageConflictValidator.Validate(graph);
        bytes[^1] = 8;
        File.WriteAllBytes(right, bytes);
        NativePackageConflictException Reject(ResolvedPackageGraph candidate)
        {
            try { NativePackageConflictValidator.Validate(candidate); }
            catch (NativePackageConflictException error) { return error; }
            throw new Exception("Different same-name native DLLs were accepted.");
        }
        var conflict = Reject(graph);
        Check(conflict.FirstPackage.Id == a.Id && conflict.SecondPackage.Id == b.Id &&
            conflict.FirstPath == left && conflict.SecondPath == right &&
            conflict.Message.Contains("1.0.0"), "Native conflict lost package identities or full paths.");
        // Native naming follows the target, not the OS running the validator.
        NativePackageConflictValidator.Validate(graph with { RuntimeIdentifier = "linux-x64" });
        Reject(graph with { Packages = new[] { a with { NativeLibraries = new[] { left, right } } } });
        NativePackageConflictValidator.Validate(graph with { Packages = new[] {
            a with { NativeLibraries = Array.Empty<string>(), RuntimeAssemblies = new[] { left } },
            b with { NativeLibraries = Array.Empty<string>(), RuntimeAssemblies = new[] { right } }
        } });
        bytes[^1] = 7;
        File.WriteAllBytes(right, bytes);
        NativePackageConflictValidator.Validate(graph);
        File.WriteAllBytes(right, new byte[] { 7 });
        Reject(graph);
        Console.WriteLine("PASS: identical native files allowed; different contents/length rejected; target case rules, same-package collisions and replacement revalidation.");

        if (!OperatingSystem.IsWindows())
            throw new PlatformNotSupportedException("Native conflict compiler regression requires Windows.");
        string feed = Path.Combine(directory, "feed");
        string cache = Path.Combine(directory, "packages");
        string output = Path.Combine(directory, "output");
        Directory.CreateDirectory(feed);
        Directory.CreateDirectory(output);
        const string firstId = "PascalABC.NativeFirst";
        const string equalId = "PascalABC.NativeEqual";
        const string differentId = "PascalABC.NativeDifferent";
        const string parentId = "PascalABC.NativeParent";
        void Pack(string id, bool different = false, string? dependency = null)
        {
            var builder = new PackageBuilder { Id = id, Version = NuGetVersion.Parse("1.0.0"),
                Description = "Local native conflict fixture; never executed" };
            builder.Authors.Add("PascalABC.NET tests");
            if (dependency != null)
                builder.DependencyGroups.Add(new PackageDependencyGroup(NuGetFramework.ParseFolder("net10.0"),
                    new[] { new PackageDependency(dependency, VersionRange.Parse("[1.0.0]")) }));
            else
            {
                string fixture = Path.Combine(directory, id + ".payload");
                File.WriteAllBytes(fixture, different ? new byte[] { 1, 2, 4 } : new byte[] { 1, 2, 3 });
                builder.Files.Add(new PhysicalPackageFile { SourcePath = fixture,
                    TargetPath = "runtimes/win-x64/native/collision.dll" });
            }
            using var file = File.Create(Path.Combine(feed, id + ".1.0.0.nupkg"));
            builder.Save(file);
        }
        Pack(firstId); Pack(equalId); Pack(differentId, different: true); Pack(parentId, dependency: differentId);
        WriteConfiguration(Path.Combine(directory, "NuGet.Config"), feed, cache);
        string source = Path.Combine(directory, "Main.pas");
        string unit = Path.Combine(directory, "NativeUnit.pas");
        foreach (string path in new[] { source, unit, Path.Combine(directory, "OtherUnit.pas") })
            HiddenRestoreDirectories.Add(new PackageRestoreService().GetConfiguration(new PackageRestoreRequest {
                Packages = Array.Empty<PackageRequirement>(), ProgramFilePath = path,
                Sources = new[] { feed }, PackagesDirectory = cache }).RestoreDirectory);
        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
        string compiler = Path.Combine(Environment.GetEnvironmentVariable("PABC_NUGET_TEST_COMPILER_DIR")
            ?? Path.Combine(repository, "bin-net10"), "pabcnetcclear.exe");
        Check(File.Exists(compiler), "Build the modern compiler before --native-conflicts.");
        string Directive(string id) => "{$package " + id + " 1.0.0}\n";
        File.WriteAllText(source, Directive(firstId) + Directive(equalId) + "begin end.");
        var equal = await RunCompiler(compiler, source, cache, output);
        Check(equal.ExitCode == 0, "Identical same-name DLLs must compile: " + equal.Output + equal.Error);
        var outputs = new[] { "Main.exe", "Main.deps.json", "Main.runtimeconfig.json" }
            .ToDictionary(name => Path.Combine(output, name), name => Hash(Path.Combine(output, name)));
        async Task MustFail(string text, string label)
        {
            File.WriteAllText(source, text);
            var result = await RunCompiler(compiler, source, cache, output);
            string diagnostic = result.Output + result.Error;
            Check(result.ExitCode != 0 && diagnostic.Contains(firstId) && diagnostic.Contains(differentId) &&
                diagnostic.Contains("1.0.0") && diagnostic.Contains("collision.dll") &&
                diagnostic.Contains(Path.Combine(cache, firstId.ToLowerInvariant(), "1.0.0")) &&
                diagnostic.Contains(Path.Combine(cache, differentId.ToLowerInvariant(), "1.0.0")),
                label + ": expected package/path native conflict, got " + diagnostic);
            Check(outputs.All(pair => Hash(pair.Key) == pair.Value), label + ": conflict changed generated output.");
        }
        await MustFail(Directive(firstId) + Directive(differentId) + "begin end.", "direct packages");
        await MustFail(Directive(firstId) + Directive(parentId) + "begin end.", "transitive package");
        File.WriteAllText(unit, Directive(firstId) + "unit NativeUnit; interface implementation end.");
        File.WriteAllText(Path.Combine(directory, "OtherUnit.pas"),
            Directive(differentId) + "unit OtherUnit; interface implementation end.");
        await MustFail("uses NativeUnit, OtherUnit; begin end.", "source modules");

        File.WriteAllText(source, "uses NativeUnit; begin end.");
        var warm = await RunCompiler(compiler, source, cache, output);
        Check(warm.ExitCode == 0 && File.Exists(Path.ChangeExtension(unit, ".pcu")),
            "Warm PCU compilation failed: " + warm.Output + warm.Error);
        foreach (string path in outputs.Keys.ToArray()) outputs[path] = Hash(path);
        File.Move(unit, unit + ".hidden");
        await MustFail(Directive(differentId) + "uses NativeUnit; begin end.", "package from PCU");
        Console.WriteLine("PASS: native conflicts diagnosed before generation, with packages/versions/paths; direct, transitive, source modules and PCU; previous output preserved.");
    }
}
