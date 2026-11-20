using System.Text;
using System.Text.Json.Nodes;
using PascalABCNet.PackageServices;

internal static class Program
{
    private static int passed;

    private static async Task<int> Main()
    {
        try
        {
            var catalog = PlatformPackCatalogReader.ReadNet10();
            Check(catalog.SchemaVersion == 1 && catalog.TargetFramework == "net10.0", "embedded catalog header");
            Check(catalog.Source.Version == "10.0.404" && catalog.Source.Files.Count == 2, "source provenance");
            Check(catalog.FrameworkReferences.Count == 4 && catalog.WindowsSdk.PlatformVersions.Count == 14,
                "all selected platform data is present");
            var cswinrt2 = catalog.WindowsSdk.PlatformVersions.Single(p => p.PlatformVersion == "10.0.19041.0");
            var cswinrt3 = catalog.WindowsSdk.PlatformVersions.Single(p => p.PlatformVersion == "10.0.19041.1");
            Check(cswinrt2.MinimumNetVersion == "8.0" && cswinrt3.MinimumNetVersion == "10.0",
                "exact .0 and .1 platform revisions remain distinct");
            Check(catalog.FrameworkReferences.Single(p => p.Id == "Microsoft.NETCore.App").Profile == null,
                "absent optional profile");
            Check(catalog.FrameworkReferences.Single(p => p.Id == "Microsoft.WindowsDesktop.App.WPF").Profile == "WPF",
                "WPF profile is read");

            using var resource = typeof(PlatformPackCatalogReader).Assembly.GetManifestResourceStream(
                "PascalABCNet.PackageServices.PlatformPacks.net10.0.json")
                ?? throw new Exception("Catalog is not embedded in the library.");
            using var textReader = new StreamReader(resource);
            string original = textReader.ReadToEnd();

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(original)))
            {
                var fromStream = PlatformPackCatalogReader.Read(stream);
                Check(stream.CanRead && stream.Position == stream.Length, "caller owns the input stream");
                Check(fromStream.Source.Files[0].Sha256 == catalog.Source.Files[0].Sha256,
                    "stream and embedded readers agree");
            }
            RejectJson("{", "malformed JSON", "Invalid platform pack catalog");
            RejectJson("null", "null root", "$");
            RejectJson("[]", "wrong root shape", "JSON");
            RejectJson(original.Replace("\"schemaVersion\": 1", "\"schemaVersion\": 1, \"schemaVersion\": 1"),
                "duplicate JSON root key", "$.schemaVersion");
            RejectJson(original.Replace("\"minimumNetVersion\": \"8.0\"",
                "\"minimumNetVersion\": \"8.0\", \"minimumNetVersion\": \"8.0\""),
                "duplicate JSON nested key", "$.windowsSdk.platformVersions[0].minimumNetVersion");

            void Reject(string name, string path, Action<JsonObject> change)
            {
                var root = JsonNode.Parse(original)!.AsObject();
                change(root);
                RejectJson(root.ToJsonString(), name, path);
            }

            Reject("unsupported schema", "$.schemaVersion", r => r["schemaVersion"] = 2);
            Reject("wrong target", "$.targetFramework", r => r["targetFramework"] = "net11.0");
            Reject("missing required property", "JSON", r => r.Remove("targetFramework"));
            Reject("missing boolean property", "JSON",
                r => r["frameworkReferences"]![0]!.AsObject().Remove("isWindowsOnly"));
            Reject("unknown property", "JSON", r => r["targetFramwork"] = "net10.0");
            Reject("null source", "$.source", r => r["source"] = null);
            Reject("null Windows data", "$.windowsSdk", r => r["windowsSdk"] = null);
            Reject("null notes", "$.notes", r => r["notes"] = null);
            Reject("null note", "$.notes[0]", r => r["notes"]![0] = null);
            Reject("null source file", "$.source.files[0]", r => r["source"]!["files"]![0] = null);
            Reject("empty sources", "$.source.files", r => r["source"]!["files"] = new JsonArray());
            Reject("absolute source path", "$.source.files[0].path",
                r => r["source"]!["files"]![0]!["path"] = "C:/SDK/versions.props");
            Reject("source traversal", "$.source.files[0].path",
                r => r["source"]!["files"]![0]!["path"] = "../versions.props");
            Reject("nonportable source path", "$.source.files[0].path",
                r => r["source"]!["files"]![0]!["path"] = "SDK\\versions.props");
            Reject("bad source hash", "$.source.files[0].sha256", r => r["source"]!["files"]![0]!["sha256"] = "xyz");
            Reject("duplicate source file", "$.source.files[2].path",
                r => r["source"]!["files"]!.AsArray().Add(r["source"]!["files"]![0]!.DeepClone()));
            Reject("non-HTTPS provenance", "$.source.selectionReference",
                r => r["source"]!["selectionReference"] = "file:///sdk/source.cs");
            Reject("empty framework list", "$.frameworkReferences", r => r["frameworkReferences"] = new JsonArray());
            Reject("null framework entry", "$.frameworkReferences[0]", r => r["frameworkReferences"]![0] = null);
            Reject("blank ID", "$.frameworkReferences[0].id", r => r["frameworkReferences"]![0]!["id"] = " ");
            Reject("invalid package ID", "$.frameworkReferences[0].targetingPackName",
                r => r["frameworkReferences"]![0]!["targetingPackName"] = "../bad");
            Reject("duplicate framework ignoring case", "$.frameworkReferences[4].id", r => {
                var duplicate = r["frameworkReferences"]![0]!.DeepClone();
                duplicate["id"] = "microsoft.netcore.app";
                r["frameworkReferences"]!.AsArray().Add(duplicate);
            });
            Reject("invalid version", "$.frameworkReferences[0].targetingPackVersion",
                r => r["frameworkReferences"]![0]!["targetingPackVersion"] = "not-a-version");
            Reject("reversed runtime versions", "$.frameworkReferences[0].latestRuntimeFrameworkVersion",
                r => r["frameworkReferences"]![0]!["latestRuntimeFrameworkVersion"] = "9.0.0");
            Reject("missing RID placeholder", "$.frameworkReferences[0].runtimePackNamePattern",
                r => r["frameworkReferences"]![0]!["runtimePackNamePattern"] = "Microsoft.NETCore.App.Runtime.win-x64");
            Reject("duplicate RID placeholder", "$.frameworkReferences[0].runtimePackNamePattern",
                r => r["frameworkReferences"]![0]!["runtimePackNamePattern"] = "Runtime.**RID**.**RID**");
            Reject("blank profile", "$.frameworkReferences[2].profile", r => r["frameworkReferences"]![2]!["profile"] = "");
            Reject("null pack ID", "$.windowsSdk.packageId", r => r["windowsSdk"]!["packageId"] = null);
            Reject("non-Windows SDK", "$.windowsSdk.isWindowsOnly", r => r["windowsSdk"]!["isWindowsOnly"] = false);
            Reject("invalid RID", "$.windowsSdk.runtimeIdentifier", r => r["windowsSdk"]!["runtimeIdentifier"] = "../any");
            Reject("empty Windows list", "$.windowsSdk.platformVersions",
                r => r["windowsSdk"]!["platformVersions"] = new JsonArray());
            Reject("null Windows entry", "$.windowsSdk.platformVersions[0]",
                r => r["windowsSdk"]!["platformVersions"]![0] = null);
            Reject("bad platform version", "$.windowsSdk.platformVersions[0].platformVersion",
                r => r["windowsSdk"]!["platformVersions"]![0]!["platformVersion"] = "10.x");
            Reject("minimum .NET too high", "$.windowsSdk.platformVersions[0].minimumNetVersion",
                r => r["windowsSdk"]!["platformVersions"]![0]!["minimumNetVersion"] = "11.0");
            Reject("bad Windows package version", "$.windowsSdk.platformVersions[0].packageVersion",
                r => r["windowsSdk"]!["platformVersions"]![0]!["packageVersion"] = "bad");
            Reject("duplicate exact Windows version", "$.windowsSdk.platformVersions[14].platformVersion",
                r => r["windowsSdk"]!["platformVersions"]!.AsArray().Add(
                    r["windowsSdk"]!["platformVersions"]![0]!.DeepClone()));
            Reject("overlapping pack/no-pack Windows version", "$.windowsSdk.platformVersions[0].platformVersion",
                r => r["windowsSdk"]!["platformVersionsWithoutPack"]!.AsArray().Add("10.0.17763.0"));
            Reject("equivalent no-pack Windows versions", "$.windowsSdk.platformVersionsWithoutPack[2]",
                r => r["windowsSdk"]!["platformVersionsWithoutPack"]!.AsArray().Add("7.0.0.0"));

            var withNullProfile = JsonNode.Parse(original)!;
            withNullProfile["frameworkReferences"]![0]!["profile"] = null;
            using (var input = new MemoryStream(Encoding.UTF8.GetBytes(withNullProfile.ToJsonString())))
                Check(PlatformPackCatalogReader.Read(input).FrameworkReferences[0].Profile == null, "explicit null profile");
            Check(PlatformPackCatalogReader.ReadNet10().WindowsSdk.PlatformVersions.Count == 14,
                "failed reads do not change the embedded catalog");
            passed += PlatformPackSelectionTests.Run();
            passed += PlatformPackAssemblyTests.Run();
            passed += await PlatformPackDownloadTests.RunAsync();
            Console.WriteLine($"PASS: {passed} platform pack checks (local feeds only; no internet or SDK lookup).");
            return 0;
        }
        catch (Exception error)
        {
            Console.Error.WriteLine(error);
            return 1;
        }
    }

    private static void RejectJson(string json, string name, string messagePart)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        try { PlatformPackCatalogReader.Read(stream); }
        catch (InvalidDataException error)
        {
            Check(error.Message.Contains(messagePart, StringComparison.Ordinal),
                name + ": diagnostic includes " + messagePart + "; actual: " + error.Message);
            Check(stream.CanRead, name + ": reader leaves stream open on failure");
            return;
        }
        throw new Exception("Invalid catalog was accepted: " + name);
    }

    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
        passed++;
    }
}
