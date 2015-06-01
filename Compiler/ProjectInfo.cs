// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace PascalABCCompiler
{
	public class ProjectInfo : PascalABCCompiler.IProjectInfo
	{
        private static int ProjectFileVersion = 2;
        private string _name;
		private string _path;
		private bool _include_debug_info;
		private bool _generate_xml_doc;
		private string _output_file_name;
		private string _output_directory;
		private string _main_file;
        private bool _delete_exe=true;
        private bool _delete_pdb=true;
        private string _command_line_arguments;
        private string _app_icon;
        private int _major_version;
        private int _minor_version;
        private int _build_version;
        private int _revision_version;
        private string _product;
        private string _company;
        private string _trademark;
        private string _copyright;
		private PascalABCCompiler.ProjectType _project_type;
		private List<SourceCodeFileInfo> _source_files = new List<SourceCodeFileInfo>();
		private List<ReferenceInfo> _references = new List<ReferenceInfo>();
		private List<ResourceInfo> _resources = new List<ResourceInfo>();
		
		public string name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

        public bool delete_exe
        {
            get
            {
                return _delete_exe;
            }
            set
            {
                _delete_exe = value;
            }
        }

        public bool delete_pdb
        {
            get
            {
                return _delete_pdb;
            }
            set
            {
                _delete_pdb = value;
            }
        }

        public string command_line_arguments
        {
            get
            {
                return _command_line_arguments;
            }
            set
            {
                _command_line_arguments = value;
            }
        }

        public string app_icon
        {
            get
            {
                return _app_icon;
            }
            set
            {
                _app_icon = value;
            }
        }

        public int major_version
        {
            get
            {
                return _major_version;
            }
            set
            {
                _major_version = value;
            }
        }

        public int minor_version
        {
            get
            {
                return _minor_version;
            }
            set
            {
                _minor_version = value;
            }
        }

        public int build_version
        {
            get
            {
                return _build_version;
            }
            set
            {
                _build_version = value;
            }
        }

        public int revision_version
        {
            get
            {
                return _revision_version;
            }
            set
            {
                _revision_version = value;
            }
        }

        public string product
        {
            get
            {
                return _product;
            }
            set
            {
                _product = value;
            }
        }

        public string company
        {
            get
            {
                return _company;
            }
            set
            {
                _company = value;
            }
        }

        public string trademark
        {
            get
            {
                return _trademark;
            }
            set
            {
                _trademark = value;
            }
        }

        public string copyright
        {
            get
            {
                return _copyright;
            }
            set
            {
                _copyright = value;
            }
        }

		public string path
		{
			get
			{
				return _path;
			}
			set
			{
				_path = value;
			}
		}
		
		public string main_file
		{
			get
			{
				return _main_file;
			}
			set
			{
				_main_file = value;
			}
		}
		
		public bool include_debug_info
		{
			get
			{
				return _include_debug_info;
			}
			set
			{
				_include_debug_info = value;
			}
		}
		
		public bool generate_xml_doc
		{
			get
			{
				return _generate_xml_doc;
			}
			set
			{
				_generate_xml_doc = value;
			}
		}
		
		public string output_file_name
		{
			get
			{
				return _output_file_name;
			}
			set
			{
				_output_file_name = value;
			}
		}
		
		public string output_directory
		{
			get
			{
				return _output_directory;
			}
			set
			{
				_output_directory = value;
			}
		}
		
		public PascalABCCompiler.ProjectType project_type
		{
			get
			{
				return _project_type;
			}
			set
			{
				_project_type = value;
			}
		}
		
		public List<SourceCodeFileInfo> source_files
		{
			get
			{
				return _source_files;
			}
			set
			{
				_source_files = value;
			}
		}
		
		public List<ReferenceInfo> references
		{
			get
			{
				return _references;
			}
			set
			{
				_references = value;
			}
		}
		
		public List<ResourceInfo> resources
		{
			get
			{
				return _resources;
			}
			set
			{
				_resources = value;
			}
		}
		
		public string Name
		{
			get
			{
				return _name;
			}
		}
		
		public string Path
		{
			get
			{
				return _path;
			}
		}
		
		public string MainFile
		{
			get
			{
				return _main_file;
			}
            set
            {
                _main_file = value;
            }
		}
		
		public bool IncludeDebugInfo
		{
			get
			{
				return _include_debug_info;
			}
            set
            {
                _include_debug_info = value;
            }
		}
		
		public bool GenerateXMLDoc
		{
			get
			{
				return _generate_xml_doc;
			}
            set
            {
                _generate_xml_doc = value;
            }
		}
		
		public string OutputFileName
		{
			get
			{
				return _output_file_name;
			}
		}

        public string ProjectDirectory
        {
            get
            {
                return System.IO.Path.GetDirectoryName(main_file);
            }
        }

		public string OutputDirectory
		{
			get
			{
				return _output_directory;
			}
            set
            {
                _output_directory = value;
            }
		}
		
		public PascalABCCompiler.ProjectType ProjectType
		{
			get
			{
				return _project_type;
			}
		}
		
		public PascalABCCompiler.IFileInfo[] SourceFiles
		{
			get
			{
				return _source_files.ToArray();
			}
		}
		
		public PascalABCCompiler.IReferenceInfo[] References
		{
			get
			{
				return _references.ToArray();
			}
		}
		
		public PascalABCCompiler.IResourceInfo[] Resources
		{
			get
			{
				return _resources.ToArray();
			}
		}
		
		public bool ContainsSourceFile(string FileName)
		{
			foreach (SourceCodeFileInfo src in _source_files)
			{
				if (string.Compare(src.path, FileName,true)==0)
					return true;
			}
			return false;
		}
		
		public void RemoveReference(IReferenceInfo ri)
		{
			this.references.Remove(ri as ReferenceInfo);
		}
		
		public void ExcludeFile(IFileInfo fi)
		{
			this.source_files.Remove(fi as SourceCodeFileInfo);
		}

        public void Save()
        {
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.Indent = true;
            xws.Encoding = System.Text.Encoding.UTF8;
            XmlWriter writer = XmlTextWriter.Create(path, xws);
            writer.WriteStartDocument();
            writer.WriteStartElement("Project");
            writer.WriteAttributeString("FileVersion", Convert.ToString(ProjectFileVersion));
            writer.WriteAttributeString("Name", name);
            //writer.WriteAttributeString("Path", path);
            writer.WriteAttributeString("MainFile", Tools.RelativePathTo(System.IO.Path.GetDirectoryName(path), main_file));
            writer.WriteAttributeString("OutputFileName", output_file_name);
            writer.WriteAttributeString("OutputDirectory", Tools.RelativePathTo(System.IO.Path.GetDirectoryName(path), output_directory));
            writer.WriteAttributeString("ProjectType", Enum.GetName(typeof(ProjectType), project_type));
            writer.WriteAttributeString("IncludeDebugInfo", Convert.ToString(include_debug_info));
            writer.WriteAttributeString("GenerateXMLDoc", Convert.ToString(generate_xml_doc));
            writer.WriteAttributeString("CommandLineArguments", command_line_arguments);
            writer.WriteAttributeString("AppIcon", string.IsNullOrEmpty(app_icon)?app_icon:Tools.RelativePathTo(System.IO.Path.GetDirectoryName(path), app_icon));
            writer.WriteAttributeString("DeleteEXE", Convert.ToString(delete_exe));
            writer.WriteAttributeString("DeletePDB", Convert.ToString(delete_pdb));
            writer.WriteAttributeString("Product", product);
            writer.WriteAttributeString("Company", company);
            writer.WriteAttributeString("Trademark", trademark);
            writer.WriteAttributeString("Copyright", copyright);
            writer.WriteAttributeString("MajorVersion", Convert.ToString(major_version));
            writer.WriteAttributeString("MinorVersion", Convert.ToString(minor_version));
            writer.WriteAttributeString("BuildVersion", Convert.ToString(build_version));
            writer.WriteAttributeString("RevisionVersion", Convert.ToString(revision_version));

            writer.WriteStartElement("Items");
            for (int i = 0; i < _source_files.Count; i++)
            {
                writer.WriteStartElement("Item");
                writer.WriteAttributeString("Type", "SourceFile");
                writer.WriteAttributeString("Name", _source_files[i].name);
                writer.WriteAttributeString("Path", Tools.RelativePathTo(System.IO.Path.GetDirectoryName(path), _source_files[i].path));
                writer.WriteEndElement();
            }
            for (int i = 0; i < _references.Count; i++)
            {
                writer.WriteStartElement("Item");
                writer.WriteAttributeString("Type", "Reference");
                writer.WriteAttributeString("AssemblyName", _references[i].assembly_name);
                writer.WriteAttributeString("FullAssemblyName", _references[i].full_assembly_name);
                writer.WriteEndElement();
            }
            for (int i = 0; i < _resources.Count; i++)
            {
                writer.WriteStartElement("Item");
                writer.WriteAttributeString("Type", "Resource");
                writer.WriteAttributeString("Name", _resources[i].name);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
		
		public void Load(string FileName)
		{
			XmlReaderSettings xrs = new XmlReaderSettings();
		    
			XmlReader reader = XmlTextReader.Create(FileName,xrs);
        	reader.ReadToFollowing("Project");
            string vers = reader.GetAttribute("FileVersion");
            if (vers == null || Convert.ToInt32(vers) != ProjectFileVersion)
                throw new TooOldProjectFileVersion();
        	name = reader.GetAttribute("Name");
            path = FileName;
            string proj_dir = System.IO.Path.GetDirectoryName(FileName);
            //_project_directory = proj_dir;
            //path = reader.GetAttribute("Path");
        	main_file = System.IO.Path.Combine(proj_dir, reader.GetAttribute("MainFile"));
        	output_file_name = reader.GetAttribute("OutputFileName");
        	output_directory = System.IO.Path.Combine(proj_dir, reader.GetAttribute("OutputDirectory"));
        	string s = reader.GetAttribute("ProjectType");
        	project_type = (ProjectType)ProjectType.Parse(typeof(ProjectType),s);
        	include_debug_info = Convert.ToBoolean(reader.GetAttribute("IncludeDebugInfo"));
        	generate_xml_doc = Convert.ToBoolean(reader.GetAttribute("GenerateXMLDoc"));
            command_line_arguments = reader.GetAttribute("CommandLineArguments");
            if (!string.IsNullOrEmpty(reader.GetAttribute("AppIcon")))
                app_icon = System.IO.Path.Combine(proj_dir, reader.GetAttribute("AppIcon"));
            delete_exe = Convert.ToBoolean(reader.GetAttribute("DeleteEXE"));
            delete_pdb = Convert.ToBoolean(reader.GetAttribute("DeletePDB"));
            product = reader.GetAttribute("Product");
            company = reader.GetAttribute("Company");
            trademark = reader.GetAttribute("Trademark");
            copyright = reader.GetAttribute("Copyright");
            major_version = Convert.ToInt32(reader.GetAttribute("MajorVersion"));
            minor_version = Convert.ToInt32(reader.GetAttribute("MinorVersion"));
            build_version = Convert.ToInt32(reader.GetAttribute("BuildVersion"));
            revision_version = Convert.ToInt32(reader.GetAttribute("RevisionVersion"));
            reader.ReadToDescendant("Items");
        	reader.ReadToDescendant("Item");
        	string type = reader.GetAttribute("Type");
        	if (type == "SourceFile")
        	{
        		SourceCodeFileInfo src = new SourceCodeFileInfo();
        		src.name = reader.GetAttribute("Name");
        		src.path = System.IO.Path.Combine(proj_dir, reader.GetAttribute("Path"));
        		source_files.Add(src);
        	}
        	else if (type == "Reference")
        	{
        		ReferenceInfo ref_info = new ReferenceInfo();
        		ref_info.assembly_name = reader.GetAttribute("AssemblyName");
        		ref_info.full_assembly_name = reader.GetAttribute("FullAssemblyName");
        		references.Add(ref_info);
        	}
        	else if (type == "Resource")
        	{
        		ResourceInfo res_info = new ResourceInfo();
        		res_info.name = reader.GetAttribute("Name");
        		resources.Add(res_info);
        	}
        	while (reader.ReadToNextSibling("Item"))
			{
        		type = reader.GetAttribute("Type");
        		if (type == "SourceFile")
        		{
        			SourceCodeFileInfo src = new SourceCodeFileInfo();
        			src.name = reader.GetAttribute("Name");
        			src.path = System.IO.Path.Combine(proj_dir, reader.GetAttribute("Path"));
        			source_files.Add(src);
        		}
        		else if (type == "Reference")
        		{
        			ReferenceInfo ref_info = new ReferenceInfo();
        			ref_info.assembly_name = reader.GetAttribute("AssemblyName");
        			ref_info.full_assembly_name = reader.GetAttribute("FullAssemblyName");
        			references.Add(ref_info);
        		}
        		else if (type == "Resource")
        		{
        			ResourceInfo res_info = new ResourceInfo();
        			res_info.name = reader.GetAttribute("Name");
        			resources.Add(res_info);
        		}
        	}
        	reader.ReadEndElement();
        	reader.ReadEndElement();
        	reader.Close();
		}

        public bool DeleteEXE
        {
            get
            {
                return _delete_exe;
            }
            set
            {
                _delete_exe = value;
            }
        }

        public bool DeletePDB
        {
            get
            {
                return _delete_pdb;
            }
            set
            {
                _delete_pdb = value;
            }
        }

        public string AppIcon
        {
            get
            {
                return _app_icon;
            }
            set
            {
                _app_icon = value;
            }
        }

        public int MajorVersion
        {
            get
            {
                return _major_version;
            }
            set
            {
                _major_version = value;
            }
        }

        public int MinorVersion
        {
            get
            {
                return _minor_version;
            }
            set
            {
                _minor_version = value;
            }
        }

        public int BuildVersion
        {
            get
            {
                return _build_version;
            }
            set
            {
                _build_version = value;
            }
        }

        public int RevisionVersion
        {
            get
            {
                return _revision_version;
            }
            set
            {
                _revision_version = value;
            }
        }

        public string CommandLineArguments
        {
            get
            {
                return _command_line_arguments;
            }
            set
            {
                _command_line_arguments = value;
            }
        }

        public string Product
        {
            get
            {
                return _product;
            }
            set
            {
                _product = value;
            }
        }

        public string Company
        {
            get
            {
                return _company;
            }
            set
            {
                _company = value;
            }
        }

        public string Trademark
        {
            get
            {
                return _trademark;
            }
            set
            {
                _trademark = value;
            }
        }

        public string Copyright
        {
            get
            {
                return _copyright;
            }
            set
            {
                _copyright = value;
            }
        }
    }
	
	public class SourceCodeFileInfo : PascalABCCompiler.IFileInfo
	{
		private string _name;
		private string _path;
		
		public SourceCodeFileInfo()
		{
			
		}
		
		public SourceCodeFileInfo(string name, string path)
		{
			this._name = name;
			this._path = path;
		}
		
		public string name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
                if (_path != null)
                _path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(_path),_name);
			}
		}
		
		public string path
		{
			get
			{
				return _path;
			}
			set
			{
				_path = value;
			}
		}
		
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				name = value;
			}
		}
		
		public string Path
		{
			get
			{
				return _path;
			}
		}
	}
	
	public class ReferenceInfo : PascalABCCompiler.IReferenceInfo
	{
		private string _assembly_name;
		private string _full_assembly_name;
		
		public ReferenceInfo()
		{
			
		}
		
		public ReferenceInfo(string assembly_name, string full_assembly_name)
		{
			this._assembly_name = assembly_name;
			this._full_assembly_name = full_assembly_name;
		}
		
		public string assembly_name
		{
			get
			{
				return _assembly_name;
			}
			set
			{
				_assembly_name = value;
			}
		}
		
		public string full_assembly_name
		{
			get
			{
				return _full_assembly_name;
			}
			set
			{
				_full_assembly_name = value;
			}
		}
		
		public string AssemblyName
		{
			get
			{
				return _assembly_name;
			}
		}
		
		public string FullAssemblyName
		{
			get
			{
				return _full_assembly_name;
			}
		}
	}
	
	public class ResourceInfo : PascalABCCompiler.IResourceInfo
	{
		private string _name;
		
		public string name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}
		
		public string Name
		{
			get
			{
				return _name;
			}
		}
	}

    public class TooOldProjectFileVersion : Exception
    {

    }
}

