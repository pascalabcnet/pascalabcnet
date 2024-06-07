using System.Collections.Generic;
using System.Linq;
using AssignTupleDesugarAlgorithm;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.SugarVisitors
{
    public class NewAssignTuplesDesugarVisitor : AssignTuplesDesugarVisitor
    {

        private BindCollectLightSymInfo binder;

    
        public static NewAssignTuplesDesugarVisitor Create(BindCollectLightSymInfo binder)
        {
            return new NewAssignTuplesDesugarVisitor(binder);
        }

        public NewAssignTuplesDesugarVisitor(BindCollectLightSymInfo binder)
        {
            this.binder = binder; 
        }
     

        List<statement> desugar(tuple_node tn, addressed_value_list vars)
        {
            var order = Assign.getAssignOrder(tn, vars, binder);
            var assigns = new List<statement>();
            foreach (var a in order)
            {
                if (a.to is TempSymbol ts)
                {
                    var cur = new var_def_statement(ts.node as ident, a.from.node, tn.Parent.source_context);
                    assigns.Add(new var_statement(cur, tn.Parent.source_context));
                }
                else
                {
                    var cur = new assign(a.to.node as addressed_value, a.from.node, tn.Parent.source_context);
                    assigns.Add(cur);
                }
            }
            return assigns;
        }


        public override void visit(assign_tuple node)
        {
            if (node.expr is tuple_node tn)
            {
                var n = node.vars.variables.Count();
                if (n > tn.el.Count)
                    throw new SyntaxVisitorError("TOO_MANY_ELEMENTS_ON_LEFT_SIDE_OF_TUPLE_ASSIGNMENT", node.vars.variables[0]);

                var assigns = desugar(tn, node.vars);
                ReplaceStatementUsingParent(node, assigns);
            }
            else
                base.visit(node);
        }
        
        public override void visit(assign_var_tuple node)
        {
            if (node.expr is tuple_node tn)
            {
                var n = node.idents.idents.Count();
                if (n > tn.el.Count)
                    throw new SyntaxVisitorError("TOO_MANY_ELEMENTS_ON_LEFT_SIDE_OF_TUPLE_ASSIGNMENT", node.idents.idents[0]);

                var assigns = new List<statement>();
                for(var i =0; i < n; i++)
                {
                    var cur = new var_def_statement(node.idents.idents[i], tn.el.expressions[i], tn.Parent.source_context);
                    assigns.Add(new var_statement(cur, node.source_context));
                }
                ReplaceStatementUsingParent(node, assigns);
            }
            else
                base.visit(node);
            
        }

        public override void visit(var_tuple_def_statement node)
        {
            if (node.inital_value is tuple_node tn)
            {
                var n = node.vars.idents.Count();
                if (n > tn.el.Count)
                    throw new SyntaxVisitorError("TOO_MANY_ELEMENTS_ON_LEFT_SIDE_OF_TUPLE_ASSIGNMENT", node.vars.idents[0]);
                var assigns = new List<var_def_statement>();
                for (var i = 0; i < n; i++)
                {
                    var cur = new var_def_statement(node.vars.idents[i], tn.el.expressions[i], tn.Parent.source_context);
                    assigns.Add(cur);
                }
                ReplaceVarTupleDefStatementUsingParent(node, assigns);
            }
            else
                base.visit(node);

        }
    }
}
