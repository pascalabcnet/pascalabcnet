using PascalABCNet.LanguageServices;
using PascalABCNet.PackageServices;

internal static partial class Program
{
    private static async Task CheckModuleIntelliSense(string work, bool runLsp = true)
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
        string kit = Environment.GetEnvironmentVariable("PABC_NUGET_TEST_COMPILER_DIR") ?? Path.Combine(repository, "bin-net10");
        string directory = Path.Combine(work, "module-intellisense");
        Directory.CreateDirectory(directory);
        string cache = Path.Combine(directory, "packages");
        WriteConfiguration(Path.Combine(directory, "NuGet.Config"), Path.Combine(work, "feed"), cache);
        PascalABCCompiler.StringResources.ResDirectoryName = Path.Combine(kit, "Lng", "Eng");
        string Package(string version = "1.0.0") => "{$package " + RootId + " " + version + "}\n";
        string Unit(string header = "", string uses = "") => header + "unit P; interface\n" + uses +
            "function GetValue: integer; implementation\nfunction GetValue: integer; begin Result := 1; end; end.";
        string Query(string uses = "uses P;") => uses + "\nbegin\n" +
            "  Println(NuGetRestoreFixture.Root.Value);\n" +
            "  Println(NuGetRestoreFixture.Dependency.Value);\n" +
            "  Println(NuGetRestoreFixture.Root.Combine(1,2));\nend.";

        string main = Path.Combine(directory, "Main.pas");
        string p = Path.Combine(directory, "P.pas");
        string q = Path.Combine(directory, "Q.pas");
        string uri = new Uri(main).AbsoluteUri;
        string qUri = new Uri(q).AbsoluteUri;
        string source = Query();
        File.WriteAllText(main, source);
        File.WriteAllText(p, Unit(Package()));
        var service = new PascalLanguageService("en", Path.Combine(kit, "Lib"));
        HiddenRestoreDirectories.Add(new PackageRestoreService().GetConfiguration(new PackageRestoreRequest {
            ProgramFilePath = main, Packages = Array.Empty<PackageRequirement>() }).RestoreDirectory);
        HiddenRestoreDirectories.Add(new PackageRestoreService().GetConfiguration(new PackageRestoreRequest {
            ProgramFilePath = q, Packages = Array.Empty<PackageRequirement>() }).RestoreDirectory);

        async Task Analyze(string text, bool expected = true)
        {
            var analysis = await service.OpenOrUpdateDocumentAsync(uri, main, text);
            Check(analysis.IsCompiled == expected, "Module IntelliSense analysis: " + analysis.LastConversionError);
        }
        int Dot(string text) => text.IndexOf("Root.Value", StringComparison.Ordinal) + "Root.".Length;
        async Task CheckVersion(string text, bool version2, bool present = true)
        {
            var completion = await service.GetCompletionAfterDotAsync(uri, Dot(text));
            Check(completion.Any(item => item.Label == "Value") == present,
                "Module package visibility does not match the current graph.");
            Check(completion.Any(item => item.Label == "OnlyInV2") == (present && version2),
                "Module package DLL generation is stale.");
        }
        await Analyze(source);
        await CheckVersion(source, false);
        var hover = await service.GetHoverAsync(uri, Dot(source) + 1);
        Check(hover?.Contents.Contains("Value") == true, "Hover cannot see a package declared only by a module.");
        int signatureOffset = source.IndexOf("Root.Combine(", StringComparison.Ordinal) + "Root.Combine(".Length;
        var signature = await service.GetSignatureHelpAsync(uri, signatureOffset, '(');
        Check(signature?.Signatures.Any(item => item.Label.Contains("Combine")) == true,
            "Signature help cannot see a package declared only by a module.");
        int dependencyDot = source.IndexOf("Dependency.Value", StringComparison.Ordinal) + "Dependency.".Length;
        Check((await service.GetCompletionAfterDotAsync(uri, dependencyDot)).Any(item => item.Label == "Value"),
            "A transitive package is absent from IntelliSense.");
        Console.WriteLine("PASS: module IntelliSense: completion, hover, signature help and transitive package");

        File.WriteAllText(p, Unit("", "uses Q;\n"));
        string qText = Package() + "unit Q; interface implementation end.";
        File.WriteAllText(q, qText);
        await Analyze(source);
        await CheckVersion(source, false);
        string unsaved = qText.Replace("1.0.0}", "1.1.0}");
        var qAnalysis = await service.OpenOrUpdateDocumentAsync(qUri, q, unsaved, 1);
        Check(qAnalysis.IsCompiled, "Unsaved module failed: " + qAnalysis.LastConversionError);
        Check(File.ReadAllText(q) == qText, "The test overwrote the user's module snapshot on disk.");
        await CheckVersion(source, true);
        await service.CloseDocumentAsync(qUri);
        await CheckVersion(source, false);
        Console.WriteLine("PASS: module IntelliSense: nested uses, unsaved module and didClose fallback to disk");

        await service.OpenOrUpdateDocumentAsync(qUri, q, qText.Replace("1.0.0}", "9.9.9}"), 2);
        await CheckVersion(source, false, present: false);
        await service.OpenOrUpdateDocumentAsync(qUri, q, qText, 3);
        await CheckVersion(source, false);
        await service.OpenOrUpdateDocumentAsync(qUri, q, qText.Replace(Package(), ""), 4);
        await CheckVersion(source, false, present: false);
        await service.OpenOrUpdateDocumentAsync(qUri, q, qText, 5);
        await CheckVersion(source, false);
        Console.WriteLine("PASS: module IntelliSense: missing package, removal and recovery do not retain stale symbols");

        await Analyze(Package("1.1.0") + source, expected: false);
        await CheckVersion(Package("1.1.0") + source, true, present: false);
        await Analyze(Package() + source);
        await CheckVersion(Package() + source, false);
        await service.CloseDocumentAsync(qUri);
        File.WriteAllText(q, qText.Replace("1.0.0}", "1.1.0}"));
        File.SetLastWriteTimeUtc(q, DateTime.UtcNow.AddSeconds(5));
        await Analyze(source);
        await CheckVersion(source, true);
        File.WriteAllText(q, qText);
        File.SetLastWriteTimeUtc(q, DateTime.UtcNow.AddSeconds(10));
        await CheckVersion(source, false);
        Console.WriteLine("PASS: module IntelliSense: version conflicts and disk edits without a main-file edit");

        // A newly encountered module must not silently replace a transitive
        // version already selected by the main program.
        const string upgrade = "PascalABC.TestIntelliSenseUpgrade";
        var builder = new NuGet.Packaging.PackageBuilder {
            Id = upgrade, Version = NuGet.Versioning.NuGetVersion.Parse("1.0.0"),
            Description = "Local module package graph test"
        };
        builder.Authors.Add("PascalABC.NET tests");
        builder.DependencyGroups.Add(new NuGet.Packaging.PackageDependencyGroup(
            NuGet.Frameworks.NuGetFramework.ParseFolder("net10.0"),
            new[] { new NuGet.Packaging.Core.PackageDependency(RootId, NuGet.Versioning.VersionRange.Parse("[1.1.0]")) }));
        using (var stream = File.Create(Path.Combine(work, "feed", upgrade + ".1.0.0.nupkg"))) builder.Save(stream);
        File.WriteAllText(q, "{$package " + upgrade + " 1.0.0}\nunit Q; interface implementation end.");
        await Analyze(Package() + source, expected: false);
        await CheckVersion(Package() + source, false, present: false);
        // Repeated compatible requirements can be resolved without replacement.
        string combined = "{$package " + upgrade + " 1.0.0}\n" + source;
        await Analyze(combined);
        await CheckVersion(combined, true);
        File.WriteAllText(q, qText);
        await Analyze(source);
        await CheckVersion(source, false);
        Console.WriteLine("PASS: module IntelliSense: transitive version replacement is rejected and analysis recovers");

        Directory.CreateDirectory(Path.Combine(directory, "sub"));
        File.WriteAllText(Path.Combine(directory, "sub", "R.pas"), Package() + "unit R; interface implementation end.");
        string located = Query("uses R in 'sub/R.pas';");
        await Analyze(located);
        await CheckVersion(located, false);
        string virtualFile = Path.Combine(directory, "UnsavedUnit.pas");
        string virtualUri = new Uri(virtualFile).AbsoluteUri;
        await service.OpenOrUpdateDocumentAsync(virtualUri, virtualFile,
            Package("1.1.0") + "unit UnsavedUnit; interface implementation end.", 1);
        string virtualMain = Query("uses UnsavedUnit;");
        await Analyze(virtualMain);
        await CheckVersion(virtualMain, true);
        await service.CloseDocumentAsync(virtualUri);
        Console.WriteLine("PASS: module IntelliSense: uses in another directory and a never-saved module");

        File.WriteAllText(p, Unit().Replace("implementation\n", "implementation uses Q;\n"));
        await Analyze(source);
        await CheckVersion(source, false);
        File.WriteAllText(p, Unit("", "uses Q;\n"));
        File.WriteAllText(q, qText.Replace("implementation", "implementation uses P;"));
        await Analyze(source);
        await CheckVersion(source, false);
        Console.WriteLine("PASS: module IntelliSense: implementation uses and a permitted unit cycle");

        string direct = Path.Combine(directory, "direct");
        Directory.CreateDirectory(direct);
        File.Copy(Path.Combine(AppContext.BaseDirectory, "fixtures", "v2", "lib", "TestRoot.dll"), Path.Combine(direct, "TestRoot.dll"));
        File.Copy(Path.Combine(AppContext.BaseDirectory, "fixtures", "lib", "TestDependency.dll"), Path.Combine(direct, "TestDependency.dll"));
        string conflict = "{$reference '" + Path.Combine(direct, "TestRoot.dll") + "'}\n" + source;
        await Analyze(conflict, expected: false);
        await CheckVersion(conflict, true, present: false);
        await Analyze(source);
        await CheckVersion(source, false);
        Console.WriteLine("PASS: module IntelliSense: package/reference conflict and recovery");

        // Different programs can legitimately select different versions of the
        // same package while sharing the source of a package-free Pascal unit.
        File.WriteAllText(p, Unit());
        string first = Package() + source;
        string second = Package("1.1.0") + source;
        await Analyze(first);
        string secondFile = Path.Combine(directory, "Second.pas");
        string secondUri = new Uri(secondFile).AbsoluteUri;
        File.WriteAllText(secondFile, second);
        HiddenRestoreDirectories.Add(new PackageRestoreService().GetConfiguration(new PackageRestoreRequest {
            ProgramFilePath = secondFile, Packages = Array.Empty<PackageRequirement>() }).RestoreDirectory);
        await service.OpenOrUpdateDocumentAsync(secondUri, secondFile, second, 1);
        await CheckVersion(first, false);
        Check((await service.GetCompletionAfterDotAsync(secondUri, Dot(second))).Any(item => item.Label == "OnlyInV2"),
            "A shared Pascal module pinned another document's package version.");
        await CheckVersion(first, false);
        Console.WriteLine("PASS: module IntelliSense: separate package graphs for programs sharing a Pascal module");

        string pcuDirectory = Path.Combine(directory, "pcu");
        Directory.CreateDirectory(pcuDirectory);
        string pcuSource = Path.Combine(pcuDirectory, "P.pas");
        string pcuMain = Path.Combine(pcuDirectory, "PcuMain.pas");
        string pcuText = Query().Replace("end.", "Println(GetValue); end.");
        File.WriteAllText(pcuSource, Unit(Package()));
        File.WriteAllText(pcuMain, pcuText);
        HiddenRestoreDirectories.Add(new PackageRestoreService().GetConfiguration(new PackageRestoreRequest {
            ProgramFilePath = pcuMain, Packages = Array.Empty<PackageRequirement>() }).RestoreDirectory);
        var build = await RunCompiler(Path.Combine(kit, "pabcnetcclear.exe"), pcuMain, cache);
        Check(build.ExitCode == 0 && File.Exists(Path.ChangeExtension(pcuSource, ".pcu")),
            "PCU setup failed: " + build.Output + build.Error);
        File.Move(pcuSource, pcuSource + ".hidden");
        string pcuUri = new Uri(pcuMain).AbsoluteUri;
        var pcuAnalysis = await service.OpenOrUpdateDocumentAsync(pcuUri, pcuMain, pcuText);
        Check(pcuAnalysis.IsCompiled, "PCU-only package analysis failed: " + pcuAnalysis.LastConversionError);
        Check((await service.GetCompletionAfterDotAsync(pcuUri, Dot(pcuText))).Any(item => item.Label == "Value"),
            "A package declared only in a PCU is absent from completion.");
        Check((await service.GetHoverAsync(pcuUri, pcuText.IndexOf("GetValue", StringComparison.Ordinal) + 2))
                ?.Contents.Contains("GetValue") == true,
            "The PCU model was not actually imported.");
        Console.WriteLine("PASS: module IntelliSense: package and exported member from a PCU with no source file");
        if (runLsp) await CheckModuleIntelliSenseLsp(work, kit);
    }

    private static async Task CheckModuleIntelliSenseLsp(string work, string kit)
    {
        string directory = Path.Combine(work, "module-intellisense-lsp");
        Directory.CreateDirectory(directory);
        WriteConfiguration(Path.Combine(directory, "NuGet.Config"), Path.Combine(work, "feed"), Path.Combine(directory, "packages"));
        string module = Path.Combine(directory, "LspUnit.pas");
        string moduleText = "{$package " + RootId + " 1.0.0}\nunit LspUnit; interface implementation end.";
        string main = Path.Combine(directory, "Main.pas");
        string mainText = "uses LspUnit; begin Println(NuGetRestoreFixture.Root.Value);\n" +
            "Println(NuGetRestoreFixture.Root.Combine(1,2)); end.";
        File.WriteAllText(module, moduleText);
        File.WriteAllText(main, mainText);
        foreach (string file in new[] { module, main })
            HiddenRestoreDirectories.Add(new PackageRestoreService().GetConfiguration(new PackageRestoreRequest {
                ProgramFilePath = file, Packages = Array.Empty<PackageRequirement>() }).RestoreDirectory);
        using var server = new System.Diagnostics.Process { StartInfo = new System.Diagnostics.ProcessStartInfo("dotnet") {
            WorkingDirectory = kit, UseShellExecute = false, CreateNoWindow = true,
            RedirectStandardInput = true, RedirectStandardOutput = true, RedirectStandardError = true
        } };
        server.StartInfo.ArgumentList.Add(Path.Combine(kit, "PascalABCNet.LanguageServer.dll"));
        server.StartInfo.ArgumentList.Add("--stdio");
        server.Start();
        var errors = server.StandardError.ReadToEndAsync();
        try
        {
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(90));
            var input = server.StandardInput.BaseStream;
            var output = server.StandardOutput.BaseStream;
            string uri = new Uri(main).AbsoluteUri;
            string moduleUri = new Uri(module).AbsoluteUri;
            int request = 1;
            await WriteLspAsync(input, new { jsonrpc = "2.0", id = request, method = "initialize",
                @params = new { processId = Environment.ProcessId, rootUri = new Uri(directory + Path.DirectorySeparatorChar).AbsoluteUri,
                    capabilities = new { } } }, timeout.Token);
            using (await ReadLspResponseAsync(output, request++, timeout.Token)) { }
            await WriteLspAsync(input, new { jsonrpc = "2.0", method = "initialized", @params = new { } }, timeout.Token);
            async Task Open(string id, string text) => await WriteLspAsync(input, new { jsonrpc = "2.0", method = "textDocument/didOpen",
                @params = new { textDocument = new { uri = id, languageId = "pascal", version = 1, text } } }, timeout.Token);
            async Task Change(string text, int version) => await WriteLspAsync(input, new { jsonrpc = "2.0", method = "textDocument/didChange",
                @params = new { textDocument = new { uri = moduleUri, version }, contentChanges = new[] { new { text } } } }, timeout.Token);
            int dot = mainText.IndexOf("Root.Value", StringComparison.Ordinal) + "Root.".Length;
            async Task Expect(bool newer, bool present = true)
            {
                using var response = await RequestLspPositionAsync(input, output, request++, "textDocument/completion", uri, mainText, dot, timeout.Token);
                var labels = response.RootElement.GetProperty("result").GetProperty("items").EnumerateArray()
                    .Select(item => item.GetProperty("label").GetString()).ToArray();
                Check(labels.Contains("Value") == present && labels.Contains("OnlyInV2") == (present && newer),
                    "LSP returned a stale module package graph: " + response.RootElement);
            }
            await Open(uri, mainText);
            await Expect(false);
            await Open(moduleUri, moduleText.Replace("1.0.0}", "1.1.0}"));
            await Expect(true);
            using (var hover = await RequestLspPositionAsync(input, output, request++, "textDocument/hover", uri, mainText, dot + 1, timeout.Token))
                Check(hover.RootElement.GetProperty("result").GetProperty("contents").GetProperty("value").GetString()!.Contains("Value"), "LSP module hover failed.");
            int signature = mainText.IndexOf("Root.Combine(", StringComparison.Ordinal) + "Root.Combine(".Length;
            using (var response = await RequestLspPositionAsync(input, output, request++, "textDocument/signatureHelp", uri, mainText, signature, timeout.Token))
                Check(response.RootElement.GetProperty("result").GetProperty("signatures").EnumerateArray()
                    .Any(item => item.GetProperty("label").GetString()!.Contains("Combine")), "LSP module signature help failed.");
            await Change(moduleText, 2);
            await Expect(false);
            await Change("unit LspUnit; interface implementation end.", 3);
            await Expect(false, present: false);
            await Change(moduleText, 4);
            await Expect(false);
            await WriteLspAsync(input, new { jsonrpc = "2.0", method = "textDocument/didClose", @params = new { textDocument = new { uri = moduleUri } } }, timeout.Token);
            File.WriteAllText(module, moduleText.Replace("1.0.0}", "1.1.0}"));
            File.SetLastWriteTimeUtc(module, DateTime.UtcNow.AddSeconds(5));
            await Expect(true);
            await WriteLspAsync(input, new { jsonrpc = "2.0", id = request, method = "shutdown" }, timeout.Token);
            using (await ReadLspResponseAsync(output, request++, timeout.Token)) { }
            await WriteLspAsync(input, new { jsonrpc = "2.0", method = "exit" }, timeout.Token);
            await server.WaitForExitAsync(timeout.Token);
            Check(server.ExitCode == 0, "Language server did not exit cleanly: " + await errors);
            Console.WriteLine("PASS: module IntelliSense over LSP: completion/hover/signature, didOpen/didChange/didClose, version removal and disk reload");
        }
        finally
        {
            if (!server.HasExited) { server.Kill(entireProcessTree: true); await server.WaitForExitAsync(); }
        }
    }
}
