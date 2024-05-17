using System.Collections.Generic;
using Languages.Facade;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTreeConverters;
using PascalABCCompiler.SystemLibrary;
using PascalABCCompiler.TreeConverter;

namespace Languages.Pascal
{
    public class PascalLanguage : BaseLanguage
    {

        public PascalLanguage() : base(
            name: StringConstants.pascalLanguageName,
            version: "1.2",
            copyright: "Copyright © 2005-2024 by Ivan Bondarev, Stanislav Mikhalkovich",
            
            parser: new Frontend.Wrapping.PascalABCNewLanguageParser(),
            docParser: new Frontend.Documentation.PascalDocTagsLanguageParser(),
            
            syntaxTreeConverters: new List<ISyntaxTreeConverter>() { new Frontend.Converters.StandardSyntaxTreeConverter(), new Frontend.Converters.LambdaAnyConverter() },
            syntaxTreeToSemanticTreeConverter: new syntax_tree_visitor(),
            
            filesExtensions: new string[] { StringConstants.pascalSourceFileExtension },
            caseSensitive: false,
            systemUnitNames: StringConstants.pascalDefaultStandardModules
            ) { }


        public override void SetSemanticRules()
        {
            SemanticRules.ClassBaseType = SystemLibrary.object_type;
            SemanticRules.StructBaseType = SystemLibrary.value_type;
            SemanticRules.AddResultVariable = true;
            SemanticRules.ZeroBasedStrings = false;
            SemanticRules.FastStrings = false;
            SemanticRules.InitStringAsEmptyString = true;
            SemanticRules.UseDivisionAssignmentOperatorsForIntegerTypes = false;
            SemanticRules.ManyVariablesOneInitializator = false;
            SemanticRules.OrderIndependedMethodNames = true;
            SemanticRules.OrderIndependedFunctionNames = false;
            SemanticRules.OrderIndependedTypeNames = false;
            SemanticRules.EnableExitProcedure = true;
            SemanticRules.StrongPointersTypeCheckForDotNet = true;
            SemanticRules.AllowChangeLoopVariable = false;
            SemanticRules.AllowGlobalVisibilityForPABCDll = true;
        }

        public override void RefreshSyntaxTreeToSemanticTreeConverter()
        {
            SyntaxTreeToSemanticTreeConverter = new syntax_tree_visitor();
        }
    }
}
