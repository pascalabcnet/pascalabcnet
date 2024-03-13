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
        private string[] filesExtensions = { ".yavb" };

        public override string[] FilesExtensions
        {
            get
            {
                return filesExtensions;
            }
        }
        /*public void RunAdditionalChecks(syntax_tree_node node)
        {
            switch (node)
            {
                case bin_expr _bin_expr:
                    expression_node left = convert_strong(_bin_expr.left);
                    expression_node right = convert_strong(_bin_expr.right);
                    if (_bin_expr.operation_type == Operators.Plus)
                    {
                        if (type_table.compare_types(left.type, right.type) == type_compare.greater_type)
                            AddError(left.location, "NOT_ALLOWED_SUM_DIFF_TYPES");
                        break;
                    }
                    break;
            }
        }*/
        public override void AddError(Error err, bool shouldReturn = false)
        {
            // TODO : Add Error Rerouting according to Python semantics
            /*switch (err)
            {
                case OperatorCanNotBeAppliedToThisTypes _op_err:
                    break;
                default:
                    base.AddError(err, shouldReturn);
                    break;
            }*/
            base.AddError(err, shouldReturn);
           
        }
        public override void visit(bin_expr _bin_expr)
        {
            expression_node left = convert_strong(_bin_expr.left);
            expression_node right = convert_strong(_bin_expr.right);

            switch (_bin_expr.operation_type)
            {
                case Operators.Plus:
                    if (type_table.compare_types(left.type, right.type) == type_compare.greater_type)
                        AddError(left.location, "NOT_ALLOWED_SUM_DIFF_TYPES");
                    break;
                case Operators.Division:
                    if (left.type == right.type && left.type.name == "string")
                    {
                        var mcn = new method_call(new dot_node(_bin_expr.left as ident, new ident("IndexOf")),
                            new expression_list(_bin_expr.right as ident), _bin_expr.source_context);
                        base.visit(mcn);
                        return;
                    }
                    break;
                case Operators.IntegerDivision:
                    if (left.type == right.type && left.type.name == "real")
                    {
                        var divnode = new bin_expr(new semantic_addr_value(left, left.location), new semantic_addr_value(right, right.location), Operators.Division, _bin_expr.source_context);
                        var floornode = new method_call(new ident("floor"), new expression_list(divnode));
                        base.visit(floornode);
                        return;
                    }
                    break;
                case Operators.ModulusRemainder:
                    if (left.type == right.type && left.type.name == "real")
                    {
                        var divnode = new bin_expr(new semantic_addr_value(left, left.location), new semantic_addr_value(right, right.location), Operators.IntegerDivision, _bin_expr.source_context);
                        var multnode = new bin_expr(new semantic_addr_value(right, right.location), divnode, Operators.Multiplication);
                        var modnode = new bin_expr(new semantic_addr_value(left, left.location), multnode, Operators.Minus);
                        base.visit(modnode);
                        return;
                    }
                    break;
            }
            var new_bin_expr = new bin_expr(new semantic_addr_value(left), new semantic_addr_value(right), _bin_expr.operation_type, _bin_expr.source_context);
            base.visit(new_bin_expr);
        }
    }
}
