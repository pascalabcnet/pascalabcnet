using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors;

namespace Languages.SPython.Frontend.Converters
{
    internal class CompoundComparisonDesugarVisitor : BaseChangeVisitor
    {
        private int curr_sl_index;

        public CompoundComparisonDesugarVisitor() { }

        public override void visit(statement_list sl)
        {
            if (sl == null) return;
            for (var i = sl.subnodes_count - 1; i >= 0; i--)
            {
                curr_sl_index = i;
                ProcessNode(sl[i]);
            }
        }

        private static statement_list GetCurrentStatementList(syntax_tree_node curr)
        {
            while (!(curr is statement_list)) curr = curr.Parent;
            return curr as statement_list;
        }

        private static bool IsComparison(bin_expr _bin_expr)
        {
            Operators Operator = _bin_expr.operation_type;
            switch (Operator)
            {
                case Operators.Less:
                case Operators.Greater:
                case Operators.LessEqual:
                case Operators.GreaterEqual:
                case Operators.Equal:
                case Operators.NotEqual:
                    return true;
            }
            return false;
        }

        private static bool IsCompoundComparison(bin_expr _bin_expr)
        {
            return (_bin_expr.left is bin_expr left) && IsComparison(_bin_expr) && IsComparison(left);
        }

        private int createdIdentsCounter = 0;

        private string NewIdentName()
        {
            string result = '%' + createdIdentsCounter.ToString();
            createdIdentsCounter++;
            return result;
        }

        public override void visit(bin_expr _bin_expr)
        {
            if (IsCompoundComparison(_bin_expr))
            {
                // transform
                //
                // ((left comp1 middle) comp2 right)
                //
                // to
                //
                // var id: typeof(middle);
                // ((left comp1 !assign(id, middle)) && (id comp2 right))
                //
                // then recursive call for left
                // P.S. !assign(a, b) assigns b to a and return a

                bin_expr left_comp_middle = _bin_expr.left as bin_expr;
                expression left = left_comp_middle.left;
                Operators comp1 = left_comp_middle.operation_type;
                expression middle = left_comp_middle.right;
                Operators comp2 = _bin_expr.operation_type;
                expression right = _bin_expr.right;
                ident id = new ident(NewIdentName(), middle.source_context);
                type_definition td = new same_type_node(middle.TypedClone(), middle.source_context);
                var_def_statement vds = new var_def_statement(id, td, middle.source_context);
                statement_list curr_statement_list = GetCurrentStatementList(_bin_expr);
                int index = curr_sl_index - curr_statement_list.subnodes_without_list_elements_count;
                curr_statement_list.list.Insert(index, new var_statement(vds, middle.source_context));
                curr_sl_index++;

                addressed_value method_name = new ident("!assign", middle.source_context);
                expression_list el = new expression_list(new List<expression> { id, middle }, middle.source_context);
                method_call method_call = new method_call(method_name, el, middle.source_context);
                bin_expr new_left = new bin_expr(left, method_call, comp1, new SourceContext(left.source_context, method_call.source_context));
                bin_expr new_right = new bin_expr(id, right, comp2, new SourceContext(id.source_context, right.source_context));
                bin_expr new_bin_expr = new bin_expr(new_left, new_right, Operators.LogicalAND, _bin_expr.source_context);
                _bin_expr.Parent.ReplaceDescendant(_bin_expr, new_bin_expr);
                new_bin_expr.left.visit(this);
            }
            else
                base.visit(_bin_expr);
        }
    }
}
