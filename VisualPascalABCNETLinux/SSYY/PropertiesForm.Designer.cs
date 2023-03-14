namespace VisualPascalABC
{
    partial class PropertiesForm
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
            this.componentsComboPanel = new System.Windows.Forms.Panel();
            this.componentsCombo = new NETXP.Controls.ComboBoxEx();
            this.panel2 = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.componentsComboPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // componentsComboPanel
            // 
            this.componentsComboPanel.Controls.Add(this.componentsCombo);
            this.componentsComboPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.componentsComboPanel.Location = new System.Drawing.Point(0, 0);
            this.componentsComboPanel.Name = "componentsComboPanel";
            this.componentsComboPanel.Size = new System.Drawing.Size(292, 26);
            this.componentsComboPanel.TabIndex = 1;
            // 
            // componentsCombo
            // 
            this.componentsCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.componentsCombo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.componentsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.componentsCombo.EnableMRU = false;
            this.componentsCombo.Flags = NETXP.Controls.AutoCompleteFlags.None;
            this.componentsCombo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.componentsCombo.ItemHeight = 20;
            this.componentsCombo.Location = new System.Drawing.Point(0, 0);
            this.componentsCombo.MRUHive = NETXP.Controls.MRUKeyHive.CurrentUser;
            this.componentsCombo.MRUKey = "Software\\Microsoft\\Internet Explorer\\TypedURLs";
            this.componentsCombo.Name = "componentsCombo";
            this.componentsCombo.Size = new System.Drawing.Size(292, 26);
            this.componentsCombo.TabIndex = 0;
            this.componentsCombo.Visible = false;
            this.componentsCombo.SelectedIndexChanged += new System.EventHandler(this.componentsCombo_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.propertyGrid1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 26);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(292, 236);
            this.panel2.TabIndex = 2;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(292, 236);
            this.propertyGrid1.TabIndex = 0;
            // 
            // PropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 262);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.componentsComboPanel);
            this.HideOnClose = true;
            this.Name = "PropertiesForm";
            this.TabText = "PropertiesForm";
            this.Text = "PropertiesForm";
            this.componentsComboPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel componentsComboPanel;
        private NETXP.Controls.ComboBoxEx componentsCombo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
    }
}