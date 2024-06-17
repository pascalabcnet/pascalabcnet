using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyntaxVisitors.Async
{
    // Визитор, который заменяет все await 
    // на await.GetResult
    internal class OperationsVisitor : BaseChangeVisitor
    {
        public static bool HasAwait = false;
        public static expression Expr = new expression();
        public static OperationsVisitor New
        {
            get { return new OperationsVisitor(); }
        }

        public static void Accept(if_node if_nd)
        {
            if (if_nd == null)
            {
                return;
            }
            if (if_nd.condition[0] is bin_expr)
            {
                New.ProcessNode(if_nd.condition[0] as bin_expr);
            }
            if (if_nd.condition is un_expr)
            {
                New.ProcessNode(if_nd.condition);
            }
            
        }
        public static void Accept(var_def_statement vds)
        {
            if (vds == null)
            {
                return;
            }
            if (vds.inital_value is bin_expr)
            {
                New.ProcessNode(vds.inital_value as bin_expr);
            }

        }
        public static void Accept(assign a)
        {
            if (a == null)
            {
                return;
            }
            New.ProcessNode(a.from);

            if (a.from is await_node)
            {
                var aw = a.from as await_node;
                aw.ex = Expr;
            }
            New.ProcessNode(a.to);

        }

        public override void visit(un_expr un_Expr)
        {
            ProcessNode(un_Expr.subnode);

            if (un_Expr.subnode is un_expr)
            {
                var us = un_Expr.subnode as un_expr;
                us[0] = Expr;
            }
            else
            {
                if (un_Expr.subnode is bin_expr)
                {
                    ProcessNode(un_Expr.subnode);
                }
                else
                un_Expr.subnode = Expr;
            }

        }
        public override void visit(bin_expr b_epxr)
        {
            ProcessNode(b_epxr.left);
            if (b_epxr.left is await_node)
            {
                b_epxr.left = Expr;
            }
            ProcessNode(b_epxr.right);
            if (b_epxr.right is await_node)
            {
                b_epxr.right = Expr;
            }
        }
        public override void visit(await_node a)
        {
            var mmc = new method_call();
            mmc.source_context = a.source_context;
            if (a.ex is method_call)
                mmc.dereferencing_value = AwaitBuilder.GenGetResult(new dot_node(a.ex as method_call,
                    new ident("GetAwaiter", a.source_context)),a);
            else
                mmc.dereferencing_value = AwaitBuilder.GenGetResult(new dot_node(new ident(a.ex.ToString(), a.source_context),
                     new ident("GetAwaiter", a.source_context)), a);
            Expr = mmc;
             HasAwait = true;
        }
    }
}
