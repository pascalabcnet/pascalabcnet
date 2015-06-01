// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;

namespace PascalABCCompiler.TreeRealization
{
    public class throw_statement_node : statement_node, SemanticTree.IThrowNode
    {
        private expression_node _excpetion;

        public throw_statement_node(expression_node exception,location loc) :
            base(loc)
        {
            _excpetion = exception;
        }

        public expression_node excpetion
        {
            get
            {
                return _excpetion;
            }
        }

        public SemanticTree.IExpressionNode exception_expresion
        {
            get
            {
                return _excpetion;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.throw_statement;
            }
        }

        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }
    }
    
    public class rethrow_statement_node : statement_node, SemanticTree.IRethrowStatement
    {

        public rethrow_statement_node(location loc) :
            base(loc)
        {
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.rethrow_statement;
            }
        }

        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }
    }
}
