// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System.Collections.Generic;
using System.Linq;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeRealization;

namespace TreeConverter.LambdaExpressions
{
    internal class LambdaCapturedSymbolsHelper
    {
        private static List<ident> GetTemplateParametersTypeDependsOn(type_node type)
        {
            if (type.generic_function_container != null)
            {
                return new List<ident>
                    {
                        new ident(type.name)
                    };
            }

            var typeRef = type as ref_type_node;
            if (typeRef != null)
            {
                return GetTemplateParametersTypeDependsOn(typeRef.pointed_type);
            }

            var typeIi = type.get_internal_interface(internal_interface_kind.unsized_array_interface) as array_internal_interface;
            if (typeIi != null)
            {
                return GetTemplateParametersTypeDependsOn(typeIi.element_type);
            }

            if (type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.set_type)
            {
                return GetTemplateParametersTypeDependsOn(type.element_type);
            }

            if (type.IsDelegate)
            {
                var dii = type.get_internal_interface(internal_interface_kind.delegate_interface) as delegate_internal_interface;
                var res = new List<ident>();
                if (dii != null)
                {
                    var paramCount = dii.parameters.Count;
                    for (var i = 0; i < paramCount; i++)
                    {
                        res.AddRange(GetTemplateParametersTypeDependsOn(dii.parameters[i].type));
                    }
                }
                return res;
            }

            if (type.is_generic_type_instance)
            {
                var pcount = type.instance_params.Count;
                var res = new List<ident>();

                for (var i = 0; i < pcount; i++)
                {
                    res.AddRange(GetTemplateParametersTypeDependsOn(type.instance_params[i]));
                }
                return res;
            }

            return new List<ident>();
        }

        public static List<ident> GetIdsForTemplateParametersFromLambdaParameters(List<type_node> parametersTypes)
        {
            var res = new List<ident>();

            if (parametersTypes == null
                || parametersTypes.Count == 0)
            {
                return res;
            }

            foreach (var parType in parametersTypes)
            {
                res.AddRange(GetTemplateParametersTypeDependsOn(parType)); 
            }

            return res.GroupBy(id => id.name)
                      .Select(ids => ids.First())
                      .ToList();
        }
    }
}