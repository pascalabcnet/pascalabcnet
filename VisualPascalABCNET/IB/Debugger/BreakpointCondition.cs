// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPascalABC
{
	/// <summary>
	/// Description of BreakpointCondition.
	/// </summary>
	public partial class BreakpointConditionForm : Form
	{
		public BreakpointConditionForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		public bool IsConditionEnabled
		{
			get
			{
				return this.chbCondtionEnabled.Checked;
			}
			set
			{
				this.chbCondtionEnabled.Checked = value;
			}
		}
		
		public string Condition
		{
			get
			{
				return this.tbCondition.Text;
			}
			set
			{
				this.tbCondition.Text = value;
				this.tbCondition.Select();
			}
		}
		
		public bool IfTrue
		{
			get
			{
				return this.rbTrue.Checked;
			}
			set
			{
				this.rbTrue.Checked = value;
			}
		}
		
		public bool IfChanged
		{
			get
			{
				return this.rbChanged.Checked;
			}
			set
			{
				this.rbChanged.Checked = value;
			}
		}
		
		void ChbCondtionEnabledCheckedChanged(object sender, EventArgs e)
		{
			if (this.chbCondtionEnabled.Checked)
			{
				this.tbCondition.Enabled = true;
				this.rbTrue.Enabled = true;
				this.rbChanged.Enabled = true;
			}
			else
			{
				this.tbCondition.Enabled = false;
				this.rbTrue.Enabled = false;
				this.rbChanged.Enabled = false;
			}
		}
	}
}
