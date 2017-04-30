// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class ReplaceVariableNameVisitor : BaseChangeVisitor
    {
        private ident _oldName;
        private ident _newName;

        public ReplaceVariableNameVisitor(ident oldName, ident newName)
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
                upperNode != null /*&& (upperNode as dot_node) == null*/) // Я не знаю, зачем вообще было второе условие. Видимо, это всё надо убрать
            {
                //Replace(id, _newName);
                // заменяются только строки, а сами идентификаторы как объекты не меняются!
                id.name = _newName.name;
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
