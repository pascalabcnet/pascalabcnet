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

        public override void visit(function_lambda_definition ld)
        {
            if (ld.DescendantNodes().OfType<yield_node>().Count() > 0)
            {
                throw new SyntaxError("Лямбда-выражения не могут содержать yield", "", ld.source_context, ld);
            }

            //base.visit(ld); // SSM 15/07/16 - незачем обходить внутренности лямбды - мы и так ищем внутри них yield - этого в этом визиторе достаточно
        }

        public override void visit(with_statement ws)
        {
            if (ws.DescendantNodes().OfType<yield_node>().Count() > 0)
            {
                throw new SyntaxError("Yield запрещен внутри with", "", ws.source_context, ws);
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
                    var FirstTypeDeclaration = ee.defs.DescendantNodes().OfType<type_declarations>().First();
                    if (FirstTypeDeclaration != null)
                    {
                        throw new SyntaxError("Функции с yield не могут содержать локальные определения типов", "", FirstTypeDeclaration.source_context, FirstTypeDeclaration);
                    }
                }
            }

            var innerPds = pd.DescendantNodes().OfType<procedure_definition>();

            if (pd.has_yield && innerPds.Count() > 0
                || innerPds.Where(npd => npd.has_yield).Count() > 0)
            {
                // Есть yield и вложенные - плохо
                // Или нет yield но есть вложенные с yield
                throw new SyntaxError("Функции с yield не могут содержать вложенных подпрограмм result", "", pd.source_context, pd);
            }

            if (pd.has_yield && pd.DescendantNodes().OfType<try_stmt>().Count() > 0)
            {
                throw new SyntaxError("Функции с yield не могут содержать блоков try..except..finally", "", pd.source_context, pd);
            }

            if (pd.has_yield && pd.DescendantNodes().OfType<lock_stmt>().Count() > 0)
            {
                throw new SyntaxError("Функции с yield не могут содержать lock", "", pd.source_context, pd);
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
                throw new SyntaxError("Функции с yield не могут использовать result", "", id.source_context, id);
            }
        }

        public override void visit(dot_node dn)
        {
            ProcessNode(dn.left);
            if (dn.right.GetType() != typeof(ident))
                ProcessNode(dn.right);
        }

        public override void visit(yield_node yn)
        {
            //var pd = CurrentMethod;
            var pd = MethodsStack.Peek();

            if (pd == null)
                throw new SyntaxError("Только функции могут содержать yield", "", yn.source_context, yn);

            var fh = (pd.proc_header as function_header);
            if (fh == null)
                throw new SyntaxError("Только функции могут содержать yield", "", pd.proc_header.source_context, pd.proc_header);
            var seqt = fh.return_type as sequence_type;
            if (seqt == null)
                //throw new SyntaxError("Функции с yield должны возвращать последовательность", "", fh.source_context, fh);
                throw new SyntaxVisitorError("YIELD_FUNC_MUST_RETURN_SEQUENCE", fh.source_context);

            var pars = fh.parameters;
            if (pars != null)
                foreach (var ps in pars.params_list)
                {
                    if (ps.param_kind != parametr_kind.none)
                        throw new SyntaxError("В параметрах функции с yield не должно быть модификаторов 'var', 'const' или 'params'", "", pars.source_context, pars);
                    if (ps.inital_value != null)
                        throw new SyntaxError("Параметры функции с yield не должны иметь начальных значений", "", pars.source_context, pars);
                }

            HasYields = true;

            base.visit(yn);
        }
    }
}
