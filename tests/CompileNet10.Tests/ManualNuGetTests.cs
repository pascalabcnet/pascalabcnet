using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using VisualPascalABCPlugins;

internal static partial class Program
{
    private static async Task CheckManualNuGetClient(string runtime, string testAssembly)
    {
        string root = Path.Combine(Path.GetTempPath(), "pabc-manual-nuget-" + Guid.NewGuid().ToString("N"));
        string file = Path.Combine(root, "Main.pas"), packages = Path.Combine(root, "packages");
        Directory.CreateDirectory(root);
        try
        {
            string feed = Path.Combine(root, "feed"); Directory.CreateDirectory(feed);
            using (var zip = new ZipArchive(File.Create(Path.Combine(feed, "pascalabc.testdependency.1.0.0.nupkg")), ZipArchiveMode.Create))
            {
                using (var writer = new StreamWriter(zip.CreateEntry("PascalABC.TestDependency.nuspec").Open()))
                    writer.Write("<package><metadata><id>PascalABC.TestDependency</id><version>1.0.0</version><authors>Tests</authors><description>Local</description></metadata></package>");
                using (var input = File.OpenRead(Path.Combine(Path.GetDirectoryName(testAssembly), "fixtures", "lib", "TestDependency.dll")))
                using (var output = zip.CreateEntry("lib/net10.0/TestDependency.dll").Open()) input.CopyTo(output);
            }
            File.WriteAllText(Path.Combine(root, "NuGet.Config"), "<configuration><packageSources><clear/><add key=\"local\" value=\"" +
                System.Security.SecurityElement.Escape(feed) + "\"/></packageSources><config><add key=\"globalPackagesFolder\" value=\"" +
                System.Security.SecurityElement.Escape(packages) + "\"/></config></configuration>");
            const string source = "{$package PascalABC.TestDependency 1.0.0}\nbegin Print(NuGetRestoreFixture.Dependency.Value); end.";
            File.WriteAllText(file, source);
            string uri = new Uri(file).AbsoluteUri;
            var json = new JavaScriptSerializer();
            using (var language = new Net10LanguageClient(runtime, "dotnet", "en", manualNuGetRestore: true))
            {
                bool required = false;
                language.PackageRestoreRequiredChanged += value => { if (value.uri == uri) required = value.required; };
                await language.StartAsync();
                int pid = language.ProcessId;
                await language.NotifyAsync("textDocument/didOpen", new { textDocument = new { uri, languageId = "pascal", version = 1, text = source } });
                Task<object> Complete() => language.RequestAsync("textDocument/completion", new {
                    textDocument = new { uri }, position = new { line = 1, character = 44 } });
                await Complete();
                Check(required && !Directory.Exists(packages), "manual IDE mode opens package source without restoring or a server error");
                await language.NotifyAsync("textDocument/didChange", new { textDocument = new { uri, version = 2 }, contentChanges = new[] { new { text = source + " " } } });
                await Complete();
                Check(required && !Directory.Exists(packages) && language.IsConnected, "editing does not download packages or break the language connection");
                var result = await language.RequestAsync("pascalabc/restoreNuGetPackages", new { uri });
                Check(json.Serialize(result).IndexOf("true", StringComparison.OrdinalIgnoreCase) >= 0 && !required,
                    "explicit IDE action restores packages and clears the pending action");
                Check(json.Serialize(await Complete()).Contains("Value") && language.ProcessId == pid,
                    "completion appears immediately after explicit restore without restarting LanguageServer");

                Directory.Move(packages, packages + ".first");
                await language.NotifyAsync("textDocument/didChange", new { textDocument = new { uri, version = 3 }, contentChanges = new[] { new { text = source + "  " } } });
                await Complete();
                Check(required && !Directory.Exists(packages), "removing the cache offers restoration again without downloading");
                using (var held = await HoldRestoreLock(testAssembly, file))
                {
                    var began = new TaskCompletionSource<bool>();
                    long elapsed = 0;
                    Action<Net10RestoreProgress> progress = value => {
                        if (value.stage == "begin") began.TrySetResult(true);
                        if (value.stage == "heartbeat") Interlocked.Exchange(ref elapsed, value.elapsedMilliseconds);
                    };
                    language.RestoreProgressChanged += progress;
                    var restoring = language.RequestAsync("pascalabc/restoreNuGetPackages", new { uri });
                    await AwaitProgress(began.Task);
                    await Task.Delay(6000);
                    Check(!restoring.IsCompleted && Interlocked.Read(ref elapsed) >= 5000,
                        "manual restore streams real time beyond the progress and cancellation thresholds");
                    await language.CancelPackageRestoreAsync();
                    try { await WithProgressTimeout(restoring); throw new Exception("Manual restore ignored cancellation"); }
                    catch (OperationCanceledException) { }
                    held.StandardInput.WriteLine("release"); held.StandardInput.Flush();
                    language.RestoreProgressChanged -= progress;
                }
                result = await language.RequestAsync("pascalabc/restoreNuGetPackages", new { uri });
                Check(!required && json.Serialize(await Complete()).Contains("Value") && language.ProcessId == pid,
                    "explicit restore retries after cancellation in the same server");

                Directory.Move(packages, packages + ".second");
                await language.NotifyAsync("textDocument/didChange", new { textDocument = new { uri, version = 4 }, contentChanges = new[] { new { text = source + "   " } } });
                await Complete();
                Check(required, "second cache removal is detected");
                using (var compiler = new Net10ControllerClient(runtime, "dotnet"))
                {
                    var compiled = await compiler.CompileAsync(file, root, "__RedirectIOMode");
                    Check(compiled.success && Directory.Exists(packages), "compilation still restores missing packages automatically: " + compiled.message);
                    Check((await RunOutput(compiled.outputFile, root)).Trim() == "42", "automatically restored program still runs");
                }
                await language.RequestAsync("pascalabc/refreshNuGetPackages", new { uri });
                Check(!required && json.Serialize(await Complete()).Contains("Value"),
                    "IDE refresh picks up packages installed by compilation without another explicit restore");
                await language.NotifyAsync("textDocument/didChange", new { textDocument = new { uri, version = 5 },
                    contentChanges = new[] { new { text = source.Replace("1.0.0}", "9.9.9}") } } });
                await Complete();
                Check(required, "unavailable package version enables the explicit action rather than disconnecting LSP");
                result = await language.RequestAsync("pascalabc/restoreNuGetPackages", new { uri });
                var failed = json.Serialize(result);
                Check(failed.Contains("false") && failed.Contains("NU1102") && language.IsConnected,
                    "failed explicit restore reports diagnostics without claiming success or losing the server");
                await language.StopAsync();
            }
        }
        finally
        {
            string restore = RestoreDirectoryFor(file);
            if (Directory.Exists(restore)) Directory.Delete(restore, true);
            if (Directory.Exists(root)) Directory.Delete(root, true);
        }
    }
}
