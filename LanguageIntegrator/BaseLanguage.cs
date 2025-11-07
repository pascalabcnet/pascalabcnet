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
        /// Все параметры должны быть не null (и не пустым массивом), кроме IDocParser в случае, если он не требуется
        /// </summary>
        public BaseLanguage(ILanguageInformation languageInformation,
            IParser parser, IDocParser docParser, List<ISyntaxTreeConverter> syntaxTreeConverters)
        {
            this.LanguageInformation = languageInformation;
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

        public virtual ILanguageInformation LanguageInformation { get; }

        public virtual IParser Parser { get; protected set; }

        public virtual IDocParser DocParser { get; protected set; }

        public virtual List<ISyntaxTreeConverter> SyntaxTreeConverters { get; protected set; }

        public bool ApplySyntaxTreeConvertersForIntellisense => LanguageInformation.ApplySyntaxTreeConvertersForIntellisense;

        public virtual syntax_tree_visitor SyntaxTreeToSemanticTreeConverter { get; protected set; }

        public abstract void SetSemanticConstants();

        public abstract void SetSyntaxTreeToSemanticTreeConverter();
    }
}
