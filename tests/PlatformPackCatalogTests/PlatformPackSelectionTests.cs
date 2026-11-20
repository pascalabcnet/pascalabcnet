using PascalABCNet.PackageServices;

internal static class PlatformPackSelectionTests
{
    public static int Run()
    {
        int passed = 0;
        void Check(bool condition, string message)
        {
            if (!condition) throw new Exception(message);
            passed++;
        }
        void Reject<T>(Action action, string message) where T : Exception
        {
            try { action(); }
            catch (T) { passed++; return; }
            throw new Exception("Expected " + typeof(T).Name + ": " + message);
        }

        var catalog = PlatformPackCatalogReader.ReadNet10();
        PlatformPackSelection Select(string target, params string[] platforms) =>
            PlatformPackSelector.Select(catalog, target, platforms);
        var core = Select("net10.0");
        Check(core.TargetFramework == "net10.0" && core.Packages.Count == 0, "ordinary cross-platform target needs no Windows package");
        Check(core.FrameworkReferences.Count == 1 && core.FrameworkReferences[0].Id == "Microsoft.NETCore.App" &&
            !core.FrameworkReferences[0].IsWindowsOnly, "cross-platform Core is the only implicit framework");
        Check(Select("NET10.0", "CORE").TargetFramework == "net10.0", "case-insensitive framework and platform names");
        var desktop = Select("net10.0", "winforms", "WPF", "core", "wpf", "WINFORMS");
        Check(desktop.Packages.Count == 0 && desktop.TargetFramework == "net10.0",
            "desktop directives do not change target or request a Windows API pack");
        Check(desktop.FrameworkReferences.Select(r => r.Id).SequenceEqual(new[] {
            "Microsoft.NETCore.App", "Microsoft.WindowsDesktop.App.WPF", "Microsoft.WindowsDesktop.App.WindowsForms"
        }), "desktop references are deduplicated and ordered independently of directive order");

        var windowsDefault = Select("net10.0-windows");
        Check(windowsDefault.TargetFramework == "net10.0-windows7.0" && windowsDefault.Packages.Count == 0,
            "versionless Windows uses API 7.0 without an SDK pack");
        Check(windowsDefault.FrameworkReferences.Count == 1, "Windows target alone does not imply WPF/WinForms");
        foreach (string version in catalog.WindowsSdk.PlatformVersionsWithoutPack)
        {
            var selection = Select("net10.0-windows" + version);
            Check(selection.TargetFramework == "net10.0-windows" + version && selection.Packages.Count == 0,
                "catalog no-pack platform " + version);
        }

        foreach (var row in catalog.WindowsSdk.PlatformVersions)
        {
            var selection = Select("net10.0-windows" + row.PlatformVersion);
            Check(selection.Packages.Count == 1, "one Windows pack for " + row.PlatformVersion);
            var package = selection.Packages.Single();
            Check(package.Id == catalog.WindowsSdk.PackageId && package.Version == row.PackageVersion &&
                package.RuntimePackageId == catalog.WindowsSdk.RuntimePackageId &&
                package.RuntimeIdentifier == catalog.WindowsSdk.RuntimeIdentifier &&
                package.PlatformVersion == row.PlatformVersion, "package identity comes from the catalog");
            Check(package.Profile == (row.PlatformVersion.EndsWith(".1", StringComparison.Ordinal)
                ? "CsWinRT3.Windows" : "Windows"), "exact Windows projection profile " + row.PlatformVersion);
            Check(selection.FrameworkReferences.Count == 1, "platform pack does not download Core or Desktop runtimes");
        }
        var cswinrt2 = Select("net10.0-windows10.0.19041.0").Packages.Single();
        var cswinrt3 = Select("net10.0-windows10.0.19041.1").Packages.Single();
        Check(cswinrt2.PlatformVersion != cswinrt3.PlatformVersion && cswinrt2.Profile != cswinrt3.Profile,
            ".0 and .1 must not be collapsed even when package versions match");
        Check(Select("net10.0-windows10.0.19041").TargetFramework == "net10.0-windows10.0.19041.0",
            "omitted revision is normalized without dropping a nonzero revision");
        var wpf = Select("net10.0-windows10.0.19041.0", "wpf");
        Check(wpf.Packages.Single() == cswinrt2 && wpf.FrameworkReferences.Count == 2,
            "WPF and Windows API package requirements remain separate");

        foreach (string invalid in new[] {
            "net9.0", "net11.0", "net48", "netstandard2.1", "garbage",
            "net10.0-android", "net10.0-ios", "net10.0-macos",
            "net10.0-windows0.0", "net10.0-windows9.0", "net10.0-windows10.0",
            "net10.0-windows10.0.19040.0", "net10.0-windows10.0.19041.2", "net10.0-windows10.0.99999.0"
        })
            Reject<NotSupportedException>(() => Select(invalid), "unsupported target: " + invalid);
        foreach (string invalid in new[] { "", " ", " net10.0", "net10.0 " })
            Reject<ArgumentException>(() => Select(invalid), "empty or padded target");
        Reject<ArgumentNullException>(() => PlatformPackSelector.Select(null!, "net10.0"), "null catalog");
        Reject<ArgumentNullException>(() => PlatformPackSelector.Select(catalog, null!), "null target");
        Reject<ArgumentException>(() => Select("net10.0", "aspnet"), "unknown platform directive");
        Reject<ArgumentException>(() => Select("net10.0", new string[] { null! }), "null platform entry");
        Check(PlatformPackSelector.Select(catalog, "net10.0", platforms: null).Packages.Count == 0,
            "null platform list means no extra frameworks");

        // Alter data only in memory to prove that selection does not hard-code package versions or IDs.
        var changedRow = catalog.WindowsSdk.PlatformVersions.Single(p => p.PlatformVersion == "10.0.19041.0")
            with { PackageVersion = "10.0.19041.123" };
        var changed = catalog with {
            WindowsSdk = catalog.WindowsSdk with {
                PackageId = "Test.Windows.Reference",
                RuntimePackageId = "Test.Windows.Runtime",
                RuntimeIdentifier = "test-rid",
                PlatformVersions = catalog.WindowsSdk.PlatformVersions.Select(p =>
                    p.PlatformVersion == changedRow.PlatformVersion ? changedRow : p).ToArray()
            }
        };
        var fromData = PlatformPackSelector.Select(changed, "net10.0-windows10.0.19041.0").Packages.Single();
        Check(fromData.Id == "Test.Windows.Reference" && fromData.Version == changedRow.PackageVersion &&
            fromData.RuntimePackageId == "Test.Windows.Runtime" && fromData.RuntimeIdentifier == "test-rid",
            "selection uses provided catalog data, not host SDK data or hard-coded versions");
        Reject<InvalidDataException>(() => PlatformPackSelector.Select(catalog with { SchemaVersion = 2 }, "net10.0"),
            "unsupported catalog schema");
        Reject<InvalidDataException>(() => PlatformPackSelector.Select(
            catalog with { FrameworkReferences = Array.Empty<FrameworkReferenceDefinition>() }, "net10.0"),
            "missing Core reference is not silently invented");
        var duplicate = catalog with {
            WindowsSdk = catalog.WindowsSdk with {
                PlatformVersions = catalog.WindowsSdk.PlatformVersions.Concat(new[] { changedRow }).ToArray()
            }
        };
        Reject<InvalidDataException>(() => PlatformPackSelector.Select(duplicate, "net10.0-windows10.0.19041.0"),
            "ambiguous Windows entries are not silently chosen");
        var higherMinimum = catalog with {
            WindowsSdk = catalog.WindowsSdk with {
                PlatformVersions = new[] { changedRow with { MinimumNetVersion = "11.0" } }
            }
        };
        Reject<NotSupportedException>(() => PlatformPackSelector.Select(higherMinimum, "net10.0-windows10.0.19041.0"),
            "a package requiring a newer .NET is rejected");
        Check(catalog.WindowsSdk.PlatformVersions.Single(p => p.PlatformVersion == changedRow.PlatformVersion)
            .PackageVersion != changedRow.PackageVersion, "selection does not mutate the original catalog");
        Console.WriteLine($"PASS: {passed} pure platform pack selection checks.");
        return passed;
    }
}
