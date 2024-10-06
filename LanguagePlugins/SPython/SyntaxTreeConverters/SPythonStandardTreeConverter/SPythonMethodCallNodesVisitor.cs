using System.Collections.Generic;
using System.Data;
using System.Linq;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors;

namespace Languages.SPython.Frontend.Converters
{
    internal class SPythonMethodCallNodesVisitor : BaseChangeVisitor
    {
        public SPythonMethodCallNodesVisitor() { }

        public override void visit(method_call _method_call)
        {
            if (_method_call.parameters is expression_list exprl) {
                expression_list args = new expression_list();
                expression_list kvargs = new expression_list();

                foreach (var expr in exprl.expressions)
                {
                    if (expr is name_assign_expr)
                    {
                        kvargs.Add(expr);
                        kvargs.source_context = new SourceContext(kvargs.source_context, expr.source_context);
                    }
                    else if (kvargs.expressions.Count() == 0)
                    {
                        args.Add(expr);
                        args.source_context = new SourceContext(args.source_context, expr.source_context);
                    }
                    else throw new SyntaxVisitorError("Arg after Kvarg", _method_call.source_context);
                }

                if (kvargs.expressions.Count() == 0)
                    Replace(_method_call, new method_call(_method_call.dereferencing_value, args, _method_call.source_context));
                else
                {
                    method_call mc = new method_call(new ident("!" + (_method_call.dereferencing_value as ident).name + ".Get"), kvargs, _method_call.source_context);
                    dot_node dn = new dot_node(mc as addressed_value, _method_call.dereferencing_value as addressed_value, _method_call.source_context);
                    Replace(_method_call, new method_call(dn as addressed_value, args, _method_call.source_context));
                }
            }

            else
                Replace(_method_call, new method_call(_method_call.dereferencing_value as addressed_value, null, _method_call.source_context));
        }
    }
}
