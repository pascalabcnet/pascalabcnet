using PascalABCCompiler.ParserTools.Directives;
using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PascalABCCompiler.Parsers
{
    public abstract class BaseLanguageInformation : ILanguageInformation
    {
        public abstract BaseKeywords KeywordsStorage { get; }

        public abstract Dictionary<string, DirectiveInfo> ValidDirectives { get; protected set; }

        public abstract string BodyStartBracket { get; }

        public abstract string BodyEndBracket { get; }

        public abstract string ParameterDelimiter { get; }

        public abstract string DelimiterInIndexer { get; }

        public abstract string ResultVariableName { get; }

        public abstract string GenericTypesStartBracket { get; }
        public abstract string GenericTypesEndBracket { get; }

        public abstract string ReturnTypeDelimiter { get; }

        public abstract string ProcedureName { get; }
        public abstract string FunctionName { get; }

        public abstract bool CaseSensitive { get; }

        public abstract bool IncludeDotNetEntities { get; }

        public abstract bool AddStandardUnitNamesToUserScope { get; }

        public abstract bool AddStandardNetNamespacesToUserScope { get; }

        public abstract bool UsesFunctionsOverlappingSourceContext { get; }

        public virtual Dictionary<string, string> SpecialModulesAliases => null;

        protected abstract string IntTypeName { get; }

        public abstract bool IsParams(string paramDescription);

        public virtual void RenameOrExcludeSpecialNames(SymInfo[] symInfos) { }

        // перенести сюда реализацию  EVA
        public abstract string ConstructHeader(string meth, IProcScope scope, int tabCount);

        // перенести сюда реализацию  EVA
        public abstract string ConstructHeader(IProcRealizationScope scope, int tabCount);

        // перенести сюда реализацию  EVA
        public abstract string ConstructOverridedMethodHeader(IProcScope scope, out int off);

        // перенести сюда реализацию  EVA
        public abstract string FindExpression(int off, string Text, int line, int col, out KeywordKind keyw);

        // перенести сюда реализацию  EVA
        public abstract string FindExpressionForMethod(int off, string Text, int line, int col, char pressed_key, ref int num_param);

        public string FindExpressionFromAnyPosition(int off, string Text, int line, int col, out string expr_without_brackets)
        {
            return FindExpressionFromAnyPosition(off, Text, line, col, out _, out expr_without_brackets);
        }

        // перенести сюда реализацию  EVA
        public abstract string FindExpressionFromAnyPosition(int off, string Text, int line, int col, out KeywordKind keyw, out string expr_without_brackets);

        public virtual string FindOnlyIdentifier(int off, string Text, int line, int col, ref string name)
        {
            int i = off - 1;
            if (i < 0)
                return "";
            bool is_char = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (Text[i] != ' ' && !char.IsControl(Text[i]))
            {
                sb.Remove(0, sb.Length);
                while (i >= 0 && (Char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&' || Text[i] == '!'))
                {
                    //sb.Insert(0,Text[i]);//.Append(Text[i]);
                    i--;
                }
                is_char = true;
            }
            i++;
            if (i < Text.Length && Text[i] != ' ' && !char.IsControl(Text[i]))
            {
                while (i < Text.Length && (Char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&' || Text[i] == '!'))
                {
                    sb.Append(Text[i]);//.Append(Text[i]);
                    i++;
                }
                is_char = true;
            }
            name = sb.ToString();
            KeywordKind keyw = KeywordKind.None;
            if (is_char)
            {
                return FindExpression(i, Text, line, col, out keyw);
            }
            return null;
        }

        public virtual string FindPattern(int off, string Text, out bool is_pattern)
        {
            System.Text.StringBuilder sb = null;
            is_pattern = false;
            if (off > 0 && (char.IsLetterOrDigit(Text[off - 1]) || Text[off - 1] == '_' || Text[off - 1] == '&' || Text[off - 1] == '!'))
            {
                sb = new System.Text.StringBuilder();
                int i = off - 1;
                is_pattern = true;
                while (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[off - 1] == '&' || Text[i] == '!'))
                    sb.Insert(0, Text[i--]);
                return sb.ToString();
            }
            return null;
        }

        public abstract string GetArrayDescription(string elementType, int rank);

        public abstract string GetClassKeyword(class_keyword keyw);

        public abstract string GetCompiledTypeRepresentation(Type t, System.Reflection.MemberInfo mi, ref int line, ref int col);

        public abstract string GetDescription(IBaseScope scope);

        
        // поменять на abstract EVA
        public virtual string GetDocumentTemplate(string lineText, string Text, int line, int col, int pos)
        {
            return null;
        }

        public string GetFullTypeName(ICompiledTypeScope scope, bool no_alias = true)
        {
            return GetFullTypeName(scope.CompiledType);
        }

        protected abstract string GetFullTypeName(Type ctn, bool no_alias = true);

        protected string GetDescriptionForProcedure(IProcScope scope)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string extensionType = null;
            if (scope.IsExtension && scope.Parameters.Length > 0)
            {
                extensionType = GetSimpleDescription(scope.Parameters[0].Type);
            }
            if (scope.IsExtension)
            {
                if (extensionType != null && extensionType.IndexOf(' ') != -1)
                {
                    sb.Append("(" + PascalABCCompiler.StringResources.Get("CODE_COMPLETION_EXTENSION") + " " + extensionType + ") ");
                    extensionType = null;
                }
                else
                {
                    sb.Append("(" + PascalABCCompiler.StringResources.Get("CODE_COMPLETION_EXTENSION") + ") ");
                }
            }

            if (scope.IsStatic) sb.Append("static ");
            if (scope.IsConstructor())
                sb.Append("constructor ");
            else
            if (scope.ReturnType == null && ProcedureName != null)
                sb.Append(ProcedureName + " ");
            else
                sb.Append(FunctionName + " ");
            if (!scope.IsConstructor())
            {
                if (extensionType != null)
                {
                    sb.Append(extensionType + ".");
                    sb.Append(scope.Name);
                }
                else
                {
                    sb.Append(GetTopScopeName(scope.TopScope));
                    sb.Append(scope.Name);
                }
            }
            else
            {
                sb.Append(GetTopScopeNameWithoutDot(scope.TopScope));
            }
            /*string[] template_args = scope.TemplateParameters;
			if (template_args != null)
			{
				sb.Append('<');
				for (int i=0; i<template_args.Length; i++)
				{
					sb.Append(template_args[i]);
					if (i < template_args.Length-1)
						sb.Append(',');
				}
				sb.Append('>');
			}*/
            sb.Append(GetGenericString(scope.TemplateParameters));
            sb.Append('(');
            IElementScope[] parameters = scope.Parameters;
            for (int i = 0; i < parameters.Length; i++)
            {
                if (scope.IsExtension && i == 0)
                    continue;
                sb.Append(GetSimpleDescription(parameters[i]));
                if (i < parameters.Length - 1)
                {
                    sb.Append("; ");
                }
            }
            sb.Append(')');
            if (scope.ReturnType != null && !scope.IsConstructor() && !(scope.ReturnType is IProcType && (scope.ReturnType as IProcType).Target == scope))
                sb.Append(ReturnTypeDelimiter + " " + GetSimpleDescription(scope.ReturnType));
            //if (scope.IsStatic) sb.Append("; static");
            if (scope.IsVirtual) sb.Append("; ");
            else if (scope.IsAbstract) sb.Append("; abstract");
            else if (scope.IsOverride) sb.Append("; override");
            else if (scope.IsReintroduce) sb.Append("; reintroduce");
            sb.Append(';');
            return sb.ToString();
        }

        public string[] GetIndexerString(IBaseScope scope)
        {
            IBaseScope tmp_si = scope;
            if (scope == null) return null;
            if (scope is IElementScope)
                if ((scope as IElementScope).Indexers.Length == 0)
                    scope = (scope as IElementScope).Type;
            if (scope is IProcScope) scope = (scope as IProcScope).ReturnType;
            if (!(scope is IElementScope))
            {
                ITypeScope ts = scope as ITypeScope;
                if (ts == null) return null;
                ITypeScope[] indexers = ts.Indexers;
                if (tmp_si is ITypeScope)
                    indexers = ts.StaticIndexers;
                if ((indexers == null || indexers.Length == 0) && !(ts is IArrayScope))
                    return null;
                StringBuilder sb = new StringBuilder();
                if (!(tmp_si is ITypeScope))
                    sb.Append("this");
                else
                    sb.Append(GetSimpleDescriptionWithoutNamespace(tmp_si as ITypeScope));
                sb.Append('[');
                if (indexers != null)
                    for (int i = 0; i < indexers.Length; i++)
                    {
                        sb.Append(GetSimpleDescriptionWithoutNamespace(indexers[i]));
                        if (i < indexers.Length - 1)
                            sb.Append(',');
                    }
                else
                    sb.Append(IntTypeName);
                sb.Append("] : ");
                sb.Append(GetSimpleDescriptionWithoutNamespace(ts.ElementType));
                return new string[1] { sb.ToString() };
            }
            else
            {
                IElementScope es = scope as IElementScope;
                ITypeScope[] indexers = es.Indexers;
                if (indexers == null || indexers.Length == 0 || es.ElementType == null) return null;
                StringBuilder sb = new StringBuilder();
                sb.Append(es.Name);
                sb.Append('[');
                for (int i = 0; i < indexers.Length; i++)
                {
                    sb.Append(GetSimpleDescriptionWithoutNamespace(indexers[i]));
                    if (i < indexers.Length - 1)
                        sb.Append(',');
                }
                sb.Append("] : ");
                sb.Append(GetSimpleDescriptionWithoutNamespace(es.ElementType));
                return new string[1] { sb.ToString() };
            }
        }

        public abstract string GetKeyword(SymbolKind kind);

        public KeywordKind GetKeywordKind(string name)
        {
            if (KeywordsStorage.KeywordKinds.TryGetValue(name, out var kind))
                return kind;
            else
                return KeywordKind.None;
        }

        public string GetShortName(ICompiledTypeScope scope)
        {
            return GetShortTypeName(scope.CompiledType);
        }

        protected string GetShortTypeName(MethodInfo mi)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(mi.Name);
            if (mi.GetGenericArguments().Length > 0)
                sb.Append(GenericTypesStartBracket + GenericTypesEndBracket);
            return sb.ToString();
        }

        public string GetShortName(ICompiledMethodScope scope)
        {
            return GetShortTypeName(scope.CompiledMethod);
        }

        public abstract string GetShortName(ICompiledConstructorScope scope);

        public string GetShortName(IProcScope scope)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(scope.Name);
            if (scope.TemplateParameters != null && scope.TemplateParameters.Length > 0)
                sb.Append("<>");
            return sb.ToString();
        }

        public abstract string GetShortTypeName(Type t, bool noalias = true);

        public abstract string GetSimpleDescription(IBaseScope scope);

        protected string GetSimpleDescriptionForType(ITypeScope scope)
        {
            string template_str = GetTemplateString(scope);
            if (scope.Name.StartsWith("$"))
                return scope.Name.Substring(1, scope.Name.Length - 1) + template_str;
            if (!string.IsNullOrEmpty(template_str))
                return scope.Name.Replace("<>", "") + template_str;
            return scope.Name + template_str;
        }

        protected string GetDescriptionForCompiledMethod(ICompiledMethodScope scope)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string extensionType = null;
            if (scope.IsExtension && scope.Parameters.Length > 0)
            {
                extensionType = GetSimpleDescription(scope.Parameters[0].Type);
            }
            if (scope.IsExtension)
            {
                if (extensionType.IndexOf(' ') != -1)
                {
                    sb.Append("(" + PascalABCCompiler.StringResources.Get("CODE_COMPLETION_EXTENSION") + " " + extensionType + ") ");
                }
                else
                {
                    sb.Append("(" + PascalABCCompiler.StringResources.Get("CODE_COMPLETION_EXTENSION") + ") ");
                    extensionType = null;
                }
            }

            if (scope.IsStatic && !scope.IsGlobal) sb.Append("static ");
            if (scope.ReturnType == null && ProcedureName != null)
                sb.Append(ProcedureName + " ");
            else
                sb.Append(FunctionName + " ");
            Dictionary<string, string> generic_param_args = null;
            Dictionary<string, int> class_generic_table = new Dictionary<string, int>();
            ParameterInfo[] pis = scope.CompiledMethod.GetParameters();
            Type[] tt = scope.CompiledMethod.GetGenericArguments();
            int gen_ind = 0;
            if (!scope.IsExtension)
            {
                sb.Append(GetShortTypeName(scope.CompiledMethod.DeclaringType));
                int ind = 0;
                foreach (Type gen_arg in scope.CompiledMethod.DeclaringType.GetGenericArguments())
                {
                    if (gen_arg.IsGenericParameter)
                    {
                        if (generic_param_args == null)
                            generic_param_args = new Dictionary<string, string>();
                        if (scope.GenericArgs != null && scope.GenericArgs.Count > ind)
                            generic_param_args.Add(gen_arg.Name, scope.GenericArgs[ind]);
                        else if (scope.DeclaringType.TemplateArguments != null && scope.DeclaringType.TemplateArguments.Length > ind)
                            generic_param_args.Add(gen_arg.Name, scope.DeclaringType.TemplateArguments[ind]);
                    }
                    ind++;
                }

            }
            else
            {
                gen_ind = 0;
                generic_param_args = new Dictionary<string, string>();
                for (int i = 0; i < pis.Length; i++)
                {
                    if (i == 0)
                    {
                        Type[] class_generic_args = pis[i].ParameterType.GetGenericArguments();
                        for (int j = 0; j < class_generic_args.Length; j++)
                        {
                            if (!class_generic_table.ContainsKey(class_generic_args[i].Name))
                                class_generic_table.Add(class_generic_args[i].Name, j);
                            if (scope.GenericArgs != null && scope.GenericArgs.Count > j)
                            {
                                if (scope.DeclaringType.GenericInstances.Length > 0)
                                    generic_param_args[class_generic_args[i].Name] = GetSimpleDescription(scope.DeclaringType.GenericInstances[0]);
                                else if (scope.DeclaringType is ICompiledTypeScope)
                                {
                                    Type ctn = (scope.DeclaringType as ICompiledTypeScope).CompiledType;
                                    if (ctn.IsGenericType && !ctn.IsGenericTypeDefinition)
                                        generic_param_args[class_generic_args[i].Name] = GetSimpleDescriptionForCompiledType((scope.DeclaringType as ICompiledTypeScope).GetCompiledGenericArguments()[0]);
                                }
                            }
                            else if (scope.DeclaringType.TemplateArguments != null && scope.DeclaringType.TemplateArguments.Length > j)
                                generic_param_args[class_generic_args[i].Name] = scope.DeclaringType.TemplateArguments[j];
                        }
                        break;
                    }
                }
                if (extensionType == null)
                    sb.Append(GetShortTypeName(scope.CompiledMethod.GetParameters()[0].ParameterType));
            }
            //if (scope.Name != "Invoke")
            {
                if (extensionType == null)
                    sb.Append(".");
                sb.Append(scope.Name);
            }

            if (scope.CompiledMethod.GetGenericArguments().Length > 0)
            {
                sb.Append(GenericTypesStartBracket);
                for (int i = gen_ind; i < tt.Length; i++)
                {
                    if (class_generic_table.ContainsKey(tt[i].Name))
                    {
                        int ind = class_generic_table[tt[i].Name];
                        if (scope.GenericArgs != null && scope.GenericArgs.Count > ind)
                        {
                            if (scope.DeclaringType.GenericInstances.Length > ind)
                            {
                                sb.Append(GetSimpleDescription(scope.DeclaringType.GenericInstances[ind]));
                                if (!generic_param_args.ContainsKey(tt[i].Name))
                                    generic_param_args.Add(tt[i].Name, GetSimpleDescription(scope.DeclaringType.GenericInstances[ind]));
                            }
                            else if (scope.DeclaringType is ICompiledTypeScope)
                            {
                                Type ctn = (scope.DeclaringType as ICompiledTypeScope).CompiledType;
                                if (ctn.IsGenericType && !ctn.IsGenericTypeDefinition)
                                {
                                    sb.Append(GetSimpleDescription((scope.DeclaringType as ICompiledTypeScope).GetCompiledGenericArguments()[ind]));
                                    if (!generic_param_args.ContainsKey(tt[i].Name))
                                        generic_param_args.Add(tt[i].Name, GetSimpleDescription((scope.DeclaringType as ICompiledTypeScope).GetCompiledGenericArguments()[ind]));
                                }
                            }
                        }
                    }

                    else
                        sb.Append(tt[i].Name);
                    if (i < tt.Length - 1) sb.Append(", ");
                }
                sb.Append(GenericTypesEndBracket);
            }
            sb.Append('(');

            for (int i = 0; i < pis.Length; i++)
            {
                if (i == 0 && scope.IsExtension)
                    continue;
                if (pis[i].ParameterType.IsByRef)
                    sb.Append("var ");
                else if (IsParams(pis[i]))
                    sb.Append("params ");
                sb.Append(pis[i].Name);
                sb.Append(": ");
                string inst_type = null;
                if (!pis[i].ParameterType.IsByRef)
                {
                    if (scope.GenericArgs != null)
                    {
                        inst_type = get_type_instance(pis[i].ParameterType, scope.GenericArgs, generic_param_args);
                    }
                    if (inst_type == null)
                        sb.Append(GetShortTypeName(pis[i].ParameterType, false));
                    else
                        sb.Append(inst_type);
                    if (pis[i].IsOptional)
                    {
                        sb.Append(" := ");
                        if (pis[i].DefaultValue != null)
                        {
                            if (pis[i].DefaultValue is string) sb.Append($"'{pis[i].DefaultValue.ToString()}'");
                            else sb.Append(pis[i].DefaultValue.ToString());
                        }
                        else sb.Append("nil");
                    }
                }
                else
                {
                    Type t = pis[i].ParameterType.GetElementType();
                    if (scope.GenericArgs != null)
                    {
                        inst_type = get_type_instance(t, scope.GenericArgs, generic_param_args);
                    }
                    if (inst_type == null)
                        sb.Append(GetShortTypeName(t, false));
                    else
                        sb.Append(inst_type);
                }
                if (i < pis.Length - 1)
                    sb.Append(ParameterDelimiter + " ");
            }
            sb.Append(')');
            string ret_inst_type = null;
            if (scope.ReturnType != null)
            {
                if (scope.GenericArgs != null)
                {
                    ret_inst_type = get_type_instance(scope.CompiledMethod.ReturnType, scope.GenericArgs, generic_param_args);
                }
                if (ret_inst_type == null)
                {
                    if (scope.ReturnType is ICompiledTypeScope)
                        sb.Append(ReturnTypeDelimiter + " " + GetFullTypeName((scope.ReturnType as ICompiledTypeScope).CompiledType, false));
                    else
                        sb.Append(ReturnTypeDelimiter + " " + GetSimpleDescription(scope.ReturnType));
                }
                else
                    sb.Append(ReturnTypeDelimiter + " " + ret_inst_type);
            }
            //if (scope.CompiledMethod.IsStatic) sb.Append("; static");
            if (scope.IsVirtual) sb.Append("; ");
            else if (scope.IsAbstract) sb.Append("; abstract");
            //else if (scope.CompiledMethod.IsHideBySig) sb.Append("; reintroduce");
            sb.Append(';');
            return sb.ToString();
        }

        // TODO: Адаптировать к многоязычности EVA
        protected string GetDescriptionForCompiledMethod(MethodInfo mi)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (mi.IsPublic)
                sb.Append("public ");
            else if (mi.IsFamily)
                sb.Append("protected ");
            if (mi.IsStatic) sb.Append("static ");
            if (mi.ReturnType == typeof(void) && ProcedureName != null)
                sb.Append(ProcedureName + " ");
            else
                sb.Append(FunctionName + " ");
            sb.Append(prepare_member_name(mi.Name));
            if (mi.GetGenericArguments().Length > 0)
            {
                Type[] tt = mi.GetGenericArguments();
                sb.Append(GenericTypesStartBracket);
                for (int i = 0; i < tt.Length; i++)
                {
                    sb.Append(tt[i].Name);
                    if (i < tt.Length - 1) sb.Append(", ");
                }
                sb.Append(GenericTypesEndBracket);
            }
            sb.Append('(');
            ParameterInfo[] pis = mi.GetParameters();
            for (int i = 0; i < pis.Length; i++)
            {
                if (pis[i].ParameterType.IsByRef)
                    sb.Append("var ");
                else if (IsParams(pis[i]))
                    sb.Append("params ");
                sb.Append(prepare_member_name(pis[i].Name));
                sb.Append(": ");
                string inst_type = null;
                if (!pis[i].ParameterType.IsByRef)
                {
                    sb.Append(GetFullTypeName(pis[i].ParameterType));
                    if (pis[i].IsOptional)
                    {
                        sb.Append(" := ");
                        if (pis[i].DefaultValue != null)
                        {
                            if (pis[i].DefaultValue is string) sb.Append($"'{pis[i].DefaultValue.ToString()}'");
                            else sb.Append(pis[i].DefaultValue.ToString());
                        }
                        else sb.Append("nil");
                    }
                }
                else
                {
                    Type t = pis[i].ParameterType.GetElementType();
                    sb.Append(GetFullTypeName(t));
                }

                if (i < pis.Length - 1)
                    sb.Append(ParameterDelimiter + " ");
            }
            sb.Append(')');
            string ret_inst_type = null;
            if (mi.ReturnType != typeof(void))
            {
                sb.Append(ReturnTypeDelimiter + " " + GetFullTypeName(mi.ReturnType));
            }
            //if (scope.CompiledMethod.IsStatic) sb.Append("; static");
            if (mi.IsVirtual && !mi.IsFinal) sb.Append("; ");
            else if (mi.IsAbstract) sb.Append("; abstract");
            //else if (scope.CompiledMethod.IsHideBySig) sb.Append("; reintroduce");
            sb.Append(';');
            return sb.ToString();
        }

        protected string prepare_member_name(string s)
        {
            if (IsKeyword(s))
                return "&" + s;
            return s;
        }

        // TODO: Адаптировать к многоязычности EVA
        protected string GetSimpleDescriptionForCompiledType(ICompiledTypeScope scope, bool fullName)
        {
            if (scope.CompiledType.Name != null && scope.CompiledType.Name.Contains("Func`"))
            {
                return getLambdaRepresentation(scope, true);
            }
            else if (scope.CompiledType.Name != null && scope.CompiledType.Name.Contains("Action`"))
            {
                return getLambdaRepresentation(scope, false);
            }
            else if (scope.CompiledType.Name != null && scope.CompiledType.Name.Contains("Predicate`1"))
            {
                return getLambdaRepresentation(scope, false);
            }
            else if (scope.CompiledType.Name == "IEnumerable`1")
            {
                ITypeScope[] instances = scope.GenericInstances;
                if (instances != null && instances.Length > 0)
                {
                    return "sequence of " + GetSimpleDescriptionWithoutNamespace(instances[0]);
                }
                else
                    return "sequence of T";
            }
            else if (scope.CompiledType.Name != null && scope.CompiledType.Name.Contains("Tuple`"))
            {
                ITypeScope[] instances = scope.GenericInstances;
                if (instances != null && instances.Length > 0)
                {
                    return get_tuple_string(instances);
                }
                else
                    return "(T1,...)";
            }
            else
            {
                string s = !fullName ? GetShortTypeName(scope.CompiledType) : GetFullTypeName(scope.CompiledType);
                ITypeScope[] instances = scope.GenericInstances;
                if (instances != null && instances.Length > 0)
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
                        if (i < instances.Length - 1) sb.Append(", ");
                    }
                    sb.Append(GenericTypesEndBracket);
                    s = sb.ToString();
                }
                return s;
            }
        }

        protected string GetSimpleDescriptionForCompiledType(ICompiledTypeScope scope)
        {
            return GetSimpleDescriptionForCompiledType(scope, false);
        }

        protected bool IsParams(ParameterInfo _par)
        {
            object[] objarr = _par.GetCustomAttributes(typeof(ParamArrayAttribute), true);
            if ((objarr == null) || (objarr.Length == 0))
            {
                return false;
            }
            return true;
        }

        protected string GetTopScopeName(IBaseScope sc)
        {
            if (sc == null) return "";
            if (sc.Name == "" || sc.Name.Contains("$") || sc.Name == "PABCSystem") return "";
            if (sc is IProcScope) return "";
            if (sc is ITypeScope)
            {
                return sc.Name + (((sc as ITypeScope).TemplateArguments != null && !sc.Name.EndsWith("<>") && sc.Name != "class") ? "<>" : "") + ".";
            }

            if (sc is IInterfaceUnitScope && SpecialModulesAliases != null)
            {
                var p = SpecialModulesAliases.FirstOrDefault(kv => kv.Value == sc.Name);

                if (!p.Equals(default(KeyValuePair<string, string>)))
                    return p.Key + ".";
            }

            return sc.Name + ".";
        }

        protected string GetTopScopeNameWithoutDot(IBaseScope sc)
        {
            if (sc == null) return "";
            if (sc.Name == "" || sc.Name.Contains("$") || sc.Name == "PABCSystem") return "";
            if (sc is IProcScope) return "";
            if (sc is ITypeScope)
            {
                return sc.Name + (((sc as ITypeScope).TemplateArguments != null && !sc.Name.EndsWith("<>")) ? "<>" : "");
            }

            if (sc is IInterfaceUnitScope && SpecialModulesAliases != null)
            {
                var p = SpecialModulesAliases.FirstOrDefault(kv => kv.Value == sc.Name);

                if (!p.Equals(default(KeyValuePair<string, string>)))
                    return p.Key + ".";
            }

            return sc.Name;
        }

        protected abstract string kind_of_param(IElementScope scope);

        protected string GetSimpleDescriptionForElementScope(IElementScope scope)
        {
            string type_name = GetSimpleDescription(scope.Type);
            if (type_name.StartsWith("$")) type_name = type_name.Substring(1, type_name.Length - 1);
            return kind_of_param(scope) + scope.Name + ": " + type_name + (scope.ConstantValue != null ? (":=" + scope.ConstantValue.ToString()) : "");
        }

        protected string GetSimpleDescriptionForProcedure(IProcScope scope)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (scope.TopScope is ITypeScope && scope.Realization == null)
                sb.Append(GetTopScopeName(scope.TopScope));
            sb.Append(scope.Name);
            sb.Append(GetGenericString(scope.TemplateParameters));
            sb.Append('(');
            IElementScope[] parameters = scope.Parameters;
            for (int i = 0; i < parameters.Length; i++)
            {
                sb.Append(GetSimpleDescription(parameters[i]));
                if (i < parameters.Length - 1)
                {
                    sb.Append(ParameterDelimiter + " ");
                }
            }
            sb.Append(')');
            return sb.ToString();
        }

        protected string get_tuple_string(ITypeScope[] generic_args)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            for (int i = 0; i < generic_args.Length; i++)
            {
                sb.Append(GetSimpleDescriptionWithoutNamespace(generic_args[i]));
                if (i < generic_args.Length - 1)
                    sb.Append(",");
            }
            sb.Append(")");
            return sb.ToString();
        }

        protected string get_tuple_string(Type t)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            Type[] generic_args = t.GetGenericArguments();
            for (int i = 0; i < generic_args.Length; i++)
            {
                sb.Append(GetShortTypeName(generic_args[i], false));
                if (i < generic_args.Length - 1)
                    sb.Append(",");
            }
            sb.Append(")");
            return sb.ToString();
        }

        protected string GetGenericString(string[] template_args)
        {
            StringBuilder sb = new StringBuilder();
            if (template_args != null)
            {
                sb.Append(GenericTypesStartBracket);
                for (int i = 0; i < template_args.Length; i++)
                {
                    sb.Append(template_args[i]);
                    if (i < template_args.Length - 1)
                        sb.Append(", ");
                }
                sb.Append(GenericTypesEndBracket);
            }
            return sb.ToString();
        }

        public string GetSimpleDescriptionWithoutNamespace(ITypeScope scope)
        {
            ICompiledTypeScope cts = scope as ICompiledTypeScope;
            if (cts == null)
                return GetSimpleDescription(scope);
            string s = GetShortName(cts);
            ITypeScope[] instances = scope.GenericInstances;
            if (instances != null && instances.Length > 0)
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
                    if (i < instances.Length - 1) sb.Append(", ");
                }
                sb.Append(GenericTypesEndBracket);
                s = sb.ToString();
            }
            return s;
        }

        public abstract string GetStandardTypeByKeyword(KeywordKind keyw);

        public abstract string GetStringForChar(char c);

        public abstract string GetStringForSharpChar(int num);

        public abstract string GetStringForString(string s);

        public abstract string GetSynonimDescription(ITypeScope scope);

        public abstract string GetSynonimDescription(ITypeSynonimScope scope);

        public abstract string GetSynonimDescription(IProcScope scope);

        protected string GetTemplateString(ITypeScope scope)
        {
            string[] generic_params = scope.TemplateArguments;
            if (generic_params != null && generic_params.Length > 0)
            {
                ITypeScope[] gen_insts = scope.GenericInstances;
                if (gen_insts == null || gen_insts.Length == 0)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(GenericTypesStartBracket);
                    for (int i = 0; i < generic_params.Length; i++)
                    {
                        sb.Append(generic_params[i]);
                        if (i < generic_params.Length - 1)
                            sb.Append(',');
                    }
                    sb.Append(GenericTypesEndBracket);
                    return sb.ToString();
                }
                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(GenericTypesStartBracket);
                    for (int i = 0; i < gen_insts.Length; i++)
                    {
                        sb.Append(GetSimpleDescriptionWithoutNamespace(gen_insts[i]));
                        if (i < gen_insts.Length - 1)
                            sb.Append(", ");
                    }
                    sb.Append(GenericTypesEndBracket);
                    return sb.ToString();
                }
            }
            return null;
        }

        public abstract bool IsDefinitionIdentifierAfterKeyword(KeywordKind keyw);

        public bool IsKeyword(string value)
        {
            return KeywordsStorage.KeywordsForIntellisenseSet.Contains(value);
        }

        public abstract bool IsMethodCallParameterSeparator(char key);

        public virtual bool IsNamespaceAfterKeyword(KeywordKind keyw)
        {
            return keyw == KeywordKind.Uses;
        }

        public virtual bool IsOpenBracketForIndex(char key)
        {
            return key == '[';
        }

        public virtual bool IsOpenBracketForMethodCall(char key)
        {
            return key == '(';
        }

        public abstract bool IsTypeAfterKeyword(KeywordKind keyw);

        // перенести реализацию сюда EVA
        public virtual string SkipNew(int off, string Text, ref KeywordKind keyw)
        {
            return null;
        }

        public abstract KeywordKind TestForKeyword(string Text, int i);

        // TODO: поправить для универсальности EVA
        protected void TestForKeyword(string Text, int i, ref int bound, bool sym_punkt, out KeywordKind keyword)
        {
            StringBuilder sb = new StringBuilder();
            while (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_'))
            {
                sb.Insert(0, Text[i]);
                i--;
            }
            while (i >= 0 && char.IsWhiteSpace(Text[i]))
            {
                i--;
            }
            if (i >= 0 && (Text[i] == '.' || Text[i] == '&'))
                sb.Insert(0, Text[i]);
            string s = sb.ToString().ToLower();
            if (s == "new")
            {
                bound = i + 1;
                keyword = KeywordKind.New;
            }
            else if ((s == "procedure" || s == "function") && !sym_punkt)
            {
                keyword = KeywordKind.Function;
            }
            else if (s == "constructor" && !sym_punkt)
            {
                keyword = KeywordKind.Constructor;
            }
            else if (s == "destructor" && !sym_punkt)
            {
                keyword = KeywordKind.Destructor;
            }
            else if (s == "uses")
            {
                keyword = KeywordKind.Uses;
            }
            else if (s == "inherited")
            {
                bound = i + 1;
                keyword = KeywordKind.Inherited;
            }
            else if (s == "raise")
            {
                keyword = KeywordKind.Raise;
            }
            else if (IsKeyword(s))
            {
                keyword = KeywordKind.CommonKeyword;
            }

            else keyword = KeywordKind.None;
        }

        protected bool IsPunctuation(string Text, int ind)
        {
            while (ind < Text.Length && char.IsWhiteSpace(Text[ind]))
                ind++;
            if (ind >= Text.Length)
                return true;
            return char.IsPunctuation(Text[ind]);
        }

        protected string getLambdaRepresentation(Type t, bool has_return_value, List<string> parameters, Dictionary<string, string> generic_param_args = null)
        {
            StringBuilder sb = new StringBuilder();
            if (parameters.Count != t.GetGenericArguments().Length || true)
            {
                List<string> old_parameters = parameters;
                parameters = new List<string>();
                int ind = 0;
                foreach (Type generic_arg in t.GetGenericArguments())
                {
                    if (generic_arg.IsGenericParameter)
                    {
                        if (generic_param_args != null)
                        {
                            if (generic_param_args.ContainsKey(generic_arg.Name))
                                parameters.Add(generic_param_args[generic_arg.Name]);
                            else
                                parameters.Add(generic_arg.Name);
                        }
                        else if (ind < old_parameters.Count)
                        {
                            parameters.Add(old_parameters[ind]);
                            ind++;
                        }
                        else
                            parameters.Add(generic_arg.Name);
                    }
                    else
                        //parameters.Add(GetShortTypeName(generic_arg, false));
                        parameters.Add(get_type_instance(generic_arg, parameters, generic_param_args));
                }
            }
            foreach (string parameter in parameters)
            {
                if (!isIdentifier(parameter))
                    return get_type_instance(t, parameters, generic_param_args, true);
            }
            if (has_return_value)
            {
                if (parameters.Count > 2)
                    sb.Append("(" + string.Join(",", parameters.GetRange(0, parameters.Count - 1).ToArray()) + ")->" + parameters[parameters.Count - 1]);
                else if (parameters.Count > 1)
                    sb.Append(parameters[0] + "->" + parameters[1]);
                else if (parameters.Count == 1)
                {
                    if (t.FullName == "System.Predicate`1" || t.Name == "Predicate`1")
                        sb.Append(parameters[0] + "->boolean");
                    else
                        sb.Append("()->" + parameters[0]);
                }

            }
            else if (parameters.Count > 0)
            {
                if (t.FullName == "System.Predicate`1" || t.Name == "Predicate`1")
                    sb.Append(parameters[0] + "->boolean");
                else if (parameters.Count > 1)
                    sb.Append("(" + string.Join(",", parameters.ToArray()) + ")->()");
                else
                    sb.Append(parameters[0] + "->()");
            }
            if (sb.Length == 0)
                sb.Append(t.Name);
            return sb.ToString();
        }

        protected string getLambdaRepresentation(ICompiledTypeScope scope, bool has_return_value)
        {
            StringBuilder sb = new StringBuilder();
            ITypeScope[] instances = scope.GenericInstances;
            List<string> parameters = new List<string>();
            if (instances != null && instances.Length > 0)
            {
                foreach (ITypeScope ts in instances)
                    parameters.Add(GetSimpleDescriptionWithoutNamespace(ts));
            }
            /*else
            {
                foreach (Type t in scope.CompiledType.GetGenericArguments())
                    parameters.Add(t.Name);
            }*/
            return getLambdaRepresentation(scope.CompiledType, has_return_value, parameters);
        }

        protected bool isIdentifier(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsLetterOrDigit(c))
                    return false;
            }
            return true;
        }

        protected string get_type_instance(Type t, List<string> generic_args, Dictionary<string, string> generic_param_args = null, bool no_alias = false)
        {
            if (t.IsGenericParameter)
            {
                if (generic_param_args != null)
                {
                    if (generic_param_args.ContainsKey(t.Name))
                        return generic_param_args[t.Name];
                    else
                        return t.Name;
                }
                else if (t.GenericParameterPosition < generic_args.Count)
                    return generic_args[t.GenericParameterPosition];
                else
                    return t.Name;
            }
            if (t.IsArray)
                return "array of " + get_type_instance(t.GetElementType(), generic_args, generic_param_args, no_alias);
            if (t.ContainsGenericParameters)
            {
                if (!no_alias)
                {
                    if (t.Name == "IEnumerable`1")
                    {
                        Type tt = t.GetGenericArguments()[0];
                        if (generic_param_args != null)
                        {
                            if (tt.IsGenericParameter && generic_param_args.ContainsKey(tt.Name))
                                return "sequence of " + generic_param_args[tt.Name];
                            else
                                return "sequence of " + GetShortTypeName(tt, false);
                        }
                        else
                            return "sequence of " + (generic_args.Count > 0 ? generic_args[0] : GetShortTypeName(tt, false));
                    }
                    else if (t.Name.Contains("Func`") || t.Name.Contains("Predicate`"))
                        return getLambdaRepresentation(t, true, generic_args, generic_param_args);
                    else if (t.Name.Contains("Action`"))
                        return getLambdaRepresentation(t, false, generic_args, generic_param_args);
                    else if (t.Name.Contains("Tuple`"))
                        return get_tuple_string(t);
                }
                string name = GetShortTypeName(t);
                StringBuilder sb = new StringBuilder();
                int ind = name.IndexOf(GenericTypesStartBracket);
                if (ind == -1)
                    return name;
                sb.Append(name.Substring(0, ind));
                Type[] args = t.GetGenericArguments();
                sb.Append(GenericTypesStartBracket);
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].IsGenericParameter)
                    {
                        if (generic_param_args != null)
                        {
                            if (generic_param_args.ContainsKey(args[i].Name))
                                sb.Append(generic_param_args[args[i].Name]);
                            else
                                sb.Append(args[i].Name);
                        }
                        else if (args[i].GenericParameterPosition < generic_args.Count)
                            sb.Append(generic_args[args[i].GenericParameterPosition]);
                        else
                            sb.Append(args[i].Name);
                    }
                    else
                        sb.Append(get_type_instance(args[i], generic_args, generic_param_args));
                    if (i < args.Length - 1)
                        sb.Append(", ");
                }
                sb.Append(GenericTypesEndBracket);
                return sb.ToString();
            }
            return GetFullTypeName(t, no_alias);
        }

        public int FindClosingParenthesis(string descriptionAfterOpeningParenthesis, char parenthesis)
        {
            char openingParenthesis = parenthesis == ')' ? '(' : '[';

            int count = 1;

            int i = 0;

            foreach (char c in descriptionAfterOpeningParenthesis)
            {
                if (c == openingParenthesis)
                {
                    count++;
                }
                else if (c == parenthesis)
                {
                    count--;
                }

                if (count == 0)
                {
                    break;
                }

                i++;
            }

            return i;
        }

        public int FindParamDelim(string descriptionAfterOpeningParenthesis, int number) => FindParamDelim(descriptionAfterOpeningParenthesis, number, ParameterDelimiter);

        public int FindParamDelimForIndexer(string descriptionAfterOpeningParenthesis, int number) => FindParamDelim(descriptionAfterOpeningParenthesis, number, DelimiterInIndexer);

        public int FindParamDelim(string descriptionAfterOpeningParenthesis, int number, string paramDelim)
        {
            int count = 1;

            int i = 0;

            int delimNum = 0;

            foreach (char c in descriptionAfterOpeningParenthesis)
            {
                if (c == '(' || c == '[' || c == '<' || c == '{')
                {
                    count++;
                }
                else if (c == ')' || c == ']' || c == '>' || c == '}')
                {
                    count--;
                }

                if (count == 0)
                {
                    break;
                }
                // если не внутри внутренних скобок
                else if (count == 1)
                {
                    if (descriptionAfterOpeningParenthesis.Substring(i).StartsWith(paramDelim))
                    {
                        delimNum++;

                        if (delimNum == number)
                            break;
                    }
                }

                i++;
            }

            if (delimNum < number || i == descriptionAfterOpeningParenthesis.Length)
                return -1;

            return i;
        }
    }
}
