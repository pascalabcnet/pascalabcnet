using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.SugarVisitors
{
    public class SliceQuestionDesugarVisitor : BaseChangeVisitor
    {
        public static SliceQuestionDesugarVisitor New
        {
            get { return new SliceQuestionDesugarVisitor(); }
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

            return el;
        }

        public override void visit(slice_expr_question sl)
        {
            var el = construct_expression_list_for_slice_expr(sl);
            var mc = new method_call(new dot_node(sl.v, new ident("SystemSliceQuestion", sl.v.source_context), sl.v.source_context), el, sl.source_context);

            ReplaceUsingParent(sl, mc);
            visit(mc); // обойти заменённое на предмет наличия такого же синтаксического сахара
        }

    }
}
