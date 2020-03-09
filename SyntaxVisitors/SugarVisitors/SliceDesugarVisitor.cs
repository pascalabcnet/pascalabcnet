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

            if ((sl.from is int32_const) && (sl.from as int32_const).val == int.MaxValue)
                situation += 1;
            if ((sl.to is int32_const) && (sl.to as int32_const).val == int.MaxValue)
                situation += 2;

            var el = new expression_list();
            el.Add(new int32_const(situation));
            el.Add(sl.from); // Это плохо - считается 2 раза. Надо делать semantic_expr_node !!!? Нет!!!
                             // Если там будет лямбда, то не будет работать - известно, что semantic_expr_node не работает с лямбдами 
                             // т.к. они несколько раз обходят код. 
            el.Add(sl.to);
            if (sl.step != null)
                el.Add(sl.step);
            el.Add(new bool_const(sl.index_inversion_from));
            el.Add(new bool_const(sl.index_inversion_to));
            return el;
        }

        public override void visit(assign _assign)
        {
            _assign.from.visit(this);
            _assign.to.visit(this);
        }

        public override void visit(slice_expr sl)
        {
            var el = construct_expression_list_for_slice_expr(sl);
            if (sl.Parent is assign parent_assign && parent_assign.to == sl)
            {
                el.Insert(0, parent_assign.from);
                var systemSliceAssignmentCall = new procedure_call(
                    method_call.NewP(
                        dot_node.NewP(
                            sl.v,
                            new ident("SystemSliceAssignment", sl.v.source_context),
                            sl.v.source_context),
                        el, sl.source_context),
                    sl.source_context);
                var typeCompatibilityCheck = GetAssignmentTypeCompatibilityCheck(sl.v, parent_assign.from);
                var checkAndDesugaredSliceBlock = new statement_list(typeCompatibilityCheck, systemSliceAssignmentCall);
                checkAndDesugaredSliceBlock.source_context = sl.source_context;
                ReplaceUsingParent(parent_assign, checkAndDesugaredSliceBlock);
                //visit(systemSliceAssignmentCall); // обойти заменённое на предмет наличия такого же синтаксического сахара
            }
            else
            {
                var mc = method_call.NewP(dot_node.NewP(sl.v, new ident("SystemSlice", sl.v.source_context), sl.v.source_context), el, sl.source_context);
                var sug = sugared_addressed_value.NewP(sl, mc, sl.source_context);
                ReplaceUsingParent(sl, sug);
                visit(mc); // обойти заменённое на предмет наличия такого же синтаксического сахара
            }
        }

        public override void visit(slice_expr_question sl)
        {
            var el = construct_expression_list_for_slice_expr(sl);
            var mc = method_call.NewP(dot_node.NewP(sl.v, new ident("SystemSliceQuestion", sl.v.source_context), sl.v.source_context), el, sl.source_context);
            var sug = sugared_addressed_value.NewP(sl, mc, sl.source_context);
            ReplaceUsingParent(sl, sug);
            visit(mc); // обойти заменённое на предмет наличия такого же синтаксического сахара
        }

        private semantic_check_sugared_statement_node GetAssignmentTypeCompatibilityCheck(expression expression1, expression expression2) =>
            new semantic_check_sugared_statement_node(SemanticCheckType.SliceAssignmentTypeCompatibility, new List<syntax_tree_node>() { expression1, expression2 });
    }
}
