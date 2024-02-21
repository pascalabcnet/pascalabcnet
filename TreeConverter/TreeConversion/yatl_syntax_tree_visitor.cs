using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.SyntaxTree;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeRealization;
using PascalABCCompiler.SemanticTree;
using PascalABCCompiler.SystemLibrary;
using PascalABCCompiler.TreeConverter.TreeConversion;

namespace YATLTreeConversion
{
    class yatl_syntax_tree_visitor: syntax_tree_visitor, ISyntaxTreeVisitor
    {
        private string[] filesExtensions = { ".yatp" };

        public override string[] FilesExtensions
        {
            get
            {
                return filesExtensions;
            }
        }
        public override void visit(bin_expr _bin_expr)
        {
            expression_node left = convert_strong(_bin_expr.left);
            expression_node right = convert_strong(_bin_expr.right);

            if (_bin_expr.operation_type == Operators.Plus)
            {
                if (type_table.compare_types(left.type, right.type) == type_compare.greater_type)
                    AddError(left.location, "ERROR");
            }

            if (_bin_expr.operation_type == Operators.In)
                try_convert_typed_expression_to_function_call(ref left);
            expression_node res = find_operator(_bin_expr.operation_type, left, right, get_location(_bin_expr));

            if (res.type is undefined_type)
                AddError(get_location(_bin_expr), "OPERATOR_RETURN_TYPE_UNDEFINED_{0}", name_reflector.get_name(_bin_expr.operation_type));
            if (res.type.type_special_kind == type_special_kind.base_set_type)
            {
                if (left.type.element_type == right.type.element_type)
                    res.type = left.type;
                else if (type_table.compare_types(left.type, right.type) == type_compare.greater_type)
                    res.type = left.type;
                else if (type_table.compare_types(left.type, right.type) == type_compare.less_type)
                    res.type = right.type;
                else if (type_table.compare_types(left.type, right.type) == type_compare.non_comparable_type)
                    res.type = context.create_set_type(SystemLibrary.object_type, get_location(_bin_expr));
            }
            //ssyy, 15.05.2009
            switch (_bin_expr.operation_type)
            {
                case PascalABCCompiler.SyntaxTree.Operators.Equal:
                case PascalABCCompiler.SyntaxTree.Operators.NotEqual:
                    if (left.type.is_generic_parameter && right.type.is_generic_parameter)
                    {
                        compiled_static_method_call cfc = new compiled_static_method_call(
                            SystemLibrary.object_equals_method, get_location(_bin_expr));
                        cfc.parameters.AddElement(left);
                        cfc.parameters.AddElement(right);
                        if (_bin_expr.operation_type == PascalABCCompiler.SyntaxTree.Operators.NotEqual)
                        {
                            res = new basic_function_call(SystemLibrary.bool_not as basic_function_node, get_location(_bin_expr), cfc);
                        }
                        else
                        {
                            res = cfc;
                        }
                    }
                    else if (left is static_event_reference)
                    {
                        if ((left as static_event_reference).en is compiled_event)
                            AddError(left.location, "EVENT_{0}_MUST_BE_IN_LEFT_PART", (left as static_event_reference).en.name);
                        if (context.converted_type != null && context.converted_type != ((left as static_event_reference).en as common_event).cont_type && !context.converted_type.name.Contains("<>local_variables_class"))
                            AddError(left.location, "EVENT_{0}_MUST_BE_IN_LEFT_PART", (left as static_event_reference).en.name);
                    }
                    else if (right is static_event_reference)
                    {
                        if ((right as static_event_reference).en is compiled_event)
                            AddError(right.location, "EVENT_{0}_MUST_BE_IN_LEFT_PART", (right as static_event_reference).en.name);
                        if (context.converted_type != null && context.converted_type != ((right as static_event_reference).en as common_event).cont_type)
                            AddError(right.location, "EVENT_{0}_MUST_BE_IN_LEFT_PART", (right as static_event_reference).en.name);
                    }
                    break;
            }
            base.return_value(res);
        }
    }
}
