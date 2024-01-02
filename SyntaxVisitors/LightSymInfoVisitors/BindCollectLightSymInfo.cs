﻿

namespace PascalABCCompiler.SyntaxTree
{
   public class BindCollectLightSymInfo : CollectLightSymInfoVisitor 
    {
        protected override AbstractScopeCreator scopeCreator => _scopeCreator;

        private CachingScopeCreator _scopeCreator = new CachingScopeCreator();

        public BindCollectLightSymInfo(compilation_unit root) : base(root)
        {
            Current = Root;
            visit(root);
        }

        public SymInfoSyntax bind(ident node)
        {
            ///Console.WriteLine("searching " + node.ToString());
            syntax_tree_node cur_node = node;
            while (cur_node != null)
            {
               // Console.WriteLine("cur_node: " + cur_node.ToString());

                if (AbstractScopeCreator.IsScopeCreator(cur_node))
                {

                    //Console.WriteLine("checking scope" + cur_node.ToString())
                    var cur_scope = scopeCreator.GetScope(cur_node);
                    //Console.WriteLine(cur_scope.ToString() + ": " + cur_scope.Symbols.Count);
                    var prev_scope = Current;
                    
                    if (prev_scope != null)
                        prev_scope.Parent = cur_scope;

                    Current = cur_scope;
                    if (cur_scope.Parent == null && !(Current is GlobalScopeSyntax))
                    {
                        //Console.WriteLine("visiting " + cur_node.ToString());
                        cur_node.visit(this);
                    }
                    var res = Current.bind(node);
                    if (res != null) return res;
                }
                cur_node = cur_node.Parent;
            }
            return null;
        }

        public override void Enter(syntax_tree_node st)
        {
            if (AbstractScopeCreator.IsScopeCreator(st))
            {
                if (st is procedure_definition pd)
                {
                    var attr = pd.proc_header.class_keyword ? SymbolAttributes.class_attr : 0;
                    var name = pd.proc_header?.name?.meth_name;
                    if (name != null)
                    if (pd.proc_header is function_header)
                        Current.AddSymbol(name, SymKind.funcname, null, attr);
                    else
                        Current.AddSymbol(name, SymKind.procname, null, attr);
                }

                visitNode = false;
            }
        }

        public override void PreExitScope(syntax_tree_node st){}
    }
}
