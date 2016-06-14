using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

using PascalABCCompiler.Errors;

namespace SyntaxVisitors
{
    public class MarkMethodHasYieldVisitor : WalkingVisitorNew
    {
        private bool HasYields = false;
        private procedure_definition CurrentMethod = null;

        public override void visit(procedure_definition pd)
        {
            this.CurrentMethod = pd;

            HasYields = false;

            base.visit(pd);
            pd.has_yield = HasYields;

            this.CurrentMethod = null;
        }

        public override void visit(ident id)
        {
            if (CurrentMethod == null || !HasYields)
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
            var pd = CurrentMethod;

            if (pd == null)
                throw new SyntaxError("Только функции могут содержать yield", "", yn.source_context, yn);

            var fh = (pd.proc_header as function_header);
            if (fh == null)
                throw new SyntaxError("Только функции могут содержать yield", "", pd.proc_header.source_context, pd.proc_header);
            var seqt = fh.return_type as sequence_type;
            if (seqt == null)
                throw new SyntaxError("Функции с yield должны возвращать последовательность", "", fh.source_context, fh);

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
