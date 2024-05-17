using PascalABCCompiler.Parsers;
using PascalABCCompiler.SyntaxTree;
using System.Collections.Generic;
using PascalABCCompiler.TreeConverter.TreeConversion;
using PascalABCCompiler.SyntaxTreeConverters;
using PascalABCCompiler.TreeConverter;

namespace Languages.Facade
{
    public abstract class BaseLanguage : ILanguage
    {

        public BaseLanguage(string name, string version, string copyright, IParser parser, IDocParser docParser,
            List<ISyntaxTreeConverter> syntaxTreeConverters, ISyntaxToSemanticTreeConverter syntaxTreeToSemanticTreeConverter,
            string[] filesExtensions, bool caseSensitive, string[] systemUnitNames)
        {
            this.Name = name;
            this.Version = version;
            this.Copyright = copyright;
            this.Parser = parser;
            this.DocParser = docParser;
            this.SyntaxTreeConverters = syntaxTreeConverters;
            this.SyntaxTreeToSemanticTreeConverter = syntaxTreeToSemanticTreeConverter;
            this.FilesExtensions = filesExtensions;
            this.CaseSensitive = caseSensitive;
            this.SystemUnitNames = systemUnitNames;

            PascalABCCompiler.SystemLibrary.SystemLibrary.syn_visitor = SyntaxTreeToSemanticTreeConverter as syntax_tree_visitor; // TODO: избавиться от этого   EVA
        }

        public virtual string Name { get; protected set; }

        public virtual string Version { get; protected set; }

        public virtual string Copyright { get; protected set; }

        public virtual IParser Parser { get; protected set; }

        public virtual IDocParser DocParser { get; protected set; }

        public virtual List<ISyntaxTreeConverter> SyntaxTreeConverters { get; protected set; }

        public virtual ISyntaxToSemanticTreeConverter SyntaxTreeToSemanticTreeConverter { get; protected set; }

        public virtual string[] FilesExtensions { get; protected set; }

        public virtual bool CaseSensitive { get; protected set; }

        public virtual string[] SystemUnitNames { get; protected set; }

        public abstract void SetSemanticRules();

        public abstract void RefreshSyntaxTreeToSemanticTreeConverter();
    }
}
