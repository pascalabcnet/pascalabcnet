// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
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
    public class IsVarRenameVisitor : BaseChangeVisitor
    {

        public IsVarRenameVisitor()
        {
        }

        public static IsVarRenameVisitor New
        {
            get { return new IsVarRenameVisitor(); }
        }

        public static void Accept(procedure_definition pd)
        {
            New.ProcessNode(pd);
        }

        public override void visit(procedure_header proc_header)
        {
            // DO THIS THING!
        }
       /* public override void visit(desugared_deconstruction dd)
        {
            var l = new List<statement>();
            l.Add(dd);
            foreach (var d in dd.variables.definitions)
            {
                foreach (var id in d.vars.idents)
                {
                    // добавить после неё кучу объявлений с присваиваниями
                    //ReplaceStatement(dd,)
                    var newvar = new ident(NewVariableName(id.name),id.source_context);
                    var vs = new var_statement(new ident(id.name, id.source_context), newvar, id.source_context);
                    l.Add(vs);
                    id.name = newvar.name;
                }
            }
            ReplaceStatement(dd, l);
        }*/

        private string NewVariableName(string name)
        {
            name = name.ToLower();
            return "$RenIsVarYield$" + name; 
        }
    }
}
