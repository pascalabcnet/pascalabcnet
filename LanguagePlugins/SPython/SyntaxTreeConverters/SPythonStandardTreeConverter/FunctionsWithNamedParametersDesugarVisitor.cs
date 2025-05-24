using System.Collections.Generic;
using System.Data;
using System.Linq;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors;

namespace Languages.SPython.Frontend.Converters
{
    internal class FunctionsWithNamedParametersDesugarVisitor : BaseChangeVisitor
    {
        public FunctionsWithNamedParametersDesugarVisitor() { }

        public override void visit(method_call _method_call)
        {
            if (_method_call.parameters is expression_list exprl &&
                _method_call.dereferencing_value is ident caller) {
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
                    else throw new SPythonSyntaxVisitorError("ARG_AFTER_KVARGS", expr.source_context);
                }

                if (kvargs.expressions.Count() != 0)
                {
                    ident method_name = caller;
                    ident class_name = new ident(method_name.name);
                    dot_node ctor_call_name = new dot_node(class_name, new ident("create"));
                    method_call ctor_call = new method_call(ctor_call_name, null, _method_call.source_context);
                    dot_node dn = new dot_node(ctor_call, new ident("!" + method_name), _method_call.source_context);
                    method_call new_method_call = new method_call(dn, args, _method_call.source_context);
                    Replace(_method_call, new_method_call);
                }
            }
        }
    }
}
