// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPascalABC
{
	/// <summary>
	/// Description of AddWatchForm.
	/// </summary>
	public partial class AddWatchForm : Form
	{
		public AddWatchForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		public string EditValue
		{
			get
			{
				return this.tbName.Text;
			}
			set
			{
				this.tbName.Text = value;
				this.tbName.Select();
			}
		}
	}
}
