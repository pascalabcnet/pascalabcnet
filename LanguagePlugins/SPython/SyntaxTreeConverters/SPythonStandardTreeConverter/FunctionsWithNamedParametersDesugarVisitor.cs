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
            if (_method_call.parameters is expression_list exprl) {
                expression_list args = new expression_list();
                expression_list kvargs_names_array = new expression_list();
                expression_list kvargs = new expression_list();

                foreach (var expr in exprl.expressions)
                {
                    if (expr is name_assign_expr nae)
                    {
                        kvargs_names_array.Add(new string_const(nae.name.name));
                        kvargs_names_array.source_context
                            = new SourceContext(kvargs_names_array.source_context, expr.source_context);
                        kvargs.Add(nae.expr);
                        kvargs.source_context 
                            = new SourceContext(kvargs.source_context, expr.source_context);
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
                    array_const_new acn = new array_const_new(kvargs_names_array, '|');
                    kvargs.expressions.Insert(0, acn);

                    ident method_name = new ident();
                    if (_method_call.dereferencing_value is ident id)
                    {
                        method_name = id;
                    }
                    else if (_method_call.dereferencing_value is dot_node dnn)
                    {
                        method_name = dnn.right as ident;
                    }

                    ident class_name = new ident("!" + method_name);
                    dot_node ctor_call_name = new dot_node(class_name, new ident("create"));
                    method_call ctor_call = new method_call(ctor_call_name, kvargs, _method_call.source_context);
                    dot_node dn = new dot_node(ctor_call, new ident(method_name.name), _method_call.source_context);
                    method_call new_method_call = new method_call(dn, args, _method_call.source_context);
                    Replace(_method_call, new_method_call);
                    return;
                }
            }
            base.visit(_method_call);
        }
    }
}
