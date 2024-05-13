using PascalABCCompiler.SyntaxTree;

namespace Languages.Pascal.Frontend.Core
{
    public class Union
    {
        public expression ex;
        public ident id;
        public object ob;
        public op_type_node op;
        public syntax_tree_node stn;
        public token_info ti;
        public type_definition td;
    }
}
