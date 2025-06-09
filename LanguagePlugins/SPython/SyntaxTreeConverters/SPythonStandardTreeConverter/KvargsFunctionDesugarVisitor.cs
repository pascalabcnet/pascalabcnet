using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace Languages.SPython.Frontend.Converters
{
    internal class KvargsFunctionDesugarVisitor: BaseChangeVisitor
    {
        public KvargsFunctionDesugarVisitor() { }

        private declarations decls;
        private declarations current_decls;

        public override void Enter(syntax_tree_node stn)
        {
            if (stn is program_module pm)
            {
                decls = pm.program_block.defs;
            }
            if (stn is interface_node intn)
            {
                decls = intn.interface_definitions;
            }
            if (stn is declarations_as_statement das)
            {
                current_decls = das.defs;
            }

            base.Enter(stn);
        }

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

        private void AddNoKvargsVersion(procedure_definition pd)
        {
            procedure_definition noKvargsVersion = pd.TypedClone();
            typed_parameters kvargs_parameter = LastParameter(noKvargsVersion);
            noKvargsVersion.proc_header.parameters.Remove(kvargs_parameter);
            ident name = noKvargsVersion.proc_header.name.meth_name;
            ident class_name = "!" + name;

            if (IsForwardDeclaration(noKvargsVersion.proc_header))
            {
                decls.Add(noKvargsVersion);
            }
            else
            {
                List<typed_parameters> tp_list = noKvargsVersion.proc_header.parameters.params_list;
                expression_list args = new expression_list();
                foreach (typed_parameters tp in tp_list)
                {
                    foreach (ident id in tp.idents.list)
                    {
                        args.Add(id);
                    }
                }

                dot_node ctor_call_name = new dot_node(class_name, new ident("create"));
                method_call ctor_call = new method_call(ctor_call_name, null);
                dot_node dn = new dot_node(ctor_call, new ident(name.name));
                method_call method_call = new method_call(dn, args);

                assign assign = new assign(new ident("result"), method_call, Operators.Assignment);

                statement_list stmtlist = new statement_list(assign);
                declarations empty_declarations = new declarations();
                block block = new block(empty_declarations, stmtlist);
                noKvargsVersion.proc_body = block;
                current_decls.Add(noKvargsVersion);
            }
        }

        public override void visit(procedure_definition pd)
        {
            if (!HasKvargsParameter(pd))
                return;
            
            declaration d = CreateTypeDefinitionForKvargsFunction(pd);
            AddNoKvargsVersion(pd);
            Replace(pd, d);
        }

        private bool IsForwardDeclaration(procedure_header _procedure_header)
        {
            foreach (procedure_attribute attr in _procedure_header.proc_attributes.proc_attributes)
                if (attr.attribute_type is proc_attribute.attr_forward)
                    return true;

            return false;
        }

        private int createdDeclarations = 0;

        private declaration CreateTypeDefinitionForKvargsFunction(procedure_definition pd)
        {
            procedure_definition new_pd = pd.TypedClone();
            typed_parameters kvargs_parameter = LastParameter(new_pd);
            new_pd.proc_header.parameters.Remove(kvargs_parameter);
            ident name = new_pd.proc_header.name.meth_name;
            ident class_name = "!" + name;

            if (IsForwardDeclaration(new_pd.proc_header))
            {
                // base class type
                template_param_list tpl = new template_param_list(kvargs_parameter.vars_type, kvargs_parameter.source_context);
                ident base_class_name = new ident("kvargs_gen`1");
                named_type_reference ntr = new named_type_reference(base_class_name);
                template_type_reference ttr = new template_type_reference(ntr, tpl);
                named_type_reference_list ntrl = new named_type_reference_list(ttr);

                // class body
                access_modifer_node amn = new access_modifer_node(access_modifer.public_modifer);
                new_pd.proc_header.name.meth_name = new_pd.proc_header.name.meth_name;
                class_members cm = new class_members(new_pd.proc_header);
                class_body_list cbl = new class_body_list(cm);

                // type class declaration 
                class_definition cd = new class_definition(ntrl, cbl);
                type_declaration td = new type_declaration(class_name, cd);
                type_declarations result = new type_declarations(td);

                return result;
            }
            else
            {
                new_pd.proc_header.name = new method_name(null, class_name, name, null);
                return new_pd;
            }
        }
    }
}
