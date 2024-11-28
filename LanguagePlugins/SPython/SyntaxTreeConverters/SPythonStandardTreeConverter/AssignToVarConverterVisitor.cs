using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization;
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
        HashSet<string> importedModules = new HashSet<string>();
        Dictionary<string, string> modulesAliases = new Dictionary<string, string>()
        {
            {"SpythonSystem", "SpythonSystem"},
            {"SpythonHidden", "SpythonHidden"},
        };
        Dictionary<string, string> aliasToRealName = new Dictionary<string, string>();
        Dictionary<string, string> aliasToModuleName = new Dictionary<string, string>();

        private Dictionary<string, HashSet<string>> moduleNameToSymbols;

        public Dictionary<string, HashSet<string>> ModuleNameToSymbols 
        { 
            get => moduleNameToSymbols; 
            set
            {
                moduleNameToSymbols = value;

                foreach (string name in moduleNameToSymbols["SpythonSystem"])
                {
                    aliasToRealName[name] = name;
                    aliasToModuleName[name] = "SpythonSystem";
                }
                foreach (string name in moduleNameToSymbols["SpythonHidden"])
                {
                    aliasToRealName[name] = name;
                    aliasToModuleName[name] = "SpythonHidden";
                }
            }
        }

        HashSet<string> keyWords = new HashSet<string>()
        {
            "integer", "true", "false", "int", "break", "exit"
        };

        bool isInFunctionBody = false;

        public AssignToVarConverterVisitor() {
            localVariables.Add("result");

        }

        // нужны методы из BaseChangeVisitor, но порядок обхода из WalkingVisitorNew
        public override void DefaultVisit(syntax_tree_node n)
        {
            if (n == null) return;
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

        public override void visit(procedure_header _procedure_header)
        {
            globalVariables.Add(_procedure_header.name.ToString());
            base.visit(_procedure_header);
        }

        public override void visit(function_header _function_header)
        {
            globalVariables.Add(_function_header.name.ToString());
            base.visit(_function_header);
        }

        public override void visit(dot_node _dot_node)
        {
            if (_dot_node.left is ident left)
                if (modulesAliases.ContainsKey(left.name))
                    left.name = modulesAliases[left.name];
                else if (!localVariables.Contains(left.name) &&
                        !globalVariables.Contains(left.name) &&
                        !functionParameters.Contains(left.name) &&
                        aliasToRealName.ContainsKey(left.name))
                {
                    _dot_node.left = new dot_node(new ident(aliasToModuleName[left.name]), left);
                    left.name = aliasToRealName[left.name];
                }
                else if (!localVariables.Contains(left.name) &&
                        !globalVariables.Contains(left.name) &&
                        !functionParameters.Contains(left.name) &&
                        !modulesAliases.ContainsKey(left.name) &&
                        !importedModules.Contains(left.name) &&
                        !keyWords.Contains(left.name))
                {
                    throw new SyntaxVisitorError("Unknown name " + left.name,
                        _dot_node.source_context);
                }
        }

        public override void visit(ident _ident)
        {
            if (!localVariables.Contains(_ident.name) &&
                !globalVariables.Contains(_ident.name) &&
                !functionParameters.Contains(_ident.name) &&
                aliasToRealName.ContainsKey(_ident.name))
            {
                Replace(_ident, new dot_node(new ident(aliasToModuleName[_ident.name]), new ident(aliasToRealName[_ident.name])));
            }
            else if (
                !localVariables.Contains(_ident.name) &&
                !globalVariables.Contains(_ident.name) &&
                !functionParameters.Contains(_ident.name) &&
                !modulesAliases.ContainsKey(_ident.name) &&
                !keyWords.Contains(_ident.name))
            {
                throw new SyntaxVisitorError("Unknown name " + _ident.name,
                        _ident.source_context);
            }
        }

        public override void visit(unit_name _unit_name)
        {
            globalVariables.Add(_unit_name.idunit_name.name);
        }

        public override void visit(interface_node _interface_node)
        {
            visit(_interface_node.uses_modules);
            visit(_interface_node.using_namespaces);
            visit(_interface_node.interface_definitions);
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

            base.visit(_var_statement);
        }

        public override void visit(unit_or_namespace _unit_or_Namespace)
        {
            foreach (ident id in _unit_or_Namespace.name.idents)
                modulesAliases[id.name] = id.name;
        }

        public override void visit(import_statement _import_statement)
        {
            foreach (as_statement as_Statement in _import_statement.modules_names.as_statements)
            {
                importedModules.Add(as_Statement.real_name.name);
                modulesAliases[as_Statement.alias.name] = as_Statement.real_name.name;
            }

            ReplaceStatement(_import_statement, new empty_statement());
        }

        public override void visit(from_import_statement _from_import_statement)
        {
            importedModules.Add(_from_import_statement.module_name.name);

            if (_from_import_statement.is_star)
            {
                foreach (string name in moduleNameToSymbols[_from_import_statement.module_name.name])
                {
                    aliasToRealName[name] = name;
                    aliasToModuleName[name] = _from_import_statement.module_name.name;
                }
            }
            else
            {
                foreach (as_statement as_Statement in _from_import_statement.imported_names.as_statements)
                {
                    if (!moduleNameToSymbols[_from_import_statement.module_name.name].Contains(as_Statement.real_name.name))
                        throw new SyntaxVisitorError("No such name in the module",
                       _from_import_statement.source_context);

                    aliasToRealName[as_Statement.alias.name] = as_Statement.real_name.name;
                    aliasToModuleName[as_Statement.alias.name] = _from_import_statement.module_name.name;
                }
            }

            ReplaceStatement(_from_import_statement, new empty_statement());
        }

        public override void visit(variable_definitions _variable_definitions)
        {
            string variable_name = _variable_definitions.var_definitions[0].vars.idents[0].name;
            globalVariables.Add(variable_name);

            base.visit(_variable_definitions);
        }

        public override void visit(assign _assign)
        {
            if (_assign.to is ident _ident) { 
                if (
                    !importedModules.Contains(_ident.name) &&
                    !functionParameters.Contains(_ident.name) &&
                    !localVariables.Contains(_ident.name) &&
                    !functionGlobalVariables.Contains(_ident.name) &&
                    (!globalVariables.Contains(_ident.name) || isInFunctionBody)) {
                    localVariables.Add(_ident.name);

                    var _var_statement = SyntaxTreeBuilder.BuildVarStatementNodeFromAssignNode(_assign);
                    //if (!_assign.first_assignment_defines_type)
                    //_var_statement.var_def.vars_type = VariablesToDefinitions[_ident.name].var_definitions[0].vars_type;

                    ReplaceStatement(_assign, _var_statement);

                    base.visit(_assign);
                    return;
                }
                localVariables.Add(_ident.name);
            }
            base.visit(_assign);
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
