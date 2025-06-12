using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyntaxVisitors.PatternsVisitors
{
    public class ExtendedIsDesugaringVisitor : BaseChangeVisitor
    {
        public static ExtendedIsDesugaringVisitor New => new ExtendedIsDesugaringVisitor();

        public override void visit(if_node _if_node)
        {
            var condition = _if_node.condition;
            if (condition is bin_expr binEpxr && HasExtendedIs(binEpxr))
            {
                if_node rightExprIfNode = null;
                if_node leftExprIfNode = null;
                if (binEpxr.operation_type == Operators.LogicalAND)
                {
                    rightExprIfNode = new if_node(binEpxr.right, (statement)_if_node.then_body?.Clone(), (statement)_if_node.else_body?.Clone(), _if_node.source_context);
                    leftExprIfNode = new if_node(binEpxr.left, rightExprIfNode, (statement)_if_node.else_body?.Clone(), _if_node.source_context);
                }
                else if (binEpxr.operation_type == Operators.LogicalOR)
                {
                    var rightExprThenBody = _if_node.else_body == null ? new empty_statement() : (statement)_if_node.else_body.Clone();
                    rightExprIfNode = new if_node(
                        new un_expr(binEpxr.right, Operators.LogicalNOT, binEpxr.source_context),
                        rightExprThenBody,
                         (statement)_if_node.then_body?.Clone(),
                        _if_node.source_context);

                    leftExprIfNode = new if_node(
                        new un_expr(binEpxr.left, Operators.LogicalNOT, binEpxr.source_context),
                        rightExprIfNode,
                        (statement)_if_node.then_body?.Clone(),
                        _if_node.source_context);
                }
                ReplaceUsingParent(_if_node, leftExprIfNode);
                visit(leftExprIfNode);
                visit(rightExprIfNode);
            }
        }

        protected bool HasExtendedIs(bin_expr expr)
        {
            if (expr.left is bin_expr leftBinExpr &&
                expr.right is bin_expr rightBinExpr)
            {
                return HasExtendedIs(leftBinExpr) || HasExtendedIs(rightBinExpr);
            }
            if (expr.left is is_pattern_expr ||
                expr.right is is_pattern_expr)
            {
                return true;
            }
            if (expr.left is bin_expr lBinExpr)
            {
                return HasExtendedIs(lBinExpr);
            }
            if (expr.right is bin_expr rBinExpr)
            {
                return HasExtendedIs(rBinExpr);
            }
            return false;
        }
    }
}