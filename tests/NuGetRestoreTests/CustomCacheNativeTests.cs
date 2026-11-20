using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using PascalABCNet.PackageServices;

internal static partial class Program
{
    // Real packages, optional network test. The compiler and executable receive
    // no NUGET_PACKAGES override: only the source-local NuGet.Config selects cache.
    private static async Task CheckCustomCacheNative(string work)
    {
        if (!OperatingSystem.IsWindows() || RuntimeInformation.ProcessArchitecture != Architecture.X64)
            throw new PlatformNotSupportedException("Custom native cache regression requires Windows x64.");
        string originalOverride = Environment.GetEnvironmentVariable("NUGET_PACKAGES") ?? "";
        string directory = Path.Combine(work, "Пример с пробелами");
        string cache = Path.GetFullPath(Path.Combine(work, "Кеш NuGet"));
        string source = Path.Combine(directory, "CacheProbe.pas");
        Directory.CreateDirectory(directory);
        new XDocument(new XElement("configuration",
            new XElement("packageSources", new XElement("clear"),
                new XElement("add", new XAttribute("key", "nuget.org"),
                    new XAttribute("value", "https://api.nuget.org/v3/index.json"))),
            new XElement("config", new XElement("add", new XAttribute("key", "globalPackagesFolder"),
                new XAttribute("value", "../Кеш NuGet"))))).Save(Path.Combine(directory, "NuGet.Config"));
        File.WriteAllText(source, """
{$target windows}
{$package SharpAssimp 6.0.6}
uses System, System.Text, System.Diagnostics, SharpAssimp.Unmanaged;
begin
  if Environment.GetEnvironmentVariable('NUGET_PACKAGES') <> nil then
    raise new Exception('NUGET_PACKAGES must be absent');
  Writeln('ASSIMP=', AssimpLibrary.Instance.GetVersionMajor);
  foreach var m: ProcessModule in Process.GetCurrentProcess.Modules do
    if string.Equals(m.ModuleName, 'assimp.dll', StringComparison.OrdinalIgnoreCase) then
      Writeln('NATIVE64=', Convert.ToBase64String(Encoding.UTF8.GetBytes(m.FileName)));
end.
""");
        // Only locate cleanup metadata here. Do not restore packages in the parent
        // or use RunCompiler(), which explicitly overrides NUGET_PACKAGES.
        HiddenRestoreDirectories.Add(new PackageRestoreService().GetConfiguration(new PackageRestoreRequest {
            Packages = Array.Empty<PackageRequirement>(), ProgramFilePath = source,
            PackagesDirectory = cache, LocalPackagesOnly = true
        }).RestoreDirectory);
        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
        string kit = Environment.GetEnvironmentVariable("PABC_NUGET_TEST_COMPILER_DIR") ?? Path.Combine(repository, "bin-net10");
        string compiler = Path.Combine(kit, "pabcnetcclear.exe");
        Check(File.Exists(compiler), "Build the modern console compiler before --custom-cache-native.");
        string executable = Path.ChangeExtension(source, ".exe");
        var platform = PlatformPackSelector.Select(PlatformPackCatalogReader.ReadNet10(),
            "net10.0-windows10.0.19041.0").Packages.Single();
        string native = Path.Combine(cache, "sharpassimp", "6.0.6", "runtimes", "win-x64", "native", "assimp.dll");

        async Task<(int ExitCode, string Output, string Error)> WithoutOverride(string command, params string[] arguments)
        {
            var start = new ProcessStartInfo(command) {
                // Deliberately not the source directory: config discovery must
                // start beside the source, not at the compiler's working directory.
                WorkingDirectory = kit, UseShellExecute = false,
                RedirectStandardOutput = true, RedirectStandardError = true, CreateNoWindow = true
            };
            start.Environment.Remove("NUGET_PACKAGES");
            foreach (string argument in arguments) start.ArgumentList.Add(argument);
            using var process = Process.Start(start)!;
            var output = process.StandardOutput.ReadToEndAsync();
            var error = process.StandardError.ReadToEndAsync();
            try { await process.WaitForExitAsync().WaitAsync(TimeSpan.FromMinutes(3)); }
            catch { if (!process.HasExited) process.Kill(entireProcessTree: true); throw; }
            return (process.ExitCode, await output, await error);
        }

        for (int pass = 1; pass <= 2; pass++)
        {
            var compiled = await WithoutOverride(compiler, source);
            Check(compiled.ExitCode == 0, "Custom cache compilation failed: " + compiled.Output + compiled.Error);
            Check(File.Exists(native), "SharpAssimp was not downloaded into the config-selected cache.");
            Check(File.Exists(Path.Combine(cache, platform.RuntimePackageId.ToLowerInvariant(), platform.Version,
                "data", "RuntimeList.xml")), "Platform pack was not downloaded into the config-selected cache.");
            using (var config = JsonDocument.Parse(File.ReadAllText(Path.ChangeExtension(source, ".runtimeconfig.json"))))
            {
                var options = config.RootElement.GetProperty("runtimeOptions");
                Check(options.GetProperty("additionalProbingPaths").EnumerateArray().Any(p =>
                    string.Equals(p.GetString(), cache, StringComparison.OrdinalIgnoreCase)),
                    "Managed probing did not use the custom cache.");
                string[] paths = options.GetProperty("configProperties").GetProperty("PascalABCNET.NativeSearchPaths")
                    .GetString()!.Split(';');
                Check(paths.Contains(Path.GetDirectoryName(native)!, StringComparer.OrdinalIgnoreCase) &&
                    paths.All(p => p.StartsWith(cache + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)),
                    "Native search directories escaped the config-selected cache.");
            }
            using (var deps = JsonDocument.Parse(File.ReadAllText(Path.ChangeExtension(source, ".deps.json"))))
                Check(deps.RootElement.GetProperty("libraries").TryGetProperty(
                    platform.RuntimePackageId + "/" + platform.Version, out _), "Platform pack is absent from runtime manifest.");
            var run = await WithoutOverride("dotnet", executable);
            Check(run.ExitCode == 0 && run.Output.Contains("ASSIMP=6"),
                "Custom cache execution failed: " + run.Output + run.Error);
            string[] reported = run.Output.Split('\n').Select(s => s.Trim())
                .Where(s => s.StartsWith("NATIVE64=", StringComparison.Ordinal)).ToArray();
            Check(reported.Length == 1, "The actual native module path was not reported.");
            string loaded = Encoding.UTF8.GetString(Convert.FromBase64String(reported[0][9..]));
            Check(string.Equals(loaded, native, StringComparison.OrdinalIgnoreCase),
                "Assimp loaded from the wrong cache: " + loaded);
            Check(!Directory.GetFiles(directory, "*.dll").Any(), "DLLs were copied beside the program.");
            Console.WriteLine($"PASS: custom-cache compilation/execution {pass}; NuGet.Config, spaces and Cyrillic, no NUGET_PACKAGES or DLL copying.");
        }
        Check((Environment.GetEnvironmentVariable("NUGET_PACKAGES") ?? "") == originalOverride,
            "The test changed the parent process's package-cache environment.");
        Console.WriteLine("PASS: platform packs and real SharpAssimp use the config-selected custom cache; actual native module path verified.");
    }
}
