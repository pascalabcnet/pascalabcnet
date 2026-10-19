namespace PascalABCNet.LanguageServices;

public sealed record PackagePreparationResult(bool Success, string? Message);

public sealed record DocumentAnalysis(
    string DocumentId,
    int? Version,
    bool IsCompiled,
    string? LastConversionError);

public sealed record CompletionItem(
    string Label,
    string? Detail,
    string? Documentation,
    string Kind);

public sealed record HoverInfo(string Contents);

// Parser positions converted once to zero-based LSP coordinates.
public sealed record NavigationTarget(string? FileName, int Line, int Character,
    string? MetadataTitle = null, string? MetadataText = null);

public sealed record SignatureInfo(
    string Label,
    string? Documentation,
    IReadOnlyList<string> Parameters);

public sealed record SignatureHelpInfo(
    IReadOnlyList<SignatureInfo> Signatures,
    int ActiveSignature,
    int ActiveParameter,
    int ParameterCount);
