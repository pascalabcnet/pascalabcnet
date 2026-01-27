using PascalABCCompiler.Parsers;
using PascalABCCompiler.ParserTools.Directives;
using System.Collections.Generic;

namespace Languages.SPython.Frontend.Data
{
    internal class SPythonLanguageInformation : ILanguageInformation
    {
        public string Name => "SPython";

        public string Version => "0.0.1";

        public string Copyright => "Copyright © 2023-2026 by Vladislav Krylov, Egor Movchan";

        public string[] FilesExtensions => new string[] { ".pys" };

        public string[] SystemUnitNames => new string[] { "SPythonSystem", "SPythonHidden", "SPythonSystemPys" };

        public bool SyntaxTreeIsConvertedAfterUsedModulesCompilation => true;

        public BaseKeywords KeywordsStorage { get; } = new SPythonParser.SPythonKeywords();

        public Dictionary<string, DirectiveInfo> ValidDirectives { get; protected set; }

        public string CommentSymbol => "#";

        public bool CaseSensitive => true;

        private readonly Dictionary<string, string> specialModulesAliases = new Dictionary<string, string>
        {
            { "time", "time1" },
            { "random", "random1" },
        };

        public Dictionary<string, string> SpecialModulesAliases => specialModulesAliases;
    }
}
