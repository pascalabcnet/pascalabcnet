

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace VisualPascalABCPlugins
{
	/// <summary>
	/// Description of HelpBuilderForm.
	/// </summary>
	public partial class HelpBuilderForm : Form
	{
		public HelpBuilderForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			PascalABCCompiler.StringResources.SetTextForAllObjects(this, HelpBuilder_VisualPascalABCPlugin.StringsPrefix);
			this.FileToProjectAdd += FileToProjectAddHandler;
			this.FileFromProjectRemove += FileFromProjectRemovedHandler;
			this.NewProject += NewProjectHandler;
			this.OpenProject += OpenProjectHandler;
			this.SaveProject += SaveProjectHandler;
		}
		
		public IVisualEnvironmentCompiler VisualEnvironmentCompiler;
		public HelpBuilderProject project = new HelpBuilderProject();
		public HelpBuilder builder;
		event Proc FileToProjectAdd;
		event Proc FileFromProjectRemove;
		event Proc NewProject;
		event Proc OpenProject;
		event Proc SaveProject;
		
		public PictureBox PluginImage
		{
			get
			{
				return this.pictureBox1;
			}
		}
		
		delegate void Proc();
		
		void SaveProjectHandler()
		{
			this.Text = PascalABCCompiler.StringResources.Get(HelpBuilder_VisualPascalABCPlugin.StringsPrefix+"FORM_NAME")+" - "+System.IO.Path.GetFileNameWithoutExtension(project.file_name);
		}
		
		void OpenProjectHandler()
		{
			if (project.files.Count == 0)
			{
				SetStartCompilingButtonsEnabled(false);
			}
			else
			{
				SetStartCompilingButtonsEnabled(true);
			}
			this.Text = PascalABCCompiler.StringResources.Get(HelpBuilder_VisualPascalABCPlugin.StringsPrefix+"FORM_NAME")+" - "+System.IO.Path.GetFileNameWithoutExtension(project.file_name);
		}
		
		void FileToProjectAddHandler()
		{
			SetStartCompilingButtonsEnabled(true);
		}
		
		void NewProjectHandler()
		{
			SetStartCompilingButtonsEnabled(false);
		}
		
		void FileFromProjectRemovedHandler()
		{
			if (project.files.Count == 0)
			{
				SetStartCompilingButtonsEnabled(false);
			}
		}
		
		public void CreateBuilder()
		{
			builder = new HelpBuilder(VisualEnvironmentCompiler);
			this.propertyGrid1.SelectedObject = builder.options;
			builder.CompilationProgress += OnCompilationProgress;
			this.project.options = builder.options;
		}
		
		void ClearStatusLine()
		{
			this.toolStripStatusLabel1.Text = "";
			this.statusStrip1.Update();
		}
		
		void OnCompilationProgress(string mes)
		{
			this.toolStripStatusLabel1.Text = mes;
			this.statusStrip1.Update();
		}
		
		void HelpBuilderFormFormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			VisualEnvironmentCompiler.StandartCompiler.OnChangeCompilerState -= new PascalABCCompiler.ChangeCompilerStateEventDelegate(Compiler_OnChangeCompilerState);
            this.Visible = false;
		}
		
		void TsbBuildClick(object sender, EventArgs e)
		{
			// TODO: Implement TsbBuildClick
			builder.Clear();
			if (project.files.Count == 0)
			{
				MessageBox.Show(PascalABCCompiler.StringResources.Get(HelpBuilder_VisualPascalABCPlugin.StringsPrefix+"NO_FILES_MESSAGE"),PascalABCCompiler.StringResources.Get(HelpBuilder_VisualPascalABCPlugin.StringsPrefix+"ERROR"),MessageBoxButtons.OK,MessageBoxIcon.Warning);
				return;
			}
            string[] files = project.files.ToArray();
            //if (files.Length == 1 && files[0].EndsWith(".dll"))
            {
                builder.BuildHelpForAssembly(files);
            }
            /*else
			if (builder.BuildHelp(files) == null)
				ClearStatusLine();*/
		}
		
		internal void Compiler_OnChangeCompilerState(PascalABCCompiler.ICompiler sender, PascalABCCompiler.CompilerState State, string FileName)
        {
            switch (State)
            {
                case PascalABCCompiler.CompilerState.CompilationStarting:              
            		SetStartCompilingButtonsEnabled(false);
                    break;
                case PascalABCCompiler.CompilerState.CompilationFinished:
                    this.builder.parser.SetSemanticTree(sender.SemanticTree);
                    SetStartCompilingButtonsEnabled(true);
                    break;
            }
        }
		
		void SetStartCompilingButtonsEnabled(bool val)
		{
			this.tsbBuild.Enabled = val;
			this.tsbRun.Enabled = val;
		}
		
		internal void Init()
		{
			if (project.closed && lbFiles.Items.Count < 2)
			{
				project.files.Clear();
				string FileName = (string)VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.GetCurrentSourceFileName, null);
				builder.options.OutputCHMFileName = System.IO.Path.ChangeExtension(System.IO.Path.GetFileName(FileName),".chm");
				builder.options.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
				builder.options.CHMBannerText = PascalABCCompiler.StringResources.Get(HelpBuilder_VisualPascalABCPlugin.StringsPrefix+"BANNER_TEXT");
				this.Text = PascalABCCompiler.StringResources.Get(HelpBuilder_VisualPascalABCPlugin.StringsPrefix+"FORM_NAME")+" - "+PascalABCCompiler.StringResources.Get(HelpBuilder_VisualPascalABCPlugin.StringsPrefix+"UNTITLED");
				this.propertyGrid1.Update();
				project.files.Add(FileName);
				lbFiles.Items.Clear();
				lbFiles.Items.Add(System.IO.Path.GetFileName(FileName));
				SetStartCompilingButtonsEnabled(true);
			}
		}
		
		internal void InitNewProject()
		{
			string FileName = (string)VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.GetCurrentSourceFileName, null);
			builder.options.OutputCHMFileName = System.IO.Path.ChangeExtension(System.IO.Path.GetFileName(FileName),".chm");
			builder.options.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
			builder.options.CHMBannerText = PascalABCCompiler.StringResources.Get(HelpBuilder_VisualPascalABCPlugin.StringsPrefix+"BANNER_TEXT");
			this.Text = PascalABCCompiler.StringResources.Get(HelpBuilder_VisualPascalABCPlugin.StringsPrefix+"FORM_NAME")+" - "+PascalABCCompiler.StringResources.Get(HelpBuilder_VisualPascalABCPlugin.StringsPrefix+"UNTITLED");
			this.propertyGrid1.Update();
		}
		
		void HelpBuilderFormShown(object sender, EventArgs e)
		{
			
		}
		
		void ToolStripButton1Click(object sender, EventArgs e)
		{
			// TODO: Implement ToolStripButton1Click
			builder.Clear();
			string output_file = builder.BuildHelp(project.files.ToArray());
			if (output_file != null)
			Help.ShowHelp(null, output_file);
			else
				ClearStatusLine();
		}
		
		void OpenFileDialog1FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			foreach(string FileName in openFileDialog1.FileNames)
			{
				lbFiles.Items.Add(System.IO.Path.GetFileName(FileName));
				project.files.Add(FileName);
			}
			FileToProjectAddHandler();
		}
		
		void btnAddClick(object sender, EventArgs e)
		{
			openFileDialog1.ShowDialog();
		}
		
		void BtnRemoveClick(object sender, EventArgs e)
		{
			int ind = lbFiles.SelectedIndex;
			if (ind == -1)
			if (lbFiles.Items.Count == 1)
			ind = 0;
			else
			return;
			project.files.RemoveAt(ind);
			lbFiles.Items.RemoveAt(ind);
			lbFiles.Update();
			FileFromProjectRemove();
		}
		
		void TsbSaveClick(object sender, EventArgs e)
		{
			if (project.file_name == null)
			{
				if (saveFileDialog1.ShowDialog() == DialogResult.OK)
					project.file_name = saveFileDialog1.FileName;
				else
					return;
			}
			SaveProject();
			project.Save();
		}
		
		void UpdateForm()
		{
			propertyGrid1.SelectedObject = project.options;
			lbFiles.Items.Clear();
			foreach (string s in project.files)
			{
				lbFiles.Items.Add(System.IO.Path.GetFileName(s));
			}
		}
		
		void TsbNewClick(object sender, EventArgs e)
		{
			project.Close();
			UpdateForm();
			InitNewProject();
			NewProject();
		}
		
		void TsbOpenClick(object sender, EventArgs e)
		{
			if (openFileDialog2.ShowDialog()== DialogResult.OK)
			{
				project.Close();
				project.Open(openFileDialog2.FileName);
				UpdateForm();
				OpenProject();
			}
		}
		
		void BtnAddCurrentClick(object sender, EventArgs e)
		{
			string FileName = (string)VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.GetCurrentSourceFileName, null);
			lbFiles.Items.Add(System.IO.Path.GetFileName(FileName));
			project.files.Add(FileName);
			FileToProjectAddHandler();
		}
	}
	
}
