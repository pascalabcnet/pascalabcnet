using System;
using static System.Console;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler.Errors;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace ParsePABC1
{
    class Program
    {
        static syntax_tree_node ParseFile(string fname)
        {
            Compiler c = new Compiler();

            var err = new List<Error>();

            var txt = System.IO.File.ReadAllText(fname);

            var cu = c.ParseText(fname, txt, err);

            if (cu == null)
            {
                if (err.Count > 0)
                    WriteLine(err[0]);
                else WriteLine("Не распарсилось");
            }
            return cu;
        }

        static void Main(string[] args)
        {
            try
            {
                var cu = ParseFile(@"..\..\..\yield2.pas");
                if (cu == null)
                    return;

                /*cu.visit(new ChangeWhileVisitor());
                cu.visit(new DeleteRedundantBeginEnds());*/

                /*cu.visit(new CollectUpperNamespacesVisitor());

                var allv = new AllVarsInProcYields();
                cu.visit(allv);
                allv.PrintDict();*/

                /*var cnt = new CountNodesVisitor();
                cu.visit(cnt);
                cnt.PrintSortedByValue();*/

                /*var ld = new HashSet<string>();
                ld.Add("a");
                ld.Add("p1");
                ld.Add("s");
                var dld = new DeleteLocalDefs(ld);
                cu.visit(dld);*/

                var p = new ProcessYieldCapturedVarsVisitor();
                cu.visit(p);

                /*var p = new CalcConstExprs();
                cu.visit(p);*/

                cu.visit(new SimplePrettyPrinterVisitor());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
