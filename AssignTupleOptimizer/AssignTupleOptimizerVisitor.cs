﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors.SugarVisitors;

namespace AssignTupleOptimizer
{
    public class AssignTupleOptimizerVisitor : AssignTuplesDesugarVisitor
    {

        BindCollectLightSymInfo binder;

        public AssignTupleOptimizerVisitor(BindCollectLightSymInfo binder)
        {
            this.binder = binder; 
        }



        public override void visit(assign_tuple node)
        {
            if (node.expr is tuple_node tn)
            {
                List<Symbol> left = new List<Symbol>();
                List<Symbol> right = new List<Symbol>();
                foreach (var sym in node.vars.variables)
                {
                    if (sym is ident id)
                    {
                        var s = binder.bind(id);
                        if (s != null)
                        {
                            System.Console.WriteLine(id.name + " -> " + s);
                            left.Add(new Symbol(s.Id.name));
                        }
                        else
                        {
                            System.Console.WriteLine(id.name +" -> from outer scope");
                            left.Add(new Symbol(id.name) { fromOuterScope = true });
                        }
                    } 
                    else
                    {
                            left.Add( new Symbol("$expr") { isExpr = true  });
                    }
                }

                foreach (var sym in tn.el.expressions)
                {
                    if (sym is ident id)
                    {
                        var s = binder.bind(id);
                        if (s != null)
                        {
                            System.Console.WriteLine(id.name + " -> " + s);
                            right.Add(new Symbol(s.Id.name));
                        }
                        else
                        {
                            System.Console.WriteLine(id.name + " -> from outer scope");
                            right.Add(new Symbol(id.name) { fromOuterScope = true });
                        }
                    }
                    else
                    {
                        right.Add(new Symbol("$expr") { isExpr = true });
                    }
                }

                var order =Assign.getAssignOrder(left: left, right: right);
                var assigns = new List<statement>();
                foreach(var a in order)
                {
                    if (a.to is TempSymbol ts)
                    {
                        var cur = new var_def_statement(new ident(ts.name), new ident(a.from.name));
                        assigns.Add(new var_statement(cur));
                    }
                    else
                    {
                        var cur = new assign(new ident(a.to.name), new ident(a.from.name), node.source_context);
                        assigns.Add(cur);
                    }
                }
                ReplaceStatementUsingParent(node, assigns);
            }
        }
    }
}