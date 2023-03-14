// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.SyntaxTree;
using System.Collections;

namespace PascalABCCompiler
{
    public class DocumentationConstructor : AbstractVisitor
    {
        Dictionary<int, documentation_comment_section> line_to_section = new Dictionary<int, documentation_comment_section>();
        Dictionary<syntax_tree_node, string> documentation;
        public Dictionary<syntax_tree_node, string> Construct(syntax_tree_node tree_root, documentation_comment_list sections_list)
        {
            documentation = new Dictionary<syntax_tree_node, string>();
            if (sections_list.sections.Count == 0)
                return documentation;
            foreach (documentation_comment_section dcs in sections_list.sections)
                line_to_section.Add(dcs.source_context.end_position.line_num + 1, dcs);
            try
            {
                visit_node(tree_root);
            }
            catch
            {
            }
            //visit_node(tree_root);
            line_to_section.Clear();
            Dictionary<syntax_tree_node, string> dcn = documentation;
            documentation = null;
            return dcn;
        }
        void visit_node(syntax_tree_node node)
        {
            if (node != null)
                node.visit(this);
        }
        void visit_collection(IEnumerable colection)
        {
            if (colection != null)
                foreach (syntax_tree_node node in colection)
                    node.visit(this);
        }
        void connect_collection(IEnumerable colection)
        {
            if (colection != null)
                foreach (syntax_tree_node node in colection)
                    connect(node);
        }
        void connect(syntax_tree_node node)
        {
            if (node == null || node.source_context == null || documentation.ContainsKey(node))
                return;
            if (line_to_section.ContainsKey(node.source_context.begin_position.line_num) )
            {
                documentation_comment_section dsc = line_to_section[node.source_context.begin_position.line_num];
                if (dsc.tags.Count == 0)
                    if (dsc.text != null)
                    {
                        documentation.Add(node, dsc.text.Trim());
                        return;
                    }
                foreach (documentation_comment_tag dt in dsc.tags)
                    if (dt.name.ToLower() == "summary")
                        if (dt.text != null)
                            documentation.Add(node, dt.text.Trim());
            }
            
        }
        public override void visit(program_module _program_module)
        {
            _program_module.program_block.visit(this);
        }
        public override void visit(unit_module _unit_module)
        {
            connect(_unit_module.unit_name);
            visit_node(_unit_module.interface_part);
            visit_node(_unit_module.implementation_part);
        }
        public override void visit(interface_node _interface_node)
        {
            visit_node(_interface_node.interface_definitions);
        }
        public override void visit(implementation_node _implementation_node)
        {
            visit_node(_implementation_node.implementation_definitions);
        }
        public override void visit(block _block)
        {
            visit_node(_block.defs);
        }
        public override void visit(declarations _declarations)
        {
            visit_collection(_declarations.defs);
        }
        public override void visit(procedure_definition _procedure_definition)
        {
            visit_node(_procedure_definition.proc_header);
            if (_procedure_definition.proc_body is block)
            visit_node(_procedure_definition.proc_body as block);
        }
        public override void visit(procedure_header _procedure_header)
        {
            connect(_procedure_header);
        }
        public override void visit(function_header _function_header)
        {
            connect(_function_header);
        }
        public override void visit(class_definition _class_definition)
        {
            connect(_class_definition);
            visit_node(_class_definition.body);
        }
        public override void visit(class_body_list _class_body)
        {
            visit_collection(_class_body.class_def_blocks);
        }
        public override void visit(class_members _class_members)
        {
            visit_collection(_class_members.members);
        }
        public override void visit(var_def_statement _var_def_statement)
        {
            connect(_var_def_statement);
        }
        public override void visit(type_declarations _type_declarations)
        {
            visit_collection(_type_declarations.types_decl);
        }
        public override void visit(type_declaration _type_declaration)
        {
            connect(_type_declaration);
            visit_node(_type_declaration.type_def);
        }
        public override void visit(consts_definitions_list _consts_definitions_list)
        {
            visit_collection(_consts_definitions_list.const_defs);
        }
        public override void visit(const_definition _const_definition)
        {
            connect(_const_definition);
        }
        public override void visit(variable_definitions _variable_definitions)
        {
            visit_collection(_variable_definitions.var_definitions);
        }
        public override void visit(simple_const_definition _const_definition)
        {
            connect(_const_definition);
        }
        public override void visit(typed_const_definition _const_definition)
        {
            connect(_const_definition);
        }
        public override void visit(simple_property _simple_property)
        {
        	connect(_simple_property);
        }
        public override void visit(constructor _constructor)
        {
        	connect(_constructor);
        }
        public override void visit(destructor _destructor)
        {
        	connect(_destructor);
        }
        public override void visit(enum_type_definition _enum_type_definition)
        {
            visit_collection(_enum_type_definition.enumerators.enumerators);
        }
        public override void visit(enumerator _enumerator)
        {
            connect(_enumerator);
        }
    }
}
