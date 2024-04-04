// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
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
