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
        public void semantic_check_assign_tuple(addressed_value_list vars, expression ex) // подходит и для assign_var_tuple
        {
            // Проверить, что справа - Tuple
            var expr = convert_strong(ex);
            expr = convert_if_typed_expression_to_function_call(expr);

            var t = ConvertSemanticTypeNodeToNETType(expr.type);
            if (t == null)
                AddError(expr.location, "TUPLE_EXPECTED");

            if (!t.FullName.StartsWith("System.Tuple"))
                AddError(expr.location, "TUPLE_EXPECTED");

            var n = vars.variables.Count();
            if (n > t.GetGenericArguments().Count())
                AddError(get_location(vars), "TOO_MANY_ELEMENTS_ON_LEFT_SIDE_OF_TUPLE_ASSIGNMRNT");
        }

        void semantic_check_method_call_as_slice_expr(SyntaxTree.method_call mc)
        // нельзя проверять сахарный узел, т.к.могут быть вложенные сахарные expression!!
        {
            var v = (mc.dereferencing_value as dot_node).left;
            var from = mc.parameters.expressions[1];
            var to = mc.parameters.expressions[2];
            expression step = mc.parameters.expressions.Count > 3 ? mc.parameters.expressions[3] : null;

            var semvar = convert_strong(v);
            if (semvar is typed_expression)
                semvar = convert_typed_expression_to_function_call(semvar as typed_expression);

            var IsSlicedType = 0; // проверим, является ли semvar.type динамическим массивом, списком List или строкой
            if (semvar.type.type_special_kind == SemanticTree.type_special_kind.array_kind)
                IsSlicedType = 1;

            if (IsSlicedType == 0)
            {
                var t = ConvertSemanticTypeNodeToNETType(semvar.type); // не работает для array of T

                // semvar.type должен быть array of T, List<T> или string
                if (t == null)
                    IsSlicedType = 0; // можно ничего не присваивать :)
//                else if (t.IsArray)
//                  IsSlicedType = 1;
                else if (t == typeof(System.String))
                    IsSlicedType = 2;
                else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(System.Collections.Generic.List<>))
                    IsSlicedType = 3;
            }

            if (IsSlicedType == 0)
                AddError(get_location(v), "BAD_SLICE_OBJECT");

            var semfrom = convert_strong(from);
            var b = convertion_data_and_alghoritms.can_convert_type(semfrom, SystemLibrary.SystemLibrary.integer_type);
            if (!b)
                AddError(get_location(from), "INTEGER_VALUE_EXPECTED");

            var semto = convert_strong(to);
            b = convertion_data_and_alghoritms.can_convert_type(semto, SystemLibrary.SystemLibrary.integer_type);
            if (!b)
                AddError(get_location(to), "INTEGER_VALUE_EXPECTED");

            if (step != null)
            {
                var semstep = convert_strong(step);
                b = convertion_data_and_alghoritms.can_convert_type(semstep, SystemLibrary.SystemLibrary.integer_type);
                if (!b)
                    AddError(get_location(step), "INTEGER_VALUE_EXPECTED");
            }

        }
        /*void semantic_check_tuple(SyntaxTree.tuple_node tup)
        {
            //if (tup.el.expressions.Count > 7) 
			//	AddError(get_location(tup),"TUPLE_ELEMENTS_COUNT_MUST_BE_LESSEQUAL_7");
        }*/
    }
}
