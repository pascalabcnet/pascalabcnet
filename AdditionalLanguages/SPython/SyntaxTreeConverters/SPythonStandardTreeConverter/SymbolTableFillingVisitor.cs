using PascalABCCompiler;
using PascalABCCompiler.Parsers;
using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;

namespace Languages.SPython.Frontend.Converters
{
    // заполняет таблицу символов динамически, при проходе по дереву сверху вниз
    public class SymbolTableFillingVisitor : BaseChangeVisitor
    {
        protected SymbolTable symbolTable;

        private readonly ILanguageInformation languageInformation = Facade.LanguageProvider.Instance.SelectLanguageByName("SPython").LanguageInformation;

        public SymbolTableFillingVisitor(Dictionary<string, Dictionary<string, bool>> par) {
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
                symbolTable.OpenLocalScope();
                symbolTable.IsInFunctionBody = true;
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

        public override void visit(semantic_check_sugared_statement_node _)
        {
            // Скипаем. Узел нужен на семантике, здесь порождает лишние ошибки.
        }

        private bool IsForwardDeclaration(procedure_header _procedure_header)
        {
            foreach (procedure_attribute attr in _procedure_header.proc_attributes.proc_attributes)
                if (attr.attribute_type is proc_attribute.attr_forward)
                    return true;

            return false;
        }

        public override void visit(procedure_header _procedure_header)
        {
            string procedure_name = _procedure_header.name.meth_name.name;
            // Сейчас здесь не могут быть встречены forward-объявления функций
            // но на будущее...
            if (IsForwardDeclaration(_procedure_header))
            {
                symbolTable.Add(procedure_name, NameKind.ForwardDeclaredFunction);
            }
            else
            {
                symbolTable.AddToParentScope(procedure_name, NameKind.GlobalFunction);
            }
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

            if (_foreach_stmt.ext is ident_list _ident_list)
            {
                foreach (ident _ident in _ident_list.idents)
                {
                    symbolTable.Add(_ident.name, NameKind.LocalVariable);
                }
            }

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
                if (languageInformation.SpecialModulesAliases.ContainsKey(real_name))
                    real_name = languageInformation.SpecialModulesAliases[real_name];
                symbolTable.AddModuleAlias(real_name, alias);
            }
        }

        public override void visit(from_import_statement _from_import_statement)
        {
            string module_real_name = _from_import_statement.module_name.name;
            string module_name = module_real_name;
            if (languageInformation.SpecialModulesAliases.ContainsKey(module_name))
                module_name = languageInformation.SpecialModulesAliases[module_name];

            if (_from_import_statement.is_star)
            {
                foreach (var kv in symbolTable.ModuleNameToSymbols[module_name])
                    symbolTable.AddAlias(kv.Key, kv.Value, kv.Key, module_name);
            }
            else
            {
                foreach (as_statement as_Statement in _from_import_statement.imported_names.as_statements)
                {
                    var namesDict = symbolTable.ModuleNameToSymbols[module_name];

                    if (!namesDict.ContainsKey(as_Statement.real_name.name))
                        throw new SPythonSyntaxVisitorError("MODULE_{0}_HAS_NO_NAME_{1}",
                       _from_import_statement.source_context, module_real_name, as_Statement.real_name.name);

                    symbolTable.AddAlias(as_Statement.real_name.name, namesDict[as_Statement.real_name.name], as_Statement.alias.name, module_name);
                }
            }
        }

        // Возможно рудимент (Выпилить, если не понадобится) 13.12.2025  EVA
/*        public override void visit(variable_definitions _variable_definitions)
        {
            foreach (var_def_statement vds in _variable_definitions.var_definitions)
            {
                foreach (ident id in vds.vars.idents)
                {
                    symbolTable.Add(id.name, NameKind.GlobalVariable);
                }
            }
            base.visit(_variable_definitions);
        }*/

        [Flags]
        protected enum NameKind
        {
            // Имя отсутствует в текущем контексте
            Unknown                  = 0b_0000_0000,
            // Ключевые слова языка
            Keyword                  = 0b_0000_0001,
            // Имя глобальной переменной
            GlobalVariable           = 0b_0000_0010,
            // Имя глобальной функции
            GlobalFunction           = 0b_0000_0100,
            // Имя подключённого модуля или его псевдоним
            ModuleAlias              = 0b_0000_1000,
            // Имя, подключённое из модуля (не переменная или константа), или его псевдоним 
            ImportedNotVariableAlias = 0b_0001_0000,
            // Подключенная из модуля константа или переменная
            ImportedVariableAlias    = 0b_0010_0000,
            // Локальня переменная
            LocalVariable            = 0b_0100_0000,
            // Имя глобальной forward-объявленной функции 
            ForwardDeclaredFunction  = 0b_1000_0000,
        }

        protected class SymbolTable
        {
            private readonly HashSet<string> forwardDeclaredFunctions = new HashSet<string>();

            private readonly string[] Keywords = {
                "int", "float", "str", "bool", "tuple", // standard types
                "break", "continue", "exit", "halt",    // standard ops
                "true", "false",                        // constants
            };

            private void FillKeywords()
            {
                foreach (var keyword in Keywords)
                    currentScope.Add(keyword, NameKind.Keyword);
            }

            public SymbolTable(Dictionary<string, Dictionary<string, bool>> par)
            {
                ModuleNameToSymbols = par;
                entryScope = new LocalScope();
                currentScope = entryScope;
                FillKeywords();
                AddAliasesFromStandartLibraries();
            }

            // names added to current function with global statements
            private readonly HashSet<string> NamesAddedByGlobal = new HashSet<string>();

            private bool isInFunctionBody = false;

            public bool IsInFunctionBody {
                get { return isInFunctionBody; }
                set 
                { 
                    isInFunctionBody = value; 
                    if (isInFunctionBody)
                    {
                        Add(StringConstants.result_var_name, NameKind.LocalVariable);
                    }
                    else
                    {
                        NamesAddedByGlobal.Clear();
                    }
                }
            }

            private readonly string[] StandardLibraries = Facade.LanguageProvider.Instance.SelectLanguageByName("SPython").SystemUnitNames;

            private LocalScope currentScope;

            private readonly LocalScope entryScope;

            // moduleRealName -> functions and global variables in this module real names
            public Dictionary<string, Dictionary<string, bool>> ModuleNameToSymbols { get; set; }

            private void AddAliasesFromStandartLibraries()
            {
                foreach (string standardLibrary in StandardLibraries)
                {
                    if (ModuleNameToSymbols.ContainsKey(standardLibrary))
                        foreach (var kv in ModuleNameToSymbols[standardLibrary])
                            AddAlias(kv.Key, kv.Value, kv.Key, standardLibrary);
                }
            }

            public string AliasToRealName(string alias)
            {
                return entryScope.GetRealName(alias);
            }

            public string AliasToModuleName(string alias)
            {
                return entryScope.GetDeclaredModuleName(alias);
            }

            public void MakeVisibleForAssignment(string name)
            {
                NamesAddedByGlobal.Add(name);
            }

            public bool IsVisibleToAssign(string name)
            {
                NameKind kind = this[name];
                if (kind == NameKind.Unknown || kind == NameKind.ImportedNotVariableAlias) return false;

                if (!isInFunctionBody) return true;
                return (kind != NameKind.GlobalVariable &&
                    kind != NameKind.ImportedVariableAlias) ||
                    NamesAddedByGlobal.Contains(name);
            }

            public NameKind this[string name]
            {
                get { 
                    if (currentScope.Contains(name))
                        return currentScope.GetNameKind(name);
                    if (forwardDeclaredFunctions.Contains(name))
                        return NameKind.ForwardDeclaredFunction;
                    return NameKind.Unknown; 
                }
            }

            public void Add(string name, NameKind nameType)
            {
                switch (nameType)
                {
                    case NameKind.LocalVariable:
                    case NameKind.GlobalVariable:
                    case NameKind.GlobalFunction:
                        currentScope.Add(name, nameType);
                        break;
                    case NameKind.ForwardDeclaredFunction:
                        forwardDeclaredFunctions.Add(name);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            public void AddToParentScope(string name, NameKind nameType)
            {
                currentScope.OuterScope.Add(name, nameType);
            }

            public void AddAlias(string realName, bool isVariable, string alias, string moduleName)
            {                
                entryScope.Add(alias, realName, isVariable ? NameKind.ImportedVariableAlias : NameKind.ImportedNotVariableAlias, moduleName);
            }

            public void AddModuleAlias(string realName, string alias)
            {
                entryScope.Add(alias, realName, NameKind.ModuleAlias);
            }

            // Возможно рудимент (Выпилить, если не понадобится) 13.12.2025  EVA 
            /*private void EraseNameAsLocal(string name)
            {
                localVariables.EraseName(name);
            }*/

            public void OpenLocalScope()
            {
                currentScope = new LocalScope(currentScope);
            }

            public void CloseLocalScope()
            {
                currentScope.ClearScope();
            }

            public bool IsOutermostScope()
            {
                return currentScope.IsOutermostScope();
            }

            private class LocalScope
            {
                private class SymbolInfo
                {
                    internal string realName;
                    internal NameKind kind;
                    internal string declaredModuleName;

                    internal SymbolInfo(string realName, NameKind kind, string declaredModuleName)
                    {
                        this.realName = realName;
                        this.kind = kind;
                        this.declaredModuleName = declaredModuleName;
                    }
                }

                public LocalScope OuterScope { get; private set; }
                private Dictionary<string, SymbolInfo> symbolsInfos = new Dictionary<string, SymbolInfo>();

                public LocalScope(LocalScope outerScope = null)
                {
                    this.OuterScope = outerScope;
                }

                public bool Contains(string name) => FindNameRecursive(name) != null;

                // Возможно рудимент (Выпилить, если не понадобится) 13.12.2025  EVA
                /*public void EraseName(string name)
                {
                    LocalScope curr = this;

                    while (curr != null)
                    {
                        //if (curr.symbols.Contains(name))
                        curr.symbols.Remove(name);
                        curr = curr.outerScope;
                    }
                }*/

                public void Add(string ident, string realName, NameKind kind, string declaredModuleName = null)
                {
                    symbolsInfos[ident] = new SymbolInfo(realName, kind, declaredModuleName);
                }

                public void Add(string ident, NameKind kind)
                {
                    Add(ident, ident, kind);
                }

                private SymbolInfo FindNameRecursive(string name)
                {
                    LocalScope curr = this;

                    while (curr != null)
                    {
                        if (curr.symbolsInfos.ContainsKey(name))
                            return curr.symbolsInfos[name];
                        curr = curr.OuterScope;
                    }

                    return null;
                }

                public string GetRealName(string ident) => FindNameRecursive(ident).realName;

                public NameKind GetNameKind(string ident) => FindNameRecursive(ident).kind;

                public string GetDeclaredModuleName(string ident) => FindNameRecursive(ident).declaredModuleName;

                public void ClearScope()
                {
                    if (OuterScope != null)
                    {
                        symbolsInfos = OuterScope.symbolsInfos;
                        OuterScope = OuterScope.OuterScope;
                    }
                }

                public bool IsOutermostScope()
                {
                    return OuterScope.OuterScope == null;
                }
            }
        }
    }
}
