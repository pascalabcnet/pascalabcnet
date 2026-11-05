using Languages.Facade;
using PascalABCCompiler.SyntaxTreeConverters;
using PascalABCCompiler.SystemLibrary;
using PascalABCCompiler.TreeConverter;

namespace Languages.SPython
{
    public class SPythonLanguage : BaseLanguage
    {
        public SPythonLanguage() : base(BuildLanguageComponents()) { }

        private static LanguageComponents BuildLanguageComponents()
        {
            var languageInformation = new Frontend.Data.SPythonLanguageInformation();

            var intellisenseSupport = new Frontend.Data.SPythonIntellisenseSupport(languageInformation);

            var syntaxConverters = new ISyntaxTreeConverter[] { new Frontend.Converters.StandardSyntaxTreeConverter(), new SyntaxSemanticVisitors.LambdaAnyConverter() };

            var parser = new SPythonParser.SPythonLanguageParser(languageInformation, syntaxConverters);

            return new LanguageBuilder()
                .WithInfo(languageInformation)
                .WithIntellisenseSupport(intellisenseSupport)
                .WithParser(parser)
                .WithSyntaxConverters(syntaxConverters)
                .Build();
        }

        /// <summary>
        /// TODO: требуется проверка и настройка работы  EVA
        /// </summary>
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
            SyntaxTreeToSemanticTreeConverter = new SPythonSyntaxTreeVisitor.spython_syntax_tree_visitor(LanguageProvider.Instance.MainLanguage.SyntaxTreeToSemanticTreeConverter);
        }

    }
}
