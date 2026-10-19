using PascalABCNet.LanguageServices;
using PascalABCNet.PackageServices;

internal static partial class Program
{
    private static async Task CheckManualIntelliSense(string work)
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        string kit = Environment.GetEnvironmentVariable("PABC_NUGET_TEST_COMPILER_DIR") ??
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../bin-net10"));
        string directory = Path.Combine(work, "manual-analysis");
        Directory.CreateDirectory(directory);
        string packages = Path.Combine(directory, "packages");
        WriteConfiguration(Path.Combine(directory, "NuGet.Config"), Path.Combine(work, "feed"), packages);
        string main = Path.Combine(directory, "Main.pas"), unit = Path.Combine(directory, "LocalUnit.pas");
        File.WriteAllText(unit, "{$package " + RootId + " 1.0.0}\nunit LocalUnit; interface implementation end.");
        const string source = "uses LocalUnit; begin Println(NuGetRestoreFixture.Root.Value); end.";
        string uri = new Uri(main).AbsoluteUri;
        var restore = new PackageRestoreService();
        var request = new PackageRestoreRequest { Packages = new[] { new PackageRequirement(RootId, "1.0.0") }, ProgramFilePath = main };
        HiddenRestoreDirectories.Add(restore.GetConfiguration(request).RestoreDirectory);
        PascalABCCompiler.StringResources.ResDirectoryName = Path.Combine(kit, "Lng", "Eng");
        var language = new PascalLanguageService("en", Path.Combine(kit, "Lib")) { ManualPackageRestore = true };
        bool needed = false;
        language.PackageRestoreRequiredChanged += (id, required) => { if (id == uri) needed = required; };
        await language.OpenOrUpdateDocumentAsync(uri, main, source);
        Check(needed && !Directory.Exists(packages), "Opening a file with a package in a module must not install it.");
        for (int i = 0; i < 3; i++)
            await language.OpenOrUpdateDocumentAsync(uri, main, source + new string(' ', i + 1));
        Check(needed && !Directory.Exists(packages), "Editing must not restore missing packages.");
        var result = await language.RestoreDocumentPackagesAsync(uri, CancellationToken.None);
        Check(result.Success && !needed && Directory.Exists(packages), "Explicit restore did not prepare module packages: " + result.Message);
        int dot = source.IndexOf("Root.Value", StringComparison.Ordinal) + "Root.".Length;
        Check((await language.GetCompletionAfterDotAsync(uri, dot)).Any(item => item.Label == "Value"),
            "Package completion did not refresh after explicit restore.");

        // Even with a live remote source, local-only resolution must not contact it.
        var listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, 0);
        listener.Start();
        try
        {
            string remote = "http://127.0.0.1:" + ((System.Net.IPEndPoint)listener.LocalEndpoint).Port + "/v3/index.json";
            var cached = await restore.RestoreAsync(request with { LocalPackagesOnly = true,
                Sources = new[] { remote }, RestoreDirectory = Path.Combine(directory, "local-only") });
            Check(cached.Packages.Count == 2 && cached.InstalledPackageCount == 0 && !listener.Pending(),
                "Local-only resolution must use cached root/transitive packages without network access.");
            var noSources = await restore.RestoreAsync(request with { LocalPackagesOnly = true,
                Sources = Array.Empty<string>(), RestoreDirectory = Path.Combine(directory, "no-feeds") });
            Check(noSources.Packages.Count == 2, "Ready local packages must work even with no enabled feed.");
            Directory.Move(packages, packages + ".saved");
            Directory.CreateDirectory(packages);
            try
            {
                await restore.RestoreAsync(request with { LocalPackagesOnly = true,
                    Sources = new[] { remote }, RestoreDirectory = Path.Combine(directory, "empty-local") });
                throw new Exception("An empty local cache was accepted.");
            }
            catch (PackageRestoreRequiredException) { }
            Check(!listener.Pending(), "A missing package triggered a remote request during local-only resolution.");
        }
        finally { listener.Stop(); }
        await language.OpenOrUpdateDocumentAsync(uri, main, source + "  ");
        Check(needed, "Deleting cached packages must bring back the restore action.");
        await restore.RestoreAsync(request); // Same automatic operation used by the compiler.
        var refresh = await language.PrepareDocumentPackagesAsync(uri, false, CancellationToken.None);
        Check(refresh.Success && !needed, "Packages restored outside IntelliSense were not picked up locally.");
        await language.OpenOrUpdateDocumentAsync(uri, main, "begin Println(1); end.");
        Check(!needed, "Removing package/module references must clear the restore action.");
        await language.CloseDocumentAsync(uri);
        Console.WriteLine("PASS: manual IntelliSense restore, module graph, offline cache and external restore refresh.");
    }
}
