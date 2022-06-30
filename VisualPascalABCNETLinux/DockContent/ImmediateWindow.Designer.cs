/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Pavel
 * Datum: 06.05.2009
 * Zeit: 12:34
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
namespace VisualPascalABC
{
	partial class ImmediateWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImmediateWindow));
            this.ImmediateConsole = new VisualPascalABC.CodeFileDocumentTextEditorControl();
            this.SuspendLayout();
            // 
            // ImmediateConsole
            // 
            this.ImmediateConsole.CaretColumn = 0;
            this.ImmediateConsole.CaretLine = 0;
            this.ImmediateConsole.ConvertTabsToSpaces = true;
            this.ImmediateConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImmediateConsole.Location = new System.Drawing.Point(0, 0);
            this.ImmediateConsole.Name = "ImmediateConsole";
            this.ImmediateConsole.ShowVRuler = false;
            this.ImmediateConsole.Size = new System.Drawing.Size(472, 271);
            this.ImmediateConsole.TabIndent = 4;
            this.ImmediateConsole.TabIndex = 0;
            // 
            // ImmediateWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 271);
            this.Controls.Add(this.ImmediateConsole);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ImmediateWindow";
            this.TabText = "TP_IMMEDIATE_WINDOW";
            this.Text = "TP_IMMEDIATE_WINDOW";
            this.ResumeLayout(false);

		}
		private VisualPascalABC.CodeFileDocumentTextEditorControl ImmediateConsole;
		
	}
}
