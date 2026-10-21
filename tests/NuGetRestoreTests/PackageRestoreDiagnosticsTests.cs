using System.Reflection;
using PascalABCCompiler;
using PascalABCNet.PackageServices;

internal static partial class Program
{
    private static async Task CheckRestoreDiagnostics(string work)
    {
        string directory = Path.Combine(work, "restore-diagnostics");
        string feed = Path.Combine(directory, "feed");
        Directory.CreateDirectory(feed);
        CreatePackage(feed, RootId, "1.0.0", "TestRoot", true, true);
        var service = new PackageRestoreService();
        var request = new PackageRestoreRequest
        {
            Packages = new[] { new PackageRequirement(RootId, "1.0.0") },
            Sources = new[] { feed },
            PackagesDirectory = Path.Combine(directory, "packages"),
            RestoreDirectory = Path.Combine(directory, "restore")
        };

        async Task<PackageRestoreException> Failure(PackageRestoreRequest input)
        {
            try { await service.RestoreAsync(input); }
            catch (PackageRestoreException error) { return error; }
            throw new Exception("NuGet restore unexpectedly succeeded.");
        }

        var missingPackage = await Failure(request with
        {
            Packages = new[] { new PackageRequirement("PascalABC.Missing.DiagnosticsTest", "1.0.0") }
        });
        var missingVersion = await Failure(request with
        {
            Packages = new[] { new PackageRequirement(RootId, "9.9.9") }
        });
        var unavailableSource = await Failure(request with
        {
            Sources = new[] { Path.Combine(directory, "unavailable-feed") },
            RestoreDirectory = Path.Combine(directory, "unavailable-restore")
        });
        var mixed = new PackageRestoreException(new[] {
            new PackageDiagnostic("Error", "NU1101", "Package A was not found"),
            new PackageDiagnostic("Error", "NU1301", "Source is unavailable")
        });
        var other = new PackageRestoreException(new[] {
            new PackageDiagnostic("Error", "NU1605", "Dependency conflict")
        });

        Check(missingPackage.Diagnostics.Any(d => d.Code == "NU1101"), "Missing package did not yield NU1101.");
        Check(missingVersion.Diagnostics.Any(d => d.Code == "NU1102"), "Missing version did not yield NU1102.");
        Check(unavailableSource.Diagnostics.Any(d => d.Code == "NU1301"), "Unavailable source did not yield NU1301.");

        var format = typeof(Compiler).GetMethod("FormatPackageRestoreFailure",
            BindingFlags.NonPublic | BindingFlags.Static)!;
        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        foreach (var language in new[] { "Rus", "Eng" })
        {
            StringResources.ResDirectoryName = Path.Combine(repository, "bin", "Lng", language);
            string packageMessage = (string)format.Invoke(null, new object[] { missingPackage })!;
            string versionMessage = (string)format.Invoke(null, new object[] { missingVersion })!;
            string sourceMessage = (string)format.Invoke(null, new object[] { unavailableSource })!;
            string mixedMessage = (string)format.Invoke(null, new object[] { mixed })!;
            string otherMessage = (string)format.Invoke(null, new object[] { other })!;
            Check(packageMessage.Contains("NU1101") && packageMessage.Contains("PascalABC.Missing.DiagnosticsTest") &&
                packageMessage.Contains(language == "Rus" ? "Пакет NuGet не найден" : "NuGet package was not found"),
                "Missing package diagnostic is not localized for " + language + ": " + packageMessage);
            Check(versionMessage.Contains("NU1102") && versionMessage.Contains("9.9.9") &&
                versionMessage.Contains(language == "Rus" ? "Запрошенная версия" : "requested NuGet package version"),
                "Missing version diagnostic is not localized for " + language + ": " + versionMessage);
            Check(sourceMessage.Contains("NU1301") &&
                sourceMessage.Contains(language == "Rus" ? "Источник пакетов NuGet недоступен" : "NuGet package source is unavailable"),
                "Unavailable source diagnostic is not localized for " + language + ": " + sourceMessage);
            Check(mixedMessage.Contains("NU1101") && mixedMessage.Contains("NU1301") &&
                mixedMessage.StartsWith(language == "Rus" ? "Источник пакетов NuGet недоступен" :
                    "A NuGet package source is unavailable", StringComparison.Ordinal),
                "A source failure was hidden by a secondary missing-package diagnostic.");
            Check(otherMessage.Contains("NU1605") &&
                otherMessage.StartsWith(language == "Rus" ? "Не удалось восстановить пакеты NuGet" :
                    "NuGet package restore failed", StringComparison.Ordinal),
                "Unrecognized NuGet diagnostics lost the generic fallback.");
        }
        Console.WriteLine("PASS: missing package, unavailable version and source diagnostics in Russian and English.");
    }
}
