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
    public class LoweringVisitor : BaseChangeVisitor
    {
        public static LoweringVisitor New
        {
            get { return new LoweringVisitor();  }
        }

        int varnum = 0;

        public string newVarName()
        {
            varnum++;
            return "<>varLV" + varnum.ToString();
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
                visitNode = false;
            }
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
            var lb2 = new labeled_statement(gt2.label, if0);
            var lb1 = new labeled_statement(gt1.label);

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

            var endtemp = new ident(newVarName());
            var ass1 = new var_statement(fn.loop_variable, fn.type_name, fn.initial_value);
            var ass2 = new var_statement(endtemp, fn.type_name, fn.finish_value);


            var if0 = new if_node(bin_expr.Greater(fn.loop_variable, fn.finish_value), gt1);
            var lb2 = new labeled_statement(gt2.label, if0);
            var lb1 = new labeled_statement(gt1.label);
            var Inc = new procedure_call(new method_call(new ident("Inc"),new expression_list(fn.loop_variable)));

            ReplaceStatement(fn, SeqStatements(ass1,ass2,lb2, fn.statements, Inc, gt2, lb1));

            // в declarations ближайшего блока добавить описание labels
            block bl = listNodes.FindLast(x => x is block) as block;

            bl.defs.Add(new label_definitions(gt1.label, gt2.label));
        }
    }

}
