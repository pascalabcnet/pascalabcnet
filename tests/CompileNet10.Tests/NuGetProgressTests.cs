using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using PascalABCNet.Protocol;
using VisualPascalABCPlugins;

internal static partial class Program
{
    private static void CheckRestoreDeadlines()
    {
        long time = 0;
        var activity = new RestoreActivity(() => time);
        var deadline = activity.BeginRequest(10000);
        time = 9000;
        Check(!deadline.Expired, "ordinary request retains its ten-second deadline");
        activity.Observe("a", "begin");
        for (int i = 0; i < 40; i++)
        { time += 1000; activity.Observe("a", "heartbeat"); Check(!deadline.Expired, "live restore pauses deadline " + i); }
        var duringRestore = activity.BeginRequest(10000);
        activity.Observe("a", "begin"); // Duplicate must not reset the busy interval.
        time += 1000; activity.Observe("b", "begin");
        activity.Observe("a", "end");
        time += 1000; activity.Observe("b", "heartbeat");
        Check(!deadline.Expired && !duringRestore.Expired, "overlapping and duplicate notifications keep correct accounting");
        activity.Observe("b", "end");
        time += 1001;
        Check(deadline.Expired && !duringRestore.Expired, "ordinary total timeout resumes after NuGet finishes");
        activity.Observe("dead", "begin");
        var dead = activity.BeginRequest(10000);
        time += 10001;
        Check(dead.Expired, "missing heartbeat still times out a dead server");
        activity.Observe("dead", "end");
        var ordinary = activity.BeginRequest(10000);
        time += 10001;
        activity.Observe(null, "heartbeat");
        activity.Observe("unknown", "heartbeat");
        Check(ordinary.Expired, "unrelated compiler events cannot extend timeouts");
    }

    private static string ProgressResource(string key)
    {
        switch (key)
        {
            case "NUGET_RESOLVING{0}": return "NuGet: resolving dependencies… · {0} s";
            case "NUGET_INSTALLED{0}{1}{2}": return "NuGet: {2} s · installed: {0} · {1}";
            case "NUGET_CANCEL": return "Cancel";
            case "NUGET_RESTORE_PACKAGES": return "Restore packages";
            case "NUGET_RESTORED": return "Packages restored";
            default: return key;
        }
    }

    private static void CheckRestoreStatus()
    {
        using (var form = new Form { Width = 850 })
        using (var strip = new StatusStrip { Name = "statusStrip1" })
        {
            var label = new ToolStripStatusLabel { Name = "toolStripStatusLabel5", Text = "Ready", Spring = true,
                ToolTipText = "Original tooltip" };
            strip.Items.Add(label); form.Controls.Add(strip);
            using (var status = new Net10StatusDisplay(form, ProgressResource))
            {
                int cancellations = 0;
                int changes = 0;
                label.TextChanged += (sender, e) => changes++;
                foreach (string source in new[] { "compiler", "language" })
                {
                    var fast = new Net10RestoreProgress { operationId = "cached", stage = "begin" };
                    status.ShowRestoreProgress(source, fast, () => cancellations++);
                    fast.stage = "heartbeat"; fast.elapsedMilliseconds = 1999;
                    status.ShowRestoreProgress(source, fast, () => cancellations++);
                    fast.stage = "end";
                    status.ShowRestoreProgress(source, fast, null);
                    Check(changes == 0 && label.Text == "Ready" && strip.Items.Count == 1 &&
                        label.ToolTipText == "Original tooltip" && cancellations == 0,
                        source + " cached restore stays completely silent before two seconds, including completion");
                }
                var installedFast = new Net10RestoreProgress { operationId = "quick-install", stage = "begin" };
                status.ShowRestoreProgress("compiler", installedFast, null);
                installedFast.stage = "installed"; installedFast.installedCount = 1;
                installedFast.packageId = "Quick.Package"; installedFast.elapsedMilliseconds = 500;
                status.ShowRestoreProgress("compiler", installedFast, null);
                installedFast.stage = "end";
                status.ShowRestoreProgress("compiler", installedFast, null);
                Check(changes == 0 && strip.Items.Count == 1,
                    "a quick installation also avoids flashing temporary progress");
                var value = new Net10RestoreProgress { operationId = "a", stage = "begin" };
                status.ShowRestoreProgress("compiler", value, () => cancellations++);
                Check(strip.Items.Count == 1 && label.Text == "Ready", "restore begin does not flash a status or controls");
                value.stage = "heartbeat"; value.elapsedMilliseconds = 2000;
                status.ShowRestoreProgress("compiler", value, () => cancellations++);
                var cancel = (ToolStripButton)strip.Items["Net10NuGetCancel"];
                Check(label.Text.Contains("2 s") && !cancel.Available && strip.Items.Count == 3,
                    "progress appears at two seconds on the existing strip, without a cancel button yet");
                value.stage = "heartbeat"; value.elapsedMilliseconds = 4999;
                status.ShowRestoreProgress("compiler", value, () => cancellations++);
                Check(label.Text.Contains("4 s") && !cancel.Available, "elapsed time ticks before the cancel threshold");
                value.elapsedMilliseconds = 5000;
                status.ShowRestoreProgress("compiler", value, () => cancellations++);
                Check(cancel.Available && label.Text.Contains("5 s"), "Cancel appears at five seconds");
                cancel.PerformClick(); cancel.PerformClick();
                Check(cancellations == 1 && !cancel.Enabled, "Cancel is isolated, single-shot and immediately disabled");
                value.stage = "end";
                status.ShowRestoreProgress("compiler", value, null);
                Check(strip.Items.Count == 1 && label.Text == "Ready" && label.ToolTipText == "Original tooltip",
                    "end removes temporary controls and restores prior status and tooltip");
                value = new Net10RestoreProgress { operationId = "b", stage = "begin", installedCount = 3,
                    packageId = "Example 1.2.3", elapsedMilliseconds = 7000 };
                status.ShowRestoreProgress("language", value, null);
                Check(label.Text.Contains("installed: 3") && label.Text.Contains("Example 1.2.3") && label.Text.Contains("7 s"),
                    "status shows actual package count, identity and elapsed time without a fake percentage");
                int height = strip.Height;
                form.Width = 420;
                value.packageId = "Very.Long.Package." + new string('W', 160);
                status.ShowRestoreProgress("language", value, null);
                Check(strip.Height == height && label.Text.EndsWith("…") && label.Text.Contains("7 s") &&
                    label.ToolTipText.Contains(value.packageId),
                    "narrow status strip stays one row; long package is abbreviated with full identity in tooltip");
                label.Text = "New ordinary compilation";
                status.ClearRestores("language");
                Check(label.Text == "New ordinary compilation", "late restore completion preserves a newer ordinary IDE status");
                status.SetCompilationActive(true);
                status.ShowRestoreProgress("language", new Net10RestoreProgress { operationId = "c", stage = "begin",
                    elapsedMilliseconds = 7000 }, null);
                Check(strip.Items.Count == 1, "background language restore cannot cover foreground compilation");
                status.ClearRestores("language");
                status.SetCompilationActive(false);
                status.SetLanguageRestoreVisible(false);
                status.ShowRestoreProgress("language", new Net10RestoreProgress { operationId = "hidden", stage = "begin",
                    elapsedMilliseconds = 7000 }, null);
                Check(strip.Items.Count == 1, "inactive editor's long restore remains hidden");
                status.SetLanguageRestoreVisible(true);
                Check(strip.Items.Count == 3 && label.Text.Contains("7 s"),
                    "returning to an editor shows its already long-running restore");
                status.ClearRestores("language");
                var delayed = new Net10RestoreProgress { operationId = "disposed", stage = "begin" };
                status.ShowRestoreProgress("compiler", delayed, null);
                status.ClearRestores("compiler");
                delayed.stage = "heartbeat"; delayed.elapsedMilliseconds = 7000;
                status.ShowRestoreProgress("compiler", delayed, null);
                Check(strip.Items.Count == 1, "a late heartbeat cannot resurrect cleared, never-shown progress");
                int restoreClicks = 0;
                status.SetPackageRestoreRequired(() => true, () => restoreClicks++);
                var restoreButton = (ToolStripButton)strip.Items["Net10NuGetRestore"];
                Check(restoreButton.Available && label.Text == "New ordinary compilation",
                    "missing packages offer a button without replacing the status text");
                restoreButton.PerformClick();
                Check(restoreClicks == 1, "restore button invokes the explicit action");
                status.SetManualRestoreActive(true);
                Check(!restoreButton.Available, "explicit request hides the action immediately, even before progress appears");
                status.SetPackageRestoreRequired(() => false, null);
                status.Start(ProgressResource("NUGET_RESTORED"));
                status.SetManualRestoreActive(false);
                Check(!restoreButton.Available && label.Text == "Packages restored",
                    "even fast explicit restore acknowledges completion in the existing status line");
                status.SetPackageRestoreRequired(() => true, null);
                status.SetCompilationActive(true);
                Check(!restoreButton.Available, "compilation hides the manual package action");
                status.SetCompilationActive(false);
                status.SetLanguageRestoreVisible(false);
                Check(!restoreButton.Available, "switching away from modern IntelliSense hides the package action");
                status.SetPackageRestoreRequired(() => false, null);
                string fitted = Net10StatusDisplay.FitSingleLine("NuGet: 12 s · installed: 3 · " + new string('W', 200) + "\nnext", label.Font, 240);
                Check(!fitted.Contains("\n") && fitted.EndsWith("…") &&
                    TextRenderer.MeasureText(fitted, label.Font).Width <= 240 && fitted.Contains("12 s"),
                    "long package identity is ellipsized on one line, elapsed time remains visible");
            }
        }
    }

    private static string RestoreDirectoryFor(string file)
    {
        using (var hash = SHA256.Create())
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "PascalABC.NET", "NuGet", "Restore", BitConverter.ToString(hash.ComputeHash(
                    Encoding.UTF8.GetBytes(Path.GetFullPath(file).ToUpperInvariant()))).Replace("-", "").ToLowerInvariant());
    }

    private static async Task<Process> HoldRestoreLock(string testAssembly, string file)
    {
        var process = Process.Start(new ProcessStartInfo("dotnet", "\"" + testAssembly + "\" --hold-restore-lock \"" + RestoreDirectoryFor(file) + "\"") {
            UseShellExecute = false, CreateNoWindow = true, RedirectStandardInput = true,
            RedirectStandardOutput = true, RedirectStandardError = true
        });
        try
        {
            var ready = process.StandardOutput.ReadLineAsync();
            if (await Task.WhenAny(ready, Task.Delay(15000)) != ready || await ready != "LOCKED")
                throw new Exception("Restore lock helper failed");
            return process;
        }
        catch { if (!process.HasExited) process.Kill(); process.Dispose(); throw; }
    }

    private static async Task CheckLiveRestoreProgress(string runtime, string testAssembly)
    {
        string root = Path.Combine(Path.GetTempPath(), "pabc-progress-" + Guid.NewGuid().ToString("N"));
        var files = new List<string>();
        Directory.CreateDirectory(root);
        try
        {
            string feed = Path.Combine(root, "feed"); Directory.CreateDirectory(feed);
            using (var zip = new ZipArchive(File.Create(Path.Combine(feed, "pascalabc.testdependency.1.0.0.nupkg")), ZipArchiveMode.Create))
            {
                using (var writer = new StreamWriter(zip.CreateEntry("PascalABC.TestDependency.nuspec").Open()))
                    writer.Write("<package><metadata><id>PascalABC.TestDependency</id><version>1.0.0</version><authors>Tests</authors><description>Local tests only</description></metadata></package>");
                using (var input = File.OpenRead(Path.Combine(Path.GetDirectoryName(testAssembly), "fixtures", "lib", "TestDependency.dll")))
                using (var output = zip.CreateEntry("lib/net10.0/TestDependency.dll").Open()) input.CopyTo(output);
            }
            File.WriteAllText(Path.Combine(root, "NuGet.Config"), "<configuration><packageSources><clear/><add key=\"local\" value=\"" +
                System.Security.SecurityElement.Escape(feed) + "\"/></packageSources><config><add key=\"globalPackagesFolder\" value=\"" +
                System.Security.SecurityElement.Escape(Path.Combine(root, "packages")) + "\"/></config></configuration>");
            const string source = "{$package PascalABC.TestDependency 1.0.0}\nbegin Print(NuGetRestoreFixture.Dependency.Value); end.";
            string FileFor(string name)
            { string file = Path.Combine(root, name + ".pas"); File.WriteAllText(file, source); files.Add(file); return file; }
            using (var compiler = new Net10ControllerClient(runtime, "dotnet"))
            {
                string file = FileFor("LongCompiler");
                using (var held = await HoldRestoreLock(testAssembly, file))
                {
                    var begin = new TaskCompletionSource<bool>(); int heartbeats = 0, installs = 0;
                    var compiling = compiler.CompileAsync(file, root, progress: value => {
                        if (value.nuGetStage == "begin") begin.TrySetResult(true);
                        if (value.nuGetStage == "heartbeat") Interlocked.Increment(ref heartbeats);
                        if (value.nuGetStage == "installed") Interlocked.Increment(ref installs);
                        return Task.CompletedTask;
                    });
                    await AwaitProgress(begin.Task);
                    await Task.Delay(33000); // Exceeds Controller's original 30-second deadline.
                    Check(!compiling.IsCompleted && heartbeats >= 30, "Controller survives a long, live NuGet restore");
                    held.StandardInput.WriteLine("release"); held.StandardInput.Flush();
                    var result = await WithProgressTimeout(compiling);
                    Check(result.success && installs >= 1, "compiler recovers after long restore and streams installation counts: " + result.message);
                }
                file = FileFor("CancelledCompiler");
                using (var held = await HoldRestoreLock(testAssembly, file))
                using (var cancellation = new CancellationTokenSource())
                {
                    var begin = new TaskCompletionSource<bool>();
                    var compiling = compiler.CompileAsync(file, root, progress: value => {
                        if (value.nuGetStage == "begin") begin.TrySetResult(true); return Task.CompletedTask;
                    }, cancellation: cancellation.Token);
                    await AwaitProgress(begin.Task);
                    cancellation.Cancel();
                    try { await WithProgressTimeout(compiling); throw new Exception("Compiler ignored cancellation"); }
                    catch (OperationCanceledException) { }
                    held.StandardInput.WriteLine("release"); held.StandardInput.Flush();
                }
                var retry = await compiler.CompileAsync(file, root);
                Check(retry.success, "compiler can run again after explicit cancellation: " + retry.message);
            }
            using (var language = new Net10LanguageClient(runtime, "dotnet", "en"))
            {
                await language.StartAsync();
                foreach (bool cancel in new[] { false, true })
                {
                    string file = FileFor(cancel ? "CancelledLanguage" : "LongLanguage");
                    string uri = new Uri(file).AbsoluteUri;
                    var begin = new TaskCompletionSource<bool>();
                    int heartbeats = 0;
                    Action<Net10RestoreProgress> handler = value => {
                        if (value.stage == "begin") begin.TrySetResult(true);
                        if (value.stage == "heartbeat") Interlocked.Increment(ref heartbeats);
                    };
                    language.RestoreProgressChanged += handler;
                    using (var held = await HoldRestoreLock(testAssembly, file))
                    {
                        await language.NotifyAsync("textDocument/didOpen", new { textDocument = new {
                            uri, languageId = "pascal", version = 1, text = source } });
                        var completion = language.RequestAsync("textDocument/completion", new {
                            textDocument = new { uri }, position = new { line = 1, character = 44 } });
                        await AwaitProgress(begin.Task);
                        if (cancel)
                        {
                            await language.CancelPackageRestoreAsync();
                            try { await WithProgressTimeout(completion); throw new Exception("LSP ignored cancellation"); }
                            catch (OperationCanceledException) { }
                        }
                        else
                        {
                            await Task.Delay(12000);
                            Check(!completion.IsCompleted && heartbeats >= 10 && language.IsConnected,
                                "IntelliSense has no false ten-second timeout while restore sends heartbeats");
                        }
                        held.StandardInput.WriteLine("release"); held.StandardInput.Flush();
                        if (!cancel) await WithProgressTimeout(completion);
                    }
                    language.RestoreProgressChanged -= handler;
                    var result = await language.RequestAsync("textDocument/completion", new {
                        textDocument = new { uri }, position = new { line = 1, character = 44 } });
                    Check(new JavaScriptSerializer().Serialize(result).Contains("Value"),
                        "LSP returns package members after long restore/cancellation without restart: " + cancel);
                    if (!cancel)
                    {
                        string packages = Path.Combine(root, "packages");
                        Check(Path.GetDirectoryName(packages) == root, "cache-removal target stays in the test's own directory");
                        Directory.Move(packages, packages + ".removed");
                        Console.WriteLine("CASE: package cache removed while LanguageServer remains alive");
                        await language.NotifyAsync("textDocument/didChange", new {
                            textDocument = new { uri, version = 2 }, contentChanges = new[] { new { text = source + " " } }
                        });
                        result = await language.RequestAsync("textDocument/completion", new {
                            textDocument = new { uri }, position = new { line = 1, character = 44 } });
                        Check(new JavaScriptSerializer().Serialize(result).Contains("Value") && language.IsConnected,
                            "same LanguageServer restores removed packages after an edit without losing completion");
                    }
                    await language.NotifyAsync("textDocument/didClose", new { textDocument = new { uri } });
                }
                await CheckExtensionCacheRemoval(language, FileFor("ExtensionCacheRemoval"), root);
                await language.StopAsync();
            }
        }
        finally
        {
            foreach (string file in files)
            {
                string directory = RestoreDirectoryFor(file);
                if (Directory.Exists(directory)) Directory.Delete(directory, true);
            }
            Directory.Delete(root, true);
        }
    }

    private static async Task CheckExtensionCacheRemoval(Net10LanguageClient language, string file, string root)
    {
        string uri = new Uri(file).AbsoluteUri;
        const string source = "{$package PascalABC.TestDependency 1.0.0}\nuses NuGetRestoreFixture;\nbegin var x := new object; Print(x.PackageCacheProbe); var y := new InheritedCacheProbe; Print(y.ToString); end.";
        File.WriteAllText(file, source);
        int pid = language.ProcessId;
        await language.NotifyAsync("textDocument/didOpen", new { textDocument = new {
            uri, languageId = "pascal", version = 1, text = source } });
        async Task Completion()
        {
            var result = await language.RequestAsync("textDocument/completion", new {
                textDocument = new { uri }, position = new { line = 2, character = "begin var x := new object; Print(x.".Length } });
            Check(new JavaScriptSerializer().Serialize(result).Contains("PackageCacheProbe"),
                "package extension on System.Object remains visible after cache replacement");
        }
        await Completion();
        for (int version = 2; version <= 5; version++)
        {
            string cache = Path.Combine(root, "packages");
            Check(Path.GetDirectoryName(cache) == root, "extension cache-removal target stays in its own temporary directory");
            Directory.Move(cache, cache + ".extensions-removed-" + version);
            await language.NotifyAsync("textDocument/didChange", new {
                textDocument = new { uri, version }, contentChanges = new[] { new { text = source + new string(' ', version) } }
            });
            if (version % 2 == 1) await Task.Delay(700);
            await Completion();
            Check(language.IsConnected && language.ProcessId == pid,
                "collectible extension cache released without restarting LanguageServer, edit " + version);
        }
        await language.NotifyAsync("textDocument/didClose", new { textDocument = new { uri } });
    }

    private static async Task CheckMlCacheRemoval(string runtime)
    {
        string root = Path.Combine(Path.GetTempPath(), "pabc-ml-cache-removal-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        string file = Path.Combine(root, "ProgressTest.pas");
        string uri = new Uri(file).AbsoluteUri;
        const string source = "{$package Microsoft.ML.TimeSeries 5.0.0}\n{$package Microsoft.ML.Mkl.Redist 5.0.0}\nbegin\nvar ml := new Microsoft.ML.MLContext;\nend.";
        File.WriteAllText(Path.Combine(root, "NuGet.Config"), "<configuration><packageSources><clear/><add key=\"nuget.org\" value=\"https://api.nuget.org/v3/index.json\"/></packageSources><config><add key=\"globalPackagesFolder\" value=\"./packages\"/></config></configuration>");
        try
        {
            using (var language = new Net10LanguageClient(runtime, "dotnet", "en"))
            {
                language.RestoreProgressChanged += value => Console.WriteLine("ML-PROGRESS: " + value.stage + " " + value.installedCount + " " + value.elapsedMilliseconds);
                await language.StartAsync();
                await language.NotifyAsync("textDocument/didOpen", new { textDocument = new {
                    uri, languageId = "pascal", version = 1, text = source } });
                async Task Completion()
                {
                    var result = await language.RequestAsync("textDocument/completion", new {
                        textDocument = new { uri }, position = new { line = 3, character = "var ml := new Microsoft.ML.".Length } });
                    Check(new JavaScriptSerializer().Serialize(result).Contains("MLContext"), "ML.NET completion in the same live server");
                }
                await Completion();
                int pid = language.ProcessId;
                for (int version = 2; version <= 6; version++)
                {
                    string cache = Path.Combine(root, "packages");
                    Check(Path.GetDirectoryName(cache) == root, "ML.NET cache-removal target stays in its own temporary directory");
                    Directory.Move(cache, cache + ".removed" + version);
                    Console.WriteLine("CASE: removed ML.NET packages; edit version " + version);
                    await language.NotifyAsync("textDocument/didChange", new {
                        textDocument = new { uri, version }, contentChanges = new[] { new { text = source + new string(' ', version) } }
                    });
                    if (version % 2 == 1) await Task.Delay(700); // Also exercise background debounce.
                    await Completion();
                    Check(language.ProcessId == pid && language.IsConnected, "ML.NET cache restored without restarting LanguageServer");
                }
                await language.StopAsync();
            }
        }
        finally
        {
            string restore = RestoreDirectoryFor(file);
            if (Directory.Exists(restore)) Directory.Delete(restore, true);
            Directory.Delete(root, true);
        }
    }

    private static async Task AwaitProgress(Task task)
    { if (await Task.WhenAny(task, Task.Delay(30000)) != task) throw new Exception("No NuGet begin notification"); await task; }

    private static async Task<T> WithProgressTimeout<T>(Task<T> task)
    { if (await Task.WhenAny(task, Task.Delay(15000)) != task) throw new Exception("Cancellation/recovery timed out"); return await task; }
}
