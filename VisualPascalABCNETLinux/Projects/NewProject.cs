// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPascalABC
{
	/// <summary>
	/// Description of NewProject.
	/// </summary>
	public partial class NewProjectForm : Form
	{
		public NewProjectForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			tbProjectName.Text = InstNewProjectName();
			if (!string.IsNullOrEmpty(tbProjectName.Text))
				tbProjectDir.Text = currentDirectory+ Path.DirectorySeparatorChar+tbProjectName.Text;
			this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
		}
		
		private string currentDirectory = Constants.DefaultWorkingDirectory;
		
		public string ProjectName
		{
			get
			{
				return tbProjectName.Text;
			}
		}
		
		public PascalABCCompiler.ProjectType ProjectType
		{
			get
			{
				if (this.lvTemplates.SelectedIndices.Count == 0)
					return PascalABCCompiler.ProjectType.WindowsApp;
				switch (this.lvTemplates.SelectedIndices[0])
				{
					case 0 : return PascalABCCompiler.ProjectType.WindowsApp;
					case 1 : return PascalABCCompiler.ProjectType.ConsoleApp;
					case 2 : return PascalABCCompiler.ProjectType.Library;
				}
				return PascalABCCompiler.ProjectType.WindowsApp;
			}
		}
		
		public string ProjectFileName
		{
			get
			{
				return System.IO.Path.Combine(tbProjectDir.Text,ProjectName+".pabcproj");
			}
		}
		
		private string InstNewProjectName()
		{
			string[] dirs = Directory.GetDirectories(currentDirectory,"Project?*");
			List<int> lst = new List<int>();
			for (int i=0; i<dirs.Length; i++)
			{
				string num = dirs[i].Substring(dirs[i].LastIndexOf("Project")+7);
				int res = 0;
				if (int.TryParse(num, out res))
				{
					lst.Add(res);
				}
			}
			lst.Sort();
			if (lst.Count > 0)
				return "Project"+(lst[lst.Count-1]+1).ToString();
			else
				return "Project1";
		}
		
		void TbProjectNameTextChanged(object sender, EventArgs e)
		{
			tbProjectDir.Text = currentDirectory+Path.DirectorySeparatorChar+tbProjectName.Text;
		}
		
		void NewProjectFormActivated(object sender, EventArgs e)
		{
			//this.lvTemplates.Select();
		
		}
		
		void BtnOkClick(object sender, EventArgs e)
		{
			// TODO: Implement BtnOkClick
		}
		
		void NewProjectFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.DialogResult == DialogResult.Cancel)
				return;
			try
			{
				if (this.lvTemplates.SelectedItems.Count == 0)
				{
					e.Cancel = true;
					MessageBox.Show(Form1StringResources.Get("PROJECT_TEMPLATE_NOT_SELECTED"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				if (string.IsNullOrEmpty(tbProjectDir.Text))
				{
					e.Cancel = true;
					MessageBox.Show(Form1StringResources.Get("PROJECT_DIRECTORY_EMPTY"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				if (string.IsNullOrEmpty(tbProjectName.Text))
				{
					e.Cancel = true;
					MessageBox.Show(Form1StringResources.Get("PROJECT_NAME_EMPTY"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				if (Directory.Exists(tbProjectDir.Text))
				{
					e.Cancel = true;
					MessageBox.Show(string.Format(Form1StringResources.Get("DIRECTORY_ALREADY_EXISTS{0}"), tbProjectDir.Text), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				
				FileInfo fi = new FileInfo(Path.Combine(tbProjectDir.Text,tbProjectName.Text));
			}
			catch(PathTooLongException ex)
			{
				e.Cancel = true;
				MessageBox.Show(string.Format(Form1StringResources.Get("TOO_LONG_FILE_NAME{0}"), Path.Combine(tbProjectDir.Text,tbProjectName.Text)), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				MessageBox.Show(Form1StringResources.Get("ERROR_IN_PROJECT_CREATION"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		}
		
		void BtnProjectDirSelectClick(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				tbProjectDir.Text = folderBrowserDialog1.SelectedPath;
				currentDirectory = folderBrowserDialog1.SelectedPath;
				if (!string.IsNullOrEmpty(tbProjectName.Text))
					tbProjectDir.Text += Path.DirectorySeparatorChar+tbProjectName.Text;
			}
		}
	}
}
