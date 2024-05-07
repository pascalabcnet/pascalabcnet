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
        public void semantic_check_assign_tuple(addressed_value_list vars, expression ex) 
        {
            // Почти полностью идентичный код в двух проверках
            var expr = convert_strong(ex);
            expr = convert_if_typed_expression_to_function_call(expr);

            var t = ConvertSemanticTypeNodeToNETType(expr.type);
            if (t == null)
                AddError(expr.location, "TUPLE_OR_SEQUENCE_EXPECTED");

            var IsTuple = IsTupleType(t);
            var IsSequence = !IsTuple && IsSequenceType(t);

            if (!IsTuple && !IsSequence)
            {
                AddError(expr.location, "TUPLE_OR_SEQUENCE_EXPECTED");
            }

            if (IsTuple)
            {
                var n = vars.variables.Count();
                if (n > t.GetGenericArguments().Count())
                    AddError(get_location(vars), "TOO_MANY_ELEMENTS_ON_LEFT_SIDE_OF_TUPLE_ASSIGNMRNT");
            }
        }

        public void semantic_check_assign_var_tuple(ident_list vars, expression ex) 
        {
            // Почти полностью идентичный код в двух проверках
            var expr = convert_strong(ex);
            expr = convert_if_typed_expression_to_function_call(expr);

            var t = ConvertSemanticTypeNodeToNETType(expr.type);
            if (t == null)
            {
                bool bb;
                type_node elem_type = null;
                var b = FindIEnumerableElementType(expr.type, ref elem_type, out bb);
                if (!b)
                    AddError(expr.location, "TUPLE_OR_SEQUENCE_EXPECTED");
                return;
            }
                

            var IsTuple = IsTupleType(t);
            var IsSequence = !IsTuple && IsSequenceType(t);

            if (!IsTuple && !IsSequence)
            {
                AddError(expr.location, "TUPLE_OR_SEQUENCE_EXPECTED");
            }

            if (IsTuple)
            {
                var n = vars.idents.Count();
                if (n > t.GetGenericArguments().Count())
                    AddError(get_location(vars), "TOO_MANY_ELEMENTS_ON_LEFT_SIDE_OF_TUPLE_ASSIGNMRNT");
            }
        }

        void CheckOrdinaryType(expression ex)
        {
            var expr = convert_strong(ex);
            internal_interface ii = expr.type.get_internal_interface(internal_interface_kind.ordinal_interface);
            if (ii == null)
            {
                AddError(new OrdinalTypeExpected(get_location(ex)));
            }
        }

        void CheckOrdinaryType(expression_node expr)
        {
            internal_interface ii = expr.type.get_internal_interface(internal_interface_kind.ordinal_interface);
            if (ii == null)
            {
                AddError(new OrdinalTypeExpected(expr.location));
            }
        }

        void CheckInteger(expression ex)
        {
            if (ex == null)
                return;
            var semex = convert_strong(ex);
            var b = convertion_data_and_alghoritms.can_convert_type(semex, SystemLibrary.SystemLibrary.integer_type);
            if (!b)
                AddError(get_location(ex), "INTEGER_VALUE_EXPECTED");
        }

        void CheckIntegerOrIndex(expression ex)
        {
            var semex = convert_strong(ex);
            var b = convertion_data_and_alghoritms.can_convert_type(semex, SystemLibrary.SystemLibrary.integer_type);
            var toIsIndex = (semex is common_constructor_call toCall) &&
                toCall.common_type.comprehensive_namespace.namespace_full_name.Equals(StringConstants.pascalSystemUnitName) &&
                toCall.common_type.PrintableName.Equals("SystemIndex");

            var toIsIndex1 = (semex is compiled_constructor_call toCall1) &&
                toCall1.compiled_type.compiled_type.FullName.Equals("PABCSystem.SystemIndex");

            if (!b && !toIsIndex && !toIsIndex1)
            {
                AddError(get_location(ex), "INTEGER_VALUE_EXPECTED");
            }
        }

        void semantic_check_method_call_as_slice_expr(SyntaxTree.method_call mc)
        // нельзя проверять сахарный узел, т.к.могут быть вложенные сахарные expression!!
        {
            var v = (mc.dereferencing_value as dot_node).left;
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

            var from = mc.parameters.expressions[1];
            var to = mc.parameters.expressions[2];
            expression step = mc.parameters.expressions.Count > 3 ? mc.parameters.expressions[3] : null;

            CheckIntegerOrIndex(from);
            CheckIntegerOrIndex(to);
            CheckInteger(step);
        }

        void semantic_check_method_call_as_slice_expr_multi(SyntaxTree.method_call mc)
        {
            var v = (mc.dereferencing_value as dot_node).left;

            var semvar = convert_strong(v);
            if (semvar is typed_expression)
                semvar = convert_typed_expression_to_function_call(semvar as typed_expression);

            if (semvar.type.type_special_kind != SemanticTree.type_special_kind.array_kind)
                AddError(semvar.location, "SLICES_MULTI_DIMENSIONAL_ARRAY_EXPECTED");

            // Теперь с размерностями разберёмся
            int rank = 0;
            if (semvar.type is compiled_type_node)
                rank = (semvar.type as compiled_type_node).rank;
            else if (semvar.type is common_type_node)
                rank = (semvar.type as common_type_node).rank;
            if (rank != mc.parameters.Count)
                AddError(semvar.location, "NUMBER_OF_SLICES_IN_MULTIDIMENSIONAL_ARRAY_SHOULD_BE_EQUAL_TO_ARRAY_RANK");

            foreach (var param in mc.parameters.expressions)
            {
                var p = param as method_call; // Tuple.Create
                if (p == null)
                    AddError(semvar.location, "_Compiler_error_param_must_be_Tuple");

                var from = p.parameters.expressions[0];
                var to = p.parameters.expressions[1];
                var step = p.parameters.expressions[2];

                CheckIntegerOrIndex(from);
                CheckIntegerOrIndex(to);
                CheckInteger(step);
            }
        }

        /*void semantic_check_tuple(SyntaxTree.tuple_node tup)
        {
            //if (tup.el.expressions.Count > 7) 
			//	AddError(get_location(tup),"TUPLE_ELEMENTS_COUNT_MUST_BE_LESSEQUAL_7");
        }*/

        public void semantic_check_dot_question(SyntaxTree.question_colon_expression qce)
        {
            var av = convert_strong((qce.condition as bin_expr).left);
            Type t = null;
            if (av.type is compiled_generic_instance_type_node ctn2)
                t = ctn2.compiled_original_generic.compiled_type;
            else if (av.type is compiled_type_node ctn1)
                t = ctn1.compiled_type;
            if (av.type.type_special_kind == SemanticTree.type_special_kind.array_kind)
                return;
            if (!av.type.is_class && !av.type.IsInterface && !(t != null && t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>)) && !av.type.is_generic_parameter && !av.type.IsDelegate)
                AddError(av.location, "OPERATOR_DQ_MUST_BE_USED_WITH_A_REFERENCE_TYPE_VALUETYPE");
        }

        public void semantic_check_loop_stmt(SyntaxTree.expression ex)
        {
            var sem_ex = convert_strong(ex);
            var b = convertion_data_and_alghoritms.can_convert_type(sem_ex, SystemLibrary.SystemLibrary.integer_type);
            if (!b)
                AddError(sem_ex.location, "INTEGER_VALUE_EXPECTED");
        }

        public void semantic_check_slice_assignment_types(SyntaxTree.expression expr1, SyntaxTree.expression expr2, method_call mc)
        {
            var slice = expr1 as slice_expr;
            var leftValue = convert_strong(slice.v);
            var leftType = leftValue.type;
            var rightValue = convert_strong(expr2);
            var rightType = rightValue.type;

            var v = slice.v;
            var from = mc.parameters.expressions[2] as expression;
            var to = mc.parameters.expressions[3] as expression;
            expression step = mc.parameters.expressions.Count > 4 ? mc.parameters.expressions[4] : null;

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
            var fromIsIndex = (semfrom is common_constructor_call fromCall) &&
                fromCall.common_type.comprehensive_namespace.namespace_full_name.Equals(StringConstants.pascalSystemUnitName) &&
                fromCall.common_type.PrintableName.Equals("SystemIndex");
            var b = convertion_data_and_alghoritms.can_convert_type(semfrom, SystemLibrary.SystemLibrary.integer_type);
            if (!b && !fromIsIndex)
                AddError(get_location(from), "INTEGER_VALUE_EXPECTED");

            var semto = convert_strong(to);
            var toIsIndex = (semto is common_constructor_call toCall) &&
                toCall.common_type.comprehensive_namespace.namespace_full_name.Equals(StringConstants.pascalSystemUnitName) &&
                toCall.common_type.PrintableName.Equals("SystemIndex");
            b = convertion_data_and_alghoritms.can_convert_type(semto, SystemLibrary.SystemLibrary.integer_type);
            if (!b && !toIsIndex)
                AddError(get_location(to), "INTEGER_VALUE_EXPECTED");

            if (step != null)
            {
                var semstep = convert_strong(step);
                b = convertion_data_and_alghoritms.can_convert_type(semstep, SystemLibrary.SystemLibrary.integer_type);
                if (!b)
                    AddError(get_location(step), "INTEGER_VALUE_EXPECTED");
            }

            if (!AreTheSameType(leftType, rightType))
            {
                AddError(get_location(expr2), "EXPRESSION_OF_TYPE_{0}_CANNOT_BE_ASSIGNED_TO_SLICE_OF_TYPE_{1}", rightType.PrintableName, leftType.PrintableName);
            }
        }

        /*void common_diap_check(expression from, expression to)
        {

        }*/

        void semantic_check_method_call_as_diapason_expr(SyntaxTree.method_call mc)
        {
            var from = mc.parameters.expressions[0];
            var to = mc.parameters.expressions[1];

            var semfrom = convert_strong(from);
            var b = convertion_data_and_alghoritms.can_convert_type(semfrom, SystemLibrary.SystemLibrary.integer_type);
            var b1 = false;
            var br = false;
            if (!b)
                b1 = convertion_data_and_alghoritms.can_convert_type(semfrom, SystemLibrary.SystemLibrary.char_type);
            if (!b1)
                br = convertion_data_and_alghoritms.can_convert_type(semfrom, SystemLibrary.SystemLibrary.double_type);
            if (!b && !b1 && !br)
                AddError(get_location(from), "INTEGER_OR_REAL_OR_CHAR_VALUE_EXPECTED");

            var semto = convert_strong(to);
            var c = convertion_data_and_alghoritms.can_convert_type(semto, SystemLibrary.SystemLibrary.integer_type);
            var c1 = false;
            var cr = false;
            if (!c)
                c1 = convertion_data_and_alghoritms.can_convert_type(semto, SystemLibrary.SystemLibrary.char_type);
            if (!c1)
                cr = convertion_data_and_alghoritms.can_convert_type(semto, SystemLibrary.SystemLibrary.double_type);
            if (!c && !c1 && !cr)
                AddError(get_location(to), "INTEGER_OR_REAL_OR_CHAR_VALUE_EXPECTED");
            //if (b != c || c1 != b1 || cr != br)
            if (b && c || c1 && b1 || cr && br)
                return;
            if (b && cr || c && br)
                return;
            AddError(get_location(to), "INCOMPATIBLE_DIAPASON_BOUNDS_TYPES");
        }

        void semantic_check_method_call_as_inrange_expr(SyntaxTree.method_call mc)
        {
            var v = mc.parameters.expressions[0];
            var from = mc.parameters.expressions[1];
            var to = mc.parameters.expressions[2];

            var semv = convert_strong(v);
            var a = convertion_data_and_alghoritms.can_convert_type(semv, SystemLibrary.SystemLibrary.integer_type);
            var a1 = false;
            var ar = false;
            if (!a)
                a1 = convertion_data_and_alghoritms.can_convert_type(semv, SystemLibrary.SystemLibrary.char_type);
            if (!a1)
                ar = convertion_data_and_alghoritms.can_convert_type(semv, SystemLibrary.SystemLibrary.double_type);
            if (!a && !a1 && !ar)
                AddError(get_location(v), "INTEGER_OR_REAL_OR_CHAR_VALUE_EXPECTED");

            var semfrom = convert_strong(from);
            var b = convertion_data_and_alghoritms.can_convert_type(semfrom, SystemLibrary.SystemLibrary.integer_type);
            var b1 = false;
            var br = false;
            if (!b)
                b1 = convertion_data_and_alghoritms.can_convert_type(semfrom, SystemLibrary.SystemLibrary.char_type);
            if (!b1)
                br = convertion_data_and_alghoritms.can_convert_type(semfrom, SystemLibrary.SystemLibrary.double_type);
            if (!b && !b1 && !br)
                AddError(get_location(from), "INTEGER_OR_REAL_OR_CHAR_VALUE_EXPECTED");

            var semto = convert_strong(to);
            var c = convertion_data_and_alghoritms.can_convert_type(semto, SystemLibrary.SystemLibrary.integer_type);
            var c1 = false;
            var cr = false;
            if (!c)
                c1 = convertion_data_and_alghoritms.can_convert_type(semto, SystemLibrary.SystemLibrary.char_type);
            if (!c1)
                cr = convertion_data_and_alghoritms.can_convert_type(semto, SystemLibrary.SystemLibrary.double_type);
            if (!c && !c1 && !cr)
                AddError(get_location(to), "INTEGER_OR_REAL_OR_CHAR_VALUE_EXPECTED");

            var incompbound = true; 
            if (b && c || c1 && b1 || cr && br)
                incompbound = false;
            if (b && cr || c && br)
                incompbound = false;

            if (incompbound)
                AddError(get_location(to), "INCOMPATIBLE_DIAPASON_BOUNDS_TYPES");
            if (a && b1 || ar && b1 || a1 && b || a1 && c)
                AddError(get_location(v), "INCOMPATIBLE_TYPES_OF_ELEMENT_AND_DIAPASON");
        }

        void semantic_check_for_new_range(SyntaxTree.diapason_expr_new diap, type_definition td, ident id)
        {
            var from = diap.left;
            var to = diap.right;

            var semfrom = convert_strong(from);
            var b = convertion_data_and_alghoritms.can_convert_type(semfrom, SystemLibrary.SystemLibrary.integer_type);
            var b1 = false;
            if (!b)
                b1 = convertion_data_and_alghoritms.can_convert_type(semfrom, SystemLibrary.SystemLibrary.char_type);
            if (!b && !b1)
                AddError(get_location(from), "INTEGER_OR_CHAR_VALUE_EXPECTED");

            var semto = convert_strong(to);
            var c = convertion_data_and_alghoritms.can_convert_type(semto, SystemLibrary.SystemLibrary.integer_type);
            var c1 = false;
            if (!c)
                c1 = convertion_data_and_alghoritms.can_convert_type(semto, SystemLibrary.SystemLibrary.char_type);
            if (!c && !c1)
                AddError(get_location(to), "INTEGER_OR_CHAR_VALUE_EXPECTED");

            if (b != c || c1 != b1)
                AddError(get_location(to), "INCOMPATIBLE_DIAPASON_BOUNDS_TYPES");

            if (td != null && !(td is no_type_foreach))
            {
                // то мы определили тип явно в заголовке
                var semtype = convert_strong(td);
                var d = convertion_data_and_alghoritms.can_convert_type(semtype, SystemLibrary.SystemLibrary.integer_type);
                var d1 = false;
                if (!d)
                    d1 = convertion_data_and_alghoritms.can_convert_type(semtype, SystemLibrary.SystemLibrary.char_type);
                if (!d && !d1)
                    AddError(get_location(td), "INTEGER_OR_CHAR_VALUE_EXPECTED");
                if (b != d || b1 != d1)
                    AddError(get_location(id), "INCOMPATIBLE_TYPES_OF_ELEMENT_AND_DIAPASON");
            }
            else if (td == null)
            {
                var semid = convert_strong(id);
                var e = convertion_data_and_alghoritms.can_convert_type(semid, SystemLibrary.SystemLibrary.integer_type);
                var e1 = false;
                if (!e)
                    e1 = convertion_data_and_alghoritms.can_convert_type(semid, SystemLibrary.SystemLibrary.char_type);
                if (!e && !e1)
                    AddError(get_location(id), "INTEGER_OR_CHAR_VALUE_EXPECTED");

                if (b != e || b1 != e1)
                    AddError(get_location(id), "INCOMPATIBLE_TYPES_OF_ELEMENT_AND_DIAPASON");
            }
        }

        void semantic_check_for_indices(SyntaxTree.expression expr)
        {
            // Надо проверить, что expr - это одноразмерный массив или список. Эта проверка была в visit(foreach)
            var semexpr = convert_strong(expr);
            var Is1DArr = Is1DArray(semexpr);
            var il = IsIList(semexpr);
            // Тут может быть другая ситуация - Indices может быть членом semexpr - и тогда нельзя преобразовывать
            if (!Is1DArr && !il)
                AddError(get_location(expr), "ONE_DIM_ARRAY_OR_LIST_EXPECTED");
        }


    }
}
