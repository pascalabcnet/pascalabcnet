// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.SugarVisitors
{
    public class TestAssignIsDefVisitor : BaseChangeVisitor
    {
        public static TestAssignIsDefVisitor New
        {
            get { return new TestAssignIsDefVisitor(); }
        }

        public override void visit(assign ass)
        {
            if ((ass.to as ident).name != "a")
                return;
            syntax_tree_node p = ass;
            while (!(p is PascalABCCompiler.SyntaxTree.block) && p != null)
                p = p.Parent;
            if (p == null)
                return;

            var typ = new same_type_node(ass.from);
            var decls = (p as block).defs;
            var vds = new var_def_statement(ass.to as ident, typ, ass.source_context);
            decls.Add(vds);
        }
    }
}
