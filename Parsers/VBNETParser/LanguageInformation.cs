using System;
using System.IO;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;
using System.Resources;
using System.Reflection;
using System.Collections.Generic;
using PascalABCCompiler.Errors;
using System.Text;

namespace PascalABCCompiler.VBNETParser
{
    public class VBNETLanguageInformation : DefaultLanguageInformation
    {

        public VBNETLanguageInformation()
        {

        }

        public VBNETLanguageInformation(IParser p)
        {
            this.parser = p;
            InitKeywords();
        }

        protected override void InitKeywords()
        {
            List<string> keys = new List<string>();
            List<string> type_keys = new List<string>();
            keywords.Add("AddHandler", "AddHandler"); keys.Add("AddHandler");
            keywords.Add("AddressOf", "AddressOf"); keys.Add("AddressOf");
            keywords.Add("Alias", "Alias"); keys.Add("Alias");
            keywords.Add("And", "And"); keys.Add("And");
            keywords.Add("AndAlso", "AndAlso"); keys.Add("AndAlso");
            keywords.Add("Ansi", "Ansi"); keys.Add("Ansi");
            keywords.Add("As", "As"); keys.Add("As");
            keywords.Add("Assembly", "Assembly"); keys.Add("Assembly");
            keywords.Add("Auto", "Auto"); keys.Add("Auto");
            keywords.Add("Binary", "Binary"); keys.Add("Binary");
            keywords.Add("ByRef", "ByRef"); keys.Add("ByRef");
            keywords.Add("ByVal", "ByVal"); keys.Add("ByVal");
            keywords.Add("Call", "Call"); keys.Add("Call");
            keywords.Add("Case", "Case"); keys.Add("Case");
            keywords.Add("Catch", "Catch"); keys.Add("Catch");
            keywords.Add("CBool", "CBool"); keys.Add("CBool"); ignored_keywords.Add("CBool", "CBool");
            keywords.Add("CByte", "CByte"); keys.Add("CByte"); ignored_keywords.Add("CByte", "CByte");
            keywords.Add("CChar", "CChar"); keys.Add("CChar"); ignored_keywords.Add("CChar", "CChar");
            keywords.Add("CDate", "CDate"); keys.Add("CDate"); ignored_keywords.Add("CDate", "CDate");
            keywords.Add("CDbl", "CDbl"); keys.Add("CDbl"); ignored_keywords.Add("CDbl", "CDbl");
            keywords.Add("CDec", "CDec"); keys.Add("CDec"); ignored_keywords.Add("CDec", "CDec");
            keywords.Add("CInt", "CInt"); keys.Add("CInt"); ignored_keywords.Add("CInt", "CInt");
            keywords.Add("Class", "Class"); keys.Add("Class"); type_keys.Add("Class");
            keywords.Add("CLng", "CLng"); keys.Add("CLng"); ignored_keywords.Add("CLng", "CLng");
            keywords.Add("CObj", "CObj"); keys.Add("CObj"); ignored_keywords.Add("CObj", "CObj");
            keywords.Add("Compare", "Compare"); keys.Add("Compare");
            keywords.Add("Const", "Const"); keys.Add("Const");
            keywords.Add("CShort", "CShort"); keys.Add("CShort"); ignored_keywords.Add("CShort", "CShort");
            keywords.Add("CSng", "CSng"); keys.Add("CSng"); ignored_keywords.Add("CSgn", "CSgn");
            keywords.Add("CStr", "CStr"); keys.Add("CStr"); ignored_keywords.Add("CStr", "CStr");
            keywords.Add("CType", "CType"); keys.Add("CType"); ignored_keywords.Add("CType", "CType");
            keywords.Add("Declare", "Declare"); keys.Add("Declare");
            keywords.Add("Default", "Default"); keys.Add("Default");
            keywords.Add("Delegate", "Delegate"); keys.Add("Delegate");
            keywords.Add("Dim", "Dim"); keys.Add("Dim");
            keywords.Add("DirectCast", "DirectCast"); keys.Add("DirectCast");
            keywords.Add("Do", "Do"); keys.Add("Do");
            keywords.Add("Each", "Each"); keys.Add("Each");
            keywords.Add("Else", "Else"); keys.Add("Else");
            keywords.Add("ElseIf", "ElseIf"); keys.Add("ElseIf");
            keywords.Add("End", "End"); keys.Add("End");
            keywords.Add("EndIf", "EndIf"); keys.Add("EndIf");
            keywords.Add("Enum", "Enum"); keys.Add("Enum"); type_keys.Add("Enum");
            keywords.Add("Erase", "Erase"); keys.Add("Erase");
            keywords.Add("Error", "Error"); keys.Add("Error");
            keywords.Add("Event", "Event"); keys.Add("Event");
            keywords.Add("Exit", "Exit"); keys.Add("Exit");
            keywords.Add("Explicit", "Explicit"); keys.Add("Explicit");
            keywords.Add("Finally", "Finally"); keys.Add("Finally");
            keywords.Add("For", "For"); keys.Add("For");
            keywords.Add("Friend", "Friend"); keys.Add("Friend");
            keywords.Add("Function", "Function"); keys.Add("Function"); keyword_kinds.Add("Function", KeywordKind.Function); type_keys.Add("Function");
            keywords.Add("GetType", "GetType"); keys.Add("GetType"); ignored_keywords.Add("GetType", "GetType");
            keywords.Add("GoTo", "GoTo"); keys.Add("GoTo");
            keywords.Add("Handles", "Handles"); keys.Add("Handles");
            keywords.Add("Implements", "Implements"); keys.Add("Implements");
            keywords.Add("Imports", "Imports"); keys.Add("Imports"); keyword_kinds.Add("Imports", KeywordKind.Uses);
            keywords.Add("In", "In"); keys.Add("In");
            keywords.Add("Inherits", "Inherits"); keys.Add("Inherits");
            keywords.Add("Interface", "Interface"); keys.Add("Interface"); type_keys.Add("Interface");
            keywords.Add("Is", "Is"); keys.Add("Is");
            keywords.Add("Loop", "Loop"); keys.Add("Loop");
            keywords.Add("Mod", "Mod"); keys.Add("Mod");
            keywords.Add("Module", "Module"); keys.Add("Module"); keyword_kinds.Add("Module", KeywordKind.Unit);
            keywords.Add("MustInherit", "MustInherit"); keys.Add("MustInherit");
            keywords.Add("MustOverride", "MustOverride"); keys.Add("MustOverride");
            keywords.Add("New", "New"); keys.Add("New"); keyword_kinds.Add("New", KeywordKind.New);
            keywords.Add("Next", "Next"); keys.Add("Next");
            keywords.Add("Not", "Not"); keys.Add("Not");
            keywords.Add("Nothing", "Nothing"); keys.Add("Nothing");
            keywords.Add("NotInheritable", "NotInheritable"); keys.Add("NotInheritable");
            keywords.Add("NotOverridable", "NotOverridable"); keys.Add("NotOverridable");
            keywords.Add("Off", "Off"); keys.Add("Off");
            keywords.Add("On", "On"); keys.Add("On");
            keywords.Add("Option", "Option"); keys.Add("Option");
            keywords.Add("Optional", "Optional"); keys.Add("Optional");
            keywords.Add("Or", "Or"); keys.Add("Or");
            keywords.Add("OrElse", "OrElse"); keys.Add("OrElse");
            keywords.Add("Overloads", "Overloads"); keys.Add("Overloads");
            keywords.Add("Overridable", "Overridable"); keys.Add("Overridable");
            keywords.Add("Overrides", "Overrides"); keys.Add("Overrides");
            keywords.Add("ParamArray", "ParamArray"); keys.Add("ParamArray");
            keywords.Add("Preserve", "Preserve"); keys.Add("Preserve");
            keywords.Add("Private", "Private"); keys.Add("Private");
            keywords.Add("Property", "Property"); keys.Add("Property");
            keywords.Add("Protected", "Protected"); keys.Add("Protected");
            keywords.Add("Public", "Public"); keys.Add("Public");
            keywords.Add("RaiseEvent", "RaiseEvent"); keys.Add("RaiseEvent");
            keywords.Add("ReadOnly", "ReadOnly"); keys.Add("ReadOnly");
            keywords.Add("ReDim", "ReDim"); keys.Add("ReDim");
            keywords.Add("RemoveHandler", "RemoveHandler"); keys.Add("RemoveHandler");
            keywords.Add("Resume", "Resume"); keys.Add("Resume");
            keywords.Add("Return", "Return"); keys.Add("Return");
            keywords.Add("Select", "Select"); keys.Add("Select");
            keywords.Add("Set", "Set"); keys.Add("Set");
            keywords.Add("Shadows", "Shadows"); keys.Add("Shadows");
            keywords.Add("Shared", "Shared"); keys.Add("Shared");
            keywords.Add("Static", "Static"); keys.Add("Static");
            keywords.Add("Step", "Step"); keys.Add("Step");
            keywords.Add("Strict", "Strict"); keys.Add("Strict");
            keywords.Add("Structure", "Structure"); keys.Add("Structure"); type_keys.Add("Structure");
            keywords.Add("Sub", "Sub"); keys.Add("Sub"); keyword_kinds.Add("Sub", KeywordKind.Function); type_keys.Add("Sub");
            keywords.Add("SyncLock", "SyncLock"); keys.Add("SyncLock");
            keywords.Add("Then", "Then"); keys.Add("Then");
            keywords.Add("Throw", "Throw"); keys.Add("Throw");
            keywords.Add("To", "To"); keys.Add("To");
            keywords.Add("Try", "Try"); keys.Add("Try");
            keywords.Add("Until", "Until"); keys.Add("Until");
            keywords.Add("While", "While"); keys.Add("While");
            keywords.Add("With", "With"); keys.Add("With");
            keywords.Add("WithEvents", "WithEvents"); keys.Add("WithEvents");
            keywords.Add("WriteOnly", "WriteOnly"); keys.Add("WriteOnly");
            keywords.Add("Xor", "Xor"); keys.Add("Xor");
            keywords.Add("Continue", "Continue"); keys.Add("Continue");
            keywords.Add("Operator", "Operator"); keys.Add("Operator");
            keywords.Add("Using", "Using"); keys.Add("Using");
            keywords.Add("IsNot", "IsNot"); keys.Add("IsNot");
            keywords.Add("CSByte", "CSByte"); keys.Add("CSByte"); ignored_keywords.Add("CSByte", "CSByte");
            keywords.Add("CUShort", "CUShort"); keys.Add("CUShort"); ignored_keywords.Add("CUShort", "CUShort");
            keywords.Add("CUInt", "CUInt"); keys.Add("CUInt"); ignored_keywords.Add("CUInt", "CUInt");
            keywords.Add("CULng", "CULng"); keys.Add("CULng"); ignored_keywords.Add("CULng", "CULng");
            keywords.Add("Global", "Global"); keys.Add("Global");
            keywords.Add("TryCast", "TryCast"); keys.Add("TryCast"); ignored_keywords.Add("TryCast", "TryCast");
            keywords.Add("Of", "Of"); keys.Add("Of");
            keywords.Add("Partial", "Partial"); keys.Add("Partial");
            /*keywords.Add("type", "type"); keys.Add("type"); keyword_kinds.Add("type", KeywordKind.Type);
            keywords.Add("array", "array"); keys.Add("array"); type_keys.Add("array");
            keywords.Add("of", "of"); keys.Add("of"); keyword_kinds.Add("of", KeywordKind.Of);
            keywords.Add("private", "private"); keys.Add("private");
            keywords.Add("protected", "protected"); keys.Add("protected");
            keywords.Add("public", "public"); keys.Add("public");
            keywords.Add("inherited", "inherited"); keys.Add("inherited"); keyword_kinds.Add("inherited", KeywordKind.Inherited);
            keywords.Add("foreach", "foreach"); keys.Add("foreach");
            keywords.Add("try", "try"); keys.Add("try");
            keywords.Add("except", "except"); keys.Add("except");
            keywords.Add("finally", "finally"); keys.Add("finally");
            keywords.Add("uses", "uses"); keys.Add("uses"); keyword_kinds.Add("uses", KeywordKind.Uses);
            keywords.Add("unit", "unit"); keys.Add("unit"); keyword_kinds.Add("unit", KeywordKind.Unit);

            keywords.Add("procedure", "procedure"); keys.Add("procedure"); keyword_kinds.Add("procedure", KeywordKind.Function); type_keys.Add("procedure");
            keywords.Add("function", "function"); keys.Add("function"); keyword_kinds.Add("function", KeywordKind.Function); type_keys.Add("function");
            keywords.Add("constructor", "constructor"); keys.Add("constructor"); keyword_kinds.Add("constructor", KeywordKind.Constructor);
            keywords.Add("destructor", "destructor"); keys.Add("destructor"); keyword_kinds.Add("destructor", KeywordKind.Destructor);
            keywords.Add("interface", "interface"); keys.Add("interface");
            keywords.Add("implementation", "implementation"); keys.Add("implementation");
            keywords.Add("initialization", "initialization"); keys.Add("initialization");
            keywords.Add("finalization", "finalization"); keys.Add("finalization");
            keywords.Add("with", "with"); keys.Add("with");
            keywords.Add("abstract","abstract"); keys.Add("abstract");
            keywords.Add("virtual", "virtual"); keys.Add("virtual");
            keywords.Add("override", "override"); keys.Add("override");
            keywords.Add("reintroduce", "reintroduce"); keys.Add("reintroduce");
            keywords.Add("static", "static"); keys.Add("static");

            keywords.Add("case", "case"); keys.Add("case");
            keywords.Add("var", "var"); keys.Add("var"); keyword_kinds.Add("var", KeywordKind.Var);
            keywords.Add("const", "const"); keys.Add("const"); keyword_kinds.Add("const", KeywordKind.Const);
            keywords.Add("if", "if"); keys.Add("if");
            keywords.Add("then", "then"); keys.Add("then");
            keywords.Add("else", "else"); keys.Add("else");
            keywords.Add("in", "in"); keys.Add("in");
            keywords.Add("operator", "operator"); keys.Add("operator");
            keywords.Add("raise", "raise"); keys.Add("raise"); keyword_kinds.Add("raise", KeywordKind.Raise);
            keywords.Add("program", "program"); keys.Add("program"); keyword_kinds.Add("program", KeywordKind.Program);
            keywords.Add("new", "new"); keys.Add("new"); keyword_kinds.Add("new", KeywordKind.New);
            keywords.Add("nil", "nil"); keys.Add("nil");*/
            keywords_array = keys.ToArray();
            type_keywords_array = type_keys.ToArray();
        }

        public override string SystemUnitName
        {
            get
            {
                return "VBSystem";
            }
        }

        public override bool CaseSensitive
        {
            get
            {
                return false;
            }
        }

        public override bool IncludeDotNetEntities
        {
            get
            {
                return true;
            }
        }

        public override string SkipNew(int off, string Text, ref KeywordKind keyw)
        {
            int tmp = off;
            string expr = null;
            while (off >= 0 && Char.IsLetterOrDigit(Text[off])) off--;
            while (off >= 0 && (Text[off] == ' ' || char.IsControl(Text[off]))) off--;
            if (off >= 1 && Text[off] == '=')
            {
                off -= 2;
                while (off >= 0 && (Text[off] == ' ' || char.IsControl(Text[off]))) off--;
                if (off >= 0 && (Text[off] == '_' || char.IsLetterOrDigit(Text[off]) || Text[off] == ')'))
                    expr = FindExpression(off + 1, Text, 0, 0, out keyw);
            }
            return expr;
        }

        protected override string GetTemplateString(ITypeScope scope)
        {
            string[] generic_params = scope.TemplateArguments;
            if (generic_params != null && generic_params.Length > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append('(');
                for (int i = 0; i < generic_params.Length; i++)
                {
                    sb.Append("Of ");
                    sb.Append(generic_params[i]);
                    if (i < generic_params.Length - 1)
                        sb.Append(',');
                }
                sb.Append(')');
                return sb.ToString();
            }
            return null;
        }

        protected override string GetDescriptionForType(ITypeScope scope)
        {
            string template_str = GetTemplateString(scope);
            switch (scope.ElemKind)
            {
                case SymbolKind.Class:
                    if (scope.TopScope != null && scope.TopScope.Name != "")
                        return (scope.IsFinal ? "NotInheritable " : "") + "Class " + scope.TopScope.Name + "." + scope.Name + template_str;
                    else return (scope.IsFinal ? "NotInheritable " : "") + "Class " + scope.Name + template_str;
                case SymbolKind.Interface:
                    if (scope.TopScope != null && scope.TopScope.Name != "")
                        return "Interface " + scope.TopScope.Name + "." + scope.Name + template_str;
                    else return "Interface " + scope.Name + template_str;
                case SymbolKind.Enum:
                    if (scope.TopScope != null && scope.TopScope.Name != "")
                        return "Enum " + scope.TopScope.Name + "." + scope.Name;
                    else return "Enum " + scope.Name;
                case SymbolKind.Delegate:
                    if (scope.TopScope != null && scope.TopScope.Name != "")
                        return "Delegate " + scope.TopScope.Name + "." + scope.Name + template_str;
                    else return "Delegate " + scope.Name + template_str;
                case SymbolKind.Struct:
                    if (scope.TopScope != null && scope.TopScope.Name != "")
                        return "Struct " + scope.TopScope.Name + "." + scope.Name + template_str;
                    else return "Struct " + scope.Name + template_str;
            }

            if (scope.TopScope != null)
                return scope.TopScope.Name + "." + scope.Name;
            else return scope.Name;
        }

        protected override string GetDescriptionForCompiledType(ICompiledTypeScope scope)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("Class ");
            string s = GetFullTypeName(scope.CompiledType);
            sb.Append(s);
            ITypeScope[] instances = scope.GenericInstances;
            if (instances.Length > 0)
            {
                int ind = s.IndexOf('(');
                if (ind != -1) sb.Append(s.Substring(0, ind));
                else
                    sb.Append(s);
                sb.Append('(');
                sb.Append("Of ");
                for (int i = 0; i < instances.Length; i++)
                {
                    sb.Append(instances[i].Name);
                    if (i < instances.Length - 1) sb.Append(',');
                }
                sb.Append(')');
            }
            return sb.ToString();
        }

        public override string ParameterDelimiter
        {
            get
            {
                return ",";
            }
        }

        public override string GetDescriptionForModule(IInterfaceUnitScope scope)
        {
            return "Module " + scope.Name;
        }

        private string get_access_modifier(access_modifer mod)
        {
            switch (mod)
            {
                case access_modifer.private_modifer: return "Private ";
                case access_modifer.public_modifer: return "Public ";
                case access_modifer.protected_modifer: return "Protected ";
                case access_modifer.internal_modifer: return "Friend ";
                case access_modifer.none: return "";
            }
            return "";
        }

        private string get_other_modifier(IElementScope scope)
        {
            if (scope.IsVirtual) return "Overridable ";
            if (scope.IsAbstract) return "MustOverride ";
            if (scope.IsStatic) return "Shared ";
            if (scope.IsOverride) return "Overrides ";
            if (scope.IsReintroduce) return "Shadows ";
            return "";
        }

        protected override string GetDescriptionForCompiledMethod(ICompiledMethodScope scope)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(get_access_modifier(scope.AccessModifier));
            if (scope.CompiledMethod.IsStatic) sb.Append("Shared ");
            else if (scope.CompiledMethod.IsVirtual) sb.Append("Overridable ");
            else if (scope.CompiledMethod.IsAbstract) sb.Append("MustOverride ");
            if (scope.ReturnType == null)
                sb.Append("Sub ");
            else
                sb.Append("Function ");
            //sb.Append(GetShortTypeName(scope.CompiledMethod.DeclaringType));
            //sb.Append(".");
            sb.Append(scope.CompiledMethod.Name);
            sb.Append('(');
            ParameterInfo[] pis = scope.CompiledMethod.GetParameters();
            for (int i = 0; i < pis.Length; i++)
            {
                if (pis[i].ParameterType.IsByRef)
                    sb.Append("ByRef ");
                sb.Append(pis[i].Name);
                sb.Append(" As ");
                if (!pis[i].ParameterType.IsByRef)
                    sb.Append(GetFullTypeName(pis[i].ParameterType));
                else sb.Append(GetFullTypeName(pis[i].ParameterType.GetElementType()));
                if (i < pis.Length - 1)
                    sb.Append(", ");
            }
            sb.Append(')');
            if (scope.ReturnType != null)
            {
                sb.Append(" As " + GetSimpleDescription(scope.ReturnType));
            }

            //else if (scope.CompiledMethod.IsHideBySig) sb.Append("; reintroduce");
            return sb.ToString();
        }

        private string kind_of_param(IElementScope scope)
        {
            switch (scope.ParamKind)
            {
                case PascalABCCompiler.SyntaxTree.parametr_kind.var_parametr: return "ByRef ";
                case PascalABCCompiler.SyntaxTree.parametr_kind.params_parametr: return "ParamArray ";
                case PascalABCCompiler.SyntaxTree.parametr_kind.out_parametr: return "ByRef ";
            }
            return "";
        }

        private string get_index_description(IElementScope scope)
        {
            ITypeScope[] indexers = scope.Indexers;
            if (indexers == null) return "";
            StringBuilder sb = new StringBuilder();
            sb.Append('(');
            for (int i = 0; i < indexers.Length; i++)
            {
                sb.Append(indexers[i].Name);
                if (i < indexers.Length - 1)
                    sb.Append(',');
            }
            sb.Append(')');
            return sb.ToString();
        }

        public override string GetStandardTypeByKeyword(KeywordKind keyw)
        {
            switch (keyw)
            {
                case KeywordKind.ByteType: return "Byte";
                case KeywordKind.SByteType: return "SByte";
                case KeywordKind.ShortType: return "Short";
                case KeywordKind.UShortType: return "UShort";
                case KeywordKind.IntType: return "Integer";
                case KeywordKind.UIntType: return "UInteger";
                case KeywordKind.Int64Type: return "Long";
                case KeywordKind.UInt64Type: return "ULong";
                case KeywordKind.DoubleType: return "Double";
                case KeywordKind.FloatType: return "Single";
                case KeywordKind.CharType: return "Char";
                case KeywordKind.BoolType: return "Boolean";
            }
            return null;
        }

        public override string GetClassKeyword(class_keyword keyw)
        {
            switch (keyw)
            {
                case class_keyword.Class: return "Class";
                case class_keyword.Interface: return "Interface";
                case class_keyword.Record:
                case class_keyword.Struct: return "Struct";
            }
            return "";
        }

        protected override string GetAccessModifier(access_modifer mod)
        {
            switch (mod)
            {
                case access_modifer.private_modifer: return "Private";
                case access_modifer.protected_modifer: return "Protected";
                case access_modifer.public_modifer: return "Public";
                case access_modifer.internal_modifer: return "Friend";
            }
            return "";
        }

        protected override string GetSimpleDescriptionForElementScope(IElementScope scope)
        {
            string type_name = GetSimpleDescription(scope.Type);
            if (type_name.StartsWith("$")) type_name = type_name.Substring(1, type_name.Length - 1);
            return kind_of_param(scope) + scope.Name + " As " + type_name;
        }

        protected override string GetSimpleDescriptionForProcedure(IProcScope scope)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (scope.TopScope is ITypeScope && scope.Realization == null)
                sb.Append(GetTopScopeName(scope.TopScope));
            sb.Append(scope.Name); sb.Append('(');
            IElementScope[] parameters = scope.Parameters;
            for (int i = 0; i < parameters.Length; i++)
            {
                sb.Append(GetSimpleDescription(parameters[i]));
                if (i < parameters.Length - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(')');
            return sb.ToString();
        }

        protected override string GetSimpleDescriptionForCompiledType(ICompiledTypeScope scope)
        {
            string s = GetFullTypeName(scope.CompiledType);
            ITypeScope[] instances = scope.GenericInstances;
            if (instances.Length > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                int ind = s.IndexOf('(');
                if (ind != -1) sb.Append(s.Substring(0, ind));
                else
                    sb.Append(s);
                sb.Append('(');
                sb.Append("Of ");
                for (int i = 0; i < instances.Length; i++)
                {
                    sb.Append(instances[i].Name);
                    if (i < instances.Length - 1) sb.Append(',');
                }
                sb.Append(')');
                s = sb.ToString();
            }
            return s;
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
                case SymbolKind.Variable: sb.Append("Dim " + scope.Name + " As " + type_name); break;
                case SymbolKind.Parameter: sb.Append(kind_of_param(scope) + "Parameter " + scope.Name + " As " + type_name); break;
                case SymbolKind.Constant:
                    {
                        if (scope.ConstantValue == null)
                            sb.Append("Const " + scope.Name + " As " + type_name);
                        else sb.Append("Const " + scope.Name + " = " + scope.ConstantValue.ToString());
                    }
                    break;
                case SymbolKind.Event:
                    sb.Append(get_access_modifier(scope.AccessModifier));
                    sb.Append(get_other_modifier(scope));
                    sb.Append("Event " + scope.Name + " As " + type_name); break;
                case SymbolKind.Field:
                    sb.Append(get_access_modifier(scope.AccessModifier));
                    sb.Append(get_other_modifier(scope));
                    sb.Append(scope.Name + " As " + type_name); break;
                case SymbolKind.Property:
                    sb.Append(get_access_modifier(scope.AccessModifier));
                    sb.Append(get_other_modifier(scope));
                    sb.Append("Property " + scope.Name + get_index_description(scope) + " As " + type_name); break;
            }
            return sb.ToString();
        }

        protected override string GetDescriptionForCompiledProperty(ICompiledPropertyScope scope)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            MethodInfo acc = scope.CompiledProperty.GetGetMethod();
            sb.Append(get_access_modifier(scope.AccessModifier));
            if (acc != null)
            {

                if (acc.IsStatic) sb.Append("Shared ");
                else if (acc.IsVirtual) sb.Append("Overridable ");
                else if (acc.IsAbstract) sb.Append("MustOverride ");
            }
            sb.Append("Property " + scope.CompiledProperty.Name + " As " + GetSimpleDescription(scope.Type));
            return sb.ToString();
        }

        protected override string GetDescriptionForCompiledField(ICompiledFieldScope scope)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(get_access_modifier(scope.AccessModifier));
            if (scope.CompiledField.IsStatic) sb.Append("Shared ");
            if (!scope.CompiledField.IsLiteral)
                sb.Append(scope.CompiledField.Name + " As " + GetSimpleDescription(scope.Type));
            else
                sb.Append("Const " + scope.CompiledField.Name + " As " + GetSimpleDescription(scope.Type));
            return sb.ToString();
        }

        protected override string GetDescriptionForCompiledEvent(ICompiledEventScope scope)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(get_access_modifier(scope.AccessModifier));
            sb.Append("Event " + scope.CompiledEvent.Name + " As " + GetSimpleDescription(scope.Type));
            return sb.ToString();
        }

        protected override string GetDescriptionForCompiledConstructor(ICompiledConstructorScope scope)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("New");
            sb.Append('(');
            ParameterInfo[] pis = scope.CompiledConstructor.GetParameters();
            for (int i = 0; i < pis.Length; i++)
            {
                if (pis[i].ParameterType.IsByRef)
                    sb.Append("ByRef ");
                sb.Append(pis[i].Name);
                sb.Append(" As ");
                if (!pis[i].ParameterType.IsByRef)
                    sb.Append(GetFullTypeName(pis[i].ParameterType));
                else sb.Append(GetFullTypeName(pis[i].ParameterType.GetElementType()));
                if (i < pis.Length - 1)
                    sb.Append(", ");
            }
            sb.Append(')');
            return sb.ToString();
        }

        protected override string GetDescriptionForProcedure(IProcScope scope)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (scope.IsStatic) sb.Append("Shared ");
            else if (scope.IsVirtual) sb.Append("Overridable ");
            else if (scope.IsAbstract) sb.Append("MustOverride ");
            else if (scope.IsOverride) sb.Append("Overrides ");
            else if (scope.IsReintroduce) sb.Append("Shadows ");
            if (scope.IsConstructor())
                sb.Append("New");
            else
                if (scope.ReturnType == null)
                    sb.Append("Sub ");
                else
                    sb.Append("Function ");
            sb.Append(scope.Name); sb.Append('(');
            IElementScope[] parameters = scope.Parameters;
            for (int i = 0; i < parameters.Length; i++)
            {
                sb.Append(GetSimpleDescription(parameters[i]));
                if (i < parameters.Length - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(')');
            if (scope.ReturnType != null && !scope.IsConstructor())
                sb.Append(" As " + GetSimpleDescription(scope.ReturnType));

            return sb.ToString();
        }

        protected override string GetDescriptionForNamespace(INamespaceScope scope)
        {
            return "Namespace " + scope.Name;
        }

        protected override string GetDescriptionForShortString(IShortStringScope scope)
        {
            if (scope.Length != null)
                return "String * " + scope.Length.ToString();
            return scope.Name;
        }

        protected override string GetFullTypeName(Type ctn, bool no_alias=true)
        {
            TypeCode tc = Type.GetTypeCode(ctn);
            if (ctn.FullName == null && !ctn.IsArray && !ctn.IsGenericTypeDefinition)
                return "T";
            if (!ctn.IsEnum)
                switch (tc)
                {
                    case TypeCode.Int32: return "Integer";
                    case TypeCode.Double: return "Double";
                    case TypeCode.Boolean: return "Boolean";
                    case TypeCode.String: return "String";
                    case TypeCode.Char: return "Char";
                    case TypeCode.Byte: return "Byte";
                    case TypeCode.SByte: return "SByte";
                    case TypeCode.Int16: return "Short";
                    case TypeCode.Int64: return "Long";
                    case TypeCode.UInt16: return "UShort";
                    case TypeCode.UInt32: return "UInteger";
                    case TypeCode.UInt64: return "ULong";
                    case TypeCode.Single: return "Single";
                }
            else return ctn.FullName;

            if (ctn.Name.Contains("`"))
            {
                int len = ctn.GetGenericArguments().Length;
                Type[] gen_ps = ctn.GetGenericArguments();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(ctn.Namespace + "." + ctn.Name.Substring(0, ctn.Name.IndexOf('`')));
                sb.Append('(');
                sb.Append("Of ");
                for (int i = 0; i < len; i++)
                {
                    sb.Append(gen_ps[i].Name);
                    if (i < len - 1)
                        sb.Append(',');
                }
                sb.Append(')');
                return sb.ToString();
            }
            if (ctn.IsArray) return GetFullTypeName(ctn.GetElementType()) + "()";
            if (ctn == Type.GetType("System.Void*")) return "System.IntPtr";
            return ctn.FullName;
        }

        public override string GetShortTypeName(Type ctn, bool noalias=true)
        {
            TypeCode tc = Type.GetTypeCode(ctn);
            if (ctn.FullName == null && !ctn.IsArray && !ctn.IsGenericTypeDefinition)
                return "T";
            if (!ctn.IsEnum)
                switch (tc)
                {
                    case TypeCode.Int32: return "Integer";
                    case TypeCode.Double: return "Double";
                    case TypeCode.Boolean: return "Boolean";
                    case TypeCode.String: return "String";
                    case TypeCode.Char: return "Char";
                    case TypeCode.Byte: return "Byte";
                    case TypeCode.SByte: return "SByte";
                    case TypeCode.Int16: return "Short";
                    case TypeCode.Int64: return "Long";
                    case TypeCode.UInt16: return "UShort";
                    case TypeCode.UInt32: return "UInteger";
                    case TypeCode.UInt64: return "ULong";
                    case TypeCode.Single: return "Single";
                }
            else
                return ctn.Name;
            if (ctn.Name.Contains("`"))
            {
                int len = ctn.GetGenericArguments().Length;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(ctn.Name.Substring(0, ctn.Name.IndexOf('`')));
                sb.Append('(');
                sb.Append(')');
                /*sb.Append('<');
                for (int i=0; i<len; i++)
                {
                    sb.Append('T');
                    if (i<len-1)
                    sb.Append(',');
                }
                sb.Append('>');*/
                return sb.ToString();
            }
            //if (ctn.IsArray) return "array of "+GetTypeName(ctn.GetElementType());
            //if (ctn == Type.GetType("System.Void*")) return StringConstants.pointer_type_name;
            return ctn.Name;
        }

        public override string GetStringForChar(char c)
        {
            return "\"" + c.ToString() + "\"";
        }

        public override KeywordKind TestForKeyword(string Text, int i)
        {
            StringBuilder sb = new StringBuilder();
            int j = i;
            bool in_keyw = false;
            while (j >= 0 && Text[j] != '\n')
                j--;
            Stack<char> kav_stack = new Stack<char>();
            j++;
            while (j <= i)
            {
                if (Text[j] == '"')
                {
                    if (kav_stack.Count == 0 && !in_keyw)
                        kav_stack.Push('"');
                    else if (kav_stack.Count > 0)
                        kav_stack.Pop();
                }
                j++;
            }
            j = i;
            if (kav_stack.Count != 0 || in_keyw) return PascalABCCompiler.Parsers.KeywordKind.Punkt;
            if (j >= 0 && Text[j] == '.') return PascalABCCompiler.Parsers.KeywordKind.Punkt;
            while (j >= 0 && Text[j] != '"' && Text[j] != '\n')
            {
                //if (Text[j] == '{') return PascalABCCompiler.Parsers.KeywordKind.Punkt;
                if (Text[j] == '\'') return PascalABCCompiler.Parsers.KeywordKind.Punkt;
                j--;
            }
            //if (j>= 0 && Text[j] == '\'') return CodeCompletion.KeywordKind.kw_punkt;
            while (i >= 0 && (Text[i] == ' ' || char.IsControl(Text[i]))) i--;
            if (i >= 0 && Text[i] == ':') return PascalABCCompiler.Parsers.KeywordKind.Colon;
            if (i >= 0 && Text[i] == ',')
            {

                while (i >= 0 && Text[i] != '\n')
                {
                    if (char.IsLetterOrDigit(Text[i]))
                        sb.Insert(0, Text[i]);
                    else
                    {
                        PascalABCCompiler.Parsers.KeywordKind keyw = GetKeywordKind(sb.ToString());
                        if (keyw == PascalABCCompiler.Parsers.KeywordKind.Uses)
                            return PascalABCCompiler.Parsers.KeywordKind.Uses;
                        else if (keyw == PascalABCCompiler.Parsers.KeywordKind.Var)
                            return PascalABCCompiler.Parsers.KeywordKind.Var;
                        else sb.Remove(0, sb.Length);
                    }
                    i--;
                }
            }
            else
                while (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_'))
                {
                    sb.Insert(0, Text[i]);
                    i--;
                }
            string s = sb.ToString().ToLower();

            return GetKeywordKind(s);
        }

        private bool CheckForComment(string Text, int off)
        {
            int i = off;
            Stack<char> kav = new Stack<char>();
            bool is_comm = false;
            while (i >= 0 && !is_comm && Text[i] != '\n')
            {
                if (Text[i] == '"')
                {
                    if (kav.Count == 0) kav.Push('"');
                    else kav.Pop();
                }
                else if (Text[i] == '\'')
                {
                    if (kav.Count == 0)
                        return true;
                }
                i--;
            }
            return is_comm;
        }

        private void TestForKeyword(string Text, int i, ref int bound, bool sym_punkt, out KeywordKind keyword)
        {
            StringBuilder sb = new StringBuilder();
            while (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_'))
            {
                sb.Insert(0, Text[i]);
                i--;
            }
            string s = sb.ToString().ToLower();
            if (s == "new")
            {
                bound = i + 1;
                keyword = KeywordKind.New;
            }
            else if ((s == "sub" || s == "function") && !sym_punkt)
            {
                keyword = KeywordKind.Function;
            }
            else if (s == "imports")
            {
                keyword = KeywordKind.Uses;
            }
            else if (s == "throw")
            {
                keyword = KeywordKind.Raise;
            }
            else if (keywords.ContainsKey(s) && !ignored_keywords.ContainsKey(s))
            {
                keyword = KeywordKind.CommonKeyword;
            }
            else keyword = KeywordKind.None;
        }

        public override string FindExpression(int off, string Text, int line, int col, out KeywordKind keyw)
        {
            int i = off - 1;
            int bound = 0;
            bool punkt_sym = false;
            keyw = KeywordKind.None;
            System.Text.StringBuilder sb = new StringBuilder();
            Stack<char> tokens = new Stack<char>();
            Stack<char> kav = new Stack<char>();
            Stack<char> ugl_skobki = new Stack<char>();
            int num_in_ident = -1;
            keyw = TestForKeyword(Text, i);
            if (keyw == KeywordKind.Punkt) return "";
            while (i >= bound)
            {
                bool end = false;
                char ch = Text[i];

                if (kav.Count == 0 && (char.IsLetterOrDigit(ch) || ch == '_' || ch == '"'))
                {
                    num_in_ident = i;
                    if (kav.Count == 0 && tokens.Count == 0)
                    {
                        int tmp = i;
                        if (ch == '"')
                        {
                            i--;
                            if (kav.Count == 0)
                                kav.Push('"');
                            while (i >= 0 && Text[i] != '"')
                                i--;
                            if (i >= 0)
                                i--;
                        }
                        else
                            while (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_'))
                            {
                                i--;
                            }
                        while (i >= 0 && (Text[i] == ' ' || char.IsControl(Text[i])) && Text[i] != '\n' && Text[i] != '\'')
                        {
                            i--;
                        }
                        if (i >= 0)
                            //if (Text[i] == '\n')
                            //	return sb.ToString();
                            if (Text[i] == '\'')
                                return "";
                        if (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_'))
                        {
                            bound = i + 1;
                            TestForKeyword(Text, i, ref bound, punkt_sym, out keyw);
                        }
                        else if (i >= 0 && Text[i] == '"') return "";
                        i = tmp;
                    }
                    sb.Insert(0, ch);//.Append(Text[i]);
                }
                /*else if (ch == '\n')
                {
                    return sb.ToString();
                }*/
                else if (ch == '.')
                {
                    if (ch == '.' && i >= 1 && Text[i - 1] == '.') end = true; else sb.Insert(0, ch);
                    if (ch != '.') punkt_sym = true;
                }
                else
                    switch (ch)
                    {
                        case ')':
                            if (kav.Count == 0) tokens.Push(ch); sb.Insert(0, ch); punkt_sym = true; break;
                        case '>':
                            if (tokens.Count == 0)
                            {
                                if (ugl_skobki.Count > 0 || i == off - 1 || i + 1 < Text.Length && (Text[i + 1] == '.' || Text[i + 1] == '('))
                                {
                                    ugl_skobki.Push('>');
                                    sb.Insert(0, ch);
                                }
                                else
                                    end = true;
                            }
                            else
                                sb.Insert(0, ch);
                            break;
                        case '<':
                            if (tokens.Count == 0)
                            {
                                if (ugl_skobki.Count > 0)
                                {
                                    ugl_skobki.Pop();
                                    sb.Insert(0, ch);
                                }
                                else
                                    end = true;
                            }
                            else
                                sb.Insert(0, ch);
                            break;
                        case '(':
                            if (kav.Count == 0)
                            {
                                if (tokens.Count > 0)
                                {
                                    tokens.Pop();
                                    punkt_sym = true;
                                    sb.Insert(0, ch);
                                    if (ch == '(')
                                    {
                                        int tmp = i--;
                                        /*while (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&'))
                                        {
                                            i--;
                                        }*/
                                        while (i >= 0 && (Text[i] == ' ' || char.IsControl(Text[i])))
                                        {
                                            i--;
                                        }
                                        if (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_'))
                                        {
                                            bound = i + 1;
                                            TestForKeyword(Text, i, ref bound, punkt_sym, out keyw);
                                            if (keyw != KeywordKind.None && tokens.Count == 0)
                                            {
                                                end = true;
                                            }
                                            else bound = 0;
                                        }
                                        else if (i >= 0 && Text[i] == '"') return "";
                                        i = tmp;
                                    }
                                }
                                else end = true;
                            }
                            else sb.Insert(0, ch); punkt_sym = true;
                            break;
                        case '"':
                            if (kav.Count == 0)
                                kav.Push(ch);
                            else
                                kav.Pop();
                            sb.Insert(0, ch);
                            break;
                        default:
                            if (!(ch == ' ' || char.IsControl(ch)))
                            {
                                if (kav.Count == 0)
                                {
                                    if (tokens.Count == 0) end = true;
                                    else sb.Insert(0, ch);
                                }
                                else sb.Insert(0, ch);
                            }
                            else
                            {
                                if (Text[i] == '\n')
                                {
                                    end = true;
                                }
                                else
                                    sb.Insert(0, ch);
                            }
                            punkt_sym = true;
                            break;
                    }

                if (end)
                {
                    if (CheckForComment(Text, i))
                    {
                        int new_line_ind = sb.ToString().IndexOf('\n');
                        if (new_line_ind != -1) sb = sb.Remove(0, new_line_ind + 1);
                        else sb = sb.Remove(0, sb.Length);
                    }
                    break;
                }
                i--;
            }
            if (!keywords.ContainsKey(sb.ToString().Trim(' ', '\n', '\t')))
                //return RemovePossibleKeywords(sb);
                return sb.ToString();
            else return null;
        }

        public override string FindExpressionForMethod(int off, string Text, int line, int col, char pressed_key, ref int num_param)
        {
            int i = off - 1;
            string pattern = null;
            int bound = 0;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            Stack<char> tokens = new Stack<char>();
            Stack<char> kav = new Stack<char>();
            Stack<char> skobki = new Stack<char>();
            Stack<char> ugl_skobki = new Stack<char>();
            bool comma_pressed = pressed_key == ',';
            int num_in_ident = -1;
            bool punkt_sym = false;
            KeywordKind keyw = TestForKeyword(Text, i);
            bool on_skobka = false;
            if (keyw == KeywordKind.Punkt)
                return "";
            try
            {
                while (i >= bound)
                {
                    bool end = false;
                    char ch = Text[i];
                    if (char.IsLetterOrDigit(ch) || ch == '_')
                    {
                        num_in_ident = i;
                        if (kav.Count == 0 && tokens.Count == 0)
                        {
                            int tmp = i;
                            while (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_'))
                            {
                                i--;
                            }
                            while (i >= 0 && (Text[i] == ' ' || char.IsControl(Text[i])) && Text[i] != '\n')
                            {
                                i--;
                            }
                            if (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_'))
                            {
                                bound = i + 1;
                                TestForKeyword(Text, i, ref bound, punkt_sym, out keyw);
                                if (keyw == KeywordKind.New && comma_pressed)
                                    bound = 0;
                                if (keyw == KeywordKind.Function || keyw == KeywordKind.Constructor || keyw == KeywordKind.Destructor)
                                    return "";
                            }
                            else if (i >= 0 && Text[i] == '"') return "";
                            i = tmp;
                        }
                        sb.Insert(0, ch);//.Append(Text[i]);
                    }
                    else if (ch == '.') sb.Insert(0, ch);
                    else
                        switch (ch)
                        {
                            case ')':
                            case '>':
                                if (kav.Count == 0)
                                {
                                    if (ch != '>')
                                        tokens.Push(ch);
                                    if (ch == ')')
                                        skobki.Push(ch);
                                    if (tokens.Count > 0 || pressed_key == ',')
                                        sb.Insert(0, ch);
                                    else if (i == off - 1 || ugl_skobki.Count > 0 || i + 1 < Text.Length && (Text[i + 1] == '.' || Text[i + 1] == '('))
                                    {
                                        tokens.Push(ch);
                                        ugl_skobki.Push(ch);
                                        sb.Insert(0, ch);
                                    }
                                    else
                                        end = true;
                                }
                                else
                                    sb.Insert(0, ch); break;
                            case '<':
                            case '(':
                                if (kav.Count == 0)//esli ne v kavychkah
                                {
                                    if (ch == '(')
                                        if (skobki.Count > 0)
                                            skobki.Pop();
                                        else skobki.Push('(');
                                    //esli byli zakryvaushie tokeny (polagaem, chto skobki korrektny, esli net, to parser v lubom sluchae ne proparsit
                                    if (tokens.Count > 0)
                                    {
                                        if (ch != '<')
                                            tokens.Pop();
                                        else if (ugl_skobki.Count > 0)
                                        {
                                            tokens.Pop();
                                            ugl_skobki.Pop();
                                        }
                                        sb.Insert(0, ch);//dobavljaem k stroke
                                        if (ch == '(')
                                        {
                                            int tmp = i--;
                                            /*while (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&'))
                                            {
                                                i--;
                                            }*/
                                            while (i >= 0 && (Text[i] == ' ' || char.IsControl(Text[i])) && Text[i] != '\n')
                                            {
                                                i--;
                                            }

                                            if (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_'))
                                            {
                                                bound = i + 1;
                                                TestForKeyword(Text, i, ref bound, punkt_sym, out keyw);
                                                if (keyw == KeywordKind.New)
                                                    bound = 0;
                                                else
                                                    if (keyw != KeywordKind.None)
                                                        end = true;
                                                    else
                                                        bound = 0;
                                            }
                                            else if (i >= 0 && Text[i] == '"') return "";
                                            i = tmp;
                                        }
                                    }
                                    else if (ch == '<' || ch == '>')
                                    {
                                        if (tokens.Count > 0 || pressed_key == ',')
                                            sb.Insert(0, ch);
                                        else
                                            end = true;
                                    }
                                    else
                                        if (!comma_pressed) //esli my ne v parametrah
                                            end = true; //zakonchili, tak kak doshli do pervoj skobki
                                        else
                                        {
                                            sb.Remove(0, sb.Length);//doshli do skobki, byla nazhata zapjataja, poetomu udaljaem vse parametry
                                            if (ch == '(')
                                                on_skobka = true;
                                            //on_skobka = true;
                                            comma_pressed = false;
                                        }
                                }
                                else sb.Insert(0, ch);
                                break;
                            case '"':
                                if (kav.Count == 0)
                                    kav.Push(ch);
                                else
                                    kav.Pop();
                                sb.Insert(0, ch);
                                break;
                            default:
                                if (!(ch == ' ' || char.IsControl(ch)))
                                {
                                    if (kav.Count == 0)//ne v kavychkah
                                    {
                                        if (tokens.Count == 0)
                                        {
                                            if (ch != ',' && !comma_pressed)
                                                end = true;//esli ne na zapjatoj i ne v parametrah, to finish
                                            else if (ch == ',' && !comma_pressed)
                                                end = true;
                                            else if (ch == ',' && (pressed_key == '(' || pressed_key == '['))
                                                end = true;
                                            else
                                                sb.Insert(0, ch);//prodolzhaem
                                        }
                                        else
                                            sb.Insert(0, ch);//esli est skobki, prodolzhaem
                                        if (!end && ch == ',')
                                        {
                                            if (tokens.Count == 0)
                                                num_param++;//esli na zapjatoj, uvelichivaem nomer parametra
                                        }
                                    }
                                    else
                                        sb.Insert(0, ch);//prodolzhaem

                                }
                                else
                                    if (Text[i] == '\n')
                                    {
                                        //if (CheckForComment(Text,i-1)) //proverjaem, net li kommenta ne predydushej stroke
                                        end = true;//esli est to finish
                                    }
                                    else
                                        sb.Insert(0, ch);
                                break;
                        }

                    if (end)
                    {
                        if (comma_pressed && !on_skobka)
                            return "";
                        if (CheckForComment(Text, i))//proverka na kommentarii
                        {
                            int new_line_ind = sb.ToString().IndexOf('\n');
                            if (new_line_ind != -1) sb = sb.Remove(0, new_line_ind + 1);
                            else sb = sb.Remove(0, sb.Length);
                        }
                        break;
                    }
                    i--;
                }
                if (pressed_key == ',')
                    num_param++;
            }
            catch (Exception e)
            {

            }
            if (pressed_key == ',' && (!on_skobka || skobki.Count == 0))
                return "";
            //return RemovePossibleKeywords(sb);
            return sb.ToString();
        }

        public override string FindExpressionFromAnyPosition(int off, string Text, int line, int col, out KeywordKind keyw, out string expr_without_brackets)
        {
            int i = off - 1;
            expr_without_brackets = null;
            keyw = KeywordKind.None;
            if (i < 0)
                return "";
            bool is_char = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (Text[i] != ' ' && (Char.IsLetterOrDigit(Text[i]) || Text[i] == '_'))
            {
                //sb.Remove(0,sb.Length);
                while (i >= 0 && (Char.IsLetterOrDigit(Text[i]) || Text[i] == '_'))
                {
                    //sb.Insert(0,Text[i]);//.Append(Text[i]);
                    i--;
                }
                is_char = true;
            }
            i = off;
            if (i < Text.Length && Text[i] != ' ' && (Char.IsLetterOrDigit(Text[i]) || Text[i] == '_'))
            {
                while (i < Text.Length && (Char.IsLetterOrDigit(Text[i]) || Text[i] == '_'))
                {
                    //sb.Append(Text[i]);//.Append(Text[i]);
                    i++;
                }
                is_char = true;
            }
            if (is_char)
            {
                expr_without_brackets = FindExpression(i, Text, line, col, out keyw);
            }
            bool is_new = keyw == KeywordKind.New;
            bool meth_call = false;
            KeywordKind new_keyw = KeywordKind.None;
            int j = i;
            bool in_comment = false;
            while (j < Text.Length)
            {
                char c = Text[j];

                if (c == '(' && !in_comment)
                {
                    Stack<char> sk_stack = new Stack<char>();
                    in_comment = false;
                    bool in_kav = false;
                    sk_stack.Push('(');
                    j++;
                    while (j < Text.Length)
                    {
                        c = Text[j];
                        if (c == '(' && !in_kav)
                            sk_stack.Push('(');
                        else
                            if (c == ')' && !in_kav)
                            {
                                if (sk_stack.Count == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    sk_stack.Pop();
                                    if (sk_stack.Count == 0)
                                    {
                                        i = j + 1;
                                        meth_call = true;
                                        break;
                                    }
                                }
                            }
                            else if (c == '"')
                            {
                                in_kav = !in_kav;
                            }
                            else if (c == '\'' && !in_kav)
                            {
                                break;
                            }

                        j++;
                    }
                    break;
                }
                else if (c == '\n')
                {
                    break;
                }
                else
                {
                    if (!in_comment)
                        break;
                }
                j++;
            }
            if (is_new && string.Compare(expr_without_brackets.Trim(' ', '\n', '\t', '\r'), "new", true) == 0 && !meth_call)
            {
                expr_without_brackets = null;
                return null;
            }
            if (is_char)
            {
                return FindExpression(i, Text, line, col, out new_keyw);
            }
            return null;
        }
    }
	
}