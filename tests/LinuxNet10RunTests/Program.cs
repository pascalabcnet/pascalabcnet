using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using VisualPascalABC;
using VisualPascalABCPlugins;

class Program
{
    static void Check(bool value, string message) { if (!value) throw new Exception(message); Console.WriteLine("PASS " + message); }
    sealed class Handler : IIdeCommandHandler
    {
        public readonly List<VisualEnvironmentCompilerAction> Actions = new List<VisualEnvironmentCompilerAction>();
        public bool DebugRequested;
        public bool TryExecute(VisualEnvironmentCompilerAction action, object context) { Actions.Add(action); if (context is IdeRunCommand run) DebugRequested = run.DebugRequested; return true; }
    }
    sealed class Proxy : System.Runtime.Remoting.Proxies.RealProxy
    {
        readonly Func<string,object> get;
        public Proxy(Type type, Func<string,object> get) : base(type) { this.get=get; }
        public override System.Runtime.Remoting.Messaging.IMessage Invoke(System.Runtime.Remoting.Messaging.IMessage message)
        {
            var call=(System.Runtime.Remoting.Messaging.IMethodCallMessage)message;
            return new System.Runtime.Remoting.Messaging.ReturnMessage(get(call.MethodName),null,0,call.LogicalCallContext,call);
        }
    }
    static void Commands()
    {
        Check(!WorkbenchServiceFactory.CommandRouter.TryExecute(VisualEnvironmentCompilerAction.Run), "no handler leaves the legacy path available");
        Check(WorkbenchServiceFactory.ExternalRunService == null, "startup external lookup does not construct the legacy run service");
        var build = (WorkbenchBuildService)FormatterServices.GetUninitializedObject(typeof(WorkbenchBuildService));
        var run = (WorkbenchRunService)FormatterServices.GetUninitializedObject(typeof(WorkbenchRunService));
        typeof(WorkbenchRunService).GetField("DocumentService", BindingFlags.Instance|BindingFlags.NonPublic).SetValue(run,
            new Proxy(typeof(IWorkbenchDocumentService), name=>null).GetTransparentProxy());
        typeof(WorkbenchRunService).GetField("Workbench", BindingFlags.Instance|BindingFlags.NonPublic).SetValue(run,
            new Proxy(typeof(IWorkbench), name=>name=="get_UserOptions" ? new UserOptions() : null).GetTransparentProxy());
        var handler = new Handler();
        using (WorkbenchServiceFactory.CommandRouter.Register(handler))
        {
            Check(build.Build() && build.Rebuild(), "Linux Build/Rebuild are intercepted before the legacy compiler");
            build.StartCompile(false);
            Check(run.Run(true), "ordinary Run uses the common command route");
            Check(run.Run(null, true, false, false) && handler.DebugRequested, "debug requests stay distinguishable from ordinary Run");
            Check(run.Stop() && handler.Actions.Last() == VisualEnvironmentCompilerAction.Stop, "Linux Stop uses the common route");
            Check(handler.Actions.Count == 6, "each command is delivered once");
        }
        Check(!WorkbenchServiceFactory.CommandRouter.TryExecute(VisualEnvironmentCompilerAction.Build), "unregistration restores legacy dispatch");
        Check(typeof(IWorkbenchCommandService).IsAssignableFrom(typeof(WorkbenchServiceContainer)) && typeof(IExternalRunService).IsAssignableFrom(typeof(WorkbenchRunService)) && typeof(IExternalRunInputService).IsAssignableFrom(typeof(WorkbenchRunService)), "Linux exposes existing command/lifecycle/input contracts");
    }
    static void Lifecycle()
    {
        var manager = new RunManager(id => {});
        int starts=0, exits=0, stops=0; string output=""; string file=Path.GetFullPath("external-net10.exe");
        manager.Starting += id => { Check(manager.IsRun(id), "registered before Starting"); starts++; };
        manager.Exited += id => { Check(!manager.IsRun(id), "removed before Exited"); exits++; };
        manager.OutputStringReceived += (id, stream, text) => output += text;
        manager.ChangeArgsBeforeRun += (ref string args) => args += " teacher";
        Check(manager.PrepareExternalArguments("[REDIRECTIOMODE]").EndsWith(" teacher"), "TeacherControl argument hook is retained");
        using(var session = new ExternalRunSession(() => manager.ExternalStarted(file, () => stops++), () => manager.ExternalExited(file), error => {}, text => manager.ExternalOutput(file,text)))
        {
            session.Started(); session.Started(); session.WriteOutput("Привет"); manager.Stop(file); manager.KillAll();
            session.Dispose(); session.Dispose(); session.WriteOutput("late");
        }
        Check(starts==1 && exits==1 && stops==2 && output=="Привет" && manager.Count==0, "external run has one lifecycle, Unicode output, Stop/KillAll and no late output");
        var plugin=Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"CompileNet10Plugin.dll"));
        Check(!plugin.GetReferencedAssemblies().Any(a=>a.Name=="PascalABCNET" || a.Name=="PascalABCNETLinux"), "Linux plugin has no shell assembly dependency");
        Check(plugin.GetTypes().Any(t=>t.Name=="Net10Hover") && plugin.GetTypes().Any(t=>t.Name=="Net10Navigation"), "Linux plugin includes hover and navigation");
        Check(Net10RuntimeAvailability.HasStableDesktopRuntime("Microsoft.NETCore.App 10.0.0 [/usr/share/dotnet/shared/Microsoft.NETCore.App]"), "Linux needs Core runtime without WindowsDesktop");
        Check(!Net10RuntimeAvailability.HasStableDesktopRuntime("Microsoft.NETCore.App 9.0.1 [/tmp/core]"), "wrong runtime does not activate modern mode");
    }
    static async Task<string> Run(Net10CompileResponse response, string root, string input=null, bool stop=false, Action<RuntimeExceptionInfo> error=null)
    {
        var output=new StringBuilder();
        using(var runner=new Net10ProgramRunner("dotnet",response.outputFile,root,""))
        {
            var task=runner.RunAsync(s=>{lock(output)output.Append(s);}, ()=>{ if(input!=null)runner.SendInputAsync(input).GetAwaiter().GetResult(); }, exception:error);
            if(stop){await Task.Delay(200);runner.Stop();}
            if(await Task.WhenAny(task,Task.Delay(30000))!=task){runner.Stop();throw new TimeoutException("Program did not exit");}
            int code=await task;
            if(!stop && error==null)Check(code==0,"compiled application exits successfully");
            if(stop)Check(runner.WasStopped,"Stop terminates the .NET process");
        }
        return output.ToString();
    }
    static async Task Pipeline(string kit)
    {
        string root=Path.Combine(Path.GetTempPath(),"PABC-Linux-net10-"+Guid.NewGuid().ToString("N")); Directory.CreateDirectory(root);
        string main=Path.Combine(root,"Main.pas"), unit=Path.Combine(root,"A.pas"), output=Path.Combine(root,"net10-output");
        File.WriteAllText(unit,"unit A; procedure p1; begin Print(2) end; end.");
        using(var client=new Net10ControllerClient(kit,"dotnet"))
        {
            var sources=new List<Net10SourceFile>{new Net10SourceFile{fileName=main,text="uses A; begin p1 end."}, new Net10SourceFile{fileName=unit,text="unit A; procedure p1; begin Print(3) end; end."}};
            var result=await client.CompileAsync(main,output,"__RedirectIOMode",sources,debug:true);
            Check(result.success,"controller compiles unsaved main and unit snapshots");
            Check((await Run(result,root)).Trim()=="3" && File.ReadAllText(unit).Contains("Print(2)"),"execution uses unsaved unit without overwriting disk");
            sources.RemoveAt(1);sources[0].text="begin var n := ReadInteger; Print('Привет ', n) end.";
            result=await client.CompileAsync(main,output,"__RedirectIOMode",sources,debug:true);
            string readOutput=await Run(result,root,"7");
            Check(result.success && readOutput.Contains("Привет") && readOutput.TrimEnd().EndsWith("7"),"standard input and Cyrillic output round-trip: "+readOutput);
            sources[0].text="begin while true do System.Threading.Thread.Sleep(10) end.";
            result=await client.CompileAsync(main,output,"__RedirectIOMode",sources);
            Check(result.success,"long-running program compiles");await Run(result,root,stop:true);
            sources[0].text="begin\n var x := ;\nend.";
            result=await client.CompileAsync(main,output,"__RedirectIOMode",sources);
            Check(!result.success && result.diagnostics.Any(d=>d.line==2),"compiler error retains source line");
            sources[0].text="begin\n var n := ReadInteger;\n Print(10 div n)\nend.";
            result=await client.CompileAsync(main,output,"__RedirectIOMode",sources,debug:true);
            RuntimeExceptionInfo exception=null;await Run(result,root,"0",error:e=>exception=e);
            Check(exception!=null && exception.Frames.Any(f=>f.Line==3 && Path.GetFileName(f.FileName)=="Main.pas"),"runtime exception has message and source line");
        }
        Console.WriteLine("Test artifacts: "+root);
    }
    [STAThread] static int Main(string[] args)
    {
        try{Commands();Lifecycle();if(args.Length>0)Pipeline(Path.GetFullPath(args[0])).GetAwaiter().GetResult();return 0;}
        catch(Exception e){Console.Error.WriteLine(e);return 1;}
    }
}
