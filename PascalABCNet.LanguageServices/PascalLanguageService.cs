using CodeCompletion;
using Languages.Facade;
using Languages.Pascal;
using PascalABCCompiler.Parsers;

namespace PascalABCNet.LanguageServices;

public sealed class PascalLanguageService : IPascalLanguageService
{
    private const string LibrarySourceDirectoryKey = "%LIBSOURCEDIRECTORY%";
    private static readonly TimeSpan AnalysisDebounce = TimeSpan.FromMilliseconds(200);
    private static readonly SemaphoreSlim SemanticGate = new(1, 1);
    private static readonly List<WeakReference<PascalLanguageService>> Instances = new();

    private readonly string _documentationLanguageIso;
    private readonly ILanguage _language;
    private readonly string _standardLibraryDirectory;
    private readonly CodeCompletionController _controller = new();
    private readonly Dictionary<string, DomConverter> _libraryImplementationModels = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, DocumentState> _states = new(StringComparer.Ordinal);
    private readonly Dictionary<string, CancellationTokenSource> _pendingAnalyses = new(StringComparer.Ordinal);
    private readonly object _pendingAnalysesSync = new();

    public PascalLanguageService(string documentationLanguageIso = "en")
        : this(documentationLanguageIso, FindStandardLibraryDirectory())
    {
    }

    public PascalLanguageService(
        string documentationLanguageIso,
        string standardLibraryDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(documentationLanguageIso);
        ArgumentException.ThrowIfNullOrWhiteSpace(standardLibraryDirectory);
        _documentationLanguageIso = documentationLanguageIso;

        var fullStandardLibraryDirectory = Path.GetFullPath(standardLibraryDirectory);
        _standardLibraryDirectory = fullStandardLibraryDirectory;
        if (!File.Exists(Path.Combine(fullStandardLibraryDirectory, "PABCSystem.pas")))
        {
            throw new DirectoryNotFoundException(
                $"PascalABC.NET standard library was not found in '{fullStandardLibraryDirectory}'.");
        }

        SemanticGate.Wait();
        try
        {
            _language = PascalLanguageRegistration.RegisterPascalLanguage();
            CodeCompletionBootstrap.Initialize(_documentationLanguageIso);
            _controller.BeforeIntelliSenseReferenceReload += InvalidateCachedAnalysis;
            lock (Instances)
            {
                Instances.RemoveAll(reference => !reference.TryGetTarget(out _));
                Instances.Add(new WeakReference<PascalLanguageService>(this));
            }
            CodeCompletionController.StandartDirectories[LibrarySourceDirectoryKey] =
                fullStandardLibraryDirectory;
        }
        finally
        {
            SemanticGate.Release();
        }
    }

    public DocumentStorage Documents { get; } = new();

    public Task<DocumentAnalysis> OpenOrUpdateDocumentAsync(
        string documentId,
        string fileName,
        string text,
        int? version = null,
        CancellationToken cancellationToken = default)
    {
        ValidateDocument(documentId, fileName, text);

        return ExecuteSerializedAsync(() =>
        {
            var document = Documents.Set(documentId, fileName, text, version);
            var domConverter = CompileAndStore(document);

            RecompileOpenDependents(document);

            return new DocumentAnalysis(
                documentId,
                version,
                domConverter.is_compiled,
                domConverter.LastConversionError?.ToString());
        }, cancellationToken);
    }

    public Task<bool> CloseDocumentAsync(
        string documentId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(documentId);

        return ExecuteSerializedAsync(() =>
        {
            CancelPendingAnalysis(documentId);
            Documents.TryGet(documentId, out var document);
            _states.Remove(documentId);
            var removed = Documents.Remove(documentId);

            if (document is not null)
                RestoreClosedDocumentFromDisk(document);

            return removed;
        }, cancellationToken);
    }

    public Task QueueDocumentUpdateAsync(
        string documentId,
        string fileName,
        string text,
        int? version = null,
        CancellationToken cancellationToken = default)
    {
        ValidateDocument(documentId, fileName, text);

        return ExecuteSerializedAsync(() =>
        {
            if (Documents.TryGet(documentId, out var currentDocument) &&
                currentDocument?.Version is int currentVersion &&
                version is int incomingVersion &&
                incomingVersion <= currentVersion)
            {
                return true;
            }

            var document = Documents.Set(documentId, fileName, text, version);

            if (_states.TryGetValue(documentId, out var state))
            {
                _states[documentId] = state with
                {
                    Document = document,
                    IsStale = true,
                    IsDirty = true
                };
                ScheduleAnalysis(document);
            }
            else
            {
                var domConverter = CompileAndStore(document);
                RecompileOpenDependents(document);
            }

            return true;
        }, cancellationToken);
    }

    public Task<IReadOnlyList<CompletionItem>> GetCompletionAfterDotAsync(
        string documentId,
        int caretOffset,
        CancellationToken cancellationToken = default)
    {
        return GetCompletionAsync(documentId, caretOffset, cancellationToken);
    }

    public Task<IReadOnlyList<CompletionItem>> GetCompletionAsync(
        string documentId,
        int caretOffset,
        CancellationToken cancellationToken = default)
    {
        return ExecuteSerializedAsync<IReadOnlyList<CompletionItem>>(() =>
        {
            var state = GetCompiledState(documentId);
            if (state is null)
                return Array.Empty<CompletionItem>();

            var text = state.Document.Text;
            if (caretOffset < 0 || caretOffset > text.Length)
                throw new ArgumentOutOfRangeException(nameof(caretOffset));

            var position = TextCoordinates.GetPosition(text, caretOffset);
            var textBeforeCaret = text[..caretOffset];
            var items = new List<CompletionItem>();
            if (caretOffset == 0)
            {
                items.AddRange(CodeCompletionNameHelper.Helper.GetKeywords().Select(keywordText =>
                    new CompletionItem(keywordText, null, null, "Keyword")));
                var rootSymbols = CompletionSymbolService.GetSymbolsByFirstCharacter(
                    state.DomConverter,
                    position.Line,
                    position.Character,
                    typeExpected: false,
                    namespaceExpected: false,
                    pattern: string.Empty,
                    smartIntellisense: true,
                    namespaceVisibleRange: 0);
                return CreateCompletionItems(items, rootSymbols);
            }

            var afterDot = caretOffset > 0 && text[caretOffset - 1] == '.';
            var context = new CompletionTriggerContext
            {
                DotPressed = afterDot,
                CtrlSpace = !afterDot
            };
            var expressionInfo = CompletionExpressionService.AnalyzeAtCaret(
                caretOffset,
                textBeforeCaret,
                position.Line,
                position.Character,
                _language.LanguageIntellisenseSupport,
                context,
                PascalABCCompiler.Parsers.KeywordKind.None);
            var symbols = GetCompletionSymbols(
                state,
                position,
                textBeforeCaret,
                caretOffset,
                context,
                expressionInfo,
                items);

            var completionItems = CreateCompletionItems(items, symbols);
            if (string.IsNullOrEmpty(expressionInfo.Pattern))
                return completionItems;

            var comparison = _language.CaseSensitive
                ? StringComparison.Ordinal
                : StringComparison.OrdinalIgnoreCase;
            return completionItems
                .Where(item => item.Label.StartsWith(expressionInfo.Pattern, comparison))
                .ToArray();
        }, cancellationToken);
    }

    private IReadOnlyList<CompletionItem> CreateCompletionItems(
        List<CompletionItem> items,
        SymInfo[]? symbols)
    {
        if (symbols is not null)
        {
            _language.LanguageIntellisenseSupport.RenameOrExcludeSpecialNames(symbols);
            items.AddRange(symbols
                .Where(symbol => symbol is not null && !symbol.not_include)
                .Select(CreateCompletionItem));
        }

        return items
            .DistinctBy(item => (item.Label.ToUpperInvariant(), item.Kind))
            .ToArray();
    }

    private SymInfo[]? GetCompletionSymbols(
        DocumentState state,
        TextPosition position,
        string textBeforeCaret,
        int caretOffset,
        CompletionTriggerContext context,
        CompletionExpressionInfo expressionInfo,
        List<CompletionItem> items)
    {
        if (context.DotPressed || expressionInfo.InsidePatternWithDots)
        {
            var parseResult = CompletionExpressionService.Parse(
                _language.Parser,
                state.Document.FileName,
                expressionInfo.ExpressionText,
                context);
            if (parseResult.ShouldAbortCompletion || parseResult.Expression is null)
                return null;

            var result = CompletionSymbolService.GetSymbols(
                state.DomConverter,
                position.Line,
                position.Character,
                context,
                _language.CaseSensitive,
                expressionInfo.ExpressionText,
                expressionInfo.CtrlOrShiftSpaceAfterDot,
                expressionInfo.InsidePatternWithDots,
                expressionInfo.Pattern,
                parseResult.Expression,
                expressionInfo.Keyword,
                smartIntellisense: true,
                namespaceVisibleRange: 0);
            return result.ShouldAbortCompletion ? null : result.Symbols;
        }

        var pattern = expressionInfo.Pattern ?? string.Empty;
        var prefixStart = caretOffset - pattern.Length;
        var keyword = prefixStart > 0
            ? _language.LanguageIntellisenseSupport.TestForKeyword(textBeforeCaret, prefixStart - 1)
            : PascalABCCompiler.Parsers.KeywordKind.None;

        if (_language.LanguageIntellisenseSupport.IsDefinitionIdentifierAfterKeyword(keyword))
            return null;

        var typeExpected = keyword == PascalABCCompiler.Parsers.KeywordKind.New ||
                           _language.LanguageIntellisenseSupport.IsTypeAfterKeyword(keyword);
        var namespaceExpected = _language.LanguageIntellisenseSupport.IsNamespaceAfterKeyword(keyword);
        if (!namespaceExpected)
        {
            var keywords = typeExpected
                ? CodeCompletionNameHelper.Helper.GetTypeKeywords()
                : CodeCompletionNameHelper.Helper.GetKeywords();
            items.AddRange(keywords.Select(keywordText =>
                new CompletionItem(keywordText, null, null, "Keyword")));
        }

        if (namespaceExpected)
            return GetNamespaceSymbols(state.DomConverter);

        return CompletionSymbolService.GetSymbolsByFirstCharacter(
            state.DomConverter,
            position.Line,
            position.Character,
            typeExpected,
            namespaceExpected,
            pattern,
            smartIntellisense: true,
            namespaceVisibleRange: 0);
    }

    private static SymInfo[] GetNamespaceSymbols(DomConverter domConverter)
    {
        var symbols = new List<SymInfo>();
        if (DomConverter.standard_units is not null)
            symbols.AddRange(DomConverter.standard_units.Where(symbol => symbol is not null));

        var names = domConverter.visitor.entry_scope?.GetNamesInAllTopScopes(true, null, false);
        if (names is not null)
        {
            symbols.AddRange(names.Where(symbol =>
                symbol is not null &&
                symbol.kind == SymbolKind.Namespace &&
                !symbol.IsUnitNamespace));
        }

        return symbols.ToArray();
    }

    private static CompletionItem CreateCompletionItem(SymInfo symbol)
    {
        var (detail, documentation) = SymbolDescriptionParser.Split(symbol.description ?? string.Empty);
        var label = symbol.aliasName ?? symbol.addit_name ?? symbol.name;
        return new CompletionItem(label, detail, documentation, symbol.kind.ToString());
    }

    public Task<HoverInfo?> GetHoverAsync(
        string documentId,
        int offset,
        CancellationToken cancellationToken = default)
    {
        return ExecuteSerializedAsync(() =>
        {
            var state = GetCompiledState(documentId);
            if (state is null)
                return null;

            var position = TextCoordinates.GetPosition(state.Document.Text, offset);
            var description = HoverService.GetDescription(
                _language,
                state.DomConverter,
                state.Document.FileName,
                state.Document.Text,
                offset,
                position.Line,
                position.Character);

            return string.IsNullOrWhiteSpace(description) ? null : new HoverInfo(description);
        }, cancellationToken);
    }

    public Task<IReadOnlyList<NavigationTarget>> GetNavigationAsync(string documentId, int offset,
        bool implementation, CancellationToken cancellationToken = default)
    {
        return ExecuteSerializedAsync<IReadOnlyList<NavigationTarget>>(() =>
        {
            var state = GetCompiledState(documentId);
            if (state is null) return Array.Empty<NavigationTarget>();
            var position = TextCoordinates.GetPosition(state.Document.Text, offset);
            var full = _language.LanguageIntellisenseSupport.FindExpressionFromAnyPosition(offset,
                state.Document.Text, position.Line, position.Character, out var keyword, out var shortExpression);
            // Same expression and DOM APIs as historical Ctrl-click; no second semantic analyzer.
            foreach (var candidate in new[] { full, shortExpression }.Distinct())
            {
                if (string.IsNullOrWhiteSpace(candidate)) continue;
                var expression = _language.Parser.GetExpression("test.pas", candidate.Replace("^", ""),
                    new List<PascalABCCompiler.Errors.Error>(), new List<PascalABCCompiler.Errors.CompilerWarning>());
                if (expression is null) continue;
                var positions = implementation
                    ? state.DomConverter.GetRealization(expression, position.Line, position.Character, keyword)
                    : state.DomConverter.GetDefinition(expression, position.Line, position.Character, keyword, false);
                if (implementation && positions.Count == 0)
                {
                    // Standard modules are normally analyzed interface-only for completion.
                    // Load their implementation only for an explicit navigation request.
                    var definitions = state.DomConverter.GetDefinition(expression,
                        position.Line, position.Character, keyword, false);
                    foreach (var definition in definitions)
                    {
                        if (definition.file_name is null || definition.line <= 0 || definition.column <= 0 ||
                            !Path.GetFullPath(definition.file_name).StartsWith(
                                _standardLibraryDirectory + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) ||
                            !File.Exists(definition.file_name)) continue;
                        var libraryDom = CodeCompletionController.comp_modules[definition.file_name] as DomConverter;
                        if (libraryDom is null ||
                            !_libraryImplementationModels.TryGetValue(definition.file_name, out var implementationModel) ||
                            !ReferenceEquals(libraryDom, implementationModel))
                        {
                            libraryDom = _controller.CompileAllIfNeed(definition.file_name, false);
                            if (libraryDom.is_compiled)
                                _libraryImplementationModels[definition.file_name] = libraryDom;
                        }
                        if (libraryDom is null || !libraryDom.is_compiled) continue;
                        positions.AddRange(libraryDom.GetRealization(expression,
                            definition.line - 1, definition.column - 1,
                            PascalABCCompiler.Parsers.KeywordKind.Function));
                    }
                }
                var targets = positions.Where(item => item.line > 0 && item.column > 0 &&
                    (item.file_name is not null || item.from_metadata && !string.IsNullOrEmpty(item.metadata)))
                    .Select(item => new NavigationTarget(item.file_name, item.line - 1, item.column - 1,
                        item.from_metadata ? item.full_metadata_title ?? item.metadata_title : null,
                        item.from_metadata ? item.metadata : null)).Distinct().ToArray();
                if (targets.Length > 0) return targets;
            }
            return Array.Empty<NavigationTarget>();
        }, cancellationToken);
    }

    public Task<IReadOnlyList<DocumentSymbolInfo>> GetDocumentSymbolsAsync(
        string documentId, CancellationToken cancellationToken = default)
    {
        return ExecuteSerializedAsync<IReadOnlyList<DocumentSymbolInfo>>(() =>
        {
            var state = GetCompiledState(documentId);
            if (state?.DomConverter.visitor.entry_scope is not IInterfaceUnitScope unit)
                return Array.Empty<DocumentSymbolInfo>();

            var symbols = new List<DocumentSymbolInfo>();
            AddDocumentSymbols(unit.Members, state.Document.FileName, symbols);
            if (unit.ImplementationUnitScope is { } implementation)
                AddDocumentSymbols(implementation.Members, state.Document.FileName, symbols);
            return symbols;
        }, cancellationToken);
    }

    private static void AddDocumentSymbols(IBaseScope[] scopes, string fileName, List<DocumentSymbolInfo> result)
    {
        if (scopes is null) return;
        foreach (var scope in scopes)
        {
            if (scope?.SymbolInfo is not { not_include: false } info ||
                string.IsNullOrEmpty(info.name) || info.name.Contains('$') || info.name.StartsWith('<'))
                continue;

            var kind = info.kind;
            if (kind is not (SymbolKind.Class or SymbolKind.Struct or SymbolKind.Interface or
                SymbolKind.Enum or SymbolKind.Delegate or SymbolKind.Type or SymbolKind.Method or
                SymbolKind.Property or SymbolKind.Field or SymbolKind.Event or SymbolKind.Variable or
                SymbolKind.Constant)) continue;

            var position = scope.GetPosition();
            if (string.IsNullOrEmpty(position.file_name) ||
                !string.Equals(Path.GetFullPath(position.file_name), Path.GetFullPath(fileName),
                    StringComparison.OrdinalIgnoreCase) || position.line < 1) continue;

            var children = new List<DocumentSymbolInfo>();
            if (kind is SymbolKind.Class or SymbolKind.Struct or SymbolKind.Interface or
                SymbolKind.Enum or SymbolKind.Delegate or SymbolKind.Type)
                AddDocumentSymbols(scope.Members, fileName, children);

            int startLine = position.line - 1, startCharacter = Math.Max(0, position.column - 1);
            int endLine = Math.Max(startLine, position.end_line - 1);
            int endCharacter = endLine == startLine
                ? Math.Max(startCharacter, position.end_column - 1)
                : Math.Max(0, position.end_column - 1);
            result.Add(new DocumentSymbolInfo(info.name, scope.GetDescriptionWithoutDoc() ?? info.name,
                kind.ToString(), startLine, startCharacter, endLine, endCharacter, children));
        }
    }

    public Task<SignatureHelpInfo?> GetSignatureHelpAsync(
        string documentId,
        int caretOffset,
        char triggerCharacter,
        int currentParameter = 1,
        int currentParameterForSelection = 1,
        CancellationToken cancellationToken = default)
    {
        return ExecuteSerializedAsync(() =>
        {
            var state = GetCompiledState(documentId);
            if (state is null)
                return null;

            var text = state.Document.Text;
            var position = TextCoordinates.GetPosition(text, caretOffset);
            var result = SignatureHelpService.GetSignatureHelpAtCaret(
                _language,
                state.DomConverter,
                state.Document.FileName,
                text[..caretOffset],
                caretOffset,
                position.Line,
                position.Character,
                triggerCharacter,
                currentParameter,
                currentParameterForSelection);

            if (result?.Signatures is null || result.Signatures.Length == 0)
                return null;

            return new SignatureHelpInfo(
                result.Signatures.Select(SymbolDescriptionParser.ParseSignature).ToArray(),
                result.DefaultIndex,
                result.CurrentParameter,
                result.ParameterCount);
        }, cancellationToken);
    }

    private async Task<T> ExecuteSerializedAsync<T>(Func<T> action, CancellationToken cancellationToken)
    {
        await SemanticGate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            CodeCompletionBootstrap.Initialize(_documentationLanguageIso);
            var compiler = CodeCompletionController.comp;
            var previousProvider = compiler.SourceFilesProvider;
            compiler.SourceFilesProvider = (file, operation) =>
            {
                var snapshot = Documents.GetOpenDocuments().FirstOrDefault(document =>
                    string.Equals(NormalizeFileName(document.FileName), NormalizeFileName(file), StringComparison.OrdinalIgnoreCase));
                if (snapshot is not null)
                    switch (operation)
                    {
                        case PascalABCCompiler.CoreUtils.SourceFileOperation.GetText: return snapshot.Text;
                        case PascalABCCompiler.CoreUtils.SourceFileOperation.Exists: return true;
                        case PascalABCCompiler.CoreUtils.SourceFileOperation.GetLastWriteTime: return DateTime.MaxValue;
                        case PascalABCCompiler.CoreUtils.SourceFileOperation.FileEncoding: return System.Text.Encoding.UTF8;
                    }
                return previousProvider(file, operation);
            };
            try { return action(); }
            finally { compiler.SourceFilesProvider = previousProvider; }
        }
        finally
        {
            SemanticGate.Release();
        }
    }

    private DocumentState? GetCompiledState(string documentId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(documentId);

        _controller.RefreshReferenceAssemblies();
        EnsureDirtyDocumentsFresh();

        if (_states.TryGetValue(documentId, out var cached) &&
            cached.SourceStamps.Any(pair => ReadSourceStamp(pair.Key) != pair.Value))
            CompileAndStore(cached.Document);

        if (!_states.ContainsKey(documentId) && Documents.TryGet(documentId, out var document) &&
            document is not null)
            CompileAndStore(document);

        return _states.TryGetValue(documentId, out var state) && state.DomConverter.is_compiled
            ? state
            : null;
    }

    private DomConverter CompileAndStore(DocumentSnapshot document)
    {
        string analysisText = document.Text;
        if (document.DocumentId.StartsWith("pascalabc-metadata-view:", StringComparison.Ordinal))
        {
            // The historical metadata renderer emits a unit header without interface/
            // implementation. Supply them only to the parser, on existing lines, so
            // displayed member/type coordinates stay unchanged.
            int headerEnd = analysisText.IndexOf(';');
            int unitEnd = analysisText.LastIndexOf("end.", StringComparison.OrdinalIgnoreCase);
            if (analysisText.TrimStart().StartsWith("unit ", StringComparison.OrdinalIgnoreCase) && headerEnd >= 0 && unitEnd > headerEnd)
                analysisText = analysisText.Insert(unitEnd, "implementation ").Insert(headerEnd + 1, " interface");
            // Reflection's by-ref return names (System.Char&) are not Pascal syntax.
            // Keep the original rendered text and its columns; only the analysis
            // view uses the element type. Escaped identifiers such as &index remain.
            var chars = analysisText.ToCharArray();
            for (int i = 1; i < chars.Length; i++)
                if (chars[i] == '&' && (char.IsLetterOrDigit(chars[i - 1]) || chars[i - 1] == '>' || chars[i - 1] == ']')) chars[i] = ' ';
            analysisText = new string(chars);
        }
        var domConverter = _controller.Compile(document.FileName, analysisText);

        if (domConverter.is_compiled)
        {
            var generation = _states.TryGetValue(document.DocumentId, out var previousState)
                ? previousState.Generation + 1
                : 1;
            _states[document.DocumentId] = new DocumentState(
                document,
                domConverter,
                GetDirectDependencies(domConverter),
                generation,
                IsStale: false,
                IsDirty: false,
                LastConversionError: null);
        }
        else if (_states.TryGetValue(document.DocumentId, out var lastSuccessfulState) &&
                 lastSuccessfulState.DomConverter.is_compiled &&
                 domConverter.LastConversionError is not PascalABCCompiler.Errors.PackageRestoreFailed)
        {
            _states[document.DocumentId] = lastSuccessfulState with
            {
                Document = document,
                IsStale = true,
                IsDirty = false,
                LastConversionError = domConverter.LastConversionError?.ToString()
            };
        }
        else
        {
            _states[document.DocumentId] = new DocumentState(
                document,
                domConverter,
                GetDirectDependencies(domConverter),
                Generation: 0,
                IsStale: true,
                IsDirty: false,
                LastConversionError: domConverter.LastConversionError?.ToString());
        }

        return domConverter;
    }

    private static void InvalidateCachedAnalysis()
    {
        // A DOM retains reflection Types from the old load context. Other open
        // documents stay in Documents and are reanalysed on their next request.
        lock (Instances)
        {
            for (int index = Instances.Count - 1; index >= 0; index--)
                if (Instances[index].TryGetTarget(out var service))
                {
                    service._states.Clear();
                    service._libraryImplementationModels.Clear();
                }
                else
                    Instances.RemoveAt(index);
        }
        CodeCompletionController.comp_modules.Clear();
        CodeCompletionController.pabcNamespaces.Clear();
        CodeCompletion.AssemblyDocCache.ReleaseReferenceGeneration();
        CodeCompletion.UnitDocCache.ReleaseReferenceGeneration();
        CodeCompletion.IntellisensePCUReader.ReleaseReferenceGeneration();
        CodeCompletionTools.AssemblyDocCache.ReleaseCollectibleAssemblies();
    }

    private void RecompileOpenDependents(DocumentSnapshot changedDocument)
    {
        var changedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            NormalizeFileName(changedDocument.FileName)
        };
        var rebuiltDocuments = new HashSet<string>(StringComparer.Ordinal)
        {
            changedDocument.DocumentId
        };

        while (true)
        {
            var dependents = _states.Values
                .Where(state =>
                    !rebuiltDocuments.Contains(state.Document.DocumentId) &&
                    state.Dependencies.Any(changedFiles.Contains))
                .ToArray();

            if (dependents.Length == 0)
                return;

            foreach (var dependent in dependents)
            {
                rebuiltDocuments.Add(dependent.Document.DocumentId);
                var domConverter = CompileAndStore(dependent.Document);

                changedFiles.Add(NormalizeFileName(dependent.Document.FileName));
            }
        }
    }

    private void RestoreClosedDocumentFromDisk(DocumentSnapshot closedDocument)
    {
        RemoveCachedModule(closedDocument.FileName);

        if (TryReadFile(closedDocument.FileName, out var diskText))
        {
            var diskConverter = _controller.Compile(
                closedDocument.FileName,
                diskText);
            if (!diskConverter.is_compiled)
                RemoveCachedModule(closedDocument.FileName);
        }

        RecompileOpenDependents(closedDocument);
    }

    private static void RemoveCachedModule(string fileName)
    {
        CodeCompletionController.comp_modules.Remove(fileName);

        var normalizedFileName = NormalizeFileName(fileName);
        if (!string.Equals(fileName, normalizedFileName, StringComparison.OrdinalIgnoreCase))
            CodeCompletionController.comp_modules.Remove(normalizedFileName);
    }

    private static bool TryReadFile(string fileName, out string text)
    {
        text = string.Empty;
        if (!File.Exists(fileName))
            return false;

        try
        {
            text = File.ReadAllText(fileName);
            return true;
        }
        catch (IOException)
        {
            return false;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
    }

    private static IReadOnlySet<string> GetDirectDependencies(DomConverter domConverter)
    {
        var sources = domConverter.ReferencedSourceFiles.Select(NormalizeFileName);
        if (!domConverter.is_compiled)
            return sources.ToHashSet(StringComparer.OrdinalIgnoreCase);

        return new[]
            {
                domConverter.visitor.entry_scope?.used_units,
                domConverter.visitor.impl_scope?.used_units
            }
            .Where(usedUnits => usedUnits is not null)
            .SelectMany(usedUnits => usedUnits!)
            .Select(scope => scope.file_name)
            .Where(fileName => !string.IsNullOrWhiteSpace(fileName))
            .Select(fileName => NormalizeFileName(fileName!))
            .Concat(sources)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    private static (DateTime Time, long Length) ReadSourceStamp(string fileName)
    {
        try
        {
            var file = new FileInfo(fileName);
            return file.Exists ? (file.LastWriteTimeUtc, file.Length) : (DateTime.MinValue, 0);
        }
        catch (IOException) { return (DateTime.MinValue, -1); }
        catch (UnauthorizedAccessException) { return (DateTime.MinValue, -1); }
    }

    private static string NormalizeFileName(string fileName) => Path.GetFullPath(fileName);

    private static string FindStandardLibraryDirectory()
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory);
             directory is not null;
             directory = directory.Parent)
        {
            foreach (var candidate in new[]
                     {
                         Path.Combine(directory.FullName, "LibSource"),
                         Path.Combine(directory.FullName, "Lib"),
                         Path.Combine(directory.FullName, "pascalabcnet", "bin", "LibSource"),
                         Path.Combine(directory.FullName, "pascalabcnet", "bin", "Lib")
                     })
            {
                if (File.Exists(Path.Combine(candidate, "PABCSystem.pas")))
                    return candidate;
            }
        }

        throw new DirectoryNotFoundException(
            "Could not locate the PascalABC.NET standard library containing PABCSystem.pas.");
    }

    private void EnsureDirtyDocumentsFresh()
    {
        while (true)
        {
            var dirtyStates = _states.Values.Where(state => state.IsDirty).ToArray();
            if (dirtyStates.Length == 0)
                return;

            var dirtyFileNames = dirtyStates
                .Select(state => NormalizeFileName(state.Document.FileName))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            var state = dirtyStates.FirstOrDefault(candidate =>
                            !candidate.Dependencies.Any(dirtyFileNames.Contains))
                        ?? dirtyStates[0];

            CancelPendingAnalysis(state.Document.DocumentId);
            var domConverter = CompileAndStore(state.Document);
            RecompileOpenDependents(state.Document);
        }
    }

    private void ScheduleAnalysis(DocumentSnapshot document)
    {
        var cancellation = new CancellationTokenSource();

        lock (_pendingAnalysesSync)
        {
            if (_pendingAnalyses.TryGetValue(document.DocumentId, out var previous))
                previous.Cancel();
            _pendingAnalyses[document.DocumentId] = cancellation;
        }

        _ = AnalyzeAfterDebounceAsync(document, cancellation);
    }

    private async Task AnalyzeAfterDebounceAsync(
        DocumentSnapshot scheduledDocument,
        CancellationTokenSource cancellation)
    {
        try
        {
            await Task.Delay(AnalysisDebounce, cancellation.Token).ConfigureAwait(false);
            await ExecuteSerializedAsync(() =>
            {
                if (!Documents.TryGet(scheduledDocument.DocumentId, out var latestDocument) ||
                    latestDocument is null ||
                    latestDocument != scheduledDocument ||
                    !_states.TryGetValue(scheduledDocument.DocumentId, out var state) ||
                    !state.IsDirty)
                {
                    return true;
                }

                var domConverter = CompileAndStore(latestDocument);
                RecompileOpenDependents(latestDocument);
                return true;
            }, cancellation.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (cancellation.IsCancellationRequested)
        {
        }
        catch (Exception exception)
        {
            await ExecuteSerializedAsync(() =>
            {
                if (_states.TryGetValue(scheduledDocument.DocumentId, out var state) &&
                    state.Document == scheduledDocument)
                {
                    _states[scheduledDocument.DocumentId] = state with
                    {
                        IsStale = true,
                        IsDirty = false,
                        LastConversionError = exception.ToString()
                    };
                }

                return true;
            }, CancellationToken.None).ConfigureAwait(false);
        }
        finally
        {
            lock (_pendingAnalysesSync)
            {
                if (_pendingAnalyses.TryGetValue(scheduledDocument.DocumentId, out var current) &&
                    ReferenceEquals(current, cancellation))
                {
                    _pendingAnalyses.Remove(scheduledDocument.DocumentId);
                }
            }

            cancellation.Dispose();
        }
    }

    private void CancelPendingAnalysis(string documentId)
    {
        lock (_pendingAnalysesSync)
        {
            if (_pendingAnalyses.Remove(documentId, out var cancellation))
                cancellation.Cancel();
        }
    }

    private static void ValidateDocument(string documentId, string fileName, string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(documentId);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentNullException.ThrowIfNull(text);

        if (!string.Equals(Path.GetExtension(fileName), ".pas", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Only PascalABC.NET .pas documents are supported.", nameof(fileName));
    }

    private sealed record DocumentState(
        DocumentSnapshot Document,
        DomConverter DomConverter,
        IReadOnlySet<string> Dependencies,
        long Generation,
        bool IsStale,
        bool IsDirty,
        string? LastConversionError)
    {
        public IReadOnlyDictionary<string, (DateTime Time, long Length)> SourceStamps { get; } =
            Dependencies.Where(path => !string.Equals(path, NormalizeFileName(Document.FileName), StringComparison.OrdinalIgnoreCase))
                .ToDictionary(path => path, ReadSourceStamp, StringComparer.OrdinalIgnoreCase);
    }
}
