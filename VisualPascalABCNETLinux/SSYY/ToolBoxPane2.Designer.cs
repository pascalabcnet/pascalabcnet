namespace SampleDesignerApplication
{
    partial class ToolBoxPane
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBar1 = new NETXP.Controls.Bars.ListBar();
            this.listWindowsForms = new NETXP.Controls.Bars.ListBarGroup();
            this.listData = new NETXP.Controls.Bars.ListBarGroup();
            this.listComponents = new NETXP.Controls.Bars.ListBarGroup();
            ((System.ComponentModel.ISupportInitialize)(this.listBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // listBar1
            // 
            this.listBar1.AllowDragItems = false;
            this.listBar1.AllowDrop = true;
            this.listBar1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.listBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBar1.Groups.AddRange(new NETXP.Controls.Bars.ListBarGroup[] {
            this.listWindowsForms,
            this.listData,
            this.listComponents});
            this.listBar1.Location = new System.Drawing.Point(0, 0);
            this.listBar1.Name = "listBar1";
            this.listBar1.Size = new System.Drawing.Size(266, 337);
            this.listBar1.TabIndex = 0;
            this.listBar1.ToolTip = null;
            this.listBar1.SelectedGroupChanged += new System.EventHandler(this.listBar1_SelectedGroupChanged);
            this.listBar1.ItemClicked += new NETXP.Controls.Bars.ItemClickedEventHandler(this.listBar1_ItemClicked);
            this.listBar1.Paint += new System.Windows.Forms.PaintEventHandler(this.listBar1_Paint);
            this.listBar1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBar1_MouseDown);
            // 
            // listWindowsForms
            // 
            this.listWindowsForms.Caption = "VP_MF_M_COMMON_COMPONENTS";
            this.listWindowsForms.Key = "";
            this.listWindowsForms.ToolTipText = "";
            this.listWindowsForms.View = NETXP.Controls.Bars.ListBarGroupView.TextRight;
            // 
            // listData
            // 
            this.listData.Caption = "VP_MF_M_DATA_COMPONENTS";
            this.listData.Key = "";
            this.listData.ToolTipText = "";
            this.listData.View = NETXP.Controls.Bars.ListBarGroupView.TextRight;
            // 
            // listComponents
            // 
            this.listComponents.Caption = "VP_MF_M_NON_VISUAL_COMPONENTS";
            this.listComponents.Key = "";
            this.listComponents.ToolTipText = "";
            this.listComponents.View = NETXP.Controls.Bars.ListBarGroupView.TextRight;
            // 
            // ToolBoxPane
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listBar1);
            this.Name = "ToolBoxPane";
            this.Size = new System.Drawing.Size(266, 337);
            ((System.ComponentModel.ISupportInitialize)(this.listBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private NETXP.Controls.Bars.ListBar listBar1;
        private NETXP.Controls.Bars.ListBarGroup listData;
        private NETXP.Controls.Bars.ListBarGroup listWindowsForms;
        private NETXP.Controls.Bars.ListBarGroup listComponents;
    }
}
