using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class AllVarsInProcYields : CollectUpperNamespacesVisitor 
    {
        public FindMainIdentsVisitor mids = new FindMainIdentsVisitor();

        public Dictionary<procedure_definition, ISet<string>> d = new Dictionary<procedure_definition, ISet<string>>();

        public int countNodesVisited;

        public AllVarsInProcYields()
        {
            PrintInfo = false; 
        }

        public override void Enter(syntax_tree_node st)
        {
            base.Enter(st);
            countNodesVisited++;
            if (st is procedure_definition)
            {
                // пока вложенные процедуры не анализируются, хотя надо
                mids.vars.Clear();
            }

            // сокращение обходимых узлов. Как сделать фильтр по тем узлам, которые необходимо обходить? Например, все операторы (без выражений и описаний), все описания (без операторов)
            if (st is assign || st is var_def_statement || st is procedure_call || st is procedure_header || st is expression)
            {
                visitNode = false;
            }
        }
        public override void Exit(syntax_tree_node st)
        {
            if (st is procedure_definition)
            {
                if (mids.vars.Count>0)
                {
                    d[st as procedure_definition] = new HashSet<string>(mids.vars);
                }
                var fld = new FindLocalDefsVisitor();
                st.visit(fld);
                fld.Print();
                var t = fld.ids.Intersect(mids.vars); // идентификаторы, захваченные из локального контекста
                
            }
            base.Exit(st);
        }
        public override void visit(yield_node yn)
        {
            yn.visit(mids);
            // mids.vars - надо установить, какие из них - локальные, какие - из этого класса, какие - являются параметрами функции, а какие - глобальные (все остальные)
            // те, которые являются параметрами, надо скопировать в локальные переменные и переименовать использование везде по ходу данной функции 
            // самое сложное - переменные-поля этого класса - они требуют в создаваемом классе, реализующем итератор, хранить Self текущего класса и добавлять это Self везде по ходу алгоритма
            // вначале будем считать, что переменные-поля этого класса и переменные-параметры не захватываются yield
            //base.visit(yn);
        }

        public void PrintDict()
        {
            foreach (var a in d)
            {
                Console.Write("{0}: {1} ", a.Key.proc_header.name.meth_name, a.Value.Count);
                foreach (var v in a.Value)
                    Console.Write("{0}, ", v);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("countNodesVisited={0}", countNodesVisited);
            } 

        }
    }
}
