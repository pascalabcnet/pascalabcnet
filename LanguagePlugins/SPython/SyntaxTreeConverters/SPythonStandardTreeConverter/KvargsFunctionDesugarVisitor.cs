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

        private bool IsKvargsParameter(typed_parameters tp)
        {
            return tp.param_kind == parametr_kind.kvargs_parameter;
        }

        private bool HasKvargsParameter(procedure_definition pd)
        {
            return IsKvargsParameter(LastParameter(pd));
        }

        public override void visit(procedure_definition pd)
        {
            if (!HasKvargsParameter(pd))
                return;

            type_declarations td = CreateTypeDefinitionForKvargsFunction(pd);
            Replace(pd, td);
        }

        private type_declarations CreateTypeDefinitionForKvargsFunction(procedure_definition pd)
        {
            typed_parameters kvargs_parameter = LastParameter(pd);

            // base class type
            template_param_list tpl = new template_param_list(kvargs_parameter.vars_type, kvargs_parameter.source_context);
            ident base_class_name = new ident("kvargs_gen`1");
            named_type_reference ntr = new named_type_reference(base_class_name);
            template_type_reference ttr = new template_type_reference(ntr, tpl);
            named_type_reference_list ntrl = new named_type_reference_list(ttr);

            // class body
            access_modifer_node amn = new access_modifer_node(access_modifer.public_modifer);
            pd.proc_header.parameters.Remove(kvargs_parameter);
            class_members cm = new class_members(pd);
            class_body_list cbl = new class_body_list(cm);

            // type class declaration 
            class_definition cd = new class_definition(ntrl, cbl);
            ident class_name = new ident("tempname");
            type_declaration td = new type_declaration(class_name, cd);
            type_declarations result = new type_declarations(td);

            return result;
        }
    }
}
