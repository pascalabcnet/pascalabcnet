using PascalABCCompiler.Contracts.CodeCompletion;
using PascalABCCompiler.Contracts.Languages;
using PascalABCCompiler.Contracts.Parsers;
using PascalABCCompiler.SyntaxTreeConverters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

        public LanguageComponents Build()
        {
            ValidateComponents();

            return new LanguageComponents(languageInformation, languageIntellisenseSupport, parser, docParser, syntaxTreeConverters);
        }

        private void ValidateComponents()
        {
            if (languageInformation == null)
            {
                throw new InvalidOperationException($"Инициализация компонента с типом {typeof(ILanguageInformation)} обязательна. Не хватает вызова {nameof(WithInfo)}.");
            }

            if (parser == null)
            {
                throw new InvalidOperationException($"Инициализация компонента с типом {typeof(IParser)} обязательна. Не хватает вызова {nameof(WithParser)}.");
            }

            if (syntaxTreeConverters == null || syntaxTreeConverters.Count() == 0)
            {
                LogWarning($"Конверторы синтаксического сахара у языка {languageInformation.Name} не заданы. Используется дефолтный конвертор типа {typeof(DefaultSyntaxTreeConverter)}.");

                syntaxTreeConverters = new ISyntaxTreeConverter[] { new DefaultSyntaxTreeConverter() };
            }

            if (languageIntellisenseSupport == null)
            {
                LogWarning($"Компонент поддержки Intellisense у языка {languageInformation.Name} не инициализирован. Движок Intellisense не будет работать для языка.");
            }

            if (docParser == null)
            {
                LogWarning($"Компонент парсинга документирующих комментариев у языка {languageInformation.Name} не инициализирован. Intellisense не подхватит комментарии.");
            }
        }

        private void LogWarning(string message)
        {
            Debug.WriteLine($"[WARNING] LanguageBuilder: {message}");
        }
    }
}
