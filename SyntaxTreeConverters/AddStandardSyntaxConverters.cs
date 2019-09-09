// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PascalABCCompiler.SyntaxTreeConverters
{
    
    class StandardSyntaxConverters: ISyntaxTreeConverter
    {
        public string Name { get; } = "Standard";
        public syntax_tree_node Convert(syntax_tree_node root)
        {
            //var v = new SampleVisitor();
            //v.ProcessNode(root);
            return root;
        }
    }
}
