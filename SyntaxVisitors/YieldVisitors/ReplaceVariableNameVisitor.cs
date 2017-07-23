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
        private string _oldName;
        private string _newName;

        public ReplaceVariableNameVisitor(ident oldName, ident newName)
        {
            _oldName = oldName.name;
            _newName = newName.name;
        }

        // Этот алгоритм маломощный и меняет всё внутри лямбды. 
        // Например, если переименовывается x, то 
        // в записи     t := y->begin var x := '4'; Result := x*2 end;
        // и в записи   t := x->x;
        // все x будут переименованы. Но это нестрашно, хотя неэффективно

        public override void visit(ident id)
        {
            if (id.name != _oldName)
            {
                return;
            }

            //var upperNode = UpperNode();
            //if (
            //    upperNode != null /*&& (upperNode as dot_node) == null*/) // Я не знаю, зачем вообще было второе условие. Видимо, это всё надо убрать SSM
            //{
                //Replace(id, _newName);
                // заменяются только строки, а сами идентификаторы как объекты не меняются!
            id.name = _newName;
            //}
        }

        // SSM 29/04/17 - В записи x.x обходится (и заменяется если надо) только первое x
        // Важно что в записи x.x.y всё разбивается на части так: x.x и y
        public override void visit(dot_node dn)
        {
            ProcessNode(dn.left);
            if (dn.right.GetType() != typeof(ident))
                ProcessNode(dn.right);
        }
    }
}
