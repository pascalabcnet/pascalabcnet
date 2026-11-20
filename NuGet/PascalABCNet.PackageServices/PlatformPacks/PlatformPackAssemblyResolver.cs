using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Xml;
using System.Xml.Linq;
using NuGet.Versioning;

namespace PascalABCNet.PackageServices;

/// <summary>A managed runtime implementation usable by the compiler's reflection-based semantic reader.</summary>
public sealed record PlatformPackAssembly(string Name, string Path);

/// <summary>
/// Resolves compile-time reflection inputs from downloaded runtime-pack manifests.
/// Does not load assemblies, download packages, copy DLLs or write execution manifests.
/// </summary>
public static class PlatformPackAssemblyResolver
{
    public static IReadOnlyList<PlatformPackAssembly> Resolve(PlatformPackSelection selection,
        IReadOnlyList<CachedPlatformPackage> packages)
    {
        ArgumentNullException.ThrowIfNull(selection);
        ArgumentNullException.ThrowIfNull(packages);
        var result = new Dictionary<string, PlatformPackAssembly>(StringComparer.OrdinalIgnoreCase);
        foreach (var selected in selection.Packages)
        {
            var matches = packages.Where(p =>
                string.Equals(p.Id, selected.RuntimePackageId, StringComparison.OrdinalIgnoreCase) &&
                NuGetVersion.Parse(p.Version) == NuGetVersion.Parse(selected.Version)).ToArray();
            if (matches.Length != 1)
                throw new InvalidDataException("Expected one cached runtime platform package: " +
                    selected.RuntimePackageId + " " + selected.Version);
            string root = Path.GetFullPath(matches[0].Directory);
            string manifest = Path.Combine(root, "data", "RuntimeList.xml");
            if (!File.Exists(manifest))
                throw new InvalidDataException("Platform runtime manifest is missing: " + manifest);
            XDocument document;
            try
            {
                using var xml = XmlReader.Create(manifest, new XmlReaderSettings {
                    DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null,
                    MaxCharactersInDocument = 4 * 1024 * 1024
                });
                document = XDocument.Load(xml);
            }
            catch (XmlException error)
            {
                throw new InvalidDataException("Invalid platform runtime manifest: " + manifest, error);
            }
            if (document.Root?.Name != "FileList")
                throw new InvalidDataException("Expected FileList in platform runtime manifest: " + manifest);
            int count = 0;
            foreach (var file in document.Root.Elements("File"))
            {
                if (!string.Equals((string?)file.Attribute("Type"), "Managed", StringComparison.OrdinalIgnoreCase))
                    continue;
                string? profile = (string?)file.Attribute("Profile");
                if (!string.IsNullOrEmpty(profile) &&
                    !profile.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                        .Contains(selected.Profile, StringComparer.OrdinalIgnoreCase))
                    continue;
                string path = ResolvePath(root, (string?)file.Attribute("Path"));
                string name = ReadAssemblyName(path);
                string? declaredName = (string?)file.Attribute("AssemblyName");
                if (declaredName is not null && !string.Equals(name, declaredName, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidDataException("Platform manifest assembly identity does not match: " + path);
                if (result.TryGetValue(name, out var previous))
                {
                    if (!string.Equals(previous.Path, path,
                        OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                        throw new InvalidDataException("Duplicate platform assembly: " + name);
                }
                else
                    result.Add(name, new PlatformPackAssembly(name, path));
                count++;
            }
            if (count == 0)
                throw new InvalidDataException("No managed runtime assemblies for platform profile " +
                    selected.Profile + " in " + manifest);
        }
        return Array.AsReadOnly(result.Values.OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase).ToArray());
    }

    private static string ResolvePath(string root, string? relative)
    {
        if (string.IsNullOrWhiteSpace(relative) || relative != relative.Trim() ||
            relative.StartsWith('/') || relative.StartsWith('\\') || relative.Contains(':'))
            throw new InvalidDataException("Invalid platform runtime asset path: " + relative);
        string[] segments = relative.Replace('\\', '/').Split('/');
        if (segments.Any(p => p.Length == 0 || p == "." || p == "..") ||
            string.Equals(segments[0], "ref", StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(Path.GetExtension(relative), ".dll", StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("Expected a runtime DLL inside the platform package: " + relative);
        string prefix = Path.TrimEndingDirectorySeparator(root) + Path.DirectorySeparatorChar;
        string path = Path.GetFullPath(Path.Combine(root, Path.Combine(segments)));
        if (!path.StartsWith(prefix, OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)
            || !File.Exists(path))
            throw new InvalidDataException("Platform runtime DLL is missing or outside its package: " + relative);
        return path;
    }

    private static string ReadAssemblyName(string path)
    {
        try
        {
            using var stream = File.OpenRead(path);
            using var pe = new PEReader(stream);
            if (!pe.HasMetadata)
                throw new BadImageFormatException("Missing managed metadata.");
            var metadata = pe.GetMetadataReader();
            if (!metadata.IsAssembly)
                throw new BadImageFormatException("Not an assembly.");
            foreach (var handle in metadata.GetAssemblyDefinition().GetCustomAttributes())
            {
                var constructor = metadata.GetCustomAttribute(handle).Constructor;
                EntityHandle type = constructor.Kind switch {
                    HandleKind.MemberReference => metadata.GetMemberReference((MemberReferenceHandle)constructor).Parent,
                    HandleKind.MethodDefinition => metadata.GetMethodDefinition((MethodDefinitionHandle)constructor).GetDeclaringType(),
                    _ => default
                };
                string name, ns;
                if (type.Kind == HandleKind.TypeReference)
                {
                    var reference = metadata.GetTypeReference((TypeReferenceHandle)type);
                    name = metadata.GetString(reference.Name);
                    ns = metadata.GetString(reference.Namespace);
                }
                else if (type.Kind == HandleKind.TypeDefinition)
                {
                    var definition = metadata.GetTypeDefinition((TypeDefinitionHandle)type);
                    name = metadata.GetString(definition.Name);
                    ns = metadata.GetString(definition.Namespace);
                }
                else continue;
                if (name == "ReferenceAssemblyAttribute" && ns == "System.Runtime.CompilerServices")
                    throw new InvalidDataException("Platform asset is reference-only and cannot be loaded for semantic analysis: " + path);
            }
            return metadata.GetString(metadata.GetAssemblyDefinition().Name);
        }
        catch (BadImageFormatException error)
        {
            throw new InvalidDataException("Platform asset is not a managed runtime assembly: " + path, error);
        }
    }
}
