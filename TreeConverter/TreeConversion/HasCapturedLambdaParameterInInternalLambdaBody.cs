using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.TreeConverter
{
    public class HasCapturedLambdaParameterInInternalLambdaBody : BaseEnterExitVisitor
    {
        public bool HasCapturedParameter = false;
        public List<ident> idents = new List<ident>();
        public bool InLambda = false;
        public HasCapturedLambdaParameterInInternalLambdaBody(function_lambda_definition fld)
        {
            idents = fld.ident_list.idents;
        }
        public static HasCapturedLambdaParameterInInternalLambdaBody New(function_lambda_definition fld)
        {
            return new HasCapturedLambdaParameterInInternalLambdaBody(fld);
        }
        public override void visit(var_def_statement vds)
        {
            //ProcessNode(vds.vars_type);
        }
        public override void visit(ident id)
        {
            if (!InLambda) // мы ищем только в лямбдах
                return;
            if (HasCapturedParameter)
                return;
            if (idents.Select(i => i.name).Contains(id.name,StringComparer.OrdinalIgnoreCase))
            {
                HasCapturedParameter = true;
            }
        }
        public override void visit(function_lambda_definition fld)
        {
            InLambda = true;
            // Исключить все параметры из idents
            foreach (var fid in fld.ident_list.idents)
            {
                var ind = idents.FindIndex(x => string.Compare(x.name, fid.name, true) == 0);
                if (ind != -1)
                    idents.RemoveAt(ind);
            }

            ProcessNode(fld.proc_body);
        }



    }
}
