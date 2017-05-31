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
using NodeGenerator;
using System.Collections;

namespace NodesGenerator
{
    public partial class FilterTagsCollection : Form
    {
        private List<node_info> nodes;
        private List<FilterCategory> source;
        private int selected_cat = -1;
        private int selected_tag = -1;
        private bool add_mode = false;
        private bool add_tag_mode = false;

        private Point dd_start_point;

        public FilterTagsCollection()
        {
            InitializeComponent();
        }

        public FilterTagsCollection(NodeGenerator.NodeGenerator node_gen, int init_cat)
        {
            InitializeComponent();
            set_contents(node_gen, init_cat);
        }

        public void set_contents(NodeGenerator.NodeGenerator node_gen, int init_cat)
        {
            source = node_gen.tag_cats;
            nodes = new List<node_info>();
            for (int i = 0; i < node_gen.all_nodes.Count; i++)
                nodes.Add(node_gen.all_nodes[i] as node_info);            
            categories.Items.Clear();            
            for (int i = 0; i < source.Count; i++)
            {
                categories.Items.Add(source[i].name);                
            }
            if (init_cat >= 0)
                categories.SelectedIndices.Add(init_cat);
            else if (categories.Items.Count > 0)
                categories.SelectedIndices.Add(0);
            select_category();
        }

        public void select_category()
        {
            if (categories.SelectedIndices.Count == 0) return;
            int x = categories.SelectedIndices[0];
            if (x != selected_cat)
            {                
                selected_cat = x;
                load_category(selected_cat);
            }
        }

        public void load_category(int ind)
        {
            items.Items.Clear();
            for (int j = 0; j < source[ind].tags.Count; j++)
                items.Items.Add(source[ind].tags[j].name);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            add_mode = true;
            selected_cat = source.Count;
            source.Add(new FilterCategory());            
            categories.Items.Add(String.Empty).BeginEdit();            
        }

        private void categories_SelectedIndexChanged(object sender, EventArgs e)
        {
            select_category();
        }

        private void categories_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {                       
            if (categories.Items[e.Item].Text != "" && e.Label == null) return;
            string label = null;
            if (e.Label != null) label = e.Label.Trim();
            if (e.Label == null || e.Label == "")
            {
                if (add_mode)
                {
                    source.RemoveAt(selected_cat);
                    categories.Items.RemoveAt(categories.Items.Count - 1);
                    add_mode = false;
                }
                else
                {
                    MessageBox.Show("Name can not be Empty!");
                    e.CancelEdit = true;
                }
            }
            else
            {
                source[selected_cat].name = label;
                if (add_mode)
                {
                    add_mode = false;
                    selected_cat = -1;
                    categories.SelectedIndices.Clear();
                    categories.SelectedIndices.Add(categories.Items.Count - 1);
                    select_category();
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string messageBoxText = "Are you sure, you want to delete '" + categories.SelectedItems[0].Text + "' category?";
            string caption = "Delete the category?";
            MessageBoxButtons button = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Warning;
            MessageBoxDefaultButton def_btn = MessageBoxDefaultButton.Button2;

            if (MessageBox.Show(messageBoxText, caption, button, icon, def_btn) == DialogResult.Yes)
            {
                for (int j = 0; j < nodes.Count; j++)
                {                    
                    var remove_list = new List<Tuple<int, int>>();
                    for (int i = 0; i < nodes[j].tags.Count; i++)
                        if (nodes[j].tags[i].Item1 == selected_cat)
                            remove_list.Add(nodes[j].tags[i]);
                        else if (nodes[j].tags[i].Item1 > selected_cat)
                            nodes[j].tags[i] = new Tuple<int,int>(nodes[j].tags[i].Item1-1, nodes[j].tags[i].Item2);
                    foreach (var ni in remove_list)
                        nodes[j].tags.Remove(ni);
                }

                source.RemoveAt(selected_cat);
                categories.Items.RemoveAt(selected_cat);
                items.Items.Clear();
            }
        }        

        private void merge_tags(int cat_id, int tag1, int tag2)
        {
            if (tag1 == tag2) return;

            int t = Math.Max(tag1, tag2);
            tag1 = Math.Min(tag1, tag2);
            tag2 = t;
            
            for (int j = 0; j < nodes.Count; j++)
            {
                bool has_tag1 = false;
                int node_tag2 = -1;
                for (int k = 0; k < nodes[j].tags.Count; k++)
                {
                    if (nodes[j].tags[k].Item1 == cat_id)
                    {
                        if (nodes[j].tags[k].Item2 == tag1)
                            has_tag1 = true; else
                        if (nodes[j].tags[k].Item2 == tag2)
                            node_tag2 = k;
                    }
                }
                if (node_tag2 >= 0)
                    if (has_tag1)
                        nodes[j].tags.RemoveAt(node_tag2);
                    else
                    {
                        nodes[j].tags[node_tag2] = new Tuple<int, int>(cat_id, tag1);
                        source[cat_id].tags[tag1].ref_count++;
                    }
                for (int i = 0; i < nodes[j].tags.Count; i++)
                    if (nodes[j].tags[i].Item1 == cat_id && nodes[j].tags[i].Item2 > tag2)
                        nodes[j].tags[i] = new Tuple<int, int>(cat_id, nodes[j].tags[i].Item2 - 1);
            }

            source[cat_id].tags.RemoveAt(tag2);
        }

        private void merger_Tick(object sender, EventArgs e)
        {
            int x = (merger.Tag as Tuple<int, int>).Item1;
            int y = (merger.Tag as Tuple<int, int>).Item2;
            merge_tags(selected_cat, x, y);
            items.Items.RemoveAt(x);
            merger.Enabled = false;
        }

        private void items_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (items.Items[e.Item].Text != "" && e.Label == null) return;
            string label = null;
            if (e.Label != null) label = e.Label.Trim();
            if (e.Label == null || label == "")
            {
                if (add_tag_mode)
                {
                    source[selected_cat].tags.RemoveAt(selected_tag);
                    items.Items.RemoveAt(items.Items.Count - 1);
                    add_tag_mode = false;
                }
                else
                {
                    MessageBox.Show("Name can not be Empty!");
                    e.CancelEdit = true;
                }
            }
            else
            {
                int y = source[selected_cat].tags.FindIndex(x => x.name == label);
                if (y >= 0 && y != selected_tag)
                {
                    merger.Tag = new Tuple<int, int>(selected_tag, y);
                    merger.Enabled = true;
                }
                else
                    source[selected_cat].tags[selected_tag].name = label;
            }
        }

        private void deleteTag_Click(object sender, EventArgs e)
        {
            if (selected_cat == -1 || selected_tag == -1) return;
            string tag_name = items.SelectedItems[0].Text;
            var ind = source[selected_cat].tags.FindIndex(x => x.name == tag_name);
            foreach (var node in nodes)
            {
                int remove_ind = -1;
                for (int i = 0; i < node.tags.Count; i++)
                    if (node.tags[i].Item1 == selected_cat)
                        if (node.tags[i].Item2 > ind)
                            node.tags[i] = new Tuple<int, int>(selected_cat, node.tags[i].Item2 - 1);
                        else if (node.tags[i].Item2 == ind)
                            remove_ind = i;
                if (remove_ind > -1)
                    node.tags.RemoveAt(remove_ind);
            }
            source[selected_cat].tags.RemoveAt(ind);
            items.Items.Remove(items.SelectedItems[0]);
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selected_cat >= 0)
                categories.Items[selected_cat].BeginEdit();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (selected_tag >= 0)
                items.Items[selected_tag].BeginEdit();
        }

        private void items_MouseDown(object sender, MouseEventArgs e)
        {
            dd_start_point = e.Location;
        }

        private void items_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button == System.Windows.Forms.MouseButtons.Left) &&
                (Math.Abs(e.X - dd_start_point.X) + Math.Abs(e.Y - dd_start_point.Y) >= 5))                
                    DoDragDrop(sender, DragDropEffects.Move);
        }

        private void move_tag(int old_cat, int new_cat, int tag)
        {
            if (old_cat == new_cat) return;
            
            bool b;
            int new_tag = source[new_cat].tags.FindIndex(x => x.name == source[old_cat].tags[tag].name);
            if (new_tag == -1)
            {
                source[new_cat].tags.Add(source[old_cat].tags[tag]);
                new_tag = source[new_cat].tags.Count - 1;
            }
            b = (new_tag == -1);

            source[old_cat].tags.RemoveAt(tag);

            foreach (var node in nodes)
            {
                int remove_at = -1;
                for (int i = 0; i < node.tags.Count; i++)
                    if (node.tags[i].Item1 == old_cat)
                        if (node.tags[i].Item2 == tag)
                        {
                            if (b)
                                node.tags[i] = new Tuple<int, int>(new_cat, new_tag);
                            else
                            {
                                if (node.tags.Contains(new Tuple<int, int>(new_cat, new_tag)))
                                {
                                    source[new_cat].tags[new_tag].ref_count++;
                                    remove_at = i;
                                }
                                else node.tags[i] = new Tuple<int, int>(new_cat, new_tag);
                            }
                        }
                        else if (node.tags[i].Item2 > tag)
                            node.tags[i] = new Tuple<int, int>(old_cat, node.tags[i].Item2 - 1);
                if (remove_at > -1)
                    node.tags.RemoveAt(remove_at);
            }
        }

        private void categories_DragDrop(object sender, DragEventArgs e)
        {
            var pnt = categories.PointToClient(new Point(e.X, e.Y));
            var new_cat = categories.GetItemAt(pnt.X, pnt.Y);
            if ((new_cat != null) && (new_cat != categories.Items[selected_cat]))
            {
                move_tag(selected_cat, categories.Items.IndexOf(new_cat), selected_tag);
                categories.SelectedIndices.Clear();
                categories.SelectedIndices.Add(categories.Items.IndexOf(new_cat));             
            }
        }

        private void categories_DragOver(object sender, DragEventArgs e)
        {
            var pnt = categories.PointToClient(new Point(e.X, e.Y));
            var new_cat = categories.GetItemAt(pnt.X, pnt.Y);
            if ((new_cat != null) && (new_cat != categories.Items[selected_cat]))
                e.Effect = DragDropEffects.Move;
            else e.Effect = DragDropEffects.None;
        }

        private void items_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (items.SelectedIndices.Count > 0)
                selected_tag = items.SelectedIndices[0];
            else selected_tag = -1;
        }

        private void addTag_Click(object sender, EventArgs e)
        {
            if (selected_cat < 0) return;
            add_tag_mode = true;
            selected_tag = source[selected_cat].tags.Count;
            source[selected_cat].tags.Add(new FilterTag());
            items.Items.Add(String.Empty).BeginEdit();  
        }
    }
}
