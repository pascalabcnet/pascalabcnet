// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.SugarVisitors
{
    // Попробую сюда включить десахаризацию for с шагом
    public class LoopDesugarVisitor : BaseChangeVisitor
    {
        public static LoopDesugarVisitor New
        {
            get { return new LoopDesugarVisitor(); }
        }

        private int num = 0;

        public string UniqueNumStr()
        {
            num++;
            return num.ToString();
        }

        public override void visit(loop_stmt loop)
        {
            // тут возможно ошибка более глубокая - в semantic_check_sugared_statement_node(asstup) возможно остаются во вложенных лямбдах другие assign_tuple
            var sl = new statement_list();
            sl.Add(new semantic_check_sugared_statement_node(typeof(loop_stmt), new List<syntax_tree_node> { loop.count }, loop.source_context));

            var tname = "#loop_var" + UniqueNumStr();
            var fn = new for_node(new ident(tname), new int32_const(1), loop.count, loop.stmt, loop.source_context);
            sl.Add(fn);

            ReplaceUsingParent(loop, sl);

            visit(fn);
        }

        public override void visit(for_node fn)
        {
            if (fn.increment_value != null)
            {
                if (fn.cycle_type == for_cycle_type.downto)
                    throw new SyntaxVisitorError("FOR_LOOP_WITH_STEP_CANNOT_USE_DOWNTO", fn);
                if (fn.increment_value is int32_const ic && ic.val == 0)
                    throw new SyntaxVisitorError("STEP_CANNOT_BE_0", fn.increment_value);
                // * семантика - проверка, что h - целое. 
                // * Можно на константу 0 проверить
                // * семантика - a, b - ожидался порядковый тип
                // семантика - b приводится к типу a
                var un = UniqueNumStr();
                var a = new ident("#a" + un, fn.initial_value.source_context);
                var b = new ident("#b" + un, fn.finish_value.source_context);
                var h = new ident("#h" + un, fn.increment_value.source_context);
                var n = new ident("#n" + un, a.source_context);
                var i = fn.loop_variable; //new ident(iname, a.source_context);
                var j = new ident("#j" + un, a.source_context);
                var avar = new var_statement(a, fn.initial_value, fn.initial_value.source_context);
                var bvar = new var_statement(b, fn.finish_value, fn.finish_value.source_context);
                var hvar = new var_statement(h, "integer", fn.increment_value, fn.increment_value.source_context);
                statement ivar = null;
                if (fn.create_loop_variable || fn.type_name != null)
                { ivar = new var_statement(i, fn.type_name, a); ivar.source_context = a.source_context; }
                else
                    ivar = new assign(i, a, a.source_context);

                var mcOrdb = new method_call(new dot_node(new ident("PABCSystem"), new ident("Ord")), new expression_list(b), b.source_context);
                var mcOrda = new method_call(new dot_node(new ident("PABCSystem"), new ident("Ord")), new expression_list(a), a.source_context);
                var sub = new bin_expr(mcOrdb, mcOrda, Operators.Minus, b.source_context);
                var div = new bin_expr(sub, h, Operators.IntegerDivision, b.source_context);
                var nexpr = new bin_expr(div, new int32_const(1), Operators.Plus, b.source_context);
                var nvar = new var_statement(n, nexpr, n.source_context);

                var exlist = new expression_list(i, i.source_context);
                exlist.Add(h);

                var IncIh = new method_call(new ident("Inc"), exlist, i.source_context);
                var stlist = new statement_list(fn.statements, fn.statements.source_context);
                var pc = new procedure_call(IncIh, IncIh.source_context);
                stlist.Add(pc);

                var semCheck1 = new semantic_check_sugared_statement_node(typeof(for_node), new List<syntax_tree_node> { a,b }, a.source_context);

                var for_st = new for_node(j, new int32_const(1), n, stlist, fn.statements.source_context);
                var mainstlist = new statement_list(avar, bvar, semCheck1, hvar, nvar, ivar, for_st);
                mainstlist.source_context = avar.source_context;

                ReplaceUsingParent(fn, mainstlist);
                ProcessNode(avar);
                ProcessNode(bvar);
                ProcessNode(hvar);
                ProcessNode(stlist.list[0]);
                // Надо обойти подкомпоненты - вдруг там в лямбдах for со stepом
            }
            else DefaultVisit(fn);
        }

        //public override void visit(for_node fn)
        //{
        // for var i:=a to b step h do
        //    Print(i); 
        /*var a := 4;
        var b := 1;
        var h := -2;*/
        // семантика - проверка, что h - целое. Можно на константу 0 проверить
        // семантика - a, b - ожидался порядковый тип
        // семантика - b приводится к типу a
        /* if h = 0 then
          raise new ZeroStepException;
        var n := (Ord(b) - Ord(a)) div h + 1;
        var i := a;
        for var j := 1 to n do
        begin
          Print(i);
          Inc(i, h);
        end; */

        //if (fn.increment_value != null)
        //{

        //}
        /*var sl = new statement_list();
        sl.Add(new semantic_check_sugared_statement_node(typeof(loop_stmt), new List<syntax_tree_node> { loop.count }, loop.source_context));

        var tname = "#loop_var" + UniqueNumStr();
        var fn = new for_node(new ident(tname), new int32_const(1), loop.count, loop.stmt, loop.source_context);
        sl.Add(fn);

        ReplaceUsingParent(loop, sl);

        visit(fn);*/
        //}

    }
}
