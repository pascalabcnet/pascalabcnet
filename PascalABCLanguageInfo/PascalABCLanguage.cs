// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System.Collections.Generic;
using Languages.Facade;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTreeConverters;
using PascalABCCompiler.SystemLibrary;
using PascalABCCompiler.TreeConverter;

namespace Languages.Pascal
{
    public class PascalABCLanguage : BaseLanguage
    {

        public PascalABCLanguage() : base(
            name: StringConstants.pascalLanguageName,
            version: "1.2",
            copyright: "Copyright © 2005-2025 by Ivan Bondarev, Stanislav Mikhalkovich",

            languageInformation: new Frontend.Data.PascalABCLanguageInformation(),
            parser: new Frontend.Wrapping.PascalABCNewLanguageParser(),
            docParser: new Frontend.Documentation.PascalDocTagsLanguageParser(),

            syntaxTreeConverters: new List<ISyntaxTreeConverter>() { new Frontend.Converters.StandardSyntaxTreeConverter(), new SyntaxSemanticVisitors.LambdaAnyConverter() },
            syntaxTreeConvertersForIntellisense: null,
            
            filesExtensions: new string[] { StringConstants.pascalSourceFileExtension },
            caseSensitive: false,
            systemUnitNames: StringConstants.pascalDefaultStandardModules
            ) { }


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
        }

        public override void SetSyntaxTreeToSemanticTreeConverter()
        {
            SyntaxTreeToSemanticTreeConverter = new syntax_tree_visitor();
        }
    }
}
