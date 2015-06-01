// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.SyntaxTree;
using System.Reflection;
using PascalABCCompiler;
using PascalABCCompiler.TreeConverter;
//using PascalABCCompiler.TreeRealization;
using SymbolTable;
using PascalABCCompiler.Parsers;

namespace CodeCompletion
{

	/// <summary>
	/// Парсит выражение до точки, скобки и проч.
	/// </summary>
	public class ExpressionVisitor : PascalABCCompiler.SyntaxTree.AbstractVisitor
	{
		private expression expr;
		public SymScope ret_tn;
		private List<SymScope> ret_names = new List<SymScope>();
		private bool search_all = false;
		public SymScope entry_scope;
		private bool any_order = false;
		private bool by_dot=false;
		private bool on_bracket;
		internal bool mouse_hover = false;
        private DomSyntaxTreeVisitor stv = null;

		public ExpressionVisitor(SymScope entry_scope, DomSyntaxTreeVisitor stv)
		{
			this.entry_scope = entry_scope;
            this.stv = stv;
		}
		
		public ExpressionVisitor(expression expr, SymScope entry_scope, DomSyntaxTreeVisitor stv)
		{
			this.expr = expr;
			this.entry_scope = entry_scope;
            this.stv = stv;
		}
		
		public SymScope GetScopeOfExpression()
		{
			if (expr is const_node && !(expr is string_const) && !(expr is char_const) && !(expr is bool_const))
				return null;
			any_order = true;
			if (mouse_hover && (expr is typeof_operator || expr is sizeof_operator))
				return null;
			try
			{
				expr.visit(this);
			}
			catch(Exception e)
			{
				ret_tn = null;
			}
			if (ret_tn != null && ret_tn is ElementScope && (ret_tn as ElementScope).sc is ProcScope)
			{
				if ((ret_tn as ElementScope).si.kind == SymbolKind.Delegate)
					ret_tn = (ret_tn as ElementScope).sc;
			}
				//else ret_scope = (ret_scope as ElementScope).sc as ProcScope;
			return ret_tn;
		}
		
		public SymScope GetScopeOfExpression(bool by_dot, bool on_bracket)
		{
			if (expr is const_node && !(expr is string_const) && !(expr is char_const) && !(expr is bool_const))
				return null;
			this.on_bracket = on_bracket;
			try
			{
				expr.visit(this);
			}
			catch (Exception e)
			{
				ret_tn = null;
			}
			if (ret_tn != null && ret_tn is ElementScope && (ret_tn as ElementScope).sc is ProcType)
			{
				if (by_dot) 
                    if (((ret_tn as ElementScope).sc as ProcType).target.return_type != null) 
                        ret_tn = new ElementScope(((ret_tn as ElementScope).sc as ProcType).target.return_type);
					else 
                        ret_tn = null;
			}
			else if (ret_tn != null && ret_tn is ElementScope && (ret_tn as ElementScope).sc is ProcScope)
			{
				if (by_dot) 
                    if (((ret_tn as ElementScope).sc as ProcScope).return_type != null) 
                        ret_tn = new ElementScope(((ret_tn as ElementScope).sc as ProcScope).return_type);
					else 
                        ret_tn = null;
			}
            else if (ret_tn != null && ret_tn is ElementScope && (ret_tn as ElementScope).sc is CompiledScope && ((ret_tn as ElementScope).sc as CompiledScope).CompiledType.BaseType == typeof(MulticastDelegate))
            {
                ProcScope invoke_meth = ((ret_tn as ElementScope).sc as CompiledScope).FindNameOnlyInThisType("Invoke") as ProcScope;
                if (invoke_meth != null)
                    ret_tn = new ElementScope(invoke_meth.return_type);
            }
            else if (ret_tn != null && ret_tn is ProcScope)
                if ((ret_tn as ProcScope).return_type == null)
                {
                    if (by_dot) ret_tn = null;
                }
                else if (by_dot) 
                    ret_tn = new ElementScope((ret_tn as ProcScope).return_type);
				
				//else ret_scope = (ret_scope as ElementScope).sc as ProcScope;
			return ret_tn;
		}

		public List<ProcScope> GetOverloadScopes()
		{
			if (expr is const_node && !(expr is string_const) && !(expr is char_const) && !(expr is bool_const))
				return null;
			this.search_all = true;
			this.on_bracket = true;
			List<ProcScope> proces = new List<ProcScope>();
			try
			{
				expr.visit(this);
			}
			catch (Exception e)
			{
				ret_tn = null;
			}
			if (ret_names != null)
			for (int i=0; i<ret_names.Count; i++)
			{
				if (ret_names[i] is ElementScope && (ret_names[i] as ElementScope).sc is ProcScope) 
				{
					ProcScope tmp = (ret_names[i] as ElementScope).sc as ProcScope;
					if (tmp != null)
						proces.Add(tmp);
				}
				else
        		if (ret_names[i] is ElementScope && (ret_names[i] as ElementScope).sc is ProcType) 
        		{
        			ProcScope tmp = ((ret_names[i] as ElementScope).sc as ProcType).target;
        			if (tmp != null)
						proces.Add(tmp);
        		}
				else if (ret_names[i] is ProcScope)
					proces.Add(ret_names[i] as ProcScope);
                else if (ret_names[i] is ElementScope && (ret_names[i] as ElementScope).sc is CompiledScope)
                {
                    //ProcType pt = new ProcType();
                    CompiledScope cs = (ret_names[i] as ElementScope).sc as CompiledScope;
                    ProcScope ps = cs.FindNameOnlyInThisType("Invoke") as ProcScope;
                    if (ps != null)
                        proces.Add(ps);
                }
			}
			return proces;
		}
		
        public override void visit(default_operator node)
        {
            ret_tn = null;
        }

        public override void visit(template_type_name node)
        {
            throw new NotImplementedException();
        }
        
		public override void visit(syntax_tree_node _syntax_tree_node)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(statement_list _statement_list)
		{
            foreach (statement stmt in _statement_list.subnodes)
                stmt.visit(this);
		}
		
		public override void visit(expression _expression)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(assign _assign)
		{
            if (_assign.to is ident && (_assign.to as ident).name == "result")
                _assign.from.visit(this);
		}
		
		public override void visit(bin_expr _bin_expr)
		{
			//throw new NotImplementedException();
			//_bin_expr.left.visit(this);
			if (_bin_expr.operation_type == Operators.As)
			{
				_bin_expr.right.visit(this);
				if (ret_tn != null)
					ret_tn = new ElementScope(ret_tn);
			}
			else if (_bin_expr.operation_type == Operators.Is)
				ret_tn = new ElementScope(TypeTable.bool_type);
			else
			{
				_bin_expr.left.visit(this);
				TypeScope tleft = ret_tn as TypeScope;
				if (tleft == null && ret_tn is ElementScope)
					tleft = (ret_tn as ElementScope).sc as TypeScope;
				_bin_expr.right.visit(this);
				TypeScope tright = ret_tn as TypeScope;
				if (tright == null && ret_tn is ElementScope)
					tright = (ret_tn as ElementScope).sc as TypeScope;
				if (tleft != null && tright != null)
        		{
        			List<SymScope> lst = tleft.FindOverloadNamesOnlyInType
        				(PascalABCCompiler.TreeConverter.name_reflector.get_name(_bin_expr.operation_type));
        			ProcScope ps = select_method(lst.ToArray(),_bin_expr.left,_bin_expr.right);
        			if (ps != null)
        				ret_tn = new ElementScope(ps.return_type);
        			else
        				ret_tn = new ElementScope(tleft);
        		}
        		else if (tleft != null)
        			ret_tn = new ElementScope(tleft);
        		else
        			ret_tn = new ElementScope(tright);
			}
			
		}
		
		public override void visit(un_expr _un_expr)
		{
			//throw new NotImplementedException();
			if (_un_expr.operation_type == Operators.LogicalNOT)
				ret_tn = TypeTable.bool_type;
			else
			{
				_un_expr.subnode.visit(this);
				if (ret_tn != null && ret_tn is TypeScope)
					ret_tn = new ElementScope(ret_tn);
			}
		}
		
		public override void visit(const_node _const_node)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(bool_const _bool_const)
		{
			ret_tn = new ElementScope(TypeTable.bool_type);
		}
		
		public override void visit(int32_const _int32_const)
		{
			ret_tn = new ElementScope(TypeTable.int_type);
		}
		
		public override void visit(double_const _double_const)
		{
			ret_tn = new ElementScope(TypeTable.real_type);
		}
		
		public override void visit(statement _statement)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(subprogram_body _subprogram_body)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(ident _ident)
		{
            if (!search_all)
            {
                ret_tn = null;
                //if (any_order) ret_scope = entry_scope.FindNameInAnyOrder(_ident.name);
                ret_tn = entry_scope.FindName(_ident.name);
                if (ret_tn == null && entry_scope.topScope == null)
                {
                    try
                    {
                        ret_tn = entry_scope.FindNameInAnyOrder(_ident.name);
                    }
                    catch (Exception e)
                    {

                    }
                }
                else if (ret_tn == null && entry_scope.topScope != null)
                {
                    try
                    {
                        ret_tn = entry_scope.topScope.FindNameInAnyOrder(_ident.name);
                        /*if (ret_scope != null)
                        {
                            if (!ret_scope.IsInScope(ret_scope.loc,_ident.source_context.begin_position.line_num,_ident.source_context.begin_position.column_num))
                                ret_scope = null;
                        }*/
                    }
                    catch (Exception e)
                    {

                    }
                }
                if (ret_tn != null)
                {
                    if (ret_tn is ProcScope) ret_tn = new ElementScope(ret_tn);
                    else if (ret_tn is ElementScope && (ret_tn as ElementScope).sc is ProcType && by_dot)
                    {
                        TypeScope ts = ((ret_tn as ElementScope).sc as ProcType).target.return_type;
                        if (ts != null)
                            ret_tn = new ElementScope(ts);
                    }
                    if (ret_tn is ElementScope)
                        ret_tn = CheckForAccess(entry_scope, ret_tn as ElementScope);
                }
            }
            else
            {
                ret_names = null;
                ret_names = entry_scope.FindOverloadNames(_ident.name);
                for (int i = 0; i < ret_names.Count; i++)
                {
                    if (ret_names[i] is ElementScope && ((ret_names[i] as ElementScope).sc is ProcType))
                    {
                        ret_names[i] = CheckForAccess(entry_scope, ret_names[i] as ElementScope);
                        //if (ret_names[i] != null)
                        //ret_names[i] = ((ret_names[i] as ElementScope).sc as ProcType).target;				
                    }
                }
                search_all = false;
            }
		}
		
		public override void visit(addressed_value _addressed_value)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(type_definition _type_definition)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(named_type_reference _named_type_reference)
		{
			//throw new NotImplementedException();
			 ret_tn = entry_scope;
           	 for (int i=0; i<_named_type_reference.names.Count; i++)
           	 {
           	 	ret_tn = ret_tn.FindName(_named_type_reference.names[i].name);
           	 	if (ret_tn == null) 
           	 		return;
           	 }
		}
		
		public override void visit(variable_definitions _variable_definitions)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(ident_list _ident_list)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(var_def_statement _var_def_statement)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(declaration _declaration)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(declarations _declarations)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(program_tree _program_tree)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(program_name _program_name)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(string_const _string_const)
		{
			ret_tn = new ElementScope(entry_scope.FindName(PascalABCCompiler.TreeConverter.compiler_string_consts.string_type_name));
		}
		
		public override void visit(expression_list _expression_list)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(dereference _dereference)
		{
			//throw new NotImplementedException();
			
		}
		
		public override void visit(roof_dereference _roof_dereference)
		{
			//throw new NotImplementedException();
			_roof_dereference.dereferencing_value.visit(this);
			ElementScope es = ret_tn as ElementScope;
			if (es == null) 
			{
				ret_tn = null;
				return;
			}
			if (es.sc != null && es.sc is PointerScope)
			{
				
				ret_tn = new ElementScope((es.sc as PointerScope).ref_type);
			}
			else ret_tn = null;
		}
		
		public override void visit(indexer _indexer)
		{
			//throw new NotImplementedException();
			_indexer.dereferencing_value.visit(this);
			if (ret_tn != null)
			if (ret_tn is ElementScope && (ret_tn as ElementScope).sc is ProcScope && ((ret_tn as ElementScope).sc as ProcScope).return_type != null)
				ret_tn = new ElementScope(((ret_tn as ElementScope).sc as ProcScope).return_type.GetElementType());
			else
				ret_tn = new ElementScope(ret_tn.GetElementType());
		}
		
		public override void visit(for_node _for_node)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(repeat_node _repeat_node)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(while_node _while_node)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(if_node _if_node)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(ref_type _ref_type)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(diapason _diapason)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(indexers_types _indexers_types)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(array_type _array_type)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(label_definitions _label_definitions)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(procedure_attribute _procedure_attribute)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(typed_parameters _typed_parametres)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(formal_parameters _formal_parametres)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(procedure_attributes_list _procedure_attributes_list)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(procedure_header _procedure_header)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(function_header _function_header)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(procedure_definition _procedure_definition)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(type_declaration _type_declaration)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(type_declarations _type_declarations)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(simple_const_definition _simple_const_definition)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(typed_const_definition _typed_const_definition)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(const_definition _const_definition)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(consts_definitions_list _consts_definitions_list)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(unit_name _unit_name)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(unit_or_namespace _unit_or_namespace)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(uses_unit_in _uses_unit_in)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(uses_list _uses_list)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(program_body _program_body)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(compilation_unit _compilation_unit)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(unit_module _unit_module)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(program_module _program_module)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(hex_constant _hex_constant)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(get_address _get_address)
		{
			//throw new NotImplementedException();
			ret_tn = null;
		}
		
		public override void visit(case_variant _case_variant)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(case_node _case_node)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(method_name _method_name)
		{
			throw new NotImplementedException();
		}
		
		public bool CheckForBaseAccess(SymScope cur, SymScope sc)
		{
			if (IsInOneModule(cur, sc)) return true;
			if (cur == sc) return true;
			if (cur is CompiledScope && sc is CompiledScope && (cur as CompiledScope).ctn == (sc as CompiledScope).ctn) return true;
			if (cur == null) return false;
			if (cur is TypeScope)
			{
				return CheckForBaseAccess((cur as TypeScope).baseScope, sc);
			}
			else return CheckForBaseAccess(cur.topScope,sc);
		}
		
		public bool CheckPrivateInSameUnit(SymScope cur, SymScope sc)
		{
			return true;
		}
		
		public bool IsInOneModule(SymScope cur, SymScope sc)
		{
			if (cur == sc) return true;
			if (cur == null) return false;
			SymScope tmp = cur.topScope;
			if (tmp == null)
				tmp = cur;
			if (sc.topScope == null) return false;
			while (tmp != null)
			{
				if (tmp == sc.topScope) return true;
				tmp = tmp.topScope;
			}
			return false;
		}
		
		public bool CheckPrivateForBaseAccess(SymScope cur, SymScope sc)
		{
			//if (CheckPrivateInSameUnit(cur, sc)) return true;
			if (IsInOneModule(cur, sc)) return true;
			if (cur == sc) return true;
			if (cur == null) return false;
			
			return CheckPrivateForBaseAccess(cur.topScope,sc);
		}
		
		public SymScope CheckForAccess(ElementScope ss, ElementScope es)
		{
			if (es.acc_mod == access_modifer.none || es.acc_mod == access_modifer.public_modifer || es.acc_mod == access_modifer.published_modifer || es.acc_mod == access_modifer.internal_modifer)
				return es;
			if (es.acc_mod == access_modifer.private_modifer)
				//if (entry_scope == ss.sc) return es; else 
				if (es.topScope is TypeScope && CheckPrivateForBaseAccess(entry_scope,es.topScope)) return es;
			if (es.acc_mod == access_modifer.protected_modifer)
				//if (entry_scope == ss.sc || CheckForBaseAccess(entry_scope,ss.sc))
				if (es.topScope is TypeScope && CheckForBaseAccess(entry_scope,es.topScope)) return es;
				//return es;
			return null;
		}
		
		public SymScope CheckForAccess(TypeScope ss, ElementScope es)
		{
			if (es.acc_mod == access_modifer.none || es.acc_mod == access_modifer.public_modifer || es.acc_mod == access_modifer.published_modifer || es.acc_mod == access_modifer.internal_modifer)
				return es;
			if (es.acc_mod == access_modifer.private_modifer)
				if (entry_scope == ss) return es; else if (CheckPrivateForBaseAccess(entry_scope,ss)) return es;
			if (es.acc_mod == access_modifer.protected_modifer)
				if (entry_scope == ss || CheckForBaseAccess(entry_scope,ss))
				return es;
			return null;
		}
		
		public SymScope CheckForAccess(SymScope ss, ElementScope es)
		{
			if (ss == null) return null;
			if (ss is ProcScope) return CheckForAccess(ss.topScope,es);
			if (ss is TypeScope) 
			if (es.topScope is TypeScope) return CheckForAccess(es.topScope as TypeScope,es);
			return es;
		}
		
		public override void visit(dot_node _dot_node)
		{
			bool tmp = by_dot;
			by_dot = true;
			bool tmp2 = search_all;
			search_all = false;
			_dot_node.left.visit(this);
			search_all = tmp2;
			by_dot = tmp;
			if (ret_tn != null && ret_tn is ElementScope && (ret_tn as ElementScope).sc is ProcScope)
			{
				if (((ret_tn as ElementScope).sc as ProcScope).return_type != null) 
					ret_tn = new ElementScope(((ret_tn as ElementScope).sc as ProcScope).return_type);
				else 
					ret_tn = null;
			}
			else if (ret_tn != null && ret_tn is ProcScope)
			{
				if ((ret_tn as ProcScope).return_type == null) 
				{
					ret_tn = null;
				}
				else 
					ret_tn = new ElementScope((ret_tn as ProcScope).return_type);
			}
			else if (ret_tn != null && ret_tn is ElementScope && (ret_tn as ElementScope).sc is ProcType)
			{
				TypeScope ts = ((ret_tn as ElementScope).sc as ProcType).target.return_type;
				if (ts != null)
					ret_tn = new ElementScope(ts);
			}
			
			if (ret_tn != null)
			{
                if (!search_all)
                {
                    SymScope left_scope = ret_tn;

                    if (_dot_node.right is ident)
                    {
                        SymScope tmp_tn = ret_tn;
                        ret_tn = ret_tn.FindNameOnlyInType((_dot_node.right as ident).name);
                        if (ret_tn != null && ret_tn is ElementScope)
                        {
                            if (left_scope is ElementScope)
                                ret_tn = CheckForAccess(left_scope as ElementScope, ret_tn as ElementScope);
                            else if (left_scope is TypeScope)
                                ret_tn = CheckForAccess(left_scope as TypeScope, ret_tn as ElementScope);
                            return;
                        }
                        if (ret_tn != null && ret_tn is ProcScope && (ret_tn as ProcScope).return_type != null)
                        {
                            CompiledMethodScope tmp_sc = ret_tn as CompiledMethodScope;
                            ret_tn = new ElementScope(ret_tn as ProcScope);
                            if (tmp_sc != null)
                                ret_tn.topScope = tmp_sc.topScope;
                            if (left_scope is ElementScope)
                                ret_tn = CheckForAccess(left_scope as ElementScope, ret_tn as ElementScope);
                            else if (left_scope is TypeScope)
                                ret_tn = CheckForAccess(left_scope as TypeScope, ret_tn as ElementScope);
                            return;
                        }
                        if (tmp_tn is ElementScope && stv != null)
                        {
                            List<ProcScope> procs = stv.entry_scope.GetExtensionMethods((_dot_node.right as ident).name, (tmp_tn as ElementScope).sc as TypeScope);
                            if (procs.Count > 0)
                            {
                                ret_tn = procs[0];
                                return;
                            }
                        }
                    }
                }
                else
                {
                    SymScope left_scope = ret_tn;
                    ret_names = ret_tn.FindOverloadNamesOnlyInType((_dot_node.right as ident).name);
                    for (int i = 0; i < ret_names.Count; i++)
                    {
                        if (ret_names[i] is ElementScope && (ret_names[i] as ElementScope).sc is ProcType)
                        {
                            if (left_scope is ElementScope)
                                ret_names[i] = CheckForAccess(left_scope as ElementScope, ret_names[i] as ElementScope);
                            //if (ret_names[i] != null)
                            //ret_names[i] = ((ret_names[i] as ElementScope).sc as ProcType).target;
                        }
                    }
                    if (ret_tn is ElementScope && stv != null)
                    {
                        List<ProcScope> procs = stv.entry_scope.GetExtensionMethods((_dot_node.right as ident).name,(ret_tn as ElementScope).sc as TypeScope);
                        for (int i = 0; i < procs.Count; i++)
                            ret_names.Add(procs[i]);
                    }
                    
                    search_all = false;
                }
			}
		}
		
		public override void visit(empty_statement _empty_statement)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(goto_statement _goto_statement)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(labeled_statement _labeled_statement)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(with_statement _with_statement)
		{
			throw new NotImplementedException();
		}
		
		private ProcScope select_method(SymScope[] procs, params expression[] args)
        {
        	List<SymScope> arg_types = new List<SymScope>();
        	if (args != null)
        	foreach (expression e in args)
        	{
        		bool tmp = by_dot;
        		e.visit(this);
        		ret_names.Clear();
        		if (ret_tn != null)
        		{
        			if (ret_tn is ElementScope)
        			{
        				ret_tn = (ret_tn as ElementScope).sc;
        				if (ret_tn is ProcScope)
        					ret_tn = (ret_tn as ProcScope).return_type;
        			}
        			else if (ret_tn is ProcScope)
        				ret_tn = (ret_tn as ProcScope).return_type;
        		}
        		arg_types.Add(ret_tn);
        	}
        	List<ProcScope> good_procs = new List<ProcScope>();
        	for (int i=0; i<procs.Length; i++)
        	{
        		/*if (procs[i] is ElementScope && (procs[i] as ElementScope).sc is ProcType)
        		{
        			procs[i] = ((procs[i] as ElementScope).sc as ProcType).target;
        		}*/
        		if (procs[i] is ProcScope)
        		{
        			if (DomSyntaxTreeVisitor.is_good_overload(procs[i] as ProcScope, arg_types))
        			if (!procs[i].si.not_include || by_dot)
        				good_procs.Add(procs[i] as ProcScope);
        		}
        	}
        	if (good_procs.Count > 1)
        	for (int i=0; i<good_procs.Count; i++)
        	if (DomSyntaxTreeVisitor.is_good_exact_overload(good_procs[i] as ProcScope, arg_types))
        		return good_procs[i];
        	if (good_procs.Count > 0)
        		return good_procs[0];
        	return null;
        }
		
		public override void visit(method_call _method_call)
		{
			ret_names.Clear();
			search_all = true;
			_method_call.dereferencing_value.visit(this);
            search_all = false;
			SymScope[] names = ret_names.ToArray();
            if (names.Length > 0 && names[0] is ElementScope && ((names[0] as ElementScope).sc is ProcType || (names[0] as ElementScope).sc is CompiledScope))
			{
				ret_tn = names[0];
				return;
			}
			ProcScope ps = select_method(names,_method_call.parameters!=null?_method_call.parameters.expressions.ToArray():null);
			ret_tn = ps;
			if (ps == null && names.Length > 0)
        	{
        		ret_tn = null;
        		foreach (SymScope ss in names)
        		if (ss is ProcScope)
        		{
        			ret_tn = ss;
        			break;
        		}
        		else if (ss is TypeScope)
        		{
        			ret_tn = new ElementScope(ss);
                    return ;
        		}	 
        	}
			if (ret_tn != null)
			{
				if (ret_tn is ProcScope)
				{
					ps = ret_tn as ProcScope;
					if (by_dot)
					{
						if (ps.return_type != null)
							ret_tn = new ElementScope(ps.return_type);
						else ret_tn = null;
					}
					else
						ret_tn = new ElementScope(ps);
				}
				else if (ret_tn is ProcRealization)
				{
					if ((ret_tn as ProcRealization).def_proc != null && (ret_tn as ProcRealization).def_proc.return_type != null)
					ret_tn = new ElementScope((ret_tn as ProcRealization).def_proc.return_type);
					else ret_tn = null;
				}
				else if (ret_tn is CompiledMethodScope)
				{
					if ((ret_tn as CompiledMethodScope).return_type != null)
					ret_tn = new ElementScope((ret_tn as CompiledMethodScope).return_type);
					else ret_tn = null;
				}
				/*else if (ret_tn is ElementScope && (ret_tn as ElementScope).sc is ProcScope)
				{
					ret_tn = new ElementScope(((ret_tn as ElementScope).sc as ProcScope).return_type);
				}
				else if (ret_tn is ElementScope && (ret_tn as ElementScope).sc is ProcType)
				{
					TypeScope ts = ((ret_tn as ElementScope).sc as ProcType).target.return_type;
					if (ts != null)
						ret_tn = new ElementScope(ts);
				}
				else ret_tn = null;*/
			}
		}
		
		public override void visit(pascal_set_constant _pascal_set_constant)
		{
			//throw new NotImplementedException();
			ret_tn = null;
		}
		
		public override void visit(array_const _array_const)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(write_accessor_name _write_accessor_name)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(read_accessor_name _read_accessor_name)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(property_accessors _property_accessors)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(simple_property _simple_property)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(index_property _index_property)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(class_members _class_members)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(access_modifer_node _access_modifer_node)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(class_body _class_body)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(class_definition _class_definition)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(default_indexer_property_node _default_indexer_property_node)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(known_type_definition _known_type_definition)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(set_type_definition _set_type_definition)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(try_statement _try_statement)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(on_exception _on_exception)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(on_exception_list _on_exception_list)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(try_finally_statement _try_finally_statement)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(try_except_statement _try_except_statement)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(record_const_definition _record_const_definition)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(record_const _record_const)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(record_type _record_type)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(enum_type_definition _enum_type_definition)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(char_const _char_const)
		{
			//throw new NotImplementedException();
			ret_tn = new ElementScope(TypeTable.char_type);
		}
		
		public override void visit(raise_statement _raise_statement)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(sharp_char_const _sharp_char_const)
		{
			ret_tn = new ElementScope(TypeTable.char_type);
		}
		
		public override void visit(literal_const_line _literal_const_line)
		{
			ret_tn = new ElementScope(TypeTable.string_type);
		}
		
		public override void visit(string_num_definition _string_num_definition)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(variant _variant)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(variant_list _variant_list)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(variant_type _variant_type)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(variant_types _variant_types)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(variant_record_type _variant_record_type)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(procedure_call _procedure_call)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(class_predefinition _class_predefinition)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(nil_const _nil_const)
		{
			//throw new NotImplementedException();
			ret_tn = new NullTypeScope();
		}
		
		public override void visit(file_type_definition _file_type_definition)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(constructor _constructor)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(destructor _destructor)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(inherited_method_call _inherited_method_call)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(typecast_node _typecast_node)
		{
			if (_typecast_node.cast_op == op_typecast.as_op || _typecast_node.cast_op == op_typecast.typecast)
			{
				_typecast_node.type_def.visit(this);
				if (ret_tn != null) ret_tn = new ElementScope(ret_tn);				
			}
			else if (_typecast_node.cast_op == op_typecast.is_op)
				ret_tn = new ElementScope(entry_scope.FindName(PascalABCCompiler.TreeConverter.compiler_string_consts.bool_type_name));
		}
		
		public override void visit(interface_node _interface_node)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(implementation_node _implementation_node)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(diap_expr _diap_expr)
		{
			ret_tn = null;
		}
		
		public override void visit(block _block)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(proc_block _proc_block)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(array_of_named_type_definition _array_of_named_type_definition)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(array_of_const_type_definition _array_of_const_type_definition)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(literal _literal)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(case_variants _case_variants)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(diapason_expr _diapason_expr)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(var_def_list _var_def_list)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(record_type_parts _record_type_parts)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(property_array_default _property_array_default)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(property_interface _property_interface)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(property_parameter _property_parameter)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(property_parameter_list _property_parameter_list)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(inherited_ident _inherited_ident)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(format_expr _format_expr)
		{
			ret_tn = entry_scope.FindName(PascalABCCompiler.TreeConverter.compiler_string_consts.string_type_name);
		}
		
		public override void visit(initfinal_part _initfinal_part)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(token_info _token_info)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(compiler_directive_if _compiler_directive_if)
        {
        	
        }
        
        public override void visit(compiler_directive_list _compiler_directive_list)
        {
        	
        }
        
		public override void visit(raise_stmt _raise_stmt)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(op_type_node _op_type_node)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(file_type _file_type)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(known_type_ident _known_type_ident)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(exception_handler _exception_handler)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(exception_ident _exception_ident)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(exception_handler_list _exception_handler_list)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(exception_block _exception_block)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(try_handler _try_handler)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(try_handler_finally _try_handler_finally)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(try_handler_except _try_handler_except)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(try_stmt _try_stmt)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(inherited_message _inherited_message)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(external_directive _external_directive)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(using_list _using_list)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(oberon_import_module _oberon_import_module)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(oberon_module _oberon_module)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(oberon_ident_with_export_marker _oberon_ident_with_export_marker)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(oberon_exit_stmt _oberon_exit_stmt)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(jump_stmt _jump_stmt)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(oberon_procedure_receiver _oberon_procedure_receiver)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(oberon_procedure_header _oberon_procedure_header)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(oberon_withstmt_guardstat _oberon_withstmt_guardstat)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(oberon_withstmt_guardstat_list _oberon_withstmt_guardstat_list)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(oberon_withstmt _oberon_withstmt)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(loop_stmt _loop_stmt)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(foreach_stmt _foreach_stmt)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(addressed_value_funcname _addressed_value_funcname)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(named_type_reference_list _named_type_reference_list)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(template_param_list _template_param_list)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(template_type_reference _template_type_reference)
		{
			 ret_tn = entry_scope;
           	 for (int i=0; i<_template_type_reference.name.names.Count; i++)
           	 {
           	 	ret_tn = ret_tn.FindName(_template_type_reference.name.names[i].name);
           	 	if (ret_tn == null) 
           	 		return;
           	 }
           	 if (ret_tn is TypeScope)
           	 {
           	 	TypeScope ts = ret_tn as TypeScope;
           	 	List<TypeScope> gen_args = new List<TypeScope>();
           	 	foreach (type_definition td in _template_type_reference.params_list.params_list)
           	 	{
           	 		td.visit(this);
           	 		gen_args.Add(ret_tn as TypeScope);
           	 	}
           	 	ret_tn = ts.GetInstance(gen_args);
           	 }
           	 else
           	 	ret_tn = null;
		}
		
		public override void visit(int64_const _int64_const)
		{
			ret_tn = new ElementScope(TypeTable.int64_type);
		}
		
		public override void visit(uint64_const _uint64_const)
		{
			ret_tn = new ElementScope(TypeTable.uint64_type);
		}
		
		public override void visit(new_expr _new_expr)
		{
			_new_expr.type.visit(this);
			if (ret_tn != null && ret_tn is TypeScope)
			{
				TypeScope ts = ret_tn as TypeScope;
				if (_new_expr.new_array)
				{
					ts = new ArrayScope(ts,new TypeScope[1]{TypeTable.int_type});
					(ts as ArrayScope).is_dynamic_arr = true;
					ret_tn = ts;
					return;
				}
				ProcScope tmp = ts.GetConstructor();
				List<ProcScope> cnstrs = ts.GetConstructors(false);
				if (search_all)
					ret_names.AddRange(cnstrs.ToArray());
				if (!on_bracket)
				{
					ProcScope[] constrs = cnstrs.ToArray();
					ProcScope ps = select_method(constrs,_new_expr.params_list!=null?_new_expr.params_list.expressions.ToArray():null);
					if (ps != null)
						ret_tn = ps;
					else
						ret_tn = tmp;
				}
				else
				ret_tn = tmp;
			}
		}
		
		public override void visit(type_definition_list _type_definition_list)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(where_definition _where_definition)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(where_definition_list _where_definition_list)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(sizeof_operator _sizeof_operator)
		{
			ret_tn = new ElementScope(entry_scope.FindName(PascalABCCompiler.TreeConverter.compiler_string_consts.integer_type_name));
		}
		
		public override void visit(typeof_operator _typeof_operator)
		{
			ret_tn = new ElementScope(TypeTable.get_compiled_type(new SymInfo("Type", SymbolKind.Type,"System.Type"),typeof(Type)));
		}
		
		public override void visit(compiler_directive _compiler_directive)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(operator_name_ident _operator_name_ident)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(var_statement _var_statement)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(question_colon_expression _question_colon_expression)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(expression_as_statement _expression_as_statement)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(c_scalar_type _c_scalar_type)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(c_module _c_module)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(declarations_as_statement _declarations_as_statement)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(array_size _array_size)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(enumerator _enumerator)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(enumerator_list _enumerator_list)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(c_for_cycle _c_for_cycle)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(switch_stmt _switch_stmt)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(type_definition_attr_list _type_definition_attr_list)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(type_definition_attr _type_definition_attr)
		{
			throw new NotImplementedException();
		}
		
		public override void visit(lock_stmt _lock_stmt)
		{
			throw new NotImplementedException();
		}
        public override void visit(documentation_comment_list node)
        {
            throw new NotImplementedException();
        }
        public override void visit(documentation_comment_section node)
        {
            throw new NotImplementedException();
        }
        public override void visit(documentation_comment_tag node)
        {
            throw new NotImplementedException();
        }
        public override void visit(documentation_comment_tag_param node)
        {
            throw new NotImplementedException();
        }
        public override void visit(token_taginfo node)
        {
            throw new NotImplementedException();
        }
        public override void visit(declaration_specificator node)
        {
            throw new NotImplementedException();
        }

        public override void visit(ident_with_templateparams node)
        {
        	node.name.visit(this);
        	if (this.ret_tn != null)
        	{
        		if (this.ret_tn is TypeScope)
        		{
        			TypeScope ts = ret_tn as TypeScope;
        			List<TypeScope> instances = new List<TypeScope>();
        			foreach (type_definition td in node.template_params.params_list)
        			{
        				td.visit(this);
        				if (ret_tn != null && ret_tn is TypeScope)
        					instances.Add(ret_tn as TypeScope);
        			}
        			ret_tn = ts.GetInstance(instances);
        		}
        	}
        }
        
        public override void visit(bracket_expr _bracket_expr)
        {
        	_bracket_expr.expr.visit(this);
        }
        
        public override void visit(attribute _attribute)
        {
        	
        }
        
        public override void visit(attribute_list _attribute_list)
        {
        	
        }
        
        public override void visit(simple_attribute_list _simple_attribute_list)
        {
        	
        }

        public override void visit(function_lambda_definition _function_lambda_definition)
        {
            ProcScope ps = new ProcScope(_function_lambda_definition.lambda_name, null);
            if (_function_lambda_definition.ident_list != null)
                foreach (ident id in _function_lambda_definition.ident_list.idents)
                    ps.AddParameter(new ElementScope(new SymInfo(id.name, SymbolKind.Parameter, ""), new UnknownScope(new SymInfo("", SymbolKind.Type, "")), ps));
            _function_lambda_definition.proc_body.visit(this);
            if (ret_tn is ElementScope)
                ret_tn = (ret_tn as ElementScope).sc;
            ps.return_type = ret_tn as TypeScope;
            ret_tn = new ProcType(ps);
        }
        public override void visit(function_lambda_call _function_lambda_call)
        {
            //
        }
        public override void visit(semantic_check _semantic_check)
        {
        }
		public override void visit(lambda_inferred_type lit) //lroman//
        {
        }
        public override void visit(same_type_node stn) //SS 22/06/13//
        {
        }
        public override void visit(name_assign_expr _name_assign_expr) // SSM 27.06.13
        {
        }
        public override void visit(name_assign_expr_list _name_assign_expr_list) // SSM 27.06.13
        {
        }
        public override void visit(unnamed_type_object _unnamed_type_object) // SSM 27.06.13
        {
        }
        public override void visit(semantic_type_node stn) // SSM 
        {
        }
    }
}
