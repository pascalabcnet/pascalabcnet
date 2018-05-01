using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PascalABCCompiler.TreeRealization;

namespace PascalABCCompiler.TreeConverter
{
    public partial class syntax_tree_visitor
    {

        public override void visit(SyntaxTree.pattern_case _pattern_case)
        {
            CheckWhenExpression(_pattern_case.condition);
        }

        private void CheckWhenExpression(SyntaxTree.expression when)
        {
            if (when == null)
                return;

            var convertedWhen = convert_strong(when);
            if (!convertion_data_and_alghoritms.can_convert_type(convertedWhen, SystemLibrary.SystemLibrary.bool_type))
                AddError(get_location(when), "WHEN_EXPRESSION_SHOULD_HAVE_BOOL_TYPE");
        }
    }
}
