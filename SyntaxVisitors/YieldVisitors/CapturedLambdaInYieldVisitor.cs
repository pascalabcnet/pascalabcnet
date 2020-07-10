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
    public class CapturedLambdaInYieldVisitor : BaseChangeVisitor
    {

        public CapturedLambdaInYieldVisitor()
        {
        }

        public static CapturedLambdaInYieldVisitor New
        {
            get { return new CapturedLambdaInYieldVisitor(); }
        }

        public static void Accept(procedure_definition pd)
        {
            New.ProcessNode(pd);
        }

        int ynum = 0;
        public string CreateNameForLambdaInYield()
        {
            ynum += 1;
            return "@RenLamInYield" + ynum + "$";
        }

        public override void visit(yield_node yn)
        {
            if (yn.ex is function_lambda_definition)
            {
                syntax_tree_node sn = yn;
                do
                {
                    sn = sn.Parent;
                } while (sn != null && !(sn is procedure_definition));
                procedure_definition pd = sn as procedure_definition;

                if (sn == null)
                {
                    // этого не будет
                }
                var fh = pd.proc_header as function_header;
                if (fh == null)
                {
                    // этого тоже не будет
                }
                var sq = fh.return_type as sequence_type;
                if (sq == null)
                {
                    // и этого не будет
                }

                var lst = new List<statement>();
                var newid = new ident(CreateNameForLambdaInYield(), yn.ex.source_context);
                var vs = new var_statement(newid, sq.elements_type, yn.ex);
                vs.source_context = yn.ex.source_context;
                var newyn = new yield_node(newid, newid.source_context);
                lst.Add(vs);
                lst.Add(newyn);
                ReplaceStatementUsingParent(yn, lst);
            }
        }


    }
}
