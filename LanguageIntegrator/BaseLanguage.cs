// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using PascalABCCompiler.Parsers;
using PascalABCCompiler.SyntaxTreeConverters;
using PascalABCCompiler.SystemLibrary;
using PascalABCCompiler.TreeConverter;
using System.Collections.Generic;

namespace Languages.Facade
{
    /// <summary>
    /// Базовый класс языка программирования, поддерживаемого платформой
    /// </summary>
    public abstract class BaseLanguage : ILanguage
    {
        /// <summary>
        /// Все параметры должны быть не null (и не пустым массивом),
        /// кроме IDocParser и ILanguageIntellisenseSupport в случае, если они не требуются
        /// </summary>
        public BaseLanguage(ILanguageInformation languageInformation, ILanguageIntellisenseSupport languageIntellisenseSupport,
            IParser parser, IDocParser docParser, List<ISyntaxTreeConverter> syntaxTreeConverters)
        {
            this.LanguageInformation = languageInformation;
            this.LanguageIntellisenseSupport = languageIntellisenseSupport;
            if (this.LanguageIntellisenseSupport != null)
                this.LanguageIntellisenseSupport.LanguageInformation = languageInformation;
            this.Parser = parser;
            this.Parser.LanguageInformation = languageInformation;
            this.DocParser = docParser;
            this.SyntaxTreeConverters = syntaxTreeConverters;
        }

        public string Name => LanguageInformation.Name;

        public string Version => LanguageInformation.Version;

        public string Copyright => LanguageInformation.Copyright;

        public string[] FilesExtensions => LanguageInformation.FilesExtensions;

        public bool CaseSensitive => LanguageInformation.CaseSensitive;

        public string[] SystemUnitNames => LanguageInformation.SystemUnitNames;

        public ILanguageInformation LanguageInformation { get; }

        public ILanguageIntellisenseSupport LanguageIntellisenseSupport { get; }

        public IParser Parser { get; protected set; }

        public IDocParser DocParser { get; protected set; }

        public List<ISyntaxTreeConverter> SyntaxTreeConverters { get; protected set; }

        public syntax_tree_visitor SyntaxTreeToSemanticTreeConverter { get; protected set; }

        public virtual void SetSemanticConstants()
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

        public abstract void SetSyntaxTreeToSemanticTreeConverter();
    }
}
