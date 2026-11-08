using System;
using System.IO;
using System.Linq;
using VisualPascalABCPlugins;
namespace VisualPascalABC
{
    public partial class WorkbenchRunService : IExternalRunInputService, IExternalRunService
    {
        private ExternalInputSession externalInput;
        private ICodeFileDocument externalDocument;
        private string externalOutputFile;

        private sealed class ExternalCompileScope : IDisposable
        {
            private Action release;
            public ExternalCompileScope(Action release) { this.release = release; }
            public void Dispose() { var action = release; release = null; action?.Invoke(); }
        }

        public IDisposable PrepareExternalCompile(string sourceFileName)
        {
            if (RunnerManager.IsRun())
                throw new InvalidOperationException("Сначала остановите выполняющуюся программу IDE.");
            ErrorLineBookmark.Remove();
            RuntimeErrorBookmark.Remove();
            Workbench.WidgetController.SetCompilingAndRunButtonsEnabled(false);
            Workbench.WidgetController.SetDebugButtonsEnabled(false);
            Workbench.WidgetController.SetOptionsEnabled(false);
            var scope = new ExternalCompileScope(() =>
            {
                if (!Workbench.MainForm.IsDisposed && !RunnerManager.IsRun())
                    ButtonsEnableDisable_RunStop();
            });
            try
            {
                BuildService.BeforeCompile?.Invoke(sourceFileName);
                return scope;
            }
            catch { scope.Dispose(); throw; }
        }

        public string PrepareExternalArguments(string arguments) => RunnerManager.PrepareExternalArguments(arguments);

        public string GetExternalOutputFile(ICodeFileDocument document) =>
            document != null && document == externalDocument ? externalOutputFile : null;

        public IExternalRunSession RegisterExternalRun(ICodeFileDocument document, string outputFile, Action stop)
        {
            if (externalDocument != null || RunnerManager.IsRun())
                throw new InvalidOperationException("Уже запущена программа IDE.");
            outputFile = Path.GetFullPath(outputFile);
            return new ExternalRunSession(() =>
            {
                externalDocument = document;
                externalOutputFile = outputFile;
                RunTabsAdd(outputFile, document);
                DocumentService.ActiveCodeFileDocument = document;
                WorkbenchStorage.SetCurrentTabPageIfWriteToOutputWindow = true;
                RunnerManager.ExternalStarted(outputFile, stop);
            }, () =>
            {
                try
                {
                    if (!Workbench.MainForm.IsDisposed) RunnerManager.ExternalExited(outputFile);
                    else RunnerManager.ForgetExternalRun(outputFile);
                }
                finally
                {
                    document.Run = false;
                    RunTabs.Remove(Tools.FileNameToLower(outputFile));
                    externalDocument = null;
                    externalOutputFile = null;
                }
            }, error => RunnerManager_RunnerManagerUnhanledRuntimeException(outputFile, error.Type, error.Message,
                error.Stack, error.Frames.Select(frame => new RunManager.StackTraceItem
                {
                    FunctionName = frame.FunctionName, SourceFileName = frame.FileName, LineNumber = frame.Line
                }).ToList()), text => RunnerManager.ExternalOutput(outputFile, text));
        }

        public IExternalInputSession RegisterInput(ICodeFileDocument document, Action<string> send, Action stop)
        {
            if (externalInput != null || RunnerManager.IsRun())
                throw new InvalidOperationException("Another program already owns the input panel.");
            externalInput = new ExternalInputSession(document, send, stop,
                () =>
                {
                    ReadRequests[document] = "";
                    UpdateReadRequest(false);
                },
                () =>
                {
                    ReadRequests.Remove(document);
                    externalInput = null;
                    if (!Workbench.MainForm.IsDisposed && !Workbench.MainForm.Disposing)
                        UpdateReadRequest(false);
                });
            return externalInput;
        }

        public bool TryStopExternalInput()
        {
            return externalInput != null && externalInput.TryStop(DocumentService.CurrentCodeFileDocument);
        }

    }
}
