namespace PascalABCNet.PackageServices;

public sealed class NativePackageConflictException : IOException
{
    public string FileName { get; }
    public ResolvedPackage FirstPackage { get; }
    public ResolvedPackage SecondPackage { get; }
    public string FirstPath { get; }
    public string SecondPath { get; }

    internal NativePackageConflictException(string name, ResolvedPackage first, string firstPath,
        ResolvedPackage second, string secondPath)
        : base($"Different native files named '{name}' in NuGet packages '{first.Id}/{first.Version}' ({firstPath}) and '{second.Id}/{second.Version}' ({secondPath}).")
    {
        FileName = name;
        FirstPackage = first;
        SecondPackage = second;
        FirstPath = firstPath;
        SecondPath = secondPath;
    }
}

/// <summary>Checks only RID-selected native assets, never loads DLLs or scans package directories.</summary>
public static class NativePackageConflictValidator
{
    public static void Validate(ResolvedPackageGraph graph)
    {
        ArgumentNullException.ThrowIfNull(graph);
        bool windows = graph.RuntimeIdentifier is null ? OperatingSystem.IsWindows()
            : graph.RuntimeIdentifier.StartsWith("win-", StringComparison.OrdinalIgnoreCase);
        var comparer = windows ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        var names = new Dictionary<string, (ResolvedPackage Package, string Path)>(comparer);
        foreach (var package in graph.Packages.OrderBy(p => p.Id, StringComparer.Ordinal))
        foreach (string asset in package.NativeLibraries)
        {
            string path = Path.GetFullPath(asset);
            string name = Path.GetFileName(path);
            if (names.TryGetValue(name, out var previous))
            {
                if (!SameContents(previous.Path, path))
                    throw new NativePackageConflictException(name, previous.Package, previous.Path, package, path);
            }
            else names.Add(name, (package, path));
        }
    }

    private static bool SameContents(string first, string second)
    {
        // Re-read on each validation: a new module or a replaced DLL must not
        // inherit a previous successful comparison. Bounded memory for large native DLLs.
        using var left = File.OpenRead(first);
        using var right = File.OpenRead(second);
        if (left.Length != right.Length) return false;
        long remaining = left.Length;
        byte[] a = new byte[81920], b = new byte[81920];
        while (remaining > 0)
        {
            int count = (int)Math.Min(a.Length, remaining);
            left.ReadExactly(a.AsSpan(0, count));
            right.ReadExactly(b.AsSpan(0, count));
            if (!a.AsSpan(0, count).SequenceEqual(b.AsSpan(0, count))) return false;
            remaining -= count;
        }
        return true;
    }
}
