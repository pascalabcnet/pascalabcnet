// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Build.BuildEngine;
using PascalABCCompiler;

namespace VisualPascalABC
{
	/// <summary>
	/// Description of ProjectExplorer.
	/// </summary>
	public partial class ProjectExplorerForm : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		public ProjectExplorerForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
            var sc = ScreenScale.Calc();
            if (sc >= 1.99) 
                this.tvProjectExplorer.ImageList = this.imageList2;

            this.tvProjectExplorer.LabelEdit = true;
            this.tvProjectExplorer.BeforeLabelEdit += tvProjectExplorer_BeforeLabelEdit;
			this.Icon = new System.Drawing.Icon(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VisualPascalABC.Resources.PadIcons.ProjectBrowser.ico"));
			pRJBUILDToolStripMenuItem.Image = VisualPABCSingleton.MainForm.BuildImage;
			pRJBUILDALLToolStripMenuItem.Image = VisualPABCSingleton.MainForm.RebuildImage;
			pRJRUNToolStripMenuItem.Image = VisualPABCSingleton.MainForm.RunImage;
            pRJNEWFILEToolStripMenuItem.Image = VisualPABCSingleton.MainForm.NewFileImage;
            pRJOPENToolStripMenuItem.Image = VisualPABCSingleton.MainForm.OpenFileImage;
            pRJFORMToolStripMenuItem.Image = VisualPABCSingleton.MainForm.NewFormImage;
			Form1StringResources.SetTextForAllControls(this.AddReferenceMenuStrip);
			Form1StringResources.SetTextForAllControls(this.ProjectMenuStrip);
			Form1StringResources.SetTextForAllControls(this.ReferenceMenuStrip);
			Form1StringResources.SetTextForAllControls(this.SouceFileMenuStrip);
			this.tvProjectExplorer.AfterLabelEdit += new NodeLabelEditEventHandler(LabelEditFinished);
		}

        private void tvProjectExplorer_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (!source_item_nodes.Contains(e.Node))
                e.CancelEdit = true;
        }
		
		public void Clear()
		{
            if (!this.tvProjectExplorer.IsDisposed)
			this.tvProjectExplorer.Nodes.Clear();
			reference_nodes.Clear();
			reference_item_nodes.Clear();
			project_item_nodes.Clear();
			source_item_nodes.Clear();
		}
		
		class NodeInfo
		{
			public bool IsForm;
			
			public NodeInfo()
			{
				
			}
		}
		
		private Hashtable items = new Hashtable();
		private TreeNode ReferencesNode = null;
		private TreeNode ProjectNode = null;
		private List<TreeNode> reference_nodes = new List<TreeNode>();
		private List<TreeNode> reference_item_nodes = new List<TreeNode>();
		private List<TreeNode> project_item_nodes = new List<TreeNode>();
		private List<TreeNode> source_item_nodes = new List<TreeNode>();
		private const int ProjectImageIndex = 2;
		private const int ClosedReferencesImageIndex = 0;
		private const int OpenReferencesImageIndex = 1;
		private const int ReferenceImageIndex = 3;
		private const int SourceFileImageIndex = 4;
		private const int FormImageIndex = 5;
		
		public void LoadProject(string projectName, PascalABCCompiler.IProjectInfo project)
		{
			TreeNode proj_tn = this.tvProjectExplorer.Nodes.Add(Form1StringResources.Get("MR_PROJECT")+" "+projectName);
			ProjectNode = proj_tn;
			project_item_nodes.Add(proj_tn);
			proj_tn.ImageIndex = ProjectImageIndex;
			proj_tn.SelectedImageIndex = ProjectImageIndex;
			TreeNode ref_tn = proj_tn.Nodes.Add(Form1StringResources.Get("PRJ_PROJECT_REFERENCES"));
			ReferencesNode = ref_tn;
			reference_nodes.Add(ref_tn);
			ref_tn.ImageIndex = ClosedReferencesImageIndex;
			foreach (PascalABCCompiler.IFileInfo fi in project.SourceFiles)
			{
				TreeNode tn = proj_tn.Nodes.Add(fi.Name);
				tn.ImageIndex = SourceFileImageIndex;
				tn.SelectedImageIndex = SourceFileImageIndex;
				items[tn] = fi;
				if (VisualPABCSingleton.MainForm.IsForm(fi.Path))
				{
					tn.ImageIndex = FormImageIndex;
					tn.SelectedImageIndex = FormImageIndex;
					NodeInfo ni = new NodeInfo();
					ni.IsForm = true;
					tn.Tag = ni;
				}
				source_item_nodes.Add(tn);
			}
			foreach (PascalABCCompiler.IReferenceInfo ri in project.References)
			{
				TreeNode tn = ref_tn.Nodes.Add(ri.AssemblyName);
				tn.ImageIndex = ReferenceImageIndex;
				tn.SelectedImageIndex = ReferenceImageIndex;
				items[tn] = ri;
				reference_item_nodes.Add(tn);
			}
			proj_tn.Expand();
		}
		
		public void AddSourceFile(PascalABCCompiler.IFileInfo fi, bool is_form)
		{
			TreeNode tn = ProjectNode.Nodes.Add(fi.Name);
			tn.ImageIndex = SourceFileImageIndex;
			tn.SelectedImageIndex = SourceFileImageIndex;
			items[tn] = fi;
			source_item_nodes.Add(tn);
			if (VisualPABCSingleton.MainForm.IsForm(fi.Path) || is_form)
			{
				tn.ImageIndex = FormImageIndex;
				tn.SelectedImageIndex = FormImageIndex;
				NodeInfo ni = new NodeInfo();
				ni.IsForm = true;
				tn.Tag = ni;
			}
            ProjectFactory.Instance.SaveProject();
		}
		
		void TvProjectExplorerBeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			e.Cancel = false;
		}
		
		void TvProjectExplorerMouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				TreeNode tn = this.tvProjectExplorer.GetNodeAt(e.Location);
				if (tn != null)
				{
					if (source_item_nodes.Contains(tn))
					{
						this.tvProjectExplorer.ContextMenuStrip = this.SouceFileMenuStrip;
						if (tn.Tag != null && (tn.Tag as NodeInfo).IsForm)
							this.pRJToolStripMenuItem.Visible = true;
						else
							this.pRJToolStripMenuItem.Visible = false;
					}
					else if (project_item_nodes.Contains(tn))
					{
						this.tvProjectExplorer.ContextMenuStrip = this.ProjectMenuStrip;
					}
					else if (reference_item_nodes.Contains(tn))
					{
						this.tvProjectExplorer.ContextMenuStrip = this.ReferenceMenuStrip;
					}
					else if (reference_nodes.Contains(tn))
					{
						this.tvProjectExplorer.ContextMenuStrip = this.AddReferenceMenuStrip;
					}
					else
					this.tvProjectExplorer.ContextMenuStrip = null;
				}
				else
				{
					this.tvProjectExplorer.ContextMenuStrip = null;
				}
				this.tvProjectExplorer.SelectedNode = tn;
			}
		}
		
		private void OpenFile()
		{
			object o = items[this.tvProjectExplorer.SelectedNode];
			if (o != null)
			{
				if (o is PascalABCCompiler.IFileInfo)
				{
					PascalABCCompiler.IFileInfo fi = o as PascalABCCompiler.IFileInfo;
					WorkbenchServiceFactory.FileService.OpenFile(fi.Path,null);
				}
			}
		}
		
		private void ViewCode()
		{
			object o = items[this.tvProjectExplorer.SelectedNode];
			if (o != null)
			{
				if (o is PascalABCCompiler.IFileInfo)
				{
					PascalABCCompiler.IFileInfo fi = o as PascalABCCompiler.IFileInfo;
                    WorkbenchServiceFactory.FileService.OpenFile(fi.Path, null);
					if (VisualPABCSingleton.MainForm.CurrentCodeFileDocument.DesignerAndCodeTabs != null)
					{
						VisualPABCSingleton.MainForm.CurrentCodeFileDocument.DesignerAndCodeTabs.SelectedTab =
							VisualPABCSingleton.MainForm.CurrentCodeFileDocument.TextPage;
					}
				}
			}
		}
		
		private void ShowForm()
		{
			object o = items[this.tvProjectExplorer.SelectedNode];
			if (o != null)
			{
				if (o is PascalABCCompiler.IFileInfo)
				{
					PascalABCCompiler.IFileInfo fi = o as PascalABCCompiler.IFileInfo;
                    WorkbenchServiceFactory.FileService.OpenFile(fi.Path, null);
					if (VisualPABCSingleton.MainForm.CurrentCodeFileDocument.DesignerAndCodeTabs != null)
					{
						VisualPABCSingleton.MainForm.CurrentCodeFileDocument.DesignerAndCodeTabs.SelectedTab =
							VisualPABCSingleton.MainForm.CurrentCodeFileDocument.DesignerPage;
					}
				}
			}
		}
		
		void TvProjectExplorerDoubleClick(object sender, EventArgs e)
		{
			OpenFile();
		}
		
		public void AddReferenceNode(PascalABCCompiler.IReferenceInfo ri)
		{
            foreach (TreeNode ttt in ReferencesNode.Nodes)
            {
                if (ttt.Text == ri.AssemblyName)
                    return;
            }
            TreeNode tn = ReferencesNode.Nodes.Add(ri.AssemblyName);
			tn.ImageIndex = ReferenceImageIndex;
			tn.SelectedImageIndex = ReferenceImageIndex;
			reference_item_nodes.Add(tn);
			items[tn] = ri;
            ProjectFactory.Instance.SaveProject();
		}
		
		void PRJADDREFERENCEToolStripMenuItemClick(object sender, EventArgs e)
		{
			ProjectTask.AddReferencesToProject(this);
		}
		
		//build project
        private void pRJBUILDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkbenchServiceFactory.BuildService.StartCompile(false);
        }

		//build all
        private void pRJBUILDALLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkbenchServiceFactory.BuildService.StartCompile(true);
        }
		
        //run project
        private void pRJRUNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkbenchServiceFactory.RunService.Run(true);
        }
		
        //new file
        private void pRJNEWFILEToolStripMenuItem_Click(object sender, EventArgs e)
        {
        	ProjectTask.NewFile(this);
        }
		
        //existing file
        private void pRJEXISTFILEToolStripMenuItem_Click(object sender, EventArgs e)
        {
        	ProjectTask.AddFile(this);
        }
		
        //new form
        private void pRJFORMToolStripMenuItem_Click(object sender, EventArgs e)
        {
        	ProjectTask.NewForm(this, true);//roman//
        }
		
        //add reference
        private void pRJToolStripMenuItem2_Click(object sender, EventArgs e)
        {
        	ProjectTask.AddReferencesToProject(this);
        }

        //svojstva proekta
        private void pRJPROPERTIESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectTask.ProjectProperties();
        }
		
        //udalenie reference
        private void pRJREMOVEToolStripMenuItem_Click(object sender, EventArgs e)
        {
			object o = items[this.tvProjectExplorer.SelectedNode];
			if (o != null)
			{
				if (o is PascalABCCompiler.IReferenceInfo)
				{
					ProjectTask.RemoveReference(o as PascalABCCompiler.IReferenceInfo);
					this.tvProjectExplorer.Nodes.Remove(this.tvProjectExplorer.SelectedNode);
                    ProjectFactory.Instance.SaveProject();
				}
			}
        }
		
        //open file
        private void pRJOPENToolStripMenuItem_Click(object sender, EventArgs e)
        {
        	OpenFile();
        }
		
        //show form
        private void pRJToolStripMenuItem_Click(object sender, EventArgs e)
        {
        	ShowForm();
        }
		
        //view code
        private void pRJVIEWCODEToolStripMenuItem_Click(object sender, EventArgs e)
        {
        	ViewCode();
        }
		
        //exclude from project
        private void pRJEXCLUDEToolStripMenuItem_Click(object sender, EventArgs e)
        {
			object o = items[this.tvProjectExplorer.SelectedNode];
			if (o != null)
			{
				if (o is PascalABCCompiler.IFileInfo)
				{
                    PascalABCCompiler.IFileInfo fi = o as PascalABCCompiler.IFileInfo;
                    if (ProjectFactory.Instance.CurrentProject.MainFile != fi.Path)
                    {
                        ProjectTask.ExcludeFile(fi);
                        this.tvProjectExplorer.Nodes.Remove(this.tvProjectExplorer.SelectedNode);
                        ProjectFactory.Instance.SaveProject();
                    }
                    else
                        MessageBox.Show(Form1StringResources.Get("CANNOT_EXCLUDE_MAIN_FILE"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
        }
		
        //rename file
        private void pRJRENAMEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            object o = items[this.tvProjectExplorer.SelectedNode];
            if (o != null)
            {
                if (o is PascalABCCompiler.IFileInfo)
                {
                    this.tvProjectExplorer.LabelEdit = true;
                    this.tvProjectExplorer.SelectedNode.BeginEdit();
                }
            }
        }
        
        private void LabelEditFinished(object sender, NodeLabelEditEventArgs e)
        {
            this.tvProjectExplorer.LabelEdit = false;
            PascalABCCompiler.IFileInfo fi = items[e.Node] as PascalABCCompiler.IFileInfo;
            if (e.Label == null)
                return;
            if (!PascalABCCompiler.Tools.CheckFileNameValid(e.Label))
            {
                e.CancelEdit = true;
                MessageBox.Show(Form1StringResources.Get("INVALID_SOURCE_FILE_NAME"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.Compare(Path.GetExtension(e.Label), StringConstants.pascalSourceFileExtension, true) != 0)
            {
                e.CancelEdit = true;
                MessageBox.Show(Form1StringResources.Get("INVALID_SOURCE_FILE_EXTENSION"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (File.Exists(Path.Combine(Path.GetDirectoryName(fi.Name),e.Label)))
            {
                e.CancelEdit = true;
                MessageBox.Show(string.Format(Form1StringResources.Get("FILE_ALREADY_EXISTS{0}"), e.Label), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string oldFileName = fi.Path;
            ProjectFactory.Instance.RenameFile(fi, e.Label);
            if (oldFileName == ProjectFactory.Instance.CurrentProject.MainFile)
            {
                ProjectFactory.Instance.CurrentProject.MainFile = System.IO.Path.Combine(ProjectFactory.Instance.CurrentProject.ProjectDirectory,e.Label);
            }
            WorkbenchServiceFactory.FileService.RenameFile(oldFileName, fi.Path);
            CodeCompletionActionsManager.RenameUnit(fi.Path, Path.GetFileNameWithoutExtension(e.Label));
            string oldFileNameWithoutExt = Path.Combine(Path.GetDirectoryName(oldFileName),Path.GetFileNameWithoutExtension(oldFileName));
            string newFilePath = Path.Combine(Path.GetDirectoryName(oldFileName), e.Label);
            if (File.Exists(oldFileNameWithoutExt+".fmabc"))
            {
                string fmabcFullName = Path.Combine(Path.GetDirectoryName(oldFileName), oldFileNameWithoutExt + ".fmabc");
                File.Copy(fmabcFullName, Path.Combine(Path.GetDirectoryName(oldFileName), Path.GetFileNameWithoutExtension(e.Label) + ".fmabc"), true);
                File.Delete(fmabcFullName);  
            }
            ProjectFactory.Instance.SaveProject();
            WorkbenchServiceFactory.FileService.SaveAll(false);
            WorkbenchServiceFactory.FileService.CloseFile(newFilePath);
            WorkbenchServiceFactory.FileService.OpenFile(newFilePath, Path.GetFileName(newFilePath));
            WorkbenchServiceFactory.DesignerService.SetActiveDesignerDirty();
            WorkbenchServiceFactory.DesignerService.GenerateAllDesignersCode();
            /*if (File.Exists(oldFileNameWithoutExt + ".resources"))
            {
                string fmabcFullName = Path.Combine(Path.GetDirectoryName(oldFileName), oldFileNameWithoutExt + ".fmabc");
                File.Copy(fmabcFullName, Path.Combine(Path.GetDirectoryName(e.Label), Path.GetFileNameWithoutExtension(e.Label) + ".fmabc"), true);
                File.Delete(fmabcFullName);
            }*/
        }

        private void ProjectExplorerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            { 
                e.Cancel = true;
                this.Hide();
            }
        }
	}
}
