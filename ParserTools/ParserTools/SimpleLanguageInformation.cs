using PascalABCCompiler.ParserTools.Directives;
using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;

namespace PascalABCCompiler.Parsers
{
    public abstract class SimpleLanguageInformation : BaseLanguageInformation
    {
        public override Dictionary<string, DirectiveInfo> ValidDirectives { get; protected set; }

        public override string BodyStartBracket => null;

        public override string BodyEndBracket => null;

        public override string ParameterDelimiter => ",";

        public override string DelimiterInIndexer => ",";

        public override string ResultVariableName => null;

        public override string GenericTypesStartBracket => "<";

        public override string GenericTypesEndBracket => ">";

        public override string ReturnTypeDelimiter => ":";

        public override bool SyntaxTreeIsConvertedAfterUsedModulesCompilation => false;

        public override bool ApplySyntaxTreeConvertersForIntellisense => false;

        public override bool IncludeDotNetEntities => true;

        public override bool AddStandardUnitNamesToUserScope => false;

        public override bool AddStandardNetNamespacesToUserScope => true;

        public override bool UsesFunctionsOverlappingSourceContext => false;

        protected override string IntTypeName => "int";

        public override string ConstructHeader(string meth, IProcScope scope, int tabCount)
        {
            throw new NotImplementedException();
        }

        public override string ConstructHeader(IProcRealizationScope scope, int tabCount)
        {
            throw new NotImplementedException();
        }

        public override string ConstructOverridedMethodHeader(IProcScope scope, out int off)
        {
            throw new NotImplementedException();
        }

        public override string FindExpression(int off, string Text, int line, int col, out KeywordKind keyw)
        {
            throw new NotImplementedException();
        }

        public override string FindExpressionForMethod(int off, string Text, int line, int col, char pressed_key, ref int num_param)
        {
            throw new NotImplementedException();
        }

        public override string FindExpressionFromAnyPosition(int off, string Text, int line, int col, out KeywordKind keyw, out string expr_without_brackets)
        {
            throw new NotImplementedException();
        }

        // TODO: Проверить работу
        public override string GetDescription(IBaseScope scope)
        {
            throw new NotImplementedException();
        }

        public override string GetShortTypeName(Type t, bool noalias = true)
        {
            throw new NotImplementedException();
        }

        public override string GetSimpleDescription(IBaseScope scope)
        {
            throw new NotImplementedException();
        }

        public override string GetStandardTypeByKeyword(KeywordKind keyw)
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

        public override KeywordKind TestForKeyword(string Text, int i)
        {
            throw new NotImplementedException();
        }

        protected override string GetFullTypeName(Type ctn, bool no_alias = true)
        {
            throw new NotImplementedException();
        }
    }
}
