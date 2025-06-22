﻿using System.Collections.Generic;
using PascalABCCompiler.SemanticTree;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SystemLibrary;
using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.TreeRealization;
using PascalABCCompiler.Errors;
using System.ComponentModel.Design;
using System;

namespace SPythonSyntaxTreeVisitor
{
    // Возможно, стоит заменить на декоратор или стратегию вместо наследования EVA
    public class spython_syntax_tree_visitor : syntax_tree_visitor
    {
        syntax_tree_visitor mainVisitor;

        public spython_syntax_tree_visitor(syntax_tree_visitor mainSyntaxTreeVisitor) : base(false)
        {

            mainVisitor = mainSyntaxTreeVisitor;
            convertion_data_and_alghoritms = mainSyntaxTreeVisitor.convertion_data_and_alghoritms;
            ret = mainSyntaxTreeVisitor.ret;
            context = mainSyntaxTreeVisitor.context;
            contextChanger = mainSyntaxTreeVisitor.contextChanger;

            OnLeave = RunAdditionalChecks;
        }

        private void RunAdditionalChecks(syntax_tree_node node)
        {
            switch (node)
            {
                case bin_expr _bin_expr:
                    expression_node left = convert_strong(_bin_expr.left);
                    expression_node right = convert_strong(_bin_expr.right);
                    if (_bin_expr.operation_type == Operators.Plus)
                    {
                        if ((left.type.name == "string" && right.type.name == "integer") || (left.type.name == "integer" && right.type.name == "string"))
                        {
                            AddError(left.location, "SPYTHONSEMANTIC_NOT_ALLOWED_{0}_DIFF_TYPES_{1}_{2}", '+', left.type, right.type); ;
                            //base.AddError(left.location, "SPYTHONSEMANTIC_NOT_ALLOWED_{0}_DIFF_TYPES_{1}_{2}", Operators.Plus, left.type, right.type); ;
                            return;
                        }
                    }
                    break;
            }
            return;
        }

        protected override void get_system_module(common_unit_node psystem_unit)
        {
            init_system_module(psystem_unit);

            //esli zapustili v otladke, to vosstanovim mnozhestvo i procedury sozdanija diapasonov, inache ne budet rabotat
            if (debugging)
            {
                List<SymbolInfo> si = SystemLibInitializer.CreateDiapason.SymbolInfo;
                si = SystemLibInitializer.CreateObjDiapason.SymbolInfo;
                si = SystemLibInitializer.TypedSetType.SymbolInfo;
            }

            // SystemUnitAssigned = true; - убрали для SPython
            CreateSpecialFields(psystem_unit);
        }

        // Инициализируем только переменные экземпляра, не влияем на глобальное состояние в отличие от визитора Паскаля EVA
        protected override void internal_reset()
        {
            _system_unit = mainVisitor._system_unit;
            SystemLibrary.system_unit = _system_unit;
            ResetSelfFields();
        }

        public override void AddError(location loc, string ErrResourceString, params object[] values)
        {
            Error err = new SPythonSemanticError(loc, ErrResourceString, values);
            if (ErrResourceString == "FORWARD_DECLARATION_{0}_AS_BASE_TYPE")
            {
                throw err;
            }
            else
            {
                base.AddError(err);
            }
        }

        private type_node ConvertTypeNameToSPythonTypeName(type_node tn)
        {
            string new_name = tn.PrintableName;
            new_name = new_name
                .Replace(",", ", ")
                .Replace("<", "[")
                .Replace(">", "]")
                .Replace("Tuple", "tuple")
                .Replace("List", "list")
                .Replace("NewSet", "set")
                .Replace("Dictionary", "dict")
                .Replace("empty_list", "list[anytype]")
                .Replace("empty_set", "set[anytype]")
                .Replace("empty_dict", "dict[anytype]")
                .Replace("integer", "int")
                .Replace("string", "str")
                .Replace("real", "float")
                .Replace("boolean", "bool")
                .Replace("System.Numerics.BigInteger", "bigint")
                ;
            return new common_type_node(new_name, type_access_level.tal_public, null, null, tn.location);
        }

        public override void AddError(Error err, bool shouldReturn = false)
        {
            // TODO : Add Error Rerouting according to SPython semantics
            switch (err)
            {
                case OperatorCanNotBeAppliedToThisTypes _op_err:
                    if (_op_err.operator_name == "mod")
                        base.AddError(new OperatorCanNotBeAppliedToThisTypes("%", _op_err.left, _op_err.right, _op_err.loc), shouldReturn);
                    else if (_op_err.operator_name == "div")
                        base.AddError(new OperatorCanNotBeAppliedToThisTypes("//", _op_err.left, _op_err.right, _op_err.loc), shouldReturn);
                    return;
                case FunctionExpectedProcedureMeet _proc_meet:
                    base.AddError(new SPythonSemanticError(_proc_meet.loc, "SPYTHONSEMANTIC_FUNCTION_{0}_NO_RETURN", _proc_meet.function.name));
                    return;
                case CanNotConvertTypes conv_err:
                    base.AddError(new CanNotConvertTypes(conv_err.expression_node,
                        ConvertTypeNameToSPythonTypeName(conv_err.from), 
                        ConvertTypeNameToSPythonTypeName(conv_err.to),
                        conv_err.loc));
                    return;
            }
            base.AddError(err, shouldReturn);

        }

        private Dictionary<string, Dictionary<string, string>> containersNamesMapping = new Dictionary<string, Dictionary<string, string>>
        {
            {"List",
                new Dictionary<string, string> {
                { "append", "Add" },
                { "clear", "Clear" },
                { "insert", "Insert" },
                { "remove", "Remove" },
                { "pop", "pop" },
                { "index", "IndexOf" },
                { "count", "!count" },
                { "sort", "Sort" },
                { "reverse", "Reverse" },
                { "copy", "ToList" },
                { "Select", "Select" },
                { "Where", "Where" },
            } },

            {"NewSet",
                new Dictionary<string, string> {
                { "add", "add" },
                { "remove", "remove" },
                { "copy", "copy" },
                { "Select", "Select" },
                { "Where", "Where" },
            } },

            {"Dictionary",
                new Dictionary<string, string> {
                { "keys", "get_keys" },
                { "values", "get_values" },
                { "copy", "copy" },
                { "Select", "Select" },
                { "Where", "Where" },
            } }
        };


        HashSet<method_call> visited_method_calls = new HashSet<method_call>();

        public override void visit(method_call _method_call)
        {
            if (visited_method_calls.Contains(_method_call))
            {
                base.visit(_method_call);
                return;
            }
            visited_method_calls.Add(_method_call);
            if (_method_call.dereferencing_value is dot_node dn && dn.right is ident id)
            {
                try
                {
                    expression_node left = convert_strong(dn.left);
                    //dn.left = new semantic_addr_value(left);
                    if (left?.type != null) 
                    foreach (string tName in containersNamesMapping.Keys)
                    {
                        if (left.type.name.StartsWith(tName))
                        {
                            if (!containersNamesMapping[tName].ContainsKey(id.name))
                            {
                                AddError(left.location, "SPYTHONSEMANTIC_TYPE_{0}_HAS_NO_{1}_METHOD", ConvertTypeNameToSPythonTypeName(left.type), id.name);
                            }
                            else
                            {
                                id.name = containersNamesMapping[tName][id.name];
                            }
                        }
                    }
                }
                catch (ExpectedAnotherKindOfObject e)
                {
                }
            }
            base.visit(_method_call);
        }

        public override void visit(assign _assign)
        {

            if (_assign.from is bin_expr be && 
                _assign.to is ident il && 
                be.left is ident ir &&
                il.name == ir.name)
            {
                expression_node to = convert_strong(_assign.to);
                SourceContext sc = _assign.source_context;

                if (to.type.name.StartsWith("NewSet"))
                {
                    if (be.operation_type == Operators.LogicalAND)
                    {
                        var replace = new assign(new semantic_addr_value(to), be.right, Operators.AssignmentMultiplication, sc);
                        base.visit(replace);
                        return;
                    }
                    if (be.operation_type == Operators.LogicalOR)
                    {
                        var replace = new assign(new semantic_addr_value(to), be.right, Operators.AssignmentAddition, sc);
                        base.visit(replace);
                        return;
                    }
                }
            }
            base.visit(_assign);
        }

        public override void visit(bin_expr _bin_expr)
        {
            expression_node left = convert_strong(_bin_expr.left);
            expression_node right = convert_strong(_bin_expr.right);

            var new_bin_expr = new bin_expr(new semantic_addr_value(left), new semantic_addr_value(right), _bin_expr.operation_type, _bin_expr.source_context);

            RunAdditionalChecks(new_bin_expr);

            switch (_bin_expr.operation_type)
            {
                case Operators.LogicalOR:
                    if (left.type.name.StartsWith("NewSet") && left.type.name == right.type.name)
                    {
                        new_bin_expr.operation_type = Operators.Plus;
                    }
                    break;
                case Operators.LogicalAND:
                    if (left.type.name.StartsWith("NewSet") && left.type.name == right.type.name)
                    {
                        new_bin_expr.operation_type = Operators.Multiplication;
                    }
                    break;
                /*case Operators.Plus:
                    if (left.type == right.type && left.type.name == "boolean")
                    {
                        var int_left = new method_call(new ident("int"), new expression_list(new semantic_addr_value(left, left.location)), left.location);
                        var int_right = new method_call(new ident("int"), new expression_list(new semantic_addr_value(right, right.location)), right.location);
                        var bti_bin_expr = new bin_expr(int_left, int_right, _bin_expr.operation_type, _bin_expr.source_context);
                        visit(bti_bin_expr);
                        return;
                    }
                    break;*/
                case Operators.Division:
                    if (left.type == right.type && left.type.name == "string")
                    {
                        var mcn = new method_call(new dot_node(new semantic_addr_value(left, left.location), new ident("IndexOf")),
                            new expression_list(new semantic_addr_value(right, right.location)), _bin_expr.source_context);
                        visit(mcn);
                        return; 
                    }
                    break;
                case Operators.IntegerDivision:
                    if (left.type == right.type && left.type.name == "real")
                    {
                        var exprlist = new expression_list(); exprlist.source_context = _bin_expr.source_context;
                        exprlist.Add(new semantic_addr_value(left, left.location));
                        exprlist.Add(new semantic_addr_value(right, right.location));
                        var floornode = new method_call(new ident("!FloorDiv"), exprlist, _bin_expr.source_context);
                        visit(floornode);
                        return;
                    }
                    break;
                case Operators.ModulusRemainder:
                    if (left.type == right.type && left.type.name == "real")
                    {
                        //var divnode = new bin_expr(new semantic_addr_value(left, left.location), new semantic_addr_value(right, right.location), Operators.IntegerDivision, _bin_expr.source_context);
                        //var multnode = new bin_expr(new semantic_addr_value(right, right.location), divnode, Operators.Multiplication);
                        //var modnode = new bin_expr(new semantic_addr_value(left, left.location), multnode, Operators.Minus);
                        var exprlist = new expression_list(); exprlist.source_context = _bin_expr.source_context;
                        exprlist.Add(new semantic_addr_value(left, left.location));
                        exprlist.Add(new semantic_addr_value(right, right.location));
                        var modnode = new method_call(new ident("!FloorMod"), exprlist, _bin_expr.source_context);
                        visit(modnode);
                        return;
                    }
                    break;
            }
            base.visit(new_bin_expr);
        }
        
        // Реализация глобальных переменных для семантики. Возможно, пригодится позже   EVA
        /*public override void visit(var_def_statement _var_def_statement)
        {
            if (_var_def_statement.vars_type != null)
            {
                var loc = get_location(_var_def_statement.vars_type);
                type_node tn = find_type(_var_def_statement.vars_type as named_type_reference, loc);
                _var_def_statement.vars_type = new semantic_type_node(tn);
            }
            base.visit(_var_def_statement);
        }

        public override void visit(ident _ident)
        {
            var si = context.find_first(_ident.name)?.sym_info as var_definition_node;
            if (si != null && si.type.name == "@UnknownType")
            {
                var loc = get_location(_ident);
                AddError(loc, "SPYTHONSEMANTIC_USING_VARIABLE_BEFORE_ASSIGNMENT");
            }
            base.visit(_ident);
        }*/

        /*public override void visit(named_type_reference _named_type_reference)
        {
            var loc = get_location(_named_type_reference);
            type_node tn = find_type(_named_type_reference, loc);
            if (tn.name == "UnknownType")
            {
                base.AddError(loc, "USING_VARIABLE_BEFORE_ASSIGNMENT");
            }
            var _new_ntr = new semantic_addr_value(tn);
            base.visit(_named_type_reference);
        }*/
    }
}
