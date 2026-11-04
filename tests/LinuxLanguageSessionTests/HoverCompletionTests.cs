using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using VisualPascalABCPlugins;

partial class Program
{
    static void HoverCompletion(string kit=null)
    {
        using(var doc=new Document{FileName=Path.Combine(Path.GetTempPath(),"Hover-"+Guid.NewGuid().ToString("N")+".pas")})
        {
            doc.Text="begin var имя := 'hello'; Print(имя); end.";
            var position=doc.TextEditor.Document.OffsetToPosition(doc.Text.IndexOf("имя",StringComparison.Ordinal));
            var context=Net10HoverContext.Capture(doc,position);
            Check(context!=null && context.IsCurrent && context.Contains(position),"hover captures an unsaved Cyrillic identifier");
            doc.Text+=" ";Check(!context.IsCurrent,"editing invalidates a pending hover context");
            if(kit==null)return;
            var events=new WorkbenchDocumentEventSource(()=>new ICodeFileDocument[]{doc});
            var sync=new Net10LanguageDocuments(events,()=>new Net10LanguageClient(kit,"dotnet","ru",true),text=>{});
            try
            {
                sync.SetActive(true);Wait(sync.WhenIdleAsync());
                var request=sync.HoverAsync(doc,position.Y,position.X,CancellationToken.None);Wait(request);
                string description=Net10HoverContext.Description(request.Result);
                Check(description!=null && description.Contains("string") && !File.Exists(doc.FileName),"real server describes an unsaved symbol without saving");
            }
            finally{Wait(sync.ShutdownAsync());sync.Dispose();}
        }
    }
}
