using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class FindLocalDefsVisitor : WalkingVisitorNew // Запускать только для подпрограмм
    {
        public ISet<string> ids = new HashSet<string>();
        private bool indef = false;
        public override void visit(ident id)
        {
            if (indef)
                ids.Add(id.name);
        }
        public override void visit(var_def_statement defs)
        {
            indef = true;
            ProcessNode(defs.vars); // исключаем типы - просматриваем только имена переменных
            indef = false;
        }

        public void Print()
        {
            foreach (var x in ids)
                Console.Write(x + ", ");
            Console.WriteLine();
        }
    }

    public class FindMainIdentsVisitor : WalkingVisitorNew  // в выражении yield это надо будет. 
    {
        public HashSet<string> vars = new HashSet<string>();
        public override void visit(ident id)
        {
            vars.Add(id.name);
        }
        public override void visit(dot_node dn)
        {
            ProcessNode(dn.left);
            if (dn.right.GetType() != typeof(ident))
                ProcessNode(dn.right);
        }
    }
}
