using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PascalABCNet.PackageServices;

/// <summary>Do not ship older package copies of assemblies already supplied by .NETCore.App.
/// This is deliberately not a general resolver for conflicting user DLLs or WindowsDesktop.</summary>
internal static class FrameworkAssemblyPolicy
{
    private static readonly string RuntimeDirectory = RuntimeEnvironment.GetRuntimeDirectory();

    internal static IReadOnlyList<string> ExcludeProvidedAssemblies(IReadOnlyList<string> assets)
        => Array.AsReadOnly(assets.Where(path => !IsProvided(path)).ToArray());

    private static bool IsProvided(string path)
    {
        // Look only in the running core framework, not TPA (which also contains host
        // and NuGet libraries). This bridge targets the same net10.0 as the compiler.
        string frameworkPath = Path.Combine(RuntimeDirectory, Path.GetFileName(path));
        if (!File.Exists(frameworkPath)) return false;
        try
        {
            var package = AssemblyName.GetAssemblyName(path);
            var framework = AssemblyName.GetAssemblyName(frameworkPath);
            byte[] token = framework.GetPublicKeyToken() ?? Array.Empty<byte>();
            return token.Length != 0 &&
                string.Equals(package.Name, framework.Name, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(package.CultureName ?? "", framework.CultureName ?? "", StringComparison.OrdinalIgnoreCase) &&
                token.SequenceEqual(package.GetPublicKeyToken() ?? Array.Empty<byte>()) &&
                package.Version is not null && framework.Version is not null &&
                (framework.Version > package.Version || (framework.Version == package.Version &&
                    GetFileVersion(frameworkPath) >= GetFileVersion(path)));
        }
        catch (BadImageFormatException)
        {
            // Preserve invalid/foreign assets: filtering must not conceal an error.
            return false;
        }
    }

    private static Version GetFileVersion(string path)
    {
        var info = FileVersionInfo.GetVersionInfo(path);
        return new Version(info.FileMajorPart, info.FileMinorPart, info.FileBuildPart, info.FilePrivatePart);
    }
}
