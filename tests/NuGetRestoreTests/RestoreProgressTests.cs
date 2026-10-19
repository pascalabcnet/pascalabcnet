using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using PascalABCNet.PackageServices;

internal static partial class Program
{
    private static async Task CheckRestoreProgress(string work)
    {
        var request = RaceRequest(Path.Combine(work, "progress"), "1.0.0") with {
            NuGetConfigFile = Path.Combine(work, "isolated.config"),
            PackagesDirectory = Path.Combine(work, "progress-packages")
        };
        var notifications = new ConcurrentQueue<PackageRestoreProgress>();
        void Progress(PackageRestoreProgress value) => notifications.Enqueue(value);
        void BrokenObserver(PackageRestoreProgress _) => throw new InvalidOperationException("broken UI observer");
        PackageRestoreService.RestoreProgress += BrokenObserver;
        PackageRestoreService.RestoreProgress += Progress;
        try
        {
            var graph = await new PackageRestoreService().RestoreAsync(request);
            var values = notifications.ToArray();
            Check(values.First().Stage == "begin" && values.Last().Stage == "end" &&
                values.Select(v => v.OperationId).Distinct().Count() == 1,
                "Restore progress is unbalanced or mixes operations.");
            Check(values.Where(v => v.Stage == "installed").Count() == graph.InstalledPackageCount &&
                values.Last().InstalledCount == 2 && values.All(v => !v.PackageId.Contains(work)),
                "Install progress has the wrong count or exposes cache paths.");
            notifications.Clear();
            var entered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var holder = NuGet.Common.ConcurrencyUtilities.ExecuteWithFileLockedAsync(
                Path.Combine(request.RestoreDirectory!, "PascalABCNET.restore"), async _ => {
                    entered.SetResult(); await release.Task; return 0;
                }, CancellationToken.None);
            await entered.Task;
            try
            {
                var waiting = new PackageRestoreService().RestoreAsync(request);
                await Task.Delay(2300);
                Check(notifications.Count(v => v.Stage == "heartbeat") >= 2 && !waiting.IsCompleted,
                    "No live progress while waiting for another process's restore.");
                PackageRestoreService.CancelPendingRestores();
                try { await waiting.WaitAsync(TimeSpan.FromSeconds(5)); throw new Exception("Cancellation ignored"); }
                catch (OperationCanceledException) { }
                Check(notifications.Last().Stage == "end" && notifications.Last().Cancelled,
                    "Cancellation must finish progress cleanly.");
                int count = notifications.Count;
                await Task.Delay(1100);
                Check(notifications.Count == count, "Heartbeat continues after cancellation.");
            }
            finally { release.SetResult(); await holder; }
            notifications.Clear();
            var cached = await new PackageRestoreService().RestoreAsync(request with { ForceRestore = false });
            Check(cached.UsedCachedRestore && notifications.Last().InstalledCount == 0 &&
                !notifications.Last().Cancelled, "Cancellation poisoned a later cached restore.");
            await CheckDownloadCancellation(work);
            Console.WriteLine("PASS: install counts, isolated observers, live lock-wait progress, cancellation, balanced end and cached retry.");
        }
        finally
        {
            PackageRestoreService.RestoreProgress -= Progress;
            PackageRestoreService.RestoreProgress -= BrokenObserver;
        }
    }

    private static async Task CheckDownloadCancellation(string work)
    {
        using var feed = new PausedPackageFeed(Path.Combine(work, "feed", DependencyId + ".1.0.0.nupkg"));
        string config = Path.Combine(work, "paused-feed.config");
        File.WriteAllText(config, new XDocument(new XElement("configuration", new XElement("packageSources",
            new XElement("clear"), new XElement("add", new XAttribute("key", "loopback-test"),
                new XAttribute("value", feed.Source), new XAttribute("allowInsecureConnections", "true"))))).ToString());
        var request = new PackageRestoreRequest {
            Packages = new[] { new PackageRequirement(DependencyId, "1.0.0") },
            Sources = new[] { feed.Source }, NuGetConfigFile = config,
            PackagesDirectory = Path.Combine(work, "download-packages"),
            RestoreDirectory = Path.Combine(work, "download-restore")
        };
        var restoring = new PackageRestoreService().RestoreAsync(request);
        await feed.Downloading.Task.WaitAsync(TimeSpan.FromSeconds(15));
        await Task.Delay(1200);
        PackageRestoreService.CancelPendingRestores();
        try { await restoring.WaitAsync(TimeSpan.FromSeconds(5)); throw new Exception("Download ignored cancellation"); }
        catch (OperationCanceledException) { }
        feed.Release.TrySetResult();
        var retry = await new PackageRestoreService().RestoreAsync(request);
        Check(retry.Packages.Single().Id == DependencyId,
            "Retry failed after cancelling an actual NuGet HTTP download.");
        Console.WriteLine("PASS: actual package download from paused loopback feed cancels and retries; no external network.");
    }

    private sealed class PausedPackageFeed : IDisposable
    {
        private readonly TcpListener listener = new(IPAddress.Loopback, 0);
        private readonly CancellationTokenSource stop = new();
        private readonly byte[] package;
        internal readonly TaskCompletionSource Downloading = new(TaskCreationOptions.RunContinuationsAsynchronously);
        internal readonly TaskCompletionSource Release = new(TaskCreationOptions.RunContinuationsAsynchronously);
        internal string Source { get; }
        internal PausedPackageFeed(string nupkg)
        {
            package = File.ReadAllBytes(nupkg);
            listener.Start();
            Source = $"http://127.0.0.1:{((IPEndPoint)listener.LocalEndpoint).Port}/index.json";
            _ = AcceptAsync();
        }
        private async Task AcceptAsync()
        {
            try
            {
                while (!stop.IsCancellationRequested)
                {
                    var client = await listener.AcceptTcpClientAsync(stop.Token);
                    _ = ServeAsync(client);
                }
            }
            catch (Exception) when (stop.IsCancellationRequested) { }
        }
        private async Task ServeAsync(TcpClient client)
        {
            using (client)
            try
            {
                using var stream = client.GetStream();
                using var reader = new StreamReader(stream, Encoding.ASCII, false, 1024, leaveOpen: true);
                string path = (await reader.ReadLineAsync(stop.Token))!.Split(' ')[1];
                while (!string.IsNullOrEmpty(await reader.ReadLineAsync(stop.Token))) { }
                byte[] body;
                if (path.EndsWith(".nupkg", StringComparison.Ordinal))
                {
                    Downloading.TrySetResult();
                    await Release.Task.WaitAsync(stop.Token);
                    body = package;
                }
                else if (path == "/index.json")
                    body = JsonSerializer.SerializeToUtf8Bytes(new { version = "3.0.0", resources = new[] {
                        new Dictionary<string, string> { ["@id"] = Source.Replace("index.json", "flat/"), ["@type"] = "PackageBaseAddress/3.0.0" }
                    } });
                else body = Encoding.UTF8.GetBytes("{\"versions\":[\"1.0.0\"]}");
                await stream.WriteAsync(Encoding.ASCII.GetBytes($"HTTP/1.1 200 OK\r\nContent-Length: {body.Length}\r\nConnection: close\r\n\r\n"), stop.Token);
                await stream.WriteAsync(body, stop.Token);
            }
            catch (Exception error) when (error is IOException or OperationCanceledException or SocketException) { }
        }
        public void Dispose() { stop.Cancel(); listener.Stop(); Release.TrySetResult(); }
    }
}
