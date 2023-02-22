namespace VisualPascalABC
{
    partial class OutputWindowForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutputWindowForm));
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.outputTextBox = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mCOPYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSELECTALLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InputPanel = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.InputTextBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.InputPanel.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.InputPanel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(916, 310);
            this.panel2.TabIndex = 13;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Red;
            this.panel6.Controls.Add(this.outputTextBox);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(916, 264);
            this.panel6.TabIndex = 13;
            // 
            // outputTextBox
            // 
            this.outputTextBox.BackColor = System.Drawing.Color.White;
            this.outputTextBox.ContextMenuStrip = this.contextMenuStrip1;
            this.outputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.outputTextBox.Location = new System.Drawing.Point(0, 0);
            this.outputTextBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ReadOnly = true;
            this.outputTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.outputTextBox.Size = new System.Drawing.Size(916, 264);
            this.outputTextBox.TabIndex = 12;
            this.outputTextBox.Text = "";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mCOPYToolStripMenuItem,
            this.mSELECTALLToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(246, 80);
            // 
            // mCOPYToolStripMenuItem
            // 
            this.mCOPYToolStripMenuItem.Name = "mCOPYToolStripMenuItem";
            this.mCOPYToolStripMenuItem.Size = new System.Drawing.Size(245, 38);
            this.mCOPYToolStripMenuItem.Text = "M_COPY";
            this.mCOPYToolStripMenuItem.Click += new System.EventHandler(this.mCOPYToolStripMenuItem_Click);
            // 
            // mSELECTALLToolStripMenuItem
            // 
            this.mSELECTALLToolStripMenuItem.Name = "mSELECTALLToolStripMenuItem";
            this.mSELECTALLToolStripMenuItem.Size = new System.Drawing.Size(245, 38);
            this.mSELECTALLToolStripMenuItem.Text = "M_SELECT_ALL";
            this.mSELECTALLToolStripMenuItem.Click += new System.EventHandler(this.mSELECTALLToolStripMenuItem_Click);
            // 
            // InputPanel
            // 
            this.InputPanel.Controls.Add(this.panel4);
            this.InputPanel.Controls.Add(this.panel3);
            this.InputPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.InputPanel.Location = new System.Drawing.Point(0, 264);
            this.InputPanel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.InputPanel.Name = "InputPanel";
            this.InputPanel.Size = new System.Drawing.Size(916, 46);
            this.InputPanel.TabIndex = 12;
            this.InputPanel.Visible = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.panel1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(162, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(754, 46);
            this.panel4.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.InputTextBox);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(400, 46);
            this.panel5.TabIndex = 2;
            // 
            // InputTextBox
            // 
            this.InputTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.InputTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.InputTextBox.Location = new System.Drawing.Point(0, 0);
            this.InputTextBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.InputTextBox.Multiline = true;
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.Size = new System.Drawing.Size(400, 39);
            this.InputTextBox.TabIndex = 1;
            this.InputTextBox.TextChanged += new System.EventHandler(this.InputTextBox_TextChanged);
            this.InputTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputTextBox_KeyDown);
            this.InputTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.InputTextBox_KeyPress);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(400, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(354, 46);
            this.panel1.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(188, 0);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(148, 40);
            this.button2.TabIndex = 2;
            this.button2.Text = "M_STOP";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 40);
            this.button1.TabIndex = 0;
            this.button1.Text = "INPUT_OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(162, 46);
            this.panel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "INPUT";
            // 
            // OutputWindowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 310);
            this.Controls.Add(this.panel2);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "OutputWindowForm";
            this.TabText = "TP_OUTPUTWINDOW";
            this.Text = "TP_OUTPUTWINDOW";
            this.Activated += new System.EventHandler(this.OutputWindowForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OutputWindowForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OutputWindowForm_FormClosed);
            this.VisibleChanged += new System.EventHandler(this.OutputWindowForm_VisibleChanged);
            this.panel2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.InputPanel.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.RichTextBox outputTextBox;
        internal System.Windows.Forms.Panel InputPanel;
        internal System.Windows.Forms.TextBox InputTextBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mCOPYToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mSELECTALLToolStripMenuItem;
    }
}