// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;

namespace PascalABCCompiler.SyntaxTree
{
    class InvalidIntegerExpression : Exception
    {
    }
    public class ConstantExpressionToInt32ConstantConvertor : AbstractVisitor
    {
        Stack<int> intStack = new Stack<int>();
        public int32_const Convert(expression expr)
        {
            if (expr == null)
                return null;
            try
            {
                expr.visit(this);
            }
            catch
            {
                return null;
            }
            if (intStack.Count == 1)
            {
                int32_const i = new int32_const(intStack.Pop());
                i.source_context = expr.source_context;
                return i;
            }
            return null;
        }
        public override void visit(int32_const _int32_const)
        {
            intStack.Push(_int32_const.val);
        }
        public override void visit(un_expr node)
        {
            node.visit(this);
            int val = intStack.Pop();
            switch (node.operation_type)
            {
                case Operators.Minus:
                    intStack.Push(-val);
                    break;
                case Operators.Plus:
                    intStack.Push(val);
                    break;
                case Operators.BitwiseNOT:
                    intStack.Push(~val);
                    break;
                case Operators.LogicalNOT:
                    intStack.Push(val == 0 ? 1 : 0);
                    break;
                default:
                    throw new InvalidIntegerExpression();
            }
        }
        public override void visit(bin_expr node)
        {
            node.left.visit(this);
            int left = intStack.Pop();
            node.right.visit(this);
            int right = intStack.Pop();
            switch (node.operation_type)
            {
                case Operators.Minus:
                    intStack.Push(left - right);
                    break;
                case Operators.Plus:
                    intStack.Push(left + right);
                    break;
                case Operators.Multiplication:
                    intStack.Push(left * right);
                    break;
                case Operators.Division:
                    intStack.Push(left / right);
                    break;
                case Operators.Greater:
                    intStack.Push(left > right ? 1 : 0);
                    break;
                case Operators.Less:
                    intStack.Push(left < right ? 1 : 0);
                    break;
                case Operators.GreaterEqual:
                    intStack.Push(left >= right ? 1 : 0);
                    break;
                case Operators.LessEqual:
                    intStack.Push(left <= right ? 1 : 0);
                    break;
                case Operators.Equal:
                    intStack.Push(left == right ? 1 : 0);
                    break;
                case Operators.NotEqual:
                    intStack.Push(left != right ? 1 : 0);
                    break;
                case Operators.BitwiseLeftShift:
                    intStack.Push(left << right);
                    break;
                case Operators.BitwiseRightShift:
                    intStack.Push(left >> right);
                    break;
                case Operators.BitwiseAND:
                    intStack.Push(left & right);
                    break;
                case Operators.BitwiseOR:
                    intStack.Push(left | right);
                    break;
                case Operators.BitwiseXOR:
                    intStack.Push(left ^ right);
                    break;      
                default:
                    throw new InvalidIntegerExpression();
            }
        }
    }
}
