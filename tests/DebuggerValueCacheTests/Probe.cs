using System;
using System.Windows.Forms;
using Debugger;
class Probe {
 static bool done,busy; static int code=1;
 [STAThread] static int Main(string[] args) {
 var d=new NDebugger();
 d.ProcessStarted+=(s,e)=>e.Process.DebuggingPaused+=(s2,e2)=>{
 if(busy || done)return;busy=true;
 var p=e2.Process;
 try {
 Console.WriteLine("PAUSED "+p.PausedReason);
 var t=DebugType.Create(p,null,"Fixture");
 var root=((Debugger.FieldInfo)t.GetMember("Root",Debugger.BindingFlags.All)[0]).GetValue(null).GetPermanentReference();
 Console.WriteLine("Before: "+root.GetMember("Condition").Type.FullName);
 var type=root.Type;
 while(type.GetMember("ToString",Debugger.BindingFlags.All).Count==0)type=type.BaseType;
 var tostring=(Debugger.MethodInfo)type.GetMember("ToString",Debugger.BindingFlags.All)[0];
 Console.WriteLine("Eval: "+tostring.Invoke(root,new Value[0]).AsString);
 for(int i=0;i<3;i++) {
 foreach(var name in new[]{"Condition","ThenStat","ElseStat"}) {
 var child=root.GetMember(name);
 if(child.Type.FullName!="Node" || (int)child.GetMember("Number").PrimitiveValue!=42)
 throw new System.Exception("Incorrect nested value: "+name);
 }
 if((int)root.GetMember("Number").PrimitiveValue!=42)throw new System.Exception("Incorrect primitive value");
 tostring.Invoke(root,new Value[0]);
 }
 Console.WriteLine("PASS: nested fields and primitive values survive repeated evaluation and GC");
 code=0;
 }catch(System.Exception ex){Console.WriteLine(ex.ToString());}
 finally{done=true;p.Terminate();}
 };
 d.Start(args[0],System.IO.Path.GetDirectoryName(args[0]),"");
 var until=DateTime.UtcNow.AddSeconds(25);
 while(!done && DateTime.UtcNow<until){Application.DoEvents();System.Threading.Thread.Sleep(10);}
 if(!done){Console.WriteLine("FAIL: debugger timeout");foreach(var p in d.Processes)p.Terminate();}
 return code;
 }
}


