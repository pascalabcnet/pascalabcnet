using System;

using PascalABCCompiler.TreeConverter;

namespace PascalABCCompiler.Collections
{

    public class returner<T,ST> where ST : SyntaxTree.tree_node
    {
        private stack<T> internal_stack = new stack<T>();
        private syntax_tree_visitor stg;

        public returner(syntax_tree_visitor _stg)
        {
            this.stg = _stg;
        }

        public T visit(ST value)
        {
#if (DEBUG)
            int stack_size = internal_stack.size;
#endif

            value.visit(stg);

#if (DEBUG)
            if (internal_stack.size != stack_size + 1)
            {
                throw new InvalidStackState();
            }
#endif

            T en = internal_stack.pop();
            return en;
        }

        public void return_value(T value)
        {
            internal_stack.push(value);
        }
    }

}
