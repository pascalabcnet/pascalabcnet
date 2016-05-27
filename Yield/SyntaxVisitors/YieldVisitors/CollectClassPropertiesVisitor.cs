using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class CollectClassPropertiesVisitor : CollectUpperNodesVisitor
    {
        private ident _className;

        public ISet<ident> CollectedProperties { get; private set; }

        public CollectClassPropertiesVisitor(ident className)
        {
            CollectedProperties = new HashSet<ident>();
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

            var properties = cd.body.class_def_blocks.SelectMany(cm => cm.members.Select(decl => decl as simple_property)
                                                                              .Where(name => (object)name != null)
                                                                              .Select(sp => sp.property_name));

            CollectedProperties.UnionWith(properties);
        }
    }
}
