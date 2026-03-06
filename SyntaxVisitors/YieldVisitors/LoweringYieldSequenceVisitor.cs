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
using PascalABCCompiler.CoreUtils;

namespace SyntaxVisitors
{
    public class LoweringYieldSequenceVisitor : BaseChangeVisitor
    {
        private readonly GeneratedNamesManager generatedNamesManager;

        private LoweringYieldSequenceVisitor(GeneratedNamesManager generatedNamesManager)
        {
            this.generatedNamesManager = generatedNamesManager;
        }

        private ident NewVarName()
        {
            return new ident(generatedNamesManager.GenerateName("$yieldSeqForeachVar$"));
        }

        public static LoweringYieldSequenceVisitor Create(GeneratedNamesManager generatedNamesManager) => new LoweringYieldSequenceVisitor(generatedNamesManager);

        public static void Accept(procedure_definition pd, GeneratedNamesManager generatedNamesManager)
        {
            Create(generatedNamesManager).ProcessNode(pd);
        }

        public override void visit(yield_sequence_node yn)
        {
            var id = NewVarName();
            id.source_context = yn.source_context;
            var fe = new foreach_stmt(id, new no_type_foreach(), yn.ex, new yield_node(id,yn.source_context),null,yn.source_context);
            ReplaceStatement(yn, fe);
        }
    }
}
