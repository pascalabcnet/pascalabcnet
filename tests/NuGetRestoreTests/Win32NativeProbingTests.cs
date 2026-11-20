using System.Runtime.InteropServices;
using System.Text.Json;
using NuGet.Packaging;
using NuGet.Versioning;
using PascalABCNet.PackageServices;

internal static partial class Program
{
    private static async Task CheckSharpAssimpNative(string work)
    {
        if (!OperatingSystem.IsWindows() || RuntimeInformation.ProcessArchitecture != Architecture.X64)
            throw new PlatformNotSupportedException("SharpAssimp regression requires Windows x64.");
        string source = Path.Combine(work, "SharpAssimpProbe.pas");
        File.WriteAllText(source, """
{$package SharpAssimp 6.0.6}
uses System, System.Diagnostics, SharpAssimp.Unmanaged;
begin
  Writeln('ASSIMP=', AssimpLibrary.Instance.GetVersionMajor);
  foreach var m: ProcessModule in Process.GetCurrentProcess.Modules do
    if string.Equals(m.ModuleName, 'assimp.dll', StringComparison.OrdinalIgnoreCase) then
      Writeln('NATIVE=', m.FileName);
end.
""");
        var request = new PackageRestoreRequest {
            Packages = new[] { new PackageRequirement("SharpAssimp", "6.0.6") }, ProgramFilePath = source
        };
        var service = new PackageRestoreService();
        var configuration = service.GetConfiguration(request);
        HiddenRestoreDirectories.Add(configuration.RestoreDirectory);
        // Optional real-package test: uses configured sources/cache and may download.
        var graph = await service.RestoreAsync(request);
        string native = graph.Packages.SelectMany(p => p.NativeLibraries).Single(p =>
            Path.GetFileName(p).Equals("assimp.dll", StringComparison.OrdinalIgnoreCase));
        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
        string kit = Environment.GetEnvironmentVariable("PABC_NUGET_TEST_COMPILER_DIR") ?? Path.Combine(repository, "bin-net10");
        var compiled = await RunCompiler(Path.Combine(kit, "pabcnetcclear.exe"), source, configuration.PackagesDirectory);
        Check(compiled.ExitCode == 0, "SharpAssimp compilation failed: " + compiled.Output + compiled.Error);
        var run = await RunDotnet(Path.ChangeExtension(source, ".exe"), work);
        Check(run.ExitCode == 0 && run.Output.Contains("ASSIMP=6") &&
            run.Output.Split('\n').Any(line => line.Trim().Equals("NATIVE=" + native, StringComparison.OrdinalIgnoreCase)),
            "SharpAssimp must load its native implementation from the selected cache: " + run.Output + run.Error);
        Check(!Directory.GetFiles(work, "*.dll").Any(), "SharpAssimp DLLs were copied beside the program.");
        Console.WriteLine("PASS: real SharpAssimp 6.0.6 loads Assimp from cache without explicit loading or paths in Pascal code.");
    }

    private static async Task CheckWin32NativeProbing(string work)
    {
        if (!OperatingSystem.IsWindows() || RuntimeInformation.ProcessArchitecture != Architecture.X64)
            throw new PlatformNotSupportedException("Win32 native probing regression requires Windows x64.");
        string directory = Path.Combine(work, "win32 probing with spaces");
        string feed = Path.Combine(directory, "feed");
        string cache = Path.Combine(directory, "packages");
        Directory.CreateDirectory(feed);
        string source = Path.Combine(directory, "NativeProbe.pas");
        string originalPath = Environment.GetEnvironmentVariable("PATH") ?? "";
        WriteConfiguration(Path.Combine(directory, "NuGet.Config"), feed, cache);
        // Use an OS-supplied native PE under a unique package-only name. No C/C++
        // toolchain, internet, pre-existing Assimp installation or machine PATH needed.
        const string id = "PascalABC.Win32NativeProbe";
        const string dll = "pabc-native-probe.dll";
        var builder = new PackageBuilder { Id = id, Version = NuGetVersion.Parse("1.0.0"),
            Description = "Direct Win32 LoadLibrary and PInvoke from a local NuGet cache" };
        builder.Authors.Add("PascalABC.NET tests");
        builder.Files.Add(new PhysicalPackageFile {
            SourcePath = Path.Combine(Environment.SystemDirectory, "version.dll"),
            TargetPath = "runtimes/win-x64/native/" + dll });
        using (var stream = File.Create(Path.Combine(feed, id + ".1.0.0.nupkg"))) builder.Save(stream);
        File.WriteAllText(Path.Combine(directory, "NativeProbeUnit.pas"), """
unit NativeProbeUnit;
interface
implementation
uses System, System.Text;
function LoadLibraryA(name: string): IntPtr; external 'kernel32.dll' name 'LoadLibraryA';
function GetProcAddress(h: IntPtr; name: string): IntPtr; external 'kernel32.dll' name 'GetProcAddress';
function GetModuleFileNameA(h: IntPtr; name: StringBuilder; size: integer): integer;
  external 'kernel32.dll' name 'GetModuleFileNameA';
function FreeLibrary(h: IntPtr): boolean; external 'kernel32.dll' name 'FreeLibrary';
initialization
  var h := LoadLibraryA('pabc-native-probe.dll');
  if h = IntPtr.Zero then
    raise new Exception('LoadLibraryA did not find the package native DLL');
  var entry := GetProcAddress(h, 'GetFileVersionInfoSizeA');
  if entry = IntPtr.Zero then
    raise new Exception('GetProcAddress did not find GetFileVersionInfoSizeA');
  var name := new StringBuilder(4096);
  var length := GetModuleFileNameA(h, name, name.Capacity);
  if (length = 0) or (length >= name.Capacity) then
    raise new Exception('GetModuleFileNameA failed or truncated the native path');
  Writeln('NATIVE=', name.ToString);
  var freed := FreeLibrary(h);
  if not freed then
    raise new Exception('FreeLibrary failed');
end.
""");
        File.WriteAllText(source, "{$package " + id + " 1.0.0}\n" + """
uses NativeProbeUnit;
function InfoSize(name: string; var handle: cardinal): cardinal;
  external 'pabc-native-probe.dll' name 'GetFileVersionInfoSizeA';
begin
  var handle: cardinal := 0;
  var size := InfoSize('not-an-existing-file.pabc', handle);
  if size <> 0 then
    raise new System.Exception('PInvoke returned an unexpected version info size');
  Writeln('PINVOKE=OK');
end.
""");
        var service = new PackageRestoreService();
        foreach (string file in new[] { source, Path.Combine(directory, "NativeProbeUnit.pas") })
            HiddenRestoreDirectories.Add(service.GetConfiguration(new PackageRestoreRequest {
                Packages = Array.Empty<PackageRequirement>(), ProgramFilePath = file,
                Sources = new[] { feed }, PackagesDirectory = cache }).RestoreDirectory);
        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
        string kit = Environment.GetEnvironmentVariable("PABC_NUGET_TEST_COMPILER_DIR") ?? Path.Combine(repository, "bin-net10");
        var compiled = await RunCompiler(Path.Combine(kit, "pabcnetcclear.exe"), source, cache);
        Check(compiled.ExitCode == 0, "Native probing compilation failed: " + compiled.Output + compiled.Error);
        string executable = Path.ChangeExtension(source, ".exe");
        string native = Path.Combine(cache, id.ToLowerInvariant(), "1.0.0", "runtimes", "win-x64", "native", dll);
        using (var config = JsonDocument.Parse(File.ReadAllText(Path.ChangeExtension(source, ".runtimeconfig.json"))))
        {
            string paths = config.RootElement.GetProperty("runtimeOptions").GetProperty("configProperties")
                .GetProperty("PascalABCNET.NativeSearchPaths").GetString()!;
            Check(paths == Path.GetDirectoryName(native), "Only the RID-selected native package directory may enter PATH.");
        }
        Directory.Move(feed, feed + "-offline");
        for (int i = 0; i < 2; i++)
        {
            var run = await RunDotnet(executable, directory);
            Check(run.ExitCode == 0 && run.Output.Contains("PINVOKE=OK") &&
                run.Output.Split('\n').Any(line => line.Trim().Equals("NATIVE=" + native, StringComparison.OrdinalIgnoreCase)),
                "Raw Win32 LoadLibrary in unit initialization / PInvoke failed: " + run.Output + run.Error);
        }
        Check(Environment.GetEnvironmentVariable("PATH") == originalPath, "Child startup changed the test runner's PATH.");
        Check(!Directory.GetFiles(directory, "*.dll").Any(), "Native probing copied DLLs beside the program.");
        string published = PackagePublisher.Publish(executable, source, kit);
        using (var config = JsonDocument.Parse(File.ReadAllText(Path.Combine(published, "NativeProbe.runtimeconfig.json"))))
        {
            var options = config.RootElement.GetProperty("runtimeOptions");
            Check(options.GetProperty("configProperties").GetProperty("PascalABCNET.NativeSearchPaths").GetString() == ".",
                "Publication must use a relocatable app-base marker, not the original cache.");
        }
        File.Move(native, native + ".hidden");
        try
        {
            var run = await RunDotnet(Path.Combine(published, "NativeProbe.exe"), directory);
            Check(run.ExitCode == 0 && run.Output.Contains("PINVOKE=OK"), "Published native application needs the cache: " + run.Output + run.Error);
        }
        finally { File.Move(native + ".hidden", native); }
        Console.WriteLine("PASS: Win32 LoadLibrary before unit startup, PInvoke, offline cache, isolated PATH, no copying and cache-free publication.");
    }
}
