using PascalABCCompiler.Contracts.CodeCompletion;
using PascalABCCompiler.Contracts.Languages;
using PascalABCCompiler.Contracts.Parsers;
using PascalABCCompiler.SyntaxTreeConverters;
using System.Collections.Generic;

namespace Languages.Facade
{
    public readonly struct LanguageComponents
    {
        public ILanguageInformation LanguageInformation { get; }
        public ILanguageIntellisenseSupport LanguageIntellisenseSupport { get; }
        public IParser Parser { get; }
        public IDocParser DocParser { get; }
        public IEnumerable<ISyntaxTreeConverter> SyntaxTreeConverters { get; }

        internal LanguageComponents(
            ILanguageInformation info,
            ILanguageIntellisenseSupport intellisense,
            IParser parser,
            IDocParser docParser,
            IEnumerable<ISyntaxTreeConverter> converters)
        {
            LanguageInformation = info;
            LanguageIntellisenseSupport = intellisense;
            Parser = parser;
            DocParser = docParser;
            SyntaxTreeConverters = converters;
        }
    }
}
