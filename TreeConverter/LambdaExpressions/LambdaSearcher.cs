// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using PascalABCCompiler.SyntaxTree;

namespace TreeConverter.LambdaExpressions
{
    public class LambdaSearcher : WalkingVisitorNew
    {
        private readonly syntax_tree_node _treeNodeToProcess;
        private class LambdaIsFound : Exception { }

        public function_lambda_definition FoundLambda
        {
            get; 
            private set;
        }

        public LambdaSearcher(syntax_tree_node syntaxTreeNode)
        {
            _treeNodeToProcess = syntaxTreeNode;
        }

        public override void visit(function_lambda_definition lambdaDef)
        {
            FoundLambda = lambdaDef;
            throw new LambdaIsFound();
        }

        public bool CheckIfContainsLambdas()
        {
            var lambdaIsFound = false;

            try
            {
                ProcessNode(_treeNodeToProcess);
            }
            catch (LambdaIsFound)
            {
                lambdaIsFound = true;
            }

            return lambdaIsFound;
        }
    }
}