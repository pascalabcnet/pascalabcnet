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
        public string ExprType = "";
        public static OperationsVisitor New
        {
            get { return new OperationsVisitor(); }
        }

        public static void Accept(if_node if_nd)
        {
            if (if_nd == null)
                return;
            if (if_nd.condition[0] is bin_expr)
            {
                New.ProcessNode(if_nd.condition[0] as bin_expr);
                return;
            }
            if (if_nd.condition is bin_expr)
            {
                New.ProcessNode(if_nd.condition as bin_expr);
                return;
            }
            if (if_nd.condition is un_expr)
            {
                New.ProcessNode(if_nd.condition);
                return;
            }
            
        }
        public static void Accept(var_def_statement vds)
        {
            if (vds == null)
                return;
            if (vds.inital_value is bin_expr)
            {
                New.ProcessNode(vds.inital_value as bin_expr);
            }
            if (vds.inital_value is un_expr)
            {
                New.ProcessNode(vds.inital_value as un_expr);
            }

        }
        public static void Accept(assign a)
        {
            if (a == null)
                return;
            New.ProcessNode(a.from);
        }

        public override void visit(un_expr un_Expr)
        {
            ProcessNode(un_Expr.subnode);
        }
        public override void visit(bin_expr b_epxr)
        {
            ExprType = "left";
            ProcessNode(b_epxr.left);
            ExprType = "right";
            ProcessNode(b_epxr.right);
        }
        public override void visit(await_node a)
        {
            if (a.Parent == null)
                return; 
            var mmc = new method_call();
            mmc.source_context = a.source_context;
            if (a.ex is method_call)
                mmc.dereferencing_value = AwaitBuilder.GenGetResult(new dot_node(a.ex as method_call,
                    new ident("GetAwaiter", a.source_context)),a);
            else
                mmc.dereferencing_value = AwaitBuilder.GenGetResult(new dot_node(new ident(a.ex.ToString(), a.source_context),
                     new ident("GetAwaiter", a.source_context)), a);
            if (a.Parent is bin_expr)
            {
                if (ExprType == "left")
                    (a.Parent as bin_expr).left = mmc;
                if (ExprType == "right")
                    (a.Parent as bin_expr).right = mmc;
            }
            if (a.Parent is un_expr)
            {
                (a.Parent as un_expr).subnode = mmc;
            }
            if (a.Parent is assign)
            {
                (a.Parent as assign).from = mmc;
            }
             HasAwait = true;
        }
    }
}
