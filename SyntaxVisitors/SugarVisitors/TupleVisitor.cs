// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.SugarVisitors
{
    /// <summary>
    ///  А здесь не только замена кортежа - здесь оптимизация (a,b) := (1,2) и var (a,b) := (1,2)
    /// </summary>
    public class TupleVisitor : BaseChangeVisitor
    {
        public static TupleVisitor New
        {
            get { return new TupleVisitor(); }
        }

        public override void visit(read_accessor_name wn)
        {
            DefaultVisit(wn);
        }

        public override void visit(unnamed_type_object u)
        {
            DefaultVisit(u);
        }

        public override void visit(name_assign_expr_list ne)
        {
            DefaultVisit(ne);
        }

        public override void visit(name_assign_expr ne)
        {
            DefaultVisit(ne);
        }

        public override void visit(labeled_statement ls)
        {
            var sl = new List<statement>();
            sl.Add(new labeled_statement(ls.label_name, new empty_statement(), ls.source_context));
            sl.Add(ls.to_statement);
            ReplaceStatementUsingParent(ls, sl);
            ProcessNode(ls.to_statement);
        }

        public override void visit(assign_var_tuple assvartup)
        {
            if (assvartup.expr is tuple_node tn && tn.el.expressions.All(ex => ex is const_node) && !tn.el.expressions.Any(ex => ex is nil_const))
            {
                var n = assvartup.idents.idents.Count();
                if (n > tn.el.Count)
                    throw new SyntaxVisitorError("TOO_MANY_ELEMENTS_ON_LEFT_SIDE_OF_TUPLE_ASSIGNMENT", assvartup.idents[0]);

                // Оптимизация, т.к. все - константы
                var sl = new List<statement>();
                for (var i = 0; i < n; i++)
                {
                    var a = new var_statement(
                        assvartup.idents.idents[i],
                        tn.el.expressions[i],
                        assvartup.idents.idents[i].source_context);
                    sl.Add(a);
                }
                ReplaceStatementUsingParent(assvartup, sl);
            }
            else
            {
                DefaultVisit(assvartup);
            }
        }

        public override void visit(assign_tuple asstup)
        {
            if (asstup.expr is tuple_node tn && tn.el.expressions.All(ex => ex is const_node) && !tn.el.expressions.Any(ex => ex is nil_const))
            {
                var n = asstup.vars.variables.Count();
                if (n > tn.el.Count)
                    throw new SyntaxVisitorError("TOO_MANY_ELEMENTS_ON_LEFT_SIDE_OF_TUPLE_ASSIGNMENT", asstup.vars.variables[0]);

                // Оптимизация, т.к. все - константы
                var sl = new List<statement>();
                for (var i = 0; i < n; i++)
                {
                    var a = new assign(asstup.vars.variables[i],
                        tn.el.expressions[i],
                        Operators.Assignment,
                        asstup.vars.variables[i].source_context);
                    sl.Add(a);
                }
                ReplaceStatementUsingParent(asstup, sl);
            }
            else
            {
                DefaultVisit(asstup);
            }
        }

        public void ReplaceVarTupleDefStatementUsingParent(var_tuple_def_statement from, IEnumerable<var_def_statement> to)
        {
            foreach (var x in to)
                x.Parent = from.Parent;
            var sl = from.Parent as variable_definitions;
            if (sl != null)
            {
                sl.ReplaceInList(from, to);
            }
        }

        public override void visit(var_tuple_def_statement vtd)
        {
            if (vtd.inital_value is tuple_node tn && tn.el.expressions.All(ex => ex is const_node) && !tn.el.expressions.Any(ex => ex is nil_const))
            {
                var n = vtd.vars.idents.Count();
                if (n > tn.el.Count)
                    throw new SyntaxVisitorError("TOO_MANY_ELEMENTS_ON_LEFT_SIDE_OF_TUPLE_ASSIGNMENT", vtd.vars.idents[0]);

                var vd = new List<var_def_statement>();
                for (var i = 0; i < n; i++)
                {
                    var a = new var_def_statement(vtd.vars.idents[i],
                        tn.el.expressions[i],
                        vtd.vars.idents[i].source_context);
                    vd.Add(a);
                }
                ReplaceVarTupleDefStatementUsingParent(vtd, vd);
                visit(vtd.inital_value);
            }
            else
            {
                DefaultVisit(vtd);
            }
        }

        public override void visit(tuple_node tup)
        {
            var dn = new dot_node(new dot_node(new ident("?System", tup.source_context), new ident("Tuple", tup.source_context), tup.source_context), new ident("Create", tup.source_context));
			var mc = new method_call(dn, tup.el, tup.source_context);

            //var sug = new sugared_expression(tup, mc, tup.source_context); - нет никакой семантической проверки - всё - на уровне синтаксиса!

            //ReplaceUsingParent(tup, mc); - исправление #1199. Оказывается, ReplaceUsingParent и Replace не эквивалентны - у копии Parent на старого родителя
            Replace(tup, mc);
            visit(mc); 
        }

    }
}
