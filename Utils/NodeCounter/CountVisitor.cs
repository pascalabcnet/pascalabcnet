// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PascalABCCompiler.SyntaxTree
{
    public class CountVisitor : AbstractVisitor
    {
        public Dictionary<string, int> nodeCount = new Dictionary<string, int>();

        public override void visit(SyntaxTree.syntax_tree_node node)
        {
            if (nodeCount.ContainsKey("syntax_tree_node") == false)
                nodeCount.Add("syntax_tree_node", 0);
            ++nodeCount["syntax_tree_node"];
        }

        public override void visit(SyntaxTree.expression node)
        {
            if (nodeCount.ContainsKey("expression") == false)
                nodeCount.Add("expression", 0);
            ++nodeCount["expression"];
        }

        public override void visit(SyntaxTree.statement node)
        {
            if (nodeCount.ContainsKey("statement") == false)
                nodeCount.Add("statement", 0);
            ++nodeCount["statement"];
        }

        public override void visit(SyntaxTree.statement_list node)
        {
            if (nodeCount.ContainsKey("statement_list") == false)
                nodeCount.Add("statement_list", 0);
            ++nodeCount["statement_list"];
        }

        public override void visit(SyntaxTree.ident node)
        {
            if (nodeCount.ContainsKey("ident") == false)
                nodeCount.Add("ident", 0);
            ++nodeCount["ident"];
        }

        public override void visit(SyntaxTree.assign node)
        {
            if (nodeCount.ContainsKey("assign") == false)
                nodeCount.Add("assign", 0);
            ++nodeCount["assign"];
        }

        public override void visit(SyntaxTree.bin_expr node)
        {
            if (nodeCount.ContainsKey("bin_expr") == false)
                nodeCount.Add("bin_expr", 0);
            ++nodeCount["bin_expr"];
        }

        public override void visit(SyntaxTree.un_expr node)
        {
            if (nodeCount.ContainsKey("un_expr") == false)
                nodeCount.Add("un_expr", 0);
            ++nodeCount["un_expr"];
        }

        public override void visit(SyntaxTree.const_node node)
        {
            if (nodeCount.ContainsKey("const_node") == false)
                nodeCount.Add("const_node", 0);
            ++nodeCount["const_node"];
        }

        public override void visit(SyntaxTree.bool_const node)
        {
            if (nodeCount.ContainsKey("bool_const") == false)
                nodeCount.Add("bool_const", 0);
            ++nodeCount["bool_const"];
        }

        public override void visit(SyntaxTree.int32_const node)
        {
            if (nodeCount.ContainsKey("int32_const") == false)
                nodeCount.Add("int32_const", 0);
            ++nodeCount["int32_const"];
        }

        public override void visit(SyntaxTree.double_const node)
        {
            if (nodeCount.ContainsKey("double_const") == false)
                nodeCount.Add("double_const", 0);
            ++nodeCount["double_const"];
        }

        public override void visit(SyntaxTree.subprogram_body node)
        {
            if (nodeCount.ContainsKey("subprogram_body") == false)
                nodeCount.Add("subprogram_body", 0);
            ++nodeCount["subprogram_body"];
        }

        public override void visit(SyntaxTree.addressed_value node)
        {
            if (nodeCount.ContainsKey("addressed_value") == false)
                nodeCount.Add("addressed_value", 0);
            ++nodeCount["addressed_value"];
        }

        public override void visit(SyntaxTree.type_definition node)
        {
            if (nodeCount.ContainsKey("type_definition") == false)
                nodeCount.Add("type_definition", 0);
            ++nodeCount["type_definition"];
        }

        public override void visit(SyntaxTree.roof_dereference node)
        {
            if (nodeCount.ContainsKey("roof_dereference") == false)
                nodeCount.Add("roof_dereference", 0);
            ++nodeCount["roof_dereference"];
        }

        public override void visit(SyntaxTree.named_type_reference node)
        {
            if (nodeCount.ContainsKey("named_type_reference") == false)
                nodeCount.Add("named_type_reference", 0);
            ++nodeCount["named_type_reference"];
        }

        public override void visit(SyntaxTree.variable_definitions node)
        {
            if (nodeCount.ContainsKey("variable_definitions") == false)
                nodeCount.Add("variable_definitions", 0);
            ++nodeCount["variable_definitions"];
        }

        public override void visit(SyntaxTree.ident_list node)
        {
            if (nodeCount.ContainsKey("ident_list") == false)
                nodeCount.Add("ident_list", 0);
            ++nodeCount["ident_list"];
        }

        public override void visit(SyntaxTree.var_def_statement node)
        {
            if (nodeCount.ContainsKey("var_def_statement") == false)
                nodeCount.Add("var_def_statement", 0);
            ++nodeCount["var_def_statement"];
        }

        public override void visit(SyntaxTree.declaration node)
        {
            if (nodeCount.ContainsKey("declaration") == false)
                nodeCount.Add("declaration", 0);
            ++nodeCount["declaration"];
        }

        public override void visit(SyntaxTree.declarations node)
        {
            if (nodeCount.ContainsKey("declarations") == false)
                nodeCount.Add("declarations", 0);
            ++nodeCount["declarations"];
        }

        public override void visit(SyntaxTree.program_tree node)
        {
            if (nodeCount.ContainsKey("program_tree") == false)
                nodeCount.Add("program_tree", 0);
            ++nodeCount["program_tree"];
        }

        public override void visit(SyntaxTree.program_name node)
        {
            if (nodeCount.ContainsKey("program_name") == false)
                nodeCount.Add("program_name", 0);
            ++nodeCount["program_name"];
        }

        public override void visit(SyntaxTree.string_const node)
        {
            if (nodeCount.ContainsKey("string_const") == false)
                nodeCount.Add("string_const", 0);
            ++nodeCount["string_const"];
        }

        public override void visit(SyntaxTree.expression_list node)
        {
            if (nodeCount.ContainsKey("expression_list") == false)
                nodeCount.Add("expression_list", 0);
            ++nodeCount["expression_list"];
        }

        public override void visit(SyntaxTree.dereference node)
        {
            if (nodeCount.ContainsKey("dereference") == false)
                nodeCount.Add("dereference", 0);
            ++nodeCount["dereference"];
        }

        public override void visit(SyntaxTree.indexer node)
        {
            if (nodeCount.ContainsKey("indexer") == false)
                nodeCount.Add("indexer", 0);
            ++nodeCount["indexer"];
        }

        public override void visit(SyntaxTree.for_node node)
        {
            if (nodeCount.ContainsKey("for_node") == false)
                nodeCount.Add("for_node", 0);
            ++nodeCount["for_node"];
        }

        public override void visit(SyntaxTree.repeat_node node)
        {
            if (nodeCount.ContainsKey("repeat_node") == false)
                nodeCount.Add("repeat_node", 0);
            ++nodeCount["repeat_node"];
        }

        public override void visit(SyntaxTree.while_node node)
        {
            if (nodeCount.ContainsKey("while_node") == false)
                nodeCount.Add("while_node", 0);
            ++nodeCount["while_node"];
        }

        public override void visit(SyntaxTree.if_node node)
        {
            if (nodeCount.ContainsKey("if_node") == false)
                nodeCount.Add("if_node", 0);
            ++nodeCount["if_node"];
        }

        public override void visit(SyntaxTree.ref_type node)
        {
            if (nodeCount.ContainsKey("ref_type") == false)
                nodeCount.Add("ref_type", 0);
            ++nodeCount["ref_type"];
        }

        public override void visit(SyntaxTree.diapason node)
        {
            if (nodeCount.ContainsKey("diapason") == false)
                nodeCount.Add("diapason", 0);
            ++nodeCount["diapason"];
        }

        public override void visit(SyntaxTree.indexers_types node)
        {
            if (nodeCount.ContainsKey("indexers_types") == false)
                nodeCount.Add("indexers_types", 0);
            ++nodeCount["indexers_types"];
        }

        public override void visit(SyntaxTree.array_type node)
        {
            if (nodeCount.ContainsKey("array_type") == false)
                nodeCount.Add("array_type", 0);
            ++nodeCount["array_type"];
        }

        public override void visit(SyntaxTree.label_definitions node)
        {
            if (nodeCount.ContainsKey("label_definitions") == false)
                nodeCount.Add("label_definitions", 0);
            ++nodeCount["label_definitions"];
        }

        public override void visit(SyntaxTree.procedure_attribute node)
        {
            if (nodeCount.ContainsKey("procedure_attribute") == false)
                nodeCount.Add("procedure_attribute", 0);
            ++nodeCount["procedure_attribute"];
        }

        public override void visit(SyntaxTree.typed_parameters node)
        {
            if (nodeCount.ContainsKey("typed_parameters") == false)
                nodeCount.Add("typed_parameters", 0);
            ++nodeCount["typed_parameters"];
        }

        public override void visit(SyntaxTree.formal_parameters node)
        {
            if (nodeCount.ContainsKey("formal_parameters") == false)
                nodeCount.Add("formal_parameters", 0);
            ++nodeCount["formal_parameters"];
        }

        public override void visit(SyntaxTree.procedure_attributes_list node)
        {
            if (nodeCount.ContainsKey("procedure_attributes_list") == false)
                nodeCount.Add("procedure_attributes_list", 0);
            ++nodeCount["procedure_attributes_list"];
        }

        public override void visit(SyntaxTree.procedure_header node)
        {
            if (nodeCount.ContainsKey("procedure_header") == false)
                nodeCount.Add("procedure_header", 0);
            ++nodeCount["procedure_header"];
        }

        public override void visit(SyntaxTree.function_header node)
        {
            if (nodeCount.ContainsKey("function_header") == false)
                nodeCount.Add("function_header", 0);
            ++nodeCount["function_header"];
        }

        public override void visit(SyntaxTree.procedure_definition node)
        {
            if (nodeCount.ContainsKey("procedure_definition") == false)
                nodeCount.Add("procedure_definition", 0);
            ++nodeCount["procedure_definition"];
        }

        public override void visit(SyntaxTree.type_declaration node)
        {
            if (nodeCount.ContainsKey("type_declaration") == false)
                nodeCount.Add("type_declaration", 0);
            ++nodeCount["type_declaration"];
        }

        public override void visit(SyntaxTree.type_declarations node)
        {
            if (nodeCount.ContainsKey("type_declarations") == false)
                nodeCount.Add("type_declarations", 0);
            ++nodeCount["type_declarations"];
        }

        public override void visit(SyntaxTree.simple_const_definition node)
        {
            if (nodeCount.ContainsKey("simple_const_definition") == false)
                nodeCount.Add("simple_const_definition", 0);
            ++nodeCount["simple_const_definition"];
        }

        public override void visit(SyntaxTree.typed_const_definition node)
        {
            if (nodeCount.ContainsKey("typed_const_definition") == false)
                nodeCount.Add("typed_const_definition", 0);
            ++nodeCount["typed_const_definition"];
        }

        public override void visit(SyntaxTree.const_definition node)
        {
            if (nodeCount.ContainsKey("const_definition") == false)
                nodeCount.Add("const_definition", 0);
            ++nodeCount["const_definition"];
        }

        public override void visit(SyntaxTree.consts_definitions_list node)
        {
            if (nodeCount.ContainsKey("consts_definitions_list") == false)
                nodeCount.Add("consts_definitions_list", 0);
            ++nodeCount["consts_definitions_list"];
        }

        public override void visit(SyntaxTree.unit_name node)
        {
            if (nodeCount.ContainsKey("unit_name") == false)
                nodeCount.Add("unit_name", 0);
            ++nodeCount["unit_name"];
        }

        public override void visit(SyntaxTree.unit_or_namespace node)
        {
            if (nodeCount.ContainsKey("unit_or_namespace") == false)
                nodeCount.Add("unit_or_namespace", 0);
            ++nodeCount["unit_or_namespace"];
        }

        public override void visit(SyntaxTree.uses_unit_in node)
        {
            if (nodeCount.ContainsKey("uses_unit_in") == false)
                nodeCount.Add("uses_unit_in", 0);
            ++nodeCount["uses_unit_in"];
        }

        public override void visit(SyntaxTree.uses_list node)
        {
            if (nodeCount.ContainsKey("uses_list") == false)
                nodeCount.Add("uses_list", 0);
            ++nodeCount["uses_list"];
        }

        public override void visit(SyntaxTree.program_body node)
        {
            if (nodeCount.ContainsKey("program_body") == false)
                nodeCount.Add("program_body", 0);
            ++nodeCount["program_body"];
        }

        public override void visit(SyntaxTree.compilation_unit node)
        {
            if (nodeCount.ContainsKey("compilation_unit") == false)
                nodeCount.Add("compilation_unit", 0);
            ++nodeCount["compilation_unit"];
        }

        public override void visit(SyntaxTree.unit_module node)
        {
            if (nodeCount.ContainsKey("unit_module") == false)
                nodeCount.Add("unit_module", 0);
            ++nodeCount["unit_module"];
        }

        public override void visit(SyntaxTree.program_module node)
        {
            if (nodeCount.ContainsKey("program_module") == false)
                nodeCount.Add("program_module", 0);
            ++nodeCount["program_module"];
        }

        public override void visit(SyntaxTree.hex_constant node)
        {
            if (nodeCount.ContainsKey("hex_constant") == false)
                nodeCount.Add("hex_constant", 0);
            ++nodeCount["hex_constant"];
        }

        public override void visit(SyntaxTree.get_address node)
        {
            if (nodeCount.ContainsKey("get_address") == false)
                nodeCount.Add("get_address", 0);
            ++nodeCount["get_address"];
        }

        public override void visit(SyntaxTree.case_variant node)
        {
            if (nodeCount.ContainsKey("case_variant") == false)
                nodeCount.Add("case_variant", 0);
            ++nodeCount["case_variant"];
        }

        public override void visit(SyntaxTree.case_node node)
        {
            if (nodeCount.ContainsKey("case_node") == false)
                nodeCount.Add("case_node", 0);
            ++nodeCount["case_node"];
        }

        public override void visit(SyntaxTree.method_name node)
        {
            if (nodeCount.ContainsKey("method_name") == false)
                nodeCount.Add("method_name", 0);
            ++nodeCount["method_name"];
        }

        public override void visit(SyntaxTree.dot_node node)
        {
            if (nodeCount.ContainsKey("dot_node") == false)
                nodeCount.Add("dot_node", 0);
            ++nodeCount["dot_node"];
        }

        public override void visit(SyntaxTree.empty_statement node)
        {
            if (nodeCount.ContainsKey("empty_statement") == false)
                nodeCount.Add("empty_statement", 0);
            ++nodeCount["empty_statement"];
        }

        public override void visit(SyntaxTree.goto_statement node)
        {
            if (nodeCount.ContainsKey("goto_statement") == false)
                nodeCount.Add("goto_statement", 0);
            ++nodeCount["goto_statement"];
        }

        public override void visit(SyntaxTree.labeled_statement node)
        {
            if (nodeCount.ContainsKey("labeled_statement") == false)
                nodeCount.Add("labeled_statement", 0);
            ++nodeCount["labeled_statement"];
        }

        public override void visit(SyntaxTree.with_statement node)
        {
            if (nodeCount.ContainsKey("with_statement") == false)
                nodeCount.Add("with_statement", 0);
            ++nodeCount["with_statement"];
        }

        public override void visit(SyntaxTree.method_call node)
        {
            if (nodeCount.ContainsKey("method_call") == false)
                nodeCount.Add("method_call", 0);
            ++nodeCount["method_call"];
        }

        public override void visit(SyntaxTree.pascal_set_constant node)
        {
            if (nodeCount.ContainsKey("pascal_set_constant") == false)
                nodeCount.Add("pascal_set_constant", 0);
            ++nodeCount["pascal_set_constant"];
        }

        public override void visit(SyntaxTree.array_const node)
        {
            if (nodeCount.ContainsKey("array_const") == false)
                nodeCount.Add("array_const", 0);
            ++nodeCount["array_const"];
        }

        public override void visit(SyntaxTree.write_accessor_name node)
        {
            if (nodeCount.ContainsKey("write_accessor_name") == false)
                nodeCount.Add("write_accessor_name", 0);
            ++nodeCount["write_accessor_name"];
        }

        public override void visit(SyntaxTree.read_accessor_name node)
        {
            if (nodeCount.ContainsKey("read_accessor_name") == false)
                nodeCount.Add("read_accessor_name", 0);
            ++nodeCount["read_accessor_name"];
        }

        public override void visit(SyntaxTree.property_accessors node)
        {
            if (nodeCount.ContainsKey("property_accessors") == false)
                nodeCount.Add("property_accessors", 0);
            ++nodeCount["property_accessors"];
        }

        public override void visit(SyntaxTree.simple_property node)
        {
            if (nodeCount.ContainsKey("simple_property") == false)
                nodeCount.Add("simple_property", 0);
            ++nodeCount["simple_property"];
        }

        public override void visit(SyntaxTree.index_property node)
        {
            if (nodeCount.ContainsKey("index_property") == false)
                nodeCount.Add("index_property", 0);
            ++nodeCount["index_property"];
        }

        public override void visit(SyntaxTree.class_members node)
        {
            if (nodeCount.ContainsKey("class_members") == false)
                nodeCount.Add("class_members", 0);
            ++nodeCount["class_members"];
        }

        public override void visit(SyntaxTree.access_modifer_node node)
        {
            if (nodeCount.ContainsKey("access_modifer_node") == false)
                nodeCount.Add("access_modifer_node", 0);
            ++nodeCount["access_modifer_node"];
        }

        public override void visit(SyntaxTree.class_body node)
        {
            if (nodeCount.ContainsKey("class_body") == false)
                nodeCount.Add("class_body", 0);
            ++nodeCount["class_body"];
        }

        public override void visit(SyntaxTree.class_definition node)
        {
            if (nodeCount.ContainsKey("class_definition") == false)
                nodeCount.Add("class_definition", 0);
            ++nodeCount["class_definition"];
        }

        public override void visit(SyntaxTree.default_indexer_property_node node)
        {
            if (nodeCount.ContainsKey("default_indexer_property_node") == false)
                nodeCount.Add("default_indexer_property_node", 0);
            ++nodeCount["default_indexer_property_node"];
        }

        public override void visit(SyntaxTree.known_type_definition node)
        {
            if (nodeCount.ContainsKey("known_type_definition") == false)
                nodeCount.Add("known_type_definition", 0);
            ++nodeCount["known_type_definition"];
        }

        public override void visit(SyntaxTree.set_type_definition node)
        {
            if (nodeCount.ContainsKey("set_type_definition") == false)
                nodeCount.Add("set_type_definition", 0);
            ++nodeCount["set_type_definition"];
        }

        public override void visit(SyntaxTree.try_statement node)
        {
            if (nodeCount.ContainsKey("try_statement") == false)
                nodeCount.Add("try_statement", 0);
            ++nodeCount["try_statement"];
        }

        public override void visit(SyntaxTree.on_exception node)
        {
            if (nodeCount.ContainsKey("on_exception") == false)
                nodeCount.Add("on_exception", 0);
            ++nodeCount["on_exception"];
        }

        public override void visit(SyntaxTree.on_exception_list node)
        {
            if (nodeCount.ContainsKey("on_exception_list") == false)
                nodeCount.Add("on_exception_list", 0);
            ++nodeCount["on_exception_list"];
        }

        public override void visit(SyntaxTree.try_finally_statement node)
        {
            if (nodeCount.ContainsKey("try_finally_statement") == false)
                nodeCount.Add("try_finally_statement", 0);
            ++nodeCount["try_finally_statement"];
        }

        public override void visit(SyntaxTree.try_except_statement node)
        {
            if (nodeCount.ContainsKey("try_except_statement") == false)
                nodeCount.Add("try_except_statement", 0);
            ++nodeCount["try_except_statement"];
        }

        public override void visit(SyntaxTree.record_const_definition node)
        {
            if (nodeCount.ContainsKey("record_const_definition") == false)
                nodeCount.Add("record_const_definition", 0);
            ++nodeCount["record_const_definition"];
        }

        public override void visit(SyntaxTree.record_const node)
        {
            if (nodeCount.ContainsKey("record_const") == false)
                nodeCount.Add("record_const", 0);
            ++nodeCount["record_const"];
        }

        public override void visit(SyntaxTree.record_type node)
        {
            if (nodeCount.ContainsKey("record_type") == false)
                nodeCount.Add("record_type", 0);
            ++nodeCount["record_type"];
        }

        public override void visit(SyntaxTree.enum_type_definition node)
        {
            if (nodeCount.ContainsKey("enum_type_definition") == false)
                nodeCount.Add("enum_type_definition", 0);
            ++nodeCount["enum_type_definition"];
        }

        public override void visit(SyntaxTree.char_const node)
        {
            if (nodeCount.ContainsKey("char_const") == false)
                nodeCount.Add("char_const", 0);
            ++nodeCount["char_const"];
        }

        public override void visit(SyntaxTree.raise_statement node)
        {
            if (nodeCount.ContainsKey("raise_statement") == false)
                nodeCount.Add("raise_statement", 0);
            ++nodeCount["raise_statement"];
        }

        public override void visit(SyntaxTree.sharp_char_const node)
        {
            if (nodeCount.ContainsKey("sharp_char_const") == false)
                nodeCount.Add("sharp_char_const", 0);
            ++nodeCount["sharp_char_const"];
        }

        public override void visit(SyntaxTree.literal_const_line node)
        {
            if (nodeCount.ContainsKey("literal_const_line") == false)
                nodeCount.Add("literal_const_line", 0);
            ++nodeCount["literal_const_line"];
        }

        public override void visit(SyntaxTree.string_num_definition node)
        {
            if (nodeCount.ContainsKey("string_num_definition") == false)
                nodeCount.Add("string_num_definition", 0);
            ++nodeCount["string_num_definition"];
        }

        public override void visit(SyntaxTree.variant node)
        {
            if (nodeCount.ContainsKey("variant") == false)
                nodeCount.Add("variant", 0);
            ++nodeCount["variant"];
        }

        public override void visit(SyntaxTree.variant_list node)
        {
            if (nodeCount.ContainsKey("variant_list") == false)
                nodeCount.Add("variant_list", 0);
            ++nodeCount["variant_list"];
        }

        public override void visit(SyntaxTree.variant_type node)
        {
            if (nodeCount.ContainsKey("variant_type") == false)
                nodeCount.Add("variant_type", 0);
            ++nodeCount["variant_type"];
        }

        public override void visit(SyntaxTree.variant_types node)
        {
            if (nodeCount.ContainsKey("variant_types") == false)
                nodeCount.Add("variant_types", 0);
            ++nodeCount["variant_types"];
        }

        public override void visit(SyntaxTree.variant_record_type node)
        {
            if (nodeCount.ContainsKey("variant_record_type") == false)
                nodeCount.Add("variant_record_type", 0);
            ++nodeCount["variant_record_type"];
        }

        public override void visit(SyntaxTree.procedure_call node)
        {
            if (nodeCount.ContainsKey("procedure_call") == false)
                nodeCount.Add("procedure_call", 0);
            ++nodeCount["procedure_call"];
        }

        public override void visit(SyntaxTree.class_predefinition node)
        {
            if (nodeCount.ContainsKey("class_predefinition") == false)
                nodeCount.Add("class_predefinition", 0);
            ++nodeCount["class_predefinition"];
        }

        public override void visit(SyntaxTree.nil_const node)
        {
            if (nodeCount.ContainsKey("nil_const") == false)
                nodeCount.Add("nil_const", 0);
            ++nodeCount["nil_const"];
        }

        public override void visit(SyntaxTree.file_type_definition node)
        {
            if (nodeCount.ContainsKey("file_type_definition") == false)
                nodeCount.Add("file_type_definition", 0);
            ++nodeCount["file_type_definition"];
        }

        public override void visit(SyntaxTree.constructor node)
        {
            if (nodeCount.ContainsKey("constructor") == false)
                nodeCount.Add("constructor", 0);
            ++nodeCount["constructor"];
        }

        public override void visit(SyntaxTree.destructor node)
        {
            if (nodeCount.ContainsKey("destructor") == false)
                nodeCount.Add("destructor", 0);
            ++nodeCount["destructor"];
        }

        public override void visit(SyntaxTree.inherited_method_call node)
        {
            if (nodeCount.ContainsKey("inherited_method_call") == false)
                nodeCount.Add("inherited_method_call", 0);
            ++nodeCount["inherited_method_call"];
        }

        public override void visit(SyntaxTree.typecast_node node)
        {
            if (nodeCount.ContainsKey("typecast_node") == false)
                nodeCount.Add("typecast_node", 0);
            ++nodeCount["typecast_node"];
        }

        public override void visit(SyntaxTree.interface_node node)
        {
            if (nodeCount.ContainsKey("interface_node") == false)
                nodeCount.Add("interface_node", 0);
            ++nodeCount["interface_node"];
        }

        public override void visit(SyntaxTree.implementation_node node)
        {
            if (nodeCount.ContainsKey("implementation_node") == false)
                nodeCount.Add("implementation_node", 0);
            ++nodeCount["implementation_node"];
        }

        public override void visit(SyntaxTree.diap_expr node)
        {
            if (nodeCount.ContainsKey("diap_expr") == false)
                nodeCount.Add("diap_expr", 0);
            ++nodeCount["diap_expr"];
        }

        public override void visit(SyntaxTree.block node)
        {
            if (nodeCount.ContainsKey("block") == false)
                nodeCount.Add("block", 0);
            ++nodeCount["block"];
        }

        public override void visit(SyntaxTree.proc_block node)
        {
            if (nodeCount.ContainsKey("proc_block") == false)
                nodeCount.Add("proc_block", 0);
            ++nodeCount["proc_block"];
        }

        public override void visit(SyntaxTree.array_of_named_type_definition node)
        {
            if (nodeCount.ContainsKey("array_of_named_type_definition") == false)
                nodeCount.Add("array_of_named_type_definition", 0);
            ++nodeCount["array_of_named_type_definition"];
        }

        public override void visit(SyntaxTree.array_of_const_type_definition node)
        {
            if (nodeCount.ContainsKey("array_of_const_type_definition") == false)
                nodeCount.Add("array_of_const_type_definition", 0);
            ++nodeCount["array_of_const_type_definition"];
        }

        public override void visit(SyntaxTree.literal node)
        {
            if (nodeCount.ContainsKey("literal") == false)
                nodeCount.Add("literal", 0);
            ++nodeCount["literal"];
        }

        public override void visit(SyntaxTree.case_variants node)
        {
            if (nodeCount.ContainsKey("case_variants") == false)
                nodeCount.Add("case_variants", 0);
            ++nodeCount["case_variants"];
        }

        public override void visit(SyntaxTree.diapason_expr node)
        {
            if (nodeCount.ContainsKey("diapason_expr") == false)
                nodeCount.Add("diapason_expr", 0);
            ++nodeCount["diapason_expr"];
        }

        public override void visit(SyntaxTree.var_def_list node)
        {
            if (nodeCount.ContainsKey("var_def_list") == false)
                nodeCount.Add("var_def_list", 0);
            ++nodeCount["var_def_list"];
        }

        public override void visit(SyntaxTree.record_type_parts node)
        {
            if (nodeCount.ContainsKey("record_type_parts") == false)
                nodeCount.Add("record_type_parts", 0);
            ++nodeCount["record_type_parts"];
        }

        public override void visit(SyntaxTree.property_array_default node)
        {
            if (nodeCount.ContainsKey("property_array_default") == false)
                nodeCount.Add("property_array_default", 0);
            ++nodeCount["property_array_default"];
        }

        public override void visit(SyntaxTree.property_interface node)
        {
            if (nodeCount.ContainsKey("property_interface") == false)
                nodeCount.Add("property_interface", 0);
            ++nodeCount["property_interface"];
        }

        public override void visit(SyntaxTree.property_parameter node)
        {
            if (nodeCount.ContainsKey("property_parameter") == false)
                nodeCount.Add("property_parameter", 0);
            ++nodeCount["property_parameter"];
        }

        public override void visit(SyntaxTree.property_parameter_list node)
        {
            if (nodeCount.ContainsKey("property_parameter_list") == false)
                nodeCount.Add("property_parameter_list", 0);
            ++nodeCount["property_parameter_list"];
        }

        public override void visit(SyntaxTree.inherited_ident node)
        {
            if (nodeCount.ContainsKey("inherited_ident") == false)
                nodeCount.Add("inherited_ident", 0);
            ++nodeCount["inherited_ident"];
        }

        public override void visit(SyntaxTree.format_expr node)
        {
            if (nodeCount.ContainsKey("format_expr") == false)
                nodeCount.Add("format_expr", 0);
            ++nodeCount["format_expr"];
        }

        public override void visit(SyntaxTree.initfinal_part node)
        {
            if (nodeCount.ContainsKey("initfinal_part") == false)
                nodeCount.Add("initfinal_part", 0);
            ++nodeCount["initfinal_part"];
        }

        public override void visit(SyntaxTree.token_info node)
        {
            if (nodeCount.ContainsKey("token_info") == false)
                nodeCount.Add("token_info", 0);
            ++nodeCount["token_info"];
        }

        public override void visit(SyntaxTree.raise_stmt node)
        {
            if (nodeCount.ContainsKey("raise_stmt") == false)
                nodeCount.Add("raise_stmt", 0);
            ++nodeCount["raise_stmt"];
        }

        public override void visit(SyntaxTree.op_type_node node)
        {
            if (nodeCount.ContainsKey("op_type_node") == false)
                nodeCount.Add("op_type_node", 0);
            ++nodeCount["op_type_node"];
        }

        public override void visit(SyntaxTree.file_type node)
        {
            if (nodeCount.ContainsKey("file_type") == false)
                nodeCount.Add("file_type", 0);
            ++nodeCount["file_type"];
        }

        public override void visit(SyntaxTree.known_type_ident node)
        {
            if (nodeCount.ContainsKey("known_type_ident") == false)
                nodeCount.Add("known_type_ident", 0);
            ++nodeCount["known_type_ident"];
        }

        public override void visit(SyntaxTree.exception_handler node)
        {
            if (nodeCount.ContainsKey("exception_handler") == false)
                nodeCount.Add("exception_handler", 0);
            ++nodeCount["exception_handler"];
        }

        public override void visit(SyntaxTree.exception_ident node)
        {
            if (nodeCount.ContainsKey("exception_ident") == false)
                nodeCount.Add("exception_ident", 0);
            ++nodeCount["exception_ident"];
        }

        public override void visit(SyntaxTree.exception_handler_list node)
        {
            if (nodeCount.ContainsKey("exception_handler_list") == false)
                nodeCount.Add("exception_handler_list", 0);
            ++nodeCount["exception_handler_list"];
        }

        public override void visit(SyntaxTree.exception_block node)
        {
            if (nodeCount.ContainsKey("exception_block") == false)
                nodeCount.Add("exception_block", 0);
            ++nodeCount["exception_block"];
        }

        public override void visit(SyntaxTree.try_handler node)
        {
            if (nodeCount.ContainsKey("try_handler") == false)
                nodeCount.Add("try_handler", 0);
            ++nodeCount["try_handler"];
        }

        public override void visit(SyntaxTree.try_handler_finally node)
        {
            if (nodeCount.ContainsKey("try_handler_finally") == false)
                nodeCount.Add("try_handler_finally", 0);
            ++nodeCount["try_handler_finally"];
        }

        public override void visit(SyntaxTree.try_handler_except node)
        {
            if (nodeCount.ContainsKey("try_handler_except") == false)
                nodeCount.Add("try_handler_except", 0);
            ++nodeCount["try_handler_except"];
        }

        public override void visit(SyntaxTree.try_stmt node)
        {
            if (nodeCount.ContainsKey("try_stmt") == false)
                nodeCount.Add("try_stmt", 0);
            ++nodeCount["try_stmt"];
        }

        public override void visit(SyntaxTree.inherited_message node)
        {
            if (nodeCount.ContainsKey("inherited_message") == false)
                nodeCount.Add("inherited_message", 0);
            ++nodeCount["inherited_message"];
        }

        public override void visit(SyntaxTree.external_directive node)
        {
            if (nodeCount.ContainsKey("external_directive") == false)
                nodeCount.Add("external_directive", 0);
            ++nodeCount["external_directive"];
        }

        public override void visit(SyntaxTree.using_list node)
        {
            if (nodeCount.ContainsKey("using_list") == false)
                nodeCount.Add("using_list", 0);
            ++nodeCount["using_list"];
        }

        public override void visit(SyntaxTree.oberon_import_module node)
        {
            if (nodeCount.ContainsKey("oberon_import_module") == false)
                nodeCount.Add("oberon_import_module", 0);
            ++nodeCount["oberon_import_module"];
        }

        public override void visit(SyntaxTree.oberon_module node)
        {
            if (nodeCount.ContainsKey("oberon_module") == false)
                nodeCount.Add("oberon_module", 0);
            ++nodeCount["oberon_module"];
        }

        public override void visit(SyntaxTree.oberon_ident_with_export_marker node)
        {
            if (nodeCount.ContainsKey("oberon_ident_with_export_marker") == false)
                nodeCount.Add("oberon_ident_with_export_marker", 0);
            ++nodeCount["oberon_ident_with_export_marker"];
        }

        public override void visit(SyntaxTree.oberon_exit_stmt node)
        {
            if (nodeCount.ContainsKey("oberon_exit_stmt") == false)
                nodeCount.Add("oberon_exit_stmt", 0);
            ++nodeCount["oberon_exit_stmt"];
        }

        public override void visit(SyntaxTree.jump_stmt node)
        {
            if (nodeCount.ContainsKey("jump_stmt") == false)
                nodeCount.Add("jump_stmt", 0);
            ++nodeCount["jump_stmt"];
        }

        public override void visit(SyntaxTree.oberon_procedure_receiver node)
        {
            if (nodeCount.ContainsKey("oberon_procedure_receiver") == false)
                nodeCount.Add("oberon_procedure_receiver", 0);
            ++nodeCount["oberon_procedure_receiver"];
        }

        public override void visit(SyntaxTree.oberon_procedure_header node)
        {
            if (nodeCount.ContainsKey("oberon_procedure_header") == false)
                nodeCount.Add("oberon_procedure_header", 0);
            ++nodeCount["oberon_procedure_header"];
        }

        public override void visit(SyntaxTree.oberon_withstmt_guardstat node)
        {
            if (nodeCount.ContainsKey("oberon_withstmt_guardstat") == false)
                nodeCount.Add("oberon_withstmt_guardstat", 0);
            ++nodeCount["oberon_withstmt_guardstat"];
        }

        public override void visit(SyntaxTree.oberon_withstmt_guardstat_list node)
        {
            if (nodeCount.ContainsKey("oberon_withstmt_guardstat_list") == false)
                nodeCount.Add("oberon_withstmt_guardstat_list", 0);
            ++nodeCount["oberon_withstmt_guardstat_list"];
        }

        public override void visit(SyntaxTree.oberon_withstmt node)
        {
            if (nodeCount.ContainsKey("oberon_withstmt") == false)
                nodeCount.Add("oberon_withstmt", 0);
            ++nodeCount["oberon_withstmt"];
        }

        public override void visit(SyntaxTree.loop_stmt node)
        {
            if (nodeCount.ContainsKey("loop_stmt") == false)
                nodeCount.Add("loop_stmt", 0);
            ++nodeCount["loop_stmt"];
        }

        public override void visit(SyntaxTree.foreach_stmt node)
        {
            if (nodeCount.ContainsKey("foreach_stmt") == false)
                nodeCount.Add("foreach_stmt", 0);
            ++nodeCount["foreach_stmt"];
        }

        public override void visit(SyntaxTree.addressed_value_funcname node)
        {
            if (nodeCount.ContainsKey("addressed_value_funcname") == false)
                nodeCount.Add("addressed_value_funcname", 0);
            ++nodeCount["addressed_value_funcname"];
        }

        public override void visit(SyntaxTree.named_type_reference_list node)
        {
            if (nodeCount.ContainsKey("named_type_reference_list") == false)
                nodeCount.Add("named_type_reference_list", 0);
            ++nodeCount["named_type_reference_list"];
        }

        public override void visit(SyntaxTree.template_param_list node)
        {
            if (nodeCount.ContainsKey("template_param_list") == false)
                nodeCount.Add("template_param_list", 0);
            ++nodeCount["template_param_list"];
        }

        public override void visit(SyntaxTree.template_type_reference node)
        {
            if (nodeCount.ContainsKey("template_type_reference") == false)
                nodeCount.Add("template_type_reference", 0);
            ++nodeCount["template_type_reference"];
        }

        public override void visit(SyntaxTree.int64_const node)
        {
            if (nodeCount.ContainsKey("int64_const") == false)
                nodeCount.Add("int64_const", 0);
            ++nodeCount["int64_const"];
        }

        public override void visit(SyntaxTree.uint64_const node)
        {
            if (nodeCount.ContainsKey("uint64_const") == false)
                nodeCount.Add("uint64_const", 0);
            ++nodeCount["uint64_const"];
        }

        public override void visit(SyntaxTree.new_expr node)
        {
            if (nodeCount.ContainsKey("new_expr") == false)
                nodeCount.Add("new_expr", 0);
            ++nodeCount["new_expr"];
        }

        public override void visit(SyntaxTree.type_definition_list node)
        {
            if (nodeCount.ContainsKey("type_definition_list") == false)
                nodeCount.Add("type_definition_list", 0);
            ++nodeCount["type_definition_list"];
        }

        public override void visit(SyntaxTree.where_definition node)
        {
            if (nodeCount.ContainsKey("where_definition") == false)
                nodeCount.Add("where_definition", 0);
            ++nodeCount["where_definition"];
        }

        public override void visit(SyntaxTree.where_definition_list node)
        {
            if (nodeCount.ContainsKey("where_definition_list") == false)
                nodeCount.Add("where_definition_list", 0);
            ++nodeCount["where_definition_list"];
        }

        public override void visit(SyntaxTree.sizeof_operator node)
        {
            if (nodeCount.ContainsKey("sizeof_operator") == false)
                nodeCount.Add("sizeof_operator", 0);
            ++nodeCount["sizeof_operator"];
        }

        public override void visit(SyntaxTree.typeof_operator node)
        {
            if (nodeCount.ContainsKey("typeof_operator") == false)
                nodeCount.Add("typeof_operator", 0);
            ++nodeCount["typeof_operator"];
        }

        public override void visit(SyntaxTree.compiler_directive node)
        {
            if (nodeCount.ContainsKey("compiler_directive") == false)
                nodeCount.Add("compiler_directive", 0);
            ++nodeCount["compiler_directive"];
        }

        public override void visit(SyntaxTree.operator_name_ident node)
        {
            if (nodeCount.ContainsKey("operator_name_ident") == false)
                nodeCount.Add("operator_name_ident", 0);
            ++nodeCount["operator_name_ident"];
        }

        public override void visit(SyntaxTree.var_statement node)
        {
            if (nodeCount.ContainsKey("var_statement") == false)
                nodeCount.Add("var_statement", 0);
            ++nodeCount["var_statement"];
        }

        public override void visit(SyntaxTree.question_colon_expression node)
        {
            if (nodeCount.ContainsKey("question_colon_expression") == false)
                nodeCount.Add("question_colon_expression", 0);
            ++nodeCount["question_colon_expression"];
        }

        public override void visit(SyntaxTree.expression_as_statement node)
        {
            if (nodeCount.ContainsKey("expression_as_statement") == false)
                nodeCount.Add("expression_as_statement", 0);
            ++nodeCount["expression_as_statement"];
        }

        public override void visit(SyntaxTree.c_scalar_type node)
        {
            if (nodeCount.ContainsKey("c_scalar_type") == false)
                nodeCount.Add("c_scalar_type", 0);
            ++nodeCount["c_scalar_type"];
        }

        public override void visit(SyntaxTree.c_module node)
        {
            if (nodeCount.ContainsKey("c_module") == false)
                nodeCount.Add("c_module", 0);
            ++nodeCount["c_module"];
        }

        public override void visit(SyntaxTree.declarations_as_statement node)
        {
            if (nodeCount.ContainsKey("declarations_as_statement") == false)
                nodeCount.Add("declarations_as_statement", 0);
            ++nodeCount["declarations_as_statement"];
        }

        public override void visit(SyntaxTree.array_size node)
        {
            if (nodeCount.ContainsKey("array_size") == false)
                nodeCount.Add("array_size", 0);
            ++nodeCount["array_size"];
        }

        public override void visit(SyntaxTree.enumerator node)
        {
            if (nodeCount.ContainsKey("enumerator") == false)
                nodeCount.Add("enumerator", 0);
            ++nodeCount["enumerator"];
        }

        public override void visit(SyntaxTree.enumerator_list node)
        {
            if (nodeCount.ContainsKey("enumerator_list") == false)
                nodeCount.Add("enumerator_list", 0);
            ++nodeCount["enumerator_list"];
        }

        public override void visit(SyntaxTree.c_for_cycle node)
        {
            if (nodeCount.ContainsKey("c_for_cycle") == false)
                nodeCount.Add("c_for_cycle", 0);
            ++nodeCount["c_for_cycle"];
        }

        public override void visit(SyntaxTree.switch_stmt node)
        {
            if (nodeCount.ContainsKey("switch_stmt") == false)
                nodeCount.Add("switch_stmt", 0);
            ++nodeCount["switch_stmt"];
        }

        public override void visit(SyntaxTree.type_definition_attr_list node)
        {
            if (nodeCount.ContainsKey("type_definition_attr_list") == false)
                nodeCount.Add("type_definition_attr_list", 0);
            ++nodeCount["type_definition_attr_list"];
        }

        public override void visit(SyntaxTree.type_definition_attr node)
        {
            if (nodeCount.ContainsKey("type_definition_attr") == false)
                nodeCount.Add("type_definition_attr", 0);
            ++nodeCount["type_definition_attr"];
        }

        public override void visit(SyntaxTree.lock_stmt node)
        {
            if (nodeCount.ContainsKey("lock_stmt") == false)
                nodeCount.Add("lock_stmt", 0);
            ++nodeCount["lock_stmt"];
        }

        public override void visit(SyntaxTree.compiler_directive_list node)
        {
            if (nodeCount.ContainsKey("compiler_directive_list") == false)
                nodeCount.Add("compiler_directive_list", 0);
            ++nodeCount["compiler_directive_list"];
        }

        public override void visit(SyntaxTree.compiler_directive_if node)
        {
            if (nodeCount.ContainsKey("compiler_directive_if") == false)
                nodeCount.Add("compiler_directive_if", 0);
            ++nodeCount["compiler_directive_if"];
        }

        public override void visit(SyntaxTree.documentation_comment_list node)
        {
            if (nodeCount.ContainsKey("documentation_comment_list") == false)
                nodeCount.Add("documentation_comment_list", 0);
            ++nodeCount["documentation_comment_list"];
        }

        public override void visit(SyntaxTree.documentation_comment_tag node)
        {
            if (nodeCount.ContainsKey("documentation_comment_tag") == false)
                nodeCount.Add("documentation_comment_tag", 0);
            ++nodeCount["documentation_comment_tag"];
        }

        public override void visit(SyntaxTree.documentation_comment_tag_param node)
        {
            if (nodeCount.ContainsKey("documentation_comment_tag_param") == false)
                nodeCount.Add("documentation_comment_tag_param", 0);
            ++nodeCount["documentation_comment_tag_param"];
        }

        public override void visit(SyntaxTree.documentation_comment_section node)
        {
            if (nodeCount.ContainsKey("documentation_comment_section") == false)
                nodeCount.Add("documentation_comment_section", 0);
            ++nodeCount["documentation_comment_section"];
        }

        public override void visit(SyntaxTree.token_taginfo node)
        {
            if (nodeCount.ContainsKey("token_taginfo") == false)
                nodeCount.Add("token_taginfo", 0);
            ++nodeCount["token_taginfo"];
        }

        public override void visit(SyntaxTree.declaration_specificator node)
        {
            if (nodeCount.ContainsKey("declaration_specificator") == false)
                nodeCount.Add("declaration_specificator", 0);
            ++nodeCount["declaration_specificator"];
        }

        public override void visit(SyntaxTree.ident_with_templateparams node)
        {
            if (nodeCount.ContainsKey("ident_with_templateparams") == false)
                nodeCount.Add("ident_with_templateparams", 0);
            ++nodeCount["ident_with_templateparams"];
        }

        public override void visit(SyntaxTree.template_type_name node)
        {
            if (nodeCount.ContainsKey("template_type_name") == false)
                nodeCount.Add("template_type_name", 0);
            ++nodeCount["template_type_name"];
        }

        public override void visit(SyntaxTree.default_operator node)
        {
            if (nodeCount.ContainsKey("default_operator") == false)
                nodeCount.Add("default_operator", 0);
            ++nodeCount["default_operator"];
        }

        public override void visit(SyntaxTree.bracket_expr node)
        {
            if (nodeCount.ContainsKey("bracket_expr") == false)
                nodeCount.Add("bracket_expr", 0);
            ++nodeCount["bracket_expr"];
        }

        public override void visit(SyntaxTree.attribute node)
        {
            if (nodeCount.ContainsKey("attribute") == false)
                nodeCount.Add("attribute", 0);
            ++nodeCount["attribute"];
        }

        public override void visit(SyntaxTree.simple_attribute_list node)
        {
            if (nodeCount.ContainsKey("simple_attribute_list") == false)
                nodeCount.Add("simple_attribute_list", 0);
            ++nodeCount["simple_attribute_list"];
        }

        public override void visit(SyntaxTree.attribute_list node)
        {
            if (nodeCount.ContainsKey("attribute_list") == false)
                nodeCount.Add("attribute_list", 0);
            ++nodeCount["attribute_list"];
        }

        public override void visit(SyntaxTree.function_lambda_definition node)
        {
            if (nodeCount.ContainsKey("function_lambda_definition") == false)
                nodeCount.Add("function_lambda_definition", 0);
            ++nodeCount["function_lambda_definition"];
        }

        public override void visit(SyntaxTree.function_lambda_call node)
        {
            if (nodeCount.ContainsKey("function_lambda_call") == false)
                nodeCount.Add("function_lambda_call", 0);
            ++nodeCount["function_lambda_call"];
        }

        public override void visit(SyntaxTree.semantic_check node)
        {
            if (nodeCount.ContainsKey("semantic_check") == false)
                nodeCount.Add("semantic_check", 0);
            ++nodeCount["semantic_check"];
        }

        public override void visit(SyntaxTree.lambda_inferred_type node)
        {
            if (nodeCount.ContainsKey("lambda_inferred_type") == false)
                nodeCount.Add("lambda_inferred_type", 0);
            ++nodeCount["lambda_inferred_type"];
        }

        public override void visit(SyntaxTree.same_type_node node)
        {
            if (nodeCount.ContainsKey("same_type_node") == false)
                nodeCount.Add("same_type_node", 0);
            ++nodeCount["same_type_node"];
        }
    }
}