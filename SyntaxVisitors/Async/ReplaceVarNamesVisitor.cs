using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.Async
{
    public class ReplaceVarNamesVisitor: BaseChangeVisitor
    {
        private string _oldName;
        private string _newName;
        public int counter = 0;

        public ReplaceVarNamesVisitor(ident oldName, ident newName)
        {
            _oldName = oldName.name;
            _newName = newName.name;
            counter = 0;
        }

        public override void visit(ident id)
        {
            if (id.name == _oldName)
            {
                id.name = _newName;
                counter++;
            }
        }

        public override void visit(try_stmt tr)
        {
            ProcessNode(tr.stmt_list);
        }
    }
}
