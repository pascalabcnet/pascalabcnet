namespace PascalABCNet.LanguageServer;

internal sealed class SerialRequestDispatcher
{
    private readonly object _syncRoot = new();
    private Task _tail = Task.CompletedTask;

    public Task RunAsync(Func<Task> action, CancellationToken cancellationToken) =>
        RunAsync(async () =>
        {
            await action().ConfigureAwait(false);
            return true;
        }, cancellationToken);

    public Task<T> RunAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken)
    {
        Task<T> scheduled;

        lock (_syncRoot)
        {
            scheduled = RunAfterAsync(_tail, action, cancellationToken);
            _tail = scheduled.ContinueWith(
                _ => { },
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);
        }

        return scheduled;
    }

    private static async Task<T> RunAfterAsync<T>(
        Task predecessor,
        Func<Task<T>> action,
        CancellationToken cancellationToken)
    {
        try
        {
            await predecessor.ConfigureAwait(false);
        }
        catch
        {
            // A failed request must not stop later LSP messages from being processed.
        }

        cancellationToken.ThrowIfCancellationRequested();
        return await action().ConfigureAwait(false);
    }
}
