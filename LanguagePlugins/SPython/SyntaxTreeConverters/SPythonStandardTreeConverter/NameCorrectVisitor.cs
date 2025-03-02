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
                foreach (ident id in _var_statement.var_def.vars.idents)
                {
                    type_definition td = _var_statement.var_def.vars_type;
                    SourceContext sc = _var_statement.source_context;
                    var vds = new var_def_statement(new ident_list(id.name, id.source_context), td, null, definition_attribute.None, false, sc);
                    decls.Add(new variable_definitions(vds, sc), sc);
                }
            }

            base.visit(_var_statement);
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
                    if (symbolTable.IsOutermostScope())
                    {
                        symbolTable.Add(_ident.name, NameKind.GlobalVariable);

                        _assign.first_assignment_defines_type = true;
                        type_definition td = new named_type_reference(new ident("integer"));
                        SourceContext sc = _assign.source_context;
                        var vds = new var_def_statement(new ident_list(_ident.name, _ident.source_context), td, null, definition_attribute.None, false, sc);
                        decls.Add(new variable_definitions(vds, sc), sc);

                        base.visit(_assign);
                        return;
                    }
                    else
                    {
                        symbolTable.Add(_ident.name, NameKind.LocalVariable);

                        var _var_statement = SyntaxTreeBuilder.BuildVarStatementNodeFromAssignNode(_assign);

                        ReplaceStatement(_assign, _var_statement);

                        ProcessNode(_var_statement);
                        return;
                    }
                }
            }
            base.visit(_assign);
        }
    }
}
