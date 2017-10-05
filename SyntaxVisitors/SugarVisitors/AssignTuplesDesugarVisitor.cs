// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.SugarVisitors
{
    public class AssignTuplesDesugarVisitor : BaseChangeVisitor
    {
        public static AssignTuplesDesugarVisitor New
        {
            get { return new AssignTuplesDesugarVisitor(); }
        }

        private int num = 0;

        public string UniqueNumStr()
        {
            num++;
            return num.ToString();
        }

        public override void visit(assign_tuple asstup)
        {
            // тут возможно ошибка более глубокая - в semantic_check_sugared_statement_node(asstup) возможно остаются во вложенных лямбдах другие assign_tuple
            var sl = new statement_list();
            sl.Add(new semantic_check_sugared_statement_node(typeof(assign_tuple), new List<syntax_tree_node> { asstup.vars, asstup.expr }, asstup.source_context)); // Это нужно для проверок на этапе преобразования в семантику

            var tname = "#temp_var" + UniqueNumStr();
            var tt = new var_statement(new ident(tname), asstup.expr);
            sl.Add(tt);

            var n = asstup.vars.variables.Count();
            for (var i = 0; i < n; i++)
            {
                var a = new assign(asstup.vars.variables[i], new dot_node(new ident(tname),
                    new ident("Item" + (i + 1).ToString())), Operators.Assignment,
                    asstup.vars.variables[i].source_context);
                sl.Add(a);
            }
            // Замена 1 оператор на 1 оператор - всё OK
            ReplaceUsingParent(asstup, sl);

            visit(asstup.expr);
        }

        public void ReplaceAssignVarTupleUsingParent(assign_var_tuple from, IEnumerable<declaration> to, Desc d = Desc.DirectDescendants)
        {
            foreach (var x in to)
                x.Parent = from.Parent;
            var sl = from.Parent as declarations;
            if (sl != null)
            {
                sl.ReplaceInList(from, to);
            }
        }


        public override void visit(assign_var_tuple assvartup)
        {
            var tname = "#temp_var" + UniqueNumStr();
            if (assvartup.Parent is declarations ds)
            {
                var ld = new List<declaration>();
                ld.Add(new semantic_check_sugared_statement_node(typeof(assign_var_tuple), new List<syntax_tree_node> { assvartup.idents, assvartup.expr }, assvartup.source_context)); // Это нужно для проверок на этапе преобразования в семантику

                var vd = new variable_definitions();
                var tt1 = new var_def_statement(new ident(tname), assvartup.expr);
                vd.Add(tt1);

                var nn = assvartup.idents.idents.Count();
                for (var i = 0; i < nn; i++)
                {
                    var a = new var_def_statement(assvartup.idents.idents[i],
                        new dot_node(new ident(tname), new ident("Item" + (i + 1).ToString())),
                        assvartup.idents.idents[i].source_context);
                    vd.Add(a);
                }
                ld.Add(vd);
                ReplaceAssignVarTupleUsingParent(assvartup,ld);
                visit(assvartup.expr);
                return;
            }

            var sl = new List<statement>();
            sl.Add(new semantic_check_sugared_statement_node(typeof(assign_var_tuple), new List<syntax_tree_node> { assvartup.idents, assvartup.expr }, assvartup.source_context)); // Это нужно для проверок на этапе преобразования в семантику

            var tt = new var_statement(new ident(tname), assvartup.expr); // тут для assvartup.expr внутри повторно вызывается convert_strong, это плохо, но если там лямбда, то иначе - с semantic_addr_value - не работает!!!
            sl.Add(tt); // он же помещается в новое синтаксическое дерево

            var n = assvartup.idents.idents.Count();
            for (var i = 0; i < n; i++)
            {
                var a = new var_statement(assvartup.idents.idents[i],
                    new dot_node(new ident(tname), new ident("Item" + (i + 1).ToString())),
                    assvartup.idents.idents[i].source_context);
                sl.Add(a);
            }
            ReplaceStatementUsingParent(assvartup, sl);

            visit(assvartup.expr); // В assvartup.expr могут содержаться лямбды, в которых будут другие assign_var_tuple. 
            // И вообще другой синтаксический сахар, размещённый в этом визиторе
        }
    }
}
