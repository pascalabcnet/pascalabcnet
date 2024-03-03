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
        public override void RunAdditionalChecks<T>(T node)
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
        public override void TryFixError(Error err, bool shouldReturn = false)
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
                            var mcn = new method_call(new dot_node(new ident((left as IReferenceNode).Variable.name, left.location), new ident("IndexOf")),
                                new expression_list(new ident((right as IReferenceNode).Variable.name, right.location)), _op_err.SourceContext);
                            base.visit(mcn);
                            //return;
                            throw new SemanticErrorFixed();
                        }
                    }
                    break;
                default:
                    base.TryFixError(err, shouldReturn);
                    break;
            }
           
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
