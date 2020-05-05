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
                fromInverted = fromInd.inverted;
                fromExpr = fromInd.index_expr;
            }

            var toInverted = false;
            var toExpr = sl.to;
            if (sl.to is index toInd)
            {
                toInverted = toInd.inverted;
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
        public override void Exit(syntax_tree_node st)
        {
            if (st is slice_expr_question)
            {
                ProceedSliceQuestionExpr(st as slice_expr_question);
            } else if (st is slice_expr)
            {
                ProceedSliceExpr(st as slice_expr); 
            }
           // base.Exit(st);
        }
        public override void visit(assign _assign)
        {
            ProcessNode(_assign.from);
            ProcessNode(_assign.to);
        }
        
        public void ProceedSliceExpr(slice_expr sl)
        {
            var el = construct_expression_list_for_slice_expr(sl);
            if (sl.Parent is assign parent_assign && parent_assign.to == sl)
            {
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
                var mc = method_call.NewP(dot_node.NewP(sl.v, new ident("SystemSlice", sl.v.source_context), sl.v.source_context), el, sl.source_context);
                var sug = sugared_addressed_value.NewP(sl, mc, sl.source_context);
                ReplaceUsingParent(sl, sug);
                visit(mc); // обойти заменённое на предмет наличия такого же синтаксического сахара
            }
        }

        public void ProceedSliceQuestionExpr(slice_expr_question sl)
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
