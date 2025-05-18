
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System.Linq;

namespace LanguageServerEngine
{
    internal static class BaseConfiguration
    {

        public static DocumentSelector TextDocumentSelector => new DocumentSelector(
            DocumentFilter.ForPattern(
                "**/*.{" +
                string.Join(
                    ",",
                    Languages.Facade.LanguageProvider.Instance.Languages
                    .SelectMany(lang => lang.FilesExtensions)
                    .Select(ext => ext.Substring(1))
                    )
                + "}"
            )
        );
    }
}
