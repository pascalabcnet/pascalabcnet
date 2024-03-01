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
        public override void visit(bin_expr _bin_expr)
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
        }
    }
}
