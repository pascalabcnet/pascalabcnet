using PascalABCCompiler.SemanticTree;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeRealization;
using TreeConverter.LambdaExpressions;
using for_node = PascalABCCompiler.SyntaxTree.for_node;

namespace PascalABCCompiler.TreeConverter
{
    public partial class syntax_tree_visitor // SSM 02.01.17 менять на визитор с другим порядком обхода можно, но бессмысленно
    {



        public override void visit(foreach_stmt _foreach_stmt)
        {
            statements_list sl2 = new statements_list(get_location(_foreach_stmt));            
            convertion_data_and_alghoritms.statement_list_stack_push(sl2);

            expression_node foreachCollection;
            var_definition_node foreachVariable;
            ForeachCheckAndConvert(_foreach_stmt, out foreachCollection, out foreachVariable);

            // SSM 29.07.16 - если in_what - одномерный массив, то заменить код foreach на for
            // if (OptimizeForeachInCase1DArray(_foreach_stmt, foreachCollection)) return;

            statements_list sl = new statements_list(get_location(_foreach_stmt.stmt));
            convertion_data_and_alghoritms.statement_list_stack_push(sl);

            foreach_node foreachNode = new foreach_node(foreachVariable, foreachCollection, null, get_location(_foreach_stmt));

            context.cycle_stack.push(foreachNode);
            context.loop_var_stack.Push(foreachVariable);
            context.enter_code_block_with_bind();
            statement_node body = convert_strong(_foreach_stmt.stmt);
            context.leave_code_block();
            context.loop_var_stack.Pop();
            context.cycle_stack.pop();

            sl = convertion_data_and_alghoritms.statement_list_stack.pop();

            if (sl.statements.Count > 0 || sl.local_variables.Count > 0)
            {
                sl.statements.AddElement(body);
                body = sl;
            }

            foreachNode.what_do = body;

            convertion_data_and_alghoritms.statement_list_stack.pop();
            sl2.statements.AddElement(foreachNode);
            
            return_value(sl2);
        }



        private void ForeachCheckAndConvert(foreach_stmt _foreach_stmt, out expression_node foreachCollection,
            out var_definition_node foreachVariable)
        {
            var lambdaSearcher = new LambdaSearcher(_foreach_stmt.in_what);
            if (lambdaSearcher.CheckIfContainsLambdas())
            {
                AddError(new LambdasNotAllowedInForeachInWhatSatetement(get_location(lambdaSearcher.FoundLambda)));
            }

            foreachCollection = convert_strong(_foreach_stmt.in_what);

            expression_node tmp = convert_if_typed_expression_to_function_call(foreachCollection);
            if (tmp.type != null)
                foreachCollection = tmp;

            bool sys_coll_ienum; // true означает, что мы нашли тип object у интерфейса System.Collections.IEnumerable
            type_node elem_type = null;
            if (!FindIEnumerableElementType(foreachCollection.type, ref elem_type, out sys_coll_ienum))
                AddError(foreachCollection.location, "CAN_NOT_EXECUTE_FOREACH_BY_EXPR_OF_TYPE_{0}", foreachCollection.type.name);

            CheckToEmbeddedStatementCannotBeADeclaration(_foreach_stmt.stmt);

            foreachVariable = FindForeachVariable(_foreach_stmt, elem_type, sys_coll_ienum);
        }


        
        private var_definition_node FindForeachVariable(foreach_stmt _foreach_stmt, type_node elem_type, bool sys_coll_ienum)
        {
            var_definition_node foreachVariable = null;
            if (_foreach_stmt.type_name == null) // значит, переменная определена в другом месте
            {
                location loc1 = get_location(_foreach_stmt.identifier);
                definition_node dn = context.check_name_node_type(_foreach_stmt.identifier.name, loc1,
                    general_node_type.variable_node);
                foreachVariable = (var_definition_node) dn;
                if (!check_name_in_current_scope(_foreach_stmt.identifier.name))
                    AddError(loc1, "FOREACH_LOOP_CONTROL_MUST_BE_SIMPLE_LOCAL_VARIABLE");
            }
            else
            {
                foreachVariable = context.add_var_definition(_foreach_stmt.identifier.name,
                    get_location(_foreach_stmt.identifier));

                type_node tn;
                if (_foreach_stmt.type_name is no_type_foreach) // значит, это for var x in a
                {
                    tn = elem_type;
                }
                else // значит, это for var x: T in a
                {
                    tn = convert_strong(_foreach_stmt.type_name);
                    check_for_type_allowed(tn, get_location(_foreach_stmt.type_name));
                }

                context.close_var_definition_list(tn, null);
            }

            if (!(foreachVariable.type is compiled_generic_instance_type_node) && !sys_coll_ienum)
                convertion_data_and_alghoritms.check_convert_type_with_inheritance(elem_type, foreachVariable.type,
                    get_location(_foreach_stmt.identifier));
            return foreachVariable;
        }



        /// <summary>
        /// Преобразует foreach в for, если коллекция это одномерный массив.
        /// </summary>
        /// <param name="_foreach_stmt"></param>
        /// <param name="in_what"></param>
        /// <returns>True - если преобразование удалось, иначе False</returns>
        private bool OptimizeForeachInCase1DArray(foreach_stmt _foreach_stmt, expression_node in_what)
        {
            var is1dimdynarr = false;
            var comptn = in_what.type as compiled_type_node;
            if (comptn != null && comptn.type_special_kind == type_special_kind.array_kind && comptn.rank == 1)
            {
                is1dimdynarr = true;
            }
            if (!is1dimdynarr)
            {
                var comtn = in_what.type as common_type_node;
                if (comtn != null && comtn.internal_type_special_kind == type_special_kind.array_kind &&
                    comtn.rank == 1)
                {
                    is1dimdynarr = true;
                }
            }


            // SSM 23.08.16 Закомментировал оптимизацию. Не работает с лямбдами. Лямбды обходят старое дерево. А заменить foreach на for на этом этапе пока не получается - не развита инфраструктура

            if (is1dimdynarr) // Замена foreach на for для массива
            {
                // сгенерировать код для for и вызвать соответствующий visit
                var arrid = GenIdentName();
                //var vdarr = new var_statement(arrid, new semantic_addr_value(in_what)); // semantic_addr_value - перевод в синтаксис для мгновенного вычисления семантического выражения, которое уже вычислено в in_what
                var vdarr = new var_statement(arrid, _foreach_stmt.in_what);

                var i = GenIdentName();
                var x = _foreach_stmt.identifier;

                // Возможны 3 случая:
                // 1. _foreach_stmt.type_name = null - значит, переменная определена в другом месте
                // 2. _foreach_stmt.type_name = no_type_foreach - значит, это for var x in a
                // 3. _foreach_stmt.type_name = T - значит, это for var x: T in a

                statement vd;
                if (_foreach_stmt.type_name == null) // 1.
                    vd = new assign(x, arrid.indexer(i));
                else if (_foreach_stmt.type_name is no_type_foreach) // 2.
                    vd = new var_statement(x, arrid.indexer(i));
                else // 3.
                    vd = new var_statement(x, _foreach_stmt.type_name, arrid.indexer(i));

                // Превратить старое тело в statement_list и добавить к нему в начало x := a[i] или var x := a[i] 
                var newbody = _foreach_stmt.stmt.ToStatementList();
                newbody.AddFirst(vd);

                var high = arrid.dot_node("Length").Minus(1);

                var fornode = new for_node(i, 0, high, newbody, for_cycle_type.to, null, null, true);

                var stl = new statement_list(vdarr, fornode);
                // Замена 1 оператора на 1 оператор. Всё хорошо даже если оператор помечен меткой
                ReplaceUsingParent(_foreach_stmt, stl);

                visit(stl);
                //visit(vdarr);
                //visit(fornode);

                return true;
            }
            /// SSM 29.07.16 
            return false;
        }
    }
}
