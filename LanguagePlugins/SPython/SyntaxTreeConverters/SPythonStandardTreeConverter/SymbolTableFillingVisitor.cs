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
    // заполняет таблицу символов динамически, при проходе по дереву сверху вниз
    public class SymbolTableFillingVisitor : BaseChangeVisitor
    {
        protected SymbolTable symbolTable;

        public SymbolTableFillingVisitor(Dictionary<string, HashSet<string>> par) {
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
            if (stn is statement_list)
            {
                symbolTable.OpenLocalScope();
            }
            if (stn is procedure_definition || stn is function_lambda_definition)
            {
                symbolTable.IsInFunctionBody = true;
                symbolTable.OpenLocalScope();
            }
            
            base.Enter(stn);
        }

        public override void Exit(syntax_tree_node stn)
        {
            if (stn is procedure_definition || stn is function_lambda_definition)
            {
                symbolTable.IsInFunctionBody = false;
                symbolTable.CloseLocalScope();
            }
            if (stn is statement_list)
            {
                symbolTable.CloseLocalScope();
            }

            base.Exit(stn);
        }

        public override void visit(procedure_header _procedure_header)
        {
            string procedure_name = _procedure_header.name.meth_name.name;
            symbolTable.Add(procedure_name, NameKind.GlobalFunction);
            base.visit(_procedure_header);
        }

        public override void visit(function_header _function_header)
        {
            visit(_function_header as procedure_header);
        }

        public override void visit(function_lambda_definition _function_lambda_definition)
        {
            foreach (ident id in _function_lambda_definition.ident_list.list)
                symbolTable.Add(id.name, NameKind.LocalVariable);

            base.visit(_function_lambda_definition);
        }

        public override void visit(foreach_stmt _foreach_stmt)
        {
            symbolTable.OpenLocalScope();
            symbolTable.Add(_foreach_stmt.identifier.name, NameKind.LocalVariable);
            base.visit(_foreach_stmt);
            symbolTable.CloseLocalScope();
        }

        public override void visit(interface_node _interface_node)
        {
            ProcessNode(_interface_node.uses_modules);
            ProcessNode(_interface_node.using_namespaces);
            ProcessNode(_interface_node.interface_definitions);
        }

        public override void visit(global_statement _global_statement)
        {
            foreach (ident _ident in _global_statement.idents.idents)
            {
                symbolTable.MakeVisibleForAssignment(_ident.name);
            }
        }

        public override void visit(typed_parameters _typed_parameters)
        {
            foreach (ident id in _typed_parameters.idents.idents)
                symbolTable.Add(id.name, NameKind.LocalVariable);

            base.visit(_typed_parameters);
        }

        public override void visit(var_statement _var_statement)
        {
            foreach (ident id in _var_statement.var_def.vars.idents)
                symbolTable.Add(id.name, NameKind.LocalVariable);

            base.visit(_var_statement);
        }

        public override void visit(import_statement _import_statement)
        {
            foreach (as_statement as_Statement in _import_statement.modules_names.as_statements)
            {
                string real_name = as_Statement.real_name.name;
                string alias = as_Statement.alias.name;
                if (SymbolTable.specialModulesAliases.ContainsKey(real_name))
                    real_name = SymbolTable.specialModulesAliases[real_name];
                symbolTable.AddModuleAlias(real_name, alias);
            }
        }

        public override void visit(from_import_statement _from_import_statement)
        {
            string module_real_name = _from_import_statement.module_name.name;
            string module_name = module_real_name;
            if (SymbolTable.specialModulesAliases.ContainsKey(module_name))
                module_name = SymbolTable.specialModulesAliases[module_name];

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
        }

        public override void visit(variable_definitions _variable_definitions)
        {
            foreach (var_def_statement vds in _variable_definitions.var_definitions)
            {
                foreach (ident id in vds.vars.idents)
                {
                    symbolTable.Add(id.name, NameKind.GlobalVariable);
                    base.visit(_variable_definitions);
                }
            }
        }

        [Flags]
        protected enum NameKind
        {
            // Имя отсутствует в текущем контексте
            Unknown             = 0b_0000_0000,
            // Ключевые слова языка
            Keyword             = 0b_0000_0001,
            // Имя глобальной переменной
            GlobalVariable      = 0b_0000_0010,
            // Имя глобальной функции
            GlobalFunction      = 0b_0000_0100,
            // Имя подключённого модуля или его псевдоним 
            ModuleAlias         = 0b_0000_1000,
            // Имя, подключённое из модуля, или его псевдоним 
            ImportedNameAlias   = 0b_0001_0000,
            // Локальня переменная
            LocalVariable       = 0b_0010_0000,
        }

        protected class SymbolTable
        {
            private Dictionary<string, NameKind> nameTypes = new Dictionary<string, NameKind>();

            static string[] Keywords = {
                    "integer"
                    , "real"
                    , "string"
                    , "boolean"

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

            public SymbolTable(Dictionary<string, HashSet<string>> par)
            {
                ModuleNameToSymbols = par;
                FillKeywords();
                AddAliasesFromStandartLibraries();
            }

            public static Dictionary<string, string> specialModulesAliases = new Dictionary<string, string>
            {
                { "time", "time1" },
                { "random", "random1" },
            };

            // names added to current function with global statements
            private HashSet<string> NamesAddedByGlobal = new HashSet<string>();

            private bool isInFunctionBody = false;

            public bool IsInFunctionBody {
                get { return isInFunctionBody; }
                set 
                { 
                    isInFunctionBody = value; 
                    if (isInFunctionBody)
                    {
                        Add("result", NameKind.LocalVariable);
                    }
                    else
                    {
                        NamesAddedByGlobal.Clear();
                    }
                }
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

            public void MakeVisibleForAssignment(string name)
            {
                NamesAddedByGlobal.Add(name);
            }

            public bool IsVisibleForAssignment(string name)
            {
                NameKind kind = this[name];
                if (kind == NameKind.Unknown) return false;
                if (!isInFunctionBody) return true;
                return (kind != NameKind.GlobalVariable &&
                    kind != NameKind.ImportedNameAlias) ||
                    NamesAddedByGlobal.Contains(name);
            }

            public NameKind this[string name]
            {
                get { 
                    if (localVariables.Contains(name))
                        return NameKind.LocalVariable;
                    if (nameTypes.ContainsKey(name))
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

            public bool IsOutermostScope()
            {
                return localVariables.IsOutermostScope();
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

                public bool IsOutermostScope()
                {
                    return outerScope.outerScope == null;
                }
            }
        }
    }
}
