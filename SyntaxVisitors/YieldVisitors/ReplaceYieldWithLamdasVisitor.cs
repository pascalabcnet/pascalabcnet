// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using PascalABCCompiler.CoreUtils;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class ReplaceYieldExprByVarVisitor : BaseChangeVisitor
    {
        private readonly GeneratedNamesManager generatedNamesManager;

        private ReplaceYieldExprByVarVisitor(GeneratedNamesManager generatedNamesManager)
        {
            this.generatedNamesManager = generatedNamesManager;
        }

        private ident NewVarName()
        {
            return new ident(generatedNamesManager.GenerateName("$yieldExprVar$"));
        }

        public static ReplaceYieldExprByVarVisitor Create(GeneratedNamesManager generatedNamesManager) => new ReplaceYieldExprByVarVisitor(generatedNamesManager);

        public static void Accept(procedure_definition pd, GeneratedNamesManager generatedNamesManager)
        {
            Create(generatedNamesManager).ProcessNode(pd);
        }

        public override void visit(yield_node yn)
        {
            var VarIdent = this.NewVarName();
            VarIdent.source_context = yn.ex.source_context;
            var_statement vs;
            if (yn.ex is nil_const)
                vs = new var_statement(VarIdent, new named_type_reference("$yield_element_type"), yn.ex);
            else
                vs = new var_statement(VarIdent, yn.ex);
            vs.source_context = yn.ex.source_context;
            ReplaceStatement(yn, SeqStatements(vs, new yield_node(VarIdent, yn.ex.source_context)));
        }

        public override void visit(yield_sequence_node yn)
        {
            var VarIdent = this.NewVarName();
            VarIdent.source_context = yn.ex.source_context;

            var_statement vs;
            if (yn.ex is nil_const)
                vs = new var_statement(VarIdent, new named_type_reference("System.Object"), yn.ex); // bug fix #246
            else
                vs = new var_statement(VarIdent, yn.ex);


            //var_statement VS = new var_statement(VarIdent, yn.ex) { source_context = yn.ex.source_context };
            ReplaceStatement(yn, SeqStatements(vs, new yield_sequence_node(VarIdent, yn.ex.source_context)));
        }
    }
}
