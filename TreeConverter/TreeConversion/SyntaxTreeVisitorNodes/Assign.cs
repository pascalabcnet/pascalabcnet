using PascalABCCompiler.SemanticTree;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SystemLibrary;
using PascalABCCompiler.TreeRealization;
using SymTable = SymbolTable;

namespace PascalABCCompiler.TreeConverter
{
    public partial class syntax_tree_visitor
    {



        public bool internal_is_assign;



        public override void visit(assign _assign)
        {
            var loc = get_location(_assign);


            addressed_expression to;
            expression_node from;
            AssignCheckAndConvert(_assign, out to, out from);


            InferLambdaResultTypeFromAssignment(_assign, from, to);

            if (ProcessAssignToPropertyIfPossible(_assign, to, loc, from))
                return;
            if (ProcessAssignmentToEventIfPossible(_assign, to, from, loc))
                return;
            if (ProcessAssignmentToShortStringIfPossible(_assign, to, from, loc))
                return;
            
            
            assign_is_converting = true;
            statement_node en = find_operator(_assign.operator_type, to, from, get_location(_assign));
            assign_is_converting = false;
            return_value(en);
        }



        /// <summary>
        /// Выводит тип результата лямбды по первому присваиванию переменной Result.
        /// </summary>
        private void InferLambdaResultTypeFromAssignment(assign _assign, expression_node from, addressed_expression to)
        {
            if (stflambda.Count > 0) // мы находимся внутри лямбды - возможно, вложенной
            {
                var fld = stflambda.Peek();
                if (_assign.to is ident && (_assign.to as ident).name.ToLower() == "result" && fld.RealSemTypeOfResExpr == null)
                    // если это - первое присваивание Result
                {
                    fld.RealSemTypeOfResExpr = from.type;
                    fld.RealSemTypeOfResult = to.type;
                }
            }
        }

        

        /// <summary>
        /// Обрабатывает случай, когда левая часть присваивания short string.
        /// </summary>
        /// <returns>True - обработка прошла, иначе False.</returns>
        private bool ProcessAssignmentToShortStringIfPossible(assign _assign, addressed_expression to, expression_node from, location loc)
        {
            if (_assign.operator_type == Operators.Assignment)
            {
                if (to is simple_array_indexing &&
                    (to as simple_array_indexing).simple_arr_expr.type.type_special_kind == type_special_kind.short_string)
                {
                    expression_node expr = (to as simple_array_indexing).simple_arr_expr;
                    expression_node ind_expr = (to as simple_array_indexing).ind_expr;
                    from = convertion_data_and_alghoritms.convert_type(from, SystemLibrary.SystemLibrary.char_type);
                    ind_expr = convertion_data_and_alghoritms.create_simple_function_call(
                        SystemLibInitializer.SetCharInShortStringProcedure.sym_info as function_node,
                        loc,
                        expr,
                        ind_expr,
                        new int_const_node((expr.type as short_string_type_node).Length, null), from);
                    return_value(find_operator(compiler_string_consts.assign_name, expr, ind_expr, get_location(_assign)));
                    return true;
                }
                if (to.type.type_special_kind == type_special_kind.short_string)
                {
                    if (from.type is null_type_node)
                        AddError(get_location(_assign), "NIL_WITH_VALUE_TYPES_NOT_ALLOWED");
                    expression_node clip_expr = convertion_data_and_alghoritms.create_simple_function_call(
                        SystemLibInitializer.ClipShortStringProcedure.sym_info as function_node,
                        loc,
                        convertion_data_and_alghoritms.convert_type(from, SystemLibrary.SystemLibrary.string_type),
                        new int_const_node((to.type as short_string_type_node).Length,
                            null));
                    statement_node en = find_operator(compiler_string_consts.assign_name, to, clip_expr, get_location(_assign));
                    return_value(en);
                    return true;
                }
            }
            return false;
        }
        


        /// <summary>
        /// Преобразует в семантическое представление поля to и from, проводя семантические проверки.
        /// </summary>
        private void AssignCheckAndConvert(assign _assign, out addressed_expression to, out expression_node from)
        {
            internal_is_assign = true;
            to = convert_address_strong(_assign.to);
            internal_is_assign = false;
            if (to == null)
                AddError(get_location(_assign.to), "CAN_NOT_ASSIGN_TO_LEFT_PART");

            //(ssyy) Вставляю проверки прямо сюда, т.к. запарился вылавливать другие случаи.
            bool flag;
            general_node_type node_type;
            if (convertion_data_and_alghoritms.check_for_constant_or_readonly(to, out flag, out node_type))
            {
                if (flag)
                    AddError(to.location, "CAN_NOT_ASSIGN_TO_CONSTANT_OBJECT");
                else
                    AddError(new CanNotAssignToReadOnlyElement(to.location, node_type));
            }

            // SSM исправление Саушкина 10.03.16
            var fromAsLambda = _assign.from as function_lambda_definition;
            if (fromAsLambda != null)
            {
                #region Вывод параметров лямбда-выражения

                LambdaHelper.InferTypesFromVarStmt(to.type, fromAsLambda, this); //lroman//

                #endregion

                var lambdaVisitMode = fromAsLambda.lambda_visit_mode;
                fromAsLambda.lambda_visit_mode = LambdaVisitMode.VisitForAdvancedMethodCallProcessing;
                from = convert_strong(_assign.from);
                fromAsLambda.lambda_visit_mode = lambdaVisitMode;
            }
            else
            {
                from = convert_strong(_assign.from);
                ProcessAssigntToAutoType(to, ref from);
            }
            // end

            //SSM 4.04.16
            if (to.type is undefined_type)
                to.type = from.type;

            location loc = get_location(_assign);

            if (to is class_field_reference)
            {
                var classFieldRef = to as class_field_reference;
                if (classFieldRef.obj.type.type_special_kind == type_special_kind.record &&
                    classFieldRef.obj is base_function_call)
                {
                    //исключим ситуацию обращения к массиву
                    if (!(classFieldRef.obj is common_method_call &&
                          (classFieldRef.obj as common_method_call).obj.type.type_special_kind ==
                          type_special_kind.array_wrapper))
                        AddError(loc, "LEFT_SIDE_CANNOT_BE_ASSIGNED_TO");
                }
                //else check_field_reference_for_assign(to as class_field_reference,loc);
            }
            if (context.is_in_cycle() && !SemanticRules.AllowChangeLoopVariable)
            {
                var_definition_node toAsVariable = GetLocalVariableFromAdressExpressionIfPossible(to);
                if (toAsVariable != null && context.is_loop_variable(toAsVariable))
                    AddError(to.location, "CANNOT_ASSIGN_TO_LOOP_VARIABLE");
            }
            {
                var classFieldRef = (to as simple_array_indexing)?.simple_arr_expr as class_field_reference;
                if (classFieldRef?.obj is constant_node)
                    AddError(loc, "LEFT_SIDE_CANNOT_BE_ASSIGNED_TO");
            }
        }



        /// <summary>
        /// Автовыведение типов в yield'ax.
        /// </summary>
        private void ProcessAssigntToAutoType(addressed_expression to, ref expression_node from)
        {
            var sequence = to.type as compiled_generic_instance_type_node;
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
            else if (sequence?.instance_params[0] is ienumerable_auto_type)
            {
                type_node elem_type = null;
                try_convert_typed_expression_to_function_call(ref from);
                bool bb; // здесь bb не нужно. Оно нужно в foreach
                var b = FindIEnumerableElementType(from.type, ref elem_type, out bb);
                if (!b)
                    AddError(from.location, "CAN_NOT_EXECUTE_FOREACH_BY_EXPR_OF_TYPE_{0}", from.type.full_name);

                var IEnumType = new template_type_reference(new named_type_reference("System.Collections.Generic.IEnumerable"),
                    new template_param_list(new semantic_type_node(elem_type)));
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
                    lvr.type = lvr.var.type; // замена типа у переменной
                }
            }
        }



        /// <summary>
        /// Обрабатывает случай, когда левая часть присваивания свойство.
        /// </summary>
        /// <returns>True - обработка прошла, иначе False.</returns>
        private bool ProcessAssignToPropertyIfPossible(assign _assign, addressed_expression to, location loc,
            expression_node from)
        {
            //проверка на обращение к полю записи возвращенной из функции с целью присваивания
            //нужно чтобы пользователь не мог менять временный обьект
            if (to.semantic_node_type == semantic_node_type.static_property_reference ||
                to.semantic_node_type == semantic_node_type.non_static_property_reference)
            {
                property_node pn;
                if (to.semantic_node_type == semantic_node_type.static_property_reference)
                    pn = (to as static_property_reference).property;
                else
                    pn = (to as non_static_property_reference).property;

                var ot = MapCompositeAssignmentOperatorToSameBinaryOperator(_assign);
                var oper_ass_in_prop = ot != Operators.Undefined;

                if (_assign.operator_type == Operators.Assignment || oper_ass_in_prop)
                {
                    if (oper_ass_in_prop)
                    {
                        if (pn.get_function == null)
                            AddError(loc, "THIS_PROPERTY_{0}_CAN_NOT_BE_READED", pn.name);
                        base_function_call prop_expr;
                        if (to.semantic_node_type == semantic_node_type.non_static_property_reference)
                        {
                            prop_expr = create_not_static_method_call(pn.get_function,
                                (to as non_static_property_reference).expression, loc, false);
                            prop_expr.parameters.AddRange((to as non_static_property_reference).fact_parametres);
                        }
                        else
                        {
                            prop_expr = create_static_method_call(pn.get_function, loc, pn.comprehensive_type, false);
                            prop_expr.parameters.AddRange((to as static_property_reference).fact_parametres);
                        }
                        from = find_operator(ot, prop_expr, from, loc);
                    }

                    if (to.semantic_node_type == semantic_node_type.static_property_reference)
                    {
                        static_property_reference spr = (static_property_reference) to;
                        if (spr.property.set_function == null)
                        {
                            AddError(loc, "THIS_PROPERTY_{0}_CAN_NOT_BE_WRITED", spr.property.name);
                        }
                        check_property_params(spr, loc);
                        function_node set_func = spr.property.set_function;
                        from = convertion_data_and_alghoritms.convert_type(from, spr.property.property_type);
                        spr.fact_parametres.AddElement(from);
                        base_function_call bfc = create_static_method_call(set_func, loc,
                            spr.property.comprehensive_type,
                            true);
                        bfc.parameters.AddRange(spr.fact_parametres);
                        return_value((statement_node) bfc);
                    }
                    else if (to.semantic_node_type == semantic_node_type.non_static_property_reference)
                    {
                        non_static_property_reference nspr = (non_static_property_reference) to;
                        check_property_params(nspr, loc);
                        from = convertion_data_and_alghoritms.convert_type(from, nspr.property.property_type);
                        nspr.fact_parametres.AddElement(from);

                        //Обработка s[i]:='c'
                        if (SystemUnitAssigned)
                            if (nspr.property.comprehensive_type == SystemLibrary.SystemLibrary.string_type)
                            {
                                if (nspr.property == SystemLibrary.SystemLibrary.string_type.default_property_node)
                                {
                                    if (SystemLibInitializer.StringDefaultPropertySetProcedure != null)
                                    {
                                        expressions_list exl = new expressions_list();
                                        exl.AddElement(nspr.expression);
                                        exl.AddElement(nspr.fact_parametres[0]);
                                        exl.AddElement(from);
                                        function_node fn = convertion_data_and_alghoritms.select_function(exl,
                                            SystemLibInitializer.StringDefaultPropertySetProcedure
                                                .SymbolInfo, loc);
                                        expression_node ret =
                                            convertion_data_and_alghoritms.create_simple_function_call(fn, loc,
                                                exl.ToArray());
                                        return_value((statement_node) ret);
                                        return true;
                                    }
                                }
                            }

                        if (nspr.property.set_function == null)
                        {
                            AddError(loc, "THIS_PROPERTY_{0}_CAN_NOT_BE_WRITED", nspr.property.name);
                        }
                        function_node set_func = nspr.property.set_function;
                        base_function_call bfc = create_not_static_method_call(set_func, nspr.expression, loc,
                            true);
                        bfc.parameters.AddRange(nspr.fact_parametres);
                        return_value((statement_node) bfc);
                    }
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// Если addressedExpression это ссылка на переменную, то возвращает узел этой переменной.
        /// Например, для namespace_variable_reference вернёт (addressedExpression as namespace_variable_reference).var
        /// </summary>
        /// <returns>Возвращает узел этой переменной, иначе null</returns>
        private static var_definition_node GetLocalVariableFromAdressExpressionIfPossible(addressed_expression addressedExpression)
        {
            switch (addressedExpression.semantic_node_type)
            {
                case semantic_node_type.namespace_variable_reference:
                    return (addressedExpression as namespace_variable_reference).var;
                case semantic_node_type.local_variable_reference:
                    return (addressedExpression as local_variable_reference).var;
                case semantic_node_type.local_block_variable_reference:
                    return (addressedExpression as local_block_variable_reference).var;
                default:
                    return null;
            }
        }



        /// <summary>
        /// Сопоставляет составному присваиванию его аналог.
        /// Оператору '+=' возвращает '+', для '-=' возвращает '-', и т.д.
        /// </summary>
        /// <returns>Возвращает оператор. Если было передано не присваивание возвращает Operators.Undefined</returns>
        private static Operators MapCompositeAssignmentOperatorToSameBinaryOperator(assign _assign)
        {
            switch (_assign.operator_type)
            {
                case Operators.AssignmentAddition:
                    return Operators.Plus;
                case Operators.AssignmentBitwiseAND:
                    return Operators.BitwiseAND;
                case Operators.AssignmentBitwiseLeftShift:
                    return Operators.BitwiseLeftShift;
                case Operators.AssignmentBitwiseOR:
                    return Operators.BitwiseOR;
                case Operators.AssignmentBitwiseRightShift:
                    return Operators.BitwiseRightShift;
                case Operators.AssignmentBitwiseXOR:
                    return Operators.BitwiseXOR;
                case Operators.AssignmentDivision:
                    return Operators.Division;
                case Operators.AssignmentModulus:
                    return Operators.ModulusRemainder;
                case Operators.AssignmentMultiplication:
                    return Operators.Multiplication;
                case Operators.AssignmentSubtraction:
                    return Operators.Minus;
                default:
                    return Operators.Undefined;
            }
        }



        /// <summary>
        /// Обрабатывает случай, когда левая часть присваивания имеет тип event.
        /// </summary>
        /// <returns>True - обработка прошла, иначе False.</returns>
        private bool ProcessAssignmentToEventIfPossible(assign _assign, addressed_expression to, expression_node from,
            location loc)
        {
            if ((to.semantic_node_type == semantic_node_type.static_event_reference)
                || (to.semantic_node_type == semantic_node_type.nonstatic_event_reference))
            {
                statement_node event_assign = null;
                static_event_reference ser = (static_event_reference) to;
                expression_node right_del = convertion_data_and_alghoritms.convert_type(from, ser.en.delegate_type);
                switch (_assign.operator_type)
                {
                    case Operators.AssignmentAddition:
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
                                nonstatic_event_reference nser = (nonstatic_event_reference) ser;
                                compiled_function_node cfn = (compiled_function_node) ser.en.add_method;
                                compiled_function_call tmp_event_assign = new compiled_function_call(cfn, nser.obj, loc);
                                tmp_event_assign.parameters.AddElement(right_del);
                                event_assign = tmp_event_assign;
                            }
                            else if (ser.en.semantic_node_type == semantic_node_type.common_event)
                            {
                                nonstatic_event_reference nser = (nonstatic_event_reference) ser;
                                common_method_node cfn = (common_method_node) ser.en.add_method;
                                common_method_call tmp_event_assign = new common_method_call(cfn, nser.obj, loc);
                                tmp_event_assign.parameters.AddElement(right_del);
                                event_assign = tmp_event_assign;
                            }
                        }
                        break;
                    }
                    case Operators.AssignmentSubtraction:
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
                                nonstatic_event_reference nser = (nonstatic_event_reference) ser;
                                compiled_function_node cfn = (compiled_function_node) ser.en.remove_method;
                                compiled_function_call tmp_event_assign = new compiled_function_call(cfn, nser.obj, loc);
                                tmp_event_assign.parameters.AddElement(right_del);
                                event_assign = tmp_event_assign;
                            }
                            else if (ser.en.semantic_node_type == semantic_node_type.common_event)
                            {
                                nonstatic_event_reference nser = (nonstatic_event_reference) ser;
                                common_method_node cfn = (common_method_node) ser.en.remove_method;
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
                return true;
            }
            return false;
        }
    }
}
