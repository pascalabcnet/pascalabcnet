using PascalABCCompiler.SyntaxTree;
using System.Collections.Generic;

namespace Languages.SPython.Frontend.Converters
{
    internal class NameCorrectVisitor : SymbolTableFillingVisitor
    {
        public HashSet<string> variablesUsedAsGlobal = new HashSet<string>();

        public NameCorrectVisitor(Dictionary<string, HashSet<string>> namesFromUsedUnits, HashSet<string> definedFunctionsNames) : base(namesFromUsedUnits) 
        {
            foreach (string definedFunctionName in definedFunctionsNames)
            {
                symbolTable.Add(definedFunctionName, NameKind.ForwardDeclaredFunction);
            }
        }

        public override void Enter(syntax_tree_node stn)
        {
            if (stn is ident && stn.Parent is dot_node dn && dn.right == stn)
            {
                visitNode = false;
            }

            base.Enter(stn);
        }

        public override void visit(var_statement _var_statement)
        {
            if (symbolTable.IsOutermostScope())
            {
                // не работает для нескольких переменных (var a1, a2, ...: type;)
                ident id = _var_statement.var_def.vars.idents[0];
                symbolTable.Add(id.name, NameKind.GlobalVariable);
                ProcessNode(_var_statement.var_def);
            }
            else base.visit(_var_statement);
        }

        public override void visit(name_assign_expr _name_assign_expr)
        {
            ProcessNode(_name_assign_expr[1]);
        }

        public override void visit(unit_or_namespace _unit_or_namespace)
        {

        }

        public override void visit(unit_name _unit_name)
        {

        }

        public override void visit(global_statement _global_statement)
        {
            foreach (ident _ident in _global_statement.idents.idents)
            {
                NameKind nameType = symbolTable[_ident.name];
                switch (nameType)
                {
                    case NameKind.GlobalVariable:
                    case NameKind.ImportedNameAlias:
                        break;
                    case NameKind.Unknown:
                        throw new SPythonSyntaxVisitorError("UNKNOWN_NAME_{0}",
                        _ident.source_context, _ident.name);
                    default:
                        throw new SPythonSyntaxVisitorError("SCOPE_CONTAINS_NAME_{0}",
                        _ident.source_context, _ident.name);
                }
            }

            base.visit(_global_statement);
        }

        public override void visit(named_type_reference _named_type_reference)
        {
            ident id = _named_type_reference.names[0];
            string name = id.name;

            if (name == "int" || name == "str" || name == "bool" || name == "float")
            {
                return;
            }

            NameKind nameKind = symbolTable[name];
            switch (nameKind)
            {
                case NameKind.ModuleAlias:
                    id.name = symbolTable.AliasToRealName(id.name);
                    break;

                case NameKind.ImportedNameAlias:
                    _named_type_reference.names.Insert(0, new ident(symbolTable.AliasToModuleName(id.name), id.source_context));
                    _named_type_reference.names[1].name = symbolTable.AliasToRealName(_named_type_reference.names[1].name);
                    break;

                case NameKind.Unknown:
                    throw new SPythonSyntaxVisitorError("UNKNOWN_NAME_{0}",
                    id.source_context, id.name);
            }
        }

        public override void visit(template_type_reference _template_type_reference)
        {
            named_type_reference _named_type_reference = _template_type_reference.name;
            string name = _named_type_reference.names[0].name;

            string fullName = name + $"`{_template_type_reference.params_list.Count}";

            if (name == "list" || name == "set" || name == "dict")
                return;

            NameKind nameKind = symbolTable[fullName];

            switch (nameKind)
            {

                case NameKind.ImportedNameAlias:
                    _named_type_reference.names.Insert(0, new ident(symbolTable.AliasToModuleName(fullName), _named_type_reference.names[0].source_context));
                    _named_type_reference.names[1].name = symbolTable.AliasToRealName(fullName);
                    break;

                case NameKind.Unknown:
                    throw new SPythonSyntaxVisitorError("UNKNOWN_NAME_{0}",
                    _named_type_reference.names[0].source_context, name);
            }
        }

        public override void visit(ident _ident)
        {
            SourceContext sc = _ident.source_context;
            NameKind nameKind = symbolTable[_ident.name];
            switch (nameKind)
            {
                case NameKind.ModuleAlias:
                    _ident.name = symbolTable.AliasToRealName(_ident.name);
                    break;

                case NameKind.ImportedNameAlias:
                    Replace(_ident, new dot_node(new ident(symbolTable.AliasToModuleName(_ident.name), sc)
                    , new ident(symbolTable.AliasToRealName(_ident.name), sc), sc));
                    break;

                case NameKind.GlobalVariable:
                    if (symbolTable.IsInFunctionBody &&
                        !variablesUsedAsGlobal.Contains(_ident.name))
                        variablesUsedAsGlobal.Add(_ident.name);
                    break;

                case NameKind.ForwardDeclaredFunction:
                    if (!symbolTable.IsInFunctionBody)
                        throw new SPythonSyntaxVisitorError("FUNCTION_{0}_USED_BEFORE_DECLARATION",
                                                            sc, _ident.name);
                    break;

                case NameKind.Unknown:
                    throw new SPythonSyntaxVisitorError("UNKNOWN_NAME_{0}", 
                                                        sc, _ident.name);
            }
        }

        // кидает ошибки если инициализировать пустой коллекцией
        // не указывая типа переменной
        private void CheckInitializationWithEmptyCollection(assign _assign)
        {
            // a = []
            // a = !empty_list()
            // a = {}
            // a = !empty_dict()
            // a = set()
            if (_assign.from is method_call mc &&
                mc.dereferencing_value is ident id &&
                mc.parameters == null)
            {
                if (id.name == "!empty_list")
                {
                    throw new SPythonSyntaxVisitorError("IMPOSSIBLE_TO_INFER_LIST_TYPE",
                        _assign.from.source_context);
                }
                else if (id.name == "!empty_dict")
                {
                    throw new SPythonSyntaxVisitorError("IMPOSSIBLE_TO_INFER_DICT_TYPE",
                        _assign.from.source_context);
                }
                if (id.name == "set")
                {
                    throw new SPythonSyntaxVisitorError("IMPOSSIBLE_TO_INFER_SET_TYPE",
                        _assign.from.source_context);
                }
            }
        }

        //public override void visit(assign_tuple _assign_tuple)
        //{
        //
        //}

        public override void visit(assign _assign)
        {
            if (_assign.operator_type == Operators.Assignment && _assign.to is ident _ident)
            {
                if (!symbolTable.IsVisibleToAssign(_ident.name))
                {
                    // инициализация новой переменной с присвоением
                    if (symbolTable.IsOutermostScope())
                        symbolTable.Add(_ident.name, NameKind.GlobalVariable);
                    else
                        symbolTable.Add(_ident.name, NameKind.LocalVariable);

                    CheckInitializationWithEmptyCollection(_assign);

                    var _var_statement = SyntaxTreeBuilder.BuildVarStatementNodeFromAssignNode(_assign);

                    ReplaceStatement(_assign, _var_statement);

                    ProcessNode(_var_statement);
                    return;
                }
                else if (symbolTable.IsInFunctionBody 
                    && symbolTable[_ident.name] == NameKind.GlobalVariable 
                    && !variablesUsedAsGlobal.Contains(_ident.name))
                {
                    variablesUsedAsGlobal.Add(_ident.name);
                }
            }
            base.visit(_assign);
        }
    }
}
