// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)


using System.Collections.Generic;
using PascalABCCompiler.Contracts.Languages;
using PascalABCCompiler.Contracts.Parsers;

namespace PascalABCCompiler.Parsers
{
    /// <summary>
    /// Можно наследовать, если хочется больше дефолтных настроек
    /// </summary>
    public abstract class DefaultLanguageInformation : ILanguageInformation
    {
        public abstract string Name { get; }

        public abstract string Version { get; }

        public abstract string Copyright { get; }

        public abstract bool CaseSensitive { get; }

        public abstract string[] FilesExtensions { get; }

        public abstract string[] SystemUnitNames { get; }

        public abstract IKeywords KeywordsStorage { get; }

        public virtual bool SyntaxTreeIsConvertedAfterUsedModulesCompilation => false;

        public virtual IReadOnlyDictionary<string, IDirectiveInfo> ValidDirectives => null;

        // Важно переопределять для реализации тестов
        public virtual string CommentSymbol => "//";

        public virtual IReadOnlyDictionary<string, string> SpecialModulesAliases => null;
    }
}
