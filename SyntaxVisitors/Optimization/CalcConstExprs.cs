using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class CalcConstExprs : BaseChangeVisitor // вычисление целых и вещественных константных выражений на этапе парсинга
    {
        public override void Exit(syntax_tree_node st)
        {
            bracket_expr bre = st as bracket_expr;
            if (bre != null)
            {
                if (bre.expr is int32_const)
                    Replace(st, bre.expr);
            }

            bin_expr vs = st as bin_expr;
            if (vs != null)
            {
                if (vs.left is int32_const && vs.right is int32_const)
                {
                    var a = vs.left as int32_const;
                    var b = vs.right as int32_const;
                    var op = vs.operation_type;

                    syntax_tree_node res;
                    switch (op)
                    {
                        case Operators.Plus:
                            res = new int32_const(a.val + b.val);
                            break;
                        case Operators.Minus:
                            res = new int32_const(a.val - b.val);
                            break;
                        case Operators.Multiplication:
                            res = new int32_const(a.val * b.val);
                            break;
                        case Operators.Division:
                            res = new double_const((double)a.val / b.val);
                            break;
                        case Operators.Greater:
                            res = new bool_const(a.val > b.val);
                            break;
                        case Operators.Less:
                            res = new bool_const(a.val < b.val);
                            break;
                        case Operators.GreaterEqual:
                            res = new bool_const(a.val >= b.val);
                            break;
                        case Operators.LessEqual:
                            res = new bool_const(a.val <= b.val);
                            break;
                        default:
                            res = vs;
                            break;
                    }
                
                    Replace(vs, res);
                }
                if (vs.left is int32_const && vs.right is double_const || vs.right is int32_const && vs.left is double_const || vs.left is double_const && vs.right is double_const)
                {
                    double x,y;
                    if (vs.left is int32_const)
                        x = (vs.left as int32_const).val;
                    else
                        x = (vs.left as double_const).val;
                    if (vs.right is int32_const)
                        y = (vs.right as int32_const).val;
                    else
                        y = (vs.right as double_const).val;

                    var op = vs.operation_type;

                    syntax_tree_node res;
                    switch (op)
                    {
                        case Operators.Plus:
                            res = new double_const(x + y);
                            break;
                        case Operators.Minus:
                            res = new double_const(x - y);
                            break;
                        case Operators.Multiplication:
                            res = new double_const(x * y);
                            break;
                        case Operators.Division:
                            res = new double_const(x / y);
                            break;
                        case Operators.Greater:
                            res = new bool_const(x > y);
                            break;
                        case Operators.Less:
                            res = new bool_const(x < y);
                            break;
                        case Operators.GreaterEqual:
                            res = new bool_const(x >= y);
                            break;
                        case Operators.LessEqual:
                            res = new bool_const(x <= y);
                            break;
                        default:
                            res = vs;
                            break;
                    }

                    Replace(vs, res);
                }

            }
            
            base.Exit(st); // это обязательно!
        }
    }
}
