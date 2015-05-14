using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NodeGenerator;

namespace NodesGenerator
{
    public partial class method_editor : Form
    {
        private method_info nm;
        private static method_info snm;
        private NodeGenerator.NodeGenerator ng;
        private NodeGenerator.node_info ninf;

        public method_editor()
        {
            InitializeComponent();
        }

        public method_editor(method_info nm, node_info ninf, NodeGenerator.NodeGenerator ng)
		{
			InitializeComponent();
            if (nm != null)
                this.nm = nm;
            else this.nm = new method_info();

            this.ninf=ninf;
			this.ng=ng;

			set_element();
		}

        public static method_info edit_element(method_info nm, node_info ninf, NodeGenerator.NodeGenerator _ng)
        {
            method_editor me = new method_editor(nm, ninf, _ng);
            if (me.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(me.nm.method_text))
                snm = me.nm;
            else snm = null;
            return snm;
        }

        private string help_context_name()
        {
            return ninf.node_name + "." + nm.method_header;
        }

        private void set_element()
        {
            method_code.Text = nm.method_text;
            method_code.Select(0,0);
            help_context.Text = ng.help_storage.get_help_context(help_context_name()).help_context;
        }

        private void get_element()
        {
            nm.method_text = method_code.Text;
            ng.help_storage.get_help_context(help_context_name()).help_context = help_context.Text;
        }

        private void ok_Click(object sender, EventArgs e)
        {
            get_element();
            //DialogResult = DialogResult.OK;
            //snm = nm;
            Close();
        }

    }
}
