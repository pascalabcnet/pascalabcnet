// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using PascalABCCompiler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VisualPascalABCPlugins;

namespace VisualPascalABC
{
    public partial class Form1 : IWorkbenchProjectService
    {
        public List<string> LastOpenProjects;
        
        public void NewProject()
        {
            NewProjectForm npf = new NewProjectForm();
            Form1StringResources.SetTextForAllControls(npf);

            if (npf.ShowDialog() == DialogResult.OK)
            {
                if (ProjectFactory.Instance.ProjectLoaded)
                {
                    if (!CloseProject())
                        return;
                }
                PascalABCCompiler.IProjectInfo proj = ProjectFactory.Instance.CreateProject(npf.ProjectName, npf.ProjectFileName, npf.ProjectType);
                ProjectExplorerWindow.LoadProject(npf.ProjectName, ProjectFactory.Instance.CurrentProject);
                ProjectExplorerWindow.Show();
                //ShowContent(ProjectExplorerWindow,true);
                CloseFilesAndSaveState();
                ClearAndSaveWatch();
                SelectContent(ProjectExplorerWindow, false);
                WorkbenchServiceFactory.FileService.OpenFile(proj.MainFile, null);
                ActiveCodeFileDocument = CurrentCodeFileDocument;
                AddLastProject(npf.ProjectFileName);
                if (CodeCompletion.CodeCompletionController.comp != null)
                {
                    CodeCompletion.CodeCompletionController.comp.CompilerOptions.CurrentProject = proj;
                }
                this.mRPROJECTToolStripMenuItem.Visible = true;
                this.miCloseProject.Visible = true;
                if (proj.ProjectType == PascalABCCompiler.ProjectType.WindowsApp)
                {
                    //string s = ProjectFactory.Instance.GetFullUnitFileName();
                    //OpenFile(null,s);
                    //CurrentCodeFileDocument.AddDesigner(null);
                    ProjectTask.NewForm(ProjectExplorerWindow, false);//roman//
                    //GenerateMainProgramForDesigner(proj.MainFile);
                    //OpenFile(s,null);
                    //SaveFileAs(CurrentCodeFileDocument,s);
                    //CurrentCodeFileDocument.DesignerAndCodeTabs.SelectedTab = CurrentCodeFileDocument.DesignerPage;
                    //ProjectTask.AddFile(ProjectExplorerWindow,Path.GetFileName(s));
                    SaveAll(false);
                }
            }
        }

        public void OpenProject(string projectFileName)
        {
            if (!File.Exists(projectFileName))
            {
                MessageBox.Show(string.Format(PascalABCCompiler.StringResources.Get("!PROJECT_NOT_FOUND{0}"), projectFileName), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (ProjectFactory.Instance.ProjectLoaded)
            {
                CloseProject();
            }
            try
            {
                PascalABCCompiler.IProjectInfo proj = ProjectFactory.Instance.OpenProject(projectFileName);
                ProjectExplorerWindow.LoadProject(Path.GetFileNameWithoutExtension(projectFileName), ProjectFactory.Instance.CurrentProject);
                //ICSharpCode.FormsDesigner.ToolboxProvider.ReloadSideTabs(false);
                CloseFilesAndSaveState();
                ClearAndSaveWatch();
                this.miProjectExplorer.Visible = true;
                ShowContent(ProjectExplorerWindow, false);
                UserProjectSettings setts = ProjectUserOptionsManager.LoadOptions(projectFileName);
                foreach (IReferenceInfo ri in ProjectFactory.Instance.CurrentProject.References)
                {
                    var path = Compiler.get_assembly_path(Path.Combine(ProjectFactory.Instance.ProjectDirectory, ri.FullAssemblyName), false);
                    if (path == null)
                        path = Compiler.get_assembly_path(ri.FullAssemblyName, false);
                    //ICSharpCode.FormsDesigner.ToolboxProvider.AddComponentsFromAssembly(PascalABCCompiler.NetHelper.NetHelper.LoadAssembly(path));
                }
                if (setts != null)
                {
                    foreach (OpenedFileInfo fi in setts.OpenDocuments)
                    {
                        if (fi.InProject)
                        {
                            string f = Path.Combine(proj.ProjectDirectory, fi.FileName);
                            if (File.Exists(f))
                                WorkbenchServiceFactory.FileService.OpenFile(f, null);
                        }
                        else
                            if (File.Exists(fi.FileName))
                                WorkbenchServiceFactory.FileService.OpenFile(fi.FileName, null);
                        if (fi.CaretLine > 0)
                            CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.Caret.Line = fi.CaretLine;
                        if (fi.CaretColumn > 0)
                            CurrentCodeFileDocument.TextEditor.ActiveTextAreaControl.Caret.Column = fi.CaretColumn;
                    }
                    if (!string.IsNullOrEmpty(setts.CurrentDocument))
                    {
                        if (!setts.CurrentDocumentIsInProject)
                        {
                            if (File.Exists(setts.CurrentDocument))
                                WorkbenchServiceFactory.FileService.OpenFile(setts.CurrentDocument, null);
                        }
                        else
                        {
                            string f = Path.Combine(proj.ProjectDirectory, setts.CurrentDocument);
                            if (File.Exists(f))
                                WorkbenchServiceFactory.FileService.OpenFile(f, null);
                        }
                    }
                    else
                        WorkbenchServiceFactory.FileService.OpenFile(proj.MainFile, null);
                    foreach (WatchExprInfo wi in setts.WatchExprs)
                    {
                        AddVariable(wi.Expression, false);
                    }
                }
                else
                    WorkbenchServiceFactory.FileService.OpenFile(proj.MainFile, null);
                ActiveCodeFileDocument = CurrentCodeFileDocument;
                AddLastProject(projectFileName);
                if (CodeCompletion.CodeCompletionController.comp != null)
                {
                    CodeCompletion.CodeCompletionController.comp.CompilerOptions.CurrentProject = proj;
                }
                this.mRPROJECTToolStripMenuItem.Visible = true;
                this.miCloseProject.Visible = true;
            }
            catch (PascalABCCompiler.TooOldProjectFileVersion)
            {
                MessageBox.Show(Form1StringResources.Get("TOO_OLD_PROJECT_FILE_VERSION"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                MessageBox.Show(Form1StringResources.Get("ERROR_OPEN_PROJECT"), PascalABCCompiler.StringResources.Get("!ERROR"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool CloseProject()
        {
            if (SaveAllInProject() == 2)
                return false;
            BottomTabsVisible = true;
            ProjectUserOptionsManager.SaveOptions(ProjectFactory.Instance.CurrentProject.Path);
            CloseProjectFiles();
            ProjectFactory.Instance.CloseProject();
            ProjectExplorerWindow.Clear();
            HideContent(ProjectExplorerWindow);
            //HideContent(PropertiesWindow);
            //if (ToolBoxWindow != null)
            //    HideContent(ToolBoxWindow);
            this.miProjectExplorer.Visible = false;
            CodeFileDocumentControl tmp = CurrentCodeFileDocument;
            //CloseAllButThis(CurrentCodeFileDocument);
            RestoreFiles();
            RestoreWatch();
            //CloseFile(tmp, false);
            WorkbenchServiceFactory.BuildService.CompilerOptions.ProjectCompiled = false;
            mrDesigner.Visible = false;
            miCloseProject.Visible = false;
            this.mRPROJECTToolStripMenuItem.Visible = false;
            CodeCompletion.CodeCompletionController.comp.CompilerOptions.CurrentProject = null;
            return true;
        }

        public void SaveProject()
        {
            if (ProjectFactory.Instance.Dirty)
            {
                ProjectFactory.Instance.SaveProject();
                UpdateSaveButtonsEnabled();
            }
        }

        internal void CloseProjectFiles()
        {
            CodeFileDocumentControl[] l = new CodeFileDocumentControl[OpenDocuments.Values.Count];
            OpenDocuments.Values.CopyTo(l, 0);

            foreach (CodeFileDocumentControl tp in l)
                if (ProjectFactory.Instance.CurrentProject.ContainsSourceFile(tp.FileName))
                {
                    CloseFile(tp, false);
                }
        }

        internal int SaveAllInProject()
        {
            SaveCanceled = false;
            CodeFileDocumentControl[] l = new CodeFileDocumentControl[OpenDocuments.Values.Count];
            OpenDocuments.Values.CopyTo(l, 0);
            bool changed = false;
            foreach (CodeFileDocumentControl tp in l)
                if (tp.DocumentChanged && !tp.FromMetadata && ProjectFactory.Instance.CurrentProject.ContainsSourceFile(tp.FileName))
                {
                    changed = true;
                    break;
                }
            if (!changed)
                return 1;
            int result = QuestionAndSaveProject();
            if (result == 2)
            {
                return result;
            }
            if (result == 0)
                foreach (CodeFileDocumentControl tp in l)
                    if (tp.DocumentChanged && !tp.FromMetadata)
                        SaveSelFile(tp);
            UpdateSaveButtonsEnabled();
            return result;
        }

        int QuestionAndSaveProject()
        {
            DialogResult result = MessageBox.Show(string.Format(Form1StringResources.Get("SAVE_CHANGES_IN_PROJECT{0}"), ProjectFactory.Instance.CurrentProject.Name), PascalABCCompiler.StringResources.Get("!CONFIRM"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ProjectFactory.Instance.SaveProject();
                UpdateSaveButtonsEnabled();
                return 0;
            }
            else
                if (result == DialogResult.Cancel)
                    return 2;
            return 1;
        }

        bool ReplaceLastProject(string OldFileName, string NewFileName)
        {
            ToolStripMenuItem mI;
            for (int i = 0; i < LastOpenProjects.Count; i++)
                if (LastOpenProjects[i].ToLower() == OldFileName.ToLower())
                {
                    LastOpenProjects.RemoveAt(i);
                    LastOpenProjects.Insert(0, NewFileName);
                    mI = (ToolStripMenuItem)miRecentProjects.DropDownItems[i];
                    miRecentProjects.DropDownItems.RemoveAt(i);
                    mI.Text = GetLastFileCaption(NewFileName);
                    mI.Tag = NewFileName;
                    miRecentProjects.DropDownItems.Insert(0, mI);
                    return true;
                }
            return false;
        }

        public void AddLastProject(string FileName)
        {
            if (Path.GetDirectoryName(FileName) == "") return;
            ToolStripMenuItem mI;
            if (ReplaceLastProject(FileName, FileName)) return;
            if (LastOpenProjects.Count >= MaxLastOpenFiles)
            {
                LastOpenProjects.RemoveAt(LastOpenProjects.Count - 1);
                miRecentProjects.DropDownItems.RemoveAt(miRecentProjects.DropDownItems.Count - 1);
            }
            LastOpenProjects.Insert(0, FileName);
            mI = new ToolStripMenuItem();
            mI.Text = GetLastFileCaption(FileName);
            mI.Tag = FileName;
            miRecentProjects.DropDownItems.Insert(0, mI);
            mI.Click += new System.EventHandler(this.OpenLastProject_ToolStripMenuItem_Click);
        }
    }
}
