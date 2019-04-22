// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;

namespace PascalABCCompiler
{
	public enum ProjectType
	{
		ConsoleApp,
		Library,
		WindowsApp
	}
	
	public interface IProjectInfo
	{
		string Name
		{
			get;
		}

        string MainFile
        {
            get;
            set;
        }
		
		string Path
		{
			get;
		}

        string ProjectDirectory
        {
            get;
        }

        bool DeleteEXE
        {
            get;
            set;
        }

        bool DeletePDB
        {
            get;
            set;
        }

        string AppIcon
        {
            get;
            set;
        }

        int MajorVersion
        {
            get;
            set;
        }

        int MinorVersion
        {
            get;
            set;
        }

        int BuildVersion
        {
            get;
            set;
        }

        int RevisionVersion
        {
            get;
            set;
        }

        string Product
        {
            get;
            set;
        }

        string Company
        {
            get;
            set;
        }

        string Trademark
        {
            get;
            set;
        }

        string Copyright
        {
            get;
            set;
        }

        string CommandLineArguments
        {
            get;
            set;
        }

        bool IncludeDebugInfo
        {
            get;
            set;
        }

        bool GenerateXMLDoc
        {
            get;
            set;
        }
		
		string OutputFileName
		{
			get;
		}

        string OutputDirectory
        {
            get;
            set;
        }
		
		ProjectType ProjectType
		{
			get;
		}
		
		IFileInfo[] SourceFiles
		{
			get;
		}
		
		IReferenceInfo[] References
		{
			get;
		}
		
		IResourceInfo[] Resources
		{
			get;
		}
		
		bool ContainsSourceFile(string FileName);
	}
	
	public interface IFileInfo
	{
		string Name
		{
			get;
			set;
		}
		
		string Path
		{
			get;
		}
	}
	
	public interface IReferenceInfo
	{
		string AssemblyName
		{
			get;
		}
		
		string FullAssemblyName
		{
			get;
		}
	}
	
	public interface IResourceInfo
	{
		string Name
		{
			get;
		}
	}
}
