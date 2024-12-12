using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Xml.Serialization;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors;

namespace Languages.SPython.Frontend.Converters
{
    public class AssignToVarConverterVisitor : BaseChangeVisitor
    {
        private SymbolTable symbolTable = new SymbolTable();
        private bool isInProgramBlock = false;
        private HashSet<string> skippedFunction = new HashSet<string>();

        public void SendObject(Dictionary<string, HashSet<string>> par)
        {
            symbolTable.ModuleNameToSymbols = par;
        }

        public AssignToVarConverterVisitor() {
            symbolTable.Add("result", NameType.LocalVariable);
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
            if (stn is foreach_stmt         
                ||  stn is while_node           
                ||  stn is procedure_definition)
            {
                symbolTable.OpenLocalScope();
            }
            if (stn is procedure_definition)
            {
                symbolTable.IsInFunctionBody = true;
            }
            if (stn is block && !isInProgramBlock && !symbolTable.IsInFunctionBody)
            {
                isInProgramBlock = true;
                symbolTable.ResetAliases();
            }
            
            base.Enter(stn);
        }

        public override void Exit(syntax_tree_node stn)
        {
            if (stn is procedure_definition)
            {
                symbolTable.IsInFunctionBody = false;
            }
            if (stn is foreach_stmt
                || stn is while_node
                || stn is procedure_definition)
            {
                symbolTable.CloseLocalScope();
            }

            base.Exit(stn);
        }

        public override void visit(procedure_header _procedure_header)
        {
            string name = _procedure_header.name.meth_name.name;
            if (!skippedFunction.Contains(name))
            {
                skippedFunction.Add(name);
                return;
            }

            symbolTable.Add(_procedure_header.name.ToString(), NameType.GlobalFunction);
            base.visit(_procedure_header);
        }

        public override void visit(function_header _function_header)
        {
            string name = _function_header.name.meth_name.name;
            if (!skippedFunction.Contains(name))
            {
                skippedFunction.Add(name);
                return;
            }

            symbolTable.Add(_function_header.name.ToString(), NameType.GlobalFunction);
            base.visit(_function_header);
        }

        public override void visit(dot_node _dot_node)
        {
            if (_dot_node.left is ident left)
            {
                NameType nameType = symbolTable[left.name];
                switch (nameType)
                {
                    case NameType.ModuleAlias:
                        left.name = symbolTable.AliasToRealName(left.name);
                        break;
                    case NameType.ImportedNameAlias:
                        _dot_node.left = new dot_node(new ident(symbolTable.AliasToModuleName(left.name)), left);
                        left.name = symbolTable.AliasToRealName(left.name);
                        break;
                    case NameType.NoType:
                        throw new SyntaxVisitorError("UNEXPECTED_TOKEN_{0}",
                        left.source_context, left.name);
                }
            }
        }

        public override void visit(ident _ident)
        {
            NameType nameType = symbolTable[_ident.name];
            if (nameType == NameType.ImportedNameAlias)
            {
                Replace(_ident, new dot_node(new ident(symbolTable.AliasToModuleName(_ident.name))
                    , new ident(symbolTable.AliasToRealName(_ident.name))));
            }
            else if (nameType == NameType.NoType)
            {
                throw new SyntaxVisitorError("UNEXPECTED_TOKEN_{0}"
                    , _ident.source_context, _ident.name);
            }
        }

        public override void visit(unit_or_namespace _unit_or_namespace)
        {

        }

        public override void visit(unit_name _unit_name)
        {

        }

        public override void visit(foreach_stmt _foreach_stmt)
        {
            symbolTable.Add(_foreach_stmt.identifier.name, NameType.LocalVariable);
            base.visit(_foreach_stmt);
        }

        public override void visit(interface_node _interface_node)
        {
            visit(_interface_node.uses_modules);
            visit(_interface_node.using_namespaces);
            visit(_interface_node.interface_definitions);
        }

        //public override void visit(global_statement _global_statement)
        //{
        //    foreach (ident _ident in _global_statement.idents.idents)
        //        if (symbolTable.localVariables.Contains(_ident.name))
        //            throw new SyntaxVisitorError("Variable local declaration before global statement", 
        //                _global_statement.source_context);
        //        else if (functionParameters.Contains(_ident.name))
        //            throw new SyntaxVisitorError("Variable declared global has the same name as function parameter", 
        //                _global_statement.source_context);
        //        else functionGlobalVariables.Add(_ident.name);
        //}

        public override void visit(typed_parameters _typed_parameters)
        {
            symbolTable.Add(_typed_parameters.idents.idents[0].name, NameType.FunctionParameter);
        }

        public override void visit(var_statement _var_statement)
        {
            symbolTable.Add(_var_statement.var_def.vars.idents[0].name, NameType.LocalVariable);

            base.visit(_var_statement);
        }

        public override void visit(if_node _if_node)
        {
            base.visit(_if_node.condition);

            symbolTable.OpenLocalScope();
            base.visit(_if_node.then_body);
            symbolTable.CloseLocalScope();

            symbolTable.OpenLocalScope();
            base.visit(_if_node.else_body);
            symbolTable.CloseLocalScope();
        }

        public override void visit(import_statement _import_statement)
        {
            foreach (as_statement as_Statement in _import_statement.modules_names.as_statements)
            {
                symbolTable.AddModuleAlias(as_Statement.real_name.name, as_Statement.alias.name);
            }

            var upper = UpperNode();
            if (upper is interface_node _interface_node)
                _interface_node.interface_definitions.ReplaceDescendant<syntax_tree_node, syntax_tree_node>(
                    _import_statement, new empty_statement());
            else Replace(_import_statement, new empty_statement());
        }

        public override void visit(from_import_statement _from_import_statement)
        {
            if (_from_import_statement.is_star)
            {
                foreach (string name in symbolTable.ModuleNameToSymbols[_from_import_statement.module_name.name])
                    symbolTable.AddAlias(name, name, _from_import_statement.module_name.name);
            }
            else
            {
                foreach (as_statement as_Statement in _from_import_statement.imported_names.as_statements)
                {
                    if (!symbolTable.ModuleNameToSymbols[_from_import_statement.module_name.name].Contains(as_Statement.real_name.name))
                        throw new SyntaxVisitorError("No such name in the module",
                       _from_import_statement.source_context);

                    symbolTable.AddAlias(as_Statement.alias.name, as_Statement.real_name.name, _from_import_statement.module_name.name);
                }
            }

            var upper = UpperNode();
            if (upper is interface_node _interface_node)
                _interface_node.interface_definitions.ReplaceDescendant<syntax_tree_node, syntax_tree_node>(
                    _from_import_statement, new empty_statement());
            else Replace(_from_import_statement, new empty_statement());
        }

        public override void visit(variable_definitions _variable_definitions)
        {
            string variable_name = _variable_definitions.var_definitions[0].vars.idents[0].name;
            symbolTable.Add(variable_name, NameType.GlobalVariable);

            base.visit(_variable_definitions);
        }

        public override void visit(assign _assign)
        {
            if (_assign.to is ident _ident) { 
                if (symbolTable[_ident.name] == NameType.NoType) {
                    symbolTable.Add(_ident.name, NameType.LocalVariable);

                    var _var_statement = SyntaxTreeBuilder.BuildVarStatementNodeFromAssignNode(_assign);
                    //if (!_assign.first_assignment_defines_type)
                    //_var_statement.var_def.vars_type = VariablesToDefinitions[_ident.name].var_definitions[0].vars_type;

                    ReplaceStatement(_assign, _var_statement);

                    base.visit(_assign);
                    return;
                }
                symbolTable.Add(_ident.name, NameType.LocalVariable);
            }
            base.visit(_assign);
        }

        [Flags]
        private enum NameType
        {
            NoType              = 0b_0000_0000,
            Keyword             = 0b_0000_0001,
            GlobalVariable      = 0b_0000_0010,
            FunctionParameter   = 0b_0000_0100,
            ModuleAlias         = 0b_0000_1000,
            ImportedNameAlias   = 0b_0001_0000,
            LocalVariable       = 0b_0010_0000,
            GlobalFunction      = 0b_0100_0000
        }

        private class SymbolTable
        {
            private Dictionary<string, NameType> nameTypes = new Dictionary<string, NameType> 
            {
                { "integer",    NameType.Keyword },
                { "real",       NameType.Keyword },
                { "true",       NameType.Keyword },
                { "false",      NameType.Keyword },
                { "break",      NameType.Keyword },
                { "continue",   NameType.Keyword },
                { "exit",       NameType.Keyword }
            };

            private bool isInFunctionBody = false;
            public bool IsInFunctionBody {
                get { return isInFunctionBody; }
                set {
                    isInFunctionBody = value;
                    // enter function
                    if (isInFunctionBody)
                    {

                    }
                    // exit function
                    else
                    {
                        functionGlobalVariables.Clear();
                        functionParameters.Clear();
                    }
                }
            }

            // module alias -> module real name
            private Dictionary<string, string> modulesAliases = new Dictionary<string, string>();
            // alias of function or global variable from module -> real name and module real name
            private Dictionary<string, Tuple<string, string>> aliasToRealNameAndModuleName = new Dictionary<string, Tuple<string, string>>();
            // переменные используемые в функции и не являющиеся локальными для неё
            HashSet<string> functionGlobalVariables = new HashSet<string>();
            // имена формальных параметров функции
            HashSet<string> functionParameters = new HashSet<string>();

            private LocalScope localVariables = new LocalScope();

            // moduleRealName -> functions and global variables in this module real names
            private Dictionary<string, HashSet<string>> moduleNameToSymbols;
            public Dictionary<string, HashSet<string>> ModuleNameToSymbols
            {
                get => moduleNameToSymbols;
                set
                {
                    moduleNameToSymbols = value;

                    ResetAliases();
                }
            }

            public void ResetAliases()
            {
                modulesAliases.Clear();
                aliasToRealNameAndModuleName.Clear();
                AddAliasesFromStandartLibraries();
            }

            private void AddAliasesFromStandartLibraries()
            {
                foreach (string name in moduleNameToSymbols["SpythonSystem"])
                    AddAlias(name, name, "SpythonSystem");
                foreach (string name in moduleNameToSymbols["SpythonHidden"])
                    AddAlias(name, name, "SpythonHidden");
            }

            public void AddAlias(string alias, string realName, string moduleName)
            {
                aliasToRealNameAndModuleName[alias] = Tuple.Create(realName, moduleName);
            }

            public void AddModuleAlias(string realName, string alias)
            {
                modulesAliases[alias] = realName;
            }

            public string AliasToRealName(string alias)
            {
                if (modulesAliases.ContainsKey(alias))
                    return modulesAliases[alias];
                else
                    return aliasToRealNameAndModuleName[alias].Item1;
            }

            public string AliasToModuleName(string alias)
            {
                return aliasToRealNameAndModuleName[alias].Item2;
            }

            public NameType this[string name]
            {
                get { 
                    if (nameTypes.ContainsKey(name) &&
                        (nameTypes[name] != NameType.GlobalVariable
                        || !isInFunctionBody))
                        return nameTypes[name];
                    if (modulesAliases.ContainsKey(name))
                        return NameType.ModuleAlias;
                    if (aliasToRealNameAndModuleName.ContainsKey(name))
                        return NameType.ImportedNameAlias;
                    if (localVariables.Contains(name))
                        return NameType.LocalVariable;
                    if (isInFunctionBody)
                    {
                        if (functionParameters.Contains(name))
                            return NameType.FunctionParameter;
                        if (functionGlobalVariables.Contains(name))
                            return NameType.LocalVariable;
                    }
                    return NameType.NoType; 
                }
            }

            public void Add(string name, NameType nameType)
            {
                switch (nameType)
                {
                    case NameType.LocalVariable:
                        localVariables.Add(name);
                        break;
                    case NameType.FunctionParameter:
                        functionParameters.Add(name); 
                        break;
                    default:
                        if (!nameTypes.ContainsKey(name))
                            nameTypes.Add(name, nameType);
                        break;
                }
            }

            public void OpenLocalScope()
            {
                localVariables = new LocalScope(localVariables);
            }

            public void CloseLocalScope()
            {
                localVariables.ClearScope();
            }

            public class LocalScope
            {
                private LocalScope outerScope;
                private HashSet<string> symbols = new HashSet<string>();

                public LocalScope(LocalScope outerScope = null)
                {
                    this.outerScope = outerScope;
                }

                public bool Contains(string ident)
                {
                    LocalScope curr = this;

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
}
