using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;

namespace Languages.SPython.Frontend.Converters
{
    public class SPythonGlobalStatementVisitor : BaseChangeVisitor
    {
        HashSet<string> FunctionGlobalVariables;
        HashSet<string> FunctionLocalVariables;
        HashSet<string> FunctionParameters;
        private bool IsInFunctionBody { get; set; }
        private bool IsInProgramCode { get; set; }

        public SPythonGlobalStatementVisitor() {
            FunctionGlobalVariables = new HashSet<string>();
            FunctionLocalVariables = new HashSet<string>();
            FunctionParameters = new HashSet<string>();
            IsInFunctionBody = false;
            IsInProgramCode = false;
        }

        // нужны методы из BaseChangeVisitor, но порядок обхода из WalkingVisitorNew
        public override void DefaultVisit(syntax_tree_node n)
        {
            for (var i = 0; i < n.subnodes_count; i++)
                ProcessNode(n[i]);
        }

        public override void Enter(syntax_tree_node stn)
        {
            if (stn is procedure_definition _procedure_definition)
            {
                IsInFunctionBody = true;
            }
            if (stn is statement_list _statement_list)
            {
                if (!IsInFunctionBody)
                    IsInProgramCode = true;
            }

            base.Enter(stn);
        }

        public override void Exit(syntax_tree_node stn)
        {
            if (stn is procedure_definition _procedure_definition)
            {
                IsInFunctionBody = false;
                FunctionGlobalVariables.Clear();
                FunctionLocalVariables.Clear();
                FunctionParameters.Clear();
            }

            base.Exit(stn);
        }

        public override void visit(global_statement _global_statement)
        {
            foreach (ident _ident in _global_statement.idents.idents)
                FunctionGlobalVariables.Add(_ident.name);
            DeleteInStatementList(_global_statement);
        }

        public override void visit(typed_parameters _typed_parameters)
        {
            FunctionParameters.Add(_typed_parameters.idents.idents[0].name);

            base.visit(_typed_parameters);
        }

        public override void visit(assign _assign)
        {
            if (IsInFunctionBody && _assign.to is ident _ident &&
                !FunctionParameters.Contains(_ident.name) &&
                !FunctionLocalVariables.Contains(_ident.name) &&
                !FunctionGlobalVariables.Contains(_ident.name))
            {
                var _var_statement = SyntaxTreeBuilder.BuildVarStatementNodeFromAssignNode(_assign);
                //if (!_assign.first_assignment_defines_type)
                    //_var_statement.var_def.vars_type = VariablesToDefinitions[_ident.name].var_definitions[0].vars_type;

                ReplaceStatement(_assign, _var_statement);
                FunctionLocalVariables.Add(_ident.name);
                return;
            }

            base.visit(_assign);
        }
    }
}
