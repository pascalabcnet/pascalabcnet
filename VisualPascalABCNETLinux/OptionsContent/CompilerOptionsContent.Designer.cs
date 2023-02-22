namespace VisualPascalABC.OptionsContent
{
    partial class CompilerOptionsContent
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
            this.btSelectOutpotDirectory = new System.Windows.Forms.Button();
            this.tbOutputDirectory = new System.Windows.Forms.TextBox();
            this.cbUseOutputDirectory = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbUseDllForSystemUnits = new System.Windows.Forms.CheckBox();
            this.cbDeletePdb = new System.Windows.Forms.CheckBox();
            this.cbDeleteExe = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fbSelectOutputDirectory = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btSelectOutpotDirectory
            // 
            this.btSelectOutpotDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSelectOutpotDirectory.Location = new System.Drawing.Point(222, 144);
            this.btSelectOutpotDirectory.Name = "btSelectOutpotDirectory";
            this.btSelectOutpotDirectory.Size = new System.Drawing.Size(77, 21);
            this.btSelectOutpotDirectory.TabIndex = 27;
            this.btSelectOutpotDirectory.Text = "BROWSE";
            this.btSelectOutpotDirectory.UseVisualStyleBackColor = true;
            this.btSelectOutpotDirectory.Click += new System.EventHandler(this.btSelectOutpotDirectory_Click);
            // 
            // tbOutputDirectory
            // 
            this.tbOutputDirectory.BackColor = System.Drawing.Color.White;
            this.tbOutputDirectory.Location = new System.Drawing.Point(5, 144);
            this.tbOutputDirectory.Name = "tbOutputDirectory";
            this.tbOutputDirectory.Size = new System.Drawing.Size(207, 20);
            this.tbOutputDirectory.TabIndex = 26;
            // 
            // cbUseOutputDirectory
            // 
            this.cbUseOutputDirectory.AutoSize = true;
            this.cbUseOutputDirectory.Location = new System.Drawing.Point(5, 120);
            this.cbUseOutputDirectory.Name = "cbUseOutputDirectory";
            this.cbUseOutputDirectory.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbUseOutputDirectory.Size = new System.Drawing.Size(168, 17);
            this.cbUseOutputDirectory.TabIndex = 25;
            this.cbUseOutputDirectory.Text = "USE_OUTPUT_DIRECTORY";
            this.cbUseOutputDirectory.UseVisualStyleBackColor = true;
            this.cbUseOutputDirectory.CheckedChanged += new System.EventHandler(this.cbUseOutputDirectory_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbUseDllForSystemUnits);
            this.groupBox2.Controls.Add(this.cbDeletePdb);
            this.groupBox2.Controls.Add(this.cbDeleteExe);
            this.groupBox2.Location = new System.Drawing.Point(5, 26);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(392, 88);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ENVORIMENT_RUNMODE";
            // 
            // cbUseDllForSystemUnits
            // 
            this.cbUseDllForSystemUnits.AutoSize = true;
            this.cbUseDllForSystemUnits.Location = new System.Drawing.Point(6, 65);
            this.cbUseDllForSystemUnits.Name = "cbUseDllForSystemUnits";
            this.cbUseDllForSystemUnits.Size = new System.Drawing.Size(191, 17);
            this.cbUseDllForSystemUnits.TabIndex = 2;
            this.cbUseDllForSystemUnits.Text = "USE_DLL_FOR_SYSTEM_UNITS";
            this.cbUseDllForSystemUnits.UseVisualStyleBackColor = true;
            // 
            // cbDeletePdb
            // 
            this.cbDeletePdb.AutoSize = true;
            this.cbDeletePdb.Location = new System.Drawing.Point(6, 42);
            this.cbDeletePdb.Name = "cbDeletePdb";
            this.cbDeletePdb.Size = new System.Drawing.Size(96, 17);
            this.cbDeletePdb.TabIndex = 1;
            this.cbDeletePdb.Text = "DELETE_PDB";
            this.cbDeletePdb.UseVisualStyleBackColor = true;
            // 
            // cbDeleteExe
            // 
            this.cbDeleteExe.AutoSize = true;
            this.cbDeleteExe.Location = new System.Drawing.Point(6, 19);
            this.cbDeleteExe.Name = "cbDeleteExe";
            this.cbDeleteExe.Size = new System.Drawing.Size(95, 17);
            this.cbDeleteExe.TabIndex = 0;
            this.cbDeleteExe.Text = "DELETE_EXE";
            this.cbDeleteExe.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(5, 5);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBox1.Size = new System.Drawing.Size(148, 17);
            this.checkBox1.TabIndex = 21;
            this.checkBox1.Text = "GENERATE_PDP_FILES";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Items.AddRange(new object[] {
            "Console Application",
            "Windows Application",
            "Class Library"});
            this.comboBox1.Location = new System.Drawing.Point(134, 171);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(165, 21);
            this.comboBox1.TabIndex = 20;
            this.comboBox1.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 171);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "OUTPUT_FILE_TYPE";
            this.label1.Visible = false;
            // 
            // CompilerOptionsContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btSelectOutpotDirectory);
            this.Controls.Add(this.tbOutputDirectory);
            this.Controls.Add(this.cbUseOutputDirectory);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Name = "CompilerOptionsContent";
            this.Size = new System.Drawing.Size(482, 311);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btSelectOutpotDirectory;
        private System.Windows.Forms.TextBox tbOutputDirectory;
        private System.Windows.Forms.CheckBox cbUseOutputDirectory;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbDeletePdb;
        private System.Windows.Forms.CheckBox cbDeleteExe;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog fbSelectOutputDirectory;
        private System.Windows.Forms.CheckBox cbUseDllForSystemUnits;
    }
}
