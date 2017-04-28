using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;

using SyntaxVisitors;

using PascalABCCompiler.SyntaxTreeConverters;

namespace YieldDesugarSyntaxTreeConverter
{
    // Он перестал быть нужен, поскольку код YieldDesugarSyntaxTreeConverter.Convert был явно добавлен в StandardSyntaxTreeConverter.Convert
    // Со временем этот проект надо исключить из решения, а YieldConversionSyntax.dll - исключить из инсталлята
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
               //root.visit(new SimplePrettyPrinterVisitor(@"d:\\zzz3.txt"));
            }
            catch 
            {

            }
            
#endif

            return root;
        }
    }
}
