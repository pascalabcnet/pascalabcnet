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
        public void semantic_check_assign_tuple(SyntaxTree.assign_tuple asstup) // подходит и для assign_var_tuple
        {
            // Проверить, что справа - Tuple
            var expr = convert_strong(asstup.expr);
            expr = convert_if_typed_expression_to_function_call(expr);

            var t = ConvertSemanticTypeNodeToNETType(expr.type);
            if (t == null)
                AddError(expr.location, "TUPLE_EXPECTED");

            if (!t.FullName.StartsWith("System.Tuple"))
                AddError(expr.location, "TUPLE_EXPECTED");

            var n = asstup.vars.variables.Count();
            if (n > t.GetGenericArguments().Count())
                AddError(get_location(asstup.vars), "TOO_MANY_ELEMENTS_ON_LEFT_SIDE_OF_TUPLE_ASSIGNMRNT");
        }

        void semantic_check_slice_expr(SyntaxTree.slice_expr sl)
        {
            var semvar = convert_strong(sl.v);
            if (semvar is typed_expression)
                semvar = convert_typed_expression_to_function_call(semvar as typed_expression);

            var t = ConvertSemanticTypeNodeToNETType(semvar.type);

            var IsSlicedType = 0; // проверим, является ли semvar.type динамическим массивом, списком List или строкой
            // semvar.type должен быть array of T, List<T> или string
            if (t == null)
                IsSlicedType = 0; // можно ничего не присваивать :)
            else if (t.IsArray)
                IsSlicedType = 1;
            else if (t == typeof(System.String))
                IsSlicedType = 2;
            else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(System.Collections.Generic.List<>))
                IsSlicedType = 3;

            if (IsSlicedType == 0)
                AddError(get_location(sl.v), "BAD_SLICE_OBJECT");

            var semfrom = convert_strong(sl.from);
            var b = convertion_data_and_alghoritms.can_convert_type(semfrom, SystemLibrary.SystemLibrary.integer_type);
            if (!b)
                AddError(get_location(sl.from), "INTEGER_VALUE_EXPECTED");

            var semto = convert_strong(sl.to);
            b = convertion_data_and_alghoritms.can_convert_type(semto, SystemLibrary.SystemLibrary.integer_type);
            if (!b)
                AddError(get_location(sl.to), "INTEGER_VALUE_EXPECTED");

            if (sl.step != null)
            {
                var semstep = convert_strong(sl.step);
                b = convertion_data_and_alghoritms.can_convert_type(semstep, SystemLibrary.SystemLibrary.integer_type);
                if (!b)
                    AddError(get_location(sl.step), "INTEGER_VALUE_EXPECTED");
            }
        }
    }
}
