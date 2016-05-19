using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler.Errors;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.ParserTools;

using SyntaxVisitors;

namespace ParsePABC1
{
    class Program
    {
        // PascalABCSaushkinParser reference needed!!! DO NOT REMOVE!!!

        static syntax_tree_node ParseFile(string fname)
        {
            Compiler c = new Compiler();
           // c.SyntaxTreeChanger = new YieldDesugarSyntaxTreeConverter();
            var opts = new CompilerOptions(fname, CompilerOptions.OutputType.ConsoleApplicaton);
            
            var res = c.Compile(opts);

            var err = new List<Error>();

            var txt = System.IO.File.ReadAllText(fname);

            var cu = c.ParsersController.Compile(fname, txt, err, PascalABCCompiler.Parsers.ParseMode.Normal);

            if (cu == null)
            {
                Console.WriteLine("Не распарсилось");
            }
            return cu;
        }

        static void Main(string[] args)
        {
            var cu = ParseFile(@"D:\PascalABC.NET\!PABC_Git\Yield\tests\basic\yieldSimpleMethodDef.pas");
            if (cu == null)
                return;

            var yieldVis = new ProcessYieldCapturedVarsVisitor();
            cu.visit(yieldVis);


            cu.visit(new SimplePrettyPrinterVisitor());

            Console.ReadKey();
        }
    }
}
