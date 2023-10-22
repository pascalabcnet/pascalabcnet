using System;
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

        SymInfoFilterVisitor symInfoVisitor;

        ScopeSyntax Current;
        ScopeSyntax Root;

        public override void Enter(syntax_tree_node st)
        {
            base.Enter(st);
            if (AbstractLightScopeCreator.IsScopeCreator(st))
            {
                Current = symInfoVisitor?.map[st];
            }
        }

        public override void Exit(syntax_tree_node st)
        {
           
            if (AbstractLightScopeCreator.IsScopeCreator(st))
            {
                Current = Current?.Parent;
            }
            base.Exit(st);
        }

        public override void visit(program_module pm)
        {
            visit(pm.program_block.defs);
            var assign_count = new AssignTupleFilterVisitor();

            var code = pm.program_block.program_code;

            var root = new GlobalScopeSyntax();
            Root = root;

            assign_count.visit(code);
            if (assign_count.count >  0)
            {
                symInfoVisitor = new SymInfoFilterVisitor(assign_count.targets);
                symInfoVisitor.map.Add(pm, Root);
                symInfoVisitor.ProcessNode(code);
                visit(code);
            }

        }

        public override void visit(procedure_definition def)
        {
            var infoVisitor = new AssignTupleFilterVisitor();
            infoVisitor.visit(def);
            if (infoVisitor.count > 0)
            {
                symInfoVisitor = new SymInfoFilterVisitor(infoVisitor.targets);
                symInfoVisitor.visit(def);
                base.visit(def);
            }
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
                        var s = Current.bind(id);
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
                        var s = Current.bind(id);
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

       /* public SymInfoSyntax FindSymbol(ident id)
        {
            SymInfoSyntax res = null;
            var cur = Current;

            while (cur != null && res == null)
            {
                Console.WriteLine("Search in " + cur.ToString());
                foreach (var symbol in cur.Symbols)
                {
                    Console.WriteLine("checking symbol:" + symbol.ToString() + "names?: " + (symbol.Id.name == id.name).ToString()
                         + " source context?: " + id.source_context.Less(symbol.Id.source_context).ToString());
                    if (symbol.Id.name == id.name && symbol.Id.source_context.Less(id.source_context))
                    {
                        Console.WriteLine("Found!!");
                        res = symbol;
                        break;
                    }
                }
                cur = cur.Parent;
            }

            return res;
            
        }*/

    }
}
