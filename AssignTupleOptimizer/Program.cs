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
            string name = "../../test5.pas";
            var parser = new PascalABCNewLanguageParser();
            
            StreamReader sr = new StreamReader(name);
            string text = sr.ReadToEnd();
            var root = parser.BuildTree(name, text, ParseMode.Normal);

            var binder = new BindCollectLightSymInfo(root as program_module);

            root.FillParentsInAllChilds();

            var varVisitor = new PascalABCCompiler.SyntaxTreeConverters.VarNamesInMethodsWithSameNameAsClassGenericParamsReplacer(root as program_module);
            varVisitor.ProcessNode(root);

            var visitor = new BindTestVisitor(binder);
            visitor.ProcessNode(root);
            Console.WriteLine();
            
            var pp = new SyntaxVisitors.SimplePrettyPrinterVisitor();
            pp.visit(root);
            Console.ReadKey();
        }
    }


    public class BindTestVisitor : BaseEnterExitVisitor
    {

        BindCollectLightSymInfo binder;
        
        public BindTestVisitor(BindCollectLightSymInfo binder)
        {
            this.binder = binder;
        }

     

       public override void visit(ident id)
        {

            var res = binder.bind(id);
            Console.Write(id.name);
            if(res == null)
            {
                Console.WriteLine(" not found");
            }
            else
            {
                Console.WriteLine("(" + id.source_context.ToString() + ")" + " found: " +  res.ToString() + ", kind: " + res.SK.ToString() + ", (" + res.Id.source_context.ToString() + ")"); ;
            }
            Console.WriteLine();

        }
    }
}
