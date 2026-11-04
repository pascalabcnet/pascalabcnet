using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualPascalABC;
using VisualPascalABCPlugins;

partial class Program
{
    static object ParameterResponse(int active=0)=>new Dictionary<string,object>{
        ["activeParameter"]=active,["signatures"]=new object[]{
            new Dictionary<string,object>{["label"]="function F(a: integer; b: integer): integer;",
                ["parameters"]=new object[]{new Dictionary<string,object>{["label"]="a: integer"},new Dictionary<string,object>{["label"]="b: integer"}}},
            new Dictionary<string,object>{["label"]="function F(a: integer): integer;",
                ["parameters"]=new object[]{new Dictionary<string,object>{["label"]="a: integer"}}}
        }};
    static void SignatureCompletion()
    {
        using(var form=new Form{Width=800,Height=450,ShowInTaskbar=false,Opacity=0})
        using(var doc=new Document{FileName=Path.GetFullPath("Signature.pas")})
        using(var ui=new ExternalIntelliSenseSession(form))
        {
            form.Controls.Add(doc.TextEditor);doc.TextEditor.Dock=DockStyle.Fill;form.Show();
            var area=doc.TextEditor.ActiveTextAreaControl.TextArea;area.Focus();Application.DoEvents();
            int lost=0,calls=0;area.LostFocus+=(sender,e)=>lost++;
            bool active=true,parameters=true;
            var client=new Client{Signature=()=>{calls++;return Task.FromResult(ParameterResponse());}};
            var open=new List<ICodeFileDocument>{doc};var events=new WorkbenchDocumentEventSource(()=>open);
            using(var session=new Net10LinuxLanguageSession(form,events,()=>active,()=>client,ui,()=>true,()=>parameters))
            {
                session.UpdateActivity();Wait(session.Documents.WhenIdleAsync());
                Action<string> source=text=>{ui.Hide(IntelliSenseHint.Parameters);doc.Text=text;area.Caret.Position=area.Document.OffsetToPosition(text.Length);area.Focus();Application.DoEvents();};
                source("F");area.SimulateKeyPress('(');PumpUntil(()=>ui.ParameterProvider!=null);
                var outer=(Net10SignatureProvider)ui.ParameterProvider;
                Check(doc.Text=="F(" && outer.InsightDataCount==2 && area.Focused && lost==0,"typed opening bracket shows overloads without taking focus");
                area.SimulateKeyPress('1');client.Signature=()=>{calls++;return Task.FromResult(ParameterResponse(1));};area.SimulateKeyPress(',');
                PumpUntil(()=>ui.ParameterProvider is Net10SignatureProvider p && p.CurrentParameter==2);
                var current=(Net10SignatureProvider)ui.ParameterProvider;
                current.GetParameterHighlight(0,out int start,out int length);
                Check(current.GetInsightData(0).Substring(start,length)=="b: integer","comma updates the active parameter and bold range");
                foreach(char ch in "F(")area.SimulateKeyPress(ch);PumpUntil(()=>ui.ParameterProvider is Net10SignatureProvider p && p.Context.OpenOffset>1);
                area.SimulateKeyPress(')');
                Check(ReferenceEquals(ui.ParameterProvider,current),"closing nested call restores the outer signature");
                area.SimulateKeyPress(')');Check(ui.ParameterProvider==null,"closing outer call hides parameter help");
                source("F");area.SimulateKeyPress('[');PumpUntil(()=>ui.ParameterProvider!=null);
                Check(area.Focused && lost==0,"indexer bracket uses passive parameter help too");
                Check(area.ExecuteDialogKey(Keys.Escape) && ui.ParameterProvider==null,"Escape closes the passive parameter window");
                int before=calls;source("'F");area.SimulateKeyPress('(');Application.DoEvents();Check(calls==before,"brackets in strings do not request signatures");
                source("F");parameters=false;session.UpdateActivity();area.SimulateKeyPress('(');Application.DoEvents();Check(ui.ParameterProvider==null && calls==before,"parameter preference disables signature triggers");
                parameters=true;session.UpdateActivity();source("F");
                var delayed=new TaskCompletionSource<object>();bool requested=false;
                client.Signature=()=>{requested=true;return delayed.Task;};area.SimulateKeyPress('(');PumpUntil(()=>requested);
                area.Caret.Position=new ICSharpCode.TextEditor.TextLocation(0,0);delayed.SetResult(ParameterResponse());
                Wait(session.Documents.WhenIdleAsync());Application.DoEvents();Check(ui.ParameterProvider==null,"late signature cannot reopen after caret moves away");
                client.Signature=()=>Task.FromResult(ParameterResponse());source("F");area.SimulateKeyPress('(');PumpUntil(()=>ui.ParameterProvider!=null);
                active=false;session.UpdateActivity();Check(ui.ParameterProvider==null,"switching back to Mono closes modern parameter help");
                Check(area.Focused && lost==0,"signature triggers never transfer editor focus");
            }
        }
    }
    static void RealSignature(string kit)
    {
        using(var form=new Form{Width=800,Height=450,ShowInTaskbar=false,Opacity=0})
        using(var doc=new Document{FileName=Path.Combine(Path.GetTempPath(),"Signature-"+Guid.NewGuid().ToString("N")+".pas")})
        using(var ui=new ExternalIntelliSenseSession(form))
        {
            form.Controls.Add(doc.TextEditor);doc.TextEditor.Dock=DockStyle.Fill;
            doc.Text="begin var s := 'hello'; s.Substring end.";
            var area=doc.TextEditor.ActiveTextAreaControl.TextArea;
            area.Caret.Position=area.Document.OffsetToPosition("begin var s := 'hello'; s.Substring".Length);
            var events=new WorkbenchDocumentEventSource(()=>new ICodeFileDocument[]{doc});
            using(var session=new Net10LinuxLanguageSession(form,events,()=>true,
                ()=>new Net10LanguageClient(kit,"dotnet","ru",true),ui))
            {
                session.UpdateActivity();Wait(session.Documents.WhenIdleAsync());
                form.Show();area.Focus();Application.DoEvents();int lost=0;area.LostFocus+=(sender,e)=>lost++;
                area.SimulateKeyPress('(');PumpUntil(()=>ui.ParameterProvider!=null);
                Check(ui.ParameterProvider.GetInsightData(0).Contains("Substring"),"real LanguageServer supplies signatures through the Linux popup");
                area.SimulateKeyPress('1');area.SimulateKeyPress(',');
                PumpUntil(()=>ui.ParameterProvider is Net10SignatureProvider provider && provider.CurrentParameter==2);
                Check(area.Focused && lost==0 && !File.Exists(doc.FileName),"real server updates the parameter without saving or moving focus");
            }
        }
    }

}
