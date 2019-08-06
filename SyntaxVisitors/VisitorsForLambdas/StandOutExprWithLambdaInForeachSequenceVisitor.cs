﻿// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
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

        //private ident_list ClassTemplateArgsOrNull = null; // если мы - в обобщенном классе, то это - его обобщенные параметры
        public ident GenIdentName()
        {
            GenIdNum++;
            return new ident("$GenContFE" + GenIdNum.ToString());
        }

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
}
