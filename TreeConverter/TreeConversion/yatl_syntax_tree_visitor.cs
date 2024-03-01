using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.SyntaxTree;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeRealization;
using PascalABCCompiler.SemanticTree;
using PascalABCCompiler.SystemLibrary;
using PascalABCCompiler.TreeConverter.TreeConversion;

namespace YATLTreeConversion
{
    class yatl_syntax_tree_visitor: syntax_tree_visitor, ISyntaxTreeVisitor
    {
        private string[] filesExtensions = { ".yatp" };

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

            if (_bin_expr.operation_type == Operators.Plus)
            {
                if (type_table.compare_types(left.type, right.type) == type_compare.greater_type)
                    AddError(left.location, "NOT_ALLOWED_SUM_DIFF_TYPES");
            }  
            base.visit(_bin_expr);
        }
    }
}
