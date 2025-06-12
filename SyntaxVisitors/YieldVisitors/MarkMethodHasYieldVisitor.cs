// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

using PascalABCCompiler.Errors;
using PascalABCCompiler.SyntaxTreeConverters;

namespace SyntaxVisitors
{
    public class CheckArrayRecordInitializersVisitor : WalkingVisitorNew
    {
        //procedure_definition PD;
        public static CheckArrayRecordInitializersVisitor New
        {
            get { return new CheckArrayRecordInitializersVisitor(); }
        }
        public override void visit(function_lambda_definition ld)
        {
            // пропустить всё
        }
        public override void visit(var_def_statement vd)
        {
            if (vd.inital_value is array_const)
                throw new SyntaxVisitorError("FUNCTION_WITH_YIELD_CANNOT_CONTAIN_OLDSTYLE_ARRAY_INITIALIZERS", vd.inital_value.source_context);
            if (vd.inital_value is record_const)
                throw new SyntaxVisitorError("FUNCTION_WITH_YIELD_CANNOT_CONTAIN_OLDSTYLE_RECORD_INITIALIZERS", vd.inital_value.source_context);
        }

    }

    public class MarkMethodHasYieldAndCheckSomeErrorsVisitor : WalkingVisitorNew, IPipelineVisitor
    {
        private bool HasYields = false;
        private procedure_definition CurrentMethod = null;

        private Stack<procedure_definition> MethodsStack = new Stack<procedure_definition>();

        public static MarkMethodHasYieldAndCheckSomeErrorsVisitor New
        {
            get { return new MarkMethodHasYieldAndCheckSomeErrorsVisitor(); }
        }

        public void Visit(syntax_tree_node root, VisitorsContext context, Action next)
        {
            CapturedNamesHelper.Reset();

            ProcessNode(root);

            next();
        }

        public override void visit(function_lambda_definition ld)
        {
            if (ld.DescendantNodes().OfType<yield_node>().Count() > 0)
            {
                throw new SyntaxVisitorError("LAMBDA_EXPRESSIONS_CANNOT_CONTAIN_YIELD", ld.source_context);
            }

            //base.visit(ld); // SSM 15/07/16 - незачем обходить внутренности лямбды - мы и так ищем внутри них yield - этого в этом визиторе достаточно
        }

        public override void visit(with_statement ws)
        {
            if (ws.DescendantNodes().OfType<yield_node>().Count() > 0)
            {
                throw new SyntaxVisitorError("YIELDS_INSIDE_WITH_ARE_ILLEGAL", ws.source_context);
            }
            if (ws.DescendantNodes().OfType<yield_sequence_node>().Count() > 0)
            {
                throw new SyntaxVisitorError("YIELDSEQUENCES_INSIDE_WITH_ARE_ILLEGAL", ws.source_context);
            }
            base.visit(ws);
        }

        ident IdResult = null;
        public override void visit(procedure_definition pd)
        {
            //this.CurrentMethod = pd;

            MethodsStack.Push(pd);

            HasYields = false;

            base.visit(pd);
            pd.has_yield = HasYields;

            if (pd.has_yield) // SSM bug fix #219
            {
                var ee = pd.proc_body as block;
                if (ee != null)
                {
                    var FirstTypeDeclaration = ee.defs.DescendantNodes().OfType<type_declarations>();
                    if (FirstTypeDeclaration.Count() > 0)
                    {
                        throw new SyntaxVisitorError("FUNCTIONS_WITH_YIELDS_CANNOT_CONTAIN_LOCAL_TYPE_DEFINITIONS", FirstTypeDeclaration.First().source_context);
                    }
                    var FirstInheritedIdent = ee.DescendantNodes().OfType<inherited_ident>(); // SSM bug fix #1440
                    if (FirstInheritedIdent.Count() > 0)
                    {
                        throw new SyntaxVisitorError("FUNCTIONS_WITH_YIELDS_CANNOT_CONTAIN_INHERITED_CALLS", FirstInheritedIdent.First().source_context);
                    }
                }

                CheckArrayRecordInitializersVisitor.New.ProcessNode(pd);
            }

            /*if (pd.has_yield)
            {
                var dn = pd.DescendantNodes().OfType<ident>().Where(id => id.name.ToLower() == "result").FirstOrDefault();
                if (dn != null)
                    throw new SyntaxVisitorError("FUNCTIONS_WITH_YIELDS_CANNOT_CONTAIN_RESULT", dn);
            }*/

            if (pd.has_yield)
            {
                if (IdResult != null)
                    throw new SyntaxVisitorError("FUNCTIONS_WITH_YIELDS_CANNOT_CONTAIN_RESULT", IdResult);
            }

            var innerPds = pd.DescendantNodes().OfType<procedure_definition>();

            if (pd.has_yield && innerPds.Count() > 0
                || innerPds.Where(npd => npd.has_yield).Count() > 0)
            {
                // Есть yield и вложенные - плохо
                // Или нет yield но есть вложенные с yield
                throw new SyntaxVisitorError("FUNCTIONS_WITH_YIELDS_CANNOT_CONTAIN_NESTED_SUBROUTINES", pd.source_context);
            }

            if (pd.has_yield && HasStatementWithBarrierVisitor<try_stmt,function_lambda_definition>.Has(pd))
                //pd.DescendantNodes().OfType<try_stmt>().Count() > 0)
            {
                throw new SyntaxVisitorError("FUNCTIONS_WITH_YIELDS_CANNOT_CONTAIN_TRY_EXCEPT_FINALLY", pd.source_context);
            }

            if (pd.has_yield && HasStatementWithBarrierVisitor<lock_stmt,function_lambda_definition>.Has(pd))
                //pd.DescendantNodes().OfType<lock_stmt>().Count() > 0)
            {
                throw new SyntaxVisitorError("FUNCTIONS_WITH_YIELDS_CANNOT_CONTAIN_LOCK", pd.source_context);
            }

            HasYields = false;
            IdResult = null;

            MethodsStack.Pop();

            //this.CurrentMethod = null;
        }

        public override void visit(ident id)
        {
            if (MethodsStack.Count > 0 && id.name.ToLower() == "result")
            {
                IdResult = id;
                //throw new SyntaxVisitorError("FUNCTIONS_WITH_YIELDS_CANNOT_CONTAIN_RESULT", id.source_context);
            }
            //if (CurrentMethod == null || !HasYields)
            if (MethodsStack.Count == 0 || !HasYields)
            {
                return;
            }
        }

        public override void visit(dot_node dn)
        {
            ProcessNode(dn.left);
            if (dn.right.GetType() != typeof(ident))
                ProcessNode(dn.right);
        }

        public void visit_yield_helper(syntax_tree_node yn)
        {
            procedure_definition pd = null;
            if (MethodsStack.Count > 0)
                pd = MethodsStack.Peek();

            if (pd == null)
                throw new SyntaxVisitorError("ONLY_FUNCTIONS_CAN_CONTAIN_YIELDS", yn.source_context);

            var fh = (pd.proc_header as function_header);
            if (fh == null)
                throw new SyntaxVisitorError("ONLY_FUNCTIONS_CAN_CONTAIN_YIELDS", pd.proc_header.source_context);

            var ttr = fh.return_type as template_type_reference;
            if (ttr != null)
            { 
                if (ttr.name.names.Count == 1 && ttr.name.names[0].name.ToLower() == "ienumerable" && ttr.params_list.Count == 1)
                    fh.return_type = new sequence_type(ttr.params_list.params_list[0]);
                else if (ttr.name.names.Count == 4 && ttr.name.names[0].name.ToLower() == "system" && ttr.name.names[1].name.ToLower() == "collections" && ttr.name.names[2].name.ToLower() == "generic" && ttr.name.names[3].name.ToLower() == "ienumerable" && ttr.params_list.Count == 1)
                    fh.return_type = new sequence_type(ttr.params_list.params_list[0]);
            }
            var seqt = fh.return_type as sequence_type;
            if (seqt == null)
                throw new SyntaxVisitorError("YIELD_FUNC_MUST_RETURN_SEQUENCE", fh.source_context);
            if (seqt.elements_type is procedure_header || seqt.elements_type is function_header)
                throw new SyntaxVisitorError("YIELD_FUNC_CANNOT_RETURN_SEQUENCE_OF_ANONYMOUS_DELEGATES", fh.source_context);
            var pars = fh.parameters;
            if (pars != null)
                foreach (var ps in pars.params_list)
                {
                    if (ps.param_kind != parametr_kind.none && ps.param_kind != parametr_kind.params_parametr)
                        throw new SyntaxVisitorError("FUNCTIONS_WITH_YIELDS_CANNOT_CONTAIN_VAR_CONST_PARAMS_MODIFIERS", pars.source_context);
                    /*if (ps.inital_value != null)
                        throw new SyntaxVisitorError("FUNCTIONS_WITH_YIELDS_CANNOT_CONTAIN_DEFAULT_PARAMETERS", pars.source_context);*/
                }

            HasYields = true;
        }


        public override void visit(yield_node yn)
        {
            visit_yield_helper(yn);
            base.visit(yn);
        }

        public override void visit(yield_sequence_node yn)
        {
            visit_yield_helper(yn);
            base.visit(yn);
        }
    }
}
