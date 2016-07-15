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
    public class ReplaceYieldWithLamdasVisitor : BaseChangeVisitor
    {
        private int _lambdaNum = 0;

        private ident NewLamdaVarName()
        {
            ++_lambdaNum;
            return new ident("$lambdaVar$" + _lambdaNum);
        }

        public static ReplaceYieldWithLamdasVisitor New
        {
            get { return new ReplaceYieldWithLamdasVisitor(); }
        }

        public static void Accept(procedure_definition pd)
        {
            New.ProcessNode(pd);
        }

        public override void visit(yield_node yn)
        {
            var lambdaSearcher = new TreeConverter.LambdaExpressions.LambdaSearcher(yn);
            if (lambdaSearcher.CheckIfContainsLambdas())
            {
                var lambdaVarIdent = this.NewLamdaVarName();
                var_statement lambdaVS = new var_statement(lambdaVarIdent, yn.ex) { source_context = yn.source_context };
                ReplaceStatement(yn, SeqStatements(lambdaVS, new yield_node(lambdaVarIdent, yn.source_context)));
            }
        }
    }
}
