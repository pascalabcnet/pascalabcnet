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

namespace NodesGenerator
{
    public partial class TagEmForm : Form
    {
        private List<FilterCategory> tag_cats;

        public TagEmForm()
        {
            InitializeComponent();
        }

        public TagEmForm(List<FilterCategory> tag_cats)
        {
            InitializeComponent();
            foreach (var i in tag_cats)
                categories.Items.Add(i.name);
            this.tag_cats = tag_cats;
            if (categories.Items.Count > 0)
            {
                categories.SelectedIndex = 0;
                categories.Text = categories.Items[0].ToString();
            }            
        }

        private void categories_SelectedIndexChanged(object sender, EventArgs e)
        {
            tagName.Items.Clear();
            foreach (FilterTag tag in tag_cats[categories.SelectedIndex].tags)
                tagName.Items.Add(tag.name);
        }

    }
}
