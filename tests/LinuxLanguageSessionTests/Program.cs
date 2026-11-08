using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using VisualPascalABCPlugins;

partial class Program
{
    static void Check(bool value,string text){if(!value)throw new Exception(text);Console.WriteLine("PASS "+text);}
    sealed class Document : Control, ICodeFileDocument
    {
        public string FileName { get; set; }
        public TextEditorControl TextEditor { get; }
        public Document(TextEditorControl editor=null){TextEditor=editor ?? new TextEditorControl();}
        public override string Text { get=>TextEditor.Document.TextContent; set=>TextEditor.Document.TextContent=value; }
        public string EXEFileName=>null; public int LinesCount=>TextEditor.Document.TotalNumberOfLines;
        public bool FromMetadata=>false; public bool DocumentChanged=>true;
        public string ToolTipText{get;set;} public bool Run{get;set;}
        public void LoadFromFile(string path){Text=File.ReadAllText(path);}
        protected override void Dispose(bool value){if(value)TextEditor.Dispose();base.Dispose(value);}
    }
    sealed class Message { internal string Method; internal Dictionary<string,object> Args; }
    sealed class Client : INet10LanguageClient
    {
        public bool IsConnected{get;private set;}=true; public bool IsRestoringPackages=>false;
        public event Action<Exception> ConnectionLost;
        public event Action<Net10RestoreProgress> RestoreProgressChanged{add{}remove{}}
        public event Action<Net10PackageRestoreRequired> PackageRestoreRequiredChanged{add{}remove{}}
        internal readonly List<Message> Sent=new List<Message>(); internal int Starts;
        public Task StartAsync(){Starts++;return Task.CompletedTask;}
        public Task NotifyAsync(string method,object args){Sent.Add(new Message{Method=method,Args=new JavaScriptSerializer().Deserialize<Dictionary<string,object>>(new JavaScriptSerializer().Serialize(args))});return Task.CompletedTask;}
        internal Func<Task<object>> Completion, Signature, Hover, Navigation;
        internal Func<string, Task<object>> PackageDiscovery;
        public Task<object> RequestAsync(string method,object args,CancellationToken cancellation=default(CancellationToken))
        {
            if (method == "pascalabc/searchNuGetPackages" || method == "pascalabc/nugetPackageVersions")
                return PackageDiscovery?.Invoke(method) ?? Task.FromResult<object>(null);
            return (method=="textDocument/signatureHelp" ? Signature : method=="textDocument/hover" ? Hover : method=="textDocument/implementation" || method=="textDocument/definition" ? Navigation : Completion)?.Invoke() ?? Task.FromResult<object>(null);
        }
        public Task StopAsync(){IsConnected=false;return Task.CompletedTask;}
        public Task CancelPackageRestoreAsync()=>Task.CompletedTask;
        public void Dispose(){IsConnected=false;}
        internal void Crash(){IsConnected=false;ConnectionLost?.Invoke(new IOException("test crash"));}
    }
    static void Wait(Task task){var watch=Stopwatch.StartNew();while(!task.IsCompleted){if(watch.ElapsedMilliseconds>30000)throw new TimeoutException();Application.DoEvents();Thread.Sleep(5);}task.GetAwaiter().GetResult();Application.DoEvents();}
    static string UriOf(Document doc)=>new Uri(Path.GetFullPath(doc.FileName)).AbsoluteUri;
    static string MessageUri(Message msg)=>(string)((Dictionary<string,object>)msg.Args["textDocument"])["uri"];
    static void Session()
    {
        using(var form=new Form{Width=800,Height=450,ShowInTaskbar=false,Opacity=0})
        using(var first=new Document{FileName=Path.GetFullPath("Main unsaved.pas"),Text="begin end."})
        using(var unit=new Document{FileName=Path.GetFullPath("A.pas"),Text="unit A; end."})
        using(var lower=new Document{FileName=Path.GetFullPath("a.pas"),Text="unit a; end."})
        {
            var strip=new StatusStrip();form.Controls.Add(strip);
            var open=new List<ICodeFileDocument>{first,unit};var events=new WorkbenchDocumentEventSource(()=>open);
            bool active=false;Client current=null;var clients=new List<Client>();
            using(var session=new Net10LinuxLanguageSession(form,events,()=>active,()=>{current=new Client();clients.Add(current);return current;}))
            {
                session.UpdateActivity();Check(clients.Count==0,"legacy mode starts no LSP");
                active=true;session.UpdateActivity();Wait(session.Documents.WhenIdleAsync());
                form.Show();Application.DoEvents();
                Check(clients.Count==1 && current.Sent.Count(m=>m.Method=="textDocument/didOpen")==2,"one server receives active and inactive unsaved tabs");
                var label=strip.Items["LinuxLanguageServerStatus"];
                Check(label.Text=="LSP: 2 файлов","separate status counter confirms synchronized tabs");
                first.Text="begin Print(3) end.";session.Documents.Flush();Wait(session.Documents.WhenIdleAsync());
                Check(current.Sent.Last().Method=="textDocument/didChange","unsaved editing sends didChange");
                string previous=first.FileName;first.FileName=Path.GetFullPath("Новое имя.pas");events.Renamed(first,previous);Wait(session.Documents.WhenIdleAsync());
                var rename=current.Sent.Skip(current.Sent.Count-2).ToArray();
                Check(rename[0].Method=="textDocument/didClose" && rename[1].Method=="textDocument/didOpen" && MessageUri(rename[1])==UriOf(first),"Save As closes old URI and opens Cyrillic new URI");
                int count=current.Sent.Count;session.Documents.Flush();Wait(session.Documents.WhenIdleAsync());
                Check(current.Sent.Count==count && open.Contains(first),"cancelled close leaves the existing snapshot and tab intact");
                open.Add(lower);events.Opened(lower);Wait(session.Documents.WhenIdleAsync());
                Check(current.Sent.Last().Method=="textDocument/didOpen" && MessageUri(current.Sent.Last())==UriOf(lower),"Linux transport retains paths differing only by case");
                open.Remove(unit);events.Closed(unit);Wait(session.Documents.WhenIdleAsync());
                Check(current.Sent.Last().Method=="textDocument/didClose" && MessageUri(current.Sent.Last())==UriOf(unit),"closing one tab removes only its snapshot");
                var old=current;old.Crash();first.Text="begin Print(4) end.";session.Documents.Flush();Wait(session.Documents.WhenIdleAsync());
                Check(clients.Count==2 && current.Sent.Count(m=>m.Method=="textDocument/didOpen")==2,"next edit recreates a lost server and replays all current snapshots");
                active=false;session.UpdateActivity();Wait(session.Documents.WhenIdleAsync());
                Check(current.Sent.Count(m=>m.Method=="textDocument/didClose")==2 && !label.Visible,"returning to legacy mode closes overlays and hides LSP counter");
                int messages=current.Sent.Count;first.Text="begin Print(5) end.";session.Documents.Flush();Wait(session.Documents.WhenIdleAsync());
                Check(messages==current.Sent.Count,"legacy edits are not sent while inactive");
                active=true;session.UpdateActivity();Wait(session.Documents.WhenIdleAsync());
                Check(clients.Count==2,"reactivation reuses the live server");
            }
            Check(clients.All(c=>!c.IsConnected),"session disposal leaves no server alive");
        }
    }
    static bool HasLabel(object response,string label)
    {
        var obj=response as Dictionary<string,object>;IEnumerable items=response as IEnumerable;
        if(obj!=null && obj.TryGetValue("items",out var value))items=value as IEnumerable;
        return items!=null && items.Cast<object>().OfType<Dictionary<string,object>>().Any(item=>item.TryGetValue("label",out var text) && (string)text==label);
    }
    static void Server(string kit)
    {
        string root=Path.Combine(Path.GetTempPath(),"Linux-LSP-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(root);
        using(var doc=new Document{FileName=Path.Combine(root,"Main unsaved.pas"),Text="begin var s := 'abc'; s.Substring(1); end."})
        {
            var open=new List<ICodeFileDocument>{doc};var events=new WorkbenchDocumentEventSource(()=>open);Net10LanguageClient client=null;
            var sync=new Net10LanguageDocuments(events,()=>client=new Net10LanguageClient(kit,"dotnet","ru",true),Console.WriteLine);
            try
            {
                sync.SetActive(true);Wait(sync.WhenIdleAsync());
                var request=sync.CompleteAsync(doc,0,doc.Text.IndexOf("s.",StringComparison.Ordinal)+2,CancellationToken.None);Wait(request);
                if(!HasLabel(request.Result,"Length")) Console.WriteLine("Response: "+new JavaScriptSerializer().Serialize(request.Result));
                Check(HasLabel(request.Result,"Length"),"real server analyzes unsaved source from snapshots");
                doc.Text="begin var s := 42; s.ToString; end.";
                request=sync.CompleteAsync(doc,0,doc.Text.IndexOf("s.",StringComparison.Ordinal)+2,CancellationToken.None);Wait(request);
                Check(!HasLabel(request.Result,"Length") && HasLabel(request.Result,"ToString"),"changed unsaved type reaches real server without saving");
                doc.Text="begin\n  var s := 'abc';\n  s.\nend.";
                request=sync.CompleteAsync(doc,2,4,CancellationToken.None);Wait(request);
                Check(HasLabel(request.Result,"Length"),"real server completes immediately after an unfinished dot expression");
                doc.Text="begin var s := 123; s. end.";
                request=sync.CompleteAsync(doc,0,doc.Text.IndexOf("s.",StringComparison.Ordinal)+2,CancellationToken.None);Wait(request);
                Check(!HasLabel(request.Result,"Length") && HasLabel(request.Result,"ToString"),"unfinished expression uses the latest integer declaration");
                using(var fresh=new Document{FileName=Path.Combine(root,"Fresh.pas"),Text="begin var s := 'fresh'; s. end."})
                {
                    open.Add(fresh); events.Opened(fresh);
                    request=sync.CompleteAsync(fresh,0,fresh.Text.IndexOf("s.",StringComparison.Ordinal)+2,CancellationToken.None);Wait(request);
                    Check(HasLabel(request.Result,"Length"),"unfinished dot works without a previous successful model");
                    open.Remove(fresh); events.Closed(fresh);
                }
                Check(!File.Exists(doc.FileName),"LSP does not create or save source files");
                using(var process=Process.GetProcessById(client.ProcessId)){process.Kill();process.WaitForExit();}
                doc.Text="begin var s := 'updated'; s.Substring(1); end.";
                sync.Flush();Wait(sync.WhenIdleAsync());request=sync.CompleteAsync(doc,0,doc.Text.IndexOf("s.",StringComparison.Ordinal)+2,CancellationToken.None);Wait(request);
                Check(HasLabel(request.Result,"Length"),"real server recovers after process termination");
            }
            finally {Wait(sync.ShutdownAsync());}
            Check(!client.IsConnected,"real LSP process shuts down");
        }
    }
    [STAThread]static int Main(string[] args){try{KeyRegressions();Session();DotCompletion();SignatureCompletion();HoverCompletion();NavigationCompletion();DocumentSymbols();if(args.Length>0){Server(Path.GetFullPath(args[0]));RealSignature(Path.GetFullPath(args[0]));HoverCompletion(Path.GetFullPath(args[0]));NavigationCompletion(Path.GetFullPath(args[0]));DocumentSymbols(Path.GetFullPath(args[0]));}return 0;}catch(Exception e){Console.Error.WriteLine(e);return 1;}}
}
