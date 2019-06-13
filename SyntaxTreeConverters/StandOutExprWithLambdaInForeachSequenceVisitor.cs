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


    public class GivenNamesReplacer: CollectLightSymInfoVisitor
    {
        /// <summary>
        /// Надо приводить к нижнему регистру
        /// </summary>
        public List<string> NamesForReplace { get; }

        private int LambdaNestedLevel = 0;

        Dictionary<string, Stack<ScopeSyntax>> d = new Dictionary<string, Stack<ScopeSyntax>>(); // словарь скоупов для каждого имени
        // При добавлении символа мы в словарь добавляем его ПИ (символ может быть правильно вложенным только если вначале идет локальная переменная, 
        // потом - параметр лямбды, потом - локальная переменная вложенной лямбды и т.д.
        public static new GivenNamesReplacer New 
        {
            get => new GivenNamesReplacer();
        }
        public override void AddSymbol(ident name, SymKind kind, type_definition td = null, Attributes attr = 0)
        {
            var n = name.name.ToLower();
            if (LambdaNestedLevel > 0 && d[n].Count() == 0) // т.е. впервые встретилось переопределение именно в лямбде, тогда пропускать эту лямбду
                return;
            if (NamesForReplace.Contains(n) && (kind == SymKind.var || kind == SymKind.param)) // Добавляем не все, а только разыскиваемые и только если это - переменная
            {
                d[n].Push(this.Current);
                base.AddSymbol(name, kind, td, attr);
            }
        }
        public override void Enter(syntax_tree_node st)
        {
            if (st is function_lambda_definition)
                LambdaNestedLevel += 1;
            else if (st is class_definition cl)
            {
                var td = cl.Parent as type_declaration;
            }
            base.Enter(st);
        }
        public override void Exit(syntax_tree_node st)
        {
            // Обходим все словари и смотрим у них вершину стеков
            foreach (var k in d.Keys)
            {
                var stack = d[k];
                if (stack.Count() > 0 && stack.Peek() == Current)
                {
                    stack.Pop();
                }
            }
            if (st is function_lambda_definition)
                LambdaNestedLevel -= 1;
            base.Exit(st);
        }

        public override void visit(ident id)
        {
            Process(id);
        }
        public override void visit(dot_node dn)
        {
            if (dn.left is ident id)
                Process(id);
            else ProcessNode(dn.left);
        }

        public static int ReplaceNum = 0;
        public void Process(ident id)
        {
            var n = id.name.ToLower();
            if (NamesForReplace.Contains(n) && d[n].Count() == 1) // Проблема такая - первый раз T будет описано именно в лямбде. 
            {
                id.name = "$" + id.name + ReplaceNum;
            }
        }
    }

    // Может, переименовать все T? Всё равно алгоритм переименовывания будет один
    // Искать описания с T. Как нашли - с этого места переименовывать
    // Сделать визитор LocalRenamer и переименовывать с места описания

    /*public class LocalRenamer : BaseChangeVisitor
    {

        public static void RenameFrom(var_def_statement vd, string Name, string NewName)
        {
            var lr = new LocalRenamer();
            lr.ProcessNode(vd);
        }
    }*/


    // Поиск всех захваченных переменных в лямбде
    // Это сложно
    /*public class LambdaCapturedNamesSearcher : BaseChangeVisitor
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
    }*/
}
