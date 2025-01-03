using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Xml.Linq;
using System.Xml.Serialization;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors;

namespace Languages.SPython.Frontend.Converters
{
    public class NameInterpreterVisitor : BaseChangeVisitor
    {
        private SymbolTable symbolTable;
        private bool isInProgramBlock = false;
        private HashSet<string> skippedFunction = new HashSet<string>();

        public NameInterpreterVisitor(Dictionary<string, HashSet<string>> par) {
            symbolTable = new SymbolTable(par);
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
                || stn is for_node
                ||  stn is while_node           
                ||  stn is procedure_definition)
            {
                symbolTable.OpenLocalScope();
            }
            if (stn is procedure_definition)
            {
                symbolTable.Add("result", NameKind.LocalVariable);
                symbolTable.IsInFunctionBody = true;
            }
            if (stn is block && !isInProgramBlock && !symbolTable.IsInFunctionBody)
            {
                isInProgramBlock = true;
                symbolTable.ResetDictionaries();
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
                || stn is for_node
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

            symbolTable.Add(_procedure_header.name.ToString(), NameKind.GlobalFunction);
            base.visit(_procedure_header);
        }

        public override void visit(name_assign_expr _name_assign_expr)
        {
        }

        public override void visit(function_header _function_header)
        {
            string name = _function_header.name.meth_name.name;
            if (!skippedFunction.Contains(name))
            {
                skippedFunction.Add(name);
                return;
            }

            symbolTable.Add(_function_header.name.ToString(), NameKind.GlobalFunction);
            base.visit(_function_header);
        }

        public override void visit(dot_node _dot_node)
        {
            if (_dot_node.left is ident left)
            {
                NameKind nameType = symbolTable[left.name];
                switch (nameType)
                {
                    case NameKind.ModuleAlias:
                        left.name = symbolTable.AliasToRealName(left.name);
                        break;
                    case NameKind.ImportedNameAlias:
                        _dot_node.left = new dot_node(new ident(symbolTable.AliasToModuleName(left.name)), left);
                        left.name = symbolTable.AliasToRealName(left.name);
                        break;
                    case NameKind.Unknown:
                        throw new SPythonSyntaxVisitorError("UNKNOWN_NAME_{0}",
                        left.source_context, left.name);
                }
            }
        }

        public override void visit(ident _ident)
        {
            NameKind nameType = symbolTable[_ident.name];
            if (nameType == NameKind.ImportedNameAlias)
            {
                Replace(_ident, new dot_node(new ident(symbolTable.AliasToModuleName(_ident.name))
                    , new ident(symbolTable.AliasToRealName(_ident.name))));
            }
            else if (nameType == NameKind.Unknown)
            {
                throw new SPythonSyntaxVisitorError("UNKNOWN_NAME_{0}"
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
            symbolTable.Add(_foreach_stmt.identifier.name, NameKind.LocalVariable);
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
            symbolTable.Add(_typed_parameters.idents.idents[0].name, NameKind.LocalVariable);

            base.visit(_typed_parameters);
        }

        public override void visit(var_statement _var_statement)
        {
            symbolTable.Add(_var_statement.var_def.vars.idents[0].name, NameKind.LocalVariable);

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
                string real_name = as_Statement.real_name.name;
                string alias = as_Statement.alias.name;
                if (symbolTable.specialModulesAliases.ContainsKey(real_name))
                    real_name = symbolTable.specialModulesAliases[real_name];
                symbolTable.AddModuleAlias(real_name, alias);
            }

            var upper = UpperNode();
            if (upper is interface_node _interface_node)
                _interface_node.interface_definitions.ReplaceDescendant<syntax_tree_node, syntax_tree_node>(
                    _import_statement, new empty_statement());
            else Replace(_import_statement, new empty_statement());
        }

        public override void visit(from_import_statement _from_import_statement)
        {
            string module_real_name = _from_import_statement.module_name.name;
            string module_name = module_real_name;
            if (symbolTable.specialModulesAliases.ContainsKey(module_name))
                module_name = symbolTable.specialModulesAliases[module_name];

            if (_from_import_statement.is_star)
            {
                foreach (string name in symbolTable.ModuleNameToSymbols[module_name])
                    symbolTable.AddAlias(name, name, module_name);
            }
            else
            {
                foreach (as_statement as_Statement in _from_import_statement.imported_names.as_statements)
                {
                    if (!symbolTable.ModuleNameToSymbols[module_name].Contains(as_Statement.real_name.name))
                        throw new SPythonSyntaxVisitorError("MODULE_{0}_HAS_NO_NAME_{1}",
                       _from_import_statement.source_context, module_real_name, as_Statement.real_name.name);

                    symbolTable.AddAlias(as_Statement.real_name.name, as_Statement.alias.name, module_name);
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
            symbolTable.Add(variable_name, NameKind.GlobalVariable);

            base.visit(_variable_definitions);
        }

        public override void visit(assign _assign)
        {
            if (_assign.to is ident _ident) { 
                if (symbolTable[_ident.name] == NameKind.Unknown) {
                    symbolTable.Add(_ident.name, NameKind.LocalVariable);

                    var _var_statement = SyntaxTreeBuilder.BuildVarStatementNodeFromAssignNode(_assign);
                    //if (!_assign.first_assignment_defines_type)
                    //_var_statement.var_def.vars_type = VariablesToDefinitions[_ident.name].var_definitions[0].vars_type;

                    ReplaceStatement(_assign, _var_statement);

                    base.visit(_assign);
                    return;
                }
                symbolTable.Add(_ident.name, NameKind.LocalVariable);
            }
            base.visit(_assign);
        }

        [Flags]
        private enum NameKind
        {
            Unknown             = 0b_0000_0000,
            Keyword             = 0b_0000_0001,
            GlobalVariable      = 0b_0000_0010,
            GlobalFunction      = 0b_0000_0100,
            ModuleAlias         = 0b_0000_1000,
            ImportedNameAlias   = 0b_0001_0000,
            LocalVariable       = 0b_0010_0000,
        }

        private class SymbolTable
        {
            private Dictionary<string, NameKind> nameTypes = new Dictionary<string, NameKind>();

            private Dictionary<string, NameKind> globalNamesReserve = new Dictionary<string, NameKind>();

            static string[] Keywords = {
                    "integer"
                    , "real"
                    , "true"
                    , "false"
                    , "break"
                    , "continue"
                    , "exit"
            };

            private void FillKeywords()
            {
                foreach (var keyword in Keywords)
                    nameTypes.Add(keyword, NameKind.Keyword);
            }

            // Erase everything, except global variables and keywords
            public void ResetDictionaries()
            {
                nameTypes.Clear();
                FillKeywords();
                foreach (var kvpair in globalNamesReserve)
                    nameTypes.Add(kvpair.Key, kvpair.Value);
                globalNamesReserve.Clear();
                modulesAliases.Clear();
                aliasToRealNameAndModuleName.Clear();
                AddAliasesFromStandartLibraries();
            }

            public SymbolTable(Dictionary<string, HashSet<string>> par)
            {
                ModuleNameToSymbols = par;
                ResetDictionaries();
            }

            public Dictionary<string, string> specialModulesAliases = new Dictionary<string, string>
            {
                { "time", "time1" },
                { "random", "random1" },
            };

            private bool isInFunctionBody = false;
            public bool IsInFunctionBody {
                get { return isInFunctionBody; }
                set { isInFunctionBody = value; }
            }

            // module alias -> module real name
            private Dictionary<string, string> modulesAliases = new Dictionary<string, string>();
            // alias of function or global variable from module -> real name and module real name
            private Dictionary<string, Tuple<string, string>> aliasToRealNameAndModuleName = new Dictionary<string, Tuple<string, string>>();

            private LocalScope localVariables = new LocalScope();

            // moduleRealName -> functions and global variables in this module real names
            private Dictionary<string, HashSet<string>> moduleNameToSymbols;
            public Dictionary<string, HashSet<string>> ModuleNameToSymbols
            {
                get => moduleNameToSymbols;
                set
                { moduleNameToSymbols = value; }
            }

            private void AddAliasesFromStandartLibraries()
            {
                foreach (string name in moduleNameToSymbols["SPythonSystem"])
                    AddAlias(name, name, "SPythonSystem");
                foreach (string name in moduleNameToSymbols["SPythonHidden"])
                    AddAlias(name, name, "SPythonHidden");
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

            public NameKind this[string name]
            {
                get { 
                    if (localVariables.Contains(name))
                        return NameKind.LocalVariable;
                    if (nameTypes.ContainsKey(name) &&
                        (nameTypes[name] != NameKind.GlobalVariable
                        || !isInFunctionBody))
                        return nameTypes[name];
                    return NameKind.Unknown; 
                }
            }

            public void Add(string name, NameKind nameType)
            {
                switch (nameType)
                {
                    case NameKind.LocalVariable:
                        localVariables.Add(name);
                        break;
                    case NameKind.GlobalVariable:
                    case NameKind.GlobalFunction:
                        AddGlobalName(name, nameType);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            private void AddGlobalName(string name, NameKind nameType)
            {
                if (nameType == NameKind.GlobalVariable
                    || nameType == NameKind.GlobalFunction)
                    if (!globalNamesReserve.ContainsKey(name))
                        globalNamesReserve.Add(name, nameType);

                EraseNameAsLocal(name);

                if (nameTypes.ContainsKey(name))
                    nameTypes[name] = nameType;
                else nameTypes.Add(name, nameType);
            }

            public void AddAlias(string realName, string alias, string moduleName)
            {
                if (aliasToRealNameAndModuleName.ContainsKey(alias))
                    aliasToRealNameAndModuleName[alias] = Tuple.Create(realName, moduleName);
                else aliasToRealNameAndModuleName.Add(alias, Tuple.Create(realName, moduleName));
                AddGlobalName(alias, NameKind.ImportedNameAlias);
            }

            public void AddModuleAlias(string realName, string alias)
            {
                if (modulesAliases.ContainsKey(alias))
                    modulesAliases[alias] = realName;
                else modulesAliases.Add(alias, realName);
                AddGlobalName(alias, NameKind.ModuleAlias);
            }

            private void EraseNameAsLocal(string name)
            {
                localVariables.EraseName(name);
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

                public bool Contains(string name)
                {
                    LocalScope curr = this;

                    while (curr != null)
                    {
                        if (curr.symbols.Contains(name))
                            return true;
                        curr = curr.outerScope;
                    }

                    return false;
                }

                public void EraseName(string name)
                {
                    LocalScope curr = this;

                    while (curr != null)
                    {
                        //if (curr.symbols.Contains(name))
                        curr.symbols.Remove(name);
                        curr = curr.outerScope;
                    }
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
