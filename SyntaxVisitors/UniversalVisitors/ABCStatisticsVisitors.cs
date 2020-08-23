// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{

    public class ABCStatisticsVisitor : WalkingVisitorNew  
    {
        public int InBlockVarDefs { get; set; }
        public int OutBlockVarDefs { get; set; }
        public int ForsWithoutVar { get; set; }
        public bool ProgramKeyword { get; set; }
        public bool OldStrings { get; set; }
        public bool StaticArrays { get; set; }
        public bool ReadProc { get; set; }
        public bool WriteProcWithSpace { get; set; }
        public ABCStatisticsVisitor()
        {
        }

        public override void visit(var_def_statement vds)
        {
            if (vds.Parent is var_statement)
                InBlockVarDefs += 1;
            else OutBlockVarDefs += 1;

            if (vds.vars_type != null && vds.vars_type is string_num_definition)
            {
                OldStrings = true;
            }
            if (vds.vars_type != null && vds.vars_type is array_type at && at.indexers != null && at.indexers.Count > 0 && at.indexers.indexers[0] != null)
            {
                StaticArrays = true;
            }
        }

        public override void visit(program_name pn)
        {
            if (pn != null)
                ProgramKeyword = true;
        }

        public override void visit(method_call mc)
        {
            if (mc.dereferencing_value is ident id && (id.name.ToLower() == "read" || id.name.ToLower() == "readln"))
            {
                syntax_tree_node n = mc;
                do
                {
                    n = n.Parent;
                } while (n != null && !(n is class_members));
                if (n != null)
                    ReadProc = false;
                else
                    ReadProc = true;
            }
            if (mc.dereferencing_value is ident id1 && (id1.name.ToLower() == "write" || id1.name.ToLower() == "writeln") &&
                mc.parameters != null && mc.parameters.expressions.Any(ex => ex is char_const cc && cc.cconst == ' '))
            {
                WriteProcWithSpace = true;
            }
        }
        public override void visit(procedure_call pc)
        {
            if (pc.func_name is method_call mc && mc.dereferencing_value is ident id && (id.name.ToLower() == "read" || id.name.ToLower() == "readln"))
            {
                syntax_tree_node n = pc;
                do
                {
                    n = n.Parent;
                } while (n != null && !(n is class_members));
                if (n != null)
                    ReadProc = false;
                else
                    ReadProc = true;
            }
            if (pc.func_name is method_call mc1 && mc1.dereferencing_value is ident id1 && (id1.name.ToLower() == "write" || id1.name.ToLower() == "writeln")
                && mc1.parameters != null && mc1.parameters.expressions.Any(ex => ex is char_const cc && cc.cconst == ' '))
            {
                WriteProcWithSpace = true;
            }
        }

        /*public override void visit(dot_node dn)
        {
            ProcessNode(dn.left);
            if (dn.right.GetType() != typeof(ident))
                ProcessNode(dn.right);
        }*/
        /*public override void visit(function_lambda_definition fd)
        {            
        }*/
    }

}
