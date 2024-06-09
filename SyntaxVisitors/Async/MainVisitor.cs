using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.Async
{
    internal class MainVisitor :BaseChangeVisitor
    {
        // Визитор, который проверяет, есть ли хоть
        // один await в  асинхронном методе
        public static bool HasAwait = false;
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
            HasAwait = true;
        }
        public override void visit(await_node aw)
        {
            HasAwait = true;
        }
    }
}
