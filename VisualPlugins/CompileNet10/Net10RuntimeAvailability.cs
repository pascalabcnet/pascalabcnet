using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Threading.Tasks;

namespace VisualPascalABCPlugins
{
    internal static class Net10RuntimeAvailability
    {
        // Probe the actual configured host, not an SDK version or an unrelated registry entry.
        // --list-runtimes neither starts the compiler nor restores packages.
        internal static bool IsAvailable(string dotnetPath, int timeoutMilliseconds = 2000)
        {
            if (string.IsNullOrWhiteSpace(dotnetPath)) return false;
            try
            {
                using (var process = new Process { StartInfo = new ProcessStartInfo(dotnetPath, "--list-runtimes")
                {
                    UseShellExecute = false, CreateNoWindow = true,
                    RedirectStandardOutput = true, RedirectStandardError = true
                } })
                {
                    process.Start();
                    Task<string> output = process.StandardOutput.ReadToEndAsync();
                    Task<string> error = process.StandardError.ReadToEndAsync();
                    if (!process.WaitForExit(timeoutMilliseconds))
                    {
                        try { process.Kill(); } catch (Win32Exception) { } catch (InvalidOperationException) { }
                        return false;
                    }
                    if (!Task.WaitAll(new Task[] { output, error }, timeoutMilliseconds)) return false;
                    return process.ExitCode == 0 && HasStableDesktopRuntime(output.Result);
                }
            }
            // Missing/inaccessible/broken custom hosts must leave the legacy mode usable.
            catch (Win32Exception) { return false; }
            catch (IOException) { return false; }
            catch (InvalidOperationException) { return false; }
            catch (UnauthorizedAccessException) { return false; }
            catch (SecurityException) { return false; }
            catch (ArgumentException) { return false; }
            catch (AggregateException) { return false; }
        }

        internal static bool HasStableDesktopRuntime(string text)
        {
            bool core = false, desktop = false;
            foreach (string line in (text ?? "").Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                Version version;
                if (parts.Length < 3 || !parts[2].StartsWith("[", StringComparison.Ordinal) ||
                    !Version.TryParse(parts[1], out version) || version.Major != 10 ||
                    version.Minor != 0 || version.Build < 0 || version.Revision != -1) continue;
                core |= parts[0] == "Microsoft.NETCore.App";
                desktop |= parts[0] == "Microsoft.WindowsDesktop.App";
            }
            return core && desktop;
        }
    }
}
