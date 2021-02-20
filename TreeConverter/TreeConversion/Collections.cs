// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;

using PascalABCCompiler.Collections;

namespace PascalABCCompiler.TreeRealization
{
    [Serializable]
    public class common_function_node_stack : stack<common_function_node>
    {
        public common_function_node_stack Clone()
        {
            var s = new common_function_node_stack();
            s.internal_stack = this.CloneInternalStack();
            return s;
        }
    }

    [Serializable]
    public class var_definition_node_list : extendable_collection<var_definition_node>
    {
    }

    [Serializable]
    public class statement_list_stack : stack<statements_list>
    {
        public statement_list_stack Clone()
        {
            var s = new statement_list_stack();
            s.internal_stack = this.CloneInternalStack();
            return s;
        }
    }
	
	[Serializable]
    public class lambda_stack : stack<lambda_node> //lroman//
    {
        public lambda_stack Clone()
        {
            var s = new lambda_stack();
            s.internal_stack = this.CloneInternalStack();
            return s;
        }
    }
}