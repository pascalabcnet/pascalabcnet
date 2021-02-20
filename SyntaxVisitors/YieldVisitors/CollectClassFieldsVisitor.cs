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
    public class CollectClassFieldsVisitor : CollectUpperNodesVisitor
    {
        private ident _className;

        public ISet<ident> CollectedFields { get; private set; }

        public CollectClassFieldsVisitor(ident className)
        {
            CollectedFields = new HashSet<ident>();

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
            /*var fields = cd.body.class_def_blocks.SelectMany(cm => cm.members.Where(decl => decl is var_def_statement)
                                                                .Select(decl1 => (decl1 as var_def_statement).vars.idents)
                                                                .SelectMany(ids => ids.Select(id => id)));
            

            CollectedFields.UnionWith(fields);*/

            foreach (var defs in cd.body.class_def_blocks)
            {
                // Class members
                foreach (var decl in defs.members)
                {
                    // Declaration: fields + procedure defs + headers + etc
                    // Need vars
                    if (decl is var_def_statement vds)
                    {
                        CollectedFields.UnionWith(vds.vars.idents);
                        // Fields
                        /*foreach (var field in vds.vars.idents)
                        {
                            CollectedFields.Add(field);
                        }*/
                    }
                    else if (decl is simple_const_definition scd)
                    {
                        CollectedFields.Add(scd.const_name);
                    }
                }
            }

        }
    }
}
