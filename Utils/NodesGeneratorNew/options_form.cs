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

namespace NodesGenerator
{
    public partial class options_form : Form
    {
        public options_form()
        {
            InitializeComponent();
        }

        internal void set_options_infromation(NodeGenerator.NodeGenerator node_gen, bool auto_save, string ver_storage, List<string> ac_list)
        {
            file_name.Text = node_gen.file_name;
            namespace_name.Text = node_gen.namespace_name;            
            visitor_interface_file_name.Text = node_gen.visitor_interface_file_name;
            tb_pcu_writer.Text = node_gen.pcu_writer_name;
            tb_pcu_reader.Text = node_gen.pcu_reader_name;

            autoSaveOnExit.Checked = auto_save;
            versionStorage.Text = ver_storage;
            AutoCompletionList.Lines = ac_list.ToArray();
        }

        internal void get_options_infromation(NodeGenerator.NodeGenerator node_gen, out bool auto_save, out string ver_storage, List<string> ac_list)
        {
            node_gen.file_name = file_name.Text;
            node_gen.namespace_name = namespace_name.Text;            
            node_gen.visitor_interface_file_name = visitor_interface_file_name.Text;
            node_gen.pcu_writer_file_name = tb_pcu_writer.Text + ".cs";
            node_gen.pcu_writer_name = tb_pcu_writer.Text;
            node_gen.pcu_reader_file_name = tb_pcu_reader.Text + ".cs";
            node_gen.pcu_reader_file_name_h = tb_pcu_reader.Text + ".h";
            node_gen.pcu_reader_file_name_cpp = tb_pcu_reader.Text + ".cpp";
            node_gen.pcu_reader_name = tb_pcu_reader.Text;

            auto_save = autoSaveOnExit.Checked;
            ver_storage = versionStorage.Text;
            ac_list.Clear();
            for (int i = 0; i < AutoCompletionList.Lines.Count(); i++)
                ac_list.Add(AutoCompletionList.Lines[i]);
        }

        private void browseVersionStorage_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = versionStorage.Text;
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                versionStorage.Text = folderBrowserDialog1.SelectedPath;
        }

    }
}
