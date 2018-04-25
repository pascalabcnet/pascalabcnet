using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler;
using PascalABCCompiler.Errors;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.TypeclassVisitors
{
    public class FindInstancesAndRestrictedFunctionsVisitor: WalkingVisitorNew
    {

        // (Typeclass name) -> (Type arguments count)
        public Dictionary<string, int> typeclasses;
        // (Typeclass name) -> [Instances]
        public Dictionary<string, List<typeclass_param_list>> instances = new Dictionary<string, List<typeclass_param_list>>();
        public Dictionary<string, List<string>> restrictedFunctions = new Dictionary<string, List<string>>();


        public FindInstancesAndRestrictedFunctionsVisitor(Dictionary<string, int> typeclasses)
        {
            this.typeclasses = typeclasses;

            foreach (var typeclass in typeclasses)
            {
                instances.Add(typeclass.Key, new List<typeclass_param_list>());
            }
        }


        public static FindInstancesAndRestrictedFunctionsVisitor New(Dictionary<string, int> typeclasses)
        {
            return new FindInstancesAndRestrictedFunctionsVisitor(typeclasses);
        }


        public override void visit(type_declaration instanceDeclaration)
        {
            var instanceDefinition = instanceDeclaration.type_def as instance_definition;
            if (instanceDefinition == null)
            {
                return;
            }
            var instanceName = instanceDeclaration.type_name as typeclass_restriction;

            instances[instanceName.name].Add(instanceName.restriction_args as typeclass_param_list);
        }


        public override void visit(procedure_definition procedureDefinition)
        {
            bool isConstrainted = procedureDefinition.proc_header.where_defs != null &&
                procedureDefinition.proc_header.where_defs.defs.Any(x => x is where_typeclass_constraint);
            if (!isConstrainted)
                return;

            var usedTypeclasses = procedureDefinition.proc_header.where_defs.
                defs.OfType<where_typeclass_constraint>()
                .Select(x => x.restriction.name).ToList();

            restrictedFunctions.Add(procedureDefinition.proc_header.name.meth_name.name,
                usedTypeclasses);
        }
    }
}
