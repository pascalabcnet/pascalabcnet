using System;
using System.Collections.Generic;
using PascalABCCompiler.CoreUtils;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors.SugarVisitors;

internal static class Program
{
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
        Console.WriteLine("PASS " + message);
    }

    private static void Main()
    {
        var oldValue = new int32_const(1);
        var replacement = new int32_const(2);
        var pair = new to_expr(oldValue, oldValue);
        pair[1] = replacement;
        Check(ReferenceEquals(pair.value, replacement), "to_expr indexer replaces value");
        Check(ReferenceEquals(replacement.Parent, pair), "to_expr replacement has correct parent");
        var item = new enumerator(null, oldValue);
        item[1] = replacement;
        Check(ReferenceEquals(item.value, replacement), "enumerator indexer replaces value");
        Check(ReferenceEquals(replacement.Parent, item), "enumerator replacement has correct parent");

        var tuple = new tuple_node(new expression_list(new List<expression> { new int32_const(2), new int32_const(3) }));
        pair = new to_expr(new int32_const(1), tuple);
        var root = new expression_list(pair);
        TupleVisitor.Create(true, new GeneratedNamesManager()).ProcessNode(root);
        Check(pair.value is method_call, "tuple replacement reaches to_expr.value");
        ToExprVisitor.New.ProcessNode(root);
        var call = root.expressions[0] as method_call;
        Check(call != null && call.parameters.expressions.Count == 2, "to becomes a two-argument call");
        Check(call.parameters.expressions[1] is method_call, "second argument remains Tuple.Create");
        var nested = new to_expr(new int32_const(1), new to_expr(new int32_const(2), new int32_const(3)));
        root = new expression_list(nested);
        ToExprVisitor.New.ProcessNode(root);
        call = root.expressions[0] as method_call;
        Check(call != null && call.parameters.expressions[1] is method_call,
            "nested to is rewritten before its parent is replaced");
    }
}
