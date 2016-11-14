using System;
using System.Collections.Generic;
using System.Linq;
using PascalABCCompiler.SyntaxTree;

public class StandOutExprWithLambdaInForeachSequenceVisitor : BaseEnterExitVisitor
{
    public static StandOutExprWithLambdaInForeachSequenceVisitor New
    {
        get
        {
            return new StandOutExprWithLambdaInForeachSequenceVisitor();
        }
    }

    private int GenIdNum = 0;
    public ident GenIdentName()
    {
        GenIdNum++;
        return new ident("$GenContFE" + GenIdNum.ToString());
    }

    public void ReplaceStatement(statement from, IEnumerable<statement> to)
    {
        foreach (var x in to)
            x.Parent = from.Parent;
        var sl = from.Parent as statement_list;
        if (sl != null)
        {
            sl.ReplaceInList(from, to);
        }
        else
        {
            var l = new statement_list();
            l.AddMany(to);
            l.source_context = from.source_context;
            from.Parent.Replace(from, l);
        }
    }


    public override void visit(foreach_stmt fe)
    {
        if (fe.in_what.DescendantNodes().OfType<function_lambda_definition>().Count()>0)
        {
            var id = GenIdentName();
            id.Parent = fe;
            var ass = new var_statement(id, fe.in_what, fe.in_what.source_context);
            fe.in_what = id;
            var l = new List<statement>();
            l.Add(ass);
            l.Add(fe);
            ReplaceStatement(fe, l);
        }

        base.visit(fe);
    }

}
