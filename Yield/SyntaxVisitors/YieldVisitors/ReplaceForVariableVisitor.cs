using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class ReplaceForVariableVisitor : BaseChangeVisitor
    {
        private ident _oldName;
        private ident _newName;

        public ReplaceForVariableVisitor(ident oldName, ident newName)
        {
            _oldName = oldName;
            _newName = newName;
        }

        public override void visit(ident id)
        {
            if (id.name != _oldName.name)
            {
                return;
            }

            var upperNode = UpperNode();
            if (
                (object)upperNode != null && (object)(upperNode as dot_node) == null)
            {
                Replace(id, _newName);
            }
        }

        // frninja 11/03/16 - оно тут вообще надо???
        public override void visit(dot_node dn)
        {
            ProcessNode(dn.left);
            if (dn.right.GetType() != typeof(ident))
                ProcessNode(dn.right);
        }
    }
}
