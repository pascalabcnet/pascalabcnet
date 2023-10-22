using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PascalABCCompiler.PascalABCNewParser;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.SyntaxTree;

namespace AssignTupleOptimizer
{
    class Program  
    {
        public static void Main()
        {
            string name = "../../simple_test.pas";
            var parser = new PascalABCNewLanguageParser();
            
            StreamReader sr = new StreamReader(name);
            string text = sr.ReadToEnd();
            var root = parser.BuildTree(name, text, ParseMode.Normal);
            
            var visitor = new AssignTupleOptimizerVisitor();
            visitor.ProcessNode(root);
            Console.WriteLine();
            
            var pp = new SyntaxVisitors.SimplePrettyPrinterVisitor();
            pp.visit(root);
            Console.ReadKey();
        }
    }
}
