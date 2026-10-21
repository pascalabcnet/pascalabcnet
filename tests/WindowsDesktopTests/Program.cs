using System.Diagnostics;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.Json;
using PascalABCCompiler.NetHelper;

System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

if (!OperatingSystem.IsWindows()) { Console.WriteLine("SKIP: Windows only"); return 0; }
if (args.Length < 1 || args.Length > 2) { Console.Error.WriteLine("WindowsDesktopTests <runtime-directory> [--graphics]"); return 2; }
string runtime = Path.GetFullPath(args[0]);
string work = Directory.CreateTempSubdirectory("pabc-windows-desktop-").FullName;
Console.WriteLine("Test files: " + work);
try
{
    Check(!NetCoreSystemReferences.AssemblyPaths.Any(IsDesktop), "implicit references stay Core-only");
    Check(NetCoreSystemReferences.GetPlatformAssemblyPaths("core").Count == 0, "Core adds no desktop references");
    Check(!AppDomain.CurrentDomain.GetAssemblies().Any(a => !a.IsDynamic && IsDesktop(a.Location)), "desktop is not loaded at startup");
    var finder = typeof(NetCoreSystemReferences).GetMethod("FindWindowsDesktopDirectory",
        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!;
    string fakeShared = Path.Combine(work, "shared");
    string fakeCore = Path.Combine(fakeShared, "Microsoft.NETCore.App", "10.0.13");
    Directory.CreateDirectory(fakeCore);
    foreach (string version in new[] { "9.0.99", "10.0.12", "10.0.14", "11.0.1" })
        Directory.CreateDirectory(Path.Combine(fakeShared, "Microsoft.WindowsDesktop.App", version));
    Check((string?)finder.Invoke(null, new object[] { fakeCore }) == Path.Combine(fakeShared, "Microsoft.WindowsDesktop.App", "10.0.14"),
        "desktop patch selection stays within runtime family");
    Check(finder.Invoke(null, new object[] { Path.Combine(fakeShared, "Microsoft.NETCore.App", "8.0.1") }) == null,
        "missing compatible desktop is not replaced with another major version");
    foreach (string platform in new[] { "WinForms", "WPF" })
    {
        var paths = NetCoreSystemReferences.GetPlatformAssemblyPaths(platform);
        Check(paths.Any(IsDesktop), platform + " loads Desktop on demand");
        Check(paths.Select(Path.GetFileName).Distinct(StringComparer.OrdinalIgnoreCase).Count() == paths.Count,
            platform + " has no duplicate assembly names");
        Check(paths.All(File.Exists), platform + " dependency closure exists");
    }
    foreach (string name in new[] { "core", "windows_core", "forms", "wpf", "wpf_console", "forms_reference", "graphabc", "formsabc", "graphwpf" })
    {
        string source = Path.Combine(work, "test_" + name + ".pas");
        File.Copy(Path.Combine(AppContext.BaseDirectory, "Sources", name + ".pas"), source);
        var result = await Run(Path.Combine(runtime, "pabcnetcclear.exe"), new[] { source }, runtime);
        string exe = Path.ChangeExtension(source, ".exe");
        Check(result.ExitCode == 0 && File.Exists(exe), name + " compiles", result.Text);
        bool module = name is "graphabc" or "formsabc" or "graphwpf";
        if (!module)
        {
            using var config = JsonDocument.Parse(File.ReadAllText(Path.ChangeExtension(exe, ".runtimeconfig.json")));
            string framework = config.RootElement.GetProperty("runtimeOptions").GetProperty("framework").GetProperty("name").GetString()!;
            Check(framework == (name is "core" or "windows_core" ? "Microsoft.NETCore.App" : "Microsoft.WindowsDesktop.App"), name + " runtimeconfig");
            result = await Run("dotnet", new[] { exe }, work);
            Check(result.ExitCode == 0 && result.Text.Contains("DESKTOP_OK"), name + " executes", result.Text);
        }
        else
        {
            Check(File.ReadAllText(Path.ChangeExtension(exe, ".runtimeconfig.json")).Contains("Microsoft.WindowsDesktop.App"), name + " inherits desktop from unit");
            result = await Run(Path.Combine(runtime, "pabcnetcclear.exe"), new[] { source }, runtime);
            Check(result.ExitCode == 0, name + " compiles using cached PCU", result.Text);
            Check(File.ReadAllText(Path.ChangeExtension(exe, ".runtimeconfig.json")).Contains("Microsoft.WindowsDesktop.App"), name + " preserves desktop after PCU load");
            if (args.Contains("--graphics"))
            {
                result = await Run("dotnet", new[] { exe }, work);
                Check(result.ExitCode == 0 && result.Text.Contains("DESKTOP_OK"), name + " draws/creates controls and exits", result.Text);
            }
        }
        if (name == "core")
        {
            using var stream = File.OpenRead(exe);
            using var pe = new PEReader(stream);
            var reader = pe.GetMetadataReader();
            Check(!reader.AssemblyReferences.Any(h => reader.GetString(reader.GetAssemblyReference(h).Name)
                is "System.Windows.Forms" or "PresentationFramework" or "System.IO.Packaging"), "console binary has no Desktop references");
        }
    }
    Check(!Directory.GetFiles(work, "*.dll").Any(path => NetCoreSystemReferences.IsRuntimeAssembly(Path.GetFileName(path))),
        "framework DLLs are not copied beside user programs");
    var drawing = PascalABCCompiler.NetHelper.NetHelper.LoadAssembly(NetCoreSystemReferences.ResolveAssemblyPath("System.Drawing.Primitives.dll"));
    Check(drawing.GetType("System.Drawing.Point") == typeof(System.Drawing.Point), "Drawing.Point uses canonical core type identity");
    var formsAssembly = PascalABCCompiler.NetHelper.NetHelper.LoadAssembly(NetCoreSystemReferences.ResolveAssemblyPath("System.Windows.Forms.dll"));
    var desktopContext = System.Runtime.Loader.AssemblyLoadContext.GetLoadContext(formsAssembly)!;
    Check(!desktopContext.IsCollectible, "desktop platform assemblies are permanent");
    var commonAssembly = PascalABCCompiler.NetHelper.NetHelper.LoadAssembly(NetCoreSystemReferences.ResolveAssemblyPath("System.Drawing.Common.dll"));
    Check(System.Runtime.Loader.AssemblyLoadContext.GetLoadContext(commonAssembly) == desktopContext,
        "WinForms and Drawing share one platform context");
    var hostProtectedData = System.Reflection.Assembly.Load("System.Security.Cryptography.ProtectedData");
    var desktopProtectedData = NetCoreSystemReferences.LoadRuntimeAssembly(NetCoreSystemReferences.ResolveAssemblyPath("System.Security.Cryptography.ProtectedData.dll"));
    Check(System.Runtime.Loader.AssemblyLoadContext.GetLoadContext(hostProtectedData) != System.Runtime.Loader.AssemblyLoadContext.GetLoadContext(desktopProtectedData),
        "desktop dependencies are isolated from the NuGet host dependencies");

    string collisionDir = Path.Combine(work, "collision");
    Directory.CreateDirectory(collisionDir);
    var fakeAssembly = new System.Reflection.Emit.PersistedAssemblyBuilder(
        new System.Reflection.AssemblyName("System.Drawing.Common") { Version = new Version(1, 0, 0, 0) }, typeof(object).Assembly);
    fakeAssembly.DefineDynamicModule("Fixture").DefineType("FakeDrawing").CreateType();
    fakeAssembly.Save(Path.Combine(collisionDir, "System.Drawing.Common.dll"));
    string collisionSource = Path.Combine(work, "collision.pas");
    File.WriteAllText(collisionSource, "{$reference 'collision/System.Drawing.Common.dll'}\n{$platform WinForms}\nbegin end.");
    var collision = await Run(Path.Combine(runtime, "pabcnetcclear.exe"), new[] { collisionSource }, runtime);
    Check(collision.ExitCode != 0 && collision.Text.Contains("System.Drawing.Common.dll") && !File.Exists(Path.ChangeExtension(collisionSource, ".exe")),
        "explicit conflicting reference is not hidden by platform selection", collision.Text);
    var languageService = new PascalABCNet.LanguageServices.PascalLanguageService("en");
    foreach (string name in new[] { "forms", "wpf" })
    {
        string source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Sources", name + ".pas"));
        string path = Path.Combine(work, "intellisense_" + name + ".pas");
        string id = new Uri(path).AbsoluteUri;
        var analysis = await languageService.OpenOrUpdateDocumentAsync(id, path, source, version: 1);
        Check(analysis.IsCompiled && analysis.LastConversionError == null, name + " IntelliSense analysis", analysis.LastConversionError?.ToString() ?? "");
        string marker = name == "forms" ? "f.Text" : "b.Content";
        int offset = source.IndexOf(marker, StringComparison.Ordinal) + 2;
        var symbols = await languageService.GetCompletionAfterDotAsync(id, offset);
        Check(symbols.Any(s => s.Label == (name == "forms" ? "Text" : "Content")), name + " IntelliSense properties");
    }
    var compiler = new PascalABCCompiler.Compiler();
    foreach (string name in new[] { "forms", "wpf", "core", "forms", "core" })
    {
        compiler.Reload();
        string source = Path.Combine(work, "test_" + name + ".pas");
        var options = new PascalABCCompiler.CompilerOptions(source, PascalABCCompiler.CompilerOptions.OutputType.ConsoleApplicaton)
            { UseDllForSystemUnits = false, Debug = false };
        Check(compiler.Compile(options) != null, "one compiler: " + name,
            string.Join("\n", compiler.ErrorsList.Select(e => e.ToString())));
        string config = File.ReadAllText(Path.ChangeExtension(source, ".runtimeconfig.json"));
        Check(config.Contains(name == "core" ? "Microsoft.NETCore.App" : "Microsoft.WindowsDesktop.App"),
            "one compiler selects correct framework: " + name);
    }
    Console.WriteLine("All WindowsDesktop checks passed.");
    return 0;
}
catch (Exception ex) { Console.Error.WriteLine(ex); return 1; }

static bool IsDesktop(string path) => path.Contains("Microsoft.WindowsDesktop.App", StringComparison.OrdinalIgnoreCase);
static void Check(bool condition, string message, string details = "")
{
    if (!condition) throw new Exception("FAIL: " + message + "\n" + details);
    Console.WriteLine("PASS: " + message);
}
static async Task<(int ExitCode, string Text)> Run(string command, string[] arguments, string directory)
{
    var start = new ProcessStartInfo(command) { WorkingDirectory = directory, UseShellExecute = false,
        RedirectStandardOutput = true, RedirectStandardError = true, CreateNoWindow = true,
        StandardOutputEncoding = System.Text.Encoding.GetEncoding(866), StandardErrorEncoding = System.Text.Encoding.UTF8 };
    foreach (string arg in arguments) start.ArgumentList.Add(arg);
    using var process = Process.Start(start)!;
    var output = process.StandardOutput.ReadToEndAsync();
    var error = process.StandardError.ReadToEndAsync();
    try { await process.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(120)); }
    catch { if (!process.HasExited) process.Kill(entireProcessTree: true); throw; }
    return (process.ExitCode, "exit=" + process.ExitCode + "\n" + await output + "\n" + await error);
}
