using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using PascalABCNet.LanguageServices;

namespace PascalABCNet.LanguageServer;

internal static class DocumentConversions
{
    public static string GetDocumentId(Uri uri) => uri.AbsoluteUri;

    public static string GetFileName(Uri uri)
    {
        var path = uri.IsFile ? uri.LocalPath : string.Empty;
        return string.Equals(Path.GetExtension(path), ".pas", StringComparison.OrdinalIgnoreCase)
            ? path
            : GetSyntheticFileName(uri);
    }

    private static string GetSyntheticFileName(Uri uri)
    {
        var uriHash = SHA256.HashData(Encoding.UTF8.GetBytes(uri.AbsoluteUri));
        return $"Untitled_{Convert.ToHexString(uriHash)}.pas";
    }

    public static bool TryGetOffset(
        IPascalLanguageService languageService,
        Uri uri,
        Position position,
        out int offset)
    {
        offset = 0;
        if (!languageService.Documents.TryGet(GetDocumentId(uri), out var document) || document is null)
            return false;

        try
        {
            offset = TextCoordinates.GetOffset(
                document.Text,
                new TextPosition((int)position.Line, (int)position.Character));
            return true;
        }
        catch (ArgumentOutOfRangeException)
        {
            return false;
        }
    }

    public static bool TryApplyContentChanges(
        string originalText,
        IEnumerable<TextDocumentContentChangeEvent> changes,
        out string updatedText)
    {
        ArgumentNullException.ThrowIfNull(originalText);
        ArgumentNullException.ThrowIfNull(changes);

        updatedText = originalText;
        try
        {
            foreach (var change in changes)
            {
                if (change.Range is null)
                {
                    updatedText = change.Text;
                    continue;
                }

                var start = TextCoordinates.GetOffset(
                    updatedText,
                    new TextPosition((int)change.Range.Start.Line, (int)change.Range.Start.Character));
                var end = TextCoordinates.GetOffset(
                    updatedText,
                    new TextPosition((int)change.Range.End.Line, (int)change.Range.End.Character));
                if (end < start)
                    return false;

                updatedText = string.Concat(
                    updatedText.AsSpan(0, start),
                    change.Text,
                    updatedText.AsSpan(end));
            }

            return true;
        }
        catch (ArgumentOutOfRangeException)
        {
            updatedText = originalText;
            return false;
        }
    }
}
