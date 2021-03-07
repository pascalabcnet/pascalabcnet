// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeRealization;

namespace SyntaxVisitors.SugarVisitors
{
    public class UnpackLambdaParametersVisitor : BaseChangeVisitor
    {
        public static UnpackLambdaParametersVisitor New
        {
            get { return new UnpackLambdaParametersVisitor(); }
        }

        private static int num = 0;

        public static string UniqueNumStr()
        {
            num++;
            return num.ToString();
        }

        public ident CreateIdent(SourceContext sc)
        {
            return new ident("#fpl" + UniqueNumStr(), sc);
        }

        public void CreateUnpackedListOfAssignments(List<ident_or_list> ll, List<statement> res, ident prevname, SourceContext sc)
        {
            var idlist = new ident_list();
            foreach (var llelem in ll)
            {
                if (llelem.lst != null)
                {
                    var id = CreateIdent(sc);
                    // надо у этого id установить souce_context
                    idlist.Add(id);
                    CreateUnpackedListOfAssignments(llelem.lst, res, id, sc);
                }
                else idlist.Add(llelem.id);
            }
            idlist.source_context = sc;
            var ass = new assign_var_tuple(idlist, prevname, sc);
            res.Insert(0, ass);
        }
        public override void visit(function_lambda_definition fld)
        {
            ProcessNode(fld.proc_body);
            if (fld.unpacked_params == null)
            {
                return;
            } 
                
            var main_param_id = CreateIdent(fld.source_context);
            var idlist = new ident_list(main_param_id);
            var lam_inft = new lambda_inferred_type(new lambda_any_type_node());
            var typp = new typed_parameters(idlist, lam_inft);
            fld.formal_parameters = new formal_parameters(typp, fld.source_context);

            // Пусть параметр имеет вид \(x,y)
            // Тогда в начало тела лямбды надо добавить var (x,y) = #fpl1; 
            //var idl = new ident_list(fld.unpacked_params.Select(p => p.id).ToList(),fld.source_context); // 

            //var ass = new assign_var_tuple(idl, main_param_id, fld.source_context);
            List<statement> res = new List<statement>();
            CreateUnpackedListOfAssignments(fld.unpacked_params, res, main_param_id, fld.source_context);

            var stl = fld.proc_body as statement_list;
            res.Reverse();
            foreach (var st in res)
            {
                var ass = st as assign_var_tuple;
                ass.idents.source_context = new SourceContext(ass.idents[0].source_context, ass.idents[ass.idents.Count - 1].source_context);
                ass.expr.source_context = ass.idents.source_context;
                stl.Insert(0, st); // возможно, в обратном порядке
            }
                
            //stl.Insert(0, ass);
            fld.unpacked_params = null; // на рассахаренном уровне это поле нулевое!
        }
    }
}
