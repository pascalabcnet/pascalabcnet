
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System.Linq;

namespace LanguageServerEngine
{
    internal static class BaseConfiguration
    {

        public static TextDocumentSelector TextDocumentSelector => new TextDocumentSelector(
            TextDocumentFilter.ForPattern(
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
