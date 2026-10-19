using System.Diagnostics;
using PascalABCNet.PackageServices;

internal static partial class Program
{
    private static PackageRestoreRequest RaceRequest(string directory, string version) => new() {
        Packages = new[] { new PackageRequirement(RootId, version) },
        Sources = new[] { Path.Combine(Path.GetDirectoryName(directory)!, "feed") },
        PackagesDirectory = Path.Combine(Path.GetDirectoryName(directory)!, "packages"),
        RestoreDirectory = Path.Combine(directory, "restore"),
        NuGetConfigFile = Path.Combine(directory, "NuGet.Config"), ForceRestore = true
    };

    private static async Task RestoreRaceChild(string directory, string version)
    {
        Console.WriteLine("READY");
        await Console.In.ReadLineAsync();
        await RaceRestores(directory, version);
    }

    private static async Task RaceRestores(string directory, string version)
    {
        var request = RaceRequest(directory, version);
        for (int i = 0; i < 12; i++)
        {
            var graph = await new PackageRestoreService().RestoreAsync(request);
            Check(graph.Packages.Single(p => p.Id == RootId).Version == version,
                "Restore read another client's package graph instead of " + version);
            string expected = Path.Combine(AppContext.BaseDirectory, "fixtures", version == "1.1.0" ? "v2" : "",
                "lib", "TestRoot.dll");
            Check(Hash(graph.Packages.Single(p => p.Id == RootId).RuntimeAssemblies.Single()) == Hash(expected),
                "Restore returned another client's runtime DLL.");
        }
    }

    private static async Task CheckRestoreConcurrency(string work)
    {
        string directory = Path.Combine(work, "restore-concurrency");
        Directory.CreateDirectory(directory);
        WriteConfiguration(Path.Combine(directory, "NuGet.Config"), Path.Combine(work, "feed"), Path.Combine(work, "packages"));
        async Task RunChild(string version)
        {
            var start = new ProcessStartInfo(Environment.ProcessPath!) {
                UseShellExecute = false, RedirectStandardInput = true, RedirectStandardOutput = true,
                RedirectStandardError = true, CreateNoWindow = true
            };
            if (string.Equals(Path.GetFileNameWithoutExtension(Environment.ProcessPath), "dotnet", StringComparison.OrdinalIgnoreCase))
                start.ArgumentList.Add(typeof(Program).Assembly.Location);
            foreach (string arg in new[] { "--restore-race-child", directory, version }) start.ArgumentList.Add(arg);
            using var process = Process.Start(start)!;
            try
            {
                Check(await process.StandardOutput.ReadLineAsync().WaitAsync(TimeSpan.FromSeconds(30)) == "READY", "Restore child did not start.");
                var output = process.StandardOutput.ReadToEndAsync();
                var error = process.StandardError.ReadToEndAsync();
                await process.StandardInput.WriteLineAsync("GO");
                await process.StandardInput.FlushAsync();
                await process.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(60));
                Check(process.ExitCode == 0, "Concurrent restore failed for " + version + ": " + await output + await error);
            }
            finally { if (!process.HasExited) process.Kill(entireProcessTree: true); }
        }
        await Task.WhenAll(RunChild("1.0.0"), RunChild("1.1.0"));
        await Task.WhenAll(RaceRestores(directory, "1.0.0"), RaceRestores(directory, "1.1.0"));
        var entered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var held = NuGet.Common.ConcurrencyUtilities.ExecuteWithFileLockedAsync(
            Path.Combine(directory, "restore", "PascalABCNET.restore"), async _ => {
                entered.SetResult();
                await release.Task;
                return 0;
            }, CancellationToken.None);
        await entered.Task;
        try
        {
            using var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            try
            {
                await new PackageRestoreService().RestoreAsync(RaceRequest(directory, "1.0.0"), cancellation.Token);
                throw new Exception("Restore ignored cancellation while waiting for another client.");
            }
            catch (OperationCanceledException) when (cancellation.IsCancellationRequested) { }
        }
        finally { release.SetResult(); await held; }
        await new PackageRestoreService().RestoreAsync(RaceRequest(directory, "1.0.0"));
        var cached = await new PackageRestoreService().RestoreAsync(RaceRequest(directory, "1.0.0") with { ForceRestore = false });
        Check(cached.UsedCachedRestore && cached.Packages.Single(p => p.Id == RootId).Version == "1.0.0",
            "No-op restore failed after concurrent clients.");
        Console.WriteLine("PASS: two processes and two in-process clients share one restore directory without missing/mixed assets; waiting cancels, no-op recovers; local feed only.");
    }
}
