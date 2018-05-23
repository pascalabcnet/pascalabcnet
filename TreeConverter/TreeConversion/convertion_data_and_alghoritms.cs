// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
//Некоторые алгоритмы . В основном выбор перегруженного метода. Сильно связан с syntax_tree_visitor.
using System;
using System.Collections.Generic;
using System.Linq;

using PascalABCCompiler.SemanticTree;
using PascalABCCompiler.TreeRealization;

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

        //TODO: Наверно это свойство вообще не нужно.
		public syntax_tree_visitor syntax_tree_visitor
		{
			get
			{
				return _stv;
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
            common_type_node converted_type, common_function_node top_function, bool allow_procedure)
        {
            function_node fn = select_function(exprs, si, loc);

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
                                                syntax_tree_visitor.AddError(new SimpleSemanticError(loc, "CAN_NOT_CALL_CONSTRUCTOR_AS_PROCEDURE"));
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

            //TODO: А если наследование?
			possible_type_convertions pct=type_table.get_convertions(en.type,to);

			if (pct.second!=null)
			{
                AddError(new TwoTypeConversionsPossible(en,pct.first,pct.second));
			}

			if (pct.first==null)
			{
                if (to is delegated_methods && (to as delegated_methods).empty_param_method != null)
                {
                    return convert_type(en, (to as delegated_methods).empty_param_method.ret_type, loc);
                }
                en.location = loc;
                AddError(new CanNotConvertTypes(en, en.type, to, loc));
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
                    !(factparams.Count == formalparams.Count && factparams[factparams.Count - 1].type == formalparams[formalparams.Count - 1].type))
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
                            vdn = syntax_tree_visitor.context.add_field(get_temp_arr_name(), loc, pr.type, polymorphic_state.ps_static);
                        else
                            vdn = syntax_tree_visitor.context.add_var_definition_in_entry_scope(get_temp_arr_name(), loc);
                    }
                    else if (syntax_tree_visitor.context.converted_type != null)
                        vdn = syntax_tree_visitor.context.add_field(get_temp_arr_name(), loc, pr.type, polymorphic_state.ps_static);
                    else
                        vdn = syntax_tree_visitor.context.add_var_definition_in_entry_scope(get_temp_arr_name(), loc);
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
                    tc.snl.AddElement(bfc);
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
						if (factparams[i].is_addressed==false)
						{
                            if (factparams[i].semantic_node_type == semantic_node_type.common_method_call)
                            {
                                common_method_call cmc = (common_method_call)factparams[i];
                                internal_interface ii = cmc.obj.type.get_internal_interface(internal_interface_kind.bounded_array_interface);
                                if (ii != null)
                                {
                                    if (cmc.function_node.name == compiler_string_consts.get_val_pascal_array_name)
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
                        statement_node stat = syntax_tree_visitor.find_operator(compiler_string_consts.assign_name, to, from, factparams[i].location);
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
                    ptc = type_table.get_convertions(factparams[i].type, formal_param_type);
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
                                statement_node stat = syntax_tree_visitor.find_operator(compiler_string_consts.assign_name, to, from, factparams[i].location);
                                tc.snl.AddElement(stat);
                            }

						}
						else
						{
                            if (is_alone_method_defined)
                            {
                                //issue #348
                                if (formal_param_type == SystemLibrary.SystemLibrary.object_type && factparams[i].type is delegated_methods)
                                {
                                    possible_type_convertions ptci = new possible_type_convertions();
                                    ptci.first = null;
                                    ptci.second = null;
                                    ptci.from = factparams[i].type;
                                    ptci.to = formal_param_type;
                                    tc.AddElement(ptci);
                                    factparams[i] = syntax_tree_visitor.CreateDelegateCall((factparams[i].type as delegated_methods).proper_methods[0]);
                                    return tc;
                                }
                                else
                                    error = new CanNotConvertTypes(factparams[i], factparams[i].type, formal_param_type, locg);
                            }
							return null;
                            
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
                            statement_node stat = syntax_tree_visitor.find_operator(compiler_string_consts.assign_name, to, from, factparams[i].location);
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

		private method_compare compare_methods(function_node left_func, function_node right_func,
            possible_type_convertions_list left,possible_type_convertions_list right)
		{
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
            if (!left_func.is_generic_function_instance && right_func.is_generic_function_instance)
            {
                if (left_func is basic_function_node)
                    return method_compare.less_method;
                else
                    return method_compare.greater_method;
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
            return method_compare.not_comparable_methods;
		}

        //DS добавил этот метод
        //А так ли это надо делать?
        //Сергей, может ты знаеш?
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

        //Этот метод сверяет не только параметры, но и возвращаемое значение
        public static bool function_eq_params_and_result(function_node left, function_node right, bool weak=false)
        {
            if (!function_eq_params(left, right, weak))
            {
                return false;
            }
            else
            {
                return eq_type_nodes(left.return_value_type,right.return_value_type);
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
            for (int i = 0; i < left_type_params.Count; i++)
            {
                if (string.Compare(left_type_params[i].name, right_type_params[i].name, true) != 0)
                {
                    return false;
                }
            }
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
					AddError(new PossibleTwoTypeConversionsInFunctionCall(loc,ptc.first,ptc.second));
				}
				
			}
		}

		private void convert_function_call_expressions(function_node fn,expressions_list exprs,
			possible_type_convertions_list ptcal)
		{
			for(int i=0;i<exprs.Count;i++)
			{
                if ((ptcal.snl != null) && (i >= fn.parameters.Count - 1))
                {
                    //statement_list_stack.top().statements.AddRange(ptcal.snl);
                    statements_expression_node sre = new statements_expression_node(ptcal.snl, ptcal.var_ref, ptcal.var_ref.location);
                    exprs.remove_range(fn.parameters.Count - 1, exprs.Count - fn.parameters.Count);
                    //exprs.AddElement(ptcal.var_ref);
                    //exprs[i] = ptcal.var_ref;
                    exprs[i] = sre;
                    break;
                }
                if ((ptcal[i]==null)||(ptcal[i].first==null)||exprs[i] is null_const_node)
				{
					continue;
				}
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
                if (exprs.Count == 0 && fn.parameters != null && fn.parameters.Count == 1 && fn.parameters[0].is_params ||
                    exprs.Count == 1 && fn.parameters != null && fn is common_namespace_function_node && (fn as common_namespace_function_node).ConnectedToType != null &&
                    fn.parameters.Count == 2 && fn.parameters[1].is_params)
                {
                    statements_expression_node sre = new statements_expression_node(ptcal.snl, ptcal.var_ref, ptcal.var_ref.location);
                    //exprs.remove_range(fn.parameters.Count - 1, exprs.Count - fn.parameters.Count);
                    //exprs.AddElement(ptcal.var_ref);
                    //exprs[i] = ptcal.var_ref;
                    exprs.AddElement(sre);
                }
                common_function_node cfn = (common_function_node)fn;
                for (int j = exprs.Count; j < cfn.parameters.Count; j++)
                {
                    common_parameter cp = (common_parameter)cfn.parameters[j];
                    if (cp.default_value != null)
                        exprs.AddElement(cp.default_value);
                }
            }
            else if ((exprs.Count < fn.parameters.Count) && (fn.node_kind == SemanticTree.node_kind.compiled))
            {
                if (exprs.Count == 0 && fn.parameters != null && fn.parameters.Count == 1 && fn.parameters[0].is_params ||
                    exprs.Count == 1 && fn.parameters != null && fn is compiled_function_node && (fn as compiled_function_node).ConnectedToType != null &&
                    fn.parameters.Count == 2 && fn.parameters[1].is_params)
                {
                    statements_expression_node sre = new statements_expression_node(ptcal.snl, ptcal.var_ref, ptcal.var_ref.location);
                    exprs.AddElement(sre);
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

        private function_node find_eq_return_value_method_in_list(function_node fn, function_node_list funcs)
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

		public function_node find_eq_method_in_list(function_node fn,function_node_list funcs)
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

		public function_node is_exist_eq_method_in_list(function_node fn, function_node_list funcs)
		{
			return find_eq_return_value_method_in_list(fn, funcs);
		}

		public void init_reference_type(type_node ctn)
		{
			SystemLibrary.SystemLibrary.init_reference_type(ctn);
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

        //Первый параметр - выходной. Он содержит выражения с необходимыми преобразованиями типов.
        public function_node select_function(expressions_list parameters, List<SymbolInfo> functions, location loc, List<SyntaxTree.expression> syntax_nodes_parameters = null)
        {
            if (functions == null)
            {
                return AddError<function_node>(new NoFunctionWithThisName(loc));
            }

            function_node_list set_of_possible_functions = new function_node_list();

            bool is_alone_method_defined = (functions.Count() == 1);
            function_node first_function = functions.FirstOrDefault().sym_info as function_node;
            bool _is_assigment = first_function.name == compiler_string_consts.assign_name;
            basic_function_node _tmp_bfn = functions.FirstOrDefault().sym_info as basic_function_node;

            List<function_node> indefinits = new List<function_node>();

            foreach (SymbolInfo function in functions)
            {
#if (DEBUG)
                if (function.sym_info.general_node_type != general_node_type.function_node && function.sym_info.general_node_type != general_node_type.property_node)
                {
                    throw new CompilerInternalError("Function name is used to define another kind of object.");
                }
#endif
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
                        if (is_exist_eq_method_in_list(fn, set_of_possible_functions) != null)
                        {
                            if (set_of_possible_functions.Count > 0)
                                if (set_of_possible_functions[0] is basic_function_node)
                                {
                                    set_of_possible_functions.remove(set_of_possible_functions[0]);
                                    set_of_possible_functions.AddElement(fn);
                                }

                            continue;
                        }
                        set_of_possible_functions.AddElement(fn);
                    }
                }
                else
                {
                    //TODO: Здесь нужно поправить, если создавать возможность вызова метода с параметрами по умолчанию из откомпилированной dll.
                    if (parameters.Count == fn.parameters.Count)
                    {
                        function_node func = null;
                        if ((func = is_exist_eq_method_in_list(fn, set_of_possible_functions)) != null)
                        {
                            if (!eq_type_nodes(fn.return_value_type, func.return_value_type))
                            {
                                set_of_possible_functions[set_of_possible_functions.IndexOf(func)] = fn;

                            }
                            continue;
                        }
                        set_of_possible_functions.AddElement(fn);
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
                            set_of_possible_functions.AddElement(fn);
                        }
                    }
                }
                else if ((parameters.Count == 0 && fn.parameters.Count == 1) && fn.parameters[0].is_params && !set_of_possible_functions.Contains(fn))
                    set_of_possible_functions.AddElement(fn);
                else if (fn.num_of_default_parameters != 0 && parameters.Count >= fn.parameters.Count - fn.num_of_default_parameters)
                {
                    if (!set_of_possible_functions.Contains(fn))
                        set_of_possible_functions.AddElement(fn);
                }
                else if (parameters.Count == 1 && fn is common_namespace_function_node && (fn as common_namespace_function_node).ConnectedToType != null && fn.parameters.Count == 2 && fn.parameters[1].is_params)
                    set_of_possible_functions.AddElement(fn);
                else if (parameters.Count == 1 && fn is compiled_function_node && (fn as compiled_function_node).ConnectedToType != null && fn.parameters.Count == 2 && fn.parameters[1].is_params)
                    if (!set_of_possible_functions.Contains(fn))
                        set_of_possible_functions.AddElement(fn);
            }

            if (set_of_possible_functions.Count == 0 && indefinits.Count == 0)
            {
                return AddError<function_node>(new NoFunctionWithSameParametresNum(loc, is_alone_method_defined, first_function));
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

                        function_node inst = generic_convertions.DeduceFunction(func, parameters,
                                                                                is_alone_method_defined, syntax_tree_visitor.context, loc, syntax_nodes_parameters);
                        if (inst == null)
                        {
                            set_of_possible_functions.remove_at(i);
                        }
                        else
                        {
                            set_of_possible_functions[i] = inst;
                        }
                    }
                    catch (FailedWhileTryingToCompileLambdaBodyWithGivenParametersException exc) //lroman Отлавливаем последнее исключение, которое возникло при попытке скомпилировать тело лямбды с заданными типами параметров
                    {
                        set_of_possible_functions.remove_at(i);
                        lastFailedWhileTryingToCompileLambdaBodyWithGivenParametersException = exc;
                    }
                    catch(Errors.Error err)
                    {
                        return AddError<function_node>(err);
                    }
                }
            }

            if (lastFailedWhileTryingToCompileLambdaBodyWithGivenParametersException != null
                && set_of_possible_functions.Count == 0
                && indefinits.Count == 0)
            {
                throw lastFailedWhileTryingToCompileLambdaBodyWithGivenParametersException.ExceptionOnCompileBody;  // Если перебрали все, но ничто не подошло, то кидаем последнее исключение
            }

            if (set_of_possible_functions.Count == 0 && indefinits.Count == 0)
            {
                return AddError<function_node>(loc, "CAN_NOT_CALL_ANY_GENERIC_FUNCTION_{0}_WITH_THESE_PARAMETERS", first_function.name);
            }

            possible_type_convertions_list_list tcll = new possible_type_convertions_list_list();

            for (int i = 0; i < set_of_possible_functions.Count; i++)
            {
                Errors.Error err = null;
                possible_type_convertions_list tc = get_conversions(parameters, set_of_possible_functions[i].parameters,
                    is_alone_method_defined, loc, out err);
                if (err != null)
                    return AddError<function_node>(err);
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
                            return AddError<function_node>(err);
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
                    set_of_possible_functions.remove_at(j);
                }
                else
                {
                    j++;
                }
            }

            if (set_of_possible_functions.Count == 0 && indefinits.Count == 0)
            {
                if (_is_assigment && parameters.Count == 2)
                    return AddError<function_node>(new CanNotConvertTypes(parameters[1], parameters[1].type, parameters[0].type, parameters[1].location));
                if (_tmp_bfn != null && parameters.Count == 2)
                    return AddError<function_node>(new OperatorCanNotBeAppliedToThisTypes(_tmp_bfn.name, parameters[0], parameters[1], loc));
                return AddError<function_node>(new NoFunctionWithSameArguments(loc, is_alone_method_defined));
            }

            bool remove = true;

            while (remove)
            {
                if (set_of_possible_functions.Count == 1)
                {
                    check_single_possible_convertion(loc, tcll[0]);
                    convert_function_call_expressions(set_of_possible_functions[0], parameters, tcll[0]);
                    return set_of_possible_functions[0];
                }
                remove = false;

                var i = 0;
                while (i < set_of_possible_functions.Count - 1)
                {
                    j = i + 1;
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
                        if (mc == method_compare.greater_method)
                        {
                            tcll.remove_at(j);
                            set_of_possible_functions.remove_at(j);
                            remove = true;
                        }
                        else if (mc == method_compare.less_method)
                        {
                            tcll[i] = tcll[j];
                            set_of_possible_functions[i] = set_of_possible_functions[j];
                            tcll.remove_at(j);
                            set_of_possible_functions.remove_at(j);
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

            //Тупая заглушка для примитивных типов. иначе не работает +=, у нас лишком много неявных приведений
            //в дальнейшем может вызвать странное поведение, это надо проверить
            if (set_of_possible_functions.Count == 2 && indefinits.Count == 0)
                if (set_of_possible_functions[0] is basic_function_node && set_of_possible_functions[1] is basic_function_node)
                    return set_of_possible_functions[0];

            if (indefinits.Count > 0)
            {
                if (indefinits.Count == 1 && set_of_possible_functions.Count == 0)
                {
                    return indefinits[0];
                }
                indefinits.AddRange(set_of_possible_functions);
                return new indefinite_functions_set(indefinits);
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
                return new indefinite_functions_set(indefinits);
            }

            SortedDictionary<int, List<function_node>> distances = new SortedDictionary<int, List<function_node>>();
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
                set_of_possible_functions.remove(fn);
            }

            // Формирование словаря списков функций с одинаковым значением расстояния
            // SSM 07/11/17 Для a.Average(x->x) - остаётся 4 функции. На этом уровне выбрать невозможно:
            // Average(IEnumerable<integer>,integer->real): real
            // Average(IEnumerable<integer>,integer->int64): real
            // Average(IEnumerable<integer>,integer->Nullable<integer>): real
            // Average(IEnumerable<integer>,integer->integer): real
            // Дело в том, что на этом уровне лямбда x->x имеет тип lambda_anytype->lambda_anytype. Все подходят :)
            foreach (function_node fn in set_of_possible_functions)
            {
                int distance = 0;
                for (int i = 0; i < parameters.Count; i++)
                {
                    type_node from = parameters[i].type;
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
            foreach (int dist in distances.Keys) // Дистанции упорядочены по возрастанию. Логика тут немного странная: 
                                                 // если для какой-то минимальной дистанции найдется ровно одна функция, то она и выбирается
                                                 // если две и больше - то они пробрасываются и мы переходим к бОльшей дистанции
            {
                List<function_node> funcs = distances[dist];
                if (funcs.Count == 1)
                {
                    Errors.Error err = null;
                    possible_type_convertions_list tcl = get_conversions(parameters, funcs[0].parameters, true, loc, out err);
                    if (err != null)
                        return AddError<function_node>(err);
                    convert_function_call_expressions(funcs[0], parameters, tcl);
                    return funcs[0];
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
                var b = funcs.All(f => f.parameters.Count == ParamsCount);
                if (!b) break;

                var bools = Enumerable.Range(1, funcs.Count()).Select(x => true).ToList();

                var AllOK = true;
                for (var i=0; i < ParamsCount; i++)
                {
                    // Хочу взять проекцию всех функций на i-тый параметр
                    var parsi = funcs.Select(f => f.parameters[i].type);
                    if (parsi.Any(p => !IsFuncT(p))) // Если какой-то параметр не функция, то пробросим его
                        continue;

                    var argss = funcs.Select(f => get_type(f.parameters[i].type).GetGenericArguments()).ToArray(); // последовательность массивов параметров Func
                    var cnts = argss.Select(a => a.Count());

                    var fpi = funcs[0].parameters[i];
                    var cnt = get_type(fpi.type).GetGenericArguments().Count();
                    // Проверю, что у остальных функций столько же параметров
                    if (cnts.Any(c => c != cnts.First())) // если нет, то - всё - это ошибка - многозначность. Хотя тут уже не должно быть
                    {
                        AllOK = false;
                        break;
                    }

                    // Цикл по параметрам Func кроме последнего
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
                    var fldiResType = funcs[0].is_extension_method ? // странно, но всегда кво параметров в syntax_nodes_parameters на 1 меньше. Иначе падает
                        ((syntax_nodes_parameters[i - 1] as SyntaxTree.function_lambda_definition).RealSemTypeOfResult as compiled_type_node).compiled_type :
                        ((syntax_nodes_parameters[i - 1] as SyntaxTree.function_lambda_definition).RealSemTypeOfResult as compiled_type_node).compiled_type;

                    for (int n = 0; n < bools.Count; n++)
                    {
                        if (!fldiResType.Equals(kres[n]))
                            bools[n] = false;
                    }
                //var rettype_i = 
                }
                if (AllOK)
                    if (bools.Count(x=>x==true) == 1) // урра! нашлась ровно одна функция! мы победили!
                    {
                        var ind = bools.IndexOf(true);
                        return funcs[ind];
                    }

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

                if (funcs.Count == 2) // SSM - это ужасный способ устранения бага #236 Range(1,10).Sum(e -> e); Для хорошего способа весь код выбора версий функций для делегатов надо переписывать
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
                                    return f2;
                            }

                            System.Type ct2 = ctn2.compiled_type;
                            if (ct2.IsGenericType && ct2.FullName.StartsWith("System.Func"))
                            {
                                var c = ct2.GetGenericArguments().Length;
                                if (c != 0 && (ct2.GetGenericArguments().Last().FullName == null || ct2.GetGenericArguments().Last().FullName.StartsWith("System.Nullable")))
                                    return f1;
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
            }
            return AddError<function_node>(new SeveralFunctionsCanBeCalled(loc, set_of_possible_functions));
        }

        private int get_type_distance(type_node from, type_node to)
        {
            if (from == to)
                return 0;
            if (from is delegated_methods && (from as delegated_methods).empty_param_method != null && (from as delegated_methods).empty_param_method.ret_type != null)
                from = (from as delegated_methods).empty_param_method.ret_type;
            if (to is delegated_methods && (to as delegated_methods).empty_param_method != null && (to as delegated_methods).empty_param_method.ret_type != null)
                to = (to as delegated_methods).empty_param_method.ret_type;
            type_compare tc = type_table.compare_types(from, to);
            if (tc == type_compare.non_comparable_type)
                return 1000;
            if (!from.IsInterface && to.IsInterface)
                return 2;
            if (tc == type_compare.less_type)
                return 1;
            if (tc == type_compare.greater_type)
                return 3;
            return 1000;
        }

        private function_node is_exist_eq_return_value_method(function_node fn, function_node_list funcs)
        {
            fn = find_eq_return_value_method_in_list(fn, funcs);
            return fn;
        }

		private string get_return_variable_name(string function_name)
		{
			return (compiler_string_consts.function_return_value_prefix+function_name);
		}

        public void create_function_return_variable(common_function_node cfn, SymbolInfo ret_var)
        {
            if (!SemanticRules.AddResultVariable)
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

        public type_node select_base_type(type_node_list types)
        {
            type_node ret_type = null;
            if (types.Count > 0) ret_type = types[0];
            for (int i = 1; i<types.Count; i++)
            {
                if (types[i] == ret_type) continue;
                type_compare tc = type_table.compare_types_in_specific_order(types[i], ret_type);
                if (tc == type_compare.greater_type && ret_type != SystemLibrary.SystemLibrary.object_type) ret_type = types[i];
                else if (tc == type_compare.non_comparable_type) 
                	ret_type = SystemLibrary.SystemLibrary.object_type;
            }
            return ret_type;
        }
	}

}
