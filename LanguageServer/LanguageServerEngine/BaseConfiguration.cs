// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
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
