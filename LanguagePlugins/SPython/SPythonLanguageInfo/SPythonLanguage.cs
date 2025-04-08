using System.Collections.Generic;
using Languages.Facade;
using PascalABCCompiler.SyntaxTreeConverters;
using PascalABCCompiler.SystemLibrary;
using PascalABCCompiler.TreeConverter;

namespace Languages.SPython
{
    public class SPythonLanguage : BaseLanguage
    {
        public SPythonLanguage() : base(
            name: "SPython",
            version: "0.0.1",
            copyright: "Copyright © 2023-2025 by Vladislav Krylov, Egor Movchan",

            languageInformation: new Frontend.Data.SPythonLanguageInformation(),
            parser: new SPythonParser.SPythonLanguageParser(),
            docParser: null,

            syntaxTreeConverters: new List<ISyntaxTreeConverter>() { new Frontend.Converters.StandardSyntaxTreeConverter(), new SyntaxSemanticVisitors.LambdaAnyConverter() },
            // syntaxTreeToSemanticTreeConverter: new SPythonSyntaxTreeVisitor.spython_syntax_tree_visitor(LanguageProvider.Instance.MainLanguage.SyntaxTreeToSemanticTreeConverter),

            filesExtensions: new string[] { ".pys" },
            caseSensitive: true,
            systemUnitNames: new string[] { "SPythonSystem", "SPythonHidden" }
            )
        { }

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
        }

        public override void SetSyntaxTreeToSemanticTreeConverter()
        {
            SyntaxTreeToSemanticTreeConverter = new SPythonSyntaxTreeVisitor.spython_syntax_tree_visitor();
        }

    }
}
