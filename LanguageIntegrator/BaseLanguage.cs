// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using PascalABCCompiler.Parsers;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTreeConverters;
using PascalABCCompiler.TreeConverter;

namespace Languages.Facade
{
    /// <summary>
    /// Базовый класс языка программирования, поддерживаемого платформой
    /// </summary>
    public abstract class BaseLanguage : ILanguage
    {
        /// <summary>
        /// Все параметры должны быть не null (и не пустым массивом),
        /// кроме IDocParser и ILanguageIntellisenseSupport в случае, если они не требуются
        /// </summary>
        public BaseLanguage(ILanguageInformation languageInformation, ILanguageIntellisenseSupport languageIntellisenseSupport,
            IParser parser, IDocParser docParser, List<ISyntaxTreeConverter> syntaxTreeConverters)
        {
            this.LanguageInformation = languageInformation;
            this.LanguageIntellisenseSupport = languageIntellisenseSupport;
            this.LanguageIntellisenseSupport.LanguageInformation = languageInformation;
            this.Parser = parser;
            this.Parser.LanguageInformation = languageInformation;
            this.DocParser = docParser;
            this.SyntaxTreeConverters = syntaxTreeConverters;
        }

        public string Name => LanguageInformation.Name;

        public string Version => LanguageInformation.Version;

        public string Copyright => LanguageInformation.Copyright;

        public string[] FilesExtensions => LanguageInformation.FilesExtensions;

        public bool CaseSensitive => LanguageInformation.CaseSensitive;

        public string[] SystemUnitNames => LanguageInformation.SystemUnitNames;

        public ILanguageInformation LanguageInformation { get; }

        public ILanguageIntellisenseSupport LanguageIntellisenseSupport { get; }

        public IParser Parser { get; protected set; }

        public IDocParser DocParser { get; protected set; }

        public List<ISyntaxTreeConverter> SyntaxTreeConverters { get; protected set; }

        public syntax_tree_visitor SyntaxTreeToSemanticTreeConverter { get; protected set; }

        public abstract void SetSemanticConstants();

        public abstract void SetSyntaxTreeToSemanticTreeConverter();
    }
}
