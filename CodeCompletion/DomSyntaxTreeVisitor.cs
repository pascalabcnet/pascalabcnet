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
	public class SemanticOptions
	{
		public bool allow_import_types=true;
	}
	
	public class DomSyntaxTreeVisitor : AbstractVisitor
    {
		private SymScope returned_scope;//vozvrashaemyj scope posle vyzova visit
        private Stack<SymScope> saved_returned_scopes = new Stack<SymScope>();//nuzhen dlja lambd
		private List<SymScope> returned_scopes = new List<SymScope>();//hranit spisok scopov (metodov) posle vyzova visit, esli search_all=true
        public SymScope entry_scope;//korenvoj scope unita
		public SymScope impl_scope;
		private string cur_type_name;
		private location cur_loc;
		private DomConverter converter;
		private bool is_proc_realization=false;
		private string meth_name;
		private string cur_unit_file_name;
		private RetValue cnst_val;
		private ExpressionEvaluator ev = new ExpressionEvaluator();
		public bool add_doc_from_text=true;
		private bool search_all = false;//flag, iskat vse peregruzki ili tolko odnu. sdelal dlja effektivnosti. true zadaetsja tolko v visit(method_call)
        internal bool parse_only_interface = false;
        private template_type_reference converted_template_type = null;

		public SemanticOptions semantic_options = new SemanticOptions();
		
		public DomSyntaxTreeVisitor(DomConverter converter)
		{
			//tcst = new TreeConverterSymbolTable(false);
			this.converter = converter;
		}

        public override void visit(default_operator node)
        {
        	node.type_name.visit(this);
        }

		public void Convert(compilation_unit cu)
        {
            try
            {
				cu.visit(this);
            }
            catch(Exception e)
            {
#if DEBUG
                File.AppendAllText("log.txt", e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
#endif
                //throw e;
            	//System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
        }
		
		public SymScope FindScopeByLocation(int line, int col)
		{
			try
			{
				SymScope ss = entry_scope.FindScopeByLocation(line,col);
				if (ss != null) return ss;
				if (impl_scope != null) return impl_scope.FindScopeByLocation(line,col);
				//if (impl_scope != null) return impl_scope.FindScopeByLocation(line,col);
			}
			catch
			{
				
			}
			return null;
		}
		
        private void save_return_value()
        {
            saved_returned_scopes.Push(returned_scope);
        }

        private void restore_return_value()
        {
            returned_scope = saved_returned_scopes.Pop();
        }

        public override void visit(syntax_tree_node _syntax_tree_node)
        {
            throw new Exception("The method or operation is not implemented.");
        }
		
        private bool HasVariablesInBlock(statement_list _statement_list)
        {
        	foreach (statement stmt in _statement_list.subnodes)
        		if (stmt is var_statement) return true;
        	return false;
        }

        private bool has_lambdas(expression node)
        {
            if (node is function_lambda_definition)
                return true;
            else if (node is dot_node)
            {
                dot_node dn = node as dot_node;
                if (has_lambdas(dn.left))
                    return true;
                if (has_lambdas(dn.right))
                    return true;
            }
            else if (node is method_call)
            {
                method_call mc = node as method_call;
                if (has_lambdas(mc.dereferencing_value))
                    return true;
                if (mc.parameters != null)
                foreach (expression e in mc.parameters.expressions)
                {
                    if (has_lambdas(e))
                        return true;
                }
            }
            else if (node is tuple_node)
            {
                tuple_node tn = node as tuple_node;
                foreach (expression e in tn.el.expressions)
                {
                    if (has_lambdas(e))
                        return true;
                }
            }
            return false;
        }

        public override void visit(template_type_name node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(statement_list _statement_list)
        {
        	SymScope tmp = cur_scope;
        	
        	if (HasVariablesInBlock(_statement_list))
        	{
        		SymScope stmt_scope = new BlockScope(cur_scope);
        		cur_scope.AddName("$block_scope",stmt_scope);
        		stmt_scope.loc = get_location(_statement_list);
        		cur_scope = stmt_scope;
        	}
        	//try
        	{
        		foreach (statement stmt in _statement_list.subnodes)
        			stmt.visit(this);
        	}
        	//catch(Exception e)
        	{
        		
        	}
        	cur_scope = tmp;
        }

        public override void visit(expression _expression)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(assign _assign)
        {
            if (_assign.from is function_lambda_definition)
            {
                _assign.to.visit(this);
                TypeScope tmp_awaitedProcType = awaitedProcType;
                if (returned_scope != null && returned_scope is TypeScope)
                {
                    if (returned_scope is ProcScope)
                        returned_scope = new ProcType(returned_scope as ProcScope);
                    awaitedProcType = returned_scope as TypeScope;
                }
                _assign.from.visit(this);
                awaitedProcType = tmp_awaitedProcType;
            }
            else if (has_lambdas(_assign.from))
                _assign.from.visit(this);
        }

        public override void visit(bin_expr _bin_expr)
        {
            try
            {
                _bin_expr.left.visit(this);
                RetValue left = cnst_val;
                TypeScope tleft = returned_scope as TypeScope;
                cnst_val.prim_val = null;
                _bin_expr.right.visit(this);
                TypeScope tright = returned_scope as TypeScope;
                RetValue right = cnst_val;
                cnst_val.prim_val = null;
                if (left.prim_val != null && right.prim_val != null)
                {
                    ev.eval_stack.Push(left);
                    ev.eval_stack.Push(right);
                    switch (_bin_expr.operation_type)
                    {
                        case Operators.Plus: ev.EvalPlus(); break;
                        case Operators.Minus: ev.EvalMinus(); break;
                        case Operators.Multiplication: ev.EvalMult(); break;
                        case Operators.Division: ev.EvalDiv(); break;
                        case Operators.IntegerDivision: ev.EvalIDiv(); break;
                        case Operators.BitwiseAND: ev.EvalAnd(); break;
                        case Operators.BitwiseOR: ev.EvalOr(); break;
                        case Operators.BitwiseXOR: ev.EvalXor(); break;
                        case Operators.BitwiseLeftShift: ev.EvalBitwiseLeft(); break;
                        case Operators.BitwiseRightShift: ev.EvalBitwiseRight(); break;
                        case Operators.Equal: ev.EvalEqual(); break;
                        case Operators.NotEqual: ev.EvalNotEqual(); break;
                        case Operators.Less: ev.EvalLess(); break;
                        case Operators.LessEqual: ev.EvalLessEqual(); break;
                        case Operators.Greater: ev.EvalGreater(); break;
                        case Operators.GreaterEqual: ev.EvalGreaterEqual(); break;
                        case Operators.LogicalAND: ev.EvalAnd(); break;
                        case Operators.LogicalOR: ev.EvalOr(); break;
                        case Operators.ModulusRemainder: ev.EvalRem(); break;
                        default:
                            ev.eval_stack.Clear();
                            break;
                    }
                    if (ev.eval_stack.Count > 0)
                    {
                        cnst_val = ev.eval_stack.Pop();
                        returned_scope = TypeTable.get_type(cnst_val.prim_val);
                    }
                    else
                    {
                        cnst_val.prim_val = null;

                    }
                }
                else
                {
                    cnst_val.prim_val = null;
                }

                if (tleft != null && tright != null)
                {
                    RetValue tmp = cnst_val;
                    string name = PascalABCCompiler.TreeConverter.name_reflector.get_name(_bin_expr.operation_type);
                    List<SymScope> lst = tleft.FindOverloadNamesOnlyInType(name);
                    List<SymScope> lst_right = tright.FindOverloadNamesOnlyInType(name);
                    if (lst.Count == 0 && !char.IsLetter(name[0]))
                        lst = tleft.FindOverloadNamesOnlyInType("operator" + name);
                    if (lst_right.Count == 0 && !char.IsLetter(name[0]))
                        lst_right = tright.FindOverloadNamesOnlyInType("operator" + name);
                    if (!char.IsLetter(name[0]))
                        name = "operator" + name;
                    else
                        name = "operator " + name;
                    List<ProcScope> meths = entry_scope.GetExtensionMethods(name, tleft);
                    foreach (ProcScope meth in meths)
                        lst.Add(meth);
                    lst.AddRange(lst_right);
                    meths = entry_scope.GetExtensionMethods(name, tright);
                    foreach (ProcScope meth in meths)
                        lst.Add(meth);
                    ProcScope ps = select_method(lst.ToArray(), tleft, tright, null, false, _bin_expr.left, _bin_expr.right);
                    if (ps != null)
                        returned_scope = ps.return_type;
                    else
                    {
                        if (_bin_expr.operation_type == Operators.Division)
                            returned_scope = TypeTable.real_type;
                        else
                            returned_scope = tleft;
                    }
                        
                    cnst_val = tmp;
                }
                else if (tleft != null)
                    returned_scope = tleft;
                else
                    returned_scope = tright;
            }
            catch (Exception e)
            {
#if DEBUG
                File.AppendAllText("log.txt", e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
#endif
                cnst_val.prim_val = null;
                ev.eval_stack.Clear();
                returned_scope = null;
            }

        }

        public override void visit(un_expr _un_expr)
        {
            try
            {
                _un_expr.subnode.visit(this);
                TypeScope tleft = returned_scope as TypeScope;
                if (cnst_val.prim_val != null)
                {
                    ev.eval_stack.Push(cnst_val);
                    switch (_un_expr.operation_type)
                    {
                        case Operators.BitwiseNOT:
                            ev.EvalNot(); break;
                        case Operators.LogicalNOT:
                            ev.EvalNot(); returned_scope = entry_scope.FindName(PascalABCCompiler.TreeConverter.compiler_string_consts.bool_type_name); break;
                        case Operators.Minus:
                            ev.EvalUnmin(); break;
                    }
                    if (ev.eval_stack.Count > 0)
                    {
                        cnst_val = ev.eval_stack.Pop();
                        returned_scope = TypeTable.get_type(cnst_val.prim_val);
                    }
                    else
                    {
                        cnst_val.prim_val = null;
                        returned_scope = tleft;
                    }
                }
            }
            catch (Exception e)
            {
                cnst_val.prim_val = null;
                ev.eval_stack.Clear();
                returned_scope = null;
            }
			
        }

        public override void visit(token_taginfo node)
        {
        }
        public override void visit(declaration_specificator node)
        {
        }


        public override void visit(const_node _const_node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(bool_const _bool_const)
        {
            returned_scope = TypeTable.bool_type;//entry_scope.FindName(PascalABCCompiler.TreeConverter.compiler_string_consts.bool_type_name);
            cnst_val.prim_val = _bool_const.val;
        }

        public override void visit(int32_const _int32_const)
        {
        	returned_scope = TypeTable.int_type;//entry_scope.FindName(PascalABCCompiler.TreeConverter.compiler_string_consts.integer_type_name);
        	cnst_val.prim_val = _int32_const.val;
        }

        public override void visit(double_const _double_const)
        {
            returned_scope = TypeTable.real_type;//entry_scope.FindName(PascalABCCompiler.TreeConverter.compiler_string_consts.real_type_name);
            cnst_val.prim_val = _double_const.val;
        }

        public override void visit(statement _statement)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(subprogram_body _subprogram_body)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(ident _ident)
        {
            if (!search_all)
            {
                returned_scope = cur_scope.FindName(_ident.name);
                if (returned_scope == null)
                {
                    //cnst_val.prim_val = _ident.name;
                    cnst_val.prim_val = null;
                    return;
                }
                if (returned_scope is ElementScope)
                {
                    cnst_val.prim_val = (returned_scope as ElementScope).cnst_val;
                    returned_scope = (returned_scope as ElementScope).sc;
                    return;
                }
                else
                    if (returned_scope is ProcScope)
                {
                    ProcScope ps = returned_scope as ProcScope;
                    if (ps.parameters.Count == 0 || ps.parameters[0].param_kind == parametr_kind.params_parametr || ps.parameters[0].cnst_val != null)
                    {
                        if (ps.return_type != null)
                            returned_scope = ps.return_type;
                        else
                            returned_scope = new ProcType(ps);
                    }
                    else
                    {
                        returned_scopes = cur_scope.FindOverloadNames(_ident.name);
                        returned_scope = returned_scopes.Find(x => x is ProcScope && ((x as ProcScope).parameters.Count == 0 || (x as ProcScope).parameters[0].param_kind == parametr_kind.params_parametr || (x as ProcScope).parameters[0].cnst_val != null));
                        if (returned_scope == null)
                        {
                            returned_scope = returned_scopes[0];
                            if (returned_scope is ProcScope && (returned_scope as ProcScope).return_type == null)
                                returned_scope = new ProcType(returned_scope as ProcScope);
                        }
                        else if (returned_scope is ProcScope && (returned_scope as ProcScope).return_type != null)
                            returned_scope = (returned_scope as ProcScope).return_type;
                        else if (returned_scopes.Count > 0 && returned_scopes[0] is ProcScope && (returned_scopes[0] as ProcScope).return_type == null)
                            returned_scope = new ProcType(returned_scopes[0] as ProcScope);
                    }
                }
                else if (returned_scope is TypeScope)
                    is_type = true;
            }
            else
            {
                returned_scopes = cur_scope.FindOverloadNames(_ident.name);
                for (int i = 0; i < returned_scopes.Count; i++)
                {
                    if (returned_scopes[i] is ElementScope)
                    {
                        cnst_val.prim_val = (returned_scopes[i] as ElementScope).cnst_val;
                        returned_scope = (returned_scopes[i] as ElementScope).sc;
                        returned_scopes[i] = returned_scope;
                        return;
                    }
                    else
                    if (returned_scopes[i] is TypeScope)
                    {
                        is_type = true;
                    }
                }
                search_all = false;
            }
            //cnst_val.prim_val = _ident.name;
        }

        public override void visit(addressed_value _addressed_value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(type_definition _type_definition)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(named_type_reference _named_type_reference)
        {
            //throw new Exception("The method or operation is not implemented.");
            returned_scope = cur_scope;
            string suffix = "";
            SymScope tmp_scope = returned_scope;
            for (int i = 0; i < _named_type_reference.names.Count; i++)
            {
                if (i == _named_type_reference.names.Count - 1 && converted_template_type != null)
                    suffix = "`" + converted_template_type.params_list.params_list.Count;
                if (i > 0)
                    returned_scope = returned_scope.FindNameOnlyInType(_named_type_reference.names[i].name+suffix);
                else
                    returned_scope = returned_scope.FindName(_named_type_reference.names[i].name+suffix);
                if (returned_scope == null)
                {
                    if (suffix != "")
                    {
                        returned_scope = tmp_scope;
                        if (i > 0)
                            returned_scope = returned_scope.FindNameOnlyInType(_named_type_reference.names[i].name);
                        else
                            returned_scope = returned_scope.FindName(_named_type_reference.names[i].name);
                        if (returned_scope == null)
                        {
                            break;
                        }
                    }
                    else
                        break;
                }
                tmp_scope = returned_scope;
            }

            if (returned_scope == null || !(returned_scope is TypeScope))
                returned_scope = new UnknownScope(new SymInfo(_named_type_reference.names[_named_type_reference.names.Count - 1].name, SymbolKind.Type, _named_type_reference.names[_named_type_reference.names.Count - 1].name));
        }

        public override void visit(variable_definitions _variable_definitions)
        {
            bool is_event = false;
            foreach (var_def_statement vds in _variable_definitions.var_definitions)
            {
                if (vds.is_event && !is_event)
                    is_event = true;
                else if (is_event)
                    vds.is_event = true;
                vds.visit(this);
            }
        }

        public override void visit(ident_list _ident_list)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public System.Collections.Hashtable vars = new System.Collections.Hashtable();
        private TypeScope awaitedProcType;

        public override void visit(var_def_statement _var_def_statement)
        {
            try
            {
                returned_scope = null;
                if (_var_def_statement.vars_type != null)
                    _var_def_statement.vars_type.visit(this);
                if (_var_def_statement.vars_type == null && _var_def_statement.inital_value != null || _var_def_statement.inital_value is function_lambda_definition)
                {
                    SymScope tmp_scope = returned_scope;
                    TypeScope tmp_awaitedProcType = awaitedProcType;
                    if (_var_def_statement.inital_value is function_lambda_definition)
                    {
                        if (tmp_scope is ProcScope)
                            tmp_scope = new ProcType(tmp_scope as ProcScope);
                        awaitedProcType = tmp_scope as TypeScope;
                    }

                    _var_def_statement.inital_value.visit(this);
                    awaitedProcType = tmp_awaitedProcType;
                    if (tmp_scope != null && _var_def_statement.inital_value is function_lambda_definition)
                        returned_scope = tmp_scope;
                }
                // if (si == null) dn = compiled_type_node.get_type_node(PascalABCCompiler.NetHelper.NetHelper.FindType((_var_def_statement.vars_type as named_type_reference).names[0].name,unl));

                if (returned_scope == null) return;
                if (returned_scope is ProcScope)
                {
                    returned_scope = new ProcType(returned_scope as ProcScope);
                }

                if (_var_def_statement.vars != null)
                    foreach (ident s in _var_def_statement.vars.idents)
                    {
                        SymInfo si = new SymInfo(s.name, SymbolKind.Variable, s.name);
                        if (cur_scope is TypeScope) si.kind = SymbolKind.Field;
                        if (_var_def_statement.is_event) si.kind = SymbolKind.Event;
                        ElementScope es = new ElementScope(si, returned_scope, cur_scope);
                        if (add_doc_from_text && this.converter.controller.docs != null && this.converter.controller.docs.ContainsKey(_var_def_statement))
                            es.AddDocumentation(this.converter.controller.docs[_var_def_statement]);
                        es.acc_mod = cur_access_mod;
                        es.is_static = _var_def_statement.var_attr == definition_attribute.Static;
                        es.si.acc_mod = cur_access_mod;
                        es.loc = get_location(s);
                        cur_scope.AddName(s.name, es);
                        es.declaringUnit = cur_scope;
                    }
            }
            catch (Exception e)
            {
#if DEBUG
                File.AppendAllText("log.txt", e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
#endif
            }
        }
		
        public document doc;
        
        public location get_location(syntax_tree_node tn)
		{
			if (tn.source_context==null)
			{
				return null;
			}
			return new location(tn.source_context.begin_position.line_num,tn.source_context.begin_position.column_num,
				tn.source_context.end_position.line_num,tn.source_context.end_position.column_num,doc);
		}
        
        public override void visit(declaration _declaration)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(declarations _declarations)
        {
            //throw new Exception("The method or operation is not implemented.");
            foreach (declaration decl in _declarations.defs)
            	decl.visit(this);
        }

        public override void visit(program_tree _program_tree)
        {
            
        }

        public override void visit(program_name _program_name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(string_const _string_const)
        {
            returned_scope = TypeTable.string_type;//entry_scope.FindName(PascalABCCompiler.TreeConverter.compiler_string_consts.string_type_name);
            //cnst_val.prim_val = "'"+_string_const.Value+"'";
        	cnst_val.prim_val = this.converter.controller.Parser.LanguageInformation.GetStringForString(_string_const.Value);
        }

        public override void visit(expression_list _expression_list)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(dereference _dereference)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(roof_dereference _roof_dereference)
        {
            //throw new Exception("The method or operation is not implemented.");
            _roof_dereference.dereferencing_value.visit(this);
            if (returned_scope != null && returned_scope is PointerScope)
            	returned_scope = (returned_scope as PointerScope).ref_type;
            else returned_scope = null;
        }

        public override void visit(indexer _indexer)
        {
            //throw new Exception("The method or operation is not implemented.");
            _indexer.dereferencing_value.visit(this);
            if (returned_scope != null && returned_scope is TypeScope)
            {
                if ((returned_scope as TypeScope).GetFullName() != null && (returned_scope as TypeScope).GetFullName().IndexOf("System.Tuple") == 0)
                {
                    if (_indexer.indexes.expressions[0] is int32_const)
                    {
                        if ((_indexer.indexes.expressions[0] as int32_const).val >= 0)
                        {
                            dot_node dn = new dot_node(_indexer.dereferencing_value, new ident("Item" + ((_indexer.indexes.expressions[0] as int32_const).val + 1)));
                            dn.visit(this);
                        }
                        else
                            returned_scope = null;
                    }
                    else
                        returned_scope = null;
                }
                else
            	    returned_scope = returned_scope.GetElementType();
            }
        }

        public override void visit(PascalABCCompiler.SyntaxTree.for_node _for_node)
        {
            //throw new Exception("The method or operation is not implemented.");
            SymScope tmp = cur_scope;
            //if (_for_node.type_name != null)
            {
            	SymScope stmt_scope = new BlockScope(cur_scope);
        		cur_scope.AddName("$block_scope",stmt_scope);
        		stmt_scope.loc = get_location(_for_node);
        		returned_scope = null;
        		if (_for_node.type_name != null)
        		_for_node.type_name.visit(this);
        		if (returned_scope != null)
        		{
        			cur_scope = stmt_scope;
        			ElementScope es = new ElementScope(new SymInfo(_for_node.loop_variable.name, SymbolKind.Variable,_for_node.loop_variable.name),returned_scope,cur_scope);
        			stmt_scope.AddName(_for_node.loop_variable.name,es);
        			es.loc = get_location(_for_node.loop_variable);
        			returned_scope = null;
        		}
        		else
        		{
        			_for_node.initial_value.visit(this);
        			if (returned_scope != null)
        			{
        				cur_scope = stmt_scope;
        				if (_for_node.create_loop_variable)
        				{
        					ElementScope es = new ElementScope(new SymInfo(_for_node.loop_variable.name, SymbolKind.Variable,_for_node.loop_variable.name),returned_scope,cur_scope);
        					stmt_scope.AddName(_for_node.loop_variable.name,es);
        					es.loc = get_location(_for_node.loop_variable);
        				}
        				returned_scope = null;
        			}
        		}
            }
            if (_for_node.statements != null)
            _for_node.statements.visit(this);
            cur_scope = tmp;
        }

        public override void visit(PascalABCCompiler.SyntaxTree.repeat_node _repeat_node)
        {
        	if (_repeat_node.statements != null)
        	_repeat_node.statements.visit(this);
        }

        public override void visit(PascalABCCompiler.SyntaxTree.while_node _while_node)
        {
            if (_while_node.statements != null)
        	_while_node.statements.visit(this);
        }

        public override void visit(PascalABCCompiler.SyntaxTree.if_node _if_node)
        {
            if (_if_node.then_body != null)
        	_if_node.then_body.visit(this);
            if (_if_node.else_body != null)
        	_if_node.else_body.visit(this);
        }

        public override void visit(ref_type _ref_type)
        {
            //throw new Exception("The method or operation is not implemented.");
            _ref_type.pointed_to.visit(this);
            if (returned_scope != null && returned_scope is TypeScope)
            {
            	returned_scope = new PointerScope(returned_scope as TypeScope);
            	returned_scope.loc = get_location(_ref_type);
            }
            else 
            {
            	returned_scope = new PointerScope();
            	returned_scope.loc = get_location(_ref_type);
            }
            returned_scope.topScope = cur_scope;
        }

        public override void visit(diapason _diapason)
        {
            object left = null, right = null;
            
            try
            {
            	_diapason.left.visit(this);
            	left = cnst_val.prim_val;
            	cnst_val.prim_val = null;
            	_diapason.right.visit(this);
            	right = cnst_val.prim_val;
            }
            catch (Exception e)
            {
            	
            }
            if (left != null && right != null)
            {
            	returned_scope = new DiapasonScope(left, right);
            	returned_scope.loc = get_location(_diapason);
            	returned_scope.topScope = cur_scope;
            }
            else returned_scope = null;
        }

        public override void visit(indexers_types _indexers_types)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(array_type _array_type)
        {
            //throw new Exception("The method or operation is not implemented.");
            _array_type.elements_type.visit(this);
            if (returned_scope == null) return;
           	SymScope of_type = returned_scope;
            List<TypeScope> indexes = new List<TypeScope>();
            if (_array_type.indexers != null)
            {
            	foreach (type_definition td in _array_type.indexers.indexers)
            	{
            		if (td != null)
            		{
            			td.visit(this);
            			if (returned_scope != null) indexes.Add((TypeScope)returned_scope);
            			else
            			{
            				returned_scope = null;
            				return;
            			}
            		}
            		else
            			indexes.Add(null);
            	}
            }
            else indexes.Add((TypeScope)entry_scope.FindName(PascalABCCompiler.TreeConverter.compiler_string_consts.integer_type_name));
       
            returned_scope = new ArrayScope(of_type as TypeScope,indexes.ToArray());
            returned_scope.topScope = cur_scope;
            if (_array_type.indexers == null) (returned_scope as ArrayScope).is_dynamic_arr = true;
            returned_scope.loc = get_location(_array_type);
        }

        public override void visit(label_definitions _label_definitions)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(procedure_attribute _procedure_attribute)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(typed_parameters _typed_parametres)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(formal_parameters _formal_parametres)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(procedure_attributes_list _procedure_attributes_list)
        {
            throw new Exception("The method or operation is not implemented.");
        }
		
        private bool IsForward(procedure_header hdr)
        {
        	if (hdr.proc_attributes == null) return false;
        	for (int i=0; i<hdr.proc_attributes.proc_attributes.Count; i++)
        		if (hdr.proc_attributes.proc_attributes[i].attribute_type == proc_attribute.attr_forward)
        		return true;
        	return false;
        }
        
        private bool IsForward(function_header hdr)
        {
        	if (hdr.proc_attributes == null) return false;
        	for (int i=0; i<hdr.proc_attributes.proc_attributes.Count; i++)
        		if (hdr.proc_attributes.proc_attributes[i].attribute_type == proc_attribute.attr_forward)
        		return true;
        	return false;
        }
        
        private bool IsStatic(procedure_attributes_list attrs)
        {
        	if (attrs == null) return false;
        	for (int i=0; i<attrs.proc_attributes.Count; i++)
        		if (attrs.proc_attributes[i].attribute_type == proc_attribute.attr_static)
        		return true;
        	return false;
        }

        private void SetAttributes(ProcScope ps, procedure_attributes_list attrs)
        {
            if (attrs == null) return;
            for (int i = 0; i < attrs.proc_attributes.Count; i++)
                if (attrs.proc_attributes[i].attribute_type == proc_attribute.attr_static)
                    ps.is_static = true;
                else if (attrs.proc_attributes[i].attribute_type == proc_attribute.attr_virtual)
                    ps.is_virtual = true;
                else if (attrs.proc_attributes[i].attribute_type == proc_attribute.attr_abstract)
                {
                    ps.is_abstract = true;
                    if (ps.topScope is TypeScope)
                        (ps.topScope as TypeScope).is_abstract = true;
                }  
                else if (attrs.proc_attributes[i].attribute_type == proc_attribute.attr_override)
                    ps.is_override = true;
                else if (attrs.proc_attributes[i].attribute_type == proc_attribute.attr_reintroduce)
                    ps.is_reintroduce = true;
        }

        private ProcScope select_function_definition(ProcScope ps, formal_parameters prms, TypeScope return_type, TypeScope declType, bool function=false)
        {
            SymScope tmp = returned_scope;
            List<ElementScope> lst = new List<ElementScope>();
            if (prms != null)
            {
                foreach (typed_parameters pars in prms.params_list)
                {
                    pars.vars_type.visit(this);

                    if (returned_scope != null)
                    {
                        foreach (ident id in pars.idents.idents)
                        {
                            if (returned_scope is ProcScope)
                                returned_scope = new ProcType(returned_scope as ProcScope);
                            ElementScope si = new ElementScope(new SymInfo(id.name, SymbolKind.Parameter, id.name), returned_scope, ps);
                            si.loc = get_location(id);
                            si.param_kind = pars.param_kind;
                            if (pars.inital_value != null)
                            {
                                cnst_val.prim_val = null;
                                if (pars.inital_value is nil_const)
                                    si.cnst_val = "nil";
                                else
                                {
                                    pars.inital_value.visit(this);
                                    if (cnst_val.prim_val != null)
                                        si.cnst_val = cnst_val.prim_val;
                                    else
                                        si.cnst_val = pars.inital_value.ToString();
                                }
                                    
                            }
                            lst.Add(si);
                        }
                    }
                }
                while (ps != null)
                {
                    if (ps.parameters != null)
                    {
                        if (ps.parameters.Count == lst.Count)
                        {
                            bool eq = true;
                            for (int i = 0; i < lst.Count; i++)
                            {
                                TypeScope left = ps.parameters[i].sc as TypeScope;
                                TypeScope right = lst[i].sc as TypeScope;
                                if (!left.IsEqual(right) || ps.parameters[i].param_kind != lst[i].param_kind)
                                {
                                    eq = false;
                                    break;
                                }
                            }
                            if (eq)
                            {
                                if (return_type == null) return ps;
                                if (ps.return_type != null && return_type != null)
                                {
                                    if ((ps.return_type as TypeScope).IsEqual(return_type))
                                        return ps;
                                }
                            }
                        }
                    }
                    ps = ps.nextProc;
                }
            }
            else
            {
                while (ps != null)
                {
                    if (ps.parameters == null || ps.parameters.Count == 0)
                    {
                        if (function && ps.return_type != null && return_type == null)
                            return ps;
                        if (ps.return_type == null && return_type == null)
                            return ps;
                        if (ps.return_type != null && return_type != null)
                        {
                            if ((ps.return_type as TypeScope).IsEqual(return_type))
                                return ps;
                        }
                        else
                            return null;
                    }
                    ps = ps.nextProc;
                }
            }
            returned_scope = tmp;
            return ps;
        }

        public override void visit(procedure_header _procedure_header)
        {
            //throw new Exception("The method or operation is not implemented.");
            SymScope topScope;
            ProcScope ps = null;
            bool not_def = false;
            ProcRealization pr = null;
            bool is_realization = false;
            location loc = get_location(_procedure_header);
            if (_procedure_header.name != null)
            {
                _procedure_header.name.visit(this);

                if (_procedure_header.name.class_name != null)
                {
                    topScope = null;
                    if (_procedure_header.name.ln != null && _procedure_header.name.ln.Count > 0)
                    {
                        SymScope tmp_scope = cur_scope;
                        for (int i = 0; i < _procedure_header.name.ln.Count; i++)
                        {
                            tmp_scope = tmp_scope.FindName(_procedure_header.name.ln[i].name);
                            if (tmp_scope == null)
                                break;
                        }
                        topScope = tmp_scope;
                    }
                    else
                        topScope = cur_scope.FindName(_procedure_header.name.class_name.name);
                    if (topScope != null)
                    {
                        ps = topScope.FindNameOnlyInThisType(meth_name) as ProcScope;
                        if (ps != null && ps is CompiledMethodScope)
                            ps = null;
                        if (ps == null)
                        {
                            ps = new ProcScope(meth_name, topScope);
                            ps.head_loc = loc;
                            bool ext = false;
                            if (topScope is CompiledScope || topScope is ArrayScope || topScope is TypeSynonim /*&& ((topScope as TypeSynonim).actType is CompiledScope || (topScope as TypeSynonim).actType is ArrayScope || (topScope as TypeSynonim).actType is DiapasonScope)*/)
                                ext = true;
                            else if (!(topScope is TypeSynonim) && !(topScope is PointerScope) && !(topScope is SetScope) && !(topScope is FileScope))
                                ext = true;
                            if (ext)
                            {
                                not_def = true;
                                ps.is_extension = true;
                                ps.AddName("self", new ElementScope(new SymInfo("self", SymbolKind.Parameter, "self"), topScope, cur_scope));
                                ps.declaringType = topScope as TypeScope;
                                TypeScope ts = topScope as TypeScope;
                                if (topScope is TypeSynonim)
                                    ts = (ts as TypeSynonim).actType;
                                this.entry_scope.AddExtensionMethod(meth_name, ps, ts);
                                ts.AddExtensionMethod(meth_name, ps, ts);
                            }
                        }
                        //while (ps != null && ps.already_defined) ps = ps.nextProc;
                        else
                            ps = select_function_definition(ps, _procedure_header.parameters, null, topScope as TypeScope);
                        if (ps == null)
                        {
                            ps = new ProcScope(meth_name, cur_scope);
                            ps.head_loc = loc;
                        }
                        if (ps.parameters.Count != 0 && _procedure_header.parameters != null && is_proc_realization)
                        {
                            ps.parameters.Clear();
                            ps.already_defined = true;
                        }
                        if (impl_scope == null)
                        {
                            pr = new ProcRealization(ps, cur_scope);
                            pr.already_defined = true;
                            pr.loc = cur_loc;
                            pr.head_loc = loc;
                            is_realization = true;
                            entry_scope.AddName("$method", pr);
                        }
                        else
                        {
                            pr = new ProcRealization(ps, impl_scope);
                            pr.already_defined = true;
                            pr.loc = cur_loc;
                            is_realization = true;
                            pr.head_loc = loc;
                            impl_scope.AddName("$method", pr);
                        }
                    }
                    else
                    {
                        ps = new ProcScope(meth_name, cur_scope);
                        ps.head_loc = loc;
                    }
                }
                else
                {
                    ps = new ProcScope(meth_name, cur_scope);
                    if (has_extensionmethod_attr(_procedure_header.proc_attributes.proc_attributes) && _procedure_header.parameters.params_list.Count > 0)
                    {
                        ps.is_extension = true;
                        _procedure_header.parameters.params_list[0].vars_type.visit(this);
                        topScope = returned_scope;
                        ps.declaringType = topScope as TypeScope;
                        TypeScope ts = topScope as TypeScope;
                        if (topScope is TypeSynonim)
                            ts = (ts as TypeSynonim).actType;
                        if (ts.original_type != null && ts.instances != null)
                        {
                            bool pure_instance = true;
                            foreach (TypeScope gen_arg in ts.instances)
                            {
                                if (!(gen_arg is TemplateParameterScope))
                                    pure_instance = false;
                            }
                            if (pure_instance)
                                ts = ts.original_type;
                        }
                        this.entry_scope.AddExtensionMethod(meth_name, ps, ts);
                        topScope.AddExtensionMethod(meth_name, ps, ts);
                        pr = new ProcRealization(ps, cur_scope);
                        pr.already_defined = true;
                        pr.loc = cur_loc;
                        pr.head_loc = loc;
                        if (impl_scope != null)
                            impl_scope.AddName("$method", pr);
                        else
                            this.entry_scope.AddName("$method", pr);
                    }
                    ps.head_loc = loc;
                    if (!ps.is_extension)
                    {
                        if (IsForward(_procedure_header))
                        {
                            cur_scope.AddName(meth_name, ps);
                            ps.is_forward = true;
                        }
                        else
                        {
                            bool found_in_top = false;
                            SymScope ss = null;
                            if (cur_scope is ImplementationUnitScope)
                            {
                                ss = (cur_scope as ImplementationUnitScope).topScope.FindNameOnlyInThisType(meth_name);
                                if (ss != null && ss is ProcScope)
                                {
                                    //ps = ss as ProcScope;
                                    //while ((ss as ProcScope).already_defined && (ss as ProcScope).nextProc != null)
                                    //	ss = (ss as ProcScope).nextProc;
                                    ps = select_function_definition(ss as ProcScope, _procedure_header.parameters, null, null);
                                    if (ps == null)
                                    {
                                        ps = new ProcScope(meth_name, cur_scope);
                                        ps.head_loc = loc;
                                    }
                                    //ps = ss as ProcScope;
                                    if (ps.parameters.Count != 0 && _procedure_header.parameters != null)
                                    {
                                        ps.parameters.Clear();
                                        ps.already_defined = true;
                                    }
                                    pr = new ProcRealization(ps, cur_scope);
                                    pr.already_defined = true;
                                    pr.loc = cur_loc;
                                    pr.head_loc = loc;
                                    is_realization = true;
                                    cur_scope.AddName("$method", pr);
                                    found_in_top = true;
                                }
                            }
                            if (!found_in_top) //ne nashli opisanie v interface chasti modilja
                            {
                                //ss = cur_scope.FindNameOnlyInType(meth_name);
                                ss = cur_scope.FindName(meth_name);
                                if (ss != null && ss is ProcScope)
                                {
                                    if ((ss as ProcScope).is_forward && ss == select_function_definition(ss as ProcScope, _procedure_header.parameters, null, null))
                                    {
                                        //if ((ss as ProcScope).parameters.Count != 0 && _procedure_header.parameters != null) (ss as ProcScope).parameters.Clear();
                                        pr = new ProcRealization(ss as ProcScope, cur_scope);
                                        pr.already_defined = true;
                                        pr.loc = cur_loc;
                                        cur_scope.AddName("$method", pr);
                                        returned_scope = pr;
                                        pr.head_loc = loc;
                                        return;
                                    }
                                    else
                                    {
                                        ps = new ProcScope(meth_name, cur_scope);
                                        ps.head_loc = loc;
                                        if (ps.topScope == ss.topScope)
                                        {
                                            while ((ss as ProcScope).nextProc != null && (ss as ProcScope).nextProc.topScope == ps.topScope) ss = (ss as ProcScope).nextProc;
                                            ProcScope tmp_ps = (ss as ProcScope).nextProc;
                                            (ss as ProcScope).nextProc = ps;
                                            ps.nextProc = tmp_ps;
                                            cur_scope.AddName(meth_name, ps);
                                            ps.si.name = meth_name;
                                        }
                                        else
                                        {
                                            ps.nextProc = ss as ProcScope;
                                            cur_scope.AddName(meth_name, ps);
                                        }
                                        //ps = select_function_definition(ss as ProcScope,_procedure_header.parameters);
                                    }
                                }
                                else
                                {
                                    cur_scope.AddName(meth_name, ps);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                ps = new ProcScope("", cur_scope);
                ps.head_loc = loc;
            }
            if ((!is_realization || not_def) && ps.loc == null)
                ps.loc = cur_loc;
            //ps.head_loc = loc;
            ps.declaringUnit = entry_scope;
            if (_procedure_header.template_args != null && !ps.IsGeneric())
            {
                Dictionary<string, TypeScope> where_types = new Dictionary<string, TypeScope>();
                if (_procedure_header.where_defs != null)
                {
                    foreach (where_definition wd in _procedure_header.where_defs.defs)
                    {
                        for (int i = 0; i < wd.names.idents.Count; i++)
                        {
                            string name = wd.names.idents[i].name;
                            wd.types.defs[i].visit(this);
                            where_types[name] = returned_scope as TypeScope;
                        }
                    }
                }
                foreach (ident s in _procedure_header.template_args.idents)
                {
                    ps.AddTemplateParameter(s.name);
                    TypeScope where_ts = null;
                    where_types.TryGetValue(s.name, out where_ts);
                    if (where_ts == null)
                        where_ts = TypeTable.obj_type;
                    TemplateParameterScope tps = new TemplateParameterScope(s.name, where_ts, ps);
                    tps.loc = get_location(s);
                    ps.AddName(s.name, tps);
                }
            }
            SetAttributes(ps, _procedure_header.proc_attributes);
            ps.is_static = _procedure_header.class_keyword;
            if (add_doc_from_text && this.converter.controller.docs != null && this.converter.controller.docs.ContainsKey(_procedure_header))
                ps.AddDocumentation(this.converter.controller.docs[_procedure_header]);

            if (is_proc_realization) ps.already_defined = true;
            else
            {
                ps.loc = loc;
            }
            if (_procedure_header.name == null || _procedure_header.name.class_name == null)
            {
                ps.acc_mod = cur_access_mod;
                ps.si.acc_mod = cur_access_mod;
            }
            SymScope tmp = cur_scope;
            cur_scope = ps;
            if (_procedure_header.parameters != null)
                add_parameters(ps, _procedure_header.parameters);
            cur_scope = tmp;
            if (cur_scope is TypeScope && !ps.is_static)
                ps.AddName("self", new ElementScope(new SymInfo("self", SymbolKind.Parameter, "self"), cur_scope, ps));
            //cur_scope = ps;
            returned_scope = ps;
            ps.Complete();
            if (pr != null && not_def)
                pr.Complete();
        }

        bool has_extensionmethod_attr(List<procedure_attribute> attrs)
        {
            foreach (procedure_attribute attr in attrs)
            {
                if (attr.attribute_type == PascalABCCompiler.SyntaxTree.proc_attribute.attr_extension)
                {
                    return true;
                }
            }
            return false;
        }

        public override void visit(function_header _function_header)
        {
            SymScope topScope;
            ProcScope ps = null;
            bool is_realization = false;
            TypeScope return_type = null;
            bool not_def = false;
            ProcRealization pr = null;
            location loc = get_location(_function_header);
            returned_scope = null;
            if (_function_header.return_type != null && !(_function_header.return_type is enum_type_definition && _function_header.template_args != null && _function_header.template_args.Count > 0))
                _function_header.return_type.visit(this);
            if (returned_scope != null)
            {
                if (returned_scope is ProcScope)
                    returned_scope = new ProcType(returned_scope as ProcScope);
                return_type = returned_scope as TypeScope;
            }
            if (_function_header.name != null)
            {
                _function_header.name.visit(this);
                if (_function_header.name.class_name != null)
                {
                    topScope = null;
                    if (_function_header.name.ln != null && _function_header.name.ln.Count > 0)
                    {
                        SymScope tmp_scope = cur_scope;
                        for (int i = 0; i < _function_header.name.ln.Count; i++)
                        {
                            tmp_scope = tmp_scope.FindName(_function_header.name.ln[i].name);
                            if (tmp_scope == null)
                                break;
                        }
                        topScope = tmp_scope;
                    }
                    else
                        topScope = cur_scope.FindName(_function_header.name.class_name.name);
                    if (topScope != null)
                    {
                        ps = topScope.FindNameOnlyInThisType(meth_name) as ProcScope;
                        if (ps != null && ps is CompiledMethodScope)
                            ps = null;
                        if (ps == null)
                        {
                            ps = new ProcScope(meth_name, topScope);

                            ps.head_loc = loc;
                            bool ext = false;
                            if (topScope is CompiledScope || topScope is ArrayScope || topScope is TypeSynonim /*&& ((topScope as TypeSynonim).actType is CompiledScope || (topScope as TypeSynonim).actType is ArrayScope || (topScope as TypeSynonim).actType is DiapasonScope)*/)
                                ext = true;
                            else if (!(topScope is TypeSynonim) && !(topScope is PointerScope) && !(topScope is SetScope) && !(topScope is FileScope))
                                ext = true;
                            if (ext)
                            {
                                not_def = true;
                                ps.AddName("self", new ElementScope(new SymInfo("self", SymbolKind.Parameter, "self"), topScope, cur_scope));
                                ps.declaringType = topScope as TypeScope;
                                ps.is_extension = true;
                                TypeScope ts = topScope as TypeScope;
                                if (topScope is TypeSynonim)
                                    ts = (ts as TypeSynonim).actType;
                                this.entry_scope.AddExtensionMethod(meth_name, ps, ts);
                                ts.AddExtensionMethod(meth_name, ps, ts);
                            }
                        }
                        else
                            ps = select_function_definition(ps, _function_header.parameters, return_type, topScope as TypeScope, true);
                        //while (ps != null && ps.already_defined) ps = ps.nextProc;
                        if (ps == null)
                        {
                            ps = new ProcScope(meth_name, cur_scope);
                            ps.head_loc = loc;

                        }
                        if (ps.parameters.Count != 0 && _function_header.parameters != null && is_proc_realization)
                        {
                            ps.parameters.Clear();
                            ps.already_defined = true;
                        }
                        if (impl_scope == null)
                        {
                            pr = new ProcRealization(ps, cur_scope);
                            pr.already_defined = true;
                            pr.loc = cur_loc;
                            is_realization = true;
                            pr.head_loc = loc;
                            entry_scope.AddName("$method", pr);
                        }
                        else
                        {
                            pr = new ProcRealization(ps, impl_scope);
                            pr.already_defined = true;
                            pr.loc = cur_loc;
                            pr.head_loc = loc;
                            is_realization = true;
                            impl_scope.AddName("$method", pr);
                        }
                    }
                    else
                    {
                        ps = new ProcScope(meth_name, cur_scope);
                        ps.head_loc = loc;
                    }
                }
                else
                {
                    ps = new ProcScope(meth_name, cur_scope);
                    ps.head_loc = loc;
                    if (has_extensionmethod_attr(_function_header.proc_attributes.proc_attributes) && _function_header.parameters.params_list.Count > 0)
                    {
                        ps.is_extension = true;
                        _function_header.parameters.params_list[0].vars_type.visit(this);
                        topScope = returned_scope;
                        if (topScope is ProcScope)
                            topScope = new ProcType(topScope as ProcScope);
                        ps.declaringType = topScope as TypeScope;

                        TypeScope ts = topScope as TypeScope;
                        if (topScope is TypeSynonim)
                            ts = (ts as TypeSynonim).actType;
                        if (ts.original_type != null && ts.instances != null)
                        {
                            bool pure_instance = true;
                            foreach (TypeScope gen_arg in ts.instances)
                            {
                                if (!(gen_arg is TemplateParameterScope) && !(gen_arg is UnknownScope))
                                    pure_instance = false;
                            }
                            if (pure_instance)
                                ts = ts.original_type;
                        }
                        this.entry_scope.AddExtensionMethod(meth_name, ps, ts);
                        topScope.AddExtensionMethod(meth_name, ps, ts);
                        /*if (topScope is TemplateParameterScope || topScope is UnknownScope)
                            TypeTable.obj_type.AddExtensionMethod(meth_name, ps, ts);*/
                        pr = new ProcRealization(ps, cur_scope);
                        pr.already_defined = true;
                        pr.loc = cur_loc;
                        pr.head_loc = loc;
                        if (impl_scope != null)
                            impl_scope.AddName("$method", pr);
                        else
                            this.entry_scope.AddName("$method", pr);
                    }
                    if (!ps.is_extension)
                    {
                        if (IsForward(_function_header))
                        {
                            cur_scope.AddName(meth_name, ps);
                            ps.is_forward = true;
                        }
                        else
                        {
                            bool found_in_top = false;
                            SymScope ss = null;
                            if (cur_scope is ImplementationUnitScope)
                            {
                                ss = (cur_scope as ImplementationUnitScope).topScope.FindNameOnlyInThisType(meth_name);
                                if (ss != null && ss is ProcScope)
                                {
                                    //while ((ss as ProcScope).already_defined && (ss as ProcScope).nextProc != null) ss = (ss as ProcScope).nextProc;
                                    //ps = ss as ProcScope;
                                    ps = select_function_definition(ss as ProcScope, _function_header.parameters, return_type, null, true);
                                    if (ps == null)
                                    {
                                        ps = new ProcScope(meth_name, cur_scope);
                                        ps.head_loc = loc;
                                    }
                                    if (ps.parameters.Count != 0 && _function_header.parameters != null)
                                    {
                                        ps.parameters.Clear();
                                        ps.already_defined = true;
                                    }
                                    pr = new ProcRealization(ps, cur_scope);
                                    pr.already_defined = true;
                                    pr.loc = cur_loc;
                                    pr.head_loc = loc;
                                    is_realization = true;
                                    cur_scope.AddName("$method", pr);
                                    found_in_top = true;
                                }
                            }
                            if (!found_in_top)
                            {
                                //ss = cur_scope.FindNameOnlyInType(meth_name);
                                ss = cur_scope.FindName(meth_name);
                                if (ss != null && ss is ProcScope)
                                {
                                    if ((ss as ProcScope).is_forward && ss == select_function_definition(ss as ProcScope, _function_header.parameters, (ss as ProcScope).return_type, null, true))
                                    {
                                        //if ((ss as ProcScope).parameters.Count != 0 && _function_header.parameters != null) (ss as ProcScope).parameters.Clear();
                                        pr = new ProcRealization(ss as ProcScope, cur_scope);
                                        pr.already_defined = true;
                                        pr.loc = cur_loc;
                                        cur_scope.AddName("$method", pr);
                                        pr.head_loc = loc;
                                        returned_scope = pr;
                                        return;
                                    }
                                    else
                                    {
                                        ps = new ProcScope(meth_name, cur_scope);
                                        ps.head_loc = loc;
                                        if (ps.topScope == ss.topScope)
                                        {
                                            while ((ss as ProcScope).nextProc != null && (ss as ProcScope).nextProc.topScope == ps.topScope)
                                                ss = (ss as ProcScope).nextProc;
                                            ProcScope tmp_ps = (ss as ProcScope).nextProc;
                                            (ss as ProcScope).nextProc = ps;
                                            ps.nextProc = tmp_ps;
                                            cur_scope.AddName(meth_name, ps);
                                            ps.si.name = meth_name;
                                        }
                                        else
                                        {
                                            ps.nextProc = ss as ProcScope;
                                            cur_scope.AddName(meth_name, ps);
                                        }
                                    }
                                }
                                else
                                {
                                    cur_scope.AddName(meth_name, ps);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                ps = new ProcScope("", cur_scope);
                ps.head_loc = loc;
            }
            if ((!is_realization || not_def) && ps.loc == null)
                ps.loc = cur_loc;
            //ps.head_loc = loc;
            ps.declaringUnit = entry_scope;
            SymScope tmp = cur_scope;
            if (_function_header.template_args != null && !ps.IsGeneric())
            {
                Dictionary<string, TypeScope> where_types = new Dictionary<string, TypeScope>();
                if (_function_header.where_defs != null)
                {
                    foreach (where_definition wd in _function_header.where_defs.defs)
                    {
                        for (int i = 0; i < wd.names.idents.Count; i++)
                        {
                            string name = wd.names.idents[i].name;
                            wd.types.defs[i].visit(this);
                            where_types[name] = returned_scope as TypeScope;
                        }
                    }
                }
                foreach (ident s in _function_header.template_args.idents)
                {
                    ps.AddTemplateParameter(s.name);
                    TypeScope where_ts = null;
                    where_types.TryGetValue(s.name, out where_ts);
                    if (where_ts == null)
                        where_ts = TypeTable.obj_type;
                    TemplateParameterScope tps = new TemplateParameterScope(s.name, where_ts, ps);

                    tps.loc = get_location(s);
                    ps.AddName(s.name, tps);
                }
                if (ps.return_type == null)
                {
                    cur_scope = ps;
                    if (_function_header.return_type != null)
                        _function_header.return_type.visit(this);
                    return_type = returned_scope as TypeScope;

                    cur_scope = tmp;
                }
            }

            SetAttributes(ps, _function_header.proc_attributes);
            ps.is_static = _function_header.class_keyword;
            if (add_doc_from_text && this.converter.controller.docs != null && this.converter.controller.docs.ContainsKey(_function_header))
                ps.AddDocumentation(this.converter.controller.docs[_function_header]);
            if (is_proc_realization)
            {
                ps.already_defined = true;
                if (ps.loc == null)
                    ps.loc = loc;
            }
            else
            {
                ps.loc = loc;
            }
            if (_function_header.name == null || _function_header.name.class_name == null)
            {
                ps.acc_mod = cur_access_mod;
                ps.si.acc_mod = cur_access_mod;
            }

            cur_scope = ps;
            if (_function_header.parameters != null)
                add_parameters(ps, _function_header.parameters);
            cur_scope = tmp;
            if (ps.procRealization == null || ps.return_type == null)
                ps.return_type = return_type;
            //cur_scope = ps;
            if (cur_scope is TypeScope && !ps.is_static)
                ps.AddName("self", new ElementScope(new SymInfo("self", SymbolKind.Parameter, "self"), cur_scope, ps));

            returned_scope = ps;
            ps.AddName("Result", new ElementScope(new SymInfo("Result", SymbolKind.Variable, "Result"), ps.return_type, ps));
            ps.Complete();
            if (pr != null && not_def)
                pr.Complete();
        }
		
        private void add_parameters(ProcScope ps, formal_parameters parameters)
        {
            foreach (typed_parameters pars in parameters.params_list)
            {
                pars.vars_type.visit(this);
                if (returned_scope != null)
                {
                    if (returned_scope is ProcScope)
                        returned_scope = new ProcType(returned_scope as ProcScope);
                    foreach (ident id in pars.idents.idents)
                    {
                        ElementScope si = new ElementScope(new SymInfo(id.name, SymbolKind.Parameter, id.name), returned_scope, ps);
                        si.loc = get_location(id);
                        if (pars.inital_value != null)
                        {
                            cnst_val.prim_val = null;
                            if (!(pars.inital_value is nil_const))
                            {
                                pars.inital_value.visit(this);
                                if (cnst_val.prim_val != null)
                                    si.cnst_val = cnst_val.prim_val;
                                else
                                    si.cnst_val = pars.inital_value.ToString();
                            }
                            else
                                si.cnst_val = "nil";
                        }
                        ps.AddName(id.name, si);
                        si.param_kind = pars.param_kind;
                        si.MakeDescription();
                        ps.AddParameter(si);
                    }
                }
            }
        }

        
        public override void visit(procedure_definition _procedure_definition)
        {
            //throw new Exception("The method or operation is not implemented.");
            SymScope tmp = cur_scope;
            cur_loc = get_location(_procedure_definition);
            if (!parse_only_method_body)
            {
                is_proc_realization = true;
                _procedure_definition.proc_header.visit(this);
                is_proc_realization = false;
            }
            
            if (_procedure_definition.proc_body != null)
            {
                if (!parse_only_method_body && _procedure_definition.proc_header is function_header && (_procedure_definition.proc_header as function_header).return_type == null)
                {
                    var fh = (_procedure_definition.proc_header as function_header);
                    if (fh != null && fh.return_type == null && !(returned_scope is ProcScope && (returned_scope as ProcScope).procRealization != null && !(returned_scope as ProcScope).is_extension))
                    {
                        var bl = _procedure_definition.proc_body as block;
                        if (bl != null && bl.program_code != null)
                        {
                            if (bl.program_code.subnodes.Count == 1)
                            {
                                var ass = bl.program_code.subnodes[0] as assign;
                                if (ass != null && ass.to is ident && (ass.to as ident).name.ToLower() == "result")
                                {
                                    if (!(ass.from is nil_const))
                                    {
                                        ProcScope tmp_scope = returned_scope as ProcScope;
                                        cur_scope = returned_scope;
                                        ass.from.visit(this);
                                        if (returned_scope != null && returned_scope is TypeScope)
                                        {
                                            tmp_scope.return_type = returned_scope as TypeScope;
                                            tmp_scope.Complete();
                                            returned_scope = tmp_scope;
                                        }
                                    }
                                }
                            } 
                        }
                    }
                }
                if (!parse_only_method_body)
                    cur_scope = returned_scope;
            	/*if ((ret_tn as ProcScope).return_type != null)
            	{
            		ret_tn.AddName("Result",new ElementScope(new SymInfo("Result", SymbolKind.Variable,"Result"),(ret_tn as ProcScope).return_type,cur_scope));
            	}*/
                if (!parse_only_interface && !parse_only_method_header)
                {
                	try
                	{
                		_procedure_definition.proc_body.visit(this);
                	}
                	catch (Exception ex)
                	{
                		
                	}
                }
            	    
                if (cur_scope != null && cur_scope is ProcScope)
                {
                    ProcRealization pr = (cur_scope as ProcScope).procRealization;
                    if (pr == null)
                        cur_scope.body_loc = get_location(_procedure_definition.proc_body);
                    else
                        pr.body_loc = get_location(_procedure_definition.proc_body);
                }
            }
            cur_scope = tmp;
        }
		
        private List<PointerScope> ref_type_wait_list = new List<PointerScope>();
        
        private ident_list template_args = null;

        public override void visit(type_declaration _type_declaration)
        {
            //throw new Exception("The method or operation is not implemented.");
            cur_type_name = _type_declaration.type_name.name;
            List<string> generic_params = new List<string>();
            if (_type_declaration.type_name is template_type_name)
            {
                template_args = (_type_declaration.type_name as template_type_name).template_args;
                foreach (ident id in (_type_declaration.type_name as template_type_name).template_args.list)
                {
                    generic_params.Add(id.name);
                }
            }
            
            _type_declaration.type_def.visit(this);
            if (returned_scope != null && returned_scope is PointerScope && (returned_scope as PointerScope).ref_type is UnknownScope)
            {
                ref_type_wait_list.Add(returned_scope as PointerScope);
            }
            //else
            if (returned_scope != null && returned_scope is TypeScope)
            {
                if (!(_type_declaration.type_def is named_type_reference) && !(returned_scope is CompiledScope && _type_declaration.type_def is enum_type_definition))
                {
                    //(ret_tn as TypeScope).name = _type_declaration.type_name.name;
                    returned_scope.si.name = _type_declaration.type_name.name;
                    returned_scope.si.description = returned_scope.GetDescription();
                    if (!(_type_declaration.type_def is class_definition))
                        returned_scope.MakeSynonimDescription();
                    returned_scope.loc = get_location(_type_declaration);//new location(loc.begin_line_num,loc.begin_column_num,ret_tn.loc.end_line_num,ret_tn.loc.end_column_num,ret_tn.loc.doc);
                    if (_type_declaration.type_def is class_definition)
                    {
                        class_definition cl_def = _type_declaration.type_def as class_definition;
                        string key = this.converter.controller.Parser.LanguageInformation.GetClassKeyword(cl_def.keyword);
                        if (cl_def.attribute == class_attribute.Auto)
                            key = "auto " + key;
                        else if (cl_def.attribute == class_attribute.Abstract)
                            key = "abstract " + key;
                        else if (cl_def.attribute == class_attribute.Sealed)
                            key = "sealed " + key;
                        if (key != null && returned_scope.body_loc != null)
                        {
                            returned_scope.head_loc = new location(returned_scope.body_loc.begin_line_num, returned_scope.body_loc.begin_column_num, returned_scope.body_loc.begin_line_num, returned_scope.body_loc.begin_column_num + key.Length, doc);
                        }
                    }
                    if (add_doc_from_text && this.converter.controller.docs != null && this.converter.controller.docs.ContainsKey(_type_declaration))
                        returned_scope.AddDocumentation(this.converter.controller.docs[_type_declaration]);
                    if (!(_type_declaration.type_def is class_definition))
                        cur_scope.AddName(_type_declaration.type_name.name, returned_scope);
                }
                else
                {
                    TypeSynonim ts = new TypeSynonim(new SymInfo(_type_declaration.type_name.name, returned_scope.si.kind, _type_declaration.type_name.name), returned_scope, generic_params);
                    ts.loc = get_location(_type_declaration);
                    ts.topScope = cur_scope;
                    ts.declaringUnit = entry_scope;
                    //ts.si.describe = "type "+ret_tn.si.name+" = "+ret_tn.si.describe;
                    cur_scope.AddName(_type_declaration.type_name.name, ts);
                    if (add_doc_from_text && this.converter.controller.docs != null && this.converter.controller.docs.ContainsKey(_type_declaration))
                        ts.AddDocumentation(this.converter.controller.docs[_type_declaration]);
                }

            }
            else if (returned_scope != null)
            {
                if (returned_scope is ProcScope)
                {
                    returned_scope = new ProcType(returned_scope as ProcScope, generic_params);
                    returned_scope.topScope = cur_scope;
                }
                cur_scope.AddName(_type_declaration.type_name.name, returned_scope);
                if (returned_scope is ProcType)
                {
                    returned_scope.MakeSynonimDescription();
                }
                location loc = get_location(_type_declaration);
                if (returned_scope.loc == null)
                {
                    returned_scope.loc = loc;
                    //ret_tn.loc = new location(loc.begin_line_num,loc.begin_column_num,ret_tn.loc.end_line_num,ret_tn.loc.end_column_num,ret_tn.loc.doc);
                }
            }
            returned_scope.declaringUnit = entry_scope;
            if (ref_type_wait_list.Count == 0) returned_scope = null;
            cur_type_name = null;
            template_args = null;
        }

        public override void visit(type_declarations _type_declarations)
        {
            //throw new Exception("The method or operation is not implemented.");
            Hashtable ht = new Hashtable(StringComparer.OrdinalIgnoreCase);
            foreach (type_declaration td in _type_declarations.types_decl)
            {
            	
            	td.visit(this);
            	if (td.type_name != null)
            	ht[td.type_name.name] = returned_scope;
            }
            if (ref_type_wait_list.Count > 0)
            {
            	for (int i=0; i<ref_type_wait_list.Count; i++)
            	{
            		PointerScope ps = ref_type_wait_list[i];
            		TypeScope ts = ht[ps.ref_type.si.name] as TypeScope;
            		if (ts != null)
            		{
            			ps.ref_type = ts;
            		}
            	}
            }
            returned_scope = null;
            ref_type_wait_list.Clear();
        }

        public override void visit(simple_const_definition _simple_const_definition)
        {
            //throw new Exception("The method or operation is not implemented.");
            try
            {
                _simple_const_definition.const_value.visit(this);
            }
            catch (Exception e)
            {
#if DEBUG
                File.AppendAllText("log.txt", e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
#endif
            }
            if (returned_scope != null /*&& cnst_val.prim_val != null*/)
            {
                ElementScope es = new ElementScope(new SymInfo(_simple_const_definition.const_name.name, SymbolKind.Constant, _simple_const_definition.const_name.name), returned_scope, cnst_val.prim_val != null? cnst_val.prim_val:_simple_const_definition.const_value.ToString(), cur_scope);
                es.is_static = true;
                cur_scope.AddName(_simple_const_definition.const_name.name, es);
                es.loc = get_location(_simple_const_definition.const_name);
                es.declaringUnit = entry_scope;
                if (add_doc_from_text && this.converter.controller.docs != null && this.converter.controller.docs.ContainsKey(_simple_const_definition))
                    es.AddDocumentation(this.converter.controller.docs[_simple_const_definition]);
            }
            returned_scope = null;
            cnst_val.prim_val = null;
        }

        public override void visit(typed_const_definition _typed_const_definition)
        {
            //throw new Exception("The method or operation is not implemented.");
            _typed_const_definition.const_type.visit(this);
            if (returned_scope == null) return;
            cnst_val.prim_val = null;
            SymScope cnst_type = returned_scope;
            try
            {
                _typed_const_definition.const_value.visit(this);
            }
            catch (Exception e)
            {
#if DEBUG
                File.AppendAllText("log.txt", e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
#endif
            }
            ElementScope es = new ElementScope(new SymInfo(_typed_const_definition.const_name.name, SymbolKind.Constant, _typed_const_definition.const_name.name), cnst_type, cnst_val.prim_val != null?cnst_val.prim_val: _typed_const_definition.const_value.ToString(), cur_scope);
            cur_scope.AddName(_typed_const_definition.const_name.name, es);
            es.loc = get_location(_typed_const_definition);
            es.is_static = true;
            es.declaringUnit = entry_scope;
            if (add_doc_from_text && this.converter.controller.docs != null && this.converter.controller.docs.ContainsKey(_typed_const_definition))
                es.AddDocumentation(this.converter.controller.docs[_typed_const_definition]);
            cnst_val.prim_val = null;
        }

        public override void visit(const_definition _const_definition)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(consts_definitions_list _consts_definitions_list)
        {
            //throw new Exception("The method or operation is not implemented.");
            foreach (const_definition cnst in _consts_definitions_list.const_defs)
            	cnst.visit(this);
        }

        public override void visit(unit_name _unit_name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(unit_or_namespace _unit_or_namespace)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(uses_unit_in _uses_unit_in)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(uses_list _uses_list)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(program_body _program_body)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(compilation_unit _compilation_unit)
        {
            throw new Exception("The method or operation is not implemented.");
        }
		
        private Hashtable ns_cache;
        private bool is_system_unit=false;
        private bool is_extensions_unit = false;

        public override void visit(unit_module _unit_module)
        {
            string path = get_assembly_path("mscorlib.dll",_unit_module.file_name);
        	Assembly _as = PascalABCCompiler.NetHelper.NetHelper.LoadAssembly(path);
            AssemblyDocCache.Load(_as, path);
            PascalABCCompiler.NetHelper.NetHelper.init_namespaces(_as);
            List<string> namespaces = new List<string>();
            namespaces.AddRange(PascalABCCompiler.NetHelper.NetHelper.GetNamespaces(_as));
            InterfaceUnitScope unit_scope = null;
            cur_scope = unit_scope= new InterfaceUnitScope(new SymInfo(_unit_module.unit_name.idunit_name.name, SymbolKind.Namespace,_unit_module.unit_name.idunit_name.name),null);
            this.cur_unit_file_name = _unit_module.file_name;
            //if (XmlDoc.LookupLocalizedXmlDocForUnitWithSources(_unit_module.file_name) != null)
            if (!add_doc_from_text)
            {
            	UnitDocCache.LoadWithSources(cur_scope,_unit_module.file_name);
            	//this.add_doc_from_text = false;
            }
            //add_standart_types_simple();
            Stack<Position> regions_stack = new Stack<Position>();
            if (CodeCompletionController.comp.CompilerOptions.CurrentProject != null && CodeCompletionController.comp.CompilerOptions.CurrentProject.ContainsSourceFile(_unit_module.file_name))
            {
            	IReferenceInfo[] refs = CodeCompletionController.comp.CompilerOptions.CurrentProject.References;
            	if (_unit_module.compiler_directives == null)
            		_unit_module.compiler_directives = new List<PascalABCCompiler.SyntaxTree.compiler_directive>();
            	foreach (IReferenceInfo ri in refs)
            	{
            		_unit_module.compiler_directives.Add
            			(new PascalABCCompiler.SyntaxTree.compiler_directive(new token_info("reference"),new token_info(ri.FullAssemblyName)));
            	}
            }
            if (_unit_module.compiler_directives != null)
            foreach (PascalABCCompiler.SyntaxTree.compiler_directive dir in _unit_module.compiler_directives)
            {
            	if (dir.Name.text.ToLower() == "reference")
                {
                    try
                    {
                		//System.Reflection.Assembly assm = System.Reflection.Assembly.LoadFrom(get_assembly_path(dir.Directive.text,_unit_module.file_name));
                		path = get_assembly_path(dir.Directive.text,_unit_module.file_name);
                    	System.Reflection.Assembly assm = PascalABCCompiler.NetHelper.NetHelper.LoadAssembly(path);
                		PascalABCCompiler.NetHelper.NetHelper.init_namespaces(assm);
                    	AssemblyDocCache.Load(assm, path);
                    	namespaces.AddRange(PascalABCCompiler.NetHelper.NetHelper.GetNamespaces(assm));
                    	unit_scope.AddReferencedAssembly(assm);
                    }
                    catch (Exception e)
                    {
                    	
                    }
                    //ns=new PascalABCCompiler.NetHelper.NetScope(unl,assm,tcst);
                }
                else
                if (dir.Name.text.ToLower() == "region")
                {
                	if (cur_scope.regions == null)
                		cur_scope.regions = new List<Position>();
                    regions_stack.Push(new Position(dir.source_context.begin_position.line_num, dir.source_context.begin_position.column_num, dir.source_context.end_position.line_num, dir.source_context.end_position.column_num, dir.source_context.FileName, dir.Directive.text));	                   
                }
                else if (dir.Name.text.ToLower() == "endregion")
                {
                	if (regions_stack.Count > 0)
                	{
                		Position pos = regions_stack.Pop();
                		if (cur_scope.regions != null)
                		{
                            cur_scope.regions.Add(new Position(pos.line, pos.column-1, dir.source_context.end_position.line_num, dir.source_context.end_position.column_num, pos.file_name, pos.fold_text));
                		}
                	}
                }
                
            }
            ns_cache = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
            
            doc = new document(_unit_module.file_name);
            cur_scope.head_loc = get_location(_unit_module.unit_name); 
            cur_scope.file_name = _unit_module.file_name;
            cur_scope.loc = get_location(_unit_module);
            cur_scope.AddName(_unit_module.unit_name.idunit_name.name,cur_scope);
            if (add_doc_from_text && this.converter.controller.docs != null && this.converter.controller.docs.ContainsKey(_unit_module.unit_name))
        		cur_scope.AddDocumentation(this.converter.controller.docs[_unit_module.unit_name]);
            entry_scope = cur_scope;
            if (_unit_module.unit_name.idunit_name.name == this.converter.controller.Parser.LanguageInformation.SystemUnitName)
            {
            	is_system_unit = true;
            	add_standart_types(entry_scope);
            }
            if (_unit_module.unit_name.idunit_name.name == PascalABCCompiler.TreeConverter.compiler_string_consts.extensions_unit_file_name)
            {
                is_extensions_unit = true;
            }
            CodeCompletionController.comp_modules[_unit_module.file_name] = this.converter;
            DateTime start_time = DateTime.Now;
            System.Diagnostics.Debug.WriteLine("intellisense parsing interface started " + System.Convert.ToInt32((DateTime.Now - start_time).TotalMilliseconds));
            _unit_module.interface_part.visit(this);
            System.Diagnostics.Debug.WriteLine("intellisense parsing interface ended " + System.Convert.ToInt32((DateTime.Now - start_time).TotalMilliseconds));
            foreach (string s in namespaces)
            {
            	if (!ns_cache.ContainsKey(s))
            	{
                  NamespaceScope ns_scope = new NamespaceScope(s);
                  entry_scope.AddName(s,ns_scope);
                  ns_cache[s] = s;
            	}
            }
            start_time = DateTime.Now;
            System.Diagnostics.Debug.WriteLine("intellisense parsing implementation started "+ System.Convert.ToInt32((DateTime.Now - start_time).TotalMilliseconds));
            if (_unit_module.implementation_part != null)
                _unit_module.implementation_part.visit(this);
            System.Diagnostics.Debug.WriteLine("intellisense parsing implementation ended " + System.Convert.ToInt32((DateTime.Now - start_time).TotalMilliseconds));
            if (_unit_module.initialization_part != null)
            {
            	SymScope tmp = cur_scope;
            	SymScope stmt_scope = new BlockScope(cur_scope);
        		cur_scope.AddName("$block_scope",stmt_scope);
        		stmt_scope.loc = get_location(_unit_module.initialization_part);
        		cur_scope = stmt_scope;
            	_unit_module.initialization_part.visit(this);
            	cur_scope = tmp;
            }
            if (_unit_module.finalization_part != null)
            {
            	SymScope tmp = cur_scope;
            	SymScope stmt_scope = new BlockScope(cur_scope);
        		cur_scope.AddName("$block_scope",stmt_scope);
        		stmt_scope.loc = get_location(_unit_module.finalization_part);
        		cur_scope = stmt_scope;
            	_unit_module.finalization_part.visit(this);
            	cur_scope = tmp;
            }
            
        }

        public string get_assembly_path(string name, string CompFile)
        {
        	string fname = (string.Format("{0}\\{1}",System.IO.Path.GetDirectoryName(CompFile), name)).ToLower();
        	if (System.IO.File.Exists(fname))
            	return fname;
        	return Compiler.get_assembly_path(name,true);
        }

        PascalABCCompiler.TreeRealization.using_namespace_list unl = new PascalABCCompiler.TreeRealization.using_namespace_list();
		public SymScope cur_scope;

        public string FindPCUFileName(string UnitName)
        {
        	return CodeCompletionController.comp.FindPCUFileNameWithoutSources(UnitName,System.IO.Path.GetDirectoryName(doc.file_name));
        }
        
		private void add_system_unit()
		{
			string unit_file_name = this.converter.controller.Parser.LanguageInformation.SystemUnitName;
			if (unit_file_name == null) return;
			string unit_name = CodeCompletionNameHelper.FindSourceFileName(unit_file_name);
                    	 
            if (unit_name != null)
            {
                 DomConverter dc = CodeCompletionController.comp_modules[unit_name] as DomConverter;
                 if (dc == null)
                 {
                 	CodeCompletionController ccc = new CodeCompletionController();
                 	dc = ccc.CompileAllIfNeed(unit_name, true);
                 	//dc.CompileAllIfNeed(unit_name);
                 	if (dc.visitor != null && dc.visitor.entry_scope != null)
                 	{
                 		dc.visitor.entry_scope.InitAssemblies();
                 		entry_scope.AddUsedUnit(dc.visitor.entry_scope);
                 		add_standart_types(dc.visitor.entry_scope);
                 		//get_standart_types(dc.stv);
                 		entry_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.system_unit_file_name,dc.visitor.entry_scope);
                 	}
                 	CodeCompletionController.comp_modules[unit_name] = dc;
                 	
                 }
                 else if (dc.visitor != null && dc.visitor.entry_scope != null)
                 {
                 	dc.visitor.entry_scope.InitAssemblies();
                 	entry_scope.AddUsedUnit(dc.visitor.entry_scope);
                 	//get_standart_types(dc.stv);
                 	entry_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.system_unit_file_name,dc.visitor.entry_scope);
                 }
            }
		}

        private void add_extensions_unit()
        {
            string unit_name = CodeCompletionNameHelper.FindSourceFileName(PascalABCCompiler.TreeConverter.compiler_string_consts.extensions_unit_file_name);

            if (unit_name != null)
            {
                DomConverter dc = CodeCompletionController.comp_modules[unit_name] as DomConverter;
                if (dc == null)
                {
                    CodeCompletionController ccc = new CodeCompletionController();
                    dc = ccc.CompileAllIfNeed(unit_name, true);
                    if (dc.visitor != null && dc.visitor.entry_scope != null)
                    {
                        dc.visitor.entry_scope.InitAssemblies();
                        entry_scope.AddUsedUnit(dc.visitor.entry_scope);
                        entry_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.extensions_unit_file_name, dc.visitor.entry_scope);
                    }
                    CodeCompletionController.comp_modules[unit_name] = dc;
                }
                else if (dc.visitor != null && dc.visitor.entry_scope != null)
                {
                    dc.visitor.entry_scope.InitAssemblies();
                    entry_scope.AddUsedUnit(dc.visitor.entry_scope);
                    entry_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.extensions_unit_file_name, dc.visitor.entry_scope);
                }
            }
        }

        public override void visit(program_module _program_module)
        {
            //Assembly _as = System.Reflection.Assembly.LoadFrom(get_assembly_path("mscorlib.dll",_program_module.file_name));
            string path = get_assembly_path("mscorlib.dll", _program_module.file_name);
            System.Reflection.Assembly _as = PascalABCCompiler.NetHelper.NetHelper.LoadAssembly(path);
            List<string> namespaces = new List<string>();
            PascalABCCompiler.NetHelper.NetHelper.init_namespaces(_as);
            AssemblyDocCache.Load(_as, path);

            namespaces.AddRange(PascalABCCompiler.NetHelper.NetHelper.GetNamespaces(_as));
            //List<Scope> netScopes = new List<Scope>();
            //PascalABCCompiler.NetHelper.NetScope ns=new PascalABCCompiler.NetHelper.NetScope(unl,_as,tcst);
            InterfaceUnitScope unit_scope = null;
            cur_scope = unit_scope = new InterfaceUnitScope(new SymInfo("", SymbolKind.Namespace, "program"), null);
            CodeCompletionController.comp_modules[_program_module.file_name] = this.converter;
            Stack<Position> regions_stack = new Stack<Position>();
            if (CodeCompletionController.comp.CompilerOptions.CurrentProject != null && CodeCompletionController.comp.CompilerOptions.CurrentProject.ContainsSourceFile(_program_module.file_name))
            {
                IReferenceInfo[] refs = CodeCompletionController.comp.CompilerOptions.CurrentProject.References;
                if (_program_module.compiler_directives == null)
                    _program_module.compiler_directives = new List<PascalABCCompiler.SyntaxTree.compiler_directive>();
                foreach (IReferenceInfo ri in refs)
                {
                    _program_module.compiler_directives.Add
                        (new PascalABCCompiler.SyntaxTree.compiler_directive(new token_info("reference"), new token_info(ri.FullAssemblyName)));
                }
            }
            List<string> included_files = new List<string>();
            if (_program_module.compiler_directives != null)
                foreach (PascalABCCompiler.SyntaxTree.compiler_directive dir in _program_module.compiler_directives)
                {
#if DEBUG
                    // SSM test 05.08.17
                    if (dir == null)
                        File.AppendAllText("log.txt", "dir == null" + Environment.NewLine + _program_module.compiler_directives.Count + Environment.NewLine);
#endif
                    if (dir.Name.text.ToLower() == "reference")
                    {
                        try
                        {
                            //System.Reflection.Assembly assm = System.Reflection.Assembly.LoadFrom(get_assembly_path(dir.Directive.text,_program_module.file_name));
                            path = get_assembly_path(dir.Directive.text, _program_module.file_name);
                            System.Reflection.Assembly assm = PascalABCCompiler.NetHelper.NetHelper.LoadAssembly(path);
                            if (assm != null)
                            {
                                PascalABCCompiler.NetHelper.NetHelper.init_namespaces(assm);
                                AssemblyDocCache.Load(assm, path);
                                namespaces.AddRange(PascalABCCompiler.NetHelper.NetHelper.GetNamespaces(assm));
                                unit_scope.AddReferencedAssembly(assm);
                            }
                        }
                        catch (Exception e)
                        {

                        }
                    }
                    else
                    if (dir.Name.text.ToLower() == "region")
                    {
                        if (cur_scope.regions == null)
                            cur_scope.regions = new List<Position>();
                        regions_stack.Push(new Position(dir.source_context.begin_position.line_num, dir.source_context.begin_position.column_num, dir.source_context.end_position.line_num, dir.source_context.end_position.column_num, dir.source_context.FileName));
                    }
                    else if (dir.Name.text.ToLower() == "endregion")
                    {
                        if (regions_stack.Count > 0)
                        {
                            Position pos = regions_stack.Pop();
                            if (cur_scope.regions != null)
                            {
                                cur_scope.regions.Add(new Position(pos.end_line, pos.end_column, dir.source_context.end_position.line_num, dir.source_context.end_position.column_num, pos.file_name));
                            }
                        }
                    }
                    else if (dir.Name.text.ToLower() == "includenamespace")
                    {
                        string directive = dir.Directive.text.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);

                        if (directive == "*.pas" || directive.EndsWith(Path.DirectorySeparatorChar + "*.pas"))
                        {
                            string udir = Path.Combine(Path.GetDirectoryName(_program_module.file_name), directive.Replace(Path.DirectorySeparatorChar + "*.pas", ""));
                            foreach (string file in Directory.EnumerateFiles(udir, "*.pas"))
                                included_files.Add(file);
                        }
                        else
                            included_files.Add(Path.Combine(Path.GetDirectoryName(_program_module.file_name), directive));
                    }
                }

            doc = new document(_program_module.file_name);
            cur_scope.loc = get_location(_program_module);
            entry_scope = cur_scope;
            if (_program_module.program_name != null)
                cur_scope.head_loc = get_location(_program_module.program_name);

            Hashtable ns_cache = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
            bool has_system_unit = false;
            bool has_extensions_unit = false;
            foreach (string file in included_files)
            {
                DomConverter dc = CodeCompletionController.comp_modules[file] as DomConverter;
                if (dc == null)
                {
                    dc = new CodeCompletionController().CompileAllIfNeed(file, true);
                }
                if (dc.visitor != null)
                {
                    
                }
            }
            if (_program_module.used_units != null)
            {
                unit_scope.uses_source_range = get_location(_program_module.used_units);

                //foreach (unit_or_namespace s in _program_module.used_units.units)
                for (int j = _program_module.used_units.units.Count - 1; j >= 0; j--)
                {
                    unit_or_namespace s = _program_module.used_units.units[j];

                    try
                    {
                        string str = "";
                        for (int i = 0; i < s.name.idents.Count; i++)
                        {
                            str += s.name.idents[i].name;
                            NamespaceScope ns_scope = null;
                            if (i == 0)
                            {
                                string unit_name = null;
                                string pcu_unit_name = FindPCUFileName(str);
                                if (s is uses_unit_in)
                                    unit_name = (s as uses_unit_in).in_file.Value;
                                else
                                    unit_name = CodeCompletionNameHelper.FindSourceFileName(str, System.IO.Path.GetDirectoryName(_program_module.file_name));
                                if (pcu_unit_name != null && unit_name != null && string.Compare(System.IO.Path.GetDirectoryName(_program_module.file_name), System.IO.Path.GetDirectoryName(pcu_unit_name), true) == 0
                                    && string.Compare(System.IO.Path.GetDirectoryName(_program_module.file_name), System.IO.Path.GetDirectoryName(unit_name), true) != 0)
                                    unit_name = null;
                                if (unit_name != null)
                                {
                                    DomConverter dc = CodeCompletionController.comp_modules[unit_name] as DomConverter;
                                    if (dc == null /*|| CodeCompletionController.recomp_files[unit_name] != null*/)
                                    {
                                        dc = new CodeCompletionController().CompileAllIfNeed(unit_name, true);
                                    }
                                    if (dc.visitor != null)
                                    {
                                        dc.visitor.entry_scope.InitAssemblies();
                                        cur_scope.AddUsedUnit(dc.visitor.entry_scope);
                                        cur_scope.AddName(str, dc.visitor.entry_scope);
                                    }
                                }
                                else
                                {
                                    //unit_name = FindPCUFileName(str);
                                    unit_name = pcu_unit_name;
                                    if (unit_name != null)
                                    {
                                        IntellisensePCUReader pcu_rdr = new IntellisensePCUReader();
                                        SymScope ss = pcu_rdr.GetUnit(unit_name);
                                        UnitDocCache.Load(ss, unit_name);
                                        cur_scope.AddUsedUnit(ss);
                                        cur_scope.AddName(str, ss);
                                    }
                                    else
                                    {
                                        if (PascalABCCompiler.NetHelper.NetHelper.IsNetNamespace(str))
                                        {
                                            ns_scope = new NamespaceScope(str);
                                            ns_cache[str] = str;
                                            cur_scope.AddName(str, ns_scope);
                                            if (s.name.idents.Count == 1)
                                                cur_scope.AddUsedUnit(ns_scope);
                                        }
                                        else if (PascalABCCompiler.NetHelper.NetHelper.IsType(str) && semantic_options.allow_import_types)
                                        {
                                            Type t = PascalABCCompiler.NetHelper.NetHelper.FindType(str);
                                            cur_scope.AddUsedUnit(new NamespaceTypeScope(TypeTable.get_compiled_type(new SymInfo(t.Name, SymbolKind.Class, t.FullName), t)));
                                        }
                                    }
                                    //unit_name = System.IO.Path.GetDirectoryName(_program_module.file_name)+"\\"+str+System.IO.Path.GetExtension(_program_module.file_name);
                                }
                            }
                            if (i == s.name.idents.Count - 1 && i > 0 /*&& PascalABCCompiler.NetHelper.NetHelper.IsNetNamespace(str)*/)
                                //possible_namespaces.Add(str);
                                cur_scope.AddUsedUnit(new NamespaceScope(str));
                            if (i < s.name.idents.Count - 1)
                                str += ".";

                        }
                        if (string.Compare(str, PascalABCCompiler.TreeConverter.compiler_string_consts.system_unit_name, true) == 0)
                            has_system_unit = true;
                        if (string.Compare(str, PascalABCCompiler.TreeConverter.compiler_string_consts.extensions_unit_file_name, true) == 0)
                            has_extensions_unit = true;
                        unl.AddElement(new PascalABCCompiler.TreeRealization.using_namespace(str));
                    }
                    catch (Exception e)
                    {
#if DEBUG
                        File.AppendAllText("log.txt", e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
#endif
                    }
                }

            }
            if (!has_system_unit)
                add_system_unit();

            if (!has_extensions_unit)
                add_extensions_unit();

            foreach (string s in namespaces)
            {
                if (!ns_cache.ContainsKey(s))
                {
                    NamespaceScope ns_scope = new NamespaceScope(s);
                    cur_scope.AddName(s, ns_scope);
                    ns_cache[s] = s;
                }
            }

            //PascalABCCompiler.TreeRealization.common_type_node ctn = new ;
            if (_program_module.program_block.defs != null)
                foreach (declaration decl in _program_module.program_block.defs.defs)
                {
                    try
                    {
                        decl.visit(this);
                    }
                    catch (Exception e)
                    {
#if DEBUG
                        File.AppendAllText("log.txt", e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
#endif
                    }
                }
            if (cur_scope != null && _program_module.program_block.program_code != null)
            {
                cur_scope.body_loc = new location(_program_module.program_block.program_code.left_logical_bracket.source_context.end_position.line_num,
                                                  _program_module.program_block.program_code.left_logical_bracket.source_context.end_position.column_num,
                                                  _program_module.program_block.program_code.source_context.end_position.line_num, _program_module.program_block.program_code.source_context.end_position.column_num,
                                                 doc);
                _program_module.program_block.program_code.visit(this);
            }
        }
        
        private void add_standart_types(SymScope cur_scope)
        {
        	string type_name = null;
        	//obj_type = new CompiledScope(new SymInfo(PascalABCCompiler.TreeConverter.compiler_string_consts.object_type_name, SymbolKind.Type,PascalABCCompiler.TreeConverter.compiler_string_consts.object_type_name),typeof(object));
        	cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.object_type_name,TypeTable.obj_type);
        	//int_type = new CompiledScope(new SymInfo(PascalABCCompiler.TreeConverter.compiler_string_consts.integer_type_name, SymbolKind.Type,PascalABCCompiler.TreeConverter.compiler_string_consts.integer_type_name),typeof(int));
        	//cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.integer_type_name, int_type);
        	type_name = this.converter.controller.Parser.LanguageInformation.GetStandardTypeByKeyword(PascalABCCompiler.Parsers.KeywordKind.IntType);
        	if (type_name != null) cur_scope.AddName(type_name, TypeTable.int_type);
        	//real_type = new CompiledScope(new SymInfo(PascalABCCompiler.TreeConverter.compiler_string_consts.real_type_name, SymbolKind.Type,PascalABCCompiler.TreeConverter.compiler_string_consts.real_type_name),typeof(double));
        	//cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.real_type_name,real_type);
            type_name = this.converter.controller.Parser.LanguageInformation.GetStandardTypeByKeyword(PascalABCCompiler.Parsers.KeywordKind.DoubleType);
            if (type_name != null) cur_scope.AddName(type_name, TypeTable.real_type);
        	//string_type = new CompiledScope(new SymInfo(PascalABCCompiler.TreeConverter.compiler_string_consts.string_type_name, SymbolKind.Class,PascalABCCompiler.TreeConverter.compiler_string_consts.string_type_name),typeof(string));
        	cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.string_type_name,TypeTable.string_type);
        	//cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.string_type_name,
        	//new CompiledScope(new SymInfo(PascalABCCompiler.TreeConverter.compiler_string_consts.string_type_name, SymbolKind.Type,PascalABCCompiler.TreeConverter.compiler_string_consts.ShortStringTypeName),typeof(string)));
        	//char_type = new CompiledScope(new SymInfo(PascalABCCompiler.TreeConverter.compiler_string_consts.char_type_name, SymbolKind.Type,PascalABCCompiler.TreeConverter.compiler_string_consts.char_type_name),typeof(char));
        	type_name = this.converter.controller.Parser.LanguageInformation.GetStandardTypeByKeyword(PascalABCCompiler.Parsers.KeywordKind.CharType);
            if (type_name != null) cur_scope.AddName(type_name, TypeTable.char_type);
        	//cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.char_type_name,char_type);
        	//bool_type = new CompiledScope(new SymInfo(PascalABCCompiler.TreeConverter.compiler_string_consts.bool_type_name, SymbolKind.Type,PascalABCCompiler.TreeConverter.compiler_string_consts.bool_type_name),typeof(bool));
        	type_name = this.converter.controller.Parser.LanguageInformation.GetStandardTypeByKeyword(PascalABCCompiler.Parsers.KeywordKind.BoolType);
            if (type_name != null) cur_scope.AddName(type_name, TypeTable.bool_type);
        	//cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.bool_type_name,bool_type);
        	//byte_type = new CompiledScope(new SymInfo(PascalABCCompiler.TreeConverter.compiler_string_consts.byte_type_name, SymbolKind.Type,PascalABCCompiler.TreeConverter.compiler_string_consts.byte_type_name),typeof(byte));
        	type_name = this.converter.controller.Parser.LanguageInformation.GetStandardTypeByKeyword(PascalABCCompiler.Parsers.KeywordKind.ByteType);
            if (type_name != null) cur_scope.AddName(type_name, TypeTable.byte_type);
        	//cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.byte_type_name,byte_type);
        	//int16_type = new CompiledScope(new SymInfo(PascalABCCompiler.TreeConverter.compiler_string_consts.short_type_name, SymbolKind.Type,PascalABCCompiler.TreeConverter.compiler_string_consts.short_type_name),typeof(short));
        	type_name = this.converter.controller.Parser.LanguageInformation.GetStandardTypeByKeyword(PascalABCCompiler.Parsers.KeywordKind.ShortType);
            if (type_name != null) cur_scope.AddName(type_name, TypeTable.int16_type);
        	//cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.short_type_name,int16_type);
        	//sbyte_type = new CompiledScope(new SymInfo(PascalABCCompiler.TreeConverter.compiler_string_consts.sbyte_type_name, SymbolKind.Type,PascalABCCompiler.TreeConverter.compiler_string_consts.sbyte_type_name),typeof(sbyte));
        	type_name = this.converter.controller.Parser.LanguageInformation.GetStandardTypeByKeyword(PascalABCCompiler.Parsers.KeywordKind.SByteType);
            if (type_name != null) cur_scope.AddName(type_name, TypeTable.sbyte_type);
        	//cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.sbyte_type_name,sbyte_type);
        	//uint16_type = new CompiledScope(new SymInfo(PascalABCCompiler.TreeConverter.compiler_string_consts.ushort_type_name, SymbolKind.Type,PascalABCCompiler.TreeConverter.compiler_string_consts.ushort_type_name),typeof(ushort));
        	type_name = this.converter.controller.Parser.LanguageInformation.GetStandardTypeByKeyword(PascalABCCompiler.Parsers.KeywordKind.UShortType);
            if (type_name != null) cur_scope.AddName(type_name, TypeTable.uint16_type);
        	//cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.ushort_type_name,uint16_type);
        	//uint32_type = new CompiledScope(new SymInfo(PascalABCCompiler.TreeConverter.compiler_string_consts.uint_type_name, SymbolKind.Type,PascalABCCompiler.TreeConverter.compiler_string_consts.uint_type_name),typeof(uint));
        	type_name = this.converter.controller.Parser.LanguageInformation.GetStandardTypeByKeyword(PascalABCCompiler.Parsers.KeywordKind.UIntType);
            if (type_name != null) cur_scope.AddName(type_name, TypeTable.uint32_type);
        	//cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.uint_type_name,uint32_type);
        	//int64_type = new CompiledScope(new SymInfo(PascalABCCompiler.TreeConverter.compiler_string_consts.long_type_name, SymbolKind.Type,PascalABCCompiler.TreeConverter.compiler_string_consts.long_type_name),typeof(long));
        	type_name = this.converter.controller.Parser.LanguageInformation.GetStandardTypeByKeyword(PascalABCCompiler.Parsers.KeywordKind.Int64Type);
            if (type_name != null) cur_scope.AddName(type_name, TypeTable.int64_type);
        	//cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.long_type_name,int64_type);
        	//uint64_type = new CompiledScope(new SymInfo(PascalABCCompiler.TreeConverter.compiler_string_consts.ulong_type_name, SymbolKind.Type,PascalABCCompiler.TreeConverter.compiler_string_consts.ulong_type_name),typeof(ulong));
        	type_name = this.converter.controller.Parser.LanguageInformation.GetStandardTypeByKeyword(PascalABCCompiler.Parsers.KeywordKind.UInt64Type);
            if (type_name != null) cur_scope.AddName(type_name, TypeTable.uint64_type);
        	//cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.ulong_type_name,uint64_type);
        	//float_type = new CompiledScope(new SymInfo(PascalABCCompiler.TreeConverter.compiler_string_consts.float_type_name, SymbolKind.Type,PascalABCCompiler.TreeConverter.compiler_string_consts.float_type_name),typeof(float));
        	type_name = this.converter.controller.Parser.LanguageInformation.GetStandardTypeByKeyword(PascalABCCompiler.Parsers.KeywordKind.FloatType);
            if (type_name != null) cur_scope.AddName(type_name, TypeTable.float_type);
        	//cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.float_type_name,float_type);
        	//ptr_type = new CompiledScope(new SymInfo(PascalABCCompiler.TreeConverter.compiler_string_consts.pointer_type_name, SymbolKind.Type,PascalABCCompiler.TreeConverter.compiler_string_consts.pointer_type_name),Type.GetType("System.Void*"));
        	type_name = this.converter.controller.Parser.LanguageInformation.GetStandardTypeByKeyword(PascalABCCompiler.Parsers.KeywordKind.PointerType);
            if (type_name != null) cur_scope.AddName(type_name, TypeTable.ptr_type);
        	//cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.pointer_type_name,ptr_type);
        	ProcScope ps = new ProcScope(PascalABCCompiler.TreeConverter.compiler_string_consts.set_length_procedure_name,null);
        	ps.AddParameter(new ElementScope(new SymInfo("arr", SymbolKind.Parameter,"arr"),new ArrayScope(),null,ps));
        	ps.parameters[0].param_kind = parametr_kind.var_parametr;
        	ps.AddParameter(new ElementScope(new SymInfo("length", SymbolKind.Parameter,"length"),TypeTable.int_type,null,ps));
        	ps.Complete();
        	cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.set_length_procedure_name,ps);
        	cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.true_const_name,new ElementScope(new SymInfo(PascalABCCompiler.TreeConverter.compiler_string_consts.true_const_name, SymbolKind.Constant,PascalABCCompiler.TreeConverter.compiler_string_consts.true_const_name),TypeTable.bool_type,true,null));
        	cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.false_const_name,new ElementScope(new SymInfo(PascalABCCompiler.TreeConverter.compiler_string_consts.false_const_name, SymbolKind.Constant,PascalABCCompiler.TreeConverter.compiler_string_consts.false_const_name),TypeTable.bool_type,false,null));
       		ps = new ProcScope(PascalABCCompiler.TreeConverter.compiler_string_consts.new_procedure_name,null);
       		ElementScope prm = new ElementScope(new SymInfo("p", SymbolKind.Parameter,"p"),TypeTable.ptr_type,null,ps);
       		prm.param_kind = parametr_kind.var_parametr;
       		ps.AddParameter(prm);
       		ps.Complete();
       		cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.new_procedure_name,ps);
       		ps = new ProcScope(PascalABCCompiler.TreeConverter.compiler_string_consts.dispose_procedure_name,null);
       		prm = new ElementScope(new SymInfo("p", SymbolKind.Parameter,"p"),TypeTable.ptr_type,null,ps);
       		prm.param_kind = parametr_kind.var_parametr;
       		ps.AddParameter(prm);
       		ps.Complete();
       		cur_scope.AddName(PascalABCCompiler.TreeConverter.compiler_string_consts.dispose_procedure_name,ps);
       }
        
        public override void visit(hex_constant _hex_constant)
        {
            //throw new Exception("The method or operation is not implemented.");
            returned_scope = TypeTable.uint64_type;//entry_scope.FindName(PascalABCCompiler.TreeConverter.compiler_string_consts.ulong_type_name);
            cnst_val.prim_val = _hex_constant.val;
        }

        public override void visit(get_address _get_address)
        {
            //throw new Exception("The method or operation is not implemented.");
            _get_address.address_of.visit(this);
            if (returned_scope != null && returned_scope is TypeScope)
            {
            	returned_scope = new PointerScope(returned_scope as TypeScope);
            }
            else returned_scope = null;
        }

        public override void visit(case_variant _case_variant)
        {
            //throw new Exception("The method or operation is not implemented.");
        }
		
        public override void visit(compiler_directive_if _compiler_directive_if)
        {
        	
        }
        
        public override void visit(compiler_directive_list _compiler_directive_list)
        {
        	
        }

        public override void visit(case_node _case_node)
        {
            if (_case_node.conditions != null)
                for (int i = 0; i < _case_node.conditions.variants.Count; i++)
                    _case_node.conditions.variants[i].exec_if_true.visit(this);
            if (_case_node.else_statement != null)
                _case_node.else_statement.visit(this);
        }

        public override void visit(method_name _method_name)
        {
        	if (_method_name.meth_name is operator_name_ident)
        		_method_name.meth_name.visit(this);
        	else
        		meth_name = _method_name.meth_name.name;
        }

        public override void visit(dot_node _dot_node)
        {
            bool tmp = search_all;
        	search_all = false;
        	_dot_node.left.visit(this);
            search_all = tmp;
        	if (returned_scope != null)
			{
				if (_dot_node.right is ident)
				{
					if (!search_all)
					{
                        if (returned_scope is NamespaceScope)
                        {
                            returned_scope = returned_scope.FindNameOnlyInType((_dot_node.right as ident).name);
                            return;
                        }
                        TypeScope ts = returned_scope as TypeScope;
                        if (returned_scope is ProcScope)
                            ts = (returned_scope as ProcScope).return_type;
                        if (ts != null)  
						    returned_scope = ts.FindNameOnlyInType((_dot_node.right as ident).name);
                        if (returned_scope == null)
                        {
                            List<ProcScope> meths = entry_scope.GetExtensionMethods((_dot_node.right as ident).name, ts);
                            if (meths.Count > 0)
                            {
                                method_call mc = new method_call(_dot_node, new expression_list());
                                search_all = true;
                                mc.visit(this);
                                return;
                            }
                                
                        }
                        if (returned_scope != null && returned_scope is ProcScope && (returned_scope as ProcScope).return_type != null)
						{
                            ProcScope ps = returned_scope as ProcScope;
                            if (ps.parameters.Count == 0 || ps.is_extension || ps.parameters[0].param_kind == parametr_kind.params_parametr || ps.parameters[0].cnst_val != null)
                                returned_scope = ps.return_type;
							return;
						}
						else if (returned_scope is ElementScope)
						{
							this.cnst_val.prim_val = (returned_scope as ElementScope).cnst_val;
							returned_scope = (returned_scope as ElementScope).sc;
							return;
						}
						else if (returned_scope is TypeScope)
							is_type = true;
					}
					else
					{
                        TypeScope ts = returned_scope as TypeScope;
                        if (returned_scope is ProcScope)
                            ts = (returned_scope as ProcScope).return_type;
                        returned_scopes = returned_scope.FindOverloadNamesOnlyInType((_dot_node.right as ident).name);
                        if (ts != null)
                        {
                            List<ProcScope> meths = entry_scope.GetExtensionMethods((_dot_node.right as ident).name, ts);
                            if (meths.Count > 0)
                            {
                                if (returned_scopes == null)
                                    returned_scopes = new List<SymScope>();
                                foreach (ProcScope meth in meths)
                                    returned_scopes.Add(meth);
                            }
                        }
                        search_all = false;
					}
				}
			}
        }

        public override void visit(PascalABCCompiler.SyntaxTree.empty_statement _empty_statement)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(PascalABCCompiler.SyntaxTree.goto_statement _goto_statement)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(PascalABCCompiler.SyntaxTree.labeled_statement _labeled_statement)
        {
            //throw new Exception("The method or operation is not implemented.");
            if (_labeled_statement.to_statement != null)
            	_labeled_statement.to_statement.visit(this);
        }
		
        private bool is_type = false;
        
        public override void visit(with_statement _with_statement)
        {
            //throw new Exception("The method or operation is not implemented.");
            SymScope tmp = cur_scope;
            WithScope ws = new WithScope(cur_scope);
            for (int i=_with_statement.do_with.expressions.Count-1; i>=0; i--)
            {
            	is_type = false;
            	_with_statement.do_with.expressions[i].visit(this);
            	if (returned_scope != null)
            	{
            		ws.AddWithScope(returned_scope,is_type);
            	}
            	is_type = false;
            }
            cur_scope.AddName("$with_block",ws);
            ws.loc = get_location(_with_statement);
            cur_scope = ws;
            _with_statement.what_do.visit(this);
            cur_scope = tmp;
        }

        internal static bool is_good_overload(ProcScope ps, List<SymScope> args)
        {
            if (ps.parameters == null || ps.parameters.Count == 0)
                if (args.Count == 0)
                    return true;
                else
                    return false;
            if (args.Count == 0)
                if (ps.parameters.Count == 1 && ps.parameters[0].param_kind == parametr_kind.params_parametr)
                    return true;
                else if (ps.IsExtension && ps.parameters.Count == 1)
                    return true;
                else
                    return false;
            if (args.Count == ps.parameters.Count || ps.IsExtension && args.Count == ps.parameters.Count - 1)
            {
                int off = 0;
                if (ps.IsExtension && args.Count == ps.parameters.Count - 1)
                    off = 1;
                for (int i = 0; i < args.Count; i++)
                {
                    if (args[i] is UnknownScope)
                        continue;
                    if (!(args[i] is TypeScope))
                        return false;
                    TypeScope ts = args[i] as TypeScope;
                    ElementScope parameter = ps.parameters[i + off];
                    if (!ts.IsConvertable(parameter.sc as TypeScope))
                    {
                        if (parameter.param_kind == parametr_kind.params_parametr)
                        {
                            if (!(parameter.sc is TypeScope && (parameter.sc as TypeScope).IsArray && ts.IsConvertable((parameter.sc as TypeScope).elementType)))
                                return false;
                        }
                        else
                            return false;
                    }
                }
                return true;
            }
            else
            {
                if (args.Count < ps.parameters.Count)
                    return false;
                int min_arg_cnt = Math.Min(args.Count, ps.parameters.Count);

                for (int i = 0; i < min_arg_cnt; i++)
                {
                    if (!(args[i] is TypeScope))
                        return false;
                    TypeScope ts = args[i] as TypeScope;
                    if (!ts.IsConvertable(ps.parameters[i].sc as TypeScope))
                    {
                        if (ps.parameters[i].param_kind == parametr_kind.params_parametr)
                        {
                            if (!(ps.parameters[i].sc is ArrayScope && ts.IsConvertable((ps.parameters[i].sc as ArrayScope).elementType)))
                                return false;
                        }
                        else
                            return false;
                    }
                    else if (i == ps.parameters.Count - 1 && ps.parameters[i].param_kind != parametr_kind.params_parametr)
                        return false;

                }
                return true;
            }
        }

        internal static bool is_good_exact_overload(ProcScope ps, List<SymScope> args)
        {
            if (ps.parameters == null || ps.parameters.Count == 0)
                if (args.Count == 0)
                    return true;
                else
                    return false;
            if (args.Count == 0)
                if (ps.parameters.Count == 1 && ps.parameters[0].param_kind == parametr_kind.params_parametr)
                    return true;
                else
                    return false;
            if (args.Count == ps.parameters.Count || ps.IsExtension && args.Count == ps.parameters.Count - 1)
            {
                int off = 0;
                if (ps.IsExtension && args.Count == ps.parameters.Count - 1)
                    off = 1;
                for (int i = 0; i < args.Count; i++)
                {
                    if (!(args[i] is TypeScope))
                        return false;
                    TypeScope ts = args[i] as TypeScope;
                    ElementScope parameter = ps.parameters[i + off];
                    if (ps.IsExtension && i == 0 && ts.IsEqual((parameter.sc as TypeScope).GetElementType()))
                        continue;
                    if (!ts.IsEqual(parameter.sc as TypeScope))
                    {
                        if (parameter.param_kind == parametr_kind.params_parametr)
                        {
                            if (!(parameter.sc is ArrayScope && ts.IsEqual((parameter.sc as ArrayScope).elementType)))
                                return false;
                        }
                        else
                            return false;
                    }
                }
                return true;
            }
            else
            {
                if (args.Count < ps.parameters.Count)
                    return false;
                int min_arg_cnt = Math.Min(args.Count, ps.parameters.Count);

                for (int i = 0; i < min_arg_cnt; i++)
                {
                    if (!(args[i] is TypeScope))
                        return false;
                    TypeScope ts = args[i] as TypeScope;
                    if (!ts.IsEqual(ps.parameters[i].sc as TypeScope))
                    {
                        if (ps.parameters[i].param_kind == parametr_kind.params_parametr)
                        {
                            if (!(ps.parameters[i].sc is ArrayScope && ts.IsEqual((ps.parameters[i].sc as ArrayScope).elementType)))
                                return false;
                        }
                        else
                            return false;
                    }
                    else if (i == ps.parameters.Count - 1 && ps.parameters[i].param_kind != parametr_kind.params_parametr)
                        return false;

                }
                return false;
            }
        }

        private SymScope[] selected_methods = null;
        private bool disable_lambda_compilation = false;

        private ProcScope select_method(SymScope[] meths, TypeScope tleft, TypeScope tright, TypeScope obj, bool obj_instanced, params expression[] args)
        {
            List<SymScope> arg_types = new List<SymScope>();
            List<TypeScope> arg_types2 = new List<TypeScope>();
            SymScope[] saved_selected_methods = selected_methods;
            selected_methods = meths;
            if (tleft != null || tright != null)
            {
                if (tleft != null)
                {
                    arg_types.Add(tleft);
                    arg_types2.Add(tleft);
                }
                if (tright != null)
                {
                    arg_types.Add(tright);
                    arg_types2.Add(tright);
                }
            }
            else if (args != null)
            {
                bool tmp_disable_lambda_compilation = disable_lambda_compilation;
                disable_lambda_compilation = true;
                foreach (expression e in args)
                {
                    e.visit(this);
                    returned_scopes.Clear();
                    arg_types.Add(returned_scope);
                    arg_types2.Add(returned_scope as TypeScope);
                }
                disable_lambda_compilation = false;
                disable_lambda_compilation = tmp_disable_lambda_compilation;
            }
            selected_methods = saved_selected_methods;
            List<ProcScope> good_procs = new List<ProcScope>();
            for (int i = 0; i < meths.Length; i++)
            {
                if (meths[i] is ProcScope)
                {
                    if (is_good_overload(meths[i] as ProcScope, arg_types))
                        good_procs.Add(meths[i] as ProcScope);
                }
                else if (meths[i] is ProcType)
                {
                    if (is_good_overload((meths[i] as ProcType).target, arg_types))
                        good_procs.Add((meths[i] as ProcType).target);
                }
            }
            if (good_procs.Count > 1)
                for (int i = 0; i < good_procs.Count; i++)
                    if (DomSyntaxTreeVisitor.is_good_exact_overload(good_procs[i] as ProcScope, arg_types))
                        return good_procs[i].GetInstance(arg_types2);
            if (good_procs.Count == 0)
            {
                for (int i = 0; i < meths.Length; i++)
                {
                    if (meths[i] is ProcScope)
                    {
                        if (obj != null && !(meths[i] as ProcScope).IsStatic)
                        {
                            good_procs.Add(meths[i] as ProcScope);
                        }
                    }
                }
            }
            if (good_procs.Count > 0)
            {
                if (obj != null)
                {
                    if (!obj_instanced && obj.GetElementType() != null && good_procs[0].IsExtension && !(good_procs[0].parameters[0].sc is TemplateParameterScope))
                        obj = obj.GetElementType();
                    arg_types2.Insert(0, obj);
                    arg_types.Insert(0, obj);
                    for (int i = 0; i < good_procs.Count; i++)
                        if (DomSyntaxTreeVisitor.is_good_exact_overload(good_procs[i] as ProcScope, arg_types))
                            return good_procs[i].GetInstance(arg_types2);
                }
                
                return good_procs[0].GetInstance(arg_types2);
            }
            return null;
        }

        public override void visit(method_call _method_call)
        {
            search_all = true;
            _method_call.dereferencing_value.visit(this);
            search_all = false;
            this.cnst_val.prim_val = null;
            SymScope[] names = returned_scopes.ToArray();
            List<expression> parameters = new List<expression>();
            TypeScope obj = null;
            bool obj_instanced = false;
            if (names.Length > 0 && names[0] is TypeScope)
            {
                returned_scope = names[0];
                return;
            }
            foreach (SymScope ss in names)
            {
                if (ss is ProcScope && (ss as ProcScope).is_extension)
                {
                    ProcScope proc = ss as ProcScope;
                    if (_method_call.dereferencing_value is dot_node)
                    {
                        (_method_call.dereferencing_value as dot_node).left.visit(this);
                        obj = returned_scope as TypeScope;
                        if (obj != null && proc.parameters != null && proc.parameters.Count > 0 && !(proc.parameters[0].sc is TemplateParameterScope || proc.parameters[0].sc is UnknownScope))
                        {
                            TypeScope param_type = proc.parameters[0].sc as TypeScope;
                            if (obj.implemented_interfaces != null)
                            {
                                foreach (TypeScope interf in obj.implemented_interfaces)
                                {
                                    if (interf.original_type != null && param_type.original_type != null && interf.original_type == param_type.original_type)
                                    {
                                        List<TypeScope> generic_args = interf.GetInstances();
                                        if (generic_args != null && generic_args.Count > 0)
                                        {
                                            obj = generic_args[0];
                                            obj_instanced = true;
                                        }
                                            
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (_method_call.parameters != null)
            {
                parameters.AddRange(_method_call.parameters.expressions);
            }
            List<TypeScope> lambda_types = new List<TypeScope>();    
            ProcScope ps = select_method(names, null, null, obj, obj_instanced, parameters.ToArray());
            if (_method_call.parameters != null && ps != null)
                for (int i = 0; i < _method_call.parameters.expressions.Count; i++)
                {
                    expression e = _method_call.parameters.expressions[i];
                    if (e is function_lambda_definition)
                    {
                        TypeScope tmp_awaitedProcType = awaitedProcType;
                        if (ps.parameters != null && ps.parameters.Count > i + (ps.IsExtension ? 1 : 0))
                            awaitedProcType = ps.parameters[i + (ps.IsExtension ? 1 : 0)].sc as TypeScope;
                        e.visit(this);
                        awaitedProcType = tmp_awaitedProcType;
                    }
                }
            returned_scopes.Clear();

            if (ps != null)
            {
                if (ps.return_type != null)
                {
                    if (ps.name == "Copy" && ps.return_type.Name == "Array" && _method_call.parameters.expressions.Count > 0)
                    {
                        _method_call.parameters.expressions[0].visit(this);
                        
                    }
                    else
                        returned_scope = ps.return_type;
                }
                else
                    returned_scope = null;
            }
            else if (names.Length > 0)
            {
                returned_scope = null;
                foreach (SymScope ss in names)
                    if (ss is ProcScope)
                    {
                        ps = ss as ProcScope;
                        if (ps.return_type != null)
                        {
                            returned_scope = ps.return_type;
                            if (ps.IsExtension && ps.IsGeneric() && obj != null)
                            {
                                TypeScope elem_ts = obj.GetElementType();
                                if (elem_ts == null)
                                    elem_ts = obj;
                                returned_scope = ps.return_type.GetInstance(new List<TypeScope>() { elem_ts });
                            }
                            break;
                        }
                        else
                            returned_scope = null;
                    }
                    else if (ss is TypeScope)
                    {
                        returned_scope = ss;
                    }
            }
            cnst_val.prim_val = null;
        }

        public override void visit(pascal_set_constant _pascal_set_constant)
        {
            TypeScope element_type = null;
            List<TypeScope> element_types = new List<TypeScope>();
            if (_pascal_set_constant.values != null)
                foreach (expression ex in _pascal_set_constant.values.expressions)
                {
                    ex.visit(this);
                    if (returned_scope != null && returned_scope is TypeScope)
                        element_types.Add(returned_scope as TypeScope);
                }
            if (element_types.Count > 0)
            {
                element_type = element_types[0];
                for (int i = 1; i < element_types.Count; i++)
                {
                    if (element_type.IsConvertable(element_types[i]))
                        element_type = element_types[i];
                    else if (!element_types[i].IsConvertable(element_type))
                    {
                        element_type = TypeTable.obj_type;
                        break;
                    }
                }
            }
            
            if (element_type != null)
                returned_scope = new SetScope(element_type);
            else
                returned_scope = cur_scope.FindName(PascalABCCompiler.TreeConverter.compiler_string_consts.set_name);
            cnst_val.prim_val = null;
        }

        public override void visit(PascalABCCompiler.SyntaxTree.array_const _array_const)
        {
            //throw new Exception("The method or operation is not implemented.");
            cnst_val.prim_val = null;
        }

        public override void visit(write_accessor_name _write_accessor_name)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(read_accessor_name _read_accessor_name)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(property_accessors _property_accessors)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(simple_property _simple_property)
        {
            //throw new Exception("The method or operation is not implemented.");
            _simple_property.property_type.visit(this);
            if (returned_scope == null)
                return;
            ElementScope es = new ElementScope(new SymInfo(_simple_property.property_name.name, SymbolKind.Property, _simple_property.property_name.name),
                                                                                   returned_scope, cur_scope);
            es.declaringUnit = entry_scope;
            if (_simple_property.array_default != null)
            {
                TypeScope ts = cur_scope as TypeScope;
                if (_simple_property.parameter_list != null)
                {
                    ts.elementType = returned_scope as TypeScope;
                    for (int i = 0; i < _simple_property.parameter_list.parameters.Count; i++)
                    {
                        _simple_property.parameter_list.parameters[i].type.visit(this);
                        if (returned_scope == null || !(returned_scope is TypeScope)) return;
                        for (int j = 0; j < _simple_property.parameter_list.parameters[i].names.idents.Count; j++)
                            ts.AddIndexer(returned_scope as TypeScope);
                    }

                }
            }
            if (_simple_property.parameter_list != null)
            {
                es.elementType = returned_scope as TypeScope;
                for (int i = 0; i < _simple_property.parameter_list.parameters.Count; i++)
                {
                    _simple_property.parameter_list.parameters[i].type.visit(this);
                    if (returned_scope == null || !(returned_scope is TypeScope)) return;
                    for (int j = 0; j < _simple_property.parameter_list.parameters[i].names.idents.Count; j++)
                        es.AddIndexer(returned_scope as TypeScope);
                }
                es.MakeDescription();
            }
            es.loc = get_location(_simple_property);
            if (add_doc_from_text && this.converter.controller.docs != null && this.converter.controller.docs.ContainsKey(_simple_property))
                es.AddDocumentation(this.converter.controller.docs[_simple_property]);
            if (_simple_property.attr == definition_attribute.Static)
                es.is_static = true;
            es.acc_mod = cur_access_mod;
            es.si.acc_mod = cur_access_mod;
            if (_simple_property.accessors != null && _simple_property.accessors.write_accessor == null)
            {
                es.is_readonly = true;
                es.MakeDescription();
            }
                
            cur_scope.AddName(_simple_property.property_name.name, es);

        }

        public override void visit(index_property _index_property)
        {
            //throw new Exception("The method or operation is not implemented.");
            if (_index_property.is_default != null)
            {
                TypeScope ts = cur_scope as TypeScope;
                if (_index_property.parameter_list != null)
                {
                    for (int i = 0; i < _index_property.parameter_list.parameters.Count; i++)
                    {
                        _index_property.parameter_list.parameters[i].type.visit(this);
                        if (returned_scope == null || !(returned_scope is TypeScope)) return;
                        for (int j = 0; j < _index_property.parameter_list.parameters[i].names.idents.Count; j++)
                            ts.AddIndexer(returned_scope as TypeScope);
                    }

                }
            }
        }

        public override void visit(class_members _class_members)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(access_modifer_node _access_modifer_node)
        {
            //throw new Exception("The method or operation is not implemented.");
        }
		
        private access_modifer cur_access_mod = access_modifer.none;
        private bool parse_only_method_header = false;
        private bool parse_only_method_body = false;

        public override void visit(class_body_list _class_body)
        {
            //throw new Exception("The method or operation is not implemented.");
            parse_only_method_header = true;
            Dictionary<declaration, SymScope> scopes = new Dictionary<declaration, SymScope>();
            foreach (class_members mems in _class_body.class_def_blocks)
            {
            	if (mems.access_mod != null)
            		cur_access_mod = mems.access_mod.access_level;
               
                foreach (declaration decl in mems.members)
            	{
            		try
            		{      
                        decl.visit(this);
                        scopes.Add(decl, returned_scope);
            		}
            		catch(Exception e)
            		{
            			
            		}
            	}
                
            	cur_access_mod = access_modifer.none;
            }
            parse_only_method_header = false;
            parse_only_method_body = true;
            foreach (class_members mems in _class_body.class_def_blocks)
            {
                if (mems.access_mod != null)
                    cur_access_mod = mems.access_mod.access_level;
                foreach (declaration decl in mems.members)
                {
                    try
                    {
                        if (decl is procedure_definition)
                        {
                            cur_scope = scopes[decl];
                            decl.visit(this);
                        }
                            
                    }
                    catch (Exception e)
                    {

                    }
                }
                cur_access_mod = access_modifer.none;
            }
            parse_only_method_body = false;
        }
		
        private bool has_cyclic_inheritance(TypeScope ts)
        {
        	TypeScope tmp = ts.baseScope;
        	while (tmp != null)
        		if (tmp == ts) 
        		return true;
        		else tmp = tmp.baseScope;
        	return false;
        }

        public override void visit(class_definition _class_definition)
        {
            //throw new Exception("The method or operation is not implemented.");
            SymScope tmp = cur_scope;
            TypeScope ss = null;
            returned_scope = null;
            if (_class_definition.class_parents != null && _class_definition.class_parents.types.Count > 0)
                _class_definition.class_parents.types[0].visit(this);
            if (returned_scope is TypeScope && has_cyclic_inheritance(returned_scope as TypeScope))
                returned_scope = null;
            if (cur_type_name != null)
                ss = cur_scope.FindNameOnlyInType(cur_type_name) as TypeScope;
            if (ss == null || !(ss.members != null && ss.members.Count == 0))
            {
                if (_class_definition.keyword == class_keyword.Record)
                {
                    ss = new TypeScope(SymbolKind.Struct, cur_scope, returned_scope);
                    if (cur_type_name == null)
                        cur_type_name = "$record";
                }
                else if (_class_definition.keyword == class_keyword.Class)
                {
                    ss = new TypeScope(SymbolKind.Class, cur_scope, returned_scope);
                    if ((_class_definition.attribute & class_attribute.Sealed) == class_attribute.Sealed)
                        ss.is_final = true;
                    if ((_class_definition.attribute & class_attribute.Abstract) == class_attribute.Abstract)
                        ss.is_abstract = true;
                    if (cur_type_name == null)
                        cur_type_name = "$class";
                }
                else if (_class_definition.keyword == class_keyword.Interface || _class_definition.keyword == class_keyword.TemplateInterface)
                {
                    ss = new TypeScope(SymbolKind.Interface, cur_scope, returned_scope);
                    if (cur_type_name == null) cur_type_name = "$interface";
                }
                else if (_class_definition.keyword == class_keyword.TemplateClass)
                {
                    ss = new TypeScope(SymbolKind.Class, cur_scope, returned_scope);
                    if (cur_type_name == null) cur_type_name = "$class";
                }
                else if (_class_definition.keyword == class_keyword.TemplateRecord)
                {
                    ss = new TypeScope(SymbolKind.Struct, cur_scope, returned_scope);
                    if (cur_type_name == null) cur_type_name = "$record";
                }
                if (ss != null)
                    cur_scope.AddName(cur_type_name, ss);
            }
            else
                ss.baseScope = returned_scope as TypeScope;
            int num = 0;
            if (_class_definition.keyword != class_keyword.Interface)
                num = 1;
            else
                ss.baseScope = null;
            if (_class_definition.class_parents != null && _class_definition.class_parents.types.Count > num)
            {
                for (int i = num; i < _class_definition.class_parents.types.Count; i++)
                {
                    _class_definition.class_parents.types[i].visit(this);
                    if (returned_scope != null && returned_scope is TypeScope && has_cyclic_inheritance(returned_scope as TypeScope))
                        returned_scope = null;
                    if (returned_scope != null && returned_scope is TypeScope && (returned_scope as TypeScope).si.kind == SymbolKind.Interface)
                        ss.AddImplementedInterface(returned_scope as TypeScope);
                }
            }
            if (has_cyclic_inheritance(ss))
                ss.baseScope = null;
            if (ss.loc != null)
                ss.predef_loc = ss.loc;
            ss.loc = get_location(_class_definition);
            cur_scope = ss;
            
            if (_class_definition.template_args != null)
            {
                foreach (ident id in _class_definition.template_args.idents)
                    ss.AddGenericParameter(id.name);
            }
            else if (template_args != null)
            {
                foreach (ident id in template_args.idents)
                    ss.AddGenericParameter(id.name);
            }
            string tmp_name = cur_type_name;
            cur_type_name = null;
            if (_class_definition.body != null)
            {
                _class_definition.body.visit(this);
                ss.body_loc = get_location(_class_definition);
                ss.real_body_loc = get_location(_class_definition.body);
                ss.AddDefaultConstructorIfNeed();
                if (((_class_definition.attribute & PascalABCCompiler.SyntaxTree.class_attribute.Auto) == class_attribute.Auto))
                {
                    ProcScope ps = new ProcScope("Create", ss, true);
                    foreach (class_members members in _class_definition.body.class_def_blocks)
                    {
                        foreach (declaration decl in members.members)
                        {
                            var_def_statement vds = decl as var_def_statement;
                            if (vds == null)
                                continue;
                            if (vds.vars_type != null)
                                vds.vars_type.visit(this);
                            else
                                vds.inital_value.visit(this);
                            foreach (ident id in vds.vars.list)
                            {
                                ps.parameters.Add(new ElementScope(new SymInfo(id.name, SymbolKind.Parameter, id.name), returned_scope, ps));
                            }
                        }
                    }
                    ps.return_type = ss;
                    ps.Complete();
                    ss.AddName("Create", ps);
                }
            }
            mark_as_possible_abstract(ss);
            cur_type_name = tmp_name;
            returned_scope = ss;
            cur_scope = tmp;
        }

        private void mark_as_possible_abstract(TypeScope ts)
        {
            if (ts.is_abstract)
                return;
            if (ts.baseScope == null || !ts.baseScope.is_abstract)
                return;
            List<ProcScope> meths = ts.baseScope.GetAbstractMethods();
            foreach (ProcScope abstr_meth in meths)
            {
                bool has_override = false;
                foreach (ProcScope meth in ts.GetMethods())
                {
                    if (meth.name == abstr_meth.name && meth.IsParamsEquals(abstr_meth))
                    {
                        has_override = true;
                        break;
                    }
                }
                if (!has_override)
                {
                    ts.is_abstract = true;
                    break;
                }
                    
            }
        }

        public override void visit(default_indexer_property_node _default_indexer_property_node)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(known_type_definition _known_type_definition)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(set_type_definition _set_type_definition)
        {
            //throw new Exception("The method or operation is not implemented.");
            _set_type_definition.of_type.visit(this);
            returned_scope = new SetScope(returned_scope as TypeScope);
            returned_scope.topScope = cur_scope;
        }

        public override void visit(record_const_definition _record_const_definition)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(record_const _record_const)
        {
           // throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(record_type _record_type)
        {
            //throw new Exception("The method or operation is not implemented.");
            SymScope tmp = cur_scope;
            RecordScope rs = new RecordScope();
            rs.loc = get_location(_record_type);
            cur_scope.AddName("$record",rs);
            cur_scope = rs;
            if (_record_type.parts.fixed_part != null)
            	foreach (var_def_statement vds in _record_type.parts.fixed_part.vars)
            {
            	vds.visit(this);
            }
            returned_scope = cur_scope;
            cur_scope = tmp;
        }

        public override void visit(enum_type_definition _enum_type_definition)
        {
            bool is_tuple = false;
            template_type_reference tuple = new template_type_reference();
            tuple.name = new named_type_reference(new ident_list(new ident("System"), new ident("Tuple")).idents);
            template_param_list tpl = new template_param_list();
            tuple.params_list = tpl;
            if (_enum_type_definition.enumerators != null)
                foreach (enumerator en in _enum_type_definition.enumerators.enumerators)
                {
                    if (!(en.name is named_type_reference))
                    {
                        is_tuple = true;
                    }
                    else
                    {
                        named_type_reference ntr = en.name as named_type_reference;
                        if (ntr.names.Count > 1 || ntr.names.Count == 0)
                        {
                            is_tuple = true;
                        }
                        else
                        {
                            SymScope ss = cur_scope.FindName(ntr.FirstIdent.name);
                            if (ss != null && ss is TypeScope)
                            {
                                is_tuple = true;
                            }
                        }
                    }
                    tpl.Add(en.name);
                }
            if (is_tuple)
            {
                tuple.visit(this);
                return;
            }
            //throw new Exception("The method or operation is not implemented.");
            EnumScope enum_scope = new EnumScope(SymbolKind.Enum, cur_scope,
                                         TypeTable.get_compiled_type(new SymInfo(typeof(Enum).Name, SymbolKind.Enum, typeof(Enum).FullName), typeof(Enum)));
            enum_scope.loc = get_location(_enum_type_definition);
            enum_scope.topScope = cur_scope;
            List<ElementScope> elems = new List<ElementScope>();
            if (_enum_type_definition.enumerators != null)
                foreach (enumerator en in _enum_type_definition.enumerators.enumerators)
                {
                    var name = (en.name as named_type_reference).FirstIdent.name;
                    ElementScope ss = new ElementScope(new SymInfo(name, SymbolKind.Constant, name),/*cur_scope.FindName(PascalABCCompiler.TreeConverter.compiler_string_consts.integer_type_name)*/enum_scope, cur_scope);
                    ss.is_static = true;
                    ss.cnst_val = name;
                    elems.Add(ss);
                    ss.loc = get_location(en);
                    cur_scope.AddName(name, ss);
                    enum_scope.AddName(name, ss);
                    enum_scope.AddEnumConstant(name);
                    if (this.converter.controller.docs.ContainsKey(en))
                        ss.AddDocumentation(this.converter.controller.docs[en]);
                }
            for (int i = 0; i < elems.Count; i++)
                elems[i].MakeDescription();
            returned_scope = enum_scope;
        }

        public override void visit(char_const _char_const)
        {
            returned_scope = TypeTable.char_type;//entry_scope.FindName(PascalABCCompiler.TreeConverter.compiler_string_consts.char_type_name);
            if (in_kav)
            	cnst_val.prim_val = this.converter.controller.Parser.LanguageInformation.GetStringForChar(_char_const.cconst);
            //cnst_val.prim_val = "'"+_char_const.cconst.ToString()+"'";
            else cnst_val.prim_val = _char_const.cconst;
        }

        public override void visit(raise_statement _raise_statement)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(sharp_char_const _sharp_char_const)
        {
            returned_scope = TypeTable.char_type;//entry_scope.FindName(PascalABCCompiler.TreeConverter.compiler_string_consts.char_type_name);
            cnst_val.prim_val = this.converter.controller.Parser.LanguageInformation.GetStringForSharpChar(_sharp_char_const.char_num);
        }
		
        private bool in_kav=true;
        public override void visit(literal_const_line _literal_const_line)
        {
            //entry_scope.FindName(PascalABCCompiler.TreeConverter.compiler_string_consts.string_type_name);
            StringBuilder sb = new StringBuilder();
            in_kav = false;
            for (int i = 0; i < _literal_const_line.literals.Count; i++)
            {
                _literal_const_line.literals[i].visit(this);
                if (cnst_val.prim_val != null && cnst_val.prim_val is char)
                    sb.Append((char)cnst_val.prim_val);
                else if (cnst_val.prim_val != null && cnst_val.prim_val is string)
                    sb.Append((string)cnst_val.prim_val);
            }
            in_kav = true;
            returned_scope = TypeTable.string_type;
            cnst_val.prim_val = sb.ToString();
        }

        public override void visit(string_num_definition _string_num_definition)
        {
            //throw new Exception("The method or operation is not implemented.");
            try
            {
            	_string_num_definition.num_of_symbols.visit(this);
            }
            catch(Exception e)
            {
            	
            }
            returned_scope = new ShortStringScope(TypeTable.string_type,cnst_val.prim_val);//entry_scope.FindName(PascalABCCompiler.TreeConverter.compiler_string_consts.string_type_name);
            returned_scope.topScope = cur_scope;
            returned_scope.loc = get_location(_string_num_definition);
        }

        public override void visit(variant _variant)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(variant_list _variant_list)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(variant_type _variant_type)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(variant_types _variant_types)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(variant_record_type _variant_record_type)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(procedure_call _procedure_call)
        {
            //throw new Exception("The method or operation is not implemented.");
            if (has_lambdas(_procedure_call.func_name))
            {
                _procedure_call.func_name.visit(this);
            }
        }

        public override void visit(class_predefinition _class_predefinition)
        {
            //throw new Exception("The method or operation is not implemented.");
           
        }

        public override void visit(nil_const _nil_const)
        {
        	returned_scope = new NullTypeScope();
        }

        public override void visit(file_type_definition _file_type_definition)
        {
            //throw new Exception("The method or operation is not implemented.");
            if (_file_type_definition.elem_type != null)
            _file_type_definition.elem_type.visit(this);
            returned_scope = new FileScope(returned_scope as TypeScope, cur_scope);
        }

        public override void visit(constructor _constructor)
        {
            SymScope topScope = cur_scope;
            ProcScope ps = null;
            location loc = get_location(_constructor);
            bool is_realization = false;
            if (_constructor.name != null)
            {
                _constructor.name.visit(this);
                if (_constructor.name.class_name != null)
                {
                    topScope = cur_scope.FindName(_constructor.name.class_name.name);
                    if (topScope != null)
                    {
                        ps = topScope.FindNameOnlyInThisType(meth_name) as ProcScope;
                        if (ps == null)
                        {
                            ps = new ProcScope("Create", cur_scope, true);
                            ps.head_loc = loc;
                        }
                        else
                            ps = select_function_definition(ps, _constructor.parameters, topScope as TypeScope, topScope as TypeScope);
                        //while (ps != null && ps.already_defined) ps = ps.nextProc;
                        if (ps == null)
                        {
                            ps = new ProcScope(meth_name, cur_scope);
                            ps.head_loc = loc;
                        }
                        if (ps.parameters.Count != 0 && _constructor.parameters != null)
                        {
                            ps.parameters.Clear();
                            ps.already_defined = true;
                        }
                        if (impl_scope == null)
                        {
                            ProcRealization pr = new ProcRealization(ps, cur_scope);
                            pr.already_defined = true;
                            ps.procRealization = pr;
                            pr.loc = cur_loc;
                            pr.head_loc = loc;
                            is_realization = true;
                            entry_scope.AddName("$method", pr);
                        }
                        else
                        {
                            ProcRealization pr = new ProcRealization(ps, impl_scope);
                            pr.already_defined = true;
                            ps.procRealization = pr;
                            pr.loc = cur_loc;
                            pr.head_loc = loc;
                            is_realization = true;
                            impl_scope.AddName("$method", pr);
                        }
                    }
                    else
                    {
                        ps = new ProcScope("Create", cur_scope, true);
                        ps.head_loc = loc;
                    }
                }
                else
                {
                    ps = new ProcScope("Create", cur_scope, true);
                    ps.head_loc = loc;
                    SymScope ss = cur_scope.FindNameOnlyInThisType("Create");
                    if (ss != null && ss is ProcScope)
                    {
                        if (ps.topScope == ss.topScope)
                        {
                            while ((ss as ProcScope).nextProc != null) ss = (ss as ProcScope).nextProc;
                            (ss as ProcScope).nextProc = ps;
                        }
                        else
                            ps.nextProc = ss as ProcScope;
                        cur_scope.AddName("$method", ps);
                        ps.si.name = "Create";
                    }
                    else
                    {
                        cur_scope.AddName(meth_name, ps);
                    }
                    ps.return_type = cur_scope as TypeScope;
                }
            }
            else
            {
                ps = new ProcScope("Create", cur_scope, true);
                ps.head_loc = loc;
                SymScope ss = cur_scope.FindNameOnlyInType("Create");
                if (ss != null && ss is ProcScope)
                {
                    while ((ss as ProcScope).nextProc != null) ss = (ss as ProcScope).nextProc;
                    (ss as ProcScope).nextProc = ps;
                    cur_scope.AddName("$method", ps);
                    ps.si.name = "Create";
                }
                else
                {
                    cur_scope.AddName("Create", ps);
                }
                ps.return_type = cur_scope as TypeScope;
            }
            if (!is_realization && ps.loc == null)
            {
                ps.loc = cur_loc;
            }
            //ps.head_loc = loc;
            ps.declaringUnit = entry_scope;
            SetAttributes(ps, _constructor.proc_attributes);
            if (add_doc_from_text && this.converter.controller.docs != null && this.converter.controller.docs.ContainsKey(_constructor))
                ps.AddDocumentation(this.converter.controller.docs[_constructor]);
            if (is_proc_realization) ps.already_defined = true;
            else ps.loc = loc;
            if (_constructor.name == null || _constructor.name.class_name == null)
            {
                ps.acc_mod = cur_access_mod;
                ps.si.acc_mod = cur_access_mod;
            }
            if (_constructor.parameters != null)
                foreach (typed_parameters pars in _constructor.parameters.params_list)
                {
                    pars.vars_type.visit(this);
                    if (returned_scope != null)
                    {
                        if (returned_scope is ProcScope)
                            returned_scope = new ProcType(returned_scope as ProcScope);
                        foreach (ident id in pars.idents.idents)
                        {
                            ElementScope si = new ElementScope(new SymInfo(id.name, SymbolKind.Parameter, id.name), returned_scope, ps);
                            si.loc = get_location(id);
                            ps.AddName(id.name, si);
                            if (pars.inital_value != null)
                            {
                                pars.inital_value.visit(this);
                                si.cnst_val = cnst_val.prim_val;
                            }
                            si.param_kind = pars.param_kind;
                            si.MakeDescription();
                            ps.AddParameter(si);
                        }
                    }
                }
            if (cur_scope is TypeScope && !ps.is_static)
                ps.AddName("self", new ElementScope(new SymInfo("self", SymbolKind.Parameter, "self"), cur_scope, ps));

            //cur_scope = ps;
            returned_scope = ps;
            ps.Complete();
        }

        public override void visit(destructor _destructor)
        {
            //throw new Exception("The method or operation is not implemented.");
            SymScope topScope = cur_scope;
            ProcScope ps = null;
            location loc = get_location(_destructor);
            bool is_realization = false;
            _destructor.name.visit(this);
            if (_destructor.name.class_name != null)
            {
                topScope = cur_scope.FindName(_destructor.name.class_name.name);
                if (topScope != null)
                {
                    ps = topScope.FindNameOnlyInThisType(meth_name) as ProcScope;
                    if (ps == null)
                    {
                        ps = new ProcScope(meth_name, cur_scope);
                        ps.head_loc = loc;
                    }
                    else ps = select_function_definition(ps, _destructor.parameters, null, topScope as TypeScope);
                    //while (ps != null && ps.already_defined) ps = ps.nextProc;
                    if (ps == null)
                    {
                        ps = new ProcScope(meth_name, cur_scope);
                        ps.head_loc = loc;
                    }
                    if (ps.parameters.Count != 0 && _destructor.parameters != null)
                    {
                        ps.parameters.Clear();
                        ps.already_defined = true;
                    }
                    if (impl_scope == null)
                    {
                        ProcRealization pr = new ProcRealization(ps, cur_scope);
                        pr.already_defined = true;
                        pr.loc = cur_loc;
                        pr.head_loc = loc;
                        is_realization = true;
                        entry_scope.AddName("$method", pr);

                    }
                    else
                    {
                        ProcRealization pr = new ProcRealization(ps, impl_scope);
                        pr.already_defined = true;
                        pr.loc = cur_loc;
                        pr.head_loc = loc;
                        is_realization = true;
                        impl_scope.AddName("$method", pr);
                    }
                }
                else
                {
                    ps = new ProcScope(meth_name, cur_scope);
                    ps.head_loc = loc;
                }
            }
            else
            {
                ps = new ProcScope(meth_name, cur_scope);
                ps.head_loc = loc;
                SymScope ss = cur_scope.FindNameOnlyInThisType(meth_name);
                if (ss != null && ss is ProcScope)
                {
                    while ((ss as ProcScope).nextProc != null) ss = (ss as ProcScope).nextProc;
                    (ss as ProcScope).nextProc = ps;
                    cur_scope.AddName("$method", ps);
                    ps.si.name = meth_name;
                }
                else
                {
                    cur_scope.AddName(meth_name, ps);
                }
            }
            if (!is_realization && ps.loc == null)
                ps.loc = cur_loc;
            //ps.head_loc = loc;
            ps.declaringUnit = entry_scope;
            SetAttributes(ps, _destructor.proc_attributes);
            if (add_doc_from_text && this.converter.controller.docs != null && this.converter.controller.docs.ContainsKey(_destructor))
                ps.AddDocumentation(this.converter.controller.docs[_destructor]);
            if (is_proc_realization) ps.already_defined = true;
            else ps.loc = loc;
            if (_destructor.name == null || _destructor.name.class_name == null)
            {
                ps.acc_mod = cur_access_mod;
                ps.si.acc_mod = cur_access_mod;
            }
            if (_destructor.parameters != null)
                foreach (typed_parameters pars in _destructor.parameters.params_list)
                {
                    pars.vars_type.visit(this);
                    if (returned_scope != null)
                    {
                        if (returned_scope is ProcScope)
                            returned_scope = new ProcType(returned_scope as ProcScope);
                        foreach (ident id in pars.idents.idents)
                        {
                            ElementScope si = new ElementScope(new SymInfo(id.name, SymbolKind.Parameter, id.name), returned_scope, ps);
                            si.loc = get_location(id);
                            ps.AddName(id.name, si);
                            si.param_kind = pars.param_kind;
                            si.MakeDescription();
                            ps.AddParameter(si);
                        }
                    }
                }
            if (cur_scope is TypeScope)
                ps.AddName("self", new ElementScope(new SymInfo("self", SymbolKind.Parameter, "self"), cur_scope, ps));
            //cur_scope = ps;
            returned_scope = ps;
            ps.Complete();
        }

        public override void visit(inherited_method_call _inherited_method_call)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(typecast_node _typecast_node)
        {
            if (_typecast_node.cast_op == op_typecast.is_op)
                returned_scope = TypeTable.bool_type;
            else
                _typecast_node.type_def.visit(this);
        }

        public override void visit(interface_node _interface_node)
        {
            //throw new Exception("The method or operation is not implemented.");
            bool has_system_unit = false;
            bool has_extensions_unit = false;
            if (_interface_node.uses_modules != null)
            {
                (cur_scope as InterfaceUnitScope).uses_source_range = get_location(_interface_node.uses_modules);
                //foreach (unit_or_namespace s in _interface_node.uses_modules.units)
                for (int j = _interface_node.uses_modules.units.Count - 1; j >= 0; j--)
                {
                    unit_or_namespace s = _interface_node.uses_modules.units[j];
                    string str = "";
                    for (int i = 0; i < s.name.idents.Count; i++)
                    {
                        str += s.name.idents[i].name;
                        NamespaceScope ns_scope = null;
                        if (i == 0)
                        {
                            try
                            {
                                string unit_name = null;
                                string pcu_unit_name = FindPCUFileName(str);
                                if (s is uses_unit_in)
                                    unit_name = (s as uses_unit_in).in_file.Value;
                                else
                                    unit_name = CodeCompletionNameHelper.FindSourceFileName(str, System.IO.Path.GetDirectoryName(this.cur_unit_file_name));
                                if (pcu_unit_name != null && unit_name != null && string.Compare(System.IO.Path.GetDirectoryName(this.cur_unit_file_name), System.IO.Path.GetDirectoryName(pcu_unit_name), true) == 0
                                    && string.Compare(System.IO.Path.GetDirectoryName(this.cur_unit_file_name), System.IO.Path.GetDirectoryName(unit_name), true) != 0)
                                    unit_name = null;
                                if (unit_name != null)
                                {
                                    DomConverter dc = CodeCompletionController.comp_modules[unit_name] as DomConverter;
                                    if (dc == null)
                                    {
                                        dc = new CodeCompletionController().CompileAllIfNeed(unit_name, true);
                                    }
                                    if (dc.visitor != null)
                                    {
                                        dc.visitor.entry_scope.InitAssemblies();
                                        cur_scope.AddUsedUnit(dc.visitor.entry_scope);
                                        cur_scope.AddName(str, dc.visitor.entry_scope);
                                    }
                                }
                                else
                                {
                                    //unit_name = FindPCUFileName(str);
                                    unit_name = pcu_unit_name;
                                    if (unit_name != null)
                                    {
                                        IntellisensePCUReader pcu_rdr = new IntellisensePCUReader();
                                        SymScope ss = pcu_rdr.GetUnit(unit_name);
                                        UnitDocCache.Load(ss, unit_name);
                                        cur_scope.AddUsedUnit(ss);
                                        cur_scope.AddName(str, ss);
                                    }
                                    else
                                    {
                                        if (PascalABCCompiler.NetHelper.NetHelper.IsNetNamespace(str))
                                        {
                                            ns_scope = new NamespaceScope(str);
                                            ns_cache[str] = str;
                                            cur_scope.AddName(str, ns_scope);
                                            if (s.name.idents.Count == 1)
                                                cur_scope.AddUsedUnit(ns_scope);
                                        }
                                        else if (PascalABCCompiler.NetHelper.NetHelper.IsType(str) && semantic_options.allow_import_types)
                                        {
                                            Type t = PascalABCCompiler.NetHelper.NetHelper.FindType(str);
                                            cur_scope.AddUsedUnit(new NamespaceTypeScope(TypeTable.get_compiled_type(new SymInfo(t.Name, SymbolKind.Class, t.FullName), t)));
                                        }
                                    }
                                    //unit_name = System.IO.Path.GetDirectoryName(_program_module.file_name)+"\\"+str+System.IO.Path.GetExtension(_program_module.file_name);
                                }
                            }
                            catch (Exception e)
                            {
#if DEBUG
                                File.AppendAllText("log.txt", e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
#endif
                            }
                        }
                        if (i == s.name.idents.Count - 1 && i > 0 /*&& PascalABCCompiler.NetHelper.NetHelper.IsNetNamespace(str)*/)
                            cur_scope.AddUsedUnit(new NamespaceScope(str));
                        if (i < s.name.idents.Count - 1) str += ".";

                    }
                    if (string.Compare(str, PascalABCCompiler.TreeConverter.compiler_string_consts.system_unit_name, true) == 0)
                        has_system_unit = true;
                    if (string.Compare(str, PascalABCCompiler.TreeConverter.compiler_string_consts.extensions_unit_file_name, true) == 0)
                        has_extensions_unit = true;
                    unl.AddElement(new PascalABCCompiler.TreeRealization.using_namespace(str));
                }
            }
            //if (_interface_node.unit_name.idunit_name.name != PascalABCCompiler.TreeConverter.compiler_string_consts.system_unit_file_name)
            if (!is_system_unit && !has_system_unit)
                add_system_unit();
            if (!is_system_unit && !is_extensions_unit && !has_extensions_unit)
                add_extensions_unit();
            if (_interface_node.interface_definitions != null)
                foreach (declaration decl in _interface_node.interface_definitions.defs)
                {
                    try
                    {
                        decl.visit(this);
                    }
                    catch (Exception e)
                    {
#if DEBUG
                        File.AppendAllText("log.txt", e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
#endif
                    }
                }

        }

        public override void visit(implementation_node _implementation_node)
        {
            //throw new Exception("The method or operation is not implemented.");
            SymScope tmp = cur_scope;
            unl.clear();
            cur_scope = new ImplementationUnitScope(new SymInfo("$implementation", SymbolKind.Namespace, "implementation"), cur_scope);
            tmp.AddName("$implementation", cur_scope);
            (tmp as InterfaceUnitScope).impl_scope = cur_scope as ImplementationUnitScope;
            cur_scope.loc = get_location(_implementation_node);
            if (_implementation_node.uses_modules != null)
            {
                (cur_scope as ImplementationUnitScope).uses_source_range = get_location(_implementation_node.uses_modules);
                for (int j = _implementation_node.uses_modules.units.Count - 1; j >= 0; j--)
                {
                    unit_or_namespace s = _implementation_node.uses_modules.units[j];
                    string str = "";
                    for (int i = 0; i < s.name.idents.Count; i++)
                    {
                        str += s.name.idents[i].name;
                        NamespaceScope ns_scope = null;
                        if (i == 0)
                        {
                            try
                            {
                                string unit_name = null;
                                string pcu_unit_name = FindPCUFileName(str);
                                if (s is uses_unit_in)
                                    unit_name = (s as uses_unit_in).in_file.Value;
                                else
                                    unit_name = CodeCompletionNameHelper.FindSourceFileName(str, System.IO.Path.GetDirectoryName(this.cur_unit_file_name));
                                if (pcu_unit_name != null && unit_name != null && string.Compare(System.IO.Path.GetDirectoryName(this.cur_unit_file_name), System.IO.Path.GetDirectoryName(pcu_unit_name), true) == 0
                                       && string.Compare(System.IO.Path.GetDirectoryName(this.cur_unit_file_name), System.IO.Path.GetDirectoryName(unit_name), true) != 0)
                                    unit_name = null;
                                if (unit_name != null)
                                {
                                    DomConverter dc = CodeCompletionController.comp_modules[unit_name] as DomConverter;
                                    if (dc == null /*|| CodeCompletionController.recomp_files[unit_name] != null*/)
                                    {
                                        dc = new CodeCompletionController().CompileAllIfNeed(unit_name, true);
                                    }
                                    if (dc.visitor != null)
                                    {
                                        dc.visitor.entry_scope.InitAssemblies();
                                        cur_scope.AddUsedUnit(dc.visitor.entry_scope);
                                        cur_scope.AddName(str, dc.visitor.entry_scope);
                                    }
                                }
                                else
                                {
                                    //unit_name = FindPCUFileName(str);
                                    unit_name = pcu_unit_name;
                                    if (unit_name != null)
                                    {
                                        IntellisensePCUReader pcu_rdr = new IntellisensePCUReader();
                                        SymScope ss = pcu_rdr.GetUnit(unit_name);
                                        UnitDocCache.Load(ss, unit_name);
                                        cur_scope.AddUsedUnit(ss);
                                        cur_scope.AddName(str, ss);
                                    }
                                    else
                                    {
                                        if (PascalABCCompiler.NetHelper.NetHelper.IsNetNamespace(str))
                                        {
                                            ns_scope = new NamespaceScope(str);
                                            cur_scope.AddName(str, ns_scope);
                                            if (s.name.idents.Count == 1)
                                                cur_scope.AddUsedUnit(ns_scope);
                                        }
                                        else if (PascalABCCompiler.NetHelper.NetHelper.IsType(str) && semantic_options.allow_import_types)
                                        {
                                            Type t = PascalABCCompiler.NetHelper.NetHelper.FindType(str);
                                            cur_scope.AddUsedUnit(new NamespaceTypeScope(TypeTable.get_compiled_type(new SymInfo(t.Name, SymbolKind.Class, t.FullName), t)));
                                        }
                                    }
                                    //unit_name = System.IO.Path.GetDirectoryName(_program_module.file_name)+"\\"+str+System.IO.Path.GetExtension(_program_module.file_name);
                                }
                            }
                            catch (Exception e)
                            {
#if DEBUG
                                File.AppendAllText("log.txt", e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
#endif
                            }
                        }
                        if (i == s.name.idents.Count - 1 && i > 0 /*&& PascalABCCompiler.NetHelper.NetHelper.IsNetNamespace(str)*/)
                            cur_scope.AddUsedUnit(new NamespaceScope(str));
                        if (i < s.name.idents.Count - 1)
                            str += ".";
                    }

                    unl.AddElement(new PascalABCCompiler.TreeRealization.using_namespace(str));
                }
            }
            impl_scope = cur_scope;
            if (_implementation_node.implementation_definitions != null)
                foreach (declaration decl in _implementation_node.implementation_definitions.defs)
                {
                    try
                    {
                        if (parse_only_interface && !need_to_parse(decl))
                            continue;
                        decl.visit(this);
                    }
                    catch (Exception e)
                    {
#if DEBUG
                        File.AppendAllText("log.txt", e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
#endif
                        cur_scope = impl_scope;
                    }
                }
        }

        private bool need_to_parse(declaration _declaration)
        {
            if (!(_declaration is procedure_definition))
                return false;
            procedure_definition proc = _declaration as procedure_definition;
            if (proc.proc_header.name.class_name != null)
                return true;
            if (proc.proc_header.proc_attributes == null)
                return false;
            if (has_extensionmethod_attr(proc.proc_header.proc_attributes.proc_attributes))
                return true;
            return false;
        }

        public override void visit(diap_expr _diap_expr)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(block _block)
        {
            //throw new Exception("The method or operation is not implemented.");
            if (_block.defs != null)
                _block.defs.visit(this);
            if (_block.program_code != null)
                _block.program_code.visit(this);
            //cur_scope.loc = get_location(_block.program_code);
        }

        public override void visit(proc_block _proc_block)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(array_of_named_type_definition _array_of_named_type_definition)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(array_of_const_type_definition _array_of_const_type_definition)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(literal _literal)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(case_variants _case_variants)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(diapason_expr _diapason_expr)
        {
            _diapason_expr.left.visit(this);
        }

        public override void visit(var_def_list_for_record _var_def_list)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(record_type_parts _record_type_parts)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(property_array_default _property_array_default)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(property_interface _property_interface)
        {
           // throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(property_parameter _property_parameter)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(property_parameter_list _property_parameter_list)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(inherited_ident _inherited_ident)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(format_expr _format_expr)
        {
            //throw new Exception("The method or operation is not implemented.");
            returned_scope = TypeTable.string_type;
        }

        public override void visit(initfinal_part _initfinal_part)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(token_info _token_info)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(raise_stmt _raise_stmt)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(op_type_node _op_type_node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(file_type _file_type)
        {
        	 if (_file_type.file_of_type != null)
            _file_type.file_of_type.visit(this);
            returned_scope = new FileScope(returned_scope as TypeScope, cur_scope);
        }

        public override void visit(known_type_ident _known_type_ident)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(exception_handler _exception_handler)
        {
            SymScope tmp = cur_scope;
            SymScope stmt_scope = new BlockScope(cur_scope);
            cur_scope.AddName("$block_scope", stmt_scope);
            stmt_scope.loc = get_location(_exception_handler);
            if (_exception_handler.statements.source_context != null)
                stmt_scope.loc = new location(stmt_scope.loc.begin_line_num, stmt_scope.loc.begin_column_num,
                                              _exception_handler.statements.source_context.end_position.line_num,
                                              _exception_handler.statements.source_context.end_position.column_num, stmt_scope.loc.doc);
            returned_scope = null;
            if (_exception_handler.variable == null) return;
            if (_exception_handler.type_name != null)
                _exception_handler.type_name.visit(this);
            else returned_scope = cur_scope.FindName(_exception_handler.variable.name);
            if (returned_scope != null)
            {
                cur_scope = stmt_scope;
                ElementScope es = new ElementScope(new SymInfo(_exception_handler.variable.name, SymbolKind.Variable, _exception_handler.variable.name), returned_scope, cur_scope);
                es.loc = get_location(_exception_handler.variable);
                stmt_scope.AddName(_exception_handler.variable.name, es);
            }
            _exception_handler.statements.visit(this);
            cur_scope = tmp;
        }

        public override void visit(exception_ident _exception_ident)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(exception_handler_list _exception_handler_list)
        {
            foreach (exception_handler eh in _exception_handler_list.handlers)
				eh.visit(this);
        }

        public override void visit(exception_block _exception_block)
        {
            if (_exception_block.handlers != null)
                _exception_block.handlers.visit(this);
            if (_exception_block.stmt_list != null)
                _exception_block.stmt_list.visit(this);
            if (_exception_block.else_stmt_list != null)
                _exception_block.else_stmt_list.visit(this);
        }

        public override void visit(try_handler _try_handler)
        {
            
        }

        public override void visit(try_handler_finally _try_handler_finally)
        {
            _try_handler_finally.stmt_list.visit(this);
        }

        public override void visit(try_handler_except _try_handler_except)
        {
            _try_handler_except.except_block.visit(this);
        }

        public override void visit(try_stmt _try_stmt)
        {
            if (_try_stmt.stmt_list != null)
                _try_stmt.stmt_list.visit(this);
            if (_try_stmt.handler != null)
                _try_stmt.handler.visit(this);
        }

        public override void visit(inherited_message _inherited_message)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(external_directive _external_directive)
        {
            
        }

        public override void visit(using_list _using_list)
        {
            throw new Exception("The method or operation is not implemented.");
        }


        public override void visit(jump_stmt _jump_stmt)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(loop_stmt _loop_stmt)
        {
            SymScope tmp = cur_scope;
            SymScope stmt_scope = new BlockScope(cur_scope);
            cur_scope.AddName("$block_scope", stmt_scope);
            stmt_scope.loc = get_location(_loop_stmt);
            cur_scope = stmt_scope;
            if (_loop_stmt.stmt != null)
                _loop_stmt.stmt.visit(this);
            cur_scope = tmp;
        }

        public override void visit(foreach_stmt _foreach_stmt)
        {
            //throw new Exception("The method or operation is not implemented.");
            SymScope tmp = cur_scope;
            if (_foreach_stmt.type_name != null)
            {
            	SymScope stmt_scope = new BlockScope(cur_scope);
        		cur_scope.AddName("$block_scope",stmt_scope);
        		stmt_scope.loc = get_location(_foreach_stmt);
                if (_foreach_stmt.type_name is no_type_foreach)
                {
                    _foreach_stmt.in_what.visit(this);
                    if (returned_scope != null)
                        returned_scope = returned_scope.GetElementType();
                    if (returned_scope == null)
                        returned_scope = TypeTable.obj_type;
                }
                else
                {
                    _foreach_stmt.type_name.visit(this);
                }
        		if (returned_scope != null)
        		{
        			cur_scope = stmt_scope;
        			ElementScope es = new ElementScope(new SymInfo(_foreach_stmt.identifier.name, SymbolKind.Variable,_foreach_stmt.identifier.name),returned_scope,cur_scope);
        			es.loc = get_location(_foreach_stmt.identifier);
        			stmt_scope.AddName(_foreach_stmt.identifier.name,es);
        		}
            }
            if (_foreach_stmt.stmt != null)
            _foreach_stmt.stmt.visit(this);
            cur_scope = tmp;
        }

        public override void visit(addressed_value_funcname _addressed_value_funcname)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(named_type_reference_list _named_type_reference_list)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(template_param_list _template_param_list)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(template_type_reference _template_type_reference)
        {
            returned_scope = null;
            converted_template_type = _template_type_reference;
            _template_type_reference.name.visit(this);
            converted_template_type = null;
            List<TypeScope> gen_args = new List<TypeScope>();
            if (returned_scope != null && returned_scope is TypeScope)
            {
            	TypeScope ts = returned_scope as TypeScope;
            	
            	if (_template_type_reference.params_list != null)
            	{
            		List<TypeScope> instances = new List<TypeScope>();
            		foreach (type_definition td in _template_type_reference.params_list.params_list)
            		{
            			returned_scope = null;
            			td.visit(this);
            			if (returned_scope != null && returned_scope is TypeScope) 
            				//instances.Add(ret_tn as TypeScope);
            				//ts.AddGenericInstanciation(ret_tn as TypeScope);
            				gen_args.Add(returned_scope as TypeScope);
            		}
            		
            	}
            	if (gen_args.Count > 0)
            		ts = ts.GetInstance(gen_args, true);
            	//ts.MakeInstance();
            	//ret_tn = ts.GetGenericInstance(gen_args);
            	//ts.si.describe = ts.ToString();
            	//else
            	returned_scope = ts;
            }
        }

        public override void visit(int64_const _int64_const)
        {
            //throw new Exception("The method or operation is not implemented.");
            returned_scope = TypeTable.int64_type;//entry_scope.FindName(PascalABCCompiler.TreeConverter.compiler_string_consts.long_type_name);
        	cnst_val.prim_val = _int64_const.val;
        }

        public override void visit(uint64_const _uint64_const)
        {
            returned_scope = TypeTable.uint64_type;//entry_scope.FindName(PascalABCCompiler.TreeConverter.compiler_string_consts.ulong_type_name);
           cnst_val.prim_val = _uint64_const.val;
        }

        public override void visit(new_expr _new_expr)
        {
            _new_expr.type.visit(this);
            if (_new_expr.new_array && returned_scope != null && returned_scope is TypeScope)
            {
                List<TypeScope> indexes = new List<TypeScope>();
                indexes.Add(TypeTable.int_type);
                for (int i = 1; i < _new_expr.params_list.expressions.Count; i++)
                    indexes.Add(TypeTable.int_type);
                if (indexes.Count > 1)
                {
                    for (int i = 0; i < indexes.Count; i++)
                    {
                        indexes[i] = null;
                    }
                }
            	returned_scope = new ArrayScope(returned_scope as TypeScope, indexes.ToArray());
                if (indexes.Count == 1)
            	    (returned_scope as ArrayScope).is_dynamic_arr = true;
            }
            save_return_value();
            if (_new_expr.params_list != null)
            foreach (expression ex in _new_expr.params_list.expressions)
            {
                if (ex is function_lambda_definition)
                    ex.visit(this);
            }
            restore_return_value();
			/*if (ret_tn != null && ret_tn is TypeScope)
			{
				TypeScope ts = ret_tn as TypeScope;
				ret_tn = ts.GetConstructor();
			}*/
        }

        public override void visit(where_type_specificator_list _type_definition_list)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(where_definition _where_definition)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(where_definition_list _where_definition_list)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(PascalABCCompiler.SyntaxTree.sizeof_operator _sizeof_operator)
        {
           returned_scope = TypeTable.int_type;
        }

        public override void visit(PascalABCCompiler.SyntaxTree.typeof_operator _typeof_operator)
        {
            //throw new Exception("The method or operation is not implemented.");
            returned_scope = TypeTable.get_compiled_type(new SymInfo(null, SymbolKind.Class,null),typeof(Type));
        }

        public override void visit(PascalABCCompiler.SyntaxTree.compiler_directive _compiler_directive)
        {
            //throw new Exception("The method or operation is not implemented.");
        }
		
        private string get_operator_sign(Operators op)
        {
        	switch(op)
        	{
        		case Operators.Plus : return PascalABCCompiler.TreeConverter.compiler_string_consts.plus_name;
        		case Operators.Minus : return PascalABCCompiler.TreeConverter.compiler_string_consts.minus_name;
        		case Operators.Division : return PascalABCCompiler.TreeConverter.compiler_string_consts.div_name;
                case Operators.IntegerDivision: return PascalABCCompiler.TreeConverter.compiler_string_consts.div_name;
                case Operators.Multiplication : return PascalABCCompiler.TreeConverter.compiler_string_consts.mul_name;
        		case Operators.ModulusRemainder : return PascalABCCompiler.TreeConverter.compiler_string_consts.mod_name;
        		case Operators.Less : return PascalABCCompiler.TreeConverter.compiler_string_consts.sm_name;
        		case Operators.LessEqual : return PascalABCCompiler.TreeConverter.compiler_string_consts.smeq_name;
        		case Operators.Greater : return PascalABCCompiler.TreeConverter.compiler_string_consts.gr_name;
        		case Operators.GreaterEqual : return PascalABCCompiler.TreeConverter.compiler_string_consts.greq_name;
        		case Operators.Equal : return PascalABCCompiler.TreeConverter.compiler_string_consts.eq_name;
        		case Operators.NotEqual : return PascalABCCompiler.TreeConverter.compiler_string_consts.noteq_name;
        		case Operators.AssignmentAddition : return PascalABCCompiler.TreeConverter.compiler_string_consts.plusassign_name;
        		case Operators.AssignmentMultiplication : return PascalABCCompiler.TreeConverter.compiler_string_consts.multassign_name;
        		case Operators.AssignmentSubtraction : return PascalABCCompiler.TreeConverter.compiler_string_consts.minusassign_name;
        		case Operators.AssignmentDivision : return PascalABCCompiler.TreeConverter.compiler_string_consts.divassign_name;
        		case Operators.Implicit : return "implicit";
        		case Operators.Explicit : return "explicit";
                case Operators.In: return PascalABCCompiler.TreeConverter.compiler_string_consts.in_name;
                case Operators.Power: return PascalABCCompiler.TreeConverter.compiler_string_consts.power_name;
            }
        	return "";
        }
        
        public override void visit(operator_name_ident _operator_name_ident)
        {
            //throw new Exception("The method or operation is not implemented.");
            string sign = get_operator_sign(_operator_name_ident.operator_type);
            if (sign.Length > 0 && char.IsLetter(sign[0]))
                sign = " " + sign;
            meth_name = "operator"+sign;
        }

        public override void visit(var_statement _var_statement)
        {
            //throw new Exception("The method or operation is not implemented.");
            _var_statement.var_def.visit(this);
        }

        public override void visit(PascalABCCompiler.SyntaxTree.question_colon_expression _question_colon_expression)
        {
        	_question_colon_expression.ret_if_true.visit(this);
        }

        public override void visit(expression_as_statement _expression_as_statement)
        {
            //throw new Exception("The method or operation is not implemented.");
            
        }

        public override void visit(c_scalar_type _c_scalar_type)
        {
        	switch (_c_scalar_type.scalar_name)
        	{
        		case c_scalar_type_name.tn_int : returned_scope = TypeTable.int_type; break;
        		case c_scalar_type_name.tn_double : returned_scope = TypeTable.real_type; break;
        		case c_scalar_type_name.tn_float : returned_scope = TypeTable.float_type; break;
        		case c_scalar_type_name.tn_char : returned_scope = TypeTable.char_type; break;
        		case c_scalar_type_name.tn_long :
        		case c_scalar_type_name.tn_long_int : if (_c_scalar_type.sign == c_scalar_sign.unsigned) returned_scope = TypeTable.uint64_type;
        											  else returned_scope = TypeTable.int64_type;
        											  break;
        		case c_scalar_type_name.tn_short :
        		case c_scalar_type_name.tn_short_int : if (_c_scalar_type.sign == c_scalar_sign.unsigned) returned_scope = TypeTable.uint16_type;
        											  else returned_scope = TypeTable.int16_type;
        											  break;
        		case c_scalar_type_name.tn_void : returned_scope = TypeTable.void_type; break;
        	}
        }

        public override void visit(c_module _c_module)
        {
            //Assembly _as = System.Reflection.Assembly.LoadFrom(get_assembly_path("mscorlib.dll",_c_module.file_name));
            System.Reflection.Assembly _as = System.Reflection.Assembly.LoadFrom(get_assembly_path("mscorlib.dll",_c_module.file_name));
            PascalABCCompiler.NetHelper.NetHelper.init_namespaces(_as);
            //add_standart_types_simple();
            //List<Scope> netScopes = new List<Scope>();
            //PascalABCCompiler.NetHelper.NetScope ns=new PascalABCCompiler.NetHelper.NetScope(unl,_as,tcst);
            if (_c_module.compiler_directives != null)
                foreach (PascalABCCompiler.SyntaxTree.compiler_directive dir in _c_module.compiler_directives)
                {
                    if (dir.Name.text == "reference")
                    {
                        try
                        {
                            //System.Reflection.Assembly assm = System.Reflection.Assembly.LoadFrom(get_assembly_path(dir.Directive.text,_c_module.file_name));
                            System.Reflection.Assembly assm = PascalABCCompiler.NetHelper.NetHelper.LoadAssembly(get_assembly_path(dir.Directive.text, _c_module.file_name));
                            PascalABCCompiler.NetHelper.NetHelper.init_namespaces(assm);
                        }
                        catch (Exception e)
                        {
#if DEBUG
                            File.AppendAllText("log.txt", e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
#endif
                        }
                    }
                }
            cur_scope = new InterfaceUnitScope(new SymInfo("", SymbolKind.Block,"module"),null);
            doc = new document(_c_module.file_name);
            cur_scope.loc = get_location(_c_module);
            entry_scope = cur_scope;
            
            add_system_unit();
            if (_c_module.used_units != null)
            {
                (cur_scope as InterfaceUnitScope).uses_source_range = get_location(_c_module.used_units);
                foreach (unit_or_namespace s in _c_module.used_units.units)
                {
                    try
                    {
                        string str = "";
                        for (int i = 0; i < s.name.idents.Count; i++)
                        {
                            str += s.name.idents[i].name;
                            NamespaceScope ns_scope = null;
                            if (i == 0)
                            {
                                if (PascalABCCompiler.NetHelper.NetHelper.IsNetNamespace(str))
                                {
                                    ns_scope = new NamespaceScope(str);
                                    cur_scope.AddName(str, ns_scope);
                                    if (s.name.idents.Count == 1) cur_scope.AddUsedUnit(ns_scope);
                                }
                                else
                                {
                                    string unit_name = CodeCompletionController.comp.FindSourceFileName(str);
                                    if (unit_name == null)
                                    {
                                        unit_name = Path.Combine(System.IO.Path.GetDirectoryName(_c_module.file_name), str) + System.IO.Path.GetExtension(_c_module.file_name);
                                        if (!System.IO.File.Exists(unit_name)) unit_name = null;
                                    }
                                    if (unit_name != null)
                                    {
                                        DomConverter dc = CodeCompletionController.comp_modules[unit_name] as DomConverter;
                                        if (dc == null /*|| CodeCompletionController.recomp_files[unit_name] != null*/)
                                        {
                                            dc = new CodeCompletionController().CompileAllIfNeed(unit_name);
                                        }
                                        if (dc.visitor != null)
                                        {
                                            cur_scope.AddUsedUnit(dc.visitor.entry_scope);
                                            cur_scope.AddName(str, dc.visitor.entry_scope);
                                        }
                                    }
                                }
                            }
                            if (i < s.name.idents.Count - 1) str += ".";
                            if (i == s.name.idents.Count - 1 && i > 0) cur_scope.AddUsedUnit(new NamespaceScope(str));
                        }
                        unl.AddElement(new PascalABCCompiler.TreeRealization.using_namespace(str));
                    }
                    catch (Exception e)
                    {
#if DEBUG
                        File.AppendAllText("log.txt", e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
#endif
                    }
                }
            }
           
            
			//PascalABCCompiler.TreeRealization.common_type_node ctn = new ;
			if (_c_module.defs != null)
			foreach (declaration decl in _c_module.defs.defs)
            {
				decl.visit(this);
            }
			
        }

        public override void visit(declarations_as_statement _declarations_as_statement)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(array_size _array_size)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(enumerator _enumerator)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(enumerator_list _enumerator_list)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(c_for_cycle _c_for_cycle)
        {
            //throw new Exception("The method or operation is not implemented.");
            
        }

        public override void visit(switch_stmt _switch_stmt)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(type_definition_attr_list _type_definition_attr_list)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(type_definition_attr _type_definition_attr)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void visit(lock_stmt _lock_stmt)
        {
        	if (_lock_stmt != null)
        	 _lock_stmt.stmt.visit(this);
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
        public override void visit(ident_with_templateparams node)
        {
            node.name.visit(this);
            if ((search_all || returned_scope == null) && returned_scopes.Count > 0 && returned_scopes[0] is ProcScope)
            {
                ProcScope ps = returned_scopes[0] as ProcScope;
                List<TypeScope> template_params = new List<TypeScope>();
                foreach (type_definition td in node.template_params.params_list)
                {
                    td.visit(this);
                    if (returned_scope is TypeScope)
                        template_params.Add(returned_scope as TypeScope);
                    else
                    {
                        return;
                    }
                }
                returned_scopes[0] = ps.GetInstance(template_params);
            }
            else if (returned_scope is ProcScope)
            {
            	ProcScope ps = returned_scope as ProcScope;
                List<TypeScope> template_params = new List<TypeScope>();
                foreach (type_definition td in node.template_params.params_list)
                {
                    td.visit(this);
                    if (returned_scope is TypeScope)
                        template_params.Add(returned_scope as TypeScope);
                    else
                    {
                    	returned_scope = ps;
                        return;
                    }
                }
                returned_scope = ps.GetInstance(template_params);
            }
            else if (returned_scope is TypeScope)
            {
                TypeScope ts = returned_scope as TypeScope;
                List<TypeScope> template_params = new List<TypeScope>();
                foreach (type_definition td in node.template_params.params_list)
                {
                    td.visit(this);
                    if (returned_scope is TypeScope)
                        template_params.Add(returned_scope as TypeScope);
                    else
                    {
                        returned_scope = ts;
                        return;
                    }
                }
                returned_scope = ts.GetInstance(template_params);
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
            ProcScope ps = new ProcScope(_function_lambda_definition.lambda_name, this.cur_scope);
            ps.loc = get_location(_function_lambda_definition);
            if (!disable_lambda_compilation)
                cur_scope.AddName(_function_lambda_definition.lambda_name, ps);
            if (_function_lambda_definition.ident_list != null)
                for (int i=0; i<_function_lambda_definition.ident_list.idents.Count; i++)
                {
                    ident id = _function_lambda_definition.ident_list.idents[i];
                    TypeScope param_type = new UnknownScope(new SymInfo("T", SymbolKind.Type, "T"));
                    if (awaitedProcType != null)
                    {
                        if (awaitedProcType is TypeSynonim)
                            awaitedProcType = (awaitedProcType as TypeSynonim).actType;
                        if (awaitedProcType is ProcType)
                        {
                            if ((awaitedProcType as ProcType).target.parameters.Count > 0)
                                param_type = (awaitedProcType as ProcType).target.parameters[i].sc as TypeScope;
                        }
                        else if (awaitedProcType.IsDelegate && awaitedProcType.instances != null && awaitedProcType.instances.Count > i)
                        {
                            param_type = awaitedProcType.instances[i];
                        }
                        else if (awaitedProcType.IsDelegate)
                        {
                            var invokeMeth = awaitedProcType.FindNameOnlyInType("Invoke") as ProcScope;
                            if (invokeMeth != null && invokeMeth.parameters != null && invokeMeth.parameters.Count > i)
                            {
                                param_type = invokeMeth.parameters[i].sc as TypeScope;
                            }
                        }
                    }
                    /*if (selected_methods != null)
                    {
                        foreach (ProcScope meth in selected_methods)
                        {
                            if (meth.parameters != null && meth.parameters.Count > 0 && (meth.parameters[1].sc as TypeScope).IsDelegate
                                && (meth.parameters[1].sc as TypeScope).IsGeneric)
                            {
                                
                            }
                        }
                    }*/
                    ElementScope es = new ElementScope(new SymInfo(id.name, SymbolKind.Parameter, ""), param_type, ps);
                    ps.AddName(id.name, es);
                    ps.AddParameter(es);
                    es.loc = get_location(id);
                }
            SymScope tmp = cur_scope;
            cur_scope = ps;
            if (!disable_lambda_compilation)
            {
                statement_list sl = _function_lambda_definition.proc_body as statement_list;
                if (sl != null && sl.list.Count == 1 && sl.list[0] is assign && (sl.list[0] as assign).to is ident
                    && ((sl.list[0] as assign).to as ident).name.ToLower() == "result")
                    (sl.list[0] as assign).from.visit(this);
                else
                    _function_lambda_definition.proc_body.visit(this);
            }
                
            cur_scope = tmp;
            ps.return_type = new UnknownScope(new SymInfo("",SymbolKind.Class,""));// returned_scope as TypeScope;
            returned_scope = new ProcType(ps);
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
            returned_scope = null;
            if (_name_assign_expr.expr == null)
                _name_assign_expr.expr = new ident(_name_assign_expr.name.name);
            _name_assign_expr.expr.visit(this);
            if (returned_scope != null)
            {
                ElementScope es = new ElementScope(new SymInfo(_name_assign_expr.name.name, SymbolKind.Property, ""), returned_scope, cur_scope);
                cur_scope.AddName(_name_assign_expr.name.name, es);
            }
            else
            {
                ElementScope es = new ElementScope(new SymInfo(_name_assign_expr.name.name, SymbolKind.Property, ""), new UnknownScope(new SymInfo("",SymbolKind.Type,"")), cur_scope);
                cur_scope.AddName(_name_assign_expr.name.name, es);
            }
        }
        public override void visit(name_assign_expr_list _name_assign_expr_list) // SSM 27.06.13
        {
            foreach (name_assign_expr expr in _name_assign_expr_list.name_expr)
                expr.visit(this);
        }
        public override void visit(unnamed_type_object _unnamed_type_object) // SSM 27.06.13
        {
            SymScope tmp = cur_scope;
            TypeScope ts = null;
            cur_scope = ts = new TypeScope(SymbolKind.Class, cur_scope, null);
            tmp.AddName("class", cur_scope);
            ts.loc = get_location(_unnamed_type_object);
            ts.si = new SymInfo("class", SymbolKind.Class, "");
            if (_unnamed_type_object.ne_list != null)
                _unnamed_type_object.ne_list.visit(this);
            returned_scope = cur_scope;
            cur_scope = tmp;
        }
        public override void visit(semantic_type_node stn) // SSM 
        {
        }
        public override void visit(sequence_type _sequence_type)
        {
            template_type_reference ttr = new template_type_reference();
            List<ident> names = new List<ident>();
            names.Add(new ident("System"));
            names.Add(new ident("Collections"));
            names.Add(new ident("Generic"));
            names.Add(new ident("IEnumerable"));
            ttr.name = new named_type_reference(names);
            ttr.source_context = _sequence_type.source_context;
            ttr.params_list = new template_param_list();
            ttr.params_list.params_list.Add(_sequence_type.elements_type);
            visit(ttr);
        }
        public override void visit(assign_var_tuple _assign_var_tuple)
        {
            _assign_var_tuple.expr.visit(this);
            TypeScope ts = returned_scope as TypeScope;
            if (ts != null && ts.instances != null && ts.instances.Count > 0)
            {
                for (int i = 0; i < _assign_var_tuple.idents.idents.Count; i++)
                {
                    ident id = _assign_var_tuple.idents.idents[i];
                    if (id != null)
                    {
                        SymInfo si = new SymInfo(id.name, SymbolKind.Variable, id.name);

                        ElementScope es = new ElementScope(si, ts.instances[Math.Min(i, ts.instances.Count-1)], cur_scope);
                        es.acc_mod = cur_access_mod;
                        es.si.acc_mod = cur_access_mod;
                        es.loc = get_location(id);
                        cur_scope.AddName(id.name, es);
                        es.declaringUnit = cur_scope;
                    }
                }
            }
            
        }
        public override void visit(tuple_node _tuple_node)
        {
            method_call mc = new method_call();
            mc.parameters = _tuple_node.el;
            mc.dereferencing_value = new dot_node(new ident("Tuple"), new ident("Create"));
            mc.visit(this);
        }
		
		public override void visit(slice_expr _slice_expr)
		{
			_slice_expr.v.visit(this);
		}

        public override void visit(slice_expr_question _slice_expr_question)
        {
            _slice_expr_question.v.visit(this);
        }

        public override void visit(modern_proc_type _modern_proc_type)
        {
            template_type_reference ttr = new template_type_reference();
            List<ident> names = new List<ident>();
            if (_modern_proc_type.res != null)
            {
                names.Add(new ident("System"));
                names.Add(new ident("Func"));
            }
            else
            {
                names.Add(new ident("System"));
                names.Add(new ident("Action"));
            }
            ttr.name = new named_type_reference(names);
            ttr.source_context = _modern_proc_type.source_context;
            ttr.params_list = new template_param_list();
            if (_modern_proc_type.aloneparam != null)
            {
                ttr.params_list.params_list.Add(_modern_proc_type.aloneparam);
                if (_modern_proc_type.res != null)
                    ttr.params_list.params_list.Add(_modern_proc_type.res);
            }
            else
            {
                if (_modern_proc_type.el != null)
                foreach (enumerator en in _modern_proc_type.el.enumerators)
                {
                    ttr.params_list.params_list.Add(en.name); // Здесь исправил SSM 15.1.16
                }
                if (_modern_proc_type.res != null)
                    ttr.params_list.params_list.Add(_modern_proc_type.res);
            }
            if (ttr.params_list.params_list.Count > 0)
                visit(ttr);
            else
                visit(ttr.name);
        }
    }
}

