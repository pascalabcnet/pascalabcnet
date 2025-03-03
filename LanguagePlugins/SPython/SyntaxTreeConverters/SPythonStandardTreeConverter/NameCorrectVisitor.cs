using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Xml.Serialization;
using AssignTupleDesugarAlgorithm;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors;

namespace Languages.SPython.Frontend.Converters
{
    internal class NameCorrectVisitor : SymbolTableFillingVisitor
    {
        private declarations decls;

        public HashSet<string> variablesUsedAsGlobal = new HashSet<string>();

        public NameCorrectVisitor(Dictionary<string, HashSet<string>> par) : base(par) { }

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
                        _global_statement.source_context, _ident.name);
                    default:
                        throw new SPythonSyntaxVisitorError("SCOPE_CONTAINS_NAME_{0}",
                        _global_statement.source_context, _ident.name);
                }
            }

            base.visit(_global_statement);
        }

        public override void visit(dot_node _dot_node)
        {
            if (_dot_node.left is ident left)
            {
                NameKind nameKind = symbolTable[left.name];
                switch (nameKind)
                {
                    case NameKind.ModuleAlias:
                        left.name = symbolTable.AliasToRealName(left.name);
                        break;

                    case NameKind.ImportedNameAlias:
                        _dot_node.left = new dot_node(new ident(symbolTable.AliasToModuleName(left.name)), left);
                        left.name = symbolTable.AliasToRealName(left.name);
                        break;

                    case NameKind.GlobalVariable:
                        if (symbolTable.IsInFunctionBody &&
                        !variablesUsedAsGlobal.Contains(left.name))
                            variablesUsedAsGlobal.Add(left.name);
                        break;

                    case NameKind.Unknown:
                        throw new SPythonSyntaxVisitorError("UNKNOWN_NAME_{0}",
                        left.source_context, left.name);
                }
            }
        }

        public override void visit(named_type_reference _named_type_reference)
        {
            ident id = _named_type_reference.names[0];
            NameKind nameKind = symbolTable[id.name];
            switch (nameKind)
            {
                case NameKind.ModuleAlias:
                    id.name = symbolTable.AliasToRealName(id.name);
                    break;

                case NameKind.ImportedNameAlias:
                    _named_type_reference.names.Insert(0, new ident(symbolTable.AliasToModuleName(id.name), id.source_context));
                    _named_type_reference.names[1].name = symbolTable.AliasToRealName(_named_type_reference.names[1].name);
                    break;

                // Сомнительно
                case NameKind.GlobalVariable:
                    if (symbolTable.IsInFunctionBody &&
                    !variablesUsedAsGlobal.Contains(id.name))
                        variablesUsedAsGlobal.Add(id.name);
                    break;

                case NameKind.Unknown:
                    throw new SPythonSyntaxVisitorError("UNKNOWN_NAME_{0}",
                    id.source_context, id.name);
            }
        }

        public override void visit(ident _ident)
        {
            NameKind nameKind = symbolTable[_ident.name];
            switch (nameKind)
            {
                case NameKind.ImportedNameAlias:
                    Replace(_ident, new dot_node(new ident(symbolTable.AliasToModuleName(_ident.name))
                    , new ident(symbolTable.AliasToRealName(_ident.name))));
                    break;

                case NameKind.GlobalVariable:
                    if (symbolTable.IsInFunctionBody &&
                        !variablesUsedAsGlobal.Contains(_ident.name))
                        variablesUsedAsGlobal.Add(_ident.name);
                    break;

                case NameKind.Unknown:
                    throw new SPythonSyntaxVisitorError("UNKNOWN_NAME_{0}"
                    , _ident.source_context, _ident.name);
            }
        }

        public override void visit(assign _assign)
        {
            if (_assign.to is ident _ident)
            {
                if (!symbolTable.IsVisibleForAssignment(_ident.name))
                {
                    if (symbolTable.IsOutermostScope())
                        symbolTable.Add(_ident.name, NameKind.GlobalVariable);
                    else
                        symbolTable.Add(_ident.name, NameKind.LocalVariable);

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
