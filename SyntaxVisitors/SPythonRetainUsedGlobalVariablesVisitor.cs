using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class SPythonRetainUsedGlobalVariablesVisitor : BaseChangeVisitor
    {
        private HashSet<string> VariablesDeclaredGlobal { get; set; }
        private HashSet<string> VariablesUsedGlobal { get; set; }
        private HashSet<string> LocalVariables { get; set; }
        private Dictionary<string, variable_definitions> VariablesToDefinitions{ get; set; }
        private bool IsInFunctionBody { get; set; }
        private bool IsInProgramCode { get; set; }
        private declarations DeclarationsNode { get; set; }

        public SPythonRetainUsedGlobalVariablesVisitor() 
        {
            VariablesDeclaredGlobal = new HashSet<string>();
            VariablesUsedGlobal = new HashSet<string>();
            LocalVariables = new HashSet<string>();
            VariablesToDefinitions = new Dictionary<string, variable_definitions>();

            IsInFunctionBody = false;
            IsInProgramCode = false;
        }

        // нужны методы из BaseChangeVisitor, но порядок обхода из WalkingVisitorNew
        public override void DefaultVisit(syntax_tree_node n)
        {
            for (var i = 0; i < n.subnodes_count; i++)
                ProcessNode(n[i]);
        }

        private bool IsUnusedGlobalVariable(string name) {
            return VariablesDeclaredGlobal.Contains(name) && !VariablesUsedGlobal.Contains(name);
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
            if (stn is declarations _declarations)
            {
                DeclarationsNode = _declarations;
            }

            base.Enter(stn);
        }

        public override void Exit(syntax_tree_node stn)
        {
            if (stn is procedure_definition _procedure_definition)
            {
                IsInFunctionBody = false;
                LocalVariables.Clear();
            }

            base.Exit(stn);
        }

        public override void visit(variable_definitions _variable_definitions)
        {
            string variable_name = _variable_definitions.var_definitions[0].vars.idents[0].name;
            VariablesDeclaredGlobal.Add(variable_name);
            VariablesToDefinitions.Add(variable_name, _variable_definitions);

            base.visit(_variable_definitions);
        }

        public override void visit(var_statement _var_statement)
        {
            if (IsInFunctionBody)
                LocalVariables.Add(_var_statement.var_def.vars.idents[0].name);

            base.visit(_var_statement);
        }

        public override void visit(typed_parameters _typed_parameters)
        {
            LocalVariables.Add(_typed_parameters.idents.idents[0].name);

            base.visit(_typed_parameters);
        }

        public override void visit(ident _ident)
        {
            if (IsInFunctionBody &&
                VariablesDeclaredGlobal.Contains(_ident.name) &&
                !LocalVariables.Contains(_ident.name)
                && !VariablesUsedGlobal.Contains(_ident.name) // для дебага
                )
                VariablesUsedGlobal.Add(_ident.name);

            base.visit(_ident);
        }

        public override void visit(assign _assign)
        {
            if (IsInProgramCode && _assign.to is ident _ident && IsUnusedGlobalVariable(_ident.name))
            {
                VariablesDeclaredGlobal.Remove(_ident.name);
                DeclarationsNode.Remove(VariablesToDefinitions[_ident.name]);

                var _var_statement = SyntaxTreeBuilder.BuildVarStatementNodeFromAssignNode(_assign);
                ReplaceStatement(_assign, _var_statement);
                return;
            }
            
            base.visit(_assign);
        }
    }
}
