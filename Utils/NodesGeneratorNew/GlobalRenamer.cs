// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using NodeGenerator;

namespace NodesGenerator
{
    public partial class GlobalRenamer : Form
    {        
        private IEnumerable<node_info> nodes;
        private ListBox list_box;
        private NodeGenerator.NodeGenerator node_gen; //needed to refactor names in lists

        public GlobalRenamer()
        {
            InitializeComponent();
        }

        public GlobalRenamer(ListBox nds, NodeGenerator.NodeGenerator ngen)
        {
            InitializeComponent();
            nodes = nds.Items.Cast<node_info>();
            list_box = nds;
            node_gen = ngen;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Apply
            Regex replacer = new Regex(textBox1.Text);
            foreach (var node in nodes)
                node.node_name = replacer.Replace(node.node_name, textBox2.Text);

            //Update visual list
            for (int i = 0; i < list_box.Items.Count; i++)
            {
                var nd = list_box.Items[i];
                list_box.Items.RemoveAt(i);
                list_box.Items.Insert(i, nd);
            }

            //Refactoring names in lists
            foreach (node_info node in node_gen.all_nodes)
            {
                foreach (var subnode in node.subnodes)
                {
                    if (subnode is extended_simple_element)
                    {
                        extended_simple_element sn = (subnode as extended_simple_element);
                        string list_type = sn.list_type;
                        if (list_type != "")
                            sn.list_type = replacer.Replace(list_type, textBox2.Text);
                    }
                }
            }
        }
        
    }
}
