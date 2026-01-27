using System.Collections.Generic;
using Languages.Facade;
using PascalABCCompiler.SyntaxTreeConverters;
using PascalABCCompiler.SystemLibrary;
using PascalABCCompiler.TreeConverter;

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

        public override void SetSemanticConstants()
        {
            SemanticRulesConstants.ClassBaseType = SystemLibrary.object_type;
            SemanticRulesConstants.StructBaseType = SystemLibrary.value_type;
            SemanticRulesConstants.AddResultVariable = true;
            SemanticRulesConstants.ZeroBasedStrings = true;
            SemanticRulesConstants.FastStrings = false;
            SemanticRulesConstants.InitStringAsEmptyString = true;
            SemanticRulesConstants.UseDivisionAssignmentOperatorsForIntegerTypes = false;
            SemanticRulesConstants.ManyVariablesOneInitializator = false;
            SemanticRulesConstants.OrderIndependedMethodNames = true;
            SemanticRulesConstants.OrderIndependedFunctionNames = false;
            SemanticRulesConstants.OrderIndependedTypeNames = false;
            SemanticRulesConstants.EnableExitProcedure = true;
            SemanticRulesConstants.StrongPointersTypeCheckForDotNet = true;
            SemanticRulesConstants.AllowChangeLoopVariable = false;
            SemanticRulesConstants.AllowGlobalVisibilityForPABCDll = true;
            SemanticRulesConstants.AllowMethodCallsWithoutParentheses = false;
        }

        public override void SetSyntaxTreeToSemanticTreeConverter()
        {
            // используем конвертор главного языка платформы (PascalABC.NET)
            SyntaxTreeToSemanticTreeConverter = LanguageProvider.Instance.MainLanguage.SyntaxTreeToSemanticTreeConverter;
        }

    }
}
