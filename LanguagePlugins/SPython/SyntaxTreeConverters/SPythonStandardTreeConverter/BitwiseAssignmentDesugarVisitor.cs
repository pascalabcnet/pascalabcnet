using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;
using System;

namespace Languages.SPython.Frontend.Converters
{
    internal class BitwiseAssignmentDesugarVisitor : BaseChangeVisitor, IPipelineVisitor
    {
        public BitwiseAssignmentDesugarVisitor() { }

        public void Visit(syntax_tree_node root, VisitorsContext context, Action next)
        {
            ProcessNode(root);

            next();
        }

        public override void visit(assign _assign)
        {
            Operators bin_op_type;

            switch (_assign.operator_type)
            {
                case Operators.AssignmentBitwiseAND: {
                        bin_op_type = Operators.LogicalAND;
                        break; }
                case Operators.AssignmentBitwiseOR: {
                        bin_op_type = Operators.LogicalOR;
                        break; }
                case Operators.AssignmentBitwiseXOR: {
                        bin_op_type = Operators.BitwiseXOR;
                        break; }
                case Operators.AssignmentBitwiseLeftShift: {
                        bin_op_type = Operators.BitwiseLeftShift;
                        break; }
                case Operators.AssignmentBitwiseRightShift: {
                        bin_op_type = Operators.BitwiseRightShift;
                        break; }
                default: {
                        return; }
            }

            SourceContext sc = _assign.source_context;
            bin_expr from = new bin_expr(_assign.to.TypedClone(), _assign.from, bin_op_type, sc);
            assign new_assign = new assign(_assign.to, from, sc);
            Replace(_assign, new_assign);
        }
    }
}
