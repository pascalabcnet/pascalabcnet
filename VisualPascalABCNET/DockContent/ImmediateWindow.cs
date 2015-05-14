/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Pavel
 * Datum: 06.05.2009
 * Zeit: 12:34
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPascalABC
{
	/// <summary>
	/// Description of ImmediateWindow.
	/// </summary>
	public partial class ImmediateWindow : BottomDockContentForm
	{
		public ImmediateWindow(Form1 MainForm):base(MainForm)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			ImmediateConsole.ShowHRuler = false;
			ImmediateConsole.ShowLineNumbers = false;
			ImmediateConsole.MarkForImmediateWindow();
			ImmediateConsole.ShowLineNumbers = true;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
	}
}
