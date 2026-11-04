using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisualPascalABCPlugins;

internal static partial class Program
{
    private static void CheckLanguageRecovery()
    {
        using (var main = new EditorDocument { FileName = Path.GetFullPath("RecoveryMain.pas") })
        using (var unit = new EditorDocument { FileName = Path.GetFullPath("RecoveryUnit.pas") })
        {
            var events = new WorkbenchDocumentEventSource(() => new ICodeFileDocument[] { main, unit });
            var clients = new List<FakeLanguageClient>();
            DateTime now = new DateTime(2026, 10, 31, 12, 0, 0, DateTimeKind.Utc);
            var sync = new Net10LanguageDocuments(events, () =>
            {
                var client = new FakeLanguageClient { FailNext = clients.Count < 3 };
                clients.Add(client); return client;
            }, _ => { }, () => now);
            try
            {
                sync.SetActive(true); sync.WhenIdleAsync().GetAwaiter().GetResult();
                for (int i = 0; i < 2; i++) { sync.Flush(); sync.WhenIdleAsync().GetAwaiter().GetResult(); }
                for (int i = 0; i < 5; i++) sync.CompleteAsync(main, 0, 0, CancellationToken.None).GetAwaiter().GetResult();
                Check(clients.Count == 3 && clients.All(client => client.Disposed),
                    "three repeated connection failures pause automatic restarts despite further requests");
                main.TextEditor.Document.TextContent = "uses RecoveryUnit; begin p1 end.";
                unit.TextEditor.Document.TextContent = "unit RecoveryUnit; interface procedure p1; implementation procedure p1; begin Print(3) end; end.";
                now = now.AddSeconds(11);
                sync.CompleteAsync(main, 0, 0, CancellationToken.None).GetAwaiter().GetResult();
                var live = clients.Last();
                Check(clients.Count == 4 && live.Messages.Take(2).All(item => item.Item1 == "textDocument/didOpen") &&
                    live.Messages.Take(2).Select(item => (string)DocumentValue(item, "text")).Contains(unit.TextEditor.Document.TextContent),
                    "next request after cooldown reconnects and replays current unsaved main and unit");
                live.Request = (method, value, token) => Task.FromException<object>(new IOException("analysis request error"));
                sync.CompleteAsync(main, 0, 0, CancellationToken.None).GetAwaiter().GetResult();
                Check(!live.Disposed && clients.Count == 4, "an individual request failure retains the healthy LSP process");
                live.Request = (method, value, token) => Task.FromResult<object>("recovered");
                var result = sync.CompleteAsync(main, 0, 0, CancellationToken.None).GetAwaiter().GetResult();
                Check((string)result == "recovered" && clients.Count == 4, "next request succeeds without switching platform or restarting IDE");
            }
            finally { sync.ShutdownAsync().GetAwaiter().GetResult(); }
        }
    }
}
