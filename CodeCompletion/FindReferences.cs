// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.SyntaxTree;
//using ICSharpCode.SharpDevelop.Dom;
using System.Reflection;
using PascalABCCompiler;
using PascalABCCompiler.TreeConverter;
//using PascalABCCompiler.TreeRealization;
using SymbolTable;
using PascalABCCompiler.Parsers;

namespace CodeCompletion
{
    public class ReferenceFinder : AbstractVisitor
    {
        private IBaseScope founded_scope;
        private IBaseScope entry_scope;
        private IBaseScope cur_scope;
        private List<Position> pos_list;
        private IBaseScope ret_tn;
        private compilation_unit cu;
        private string FileName;
        public bool for_refactoring = false;

        public ReferenceFinder(IBaseScope founded_scope, IBaseScope entry_scope, compilation_unit cu, string FileName, bool for_refactoring)
        {
            this.founded_scope = founded_scope;
            this.entry_scope = entry_scope;
            this.cur_scope = entry_scope;
            this.pos_list = new List<Position>();
            this.cu = cu;
            this.FileName = FileName;
            this.for_refactoring = for_refactoring;
        }

        public Position[] FindPositions()
        {
            try
            {
                cu.visit(this);
            }
            catch (Exception e)
            {

            }
            return pos_list.ToArray();
        }

        public override void visit(default_operator _default_operator)
        {
            _default_operator.type_name.visit(this);
        }

        public override void visit(syntax_tree_node _syntax_tree_node)
        {
            throw new NotImplementedException();
        }

        public override void visit(statement_list _statement_list)
        {
            if (_statement_list == null || _statement_list.source_context == null) return;
            IBaseScope tmp = cur_scope;
            cur_scope = entry_scope.FindScopeByLocation(_statement_list.source_context.begin_position.line_num, _statement_list.source_context.begin_position.column_num);
            if (cur_scope == null)
                cur_scope = tmp;
            foreach (statement stmt in _statement_list.subnodes)
                stmt.visit(this);
            cur_scope = tmp;
        }

        public override void visit(expression _expression)
        {
            throw new NotImplementedException();
        }

        public override void visit(assign _assign)
        {
            _assign.to.visit(this);
            _assign.from.visit(this);
        }

        public override void visit(bin_expr _bin_expr)
        {
            //throw new NotImplementedException();
            if (_bin_expr.left != null)
                _bin_expr.left.visit(this);
            if (_bin_expr.right != null)
                _bin_expr.right.visit(this);
        }

        public override void visit(un_expr _un_expr)
        {
            //throw new NotImplementedException();
            if (_un_expr.subnode != null)
                _un_expr.subnode.visit(this);
        }
        public override void visit(template_type_name name)
        {
            throw new NotImplementedException();
        }
        public override void visit(const_node _const_node)
        {
            throw new NotImplementedException();
        }

        public override void visit(bool_const _bool_const)
        {
            //throw new NotImplementedException();
        }

        public override void visit(int32_const _int32_const)
        {
            //throw new NotImplementedException();
        }

        public override void visit(double_const _double_const)
        {
            //throw new NotImplementedException();
        }

        public override void visit(statement _statement)
        {
            throw new NotImplementedException();
        }

        public override void visit(subprogram_body _subprogram_body)
        {
            if (_subprogram_body.subprogram_defs != null)
                _subprogram_body.subprogram_defs.visit(this);
            if (_subprogram_body.subprogram_code != null)
                _subprogram_body.subprogram_code.visit(this);
        }

        public override void visit(ident _ident)
        {
            ret_tn = cur_scope.FindNameInAnyOrder(_ident.name);
            if (ret_tn == null && cur_scope.TopScope != null)
            {
                try
                {
                    ret_tn = cur_scope.TopScope.FindNameInAnyOrder(_ident.name);
                }
                catch (Exception e)
                {

                }
            }
            if (ret_tn != null && ret_tn.IsEqual(founded_scope))
                pos_list.Add(get_position(_ident));
        }

        public override void visit(addressed_value _addressed_value)
        {
            throw new NotImplementedException();
        }

        public override void visit(type_definition _type_definition)
        {
            throw new NotImplementedException();
        }

        private Position get_position(syntax_tree_node stn)
        {
            Position pos = new Position();
            if (stn != null && stn.source_context != null)
            {
                pos.line = stn.source_context.begin_position.line_num;
                pos.column = stn.source_context.begin_position.column_num;
                pos.end_line = stn.source_context.end_position.line_num;
                pos.end_column = stn.source_context.end_position.column_num;
                pos.file_name = FileName;
            }
            return pos;
        }

        public override void visit(named_type_reference _named_type_reference)
        {
            ret_tn = cur_scope;
            for (int i = 0; i < _named_type_reference.names.Count; i++)
            {
                if (i > 0)
                    ret_tn = ret_tn.FindNameOnlyInType(_named_type_reference.names[i].name);
                else ret_tn = ret_tn.FindNameInAnyOrder(_named_type_reference.names[i].name);
                if (ret_tn == null) break;
                else if (founded_scope.IsEqual(ret_tn))
                    pos_list.Add(get_position(_named_type_reference));
            }
        }

        public override void visit(variable_definitions _variable_definitions)
        {
            foreach (var_def_statement vs in _variable_definitions.var_definitions)
                vs.visit(this);
        }

        public override void visit(ident_list _ident_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(var_def_statement _var_def_statement)
        {
            if (for_refactoring && _var_def_statement.vars != null)
                foreach (ident s in _var_def_statement.vars.idents)
                {
                    IBaseScope ss = entry_scope.FindScopeByLocation(s.source_context.begin_position.line_num, s.source_context.begin_position.column_num);
                    if (ss != null && ss.IsEqual(founded_scope))
                        pos_list.Add(get_position(s));
                }

            if (_var_def_statement.vars_type != null)
                _var_def_statement.vars_type.visit(this);
            if (_var_def_statement.inital_value != null)
                _var_def_statement.inital_value.visit(this);
        }

        public override void visit(declaration _declaration)
        {
            throw new NotImplementedException();
        }

        public override void visit(declarations _declarations)
        {
            foreach (declaration decl in _declarations.defs)
                decl.visit(this);
        }

        public override void visit(program_tree _program_tree)
        {
            throw new NotImplementedException();
        }

        public override void visit(program_name _program_name)
        {
            if (_program_name.prog_name != null)
                _program_name.prog_name.visit(this);
        }

        public override void visit(string_const _string_const)
        {
            //throw new NotImplementedException();
        }

        public override void visit(expression_list _expression_list)
        {
            if (_expression_list != null)
                foreach (expression e in _expression_list.expressions)
                    e.visit(this);
        }

        public override void visit(dereference _dereference)
        {
            throw new NotImplementedException();
        }

        public override void visit(roof_dereference _roof_dereference)
        {
            _roof_dereference.dereferencing_value.visit(this);
            if (ret_tn != null && ret_tn is IElementScope)
            {
                IPointerScope ts = (ret_tn as IElementScope).Type as IPointerScope;
                if (ts != null)
                    ret_tn = ts.ElementType.MakeElementScope();//new ElementScope(ts.ref_type);
                else
                    ret_tn = null;
            }
            else ret_tn = null;
        }

        public override void visit(indexer _indexer)
        {
            _indexer.dereferencing_value.visit(this);
            if (ret_tn != null)
            {
                if (ret_tn != null)
                    if (ret_tn is IElementScope && (ret_tn as IElementScope).Type is IProcScope && ((ret_tn as IElementScope).Type as IProcScope).ReturnType != null)
                        ret_tn = ((ret_tn as IElementScope).Type as IProcScope).ReturnType.GetElementType();
                    else
                        ret_tn = ret_tn.GetElementType();
                IBaseScope tmp = ret_tn;
                _indexer.indexes.visit(this);
                ret_tn = tmp;
            }
        }

        public override void visit(for_node _for_node)
        {
            if (_for_node.loop_variable != null)
            {
                ret_tn = cur_scope.FindNameInAnyOrder(_for_node.loop_variable.name);
                if (ret_tn != null && ret_tn.IsEqual(founded_scope))
                    pos_list.Add(get_position(_for_node.loop_variable));
            }
            if (_for_node.type_name != null)
                _for_node.type_name.visit(this);
            if (_for_node.initial_value != null)
                _for_node.initial_value.visit(this);
            if (_for_node.finish_value != null)
                _for_node.finish_value.visit(this);
            if (_for_node.increment_value != null)
                _for_node.increment_value.visit(this);
            if (_for_node.statements != null)
                _for_node.statements.visit(this);
        }

        public override void visit(repeat_node _repeat_node)
        {
            if (_repeat_node.statements != null)
                _repeat_node.statements.visit(this);
            if (_repeat_node.expr != null)
                _repeat_node.expr.visit(this);
        }

        public override void visit(while_node _while_node)
        {
            if (_while_node.expr != null)
                _while_node.expr.visit(this);
            if (_while_node.statements != null)
                _while_node.statements.visit(this);
        }

        public override void visit(if_node _if_node)
        {
            //throw new NotImplementedException();
            if (_if_node.condition != null)
                _if_node.condition.visit(this);
            if (_if_node.then_body != null)
                _if_node.then_body.visit(this);
            if (_if_node.else_body != null)
                _if_node.else_body.visit(this);
        }

        public override void visit(ref_type _ref_type)
        {
            //throw new NotImplementedException();
            if (_ref_type.pointed_to != null)
                _ref_type.pointed_to.visit(this);
        }

        public override void visit(diapason _diapason)
        {
            //throw new NotImplementedException();
            if (_diapason.left != null)
                _diapason.left.visit(this);
            if (_diapason.right != null)
                _diapason.right.visit(this);
        }

        public override void visit(indexers_types _indexers_types)
        {
            foreach (type_definition td in _indexers_types.indexers)
                if (td != null)
                    td.visit(this);
        }

        public override void visit(array_type _array_type)
        {
            if (_array_type.indexers != null)
                _array_type.indexers.visit(this);
            if (_array_type.elements_type != null)
                _array_type.elements_type.visit(this);
        }

        public override void visit(label_definitions _label_definitions)
        {
            //throw new NotImplementedException();
        }

        public override void visit(procedure_attribute _procedure_attribute)
        {
            throw new NotImplementedException();
        }

        public override void visit(typed_parameters _typed_parametres)
        {
            if (for_refactoring)
                foreach (ident s in _typed_parametres.idents.idents)
                {
                    IBaseScope ss = entry_scope.FindScopeByLocation(s.source_context.begin_position.line_num, s.source_context.begin_position.column_num);
                    if (ss != null && ss.IsEqual(founded_scope))
                        pos_list.Add(get_position(s));
                }
            if (_typed_parametres.vars_type != null)
                _typed_parametres.vars_type.visit(this);
        }

        public override void visit(formal_parameters _formal_parametres)
        {
            foreach (typed_parameters tp in _formal_parametres.params_list)
                tp.visit(this);
        }

        public override void visit(procedure_attributes_list _procedure_attributes_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(procedure_header _procedure_header)
        {
            if (for_refactoring)
            {
                IBaseScope sc = null;
                if (_procedure_header.name != null)
                {
                    if (_procedure_header.name.meth_name != null)
                    {
                        if (with_body)
                        {
                            if (cur_scope != null && cur_scope.IsEqual(founded_scope))
                                pos_list.Add(get_position(_procedure_header.name.meth_name));
                        }
                        else
                        {
                            sc = cur_scope.FindNameOnlyInType(_procedure_header.name.meth_name.name);
                            if (sc != null && sc.IsEqual(founded_scope))
                                pos_list.Add(get_position(_procedure_header.name.meth_name));
                        }
                    }
                    if (_procedure_header.name.class_name != null)
                    {
                        sc = cur_scope.FindNameInAnyOrder(_procedure_header.name.class_name.name);
                        if (sc != null && sc.IsEqual(founded_scope))
                            pos_list.Add(get_position(_procedure_header.name.class_name));
                    }
                }
            }
            if (_procedure_header.parameters != null)
                _procedure_header.parameters.visit(this);
            if (_procedure_header.where_defs != null)
                _procedure_header.where_defs.visit(this);
        }

        public override void visit(function_header _function_header)
        {
            if (for_refactoring)
            {
                IBaseScope sc = null;
                if (_function_header.name != null)
                {
                    if (_function_header.name.meth_name != null)
                    {
                        if (with_body)
                        {
                            if (cur_scope != null && cur_scope.IsEqual(founded_scope))
                                pos_list.Add(get_position(_function_header.name.meth_name));
                        }
                        else
                        {
                            sc = cur_scope.FindNameOnlyInType(_function_header.name.meth_name.name);
                            if (sc != null && sc.IsEqual(founded_scope))
                                pos_list.Add(get_position(_function_header.name.meth_name));
                        }
                    }
                    if (_function_header.name.class_name != null)
                    {
                        sc = cur_scope.FindNameInAnyOrder(_function_header.name.class_name.name);
                        if (sc != null && sc.IsEqual(founded_scope))
                            pos_list.Add(get_position(_function_header.name.class_name));
                    }
                }
            }
            if (_function_header.parameters != null)
                _function_header.parameters.visit(this);
            if (_function_header.return_type != null)
                _function_header.return_type.visit(this);
            if (_function_header.where_defs != null)
                _function_header.where_defs.visit(this);
        }

        private bool with_body = false;
        public override void visit(procedure_definition _procedure_definition)
        {
            //throw new NotImplementedException();
            IBaseScope tmp = cur_scope;
            cur_scope = entry_scope.FindScopeByLocation(_procedure_definition.source_context.begin_position.line_num, _procedure_definition.source_context.begin_position.column_num);
            if (cur_scope == null)
                cur_scope = tmp;

            with_body = true;
            _procedure_definition.proc_header.visit(this);
            with_body = false;
            if (_procedure_definition.proc_body != null)
                _procedure_definition.proc_body.visit(this);
            cur_scope = tmp;
        }

        public override void visit(type_declaration _type_declaration)
        {
            //SymScope ss = entry_scope.FindScopeByLocation(_type_declaration.source_context.begin_position.line_num,_type_declaration.source_context.begin_position.column_num);
            IBaseScope ss = entry_scope.FindNameInAnyOrder(_type_declaration.type_name.name);
            if (for_refactoring && ss != null && ss.IsEqual(founded_scope) && string.Compare(ss.SymbolInfo.name, _type_declaration.type_name.name, true) == 0)
            {
                pos_list.Add(get_position(_type_declaration.type_name));
            }
            if (_type_declaration.type_def != null)
                _type_declaration.type_def.visit(this);
        }

        public override void visit(type_declarations _type_declarations)
        {
            foreach (type_declaration td in _type_declarations.types_decl)
                td.visit(this);
        }

        public override void visit(simple_const_definition _simple_const_definition)
        {
            if (for_refactoring)
            {
                IBaseScope ss = entry_scope.FindScopeByLocation(_simple_const_definition.source_context.begin_position.line_num, _simple_const_definition.source_context.begin_position.column_num);
                if (ss != null && ss.IsEqual(founded_scope))
                    pos_list.Add(get_position(_simple_const_definition.const_name));
            }
            if (_simple_const_definition.const_value != null)
                _simple_const_definition.const_value.visit(this);
        }

        public override void visit(typed_const_definition _typed_const_definition)
        {
            if (for_refactoring)
            {
                IBaseScope ss = entry_scope.FindScopeByLocation(_typed_const_definition.source_context.begin_position.line_num, _typed_const_definition.source_context.begin_position.column_num);
                if (ss != null && ss.IsEqual(founded_scope))
                    pos_list.Add(get_position(_typed_const_definition.const_name));
            }
            _typed_const_definition.const_type.visit(this);
            if (_typed_const_definition.const_value != null)
                _typed_const_definition.const_value.visit(this);
        }

        public override void visit(const_definition _const_definition)
        {
            throw new NotImplementedException();
        }

        public override void visit(consts_definitions_list _consts_definitions_list)
        {
            foreach (const_definition cnst in _consts_definitions_list.const_defs)
                cnst.visit(this);
        }

        public override void visit(unit_name _unit_name)
        {
            _unit_name.idunit_name.visit(this);
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
            try
            {
                foreach (unit_or_namespace unit in _uses_list.units)
                {
                    foreach (ident id in unit.name.idents)
                    {
                        id.visit(this);
                    }
                }
            }
            catch
            {
            }
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
            if (_unit_module.unit_name != null)
                _unit_module.unit_name.visit(this);
            if (_unit_module.interface_part != null)
                _unit_module.interface_part.visit(this);
            if (_unit_module.implementation_part != null)
                _unit_module.implementation_part.visit(this);
            if (_unit_module.initialization_part != null)
                _unit_module.initialization_part.visit(this);
            if (_unit_module.finalization_part != null)
                _unit_module.finalization_part.visit(this);
        }

        public override void visit(program_module _program_module)
        {
            if (_program_module.program_name != null)
                _program_module.program_name.visit(this);
            if (_program_module.used_units != null)
                _program_module.used_units.visit(this);
            if (_program_module.program_block.defs != null)
                _program_module.program_block.defs.visit(this);
            if (_program_module.program_block.program_code != null)
                _program_module.program_block.program_code.visit(this);
        }

        public override void visit(hex_constant _hex_constant)
        {
            //throw new NotImplementedException();
        }

        public override void visit(get_address _get_address)
        {
            //throw new NotImplementedException();
            if (_get_address.address_of != null)
                _get_address.address_of.visit(this);
        }

        public override void visit(case_variant _case_variant)
        {
            if (_case_variant.conditions != null)
                _case_variant.conditions.visit(this);
            if (_case_variant.exec_if_true != null)
                _case_variant.exec_if_true.visit(this);
        }

        public override void visit(case_node _case_node)
        {
            if (_case_node.param != null)
                _case_node.param.visit(this);
            if (_case_node.conditions != null)
                _case_node.conditions.visit(this);
            if (_case_node.else_statement != null)
                _case_node.else_statement.visit(this);
        }

        public override void visit(method_name _method_name)
        {
            throw new NotImplementedException();
        }

        public override void visit(dot_node _dot_node)
        {
            _dot_node.left.visit(this);
            if (ret_tn != null)
            {
                IBaseScope left_scope = ret_tn;
                if (_dot_node.right is ident)
                {
                    ret_tn = ret_tn.FindNameOnlyInType((_dot_node.right as ident).name);
                }
            }
            if (ret_tn != null && ret_tn.IsEqual(founded_scope))
                pos_list.Add(get_position(_dot_node.right));
        }

        public override void visit(empty_statement _empty_statement)
        {
            //throw new NotImplementedException();
        }

        public override void visit(goto_statement _goto_statement)
        {

        }

        public override void visit(labeled_statement _labeled_statement)
        {
            //throw new NotImplementedException();
            if (_labeled_statement.to_statement != null)
                _labeled_statement.to_statement.visit(this);
        }

        public override void visit(with_statement _with_statement)
        {
            IBaseScope tmp = cur_scope;
            cur_scope = entry_scope.FindScopeByLocation(_with_statement.source_context.begin_position.line_num, _with_statement.source_context.begin_position.column_num);
            if (cur_scope == null)
                cur_scope = tmp;
            if (_with_statement.do_with != null)
                _with_statement.do_with.visit(this);
            if (_with_statement.what_do != null)
                _with_statement.what_do.visit(this);
            cur_scope = tmp;
        }

        public override void visit(method_call _method_call)
        {
            if (_method_call.dereferencing_value != null)
                _method_call.dereferencing_value.visit(this);
            //if (ret_tn != null && ret_tn.IsEqual(founded_scope))
            //	pos_list.Add(get_position(_method_call));
            if (_method_call.parameters != null)
                _method_call.parameters.visit(this);
        }

        public override void visit(pascal_set_constant _pascal_set_constant)
        {
            //throw new NotImplementedException();
            if (_pascal_set_constant.values != null)
                _pascal_set_constant.values.visit(this);
        }

        public override void visit(array_const _array_const)
        {
            if (_array_const.elements != null)
                _array_const.elements.visit(this);
        }

        public override void visit(write_accessor_name _write_accessor_name)
        {
            IBaseScope sc = cur_scope.FindNameOnlyInType(_write_accessor_name.accessor_name.name);
            if (sc != null && sc.IsEqual(founded_scope))
                pos_list.Add(get_position(_write_accessor_name.accessor_name));
        }

        public override void visit(read_accessor_name _read_accessor_name)
        {
            IBaseScope sc = cur_scope.FindNameOnlyInType(_read_accessor_name.accessor_name.name);
            if (sc != null && sc.IsEqual(founded_scope))
                pos_list.Add(get_position(_read_accessor_name.accessor_name));
        }

        public override void visit(property_accessors _property_accessors)
        {
        }

        public override void visit(simple_property _simple_property)
        {
            if (for_refactoring)
            {
                IBaseScope ss = entry_scope.FindScopeByLocation(_simple_property.source_context.begin_position.line_num, _simple_property.source_context.begin_position.column_num);
                if (ss != null && ss.IsEqual(founded_scope))
                    pos_list.Add(get_position(_simple_property.property_name));
            }
            if (_simple_property.parameter_list != null)
                _simple_property.parameter_list.visit(this);
            if (_simple_property.property_type != null)
                _simple_property.property_type.visit(this);
            if (_simple_property.accessors != null)
            {
                if (_simple_property.accessors.read_accessor != null)
                    _simple_property.accessors.read_accessor.visit(this);
                if (_simple_property.accessors.write_accessor != null)
                    _simple_property.accessors.write_accessor.visit(this);
            }
        }

        public override void visit(index_property _index_property)
        {
            if (for_refactoring)
            {
                IBaseScope ss = entry_scope.FindScopeByLocation(_index_property.source_context.begin_position.line_num, _index_property.source_context.begin_position.column_num);
                if (ss != null && ss.IsEqual(founded_scope))
                    pos_list.Add(get_position(_index_property.property_name));
            }
            if (_index_property.property_type != null)
                _index_property.property_type.visit(this);
            if (_index_property.parameter_list != null)
                _index_property.parameter_list.visit(this);
            if (_index_property.index_expression != null)
                _index_property.index_expression.visit(this);
        }

        public override void visit(class_members _class_members)
        {
            foreach (declaration decl in _class_members.members)
                decl.visit(this);
        }

        public override void visit(access_modifer_node _access_modifer_node)
        {
            //throw new NotImplementedException();
        }

        public override void visit(class_body _class_body)
        {
            if (_class_body.class_def_blocks != null)
                foreach (class_members cm in _class_body.class_def_blocks)
                    cm.visit(this);
        }

        public override void visit(class_definition _class_definition)
        {
            IBaseScope tmp = cur_scope;
            cur_scope = entry_scope.FindScopeByLocation(_class_definition.source_context.begin_position.line_num, _class_definition.source_context.begin_position.column_num);
            if (_class_definition.class_parents != null)
                _class_definition.class_parents.visit(this);
            if (_class_definition.body != null)
                _class_definition.body.visit(this);
            cur_scope = tmp;
        }

        public override void visit(default_indexer_property_node _default_indexer_property_node)
        {
            //throw new NotImplementedException();

        }

        public override void visit(known_type_definition _known_type_definition)
        {
            //throw new NotImplementedException();
        }

        public override void visit(set_type_definition _set_type_definition)
        {
            //throw new NotImplementedException();
            _set_type_definition.of_type.visit(this);
        }

        public override void visit(record_const_definition _record_const_definition)
        {
            _record_const_definition.val.visit(this);
        }

        public override void visit(record_const _record_const)
        {
            foreach (record_const_definition rcd in _record_const.rec_consts)
                rcd.visit(this);
            //throw new NotImplementedException();
        }

        public override void visit(record_type _record_type)
        {
            //throw new NotImplementedException();
        }

        public override void visit(enum_type_definition _enum_type_definition)
        {
            //throw new NotImplementedException();
        }

        public override void visit(char_const _char_const)
        {
            //throw new NotImplementedException();
        }

        public override void visit(raise_statement _raise_statement)
        {
            throw new NotImplementedException();
        }

        public override void visit(sharp_char_const _sharp_char_const)
        {
            //throw new NotImplementedException();
        }

        public override void visit(literal_const_line _literal_const_line)
        {
            //throw new NotImplementedException();
        }

        public override void visit(string_num_definition _string_num_definition)
        {
            //throw new NotImplementedException();
        }

        public override void visit(variant _variant)
        {

        }

        public override void visit(variant_list _variant_list)
        {

        }

        public override void visit(variant_type _variant_type)
        {

        }

        public override void visit(variant_types _variant_types)
        {

        }

        public override void visit(variant_record_type _variant_record_type)
        {

        }

        public override void visit(procedure_call _procedure_call)
        {
            if (_procedure_call.func_name != null)
                _procedure_call.func_name.visit(this);
        }

        public override void visit(class_predefinition _class_predefinition)
        {
            //throw new NotImplementedException();
        }

        public override void visit(nil_const _nil_const)
        {
            //throw new NotImplementedException();
        }

        public override void visit(file_type_definition _file_type_definition)
        {
            if (_file_type_definition.elem_type != null)
                _file_type_definition.elem_type.visit(this);
        }

        public override void visit(constructor _constructor)
        {
            if (for_refactoring)
            {
                IBaseScope sc = null;
                if (_constructor.name != null)
                {
                    if (_constructor.name.meth_name != null)
                    {
                        if (with_body)
                        {
                            if (cur_scope != null && cur_scope.IsEqual(founded_scope))
                                pos_list.Add(get_position(_constructor.name.meth_name));
                        }
                        else
                        {
                            sc = cur_scope.FindNameOnlyInType(_constructor.name.meth_name.name);
                            if (sc != null && sc.IsEqual(founded_scope))
                                pos_list.Add(get_position(_constructor.name.meth_name));
                        }
                    }
                    if (_constructor.name.class_name != null)
                    {
                        sc = cur_scope.FindNameInAnyOrder(_constructor.name.class_name.name);
                        if (sc != null && sc.IsEqual(founded_scope))
                            pos_list.Add(get_position(_constructor.name.class_name));
                    }
                }
            }
            if (_constructor.parameters != null)
                _constructor.parameters.visit(this);
        }

        public override void visit(destructor _destructor)
        {
            if (for_refactoring)
            {
                IBaseScope sc = null;
                if (_destructor.name != null)
                {
                    if (_destructor.name.meth_name != null)
                    {
                        if (with_body)
                        {
                            if (cur_scope != null && cur_scope.IsEqual(founded_scope))
                                pos_list.Add(get_position(_destructor.name.meth_name));
                        }
                        else
                        {
                            sc = cur_scope.FindNameOnlyInType(_destructor.name.meth_name.name);
                            if (sc != null && sc.IsEqual(founded_scope))
                                pos_list.Add(get_position(_destructor.name.meth_name));
                        }
                    }
                    if (_destructor.name.class_name != null)
                    {
                        sc = cur_scope.FindNameInAnyOrder(_destructor.name.class_name.name);
                        if (sc != null && sc.IsEqual(founded_scope))
                            pos_list.Add(get_position(_destructor.name.class_name));
                    }
                }
            }
            if (_destructor.parameters != null)
                _destructor.parameters.visit(this);
        }

        public override void visit(inherited_method_call _inherited_method_call)
        {
            _inherited_method_call.method_name.visit(this);
            if (_inherited_method_call.exprs != null)
                _inherited_method_call.exprs.visit(this);
        }

        public override void visit(typecast_node _typecast_node)
        {
            if (_typecast_node.expr != null)
                _typecast_node.expr.visit(this);
            if (_typecast_node.type_def != null)
                _typecast_node.type_def.visit(this);
        }

        public override void visit(interface_node _interface_node)
        {
            if (_interface_node.uses_modules != null)
                _interface_node.uses_modules.visit(this);
            if (_interface_node.interface_definitions != null)
                _interface_node.interface_definitions.visit(this);
        }

        public override void visit(implementation_node _implementation_node)
        {
            if (_implementation_node.uses_modules != null)
                _implementation_node.uses_modules.visit(this);
            if (_implementation_node.implementation_definitions != null)
                _implementation_node.implementation_definitions.visit(this);
        }

        public override void visit(diap_expr _diap_expr)
        {
            if (_diap_expr.left != null)
                _diap_expr.left.visit(this);
            if (_diap_expr.right != null)
                _diap_expr.right.visit(this);
        }

        public override void visit(block _block)
        {
            if (_block.defs != null)
                _block.defs.visit(this);
            if (_block.program_code != null)
                _block.program_code.visit(this);
        }

        public override void visit(proc_block _proc_block)
        {
            throw new NotImplementedException();
        }

        public override void visit(array_of_named_type_definition _array_of_named_type_definition)
        {
            if (_array_of_named_type_definition.type_name != null)
                _array_of_named_type_definition.type_name.visit(this);
        }

        public override void visit(array_of_const_type_definition _array_of_const_type_definition)
        {
            //throw new NotImplementedException();
        }

        public override void visit(literal _literal)
        {
            //throw new NotImplementedException();
        }

        public override void visit(case_variants _case_variants)
        {
            //throw new NotImplementedException();
            foreach (case_variant cv in _case_variants.variants)
                cv.visit(this);
        }

        public override void visit(diapason_expr _diapason_expr)
        {
            if (_diapason_expr.left != null)
                _diapason_expr.left.visit(this);
            if (_diapason_expr.right != null)
                _diapason_expr.right.visit(this);
        }

        public override void visit(var_def_list_for_record _var_def_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(record_type_parts _record_type_parts)
        {

        }

        public override void visit(property_array_default _property_array_default)
        {

        }

        public override void visit(property_interface _property_interface)
        {

        }

        public override void visit(property_parameter _property_parameter)
        {
            if (_property_parameter.type != null)
                _property_parameter.type.visit(this);
        }

        public override void visit(property_parameter_list _property_parameter_list)
        {
            foreach (property_parameter prm in _property_parameter_list.parameters)
                prm.visit(this);
        }

        public override void visit(inherited_ident _inherited_ident)
        {
            ret_tn = cur_scope.FindNameInAnyOrder(_inherited_ident.name);
            if (ret_tn != null && ret_tn.IsEqual(founded_scope))
                pos_list.Add(get_position(_inherited_ident));
        }

        public override void visit(format_expr _format_expr)
        {
            _format_expr.expr.visit(this);
            if (_format_expr.format1 != null)
                _format_expr.format1.visit(this);
            if (_format_expr.format2 != null)
                _format_expr.format2.visit(this);
        }

        public override void visit(initfinal_part _initfinal_part)
        {
            //throw new NotImplementedException();
        }

        public override void visit(token_info _token_info)
        {
            //throw new NotImplementedException()
        }

        public override void visit(raise_stmt _raise_stmt)
        {
            if (_raise_stmt.address != null)
                _raise_stmt.address.visit(this);
            if (_raise_stmt.expr != null)
                _raise_stmt.expr.visit(this);
        }

        public override void visit(op_type_node _op_type_node)
        {
            //throw new NotImplementedException();
        }

        public override void visit(file_type _file_type)
        {
            if (_file_type.file_of_type != null)
                _file_type.file_of_type.visit(this);
        }

        public override void visit(known_type_ident _known_type_ident)
        {
            //throw new NotImplementedException();

        }

        public override void visit(exception_handler _exception_handler)
        {
            if (_exception_handler.type_name != null)
                _exception_handler.type_name.visit(this);
            if (_exception_handler.statements != null)
                _exception_handler.statements.visit(this);
        }

        public override void visit(exception_ident _exception_ident)
        {
            //throw new NotImplementedException();
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
            if (_try_handler_finally.stmt_list != null)
                _try_handler_finally.stmt_list.visit(this);
        }

        public override void visit(try_handler_except _try_handler_except)
        {
            if (_try_handler_except.except_block != null)
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
            //throw new NotImplementedException();
        }

        public override void visit(external_directive _external_directive)
        {
            //throw new NotImplementedException();
        }

        public override void visit(using_list _using_list)
        {
            throw new NotImplementedException();
        }

        public override void visit(jump_stmt _jump_stmt)
        {
            throw new NotImplementedException();
        }

        public override void visit(loop_stmt _loop_stmt)
        {
            throw new NotImplementedException();
        }

        public override void visit(foreach_stmt _foreach_stmt)
        {
            ret_tn = cur_scope.FindNameInAnyOrder(_foreach_stmt.identifier.name);
            if (ret_tn != null && ret_tn.IsEqual(founded_scope))
                pos_list.Add(get_position(_foreach_stmt.identifier));
            if (_foreach_stmt.type_name != null)
                _foreach_stmt.type_name.visit(this);
            if (_foreach_stmt.in_what != null)
                _foreach_stmt.in_what.visit(this);
            if (_foreach_stmt.stmt != null)
                _foreach_stmt.stmt.visit(this);
        }

        public override void visit(addressed_value_funcname _addressed_value_funcname)
        {
            //throw new NotImplementedException();
        }

        public override void visit(named_type_reference_list _named_type_reference_list)
        {
            foreach (named_type_reference ntr in _named_type_reference_list.types)
                ntr.visit(this);
        }

        public override void visit(template_param_list _template_param_list)
        {
            //throw new NotImplementedException();
            foreach (type_definition td in _template_param_list.params_list)
                td.visit(this);
        }

        public override void visit(template_type_reference _template_type_reference)
        {
            //throw new NotImplementedException();
            if (_template_type_reference.name != null)
                _template_type_reference.name.visit(this);
            if (_template_type_reference.params_list != null)
                _template_type_reference.params_list.visit(this);
        }

        public override void visit(int64_const _int64_const)
        {
            //throw new NotImplementedException();
        }

        public override void visit(uint64_const _uint64_const)
        {
            //throw new NotImplementedException();
        }

        public override void visit(new_expr _new_expr)
        {
            if (_new_expr.type != null)
                _new_expr.type.visit(this);
            if (ret_tn != null && ret_tn is ITypeScope)
            {
                IProcScope ps = (ret_tn as ITypeScope).GetConstructor();
                while (ps != null)
                {
                    if (ps.IsEqual(founded_scope))
                    {
                        pos_list.Add(get_position(_new_expr.type));
                        break;
                    }
                    else ps = ps.NextFunction;
                }
            }
            if (_new_expr.params_list != null)
                _new_expr.params_list.visit(this);
        }

        public override void visit(where_type_specificator_list _type_definition_list)
        {
            //throw new NotImplementedException();
        }

        public override void visit(where_definition _where_definition)
        {
            foreach (ident id in _where_definition.names.list)
                id.visit(this);
            foreach (type_definition td in _where_definition.types.defs)
                td.visit(this);
        }

        public override void visit(where_definition_list _where_definition_list)
        {
            foreach (where_definition wd in _where_definition_list.defs)
                wd.visit(this);
        }

        public override void visit(sizeof_operator _sizeof_operator)
        {
            //throw new NotImplementedException();
            if (_sizeof_operator.type_def != null)
                _sizeof_operator.type_def.visit(this);
            if (_sizeof_operator.expr != null)
                _sizeof_operator.expr.visit(this);
        }

        public override void visit(typeof_operator _typeof_operator)
        {
            if (_typeof_operator.type_name != null)
                _typeof_operator.type_name.visit(this);
        }

        public override void visit(compiler_directive _compiler_directive)
        {
            //throw new NotImplementedException();
        }

        public override void visit(operator_name_ident _operator_name_ident)
        {
            //throw new NotImplementedException();
        }

        public override void visit(var_statement _var_statement)
        {
            if (_var_statement.var_def != null)
                _var_statement.var_def.visit(this);
        }

        public override void visit(question_colon_expression _question_colon_expression)
        {
            if (_question_colon_expression.condition != null)
                _question_colon_expression.condition.visit(this);
            if (_question_colon_expression.ret_if_true != null)
                _question_colon_expression.ret_if_true.visit(this);
            if (_question_colon_expression.ret_if_false != null)
                _question_colon_expression.ret_if_false.visit(this);
        }

        public override void visit(expression_as_statement _expression_as_statement)
        {
            if (_expression_as_statement.expr != null)
                _expression_as_statement.expr.visit(this);
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
            
        }

        public override void visit(enumerator_list _enumerator_list)
        {
            //throw new NotImplementedException();
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
            if (_lock_stmt.lock_object != null)
                _lock_stmt.lock_object.visit(this);
            if (_lock_stmt.stmt != null)
                _lock_stmt.stmt.visit(this);
        }

        public override void visit(compiler_directive_list _compiler_directive_list)
        {

        }

        public override void visit(compiler_directive_if _compiler_directive_if)
        {

        }

        public override void visit(documentation_comment_list _documentation_comment_list)
        {

        }

        public override void visit(documentation_comment_tag _documentation_comment_tag)
        {

        }

        public override void visit(documentation_comment_tag_param _documentation_comment_tag_param)
        {

        }

        public override void visit(documentation_comment_section _documentation_comment_section)
        {

        }
        public override void visit(token_taginfo node)
        {

        }

        public override void visit(declaration_specificator node)
        {

        }
        public override void visit(ident_with_templateparams node)
        {
            node.name.visit(this);
            foreach (type_definition td in node.template_params.params_list)
            {
                td.visit(this);
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
            IBaseScope tmp = cur_scope;
            cur_scope = entry_scope.FindScopeByLocation(_function_lambda_definition.source_context.begin_position.line_num, _function_lambda_definition.source_context.begin_position.column_num);
            if (cur_scope == null)
                cur_scope = tmp;
            foreach (ident id in _function_lambda_definition.ident_list.list)
                id.visit(this);
            _function_lambda_definition.proc_body.visit(this);
            cur_scope = tmp;
        }

        public override void visit(function_lambda_call _function_lambda_call)
        {
            //throw new NotImplementedException();
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
            IBaseScope ss = entry_scope.FindScopeByLocation(_name_assign_expr.name.source_context.begin_position.line_num, _name_assign_expr.name.source_context.begin_position.column_num);
            if (ss != null && ss.IsEqual(founded_scope))
                pos_list.Add(get_position(_name_assign_expr.name));
            _name_assign_expr.expr.visit(this);
        }
        public override void visit(name_assign_expr_list _name_assign_expr_list) // SSM 27.06.13
        {
            foreach (name_assign_expr expr in _name_assign_expr_list.name_expr)
                expr.visit(this);
        }
        public override void visit(unnamed_type_object _unnamed_type_object) // SSM 27.06.13
        {
            IBaseScope tmp = cur_scope;
            cur_scope = entry_scope.FindScopeByLocation(_unnamed_type_object.source_context.begin_position.line_num, _unnamed_type_object.source_context.begin_position.column_num);
            if (_unnamed_type_object.ne_list != null)
                _unnamed_type_object.ne_list.visit(this);
            cur_scope = tmp;
        }
        public override void visit(semantic_type_node stn) // SSM 
        {
        }
        public override void visit(sequence_type _sequence_type)
        {
            _sequence_type.elements_type.visit(this);
        }
        public override void visit(modern_proc_type _modern_proc_type)
        {
            if (_modern_proc_type.aloneparam != null)
            {
                _modern_proc_type.aloneparam.visit(this);
                if (_modern_proc_type.res != null)
                    _modern_proc_type.res.visit(this);
            }
            else
            {
                foreach (enumerator en in _modern_proc_type.el.enumerators)
                {
                    en.name.visit(this); // Это исправил - SSM 15.1.16
                }
                if (_modern_proc_type.res != null)
                    _modern_proc_type.res.visit(this);
            }
        }
    }
}
