using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler.Errors;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class AddBeginEndsVisitor : BaseChangeVisitor
    {
        public override void Exit(syntax_tree_node st)
        {
            var sts = st as statement;

            if (sts != null && !(sts is statement_list) && !(sts is case_variant) && !(UpperNode() is statement_list))
            {
                // Одиночный оператор
                var stl = new statement_list(sts, st.source_context);
                Replace(sts, stl);
            }

            
            var lst = st as labeled_statement;

            if (lst != null && !(lst.to_statement is statement_list) && !(lst.to_statement is case_variant))
            {
                // Одиночный оператор
                var stl = new statement_list(sts, st.source_context);
                Replace(lst.to_statement, stl);
            }
            
            base.Exit(st);
        }

    }
}
