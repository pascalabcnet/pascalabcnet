using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PascalABCCompiler.PascalABCNewParser;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.SyntaxTree;

namespace AssignTupleDesugar
{
    class Program
    {
        static  Dictionary<string, ScopeSyntax> scopes = new Dictionary<string, ScopeSyntax>();

        static ScopeSyntax getScopeForUnit(string name) => scopes[name];

        static void addScope(string name, ScopeSyntax scope)
        {
            scopes.Add(name, scope);
        }

        public static void Main()
        {
            string unitName = "../../Test.pas";
            string name = "../../test_units.pas";
            var parser = new PascalABCNewLanguageParser();
            
            StreamReader sr = new StreamReader(unitName);
            string unitText = sr.ReadToEnd();

            var sr2 = new StreamReader(name);
            string text = sr2.ReadToEnd();


            var unitRoot = parser.BuildTree(unitName, unitText, ParseMode.Normal);
            unitRoot.FillParentsInAllChilds();

            var root = parser.BuildTree(name, text, ParseMode.Normal);
            root.FillParentsInAllChilds();
            
            var unitBinder = new BindCollectLightSymInfo(unitRoot as compilation_unit, getScopeForUnit, addScope);
            var binder = new BindCollectLightSymInfo(root as compilation_unit, getScopeForUnit, addScope);



            var unitVisitor = new BindTestVisitor(unitBinder);
            unitVisitor.ProcessNode(unitRoot);

            var visitor = new BindTestVisitor(binder);
            visitor.ProcessNode(root);

            Console.WriteLine();
            
            //var pp = new SyntaxVisitors.SimplePrettyPrinterVisitor();
            //pp.visit(unitRoot);
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
