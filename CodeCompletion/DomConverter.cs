// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using PascalABCCompiler.SyntaxTree;
using System.Reflection;
using PascalABCCompiler;
using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.TreeRealization;
using PascalABCCompiler.Parsers;

namespace CodeCompletion
{
    public struct RetValue
    {
        public object prim_val;
    }

    public class DomConverter : ICodeCompletionDomConverter
    {
        public DomSyntaxTreeVisitor visitor;
        //TreeConverterSymbolTable tcst;
        public CodeCompletionController controller;
        public bool is_compiled = false;
        public static SymInfo[] standard_units;
        private Hashtable cur_used_assemblies;
        public CompilationUnit unit;

        private void InitModules()
        {
            try
            {
                string[] files = Directory.GetFiles(CodeCompletionController.comp.CompilerOptions.SearchDirectory, "*.pas");
                standard_units = new SymInfo[files.Length];
                int i = 0;
                foreach (string s in files)
                {
                    string fname = Path.GetFileNameWithoutExtension(s);
                    if (fname == "__RedirectIOMode" || fname == "__RunMode" || fname == "PABCExtensions")
                        continue;
                    SymInfo si = new SymInfo(Path.GetFileNameWithoutExtension(s), SymbolKind.Namespace, null);
                    si.IsUnitNamespace = true;

                    si.description = GetUnitDescription(s);
                    standard_units[i++] = si;
                }
            }
            catch (Exception e)
            {
                //standard_units = new SymInfo[0];
            }
        }

        private string GetUnitDescription(string fileName)
        {
            var reader = File.OpenText(fileName);
            string line = null;
            string doc = "";
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                if (line.StartsWith("///"))
                    doc += line.Substring(3) + Environment.NewLine;
                else if (line.StartsWith("unit"))
                    break;
            }
            reader.Close();
            return doc;
        }

        bool ICodeCompletionDomConverter.IsCompiled
        {
            get
            {
                return is_compiled;
            }
        }

        IBaseScope ICodeCompletionDomConverter.EntryScope
        {
            get
            {
                return visitor.entry_scope;
            }
        }

        public DomConverter(CodeCompletionController controller)
        {
            visitor = new DomSyntaxTreeVisitor(this);
            this.controller = controller;
            if (standard_units == null) InitModules();
        }

        public void ConvertToDom(compilation_unit cu)
        {
            CodeCompletionController.comp.CompilerOptions.SourceFileName = cu.file_name;
            visitor.Convert(cu);
            is_compiled = true;
            cur_used_assemblies = (Hashtable)PascalABCCompiler.NetHelper.NetHelper.cur_used_assemblies.Clone();
            return;
        }

        public bool IsAssembliesChanged()
        {
            return visitor.entry_scope.IsAssembliesChanged();
        }

        public definition_node[] GetNamesAfterDot(expression expr, string str, int line, int col, PascalABCCompiler.Parsers.KeywordKind keyword, ref definition_node root)
        {
            unit.SemanticTree.find_by_location(line, col);
            return null;
        }

        /// <summary>
        /// Получение имен после точки
        /// </summary>
        public SymInfo[] GetName(expression expr, string str, int line, int col, PascalABCCompiler.Parsers.KeywordKind keyword, ref SymScope root)
        {
            if (visitor.cur_scope == null) return null;
            if (col + 1 > str.Length)
                col -= str.Length;
            SymScope si = visitor.FindScopeByLocation(line + 1, col);//stv.cur_scope;
            if (si == null)
            {
                si = visitor.FindScopeByLocation(line, col + 1);
                if (si == null)
                    return null;
            }
            SetCurrentUsedAssemblies();
            ExpressionVisitor ev = new ExpressionVisitor(expr, si, visitor);
            si = ev.GetScopeOfExpression(true, false);
            root = si;
            if (si is ElementScope) root = (si as ElementScope).sc;
            else if (si is ProcScope) root = (si as ProcScope).return_type;
            if (si != null)
            {
                if (!(si is TypeScope) && !(si is NamespaceScope))
                {
                    SymInfo[] syms = si.GetNamesAsInObject(ev);
                    SymInfo[] ext_syms = null;
                    if (si is ElementScope)
                    {
                        SymScope root_scope = visitor.entry_scope;
                        //if (root_scope is ImplementationUnitScope || root_scope is BlockScope)
                        //    root_scope = root_scope.topScope;
                        ext_syms = root_scope.GetSymInfosForExtensionMethods((si as ElementScope).sc as TypeScope);
                    }
                        
                    List<SymInfo> lst = new List<SymInfo>();
                    lst.AddRange(syms);
                    if (ext_syms != null)
                        lst.AddRange(ext_syms);
                    RestoreCurrentUsedAssemblies();
                    List<SymInfo> lst_to_remove = new List<SymInfo>();
                    foreach (SymInfo si2 in lst)
                        if (si2.name.StartsWith("operator"))
                            lst_to_remove.Add(si2);
                    foreach (SymInfo si2 in lst_to_remove)
                        lst.Remove(si2);
                    return lst.ToArray();
                }
                else
                {
                    if (si is TypeScope)
                    {
                        RestoreCurrentUsedAssemblies();
                        return (si as TypeScope).GetNames(ev, keyword, false);
                    }
                    else
                    {
                        if (ev.entry_scope.InUsesRange(line + 1, col + 1))
                            keyword = PascalABCCompiler.Parsers.KeywordKind.Uses;
                        RestoreCurrentUsedAssemblies();
                        return (si as NamespaceScope).GetNames(ev, keyword);
                    }
                }
            }
            RestoreCurrentUsedAssemblies();
            return null;
        }

        public SymInfo[] GetOverridableMethods(int line, int col)
        {
            if (visitor.cur_scope == null) return null;
            SymScope ss = visitor.FindScopeByLocation(line + 1, col + 1);//stv.cur_scope;
            if (ss == null) return null;
            List<SymInfo> meths = new List<SymInfo>();
            SetCurrentUsedAssemblies();
            if (ss is TypeScope && ((ss as TypeScope).kind == SymbolKind.Class || (ss as TypeScope).kind == SymbolKind.Struct))
            {
                TypeScope ts = ss as TypeScope;
                if (ts.baseScope != null)
                {
                    List<ProcScope> procs = ts.baseScope.GetOverridableMethods();
                    for (int i = 0; i < procs.Count; i++)
                    {
                        SymScope elem = ts.FindNameOnlyInThisType(procs[i].Name);
                        if (elem == null || !(elem is ProcScope))
                        {
                            int pos = 0;
                            string full_name = CodeCompletionController.CurrentParser.LanguageInformation.ConstructOverridedMethodHeader(procs[i], out pos);
                            SymInfo si = new SymInfo(full_name.Substring(pos), procs[i].si.kind, full_name);
                            si.acc_mod = procs[i].si.acc_mod;
                            meths.Add(si);
                        }
                    }
                }
            }
            RestoreCurrentUsedAssemblies();
            return meths.ToArray();
        }

        public ProcScope[] GetNotImplementedMethodHeaders(int line, int col, ref Position pos)
        {
            if (visitor.cur_scope == null) return null;
            SymScope ss = visitor.FindScopeByLocation(line + 1, col + 1);//stv.cur_scope;
            if (ss == null) return null;
            List<ProcScope> meths = new List<ProcScope>();
            pos.file_name = this.visitor.doc.file_name;
            SetCurrentUsedAssemblies();
            if (ss is TypeScope && (ss as TypeScope).kind == SymbolKind.Class)
            {
                TypeScope ts = ss as TypeScope;
                pos.line = ts.real_body_loc.begin_line_num;
                pos.column = ts.real_body_loc.begin_column_num;
                if (ts.baseScope != null)
                {
                    List<ProcScope> procs = ts.baseScope.GetAbstractMethods();
                    for (int i = 0; i < procs.Count; i++)
                    {
                        SymScope elem = ts.FindNameOnlyInThisType(procs[i].Name);
                        if (elem == null || !(elem is ProcScope))
                        {
                            meths.Add(new ProcRealization(procs[i], ts));
                        }
                    }
                }
                if (ts.implemented_interfaces != null)
                {
                    for (int i = 0; i < ts.implemented_interfaces.Count; i++)
                    {
                        List<ProcScope> procs = ts.implemented_interfaces[i].GetAbstractMethods();
                        for (int j = 0; j < procs.Count; j++)
                        {
                            SymScope elem = ts.FindNameOnlyInThisType(procs[j].Name);
                            if (elem == null || !(elem is ProcScope))
                            {
                                meths.Add(new ProcRealization(procs[j], ts));
                            }
                            else if (elem is ProcScope)
                            {
                                ProcScope ps = elem as ProcScope;
                                bool is_impl = false;
                                while (ps != null)
                                    if (ps.IsEqual(elem))
                                    {
                                        is_impl = true;
                                        break;
                                    }
                                    else ps = ps.nextProc;
                                if (!is_impl)
                                    meths.Add(new ProcRealization(procs[j], ts));
                            }
                        }
                    }
                }
            }
            RestoreCurrentUsedAssemblies();
            return meths.ToArray();
        }

        /// <summary>
        /// Получение списка нереализованных методов
        /// </summary>
        public ProcScope[] GetNotImplementedMethods(int line, int col, ref Position pos)
        {
            try
            {
                if (visitor.cur_scope == null) return null;
                SymScope ss = visitor.FindScopeByLocation(line + 1, col + 1);//stv.cur_scope;
                if (ss == null) return null;
                List<ProcScope> meths = new List<ProcScope>();
                pos.file_name = this.visitor.doc.file_name;
                SetCurrentUsedAssemblies();
                if (ss is TypeScope && ((ss as TypeScope).kind == SymbolKind.Class || (ss as TypeScope).kind == SymbolKind.Struct))
                {
                    TypeScope ts = ss as TypeScope;

                    if (ts.topScope is InterfaceUnitScope)
                    {
                        SymScope impl_scope = (ts.topScope as InterfaceUnitScope).impl_scope;
                        if (impl_scope != null)
                        {
                            pos.line = impl_scope.loc.end_line_num;
                            pos.column = impl_scope.loc.end_column_num + 2;
                        }
                        else
                        {
                            pos.line = ts.loc.end_line_num;
                            pos.column = ts.loc.end_column_num + 2;
                        }
                    }
                    else
                    {
                        pos.line = ts.loc.end_line_num;
                        pos.column = ts.loc.end_column_num + 2;
                    }
                    foreach (SymScope symsc in ts.members)
                    {
                        if (symsc is ProcScope && !(symsc as ProcScope).already_defined && symsc.loc != null)
                        {
                            meths.Add(symsc as ProcScope);
                        }
                    }
                }
                else if (ss is ProcScope)
                {
                    TypeScope ts = (ss as ProcScope).topScope as TypeScope;
                    if (ts != null && (ts.kind == SymbolKind.Class || ts.kind == SymbolKind.Struct))
                    {
                        if (ts.topScope is InterfaceUnitScope)
                        {
                            SymScope impl_scope = (ts.topScope as InterfaceUnitScope).impl_scope;
                            if (impl_scope != null)
                            {
                                pos.line = impl_scope.loc.end_line_num;
                                pos.column = impl_scope.loc.end_column_num + 2;
                            }
                            else
                            {
                                pos.line = ts.loc.end_line_num;
                                pos.column = ts.loc.end_column_num + 2;
                            }
                        }
                        else
                        {
                            pos.line = ts.loc.end_line_num;
                            pos.column = ts.loc.end_column_num + 2;
                        }
                        foreach (SymScope symsc in ts.members)
                        {
                            if (symsc is ProcScope && !(symsc as ProcScope).already_defined && symsc.loc != null)
                            {
                                meths.Add(symsc as ProcScope);
                            }
                        }
                    }
                    else if (ts == null && (ss as ProcScope).topScope is InterfaceUnitScope)
                    {
                        SymScope impl_scope = ((ss as ProcScope).topScope as InterfaceUnitScope).impl_scope;
                        if (impl_scope != null)
                        {
                            pos.line = impl_scope.loc.end_line_num;
                            pos.column = impl_scope.loc.end_column_num + 2;
                            if (!(ss as ProcScope).already_defined && ss.loc != null)
                                meths.Add(ss as ProcScope);
                        }
                    }
                }
                RestoreCurrentUsedAssemblies();
                if (meths.Count > 0) return meths.ToArray();
            }
            catch
            {

            }
            RestoreCurrentUsedAssemblies();
            return null;
        }

        /// <summary>
        /// Получение описания символа
        /// </summary>
        public IBaseScope GetSymDefinition(expression expr, int line, int col, PascalABCCompiler.Parsers.KeywordKind keyword)
        {
            if (visitor.cur_scope == null) return null;
            SymScope ss = visitor.FindScopeByLocation(line + 1, col + 1);//stv.cur_scope;
            if (ss == null) return null;
            bool on_proc = false;
            if (keyword == PascalABCCompiler.Parsers.KeywordKind.Function || keyword == PascalABCCompiler.Parsers.KeywordKind.Constructor || keyword == PascalABCCompiler.Parsers.KeywordKind.Destructor)
            {
                if (ss is ProcRealization)
                {
                    if (expr is ident)
                    {
                        if ((expr as ident).name == (ss as ProcRealization).def_proc.si.name) on_proc = true;
                    }
                    else on_proc = true;
                }
                else if (ss is ProcScope)
                {
                    if (expr is ident)
                    {
                        if ((expr as ident).name == (ss as ProcScope).si.name) on_proc = true;
                    }
                    else on_proc = true;
                }
            }
            //if (!((keyword == KeywordKind.kw_proc || keyword == KeywordKind.kw_constr || keyword == KeywordKind.kw_destr) && ss is ProcScope))
            if (!on_proc)
            {
                SetCurrentUsedAssemblies();
                ExpressionVisitor ev = new ExpressionVisitor(expr, ss, visitor);
                ss = ev.GetScopeOfExpression();
                RestoreCurrentUsedAssemblies();
            }
            return ss;
        }

        /// <summary>
        /// Нахождение области видимости по координатам курсора
        /// </summary>
        public SymScope FindScopeByLocation(int line, int col)
        {
            if (visitor.entry_scope != null)
                return visitor.entry_scope.FindScopeByLocation(line, col);
            return null;
        }

        /// <summary>
        /// Получение списка пространств имен
        /// </summary>
        public SymInfo[] GetNamespaces()
        {
            SymInfo[] elems = visitor.entry_scope.GetNamesInAllTopScopes(true, null, false);
            List<SymInfo> result_names = new List<SymInfo>();
            result_names.AddRange(standard_units);
            if (elems == null) return null;
            for (int i = 0; i < elems.Length; i++)
            {
                if (elems[i].kind == SymbolKind.Namespace && !elems[i].IsUnitNamespace)
                    result_names.Add(elems[i]);
            }
            return result_names.ToArray();
        }

        /// <summary>
        /// Получение имен, начинающихся с pattern
        /// </summary>
        public SymInfo[] GetNameByPattern(string pattern, int line, int col, bool all_names, int nest_level)
        {
            if (visitor.cur_scope == null) return null;
            SymScope si = visitor.FindScopeByLocation(line + 1, col + 1);
            if (si == null)
            {
                si = visitor.FindScopeByLocation(line, col + 1);
                if (si == null)
                    si = visitor.cur_scope;
            }

            SymInfo[] elems = null;
            if (si == null) return null;
            if (pattern == null || pattern == "")
                elems = si.GetNamesInAllTopScopes(all_names, new ExpressionVisitor(si, visitor), false);
            else
                elems = si.GetNamesInAllTopScopes(all_names, new ExpressionVisitor(si, visitor), false);
            List<SymInfo> result_names = new List<SymInfo>();
            if (elems == null) return null;
            for (int i = 0; i < elems.Length; i++)
                if (pattern == null || pattern == "")
                {
                    if (!elems[i].name.StartsWith("$") && !elems[i].name.StartsWith("<"))
                        if (all_names)
                        {
                            if (elems[i].kind != SymbolKind.Namespace || nest_level == 0)
                                result_names.Add(elems[i]);
                            else
                            {
                                string name = elems[i].name;
                                string tmp = name;
                                int num_dot = 0;
                                for (int j = 0; j < nest_level; j++)
                                {
                                    int pos = tmp.IndexOf('.');
                                    if (pos != -1)
                                    {
                                        num_dot++;
                                        tmp = tmp.Substring(pos + 1, tmp.Length - pos - 1);
                                    }
                                    else break;
                                }
                                if (num_dot < nest_level)
                                    result_names.Add(elems[i]);
                            }
                        }
                        else
                        {
                            if (elems[i].kind != SymbolKind.Namespace) result_names.Add(elems[i]);
                            else if (elems[i].name.IndexOf('.') == -1) result_names.Add(elems[i]);
                        }
                }
                else
                if (elems[i].name.StartsWith(pattern, true, System.Globalization.CultureInfo.CurrentCulture))
                    result_names.Add(elems[i]);
            return result_names.ToArray();
        }

        /// <summary>
        /// Получение типов, начинающихся с pattern
        /// </summary>
        public SymInfo[] GetTypeByPattern(string pattern, int line, int col, bool all_names, int nest_level)
        {
            if (visitor.cur_scope == null) return null;
            SymScope si = visitor.FindScopeByLocation(line + 1, col + 1);//stv.cur_scope;
            SymInfo[] elems = null;
            if (si == null)
            {
                elems = visitor.entry_scope.GetNamesInAllTopScopes(all_names, null, false);
            }
            else
            if (pattern == null || pattern == "") elems = si.GetNamesInAllTopScopes(all_names, new ExpressionVisitor(si, visitor), false);
            else elems = si.GetNamesInAllTopScopes(all_names, new ExpressionVisitor(si, visitor), false);
            List<SymInfo> result_names = new List<SymInfo>();
            if (elems == null) return null;
            for (int i = 0; i < elems.Length; i++)
                if (pattern != null && pattern != "")
                {
                    if (!elems[i].name.StartsWith("$"))
                        if (all_names)
                        {
                            if (elems[i].kind == SymbolKind.Namespace || elems[i].kind == SymbolKind.Class || elems[i].kind == SymbolKind.Type
                                || elems[i].kind == SymbolKind.Struct || elems[i].kind == SymbolKind.Delegate || elems[i].kind == SymbolKind.Enum
                                || elems[i].kind == SymbolKind.Interface)
                                result_names.Add(elems[i]);
                        }
                }
            return result_names.ToArray();
        }

        /// <summary>
        /// Выдает все типы (используется после new)
        /// </summary>
        public SymInfo[] GetTypes(expression expr, int line, int col, out SymInfo out_si)
        {
            out_si = null;
            if (visitor.cur_scope == null) return null;
            SymScope si = visitor.FindScopeByLocation(line + 1, col + 1);//stv.cur_scope;
            SymInfo[] elems = null;
            if (si == null) return null;
            elems = si.GetNamesInAllTopScopes(true, new ExpressionVisitor(si, visitor), false);
            if (expr != null)
            {
                ExpressionVisitor ev = new ExpressionVisitor(expr, si, visitor);
                si = ev.GetScopeOfExpression();
                if (si is TypeScope)
                    si = new ElementScope(si);
            }
            List<SymInfo> result_names = new List<SymInfo>();
            if (elems == null) return null;
            for (int i = 0; i < elems.Length; i++)
            {
                if (!elems[i].name.StartsWith("$") && (elems[i].kind == SymbolKind.Class || elems[i].kind == SymbolKind.Struct || elems[i].kind == SymbolKind.Namespace) && elems[i].kind != SymbolKind.Interface)
                {
                    if (expr != null && si != null && si is ElementScope &&
                    string.Compare(elems[i].name, (si as ElementScope).sc.si.name, true) == 0)
                    {
                        out_si = elems[i];
                        //out_si = new SymInfo(elems[i].name,elems[i].kind,elems[i].describe);
                        string s = CodeCompletionController.CurrentParser.LanguageInformation.GetSimpleDescriptionWithoutNamespace((si as ElementScope).sc as PascalABCCompiler.Parsers.ITypeScope);
                        if (s != out_si.name)
                            out_si.addit_name = s;
                    }
                    result_names.Add(elems[i]);
                }
            }
            if (out_si == null && expr != null && si != null && si is ElementScope)
            {
                out_si = (si as ElementScope).sc.si;
                if (!out_si.name.StartsWith("$") && out_si.kind != SymbolKind.Interface)
                {
                    out_si.name = (si as ElementScope).sc.GetFullName();
                    result_names.Add(out_si);
                }
                else
                {
                    out_si = null;
                }
            }
            return result_names.ToArray();
        }

        private string get_references(Type t, out int lines)
        {
            lines = 0;
            StringBuilder assemblies = new StringBuilder();
            if (t.Module.ScopeName != "CommonLanguageRuntimeLibrary")
            {
                assemblies.AppendLine("{$reference " + t.Module.ScopeName + "}");
                lines++;
            }
            AssemblyName[] assm_names = t.Assembly.GetReferencedAssemblies();
            for (int i = 0; i < assm_names.Length; i++)
                if (assm_names[i].Name != "mscorlib" && assm_names[i].Name != "CommonLanguageRuntimeLibrary")
                {
                    assemblies.AppendLine("{$reference " + assm_names[i].Name + ".dll}");
                    lines++;
                }
            return assemblies.ToString();
        }

        private string prepare_file_name(string name)
        {
            name = name.Replace('+', '_');
            int ind = name.IndexOf('`');
            if (ind != -1)
                name = name.Substring(0, ind);
            return name;
        }

        /// <summary>
        /// Получить определение expr
        /// </summary>
        public List<Position> GetDefinition(expression expr, int line, int col, PascalABCCompiler.Parsers.KeywordKind keyword, bool only_check)
        {
            List<Position> poses = new List<Position>();
            Position pos = new Position(); pos.line = -1; pos.column = -1;
            try
            {
                if (visitor.cur_scope == null) return poses;
                SymScope ss = visitor.FindScopeByLocation(line + 1, col + 1);//stv.cur_scope;
                if (ss == null) return poses;
                bool on_proc = false;
                SetCurrentUsedAssemblies();
                if (keyword == PascalABCCompiler.Parsers.KeywordKind.Function || keyword == PascalABCCompiler.Parsers.KeywordKind.Constructor || keyword == PascalABCCompiler.Parsers.KeywordKind.Destructor)
                {
                    if (ss is ProcRealization)
                    {
                        if (expr is ident)
                        {
                            if ((expr as ident).name == (ss as ProcRealization).def_proc.si.name)
                                on_proc = true;
                        }
                        else
                            on_proc = true;
                    }
                    else if (ss is ProcScope)
                    {
                        if (expr is ident)
                        {
                            if ((expr as ident).name == (ss as ProcScope).si.name)
                                on_proc = true;
                        }
                        else
                            on_proc = true;
                    }
                }
                //if (!((keyword == KeywordKind.kw_proc || keyword == KeywordKind.kw_constr || keyword == KeywordKind.kw_destr) && ss is ProcScope))
                if (!on_proc)
                //if (!((keyword == KeywordKind.kw_proc || keyword == KeywordKind.kw_constr || keyword == KeywordKind.kw_destr) && ss is ProcRealization))
                {
                    ExpressionVisitor ev = new ExpressionVisitor(expr, ss, visitor);
                    ss = ev.GetScopeOfExpression();
                }
                else
                if (ss is ProcRealization)
                    ss = (ss as ProcRealization).def_proc;
                if (ss != null)
                {
                    if (ss.loc != null)
                    {
                        pos.line = ss.loc.begin_line_num;
                        pos.column = ss.loc.begin_column_num;
                        pos.file_name = ss.loc.doc.file_name;
                        poses.Add(pos);
                    }
                    else if (ss is ProcScope && (ss as ProcScope).is_constructor)
                    {
                        ss = (ss as ProcScope).declaringType;
                        if (ss.loc != null)
                        {
                            pos.line = ss.loc.begin_line_num;
                            pos.column = ss.loc.begin_column_num;
                            pos.file_name = ss.loc.doc.file_name;
                            poses.Add(pos);
                        }
                    }
                    else if (ss is CompiledScope)
                    {
                        pos.from_metadata = true;
                        pos.line = 1;
                        pos.column = 1;
                        CompiledScope cs = ss as CompiledScope;
                        pos.metadata_title = prepare_file_name(cs.CompiledType.Name);
                        pos.full_metadata_title = cs.CompiledType.FullName;
                        if (cs.CompiledType.IsInterface)
                            pos.metadata_type = MetadataType.Interface;
                        else if (cs.CompiledType.BaseType == typeof(Delegate) || cs.CompiledType.BaseType == typeof(MulticastDelegate))
                            pos.metadata_type = MetadataType.Delegate;
                        else if (cs.CompiledType.IsEnum)
                            pos.metadata_type = MetadataType.Enumeration;
                        else if (cs.CompiledType.IsValueType)
                            pos.metadata_type = MetadataType.Struct;
                        else
                            pos.metadata_type = MetadataType.Class;
                        if (!only_check)
                        {
                            pos.metadata = CodeCompletionController.CurrentParser.LanguageInformation.GetCompiledTypeRepresentation(cs.CompiledType, cs.CompiledType, ref pos.line, ref pos.column);
                            Type t = cs.CompiledType;
                            int lines = 0;
                            int ind = pos.metadata.IndexOf('\n');
                            pos.metadata = pos.metadata.Insert(ind + 1, get_references(t, out lines));
                            pos.line += lines;
                        }
                        poses.Add(pos);
                    }
                    else if (ss is CompiledFieldScope)
                    {
                        pos.from_metadata = true;
                        pos.line = 1;
                        pos.column = 1;
                        CompiledFieldScope cfs = ss as CompiledFieldScope;
                        if (cfs.CompiledField.DeclaringType.IsEnum)
                            pos.metadata_type = MetadataType.EnumerationMember;
                        else
                            pos.metadata_type = MetadataType.Field;
                        pos.metadata_title = prepare_file_name(cfs.CompiledField.DeclaringType.Name);
                        if (!only_check)
                        {
                            pos.metadata = CodeCompletionController.CurrentParser.LanguageInformation.GetCompiledTypeRepresentation(cfs.CompiledField.DeclaringType, cfs.CompiledField, ref pos.line, ref pos.column);
                            Type t = cfs.CompiledField.DeclaringType;
                            int lines = 0;
                            int ind = pos.metadata.IndexOf('\n');
                            pos.metadata = pos.metadata.Insert(ind + 1, get_references(t, out lines));
                            pos.line += lines;
                        }
                        pos.full_metadata_title = cfs.CompiledField.DeclaringType.FullName + "." + cfs.CompiledField.Name;
                        poses.Add(pos);
                    }
                    else if (ss is CompiledMethodScope)
                    {
                        pos.from_metadata = true;
                        pos.line = 1;
                        pos.column = 1;
                        CompiledMethodScope cms = ss as CompiledMethodScope;
                        pos.metadata_title = prepare_file_name(cms.CompiledMethod.DeclaringType.Name);
                        pos.metadata_type = MetadataType.Method;
                        if (!only_check)
                        {
                            pos.metadata = CodeCompletionController.CurrentParser.LanguageInformation.GetCompiledTypeRepresentation(cms.CompiledMethod.DeclaringType, cms.CompiledMethod, ref pos.line, ref pos.column);
                            Type t = cms.CompiledMethod.DeclaringType;
                            int lines = 0;
                            int ind = pos.metadata.IndexOf('\n');
                            pos.metadata = pos.metadata.Insert(ind + 1, get_references(t, out lines));
                            pos.line += lines;
                        }
                        pos.full_metadata_title = cms.CompiledMethod.DeclaringType.FullName + "." + cms.CompiledMethod.Name;
                        poses.Add(pos);
                    }
                    else if (ss is CompiledPropertyScope)
                    {
                        pos.from_metadata = true;
                        CompiledPropertyScope cps = ss as CompiledPropertyScope;
                        pos.metadata_title = prepare_file_name(cps.CompiledProperty.DeclaringType.Name);
                        pos.line = 1;
                        pos.column = 1;
                        pos.metadata_type = MetadataType.Property;
                        if (!only_check)
                        {
                            pos.metadata = CodeCompletionController.CurrentParser.LanguageInformation.GetCompiledTypeRepresentation((ss as CompiledPropertyScope).CompiledProperty.DeclaringType, (ss as CompiledPropertyScope).CompiledProperty, ref pos.line, ref pos.column);
                            Type t = cps.CompiledProperty.DeclaringType;
                            int lines = 0;
                            int ind = pos.metadata.IndexOf('\n');
                            pos.metadata = pos.metadata.Insert(ind + 1, get_references(t, out lines));
                            pos.line += lines;
                        }
                        pos.full_metadata_title = cps.CompiledProperty.DeclaringType.FullName + "." + cps.CompiledProperty.Name;
                        poses.Add(pos);
                    }
                    else if (ss is CompiledConstructorScope)
                    {
                        CompiledConstructorScope ccs = ss as CompiledConstructorScope;
                        pos.from_metadata = true;
                        pos.line = 1;
                        pos.column = 1;
                        pos.metadata_title = prepare_file_name(ccs.CompiledConstructor.DeclaringType.Name);
                        pos.metadata_type = MetadataType.Constructor;
                        if (!only_check)
                        {
                            pos.metadata = CodeCompletionController.CurrentParser.LanguageInformation.GetCompiledTypeRepresentation(ccs.CompiledConstructor.DeclaringType, ccs.CompiledConstructor, ref pos.line, ref pos.column);
                            Type t = ccs.CompiledConstructor.DeclaringType;
                            int lines = 0;
                            int ind = pos.metadata.IndexOf('\n');
                            pos.metadata = pos.metadata.Insert(ind + 1, get_references(t, out lines));
                            pos.line += lines;
                        }
                        pos.full_metadata_title = ccs.CompiledConstructor.DeclaringType.FullName + "." + ccs.CompiledConstructor.DeclaringType.Name;
                        poses.Add(pos);
                    }
                    else if (ss is CompiledEventScope)
                    {
                        CompiledEventScope ces = ss as CompiledEventScope;
                        pos.from_metadata = true;
                        pos.line = 1;
                        pos.column = 1;
                        pos.metadata_title = prepare_file_name(ces.CompiledEvent.DeclaringType.Name);
                        pos.metadata_type = MetadataType.Event;
                        if (!only_check)
                        {
                            pos.metadata = CodeCompletionController.CurrentParser.LanguageInformation.GetCompiledTypeRepresentation(ces.CompiledEvent.DeclaringType, ces.CompiledEvent, ref pos.line, ref pos.column);
                            Type t = ces.CompiledEvent.DeclaringType;
                            int lines = 0;
                            int ind = pos.metadata.IndexOf('\n');
                            pos.metadata = pos.metadata.Insert(ind + 1, get_references(t, out lines));
                            pos.line += lines;
                        }
                        pos.full_metadata_title = ces.CompiledEvent.DeclaringType.FullName + "." + ces.CompiledEvent.Name;
                        poses.Add(pos);
                    }
                    else if (ss is NamespaceScope)
                    {
                        pos.from_metadata = true;
                        pos.line = 1;
                        pos.column = 1;
                        pos.metadata_title = prepare_file_name((ss as NamespaceScope).name);
                        pos.metadata_type = MetadataType.Event;
                        pos.full_metadata_title = (ss as NamespaceScope).GetFullName();
                        poses.Add(pos);
                    }
                }
            }
            catch (Exception e)
            {

            }
            RestoreCurrentUsedAssemblies();
            return poses;
        }

        /// <summary>
        /// Получить реализацию expr
        /// </summary>
        public List<Position> GetRealization(expression expr, int line, int col, PascalABCCompiler.Parsers.KeywordKind keyword)
        {
            List<Position> poses = new List<Position>();
            Position pos = new Position(); pos.line = -1; pos.column = -1;
            try
            {
                if (visitor.cur_scope == null) return poses;
                SymScope ss = visitor.FindScopeByLocation(line + 1, col + 1);//stv.cur_scope;
                if (ss == null) return poses;
                //if (!(expr is ident && string.Compare((expr as ident).name,ss.si.name) == 0))
                bool on_proc = false;
                SetCurrentUsedAssemblies();
                if (keyword == PascalABCCompiler.Parsers.KeywordKind.Function || keyword == PascalABCCompiler.Parsers.KeywordKind.Constructor || keyword == PascalABCCompiler.Parsers.KeywordKind.Destructor)
                {
                    if (ss is ProcRealization)
                    {
                        if (expr is ident)
                        {
                            if ((expr as ident).name == (ss as ProcRealization).def_proc.si.name)
                                on_proc = true;
                        }
                        else
                            on_proc = true;
                    }
                    else if (ss is ProcScope)
                    {
                        if (expr is ident)
                        {
                            if ((expr as ident).name == (ss as ProcScope).si.name)
                                on_proc = true;
                        }
                        else
                            on_proc = true;
                    }
                }
                //if (!((keyword == KeywordKind.kw_proc || keyword == KeywordKind.kw_constr || keyword == KeywordKind.kw_destr) && ss is ProcScope))
                if (!on_proc)
                //if (keyword != KeywordKind.kw_proc && keyword != KeywordKind.kw_constr && keyword != KeywordKind.kw_destr)
                {
                    ExpressionVisitor ev = new ExpressionVisitor(expr, ss, visitor);
                    ss = ev.GetScopeOfExpression();
                }
                while (ss != null && ss is ProcScope && (ss as ProcScope).procRealization != null && (ss as ProcScope).procRealization.loc != null)
                {
                    ProcRealization pr = (ss as ProcScope).procRealization;
                    pos.line = pr.loc.begin_line_num;
                    pos.column = pr.loc.begin_column_num;
                    pos.file_name = pr.loc.doc.file_name;
                    poses.Add(pos);
                    if (on_proc) break;
                    //ss = (ss as ProcScope).nextProc;
                    ss = null;
                }
            }
            catch (Exception e)
            {

            }
            RestoreCurrentUsedAssemblies();
            return poses;
        }

        /// <summary>
        /// Получить описание элемента при наведении мышью
        /// </summary>
        public string GetDescription(expression expr, string FileName, string expr_without_brackets, PascalABCCompiler.Parsers.Controller parser, int line, int col, PascalABCCompiler.Parsers.KeywordKind keyword, bool header)
        {
            if (visitor.cur_scope == null) return null;
            SymScope ss = visitor.FindScopeByLocation(line + 1, col + 1);//stv.cur_scope;
            if (ss == null) return null;
            if (!header && ss.IsInScope(ss.head_loc, line + 1, col + 1))
            {
                List<PascalABCCompiler.Errors.Error> Errors = new List<PascalABCCompiler.Errors.Error>();
                List<PascalABCCompiler.Errors.CompilerWarning> Warnings = new List<PascalABCCompiler.Errors.CompilerWarning>();
                expr = parser.GetExpression("test" + Path.GetExtension(FileName), expr_without_brackets, Errors, Warnings);
                if (expr == null || Errors.Count > 0)
                    return null;
            }
            bool on_proc = false;
            SetCurrentUsedAssemblies();
            if (keyword == PascalABCCompiler.Parsers.KeywordKind.Function || keyword == PascalABCCompiler.Parsers.KeywordKind.Constructor || keyword == PascalABCCompiler.Parsers.KeywordKind.Destructor)
            {
                if (ss is ProcRealization)
                {
                    if (expr is ident)
                    {
                        if ((expr as ident).name == (ss as ProcRealization).def_proc.si.name) on_proc = true;
                    }
                    else on_proc = true;
                }
                else if (ss is ProcScope)
                {
                    if (expr is ident)
                    {
                        if ((expr as ident).name == (ss as ProcScope).si.name) on_proc = true;
                    }
                    else on_proc = true;
                }
            }
            //if (!((keyword == KeywordKind.kw_proc || keyword == KeywordKind.kw_constr || keyword == KeywordKind.kw_destr) && ss is ProcScope))
            if (!on_proc)
            {
                ExpressionVisitor ev = new ExpressionVisitor(expr, ss, visitor);
                ev.mouse_hover = true;
                ss = ev.GetScopeOfExpression();
            }
            if (ss != null && ss.si != null)
            {
                try
                {
                    if (ss.si.has_doc != true)
                        if (ss is CompiledScope)
                            ss.AddDocumentation(AssemblyDocCache.GetDocumentation((ss as CompiledScope).ctn));
                        else if (ss is CompiledMethodScope)
                            ss.AddDocumentation(AssemblyDocCache.GetDocumentation((ss as CompiledMethodScope).mi));
                        else if (ss is CompiledPropertyScope)
                            ss.AddDocumentation(AssemblyDocCache.GetDocumentation((ss as CompiledPropertyScope).pi));
                        else if (ss is CompiledFieldScope)
                            ss.AddDocumentation(AssemblyDocCache.GetDocumentation((ss as CompiledFieldScope).fi));
                        else if (ss is CompiledEventScope)
                            ss.AddDocumentation(AssemblyDocCache.GetDocumentation((ss as CompiledEventScope).ei));
                        else if (ss is CompiledConstructorScope)
                            ss.AddDocumentation(AssemblyDocCache.GetDocumentation((ss as CompiledConstructorScope).mi));
                        else if (ss is NamespaceScope)
                            ss.AddDocumentation(AssemblyDocCache.GetDocumentationForNamespace((ss as NamespaceScope).name));
                        else if (ss is TypeScope)
                            ss.AddDocumentation(UnitDocCache.GetDocumentation(ss as TypeScope));
                        else if (ss is ProcScope)
                        {
                            ProcScope ps = ss as ProcScope;
                            if (ps.original_function != null)
                                ps = ps.original_function;
                            if (ps is CompiledMethodScope)
                                ss.AddDocumentation(AssemblyDocCache.GetDocumentation((ps as CompiledMethodScope).mi));
                            else
                                ss.AddDocumentation(UnitDocCache.GetDocumentation(ps));
                        }
                            
                        else if (ss is InterfaceUnitScope)
                            ss.AddDocumentation(UnitDocCache.GetDocumentation(ss as InterfaceUnitScope));
                        else if (ss is ElementScope && string.IsNullOrEmpty(ss.si.description) && (ss as ElementScope).sc is TypeScope)
                            ss.si.description = (ss as ElementScope).sc.Description;
                }
                catch (Exception e)
                {
#if DEBUG
                    File.AppendAllText("log.txt", e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
#endif
                }
                RestoreCurrentUsedAssemblies();
                string description = ss.si.description;
                if (description != null)
                    description = description.Replace("!#", "");
                return description;
            }
            RestoreCurrentUsedAssemblies();
            return null;
        }

        /// <summary>
        /// Получить подсказку индекса
        /// </summary>
        public string[] GetIndex(expression expr, int line, int col)
        {
            if (visitor.cur_scope == null) return null;
            SymScope si = visitor.FindScopeByLocation(line + 1, col + 1);//stv.cur_scope;
            if (si == null)
            {
                si = visitor.FindScopeByLocation(line, col + 1);
                if (si == null)
                    return null;
            }
            ExpressionVisitor ev = new ExpressionVisitor(expr, si, visitor);
            si = ev.GetScopeOfExpression(false, true);
            return CodeCompletionController.CurrentParser.LanguageInformation.GetIndexerString(si);
        }

        /// <summary>
        /// Получить подсказку параметров метода
        /// </summary>
        public string[] GetNameOfMethod(expression expr, string str, int line, int col, int num_param, ref int defIndex, int choose_param_num, out int param_count)
        {
            param_count = 0;
            if (visitor.cur_scope == null) return null;
            if (col + 1 > str.Length)
                col -= str.Length;
            SymScope si = visitor.FindScopeByLocation(line + 1, col + 1);//stv.cur_scope;
            if (si == null)
            {
                si = visitor.FindScopeByLocation(line, col + 1);
                if (si == null)
                    return null;
            }
            SetCurrentUsedAssemblies();
            ExpressionVisitor ev = new ExpressionVisitor(expr, si, visitor);
            List<ProcScope> scopes = ev.GetOverloadScopes();
            if (scopes.Count == 0)
            {
                RestoreCurrentUsedAssemblies();
                return null;
            }
            si = scopes[0];
            if (si != null && si is ProcScope)
            {
                List<string> procs = new List<string>();
                List<ProcScope> proc_defs = new List<ProcScope>();
                ProcScope ps = si as ProcScope;
                int i = 0; bool stop = false;
                ProcScope tmp = ps;
                while (i < scopes.Count)
                {
                    if (i == defIndex)
                    {
                        if (tmp.GetParametersCount() != 0)
                        {
                            choose_param_num = tmp.GetParametersCount();
                            param_count = choose_param_num;
                        }
                        break;
                    }
                    i++;
                    tmp = scopes[i];
                }
                i = 0;
                while (ps != null)
                {
                    if (!stop && num_param > choose_param_num && ps.GetParametersCount() >= num_param && ps.GetParametersCount() > choose_param_num)
                    {
                        defIndex = i;
                        stop = true;
                        param_count = ps.GetParametersCount();
                    }
                    if (ps is CompiledMethodScope)
                        ps.AddDocumentation(AssemblyDocCache.GetDocumentation((ps as CompiledMethodScope).mi));
                    else if (ps is CompiledConstructorScope)
                        ps.AddDocumentation(AssemblyDocCache.GetDocumentation((ps as CompiledConstructorScope).mi));
                    else if (ps is ProcScope)
                    {
                        if (!ps.si.has_doc)
                        {
                            ps.AddDocumentation(UnitDocCache.GetDocumentation(ps as ProcScope));
                        }
                    }
                    if (ps.acc_mod == access_modifer.protected_modifer || ps.acc_mod == access_modifer.private_modifer)
                    {
                        if (ps.acc_mod == access_modifer.private_modifer)
                        {
                            if (ev.IsInOneModule(ev.entry_scope, ps.topScope))
                                if (!ps.si.not_include && !equal_params(ps, proc_defs))
                                {
                                    procs.Add(ps.si.description);
                                    proc_defs.Add(ps);
                                }
                        }
                        else
                        if (ev.CheckForBaseAccess(ev.entry_scope, ps.topScope))
                            if (!ps.si.not_include && !equal_params(ps, proc_defs))
                            {
                                procs.Add(ps.si.description);
                                proc_defs.Add(ps);
                            }
                    }
                    else
                    if (!ps.si.not_include)
                        if (!equal_params(ps, proc_defs))
                        {
                            procs.Add(ps.si.description);
                            proc_defs.Add(ps);
                        }
                    i++;
                    if (i < scopes.Count)
                        ps = scopes[i];
                    else
                        ps = null;
                }
                RestoreCurrentUsedAssemblies();
                return procs.ToArray();
            }
            RestoreCurrentUsedAssemblies();
            return null;
        }

        private bool equal_params(ProcScope ps, List<ProcScope> procs)
        {
            foreach (ProcScope proc in procs)
            {
                if ((ps.IsOverride || proc.IsOverride) && ps.IsParamsEquals(proc))
                    return true;
            }
            return false;
        }

        private Hashtable tmp_cur_used_assemblies;

        //kazhdaja programma mozhet imet svoj spisok podkluchaemyh sborok, 
        //poetomu nado sohranat i vosstanavlivat tekushij kesh sborok
        private void SetCurrentUsedAssemblies()
        {
            tmp_cur_used_assemblies = PascalABCCompiler.NetHelper.NetHelper.cur_used_assemblies;
            PascalABCCompiler.NetHelper.NetHelper.cur_used_assemblies = cur_used_assemblies;
        }

        private void RestoreCurrentUsedAssemblies()
        {
            if (tmp_cur_used_assemblies != null)
                PascalABCCompiler.NetHelper.NetHelper.cur_used_assemblies = tmp_cur_used_assemblies;
        }

        ~DomConverter()
        {

        }
    }

}
