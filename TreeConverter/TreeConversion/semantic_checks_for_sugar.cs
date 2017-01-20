using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeRealization;

namespace PascalABCCompiler.TreeConverter
{
    public partial class syntax_tree_visitor
    {
        public void semantic_check_sugared_statement(SyntaxTree.assign_tuple assvartup) // подходит и для assign_var_tuple
        {
            // Проверить, что справа - Tuple
            var expr = convert_strong(assvartup.expr);
            expr = convert_if_typed_expression_to_function_call(expr);

            var t = ConvertSemanticTypeNodeToNETType(expr.type);
            if (t == null)
                AddError(expr.location, "TUPLE_EXPECTED");

            if (!t.FullName.StartsWith("System.Tuple"))
                AddError(expr.location, "TUPLE_EXPECTED");

            var n = assvartup.vars.variables.Count();
            if (n > t.GetGenericArguments().Count())
                AddError(get_location(assvartup.vars), "TOO_MANY_ELEMENTS_ON_LEFT_SIDE_OF_TUPLE_ASSIGNMRNT");
        }
    }
}
