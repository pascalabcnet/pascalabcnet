using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeRealization;

using SyntaxTreeBuilder = PascalABCCompiler.SyntaxTree.SyntaxTreeBuilder;
using SymTable = SymbolTable;
using array_const = PascalABCCompiler.TreeRealization.array_const;
using compiler_directive = PascalABCCompiler.TreeRealization.compiler_directive;
using empty_statement = PascalABCCompiler.TreeRealization.empty_statement;
using for_node = PascalABCCompiler.TreeRealization.for_node;
using goto_statement = PascalABCCompiler.TreeRealization.goto_statement;
using if_node = PascalABCCompiler.TreeRealization.if_node;
using labeled_statement = PascalABCCompiler.TreeRealization.labeled_statement;
using question_colon_expression = PascalABCCompiler.TreeRealization.question_colon_expression;
using repeat_node = PascalABCCompiler.TreeRealization.repeat_node;
using sizeof_operator = PascalABCCompiler.TreeRealization.sizeof_operator;
using typeof_operator = PascalABCCompiler.TreeRealization.typeof_operator;
using while_node = PascalABCCompiler.TreeRealization.while_node;
using TreeConverter.LambdaExpressions.Closure;
using TreeConverter.LambdaExpressions;

namespace PascalABCCompiler.TreeConverter
{
    public partial class syntax_tree_visitor
    {



        public bool internal_is_assign = false;



        public override void visit(SyntaxTree.assign _assign)
        {
            internal_is_assign = true;
            addressed_expression to = convert_address_strong(_assign.to);
            internal_is_assign = false;
            if (to == null)
                AddError(get_location(_assign.to), "CAN_NOT_ASSIGN_TO_LEFT_PART");


            #region Вывод параметров лямбда-выражения
            var fld1 = _assign.from as SyntaxTree.function_lambda_definition;
            if (fld1 != null)
            {
                //MaybeConvertFunctionLambdaDefinitionToProcedureLambdaDefinition(fld1); - вроде бы это делается внутри InferTypesFromVarStmt
                LambdaHelper.InferTypesFromVarStmt(to.type, fld1, this);  //lroman//
            }

            #endregion


            //(ssyy) Вставляю проверки прямо сюда, т.к. запарился вылавливать другие случаи.
            bool flag = false;
            general_node_type node_type = general_node_type.constant_definition;
            if (convertion_data_and_alghoritms.check_for_constant_or_readonly(to, out flag, out node_type))
            {
                if (flag)
                    AddError(to.location, "CAN_NOT_ASSIGN_TO_CONSTANT_OBJECT");
                else
                    AddError(new CanNotAssignToReadOnlyElement(to.location, node_type));
            }

            //expression_node from = convert_strong(_assign.from);
            // SSM исправление Саушкина 10.03.16
            expression_node from;
            var fromAsLambda = _assign.from as function_lambda_definition;
            if (fromAsLambda != null)
            {
                var lambdaVisitMode = fromAsLambda.lambda_visit_mode;
                fromAsLambda.lambda_visit_mode = LambdaVisitMode.VisitForAdvancedMethodCallProcessing;
                from = convert_strong(_assign.from);
                fromAsLambda.lambda_visit_mode = lambdaVisitMode;
            }
            else
            {
                from = convert_strong(_assign.from);
                // SSM 26.06.16 - правка в связи с автовыведением типов в yieldах
                if (to.type is auto_type)
                {
                    try_convert_typed_expression_to_function_call(ref from);
                    if (to is class_field_reference)
                    {
                        var cfr = to as class_field_reference;
                        cfr.field.type = from.type;
                        cfr.type = from.type;
                    }
                    else if (to is local_block_variable_reference)
                    {
                        var lvr = to as local_block_variable_reference;
                        lvr.var.type = from.type;
                        lvr.type = from.type;
                    }
                    else AddError(to.location, "Не могу вывести тип при наличии yield: " + to.type.full_name);
                    //to.type = from.type; // и без всякого real_type!
                }
                else if (to.type is compiled_generic_instance_type_node && (to.type as compiled_generic_instance_type_node).instance_params[0] is ienumerable_auto_type)
                {
                    var tt = to.type;
                    type_node elem_type = null;
                    try_convert_typed_expression_to_function_call(ref from);
                    bool bb; // здесь bb не нужно. Оно нужно в foreach
                    var b = FindIEnumerableElementType(from.type, ref elem_type, out bb);
                    if (!b)
                        AddError(from.location, "CAN_NOT_EXECUTE_FOREACH_BY_EXPR_OF_TYPE_{0}", from.type.full_name);

                    var IEnumType = new template_type_reference(new named_type_reference("System.Collections.Generic.IEnumerable"), new template_param_list(new semantic_type_node(elem_type)));
                    if (to is class_field_reference)
                    {
                        var cfr = to as class_field_reference;

                        cfr.field.type = convert_strong(IEnumType);
                        cfr.type = cfr.field.type;
                    }
                    else if (to is local_block_variable_reference)
                    {
                        var lvr = to as local_block_variable_reference;

                        lvr.var.type = convert_strong(IEnumType); // замена типа у описания переменной
                        lvr.type = lvr.var.type;                  // замена типа у переменной
                    }
                }
            }
            // end

            //SSM 4.04.16
            if (to.type is undefined_type)
                to.type = from.type;

            if (stflambda.Count > 0) // мы находимся внутри лямбды - возможно, вложенной
            {
                var fld = stflambda.Peek();
                if (_assign.to is ident && (_assign.to as ident).name.ToLower() == "result" && fld.RealSemTypeOfResExpr == null) // если это - первое присваивание Result
                {
                    fld.RealSemTypeOfResExpr = from.type;
                    fld.RealSemTypeOfResult = to.type;
                }

            }

            location loc = get_location(_assign);
            bool oper_ass_in_prop = false;

            //проверка на обращение к полю записи возвращенной из функции с целью присваивания
            //нужно чтобы пользователь не мог менять временный обьект
            if (to.semantic_node_type == semantic_node_type.static_property_reference ||
                to.semantic_node_type == semantic_node_type.non_static_property_reference)
            {
                property_node pn = null;
                base_function_call prop_expr = null;
                if (to.semantic_node_type == semantic_node_type.static_property_reference)
                    pn = (to as static_property_reference).property;
                else
                    pn = (to as non_static_property_reference).property;

                PascalABCCompiler.SyntaxTree.Operators ot = PascalABCCompiler.SyntaxTree.Operators.Undefined;
                switch (_assign.operator_type)
                {
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentAddition:
                        ot = PascalABCCompiler.SyntaxTree.Operators.Plus;
                        oper_ass_in_prop = true;
                        break;
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentBitwiseAND:
                        ot = PascalABCCompiler.SyntaxTree.Operators.BitwiseAND;
                        oper_ass_in_prop = true;
                        break;
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentBitwiseLeftShift:
                        ot = PascalABCCompiler.SyntaxTree.Operators.BitwiseLeftShift;
                        oper_ass_in_prop = true;
                        break;
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentBitwiseOR:
                        ot = PascalABCCompiler.SyntaxTree.Operators.BitwiseOR;
                        oper_ass_in_prop = true;
                        break;
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentBitwiseRightShift:
                        ot = PascalABCCompiler.SyntaxTree.Operators.BitwiseRightShift;
                        oper_ass_in_prop = true;
                        break;
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentBitwiseXOR:
                        ot = PascalABCCompiler.SyntaxTree.Operators.BitwiseXOR;
                        oper_ass_in_prop = true;
                        break;
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentDivision:
                        ot = PascalABCCompiler.SyntaxTree.Operators.Division;
                        oper_ass_in_prop = true;
                        break;
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentModulus:
                        ot = PascalABCCompiler.SyntaxTree.Operators.ModulusRemainder;
                        oper_ass_in_prop = true;
                        break;
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentMultiplication:
                        ot = PascalABCCompiler.SyntaxTree.Operators.Multiplication;
                        oper_ass_in_prop = true;
                        break;
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentSubtraction:
                        ot = PascalABCCompiler.SyntaxTree.Operators.Minus;
                        oper_ass_in_prop = true;
                        break;
                }
                if (oper_ass_in_prop)
                {
                    if (pn.get_function == null)
                        AddError(new ThisPropertyCanNotBeReaded(pn, loc));
                    if (to.semantic_node_type == semantic_node_type.non_static_property_reference)
                    {
                        prop_expr = create_not_static_method_call(pn.get_function, (to as non_static_property_reference).expression, loc, false);
                        prop_expr.parameters.AddRange((to as non_static_property_reference).fact_parametres);
                    }
                    else
                    {
                        prop_expr = create_static_method_call(pn.get_function, loc, pn.comprehensive_type, false);
                        prop_expr.parameters.AddRange((to as static_property_reference).fact_parametres);
                    }
                    from = find_operator(ot, prop_expr, from, loc);
                }
            }
            else
            if (to is class_field_reference)
            {
                if ((to as class_field_reference).obj.type.type_special_kind == SemanticTree.type_special_kind.record &&
                    (to as class_field_reference).obj is base_function_call)
                {
                    //исключим ситуацию обращения к массиву
                    if (!(((to as class_field_reference).obj is common_method_call) &&
                    ((to as class_field_reference).obj as common_method_call).obj.type.type_special_kind == SemanticTree.type_special_kind.array_wrapper))
                        AddError(loc, "LEFT_SIDE_CANNOT_BE_ASSIGNED_TO");
                }
                //else check_field_reference_for_assign(to as class_field_reference,loc);
            }
            else if (context.is_in_cycle() && !SemanticRules.AllowChangeLoopVariable && to.semantic_node_type == semantic_node_type.namespace_variable_reference)
            {
                if (context.is_loop_variable((to as namespace_variable_reference).var))
                    AddError(to.location, "CANNOT_ASSIGN_TO_LOOP_VARIABLE");
            }
            else if (context.is_in_cycle() && !SemanticRules.AllowChangeLoopVariable && to.semantic_node_type == semantic_node_type.local_variable_reference)
            {
                if (context.is_loop_variable((to as local_variable_reference).var))
                    AddError(to.location, "CANNOT_ASSIGN_TO_LOOP_VARIABLE");
            }
            else if (context.is_in_cycle() && !SemanticRules.AllowChangeLoopVariable && to.semantic_node_type == semantic_node_type.local_block_variable_reference)
            {
                if (context.is_loop_variable((to as local_block_variable_reference).var))
                    AddError(to.location, "CANNOT_ASSIGN_TO_LOOP_VARIABLE");
            }
            else if (to is simple_array_indexing)
                if ((to as simple_array_indexing).simple_arr_expr is class_field_reference && ((to as simple_array_indexing).simple_arr_expr as class_field_reference).obj != null &&
                   ((to as simple_array_indexing).simple_arr_expr as class_field_reference).obj is constant_node)
                    AddError(loc, "LEFT_SIDE_CANNOT_BE_ASSIGNED_TO");
            if ((to.semantic_node_type == semantic_node_type.static_event_reference)
                    || (to.semantic_node_type == semantic_node_type.nonstatic_event_reference))
            {
                statement_node event_assign = null;
                if (_assign.operator_type == PascalABCCompiler.SyntaxTree.Operators.Assignment)
                {
                    //throw new CanNotAssignToEvent();
                }
                static_event_reference ser = (static_event_reference)to;
                expression_node right_del = convertion_data_and_alghoritms.convert_type(from, ser.en.delegate_type);
                switch (_assign.operator_type)
                {
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentAddition:
                        {
                            if (to.semantic_node_type == semantic_node_type.static_event_reference)
                            {
                                event_assign = convertion_data_and_alghoritms.create_simple_function_call(
                                    ser.en.add_method, loc, right_del);
                            }
                            else
                            {
                                if (ser.en.semantic_node_type == semantic_node_type.compiled_event)
                                {
                                    nonstatic_event_reference nser = (nonstatic_event_reference)ser;
                                    compiled_function_node cfn = (compiled_function_node)ser.en.add_method;
                                    compiled_function_call tmp_event_assign = new compiled_function_call(cfn, nser.obj, loc);
                                    tmp_event_assign.parameters.AddElement(right_del);
                                    event_assign = tmp_event_assign;
                                }
                                else if (ser.en.semantic_node_type == semantic_node_type.common_event)
                                {
                                    nonstatic_event_reference nser = (nonstatic_event_reference)ser;
                                    common_method_node cfn = (common_method_node)ser.en.add_method;
                                    common_method_call tmp_event_assign = new common_method_call(cfn, nser.obj, loc);
                                    tmp_event_assign.parameters.AddElement(right_del);
                                    event_assign = tmp_event_assign;
                                }
                            }
                            break;
                        }
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentSubtraction:
                        {
                            if (to.semantic_node_type == semantic_node_type.static_event_reference)
                            {
                                event_assign = convertion_data_and_alghoritms.create_simple_function_call(
                                    ser.en.remove_method, loc, right_del);
                            }
                            else
                            {
                                if (ser.en.semantic_node_type == semantic_node_type.compiled_event)
                                {
                                    nonstatic_event_reference nser = (nonstatic_event_reference)ser;
                                    compiled_function_node cfn = (compiled_function_node)ser.en.remove_method;
                                    compiled_function_call tmp_event_assign = new compiled_function_call(cfn, nser.obj, loc);
                                    tmp_event_assign.parameters.AddElement(right_del);
                                    event_assign = tmp_event_assign;
                                }
                                else if (ser.en.semantic_node_type == semantic_node_type.common_event)
                                {
                                    nonstatic_event_reference nser = (nonstatic_event_reference)ser;
                                    common_method_node cfn = (common_method_node)ser.en.remove_method;
                                    common_method_call tmp_event_assign = new common_method_call(cfn, nser.obj, loc);
                                    tmp_event_assign.parameters.AddElement(right_del);
                                    event_assign = tmp_event_assign;
                                }
                            }
                            break;
                        }
                    default:
                        {
                            AddError(loc, "ASSIGN_TO_EVENT");
                            //throw new CanNotApplyThisOperationToEvent

                            break;
                        }
                }
                return_value(event_assign);
                return;
            }

            if (_assign.operator_type == PascalABCCompiler.SyntaxTree.Operators.Assignment || oper_ass_in_prop)
            {
                if (to.semantic_node_type == semantic_node_type.static_property_reference)
                {
                    static_property_reference spr = (static_property_reference)to;
                    if (spr.property.set_function == null)
                    {
                        AddError(new ThisPropertyCanNotBeWrited(spr.property, loc));
                    }
                    check_property_params(spr, loc);
                    function_node set_func = spr.property.set_function;
                    from = convertion_data_and_alghoritms.convert_type(from, spr.property.property_type);
                    spr.fact_parametres.AddElement(from);
                    base_function_call bfc = create_static_method_call(set_func, loc, spr.property.comprehensive_type,
                        true);
                    bfc.parameters.AddRange(spr.fact_parametres);
                    return_value((statement_node)bfc);
                    return;
                }
                else if (to.semantic_node_type == semantic_node_type.non_static_property_reference)
                {
                    non_static_property_reference nspr = (non_static_property_reference)to;
                    check_property_params(nspr, loc);
                    from = convertion_data_and_alghoritms.convert_type(from, nspr.property.property_type);
                    nspr.fact_parametres.AddElement(from);

                    //Обработка s[i]:='c'
                    if (SystemUnitAssigned)
                        if (nspr.property.comprehensive_type == SystemLibrary.SystemLibrary.string_type)
                        {
                            if (nspr.property == SystemLibrary.SystemLibrary.string_type.default_property_node)
                            {
                                if (SystemLibrary.SystemLibInitializer.StringDefaultPropertySetProcedure != null)
                                {
                                    expressions_list exl = new expressions_list();
                                    exl.AddElement(nspr.expression);
                                    exl.AddElement(nspr.fact_parametres[0]);
                                    exl.AddElement(from);
                                    function_node fn = convertion_data_and_alghoritms.select_function(exl, SystemLibrary.SystemLibInitializer.StringDefaultPropertySetProcedure.SymbolInfo, loc);
                                    expression_node ret = convertion_data_and_alghoritms.create_simple_function_call(fn, loc, exl.ToArray());
                                    return_value((statement_node)ret);
                                    return;
                                }
                            }
                        }

                    if (nspr.property.set_function == null)
                    {
                        AddError(new ThisPropertyCanNotBeWrited(nspr.property, loc));
                    }
                    function_node set_func = nspr.property.set_function;
                    base_function_call bfc = create_not_static_method_call(set_func, nspr.expression, loc,
                        true);
                    bfc.parameters.AddRange(nspr.fact_parametres);
                    return_value((statement_node)bfc);
                    return;
                }
                else if (to is simple_array_indexing && (to as simple_array_indexing).simple_arr_expr.type.type_special_kind == SemanticTree.type_special_kind.short_string)
                {
                    expression_node expr = (to as simple_array_indexing).simple_arr_expr;
                    expression_node ind_expr = (to as simple_array_indexing).ind_expr;
                    from = convertion_data_and_alghoritms.convert_type(from, SystemLibrary.SystemLibrary.char_type);
                    ind_expr = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.SetCharInShortStringProcedure.sym_info as function_node, loc, expr, ind_expr, new int_const_node((expr.type as short_string_type_node).Length, null), from);
                    return_value(find_operator(compiler_string_consts.assign_name, expr, ind_expr, get_location(_assign)));
                    return;
                }
                else if (to.type.type_special_kind == SemanticTree.type_special_kind.short_string)
                {
                    if (from.type is null_type_node)
                        AddError(get_location(_assign), "NIL_WITH_VALUE_TYPES_NOT_ALLOWED");
                    expression_node clip_expr = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info as function_node, loc, convertion_data_and_alghoritms.convert_type(from, SystemLibrary.SystemLibrary.string_type), new int_const_node((to.type as short_string_type_node).Length, null));
                    statement_node en = find_operator(compiler_string_consts.assign_name, to, clip_expr, get_location(_assign));
                    return_value(en);
                    return;
                }
                else
                {

                    assign_is_converting = true;
                    statement_node en = find_operator(compiler_string_consts.assign_name, to, from, get_location(_assign));
                    assign_is_converting = false;
                    return_value(en);
                    return;
                }
            }
            else
            {
                assign_is_converting = true;
                statement_node en = find_operator(_assign.operator_type, to, from, get_location(_assign));
                assign_is_converting = false;
                return_value(en);
                return;
            }
            //throw new CompilerInternalError("Undefined assign to type");
        }


    }
}
