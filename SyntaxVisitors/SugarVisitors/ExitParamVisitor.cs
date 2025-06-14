// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.SugarVisitors
{
    public class ExitParamVisitor : BaseChangeVisitor
    {
        public static ExitParamVisitor New
        {
            get { return new ExitParamVisitor(); }
        }

        public override void visit(procedure_call pc)
        {
            var fname = pc.func_name as method_call;
            var id = fname?.dereferencing_value as ident;
            if (id != null && id.name.ToLower() == "exit")
            {
                if (fname.parameters == null || fname.parameters.Count == 0)
                {
                    return;
                }
                if (fname.parameters.Count > 1)
                {
                    throw new SyntaxVisitorError("EXIT_PROC_CAN_HAVE_ONLY_ONE_OR_ZERO_PARAMS", pc.source_context);
                }
                // Поднимаемся по Parent до procedure_definition или nil
                var parent = pc.Parent;
                while (parent != null && !(parent is procedure_definition)
                    && !(parent is function_lambda_definition))
                    parent = parent.Parent;
                if (parent == null)
                {
                    throw new SyntaxVisitorError("EXIT_WITH_PARAM_MUST_BE_ONLY_IN_FUNC", pc.source_context);
                }
                if (parent is procedure_definition pd && !(pd.proc_header is function_header))
                {
                    throw new SyntaxVisitorError("EXIT_WITH_PARAM_MUST_BE_ONLY_IN_FUNC", pc.source_context);
                }
                if (parent is function_lambda_definition fld /*&& fld.return_type == null*/)
                {
                    fld.usedkeyword = 1;
                    fld.return_type = new lambda_inferred_type();
                    //throw new SyntaxVisitorError("EXIT_WITH_PARAM_CANNOT_BE_IN_LAMBDA_PROCEDURE", pc.source_context);
                }

                var arg = fname.parameters[0] as expression;
                // Теперь сформировать 2 узла как statement_list: Result := arg; exit

                var sl = new List<statement>();
                var exitproc = new procedure_call(new ident("exit", pc.source_context));
                var ass_Res = new assign(new ident("Result", pc.source_context), arg, 
                    Operators.Assignment, pc.source_context);
                sl.Add(ass_Res);
                sl.Add(exitproc);

                ReplaceStatementUsingParent(pc, sl);

            }

            //mc = new method_call(
            //            new dot_node(indexCreation, new ident("Reverse", ind.source_context)),
            //            reverseIndexMethodParams,
            //            ind.source_context);

            //ReplaceUsingParent(ind, mc);
            //visit(mc);
        }

    }

}
