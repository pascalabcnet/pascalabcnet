using PascalABCNet.LanguageServices;
using StreamJsonRpc;

namespace PascalABCNet.LanguageServer;

internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        if (!TryReadOptions(args, out var documentationLanguageIso))
            return 2;

        var formatter = new JsonMessageFormatter();
        using var messageHandler = new HeaderDelimitedMessageHandler(
            Console.OpenStandardOutput(),
            Console.OpenStandardInput(),
            formatter);
        using var rpc = new JsonRpc(messageHandler);

        var exitSignal = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        var target = new PascalLanguageServerTarget(
            new PascalLanguageService(documentationLanguageIso),
            new SerialRequestDispatcher(),
            exitSignal);

        rpc.AddLocalRpcTarget(target);
        rpc.CancelLocallyInvokedMethodsWhenConnectionIsClosed = true;
        rpc.StartListening();

        await Task.WhenAny(exitSignal.Task, rpc.Completion).ConfigureAwait(false);
        return 0;
    }

    private static bool TryReadOptions(string[] args, out string documentationLanguageIso)
    {
        documentationLanguageIso = "en";

        for (var index = 0; index < args.Length; index++)
        {
            var argument = args[index];
            if (argument == "--stdio")
                continue;

            if (argument == "--documentation-language" && index + 1 < args.Length)
            {
                documentationLanguageIso = args[++index];
                continue;
            }

            Console.Error.WriteLine($"Unsupported argument: {argument}");
            Console.Error.WriteLine(
                "Usage: PascalABCNet.LanguageServer [--stdio] [--documentation-language <iso>]");
            return false;
        }

        return true;
    }
}
