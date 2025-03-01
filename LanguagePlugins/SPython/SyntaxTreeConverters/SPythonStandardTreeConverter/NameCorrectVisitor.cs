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
    internal class NameCorrectVisitor : SymbolTableFillingVisitor
    {
        public NameCorrectVisitor(Dictionary<string, HashSet<string>> par) : base(par) { }

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
                        symbolTable.MakeVisibleForAssignment(_ident.name);
                        break;
                    case NameKind.Unknown:
                        throw new SPythonSyntaxVisitorError("UNKNOWN_NAME_{0}",
                        _global_statement.source_context, _ident.name);
                    default:
                        throw new SPythonSyntaxVisitorError("SCOPE_CONTAINS_NAME_{0}",
                        _global_statement.source_context, _ident.name);
                }
            }
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

        public override void visit(assign _assign)
        {
            if (_assign.to is ident _ident)
            {
                if (!symbolTable.IsVisibleForAssignment(_ident.name))
                {
                    symbolTable.Add(_ident.name, NameKind.LocalVariable);

                    var _var_statement = SyntaxTreeBuilder.BuildVarStatementNodeFromAssignNode(_assign);

                    ReplaceStatement(_assign, _var_statement);

                    base.visit(_var_statement);
                    return;
                }
                else if (symbolTable[_ident.name] == NameKind.ImportedNameAlias)
                {
                    // Присвоение к переменной из модуля воспринимается парсером как объявление
                    //_assign.first_assignment_defines_type = false;
                }
            }
            base.visit(_assign);
        }
    }
}
