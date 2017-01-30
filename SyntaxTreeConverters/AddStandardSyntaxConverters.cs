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
