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
        private string[] filesExtensions = { ".pys" };
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
                        if ((left.type.name == "string" && right.type.name == "integer") || ((left.type.name == "integer" && right.type.name == "string")))
                        {
                            AddError(left.location, "SPYTHONSEMANTIC_NOT_ALLOWED_{0}_DIFF_TYPES_{1}_{2}", '+', left.type, right.type); ;
                            //base.AddError(left.location, "SPYTHONSEMANTIC_NOT_ALLOWED_{0}_DIFF_TYPES_{1}_{2}", Operators.Plus, left.type, right.type); ;
                            return;
                        }
                    }
                    break;
            }
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
                        var divnode = new bin_expr(new semantic_addr_value(left, left.location), new semantic_addr_value(right, right.location), Operators.Division, _bin_expr.source_context);
                        var floornode = new method_call(new ident("@Floor"), new expression_list(divnode));
                        visit(floornode);
                        return;
                    }
                    break;
                case Operators.ModulusRemainder:
                    if (left.type == right.type && left.type.name == "real")
                    {
                        var divnode = new bin_expr(new semantic_addr_value(left, left.location), new semantic_addr_value(right, right.location), Operators.IntegerDivision, _bin_expr.source_context);
                        var multnode = new bin_expr(new semantic_addr_value(right, right.location), divnode, Operators.Multiplication);
                        var modnode = new bin_expr(new semantic_addr_value(left, left.location), multnode, Operators.Minus);
                        visit(modnode);
                        return;
                    }
                    break;
            }
            var new_bin_expr = new bin_expr(new semantic_addr_value(left), new semantic_addr_value(right), _bin_expr.operation_type, _bin_expr.source_context);
            base.visit(new_bin_expr);
        }
    }
}
