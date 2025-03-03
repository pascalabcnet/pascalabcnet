namespace Converter
{
    partial class TextFormatterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextFormatterForm));
            this.comboBoxBlockBegin = new System.Windows.Forms.ComboBox();
            this.comboBoxBlockEnd = new System.Windows.Forms.ComboBox();
            this.labelBlockBegin = new System.Windows.Forms.Label();
            this.labelBlockEnd = new System.Windows.Forms.Label();
            this.labelBlockBodySymb = new System.Windows.Forms.Label();
            this.labelBetweenWordsSymb = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.comboBoxBodyBlock = new System.Windows.Forms.ComboBox();
            this.comboBoxBetweenWords = new System.Windows.Forms.ComboBox();
            this.labelBlockBodyCount = new System.Windows.Forms.Label();
            this.labelBetweenWordsCount = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.cbLanguage = new System.Windows.Forms.ComboBox();
            this.ImageOptions = new System.Windows.Forms.PictureBox();
            this.ImageBuild = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBuild)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxBlockBegin
            // 
            this.comboBoxBlockBegin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBlockBegin.FormattingEnabled = true;
            this.comboBoxBlockBegin.Items.AddRange(new object[] {
            "C новой строки",
            "C текущей строки"});
            this.comboBoxBlockBegin.Location = new System.Drawing.Point(177, 33);
            this.comboBoxBlockBegin.Name = "comboBoxBlockBegin";
            this.comboBoxBlockBegin.Size = new System.Drawing.Size(283, 21);
            this.comboBoxBlockBegin.TabIndex = 3;
            // 
            // comboBoxBlockEnd
            // 
            this.comboBoxBlockEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBlockEnd.FormattingEnabled = true;
            this.comboBoxBlockEnd.Items.AddRange(new object[] {
            "C новой строки",
            "C текущей строки"});
            this.comboBoxBlockEnd.Location = new System.Drawing.Point(177, 60);
            this.comboBoxBlockEnd.Name = "comboBoxBlockEnd";
            this.comboBoxBlockEnd.Size = new System.Drawing.Size(283, 21);
            this.comboBoxBlockEnd.TabIndex = 4;
            // 
            // labelBlockBegin
            // 
            this.labelBlockBegin.AutoSize = true;
            this.labelBlockBegin.Location = new System.Drawing.Point(12, 33);
            this.labelBlockBegin.Name = "labelBlockBegin";
            this.labelBlockBegin.Size = new System.Drawing.Size(77, 13);
            this.labelBlockBegin.TabIndex = 5;
            this.labelBlockBegin.Text = "Начало блока";
            // 
            // labelBlockEnd
            // 
            this.labelBlockEnd.AutoSize = true;
            this.labelBlockEnd.Location = new System.Drawing.Point(12, 60);
            this.labelBlockEnd.Name = "labelBlockEnd";
            this.labelBlockEnd.Size = new System.Drawing.Size(71, 13);
            this.labelBlockEnd.TabIndex = 6;
            this.labelBlockEnd.Text = "Конец блока";
            // 
            // labelBlockBodySymb
            // 
            this.labelBlockBodySymb.AutoSize = true;
            this.labelBlockBodySymb.Location = new System.Drawing.Point(12, 92);
            this.labelBlockBodySymb.Name = "labelBlockBodySymb";
            this.labelBlockBodySymb.Size = new System.Drawing.Size(110, 13);
            this.labelBlockBodySymb.TabIndex = 7;
            this.labelBlockBodySymb.Text = "Отступ в теле блока";
            this.labelBlockBodySymb.Click += new System.EventHandler(this.labelBlockBody_Click);
            // 
            // labelBetweenWordsSymb
            // 
            this.labelBetweenWordsSymb.AutoSize = true;
            this.labelBetweenWordsSymb.Location = new System.Drawing.Point(12, 117);
            this.labelBetweenWordsSymb.Name = "labelBetweenWordsSymb";
            this.labelBetweenWordsSymb.Size = new System.Drawing.Size(125, 13);
            this.labelBetweenWordsSymb.TabIndex = 8;
            this.labelBetweenWordsSymb.Text = "Отступ между словами";
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(304, 150);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 9;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(385, 150);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // comboBoxBodyBlock
            // 
            this.comboBoxBodyBlock.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBodyBlock.FormattingEnabled = true;
            this.comboBoxBodyBlock.Items.AddRange(new object[] {
            "Пробел"});
            this.comboBoxBodyBlock.Location = new System.Drawing.Point(177, 87);
            this.comboBoxBodyBlock.Name = "comboBoxBodyBlock";
            this.comboBoxBodyBlock.Size = new System.Drawing.Size(121, 21);
            this.comboBoxBodyBlock.TabIndex = 11;
            // 
            // comboBoxBetweenWords
            // 
            this.comboBoxBetweenWords.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBetweenWords.FormattingEnabled = true;
            this.comboBoxBetweenWords.Items.AddRange(new object[] {
            "Пробел"});
            this.comboBoxBetweenWords.Location = new System.Drawing.Point(177, 114);
            this.comboBoxBetweenWords.Name = "comboBoxBetweenWords";
            this.comboBoxBetweenWords.Size = new System.Drawing.Size(121, 21);
            this.comboBoxBetweenWords.TabIndex = 12;
            this.comboBoxBetweenWords.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // labelBlockBodyCount
            // 
            this.labelBlockBodyCount.AutoSize = true;
            this.labelBlockBodyCount.Location = new System.Drawing.Point(313, 94);
            this.labelBlockBodyCount.Name = "labelBlockBodyCount";
            this.labelBlockBodyCount.Size = new System.Drawing.Size(66, 13);
            this.labelBlockBodyCount.TabIndex = 13;
            this.labelBlockBodyCount.Text = "Количество";
            // 
            // labelBetweenWordsCount
            // 
            this.labelBetweenWordsCount.AutoSize = true;
            this.labelBetweenWordsCount.Location = new System.Drawing.Point(313, 122);
            this.labelBetweenWordsCount.Name = "labelBetweenWordsCount";
            this.labelBetweenWordsCount.Size = new System.Drawing.Size(66, 13);
            this.labelBetweenWordsCount.TabIndex = 14;
            this.labelBetweenWordsCount.Text = "Количество";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(395, 87);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(65, 20);
            this.numericUpDown1.TabIndex = 15;
            this.numericUpDown1.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(395, 115);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(65, 20);
            this.numericUpDown2.TabIndex = 16;
            this.numericUpDown2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Язык";
            // 
            // cbLanguage
            // 
            this.cbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLanguage.FormattingEnabled = true;
            this.cbLanguage.Items.AddRange(new object[] {
            "C новой строки",
            "C текущей строки"});
            this.cbLanguage.Location = new System.Drawing.Point(177, 6);
            this.cbLanguage.Name = "cbLanguage";
            this.cbLanguage.Size = new System.Drawing.Size(283, 21);
            this.cbLanguage.TabIndex = 18;
            this.cbLanguage.SelectedIndexChanged += new System.EventHandler(this.cbLanguage_SelectedIndexChanged);
            // 
            // ImageOptions
            // 
            this.ImageOptions.BackColor = System.Drawing.Color.Magenta;
            this.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ImageOptions.Image")));
            this.ImageOptions.Location = new System.Drawing.Point(88, 150);
            this.ImageOptions.Name = "ImageOptions";
            this.ImageOptions.Size = new System.Drawing.Size(16, 16);
            this.ImageOptions.TabIndex = 19;
            this.ImageOptions.TabStop = false;
            this.ImageOptions.Visible = false;
            // 
            // ImageBuild
            // 
            this.ImageBuild.BackColor = System.Drawing.Color.Magenta;
            this.ImageBuild.Image = ((System.Drawing.Image)(resources.GetObject("ImageBuild.Image")));
            this.ImageBuild.Location = new System.Drawing.Point(110, 150);
            this.ImageBuild.Name = "ImageBuild";
            this.ImageBuild.Size = new System.Drawing.Size(16, 16);
            this.ImageBuild.TabIndex = 20;
            this.ImageBuild.TabStop = false;
            this.ImageBuild.Visible = false;
            // 
            // TextFormatterForm
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(475, 184);
            this.Controls.Add(this.ImageBuild);
            this.Controls.Add(this.ImageOptions);
            this.Controls.Add(this.cbLanguage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.labelBetweenWordsCount);
            this.Controls.Add(this.labelBlockBodyCount);
            this.Controls.Add(this.comboBoxBetweenWords);
            this.Controls.Add(this.comboBoxBodyBlock);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.labelBetweenWordsSymb);
            this.Controls.Add(this.labelBlockBodySymb);
            this.Controls.Add(this.labelBlockEnd);
            this.Controls.Add(this.labelBlockBegin);
            this.Controls.Add(this.comboBoxBlockEnd);
            this.Controls.Add(this.comboBoxBlockBegin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TextFormatterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Форматирование";
            this.Shown += new System.EventHandler(this.TextFormatterForm_Shown);
            this.Load += new System.EventHandler(this.TextFormatterForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBuild)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxBlockBegin;
        private System.Windows.Forms.ComboBox comboBoxBlockEnd;
        private System.Windows.Forms.Label labelBlockBegin;
        private System.Windows.Forms.Label labelBlockEnd;
        private System.Windows.Forms.Label labelBlockBodySymb;
        private System.Windows.Forms.Label labelBetweenWordsSymb;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ComboBox comboBoxBodyBlock;
        private System.Windows.Forms.ComboBox comboBoxBetweenWords;
        private System.Windows.Forms.Label labelBlockBodyCount;
        private System.Windows.Forms.Label labelBetweenWordsCount;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbLanguage;
        public System.Windows.Forms.PictureBox ImageOptions;
        public System.Windows.Forms.PictureBox ImageBuild;
    }
}