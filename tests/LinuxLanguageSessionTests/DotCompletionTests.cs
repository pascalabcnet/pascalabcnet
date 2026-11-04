using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using VisualPascalABC;
using VisualPascalABCPlugins;

partial class Program
{
    static void PumpUntil(Func<bool> ready)
    {
        var watch=Stopwatch.StartNew();
        while(!ready()){Application.DoEvents();Thread.Sleep(5);if(watch.ElapsedMilliseconds>5000)throw new TimeoutException("UI response timeout");}
        Application.DoEvents();
    }
    static object CompletionResponse()=>new Dictionary<string,object>{["items"]=new object[]{
        new Dictionary<string,object>{["label"]="Substring",["kind"]=2,["detail"]="function Substring(i: integer): string"},
        new Dictionary<string,object>{["label"]="Select<>",["kind"]=2,["data"]=new Dictionary<string,object>{["isExtensionMethod"]=true}}
    }};
    static void DotCompletion()
    {
        var singleton=typeof(VisualPABCSingleton).GetField("mainForm",BindingFlags.Static|BindingFlags.NonPublic);
        var original=singleton.GetValue(null);
        bool wasSuspended=CodeCompletion.CodeCompletionController.LegacyAnalysisSuspended;
        var host=new WorkbenchServiceContainer();
        var parser=(CodeCompletionParserController)FormatterServices.GetUninitializedObject(typeof(CodeCompletionParserController));
        string queued=Path.GetFullPath("Suspended.pas");
        var fake=(Form1)FormatterServices.GetUninitializedObject(typeof(Form1));GC.SuppressFinalize(fake);
        fake.UserOptions=new UserOptions{AllowCodeCompletion=true,CodeCompletionDot=true,CodeCompletionHint=true,CodeCompletionParams=true,HighlightOperatorBrackets=false,ShowCompletionInfoByGroup=true};
        fake.VisualEnvironmentCompiler=(VisualEnvironmentCompiler)FormatterServices.GetUninitializedObject(typeof(VisualEnvironmentCompiler));GC.SuppressFinalize(fake.VisualEnvironmentCompiler);fake.VisualEnvironmentCompiler.compilerLoaded=true;
        try
        {
            singleton.SetValue(null,null);host.LegacyServicesSuspended=true;
            Check(CodeCompletion.CodeCompletionController.LegacyAnalysisSuspended,"modern mode suspends legacy IntelliSense");
            CodeCompletionParserController.filesToParse[queued]=true;
            typeof(CodeCompletionParserController).GetMethod("ParseInThread",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(parser,null);
            Check(CodeCompletionParserController.filesToParse[queued],"suspended legacy parser preserves queued editor changes");
            Check(!(bool)typeof(CodeCompletionParserController).GetMethod("CompileWatchedFile",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(parser,new object[]{queued,"begin end.",true}),"suspended parser starts no legacy compilation");
            parser.ResumeForOpenDocuments(new[]{queued});Check(CodeCompletionParserController.filesToParse[queued],"return to Mono queues all open sources for reanalysis");
            singleton.SetValue(null,fake);
            using(var form=new Form{Width=800,Height=450,ShowInTaskbar=false,Opacity=0})
            using(var doc=new Document{FileName=Path.GetFullPath("Dot.pas")})
            using(var ui=new ExternalIntelliSenseSession(form))
            {
                form.Controls.Add(doc.TextEditor);doc.TextEditor.Dock=DockStyle.Fill;form.Show();
                var area=doc.TextEditor.ActiveTextAreaControl.TextArea;
                var legacy=typeof(Form1).Assembly.GetType("VisualPascalABC.CodeCompletionKeyHandler");
                var oldHandler=legacy.GetMethod("Attach").Invoke(null,new object[]{doc.TextEditor});
                bool active=true;var client=new Client();var open=new List<ICodeFileDocument>{doc};var events=new WorkbenchDocumentEventSource(()=>open);
                using(var session=new Net10LinuxLanguageSession(form,events,()=>active,()=>client,ui,()=>fake.UserOptions.CodeCompletionDot))
                {
                    session.UpdateActivity();Wait(session.Documents.WhenIdleAsync());
                    Func<object> popup=()=>ui.GetType().GetField("completion",BindingFlags.Instance|BindingFlags.NonPublic).GetValue(ui);
                    Action<string> source=text=>{ui.Hide(IntelliSenseHint.Completion);doc.Text=text;area.Caret.Position=doc.TextEditor.Document.OffsetToPosition(text.Length);area.Focus();Application.DoEvents();};
                    int lost=0;area.LostFocus+=(sender,e)=>lost++;
                    foreach(var key in new[]{Keys.Enter,Keys.Tab,Keys.Escape})
                    {
                        source("s");client.Completion=()=>Task.FromResult(CompletionResponse());area.SimulateKeyPress('.');
                        PumpUntil(()=>popup()!=null);
                        Check(doc.Text=="s." && area.Focused && lost==0,"dot is inserted once and popup keeps keyboard focus: "+key);
                        Check(legacy.GetField("codeCompletionWindow").GetValue(oldHandler)==null,"legacy dot popup does not compete with external completion");
                        area.SimulateKeyPress('S');area.SimulateKeyPress('u');area.SimulateKeyPress('b');area.ExecuteDialogKey(key);
                        Check(doc.Text==(key==Keys.Escape?"s.Sub":"s.Substring") && popup()==null,"prefix filtering and insertion/cancellation: "+key);
                    }
                    source("s");var pending=new TaskCompletionSource<object>();client.Completion=()=>pending.Task;
                    area.SimulateKeyPress('.');Application.DoEvents();area.Caret.Position=new ICSharpCode.TextEditor.TextLocation(0,0);pending.SetResult(CompletionResponse());
                    Wait(session.Documents.WhenIdleAsync());Application.DoEvents();
                    Check(popup()==null,"late completion is rejected after caret movement");
                    source("s");client.Completion=()=>Task.FromResult(CompletionResponse());area.SimulateKeyPress('.');PumpUntil(()=>popup()!=null);
                    active=false;session.UpdateActivity();Check(popup()==null,"switching away closes the external popup");
                    Check(fake.UserOptions.AllowCodeCompletion && fake.UserOptions.CodeCompletionDot && fake.UserOptions.CodeCompletionHint && fake.UserOptions.CodeCompletionParams,"platform changes preserve saved IntelliSense preferences");
                    Check(area.Focused && lost==0,"completion never transfers focus");
                }
            }
            singleton.SetValue(null,null);host.LegacyServicesSuspended=false;
            Check(!CodeCompletion.CodeCompletionController.LegacyAnalysisSuspended,"return to Mono restores legacy IntelliSense");
        }
        finally{singleton.SetValue(null,null);host.LegacyServicesSuspended=wasSuspended;singleton.SetValue(null,original);CodeCompletionParserController.filesToParse.Remove(queued);}
    }
}
