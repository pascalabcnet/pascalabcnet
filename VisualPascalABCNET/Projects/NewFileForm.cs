// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPascalABC
{
    public enum FileType { Unit, Namespace, Form}

	/// <summary>
	/// Description of NewFileForm.
	/// </summary>
	public partial class NewFileForm : Form
	{
		public NewFileForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			foreach (ListViewItem li in lvTemplates.Items)
			{
				if ((string)li.Tag == "0" || (string)li.Tag == "2")
					source_items.Add(li);
				else
					forms_items.Add(li);
			}
		}
		
		private List<ListViewItem> source_items = new List<ListViewItem>();
		private List<ListViewItem> forms_items = new List<ListViewItem>();
		
		public string FileName
		{
			get
			{
				return tbFileName.Text;
			}
			set
			{
				tbFileName.Text = value;
			}
		}
		
		void NewFileFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.DialogResult == DialogResult.Cancel)
				return;
			try
			{
				if (lvTemplates.SelectedItems.Count == 0)
				{
					e.Cancel = true;
					MessageBox.Show(Form1StringResources.Get("FILE_TEMPLATE_NOT_SELECTED"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				if (string.IsNullOrEmpty(tbFileName.Text))
				{
					e.Cancel = true;
					MessageBox.Show(Form1StringResources.Get("FILE_NAME_EMPTY"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				if (File.Exists(Path.Combine(ProjectFactory.Instance.ProjectDirectory,tbFileName.Text)))
				{
					e.Cancel = true;
					MessageBox.Show(string.Format(Form1StringResources.Get("FILE_ALREADY_EXISTS{0}"), tbFileName.Text), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				FileInfo fi = new FileInfo(tbFileName.Text);
			}
			catch(PathTooLongException ex)
			{
				e.Cancel = true;
				MessageBox.Show(string.Format(Form1StringResources.Get("TOO_LONG_FILE_NAME{0}"), tbFileName.Text), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;	
			}
			catch(ArgumentException ex)
			{
				e.Cancel = true;
				MessageBox.Show(Form1StringResources.Get("ERROR_IN_PATH"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;	
			}
			catch(Exception ex)
			{
				e.Cancel = true;
				MessageBox.Show(Form1StringResources.Get("ERROR_IN_FILE_CREATION"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		}
		
		public void SetUnitFilter()
		{
			lvTemplates.Items.Clear();
			foreach (ListViewItem li in source_items)
				lvTemplates.Items.Add(li);
            lvTemplates.Items[0].Selected = true;
		}
		
		public void SetWinFormsFilter()
		{
			lvTemplates.Items.Clear();
			foreach (ListViewItem li in forms_items)
				lvTemplates.Items.Add(li);
            lvTemplates.Items[0].Selected = true;
        }

        public FileType GetFileFilter()
        {
            if ((string)lvTemplates.SelectedItems[0].Tag == "0")
                return FileType.Unit;
            else if ((string)lvTemplates.SelectedItems[0].Tag == "2")
                return FileType.Namespace;
            return FileType.Form;
        }

        public Button CancelButtonCommon
        {
            get
            {
                return btnCancel;
            }
        }
	}
}
