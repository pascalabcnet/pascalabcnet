// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VisualPascalABC.OptionsContent
{
    public partial class OptionsForm : Form
    {
        OptionsContentEngine contentEngine;
        TreeNode lastSelectedNode = null;
        Dictionary<TreeNode, IOptionsContent> nodes = new Dictionary<TreeNode, IOptionsContent>();
        string strprefix = "VP_OPTIONSFORM_";
        public OptionsForm(OptionsContentEngine contentEngine)
        {
            var sl = 50;
            this.contentEngine = contentEngine;
            System.Threading.Thread.Sleep(sl); // SSM 07.11.16 поставил на всякий случай задержки - иногда окно опций при открытии неактивное
            InitializeComponent();
            System.Threading.Thread.Sleep(sl);
            foreach (IOptionsContent content in contentEngine.ContentList)
            {
                TreeNode tn = new TreeNode(content.ContentName);
                tvContentList.Nodes.Add(tn);
                nodes.Add(tn, content);
            }
            System.Threading.Thread.Sleep(sl); 
            PascalABCCompiler.StringResources.SetTextForAllObjects(this, strprefix);
            System.Threading.Thread.Sleep(sl);
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            contentEngine.Action(OptionsContentAction.Ok);
            Close();
        }

        private void OptionsForm_Shown(object sender, EventArgs e)
        {
            if (tvContentList.Nodes.Count > 0)
            {
                tvContentList.SelectedNode = tvContentList.Nodes[0];
                lastSelectedNode = tvContentList.SelectedNode;
            }
        }

        private void tvContentList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            IOptionsContent cont;
            cont = nodes[e.Node];
            contentEngine.Action(cont, OptionsContentAction.Show);
            contentPanel.Controls.Add(cont.Content);
            contentContainer.Text = cont.Description;
            cont.Content.Show();

            if (lastSelectedNode != null && e.Node != lastSelectedNode)
            {
                cont = nodes[lastSelectedNode];
                contentEngine.Action(cont, OptionsContentAction.Hide);
                contentPanel.Controls.Remove(cont.Content);
                cont.Content.Hide();
            }


            lastSelectedNode = e.Node;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            contentEngine.Action(OptionsContentAction.Cancel);
            Close();
        }

    }
}
