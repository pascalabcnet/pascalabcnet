// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;
using System;
using System.Collections.Generic;

namespace SyntaxVisitors.SugarVisitors
{
    // Попробую сюда включить десахаризацию for с шагом
    public class LoopDesugarVisitor : BaseChangeVisitor, IPipelineVisitor
    {
        public static LoopDesugarVisitor New
        {
            get { return new LoopDesugarVisitor(); }
        }

        public void Visit(syntax_tree_node root, VisitorsContext context, Action next)
        {
            ProcessNode(root);

            next();
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
                var oa = new ident("#oa" + un, a.source_context);
                var ob = new ident("#ob" + un, b.source_context);
                var h = new ident("#h" + un, fn.increment_value.source_context);
                var n = new ident("#n" + un, a.source_context);
                var i = fn.loop_variable; //new ident(iname, a.source_context);
                var j = new ident("#j" + un, a.source_context);
                var avar = new var_statement(a, fn.initial_value, fn.initial_value.source_context);
                var bvar = new var_statement(b, fn.finish_value, fn.finish_value.source_context);
                var hvar = new var_statement(h, "integer", fn.increment_value, fn.increment_value.source_context);
                var bexhe0 = new bin_expr(h.TypedClone(), new int32_const(0), Operators.Equal, h.source_context);
                var raise = new raise_stmt(new new_expr("PABCSystem.ZeroStepException"), null,h.source_context);
                var if0 = new if_node(bexhe0, raise, null, h.source_context);

                statement ivar = null;
                if (fn.create_loop_variable || fn.type_name != null)
                { ivar = new var_statement(i, fn.type_name, a); ivar.source_context = a.source_context; }
                else
                    ivar = new assign(i, a, a.source_context);

                var mcOrdb = new method_call(new dot_node(new ident("PABCSystem"), new ident("Ord")), new expression_list(b), b.source_context);
                var mcOrda = new method_call(new dot_node(new ident("PABCSystem"), new ident("Ord")), new expression_list(a), a.source_context);
                var oavar = new var_statement(oa, mcOrda, oa.source_context);
                var obvar = new var_statement(ob, mcOrdb, ob.source_context);
                var bexhg0 = new bin_expr(h.TypedClone(), new int32_const(0), Operators.Greater, h.source_context);
                var bexhl0 = new bin_expr(h.TypedClone(), new int32_const(0), Operators.Less, h.source_context);
                var bexbla = new bin_expr(b.TypedClone(), a.TypedClone(), Operators.Less, b.source_context);
                var bexalb = new bin_expr(a.TypedClone(), b.TypedClone(), Operators.Less, a.source_context);
                var bexand1 = new bin_expr(bexhg0, bexbla, Operators.LogicalAND, h.source_context);
                var bexand2 = new bin_expr(bexhl0, bexalb, Operators.LogicalAND, h.source_context);
                var bexor = new bin_expr(bexand1, bexand2, Operators.LogicalOR, h.source_context);
                var assn0 = new assign(n.TypedClone(), new int32_const(0), Operators.Assignment, n.source_context);
                var ifn = new if_node(bexor, assn0, null, bexor.source_context);

                var sub = new bin_expr(ob.TypedClone(), oa.TypedClone(), Operators.Minus, b.source_context);
                var div = new bin_expr(sub, h, Operators.IntegerDivision, b.source_context);
                var nexpr = new bin_expr(div, new int32_const(1), Operators.Plus, b.source_context);
                var nvar = new var_statement(n, nexpr, n.source_context);

                var exlist = new expression_list(i, i.source_context);
                exlist.Add(h);

                var IncIh = new method_call(new ident("Inc"), exlist, i.source_context);
                var stlist = new statement_list(fn.statements, fn.statements.source_context);
                var pc = new procedure_call(IncIh, IncIh.source_context);
                var ig0 = new bin_expr(j.TypedClone(), new int32_const(1), Operators.Greater, i.source_context);
                var ifpc = new if_node(ig0, pc, null, ig0.source_context);

                stlist.Insert(0, ifpc);
                //stlist.Add(pc);


                var semCheck1 = new semantic_check_sugared_statement_node(typeof(for_node), new List<syntax_tree_node> { a,b }, a.source_context);

                var for_st = new for_node(j, new int32_const(1), n, stlist, fn.statements.source_context);
                var mainstlist = new statement_list(avar, bvar, semCheck1, oavar, obvar, hvar, if0, nvar, ifn, ivar, for_st);
                mainstlist.source_context = avar.source_context;

                ReplaceUsingParent(fn, mainstlist);
                ProcessNode(avar);
                ProcessNode(bvar);
                ProcessNode(hvar);
                // ProcessNode(stlist.list[0]); // это было сделано чтобы выводилась ошибка Нельзя менять переменную цикла в теле цикла. 
                // Но во вложенных циклах все равно не срабатывает - убрал. Буду думать - что делать
                ProcessNode(stlist); // К сожалению, из-за этого не выводится ошибка при попытке изменения переменной цикла внутри цикла
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
        var ob := Ord(b);
        var oa := Ord(a);
        var n := (ob - oa) div h + 1;
        if h > 0 and b < a or h < 0 and b > a then
          n := 0;
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

        public override void visit(foreach_stmt fe)
        {
            if (fe.index != null && !fe.index.name.StartsWith("##")) // Повторно обходить ## не надо - он для семантики
            {
                var newindex = new ident("##" + fe.index, fe.index.source_context); // нам нужно это имя на семантике для контроля неизменения переменной внутри цикла
                var indexvar = new var_statement(fe.index, new int32_const(-1), fe.index.source_context);
                var IncIndex = new assign(fe.index.TypedClone(), new int32_const(1), Operators.AssignmentAddition);
                var forstat = new statement_list(IncIndex, fe.stmt);
                var fe2 = new foreach_stmt(fe.identifier, fe.type_name, fe.in_what, forstat, newindex, fe.source_context);
                var stat = new statement_list(indexvar,fe2);
                ReplaceUsingParent(fe, stat);
                ProcessNode(fe2.in_what);
                ProcessNode(forstat);
            }
            else DefaultVisit(fe);
        }


    }
}
