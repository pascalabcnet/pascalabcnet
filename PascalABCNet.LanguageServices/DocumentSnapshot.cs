namespace PascalABCNet.LanguageServices;

public sealed record DocumentSnapshot(
    string DocumentId,
    string FileName,
    string Text,
    int? Version);
