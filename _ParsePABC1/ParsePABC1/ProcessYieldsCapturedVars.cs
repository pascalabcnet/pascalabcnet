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
       
        
        public FindMainIdentsVisitor mids = new FindMainIdentsVisitor(); // захваченные переменные процедуры по всем её yield 

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
                visitNode = false;
            }
            if (st is procedure_definition)
            {
                mids.vars.Clear(); // очищать при входе в процедуру
            }
        }
        public override void Exit(syntax_tree_node st)
        {
            if (st is procedure_definition) // т.е. мы разобрали процедуру и уже выходим. Это значит, что пока yield будет обрабатываться только в процедурах. Так это и надо.
            {
                if (!hasYields)
                    return;

                var dld = new DeleteLocalDefs(mids.vars); // mids.vars - все захваченные переменные
                st.visit(dld); // Удалить в локальных и блочных описаниях этой процедуры все захваченные переменные
                // В результате работы в mids.vars что-то осталось. Это не локальные переменные и с ними непонятно что делать
                dld.AfterProcTraverse();

                var cm = new class_members(access_modifer.public_modifer);
                foreach (var m in dld.BlockDeletedIds)
                    cm.Add(m);
                cm.Add(st as procedure_definition);
                var cc = new type_declaration(newClassName(), SyntaxTreeBuilder.BuildClassDefinition(true, cm), SyntaxTreeBuilder.BuildGenSC);
                var cct = new type_declarations(cc, null);

                Replace(st, cct);
                
                /*Console.WriteLine("Удаленные переменные: " + (st as procedure_definition).proc_header.name.meth_name.name);
                foreach (var dd in dld.deletedIdsToDeleteInLocalScope)
                    Console.Write(dd + " ");
                Console.WriteLine();
                Console.WriteLine("Оставшиеся переменные: " + (st as procedure_definition).proc_header.name.meth_name.name);
                foreach (var dd in mids.vars)
                    Console.Write(dd + " ");
                Console.WriteLine();*/
            }
            base.Exit(st);
        }
        public override void visit(yield_node yn)
        {
            hasYields = true;
            yn.visit(mids);
            // mids.vars - надо установить, какие из них - локальные, какие - из этого класса, какие - являются параметрами функции, а какие - глобальные (все остальные)
            // те, которые являются параметрами, надо скопировать в локальные переменные и переименовать использование везде по ходу данной функции 
            // самое сложное - переменные-поля этого класса - они требуют в создаваемом классе, реализующем итератор, хранить Self текущего класса и добавлять это Self везде по ходу алгоритма
            // вначале будем считать, что переменные-поля этого класса и переменные-параметры не захватываются yield
            //base.visit(yn);
        }
        public override void visit(declarations d) // в обратном порядке
        {
            if (d.defs != null)
                for (int i = d.defs.Count - 1; i >= 0; --i) // в обратном порядке - тогда удаление текущего элемента работает корректно
                    ProcessNode(d.defs[i]);
        }

    }
}
