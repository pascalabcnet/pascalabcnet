using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using VisualPascalABC;
using VisualPascalABCPlugins;

partial class Program
{
    static void KeyRegressions()
    {
        var platform=typeof(TextArea).Assembly.GetType("ICSharpCode.TextEditor.EditorPlatform").GetField("IsMonoUnix",BindingFlags.Static|BindingFlags.NonPublic);
        bool previous=(bool)platform.GetValue(null);
        bool suspended=CodeCompletion.CodeCompletionController.LegacyAnalysisSuspended;
        var templates=CodeCompletionActionsManager.templateManager;
        try
        {
            foreach(bool mono in new[]{false,true})
            foreach(bool modern in new[]{false,true})
            using(var form=new Form())
            using(var doc=new Document{FileName=Path.GetFullPath("Keys.pas")})
            using(var ui=new ExternalIntelliSenseSession(form))
            {
                platform.SetValue(null,mono);
                CodeCompletion.CodeCompletionController.LegacyAnalysisSuspended=modern;
                var area=doc.TextEditor.ActiveTextAreaControl.TextArea;
                doc.Text="begin";area.Caret.Position=new TextLocation(5,0);
                int insertions=0;
                area.KeyEventHandler+=ch=>{
                    if(ch!='\n')return false;
                    insertions++;area.InsertString("\n  ");area.Document.Insert(area.Caret.Offset,"\nend.");return true;
                };
                var events=new WorkbenchDocumentEventSource(()=>new List<ICodeFileDocument>{doc});
                using(var sync=new Net10LanguageDocuments(events,()=>new Client(),text=>{}))
                using(var completion=new Net10Completion(form,events,sync,()=>true,ui))
                {
                    completion.SetActive(modern);
                    new ICSharpCode.TextEditor.Actions.Return().Execute(area);
                    Check(doc.Text.Replace("\r\n","\n")=="begin\n  \nend." && area.Caret.Line==1 && insertions==1,
                        "Enter is consumed once; Mono="+mono+", net10="+modern);
                }
                doc.Text="be";area.Caret.Position=new TextLocation(2,0);
                var manager=new CodeTemplateManager();manager.ht["begin"]="begin\n  |\nend.";
                CodeCompletionActionsManager.templateManager=manager;
                new CodeCompletionNamesOnlyInModuleAction().Execute(area);
                Check(doc.Text.Replace("\r\n","\n")=="begin\n  \nend." && area.Caret.Line==1 && area.Caret.Column==2,
                    "Shift+Space expands be despite legacy analysis suspension; Mono="+mono+", net10="+modern);
            }
            using(var editor=new TextEditorControl())
            {
                var area=editor.ActiveTextAreaControl.TextArea;
                new ICSharpCode.TextEditor.Actions.Return().Execute(area);
                Check(editor.Document.TotalNumberOfLines==2,"unhandled Enter still inserts one newline");
            }
        }
        finally{platform.SetValue(null,previous);CodeCompletion.CodeCompletionController.LegacyAnalysisSuspended=suspended;CodeCompletionActionsManager.templateManager=templates;}
    }
}
