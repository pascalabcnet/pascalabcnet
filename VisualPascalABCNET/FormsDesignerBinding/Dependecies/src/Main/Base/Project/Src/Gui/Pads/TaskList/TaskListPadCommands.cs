﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Windows.Forms;
using ICSharpCode.Core;
using ICSharpCode.Core.WinForms;

namespace ICSharpCode.SharpDevelop.Gui
{
	/// <summary>
	/// Description of SelectScopeCommand
	/// </summary>
	public class SelectScopeCommand : AbstractComboBoxCommand
	{
		private ComboBox comboBox;
		private static string[] viewTypes = new string[] {"Solution", "Project", "All open documents", "Document", "Namespace", "Class/Module"};
		
		protected override void OnOwnerChanged(EventArgs e)
		{
			base.OnOwnerChanged(e);
			ToolBarComboBox toolbarItem = (ToolBarComboBox)base.ComboBox;
			comboBox = toolbarItem.ComboBox;
			SetItems();
			comboBox.SelectedIndex = 0;
			comboBox.SelectedIndexChanged += new EventHandler(ComboBoxSelectedIndexChanged);
		}
		
		void ComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox.SelectedIndex != TaskListPad.Instance.SelectedScopeIndex) {
				TaskListPad.Instance.SelectedScopeIndex = comboBox.SelectedIndex;
			}
		}
		
		void SetItems()
		{
			comboBox.Items.Clear();
			comboBox.Items.AddRange(viewTypes);
		}
		
		public override void Run()
		{
		}
	}
	
	public class ShowTaskListTokenButton : AbstractCheckableMenuCommand
	{
		string token = "";
		
		public ShowTaskListTokenButton(string token)
		{
			this.token = token;
			ToolBarCheckBox cb = new ToolBarCheckBox(token);
			
			cb.CheckedChanged += new EventHandler(cb_CheckedChanged);
			cb.CheckOnClick = true;
			cb.Checked = true;
			
			this.Owner = cb;
		}

		void cb_CheckedChanged(object sender, EventArgs e)
		{
			this.IsChecked = ((ToolBarCheckBox)sender).Checked;
		}
		
		public override bool IsChecked {
			get { return TaskListPad.Instance.DisplayedTokens[token]; }
			set { TaskListPad.Instance.DisplayedTokens[token] = value;
				if (TaskListPad.Instance.IsInitialized)
					TaskListPad.Instance.UpdateItems();
			}
		}
	}
	
	public class TaskListTokensBuilder
	{
		public static ShowTaskListTokenButton[] BuildItems(string[] tokens)
		{
			ShowTaskListTokenButton[] buttons = new ShowTaskListTokenButton[tokens.Length];
			
			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i] = new ShowTaskListTokenButton(tokens[i]);
			}
			
			return buttons;
		}
	}
}
