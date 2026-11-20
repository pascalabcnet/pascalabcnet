using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;
using PascalABCNet.PackageServices;

internal static class PlatformPackAssemblyTests
{
    public static int Run()
    {
        string root = Path.Combine(Path.GetTempPath(), "pabc-platform-assemblies-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(root, "data"));
        Directory.CreateDirectory(Path.Combine(root, "lib"));
        int passed = 0;
        void Check(bool ok, string message)
        {
            if (!ok) throw new Exception(message);
            passed++;
        }
        void Reject(Action action, string message)
        {
            try { action(); }
            catch (InvalidDataException) { passed++; return; }
            throw new Exception("Expected invalid platform manifest: " + message);
        }
        string WriteAssembly(string file, string name, bool referenceOnly = false)
        {
            var metadata = new MetadataBuilder();
            metadata.AddModule(0, metadata.GetOrAddString(file), metadata.GetOrAddGuid(Guid.NewGuid()), default, default);
            var assembly = metadata.AddAssembly(metadata.GetOrAddString(name), new Version(1, 0, 0, 0),
                default, default, (AssemblyFlags)0, AssemblyHashAlgorithm.None);
            metadata.AddTypeDefinition(TypeAttributes.NotPublic, default, metadata.GetOrAddString("<Module>"),
                default, MetadataTokens.FieldDefinitionHandle(1), MetadataTokens.MethodDefinitionHandle(1));
            if (referenceOnly)
            {
                var core = metadata.AddAssemblyReference(metadata.GetOrAddString("System.Runtime"),
                    new Version(10, 0, 0, 0), default, default, (AssemblyFlags)0, default);
                var attribute = metadata.AddTypeReference(core,
                    metadata.GetOrAddString("System.Runtime.CompilerServices"), metadata.GetOrAddString("ReferenceAssemblyAttribute"));
                var ctor = metadata.AddMemberReference(attribute, metadata.GetOrAddString(".ctor"),
                    metadata.GetOrAddBlob(new byte[] { 0x20, 0, 1 }));
                metadata.AddCustomAttribute(assembly, ctor, metadata.GetOrAddBlob(new byte[] { 1, 0, 0, 0 }));
            }
            var pe = new ManagedPEBuilder(new PEHeaderBuilder(
                imageCharacteristics: Characteristics.ExecutableImage | Characteristics.Dll),
                new MetadataRootBuilder(metadata), new BlobBuilder());
            var blob = new BlobBuilder();
            pe.Serialize(blob);
            string path = Path.Combine(root, "lib", file);
            File.WriteAllBytes(path, blob.ToArray());
            return path;
        }
        string manifest = Path.Combine(root, "data", "RuntimeList.xml");
        XElement Entry(string path, string? profile = "Windows", string type = "Managed") =>
            new("File", new XAttribute("Type", type), new XAttribute("Path", path),
                profile is null ? null : new XAttribute("Profile", profile));
        void Write(params XElement[] entries) => new XDocument(new XElement("FileList", entries)).Save(manifest);
        var selected = new SelectedPlatformPackage("Test.Pack", "1.0.0", "Test.Pack", "any", "10.0.19041.0", "Windows");
        var selection = new PlatformPackSelection("net10.0-windows10.0.19041.0",
            Array.Empty<FrameworkReferenceDefinition>(), new[] { selected });
        var packages = new[] { new CachedPlatformPackage("Test.Pack", "1.0.0", root, true) };
        IReadOnlyList<PlatformPackAssembly> Resolve() => PlatformPackAssemblyResolver.Resolve(selection, packages);
        try
        {
            string win2 = WriteAssembly("win2.dll", "WindowsProjection2");
            string win3 = WriteAssembly("win3.dll", "WindowsProjection3");
            string common = WriteAssembly("common.dll", "CommonPlatform");
            string refOnly = WriteAssembly("reference.dll", "ReferenceOnly", referenceOnly: true);
            Write(Entry("lib/win2.dll"), Entry("lib/win3.dll", "CsWinRT3.Windows"),
                Entry("lib/common.dll", null), Entry("not-installed-analyzer.dll", type: "Analyzer"),
                Entry("not-installed-native.dll", type: "Native"));
            var result = Resolve();
            Check(result.Select(p => p.Name).SequenceEqual(new[] { "CommonPlatform", "WindowsProjection2" }),
                "only selected-profile managed runtime implementations become compilation references");
            Check(result.Any(p => p.Path == win2) && result.Any(p => p.Path == common),
                "DLLs remain in the downloaded package; no copies are made");
            var cs3 = PlatformPackAssemblyResolver.Resolve(selection with {
                Packages = new[] { selected with { Profile = "CsWinRT3.Windows", PlatformVersion = "10.0.19041.1" } }
            }, packages);
            Check(cs3.Any(p => p.Path == win3) && cs3.All(p => p.Path != win2), ".1 uses the distinct CsWinRT 3 profile");
            Check(!AppDomain.CurrentDomain.GetAssemblies().Any(a =>
                new[] { "WindowsProjection2", "WindowsProjection3", "CommonPlatform" }.Contains(a.GetName().Name)),
                "resolver reads metadata without loading platform DLLs into the host");
            Write(Entry("lib/common.dll", "Windows; CsWinRT3.Windows"));
            Check(Resolve().Single().Path == common, "semicolon-separated profile membership");
            Write(Entry("lib/common.dll"), Entry("lib/common.dll"));
            Check(Resolve().Count == 1, "same runtime path is deduplicated");

            foreach (string path in new[] { "../outside.dll", "/outside.dll", "C:/outside.dll",
                "\\\\server\\share\\a.dll", "lib/../outside.dll", "lib//common.dll", "ref/net10.0/a.dll", "lib/a.exe" })
            {
                Write(Entry(path));
                Reject(() => Resolve(), "unsafe or reference asset path: " + path);
            }
            Write(Entry("lib/missing.dll"));
            Reject(() => Resolve(), "missing runtime DLL");
            Write(Entry("lib/reference.dll"));
            Reject(() => Resolve(), "reference-only assembly outside ref directory");
            File.WriteAllText(Path.Combine(root, "lib", "invalid.dll"), "not a managed image");
            Write(Entry("lib/invalid.dll"));
            Reject(() => Resolve(), "invalid managed image");
            Write(new XElement("File", new XAttribute("Type", "Managed"), new XAttribute("Profile", "Windows")));
            Reject(() => Resolve(), "missing path");
            Write(Entry("lib/win3.dll", "CsWinRT3.Windows"));
            Reject(() => Resolve(), "no selected-profile runtime assets");
            var wrongName = Entry("lib/common.dll");
            wrongName.Add(new XAttribute("AssemblyName", "WrongName"));
            Write(wrongName);
            Reject(() => Resolve(), "manifest identity mismatch");
            WriteAssembly("duplicate.dll", "CommonPlatform");
            Write(Entry("lib/common.dll"), Entry("lib/duplicate.dll"));
            Reject(() => Resolve(), "same assembly name from two different files");
            File.WriteAllText(manifest, "<Broken>");
            Reject(() => Resolve(), "malformed XML");
            File.WriteAllText(manifest, "<!DOCTYPE FileList [<!ENTITY x SYSTEM 'file:///not-read'>]><FileList>&x;</FileList>");
            Reject(() => Resolve(), "DTD/external entities forbidden");
            File.Delete(manifest);
            Reject(() => Resolve(), "missing runtime manifest");
            Reject(() => PlatformPackAssemblyResolver.Resolve(selection, Array.Empty<CachedPlatformPackage>()),
                "missing selected package");
            Reject(() => PlatformPackAssemblyResolver.Resolve(selection,
                new[] { packages[0] with { Version = "2.0.0" } }), "wrong cached package version");
            Check(PlatformPackAssemblyResolver.Resolve(selection with { Packages = Array.Empty<SelectedPlatformPackage>() },
                Array.Empty<CachedPlatformPackage>()).Count == 0, "Core-only selection does not read Windows metadata");
            Check(!File.Exists(Path.Combine(root, "common.dll")) && !File.Exists(Path.Combine(root, "test.deps.json")),
                "no copying or runtime-manifest generation");
            Console.WriteLine($"PASS: {passed} platform compilation asset checks (metadata only).");
            return passed;
        }
        finally { Directory.Delete(root, recursive: true); }
    }
}
