using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using System.Reflection;

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
            return paramDescription.Contains("...") || paramDescription.TrimStart().StartsWith("params");
        }

        public override string GetDescription(IBaseScope scope)
        {
            switch (scope.Kind)
            {
                case ScopeKind.Type: return GetDescriptionForType(scope as ITypeScope);
                case ScopeKind.CompiledType: return GetDescriptionForCompiledType(scope as ICompiledTypeScope);
                case ScopeKind.Delegate: return GetDescriptionForDelegate(scope as IProcType);
                case ScopeKind.TypeSynonim: return GetSynonimDescription(scope as ITypeSynonimScope);
                case ScopeKind.Array: return GetDescriptionForArray(scope as IArrayScope);
                case ScopeKind.Diapason: return GetDescriptionForDiapason(scope as IDiapasonScope);
                case ScopeKind.File: return GetDescriptionForFile(scope as IFileScope);
                case ScopeKind.Pointer: return GetDescriptionForPointer(scope as IPointerScope);
                case ScopeKind.Enum: return GetDescriptionForEnum(scope as IEnumScope);
                case ScopeKind.Set: return GetDescriptionForSet(scope as ISetScope);
                case ScopeKind.ElementScope: return GetDescriptionForElementScope(scope as IElementScope);
                case ScopeKind.CompiledField: return GetDescriptionForCompiledField(scope as ICompiledFieldScope);
                case ScopeKind.CompiledProperty: return GetDescriptionForCompiledProperty(scope as ICompiledPropertyScope);
                case ScopeKind.CompiledMethod: return GetDescriptionForCompiledMethod(scope as ICompiledMethodScope);
                case ScopeKind.Namespace: return GetDescriptionForNamespace(scope as INamespaceScope);
                case ScopeKind.Procedure: return GetDescriptionForProcedure(scope as IProcScope);
                case ScopeKind.CompiledEvent: return GetDescriptionForCompiledEvent(scope as ICompiledEventScope);
                case ScopeKind.CompiledConstructor: return GetDescriptionForCompiledConstructor(scope as ICompiledConstructorScope);
                case ScopeKind.ShortString: return GetDescriptionForShortString(scope as IShortStringScope);
            }
            return "";
        }

        public override string GetSimpleDescription(IBaseScope scope)
        {
            if (scope == null)
                return "";
            switch (scope.Kind)
            {
                case ScopeKind.Delegate:
                case ScopeKind.Array:
                case ScopeKind.Diapason:
                case ScopeKind.File:
                case ScopeKind.Pointer:
                case ScopeKind.Enum:
                case ScopeKind.Set:
                case ScopeKind.ShortString:
                    if (scope is ITypeScope && (scope as ITypeScope).Aliased)
                        return scope.Name;
                    break;
            }
            switch (scope.Kind)
            {
                case ScopeKind.Type: return GetSimpleDescriptionForType(scope as ITypeScope);
                case ScopeKind.CompiledType: return GetSimpleDescriptionForCompiledType(scope as ICompiledTypeScope);
                case ScopeKind.Delegate: return GetDescriptionForDelegate(scope as IProcType);
                case ScopeKind.TypeSynonim: return GetSimpleSynonimDescription(scope as ITypeSynonimScope);
                case ScopeKind.Array: return GetDescriptionForArray(scope as IArrayScope);
                case ScopeKind.Diapason: return GetDescriptionForDiapason(scope as IDiapasonScope);
                case ScopeKind.File: return GetDescriptionForFile(scope as IFileScope);
                case ScopeKind.Pointer: return GetDescriptionForPointer(scope as IPointerScope);
                case ScopeKind.Enum: return GetDescriptionForEnum(scope as IEnumScope);
                case ScopeKind.Set: return GetDescriptionForSet(scope as ISetScope);
                case ScopeKind.ElementScope: return GetSimpleDescriptionForElementScope(scope as IElementScope);
                case ScopeKind.CompiledField: return GetDescriptionForCompiledField(scope as ICompiledFieldScope);
                case ScopeKind.CompiledProperty: return GetDescriptionForCompiledProperty(scope as ICompiledPropertyScope);
                case ScopeKind.CompiledMethod: return GetDescriptionForCompiledMethod(scope as ICompiledMethodScope);
                case ScopeKind.Namespace: return GetDescriptionForNamespace(scope as INamespaceScope);
                case ScopeKind.Procedure: return GetSimpleDescriptionForProcedure(scope as IProcScope);
                case ScopeKind.CompiledEvent: return GetDescriptionForCompiledEvent(scope as ICompiledEventScope);
                case ScopeKind.CompiledConstructor: return GetDescriptionForCompiledConstructor(scope as ICompiledConstructorScope);
                case ScopeKind.ShortString: return GetDescriptionForShortString(scope as IShortStringScope);
                case ScopeKind.UnitInterface: return GetDescriptionForModule(scope as IInterfaceUnitScope);
                    //case ScopeKind.Procedure : return GetDescriptionForProcedure(scope as IProcScope);
            }
            return "";
        }

        private string GetDescriptionForModule(IInterfaceUnitScope scope)
        {
            return (scope.IsNamespaceUnit ? "namespace " : "unit ") + scope.Name;
        }


        public override string GetShortName(ICompiledConstructorScope scope)
        {
            return StringConstants.default_constructor_name;
        }

        public override string GetKeyword(SymbolKind kind)
        {
            switch (kind)
            {
                case SymbolKind.Class: return "class";
                case SymbolKind.Enum: return "enum";
                case SymbolKind.Struct: return "record";
                case SymbolKind.Type: return "type";
                case SymbolKind.Interface: return "interface";
                case SymbolKind.Null: return "nil";
            }
            return "";
        }

        public override string GetArrayDescription(string elementType, int rank)
        {
            if (rank == 1)
                return "array of " + elementType;
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append('[');
                sb.Append(',', rank - 1);
                sb.Append(']');
                return "array" + sb.ToString() + " of " + elementType;
            }
        }

        public string GetCompiledTypeTextRepresentation(Type t)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("type " + t.Name + " = ");
            if (t.IsEnum)
            {
                //sb.Append(GetEnum(t));
                sb.AppendLine(";");
                return sb.ToString();
            }
            else if (t.BaseType == typeof(MulticastDelegate))
            {
                //sb.Append(GetDelegate(t));
                sb.AppendLine(";");
                return sb.ToString();
            }
            else if (t.IsInterface)
                sb.Append("interface");
            else if (t.IsValueType)
                sb.Append("record");
            else if (t.IsClass)
                sb.Append("class");
            if (t.BaseType != null)
            {
                sb.Append('(');
                sb.Append(GetFullTypeName(t.BaseType));
            }
            Type[] intfs = t.GetInterfaces();
            if (intfs.Length > 0)
            {
                if (t.BaseType == null)
                    sb.Append('(');
                for (int i = 0; i < intfs.Length; i++)
                {
                    if (i != 0 || t.BaseType != null)
                        sb.Append(", ");
                    sb.Append(GetFullTypeName(intfs[i]));
                }
            }
            return sb.ToString();
        }

        public override string GetStandardTypeByKeyword(KeywordKind keyw)
        {
            switch (keyw)
            {
                case KeywordKind.ObjectType: return "object";
                case KeywordKind.StringType: return "string";
                case KeywordKind.ByteType: return "byte";
                case KeywordKind.SByteType: return "shortint";
                case KeywordKind.ShortType: return "smallint";
                case KeywordKind.UShortType: return "word";
                case KeywordKind.IntType: return "integer";
                case KeywordKind.UIntType: return "longword";
                case KeywordKind.Int64Type: return "int64";
                case KeywordKind.UInt64Type: return "uint64";
                case KeywordKind.DoubleType: return "real";
                case KeywordKind.FloatType: return "single";
                case KeywordKind.CharType: return "char";
                case KeywordKind.BoolType: return "boolean";
                case KeywordKind.PointerType: return "pointer";
            }
            return null;
        }

        protected override string GetFullTypeName(Type ctn, bool no_alias = true)
        {
            TypeCode tc = Type.GetTypeCode(ctn);
            if (ctn.FullName == null && !ctn.IsArray && !ctn.IsGenericTypeDefinition && ctn.IsGenericParameter)
                return ctn.Name;
            if (!ctn.IsEnum)
            {
                switch (tc)
                {
                    case TypeCode.Int32: return "integer";
                    case TypeCode.Double: return "real";
                    case TypeCode.Boolean: return "boolean";
                    case TypeCode.String: return "string";
                    case TypeCode.Char: return "char";
                    case TypeCode.Byte: return "byte";
                    case TypeCode.SByte: return "shortint";
                    case TypeCode.Int16: return "smallint";
                    case TypeCode.Int64: return "int64";
                    case TypeCode.UInt16: return "word";
                    case TypeCode.UInt32: return "longword";
                    case TypeCode.UInt64: return "uint64";
                    case TypeCode.Single: return "single";
                }
                if (ctn.IsPointer)
                    if (ctn.FullName == "System.Void*")
                        return "pointer";
                    else
                        return "^" + GetFullTypeName(ctn.GetElementType());
            }
            else
                return ctn.FullName;
            if (!no_alias)
            {
                if (ctn.Name.Contains("Func`") || ctn.Name.Contains("Predicate`"))
                    return getLambdaRepresentation(ctn, true, new List<string>());
                else if (ctn.Name.Contains("Action`"))
                    return getLambdaRepresentation(ctn, false, new List<string>());
                else if (ctn.Name == "IEnumerable`1")
                    return "sequence of " + GetShortTypeName(ctn.GetGenericArguments()[0], false);
                else if (ctn.Name.Contains("Tuple`"))
                    return get_tuple_string(ctn);
            }
            int gen_pos = ctn.Name.IndexOf("`");
            if (gen_pos != -1 || ctn.IsGenericType)
            {
                int len = ctn.GetGenericArguments().Length;
                Type[] gen_ps = ctn.GetGenericArguments();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (gen_pos != -1)
                    sb.Append(ctn.Namespace + "." + ctn.Name.Substring(0, gen_pos));
                else
                    sb.Append(ctn.Namespace + "." + ctn.Name);
                sb.Append(GenericTypesStartBracket);
                for (int i = 0; i < len; i++)
                {
                    sb.Append(gen_ps[i].Name);
                    if (i < len - 1)
                        sb.Append(", ");
                }
                sb.Append(GenericTypesEndBracket);
                return sb.ToString();
            }
            if (ctn.IsArray)
            {
                var rank = ctn.GetArrayRank();
                var strrank = rank > 1 ? "[" + new string(',', rank - 1) + "]" : "";
                return $"array{strrank}" + " of " + GetFullTypeName(ctn.GetElementType());
            }
            if (ctn == Type.GetType("System.Void*")) return "pointer";
            if (ctn.IsNested)
                return ctn.Name;
            return ctn.FullName;
        }

        private bool enum_out_of_order(FieldInfo[] fields) //возвращает true, если значения полей этого enum'а идут не по порядку или не с нуля (IDE issue #117)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].Name != "value__")
                {
                    object o = fields[i].GetRawConstantValue(); //ошибка была здесь, так как я не знал, что у констант enum'а может быть другой тип помимо int
                    if (o is byte b1)
                    {
                        if (b1 != i - 1) return true;
                    }
                    else if (o is sbyte b2)
                    {
                        if (b2 != i - 1) return true;
                    }
                    else if (o is short b3)
                    {
                        if (b3 != i - 1) return true;
                    }
                    else if (o is ushort b4)
                    {
                        if (b4 != i - 1) return true;
                    }
                    else if (o is int b5)
                    {
                        if (b5 != i - 1) return true;
                    }
                    else if (o is uint b6)
                    {
                        return true;
                    }
                    else if (o is long b7)
                    {
                        return true;
                    }
                    else if (o is ulong b8)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private string get_enum_constants(Type t)
        {
            FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            bool outoforder = enum_out_of_order(fields);
            bool is_flags = Attribute.IsDefined(t, typeof(FlagsAttribute));
            int max_name_len = fields.Max(fi => fi.Name.Length);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("(");
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].Name != "value__")
                {
                    sb.Append(' ', 4);
                    sb.Append(fields[i].Name);
                    if (outoforder)
                    {
                        if (is_flags) sb.Append(' ', max_name_len - fields[i].Name.Length);
                        sb.Append(" = ");
                        if (is_flags)
                            sb.Append('$' + Convert.ToInt64(fields[i].GetRawConstantValue()).ToString("X"));
                        else
                            sb.Append(fields[i].GetRawConstantValue());
                    }
                    if (i < fields.Length - 1)
                        sb.AppendLine(",");
                    else
                        sb.AppendLine();
                }
            }
            sb.Append(");");

            return sb.ToString();
        }

        public override string GetCompiledTypeRepresentation(Type t, MemberInfo mi, ref int line, ref int col)
        {
            StringBuilder sb = new StringBuilder();
            int ln = 1;
            if (t.Namespace == "System")
                sb.AppendLine("unit SystemUnit;");
            else
                sb.AppendLine("unit " + t.Namespace.Replace(".", "_") + ";");
            ln++;
            sb.AppendLine(); ln++;
            sb.AppendLine("type "); ln++;

            if (mi == t)
            {
                line = ln;
                col = 1;
            }
            string doc = CodeCompletionTools.AssemblyDocCache.GetDocumentation(t);
            if (!string.IsNullOrEmpty(doc))
            {
                doc = doc.Trim(' ', '\n', '\t', '\r').Replace(Environment.NewLine, Environment.NewLine + "  /// ");
                doc = doc.Replace("<returns>", PascalABCCompiler.StringResources.Get("CODE_COMPLETION_RETURN_VALUE"));
                doc = doc.Replace("<params>", "");
                doc = doc.Replace("<param>", PascalABCCompiler.StringResources.Get("CODE_COMPLETION_PARAMETER"));
                doc = doc.Replace("</param>", "");
                sb.AppendLine("  /// " + doc);
            }
            if (mi == t)
            {
                line = get_line_nr(sb);
                col = 1;
            }
            sb.Append("  ");
            int ind = t.Name.IndexOf('`');
            if (ind != -1 || t.IsGenericType)
            {
                if (ind != -1)
                    sb.Append(prepare_member_name(t.Name.Substring(0, ind)));
                else
                    sb.Append(t.Name);
                sb.Append(GenericTypesStartBracket);
                Type[] gen_args = t.GetGenericArguments();
                for (int i = 0; i < gen_args.Length; i++)
                {
                    sb.Append(gen_args[i].Name);
                    if (i < gen_args.Length - 1)
                        sb.Append(", ");
                }
                sb.Append(GenericTypesEndBracket);
            }
            else
                sb.Append(prepare_member_name(t.Name));
            sb.Append(" = " + (t.IsSealed && t.IsAbstract ? "static " : "") + GetClassKeyword(t));
            bool bracket = false;
            if (t.IsEnum)
            {
                sb.AppendLine(get_enum_constants(t));
                sb.AppendLine();
                sb.AppendLine("end.");
                return sb.ToString();
            }
            if (t.BaseType != null && t.BaseType != typeof(object))
            {
                sb.Append("(");
                bracket = true;
                sb.Append(GetFullTypeName(t.BaseType));
            }
            Type[] intfs = t.GetInterfaces();
            if (intfs.Length > 0)
                if (!bracket)
                {
                    sb.Append("(");
                    bracket = true;
                }
                else
                    sb.Append(", ");
            for (int i = 0; i < intfs.Length; i++)
            {
                sb.Append(GetFullTypeName(intfs[i]));
                if (i < intfs.Length - 1)
                    sb.Append(", ");
            }
            if (bracket)
                sb.Append(")");
            sb.AppendLine(); ln++;
            FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (fields.Length > 0)
            {
                sb.AppendLine("  {$region " + PascalABCCompiler.StringResources.Get("CODE_COMPLETION_FIELDS") + "}");
                for (int i = 0; i < fields.Length; i++)
                {
                    if (fields[i].DeclaringType == t && !fields[i].IsPrivate && !fields[i].IsAssembly)
                    {
                        doc = CodeCompletionTools.AssemblyDocCache.GetDocumentation(fields[i]);
                        if (!string.IsNullOrEmpty(doc))
                        {
                            doc = doc.Trim(' ', '\n', '\t', '\r').Replace(Environment.NewLine, Environment.NewLine + "    /// ");
                            doc = doc.Replace("<returns>", PascalABCCompiler.StringResources.Get("CODE_COMPLETION_RETURN_VALUE"));
                            doc = doc.Replace("<params>", "");
                            doc = doc.Replace("<param>", PascalABCCompiler.StringResources.Get("CODE_COMPLETION_PARAMETER"));
                            doc = doc.Replace("</param>", "");
                            sb.AppendLine("    /// " + doc);
                            ln++;
                        }
                        if (fields[i] == mi || fields[i].Name == mi.Name)
                        {
                            line = get_line_nr(sb);
                            col = 3;
                        }
                        sb.Append("    ");
                        sb.Append(GetDescriptionForCompiledField(fields[i]));
                        sb.AppendLine(); ln++;
                        sb.AppendLine(); ln++;
                    }
                }
                sb.AppendLine("  {$endregion}");
                sb.AppendLine();
            }
            EventInfo[] events = t.GetEvents(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (events.Length > 0)
            {
                sb.AppendLine("  {$region " + PascalABCCompiler.StringResources.Get("CODE_COMPLETION_EVENTS") + "}");
                for (int i = 0; i < events.Length; i++)
                {
                    if (events[i].DeclaringType != t)
                        continue;
                    MethodInfo add_meth = events[i].GetAddMethod(true);
                    if (add_meth != null && add_meth.DeclaringType == t && !add_meth.IsPrivate && !add_meth.IsAssembly)
                    {
                        doc = CodeCompletionTools.AssemblyDocCache.GetDocumentation(events[i]);
                        if (!string.IsNullOrEmpty(doc))
                        {
                            doc = doc.Trim(' ', '\n', '\t', '\r').Replace(Environment.NewLine, Environment.NewLine + "    /// ");
                            doc = doc.Replace("<returns>", PascalABCCompiler.StringResources.Get("CODE_COMPLETION_RETURN_VALUE"));
                            doc = doc.Replace("<params>", "");
                            doc = doc.Replace("<param>", PascalABCCompiler.StringResources.Get("CODE_COMPLETION_PARAMETER"));
                            doc = doc.Replace("</param>", "");
                            sb.AppendLine("    /// " + doc);
                        }

                        if (events[i] == mi || events[i].Name == mi.Name)
                        {
                            line = get_line_nr(sb);
                            col = 3;
                        }
                        sb.Append("    ");
                        sb.Append(GetDescriptionForCompiledEvent(events[i]));
                        sb.AppendLine(); ln++;
                        sb.AppendLine(); ln++;
                    }
                }
                sb.AppendLine("  {$endregion}");
                sb.AppendLine();
            }
            PropertyInfo[] props = t.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (props.Length > 0)
            {
                sb.AppendLine("  {$region " + PascalABCCompiler.StringResources.Get("CODE_COMPLETION_PROPERTIES") + "}");
                for (int i = 0; i < props.Length; i++)
                {
                    if (props[i].DeclaringType != t)
                        continue;
                    MethodInfo get_meth = props[i].GetGetMethod(true);
                    if (get_meth != null && get_meth.DeclaringType == t && !get_meth.IsPrivate && !get_meth.IsAssembly)
                    {
                        doc = CodeCompletionTools.AssemblyDocCache.GetDocumentation(props[i]);
                        if (!string.IsNullOrEmpty(doc))
                        {
                            doc = doc.Trim(' ', '\n', '\t', '\r').Replace(Environment.NewLine, Environment.NewLine + "    /// ");
                            doc = doc.Replace("<returns>", PascalABCCompiler.StringResources.Get("CODE_COMPLETION_RETURN_VALUE"));
                            doc = doc.Replace("<params>", "");
                            doc = doc.Replace("<param>", PascalABCCompiler.StringResources.Get("CODE_COMPLETION_PARAMETER"));
                            doc = doc.Replace("</param>", "");
                            sb.AppendLine("    /// " + doc);
                        }
                        if (props[i] == mi || props[i].Name == mi.Name)
                        {
                            line = get_line_nr(sb);
                            col = 3;
                        }
                        sb.Append("    ");
                        sb.Append(GetDescriptionForCompiledProperty(props[i]).Replace("; readonly", " read"));
                        sb.AppendLine(); ln++;
                        sb.AppendLine(); ln++;
                    }
                }
                sb.AppendLine("  {$endregion}");
                sb.AppendLine();
            }

            ConstructorInfo[] constrs = t.GetConstructors(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (constrs.Length > 0)
            {
                sb.AppendLine("  {$region " + PascalABCCompiler.StringResources.Get("CODE_COMPLETION_CONSTRUCTORS") + "}");
                for (int i = 0; i < constrs.Length; i++)
                {
                    if (constrs[i].DeclaringType == t && !constrs[i].IsPrivate && !constrs[i].IsAssembly)
                    {
                        doc = CodeCompletionTools.AssemblyDocCache.GetDocumentation(constrs[i]);
                        if (!string.IsNullOrEmpty(doc))
                        {
                            doc = doc.Trim(' ', '\n', '\t', '\r').Replace(Environment.NewLine, Environment.NewLine + "    /// ");
                            doc = doc.Replace("<returns>", PascalABCCompiler.StringResources.Get("CODE_COMPLETION_RETURN_VALUE"));
                            doc = doc.Replace("<params>", "");
                            doc = doc.Replace("<param>", PascalABCCompiler.StringResources.Get("CODE_COMPLETION_PARAMETER"));
                            doc = doc.Replace("</param>", "");
                            sb.AppendLine("    /// " + doc);
                        }
                        if (constrs[i] == mi)
                        {
                            line = get_line_nr(sb);
                            col = 3;
                        }
                        sb.Append("    ");
                        sb.Append(GetDescriptionForCompiledConstructor(constrs[i]));
                        sb.AppendLine(); ln++;
                        sb.AppendLine(); ln++;
                    }
                }
                sb.AppendLine("  {$endregion}");
                sb.AppendLine();
            }
            MethodInfo[] meths = t.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (meths.Length > 0)
            {
                sb.AppendLine("  {$region " + PascalABCCompiler.StringResources.Get("CODE_COMPLETION_METHODS") + "}");
                for (int i = 0; i < meths.Length; i++)
                {
                    if (meths[i].DeclaringType == t && !meths[i].IsPrivate && !meths[i].IsAssembly && !meths[i].Name.StartsWith("get_") && !meths[i].Name.StartsWith("set_") && !meths[i].Name.StartsWith("add_") && !meths[i].Name.StartsWith("remove_"))
                    {
                        doc = CodeCompletionTools.AssemblyDocCache.GetDocumentation(meths[i]);
                        if (!string.IsNullOrEmpty(doc))
                        {
                            doc = doc.Trim(' ', '\n', '\t', '\r').Replace(Environment.NewLine, Environment.NewLine + "    /// ");
                            doc = doc.Replace("<returns>", PascalABCCompiler.StringResources.Get("CODE_COMPLETION_RETURN_VALUE"));
                            doc = doc.Replace("<params>", "");
                            doc = doc.Replace("<param>", PascalABCCompiler.StringResources.Get("CODE_COMPLETION_PARAMETER"));
                            doc = doc.Replace("</param>", "");
                            sb.AppendLine("    /// " + doc);
                        }
                        if (meths[i] == mi || (meths[i].Name == mi.Name))
                        {
                            line = get_line_nr(sb);
                            col = 3;
                        }
                        sb.Append("    ");
                        sb.Append(GetDescriptionForCompiledMethod(meths[i]));
                        sb.AppendLine(); ln++;
                        sb.AppendLine(); ln++;
                    }
                }
                sb.AppendLine("  {$endregion}");
                sb.AppendLine();
            }

            sb.AppendLine("  end;");
            sb.AppendLine();
            sb.AppendLine("end.");
            return sb.ToString();
        }

        private int get_line_nr(StringBuilder sb)
        {
            int line = 1;
            for (int i = 0; i < sb.Length; i++)
                if (sb[i] == '\n')
                    line++;
            return line;
        }

        public string GetCompiledMethodRepresentation(MethodInfo mi)
        {
            return null;
        }

        public string GetCompiledFieldRepresentation(FieldInfo mi)
        {
            return null;
        }

        public string GetCompiledPropertyRepresentation(PropertyInfo mi)
        {
            return null;
        }

        public string GetCompiledEventRepresentation(EventInfo mi)
        {
            return null;
        }

        public string GetCompiledConstructorRepresentation(ConstructorInfo mi)
        {
            return null;
        }

        public string GetClassKeyword(Type t)
        {
            if (t.IsInterface)
                return "interface";
            if (t.IsEnum)
                return "";
            if (t.IsValueType)
                return "record";
            if (t.IsClass)
                return "class";
            return "";
        }

        public override string GetClassKeyword(PascalABCCompiler.SyntaxTree.class_keyword keyw)
        {
            switch (keyw)
            {
                case PascalABCCompiler.SyntaxTree.class_keyword.Class: return "class";
                case PascalABCCompiler.SyntaxTree.class_keyword.Interface: return "interface";
                case PascalABCCompiler.SyntaxTree.class_keyword.Record: return "record";
                case PascalABCCompiler.SyntaxTree.class_keyword.TemplateClass: return "template class";
                case PascalABCCompiler.SyntaxTree.class_keyword.TemplateRecord: return "template record";
                case PascalABCCompiler.SyntaxTree.class_keyword.TemplateInterface: return "template interface";
            }
            return null;
        }

        public override string GetShortTypeName(Type ctn, bool noalias = true)
        {
            TypeCode tc = Type.GetTypeCode(ctn);
            if (ctn.FullName == null && !ctn.IsArray && !ctn.IsGenericTypeDefinition && ctn.IsGenericParameter)
                return ctn.Name;
            if (!ctn.IsEnum)
            {
                switch (tc)
                {
                    case TypeCode.Int32: return "integer";
                    case TypeCode.Double: return "real";
                    case TypeCode.Boolean: return "boolean";
                    case TypeCode.String: return "string";
                    case TypeCode.Char: return "char";
                    case TypeCode.Byte: return "byte";
                    case TypeCode.SByte: return "shortint";
                    case TypeCode.Int16: return "smallint";
                    case TypeCode.Int64: return "int64";
                    case TypeCode.UInt16: return "word";
                    case TypeCode.UInt32: return "longword";
                    case TypeCode.UInt64: return "uint64";
                    case TypeCode.Single: return "single";
                }
                if (ctn.IsPointer)
                    if (ctn.FullName == "System.Void*")
                        return "pointer";
                    else
                        return "^" + GetShortTypeName(ctn.GetElementType(), noalias);
            }
            else return ctn.Name;
            if (ctn.Name.Contains("`"))
            {
                if (!noalias)
                {
                    if (ctn.Name.Contains("Func`") || ctn.Name.Contains("Predicate`"))
                        return getLambdaRepresentation(ctn, true, new List<string>());
                    else if (ctn.Name.Contains("Action`"))
                        return getLambdaRepresentation(ctn, false, new List<string>());
                    else if (ctn.Name == "IEnumerable`1")
                        return "sequence of " + GetShortTypeName(ctn.GetGenericArguments()[0], false);
                    else if (ctn.Name.Contains("Tuple`"))
                        return get_tuple_string(ctn);
                }
                int len = ctn.GetGenericArguments().Length;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(ctn.Name.Substring(0, ctn.Name.IndexOf('`')));
                sb.Append(GenericTypesStartBracket);
                if (!noalias)
                {
                    Type[] gen_ps = ctn.GetGenericArguments();
                    for (int i = 0; i < len; i++)
                    {
                        sb.Append(gen_ps[i].Name);
                        if (i < len - 1)
                            sb.Append(", ");
                    }
                }
                sb.Append(GenericTypesEndBracket);
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
            if (ctn.IsArray)
            {
                var rank = ctn.GetArrayRank();
                var strrank = rank > 1 ? "[" + new string(',', rank - 1) + "]" : "";
                return $"array{strrank}" + " of " + GetShortTypeName(ctn.GetElementType());
            }
            //if (ctn == Type.GetType("System.Void*")) return PascalABCCompiler.StringConstants.pointer_type_name;
            return ctn.Name;
        }

        protected string GetDescriptionForType(ITypeScope scope)
        {
            string template_str = GetTemplateString(scope);
            switch (scope.ElemKind)
            {
                case SymbolKind.Class:
                    string mod = "";
                    if (scope.IsStatic)
                        mod = "static ";
                    else
                    {
                        if (scope.IsAbstract)
                            mod = "abstract ";
                        if (scope.IsFinal)
                            mod += "sealed ";
                    }
                    if (scope.TopScope != null && scope.TopScope.Name != "" && !scope.TopScope.Name.Contains("$"))
                        return mod + "class " + scope.TopScope.Name + "." + scope.Name + template_str;
                    else return mod + "class " + scope.Name + template_str;
                case SymbolKind.Interface:
                    if (scope.TopScope != null && scope.TopScope.Name != "" && !scope.TopScope.Name.Contains("$"))
                        return "interface " + scope.TopScope.Name + "." + scope.Name + template_str;
                    else return "interface " + scope.Name + template_str;
                case SymbolKind.Enum:
                    if (scope.TopScope != null && scope.TopScope.Name != "" && !scope.TopScope.Name.Contains("$"))
                        return "enum " + scope.TopScope.Name + "." + scope.Name;
                    else return "enum " + scope.Name;
                case SymbolKind.Delegate:
                    if (scope.TopScope != null && scope.TopScope.Name != "" && !scope.TopScope.Name.Contains("$"))
                        return "delegate " + scope.TopScope.Name + "." + scope.Name + template_str;
                    else return "delegate " + scope.Name + template_str;
                case SymbolKind.Struct:
                    if (scope.TopScope != null && scope.TopScope.Name != "" && !scope.TopScope.Name.Contains("$"))
                        return "record " + scope.TopScope.Name + "." + scope.Name + template_str;
                    else return "record " + scope.Name + template_str;
            }

            if (scope.TopScope != null)
                return scope.TopScope.Name + "." + scope.Name;
            else return scope.Name;
        }

        protected string GetDescriptionForCompiledType(ICompiledTypeScope scope)
        {
            string s = GetFullTypeName(scope.CompiledType);
            ITypeScope[] instances = scope.GenericInstances;
            if (instances.Length > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                int ind = s.IndexOf(GenericTypesStartBracket);
                if (ind != -1) sb.Append(s.Substring(0, ind));
                else
                    sb.Append(s);
                sb.Append(GenericTypesStartBracket);
                for (int i = 0; i < instances.Length; i++)
                {
                    sb.Append(GetSimpleDescriptionWithoutNamespace(instances[i]));
                    //sb.Append(instances[i].Name);
                    if (i < instances.Length - 1) sb.Append(", ");
                }
                sb.Append(GenericTypesEndBracket);
                s = sb.ToString();
            }

            switch (scope.ElemKind)
            {
                case SymbolKind.Class:
                    string mod = "";
                    if (scope.IsStatic)
                        mod = "static ";
                    else
                    {
                        if (scope.IsAbstract)
                            mod = "abstract ";
                        if (scope.IsFinal)
                            mod += "sealed ";
                    }
                    return mod + "class " + s;
                case SymbolKind.Interface:
                    return "interface " + s;
                case SymbolKind.Enum:
                    return "enum " + s;
                case SymbolKind.Delegate:
                    return "delegate " + s;
                case SymbolKind.Struct:
                    return "record " + s;
                case SymbolKind.Type:
                    return "type " + s;
            }
            return s;
        }

        private string GetDescriptionForDiapason(IDiapasonScope scope)
        {
            return scope.Left.ToString() + ".." + scope.Right.ToString();
        }

        private string GetDescriptionForFile(IFileScope scope)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("file");
            if (scope.ElementType != null)
            {
                string s = GetSimpleDescription(scope.ElementType);
                if (s.Length > 0 && s[0] == '$') s = s.Substring(1, s.Length - 1);
                sb.Append(" of " + s);
            }
            return sb.ToString();
        }

        private string GetDescriptionForPointer(IPointerScope scope)
        {
            string s = "";
            if (scope.ElementType != null)
            {
                s = "^" + GetSimpleDescription(scope.ElementType);
            }
            else
                s = "pointer";

            return s;
        }

        private string GetDescriptionForSet(ISetScope scope)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("set of ");
            if (scope.ElementType != null)
            {
                string s = GetSimpleDescription(scope.ElementType);
                if (s.Length > 0 && s[0] == '$') s = s.Substring(1, s.Length - 1);
                sb.Append(s);
            }
            return sb.ToString();
        }

        public string GetDescriptionForCompiledField(FieldInfo fi)
        {
            StringBuilder sb = new StringBuilder();
            if (fi.IsPublic)
                sb.Append("public ");
            else if (fi.IsFamily)
                sb.Append("protected ");
            if (!fi.IsLiteral)
                if (fi.IsStatic) sb.Append("static ");
            if (!fi.IsLiteral)
            {
                sb.Append(prepare_member_name(fi.Name));
                sb.Append(" : " + GetFullTypeName(fi.FieldType));
            }
            else
            {
                var fitype = GetFullTypeName(fi.FieldType);
                sb.Append("const " + fi.Name + " : " + fitype);
                sb.Append(" = " + (fitype == "string" ? $"'{fi.GetRawConstantValue().ToString()}'" : fi.GetRawConstantValue().ToString()));
            }
            sb.Append(";");
            return sb.ToString();
        }

        public string GetDescriptionForCompiledProperty(PropertyInfo pi)
        {
            StringBuilder sb = new StringBuilder();
            MethodInfo get_meth = pi.GetGetMethod(true);
            MethodInfo set_meth = pi.GetSetMethod(true);
            if (get_meth.IsPublic)
                sb.Append("public ");
            else if (get_meth.IsFamily)
                sb.Append("protected ");
            if (get_meth.IsStatic) sb.Append("static ");
            sb.Append("property " + prepare_member_name(pi.Name));
            ParameterInfo[] prms = get_meth.GetParameters();
            if (prms.Length > 0)
            {
                sb.Append('[');
                for (int i = 0; i < prms.Length; i++)
                {
                    sb.Append(prepare_member_name(prms[i].Name));
                    sb.Append(": ");
                    sb.Append(GetFullTypeName(prms[i].ParameterType));
                    if (i < prms.Length - 1)
                        sb.Append("; ");
                }
                sb.Append(']');
            }
            sb.Append(" : " + GetFullTypeName(pi.PropertyType));
            if (set_meth == null)
                sb.Append("; readonly");
            sb.Append(";");
            return sb.ToString();
        }

        private string GetDescriptionForNamespace(INamespaceScope scope)
        {
            return "namespace " + scope.Name;
        }

        protected string GetDescriptionForCompiledEvent(EventInfo ei)
        {
            MethodInfo add_meth = ei.GetAddMethod(true);
            StringBuilder sb = new StringBuilder();
            if (add_meth.IsPublic)
                sb.Append("public ");
            else if (add_meth.IsFamily)
                sb.Append("protected ");
            sb.Append((add_meth.IsStatic ? "static " : "") + "event " + prepare_member_name(ei.Name) + ": " + GetFullTypeName(ei.EventHandlerType) + ";");
            return sb.ToString();
        }

        protected string GetDescriptionForCompiledConstructor(ConstructorInfo ci)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (ci.IsPublic)
                sb.Append("public ");
            else if (ci.IsFamily)
                sb.Append("protected ");
            if (ci.IsStatic)
                sb.Append("static ");
            sb.Append("constructor ");
            //sb.Append(".");
            //sb.Append("Create");
            sb.Append('(');
            ParameterInfo[] pis = ci.GetParameters();
            for (int i = 0; i < pis.Length; i++)
            {
                if (pis[i].ParameterType.IsByRef)
                    sb.Append("var ");
                else if (IsParams(pis[i]))
                    sb.Append("params ");
                sb.Append(prepare_member_name(pis[i].Name));
                sb.Append(": ");
                if (!pis[i].ParameterType.IsByRef)
                    sb.Append(GetFullTypeName(pis[i].ParameterType));
                else sb.Append(GetFullTypeName(pis[i].ParameterType.GetElementType()));
                if (i < pis.Length - 1)
                    sb.Append("; ");
            }
            sb.Append(')');
            sb.Append(';');
            return sb.ToString();
        }

        public override string GetSynonimDescription(ITypeScope scope)
        {
            return "type " + scope.Name + GetGenericString(scope.TemplateArguments) + " = " + scope.Description;
        }

        public override string GetSynonimDescription(ITypeSynonimScope scope)
        {
            if (scope.ActType is ICompiledTypeScope && !(scope.ActType as ICompiledTypeScope).Aliased)
                return "type " + scope.Name + GetGenericString(scope.TemplateArguments) + " = " + GetSimpleDescriptionForCompiledType(scope.ActType as ICompiledTypeScope, true);
            else
                return "type " + scope.Name + GetGenericString(scope.TemplateArguments) + " = " + GetSimpleDescription(scope.ActType);
        }

        public override string GetSynonimDescription(IProcScope scope)
        {
            return "type " + scope.Name + " = " + scope.Description;
        }

        private string GetSimpleSynonimDescription(ITypeSynonimScope scope)
        {
            return scope.Name;
        }

        public override string GetStringForChar(char c)
        {
            return "'" + c.ToString() + "'";
        }

        public override string GetStringForSharpChar(int num)
        {
            return "#" + num.ToString();
        }

        public override string GetStringForString(string s)
        {
            return "'" + s + "'";
        }


        protected string GetAccessModifier(access_modifer mod)
        {
            switch (mod)
            {
                case access_modifer.private_modifer: return "private";
                case access_modifer.protected_modifer: return "protected";
                case access_modifer.public_modifer: return "public";
                case access_modifer.published_modifer: return "published";
                case access_modifer.internal_modifer: return "internal";
            }
            return "";
        }

        public override string ConstructOverridedMethodHeader(IProcScope scope, out int off)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (scope.AccessModifier != access_modifer.internal_modifer)
                sb.Append(GetAccessModifier(scope.AccessModifier) + " ");
            if (scope.ReturnType == null)
                sb.Append("procedure ");
            else
                sb.Append("function ");
            off = sb.Length;
            sb.Append(scope.Name);
            sb.Append(GetGenericString(scope.TemplateParameters));
            if (!(scope is ICompiledMethodScope))
            {
                IElementScope[] parameters = scope.Parameters;
                if (parameters != null && parameters.Length > 0)
                {
                    sb.Append('(');
                    for (int i = 0; i < scope.Parameters.Length; i++)
                    {
                        sb.Append(GetSimpleDescription(parameters[i]));
                        if (i < parameters.Length - 1)
                        {
                            sb.Append("; ");
                        }
                    }
                    sb.Append(')');
                }
            }
            else
            {
                ParameterInfo[] pis = (scope as ICompiledMethodScope).CompiledMethod.GetParameters();
                if (pis.Length > 0)
                {
                    sb.Append('(');
                    for (int i = 0; i < pis.Length; i++)
                    {
                        if (pis[i].ParameterType.IsByRef)
                            sb.Append("var ");
                        else if (IsParams(pis[i]))
                            sb.Append("params ");
                        sb.Append(pis[i].Name);
                        sb.Append(": ");
                        if (!pis[i].ParameterType.IsByRef)
                            sb.Append(GetFullTypeName(pis[i].ParameterType));
                        else sb.Append(GetFullTypeName(pis[i].ParameterType.GetElementType()));
                        if (i < pis.Length - 1)
                            sb.Append("; ");
                    }
                    sb.Append(')');
                }
            }
            if (scope.ReturnType != null)
                sb.Append(ReturnTypeDelimiter + " " + GetSimpleDescription(scope.ReturnType));
            sb.Append("; override;");
            return sb.ToString();
        }

        public override string ConstructHeader(IProcRealizationScope scope, int tabCount)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append('\t');
            if (!scope.DefProc.IsAbstract)
                sb.Append(GetAccessModifier(access_modifer.public_modifer) + " ");
            else
            if (scope.DefProc.AccessModifier != access_modifer.none && scope.DefProc.AccessModifier != access_modifer.internal_modifer)
                sb.Append(GetAccessModifier(scope.DefProc.AccessModifier) + " ");
            if (scope.DefProc.ReturnType == null)
                sb.Append("procedure ");
            else
                sb.Append("function ");
            sb.Append(scope.Name);
            sb.Append(GetGenericString(scope.TemplateParameters));

            if (!(scope.DefProc is ICompiledMethodScope))
            {
                IElementScope[] parameters = scope.DefProc.Parameters;
                if (parameters != null && parameters.Length > 0)
                {
                    sb.Append('(');
                    for (int i = 0; i < scope.DefProc.Parameters.Length; i++)
                    {
                        sb.Append(GetSimpleDescription(parameters[i]));
                        if (i < parameters.Length - 1)
                        {
                            sb.Append("; ");
                        }
                    }
                    sb.Append(')');
                }
            }
            else
            {
                ParameterInfo[] pis = (scope.DefProc as ICompiledMethodScope).CompiledMethod.GetParameters();
                if (pis.Length > 0)
                {
                    sb.Append('(');
                    for (int i = 0; i < pis.Length; i++)
                    {
                        if (pis[i].ParameterType.IsByRef)
                            sb.Append("var ");
                        else if (IsParams(pis[i]))
                            sb.Append("params ");
                        sb.Append(pis[i].Name);
                        sb.Append(": ");
                        if (!pis[i].ParameterType.IsByRef)
                            sb.Append(GetFullTypeName(pis[i].ParameterType));
                        else sb.Append(GetFullTypeName(pis[i].ParameterType.GetElementType()));
                        if (i < pis.Length - 1)
                            sb.Append("; ");
                    }
                    sb.Append(')');
                }
            }

            if (scope.DefProc.ReturnType != null)
                sb.Append(ReturnTypeDelimiter + " " + GetSimpleDescription(scope.DefProc.ReturnType));
            sb.Append(';');
            if (scope.DefProc.IsAbstract)
                sb.Append("override;");
            return sb.ToString();
        }

        private void get_procedure_template(procedure_header header, StringBuilder res, int col)
        {
            if (header.parameters != null)
                for (int i = 0; i < header.parameters.params_list.Count; i++)
                    for (int j = 0; j < header.parameters.params_list[i].idents.idents.Count; j++)
                    {
                        res.AppendLine();
                        for (int k = 0; k < col - 3; k++)
                            res.Append(' ');
                        res.Append("/// <param name=\"" + header.parameters.params_list[i].idents.idents[j].name + "\"></param>");
                    }
            if (header is function_header)
            {
                res.AppendLine();
                for (int k = 0; k < col - 3; k++)
                    res.Append(' ');
                res.Append("/// <returns></returns>");
            }
        }

        public override string GetDocumentTemplate(string lineText, string Text, int line, int col, int pos)
        {
            throw new NotImplementedException();
        }

        public override string ConstructHeader(string meth, IProcScope scope, int tabCount)
        {
            int i = 0;
            bool is_cnstr = false;
            StringBuilder sb = new StringBuilder();
            if (meth.StartsWith("static "))
                meth = meth.Remove(0, "static ".Length);
            else if (meth.StartsWith("class "))
                meth = meth.Remove(0, "class ".Length);
            if (scope.IsStatic)
                sb.Append("static ");
            while (i < meth.Length && char.IsLetterOrDigit(meth[i]))
            {
                sb.Append(meth[i++]);
            }
            if (sb.ToString().ToLower() == "constructor") is_cnstr = true;
            sb.Append(' ');
            while (i < meth.Length && !(char.IsLetterOrDigit(meth[i]) || meth[i] == '_' || meth[i] == '(' || meth[i] == ';'))
                if (meth[i] == '{')
                    while (i < meth.Length && meth[i] != '}') i++;
                else
                    i++;
            if (i < meth.Length)
            {
                if (scope.TopScope is ITypeScope && ((scope.TopScope as ITypeScope).ElemKind == SymbolKind.Class || (scope.TopScope as ITypeScope).ElemKind == SymbolKind.Struct))
                    sb.Append(GetSimpleDescriptionForType(scope.TopScope as ITypeScope) + ".");
                //if (meth[i] == '(' || meth[i] == ';')
                sb.Append(scope.Name);
                sb.Append(GetGenericString(scope.TemplateParameters));

                while (i < meth.Length && (char.IsLetterOrDigit(meth[i]) || meth[i] == '_')) i++;
                while (i < meth.Length && meth[i] != ';' && meth[i] != '(' && meth[i] != ':')
                    if (meth[i] == '{') while (i < meth.Length && meth[i] != '}') i++;
                    else i++;
                if (meth[i] == '(')
                {
                    sb.Append('(');
                    bool in_kav = false;
                    Stack<char> sk_stack = new Stack<char>();
                    sk_stack.Push('('); i++;
                    bool default_value = false;
                    while (i < meth.Length && sk_stack.Count > 0)
                    {
                        if (meth[i] == '\'') in_kav = !in_kav;
                        else if (meth[i] == '(') { if (!in_kav) sk_stack.Push('('); }
                        else if (meth[i] == ')') { if (!in_kav) sk_stack.Pop(); }
                        if (meth[i] == ':' && meth[i + 1] == '=' && !in_kav)
                            default_value = true;
                        else if (meth[i] == ';' && !in_kav)
                            default_value = false;
                        if (!default_value || meth[i] == ')' && sk_stack.Count == 0)
                        {
                            if (meth[i] == ')' && sk_stack.Count == 0 && default_value)
                                sb = new StringBuilder(sb.ToString().TrimEnd());
                            sb.Append(meth[i]);
                        }
                        i++;
                    }
                    while (i < meth.Length && meth[i] != ':' && meth[i] != ';')
                        if (meth[i] == '{') while (i < meth.Length && meth[i] != '}') i++;
                        else sb.Append(meth[i++]);

                    //sb.Append(')');
                }
                if (meth[i] == ':')
                {
                    bool in_kav = false;
                    while (i < meth.Length && !(meth[i] == ';' && !in_kav))
                    {
                        if (meth[i] == '{' && !in_kav) while (i < meth.Length && meth[i] != '}') i++;
                        else if (meth[i] == '\'') in_kav = !in_kav;
                        sb.Append(meth[i]);
                        i++;
                    }
                }
                sb.Append(';');
            }
            sb.AppendLine();
            sb.AppendLine("begin");
            for (int j = 0; j < tabCount; j++)
                sb.Append(' ');
            sb.AppendLine();
            sb.AppendLine("end;");
            return sb.ToString();
        }

        private bool isOperator(string Text, int i, out int next)
        {
            next = i;
            if (i >= 3)
            {
                string op = Text.Substring(i - 2, 3).ToLower().Trim();
                if (op == "and" || op == "div" || op == "mod" || op == "xor")
                {
                    if (!char.IsLetterOrDigit(Text[i - 3]) && Text[i - 3] != '_' && Text[i - 3] != '&' && !(i + 1 < Text.Length && char.IsLetterOrDigit(Text[i + 1])))
                    {
                        next = i - 3;
                        return true;
                    }
                    return false;
                }
            }
            if (i >= 2)
            {
                string op = Text.Substring(i - 1, 2).ToLower().Trim();
                if (op == "or")
                {
                    if (!char.IsLetterOrDigit(Text[i - 2]) && Text[i - 2] != '_' && Text[i - 2] != '&' && !(i + 1 < Text.Length && char.IsLetterOrDigit(Text[i + 1])))
                    {
                        next = i - 2;
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        private bool CheckForComment(string Text, int off, out int comment_position, out bool one_line_comment)
        {
            int i = off;
            one_line_comment = false;
            comment_position = -1;
            Stack<char> kav = new Stack<char>();
            bool is_comm = false;
            while (i >= 0 && !is_comm && Text[i] != '\n' && Text[i] != '\r')
            {
                if (Text[i] == '\'')
                {
                    if (kav.Count == 0) kav.Push('\'');
                    else kav.Pop();
                }
                else if (Text[i] == '{')
                {
                    if (kav.Count == 0)
                    {
                        comment_position = i;
                        while (i >= 0 && Text[i] != '\'')
                            i--;
                        if (i >= 1 && Text[i - 1] == '$')
                            return false;
                        is_comm = true;
                        return is_comm;
                    }
                }
                else if (Text[i] == '}')
                {
                    return false;
                }
                else if (Text[i] == '/')
                    if (i > 0 && Text[i - 1] == '/' && kav.Count == 0)
                    {
                        is_comm = true;
                        one_line_comment = true;
                        comment_position = i - 1;
                    }

                i--;
            }
            return is_comm;
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
                if (kav.Count == 0 && (char.IsLetterOrDigit(ch) || ch == '_' || ch == '&' || ch == '\'' || ch == '!'))
                {
                    num_in_ident = i;
                    if (kav.Count == 0 && tokens.Count == 0)
                    {
                        int tmp = i;
                        if (ch == '\'')
                        {
                            i--;
                            if (kav.Count == 0)
                                kav.Push('\'');
                            while (i >= 0)
                            {
                                if (Text[i] != '\'')
                                    i--;
                                else
                                {
                                    if (i >= 1 && Text[i - 1] == '\'')
                                        i -= 2;
                                    else
                                        break;
                                }
                            }

                            if (i >= 0)
                                i--;
                        }
                        else
                            while (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&' || Text[i] == '!'))
                            {
                                i--;
                            }
                        while (i >= 0 && (Text[i] == ' ' || char.IsControl(Text[i]) || Text[i] == '}'))
                        {
                            if (Text[i] != '}')
                                i--;
                            else
                            {
                                while (i >= 0 && Text[i] != '{') //propusk kommentariev
                                    i--;
                                if (i >= 0)
                                    i--;
                            }
                        }
                        if (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&' || Text[i] == '!'))
                        {
                            bound = i + 1;
                            TestForKeyword(Text, i, ref bound, punkt_sym, out keyw);
                            for (int j = tmp; j > bound; j--)
                            {
                                sb.Insert(0, Text[j]);
                            }
                            if (sb.ToString().Trim() == "new")
                                return "";
                            i = bound;
                            continue;
                        }
                        else if (i >= 0 && Text[i] == '\'') return "";
                        i = tmp;
                    }
                    else
                        if (ch == '\'')
                        kav.Push('\'');
                    sb.Insert(0, ch);//.Append(Text[i]);
                }
                else if (ch == '.' || ch == '^' || ch == '&' || ch == '?' && IsPunctuation(Text, i + 1))
                {
                    if (ch == '.' && i >= 1 && Text[i - 1] == '.' && tokens.Count == 0)
                        end = true;
                    else if (ch == '?' && i + 1 < Text.Length && Text[i + 1] != '.')
                        end = true;
                    else
                        sb.Insert(0, ch);
                    if (ch != '.')
                        punkt_sym = true;
                }
                else if (ch == '}')
                {
                    if (kav.Count == 0)
                    {
                        while (i >= 0 && Text[i] != '{')
                        {
                            if (Text[i] != '$')//skip {$
                                sb.Insert(0, Text[i]);
                            i--;
                        }
                        if (i < 0)
                        {
                            break;
                        }
                        else if (Text[i] == '{')
                        {
                            sb.Insert(0, '{');
                        }
                    }
                    else
                        sb.Insert(0, ch);
                }
                else if (ch == '{')
                {
                    if (kav.Count == 0)
                    {
                        if (keyw == KeywordKind.None)
                            return sb.ToString();
                        sb.Insert(0, ch);
                        break;
                    }
                    else sb.Insert(0, ch);
                }
                else
                    switch (ch)
                    {
                        case ')':
                        case ']':
                            if (kav.Count == 0)
                            {
                                string tmps = sb.ToString().Trim(' ', '\r', '\t', '\n');
                                if (tmps.Length >= 1 && (char.IsLetter(tmps[0]) || tmps[0] == '_' || tmps[0] == '&' || tmps[0] == '?' || tmps[0] == '!') && tokens.Count == 0)
                                    end = true;
                                else
                                    tokens.Push(ch);
                            }
                            if (!end)
                            {
                                sb.Insert(0, ch);
                                punkt_sym = true;
                            }
                            break;
                        case '>':
                            if (tokens.Count == 0)
                            {
                                int j = i + 1;

                                while (j < Text.Length && char.IsWhiteSpace(Text[j]))
                                    j++;

                                if (ugl_skobki.Count > 0 || i == off - 1 || j == off && off == Text.Length || j < Text.Length && Text[i - 1] != '-' && (Text[j] == '.' || Text[j] == '('))
                                {
                                    ugl_skobki.Push('>');
                                    sb.Insert(0, ch);
                                }
                                else if (i >= 1 && Text[i - 1] == '-')
                                {
                                    if (!(kav.Count == 0 && tokens.Count == 0))
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
                        case '[':
                        case '(':
                        case '|':
                            if (ch == '|' && ((tokens.Count == 0) || (tokens.Peek() == ']') || (tokens.Peek() == ')') || (tokens.Peek() == ',')))
                            // Закрывающий | - после него (tokens.Pop()) - пусто или ] ) ,
                            {
                                if (kav.Count == 0)
                                {
                                    string tmps = sb.ToString().Trim(' ', '\r', '\t', '\n');
                                    if (tmps.Length >= 1 && (char.IsLetter(tmps[0]) || tmps[0] == '_' || tmps[0] == '&' || tmps[0] == '?' || tmps[0] == '!') && tokens.Count == 0)
                                        end = true;
                                    else
                                        tokens.Push(ch);
                                }
                                if (!end)
                                {
                                    sb.Insert(0, ch);
                                    punkt_sym = true;
                                }
                            }
                            else
                            {
                                if (kav.Count == 0) // в т.ч. открывающий |
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
                                            while (i >= 0 && (Text[i] == ' ' || char.IsControl(Text[i]) || Text[i] == '}'))
                                            {
                                                if (Text[i] != '}')
                                                    i--;
                                                else
                                                {
                                                    while (i >= 0 && Text[i] != '{') //propusk kommentariev
                                                        i--;
                                                    if (i >= 0)
                                                        i--;
                                                }
                                            }
                                            if (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&' || Text[i] == '!' || Text[i] == '?' && IsPunctuation(Text, i + 1)))
                                            {
                                                bound = i + 1;
                                                TestForKeyword(Text, i, ref bound, punkt_sym, out keyw);
                                                if (keyw != KeywordKind.None && tokens.Count == 0)
                                                {
                                                    end = true;
                                                }
                                                else bound = 0;
                                            }
                                            else if (i >= 0 && Text[i] == '\'') return "";
                                            i = tmp;
                                        }
                                    }
                                    else
                                    {
                                        end = true;
                                        if (ch == '[')
                                        {
                                            keyw = KeywordKind.SquareBracket;
                                        }
                                    }

                                }
                                else sb.Insert(0, ch); punkt_sym = true;
                            }
                            break;
                        case '\'':
                            if (kav.Count == 0) kav.Push(ch); else kav.Pop();
                            sb.Insert(0, ch);
                            punkt_sym = true; break;
                        default:
                            if (!(ch == ' ' || char.IsControl(ch)))
                            {
                                if (kav.Count == 0)
                                {
                                    if (ch == ',' && ugl_skobki.Count > 0)
                                        sb.Insert(0, ch);
                                    else
                                    if (tokens.Count == 0)
                                        end = true;
                                    else
                                        sb.Insert(0, ch);
                                }
                                else
                                    sb.Insert(0, ch);
                            }
                            else
                            {
                                if (Text[i] == '\n')
                                {
                                    bool one_line_comment = false;
                                    int comment_position = -1;
                                    if (CheckForComment(Text, i - 1, out comment_position, out one_line_comment))
                                    {
                                        if (!one_line_comment)
                                            end = true;
                                        else
                                        {
                                            sb.Insert(0, ch);
                                            i = comment_position;
                                        }

                                    }
                                    else
                                        sb.Insert(0, ch);
                                }
                                else
                                    sb.Insert(0, ch);
                            }
                            punkt_sym = true;
                            break;
                    }

                if (end)
                {
                    bool one_line_comment = false;
                    int comment_position = -1;
                    if (CheckForComment(Text, i, out comment_position, out one_line_comment))
                    {
                        int new_line_ind = sb.ToString().IndexOf('\n');
                        if (new_line_ind != -1) sb = sb.Remove(0, new_line_ind + 1);
                        else sb = sb.Remove(0, sb.Length);
                    }
                    break;
                }
                i--;
            }

            //return RemovePossibleKeywords(sb);
            if (sb.Length > 0 && sb[sb.Length - 1] == '?')
                sb.Remove(sb.Length - 1, 1);
            return sb.ToString();

        }

        public override string FindExpressionFromAnyPosition(int off, string Text, int line, int col, out KeywordKind keyw, out string expr_without_brackets)
        {
            int i = off - 1;

            // это например вызов метода без параметров
            expr_without_brackets = null;
            keyw = KeywordKind.None;
            if (i < 0)
                return "";
            bool is_char = false;

            // идем в обе стороны от off
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (Text[i] != ' ' && (Char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&' || Text[i] == '?' && IsPunctuation(Text, i + 1) || Text[i] == '!'))
            {
                //sb.Remove(0,sb.Length);
                while (i >= 0 && (Char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&' || Text[i] == '?' && IsPunctuation(Text, i + 1) || Text[i] == '!'))
                {
                    //sb.Insert(0,Text[i]);//.Append(Text[i]);
                    i--;
                }
                is_char = true;
            }
            i = off;
            if (i < Text.Length && Text[i] != ' ' && (Char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&' || Text[i] == '?' && IsPunctuation(Text, i + 1) || Text[i] == '!'))
            {
                while (i < Text.Length && (Char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '!'))
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
            bool brackets = false;
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
                        else if (c == '\'' && !in_comment)
                        {
                            in_kav = !in_kav;
                        }
                        else if (c == '{' && !in_kav)
                        {
                            in_comment = true;
                        }
                        else if (c == '}' && !in_kav)
                        {
                            in_comment = false;
                        }
                        else if (c == '/' && !in_kav && !in_comment)
                        {
                            if (j + 1 < Text.Length && Text[j + 1] == '/')
                                break;
                        }
                        j++;
                    }
                    break;
                }
                else if ((c == '<' || c == '&' && j < Text.Length - 1 && Text[j + 1] == '<') && !in_comment)
                {
                    Stack<char> sk_stack = new Stack<char>();
                    if (c == '&')
                        j++;
                    sk_stack.Push('<');
                    j++;
                    bool generic = false;
                    while (j < Text.Length)
                    {
                        c = Text[j];
                        if (c == '>')
                        {
                            sk_stack.Pop();
                            if (sk_stack.Count == 0)
                            {
                                i = j + 1;
                                generic = true;
                                break;
                            }
                        }
                        else if (c == '<')
                        {
                            sk_stack.Push('<');

                        }
                        else if (!char.IsLetterOrDigit(c) && c != '?' && c != '&' && c != '.' && c != ' ' && c != '\t' && c != '\n' && c != ',' && c != '!')
                        {
                            break;
                        }
                        j++;
                    }
                    if (generic)
                    {
                        //break;
                    }
                }
                else if (c == '[' && !in_comment)
                {
                    brackets = true;
                    break;
                }
                else if (c == '{')
                {
                    in_comment = true;
                }
                else if (c == '}')
                {
                    if (!in_comment)
                        break;
                    else
                        in_comment = false;
                }
                else if (c == ' ' || c == '\t' || c == '\n' || c == '\r')
                {

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
                string ss = FindExpression(i, Text, line, col, out new_keyw);
                if (brackets && is_new)
                {
                    int ind = ss.ToLower().IndexOf("new");
                    if (ind != -1)
                        return ss.Substring(ind + 3);
                }
                if (is_new && ss != null && ss.IndexOf("new") == -1 && ss.IndexOf(":") != -1)
                    return expr_without_brackets + "(true?" + ss;
                return ss;
            }
            return null;
        }

        public override KeywordKind TestForKeyword(string Text, int i)
        {
            StringBuilder sb = new StringBuilder();
            int orig_i = i;
            int j = i;
            bool in_keyw = false;
            while (j >= 0 && Text[j] != '\n')
                j--;
            Stack<char> kav_stack = new Stack<char>();
            j++;
            bool in_format_str = false;
            while (j <= i)
            {
                if (Text[j] == '\'')
                {
                    if (kav_stack.Count == 0 && !in_keyw)
                    {
                        if (j == 0 || Text[j - 1] != '$')
                            kav_stack.Push('\'');
                        else
                        {
                            in_keyw = false;
                            in_format_str = true;
                        }

                    }
                    else if (kav_stack.Count > 0)
                        kav_stack.Pop();
                }
                else if (Text[j] == '{' && kav_stack.Count == 0 && !in_format_str)
                    in_keyw = true;
                else
                    if (Text[j] == '}')
                    in_keyw = false;
                j++;
            }
            j = i;
            if ((kav_stack.Count != 0 || in_keyw) && !in_format_str) return PascalABCCompiler.Parsers.KeywordKind.Punkt;
            if (j >= 0 && Text[j] == '.') return PascalABCCompiler.Parsers.KeywordKind.Punkt;
            while (j >= 0)
            {
                //if (Text[j] == '{') return PascalABCCompiler.Parsers.KeywordKind.Punkt;
                if (!in_keyw && (Text[j] == '\'' || Text[j] == '\n'))
                    break;
                if (Text[j] == '}')
                    in_keyw = true;
                else
                if (Text[j] == '/' && !in_keyw)
                    if (j > 0 && Text[j - 1] == '/') return PascalABCCompiler.Parsers.KeywordKind.Punkt;
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
                while (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '!'))
                {
                    sb.Insert(0, Text[i]);
                    i--;
                }
            string s = sb.ToString().ToLower();

            return GetKeywordKind(s);
        }

        public override string SkipNew(int off, string Text, ref KeywordKind keyw)
        {
            int tmp = off;
            string expr = null;
            while (off >= 0 && char.IsLetterOrDigit(Text[off])) off--;
            while (off >= 0 && (Text[off] == ' ' || char.IsControl(Text[off]))) off--;
            if (off >= 1 && Text[off] == '=' && Text[off - 1] == ':')
            {
                off -= 2;
                while (off >= 0 && (Text[off] == ' ' || char.IsControl(Text[off]))) off--;
                if (off >= 0 && (Text[off] == '_' || char.IsLetterOrDigit(Text[off]) || Text[off] == '!' || Text[off] == ']' || Text[off] == '>'))
                    expr = FindExpression(off + 1, Text, 0, 0, out keyw);
            }
            return expr;
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
            int next;
            KeywordKind keyw = TestForKeyword(Text, i);
            bool on_brace = false;
            if (keyw == KeywordKind.Punkt)
                return "";
            try
            {
                while (i >= bound)
                {
                    bool end = false;
                    char ch = Text[i];
                    if ((char.IsLetterOrDigit(ch) || ch == '_' || ch == '&' || ch == '!') && !isOperator(Text, i, out next))
                    {
                        num_in_ident = i;
                        if (kav.Count == 0 && tokens.Count == 0)
                        {
                            int tmp = i;
                            while (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&' || Text[i] == '!') && !isOperator(Text, i, out next))
                            {
                                i--;
                            }
                            while (i >= 0 && (Text[i] == ' ' || char.IsControl(Text[i]) || Text[i] == '}'))
                            {
                                if (Text[i] != '}')
                                    i--;
                                else
                                {
                                    while (i >= 0 && Text[i] != '{') //propusk kommentariev
                                        i--;
                                    if (i >= 0)
                                        i--;
                                }
                            }
                            if (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&' || Text[i] == '!') && !isOperator(Text, i, out next))
                            {
                                bound = i + 1;
                                TestForKeyword(Text, i, ref bound, punkt_sym, out keyw);
                                if (keyw == KeywordKind.New && comma_pressed)
                                    bound = 0;
                                if (keyw == KeywordKind.Function || keyw == KeywordKind.Constructor || keyw == KeywordKind.Destructor)
                                    return "";
                            }
                            else if (i >= 0 && Text[i] == '\'') return "";
                            i = tmp;
                        }
                        sb.Insert(0, ch);//.Append(Text[i]);
                    }
                    else if (ch == '.') sb.Insert(0, ch);
                    else if (ch == '}')
                    {
                        if (kav.Count == 0)
                        {
                            while (i >= 0 && Text[i] != '{')
                            {
                                sb.Insert(0, Text[i]);
                                i--;
                            }
                            if (i < 0)
                            {
                                break;
                            }
                            else if (Text[i] == '{')
                            {
                                sb.Insert(0, '{');
                            }
                        }
                        else
                            sb.Insert(0, ch);
                    }
                    else if (ch == '{')
                    {
                        if (kav.Count == 0)
                        {
                            sb.Insert(0, ch);
                            break;
                        }
                        else sb.Insert(0, ch);
                    }
                    else
                        switch (ch)
                        {
                            case ')':
                            case ']':
                            case '>':
                                if (kav.Count == 0)
                                {
                                    int j = i + 1;

                                    while (j < Text.Length && char.IsWhiteSpace(Text[j]))
                                        j++;
                                    if (ch != '>')
                                        tokens.Push(ch);
                                    if (ch == ')')
                                        skobki.Push(ch);
                                    if (tokens.Count > 0 || pressed_key == ',')
                                        sb.Insert(0, ch);
                                    else if (i == off - 1 || j == off && off == Text.Length || ugl_skobki.Count > 0 || j < Text.Length && Text[i - 1] != '-' && (Text[j] == '.' || Text[j] == '('))
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
                            case '[':
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
                                            while (i >= 0 && (Text[i] == ' ' || char.IsControl(Text[i]) || Text[i] == '}'))
                                            {
                                                if (Text[i] != '}')
                                                {
                                                    i--;
                                                }
                                                else
                                                {
                                                    while (i >= 0 && Text[i] != '{')
                                                    {
                                                        //propusk kommentariev
                                                        i--;
                                                    }
                                                    if (i >= 0)
                                                        i--;
                                                }
                                            }

                                            if (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&' || Text[i] == '!'))
                                            {
                                                bound = i + 1;
                                                TestForKeyword(Text, i, ref bound, punkt_sym, out keyw);
                                                if (keyw == KeywordKind.New)
                                                    bound = 0;
                                                else
                                                if (keyw != KeywordKind.None && tokens.Count == 0)
                                                    end = true;
                                                else
                                                    bound = 0;
                                            }
                                            else if (i >= 0 && Text[i] == '\'') return "";
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
                                            on_brace = true;
                                        //on_skobka = true;
                                        comma_pressed = false;
                                    }
                                }
                                else sb.Insert(0, ch);
                                break;
                            case '\'':
                                if (kav.Count == 0)
                                    kav.Push(ch);
                                else
                                    kav.Pop();
                                sb.Insert(0, ch);
                                if (kav.Count == 0 && tokens.Count == 0)
                                    end = true;
                                break;
                            default:
                                if (!(ch == ' ' || char.IsControl(ch)))
                                {
                                    if (ch == '^')
                                        sb.Insert(0, ch);
                                    else
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
                                            {
                                                sb.Insert(0, ch);//prodolzhaem
                                                if (isOperator(Text, i, out next))
                                                {
                                                    i = next;
                                                    continue;
                                                }
                                            }

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
                                    bool one_line_comment = false;
                                    int comment_position = -1;
                                    if (CheckForComment(Text, i - 1, out comment_position, out one_line_comment)) //proverjaem, net li kommenta ne predydushej stroke
                                    {
                                        if (!one_line_comment)
                                            end = true;
                                        else
                                        {
                                            sb.Insert(0, ch);
                                            i = comment_position;
                                        }

                                    }
                                    else
                                        sb.Insert(0, ch);//a inache vyrazhenie na neskolkih strokah
                                }
                                else
                                    sb.Insert(0, ch);
                                break;
                        }

                    if (end)
                    {
                        if (comma_pressed && !on_brace)
                            return "";
                        bool one_line_comment = false;
                        int comment_position = -1;
                        if (CheckForComment(Text, i, out comment_position, out one_line_comment))//proverka na kommentarii
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
            if (pressed_key == ',' && (!on_brace || skobki.Count == 0))
                return "";
            //return RemovePossibleKeywords(sb);
            return sb.ToString();
        }


        public override bool IsMethodCallParameterSeparator(char key)
        {
            return key == ',';
        }


        public override bool IsDefinitionIdentifierAfterKeyword(KeywordKind keyw)
        {
            if (keyw == PascalABCCompiler.Parsers.KeywordKind.Function || keyw == PascalABCCompiler.Parsers.KeywordKind.Constructor || keyw == PascalABCCompiler.Parsers.KeywordKind.Destructor || keyw == PascalABCCompiler.Parsers.KeywordKind.Type || keyw == PascalABCCompiler.Parsers.KeywordKind.Var
                   || keyw == PascalABCCompiler.Parsers.KeywordKind.Unit || keyw == PascalABCCompiler.Parsers.KeywordKind.Const || keyw == PascalABCCompiler.Parsers.KeywordKind.Program || keyw == PascalABCCompiler.Parsers.KeywordKind.Punkt)
                return true;
            return false;
        }

        public override bool IsTypeAfterKeyword(KeywordKind keyw)
        {
            if (keyw == KeywordKind.Colon || keyw == KeywordKind.Of || keyw == KeywordKind.TypeDecl)
                return true;
            return false;
        }

    }
}
