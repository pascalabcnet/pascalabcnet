// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.TreeConverter
{

    public class CompilerDirectivesToSyntaxTreeNodesLinker
    {
        public static Dictionary<syntax_tree_node, compiler_directive> BuildLinks(SyntaxTree.compilation_unit unit, List<Errors.Error> ErrorsList)
        {
            Dictionary<syntax_tree_node, compiler_directive> links = new Dictionary<syntax_tree_node, compiler_directive>();

            SyntaxTreeNodeFinder finder = new SyntaxTreeNodeFinder();

            if (unit.compiler_directives != null)
                foreach (compiler_directive cd in unit.compiler_directives)
                    if (IsKnownDirectivee(cd))
                    {
                        syntax_tree_node sn = finder.Find(unit, cd.source_context);
                        if (sn != null)
                            if (!links.ContainsKey(sn))
                                links.Add(sn, cd);
                            else
                                ErrorsList.Add(new PascalABCCompiler.Errors.CommonCompilerError("Повторное объявление директивы", unit.file_name, sn.source_context.begin_position.line_num, sn.source_context.begin_position.column_num));
                    }
            return links;
        }

        private static bool IsKnownDirectivee(compiler_directive cd)
        {
            return cd.Name.text.ToLower() == "omp";
        }
    }


    class SyntaxTreeNodeFinder : WalkingVisitorNew
    {
        SourceContext _findContext = null;
        syntax_tree_node _findResult = null;

        public syntax_tree_node Find(SyntaxTree.compilation_unit unit, SourceContext findContext)
        {
            _findContext = findContext;
            _findResult = null;
            //unit.visit(this);
            ProcessNode(unit);

            return _findResult;
        }

        private bool FindContextIn(syntax_tree_node node)
        {
            return _findContext.In(node.source_context);
        }

        private bool FindContextBetween(syntax_tree_node node1, syntax_tree_node node2)
        {
            return _findContext.Between(node1.source_context, node2.source_context);
        }

        //второй узел начинается после конца первого
        private bool ContextInRightOrder(SourceContext sc1, SourceContext sc2)
        {
            return (sc1.end_position.line_num<sc2.begin_position.line_num)
                    ||((sc1.end_position.line_num == sc2.begin_position.line_num)
                       &&(sc1.end_position.column_num <=sc2.begin_position.column_num));
        }
        //второй узел начинается после начала первого
        private bool ContextStartsInRightOrder(SourceContext sc1, SourceContext sc2)
        {
            return (sc1.begin_position.line_num < sc2.begin_position.line_num)
                    || ((sc1.begin_position.line_num == sc2.begin_position.line_num)
                       && (sc1.begin_position.column_num <= sc2.begin_position.column_num));
        }

        public override void ProcessNode(syntax_tree_node Node)
        {
            if (Node == null)
                return;
            if (Node.source_context == null)
                return;
            if (FindContextIn(Node))
                Node.visit(this);
            else
            {
                if (ContextInRightOrder(_findContext, Node.source_context))
                {
                    if (_findResult == null)
                        _findResult = Node;
                    else
                        if (ContextStartsInRightOrder(Node.source_context, _findResult.source_context))
                            _findResult = Node;
                }
            }
        }

        //public override void visit(program_module _program_module)
        //{
        //    if (FindContextIn(_program_module.program_block))
        //        _program_module.program_block.visit(this);
        //}

        //public override void visit(block _block)
        //{
        //    if (FindContextIn(_block.program_code))
        //        _block.program_code.visit(this);
        //    else if (FindContextIn(_block.defs))
        //        _block.defs.visit(this);          
        //}

        //public override void visit(statement_list _statement_list)
        //{
        //    for (int i = 0; i < _statement_list.subnodes.Count; i++)
        //        if (FindContextIn(_statement_list.subnodes[i]))
        //        {
        //            _statement_list.subnodes[i].visit(this);
        //            return;
        //        }
        //        else
        //        {
        //            if (i == 0)
        //            {
        //                if (FindContextBetween(_statement_list.left_logical_bracket, _statement_list.subnodes[0]))
        //                {
        //                    _findResult = _statement_list.subnodes[0];
        //                    return;
        //                }
        //            }
        //            else if (FindContextBetween(_statement_list.subnodes[i - 1], _statement_list.subnodes[i]))
        //            {
        //                _findResult = _statement_list.subnodes[i];
        //                return;
        //            }

        //        }
        //}

        //public override void visit(declarations _declarations)
        //{
        //    foreach(declaration dcl in _declarations.defs)
        //        if (FindContextIn(dcl))
        //        {
        //            dcl.visit(this);
        //            return;
        //        }
        //}

        //public override void visit(procedure_definition _procedure_definition)
        //{
        //    if (FindContextIn(_procedure_definition.proc_body))
        //        _procedure_definition.proc_body.visit(this);
        //}

        //public override void visit(type_declarations _type_declarations)
        //{
        //    foreach (type_declaration dcl in _type_declarations.types_decl)
        //        if (FindContextIn(dcl))
        //        {
        //            dcl.visit(this);
        //            return;
        //        }
        //}

        //public override void visit(type_declaration _type_declaration)
        //{
        //    if (FindContextIn(_type_declaration.type_def))
        //        _type_declaration.type_def.visit(this);
        //}

        //public override void visit(class_definition _class_definition)
        //{
        //    if (FindContextIn(_class_definition.body))
        //        _class_definition.body.visit(this);
        //}

        //public override void visit(class_body _class_body)
        //{
        //    foreach (class_members clm in _class_body.class_def_blocks)
        //        if (FindContextIn(clm))
        //        {
        //            clm.visit(this);
        //            return;
        //        }            
        //}

        //public override void visit(for_node _for_node)
        //{
        //    _for_node.statements.visit(this);
        //    if (FindContextIn(_for_node) && !FindContextIn(_for_node.statements))
        //        _findResult = _for_node.statements;
        //}

        //public override void visit(class_members _class_members)
        //{
        //    foreach (declaration dcl in _class_members.members)
        //        if (FindContextIn(dcl))
        //        {
        //            dcl.visit(this);
        //            return;
        //        }

        //}
    }
}
