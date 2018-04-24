using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler;
using PascalABCCompiler.Errors;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.TypeclassVisitors
{
    public class FindInstancesVisitor: WalkingVisitorNew
    {

        // (Typeclass name) -> (Type arguments count)
        Dictionary<string, int> typeclasses;
        // (Typeclass name) -> [Instances]
        Dictionary<string, HashSet<template_param_list>> instances = new Dictionary<string, HashSet<template_param_list>>();


        public FindInstancesVisitor(Dictionary<string, int> typeclasses)
        {
            this.typeclasses = typeclasses;

            foreach (var typeclass in typeclasses)
            {
                instances.Add(typeclass.Key, new HashSet<template_param_list>());
            }
        }


        public static FindInstancesVisitor New(Dictionary<string, int> typeclasses)
        {
            return new FindInstancesVisitor(typeclasses);
        }


        public override void visit(type_declaration instanceDeclaration)
        {
            var instanceDefinition = instanceDeclaration.type_def as instance_definition;
            if (instanceDefinition == null)
            {
                return;
            }
            var instanceName = instanceDeclaration.type_name as typeclass_restriction;

            instances[instanceName.name].Add(instanceName.restriction_args);
        }
    }
}
