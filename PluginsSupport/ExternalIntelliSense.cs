using System;
using System.Drawing;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using ICSharpCode.TextEditor.Gui.InsightWindow;

namespace VisualPascalABCPlugins
{
    // Optional host capability. The historical workbench interfaces remain unchanged.
    public interface IWorkbenchIntelliSenseUI
    {
        IExternalIntelliSenseSession CreateIntelliSenseSession();
    }

    // Optional document-outline capability; symbols contain no compiler scopes.
    public interface IWorkbenchDocumentSymbolsUI
    {
        IExternalDocumentSymbolsView GetDocumentSymbolsView(ICodeFileDocument document);
    }
    public interface IExternalDocumentSymbolsView
    {
        Control View { get; }
        bool Visible { get; }
        void SetModernMode(bool enabled);
        void SetModernSymbols(System.Collections.Generic.IReadOnlyList<DocumentBrowserSymbol> symbols);
    }
    public sealed class DocumentBrowserSymbol
    {
        public string Name, Detail, Kind;
        public int StartLine, StartCharacter, EndLine, EndCharacter;
        public System.Collections.Generic.IReadOnlyList<DocumentBrowserSymbol> Children = Array.Empty<DocumentBrowserSymbol>();
    }

    // Optional command routing; hosts without this capability keep their existing navigation.
    public interface IExternalNavigationSession
    {
        bool NavigationEnabled { get; set; }
        event Action<ICodeFileDocument, bool> NavigationRequested;
    }

    public enum IntelliSenseHint { Completion, Hover, Parameters }
    public enum CompletionItemKind
    {
        Class, Method, Field, Local, Interface, Namespace, Property, Enum,
        Keyword, Constant, Struct, Event, ExtensionMethod
    }

    // All calls/events run on the IDE thread. A session owns only its own popups.
    // Dispose is idempotent; late calls on a disposed session are ignored.
    // Providers may use the host's images but must not dispose or modify them.
    public interface IExternalIntelliSenseSession : IDisposable
    {
        ImageList CompletionImages { get; }
        int GetImageIndex(CompletionItemKind kind);
        bool ProcessCompletionKey(char key);
        void ShowCompletion(ICodeFileDocument document, ICompletionDataProvider provider, bool preserveOrder = false);
        void ShowHover(ICodeFileDocument document, TextLocation position, Point mousePosition, string description);
        void ShowParameters(ICodeFileDocument document, int openingOffset, IExternalParameterInfo provider);
        IInsightDataProvider ParameterProvider { get; }
        bool ParameterHelpContainsFocus { get; }
        void Hide(IntelliSenseHint hint);
        // Completion/parameter windows notify when they close (including Hide).
        // Hover invalidation remains with the provider, as in the existing editor.
        event Action<IntelliSenseHint> Closed;
    }

    // Extra rendering information; ordinary editor providers and their text/offset
    // conventions are reused rather than introducing another signature DTO.
    public interface IExternalParameterInfo : IInsightDataProvider
    {
        int CurrentParameter { get; }
        bool GetParameterHighlight(int signature, out int start, out int length);
    }
}
