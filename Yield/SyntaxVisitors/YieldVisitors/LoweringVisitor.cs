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
        private int _enumeratorNum = 0;

        private VarNames NewVarNames(ident name)
        {
            _varnum++;
            return new VarNames()
            {
                VarName = "$" + name.name + _varnum,
                VarEndName = "<>varLV" + _varnum
            };
        }

        private ident NewEnumeratorName()
        {
            ++_enumeratorNum;
            return "$enumerator$" + _enumeratorNum;
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



        public override void visit(yield_unknown_foreach_type _unk)
        {
            // DO NOTHING
        }

        public override void visit(yield_unknown_foreach_type_ident _unk)
        {
            // DO NOTHING
        }

        // frninja 21/05/16
        public override void visit(foreach_stmt frch)
        {
            var enumeratorIdent = this.NewEnumeratorName();

            // Переменная цикла
            var currentIdent = frch.identifier;
            type_definition currentIdentType = null;

            // Новое тело
            var stl = new statement_list();

            // С типом
            if (frch.type_name == null)
            {
                // Внешнее имя, создавать ниче не надо
            }
            else
            {
                // var имеет место быть
                if (frch.type_name is no_type_foreach)
                {
                    // автовывод
                    currentIdentType = new yield_unknown_foreach_type(frch);
                }
                else
                {
                    currentIdentType = frch.type_name;
                }

                // Получаем служебное имя с $ и заменяем его в теле цикла
                currentIdent = this.NewVarNames(frch.identifier).VarName;
                var replacerVis = new ReplaceVariableNameVisitor(frch.identifier, currentIdent);
                frch.visit(replacerVis);


                // Создаем переменную цикла и добавляем в stl
                stl.Add(new var_statement(currentIdent, currentIdentType));

            }

            // Добавляем $current$ = $enumerator$.Current
            //stl.Add(new assign(currentIdent, new dot_node(enumeratorIdent, "Current")));
            stl.Add(new assign(currentIdent, new method_call(new yield_unknown_foreach_type_ident(frch), new expression_list(new dot_node(enumeratorIdent, "Current")))));


            // Добавляем тело цикла в stl
            ProcessNode(frch.stmt);
            stl.Add(frch.stmt);

            var whileNode = new while_node(new method_call(new dot_node(enumeratorIdent, "MoveNext"), new expression_list()),
                stl,
                WhileCycleType.While);

            ReplaceStatement(frch,
                SeqStatements(new var_statement(enumeratorIdent,
                                    new named_type_reference("System.Collections.IEnumerator"),
                // НЕБЕЗОПАСНО!
                                    new method_call(new dot_node(frch.in_what as addressed_value, new ident("GetEnumerator")), new expression_list())),
                              whileNode));

        }

        private expression CreateConditionFromCaseVariant(expression param, expression_list list)
        {
            var res = list.expressions.Aggregate(new bool_const(false) as expression, (acc, expr) =>
                {
                    bin_expr currentExpr = null;
                    diapason_expr diap = expr as diapason_expr;
                    if (diap != null)
                    {
                        currentExpr = new bin_expr(new bin_expr(param, diap.left, Operators.GreaterEqual),
                            new bin_expr(param, diap.right, Operators.LessEqual),
                            Operators.LogicalAND);
                    }
                    else
                    {
                        currentExpr = new bin_expr(param, expr, Operators.Equal);
                    }

                    return new bin_expr(acc, currentExpr, Operators.LogicalOR);
                });

            return res;
        }

        // frninja 30/05/16
        public override void visit(case_node csn)
        {
            var b = HasStatementVisitor<yield_node>.Has(csn);
            if (!b)
                return;
            
            /*
             * 
             * case i of
             *   cv1: bla1;
             *   cv2: bla2;
             *   ..
             *   cvN: blaN;
             * else: bla_else;
             * 
             * --->
             * 
             * if i satisfy cv1 
             *   then bla1
             * else if i satisfy cv2
             *   then bla2
             * ..
             * else if i satisfy cvN
             *   then blaN
             * else bla_else
             * 
            */

            if_node currentIfNode = null;
            statement currentIfElseClause = (csn.else_statement != null) ? csn.else_statement :new statement_list(new empty_statement()); ;
            
            for (int i = csn.conditions.variants.Count - 1; i >= 0; --i)
            {
                case_variant cv = csn.conditions.variants[i];

                ProcessNode(cv.exec_if_true);

                var ifCondition = this.CreateConditionFromCaseVariant(csn.param, cv.conditions);
                currentIfNode = new if_node(ifCondition, new statement_list(cv.exec_if_true), new statement_list(currentIfElseClause));
                currentIfElseClause = currentIfNode;
            }

            if_node finalIfNode = currentIfNode;

            ReplaceStatement(csn, finalIfNode);
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
                //Replace(ifn, SeqStatements(gotoStartIfCondition, ifn.then_body, lbAfter));
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
            var b = HasStatementVisitor<yield_node>.Has(rn);
            if (!b)
                return;

            var gotoContinue = goto_statement.New;
            var gotoBreak = goto_statement.New;

            ReplaceBreakContinueWithGotoLabelVisitor replaceBreakContinueVis = new ReplaceBreakContinueWithGotoLabelVisitor(gotoContinue, gotoBreak);
            rn.statements.visit(replaceBreakContinueVis);

            ProcessNode(rn.statements);

            var gotoContinueIfNotCondition = new if_node(un_expr.Not(rn.expr), gotoContinue);
            var continueLabeledStatement = new labeled_statement(gotoContinue.label, new statement_list(rn.statements, gotoContinueIfNotCondition));

            var breakLabeledStatement = new labeled_statement(gotoBreak.label);


            ReplaceStatement(rn, SeqStatements(gotoContinue, continueLabeledStatement, breakLabeledStatement));

            // в declarations ближайшего блока добавить описание labels
            block bl = listNodes.FindLast(x => x is block) as block;

            bl.defs.Add(new label_definitions(gotoContinue.label, gotoBreak.label));

        }

        public override void visit(while_node wn)
        {
            var b = HasStatementVisitor<yield_node>.Has(wn);
            if (!b)
                return;

            var gotoBreak = goto_statement.New;
            var gotoContinue = goto_statement.New;

            ReplaceBreakContinueWithGotoLabelVisitor replaceBreakContinueVis = new ReplaceBreakContinueWithGotoLabelVisitor(gotoContinue, gotoBreak);
            wn.statements.visit(replaceBreakContinueVis);

            ProcessNode(wn.statements);

            var if0 = new if_node(un_expr.Not(wn.expr), gotoBreak);
            var lb2 = new labeled_statement(gotoContinue.label, if0); // continue
            var lb1 = new labeled_statement(gotoBreak.label); // break

            ReplaceStatement(wn, SeqStatements(lb2, wn.statements, gotoContinue, lb1));

            // в declarations ближайшего блока добавить описание labels
            block bl = listNodes.FindLast(x => x is block) as block;

            bl.defs.Add(new label_definitions(gotoBreak.label, gotoContinue.label));
        }

        public override void visit(for_node fn)
        {
            /*
             * initializer;
             * goto end;
             * start:
             *      body;
             * continue:
             *      increment;
             * end:
             *      GotoIfTrue condition start;
             * break:

             */

            var b = HasStatementVisitor<yield_node>.Has(fn);
            if (!b)
                return;

            var gotoStart = goto_statement.New;
            var gotoEnd = goto_statement.New;
            var gotoContinue = goto_statement.New;
            var gotoBreak = goto_statement.New;

            ReplaceBreakContinueWithGotoLabelVisitor replaceBreakContinueVis = new ReplaceBreakContinueWithGotoLabelVisitor(gotoContinue, gotoBreak);
            fn.statements.visit(replaceBreakContinueVis);

            ProcessNode(fn.statements);


            var newNames = this.NewVarNames(fn.loop_variable);

            var newLoopVar = fn.create_loop_variable ? new ident(newNames.VarName) : fn.loop_variable;

            // Нужно заменить fn.loop_variable -> newLoopVar в теле цикла
            var replacerVis = new ReplaceVariableNameVisitor(fn.loop_variable, newLoopVar);
            fn.visit(replacerVis);

            fn.loop_variable = newLoopVar;

            var endtemp = new ident(newNames.VarEndName); //new ident(newVarName());

            //var initializer = new var_statement(fn.loop_variable, fn.type_name, fn.initial_value);
            //var initializer = new var_statement(fn.loop_variable, fn.type_name, fn.initial_value);
            //var ass2 = new var_statement(endtemp, fn.type_name, fn.finish_value);

            // frninja 05/06/16 - фиксим для !fn.create_variable
            var initializer = fn.create_loop_variable
                ? new var_statement(fn.loop_variable, fn.type_name, fn.initial_value) as statement
                : new assign(fn.loop_variable, fn.initial_value) as statement;


            var gotoStartIfCondition = new if_node((fn.cycle_type == for_cycle_type.to) ?
                new bin_expr(fn.loop_variable, fn.finish_value, Operators.LessEqual) :
                new bin_expr(fn.loop_variable, fn.finish_value, Operators.GreaterEqual), gotoStart);

            var endLabeledStatement = new labeled_statement(gotoEnd.label, gotoStartIfCondition);
            var startLabeledStatement = new labeled_statement(gotoStart.label, fn.statements);
            var Inc = new procedure_call(new method_call((fn.cycle_type == for_cycle_type.to) ?
                new ident("Inc") :
                new ident("Dec"), new expression_list(fn.loop_variable)));

            var continueLabeledStatement = new labeled_statement(gotoContinue.label, Inc);
            var breakLabeledStatement = new labeled_statement(gotoBreak.label);

            ReplaceStatement(fn, SeqStatements(initializer, gotoEnd, startLabeledStatement, continueLabeledStatement, endLabeledStatement, breakLabeledStatement));

            // в declarations ближайшего блока добавить описание labels
            block bl = listNodes.FindLast(x => x is block) as block;

            bl.defs.Add(new label_definitions(gotoStart.label, gotoEnd.label, gotoContinue.label, gotoBreak.label));
        }

        
    }

}
