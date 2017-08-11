using System;
using PascalABCCompiler.SemanticTree;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeRealization;
using for_node = PascalABCCompiler.SyntaxTree.for_node;

namespace PascalABCCompiler.TreeConverter
{
    public partial class syntax_tree_visitor
    {
        public override void visit(for_node _for_node)
        {
            #region MikhailoMMX, обработка omp parallel for
            bool isGenerateParallel = false;
            bool isGenerateSequential = true;
            if (OpenMP.ForsFound)
            {
                OpenMP.LoopVariables.Push(_for_node.loop_variable.name.ToLower());
                //если в программе есть хоть одна директива parallel for - проверяем:
                if (DirectivesToNodesLinks.ContainsKey(_for_node) && OpenMP.IsParallelForDirective(DirectivesToNodesLinks[_for_node]))
                {
                    //перед этим узлом есть директива parallel for
                    if (CurrentParallelPosition == ParallelPosition.Outside)            //входим в самый внешний параллельный for
                    {
                        if (_for_node.create_loop_variable || (_for_node.type_name != null))
                        {
                            //сгенерировать сначала последовательную ветку, затем параллельную
                            //устанавливаем флаг и продолжаем конвертирование, считая, что конвертируем последовательную ветку
                            isGenerateParallel = true;
                            CurrentParallelPosition = ParallelPosition.InsideSequential;
                            //в конце за счет флага вернем состояние обратно и сгенерируем и параллельную ветку тоже
                        }
                        else
                            WarningsList.Add(new OMP_BuildigError(new Exception("Переменная параллельного цикла должна быть определена в заголовке цикла"), new location(_for_node.source_context.begin_position.line_num, _for_node.source_context.begin_position.column_num, _for_node.source_context.end_position.line_num, _for_node.source_context.end_position.column_num, new document(_for_node.source_context.FileName))));
                    }
                    else //уже генерируем одну из веток
                        //если это параллельная ветка - последовательную генерировать не будем
                    if (CurrentParallelPosition == ParallelPosition.InsideParallel)
                    {
                        isGenerateParallel = true;
                        isGenerateSequential = false;
                    }
                    //else
                    //а если последовательная - то флаг isGenerateParallel не установлен, сгенерируется только последовательная
                }
            }
            #endregion


            location loopVariableLocation = get_location(_for_node.loop_variable);
            var_definition_node vdn = null;
            expression_node left, right, res;
            expression_node initialValue = convert_strong(_for_node.initial_value);
            expression_node tmp = initialValue;
            if (initialValue is typed_expression) initialValue = convert_typed_expression_to_function_call(initialValue as typed_expression);
            if (initialValue.type == null)
                initialValue = tmp;
            statements_list head_stmts = new statements_list(loopVariableLocation);
            convertion_data_and_alghoritms.statement_list_stack_push(head_stmts);
            if (_for_node.type_name == null && !_for_node.create_loop_variable)
            {
                definition_node dn = context.check_name_node_type(_for_node.loop_variable.name, loopVariableLocation,
                    general_node_type.variable_node);
                vdn = (var_definition_node)dn;
                if (context.is_loop_variable(vdn))
                    AddError(get_location(_for_node.loop_variable), "CANNOT_ASSIGN_TO_LOOP_VARIABLE");
                if (!check_name_in_current_scope(_for_node.loop_variable.name))
                    AddError(new ForLoopControlMustBeSimpleLocalVariable(loopVariableLocation));
            }
            else
            {
                //В разработке DS
                //throw new NotSupportedError(get_location(_for_node.type_name));
                type_node tn;
                if (_for_node.type_name != null)
                    tn = convert_strong(_for_node.type_name);
                else
                    tn = initialValue.type;
                //if (tn == SystemLibrary.SystemLibrary.void_type && _for_node.type_name != null)
                //	AddError(new VoidNotValid(get_location(_for_node.type_name)))
                if (_for_node.type_name != null)
                    check_for_type_allowed(tn, get_location(_for_node.type_name));
                vdn = context.add_var_definition(_for_node.loop_variable.name, get_location(_for_node.loop_variable), tn, polymorphic_state.ps_common);
            }
            internal_interface ii = vdn.type.get_internal_interface(internal_interface_kind.ordinal_interface);
            if (ii == null)
            {
                AddError(new OrdinalTypeExpected(loopVariableLocation));
            }
            ordinal_type_interface oti = (ordinal_type_interface)ii;


            location finishValueLocation = get_location(_for_node.finish_value);
            var_definition_node vdn_finish = context.create_for_temp_variable(vdn.type, finishValueLocation);
            //Это должно стаять первее!
            left = create_variable_reference(vdn_finish, loopVariableLocation);
            expression_node finishValue = convert_strong(_for_node.finish_value);
            right = finishValue;
            if (right is typed_expression)
                right = convert_typed_expression_to_function_call(right as typed_expression);
            res = find_operator(compiler_string_consts.assign_name, left, right, finishValueLocation);
            head_stmts.statements.AddElement(res);

            left = create_variable_reference(vdn, loopVariableLocation);
            right = initialValue;
            res = find_operator(compiler_string_consts.assign_name, left, right, loopVariableLocation);
            head_stmts.statements.AddElement(res);
            

            location initialValueLocation = get_location(_for_node.initial_value);

            statement_node sn_inc = null;
            expression_node sn_while = null;
            expression_node sn_init_while = null;
            left = create_variable_reference(vdn, initialValueLocation);
            right = create_variable_reference(vdn, finishValueLocation);
            expression_node right_border = create_variable_reference(vdn_finish, finishValueLocation);
            switch (_for_node.cycle_type)
            {
                case for_cycle_type.to:
                {
                    sn_inc =
                        convertion_data_and_alghoritms.create_simple_function_call(oti.inc_method, loopVariableLocation,
                            left);
                    sn_while = convertion_data_and_alghoritms.create_simple_function_call(oti.lower_method,
                        finishValueLocation, right, right_border);
                    sn_init_while = convertion_data_and_alghoritms.create_simple_function_call(oti.lower_eq_method,
                        finishValueLocation, right, right_border);
                    break;
                }
                case for_cycle_type.downto:
                {
                    sn_inc =
                        convertion_data_and_alghoritms.create_simple_function_call(oti.dec_method, loopVariableLocation,
                            left);
                    sn_while = convertion_data_and_alghoritms.create_simple_function_call(oti.greater_method,
                        finishValueLocation, right, right_border);
                    sn_init_while = convertion_data_and_alghoritms.create_simple_function_call(oti.greater_eq_method,
                        finishValueLocation, right, right_border);
                    break;
                }
            }

            CheckToEmbeddedStatementCannotBeADeclaration(_for_node.statements);

            //DarkStar Modifed
            //исправил ошибку:  не работали break в циклах
            TreeRealization.for_node forNode = new TreeRealization.for_node(null, sn_while, sn_init_while, sn_inc, null, get_location(_for_node));
            if (vdn.type == SystemLibrary.SystemLibrary.bool_type)
                forNode.bool_cycle = true;
            context.cycle_stack.push(forNode);
            context.loop_var_stack.Push(vdn);
            statements_list slst = new statements_list(get_location(_for_node.statements));
            convertion_data_and_alghoritms.statement_list_stack_push(slst);

            context.enter_code_block_with_bind();
            forNode.body = convert_strong(_for_node.statements);
            context.leave_code_block();

            slst = convertion_data_and_alghoritms.statement_list_stack.pop();
            if (slst.statements.Count > 0 || slst.local_variables.Count > 0)
            {
                slst.statements.AddElement(forNode.body);
                forNode.body = slst;
            }

            context.cycle_stack.pop();
            context.loop_var_stack.Pop();
            head_stmts = convertion_data_and_alghoritms.statement_list_stack.pop();
            head_stmts.statements.AddElement(forNode);

            #region MikhailoMMX, обработка omp parallel for
            //флаг был установлен только если это самый внешний parallel for и нужно сгенерировать обе ветки
            //или если это вложенный parallel for, нужно сгенерировать обе ветки, но без проверки на OMP_Available
            //Последовательная ветка только что сгенерирована, теперь меняем состояние и генерируем параллельную
            if (isGenerateParallel)
            {
                CurrentParallelPosition = ParallelPosition.InsideParallel;
                statements_list stl = OpenMP.TryConvertFor(head_stmts, _for_node, forNode, vdn, initialValue, finishValue, this);
                CurrentParallelPosition = ParallelPosition.Outside;
                if (stl != null)
                {
                    OpenMP.LoopVariables.Pop();
                    return_value(stl);
                    return;
                }
            }
            if (OpenMP.ForsFound)
            {
                OpenMP.LoopVariables.Pop();
            }
            #endregion

            return_value(head_stmts);
        }
    }
}
