using System.Collections.Generic;
using AssignTupleDesugarAlgorithm;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors.SugarVisitors;

namespace AssignTupleDesugar
{
    public class NewAssignTuplesDesugarVisitor : AssignTuplesDesugarVisitor
    {

        BindCollectLightSymInfo binder;

        public NewAssignTuplesDesugarVisitor(BindCollectLightSymInfo binder)
        {
            this.binder = binder; 
        }

        
        // (a[i], a[j], a[k]) = (a[j], a[k], a[i])
        //temp = a[i]
        //temp = a[j]
        //a[j] = a[k]
        //a[k] = temp
        //a[i] = temp
        List<statement> desugar(tuple_node tn, addressed_value_list vars)
        {

            var order = Assign.getAssignOrder(tn, vars, binder);
            var assigns = new List<statement>();
            foreach (var a in order)
            {
                //parents?
                if (a.to is TempSymbol ts)
                {
                    var cur = new var_def_statement(ts.node as ident, a.from.node, tn.Parent.source_context);
                    assigns.Add(new var_statement(cur));
                }
                else
                {
                    var cur = new assign(a.to.node as addressed_value, a.from.node, tn.Parent.source_context);
                    assigns.Add(cur);
                }
            }
            return assigns;
        }

        //a[i]
        //(a[i], a[j]) = (a[j], a[i])
        //(a, a[i]) = (b, b[j])
        //indexer, dot_node
        //get_address, roof_dereference
        public override void visit(assign_tuple node)
        {
            if (node.expr is tuple_node tn)
            {
                var assigns = desugar(tn, node.vars);
                ReplaceStatementUsingParent(node, assigns);
            }
        }
    }
}
