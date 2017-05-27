// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
namespace NodesGenerator
{
    partial class options_form
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.autoSaveOnExit = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tb_pcu_reader = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_pcu_writer = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.visitor_interface_file_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.namespace_name = new System.Windows.Forms.TextBox();
            this.file_name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.AutoCompletionList = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.browseVersionStorage = new System.Windows.Forms.Button();
            this.versionStorage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(200, 283);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 38;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(325, 283);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 39;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // autoSaveOnExit
            // 
            this.autoSaveOnExit.AutoSize = true;
            this.autoSaveOnExit.Location = new System.Drawing.Point(6, 19);
            this.autoSaveOnExit.Name = "autoSaveOnExit";
            this.autoSaveOnExit.Size = new System.Drawing.Size(113, 17);
            this.autoSaveOnExit.TabIndex = 40;
            this.autoSaveOnExit.Text = "Auto Save On Exit";
            this.autoSaveOnExit.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tb_pcu_reader);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.tb_pcu_writer);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.visitor_interface_file_name);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.namespace_name);
            this.groupBox1.Controls.Add(this.file_name);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(213, 265);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Project Options";
            // 
            // tb_pcu_reader
            // 
            this.tb_pcu_reader.Location = new System.Drawing.Point(6, 230);
            this.tb_pcu_reader.Name = "tb_pcu_reader";
            this.tb_pcu_reader.Size = new System.Drawing.Size(200, 20);
            this.tb_pcu_reader.TabIndex = 47;
            this.tb_pcu_reader.Text = "SyntaxTreeStreamReader";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 213);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 13);
            this.label6.TabIndex = 46;
            this.label6.Text = "Name of pcu-deserializer:";
            // 
            // tb_pcu_writer
            // 
            this.tb_pcu_writer.Location = new System.Drawing.Point(6, 178);
            this.tb_pcu_writer.Name = "tb_pcu_writer";
            this.tb_pcu_writer.Size = new System.Drawing.Size(200, 20);
            this.tb_pcu_writer.TabIndex = 45;
            this.tb_pcu_writer.Text = "SyntaxTreeStreamWriter";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(160, 16);
            this.label5.TabIndex = 44;
            this.label5.Text = "Name of pcu-serializer:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(175, 16);
            this.label4.TabIndex = 43;
            this.label4.Text = "Visitor Interface file name:";
            // 
            // visitor_interface_file_name
            // 
            this.visitor_interface_file_name.Location = new System.Drawing.Point(6, 127);
            this.visitor_interface_file_name.Name = "visitor_interface_file_name";
            this.visitor_interface_file_name.Size = new System.Drawing.Size(200, 20);
            this.visitor_interface_file_name.TabIndex = 42;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 16);
            this.label1.TabIndex = 40;
            this.label1.Text = "Syntax Tree file name:";
            // 
            // namespace_name
            // 
            this.namespace_name.Location = new System.Drawing.Point(6, 79);
            this.namespace_name.Name = "namespace_name";
            this.namespace_name.Size = new System.Drawing.Size(200, 20);
            this.namespace_name.TabIndex = 39;
            // 
            // file_name
            // 
            this.file_name.Location = new System.Drawing.Point(6, 31);
            this.file_name.Name = "file_name";
            this.file_name.Size = new System.Drawing.Size(200, 20);
            this.file_name.TabIndex = 38;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 16);
            this.label2.TabIndex = 41;
            this.label2.Text = "Namespace:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.AutoCompletionList);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.browseVersionStorage);
            this.groupBox2.Controls.Add(this.versionStorage);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.autoSaveOnExit);
            this.groupBox2.Location = new System.Drawing.Point(231, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(358, 265);
            this.groupBox2.TabIndex = 42;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Global Options:";
            // 
            // AutoCompletionList
            // 
            this.AutoCompletionList.Location = new System.Drawing.Point(6, 102);
            this.AutoCompletionList.Multiline = true;
            this.AutoCompletionList.Name = "AutoCompletionList";
            this.AutoCompletionList.Size = new System.Drawing.Size(342, 148);
            this.AutoCompletionList.TabIndex = 45;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 83);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(175, 16);
            this.label7.TabIndex = 44;
            this.label7.Text = "Custom Autocompletion List:";
            // 
            // browseVersionStorage
            // 
            this.browseVersionStorage.Location = new System.Drawing.Point(297, 55);
            this.browseVersionStorage.Name = "browseVersionStorage";
            this.browseVersionStorage.Size = new System.Drawing.Size(51, 23);
            this.browseVersionStorage.TabIndex = 43;
            this.browseVersionStorage.Text = "Browse";
            this.browseVersionStorage.UseVisualStyleBackColor = true;
            this.browseVersionStorage.Click += new System.EventHandler(this.browseVersionStorage_Click);
            // 
            // versionStorage
            // 
            this.versionStorage.Location = new System.Drawing.Point(9, 58);
            this.versionStorage.Name = "versionStorage";
            this.versionStorage.Size = new System.Drawing.Size(282, 20);
            this.versionStorage.TabIndex = 42;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(175, 16);
            this.label3.TabIndex = 41;
            this.label3.Text = "Versions Storage:";
            // 
            // options_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(601, 314);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "options_form";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options...";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.CheckBox autoSaveOnExit;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TextBox tb_pcu_reader;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox tb_pcu_writer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox visitor_interface_file_name;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox namespace_name;
        public System.Windows.Forms.TextBox file_name;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button browseVersionStorage;
        public System.Windows.Forms.TextBox versionStorage;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox AutoCompletionList;
        private System.Windows.Forms.Label label7;
    }
}