using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace ParsePABC1
{
    class ProcessYieldCapturedVarsVisitor : BaseChangeVisitor 
    {
        int clnum = 0;

        public string newClassName()
        {
            clnum++;
            return "clyield#" + clnum.ToString();
        }
       
        public FindMainIdentsVisitor mids; // захваченные переменные процедуры по всем её yield 

        public int countNodesVisited;

        public bool hasYields = false;

        public ProcessYieldCapturedVarsVisitor()
        {
            //PrintInfo = false; 
        }

        /*public override void visit(procedure_definition pd)
        {

        } */

        public override void Enter(syntax_tree_node st)
        {
            base.Enter(st);
            countNodesVisited++;

            // сокращение обходимых узлов. Как сделать фильтр по тем узлам, которые необходимо обходить? Например, все операторы (без выражений и описаний), все описания (без операторов)
            if (st is assign || st is var_def_statement || st is procedure_call || st is procedure_header || st is expression)
            {
                visitNode = false; // фильтр - куда не заходить 
            }
        }

        type_declaration GenClassForYield(procedure_definition pd, IEnumerable<var_def_statement> fields)
        {
            // Теперь на месте процедуры генерируем класс
            var cm = new class_members(access_modifer.public_modifer);
            foreach (var m in fields)
                cm.Add(m);
            cm.Add(pd);
            return new type_declaration(newClassName(), SyntaxTreeBuilder.BuildClassDefinition(cm), SyntaxTreeBuilder.BuildGenSC);
        }

        public override void visit(procedure_definition pd)
        {
            hasYields = false;
            mids = new FindMainIdentsVisitor();

            base.visit(pd);

            if (!hasYields) // т.е. мы разобрали процедуру и уже выходим. Это значит, что пока yield будет обрабатываться только в процедурах. Так это и надо.
                return;

            var dld = new DeleteLocalDefs(mids.vars); // mids.vars - все захваченные переменные
            pd.visit(dld); // Удалить в локальных и блочных описаниях этой процедуры все захваченные переменные
            // В результате работы в mids.vars что-то осталось. Это не локальные переменные и с ними непонятно что делать
            dld.AfterProcTraverse();

            // Конструируем определение класса
            var cc = GenClassForYield(pd, dld.BlockDeletedIds.Union(dld.LocalDeletedIds));
            var cct = new type_declarations(cc);

            Replace(pd, cct);

            mids = null; // вдруг мы выйдем из процедуры, не зайдем в другую, а там - оператор! Такого конечно не может быть
        }

        public override void visit(yield_node yn)
        {
            hasYields = true;
            if (mids != null) // если мы - внутри процедуры
                yn.visit(mids);
            // mids.vars - надо установить, какие из них - локальные, какие - из этого класса, какие - являются параметрами функции, а какие - глобальные (все остальные)
            // те, которые являются параметрами, надо скопировать в локальные переменные и переименовать использование везде по ходу данной функции 
            // самое сложное - переменные-поля этого класса - они требуют в создаваемом классе, реализующем итератор, хранить Self текущего класса и добавлять это Self везде по ходу алгоритма
            // вначале будем считать, что переменные-поля этого класса и переменные-параметры не захватываются yield
            //base.visit(yn);
        }
    }
}
