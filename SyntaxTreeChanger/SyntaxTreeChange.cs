using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxTreeChanger
{
    public interface ISyntaxTreeChanger
    {
        void Change(syntax_tree_node root);
    }

    public class SyntaxTreeChange: ISyntaxTreeChanger
    {
        public void Change(syntax_tree_node root)
        {

        }
    }
}
