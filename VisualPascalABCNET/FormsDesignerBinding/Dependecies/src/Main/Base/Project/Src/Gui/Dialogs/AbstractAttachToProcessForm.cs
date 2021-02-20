﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Debugging;

namespace ICSharpCode.SharpDevelop.Gui
{
	public partial class AbstractAttachToProcessForm : Form
	{		
		public AbstractAttachToProcessForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			UpdateResourceStrings();
			RefreshProcessList();
		}
		
		public System.Diagnostics.Process Process {
			get { return GetSelectedProcess(); }
		}	

		protected virtual void RefreshProcessList(ListView listView, bool showNonManaged)
		{
		}
		
		void RefreshProcessList()
		{
			RefreshProcessList(listView, showNonManagedCheckBox.Checked);
			
			if (listView.Items.Count > 0) {
				listView.Items[0].Selected = true;
			} else {
				attachButton.Enabled = false;
			}
		}
		
		System.Diagnostics.Process GetSelectedProcess()
		{
			if (listView.SelectedItems.Count > 0) {
				return listView.SelectedItems[0].Tag as System.Diagnostics.Process;
			}
			return null;
		}
		
		void RefreshButtonClick(object sender, EventArgs e)
		{
			RefreshProcessList();
		}
		
		void UpdateResourceStrings()
		{
			Text = StringParser.Parse("${res:ICSharpCode.SharpDevelop.Gui.Dialogs.AttachToProcessForm.Title}");
			
			processColumnHeader.Text = StringParser.Parse("${res:ComponentInspector.AttachDialog.ProcessLabel}");
			processIdColumnHeader.Text = StringParser.Parse("${res:Global.ID}");
			titleColumnHeader.Text = StringParser.Parse("${res:AddIns.HtmlHelp2.Title}");
			                 
			attachButton.Text = StringParser.Parse("${res:ICSharpCode.SharpDevelop.Gui.Dialogs.AttachToProcessForm.AttachButtonText}");
			cancelButton.Text = StringParser.Parse("${res:Global.CancelButtonText}");
			refreshButton.Text = StringParser.Parse("${res:ICSharpCode.SharpDevelop.Gui.Dialogs.AddWebReferenceDialog.RefreshButtonTooltip}");
		}
		
		void ListViewItemActivate(object sender, EventArgs e)
		{
			if (listView.SelectedItems.Count > 0) {
				DialogResult = DialogResult.OK;
				Close();
			}
		}
				
		void ShowNonManagedCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			RefreshProcessList();
		}
		
		void ListViewSelectedIndexChanged(object sender, EventArgs e)
		{
			attachButton.Enabled = listView.SelectedItems.Count > 0;
		}
	}
}
