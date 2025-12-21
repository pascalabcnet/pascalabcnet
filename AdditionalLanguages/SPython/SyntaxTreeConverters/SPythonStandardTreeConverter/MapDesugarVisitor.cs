using PascalABCCompiler.SyntaxTree;

namespace Languages.SPython.Frontend.Converters
{
    internal class MapDesugarVisitor : BaseChangeVisitor
    {
        public MapDesugarVisitor() { }

        public override void visit(method_call mc)
        {
            base.visit(mc);

            if (mc.dereferencing_value is ident map && map.name == "map" &&
                mc.parameters.Count == 2 && mc.parameters[0] is addressed_value av &&
                mc.parameters[1] is expression range)
            {
                ident ident = new ident("!map_iterable_element", mc.source_context);
                expression expr = new method_call(av, new expression_list(ident), mc.source_context);

                generator_object go = new generator_object(expr, ident, range, null, mc.source_context);

                Replace(mc, go);
            }
        }
    }
}
