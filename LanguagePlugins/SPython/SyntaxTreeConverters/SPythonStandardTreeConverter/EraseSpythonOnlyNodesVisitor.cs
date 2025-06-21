using PascalABCCompiler.SyntaxTree;
using System;

namespace Languages.SPython.Frontend.Converters
{
    public class EraseSpythonOnlyNodesVisitor : BaseChangeVisitor
    {
        public EraseSpythonOnlyNodesVisitor() {}

        public override void visit(global_statement _global_statement)
        {
            DeleteInStatementList(_global_statement);
        }

        public override void visit(import_statement _import_statement)
        {
            var upper = UpperNode();
            if (upper is interface_node _interface_node)
                _interface_node.interface_definitions.ReplaceDescendant<syntax_tree_node, syntax_tree_node>(
                    _import_statement, new empty_statement());
            else Replace(_import_statement, new empty_statement());
        }

        public override void visit(from_import_statement _from_import_statement)
        {
            var upper = UpperNode();
            if (upper is interface_node _interface_node)
                _interface_node.interface_definitions.ReplaceDescendant<syntax_tree_node, syntax_tree_node>(
                    _from_import_statement, new empty_statement());
            else Replace(_from_import_statement, new empty_statement());
        }

        public override void visit(tuple_node tup)
        {
            var dn = new ident("CreateTuple", tup.source_context);
            var mc = new method_call(dn, tup.el, tup.source_context);

            //var sug = new sugared_expression(tup, mc, tup.source_context); - нет никакой семантической проверки - всё - на уровне синтаксиса!

            //ReplaceUsingParent(tup, mc); - исправление #1199. Оказывается, ReplaceUsingParent и Replace не эквивалентны - у копии Parent на старого родителя
            Replace(tup, mc);
            visit(mc);
        }
    }
}