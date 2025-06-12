using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PascalABCCompiler.SyntaxTree
{
    public class CollectFullLightSymInfoVisitor : CollectLightSymInfoVisitor
    {
        protected override AbstractScopeCreator scopeCreator => _scopeCreator;

        private CachingScopeCreator _scopeCreator = new CachingScopeCreator();

        public CollectFullLightSymInfoVisitor(compilation_unit root) : base(root)
        {
            
        }

        protected CollectFullLightSymInfoVisitor() : base() { }

        public override void Enter(syntax_tree_node st)
        {
            var scope = scopeCreator.GetScope(st);
            if (scope != null)
            {
                scope.Parent = Current;
                Current = scope;
            }
        }

        public override void Exit(syntax_tree_node st)
        {
            if (AbstractScopeCreator.IsScopeCreator(st))
            {
                PreExitScope(st);
                if (Current != null)
                    Current = Current.Parent;
            }
        }

        public override void visit(procedure_definition node)
        {
            var name = node.proc_header?.name?.meth_name;
            var attr = node.proc_header.class_keyword ? SymbolAttributes.class_attr : 0;
            if (name != null)
                if (node.proc_header is function_header)
                    Current.Parent.AddSymbol(name, SymKind.funcname, null, attr);
                else
                    Current.Parent.AddSymbol(name, SymKind.procname, null, attr);

            base.visit(node);
        } 

        public override void PreExitScope(syntax_tree_node st)
        {
   
        }

       
    }
}
