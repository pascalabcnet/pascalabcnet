// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.SyntaxTreeConverters
{

    // Первое предназначение - вынести последовательность из заголовка в foreach до foreach как отдельное присваивание
    // Второе предназначение - переименовать все переменные, совпадающие по имени с типом T обобщенного класса, в котором находится метод, содержащий лямбду
    public class VarNamesInMethodsWithSameNameAsClassGenericParamsReplacer: CollectFullLightSymInfoVisitor, IPipelineVisitor
    {
        /// <summary>
        /// Надо приводить к нижнему регистру
        /// </summary>
        //public List<string> NamesForReplace = new List<string>();

        private int LambdaNestedLevel = 0;

        Dictionary<string, ScopeSyntax> d = new Dictionary<string, ScopeSyntax>(); // словарь скоупов для каждого имени
        // При входе в обобщенный класс или запись мы в словарь добавляем все его обобщенные параметры со значением nil
        // При выходе из класса мы очищаем словарь
        // При встрече описания переменной с именем, совпадающим с одним из ключей в словаре, мы заменяем значение null 
        //   на пространство имен, в котором мы находимся
        // Описание переменной в словарь добавляется только если она описана в методе, но вне лямбды, поскольку в лямбде конфликтов имен нет
        // При выходе из пространства имен мы проверяем, есть ли в словаре такое значение, и если да, очищаем его, присваивая null
        // Переименовывать будем все имена name если ключ name есть в словаре и d[name] != null
       
        public VarNamesInMethodsWithSameNameAsClassGenericParamsReplacer(compilation_unit pm) : base(pm)
        {

        }

        public void Visit(syntax_tree_node root, VisitorsContext context, Action next)
        {
            Root = scopeCreator.GetScope(root) as GlobalScopeSyntax;

            ProcessNode(root);

            next();
        }

        public VarNamesInMethodsWithSameNameAsClassGenericParamsReplacer() : base() { }

        public override void AddSymbol(ident name, SymKind kind, type_definition td = null, SymbolAttributes attr = 0)
        {
            if (name == null || name.name == null)
            {
                return;
            }
            var n = name.name.ToLower();
            //if (LambdaNestedLevel > 0) // т.е. впервые встретилось переопределение именно в лямбде, тогда пропускать эту лямбду
            //    return;
            // ! нельзя вносить в список поля этого класса! SSM 30/05/2020 #2242
            if (d.ContainsKey(n) && d[n] == null && (kind == SymKind.var && ! (name.Parent.Parent.Parent is class_members) || kind == SymKind.param)) // Добавляем не все, а только разыскиваемые и только если это - переменная
            {
                d[n] = this.Current;
                base.AddSymbol(name, kind, td, attr);
            }
        }
        public override void Enter(syntax_tree_node st)
        {
            base.Enter(st);
            if (st is function_lambda_definition)
                LambdaNestedLevel += 1;
            else if (st is class_definition cl)
            {
                var td = cl.Parent as type_declaration;
                if (td?.type_name is template_type_name ttn)
                {
                    foreach (var id in ttn.template_args.idents)
                    {
                        d[id.name.ToLower()] = null;
                    }
                }
            }
            else if (st is procedure_definition pd)
            {
                var cn = pd.proc_header.name?.class_name;
                if (cn is template_type_name ttn)
                {
                    foreach (var id in ttn.template_args.idents)
                    {
                        d[id.name.ToLower()] = null;
                    }
                }
            }
        }
        public override void PreExitScope(syntax_tree_node st)
        {
            var l = d.Keys.Where(k => d[k] == Current).ToList();
            // Обходим все словари и смотрим у них вершину стеков
            foreach (var k in l)
            {
                d[k] = null; // т.е. в этом пространстве имен мы захватили одноименное описание переменной - освобождаем его
            }
        }
        public override void Exit(syntax_tree_node st)
        {
            if (st is function_lambda_definition)
                LambdaNestedLevel -= 1;
            else if (st is class_definition) // то мы не ищем переопределение имен, поскольку мы вышли из класса
            {
                d.Clear();
            }
            else if (st is procedure_definition pd) // мы вышли из метода вида t1<t>.p
            {
                var cn = pd.proc_header.name?.class_name;
                if (cn is template_type_name ttn)
                    d.Clear();
            }
            base.Exit(st);
        }

        public override void visit(template_type_name tn)
        {
            // игнорировать чтобы там ничего не переименовывалось
        }
        public override void visit(ident id)
        {
            ReplaceNameOrNot(id);
        }
        public override void visit(dot_node dn)
        {
            if (dn.left is ident id)
                ReplaceNameOrNot(id);
            else ProcessNode(dn.left);
        }

        public static int ReplaceNum = 0;
        public void ReplaceNameOrNot(ident id)
        {
            var n = id.name.ToLower();
            if (d.ContainsKey(n) && d[n] != null) // Переименовываем без страха
            {
                id.name = id.name + "$Replace$" + ReplaceNum; // имя такое чтобы при выдаче сообщения об ошибке в нем можно было бы обрезать конец
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
