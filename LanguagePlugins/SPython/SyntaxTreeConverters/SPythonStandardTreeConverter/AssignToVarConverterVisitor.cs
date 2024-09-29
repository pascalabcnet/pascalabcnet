using System.Collections.Generic;
using System.Data;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors;

namespace Languages.SPython.Frontend.Converters
{
    public class AssignToVarConverterVisitor : BaseChangeVisitor
    {
        // переменные используемые в функции и не являющиеся локальными для неё
        HashSet<string> functionGlobalVariables = new HashSet<string>();
        // параметры текущей просматриваемой функции
        HashSet<string> functionParameters = new HashSet<string>();
        SymbolTable localVariables = new SymbolTable();
        HashSet<string> globalVariables = new HashSet<string>();
        bool isInFunctionBody = false;

        public AssignToVarConverterVisitor() {}

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
                localVariables = new SymbolTable(localVariables);
            }
            if (stn is procedure_definition)
            {
                isInFunctionBody = true;
            }

            base.Enter(stn);
        }

        public override void Exit(syntax_tree_node stn)
        {
            if (stn is procedure_definition)
            {
                isInFunctionBody = false;
                functionGlobalVariables.Clear();
                functionParameters.Clear();
            }
            if (stn is statement_list)
                localVariables.ClearScope();


            base.Exit(stn);
        }

        public override void visit(global_statement _global_statement)
        {
            foreach (ident _ident in _global_statement.idents.idents)
                if (localVariables.Contains(_ident.name))
                    throw new SyntaxVisitorError("Variable local declaration before global statement", 
                        _global_statement.source_context);
                else if (functionParameters.Contains(_ident.name))
                    throw new SyntaxVisitorError("Variable declared global has the same name as function parameter", 
                        _global_statement.source_context);
                else functionGlobalVariables.Add(_ident.name);
        }

        public override void visit(typed_parameters _typed_parameters)
        {
            functionParameters.Add(_typed_parameters.idents.idents[0].name);
        }

        public override void visit(var_statement _var_statement)
        {
            localVariables.Add(_var_statement.var_def.vars.idents[0].name);
        }

        public override void visit(variable_definitions _variable_definitions)
        {
            string variable_name = _variable_definitions.var_definitions[0].vars.idents[0].name;
            globalVariables.Add(variable_name);
        }

        public override void visit(assign _assign)
        {
            if (_assign.to is ident _ident) { 
                if (
                    !functionParameters.Contains(_ident.name) &&
                    !localVariables.Contains(_ident.name) &&
                    !functionGlobalVariables.Contains(_ident.name) &&
                    (!globalVariables.Contains(_ident.name) || isInFunctionBody)) {
                    localVariables.Add(_ident.name);

                    var _var_statement = SyntaxTreeBuilder.BuildVarStatementNodeFromAssignNode(_assign);
                    //if (!_assign.first_assignment_defines_type)
                    //_var_statement.var_def.vars_type = VariablesToDefinitions[_ident.name].var_definitions[0].vars_type;

                    ReplaceStatement(_assign, _var_statement);

                    return;
                }
                localVariables.Add(_ident.name);
            }
                
        }


        public class SymbolTable
        {
            private SymbolTable outerScope;
            private HashSet<string> symbols = new HashSet<string>();
            public SymbolTable(SymbolTable outerScope = null)
            {
                this.outerScope = outerScope;
            }

            public bool Contains(string ident)
            {
                SymbolTable curr = this;

                while (curr != null)
                {
                    if (curr.symbols.Contains(ident))
                        return true;
                    curr = curr.outerScope;
                }

                return false;
            }

            public void Add(string ident)
            {
                symbols.Add(ident);
            }

            public void ClearScope()
            {
                if (outerScope != null)
                {
                    symbols = outerScope.symbols;
                    outerScope = outerScope.outerScope;
                }
            }
        }
    }
}
