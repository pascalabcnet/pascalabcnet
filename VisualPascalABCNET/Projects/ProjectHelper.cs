// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
using PascalABCCompiler;

namespace VisualPascalABC
{
	
	public class ProjectFactory
	{
		private PascalABCCompiler.ProjectInfo currentProject;
		private int uid=1;
		
		static ProjectFactory()
		{
			
		}
		
		public PascalABCCompiler.IProjectInfo CurrentProject
		{
			get
			{
				return currentProject;
			}
		}
		
		private static ProjectFactory instance;
		public static ProjectFactory Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ProjectFactory();
				}
				return instance;
			}
		}
		
		public string ProjectDirectory
		{
			get
			{
				return Path.GetDirectoryName(currentProject.Path);
			}
		}

        public PascalABCCompiler.IProjectInfo CreateProject(string projectName, string projectFileName, PascalABCCompiler.ProjectType projectType)
        {
            currentProject = new PascalABCCompiler.ProjectInfo();
            currentProject.name = projectName;
            currentProject.path = projectFileName;
            string dir = Path.GetDirectoryName(projectFileName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            currentProject.include_debug_info = true;

            currentProject.project_type = projectType;
            currentProject.source_files.Add(new PascalABCCompiler.SourceCodeFileInfo(projectName + StringConstants.pascalSourceFileExtension, Path.Combine(dir, projectName + StringConstants.pascalSourceFileExtension)));
            currentProject.references.Add(new PascalABCCompiler.ReferenceInfo("System", "System.dll"));
            if (projectType == PascalABCCompiler.ProjectType.WindowsApp)
            {
                currentProject.references.Add(new PascalABCCompiler.ReferenceInfo("System.Windows.Forms", "System.Windows.Forms.dll"));
                currentProject.references.Add(new PascalABCCompiler.ReferenceInfo("System.Drawing", "System.Drawing.dll"));
                //roman//
                currentProject.references.Add(new PascalABCCompiler.ReferenceInfo("System.Core", "System.Core.dll"));
                currentProject.references.Add(new PascalABCCompiler.ReferenceInfo("System.Data", "System.Data.dll"));
                currentProject.references.Add(new PascalABCCompiler.ReferenceInfo("System.Data.DataSetExtensions", "System.Data.DataSetExtensions.dll"));
                currentProject.references.Add(new PascalABCCompiler.ReferenceInfo("System.Xml", "System.Xml.dll"));
                currentProject.references.Add(new PascalABCCompiler.ReferenceInfo("System.Xml.Linq", "System.Xml.Linq.dll"));
                //roman//
            }
            currentProject.main_file = Path.Combine(dir, projectName + StringConstants.pascalSourceFileExtension);
            currentProject.generate_xml_doc = false;
            currentProject.delete_exe = true;
            currentProject.delete_pdb = true;
            currentProject.major_version = 0;
            currentProject.minor_version = 0;
            currentProject.build_version = 0;
            currentProject.revision_version = 0;
            currentProject.output_directory = dir;
            StreamWriter sw = File.CreateText(Path.Combine(dir, projectName + StringConstants.pascalSourceFileExtension));
            currentProject.output_file_name = projectName + ".exe";
            if (projectType == PascalABCCompiler.ProjectType.ConsoleApp)
            {
                //sw.WriteLine("program "+projectName+";");
                //sw.WriteLine();
                //sw.WriteLine("uses System;");
                //sw.WriteLine();
                sw.WriteLine("begin");
                sw.WriteLine();
                sw.Write("end.");
            }
            else if (projectType == PascalABCCompiler.ProjectType.Library)
            {
                sw.WriteLine("library " + projectName + ";");
                sw.WriteLine();
                //sw.WriteLine("uses System;");
                //sw.WriteLine();
                sw.Write("end.");
                currentProject.output_file_name = projectName + ".dll";
            }
            sw.Close();
            currentProject.Save();
            //VisualPABCSingleton.MainForm.OpenFile(Path.Combine(dir,projectName+".pas"));
            return currentProject;
        }

        public PascalABCCompiler.IProjectInfo OpenProject(string projectFileName)
        {
            PascalABCCompiler.ProjectInfo _currentProject = new PascalABCCompiler.ProjectInfo();
            _currentProject.Load(projectFileName);
            currentProject = _currentProject;
            return currentProject;
        }
		
		public bool ProjectLoaded
		{
			get
			{
				return currentProject != null;
			}
		}
		
		private bool dirty = false;
		public bool Dirty
		{
			get
			{
				return dirty;
			}
			set
			{
				dirty = value;
				if (value == true)
				{
					VisualPABCSingleton.MainForm.SaveAllButtonsEnabled = true;
					WorkbenchServiceFactory.CodeCompletionParserController.SetAllInProjectChanged();
				}
				else
				{
					VisualPABCSingleton.MainForm.SaveAllButtonsEnabled = VisualPABCSingleton.MainForm.AllSaved();
				}
			}
		}

        public PascalABCCompiler.IFileInfo AddSourceFile(string fileName)
        {
            PascalABCCompiler.SourceCodeFileInfo fi = new PascalABCCompiler.SourceCodeFileInfo(fileName, Path.Combine(Path.GetDirectoryName(currentProject.Path), fileName));
            currentProject.source_files.Add(fi);
            Dirty = true;
            return fi;
        }
        
        public void AddNamespaceFileReference(string fileName)
        {
            var text = WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.SourceFilesProvider(currentProject.main_file, PascalABCCompiler.SourceFileOperation.GetText) as string;
            text = "{$includenamespace " + Path.GetFileName(fileName) + "}"+Environment.NewLine + text;
            var doc = WorkbenchServiceFactory.DocumentService.GetDocument(currentProject.main_file);
            if (doc != null)
            {
            	doc.TextEditor.Text = text;
            }
            else
            {
            	File.WriteAllText(currentProject.main_file, text);
            }
        }
        
        public bool hasNamespaceFileReference(string fileName)
        {
            var text = WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.SourceFilesProvider(currentProject.main_file, PascalABCCompiler.SourceFileOperation.GetText) as string;
            return text.IndexOf("{$includenamespace " + Path.GetFileName(fileName) + "}") != -1;
        }

        public void RemoveNamespaceFileReference(string fileName)
        {
            var text = WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.SourceFilesProvider(currentProject.main_file, PascalABCCompiler.SourceFileOperation.GetText) as string;
            text = text.Replace("{$includenamespace " + Path.GetFileName(fileName) + "}" + Environment.NewLine, "");
            var doc = WorkbenchServiceFactory.DocumentService.GetDocument(currentProject.main_file);
            if (doc != null)
            {
                doc.TextEditor.Text = text;
            }
            else
            {
                File.WriteAllText(currentProject.main_file, text);
            }
        }

        public PascalABCCompiler.IReferenceInfo AddReference(string s)
        {
            PascalABCCompiler.ReferenceInfo ri = new PascalABCCompiler.ReferenceInfo(s, s + ".dll");
            if (currentProject.references.FindIndex(r => r.assembly_name == s) == -1)
                currentProject.references.Add(ri);
            Dirty = true;
            return ri;
        }
		
		public void RemoveReference(PascalABCCompiler.IReferenceInfo ri)
		{
			currentProject.RemoveReference(ri);
			Dirty = true;
		}
		
		public void ExcludeFile(PascalABCCompiler.IFileInfo fi)
		{
			currentProject.ExcludeFile(fi);
			Dirty = true;
		}
		
		public void RenameFile(PascalABCCompiler.IFileInfo fi, string new_name)
		{
            bool hasNsReference = hasNamespaceFileReference(fi.Name);
            if (hasNsReference)
                RemoveNamespaceFileReference(fi.Name);
            fi.Name = new_name;
			Dirty = true;
            if (hasNsReference)
                AddNamespaceFileReference(fi.Name);
		}
		
		public string GetUnitFileName()
		{
			return "Unit"+uid++ + StringConstants.pascalSourceFileExtension;
		}

        public string GetFullUnitFileName()
        {
            return Path.Combine(Path.GetDirectoryName(currentProject.path), GetUnitFileName());
        }
		
		public void SaveProject()
		{
			if (currentProject != null)
			{
				currentProject.Save();
				Dirty = false;
			}
		}
		
		public void CloseProject()
		{
			currentProject = null;
			uid = 1;
			Dirty = false;
            GC.Collect();
		}
	}
}
