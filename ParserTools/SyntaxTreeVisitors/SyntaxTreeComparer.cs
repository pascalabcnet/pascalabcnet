// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PascalABCCompiler.SyntaxTree
{
    public class SyntaxNodesNotEqual : Exception
    {
        public syntax_tree_node left;
        public syntax_tree_node right;

        public SyntaxNodesNotEqual(syntax_tree_node left, syntax_tree_node right)
        {
            this.left = left;
            this.right = right;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class SyntaxTreeComparer
    {
        public void Compare(compilation_unit left, compilation_unit right)
        {
            if (left.GetType() != right.GetType())
                throw_not_equal(left, right);
            if (left is program_module)
                CompareInternal(left as program_module, right as program_module);
            else if (left is unit_module)
                CompareInternal(left as unit_module, right as unit_module);
        }

        private void throw_not_equal(syntax_tree_node left, syntax_tree_node right)
        {
            throw new SyntaxNodesNotEqual(left, right);
        }

        public void CompareInternal(access_modifer_node left, access_modifer_node right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.access_level != right.access_level)
                    throw_not_equal(left, right);
            }
        }

        public void CompareInternal(array_const left, array_const right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.elements, right.elements);
            }
        }

        public void CompareInternal(array_of_const_type_definition left, array_of_const_type_definition right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.attr_list, right.attr_list);
            }
        }

        public void CompareInternal(array_of_named_type_definition left, array_of_named_type_definition right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.attr_list, right.attr_list);
                CompareInternal(left.type_name, right.type_name);
            }
        }

        public void CompareInternal(array_size left, array_size right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.attr_list, right.attr_list);
                CompareInternal(left.max_value, right.max_value);
            }
        }

        public void CompareInternal(array_type left, array_type right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.attr_list, right.attr_list);
                CompareInternal(left.indexers, right.indexers);
                CompareInternal(left.elements_type, right.elements_type);
            }
        }

        public void CompareInternal(assign left, assign right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.operator_type != right.operator_type)
                    throw_not_equal(left, right);
                CompareInternal(left.to, right.to);
                CompareInternal(left.from, right.from);
            }
        }

        public void CompareInternal(attribute left, attribute right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.arguments, right.arguments);
                CompareInternal(left.qualifier, right.qualifier);
                CompareInternal(left.type, right.type);
            }
        }

        public void CompareInternal(attribute_list left, attribute_list right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.attributes.Count != right.attributes.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.attributes.Count; i++)
                    CompareInternal(left.attributes[i], right.attributes[i]);
            }
        }

        public void CompareInternal(bin_expr left, bin_expr right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.operation_type != right.operation_type)
                    throw_not_equal(left, right);
                CompareInternal(left.left, right.left);
                CompareInternal(left.right, right.right);
            }
        }

        public void CompareInternal(block left, block right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.defs, right.defs);
                CompareInternal(left.program_code, right.program_code);
            }
        }

        public void CompareInternal(bool_const left, bool_const right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.val != right.val)
                    throw_not_equal(left, right);
            }
        }

        public void CompareInternal(bracket_expr left, bracket_expr right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.expr, right.expr);
            }
        }

        public void CompareInternal(case_node left, case_node right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.param, right.param);
                CompareInternal(left.conditions, right.conditions);
                CompareInternal(left.else_statement, right.else_statement);
            }
        }

        public void CompareInternal(case_variant left, case_variant right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.conditions, right.conditions);
                CompareInternal(left.exec_if_true, right.exec_if_true);
            }
        }

        public void CompareInternal(case_variants left, case_variants right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.variants.Count != right.variants.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.variants.Count; i++)
                    CompareInternal(left.variants[i], right.variants[i]);
            }
        }

        public void CompareInternal(char_const left, char_const right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.cconst != right.cconst)
                    throw_not_equal(left, right);
            }
        }

        public void CompareInternal(class_body left, class_body right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.class_def_blocks.Count != right.class_def_blocks.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.class_def_blocks.Count; i++)
                    CompareInternal(left.class_def_blocks[i], right.class_def_blocks[i]);
            }
        }

        public void CompareInternal(class_definition left, class_definition right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.attr_list, right.attr_list);
                if (left.attribute != right.attribute)
                    throw_not_equal(left, right);
                if (left.keyword != right.keyword)
                    throw_not_equal(left, right);
                CompareInternal(left.class_parents, right.class_parents);
                CompareInternal(left.where_section, right.where_section);
                CompareInternal(left.template_args, right.template_args);
                CompareInternal(left.body, right.body);
            }
        }

        public void CompareInternal(class_members left, class_members right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.access_mod, right.access_mod);
                if (left.members.Count != right.members.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.members.Count; i++)
                    CompareInternal(left.members[i], right.members[i]);
            }
        }

        public void CompareInternal(compiler_directive left, compiler_directive right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.Name, right.Name);
                CompareInternal(left.Directive, right.Directive);
            }
        }

        public void CompareInternal(constructor left, constructor right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.attributes, right.attributes);
                if (left.class_keyword != right.class_keyword)
                    throw_not_equal(left, right);
                CompareInternal(left.name, right.name);
                CompareInternal(left.parameters, right.parameters);
                CompareInternal(left.template_args, right.template_args);
                CompareInternal(left.where_defs, right.where_defs);
                CompareInternal(left.proc_attributes, right.proc_attributes);
            }
        }

        public void CompareInternal(consts_definitions_list left, consts_definitions_list right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.const_defs.Count != right.const_defs.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.const_defs.Count; i++)
                    CompareInternal(left.const_defs[i], right.const_defs[i]);
            }
        }

        public void CompareInternal(declaration left, declaration right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.GetType() != right.GetType())
                    throw_not_equal(left, right);
                if (left is function_header)
                    CompareInternal(left as function_header, right as function_header);
                else if (left is constructor)
                    CompareInternal(left as constructor, right as constructor);
                else if (left is destructor)
                    CompareInternal(left as destructor, right as destructor);
                else if (left is procedure_header)
                    CompareInternal(left as procedure_header, right as procedure_header);
                else if (left is procedure_definition)
                    CompareInternal(left as procedure_definition, right as procedure_definition);
                else if (left is variable_definitions)
                    CompareInternal(left as variable_definitions, right as variable_definitions);
                else if (left is consts_definitions_list)
                    CompareInternal(left as consts_definitions_list, right as consts_definitions_list);
                else if (left is type_declarations)
                    CompareInternal(left as type_declarations, right as type_declarations);
                else if (left is label_definitions)
                    CompareInternal(left as label_definitions, right as label_definitions);
                else if (left is simple_property)
                    CompareInternal(left as simple_property, right as simple_property);
                else if (left is var_def_statement)
                    CompareInternal(left as var_def_statement, right as var_def_statement);
                else if (left is simple_const_definition)
                    CompareInternal(left as simple_const_definition, right as simple_const_definition);
                else if (left is typed_const_definition)
                    CompareInternal(left as typed_const_definition, right as typed_const_definition);
                else
                    throw new NotImplementedException(left.GetType().ToString());
            }
        }

        public void CompareInternal(declaration_specificator left, declaration_specificator right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (string.Compare(left.name, right.name, true) != 0)
                    throw_not_equal(left, right);
                if (left.specificator != right.specificator)
                    throw_not_equal(left, right);
            }
        }

        public void CompareInternal(declarations left, declarations right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.defs.Count != right.defs.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.defs.Count; i++)
                {
                    CompareInternal(left.defs[i], right.defs[i]);
                }
            }
        }

        public void CompareInternal(declarations_as_statement left, declarations_as_statement right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.defs, right.defs);
            }
        }

        public void CompareInternal(default_indexer_property_node left, default_indexer_property_node right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                
            }
        }

        public void CompareInternal(default_operator left, default_operator right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.type_name, right.type_name);
            }
        }

        public void CompareInternal(destructor left, destructor right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.attributes, right.attributes);
                if (left.class_keyword != right.class_keyword)
                    throw_not_equal(left, right);
                CompareInternal(left.name, right.name);
                CompareInternal(left.template_args, right.template_args);
                CompareInternal(left.parameters, right.parameters);
                CompareInternal(left.where_defs, right.where_defs);
                CompareInternal(left.proc_attributes, right.proc_attributes);
            }
        }

        public void CompareInternal(diap_expr left, diap_expr right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.left, right.left);
                CompareInternal(left.right, right.right);
            }
        }

        public void CompareInternal(diapason left, diapason right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.left, right.left);
                CompareInternal(left.right, right.right);
            }
        }

        public void CompareInternal(diapason_expr left, diapason_expr right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.left, right.left);
                CompareInternal(left.right, right.right);
            }
        }

        public void CompareInternal(dot_node left, dot_node right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.left, right.left);
                CompareInternal(left.right, right.right);
            }
        }

        public void CompareInternal(double_const left, double_const right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.val != right.val)
                    throw_not_equal(left, right);
            }
        }

        public void CompareInternal(empty_statement left, empty_statement right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
               
            }
        }

        public void CompareInternal(enum_type_definition left, enum_type_definition right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.enumerators, right.enumerators);
            }
        }

        public void CompareInternal(enumerator left, enumerator right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.name, right.name);
                CompareInternal(left.value, right.value);
            }
        }

        public void CompareInternal(enumerator_list left, enumerator_list right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.enumerators.Count != right.enumerators.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.enumerators.Count; i++)
                    CompareInternal(left.enumerators[i], right.enumerators[i]);
            }
        }

        public void CompareInternal(exception_block left, exception_block right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.handlers, right.handlers);
                CompareInternal(left.stmt_list, right.stmt_list);
                CompareInternal(left.else_stmt_list, right.else_stmt_list);
            }
        }

        public void CompareInternal(exception_handler left, exception_handler right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.variable, right.variable);
                CompareInternal(left.type_name, right.type_name);
                CompareInternal(left.statements, right.statements);
            }
        }

        public void CompareInternal(exception_handler_list left, exception_handler_list right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.handlers.Count != right.handlers.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.handlers.Count; i++)
                    CompareInternal(left.handlers[i], right.handlers[i]);
            }
        }

        public void CompareInternal(expression left, expression right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.GetType() != right.GetType())
                    throw_not_equal(left, right);
                /*Type t = left.GetType();
                MethodInfo mi = typeof(SyntaxTreeComparer).GetMethod("CompareInternal", new Type[] { t, t });
                mi.Invoke(this, new object[] { left, right });*/
                if (left is ident)
                    CompareInternal(left as ident, right as ident);
                else if (left is dot_node)
                    CompareInternal(left as dot_node, right as dot_node);
                else if (left is method_call)
                    CompareInternal(left as method_call, right as method_call);
                else if (left is bracket_expr)
                    CompareInternal(left as bracket_expr, right as bracket_expr);
                else if (left is new_expr)
                    CompareInternal(left as new_expr, right as new_expr);
                else if (left is int32_const)
                    CompareInternal(left as int32_const, right as int32_const);
                else if (left is int64_const)
                    CompareInternal(left as int64_const, right as int64_const);
                else if (left is bool_const)
                    CompareInternal(left as bool_const, right as bool_const);
                else if (left is char_const)
                    CompareInternal(left as char_const, right as char_const);
                else if (left is string_const)
                    CompareInternal(left as string_const, right as string_const);
                else if (left is double_const)
                    CompareInternal(left as double_const, right as double_const);
                else if (left is sharp_char_const)
                    CompareInternal(left as sharp_char_const, right as sharp_char_const);
                else if (left is literal_const_line)
                    CompareInternal(left as literal_const_line, right as literal_const_line);
                else if (left is nil_const)
                    CompareInternal(left as nil_const, right as nil_const);
                else if (left is array_const)
                    CompareInternal(left as array_const, right as array_const);
                else if (left is record_const)
                    CompareInternal(left as record_const, right as record_const);
                else if (left is pascal_set_constant)
                    CompareInternal(left as pascal_set_constant, right as pascal_set_constant);
                else if (left is roof_dereference)
                    CompareInternal(left as roof_dereference, right as roof_dereference);
                else if (left is sizeof_operator)
                    CompareInternal(left as sizeof_operator, right as sizeof_operator);
                else if (left is default_operator)
                    CompareInternal(left as default_operator, right as default_operator);
                else if (left is bin_expr)
                    CompareInternal(left as bin_expr, right as bin_expr);
                else if (left is un_expr)
                    CompareInternal(left as un_expr, right as un_expr);
                else if (left is uint64_const)
                    CompareInternal(left as uint64_const, right as uint64_const);
                else if (left is int64_const)
                    CompareInternal(left as int64_const, right as int64_const);
                else if (left is diap_expr)
                    CompareInternal(left as diap_expr, right as diap_expr);
                else if (left is diapason_expr)
                    CompareInternal(left as diapason_expr, right as diapason_expr);
                else if (left is get_address)
                    CompareInternal(left as get_address, right as get_address);
                else if (left is format_expr)
                    CompareInternal(left as format_expr, right as format_expr);
                else if (left is hex_constant)
                    CompareInternal(left as hex_constant, right as hex_constant);
                else if (left is typecast_node)
                    CompareInternal(left as typecast_node, right as typecast_node);
                else if (left is question_colon_expression)
                    CompareInternal(left as question_colon_expression, right as question_colon_expression);
                else if (left is indexer)
                    CompareInternal(left as indexer, right as indexer);
                else if (left is ident_with_templateparams)
                    CompareInternal(left as ident_with_templateparams, right as ident_with_templateparams);
                //else if (left is assign)      // SSM 12/06/15
                //    CompareInternal(left as assign, right as assign);
                else if (left is typeof_operator)
                    CompareInternal(left as typeof_operator, right as typeof_operator);
                else if (left is function_lambda_call)
                    CompareInternal(left as function_lambda_call, right as function_lambda_call);
                else if (left is function_lambda_definition)
                    CompareInternal(left as function_lambda_definition, right as function_lambda_definition);
                else
                    throw new NotImplementedException(left.GetType().ToString());

            }
        }

        public void CompareInternal(expression_as_statement left, expression_as_statement right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.expr, right.expr);
            }
        }

        public void CompareInternal(expression_list left, expression_list right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.expressions.Count != right.expressions.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.expressions.Count; i++)
                    CompareInternal(left.expressions[i], right.expressions[i]);
            }
        }

        public void CompareInternal(external_directive left, external_directive right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.modulename, right.modulename);
                CompareInternal(left.name, right.name);
            }
        }

        public void CompareInternal(file_type left, file_type right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.file_of_type, right.file_of_type);
            }
        }

        public void CompareInternal(file_type_definition left, file_type_definition right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.elem_type, right.elem_type);
            }
        }

        public void CompareInternal(for_node left, for_node right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.create_loop_variable != right.create_loop_variable)
                    throw_not_equal(left, right);
                if (left.cycle_type != right.cycle_type)
                    throw_not_equal(left, right);
                CompareInternal(left.initial_value, right.initial_value);
                CompareInternal(left.loop_variable, right.loop_variable);
                CompareInternal(left.finish_value, right.finish_value);
                CompareInternal(left.statements, right.statements);
            }
        }

        public void CompareInternal(foreach_stmt left, foreach_stmt right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.identifier, right.identifier);
                CompareInternal(left.in_what, right.in_what);
                CompareInternal(left.type_name, right.type_name);
                CompareInternal(left.stmt, right.stmt);
            }
        }

        public void CompareInternal(formal_parameters left, formal_parameters right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.params_list.Count != right.params_list.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.params_list.Count; i++)
                    CompareInternal(left.params_list[i], right.params_list[i]);
            }
        }

        public void CompareInternal(format_expr left, format_expr right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.expr, right.expr);
                CompareInternal(left.format1, left.format1);
                CompareInternal(left.format2, right.format2);
            }
        }

        public void CompareInternal(function_header left, function_header right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.class_keyword != right.class_keyword)
                    throw_not_equal(left, right);
                CompareInternal(left.attributes, right.attributes);
                CompareInternal(left.name, right.name);
                CompareInternal(left.template_args, right.template_args);
                CompareInternal(left.parameters, right.parameters);
                CompareInternal(left.return_type, right.return_type);
                CompareInternal(left.where_defs, right.where_defs);
                CompareInternal(left.proc_attributes, right.proc_attributes);
            }
        }

        public void CompareInternal(function_lambda_definition left, function_lambda_definition right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.ident_list, right.ident_list);
                CompareInternal(left.parameters, right.parameters);
                CompareInternal(left.formal_parameters, right.formal_parameters);
                CompareInternal(left.proc_definition, right.proc_definition);
                CompareInternal(left.proc_body, right.proc_body);
                CompareInternal(left.return_type, right.return_type);
            }
        }

        public void CompareInternal(get_address left, get_address right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.address_of, right.address_of);
            }
        }

        public void CompareInternal(goto_statement left, goto_statement right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.label, right.label);
            }
        }

        public void CompareInternal(hex_constant left, hex_constant right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.val != right.val)
                    throw_not_equal(left, right);
            }
        }

        public void CompareInternal(ident left, ident right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.GetType() != right.GetType())
                    throw_not_equal(left, right);
                if (left.GetType() != typeof(ident))
                {
                    if (left is inherited_ident)
                        CompareInternal(left as inherited_ident, right as inherited_ident);
                    else if (left is template_type_name)
                        CompareInternal(left as template_type_name, right as template_type_name);
                    else if (left is procedure_attribute)
                        CompareInternal(left as procedure_attribute, right as procedure_attribute);
                    else if (left is operator_name_ident)
                        CompareInternal(left as operator_name_ident, right as operator_name_ident);
                    else
                        throw new NotSupportedException(left.GetType().ToString());
                }
                if (string.Compare(left.name, right.name, true) != 0)
                    throw_not_equal(left, right);
            }
        }

        public void CompareInternal(ident_list left, ident_list right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.idents.Count != right.idents.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.idents.Count; i++)
                    CompareInternal(left.idents[i], right.idents[i]);
            }
        }

        public void CompareInternal(ident_with_templateparams left, ident_with_templateparams right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.name, right.name);
                CompareInternal(left.template_params, right.template_params);
            }
        }

        public void CompareInternal(if_node left, if_node right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.condition, right.condition);
                CompareInternal(left.then_body, right.then_body);
                CompareInternal(left.else_body, right.else_body);
            }
        }

        public void CompareInternal(implementation_node left, implementation_node right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.uses_modules, right.uses_modules);
                CompareInternal(left.implementation_definitions, right.implementation_definitions);
            }
        }

        public void CompareInternal(indexer left, indexer right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.dereferencing_value, right.dereferencing_value);
                CompareInternal(left.indexes, right.indexes);
            }
        }

        public void CompareInternal(indexers_types left, indexers_types right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.indexers.Count != right.indexers.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.indexers.Count; i++)
                    CompareInternal(left.indexers[i], right.indexers[i]);
            }
        }

        public void CompareInternal(inherited_ident left, inherited_ident right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (string.Compare(left.name, right.name, true) != 0)
                    throw_not_equal(left, right);
            }
        }

        public void CompareInternal(inherited_message left, inherited_message right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                
            }
        }

        public void CompareInternal(inherited_method_call left, inherited_method_call right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.method_name, right.method_name);
                CompareInternal(left.exprs, right.exprs);
            }
        }

        public void CompareInternal(int32_const left, int32_const right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.val != right.val)
                    throw_not_equal(left, right);
            }
        }

        public void CompareInternal(int64_const left, int64_const right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.val != right.val)
                    throw_not_equal(left, right);
            }
        }

        public void CompareInternal(interface_node left, interface_node right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.uses_modules, right.uses_modules);
                CompareInternal(left.interface_definitions, right.interface_definitions);
            }
        }

        public void CompareInternal(label_definitions left, label_definitions right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.attributes, right.attributes);
                CompareInternal(left.labels, right.labels);
            }
        }

        public void CompareInternal(labeled_statement left, labeled_statement right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.label_name, right.label_name);
                CompareInternal(left.to_statement, right.to_statement);
            }
        }

        public void CompareInternal(literal left, literal right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.GetType() != right.GetType())
                    throw_not_equal(left, right);
                /*Type t = left.GetType();
                MethodInfo mi = typeof(SyntaxTreeComparer).GetMethod("CompareInternal", new Type[] { t, t });
                mi.Invoke(this, new object[] { left, right });*/
                if (left is char_const)
                    CompareInternal(left as char_const, right as char_const);
                else if (left is sharp_char_const)
                    CompareInternal(left as sharp_char_const, right as sharp_char_const);
                else if (left is string_const)
                    CompareInternal(left as string_const, right as string_const);
                else
                    throw new NotImplementedException(left.GetType().ToString());
            }
        }

        public void CompareInternal(literal_const_line left, literal_const_line right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.literals.Count != right.literals.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.literals.Count; i++)
                    CompareInternal(left.literals[i], right.literals[i]);
            }
        }

        public void CompareInternal(lock_stmt left, lock_stmt right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.lock_object, right.lock_object);
                CompareInternal(left.stmt, right.stmt);
            }
        }

        public void CompareInternal(method_call left, method_call right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.dereferencing_value, right.dereferencing_value);
                CompareInternal(left.parameters, right.parameters);
            }
        }

        public void CompareInternal(method_name left, method_name right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.ln != null && right.ln == null)
                    throw_not_equal(left, right);
                if (left.ln == null && right.ln != null)
                    throw_not_equal(left, right);
                if (left.ln != null && right.ln != null)
                {
                    if (left.ln.Count != right.ln.Count)
                        throw_not_equal(left, right);
                    for (int i = 0; i < left.ln.Count; i++)
                    {
                        CompareInternal(left.ln[i], right.ln[i]);
                    }
                }
                else
                {
                    CompareInternal(left.class_name, right.class_name);
                    CompareInternal(left.explicit_interface_name, right.explicit_interface_name);
                    CompareInternal(left.meth_name, right.meth_name);
                }
            }
        }

        public void CompareInternal(named_type_reference left, named_type_reference right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.names.Count != right.names.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.names.Count; i++)
                    CompareInternal(left.names[i], right.names[i]);
            }
        }

        public void CompareInternal(lambda_inferred_type left, lambda_inferred_type right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
        }

        public void CompareInternal(no_type_foreach left, no_type_foreach right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
        }

        public void CompareInternal(named_type_reference_list left, named_type_reference_list right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.types.Count != right.types.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.types.Count; i++)
                    CompareInternal(left.types[i], right.types[i]);
            }
        }

        public void CompareInternal(new_expr left, new_expr right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.new_array != right.new_array)
                    throw_not_equal(left, right);
                CompareInternal(left.params_list, right.params_list);
                CompareInternal(left.type, right.type);
                CompareInternal(left.array_init_expr, right.array_init_expr);
            }
        }

        public void CompareInternal(nil_const left, nil_const right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
            }
        }

        public void CompareInternal(op_type_node left, op_type_node right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (string.Compare(left.text, right.text, true) != 0)
                    throw_not_equal(left, right);
                if (left.type != right.type)
                    throw_not_equal(left, right);
            }
        }

        public void CompareInternal(operator_name_ident left, operator_name_ident right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (string.Compare(left.name, right.name, true) != 0)
                    throw_not_equal(left, right);
            }
        }

        public void CompareInternal(pascal_set_constant left, pascal_set_constant right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.values, right.values);
            }
        }

        public void CompareInternal(proc_block left, proc_block right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.GetType() != right.GetType())
                    throw_not_equal(left, right);
                /*Type t = left.GetType();
                MethodInfo mi = typeof(SyntaxTreeComparer).GetMethod("CompareInternal", new Type[] { t, t });
                mi.Invoke(this, new object[] { left, right });*/
                if (left is block)
                    CompareInternal(left as block, right as block);
                else if (left is external_directive)
                    CompareInternal(left as external_directive, right as external_directive);
                else
                    throw new NotImplementedException(left.GetType().ToString());
            }
        }

        public void CompareInternal(procedure_attribute left, procedure_attribute right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.attribute_type != right.attribute_type)
                    throw_not_equal(left, right);
            }
        }

        public void CompareInternal(procedure_attributes_list left, procedure_attributes_list right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.proc_attributes.Count != right.proc_attributes.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.proc_attributes.Count; i++)
                    CompareInternal(left.proc_attributes[i], right.proc_attributes[i]);
            }
        }

        public void CompareInternal(procedure_call left, procedure_call right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.func_name, right.func_name);
            }
        }

        public void CompareInternal(procedure_definition left, procedure_definition right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.attributes, right.attributes);
                CompareInternal(left.proc_header, right.proc_header);
                CompareInternal(left.proc_body, right.proc_body);
            }
        }

        public void CompareInternal(procedure_header left, procedure_header right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.GetType() != right.GetType())
                    throw_not_equal(left, right);
                Type t = left.GetType();
                if (t != typeof(procedure_header))
                {
                    /*MethodInfo mi = t.GetMethod("CompareInternal", new Type[] { t, t });
                    mi.Invoke(this, new object[] { left, right });
                    return;*/
                    if (left is function_header)
                    {
                        CompareInternal(left as function_header, right as function_header);
                        return;
                    }
                    else if (left is constructor)
                    {
                        CompareInternal(left as constructor, right as constructor);
                        return;
                    }
                    else if (left is destructor)
                    {
                        CompareInternal(left as destructor, right as destructor);
                        return;
                    }
                    else
                        throw new NotSupportedException(left.GetType().ToString());
                }
                if (left.class_keyword != right.class_keyword)
                    throw_not_equal(left, right);
                CompareInternal(left.attributes, right.attributes);
                CompareInternal(left.name, right.name);
                CompareInternal(left.template_args, right.template_args);
                CompareInternal(left.parameters, right.parameters);
                CompareInternal(left.where_defs, right.where_defs);
                CompareInternal(left.proc_attributes, right.proc_attributes);
            }
        }

        public void CompareInternal(program_body left, program_body right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.program_definitions, right.program_definitions);

                CompareInternal(left.used_units, right.used_units);
                CompareInternal(left.program_code, right.program_code);
            }
        }

        public void CompareInternal(program_module left, program_module right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.program_name, right.program_name);
                CompareInternal(left.used_units, right.used_units);
                CompareInternal(left.program_block, right.program_block);
                if (left.compiler_directives.Count != right.compiler_directives.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.compiler_directives.Count; i++)
                    CompareInternal(left.compiler_directives[i], right.compiler_directives[i]);
            }
        }

        public void CompareInternal(program_name left, program_name right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
                CompareInternal(left.prog_name, right.prog_name);
        }

        public void CompareInternal(property_accessors left, property_accessors right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.read_accessor, right.read_accessor);
                CompareInternal(left.write_accessor, right.write_accessor);
            }
        }

        public void CompareInternal(property_array_default left, property_array_default right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
               
            }
        }

        public void CompareInternal(property_interface left, property_interface right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.property_type, right.property_type);
                CompareInternal(left.parameter_list, right.parameter_list);
                CompareInternal(left.index_expression, right.index_expression);
            }
        }

        public void CompareInternal(property_parameter left, property_parameter right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.names, right.names);
                CompareInternal(left.type, right.type);
            }
        }

        public void CompareInternal(property_parameter_list left, property_parameter_list right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.parameters.Count != right.parameters.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.parameters.Count; i++)
                    CompareInternal(left.parameters[i], right.parameters[i]);
            }
        }

        public void CompareInternal(question_colon_expression left, question_colon_expression right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.condition, right.condition);
                CompareInternal(left.ret_if_true, left.ret_if_true);
                CompareInternal(left.ret_if_false, right.ret_if_false);
            }
        }

        public void CompareInternal(raise_statement left, raise_statement right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.excep, right.excep);
            }
        }

        public void CompareInternal(raise_stmt left, raise_stmt right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.expr, right.expr);
            }
        }

        public void CompareInternal(read_accessor_name left, read_accessor_name right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.accessor_name, right.accessor_name);
            }
        }

        public void CompareInternal(record_const left, record_const right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.rec_consts.Count != right.rec_consts.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.rec_consts.Count; i++)
                    CompareInternal(left.rec_consts[i], right.rec_consts[i]);
            }
        }

        public void CompareInternal(record_const_definition left, record_const_definition right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.name, right.name);
                CompareInternal(left.val, right.val);
            }
        }

        public void CompareInternal(ref_type left, ref_type right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.pointed_to, right.pointed_to);
            }
        }

        public void CompareInternal(repeat_node left, repeat_node right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.expr, right.expr);
                CompareInternal(left.statements, right.statements);
            }
        }

        public void CompareInternal(roof_dereference left, roof_dereference right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.dereferencing_value, right.dereferencing_value);
            }
        }

        public void CompareInternal(set_type_definition left, set_type_definition right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.of_type, right.of_type);
            }
        }

        public void CompareInternal(sharp_char_const left, sharp_char_const right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.char_num != right.char_num)
                    throw_not_equal(left, right);
            }
        }

        public void CompareInternal(simple_attribute_list left, simple_attribute_list right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.attributes.Count != right.attributes.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.attributes.Count; i++)
                    CompareInternal(left.attributes[i], right.attributes[i]);
            }
        }

        public void CompareInternal(simple_const_definition left, simple_const_definition right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.attributes, right.attributes);
                CompareInternal(left.const_name, right.const_name);
                CompareInternal(left.const_value, left.const_value);
            }
        }

        public void CompareInternal(simple_property left, simple_property right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.attr != right.attr)
                    throw_not_equal(left, right);
                CompareInternal(left.attributes, right.attributes);
                CompareInternal(left.property_name, right.property_name);
                CompareInternal(left.parameter_list, left.parameter_list);
                CompareInternal(left.property_type, right.property_type);
                CompareInternal(left.index_expression, right.index_expression);
                CompareInternal(left.accessors, right.accessors);
                CompareInternal(left.array_default, right.array_default);
            }
        }

        public void CompareInternal(sizeof_operator left, sizeof_operator right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.expr, right.expr);
            }
        }

        public void CompareInternal(statement left, statement right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.GetType() != right.GetType())
                    throw_not_equal(left, right);
                /*Type t = left.GetType();
                MethodInfo mi = typeof(SyntaxTreeComparer).GetMethod("CompareInternal", new Type[] { t, t });
                mi.Invoke(this, new object[] { left, right });*/
                if (left is if_node)
                    CompareInternal(left as if_node, right as if_node);
                else if (left is while_node)
                    CompareInternal(left as while_node, right as while_node);
                else if (left is for_node)
                    CompareInternal(left as for_node, right as for_node);
                else if (left is repeat_node)
                    CompareInternal(left as repeat_node, right as repeat_node);
                else if (left is foreach_stmt)
                    CompareInternal(left as foreach_stmt, right as foreach_stmt);
                else if (left is raise_stmt)
                    CompareInternal(left as raise_stmt, right as raise_stmt);
                else if (left is case_node)
                    CompareInternal(left as case_node, right as case_node);
                else if (left is with_statement)
                    CompareInternal(left as with_statement, right as with_statement);
                else if (left is lock_stmt)
                    CompareInternal(left as lock_stmt, right as lock_stmt);
                else if (left is try_stmt)
                    CompareInternal(left as try_stmt, right as try_stmt);
                else if (left is goto_statement)
                    CompareInternal(left as goto_statement, right as goto_statement);
                else if (left is labeled_statement)
                    CompareInternal(left as labeled_statement, right as labeled_statement);
                else if (left is case_node)
                    CompareInternal(left as case_node, right as case_node);
                else if (left is empty_statement)
                    CompareInternal(left as empty_statement, right as empty_statement);
                else if (left is case_node)
                    CompareInternal(left as case_node, right as case_node);
                else if (left is raise_statement)
                    CompareInternal(left as raise_statement, right as raise_statement);
                else if (left is var_statement)
                    CompareInternal(left as var_statement, right as var_statement);
                else if (left is procedure_call)
                    CompareInternal(left as procedure_call, right as procedure_call);
                else if (left is expression_as_statement)
                    CompareInternal(left as expression_as_statement, right as expression_as_statement);
                else if (left is inherited_message)
                    CompareInternal(left as inherited_message, right as inherited_message);
                else if (left is statement_list)
                    CompareInternal(left as statement_list, right as statement_list);
                else if (left is inherited_method_call)
                    CompareInternal(left as inherited_method_call, right as inherited_method_call);
                else if (left is assign)
                    CompareInternal(left as assign, right as assign);
                //else if (left is expression) // SSM 12/06/15
                //    CompareInternal(left as expression, right as expression);

                else throw new NotImplementedException(left.GetType().ToString());
            }
        }

        public void CompareInternal(statement_list left, statement_list right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.subnodes.Count != right.subnodes.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.subnodes.Count; i++)
                {
                    CompareInternal(left.subnodes[i], right.subnodes[i]);
                }
            }
        }

        public void CompareInternal(string_const left, string_const right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.Value != right.Value)
                    throw_not_equal(left, right);
            }
        }

        public void CompareInternal(string_num_definition left, string_num_definition right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.name, right.name);
                CompareInternal(left.num_of_symbols, right.num_of_symbols);
            }
        }

        public void CompareInternal(template_param_list left, template_param_list right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.dereferencing_value, right.dereferencing_value);
                if (left.params_list.Count != right.params_list.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.params_list.Count; i++)
                    CompareInternal(left.params_list[i], right.params_list[i]);
            }
        }

        public void CompareInternal(template_type_name left, template_type_name right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (string.Compare(left.name, right.name, true) != 0)
                    throw_not_equal(left, right);
                CompareInternal(left.template_args, right.template_args);
            }
        }

        public void CompareInternal(template_type_reference left, template_type_reference right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.name, right.name);
                CompareInternal(left.params_list, right.params_list);
                if (left.names == null && right.names != null || left.names != null && right.names == null)
                    throw_not_equal(left, right);
                if (left.names != right.names)
                    for (int i = 0; i < left.names.Count; i++)
                        CompareInternal(left.names[i], right.names[i]);
            }
        }

        public void CompareInternal(token_info left, token_info right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (string.Compare(left.text, right.text, true) != 0)
                    throw_not_equal(left, right);
            }
        }

        public void CompareInternal(token_taginfo left, token_taginfo right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (string.Compare(left.text, right.text, true) != 0)
                    throw_not_equal(left, right);
            }
        }

        public void CompareInternal(try_handler left, try_handler right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.GetType() != right.GetType())
                    throw_not_equal(left, right);
                if (left is try_handler_except)
                    CompareInternal(left as try_handler_except, right as try_handler_except);
                else if (left is try_handler_finally)
                    CompareInternal(left as try_handler_finally, right as try_handler_finally);
            }
        }

        public void CompareInternal(try_handler_except left, try_handler_except right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.except_block, right.except_block);
            }
        }

        public void CompareInternal(try_handler_finally left, try_handler_finally right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.stmt_list, right.stmt_list);
            }
        }

        public void CompareInternal(try_stmt left, try_stmt right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.stmt_list, right.stmt_list);
                CompareInternal(left.handler, right.handler);
            }
        }

        public void CompareInternal(type_declaration left, type_declaration right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.attributes, right.attributes);
                CompareInternal(left.type_name, right.type_name);
                CompareInternal(left.type_def, right.type_def);
            }
        }

        public void CompareInternal(type_declarations left, type_declarations right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.attributes, right.attributes);
                if (left.types_decl.Count != right.types_decl.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.types_decl.Count; i++)
                    CompareInternal(left.types_decl[i], right.types_decl[i]);
            }
        }

        public void CompareInternal(type_definition left, type_definition right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.GetType() != right.GetType())
                    throw_not_equal(left, right);
                /*Type t = left.GetType();
                MethodInfo mi = typeof(SyntaxTreeComparer).GetMethod("CompareInternal", new Type[] { t, t });
                mi.Invoke(this, new object[] { left, right });*/
                if (left is array_type)
                    CompareInternal(left as array_type, right as array_type);
                else if (left is array_of_const_type_definition)
                    CompareInternal(left as array_of_const_type_definition, right as array_of_const_type_definition);
                else if (left is array_of_named_type_definition)
                    CompareInternal(left as array_of_named_type_definition, right as array_of_named_type_definition);
                else if (left is class_definition)
                    CompareInternal(left as class_definition, right as class_definition);
                else if (left is diapason)
                    CompareInternal(left as diapason, right as diapason);
                else if (left is enum_type_definition)
                    CompareInternal(left as enum_type_definition, right as enum_type_definition);
                else if (left is file_type)
                    CompareInternal(left as file_type, right as file_type);
                else if (left is file_type_definition)
                    CompareInternal(left as file_type_definition, right as file_type_definition);
                else if (left is procedure_header)
                    CompareInternal(left as procedure_header, right as procedure_header);
                else if (left is named_type_reference)
                    CompareInternal(left as named_type_reference, right as named_type_reference);
                else if (left is ref_type)
                    CompareInternal(left as ref_type, right as ref_type);
                else if (left is set_type_definition)
                    CompareInternal(left as set_type_definition, right as set_type_definition);
                else if (left is string_num_definition)
                    CompareInternal(left as string_num_definition, right as string_num_definition);
                else if (left is template_type_reference)
                    CompareInternal(left as template_type_reference, right as template_type_reference);
                else if (left is declaration_specificator)
                    CompareInternal(left as declaration_specificator, right as declaration_specificator);
                else if (left is lambda_inferred_type)
                    CompareInternal(left as lambda_inferred_type, right as lambda_inferred_type);
                else if (left is no_type_foreach)
                    CompareInternal(left as no_type_foreach, right as no_type_foreach);
                else if (left is sequence_type)
                    CompareInternal(left as sequence_type, right as sequence_type);
                else if (left is modern_proc_type)
                    CompareInternal(left as modern_proc_type, right as modern_proc_type);
                else
                    throw new NotImplementedException(left.GetType().ToString());
            }
        }

        public void CompareInternal(type_definition_attr left, type_definition_attr right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                
            }
        }

        public void CompareInternal(type_definition_attr_list left, type_definition_attr_list right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.attributes.Count != right.attributes.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.attributes.Count; i++)
                    CompareInternal(left.attributes[i], right.attributes[i]);
            }
        }

        public void CompareInternal(where_type_specificator_list left, where_type_specificator_list right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.defs.Count != right.defs.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.defs.Count; i++)
                    CompareInternal(left.defs[i], right.defs[i]);
            }
        }

        public void CompareInternal(typecast_node left, typecast_node right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.cast_op != right.cast_op)
                    throw_not_equal(left, right);
                CompareInternal(left.expr, right.expr);
                CompareInternal(left.type_def, right.type_def);
            }
        }

        public void CompareInternal(typed_const_definition left, typed_const_definition right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.attributes, right.attributes);
                CompareInternal(left.const_name, right.const_name);
                CompareInternal(left.const_type, right.const_type);
                CompareInternal(left.const_value, right.const_value);
            }
        }

        public void CompareInternal(typed_parameters left, typed_parameters right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.param_kind != right.param_kind)
                    throw_not_equal(left, right);
                CompareInternal(left.attributes, right.attributes);
                CompareInternal(left.idents, right.idents);
                CompareInternal(left.vars_type, right.vars_type);
                CompareInternal(left.inital_value, right.inital_value);
            }
        }

        public void CompareInternal(typeof_operator left, typeof_operator right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.type_name, right.type_name);
            }
        }

        public void CompareInternal(function_lambda_call left, function_lambda_call right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.parameters, right.parameters);
                CompareInternal(left.f_lambda_def, right.f_lambda_def);
            }
        }

        public void CompareInternal(uint64_const left, uint64_const right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.val != right.val)
                    throw_not_equal(left, right);
            }
        }

        public void CompareInternal(un_expr left, un_expr right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.operation_type != right.operation_type)
                    throw_not_equal(left, right);
                CompareInternal(left.subnode, right.subnode);
            }
        }

        public void CompareInternal(unit_module left, unit_module right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.unit_name, right.unit_name);
                CompareInternal(left.interface_part, right.interface_part);
                CompareInternal(left.implementation_part, right.implementation_part);
                CompareInternal(left.initialization_part, right.initialization_part);
                CompareInternal(left.finalization_part, right.finalization_part);
            }
        }

        public void CompareInternal(unit_name left, unit_name right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
                CompareInternal(left.idunit_name, right.idunit_name);
        }

        private void CompareInternal(unit_or_namespace left, unit_or_namespace right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.GetType() != right.GetType())
                    throw_not_equal(left, right);
                if (left is uses_unit_in)
                    CompareInternal(left as uses_unit_in, right as uses_unit_in);
                else
                    CompareInternal(left.name, right.name);
            }
        }

        public void CompareInternal(uses_list left, uses_list right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.units.Count != right.units.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.units.Count; i++)
                {
                    CompareInternal(left.units[i], right.units[i]);
                }
            }
        }

        public void CompareInternal(uses_unit_in left, uses_unit_in right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.name, right.name);
                CompareInternal(left.in_file, right.in_file);
            }
        }

        public void CompareInternal(var_def_list_for_record left, var_def_list_for_record right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.vars.Count != right.vars.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.vars.Count; i++)
                    CompareInternal(left.vars[i], right.vars[i]);
            }
        }

        public void CompareInternal(var_def_statement left, var_def_statement right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.attributes, right.attributes);
                if (left.is_event != right.is_event)
                    throw_not_equal(left, right);
                if (left.var_attr != right.var_attr)
                    throw_not_equal(left, right);
                CompareInternal(left.vars, right.vars);
                CompareInternal(left.vars_type, right.vars_type);
                CompareInternal(left.inital_value, right.inital_value);
            }
        }

        public void CompareInternal(var_statement left, var_statement right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.var_def, right.var_def);
            }
        }

        public void CompareInternal(variable_definitions left, variable_definitions right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.attributes, right.attributes);
                if (left.var_definitions.Count != right.var_definitions.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.var_definitions.Count; i++)
                    CompareInternal(left.var_definitions[i], right.var_definitions[i]);
            }
        }

        public void CompareInternal(where_definition left, where_definition right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.names, right.names);
                CompareInternal(left.types, right.types);
            }
        }

        public void CompareInternal(where_definition_list left, where_definition_list right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                if (left.defs.Count != right.defs.Count)
                    throw_not_equal(left, right);
                for (int i = 0; i < left.defs.Count; i++)
                    CompareInternal(left.defs[i], right.defs[i]);
            }
        }

        public void CompareInternal(while_node left, while_node right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.expr, right.expr);
                CompareInternal(left.statements, right.statements);
            }
        }

        public void CompareInternal(with_statement left, with_statement right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.do_with, right.do_with);
                CompareInternal(left.what_do, right.what_do);
            }
        }

        public void CompareInternal(write_accessor_name left, write_accessor_name right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.accessor_name, right.accessor_name);
            }
        }

        public void CompareInternal(sequence_type left, sequence_type right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.elements_type, right.elements_type);
            }
        }

        public void CompareInternal(modern_proc_type left, modern_proc_type right)
        {
            if (left == null && right != null || left != null && right == null)
                throw_not_equal(left, right);
            if (left != null && right != null)
            {
                CompareInternal(left.el, right.el);
            }
        }
    }
}