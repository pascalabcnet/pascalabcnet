using System.Collections.Generic;
using Languages.Facade;
using PascalABCCompiler.SyntaxTreeConverters;

namespace Languages.Example
{
    /// <summary>
    /// Демонстрационный язык.
    /// Программа на нем может содержать только:
    /// 1) Присваивания целых констант
    /// 2) Вызов функций модуля PABCSystem с передачей им параметром одной константы или переменной
    /// </summary>
    public class ExampleLanguage : BaseLanguage
    {
        public ExampleLanguage() : base(

            languageInformation: new Frontend.Data.ExampleLanguageInformation(),
            languageIntellisenseSupport: null,
            parser: new Frontend.Wrapping.ExampleLanguageParser(),
            docParser: null,

            syntaxTreeConverters: new List<ISyntaxTreeConverter>() { new DefaultSyntaxTreeConverter() }
            )
        { }

        public override void SetSyntaxTreeToSemanticTreeConverter()
        {
            // используем конвертор главного языка платформы (PascalABC.NET)
            SyntaxTreeToSemanticTreeConverter = LanguageProvider.Instance.MainLanguage.SyntaxTreeToSemanticTreeConverter;
        }

    }
}
