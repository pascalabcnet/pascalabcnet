using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using PascalABCNet.PackageServices;

internal static partial class Program
{
    private static async Task CheckModulePackagesInSeparateProcess()
    {
        // Compiler and IntelliSense use different processes in the product.
        // Existing IntelliSense DOMs must not pin the compiler's test context.
        var start = new System.Diagnostics.ProcessStartInfo("dotnet") {
            UseShellExecute = false, RedirectStandardOutput = true,
            RedirectStandardError = true, CreateNoWindow = true
        };
        start.ArgumentList.Add(typeof(Program).Assembly.Location);
        start.ArgumentList.Add("--module-packages");
        using var child = System.Diagnostics.Process.Start(start)!;
        var output = child.StandardOutput.ReadToEndAsync();
        var errors = child.StandardError.ReadToEndAsync();
        try { await child.WaitForExitAsync().WaitAsync(TimeSpan.FromMinutes(4)); }
        catch { if (!child.HasExited) child.Kill(entireProcessTree: true); throw; }
        Console.Write(await output);
        Check(child.ExitCode == 0, "Module-package compiler tests failed: " + await errors);
    }

    private static async Task CheckModulePackages(string work)
    {
        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
        string compilerDirectory = Environment.GetEnvironmentVariable("PABC_NUGET_TEST_COMPILER_DIR")
            ?? Path.Combine(repository, "bin-net10");
        string compiler = Path.Combine(compilerDirectory, "pabcnetcclear.exe");
        string directory = Path.Combine(work, "module-packages");
        string feed = Path.Combine(work, "feed");
        string cache = Path.Combine(directory, "packages");
        Directory.CreateDirectory(directory);
        WriteConfiguration(Path.Combine(directory, "NuGet.Config"), feed, cache);
        const string compatible = "PascalABC.TestCompatible";
        const string newer = "PascalABC.TestNewer";
        CreateRequirementPackage(compatible, "[1.0.0, )");
        CreateRequirementPackage(newer, "[1.1.0]");
        var service = new PackageRestoreService();

        void CreateRequirementPackage(string id, string range)
        {
            var builder = new PackageBuilder { Id = id, Version = NuGetVersion.Parse("1.0.0"),
                Description = "Local package for module dependency tests" };
            builder.Authors.Add("PascalABC.NET tests");
            builder.DependencyGroups.Add(new PackageDependencyGroup(NuGetFramework.ParseFolder("net10.0"),
                new[] { new PackageDependency(RootId, VersionRange.Parse(range)) }));
            using var stream = File.Create(Path.Combine(feed, id + ".1.0.0.nupkg"));
            builder.Save(stream);
        }

        string Package(string id = RootId, string version = "1.0.0") => "{$package " + id + " " + version + "}\n";
        string Unit(string header, string extraUses = "", string expression = "NuGetRestoreFixture.Root.Value") =>
            header + "unit P; interface\n" + extraUses +
            "function Value: integer; implementation\nfunction Value: integer; begin Result := " + expression + "; end; end.\n";
        const string main = "uses P; begin Println(Value); end.\n";

        async Task<string> CompileCase(string name, string program, Dictionary<string, string> units,
            string? expected, bool pcu = false, params string[] diagnosticParts)
        {
            string path = Path.Combine(directory, name);
            Directory.CreateDirectory(path);
            string source = Path.Combine(path, "Main.pas");
            File.WriteAllText(source, program);
            foreach (var unit in units)
            {
                string file = Path.Combine(path, unit.Key);
                Directory.CreateDirectory(Path.GetDirectoryName(file)!);
                File.WriteAllText(file, unit.Value);
            }
            string output = Path.Combine(path, "output");
            Directory.CreateDirectory(output);
            HiddenRestoreDirectories.Add(service.GetConfiguration(new PackageRestoreRequest {
                ProgramFilePath = source, Packages = Array.Empty<PackageRequirement>() }).RestoreDirectory);
            var result = await RunCompiler(compiler, source, cache, output);
            string diagnostics = result.Output + result.Error;
            if (expected == null)
            {
                Check(result.ExitCode != 0 && !File.Exists(Path.Combine(output, "Main.exe")),
                    name + ": a conflicting program must not produce an executable: " + diagnostics);
                foreach (string part in diagnosticParts)
                    Check(diagnostics.Contains(part, StringComparison.OrdinalIgnoreCase),
                        name + ": missing diagnostic '" + part + "': " + diagnostics);
            }
            else
            {
                Check(result.ExitCode == 0, name + ": " + diagnostics);
                var run = await RunDotnet(Path.Combine(output, "Main.exe"), output);
                Check(run.ExitCode == 0 && run.Output.Trim() == expected,
                    name + ": incorrect runtime result: " + run.Output + run.Error);
                if (pcu)
                {
                    // Our own temporary sources are hidden, so a successful
                    // repeated build genuinely reads PCUs, not Pascal sources.
                    foreach (string file in units.Keys)
                    {
                        string unit = Path.Combine(path, file);
                        Check(File.Exists(Path.ChangeExtension(unit, ".pcu")), name + ": PCU was not saved");
                        File.Move(unit, unit + ".hidden");
                    }
                    var repeated = await RunCompiler(compiler, source, cache, output);
                    Check(repeated.ExitCode == 0, name + ": PCU-only build failed: " + repeated.Output + repeated.Error);
                    var pcuRun = await RunDotnet(Path.Combine(output, "Main.exe"), output);
                    Check(pcuRun.ExitCode == 0 && pcuRun.Output.Trim() == expected,
                        name + ": PCU-only runtime failed: " + pcuRun.Output + pcuRun.Error);
                }
            }
            Console.WriteLine("PASS: module packages: " + name);
            return path;
        }

        string pcuCase = await CompileCase("package-only-in-unit", main,
            new() { ["P.pas"] = Unit(Package()) }, "43", pcu: true);
        await CompileCase("nested-units-and-pcu", main,
            new() {
                ["P.pas"] = Unit("", "uses Q;\n", "QValue"),
                ["Q.pas"] = Package() + "unit Q; interface function QValue: integer; implementation\n" +
                    "function QValue: integer; begin Result := NuGetRestoreFixture.Root.Value; end; end."
            }, "43", pcu: true);
        await CompileCase("implementation-uses", main,
            new() {
                ["P.pas"] = "unit P; interface function Value: integer; implementation uses Q;\n" +
                    "function Value: integer; begin Result := QValue; end; end.",
                ["Q.pas"] = Package() + "unit Q; interface function QValue: integer; implementation\n" +
                    "function QValue: integer; begin Result := NuGetRestoreFixture.Root.Value; end; end."
            }, "43", pcu: true);
        await CompileCase("same-package-in-main-and-unit", Package() + main,
            new() { ["P.pas"] = Unit(Package(RootId.ToLowerInvariant())) }, "43", pcu: true);
        await CompileCase("compatible-extension", main,
            new() {
                ["P.pas"] = Unit(Package(), "uses Q;\n", "QValue"),
                ["Q.pas"] = Package(DependencyId) + "unit Q; interface function QValue: integer; implementation\n" +
                    "function QValue: integer; begin Result := NuGetRestoreFixture.Root.Value; end; end."
            }, "43", pcu: true);
        await CompileCase("different-exact-versions", main,
            new() {
                ["P.pas"] = Unit(Package(), "uses Q;\n"),
                ["Q.pas"] = Package(RootId, "1.1.0") + "unit Q; interface implementation end."
            }, null, false, RootId, "1.0.0", "1.1.0", "P.pas", "Q.pas");
        await CompileCase("transitive-version-change", main,
            new() {
                ["P.pas"] = Unit(Package(compatible), "uses Q;\n"),
                ["Q.pas"] = Package(newer) + "unit Q; interface implementation end."
            }, null, false, RootId, "1.0.0", "1.1.0", "P.pas", "Q.pas");
        await CompileCase("resolve-new-batch-together", main,
            new() { ["P.pas"] = Unit(Package(compatible) + Package(newer)) }, "44", pcu: true);
        await CompileCase("uses-in-another-directory", "uses P in 'sub/P.pas'; begin Println(Value); end.",
            new() { ["sub/P.pas"] = Unit(Package()) }, "43");

        string direct = Path.Combine(directory, "direct");
        Directory.CreateDirectory(direct);
        string v1 = Path.Combine(direct, "v1");
        string v2 = Path.Combine(direct, "v2");
        Directory.CreateDirectory(v1);
        Directory.CreateDirectory(v2);
        File.Copy(Path.Combine(AppContext.BaseDirectory, "fixtures", "lib", "TestRoot.dll"), Path.Combine(v1, "TestRoot.dll"));
        File.Copy(Path.Combine(AppContext.BaseDirectory, "fixtures", "v2", "lib", "TestRoot.dll"), Path.Combine(v2, "TestRoot.dll"));
        foreach (string version in new[] { v1, v2 })
            File.Copy(Path.Combine(AppContext.BaseDirectory, "fixtures", "lib", "TestDependency.dll"), Path.Combine(version, "TestDependency.dll"));
        string Reference(string version) => "{$reference '" + Path.Combine(version, "TestRoot.dll") + "'}\n";

        await CompileCase("reference-before-package", Reference(v2) + main,
            new() { ["P.pas"] = Unit(Package()) }, null, false, "TestRoot", ".dll");
        await CompileCase("package-before-reference", main,
            new() {
                ["P.pas"] = Unit(Package(), "uses Q;\n"),
                ["Q.pas"] = Reference(v2) + "unit Q; interface implementation end."
            }, null, false, "TestRoot", ".dll");
        await CompileCase("references-in-different-units", main,
            new() {
                ["P.pas"] = Unit(Reference(v1), "uses Q;\n"),
                ["Q.pas"] = Reference(v2) + "unit Q; interface implementation end."
            }, null, false, "TestRoot", ".dll");
        await CompileCase("identical-reference-and-package", Reference(v1) + main,
            new() { ["P.pas"] = Unit(Package()) }, "43", pcu: true);

        string transitive = Path.Combine(direct, "transitive");
        Directory.CreateDirectory(transitive);
        File.Copy(Path.Combine(v1, "TestRoot.dll"), Path.Combine(transitive, "TestRoot.dll"));
        File.Copy(Path.Combine(AppContext.BaseDirectory, "fixtures", "v2", "lib", "TestDependency.dll"),
            Path.Combine(transitive, "TestDependency.dll"));
        await CompileCase("reference-transitive-dependency-before-package", Reference(transitive) + main,
            new() { ["P.pas"] = Unit(Package(DependencyId)) }, null, false,
            "TestDependency", ".dll");
        await CompileCase("package-before-reference-transitive-dependency", main,
            new() {
                ["P.pas"] = Unit(Package(DependencyId), "uses Q;\n"),
                ["Q.pas"] = Reference(transitive) + "unit Q; interface implementation end."
            }, null, false, "TestDependency", ".dll");

        string pcuMain = Path.Combine(pcuCase, "Main.pas");
        string pcuOutput = Path.Combine(pcuCase, "output");
        string pcuExecutable = Path.Combine(pcuOutput, "Main.exe");
        string published = PackagePublisher.Publish(pcuExecutable, pcuMain, compilerDirectory);
        string offlineCache = cache + ".offline";
        Directory.Move(cache, offlineCache);
        try
        {
            var run = await RunDotnet(Path.Combine(published, "Main.exe"), published);
            Check(run.ExitCode == 0 && run.Output.Trim() == "43",
                "A published module-only package needs the NuGet cache: " + run.Output + run.Error);
        }
        finally { Directory.Move(offlineCache, cache); }
        Console.WriteLine("PASS: module packages: published program runs with its temporary NuGet cache unavailable");
        string previousExecutable = Hash(pcuExecutable);
        File.WriteAllText(pcuMain, Package(RootId, "1.1.0") + main);
        var pcuConflict = await RunCompiler(compiler, pcuMain, cache, pcuOutput);
        string pcuDiagnostic = pcuConflict.Output + pcuConflict.Error;
        Check(pcuConflict.ExitCode != 0 && pcuDiagnostic.Contains("P.pcu", StringComparison.OrdinalIgnoreCase) &&
              pcuDiagnostic.Contains("1.0.0") && pcuDiagnostic.Contains("1.1.0"),
            "PCU package conflict was not diagnosed before reading types: " + pcuDiagnostic);
        Check(Hash(pcuExecutable) == previousExecutable, "A failed PCU compilation overwrote the last executable");
        File.WriteAllText(pcuMain, Reference(v2) + main);
        var pcuReferenceConflict = await RunCompiler(compiler, pcuMain, cache, pcuOutput);
        Check(pcuReferenceConflict.ExitCode != 0 &&
              (pcuReferenceConflict.Output + pcuReferenceConflict.Error).Contains("TestRoot"),
            "A direct reference did not conflict with a package declared only in PCU: " +
            pcuReferenceConflict.Output + pcuReferenceConflict.Error);
        Console.WriteLine("PASS: module packages: conflicts with PCU-only requirements do not overwrite the last executable");

        string persistent = Path.Combine(directory, "persistent");
        Directory.CreateDirectory(persistent);
        string persistentMain = Path.Combine(persistent, "Main.pas");
        string persistentUnit = Path.Combine(persistent, "P.pas");
        string? previousCache = Environment.GetEnvironmentVariable("NUGET_PACKAGES");
        try
        {
            Environment.SetEnvironmentVariable("NUGET_PACKAGES", cache);
            Languages.Pascal.PascalLanguageRegistration.RegisterPascalLanguage();
            CodeCompletion.CodeCompletionBootstrap.Initialize("en");
            PascalABCCompiler.StringResources.ResDirectoryName = Path.Combine(compilerDirectory, "Lng", "Eng");
            var instance = new PascalABCCompiler.Compiler();
            int restores = 0;
            instance.OnChangeCompilerState += (_, state, _) => {
                if (state == PascalABCCompiler.CompilerState.NuGetRestoreStarting) restores++;
            };
            async Task CompilePersistent(string header, string version, bool success, string expected)
            {
                File.WriteAllText(persistentMain, header + main);
                File.WriteAllText(persistentUnit, Unit(Package(RootId, version)));
                restores = 0;
                var options = new PascalABCCompiler.CompilerOptions(persistentMain,
                    PascalABCCompiler.CompilerOptions.OutputType.ConsoleApplicaton) {
                    SystemDirectory = compilerDirectory, OutputDirectory = persistent,
                    SavePCU = false
                };
                options.StandardDirectories["%PABCSYSTEM%"] = compilerDirectory;
                string? executable = instance.Compile(options);
                string errors = string.Join("; ", instance.ErrorsList);
                Check((executable != null && instance.ErrorsList.Count == 0) == success,
                    "Persistent compiler: " + errors);
                Check(restores == 1, "Repeated identical declarations must restore only once per compilation, got " + restores);
                if (success)
                {
                    var run = await RunDotnet(executable!, persistent);
                    Check(run.ExitCode == 0 && run.Output.Trim() == expected,
                        "Persistent compiler runtime: " + run.Output + run.Error);
                }
                else Check(errors.Contains("1.0.0") && errors.Contains("1.1.0"), "Expected version conflict: " + errors);
            }
            HiddenRestoreDirectories.Add(service.GetConfiguration(new PackageRestoreRequest {
                ProgramFilePath = persistentMain, Packages = Array.Empty<PackageRequirement>() }).RestoreDirectory);
            await CompilePersistent(Package(), "1.0.0", true, "43");
            await CompilePersistent(Package(), "1.1.0", false, "");
            await CompilePersistent("", "1.1.0", true, "44");
            await CompilePersistent("", "1.0.0", true, "43");
            File.WriteAllText(persistentUnit, Unit(Package(compatible) + Package(newer)));
            Check(instance.Compile() != null && instance.ErrorsList.Count == 0,
                "Compatible batch failed: " + string.Join("; ", instance.ErrorsList));
            Check(instance.Warnings.Any(w => w.Message.Contains("NuGet: dependency '") &&
                    w.Message.Contains("1.1.0")),
                "Compatible version resolution must produce a warning: " + string.Join("; ", instance.Warnings.Select(w => w.Message)));
            File.WriteAllText(persistentMain, "begin Println(7); end.");
            restores = 0;
            string? withoutPackage = instance.Compile();
            Check(withoutPackage != null && instance.ErrorsList.Count == 0 && restores == 0,
                "Removing the unit did not clear package requirements: " + string.Join("; ", instance.ErrorsList));
            Check(!File.Exists(Path.ChangeExtension(withoutPackage, ".deps.json")),
                "Removing the package left its runtime manifest behind");
            Console.WriteLine("PASS: module packages: one compiler, duplicate declarations, failure recovery and version switching");
        }
        finally { Environment.SetEnvironmentVariable("NUGET_PACKAGES", previousCache); }

        Console.WriteLine("PASS: module packages, transitive graph, explicit references, PCU-only builds and execution; local feed only.");
    }
}
