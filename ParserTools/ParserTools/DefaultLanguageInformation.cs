// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)


using PascalABCCompiler.ParserTools.Directives;
using System.Collections.Generic;

namespace PascalABCCompiler.Parsers
{
    public abstract class DefaultLanguageInformation : ILanguageInformation
    {
        public abstract string Name { get; }

        public abstract string Version { get; }

        public abstract string Copyright { get; }

        public abstract bool CaseSensitive { get; }

        public abstract string[] FilesExtensions { get; }

        public abstract string[] SystemUnitNames { get; }

        public abstract BaseKeywords KeywordsStorage { get; }

        public virtual bool SyntaxTreeIsConvertedAfterUsedModulesCompilation => false;

        public virtual Dictionary<string, DirectiveInfo> ValidDirectives => null;

        // Важно переопределять для реализации тестов
        public virtual string CommentSymbol => "//";

        public virtual Dictionary<string, string> SpecialModulesAliases => null;
    }
}
