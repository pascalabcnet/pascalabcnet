// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
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
    public class CollectClassMethodsVisitor : CollectUpperNodesVisitor
    {
        private ident _className;

        public ISet<ident> CollectedMethods { get; private set; }

        public CollectClassMethodsVisitor(ident className)
        {
            CollectedMethods = new HashSet<ident>();
            _className = className;
        }

        public override void visit(class_definition cd)
        {
            type_declaration td = UpperNode(1) as type_declaration;

            if ((object)td == null)
            {
                // Something went wrong
                return;
            }

            if (td.type_name.name.ToLower() != _className.name.ToLower())
            {
                // Not the class we search for
                // Yoda
                return;
            }
            if (cd.body == null)
                return;
            var methods = cd.body.class_def_blocks.SelectMany(cm => cm.members.Select(decl1 => 
                {
                    if (decl1 is procedure_header)
                        return (decl1 as procedure_header).name?.meth_name;
                    else if (decl1 is procedure_definition)
                        return (decl1 as procedure_definition).proc_header.name?.meth_name;
                    return null;
                }).Where(name => (object)name != null));

            CollectedMethods.UnionWith(methods);
        }

    }
}
