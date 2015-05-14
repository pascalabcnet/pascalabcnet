namespace ScintillaNET
{
    partial class SnippetChooser
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
            this.components = new System.ComponentModel.Container();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lblSnippet = new System.Windows.Forms.Label();
            this.txtSnippet = new ScintillaNET.Scintilla();
            ((System.ComponentModel.ISupportInitialize)(this.txtSnippet)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSnippet
            // 
            this.lblSnippet.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSnippet.Location = new System.Drawing.Point(0, 1);
            this.lblSnippet.Name = "lblSnippet";
            this.lblSnippet.Size = new System.Drawing.Size(94, 13);
            this.lblSnippet.TabIndex = 6;
            this.lblSnippet.Text = "Choose Snippet";
            this.lblSnippet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSnippet
            // 
            this.txtSnippet.AutoComplete.AutoHide = false;
            this.txtSnippet.AutoComplete.AutomaticLengthEntered = false;
            this.txtSnippet.AutoComplete.CancelAtStart = false;
            this.txtSnippet.AutoComplete.IsCaseSensitive = false;
            this.txtSnippet.AutoComplete.ListString = "";
            this.txtSnippet.BackColor = System.Drawing.Color.LightSteelBlue;
            this.txtSnippet.CallTip.BackColor = System.Drawing.Color.LightSteelBlue;
            this.txtSnippet.CurrentPos = 0;
            this.txtSnippet.DocumentNavigation.IsEnabled = false;
            this.txtSnippet.Folding.IsEnabled = false;
            this.txtSnippet.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSnippet.Location = new System.Drawing.Point(95, 1);
            this.txtSnippet.Margins.Left = 0;
            this.txtSnippet.Margins.Margin1.Width = 0;
            this.txtSnippet.MatchBraces = false;
            this.txtSnippet.Name = "txtSnippet";
            this.txtSnippet.Printing.PageSettings.Color = false;
            this.txtSnippet.Scrolling.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtSnippet.Size = new System.Drawing.Size(177, 196);
            this.txtSnippet.Styles.CallTip.BackColor = System.Drawing.Color.LightSteelBlue;
            this.txtSnippet.TabIndex = 1;
            this.txtSnippet.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSnippet_KeyDown);
            this.txtSnippet.AutoCompleteAccepted += new System.EventHandler<ScintillaNET.AutoCompleteAcceptedEventArgs>(this.txtSnippet_AutoCompleteAccepted);
            this.txtSnippet.DocumentChange += new System.EventHandler<ScintillaNET.NativeScintillaEventArgs>(this.txtSnippet_DocumentChange);
            // 
            // SnippetChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.txtSnippet);
            this.Controls.Add(this.lblSnippet);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Name = "SnippetChooser";
            this.Size = new System.Drawing.Size(285, 17);
            this.Load += new System.EventHandler(this.SnippetChooser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtSnippet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip;
        private Scintilla txtSnippet;
        private System.Windows.Forms.Label lblSnippet;
    }
}
