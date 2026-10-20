using PascalABCNet.PackageServices;

internal static partial class Program
{
    private static async Task CheckPackageDiscovery(string work)
    {
        var ranked = PackageDiscoveryService.Rank(new[] {
            new PackageSuggestion("Other.Nod", "1.0.0", null, 999999, "feed"),
            new PackageSuggestion("Nod.B", "1.0.0", null, 10, "feed"),
            new PackageSuggestion("Nod.A", "1.0.0", null, 100, "feed"),
            new PackageSuggestion("Nod.A", "0.9.0", null, 200, "feed")
        }, "Nod");
        Check(ranked.Select(s => s.Id).SequenceEqual(new[] { "Nod.A", "Nod.B", "Other.Nod" }),
            "Package discovery must rank ID-prefix matches before popularity and remove duplicates.");

        string feed = Path.Combine(work, "feed");
        WriteConfiguration(Path.Combine(work, "NuGet.Config"), feed);
        string program = Path.Combine(work, "Discovery.pas");
        var discovery = new PackageDiscoveryService();
        var results = await discovery.SearchAsync(program, "PascalABC.TestR");
        Check(results.Count == 1 && results[0].Id == RootId && results[0].Version == "1.1.0",
            "Package discovery did not use the configured local source or its latest stable version.");
        var versions = await discovery.GetVersionsAsync(program, RootId, "1.");
        Check(versions.SequenceEqual(new[] { "1.1.0", "1.0.0" }),
            "Package discovery did not return exact stable versions in descending order.");
        using var cancelled = new CancellationTokenSource();
        cancelled.Cancel();
        try
        {
            await discovery.SearchAsync(program, "PascalABC.TestR", cancelled.Token);
            throw new Exception("Cancelled package discovery still queried a source.");
        }
        catch (OperationCanceledException) { }
        Console.WriteLine("PASS: package discovery honors configured sources, versions, ranking and cancellation.");
    }
}
