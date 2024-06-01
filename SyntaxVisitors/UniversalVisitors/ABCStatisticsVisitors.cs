// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Linq;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{

    public class ABCStatisticsVisitor : WalkingVisitorNew  
    {
        public int InBlockVarDefs { get; set; }
        public int OutBlockVarDefs { get; set; }
        public int ForsWithoutVar { get; set; }
        public int ForsWithVar { get; set; }
        public bool ProgramKeyword { get; set; }
        public int OldStrings { get; set; }
        public int StaticArrays { get; set; }
        public int DynamicArrays { get; set; }
        public int ReadProc { get; set; }
        public int WriteProcWithSpace { get; set; }
        public int TuplesCount { get; set; }
        public int InitVarInDef { get; set; }
        public int UnpackingAssign { get; set; }
        public int LoopsCount { get; set; }
        public int ForeachCount { get; set; }
        public int LambdasCount { get; set; }
        public int ExtAssignCount { get; set; }
        public int BigIntegerTypeCount { get; set; }
        public int ReadFuncCount { get; set; }
        public int PrintCount { get; set; }

        public ABCStatisticsVisitor()
        {
        }

        public override void visit(ident id)
        {
            var name = id.name.ToLower();
            if (name.StartsWith("readinteger") || name.StartsWith("readreal") || name.StartsWith("readstring") ||
                name.StartsWith("readlninteger") || name.StartsWith("readlnreal") || name.StartsWith("readlnstring"))
                    ReadFuncCount++;
            if (name.StartsWith("print"))
                PrintCount++;
        }

        public override void visit(named_type_reference ntr)
        {
            var name = ntr.names[0].name.ToLower();
            if (name == "biginteger")
                BigIntegerTypeCount++;
            base.visit(ntr);
        }
        public override void visit(assign an)
        {
            if (an.operator_type == Operators.AssignmentAddition || an.operator_type == Operators.AssignmentMultiplication || an.operator_type == Operators.AssignmentDivision)
                ExtAssignCount++;
            base.visit(an);
        }
        public override void visit(function_lambda_definition fld)
        {
            LambdasCount++;
            base.visit(fld);
        }
        public override void visit(loop_stmt ln)
        {
            LoopsCount++;
            base.visit(ln);
        }
        public override void visit(foreach_stmt fn)
        {
            ForeachCount++;
            base.visit(fn);
        }
        public override void visit(tuple_node tn)
        {
            TuplesCount++;
            base.visit(tn);
        }
        public override void visit(assign_var_tuple at)
        {
            UnpackingAssign++;
            base.visit(at);
        }
        public override void visit(assign_tuple at)
        {
            UnpackingAssign++;
            base.visit(at);
        }

        public override void visit(for_node fn)
        {
            if (fn.create_loop_variable == false)
                ForsWithoutVar++;
            if (fn.create_loop_variable == true)
                ForsWithVar++;
            base.visit(fn);
        }

        public override void visit(var_def_statement vds)
        {
            if (vds.Parent is var_statement)
            {
                InBlockVarDefs += vds.vars.Count;
            }
            else if (!(vds.Parent is class_members))
            {
                OutBlockVarDefs += vds.vars.Count;
            }

            if (vds.vars_type != null && vds.vars_type is string_num_definition)
            {
                OldStrings += 1;
            }
            if (vds.vars_type != null && vds.vars_type is array_type at && at.indexers != null && at.indexers.Count > 0 && at.indexers.indexers[0] != null)
            {
                StaticArrays += 1;
            }
            if (vds.vars_type != null && vds.vars_type is array_type at1 && at1.indexers == null)
            {
                DynamicArrays += 1;
            }
            if (vds.inital_value != null)
                InitVarInDef++;
            /*if (vds.inital_value != null)
            {
                if (vds.inital_value is ident id && 
                    (id.name.ToLower().StartsWith("readinteger") || id.name.ToLower().StartsWith("readreal") || id.name.ToLower().StartsWith("readstring") ||
                    id.name.ToLower().StartsWith("readlninteger") || id.name.ToLower().StartsWith("readlnreal") || id.name.ToLower().StartsWith("readlnstring")))
                    ReadFuncCount++;
            }*/
                
            base.visit(vds);
        }

        public override void visit(program_name pn)
        {
            if (pn != null)
                ProgramKeyword = true;
        }

        public override void visit(method_call mc)
        {
            /*if (mc.dereferencing_value is ident id && (id.name.ToLower() == "read" || id.name.ToLower() == "readln"))
            {
                syntax_tree_node n = mc;
                do
                {
                    n = n.Parent;
                } while (n != null && !(n is class_members));
                if (n != null)
                    ;
                //ReadProc = false;
                else
                    ReadProc += 1;
            }*/
            /*if (mc.dereferencing_value is ident id2)
            {
                var name = id2.name.ToLower();
                var h = new HashSet<string> { "readinteger", "readlninteger", "readreal", "readlnreal", "readstring", "readlnstring", "readchar", "readlnchar" };
                if (h.Contains(name))
                    ReadFuncCount++;
            }*/

            /*if (mc.dereferencing_value is ident id1 && (id1.name.ToLower() == "write" || id1.name.ToLower() == "writeln") &&
                mc.parameters != null && mc.parameters.expressions.Any(ex => ex is char_const cc && cc.cconst == ' '))
            {
                WriteProcWithSpace += 1;
            }*/
            base.visit(mc);
        }
        public override void visit(procedure_call pc)
        {
            if (pc.func_name is method_call mc)
                if (mc.dereferencing_value is ident id)
                    if (id.name != null && (id.name.ToLower() == "read" || id.name.ToLower() == "readln"))
            {
                syntax_tree_node n = pc;
                do
                {
                    n = n.Parent;
                } while (n != null && !(n is class_members));
                if (n != null)
                    //ReadProc = false
                        ;
                else
                    ReadProc += 1;
            }
            if (pc.func_name is method_call mc1 && mc1.dereferencing_value is ident id1 && 
                (id1.name != null && (id1.name.ToLower() == "write" || id1.name.ToLower() == "writeln"))
                && mc1.parameters != null && mc1.parameters.expressions.Any(ex => ex is char_const cc && cc.cconst == ' '))
            {
                WriteProcWithSpace += 1;
            }
            base.visit(pc);
        }

        public int CalcHealth(out int NegativePercent, out int PositivePercent)
        {
            var stat = this;
            var Percent = 100;
            NegativePercent = 0;
            PositivePercent = 0;

            var diffBlockVarDefs = stat.OutBlockVarDefs; // - stat.InBlockVarDefs;
            if (diffBlockVarDefs > 0)
            {
                NegativePercent += 10; // за первую
                NegativePercent += (diffBlockVarDefs - 1) * 2; // за оставшиеся
                if (NegativePercent > 25)
                    NegativePercent = 25;
            }
            // For i
            if (stat.ForsWithoutVar > 0)
            {
                NegativePercent += Math.Min(15 + (stat.ForsWithoutVar - 1) * 3, 25);
            }
            if (stat.ReadProc > 0)
            {
                NegativePercent += Math.Min(15 + (stat.ReadProc - 1) * 2, 20);
            }
            if (stat.ProgramKeyword)
            {
                NegativePercent += 10;
            }
            if (stat.StaticArrays > 0)
            {
                NegativePercent += Math.Min(10 + (stat.StaticArrays - 1) * 2, 15);
            }
            if (stat.WriteProcWithSpace > 0)
            {
                NegativePercent += Math.Min(2 + (stat.WriteProcWithSpace - 1) * 1, 5);
            }
            if (stat.OldStrings > 0)
            {
                NegativePercent += stat.OldStrings;
            }
            if (NegativePercent < 0)
            {
                NegativePercent = 0;
            }

            Percent -= NegativePercent;

            // Проценты за положительное
            if (stat.InBlockVarDefs > 0)
            {
                PositivePercent += 10; // за первую
                PositivePercent += (stat.InBlockVarDefs - 1) * 2; // за оставшиеся 
                                                                  // этот алгоритм уравновешивает описания внутри и вне. За одинаковое количество дается 0 баллов
            }
            if (stat.ForsWithVar != 0)
            {
                PositivePercent += Math.Min(stat.ForsWithVar, 4) * 3;
            }
            if (stat.InitVarInDef > 0)
            {
                PositivePercent += Math.Min(stat.InitVarInDef, 4) * 4;
            }
            if (stat.ReadFuncCount > 0)
            {
                PositivePercent += Math.Min(stat.ReadFuncCount, 4) * 3;
            }
            if (stat.ExtAssignCount > 0)
            {
                PositivePercent += Math.Min(stat.ExtAssignCount, 4) * 3;
            }
            if (stat.PrintCount > 0)
            {
                PositivePercent += Math.Min(stat.PrintCount, 4) * 4;
            }
            if (stat.TuplesCount > 0)
            {
                PositivePercent += Math.Min(stat.TuplesCount, 4) * 5;
            }
            if (stat.DynamicArrays > 0)
            {
                PositivePercent += Math.Min(stat.DynamicArrays, 4) * 5;
            }
            if (stat.UnpackingAssign > 0)
            {
                PositivePercent += Math.Min(stat.UnpackingAssign, 4) * 5;
            }
            if (stat.LoopsCount > 0)
            {
                PositivePercent += Math.Min(stat.LoopsCount, 4) * 4;
            }
            if (stat.ForeachCount > 0)
            {
                PositivePercent += Math.Min(stat.ForeachCount, 4) * 4;
            }
            if (stat.LambdasCount > 0)
            {
                PositivePercent += Math.Min(stat.LambdasCount, 4) * 8;
            }

            Percent += PositivePercent;
            if (Percent > 200)
                Percent = 200;

            if (Percent < 0)
                Percent = 0;

            return Percent;
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
