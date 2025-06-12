// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;
using System;
using System.Collections.Generic;

namespace SyntaxVisitors.SugarVisitors
{
    public class UnpackLambdaParametersVisitor : BaseChangeVisitor, IPipelineVisitor
    {
        public static UnpackLambdaParametersVisitor New
        {
            get { return new UnpackLambdaParametersVisitor(); }
        }

        public void Visit(syntax_tree_node root, VisitorsContext context, Action next)
        {
            ProcessNode(root);

            next();
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

        public void CreateUnpackedListOfAssignments(unpacked_list_of_ident_or_list ll, List<statement> res, ident prevname, SourceContext sc)
        {
            var idlist = new ident_list();
            foreach (var llelem in ll.lst)
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

            fld.formal_parameters = new formal_parameters();
            fld.formal_parameters.source_context = fld.source_context;

            // Пусть параметр имеет вид \(x,y)
            // Тогда в начало тела лямбды надо добавить var (x,y) = #fpl1; 
            //var idl = new ident_list(fld.unpacked_params.Select(p => p.id).ToList(),fld.source_context); // 

            //var ass = new assign_var_tuple(idl, main_param_id, fld.source_context);

            List<statement> res = new List<statement>();

            foreach (var param in fld.unpacked_params)
            {
                if (param is unpacked_list_of_ident_or_list unpacked_param)
                {
                    var lam_inft = new lambda_inferred_type(new lambda_any_type_node_syntax());
                    var param_id = CreateIdent(fld.source_context);
                    var idlist = new ident_list(param_id, unpacked_param.source_context);
                    var typed_pars = new typed_parameters(idlist, lam_inft, parametr_kind.none, null, unpacked_param.source_context);
                    fld.formal_parameters.Add(typed_pars, unpacked_param.source_context);
                    fld.ident_list.Add(param_id);
                    fld.parameters.Add(param_id);

                    CreateUnpackedListOfAssignments(unpacked_param, res, param_id, unpacked_param.source_context);
                }
                else if (param is ident id)
                {
                    var lam_inft = new lambda_inferred_type(new lambda_any_type_node_syntax());
                    var idlist = new ident_list(id,id.source_context);
                    var typed_pars = new typed_parameters(idlist, lam_inft);
                    fld.formal_parameters.Add(typed_pars, fld.source_context);
                    fld.ident_list.Add(id);
                    fld.parameters.Add(id);
                }
            }


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
