using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualPascalABCPlugins;

partial class Program
{
    static void NavigationCompletion(string kit=null)
    {
        string root=Path.Combine(Path.GetTempPath(),"Nav-"+Guid.NewGuid().ToString("N"));
        using(var form=new Form{Width=800,Height=450,ShowInTaskbar=false,Opacity=0})
        using(var main=new Document{FileName=Path.Combine(root,"Main.pas")})
        using(var unit=new Document{FileName=Path.Combine(root,"NavUnit.pas")})
        {
            main.Text="uses NavUnit; begin P1 end.";
            unit.Text="unit NavUnit; interface procedure P1; implementation procedure P1; begin end; end.";
            var events=new WorkbenchDocumentEventSource(()=>new ICodeFileDocument[]{main,unit});
            var response=new object[]{new Dictionary<string,object>{["uri"]=new Uri(unit.FileName).AbsoluteUri,
                ["range"]=new Dictionary<string,object>{["start"]=new Dictionary<string,object>{["line"]=0,["character"]=62}}}};
            int requests=0;var client=new Client{Navigation=()=>{requests++;return Task.FromResult<object>(response);}};
            var sync=new Net10LanguageDocuments(events,()=>kit==null?(INet10LanguageClient)client:new Net10LanguageClient(kit,"dotnet","ru",true),text=>{});
            try
            {
                sync.SetActive(true);Wait(sync.WhenIdleAsync());
                int offset=main.Text.IndexOf("P1",StringComparison.Ordinal);
                if(kit!=null)
                {
                    var request=sync.NavigationAsync(main,0,offset,CancellationToken.None);Wait(request);
                    var targets=Net10NavigationTarget.Read(request.Result);
                    Check(targets.Count>0 && targets[0].FileName==unit.FileName,"real navigation resolves an unsaved imported unit");
                    main.Text="begin var s := 'abc'; s.Substring(1); end.";
                    request=sync.NavigationAsync(main,0,main.Text.IndexOf("Substring",StringComparison.Ordinal),CancellationToken.None);Wait(request);
                    targets=Net10NavigationTarget.Read(request.Result);
                    Check(targets.Count>0 && targets[0].MetadataText!=null && targets[0].MetadataText.Contains("Substring"),"real navigation fetches .NET metadata without writing files");
                    var definition=sync.DefinitionAsync(main,0,main.Text.IndexOf("Substring",StringComparison.Ordinal),CancellationToken.None);Wait(definition);
                    targets=Net10NavigationTarget.Read(definition.Result);
                    Check(targets.Count>0 && targets[0].MetadataText!=null && targets[0].MetadataText.Contains("Substring"),"definition command fetches .NET metadata");
                    Check(!File.Exists(main.FileName) && !File.Exists(unit.FileName),"navigation preserves unsaved source files");
                    return;
                }
                form.Controls.Add(main.TextEditor);main.TextEditor.Dock=DockStyle.Fill;form.Show();
                var area=main.TextEditor.ActiveTextAreaControl.TextArea;area.Focus();Application.DoEvents();
                int lost=0;area.LostFocus+=(sender,e)=>lost++;
                Keys modifiers=Keys.None;List<Net10NavigationTarget> opened=null;
                using(var navigation=new Net10Navigation(events,sync,()=>true,targets=>opened=targets,()=>modifiers))
                {
                    navigation.SetActive(true);
                    var bounds=area.TextView.DrawingPosition;
                    var mouse=new MouseEventArgs(MouseButtons.Left,1,bounds.X+area.TextView.GetDrawingXPos(0,offset)+1,bounds.Y+area.TextView.FontHeight/2,0);
                    var click=typeof(ICSharpCode.TextEditor.TextArea).GetMethod("OnMouseClick",BindingFlags.Instance|BindingFlags.NonPublic);
                    click.Invoke(area,new object[]{mouse});Application.DoEvents();Check(requests==0,"plain click does not request navigation");
                    modifiers=Keys.Control;click.Invoke(area,new object[]{mouse});PumpUntil(()=>opened!=null);
                    Check(opened.Count==1 && opened[0].FileName==unit.FileName && opened[0].Column==63,"Ctrl-click maps LSP targets to IDE coordinates once");
                    Check(area.Focused && lost==0,"navigation request does not move editor focus");
                    navigation.SetActive(false);int before=requests;click.Invoke(area,new object[]{mouse});Application.DoEvents();Check(requests==before,"Mono mode detaches modern navigation");
                }
            }
            finally{Wait(sync.ShutdownAsync());sync.Dispose();}
        }
    }
}
