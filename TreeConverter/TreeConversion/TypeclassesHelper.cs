using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PascalABCCompiler.TreeRealization;
using PascalABCCompiler.SemanticTree;

namespace PascalABCCompiler.TreeConverter
{
    public static class TypeclassHelper
    {
        public static bool HasRestrictions(function_node fn)
        {
            var hasRestriction = fn?.attributes?.Any(x => x.AttributeType.name == "__TypeclassRestrictedFunctionAttribute");
            return hasRestriction.HasValue && hasRestriction.Value;
        }

        public static IEnumerable<type_node> GetRestrictedGenerics(function_node fn)
        {
            return fn.get_generic_params_list().Where(x => x.Attributes != null && x.Attributes.Any(attr => attr.AttributeType.name == "__TypeclassGenericParameterAttribute"));
        }

        public static bool IsInstance(type_node ctn)
        {
            var isInstance = ctn?.attributes?.Any(x => x.AttributeType.name == "__TypeclassInstanceAttribute");
            return isInstance.HasValue && isInstance.Value;
        }

        public static bool IsTypeclass(type_node ctn)
        {
            var isTypeclass = ctn?.attributes?.Any(x => x.AttributeType.name == "__TypeclassAttribute");
            return isTypeclass.HasValue && isTypeclass.Value;
        }

        public static string OperatorName(string op)
        {
            return "$typeclass" + op;
        }

        public static string GetInstanceName(type_node ctn)
        {
            return ctn?.attributes?.First(x => x.AttributeType.name == "__TypeclassInstanceAttribute")?.Arguments[0]?.value?.ToString();
        }

        public static string GetTypeclassName(type_node ctn)
        {
            var att = ctn?.attributes;
            string res = null;

            res = att.FirstOrDefault(x => x.AttributeType.name == "__TypeclassAttribute")?.Arguments[0]?.value?.ToString();
            if (res == null)
                res = att.FirstOrDefault(x => x.AttributeType.name == "__TypeclassGenericParameterAttribute")?.Arguments[1]?.value?.ToString();
            if (res == null)
                res = att.FirstOrDefault(x => x.AttributeType.name == "__TypeclassInstanceAttribute")?.Arguments[1]?.value?.ToString();

            return res;
        }

        public static string ResugarMethodName(string name)
        {
            string newName = name;
            if(name.StartsWith("$typeclass"))
            {
                newName = name.Replace("$typeclass", "operator");
            }
            return newName;
        }
    }
}
