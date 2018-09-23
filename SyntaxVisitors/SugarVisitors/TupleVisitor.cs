// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.SugarVisitors
{
    public class TupleVisitor : BaseChangeVisitor
    {
        public static TupleVisitor New
        {
            get { return new TupleVisitor(); }
        }

        public override void visit(read_accessor_name wn)
        {

        }

        public override void visit(unnamed_type_object u)
        {
            DefaultVisit(u);
        }

        public override void visit(name_assign_expr_list ne)
        {
            DefaultVisit(ne);
        }


        public override void visit(tuple_node tup)
        {
            var dn = new dot_node(new dot_node(new ident("?System"), new ident("Tuple")), new ident("Create", tup.source_context));
			var mc = new method_call(dn, tup.el, tup.source_context);

            //var sug = new sugared_expression(tup, mc, tup.source_context); - нет никакой семантической проверки - всё - на уровне синтаксиса!

            ReplaceUsingParent(tup, mc);
            visit(mc); 
        }

    }
}
