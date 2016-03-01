// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using SymbolTable;
using PascalABCCompiler.TreeRealization;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Parsers;

namespace CodeCompletion
{
    public enum KeywordKind
    {
        kw_none,
        kw_new,
        kw_proc,
        kw_constr,
        kw_destr,
        kw_uses,
        kw_inherited,
        kw_raise,
        kw_type,
        kw_var,
        kw_unit,
        kw_punkt,
        kw_colon,
        kw_const,
        kw_program,
        kw_of,
        kw_typedecl
    }



    public class ExtensionMethodsCache
    {

    }

    class TypeUtility
    {
        public static bool IsStandType(Type t)
        {
            if (t.IsEnum) return false;
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Int32:
                case TypeCode.Double:
                case TypeCode.Boolean:
                case TypeCode.String:
                case TypeCode.Char:
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Single: return true;
            }
            if (t.IsPointer) return true;
            return false;
        }
        public static string GetTypeName(Type ctn)
        {
            TypeCode tc = Type.GetTypeCode(ctn);
            if (ctn.FullName == null && !ctn.IsArray && !ctn.IsGenericTypeDefinition)
                return "T";
            if (!ctn.IsEnum)
                switch (tc)
                {
                    case TypeCode.Int32: return PascalABCCompiler.TreeConverter.compiler_string_consts.integer_type_name;
                    case TypeCode.Double: return PascalABCCompiler.TreeConverter.compiler_string_consts.real_type_name;
                    case TypeCode.Boolean: return PascalABCCompiler.TreeConverter.compiler_string_consts.bool_type_name;
                    case TypeCode.String: return PascalABCCompiler.TreeConverter.compiler_string_consts.string_type_name;
                    case TypeCode.Char: return PascalABCCompiler.TreeConverter.compiler_string_consts.char_type_name;
                    case TypeCode.Byte: return PascalABCCompiler.TreeConverter.compiler_string_consts.byte_type_name;
                    case TypeCode.SByte: return PascalABCCompiler.TreeConverter.compiler_string_consts.sbyte_type_name;
                    case TypeCode.Int16: return PascalABCCompiler.TreeConverter.compiler_string_consts.short_type_name;
                    case TypeCode.Int64: return PascalABCCompiler.TreeConverter.compiler_string_consts.long_type_name;
                    case TypeCode.UInt16: return PascalABCCompiler.TreeConverter.compiler_string_consts.ushort_type_name;
                    case TypeCode.UInt32: return PascalABCCompiler.TreeConverter.compiler_string_consts.uint_type_name;
                    case TypeCode.UInt64: return PascalABCCompiler.TreeConverter.compiler_string_consts.ulong_type_name;
                    case TypeCode.Single: return PascalABCCompiler.TreeConverter.compiler_string_consts.float_type_name;
                }
            else return ctn.FullName;

            if (ctn.Name.Contains("`"))
            {
                int len = ctn.GetGenericArguments().Length;
                Type[] gen_ps = ctn.GetGenericArguments();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(ctn.Namespace + "." + ctn.Name.Substring(0, ctn.Name.IndexOf('`')));
                sb.Append('<');
                for (int i = 0; i < len; i++)
                {
                    sb.Append(gen_ps[i].Name);
                    if (i < len - 1)
                        sb.Append(',');
                }
                sb.Append('>');
                return sb.ToString();
            }
            if (ctn.IsArray) return "array of " + GetTypeName(ctn.GetElementType());
            if (ctn == Type.GetType("System.Void*")) return PascalABCCompiler.TreeConverter.compiler_string_consts.pointer_type_name;
            return ctn.FullName;
        }

        public static string GetShortTypeName(Type ctn)
        {
            TypeCode tc = Type.GetTypeCode(ctn);
            if (ctn.FullName == null && !ctn.IsArray && !ctn.IsGenericTypeDefinition)
                return "T";
            if (ctn.IsEnum) return ctn.Name;

            if (ctn.Name.Contains("`"))
            {
                int len = ctn.GetGenericArguments().Length;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(ctn.Name.Substring(0, ctn.Name.IndexOf('`')));
                sb.Append('<');
                sb.Append('>');
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
            //if (ctn == Type.GetType("System.Void*")) return PascalABCCompiler.TreeConverter.compiler_string_consts.pointer_type_name;
            return ctn.Name;
        }

        public static string GetTopScopeName(SymScope sc)
        {
            if (sc == null || sc.si == null) return "";
            if (sc.si.name == "" || sc.si.name.Contains("$") || sc.si.name == PascalABCCompiler.TreeConverter.compiler_string_consts.system_unit_file_name) return "";
            if (sc is ProcScope) return "";
            return sc.si.name + ".";
        }
    }



    public class SymScope : IBaseScope
    {
        public SymScope topScope;
        public string file_name;
        public List<SymScope> used_units;
        public SymInfo si;
        public location loc;
        public location head_loc;
        public location body_loc;
        public List<SymScope> members;
        public Hashtable symbol_table;
        public int cur_line = -1;
        public int cur_col = -1;
        public SymScope declaringUnit;
        public string documentation;
        public access_modifer acc_mod;
        public bool is_static = false;
        public bool is_virtual = false;
        public bool is_abstract = false;
        public bool is_override = false;
        public bool is_reintroduce = false;
        
        public List<Position> regions;
        public Dictionary<TypeScope, List<ProcScope>> extension_methods;

        public SymScope() { }

        public SymScope(SymInfo si, SymScope topScope)
        {
            this.si = si;
            //si.describe = "unit "+si.name;
            si.IsUnitNamespace = true;
            this.topScope = topScope;
            //this.ht = new Hashtable(CaseInsensitiveHashCodeProvider.Default,CaseInsensitiveComparer.Default);
            this.used_units = new List<SymScope>();
            members = new List<SymScope>();
        }

        public virtual void Clear()
        {
            if (members != null) members.Clear();
            if (symbol_table != null) symbol_table.Clear();
            if (used_units != null) used_units.Clear();
            
            declaringUnit = null;
            topScope = null;
        }

        IList<Position> IBaseScope.Regions
        {
            get
            {
                return regions;
            }
        }

        IBaseScope[] IBaseScope.Members
        {
            get
            {
                if (members != null)
                    return members.ToArray();
                return null;
            }
        }

        public virtual ScopeKind Kind
        {
            get
            {
                return ScopeKind.None;
            }
        }

        public access_modifer AccessModifier
        {
            get
            {
                return acc_mod;
            }
        }

        public virtual SymInfo SymbolInfo
        {
            get
            {
                return si;
            }
        }

        public virtual PascalABCCompiler.Parsers.SymbolKind ElemKind
        {
            get
            {
                switch (si.kind)
                {
                    case SymbolKind.Block: return PascalABCCompiler.Parsers.SymbolKind.Block;
                    case SymbolKind.Class: return PascalABCCompiler.Parsers.SymbolKind.Class;
                    case SymbolKind.Constant: return PascalABCCompiler.Parsers.SymbolKind.Constant;
                    case SymbolKind.Delegate: return PascalABCCompiler.Parsers.SymbolKind.Delegate;
                    case SymbolKind.Enum: return PascalABCCompiler.Parsers.SymbolKind.Enum;
                    case SymbolKind.Event: return PascalABCCompiler.Parsers.SymbolKind.Event;
                    case SymbolKind.Field: return PascalABCCompiler.Parsers.SymbolKind.Field;
                    case SymbolKind.Interface: return PascalABCCompiler.Parsers.SymbolKind.Interface;
                    case SymbolKind.Method: return PascalABCCompiler.Parsers.SymbolKind.Method;
                    case SymbolKind.Parameter: return PascalABCCompiler.Parsers.SymbolKind.Parameter;
                    case SymbolKind.Property: return PascalABCCompiler.Parsers.SymbolKind.Property;
                    case SymbolKind.Struct: return PascalABCCompiler.Parsers.SymbolKind.Struct;
                    case SymbolKind.Type: return PascalABCCompiler.Parsers.SymbolKind.Type;
                    case SymbolKind.Variable: return PascalABCCompiler.Parsers.SymbolKind.Variable;
                }
                return PascalABCCompiler.Parsers.SymbolKind.Block;
            }
        }

        public virtual string Name
        {
            get
            {
                return si.name;
            }
        }

        public virtual string Description
        {
            get
            {
                if (si != null) return si.describe;
                return null;
            }
        }

        public IBaseScope TopScope
        {
            get
            {
                return topScope;
            }
        }

        public virtual IElementScope MakeElementScope()
        {
            return new ElementScope(this);
        }

        public void IncreaseEndLine()
        {
            this.loc.end_line_num += 1;

        }

        public void AddExtensionMethod(string name, ProcScope meth, TypeScope ts)
        {
            if (extension_methods == null)
                extension_methods = new Dictionary<TypeScope, List<ProcScope>>();
            List<ProcScope> meth_list = null;
            if (!extension_methods.TryGetValue(ts, out meth_list))
            {
                meth_list = new List<ProcScope>();
                extension_methods.Add(ts, meth_list);
            }
            meth_list.Add(meth);
        }

        public SymInfo[] GetSymInfosForExtensionMethods(TypeScope ts)
        {
            List<SymInfo> lst = new List<SymInfo>();
            List<ProcScope> meth_list = GetExtensionMethods(ts);
            for (int i = 0; i < meth_list.Count; i++)
                lst.Add(meth_list[i].si);
            return lst.ToArray();
        }

        public List<ProcScope> GetExtensionMethods(string name, TypeScope ts)
        {
            List<ProcScope> meths = GetExtensionMethods(ts);
            List<ProcScope> lst = new List<ProcScope>();
            for (int i = 0; i < meths.Count; i++)
            {
                if (string.Compare(meths[i].si.name, name, true) == 0)
                {
                    lst.Add(meths[i]);
                }
            }
            return lst;
        }

        public List<ProcScope> GetExtensionMethods(TypeScope ts)
        {
            List<ProcScope> lst = new List<ProcScope>();
            List<ProcScope> meths = null;
            TypeScope tmp_ts = ts;
            if (extension_methods != null)
            {
                while (tmp_ts != null)
                {
                    TypeScope tmp_ts2 = tmp_ts;
                    if (tmp_ts is CompiledScope && (tmp_ts as CompiledScope).CompiledType.IsGenericType && !(tmp_ts as CompiledScope).CompiledType.IsGenericTypeDefinition)
                        tmp_ts2 = TypeTable.get_compiled_type(new SymInfo("", SymbolKind.Class, ""), (tmp_ts as CompiledScope).CompiledType.GetGenericTypeDefinition());
                    if (extension_methods.TryGetValue(tmp_ts2, out meths))
                    {
                        lst.AddRange(meths);
                    }
                    else
                    {
                        foreach (TypeScope t in extension_methods.Keys)
                        {
                            if (t.IsEqual(tmp_ts2) || (t is ArrayScope && tmp_ts2.IsArray) || ( tmp_ts2 is ArrayScope && t.IsArray) || (t is TemplateParameterScope || t is UnknownScope))
                            {
                                lst.AddRange(extension_methods[t]);
                            }
                        }
                    }
                    tmp_ts = tmp_ts.baseScope;
                }
                if (ts.implemented_interfaces != null)
                foreach (TypeScope int_ts in ts.implemented_interfaces)
                {
                    TypeScope int_ts2 = int_ts;
                    if (int_ts is CompiledScope && (int_ts as CompiledScope).CompiledType.IsGenericType && !(int_ts as CompiledScope).CompiledType.IsGenericTypeDefinition)
                        int_ts2 = TypeTable.get_compiled_type(new SymInfo("", SymbolKind.Class, ""), (int_ts as CompiledScope).CompiledType.GetGenericTypeDefinition());
                    if (extension_methods.TryGetValue(int_ts2, out meths))
                    {
                        lst.AddRange(meths);
                    }
                    else
                    {
                        foreach (TypeScope t in extension_methods.Keys)
                        {
                            if (t.IsEqual(int_ts2) || (t is ArrayScope && int_ts2.IsArray) || (int_ts2 is ArrayScope && t.IsArray))
                            {
                                lst.AddRange(extension_methods[t]);
                                //break;
                            }
                        }
                        
                    }
                }
            }
            if (this.used_units != null)
                for (int i = 0; i < this.used_units.Count; i++)
                {
                    if (this.used_units[i] != this)
                        lst.AddRange(this.used_units[i].GetExtensionMethods(ts));
                }
            return lst;
        }

        public virtual bool IsAssembliesChanged()
        {
            return false;
        }

        public virtual void InitAssemblies()
        {
        }

        public void AddUsedUnit(SymScope unit)
        {
            used_units.Add(unit);
        }

        public virtual string GetFullName()
        {
            return si.name;
        }

        public virtual string GetDescription()
        {
            if (si != null)
                return si.describe;
            return "";
        }

        public virtual void MakeSynonimDescription()
        {

        }

        public virtual string GetDescriptionWithoutDoc()
        {
            return si.name;
        }

        public Position GetPosition()
        {
            Position pos = new Position();
            if (loc != null)
            {
                pos.file_name = loc.doc.file_name;
                pos.line = loc.begin_line_num;
                pos.column = loc.begin_column_num;
                pos.end_line = loc.end_line_num;
                pos.end_column = loc.end_column_num;
            }
            return pos;
        }

        public Position GetHeaderPosition()
        {
            Position pos = new Position();
            if (head_loc != null)
            {
                pos.file_name = head_loc.doc.file_name;
                pos.line = head_loc.begin_line_num;
                pos.column = head_loc.begin_column_num;
                pos.end_line = head_loc.end_line_num;
                pos.end_column = head_loc.end_column_num;
            }
            return pos;
        }

        public Position GetBodyPosition()
        {
            Position pos = new Position();
            if (body_loc != null)
            {
                pos.file_name = body_loc.doc.file_name;
                pos.line = body_loc.begin_line_num;
                pos.column = body_loc.begin_column_num;
                pos.end_line = body_loc.end_line_num;
                pos.end_column = body_loc.end_column_num;
            }
            return pos;
        }

        bool IBaseScope.IsEqual(IBaseScope scope)
        {
            return this == scope;
        }

        public virtual bool IsEqual(SymScope ts)
        {
            return this == ts;
        }

        public virtual void AddDocumentation(string doc)
        {
            try
            {
                if (!string.IsNullOrEmpty(doc))
                {
                    doc = doc.Trim(' ', '\n', '\t', '\r');
                    if (!string.IsNullOrEmpty(doc))
                    {
                        string[] arr = doc.Split(new string[]{"!!"},StringSplitOptions.None);
                        if (CodeCompletionController.currentLanguageISO == "en")
                        {
                            if (arr.Length > 1)
                                doc = arr[1].Trim();
                            else
                                doc = "";
                        }
                        else
                            doc = arr[0].Trim();
                    }
                    this.documentation = doc;
                    if (doc.Length > 0 && doc[0] == '-')
                    {
                        string s = doc.Substring(1);
                        if (s.Length >= 1 && s[0] == '-')
                        {
                            si.describe = null;
                            si.has_doc = false;
                            si.not_include = true;
                            return;
                        }
                        s = s.TrimStart(' ', '\t');
                        int end_ind = s.IndexOf('\n');
                        if (end_ind != -1)
                        {
                            si.describe = s.Substring(0, end_ind);
                            if (end_ind < s.Length - 1)
                                si.describe += "\n" + AssemblyDocCache.GetNormalHint(s.Substring(end_ind + 1));
                        }
                        else
                            si.describe = s.Substring(0);
                        //si.describe = s.TrimStart(' ','\t');
                    }
                    else if (doc.Length > 0 && doc[0] == '<')
                    {
                        this.documentation = AssemblyDocCache.GetNormalHint(doc);
                        si.describe += "\n" + this.documentation;
                    }
                    else
                    {

                        si.describe += "\n" + doc;
                    }
                    si.has_doc = true;
                }
            }
            catch
            {

            }
        }

        public virtual void ClearNames()
        {
            if (members != null)
                foreach (SymScope ss in members)
                    if (ss is ProcScope) ss.ClearNames();
        }

        public virtual  bool InUsesRange(int line, int column)
        {
            return false;
        }

        //proverka na vhozhdenie v scope
        public bool IsInScope(location loc, int line, int column)
        {
            if (loc == null) return false;
            if (loc.begin_line_num < line && loc.end_line_num > line)
                return true;
            if (loc.begin_line_num > line || loc.end_line_num < line)
                return false;
            if (loc.begin_line_num == line && loc.end_line_num == line)
            {
                if (loc.begin_column_num <= column && loc.end_column_num >= column) return true;
                else return false;
            }
            else if (loc.end_line_num != line && loc.begin_column_num <= column) return true;
            else if (loc.begin_line_num != line && loc.end_column_num >= column) return true;
            return false;
        }

        public virtual SymInfo[] GetNamesAsInBaseClass(ExpressionVisitor ev, bool is_static)
        {
            return new SymInfo[0];
        }

        public virtual bool IsChildScopeOf(SymScope ss)
        {
            return false;
        }

        IBaseScope IBaseScope.FindScopeByLocation(int line, int column)
        {
            return this.FindScopeByLocation(line, column);
        }

        //naiti scope po location
        //algoritm lokalizueztsja scope v glubinu poka ne naidetsja istinnyj scope, soderzhashij mesto gde nahodimsja
        public virtual SymScope FindScopeByLocation(int line, int column)
        {
            SymScope res = null;
            cur_line = line;
            cur_col = column;
            if (members == null) return null;
            //if (loc != null && loc.begin_line_num <= line && loc.end_line_num >= line && loc.begin_column_num <= column && loc.end_column_num >= column)
            if (IsInScope(loc, line, column))
                res = this;
            foreach (SymScope ss in members)
                if (this != ss && ss.loc != null && (loc == null || loc != null && loc.doc != null && ss.loc.doc.file_name == loc.doc.file_name))
                {
                    if (IsInScope(ss.loc, line, column))
                    {
                        res = ss;
                        SymScope tmp = ss.FindScopeByLocation(line, column);
                        if (tmp != null) return res = tmp;
                    }
                    else if (!(ss is CompiledScope))
                    {
                        SymScope tmp = ss.FindScopeByLocation(line, column);
                        if (tmp != null) return tmp;
                    }
                }
            return res;
        }

        //poluchenie vseh imen posle tochki
        public virtual SymInfo[] GetNamesAsInObject(ExpressionVisitor ev)
        {
            List<SymInfo> lst = new List<SymInfo>();
            foreach (SymScope ss in members)
            {
                if (ss != this && !(ss is NamespaceScope) && ss.si.kind != SymbolKind.Namespace && !ss.si.name.Contains("$"))
                {
                    lst.Add(ss.si);
                    if (!ss.si.has_doc)
                        UnitDocCache.AddDescribeToComplete(ss);
                }
            }
            return lst.ToArray();
        }

        protected bool IsAfterDefinition(int def_line, int def_col)
        {
            if (cur_line == -1 || cur_col == -1) return true;
            if (cur_line > def_line) return true;
            if (cur_line == def_line) return (cur_col > def_col);
            return false;
        }

        public virtual SymInfo[] GetNames()
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            List<SymInfo> lst = new List<SymInfo>();
            /*foreach (string s in ht.Keys)
            {
                SymScope sc = ht[s] as SymScope;
                if (sc != this) lst.Add(sc.si);
            }*/
            foreach (SymScope ss in members)
            {
                if (ss != this && !ss.si.name.StartsWith("$"))
                {
                    if (ss.loc != null && loc != null)
                    {
                        if (string.Compare(ss.loc.doc.file_name, loc.doc.file_name, true) == 0)
                        {
                            if (IsAfterDefinition(ss.loc.begin_line_num, ss.loc.begin_column_num))
                            {
                                lst.Add(ss.si);
                            }
                        }
                        else lst.Add(ss.si);
                    }
                    else
                        lst.Add(ss.si);
                    if (!ss.si.has_doc)
                        UnitDocCache.AddDescribeToComplete(ss);
                }
            }
            if (topScope != null) lst.AddRange(topScope.GetNames());
            return lst.ToArray();
        }

        //poluchenie vseh imen vnutri Scope i vo vseh objemlushih scopah
        //ispolzuetsja pri nazhatii ctrl+space
        public virtual SymInfo[] GetNamesInAllTopScopes(bool all_names, ExpressionVisitor ev, bool is_static)
        {
            List<SymInfo> lst = new List<SymInfo>();
            /*foreach (string s in ht.Keys)
            {
                SymScope sc = ht[s] as SymScope;
                lst.Add(sc.si);
            }*/
            foreach (SymScope ss in members)
            {
                if (ss != this && !ss.si.name.StartsWith("$"))
                {
                    if (ss.loc != null && loc != null)
                    {
                        if (string.Compare(ss.loc.doc.file_name, loc.doc.file_name, true) == 0)
                        {
                            if (IsAfterDefinition(ss.loc.begin_line_num, ss.loc.begin_column_num))
                            {
                                lst.Add(ss.si);
                            }
                        }
                        else lst.Add(ss.si);
                    }
                    else
                        lst.Add(ss.si);
                    if (!ss.si.has_doc)
                        UnitDocCache.AddDescribeToComplete(ss);
                }
            }
            if (topScope != null) lst.AddRange(topScope.GetNamesInAllTopScopes(all_names, ev, is_static));
            if (used_units != null && all_names)
                for (int i = 0; i < used_units.Count; i++)
                {
                    lst.AddRange(used_units[i].GetNames());
                }
            return lst.ToArray();
        }

        public virtual SymInfo[] GetNamesAsInObject()
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            List<SymInfo> lst = new List<SymInfo>();
            foreach (SymScope ss in members)
            {
                if (ss != this && !(ss is NamespaceScope) && ss.si.kind != SymbolKind.Namespace) lst.Add(ss.si);
                if (!ss.si.has_doc)
                    UnitDocCache.AddDescribeToComplete(ss);
            }
            return lst.ToArray();
        }

        //poisk v scope IsAfterDefinition proverjaet nahoditsja li opisanie do ispolzovanija
        //eto nuzhno tak kak u nas tablicy vse zapolneny
        protected SymScope internal_find(string name, bool check_for_def)
        {
            if (name.Length > 0 && name[0] == '?')
                name = name.Substring(1);
            if (symbol_table != null)
            {
                object o = symbol_table[name];
                if (o == null)
                    return null;
                SymScope ss = null;
                if (o is SymScope)
                    ss = o as SymScope;
                else
                    if (!CodeCompletionController.CurrentParser.LanguageInformation.CaseSensitive)
                        ss = (o as List<SymScope>)[0];
                    else
                    {
                        List<SymScope> lst = o as List<SymScope>;
                        foreach (SymScope s in lst)
                        {
                            if (s.si.name == name)
                                ss = s;
                        }
                    }
                if (ss == null) return null;
                if (CodeCompletionController.CurrentParser.LanguageInformation.CaseSensitive)
                    if (ss.si.name != name)
                        return null;
                if (ss.loc != null && loc != null && check_for_def && cur_line != -1 && cur_col != -1)
                {
                    if (string.Compare(ss.loc.doc.file_name, loc.doc.file_name, true) == 0 && this != ss)
                    {
                        if (IsAfterDefinition(ss.loc.begin_line_num, ss.loc.begin_column_num))
                        {
                            return ss;
                        }
                        else return null;
                    }
                    else return ss;
                }
                else return ss;
            }
            else
                if (members != null)
                foreach (SymScope ss in members)
                    if (string.Compare(ss.si.name, name, !CodeCompletionController.CurrentParser.LanguageInformation.CaseSensitive) == 0)
                        if (ss.loc != null && loc != null && check_for_def && cur_line != -1 && cur_col != -1)
                        {
                            if (string.Compare(ss.loc.doc.file_name, loc.doc.file_name, !CodeCompletionController.CurrentParser.LanguageInformation.CaseSensitive) == 0 && this != ss)
                            {
                                if (IsAfterDefinition(ss.loc.begin_line_num, ss.loc.begin_column_num))
                                {
                                    return ss;
                                }
                                else return null;
                            }
                            else return ss;
                        }
                        else return ss;
            return null;
        }

        protected List<SymScope> internal_find_overloads(string name, bool check_for_def)
        {
            List<SymScope> names = new List<SymScope>();
            if (symbol_table != null)
            {
                object o = symbol_table[name];
                if (o == null)
                    return names;
                List<SymScope> tmp_names = new List<SymScope>();
                if (o is SymScope)
                    tmp_names.Add(o as SymScope);
                else
                    tmp_names.AddRange(o as List<SymScope>);
                foreach (SymScope ss in tmp_names)
                    if (ss.loc != null && loc != null && check_for_def && cur_line != -1 && cur_col != -1)
                    {
                        if (string.Compare(ss.loc.doc.file_name, loc.doc.file_name, true) == 0 && this != ss)
                        {
                            if (IsAfterDefinition(ss.loc.begin_line_num, ss.loc.begin_column_num))
                            {
                                names.Add(ss);
                            }
                            //else return null;
                        }
                        else names.Add(ss);
                    }
                    else names.Add(ss);
            }
            else if (members != null)
                foreach (SymScope ss in members)
                    if (string.Compare(ss.si.name, name, !CodeCompletionController.CurrentParser.LanguageInformation.CaseSensitive) == 0)
                        if (ss.loc != null && loc != null && check_for_def && cur_line != -1 && cur_col != -1)
                        {
                            if (string.Compare(ss.loc.doc.file_name, loc.doc.file_name, true) == 0 && this != ss)
                            {
                                if (IsAfterDefinition(ss.loc.begin_line_num, ss.loc.begin_column_num))
                                {
                                    names.Add(ss);
                                }
                                //else return null;
                            }
                            else
                                names.Add(ss);
                        }
                        else
                            names.Add(ss);
            return names;
        }

        ITypeScope IBaseScope.GetElementType()
        {
            return this.GetElementType();
        }

        public virtual TypeScope GetElementType()
        {
            return null;
        }

        public virtual void AddName(string name, SymScope sc)
        {
            //ht[name] = sc;
            sc.si.name = name;
            members.Add(sc);
        }

        IBaseScope IBaseScope.FindNameOnlyInType(string name)
        {
            return this.FindNameOnlyInType(name);
        }

        public virtual SymScope FindNameOnlyInType(string name)
        {
            return internal_find(name, false);
        }

        public virtual List<SymScope> FindOverloadNamesOnlyInType(string name)
        {
            return internal_find_overloads(name, false);
        }

        public virtual SymScope FindNameOnlyInThisType(string name)
        {
            return internal_find(name, false);
        }

        public virtual List<SymScope> FindOverloadNames(string name)
        {
            List<SymScope> names = internal_find_overloads(name, true);
            if (topScope != null)
                names.AddRange(topScope.FindOverloadNames(name));
            if (used_units != null)
                for (int i = 0; i < used_units.Count; i++)
                {
                    names.AddRange(used_units[i].FindOverloadNamesOnlyInType(name));
                }
            return names;
        }

        public virtual SymScope FindName(string name)
        {
            SymScope sc = internal_find(name, true);
            if (sc != null) return sc;
            if (topScope != null) return topScope.FindName(name);
            for (int i = 0; i < used_units.Count; i++)
            {
                sc = used_units[i].FindNameOnlyInType(name);
                if (sc != null) return sc;
            }
            //Type t = PascalABCCompiler.NetHelper.NetHelper.FindType(name,unl);
            //if (t != null && !t.IsNested) return new CompiledScope(new SymInfo(t.Name, SymbolKind.Type,t.FullName),t);
            //if (PascalABCCompiler.NetHelper.NetHelper.IsNetNamespace(name)) 
            //	return new NamespaceScope(name);
            return null;
        }

        public virtual ProcScope FindNameOnlyInUses(string name)
        {
            if (used_units != null)
            {
                foreach (SymScope ss in used_units)
                {
                    SymScope tmp = ss.FindNameOnlyInType(name);
                    if (tmp != null && tmp is ProcScope)
                        return tmp as ProcScope;
                }
            }
            return null;
        }

        IBaseScope IBaseScope.FindNameInAnyOrder(string name)
        {
            return this.FindNameInAnyOrder(name);
        }

        public virtual SymScope FindNameInAnyOrder(string name)
        {
            SymScope sc = internal_find(name, false);
            if (sc != null) return sc;
            if (topScope != null) return topScope.FindName(name);
            for (int i = 0; i < used_units.Count; i++)
            {
                sc = used_units[i].FindNameOnlyInType(name);
                if (sc != null) return sc;
            }
            //Type t = PascalABCCompiler.NetHelper.NetHelper.FindType(name,unl);
            //if (t != null && !t.IsNested) return new CompiledScope(new SymInfo(t.Name, SymbolKind.Type,t.FullName),t);
            //if (PascalABCCompiler.NetHelper.NetHelper.IsNetNamespace(name)) 
            //	return new NamespaceScope(name);
            return null;
        }
    }

    public class InterfaceUnitScope : SymScope, IInterfaceUnitScope
    {
        public ImplementationUnitScope impl_scope;
        private List<Assembly> ref_assms;
        public location uses_source_range;

        public InterfaceUnitScope(SymInfo si, SymScope topScope)
            : base(si, topScope)
        {
            UnitDocCache.AddDescribeToComplete(this);
            this.symbol_table = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
            si.describe = this.ToString();
        }

        public override bool InUsesRange(int line, int column)
        {
            if (this.uses_source_range != null)
                return IsInScope(this.uses_source_range, line, this.uses_source_range.end_column_num);
            return false;
        }

        public override void Clear()
        {
            base.Clear();
            if (ref_assms != null) ref_assms.Clear();
        }

        public override bool IsAssembliesChanged()
        {
            if (ref_assms != null)
                foreach (Assembly a in ref_assms)
                    if (PascalABCCompiler.NetHelper.NetHelper.IsAssemblyChanged(a.ManifestModule.ScopeName))
                        return true;
            return false;
        }

        public override void InitAssemblies()
        {
            if (ref_assms != null)
                foreach (Assembly a in ref_assms)
                    PascalABCCompiler.NetHelper.NetHelper.init_namespaces(a);
        }

        public void AddReferencedAssembly(System.Reflection.Assembly assm)
        {
            if (ref_assms == null) ref_assms = new List<Assembly>();
            ref_assms.Add(assm);
            List<Type> lst = PascalABCCompiler.NetHelper.NetHelper.entry_types[assm] as List<Type>;
            Type entry_type = null;
            if (lst != null)
                entry_type = lst[0];
            if (entry_type != null)
            {
                NamespaceScope ns = new NamespaceScope(entry_type.Namespace);
                ns.entry_type = TypeTable.get_compiled_type(new SymInfo("", SymbolKind.Type, ""), entry_type);
                used_units.Add(ns);
            }
        }

        public override void AddName(string name, SymScope sc)
        {
            sc.si.name = name;
            object o = symbol_table[name];
            if (o != null && !name.StartsWith("$"))
            {
                if (o is SymScope)
                {
                    List<SymScope> lst = new List<SymScope>();
                    lst.Add(o as SymScope);
                    lst.Add(sc);
                    symbol_table[name] = lst;
                }
                else
                {
                    (o as List<SymScope>).Add(sc);
                }
            }
            else
                symbol_table[name] = sc;
            members.Add(sc);
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.UnitInterface;
            }
        }

        public override string ToString()
        {
            return CodeCompletionController.CurrentParser.LanguageInformation.GetSimpleDescription(this);
        }
    }

    //interfejsnyj scope
    public class ImplementationUnitScope : SymScope, IImplementationUnitScope
    {
        public location uses_source_range;

        public ImplementationUnitScope(SymInfo si, SymScope topScope)
            : base(si, topScope)
        {

        }

        public override bool InUsesRange(int line, int column)
        {
            if (this.uses_source_range != null)
                return IsInScope(this.uses_source_range, line, this.uses_source_range.end_column_num);
            return false;
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.UnitImplementation;
            }
        }

        //osobyj porjadok poiska
        //ishem v scope, potom v usesah scopa, potom v interfacenom scope (i usesah intefejsnogo scopa)
        public override SymScope FindName(string name)
        {
            SymScope sc = internal_find(name, true);
            if (sc != null) return sc;
            for (int i = 0; i < used_units.Count; i++)
            {
                sc = used_units[i].FindNameOnlyInType(name);
                if (sc != null) return sc;
            }
            if (topScope != null) return topScope.FindName(name);

            //Type t = PascalABCCompiler.NetHelper.NetHelper.FindType(name,unl);
            //if (t != null && !t.IsNested) return new CompiledScope(new SymInfo(t.Name, SymbolKind.Type,t.FullName),t);
            //			if (PascalABCCompiler.NetHelper.NetHelper.IsNetNamespace(name)) 
            //				return new NamespaceScope(name);
            return null;
        }

        public override List<SymScope> FindOverloadNames(string name)
        {
            List<SymScope> names = internal_find_overloads(name, true);
            for (int i = 0; i < used_units.Count; i++)
            {
                names.AddRange(used_units[i].FindOverloadNamesOnlyInType(name));
            }
            if (topScope != null)
                names.AddRange(topScope.FindOverloadNames(name));
            return names;
        }

        public override SymScope FindNameInAnyOrder(string name)
        {
            SymScope sc = internal_find(name, false);
            if (sc != null) return sc;
            for (int i = 0; i < used_units.Count; i++)
            {
                sc = used_units[i].FindNameOnlyInType(name);
                if (sc != null) return sc;
            }
            if (topScope != null) return topScope.FindName(name);

            //Type t = PascalABCCompiler.NetHelper.NetHelper.FindType(name,unl);
            //if (t != null && !t.IsNested) return new CompiledScope(new SymInfo(t.Name, SymbolKind.Type,t.FullName),t);
            //if (PascalABCCompiler.NetHelper.NetHelper.IsNetNamespace(name)) 
            //	return new NamespaceScope(name);
            return null;
        }
    }

    /// <summary> 
    ///element - perem, pole, parametr, svojstvo, konstanta
    ///takzhe obertka nad vsem ostalnym, ispolzuetsja v ExpressionVisitor
    /// </summary>
    public class ElementScope : SymScope, IElementScope
    {
        public SymScope sc;
        public PascalABCCompiler.SyntaxTree.parametr_kind param_kind;
        public object cnst_val;
        public List<TypeScope> indexers;
        public TypeScope elementType;

        public ElementScope() { }
        public ElementScope(SymInfo si, SymScope sc, SymScope topScope)
        {
            this.si = si;
            this.sc = sc;
            this.topScope = topScope;
            MakeDescription();
            //UnitDocCache.AddDescribeToComplete(this);
            //if (sc is ProcScope) si.kind = SymbolKind.Delegate;
            //si.describe = si.name + ": " + sc.ToString();
        }

        public ElementScope(SymInfo si, SymScope sc, object cnst_val, SymScope topScope)
        {
            this.si = si;
            this.sc = sc;
            this.topScope = topScope;
            this.cnst_val = cnst_val;
            MakeDescription();
            //UnitDocCache.AddDescribeToComplete(this);
        }

        public ElementScope(SymScope sc)
        {
            this.si = new SymInfo("", SymbolKind.Type, "");
            this.sc = sc;
            if (sc != null)
                this.acc_mod = sc.acc_mod;
            //UnitDocCache.AddDescribeToComplete(this);
            if (sc is ProcScope)
            {
                MakeDescription();
                si.kind = SymbolKind.Delegate;
            }
        }

        public virtual bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.ElementScope;
            }
        }

        public PascalABCCompiler.SyntaxTree.parametr_kind ParamKind
        {
            get
            {
                return param_kind;
            }
        }

        public IBaseScope Type
        {
            get
            {
                return sc;
            }
        }

        public object ConstantValue
        {
            get
            {
                return cnst_val;
            }
        }

        public virtual ITypeScope ElementType
        {
            get
            {
                return elementType;
            }
        }

        public ITypeScope[] Indexers
        {
            get
            {
                return GetIndexers();
            }
        }

        public virtual bool IsVirtual
        {
            get
            {
                return is_virtual;
            }
        }

        public virtual bool IsOverride
        {
            get
            {
                return is_override;
            }
        }

        public virtual bool IsStatic
        {
            get
            {
                return is_static;
            }
        }

        public virtual bool IsAbstract
        {
            get
            {
                return is_abstract;
            }
        }

        public virtual bool IsReintroduce
        {
            get
            {
                return is_reintroduce;
            }
        }

        public void MakeDescription()
        {
            si.describe = CodeCompletionController.CurrentParser.LanguageInformation.GetDescription(this);
        }

        public virtual void AddIndexer(TypeScope ts)
        {
            if (indexers == null) indexers = new List<TypeScope>();
            indexers.Add(ts);
        }

        public virtual TypeScope[] GetIndexers()
        {
            if (indexers != null && indexers.Count > 0)
                return indexers.ToArray();
            else return new TypeScope[0];
        }

        public override SymScope FindName(string s)
        {
            return sc.FindNameOnlyInType(s);
        }

        public override List<SymScope> FindOverloadNames(string name)
        {
            return sc.FindOverloadNamesOnlyInType(name);
        }

        public override TypeScope GetElementType()
        {
            return sc.GetElementType();
        }

        //		public override SymInfo[] GetNames(ExpressionVisitor ev)
        //		{
        //			return sc.GetNamesAsInObject(ev);
        //		}

        public override SymInfo[] GetNamesAsInObject()
        {
            return sc.GetNamesAsInObject();
        }

        public override SymInfo[] GetNamesAsInObject(ExpressionVisitor ev)
        {
            return sc.GetNamesAsInObject(ev);
        }

        public override SymScope FindNameOnlyInType(string name)
        {
            return sc.FindNameOnlyInType(name);
        }

        public override SymScope FindNameOnlyInThisType(string name)
        {
            return sc.FindNameOnlyInThisType(name);
        }

        public override List<SymScope> FindOverloadNamesOnlyInType(string name)
        {
            return sc.FindOverloadNamesOnlyInType(name);
        }

        public override SymInfo[] GetNames()
        {
            return sc.GetNamesAsInObject();
        }

        public override bool IsEqual(SymScope ts)
        {
            /*ElementScope es = ts as ElementScope;
            if (es == null)
                return false;
            if (this.ElemKind != es.ElemKind || this.param_kind != es.param_kind)
                return false;
            return sc.IsEqual(es.sc);*/
            return this == ts;
        }

        public override string ToString()
        {
            return CodeCompletionController.CurrentParser.LanguageInformation.GetSimpleDescription(this);
        }
    }

    //relizacija procedury delegiruet vse metody svoemu opisaniju
    public class ProcRealization : ProcScope, IProcRealizationScope
    {
        public ProcScope def_proc;
        public SymScope top_mod_scope;

        public ProcRealization(ProcScope def_proc, SymScope top_mod_scope)
        {
            this.def_proc = def_proc;
            this.top_mod_scope = top_mod_scope;
            if (def_proc != null)
                this.topScope = def_proc.topScope;
            this.si = new SymInfo(def_proc.si.name, def_proc.si.kind, def_proc.si.describe);
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.ProcRealization;
            }
        }

        public IProcScope DefProc
        {
            get
            {
                return def_proc;
            }
        }

        public override bool IsVirtual
        {
            get
            {
                return def_proc.is_virtual;
            }
        }

        public override bool IsOverride
        {
            get
            {
                return def_proc.is_override;
            }
        }

        public override bool IsStatic
        {
            get
            {
                return def_proc.is_static;
            }
        }

        public override bool IsAbstract
        {
            get
            {
                return def_proc.is_abstract;
            }
        }

        public override bool IsReintroduce
        {
            get
            {
                return def_proc.is_reintroduce;
            }
        }

        public override IElementScope[] Parameters
        {
            get
            {
                return def_proc.Parameters;
            }
        }

        public override void AddName(string name, SymScope sc)
        {
            //ht[name] = sc;
            def_proc.AddName(name, sc);
        }

        public override bool IsConstructor()
        {
            return def_proc.IsConstructor();
        }

        public override void AddParameter(ElementScope par)
        {
            def_proc.AddParameter(par);
        }

        public override void ClearNames()
        {
            def_proc.ClearNames();
        }

        public override SymScope FindScopeByLocation(int line, int column)
        {
            return def_proc.FindScopeByLocation(line, column);
        }

        public override string GetDescriptionWithoutDoc()
        {
            string s = def_proc.GetDescriptionWithoutDoc();
            if (def_proc.topScope is TypeScope)
            {
                return TypeUtility.GetTopScopeName(def_proc.topScope) + s;
            }
            return s;
        }

        public override SymScope FindName(string name)
        {
            //SymScope sc = ht[name] as SymScope;
            SymScope sc = def_proc.FindNameOnlyInType(name);
            if (sc != null) return sc;
            if (top_mod_scope != null) 
                return top_mod_scope.FindName(name);
            return null;
        }

        public override SymScope FindNameOnlyInType(string name)
        {
            //SymScope sc = ht[name] as SymScope;
            return def_proc.FindNameOnlyInType(name);
        }

        public override SymScope FindNameOnlyInThisType(string name)
        {
            //SymScope sc = ht[name] as SymScope;
            return def_proc.FindNameOnlyInType(name);
        }

        public override SymInfo[] GetNamesAsInObject()
        {
            return def_proc.GetNamesAsInObject();
        }

        public override SymInfo[] GetNamesAsInObject(ExpressionVisitor ev)
        {
            return def_proc.GetNamesAsInObject(ev);
        }

        public override SymInfo[] GetNames()
        {
            List<SymInfo> names = new List<SymInfo>();
            names.AddRange(def_proc.GetNames());
            if (top_mod_scope != null)
                names.AddRange(top_mod_scope.GetNames());
            return names.ToArray();
        }

        public override SymScope FindNameInAnyOrder(string name)
        {
            SymScope sc = def_proc.FindNameInAnyOrder(name);
            if (sc != null) return sc;
            if (top_mod_scope != null) return top_mod_scope.FindNameInAnyOrder(name);
            return null;
        }

        public override int GetParametersCount()
        {
            return def_proc.GetParametersCount();
        }

        public override SymInfo[] GetNamesAsInBaseClass(ExpressionVisitor ev, bool is_static)
        {
            return def_proc.GetNamesAsInBaseClass(ev, is_static);
        }

        //		public override SymInfo[] GetNames(ExpressionVisitor ev)
        //		{
        //			List<SymInfo> names = new List<SymInfo>();
        //			names.AddRange(def_proc.GetNames(ev));
        //			if (top_mod_scope != null)
        //				names.AddRange(top_mod_scope.GetNames(ev));
        //			return names.ToArray();	
        //		}

        public override SymInfo[] GetNamesInAllTopScopes(bool all_names, ExpressionVisitor ev, bool is_static)
        {
            List<SymInfo> names = new List<SymInfo>();
            //names.AddRange(def_proc.GetNamesInAllTopScopes(all_names,ev,is_static));
            names.AddRange(def_proc.GetNames());
            if (top_mod_scope != null)
                names.AddRange(top_mod_scope.GetNamesInAllTopScopes(all_names, ev, is_static));
            return names.ToArray();
        }

        public override List<SymScope> FindOverloadNames(string name)
        {
            return def_proc.FindOverloadNames(name);
        }

        public override List<SymScope> FindOverloadNamesOnlyInType(string name)
        {
            return def_proc.FindOverloadNamesOnlyInType(name);
        }

        public override bool IsEqual(SymScope ts)
        {
            return def_proc.IsEqual(ts);
        }

        public override string ToString()
        {
            return def_proc.ToString();
        }
    }

    public class ProcType : TypeScope, IProcType
    {
        public ProcScope target;

        public ProcType(ProcScope target)
        {
            this.target = target;
            this.si = new SymInfo(this.ToString(), SymbolKind.Delegate, this.ToString());
        }

        IProcScope IProcType.Target
        {
            get
            {
                return target;
            }
        }

        public override PascalABCCompiler.Parsers.SymbolKind ElemKind
        {
            get { return PascalABCCompiler.Parsers.SymbolKind.Delegate; }
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.Delegate;
            }
        }

        public override TypeScope GetInstance(List<TypeScope> gen_args)
        {
            return this;
        }

        public override SymInfo[] GetNames()
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            return null;
        }

        public override SymInfo[] GetNames(ExpressionVisitor ev, PascalABCCompiler.Parsers.KeywordKind keyword, bool called_in_base)
        {
            return null;
        }

        public override string GetDescription()
        {
            return this.ToString();
        }

        public override ProcScope GetConstructor()
        {
            return null;
        }

        public override List<ProcScope> GetConstructors(bool search_in_base)
        {
            return new List<ProcScope>(0);
        }

        public override SymInfo[] GetNamesAsInObject()
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            return new SymInfo[0];
        }

        public override SymInfo[] GetNamesAsInObject(ExpressionVisitor ev)
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            return new SymInfo[0];
        }

        public override SymScope FindName(string name)
        {
            if (topScope != null) return topScope.FindName(name);
            return null;
        }

        public override SymScope FindNameInAnyOrder(string name)
        {
            if (topScope != null) return topScope.FindNameInAnyOrder(name);
            return null;
        }

        public override SymScope FindNameOnlyInType(string name)
        {
            return null;
        }

        public override SymScope FindNameOnlyInThisType(string name)
        {
            return null;
        }

        public override bool IsConvertable(TypeScope ts)
        {
            if (IsEqual(ts))
                return true;
            if (ts is CompiledScope)
                return ts.IsConvertable(this);
            return false;
        }

        public override bool IsEqual(SymScope ts)
        {
            if (ts is NullTypeScope)
                return true;
            if (ts is TypeSynonim)
                return this.IsEqual((ts as TypeSynonim).actType);
            if (ts is ProcType)
                return target.IsParamsEquals((ts as ProcType).target);
            return false;
        }

        public override string ToString()
        {
            return CodeCompletionController.CurrentParser.LanguageInformation.GetDescription(this);
        }
    }

    //procedura-funcija-metod
    public class ProcScope : SymScope, IProcScope
    {
        public string name;
        public List<ElementScope> parameters;
        public TypeScope return_type;
        public ProcScope nextProc;//sled. peregruzhennaja procedura
        public bool is_constructor;
        public bool is_forward = false;
        public bool already_defined = false;//sush. li realizacija
        public ProcRealization proc_realization;//ssylka na realizaciju 
        public List<string> template_parameters;
        public TypeScope declaringType;
        public bool is_extension = false;
        public List<string> generic_params;
        public List<string> generic_args;

        public ProcScope()
        {

        }

        public ProcScope(string name, SymScope topScope)
        {
            this.name = name;
            this.topScope = topScope;
            this.si = new SymInfo(name, SymbolKind.Method, name);
            parameters = new List<ElementScope>();
            members = new List<SymScope>();
            //UnitDocCache.AddDescribeToComplete(this);
            if (topScope is TypeScope) declaringType = topScope as TypeScope;
        }

        public ProcScope(string name, SymScope topScope, bool is_constructor)
        {
            this.name = name;
            this.topScope = topScope;
            this.si = new SymInfo(name, SymbolKind.Method, name);
            parameters = new List<ElementScope>();
            members = new List<SymScope>();
            this.is_constructor = is_constructor;
            //UnitDocCache.AddDescribeToComplete(this);
            if (topScope is TypeScope) declaringType = topScope as TypeScope;
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.Procedure;
            }
        }

        public virtual bool IsVirtual
        {
            get
            {
                return is_virtual;
            }
        }

        public virtual bool IsExtension
        {
            get { return is_extension; }
        }

        public virtual bool IsOverride
        {
            get
            {
                return is_override;
            }
        }

        public virtual bool IsStatic
        {
            get
            {
                return is_static;
            }
        }

        public virtual bool IsAbstract
        {
            get
            {
                return is_abstract;
            }
        }

        public virtual bool IsReintroduce
        {
            get
            {
                return is_reintroduce;
            }
        }

        public virtual IElementScope[] Parameters
        {
            get
            {
                if (parameters != null)
                    return parameters.ToArray();
                return null;
            }
        }

        public virtual string[] TemplateParameters
        {
            get
            {
                if (template_parameters != null)
                    return template_parameters.ToArray();
                return null;
            }
        }

        public virtual IProcScope NextFunction
        {
            get
            {
                return nextProc;
            }
        }

        public virtual ITypeScope ReturnType
        {
            get
            {
                return return_type;
            }
        }

        public IProcRealizationScope Realization
        {
            get
            {
                return proc_realization;
            }
        }

        public override void ClearNames()
        {
            members.Clear();
            //members
        }

        public ProcScope GetInstance(List<TypeScope> gen_args)
        {
            if (this.template_parameters == null || this.template_parameters.Count == 0)
                return this;
            ProcScope instance = new ProcScope(this.name, this.topScope, this.is_constructor);
            instance.parameters = new List<ElementScope>(this.parameters.Count);
            int i = 0;
            foreach (ElementScope parameter in this.parameters)
            {
                if (parameter.sc is TemplateParameterScope || parameter.sc is UnknownScope)
                {
                    int ind = this.template_parameters.IndexOf((parameter.sc as TypeScope).name);
                    ElementScope inst_param = null;
                    if (gen_args.Count > ind && ind != -1)
                        inst_param = new ElementScope(parameter.si, gen_args[ind], parameter.topScope);
                    else
                        inst_param = new ElementScope(parameter.si, parameter.sc, parameter.topScope);
                    instance.parameters.Add(inst_param);
                }
                else
                {
                    ElementScope inst_param = null;
                    if ((parameter.sc as TypeScope).IsGeneric)
                        inst_param = new ElementScope(parameter.si, (parameter.sc as TypeScope).GetInstance(gen_args), parameter.topScope);
                    else
                        inst_param = new ElementScope(parameter.si, parameter.sc, parameter.topScope);
                    instance.parameters.Add(inst_param);
                }
            }
            instance.si = this.si;
            instance.return_type = this.return_type.GetInstance(gen_args);
            return instance;
        }

        public virtual int GetParametersCount()
        {
            if (parameters == null) return 0;
            return parameters.Count;
        }

        public virtual bool IsConstructor()
        {
            return is_constructor;
        }

        public override void MakeSynonimDescription()
        {
            //aliased = true;
            si.describe = CodeCompletionController.CurrentParser.LanguageInformation.GetSynonimDescription(this);
        }

        //zavershenie opisanija, vyzyvaetsja kogda parametry razobrany
        public void Complete()
        {
            if (documentation != null && documentation.Length > 0 && documentation[0] == '-') return;
            this.si.describe = this.ToString();
            if (documentation != null) this.si.describe += "\n" + this.documentation;
        }

        public void AddTemplateParameter(string name)
        {
            if (template_parameters == null) template_parameters = new List<string>();
            template_parameters.Add(name);
        }

        public override void AddName(string name, SymScope sc)
        {
            //ht[name] = sc;
            sc.si.name = name;
            members.Add(sc);
        }

        public virtual void AddParameter(ElementScope par)
        {
            parameters.Add(par);
        }

        public bool IsGeneric()
        {
            return template_parameters != null && template_parameters.Count > 0;
        }

        public override SymScope FindName(string name)
        {
            //SymScope sc = ht[name] as SymScope;
            SymScope sc = internal_find(name, true);
            if (sc != null) return sc;
            if (topScope != null) sc = topScope.FindName(name);
            if (sc != null) return sc;
            if (proc_realization != null && proc_realization.top_mod_scope != null)
                return proc_realization.top_mod_scope.FindName(name);
            return null;
        }

        public override List<SymScope> FindOverloadNames(string name)
        {
            List<SymScope> names = internal_find_overloads(name, true);
            if (topScope != null)
                names.AddRange(topScope.FindOverloadNames(name));
            if (proc_realization != null && proc_realization.top_mod_scope != null)
                names.AddRange(proc_realization.top_mod_scope.FindOverloadNames(name));
            return names;
        }

        public override SymInfo[] GetNames()
        {
            List<SymInfo> lst = new List<SymInfo>();
            /*foreach (string s in ht.Keys)
            {
                SymScope sc = ht[s] as SymScope;
                lst.Add(sc.si);
            }*/
            foreach (SymScope ss in members)
                if (ss != this && !ss.si.name.StartsWith("$"))
                {
                    if (ss.loc != null && loc != null)
                    {
                        if (string.Compare(ss.loc.doc.file_name, loc.doc.file_name, true) == 0)
                        {
                            if (IsAfterDefinition(ss.loc.begin_line_num, ss.loc.begin_column_num))
                            {
                                lst.Add(ss.si);
                            }
                        }
                        else lst.Add(ss.si);
                    }
                    else
                        lst.Add(ss.si);
                    if (!ss.si.has_doc)
                        UnitDocCache.AddDescribeToComplete(ss);
                }
            lst.AddRange(this.topScope.GetNames());
            //if (proc_realization != null) lst.AddRange(proc_realization.top_mod_scope.GetNames());
            return lst.ToArray();
        }

        public override SymInfo[] GetNamesInAllTopScopes(bool all_names, ExpressionVisitor ev, bool is_static)
        {
            List<SymInfo> lst = new List<SymInfo>();
            /*foreach (string s in ht.Keys)
            {
                SymScope sc = ht[s] as SymScope;
                lst.Add(sc.si);
            }*/
            foreach (SymScope ss in members)
                if (ss != this && !ss.si.name.StartsWith("$"))
                {
                    if (ss.loc != null && loc != null)
                    {
                        if (string.Compare(ss.loc.doc.file_name, loc.doc.file_name, true) == 0)
                        {
                            if (IsAfterDefinition(ss.loc.begin_line_num, ss.loc.begin_column_num))
                            {
                                lst.Add(ss.si);
                            }
                        }
                        else lst.Add(ss.si);
                    }
                    else
                        lst.Add(ss.si);
                    if (!ss.si.has_doc)
                        UnitDocCache.AddDescribeToComplete(ss);
                }
            if (topScope != null) lst.AddRange(topScope.GetNamesInAllTopScopes(all_names, ev, this.is_static));
            //if (proc_realization != null && proc_realization.top_mod_scope != null) lst.AddRange(proc_realization.top_mod_scope.GetNamesInAllTopScopes(all_names,ev,is_static));
            return lst.ToArray();
        }

        public virtual bool IsParamsEquals(ProcScope ps)
        {
            try
            {
                if (ps is CompiledMethodScope)
                {
                    return (ps as CompiledMethodScope).IsParamsEquals(this);
                }
                else if (ps is CompiledConstructorScope)
                {
                    return (ps as CompiledConstructorScope).IsParamsEquals(this);
                }
                else
                {
                    if (this.parameters == null || this.parameters.Count == 0)
                        if (ps.parameters == null || ps.parameters.Count == 0)
                        {
                            if (this.return_type == null)
                            {
                                if (ps.return_type == null)
                                    return true;
                                else
                                    return false;
                            }
                            else
                                return this.return_type.IsEqual(ps.return_type);
                        }
                        else
                            return false;
                    if (this.parameters.Count != ps.parameters.Count)
                        return false;
                    for (int i = 0; i < this.parameters.Count; i++)
                    {
                        if (this.parameters[i].param_kind != ps.parameters[i].param_kind)
                            return false;
                        if (!this.parameters[i].sc.IsEqual(ps.parameters[i].sc))
                            return false;
                    }
                    if (this.return_type == null)
                        if (ps.return_type != null)
                            return false;
                        else
                            return true;
                    return this.return_type.IsEqual(ps.return_type);
                }
            }
            catch
            {
                return false;
            }
        }

        private bool equal_parameters_and_result(ProcScope ps)
        {
            if (this.parameters == null)
            {
                if (ps.parameters != null)
                    return false;
                else
                    if (this.return_type == null)
                    {
                        if (ps.return_type == null)
                            return true;
                        else
                            return false;
                    }
                return this.return_type.IsEqual(ps.return_type);
            }
            else
                if (ps.parameters == null)
                    return false;
            if (ps.parameters.Count != this.parameters.Count)
                return false;
            for (int i = 0; i < this.parameters.Count; i++)
            {
                if (!this.parameters[i].sc.IsEqual(ps.parameters[i].sc))
                    return false;
                if (this.parameters[i].param_kind != ps.parameters[i].param_kind)
                    return false;
            }

            if (this.return_type == null)
            {
                if (ps.return_type == null)
                    return true;
                else
                    return false;
            }
            return this.return_type.IsEqual(ps.return_type);
        }

        public override bool IsEqual(SymScope ts)
        {
            /*ProcScope ps = ts as ProcScope;
            if (ps == null) return false;
            if (ps is ProcRealization) ps = (ps as ProcRealization).def_proc;
            if (this == ps) return true;
            if (equal_parameters_and_result(ps))
                return true;
            if (ps.nextProc != null) return IsEqual(ps.nextProc);
            return false;*/
            ProcScope ps = ts as ProcScope;
            if (ps == null) return false;
            if (ps is ProcRealization) ps = (ps as ProcRealization).def_proc;
            if (this == ps) return true;
            if (ps.nextProc != null) return IsEqual(ps.nextProc);
            return false;
        }

        private string simp_descr;

        public override string GetDescriptionWithoutDoc()
        {
            if (simp_descr == null) ToString();
            return simp_descr;
        }

        public override string ToString()
        {
            simp_descr = CodeCompletionController.CurrentParser.LanguageInformation.GetSimpleDescription(this);
            return CodeCompletionController.CurrentParser.LanguageInformation.GetDescription(this);
        }
    }

    //perechislenie
    public class EnumScope : TypeScope, IEnumScope
    {
        private List<string> enum_consts = new List<string>();//konstanty perechislenija

        public EnumScope(SymbolKind kind, SymScope topScope, SymScope baseScope)
            : base(kind, topScope, baseScope)
        {

        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.Enum;
            }
        }

        public string[] EnumConsts
        {
            get
            {
                return enum_consts.ToArray();
            }
        }

        public override TypeScope GetInstance(List<TypeScope> gen_args)
        {
            return this;
        }

        public void AddEnumConstant(string name)
        {
            enum_consts.Add(name);
        }

        public override bool IsConvertable(TypeScope ts)
        {
            if (IsEqual(ts))
                return true;
            return false;
        }

        public override bool IsEqual(SymScope ts)
        {
            EnumScope es = ts as EnumScope;
            if (es == null) return false;
            if (enum_consts.Count != es.enum_consts.Count) return false;
            for (int i = 0; i < es.enum_consts.Count; i++)
                if (string.Compare(enum_consts[i], this.enum_consts[i], true) != 0) return false;
            return true;
        }

        public override SymScope FindName(string name)
        {
            if (topScope != null) return topScope.FindName(name);
            return null;
        }

        public override SymScope FindNameInAnyOrder(string name)
        {
            if (topScope != null) return topScope.FindNameInAnyOrder(name);
            return null;
        }

        public override SymInfo[] GetNames(ExpressionVisitor ev, PascalABCCompiler.Parsers.KeywordKind keyword, bool called_in_base)
        {
            return base.GetNames(ev, keyword, true);
        }

        public override ProcScope GetConstructor()
        {
            return null;
        }

        public override List<ProcScope> GetConstructors(bool search_in_base)
        {
            return new List<ProcScope>(0);
        }

        public override string GetDescription()
        {
            return this.ToString();
        }

        public override string ToString()
        {
            return CodeCompletionController.CurrentParser.LanguageInformation.GetDescription(this);
        }
    }

    //mnozhestvo
    public class SetScope : TypeScope, ISetScope
    {

        public SetScope(TypeScope elementType)
        {
            this.elementType = elementType;
            this.si = new SymInfo(this.ToString(), SymbolKind.Type, this.ToString());
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.Set;
            }
        }

        public override string GetDescription()
        {
            return this.ToString();
        }

        public override TypeScope GetInstance(List<TypeScope> gen_args)
        {
            return this;
        }

        public override SymInfo[] GetNames()
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            return null;
        }

        public override SymInfo[] GetNames(ExpressionVisitor ev, PascalABCCompiler.Parsers.KeywordKind keyword, bool called_in_base)
        {
            return null;
        }

        public override ProcScope GetConstructor()
        {
            return null;
        }

        public override List<ProcScope> GetConstructors(bool search_in_base)
        {
            return new List<ProcScope>(0);
        }
        //		public override SymInfo[] GetNames(ExpressionVisitor ev)
        //		{
        //			//SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
        //			return new SymInfo[0];
        //		}

        public override SymInfo[] GetNamesAsInObject()
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            return new SymInfo[0];
        }

        public override SymInfo[] GetNamesAsInObject(ExpressionVisitor ev)
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            return new SymInfo[0];
        }

        public override SymScope FindName(string name)
        {
            if (topScope != null) return topScope.FindName(name);
            return null;
        }

        public override List<SymScope> FindOverloadNames(string name)
        {
            List<SymScope> names = new List<SymScope>();
            if (topScope != null)
                names.AddRange(topScope.FindOverloadNames(name));
            return names;
        }

        public override SymScope FindNameInAnyOrder(string name)
        {
            if (topScope != null) return topScope.FindNameInAnyOrder(name);
            return null;
        }

        public override SymScope FindNameOnlyInType(string name)
        {
            return null;
        }

        public override List<SymScope> FindOverloadNamesOnlyInType(string name)
        {
            return new List<SymScope>(0);
        }

        public override SymScope FindNameOnlyInThisType(string name)
        {
            return null;
        }

        public override bool IsConvertable(TypeScope ts)
        {
            if (IsEqual(ts))
                return true;
            return false;
        }

        public override bool IsEqual(SymScope ts)
        {
            if (ts is SetScope)
                return elementType.IsEqual((ts as SetScope).elementType);
            return false;
        }

        public override string ToString()
        {
            return CodeCompletionController.CurrentParser.LanguageInformation.GetDescription(this);
        }
    }

    //file [of type]
    public class FileScope : TypeScope, IFileScope
    {

        public FileScope(TypeScope elementType, SymScope topScope)
        {
            this.topScope = topScope;
            if (topScope != null)
            {
                if (elementType != null)
                    this.baseScope = topScope.FindName("TypedFile") as TypeScope;
                else
                    this.baseScope = topScope.FindName("BinaryFile") as TypeScope;
            }
            this.elementType = elementType;
            this.si = new SymInfo(this.ToString(), SymbolKind.Type, this.ToString());
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.File;
            }
        }

        public override string GetDescription()
        {
            return this.ToString();
        }

        public override SymInfo[] GetNames()
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            if (this.baseScope != null)
                return this.baseScope.GetNames();
            return null;
        }

        public override SymInfo[] GetNames(ExpressionVisitor ev, PascalABCCompiler.Parsers.KeywordKind keyword, bool called_in_base)
        {
            if (this.baseScope != null)
                return this.baseScope.GetNames(ev, keyword, called_in_base);
            return null;
        }

        public override List<ProcScope> GetConstructors(bool search_in_base)
        {
            return new List<ProcScope>(0);
        }

        public override ProcScope GetConstructor()
        {
            return null;
        }

        //		public override SymInfo[] GetNames(ExpressionVisitor ev)
        //		{
        //			//SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
        //			return new SymInfo[0];
        //		}

        public override SymInfo[] GetNamesAsInObject()
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            if (this.baseScope != null)
                return this.baseScope.GetNamesAsInObject();
            return new SymInfo[0];
        }

        public override SymInfo[] GetNamesAsInObject(ExpressionVisitor ev)
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            if (this.baseScope != null)
                return this.baseScope.GetNamesAsInObject(ev);
            return new SymInfo[0];
        }
        public override SymScope FindName(string name)
        {
            if (this.baseScope != null)
                return this.baseScope.FindName(name);
            return null;
        }

        public override SymScope FindNameOnlyInType(string name)
        {
            if (this.baseScope != null)
                return this.baseScope.FindNameOnlyInType(name);
            return null;
        }

        public override SymScope FindNameOnlyInThisType(string name)
        {
            if (this.baseScope != null)
                return this.baseScope.FindNameOnlyInThisType(name);
            return null;
        }

        public override SymScope FindNameInAnyOrder(string name)
        {
            if (this.baseScope != null)
                return this.baseScope.FindNameInAnyOrder(name);
            return null;
        }

        public override TypeScope GetInstance(List<TypeScope> gen_args)
        {
            return this;
        }

        public override bool IsConvertable(TypeScope ts)
        {
            if (ts is FileScope)
            {
                if ((ts as FileScope).elementType == null && elementType == null)
                    return true;
                if ((ts as FileScope).elementType != null && elementType != null)
                    return elementType.IsEqual((ts as FileScope).elementType);
                return false;
            }
            else if (ts is TypeSynonim)
                return this.IsConvertable((ts as TypeSynonim).actType);
            return false;
        }

        public override bool IsEqual(SymScope ts)
        {
            if (ts is FileScope)
            {
                if ((ts as FileScope).elementType == null && elementType == null) return true;
                if ((ts as FileScope).elementType != null && elementType != null)
                    return elementType.IsEqual((ts as FileScope).elementType);
                return false;
            }
            return false;
        }

        public override List<SymScope> FindOverloadNames(string name)
        {
            if (this.baseScope != null)
                return baseScope.FindOverloadNames(name);
            return null;
        }

        public override List<SymScope> FindOverloadNamesOnlyInType(string name)
        {
            if (this.baseScope != null)
                return baseScope.FindOverloadNamesOnlyInType(name);
            return null;
        }

        public override SymInfo[] GetNamesAsInBaseClass(ExpressionVisitor ev, bool is_static)
        {
            if (this.baseScope != null)
                return baseScope.GetNamesAsInBaseClass(ev, is_static);
            return null;
        }

        public override string ToString()
        {
            return CodeCompletionController.CurrentParser.LanguageInformation.GetDescription(this);
        }
    }

    public class TemplateParameterScope : TypeScope, ITemplateParameterScope
    {
        private SymScope declScope;

        public TemplateParameterScope(string name, TypeScope baseType, SymScope declScope)
            : base(SymbolKind.Type, null, baseType)
        {
            this.declScope = declScope;
            this.topScope = declScope;
            this.name = name;
            si.describe = name + " in " + declScope.si.name;
        }

    }

    //sinonim type myreal = real;
    public class TypeSynonim : TypeScope, ITypeSynonimScope
    {
        public TypeScope actType;

        public TypeSynonim(SymInfo si, SymScope actType)
            : base(SymbolKind.Type, null, null)
        {
            this.actType = actType as TypeScope;
            this.si = si;
            if (actType.si != null && actType.si.describe != null)
                this.si.describe = CodeCompletionController.CurrentParser.LanguageInformation.GetDescription(this);
            //this.si.describe = "type "+this.si.name + " = "+actType.si.name;
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.TypeSynonim;
            }
        }

        public ITypeScope ActType
        {
            get
            {
                return actType;
            }
        }

        public override SymInfo[] GetNames()
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            return actType.GetNames();
        }

        public override SymInfo[] GetNames(ExpressionVisitor ev, PascalABCCompiler.Parsers.KeywordKind keyword, bool called_in_base)
        {
            return actType.GetNames(ev, keyword, called_in_base);
        }

        public override TypeScope GetElementType()
        {
            return actType.GetElementType();
        }

        public override SymInfo[] GetNamesAsInBaseClass(ExpressionVisitor ev, bool is_static)
        {
            return actType.GetNamesAsInBaseClass(ev, is_static);
        }

        public override SymScope FindName(string name)
        {
            SymScope ss = actType.FindName(name);
            if (ss != null) return ss;
            if (topScope != null) return topScope.FindName(name);
            return null;
        }

        public override SymScope FindNameInAnyOrder(string name)
        {
            SymScope ss = actType.FindNameInAnyOrder(name);
            if (ss != null) return ss;
            if (topScope != null) return topScope.FindNameInAnyOrder(name);
            return null;
        }

        public override SymScope FindNameOnlyInThisType(string name)
        {
            return actType.FindNameOnlyInThisType(name);
        }

        public override SymInfo[] GetNamesAsInObject()
        {
            return actType.GetNamesAsInObject();
        }

        public override TypeScope[] GetIndexers()
        {
            return actType.GetIndexers();
        }

        public override TypeScope GetInstance(List<TypeScope> gen_args)
        {
            TypeScope original_type = actType;
            if (actType.original_type != null)
                original_type = actType.original_type;
            return original_type.GetInstance(gen_args);
        }

        public override void AddIndexer(TypeScope ts)
        {
            actType.AddIndexer(ts);
        }

        public override SymScope FindNameOnlyInType(string name)
        {
            return actType.FindNameOnlyInType(name);
        }

        public override ProcScope GetConstructor()
        {
            return actType.GetConstructor();
        }

        public override List<ProcScope> GetConstructors(bool search_in_base)
        {
            return actType.GetConstructors(search_in_base);
        }

        //		public override SymInfo[] GetNames(ExpressionVisitor ev)
        //		{
        //			return actType.GetNamesAsInObject(ev);
        //		}

        public override SymInfo[] GetNamesAsInObject(ExpressionVisitor ev)
        {
            return actType.GetNamesAsInObject(ev);
        }

        public override SymInfo[] GetNamesInAllTopScopes(bool all_names, ExpressionVisitor ev, bool is_static)
        {
            return actType.GetNamesInAllTopScopes(all_names, ev, is_static);
        }

        public override List<SymScope> FindOverloadNames(string name)
        {
            return actType.FindOverloadNames(name);
        }

        public override List<SymScope> FindOverloadNamesOnlyInType(string name)
        {
            return actType.FindOverloadNamesOnlyInType(name);
        }

        public override bool IsConvertable(TypeScope ts)
        {
            if (ts is TypeSynonim)
                ts = (ts as TypeSynonim).actType;
            if (ts is TypeScope)
                return this.actType.IsConvertable(ts);
            return false;
        }

        public override bool IsEqual(SymScope ts)
        {
            if (ts is TypeSynonim) return string.Compare(this.si.name, ts.si.name, true) == 0;
            if (ts is TypeScope)
                return this.actType.IsEqual(ts as TypeScope);
            return false;
            //if (ts is TypeSynonim) return actType.IsEqual((ts as TypeSynonim).actType);
            //return actType.IsEqual(ts);
        }

        public override string ToString()
        {
            return si.name;
        }
    }

    public class ShortStringScope : TypeScope, IShortStringScope
    {
        private CompiledScope actType;
        private object len;

        public ShortStringScope(CompiledScope actType, object len)
        {
            this.actType = actType;
            this.len = len;
            if (len != null)
            {
                this.si = new SymInfo("string", SymbolKind.Type, "string");
                this.si.describe = CodeCompletionController.CurrentParser.LanguageInformation.GetDescription(this);
            }
            else this.si = actType.si;
        }

        public object Length
        {
            get
            {
                return len;
            }
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.ShortString;
            }
        }

        public override SymInfo[] GetNames()
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            return actType.GetNames();
        }

        public override SymInfo[] GetNames(ExpressionVisitor ev, PascalABCCompiler.Parsers.KeywordKind keyword, bool called_in_base)
        {
            return actType.GetNames(ev, keyword, called_in_base);
        }

        public override ITypeScope[] Indexers
        {
            get { return actType.Indexers; }
        }

        public override ITypeScope ElementType
        {
            get { return actType.ElementType; }
        }

        public override TypeScope GetInstance(List<TypeScope> gen_args)
        {
            return this;
        }

        public override TypeScope GetElementType()
        {
            return actType.GetElementType();
        }

        public override List<SymScope> FindOverloadNames(string name)
        {
            return actType.FindOverloadNames(name);
        }

        public override List<SymScope> FindOverloadNamesOnlyInType(string name)
        {
            return actType.FindOverloadNamesOnlyInType(name);
        }

        public override SymScope FindName(string name)
        {
            SymScope ss = actType.FindName(name);
            if (ss != null) return ss;
            if (topScope != null) return topScope.FindName(name);
            return null;
        }

        public override SymScope FindNameOnlyInThisType(string name)
        {
            return actType.FindNameOnlyInThisType(name);
        }

        public override SymInfo[] GetNamesAsInObject()
        {
            return actType.GetNamesAsInObject();
        }

        public override TypeScope[] GetIndexers()
        {
            return actType.GetIndexers();
        }

        public override void AddIndexer(TypeScope ts)
        {
            actType.AddIndexer(ts);
        }

        public override SymScope FindNameOnlyInType(string name)
        {
            return actType.FindNameOnlyInType(name);
        }

        public override ProcScope GetConstructor()
        {
            return actType.GetConstructor();
        }

        public override List<ProcScope> GetConstructors(bool search_in_base)
        {
            return actType.GetConstructors(search_in_base);
        }

        //		public override SymInfo[] GetNames(ExpressionVisitor ev)
        //		{
        //			return actType.GetNamesAsInObject(ev);
        //		}

        public override SymInfo[] GetNamesAsInObject(ExpressionVisitor ev)
        {
            return actType.GetNamesAsInObject(ev);
        }

        public override SymInfo[] GetNamesInAllTopScopes(bool all_names, ExpressionVisitor ev, bool is_static)
        {
            return actType.GetNamesInAllTopScopes(all_names, ev, is_static);
        }

        public override bool IsEqual(SymScope ts)
        {
            if (ts is TypeSynonim) return actType.IsEqual((ts as TypeSynonim).actType);
            if (ts is ShortStringScope) return this.len == (ts as ShortStringScope).len;
            return false;
        }

        public override bool IsConvertable(TypeScope ts)
        {
            if (ts is TypeSynonim) return actType.IsConvertable((ts as TypeSynonim).actType);
            if (ts is ShortStringScope) return true;
            return false;
        }

        public override string ToString()
        {
            return si.name;
        }
    }

    //massiv (dinamicheskij i staticheskij)
    public class ArrayScope : TypeScope, IArrayScope
    {
        public TypeScope[] indexes;
        public bool is_dynamic_arr = false;
        private bool _is_multi_dyn_arr = false;

        public ArrayScope()
        {
            is_dynamic_arr = true;
            this.si = new SymInfo("$" + this.ToString(), SymbolKind.Type, this.ToString());
        }

        public ArrayScope(TypeScope elementType, TypeScope[] indexes)
        {
            this.elementType = elementType;
            this.indexes = indexes;
            if (indexes == null)
                is_dynamic_arr = true;
            else
            {
                _is_multi_dyn_arr = true;
                foreach (TypeScope ind_ts in indexes)
                {
                    if (ind_ts != null)
                    {
                        _is_multi_dyn_arr = false;
                        break;
                    }
                }
            }
            Type tarr = typeof(Array);
            this.baseScope = TypeTable.get_compiled_type(new SymInfo(tarr.Name, SymbolKind.Type, tarr.FullName), tarr);
            //if (is_dynamic_arr || _is_multi_dyn_arr)
            {
                List<TypeScope> lst = new List<TypeScope>();
                lst.Add(elementType);
                this.implemented_interfaces = new List<TypeScope>();
                this.implemented_interfaces.Add(CompiledScope.get_type_instance(typeof(IEnumerable<>), lst));
            }
            this.si = new SymInfo("$" + this.ToString(), SymbolKind.Type, this.ToString());
            this.members = new List<SymScope>();
        }

        public bool IsMultiDynArray
        {
            get
            {
                return _is_multi_dyn_arr;
            }
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.Array;
            }
        }

        public ITypeScope[] Indexers
        {
            get
            {
                return indexes;
            }
        }

        public bool IsDynamic
        {
            get
            {
                return is_dynamic_arr;
            }
        }

        public override bool IsArray
        {
            get
            {
                return true;
            }
        }

        public override TypeScope[] GetIndexers()
        {
            return indexes;
        }

        public override string GetDescription()
        {
            return this.ToString();
        }

        public override TypeScope GetInstance(List<TypeScope> gen_args)
        {
            if ((elementType is UnknownScope || elementType is TemplateParameterScope) && gen_args.Count > 0)
                return new ArrayScope(gen_args[0], null);
            return this;
        }

        public override SymInfo[] GetNames(ExpressionVisitor ev, PascalABCCompiler.Parsers.KeywordKind keyword, bool called_in_base)
        {
            return new SymInfo[0];
        }

        //u staticheskogo massiva prinimaem, chto net metodov i sv-v, u dinam. est
        public override SymInfo[] GetNamesAsInObject()
        {
            if (is_dynamic_arr || IsMultiDynArray)
                return base.GetNamesAsInObject();
            return new SymInfo[0];
        }

        public override SymInfo[] GetNamesAsInObject(ExpressionVisitor ev)
        {
            if (is_dynamic_arr || IsMultiDynArray)
            {
                List<SymInfo> syms = new List<SymInfo>();
                syms.AddRange(base.GetNamesAsInObject(ev));
                if (implemented_interfaces != null)
                {
                    foreach (TypeScope ts in implemented_interfaces)
                        syms.AddRange(ts.GetNamesAsInObject(ev));
                }
                return syms.ToArray();
            }
            return new SymInfo[0];
        }

        public override List<SymScope> FindOverloadNamesOnlyInType(string name)
        {
            List<SymScope> syms = base.FindOverloadNamesOnlyInType(name);
            if (implemented_interfaces != null)
            {
                foreach (TypeScope ts in implemented_interfaces)
                    syms.AddRange(ts.FindOverloadNamesOnlyInType(name));
            }
            return syms;
        }
        //		public override SymInfo[] GetNames(ExpressionVisitor ev)
        //		{
        //			if (is_dynamic_arr)
        //			return base.GetNames(ev);
        //			return new SymInfo[0];
        //		}

        public override SymScope FindName(string name)
        {
            SymScope sc = null;
            if (baseScope != null && is_dynamic_arr) sc = baseScope.FindNameOnlyInType(name);
            if (sc != null) return sc;
            if (topScope != null) return topScope.FindName(name);
            return null;
        }

        public override List<SymScope> FindOverloadNames(string name)
        {
            List<SymScope> names = new List<SymScope>();
            if (baseScope != null && is_dynamic_arr) names.AddRange(baseScope.FindOverloadNamesOnlyInType(name));
            if (topScope != null)
                names.AddRange(topScope.FindOverloadNames(name));
            return names;
        }

        public override SymScope FindNameInAnyOrder(string name)
        {
            SymScope sc = null;
            if (baseScope != null && is_dynamic_arr) sc = baseScope.FindNameOnlyInType(name);
            if (sc != null) return sc;
            if (topScope != null) return topScope.FindNameInAnyOrder(name);
            return null;
        }

        public override SymScope FindNameOnlyInType(string name)
        {
            if (is_dynamic_arr || IsMultiDynArray)
                return base.FindNameOnlyInType(name);
            return null;
        }

        public override SymScope FindNameOnlyInThisType(string name)
        {
            if (is_dynamic_arr || IsMultiDynArray)
                return base.FindNameOnlyInThisType(name);
            return null;
        }

        public override void AddIndexer(TypeScope ts)
        {

        }

        public override bool IsConvertable(TypeScope ts)
        {
            if (ts is NullTypeScope && is_dynamic_arr)
                return true;
            if (!is_dynamic_arr && !IsMultiDynArray)
                return base.IsConvertable(ts);
            ArrayScope arrs = ts as ArrayScope;
            if (ts is CompiledScope && (ts as CompiledScope).IsArray)
            {
                return this.elementType.IsEqual(ts.GetElementType());
            }
            if (arrs == null || !arrs.is_dynamic_arr && !arrs.IsMultiDynArray)
                if (ts is TypeSynonim) return this.IsConvertable((ts as TypeSynonim).actType);
                else return false;
            if (arrs.elementType == null)
                return true;
            if (!this.elementType.IsEqual(arrs.elementType)) return false;
            if (this.IsMultiDynArray && arrs.IsMultiDynArray)
            {
                return this.indexes.Length == arrs.indexes.Length;
            }
            return true;
            /*if (this.indexes == null && arrs.indexes == null) return true;//?????
            if (this.indexes != null && arrs.indexes != null)
            {
                if (this.indexes.Length != arrs.indexes.Length) return false;
                for (int i=0; i<this.indexes.Length; i++)
                    if (this.indexes[i] == null && arrs.indexes[i] == null)
                        continue;
                    else if (this.indexes[i] == null && arrs.indexes[i] != null || this.indexes[i] != null && arrs.indexes[i] == null)
                        return false;
                    else
                    if (!this.indexes[i].IsEqual(arrs.indexes[i])) return false;
                return true;
            }
            return false;*/
        }

        public override bool IsEqual(SymScope ts)
        {
            if (!is_dynamic_arr) return base.IsEqual(ts);
            if (ts is NullTypeScope && is_dynamic_arr)
                return true;
            ArrayScope arrs = ts as ArrayScope;
            if (arrs == null || !arrs.is_dynamic_arr)
                if (ts is TypeSynonim) return this.IsEqual((ts as TypeSynonim).actType);
                else return false;
            if (arrs.elementType == null) return true;
            if (!this.elementType.IsEqual(arrs.elementType)) return false;
            return true;
            if (this.indexes == null && arrs.indexes == null) return true;//?????
            if (this.indexes != null && arrs.indexes != null)
            {
                if (this.indexes.Length != arrs.indexes.Length) return false;
                for (int i = 0; i < this.indexes.Length; i++)
                    if (this.indexes[i] == null && arrs.indexes[i] == null)
                        continue;
                    else if (this.indexes[i] == null && arrs.indexes[i] != null || this.indexes[i] != null && arrs.indexes[i] == null)
                        return false;
                    else
                        if (!this.indexes[i].IsEqual(arrs.indexes[i])) return false;
                return true;
            }
            return false;
        }

        public override ProcScope GetConstructor()
        {
            return null;
        }

        public override List<ProcScope> GetConstructors(bool search_in_base)
        {
            return new List<ProcScope>(0);
        }

        public override TypeScope GetElementType()
        {
            return elementType;
        }

        public override string ToString()
        {
            return CodeCompletionController.CurrentParser.LanguageInformation.GetDescription(this);
        }
    }

    //diapason
    public class DiapasonScope : TypeScope, IDiapasonScope
    {
        public object left;
        public object right;

        public DiapasonScope(object left, object right)
        {
            this.left = left;
            this.right = right;
            this.si = new SymInfo(this.ToString(), SymbolKind.Type, this.ToString());
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.Diapason;
            }
        }

        public object Left
        {
            get
            {
                return left;
            }
        }

        public object Right
        {
            get
            {
                return right;
            }
        }

        public override TypeScope GetInstance(List<TypeScope> gen_args)
        {
            return this;
        }

        public override SymInfo[] GetNames()
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            return new SymInfo[0];
        }

        public override SymInfo[] GetNames(ExpressionVisitor ev, PascalABCCompiler.Parsers.KeywordKind keyword, bool called_in_base)
        {
            return new SymInfo[0];
        }

        public override ProcScope GetConstructor()
        {
            return null;
        }

        public override List<ProcScope> GetConstructors(bool search_in_base)
        {
            return new List<ProcScope>(0);
        }

        public override SymInfo[] GetNamesAsInObject()
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            return new SymInfo[0];
        }

        //		public override SymInfo[] GetNames(ExpressionVisitor ev)
        //		{
        //			//SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
        //			return new SymInfo[0];
        //		}

        public override SymInfo[] GetNamesAsInObject(ExpressionVisitor ev)
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            return new SymInfo[0];
        }

        public override SymScope FindName(string name)
        {
            if (topScope != null) return topScope.FindName(name);
            return null;
        }

        public override List<SymScope> FindOverloadNames(string name)
        {
            if (topScope != null) return topScope.FindOverloadNames(name);
            return new List<SymScope>(0);
        }

        public override SymScope FindNameInAnyOrder(string name)
        {
            if (topScope != null) return topScope.FindNameInAnyOrder(name);
            return null;
        }

        public override SymScope FindNameOnlyInType(string name)
        {
            return null;
        }

        public override List<SymScope> FindOverloadNamesOnlyInType(string name)
        {
            return new List<SymScope>(0);
        }

        public override SymScope FindNameOnlyInThisType(string name)
        {
            return null;
        }

        public override string GetDescription()
        {
            return this.ToString();
        }

        public override string ToString()
        {
            //return left.ToString() + ".." + right.ToString();
            return CodeCompletionController.CurrentParser.LanguageInformation.GetDescription(this);
        }
    }

    //obsolete
    public class RecordScope : TypeScope
    {
        public RecordScope()
        {
            //ht = new Hashtable();
            baseScope = TypeTable.get_compiled_type(new SymInfo(typeof(ValueType).Name, SymbolKind.Struct, typeof(ValueType).FullName), typeof(ValueType));
            si = new SymInfo("record", SymbolKind.Struct, "record");
            this.members = new List<SymScope>();
        }

        public override string ToString()
        {
            return "record";
        }

    }

    public class NullTypeScope : TypeScope
    {
        public NullTypeScope()
        {

        }

        public override bool IsConvertable(TypeScope ts)
        {
            return ts.IsConvertable(this);
        }

        public override SymScope FindName(string name)
        {
            return null;
        }

        public override SymScope FindNameInAnyOrder(string name)
        {
            return null;
        }

        public override SymScope FindNameOnlyInThisType(string name)
        {
            return null;
        }

        public override SymInfo[] GetNames()
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            return new SymInfo[0];
        }

        public override SymInfo[] GetNames(ExpressionVisitor ev, PascalABCCompiler.Parsers.KeywordKind keyword, bool called_in_base)
        {
            return new SymInfo[0];
        }

        public override ProcScope GetConstructor()
        {
            return null;
        }

        public override SymInfo[] GetNamesAsInObject()
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            return new SymInfo[0];
        }

        //		public override SymInfo[] GetNames(ExpressionVisitor ev)
        //		{
        //			//SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
        //			return new SymInfo[0];
        //		}

        public override SymInfo[] GetNamesAsInObject(ExpressionVisitor ev)
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            return new SymInfo[0];
        }
    }

    //tip-ukazatel
    public class PointerScope : TypeScope, IPointerScope
    {
        public TypeScope ref_type;

        public PointerScope()
        {
            this.si = new SymInfo("", SymbolKind.Type, "");
        }

        public PointerScope(TypeScope ref_type)
        {
            this.ref_type = ref_type;
            this.si = new SymInfo(this.ToString(), SymbolKind.Type, this.ToString());
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.Pointer;
            }
        }

        public override TypeScope GetInstance(List<TypeScope> gen_args)
        {
            return this;
        }

        public override ITypeScope ElementType
        {
            get { return ref_type; }
        }

        public override bool IsConvertable(TypeScope ts)
        {
            if (IsEqual(ts))
                return true;
            return false;
        }

        public override bool IsEqual(SymScope ts)
        {
            if (ts is NullTypeScope)
                return true;
            if (ts is PointerScope)
                if ((ts as PointerScope).ref_type == null)
                    return true;
            return ref_type == (ts as PointerScope).ref_type;
            return base.IsEqual(ts);
        }

        public override string GetDescription()
        {
            return this.ToString();
        }

        public override SymInfo[] GetNames()
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            return new SymInfo[0];
        }

        public override SymInfo[] GetNames(ExpressionVisitor ev, PascalABCCompiler.Parsers.KeywordKind keyword, bool called_in_base)
        {
            return new SymInfo[0];
        }

        public override List<ProcScope> GetConstructors(bool search_in_base)
        {
            return new List<ProcScope>(0);
        }

        public override ProcScope GetConstructor()
        {
            return null;
        }

        public override SymInfo[] GetNamesAsInObject(ExpressionVisitor ev)
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            return new SymInfo[0];
        }

        public override SymScope FindName(string name)
        {
            if (topScope != null) return topScope.FindName(name);
            return null;
        }

        public override List<SymScope> FindOverloadNames(string name)
        {
            return new List<SymScope>(0);
        }

        public override SymScope FindNameInAnyOrder(string name)
        {
            if (topScope != null) return topScope.FindNameInAnyOrder(name);
            return null;
        }

        public override SymScope FindNameOnlyInType(string name)
        {
            return null;
        }

        public override List<SymScope> FindOverloadNamesOnlyInType(string name)
        {
            return new List<SymScope>(0);
        }

        public override SymScope FindNameOnlyInThisType(string name)
        {
            return null;
        }

        public override string ToString()
        {
            return CodeCompletionController.CurrentParser.LanguageInformation.GetDescription(this);
        }
    }

    //opisanie tipa (klassa, zapisi) ot nego est nasledniki. sdelal ih dlja udobstva.
    public class TypeScope : SymScope, ITypeScope
    {
        public SymbolKind kind;
        public TypeScope baseScope;
        public location real_body_loc;
        public string name;
        //public List<SymScope> members;
        public TypeScope elementType;
        public TypeScope original_type;
        private List<TypeScope> indexers;
        public List<TypeScope> implemented_interfaces;
        public List<TypeScope> instances;
        protected List<string> generic_params;
        public bool is_final;
        public bool aliased = false;

        public TypeScope() { }
        public TypeScope(SymbolKind kind, SymScope topScope, SymScope baseScope)
        {
            this.kind = kind;
            this.baseScope = baseScope as TypeScope;
            this.topScope = topScope;
            if (baseScope == null)
            {
                if (CodeCompletionController.CurrentParser.LanguageInformation.IncludeDotNetEntities)
                    switch (kind)
                    {
                        case SymbolKind.Struct: this.baseScope = TypeTable.get_compiled_type(new SymInfo(typeof(ValueType).Name, SymbolKind.Struct, typeof(ValueType).FullName), typeof(ValueType)); break;
                        case SymbolKind.Interface:
                        case SymbolKind.Class: this.baseScope = TypeTable.get_compiled_type(new SymInfo(typeof(object).Name, SymbolKind.Class, typeof(object).FullName), typeof(object)); break;
                        case SymbolKind.Enum: this.baseScope = TypeTable.get_compiled_type(new SymInfo(typeof(Enum).Name, SymbolKind.Enum, typeof(Enum).FullName), typeof(Enum)); break;
                    }
            }

            //this.ht = new Hashtable(CaseInsensitiveHashCodeProvider.Default,CaseInsensitiveComparer.Default);
            this.members = new List<SymScope>();
            this.indexers = new List<TypeScope>();
            this.instances = new List<TypeScope>();
            //this.generic_params = new List<string>();
            si = new SymInfo("type", kind, "type");
            //UnitDocCache.AddDescribeToComplete(this);
            switch (kind)
            {
                case SymbolKind.Struct: si.describe = CodeCompletionController.CurrentParser.LanguageInformation.GetKeyword(PascalABCCompiler.Parsers.SymbolKind.Struct); break;
                case SymbolKind.Class: si.describe = CodeCompletionController.CurrentParser.LanguageInformation.GetKeyword(PascalABCCompiler.Parsers.SymbolKind.Class); break;
                case SymbolKind.Interface: si.describe = CodeCompletionController.CurrentParser.LanguageInformation.GetKeyword(PascalABCCompiler.Parsers.SymbolKind.Interface); break;
                case SymbolKind.Enum: si.describe = CodeCompletionController.CurrentParser.LanguageInformation.GetKeyword(PascalABCCompiler.Parsers.SymbolKind.Enum); break;
            }

        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.Type;
            }
        }

        public bool Aliased
        {
            get
            {
                return aliased;
            }
        }

        public virtual bool IsArray
        {
            get
            {
                return false;
            }
        }

        public virtual ITypeScope[] Indexers
        {
            get
            {
                return this.GetIndexers();
            }
        }

        public virtual bool IsFinal
        {
            get
            {
                return is_final;
            }
        }

        public virtual ITypeScope[] GenericInstances
        {
            get
            {
                return instances.ToArray();
            }
        }

        public virtual string[] TemplateArguments
        {
            get
            {
                if (generic_params != null && generic_params.Count > 0)
                {
                    return generic_params.ToArray();
                }
                return null;
            }
        }

        public IBaseScope BaseType
        {
            get
            {
                return baseScope;
            }
        }

        public virtual ITypeScope ElementType
        {
            get
            {
                return elementType;
            }
        }

        public virtual void AddGenericInstanciation(TypeScope ts)
        {
            instances.Add(ts);
        }

        public void AddDefaultConstructorIfNeed()
        {
            if (members != null)
            {
                bool has_def_constr = false;
                ProcScope last_constr = null;
                //SymScope cnstr_ss = this.FindNameOnlyInThisType("Create");

                foreach (SymScope ss in members)
                    if (ss is ProcScope && (ss as ProcScope).is_constructor)
                    {
                        /*if ((ss as ProcScope).parameters != null && (ss as ProcScope).parameters.Count==0)
                        {
                            has_def_constr = true;
                            break;
                        }
                        else
                        {
                            //last_constr = ss as ProcScope;
                        }*/
                        has_def_constr = true;
                    }
                if (!has_def_constr)
                {
                    ProcScope ps = new ProcScope("Create", this, true);
                    ps.Complete();
                    /*if (cnstr_ss != null && cnstr_ss is ProcScope)
                        //last_constr.nextProc = ps;
                        ps.nextProc = cnstr_ss as ProcScope;
                    //else*/
                    //ps.si.name = this.si.name;
                    members.Insert(0, ps);
                }
            }
        }

        public override bool IsChildScopeOf(SymScope ss)
        {
            if (ss == null || !(ss is TypeScope))
                return false;
            TypeScope ts = this;
            while (ts.baseScope != null)
                if (ts.baseScope == ss)
                    return true;
                else ts = ts.baseScope;
            return false;
        }

        public virtual List<ProcScope> GetOverridableMethods()
        {
            List<ProcScope> procs = new List<ProcScope>();
            if (members != null)
                foreach (SymScope ss in members)
                    if (ss is ProcScope && ss.is_virtual)
                        procs.Add(ss as ProcScope);
            if (baseScope != null)
                procs.AddRange(baseScope.GetOverridableMethods());
            return procs;
        }

        public virtual List<ProcScope> GetAbstractMethods()
        {
            List<ProcScope> procs = new List<ProcScope>();
            if (members != null)
                foreach (SymScope ss in members)
                    if (ss is ProcScope && (ss.is_abstract || this.si.kind == SymbolKind.Interface))
                        procs.Add(ss as ProcScope);
            if (si.kind == SymbolKind.Interface && implemented_interfaces != null)
                foreach (TypeScope t in implemented_interfaces)
                    procs.AddRange(t.GetAbstractMethods());
            return procs;
        }

        private TypeScope internalInstance(TypeScope ts, List<TypeScope> gen_args)
        {
            if (ts is TemplateParameterScope || ts is UnknownScope)
            {
                int ind = this.generic_params.IndexOf(ts.Name);
                if (ind != -1)
                {
                    return gen_args[ind];
                }
                else
                    return ts;
            }
            else if (ts is ArrayScope)
            {
                ArrayScope arr = null;
                if ((ts as ArrayScope).is_dynamic_arr)
                {
                    arr = new ArrayScope(internalInstance(ts.elementType, gen_args), (ts as ArrayScope).indexes);
                    arr.is_dynamic_arr = true;
                }
                else
                    arr = new ArrayScope(internalInstance(ts.elementType, gen_args), (ts as ArrayScope).indexes);
                return arr;
            }
            else if (ts is TypeScope && ts.instances != null && ts.instances.Count > 0)
            {
                return ts.simpleGetInstance(gen_args);
            }
            else
                return ts;
        }

        public virtual bool IsGeneric
        {
            get
            {
                return generic_params != null && generic_params.Count > 0;
            }
        }

        protected virtual TypeScope simpleGetInstance(List<TypeScope> gen_args)
        {
            TypeScope ts = new TypeScope(this.kind, this.topScope, this.baseScope);
            ts.original_type = this;
            ts.loc = this.loc;
            for (int i = 0; i < gen_args.Count; i++)
            {
                ts.AddGenericParameter(gen_args[i].si.name);
                ts.AddGenericInstanciation(gen_args[i]);
            }
            ts.si.name = this.si.name;
            ts.documentation = this.documentation;
            ts.si.describe = ts.GetDescription();
            return ts;
        }

        public virtual TypeScope GetInstance(List<TypeScope> gen_args)
        {
            TypeScope ts = new TypeScope(this.kind, this.topScope, this.baseScope);
            ts.original_type = this;
            ts.loc = this.loc;
            for (int i = 0; i < gen_args.Count; i++)
            {
                ts.AddGenericParameter(gen_args[i].si.name);
                ts.AddGenericInstanciation(gen_args[i]);
            }
            ts.si.name = this.si.name;
            ts.documentation = this.documentation;
            ts.si.describe = ts.GetDescription();
            if (this.elementType != null)
            {
                ts.elementType = internalInstance(this.elementType, gen_args);
            }
            if (this.indexers != null && this.indexers.Count > 0)
            {
                ts.indexers = new List<TypeScope>();
                for (int j = 0; j < this.indexers.Count; j++)
                    ts.indexers.Add(internalInstance(this.indexers[j], gen_args));
            }
            Hashtable procs = new Hashtable();
            for (int i = 0; i < members.Count; i++)
            {
                SymScope ss = members[i];
                ts.members.Add(ss);
                if (ss is ElementScope)
                {
                    ElementScope es = ss as ElementScope;
                    ElementScope new_es = new ElementScope(new SymInfo(es.si.name, es.si.kind, es.si.name), es.sc, ts);
                    ts.members[i] = new_es;
                    new_es.loc = es.loc;
                    new_es.documentation = es.documentation;
                    new_es.si.acc_mod = es.si.acc_mod;
                    new_es.si.has_doc = es.si.has_doc;
                    if (es.indexers != null && es.indexers.Count > 0)
                    {
                        new_es.indexers = new List<TypeScope>();
                        for (int j = 0; j < es.indexers.Count; j++)
                            new_es.indexers.Add(internalInstance(es.indexers[j], gen_args));
                    }
                    if (es.elementType != null)
                    {
                        new_es.elementType = internalInstance(es.elementType, gen_args);
                    }
                    new_es.sc = internalInstance(es.sc as TypeScope, gen_args);
                    new_es.MakeDescription();
                }
                else if (ss is ProcScope)
                {
                    ProcScope ps = ss as ProcScope;
                    ProcScope new_proc = new ProcScope(ps.si.name, ts, ps.is_constructor);
                    procs[ps] = new_proc;
                    new_proc.loc = ps.loc;
                    new_proc.documentation = ps.documentation;
                    new_proc.si.acc_mod = ps.si.acc_mod;
                    new_proc.is_static = ps.is_static;
                    new_proc.is_virtual = ps.is_virtual;
                    new_proc.is_abstract = ps.is_abstract;
                    new_proc.is_override = ps.is_override;
                    new_proc.is_reintroduce = ps.is_reintroduce;
                    ts.members[i] = new_proc;
                    if (ps.parameters != null)
                        for (int j = 0; j < ps.parameters.Count; j++)
                        {
                            ElementScope es = ps.parameters[j];
                            ElementScope es2 = new ElementScope(new SymInfo(es.si.name, es.si.kind, es.si.name), es.sc, new_proc);
                            es2.loc = es.loc;
                            es2.cnst_val = es.cnst_val;
                            es2.si.acc_mod = es.si.acc_mod;
                            es2.param_kind = es.param_kind;
                            es2.sc = internalInstance(es.sc as TypeScope, gen_args);
                            es2.MakeDescription();
                            new_proc.AddParameter(es2);
                        }
                    new_proc.return_type = internalInstance(ps.return_type, gen_args);
                    new_proc.Complete();
                }
            }
            foreach (ProcScope ps in procs.Keys)
            {
                ProcScope new_ps = procs[ps] as ProcScope;
                if (ps.nextProc != null && procs[ps.nextProc] != null)
                {
                    new_ps.nextProc = procs[ps.nextProc] as ProcScope;
                }
            }
            return ts;
        }

        public virtual void ClearInstances()
        {
            if (instances != null)
                instances.Clear();
        }

        public virtual void AddGenericParameter(string name)
        {
            if (generic_params == null) generic_params = new List<string>();
            generic_params.Add(name);
            AddName(name, new TemplateParameterScope(name, TypeTable.obj_type, this));
        }

        public virtual void AddImplementedInterface(TypeScope type)
        {
            if (implemented_interfaces == null) implemented_interfaces = new List<TypeScope>();
            implemented_interfaces.Add(type);
        }

        public override void MakeSynonimDescription()
        {
            aliased = true;
            si.describe = CodeCompletionController.CurrentParser.LanguageInformation.GetSynonimDescription(this);
        }

        public virtual bool IsConvertable(TypeScope ts)
        {
            if (IsEqual(ts))
                return true;
            if (this is UnknownScope && ts is CompiledScope && (ts as CompiledScope).CompiledType.IsGenericParameter
                || ts is UnknownScope && this is CompiledScope && (this as CompiledScope).CompiledType.IsGenericParameter)
                return true;
            TypeScope tmp = this.baseScope;
            while (tmp != null)
                if (tmp.IsEqual(ts))
                    return true;
                else
                    tmp = tmp.baseScope;
            return false;
        }

        //dlja sravnivanija tipov
        public override bool IsEqual(SymScope ts)
        {
            bool eq = this == ts as TypeScope;
            if (ts is NullTypeScope && this.kind == SymbolKind.Class)
                return true;
            if (eq)
                return true;
            TypeScope typ = ts as TypeScope;
            if ((typ is TemplateParameterScope || typ is UnknownScope) && this is TemplateParameterScope)
                return string.Compare(this.si.name, typ.si.name, true) == 0;
            if (this.original_type != null && typ.original_type != null)
            {
                if (this.original_type.IsEqual(typ.original_type) && this.instances.Count == typ.instances.Count)
                {
                    for (int i = 0; i < this.instances.Count; i++)
                    {
                        if (!this.instances[i].IsEqual(typ.instances[i]))
                            return false;
                        if (typ.instances[i] is UnknownScope && this.instances[i] is TemplateParameterScope)
                            typ.instances[i] = this.instances[i];
                    }
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public virtual ProcScope GetConstructor()
        {
            foreach (SymScope ss in members)
                if (ss is ProcScope && (ss as ProcScope).IsConstructor()) return ss as ProcScope;
            if (baseScope != null)
                return baseScope.GetConstructor();
            return null;
        }

        IProcScope ITypeScope.GetConstructor()
        {
            return this.GetConstructor();
        }

        public virtual List<ProcScope> GetConstructors(bool search_in_base)
        {
            List<ProcScope> constrs = new List<ProcScope>();
            bool must_inherite = true;
            foreach (SymScope ss in members)
                if (ss is ProcScope && (ss as ProcScope).IsConstructor())
                {
                    if (ss.loc != null)
                        must_inherite = false;
                    constrs.Add(ss as ProcScope);
                }
            if (baseScope != null && (must_inherite || search_in_base))
            {
                List<ProcScope> lst = baseScope.GetConstructors(true);
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].parameters != null && lst[i].parameters.Count > 0)
                        constrs.Add(lst[i]);
                }
            }
            return constrs;
        }

        //opisanie, vysvechivaetsja v zheltkom okoshke
        public override string GetDescription()
        {
            return CodeCompletionController.CurrentParser.LanguageInformation.GetDescription(this);
        }

        //dubl?????
        public override SymInfo[] GetNames()
        {
            List<SymInfo> lst = new List<SymInfo>();
            /*foreach (string s in ht.Keys)
            {
                SymScope sc = ht[s] as SymScope;
                lst.Add(sc.si);
            }*/
            foreach (SymScope ss in members)
            {
                if (!ss.si.name.StartsWith("$"))
                {
                    lst.Add(ss.si);
                    if (!ss.si.has_doc)
                        UnitDocCache.AddDescribeToComplete(ss);
                }
            }
            if (baseScope != null)
                lst.AddRange(baseScope.GetNames());
            /*if (topScope != null)
                lst.AddRange(topScope.GetNames());*/
            return lst.ToArray();
        }

        //eto 
        public override SymInfo[] GetNamesAsInBaseClass(ExpressionVisitor ev, bool is_static)
        {
            List<SymInfo> lst = new List<SymInfo>();
            foreach (SymScope ss in members)
            {
                if (ss is ProcScope && (ss as ProcScope).IsConstructor())
                    continue;
                if (!ss.si.name.StartsWith("$"))
                {
                    if (ss.si.acc_mod == access_modifer.private_modifer)
                    {
                        if (ev.CheckPrivateForBaseAccess(ev.entry_scope, this))
                            if (!is_static) lst.Add(ss.si);
                            else if (ss.is_static) lst.Add(ss.si);
                    }
                    else if (ss.si.acc_mod == access_modifer.protected_modifer)
                    {
                        if (ev.CheckForBaseAccess(ev.entry_scope, this))
                            if (!is_static) lst.Add(ss.si);
                            else if (ss.is_static) lst.Add(ss.si);
                    }
                    else
                        if (!is_static) lst.Add(ss.si);
                        else if (ss.is_static) lst.Add(ss.si);
                    if (!ss.si.has_doc)
                        UnitDocCache.AddDescribeToComplete(ss);
                }
            }
            if (baseScope != null)
                lst.AddRange(baseScope.GetNamesAsInBaseClass(ev, is_static));
            return lst.ToArray();
        }

        public override SymInfo[] GetNamesAsInObject()
        {
            //SortedDictionary<string,SymInfo> dict = new SortedDictionary<string,SymInfo>();
            List<SymInfo> lst = new List<SymInfo>();
            /*foreach (string s in ht.Keys)
            {
                SymScope sc = ht[s] as SymScope;
                lst.Add(sc.si);
            }*/
            foreach (SymScope ss in members)
            {
                if (!ss.si.name.StartsWith("$") && !ss.is_static)
                    if (!(ss is ProcScope) && !(ss is TemplateParameterScope))
                    {
                        lst.Add(ss.si);
                        if (!ss.si.has_doc)
                            UnitDocCache.AddDescribeToComplete(ss);
                    }
                    else if (!(ss as ProcScope).IsConstructor())
                    {
                        lst.Add(ss.si);
                        if (!ss.si.has_doc)
                            UnitDocCache.AddDescribeToComplete(ss);
                    }
            }
            if (baseScope != null) lst.AddRange(baseScope.GetNamesAsInObject());
            return lst.ToArray();
        }

        public override TypeScope GetElementType()
        {
            if (elementType != null) return elementType;
            if (baseScope != null) return baseScope.GetElementType();
            return null;
        }

        //esli naprimer nazhali ctrl-probel(all_name = treu) ili shift-probel (all_names = false)
        //visitor vsegda nuzhen tak kak hranit scope, gde my nazhali
        public override SymInfo[] GetNamesInAllTopScopes(bool all_names, ExpressionVisitor ev, bool is_static)
        {
            List<SymInfo> lst = new List<SymInfo>();
            foreach (SymScope ss in members)
            {
                if (ss is ProcScope && (ss as ProcScope).IsConstructor())
                    continue;
                if (!ss.si.name.StartsWith("$"))
                {
                    if (ss.si.acc_mod == access_modifer.private_modifer)
                    {
                        if (ev.CheckPrivateForBaseAccess(ev.entry_scope, this))
                            if (!is_static) lst.Add(ss.si);
                            else if (ss.is_static) lst.Add(ss.si);
                    }
                    else if (ss.si.acc_mod == access_modifer.protected_modifer)
                    {
                        if (ev.CheckForBaseAccess(ev.entry_scope, this))
                            if (!is_static) lst.Add(ss.si);
                            else if (ss.is_static) lst.Add(ss.si);
                    }
                    else
                        if (!is_static) lst.Add(ss.si);
                        else if (ss.is_static) lst.Add(ss.si);
                    if (!ss.si.has_doc)
                        UnitDocCache.AddDescribeToComplete(ss);
                }
            }
            if (baseScope != null)
                lst.AddRange(baseScope.GetNamesAsInBaseClass(ev, is_static));
            if (topScope != null)
                lst.AddRange(topScope.GetNamesInAllTopScopes(all_names, ev, is_static));

            return lst.ToArray();
        }

        //poluchit imena s klassa s kluchevym slovom
        //vyzyvaetsja, kogda procedure TClass. tut vse ekzemplarnye i staticheskie
        public virtual SymInfo[] GetNames(ExpressionVisitor ev, PascalABCCompiler.Parsers.KeywordKind keyword, bool called_in_base)
        {
            List<SymInfo> lst = new List<SymInfo>();
            foreach (SymScope ss in members)
            {
                if (!ss.si.name.StartsWith("$"))
                {
                    if (keyword != PascalABCCompiler.Parsers.KeywordKind.Function && keyword != PascalABCCompiler.Parsers.KeywordKind.Constructor && keyword != PascalABCCompiler.Parsers.KeywordKind.Destructor/*!(ev.entry_scope is InterfaceUnitScope) && !(ev.entry_scope is ImplementationUnitScope)*/)
                    {
                        if (ss.si.acc_mod == access_modifer.private_modifer)
                        {
                            if (ss.is_static && ev.CheckPrivateForBaseAccess(ev.entry_scope, this))
                                lst.Add(ss.si);
                        }
                        else if (ss.si.acc_mod == access_modifer.protected_modifer)
                        {
                            if (ss.is_static && ev.CheckForBaseAccess(ev.entry_scope, this))
                                lst.Add(ss.si);
                        }
                        else
                            if (ss.is_static)
                                lst.Add(ss.si);
                            else if ((ss is ProcScope) && (ss as ProcScope).IsConstructor())
                                if (!((ss as ProcScope).parameters == null || (ss as ProcScope).parameters.Count == 0) || !called_in_base)
                                    lst.Add(ss.si);
                    }
                    else
                    {
                        if (ss is ProcScope && !(ss as ProcScope).already_defined)
                        {
                            if (keyword == PascalABCCompiler.Parsers.KeywordKind.Function || keyword == PascalABCCompiler.Parsers.KeywordKind.Destructor)
                                lst.Add(ss.si);
                            else if ((ss as ProcScope).IsConstructor())
                                lst.Add(ss.si);
                        }
                    }
                    if (!ss.si.has_doc)
                        UnitDocCache.AddDescribeToComplete(ss);
                }
            }

            if (baseScope != null && keyword != PascalABCCompiler.Parsers.KeywordKind.Constructor && keyword != PascalABCCompiler.Parsers.KeywordKind.Destructor)
                lst.AddRange(baseScope.GetNames(ev, keyword, true));
            /*if (topScope != null)
                lst.AddRange(topScope.GetNames());*/
            return lst.ToArray();
        }

        //poluchit vse imena kak po tochke iz objektnoj peremennoj, sootv. ekzemplarnye chleny klassa i nadklassov
        public override SymInfo[] GetNamesAsInObject(ExpressionVisitor ev)
        {
            //if (original_type != null)
            //	return original_type.GetNamesAsInObject(ev);
            List<SymInfo> lst = new List<SymInfo>();
            foreach (SymScope ss in members)
            {
                if (ss is ProcScope && (ss as ProcScope).IsConstructor()) continue;
                if (!ss.si.name.StartsWith("$") && !ss.is_static && !(ss is TemplateParameterScope))
                {
                    if (ss.si.acc_mod == access_modifer.private_modifer)
                    {
                        if (ev.CheckPrivateForBaseAccess(ev.entry_scope, this))
                            lst.Add(ss.si);
                    }
                    else if (ss.si.acc_mod == access_modifer.protected_modifer)
                    {
                        if (ev.CheckForBaseAccess(ev.entry_scope, this))
                            lst.Add(ss.si);
                    }
                    else
                        lst.Add(ss.si);
                    if (!ss.si.has_doc)
                        UnitDocCache.AddDescribeToComplete(ss);
                }
            }
            if (baseScope != null)
            {
                lst.AddRange(baseScope.GetNamesAsInObject(ev));
            }
            if (implemented_interfaces != null)
                foreach (TypeScope ts in implemented_interfaces)
                    lst.AddRange(ts.GetNamesAsInObject(ev));
            /*if (topScope != null)
                lst.AddRange(topScope.GetNames());*/
            return lst.ToArray();
        }

        public virtual TypeScope[] GetIndexers()
        {
            if (indexers.Count > 0)
                return indexers.ToArray();
            if (baseScope != null) return baseScope.GetIndexers();
            else return indexers.ToArray();
        }

        public virtual void AddIndexer(TypeScope ts)
        {
            indexers.Add(ts);
        }

        //poisk v classe, nadklassah i scope v kotorom klass opisan a takzhe vo vseh uses
        public override SymScope FindName(string name)
        {
            //SymScope sc = ht[name] as SymScope;
            SymScope sc = internal_find(name, true);
            if (sc != null) return sc;
            if (baseScope != null) sc = baseScope.FindNameOnlyInType(name);
            if (sc != null) return sc;
            if (topScope != null) return topScope.FindName(name);
            return null;
        }

        public override List<SymScope> FindOverloadNames(string name)
        {
            List<SymScope> names = internal_find_overloads(name, true);
            if (baseScope != null)
                names.AddRange(baseScope.FindOverloadNamesOnlyInType(name));
            if (topScope != null)
                names.AddRange(topScope.FindOverloadNames(name));
            return names;
        }

        public override SymScope FindNameInAnyOrder(string name)
        {
            //SymScope sc = ht[name] as SymScope;
            SymScope sc = internal_find(name, false);
            if (sc != null) return sc;
            if (baseScope != null) sc = baseScope.FindNameOnlyInType(name);
            if (sc != null) return sc;
            if (topScope != null) return topScope.FindNameInAnyOrder(name);
            return null;
        }

        //poisk tolko v etom klasse
        public override SymScope FindNameOnlyInThisType(string name)
        {
            SymScope sc = null;
            if (original_type != null)
                return original_type.FindNameOnlyInThisType(name);
            if (name != null) sc = internal_find(name, false);
            //if (sc == null && topScope != null) return  topScope.FindNameOnlyInThisType(name);
            return sc;
        }

        //poisk tolko v klasse i nadklassah
        public override SymScope FindNameOnlyInType(string name)
        {
            SymScope sc = null;
            if (original_type != null)
                return original_type.FindNameOnlyInType(name);
            if (name != null) sc = internal_find(name, false);//ht[name] as SymScope;
            if (sc == null && baseScope != null)
                sc = baseScope.FindNameOnlyInType(name);
            if (sc == null && implemented_interfaces != null)
                foreach (TypeScope ts in implemented_interfaces)
                {
                    sc = ts.FindNameOnlyInType(name);
                    if (sc != null)
                        break;
                }
            return sc;
        }

        public override List<SymScope> FindOverloadNamesOnlyInType(string name)
        {
            List<SymScope> names = internal_find_overloads(name, false);
            if (baseScope != null)
                names.AddRange(baseScope.FindOverloadNamesOnlyInType(name));
            return names;
        }

        public override void AddName(string name, SymScope sc)
        {
            /*if (name != null)
            ht[name] = sc;*/
            sc.si.name = name;
            if (members == null)
                members = new List<SymScope>();
            members.Add(sc);
        }

        public override string ToString()
        {
            return si.name;
        }
    }

    //	public class GenericInstanceScope : TypeScope, IGenericInstanceScope
    //	{
    //		public TypeScope actType;
    //		public List<TypeScope> gen_args;
    //		
    //		public GenericInstanceScope(TypeScope actType, List<TypeScope> gen_args)
    //		{
    //			this.actType = actType;
    //			this.gen_args = gen_args;
    //			CopyType();
    //		}
    //		
    //		private void CopyType()
    //		{
    //			actType = actType.GetInstance(List<TypeScope> gen_args);
    //		}
    //		
    //	}

    public class NamespaceTypeScope : SymScope, INamespaceTypeScope
    {
        private CompiledScope entry_type;

        public NamespaceTypeScope(CompiledScope entry_type)
        {
            this.entry_type = entry_type;
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.NamespaceTypeScope;
            }
        }

        public override SymScope FindName(string s)
        {
            return entry_type.FindName(s);
        }

        public override List<SymScope> FindOverloadNames(string name)
        {
            return entry_type.FindOverloadNames(name);
        }

        public override List<SymScope> FindOverloadNamesOnlyInType(string name)
        {
            return entry_type.FindOverloadNamesOnlyInType(name);
        }

        public override SymScope FindNameOnlyInType(string name)
        {
            return entry_type.FindNameOnlyInType(name);
        }

        public override SymScope FindNameInAnyOrder(string name)
        {
            return entry_type.FindName(name);
        }

        public override SymInfo[] GetNames()
        {
            return entry_type.GetStaticNames();
        }
    }

    public class NamespaceScope : SymScope, INamespaceScope
    {
        public string name;
        public CompiledScope entry_type;

        public NamespaceScope(string name)
        {
            this.name = name;
            this.si = new SymInfo(name, SymbolKind.Namespace, name);
            this.si.describe = CodeCompletionController.CurrentParser.LanguageInformation.GetDescription(this);
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.Namespace;
            }
        }

        public SymInfo[] GetNames(ExpressionVisitor ev, PascalABCCompiler.Parsers.KeywordKind keyword)
        {
            List<SymInfo> syms = new List<SymInfo>();
            Type[] types = PascalABCCompiler.NetHelper.NetHelper.FindTypesInNamespace(name);
            string[] ns = PascalABCCompiler.NetHelper.NetHelper.FindSubNamespaces(name);

            Hashtable ht = new Hashtable();
            if (keyword != PascalABCCompiler.Parsers.KeywordKind.Uses)
                if (types != null)
                    foreach (Type t in types)
                    {
                        if (!t.IsNotPublic && !t.IsSpecialName && t.IsVisible && !t.Name.Contains("$") && !t.IsNested)
                        {
                            if (t.BaseType == typeof(MulticastDelegate))
                                //syms.Add(new CompiledScope(new SymInfo(TypeUtility.GetShortTypeName(t), SymbolKind.Delegate, "delegate "+TypeUtility.GetTypeName(t) + "\n" + AssemblyDocCache.GetDocumentation(t)),t));
                                syms.Add(TypeTable.get_compiled_type(new SymInfo(null, SymbolKind.Delegate, null), t).si);
                            else
                                if (t.IsClass)
                                    //syms.Add(new SymInfo(TypeUtility.GetShortTypeName(t), SymbolKind.Type, "class "+TypeUtility.GetTypeName(t)+ "\n" +AssemblyDocCache.GetDocumentation(t)));
                                    syms.Add(TypeTable.get_compiled_type(new SymInfo(null, SymbolKind.Class, null), t).si);
                                else if (t.IsInterface)
                                    //syms.Add(new SymInfo(TypeUtility.GetShortTypeName(t), SymbolKind.Interface, "interface "+TypeUtility.GetTypeName(t)+ "\n" +AssemblyDocCache.GetDocumentation(t)));
                                    syms.Add(TypeTable.get_compiled_type(new SymInfo(null, SymbolKind.Interface, null), t).si);
                                else if (t.IsEnum)
                                    //syms.Add(new SymInfo(TypeUtility.GetShortTypeName(t), SymbolKind.Enum, "enum "+TypeUtility.GetTypeName(t)+ "\n" +AssemblyDocCache.GetDocumentation(t)));
                                    syms.Add(TypeTable.get_compiled_type(new SymInfo(null, SymbolKind.Enum, null), t).si);
                                else if (t.IsValueType)
                                    //syms.Add(new SymInfo(TypeUtility.GetShortTypeName(t), SymbolKind.Struct, "record "+TypeUtility.GetTypeName(t)+ "\n" +AssemblyDocCache.GetDocumentation(t)));
                                    syms.Add(TypeTable.get_compiled_type(new SymInfo(null, SymbolKind.Struct, null), t).si);
                        }
                    }

            if (ns != null)
                foreach (string s in ns)
                {
                    if (!s.Contains(".") && !s.Contains("$"))
                        syms.Add(new SymInfo(s, SymbolKind.Namespace, ""));
                }
            if (syms.Count != 0) return syms.ToArray();
            return null;
        }

        public override List<SymScope> FindOverloadNames(string name)
        {
            List<SymScope> names = new List<SymScope>(0);
            SymScope ss = FindName(name);
            if (ss != null)
                names.Add(ss);
            return names;
        }

        public override SymScope FindName(string s)
        {
            bool is_ns = PascalABCCompiler.NetHelper.NetHelper.IsNetNamespace(name + "." + s);
            if (is_ns)
            {
                return new NamespaceScope(name + "." + s);
            }
            else
            {
                Type t = PascalABCCompiler.NetHelper.NetHelper.FindType(name + "." + s);
                if (t == null) t = PascalABCCompiler.NetHelper.NetHelper.FindType(name + "." + s + PascalABCCompiler.TreeConverter.compiler_string_consts.generic_params_infix + "1");
                if (t == null) t = PascalABCCompiler.NetHelper.NetHelper.FindType(name + "." + s + PascalABCCompiler.TreeConverter.compiler_string_consts.generic_params_infix + "2");
                if (t != null)
                {
                    return TypeTable.get_compiled_type(new SymInfo(s, SymbolKind.Type, name + "." + s), t);
                }
                else
                {
                    PascalABCCompiler.TreeConverter.SymbolInfo si = null;
                    if (entry_type != null)
                    {
                        t = PascalABCCompiler.NetHelper.NetHelper.FindType(entry_type.ctn.Namespace + "." + s);
                        if (t != null) return TypeTable.get_compiled_type(new SymInfo(s, SymbolKind.Type, entry_type.ctn.Namespace + "." + s), t);
                        else
                        {
                            object[] attrs = entry_type.ctn.GetCustomAttributes(false);
                            for (int j = 0; j < attrs.Length; j++)
                                if (attrs[j].GetType().Name == "$GlobAttr")
                                {
                                    si = PascalABCCompiler.NetHelper.NetHelper.FindName(entry_type.ctn, s);
                                    if (si != null) break;
                                }
                        }
                    }
                    if (si != null)
                        switch (si.sym_info.semantic_node_type)
                        {
                            case semantic_node_type.compiled_function_node:
                                {
                                    CompiledMethodScope cms = new CompiledMethodScope(new SymInfo(s, SymbolKind.Method, s), (si.sym_info as compiled_function_node).method_info, entry_type, true);
                                    si = si.Next;
                                    CompiledMethodScope tmp = cms;
                                    SortedDictionary<int, List<CompiledMethodScope>> meths = new SortedDictionary<int, List<CompiledMethodScope>>();
                                    if (cms.acc_mod != access_modifer.internal_modifer && cms.acc_mod != access_modifer.private_modifer)
                                    {
                                        int par_num = cms.mi.GetParameters().Length;
                                        meths[par_num] = new List<CompiledMethodScope>();
                                        meths[par_num].Add(cms);
                                    }
                                    while (si != null)
                                    {
                                        if (si.access_level != PascalABCCompiler.TreeConverter.access_level.al_internal && si.access_level != PascalABCCompiler.TreeConverter.access_level.al_private && si.sym_info.semantic_node_type == semantic_node_type.compiled_function_node)
                                        {
                                            tmp = new CompiledMethodScope(new SymInfo(s, SymbolKind.Method, s), (si.sym_info as compiled_function_node).method_info, entry_type);
                                            tmp.is_global = true;
                                            //tmp.nextProc = cms;
                                            //cms = tmp;
                                            int par_num = tmp.mi.GetParameters().Length;
                                            if (!meths.ContainsKey(par_num))
                                                meths[par_num] = new List<CompiledMethodScope>();
                                            meths[par_num].Add(tmp);
                                        }
                                        si = si.Next;
                                    }
                                    bool beg = false;
                                    tmp = null;
                                    cms = null;
                                    foreach (List<CompiledMethodScope> lst in meths.Values)
                                    {
                                        foreach (CompiledMethodScope m in lst)
                                        {
                                            if (beg == false)
                                            {
                                                tmp = m;
                                                cms = tmp;
                                                beg = true;
                                            }
                                            else
                                            {
                                                tmp.nextProc = m;
                                                tmp = tmp.nextProc as CompiledMethodScope;
                                            }
                                        }
                                    }
                                    return cms;
                                }
                            case semantic_node_type.compiled_variable_definition:
                                if (si.access_level != PascalABCCompiler.TreeConverter.access_level.al_internal && si.access_level != PascalABCCompiler.TreeConverter.access_level.al_private)
                                {
                                    CompiledFieldScope fld = new CompiledFieldScope(new SymInfo(s, SymbolKind.Field, s), (si.sym_info as compiled_variable_definition).compiled_field, entry_type, true);
                                    return fld;
                                }
                                break;
                            case semantic_node_type.compiled_class_constant_definition:
                                if (si.access_level != PascalABCCompiler.TreeConverter.access_level.al_internal && si.access_level != PascalABCCompiler.TreeConverter.access_level.al_private)
                                {
                                    CompiledFieldScope fld = new CompiledFieldScope(new SymInfo(s, SymbolKind.Constant, s), (si.sym_info as compiled_class_constant_definition).field, entry_type, true);
                                    return fld;
                                }
                                break;
                        }
                }
                return null;
            }
        }

        public override SymScope FindNameOnlyInType(string name)
        {
            return FindName(name);
        }

        public override List<SymScope> FindOverloadNamesOnlyInType(string name)
        {
            return FindOverloadNames(name);
        }

        public override SymInfo[] GetNames()
        {
            List<SymInfo> syms = new List<SymInfo>();
            Type[] types = PascalABCCompiler.NetHelper.NetHelper.FindTypesInNamespace(name);
            string[] ns = PascalABCCompiler.NetHelper.NetHelper.FindSubNamespaces(name);

            Hashtable ht = new Hashtable();
            if (types != null)
                foreach (Type t in types)
                {
                    if (!t.IsNotPublic && !t.IsSpecialName && t.IsVisible && !t.Name.Contains("$"))
                    {
                        if (t.BaseType == typeof(MulticastDelegate))
                            //syms.Add(new CompiledScope(new SymInfo(TypeUtility.GetShortTypeName(t), SymbolKind.Delegate, "delegate "+TypeUtility.GetTypeName(t) + "\n" + AssemblyDocCache.GetDocumentation(t)),t));
                            syms.Add(TypeTable.get_compiled_type(new SymInfo(null, SymbolKind.Delegate, null), t).si);
                        else
                            if (t.IsClass)
                                //syms.Add(new SymInfo(TypeUtility.GetShortTypeName(t), SymbolKind.Class, "class "+TypeUtility.GetTypeName(t)+ "\n" +AssemblyDocCache.GetDocumentation(t)));
                                syms.Add(TypeTable.get_compiled_type(new SymInfo(null, SymbolKind.Class, null), t).si);
                            else if (t.IsInterface)
                                //	syms.Add(new SymInfo(TypeUtility.GetShortTypeName(t), SymbolKind.Interface, "interface "+TypeUtility.GetTypeName(t)+ "\n" +AssemblyDocCache.GetDocumentation(t)));
                                syms.Add(TypeTable.get_compiled_type(new SymInfo(null, SymbolKind.Interface, null), t).si);
                            else if (t.IsEnum)
                                //syms.Add(new SymInfo(TypeUtility.GetShortTypeName(t), SymbolKind.Enum, "enum "+TypeUtility.GetTypeName(t)+ "\n" +AssemblyDocCache.GetDocumentation(t)));
                                syms.Add(TypeTable.get_compiled_type(new SymInfo(null, SymbolKind.Enum, null), t).si);
                            else if (t.IsValueType)
                                //syms.Add(new SymInfo(TypeUtility.GetShortTypeName(t), SymbolKind.Struct, "record "+TypeUtility.GetTypeName(t)+ "\n" +AssemblyDocCache.GetDocumentation(t)));
                                syms.Add(TypeTable.get_compiled_type(new SymInfo(null, SymbolKind.Struct, null), t).si);
                    }
                }

            if (ns != null)
                foreach (string s in ns)
                {
                    syms.Add(new SymInfo(s, SymbolKind.Namespace, ""));
                }
            return syms.ToArray();
            //return null;
        }

        public override SymInfo[] GetNamesAsInObject()
        {
            List<SymInfo> syms = new List<SymInfo>();
            Type[] types = PascalABCCompiler.NetHelper.NetHelper.FindTypesInNamespace(name);
            string[] ns = PascalABCCompiler.NetHelper.NetHelper.FindSubNamespaces(name);

            Hashtable ht = new Hashtable();
            if (types != null)
                foreach (Type t in types)
                {
                    if (!t.IsNotPublic && !t.IsSpecialName && t.IsVisible && !t.Name.Contains("$"))
                    {
                        if (t.BaseType == typeof(MulticastDelegate))
                            //syms.Add(new CompiledScope(new SymInfo(TypeUtility.GetShortTypeName(t), SymbolKind.Delegate, "delegate "+TypeUtility.GetTypeName(t) + "\n" + AssemblyDocCache.GetDocumentation(t)),t));
                            syms.Add(TypeTable.get_compiled_type(new SymInfo(null, SymbolKind.Delegate, null), t).si);
                        else
                            if (t.IsClass)
                                //syms.Add(new SymInfo(TypeUtility.GetShortTypeName(t), SymbolKind.Class, "class "+TypeUtility.GetTypeName(t)+ "\n" +AssemblyDocCache.GetDocumentation(t)));
                                syms.Add(TypeTable.get_compiled_type(new SymInfo(null, SymbolKind.Class, null), t).si);
                            else if (t.IsInterface)
                                //syms.Add(new SymInfo(TypeUtility.GetShortTypeName(t), SymbolKind.Interface, "interface "+TypeUtility.GetTypeName(t)+ "\n" +AssemblyDocCache.GetDocumentation(t)));
                                syms.Add(TypeTable.get_compiled_type(new SymInfo(null, SymbolKind.Interface, null), t).si);
                            else if (t.IsEnum)
                                //syms.Add(new SymInfo(TypeUtility.GetShortTypeName(t), SymbolKind.Enum, "enum "+TypeUtility.GetTypeName(t)+ "\n" +AssemblyDocCache.GetDocumentation(t)));
                                syms.Add(TypeTable.get_compiled_type(new SymInfo(null, SymbolKind.Enum, null), t).si);
                            else if (t.IsValueType)
                                //syms.Add(new SymInfo(TypeUtility.GetShortTypeName(t), SymbolKind.Struct, "record "+TypeUtility.GetTypeName(t)+ "\n" +AssemblyDocCache.GetDocumentation(t)));
                                syms.Add(TypeTable.get_compiled_type(new SymInfo(null, SymbolKind.Struct, null), t).si);
                    }
                }

            return syms.ToArray();
        }

        public override SymInfo[] GetNamesAsInObject(ExpressionVisitor ev)
        {
            return GetNames();
        }
    }

    public class CompiledScope : TypeScope, ICompiledTypeScope
    {
        public Type ctn;
        private TypeScope[] indices;
        private PropertyInfo default_property;
        private bool is_def_prop_searched = false;

        public CompiledScope(SymInfo si, Type ctn)
        {
            this.ctn = ctn;
            this.si = si;
            this.instances = new List<TypeScope>();
            if (ctn.BaseType != null)
                baseScope = TypeTable.get_compiled_type(new SymInfo(null, SymbolKind.Type, null), ctn.BaseType);
            Type t = ctn.GetElementType();
            //get_default_property();
            if (t != null)
            {
                elementType = TypeTable.get_compiled_type(new SymInfo(t.Name, SymbolKind.Type, t.FullName), t);
                is_def_prop_searched = true;
            }
            if (si.name == null)
                AssemblyDocCache.AddDescribeToComplete(this.si, ctn);
            this.si.name = CodeCompletionController.CurrentParser.LanguageInformation.GetShortTypeName(this);
            this.si.kind = get_kind();
            this.si.describe = GetDescription();
            if (ctn.GetInterfaces().Length > 0)
            {
                this.implemented_interfaces = new List<TypeScope>();
                foreach (Type intf in ctn.GetInterfaces())
                {
                    this.implemented_interfaces.Add(TypeTable.get_compiled_type(new SymInfo(null, SymbolKind.Type, null),intf));
                }
            }
        }

        internal static TypeScope get_type_instance(Type t, List<TypeScope> generic_args)
        {
            if (t.IsGenericParameter)
            {
                if (t.GenericParameterPosition < generic_args.Count)
                    return generic_args[t.GenericParameterPosition];
                else
                    return generic_args[0];
            }
            if (t.IsArray)
                return new ArrayScope(get_type_instance(t.GetElementType(), generic_args), null);
            if (t.ContainsGenericParameters)
            {
                CompiledScope typ = new CompiledScope(new SymInfo(t.Name, SymbolKind.Type, t.Name), t);
                typ.generic_params = new List<string>();
                typ.original_type = TypeTable.get_compiled_type(new SymInfo(t.Name, SymbolKind.Type, t.Name), t);
                Type[] args = t.GetGenericArguments();
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].IsGenericParameter)
                    {
                        if (args[i].GenericParameterPosition < generic_args.Count)
                        {
                            typ.generic_params.Add(generic_args[args[i].GenericParameterPosition].si.name);
                            typ.AddGenericInstanciation(generic_args[args[i].GenericParameterPosition]);
                        }
                        else if (i<generic_args.Count)
                        {
                            typ.generic_params.Add(generic_args[i].si.name);
                            typ.AddGenericInstanciation(generic_args[i]);
                        }
                    }
                    else
                    {
                        TypeScope ts = get_type_instance(args[i], generic_args);
                        typ.generic_params.Add(ts.si.name);
                        typ.AddGenericInstanciation(ts);
                    }
                }
                typ.si.describe = typ.GetDescription();
                return typ;
            }
            return TypeTable.get_compiled_type(new SymInfo(t.Name, SymbolKind.Type, t.Name), t);
        }

        private SymbolKind get_kind()
        {
            if (ctn.BaseType == typeof(MulticastDelegate)) return SymbolKind.Delegate;
            if (ctn.IsClass) return SymbolKind.Class;
            if (ctn.IsInterface) return SymbolKind.Interface;
            if (ctn.IsEnum) return SymbolKind.Enum;
            if (ctn.IsValueType) return SymbolKind.Struct;
            return SymbolKind.Type;
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.CompiledType;
            }
        }

        public override bool IsArray
        {
            get
            {
                return ctn.IsArray;
            }
        }

        public override PascalABCCompiler.Parsers.SymbolKind ElemKind
        {
            get
            {
                if (TypeUtility.IsStandType(ctn)) return PascalABCCompiler.Parsers.SymbolKind.Type;
                if (ctn.IsClass) return PascalABCCompiler.Parsers.SymbolKind.Class;
                if (ctn.IsEnum) return PascalABCCompiler.Parsers.SymbolKind.Enum;
                if (ctn.IsValueType) return PascalABCCompiler.Parsers.SymbolKind.Struct;
                if (ctn.IsInterface) return PascalABCCompiler.Parsers.SymbolKind.Interface;
                return PascalABCCompiler.Parsers.SymbolKind.Type;
            }
        }

        public override bool IsFinal
        {
            get
            {
                return ctn.IsSealed;
            }
        }

        public Type CompiledType
        {
            get
            {
                return ctn;
            }
        }

        public override TypeScope GetInstance(List<TypeScope> gen_args)
        {
            Type t = this.ctn;
            if (!ctn.IsGenericTypeDefinition)
                t = PascalABCCompiler.NetHelper.NetHelper.FindType(this.ctn.FullName + "`" + gen_args.Count);
            else if (gen_args.Count != ctn.GetGenericArguments().Length)
            { 
                Type t2 = PascalABCCompiler.NetHelper.NetHelper.FindType(this.ctn.FullName.Substring(0, this.ctn.FullName.IndexOf('`')) + "`" + gen_args.Count);
                if (t2 != null)
                    t = t2;
            }
            CompiledScope sc = new CompiledScope(new SymInfo(si.name, si.kind, si.describe), t);
            sc.generic_params = new List<string>();
            sc.instances = new List<TypeScope>();
            sc.original_type = this;
            for (int i = 0; i < gen_args.Count; i++)
            {
                sc.generic_params.Add(gen_args[i].si.name);
                sc.instances.Add(gen_args[i]);
            }
            sc.si.describe = sc.GetDescription();
            return sc;
        }

        private void get_default_property()
        {
            PropertyInfo[] pis = ctn.GetProperties();
            is_def_prop_searched = true;
            for (int i = 0; i < pis.Length; i++)
            {
                ParameterInfo[] prms = pis[i].GetIndexParameters();
                if (prms.Length > 0)
                {
                    default_property = pis[i];
                    if (generic_params != null && default_property.PropertyType.IsGenericParameter)
                    {
                        elementType = instances[default_property.PropertyType.GenericParameterPosition];
                    }
                    else
                        elementType = TypeTable.get_compiled_type(new SymInfo(pis[i].PropertyType.Name, SymbolKind.Type, pis[i].PropertyType.FullName), pis[i].PropertyType);
                    break;
                }
            }
        }

        public override string GetFullName()
        {
            //return ctn.FullName;
            return CodeCompletionController.CurrentParser.LanguageInformation.GetSimpleDescription(this);
        }

        public override List<ProcScope> GetOverridableMethods()
        {
            List<ProcScope> syms = new List<ProcScope>();
            MemberInfo[] mis = ctn.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (ctn.IsInterface)
            {
                List<MemberInfo> mems = new List<MemberInfo>();
                mems.AddRange(mis);
                Type[] tt = ctn.GetInterfaces();
                foreach (Type t in tt)
                    mems.AddRange(t.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy));
                mis = mems.ToArray();
            }
            foreach (MemberInfo mi in mis)
                if (!mi.Name.Contains("$"))
                    switch (mi.MemberType)
                    {
                        case MemberTypes.Method: if (!(mi as MethodInfo).IsStatic)
                            {
                                SymInfo si2 = new SymInfo(null, SymbolKind.Method, null);
                                CompiledMethodScope member = new CompiledMethodScope(si2, mi as MethodInfo, this);
                                si2 = member.si;
                                if ((mi as MethodInfo).IsVirtual && !(mi as MethodInfo).IsFinal)
                                {
                                    if (si2.acc_mod == access_modifer.protected_modifer)
                                    {
                                        syms.Add(member);
                                    }
                                    else if (si2.acc_mod != access_modifer.private_modifer && si2.acc_mod != access_modifer.internal_modifer)
                                        syms.Add(member);
                                }
                            }
                            break;
                    }
            return syms;
        }

        public override List<ProcScope> GetAbstractMethods()
        {
            List<ProcScope> syms = new List<ProcScope>();
            MemberInfo[] mis = ctn.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (ctn.IsInterface)
            {
                List<MemberInfo> mems = new List<MemberInfo>();
                mems.AddRange(mis);
                Type[] tt = ctn.GetInterfaces();
                foreach (Type t in tt)
                    mems.AddRange(t.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy));
                mis = mems.ToArray();
            }
            foreach (MemberInfo mi in mis)
                if (!mi.Name.Contains("$"))
                    switch (mi.MemberType)
                    {
                        case MemberTypes.Method: if (!(mi as MethodInfo).IsStatic)
                            {
                                SymInfo si2 = new SymInfo(null, SymbolKind.Method, null);
                                CompiledMethodScope member = new CompiledMethodScope(si2, mi as MethodInfo, this);
                                si2 = member.si;
                                if ((mi as MethodInfo).IsAbstract || (ctn.IsInterface && (mi as MethodInfo).DeclaringType == ctn))
                                {
                                    if (si2.acc_mod == access_modifer.protected_modifer)
                                    {
                                        syms.Add(member);
                                    }
                                    else if (si2.acc_mod != access_modifer.private_modifer && si2.acc_mod != access_modifer.internal_modifer)
                                        syms.Add(member);
                                }
                            }
                            break;
                    }
            return syms;
        }

        private void AddIndexers()
        {
            List<TypeScope> indexers = new List<TypeScope>();
            if (ctn.IsArray)
            {
                /*Type t = ctn.GetElementType();
                while (t != null)
                {
                    indexers.Add(new CompiledScope(new SymInfo(t.Name, SymbolKind.Type,t.FullName),t));
                    t = t.GetElementType();
                }*/
                Type t = typeof(int);
                indexers.Add(TypeTable.get_compiled_type(new SymInfo(t.Name, SymbolKind.Type, t.FullName), t));
            }
            else
            {
                if (!is_def_prop_searched) get_default_property();
                if (default_property != null)
                {
                    ParameterInfo[] pis = default_property.GetIndexParameters();
                    for (int i = 0; i < pis.Length; i++)
                    {
                        if (generic_params != null && pis[i].ParameterType.IsGenericParameter)
                        {
                            indexers.Add(instances[pis[i].ParameterType.GenericParameterPosition]);
                        }
                        else
                            indexers.Add(TypeTable.get_compiled_type(new SymInfo(pis[i].ParameterType.Name, SymbolKind.Type, pis[i].ParameterType.FullName), pis[i].ParameterType));
                    }
                }
            }
            indices = indexers.ToArray();
        }

        public override TypeScope[] GetIndexers()
        {
            if (indices == null) AddIndexers();
            return indices;
        }

        public override void ClearInstances()
        {
            if (instances != null)
                instances.Clear();
        }

        public override bool IsConvertable(TypeScope ts)
        {
            CompiledScope cs = ts as CompiledScope;
            if (ts is NullTypeScope && !ctn.IsValueType)
                return true;
            if (cs == null)
                if (ts is TypeSynonim)
                    return this.IsConvertable((ts as TypeSynonim).actType);
                else
                    if (ts is ShortStringScope && ctn == typeof(string))
                    return true;
                else
                {
                    if (ts is UnknownScope)
                        return true;
                    else if (ts is ProcType)
                    {
                        ProcType pt = ts as ProcType;
                        MethodInfo invoke_meth = this.ctn.GetMethod("Invoke");
                        if (invoke_meth == null)
                        {
                            if (this.ctn == typeof(Delegate))
                                return true;
                            return false;
                        }
                        if (pt.target.parameters == null)
                            if (invoke_meth.GetParameters().Length > 0)
                                return false;
                        if (pt.target.parameters != null && pt.target.parameters.Count != invoke_meth.GetParameters().Length)
                            return false;
                        ParameterInfo[] parameters = invoke_meth.GetParameters();
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            CompiledScope param_cs = TypeTable.get_compiled_type(new SymInfo(null, SymbolKind.Type, null), parameters[i].ParameterType);
                            if (!(pt.target.parameters[i].sc is TypeScope) || !param_cs.IsConvertable(pt.target.parameters[i].sc as TypeScope))
                                return false;
                        }
                        return true;
                    }
                    else
                        return false;
                }
            if (this.ctn == cs.ctn)
                return true;

            if (this.ctn.IsSubclassOf(cs.ctn))
                return true;
            TypeCode code1 = Type.GetTypeCode(this.ctn);
            TypeCode code2 = Type.GetTypeCode(cs.ctn);
            bool left = false;
            bool right = false;
            switch (code1)
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.Char:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Object:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.String:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    left = true;
                    break;
            }
            switch (code2)
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.Char:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Object:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.String:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    right = true;
                    break;
            }
            if (left && right)
            {
                type_compare tc = PascalABCCompiler.TreeRealization.type_table.compare_types(
                    compiled_type_node.get_type_node(this.ctn),
                    compiled_type_node.get_type_node(cs.ctn));
                if (tc == type_compare.less_type)
                {
                    if (code1 != TypeCode.String && code1 != TypeCode.Object)
                        if (code1 == TypeCode.Double && code2 != TypeCode.Single)
                            return false;
                        else
                            return true;
                }
                if (cs.ctn.IsGenericParameter || this.ctn.IsGenericParameter)
                    return true;
                return tc == type_compare.less_type;
            }
            else
            {
                List<SymScope> conv1 = this.FindOverloadNamesOnlyInType("op_Implicit");
                List<SymScope> conv2 = cs.FindOverloadNamesOnlyInType("op_Implicit");
                if (conv1.Count > 0)
                {
                    for (int i = 0; i < conv1.Count; i++)
                        if (conv1[i] is ProcScope)
                        {
                            ProcScope ps = conv1[i] as ProcScope;
                            if (ps.parameters != null && ps.parameters.Count > 0 && ps.parameters[0].sc == cs)
                                return true;
                        }
                }
                else if (conv2.Count > 0)
                {
                    for (int i = 0; i < conv2.Count; i++)
                        if (conv2[i] is ProcScope)
                        {
                            ProcScope ps = conv2[i] as ProcScope;
                            if (ps.parameters != null && ps.parameters.Count > 0 && ps.parameters[0].sc == cs)
                                return true;
                        }
                }
            }
            return false;
        }

        public override bool IsEqual(SymScope ts)
        {
            CompiledScope cs = ts as CompiledScope;
            if (cs == null)
                if (ts is TypeSynonim) return this.IsEqual((ts as TypeSynonim).actType);
                else return false;
            return this.ctn == cs.ctn;
        }

        public override void AddIndexer(TypeScope ts)
        {

        }

        public override string GetDescription()
        {
            return CodeCompletionController.CurrentParser.LanguageInformation.GetDescription(this);
        }

        public override SymInfo[] GetNames(ExpressionVisitor ev, PascalABCCompiler.Parsers.KeywordKind keyword, bool called_in_base)
        {
            List<SymInfo> syms = new List<SymInfo>();
            if (!CodeCompletionController.CurrentParser.LanguageInformation.IncludeDotNetEntities)
                return syms.ToArray();
            MemberInfo[] mis = ctn.GetMembers(BindingFlags.Public | BindingFlags.NonPublic |/*BindingFlags.Instance|*/BindingFlags.Static | BindingFlags.FlattenHierarchy);
            List<MemberInfo> constrs = new List<MemberInfo>();
            //constrs.AddRange(ctn.GetNestedTypes(BindingFlags.Public));
            //if (ctn != typeof(object))
            ConstructorInfo[] cis = ctn.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (ConstructorInfo ci in cis)
                if (!called_in_base)
                    constrs.Add(ci);
                else if (ci.GetParameters().Length > 0)
                    constrs.Add(ci);
            constrs.AddRange(mis);
            //constrs.AddRange(PascalABCCompiler.NetHelper.NetHelper.GetExtensionMethods(ctn));
            mis = constrs.ToArray();
            if (ctn.IsInterface)
            {
                return syms.ToArray();
            }
            foreach (MemberInfo mi in mis)
                if (!mi.Name.Contains("$"))
                    switch (mi.MemberType)
                    {
                        case MemberTypes.Method: if (!(mi as MethodInfo).IsSpecialName && (mi as MethodInfo).IsStatic)
                            {
                                SymInfo si2 = new SymInfo(null, SymbolKind.Method, null);
                                CompiledMethodScope member = new CompiledMethodScope(si2, mi as MethodInfo, this);
                                si2 = member.si;
                                if (si2.acc_mod == access_modifer.protected_modifer)
                                {
                                    if (ev.CheckForBaseAccess(ev.entry_scope, this))
                                        syms.Add(si2);
                                }
                                else if (si2.acc_mod != access_modifer.private_modifer && si2.acc_mod != access_modifer.internal_modifer)
                                    syms.Add(si2);
                            }
                            break;
                        case MemberTypes.Field:
                            {
                                if (!(mi as FieldInfo).IsSpecialName && (mi as FieldInfo).IsStatic)
                                {
                                    SymInfo si2 = new SymInfo(null, SymbolKind.Field, null);
                                    if ((mi as FieldInfo).IsLiteral) si2.kind = SymbolKind.Constant;
                                    si2 = new CompiledFieldScope(si2, mi as FieldInfo, this).si;
                                    if (si2.acc_mod == access_modifer.protected_modifer)
                                    {
                                        if (ev.CheckForBaseAccess(ev.entry_scope, this))
                                            syms.Add(si2);
                                    }
                                    else if (si2.acc_mod != access_modifer.private_modifer && si2.acc_mod != access_modifer.internal_modifer)
                                        syms.Add(si2);
                                }
                            }
                            break;
                        case MemberTypes.Constructor:
                            {
                                SymInfo si2 = new SymInfo(null, SymbolKind.Method, null);
                                CompiledConstructorScope member = new CompiledConstructorScope(si2, mi as ConstructorInfo, this);
                                si2 = member.si;
                                if (si2.acc_mod == access_modifer.protected_modifer)
                                {
                                    if (ev.CheckForBaseAccess(ev.entry_scope, this))
                                        syms.Add(si2);
                                }
                                else if (si2.acc_mod != access_modifer.private_modifer && si2.acc_mod != access_modifer.internal_modifer)
                                    syms.Add(si2);
                            }
                            break;
                        case MemberTypes.Property:
                            {
                                PropertyInfo pi = mi as PropertyInfo;
                                if (pi.GetGetMethod(true) != null && pi.GetGetMethod(true).IsStatic)
                                {
                                    SymInfo si2 = new SymInfo(null, SymbolKind.Property, null);
                                    si2 = new CompiledPropertyScope(si2, mi as PropertyInfo, this).si;
                                    if (si2.acc_mod == access_modifer.protected_modifer)
                                    {
                                        if (ev.CheckForBaseAccess(ev.entry_scope, this))
                                            syms.Add(si2);
                                    }
                                    else if (si2.acc_mod != access_modifer.private_modifer && si2.acc_mod != access_modifer.internal_modifer)
                                        syms.Add(si2);
                                }
                            }
                            break;
                        case MemberTypes.Event:
                            {
                                MethodInfo acc_mi = (mi as EventInfo).GetAddMethod(true);
                                if (acc_mi != null && acc_mi.IsStatic)
                                {
                                    SymInfo si2 = new SymInfo(null, SymbolKind.Event, null);
                                    si2 = new CompiledEventScope(si2, mi as EventInfo, this).si;
                                    syms.Add(si2);
                                    if (si2.acc_mod == access_modifer.protected_modifer)
                                    {
                                        if (ev.CheckForBaseAccess(ev.entry_scope, this))
                                            syms.Add(si2);
                                    }
                                    else if (si2.acc_mod != access_modifer.private_modifer && si2.acc_mod != access_modifer.internal_modifer)
                                        syms.Add(si2);
                                }
                            }
                            break;
                        case MemberTypes.NestedType:
                            {
                                if ((mi as Type).IsNestedPublic)
                                {
                                    SymInfo si2 = new SymInfo(null, SymbolKind.Type, null);
                                    CompiledScope member = TypeTable.get_compiled_type(si2, mi as Type);
                                    si2 = member.si;
                                    syms.Add(si2);
                                }
                            }
                            break;
                    }
            return syms.ToArray();
        }

        public override ProcScope GetConstructor()
        {
            ConstructorInfo[] constrs = ctn.GetConstructors();
            ProcScope ps = null;
            ProcScope tmp = null;
            if (constrs.Length > 0)
            {
                ps = new CompiledConstructorScope(new SymInfo("constructor", SymbolKind.Method, "constructor"), constrs[0], this);
                tmp = ps;
            }
            for (int i = 1; i < constrs.Length; i++)
            {
                tmp.nextProc = new CompiledConstructorScope(new SymInfo("constructor", SymbolKind.Method, "constructor"), constrs[i], this);
                tmp = tmp.nextProc;
            }
            return ps;
        }

        public override List<ProcScope> GetConstructors(bool search_in_base)
        {
            ConstructorInfo[] constrs = ctn.GetConstructors();
            ProcScope ps = null;
            ProcScope tmp = null;
            List<ProcScope> names = new List<ProcScope>();
            if (constrs.Length > 0)
            {
                ps = new CompiledConstructorScope(new SymInfo("constructor", SymbolKind.Method, "constructor"), constrs[0], this);
                names.Add(ps);
                tmp = ps;
            }
            for (int i = 1; i < constrs.Length; i++)
            {
                tmp.nextProc = new CompiledConstructorScope(new SymInfo("constructor", SymbolKind.Method, "constructor"), constrs[i], this);
                names.Add(tmp.nextProc);
                tmp = tmp.nextProc;
            }
            return names;
        }

        public SymInfo[] GetStaticNames()
        {
            List<SymInfo> syms = new List<SymInfo>();
            if (!CodeCompletionController.CurrentParser.LanguageInformation.IncludeDotNetEntities)
                return syms.ToArray();
            MemberInfo[] mis = ctn.GetMembers(BindingFlags.Public | BindingFlags.NonPublic |/*BindingFlags.Instance|*/BindingFlags.Static | BindingFlags.FlattenHierarchy);

            if (ctn.IsInterface)
            {
                return syms.ToArray();
            }
            foreach (MemberInfo mi in mis)
                if (!mi.Name.Contains("$"))
                    switch (mi.MemberType)
                    {
                        case MemberTypes.Method: if (!(mi as MethodInfo).IsSpecialName && (mi as MethodInfo).IsStatic)
                            {
                                SymInfo si2 = new SymInfo(null, SymbolKind.Method, null);
                                CompiledMethodScope member = new CompiledMethodScope(si2, mi as MethodInfo, this);
                                si2 = member.si;
                                if (si2.acc_mod != access_modifer.private_modifer && si2.acc_mod != access_modifer.internal_modifer)
                                    syms.Add(si2);
                            }
                            break;
                        case MemberTypes.Field:
                            {
                                if (!(mi as FieldInfo).IsSpecialName && (mi as FieldInfo).IsStatic)
                                {
                                    SymInfo si2 = new SymInfo(null, SymbolKind.Field, null);
                                    if ((mi as FieldInfo).IsLiteral) si2.kind = SymbolKind.Constant;
                                    si2 = new CompiledFieldScope(si2, mi as FieldInfo, this).si;
                                    if (si2.acc_mod != access_modifer.private_modifer && si2.acc_mod != access_modifer.internal_modifer)
                                        syms.Add(si2);
                                }
                            }
                            break;
                        case MemberTypes.Constructor:
                            {
                                SymInfo si2 = new SymInfo(null, SymbolKind.Method, null);
                                CompiledConstructorScope member = new CompiledConstructorScope(si2, mi as ConstructorInfo, this);
                                si2 = member.si;
                                if (si2.acc_mod != access_modifer.private_modifer && si2.acc_mod != access_modifer.internal_modifer)
                                    syms.Add(si2);
                            }
                            break;
                        case MemberTypes.Property:
                            {
                                PropertyInfo pi = mi as PropertyInfo;
                                if (pi.GetGetMethod(true) != null && pi.GetGetMethod(true).IsStatic)
                                {
                                    SymInfo si2 = new SymInfo(null, SymbolKind.Property, null);
                                    si2 = new CompiledPropertyScope(si2, mi as PropertyInfo, this).si;
                                    if (si2.acc_mod != access_modifer.private_modifer && si2.acc_mod != access_modifer.internal_modifer)
                                        syms.Add(si2);
                                }
                            }
                            break;
                        case MemberTypes.Event:
                            {
                                MethodInfo acc_mi = (mi as EventInfo).GetAddMethod(true);
                                if (acc_mi != null && acc_mi.IsStatic)
                                {
                                    SymInfo si2 = new SymInfo(null, SymbolKind.Event, null);
                                    si2 = new CompiledEventScope(si2, mi as EventInfo, this).si;
                                    syms.Add(si2);
                                    if (si2.acc_mod != access_modifer.private_modifer && si2.acc_mod != access_modifer.internal_modifer)
                                        syms.Add(si2);
                                }
                            }
                            break;
                        case MemberTypes.NestedType:
                            {
                                if ((mi as Type).IsNestedPublic)
                                {
                                    SymInfo si2 = new SymInfo(null, SymbolKind.Type, null);
                                    CompiledScope member = TypeTable.get_compiled_type(si2, mi as Type);
                                    si2 = member.si;
                                    syms.Add(si2);
                                }
                            }
                            break;
                    }
            return syms.ToArray();
        }

        public override SymInfo[] GetNamesAsInObject(ExpressionVisitor ev)
        {
            List<SymInfo> syms = new List<SymInfo>();
            if (!CodeCompletionController.CurrentParser.LanguageInformation.IncludeDotNetEntities)
                return syms.ToArray();
            MemberInfo[] mis = ctn.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            List<MemberInfo> members = new List<MemberInfo>();
            members.AddRange(mis);
            IEnumerable<MemberInfo> en = PascalABCCompiler.NetHelper.NetHelper.GetExtensionMethods(ctn);
            members.AddRange(en);
            mis = members.ToArray();
            if (ctn.IsInterface)
            {
                List<MemberInfo> mems = new List<MemberInfo>();
                mems.AddRange(mis);
                mems.AddRange(typeof(object).GetMembers(BindingFlags.Public | BindingFlags.Instance));
                Type[] types = ctn.GetInterfaces();
                foreach (Type t in types)
                    mems.AddRange(t.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy));
                mis = mems.ToArray();
            }
            foreach (MemberInfo mi in mis)
                if (!mi.Name.Contains("$"))
                    switch (mi.MemberType)
                    {
                        case MemberTypes.Method: if (!(mi as MethodInfo).IsSpecialName)
                            {
                                SymInfo si2 = new SymInfo(null, SymbolKind.Method, null);
                                si2 = new CompiledMethodScope(si2, mi as MethodInfo, this).si;
                                if (si2.acc_mod == access_modifer.protected_modifer)
                                {
                                    if (ev.CheckForBaseAccess(ev.entry_scope, this))
                                        syms.Add(si2);
                                }
                                else if (si2.acc_mod != access_modifer.private_modifer && si2.acc_mod != access_modifer.internal_modifer)
                                    syms.Add(si2);
                            }
                            break;
                        case MemberTypes.Field:
                            {
                                if (!(mi as FieldInfo).IsSpecialName)
                                {
                                    SymInfo si2 = new SymInfo(null, SymbolKind.Field, null);
                                    if ((mi as FieldInfo).IsLiteral) si2.kind = SymbolKind.Constant;
                                    si2 = new CompiledFieldScope(si2, mi as FieldInfo, this).si;
                                    if (si2.acc_mod == access_modifer.protected_modifer)
                                    {
                                        if (ev.CheckForBaseAccess(ev.entry_scope, this))
                                            syms.Add(si2);
                                    }
                                    else if (si2.acc_mod != access_modifer.private_modifer && si2.acc_mod != access_modifer.internal_modifer)
                                        syms.Add(si2);
                                }
                            }
                            break;
                        case MemberTypes.Property:
                            {
                                SymInfo si2 = new SymInfo(null, SymbolKind.Property, null);
                                si2 = new CompiledPropertyScope(si2, mi as PropertyInfo, this).si;
                                if (si2.acc_mod == access_modifer.protected_modifer)
                                {
                                    if (ev.CheckForBaseAccess(ev.entry_scope, this))
                                        syms.Add(si2);
                                }
                                else if (si2.acc_mod != access_modifer.private_modifer && si2.acc_mod != access_modifer.internal_modifer)
                                    syms.Add(si2);
                            }
                            break;
                        case MemberTypes.Event:
                            {
                                SymInfo si2 = new SymInfo(null, SymbolKind.Event, null);
                                si2 = new CompiledEventScope(si2, mi as EventInfo, this).si;
                                if (si2.acc_mod == access_modifer.protected_modifer)
                                {
                                    if (ev.CheckForBaseAccess(ev.entry_scope, this))
                                        syms.Add(si2);
                                }
                                else if (si2.acc_mod != access_modifer.private_modifer && si2.acc_mod != access_modifer.internal_modifer)
                                    syms.Add(si2);
                            }
                            break;
                    }
            if (implemented_interfaces != null && false)
            {
                foreach (TypeScope ts in implemented_interfaces)
                    syms.AddRange(ts.GetNamesAsInObject(ev));
            }
            return syms.ToArray();
        }

        public override SymInfo[] GetNamesAsInBaseClass(ExpressionVisitor ev, bool is_static)
        {
            List<SymInfo> syms = new List<SymInfo>();
            if (!CodeCompletionController.CurrentParser.LanguageInformation.IncludeDotNetEntities)
                return syms.ToArray();
            MemberInfo[] mis = ctn.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (ctn.IsInterface)
            {
                List<MemberInfo> mems = new List<MemberInfo>();
                mems.AddRange(mis);
                mems.AddRange(typeof(object).GetMembers());
                Type[] types = ctn.GetInterfaces();
                foreach (Type t in types)
                    mems.AddRange(t.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy));
                mis = mems.ToArray();
            }
            if (!is_static)
            {
                foreach (MemberInfo mi in mis)
                    if (!mi.Name.Contains("$"))
                    {
                        switch (mi.MemberType)
                        {
                            case MemberTypes.Method: if (!(mi as MethodInfo).IsSpecialName && ((mi as MethodInfo).IsPublic || (mi as MethodInfo).IsFamily))
                                {
                                    SymInfo si2 = new SymInfo(null, SymbolKind.Method, null);
                                    si2 = new CompiledMethodScope(si2, mi as MethodInfo, this).si;
                                    syms.Add(si2);
                                }
                                break;
                            case MemberTypes.Field:
                                {
                                    if (!(mi as FieldInfo).IsSpecialName && ((mi as FieldInfo).IsPublic || (mi as FieldInfo).IsFamily))
                                    {
                                        SymInfo si2 = new SymInfo(null, SymbolKind.Field, null);
                                        if ((mi as FieldInfo).IsLiteral) si2.kind = SymbolKind.Constant;
                                        si2 = new CompiledFieldScope(si2, mi as FieldInfo, this).si;
                                        syms.Add(si2);
                                    }
                                    //syms.Add(new SymInfo(mi.Name,SymbolKind.Field,mi.Name)); break;
                                }
                                break;
                            case MemberTypes.Property:
                                {
                                    PropertyInfo pi = mi as PropertyInfo;
                                    MethodInfo acc_mi = pi.GetGetMethod(true);
                                    if (acc_mi != null && (acc_mi.IsPublic || acc_mi.IsFamily))
                                    {
                                        SymInfo si2 = new SymInfo(null, SymbolKind.Property, null);
                                        si2 = new CompiledPropertyScope(si2, mi as PropertyInfo, this).si;
                                        syms.Add(si2);
                                    }
                                    //syms.Add(new SymInfo(mi.Name,SymbolKind.Property,mi.Name));
                                }
                                break;
                            case MemberTypes.Event:
                                {
                                    SymInfo si2 = new SymInfo(null, SymbolKind.Event, null);
                                    si2 = new CompiledEventScope(si2, mi as EventInfo, this).si;
                                    syms.Add(si2);
                                    //syms.Add(new SymInfo(mi.Name,SymbolKind.Event,mi.Name));
                                }
                                break;
                        }
                    }
            }
            else
                foreach (MemberInfo mi in mis)
                    switch (mi.MemberType)
                    {
                        case MemberTypes.Method: if (!((mi as MethodInfo).IsSpecialName) && (mi as MethodInfo).IsStatic && (mi as MethodInfo).IsPublic || (mi as MethodInfo).IsFamily)
                            {
                                SymInfo si2 = new SymInfo(null, SymbolKind.Method, null);
                                si2 = new CompiledMethodScope(si2, mi as MethodInfo, this).si;
                                syms.Add(si2);
                            }
                            break;
                        case MemberTypes.Field:
                            {
                                if ((mi as FieldInfo).IsStatic && !(mi as FieldInfo).IsSpecialName && (mi as FieldInfo).IsPublic || (mi as FieldInfo).IsFamily)
                                {
                                    SymInfo si2 = new SymInfo(null, SymbolKind.Field, null);
                                    if ((mi as FieldInfo).IsLiteral) si2.kind = SymbolKind.Constant;
                                    si2 = new CompiledFieldScope(si2, mi as FieldInfo, this).si;
                                    syms.Add(si2);
                                }
                                //syms.Add(new SymInfo(mi.Name,SymbolKind.Field,mi.Name)); break;
                            }
                            break;
                        case MemberTypes.Property:
                            {
                                PropertyInfo pi = mi as PropertyInfo;
                                MethodInfo acc_mi = pi.GetGetMethod(true);
                                if (acc_mi != null && acc_mi.IsStatic && (acc_mi.IsFamily || acc_mi.IsPublic))
                                {
                                    SymInfo si2 = new SymInfo(null, SymbolKind.Property, null);
                                    si2 = new CompiledPropertyScope(si2, mi as PropertyInfo, this).si;
                                    syms.Add(si2);
                                }
                                //syms.Add(new SymInfo(mi.Name,SymbolKind.Property,mi.Name));
                            }
                            break;
                        case MemberTypes.Event:
                            {
                                SymInfo si2 = new SymInfo(null, SymbolKind.Event, null);
                                si2 = new CompiledEventScope(si2, mi as EventInfo, this).si;
                                syms.Add(si2);
                                //syms.Add(new SymInfo(mi.Name,SymbolKind.Event,mi.Name));
                            }
                            break;
                    }
            return syms.ToArray();
        }

        public override SymInfo[] GetNames()
        {
            List<SymInfo> syms = new List<SymInfo>();
            if (!CodeCompletionController.CurrentParser.LanguageInformation.IncludeDotNetEntities)
                return syms.ToArray();
            MemberInfo[] mis = ctn.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (ctn.IsInterface)
            {
                List<MemberInfo> mems = new List<MemberInfo>();
                mems.AddRange(mis);
                mems.AddRange(typeof(object).GetMembers());
                Type[] types = ctn.GetInterfaces();
                foreach (Type t in types)
                    mems.AddRange(t.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy));
                mis = mems.ToArray();
            }
            if (si.kind != SymbolKind.Type)
            {
                foreach (MemberInfo mi in mis)
                    if (!mi.Name.Contains("$"))
                        switch (mi.MemberType)
                        {
                            case MemberTypes.Method: if (!(mi as MethodInfo).IsSpecialName)
                                {
                                    SymInfo si2 = new SymInfo(null, SymbolKind.Method, null);
                                    si2 = new CompiledMethodScope(si2, mi as MethodInfo, this).si;
                                    syms.Add(si2);
                                }
                                break;
                            case MemberTypes.Field:
                                {
                                    if (!(mi as FieldInfo).IsSpecialName)
                                    {
                                        SymInfo si2 = new SymInfo(null, SymbolKind.Field, null);
                                        if ((mi as FieldInfo).IsLiteral) si2.kind = SymbolKind.Constant;
                                        si2 = new CompiledFieldScope(si2, mi as FieldInfo, this).si;
                                        syms.Add(si2);
                                    }
                                    //syms.Add(new SymInfo(mi.Name,SymbolKind.Field,mi.Name)); break;
                                }
                                break;
                            case MemberTypes.Property:
                                {
                                    PropertyInfo pi = mi as PropertyInfo;
                                    MethodInfo acc_mi = pi.GetGetMethod(true);
                                    if (acc_mi != null && !acc_mi.IsStatic)
                                    {
                                        SymInfo si2 = new SymInfo(null, SymbolKind.Property, null);
                                        si2 = new CompiledPropertyScope(si2, mi as PropertyInfo, this).si;
                                        syms.Add(si2);
                                    }
                                    //syms.Add(new SymInfo(mi.Name,SymbolKind.Property,mi.Name));
                                }
                                break;
                            case MemberTypes.Event:
                                {
                                    MethodInfo acc_mi = (mi as EventInfo).GetAddMethod(true);
                                    if (acc_mi.IsPublic && !acc_mi.IsStatic)
                                    {
                                        SymInfo si2 = new SymInfo(null, SymbolKind.Event, null);
                                        si2 = new CompiledEventScope(si2, mi as EventInfo, this).si;
                                        syms.Add(si2);
                                    }
                                    //syms.Add(new SymInfo(mi.Name,SymbolKind.Event,mi.Name));
                                }
                                break;
                        }
            }
            else
                foreach (MemberInfo mi in mis)
                    switch (mi.MemberType)
                    {
                        case MemberTypes.Method: if (!((mi as MethodInfo).IsSpecialName) && (mi as MethodInfo).IsStatic)
                            {
                                SymInfo si2 = new SymInfo(null, SymbolKind.Method, null);
                                si2 = new CompiledMethodScope(si2, mi as MethodInfo, this).si;
                                syms.Add(si2);
                            }
                            break;
                        case MemberTypes.Field:
                            {
                                if ((mi as FieldInfo).IsStatic && !(mi as FieldInfo).IsSpecialName)
                                {
                                    SymInfo si2 = new SymInfo(null, SymbolKind.Field, null);
                                    if ((mi as FieldInfo).IsLiteral) si2.kind = SymbolKind.Constant;
                                    si2 = new CompiledFieldScope(si2, mi as FieldInfo, this).si;
                                    syms.Add(si2);
                                }
                            }
                            break;
                        case MemberTypes.Property:
                            {
                                PropertyInfo pi = mi as PropertyInfo;
                                MethodInfo acc_mi = pi.GetGetMethod(true);
                                if (acc_mi != null && acc_mi.IsStatic)
                                {
                                    SymInfo si2 = new SymInfo(null, SymbolKind.Property, null);
                                    si2 = new CompiledPropertyScope(si2, mi as PropertyInfo, this).si;
                                    syms.Add(si2);
                                }
                            }
                            break;
                        case MemberTypes.Event:
                            {
                                MethodInfo acc_mi = (mi as EventInfo).GetAddMethod(true);
                                if (acc_mi.IsPublic && acc_mi.IsStatic)
                                {
                                    SymInfo si2 = new SymInfo(null, SymbolKind.Event, null);
                                    si2 = new CompiledEventScope(si2, mi as EventInfo, this).si;
                                    syms.Add(si2);
                                }
                                //syms.Add(new SymInfo(mi.Name,SymbolKind.Event,mi.Name));
                            }
                            break;
                    }

            return syms.ToArray();
        }

        public override SymInfo[] GetNamesAsInObject()
        {
            List<SymInfo> syms = new List<SymInfo>();
            if (!CodeCompletionController.CurrentParser.LanguageInformation.IncludeDotNetEntities)
                return syms.ToArray();
            MemberInfo[] mis = ctn.GetMembers(BindingFlags.Public | BindingFlags.Instance);
            foreach (MemberInfo mi in mis)
                if (!mi.Name.Contains("$"))
                    switch (mi.MemberType)
                    {
                        case MemberTypes.Method: if (!(mi as MethodInfo).IsSpecialName)
                            {
                                SymInfo si2 = new SymInfo(null, SymbolKind.Method, null);
                                si2 = new CompiledMethodScope(si2, mi as MethodInfo, this).si;
                                syms.Add(si2);
                            }
                            break;
                        case MemberTypes.Field:
                            {
                                if (!(mi as FieldInfo).IsSpecialName)
                                {
                                    SymInfo si2 = new SymInfo(null, SymbolKind.Field, null);
                                    if ((mi as FieldInfo).IsLiteral) si2.kind = SymbolKind.Constant;
                                    si2 = new CompiledFieldScope(si2, mi as FieldInfo, this).si;
                                    syms.Add(si2);
                                }
                                //syms.Add(new SymInfo(mi.Name,SymbolKind.Field,mi.Name)); break;
                            }
                            break;
                        case MemberTypes.Property:
                            {
                                SymInfo si2 = new SymInfo(null, SymbolKind.Property, null);
                                si2 = new CompiledPropertyScope(si2, mi as PropertyInfo, this).si;
                                syms.Add(si2);
                                //syms.Add(new SymInfo(mi.Name,SymbolKind.Property,mi.Name));
                            }
                            break;
                        case MemberTypes.Event:
                            {
                                SymInfo si2 = new SymInfo(null, SymbolKind.Event, null);
                                si2 = new CompiledEventScope(si2, mi as EventInfo, this).si;
                                syms.Add(si2);
                                //syms.Add(new SymInfo(mi.Name,SymbolKind.Event,mi.Name));
                            }
                            break;
                    }
            return syms.ToArray();
        }

        public override SymScope FindNameOnlyInType(string name)
        {
            return FindName(name);
        }

        public override List<SymScope> FindOverloadNamesOnlyInType(string name)
        {
            return FindOverloadNames(name);
        }

        public override SymScope FindNameOnlyInThisType(string name)
        {
            return FindName(name);
        }

        public override TypeScope GetElementType()
        {
            if (!is_def_prop_searched)
                get_default_property();
            if (ctn == typeof(IEnumerable<>) && instances.Count > 0)
            {
                return instances[0];
            }
            return elementType;
        }

        public override List<SymScope> FindOverloadNames(string name)
        {
            List<SymScope> names = new List<SymScope>();
            PascalABCCompiler.TreeConverter.SymbolInfo si = PascalABCCompiler.NetHelper.NetHelper.FindNameIncludeProtected(ctn, name);
            //IEnumerable<MemberInfo> ext_meths = PascalABCCompiler.NetHelper.NetHelper.GetExtensionMethods(ctn);
            List<ProcScope> pascal_ext_meths = this.GetExtensionMethods(name, this);

            if (si == null && names.Count == 0)
            {
                if (string.Compare(name, "Create", true) == 0 && this.ctn != typeof(object))
                    si = PascalABCCompiler.NetHelper.NetHelper.GetConstructor(ctn);
                if (si == null)
                {
                    if (pascal_ext_meths.Count > 0)
                    {
                        foreach (SymScope ss in pascal_ext_meths)
                            names.Add(ss);
                        return names;
                    }
                    if (members != null)
                    {
                        return base.FindOverloadNames(name);
                    }
                    else
                        return names;
                }
            }
            switch (si.sym_info.semantic_node_type)
            {
                case semantic_node_type.compiled_function_node:
                    {
                        CompiledMethodScope cms = new CompiledMethodScope(new SymInfo(name, SymbolKind.Method, name), (si.sym_info as compiled_function_node).method_info, this);
                        names.Insert(0, cms);
                        si = si.Next;
                        CompiledMethodScope tmp = cms;
                        SortedDictionary<int, List<CompiledMethodScope>> meths = new SortedDictionary<int, List<CompiledMethodScope>>();
                        if (cms.acc_mod != access_modifer.internal_modifer && cms.acc_mod != access_modifer.private_modifer)
                        {
                            int par_num = cms.mi.GetParameters().Length;
                            meths[par_num] = new List<CompiledMethodScope>();
                            meths[par_num].Add(cms);
                        }
                        while (si != null)
                        {
                            if (si.access_level != PascalABCCompiler.TreeConverter.access_level.al_internal && si.access_level != PascalABCCompiler.TreeConverter.access_level.al_private && si.sym_info.semantic_node_type == semantic_node_type.compiled_function_node)
                            {
                                tmp = new CompiledMethodScope(new SymInfo(name, SymbolKind.Method, name), (si.sym_info as compiled_function_node).method_info, this);
                                names.Insert(0, tmp);
                                //tmp.nextProc = cms;
                                //cms = tmp;
                                int par_num = tmp.mi.GetParameters().Length;
                                if (!meths.ContainsKey(par_num)) meths[par_num] = new List<CompiledMethodScope>();
                                meths[par_num].Add(tmp);
                            }
                            si = si.Next;
                        }
                        bool beg = false;
                        tmp = null;
                        cms = null;
                        foreach (List<CompiledMethodScope> lst in meths.Values)
                        {
                            foreach (CompiledMethodScope m in lst)
                            {
                                if (beg == false)
                                {
                                    tmp = m;
                                    cms = tmp;
                                    beg = true;
                                }
                                else
                                {
                                    tmp.nextProc = m;
                                    tmp = tmp.nextProc as CompiledMethodScope;
                                }
                            }
                        }
                    }
                    break;
                case semantic_node_type.compiled_constructor_node:
                    {
                        CompiledConstructorScope cms = new CompiledConstructorScope(new SymInfo("Create", SymbolKind.Method, "Create"), (si.sym_info as compiled_constructor_node).constructor_info, this);
                        names.Insert(0, cms);
                        si = si.Next;
                        CompiledConstructorScope tmp = cms;
                        SortedDictionary<int, List<CompiledConstructorScope>> meths = new SortedDictionary<int, List<CompiledConstructorScope>>();
                        if (cms.acc_mod != access_modifer.internal_modifer && cms.acc_mod != access_modifer.private_modifer)
                        {
                            int par_num = cms.mi.GetParameters().Length;
                            meths[par_num] = new List<CompiledConstructorScope>();
                            meths[par_num].Add(cms);
                        }
                        while (si != null)
                        {
                            if (si.access_level != PascalABCCompiler.TreeConverter.access_level.al_internal && si.access_level != PascalABCCompiler.TreeConverter.access_level.al_private && si.sym_info.semantic_node_type == semantic_node_type.compiled_constructor_node)
                            {
                                tmp = new CompiledConstructorScope(new SymInfo("Create", SymbolKind.Method, "Create"), (si.sym_info as compiled_constructor_node).constructor_info, this);
                                //tmp.nextProc = cms;
                                //cms = tmp;
                                names.Insert(0, tmp);
                                int par_num = tmp.mi.GetParameters().Length;
                                if (!meths.ContainsKey(par_num)) meths[par_num] = new List<CompiledConstructorScope>();
                                meths[par_num].Add(tmp);
                            }
                            si = si.Next;
                        }
                        bool beg = false;
                        tmp = null;
                        cms = null;
                        foreach (List<CompiledConstructorScope> lst in meths.Values)
                        {
                            foreach (CompiledConstructorScope m in lst)
                            {
                                if (beg == false)
                                {
                                    tmp = m;
                                    cms = tmp;
                                    beg = true;
                                }
                                else
                                {
                                    tmp.nextProc = m;
                                    tmp = tmp.nextProc as CompiledConstructorScope;
                                }
                            }
                        }
                    }
                    break;
                case semantic_node_type.compiled_variable_definition:
                    if (si.access_level != PascalABCCompiler.TreeConverter.access_level.al_internal && si.access_level != PascalABCCompiler.TreeConverter.access_level.al_private)
                        names.Add(new CompiledFieldScope(new SymInfo(name, SymbolKind.Field, name), (si.sym_info as compiled_variable_definition).compiled_field, this));
                    break;
                case semantic_node_type.basic_property_node:
                    if (si.access_level != PascalABCCompiler.TreeConverter.access_level.al_internal && si.access_level != PascalABCCompiler.TreeConverter.access_level.al_private)
                        names.Add(new CompiledPropertyScope(new SymInfo(name, SymbolKind.Property, name), (si.sym_info as compiled_property_node).property_info, this));
                    break;
                case semantic_node_type.compiled_class_constant_definition:
                    if (si.access_level != PascalABCCompiler.TreeConverter.access_level.al_internal && si.access_level != PascalABCCompiler.TreeConverter.access_level.al_private)
                        names.Add(new CompiledFieldScope(new SymInfo(name, SymbolKind.Constant, name), (si.sym_info as compiled_class_constant_definition).field, this));
                    break;
                case semantic_node_type.compiled_event:
                    if (si.access_level != PascalABCCompiler.TreeConverter.access_level.al_internal && si.access_level != PascalABCCompiler.TreeConverter.access_level.al_private)
                        names.Add(new CompiledEventScope(new SymInfo(name, SymbolKind.Constant, name), (si.sym_info as compiled_event).event_info, this));
                    break;
                //case semantic_node_type.compiled_event: return new CompiledScope(null,(si.sym_info as compiled_event).event_info.);
                //case semantic_node_type.compiled_event: return new CompiledScope(null,(si.sym_info as compiled_event).
            }
            
            return names;
        }

        public override SymScope FindName(string name)
        {
            PascalABCCompiler.TreeConverter.SymbolInfo si = PascalABCCompiler.NetHelper.NetHelper.FindNameIncludeProtected(ctn, name);
            if (!CodeCompletionController.CurrentParser.LanguageInformation.IncludeDotNetEntities)
                return null;
            if (si == null)
            {
                if (string.Compare(name, "Create", true) == 0)
                    si = PascalABCCompiler.NetHelper.NetHelper.GetConstructor(ctn);
                if (si == null)
                {
                    return null;
                }  
            }
            switch (si.sym_info.semantic_node_type)
            {
                case semantic_node_type.compiled_function_node:
                    {
                        CompiledMethodScope cms = new CompiledMethodScope(new SymInfo(name, SymbolKind.Method, name), (si.sym_info as compiled_function_node).method_info, this);
                        si = si.Next;
                        CompiledMethodScope tmp = cms;
                        SortedDictionary<int, List<CompiledMethodScope>> meths = new SortedDictionary<int, List<CompiledMethodScope>>();
                        if (cms.acc_mod != access_modifer.internal_modifer && cms.acc_mod != access_modifer.private_modifer)
                        {
                            int par_num = cms.mi.GetParameters().Length;
                            meths[par_num] = new List<CompiledMethodScope>();
                            meths[par_num].Add(cms);
                        }
                        while (si != null)
                        {
                            if (si.access_level != PascalABCCompiler.TreeConverter.access_level.al_internal && si.access_level != PascalABCCompiler.TreeConverter.access_level.al_private && si.sym_info.semantic_node_type == semantic_node_type.compiled_function_node)
                            {
                                tmp = new CompiledMethodScope(new SymInfo(name, SymbolKind.Method, name), (si.sym_info as compiled_function_node).method_info, this);
                                //tmp.nextProc = cms;
                                //cms = tmp;
                                int par_num = tmp.mi.GetParameters().Length;
                                if (!meths.ContainsKey(par_num)) meths[par_num] = new List<CompiledMethodScope>();
                                meths[par_num].Add(tmp);
                            }
                            else if (si.access_level != PascalABCCompiler.TreeConverter.access_level.al_internal && si.access_level != PascalABCCompiler.TreeConverter.access_level.al_private && si.sym_info is compiled_property_node)
                            {
                                return new CompiledPropertyScope(new SymInfo(name, SymbolKind.Property, name), (si.sym_info as compiled_property_node).property_info, this);
                            }
                            si = si.Next;
                        }
                        bool beg = false;
                        tmp = null;
                        cms = null;
                        foreach (List<CompiledMethodScope> lst in meths.Values)
                        {
                            foreach (CompiledMethodScope m in lst)
                            {
                                if (beg == false)
                                {
                                    tmp = m;
                                    cms = tmp;
                                    beg = true;
                                }
                                else
                                {
                                    tmp.nextProc = m;
                                    tmp = tmp.nextProc as CompiledMethodScope;
                                }
                            }
                        }
                        return cms;
                    }
                case semantic_node_type.compiled_constructor_node:
                    {
                        CompiledConstructorScope cms = new CompiledConstructorScope(new SymInfo("Create", SymbolKind.Method, "Create"), (si.sym_info as compiled_constructor_node).constructor_info, this);
                        si = si.Next;
                        CompiledConstructorScope tmp = cms;
                        SortedDictionary<int, List<CompiledConstructorScope>> meths = new SortedDictionary<int, List<CompiledConstructorScope>>();
                        if (cms.acc_mod != access_modifer.internal_modifer && cms.acc_mod != access_modifer.private_modifer)
                        {
                            int par_num = cms.mi.GetParameters().Length;
                            meths[par_num] = new List<CompiledConstructorScope>();
                            meths[par_num].Add(cms);
                        }
                        while (si != null)
                        {
                            if (si.access_level != PascalABCCompiler.TreeConverter.access_level.al_internal && si.access_level != PascalABCCompiler.TreeConverter.access_level.al_private && si.sym_info.semantic_node_type == semantic_node_type.compiled_constructor_node)
                            {
                                tmp = new CompiledConstructorScope(new SymInfo("Create", SymbolKind.Method, "Create"), (si.sym_info as compiled_constructor_node).constructor_info, this);
                                //tmp.nextProc = cms;
                                //cms = tmp;
                                int par_num = tmp.mi.GetParameters().Length;
                                if (!meths.ContainsKey(par_num)) meths[par_num] = new List<CompiledConstructorScope>();
                                meths[par_num].Add(tmp);
                            }
                            si = si.Next;
                        }
                        bool beg = false;
                        tmp = null;
                        cms = null;
                        foreach (List<CompiledConstructorScope> lst in meths.Values)
                        {
                            foreach (CompiledConstructorScope m in lst)
                            {
                                if (beg == false)
                                {
                                    tmp = m;
                                    cms = tmp;
                                    beg = true;
                                }
                                else
                                {
                                    tmp.nextProc = m;
                                    tmp = tmp.nextProc as CompiledConstructorScope;
                                }
                            }
                        }
                        return cms;
                    }
                case semantic_node_type.compiled_variable_definition:
                    if (si.access_level != PascalABCCompiler.TreeConverter.access_level.al_internal && si.access_level != PascalABCCompiler.TreeConverter.access_level.al_private)
                        return new CompiledFieldScope(new SymInfo(name, SymbolKind.Field, name), (si.sym_info as compiled_variable_definition).compiled_field, this);
                    break;
                case semantic_node_type.basic_property_node:
                    if (si.access_level != PascalABCCompiler.TreeConverter.access_level.al_internal && si.access_level != PascalABCCompiler.TreeConverter.access_level.al_private)
                        return new CompiledPropertyScope(new SymInfo(name, SymbolKind.Property, name), (si.sym_info as compiled_property_node).property_info, this);
                    break;
                case semantic_node_type.compiled_class_constant_definition:
                    if (si.access_level != PascalABCCompiler.TreeConverter.access_level.al_internal && si.access_level != PascalABCCompiler.TreeConverter.access_level.al_private)
                        return new CompiledFieldScope(new SymInfo(name, SymbolKind.Constant, name), (si.sym_info as compiled_class_constant_definition).field, this);
                    break;
                case semantic_node_type.compiled_event:
                    if (si.access_level != PascalABCCompiler.TreeConverter.access_level.al_internal && si.access_level != PascalABCCompiler.TreeConverter.access_level.al_private)
                        return new CompiledEventScope(new SymInfo(name, SymbolKind.Constant, name), (si.sym_info as compiled_event).event_info, this);
                    break;
                case semantic_node_type.compiled_type_node:
                    if (si.access_level != PascalABCCompiler.TreeConverter.access_level.al_internal && si.access_level != PascalABCCompiler.TreeConverter.access_level.al_private && si.access_level != PascalABCCompiler.TreeConverter.access_level.al_protected)
                        return TypeTable.get_compiled_type(new SymInfo(name, SymbolKind.Type, name), (si.sym_info as compiled_type_node).compiled_type);
                    break;
                //case semantic_node_type.compiled_event: return new CompiledScope(null,(si.sym_info as compiled_event).event_info.);
                //case semantic_node_type.compiled_event: return new CompiledScope(null,(si.sym_info as compiled_event).
            }
            return null;
        }

        public override string ToString()
        {
            return CodeCompletionController.CurrentParser.LanguageInformation.GetFullTypeName(this);
        }
    }

    public class CompiledFieldScope : ElementScope, ICompiledFieldScope
    {
        public FieldInfo fi;
        public bool is_global;
        public List<string> generic_args;

        public CompiledFieldScope(SymInfo si, FieldInfo fi, CompiledScope declaringType)
        {
            this.si = si;
            this.fi = fi;
            string[] args = declaringType.TemplateArguments;
            if (args != null)
            {
                generic_args = new List<string>();
                generic_args.AddRange(args);
            }
            if (generic_args != null)
            {
                //TypeScope ts = declaringType.instances[fi.FieldType.GenericParameterPosition];
                this.sc = CompiledScope.get_type_instance(fi.FieldType, declaringType.instances);
            }
            else
                this.sc = TypeTable.get_compiled_type(new SymInfo(fi.FieldType.Name, SymbolKind.Type, fi.FieldType.FullName), fi.FieldType);
            if (fi.IsLiteral)
                this.cnst_val = fi.GetRawConstantValue();
            if (si.name == null)
                AssemblyDocCache.AddDescribeToComplete(this.si, fi);
            this.si.name = fi.Name;
            this.si.describe = this.ToString();
            //this.si.describe += "\n"+AssemblyDocCache.GetDocumentation(fi.DeclaringType.Assembly,"F:"+fi.DeclaringType.FullName+"."+fi.Name);
            this.topScope = declaringType;
            if (fi.IsPrivate)
            {
                this.acc_mod = access_modifer.private_modifer;
                this.si.acc_mod = access_modifer.private_modifer;
            }
            else if (fi.IsFamily || fi.IsFamilyOrAssembly)
            {
                this.acc_mod = access_modifer.protected_modifer;
                this.si.acc_mod = access_modifer.protected_modifer;
            }
            else if (fi.IsFamilyAndAssembly || fi.IsAssembly)
            {
                this.acc_mod = access_modifer.internal_modifer;
                this.si.acc_mod = access_modifer.internal_modifer;
            }
        }

        public CompiledFieldScope(SymInfo si, FieldInfo fi, CompiledScope declaringType, bool is_global)
        {
            this.si = si;
            this.fi = fi;
            string[] args = declaringType.TemplateArguments;
            if (args != null)
            {
                generic_args = new List<string>();
                generic_args.AddRange(args);
            }
            if (generic_args != null)
            {
                //TypeScope ts = declaringType.instances[fi.FieldType.GenericParameterPosition];
                this.sc = CompiledScope.get_type_instance(fi.FieldType, declaringType.instances);
            }
            else
                this.sc = TypeTable.get_compiled_type(new SymInfo(fi.FieldType.Name, SymbolKind.Type, fi.FieldType.FullName), fi.FieldType);
            if (fi.IsLiteral)
                this.cnst_val = fi.GetRawConstantValue();
            if (si.name == null)
                AssemblyDocCache.AddDescribeToComplete(this.si, fi);
            this.si.name = fi.Name;
            this.is_global = is_global;
            this.si.describe = this.ToString();
            //this.si.describe += "\n"+AssemblyDocCache.GetDocumentation(fi.DeclaringType.Assembly,"F:"+fi.DeclaringType.FullName+"."+fi.Name);
            this.topScope = declaringType;
            if (fi.IsPrivate)
            {
                this.acc_mod = access_modifer.private_modifer;
                this.si.acc_mod = access_modifer.private_modifer;
            }
            else if (fi.IsFamily || fi.IsFamilyOrAssembly)
            {
                this.acc_mod = access_modifer.protected_modifer;
                this.si.acc_mod = access_modifer.protected_modifer;
            }
            else if (fi.IsFamilyAndAssembly || fi.IsAssembly)
            {
                this.acc_mod = access_modifer.internal_modifer;
                this.si.acc_mod = access_modifer.internal_modifer;
            }
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.CompiledField;
            }
        }

        public bool IsGlobal
        {
            get
            {
                return is_global;
            }
        }

        public List<string> GenericArgs
        {
            get
            {
                return generic_args;
            }
        }

        public FieldInfo CompiledField
        {
            get
            {
                return fi;
            }
        }

        public override bool IsReadOnly
        {
            get { return fi.IsInitOnly; }
        }

        public override bool IsEqual(SymScope ts)
        {
            if (ts is CompiledFieldScope && (ts as CompiledFieldScope).fi == this.fi) return true;
            return false;
        }

        public override string ToString()
        {
            return CodeCompletionController.CurrentParser.LanguageInformation.GetDescription(this);
        }
    }

    public class CompiledEventScope : ElementScope, ICompiledEventScope
    {
        public EventInfo ei;
        public List<string> generic_args;

        public CompiledEventScope(SymInfo si, EventInfo ei, CompiledScope declaringType)
        {
            this.si = si;
            this.ei = ei;
            string[] args = declaringType.TemplateArguments;
            if (args != null)
            {
                generic_args = new List<string>();
                generic_args.AddRange(args);
            }
            if (generic_args != null)
            {
                //TypeScope ts = declaringType.instances[ei.EventHandlerType.GenericParameterPosition];
                this.sc = CompiledScope.get_type_instance(ei.EventHandlerType, declaringType.instances);
            }
            else
                this.sc = TypeTable.get_compiled_type(new SymInfo(ei.EventHandlerType.Name, SymbolKind.Type, ei.EventHandlerType.FullName), ei.EventHandlerType);
            if (si.name == null)
                AssemblyDocCache.AddDescribeToComplete(this.si, ei);
            this.si.name = ei.Name;
            this.si.describe = this.ToString();
            this.topScope = declaringType;
            MethodInfo acc_mi = ei.GetAddMethod(true);
            is_static = acc_mi.IsStatic;
            if (acc_mi.IsPrivate)
            {
                this.acc_mod = access_modifer.private_modifer;
                this.si.acc_mod = access_modifer.private_modifer;
            }
            else if (acc_mi.IsFamily || acc_mi.IsFamilyOrAssembly)
            {
                this.acc_mod = access_modifer.protected_modifer;
                this.si.acc_mod = access_modifer.protected_modifer;
            }
            else if (acc_mi.IsFamilyAndAssembly || acc_mi.IsAssembly)
            {
                this.acc_mod = access_modifer.internal_modifer;
                this.si.acc_mod = access_modifer.internal_modifer;
            }
            //this.si.describe += "\n"+AssemblyDocCache.GetDocumentation(ei.DeclaringType.Assembly,"E:"+ei.DeclaringType.FullName+"."+ei.Name);
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.CompiledEvent;
            }
        }

        public List<string> GenericArgs
        {
            get
            {
                return generic_args;
            }
        }

        public override bool IsStatic
        {
            get
            {
                return is_static;
            }
        }

        public EventInfo CompiledEvent
        {
            get
            {
                return ei;
            }
        }

        public override bool IsEqual(SymScope ts)
        {
            if (ts is CompiledEventScope && (ts as CompiledEventScope).ei == this.ei) return true;
            return false;
        }

        public override string ToString()
        {
            //return "event "+ TypeUtility.GetShortTypeName(ei.DeclaringType) +"."+ ei.Name + ": "+sc.ToString();
            return CodeCompletionController.CurrentParser.LanguageInformation.GetDescription(this);
        }
    }

    public class CompiledPropertyScope : ElementScope, ICompiledPropertyScope
    {
        public PropertyInfo pi;
        public List<string> generic_args;

        public CompiledPropertyScope(SymInfo si, PropertyInfo pi, CompiledScope declaringType)
        {
            this.si = si;
            this.pi = pi;
            string[] args = declaringType.TemplateArguments;
            if (args != null)
            {
                generic_args = new List<string>();
                generic_args.AddRange(args);
            }
            if (generic_args != null)
            {
                //TypeScope ts = declaringType.instances[pi.PropertyType.GenericParameterPosition];
                this.sc = CompiledScope.get_type_instance(pi.PropertyType, declaringType.instances);
            }
            else
                this.sc = TypeTable.get_compiled_type(new SymInfo(pi.PropertyType.Name, SymbolKind.Type, pi.PropertyType.FullName), pi.PropertyType);
            if (si.name == null)
                AssemblyDocCache.AddDescribeToComplete(this.si, pi);
            this.si.name = pi.Name;
            this.topScope = declaringType;
            //this.si.describe += "\n"+AssemblyDocCache.GetDocumentation(pi.DeclaringType.Assembly,"P:"+pi.DeclaringType.FullName+"."+pi.Name);
            MethodInfo acc_mi = PascalABCCompiler.NetHelper.NetHelper.GetAnyAccessor(pi);
            if (acc_mi != null && acc_mi == pi.GetGetMethod(true))
            {
                ParameterInfo[] prms = acc_mi.GetParameters();
                if (prms.Length > 0)
                {
                    indexers = new List<TypeScope>();
                    foreach (ParameterInfo p in prms)
                    {
                        indexers.Add(TypeTable.get_compiled_type(new SymInfo(p.ParameterType.Name, SymbolKind.Type, p.ParameterType.Name), p.ParameterType));
                    }
                    Type ret_type = acc_mi.ReturnType;
                    elementType = TypeTable.get_compiled_type(new SymInfo(ret_type.Name, SymbolKind.Type, ret_type.Name), ret_type);
                }
            }
            if (acc_mi != null)
                if (acc_mi.IsPrivate)
                {
                    this.acc_mod = access_modifer.private_modifer;
                    this.si.acc_mod = access_modifer.private_modifer;
                }
                else if (acc_mi.IsFamily || acc_mi.IsFamilyOrAssembly)
                {
                    this.acc_mod = access_modifer.protected_modifer;
                    this.si.acc_mod = access_modifer.protected_modifer;
                }
                else if (acc_mi.IsFamilyAndAssembly || acc_mi.IsAssembly)
                {
                    this.acc_mod = access_modifer.internal_modifer;
                    this.si.acc_mod = access_modifer.internal_modifer;
                }
            this.si.describe = this.ToString();
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.CompiledProperty;
            }
        }

        public List<string> GenericArgs
        {
            get
            {
                return generic_args;
            }
        }

        public PropertyInfo CompiledProperty
        {
            get
            {
                return pi;
            }
        }

        public override bool IsReadOnly
        {
            get { return pi.GetSetMethod(true) == null; }
        }

        public override bool IsEqual(SymScope ts)
        {
            if (ts is CompiledPropertyScope && (ts as CompiledPropertyScope).pi == this.pi) return true;
            return false;
        }

        public override string ToString()
        {
            return CodeCompletionController.CurrentParser.LanguageInformation.GetDescription(this);
        }
    }

    public class CompiledParameterScope : ElementScope
    {
        private ParameterInfo pi;

        public CompiledParameterScope(SymInfo si, ParameterInfo pi)
        {
            this.si = si;
            this.pi = pi;
            this.sc = TypeTable.get_compiled_type(new SymInfo("", SymbolKind.Type, ""), pi.ParameterType);
            if (pi.ParameterType.IsByRef)
                this.param_kind = parametr_kind.var_parametr;
            else if (is_params(pi))
                this.param_kind = parametr_kind.params_parametr;
            //MakeDescription();
        }

        private bool is_params(ParameterInfo _par)
        {
            object[] objarr = _par.GetCustomAttributes(typeof(ParamArrayAttribute), true);
            if ((objarr == null) || (objarr.Length == 0))
            {
                return false;
            }
            return true;
        }
    }

    public class CompiledMethodScope : ProcScope, ICompiledMethodScope
    {
        public MethodInfo mi;
        public bool is_global;
        //private CompiledScope ret_type;
        

        public CompiledMethodScope(SymInfo si, MethodInfo mi, CompiledScope declaringType)
        {
            this.si = si;
            this.mi = mi;
            string[] args = declaringType.TemplateArguments;

            if (args != null)
            {
                generic_args = new List<string>();
                generic_args.AddRange(args);
            }
            if (mi.GetGenericArguments().Length > 0)
            {
                
            }
            if (mi.ReturnType != typeof(void))
            {
                if (generic_args != null /*&& mi.GetGenericArguments().Length == 0*/)
                {
                    this.return_type = CompiledScope.get_type_instance(mi.ReturnType, declaringType.instances);
                }
                else
                    return_type = TypeTable.get_compiled_type(new SymInfo(mi.ReturnType.Name, SymbolKind.Type, mi.ReturnType.FullName), mi.ReturnType);
            }
            parameters = new List<ElementScope>();
            int i = 0;
            is_extension = PascalABCCompiler.NetHelper.NetHelper.IsExtensionMethod(mi);
            foreach (ParameterInfo pi in mi.GetParameters())
            {
                parameters.Add(new CompiledParameterScope(new SymInfo(pi.Name, SymbolKind.Parameter, pi.Name), pi));
            }
            if (si.name == null)
                AssemblyDocCache.AddDescribeToComplete(this.si, mi);
            this.si.name = CodeCompletionController.CurrentParser.LanguageInformation.GetShortTypeName(this);
            this.si.describe = this.ToString();
            //this.si.describe += "\n"+AssemblyDocCache.GetDocumentation(mi.DeclaringType.Assembly,"M:"+mi.DeclaringType.FullName+"."+mi.Name+GetParamNames());
            this.topScope = declaringType;
            if (mi.IsPrivate)
            {
                this.si.acc_mod = access_modifer.private_modifer;
                this.acc_mod = access_modifer.private_modifer;
            }
            else if (mi.IsFamily || mi.IsFamilyOrAssembly)
            {
                this.acc_mod = access_modifer.protected_modifer;
                this.si.acc_mod = access_modifer.protected_modifer;
            }
            else if (mi.IsFamilyAndAssembly || mi.IsAssembly)
            {
                this.acc_mod = access_modifer.internal_modifer;
                this.si.acc_mod = access_modifer.internal_modifer;
            }
        }

        public CompiledMethodScope(SymInfo si, MethodInfo mi, CompiledScope declaringType, bool is_global)
        {
            this.si = si;
            this.mi = mi;
            string[] args = declaringType.TemplateArguments;
            if (args != null)
            {
                generic_args = new List<string>();
                generic_args.AddRange(args);
            }
            if (mi.ReturnType != typeof(void))
                return_type = TypeTable.get_compiled_type(new SymInfo(mi.ReturnType.Name, SymbolKind.Type, mi.ReturnType.FullName), mi.ReturnType);
            parameters = new List<ElementScope>();
            int i = 0;
            is_extension = PascalABCCompiler.NetHelper.NetHelper.IsExtensionMethod(mi);
            foreach (ParameterInfo pi in mi.GetParameters())
            {
                parameters.Add(new CompiledParameterScope(new SymInfo(pi.Name, SymbolKind.Parameter, pi.Name), pi));
            }
            this.is_global = is_global;
            if (si.name == null)
                AssemblyDocCache.AddDescribeToComplete(this.si, mi);
            this.si.name = CodeCompletionController.CurrentParser.LanguageInformation.GetShortTypeName(this);
            this.si.describe = this.ToString();
            //this.si.describe += "\n"+AssemblyDocCache.GetDocumentation(mi.DeclaringType.Assembly,"M:"+mi.DeclaringType.FullName+"."+mi.Name+GetParamNames());
            this.topScope = declaringType;
            if (mi.IsPrivate)
            {
                this.si.acc_mod = access_modifer.private_modifer;
                this.acc_mod = access_modifer.private_modifer;
            }
            else if (mi.IsFamily || mi.IsFamilyOrAssembly)
            {
                this.acc_mod = access_modifer.protected_modifer;
                this.si.acc_mod = access_modifer.protected_modifer;
            }
            else if (mi.IsFamilyAndAssembly || mi.IsAssembly)
            {
                this.acc_mod = access_modifer.internal_modifer;
                this.si.acc_mod = access_modifer.internal_modifer;
            }
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.CompiledMethod;
            }
        }

        public List<string> GenericArgs
        {
            get
            {
                return generic_args;
            }
        }

        public bool IsGlobal
        {
            get
            {
                return is_global;
            }
        }

        public override string Name
        {
            get { return mi.Name; }
        }

        public MethodInfo CompiledMethod
        {
            get
            {
                return mi;
            }
        }

        public override bool IsAbstract
        {
            get { return mi.IsAbstract && !mi.DeclaringType.IsInterface; }
        }

        public override bool IsStatic
        {
            get { return mi.IsStatic && !is_extension; }
        }

        public override bool IsVirtual
        {
            get { return mi.IsVirtual; }
        }

        public override bool IsConstructor()
        {
            return false;
        }

        public override int GetParametersCount()
        {
            return mi.GetParameters().Length;
        }

        public override SymInfo[] GetNames()
        {
            return return_type.GetNames();
        }

        //		public override SymInfo[] GetNames(ExpressionVisitor ev)
        //		{
        //			return return_type.GetNames();
        //		}

        public override SymScope FindName(string name)
        {
            return return_type.FindName(name);
        }

        public override SymScope FindNameOnlyInType(string name)
        {
            return FindName(name);
        }

        public override bool IsEqual(SymScope ts)
        {
            CompiledMethodScope cms = ts as CompiledMethodScope;
            if (cms == null) return false;
            if (cms.mi == this.mi) return true;
            if (cms.nextProc != null) return IsEqual(cms.nextProc);
            return false;
        }

        public override bool IsParamsEquals(ProcScope ps)
        {
            if (ps is CompiledMethodScope || ps is CompiledConstructorScope)
                return false;
            try
            {
                ParameterInfo[] prms = this.mi.GetParameters();
                if (ps.parameters == null)
                {
                    if (prms.Length != 0) return false;
                    if (ps.return_type == null)
                    {
                        if (this.mi.ReturnType == typeof(void))
                            return true;
                        else
                            return false;
                    }
                    if (this.mi.ReturnType == null)
                        return false;
                    return ps.return_type.IsEqual(TypeTable.get_compiled_type(new SymInfo("", SymbolKind.Class, ""), this.mi.ReturnType));
                }
                if (prms.Length != ps.parameters.Count)
                    return false;
                for (int i = 0; i < prms.Length; i++)
                {
                    if (!ps.parameters[i].sc.IsEqual(TypeTable.get_compiled_type(new SymInfo("", SymbolKind.Class, ""), prms[i].ParameterType)))
                        return false;
                }
                if (ps.return_type == null)
                {
                    if (this.mi.ReturnType == typeof(void))
                        return true;
                    else
                        return false;
                }
                if (this.mi.ReturnType == null)
                    return false;
                return ps.return_type.IsEqual(TypeTable.get_compiled_type(new SymInfo("", SymbolKind.Class, ""), this.mi.ReturnType));
            }
            catch
            {
                return false;
            }
        }

        public override string ToString()
        {
            return CodeCompletionController.CurrentParser.LanguageInformation.GetDescription(this);
        }
    }

    public class CompiledConstructorScope : ProcScope, ICompiledConstructorScope
    {
        public ConstructorInfo mi;
        //private CompiledScope ret_type;

        public override bool IsConstructor()
        {
            return true;
        }

        public CompiledConstructorScope(SymInfo si, ConstructorInfo mi, CompiledScope declaringType)
        {
            this.si = si;
            this.mi = mi;
            //if (mi.ReturnType != typeof(void))
            return_type = TypeTable.get_compiled_type(new SymInfo(mi.DeclaringType.Name, SymbolKind.Type, mi.DeclaringType.FullName), mi.DeclaringType);
            if (si.name == null)
                AssemblyDocCache.AddDescribeToComplete(this.si, mi);

            parameters = new List<ElementScope>();
            foreach (ParameterInfo pi in mi.GetParameters())
                parameters.Add(new CompiledParameterScope(new SymInfo(pi.Name, SymbolKind.Parameter, pi.Name), pi));

            this.si.name = CodeCompletionController.CurrentParser.LanguageInformation.GetShortTypeName(this);
            this.si.describe = this.ToString();//+"\n"+AssemblyDocCache.GetDocumentation(mi.DeclaringType.Assembly,"M:"+mi.DeclaringType.FullName+".#ctor"+GetParamNames());
            this.topScope = declaringType;
            if (mi.IsPrivate)
            {
                this.si.acc_mod = access_modifer.private_modifer;
                this.acc_mod = access_modifer.private_modifer;
            }
            else if (mi.IsFamily || mi.IsFamilyOrAssembly)
            {
                this.acc_mod = access_modifer.protected_modifer;
                this.si.acc_mod = access_modifer.protected_modifer;
            }
            else if (mi.IsFamilyAndAssembly || mi.IsAssembly)
            {
                this.acc_mod = access_modifer.internal_modifer;
                this.si.acc_mod = access_modifer.internal_modifer;
            }
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.CompiledConstructor;
            }
        }

        public ConstructorInfo CompiledConstructor
        {
            get
            {
                return mi;
            }
        }

        public IBaseScope Type
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int GetParametersCount()
        {
            return mi.GetParameters().Length;
        }

        public override SymInfo[] GetNames()
        {
            return return_type.GetNames();
        }

        //		public override SymInfo[] GetNames(ExpressionVisitor ev)
        //		{
        //			return return_type.GetNames();
        //		}

        public override SymScope FindName(string name)
        {
            return return_type.FindName(name);
        }

        public override SymScope FindNameOnlyInType(string name)
        {
            return FindName(name);
        }

        public override bool IsEqual(SymScope ts)
        {
            CompiledConstructorScope cms = ts as CompiledConstructorScope;
            if (cms == null) return false;
            if (cms.mi == this.mi) return true;
            if (cms.nextProc != null) return IsEqual(cms.nextProc);
            return false;
        }

        public override bool IsParamsEquals(ProcScope ps)
        {
            if (ps is CompiledMethodScope || ps is CompiledConstructorScope)
                return false;
            if ((ps.parameters == null || ps.parameters.Count == 0) && this.mi.GetParameters().Length == 0)
                return true;
            return false;
        }

        public override string ToString()
        {
            return CodeCompletionController.CurrentParser.LanguageInformation.GetDescription(this);
        }
    }

    public class WithScope : SymScope
    {
        class TypeScopeDesc
        {
            public SymScope ss;
            public bool is_type;

            public TypeScopeDesc(SymScope ss, bool is_type)
            {
                this.ss = ss;
                this.is_type = is_type;
            }
        }

        private List<TypeScopeDesc> with_scopes = new List<TypeScopeDesc>();

        public WithScope(SymScope topScope)
        {
            this.topScope = topScope;
            this.si = new SymInfo("$with_block", SymbolKind.Block, "$with_block");
        }

        public void AddWithScope(SymScope ss, bool is_type)
        {
            with_scopes.Add(new TypeScopeDesc(ss, is_type));
        }

        public override SymScope FindNameInAnyOrder(string name)
        {
            for (int i = 0; i < with_scopes.Count; i++)
            {
                SymScope ss = with_scopes[i].ss.FindNameOnlyInType(name);
                if (ss != null) return ss;
            }
            return topScope.FindNameInAnyOrder(name);
        }

        public override SymInfo[] GetNames()
        {
            List<SymInfo> lst = new List<SymInfo>();
            for (int i = 0; i < with_scopes.Count; i++)
            {
                lst.AddRange(with_scopes[i].ss.GetNamesAsInObject());
            }
            lst.AddRange(topScope.GetNames());
            return lst.ToArray();
        }

        //		public override SymInfo[] GetNames(ExpressionVisitor ev)
        //		{
        //			List<SymInfo> lst = new List<SymInfo>();
        //			for (int i=0; i<with_scopes.Count; i++)
        //			{
        //				if (!with_scopes[i].is_type)
        //				lst.AddRange(with_scopes[i].ss.GetNamesAsInObject(ev));
        //				else
        //				lst.AddRange((with_scopes[i].ss as TypeScope).GetNames(ev,PascalABCCompiler.Parsers.KeywordKind.None));
        //			}
        //			lst.AddRange(topScope.GetNames(ev));
        //			return lst.ToArray();
        //		}


        public override SymInfo[] GetNamesInAllTopScopes(bool all_names, ExpressionVisitor ev, bool is_static)
        {
            List<SymInfo> lst = new List<SymInfo>();
            for (int i = 0; i < with_scopes.Count; i++)
            {
                if (!with_scopes[i].is_type)
                    lst.AddRange(with_scopes[i].ss.GetNamesAsInObject(ev));
                else
                    lst.AddRange((with_scopes[i].ss as TypeScope).GetNames(ev, PascalABCCompiler.Parsers.KeywordKind.None, false));
            }
            lst.AddRange(topScope.GetNamesInAllTopScopes(all_names, ev, is_static));
            return lst.ToArray();
        }

        public override SymScope FindName(string name)
        {
            for (int i = 0; i < with_scopes.Count; i++)
            {
                SymScope ss = with_scopes[i].ss.FindNameOnlyInType(name);
                if (ss != null) return ss;
            }
            return topScope.FindName(name);
        }

        public override List<SymScope> FindOverloadNames(string name)
        {
            List<SymScope> names = new List<SymScope>();
            for (int i = 0; i < with_scopes.Count; i++)
            {
                names.AddRange(with_scopes[i].ss.FindOverloadNamesOnlyInType(name));
            }
            names.AddRange(topScope.FindOverloadNames(name));
            return names;
        }

        public override void AddName(string name, SymScope sc)
        {
            if (members == null) members = new List<SymScope>();
            base.AddName(name, sc);
        }

        public override SymScope FindNameOnlyInType(string name)
        {
            return FindName(name);
        }

        public override List<SymScope> FindOverloadNamesOnlyInType(string name)
        {
            return FindOverloadNames(name);
        }
    }

    public class BlockScope : SymScope, IBlockScope
    {
        public BlockScope(SymScope topScope)
        {
            this.topScope = topScope;
            this.si = new SymInfo("$block_scope", SymbolKind.Block, "$block_scope");
            this.members = new List<SymScope>();
        }

        public override ScopeKind Kind
        {
            get
            {
                return ScopeKind.Block;
            }
        }
    }

    public class UnknownScope : TypeScope
    {
        public UnknownScope(SymInfo si)
        {
            this.si = si;
            this.name = si.name;
        }

        public override bool IsEqual(SymScope ts)
        {
            return string.Compare(this.si.name, ts.si.name, true) == 0;
        }

        public override SymScope FindName(string name)
        {
            return null;
        }

        public override List<SymScope> FindOverloadNames(string name)
        {
            return new List<SymScope>(0);
        }

        public override TypeScope GetInstance(List<TypeScope> gen_args)
        {
            return this;
        }

        public override List<SymScope> FindOverloadNamesOnlyInType(string name)
        {
            return new List<SymScope>(0);
        }

        public override SymScope FindNameInAnyOrder(string name)
        {
            return null;
        }

        public override SymScope FindNameOnlyInThisType(string name)
        {
            return null;
        }

        public override SymScope FindNameOnlyInType(string name)
        {
            return null;
        }

        public override SymInfo[] GetNames()
        {
            return new SymInfo[0];
        }

        //		public override SymInfo[] GetNames(ExpressionVisitor ev)
        //		{
        //			return new SymInfo[0];
        //		}

        public override SymInfo[] GetNamesAsInObject()
        {
            return new SymInfo[0];
        }

        public override SymInfo[] GetNamesAsInObject(ExpressionVisitor ev)
        {
            return new SymInfo[0];
        }

        public override SymInfo[] GetNamesInAllTopScopes(bool all_names, ExpressionVisitor ev, bool is_static)
        {
            return new SymInfo[0];
        }

        public override string ToString()
        {
            return si.describe;
        }
    }
    /*public class ClassScope : BaseScope
    {
        public override SymInfo[] GetName(string name)
        {
			
        }
		
        public override BaseScope FindName(string name)
        {
			
        }
    }*/
}
