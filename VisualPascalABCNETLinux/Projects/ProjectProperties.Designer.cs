
namespace VisualPascalABC
{
	partial class ProjectProperties
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbDebugRelease = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbDeletePdb = new System.Windows.Forms.CheckBox();
            this.cbDeleteExe = new System.Windows.Forms.CheckBox();
            this.tbOutputDirectory = new System.Windows.Forms.TextBox();
            this.btSelectOutpotDirectory = new System.Windows.Forms.Button();
            this.tbRunArguments = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbAppIcon = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.pbAppIcon = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbCopyright = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbTradeMark = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbCompany = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbProduct = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbRevision = new System.Windows.Forms.TextBox();
            this.tbBuild = new System.Windows.Forms.TextBox();
            this.tbMinor = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbMajor = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbGenerateXmlDoc = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAppIcon)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(163, 282);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(78, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "RF_OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(250, 282);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "RF_CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "PRJ_DEBUG_RELEASE";
            // 
            // cbDebugRelease
            // 
            this.cbDebugRelease.FormattingEnabled = true;
            this.cbDebugRelease.Items.AddRange(new object[] {
            "Debug",
            "Release"});
            this.cbDebugRelease.Location = new System.Drawing.Point(6, 25);
            this.cbDebugRelease.Name = "cbDebugRelease";
            this.cbDebugRelease.Size = new System.Drawing.Size(134, 21);
            this.cbDebugRelease.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbDeletePdb);
            this.groupBox2.Controls.Add(this.cbDeleteExe);
            this.groupBox2.Location = new System.Drawing.Point(6, 52);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(290, 66);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "PRJ_ENVIRONMENT_RUNMODE";
            // 
            // cbDeletePdb
            // 
            this.cbDeletePdb.AutoSize = true;
            this.cbDeletePdb.Location = new System.Drawing.Point(6, 42);
            this.cbDeletePdb.Name = "cbDeletePdb";
            this.cbDeletePdb.Size = new System.Drawing.Size(122, 17);
            this.cbDeletePdb.TabIndex = 1;
            this.cbDeletePdb.Text = "PRJ_DELETE_PDB";
            this.cbDeletePdb.UseVisualStyleBackColor = true;
            // 
            // cbDeleteExe
            // 
            this.cbDeleteExe.AutoSize = true;
            this.cbDeleteExe.Location = new System.Drawing.Point(6, 19);
            this.cbDeleteExe.Name = "cbDeleteExe";
            this.cbDeleteExe.Size = new System.Drawing.Size(121, 17);
            this.cbDeleteExe.TabIndex = 0;
            this.cbDeleteExe.Text = "PRJ_DELETE_EXE";
            this.cbDeleteExe.UseVisualStyleBackColor = true;
            // 
            // tbOutputDirectory
            // 
            this.tbOutputDirectory.BackColor = System.Drawing.Color.White;
            this.tbOutputDirectory.Location = new System.Drawing.Point(6, 147);
            this.tbOutputDirectory.Name = "tbOutputDirectory";
            this.tbOutputDirectory.Size = new System.Drawing.Size(207, 20);
            this.tbOutputDirectory.TabIndex = 27;
            this.tbOutputDirectory.Text = "bin\\";
            // 
            // btSelectOutpotDirectory
            // 
            this.btSelectOutpotDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btSelectOutpotDirectory.Location = new System.Drawing.Point(219, 146);
            this.btSelectOutpotDirectory.Name = "btSelectOutpotDirectory";
            this.btSelectOutpotDirectory.Size = new System.Drawing.Size(77, 21);
            this.btSelectOutpotDirectory.TabIndex = 28;
            this.btSelectOutpotDirectory.Text = "PRJ_BROWSE";
            this.btSelectOutpotDirectory.UseVisualStyleBackColor = true;
            this.btSelectOutpotDirectory.Click += new System.EventHandler(this.btSelectOutpotDirectory_Click);
            // 
            // tbRunArguments
            // 
            this.tbRunArguments.Location = new System.Drawing.Point(6, 196);
            this.tbRunArguments.Name = "tbRunArguments";
            this.tbRunArguments.Size = new System.Drawing.Size(236, 20);
            this.tbRunArguments.TabIndex = 29;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 180);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "PRJ_COMMAND_LINE_ARGS";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 229);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "PRJ_APP_ICON";
            this.label3.Visible = false;
            // 
            // tbAppIcon
            // 
            this.tbAppIcon.Location = new System.Drawing.Point(6, 245);
            this.tbAppIcon.Name = "tbAppIcon";
            this.tbAppIcon.Size = new System.Drawing.Size(236, 20);
            this.tbAppIcon.TabIndex = 32;
            this.tbAppIcon.Visible = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(253, 245);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(27, 19);
            this.button1.TabIndex = 33;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pbAppIcon
            // 
            this.pbAppIcon.Location = new System.Drawing.Point(293, 238);
            this.pbAppIcon.Name = "pbAppIcon";
            this.pbAppIcon.Size = new System.Drawing.Size(32, 32);
            this.pbAppIcon.TabIndex = 34;
            this.pbAppIcon.TabStop = false;
            this.pbAppIcon.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbCopyright);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.tbTradeMark);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.tbCompany);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.tbProduct);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbRevision);
            this.groupBox1.Controls.Add(this.tbBuild);
            this.groupBox1.Controls.Add(this.tbMinor);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbMajor);
            this.groupBox1.Location = new System.Drawing.Point(6, 294);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(518, 0);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PRJ_APP_INFO";
            this.groupBox1.Visible = false;
            // 
            // tbCopyright
            // 
            this.tbCopyright.Location = new System.Drawing.Point(345, 108);
            this.tbCopyright.Name = "tbCopyright";
            this.tbCopyright.Size = new System.Drawing.Size(142, 20);
            this.tbCopyright.TabIndex = 15;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(342, 93);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(96, 13);
            this.label12.TabIndex = 14;
            this.label12.Text = "PRJ_COPYRIGHT";
            // 
            // tbTradeMark
            // 
            this.tbTradeMark.Location = new System.Drawing.Point(181, 108);
            this.tbTradeMark.Name = "tbTradeMark";
            this.tbTradeMark.Size = new System.Drawing.Size(152, 20);
            this.tbTradeMark.TabIndex = 13;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(178, 93);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 13);
            this.label11.TabIndex = 12;
            this.label11.Text = "PRJ_TRADEMARK";
            // 
            // tbCompany
            // 
            this.tbCompany.Location = new System.Drawing.Point(6, 108);
            this.tbCompany.Name = "tbCompany";
            this.tbCompany.Size = new System.Drawing.Size(162, 20);
            this.tbCompany.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 92);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(86, 13);
            this.label10.TabIndex = 10;
            this.label10.Text = "PRJ_COMPANY";
            // 
            // tbProduct
            // 
            this.tbProduct.Location = new System.Drawing.Point(6, 32);
            this.tbProduct.Name = "tbProduct";
            this.tbProduct.Size = new System.Drawing.Size(229, 20);
            this.tbProduct.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(86, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "PRJ_PRODUCT";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(388, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "PRJ_VER_REVISION";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(260, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "PRJ_VER_BUILD";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(131, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "PRJ_VER_MINOR";
            // 
            // tbRevision
            // 
            this.tbRevision.Location = new System.Drawing.Point(389, 70);
            this.tbRevision.Name = "tbRevision";
            this.tbRevision.Size = new System.Drawing.Size(122, 20);
            this.tbRevision.TabIndex = 4;
            this.tbRevision.Text = "0";
            this.tbRevision.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbBuild
            // 
            this.tbBuild.Location = new System.Drawing.Point(261, 70);
            this.tbBuild.Name = "tbBuild";
            this.tbBuild.Size = new System.Drawing.Size(122, 20);
            this.tbBuild.TabIndex = 3;
            this.tbBuild.Text = "0";
            this.tbBuild.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbMinor
            // 
            this.tbMinor.Location = new System.Drawing.Point(133, 70);
            this.tbMinor.Name = "tbMinor";
            this.tbMinor.Size = new System.Drawing.Size(122, 20);
            this.tbMinor.TabIndex = 2;
            this.tbMinor.Text = "0";
            this.tbMinor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "PRJ_VER_MAJOR";
            // 
            // tbMajor
            // 
            this.tbMajor.Location = new System.Drawing.Point(5, 70);
            this.tbMajor.Name = "tbMajor";
            this.tbMajor.Size = new System.Drawing.Size(122, 20);
            this.tbMajor.TabIndex = 0;
            this.tbMajor.Text = "0";
            this.tbMajor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 131);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(103, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "PRJ_OUTPUT_DIR";
            // 
            // cbGenerateXmlDoc
            // 
            this.cbGenerateXmlDoc.AutoSize = true;
            this.cbGenerateXmlDoc.Location = new System.Drawing.Point(6, 229);
            this.cbGenerateXmlDoc.Name = "cbGenerateXmlDoc";
            this.cbGenerateXmlDoc.Size = new System.Drawing.Size(168, 17);
            this.cbGenerateXmlDoc.TabIndex = 37;
            this.cbGenerateXmlDoc.Text = "PRJ_GENERATE_XML_DOC";
            this.cbGenerateXmlDoc.UseVisualStyleBackColor = true;
            // 
            // ProjectProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 324);
            this.Controls.Add(this.cbGenerateXmlDoc);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pbAppIcon);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbAppIcon);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbRunArguments);
            this.Controls.Add(this.btSelectOutpotDirectory);
            this.Controls.Add(this.tbOutputDirectory);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cbDebugRelease);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProjectProperties";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PRJ_PROJECT_PROPERTIES";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProjectProperties_FormClosing);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAppIcon)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbDebugRelease;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbDeletePdb;
        private System.Windows.Forms.CheckBox cbDeleteExe;
        private System.Windows.Forms.TextBox tbOutputDirectory;
        private System.Windows.Forms.Button btSelectOutpotDirectory;
        private System.Windows.Forms.TextBox tbRunArguments;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbAppIcon;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pbAppIcon;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbMajor;
        private System.Windows.Forms.TextBox tbRevision;
        private System.Windows.Forms.TextBox tbBuild;
        private System.Windows.Forms.TextBox tbMinor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbGenerateXmlDoc;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbProduct;
        private System.Windows.Forms.TextBox tbCompany;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbTradeMark;
        private System.Windows.Forms.TextBox tbCopyright;
        //private System.Windows.Forms.TextBox tbTitle;
        //private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
	}
}
