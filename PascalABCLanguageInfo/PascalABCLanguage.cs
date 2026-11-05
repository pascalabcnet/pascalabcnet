// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using Languages.Facade;
using PascalABCCompiler.SyntaxTreeConverters;
using PascalABCCompiler.SystemLibrary;
using PascalABCCompiler.TreeConverter;

namespace Languages.Pascal
{
    public class PascalABCLanguage : BaseLanguage
    {

        public PascalABCLanguage() : base(BuildLanguageComponents()) { }

        private static LanguageComponents BuildLanguageComponents()
        {
            var languageInformation = new Frontend.Data.PascalABCLanguageInformation();

            var intellisenseSupport = new Frontend.Data.PascalABCIntellisenseSupport(languageInformation);

            var syntaxConverters = new ISyntaxTreeConverter[] { new Frontend.Converters.StandardSyntaxTreeConverter(), new SyntaxSemanticVisitors.LambdaAnyConverter() };

            var parser = new Frontend.Wrapping.PascalABCNewLanguageParser(languageInformation);

            var docParser = new Frontend.Documentation.PascalDocTagsLanguageParser();

            return new LanguageBuilder()
                .WithInfo(languageInformation)
                .WithIntellisenseSupport(intellisenseSupport)
                .WithParser(parser)
                .WithDocParser(docParser)
                .WithSyntaxConverters(syntaxConverters)
                .Build();
        }


        public override void SetSemanticConstants()
        {
            SemanticRulesConstants.ClassBaseType = SystemLibrary.object_type;
            SemanticRulesConstants.StructBaseType = SystemLibrary.value_type;
            SemanticRulesConstants.AddResultVariable = true;
            SemanticRulesConstants.ZeroBasedStrings = false;
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
            SemanticRulesConstants.AllowMethodCallsWithoutParentheses = true;
        }

        public override void SetSyntaxTreeToSemanticTreeConverter()
        {
            SyntaxTreeToSemanticTreeConverter = new syntax_tree_visitor();
        }
    }
}
