using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class DeleteAllLocalDefs : BaseChangeVisitor
    {
        public List<var_def_statement> LocalDeletedDefs = new List<var_def_statement>(); // все локальные описания
        public HashSet<string> LocalDeletedDefsNames = new HashSet<string>();            // их имена - для быстрого поиска  

        public DeleteAllLocalDefs() // надо запускать этот визитор начиная с корня подпрограммы
        {
        }

        public override void visit(var_statement vs) // локальные описания внутри процедуры
        {
            LocalDeletedDefs.Add(vs.var_def);
            DeleteInStatementList(vs);

            LocalDeletedDefsNames.UnionWith(vs.var_def.vars.idents.Select(id => id.name));
        }

        public override void visit(variable_definitions vd)
        {
            foreach (var v in vd.list)
            {
                LocalDeletedDefs.Add(v);
                LocalDeletedDefsNames.UnionWith(v.vars.idents.Select(id => id.name));
            }
            var d = UpperNodeAs<declarations>();
            d.defs.Remove(vd); // может ли остаться список declarations пустым?
        }

        // еще - не заходить в лямбды

    }
}
