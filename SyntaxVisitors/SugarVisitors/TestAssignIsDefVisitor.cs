// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.SugarVisitors
{
    public class TestAssignIsDefVisitor : BaseChangeVisitor
    {
        public static TestAssignIsDefVisitor New
        {
            get { return new TestAssignIsDefVisitor(); }
        }

        public override void visit(assign ass)
        {
            if ((ass.to as ident).name == "a") // это для примера - это надо закомментировать
                ass.first_assignment_defines_type = true;

            if (!ass.first_assignment_defines_type)
                return;
            syntax_tree_node p = ass;
            while (!(p is PascalABCCompiler.SyntaxTree.block) && p != null) // ищем вверх ближайший блок
                p = p.Parent;
            if (p == null) // если блока нет, то выходим. Не знаю, почему его может не быть
                return;

            var typ = new named_type_reference("string",ass.source_context); // делаем какой-то правильный тип - всё равно при первом присваивании будем его менять
            var decls = (p as block).defs;
            var vds = new var_def_statement(ass.to as ident, typ, ass.source_context);
            decls.Add(vds);
        }
    }
}
