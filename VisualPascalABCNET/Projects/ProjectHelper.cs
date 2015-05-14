using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;

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
			currentProject.source_files.Add(new PascalABCCompiler.SourceCodeFileInfo(projectName+".pas",Path.Combine(dir,projectName+".pas")));
			currentProject.references.Add(new PascalABCCompiler.ReferenceInfo("System","System.dll"));
			if (projectType == PascalABCCompiler.ProjectType.WindowsApp)
			{
				currentProject.references.Add(new PascalABCCompiler.ReferenceInfo("System.Windows.Forms","System.Windows.Forms.dll"));
				currentProject.references.Add(new PascalABCCompiler.ReferenceInfo("System.Drawing","System.Drawing.dll"));
                //roman//
                currentProject.references.Add(new PascalABCCompiler.ReferenceInfo("System.Core", "System.Core.dll"));
                currentProject.references.Add(new PascalABCCompiler.ReferenceInfo("System.Data", "System.Data.dll"));
                currentProject.references.Add(new PascalABCCompiler.ReferenceInfo("System.Data.DataSetExtensions", "System.Data.DataSetExtensions.dll"));
                currentProject.references.Add(new PascalABCCompiler.ReferenceInfo("System.Xml", "System.Xml.dll"));
                currentProject.references.Add(new PascalABCCompiler.ReferenceInfo("System.Xml.Linq", "System.Xml.Linq.dll"));
                //roman//
			}
			currentProject.main_file = Path.Combine(dir,projectName+".pas");
			currentProject.generate_xml_doc = false;
            currentProject.delete_exe = true;
            currentProject.delete_pdb = true;
            currentProject.major_version = 0;
            currentProject.minor_version = 0;
            currentProject.build_version = 0;
            currentProject.revision_version = 0;
			currentProject.output_directory = dir;
			StreamWriter sw = File.CreateText(Path.Combine(dir,projectName+".pas"));
			currentProject.output_file_name = projectName + ".exe";
			if (projectType == PascalABCCompiler.ProjectType.ConsoleApp)
			{
				//sw.WriteLine("program "+projectName+";");
				//sw.WriteLine();
				//sw.WriteLine("uses System;");
				//sw.WriteLine();
				sw.WriteLine("begin");
				sw.WriteLine();
				sw.WriteLine("end.");
			}
			else if (projectType == PascalABCCompiler.ProjectType.Library)
			{
				sw.WriteLine("library "+projectName+";");
				sw.WriteLine();
				//sw.WriteLine("uses System;");
				//sw.WriteLine();
				sw.WriteLine("end.");
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
		
		public PascalABCCompiler.IFileInfo AddSourceFile(string file_name)
		{
			PascalABCCompiler.SourceCodeFileInfo fi = new PascalABCCompiler.SourceCodeFileInfo(file_name,Path.Combine(Path.GetDirectoryName(currentProject.Path),file_name));
			currentProject.source_files.Add(fi);
			Dirty = true;
			return fi;
		}
		
		public PascalABCCompiler.IReferenceInfo AddReference(string s)
		{
			PascalABCCompiler.ReferenceInfo ri = new PascalABCCompiler.ReferenceInfo(s,s+".dll");
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
			fi.Name = new_name;
			Dirty = true;
		}
		
		public string GetUnitFileName()
		{
			return "Unit"+uid++ + ".pas";
		}
		
		public string GetFullUnitFileName()
		{
			return Path.Combine(Path.GetDirectoryName(currentProject.path),GetUnitFileName());
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
		}
	}
}
