// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
//Класс, хранящий текущий контекст. Где находится компилятор (в какой функции, типе, пространстве имен).
using System;
using System.Linq;
using PascalABCCompiler.TreeRealization;
using System.Collections.Generic;
using System.Collections;

namespace PascalABCCompiler.TreeConverter
{

	public enum block_type {function_block,type_block,namespace_block,compiled_type_block, lambda_block}; //lroman//

    public class ContextState
    {
        // Stack<global::SymbolTable.Scope> _scope_stack; не трогать - работать с ним аккуратно при переходе в другой Scope

        public common_namespace_node cmn; // текущий namespace
        public common_type_node ctn;      // текущий разбираемый тип
        public common_function_node_stack func_stack;
		public lambda_stack lambda_stack; 
        public Stack<code_block> block_stack;
        public Dictionary<int, short_string_type_node> ShortStringTypes;
        public List<var_definition_node> var_defs;
        public Stack<List<var_definition_node>> var_defs_stack;
        public List<local_variable> special_local_vars;
        public List<type_instance_and_location> possible_incorrect_instances;
        public bool skip_check_where_sections;
        public statement_node main_procedure;
        public bool allow_inherited_ctor_call;
        public bool is_order_independed_method_description;
        public template_class ctt;
        public compiled_type_node compiled_tn;
		public SymbolInfo last_created_function;
    	public statement_node_stack cycles_stack;
        public int num_of_for_cycles;
        public SemanticTree.field_access_level _fal;
        public System.Collections.Hashtable member_decls;
        public List<common_type_node> types_predefined;
        public Stack<common_type_node> type_stack; // Для вложенных типов
        public statement_list_stack stlist_stack;
        public semantic_node ret_value; // Возвращаемое значение класса returner
        public bool WithSection;
        public Dictionary<SymbolTable.Scope, expression_node> WithVariables;
        public Stack<SymbolTable.Scope> WithTypes;
        //LambdaHelper.Reset(); // Пока не знаю, что с этим делать

    }
	
	public class compilation_context
	{
        private ContextState SavedContext = null; // сохраненный контекст, используется при переходе к другому контексту при компиляции и последующем возврате к первоначальному контексту
        public Stack<ContextState> SavedContextStack = new Stack<ContextState>(); // SSM 26/08/15 - пробую сделать стек контекстов. Может, что-то надо удет клонировать?

        // Существуют 3 перехода вверх:
        // Из блока в раздел описаний до beginа этого блока (если это глобальный блок, то - на глобальный уровень). Обнуляется convertion_data_and_alghoritms.statement_list_stack. Поля func_stack и _ctn остаются нетронутыми
        // Из блока в раздел описаний вне всех подпрограмм (если нет объемлющего класса, то это - глобальный уровень). Обнуляется func_stack, _ctn остаётся нетронутым
        // Из любого блока на глобальный уровень. Обнуляется _ctn.
        public void SaveContextAndUpToGlobalLevel() 
        {
            if (!func_stack.Empty || SavedContext == null)
                SaveContextAndUpFromAllFunctionDefs();
            SavedContext.ctn = _ctn;
            _ctn = null;
        }

        public void SaveContextAndUpFromAllFunctionDefs()
        {
            if (!convertion_data_and_alghoritms.statement_list_stack.Empty || SavedContext == null)
                SaveContextAndUpToNearestDefSect();
            SavedContext.func_stack = func_stack;
            func_stack = new common_function_node_stack();
        }
        
        public void SaveContextAndUpToNearestDefSect() 
        {
            // _scope_stack вообще не трогать - он не используется !!!!!
            SavedContext = new ContextState();
            SavedContext.stlist_stack = convertion_data_and_alghoritms.statement_list_stack;
            SavedContext.cmn = _cmn;
            SavedContext.ctn = _ctn;
            SavedContext.func_stack = func_stack;
            SavedContext.block_stack = block_stack;
            SavedContext.ShortStringTypes = ShortStringTypes;
            SavedContext.var_defs = var_defs;
            SavedContext.var_defs_stack = var_defs_stack;
            SavedContext.special_local_vars = _special_local_vars;
            SavedContext.possible_incorrect_instances = possible_incorrect_instances;
            SavedContext.skip_check_where_sections = skip_check_where_sections;
            SavedContext.main_procedure = main_procedure;
            SavedContext.allow_inherited_ctor_call = allow_inherited_ctor_call;
            SavedContext.is_order_independed_method_description = is_order_independed_method_description;
            SavedContext.ctt = _ctt;
            SavedContext.compiled_tn = _compiled_tn;
            SavedContext.last_created_function = last_created_function;
            SavedContext.cycles_stack = _cycles_stack;
            //SavedContext.num_of_for_cycles = num_of_for_cycles; // num_of_for_cycles вычисляется и провоцирует ошибки!
            SavedContext._fal = _fal;
            SavedContext.member_decls = member_decls;
            SavedContext.types_predefined = _types_predefined;
            SavedContext.ret_value = syntax_tree_visitor.ret.get_result();
            SavedContext.WithTypes = WithTypes;
            SavedContext.WithSection = WithSection;
            SavedContext.WithVariables = WithVariables;

            SavedContextStack.Push(SavedContext);
            // SavedContext.type_stack = type_stack;

            convertion_data_and_alghoritms.statement_list_stack = new statement_list_stack();
            //_cmn = null;
            //_ctn = null;
            //func_stack = new common_function_node_stack();
            //block_stack = new Stack<code_block>();
            ShortStringTypes = new Dictionary<int,short_string_type_node>();
            //var_defs = new List<var_definition_node>();
            //var_defs_stack = new Stack<List<var_definition_node>>();
            _special_local_vars = new List<local_variable>();
            possible_incorrect_instances = new List<type_instance_and_location>();
            skip_check_where_sections = false;
            main_procedure = null;
            allow_inherited_ctor_call = false;
            is_order_independed_method_description = false;
            _ctt = null;
            _compiled_tn = null;
		    last_created_function = null;
    	    _cycles_stack = new statement_node_stack();
            _num_of_for_cycles = 0;
            _fal = SemanticTree.field_access_level.fal_private;
            member_decls = new Hashtable();
            _types_predefined = new List<common_type_node>();
            syntax_tree_visitor.ret.return_value(null);
            // _type_stack = new Stack<common_type_node>(); 

        }

        public void RestoreCurrentContext()
        {
            SavedContext = SavedContextStack.Pop();
            convertion_data_and_alghoritms.statement_list_stack = SavedContext.stlist_stack;
            _cmn = SavedContext.cmn;
            _ctn = SavedContext.ctn;
            func_stack = SavedContext.func_stack;
            block_stack = SavedContext.block_stack;
            ShortStringTypes = SavedContext.ShortStringTypes;
            var_defs = SavedContext.var_defs;
            var_defs_stack = SavedContext.var_defs_stack;
            _special_local_vars = SavedContext.special_local_vars;
            possible_incorrect_instances = SavedContext.possible_incorrect_instances;
            skip_check_where_sections = SavedContext.skip_check_where_sections;
            main_procedure = SavedContext.main_procedure;
            allow_inherited_ctor_call = SavedContext.allow_inherited_ctor_call;
            is_order_independed_method_description = SavedContext.is_order_independed_method_description;
            _ctt = SavedContext.ctt;
            _compiled_tn = SavedContext.compiled_tn;
            last_created_function = SavedContext.last_created_function;
            _cycles_stack = SavedContext.cycles_stack;
            //_num_of_for_cycles = SavedContext.num_of_for_cycles; // ssm 31/12/17 отключил! num_of_for_cycles вычисляется и провоцирует ошибки!
            _fal = SavedContext._fal;
            member_decls = SavedContext.member_decls;
            _types_predefined = SavedContext.types_predefined;
            WithTypes = SavedContext.WithTypes;
            WithVariables = SavedContext.WithVariables;
            WithSection = SavedContext.WithSection;
            syntax_tree_visitor.ret.return_value(SavedContext.ret_value);

            if (SavedContextStack.Count == 0)
                SavedContext = null;
            // type_stack = cs.type_stack; оно почему-то readonly - ну и ладно
        }

		public common_namespace_node _cmn;
        //TODO: Можно сделать возможность объявления вложенных типов.
		public common_type_node _ctn; // SSM - пытаюсь выходить из класса и входить заново
		private common_function_node_stack _func_stack=new common_function_node_stack();
        private type_node _explicit_interface_type;
        internal bool WithSection = false;
        internal Dictionary<SymbolTable.Scope, expression_node> WithVariables = new Dictionary<SymbolTable.Scope, expression_node>();
        internal Stack<SymbolTable.Scope> WithTypes = new Stack<SymbolTable.Scope>();

        internal common_function_node_stack func_stack
        {
            get
            {
                return _func_stack;
            }
            set
            {
                _func_stack = value;
            }
        }
       

        private Stack<code_block> _block_stack = new Stack<code_block>();

        internal Dictionary<type_node, common_type_node> TypedFiles = new Dictionary<type_node, common_type_node>();
        internal Dictionary<type_node, common_type_node> TypedSets = new Dictionary<type_node, common_type_node>();
        internal Dictionary<int, short_string_type_node> ShortStringTypes = new Dictionary<int, short_string_type_node>(); 

        internal List<var_definition_node> var_defs =
            new List<var_definition_node>();

        public List<type_node> typeclassInstances = new List<type_node>();

        internal Stack<System.Collections.Generic.List<var_definition_node>> var_defs_stack =
            new Stack<System.Collections.Generic.List<var_definition_node>>();

        internal List<local_variable> _special_local_vars =
            new List<local_variable>();

        internal Stack<SymbolTable.Scope> _scope_stack
            = new Stack<SymbolTable.Scope>();

        internal List<type_instance_and_location> possible_incorrect_instances = new List<type_instance_and_location>();
        internal bool skip_check_where_sections = false;

        private statement_node _main_procedure;
        internal bool extension_method = false;

        public bool allow_inherited_ctor_call = false;
		public bool is_order_independed_method_description = false;
        internal bool in_parameters_block = false;

        private template_class _ctt = null;

        private compiled_type_node _compiled_tn = null;

		private SymbolInfo _last_created_function;

		private statement_node_stack _cycles_stack=new statement_node_stack();

        public statement_node_stack CyclesStack
        {
            get
            {
                return _cycles_stack;
            }
            set
            {
                _cycles_stack = value;
            }
        }

        private int _num_for_delegates;
		private int _num_of_for_cycles;

        internal convertion_data_and_alghoritms convertion_data_and_alghoritms;

		private SemanticTree.field_access_level _fal;
        private bool _has_nested_functions;
        internal syntax_tree_visitor syntax_tree_visitor;
		
        private static compilation_context _instance;

        private Dictionary<common_namespace_node, Dictionary<string, template_class>> compiled_tc_cache = new Dictionary<common_namespace_node, Dictionary<string, template_class>>();
        internal System.Collections.Hashtable member_decls = new System.Collections.Hashtable();
        internal bool namespace_converted = false;

        public compilation_context(convertion_data_and_alghoritms convertion_data_and_alghoritms, syntax_tree_visitor syntax_tree_visitor)
        {
            this.convertion_data_and_alghoritms = convertion_data_and_alghoritms;
            this.syntax_tree_visitor = syntax_tree_visitor;
            _instance = this;
        }
		
        
        public static compilation_context instance
		{
			get
			{
				return _instance;
			}
		}

        internal void AddError(Errors.Error err)
        {
            syntax_tree_visitor.AddError(err);
        }

        internal void AddError(location loc, string ErrString, params string[] values)
        {
            syntax_tree_visitor.AddError(loc, ErrString, values);
        }
        
        internal bool can_call_inherited_ctor_call(statements_list lst)
        {
            if (lst.statements.Count == 0)
                return true;
            if (lst.statements[0] is basic_function_call)
            {
                base_function_call bfc = lst.statements[0] as basic_function_call;
                if (bfc.type != null && bfc.type.name.Contains("<>local_variables_class"))
                    return true;
            }
            return false;
        }
        
        public void reset()
        {
            _cmn = null;
            _ctn = null;
            _func_stack.clear();
            var_defs.Clear();
            _main_procedure = null;
            _last_created_function = null;
            _cycles_stack.clear();
            _num_of_for_cycles = 0;
            _fal = SemanticTree.field_access_level.fal_private;
            _num_for_delegates = 0;
            rec_num = 1;
            var_defs_stack.Clear();
            type_stack.Clear();
            clear_special_local_vars();
            _scope_stack.Clear();
            TypedFiles.Clear();
            ShortStringTypes.Clear();
            TypedSets.Clear();
            _compiled_tn = null;
            _explicit_interface_type = null;
            _ctt = null;
            allow_inherited_ctor_call = false;
            _types_predefined.Clear();
            _block_stack.Clear();
            member_decls.Clear();
            possible_incorrect_instances.Clear();
            skip_check_where_sections = false;
            LambdaHelper.Reset(); //lroman//
            SavedContext = null;
            SavedContextStack.Clear();
            compiled_tc_cache.Clear();
            extension_method = false;
            _last_created_function = null;
            in_parameters_block = false;
            is_order_independed_method_description = false;
            _has_nested_functions = false;
            typeclassInstances.Clear();
        }
        
        public void clear_type_prededinitions()
        {
            _types_predefined.Clear();
        }

        public bool inStaticArea()
        {
            if (converted_type != null && top_function != null)
            {
                common_method_node cmn = top_function as common_method_node;
                if (cmn == null)
                {
                    common_in_function_function_node fn = top_function as common_in_function_function_node;
                    while (fn != null)
                    {
                        cmn = fn.function as common_method_node;
                        if(cmn!=null) break;
                        fn = fn.function as common_in_function_function_node;
                    }
                }
                if (cmn != null)
                return cmn.IsStatic;
            }
            return false;
        }

        public SymbolTable.Scope CurrentScope
        {
            get
            {
                if (!syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack.Empty)
                {
                    SymbolTable.Scope sc = syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack.top().Scope;
                    if (sc != null)
                    {
                        if (top_function != null && LambdaHelper.IsLambdaName(top_function.name)) //lroman
                        {
                            var top_lambda_scope = top_function.scope.TopScope;

                            if (top_lambda_scope == sc)
                            {
                                return top_function.scope;
                            }
                        }
                        return sc;
                    }
                }
                if (top_function != null && LambdaHelper.IsLambdaName(top_function.name)) //lroman
                {
                    return top_function.scope;
                }
                switch (converting_block())
                {
                    case block_type.function_block:
                        {
                            return top_function.scope;
                        }
                    case block_type.namespace_block:
                        {
                            return converted_namespace.scope;
                        }
                    case block_type.type_block:
                        {
                            return converted_type.scope;
                        }
                    case block_type.compiled_type_block:
                        {
                            return converted_compiled_type.scope;
                        }
                    default:
                        {
                            throw new CompilerInternalError("Invalid converting block type");
                        }
                }
            }
        }
		
        public void push_function(common_function_node fn)
        {
        	_func_stack.push(fn);
        }
        
        public common_function_node get_method_to_realize(SyntaxTree.declaration dc)
        {
        	return member_decls[dc] as common_function_node;
        }

        public common_type_node get_type_to_realize(SyntaxTree.declaration dc)
        {
            return member_decls[dc] as common_type_node;
        }

        public void add_method_header(SyntaxTree.declaration dc, definition_node dn)
        {
        	member_decls.Add(dc,dn);
        }

        public void add_type_header(SyntaxTree.type_declaration td, common_type_node ctn)
        {
            member_decls.Add(td, ctn);
        }
        
        public void clear_member_bindings()
        {
            if (!namespace_converted)
        	    member_decls.Clear();
        }

        public void BeginSkipGenericInstanceChecking()
        {
            skip_check_where_sections = true;
        }

        public void EndSkipGenericInstanceChecking()
        {
            skip_check_where_sections = false;
            generic_convertions.check_instances_are_correct(possible_incorrect_instances);
            possible_incorrect_instances.Clear();
        }

        public void AddTemplate(string name, template_class tc, location loc)
        {
            //check_name_free(name, loc);
            _cmn.scope.AddSymbol(name, new SymbolInfo(tc));
            _cmn.templates.AddElement(tc);
        }
		
        public static common_type_node AddTypeToTypedFileList(common_type_node tctn)
        {
        	if (instance.TypedFiles.ContainsKey(tctn.element_type)) return instance.TypedFiles[tctn.element_type];
        	instance.TypedFiles.Add(tctn.element_type,tctn);
        	return tctn;
        }
        
        public static common_type_node AddTypeToSetTypeList(common_type_node tctn)
        {
        	if (instance.TypedSets.ContainsKey(tctn.element_type)) return instance.TypedSets[tctn.element_type];
        	instance.TypedSets.Add(tctn.element_type,tctn);
        	tctn.add_name(compiler_string_consts.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(tctn,PascalABCCompiler.SemanticTree.basic_function_type.objassign)));
        	tctn.scope.AddSymbol(compiler_string_consts.plus_name, SystemLibrary.SystemLibInitializer.SetUnionProcedure.SymbolInfo.FirstOrDefault());
            tctn.scope.AddSymbol(compiler_string_consts.mul_name, SystemLibrary.SystemLibInitializer.SetIntersectProcedure.SymbolInfo.FirstOrDefault());
            tctn.scope.AddSymbol(compiler_string_consts.in_name, SystemLibrary.SystemLibInitializer.InSetProcedure.SymbolInfo.FirstOrDefault());
            tctn.scope.AddSymbol(compiler_string_consts.minus_name, SystemLibrary.SystemLibInitializer.SetSubtractProcedure.SymbolInfo.FirstOrDefault());
            tctn.scope.AddSymbol(compiler_string_consts.eq_name, SystemLibrary.SystemLibInitializer.CompareSetEquals.SymbolInfo.FirstOrDefault());
            tctn.scope.AddSymbol(compiler_string_consts.noteq_name, SystemLibrary.SystemLibInitializer.CompareSetInEquals.SymbolInfo.FirstOrDefault());
            tctn.scope.AddSymbol(compiler_string_consts.sm_name, SystemLibrary.SystemLibInitializer.CompareSetLess.SymbolInfo.FirstOrDefault());
            tctn.scope.AddSymbol(compiler_string_consts.smeq_name, SystemLibrary.SystemLibInitializer.CompareSetLessEqual.SymbolInfo.FirstOrDefault());
            tctn.scope.AddSymbol(compiler_string_consts.gr_name, SystemLibrary.SystemLibInitializer.CompareSetGreater.SymbolInfo.FirstOrDefault());
            tctn.scope.AddSymbol(compiler_string_consts.greq_name, SystemLibrary.SystemLibInitializer.CompareSetGreaterEqual.SymbolInfo.FirstOrDefault());
            tctn.scope.AddSymbol(compiler_string_consts.plusassign_name,new SymbolInfo(make_set_plus_assign(tctn)));
            tctn.scope.AddSymbol(compiler_string_consts.minusassign_name,new SymbolInfo(make_set_minus_assign(tctn)));
            tctn.scope.AddSymbol(compiler_string_consts.multassign_name,new SymbolInfo(make_set_mult_assign(tctn)));
            return tctn;
        }
        
        public static void AddTypeToShortStringTypeList(type_node tn)
        {
        	instance.ShortStringTypes.Add((tn as short_string_type_node).Length,tn as short_string_type_node);
        	tn.add_name(compiler_string_consts.plus_name,new SymbolInfo(SystemLibrary.SystemLibrary.string_add));
        }
        
        public void pop_top_function()
        {
            _func_stack.pop();
        }

        private List<common_type_node> _types_predefined = new List<common_type_node>();
        public List<common_type_node> types_predefined
        {
            get
            {
                return _types_predefined;
            }
        }

        public Stack<code_block> block_stack
        {
            get { return _block_stack; }
            set { _block_stack = value; }
        }

        public void AddTemplateInstance(string name, type_node t)
        {
            _cmn.scope.AddSymbol(name, new SymbolInfo(t));
        }

        private void clear_special_local_vars()
        {
            _special_local_vars.Clear();
        }

        public void add_special_local_var(local_variable lv)
        {
            _special_local_vars.Add(lv);
        }

        public void apply_special_local_vars(common_namespace_function_node cnfn)
        {
            foreach (local_variable lv in _special_local_vars)
            {
                lv.function = cnfn;
                cnfn.var_definition_nodes_list.AddElement(lv);
            }
            clear_special_local_vars();
        }

        public void enter_scope(global::SymbolTable.Scope new_scope)
        {
            switch (converting_block())
            {
                case block_type.function_block:
                    {
                        _scope_stack.Push(top_function.scope);
                        top_function.scope = new_scope;
                        break;
                    }
                case block_type.namespace_block:
                    {
                        _scope_stack.Push(_cmn.scope);
                        _cmn.scope = new_scope;
                        break;
                    }
                case block_type.type_block:
                    {
                        throw new CompilerInternalError("Unsupported scope relations");
                    }
                default:
                    {
                        throw new CompilerInternalError("Undefined block type");
                    }
            }
        }

        /*private List<global::SymbolTable.Scope> with_stack = new List<global::SymbolTable.Scope>();
        private List<expression_node> with_expr_stack = new List<expression_node>();

        public void enter_scope(global::SymbolTable.Scope new_scope, expression_node en)
        {
            with_stack.Add(new_scope);
            with_expr_stack.Add(en);
        }*/

        public void leave_scope()
        {
            switch (converting_block())
            {
                case block_type.function_block:
                    {
                        top_function.scope = _scope_stack.Pop();
                        break;
                    }
                case block_type.namespace_block:
                    {
                        _cmn.scope = _scope_stack.Pop();
                        break;
                    }
                case block_type.type_block:
                    {
                        throw new CompilerInternalError("Unsupported scope relations");
                        break;
                    }
                default:
                    {
                        throw new CompilerInternalError("Undefined block type");
                    }
            }
        }

		public statement_node main_procedure
		{
			get
			{
				return _main_procedure;
			}
			set
			{
				_main_procedure=value;
			}
		}

		public void set_field_access_level(SemanticTree.field_access_level fal)
		{
			_fal=fal;
		}

        public SemanticTree.field_access_level get_field_access_level()
        {
            return _fal;
        }

		public common_function_node top_function
		{
			get
			{
				if (_func_stack.size==0)
				{
					return null;
				}
				return (_func_stack.top());
			}
		}

        public type_node converted_explicit_interface_type
        {
            get
            {
                return _explicit_interface_type;
            }
            //ssyy
            set
            {
                _explicit_interface_type = value;
            }
            //\ssyy
        }

        public common_type_node converted_type
		{
			get
			{
				return _ctn;
			}
            //ssyy
            set
            {
                _ctn = value;
            }
            //\ssyy
		}

        public bool has_nested_functions
        {
            get
            {
                return _has_nested_functions;
            }
            set
            {
                _has_nested_functions = value;
            }
        }

        //ssyy
        public common_function_node_stack converted_func_stack
        {
            get
            {
                return _func_stack;
            }
            set
            {
                _func_stack = value;
            }
        }

        public template_class converted_template_type
        {
            get
            {
                return _ctt;
            }
            set
            {
                _ctt = value;
            }
        }
        //\ssyy

        public compiled_type_node converted_compiled_type
        {
            get
            {
                return _compiled_tn;
            }
            set
            {
                _compiled_tn = value;
            }
        }

		public common_namespace_node converted_namespace
		{
			get
			{
				return _cmn;
			}
            set
            {
                _cmn = value;
            }
		}

		public common_namespace_node create_namespace(common_namespace_node comprehensive_namespace,string namespace_name,
            common_unit_node cont_unit,SymbolTable.Scope _scope,location loc)
		{
            SymbolTable.Scope scope;
            if (comprehensive_namespace == null)
            {
                //scope = convertion_data_and_alghoritms.symbol_table.CreateScope(cont_unit.scope);
                scope = _scope;
            }
            else
            {
                scope = convertion_data_and_alghoritms.symbol_table.CreateScope(comprehensive_namespace.scope, "namespace " + namespace_name);
            }
            _cmn = new common_namespace_node(comprehensive_namespace, cont_unit, namespace_name, scope, loc);
			cont_unit.namespaces.AddElement(_cmn);
			_cmn.scope.AddSymbol(namespace_name,new SymbolInfo(_cmn));
			return _cmn;
		}

        public common_namespace_node create_namespace(string namespace_name, common_unit_node cont_unit,SymbolTable.Scope _scope, location loc)
        {
            return create_namespace(null, namespace_name, cont_unit,_scope, loc);
        }

        public bool func_stack_size_is_one()
        {
            return (_func_stack.size == 1);
        }

		public block_type converting_block()
		{
            //if (_ctn != null && _ctn.is_value_type) return block_type.type_block;
            //if (with_stack.Count > 0) return block_type.with_block;
            if (_ctn != null && top_function!=null && _ctn.defined_in_scope == top_function.scope)
                return block_type.type_block;
			
            if (_func_stack.size!=0)
			{
				return block_type.function_block;
			}
			if (_ctn!=null)
			{
				return block_type.type_block;
			}
            if (_compiled_tn != null)
                return block_type.compiled_type_block;
			return block_type.namespace_block;
		}

        internal bool IsCompiledType(string type_name)
        {
            SymbolInfo si = find_first(type_name);
            if (si == null)
                return false;
            return si.sym_info is compiled_type_node;
        }

        public definition_node check_name_node_type(string name, location loc, params general_node_type[] expected_node_types) // SSM перебросил сюда из syntax_tree_visitor 3.04.14
        {
            SymbolInfo si = find_first(name);
            return check_name_node_type(name, si, loc, expected_node_types);
        }
        public definition_node check_name_node_type(string name, SymbolInfo si, location loc, params general_node_type[] expected_node_types) // SSM перебросил сюда из syntax_tree_visitor 3.04.14
        {         
            if (si == null)
            {
                AddError(new UndefinedNameReference(name, loc));
            }

            int i = 0;
            if (si.sym_info == null)
            {
                if (name.ToLower() == "result")
                    AddError(loc, "CAN_NOT_DEDUCE_TYPE_{0}", "Result");
                else
                    AddError(new ExpectedType(loc));
            }
                
            if (si.sym_info.semantic_node_type == semantic_node_type.wrap_def)
            {
                BasePCUReader.RestoreSymbol(si, name);
            }
            while (i < expected_node_types.Length)
            {
                if (si.sym_info.general_node_type == expected_node_types[i])
                {
                    break;
                }
                i++;
            }
            if (i >= expected_node_types.Length)
            {
                if (expected_node_types.Length == 1)
                    if (expected_node_types[0] == general_node_type.type_node)
                        AddError(new ExpectedType(loc));
                AddError(new ExpectedAnotherKindOfObject(expected_node_types, si.sym_info.general_node_type,
                    convertion_data_and_alghoritms.get_location(si.sym_info), loc));
            }

            //if (SystemUnitAssigned && SystemLibrary.SystemLibInitializer.ShortStringType.Found && SystemLibrary.SystemLibInitializer.ShortStringType.Equal(si))
            //    return context.create_short_string_type(SemanticRules.ShortStringDefaultLength, loc);

            return si.sym_info;
        }

        //ssyy
        //Определение предпоследнего имени - как имени модуля
        private definition_node convert_names_to_namespace(SyntaxTree.named_type_reference names, location loc)  // SSM перебросил сюда из syntax_tree_visitor 3.04.14
        {
            //предполагается, что количество имён больше чем 1.
            definition_node di = null;
            location loc1 = syntax_tree_visitor.get_location(names.names[0]);
            if (loc1 == null)
                loc1 = loc;
            di = check_name_node_type(names.names[0].name, loc1, general_node_type.namespace_node, general_node_type.unit_node, general_node_type.type_node);
            for (int i = 1; i < names.names.Count - 1; i++)
            {
                List<SymbolInfo> sil = null;
                if (di is namespace_node)
                {
                    sil = (di as namespace_node).find(names.names[i].name);
                }
                else
                    sil = (di as type_node).find_in_type(names.names[i].name);
                loc1 = syntax_tree_visitor.get_location(names.names[i]);
                if (loc1 == null)
                    loc1 = loc;
                di = check_name_node_type(names.names[i].name, sil?.FirstOrDefault(), loc1, general_node_type.namespace_node, general_node_type.type_node);
            }
            return di;
        }

        //Нахождение узла по имени либо точечному имени (напр. Unit1.type1)
        public List<SymbolInfo> find_definition_node(SyntaxTree.named_type_reference names, location loc)  // SSM перебросил сюда из syntax_tree_visitor 3.04.14
        {
            definition_node di = null;
            if (names.names.Count > 1)
            {
                di = convert_names_to_namespace(names, loc);
            }
            if (di != null)
            {
                int num = names.names.Count - 1;
                if (di is namespace_node)
                {
                    return (di as namespace_node).find(names.names[num].name);
                }
                else if (di is type_node)
                {
                    return (di as type_node).find_in_type(names.names[num].name);
                }
                else
                {
                    return (di as unit_node).find_only_in_namespace(names.names[num].name);
                }
            }
            return find(names.names[0].name);
        }
        //\ssyy

        //(ssyy) возвращаемое значение заменено на definition_node из-за шаблонов
        public definition_node enter_in_type_method(SyntaxTree.method_name meth_name, string type_name, location loc, int num_template_args)
        {
            // num_template_args - это количество обобщенных аргументов в классе (не в методе!)
            List<SymbolInfo> sil = null;

            if (meth_name.ln!=null && meth_name.ln.Count > 1)
            {
                // обработать эту ситуацию особо тщательно: в ln - список возможных пространств имен с классом (возможно, обобщенным) на конце

                // если в ln какое-то имя кроме последнего, содержит обобщенные параметры - это ошибка
                for (var i = 0; i < meth_name.ln.Count - 1; i++)
                    if (meth_name.ln[i] is SyntaxTree.template_type_name)
                        AddError(new NameCannotHaveGenericParameters(meth_name.ln[i].name, syntax_tree_visitor.get_location(meth_name.ln[i]))); 
                
                var ntr = new SyntaxTree.named_type_reference();
                for (var i = 0; i < meth_name.ln.Count; i++)
                    ntr.Add(meth_name.ln[i]);
                if (num_template_args>0 && ! ntr.names[meth_name.ln.Count-1].name.Contains(compiler_string_consts.generic_params_infix))
                {
                     ntr.names[meth_name.ln.Count-1].name += compiler_string_consts.generic_params_infix + num_template_args;
                }
                sil = find_definition_node(ntr, loc);
                // если не нашли, то ошибка будет неправильной с неправильным именем - надо исправить
            }
            else
            {
                if (num_template_args != 0)
                {
                    string template_type_name = type_name + compiler_string_consts.generic_params_infix + num_template_args.ToString();
                    sil = find(template_type_name);
                    /*if (si == null || si.sym_info.general_node_type != general_node_type.template_type)
                    {
                        type_name = type_name + compiler_string_consts.generic_params_infix + num_template_args.ToString();
                        si = null;
                    }*/
                }
                if (sil == null)
                {
                    sil = find(type_name);
                }
            }

            if (sil == null)
            {
                AddError(new UndefinedNameReference(type_name, loc));
            }
            definition_node dn = sil.FirstOrDefault().sym_info;
            if (dn.general_node_type == general_node_type.template_type)
            {
                _ctt = dn as template_class;
                if (_cmn != _ctt.cnn && _ctt.using_list2 == null)
                {
                    //Заполняем список using для внешних методов
                    _ctt.using_list2 = new using_namespace_list();
                    foreach(using_namespace un in syntax_tree_visitor.using_list)
                    {
                        _ctt.using_list2.AddElement(un);
                    }
                }
                return dn;
            }
            if (dn.general_node_type == general_node_type.generic_indicator)
            {
                dn = (dn as generic_indicator).generic;
            }
            if (dn is compiled_type_node)
            {
                _compiled_tn = dn as compiled_type_node;
                return dn;
            }
            if (dn.general_node_type != general_node_type.type_node)
            {
                AddError(loc, "TYPE_NAME_EXPECTED");
            }
#if DEBUG
            if (sil.Count() != 1)
            {
                throw new CompilerInternalError("Must find only type, found some other.");
            }
#endif
            if (dn.semantic_node_type != semantic_node_type.common_type_node)
            {
                AddError(loc, "ONLY_COMMON_TYPE_METHOD_DEFINITION_ALLOWED");
            }

            //TODO: В случае создания вложенных классов этот код надо поправить.
            common_type_node ctn = dn as common_type_node;
            _ctn = ctn;
            return ctn;
        }

        public void leave_type_method()
        {
            if (_explicit_interface_type != null)
            {
                function_node fn = top_function;
                List<SymbolInfo> sil = _ctn.Scope.FindOnlyInType(fn.name, null);
                function_node compar;
                if(sil != null)
                    foreach(var si in sil)
                    {
                        compar = si.sym_info as function_node;
                        if (fn != compar && convertion_data_and_alghoritms.function_eq_params(fn, compar))
                            //if (fn is common_namespace_function_node && compar is common_namespace_function_node && (fn as common_namespace_function_node).comprehensive_namespace == (compar as common_namespace_function_node).comprehensive_namespace)

                            AddError(new FunctionDuplicateDefinition(compar, fn));
                    }
            }
            else if (_compiled_tn != null)
            {
                function_node fn = top_function;
                List<SymbolInfo> sil = _compiled_tn.scope.FindOnlyInType(fn.name, null);
                function_node compar;
                if(sil != null)
                    foreach(var si in sil)
                    {
                        compar = si.sym_info as function_node;
                        if (fn!=compar && convertion_data_and_alghoritms.function_eq_params(fn, compar,false))
                            //if (fn is common_namespace_function_node && compar is common_namespace_function_node && (fn as common_namespace_function_node).comprehensive_namespace == (compar as common_namespace_function_node).comprehensive_namespace)

                            AddError(new FunctionDuplicateDefinition(compar, fn));
                    }                
            }
            if (_explicit_interface_type == null)
                _ctn = null;
            _compiled_tn = null;
            _explicit_interface_type = null;
        }

        private readonly System.Collections.Generic.Stack<common_type_node> type_stack = new System.Collections.Generic.Stack<common_type_node>();
        
        private void add_notequal_operator_if_need()
        {
        	SymbolInfo si = _ctn.find_first_in_type(compiler_string_consts.noteq_name);
        	if (si.sym_info is common_method_node)
        		return;
        	SymbolTable.ClassMethodScope scope = convertion_data_and_alghoritms.symbol_table.CreateClassMethodScope(_cmn.scope,_ctn.scope, si.ToString());
        	common_method_node cmn = new common_method_node(compiler_string_consts.GetNETOperName(compiler_string_consts.noteq_name),SystemLibrary.SystemLibrary.bool_type,null,_ctn,
        	                                                SemanticTree.polymorphic_state.ps_static,SemanticTree.field_access_level.fal_public,scope);
        	cmn.IsOperator = true;
        	common_parameter prm1 = new common_parameter("a",_ctn,SemanticTree.parameter_type.value,cmn,concrete_parameter_type.cpt_none,null,null);
        	common_parameter prm2 = new common_parameter("b",_ctn,SemanticTree.parameter_type.value,cmn,concrete_parameter_type.cpt_none,null,null);
        	cmn.parameters.AddElement(prm1);
        	cmn.parameters.AddElement(prm2);
        	statements_list body = new statements_list(null);
        	foreach (class_field cf in _ctn.fields)
        	{
                if (cf.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                    continue;
                expression_node left = new class_field_reference(cf,new common_parameter_reference(prm1,0,null),null);
        		expression_node right = new class_field_reference(cf,new common_parameter_reference(prm2,0,null),null);
        		expression_node cond = SystemLibrary.SystemLibrary.syn_visitor.find_operator(compiler_string_consts.noteq_name,
        		                                                                            left,right,null);
        		//basic_function_call bfc = new basic_function_call(SystemLibrary.SystemLibrary.bool_not as basic_function_node,null);
        		//bfc.parameters.AddElement(cond);
        		if_node if_stmt = new if_node(cond,new return_node(new bool_const_node(true,null),null),null,null);
        		body.statements.AddElement(if_stmt);
        	}
        	body.statements.AddElement(new return_node(new bool_const_node(false,null),null));
        	cmn.function_code = body;
        	cmn.is_overload = true;
        	_ctn.methods.AddElement(cmn);
        	_ctn.Scope.AddSymbol(compiler_string_consts.noteq_name,new SymbolInfo(cmn));
        }
        
        private void add_equal_operator_if_need()
        {
        	SymbolInfo si = _ctn.find_first_in_type(compiler_string_consts.eq_name);
        	if (si.sym_info is common_method_node)
        		return;
        	SymbolTable.ClassMethodScope scope = convertion_data_and_alghoritms.symbol_table.CreateClassMethodScope(_cmn.scope,_ctn.scope, si.ToString());
        	common_method_node cmn = new common_method_node(compiler_string_consts.GetNETOperName(compiler_string_consts.eq_name),SystemLibrary.SystemLibrary.bool_type,null,_ctn,
        	                                                SemanticTree.polymorphic_state.ps_static,SemanticTree.field_access_level.fal_public,scope);
        	cmn.IsOperator = true;
        	common_parameter prm1 = new common_parameter("a",_ctn,SemanticTree.parameter_type.value,cmn,concrete_parameter_type.cpt_none,null,null);
        	common_parameter prm2 = new common_parameter("b",_ctn,SemanticTree.parameter_type.value,cmn,concrete_parameter_type.cpt_none,null,null);
        	cmn.parameters.AddElement(prm1);
        	cmn.parameters.AddElement(prm2);
        	statements_list body = new statements_list(null);
        	foreach (class_field cf in _ctn.fields)
        	{
                if (cf.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                    continue;
        		expression_node left = new class_field_reference(cf,new common_parameter_reference(prm1,0,null),null);
        		expression_node right = new class_field_reference(cf,new common_parameter_reference(prm2,0,null),null);
        		expression_node cond = SystemLibrary.SystemLibrary.syn_visitor.find_operator(compiler_string_consts.eq_name,
        		                                                                            left,right,null);
        		basic_function_call bfc = new basic_function_call(SystemLibrary.SystemLibrary.bool_not as basic_function_node,null);
        		bfc.parameters.AddElement(cond);
        		if_node if_stmt = new if_node(bfc,new return_node(new bool_const_node(false,null),null),null,null);
        		body.statements.AddElement(if_stmt);
        	}
        	body.statements.AddElement(new return_node(new bool_const_node(true,null),null));
        	cmn.function_code = body;
        	cmn.is_overload = true;
        	_ctn.methods.AddElement(cmn);
        	_ctn.Scope.AddSymbol(compiler_string_consts.eq_name,new SymbolInfo(cmn));
        }
        
        public void leave_record()
        {
            check_implement_interfaces();
            if (!_ctn.IsEnum)
            {
            	add_equal_operator_if_need();
            	add_notequal_operator_if_need();
            }
            if (type_stack.Count != 0)
            {
                _cmn.types.AddElement(_ctn);
                _ctn = type_stack.Pop();
            }
            else
            {
                _cmn.types.AddElement(_ctn);
                _ctn = null;
            }
        }
       
        public SymbolInfo create_special_names()
        {
            SymbolInfo si= new SymbolInfo(top_function.return_variable);
            top_function.scope.AddSymbol(compiler_string_consts.result_variable_name, si);
            return si;
        }
        
		public void create_lambda() //lroman//
        {
            SymbolTable.Scope scope = convertion_data_and_alghoritms.symbol_table.CreateLambdaScope(CurrentScope);
            lambda_node new_lambda = new lambda_node(scope);
            LambdaHelper.CurrentLambdaScopeNum = new_lambda.scope.ScopeNum;
        }

        public void remove_lambda_function(string lambda_name, bool remove_from_context) //lroman//
        { 
            //На случай, если произошла ошибка при компиляции, то надо удалять из стека функций, так как не докомпилировали до конца, и ошибочная версия все еще в стеке
            if (remove_from_context)
            {
                if (converting_block() == block_type.function_block)
                {
                    var top_func = func_stack.top();
                    if (top_func.name == lambda_name)
                    {
                        _func_stack.pop();
                    }
                }
            }

            if (LambdaHelper.StatementListStackStack.Count == 0)
            {
                return;
            }

            var top_statement_list_stack = LambdaHelper.StatementListStackStack.Peek();

            if (top_statement_list_stack.Key != lambda_name)
            {
                return;
            }
            
            syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack =
                    LambdaHelper.StatementListStackStack.Pop().Value;
        }

        public void remove_lambda() //lroman//
        {
			LambdaHelper.CurrentLambdaScopeNum = -1;
        }

        //lroman//
        public common_function_node create_lambda_function(string name, location def_loc, bool add_symbol_info, SymbolTable.Scope topScope)
        {
            var func_stack_as_array = _func_stack.CloneInternalStack().ToList();

            var nestedFunc = func_stack_as_array.FirstOrDefault(func => func is common_in_function_function_node);
            if (nestedFunc != null)
            {
                syntax_tree_visitor.AddError(new LambdasAreNotAllowedInNestedSubprogram(def_loc));
            }

            LambdaHelper.StatementListStackStack.Push(new KeyValuePair<string, statement_list_stack>(name, syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack));
            syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack = new statement_list_stack();

            check_function_name(name, def_loc);
            common_function_node cfn = null;

            if (_ctn != null)
            {
                common_method_node cmmn;
                SymbolTable.Scope scope = convertion_data_and_alghoritms.symbol_table.CreateClassMethodScope(topScope, _ctn.Scope, "lambda " + name);
                //TODO:сделать static и virtual.
                //TODO: interface and implementation scopes.
                cmmn = new common_method_node(name, def_loc, _ctn, SemanticTree.polymorphic_state.ps_common, _fal, scope);

                _last_created_function = new SymbolInfo(cmmn);
                if (add_symbol_info)
                    _ctn.Scope.AddSymbol(name, _last_created_function);
                _ctn.methods.AddElement(cmmn);

                cfn = cmmn;
            }
            else
            {
                common_namespace_function_node cnfnn;
                SymbolTable.Scope scope = convertion_data_and_alghoritms.symbol_table.CreateScope(topScope, "lambda " + name);
                cnfnn = new common_namespace_function_node(name, def_loc, _cmn, scope);
                _cmn.functions.AddElement(cnfnn);
                _last_created_function = new SymbolInfo(cnfnn);
                if (add_symbol_info)
                    _cmn.scope.AddSymbol(name, _last_created_function);
                cfn = cnfnn;
            }

            _func_stack.push(cfn);
            return cfn;
        }
        //\lroman//
		
        public common_function_node create_function(string name, location def_loc)
        {
            return create_function(name, def_loc, true);
        }
		public common_function_node create_function(string name,location def_loc, bool add_symbol_info)
		{
			if (converting_block() == block_type.compiled_type_block)
			{
				_compiled_tn = compiled_type_node.get_type_node(_compiled_tn.compiled_type,this.syntax_tree_visitor.SymbolTable);
			}
			check_function_name(name, def_loc);
            common_function_node cfn = null;
			switch (converting_block())
			{
				case block_type.function_block:
				{
                    common_function_node top_func = _func_stack.top();
                    SymbolTable.Scope scope = convertion_data_and_alghoritms.symbol_table.CreateScope(top_func.scope, "function " + name);
					common_in_function_function_node ciffn =new common_in_function_function_node(name,def_loc,top_func,scope);
					top_func.functions_nodes_list.AddElement(ciffn);
                    _last_created_function = new SymbolInfo(ciffn);
                    if (add_symbol_info)
                        top_func.scope.AddSymbol(name, _last_created_function);
                    cfn=ciffn;
					break;
				}
				case block_type.type_block:
				{
                    if (!extension_method)
                    {
                        common_method_node cmmn;
                        SymbolTable.Scope scope = convertion_data_and_alghoritms.symbol_table.CreateClassMethodScope(_cmn.scope, _ctn.Scope, name=="create"? "constructor " + _ctn.Scope : "method " + name + " from " + _ctn.Scope);
                        //TODO:сделать static и virtual.
                        //TODO: interface and implementation scopes.
                        cmmn = new common_method_node(name, def_loc, _ctn, SemanticTree.polymorphic_state.ps_common, _fal, scope);

                        _last_created_function = new SymbolInfo(cmmn);
                        if (add_symbol_info)
                            _ctn.Scope.AddSymbol(name, _last_created_function);
                        _ctn.methods.AddElement(cmmn);

                        cfn = cmmn;
                    }
                    else
                    {
                        common_namespace_function_node cnfnn;
                        SymbolTable.Scope scope = convertion_data_and_alghoritms.symbol_table.CreateClassMethodScope(_cmn.scope, _ctn.scope, name);
                        cnfnn = new common_namespace_function_node(name, def_loc, _cmn, scope);
                        //_cmn.functions.AddElement(cnfnn);
                        syntax_tree_visitor.compiled_unit.namespaces[0].functions.AddElement(cnfnn);
                        cnfnn.ConnectedToType = _ctn;
                        _last_created_function = new SymbolInfo(cnfnn);
                        if (add_symbol_info)
                            _ctn.scope.AddSymbol(name, _last_created_function);
                        cfn = cnfnn;
                    }
					break;
				}
                case block_type.compiled_type_block:
                {
                    //string cname = compiler_string_consts.GetConnectedFunctionName(_compiled_tn.name, name);
                    common_namespace_function_node cnfnn;
                    SymbolTable.Scope scope = convertion_data_and_alghoritms.symbol_table.CreateClassMethodScope(_cmn.scope, _compiled_tn.scope, name);
                    cnfnn = new common_namespace_function_node(name, def_loc, _cmn, scope);
                    //_cmn.functions.AddElement(cnfnn);
                    syntax_tree_visitor.compiled_unit.namespaces[0].functions.AddElement(cnfnn);
                    cnfnn.ConnectedToType = _compiled_tn;
                    _last_created_function = new SymbolInfo(cnfnn);
                    if (add_symbol_info)
                        _compiled_tn.scope.AddSymbol(name, _last_created_function);
                    List<SymbolInfo> sss = _compiled_tn.find_in_type("Hello");
                    cfn = cnfnn;
                    //if(_compiled_tn.compiled_type.IsPrimitive)
                    //    syntax_tree_visitor.WarningsList.Add(new PascalABCCompiler.Errors.CompilerWarning
                    break;
                }
				case block_type.namespace_block:
				{
					common_namespace_function_node cnfnn;
                    SymbolTable.Scope scope = convertion_data_and_alghoritms.symbol_table.CreateScope(_cmn.scope, name);
					cnfnn=new common_namespace_function_node(name,def_loc,_cmn,scope);
					_cmn.functions.AddElement(cnfnn);
					_last_created_function= new SymbolInfo(cnfnn);
                    if (add_symbol_info)                    
					    _cmn.scope.AddSymbol(name, _last_created_function);
					cfn=cnfnn;
					break;
				}
			}
			_func_stack.push(cfn);
			return cfn;
		}

		public common_type_node advanced_create_type(string name,location def_loc, bool type_is_interface, bool is_partial=false)
        {
			//check_name_free(name,def_loc);
            common_type_node partial_class = null;
            common_type_node rez = check_type_name_free_and_predop(name, def_loc, ref partial_class, is_partial);
            if (rez != null)
            {
                _ctn = rez;
                _types_predefined.Remove(_ctn);
                return rez;
            }
            if (converting_block() != block_type.namespace_block)
            {
                AddError(new ClassCanNotBeDefinedInTypeOrFunction(name, def_loc));
            }
            SymbolTable.ClassScope scope;
            //(ssyy) Если создаётся интерфейс, то создаём ему специальный вид области видимости
            if (type_is_interface)
            {
                scope = convertion_data_and_alghoritms.symbol_table.CreateInterfaceScope(_cmn.scope, null, "interface " + name);
            }
            else
            {
                scope = convertion_data_and_alghoritms.symbol_table.CreateClassScope(_cmn.scope, null, "class " + name);
            }
			common_type_node tctn=new common_type_node(name,SemanticTree.type_access_level.tal_public,_cmn,
                scope,def_loc);
            tctn.IsPartial = is_partial;
            if (partial_class != null)
            {
                tctn.scope.PartialScope = partial_class.scope;
                tctn.Merge(partial_class);
            }
			set_field_access_level(SemanticTree.field_access_level.fal_public);
			_cmn.scope.AddSymbol(name,new SymbolInfo(tctn));
            tctn.IsInterface = type_is_interface;

			//_cmn.types.AddElement(tctn);
			_ctn=tctn;
			SystemLibrary.SystemLibrary.init_reference_type(tctn);
			return tctn;
		}

        public common_type_node CreateTempCommonType(string nameTemplate, location def_loc)
        {
            int n=0;
            while (_cmn.scope.Find(string.Format(nameTemplate, n)) != null)
                n++;
            string name = string.Format(nameTemplate, n);
            SymbolTable.ClassScope scope = convertion_data_and_alghoritms.symbol_table.CreateClassScope(_cmn.scope, null, name);
            common_type_node tctn = new common_type_node(name, SemanticTree.type_access_level.tal_internal, _cmn,
                scope, def_loc);
            _cmn.scope.AddSymbol(name, new SymbolInfo(tctn));
            tctn.IsInterface = false;
            _cmn.types.AddElement(tctn);
            _ctn = tctn;
            SystemLibrary.SystemLibrary.init_reference_type(tctn);
            return tctn;
        }
		
		public Stack<var_definition_node> loop_var_stack = new Stack<var_definition_node>();
		
		public bool is_in_cycle()
		{
			return loop_var_stack.Count != 0;
		}
		
		public bool is_loop_variable(var_definition_node vdn)
		{
			return loop_var_stack.Contains(vdn);
		}
		
		
        public common_type_node create_type(string name, location def_loc)
        {
            return advanced_create_type(name, def_loc, false);
        }
        
        public common_type_node create_typed_file_type(type_node elem_type, location def_loc)
        {
            if (TypedFiles.ContainsKey(elem_type))
                return TypedFiles[elem_type];
            string name = compiler_string_consts.GetTypedFileTypeName(elem_type.name);
            type_node base_type = SystemLibrary.SystemLibInitializer.TypedFileType.sym_info as type_node;
            //check_name_free(name, def_loc);
            SymbolTable.ClassScope scope = convertion_data_and_alghoritms.symbol_table.CreateClassScope(_cmn.scope, null, name);
            common_type_node tctn = new common_type_node(name, SemanticTree.type_access_level.tal_public, _cmn,
                scope, def_loc);
            set_field_access_level(SemanticTree.field_access_level.fal_public);
            _cmn.scope.AddSymbol(name, new SymbolInfo(tctn));
            tctn.type_special_kind = SemanticTree.type_special_kind.typed_file;
            tctn.element_type = elem_type;
            tctn.internal_is_value = base_type.is_value;
            tctn.is_class = base_type.is_class;
            tctn.SetBaseType(base_type);
            //_ctn = tctn;
            SystemLibrary.SystemLibrary.init_reference_type(tctn);
            converted_namespace.types.AddElement(tctn);
            TypedFiles.Add(elem_type, tctn);
            return tctn;
        }

        public common_type_node create_set_type(type_node elem_type, location def_loc)
        {
        	if (TypedSets.ContainsKey(elem_type))
                return TypedSets[elem_type];
            string name = compiler_string_consts.GetSetTypeName(elem_type.name);
            type_node base_type = SystemLibrary.SystemLibInitializer.TypedSetType.sym_info as type_node;
            //check_name_free(name, def_loc);
            SymbolTable.ClassScope scope = convertion_data_and_alghoritms.symbol_table.CreateClassScope(_cmn.scope, null, "set_type " + name);
            common_type_node tctn = new common_type_node(name, SemanticTree.type_access_level.tal_public, _cmn,
                scope, def_loc);
            set_field_access_level(SemanticTree.field_access_level.fal_public);
            _cmn.scope.AddSymbol(name, new SymbolInfo(tctn));
            tctn.type_special_kind = SemanticTree.type_special_kind.set_type;
            tctn.element_type = elem_type;
            tctn.internal_is_value = base_type.is_value;
            tctn.is_class = base_type.is_class;
            tctn.SetBaseType(base_type);
            tctn.add_name(compiler_string_consts.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(tctn,PascalABCCompiler.SemanticTree.basic_function_type.objassign)));
            tctn.ImplementingInterfaces.Add(compiled_type_node.get_type_node(NetHelper.NetHelper.FindType(compiler_string_consts.IEnumerableInterfaceName)));
            tctn.scope.AddSymbol(compiler_string_consts.plus_name, SystemLibrary.SystemLibInitializer.SetUnionProcedure.SymbolInfo.FirstOrDefault());
            tctn.scope.AddSymbol(compiler_string_consts.mul_name, SystemLibrary.SystemLibInitializer.SetIntersectProcedure.SymbolInfo.FirstOrDefault());
            tctn.scope.AddSymbol(compiler_string_consts.in_name, SystemLibrary.SystemLibInitializer.InSetProcedure.SymbolInfo.FirstOrDefault());
            tctn.scope.AddSymbol(compiler_string_consts.minus_name, SystemLibrary.SystemLibInitializer.SetSubtractProcedure.SymbolInfo.FirstOrDefault());
            tctn.scope.AddSymbol(compiler_string_consts.eq_name, SystemLibrary.SystemLibInitializer.CompareSetEquals.SymbolInfo.FirstOrDefault());
            tctn.scope.AddSymbol(compiler_string_consts.noteq_name, SystemLibrary.SystemLibInitializer.CompareSetInEquals.SymbolInfo.FirstOrDefault());
            tctn.scope.AddSymbol(compiler_string_consts.sm_name, SystemLibrary.SystemLibInitializer.CompareSetLess.SymbolInfo.FirstOrDefault());
            tctn.scope.AddSymbol(compiler_string_consts.smeq_name, SystemLibrary.SystemLibInitializer.CompareSetLessEqual.SymbolInfo.FirstOrDefault());
            tctn.scope.AddSymbol(compiler_string_consts.gr_name, SystemLibrary.SystemLibInitializer.CompareSetGreater.SymbolInfo.FirstOrDefault());
            tctn.scope.AddSymbol(compiler_string_consts.greq_name, SystemLibrary.SystemLibInitializer.CompareSetGreaterEqual.SymbolInfo.FirstOrDefault());
            tctn.scope.AddSymbol(compiler_string_consts.plusassign_name,new SymbolInfo(make_set_plus_assign(tctn)));
            tctn.scope.AddSymbol(compiler_string_consts.minusassign_name,new SymbolInfo(make_set_minus_assign(tctn)));
            tctn.scope.AddSymbol(compiler_string_consts.multassign_name,new SymbolInfo(make_set_mult_assign(tctn)));
            converted_namespace.types.AddElement(tctn);
            TypedSets.Add(elem_type, tctn);
            return tctn;            
        }
		
        private static basic_function_node make_set_plus_assign(common_type_node ctn)
        {
        	//basic_function_node bfn = new basic_function_node(SemanticTree.basic_function_type.objassign,ctn,false);
        	basic_function_node bfn = SystemLibrary.SystemLibrary.make_binary_operator(compiler_string_consts.plusassign_name,ctn,SemanticTree.basic_function_type.objassign);
        	bfn.compile_time_executor = SystemLibrary.static_executors.set_plusassign_executor;
        	return bfn;
        }
        
        private static basic_function_node make_set_minus_assign(common_type_node ctn)
        {
        	//basic_function_node bfn = new basic_function_node(SemanticTree.basic_function_type.objassign,ctn,false);
        	basic_function_node bfn = SystemLibrary.SystemLibrary.make_binary_operator(compiler_string_consts.minusassign_name,ctn,SemanticTree.basic_function_type.objassign);
        	bfn.compile_time_executor = SystemLibrary.static_executors.set_subassign_executor;
        	return bfn;
        }
        
        private static basic_function_node make_set_mult_assign(common_type_node ctn)
        {
        	//basic_function_node bfn = new basic_function_node(SemanticTree.basic_function_type.objassign,ctn,false);
        	basic_function_node bfn = SystemLibrary.SystemLibrary.make_binary_operator(compiler_string_consts.multassign_name,ctn,SemanticTree.basic_function_type.objassign);
        	bfn.compile_time_executor = SystemLibrary.static_executors.set_multassign_executor;
        	return bfn;
        }
        
        private static basic_function_node make_short_string_plus_assign(short_string_type_node ctn)
        {
        	basic_function_node bfn = SystemLibrary.SystemLibrary.make_binary_operator(compiler_string_consts.plusassign_name,ctn,SemanticTree.basic_function_type.objassign);
        	bfn.compile_time_executor = SystemLibrary.static_executors.short_string_addassign_executor;
        	return bfn;
        }

        public template_class create_template_class(string name, byte[] tree)
        {
            Dictionary<string, template_class> tc_dict = null;
            template_class tc = null;
            if (compiled_tc_cache.TryGetValue(converted_namespace, out tc_dict) && tc_dict.TryGetValue(name, out tc))
                return tc;

            SyntaxTree.SyntaxTreeStreamReader str = new SyntaxTree.SyntaxTreeStreamReader();
            str.br = new System.IO.BinaryReader(new System.IO.MemoryStream(tree));
            SyntaxTree.type_declaration t_d = str._read_node() as SyntaxTree.type_declaration;
            tc = new template_class(t_d, name, converted_namespace, null, new using_namespace_list());
            tc.is_synonym = true;
            if (tc_dict == null)
            {
                tc_dict = new Dictionary<string, template_class>();
                compiled_tc_cache[converted_namespace] = tc_dict;
            }
            tc_dict[name] = tc;
            return tc;
        }
        
        public short_string_type_node create_short_string_type(int length, location def_loc)
        {
            if (ShortStringTypes.ContainsKey(length))
                return ShortStringTypes[length];
            SymbolTable.ClassScope scope = convertion_data_and_alghoritms.symbol_table.CreateClassScope(null, SystemLibrary.SystemLibrary.string_type.Scope, "short_string_type");
            short_string_type_node tctn = new short_string_type_node(//SemanticTree.type_access_level.tal_public, _cmn,
                scope, def_loc, length);
            tctn.add_name(compiler_string_consts.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(tctn,PascalABCCompiler.SemanticTree.basic_function_type.objassign)));
            tctn.add_name(compiler_string_consts.plus_name,new SymbolInfo(SystemLibrary.SystemLibrary.string_add));
            tctn.scope.AddSymbol(compiler_string_consts.plusassign_name,new SymbolInfo(make_short_string_plus_assign(tctn)));
            type_intersection_node inter = new type_intersection_node(type_compare.greater_type);
            inter.another_to_this = new type_conversion(SystemLibrary.SystemLibrary.char_to_string);
            tctn.add_intersection_node(SystemLibrary.SystemLibrary.char_type, inter,false);
            ShortStringTypes.Add(length, tctn);
            return tctn;
        }

        /****************************modified***********************/
        private int rec_num=0;

        public common_type_node create_record_type(location def_loc, string name)
        {
            if (name == null)
                name = "$record$" + rec_num++;
            SymbolTable.ClassScope scope = convertion_data_and_alghoritms.symbol_table.CreateClassScope(_cmn.scope, null, "record " + name);
            common_type_node tctn = new common_type_node(name, SemanticTree.type_access_level.tal_public, _cmn,
                scope, def_loc);
            if (top_function != null)
                tctn.defined_in_scope = top_function.scope;
            set_field_access_level(SemanticTree.field_access_level.fal_public);
            tctn.SetBaseType(SemanticRules.StructBaseType);
            if (_ctn != null) type_stack.Push(_ctn);
            add_type(name, tctn, def_loc);
            _ctn = tctn;
            tctn.type_special_kind = SemanticTree.type_special_kind.record;
            SystemLibrary.SystemLibrary.init_reference_type(tctn);
            tctn.internal_is_value = true;
            return tctn;

        }

        public void create_generic_indicator(common_type_node generic)
        {
            generic_indicator gi = new generic_indicator(generic);
            int pos = generic.name.LastIndexOf(compiler_string_consts.generic_params_infix);
            string name = generic.name.Substring(0, pos);
            _cmn.scope.AddSymbol(name, new SymbolInfo(gi, access_level.al_public, symbol_kind.sk_none));
        }

        public void save_var_definitions()
        {
            var_defs_stack.Push(var_defs);
            var_defs = new System.Collections.Generic.List<var_definition_node>();
        }

        public void restore_var_definitions()
        {
            var_defs = var_defs_stack.Pop();
        }

        public string BuildName(string name)
        {
            if (converting_block() == block_type.function_block)
            {
                return name + "$" + rec_num++;
            }
            else
            {
                return name;
            }
        }
        /*************************************************************/

        internal statements_list CurrentStatementList
        {
            get
            {
                if (!syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack.Empty)
                    return syntax_tree_visitor.convertion_data_and_alghoritms.statement_list_stack.top();
                return null;
            }
        }


        public var_definition_node add_var_definition(string name, location loc)
        {
            return add_var_definition(name, loc, SemanticTree.polymorphic_state.ps_common);
        }
        public var_definition_node add_var_definition(string name, location loc, SemanticTree.polymorphic_state ps)
        {
            return add_var_definition(name, loc, null, ps);
        }
        
        public var_definition_node add_var_definition(string name, location loc, type_node tn, expression_node inital_value)
        {
            var_definition_node vdn = add_var_definition(name, loc, tn, SemanticTree.polymorphic_state.ps_common);
            vdn.inital_value = GetInitalValueForVariable(vdn, inital_value);
            return vdn;
        }
		
        public var_definition_node add_var_definition_in_entry_scope(string name, location loc)
        {
        	namespace_variable nsv=new namespace_variable(name,null,_cmn,loc);
			//_cmn.scope.AddSymbol(name,new SymbolInfo(nsv));
            CurrentScope.AddSymbol(name, new SymbolInfo(nsv));
			_cmn.variables.AddElement(nsv);
            var_defs.Add(nsv);
            return nsv;		
        }

        public common_namespace_event add_namespace_event(string name, location loc, type_node tn)
        {
            namespace_variable nv = new namespace_variable(name + "$", tn, converted_namespace, loc);
            common_namespace_event cne = new common_namespace_event(name, tn, converted_namespace, null, null, null, loc);
            common_namespace_function_node add_func = new common_namespace_function_node(compiler_string_consts.GetAddHandler(name),
                null, this.converted_namespace, null);
            common_parameter cp = new common_parameter("value", tn, SemanticTree.parameter_type.value, add_func, concrete_parameter_type.cpt_none, null, null);
            add_func.parameters.AddElement(cp);
            expression_node fld_ref = null;
            fld_ref = new namespace_variable_reference(nv, null);
            expression_node en = this.syntax_tree_visitor.convertion_data_and_alghoritms.type_constructor.delegate_add_assign_compile_time_executor
                (null, new expression_node[2] { fld_ref, new common_parameter_reference(cp, 0, null) });
            //en = this.syntax_tree_visitor.convertion_data_and_alghoritms.create_simple_function_call(tn.find_in_type(compiler_string_consts.assign_name).sym_info as function_node,null,
            //                                                                                    fld_ref,en);
            add_func.function_code = new statements_list(null);
            (add_func.function_code as statements_list).statements.AddElement(en);
            //remove
            common_namespace_function_node remove_func = new common_namespace_function_node(compiler_string_consts.GetRemoveHandler(name), null, this.converted_namespace, null);

            cp = new common_parameter("value", tn, SemanticTree.parameter_type.value, add_func, concrete_parameter_type.cpt_none, null, null);
            remove_func.parameters.AddElement(cp);
            en = this.syntax_tree_visitor.convertion_data_and_alghoritms.type_constructor.delegate_sub_assign_compile_time_executor
                (null, new expression_node[2] { fld_ref, new common_parameter_reference(cp, 0, null) });
            //en = this.syntax_tree_visitor.convertion_data_and_alghoritms.create_simple_function_call(tn.find_in_type(compiler_string_consts.assign_name).sym_info as function_node,null,
            //                                                                                    fld_ref,en);
            remove_func.function_code = new statements_list(null);
            (remove_func.function_code as statements_list).statements.AddElement(en);
            this.converted_namespace.functions.AddElement(add_func);
            this.converted_namespace.functions.AddElement(remove_func);
            cne.set_add_function(add_func);
            cne.set_remove_function(remove_func);
            cne.field = nv;
            this.converted_namespace.events.AddElement(cne);
            this.converted_namespace.variables.AddElement(nv);
            CurrentScope.AddSymbol(name, new SymbolInfo(cne));
            return cne;
        }

        public common_event add_event_definition(string name, location loc, type_node tn, SemanticTree.polymorphic_state ps, bool is_abstract)
        {
            class_field cf = new class_field(name + "$", tn, converted_type, ps, _fal, loc);
            common_event ce = new common_event(name, tn, converted_type, null, null, null, _fal, ps, loc);
            //add
            common_method_node add_meth = new common_method_node(compiler_string_consts.GetAddHandler(name), null, this.converted_type,
                                                                 ps, SemanticTree.field_access_level.fal_public, null);
            common_parameter cp = new common_parameter("value", tn, SemanticTree.parameter_type.value, add_meth, concrete_parameter_type.cpt_none, null, null);
            add_meth.parameters.AddElement(cp);
            expression_node fld_ref = null;
            if (!cf.IsStatic) fld_ref = new class_field_reference(cf, new this_node(converted_type, null), null);
            else fld_ref = new static_class_field_reference(cf, null);
            expression_node en = this.syntax_tree_visitor.convertion_data_and_alghoritms.type_constructor.delegate_add_assign_compile_time_executor
                (null, new expression_node[2] { fld_ref, new common_parameter_reference(cp, 0, null) });

            if (!is_abstract)
            {
                add_meth.function_code = new statements_list(null);
                (add_meth.function_code as statements_list).statements.AddElement(en);
            }
            converted_type.scope.AddSymbol(add_meth.name, new SymbolInfo(add_meth));
            //remove
            common_method_node remove_meth = new common_method_node(compiler_string_consts.GetRemoveHandler(name), null, this.converted_type,
                                                                 ps, SemanticTree.field_access_level.fal_public, null);

            cp = new common_parameter("value", tn, SemanticTree.parameter_type.value, add_meth, concrete_parameter_type.cpt_none, null, null);
            remove_meth.parameters.AddElement(cp);
            en = this.syntax_tree_visitor.convertion_data_and_alghoritms.type_constructor.delegate_sub_assign_compile_time_executor
                (null, new expression_node[2] { fld_ref, new common_parameter_reference(cp, 0, null) });
            if (!is_abstract)
            {
                remove_meth.function_code = new statements_list(null);
                (remove_meth.function_code as statements_list).statements.AddElement(en);
            }
            converted_type.scope.AddSymbol(remove_meth.name, new SymbolInfo(remove_meth));
            this.converted_type.methods.AddElement(add_meth);
            this.converted_type.methods.AddElement(remove_meth);
            ce.set_add_method(add_meth);
            ce.set_remove_method(remove_meth);
            ce.field = cf;
            this.converted_type.events.AddElement(ce);
            if (!is_abstract)
                this.converted_type.fields.AddElement(cf);
            converted_type.scope.AddSymbol(name, new SymbolInfo(ce));
            return ce;
        }
        
        public bool check_name_redefinition = true;

        public var_definition_node add_field(string name, location loc, type_node tn, SemanticTree.polymorphic_state ps)
        {
            class_field cf = new class_field(name, tn, _ctn, ps, _fal, loc);
            //_ctn.Scope.AddSymbol(name,new SymbolInfo(cf));
            CurrentScope.AddSymbol(name, new SymbolInfo(cf));
            _ctn.fields.AddElement(cf);
            return cf;
        }

		public var_definition_node add_var_definition(string name, location loc, type_node tn, SemanticTree.polymorphic_state ps, bool not_add_to_varlist=false)
		{
			check_name_free(name,loc);
			var_definition_node vdn=null;
            if (CurrentScope is SymbolTable.BlockScope)
            {
                if (SemanticRules.DisabledDefinitionBlockVariablesWithSameNameThatInAboveScope)
                {
                    SymbolTable.Scope cs = CurrentScope;
                    while (cs.TopScope != null /*(cs.TopScope is SymbolTable.BlockScope || cs.TopScope is SymbolTable.UnitPartScope)*/)
                    {
                    	if (cs.TopScope is SymbolTable.BlockScope)//projdemsja po blokam koda
                    	{
                    		if (cs.TopScope.FindOnlyInScope(name) != null)
                                syntax_tree_visitor.AddError(loc, "BLOCK_VARIABLES_CANNOT_HAVE_NAMES_UPPER_SCOPE");
                    		cs = cs.TopScope;
                    	}
                    	else // a zdes proverjaem v verhnem bloke i vse, to chto eshe vyshe, tam ne proverjaem, tak dejstvuet princip blochnosti
                    	{
                    		if (cs.TopScope.FindOnlyInScope(name) != null)
                                syntax_tree_visitor.AddError(loc, "BLOCK_VARIABLES_CANNOT_HAVE_NAMES_UPPER_SCOPE");
                    		break;
                    	}
                        
                    }
                }
            	local_block_variable lv = new local_block_variable(name, tn, CurrentStatementList, loc);
                CurrentScope.AddSymbol(name, new SymbolInfo(lv));
                if (!not_add_to_varlist)
                lv.block.local_variables.Add(lv);
                if (tn == null) //Тип еще неизвестен, будем закрывать.
                    var_defs.Add(lv);
                return lv;
            }
            switch (converting_block())
			{
				case block_type.function_block:
				{
                    common_function_node top_func = _func_stack.top();
					local_variable lv=new local_variable(name,tn,top_func,loc);
					vdn=lv;
					top_func.var_definition_nodes_list.AddElement(lv);
                    CurrentScope.AddSymbol(name, new SymbolInfo(lv));
					//top_func.scope.AddSymbol(name,new SymbolInfo(lv));
					break;
				}
				case block_type.type_block:
				{
                    //TODO:сделать static и virtual.
                    class_field cf = new class_field(name,tn, _ctn, ps,_fal,loc);
					vdn=cf;
					//_ctn.Scope.AddSymbol(name,new SymbolInfo(cf));
                    CurrentScope.AddSymbol(name, new SymbolInfo(cf));
					_ctn.fields.AddElement(cf);
					break;
				}
				case block_type.namespace_block:
				{
					namespace_variable nsv=new namespace_variable(name,tn,_cmn,loc);
					vdn=nsv;
					//_cmn.scope.AddSymbol(name,new SymbolInfo(nsv));
                    CurrentScope.AddSymbol(name, new SymbolInfo(nsv));
					_cmn.variables.AddElement(nsv);
					break;
				}
			}
            if (tn == null) //Тип еще неизвестен, будем закрывать.
                var_defs.Add(vdn);
            return vdn;
		}

        //ssyy
        public label_node add_label_declaration(string name, location loc)
        {
            check_name_free(name, loc);
            label_node lab = new label_node(name, loc);
            switch (converting_block())
            {
                case block_type.function_block:
                    {
                        common_function_node top_func = _func_stack.top();
                        top_func.label_nodes_list.Add(lab);
                        top_func.scope.AddSymbol(name, new SymbolInfo(lab));
                        break;
                    }
                case block_type.namespace_block:
                    {
                        _cmn.scope.AddSymbol(name, new SymbolInfo(lab));
                        _cmn.labels.Add(lab);
                        break;
                    }
                default:
                    {
                        AddError(loc, "LABEL_CAN_NOT_BE_DECLARED_HERE");
                        break;
                    }
            }
            return lab;
        }
        //\ssyy

		private int num_of_for_cycles
		{
			get
			{
				switch (converting_block())
				{
					case block_type.function_block:
					{
						return top_function.num_of_for_cycles;
					}
					case block_type.namespace_block:
					{
						return _num_of_for_cycles;
					}
					case block_type.type_block:
					{
						throw new CompilerInternalError("For cycle in class body");
                        //return 0;
                    }
                }
				throw new CompilerInternalError("Invalid block type");
			}
			set
			{
				switch (converting_block())
				{
					case block_type.function_block:
					{
						top_function.num_of_for_cycles=value;
						break;
					}
					case block_type.namespace_block:
					{
                        //TODO: Переделать. Число циклов не должно хранится в функции.
						_num_of_for_cycles=value;
						break;
					}
					case block_type.type_block:
					{
						throw new CompilerInternalError("For cycle in class body");
					}
				}
			}
		}

        private int get_and_postinc_num_of_for_cycles()
        {
            int temp = _num_of_for_cycles;
            _num_of_for_cycles++;
            return temp;
        }

        public string get_delegate_type_name()
        {
            return (compiler_string_consts.delegate_type_name_template + _num_for_delegates++);
        }

		public var_definition_node create_for_temp_variable(type_node type,location loc)
		{
			num_of_for_cycles=num_of_for_cycles+1;
			string name=compiler_string_consts.temp_for_variable_name+num_of_for_cycles.ToString();
			//return create_temp_variable(name,type,loc);
            return add_var_definition(name, loc, type,  SemanticTree.polymorphic_state.ps_common);
		}

		public var_definition_node create_temp_variable(string name,type_node type,location loc)
		{
			var_definition_node vdn=null;
			switch (converting_block())
			{
				case block_type.function_block:
				{
                    common_function_node top_func = _func_stack.top();
					local_variable lv=new local_variable(name,type,top_func,loc);
					vdn=lv;
					top_func.var_definition_nodes_list.AddElement(lv);
					break;
				}
				case block_type.type_block:
				{
                    //TODO:сделать static и virtual.
					class_field cf=new class_field(name,type,_ctn,SemanticTree.polymorphic_state.ps_common,_fal,loc);
					vdn=cf;
					_ctn.fields.AddElement(cf);
					break;
				}
				case block_type.namespace_block:
				{
					namespace_variable nsv=new namespace_variable(name,type,_cmn,loc);
					vdn=nsv;
					_cmn.variables.AddElement(nsv);
					break;
				}
			}
			return vdn;
		}

		public common_parameter add_parameter(string name,SemanticTree.parameter_type pt,
			concrete_parameter_type cpt,location loc)
		{
			check_name_free(name,loc);
#if (DEBUG)
			if (converting_block()!=block_type.function_block)
			{
				throw new CompilerInternalError("Parameters can be defined with functions only");
			}
#endif
            common_function_node top_func = _func_stack.top();
			common_parameter cp=new common_parameter(name,pt,top_func,cpt,loc);
			top_func.parameters.AddElement(cp);
			top_func.scope.AddSymbol(name,new SymbolInfo(cp));
			var_defs.Add(cp);
			return cp;
		}

		public constant_definition_node add_const_definition(string name,location loc)
		{
			constant_definition_node cdn=null;
			//cdn.loc=loc;
            check_name_free(name, loc);
			switch (converting_block())
			{
				case block_type.function_block:
				{
                    common_function_node top_func = _func_stack.top();
                    function_constant_definition fcd = new function_constant_definition(name, loc, top_func);
					top_func.scope.AddSymbol(name,new SymbolInfo(fcd));
                    top_func.constants.AddElement(fcd);
                    cdn = fcd;
					break;
				}
				case block_type.type_block:
				{
                    class_constant_definition ccd = new class_constant_definition(name, loc, _ctn, _fal);
					_ctn.Scope.AddSymbol(name,new SymbolInfo(ccd));
					_ctn.const_defs.AddElement(ccd);
                    cdn = ccd;
					break;
				}
				case block_type.namespace_block:
				{
                    namespace_constant_definition ncd = new namespace_constant_definition(name, loc, _cmn);
					_cmn.scope.AddSymbol(name,new SymbolInfo(ncd));
					_cmn.constants.AddElement(ncd);
                    cdn = ncd;
					break;
				}
			}
			return cdn;
		}
		
		public static void add_convertions_to_enum_type(common_type_node tctn)
		{
			SystemLibrary.SystemLibrary.make_binary_operator(compiler_string_consts.gr_name,tctn,SemanticTree.basic_function_type.enumgr,SystemLibrary.SystemLibrary.bool_type);
            SystemLibrary.SystemLibrary.make_binary_operator(compiler_string_consts.greq_name,tctn,SemanticTree.basic_function_type.enumgreq,SystemLibrary.SystemLibrary.bool_type);
            SystemLibrary.SystemLibrary.make_binary_operator(compiler_string_consts.sm_name,tctn,SemanticTree.basic_function_type.enumsm,SystemLibrary.SystemLibrary.bool_type);
            SystemLibrary.SystemLibrary.make_binary_operator(compiler_string_consts.smeq_name,tctn,SemanticTree.basic_function_type.enumsmeq,SystemLibrary.SystemLibrary.bool_type);
            
			SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.byte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itob,false);
            SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itosb,false);
            SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.short_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itos,false);
            SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itous,false);
            SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.integer_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.none,false);
            SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.uint_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itoui,false);
            SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.int64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itol,false);
            SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.uint64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itoul,false);
            
            SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,tctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btoi,false);
            SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,tctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtoi,false);
            SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,tctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stoi,false);
            SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,tctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustoi,false);
            SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,tctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,false);
            SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,tctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitoi,false);
            SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,tctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltoi,false);
            SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,tctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultoi,false);
		}
		
        public common_type_node create_enum_type(string name, location def_loc)
        {
            if (name == null)
                name = "$enum$" + rec_num++;
            SymbolTable.ClassScope scope = convertion_data_and_alghoritms.symbol_table.CreateClassScope(_cmn.scope, null, "enum_type " + name);
            common_type_node tctn = new common_type_node(name, SemanticTree.type_access_level.tal_public, _cmn,
                scope, def_loc);
            if (top_function != null)
                tctn.defined_in_scope = top_function.scope;
            set_field_access_level(SemanticTree.field_access_level.fal_public);
            //_cmn.scope.AddSymbol(name, new SymbolInfo(tctn));
            tctn.SetBaseType(compiled_type_node.get_type_node(NetHelper.NetHelper.EnumType));
            tctn.internal_is_value = true;
            
            add_convertions_to_enum_type(tctn);
            if (_ctn != null) type_stack.Push(_ctn);
            add_type(name, tctn, def_loc);
            _ctn = tctn;
            SystemLibrary.SystemLibrary.init_reference_type(tctn);
            return tctn;
        }

		public void close_var_definition_list(type_node tp, expression_node inital_value)
		{
			if (var_defs.Count==0)
				return;
            int count = var_defs.Count;
            for (int i=0; i<count; i++)
            {
            	var_defs[i].type = tp;
            	bool flag = false;
                if (var_defs[i] is common_parameter && ((var_defs[i] as common_parameter).parameter_type == SemanticTree.parameter_type.var || (var_defs[i] as common_parameter).concrete_parameter_type == concrete_parameter_type.cpt_const))
            		flag = true;
            	if (!flag)
            	var_defs[i].inital_value = GetInitalValueForVariable(var_defs[i],inital_value);
            }
			var_defs.Clear();
		}

        private bool IsConstructorCall(expression_node expr)
        {
            return (expr is common_constructor_call) || (expr is compiled_constructor_call);
        }
        
        internal expression_node GetInitalValueForVariable(var_definition_node vdn, expression_node userInitalValue)
        {
            if (userInitalValue != null)
            {
            	if (CurrentStatementList != null)
                {
                    //Инициализировать надо в текущем стейтменте
                    location lid = ((local_block_variable)vdn).loc;
                    local_block_variable_reference lbvr = new local_block_variable_reference((local_block_variable)vdn, lid);
                    if (vdn.type.type_special_kind == SemanticTree.type_special_kind.set_type)
                	{
                		userInitalValue = syntax_tree_visitor.get_init_call_for_set_as_constr(vdn,userInitalValue);
                	//userInitalValue.type = SystemLibrary.SystemLibInitializer.TypedSetType.sym_info as type_node;
                	}
                    else if (vdn.type.type_special_kind == SemanticTree.type_special_kind.short_string)
                    {
                        userInitalValue = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info as function_node, null, convertion_data_and_alghoritms.convert_type(userInitalValue, SystemLibrary.SystemLibrary.string_type), new int_const_node((vdn.type as short_string_type_node).Length, null));
                    }
                    else if (userInitalValue is array_initializer)
                	{
                		array_initializer arr = userInitalValue as array_initializer;
                		if (vdn.type.element_type.type_special_kind == SemanticTree.type_special_kind.short_string)
                		{
                			for (int i=0; i<arr.element_values.Count; i++)
                			{
                				//arr.element_values[i] = syntax_tree_visitor.find_operator(compiler_string_consts.assign_name, varref2, arr.element_values[i], null);
                				arr.element_values[i] = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info as function_node,null,convertion_data_and_alghoritms.convert_type(arr.element_values[i],SystemLibrary.SystemLibrary.string_type),new int_const_node((vdn.type.element_type as short_string_type_node).Length,null));
                			}
                		}
                	}
                    CurrentStatementList.statements.AddElement(syntax_tree_visitor.find_operator(compiler_string_consts.assign_name, lbvr, userInitalValue, lid));
                    if (vdn.type.type_special_kind == SemanticTree.type_special_kind.set_type)
                	{
                 		lbvr.type = SystemLibrary.SystemLibInitializer.TypedSetType.sym_info as type_node;
                	}
                    return null;
                }
                if (userInitalValue is constant_node)
                {
                    if (vdn.type.type_special_kind == SemanticTree.type_special_kind.short_string)
                	{
						expression_node varref2 = convertion_data_and_alghoritms.CreateVariableReference(vdn, null);
						userInitalValue = syntax_tree_visitor.find_operator(compiler_string_consts.assign_name, varref2, userInitalValue, null);
                	}
                	return userInitalValue;
                }
                if (userInitalValue is array_initializer)
                {
                	array_initializer arr = userInitalValue as array_initializer;
                	if (vdn.type.element_type.type_special_kind == SemanticTree.type_special_kind.short_string)
                	{
                		expression_node varref2 = convertion_data_and_alghoritms.CreateVariableReference(vdn, null);
                		for (int i=0; i<arr.element_values.Count; i++)
                		{
                			//arr.element_values[i] = syntax_tree_visitor.find_operator(compiler_string_consts.assign_name, varref2, arr.element_values[i], null);
                			arr.element_values[i] = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info as function_node,null,convertion_data_and_alghoritms.convert_type(arr.element_values[i],SystemLibrary.SystemLibrary.string_type),new int_const_node((vdn.type.element_type as short_string_type_node).Length,null));
                		}
                	}
                	return userInitalValue;
                }
                expression_node varref = convertion_data_and_alghoritms.CreateVariableReference(vdn, userInitalValue.location);
                if (vdn.type.type_special_kind == SemanticTree.type_special_kind.set_type)
                {
                	userInitalValue = syntax_tree_visitor.get_init_call_for_set_as_constr(vdn,userInitalValue);
                	//userInitalValue.type = SystemLibrary.SystemLibInitializer.TypedSetType.sym_info as type_node;
                }
                else if (vdn.type.type_special_kind == SemanticTree.type_special_kind.short_string)
                {
					expression_node cmc = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info as function_node,null,convertion_data_and_alghoritms.convert_type(userInitalValue,SystemLibrary.SystemLibrary.string_type),new int_const_node((vdn.type as short_string_type_node).Length,null));
        			userInitalValue = cmc;
                }
                userInitalValue = syntax_tree_visitor.find_operator(compiler_string_consts.assign_name, varref, userInitalValue, userInitalValue.location);
                if (vdn.type.type_special_kind == SemanticTree.type_special_kind.set_type)
                {
                 	varref.type = SystemLibrary.SystemLibInitializer.TypedSetType.sym_info as type_node;
                }
                return userInitalValue;
            }
            type_node tp = vdn.type;
            if (syntax_tree_visitor.SystemUnitAssigned && SystemLibrary.SystemLibInitializer.TextFileType.Found && tp.name == compiler_string_consts.text_file_name_type_name)
                if (tp == SystemLibrary.SystemLibInitializer.TextFileType.TypeNode)
                    SystemLibrary.SystemLibInitializer.TextFileType.TypeNode.type_special_kind = PascalABCCompiler.SemanticTree.type_special_kind.text_file;
            //(ssyy) Вставил switch и обработку BinaryFile
            switch (tp.type_special_kind)
            {
                case SemanticTree.type_special_kind.typed_file:
                    userInitalValue = syntax_tree_visitor.get_init_call_for_typed_file(vdn, tp.element_type);
                    break;
                case SemanticTree.type_special_kind.set_type:
                    userInitalValue = syntax_tree_visitor.get_init_call_for_set(vdn);
                    break;
                case SemanticTree.type_special_kind.binary_file:
                    userInitalValue = syntax_tree_visitor.get_init_call_for_binary_file(vdn);
                    break;
                case SemanticTree.type_special_kind.text_file:
                    userInitalValue = syntax_tree_visitor.get_init_call_for_text_file(vdn);
                    break;
                default:
                    if (tp is short_string_type_node)
                        userInitalValue = SystemLibrary.SystemLibrary.empty_string;//syntax_tree_visitor.get_init_call_for_short_string(vdn);
                    else
                        if (tp == SystemLibrary.SystemLibrary.string_type && SemanticRules.InitStringAsEmptyString)
                            userInitalValue = SystemLibrary.SystemLibrary.empty_string;
                    break;
            }
            return userInitalValue;
        }

        public SymbolInfo find_first(string name)
        {
            var temp = find(name);
            return temp?.FirstOrDefault();
        }

        public List<SymbolInfo> find(string name)
        {
            SymbolTable.Scope curscope = CurrentScope;
            if (name == "$yield_element_type")
            {
            	if (top_function != null)
            	{
            		return new List<SymbolInfo> { new SymbolInfo(top_function.return_value_type.instance_params[0]) };
            	}
            	else
            	{
            		type_node tn = converted_type.ImplementingInterfaces.Find(x=>(x as type_node).full_name.StartsWith("System.Collections.Generic.IEnumerable")) as type_node;
            		return new List<SymbolInfo> { new SymbolInfo(tn.instance_params[0]) };
            	}
            }
            
            	
            List<SymbolInfo> sil = curscope.Find(name, curscope);
            List<SymbolInfo> si2 = null;
            if (sil != null && sil.FirstOrDefault().scope is SymbolTable.ClassScope && curscope.TopScope is SymbolTable.BlockScope && curscope.TopScope.TopScope is SymbolTable.ClassMethodScope && curscope.TopScope.TopScope.TopScope is SymbolTable.BlockScope)
            {
                si2 = curscope.TopScope.TopScope.TopScope.Find(name, curscope.TopScope.TopScope.TopScope);
                if (si2 != null && si2.FirstOrDefault().scope is SymbolTable.ClassMethodScope)
                {
                    sil = si2;
                }
            }
                
            if (sil == null && _compiled_tn != null && curscope.TopScope != null)
            {
                SymbolTable.Scope tmp = curscope.TopScope;
                while (tmp != null && sil == null)
                {
                    sil = tmp.Find(name, curscope);
                    tmp = tmp.TopScope;
                }
            }
            if (_ctn != null && _ctn.base_generic_instance != null)
            {
                return _ctn.base_generic_instance.ConvertSymbolInfo(sil);
            }

            return sil;
        }

        public List<SymbolInfo> find_only_in_namespace(string name)
        {
            return CurrentScope.FindOnlyInScope(name);
        }

        public void leave_interface_part()
        {
            _cmn = null;
        }

        public void check_all_name_unit_defined(common_unit_node cmn)
        {
            check_predefinition_defined(cmn.namespaces[0]);
            check_predefinition_defined(cmn.namespaces[1]);
        }

		public void leave_block()
		{
			switch (converting_block())
			{
				case block_type.function_block:
				{
                    common_function_node top_func = _func_stack.top();
                    location loc;
                    if (top_func.function_code == null)
                    {
                        loc = top_func.loc;
                    }
                    else if ((top_func.function_code as statements_list) != null)
                    {
                        statements_list stl = (statements_list)top_func.function_code;
                        if (stl.statements.Count > 0)
                        {
                            loc = stl.statements[stl.statements.Count - 1].location;
                            loc = null; //ivan
                        }
                        else
                        {
                            loc = top_func.loc;
                        }
                    }
                    else
                    {
                        loc = top_func.function_code.location;
                    }
                    if (top_func.function_code != null)
                    {
                        if (top_func.return_variable != null)
                        {
                            //TODO: Обратить внимание на преобразования типов. Можно сделать метод, который если тело функции не statement создает список statement-ов и добавляет в него существующий statement.
                            statements_list stl = (statements_list)top_func.function_code;
                            local_variable_reference lvr = new local_variable_reference(top_func.return_variable, 0, loc);
                            stl.statements.AddElement(new return_node(lvr, loc));
                        }
                        else if ((top_func.return_value_type != null) && (_ctn != null))
                        {
                            //TODO: Обратить внимание на преобразования типов. Можно сделать метод, который если тело функции не statement создает список statement-ов и добавляет в него существующий statement.
                            statements_list stl = (statements_list)top_func.function_code;
                            this_node thn = new this_node(_ctn, loc);
                            stl.statements.AddElement(new return_node(thn, loc));
                        }
                    }
                    check_predefinition_defined();
					_func_stack.pop();
					break;
				}
				case block_type.type_block:
				{
                    if (_cmn.types.IndexOf(_ctn) < 0)
                    {
                        _cmn.types.AddElement(_ctn);
                    }
                    //(ssyy) Проверим тип на реализацию функций интерфейсов
                    check_predefinition_defined();
                    if (_ctn.is_generic_type_definition && !_ctn.IsInterface && _ctn.static_constr == null)
                    {
                        _ctn.static_constr = new common_method_node(PascalABCCompiler.TreeConverter.compiler_string_consts.static_ctor_prefix + "Create", null, _ctn, SemanticTree.polymorphic_state.ps_static, SemanticTree.field_access_level.fal_private, null);
                        _ctn.static_constr.is_constructor = true;
                        statements_list st = new statements_list(null); 
                        st.statements.AddElement(new return_node(new null_const_node(SystemLibrary.SystemLibrary.object_type, null), null));
                        _ctn.static_constr.function_code = st;
                        _ctn.methods.AddElement(_ctn.static_constr);
                    }
                    _ctn=null;
					break;
				}
				case block_type.namespace_block:
				{
                    check_predefinition_defined();
					_cmn=null;
					break;
				}
			}
		}

        private base_function_call CreateTypeCheckCall(definition_node func, type_node tp)
        {
            if (func is common_namespace_function_node)
            {
                common_namespace_function_call cnfc = new common_namespace_function_call(
                    func as common_namespace_function_node, null);
                expression_node ex = new typeof_operator(tp, null);
                cnfc.parameters.AddElement(ex);
                return cnfc;
            }
            else
            {
                compiled_static_method_call cnfc = new compiled_static_method_call(
                    func as compiled_function_node, null);
                expression_node ex = new typeof_operator(tp, null);
                cnfc.parameters.AddElement(ex);
                return cnfc;
            }
        }

        private void AddTypeCheckToStaticConstructor(common_type_node generic_def)
        {
            statements_list stl = new statements_list(null);
            GenericParameterAbilities gpa;
            foreach (common_type_node param in generic_def.generic_params)
            {
                if (syntax_tree_visitor.generic_param_abilities.TryGetValue(param, out gpa))
                {
                    if (gpa.useful_for_pointers)
                    {
                        stl.statements.AddElement(CreateTypeCheckCall(SystemLibrary.SystemLibInitializer.CheckCanUsePointerOnTypeProcedure.sym_info, param));
                    }
                    if (gpa.useful_for_binary_files)
                    {
                        stl.statements.AddElement(CreateTypeCheckCall(SystemLibrary.SystemLibInitializer.CheckCanUseTypeForBinaryFilesProcedure.sym_info, param));
                    }
                    if (gpa.useful_for_typed_files)
                    {
                        stl.statements.AddElement(CreateTypeCheckCall(SystemLibrary.SystemLibInitializer.CheckCanUseTypeForTypedFilesProcedure.sym_info, param));
                    }
                }
                if (SemanticRules.RuntimeInitVariablesOfGenericParameters)
                {
                    basic_function_call bfc = new basic_function_call(
                        SystemLibrary.SystemLibrary.byte_assign as basic_function_node, null);
                    bfc.parameters.AddElement(new static_class_field_reference(param.runtime_initialization_marker, null));
                    base_function_call dcall = null;
                    if (SystemLibrary.SystemLibInitializer.RuntimeDetermineTypeFunction.sym_info is common_namespace_function_node)
                        dcall = new common_namespace_function_call(
                            SystemLibrary.SystemLibInitializer.RuntimeDetermineTypeFunction.sym_info as common_namespace_function_node, null);
                    else
                        dcall = new compiled_static_method_call(
                            SystemLibrary.SystemLibInitializer.RuntimeDetermineTypeFunction.sym_info as compiled_function_node, null);
                    dcall.parameters.AddElement(new typeof_operator(param, null));
                    bfc.parameters.AddElement(dcall);
                    stl.statements.AddElement(bfc);
                }
            }
            if (stl.statements.Count == 0) return;
            string stat_ctor_name = compiler_string_consts.static_ctor_prefix + compiler_string_consts.default_constructor_name;
            List<SymbolInfo> sil = generic_def.scope.FindOnlyInScope(stat_ctor_name);
            common_method_node stat_ctor;
            if (sil == null)
            {
                stat_ctor = new common_method_node(stat_ctor_name, null, null, generic_def,
                    SemanticTree.polymorphic_state.ps_static, SemanticTree.field_access_level.fal_public, null);
                stat_ctor.is_constructor = true;
                generic_def.add_name(stat_ctor_name, new SymbolInfo(stat_ctor));
                generic_def.methods.AddElement(stat_ctor);
            }
            else
            {
                stat_ctor = sil.FirstOrDefault().sym_info as common_method_node;
            }
            if (stat_ctor.function_code != null)
            {
                stl.statements.AddElement(stat_ctor.function_code);
            }
            else
            {
                stl.statements.AddElement(new return_node(null, null));
            }
            stat_ctor.function_code = stl;
        }

		public SymbolInfo last_created_function
		{
			get
			{
				return _last_created_function;
			}
            set
            {
                _last_created_function = value;
            }
		}

        /// <summary>
        /// Возвращает true-уникальная, false-предописанная 
        /// </summary>
        /// <returns></returns>
        public bool close_function_params(bool body_exists)
		{
			if (body_exists)
			{
                //add_overloads_for_default_parameter();
				return close_function_params_with_body();
			}
			else
			{
				close_function_params_without_body();
                return true;
			}
		}

        private void add_overloads_for_default_parameter()
        {
            List<parameter> default_params = new List<parameter>();
            foreach (parameter p in top_function.parameters)
            {
                if (p.default_value != null)
                {
                    default_params.Add(p);
                }
            }
            int num_of_defaults = default_params.Count-1;
            if (default_params.Count > 0 && converted_func_stack.size == 1)
                while (num_of_defaults >= 0)
                {

                    if (converted_type == null)
                    {
                        common_namespace_function_node cnfn = new common_namespace_function_node(top_function.name, top_function.return_value_type, null, converted_namespace, null);
                        foreach (parameter p in top_function.parameters)
                        {
                            if (p.default_value == null)
                                cnfn.parameters.AddElement(p);
                        }
                        for (int i = 0; i < num_of_defaults; i++)
                            cnfn.parameters.AddElement(default_params[i]);
                        statements_list sl = new statements_list(null);
                        common_namespace_function_call cnfc = new common_namespace_function_call(top_function as common_namespace_function_node, null);
                        foreach (common_parameter p in cnfn.parameters)
                        {
                            cnfc.parameters.AddElement(new common_parameter_reference(p, 0, null));
                        }
                        for (int i = num_of_defaults; i < default_params.Count; i++)
                            cnfc.parameters.AddElement(default_params[i].default_value);
                        if (cnfn.return_value_type != null)
                            sl.statements.AddElement(new return_node(cnfc, null));
                        else
                            sl.statements.AddElement(cnfc);
                        cnfn.function_code = sl;
                        converted_namespace.functions.AddElement(cnfn);
                    }
                    else
                    {
                        common_method_node cmn = top_function as common_method_node;
                        common_method_node cnfn = new common_method_node(top_function.name, top_function.return_value_type, null, converted_type, top_function.polymorphic_state, top_function.field_access_level, null);
                        foreach (parameter p in top_function.parameters)
                        {
                            if (p.default_value == null)
                                cnfn.parameters.AddElement(p);
                        }
                        for (int i = 0; i < num_of_defaults; i++)
                            cnfn.parameters.AddElement(default_params[i]);
                        statements_list sl = new statements_list(null);
                        common_method_call cnfc = new common_method_call(cmn, (cnfn.polymorphic_state != SemanticTree.polymorphic_state.ps_static) ? new this_node(converted_type, null) : null, null);
                        foreach (common_parameter p in cnfn.parameters)
                        {
                            cnfc.parameters.AddElement(new common_parameter_reference(p, 0, null));
                        }
                        for (int i = num_of_defaults; i < default_params.Count; i++)
                            cnfc.parameters.AddElement(default_params[i].default_value);
                        if (cnfn.return_value_type != null)
                            sl.statements.AddElement(new return_node(cnfc, null));
                        else
                            sl.statements.AddElement(cnfc);
                        cnfn.function_code = sl;
                        cnfn.is_constructor = cmn.is_constructor;
                        converted_type.methods.AddElement(cnfn);
                    }
                    num_of_defaults--;
                }
        }

		private void close_function_params_without_body()
		{
			check_function_not_exists(_func_stack.top());
		}

        /// <summary>
        /// Вызывает check_unique_or_predefined(top_function). Возвращает true-уникальная, false-предописанная 
        /// </summary>
        /// <returns></returns>
		private bool close_function_params_with_body()
		{
			return check_unique_or_predefined(_func_stack.top());
		}
		
		private List<string> CurrentHandlerList=null;
		
		private Stack<List<string>> CurrentHandlerListStack = new Stack<List<string>>();
		
		public void enter_exception_handlers()
		{
			CurrentHandlerListStack.Push(CurrentHandlerList);
			CurrentHandlerList = new List<string>();
		}
		
		public void leave_exception_handlers()
		{
			CurrentHandlerList = CurrentHandlerListStack.Pop();
		}
		
		public void check_name_free(string name,SemanticTree.ILocation name_loc)
		{
			List<SymbolInfo> sil=find_only_in_namespace(name);
			if (sil==null)
			{
				if (!check_name_redefinition)
					CurrentHandlerList.Add(name.ToLower());
				return;
			}
			if (!check_name_redefinition)
				if (CurrentHandlerList.Contains(name.ToLower()))
				return;
			location first_loc=convertion_data_and_alghoritms.get_location(sil.FirstOrDefault().sym_info);
            //TODO: Можно передавать список всех повторных объявлений.
            AddError(new NameRedefinition(name, first_loc, name_loc));
		}
		
        //ssyy
        public common_type_node check_type_name_free_and_predop(string name, SemanticTree.ILocation name_loc, ref common_type_node partial_class, bool is_partial=false)
        {
            List<SymbolInfo> sil = find_only_in_namespace(name);
            if (sil == null)
            {
                if (is_partial)
                {
                    sil = find(name);
                    if (sil != null)
                    {
                        List<common_type_node> ctn_list = new List<common_type_node>();
                        foreach(SymbolInfo si in sil)
                        {
                            if (si.sym_info is common_type_node && (si.sym_info as common_type_node).IsPartial)
                                ctn_list.Add(si.sym_info as common_type_node);
                        }
                        common_type_node first_ctn = null;
                        if (ctn_list.Count > 0)
                            first_ctn = ctn_list[0];
                        
                        for (int i = 1; i < ctn_list.Count; i++)
                        {
                            ctn_list[i - 1].scope.PartialScope = ctn_list[i].scope;
                            first_ctn.Merge(ctn_list[i]);
                        }
                        partial_class = first_ctn;
                        return null;
                    }
                }
                return null;
            }
            common_type_node cnode = sil.FirstOrDefault().sym_info as common_type_node;
            if (cnode != null && cnode.ForwardDeclarationOnly)
            {
                //cnode.ForwardDeclarationOnly = false;
                return cnode;
            }
            if (cnode != null && is_partial && cnode.IsPartial)
            {
                return cnode;
            }
            location first_loc = convertion_data_and_alghoritms.get_location(sil.FirstOrDefault().sym_info);
            AddError(new NameRedefinition(name, first_loc, name_loc));
            return null;
        }
        //\ssyy


		private bool is_predefinition(common_function_node predef, common_function_node def, bool error_on_false)
		{
			if (convertion_data_and_alghoritms.function_eq_params(predef,def))
			{
				for(int param_num=0;param_num<def.parameters.Count;param_num++)
                {
                    int res=SystemLibrary.SystemLibrary.string_comparer.Compare(def.parameters[param_num].name,
                        predef.parameters[param_num].name);
					if (res!=0)
					{
						AddError(((common_parameter)def.parameters[param_num]).loc,
                            "DIFFERENT_PARAMETER_NAME_IN_FUNCTION_DEFINITION_{0}_AND_PREDEFINITION_{1}",
                            ((common_parameter)def.parameters[param_num]).name,
                            ((common_parameter)predef.parameters[param_num]).name
							);
					}
                    if(predef.parameters[param_num].parameter_type!=def.parameters[param_num].parameter_type)
                        AddError(def.loc, "FUNCTION_DEFINITION_HAVE_DIFFERENT_PARAMS_WITH_PREDEFINITION");
				}
				return true;
			}
			if (error_on_false)
			{
                if (def is common_namespace_function_node && (def as common_namespace_function_node).ConnectedToType != null && predef is common_method_node)
                {
                    return false;
                }
                AddError(def.loc, "FUNCTION_DEFINITION_HAVE_DIFFERENT_PARAMS_WITH_PREDEFINITION");
			}
			return false;
		}

        private void check_predefinition_defined(common_namespace_node cnn)
        {
            foreach (common_function_node cfn in cnn.functions)
                if (cfn.function_code == null)
                    AddError(cfn.loc, "FUNCTION_PREDEFINITION_WITHOUT_DEFINITION");
            foreach (common_type_node ctn in cnn.types)
            {
                //(ssyy) Для интерфейсов не проверяем
                if (!ctn.IsInterface)
                {
                    foreach (common_method_node cmn in ctn.methods)
                        if (cmn.function_code == null && cmn.polymorphic_state != SemanticTree.polymorphic_state.ps_virtual_abstract)
                            AddError(cmn.loc, "FUNCTION_PREDEFINITION_WITHOUT_DEFINITION");
                }
            }
        }
		
        private void check_implement_abstract_function(common_type_node cnode, function_node meth, type_node interf)
        {
        	List<SymbolInfo> sil = cnode.find_in_type(meth.name, cnode.Scope);
            function_node fn = null;
            if (sil != null)
            {
                foreach (var si in sil)
                {
                    if (si.sym_info.general_node_type == general_node_type.function_node)
                    {
                        fn = si.sym_info as function_node;
                        //Сверяем параметры и тип возвращаемого значения
                        if (convertion_data_and_alghoritms.function_eq_params_and_result(meth, fn))
                        {
                            //Нашли нужную функцию
                            if (meth == fn || fn is common_method_node && (fn as common_method_node).overrided_method == null) sil = null;
                            break;
                        }
                    }
                    //Переходим к следующей функции-кандидату
                }
            }
            if (sil == null)
            {
                cnode.SetIsAbstract(true);
                return;
                //Нет функции с таким именем, набором параметров и возвращаемым значением
                //AddError(new AbstractMemberNotImplemented(cnode.name, interf.name, Tools.GetFullMethodHeaderString(meth), cnode.is_value_type, cnode.loc));
            }
            //Теперь проверяем на public и non-static
            bool bad = false;
            common_method_node commn = sil.FirstOrDefault().sym_info as common_method_node;
            compiled_function_node compn = sil.FirstOrDefault().sym_info as compiled_function_node;
            SemanticTree.polymorphic_state pstate = SemanticTree.polymorphic_state.ps_static;
            //Проверка пройдена!

            if (commn != null && commn.common_comprehensive_type == cnode)
            {
                //Найденная функция описана в самом классе.
                if (commn.polymorphic_state == SemanticTree.polymorphic_state.ps_common)
                {
                    //Делаем её virtual final
                    commn.is_final = true;
                    commn.newslot_awaited = true;
                }
            }
            else
            {
                //Найденная функция описана в каком-то предке класса
                //Генерируем новую newslot virtual final функцию, вызывающую ту.
                if (pstate == SemanticTree.polymorphic_state.ps_common)
                {
                    syntax_tree_visitor.generate_inherited_from_base_and_interface_function(cnode, fn);
                }
            }
        }

        //Проверка типа на наличие функции из интерфейса
        private void check_implement_function(common_type_node cnode, function_node meth, type_node interf)
        {
            //Ищем все функции с нужным именем в типе и его предках
            List<SymbolInfo> sil = cnode.find_in_type(meth.name, cnode.Scope);
            List<SymbolInfo> tmp_si = null;
            if (meth is compiled_function_node)
                tmp_si = cnode.find_in_type((meth as compiled_function_node).cont_type.BaseFullName + "." + meth.name, cnode.Scope);
            else if (meth is common_method_node)
                tmp_si = cnode.find_in_type((meth as common_method_node).cont_type.BaseFullName + "." + meth.name, cnode.Scope);
            if (tmp_si != null)
            {
                /*Зачем это?
                 * SymbolInfo tmp_si2 = tmp_si;
                while (tmp_si2.Next != null)
                    tmp_si2 = tmp_si2.Next;*/
                if (sil != null)
                    sil.Insert(0, tmp_si.FirstOrDefault());
                else
                {
                    sil = tmp_si;
                    sil.RemoveRange(1, sil.Count() - 1);
                }
            }

            function_node fn = null;
            SymbolInfo find_method = null;
            if (sil != null) {
                foreach (SymbolInfo si in sil)
                {
                    if (si.sym_info.general_node_type == general_node_type.function_node)
                    {
                        if (si.sym_info is common_method_node && (si.sym_info as common_method_node).cont_type == interf)
                            continue;
                        fn = si.sym_info as function_node;
                        //Сверяем параметры и тип возвращаемого значения
                        if (convertion_data_and_alghoritms.function_eq_params_and_result(meth, fn, true) /*&& fn.polymorphic_state != SemanticTree.polymorphic_state.ps_virtual_abstract*/)
                        {
                            //Нашли нужную функцию
                            common_method_node fn_common = fn as common_method_node;
                            if (fn_common != null)
                            {
                                if (fn_common.name_case_fixed)
                                {
                                    if (fn_common.name != meth.name)
                                    {
                                        syntax_tree_visitor.AddError(fn_common.loc, "AMBIGUITY_BETWEEN_NAMES_{0}_AND_{1}", fn_common.name, meth.name);
                                    }
                                }
                                else
                                {
                                    fn_common.SetName(meth.name);
                                    fn_common.name_case_fixed = true;
                                }

                            }
                            find_method = si;
                            break;
                        }
                    }
                    //Переходим к следующей функции-кандидату
                }
            }
            if (find_method == null)
            {
                //Нет функции с таким именем, набором параметров и возвращаемым значением
                AddError(new InterfaceMemberNotImplemented(cnode.PrintableName, interf.PrintableName, Tools.GetFullMethodHeaderString(meth), cnode.is_value_type, cnode.loc));
            }
            //Теперь проверяем на public и non-static
            bool bad = false;
            common_method_node commn = find_method.sym_info as common_method_node;
            compiled_function_node compn = find_method.sym_info as compiled_function_node;
            SemanticTree.polymorphic_state pstate = SemanticTree.polymorphic_state.ps_static;
            if (commn != null)
            {
                pstate = commn.polymorphic_state;
                if (commn.polymorphic_state == SemanticTree.polymorphic_state.ps_static ||
                    commn.field_access_level != SemanticTree.field_access_level.fal_public ||
                    commn.is_constructor)
                {
                    bad = true;
                }
            }
            else
            {
                if (compn != null)
                {
                    pstate = compn.polymorphic_state;
                    if (compn.polymorphic_state == SemanticTree.polymorphic_state.ps_static ||
                        compn.field_access_level != SemanticTree.field_access_level.fal_public)
                    {
                        bad = true;
                    }
                }
                else
                {
                    //Сюда заходить не должны
                    bad = true;
                }
            }
            if (bad)
            {
                AddError(new DerivedFromInterfaceMethodMustBePublicAndNonStatic(
                    cnode.name, interf.name,
                    Tools.GetFullMethodHeaderString(meth), cnode.is_value_type, cnode.loc));
            }
            //Проверка пройдена!

            if (commn != null && commn.common_comprehensive_type == cnode)
            {
                //Найденная функция описана в самом классе.
                if (commn.polymorphic_state == SemanticTree.polymorphic_state.ps_common)
                {
                    //Делаем её virtual final
                    commn.is_final = true;
                    commn.newslot_awaited = true;
                }
            }
            else
            {
                //Найденная функция описана в каком-то предке класса
                //Генерируем новую newslot virtual final функцию, вызывающую ту.
                if (pstate == SemanticTree.polymorphic_state.ps_common)
                {
                    syntax_tree_visitor.generate_inherited_from_base_and_interface_function(cnode, fn);
                }
            }
        }

        //Проверка типа на соответствие заявленным интерфейсам
        private void check_implement_interfaces()//common_type_node cnode)
        {
            //Переводим контекст в состояние разбора класса cnode
            //_ctn = cnode;
            common_type_node cnode = _ctn;
            foreach (type_node interf in cnode.ImplementingInterfaces)
            {
                generic_instance_type_node gitn = interf as generic_instance_type_node;
                if (gitn != null)
                {
                    List<function_node> gitn_meths = gitn.all_methods;
                    foreach (function_node gitn_meth in gitn_meths)
                    {
                        check_implement_function(cnode, gitn_meth, interf);
                    }
                }
                else
                {
                    common_type_node cint = interf as common_type_node;
                    if (cint != null)
                    {
                        foreach (common_method_node meth in cint.methods)
                        {
                            check_implement_function(cnode, meth, interf);
                        }
                    }
                    else
                    {
                        compiled_type_node compiled_interf = interf as compiled_type_node;
#if (DEBUG)
                        if (compiled_interf == null)
                        {
                            throw new CompilerInternalError("Unknown interface type.");
                        }
#endif
                        System.Reflection.MemberInfo[] interf_members = compiled_interf.compiled_type.GetMembers();
                        foreach (System.Reflection.MemberInfo mi in interf_members)
                        {
                            if (mi.MemberType == System.Reflection.MemberTypes.Method)
                            {
                                compiled_function_node cmeth = compiled_function_node.get_compiled_method((System.Reflection.MethodInfo)mi);
                                check_implement_function(cnode, cmeth, interf);
                            }
                        }
                    }
                }
            }
			
            //(ssyy) Дальше, как я понимаю, вставлена проверка на реализацию
            //абстрактных функций базового класса.
            if (_ctn.base_type != null)
            {
                type_node tn = _ctn.base_type;
                while (tn != null)
                {
                    common_type_node cint = tn as common_type_node;
                    if (cint != null)
                    {
                        if (_ctn.IsAbstract) return;
                        foreach (common_method_node meth in cint.methods)
                        {
                            if (meth.polymorphic_state == SemanticTree.polymorphic_state.ps_virtual_abstract)
                                check_implement_abstract_function(cnode, meth, cint);
                        }

                    }
                    else
                    {
                        compiled_type_node compiled_interf = tn as compiled_type_node;
#if (DEBUG)
                        if (compiled_interf == null)
                        {
                            throw new CompilerInternalError("Unknown interface type.");
                        }
#endif
                        System.Reflection.MemberInfo[] interf_members = compiled_interf.compiled_type.GetMembers(System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance);
                        foreach (System.Reflection.MemberInfo mi in interf_members)
                        {
                            if (mi.MemberType == System.Reflection.MemberTypes.Method)
                            {
                                compiled_function_node cmeth = compiled_function_node.get_compiled_method((System.Reflection.MethodInfo)mi);
                                if (cmeth.polymorphic_state == SemanticTree.polymorphic_state.ps_virtual_abstract && !cmeth.IsSpecialName)
                                    check_implement_abstract_function(cnode, cmeth, tn);
                            }
                            else if (mi.MemberType == System.Reflection.MemberTypes.Property)
                            {
                                /*compiled_property_node cmeth = compiled_property_node.get_compiled_method((System.Reflection.PropertyInfo)mi);
                                if (cmeth.polymorphic_state == SemanticTree.polymorphic_state.ps_virtual_abstract)
                                check_implement_function(cnode, cmeth, _ctn.base_type);*/
                            }
                        }
                    }
                    tn = tn.base_type;
                }
            }
            //Доразбор класса окончен
            //_ctn = null;
        }

		public void check_predefinition_defined()
		{
			switch(converting_block())
			{
				case block_type.function_block:
				{
					foreach(common_function_node cfn in _func_stack.top().functions_nodes_list)
					{
						if (cfn.function_code==null)
						{
                            AddError(cfn.loc, "FUNCTION_PREDEFINITION_WITHOUT_DEFINITION");
						}
					}
                    check_labels(_func_stack.top().label_nodes_list);
                    break;
				}
				case block_type.namespace_block:
				{
                    //(ssyy) Добавил условие
                    if (!_cmn.predefinitions_checked)
                    {
                        foreach (common_function_node cfn in _cmn.functions)
                        {
                            if (cfn.function_code == null)
                            {
                                AddError(cfn.loc, "FUNCTION_PREDEFINITION_WITHOUT_DEFINITION");
                            }
                        }
                        foreach (common_type_node ctn in _cmn.types)
                        {
                            //ssyy
                            if (!ctn.IsInterface)
                            {
                                //\ssyy
                                foreach (common_method_node cmn in ctn.methods)
                                {
                                    if (cmn.function_code == null && cmn.polymorphic_state != SemanticTree.polymorphic_state.ps_virtual_abstract)
                                    {
                                        AddError(cmn.loc, "FUNCTION_PREDEFINITION_WITHOUT_DEFINITION");
                                    }
                                }
                            }
                            if (ctn.is_generic_type_definition && !ctn.IsInterface && !ctn.IsDelegate)
                            {
                                AddTypeCheckToStaticConstructor(ctn);
                            }
                        }
                        _cmn.predefinitions_checked = true;
                    }
					break;
				}
				case block_type.type_block:
				{
                    //ssyy
                    //Проверяем, что тип реализует заявленные интерфейсы.
                    if (!_ctn.IsInterface)
                    {
                        check_implement_interfaces();//_ctn);
                    }
                    //\ssyy
					break;
				}
				default:
				{
					throw new CompilerInternalError("Undefined block type");
				}
			}
		}

        //ssyy
        public void check_labels(List<label_node> lab_list)
        {
            foreach (label_node ln in lab_list)
            {
                if (!ln.is_defined && ln.goto_statements.Count > 0)
                {
                    //(ssyy) Метка не поставлена, но есть goto на неё.
                    AddError(ln.goto_statements[0].location, "NO_LABEL_{0}_DEFINITION", ln.name);
                }
            }
        }

        //\ssyy

        //(ssyy) "Я" бывают разные :)

		//Я хочу модифицировать этот метод так, чтобы в случае предописания наличия метода
		//он не выкидывал исключение, а понимал все правильно
		//Этот метод вызывается только для функций с телом

        /// <summary>
        /// Ищет предописание функции, производя необходимые проверки. Возвращает true-уникальная, false-предописанная
        /// </summary>
        /// <param name="fn">Проверяемая функция</param>
        /// <returns></returns>
		private bool check_unique_or_predefined(common_function_node fn)
		{
            List<SymbolInfo> si_list = null;
            bool in_unit = false;
			if (_func_stack.size<=1)
			{
                if (_explicit_interface_type != null)
                {
                    si_list = _explicit_interface_type.find_in_type(fn.name);
                }
				else if (_ctn!=null)
				{
					si_list = _ctn.Scope.FindOnlyInScope(fn.name);
                    if (_ctn is compiled_generic_instance_type_node && fn is common_namespace_function_node)
                    { 
                        si_list = (_ctn as compiled_generic_instance_type_node).compiled_original_generic.find_in_type(fn.name);
                        if (si_list != null)
                        {
                            foreach (var tmp_si in si_list)
                            {
                                if (tmp_si.sym_info.general_node_type != general_node_type.function_node)
                                {
                                    TreeRealization.BasePCUReader.RestoreSymbols(si_list, fn.name);
                                }
                                common_namespace_function_node cnfn = tmp_si.sym_info as common_namespace_function_node;
                                if (cnfn != null)
                                {
                                    if (cnfn.namespace_node == (fn as common_namespace_function_node).namespace_node)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (_compiled_tn != null)
                {
                    si_list = _compiled_tn.find_in_type(fn.name);
                }
                else if (fn is common_namespace_function_node && (fn as common_namespace_function_node).ConnectedToType != null)
                {
                    si_list = (fn as common_namespace_function_node).ConnectedToType.find_in_type(fn.name);
                }
                else
                {
                    in_unit = true;
                    si_list = _cmn.scope.FindOnlyInScope(fn.name);
                }
            }
			else
			{
				common_function_node temp=_func_stack.pop();
                si_list = _func_stack.top().scope.FindOnlyInScope(fn.name);
				_func_stack.push(temp);
			}
            bool predef_find = false;
            int overloads = 0;
            if(si_list != null)
                foreach (var tmp_si in si_list)
                {
                    if (tmp_si.sym_info != fn && !(si_list != null && si_list.FirstOrDefault().sym_info is basic_function_node))
                        overloads++;
                }
            if(si_list != null)
                foreach(var tmp_si in si_list)
			    {
				    if (tmp_si.sym_info==fn)
					    continue;
                    if (tmp_si.sym_info.general_node_type != general_node_type.function_node)
                    {
                        TreeRealization.BasePCUReader.RestoreSymbols(si_list, fn.name);
                    }
    #if (DEBUG)
				    if (tmp_si.sym_info.general_node_type!=general_node_type.function_node)
				    {
                        throw new CompilerInternalError("Function name is used to define not function.");
				    }
    #endif
                    if (tmp_si.sym_info is basic_function_node || tmp_si.sym_info is compiled_function_node)
                    {
                        continue;
                    }
				    //compar - найденная функция
				    common_function_node compar=((common_function_node)(tmp_si.sym_info));
				
				    //Проверяем, если мы нашли не предописание
                    if (compar.function_code != null || is_order_independed_method_description)
                    {
                        if (compar.is_overload == false)
                        {
                            AddError(new FunctionMustBeWithOverloadDirective(compar, fn));
                        }
                        if (convertion_data_and_alghoritms.function_eq_params(fn, compar, false) && fn.is_extension_method == compar.is_extension_method)
                        {
                    	    if (fn.IsOperator && (fn.name == compiler_string_consts.explicit_operator_name || fn.name == compiler_string_consts.implicit_operator_name))
                    	    {
                    		    if (convertion_data_and_alghoritms.function_eq_params_and_result(fn,compar))
                    			    AddError(new FunctionDuplicateDefinition(compar, fn));
                    	    }
                    	    else
                            {
                                if (fn is common_method_node && compar is common_method_node)
                                {
                                    common_method_node cmn1 = fn as common_method_node;
                                    common_method_node cmn2 = compar as common_method_node;
                                    if (cmn1.explicit_interface == cmn2.explicit_interface)
                                        AddError(new FunctionDuplicateDefinition(compar, fn));
                                }
                                else
                                { 
                                    AddError(new FunctionDuplicateDefinition(compar, fn));
                                }
                            }
                        }
                        continue;
                    }
               
				    bool is_find_predefinition=false;

				    if (compar.is_overload==false || overloads == 1 && in_unit)
				    {
					    //Если нет параметров в описании тела функции
					    if (fn.parameters.Count==0)
					    {
                        
						    is_find_predefinition=true;
					    }
					    else
					    {
						    is_find_predefinition=is_predefinition(compar,fn,true);
					    }
				    }
				    else
				    {
					    is_find_predefinition=is_predefinition(compar,fn,false);
				    }

                    if (!is_find_predefinition)
                    {
                        continue;
                    }
                    else
                    {
                        predef_find = true;
                        foreach (common_parameter par in fn.parameters)
                        {
                            if (par.default_value != null)
                            {
                                syntax_tree_visitor.AddError(par.loc, "DEFAULT_VALUE_MUST_BE_ONLY_IN_FIRST_DECLARATION");
                            }
                        }
                    }

                    if (compar.function_code!=null)
                    {
                        AddError(fn.loc, "FUNCTION_{0}_ALREADY_HAVE_A_BODY", fn.name);
                    }

                    if ((compar.polymorphic_state == PascalABCCompiler.SemanticTree.polymorphic_state.ps_static) !=
                        (fn.polymorphic_state == PascalABCCompiler.SemanticTree.polymorphic_state.ps_static))
                    {
                        syntax_tree_visitor.AddError(fn.loc, "CLASS_FUNCTION_ATTRIBUTE_MISMATCH");
                    }

                    common_method_node compiled_meth = fn as common_method_node;

                    if (compiled_meth != null)
                    {
                        common_method_node finded_method = (common_method_node)compar;
                        if (compiled_meth.is_constructor)
                        {
                            if (finded_method.is_constructor == false)
                            {
                                AddError(compiled_meth.loc, "CONSTRUCTOR_MUST_BE_REALIZED_WITH_CONSTRUCTOR_KEYWORD");
                            }
                            compiled_meth.return_value_type = finded_method.return_value_type;
                            convertion_data_and_alghoritms.create_function_return_variable(finded_method,null);
                        }
                    }

				    if (!convertion_data_and_alghoritms.eq_type_nodes(compar.return_value_type,fn.return_value_type) && fn.return_value_type != null && !(fn.return_value_type is undefined_type))
				    {
					    AddError(fn.loc, "FUNCTION_PREDEFINITION_AND_DEFINITION_HAVE_DIFFERENT_RESULT_TYPES");
				    }

				    //НУЖЕН СПИСОК ФУНКЦИЙ С ПРЕДОПРЕДЕЛЕНИЯМИ, ЧТОБЫ ОБНАРУЖИТЬ ОПИСАННЫЕ НО НЕ ОПРЕДЕЛЕННЫЕ
				    //Или не нужен???

				    //TODO: СИНХРОНИЗИРОВАТЬ АТТРИБУТЫ!!!!!
				    if (fn.is_overload)
				    {
					    compar.is_overload=true;
				    }

                    //TODO: Внимательно проверить следующий if.
                    if (compar.is_overload)
                    {
                        fn.is_overload = true;
                        last_created_function.symbol_kind = symbol_kind.sk_overload_function;
                    }
				
				    common_function_node cfn11=_func_stack.pop();
                    compar.scope = cfn11.scope;

                    common_method_node cmnode=compar as common_method_node;
                    if (cmnode != null)
                    {
                        List<SymbolInfo> si_local = cfn11.scope.FindOnlyInScope(compiler_string_consts.self_word);
                        if (si_local != null)
                        {
                            si_local.FirstOrDefault().sym_info = cmnode.self_variable;
                        }
                    }

                    //Удаляем текущую функцию
                    List<SymbolInfo> siint = null;
				    switch(converting_block())
				    {
					    case block_type.function_block:
					    {
						    _func_stack.top().functions_nodes_list.remove((common_in_function_function_node)fn);
                            siint = _func_stack.top().scope.FindOnlyInScope(fn.name);
						    break;
					    }
					    case block_type.namespace_block:
					    {
						    _cmn.functions.remove((common_namespace_function_node)fn);
                            siint = _cmn.scope.FindOnlyInScope(fn.name);
						    break;
					    }
					    case block_type.type_block:
					    {
                            //TODO: Доделать описание методов класса.
                            _ctn.methods.remove((common_method_node)fn);
                            siint = _ctn.scope.FindOnlyInScope(fn.name);
						    break;
					    }
                        case block_type.compiled_type_block:
                        {
                            if (syntax_tree_visitor.compiled_unit.namespaces[0].functions.Contains((common_namespace_function_node)fn))
                                syntax_tree_visitor.compiled_unit.namespaces[0].functions.remove((common_namespace_function_node)fn);
                            siint = _compiled_tn.find_in_type(fn.name);
                            break;
                        }

					    default:
					    {
						    throw new CompilerInternalError("Undefined block type");
					    }
				    }
                    bool compar_not_assign = true;
                    if (siint != null)
                    {
                        foreach (var siint_unit in siint)
                        {
                            if (siint_unit.sym_info == fn)
                            {
                                siint_unit.sym_info = compar;
                                compar_not_assign = false;
                                break;
                            }
                        }
                    }
                    if (compar_not_assign && siint != null)
                        siint.LastOrDefault().sym_info = compar;


                    //Мы удалили новую функцию. Сейчас мы должны перенести параметры из новой функции в старую.
                    //TODO: У новой функции может вообще не быть параметров. Они все в старой. Разобраться с этим.  А зачем вообще переносить параметры?

                    if (compar.parameters.Count != 0)
                    {
                        if (fn.parameters.Count == 0)
                        {
                            foreach (common_parameter pr in compar.parameters)
                            {
                                compar.scope.AddSymbol(pr.name, new SymbolInfo(pr));
                            }
                        }
                        else
                        {
                            foreach (common_parameter pr in compar.parameters)
                            {
                                List<SymbolInfo> par_sim_info = fn.scope.FindOnlyInScope(pr.name); 
                                par_sim_info.FirstOrDefault().sym_info = pr;
                            }
                        }
                    }

                    if (fn.is_generic_function)
                    {
                        for (int i = 0; i < fn.generic_params.Count; ++i)
                        {
                            fn.generic_params[i] = compar.generic_params[i] as common_type_node;
                            common_type_node t_fn = fn.generic_params[i] as common_type_node;
                            common_type_node t_compar = compar.generic_params[i] as common_type_node;
                            /*t_fn.is_class = t_compar.is_class;
                            t_fn.internal_is_value = t_compar.internal_is_value;
                            t_fn.SetImplementingInterfaces(t_compar.ImplementingInterfaces);
                            (t_fn.Scope as SymbolTable.IInterfaceScope).TopInterfaceScopeArray =
                                (t_compar.Scope as SymbolTable.IInterfaceScope).TopInterfaceScopeArray;
                            t_fn.SetBaseType(t_compar.base_type);*/
                            if (t_compar.has_default_constructor)
                            {
                                generic_parameter_eliminations.add_default_ctor(t_fn);
                            }
                            List<SymbolInfo> par_sim_info = fn.scope.FindOnlyInScope(fn.generic_params[i].name);
                            par_sim_info.FirstOrDefault().sym_info = fn.generic_params[i] as common_type_node;
                            //t_fn.generic_function_container = compar;
                        }
                        //конверитируем параметры предописания в параметры описания.
                        List<type_node> fn_types = fn.get_generic_params_list();
                        foreach (common_parameter par in compar.parameters)
                        {
                            par.type = generic_convertions.determine_type(par.type, fn_types, true);
                        }
                        if (compar.return_value_type != null)
                        {
                            compar.return_value_type = generic_convertions.determine_type(compar.return_value_type, fn_types, true);
                            if (compar.return_variable != null)
                            {
                                compar.return_variable.type = compar.return_value_type;
                            }
                        }
                        foreach (generic_parameter_eliminations gpe in compar.parameters_eliminations)
                        {

                        }
                        compar.generic_params = fn.generic_params;
                        foreach (common_type_node tn in compar.generic_params)
                        {
                            tn.generic_function_container = compar;
                        }
                    }

				    _func_stack.push(compar);
				    compar.attributes.AddRange(fn.attributes);
				    //si_list.sym_info=fn;
				    break;
			    }
            if (fn is common_method_node && !predef_find && syntax_tree_visitor.current_converted_method_not_in_class_defined && _explicit_interface_type == null)
            {
                AddError(new NoMethodInClassWithThisParams(fn as common_method_node, (fn as common_method_node).cont_type, fn.loc));
            }
            return !predef_find;
		}

		//Этот метод вызывается для предописания функции (только для предописания)
		private void check_function_not_exists(common_function_node fn)
		{
			List<SymbolInfo> sil=null;
			if (_func_stack.size<=1)
			{
				if (_ctn!=null)
				{
					sil=_ctn.Scope.FindOnlyInScope(fn.name);
				}
				else
				{
					sil=_cmn.scope.FindOnlyInScope(fn.name);
				}
			}
			else
			{
				common_function_node temp=_func_stack.pop();
				sil=_func_stack.top().scope.FindOnlyInScope(fn.name);
				_func_stack.push(temp);
			}
            if(sil != null)
			    foreach(SymbolInfo si in sil)
			    {
                    if (si.sym_info == fn || si.sym_info is basic_function_node)
					    continue;
    #if (DEBUG)
				    if (si.sym_info.general_node_type!=general_node_type.function_node)
				    {
					    throw new CompilerInternalError("Function name is used to define not function.");
				    }
    #endif
				    //compar - найденная функция

                    common_function_node compar=((common_function_node)(si.sym_info));

                    if (compar.is_overload==false)
				    {
					    AddError(new FunctionMustBeWithOverloadDirective(compar,fn));
				    }
				    if (convertion_data_and_alghoritms.function_eq_params(fn,compar,false))
				    {
					    if (fn.IsOperator && (fn.name == compiler_string_consts.explicit_operator_name || fn.name == compiler_string_consts.implicit_operator_name))
                        {
                    	    if (convertion_data_and_alghoritms.function_eq_params_and_result(fn,compar))
                    		    AddError(new FunctionDuplicateDefinition(compar, fn));
                        }
                        else
                        { 
					        AddError(new FunctionDuplicateDefinition(compar,fn));
                        }
				    }
			    }
		}

		private void check_function_name(string name,location def_loc)
		{
			List<SymbolInfo> sil=find_only_in_namespace(name);
			if (sil==null)
			{
				return;
			}

			if (sil.FirstOrDefault().sym_info.general_node_type!=general_node_type.function_node)
			{
				AddError(new FunctionNameIsUsedToDefineSomethigElse(def_loc,sil.FirstOrDefault().sym_info));
			}
		}

		public common_property_node add_property(string property_name,location loc)
		{
			if (_ctn==null)
			{
				throw new CompilerInternalError("Property without class");
			}
			check_name_free(property_name,loc);
            common_property_node pn = new common_property_node(property_name, _ctn, loc, _fal,
                SemanticTree.polymorphic_state.ps_common);
			_ctn.Scope.AddSymbol(property_name,new SymbolInfo(pn));
			_ctn.properties.AddElement(pn);
			return pn;
		}

        //(ssyy) модифицировал 05.07.2007
		public void add_type(string name,type_node tn,location loc)
		{
            if(tn.is_class)
			if (converting_block()!=block_type.namespace_block)
			if (!(converting_block()==block_type.function_block))
			{
				AddError(new ClassCanNotBeDefinedInTypeOrFunction(name,loc));
			}
            compiled_type_node comp_tn = tn as compiled_type_node;
            if (comp_tn != null)
            {
                if (_cmn.runtime_types.IndexOf(comp_tn) < 0)
                {
                    _cmn.runtime_types.AddElement(comp_tn);
                }
            }
            CurrentScope.AddSymbol(name, new SymbolInfo(tn));
            if (tn is ref_type_node && converting_block() == block_type.namespace_block)
                converted_namespace.ref_types.Add(tn as ref_type_node);

		}

		public statement_node code
		{
			get
			{
				switch (converting_block())
				{
					case block_type.function_block:
					{
						return _func_stack.top().function_code;
					}
					case block_type.type_block:
					{
                        throw new CompilerInternalError("Type have no code.");
					}
					case block_type.namespace_block:
					{
						return _main_procedure;
					}
				}
				return null;
			}
			set
			{
				switch (converting_block())
				{
					case block_type.function_block:
					{
						_func_stack.top().function_code=value;
						break;
					}
					case block_type.type_block:
					{
                        throw new CompilerInternalError("Type have no code.");
					}
					case block_type.namespace_block:
					{
						_main_procedure=value;
						break;
					}
				}
			}
		}

		public statement_node_stack cycle_stack
		{
			get
			{
				switch (converting_block())
				{
					case block_type.function_block:
					{
						return _func_stack.top().cycles_stack;
					}
					case block_type.type_block:
					{
						throw new CompilerInternalError("Can not get cycle from this point");
					}
					case block_type.namespace_block:
					{
						return _cycles_stack;
					}
				}
				throw new CompilerInternalError("Undefined block type");
			}
		}

        public void set_virtual(common_method_node cmn)
        {
            cmn.overrided_method = null;
            cmn.newslot_awaited = true;
            cmn.polymorphic_state = SemanticTree.polymorphic_state.ps_virtual;
        }

        //ssyy
        public void set_virtual_abstract(common_method_node cmn)
        {
            cmn.overrided_method = null;
            cmn.newslot_awaited = true;
            cmn.polymorphic_state = SemanticTree.polymorphic_state.ps_virtual_abstract;
        }
        //\ssyy

        public function_node FindMethodToOverride(common_namespace_function_node cmn)
        {
            type_node base_class = _compiled_tn;
            if (base_class == null)
            {
                return null;
            }
            List<SymbolInfo> sil = base_class.find_in_type(cmn.name);
            function_node fn = null;
            if(sil != null)
                foreach(SymbolInfo si in sil)
                {
                    if (si.sym_info.general_node_type != general_node_type.function_node)
                    {
                        return null;
                    }
                    fn = si.sym_info as function_node;
                    //(ssyy) Сверяем как параметры функций, так и типы возвращаемых значений
                    if (cmn != fn && convertion_data_and_alghoritms.function_eq_params_and_result(cmn, fn))
                    {
                        break;
                    }
                }
            if (sil == null)
            {
                return null;
            }
            compiled_function_node cfn_sec = fn as compiled_function_node;

            if (cfn_sec != null)
            {
                if (cfn_sec.polymorphic_state != SemanticTree.polymorphic_state.ps_virtual && cfn_sec.polymorphic_state != SemanticTree.polymorphic_state.ps_virtual_abstract && !cfn_sec.IsFinal)
                {
                    return null;
                }
            }

            return fn;
        }

        public function_node FindMethodToOverride(common_method_node cmn)
        {
            type_node base_class = cmn.cont_type.base_type;
            if (base_class == null)
            {
                return null;
            }
            List<SymbolInfo> sil = base_class.find_in_type(cmn.name, CurrentScope);
            function_node fn = null;
            SymbolInfo find_method = null;
            if (sil != null)
            {
                foreach(SymbolInfo si in sil)
                {
                    if (si.sym_info.general_node_type != general_node_type.function_node)
                    {
                        return null;
                    }
                    fn = si.sym_info as function_node;
                    //(ssyy) Сверяем как параметры функций, так и типы возвращаемых значений
                    if (convertion_data_and_alghoritms.function_eq_params_and_result(cmn, fn))
                    {
                        find_method = si;
                        break;
                    }
                }
            }
            if (find_method == null)
            {
                return null;
            }
            common_method_node cmn_sec = fn as common_method_node;
            if (cmn_sec != null)
            {
                if (cmn_sec.polymorphic_state != SemanticTree.polymorphic_state.ps_virtual && cmn_sec.polymorphic_state != SemanticTree.polymorphic_state.ps_virtual_abstract)
                {
                    return null;
                }
            }
            else
            {
                compiled_function_node cfn_sec = fn as compiled_function_node;
                if (cfn_sec != null)
                {
                    if (cfn_sec.polymorphic_state != SemanticTree.polymorphic_state.ps_virtual && cfn_sec.polymorphic_state != SemanticTree.polymorphic_state.ps_virtual_abstract && !cfn_sec.IsFinal)
                    {
                        return null;
                    }
                }
                else
                {
                    throw new CompilerInternalError("Undefined method type.");
                }
            }
            return fn;
        }

        public property_node FindPropertyToOverride(common_property_node cpn)
        {
            type_node base_class = cpn.common_comprehensive_type.base_type;
            if (base_class == null)
            {
                return null;
            }
            List<SymbolInfo> sil = base_class.find_in_type(cpn.name, CurrentScope);
            property_node pn = null;
            SymbolInfo find_property = null;
            if (sil != null)
            {
                foreach (SymbolInfo si in sil)
                {
                    if (si.sym_info.general_node_type != general_node_type.property_node)
                    {
                        return null;
                    }
                    pn = si.sym_info as property_node;
                    //(ssyy) Сверяем как параметры функций, так и типы возвращаемых значений
                    if (convertion_data_and_alghoritms.function_eq_params_and_result(cpn.get_function, pn.get_function))
                    {
                        find_property = si;
                        break;
                    }
                }
            }
            if (find_property == null)
            {
                return null;
            }
            common_property_node cpn_sec = pn as common_property_node;
            if (cpn_sec != null)
            {
                if (cpn_sec.polymorphic_state != SemanticTree.polymorphic_state.ps_virtual && cpn_sec.polymorphic_state != SemanticTree.polymorphic_state.ps_virtual_abstract)
                {
                    return null;
                }
            }
            else
            {
                compiled_property_node cmpn_sec = pn as compiled_property_node;
                if (cmpn_sec != null)
                {
                    if (cmpn_sec.polymorphic_state != SemanticTree.polymorphic_state.ps_virtual && cmpn_sec.polymorphic_state != SemanticTree.polymorphic_state.ps_virtual_abstract)
                    {
                        return null;
                    }
                }
                else
                {
                    throw new CompilerInternalError("Undefined property type.");
                }
            }
            return pn;
        }

        public void set_override(common_property_node cpn)
        {
            cpn.polymorphic_state = SemanticTree.polymorphic_state.ps_virtual;
            property_node overrided_property = FindPropertyToOverride(cpn);
            if (overrided_property == null)
                AddError(cpn.loc, "NO_PROPERTY_TO_OVERRIDE");
        }

        public void set_override(common_method_node cmn)
        {
            cmn.polymorphic_state = SemanticTree.polymorphic_state.ps_virtual;
            cmn.overrided_method = FindMethodToOverride(cmn);
            if (cmn.overrided_method == null)
                AddError(cmn.loc, "NO_METHOD_TO_OVERRIDE");
            cmn.SetName(cmn.overrided_method.name);
        }

        //ssyy
        //Добавляет блок в стек контекста, охватывающим блоком будет вершина стека
        public void enter_code_block_with_bind()
        {
            if (_block_stack.Count > 0)
            {
                _block_stack.Push(new code_block(_block_stack.Peek()));
            }
            else
            {
                _block_stack.Push(new code_block(null));
            }
        }

        //Добавляет блок в стек контекста, охватывающим блоком будет null
        public void enter_code_block_without_bind()
        {
            _block_stack.Push(new code_block(null));
        }

        //Убирает блок со стека
        public void leave_code_block()
        {
            _block_stack.Pop();
        }

        //Проверяет, возможен ли переход от блока goto_block в label_block
        public bool check_can_goto(code_block label_block, code_block goto_block)
        {
            code_block current_block = goto_block;
            while (current_block != null)
            {
                if (current_block == label_block)
                {
                    return true;
                }
                else
                {
                    current_block = current_block.up_block;
                }
            }
            return false;
        }
        //\ssyy



        internal string get_free_name(string template)
        {
            string name;
            int i = 0;
            while (CurrentScope.Find((name = string.Format(template, i++))) != null)
                ;
            return name;
        }

    }

	/// <summary>
    /// Фасад над compilation_context, служащий для изменения контекста 
    /// </summary>
	public class ContextChanger{
		public compilation_context context;

        private bool is_active;
		
		public ContextChanger(compilation_context context)
		{
			this.context = context;
            this.is_active = false;
		}
		
        /// <summary>
        /// Переход на глобальный уровень с сохранением контеста
        /// </summary>
		public void SaveContextAndUpToGlobalLevel()
        {
            this.is_active = true;
			context.SaveContextAndUpToGlobalLevel();
        }

        /// <summary>
        /// Переход на уровень класса с сохранением контеста
        /// </summary>
        public void SaveContextAndUpFromAllFunctionDefs()
        {
            this.is_active = true;
			context.SaveContextAndUpFromAllFunctionDefs();
        }

        /// <summary>
        /// Переход к ближайшему блоку определения с сохранением контеста
        /// </summary>
        public void SaveContextAndUpToNearestDefSect() 
        {
            this.is_active = true;
			context.SaveContextAndUpToNearestDefSect();
        }

        /// <summary>
        /// Восстановление сохранённого контекста
        /// </summary>
        public void RestoreCurrentContext()
		{
            this.is_active = false;
			context.RestoreCurrentContext();
		}

        public bool IsActive()
        {
            return is_active;
        }
	}
}
