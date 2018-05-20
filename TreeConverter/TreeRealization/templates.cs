// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
//Здесь описана реализация неуправляемых шаблонов
//Файлом владеет ssyy.
using System;
using System.Collections.Generic;
using PascalABCCompiler.TreeConverter;
using System.Linq;

namespace PascalABCCompiler.TreeRealization
{
    //Хранит информацию о методе, описанном вне класса
    public class procedure_definition_info
    {
        //Пространство, где метод описан
        public common_namespace_node nspace;

        //Сам метод в виде синтаксического дерева
        public SyntaxTree.procedure_definition proc;

        public procedure_definition_info(common_namespace_node _nspace,
            SyntaxTree.procedure_definition _proc)
        {
            nspace = _nspace;
            proc = _proc;
        }
    }


    public class template_class : definition_node, PascalABCCompiler.SemanticTree.ITemplateClass
    {
        public override string ToString() => type_dec.type_name + "=" + type_dec.type_def.ToString();

        public static bool check_template_definitions = true;

        public static bool TypeDependedFromTemplate(type_node tn)
        {
            common_type_node ctn = tn as common_type_node;
            if (ctn == null)
            {
                return false;
            }
            if (ctn.original_template != null)
            {
                return true;
            }
            switch (tn.type_special_kind)
            {
                case PascalABCCompiler.SemanticTree.type_special_kind.array_kind:
                case PascalABCCompiler.SemanticTree.type_special_kind.set_type:
                //case PascalABCCompiler.SemanticTree.type_special_kind.typed_file:
                    return TypeDependedFromTemplate(tn.element_type);
            }
            return false;
        }

        public static void AddUsingListToScope(SymbolTable.Scope scope, using_namespace_list unl)
        {
            SymbolTable.UnitPartScope ups = scope as SymbolTable.UnitPartScope;
            if (ups == null) return;
            foreach (SymbolTable.Scope sc in ups.TopScopeArray)
            {
                NetHelper.NetScope netsc = sc as NetHelper.NetScope;
                if (netsc != null && netsc.used_namespaces.Count == 0)
                {
                    using_namespace_list new_unl = new using_namespace_list();
                    foreach (using_namespace un in unl)
                    {
                        List<SymbolInfo> sil = netsc.FindOnlyInScope(un.namespace_name);
                        if ((sil.FirstOrDefault().sym_info as compiled_namespace_node) != null)
                        {
                            new_unl.AddElement(un);
                        }
                    }
                    netsc.used_namespaces = new_unl;
                }
            }
        }

        //добавит, если такого нет
        public string GetTemplateInstanceName(List<type_node> instance_params)
        {
            string rez = _name;
            bool first = true;
            foreach (type_node tnode in instance_params)
            {
                rez += ((first) ? "<" : ",") + tnode.name;
                first = false;
            }
            //(ssyy!!!) Проверить корректность наличия символа запятая в имени
            rez += ">";
            if (!_instances.ContainsKey(rez))
            {
                List<List<type_node>> tlist = new List<List<type_node>>();
                tlist.Add(instance_params);
                _instances.Add(rez, tlist);
                return rez;
            }
            else
            {
                List<List<type_node>> inst = _instances[rez];
                for (int i = 0; i < inst.Count; i++)
                {
                    bool equal = true;
                    for (int j = 0; j < instance_params.Count; j++)
                    {
                        if (inst[i][j] != instance_params[j])
                        {
                            equal = false;
                        }
                    }
                    if (equal)
                    {
                        return (i == 0) ? rez : (rez + '$' + i.ToString());
                    }
                }
                inst.Add(instance_params);
                return rez + '$' + (inst.Count - 1).ToString();
            }
        }

        public List<type_node> GetParamsList(common_type_node template_instance)
        {
            /*string key = instance_name;
            int num = 0;
            if (instance_name[instance_name.Length - 1] != '>')
            {
                int i = instance_name.Length - 1;
                while (instance_name[i] != '$')
                {
                    i--;
                }
                string snum = instance_name.Substring(i + 1);
                num = Convert.ToInt32(snum);
                key = instance_name.Substring(0, i);
            }
            List<List<type_node>> founded = _instances[key];
            return founded[num];*/
            List<type_node> i_params;
            if (_instance_params.TryGetValue(template_instance, out i_params))
            {
                return i_params;
            }
            else
            {
                return null;
            }
        }

        private SyntaxTree.type_declaration _type_decl; //Синтаксическое дерево шаблона
        private string _name;                           //Имя шаблона
        private common_namespace_node _cnn;             //Пространство, где описан шаблон
        private document _doc;                          //Документ, т.е. файл, где описан шаблон
        private using_namespace_list _unl;              //Список подключенных к _cnn пространств
        private using_namespace_list _unl2 = null;      //То же для пространства внешних методов
        private bool _ForwardDeclarationOnly = false;   //true, если пока есть только предописание
        private bool _is_synonym = false;               //true, если синоним шаблонного типа

        public bool is_synonym
        {
            get { return _is_synonym; }
            set { _is_synonym = value; }
        }

        //Список методов, определённых вне шаблонного класса
        private List<procedure_definition_info> _external_methods = new List<procedure_definition_info>();

        private Dictionary<string, List<List<type_node>>> _instances = new Dictionary<string, List<List<type_node>>>();

        private Dictionary<common_type_node, List<type_node>> _instance_params = new Dictionary<common_type_node, List<type_node>>();

        public string CreateTemplateInstance(List<type_node> instance_params, location loc)
        {
            SyntaxTree.class_definition cl_def = _type_decl.type_def as SyntaxTree.class_definition;
            SyntaxTree.template_type_name ttn = _type_decl.type_name as SyntaxTree.template_type_name;
            //if (cl_def == null)
            //{
            //    throw new PascalABCCompiler.TreeConverter.CompilerInternalError("No body definition in template class.");
            //}
            //if (cl_def.template_args == null || cl_def.template_args.idents == null)
            //{
            //    throw new PascalABCCompiler.TreeConverter.CompilerInternalError("No template arguments in syntax tree.");
            //}
            List<SyntaxTree.ident> template_formals = (_is_synonym) ?
                ttn.template_args.idents : cl_def.template_args.idents;
            if (instance_params.Count != template_formals.Count)
            {
                throw new PascalABCCompiler.TreeConverter.SimpleSemanticError(loc, "TEMPLATE_ARGUMENTS_COUNT_MISMATCH");
            }
            return GetTemplateInstanceName(instance_params);
             
        }

        public SyntaxTree.type_declaration type_dec
        {
            get
            {
                return _type_decl;
            }
            set
            {
                _type_decl = value;
            }
        }

        public string name
        {
            get
            {
                return _name;
            }
        }

        public common_namespace_node cnn
        {
            get
            {
                return _cnn;
            }
        }

        public document cur_document
        {
            get
            {
                return _doc;
            }
        }

        public using_namespace_list using_list
        {
            get
            {
                return _unl;
            }
        }

        public using_namespace_list using_list2
        {
            get
            {
                return _unl2;
            }
            set
            {
                _unl2 = value;
            }
        }

        public bool ForwardDeclarationOnly
        {
            get
            {
                return _ForwardDeclarationOnly;
            }
            set
            {
                _ForwardDeclarationOnly = value;
            }
        }

        public List<procedure_definition_info> external_methods
        {
            get
            {
                return _external_methods;
            }
            set
            {
                _external_methods = value;
            }
        }

        public Dictionary<common_type_node, List<type_node>> instance_params
        {
            get
            {
                return _instance_params;
            }
        }

        public template_class(SyntaxTree.type_declaration type_decl, string name,common_namespace_node cnn,/*common_type_node ctn,location loc,*/document doc,using_namespace_list unl)
        {
            _cnn = cnn;
            _type_decl = type_decl;
            _name = name;
            _doc = doc;
            
            //(ssyy) using - список, по-видимому, можно только копировать, т.к. Николай его периодически чистит.
            _unl = new using_namespace_list();
            foreach (using_namespace un in unl)
            {
                _unl.AddElement(un);
            }
        }

        public override general_node_type general_node_type
        {
            get
            {
                return general_node_type.template_type;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.template_type;
            }
        }

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            throw new PascalABCCompiler.TreeConverter.CompilerInternalError("Template class can't be visit.");
        }

        private byte[] tree=null;

        public byte[] serialized_tree
        {
            get
            {
                if (!this.is_synonym)
                    return null;
                if (tree == null)
                try
                {
                    SyntaxTree.SyntaxTreeStreamWriter stw = new SyntaxTree.SyntaxTreeStreamWriter();
                    byte[] buf = new byte[10000];
                    stw.bw = new System.IO.BinaryWriter(new System.IO.MemoryStream(buf));
                    this.type_dec.visit(stw);
                    tree = new byte[(int)stw.bw.BaseStream.Position+1];
                    tree[0] = 1;
                    Array.Copy(buf, 0, tree, 1, (int)stw.bw.BaseStream.Position);
                }
                catch (Exception ex)
                {

                }
                return tree;
            }
        }

    }


    //члены неопределенного типа (типовой переменной)
    public class indefinite_definition_node : function_node
    {
        private type_node _cont_class;

        public type_node cont_class
        {
            get { return _cont_class; }
        }

        private string _name;

        public override string name
        {
            get { return _name; }
        }

        public indefinite_definition_node(string node_name, type_node connected_class)
        {
            _name = node_name;
            _cont_class = connected_class;
            return_value_type = new indefinite_type_node();
        }

        public override general_node_type general_node_type
        {
            get { return general_node_type.function_node; }
        }

        public override semantic_node_type semantic_node_type
        {
            get { return semantic_node_type.indefinite_definition_node; }
        }

        public override PascalABCCompiler.SemanticTree.node_kind node_kind
        {
            get { return PascalABCCompiler.SemanticTree.node_kind.indefinite; }
        }

        public override PascalABCCompiler.SemanticTree.node_location_kind node_location_kind
        {
            get { return PascalABCCompiler.SemanticTree.node_location_kind.indefinite; }
        }
    }

    public class indefinite_functions_set : indefinite_definition_node
    {

        private List<function_node> _functions = null;

        public List<function_node> functions
        {
            get { return _functions; }
            set { _functions = value; }
        }

        public void Add(function_node function)
        {
            if (_functions == null)
            {
                _functions = new List<function_node>();
            }
            _functions.Add(function);
            UpdateReturnType();
        }

        public indefinite_functions_set()
            : base(null, null)
        {
            return_value_type = null;
        }

        public indefinite_functions_set(List<function_node> functions)
            : base(null, null)
        {
            _functions = functions;
            UpdateReturnType();
        }

        private bool _ret_type_generated = false;

        public void UpdateReturnType()
        {
            if (!_ret_type_generated)
            {
                type_node tn = _functions[0].return_value_type;
                int i = 1;
                while (i < _functions.Count)
                {
                    if (_functions[i].return_value_type == tn)
                        ++i;
                    else
                        break;
                }
                if (i == _functions.Count)
                {
                    return_value_type = tn;
                }
                else
                {
                    _ret_type_generated = true;
                    return_value_type = new indefinite_type_node();
                }
            }
        }
    }
    
    
    //класс для представления неопределенных типов (вспомогательных типовых переменных)
    public class indefinite_type_node : type_node
    {
        private string _name;

        public indefinite_type_node()
        {
            _name = "$it";
        }

        public indefinite_type_node(string name)
        {
            _name = name;
        }

        //public static List<type_node> get_indefinite_type_list(int count)
        //{
        //    List<type_node> rez = new List<type_node>(count);
        //    for (int i = 0; i < count; i++)
        //    {
        //        rez.Add(new indefinite_type_node());
        //    }
        //    return rez;
        //}

        public override SymbolInfo find_first_in_type(string name, bool no_search_in_extension_methods = false)
        {
            indefinite_definition_node idn = new indefinite_definition_node(name, this);
            return new SymbolInfo(idn, access_level.al_public, symbol_kind.sk_indefinite);
        }
        public override List<SymbolInfo> find_in_type(string name, bool no_search_in_extension_methods = false)
        {
            indefinite_definition_node idn = new indefinite_definition_node(name, this);
            return new List<SymbolInfo> { new SymbolInfo(idn, access_level.al_public, symbol_kind.sk_indefinite) };
        }

        public override List<SymbolInfo> find(string name, bool no_search_in_extension_methods = false)
        {
            return find_in_type(name);
        }

        public override void add_name(string name, SymbolInfo si)
        {
            //ничего не делаем
        }

        public override void add_generated_name(string name, SymbolInfo si)
        {
            //ничего не делаем
        }

        public override void clear_generated_names()
        {
            //ничего не делаем
        }

        public override void SetName(string name)
        {
            //ничего не делаем;
        }

        public override type_node base_type
        {
            get { return SystemLibrary.SystemLibrary.object_type; }
        }

        public override string name
        {
            get { return _name; }
        }

        public override string full_name
        {
            get { return _name; }
        }

        public override property_node default_property_node
        {
            get { return null; }
        }

        public override PascalABCCompiler.SemanticTree.node_kind node_kind
        {
            get { return SemanticTree.node_kind.indefinite; }
        }

        public override SymbolTable.Scope Scope
        {
            get { return null; }
        }

        public override bool is_class
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }

        public override bool is_value
        {
            get { return false; }
        }

        public override PascalABCCompiler.SemanticTree.type_special_kind type_special_kind
        {
            get
            {
                return PascalABCCompiler.SemanticTree.type_special_kind.none_kind;
            }
            set
            {

            }
        }

        public override semantic_node_type semantic_node_type
        {
            get { return semantic_node_type.indefinite_type; }
        }

        public override bool depended_from_indefinite
        {
            get
            {
                return true;
            }
        }     
    }


    //public class indefinite_expression_node : expression_node
    //{
    //    public indefinite_expression_node(location loc)
    //    {
    //        this.type = new indefinite_type_node();
    //        this.location = loc;
    //    }

    //    public indefinite_expression_node(type_node tn, location loc)
    //        : base(tn, loc)
    //    {
    //    }


    //    public override semantic_node_type semantic_node_type
    //    {
    //        get { return semantic_node_type.indefinite_expression_node; }
    //    }
    //}

    public class indefinite_function_call : base_function_call
    {
        function_node _function_node;

        public indefinite_function_call(function_node fn, location loc)
            : base(fn.return_value_type, loc)
        {
            _function_node = fn;
        }

        public override function_node simple_function_node
        {
            get { return _function_node; }
        }

        public override semantic_node_type semantic_node_type
        {
            get { return semantic_node_type.indefinite_function_call; }
        }
    }

    public class indefinite_reference : addressed_expression
    {
        private indefinite_definition_node _indefinite_def;

        public indefinite_definition_node indefinite_def
        {
            get { return _indefinite_def; }
        }

        public indefinite_reference(indefinite_definition_node idn, location loc)
            : base(idn.cont_class, loc)
        {
            _indefinite_def = idn;
        }

        public override semantic_node_type semantic_node_type
        {
            get { return semantic_node_type.indefinite_reference; }
        }
    }

}