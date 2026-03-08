// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System.Collections.Generic;
using PascalABCCompiler.CoreUtils;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    // Проба будущего визитора или не будущего
    public class RenameOnExceptVarsVisitor : BaseChangeVisitor
    {
        public string NameForRename = null;
        public string NewName = null;

        private readonly GeneratedNamesManager generatedNamesManager;

        public RenameOnExceptVarsVisitor(GeneratedNamesManager generatedNamesManager)
        {
            this.generatedNamesManager = generatedNamesManager;
        }

        public void Rename(exception_handler eh)
        {
            NameForRename = eh.variable?.name?.ToLower();
            if (NameForRename != null)
            {
                NewName = GetNewVariableName(NameForRename);
                ProcessNode(eh.statements);
            }
        }

        public override void visit(ident id)
        {
            var idl = id.name.ToLower();
            if (NameForRename == idl)
            {
                id.name = NewName;
            }
        }

        public override void visit(dot_node dn)
        {
            ProcessNode(dn.left);
            if (dn.right.GetType() != typeof(ident))
                ProcessNode(dn.right);
        }

        private string GetNewVariableName(string name)
        {
            return generatedNamesManager.GenerateName("$Rename_" + name);
        }
    }

    public class FindOnExceptVarsAndApplyRenameVisitor : BaseChangeVisitor
    {
        private readonly GeneratedNamesManager generatedNamesManager;

        private FindOnExceptVarsAndApplyRenameVisitor(GeneratedNamesManager generatedNamesManager)
        {
            this.generatedNamesManager = generatedNamesManager;
        }

        public static FindOnExceptVarsAndApplyRenameVisitor Create(GeneratedNamesManager generatedNamesManager) => new FindOnExceptVarsAndApplyRenameVisitor(generatedNamesManager);
        
        public override void visit(exception_handler eh)
        {
            if (eh.variable != null)
            {
                // Вначале переименовать везде кроме как в заголовке, потом добавить в блок операторов переприсваивание
                var vis = new RenameOnExceptVarsVisitor(generatedNamesManager);
                vis.Rename(eh);

                var vs = new var_statement(new ident(vis.NewName/*, eh.statements.source_context*/), new ident(vis.NameForRename, eh.variable.source_context)/*, eh.statements.source_context*/);
                var stl = new List<statement>();
                stl.Add(vs);
                if (eh.statements != null)
                {
                    stl.Add(eh.statements);
                    ReplaceStatementUsingParent(eh.statements, stl);
                }
                else
                {
                    var s = new statement_list(stl);
                    s.Parent = eh;
                    s.source_context = eh.source_context;
                    eh.statements = s;
                }
            }
            base.visit(eh);
        }
    }
}
