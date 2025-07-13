// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
//Некоторые алгоритмы . В основном выбор перегруженного метода. Сильно связан с syntax_tree_visitor.
using System;
using System.Collections.Generic;
using System.Linq;

using PascalABCCompiler.SemanticTree;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeRealization;
using static PascalABCCompiler.SyntaxTree.SyntaxTreeBuilder;

namespace PascalABCCompiler.TreeConverter
{

	public class convertion_data_and_alghoritms
	{
		internal SymbolTable.TreeConverterSymbolTable symtab=SymbolTable.SymbolTableController.CurrentSymbolTable;

		private PascalABCCompiler.Errors.SyntaxError _parser_error;

		private syntax_tree_visitor _stv;

        private type_constructor _type_constructor;

        public statement_list_stack statement_list_stack = new statement_list_stack();

        //private special_operation_kind _current_operation_kind= special_operation_kind.none;

		public convertion_data_and_alghoritms(syntax_tree_visitor stv)
		{
            type_table.type_table_function_call_maker = create_simple_function_call;
			_stv=stv;
            _type_constructor = new type_constructor(this);
            SystemLibrary.SystemLibrary.type_constructor = _type_constructor;
		}

        
        private void AddError(Errors.Error err)
        {
            syntax_tree_visitor.AddError(err);
        }

        private T AddError<T>(Errors.Error err)
        {
            syntax_tree_visitor.AddError(err, true);
            return default(T);
        }

        private T AddError<T>(location loc, string ErrString, params string[] values)
        {
            syntax_tree_visitor.AddError(loc, ErrString, values);
            return default(T);
        }

        private void AddError(location loc, string ErrString, params string[] values)
        {
            syntax_tree_visitor.AddError(loc, ErrString, values);
        }

		public void reset()
		{
			symtab.Clear();
            _type_constructor.reset();
            statement_list_stack.clear();
            arr_nums = 0;
            //_current_operation_kind = special_operation_kind.none;
		}

        /*
        public special_operation_kind current_operation_kind
        {
            get
            {
                return _current_operation_kind;
            }
            set
            {
                _current_operation_kind = value;
            }
        }
        */

        internal void statement_list_stack_push(statements_list sl)
        {
            sl.Scope = new SymbolTable.BlockScope(symbol_table, syntax_tree_visitor.context.CurrentScope);            
            statement_list_stack.push(sl);
        }
        
        internal SymbolTable.Scope create_block_scope()
        {
        	return new SymbolTable.BlockScope(symbol_table, syntax_tree_visitor.context.CurrentScope);
        }
        
        internal void statement_list_stack_push(statements_list sl, SymbolTable.Scope Scope)
        {
            sl.Scope = Scope;
            statement_list_stack.push(sl);
        }

        internal statements_list statement_list_stack_pop()
        {
            return statement_list_stack.pop();
        }
        
        internal statements_list statement_list_stack_first()
        {
        	return statement_list_stack.first();
        }
        
        public type_constructor type_constructor
        {
            get
            {
                return _type_constructor;
            }
        }

		public PascalABCCompiler.Errors.SyntaxError parser_error
		{
			get
			{
				return _parser_error;
			}
            set
            {
                _parser_error = value;
            }
		}
        public System.Collections.Hashtable bad_nodes_in_syntax_tree;

		public SymbolTable.TreeConverterSymbolTable symbol_table
		{
			get
			{
				return symtab;
			}
		}


		public syntax_tree_visitor syntax_tree_visitor
		{
			get
			{
				return _stv;
			}
            set
            {
                _stv = value;
            }
		}

        //TODO: Избавиться от дублирования метода.
		public location get_location(semantic_node sn)
		{
			SemanticTree.ILocated iloc=sn as SemanticTree.ILocated;
			if (iloc==null)
			{
				return null;
			}
            document doc = _stv.CurrentDocument;
            location loc=null;
            //DarkStar changed
            //В PCU не сохраняются location некоторых конструкций, например
            //числовых констант итд. Поэтому необохдимо делать эту проверку.
            //Коля может всетаки нужно хранить?
            if (iloc.Location != null)
                loc = new location(iloc.Location.begin_line_num, iloc.Location.begin_column_num,
                    iloc.Location.end_line_num, iloc.Location.end_column_num, doc);
            return loc;
		}

        //TODO: Избавиться от этого метода. Везде должен быть только location (а не ILocation).
        private location loc_to_loc(SemanticTree.ILocation l)
        {
            if (l == null)
            {
                return null;
            }
            document d = new document(l.document.file_name);
            if (d == null)
                d = syntax_tree_visitor.CurrentDocument;

            location loc = new location(l.begin_line_num, l.begin_column_num, l.end_line_num, l.end_column_num, d);
            return loc;
        }

        //TODO: Вообще нужен-ли этот набор методов (create_..._function_call)?
		private basic_function_call create_basic_function_call(basic_function_node bfn,SemanticTree.ILocation loc,params expression_node[] exprs)
		{
			basic_function_call bfc=new basic_function_call(bfn,loc_to_loc(loc));
			bfc.parameters.AddRange(exprs);
			return bfc;
		}

		private common_namespace_function_call create_common_namespace_function_call(common_namespace_function_node cnfn,
			SemanticTree.ILocation loc,params expression_node[] exprs)
		{
			common_namespace_function_call cnc=new common_namespace_function_call(cnfn,loc_to_loc(loc));
			cnc.parameters.AddRange(exprs);
			return cnc;
		}

		private common_static_method_call create_common_static_method_call(common_method_node cmn,SemanticTree.ILocation loc,
			params expression_node[] exprs)
		{
#if (DEBUG)
			if (cmn.polymorphic_state!=SemanticTree.polymorphic_state.ps_static)
			{
				throw new CompilerInternalError("Not static method can not be called as static");
			}
#endif
			common_static_method_call csmc=new common_static_method_call(cmn,loc_to_loc(loc));
			csmc.parameters.AddRange(exprs);
			return csmc;
		}

        public variable_reference CreateVariableReference(var_definition_node vd, location loc)
        {
            switch (vd.semantic_node_type)
            {
                case semantic_node_type.local_variable:
                    return new local_variable_reference((local_variable)vd, 0, loc);
                case semantic_node_type.class_field:
                    if (vd.polymorphic_state == polymorphic_state.ps_static)
                        return new static_class_field_reference((class_field)vd, loc);
                    else
                        return new class_field_reference((class_field)vd, syntax_tree_visitor.GetCurrentObjectReference((vd as class_field).cont_type.Scope,vd,loc), loc);
                case semantic_node_type.namespace_variable:
                    return new namespace_variable_reference((namespace_variable)vd, loc);
                case semantic_node_type.local_block_variable:
                    return new local_block_variable_reference((local_block_variable)vd, loc);
            }
            throw new NotSupportedError(loc);
        }

		private common_in_function_function_call create_common_in_function_function_call(common_in_function_function_node cfn,
			int static_depth,location loc,params expression_node[] exprs)
		{

			common_in_function_function_call cc=new common_in_function_function_call(cfn,static_depth,loc);
			cc.parameters.AddRange(exprs);
			return cc;
		}

		private common_method_call create_common_method_call(common_method_node cmn,location loc,expression_node obj,
			params expression_node[] exprs)
		{
#if (DEBUG)
			if (cmn.polymorphic_state==SemanticTree.polymorphic_state.ps_static)
			{
				throw new CompilerInternalError("Static method can not be called with type");
			}
#endif
			common_method_call cmc=new common_method_call(cmn,obj,loc);
            cmc.virtual_call = !syntax_tree_visitor.inherited_ident_processing;
			cmc.parameters.AddRange(exprs);
			return cmc;
		}

		private compiled_function_call create_compiled_function_call(compiled_function_node cfn, location loc,
			expression_node obj,params expression_node[] exprs)
		{
#if (DEBUG)
			if (cfn.polymorphic_state==SemanticTree.polymorphic_state.ps_static)
			{
				throw new CompilerInternalError("Static method can not be called with type");
			}
#endif
			compiled_function_call cfc=new compiled_function_call(cfn,obj,loc);
            cfc.virtual_call = !syntax_tree_visitor.inherited_ident_processing;
			cfc.parameters.AddRange(exprs);
			return cfc;
		}

		private compiled_static_method_call create_compiled_static_method_call(compiled_function_node cfn,
			location loc, params expression_node[] exprs)
		{
#if (DEBUG)
			if (cfn.polymorphic_state!=SemanticTree.polymorphic_state.ps_static)
			{
				throw new CompilerInternalError("Not static method can not be called as static");
			}
#endif
			compiled_static_method_call csmc=new compiled_static_method_call(cfn,loc);
			csmc.parameters.AddRange(exprs);
			return csmc;
		}

        public expression_node create_compiled_construcor_call(compiled_constructor_node fn, location loc, params expression_node[] exprs)
        {
            compiled_constructor_call ccc = new compiled_constructor_call(fn, loc);
            ccc.parameters.AddRange(exprs);
            return ccc;
        }
		//Метод только для внутнреннего использования.
		//НЕ ПРЕОБРАЗУЕТ ТИПЫ. Только создает узел вызова метода.
		//Этот метод и используется для создания узлов преобразования типов.
        //TODO: Переименовать.
        public expression_node create_simple_function_call(function_node fn, location loc, params expression_node[] exprs)
        {

            if (fn.compile_time_executor != null)
            {
                expression_node ex = fn.compile_time_executor(loc, exprs);
                if (ex != null)
                {
                    return ex;
                }
            }

            switch (fn.semantic_node_type)
            {
                case semantic_node_type.basic_function_node:
                    {
                        return create_basic_function_call((basic_function_node)fn, loc, exprs);
                    }
                case semantic_node_type.common_namespace_function_node:
                    {
                        return create_common_namespace_function_call((common_namespace_function_node)fn, loc, exprs);
                    }
                case semantic_node_type.common_method_node:
                    {
                        common_method_node cmn = (common_method_node)fn;
                        if (cmn.polymorphic_state != SemanticTree.polymorphic_state.ps_static)
                        {
                            break;
                        }
                        return create_common_static_method_call(cmn, loc, exprs);
                    }
                /*
				case semantic_node_type.pseudo_function:
				{
					pseudo_function pf=(pseudo_function)fn;
					return pf.execute_pseudo_function_algorithm(loc,exprs);
				}
                */
                case semantic_node_type.common_in_function_function_node: break;
                case semantic_node_type.compiled_function_node:
                    {
                        compiled_function_node cfn = (compiled_function_node)fn;
                        if (cfn.polymorphic_state != SemanticTree.polymorphic_state.ps_static)
                        {
                            break;
                        }
                        return create_compiled_static_method_call(cfn, loc, exprs);
                    }
                case semantic_node_type.compiled_constructor_node:
                    {
                        compiled_constructor_call ccc = new compiled_constructor_call((compiled_constructor_node)fn,loc);
                        ccc.parameters.AddRange(exprs);
                        return ccc;

                    }
                case semantic_node_type.indefinite_definition_node:
                    {
                        indefinite_function_call ifc = new indefinite_function_call(fn, loc);
                        ifc.parameters.AddRange(exprs);
                        return ifc;
                    }
                default: throw new CompilerInternalError("Unknown function type");
            }
            throw new CanNotReferenceToNonStaticMethodWithType(fn.name, loc);
        }

        public expression_node create_method_call(function_node fn, location loc, expression_node obj, params expression_node[] exprs)
        {
            if (fn.compile_time_executor != null)
            {
                expression_node ex = fn.compile_time_executor(loc, exprs);
                if (ex != null)
                {
                    return ex;
                }
            }

            switch (fn.semantic_node_type)
            {
                case semantic_node_type.common_method_node:
                    {
                        common_method_node cmn = (common_method_node)fn;
                        
                        return create_common_method_call(cmn, loc, obj, exprs);
                    }

                case semantic_node_type.compiled_function_node:
                    {
                        compiled_function_node cfn = (compiled_function_node)fn;
                        
                        return create_compiled_function_call(cfn, loc, obj, exprs);
                    }
                
                default: throw new CompilerInternalError("Unknown function type");
            }
        }

        /// <summary>
        /// Этот метод вызывается если мы встречаем простой вызов функии, например f(1).
        /// Он определяет является ли эта функция методом класса, вложенной функцией и т.д. и создает соответствующее обращение.
        /// Например, для метода класса он добавляет this, а для вложенной функции вычисляет статическую глубину.
        /// </summary>
        /// <param name="exprs">Список параметров.</param>
        /// <param name="si">Список методов.</param>
        /// <param name="loc">Расположение вызова.</param>
        /// <param name="converted_type">Тип в котором мы находимся. null, если мы вне типа.</param>
        /// <param name="top_function">Функция в которой мы находимся.</param>
        /// <param name="allow_procedure">Может ли это быть вызов процедуры. false если вызов стоит в выражении или правой части опреатора присваивания.</param>
        /// <returns>Возвращает узел вызова метода.</returns>
		public expression_node create_full_function_call(expressions_list exprs, List<SymbolInfo> si, location loc,
            common_type_node converted_type, common_function_node top_function, bool allow_procedure, List<SyntaxTree.expression> syntax_nodes_parameters = null)
        {
            function_node fn = select_function(exprs, si, loc, syntax_nodes_parameters);
            
            //allow_procedure = true;
            if ((!allow_procedure) && (fn.return_value_type == null))
            {
                throw new SimpleSemanticError(loc, "FUNCTION_EXPECTED_PROCEDURE_{0}_MEET", fn.name);
            }

            if (fn.return_value_type is undefined_type)
                throw new SimpleSemanticError(loc, "RETURN_TYPE_UNDEFINED_{0}", fn.name);
            expression_node expr_node = null;
            switch (fn.semantic_node_type)
            {
                case semantic_node_type.basic_function_node:
                case semantic_node_type.common_namespace_function_node:
                    {
                        expr_node = create_simple_function_call(fn, loc, exprs.ToArray());
                        break;
                    }
                case semantic_node_type.common_method_node:
                case semantic_node_type.compiled_function_node:
                    {
                        SemanticTree.IClassMemberNode icmn = (SemanticTree.IClassMemberNode)fn;
                        if (icmn.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                        {
                            expr_node = create_simple_function_call(fn, loc, exprs.ToArray());
                            break;
                        }
                        //expression_node tn = new this_node(converted_type,loc);
                        base_function_call cmc = null;
                        switch (fn.semantic_node_type)
                        {
                            case semantic_node_type.common_method_node:
                                {
                                    //ssyy добавил
                                    if (((common_method_node)fn).is_constructor)
                                    {
                                        common_constructor_call ccc = new common_constructor_call((common_method_node)fn, loc);
                                        //(ssyy) По-видимому, здесь всегда можно присваивать false, так как при создании нового объекта мы сюда не заходим...
                                        ccc._new_obj_awaited = false;
                                        if (!syntax_tree_visitor.context.allow_inherited_ctor_call && !syntax_tree_visitor.context.can_call_inherited_ctor_call(statement_list_stack_first()))
                                        {
                                            if (syntax_tree_visitor.inherited_ident_processing)
                                                syntax_tree_visitor.AddError(new SimpleSemanticError(loc, "INHERITED_CONSTRUCTOR_CALL_MUST_BE_FIRST"));
                                            else
                                            {
                                                var lambdasInParameters = syntax_nodes_parameters?.Any(ex => ex is SyntaxTree.function_lambda_definition) ?? false;
                                                if (lambdasInParameters)
                                                    syntax_tree_visitor.AddError(new SimpleSemanticError(loc, "LAMBDAS_IN_CONSTRUCTOR_PARAMETERS_ARE_FORBIDDEN_IN_THIS_CONTEXT"));
                                                else syntax_tree_visitor.AddError(new SimpleSemanticError(loc, "CAN_NOT_CALL_CONSTRUCTOR_AS_PROCEDURE"));
                                            }
                                                
                                        }
                                        cmc = ccc;
                                    }
                                    else
                                    //\ssyy
                                    {
                                        if (fn is generic_method_instance_node)
                                        {
                                            common_method_node cmn = (fn as generic_method_instance_node).original_function as common_method_node;
                                            cmc = new common_method_call((common_method_node)fn, syntax_tree_visitor.GetCurrentObjectReference(cmn.cont_type.Scope, fn, loc), loc);
                                        }
                                        else
                                            cmc = new common_method_call((common_method_node)fn, syntax_tree_visitor.GetCurrentObjectReference(((common_method_node)fn).cont_type.Scope, fn, loc), loc);
                                        (cmc as common_method_call).virtual_call = !syntax_tree_visitor.inherited_ident_processing;
                                    }
                                    break;
                                }
                            case semantic_node_type.compiled_function_node:
                                {
                                    cmc = new compiled_function_call((compiled_function_node)fn, syntax_tree_visitor.GetCurrentObjectReference(((compiled_function_node)fn).cont_type.Scope, fn, loc), loc);
                                    (cmc as compiled_function_call).virtual_call = !syntax_tree_visitor.inherited_ident_processing;
                                    break;
                                }
                        }
                        cmc.parameters.AddRange(exprs);
                        expr_node = cmc;
                        break;
                    }
                case semantic_node_type.common_in_function_function_node:
                    {
                        common_in_function_function_node cffn = (common_in_function_function_node)fn;
                        common_in_function_function_call cffc = new common_in_function_function_call(cffn,
                            symtab.GetRelativeScopeDepth(cffn.function.scope, top_function.scope), loc);
                        cffc.parameters.AddRange(exprs);
                        expr_node = cffc;
                        break;
                    }
                //ssyy добавил
                case semantic_node_type.compiled_constructor_node:
                    {
                        compiled_constructor_node ccn = fn as compiled_constructor_node;
                        if (ccn == null)
                        {
                            throw new CompilerInternalError("compiled_constructor_node expected");
                        }
                        compiled_constructor_call ccc = new compiled_constructor_call(ccn, loc);
                        ccc.parameters.AddRange(exprs);
                        ccc._new_obj_awaited = false;
                        if (!syntax_tree_visitor.context.allow_inherited_ctor_call && !syntax_tree_visitor.context.can_call_inherited_ctor_call(statement_list_stack_first()))
                        {
                            throw new SimpleSemanticError(loc, "INHERITED_CONSTRUCTOR_CALL_MUST_BE_FIRST");
                        }
                        expr_node = ccc;
                        break;
                    }
                case semantic_node_type.indefinite_definition_node:
                    {
                        indefinite_function_call ifc = new indefinite_function_call(fn, loc);
                        ifc.parameters.AddRange(exprs);
                        expr_node = ifc;
                        break;
                    }
                //\ssyy
                default:
                    {
                        throw new NotSupportedError(loc);
                    }
            }
            return expr_node;
        }

        /// <summary>
        /// Проверяет, что оператор статический метод класса.
        /// </summary>
        /// <param name="fn">Функция-оператор.</param>
		public void check_operator(function_node fn)
		{
			switch (fn.semantic_node_type)
			{
				case semantic_node_type.common_in_function_function_node:
				{
					throw new CompilerInternalError("Operator can not be nested in function function.");
				}
				case semantic_node_type.common_method_node:
				{
					common_method_node cmn=(common_method_node)fn;
					if (cmn.polymorphic_state!=SemanticTree.polymorphic_state.ps_static)
					{
						throw new CompilerInternalError("Operator can not be non static method");
					}
					break;
				}
				case semantic_node_type.compiled_function_node:
				{
					compiled_function_node cfn=(compiled_function_node)fn;
					if (cfn.polymorphic_state!=SemanticTree.polymorphic_state.ps_static)
					{
						throw new CompilerInternalError("Operator can not be non static method. Included library is can be corrupted or library data extraction error.");
					}
					break;
				}
			}
		}


        /// <summary>
        /// Создает узел преобразования типа, если необходимо.
        /// </summary>
        /// <param name="en">Выражение исходного типа.</param>
        /// <param name="to">К какому типу нужно преобразовать.</param>
        /// <returns>Преобразованное выражение.</returns>
        public expression_node convert_type(expression_node en, type_node to)
        {
            return convert_type(en, to, en.location);
        }


        /// <summary>
        /// Проверяет можно ли неявно преобразовать. Если надо кидает исключение
        /// </summary>
        /// <param name="en">Выражение исходного типа.</param>
        /// <param name="to">К какому типу нужно преобразовать.</param>
        public void check_convert_type(expression_node en, type_node to, location loc)
        {
            if (en.type == to)
                return;
            possible_type_convertions pct = type_table.get_convertions(en.type, to);
            if (pct.second != null)
                throw new TwoTypeConversionsPossible(en, pct.first, pct.second);
            if (pct.first == null)
                throw new CanNotConvertTypes(en, en.type, to, loc);
        }

        public bool can_convert_type(expression_node en, type_node to)
        {
            if (en.type == to)
                return true;
            possible_type_convertions pct = type_table.get_convertions(en.type, to);
            if (pct.second != null)
                return false;
            if (pct.first == null)
                return false;
            return true;
        }

        public bool can_convert_type(type_node from, type_node to)
        {
            if (from == to)
                return true;
            possible_type_convertions pct = type_table.get_convertions(from, to);
            if (pct.second != null)
                return false;
            if (pct.first == null)
                return false;
            return true;
        }

        public void check_convert_type_with_inheritance(type_node from, type_node to, location loc)
        {
            if (from == to)
                return;
            possible_type_convertions pct = type_table.get_convertions(from, to);
            if (pct.second != null)
                throw new TwoTypeConversionsPossibleT(pct.first, pct.second, loc);
            if (pct.first == null /*&& !type_table.is_derived(from, to)*/)
                throw new CanNotConvertTypes(null, from, to, loc);
        } 

        /// <summary>
        /// Создает узел преобразования типа, если необходимо.
        /// </summary>
        /// <param name="en">Выражение исходного типа.</param>
        /// <param name="to">К какому типу нужно преобразовать.</param>
        /// <param name="loc">Явный контекст.</param>
        /// <returns>Преобразованное выражение.</returns>
		public expression_node convert_type(expression_node en,type_node to, location loc)
		{
			if (en.type==to)
			{
				return en;
			}

            if (to == SystemLibrary.SystemLibrary.system_delegate_type && en is typed_expression)
            {
                delegated_methods dm = (en as typed_expression).type as delegated_methods;
                if (dm.proper_methods.Count > 1)
                    AddError(loc, "AMBIGUOUS_DELEGATES");
            }

            if (en.type is compiled_type_node comptn1 && to is compiled_type_node comptn2) // SSM 5/05/20 - Rubantsev csfml - две dll - во второй функция с параметром из первой. Типы разные
            {
                if (comptn1.compiled_type == comptn2.compiled_type 
                    || comptn1.compiled_type.AssemblyQualifiedName == comptn2.compiled_type.AssemblyQualifiedName
                    ) // увы - тут типы Type разные и хеш-коды у них разные
                    return en;
            }


            //TODO: А если наследование?
            possible_type_convertions pct = type_table.get_convertions(en.type,to);
            if (pct.first == null && pct.second == null && en.type is delegated_methods && to.IsDelegate)
            {
                pct = type_table.get_convertions(to, en.type);
                if (pct.first != null)
                    return en;
            }
			if (pct.second!=null)
			{
                if (pct.second.to == null && en is typed_expression && !to.IsDelegate)
                {
                    syntax_tree_visitor.try_convert_typed_expression_to_function_call(ref en);
                    return en;
                }
                if (en is null_const_node && pct.first.convertion_method is basic_function_node)
                {
                    en.type = to;
                    return en;
                }
                else
                    AddError(new TwoTypeConversionsPossible(en,pct.first,pct.second));
			}

			if (pct.first==null)
			{
                if (to is delegated_methods && (to as delegated_methods).empty_param_method != null && (to as delegated_methods).empty_param_method.ret_type != null)
                {
                    return convert_type(en, (to as delegated_methods).empty_param_method.ret_type, loc);
                }
                en.location = loc;
                if (en.type is delegated_methods dm && dm.proper_methods[0].parameters.Count == 0 && dm.proper_methods[0].ret_type != null)
                {
                    if (to != SystemLibrary.SystemLibrary.object_type && to.IsDelegate)
                        AddError(new CanNotConvertTypes(en, dm.proper_methods[0].ret_type, to, loc)); // SSM 18/06/20 #2261
                    else
                        return en;
                }
                else if (en.type is undefined_type && en is base_function_call bfc)
                    throw new SimpleSemanticError(loc, "RETURN_TYPE_UNDEFINED_{0}", bfc.function.name);
                else if (en is enum_const_node && (en as enum_const_node).constant_value != 0)
                    return new int_const_node((en as enum_const_node).constant_value, loc);
                else AddError(new CanNotConvertTypes(en, en.type, to, loc));
			}

#if (DEBUG)
			check_operator(pct.first.convertion_method);
#endif
			type_node conv_type = en.type;
			expression_node expr = create_simple_function_call(pct.first.convertion_method, en.location, en);
            expr.conversion_type = conv_type;
            
            return expr;
		}

        public static function_node get_empty_conversion(type_node from_type, type_node to_type, bool with_compile_time_executor)
        {
            basic_function_node bfn = null;
            bfn = new basic_function_node(SemanticTree.basic_function_type.none, to_type, false);
            bfn.parameters.AddElement(new basic_parameter("expr", from_type, SemanticTree.parameter_type.value, bfn));
            if (with_compile_time_executor)
                bfn.compile_time_executor = SystemLibrary.SystemLibrary.delegated_empty_method;
            return bfn;
        }

        public expression_node explicit_convert_type(expression_node from, type_node to)
        {
            if (from is base_function_call bfc && bfc.function.name.StartsWith(LambdaHelper.lambdaPrefix))
            {
                AddError(from.location, "EXPLICIT_CASTS_FOR_LAMBDA_EXPRESSIONS_ARE_FORBIDDEN");
            }

            if (from.type == to)
            {
                return from;
            }
            
            
            possible_type_convertions ptc = type_table.get_convertions(from.type, to, false);
            if (ptc.second != null)
            {
                AddError(new TwoTypeConversionsPossible(from, ptc.first, ptc.second));
            }

            if (ptc.first != null)
            {
            	type_node conv_type = from.type;
                expression_node ret = create_simple_function_call(ptc.first.convertion_method, from.location, from);
                if ((ret is base_function_call))
                    (ret as base_function_call).IsExplicitConversion = true;
                ret.type = to;
                if (ptc.first.convertion_method is basic_function_node)
                    ret.conversion_type = conv_type;
                return ret;
            }
            
            if ((type_table.is_derived(from.type, to)) || (type_table.is_derived(to, from.type)) || from.type.IsInterface || to.IsInterface && !(from.type is delegated_methods))
            {
                if (from.type.IsSealed && to.IsInterface && !from.type.ImplementingInterfaces.Contains(to) ||
                    from.type.IsInterface && to.IsSealed && !to.ImplementingInterfaces.Contains(from.type))
                {
                    AddError(from.location, "CAN_NOT_EXPLICITLY_CONVERT_TYPE_{0}_TO_TYPE_{1}", from.type.PrintableName, to.PrintableName);
                }
            	type_node conv_type = from.type;
                expression_node expr = create_simple_function_call(SystemLibrary.SystemLibrary.obj_to_obj, from.location, from);
                //TODO: Переделать.
                ((base_function_call)expr).ret_type = to;
                ((base_function_call)expr).conversion_type = conv_type;
                ((base_function_call)expr).IsExplicitConversion = true;
                return expr;
            }
            AddError(from.location, "CAN_NOT_EXPLICITLY_CONVERT_TYPE_{0}_TO_TYPE_{1}", from.type.PrintableName, to.PrintableName);
            return null;
        }

		public expression_node convert_type_to_bool(expression_node en)
		{
			return (convert_type(en,SystemLibrary.SystemLibrary.bool_type));
		}

        public int convert_type_to_int_constant(expression_node en, out bool success)
        {
            success = true;
            if (en is int_const_node)
                return (en as int_const_node).constant_value;
            else
                if (en is byte_const_node)
                    return (en as byte_const_node).constant_value;
                else
                    if (en is sbyte_const_node)
                        return (en as sbyte_const_node).constant_value;
                    else if (en is short_const_node)
                        return (en as short_const_node).constant_value;
                    else if (en is ushort_const_node)
                        return (en as ushort_const_node).constant_value;
                    else if (en is uint_const_node)
                        return (int)(en as uint_const_node).constant_value;
                    else if (en is long_const_node)
                        return (int)(en as long_const_node).constant_value;
                    else if (en is ulong_const_node)
                        return (int)(en as ulong_const_node).constant_value;
            success = false;
            return -1;

        }

        private int arr_nums = 0;
        private string get_temp_arr_name()
        {
            arr_nums++;
            return ("$intarr"+arr_nums);
        }

        //проверки на константные параметры
        internal static bool check_for_constant_or_readonly(expression_node ex, out bool flag, out general_node_type gnd)
        {
            gnd = general_node_type.expression;
            flag = true;
        	switch (ex.semantic_node_type)
            {
                case semantic_node_type.array_const:
                case semantic_node_type.bool_const_node:
                case semantic_node_type.byte_const_node:
                case semantic_node_type.char_const_node:
                case semantic_node_type.class_constant_definition:
                case semantic_node_type.compiled_class_constant_definition:
                case semantic_node_type.namespace_constant_definition:
                case semantic_node_type.namespace_constant_reference:
            		flag = true;
                    return true;
                case semantic_node_type.class_field_reference:
                    class_field_reference cfr = ex as class_field_reference;
                    if (cfr.field.IsReadOnly) 
                    {
                    	flag = false;
                    	gnd = general_node_type.variable_node;
                    	return true;
                    }
                    return check_for_constant_or_readonly(cfr.obj,out flag,out gnd);
                case semantic_node_type.static_compiled_variable_reference:
                    static_compiled_variable_reference scvr = ex as static_compiled_variable_reference;
                   	if (scvr.var.IsReadOnly) 
                   	{
                   		flag = false;
                    	gnd = general_node_type.variable_node;
                   		return true;
                   	}
                   	else if (scvr.var.IsLiteral)
                   	{
                   		flag = true;
                   		gnd = general_node_type.variable_node;
                   		return true;
                   	}
                   	return false;
                case semantic_node_type.compiled_variable_reference:
                   	compiled_variable_reference cvr = ex as compiled_variable_reference;
                   	if (cvr.var.IsReadOnly) 
                   	{
                   		flag = false;
                    	gnd = general_node_type.variable_node;
                   		return true;
                   	}
                   	else if (cvr.var.IsLiteral)
                   	{
                   		flag = true;
                   		gnd = general_node_type.variable_node;
                   		return true;
                   	}
                   	if (cvr.obj != null)
                   	return check_for_constant_or_readonly(cvr.obj,out flag,out gnd);
                   	return false;
                case semantic_node_type.common_parameter_reference:
                    common_parameter_reference cpr = ex as common_parameter_reference;
                    flag = true;
                    return (cpr.par.concrete_parameter_type == concrete_parameter_type.cpt_const);
                case semantic_node_type.simple_array_indexing:
                    simple_array_indexing sai = ex as simple_array_indexing;
                    return check_for_constant_or_readonly(sai.simple_arr_expr,out flag,out gnd);
                default:
                    return false;
            }
        }

        //производит проверку для неопределённых типов
        internal bool possible_equal_types(type_node t1, type_node t2)
        {
            if (t1 == t2)
            {
                return true;
            }
            if (t1 is compiled_type_node comptn1 && t2 is compiled_type_node comptn2) // SSM 5/05/20 - Rubantsev csfml - две dll - во второй функция с параметром из первой. Типы разные
            {
                var tt = comptn1.compiled_type.Assembly == comptn2.compiled_type.Assembly;
                if (comptn1.compiled_type == comptn2.compiled_type 
                    /*|| (comptn1.compiled_type.Assembly == comptn2.compiled_type.Assembly && comptn1.compiled_type.FullName == comptn2.compiled_type.FullName)*/
                    || comptn1.compiled_type.AssemblyQualifiedName == comptn2.compiled_type.AssemblyQualifiedName
                    ) // увы - тут типы Type разные и хеш-коды у них разные
                    return true;
            } 
            if (!t1.depended_from_indefinite && !t2.depended_from_indefinite)
            {
                return false;
            }
            if (t1.semantic_node_type == semantic_node_type.indefinite_type ||
                t2.semantic_node_type == semantic_node_type.indefinite_type)
            {
                return true;
            }
            common_type_node ctn1 = t1 as common_type_node;
            common_type_node ctn2 = t2 as common_type_node;
            if (ctn1 == null || ctn2 == null ||
                ctn1.original_template == null || ctn2.original_template == null ||
                ctn1.original_template != ctn2.original_template)
            {
                return false;
            }
            List<type_node> types1 = ctn1.original_template.GetParamsList(ctn1);
            List<type_node> types2 = ctn2.original_template.GetParamsList(ctn2);
            int count = types1.Count;
            if (count != types2.Count)
            {
                return false;
            }
            bool eq = true;
            for (int i = 0; i < count; i++)
            {
                eq = possible_equal_types(types1[i], types2[i]);
                if (!eq)
                    break;
            }
            return eq;
        }

        /// <summary>
        /// Возвращает список преобразований типов для вызова метода.
        /// </summary>
        /// <param name="factparams">Список фактических параметров.</param>
        /// <param name="formalparams">Список формальных параметров.</param>
        /// <param name="is_alone_method_defined">Для единственного метода у которого типы параметров совпадают, но в качестве var параметра мы передаем константное значение мы можем сгенерировать более подробное сообщение об ошибке.</param>
        /// <returns>Список преобразований типов.</returns>
        internal possible_type_convertions_list get_conversions(expressions_list factparams,
			parameter_list formalparams,bool is_alone_method_defined, location locg, out Errors.Error error)
		{
			//TODO:Явно указывать capacity при создании.
            possible_type_convertions_list tc = new possible_type_convertions_list();
            error = null;
			possible_type_convertions ptc;

            if (factparams.Count>formalparams.Count)
            {
                if ( (formalparams.Count==0) || (!(formalparams[formalparams.Count-1].is_params)) )
                {
                    return null;
                }
            }

            type_node for_par_type = null;
            array_internal_interface aii=null;
            if (formalparams.Count > 0)
            {
                parameter pr=formalparams[formalparams.Count - 1];
                if (pr.is_params && 
                    //это для возможности вызова сразу с массивом[], типа просто не обращаем внимаение на params
                    !(factparams.Count == formalparams.Count && (factparams[factparams.Count - 1].type == formalparams[formalparams.Count - 1].type ||
                      (factparams[factparams.Count - 1].type is delegated_methods dm) && (dm.proper_methods[0].ret_type == formalparams[formalparams.Count - 1].type))
                     )
                   )
                {
                    //TODO: Добавить проверку на правильность.
                    aii = (array_internal_interface)
                        pr.type.get_internal_interface(internal_interface_kind.unsized_array_interface);
                    for_par_type = aii.element_type;

                    tc.snl = new statement_node_list();

                    location loc=null;
                    if (factparams.Count > 0) loc = factparams[factparams.Count-1].location;
                    //var_definition_node vdn=syntax_tree_visitor.context.add_var_definition_in_entry_scope(get_temp_arr_name(),loc);
                    var_definition_node vdn = null;
                    if (syntax_tree_visitor.context.converted_func_stack.size > 0)
                    {
                        common_function_node cfn = syntax_tree_visitor.context.converted_func_stack.first();
                        if (cfn.is_generic_function || !syntax_tree_visitor.context.has_nested_functions && syntax_tree_visitor.context.converted_func_stack.size == 1)
                        {
                            vdn = syntax_tree_visitor.context.add_var_definition(get_temp_arr_name(), loc);
                            
                        }
                        else if (syntax_tree_visitor.context.converted_type != null)
                        {
                            vdn = syntax_tree_visitor.context.add_field(get_temp_arr_name(), loc, pr.type, polymorphic_state.ps_static);
                            syntax_tree_visitor.context.converted_type.fields.RemoveElement(vdn as class_field);
                        }
                        else
                        {
                            vdn = syntax_tree_visitor.context.add_var_definition_in_entry_scope(get_temp_arr_name(), loc);
                            syntax_tree_visitor.context.converted_namespace.variables.RemoveElement(vdn as namespace_variable);
                        }
                    }
                    else if (syntax_tree_visitor.context.converted_type != null)
                    {
                        vdn = syntax_tree_visitor.context.add_field(get_temp_arr_name(), loc, pr.type, polymorphic_state.ps_static);
                        syntax_tree_visitor.context.converted_type.fields.RemoveElement(vdn as class_field);
                    }
                    else
                    {
                        vdn = syntax_tree_visitor.context.add_var_definition_in_entry_scope(get_temp_arr_name(), loc);
                        syntax_tree_visitor.context.converted_namespace.variables.RemoveElement(vdn as namespace_variable);
                    }
                    syntax_tree_visitor.context.close_var_definition_list(pr.type,null);
                    
                    expression_node fst=null;
                    /*switch (syntax_tree_visitor.context.converting_block())
                    {
                        case block_type.function_block:
                            {
                                fst=new local_variable_reference((local_variable)vdn,0,loc);
                                break;
                            }
                        case block_type.namespace_block:
                            {
                                fst=new namespace_variable_reference((namespace_variable)vdn,loc);
                                break;
                            }
                    }*/
                    switch (vdn.semantic_node_type)
                    {
                        case semantic_node_type.local_variable:
                            {
                                fst = new local_variable_reference((local_variable)vdn, 0, loc);
                                break;
                            }
                        case semantic_node_type.namespace_variable:
                            {
                                fst = new namespace_variable_reference((namespace_variable)vdn, loc);
                                break;
                            }
                        case semantic_node_type.local_block_variable:
                            {
                                fst = new local_block_variable_reference((local_block_variable)vdn, loc);
                                break;
                            }
                        case semantic_node_type.class_field:
                            {
                                class_field cf = vdn as class_field;
                                cf.polymorphic_state = polymorphic_state.ps_static;
                                fst = new static_class_field_reference(cf, loc);
                                break;
                            }
                        default:
                            throw new CompilerInternalError("Invalid node type");
                    }

                    tc.var_ref = fst;

                    int del=factparams.Count-formalparams.Count+1;
                    int_const_node icn=new int_const_node(del,loc);

                    expression_node bfc = create_simple_function_call(SystemLibrary.SystemLibrary.resize_func,
                        loc, fst, icn);
                    /*if (factparams.Count == 0)
                    {
                    	possible_type_convertions ptci=new possible_type_convertions();
						ptci.first=null;
						ptci.second=null;
						ptci.from=null;
                    	ptci.to = for_par_type;
						tc.AddElement(ptci);
                    }*/
                }
            }
			
			for(int i=0;i<factparams.Count;i++)
			{
                type_node formal_param_type = null;
                if ((for_par_type!=null)&&(i >= formalparams.Count-1))
                {
                    formal_param_type = for_par_type;
                }
                else
                {
                    formal_param_type = formalparams[i].type;
                }
                if (possible_equal_types(factparams[i].type, formal_param_type))
				{
					if ((i<formalparams.Count)&&(formalparams[i].parameter_type==SemanticTree.parameter_type.var))
					{
                        bool is_pascal_array_ref = false;
                        bool is_ok = false;
						if (factparams[i].is_addressed==false || factparams[i] is static_compiled_variable_reference scvr && scvr.var.compiled_field.IsInitOnly 
                            || factparams[i] is compiled_variable_reference cvr && cvr.var.compiled_field.IsInitOnly)
						{
                            if (factparams[i].semantic_node_type == semantic_node_type.common_method_call)
                            {
                                common_method_call cmc = (common_method_call)factparams[i];
                                internal_interface ii = cmc.obj.type.get_internal_interface(internal_interface_kind.bounded_array_interface);
                                if (ii != null)
                                {
                                    if (cmc.function_node.name == StringConstants.get_val_pascal_array_name)
                                    {
                                        bounded_array_interface bai = (bounded_array_interface)ii;
                                        class_field cf = bai.int_array;
                                        expression_node left = new class_field_reference(cf, cmc.obj, cmc.location);
                                        expression_node right = cmc.parameters[0];
                                        //right = convert_type(right, SystemLibrary.SystemLibrary.integer_type);
                                        right = convert_type(right, (ii as bounded_array_interface).ordinal_type_interface.elems_type);
                                        right = create_simple_function_call(SystemLibrary.SystemLibrary.int_sub, cmc.location, right,
                                            new int_const_node(bai.ordinal_type_interface.ordinal_type_to_int(bai.ordinal_type_interface.lower_value),cmc.location));
                                        factparams[i] = new simple_array_indexing(left, right, cmc.type, cmc.location);
                                        is_pascal_array_ref = true;
                                    }
                                }
                            }
                            if (!is_pascal_array_ref)
                            {
                                //Если мы попали сюда, то мы пытаемся передать по ссылке значение,
                                //которое не мохет быть передано по ссылке. Например, передать
                                //по ссылки из одной функции в другую константный параметр.
                                if (is_alone_method_defined)
                                {
                                    if (syntax_tree_visitor.assign_is_converting)
                                    {
                                        AddError(factparams[i].location, "CAN_NOT_ASSIGN_TO_LEFT_PART");
                                    }
                                    else
                                    {
                                        if (formalparams[i] is common_parameter && (formalparams[i] as common_parameter).concrete_parameter_type == concrete_parameter_type.cpt_const /*&& factparams[i] is common_parameter_reference*/)
                                        {
                                            is_ok = true;
                                        }
                                        else if (formalparams[i] is common_parameter && (formalparams[i] as common_parameter).concrete_parameter_type == concrete_parameter_type.cpt_const)
                                            ;
                                        //throw new ThisExpressionCanNotBePassedAsConstParameter(factparams[i].location);
                                        else if (is_string_getter(factparams[i]))
                                        {
                                            if (factparams[i] is compiled_function_call)
                                                factparams[i] = make_unmanaged_string_getter(factparams[i] as compiled_function_call);
                                            else
                                                factparams[i] = make_unmanaged_shortstring_getter(factparams[i] as base_function_call);
                                            is_ok = true;
                                        }

                                        else
                                            AddError(new ThisExpressionCanNotBePassedAsVarParameter(factparams[i]));
                                    }
                                }
                                else if (is_string_getter(factparams[i]))
                                {
                                    if (factparams[i] is compiled_function_call)
                                        factparams[i] = make_unmanaged_string_getter(factparams[i] as compiled_function_call);
                                    else
                                        factparams[i] = make_unmanaged_shortstring_getter(factparams[i] as base_function_call);
                                    is_ok = true;
                                }

                                if (!is_ok)
                                    return null;
                            }
						}
					}
					//Разобраться: может лучше добавлять null к списку.
					possible_type_convertions ptci=new possible_type_convertions();
					ptci.first=null;
					ptci.second=null;
					ptci.from=factparams[i].type;
                    ptci.to = formal_param_type;
					tc.AddElement(ptci);

                    if ((for_par_type != null) && (i >= formalparams.Count - 1))
                    {
                        expression_node to = new simple_array_indexing(tc.var_ref,//factparams[formalparams.Count - 1],
                            new int_const_node(i - formalparams.Count + 1, factparams[i].location), aii.element_type, factparams[i].location);
                        expression_node from = factparams[i];
                        statement_node stat = syntax_tree_visitor.find_operator(StringConstants.assign_name, to, from, factparams[i].location);
                        tc.snl.AddElement(stat);
                    }

					continue;
				}
				if ((i<formalparams.Count)&&(formalparams[i].parameter_type==SemanticTree.parameter_type.var))
				{
					return null;
				}
				else
				{
                    if (factparams[i].type is delegated_methods dm2
                                && dm2.proper_methods[0].function.is_generic_function
                                && !syntax_tree_visitor.context.WithSection)
                    {
                        var name = dm2.proper_methods[0].function.name;
                        error = new GenericFunctionCannotBeAnArgument(name, locg);
                        return null; // фактический параметр - имя неинстанцированной generic-функции!
                    }

                    ptc = type_table.get_convertions(factparams[i].type, formal_param_type);

                    if (factparams[i] is null_const_node && !type_table.is_with_nil_allowed(formal_param_type) && !formal_param_type.IsPointer) // SSM 01.07.19 - nil не может быть преобразована за счёт вызова функции
                    {
                        ptc.first = null;
                    }

                    if (ptc.first==null)
					{
                        if (type_table.is_derived(formal_param_type, factparams[i].type))
						{
							possible_type_convertions ptci=new possible_type_convertions();
							ptci.first=null;
							ptci.second=null;
							ptci.from=factparams[i].type;
                            ptci.to = formal_param_type;
							tc.AddElement(ptci);

                            if ((for_par_type != null) && (i >= formalparams.Count - 1))
                            {
                                expression_node to = new simple_array_indexing(tc.var_ref,//factparams[formalparams.Count - 1],
                                    new int_const_node(i - formalparams.Count + 1, factparams[i].location), aii.element_type, factparams[i].location);
                                expression_node from = factparams[i];
                                statement_node stat = syntax_tree_visitor.find_operator(StringConstants.assign_name, to, from, factparams[i].location);
                                tc.snl.AddElement(stat);
                            }

						}
						else
						{
                            // SSM 20/01/25 - Tuple<int,int> -> Tuple<double,double>
                            // А если уже один такой conv есть? Как сливать несколько в один???
                            if (formal_param_type.original_generic != null 
                                && formal_param_type.original_generic.BaseFullName.StartsWith("System.Tuple`")
                                && factparams[i].type.original_generic != null
                                && factparams[i].type.original_generic.BaseFullName.StartsWith("System.Tuple`")
                                && formal_param_type.instance_params.Count == factparams[i].type.instance_params.Count
                                )
                            {
                                syntax_tree_visitor.contextChanger.SaveContextAndUpToGlobalLevel();
                                var sl = new statement_list();
                                var rettype = new semantic_type_node(formal_param_type);

                                var el = new expression_list();
                                for (int ii = 1; ii <= formal_param_type.instance_params.Count; ii++)
                                    el.Add(new dot_node(new ident("x"), new ident("Item" + ii.ToString(),
                                        factparams[i].location)));
                                        //parameters[ii-1].location)));
                                SyntaxTree.expression ex = new SyntaxTree.new_expr(rettype,el);
                                sl.Add(new SyntaxTree.assign(StringConstants.result_var_name, ex));
                                // Определим функцию преобразования на внешнем уровне
                                var fun = BuildSimpleFunctionOneParameter("_conv" + UniqueString(), "x",
                                    new SyntaxTree.semantic_type_node(factparams[i].type),
                                    rettype,
                                    sl
                                    );
                                // Теперь ее надо обойти
                                syntax_tree_visitor.ProcessNode(fun);
                                // Теперь до нее доберемся
                                var fn = syntax_tree_visitor.context.last_created_function.sym_info as function_node;
                                syntax_tree_visitor.contextChanger.RestoreCurrentContext();
                                possible_type_convertions ptci = new possible_type_convertions();
                                ptci.first = new type_conversion(fn);
                                ptci.second = null;
                                ptci.from = factparams[i].type;
                                ptci.to = formal_param_type;
                                tc.AddElement(ptci);
                                return tc;
                            }

                            //issue #2161 - SSM 12.03.2020
                            //issue #348
                            if (formal_param_type == SystemLibrary.SystemLibrary.object_type
                                && factparams[i].type is delegated_methods dm1
                                && !dm1.proper_methods[0].function.is_generic_function
                                ||
                                formal_param_type.IsDelegate 
                                && factparams[i].type is delegated_methods
                               ) // SSM 23.06.2025 issue #3290
                            {
                                possible_type_convertions ptci = new possible_type_convertions();
                                ptci.first = null;
                                ptci.second = null;
                                if (formal_param_type.instance_params != null && formal_param_type.instance_params.Count > 0)
                                {
                                    var fn = (factparams[i].type as delegated_methods).proper_methods[0].simple_function_node;
                                    if (fn.is_generic_function)
                                    {
                                        var fn_instance = fn.get_instance(formal_param_type.instance_params, true, factparams[i].location);
                                        delegated_methods dm = new delegated_methods();
                                        if (fn is common_namespace_function_node)
                                            dm.proper_methods.AddElement(new common_namespace_function_call(fn_instance as common_namespace_function_node, factparams[i].location));
                                        else if (fn is common_method_node cmn && cmn.IsStatic)
                                            dm.proper_methods.AddElement(new common_static_method_call(fn_instance as common_method_node, factparams[i].location));
                                        else if (fn is common_method_node cmn2)
                                            dm.proper_methods.AddElement(new common_method_call(fn_instance as common_method_node, ((factparams[i].type as delegated_methods).proper_methods[0] as common_method_call).obj, factparams[i].location));
                                        factparams[i].type = dm;
                                    }
                                    else if (!is_alone_method_defined)
                                        return null;
                                }
                                else if (!is_alone_method_defined && formal_param_type.IsDelegate)
                                    return null;
                                ptci.from = factparams[i].type;
                                ptci.to = formal_param_type;
                                if (formal_param_type.IsDelegate)
                                {
                                    var ttc = type_table.get_convertions(formal_param_type, factparams[i].type);
                                    if (ttc.first == null)
                                    {
                                        if (is_alone_method_defined) // если мы сюда попали, то ошибка более явная
                                        {
                                            error = new CanNotConvertTypes(factparams[i], factparams[i].type, formal_param_type, locg);
                                            return null;
                                        }
                                        else
                                            return null;
                                    }
                                }
                                
                                tc.AddElement(ptci);
                                factparams[i] = syntax_tree_visitor.CreateDelegateCall((factparams[i].type as delegated_methods).proper_methods[0]);
                                //return tc;
                            }
                            else if (is_alone_method_defined) // если мы сюда попали, то ошибка более явная
                            {
                                error = new CanNotConvertTypes(factparams[i], factparams[i].type, formal_param_type, locg);
                                return null;
                            }
                            else
                                return null;
							//return null;
                            
						}
					}
					else
					{
						tc.AddElement(ptc);
                        if ((for_par_type != null) && (i >= formalparams.Count - 1))
                        {
                            expression_node to = new simple_array_indexing(tc.var_ref,//factparams[formalparams.Count - 1],
                                new int_const_node(i - formalparams.Count + 1, factparams[i].location), aii.element_type, factparams[i].location);
                            expression_node from = create_simple_function_call(ptc.first.convertion_method,
                                factparams[i].location, factparams[i]);
                            statement_node stat = syntax_tree_visitor.find_operator(StringConstants.assign_name, to, from, factparams[i].location);
                            tc.snl.AddElement(stat);
                        }
					}
				}
			}
            if (tc.snl != null)
            foreach (statement_node sn in tc.snl)
                sn.location = null;
			return tc;
		}

        private simple_array_indexing make_unmanaged_string_getter(compiled_function_call cfc)
        {
           return new simple_array_indexing(cfc.obj, cfc.parameters[0], SystemLibrary.SystemLibrary.char_type, cfc.location);
        }

        private simple_array_indexing make_unmanaged_shortstring_getter(base_function_call cfc)
        {
            return new simple_array_indexing(cfc.parameters[0], cfc.parameters[1], SystemLibrary.SystemLibrary.char_type, cfc.location);
        }

        private bool is_string_getter(expression_node en)
        {
            if (en is common_namespace_function_call && (en as common_namespace_function_call).function_node == SystemLibrary.SystemLibInitializer.GetCharInShortStringProcedure.sym_info)
            {
                return true;   
            }
            else if (en is compiled_static_method_call && (en as compiled_static_method_call).function_node == SystemLibrary.SystemLibInitializer.GetCharInShortStringProcedure.sym_info)
            {
                return true;
            }
            else if (en is compiled_function_call)
            {
                if ((en as compiled_function_call).function_node.cont_type != SystemLibrary.SystemLibrary.string_type)
                    return false;
                if ((en as compiled_function_call).function_node.name.StartsWith("get_"))
                    return true;
            }
            return false;
        }

		private enum method_compare {less_method,greater_method,not_comparable_methods};

		private enum type_conversion_compare {less_type_conversion,greater_type_conversion,equal_type_conversions};
		
		private type_conversion_compare compare_type_conversions(possible_type_convertions left,possible_type_convertions right)
		{
			if ((left.first==null)&&(right.first!=null))
			{
                if (right.from.IsDelegate && right.to.IsDelegate && left.to == SystemLibrary.SystemLibrary.object_type)
                    return type_conversion_compare.less_type_conversion;//right.first != null из-за структурной эквивалентности процедурных типов и в силу того, что для каждого процедурного типа создается свой класс и и создается пустой метод преобразования типов
                return type_conversion_compare.greater_type_conversion;
			}
			if ((left.first!=null)&&(right.first==null))
			{
				return type_conversion_compare.less_type_conversion;
			}
            if ((left.first != null) && (right.first != null))
            {
                convert_types_function_node cleft = left.first.convertion_method as convert_types_function_node;
                convert_types_function_node cright = right.first.convertion_method as convert_types_function_node;

                if ((cleft != null) && (cright != null))
                {
                    
                    if ((cleft.Good) && (!cright.Good))
                    {
                        return type_conversion_compare.greater_type_conversion;
                    }

                    if ((!cleft.Good) && (cright.Good))
                    {
                        return type_conversion_compare.less_type_conversion;
                    }
                    
                }
            }
			/*if ((left.first==null)&&(right.first==null))
			{
				return type_conversion_compare.equal_type_conversions;
			}*/
			//type_compare tc=type_table.compare_types(left.first.to,right.first.to);
			type_compare tc=type_table.compare_types(left.to,right.to);
			
			if (tc==type_compare.greater_type)
			{
				if (left.to.type_special_kind == type_special_kind.diap_type && left.to is common_type_node && right.to.type_special_kind == type_special_kind.diap_type && right.to is common_type_node)
				{
					tc = type_table.compare_types(left.to.base_type,right.to.base_type);
					if (tc == type_compare.greater_type)
						return type_conversion_compare.less_type_conversion;
				}
				else
				return type_conversion_compare.less_type_conversion;
				//return type_conversion_compare.greater_type_conversion;
			}
			if (tc==type_compare.less_type)
			{
				if (left.to.type_special_kind == type_special_kind.diap_type && left.to is common_type_node && right.to.type_special_kind == type_special_kind.diap_type && right.to is common_type_node)
				{
					tc = type_table.compare_types(left.to.base_type,right.to.base_type);
					if (tc == type_compare.less_type)
						return type_conversion_compare.greater_type_conversion;
				}
				else
				return type_conversion_compare.greater_type_conversion;
				//return type_conversion_compare.less_type_conversion;
			}

            internal_interface left_del = left.to.get_internal_interface(internal_interface_kind.delegate_interface);
            internal_interface right_del = right.to.get_internal_interface(internal_interface_kind.delegate_interface);
            if ((left_del == null) && (right_del == null))
            {
                return type_conversion_compare.equal_type_conversions;
            }

            if ((left_del == null) && (right_del != null))
            {
                return type_conversion_compare.less_type_conversion;
            }

            if ((left_del != null) && (right_del == null))
            {
                return type_conversion_compare.greater_type_conversion;
            }

			return type_conversion_compare.equal_type_conversions;
		}

        public enum MoreSpecific { Left, Right, None}

        // SSM 04/07/2021
        public MoreSpecific compare_more_specific(function_node left_func, function_node right_func, possible_type_convertions_list left, possible_type_convertions_list right)
        {
            bool LeftIsMoreSpecific = false;
            bool RightIsMoreSpecific = false;
            var left_original = left_func.original_function;
            var right_original = right_func.original_function;
            if (left_original != null && right_original != null)
            {
                for (var i = 0; i < left_original.parameters.Count; i++)
                {
                    if (left_original.parameters[i].is_params || i < right_original.parameters.Count && right_original.parameters[i].is_params || i >= right_original.parameters.Count)
                    {
                        return MoreSpecific.None;
                    }
                    var ltt = left_original.parameters[i].type;
                    var rtt = right_original.parameters[i].type;
                    if (ltt.is_generic_parameter && !rtt.is_generic_parameter)
                    {
                        RightIsMoreSpecific = true;
                        if (left != null && right != null && left.Count > 0 && right.Count > 0)
                        {
                            type_conversion_compare tcc = compare_type_conversions(left[0], right[0]);
                            if (tcc == type_conversion_compare.greater_type_conversion && right[0].first != null && right[0].first.convertion_method != null && !(right[0].first.convertion_method is basic_function_node))
                                RightIsMoreSpecific = false;//ignore user defined implicit conversions
                        }
                        continue;
                    }
                    if (rtt.is_generic_parameter && !ltt.is_generic_parameter)
                    {
                        LeftIsMoreSpecific = true;
                        if (left != null && right != null && left.Count > 0 && right.Count > 0)
                        {
                            type_conversion_compare tcc = compare_type_conversions(left[0], right[0]);
                            if (tcc == type_conversion_compare.less_type_conversion && left[0].first != null && left[0].first.convertion_method != null && !(left[0].first.convertion_method is basic_function_node))
                                LeftIsMoreSpecific = false;//ignore user defined implicit conversions
                        }
                        continue;
                    }
                    var lt = ltt as generic_instance_type_node;
                    var rt = rtt as generic_instance_type_node;
                    if (lt == null || rt == null)
                        continue;
                    if (lt.original_generic == rt.original_generic)
                    {
                        var lins = lt.instance_params;
                        var rins = rt.instance_params;
                        for (int j = 0; j < rins.Count; j++)
                        {
                            if (lins[j].is_generic_parameter && !rins[j].is_generic_parameter)
                                RightIsMoreSpecific = true;
                            else if (rins[j].is_generic_parameter && !lins[j].is_generic_parameter)
                                LeftIsMoreSpecific = true;
                        }
                    }
                }
            }

            if (RightIsMoreSpecific && !LeftIsMoreSpecific)
                return MoreSpecific.Right;
            if (LeftIsMoreSpecific && !RightIsMoreSpecific)
                return MoreSpecific.Left;
            return MoreSpecific.None;

        }

        private method_compare compare_methods(function_node left_func, function_node right_func,
            possible_type_convertions_list left,possible_type_convertions_list right)
		{
            // Нет распознавания глубоко вложенных типов на IsMoreSpecific - только на глубину 1 или 2
            var moreSpec = compare_more_specific(left_func, right_func, left, right);
            if (moreSpec == MoreSpecific.Left)
                return method_compare.greater_method;
            if (moreSpec == MoreSpecific.Right)
                return method_compare.less_method;
                
            // а иначе пока ничего не возвращать

            function_compare fc = function_node.compare_functions(left_func, right_func);
            if (fc == function_compare.less)
            {
                return method_compare.less_method;
            }
            if (fc == function_compare.greater)
            {
                return method_compare.greater_method;
            }

			bool left_greater=false;
			bool right_greater=false;

			for(int i=0;i<left.Count;i++)
			{
                if (i >= right.Count)
                    break;
				type_conversion_compare tcc=compare_type_conversions(left[i],right[i]);
				if (tcc==type_conversion_compare.less_type_conversion)
				{
					right_greater=true;
				}
				if (tcc==type_conversion_compare.greater_type_conversion)
				{
					left_greater=true;
				}
				if ((left_greater)&&(right_greater))
				{
					return method_compare.not_comparable_methods;
				}
			}
			if ((left_greater)&&(!right_greater))
			{
				return method_compare.greater_method;
			}
			if ((!left_greater)&&(right_greater))
			{
				return method_compare.less_method;
			}
            if ((left_greater) && (right_greater))
            {
                return method_compare.not_comparable_methods;
            }

            if ((left.snl != null) && (right.snl == null))
            {
                return method_compare.less_method;
            }
            if ((left.snl == null) && (right.snl != null))
            {
                return method_compare.greater_method;
            }
            if (!left_func.is_generic_function_instance && right_func.is_generic_function_instance) // SSM 29.06.19 Это неверно для Run<int>(Func<int>) против Run(Func<Task>)
            {
                if (left_func is basic_function_node)
                    return method_compare.less_method;
                else
                {
                    if (left.Count > 0 && right.Count > 0 && left[0].from is delegated_methods && right[0].from is delegated_methods && right[0].to.is_generic_type_instance
                        && (left[0].from as delegated_methods).proper_methods[0].ret_type is lambda_any_type_node && (right[0].from as delegated_methods).proper_methods[0].ret_type is lambda_any_type_node)
                    {
                        var llist = right_func.get_generic_params_list();
                        var rlist = right[0].to.instance_params;
                        
                        if (llist.Count == rlist.Count)
                        {
                            for (int i=0; i<llist.Count; i++)
                                if (llist[i] != rlist[i])
                                    return method_compare.greater_method;
                            return method_compare.less_method;
                        }
                        else
                            return method_compare.greater_method;
                    }
                    else
                        return method_compare.greater_method;
                }
            }
            if (left_func.is_generic_function_instance && !right_func.is_generic_function_instance)
            {
                if (right_func is basic_function_node)
                    return method_compare.greater_method;
                else
                    return method_compare.less_method;
            }
            if (left_func.is_generic_function_instance && right_func.is_generic_function_instance)
            {
            	if (left_func.generic_parameters_count > right_func.generic_parameters_count)
            		return method_compare.greater_method;
            	else if (left_func.generic_parameters_count < right_func.generic_parameters_count)
            		return method_compare.less_method;
                else // SSM 8/8/15 Два метода одинаковы по всем параметрам, но у одного - больше instance_params, т.е. он менее специализирован
                if (left_func.parameters.Count == right_func.parameters.Count)
                {
                    bool eq = true;
                    for (var i=0; i<left_func.parameters.Count; i++)
                        if (left_func.parameters[i].type != right_func.parameters[i].type)
                        {
                            eq = false;
                            break;
                        }
                    
                    if (eq)
                    {
                        // SSM 03/07/2021 - в связи с EachCount с GroupBy и без

                        /*bool LeftIsMoreSpecific = false;
                        bool RightIsMoreSpecific = false;
                        var left_original = left_func.original_function;
                        var right_original = right_func.original_function;
                        if (left_original != null && right_original != null)
                        {
                            for (var i = 0; i < left_original.parameters.Count; i++)
                            {
                                var ltt = left_original.parameters[i].type;
                                var rtt = right_original.parameters[i].type;
                                if (ltt.is_generic_parameter && !rtt.is_generic_parameter)
                                {
                                    RightIsMoreSpecific = true;
                                    continue;
                                }
                                if (rtt.is_generic_parameter && !ltt.is_generic_parameter)
                                {
                                    LeftIsMoreSpecific = true;
                                    continue;
                                }
                                var lt = ltt as generic_instance_type_node;
                                var rt = rtt as generic_instance_type_node;
                                if (lt == null || rt == null)
                                    continue;
                                if (lt.original_generic == rt.original_generic)
                                {
                                    var lins = lt.instance_params;
                                    var rins = rt.instance_params;
                                    for (int j = 0; j < rins.Count; j++)
                                    {
                                        if (lins[j].is_generic_parameter && !rins[j].is_generic_parameter)
                                            RightIsMoreSpecific = true;
                                        else if (rins[j].is_generic_parameter && !lins[j].is_generic_parameter)
                                            LeftIsMoreSpecific = true;
                                    }
                                }
                            }
                        }
                        if (RightIsMoreSpecific && !LeftIsMoreSpecific)
                            return method_compare.less_method;
                        if (LeftIsMoreSpecific && !RightIsMoreSpecific)
                            return method_compare.greater_method;*/

                        // end SSM 03/07/2021

                        // Это не всегда правильно срабатывает
                        var lc = left_func.get_generic_params_list().Count;
                        var rc = right_func.get_generic_params_list().Count;
                        if (lc < rc)
                            return method_compare.greater_method;
                        if (lc > rc)
                            return method_compare.less_method;
                        // Тут совпадать они не могут, но если совпадают - пусть уж будут несравнимыми
                    }
                }
            }
            if (left_func.return_value_type != null && right_func.return_value_type != null && function_eq_params(left_func, right_func, true))
            {
                var tc = type_table.compare_types(left_func.return_value_type, right_func.return_value_type);
                if (tc == type_compare.less_type)
                    return method_compare.greater_method;
                if (tc == type_compare.greater_type)
                    return method_compare.less_method;
            }
                
            return method_compare.not_comparable_methods;
		}

        internal static bool eq_type_nodes(type_node tn1, type_node tn2, bool strong)
        {
            if (tn1 == tn2)
                return true;
            if (tn1 == null || tn2 == null)
            {
                return false;
            }
			if (tn1 is lambda_any_type_node || tn2 is lambda_any_type_node) //lroman// Считаем, что тип lambda_any_type_node равен всем типам. Это используется при выводе параметров лямбды
                return true;
            if (tn1.generic_function_container != null && tn2.generic_function_container != null)
            {
                if (strong && tn1.generic_function_container != tn2.generic_function_container)
                {
                    if (tn1.is_generic_parameter && tn2.is_generic_parameter)
                        return tn1.generic_param_index == tn2.generic_param_index;
                    return false;
                }
                return (tn1.generic_param_index == tn2.generic_param_index);
            }
            if (tn1.type_special_kind != tn2.type_special_kind)
            {
                return false;
            }
            switch (tn1.type_special_kind)
            {
                case type_special_kind.array_kind:
                case type_special_kind.set_type:
                case type_special_kind.typed_file:
                    if (tn1.type_special_kind == type_special_kind.array_kind)
                    {
                        if (tn1 is compiled_type_node && tn2 is compiled_type_node && (tn1 as compiled_type_node).rank != (tn2 as compiled_type_node).rank)
                            return false;
                        if (tn1 is common_type_node && tn2 is common_type_node && (tn1 as common_type_node).rank != (tn2 as common_type_node).rank)
                            return false;
                    }
                    return eq_type_nodes(tn1.element_type, tn2.element_type, strong);
            }
            ref_type_node rtn1 = tn1 as ref_type_node;
            if (rtn1 != null)
            {
                ref_type_node rtn2 = tn2 as ref_type_node;
                if (rtn2 == null)
                {
                    return false;
                }
                return eq_type_nodes(rtn1.pointed_type, rtn2.pointed_type, strong);
            }
            if (tn1.is_generic_type_definition && tn2.is_generic_type_instance ||
                tn2.is_generic_type_definition && tn1.is_generic_type_instance)
            {
                type_node tdef, tinst;
                if (tn1.is_generic_type_definition)
                {
                    tdef = tn1;
                    tinst = tn2;
                }
                else
                {
                    tdef = tn2;
                    tinst = tn1;
                }
                if (tinst.original_generic != tdef)
                {
                    return false;
                }
                
                if (tdef is common_type_node)
                {
                    common_type_node ctdef = tdef as common_type_node;
                    int count = ctdef.generic_params.Count;
                    for (int i = 0; i < count; ++i)
                    {
                        if (ctdef.generic_params[i] != tinst.instance_params[i])
                        {
                            return false;
                        }
                    }
                }
                else if (tdef is compiled_type_node)
                {
                    compiled_type_node ctdef = tdef as compiled_type_node;
                    int count = ctdef.generic_params.Count;
                    for (int i = 0; i < count; ++i)
                    {
                        if (ctdef.generic_params[i] != tinst.instance_params[i])
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            if (tn1.is_generic_type_instance)
            {
                if (!tn2.is_generic_type_instance)
                {
                    return false;
                }
                if (tn1.original_generic != tn2.original_generic)
                {
                    return false;
                }
                int count = tn1.instance_params.Count;
                for (int i = 0; i < count; ++i)
                {
                    if (!eq_type_nodes(tn1.instance_params[i], tn2.instance_params[i], strong))
                    {
                        return false;
                    }
                }
                return true;
            }
            if (tn1.get_internal_interface(internal_interface_kind.delegate_interface) != null)
            {
                if (strong)
                    return tn1 == tn2;
                delegate_internal_interface d1 = tn1.get_internal_interface(internal_interface_kind.delegate_interface) as delegate_internal_interface;
                delegate_internal_interface d2 = tn2.get_internal_interface(internal_interface_kind.delegate_interface) as delegate_internal_interface;
                if (d2 != null)
                    return function_eq_params_and_result(d1.invoke_method, d2.invoke_method);
            }
            return false;
        }

        internal static bool eq_type_nodes(type_node tn1, type_node tn2)
        {
            return eq_type_nodes(tn1, tn2, false);
        }

        public static bool type_or_base_type_implements_interface(type_node tn, type_node interf)
        {
            do {
                if (tn.ImplementingInterfaces.Contains(interf))
                    return true;
                tn = tn.base_type;
            } while (tn != null);
            return false;
        }

        //Этот метод сверяет не только параметры, но и возвращаемое значение
        public static bool function_eq_params_and_result(function_node left, function_node right, bool weak = false)
        {
            if (!function_eq_params(left, right, weak))
            {
                return false;
            }
            else
            {
                if (left.return_value_type != null && right.return_value_type != null && left.return_value_type.is_generic_parameter && right.return_value_type.is_generic_parameter)
                {
                    //if (string.Compare(left.return_value_type.name, right.return_value_type.name, true) != 0)
                    //    return false;
                    return eq_type_nodes(left.return_value_type, right.return_value_type, false);
                }
                    
                return eq_type_nodes(left.return_value_type, right.return_value_type, weak);
            }
        }

        //Сравниваем типы-параметры generic-функций
        public static bool function_eq_generic_params(function_node left, function_node right)
        {
            List<type_node> left_type_params = left.get_generic_params_list();
            List<type_node> right_type_params = right.get_generic_params_list();
            if (left_type_params == null && right_type_params == null)
            {
                return true;
            }
            if (left_type_params == null || right_type_params == null)
            {
                return false;
            }
            if (left_type_params.Count != right_type_params.Count)
            {
                return false;
            }
            /*for (int i = 0; i < left_type_params.Count; i++)
            {
                if (string.Compare(left_type_params[i].name, right_type_params[i].name, true) != 0)
                {
                    return false;
                }
            }*/
            return true;
        }

        public static bool function_eq_params(function_node left, function_node right)
        {
            return function_eq_params(left, right, true);
        }

        public static bool function_eq_params(function_node left, function_node right, bool compare_parameter_types)
		{
            if (compare_parameter_types)
            {
                if (!function_eq_generic_params(left, right))
                {
                    return false;
                }
            }
            else
            {
                if (left.generic_parameters_count != right.generic_parameters_count)
                {
                    return false;
                }
            }
            if (left.parameters.Count != right.parameters.Count)
            {
                return false;
            }

            for (int i = 0; i < left.parameters.Count; i++)
            {
                if (!eq_type_nodes(left.parameters[i].type,right.parameters[i].type) ||
                    compare_parameter_types && left.parameters[i].parameter_type != right.parameters[i].parameter_type)
                {
                    return false;
                }
            }

			return true;
		}

		private void check_single_possible_convertion(ILocation loc,possible_type_convertions_list ptcal)
		{
			foreach(possible_type_convertions ptc in ptcal)
			{
				if (ptc.second!=null)
				{
                    if (ptc.first.from is null_type_node || ptc.second.to == null || ptc.second.from is null_type_node || ptc.second.from.is_generic_parameter)
                        continue; // SSM 9/12/20 fix 2363
                    if (ptc.first.from == ptc.second.from && ptc.first.to == ptc.second.to) // SSM fix 03/08/24 - непонятно, как до этого могло дойти
                        continue;
                    AddError(new PossibleTwoTypeConversionsInFunctionCall(loc,ptc.first,ptc.second));
				}
				
			}
		}

		private void convert_function_call_expressions(function_node fn,expressions_list exprs,
			possible_type_convertions_list ptcal)
		{
            var needToConvertParamsToFunctionCall = true; // SSM #2079 29/06/20
            var lastIsParams = fn.parameters.Count > 0 && fn.parameters[fn.parameters.Count - 1].is_params;
            if (lastIsParams && fn.parameters[fn.parameters.Count - 1].type.element_type.IsDelegate)
                needToConvertParamsToFunctionCall = false; // параметры params - преобразовывать ли в вызовы


            for (int i = 0; i < exprs.Count; i++) 
            {
                // надо отдельно расшифровывать params и в них тоже не преобразовывать если там делегаты
                // Если функция возвращает функцию, то это к сожалению не будет работать
                if (i <= fn.parameters.Count - 1 && fn.parameters[i].type.IsDelegate || lastIsParams && i >= fn.parameters.Count - 1 && !needToConvertParamsToFunctionCall)
                {
                    // ничего не делать
                }
                else
                {
                    var ex = exprs[i];
                    syntax_tree_visitor.try_convert_typed_expression_to_function_call(ref ex);
                    exprs[i] = ex;
                }
            } // SSM end 29/06/20

            for (int i=0;i<exprs.Count;i++)
			{
                if ((ptcal.snl != null) && (i >= fn.parameters.Count - 1))
                {
                    //statement_list_stack.top().statements.AddRange(ptcal.snl);
                    statements_expression_node sre = new statements_expression_node(ptcal.snl, ptcal.var_ref, ptcal.var_ref.location);
                    List<expression_node> args = new List<expression_node>();
                    for (int j = fn.parameters.Count - 1; j < exprs.Count; j++)
                        args.Add(convert_type(exprs[j], fn.parameters[fn.parameters.Count - 1].type.element_type));
                    exprs.remove_range(fn.parameters.Count - 1, exprs.Count - fn.parameters.Count);
                    //exprs.AddElement(ptcal.var_ref);
                    //exprs[i] = ptcal.var_ref;
                    exprs[i] = new array_initializer(args, ptcal.var_ref.location);
                    exprs[i].type = fn.parameters[fn.parameters.Count - 1].type;
                    break;
                }
                if (ptcal.Count <= i)
                {
                    continue;
                }
                if ((ptcal[i] == null) || (ptcal[i].first == null) || (exprs[i] is null_const_node && exprs[i].conversion_type == null))
                {
                    continue;
                }
                if (ptcal[i].second != null && ptcal[i].second.to == null)
                    continue;
                expression_node[] temp_arr = new expression_node[1];
                temp_arr[0] = exprs[i];
                if (ptcal[i].first.convertion_method is compiled_constructor_node)
                    exprs[i] = create_compiled_construcor_call(ptcal[i].first.convertion_method as compiled_constructor_node, get_location(exprs[i]), temp_arr);
                else
                    exprs[i] = create_simple_function_call(ptcal[i].first.convertion_method, get_location(exprs[i]), temp_arr);
			}
            //TODO: Можно сделать параметры по умолчанию для откомпилированных функций.
            if ((exprs.Count < fn.parameters.Count) && (fn.node_kind == SemanticTree.node_kind.common))
            {
                if (!(fn is common_namespace_function_node && (fn as common_namespace_function_node).ConnectedToType != null) && fn.parameters != null && fn.parameters.Count - 1 == exprs.Count && fn.parameters[fn.parameters.Count-1].is_params ||
                    fn is common_namespace_function_node && (fn as common_namespace_function_node).ConnectedToType != null && fn.parameters != null
                    && fn.parameters.Count - 1 == exprs.Count && fn.parameters[fn.parameters.Count - 1].is_params)
                {
                    statements_expression_node sre = new statements_expression_node(ptcal.snl, ptcal.var_ref, ptcal.var_ref.location);
                    //exprs.remove_range(fn.parameters.Count - 1, exprs.Count - fn.parameters.Count);
                    //exprs.AddElement(ptcal.var_ref);
                    //exprs[i] = ptcal.var_ref;
                    var arri = new array_initializer(new List<expression_node>(), ptcal.var_ref.location);
                    arri.type = fn.parameters[fn.parameters.Count - 1].type;
                    exprs.AddElement(arri);
                }
                // SSM 02/05/23 - этот код ответственен за добавление параметров по умолчанию.
                // После внедрения именованных параметров здесь необходим более интеллектуальный код - для соответствующих
                // именованных параметров вместо значений по умолчанию будут добавляться их значения при вызове
                // Поступим проще - выберем одну функцию с указанными именованными параметрами и подставим все - по умолчанию и именованные
                // Если функций с указанными именоваными параметрами остается две - то кидать ошибку
                common_function_node cfn = (common_function_node)fn;
                for (int j = exprs.Count; j < cfn.parameters.Count; j++)
                {
                    common_parameter cp = (common_parameter)cfn.parameters[j];
                    if (cp.default_value != null) // default_value хранится в формальном параметре. А значение именованного параметра хранится в вызове!!
                        exprs.AddElement(cp.default_value);
                }
            }
            else if ((exprs.Count < fn.parameters.Count) && (fn.node_kind == SemanticTree.node_kind.compiled))
            {
                if (!(fn is compiled_function_node && (fn as compiled_function_node).ConnectedToType != null) && fn.parameters != null && fn.parameters.Count - 1 == exprs.Count && fn.parameters[fn.parameters.Count - 1].is_params ||
                     fn is compiled_function_node && (fn as compiled_function_node).ConnectedToType != null
                    && fn.parameters != null && fn.parameters.Count - 1 == exprs.Count && fn.parameters[fn.parameters.Count - 1].is_params)
                {
                    statements_expression_node sre = new statements_expression_node(ptcal.snl, ptcal.var_ref, ptcal.var_ref.location);
                    //exprs.AddElement(sre);
                    var arri = new array_initializer(new List<expression_node>(), ptcal.var_ref.location);
                    arri.type = fn.parameters[fn.parameters.Count - 1].type;
                    exprs.AddElement(arri);
                }
                //compiled_function_node cfn = (compiled_function_node)fn;
                for (int j = exprs.Count; j < fn.parameters.Count; j++)
                {
                    compiled_parameter cp = (compiled_parameter)fn.parameters[j];
                    if (cp.default_value != null)
                        exprs.AddElement(cp.default_value);
                }
            }
			
		}

        private function_node find_eq_return_value_method_in_list(function_node fn, List<function_node> funcs)
        {
            foreach (function_node f in funcs)
            {
                //Проверка добавлена 14.07.2006 в 14.28. Нужно с ней прогнать тесты, хотя вроде вреда от нее не должно быть.
                if (f == fn)
                {
                    
                    return f;
                }
                if (function_eq_params_and_result(fn,f))
                {
                    
                    if (fn.original_function == f.original_function)
                    {
                        return f;
                    }
                }
            }
            return null;
        }

		public function_node find_eq_method_in_list(function_node fn, List<function_node> funcs)
		{
			foreach(function_node f in funcs)
			{
				//Проверка добавлена 14.07.2006 в 14.28. Нужно с ней прогнать тесты, хотя вроде вреда от нее не должно быть.
				if (f==fn)
				{
					return f;
				}
				if (function_eq_params(fn, f))
				{
                    if (fn.original_function == f.original_function)
                    {
                        return f;
                    }
				}
			}
			return null;
		}

		public function_node is_exist_eq_method_in_list(function_node fn, List<function_node> funcs)
		{
			return find_eq_return_value_method_in_list(fn, funcs);
		}

        /// <summary>
        ///  Получение compiled_type_node из либо compiled_type_node либо compiled_generic_instance_type_node. Используется ниже локально
        /// </summary>
        private compiled_type_node get_compiled_type_node(type_node t)
        {
            var ctn = t as compiled_type_node;
            if (ctn == null)
            {
                var cgn = t as compiled_generic_instance_type_node;
                if (cgn != null)
                    ctn = cgn.original_generic as compiled_type_node;
            }
            return ctn;
        }

        // Эту функцию можно написать оптимальнее - без внешнего while. Например.
        // Запускаем алгоритм сортировки на частичном порядке. Ищем min из оставшихся и меняем его местами с текущим.
        // Потом проходимся по частично отсортированному и из пары соседних удаляем тот, что меньше
        private void delete_greater_functions(List<function_node> set_of_possible_functions, possible_type_convertions_list_list tcll)
        {
            bool remove = true;
            while (remove)
            {
                remove = false;

                // Удаляем из всех возможных перегруженных версий функций те, которые больше какой-то. Та, которая меньше по всем параметрам, остаётся. 
                // По идее это надо делать для каждой дистанции отдельно - тогда баг #1970 уйдёт
                var i = 0;
                while (i < set_of_possible_functions.Count - 1)
                {
                    var j = i + 1;
                    while (j < set_of_possible_functions.Count)
                    {
                        method_compare mc = compare_methods(set_of_possible_functions[i],
                            set_of_possible_functions[j], tcll[i], tcll[j]);
                        if (SystemLibrary.SystemLibInitializer.InSetProcedure != null)
                        {
                            if (set_of_possible_functions[j] == SystemLibrary.SystemLibInitializer.InSetProcedure.sym_info)
                            {
                                mc = method_compare.less_method;
                            }
                            else if (set_of_possible_functions[i] == SystemLibrary.SystemLibInitializer.InSetProcedure.sym_info)
                            {
                                mc = method_compare.greater_method;
                            }
                        }
                        if (mc == method_compare.greater_method) // f[i]>f[j] - значит, удалять надо f[i], а в коде наоборот!!! Странно...
                        {
                            tcll.remove_at(j);
                            set_of_possible_functions.RemoveAt(j);
                            remove = true;
                        }
                        else if (mc == method_compare.less_method)
                        {
                            tcll[i] = tcll[j];
                            set_of_possible_functions[i] = set_of_possible_functions[j];
                            tcll.remove_at(j);
                            set_of_possible_functions.RemoveAt(j);
                            remove = true;
                        }
                        else
                        {
                            j++;
                        }
                    }
                    i++;
                }
            }
        }

        private void delete_functions_with_less_result_type(List<function_node> set_of_possible_functions)
        {
            function_node best_function = null;
            List<function_node> to_remove = new List<function_node>();
            foreach (function_node fn in set_of_possible_functions)
            {
                if (fn.return_value_type != null)
                {
                    if (best_function == null)
                    {
                        best_function = fn;
                    }
                    else
                    {
                        type_compare tc = type_table.compare_types(best_function.return_value_type, fn.return_value_type);
                        if (best_function.return_value_type.is_standard_type && fn.return_value_type.is_standard_type)
                        {
                            if (tc == type_compare.less_type)
                            {
                                to_remove.Add(best_function);
                                best_function = fn;
                            }
                            else if (tc == type_compare.greater_type)
                                to_remove.Add(fn);
                        }
                        else
                        {
                            if (tc == type_compare.greater_type)
                            {
                                to_remove.Add(best_function);
                                best_function = fn;
                            }
                            else if (tc == type_compare.less_type)
                                to_remove.Add(fn);
                        }
                    }
                }
            }

            foreach (function_node fn in to_remove)
            {
                set_of_possible_functions.Remove(fn);
            }
        }


        public List<function_node> select_function_helper(expressions_list parameters, List<SymbolInfo> functions, 
             location loc, List<SyntaxTree.expression> syntax_nodes_parameters, bool only_from_not_extensions,
             out Errors.Error err_out)
        {
            // Возвращает список function_node, если он состоит из одной, то меняет parameters, 
            // в err возвращает ошибку или null если ошибки не было
            // Возможная проблема - parameters меняется после первого вызова с ошибкой и во втором вызове он некорректный
            err_out = null;
            var set_of_possible_functions = new List<function_node>();

            if (functions == null)
            {
                err_out = new NoFunctionWithThisName(loc);
                return set_of_possible_functions;
            }
            string FunctionName = "";
            if (functions != null && functions.Count > 0)
                FunctionName = (functions[0].sym_info as function_node).name;

            bool is_alone_method_defined = (functions.Count() == 1);
            function_node first_function = functions.FirstOrDefault().sym_info as function_node;
            bool _is_assigment = first_function.name == StringConstants.assign_name;
            bool is_op = StringConstants.GetNETOperName(first_function.name) != null || first_function.name.ToLower() == "in";
            basic_function_node _tmp_bfn = functions.FirstOrDefault().sym_info as basic_function_node;

            List<function_node> indefinits = new List<function_node>();

            foreach (SymbolInfo function in functions)
            {
                if (function.sym_info is compiled_function_node cfn0 &&
                    cfn0.comperehensive_type is compiled_type_node ctn0 &&
                    (ctn0.compiled_type.Name == StringConstants.pascalSystemUnitName + StringConstants.ImplementationSectionNamespaceName || ctn0.compiled_type.Name == StringConstants.pascalExtensionsUnitName + StringConstants.ImplementationSectionNamespaceName)
                    && !ctn0.compiled_type.Assembly.FullName.StartsWith("PABCRtl")) // пропустить функции (методы расширения), определенные в сборке в ПИ PABCSystem, но не в PABCRtl.dll
                    continue;
                // В режиме only_from_not_extensions пропускать все extensions
                if (only_from_not_extensions && (function.sym_info is function_node) && (function.sym_info as function_node).is_extension_method)
                    continue;

                if (function.sym_info.general_node_type != general_node_type.function_node && function.sym_info.general_node_type != general_node_type.property_node)
                {
                    continue;
                }

                function_node fn = null;
                if (function.sym_info.general_node_type == general_node_type.property_node)
                {
                    fn = (function.sym_info as property_node).get_function;
                }
                else
                    fn = (function_node)function.sym_info;

                if (fn.node_kind == node_kind.indefinite)
                {
                    indefinits.Add(fn);
                }
                else if (fn.node_kind == node_kind.common)
                {
                    common_function_node cfn = (common_function_node)fn;
                    if ((parameters.Count >= cfn.parameters.Count - cfn.num_of_default_variables) &&
                        (parameters.Count <= cfn.parameters.Count) || parameters.Count == 0 && cfn.parameters.Count == 1 && cfn.parameters[0].is_params)
                    {
                        var fm = find_eq_method_in_list(fn, set_of_possible_functions); // без возвращаемого значения в отличие от is_exist_eq_method_in_list

                        bool bo = fm != null;
                        if (bo)
                            bo = eq_type_nodes(fn.return_value_type, fm.return_value_type);

                        //var bo = is_exist_eq_method_in_list(fn, set_of_possible_functions) != null;
                        if (bo)
                        {
                            if (set_of_possible_functions.Count > 0)
                                if (set_of_possible_functions[0] is basic_function_node)
                                {
                                    set_of_possible_functions.Remove(set_of_possible_functions[0]);
                                    set_of_possible_functions.Add(fn);
                                }

                            continue;
                        }
                        if (fm != null && fn is common_method_node cmnfn && fm is common_method_node cmnfm && cmnfn.comperehensive_type != cmnfm.comperehensive_type)
                        {
                            //fn = fn;
                        }
                        // Если fm и fn принадлежат к разным классам, то не добавлять fn - она принадлежит предку поскольку встречалась позже
                        else set_of_possible_functions.Add(fn);
                    }
                }
                else
                {
                    //TODO: Здесь нужно поправить, если создавать возможность вызова метода с параметрами по умолчанию из откомпилированной dll.
                    if (parameters.Count == fn.parameters.Count)
                    {
                        function_node fm = null;
                        if ((fm = is_exist_eq_method_in_list(fn, set_of_possible_functions)) != null)
                        {
                            if (!eq_type_nodes(fn.return_value_type, fm.return_value_type))
                            {
                                set_of_possible_functions[set_of_possible_functions.IndexOf(fm)] = fn;

                            }
                            continue;
                        }
                        else if ((fm = find_eq_method_in_list(fn, set_of_possible_functions)) != null)
                        {
                            // fn и fm могут быть также common_method_node
                            // SSM 31/05/24 - добавляю в условия common_method_node. 
                            // Это ошибка проектирования - много одинаковых свойств есть в compiled_function_node и common_method_node
                              // но они не наследуются от единого предка. В результате весь этот ужас надо повторять везде по проекту. 
                              // Это надо перепроектировать!!!
                            polymorphic_state fn_ps = polymorphic_state.ps_common;
                            polymorphic_state fm_ps = polymorphic_state.ps_common;
                            if (fn is compiled_function_node)
                                fn_ps = (fn as compiled_function_node).polymorphic_state;
                            else if (fn is common_method_node)
                                fn_ps = (fn as common_method_node).polymorphic_state;
                            if (fm is compiled_function_node)
                                fm_ps = (fm as compiled_function_node).polymorphic_state;
                            else if (fm is common_method_node)
                                fm_ps = (fm as common_method_node).polymorphic_state;

                            ITypeNode fn_ct = null;
                            ITypeNode fm_ct = null;
                            if (fn is compiled_function_node)
                                fn_ct = (fn as compiled_function_node).comperehensive_type;
                            else if (fn is common_method_node)
                                fn_ct = (fn as common_method_node).comperehensive_type;
                            if (fm is compiled_function_node)
                                fm_ct = (fm as compiled_function_node).comperehensive_type;
                            else if (fm is common_method_node)
                                fm_ct = (fm as common_method_node).comperehensive_type;

                            // SSM 29/04/24 - небольшой рефакторинг для большей понятности
                            if (fn_ps == polymorphic_state.ps_static || fm_ps == polymorphic_state.ps_static)
                            {

                            }
                            // SSM 29/04/24 - добавил такое же условие что и в предыдущей ветке 
                            else if (fn_ct != null && fm_ct != null 
                                && ! (fn_ct.IsInterface || fm_ct.IsInterface)// для интерфейсов обрабатывается ниже
                                && fn_ct != fm_ct)
                            {
                                // пропускается такая же функция, но из предка!
                            }
                            else set_of_possible_functions.Add(fn);
                        }
                        else   
                            set_of_possible_functions.Add(fn);
                    }
                }
                if (parameters.Count > fn.parameters.Count)
                {
                    if (fn.parameters.Count > 0)
                    {
                        if ((fn.parameters[fn.parameters.Count - 1]).is_params)
                        {
                            //+DS ms0105
                            if (is_exist_eq_method_in_list(fn, set_of_possible_functions) != null)
                            {
                                continue;
                            }
                            //-DS
                            set_of_possible_functions.Add(fn);
                        }
                    }
                }
                //else if ((parameters.Count == 0 && fn.parameters.Count == 1) && fn.parameters[0].is_params && !set_of_possible_functions.Contains(fn))
                // SSM 6.08.18 - так просто исправить не получается - видимо, сопоставление параметров работает неверно
                else if ((parameters.Count == fn.parameters.Count - 1) && fn.parameters[fn.parameters.Count - 1].is_params && !set_of_possible_functions.Contains(fn))
                    set_of_possible_functions.Add(fn);
                else if (fn.num_of_default_parameters != 0 && parameters.Count >= fn.parameters.Count - fn.num_of_default_parameters)
                {
                    if (!set_of_possible_functions.Contains(fn))
                        set_of_possible_functions.Add(fn);
                }
                else if (parameters.Count == 1 && fn is common_namespace_function_node && (fn as common_namespace_function_node).ConnectedToType != null && fn.parameters.Count == 2 && fn.parameters[1].is_params)
                    set_of_possible_functions.Add(fn);
                else if (parameters.Count == 1 && fn is compiled_function_node && (fn as compiled_function_node).ConnectedToType != null && fn.parameters.Count == 2 && fn.parameters[1].is_params)
                    if (!set_of_possible_functions.Contains(fn))
                        set_of_possible_functions.Add(fn);
            }

            if (set_of_possible_functions.Count == 0 && indefinits.Count == 0)
            {
                err_out = new NoFunctionWithSameParametresNum(loc, is_alone_method_defined, first_function);
                return set_of_possible_functions;
            }

            //(ssyy) Инициализируем is_alone_defined
            FailedWhileTryingToCompileLambdaBodyWithGivenParametersException lastFailedWhileTryingToCompileLambdaBodyWithGivenParametersException = null;

            if (set_of_possible_functions.Count == 1 && indefinits.Count == 0)
            {
                is_alone_method_defined = true;
            }

            //(ssyy) Инстанцируем дженерики
            for (int i = set_of_possible_functions.Count - 1; i > -1; --i)
            {
                function_node func = set_of_possible_functions[i];
                if (func.is_generic_function)
                {
                    try
                    {
                        // syntax_nodes_parameters в DeduceFunction - ничего плохого нет. Из них фильтруются 
                        // только function_lambda_definition, а в значениях именованных аргументов их не может быть. 
                        // Хотя и в этом случае проблем не вижу
                        function_node inst = generic_convertions.DeduceFunction(func, parameters,
                                                                                is_alone_method_defined, syntax_tree_visitor.context, loc, syntax_nodes_parameters);
                        if (inst == null)
                        {
                            try
                            {
                                inst = generic_convertions.DeduceFunction(func, parameters, true, syntax_tree_visitor.context, loc, syntax_nodes_parameters);
                            }
                            catch
                            {

                            }
                            if (inst == null)
                                set_of_possible_functions.RemoveAt(i);
                            else
                                set_of_possible_functions[i] = inst;
                        }
                        else
                        {
                            set_of_possible_functions[i] = inst;
                        }
                    }
                    catch (FailedWhileTryingToCompileLambdaBodyWithGivenParametersException exc) //lroman Отлавливаем последнее исключение, которое возникло при попытке скомпилировать тело лямбды с заданными типами параметров
                    {
                        set_of_possible_functions.RemoveAt(i);
                        lastFailedWhileTryingToCompileLambdaBodyWithGivenParametersException = exc;
                    }
                    catch (Errors.Error err1)
                    {
                        err_out = err1;
                        return set_of_possible_functions;
                    }
                }
            }

            if (lastFailedWhileTryingToCompileLambdaBodyWithGivenParametersException != null
                && set_of_possible_functions.Count == 0
                && indefinits.Count == 0)
            {
                throw lastFailedWhileTryingToCompileLambdaBodyWithGivenParametersException.ExceptionOnCompileBody;  // Если перебрали все, но ничто не подошло, то кидаем последнее исключение
            }
            else if (lastFailedWhileTryingToCompileLambdaBodyWithGivenParametersException != null
                && set_of_possible_functions.Count > 0
                && indefinits.Count == 0)
            {
                syntax_tree_visitor.RemoveLastError();
            }
            if (set_of_possible_functions.Count == 0 && indefinits.Count == 0)
            {
                if (is_op)
                    err_out = new OperatorCanNotBeAppliedToThisTypes(first_function.name, parameters[0], parameters.Count > 1 ? parameters[1] : null, loc);
                else err_out = new SimpleSemanticError(loc, "CAN_NOT_CALL_ANY_GENERIC_FUNCTION_{0}_WITH_THESE_PARAMETERS", first_function.name);
                return set_of_possible_functions;
            }

            possible_type_convertions_list_list tcll = new possible_type_convertions_list_list();
            Dictionary<function_node, possible_type_convertions_list> last_chance_list = new Dictionary<function_node, possible_type_convertions_list>();
            for (int i = 0; i < set_of_possible_functions.Count; i++)
            {
                Errors.Error err = null;
                possible_type_convertions_list tc = get_conversions(parameters, set_of_possible_functions[i].parameters,
                    is_alone_method_defined, loc, out err);

                var proc_func_or_lambdaAndNotDelegate_OK_flag = true;
                // Цикл по всем параметрам. Если формальный - процедура, а фактический - функция, то tc делать = null и взводить специальный флаг чтобы не возиться с делегатами дальше

                if (set_of_possible_functions.Count >= 2 && set_of_possible_functions[i].parameters.Count == parameters.Count)
                    for (var k = 0; k < parameters.Count; k++)
                    {
                        var fact = parameters[k];
                        var formal = set_of_possible_functions[i].parameters[k];
                        if (fact.type is delegated_methods dm)
                        {
                            var fact_is_function_with_return_value = dm.proper_methods.Count > 0 && dm.proper_methods[0].function.return_value_type != null;
                           
                            var fact_is_lambda = syntax_nodes_parameters != null && k < syntax_nodes_parameters.Count && syntax_nodes_parameters[k] is SyntaxTree.function_lambda_definition;
                            var form_is_procedure = false;
                            var form_is_delegate = false;

                            //if (formal.type is compiled_type_node || // Это на случай System.Action или ()->()
                            // formal.type is compiled_type_node) // Это на случай procedure
                            {
                                var d = formal.type.internal_interfaces;
                                if (d != null && d.ContainsKey(internal_interface_kind.delegate_interface))
                                {
                                    var q = d[internal_interface_kind.delegate_interface] as delegate_internal_interface;
                                    if (q != null)
                                        form_is_delegate = true;
                                    if (q != null && q.return_value_type == null) // Это процедура
                                        form_is_procedure = true;
                                }
                            }

                            if (fact_is_function_with_return_value && form_is_procedure)
                            {
                                proc_func_or_lambdaAndNotDelegate_OK_flag = false;
                                if (dm.proper_methods[0].function.return_value_type is lambda_any_type_node)
                                    last_chance_list[set_of_possible_functions[i]] = tc;
                                break;
                            }
                            if (fact_is_lambda && !form_is_delegate) // лямбда вместо не делегата - исключает функцию из рассмотрения
                            {
                                proc_func_or_lambdaAndNotDelegate_OK_flag = false;
                                break;
                            }
                        }
                    }

                if (proc_func_or_lambdaAndNotDelegate_OK_flag == false)
                    tc = null;
                // Вот здесь - если фактический параметр - функция, а формальный - процедура хотя бы в одном параметре, то надо нивелировать все преобразования и присваивать tc=null и не влазить в следующий код!!!
                if (err != null)
                {
                    err_out = err;
                    return set_of_possible_functions;
                }
                    
                //fix dlja lambd i extension metodov (c->c.IsDigit)
                if (tc == null)
                {
                    expressions_list el = new expressions_list();
                    el.AddRange(parameters);
                    bool has_lambda_var = false;
                    for (int k = 0; k < el.Count; k++)
                    {
                        if (el[k] is local_variable_reference && el[k].type is delegated_methods && (el[k].type as delegated_methods).empty_param_method != null)
                        {
                            el[k].type = (el[k].type as delegated_methods).empty_param_method.ret_type;
                            has_lambda_var = true;
                        }
                    }
                    if (has_lambda_var)
                    {
                        err = null;
                        tc = get_conversions(el, set_of_possible_functions[i].parameters,
                            is_alone_method_defined, loc, out err);
                        if (err != null)
                        {
                            err_out = err;
                            return set_of_possible_functions;
                        }
                    }

                }
                tcll.AddElement(tc);
            }

            int j = 0;
            while (j < set_of_possible_functions.Count)
            {
                if (tcll[j] == null && set_of_possible_functions[j].node_kind != node_kind.indefinite)
                {
                    tcll.remove_at(j);
                    set_of_possible_functions.RemoveAt(j);
                }
                else
                {
                    j++;
                }
            }

            if (set_of_possible_functions.Count == 0 && indefinits.Count == 0 && last_chance_list.Count == 1)
            {
                tcll.AddElement(last_chance_list.First().Value);
                set_of_possible_functions.Add(last_chance_list.First().Key);
            }

            if (set_of_possible_functions.Count == 0 && indefinits.Count == 0)
            {
                if (_is_assigment && parameters.Count == 2)
                    err_out = new CanNotConvertTypes(parameters[1], parameters[1].type, parameters[0].type, parameters[1].location);
                else if (_tmp_bfn != null && parameters.Count == 2)
                    err_out = new OperatorCanNotBeAppliedToThisTypes(_tmp_bfn.name, parameters[0], parameters[1], loc);
                else if (is_op)
                    err_out = new OperatorCanNotBeAppliedToThisTypes(first_function.name, parameters[0], parameters.Count > 1 ? parameters[1] : null, loc);
                else 
                    err_out = new NoFunctionWithSameArguments(FunctionName, loc, is_alone_method_defined);
                return set_of_possible_functions;
            }

            if (set_of_possible_functions.Count == 1)
            {
                check_single_possible_convertion(loc, tcll[0]);
                convert_function_call_expressions(set_of_possible_functions[0], parameters, tcll[0]);
                return set_of_possible_functions;
            }

            // Если остались параметры функция и процедура - обе без параметров, но функция возвращает T, то функция будет удалена этим алгоритмом, что неправильно!
            // Потому что не учитывается вызов - в вызове может быть функция!!
            delete_greater_functions(set_of_possible_functions, tcll); // SSM 06/06/19 refactoring

            if (set_of_possible_functions.Count == 1)
            {
                check_single_possible_convertion(loc, tcll[0]);
                convert_function_call_expressions(set_of_possible_functions[0], parameters, tcll[0]);
                return set_of_possible_functions;
            }
            //Тупая заглушка для примитивных типов. иначе не работает +=, у нас лишком много неявных приведений
            //в дальнейшем может вызвать странное поведение, это надо проверить
            if (set_of_possible_functions.Count == 2 && indefinits.Count == 0)
                if (set_of_possible_functions[0] is basic_function_node && set_of_possible_functions[1] is basic_function_node
                    // добавил это условие из-за комментария. Не понимаю, почему иначе не работает +=. Все тесты проходят 01.07.19 если закомментировать вообще этот if !!!
                    && set_of_possible_functions[0].name == "+=" && set_of_possible_functions[1].name == "+=")
                {
                    var tlist = new List<function_node>();
                    tlist.Add(set_of_possible_functions[0]);
                    return tlist;
                }
                    

            if (indefinits.Count > 0)
            {
                if (indefinits.Count == 1 && set_of_possible_functions.Count == 0)
                {
                    var tlist1 = new List<function_node>();
                    tlist1.Add(indefinits[0]);
                    return tlist1;
                }
                indefinits.AddRange(set_of_possible_functions);
                var tlist = new List<function_node>();
                tlist.Add(new indefinite_functions_set(indefinits));
                return tlist;
            }

            bool exist_indefinite_parameter = false;
            foreach (expression_node par in parameters)
            {
                if (par.type.depended_from_indefinite)
                {
                    exist_indefinite_parameter = true;
                    break;
                }
            }
            if (exist_indefinite_parameter)
            {
                indefinits.AddRange(set_of_possible_functions);
                var tlist = new List<function_node>();
                tlist.Add(new indefinite_functions_set(indefinits));
                return tlist;
            }

            // Удалить функции, у которых тип возвращаемого значения меньше.
            // Тут это ошибка т.к. параметры могут соответствовать точнее, а они проверяются дальше. Приоритет должен даваться параметрам
            //delete_functions_with_less_result_type(set_of_possible_functions);

            // Формирование словаря списков функций с одинаковым значением расстояния
            // SSM 07/11/17 Для a.Average(x->x) - остаётся 4 функции. На этом уровне выбрать невозможно:
            // Average(IEnumerable<integer>,integer->real): real
            // Average(IEnumerable<integer>,integer->int64): real
            // Average(IEnumerable<integer>,integer->Nullable<integer>): real
            // Average(IEnumerable<integer>,integer->integer): real
            // Дело в том, что на этом уровне лямбда x->x имеет тип lambda_anytype->lambda_anytype. Все подходят :)
            SortedDictionary<int, List<function_node>> distances = new SortedDictionary<int, List<function_node>>();
            foreach (function_node fn in set_of_possible_functions)
            {
                int distance = 0;
                for (int i = 0; i < parameters.Count; i++)
                {
                    //type_node from = parameters[i].type;

                    expression_node en = parameters[i];
                    syntax_tree_visitor.try_convert_typed_expression_to_function_call(ref en);
                    type_node from = en.type;

                    type_node to = fn.parameters[Math.Min(i, fn.parameters.Count - 1)].type;
                    if (fn.parameters[Math.Min(i, fn.parameters.Count - 1)].is_params)
                        to = to.element_type;
                    // ToDo: необходимо сделать более детальную get_type_distance. 
                    // Сейчас для функциональных параметров она всегда возвращает 1000
                    distance += get_type_distance(from, to);
                }

                /*if (fn.return_value_type != null)
                {
                    if (fn.return_value_type.is_generic_type_instance || fn.return_value_type.is_generic_type_definition)
                        distance += 2;
                    else if (fn.return_value_type == SystemLibrary.SystemLibrary.object_type)
                        distance += 3;
                    else
                        distance += 1;
                }*/
                List<function_node> lst;
                if (distances.TryGetValue(distance, out lst))
                {
                    lst.Add(fn);
                }
                else
                {
                    lst = new List<function_node>();
                    lst.Add(fn);
                    distances[distance] = lst;
                }
            }
            // Оставшиеся функции отсортированы по расстояниям

            foreach (int dist in distances.Keys) // Дистанции упорядочены по возрастанию. Логика тут странная и видимо неправильная: 
                                                 // если для какой-то минимальной дистанции найдется ровно одна функция, то она и выбирается
                                                 // если две и больше - то они пробрасываются и мы переходим к бОльшей дистанции
            {
                List<function_node> funcs = distances[dist];

                // Удалить функции, у которых тип возвращаемого значения меньше.
                // Тут это ошибка т.к. параметры могут соответствовать точнее, а они проверяются дальше. Приоритет должен даваться параметрам
                // SSM 27.07.19 перенес это сюда чтобы удаление происходило среди функций с одной дистанцией. В строке 2162 закомментировал
                delete_functions_with_less_result_type(funcs);

                if (funcs.Count == 1) // если для данной дистанции ровно одна функция, то обработать её и вернуть
                {
                    Errors.Error err = null;
                    possible_type_convertions_list tcl = get_conversions(parameters, funcs[0].parameters, true, loc, out err);
                    if (err != null)
                    {
                        err_out = err;
                        return set_of_possible_functions;
                    }
                        
                    convert_function_call_expressions(funcs[0], parameters, tcl); // вот тут заполняются параметры
                    var tlist = new List<function_node>();
                    tlist.Add(funcs[0]);
                    return tlist;
                }

                Func<Type, bool> IsFunc = t => t.IsGenericType && t.FullName.StartsWith("System.Func");
                Func<type_node, Type> get_type = tt =>
                {
                    var ct = get_compiled_type_node(tt);
                    return ct?.compiled_type;
                };
                Func<type_node, bool> IsFuncT = tt =>
                {
                    var ct = get_compiled_type_node(tt);
                    if (ct == null)
                        return false;
                    Type t = ct.compiled_type;
                    return IsFunc(t);
                };


                var ParamsCount = funcs[0].parameters.Count;
                var b = funcs.All(f => f.parameters.Count == ParamsCount); // у всех функций с данной дистанцией должно быть одинаковое количество параметров
                if (!b) break; // если не у всех, то выйти из цикла и вернуть ошибку о многозначности выбора функции

                // Далее остался ТОЛЬКО анализ параметров, являющихся функциями Func!

                var bools = Enumerable.Range(1, funcs.Count()).Select(x => true).ToList(); // bools - это фильтр оставляемых функций для данной дистанции
                // более точно - bools контролирует только возвращаемые значения - параметры контролирует AllOK

                var AllOK = true; // Это означает, что все параметры с одним номером если функциональные, то у них - одинаковое количество и типы Generic-параметров
                for (var i = 0; i < ParamsCount; i++)
                {
                    // Хочу взять проекцию всех функций на i-тый параметр
                    var parsi = funcs.Select(f => f.parameters[i].type);
                    if (parsi.Any(p => !IsFuncT(p))) // Если какой-то параметр не функция у какой-то из функций с данной дистанцией, то пропустим этот параметр! 
                        // Это - странно! Даже если это - Action, то пропускаем! Это - очень слабая логика!
                        continue;
                    // ЕСЛИ ВСЕ ПАРАМЕТРЫ _ НЕ ФУНКЦИИ, ТО ВСЕ ПРОПУСТЯТСЯ!!!
                    var argss = funcs.Select(f => get_type(f.parameters[i].type).GetGenericArguments()).ToArray(); // последовательность массивов параметров Func
                    var cnts = argss.Select(a => a.Count());

                    // Теперь у всех функций i-тый параметр - функциональный
                    // Проверяем, что у всех функций i-тый параметр будет содержать одинаковое число generic-параметров
                    var fpi = funcs[0].parameters[i];
                    var cnt = get_type(fpi.type).GetGenericArguments().Count();
                    // Проверю, что у остальных функций столько же generic параметров
                    if (cnts.Any(c => c != cnts.First())) // если нет, то - всё - это ошибка - многозначность. Хотя тут уже не должно быть
                    {
                        AllOK = false;
                        break;
                    }

                    // Цикл по параметрам Func кроме последнего
                    // Здесь проверяется более точно - что у всех i-тых параметров не только одинаковое число generic-параметров, но и одинаковые типы
                    for (var k = 0; k < cnt - 1; k++)
                    {
                        var kpars = funcs.Select(f => get_type(f.parameters[i].type).GetGenericArguments()[k]);
                        Type p0 = kpars.First();
                        bool bb = kpars.All(p => p.Equals(p0));
                        if (!bb)
                        {
                            AllOK = false;
                            break;
                        }
                    }
                    if (!AllOK)
                        break;

                    var kres = funcs.Select(f => get_type(f.parameters[i].type).GetGenericArguments()[cnt - 1]).ToArray();

                    //if (funcs[0].is_extension_method)
                    Type fldiResType = null;
                    if (syntax_nodes_parameters != null)
                    {
                        if (funcs[0].is_extension_method)
                        {
                            if (i - 1 >= syntax_nodes_parameters.Count)
                                fldiResType = null;
                            // syntax_nodes_parameters[i - 1] или [i] - может быть другое количество синтаксических параметров 
                            // при вызове - тогда упадет. Не понимаю, почему этого не происходит
                            else fldiResType = ((syntax_nodes_parameters[i - 1] as SyntaxTree.function_lambda_definition)?.RealSemTypeOfResult as compiled_type_node)?.compiled_type;
                        }
                        else
                        {
                            // Здесь возможно будет несинхрон если в syntax_nodes_parameters пропущены какие то параметры по умолчанию
                            // Или такого быть не может?
                            if (i >= syntax_nodes_parameters.Count)
                                fldiResType = null;
                            else fldiResType = ((syntax_nodes_parameters[i] as SyntaxTree.function_lambda_definition)?.RealSemTypeOfResult as compiled_type_node)?.compiled_type;
                        }
                    }
                    // странно, но всегда кво параметров в syntax_nodes_parameters на 1 меньше. Иначе падает

                    // Получается, что fldiResType рассчитывается только если фактическими функциональными параметрами выступают лямбда-выражения. Если имена функций - это не сработает. Это - ОШИБКА!

                    for (int n = 0; n < bools.Count; n++)
                    {
                        if (fldiResType == null || !fldiResType.Equals(kres[n]))
                            bools[n] = false;
                    }
                }

                // SSM 04.06.19
                if (!AllOK) // Если у какого-то i-того функционального параметра в массиве func с данной дистанцией не все параметры совпадают по типам и количеству (возвращаемое значение не рассматривается пока), 
                    // то непонятно, что делать. Попробую выходить с исключением, что есть 2 версии
                    break;

                //if (AllOK) // Если все функциональные параметры одинаковы по количеству и типам и по возвращаемому значению всех функциональных параметров подходит только одна функция func, то вернуть её
                // Здесь AllOK - согласно коду выше
                var bc = bools.Count(x => x == true);
                if (bc == 1) // урра! нашлась ровно одна функция! мы победили!
                {
                    var ind = bools.IndexOf(true);
                    var tlist = new List<function_node>();
                    tlist.Add(funcs[ind]);
                    return tlist;
                }
                if (bc == 0) // SSM 04.06.19 - с этим расстоянием не осталось функций - перейти к следующему расстоянию
                    continue;

                /// Проба 15.01.18 - потом удалить
                /*{
                    var f1 = funcs[0];
                    var f2 = funcs[1];
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        type_node f1i = f1.parameters[i].type;
                        type_node f2i = f2.parameters[i].type;
                        var f1ic = get_compiled_type_node(f1i);
                        var f2ic = get_compiled_type_node(f2i);
                        if (f1ic == null || f2ic == null)
                            break;
                        Type ct1 = f1ic.compiled_type;
                        Type ct2 = f2ic.compiled_type;
                        if (IsFunc(ct1) && IsFunc(ct2))
                        {
                            var l1 = ct1.GetGenericArguments()[0];
                            var l2 = ct2.GetGenericArguments()[0];
                            var b = l1.Equals(l2);
                            var b1 = l1 == l2;
                        }
                        //var p = syntax_nodes_parameters[i];
                    }

                }*/

                // Вот здесь нужен анализ всех оставшихся функций. 
                // Если они отличаются только функциональными параметрами и у этих параметров
                // одинаковый тип параметров (они уже инстанцированы!), то надо еще раз 
                // вызвать то, что вызывается в DeduceFunction чтобы установить, каков тип возвращаемого значения лямбды, после чего
                // выбрать либо функцию из оставшихся с ровно совпадающим типом лямбды либо с ближайшим 
                // (остались только те, к которым можно преобразовать)
                // Некоторая проблема будет если лямбд останется несколько и расстояния до результата будут разными и ненулевыми везде
                if (funcs.Count >= 2) // SSM - это ужасный способ устранения бага #236 Range(1,10).Sum(e -> e); Для хорошего способа весь код выбора версий функций для делегатов надо переписывать
                {
                    var f1 = funcs[0];
                    var f2 = funcs[1];

                    if (f1.parameters.Count == f2.parameters.Count)
                    {
                        for (var i = 0; i < f1.parameters.Count; i++)
                        {
                            var p1 = f1.parameters[i].type;
                            var p2 = f2.parameters[i].type;
                            // типы у них должны начинаться на Func, надо посмотреть тип последнего параметра,
                            // и если это Nullable, то исключить его из рассмотрения
                            // Напоминаю, что p1 и p2 если это Func, то могут быть 
                            // либо compiled_type_node (List<int>) либо compiled_generic_instance_type_node (List<A>) 

                            var ctn1 = p1 as compiled_type_node;
                            if (ctn1 == null)
                            {
                                var cgn1 = p1 as compiled_generic_instance_type_node;
                                if (cgn1 != null)
                                    ctn1 = cgn1.original_generic as compiled_type_node;
                            }

                            var ctn2 = p2 as compiled_type_node;
                            if (ctn2 == null)
                            {
                                var cgn2 = p2 as compiled_generic_instance_type_node;
                                if (cgn2 != null)
                                    ctn2 = cgn2.original_generic as compiled_type_node;
                            }

                            if (ctn1 == null || ctn2 == null)
                                continue;

                            System.Type ct1 = ctn1.compiled_type;
                            if (ct1.IsGenericType && ct1.FullName.StartsWith("System.Func"))
                            {
                                var c = ct1.GetGenericArguments().Length;
                                if (c != 0 && (ct1.GetGenericArguments().Last().FullName == null || ct1.GetGenericArguments().Last().FullName.StartsWith("System.Nullable")))
                                {
                                    var tlist = new List<function_node>();
                                    tlist.Add(f2);
                                    convert_function_call_expressions(tlist[0], parameters, tcll[0]);
                                    return tlist;
                                }
                            }

                            System.Type ct2 = ctn2.compiled_type;
                            if (ct2.IsGenericType && ct2.FullName.StartsWith("System.Func"))
                            {
                                var c = ct2.GetGenericArguments().Length;
                                if (c != 0 && (ct2.GetGenericArguments().Last().FullName == null || ct2.GetGenericArguments().Last().FullName.StartsWith("System.Nullable")))
                                {
                                    var tlist = new List<function_node>();
                                    tlist.Add(f1);
                                    return tlist;
                                }
                            }
                        }

                        /*if (f1.return_value_type != null && f2.return_value_type != null)
                        {
                            var p1 = f1.return_value_type;
                            var p2 = f2.return_value_type;
                            var ctn1 = p1 as compiled_type_node;
                            if (ctn1 == null)
                            {
                                var cgn1 = p1 as compiled_generic_instance_type_node;
                                if (cgn1 != null)
                                    ctn1 = cgn1.original_generic as compiled_type_node;
                            }

                            var ctn2 = p2 as compiled_type_node;
                            if (ctn2 == null)
                            {
                                var cgn2 = p2 as compiled_generic_instance_type_node;
                                if (cgn2 != null)
                                    ctn2 = cgn2.original_generic as compiled_type_node;
                            }

                            if (ctn1 == null || ctn2 == null)
                                continue;

                            System.Type ct1 = ctn1.compiled_type;
                            if (ct1.IsGenericType && ct1.FullName.StartsWith("System.Func"))
                            {
                                var c = ct1.GetGenericArguments().Length;
                                if (c != 0 && ct1.GetGenericArguments().Last().FullName.StartsWith("System.Nullable"))
                                    return f2;
                            }

                            System.Type ct2 = ctn2.compiled_type;
                            if (ct2.IsGenericType && ct2.FullName.StartsWith("System.Func"))
                            {
                                var c = ct2.GetGenericArguments().Length;
                                if (c != 0 && ct2.GetGenericArguments().Last().FullName.StartsWith("System.Nullable"))
                                    return f1;
                            }
                        }*/
                    }
                }
                List<function_node> to_remove = new List<function_node>();
                foreach (function_node fn in funcs)
                {
                    if (fn.is_generic_function_instance)
                        foreach (parameter p in fn.original_function.parameters)
                        {
                            if (p.type.is_generic_parameter)
                            {
                                to_remove.Add(fn);
                                break;
                            }
                        }
                }
                if (set_of_possible_functions.Count - to_remove.Count == 1)
                {
                    foreach (function_node fn in to_remove)
                        set_of_possible_functions.Remove(fn);
                    var tlist = new List<function_node>();
                    tlist.Add(set_of_possible_functions[0]);
                    convert_function_call_expressions(tlist[0], parameters, tcll[0]);
                    return tlist;
                }
            }
            if (set_of_possible_functions.Count == 2 && set_of_possible_functions[0].return_value_type == set_of_possible_functions[1].return_value_type)
            {
                if (set_of_possible_functions[0].semantic_node_type == semantic_node_type.basic_function_node && set_of_possible_functions[1].semantic_node_type != semantic_node_type.basic_function_node)
                {
                    var tlist = new List<function_node>();
                    tlist.Add(set_of_possible_functions[1]);
                    convert_function_call_expressions(tlist[0], parameters, tcll[0]);
                    return tlist;
                }
                else if (set_of_possible_functions[1].semantic_node_type == semantic_node_type.basic_function_node && set_of_possible_functions[0].semantic_node_type != semantic_node_type.basic_function_node)
                {
                    var tlist = new List<function_node>();
                    tlist.Add(set_of_possible_functions[0]);
                    convert_function_call_expressions(tlist[0], parameters, tcll[0]);
                    return tlist;
                }
            }

            err_out = new SeveralFunctionsCanBeCalled(loc, set_of_possible_functions);
            return set_of_possible_functions;
        }

        //Первый параметр - выходной. Он содержит выражения с необходимыми преобразованиями типов.
        public function_node select_function(expressions_list parameters, List<SymbolInfo> functions, location loc, 
            List<SyntaxTree.expression> syntax_nodes_parameters = null, bool only_from_not_extensions = false, int params_inc = 0)
        {
            // only_from_not_extensions = false - не вырезается первый параметр (Включая методы расширения)
            // only_from_not_extensions = true - вырезается первый параметр (Исключая методы расширения)
            string FunctionName = "";
            // а вот в func[i].parameters - только если это метод расширения. Для методов не надо !!!
            if (functions != null && functions.Count > 0)
            { 
                FunctionName = (functions[0].sym_info as function_node).name;
            }
            // с какого номера начинаются именованные параметры
            var indexOfFirstNamedArgument = -1;
            if (syntax_nodes_parameters != null)
                indexOfFirstNamedArgument = syntax_nodes_parameters.FindIndex(exp => exp is SyntaxTree.name_assign_expr);

            if (indexOfFirstNamedArgument == -1)
            {
                Errors.Error err_out;
                var list = select_function_helper(parameters, functions, loc, syntax_nodes_parameters, only_from_not_extensions, out err_out);
                if (err_out != null)
                    return AddError<function_node>(err_out);
                return list[0];
            }
            else
            {
                var indexOfFirstNamedArgumentSem = indexOfFirstNamedArgument + params_inc;

                // Вначале ищем среди оставшихся параметров именованные
                // Создадим список имен ident именованных аргументов. 
                List<SyntaxTree.ident> NamesInNamedArguments = syntax_nodes_parameters.Skip(indexOfFirstNamedArgument)
                    .Select(par => (par as SyntaxTree.name_assign_expr).name).ToList();
                foreach (var name in NamesInNamedArguments)
                {
                    // Есть ли это имя в оставшихся параметрах
                    var found = false;
                    foreach (var ff in functions)
                    {
                        var is_ext1 = (ff.sym_info as function_node).is_extension_method;
                        var names = (ff.sym_info as function_node).parameters.Skip(indexOfFirstNamedArgument + (is_ext1 ? 1 : 0)).Select(par => par.name);
                        var fo = names.Contains(name.name, StringComparer.OrdinalIgnoreCase);
                        if (fo)
                        {
                            found = true;
                            break;
                        }
                    }
                    // Если какое то имя не найдено ни в одном элементе list - found = false - кидать ошибку о неизвестном имени
                    if (!found)
                    {
                        // Здесь - две ситуации. Если среди неименованных аргументов нет, то
                        var foundUnnamed = false;
                        foreach (var ff in functions)
                        {
                            var is_ext1 = (ff.sym_info as function_node).is_extension_method;
                            var names = (ff.sym_info as function_node).parameters.Take(indexOfFirstNamedArgument + (is_ext1 ? 1 : 0)).Select(par => par.name);
                            var fo = names.Contains(name.name, StringComparer.OrdinalIgnoreCase);
                            if (fo)
                            {
                                foundUnnamed = true;
                                break;
                            }
                        }
                        if (!foundUnnamed)
                            return AddError<function_node>(syntax_tree_visitor.get_location(name), "UNDEFINED_NAMED_ARGUMENT_{0}", name.name);
                        // иначе: Именованный аргумент Вася задает параметр, который уже получил значение как неименованный аргумент
                        return AddError<function_node>(syntax_tree_visitor.get_location(name), "NAMED_ARGUMENT_{0}_SPECIFIES_PARAMETER_THAT_HAD_VALUE_AS_UNNAMED_ARGUMENT", name.name);
                    }
                }

                // сделаем копию parameters без именованных аргументов
                var parameters_helper = new expressions_list();
                for (int i = 0; i < indexOfFirstNamedArgumentSem; i++)
                    parameters_helper.AddElement(parameters[i]);

                Errors.Error err_out;
                var syntax_nodes_parameters1 = syntax_nodes_parameters.Take(indexOfFirstNamedArgument).ToList();
                var list = select_function_helper(parameters_helper, functions, loc, syntax_nodes_parameters1, only_from_not_extensions, out err_out);
                if (list.Count == 0 && err_out != null) // С именованными уже точно ничего не получится
                    return AddError<function_node>(new NoFunctionWithSameArguments(FunctionName, loc, functions.Count == 1));

                // Теперь остались функции functions, такие что каждый именованный аргумент есть хотя бы в одной из них
                var hs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                for (int i = indexOfFirstNamedArgument; i < syntax_nodes_parameters.Count; i++)
                {
                    var nae = syntax_nodes_parameters[i] as SyntaxTree.name_assign_expr;
                    hs.Add(nae.name.name);
                }
                // В syntax_nodes_parameters и parameters по идее должно быть одно и то же количество значений

                // В C# - вот такое сообщение об ошибке: 
                // Наиболее подходящий перегруженный метод 'proc' не имеет параметра с именем 'Вася'

                // Удалим из list все неподходящие, где нет таких имен параметров в оставшейся части
                list.RemoveAll(ff =>
                {
                    var is_ext1 = ff.is_extension_method;
                    var names = ff.parameters.Skip(indexOfFirstNamedArgument + (is_ext1 ? 1 : 0)).Select(par => par.name);
                    var hsnames = new HashSet<string>(names, StringComparer.OrdinalIgnoreCase);
                    return !hs.IsSubsetOf(hsnames); 
                    // то есть все имена в именованных аргументах должны находиться среди оставшихся параметров по умолчанию
                });
                if (list.Count == 0)
                    return AddError<function_node>(new NoFunctionWithSameArguments(FunctionName, loc, true)); // или false - не знаю
                if (list.Count > 1)
                    return AddError<function_node>(new SeveralFunctionsCanBeCalled(loc, list)); // или другое сообщение

                // Теперь раз в list осталась одна функция, найти её в functions и только её и оставить
                // Надо делать копию functions иначе это влияет на следующий код
                if (list[0].is_generic_function_instance)
                    list[0] = list[0].original_function;
                var functions1 = functions.Where(fun => fun.sym_info == list[0]).ToList();
                if (functions1.Count == 0)
                    return AddError<function_node>(new NoFunctionWithSameArguments(FunctionName, loc, true)); // это не должно случиться, но вдруг
                // Вот и в functions осталась одна функция
                // Теперь делаем словарь сопоставлений именам -> expression_node (в параметрах)
                var dict = new Dictionary<string, expression_node>(StringComparer.OrdinalIgnoreCase); // Это словарь именованных аргументов
                for (int i = indexOfFirstNamedArgument; i < syntax_nodes_parameters.Count; i++)
                {
                    var nae = syntax_nodes_parameters[i] as SyntaxTree.name_assign_expr;
                    //dict[nae.name.name] = parameters[i + (NeedIncrementIndexFNA ? 1 : 0)]; // parameters и syntax_nodes_parameters д.б.одной длины
                    dict[nae.name.name] = parameters[i + params_inc];
                }
                // Вот теперь как-то хвост в parameters надо менять на параметры по умолчанию или на те, что хранятся в dict
                // Я бы очистил хвост parameters и добавлял бы в него как раньше параметры по умолчанию
                parameters.remove_range(indexOfFirstNamedArgumentSem, parameters.Count - indexOfFirstNamedArgumentSem); // удалить хвост

                var fun1 = functions1[0].sym_info as function_node; // это - единственная функция
                var is_ext = fun1.is_extension_method;
                for (int i = indexOfFirstNamedArgument + (is_ext ? 1 : 0); i < fun1.parameters.Count; i++) // сверить это с другим кодом
                {
                    var par = fun1.parameters[i];
                    var paramexpr = par.default_value;
                    var name = par.name;
                    if (dict.ContainsKey(name))
                        paramexpr = dict[name]; // вместо значения по умолчанию взять значение именованного параметра
                    parameters.AddElement(paramexpr);
                }
                // И наконец всё готово!!!
                // syntax_nodes_parameters мне кажется всё испортят - там именованные аргументы стоят не на своих местах!!!
                // Это единственный - опасный и потенциально с ошибками вызов!
                // Но ощущение, что всё верно, т.к. это нужно только для параметров лямбд, а у именованных аргументов их быть не может
                list = select_function_helper(parameters, functions1, loc, syntax_nodes_parameters, only_from_not_extensions, out err_out);
                if (err_out != null)
                    return AddError<function_node>(err_out);
                return list[0];
            }

            // для именованных параметров последовательность может быть такая
            // Если именованных параметров при вызове нет, то - этот алгоритм. Если есть, то
            // 1. Оставить только неименованные параметры и вызвать select_function_helper
            //    Если не найдено ни одной функции, то ошибка и выход
            //    Если найдена одна функция и это ошибка, то её и кинуть - выход
            //    Если найдена одна функция и ошибки нет или найдено две и более функций (с ошибкой разумеется), то
            // 2. Ищем среди найденных функций только те, в которых среди оставшихся после основных параметрах по умолчанию
            //    есть параметры по умолчанию ровно с такими же именами как и именованные параметры при вызове
            //    Повторяющиеся именованные параметры при вызове отсекать ранее
            // 3. Думаю, так: если не осталось ни одной функции, то - ошибка
            //    Если осталось более одной функции, то - ошибка.
            //    Если осталась ровно одна функция, то 
            // 4. Сформировать вызов такой функции с полными семантическими параметрами. Вместо параметров по умолчанию
            //    подставить их значнеия, именованных параметров подставить ИХ значения.
            //    И вызвать select_function_helper. И либо он вернет ошибку либо единственную правильную функцию
            //    Основная проблема: parameters - это параметры при вызове, в них нет имён, это семантика
            //    Взять недостающие значения по умолчанию из functions[0] и заменить именованные их значениями
            //    В parameters как-то по особому хранить именованные и первый раз не учитывать
        }

        private int get_type_distance(type_node from, type_node to)
        {
            if (from == to)
                return 0;
            if (from is short_string_type_node && to is compiled_type_node ctn && ctn.compiled_type == typeof(System.String))
                return 0;
            if (from is delegated_methods && (from as delegated_methods).empty_param_method != null && (from as delegated_methods).empty_param_method.ret_type != null)
                from = (from as delegated_methods).empty_param_method.ret_type;
            if (to is delegated_methods && (to as delegated_methods).empty_param_method != null && (to as delegated_methods).empty_param_method.ret_type != null)
                to = (to as delegated_methods).empty_param_method.ret_type;
            internal_interface from_ii = from.get_internal_interface(internal_interface_kind.delegate_interface);
            internal_interface to_ii = to.get_internal_interface(internal_interface_kind.delegate_interface);
            if (from_ii != null && to_ii != null)
            {
                delegate_internal_interface from_dii = (delegate_internal_interface)from_ii;
                if (to_ii != null)
                {
                    delegate_internal_interface to_dii = (delegate_internal_interface)to_ii;
                    if (from_dii.parameters.Count == to_dii.parameters.Count)
                    {
                        bool eq = TreeConverter.convertion_data_and_alghoritms.function_eq_params_and_result(from_dii.invoke_method, to_dii.invoke_method);
                        if (eq)
                        {
                            return 0;
                        }
                    }
                }
            }
            else if (to_ii != null && from.semantic_node_type == semantic_node_type.delegated_method && (from as delegated_methods).proper_methods[0].ret_type is lambda_any_type_node)
            {
                delegate_internal_interface to_dii = (delegate_internal_interface)to_ii;
                if (to_dii.invoke_method.return_value_type != null)
                {
                    to = to_dii.invoke_method.return_value_type;
                    if (to == SystemLibrary.SystemLibrary.integer_type)
                        return 94;
                    else if (to == SystemLibrary.SystemLibrary.int64_type)
                        return 96;
                    else if (to == SystemLibrary.SystemLibrary.double_type)
                        return 98;
                    else
                        return 1000;
                }
            }
            type_compare tc = type_table.compare_types(from, to);
            if (tc == type_compare.non_comparable_type)
                return 1000;
            if (!from.IsInterface && to.IsInterface)
                return 200;
            if (tc == type_compare.less_type)
            {
                return 100;
            }
            if (tc == type_compare.greater_type)
                return 300;
            return 1000;
        }

        
        private function_node is_exist_eq_return_value_method(function_node fn, List<function_node> funcs)
        {
            fn = find_eq_return_value_method_in_list(fn, funcs);
            return fn;
        }

		private string get_return_variable_name(string function_name)
		{
			return (StringConstants.function_return_value_prefix+function_name);
		}

        public void create_function_return_variable(common_function_node cfn, SymbolInfo ret_var)
        {
            if (!SemanticRulesConstants.AddResultVariable)
                return;
//#if (DEBUG)
            if (cfn.return_value_type == null)
            {
                return;
                //throw new CompilerInternalError("Procedure can not return value");
            }
//#endif
            if (cfn.return_variable != null)
            {
                if (ret_var != null)
                    ret_var.sym_info = cfn.return_variable;
                return;
            }
            cfn.return_variable = new local_variable(get_return_variable_name(cfn.name), cfn.return_value_type, cfn, cfn.loc);
            cfn.return_variable.inital_value = syntax_tree_visitor.context.GetInitalValueForVariable(cfn.return_variable, null);
            cfn.var_definition_nodes_list.AddElement(cfn.return_variable);
            if (ret_var != null)
            {
                ret_var.sym_info = cfn.return_variable;
            }
        }

        public void check_node_parser_error(SyntaxTree.syntax_tree_node tn)
		{
            if (_parser_error == null || tn==null)
			{
				return;
			}
			if (bad_nodes_in_syntax_tree[tn]!=null)
			{
				AddError(new ParserError(_parser_error,_stv.get_location(tn)));
			}
		}

        public type_node select_base_type(type_node_list types, bool only_implicit = false)
        {
            type_node ret_type = null;
            if (types.Count > 0) ret_type = types[0];
            for (int i = 1; i<types.Count; i++)
            {
                if (types[i] == ret_type) continue;
                type_compare tc = type_table.compare_types_in_specific_order(types[i], ret_type, only_implicit);
                if (tc == type_compare.greater_type && ret_type != SystemLibrary.SystemLibrary.object_type)
                    ret_type = types[i];
                else if (tc == type_compare.non_comparable_type)
                {
                    ret_type = SystemLibrary.SystemLibrary.object_type;
                    //ret_type = null;
                }
                	
            }
            return ret_type;
        }

        public type_node select_base_type_for_arr_const_new(type_node_list types, List<expression_node> lst, bool only_implicit = false)
        {
            type_node ret_type = null;
            if (types.Count > 0) ret_type = types[0];
            for (int i = 1; i < types.Count; i++)
            {
                if (types[i] == ret_type) continue;
                type_compare tc = type_table.compare_types_in_specific_order(types[i], ret_type, only_implicit);
                if (tc == type_compare.greater_type && ret_type != SystemLibrary.SystemLibrary.object_type)
                    ret_type = types[i];
                else if (tc == type_compare.non_comparable_type)
                {
                    if (can_convert_type(ret_type, types[i])) // трудно представить что обва типа можно преобразовать друг к другу
                    { 
                        ret_type = types[i];
                        continue;
                    }
                    if (!can_convert_type(types[i], ret_type))
                        AddError(lst[i].location, "UNCOMPARABLE_TYPES_IN_ARRAY_CONST");
                }

            }
            return ret_type;
        }

        public enum int_types { sbyte_type = 0, byte_type = 1, short_type = 2, ushort_type = 3, integer_type = 4, uint_type = 5, int64_type = 6, uint64_type = 7};
        public static bool is_value_int_type(type_node tn)
        {
            return 
                tn == SystemLibrary.SystemLibrary.sbyte_type ||
                tn == SystemLibrary.SystemLibrary.byte_type ||
                tn == SystemLibrary.SystemLibrary.short_type ||
                tn == SystemLibrary.SystemLibrary.ushort_type ||
                tn == SystemLibrary.SystemLibrary.integer_type ||
                tn == SystemLibrary.SystemLibrary.uint_type ||
                tn == SystemLibrary.SystemLibrary.int64_type ||
                tn == SystemLibrary.SystemLibrary.uint64_type
                ;
        }
        public static bool is_value_real_type(type_node tn)
        {
            return tn == SystemLibrary.SystemLibrary.double_type ||
                tn == SystemLibrary.SystemLibrary.float_type 
                ;
        }
        public static bool is_value_num_type(type_node tn)
        {
            return is_value_int_type(tn) || is_value_real_type(tn);
        }

        public type_node int_type_to_type_node(int_types it)
        {
            switch (it)
            {
                case int_types.sbyte_type: return SystemLibrary.SystemLibrary.sbyte_type;
                case int_types.byte_type: return SystemLibrary.SystemLibrary.byte_type;
                case int_types.short_type: return SystemLibrary.SystemLibrary.short_type;
                case int_types.ushort_type: return SystemLibrary.SystemLibrary.ushort_type;
                case int_types.integer_type: return SystemLibrary.SystemLibrary.integer_type;
                case int_types.uint_type: return SystemLibrary.SystemLibrary.uint_type;
                case int_types.int64_type: return SystemLibrary.SystemLibrary.int64_type;
                case int_types.uint64_type: return SystemLibrary.SystemLibrary.uint64_type;
            }
            return null; // это вхолостую
        }
        public int_types type_node_to_int_type(type_node tn)
        {
            if (tn == SystemLibrary.SystemLibrary.sbyte_type) // shortint
                return int_types.sbyte_type;
            else if (tn == SystemLibrary.SystemLibrary.byte_type)
                return int_types.byte_type;
            else if (tn == SystemLibrary.SystemLibrary.short_type) // smallint
                return int_types.short_type;
            else if (tn == SystemLibrary.SystemLibrary.ushort_type) // word
                return int_types.ushort_type;
            else if (tn == SystemLibrary.SystemLibrary.integer_type)
                return int_types.integer_type;
            else if (tn == SystemLibrary.SystemLibrary.uint_type) // longword
                return int_types.uint_type;
            else if (tn == SystemLibrary.SystemLibrary.int64_type)
                return int_types.int64_type;
            else if (tn == SystemLibrary.SystemLibrary.uint64_type)
                return int_types.uint64_type;
            return int_types.uint64_type; // это вхолостую
        }

        public type_node least_upper_bound_value_int_type(type_node tn1, type_node tn2) // для вещественных: real > single. decimal > всех, но его нельзя присвоить другим
        {
            var n1 = type_node_to_int_type(tn1);
            var n2 = type_node_to_int_type(tn2);
            if ((int)n1 > (int)n2)
            {
                var v = n1;
                n1 = n2;
                n2 = v;
                var t = tn1;
                tn1 = tn2;
                tn2 = t;
            }
            if (n1 == n2) return tn1;
            // Первый тип  - меньше или равен
            if ((int)n1 <= (int)int_types.integer_type && (int)n2 <= (int)int_types.integer_type) return SystemLibrary.SystemLibrary.integer_type;
            
            // Первый тип  - меньше
            if ((int)n2 == (int)int_types.uint64_type)
                return SystemLibrary.SystemLibrary.uint64_type;
            if ((int)n2 == (int)int_types.int64_type)
            {
                if ((int)n1 <= (int)int_types.integer_type)
                    return SystemLibrary.SystemLibrary.int64_type;
                else return SystemLibrary.SystemLibrary.uint64_type;
            }
            if ((int)n2 == (int)int_types.uint_type) // longword
            {
                if ((int)n1 == 0 || (int)n1 == 2 || (int)n1 == 4)
                    return SystemLibrary.SystemLibrary.int64_type;
                if ((int)n1 == 1 || (int)n1 == 3)
                    return SystemLibrary.SystemLibrary.uint_type; // longword
                // n1 = 6 или 7
                return SystemLibrary.SystemLibrary.uint64_type;
            }
            return null; // это вхолостую
        }

    }

}
