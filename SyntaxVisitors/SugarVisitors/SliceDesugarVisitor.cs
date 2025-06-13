// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.SugarVisitors
{
    public class SliceDesugarVisitor : BaseChangeVisitor
    {
        public static SliceDesugarVisitor New
        {
            get { return new SliceDesugarVisitor(); }
        }

        expression_list construct_expression_list_for_slice_expr_multi(slice_expr sl)
        {
            if (sl.slices == null)
                return null; // упадёт - это ошибка компилятора
            var el = new expression_list(); // будем наполнять кортежами - самое простое
            var sc = sl.source_context;
            foreach (var slice in sl.slices)
            {
                var tup = new dot_node(new dot_node(new ident("?System",sc), new ident("Tuple",sc), sc), new ident(PascalABCCompiler.StringConstants.default_constructor_name,sc), sc);
                var eel = new expression_list();
                // пытаемся разобраться с ^1

                var sl1 = slice.Item1;
                eel.Add(sl1);
                IndexVisitor.New.ProcessNode(sl1); // индексный визитор сам не вызывается поскольку в многомерных срезах хранится List кортежей троек expression, который сам не обходится
                var sl2 = slice.Item2;
                eel.Add(sl2);
                IndexVisitor.New.ProcessNode(sl2);
                var sl3 = slice.Item3; // и step тоже надо обходить!!!
                eel.Add(sl3);
                IndexVisitor.New.ProcessNode(sl3);

                var mc = new method_call(tup, eel, sc); // sc - не очень хорошо - ошибка будет в общем месте
                el.Add(mc);
                // по идее все параметры готовы. Надо только проверить, что они целые
            }
            return el;
        }
        expression_list construct_expression_list_for_slice_expr(slice_expr sl)
        {
            // situation = 0 - ничего не пропущено
            // situation = 1 - пропущен from
            // situation = 2 - пропущен to
            // situation = 3 - пропущены from и to
            // Пропущенность кодируется тем, что в соответствующем поле - int.MaxValue
            // step может просто отсутствовать - это параметр по умолчанию в SystemSlice

            int situation = 0;
            var fromInverted = false;
            var fromExpr = sl.from;

            if (sl.from is index fromInd)
            {
                fromInverted = fromInd.inverted; // странно, но не используется
                fromExpr = fromInd.index_expr;
            }

            var toInverted = false;
            var toExpr = sl.to;
            if (sl.to is index toInd)
            {
                toInverted = toInd.inverted; // странно, но не используется
                toExpr = toInd.index_expr;
            }

            if ((fromExpr is int32_const) && (fromExpr as int32_const).val == int.MaxValue)
                situation += 1;
            if ((toExpr is int32_const) && (toExpr as int32_const).val == int.MaxValue)
                situation += 2;

            var el = new expression_list();
            el.Add(new int32_const(situation));
            el.Add(sl.from); 
            // Это плохо - считается 2 раза. Надо делать semantic_expr_node !!!? Нет!!!
            // Если там будет лямбда, то не будет работать - известно, что semantic_expr_node не работает с лямбдами 
            // т.к. они несколько раз обходят код. 
            el.Add(sl.to);
            if (sl.step != null)
                el.Add(sl.step);
            return el;
        }

        /*
        public override void Exit(syntax_tree_node st)
        {
            if (st is slice_expr_question)
            {
                ProceedSliceQuestionExpr(st as slice_expr_question);
            } else if (st is slice_expr)
            {
                ProceedSliceExpr(st as slice_expr); 
            }
        }*/

        public override void visit(assign _assign)
        {
            _assign.from.visit(this);
            _assign.to.visit(this);
        }
        
        public override void visit(slice_expr sl)
        {
            expression_list el = null;
            if (sl.slices == null)
                el = construct_expression_list_for_slice_expr(sl);
            else el = construct_expression_list_for_slice_expr_multi(sl); // то это многомерный массив
            // надо как-то запретить многомерные слайсы в левой части присваивания
            if (sl.Parent is assign parent_assign && parent_assign.to == sl)
            {
                // если это многомерный слайс - кинуть ошибку
                if (sl.slices != null)
                {
                    // запретим пока или вовсе
                    throw new SyntaxVisitorError("MULTIDIMENSIONAL_SLICES_FORBIDDEN_IN_LEFT_SIDE_OF_ASSIGNMENT", sl.source_context);
                }
                el.Insert(0, parent_assign.from);
                var mc = method_call.NewP(
                        dot_node.NewP(
                            sl.v,
                            new ident("SystemSliceAssignment", sl.v.source_context),
                            sl.v.source_context),
                        el, sl.source_context);
                var systemSliceAssignmentCall = new procedure_call(mc, sl.source_context);
                var typeCompatibilityCheck = GetAssignmentTypeCompatibilityCheck(sl, parent_assign.from, mc);
                var checkAndDesugaredSliceBlock = new statement_list(typeCompatibilityCheck, systemSliceAssignmentCall);
                checkAndDesugaredSliceBlock.source_context = sl.source_context;
                ReplaceUsingParent(parent_assign, checkAndDesugaredSliceBlock);
                visit(systemSliceAssignmentCall); // обойти заменённое на предмет наличия такого же синтаксического сахара
            }
            else
            {
                if (sl.slices == null) // значит, это одномерный слайс - вызываем старый код
                {
                    var mc = method_call.NewP(dot_node.NewP(sl.v, new ident("SystemSlice", sl.v.source_context), sl.v.source_context), el, sl.source_context);
                    var sug = sugared_addressed_value.NewP(sl, mc, sl.source_context);
                    ReplaceUsingParent(sl, sug);
                    visit(mc); // обойти заменённое на предмет наличия такого же синтаксического сахара
                }
                else // многомерный слайс на чтение - вызываем груду новых функций
                {
                    // Определим, сколько размерностей надо оставлять в многомерном слайсе
                    // Столько - сколько в sl.slices кортежей с шагом int.MaxValue. Считаем:
                    var N = sl.slices.Count(s => { var u = s.Item3 as int32_const; return u == null || u.val != int.MaxValue; });
                    var mc = method_call.NewP(dot_node.NewP(sl.v, new ident("SystemSliceN"+N, sl.v.source_context), sl.v.source_context), el, sl.source_context);
                    var sug = sugared_addressed_value.NewP(sl, mc, sl.source_context);
                    // пока без семантической проверки
                    ReplaceUsingParent(sl, sug);
                    visit(mc);
                }
            }
        }

        public override void visit(slice_expr_question sl)
        {
            if (sl.Parent is assign parent_assign && parent_assign.to == sl)
            {
                throw new SyntaxVisitorError("CAN_NOT_ASSIGN_TO_LEFT_PART", sl.source_context);
            }
            var el = construct_expression_list_for_slice_expr(sl);
            var mc = method_call.NewP(dot_node.NewP(sl.v, new ident("SystemSliceQuestion", sl.v.source_context), sl.v.source_context), el, sl.source_context);
            var sug = sugared_addressed_value.NewP(sl, mc, sl.source_context);
            ReplaceUsingParent(sl, sug);
            visit(mc); // обойти заменённое на предмет наличия такого же синтаксического сахара
        }

        private semantic_check_sugared_statement_node GetAssignmentTypeCompatibilityCheck(expression expression1, expression expression2, expression expression3) =>
            new semantic_check_sugared_statement_node(SemanticCheckType.SliceAssignmentTypeCompatibility, new List<syntax_tree_node>() { expression1, expression2, expression3 });
    }
}
