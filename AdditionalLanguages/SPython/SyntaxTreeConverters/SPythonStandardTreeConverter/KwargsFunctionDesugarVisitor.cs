using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Languages.SPython.Frontend.Converters
{
    internal class KwargsFunctionDesugarVisitor: BaseChangeVisitor
    {
        public KwargsFunctionDesugarVisitor() { }

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

        private bool IsKwargsParameter(typed_parameters tp)
        {
            return tp.param_kind == parametr_kind.kwargs_parameter;
        }

        private bool HasKwargsParameter(procedure_definition pd)
        {
            if (!HasParameters(pd))
                return false;

            foreach (typed_parameters param in pd.proc_header.parameters.params_list)
            {
                if (IsKwargsParameter(param))
                    return true;
            }

            return false;
        }

        string renameFrom = "";

        private void RenameKwargParametor(procedure_definition pd)
        {
            renameFrom = LastParameter(pd).idents.idents[0].name;
            var body = pd.proc_body;
            base.visit(pd);
            renameFrom = "";
        }

        public override void visit(ident id)
        {
            // сомнительно, лучше сделать другим визитором
            if (renameFrom != "" && id.name == renameFrom)
            {
                id.name = "!kwargs";
            }
        }

        private void AddNoKwargsVersion(procedure_definition pd)
        {
            procedure_definition noKwargsVersion = pd.TypedClone();
            typed_parameters kwargs_parameter = LastParameter(noKwargsVersion);
            noKwargsVersion.proc_header.parameters.Remove(kwargs_parameter);
            ident name = noKwargsVersion.proc_header.name.meth_name;
            ident class_name = "!" + name;

            if (IsForwardDeclaration(noKwargsVersion.proc_header))
            {
                decls.Add(noKwargsVersion);
            }
            else
            {
                List<typed_parameters> tp_list = noKwargsVersion.proc_header.parameters.params_list;
                expression_list args = new expression_list();
                foreach (typed_parameters tp in tp_list)
                {
                    foreach (ident id in tp.idents.list)
                    {
                        args.Add(id);
                    }
                }

                new_expr ne = new new_expr(new named_type_reference(class_name), null, false, null);

                dot_node dn = new dot_node(ne, new ident(name.name));
                method_call method_call = new method_call(dn, args);

                statement stmt;

                if (pd.proc_header is function_header)
                {
                    stmt = new assign(new ident(StringConstants.result_var_name), method_call, Operators.Assignment);
                }
                else
                {
                    stmt = new procedure_call(method_call, false);
                }

                statement_list stmtlist = new statement_list(stmt);

                declarations empty_declarations = new declarations();
                block block = new block(empty_declarations, stmtlist);
                noKwargsVersion.proc_body = block;
                current_decls.Add(noKwargsVersion);
            }
        }

        void CheckIfKwargParametorIsUsedCorrectly(procedure_definition pd)
        {
            var params_list = pd.proc_header.parameters.params_list;
            for (int i = 0; i < params_list.Count() - 1; ++i)
            {
                if (IsKwargsParameter(params_list[i]))
                {
                    throw new SPythonSyntaxVisitorError("KWARG_PARAMETOR_NOT_LAST",
                   params_list[i].source_context);
                }
            }
        }

        public override void visit(procedure_definition pd)
        {
            if (!HasKwargsParameter(pd))
                return;

            // Проверяем что kwarg-параметр только последний
            CheckIfKwargParametorIsUsedCorrectly(pd);

            RenameKwargParametor(pd);

            declaration d = CreateTypeDefinitionForKwargsFunction(pd);
            AddNoKwargsVersion(pd);
            Replace(pd, d);
        }

        private bool IsForwardDeclaration(procedure_header _procedure_header)
        {
            foreach (procedure_attribute attr in _procedure_header.proc_attributes.proc_attributes)
                if (attr.attribute_type is proc_attribute.attr_forward)
                    return true;

            return false;
        }

        private declaration CreateTypeDefinitionForKwargsFunction(procedure_definition pd)
        {
            procedure_definition new_pd = pd.TypedClone();
            typed_parameters kwargs_parameter = LastParameter(new_pd);
            new_pd.proc_header.parameters.Remove(kwargs_parameter);
            ident name = new_pd.proc_header.name.meth_name;
            ident class_name = "!" + name;

            if (IsForwardDeclaration(new_pd.proc_header))
            {
                // base class type
                template_param_list tpl = new template_param_list(kwargs_parameter.vars_type, kwargs_parameter.source_context);
                ident base_class_name = new ident("kwargs_gen`1");
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
