using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace PascalABCCompiler.SyntaxTree
{
    public partial class syntax_tree_node
    {
        public int FindIndex(syntax_tree_node node)
        {
            int ind = -1;
            for (var i = 0; i < subnodes_count; i++)
                if (node == this[i])
                {
                    ind = i;
                    break;
                }
            if (ind == -1)
                throw new Exception(string.Format("У элемента {0} не найден {1} среди дочерних\n", this, node));
            return ind;
        }

        public void Replace(syntax_tree_node from, syntax_tree_node to) // есть риск, что типы не совпадут
        {
            var ind = FindIndex(from);
            this[ind] = to;
        }
    }

    //------------------------------
    public class SyntaxList<T> where T : syntax_tree_node // операции для работы с вложенными списками синтаксических узлов. Класс пока не используется
    {
        public List<T> list;
        public SyntaxList(List<T> l)
        {
            list = l;
        }
        public void AddMany(params T[] tt)
        {
            list.AddRange(tt);
        }
        public bool Remove(T t)
        {
            return list.Remove(t);
        }
        public void InsertAfter(T st, T newst)
        {
            var ind = list.FindIndex(x => x == st);
            if (ind == -1)
                throw new Exception(string.Format("У {0} не найден {1} среди дочерних\n", this, st));
            list.Insert(ind + 1, newst);
        }
        public void InsertBefore(T st, T newst)
        {
            var ind = list.FindIndex(x => x == st);
            if (ind == -1)
                throw new Exception(string.Format("У {0} не найден {1} среди дочерних\n", this, st));
            list.Insert(ind, newst);
        }
        public void AddFirst(T st)
        {
            list.Insert(0, st);
        }
    }
    //------------------------------

    public partial class statement_list 
    {
        public statement_list(statement st, SourceContext sc)
        {
            Add(st, sc);
        }
        public statement_list(params statement[] sts)
        {
            AddMany(sts);
        }
        public statement_list Add(statement st, SourceContext sc = null)
        {
            subnodes.Add(st);
            if (sc != null)
                source_context = sc;
            return this;
        }

        //-- List members begin
        public List<statement> list
        {
            get { return subnodes; }
        }
        public void AddMany(params statement[] els)
        {
            list.AddRange(els);
        }
        public void AddMany(IEnumerable<statement> els)
        {
            list.AddRange(els);
        }
        public bool Remove(statement el)
        {
            return list.Remove(el);
        }
        private int FindIndex(statement el)
        {
            var ind = list.FindIndex(x => x == el);
            if (ind == -1)
                throw new Exception(string.Format("У списка {0} не найден элемент {1} среди дочерних\n", this, el));
            return ind;
        }
        public void Replace(statement el, statement newel)
        {
            list[FindIndex(el)] = newel;
        }
        public void Replace(statement el, IEnumerable<statement> newels)
        {
            var ind = FindIndex(el);
            list.RemoveAt(ind);
            list.InsertRange(ind,newels);
        }
        public void InsertAfter(statement el, statement newel)
        {
            list.Insert(FindIndex(el) + 1, newel);
        }
        public void InsertBefore(statement el, statement newel)
        {
            list.Insert(FindIndex(el), newel);
        }
        public void InsertAfter(statement el, IEnumerable<statement> newels)
        {
            list.InsertRange(FindIndex(el) + 1, newels);
        }
        public void InsertBefore(statement el, IEnumerable<statement> newels)
        {
            list.InsertRange(FindIndex(el), newels);
        }
        public void AddFirst(statement el)
        {
            list.Insert(0, el);
        }
        public void AddFirst(IEnumerable<statement> els)
        {
            list.InsertRange(0, els);
        }
        public static statement_list Empty
        {
            get { return new statement_list(); }
        }
        //-- List members end
    }

    public partial class ident : addressed_value_funcname
    {
        public override string ToString()
        {
            return name;
        }
        public static implicit operator ident(string s)
        {
            return new ident(s);
        }
    }

    public partial class assign
    {
        public assign(addressed_value left, expression ex, SourceContext sc = null) : this(left, ex, Operators.Assignment, sc)
        { }
        public assign(string name, expression ex, SourceContext sc = null) : this(new ident(name), ex, sc)
        { }
        public assign(string name, string id) : this(new ident(name), new ident(id))
        { }
        public assign(string name, int value) : this(new ident(name), new int32_const(value))
        { }
        public assign(string name, double value) : this(new ident(name), new double_const(value))
        { }
        public assign(string name, char value) : this(new ident(name), new char_const(value))
        { }
        public assign(string name, bool value) : this(new ident(name), new bool_const(value))
        { }
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", to, OperatorServices.ToString(operator_type, LanguageId.PascalABCNET), from);
        }
    }

    public partial class bin_expr
    {
        public static bin_expr Greater(expression left, expression right)
        {
            return new bin_expr(left, right, Operators.Greater);
        }
        public static bin_expr Less(expression left, expression right)
        {
            return new bin_expr(left, right, Operators.Less);
        }

        public override string ToString()
        {
            return string.Format("{0} {2} {1}", left, right, OperatorServices.ToString(operation_type, LanguageId.PascalABCNET));
        }
    }

    public partial class un_expr
    {
        public static un_expr Not(expression ex)
        {
            return new un_expr(ex, Operators.LogicalNOT); 
        }
        public override string ToString()
        {
            return string.Format("{0} {1}", OperatorServices.ToString(operation_type, LanguageId.PascalABCNET),this.subnode);
        }
    }

    public partial class bool_const
    {
        public override string ToString()
        {
            if (val)
                return "True";
            else return "False";
        }
    }

    public partial class int32_const
    {
        public override string ToString()
        {
            return val.ToString();
        }
    }

    public partial class double_const
    {
        public override string ToString()
        {
            return val.ToString();
        }
    }

    public partial class roof_dereference
    {
        public override string ToString()
        {
            return base.ToString() + "^";
        }
    }

    public partial class named_type_reference
    {
        public named_type_reference(string name)
        {
            this.names = name.Split('.').Select(s => new ident(s)).ToList();
        }
        public named_type_reference(ident id, SourceContext sc = null)
        {
            Add(id, sc);
        }
        public named_type_reference Add(ident id, SourceContext sc = null)
        {
            names.Add(id);
            if (sc != null)
                source_context = sc;
            return this;
        }
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(names[0].ToString());
            for (int i = 1; i < names.Count; i++)
                sb.Append("." + names[i].ToString());
            return sb.ToString();
        }

        public ident FirstIdent
        {
            get { return names[0]; }
        }
    }

    public partial class template_type_reference
    {
        public template_type_reference(string name, template_param_list params_list): this(new named_type_reference(name), params_list)
        { }
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(this.name.ToString());
            sb.Append("<");
            sb.Append(params_list.params_list[0].ToString());
            for (int i = 1; i < params_list.params_list.Count; i++)
                sb.Append("," + params_list.params_list[i].ToString());
            sb.Append(">");
            return sb.ToString();
        }
    }


    public partial class variable_definitions
    {
        public variable_definitions(var_def_statement _var_def_statement, SourceContext sc = null)
        {
            Add(_var_def_statement, sc);
        }
        public variable_definitions Add(var_def_statement _var_def_statement, SourceContext sc = null)
        {
            var_definitions.Add(_var_def_statement);
            if (sc != null)
                source_context = sc;
            return this;
        }
        //-- List members begin
        public List<var_def_statement> list
        {
            get { return var_definitions; }
        }

        public void AddMany(params var_def_statement[] sts)
        {
            list.AddRange(sts);
        }
        public bool Remove(var_def_statement st)
        {
            return list.Remove(st);
        }
        private int FindIndex(var_def_statement st)
        {
            var ind = list.FindIndex(x => x == st);
            if (ind == -1)
                throw new Exception(string.Format("У списка {0} не найден элемент {1} среди дочерних\n", this, st));
            return ind;
        }
        public void Replace(var_def_statement st, var_def_statement newst)
        {
            list[FindIndex(st)] = newst;
        }
        public void InsertAfter(var_def_statement st, var_def_statement newst)
        {
            list.Insert(FindIndex(st) + 1, newst);
        }
        public void InsertBefore(var_def_statement st, var_def_statement newst)
        {
            list.Insert(FindIndex(st), newst);
        }
        public void AddFirst(var_def_statement st)
        {
            list.Insert(0, st);
        }
        //-- List members end
    }

    public partial class ident_list
    {
        public ident_list(ident id, SourceContext sc)
        {
            Add(id, sc);
        }
        public ident_list(params ident[] ids)
        {
            AddMany(ids);
        }
        public ident_list Add(ident id, SourceContext sc = null)
        {
            idents.Add(id);
            if (sc != null)
                source_context = sc;
            return this;
        }
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(idents[0].ToString());
            for (int i = 1; i < idents.Count; i++)
                sb.Append("," + idents[i].ToString());
            return sb.ToString();
        }
        //-- List members begin
        public List<ident> list
        {
            get { return idents; }
        }

        public void AddMany(params ident[] els)
        {
            list.AddRange(els);
        }
        public bool Remove(ident el)
        {
            return list.Remove(el);
        }
        private int FindIndex(ident el)
        {
            var ind = list.FindIndex(x => x == el);
            if (ind == -1)
                throw new Exception(string.Format("У списка {0} не найден элемент {1} среди дочерних\n", this, el));
            return ind;
        }
        public void Replace(ident el, ident newel)
        {
            list[FindIndex(el)] = newel;
        }
        public void InsertAfter(ident el, ident newel)
        {
            list.Insert(FindIndex(el) + 1, newel);
        }
        public void InsertBefore(ident el, ident newel)
        {
            list.Insert(FindIndex(el), newel);
        }
        public void AddFirst(ident el)
        {
            list.Insert(0, el);
        }
        //-- List members end
    }

    public partial class var_def_statement
    {
        public var_def_statement(ident_list vars, type_definition vars_type) : this(vars, vars_type, null, definition_attribute.None, false)
        { }

        public var_def_statement(ident_list vars, type_definition vars_type, expression iv) : this(vars, vars_type, iv, definition_attribute.None, false)
        { }

        public var_def_statement(ident id, type_definition type) : this(new ident_list(id), type)
        { }

        public var_def_statement(ident id, type_definition type, expression iv) : this(new ident_list(id), type, iv)
        { }

        public var_def_statement(ident id, string type) : this(new ident_list(id), new named_type_reference(type))
        { }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(vars.ToString());
            if (vars_type != null)
            {
                sb.Append(": ");
                sb.Append(vars_type.ToString());
            }
            if (inital_value != null)
            {
                sb.Append(" := ");
                sb.Append(inital_value.ToString());
            }
            sb.Append("; ");
            return sb.ToString();
        }
    }

    public partial class declarations
    {
        public declarations(declaration _declaration, SourceContext sc = null)
        {
            Add(_declaration, sc);
        }
        public declarations Add(declaration _declaration, SourceContext sc = null)
        {
            defs.Add(_declaration);
            if (sc != null)
                source_context = sc;
            return this;
        }

        //-- List members begin
        public List<declaration> list
        {
            get { return defs; }
        }
        public void AddMany(params declaration[] els)
        {
            list.AddRange(els);
        }
        public bool Remove(declaration el)
        {
            return list.Remove(el);
        }
        private int FindIndex(declaration el)
        {
            var ind = list.FindIndex(x => x == el);
            if (ind == -1)
                throw new Exception(string.Format("У списка {0} не найден элемент {1} среди дочерних\n", this, el));
            return ind;
        }
        public void Replace(declaration el, declaration newel)
        {
            list[FindIndex(el)] = newel;
        }
        public void Replace(declaration el, IEnumerable<declaration> newels)
        {
            var ind = FindIndex(el);
            list.RemoveAt(ind);
            list.InsertRange(ind, newels);
        }
        public void InsertAfter(declaration el, declaration newel)
        {
            list.Insert(FindIndex(el) + 1, newel);
        }
        public void InsertBefore(declaration el, declaration newel)
        {
            list.Insert(FindIndex(el), newel);
        }
        public void InsertAfter(declaration el, IEnumerable<declaration> newels)
        {
            list.InsertRange(FindIndex(el) + 1, newels);
        }
        public void InsertBefore(declaration el, IEnumerable<declaration> newels)
        {
            list.InsertRange(FindIndex(el), newels);
        }
        public void AddFirst(declaration el)
        {
            list.Insert(0, el);
        }
        public void AddFirst(IEnumerable<declaration> els)
        {
            list.InsertRange(0, els);
        }
        public static statement_list Empty
        {
            get { return new statement_list(); }
        }
        //-- List members end
    }

    public partial class program_tree
    {
        public program_tree(compilation_unit cu, SourceContext sc = null)
        {
            Add(cu, sc);
        }
        public program_tree Add(compilation_unit cu, SourceContext sc = null)
        {
            compilation_units.Add(cu);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class program_name
    {
        public override string ToString()
        {
            return prog_name.ToString();
        }
    }

    public partial class string_const
    {
        public override string ToString()
        {
            return "'" + Value + "'";
        }
    }

    public partial class expression_list
    {
        public expression_list(expression _expression, SourceContext sc = null)
        {
            Add(_expression, sc);
        }
        public expression_list Add(expression _expression, SourceContext sc = null)
        {
            expressions.Add(_expression);
            if (sc != null)
                source_context = sc;
            return this;
        }
        public static expression_list Empty
        {
            get { return new expression_list(); }
        }
        public override string ToString()
        {
            if (expressions.Count == 0)
                return "";
            var sb = new System.Text.StringBuilder();
            sb.Append(expressions[0].ToString());
            for (int i = 1; i < expressions.Count; i++)
            {
                sb.Append(",");
                sb.Append(expressions[i].ToString());
            }
            return sb.ToString();
        }
    }

    public partial class dereference
    {
        public override string ToString()
        {
            return dereferencing_value.ToString();
        }
    }

    public partial class indexer
    {
        public override string ToString()
        {
            return base.ToString() + "[" + indexes.ToString() + "]";
        }
    }

    public partial class indexers_types
    {
        public indexers_types(type_definition _type_definition, SourceContext sc = null)
        {
            Add(_type_definition, sc);
        }
        public indexers_types Add(type_definition _type_definition, SourceContext sc = null)
        {
            indexers.Add(_type_definition);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class label_definitions
    {
        public label_definitions(params ident[] ids): this(new ident_list(ids))
        {
        }
        public override string ToString()
        {
            return "label " + labels.ToString() + ";";
        }
    }

    public partial class procedure_attribute
    {
        public override string ToString()
        {
            return attribute_type.ToString();
        }
    }

    public partial class typed_parameters
    {
        public typed_parameters(ident_list idents, type_definition type): this(idents, type, parametr_kind.none, null)
        { }
        public typed_parameters(ident id, type_definition type): this(new ident_list(id), type)
        { }
        public override string ToString()
        {
            var s = this.idents.ToString() + ": " + this.vars_type.ToString();
            if (this.inital_value != null)
                s += " := " + this.inital_value.ToString();
            return s;
        }
    }

    public partial class formal_parameters
    {
        public formal_parameters(typed_parameters _typed_parameters, SourceContext sc = null)
        {
            Add(_typed_parameters, sc);
        }
        public formal_parameters Add(typed_parameters _typed_parameters, SourceContext sc = null)
        {
            params_list.Add(_typed_parameters);
            if (sc != null)
                source_context = sc;
            return this;
        }
        public override string ToString()
        {
            return string.Join(",", params_list.Select(p => p.ToString()));
        }
        public static formal_parameters Empty
        {
            get { return new formal_parameters(); }
        }
    }
    ///
    public partial class procedure_attributes_list
    {
        public procedure_attributes_list(procedure_attribute _procedure_attribute, SourceContext sc = null)
        {
            Add(_procedure_attribute, sc);
        }
        public procedure_attributes_list(proc_attribute attr, SourceContext sc = null) : this(new procedure_attribute(attr), sc)
        { }
        public procedure_attributes_list Add(procedure_attribute _procedure_attribute, SourceContext sc = null)
        {
            proc_attributes.Add(_procedure_attribute);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class procedure_header
    {
        public procedure_header(formal_parameters _parameters, procedure_attributes_list _proc_attributes, method_name _name, where_definition_list _where_defs, SourceContext sc)
        {
            this._parameters = _parameters;
            this._proc_attributes = _proc_attributes;
            this._name = _name;
            this._of_object = false;
            this._class_keyword = false;
            this._template_args = null;
            this._where_defs = _where_defs;
            source_context = sc;
            if (name != null)
                if (name.meth_name is template_type_name)
                {
                    var t = name.meth_name as template_type_name;
                    template_args = t.template_args;
                    if (name.meth_name is template_operator_name)
                    { 
                        name.meth_name = new operator_name_ident((name.meth_name as template_operator_name).opname.operator_type, name.meth_name.source_context);
                    }
                    else
                        name.meth_name = new ident(name.meth_name.name, name.meth_name.source_context);
                }
        }

        //for sugar
        public procedure_header(string name, formal_parameters fp, procedure_attributes_list pal) : this(fp, pal, new method_name(name), null, null)
        { }

        public procedure_header(string name, formal_parameters fp) : this(name, fp, new procedure_attributes_list())
        { }

        public procedure_header(string name) : this(name, new formal_parameters())
        { }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("procedure ");
            sb.Append(name.ToString());

            if (template_args != null)
                sb.Append("<" + template_args.ToString() + ">");

            if (parameters != null)
                sb.Append("(" + parameters.ToString() + ")");
            sb.Append(";");
            return sb.ToString();
        }
    }

    public partial class function_header
    {
        public function_header(formal_parameters _parameters, procedure_attributes_list _proc_attributes, method_name _name, where_definition_list _where_defs, type_definition _return_type, SourceContext sc)
            : base(_parameters, _proc_attributes, _name, _where_defs, sc)
        {
            this._return_type = _return_type;
        }

        //for sugar
        public function_header(string name, type_definition returntype, formal_parameters fp) : this(fp, new procedure_attributes_list(), new method_name(name), null, returntype, null)
        { }

        public function_header(string name, type_definition returntype) : this(new formal_parameters(), new procedure_attributes_list(), new method_name(name), null, returntype, null)
        { }

        public function_header(string name, string returntype, formal_parameters fp, procedure_attributes_list pal) : this(fp, pal, new method_name(name), null, new named_type_reference(returntype), null)
        { }

        public function_header(string name, string returntype, formal_parameters fp) : this(fp, new procedure_attributes_list(), new method_name(name), null, new named_type_reference(returntype), null)
        { }

        public function_header(string name, string returntype) : this(new formal_parameters(), new procedure_attributes_list(), new method_name(name), null, new named_type_reference(returntype), null)
        { }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(base.ToString());
            sb.Remove(0, 9);
            sb.Remove(sb.Length - 1, 1);
            sb.Insert(0, "function");
            sb.Append(": " + return_type.ToString() + ";");
            return sb.ToString();
        }
    }

    public partial class procedure_definition
    {
        public procedure_definition(procedure_header proc_header, proc_block proc_body, SourceContext sc)
        {
            this.proc_header = proc_header;
            this.proc_body = proc_body;
            source_context = sc;
            is_short_definition = false;
        }
        public procedure_definition(procedure_header proc_header, proc_block proc_body)
        {
            this.proc_header = proc_header;
            this.proc_body = proc_body;
            source_context = null;
            is_short_definition = false;
        }

        public static procedure_definition EmptyDefaultConstructor
        {
            get { return new procedure_definition(new constructor(null), block.Empty, null); }
        }
            
        public procedure_definition(string name, formal_parameters fp, declarations defs, statement_list code) : this(new procedure_header(name,fp), new block(defs, code))
        { }

        public procedure_definition(string name, formal_parameters fp, statement_list code) : this(new procedure_header(name, fp), new block(null, code))
        { }

        public procedure_definition(string name, formal_parameters fp, statement st) : this(new procedure_header(name, fp), new block(null, new statement_list(st)))
        { }

        public procedure_definition(string name, declarations defs, statement_list code) : this(new procedure_header(name), new block(defs, code))
        { }

        public procedure_definition(string name, statement_list code, proc_block b) : this(new procedure_header(name), b)
        { }

        public procedure_definition(string name, statement_list code) : this(new procedure_header(name), new block(null, code))
        { }

        public procedure_definition(procedure_header proc_header, statement_list code) : this(proc_header, new block(null, code))
        { }

        public procedure_definition(string name) : this(new procedure_header(name), new block(null, statement_list.Empty))
        { }

        public procedure_definition(string name, statement st) : this(new procedure_header(name), new block(null, new statement_list(st)))
        { }

        public procedure_definition(procedure_header proc_header, statement st) : this(proc_header, new block(null, new statement_list(st)))
        { }

        public procedure_definition(string name, string rettype, formal_parameters fp, declarations defs, statement_list code) : this(new function_header(name, rettype, fp), new block(defs, code))
        { }

        public procedure_definition(string name, string rettype, formal_parameters fp, statement_list code) : this(new function_header(name, rettype, fp), new block(null, code))
        { }

        public procedure_definition(string name, string rettype, formal_parameters fp, statement st) : this(new function_header(name, rettype, fp), new block(null, new statement_list(st)))
        { }

        public procedure_definition(string name, string rettype, declarations defs, statement_list code) : this(new function_header(name, rettype), new block(defs, code))
        { }

        public procedure_definition(string name, string rettype, proc_block b) : this(new function_header(name, rettype), b)
        { }

        public procedure_definition(string name, string rettype, statement_list code) : this(new function_header(name, rettype), new block(null, code))
        { }

        public procedure_definition(string name, string rettype, statement st) : this(new function_header(name, rettype), new block(null, new statement_list(st)))
        { }

        public void AssignAttrList(attribute_list al)
        {
            if (proc_header != null)
                proc_header.attributes = al;
        }
    }

    public partial class type_declarations
    {
        public type_declarations(type_declaration _type_declaration, SourceContext sc = null)
        {
            Add(_type_declaration, sc);
        }
        public type_declarations Add(type_declaration _type_declaration, SourceContext sc = null)
        {
            types_decl.Add(_type_declaration);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class consts_definitions_list
    {
        public consts_definitions_list(const_definition _const_definition, SourceContext sc = null)
        {
            Add(_const_definition, sc);
        }
        public consts_definitions_list Add(const_definition _const_definition, SourceContext sc = null)
        {
            const_defs.Add(_const_definition);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class unit_or_namespace
    {
        public unit_or_namespace(string name, SourceContext sc = null)
        {
            this.name = new ident_list(name,sc);
        }
    }

    public partial class uses_list
    {
        public uses_list(string name, SourceContext sc = null)
        {
            Add(new unit_or_namespace(name), sc);
        }
        public uses_list(unit_or_namespace un, SourceContext sc = null)
        {
            Add(un, sc);
        }

        public uses_list Add(unit_or_namespace un, SourceContext sc = null)
        {
            units.Add(un);
            if (sc != null)
                source_context = sc;
            return this;
        }
        public uses_list AddUsesList(uses_list ul, SourceContext sc = null)
        {
            foreach (var un in ul.units)
                units.Add(un);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class unit_module
    {
        public unit_module(LanguageId _Language, unit_name _unit_name, interface_node _interface_part, implementation_node _implementation_part, statement_list _initialization_part, statement_list _finalization_part, SourceContext sc)
        {
            this._Language = _Language;
            this._unit_name = _unit_name;
            this._interface_part = _interface_part;
            this._implementation_part = _implementation_part;
            this._initialization_part = _initialization_part;
            this._finalization_part = _finalization_part;
            source_context = sc;
        }
    }

    public partial class program_module
    {
        public static program_module create(ident id, uses_list _used_units, block _program_block, using_list _using_namespaces, SourceContext sc = null)
        {
            var r = new program_module(new program_name(id), _used_units, _program_block, _using_namespaces, sc);
            r.Language = LanguageId.CommonLanguage;
            return r;
        }
        public static program_module create(ident id, uses_list _used_units, block _program_block)
        {
            var r = new program_module(new program_name(id), _used_units, _program_block, null);
            r.Language = LanguageId.CommonLanguage;
            return r;
        }
        public static program_module create(ident id, uses_list _used_units, block _program_block, SourceContext sc = null)
        {
            var r = new program_module(new program_name(id), _used_units, _program_block, null, sc);
            r.Language = LanguageId.CommonLanguage;
            return r;
        }
    }

    public partial class method_name
    {
        public method_name(string name, SourceContext sc = null) : this(null, null, new ident(name), null, sc)
        {
        }
        public override string ToString()
        {
            return meth_name.ToString();
        }
    }

    public partial class dot_node
    {
        public dot_node(ident left, ident right)
        {
            this.left = left;
            this.right = right;
        }
        public dot_node(ident left, ident right, SourceContext sc)
        {
            this.left = left;
            this.right = right;
            this.source_context = sc;
        }
        public override string ToString()
        {
            return left.ToString() + "." + right.ToString();
        }
    }

    public partial class goto_statement
    {
        static int lbnum = 0;

        static public string newLabelName()
        {
            lbnum++;
            return "lb#" + lbnum.ToString();
        }

        public override string ToString()
        {
            return "goto " + label;
        }

        public static goto_statement New
        {
            get { return new goto_statement(newLabelName()); }
        }
    }

    public partial class method_call
    {
        public override string ToString()
        {
            string s = dereferencing_value.ToString();
            if (parameters != null)
                s += "(" + parameters.ToString() + ")";
            else s += "()";
            return s;
        }
    }

    public partial class pascal_set_constant
    {
        //Добавляет во множество элемент
        public void Add(expression value)
        {
            values.Add(value);
        }
    }

    public partial class property_accessors
    {
        public property_accessors(ident read_accessor, ident write_accessor, SourceContext sc = null) 
            : this(new read_accessor_name(read_accessor), new write_accessor_name(write_accessor),sc)
        { }
    }

    public partial class simple_property
    {
        public simple_property(ident name, type_definition type, property_accessors accessors, SourceContext sc = null) 
            : this(name, type, null, accessors, null, null, definition_attribute.None,sc)
        { }
    }

    public partial class class_members
    {
        public static class_members Public
        {
            get { return new class_members(access_modifer.public_modifer); }
        }
        public static class_members Private
        {
            get { return new class_members(access_modifer.private_modifer); }
        }
        public class_members(declaration _declaration, SourceContext sc = null)
        {
            Add(_declaration, sc);
        }
        public class_members(access_modifer access)
        {
            access_mod = new access_modifer_node(access);
        }
        public class_members Add(declaration _declaration, SourceContext sc)
        {
            members.Add(_declaration);
            if (sc != null)
                source_context = sc;
            return this;
        }
        public class_members Add(params declaration[] decls)
        {
            foreach (var d in decls)
                members.Add(d);
            return this;
        }
    }

    public partial class class_body
    {
        public class_body(class_members _class_members, SourceContext sc = null)
        {
            Add(_class_members, sc);
        }
        public class_body Add(class_members _class_members, SourceContext sc = null)
        {
            class_def_blocks.Add(_class_members);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class class_definition
    {
        public class_definition(named_type_reference_list parents, class_body body, SourceContext sc = null) : this(parents, body, class_keyword.Class, null, null, class_attribute.None, false, sc)
        { is_auto = false; }
        public class_definition(class_body body, SourceContext sc = null) : this(null, body, sc)
        { is_auto = false; }

    }

    public partial class record_const
    {
        public record_const(record_const_definition _record_const_definition, SourceContext sc = null)
        {
            Add(_record_const_definition, sc);
        }
        public record_const Add(record_const_definition _record_const_definition, SourceContext sc = null)
        {
            rec_consts.Add(_record_const_definition);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class literal_const_line
    {
        public literal_const_line(literal _literal, SourceContext sc = null)
        {
            Add(_literal, sc);
        }
        public literal_const_line Add(literal _literal, SourceContext sc = null)
        {
            literals.Add(_literal);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class variant_list
    {
        public variant_list(variant _variant, SourceContext sc = null)
        {
            Add(_variant, sc);
        }
        public variant_list Add(variant _variant, SourceContext sc = null)
        {
            vars.Add(_variant);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class variant_types
    {
        public variant_types(variant_type _variant_type, SourceContext sc = null)
        {
            Add(_variant_type, sc);
        }
        public variant_types Add(variant_type _variant_type, SourceContext sc = null)
        {
            vars.Add(_variant_type);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class procedure_call
    {
        public procedure_call(ident name)
        {
            this._func_name = name;
        }

        public override string ToString()
        {
            return func_name.ToString();
        }
    }

    public partial class constructor
    {
        public constructor(formal_parameters fp, SourceContext sc = null) : this(null, fp, new procedure_attributes_list(), new method_name("Create"), false, false, null, null, sc)
        { }
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("constructor ");
            if (parameters != null)
                sb.Append("(" + parameters.ToString() + ")");
            else
                sb.Append("()");
            sb.Append(";");
            return sb.ToString();
        }
    }

    public partial class block
    {
        public block(statement_list code) : this(null, code, null)
        { }

        public static block Empty
        {
            get { return new block(null, new statement_list()); }
        }
    }

    public partial class case_variants
    {
        public case_variants(case_variant _case_variant, SourceContext sc = null)
        {
            Add(_case_variant, sc);
        }
        public case_variants Add(case_variant _case_variant, SourceContext sc = null)
        {
            variants.Add(_case_variant);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class var_def_list_for_record
    {
        public var_def_list_for_record(var_def_statement _var_def_statement, SourceContext sc = null)
        {
            Add(_var_def_statement, sc);
        }
        public var_def_list_for_record Add(var_def_statement _var_def_statement, SourceContext sc = null)
        {
            vars.Add(_var_def_statement);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class property_parameter_list
    {
        public property_parameter_list(property_parameter _property_parameter, SourceContext sc = null)
        {
            Add(_property_parameter, sc);
        }
        public property_parameter_list Add(property_parameter _property_parameter, SourceContext sc = null)
        {
            parameters.Add(_property_parameter);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class initfinal_part
    {
        public initfinal_part(syntax_tree_node stn1, statement_list init, syntax_tree_node stn2, statement_list fin, syntax_tree_node stn3, SourceContext sc)
        {
            _initialization_sect = init;
            _finalization_sect = fin;
            source_context = sc;
            init.left_logical_bracket = stn1;
            init.right_logical_bracket = stn2;
            if (fin != null)
            {
                fin.left_logical_bracket = stn2;
                fin.right_logical_bracket = stn3;
            }
        }
    }

    public partial class token_info
    {
        public override string ToString()
        {
            return text.ToLower();
        }
    }

    public partial class exception_handler_list
    {
        public exception_handler_list(exception_handler _exception_handler, SourceContext sc = null)
        {
            Add(_exception_handler, sc);
        }
        public exception_handler_list Add(exception_handler _exception_handler, SourceContext sc = null)
        {
            handlers.Add(_exception_handler);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class named_type_reference_list
    {
        public named_type_reference_list(named_type_reference _named_type_reference, SourceContext sc = null)
        {
            Add(_named_type_reference, sc);
        }
        public named_type_reference_list(string name)
        {
            Add(new named_type_reference(name));
        }
        public named_type_reference_list(string name1, string name2)
        {
            Add(new named_type_reference(name1));
            Add(new named_type_reference(name2));
        }
        public named_type_reference_list Add(named_type_reference _named_type_reference, SourceContext sc = null)
        {
            types.Add(_named_type_reference);
            if (sc != null)
                source_context = sc;
            return this;
        }
        public override string ToString()
        {
            return string.Join(",", this.types.Select(x => x.ToString()).ToArray());
        }
    }

    public partial class template_param_list
    {
        public template_param_list(string names)
        {
            foreach (var ntr in names.Split(',').Select(s => new named_type_reference(s)))
                Add(ntr);
        }

        public template_param_list(type_definition _type_definition, SourceContext sc = null)
        {
            Add(_type_definition, sc);
        }

        public template_param_list Add(type_definition _type_definition, SourceContext sc = null)
        {
            params_list.Add(_type_definition);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class int64_const
    {
        public override string ToString()
        {
            return val.ToString();
        }
    }

    public partial class uint64_const
    {
        public override string ToString()
        {
            return val.ToString();
        }
    }

    public partial class new_expr
    {
        public new_expr(type_definition type, expression_list pars, SourceContext sc = null) : this(type, pars, false, null, sc)
        { }
        public new_expr(string type, expression_list pars) : this(new named_type_reference(type), pars, false, null)
        { }
        public new_expr(string type) : this(new named_type_reference(type), expression_list.Empty, false, null)
        { }
        public override string ToString()
        {
            return "new "+this.type.ToString()+"("+this.params_list.ToString()+")";
        }
    }

    public partial class where_type_specificator_list
    {
        public where_type_specificator_list(type_definition _type_definition, SourceContext sc = null)
        {
            Add(_type_definition, sc);
        }
        public where_type_specificator_list Add(type_definition _type_definition, SourceContext sc = null)
        {
            defs.Add(_type_definition);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class where_definition_list
    {
       public where_definition_list(where_definition _where_definition, SourceContext sc = null)
        {
            Add(_where_definition, sc);
        }
        public where_definition_list Add(where_definition _where_definition, SourceContext sc = null)
        {
            defs.Add(_where_definition);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class var_statement
    {
        public var_statement(ident_list vars, type_definition type, expression iv)
        {
            var_def = new var_def_statement(vars, type, iv);
        }

        public var_statement(ident_list vars, type_definition type)
        {
            var_def = new var_def_statement(vars, type);
        }

        public var_statement(ident id, type_definition type, expression iv)
        {
            var_def = new var_def_statement(new ident_list(id), type, iv);
        }

        public var_statement(ident id, type_definition type)
        {
            var_def = new var_def_statement(new ident_list(id), type);
        }

        public var_statement(ident id, string type)
        {
            var_def = new var_def_statement(new ident_list(id), new named_type_reference(type));
        }

        public var_statement(ident id, expression iv)
        {
            var_def = new var_def_statement(new ident_list(id), null, iv);
        }

        public override string ToString()
        {
            return "var " + var_def.ToString();
        }
    }

    public partial class enumerator_list
    {
        public enumerator_list(enumerator _enumerator, SourceContext sc = null)
        {
            Add(_enumerator, sc);
        }
        public enumerator_list Add(enumerator _enumerator, SourceContext sc = null)
        {
            enumerators.Add(_enumerator);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class type_definition_attr_list
    {
        public type_definition_attr_list(type_definition_attr _type_definition_attr, SourceContext sc = null)
        {
            Add(_type_definition_attr, sc);
        }
        public type_definition_attr_list Add(type_definition_attr _type_definition_attr, SourceContext sc = null)
        {
            attributes.Add(_type_definition_attr);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class compiler_directive_list
    {
        public compiler_directive_list(compiler_directive _compiler_directive, SourceContext sc = null)
        {
            Add(_compiler_directive, sc);
        }
        public compiler_directive_list Add(compiler_directive _compiler_directive, SourceContext sc = null)
        {
            directives.Add(_compiler_directive);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class documentation_comment_list
    {
        public documentation_comment_list(documentation_comment_section _documentation_comment_section, SourceContext sc = null)
        {
            Add(_documentation_comment_section, sc);
        }
        public documentation_comment_list Add(documentation_comment_section _documentation_comment_section, SourceContext sc = null)
        {
            sections.Add(_documentation_comment_section);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class documentation_comment_section
    {
        public documentation_comment_section(documentation_comment_tag _documentation_comment_tag, SourceContext sc = null)
        {
            Add(_documentation_comment_tag, sc);
        }
        public documentation_comment_section Add(documentation_comment_tag _documentation_comment_tag, SourceContext sc = null)
        {
            tags.Add(_documentation_comment_tag);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class bracket_expr
    {
        public override string ToString()
        {
            return "(" + expr.ToString() + ")";
        }
    }

    public partial class simple_attribute_list
    {
        public simple_attribute_list(attribute _attribute, SourceContext sc = null)
        {
            Add(_attribute, sc);
        }
        public simple_attribute_list Add(attribute _attribute, SourceContext sc = null)
        {
            attributes.Add(_attribute);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class attribute_list
    {
        public attribute_list(simple_attribute_list _simple_attribute_list, SourceContext sc = null)
        {
            Add(_simple_attribute_list, sc);
        }
        public attribute_list Add(simple_attribute_list _simple_attribute_list, SourceContext sc = null)
        {
            attributes.Add(_simple_attribute_list);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class function_lambda_definition
    {
        public object RealSemTypeOfResExpr = null; // Result := ex; - семантический тип ex - нужно для лучшего выбора среди перегруженных методов с параметрами-лямбдами
        public object RealSemTypeOfResult = null;

        public function_lambda_definition(string name, formal_parameters formalPars, type_definition returnType, statement_list body, SourceContext sc)
        {
            statement_list _statement_list = body;
            expression_list _expression_list = new expression_list();
            ident_list identList = new ident_list();
            lambda_visit_mode = LambdaVisitMode.None;

            if (formalPars != null)
            {
                for (int i = 0; i < formalPars.params_list.Count; i++)
                {
                    for (int j = 0; j < formalPars.params_list[i].idents.idents.Count; j++)
                    {
                        identList.idents.Add(formalPars.params_list[i].idents.idents[j]);
                        _expression_list.expressions.Add(formalPars.params_list[i].idents.idents[j]);
                    }
                }
            }

            formal_parameters = formalPars;
            return_type = returnType;
            ident_list = identList;
            parameters = _expression_list;
            lambda_name = name;
            proc_body = _statement_list;
            source_context = sc;
        }
    }

    public partial class semantic_check
    {
        public semantic_check(string name, params syntax_tree_node[] pars)
        {
            CheckName = name;
            param.AddRange(pars);
        }
    }

    public partial class name_assign_expr_list
    {
        public name_assign_expr_list Add(name_assign_expr ne, SourceContext sc = null)
        {
            name_expr.Add(ne);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }

    public partial class unnamed_type_object
    {
        public string name()
        {
            return (new_ex.type as SyntaxTree.named_type_reference).names[0].name;
        }

        public void set_name(string nm)
        {
            var ntr = new_ex.type as SyntaxTree.named_type_reference;
            ntr.names[0].name = nm;
        }
    }

    public partial class yield_node
    {
        public override string ToString()
        {
            return "yield " + ex.ToString();
        }
    }
    public partial class sequence_type
    {
        public override string ToString()
        {
            return "sequence of " + this.elements_type.ToString();
        }
    }

    public partial class if_node
    {
        public if_node(expression _condition, statement _then_body)
        {
            this._condition = _condition;
            this._then_body = _then_body;
            this._else_body = null;
        }
    }
    public partial class empty_statement
    {
        public static empty_statement New
        {
            get { return new empty_statement(); }
        }
    }

    public partial class case_node
    {
        public case_node(expression _param, case_variants _conditions)
        {
            this._param = _param;
            this._conditions = _conditions;
            this._else_statement = null;
        }
        public case_node(expression _param)
        {
            this._param = _param;
            this._conditions = new case_variants();
            this._else_statement = null;
        }
    }

    public partial class labeled_statement
    {
        public labeled_statement(ident label_name)
        {
            this._label_name = label_name;
            this._to_statement = empty_statement.New;
        }
    }

    public partial class uses_closure
    {
        public uses_closure(uses_list st, SourceContext sc)
        {
            Add(st, sc);
        }
        public uses_closure Add(uses_list ul, SourceContext sc = null)
        {
            listunitsections.Add(ul);
            if (sc != null)
                source_context = sc;
            return this;
        }
    }


}

