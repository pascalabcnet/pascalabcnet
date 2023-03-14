namespace VisualPascalABC.OptionsContent
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.tvContentList = new System.Windows.Forms.TreeView();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.contentContainer = new System.Windows.Forms.GroupBox();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contentContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvContentList
            // 
            this.tvContentList.Location = new System.Drawing.Point(16, 15);
            this.tvContentList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tvContentList.Name = "tvContentList";
            this.tvContentList.Size = new System.Drawing.Size(236, 334);
            this.tvContentList.TabIndex = 0;
            this.tvContentList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvContentList_AfterSelect);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(684, 357);
            this.btCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(125, 28);
            this.btCancel.TabIndex = 9;
            this.btCancel.Text = "!CANCEL";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btOk
            // 
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btOk.Location = new System.Drawing.Point(547, 357);
            this.btOk.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(125, 28);
            this.btOk.TabIndex = 8;
            this.btOk.Text = "!OK";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // contentContainer
            // 
            this.contentContainer.Controls.Add(this.contentPanel);
            this.contentContainer.Location = new System.Drawing.Point(261, 15);
            this.contentContainer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.contentContainer.Name = "contentContainer";
            this.contentContainer.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.contentContainer.Size = new System.Drawing.Size(548, 335);
            this.contentContainer.TabIndex = 11;
            this.contentContainer.TabStop = false;
            // 
            // contentPanel
            // 
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(4, 19);
            this.contentPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(540, 312);
            this.contentPanel.TabIndex = 0;
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(823, 396);
            this.Controls.Add(this.contentContainer);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.tvContentList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FORMTEXT";
            this.Shown += new System.EventHandler(this.OptionsForm_Shown);
            this.contentContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvContentList;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.GroupBox contentContainer;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}