
using System;

using PascalABCCompiler.TreeRealization;

namespace PascalABCCompiler.TreeConverter
{

	public enum block_type {function_block,type_block,namespace_block};

	public class blocks
	{
		private common_namespace_node _cmn;
        //TODO: Можно сделать возможность объявления вложенных типов.
		private common_type_node _ctn;
		private common_function_node_stack _func_stack=new common_function_node_stack();

        private System.Collections.Generic.List<var_definition_node> var_defs =
            new System.Collections.Generic.List<var_definition_node>();

        private readonly System.Collections.Generic.Stack<System.Collections.Generic.List<var_definition_node>> var_defs_stack =
            new System.Collections.Generic.Stack<System.Collections.Generic.List<var_definition_node>>();

        private readonly System.Collections.Generic.List<local_variable> _special_local_vars =
            new System.Collections.Generic.List<local_variable>();

        private System.Collections.Generic.Stack<global::SymbolTable.Scope> _scope_stack
            = new System.Collections.Generic.Stack<SymbolTable.Scope>();

        private statement_node _main_procedure;

		private SymbolInfo _last_created_function;

		private statement_node_stack _cycles_stack=new statement_node_stack();

		private int _num_of_for_cycles;

        private convertion_data_and_alghoritms convertion_data_and_alghoritms;

		private SemanticTree.field_access_level _fal;

        public blocks(convertion_data_and_alghoritms convertion_data_and_alghoritms)
        {
            this.convertion_data_and_alghoritms = convertion_data_and_alghoritms;
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
            rec_num = 1;
            var_defs_stack.Clear();
            type_stack.Clear();
            clear_special_local_vars();
            _scope_stack.Clear();
        }

        public void AddTemplate(string name, template_class tc, location loc)
        {
            check_name_free(name, loc);
            _cmn.scope.AddSymbol(name, new SymbolInfo(tc));
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

        /*
        public System.Collections.Generic.List<local_variable> special_local_vars
        {
            get
            {
                return _special_local_vars;
            }
        }
        */
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
                        break;
                    }
                default:
                    {
                        throw new CompilerInternalError("Undefined block type");
                    }
            }
        }

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
        //\ssyy

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
                scope = convertion_data_and_alghoritms.symbol_table.CreateScope(comprehensive_namespace.scope);
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
            if (_ctn != null && _ctn.is_value_type) return block_type.type_block;

            if (_func_stack.size!=0)
			{
				return block_type.function_block;
			}
			if (_ctn!=null)
			{
				return block_type.type_block;
			}
			return block_type.namespace_block;
		}

        public common_type_node enter_in_type_method(string type_name, location loc)
        {
            SymbolInfo si = find(type_name);
            if (si == null)
            {
                throw new UndefinedNameReference(type_name, loc);
            }
            if (si.sym_info.general_node_type != general_node_type.type_node)
            {
                throw new TypeNameExpected(type_name,si.sym_info,loc);
            }
#if DEBUG
            if (si.Next != null)
            {
                throw new CompilerInternalError("Must find only type, found some other.");
            }
#endif
            if (si.sym_info.semantic_node_type != semantic_node_type.common_type_node)
            {
                throw new OnlyCommonTypeMethodDefinitionAllowed(((type_node)si.sym_info), loc);
            }

            //TODO: В случае создания вложенных классов этот код надо поправить.
            common_type_node ctn = si.sym_info as common_type_node;
            _ctn = ctn;
            return ctn;
        }

        public void leave_type_method()
        {
            _ctn = null;
        }

        private readonly System.Collections.Generic.Stack<common_type_node> type_stack = new System.Collections.Generic.Stack<common_type_node>();
        
        public void leave_record()
        {
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
       
		public common_function_node create_function(string name,location def_loc)
		{
			check_function_name(name,def_loc);
			common_function_node cfn=null;
			switch (converting_block())
			{
				case block_type.function_block:
				{
                    common_function_node top_func = _func_stack.top();
                    SymbolTable.Scope scope = convertion_data_and_alghoritms.symbol_table.CreateScope(top_func.scope);
					common_in_function_function_node ciffn;
					ciffn=new common_in_function_function_node(name,def_loc,top_func,scope);
					top_func.functions_nodes_list.AddElement(ciffn);
					_last_created_function=new SymbolInfo(ciffn);
					top_func.scope.AddSymbol(name,_last_created_function);
					cfn=ciffn;
					break;
				}
				case block_type.type_block:
				{
					common_method_node cmmn;
                    SymbolTable.Scope scope = convertion_data_and_alghoritms.symbol_table.CreateClassMethodScope(_cmn.scope, _ctn.Scope);
                    //TODO:сделать static и virtual.
                    //TODO: interface and implementation scopes.
					cmmn=new common_method_node(name,def_loc,_ctn,SemanticTree.polymorphic_state.ps_common,_fal,scope);
					_last_created_function=new SymbolInfo(cmmn);
					_ctn.Scope.AddSymbol(name,_last_created_function);
					_ctn.methods.AddElement(cmmn);

                    local_variable lv = new local_variable(compiler_string_consts.self_word, _ctn, cmmn, def_loc);
                    cmmn.scope.AddSymbol(compiler_string_consts.self_word, new SymbolInfo(lv));
                    cmmn.self_variable = lv;
                    cmmn.var_definition_nodes_list.AddElement(lv);

					cfn=cmmn;
					break;
				}
				case block_type.namespace_block:
				{
					common_namespace_function_node cnfnn;
                    SymbolTable.Scope scope = convertion_data_and_alghoritms.symbol_table.CreateScope(_cmn.scope);
					cnfnn=new common_namespace_function_node(name,def_loc,_cmn,scope);
					_cmn.functions.AddElement(cnfnn);
					_last_created_function=new SymbolInfo(cnfnn);
					_cmn.scope.AddSymbol(name,_last_created_function);
					cfn=cnfnn;
					break;
				}
			}
			_func_stack.push(cfn);
			return cfn;
		}

		public common_type_node create_type(string name,location def_loc)
        {
			check_name_free(name,def_loc);
			if (converting_block()!=block_type.namespace_block)
			{
				throw new TypeCanNotBeDefinedInTypeOrFunction(name,def_loc);
			}
            SymbolTable.ClassScope scope = convertion_data_and_alghoritms.symbol_table.CreateClassScope(_cmn.scope,null);
			common_type_node tctn=new common_type_node(name,SemanticTree.type_access_level.tal_public,_cmn,
                scope,def_loc);
			set_field_access_level(SemanticTree.field_access_level.fal_public);
			_cmn.scope.AddSymbol(name,new SymbolInfo(tctn));

			//_cmn.types.AddElement(tctn);
			_ctn=tctn;
			SystemLibrary.SystemLibrary.init_reference_type(tctn);
			return tctn;
		}

        /****************************modified***********************/
        private int rec_num=0;

        public common_type_node create_record_type(location def_loc)
        {
            SymbolTable.ClassScope scope = convertion_data_and_alghoritms.symbol_table.CreateClassScope(_cmn.scope, null);
            string name = "$record$" + rec_num++;
            common_type_node tctn = new common_type_node(name, SemanticTree.type_access_level.tal_public, _cmn,
                scope, def_loc);
            set_field_access_level(SemanticTree.field_access_level.fal_public);
            //_cmn.scope.AddSymbol(name, new SymbolInfo(tctn));
            tctn.SetBaseType(compiled_type_node.get_type_node(typeof(ValueType)));
            //_cmn.types.AddElement(tctn);
            if (_ctn != null) type_stack.Push(_ctn);
            _ctn = tctn;
            SystemLibrary.SystemLibrary.init_reference_type(tctn);
            return tctn;
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

		public var_definition_node add_var_definition(string name,location loc)
		{
			check_name_free(name,loc);
			var_definition_node vdn=null;
			switch (converting_block())
			{
				case block_type.function_block:
				{
                    common_function_node top_func = _func_stack.top();
					local_variable lv=new local_variable(name,top_func,loc);
					vdn=lv;
					top_func.var_definition_nodes_list.AddElement(lv);
					top_func.scope.AddSymbol(name,new SymbolInfo(lv));
					var_defs.Add(vdn);
					break;
				}
				case block_type.type_block:
				{
                    //TODO:сделать static и virtual.
                    class_field cf = new class_field(name, _ctn, SemanticTree.polymorphic_state.ps_common,_fal,loc);
					vdn=cf;
					_ctn.Scope.AddSymbol(name,new SymbolInfo(cf));
					_ctn.fields.AddElement(cf);
					var_defs.Add(vdn);
					break;
				}
				case block_type.namespace_block:
				{
					namespace_variable nsv=new namespace_variable(name,_cmn,loc);
					vdn=nsv;
					_cmn.scope.AddSymbol(name,new SymbolInfo(nsv));
					_cmn.variables.AddElement(nsv);
					var_defs.Add(vdn);
					break;
				}
			}
			return vdn;
		}

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
            return (compiler_string_consts.delegate_type_name_template+get_and_postinc_num_of_for_cycles());
        }

		public var_definition_node create_for_temp_variable(type_node type,location loc)
		{
			num_of_for_cycles=num_of_for_cycles+1;
			string name=compiler_string_consts.temp_for_variable_name+num_of_for_cycles.ToString();
			return create_temp_variable(name,type,loc);
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
                    class_constant_definition ccd = new class_constant_definition(name, loc, _ctn);
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

		public void make_constructor()
		{
#if (DEBUG)
			if (converting_block()!=block_type.function_block)
			{
				throw new CompilerInternalError("Create constructor call without function");
			}
			if (_func_stack.top().node_location_kind!=SemanticTree.node_location_kind.in_class_location)
			{
				throw new CompilerInternalError("Create constructor applied to non class method");
			}
#endif
			common_function_node top_func=_func_stack.top();
			
			top_func.return_value_type=_ctn;

			common_method_node top_method=(common_method_node)top_func;

			common_method_node cmn=new common_method_node(top_func.name,_ctn,top_method.loc,_ctn,
                SemanticTree.polymorphic_state.ps_static,_fal,top_func.scope);
			cmn.is_constructor=true;
			cmn.is_overload=top_function.is_overload;

            //parameter_list pl = new parameter_list();
            foreach (common_parameter pr in top_method.parameters)
            {
                common_parameter new_par = new common_parameter(pr.name, pr.type, pr.parameter_type, cmn, pr.concrete_parameter_type,
                    pr.default_value, pr.loc);
                cmn.parameters.AddElement(new_par);
            }
            //cmn.parameters.AddRange(top_method.parameters);
			
			statements_list stl=new statements_list(top_func.loc);
			cmn.function_code=stl;
			this_node thn=new this_node(_ctn,top_func.loc);
			common_method_call csmc=new common_method_call(top_method,thn,top_func.loc);
			foreach(common_parameter cp in cmn.parameters)
			{
				common_parameter_reference cpr=new common_parameter_reference(cp,0,top_func.loc);
				csmc.parametres.AddElement(cpr);
			}
			stl.statements.AddElement(csmc);
			_ctn.methods.AddElement(cmn);

			top_method.pascal_associated_constructor=cmn;
		}

		public void close_var_definition_list(type_node tp)
		{
			if (var_defs.Count==0)
			{
				return;
			}
			foreach(var_definition_node vdn in var_defs)
			{
				vdn.type=tp;
			}
			var_defs.Clear();
		}

		public SymbolInfo find(string name)
		{
			switch (converting_block())
			{
				case block_type.function_block:
				{
					return _func_stack.top().find(name);
				}
				case block_type.type_block:
				{
					return _ctn.find(name);
				}
				case block_type.namespace_block:
				{
					return _cmn.find(name);
				}
			}
			return null;
		}

		public SymbolInfo find_only_in_namespace(string name)
		{
			switch (converting_block())
			{
				case block_type.function_block:
				{
					return _func_stack.top().find_only_in_namespace(name);
				}
				case block_type.type_block:
				{
					return _ctn.Scope.FindOnlyInScope(name);
				}
				case block_type.namespace_block:
				{
					return _cmn.scope.FindOnlyInScope(name);
				}
			}
			return null;
		}

        public void leave_interface_part()
        {
            _cmn = null;
        }

        public void check_all_name_unit_defined(common_unit_node cmn)
        {
            common_namespace_node cfn = cmn.namespaces[0];
            foreach (common_namespace_function_node cnfn in cfn.functions)
            {
                if (cnfn.function_code == null)
                {
                    throw new FunctionPredefinitionWithoutDefinition(cnfn);
                }
            }
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
                    //TODO: Может и сюда вставить check_predefinition_defined, несмотря на то, что в данный момент он ничего не делает.
                    _cmn.types.AddElement(_ctn);
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

		public SymbolInfo last_created_function
		{
			get
			{
				return _last_created_function;
			}
		}

		public void close_function_params(bool body_exists)
		{
			if (body_exists)
			{
				close_function_params_with_body();
			}
			else
			{
				close_function_params_without_body();
			}
		}

		private void close_function_params_without_body()
		{
			check_function_not_exists(_func_stack.top());
		}

		private void close_function_params_with_body()
		{
			check_unique_or_predefined(_func_stack.top());
		}

		public void check_name_free(string name,SemanticTree.ILocation name_loc)
		{
			SymbolInfo si=find_only_in_namespace(name);
			if (si==null)
			{
				return;
			}
			SemanticTree.ILocation first_loc=convertion_data_and_alghoritms.get_location(si.sym_info);
            //TODO: Можно передавать список всех повторных объявлений.
			throw new NameRedefinition(first_loc,name_loc);
		}

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
						throw new DifferentParameterNameInFunctionPredefinitionAndDefinition(
							(common_parameter)predef.parameters[param_num],
							(common_parameter)def.parameters[param_num],
							predef,def);
					}
				}
				return true;
			}
			if (error_on_false)
			{
				throw new FunctionDefinitionHaveDifferentParamsWithPredifinition(predef,def);
			}
			return false;
		}

		private void check_predefinition_defined()
		{
			switch(converting_block())
			{
				case block_type.function_block:
				{
					foreach(common_function_node cfn in _func_stack.top().functions_nodes_list)
					{
						if (cfn.function_code==null)
						{
							throw new FunctionPredefinitionWithoutDefinition(cfn);
						}
					}
					break;
				}
				case block_type.namespace_block:
				{
					foreach(common_function_node cfn in _cmn.functions)
					{
						if (cfn.function_code==null)
						{
							throw new FunctionPredefinitionWithoutDefinition(cfn);
						}
					}
                    foreach (common_type_node ctn in _cmn.types)
                    {
                        foreach (common_method_node cmn in ctn.methods)
                        {
                            if (cmn.function_code == null)
                            {
                                throw new FunctionPredefinitionWithoutDefinition(cmn);
                            }
                        }
                    }
					break;
				}
				case block_type.type_block:
				{
					//TODO: Доделать. На завершение компиляции модуля нужно просматривать все методы типов и функции в чатсях interface и implementation на предмет того, что все объявленные функции определены.
					break;
				}
				default:
				{
					throw new CompilerInternalError("Undefined block type");
				}
			}
		}

		//Я хочу модифицировать этот метод так, чтобы в случае предописания наличия метода
		//он не выкидывал исключение, а понимал все правильно
		//Этот метод вызывается только для функций с телом
		private void check_unique_or_predefined(common_function_node fn)
		{
			SymbolInfo si=null;
			if (_func_stack.size<=1)
			{
				if (_ctn!=null)
				{
                    //TODO: Спрость у Саши насчет того, что возвращает этот метод. Он долхен возвращать список найденных методов, а не единственный экземпляр.
					si=_ctn.Scope.FindOnlyInScope(fn.name);
				}
				else
				{
					si=_cmn.scope.FindOnlyInScope(fn.name);
				}
			}
			else
			{
				common_function_node temp=_func_stack.pop();
                si = _func_stack.top().scope.FindOnlyInScope(fn.name);
				_func_stack.push(temp);
			}
			//SymbolInfo si=find_only_in_namespace(fn.name);

			//И эта проверка наверно не нужна, но пусть пока повисит
			//Тем более с ней, наверное, быстрее работает
			//if (si.Next==null)
			//{
			//	return;
			//}

			//Тут я добавляю проверку: если это предописание, то все хорошо
			/*if (si.Next.Next==null)
			{
				if (fnd.function_code==null)
				{
					
				}
			}*/

			//Как я понимаю все должно работать правильно без следующего фрагмента
			/*if (fn.is_overload==false)
			{
				//fnd - найденная функция
				common_function_node fnd=null;
				if (si.sym_info==fn)
				{
					fnd=(common_function_node)si.Next.sym_info;
				}
				else
				{
					fnd=(common_function_node)si.sym_info;
				}
				throw new FunctionMustBeWithOverloadDirective(fnd,fn);
			}*/

			while(si!=null)
			{
				if (si.sym_info==fn)
				{
					si=si.Next;
					continue;
				}
#if (DEBUG)
				if (si.sym_info.general_node_type!=general_node_type.function_node)
				{
					throw new CompilerInternalError("Function name is used to define not function.");
				}
#endif
				//compar - найденная функция
				common_function_node compar=((common_function_node)(si.sym_info));
				
				//Проверяем, если мы нашли не предописание
				if (compar.function_code!=null)
				{
					if (compar.is_overload==false)
					{
						throw new FunctionMustBeWithOverloadDirective(compar,fn);
					}
					if (convertion_data_and_alghoritms.function_eq_params(fn,compar))
					{
						throw new FunctionDuplicateDefinition(compar,fn);
					}
					si=si.Next;
					continue;
				}

				bool is_find_predefinition=false;

				if (compar.is_overload==false)
				{
					//Если нет параметров в описании тела функции
					if (fn.parameters.Count==0)
					{
                        
						is_find_predefinition=true;
						//Добавляем все описанные параметры
                        /*
						foreach(parameter par in compar.parameters)
						{
							//Тут, конечно, приведение типа параметра, но оно вроде всегда должно быть такого типа.
							common_parameter intcpar=(common_parameter)par;
                            common_parameter new_par = new common_parameter(intcpar.name, intcpar.type, intcpar.parameter_type,
                                fn, intcpar.concrete_parameter_type, intcpar.default_value, intcpar.loc);
                            fn.parameters.AddElement(new_par);
							fn.scope.AddSymbol(new_par.name,new SymbolInfo(new_par));
						}
						compar.scope=fn.scope;
                        */
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
                    si = si.Next;
					continue;
				}

                if (compar.function_code!=null)
                {
                    throw new FunctionAlreadyHaveABody(compar, fn);
                }

                common_method_node compiled_meth = fn as common_method_node;

                if (compiled_meth != null)
                {
                    common_method_node finded_method = (common_method_node)compar;
                    if (compiled_meth.is_constructor)
                    {
                        if (finded_method.pascal_associated_constructor == null)
                        {
                            throw new ConstructorMustBeRealizedWithConstructorKeyword(finded_method,compiled_meth,compiled_meth.loc);
                        }
                        compiled_meth.return_value_type = finded_method.return_value_type;
                        convertion_data_and_alghoritms.create_function_return_variable(finded_method,null);
                    }
                }

				if (compar.return_value_type!=fn.return_value_type)
				{
					throw new FunctionPredefinitionAndDefinitionHaveDifferentResultType(compar,fn);
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
                    SymbolInfo si_local = cfn11.scope.FindOnlyInScope(compiler_string_consts.self_word);
                    if (si_local != null)
                    {
                        si_local.sym_info = cmnode.self_variable;
                    }
                }

				//Удаляем текущую функцию
                SymbolInfo siint = null;
                //TODO: Возможно в коде ниже следует вызывать не Find а FindOnlyInNamespace.
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
					default:
					{
						throw new CompilerInternalError("Undefined block type");
					}
				}
                while (siint.sym_info != fn)
                {
                    siint = siint.Next;
                }
                siint.sym_info = compar;

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
                            SymbolInfo par_sim_info=fn.scope.FindOnlyInScope(pr.name);
                            par_sim_info.sym_info = pr;
                        }
                    }
                }
                /*
                compar.parameters.clear();
                foreach (common_parameter pr in fn.parameters)
                {
                    pr.common_function = compar;
                    compar.parameters.AddElement(pr);
                }
                */

				_func_stack.push(compar);
				
				//si.sym_info=fn;
				break;
			}
		}

		//Этот метод вызывается для предописания функции (только для предописания)
		private void check_function_not_exists(common_function_node fn)
		{
			SymbolInfo si=null;
			if (_func_stack.size<=1)
			{
				if (_ctn!=null)
				{
					si=_ctn.Scope.FindOnlyInScope(fn.name);
				}
				else
				{
					si=_cmn.scope.FindOnlyInScope(fn.name);
				}
			}
			else
			{
				common_function_node temp=_func_stack.pop();
				si=_func_stack.top().scope.FindOnlyInScope(fn.name);
				_func_stack.push(temp);
			}
			//SymbolInfo si=find_only_in_namespace(fn.name);

			//И эта проверка наверно не нужна, но пусть пока повисит
			//Тем более с ней, наверное, быстрее работает
			//Лучше удалю пока
			//if (si.Next==null)
			//{
			//	return;
			//}

			while(si!=null)
			{
				if (si.sym_info==fn)
				{
					si=si.Next;
					continue;
				}
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
					throw new FunctionMustBeWithOverloadDirective(compar,fn);
				}
				if (convertion_data_and_alghoritms.function_eq_params(fn,compar))
				{
					throw new FunctionDuplicateDefinition(compar,fn);
				}
				si=si.Next;
			}
		}

		private void check_function_name(string name,location def_loc)
		{
			SymbolInfo si=find_only_in_namespace(name);
			if (si==null)
			{
				return;
			}

			if (si.sym_info.general_node_type!=general_node_type.function_node)
			{
				throw new FunctionNameIsUsedToDefineSomethigElse(def_loc,si.sym_info);
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

		public void add_type(string name,type_node tn,location loc)
		{
			if (converting_block()!=block_type.namespace_block)
			if (!(converting_block()==block_type.function_block && !tn.is_class))
			{
				throw new TypeCanNotBeDefinedInTypeOrFunction(name,loc);
			}
            //TODO: Почему не было следующего оператора.
            //_cmn.types.AddElement(tn);
            if (tn is compiled_type_node)
                _cmn.runtime_types.AddElement(tn as compiled_type_node);
			_cmn.scope.AddSymbol(name,new SymbolInfo(tn));
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
            cmn.polymorphic_state = SemanticTree.polymorphic_state.ps_virtual;
        }

        public void set_override(common_method_node cmn)
        {
            cmn.polymorphic_state = SemanticTree.polymorphic_state.ps_virtual;
            type_node base_class = cmn.cont_type.base_type;
            if (base_class == null)
            {
                throw new NoMethodToOverride(cmn);
            }
            SymbolInfo si = base_class.find_in_type(cmn.name);
            function_node fn = null;
            while (si != null)
            {
                if (si.sym_info.general_node_type != general_node_type.function_node)
                {
                    throw new NoMethodToOverride(cmn);
                }
                fn = si.sym_info as function_node;
                if (convertion_data_and_alghoritms.function_eq_params(cmn, fn))
                {
                    break;
                }
                si = si.Next;
            }
            if (si == null)
            {
                throw new NoMethodToOverride(cmn);
            }

            common_method_node cmn_sec = fn as common_method_node;
            if (cmn_sec != null)
            {
                if (cmn_sec.polymorphic_state != SemanticTree.polymorphic_state.ps_virtual)
                {
                    throw new NoMethodToOverride(cmn);
                }
            }
            else
            {
                compiled_function_node cfn_sec = fn as compiled_function_node;
                if (cfn_sec != null)
                {
                    if (cfn_sec.polymorphic_state != SemanticTree.polymorphic_state.ps_virtual)
                    {
                        throw new NoMethodToOverride(cmn);
                    }
                }
                else
                {
                    throw new CompilerInternalError("Undefined method type.");
                }
            }

            cmn.overrided_method = fn;
            cmn.SetName(fn.name);
        }

	}

}