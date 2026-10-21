using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using VisualPascalABCPlugins;

internal static partial class Program
{
    private const string FakeRuntimeEnvironment = "PABC_TEST_RUNTIME_PROBE";
    private const string StableRuntimes =
        "Microsoft.NETCore.App 10.0.1 [C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App]\r\n" +
        "Microsoft.WindowsDesktop.App 10.0.2 [C:\\Program Files\\dotnet\\shared\\Microsoft.WindowsDesktop.App]\r\n";

    private static bool TryFakeRuntimeProbe(string[] args, out int exitCode)
    {
        exitCode = 0;
        if (args.Length != 1 || args[0] != "--list-runtimes") return false;
        string mode = Environment.GetEnvironmentVariable(FakeRuntimeEnvironment);
        if (mode == "timeout") Thread.Sleep(10000);
        if (mode == "error") { Console.Error.WriteLine(StableRuntimes); exitCode = 17; return true; }
        Console.Write(mode == "stable" ? StableRuntimes : "Microsoft.NETCore.App 9.0.1 [C:\\dotnet]");
        return true;
    }

    private static void CheckRuntimeAvailability()
    {
        Check(Net10RuntimeAvailability.HasStableDesktopRuntime(StableRuntimes),
            "stable Core and Desktop 10 runtimes enable the modern default, even at different patch levels");
        Check(Net10RuntimeAvailability.HasStableDesktopRuntime(StableRuntimes +
            StableRuntimes.Replace("10.0.", "11.0.")), "side-by-side .NET 11 does not hide installed .NET 10");
        foreach (string invalid in new[] { "", null,
            "10.0.401 [C:\\dotnet\\sdk]", // SDK listings are not runtime listings.
            "Microsoft.NETCore.App 10.0.1 [C:\\dotnet]",
            "Microsoft.WindowsDesktop.App 10.0.1 [C:\\dotnet]",
            StableRuntimes.Replace("10.0.", "9.0."), StableRuntimes.Replace("10.0.", "11.0."),
            StableRuntimes.Replace("10.0.1", "10.0.1-preview.1"),
            StableRuntimes.Replace("10.0.2", "10.0.2-rc.1"),
            StableRuntimes.Replace("10.0.1", "not-a-version") })
            Check(!Net10RuntimeAvailability.HasStableDesktopRuntime(invalid),
                "missing, SDK-only, other-version and preview runtimes preserve the legacy default");

        Check(!Net10RuntimeAvailability.IsAvailable(Path.Combine(Path.GetTempPath(),
            Guid.NewGuid().ToString("N"), "dotnet.exe")), "missing dotnet host does not fail IDE startup");
        string previous = Environment.GetEnvironmentVariable(FakeRuntimeEnvironment);
        try
        {
            string host = Assembly.GetExecutingAssembly().Location;
            foreach (string mode in new[] { "stable", "missing", "error", "timeout" })
            {
                Environment.SetEnvironmentVariable(FakeRuntimeEnvironment, mode);
                var watch = Stopwatch.StartNew();
                bool available = Net10RuntimeAvailability.IsAvailable(host, mode == "timeout" ? 150 : 2000);
                Check(available == (mode == "stable"), "real probe process handles " + mode);
                Check(mode != "timeout" || watch.ElapsedMilliseconds < 3000,
                    "an unresponsive configured host cannot hang startup indefinitely");
            }
        }
        finally { Environment.SetEnvironmentVariable(FakeRuntimeEnvironment, previous); }
        Console.WriteLine("Actual configured default dotnet has Desktop Runtime 10: " +
            Net10RuntimeAvailability.IsAvailable("dotnet"));
    }
}
