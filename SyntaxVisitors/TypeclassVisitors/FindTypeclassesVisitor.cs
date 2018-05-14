using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler;
using PascalABCCompiler.Errors;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.TypeclassVisitors
{
    public class FindTypeclassesVisitor: WalkingVisitorNew
    {
        //TODO: add searching typeclasses at libraries

        // (Typeclass name) -> (Type arguments count)
        public Dictionary<string, type_declaration> typeclasses = new Dictionary<string, type_declaration>();


        public FindTypeclassesVisitor()
        {

        }


        public static FindTypeclassesVisitor New
        {
            get
            {
                return new FindTypeclassesVisitor();
            }
        }


        public override void visit(type_declaration typeclassDeclaration)
        {
            var typeclassDefinition = typeclassDeclaration.type_def as typeclass_definition;
            if (typeclassDefinition == null)
            {
                return;
            }

            var typeclassName = typeclassDeclaration.type_name as typeclass_restriction;

            typeclasses.Add(typeclassName.name, typeclassDeclaration);
        }
    }
}
