using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

using SyntaxVisitors;

using PascalABCCompiler.SyntaxTreeConverters;

namespace YieldDesugarSyntaxTreeConverter
{
    public class YieldDesugarSyntaxTreeConverter : ISyntaxTreeConverter
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }

        public ConverterType ConverterType { get; set; }
        public ExecutionOrder ExecutionOrder { get; set; }
        public syntax_tree_node Convert(syntax_tree_node root)
        {
            root.visit(new MarkMethodHasYieldVisitor());
            root.visit(new ProcessYieldCapturedVarsVisitor());

#if DEBUG
            //root.visit(new SimplePrettyPrinterVisitor(@"d:\zzz.txt"));
#endif

            return root;
        }

        /*public void Change(syntax_tree_node sn)
        {
            //sn.visit(new CalcConstExprs());
            //sn.visit(new LoweringVisitor());
            sn.visit(new ProcessYieldCapturedVarsVisitor());

        }*/

    }
}
