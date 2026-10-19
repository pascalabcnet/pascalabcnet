using System.Collections.Concurrent;
using System.Diagnostics;

namespace PascalABCNet.PackageServices;

/// <summary>Process-local notifications; no feed URLs, credentials or cache paths.</summary>
public sealed record PackageRestoreProgress(string OperationId, string Stage, string PackageId,
    int InstalledCount, long ElapsedMilliseconds, bool Cancelled);

public sealed partial class PackageRestoreService
{
    public static event Action<PackageRestoreProgress>? RestoreProgress;
    private static readonly ConcurrentDictionary<string, RestoreOperation> ActiveRestores = new();
    private static readonly AsyncLocal<RestoreOperation?> CurrentRestore = new();

    /// <summary>Cancel active package work in this host (e.g. the language server).
    /// Later restores get fresh cancellation sources and may retry normally.</summary>
    public static void CancelPendingRestores()
    {
        foreach (var operation in ActiveRestores.Values)
            try { operation.Cancellation.Cancel(); } catch (ObjectDisposedException) { }
    }

    private sealed class RestoreOperation : IDisposable
    {
        internal readonly CancellationTokenSource Cancellation;
        private readonly string id = Guid.NewGuid().ToString("N");
        private readonly Stopwatch clock = Stopwatch.StartNew();
        private readonly CancellationTokenSource heartbeat = new();
        private string package;
        private int installed;

        internal RestoreOperation(PackageRestoreRequest request, CancellationToken token)
        {
            Cancellation = CancellationTokenSource.CreateLinkedTokenSource(token);
            package = string.Join(", ", request.Packages.Where(p => p is not null).Select(p => p.Id));
        }

        internal async Task<ResolvedPackageGraph> RunAsync(Func<CancellationToken, Task<ResolvedPackageGraph>> action)
        {
            var previous = CurrentRestore.Value;
            CurrentRestore.Value = this;
            ActiveRestores[id] = this;
            Report("begin"); // Includes waiting for another host's restore lock.
            var pulse = PulseAsync();
            try { return await action(Cancellation.Token).ConfigureAwait(false); }
            finally
            {
                heartbeat.Cancel();
                await pulse.ConfigureAwait(false); // Never send a heartbeat after end.
                ActiveRestores.TryRemove(id, out _);
                Report("end");
                CurrentRestore.Value = previous;
            }
        }

        private async Task PulseAsync()
        {
            try
            {
                while (true)
                {
                    await Task.Delay(1000, heartbeat.Token).ConfigureAwait(false);
                    Report("heartbeat");
                }
            }
            catch (OperationCanceledException) when (heartbeat.IsCancellationRequested) { }
        }

        internal void Installed(string packageId)
        {
            package = packageId;
            Interlocked.Increment(ref installed);
            Report("installed");
        }

        private void Report(string stage)
        {
            var value = new PackageRestoreProgress(id, stage, package, Volatile.Read(ref installed),
                clock.ElapsedMilliseconds, Cancellation.IsCancellationRequested);
            var observers = RestoreProgress;
            if (observers is null) return;
            foreach (Action<PackageRestoreProgress> observer in observers.GetInvocationList())
                try { observer(value); } catch (Exception error) { Trace.TraceWarning("NuGet progress: {0}", error.Message); }
        }

        public void Dispose() { Cancellation.Dispose(); heartbeat.Dispose(); }
    }
}
