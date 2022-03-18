// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.SugarVisitors
{

    public class NewRangeDesugarAndFindHasYieldVisitor : BaseChangeVisitor
    {
        public static NewRangeDesugarAndFindHasYieldVisitor New
        {
            get { return new NewRangeDesugarAndFindHasYieldVisitor(); }
        }

        public override void visit(diapason_expr_new diap)
        {
            if (diap.Parent.Parent is pascal_set_constant)
            {
                // вернуть назад к diapason_expr
                var d = new diapason_expr(diap.left, diap.right, diap.source_context);
                ReplaceUsingParent(diap, d);
                visit(d);
                return;
            }

            var el = new expression_list();
            el.Add(diap.left, diap.left.source_context);
            el.Add(diap.right, diap.right.source_context);

            // Проблема в том, что тут тоже надо перепрошивать Parent!
            var mc = method_call.NewP(dot_node.NewP(new ident("PABCSystem", diap.source_context), new ident("InternalRange", diap.source_context), diap.source_context), el, diap.source_context);

            var sug = sugared_addressed_value.NewP(diap, mc, diap.source_context);

            ReplaceUsingParent(diap, sug);
            visit(mc); // обойти заменённое на предмет наличия такого же синтаксического сахара
        }

        public override void visit(bin_expr ex)
        {
            if (ex.operation_type == Operators.In && ex.right is diapason_expr_new diap && ex.left is addressed_value exl)
            {
                var el = new expression_list();
                el.Add(exl, exl.source_context);
                el.Add(diap.left, diap.left.source_context);
                el.Add(diap.right, diap.right.source_context);
                var mc = method_call.NewP(new ident("InRangeInternal", exl.source_context), el, exl.source_context);
                var sug = sugared_addressed_value.NewP(ex, mc, ex.source_context);
                ReplaceUsingParent(ex, sug);
                visit(mc); // обойти заменённое на предмет наличия такого же синтаксического сахара
            }
            else base.visit(ex);
        }

        public override void visit(foreach_stmt fe)
        {
            if (fe.in_what is diapason_expr_new diap && fe.index == null)
            {
                var from = diap.left;
                var typ = fe.type_name;
                if (typ != null && typ is no_type_foreach)
                    typ = null;
                var cr = fe.type_name is no_type_foreach;
                var fn = new for_node(fe.identifier, diap.left, diap.right, fe.stmt, for_cycle_type.to, null, typ, cr);

                var sl = new List<statement>();
                // надо проверить типы в диапазоне. Проверять соответствие типа переменной foreach возможно не надо - сообщение об ошибке и так будет хорошим. Посмотрим 
                sl.Add(new semantic_check_sugared_statement_node(typeof(foreach_stmt), new List<syntax_tree_node> { diap, fe.type_name, fe.identifier }, fe.source_context));

                sl.Add(fn);

                ReplaceStatementUsingParent(fe, sl);
                visit(fn);
            }
            /*else if (fe.in_what is dot_node dn && dn.right is ident id && id.name.ToLower() == "indices")
            {
                var typ = fe.type_name;
                if (typ != null && typ is no_type_foreach)
                    typ = null;
                var cr = fe.type_name is no_type_foreach;

                var left = new int32_const(0, fe.identifier.source_context);
                var right = dn.left.dot_node("Count").Minus(1); // Строки таким образом сделать не получится. Разве что в семантическом контроле чуть довернуть этот узел ))

                var fn = new for_node(fe.identifier, left, right, fe.stmt, for_cycle_type.to, null, typ, cr);
                var sl = new List<statement>();
                sl.Add(new semantic_check_sugared_statement_node(typeof(foreach_stmt), new List<syntax_tree_node> { dn.left }, fe.source_context));
                sl.Add(fn);

                ReplaceStatementUsingParent(fe, sl);
                visit(fn);
            }*/
            else base.visit(fe);
        }

        List<procedure_definition> lpd = new List<procedure_definition>();

        public override void visit (procedure_definition pd)
        {
            lpd.Add(pd);
            base.visit(pd);
            lpd.RemoveAt(lpd.Count - 1);
        }
        public override void visit(yield_node yn)
        {
            if (lpd.Count > 0)
                lpd[lpd.Count - 1].has_yield = true;
            base.visit(yn);
        }

        public override void visit(yield_sequence_node yn)
        {
            if (lpd.Count > 0)
                lpd[lpd.Count - 1].has_yield = true;
            base.visit(yn);
        }

    }
}