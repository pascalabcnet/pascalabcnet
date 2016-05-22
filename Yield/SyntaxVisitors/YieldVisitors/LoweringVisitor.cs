using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler.Errors;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class VarNames
    {
        public string VarName { get; set; }
        public string VarEndName { get; set; }

        public VarNames()
        {
        }

        public VarNames(string varName, string varEndName)
        {
            VarName = varName;
            VarEndName = varEndName;
        }
    }

    public class LoweringVisitor : BaseChangeVisitor
    {
        public static LoweringVisitor New
        {
            get { return new LoweringVisitor(); }
        }

        private int _varnum = 0;

        private string newVarName()
        {
            _varnum++;
            return "<>varLV" + _varnum.ToString();
        }

        private VarNames NewVarNames(ident name)
        {
            _varnum++;
            return new VarNames()
            {
                VarName = "$" + name.name + _varnum,
                VarEndName = "<>varLV" + _varnum
            };
        }

        public static void Accept(procedure_definition pd)
        {
            New.ProcessNode(pd);
        }

        public override void Enter(syntax_tree_node st)
        {
            base.Enter(st);
            if (!(st is procedure_definition || st is block || st is statement_list || st is case_node || st is for_node || st is foreach_stmt || st is if_node || st is repeat_node || st is while_node || st is with_statement || st is try_stmt || st is lock_stmt))
            {
                //visitNode = false;
            }
        }

        public override void visit(if_node ifn)
        {
            ProcessNode(ifn.then_body);
            ProcessNode(ifn.else_body);

            var b = HasStatementVisitor<yield_node>.Has(ifn);
            if (!b)
                return;

            var gtAfter = goto_statement.New;
            var lbAfter = new labeled_statement(gtAfter.label);

            if ((object)ifn.else_body == null)
            {
                var if0 = new if_node(un_expr.Not(ifn.condition), gtAfter);
                //Replace(ifn, SeqStatements(if0, ifn.then_body, lbAfter));
                ReplaceStatement(ifn, SeqStatements(if0, ifn.then_body, lbAfter));

                // в declarations ближайшего блока добавить описание labels
                block bl = listNodes.FindLast(x => x is block) as block;

                bl.defs.Add(new label_definitions(gtAfter.label));
            }
            else
            {
                var gtAlt = goto_statement.New;
                var lbAlt = new labeled_statement(gtAlt.label, ifn.else_body);

                var if0 = new if_node(un_expr.Not(ifn.condition), gtAlt);

                ReplaceStatement(ifn, SeqStatements(if0, ifn.then_body, gtAfter, lbAlt, lbAfter));

                // в declarations ближайшего блока добавить описание labels
                block bl = listNodes.FindLast(x => x is block) as block;

                bl.defs.Add(new label_definitions(gtAfter.label, gtAlt.label));
            }
        }

        public override void visit(repeat_node rn)
        {
            ProcessNode(rn.statements);

            var b = HasStatementVisitor<yield_node>.Has(rn);
            if (!b)
                return;

            var gtContinue = goto_statement.New;
            var gtBreak = goto_statement.New;

            var lbContinue = new labeled_statement(gtContinue.label, rn.statements);
            var lbBreak = new labeled_statement(gtBreak.label);

            var if0 = new if_node(un_expr.Not(rn.expr), gtContinue);


            ReplaceStatement(rn, SeqStatements(lbContinue, if0, lbBreak));

            // в declarations ближайшего блока добавить описание labels
            block bl = listNodes.FindLast(x => x is block) as block;

            bl.defs.Add(new label_definitions(gtContinue.label, gtBreak.label));

        }

        public override void visit(while_node wn)
        {
            ProcessNode(wn.statements);

            var b = HasStatementVisitor<yield_node>.Has(wn);
            if (!b)
                return;

            var gt1 = goto_statement.New;
            var gt2 = goto_statement.New;

            var if0 = new if_node(un_expr.Not(wn.expr), gt1);
            var lb2 = new labeled_statement(gt2.label, if0); // continue
            var lb1 = new labeled_statement(gt1.label); // break

            ReplaceStatement(wn, SeqStatements(lb2, wn.statements, gt2, lb1));

            // в declarations ближайшего блока добавить описание labels
            block bl = listNodes.FindLast(x => x is block) as block;

            bl.defs.Add(new label_definitions(gt1.label, gt2.label));
        }

        public override void visit(for_node fn)
        {
            ProcessNode(fn.statements);

            var b = HasStatementVisitor<yield_node>.Has(fn);
            if (!b)
                return;

            var gt1 = goto_statement.New;
            var gt2 = goto_statement.New;

            var newNames = this.NewVarNames(fn.loop_variable);

            var newLoopVar = fn.create_loop_variable ? new ident(newNames.VarName) : fn.loop_variable;

            // Нужно заменить fn.loop_variable -> newLoopVar в теле цикла
            var replacerVis = new ReplaceForVariableVisitor(fn.loop_variable, newLoopVar);
            fn.visit(replacerVis);

            fn.loop_variable = newLoopVar;

            var endtemp = new ident(newNames.VarEndName); //new ident(newVarName());

            //var ass1 = new var_statement(fn.loop_variable, fn.type_name, fn.initial_value);
            var ass1 = new var_statement(fn.loop_variable, fn.type_name, fn.initial_value);
            //var ass2 = new var_statement(endtemp, fn.type_name, fn.finish_value);



            var if0 = new if_node((fn.cycle_type == for_cycle_type.to) ?
                bin_expr.Greater(fn.loop_variable, fn.finish_value) :
                bin_expr.Less(fn.loop_variable, fn.finish_value), gt1);

            var lb2 = new labeled_statement(gt2.label, if0);
            var lb1 = new labeled_statement(gt1.label);
            var Inc = new procedure_call(new method_call((fn.cycle_type == for_cycle_type.to) ?
                new ident("Inc") :
                new ident("Dec"), new expression_list(fn.loop_variable)));

            ReplaceStatement(fn, SeqStatements(ass1, /*ass2,*/ lb2, fn.statements, Inc, gt2, lb1));

            // в declarations ближайшего блока добавить описание labels
            block bl = listNodes.FindLast(x => x is block) as block;

            bl.defs.Add(new label_definitions(gt1.label, gt2.label));
        }
    }

}
