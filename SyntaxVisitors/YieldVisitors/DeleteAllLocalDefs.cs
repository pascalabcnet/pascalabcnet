// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class MoveAllLocalDefsToLists : BaseChangeVisitor
    {

        private List<var_def_statement> LocalDeletedVD = new List<var_def_statement>(); // все локальные описания

        private List<var_def_statement> LocalDeletedVS = new List<var_def_statement>(); // var_statement's, потом объединим с LocalDeletedVD для верного порядка

        public ISet<string> LocalDeletedDefsNames = new HashSet<string>();            // их имена - для быстрого поиска  

        public ISet<ident> CollectedLocals = new HashSet<ident>(); // SSM - не очень понимаю, зачем хранить строки, identы и 

        public IEnumerable<var_def_statement> LocalDeletedDefs
        {
            get
            {
                return new List<var_def_statement>(LocalDeletedVD.Union(LocalDeletedVS));
            }
        }

        public MoveAllLocalDefsToLists() // надо запускать этот визитор начиная с корня подпрограммы
        {
        }

        public static MoveAllLocalDefsToLists New
        {
            get { return new MoveAllLocalDefsToLists(); }
        }

        public static MoveAllLocalDefsToLists Accept(procedure_definition pd)
        {
            var n = New;
            n.ProcessNode(pd);
            return n;
        }

        public override void visit(var_statement vs) // локальные описания внутри процедуры
        {
            LocalDeletedVS.Insert(0, vs.var_def);
            //DeleteInStatementList(vs);

            // frninja 02/03/16 - fix capturing unknown idents -> replace delete with assign
            if ((object)vs.var_def.inital_value == null)
            {
                DeleteInStatementList(vs);
            }
            else
            {
                // Нескольких переменных тут всё равно не будет - это ошибка
                ReplaceStatement(vs, SeqStatements(vs.var_def.vars.idents.Select(id => new assign(id, vs.var_def.inital_value, vs.source_context)).ToArray()));
            }

            LocalDeletedDefsNames.UnionWith(vs.var_def.vars.idents.Select(id => id.name.ToLower()));
            CollectedLocals.UnionWith(vs.var_def.vars.idents);
        }

        public override void visit(variable_definitions vd) // описания до begin
        {
            foreach (var v in vd.list)
            {
                LocalDeletedVD.Insert(0, v); //.Add(v);
                LocalDeletedDefsNames.UnionWith(v.vars.idents.Select(id => id.name.ToLower()));
            }
            var d = UpperNodeAs<declarations>();
            d.defs.Remove(vd); // может ли остаться список declarations пустым?

            // frninja 03/03/16 - переносим объявления переменных c initial_value в тело метода для дальнейшей обработки
            var methodBody = UpperTo<block>().program_code;
            
            methodBody.list.InsertRange(0, vd.list
                    .Where(vds => (object)vds.inital_value != null)
                    .SelectMany(vdsInit => vdsInit.vars.list.Select(id => new assign(id, vdsInit.inital_value))));

            // еще - не заходить в лямбды
        }
    }
}
