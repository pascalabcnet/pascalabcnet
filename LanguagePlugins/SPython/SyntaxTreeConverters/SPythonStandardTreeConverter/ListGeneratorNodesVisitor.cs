using System.Collections.Generic;
using System.Data;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors;

namespace Languages.SPython.Frontend.Converters
{
    internal class ListGeneratorNodesVisitor : BaseChangeVisitor
    {
        ParserLambdaHelper lambdaHelper = new ParserLambdaHelper();
        public ListGeneratorNodesVisitor() {  }

        public override void visit(list_generator _list_generator)
        {
            dot_node dn;
            ident_list idList;
            formal_parameters formalPars;
            statement_list sl;
            function_lambda_definition lambda;
            method_call mc;

            // [ expr1 for ident in expr2 if expr3 ] -> expr2.Where(ident -> expr3).Select(ident -> expr1).ToList()
            if (_list_generator._condition != null)
            {
                string ident_name = _list_generator._ident.name;
                idList = new ident_list(new ident(ident_name), _list_generator._ident.source_context);
                formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new lambda_any_type_node_syntax(), _list_generator._ident.source_context), parametr_kind.none, null, _list_generator._ident.source_context), _list_generator._ident.source_context);

                dn = new dot_node(_list_generator._range as addressed_value, (new ident("Where")) as addressed_value, _list_generator.source_context);

                sl = new statement_list(new assign("result", _list_generator._condition, _list_generator._condition.source_context), _list_generator._condition.source_context); //!
                sl.expr_lambda_body = true;
                lambda = new function_lambda_definition(
                lambdaHelper.CreateLambdaName(), formalPars,
                new lambda_inferred_type(new lambda_any_type_node_syntax(), _list_generator._ident.source_context), sl, _list_generator.source_context);

                mc = new method_call(dn as addressed_value, new expression_list(lambda as expression), _list_generator.source_context);
                dn = new dot_node(mc as addressed_value, (new ident("Select")) as addressed_value, _list_generator.source_context);
            }
            // [ expr1 for ident in expr2 ] -> expr2.Select(ident -> expr1).ToList()
            else
                dn = new dot_node(_list_generator._range as addressed_value, (new ident("Select")) as addressed_value, _list_generator.source_context);


            idList = new ident_list(_list_generator._ident, _list_generator._ident.source_context);
            formalPars = new formal_parameters(new typed_parameters(idList, new lambda_inferred_type(new lambda_any_type_node_syntax(), _list_generator._ident.source_context), parametr_kind.none, null, _list_generator._ident.source_context), _list_generator._ident.source_context);

            sl = new statement_list(new assign("result", _list_generator._expr, _list_generator._expr.source_context), _list_generator._expr.source_context);
            sl.expr_lambda_body = true;

            lambda = new function_lambda_definition(
                lambdaHelper.CreateLambdaName(), formalPars,
                new lambda_inferred_type(new lambda_any_type_node_syntax(), _list_generator._ident.source_context), sl, _list_generator.source_context);


            mc = new method_call(dn as addressed_value, new expression_list(lambda as expression), _list_generator.source_context);
            dn = new dot_node(mc as addressed_value, (new ident("ToList")) as addressed_value, _list_generator.source_context);

            Replace(_list_generator, new method_call(dn as addressed_value, null, _list_generator.source_context));
        }
    }

    public class ParserLambdaHelper
    {
        private int lambda_num = 0;
        public List<function_lambda_definition> lambdaDefinitions;
        public static string lambdaPrefix = "<>lambda";

        public ParserLambdaHelper()
        {
            lambdaDefinitions = new List<function_lambda_definition>();
        }

        public string CreateLambdaName()
        {
            lambda_num++;
            return lambdaPrefix + lambda_num.ToString();
        }

        public bool IsLambdaName(ident id)
        {
            return id.name.StartsWith(lambdaPrefix);
        }
    }
}
