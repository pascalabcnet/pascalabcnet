// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using PascalABCCompiler.Parsers;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTreeConverters;
using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.SyntaxTree;

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
        public BaseLanguage(string name, string version, string copyright, ILanguageInformation languageInformation,
            IParser parser, IDocParser docParser, List<ISyntaxTreeConverter> syntaxTreeConverters, List<ISyntaxTreeConverter> syntaxTreeConvertersForIntellisense,
            string[] filesExtensions, bool caseSensitive, string[] systemUnitNames)
        {
            this.Name = name;
            this.Version = version;
            this.Copyright = copyright;
            this.LanguageInformation = languageInformation;
            this.Parser = parser;
            this.Parser.LanguageInformation = languageInformation;
            this.DocParser = docParser;
            this.SyntaxTreeConverters = syntaxTreeConverters;
            this.SyntaxTreeConvertersForIntellisense = syntaxTreeConvertersForIntellisense;
            this.FilesExtensions = filesExtensions;
            this.CaseSensitive = caseSensitive;
            this.SystemUnitNames = systemUnitNames;
        }

        public virtual string Name { get; protected set; }

        public virtual string Version { get; protected set; }

        public virtual string Copyright { get; protected set; }

        public virtual ILanguageInformation LanguageInformation { get; }

        public virtual IParser Parser { get; protected set; }

        public virtual IDocParser DocParser { get; protected set; }

        public virtual List<ISyntaxTreeConverter> SyntaxTreeConverters { get; protected set; }

        public virtual List<ISyntaxTreeConverter> SyntaxTreeConvertersForIntellisense { get; protected set; }

        public virtual syntax_tree_visitor SyntaxTreeToSemanticTreeConverter { get; protected set; }

        public virtual string[] FilesExtensions { get; protected set; }

        public virtual bool CaseSensitive { get; protected set; }

        public virtual string[] SystemUnitNames { get; protected set; }

        public abstract void SetSemanticConstants();

        public abstract void SetSyntaxTreeToSemanticTreeConverter();
    }
}
