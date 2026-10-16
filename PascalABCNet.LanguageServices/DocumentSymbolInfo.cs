namespace PascalABCNet.LanguageServices;

public sealed record DocumentSymbolInfo(
    string Name,
    string Detail,
    string Kind,
    int StartLine,
    int StartCharacter,
    int EndLine,
    int EndCharacter,
    IReadOnlyList<DocumentSymbolInfo> Children);
