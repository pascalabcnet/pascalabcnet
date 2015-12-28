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

	public delegate void VisitorDelegateNew(syntax_tree_node node);

    public class WalkingVisitorNew : AbstractVisitor
    {

		protected VisitorDelegateNew OnEnter;
        protected VisitorDelegateNew OnLeave;

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
            var count = n.subnodes_count;
            for (var i = 0; i < count; i++)
                ProcessNode(n[i]);
        }
    }
 
}