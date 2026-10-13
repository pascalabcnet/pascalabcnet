namespace PascalABCNet.LanguageServices;

public sealed class DocumentStorage
{
    private readonly Dictionary<string, DocumentSnapshot> _documents = new(StringComparer.Ordinal);
    private readonly object _syncRoot = new();

    public bool TryGet(string documentId, out DocumentSnapshot? document)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(documentId);

        lock (_syncRoot)
            return _documents.TryGetValue(documentId, out document);
    }

    public IReadOnlyList<DocumentSnapshot> GetOpenDocuments()
    {
        lock (_syncRoot)
            return _documents.Values.ToArray();
    }

    internal DocumentSnapshot Set(string documentId, string fileName, string text, int? version)
    {
        var document = new DocumentSnapshot(documentId, fileName, text, version);

        lock (_syncRoot)
            _documents[documentId] = document;

        return document;
    }

    internal bool Remove(string documentId)
    {
        lock (_syncRoot)
            return _documents.Remove(documentId);
    }
}
