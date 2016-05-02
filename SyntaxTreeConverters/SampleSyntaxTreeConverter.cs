using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.SyntaxTreeConverters
{
    public class SampleVisitor: BaseEnterExitVisitor
    {
        int Tabs = -2;
        System.IO.StreamWriter fs;
        public SampleVisitor()
        {
            fs = File.CreateText("d:\\logSampleVisitor.log");
        }

        public string TabString
        {
            get
            {
                return new string(' ', Tabs);
            }
        }
        public override void Enter(syntax_tree_node st)
        {
            Tabs += 2;
            fs.Write(TabString);
        }
        public override void Exit(syntax_tree_node st)
        {
            Tabs -= 2;
            if (st.GetType()==typeof(program_module))
                fs.Close();
        }
        public override void DefaultVisit(syntax_tree_node n)
        {
            fs.WriteLine(n.GetType().Name);
            base.DefaultVisit(n);
        }
        public override void visit(ident id)
        {
            fs.WriteLine(id.name);
        }
        public override void visit(int32_const i)
        {
            fs.WriteLine(i.val);
        }
        public override void visit(token_info n)
        {
            fs.WriteLine(n.text);
        }
        public override void visit(bin_expr n)
        {
            fs.WriteLine(n.GetType().Name + " " + n.operation_type);
            base.DefaultVisit(n);
        }
    }

    public class SampleSyntaxTreeConverter: ISyntaxTreeConverter
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }
        public ConverterType ConverterType { get; set; }
        public ExecutionOrder ExecutionOrder { get; set; }
        public syntax_tree_node Convert(syntax_tree_node root)
        {
            //var v = new SampleVisitor();
            //v.ProcessNode(root);
            return root;
        }
    }
}
