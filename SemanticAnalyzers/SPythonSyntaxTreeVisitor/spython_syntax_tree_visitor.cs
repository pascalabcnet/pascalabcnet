using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PascalABCCompiler.SemanticTree;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SystemLibrary;
using PascalABCCompiler.TreeConverter.TreeConversion;
using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.TreeRealization;
using PascalABCCompiler.Errors;


namespace SPythonSyntaxTreeVisitor
{
    public class spython_syntax_tree_visitor : syntax_tree_visitor, ISyntaxTreeVisitor
    {
        private string[] filesExtensions = { ".pys", ".py" };
        public spython_syntax_tree_visitor(): base()
        {
            OnLeave = RunAdditionalChecks;
        }
        public override string[] FilesExtensions
        {
            get
            {
                return filesExtensions;
            }
        }
        public void RunAdditionalChecks(syntax_tree_node node)
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
        public override void AddError(Error err, bool shouldReturn = false)
        {
            // TODO : Add Error Rerouting according to Python semantics
            switch (err)
            {
                case OperatorCanNotBeAppliedToThisTypes _op_err:
                    if (_op_err.operator_name == "mod")
                    {
                        base.AddError(new OperatorCanNotBeAppliedToThisTypes("%", _op_err.left, _op_err.right, _op_err.loc), shouldReturn);
                        return;
                    }
                    else if (_op_err.operator_name == "div")
                    {
                        base.AddError(new OperatorCanNotBeAppliedToThisTypes("//", _op_err.left, _op_err.right, _op_err.loc), shouldReturn);
                    }
                    break;
                case SimpleSemanticError _ss_err:
                    break;
            }
            base.AddError(err, shouldReturn);

        }
        public override void visit(bin_expr _bin_expr)
        {
            expression_node left = convert_strong(_bin_expr.left);
            expression_node right = convert_strong(_bin_expr.right);

            RunAdditionalChecks(_bin_expr);

            switch (_bin_expr.operation_type)
            {
                case Operators.Plus:
                    if (left.type == right.type && left.type.name == "boolean")
                    {
                        var int_left = new method_call(new ident("int"), new expression_list(new semantic_addr_value(left, left.location)), left.location);
                        var int_right = new method_call(new ident("int"), new expression_list(new semantic_addr_value(right, right.location)), right.location);
                        var bti_bin_expr = new bin_expr(int_left, int_right, _bin_expr.operation_type, _bin_expr.source_context);
                        visit(bti_bin_expr);
                        return;
                    }
                    break;
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
                        //var divnode = new bin_expr(new semantic_addr_value(left, left.location), new semantic_addr_value(right, right.location), Operators.Division, _bin_expr.source_context);
                        //var floornode = new method_call(new ident("@Floor"), new expression_list(divnode));
                        var exprlist = new expression_list(); exprlist.source_context = _bin_expr.source_context;
                        exprlist.Add(new semantic_addr_value(left, left.location));
                        exprlist.Add(new semantic_addr_value(right, right.location));
                        var floornode = new method_call(new ident("@FloorDiv"), exprlist, _bin_expr.source_context);
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
                        var modnode = new method_call(new ident("@FloorMod"), exprlist, _bin_expr.source_context);
                        visit(modnode);
                        return;
                    }
                    break;
            }
            var new_bin_expr = new bin_expr(new semantic_addr_value(left), new semantic_addr_value(right), _bin_expr.operation_type, _bin_expr.source_context);
            base.visit(new_bin_expr);
        }
        public override void visit(var_def_statement _var_def_statement)
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
            var si = context.find_first(_ident.name).sym_info as var_definition_node;
            if (si != null && si.type.name == "UnknownType")
            {
                var loc = get_location(_ident);
                AddError(loc, "SPYTHONSEMANTIC_USING_VARIABLE_BEFORE_ASSIGNMENT");
            }
            base.visit(_ident);
        }

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
