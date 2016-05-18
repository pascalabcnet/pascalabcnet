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

            base.visit(pd);
            pd.has_yield = HasYields;

            this.CurrentMethod = null;
        }

        public override void visit(yield_node yn)
        {
            var pd = CurrentMethod;

            var fh = (pd.proc_header as function_header);
            if (fh == null)
                throw new SyntaxError("Only functions can contain yields", "", pd.proc_header.source_context, pd.proc_header);
            var seqt = fh.return_type as sequence_type;
            if (seqt == null)
                throw new SyntaxError("Functions with yields must return sequences", "", fh.return_type.source_context, fh.return_type);

            var pars = fh.parameters;
            if (pars != null)
                foreach (var ps in pars.params_list)
                {
                    if (ps.param_kind != parametr_kind.none)
                        throw new SyntaxError("Parameters of functions with yields must not have 'var', 'const' or 'params' modifier", "", pars.source_context, pars);
                    if (ps.inital_value != null)
                        throw new SyntaxError("Parameters of functions with yields must not have initial values", "", pars.source_context, pars);
                }

            HasYields = true;
        }
    }
}
