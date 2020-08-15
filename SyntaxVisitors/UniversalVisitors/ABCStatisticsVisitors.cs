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

    public class ABCStatisticsVisitor : WalkingVisitorNew  
    {
        public int InBlockVarDefs => 0;
        public int OutBlockVarDefs => 0;
        public int ForsWithoutVar => 0;
        public int WithProgramKeyword => 0;
        public bool OldStrings => false;
        public bool StaticArrays => false;
        public bool ReadProc => false;
        public ABCStatisticsVisitor()
        {
        }

       /* public override void visit(ident i)
        {
        }
        public override void visit(dot_node dn)
        {
            ProcessNode(dn.left);
            if (dn.right.GetType() != typeof(ident))
                ProcessNode(dn.right);
        }*/
        /*public override void visit(function_lambda_definition fd)
        {            
        }*/
    }

}
