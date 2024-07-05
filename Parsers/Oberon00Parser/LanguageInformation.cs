using System;
using System.IO;
using System.Text;
using System.Reflection;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.Errors;
using System.Collections.Generic;
using GPPGParserScanner;

namespace PascalABCCompiler.Oberon00Parser
{
    public class Oberon00LanguageInformation : DefaultLanguageInformation
    {
        public Oberon00LanguageInformation(IParser p):base(p)
		{
        }

        protected override void InitKeywords()
        {
            List<string> keys = new List<string>();
            List<string> type_keys = new List<string>();
            keywords.Add("AND", "AND"); keys.Add("AND");
            keywords.Add("OR", "OR"); keys.Add("OR");
            keywords.Add("XOR", "XOR"); keys.Add("XOR");
            keywords.Add("BEGIN", "BEGIN"); keys.Add("BEGIN");
            keywords.Add("END", "END"); keys.Add("END");
            keywords.Add("FOR", "FOR"); keys.Add("FOR");
            keywords.Add("WHILE", "WHILE"); keys.Add("WHILE");
            keywords.Add("DO", "DO"); keys.Add("DO");
            keywords.Add("TO", "TO"); keys.Add("TO");
            keywords.Add("ARRAY", "ARRAY"); keys.Add("ARRAY"); type_keys.Add("ARRAY");
            keywords.Add("MODULE", "MODULE"); keys.Add("MODULE"); keyword_kinds.Add("MODULE", KeywordKind.Unit);
            keywords.Add("VAR", "VAR"); keys.Add("VAR"); keyword_kinds.Add("VAR", KeywordKind.Var);
            keywords.Add("CONST", "CONST"); keys.Add("CONST"); keyword_kinds.Add("CONST", KeywordKind.Const);
            keywords.Add("IF", "IF"); keys.Add("IF");
            keywords.Add("THEN", "THEN"); keys.Add("THEN");
            keywords.Add("ELSE", "ELSE"); keys.Add("ELSE");
            keywords_array = keys.ToArray();
            type_keywords_array = type_keys.ToArray();
        }

        public override string SystemUnitName
        {
            get {
                return "Oberon00System";
            }
        }

        public override string BodyStartBracket
        {
            get
            {
                return "BEGIN";
            }
        }

        public override string BodyEndBracket
        {
            get
            {
                return "END";
            }
        }

        public override string ParameterDelimiter
        {
            get
            {
                return ";";
            }
        }

        public override bool CaseSensitive
        {
            get
            {
                return true;
            }
        }

        public override bool IncludeDotNetEntities
        {
            get
            {
                return false;
            }
        }

        public override string GetDescriptionForModule(IInterfaceUnitScope scope)
        {
            return "Module " + scope.Name;
        }

        public override string GetStandardTypeByKeyword(KeywordKind keyw)
        {
            switch (keyw)
            {
                case KeywordKind.ByteType: return "BYTE";
                case KeywordKind.IntType: return "INTEGER";
                case KeywordKind.DoubleType: return "REAL";
                case KeywordKind.CharType: return "CHAR";
                case KeywordKind.BoolType: return "BOOLEAN";
            }
            return null;
        }

        protected override string GetSimpleDescriptionForElementScope(IElementScope scope)
        {
            string type_name = GetSimpleDescription(scope.Type);
            if (type_name.StartsWith("$")) type_name = type_name.Substring(1, type_name.Length - 1);
            return scope.Name + ": " + type_name;
        }

        private string kind_of_param(IElementScope scope)
        {
            /*switch (scope.ParamKind)
            {
                case PascalABCCompiler.SyntaxTree.parametr_kind.var_parametr: return "VAR ";
                case PascalABCCompiler.SyntaxTree.parametr_kind.params_parametr: return "ParamArray ";
                case PascalABCCompiler.SyntaxTree.parametr_kind.out_parametr: return "ByRef ";
            }*/
            return "";
        }

        protected override string GetDescriptionForElementScope(IElementScope scope)
        {
            string type_name = null;
            StringBuilder sb = new StringBuilder();
            if (scope.Type == null) type_name = "";
            else
                type_name = GetSimpleDescription(scope.Type);
            if (type_name.StartsWith("$"))
                type_name = type_name.Substring(1, type_name.Length - 1);
            switch (scope.ElemKind)
            {
                case SymKind.Variable: sb.Append("VAR " + scope.Name + ": " + type_name); break;
                case SymKind.Parameter: sb.Append(kind_of_param(scope) + "PARAMETER " + scope.Name + ": " + type_name); break;
                case SymKind.Constant:
                    {
                        if (scope.ConstantValue == null)
                            sb.Append("CONST " + scope.Name + ": " + type_name);
                        else sb.Append("CONST " + scope.Name + " = " + scope.ConstantValue.ToString());
                    }
                    break;
                
            }
            return sb.ToString();
        }
    }
}