using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyntaxVisitors.Async
{
    internal class MainVisitor :BaseChangeVisitor
    {
        public static bool flag = false;
        public static MainVisitor New
        {
            get { return new MainVisitor(); }
        }
        public static void Accept(statement_list st)
        {
            New.ProcessNode(st);
        }
        public static void Accept(procedure_definition pd)
        {
            New.ProcessNode(pd);
        }
        public override void visit(await_node_statement ans)
        {
            flag = true;
        }
        public override void visit(await_node aw)
        {
            flag = true;
        }
    }
}
