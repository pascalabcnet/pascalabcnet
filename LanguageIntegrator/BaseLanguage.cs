using PascalABCCompiler.Parsers;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using PascalABCCompiler.SyntaxTreeConverters;
using System;
using System.Collections.Generic;
using PascalABCCompiler.TreeConverter.TreeConversion;
using PascalABCCompiler.TreeConverter;

namespace Languages.Facade
{
    public abstract class BaseLanguage : ILanguage
    {

        public BaseLanguage(string name, string version, string copyright, IParser parser, IParser docParser,
            List<ISyntaxTreeConverter> syntaxTreeConverters, ISyntaxTreeVisitor syntaxTreeToSemanticTreeConverter,
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

        public virtual IParser DocParser { get; protected set; }

        public List<IVisitor> SyntaxTreePostProcessors => throw new NotImplementedException();

        public virtual List<ISyntaxTreeConverter> SyntaxTreeConverters { get; protected set; }

        public virtual ISyntaxTreeVisitor SyntaxTreeToSemanticTreeConverter { get; protected set; }

        public virtual string[] FilesExtensions { get; protected set; }

        public virtual bool CaseSensitive { get; protected set; }

        public virtual string[] SystemUnitNames { get; protected set; }

        public abstract void SetSemanticRules();

        public abstract void RefreshSyntaxTreeToSemanticTreeConverter();
    }
}
