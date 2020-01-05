// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.SugarVisitors
{
    public class NewRangeDesugarVisitor : BaseChangeVisitor
    {
        public static NewRangeDesugarVisitor New
        {
            get { return new NewRangeDesugarVisitor(); }
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
            var mc = method_call.NewP(dot_node.NewP(new ident("PABCSystem", diap.source_context), new ident("Range", diap.source_context), diap.source_context), el, diap.source_context);

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
            if (fe.in_what is diapason_expr_new diap)
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
            }
            else base.visit(fe);
        }
    }
}