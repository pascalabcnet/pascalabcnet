using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace ParsePABC1
{
    class BaseChangeVisitor : CollectUpperNodesVisitor
    {
        public void Replace(syntax_tree_node from, syntax_tree_node to)
        {
            var upper = UpperNode();
            if (upper == null)
                throw new Exception("У корневого элемента нельзя получить UpperNode");

            int ind = -1;
            for (var i = 0; i < upper.subnodes_count; i++)
                if (from == upper[i])
                {
                    ind = i;
                    break;
                }
            upper[ind] = to;
        }

        public statement_list UpperStatementList()
        {
            var stl = UpperNode() as statement_list;
            if (stl == null)
                throw new Exception("оператор вложен не в statement_list");
            return stl;
        }

        public bool DeleteInStatementList(statement st)
        {
            var stl = UpperStatementList();
            bool b = stl.subnodes.Remove(st);
            return b;
        }
        public bool DeleteInIdentList(ident id)
        {
            var idl = UpperNode() as ident_list;
            if (idl==null)
                throw new Exception("идентификатор не вложен в id_list");
            var b = idl.idents.Remove(id);
            return b;
        }

        public void ReplaceStatement(statement from, statement to)
        {
            var stl = UpperStatementList();
            var ind = stl.subnodes.IndexOf(from);
            if (ind == -1)
                throw new Exception("оператор from не найден - некорректный вызов ReplaceStatement");
            stl.subnodes[ind] = to;
        }
        public void ReplaceStatement(statement from, List<statement> to)
        {
            var stl = UpperStatementList();
            var ind = stl.subnodes.IndexOf(from);
            if (ind == -1)
                throw new Exception("оператор from не найден - некорректный вызов ReplaceStatement");
            stl.subnodes.RemoveAt(ind);
            stl.subnodes.InsertRange(ind, to);
        }
    }

}
