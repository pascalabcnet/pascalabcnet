using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualPascalABC;
using VisualPascalABCPlugins;

partial class Program
{
    sealed class DocumentServiceProxy : RealProxy
    {
        readonly ICodeFileDocument document;
        internal DocumentServiceProxy(ICodeFileDocument document):base(typeof(IWorkbenchDocumentService)){this.document=document;}
        public override IMessage Invoke(IMessage message)=>new ReturnMessage(document,null,0,null,(IMethodCallMessage)message);
    }
    static void DocumentSymbols(string kit=null)
    {
        using(var form=new Form{Width=800,Height=450,ShowInTaskbar=false,Opacity=0})
        using(var doc=new Document(new CodeFileDocumentTextEditorControl()){FileName=Path.Combine(Path.GetTempPath(),"Outline-"+Guid.NewGuid().ToString("N")+".pas")})
        {
            doc.Text="type MyType = class\n  procedure Work; begin end;\nend;\nbegin end.";
            form.Controls.Add(doc.TextEditor);doc.TextEditor.Dock=DockStyle.Fill;form.Show();
            var area=doc.TextEditor.ActiveTextAreaControl.TextArea;area.Focus();Application.DoEvents();int lost=0;area.LostFocus+=(sender,e)=>lost++;
            var ui=new WorkbenchServiceContainer();var panel=ui.GetDocumentSymbolsView(doc);
            var flags=BindingFlags.Instance|BindingFlags.NonPublic;
            var classes=(ComboBox)typeof(QuickClassBrowserPanel).GetField("classComboBox",flags).GetValue(panel);
            var members=(ComboBox)typeof(QuickClassBrowserPanel).GetField("membersComboBox",flags).GetValue(panel);
            if(kit==null)
            {
                panel.SetModernMode(true);
                panel.SetModernSymbols(new[]{new DocumentBrowserSymbol{Name="MyType",Kind="Class",StartLine=0,EndLine=2,EndCharacter=4,
                    Children=new[]{new DocumentBrowserSymbol{Name="Work",Detail="procedure Work",Kind="Method",StartLine=1,EndLine=1,EndCharacter=30}}}});
                Check(classes.Items.Count==2 && members.Items.Count==1 && members.Items[0].ToString()=="procedure Work","Linux outline displays class and method from detached symbols");
                Check(panel.View.Enabled && area.Focused && lost==0,"modern outline enables the panel without moving editor focus");
                panel.SetModernMode(false);Check(classes.Items.Count==0 && members.Items.Count==0,"return to Mono clears modern outline data");
                return;
            }
            var events=new WorkbenchDocumentEventSource(()=>new ICodeFileDocument[]{doc});
            var sync=new Net10LanguageDocuments(events,()=>new Net10LanguageClient(kit,"dotnet","ru",true),text=>{});
            var service=(IWorkbenchDocumentService)new DocumentServiceProxy(doc).GetTransparentProxy();
            using(var outline=new Net10DocumentSymbols(events,service,sync,ui.GetDocumentSymbolsView))
            {
                try
                {
                    sync.SetActive(true);Wait(sync.WhenIdleAsync());outline.SetActive(true,true);
                    area.Caret.Position=area.Document.OffsetToPosition(doc.Text.IndexOf("Work",StringComparison.Ordinal));
                    var tick=typeof(Net10DocumentSymbols).GetMethod("Tick",flags);
                    tick.Invoke(outline,new object[]{null,EventArgs.Empty});PumpUntil(()=>classes.Items.Count==2);
                    Check(classes.Items[1].ToString()=="MyType" && members.Items.Count>0,"real LanguageServer fills Linux outline from unsaved source");
                    doc.Text=doc.Text.Replace("MyType","YourType");tick.Invoke(outline,new object[]{null,EventArgs.Empty});PumpUntil(()=>classes.Items.Count==2 && classes.Items[1].ToString()=="YourType");
                    Check(!File.Exists(doc.FileName) && area.Focused && lost==0,"outline refreshes unsaved edits without saving or moving focus");
                    doc.Text="unit Sample;\ninterface\nprocedure P;\nimplementation\nprocedure Hidden; begin end;\nprocedure P; begin end;\nend.";
                    tick.Invoke(outline,new object[]{null,EventArgs.Empty});
                    PumpUntil(()=>classes.Items.Count==2 && (classes.Items[0].GetType().GetProperty("ModernSymbol").GetValue(classes.Items[0]) as DocumentBrowserSymbol)?.Name=="interface");
                    Check(((DocumentBrowserSymbol)classes.Items[1].GetType().GetProperty("ModernSymbol").GetValue(classes.Items[1])).Name=="implementation","unit outline starts with interface and implementation instead of global names");
                    classes.SelectedIndex=0;
                    Check(members.Items.Count>0,"interface section contains its declarations");
                    classes.SelectedIndex=1;
                    Check(members.Items.Count>0,"implementation section contains its routines");
                    outline.SetActive(false,false);Check(classes.Items.Count==0,"switching to Mono restores the legacy outline path");
                }
                finally{Wait(sync.ShutdownAsync());sync.Dispose();}
            }
        }
    }
}
