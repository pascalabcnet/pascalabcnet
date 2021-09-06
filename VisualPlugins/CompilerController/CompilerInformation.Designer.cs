namespace VisualPascalABCPlugins
{
    partial class CompilerInformation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompilerInformation));
            this.ResetCompilerButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CompilerConsole = new System.Windows.Forms.TextBox();
            this.NoSemantic = new System.Windows.Forms.CheckBox();
            this.CompilerVersion = new System.Windows.Forms.Label();
            this.NoSavePCU = new System.Windows.Forms.CheckBox();
            this.NoCodeGeneration = new System.Windows.Forms.CheckBox();
            this.OnRebuld = new System.Windows.Forms.CheckBox();
            this.NoAddStandartUnits = new System.Windows.Forms.CheckBox();
            this.PluginImage = new System.Windows.Forms.PictureBox();
            this.RunILDASMImage = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.RunDbgCLRImage = new System.Windows.Forms.PictureBox();
            this.NoSkipPCUErrors = new System.Windows.Forms.CheckBox();
            this.NoSkipInternalErrorsIfSyntaxTreeIsCorrupt = new System.Windows.Forms.CheckBox();
            this.NoIncludeDebugInfoInPCU = new System.Windows.Forms.CheckBox();
            this.cbUseStandarParserForInellisense = new System.Windows.Forms.CheckBox();
            this.cbNotUseRemoteCompiler = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.cbRunMono = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.cbUseSemanticForIntellisense = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PluginImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RunILDASMImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RunDbgCLRImage)).BeginInit();
            this.SuspendLayout();
            // 
            // ResetCompilerButton
            // 
            this.ResetCompilerButton.Location = new System.Drawing.Point(198, 299);
            this.ResetCompilerButton.Name = "ResetCompilerButton";
            this.ResetCompilerButton.Size = new System.Drawing.Size(157, 23);
            this.ResetCompilerButton.TabIndex = 0;
            this.ResetCompilerButton.Text = "RESTART_COMPILER";
            this.ResetCompilerButton.UseVisualStyleBackColor = true;
            this.ResetCompilerButton.Click += new System.EventHandler(this.ResetCompilerButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CompilerConsole);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 417);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(379, 216);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "COMPILER_MESSAGES";
            // 
            // CompilerConsole
            // 
            this.CompilerConsole.BackColor = System.Drawing.Color.Black;
            this.CompilerConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CompilerConsole.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CompilerConsole.ForeColor = System.Drawing.Color.Lime;
            this.CompilerConsole.Location = new System.Drawing.Point(3, 16);
            this.CompilerConsole.Multiline = true;
            this.CompilerConsole.Name = "CompilerConsole";
            this.CompilerConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.CompilerConsole.Size = new System.Drawing.Size(373, 197);
            this.CompilerConsole.TabIndex = 0;
            // 
            // NoSemantic
            // 
            this.NoSemantic.AutoSize = true;
            this.NoSemantic.Enabled = false;
            this.NoSemantic.Location = new System.Drawing.Point(3, 207);
            this.NoSemantic.Name = "NoSemantic";
            this.NoSemantic.Size = new System.Drawing.Size(190, 17);
            this.NoSemantic.TabIndex = 3;
            this.NoSemantic.Text = "DISABLE_SEMANTIC_ANALYSIS";
            this.NoSemantic.UseVisualStyleBackColor = true;
            this.NoSemantic.CheckedChanged += new System.EventHandler(this.NoSemantic_CheckedChanged);
            // 
            // CompilerVersion
            // 
            this.CompilerVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.CompilerVersion.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CompilerVersion.ForeColor = System.Drawing.Color.DarkGreen;
            this.CompilerVersion.Location = new System.Drawing.Point(0, 0);
            this.CompilerVersion.Name = "CompilerVersion";
            this.CompilerVersion.Size = new System.Drawing.Size(379, 36);
            this.CompilerVersion.TabIndex = 4;
            this.CompilerVersion.Text = "Version\r\nVersion";
            this.CompilerVersion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // NoSavePCU
            // 
            this.NoSavePCU.AutoSize = true;
            this.NoSavePCU.Enabled = false;
            this.NoSavePCU.Location = new System.Drawing.Point(22, 226);
            this.NoSavePCU.Name = "NoSavePCU";
            this.NoSavePCU.Size = new System.Drawing.Size(145, 17);
            this.NoSavePCU.TabIndex = 2;
            this.NoSavePCU.Text = "DISABLE_PCU_SAVING";
            this.NoSavePCU.UseVisualStyleBackColor = true;
            this.NoSavePCU.CheckedChanged += new System.EventHandler(this.NoSavePCU_CheckedChanged);
            // 
            // NoCodeGeneration
            // 
            this.NoCodeGeneration.AutoSize = true;
            this.NoCodeGeneration.Enabled = false;
            this.NoCodeGeneration.Location = new System.Drawing.Point(22, 246);
            this.NoCodeGeneration.Name = "NoCodeGeneration";
            this.NoCodeGeneration.Size = new System.Drawing.Size(184, 17);
            this.NoCodeGeneration.TabIndex = 5;
            this.NoCodeGeneration.Text = "DISABLE_CODE_GENERATION";
            this.NoCodeGeneration.UseVisualStyleBackColor = true;
            this.NoCodeGeneration.CheckedChanged += new System.EventHandler(this.NoCodeGeneration_CheckedChanged);
            // 
            // OnRebuld
            // 
            this.OnRebuld.AutoSize = true;
            this.OnRebuld.Location = new System.Drawing.Point(3, 45);
            this.OnRebuld.Name = "OnRebuld";
            this.OnRebuld.Size = new System.Drawing.Size(135, 17);
            this.OnRebuld.TabIndex = 6;
            this.OnRebuld.Text = "DISABLE_PCU_READ";
            this.OnRebuld.UseVisualStyleBackColor = true;
            this.OnRebuld.CheckedChanged += new System.EventHandler(this.OnRebuld_CheckedChanged);
            // 
            // NoAddStandartUnits
            // 
            this.NoAddStandartUnits.AutoSize = true;
            this.NoAddStandartUnits.Enabled = false;
            this.NoAddStandartUnits.Location = new System.Drawing.Point(3, 138);
            this.NoAddStandartUnits.Name = "NoAddStandartUnits";
            this.NoAddStandartUnits.Size = new System.Drawing.Size(201, 17);
            this.NoAddStandartUnits.TabIndex = 7;
            this.NoAddStandartUnits.Text = "DISABLE_ADD_STANDART_UNTS";
            this.NoAddStandartUnits.UseVisualStyleBackColor = true;
            this.NoAddStandartUnits.CheckedChanged += new System.EventHandler(this.NoAddStandartUnits_CheckedChanged);
            // 
            // PluginImage
            // 
            this.PluginImage.BackColor = System.Drawing.Color.Magenta;
            this.PluginImage.Image = ((System.Drawing.Image)(resources.GetObject("PluginImage.Image")));
            this.PluginImage.Location = new System.Drawing.Point(261, 219);
            this.PluginImage.Name = "PluginImage";
            this.PluginImage.Size = new System.Drawing.Size(16, 16);
            this.PluginImage.TabIndex = 8;
            this.PluginImage.TabStop = false;
            this.PluginImage.Visible = false;
            this.PluginImage.Click += new System.EventHandler(this.PluginImage_Click);
            // 
            // RunILDASMImage
            // 
            this.RunILDASMImage.BackColor = System.Drawing.Color.Magenta;
            this.RunILDASMImage.Image = ((System.Drawing.Image)(resources.GetObject("RunILDASMImage.Image")));
            this.RunILDASMImage.Location = new System.Drawing.Point(261, 242);
            this.RunILDASMImage.Name = "RunILDASMImage";
            this.RunILDASMImage.Size = new System.Drawing.Size(16, 16);
            this.RunILDASMImage.TabIndex = 10;
            this.RunILDASMImage.TabStop = false;
            this.RunILDASMImage.Visible = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Courier New", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(12, 299);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "GC.Collect()";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // RunDbgCLRImage
            // 
            this.RunDbgCLRImage.BackColor = System.Drawing.Color.Magenta;
            this.RunDbgCLRImage.Image = ((System.Drawing.Image)(resources.GetObject("RunDbgCLRImage.Image")));
            this.RunDbgCLRImage.Location = new System.Drawing.Point(283, 242);
            this.RunDbgCLRImage.Name = "RunDbgCLRImage";
            this.RunDbgCLRImage.Size = new System.Drawing.Size(16, 16);
            this.RunDbgCLRImage.TabIndex = 12;
            this.RunDbgCLRImage.TabStop = false;
            this.RunDbgCLRImage.Visible = false;
            // 
            // NoSkipPCUErrors
            // 
            this.NoSkipPCUErrors.AutoSize = true;
            this.NoSkipPCUErrors.Location = new System.Drawing.Point(3, 67);
            this.NoSkipPCUErrors.Name = "NoSkipPCUErrors";
            this.NoSkipPCUErrors.Size = new System.Drawing.Size(181, 17);
            this.NoSkipPCUErrors.TabIndex = 13;
            this.NoSkipPCUErrors.Text = "DISABLE_SKIP_PCU_ERRORS";
            this.NoSkipPCUErrors.UseVisualStyleBackColor = true;
            this.NoSkipPCUErrors.CheckedChanged += new System.EventHandler(this.NoSkipPCUReadingErrors_CheckedChanged);
            // 
            // NoSkipInternalErrorsIfSyntaxTreeIsCorrupt
            // 
            this.NoSkipInternalErrorsIfSyntaxTreeIsCorrupt.AutoSize = true;
            this.NoSkipInternalErrorsIfSyntaxTreeIsCorrupt.Location = new System.Drawing.Point(3, 115);
            this.NoSkipInternalErrorsIfSyntaxTreeIsCorrupt.Name = "NoSkipInternalErrorsIfSyntaxTreeIsCorrupt";
            this.NoSkipInternalErrorsIfSyntaxTreeIsCorrupt.Size = new System.Drawing.Size(375, 17);
            this.NoSkipInternalErrorsIfSyntaxTreeIsCorrupt.TabIndex = 14;
            this.NoSkipInternalErrorsIfSyntaxTreeIsCorrupt.Text = "DISABLE_SKIP_INTERNALERRORS_IF_SYNTAXTREE_IS_CORRUPT";
            this.NoSkipInternalErrorsIfSyntaxTreeIsCorrupt.UseVisualStyleBackColor = true;
            this.NoSkipInternalErrorsIfSyntaxTreeIsCorrupt.CheckedChanged += new System.EventHandler(this.NoSkipInternalErrorsIfSyntaxTreeIsCorrupt_CheckedChanged);
            // 
            // NoIncludeDebugInfoInPCU
            // 
            this.NoIncludeDebugInfoInPCU.AutoSize = true;
            this.NoIncludeDebugInfoInPCU.Location = new System.Drawing.Point(3, 91);
            this.NoIncludeDebugInfoInPCU.Name = "NoIncludeDebugInfoInPCU";
            this.NoIncludeDebugInfoInPCU.Size = new System.Drawing.Size(217, 17);
            this.NoIncludeDebugInfoInPCU.TabIndex = 15;
            this.NoIncludeDebugInfoInPCU.Text = "DISABLE_INCLUDEDBGINFO_IN_PCU";
            this.NoIncludeDebugInfoInPCU.UseVisualStyleBackColor = true;
            this.NoIncludeDebugInfoInPCU.CheckedChanged += new System.EventHandler(this.NoIncludeDebugInfoInPCU_CheckedChanged);
            // 
            // cbUseStandarParserForInellisense
            // 
            this.cbUseStandarParserForInellisense.AutoSize = true;
            this.cbUseStandarParserForInellisense.Enabled = false;
            this.cbUseStandarParserForInellisense.Location = new System.Drawing.Point(3, 162);
            this.cbUseStandarParserForInellisense.Name = "cbUseStandarParserForInellisense";
            this.cbUseStandarParserForInellisense.Size = new System.Drawing.Size(273, 17);
            this.cbUseStandarParserForInellisense.TabIndex = 16;
            this.cbUseStandarParserForInellisense.Text = "USE_STANDART_PARSER_FOR_INTELLISENCE";
            this.cbUseStandarParserForInellisense.UseVisualStyleBackColor = true;
            this.cbUseStandarParserForInellisense.CheckedChanged += new System.EventHandler(this.cbUseStandarParserForInellisense_CheckedChanged);
            // 
            // cbNotUseRemoteCompiler
            // 
            this.cbNotUseRemoteCompiler.AutoSize = true;
            this.cbNotUseRemoteCompiler.Location = new System.Drawing.Point(3, 185);
            this.cbNotUseRemoteCompiler.Name = "cbNotUseRemoteCompiler";
            this.cbNotUseRemoteCompiler.Size = new System.Drawing.Size(190, 17);
            this.cbNotUseRemoteCompiler.TabIndex = 17;
            this.cbNotUseRemoteCompiler.Text = "NOT_USE_REMOTE_COMPILER";
            this.cbNotUseRemoteCompiler.UseVisualStyleBackColor = true;
            this.cbNotUseRemoteCompiler.CheckedChanged += new System.EventHandler(this.cbNotUseRemoteCompiler_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 341);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 23);
            this.button2.TabIndex = 18;
            this.button2.Text = "Test Intellisense";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2Click);
            // 
            // cbRunMono
            // 
            this.cbRunMono.Location = new System.Drawing.Point(3, 269);
            this.cbRunMono.Name = "cbRunMono";
            this.cbRunMono.Size = new System.Drawing.Size(121, 24);
            this.cbRunMono.TabIndex = 19;
            this.cbRunMono.Text = "RUN_ON_MONO";
            this.cbRunMono.UseVisualStyleBackColor = true;
            this.cbRunMono.CheckedChanged += new System.EventHandler(this.CbRunMonoCheckedChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(198, 341);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(157, 23);
            this.button3.TabIndex = 20;
            this.button3.Text = "Test Formatter";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 378);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(112, 22);
            this.button4.TabIndex = 21;
            this.button4.Text = "Test rename";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // cbUseSemanticForIntellisense
            // 
            this.cbUseSemanticForIntellisense.AutoSize = true;
            this.cbUseSemanticForIntellisense.Location = new System.Drawing.Point(158, 272);
            this.cbUseSemanticForIntellisense.Name = "cbUseSemanticForIntellisense";
            this.cbUseSemanticForIntellisense.Size = new System.Drawing.Size(159, 17);
            this.cbUseSemanticForIntellisense.TabIndex = 22;
            this.cbUseSemanticForIntellisense.Text = "Use semantic for intellisense";
            this.cbUseSemanticForIntellisense.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.cbUseSemanticForIntellisense.UseVisualStyleBackColor = true;
            this.cbUseSemanticForIntellisense.CheckedChanged += new System.EventHandler(this.cbUseSemanticForIntellisense_CheckedChanged);
            // 
            // CompilerInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 633);
            this.Controls.Add(this.cbUseSemanticForIntellisense);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.cbRunMono);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.cbNotUseRemoteCompiler);
            this.Controls.Add(this.cbUseStandarParserForInellisense);
            this.Controls.Add(this.NoIncludeDebugInfoInPCU);
            this.Controls.Add(this.NoSkipInternalErrorsIfSyntaxTreeIsCorrupt);
            this.Controls.Add(this.NoSkipPCUErrors);
            this.Controls.Add(this.RunDbgCLRImage);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.RunILDASMImage);
            this.Controls.Add(this.PluginImage);
            this.Controls.Add(this.NoAddStandartUnits);
            this.Controls.Add(this.OnRebuld);
            this.Controls.Add(this.NoCodeGeneration);
            this.Controls.Add(this.CompilerVersion);
            this.Controls.Add(this.NoSemantic);
            this.Controls.Add(this.NoSavePCU);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ResetCompilerButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CompilerInformation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FORMNAME";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CompilerInformation_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CompilerInformation_FormClosed);
            this.Load += new System.EventHandler(this.CompilerInformation_Load);
            this.Shown += new System.EventHandler(this.CompilerInformation_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PluginImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RunILDASMImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RunDbgCLRImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        private System.Windows.Forms.Button button2;

        #endregion

        private System.Windows.Forms.Button ResetCompilerButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox CompilerConsole;
        private System.Windows.Forms.CheckBox NoSemantic;
        private System.Windows.Forms.Label CompilerVersion;
        private System.Windows.Forms.CheckBox NoSavePCU;
        private System.Windows.Forms.CheckBox NoCodeGeneration;
        private System.Windows.Forms.CheckBox OnRebuld;
        private System.Windows.Forms.CheckBox NoAddStandartUnits;
        public System.Windows.Forms.PictureBox PluginImage;
        public System.Windows.Forms.PictureBox RunILDASMImage;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.PictureBox RunDbgCLRImage;
        private System.Windows.Forms.CheckBox NoSkipPCUErrors;
        private System.Windows.Forms.CheckBox NoSkipInternalErrorsIfSyntaxTreeIsCorrupt;
        private System.Windows.Forms.CheckBox NoIncludeDebugInfoInPCU;
        public System.Windows.Forms.CheckBox cbUseStandarParserForInellisense;
        private System.Windows.Forms.CheckBox cbNotUseRemoteCompiler;
        public System.Windows.Forms.CheckBox cbRunMono;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox cbUseSemanticForIntellisense;
    }
}
