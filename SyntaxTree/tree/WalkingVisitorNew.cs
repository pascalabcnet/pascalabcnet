using System;
using System.Collections.Generic;


namespace PascalABCCompiler.SyntaxTree
{

    //Два варианта использования визитора:
    //1. Создать экземпляр этого класса, привязать к OnEnter и OnLeave обработчики посещений узла
    //и запустить обход. Визитор обойдет дерево и для каждого узла вызовет эти обработчики.
    //
    //2. Унаследовать от этого класса свой класс, в котором переорпеделить обработку нужных узлов.
    //Не забывать при этом о вызове visit'ов для подузлов (или base.visit), если это нужно.

    public class WalkingVisitorNew : AbstractVisitor
    {

		protected Action<syntax_tree_node> OnEnter;
        protected Action<syntax_tree_node> OnLeave;

        protected bool visitNode = true; // в OnEnter можно сделать false

        public virtual void ProcessNode(syntax_tree_node Node)
        {
            if (Node != null)
            {
                if (OnEnter != null)
                    OnEnter(Node);

                if (visitNode)
                    Node.visit(this);
                else visitNode = true;

                if (OnLeave != null)
                    OnLeave(Node);
            }
        }

        public override void DefaultVisit(syntax_tree_node n)
        {
            // n.subnodes_count может меняться в процессе работы этого алгоритма
            // В частности, в результате ReplaceStatementWithParent
            for (var i = 0; i < n.subnodes_count; i++)
                ProcessNode(n[i]);
        }

        // Можно перенести сюда поскольку замена 1 на 1 позволяет пользоваться текущим DefaultVisit
        public void ReplaceUsingParent(syntax_tree_node from, syntax_tree_node to)
        {
            to.Parent = from.Parent;
            if (from.Parent == null)
                throw new Exception("У корневого элемента нельзя получить Parent");
            from.Parent.ReplaceDescendant(from, to);
        }

        public void ReplaceStatementUsingParent(statement from, IEnumerable<statement> to, Desc d = Desc.DirectDescendants)
        {
            var fp = from.Parent; // SSM 02.10.18 - запомним, т.к. from.Parent может быть одним из x.Parent
            //foreach (var x in to) // Это и так делается по обоим веткам!
            //    x.Parent = fp;
            var sl = fp as statement_list;
            if (sl != null)
            {
                sl.ReplaceInList(from, to);
            }
            else
            {
                var l = new statement_list();
                l.AddMany(to);
                l.source_context = from.source_context;
                fp.ReplaceDescendant(from, l, d);
                l.Parent = fp; // на всякий случай
            }
        }
    }

}