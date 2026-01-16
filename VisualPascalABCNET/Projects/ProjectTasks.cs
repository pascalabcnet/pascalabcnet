// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ICSharpCode.FormsDesigner;
using PascalABCCompiler;
using PascalABCCompiler.Parsers;

namespace VisualPascalABC
{
	public class ProjectTask
	{
		public static void AddReferencesToProject(ProjectExplorerForm ProjectExplorerWindow)
		{
			ReferenceForm.Instance.Clear();
			ReferenceForm.Instance.LoadAssemblies();
			if (ReferenceForm.Instance.ShowDialog() == DialogResult.OK)
			{
				if (ReferenceForm.Instance.AssemblyType == AssemblyType.GAC)
				{
					string[] assemblies = ReferenceForm.Instance.GetSelectedGACAssemblies();
					foreach (string s in assemblies)
					{
						PascalABCCompiler.IReferenceInfo ri = ProjectFactory.Instance.AddReference(s);
						ProjectExplorerWindow.AddReferenceNode(ri);
                        try
                        {
                            ToolboxProvider.AddComponentsFromAssembly(PascalABCCompiler.NetHelper.NetHelper.LoadAssembly(Compiler.get_assembly_path(ri.FullAssemblyName, false)));
                        }
                        catch
                        {

                        }

					}
				}
				else if (ReferenceForm.Instance.AssemblyType == AssemblyType.COM)
				{
					string[] assemblies = ReferenceForm.Instance.GetSelectedCOMObjects();
					foreach (string s in assemblies)
					{
						PascalABCCompiler.IReferenceInfo ri = ProjectFactory.Instance.AddReference(s);
						ProjectExplorerWindow.AddReferenceNode(ri);
					}
				}
				else
				{
					string[] assemblies = ReferenceForm.Instance.GetSelectedFileAssemblies();
                    string[] native_dlls = ReferenceForm.Instance.GetNativeDlls();
                    List<string> assm_lst = new List<string>();
                    foreach (string dll in assemblies)
                    {
                        if (string.Compare(dll, Path.Combine(ProjectFactory.Instance.ProjectDirectory, Path.GetFileName(dll)), true) != 0)
                            File.Copy(dll, Path.Combine(ProjectFactory.Instance.ProjectDirectory, Path.GetFileName(dll)), true);
                        string xml = Path.ChangeExtension(dll, ".xml");
                        if (File.Exists(xml))
                        {
                            if (string.Compare(xml, Path.Combine(ProjectFactory.Instance.ProjectDirectory, Path.GetFileName(xml)), true) != 0)
                                File.Copy(xml, Path.Combine(ProjectFactory.Instance.ProjectDirectory, Path.GetFileName(xml)), true);
                        }
                        //assm_lst.Add(Path.GetFileNameWithoutExtension(s));
                        //ProjectExplorerWindow.AddReferenceNode(Path.GetFileNameWithoutExtension(s));
                        try
                        {
                            PascalABCCompiler.NetHelper.NetHelper.LoadAssembly(dll);
                        }
                        catch
                        {
                            continue;
                        }
                        
                        PascalABCCompiler.IReferenceInfo ri = ProjectFactory.Instance.AddReference(Path.GetFileNameWithoutExtension(dll));
                        ProjectExplorerWindow.AddReferenceNode(ri);
                        try
                        {
                            ToolboxProvider.AddComponentsFromAssembly(PascalABCCompiler.NetHelper.NetHelper.LoadAssembly(Compiler.get_assembly_path(Path.Combine(ProjectFactory.Instance.ProjectDirectory, ri.FullAssemblyName), false)));
                        }
                        catch
                        {

                        }
                    }

                    //ProjectFactory.Instance.AddReferences(assm_lst.ToArray());
                    foreach (string ndll in native_dlls)
                    {
                        try
                        {
                            var destndllname = Path.Combine(ProjectFactory.Instance.ProjectDirectory, Path.GetFileName(ndll));
                            File.Copy(ndll, destndllname, true);
                        }
                        catch
                        {
                            
                        }
                    }

                }
			}
		}
		
		public static void NewFile(ProjectExplorerForm ProjectExplorerWindow)
		{
			NewFileForm frm = new NewFileForm();
			Form1StringResources.SetTextForAllControls(frm);
			frm.FileName = ProjectFactory.Instance.GetUnitFileName();
			frm.SetUnitFilter();
			if (frm.ShowDialog() == DialogResult.OK)
			{
				PascalABCCompiler.IFileInfo fi = ProjectFactory.Instance.AddSourceFile(frm.FileName);
				ProjectExplorerWindow.AddSourceFile(fi,false);
				string full_file_name = Path.Combine(Path.GetDirectoryName(ProjectFactory.Instance.CurrentProject.Path),frm.FileName);
                StreamWriter sw = File.CreateText(full_file_name);

                ILanguageInformation languageInfo = Languages.Facade.LanguageProvider.Instance.SelectLanguageByExtension(frm.FileName).LanguageInformation;
                
                if (frm.GetFileFilter() == FileType.Unit)
                {
                    sw.Write(languageInfo.GetUnitTemplate(Path.GetFileNameWithoutExtension(frm.FileName)));
                }
                else
                {
                    sw.WriteLine("namespace "+ProjectFactory.Instance.CurrentProject.Name+";");
                    sw.WriteLine();
                    sw.Write("end.");
                    ProjectFactory.Instance.AddNamespaceFileReference(full_file_name);
                }
				
				sw.Close();
                WorkbenchServiceFactory.FileService.OpenFile(full_file_name, null);
			}
		}
		
		public static void NewForm(ProjectExplorerForm ProjectExplorerWindow, bool prompt) //roman//
		{
			NewFileForm frm = new NewFileForm();
			frm.SetWinFormsFilter();
			Form1StringResources.SetTextForAllControls(frm);
			frm.FileName = ProjectFactory.Instance.GetUnitFileName();
            if (prompt && frm.ShowDialog() != DialogResult.OK) //roman//
                return;
            PascalABCCompiler.IFileInfo fi = ProjectFactory.Instance.AddSourceFile(frm.FileName);
            string full_file_name = Path.Combine(Path.GetDirectoryName(ProjectFactory.Instance.CurrentProject.Path), frm.FileName);            
            StreamWriter sw = File.CreateText(full_file_name);
            sw.Close();

            WorkbenchServiceFactory.FileService.OpenFile(full_file_name, null);
            VisualPABCSingleton.MainForm.CurrentCodeFileDocument.AddDesigner(null);
            ProjectExplorerWindow.AddSourceFile(fi, true);
            VisualPABCSingleton.MainForm.SaveFileAs(VisualPABCSingleton.MainForm.CurrentCodeFileDocument, full_file_name);
		}

        public static void ProjectProperties()
        {
            ProjectProperties pf = new ProjectProperties();
            Form1StringResources.SetTextForAllControls(pf);
            pf.LoadOptions(ProjectFactory.Instance.CurrentProject);
            //pf.SetProperties();
            if (pf.ShowDialog() == DialogResult.OK)
            {
                pf.SetOptions(ProjectFactory.Instance.CurrentProject);
                ProjectFactory.Instance.Dirty = true;
            }
        }

		public static void AddFile(ProjectExplorerForm ProjectExplorerWindow, string file_name)
		{
			PascalABCCompiler.IFileInfo fi = ProjectFactory.Instance.AddSourceFile(file_name);
			ProjectExplorerWindow.AddSourceFile(fi,false);
		}
		
		private static OpenFileDialog open_dlg = null;

        private static void openFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (string FileName in open_dlg.FileNames)
            {
                if (Path.GetDirectoryName(FileName) != ProjectFactory.Instance.CurrentProject.ProjectDirectory)
                {
                    File.Copy(FileName, Path.Combine(ProjectFactory.Instance.CurrentProject.ProjectDirectory, Path.GetFileName(FileName)), true);
                    AddFile(VisualPABCSingleton.MainForm.ProjectExplorerWindow, Path.GetFileName(FileName));
                }
                else
                    AddFile(VisualPABCSingleton.MainForm.ProjectExplorerWindow, Path.GetFileName(FileName));
            }
        }
		
		public static void AddFile(ProjectExplorerForm ProjectExplorerWindow)
		{
			if (open_dlg == null) 
			{
				open_dlg = new OpenFileDialog();
				open_dlg.Multiselect = true;
				open_dlg.FileOk += new System.ComponentModel.CancelEventHandler(openFileDialog_FileOk);
			}
			open_dlg.FileName = "";
			open_dlg.InitialDirectory = ProjectFactory.Instance.ProjectDirectory;
			open_dlg.Filter = VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.GetFilterForDialogs();
			open_dlg.ShowDialog();
		}
		
		public static void RemoveReference(PascalABCCompiler.IReferenceInfo ri)
		{
			ProjectFactory.Instance.RemoveReference(ri);
            ToolboxProvider.RemoveComponentsFromAssembly(GetReflectionAssembly(ri));
		}
		
        private static System.Reflection.Assembly GetReflectionAssembly(IReferenceInfo ri)
        {
            var path = Compiler.get_assembly_path(Path.Combine(ProjectFactory.Instance.ProjectDirectory, ri.FullAssemblyName), false);
            if (path == null)
                path = Compiler.get_assembly_path(ri.FullAssemblyName, false);
            if (path != null)
                return PascalABCCompiler.NetHelper.NetHelper.LoadAssembly(path);
            return null;
        }

		public static void ExcludeFile(PascalABCCompiler.IFileInfo fi)
		{
			ProjectFactory.Instance.ExcludeFile(fi);
			ProjectFactory.Instance.RemoveNamespaceFileReference(fi.Path);
		}
		
		private static TypeLibConverter type_conv = new TypeLibConverter();
		
		public static string GenerateStubForCOM(string com_obj)
		{
			return TypeLibrary.GetManagedWrapperForCOM(com_obj);
		}
	}
}

