// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler.Errors;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class LoweringYieldSequenceVisitor : BaseChangeVisitor
    {
        private int _Num = 0;

        private ident NewVarName()
        {
            ++_Num;
            return new ident("$yieldSeqForeachVar$" + _Num);
        }

        public static LoweringYieldSequenceVisitor New
        {
            get { return new LoweringYieldSequenceVisitor(); }
        }

        public static void Accept(procedure_definition pd)
        {
            New.ProcessNode(pd);
        }

        public override void visit(yield_sequence_node yn)
        {
            var id = NewVarName();
            id.source_context = yn.source_context;
            var fe = new foreach_stmt(id, new no_type_foreach(), yn.ex, new yield_node(id,yn.source_context),yn.source_context);
            ReplaceStatement(yn, fe);
        }
    }
}
