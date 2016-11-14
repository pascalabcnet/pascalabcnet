using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;

using SyntaxVisitors;

using PascalABCCompiler.SyntaxTreeConverters;

namespace YieldDesugarSyntaxTreeConverter
{
    public class YieldDesugarSyntaxTreeConverter : ISyntaxTreeConverter
    {
        public string Name { get; } = "YieldDesugar";
        public syntax_tree_node Convert(syntax_tree_node root)
        {
            root.visit(new MarkMethodHasYieldAndCheckSomeErrorsVisitor());
            ProcessYieldCapturedVarsVisitor.New.ProcessNode(root);

#if DEBUG
            try
            {
               //root.visit(new SimplePrettyPrinterVisitor(@"d:\\zzz1.txt"));
            }
            catch 
            {

            }
            
#endif

            return root;
        }
    }
}
