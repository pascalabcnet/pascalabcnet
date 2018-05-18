using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace PascalABCCompiler.SyntaxTree
{
    /// <summary>
    /// Тип обхода дерева
    /// </summary>
    public enum TraversalType
    {
        /// <summary>
        /// Обход в ширину
        /// </summary>
        LevelOrder,

        /// <summary>
        /// Постфиксный обход
        /// </summary>
        PostOrder,

        /// <summary>
        /// Префиксный обход
        /// </summary>
        PreOrder
    }

    public enum Desc
    {
        All,
        DirectDescendants
    }

    public partial class syntax_tree_node
    {

        public syntax_tree_node Parent;
        public int FindIndex(syntax_tree_node node, Desc d = Desc.All)
        {
            int ind = -1;

            var count = d == Desc.All ? subnodes_count : subnodes_without_list_elements_count;

            for (var i = 0; i < count; i++)
                if (node == this[i])
                {
                    ind = i;
                    break;
                }
            if (ind == -1)
                throw new Exception(string.Format("У элемента {0} не найден {1} среди дочерних\n", this, node));
            return ind;
        }

        /*public void ReplaceDescendant(syntax_tree_node from, syntax_tree_node to, Desc d = Desc.All) // есть риск, что типы не совпадут
        {
            var ind = FindIndex(from,d);
            this[ind] = to;
        }*/

        // Безопасная версия Replace - не сработает если менять foreach_node на if_node. Плохо
        public void ReplaceDescendant<T,T1>(T from, T1 to, Desc d = Desc.All) where T: syntax_tree_node where T1 : T
        {
            var ind = FindIndex(from,d);
            this[ind] = to;
            to.Parent = from.Parent;
        }

        public void ReplaceDescendantUnsafe(syntax_tree_node from, syntax_tree_node to, Desc d = Desc.All)
        {
            var ind = FindIndex(from, d);
            this[ind] = to;
            to.Parent = from.Parent;
        }

        /// <summary>
        /// Находит последнего потомка, удовлетворяющего условию. Возвращает null, если такой не найден.
        /// </summary>
        /// <param name="condition">Условие</param>
        /// <returns>Найденный узел, либо null</returns>
        public syntax_tree_node FindLast(Predicate<syntax_tree_node> condition)
        {
            for (int i = subnodes_count - 1; i >= 0; i--)
            {
                if (condition(this[i]))
                    return this[i];
            }

            return null;
        }

        /// <summary>
        /// Получает коллекцию предков текущего узла
        /// </summary>
        /// <param name="includeSelf">Включить в список текущий узел</param>
        /// <returns>Коллекция предков узла</returns>
        public IEnumerable<syntax_tree_node> AscendantNodes(bool includeSelf = false)
        {
            if (includeSelf)
                yield return this;

            var parentNode = Parent;
            while (parentNode != null)
            {
                yield return parentNode;
                parentNode = parentNode.Parent;
            }
        }

        /// <summary>
        /// Получает коллекцию узлов поддерева, в котором корнем является текущий узел. 
        /// </summary>
        /// <param name="traversalType">Тип обхода дерева. Определяет порядок добавления узлов в коллекцию</param>
        /// <param name="descendIntoChildren">Опциональная функция, позволяющая указать, нужно ли посещать потомков конкретного узла</param>
        /// <param name="includeSelf">Позволяет включить текущий узел в список</param>
        /// <returns>Коллекция узлов поддерева</returns>
        /// <exception cref="NotImplementedException">Выбрасывается при отсутствии реализации для заданного обхода</exception>
        public IEnumerable<syntax_tree_node> DescendantNodes(
            TraversalType traversalType = TraversalType.PostOrder, 
            Func<syntax_tree_node, bool> descendIntoChildren = null, 
            bool includeSelf = false)
        {
            switch (traversalType)
            {
                case TraversalType.LevelOrder:
                    return DescendantNodesLevelOrder(descendIntoChildren, includeSelf);
                case TraversalType.PostOrder:
                    return DescendantNodesPostOrder(descendIntoChildren, includeSelf);
                case TraversalType.PreOrder:
                    return DescendantNodesPreOrder(descendIntoChildren, includeSelf);
                default:
                    throw new NotImplementedException("Данный вид обхода не поддерживается");
            }
        }

        /// <summary>
        /// Получает список узлов поддерева, в котором корнем является текущий узел. 
        /// Порядок добавления в список: родитель, затем потомки.
        /// </summary>
        /// <param name="descendIntoChildren">Опциональная функция, позволяющая указать, нужно ли посещать потомков конкретного узла</param>
        /// <param name="includeSelf">Позволяет включить текущий узел в список</param>
        /// <returns>Список узлов поддерева</returns>
        private IEnumerable<syntax_tree_node> DescendantNodesPreOrder(Func<syntax_tree_node, bool> descendIntoChildren, bool includeSelf)
        {
            var stack = new Stack<syntax_tree_node>();

            if (includeSelf)
                stack.Push(this);
            else
                for (int childIndex = subnodes_count - 1; childIndex >= 0; childIndex--)
                    if (this[childIndex] != null)
                        stack.Push(this[childIndex]);

            while (stack.Count > 0)
            {
                syntax_tree_node node = stack.Pop();

                if (descendIntoChildren == null || descendIntoChildren(node))
                    for (int childIndex = node.subnodes_count - 1; childIndex >= 0; childIndex--)
                    {
                        var child = node[childIndex];

                        if (child == null)
                            continue;

                        stack.Push(child);
                    }

                yield return node;
            }
        }

        /// <summary>
        /// Получает список узлов поддерева, в котором корнем является текущий узел. 
        /// Порядок добавления в список: потомки, затем родитель.
        /// </summary>
        /// <param name="descendIntoChildren">Опциональная функция, позволяющая указать, нужно ли посещать потомков конкретного узла</param>
        /// <param name="includeSelf">Позволяет включить текущий узел в список</param>
        /// <returns>Список узлов поддерева</returns>
        private IEnumerable<syntax_tree_node> DescendantNodesPostOrder(Func<syntax_tree_node, bool> descendIntoChildren, bool includeSelf)
        {
            var stack = new Stack<syntax_tree_node>();

            if (includeSelf)
                stack.Push(this);
            else
                for (int childIndex = subnodes_count - 1; childIndex >= 0; childIndex--)
                    if (this[childIndex] != null)
                        stack.Push(this[childIndex]);

            syntax_tree_node lastReturnedNode = null;
            while (stack.Count > 0)
            {
                syntax_tree_node node = stack.Peek();
                bool isLeafNode = true;

                // Если мы не должны посещать потомков узла или уже добавили их в список,
                // то не кладем их на стек
                if (!ReferenceEquals(lastReturnedNode, node.FindLast(x => x != null)))
                if (descendIntoChildren == null || 
                    descendIntoChildren(node))
                    for (int childIndex = node.subnodes_count - 1; childIndex >= 0; childIndex--)
                    {
                        var child = node[childIndex];

                        if (child == null)
                            continue;

                        stack.Push(child);
                        isLeafNode = false;
                    }

                if (isLeafNode)
                {
                    stack.Pop();
                    lastReturnedNode = node;
                    yield return node;
                }
            }
        }

        /// <summary>
        /// Получает список узлов поддерева, в котором корнем является текущий узел. 
        /// Порядок добавления в список: узлы первого уровня, затем второго и т.д.
        /// </summary>
        /// <param name="descendIntoChildren">Опциональная функция, позволяющая указать, нужно ли посещать потомков конкретного узла</param>
        /// <param name="includeSelf">Позволяет включить текущий узел в список</param>
        /// <returns>Список узлов поддерева</returns>
        private IEnumerable<syntax_tree_node> DescendantNodesLevelOrder(Func<syntax_tree_node, bool> descendIntoChildren, bool includeSelf)
        {
            var queue = new Queue<syntax_tree_node>();

            if (includeSelf)
                queue.Enqueue(this);
            else
                for (int childIndex = 0; childIndex < subnodes_count; childIndex++)
                    if (this[childIndex] != null)
                        queue.Enqueue(this[childIndex]);

            while (queue.Count > 0)
            {
                syntax_tree_node node = queue.Dequeue();

                if (descendIntoChildren == null || descendIntoChildren(node))
                    for (int childIndex = 0; childIndex < node.subnodes_count; childIndex++)
                    {
                        var child = node[childIndex];

                        if (child == null)
                            continue;

                        queue.Enqueue(child);
                    }

                yield return node;
            }
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

        public statement_list(IEnumerable<statement> sts)
        {
            AddMany(sts);
        }

        public statement_list(params statement[] sts)
        {
            AddMany(sts);
        }


        //-- List members begin
        public List<statement> list
        {
            get { return subnodes; }
        }

        public void AddMany(IEnumerable<statement> els)
        {
            list.AddRange(els);
            foreach (var elem in els)
                if (elem != null)
                    elem.Parent = this;
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

        public static bin_expr LogicalAnd(expression left, expression right)
        {
            return new bin_expr(left, right, Operators.LogicalAND);
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
            return val.ToString(System.Globalization.CultureInfo.InvariantCulture);
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

        public static named_type_reference Boolean => new named_type_reference("boolean");
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
        //-- List members begin
        public List<var_def_statement> list
        {
            get { return var_definitions; }
        }

        //-- List members end
    }

    public partial class ident_list
    {
        public ident_list(params ident[] idents)
        {
            this.idents = idents.ToList();
        }

        public override string ToString()
        {
            if (idents.Count == 0)
                return "";

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

        //-- List members end
    }

    public partial class var_def_statement
    {
        public var_def_statement(ident_list vars, type_definition vars_type, SourceContext sc = null) : this(vars, vars_type, null, definition_attribute.None, false, sc)
        { }

        public var_def_statement(ident_list vars, type_definition vars_type, expression iv, SourceContext sc = null) : this(vars, vars_type, iv, definition_attribute.None, false, sc)
        { }

        public var_def_statement(ident id, type_definition type, SourceContext sc = null) : this(new ident_list(id), type, sc)
        { }

        public var_def_statement(ident id, type_definition type, expression iv, SourceContext sc = null) : this(new ident_list(id), type, iv,sc)
        { }

        public var_def_statement(ident id, expression iv, SourceContext sc = null) : this(new ident_list(id), null, iv, sc)
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

        //-- List members begin
        public List<declaration> list
        {
            get { return defs; }
        }
        
        public static statement_list Empty
        {
            get { return new statement_list(); }
        }
        //-- List members end
    }

    public partial class program_tree
    {

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
        public indexer(addressed_value av, expression ex, SourceContext sc = null)
        {
            this.dereferencing_value = av;
            this.indexes = new expression_list(ex);
            this.source_context = sc;
        }


        public override string ToString()
        {
            return base.ToString() + "[" + indexes.ToString() + "]";
        }
    }

    public partial class indexers_types
    {

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
        public procedure_attributes_list(proc_attribute attr, SourceContext sc = null) : this(new procedure_attribute(attr), sc)
        { }
    }

    public partial class procedure_header
    {
        // frninja 20/05/16 - для методов хелперов yield
        public bool is_yield_helper = false;
        // end frninja

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
            if (name != null)
                sb.Append(name.ToString());
            else
                sb.Append("NONAME");

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
            if (return_type!=null)
                sb.Append(": " + return_type.ToString() + ";");
            return sb.ToString();
        }
    }

    public partial class procedure_definition
    {
        public bool has_yield = false;
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

    }

    public partial class consts_definitions_list
    {

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
        public dot_node(ident left, ident right, SourceContext sc = null)
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

    public partial class dot_node
    {
        public static dot_node NewP(addressed_value avl, addressed_value avr, SourceContext sc)
        {
            var dn = new dot_node(avl, avr, sc);
            if (avl != null) avl.Parent = dn;
            if (avr != null) avr.Parent = dn;
            return dn;
        }
    }


    public partial class method_call
    {
        public static method_call NewP(addressed_value av, expression_list el, SourceContext sc)
        {
            var mc = new method_call(av, el, sc);
            if (av != null) av.Parent = mc;
            if (el != null) el.Parent = mc;
            return mc;
        }

        /// <summary>
        /// Простое имя метода. Возвращает null, если не удалось такое получить.
        /// </summary>
        public string SimpleName
        {
            get
            {
                var dv = dereferencing_value as ident;
                if (dv == null)
                    return null;
                else return dv.name;
            }
        }

        /// <summary>
        /// Количество параметров метода
        /// </summary>
        public int ParametersCount
        {
            get
            {
                if (parameters != null)
                    return parameters.subnodes_count;
                else
                    return 0;
            }
        }

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
            : this(name, type, null, accessors, null, null, definition_attribute.None,proc_attribute.attr_none,sc)
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

        public class_members(access_modifer access)
        {
            access_mod = new access_modifer_node(access);
        }

        public class_members Add(params declaration[] decls)
        {
            foreach (var d in decls)
                members.Add(d);
            return this;
        }
    }

    public partial class class_body_list
    {

    }

    public partial class class_definition
    {
        public class_definition(named_type_reference_list parents, class_body_list body, SourceContext sc = null) : this(parents, body, class_keyword.Class, null, null, class_attribute.None, false, sc)
        { is_auto = false; }
        public class_definition(class_body_list body, SourceContext sc = null) : this(null, body, sc)
        { is_auto = false; }

    }

    public partial class record_const
    {

    }

    public partial class literal_const_line
    {

    }

    public partial class variant_list
    {

    }

    public partial class variant_types
    {

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

    }

    public partial class var_def_list_for_record
    {

    }

    public partial class property_parameter_list
    {

    }

    public partial class initfinal_part
    {
        public initfinal_part(token_info stn1, statement_list init, token_info stn2, statement_list fin, token_info stn3, SourceContext sc)
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

    }

    public partial class named_type_reference_list
    {
        public named_type_reference_list(string name)
        {
            Add(new named_type_reference(name));
        }
        public named_type_reference_list(string name1, string name2)
        {
            Add(new named_type_reference(name1));
            Add(new named_type_reference(name2));
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
            return "new "+ (this.type != null ? this.type.ToString() : "NOTYPE") + "(" +(this.params_list != null ? this.params_list.ToString() : "NOPARAMS") + ")";
        }
    }

    public partial class where_type_specificator_list
    {

    }

    public partial class where_definition_list
    {

    }

    public partial class var_statement
    {
        public var_statement(ident_list vars, type_definition type, expression iv) : this(new var_def_statement(vars, type, iv))
        {
        }

        public var_statement(ident_list vars, type_definition type): this(new var_def_statement(vars, type))
        {
        }

        public var_statement(ident id, type_definition type, expression iv) : this(new var_def_statement(new ident_list(id), type, iv))
        {
        }

        public var_statement(ident id, type_definition type) : this(new var_def_statement(new ident_list(id), type))
        {
        }

        public var_statement(ident id, string type) : this(new var_def_statement(new ident_list(id), new named_type_reference(type)))
        {
        }

        public var_statement(ident id, expression iv) :this(new var_def_statement(new ident_list(id), null, iv))
        {
        }

        public var_statement(ident id, expression iv,SourceContext sc) : this(new var_def_statement(new ident_list(id), null, iv, sc))
        {
        }

        public override string ToString()
        {
            return "var " + var_def.ToString();
        }
    }

    public partial class enumerator_list
    {

    }

    public partial class type_definition_attr_list
    {

    }

    public partial class compiler_directive_list
    {

    }

    public partial class documentation_comment_list
    {

    }

    public partial class documentation_comment_section
    {

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

    }

    public partial class attribute_list
    {

    }

    public partial class function_lambda_definition
    {
        public object RealSemTypeOfResExpr = null; // Result := ex; - семантический тип ex - нужно для лучшего выбора среди перегруженных методов с параметрами-лямбдами
        public object RealSemTypeOfResult = null;

        public function_lambda_definition(string name, formal_parameters formalPars, type_definition returnType, statement_list body, int usedkw, SourceContext sc)
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
            usedkeyword = usedkw;
            source_context = sc;
        }
        public function_lambda_definition(string name, formal_parameters formalPars, type_definition returnType, statement_list body, SourceContext sc) :
            this(name, formalPars, returnType, body, 0, sc)
        {
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

    }

    // frninja 12/05/16 - хелперы для yield
    public partial class yield_unknown_expression_type : type_definition
    {
        protected var_def_statement _Vds;

        public var_def_statement Vds
        {
            get { return _Vds; }
            set { _Vds = value; }
        }

        public yield_unknown_expression_type(var_def_statement vds)
        {
            this.Vds = vds;
        }
    }

    public partial class yield_unknown_ident : ident
    {
        protected ident _UnknownID;
        protected ident _ClassName;
        protected bool _IsYieldInStaticMethod;

        public ident UnknownID
        {
            get { return _UnknownID; }
            set { _UnknownID = value; }
        }

        public ident ClassName
        {
            get { return _ClassName; }
            set { _ClassName = value; }
        }

        public bool IsYieldInStaticMethod
        {
            get { return _IsYieldInStaticMethod; }
            set { _IsYieldInStaticMethod = value; }
        }

        ///<summary>
        ///Конструктор с параметрами.
        ///</summary>
        public yield_unknown_ident(ident _UnknownID, ident _ClassName, bool isYieldInStaticMethod = false)
        {
            this._name = _UnknownID.name;
            this._UnknownID = _UnknownID;
            this._ClassName = _ClassName;
            this._IsYieldInStaticMethod = isYieldInStaticMethod;
        }

        ///<summary>
        ///Конструктор с параметрами.
        ///</summary>
        public yield_unknown_ident(ident _UnknownID, ident _ClassName, bool isYieldInStaticMethod, SourceContext sc)
        {
            this._name = _UnknownID.name;
            this._UnknownID = _UnknownID;
            this._ClassName = _ClassName;
            this._IsYieldInStaticMethod = isYieldInStaticMethod;
            source_context = sc;
        }
    }

    public partial class yield_unknown_foreach_type : type_definition
    {
        protected foreach_stmt _unknown_foreach;

        public foreach_stmt unknown_foreach
        {
            get { return _unknown_foreach; }
            set { _unknown_foreach = value; }
        }

        ///<summary>
        ///Конструктор с параметрами.
        ///</summary>
        public yield_unknown_foreach_type(foreach_stmt _unknown_foreach)
        {
            this._unknown_foreach = _unknown_foreach;
        }

        ///<summary>
        ///Конструктор с параметрами.
        ///</summary>
        public yield_unknown_foreach_type(foreach_stmt _unknown_foreach, SourceContext sc)
        {
            this._unknown_foreach = _unknown_foreach;
            source_context = sc;
        }

    }

    public partial class yield_unknown_foreach_type_ident : ident
    {
        protected foreach_stmt _unknown_foreach;

        public foreach_stmt unknown_foreach
        {
            get { return _unknown_foreach; }
            set { _unknown_foreach = value; }
        }

        ///<summary>
        ///Конструктор с параметрами.
        ///</summary>
        public yield_unknown_foreach_type_ident(foreach_stmt _unknown_foreach)
        {
            this._unknown_foreach = _unknown_foreach;
        }

        ///<summary>
        ///Конструктор с параметрами.
        ///</summary>
        public yield_unknown_foreach_type_ident(foreach_stmt _unknown_foreach, SourceContext sc)
        {
            this._unknown_foreach = _unknown_foreach;
            source_context = sc;
        }
    }

	public partial class array_type
    {
        public override string ToString()
        {
            return "array of " + this.elements_type.ToString();
        }
    }

	public partial class function_lambda_definition
    {
        public override string ToString()
        {
            return "" + this._ident_list.ToString() + " -> lambda_body";
        }
    }

    public partial class nil_const 
    {
        public override string ToString()
        {
            return "nil";
        }
    }
    
	public partial class access_modifer_node
    {
        public override string ToString()
        {
            return this.access_level.ToString().Replace("_modifier","");
        }
    }

	public partial class yield_unknown_ident
    {
        public override string ToString()
        {
            return this.UnknownID.ToString();
        }
    }

    public partial class statement
    {
        public statement_list ToStatementList()
        {
            var stl = this as statement_list;
            if (stl != null)
                return stl;
            else return new statement_list(this);
        }
    }

    public partial class addressed_value
    {
        public indexer indexer(expression ex, SourceContext sc = null)
        {
            return new SyntaxTree.indexer(this, ex, sc);
        }
        public dot_node dot_node(ident id, SourceContext sc = null)
        {
            return new SyntaxTree.dot_node(this, id, sc);
        }
    }

    public partial class expression
    {
        public expression Plus(expression e)
        {
            return new bin_expr(this, e, Operators.Plus);
        }
        public expression Minus(expression e)
        {
            return new bin_expr(this, e, Operators.Minus);
        }
        public static implicit operator expression(int i)
        {
            return new int32_const(i);
        }
    }

    public partial class slice_expr
    {
        public override string ToString() => this.v + "[" + this.from + ":" + this.to + ":" + this.step + "]";
    }

    public partial class slice_expr_question
    {
        public slice_expr_question(addressed_value v, expression from, expression to, expression step) : base(v, from, to, step)
        { }
        public slice_expr_question(addressed_value v, expression from, expression to, expression step, SourceContext sc) : base(v, from, to, step, sc)
        { }
    }

    public partial class sugared_addressed_value
    {
        public static sugared_addressed_value NewP(object sug, addressed_value av, SourceContext sc)
        {
            var res = new sugared_addressed_value(sug, av, sc);
            av.Parent = res;
            return res;
        }
        public override string ToString()
        {
            return "{sug}" + this.new_addr_value;
        }

    }

    public partial class question_colon_expression
    {
        public override string ToString() => this.condition + "?" + this.ret_if_true + ":" + this.ret_if_false;
    }

    public partial class dot_question_node
    {
        public override string ToString() => left.ToString() + "?." + right.ToString();
    }

    public partial class for_node
    {
        public for_node(ident loop_variable, expression init_value, expression finish_value, statement stmt, SourceContext sc):
            this(loop_variable, init_value, finish_value,stmt,for_cycle_type.to, null,null,true)
        {

        }
        public override string ToString() => "for var " + this.loop_variable.ToString() + " := " + this.initial_value.ToString() + " to " + this.finish_value.ToString()+" do \n"+this.statements.ToString();
    }

    public partial class loop_stmt
    {
        public override string ToString() => "loop " + this.count.ToString() + " do \n" + this.stmt.ToString();
    }

    public partial class lambda_inferred_type
    {
        public override string ToString() => "lam_inferred";
    }

    public partial class desugared_deconstruction
    {
        public bool HasAllExplicitTypes => variables.definitions.All(x => x.vars_type != null);

        public desugared_deconstruction(List<var_def_statement> variables, expression target, SourceContext context = null) 
            : this(new deconstruction_variables_definition(variables), target, context)
        { }

        public var_statement[] WithTypes(type_definition[] types)
        {
            var_statement[] result = new var_statement[types.Length]; 
            Debug.Assert(types.Length == variables.definitions.Count, "Inconsistent types count");

            for (int i = 0; i < variables.definitions.Count; i++)
            {
                variables.definitions[i].vars_type = types[i];
                result[i] = new var_statement(variables.definitions[i]);
            }

            return result;
        }

        public override string ToString() => $"var {string.Join(", ", variables.definitions)}";
    }

    public partial class is_pattern_expr
    {
        public override string ToString() => $"{left} is {right}";
    }

    public partial class deconstructor_pattern
    {
        public bool IsRecursive => parameters.Any(x => x is recursive_deconstructor_parameter);

        public override string ToString() => $"{type}({string.Join(", ", parameters.Select(x => x.ToString()))})";
    }

    public partial class var_deconstructor_parameter
    {
        public override string ToString() => identifier.ToString() + (type == null ? "" : $": {type}");
    }

    public partial class recursive_deconstructor_parameter
    {
        public override string ToString() => pattern.ToString();
    }
}

