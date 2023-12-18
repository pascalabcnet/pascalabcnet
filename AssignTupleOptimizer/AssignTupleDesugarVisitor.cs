using System.Collections.Generic;
using AssignTupleDesugarAlgorithm;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors.SugarVisitors;

namespace AssignTupleDesugar
{
    public class AssignTupleDesugarVisitor : AssignTuplesDesugarVisitor
    {

        BindCollectLightSymInfo binder;

        public AssignTupleDesugarVisitor(BindCollectLightSymInfo binder)
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
                            left.Add(new Symbol(s.Id.name) { fromOuterScope = isFromOuterScope(s)});
                        }
                        else
                        {
                            System.Console.WriteLine(id.name +" -> not found");
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
                            right.Add(new Symbol(s.Id.name) { fromOuterScope = isFromOuterScope(s) });
                        }
                        else
                        {
                            System.Console.WriteLine(id.name + " -> not found");
                            right.Add(new Symbol(id.name) { fromOuterScope = true });
                        }
                    }
                    else
                    {
                        right.Add(new Symbol("$expr") { isExpr = true });
                    }
                }

                var order = Assign.getAssignOrder(left: left, right: right);
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

        static bool isFromOuterScope(SymInfoSyntax symbol)
        {
            if (symbol == null)
                return false;

            if (symbol.SK == SymKind.var && symbol.Attr.HasFlag(SymbolAttributes.varparam_attr))
                return false;
            return true;
        }
    }
}
