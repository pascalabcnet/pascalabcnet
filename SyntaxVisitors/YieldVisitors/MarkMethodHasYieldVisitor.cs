using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

using PascalABCCompiler.Errors;

namespace SyntaxVisitors
{
    public class MarkMethodHasYieldAndCheckSomeErrorsVisitor : WalkingVisitorNew
    {
        private bool HasYields = false;
        private procedure_definition CurrentMethod = null;

        private Stack<procedure_definition> MethodsStack = new Stack<procedure_definition>();

        public static MarkMethodHasYieldAndCheckSomeErrorsVisitor New
        {
            get { return new MarkMethodHasYieldAndCheckSomeErrorsVisitor(); }
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
            base.visit(ws);
        }


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
                }
            }

            var innerPds = pd.DescendantNodes().OfType<procedure_definition>();

            if (pd.has_yield && innerPds.Count() > 0
                || innerPds.Where(npd => npd.has_yield).Count() > 0)
            {
                // Есть yield и вложенные - плохо
                // Или нет yield но есть вложенные с yield
                throw new SyntaxVisitorError("FUNCTIONS_WITH_YIELDS_CANNOT_CONTAIN_NESTED_SUBROUTINES", pd.source_context);
            }

            if (pd.has_yield && pd.DescendantNodes().OfType<try_stmt>().Count() > 0)
            {
                throw new SyntaxVisitorError("FUNCTIONS_WITH_YIELDS_CANNOT_CONTAIN_TRY_EXCEPT_FINALLY", pd.source_context);
            }

            if (pd.has_yield && pd.DescendantNodes().OfType<lock_stmt>().Count() > 0)
            {
                throw new SyntaxVisitorError("FUNCTIONS_WITH_YIELDS_CANNOT_CONTAIN_LOCK", pd.source_context);
            }

            HasYields = false;

            MethodsStack.Pop();

            //this.CurrentMethod = null;
        }

        public override void visit(ident id)
        {
            //if (CurrentMethod == null || !HasYields)
            if (MethodsStack.Count == 0 || !HasYields)
            {
                return;
            }
            if (id.name.ToLower() == "result")
            {
                throw new SyntaxVisitorError("FUNCTIONS_WITH_YIELDS_CANNOT_CONTAIN_RESULT", id.source_context);
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
            var seqt = fh.return_type as sequence_type;
            if (seqt == null)
                throw new SyntaxVisitorError("YIELD_FUNC_MUST_RETURN_SEQUENCE", fh.source_context);

            var pars = fh.parameters;
            if (pars != null)
                foreach (var ps in pars.params_list)
                {
                    if (ps.param_kind != parametr_kind.none)
                        throw new SyntaxVisitorError("FUNCTIONS_WITH_YIELDS_CANNOT_CONTAIN_VAR_CONST_PARAMS_MODIFIERS", pars.source_context);
                    if (ps.inital_value != null)
                        throw new SyntaxVisitorError("FUNCTIONS_WITH_YIELDS_CANNOT_CONTAIN_DEFAULT_PARAMETERS", pars.source_context);
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
