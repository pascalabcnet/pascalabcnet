using PascalABCCompiler.Contracts.CodeCompletion;
using PascalABCCompiler.Contracts.Languages;
using PascalABCCompiler.Contracts.Parsers;
using PascalABCCompiler.SyntaxTreeConverters;
using System.Collections.Generic;

namespace Languages.Facade
{
    public sealed class LanguageBuilder
    {
        private ILanguageInformation languageInformation;
        private ILanguageIntellisenseSupport languageIntellisenseSupport;
        private IParser parser;
        private IDocParser docParser;
        private IEnumerable<ISyntaxTreeConverter> syntaxTreeConverters;

        public LanguageBuilder WithInfo(ILanguageInformation languageInformation) { this.languageInformation = languageInformation; return this; }
        public LanguageBuilder WithIntellisenseSupport(ILanguageIntellisenseSupport languageIntellisenseSupport) { this.languageIntellisenseSupport = languageIntellisenseSupport; return this; }
        public LanguageBuilder WithParser(IParser parser) { this.parser = parser; return this; }
        public LanguageBuilder WithDocParser(IDocParser docParser) { this.docParser = docParser; return this; }
        public LanguageBuilder WithSyntaxConverters(IEnumerable<ISyntaxTreeConverter> syntaxTreeConverters) { this.syntaxTreeConverters = syntaxTreeConverters; return this; }

        public LanguageComponents Build() =>
            new LanguageComponents(languageInformation, languageIntellisenseSupport, parser, docParser, syntaxTreeConverters);
    }
}
