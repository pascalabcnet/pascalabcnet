using System;
using System.Collections.Generic;

using PascalABCCompiler.Parsers;
using PascalABCCompiler;
using PascalABCCompiler.ParserTools.Directives;

namespace Languages.Example.Frontend.Data
{
    internal class ExampleLanguageInformation : BaseLanguageInformation
    {
        public override string Name => "ExampleLang";

        public override string Version => "0.0.1";

        public override string Copyright => "Copyright © 2005-2025 by Example Programmer";

        public override string[] FilesExtensions => new string[] { ".exampleLang" };

        public override string[] SystemUnitNames => new string[] { StringConstants.pascalSystemUnitName };

        public override bool ApplySyntaxTreeConvertersForIntellisense => false;

        public override BaseKeywords KeywordsStorage { get; } = new Core.ExampleKeywords();

        public override Dictionary<string, DirectiveInfo> ValidDirectives { get; protected set; }

        public override string BodyStartBracket => null;

        public override string BodyEndBracket => null;

        public override string ParameterDelimiter => null;

        public override string DelimiterInIndexer => null;

        public override string ResultVariableName => null;

        public override string ProcedureName => null;

        public override string FunctionName => null;

        public override string GenericTypesStartBracket => null;

        public override string GenericTypesEndBracket => null;

        public override string ReturnTypeDelimiter => null;

        protected override string IntTypeName => "int";

        public override bool CaseSensitive => true;

        public override bool IncludeDotNetEntities => true;

        public override bool AddStandardUnitNamesToUserScope => true;

        public override bool AddStandardNetNamespacesToUserScope => true;

        public override bool UsesFunctionsOverlappingSourceContext => false;

        public override bool IsParams(string paramDescription)
        {
            throw new NotImplementedException();
        }

        public override string GetDescription(IBaseScope scope)
        {
            throw new NotImplementedException();
        }

        public override string GetSimpleDescription(IBaseScope scope)
        {
            throw new NotImplementedException();
        }


        public override string GetShortName(ICompiledConstructorScope scope)
        {
            return StringConstants.default_constructor_name;
        }

        public override string GetKeyword(SymbolKind kind)
        {
            throw new NotImplementedException();
        }

        public override string GetArrayDescription(string elementType, int rank)
        {
            throw new NotImplementedException();
        }

        public override string GetStandardTypeByKeyword(KeywordKind keyw)
        {
            throw new NotImplementedException();
        }

        protected override string GetFullTypeName(Type ctn, bool no_alias = true)
        {
            throw new NotImplementedException();
        }

        public override string GetClassKeyword(PascalABCCompiler.SyntaxTree.class_keyword keyw)
        {
            throw new NotImplementedException();
        }

        public override string GetShortTypeName(Type ctn, bool noalias = true)
        {
            throw new NotImplementedException();
        }

        public override string GetSynonimDescription(ITypeScope scope)
        {
            throw new NotImplementedException();
        }

        public override string GetSynonimDescription(ITypeSynonimScope scope)
        {
            throw new NotImplementedException();
        }

        public override string GetSynonimDescription(IProcScope scope)
        {
            throw new NotImplementedException();
        }

        public override string GetStringForChar(char c)
        {
            throw new NotImplementedException();
        }

        public override string GetStringForSharpChar(int num)
        {
            throw new NotImplementedException();
        }

        public override string GetStringForString(string s)
        {
            throw new NotImplementedException();
        }

        public override string ConstructOverridedMethodHeader(IProcScope scope, out int off)
        {
            throw new NotImplementedException();
        }

        public override string ConstructHeader(IProcRealizationScope scope, int tabCount)
        {
            throw new NotImplementedException();
        }

        public override string GetDocumentTemplate(string lineText, string Text, int line, int col, int pos)
        {
            throw new NotImplementedException();
        }

        public override string ConstructHeader(string meth, IProcScope scope, int tabCount)
        {
            throw new NotImplementedException();
        }

        public override string FindExpression(int off, string Text, int line, int col, out KeywordKind keyw)
        {
            throw new NotImplementedException();
        }

        public override string FindExpressionFromAnyPosition(int off, string Text, int line, int col, out KeywordKind keyw, out string expr_without_brackets)
        {
            throw new NotImplementedException();
        }

        public override KeywordKind TestForKeyword(string Text, int i)
        {
            throw new NotImplementedException();
        }

        public override string SkipNew(int off, string Text, ref KeywordKind keyw)
        {
            throw new NotImplementedException();
        }

        public override string FindExpressionForMethod(int off, string Text, int line, int col, char pressed_key, ref int num_param)
        {
            throw new NotImplementedException();
        }


        public override bool IsMethodCallParameterSeparator(char key)
        {
            throw new NotImplementedException();
        }


        public override bool IsDefinitionIdentifierAfterKeyword(KeywordKind keyw)
        {
            throw new NotImplementedException();
        }

        public override bool IsTypeAfterKeyword(KeywordKind keyw)
        {
            throw new NotImplementedException();
        }

    }
}
