using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.SyntaxTree
{
    public class BaseChangeVisitor : CollectUpperNodesVisitor
    {
        public override void DefaultVisit(syntax_tree_node n)
        {
            // Элементы списков - с конца в начало чтобы можно было эти элементы изменять по ходу (удалять/вставлять/заменять один несколькими)
            var Сount = n.subnodes_count;
            var СountWithoutListElements = n.subnodes_without_list_elements_count;

            for (var i = 0; i < СountWithoutListElements; i++)
                ProcessNode(n[i]);

            for (var i = Сount - 1; i >= СountWithoutListElements; i--) // в обратном порядке
                ProcessNode(n[i]);
        }

        public void Replace(syntax_tree_node from, syntax_tree_node to)
        {
            var upper = UpperNode();
            if (upper == null)
                throw new Exception("У корневого элемента нельзя получить UpperNode");
            upper.ReplaceDescendant(from, to);
        }

        public void ReplaceStatement(statement from, statement to)
        {
            to.Parent = from.Parent;
            var stl = UpperNodeAs<statement_list>();
            stl.ReplaceInList(from, to);
        }

        public void ReplaceStatement(statement from, IEnumerable<statement> to)
        {
            foreach (var x in to)
                x.Parent = from.Parent;
            var stl = UpperNodeAs<statement_list>();
            stl.ReplaceInList(from, to);
        }

        public T UpperNodeAs<T>(int up = 1) where T : syntax_tree_node
        {
            var stl = UpperNode(up) as T;
            if (stl == null)
                throw new Exception("Элемент вложен не в " + typeof(T));
            return stl;
        }

        public T UpperTo<T>() where T : syntax_tree_node
        {
            syntax_tree_node upperNode = null;

            int up = 1;

            do
            {
                upperNode = UpperNode(up);
                ++up;
            } 
            while ((object)upperNode != null && !(upperNode is T));

            return (object)upperNode != null ? upperNode as T : null;
        }

        public bool DeleteInIdentList(ident id)
        {
            var idl = UpperNodeAs<ident_list>();
            return idl.Remove(id);
        }

        public bool DeleteInStatementList(statement st)
        {
            var stl = UpperNodeAs<statement_list>();
            return stl.Remove(st);
        }

        public static IEnumerable<statement> SeqStatements(params statement[] seq)
        {
            var l = new List<statement>();
            foreach (var st in seq)
                if (st is statement_list)
                    l.AddRange((st as statement_list).list);
                else l.Add(st);
            return l;
        }
    }

}
