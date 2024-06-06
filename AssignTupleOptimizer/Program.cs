using System;

using System.IO;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors.SugarVisitors;
using Languages.Pascal.Frontend.Wrapping;
using System.Collections.Generic;

namespace AssignTupleDesugar
{
    class Program
    {
        public static void testFile(string filename, PascalABCNewLanguageParser parser)
        {
            var sr = new StreamReader(filename);
            string text = sr.ReadToEnd();
            
            var root = parser.GetCompilationUnit(filename, text, new List<PascalABCCompiler.Errors.Error>(), new List<PascalABCCompiler.Errors.CompilerWarning>(),  ParseMode.Normal);
            root.FillParentsInAllChilds();
           
           var binder = new BindCollectLightSymInfo(root as compilation_unit);
           binder.ProcessNode(root);

            var visitor = new BindTestVisitor(binder);
            // visitor.ProcessNode(root);
            var assign_visitor = new NewAssignTuplesDesugarVisitor(binder);
            assign_visitor.ProcessNode(root);

            var pp_visitor = new SyntaxVisitors.SimplePrettyPrinterVisitor();
            pp_visitor.ProcessNode(root);
        }
 
        public static void Main()
        {
            
            string name = "../../comp/c3.pas";
            var parser = new PascalABCNewLanguageParser();

            testFile(name, parser);
           
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
                var info = res.symInfo;
                var p = res.path;
                Console.WriteLine("(" + id.source_context.ToString() + ")" + " found: " +  info.ToString() + ", kind: " + info.SK.ToString() + ", (" + info.Id.source_context.ToString() + ")");
                Console.Write("Path: ");
                foreach (var s in p)
                {
                    Console.Write(s.ToString() +"->");
                }
                
            }
            Console.WriteLine();
       }
    }
}
