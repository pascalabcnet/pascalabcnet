using Microsoft.VisualStudio.LanguageServer.Protocol;
using PascalABCNet.LanguageServices;
using PascalABCNet.PackageServices;
using StreamJsonRpc;
using LspCompletionItem = Microsoft.VisualStudio.LanguageServer.Protocol.CompletionItem;

namespace PascalABCNet.LanguageServer;

internal sealed class PascalLanguageServerTarget
{
    internal sealed class NuGetDiscoveryRequest
    {
        public Uri Uri { get; set; } = null!;
        public string Prefix { get; set; } = string.Empty;
        public string? PackageId { get; set; }
    }

    // Discovery does not access semantic state. Keep network I/O off the analysis queue.
    [JsonRpcMethod("pascalabc/searchNuGetPackages", UseSingleObjectParameterDeserialization = true)]
    public Task<IReadOnlyList<PackageSuggestion>> SearchNuGetPackagesAsync(
        NuGetDiscoveryRequest request, CancellationToken token) =>
        new PackageDiscoveryService().SearchAsync(DiscoveryFile(request), request.Prefix, token);

    [JsonRpcMethod("pascalabc/nugetPackageVersions", UseSingleObjectParameterDeserialization = true)]
    public Task<IReadOnlyList<string>> NuGetPackageVersionsAsync(
        NuGetDiscoveryRequest request, CancellationToken token) =>
        new PackageDiscoveryService().GetVersionsAsync(DiscoveryFile(request),
            request.PackageId ?? string.Empty, request.Prefix, token);

    private static string DiscoveryFile(NuGetDiscoveryRequest request) =>
        request.Uri is { IsFile: true } ? request.Uri.LocalPath :
            throw new ArgumentException("NuGet discovery requires a file URI.");

    private static readonly HashSet<char> SignatureTriggers = new() { '(', '[', ',' };

    [JsonRpcMethod("pascalabc/restoreNuGetPackages", UseSingleObjectParameterDeserialization = true)]
    public Task<PackagePreparationResult> RestoreNuGetPackagesAsync(TextDocumentIdentifier request, CancellationToken token) =>
        _dispatcher.RunAsync(() => ((PascalLanguageService)_languageService).RestoreDocumentPackagesAsync(
            DocumentConversions.GetDocumentId(request.Uri), token), token);

    [JsonRpcMethod("pascalabc/refreshNuGetPackages", UseSingleObjectParameterDeserialization = true)]
    public Task<PackagePreparationResult> RefreshNuGetPackagesAsync(TextDocumentIdentifier request, CancellationToken token) =>
        _dispatcher.RunAsync(() => ((PascalLanguageService)_languageService).PrepareDocumentPackagesAsync(
            DocumentConversions.GetDocumentId(request.Uri), false, token), token);

    // Must bypass the serial analysis queue: the active analysis may be downloading.
    [JsonRpcMethod("pascalabc/cancelNuGetRestore")]
    public Task CancelNuGetRestoreAsync()
    {
        PascalABCNet.PackageServices.PackageRestoreService.CancelPendingRestores();
        return Task.CompletedTask;
    }

    private readonly IPascalLanguageService _languageService;
    private readonly SerialRequestDispatcher _dispatcher;
    private readonly TaskCompletionSource<bool> _exitSignal;
    private readonly Dictionary<Uri, MetadataDocument> _metadata = new();
    internal sealed record MetadataDocument(string Title, string Text);

    public PascalLanguageServerTarget(
        IPascalLanguageService languageService,
        SerialRequestDispatcher dispatcher,
        TaskCompletionSource<bool> exitSignal)
    {
        _languageService = languageService;
        _dispatcher = dispatcher;
        _exitSignal = exitSignal;
    }

    [JsonRpcMethod("initialize", UseSingleObjectParameterDeserialization = true)]
    public Task<InitializeResult> InitializeAsync(
        InitializeParams request,
        CancellationToken cancellationToken) =>
        _dispatcher.RunAsync(
            () => {
                if (_languageService is PascalLanguageService service && request.InitializationOptions is Newtonsoft.Json.Linq.JObject options)
                    service.ManualPackageRestore = options.Value<bool?>("manualNuGetRestore") == true;
                return Task.FromResult(new InitializeResult
            {
                Capabilities = new ServerCapabilities
                {
                    TextDocumentSync = new TextDocumentSyncOptions
                    {
                        OpenClose = true,
                        Change = TextDocumentSyncKind.Incremental,
                        Save = new SaveOptions { IncludeText = false }
                    },
                    CompletionProvider = new CompletionOptions
                    {
                        ResolveProvider = false,
                        TriggerCharacters = new[] { "." }
                    },
                    HoverProvider = true,
                    DocumentSymbolProvider = true,
                    DefinitionProvider = true,
                    ImplementationProvider = true,
                    SignatureHelpProvider = new SignatureHelpOptions
                    {
                        TriggerCharacters = new[] { "(", "[", "," }
                    }
                }
            }); },
            cancellationToken);

    [JsonRpcMethod("initialized", UseSingleObjectParameterDeserialization = true)]
    public Task InitializedAsync(InitializedParams notification, CancellationToken cancellationToken) =>
        _dispatcher.RunAsync(() => Task.CompletedTask, cancellationToken);

    [JsonRpcMethod("shutdown")]
    public Task<object?> ShutdownAsync(CancellationToken cancellationToken) =>
        _dispatcher.RunAsync<object?>(() => Task.FromResult<object?>(null), cancellationToken);

    [JsonRpcMethod("exit")]
    public Task ExitAsync(CancellationToken cancellationToken) =>
        _dispatcher.RunAsync(() =>
        {
            _exitSignal.TrySetResult(true);
            return Task.CompletedTask;
        }, cancellationToken);

    [JsonRpcMethod("textDocument/didOpen", UseSingleObjectParameterDeserialization = true)]
    public Task DidOpenAsync(
        DidOpenTextDocumentParams notification,
        CancellationToken cancellationToken) =>
        _dispatcher.RunAsync(
            () => _languageService.OpenOrUpdateDocumentAsync(
                DocumentConversions.GetDocumentId(notification.TextDocument.Uri),
                DocumentConversions.GetFileName(notification.TextDocument.Uri),
                notification.TextDocument.Text,
                notification.TextDocument.Version,
                cancellationToken),
            cancellationToken);

    [JsonRpcMethod("textDocument/didChange", UseSingleObjectParameterDeserialization = true)]
    public Task DidChangeAsync(
        DidChangeTextDocumentParams notification,
        CancellationToken cancellationToken) =>
        _dispatcher.RunAsync(async () =>
        {
            var documentId = DocumentConversions.GetDocumentId(notification.TextDocument.Uri);
            if (!_languageService.Documents.TryGet(documentId, out var document) ||
                document is null ||
                !DocumentConversions.TryApplyContentChanges(
                    document.Text,
                    notification.ContentChanges,
                    out var updatedText))
            {
                return;
            }

            await _languageService.QueueDocumentUpdateAsync(
                documentId,
                DocumentConversions.GetFileName(notification.TextDocument.Uri),
                updatedText,
                notification.TextDocument.Version,
                cancellationToken).ConfigureAwait(false);
        }, cancellationToken);

    [JsonRpcMethod("textDocument/didClose", UseSingleObjectParameterDeserialization = true)]
    public Task DidCloseAsync(
        DidCloseTextDocumentParams notification,
        CancellationToken cancellationToken) =>
        _dispatcher.RunAsync(
            () => _languageService.CloseDocumentAsync(
                DocumentConversions.GetDocumentId(notification.TextDocument.Uri),
                cancellationToken),
            cancellationToken);

    [JsonRpcMethod("textDocument/didSave", UseSingleObjectParameterDeserialization = true)]
    public Task DidSaveAsync(
        DidSaveTextDocumentParams notification,
        CancellationToken cancellationToken) =>
        _dispatcher.RunAsync(() => Task.CompletedTask, cancellationToken);

    [JsonRpcMethod("textDocument/completion", UseSingleObjectParameterDeserialization = true)]
    public Task<CompletionList> CompletionAsync(
        CompletionParams request,
        CancellationToken cancellationToken) =>
        _dispatcher.RunAsync(async () =>
        {
            if (!DocumentConversions.TryGetOffset(
                    _languageService,
                    request.TextDocument.Uri,
                    request.Position,
                    out var caretOffset))
                return EmptyCompletionList();

            var items = await _languageService.GetCompletionAsync(
                DocumentConversions.GetDocumentId(request.TextDocument.Uri),
                caretOffset,
                cancellationToken).ConfigureAwait(false);

            return new CompletionList
            {
                IsIncomplete = false,
                Items = items.Select(item => new LspCompletionItem
                {
                    Label = item.Label,
                    Detail = item.Detail,
                    Documentation = item.Documentation,
                    Kind = MapCompletionKind(item.Kind),
                    InsertText = item.Label.EndsWith("<>", StringComparison.Ordinal)
                        ? item.Label[..^2]
                        : item.Label
                }).ToArray()
            };
        }, cancellationToken);

    [JsonRpcMethod("textDocument/hover", UseSingleObjectParameterDeserialization = true)]
    public Task<Hover?> HoverAsync(
        TextDocumentPositionParams request,
        CancellationToken cancellationToken) =>
        _dispatcher.RunAsync(async () =>
        {
            if (!DocumentConversions.TryGetOffset(
                    _languageService,
                    request.TextDocument.Uri,
                    request.Position,
                    out var offset))
            {
                return null;
            }

            var hover = await _languageService.GetHoverAsync(
                DocumentConversions.GetDocumentId(request.TextDocument.Uri),
                offset,
                cancellationToken).ConfigureAwait(false);

            return hover is null
                ? null
                : new Hover
                {
                    Contents = new MarkupContent
                    {
                        Kind = MarkupKind.PlainText,
                        Value = hover.Contents
                    }
                };
        }, cancellationToken);

    [JsonRpcMethod("textDocument/documentSymbol", UseSingleObjectParameterDeserialization = true)]
    public Task<DocumentSymbol[]> DocumentSymbolsAsync(
        DocumentSymbolParams request, CancellationToken cancellationToken) =>
        _dispatcher.RunAsync(async () =>
        {
            var symbols = await _languageService.GetDocumentSymbolsAsync(
                DocumentConversions.GetDocumentId(request.TextDocument.Uri), cancellationToken).ConfigureAwait(false);
            return symbols.Select(ToDocumentSymbol).ToArray();
        }, cancellationToken);

    private static DocumentSymbol ToDocumentSymbol(DocumentSymbolInfo item)
    {
        var start = new Microsoft.VisualStudio.LanguageServer.Protocol.Position(item.StartLine, item.StartCharacter);
        var end = new Microsoft.VisualStudio.LanguageServer.Protocol.Position(item.EndLine, item.EndCharacter);
        return new DocumentSymbol
        {
            Name = item.Name,
            Detail = item.Detail,
            Kind = item.Kind switch
            {
                "Class" => Microsoft.VisualStudio.LanguageServer.Protocol.SymbolKind.Class,
                "Struct" => Microsoft.VisualStudio.LanguageServer.Protocol.SymbolKind.Struct,
                "Interface" => Microsoft.VisualStudio.LanguageServer.Protocol.SymbolKind.Interface,
                "Enum" => Microsoft.VisualStudio.LanguageServer.Protocol.SymbolKind.Enum,
                "Delegate" => Microsoft.VisualStudio.LanguageServer.Protocol.SymbolKind.Function,
                "Type" => Microsoft.VisualStudio.LanguageServer.Protocol.SymbolKind.Class,
                "Method" => Microsoft.VisualStudio.LanguageServer.Protocol.SymbolKind.Method,
                "Property" => Microsoft.VisualStudio.LanguageServer.Protocol.SymbolKind.Property,
                "Field" => Microsoft.VisualStudio.LanguageServer.Protocol.SymbolKind.Field,
                "Event" => Microsoft.VisualStudio.LanguageServer.Protocol.SymbolKind.Event,
                "Constant" => Microsoft.VisualStudio.LanguageServer.Protocol.SymbolKind.Constant,
                _ => Microsoft.VisualStudio.LanguageServer.Protocol.SymbolKind.Variable
            },
            Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range { Start = start, End = end },
            SelectionRange = new Microsoft.VisualStudio.LanguageServer.Protocol.Range { Start = start, End = start },
            Children = item.Children.Select(ToDocumentSymbol).ToArray()
        };
    }

    [JsonRpcMethod("textDocument/definition", UseSingleObjectParameterDeserialization = true)]
    public Task<Location[]> DefinitionAsync(TextDocumentPositionParams request, CancellationToken cancellationToken) =>
        NavigationAsync(request, false, cancellationToken);

    [JsonRpcMethod("textDocument/implementation", UseSingleObjectParameterDeserialization = true)]
    public Task<Location[]> ImplementationAsync(TextDocumentPositionParams request, CancellationToken cancellationToken) =>
        NavigationAsync(request, true, cancellationToken);

    private Task<Location[]> NavigationAsync(TextDocumentPositionParams request, bool implementation, CancellationToken cancellationToken) =>
        _dispatcher.RunAsync(async () =>
        {
            if (!DocumentConversions.TryGetOffset(_languageService, request.TextDocument.Uri, request.Position, out var offset))
                return Array.Empty<Location>();
            var targets = await _languageService.GetNavigationAsync(DocumentConversions.GetDocumentId(request.TextDocument.Uri),
                offset, implementation, cancellationToken).ConfigureAwait(false);
            return targets.Select(target =>
            {
                Uri uri;
                if (target.FileName is not null)
                {
                    var open = _languageService.Documents.GetOpenDocuments().FirstOrDefault(document =>
                        string.Equals(Path.GetFullPath(document.FileName), Path.GetFullPath(target.FileName), StringComparison.OrdinalIgnoreCase));
                    uri = open is null ? new Uri(Path.GetFullPath(target.FileName)) : new Uri(open.DocumentId);
                }
                else
                {
                    // On-demand generated metadata, not source files or another compiler kit.
                    var hash = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(
                        System.Text.Encoding.UTF8.GetBytes(target.MetadataTitle + "\n" + target.MetadataText)));
                    uri = new Uri("pascalabc-metadata:///" + hash);
                    if (_metadata.Count >= 32 && !_metadata.ContainsKey(uri)) _metadata.Remove(_metadata.Keys.First());
                    _metadata[uri] = new MetadataDocument(target.MetadataTitle ?? "Metadata", target.MetadataText!);
                }
                var point = new Microsoft.VisualStudio.LanguageServer.Protocol.Position(target.Line, target.Character);
                return new Location { Uri = uri, Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range { Start = point, End = point } };
            }).ToArray();
        }, cancellationToken);

    // LSP Location uses a virtual URI for CLR symbols. Retrieve its text without writing it to disk.
    [JsonRpcMethod("pascalabc/metadata", UseSingleObjectParameterDeserialization = true)]
    public Task<MetadataDocument?> MetadataAsync(TextDocumentIdentifier request, CancellationToken cancellationToken) =>
        _dispatcher.RunAsync(() => Task.FromResult(_metadata.TryGetValue(request.Uri, out var document) ? document : null), cancellationToken);

    [JsonRpcMethod("textDocument/signatureHelp", UseSingleObjectParameterDeserialization = true)]
    public Task<SignatureHelp?> SignatureHelpAsync(
        SignatureHelpParams request,
        CancellationToken cancellationToken) =>
        _dispatcher.RunAsync(async () =>
        {
            if (!DocumentConversions.TryGetOffset(
                    _languageService,
                    request.TextDocument.Uri,
                    request.Position,
                    out var caretOffset) ||
                !_languageService.Documents.TryGet(
                    DocumentConversions.GetDocumentId(request.TextDocument.Uri),
                    out var document) ||
                document is null ||
                caretOffset == 0)
            {
                return null;
            }

            var trigger = document.Text[caretOffset - 1];
            if (!SignatureTriggers.Contains(trigger))
                return null;

            var result = await _languageService.GetSignatureHelpAsync(
                DocumentConversions.GetDocumentId(request.TextDocument.Uri),
                caretOffset,
                trigger,
                cancellationToken: cancellationToken).ConfigureAwait(false);

            return result is null
                ? null
                : new SignatureHelp
                {
                    ActiveSignature = result.ActiveSignature,
                    ActiveParameter = Math.Max(0, result.ActiveParameter - 1),
                    Signatures = result.Signatures.Select(CreateSignatureInformation).ToArray()
                };
        }, cancellationToken);

    private bool TryGetTriggerOffset(Uri uri, Position position, char trigger, out int caretOffset)
    {
        caretOffset = 0;
        return DocumentConversions.TryGetOffset(_languageService, uri, position, out caretOffset) &&
               _languageService.Documents.TryGet(
                   DocumentConversions.GetDocumentId(uri),
                   out var document) &&
               document is not null &&
               caretOffset > 0 &&
               document.Text[caretOffset - 1] == trigger;
    }

    private static CompletionList EmptyCompletionList() => new()
    {
        IsIncomplete = false,
        Items = Array.Empty<LspCompletionItem>()
    };

    private static CompletionItemKind MapCompletionKind(string kind) => kind switch
    {
        "Class" => CompletionItemKind.Class,
        "Interface" => CompletionItemKind.Interface,
        "Namespace" => CompletionItemKind.Module,
        "Method" or "Function" or "Procedure" => CompletionItemKind.Method,
        "Property" => CompletionItemKind.Property,
        "Field" => CompletionItemKind.Field,
        "Variable" or "Parameter" => CompletionItemKind.Variable,
        "Constant" => CompletionItemKind.Constant,
        "Enum" => CompletionItemKind.Enum,
        "Keyword" => CompletionItemKind.Keyword,
        _ => CompletionItemKind.Text
    };

    private static SignatureInformation CreateSignatureInformation(SignatureInfo signature)
    {
        return new SignatureInformation
        {
            Label = signature.Label,
            Documentation = signature.Documentation,
            Parameters = signature.Parameters
                .Select(parameter => new ParameterInformation { Label = parameter })
                .ToArray()
        };
    }
}
