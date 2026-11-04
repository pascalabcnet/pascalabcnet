using System;
using System.Drawing;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using ICSharpCode.TextEditor.Gui.InsightWindow;
using VisualPascalABCPlugins;

namespace VisualPascalABC
{
    // Adapt the existing Linux widgets; their key handling, positioning, fonts
    // and nested-parameter stack stay in the same implementations as before.
    public sealed class ExternalIntelliSenseSession : IExternalIntelliSenseSession
    {
        private readonly Form parent;
        private CodeCompletionImagesProvider images;
        private PABCNETCodeCompletionWindow completion;
        private PABCNETInsightWindow parameters;
        private ICodeFileDocument parameterDocument;
        private bool disposed;
        public event Action<IntelliSenseHint> Closed;

        public ExternalIntelliSenseSession(Form parent)
        {
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        private CodeCompletionImagesProvider Images => images ?? (images = new CodeCompletionImagesProvider());
        public ImageList CompletionImages => disposed ? null : Images.ImageList;
        public int GetImageIndex(CompletionItemKind kind)
        {
            if (disposed) return 0;
            switch (kind)
            {
                case CompletionItemKind.Method: return Images.IconNumberMethod;
                case CompletionItemKind.ExtensionMethod: return Images.IconNumberExtensionMethod;
                case CompletionItemKind.Field: return Images.IconNumberField;
                case CompletionItemKind.Local: return Images.IconNumberLocal;
                case CompletionItemKind.Interface: return Images.IconNumberInterface;
                case CompletionItemKind.Namespace: return Images.IconNumberNamespace;
                case CompletionItemKind.Property: return Images.IconNumberProperty;
                case CompletionItemKind.Enum: return Images.IconNumberEnum;
                case CompletionItemKind.Keyword: return Images.IconNumberKeyword;
                case CompletionItemKind.Constant: return Images.IconNumberConstant;
                case CompletionItemKind.Struct: return Images.IconNumberStruct;
                case CompletionItemKind.Event: return Images.IconNumberEvent;
                default: return Images.IconNumberClass;
            }
        }

        public bool ProcessCompletionKey(char key) => !disposed && completion != null && completion.ProcessKeyEvent(key);

        public void ShowCompletion(ICodeFileDocument document, ICompletionDataProvider provider, bool preserveOrder = false)
        {
            if (disposed || parent.IsDisposed || parent.Disposing || document.TextEditor.IsDisposed) return;
            Hide(IntelliSenseHint.Completion);
            var items = provider.GenerateCompletionData(document.FileName, document.TextEditor.ActiveTextAreaControl.TextArea, '\0');
            completion = PABCNETCodeCompletionWindow.ShowExternalCompletionWindow(parent, document.TextEditor, provider, items, Images, preserveOrder);
            var shown = completion;
            if (shown != null) shown.Closed += (sender, e) =>
            {
                if (ReferenceEquals(completion, shown)) { completion = null; NotifyClosed(IntelliSenseHint.Completion); }
            };
        }

        public void ShowHover(ICodeFileDocument document, TextLocation position, Point mousePosition, string description)
        {
            if (!disposed && !parent.IsDisposed && !parent.Disposing && !document.TextEditor.IsDisposed)
                TooltipServiceManager.ShowOwnedExternalHint(this, document.TextEditor.ActiveTextAreaControl.TextArea,
                    mousePosition, position, description);
        }

        public IInsightDataProvider ParameterProvider => parameters?.InsightProviderStackLength > 0
            ? (parameters.GetInsightProvider() as ParameterAdapter)?.Provider : null;
        public bool ParameterHelpContainsFocus => parameters?.ContainsFocus == true;

        public void ShowParameters(ICodeFileDocument document, int openingOffset, IExternalParameterInfo provider)
        {
            if (disposed || parent.IsDisposed || parent.Disposing || document.TextEditor.IsDisposed || provider.InsightDataCount == 0) return;
            if (parameterDocument != null && parameterDocument != document) Hide(IntelliSenseHint.Parameters);
            if (parameters == null)
            {
                parameterDocument = document;
                parameters = new PABCNETInsightWindow(parent, document.TextEditor);
                parameters.Font = Constants.CompletionFont;
                var shown = parameters;
                shown.Closed += (sender, e) =>
                {
                    if (ReferenceEquals(parameters, shown))
                    { parameters = null; parameterDocument = null; NotifyClosed(IntelliSenseHint.Parameters); }
                };
            }
            var previous = parameters.InsightProviderStackLength > 0 ? parameters.GetInsightProvider() as ParameterAdapter : null;
            var adapter = new ParameterAdapter(provider, openingOffset);
            if (previous?.OpeningOffset == openingOffset)
            {
                if (parameters.GetCurrentData() != previous.DefaultIndex)
                    adapter.Selection = Math.Min(parameters.GetCurrentData(), provider.InsightDataCount - 1);
                parameters.ReplaceCurrentInsightDataProvider(adapter, document.FileName);
            }
            else parameters.AddInsightDataProvider(adapter, document.FileName);
            parameters.ShowInsightWindow();
        }

        public void Hide(IntelliSenseHint hint)
        {
            switch (hint)
            {
                case IntelliSenseHint.Completion:
                    var oldCompletion = completion; completion = null;
                    oldCompletion?.Close();
                    if (oldCompletion != null) NotifyClosed(hint);
                    break;
                case IntelliSenseHint.Hover:
                    TooltipServiceManager.HideOwnedExternalHint(this);
                    break;
                case IntelliSenseHint.Parameters:
                    var oldParameters = parameters; parameters = null; parameterDocument = null;
                    oldParameters?.Close();
                    if (oldParameters != null) NotifyClosed(hint);
                    break;
            }
        }

        private void NotifyClosed(IntelliSenseHint hint)
        {
            var handlers = Closed;
            if (handlers == null) return;
            foreach (Action<IntelliSenseHint> handler in handlers.GetInvocationList())
                try { handler(hint); }
                catch (Exception error) { System.Diagnostics.Trace.TraceWarning("IntelliSense window subscriber: {0}", error); }
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            Hide(IntelliSenseHint.Completion); Hide(IntelliSenseHint.Parameters); Hide(IntelliSenseHint.Hover);
            Closed = null;
            images?.ImageList.Dispose(); images = null;
        }

        // Linux renders the common parameter contract directly; the wrapper keeps
        // the call anchor and user-selected overload in the native window stack.
        private sealed class ParameterAdapter : IExternalParameterInfo
        {
            internal readonly IExternalParameterInfo Provider;
            internal readonly int OpeningOffset;
            internal int? Selection;
            internal ParameterAdapter(IExternalParameterInfo provider, int openingOffset)
            { Provider = provider; OpeningOffset = openingOffset; }
            public int DefaultIndex => Selection ?? Provider.DefaultIndex;
            public int CurrentParameter => Provider.CurrentParameter;
            public int InsightDataCount => Provider.InsightDataCount;
            public string GetInsightData(int index) => Provider.GetInsightData(index);
            public void SetupDataProvider(string fileName, TextArea area) => Provider.SetupDataProvider(fileName, area);
            public bool CaretOffsetChanged() => Provider.CaretOffsetChanged();
            public bool GetParameterHighlight(int signature, out int start, out int length) => Provider.GetParameterHighlight(signature, out start, out length);
        }
    }
}
