// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler.SyntaxTree;


namespace SyntaxVisitors
{
    public class SimplePrettyPrinterVisitor : WalkingVisitorNew
    {
        int off = 0;

        bool printNodeType = false;

        string filename = null;

        public static SimplePrettyPrinterVisitor New
        {
            get { return new SimplePrettyPrinterVisitor(); }
        }

        public SimplePrettyPrinterVisitor(string filename = null)
        {
            OnEnter = Enter;
            OnLeave = Exit;
            this.filename = filename;
            if (filename != null)
                System.IO.File.Delete(filename);
        }

        public void Println(string s)
        {
            Print(s + Environment.NewLine);
        }
        public void Print(string s)
        {
            PrintNoOffset(new string(' ', off) + s);
        }
        public void PrintNoOffset(string s)
        {
            s = s.Replace("PascalABCCompiler.SyntaxTree.", "").Replace("System.Collections.Generic.", "");
            if (filename == null)
            {
                Console.Write(s);
            }
            else
            {
                System.IO.File.AppendAllText(filename, s);
            }
        }
        public void PrintlnNoOffset(string s)
        {
            PrintNoOffset(s + Environment.NewLine);
        }

        public void PrintlnNode(syntax_tree_node st)
        {
            if (printNodeType)
                PrintNoOffset(st.GetType().Name + ": ");
            Println(""+st);
        }

        public void PrintNode(syntax_tree_node st)
        {
            if (printNodeType)
                PrintNoOffset(st.GetType().Name + ": ");
            Print(""+st);
        }

        public override void visit(assign ass)
        {
            DefaultVisit(ass);
        }
        public virtual void Enter(syntax_tree_node st)
        {
            if (st is statement_list)
            {
                Println("begin");
                off += 2;
            }
            else if (st is type_declarations)  // надо в visit и самому всё обрабатывать
            {
                var tds = st is type_declarations;
                Println("type ");
                off += 2;
            }
            else if (st is case_variant)
            {
                var td = st as case_variant;
                Println(td.conditions.ToString()+": ");
                off += 2;
                //ProcessNode(td.type_def);
                //visitNode = false;
            }
            else if (st is type_declaration)
            {
                var td = st as type_declaration;
                Print(td.type_name + " = ");
                ProcessNode(td.type_def);
                visitNode = false;
            }
            else if (st is procedure_definition)
            {
                //ProcessNode(st as procedure_definition);
                visitNode = visitNode;
            }
            else if (st is class_definition)
            {
                var cd = st as class_definition;
                PrintNoOffset("class");
                if (cd.class_parents?.types.Count > 0)
                    PrintlnNoOffset("(" + cd.class_parents.ToString() + ")");
                else
                    PrintlnNoOffset("");
                off += 2;
                ProcessNode(cd.body);
                visitNode = false;
            }
            else if (st is access_modifer_node)
            {
                var am = st as access_modifer_node;
                Println(am.access_level.ToString().Replace("_modifier",""));
            }
            else if (st is variable_definitions)
            {
                var vds = st as variable_definitions;
                Print("var");
                off += 2;
            }
            else if (st is var_def_statement)
            {
                var vds = st as var_def_statement;
                Println (vds.ToString());
            }
            else if (st is empty_statement || st is declarations || st is block || st is class_body_list || st is class_members 
                || st is case_variants || st is program_module || st is no_type_foreach || st is template_param_list
                || st is yield_unknown_foreach_type || st is yield_unknown_expression_type 
                )
            {
            }
           /* else if (st is array_type)
            {
                var vds = st as array_type;
                Println ("array of "+vds.elements_type.ToString());
            }*/
            else if (st is if_node)
            {
                var ifn = st as if_node;

                Println("if " + ifn.condition.ToString() + " then");
                off += 2;
                ProcessNode(ifn.then_body);
                if (ifn.else_body == null)
                {
                    visitNode = false;
                    return;
                }
                off -= 2;
                Println("else ");
                off += 2;
                ProcessNode(ifn.else_body);
                visitNode = false;
            }
            else if (st is while_node)
            {
                var wn = st as while_node;

                Println("while " + wn.expr.ToString() + " do");
                off += 2;
            }
            else if (st is for_node)
            {
                var fn = st as for_node;

                Println("for var " + fn.loop_variable + " := "+fn.initial_value+" to "+fn.finish_value + " do ");
                off += 2;
            }
            else if (st is foreach_stmt)
            {
                var wn = st as foreach_stmt;

                Println("foreach " + wn.identifier.ToString() + " in "+wn.in_what + " do");
                off += 2;
            }
            else if (st is case_node)
            {
                var cn = st as case_node;

                Println("case " + cn.param.ToString() + " of");
                off += 2;
            }
            else if (st is labeled_statement)
            {
                var lst = st as labeled_statement;

                Println(lst.label_name.name + ": ");
                ProcessNode(lst.to_statement);
                visitNode = false;
            }
            else if (st is var_statement || st is procedure_header)
            {
                var s = st.ToString();
                s = s.Replace("PascalABCCompiler.SyntaxTree.", "").Replace("System.Collections.Generic.","");

                Println(s);
                visitNode = false;
            }
            else if (st is procedure_definition)
            {
                Println("");
            }
            else if (st is statement || st is variable_definitions || st is label_definitions)
            {
                PrintNode(st);
                if (st is statement)
                    PrintlnNoOffset(";");
                else PrintlnNoOffset("");
                if (st is labeled_statement)
                    visitNode = false;                    
            }
            else if (st is token_info)
            {
                var s = st.ToString();
                if (s == "begin" || s == "end")
                    return;
                PrintlnNode(st);
            }
            else if (st is function_lambda_definition)
            {
                var f = st as function_lambda_definition;
                //PrintNoOffset(f.ToString());
                visitNode = false;                    
            }
            /*else if (st is named_type_reference)
            {
                var s = st.ToString();
                s = s.Replace("PascalABCCompiler.SyntaxTree.", "").Replace("System.Collections.Generic.","");

                PrintlnNoOffset(s);
            }*/
            else
            {
                var q = st.GetType().GetMethod("ToString", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
                if (q == null)
                    Println(st.GetType().Name);
            }
        }
        public virtual void Exit(syntax_tree_node st)
        {
            if (st is statement_list)
            {
                off -= 2;
                Println("end");
            }
            else if (st is while_node || st is if_node || st is type_declarations || st is variable_definitions || st is case_variant)
            {
                off -= 2;
            }
            else if (st is class_definition || st is case_node)
            {
                off -= 2;
                Println("end;");
            }
        }

    }
}
