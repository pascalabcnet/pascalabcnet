using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Languages.SPython.Frontend.Converters
{
    internal class KvargsFunctionDesugarVisitor: BaseChangeVisitor
    {
        public KvargsFunctionDesugarVisitor() { }

        private typed_parameters LastParameter(procedure_definition pd)
        {
            return pd.proc_header.parameters.Last();
        }

        private bool HasParameters(procedure_definition pd)
        {
            return pd.proc_header.parameters != null;
        }

        private bool IsKvargsParameter(typed_parameters tp)
        {
            return tp.param_kind == parametr_kind.kvargs_parameter;
        }

        private bool HasKvargsParameter(procedure_definition pd)
        {
            return HasParameters(pd) && IsKvargsParameter(LastParameter(pd));
        }

        public override void visit(procedure_definition pd)
        {
            if (!HasKvargsParameter(pd))
                return;

            declaration d = CreateTypeDefinitionForKvargsFunction(pd);
            Replace(pd, d);
        }

        private bool IsForwardDeclaration(procedure_header _procedure_header)
        {
            foreach (procedure_attribute attr in _procedure_header.proc_attributes.proc_attributes)
                if (attr.attribute_type is proc_attribute.attr_forward)
                    return true;

            return false;
        }

        private declaration CreateTypeDefinitionForKvargsFunction(procedure_definition pd)
        {
            typed_parameters kvargs_parameter = LastParameter(pd);
            pd.proc_header.parameters.Remove(kvargs_parameter);
            ident class_name = new ident("tempname");

            if (IsForwardDeclaration(pd.proc_header))
            {
                // base class type
                template_param_list tpl = new template_param_list(kvargs_parameter.vars_type, kvargs_parameter.source_context);
                ident base_class_name = new ident("kvargs_gen`1");
                named_type_reference ntr = new named_type_reference(base_class_name);
                template_type_reference ttr = new template_type_reference(ntr, tpl);
                named_type_reference_list ntrl = new named_type_reference_list(ttr);

                // class body
                access_modifer_node amn = new access_modifer_node(access_modifer.public_modifer);
                class_members cm = new class_members(pd.proc_header);
                class_body_list cbl = new class_body_list(cm);

                // type class declaration 
                class_definition cd = new class_definition(ntrl, cbl);
                type_declaration td = new type_declaration(class_name, cd);
                type_declarations result = new type_declarations(td);

                return result;
            }
            else
            {
                pd.proc_header.name = new method_name(null, class_name, pd.proc_header.name.meth_name, null);
                return pd;
            }
        }
    }
}
