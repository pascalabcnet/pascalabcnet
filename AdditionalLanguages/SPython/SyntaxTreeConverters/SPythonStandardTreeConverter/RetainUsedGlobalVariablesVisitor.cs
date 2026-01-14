using PascalABCCompiler.SyntaxTree;
using System.Collections.Generic;

namespace Languages.SPython.Frontend.Converters
{
    public class RetainUsedGlobalVariablesVisitor : BaseChangeVisitor
    {
        private HashSet<string> variablesUsedAsGlobal = new HashSet<string>();
        private int scopeCounter = 0;
        private declarations decls;
        private bool isUnitCompiled = false;

        public RetainUsedGlobalVariablesVisitor(HashSet<string> variablesUsedAsGlobal) 
        {
            this.variablesUsedAsGlobal = variablesUsedAsGlobal;
        }

        // нужны методы из BaseChangeVisitor, но порядок обхода из WalkingVisitorNew
        public override void DefaultVisit(syntax_tree_node n)
        {
            for (var i = 0; i < n.subnodes_count; i++)
                ProcessNode(n[i]);
        }

        public override void Enter(syntax_tree_node stn)
        {
            if (stn is statement_list)
            {
                scopeCounter++;
            }
            if (stn is program_module pm)
            {
                decls = pm.program_block.defs;
            }
            if (stn is interface_node intn)
            {
                isUnitCompiled = true;
                decls = intn.interface_definitions;
            }

            base.Enter(stn);
        }

        public override void Exit(syntax_tree_node stn)
        {
            if (stn is statement_list)
            {
                scopeCounter--;
            }
            base.Exit(stn);
        }

        public override void visit(var_statement _var_statement)
        {
            ident id = _var_statement.var_def.vars.idents[0];
            if (scopeCounter == 1 && (isUnitCompiled || variablesUsedAsGlobal.Contains(id.name)))
            {
                //variablesUsedAsGlobal.Remove(id.name);
                SourceContext sc = _var_statement.source_context;
                statement replace_to = new empty_statement();
                type_definition td = _var_statement.var_def.vars_type;
                if (_var_statement.var_def.inital_value != null)
                {
                    replace_to = new assign(id, _var_statement.var_def.inital_value, sc);
                }
                if (td == null)
                {
                    (replace_to as assign).first_assignment_defines_type = true;
                    td = new named_type_reference(new ident("integer"));
                }
                var vds = new var_def_statement(new ident_list(id, id.source_context), td, null, definition_attribute.None, false, sc);
                decls.Add(new variable_definitions(vds, sc), sc);
                
                ReplaceStatement(_var_statement, replace_to);
            }
        }
    }
}
