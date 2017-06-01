// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
namespace NodesGenerator
{
    partial class FilterTagsCollection
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.categories = new System.Windows.Forms.ListView();
            this.catContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.items = new System.Windows.Forms.ListView();
            this.tagContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.addTag = new System.Windows.Forms.Button();
            this.deleteTag = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.merger = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.catContextMenuStrip.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tagContextMenuStrip.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.categories);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(269, 414);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Categories:";
            // 
            // categories
            // 
            this.categories.AllowDrop = true;
            this.categories.ContextMenuStrip = this.catContextMenuStrip;
            this.categories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categories.FullRowSelect = true;
            this.categories.LabelEdit = true;
            this.categories.Location = new System.Drawing.Point(3, 16);
            this.categories.MultiSelect = false;
            this.categories.Name = "categories";
            this.categories.ShowGroups = false;
            this.categories.Size = new System.Drawing.Size(263, 354);
            this.categories.TabIndex = 0;
            this.categories.UseCompatibleStateImageBehavior = false;
            this.categories.View = System.Windows.Forms.View.List;
            this.categories.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.categories_AfterLabelEdit);
            this.categories.SelectedIndexChanged += new System.EventHandler(this.categories_SelectedIndexChanged);
            this.categories.DragDrop += new System.Windows.Forms.DragEventHandler(this.categories_DragDrop);
            this.categories.DragOver += new System.Windows.Forms.DragEventHandler(this.categories_DragOver);
            // 
            // catContextMenuStrip
            // 
            this.catContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem,
            this.toolStripMenuItem2,
            this.addToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.catContextMenuStrip.Name = "catContextMenuStrip";
            this.catContextMenuStrip.Size = new System.Drawing.Size(118, 76);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(114, 6);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonDelete);
            this.panel2.Controls.Add(this.buttonAdd);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 370);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(263, 41);
            this.panel2.TabIndex = 1;
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonDelete.Location = new System.Drawing.Point(178, 6);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 1;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonAdd.Location = new System.Drawing.Point(9, 6);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.items);
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(269, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(490, 414);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Items:";
            // 
            // items
            // 
            this.items.ContextMenuStrip = this.tagContextMenuStrip;
            this.items.Dock = System.Windows.Forms.DockStyle.Fill;
            this.items.FullRowSelect = true;
            this.items.LabelEdit = true;
            this.items.Location = new System.Drawing.Point(3, 16);
            this.items.MultiSelect = false;
            this.items.Name = "items";
            this.items.Size = new System.Drawing.Size(484, 354);
            this.items.TabIndex = 1;
            this.items.UseCompatibleStateImageBehavior = false;
            this.items.View = System.Windows.Forms.View.List;
            this.items.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.items_AfterLabelEdit);            
            this.items.SelectedIndexChanged += new System.EventHandler(this.items_SelectedIndexChanged);
            this.items.MouseDown += new System.Windows.Forms.MouseEventHandler(this.items_MouseDown);
            this.items.MouseMove += new System.Windows.Forms.MouseEventHandler(this.items_MouseMove);
            // 
            // tagContextMenuStrip
            // 
            this.tagContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem3,
            this.deleteToolStripMenuItem1});
            this.tagContextMenuStrip.Name = "renameCatContextMenuStrip";
            this.tagContextMenuStrip.Size = new System.Drawing.Size(118, 54);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
            this.toolStripMenuItem1.Text = "Rename";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(114, 6);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
            this.deleteToolStripMenuItem1.Text = "Delete";
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.deleteTag_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.addTag);
            this.panel3.Controls.Add(this.deleteTag);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(3, 370);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(484, 41);
            this.panel3.TabIndex = 2;
            // 
            // addTag
            // 
            this.addTag.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.addTag.Location = new System.Drawing.Point(3, 6);
            this.addTag.Name = "addTag";
            this.addTag.Size = new System.Drawing.Size(75, 23);
            this.addTag.TabIndex = 2;
            this.addTag.Text = "Add";
            this.addTag.UseVisualStyleBackColor = true;
            this.addTag.Click += new System.EventHandler(this.addTag_Click);
            // 
            // deleteTag
            // 
            this.deleteTag.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.deleteTag.Location = new System.Drawing.Point(400, 6);
            this.deleteTag.Name = "deleteTag";
            this.deleteTag.Size = new System.Drawing.Size(75, 23);
            this.deleteTag.TabIndex = 1;
            this.deleteTag.Text = "Delete";
            this.deleteTag.UseVisualStyleBackColor = true;
            this.deleteTag.Click += new System.EventHandler(this.deleteTag_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 414);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(759, 29);
            this.panel1.TabIndex = 2;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(672, 3);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "Close";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // merger
            // 
            this.merger.Interval = 1;
            this.merger.Tick += new System.EventHandler(this.merger_Tick);
            // 
            // FilterTagsCollection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonOK;
            this.ClientSize = new System.Drawing.Size(759, 443);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "FilterTagsCollection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tags Collection...";
            this.groupBox1.ResumeLayout(false);
            this.catContextMenuStrip.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tagContextMenuStrip.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonOK;
        public System.Windows.Forms.ListView categories;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.ListView items;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button deleteTag;
        private System.Windows.Forms.ContextMenuStrip catContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip tagContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.Timer merger;
        private System.Windows.Forms.Button addTag;


    }
}