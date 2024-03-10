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
        public spython_syntax_tree_visitor(): base()
        {
            OnLeave = RunAdditionalChecks;
        }
        public void RunAdditionalChecks(syntax_tree_node node)
        {
            switch (node)
            {
                case bin_expr _bin_expr:
                    expression_node left = convert_weak(_bin_expr.left);
                    expression_node right = convert_weak(_bin_expr.right);
                    if (_bin_expr.operation_type == Operators.Plus)
                    {
                        if (type_table.compare_types(left.type, right.type) == type_compare.greater_type)
                            AddError(left.location, "NOT_ALLOWED_SUM_DIFF_TYPES");
                        break;
                    }
                    break;
            }
        }
        public override void AddError(Error err, bool shouldReturn = false)
        {
            switch (err)
            {
                case OperatorCanNotBeAppliedToThisTypes _op_err:
                    expression_node left = _op_err.left;
                    expression_node right = _op_err.right;
                    if (_op_err.operator_name == "/")
                    {
                        if (left.type == right.type && left.type.name == "string")
                        {
                            var mcn = new method_call(new dot_node(new semantic_addr_value(left, left.location), new ident("IndexOf", left.location)),
                                new expression_list(new semantic_addr_value(right, right.location), right.location), _op_err.SourceContext);
                            visit(mcn);
                            //return;
                            throw new SemanticErrorFixed();
                        }
                    }
                    else if (_op_err.operator_name == "div")
                    {
                        if (left.type == right.type && left.type.name == "real")
                        {
                            var divnode = new bin_expr(new semantic_addr_value(left, left.location), new semantic_addr_value(right, right.location), Operators.Division, _op_err.SourceContext);
                            var floornode = new method_call(new ident("floor"), new expression_list(divnode));
                            base.visit(floornode);
                            throw new SemanticErrorFixed();
                        }
                    }
                    else if (_op_err.operator_name == "mod")
                    {
                        if (left.type == right.type && left.type.name == "real")
                        {
                            var divnode = new bin_expr(new semantic_addr_value(left, left.location), new semantic_addr_value(right, right.location), Operators.Division, _op_err.SourceContext);
                            var floornode = new method_call(new ident("floor"), new expression_list(divnode));
                            var multnode = new bin_expr(new semantic_addr_value(right, right.location), floornode, Operators.Multiplication);
                            var modnode = new bin_expr(new semantic_addr_value(left, left.location), multnode, Operators.Minus);
                            base.visit(modnode);
                            throw new SemanticErrorFixed();
                        }
                    }
                    break;
                default:
                    base.AddError(err, shouldReturn);
                    break;
            }
           
        }

        public override void visit(bin_expr _bin_expr)
        {
            try {
                base.visit(_bin_expr);
            }
            catch (SemanticErrorFixed) { }    
        }
        /*public override void visit(bin_expr _bin_expr)
        {
            // Лишний вызов convert_strong + плохо работает с лямбдами
            // TODO написать облегченный convert_strong
            expression_node left = convert_weak(_bin_expr.left);
            expression_node right = convert_weak(_bin_expr.right);

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
            }

            base.visit(_bin_expr);
        }*/
    }
}
