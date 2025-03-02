using System.Collections.Generic;
using System.ComponentModel.Design;
using PascalABCCompiler.SyntaxTree;

namespace Languages.SPython.Frontend.Converters
{
    public class RetainUsedGlobalVariablesVisitor : BaseChangeVisitor
    {
        private HashSet<string> variablesDeclaredGlobal = new HashSet<string>();
        private HashSet<string> variablesUsedGlobal = new HashSet<string>();
        private HashSet<string> localVariables = new HashSet<string>();
        private Dictionary<string, variable_definitions> variablesToDefinitions = new Dictionary<string, variable_definitions>();
        private bool isInProgramCode = false;
        private bool isInFunction = false;
        private declarations decls;
        private procedure_definition main;

        public RetainUsedGlobalVariablesVisitor() {}

        // нужны методы из BaseChangeVisitor, но порядок обхода из WalkingVisitorNew
        public override void DefaultVisit(syntax_tree_node n)
        {
            for (var i = 0; i < n.subnodes_count; i++)
                ProcessNode(n[i]);
        }

        private bool IsUnusedGlobalVariable(string name) {
            return variablesDeclaredGlobal.Contains(name) && !variablesUsedGlobal.Contains(name);
        }

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

            base.Enter(stn);
        }

        public override void Exit(syntax_tree_node stn)
        {
            if (stn is procedure_definition _procedure_definition)
            {
                localVariables.Clear();
            }
            if (stn is program_module pm)
            {
                isInProgramCode = true;
                ProcessNode(main);
            }

            base.Exit(stn);
        }

        public override void visit(procedure_definition _procedure_definition)
        {
            if (isInProgramCode || _procedure_definition.proc_header.name.meth_name.name != "%%MAIN%%")
            {
                isInFunction = !isInProgramCode;
                base.visit(_procedure_definition);
                isInFunction = false;
            }
            else main = _procedure_definition;
        }

        public override void visit(variable_definitions _variable_definitions)
        {
            string variable_name = _variable_definitions.var_definitions[0].vars.idents[0].name;
            variablesDeclaredGlobal.Add(variable_name);
            variablesToDefinitions[variable_name] = _variable_definitions;
            
            base.visit(_variable_definitions);
        }

        public override void visit(var_statement _var_statement)
        {
            if (isInFunction)
                localVariables.Add(_var_statement.var_def.vars.idents[0].name);

            base.visit(_var_statement);
        }

        public override void visit(typed_parameters _typed_parameters)
        {
            localVariables.Add(_typed_parameters.idents.idents[0].name);

            base.visit(_typed_parameters);
        }

        public override void visit(ident _ident)
        {
            if (isInFunction &&
                variablesDeclaredGlobal.Contains(_ident.name)   &&
                !localVariables.Contains(_ident.name)           && 
                !variablesUsedGlobal.Contains(_ident.name)
                )
                variablesUsedGlobal.Add(_ident.name);

            base.visit(_ident);
        }

        public override void visit(assign _assign)
        {
            if (isInProgramCode && _assign.to is ident _ident && IsUnusedGlobalVariable(_ident.name))
            {
                var _var_statement = SyntaxTreeBuilder.BuildVarStatementNodeFromAssignNode(_assign);
                if (!_assign.first_assignment_defines_type)
                    _var_statement.var_def.vars_type = variablesToDefinitions[_ident.name].var_definitions[0].vars_type;

                variablesDeclaredGlobal.Remove(_ident.name);
                decls.Remove(variablesToDefinitions[_ident.name]);

                ReplaceStatement(_assign, _var_statement);
                return;
            }
            
            base.visit(_assign);
        }
    }
}
