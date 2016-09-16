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

            if (td.type_name.name != _className.name)
            {
                // Not the class we search for
                // Yoda
                return;
            }

            var fields = cd.body.class_def_blocks.SelectMany(cm => cm.members.Where(decl => decl is var_def_statement)
                                                                .Select(decl1 => (decl1 as var_def_statement).vars.idents)
                                                                .SelectMany(ids => ids.Select(id => id)));
            

            CollectedFields.UnionWith(fields);

            /*foreach (var defs in cd.body.class_def_blocks)
            {
                // Class members
                foreach (var decl in defs.members)
                {
                    // Declaration: fields + procedure defs + headers + etc
                    // Need vars
                    if (decl is var_def_statement)
                    {
                        var vars = (decl as var_def_statement).vars;
                        // Fields
                        foreach (var field in vars.idents)
                        {
                            CollectedFields.Add(field);
                        }
                    }
                }
            }*/
        }

    }
}
