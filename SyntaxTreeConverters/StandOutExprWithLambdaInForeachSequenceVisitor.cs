// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.SyntaxTreeConverters
{

    // Первое предназначение - вынести последовательность из заголовка в foreach до foreach как отдельное присваивание
    // Второе предназначение - переименовать все переменные, совпадающие по имени с типом T обобщенного класса, в котором находится метод, содержащий лямбду
    public class StandOutExprWithLambdaInForeachSequenceVisitor : BaseChangeVisitor
    {
        public static StandOutExprWithLambdaInForeachSequenceVisitor New
        {
            get
            {
                return new StandOutExprWithLambdaInForeachSequenceVisitor();
            }
        }

        private int GenIdNum = 0;
        //private int GenRenameIdNum = 0;

        //private ident_list ClassTemplateArgsOrNull = null; // если мы - в обобщенном классе, то это - его обобщенные параметры
        // надо бы еще то же сделать если метод описывается отдельно
        public ident GenIdentName()
        {
            GenIdNum++;
            return new ident("$GenContFE" + GenIdNum.ToString());
        }

/*        public string RenameIdentName(string name)
        {
            GenRenameIdNum++;
            return "$GenRenameLocalInLambda" + GenRenameIdNum.ToString();
        }
        public override void Enter(syntax_tree_node st)
        {
            if (st is class_definition cd)
            {
                var a = cd.Parent as type_declaration;
                ClassTemplateArgsOrNull = (a?.type_name as template_type_name)?.template_args;
            }
                
        }

        public override void Exit(syntax_tree_node st)
        {
            if (st is class_definition)
                ClassTemplateArgsOrNull = null;
        }

        public override void visit(procedure_definition pd)
        {
            if (ClassTemplateArgsOrNull == null)
                return;
            var lfds = pd.DescendantNodes().OfType<function_lambda_definition>();
            if (lfds.Count() == 0)
                return;
        }*/

        public override void visit(foreach_stmt fe)
        {
            //if (fe.DescendantNodes().OfType<function_lambda_definition>().Count() > 0) // из-за #1984 убрал вообще условие. Пусть будет всегда
            {
                var id = GenIdentName();
                id.Parent = fe;
                var ass = new var_statement(id, fe.in_what, fe.in_what.source_context);
                fe.in_what = id;
                var l = new List<statement> { ass, fe };
                //ReplaceStatement(fe, l);
                ReplaceStatementUsingParent(fe, l);
            }

            base.visit(fe);
        }
    }

    // Может, переименовать все T? Всё равно алгоритм переименовывания будет один
    // Искать описания с T. Как нашли - с этого места переименовывать
    // Сделать визитор LocalRenamer и переименовывать с места описания

    public class LocalRenamer : BaseChangeVisitor
    {

        public static void RenameFrom(var_def_statement vd, string Name, string NewName)
        {
            var lr = new LocalRenamer();
            lr.ProcessNode(vd);
        }
    }


    // Поиск всех захваченных переменных в лямбде
    // Это сложно
    public class LambdaCapturedNamesSearcher : BaseChangeVisitor
    {
        List<ident> idents = new List<ident>();
        public static LambdaCapturedNamesSearcher New
        {
            get
            {
                return new LambdaCapturedNamesSearcher();
            }
        }
        public override void visit(ident id)
        {
            // хорошо бы всё здесь захватить! Но вдруг это - описание...
        }
        public override void visit(dot_node dn)
        {
            // это точно надо захватывать
        }

        public override void visit(var_def_statement vds)
        {
            // имена обойти отдельно, инициализатор - отдельно
            foreach (var id in vds.vars.idents)
            {
                // Надо проверять совпадение имен с именами в параметрах обобщения
                // если не совпадают, то просто пропускаем
                // если совпадают, то исключаем это имя из списка проверки! Всё - имя уже переопределено! И это произошло в лямбде, что можно
            }
            visit(vds.inital_value);
        }
        public override void visit(function_lambda_definition ld)
        {
            // параметры обойти отдельно, тело отдельно
            // проблема, что могут быть вложенные лямбды. Т.е это - основная, а в ней - ещё.
        }
    }
}
