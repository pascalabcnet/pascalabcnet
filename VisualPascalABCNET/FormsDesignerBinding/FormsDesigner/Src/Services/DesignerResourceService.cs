// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 2072 $</version>
// </file>

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Text;

using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.SharpDevelop.Project;

namespace ICSharpCode.FormsDesigner.Services
{
	public class DesignerResourceService : System.ComponentModel.Design.IResourceService , IDisposable
	{
		IDesignerHost host;
        FormsDesignerViewContent formsDesignerViewContent;

		public string FormFileName {
			get {
				return formsDesignerViewContent.FileName;
			}
		}


		// Culture name (or empty string) => Resources
		Dictionary<string, DesignerResourceService.ResourceStorage> resources = new Dictionary<string, DesignerResourceService.ResourceStorage>();
		
        public List<string> ResourcesFileNames
        {
            get
            {
                if (resources != null)
                {
                    List<string> resfn = new List<string>();
                    foreach (KeyValuePair<string, ResourceStorage> entry in resources)
                    {
                        string cultureName = entry.Key;
                        string resourceFileName = CalcResourceFileName(FormFileName, cultureName);
                        if (entry.Value.ContainsData || entry.Value.IsReadFromFile)
                            resfn.Add(resourceFileName);
                    }
                    return resfn;
                }
                return null;
            }
        }

		#region ResourceStorage
		public class ResourceStorage
		{
			MemoryStream stream;
			IResourceWriter writer;
			byte[] buffer;
			
			/// <summary>
			/// true, if the currently stored resource is not empty.
			/// Note that this property is only valid after at least one
			/// of GetReader, GetWriter or Save has been called.
			/// </summary>
			public bool ContainsData {
				get {
					return this.buffer != null;
				}
			}
			
			public void Dispose()
			{
				if (this.stream != null) {
					this.writer.Dispose();
					this.stream.Dispose();
				}
				this.buffer = null;
			}
			
			/// <summary>
			/// Writes the byte array containing the most recent version of the resource
			/// represented by this instance into the private field "buffer" and returns it.
			/// Returns null, if this resource has not been written to yet.
			/// </summary>
			byte[] GetBuffer()
			{
				if (this.stream != null) {
					byte[] buffer = this.stream.ToArray();
					if (buffer.Length > 0) {
						this.writer.Close();
						this.writer.Dispose();
						this.buffer = this.stream.ToArray();
						this.writer = null;
						this.stream.Dispose();
						this.stream = null;
					}
				}
				return this.buffer;
			}
			
			/// <summary>
			/// Returns a new resource reader for this resource based on the most recent
			/// version available (either in memory or on disk).
			/// </summary>

            public bool IsReadFromFile = false;
			public IResourceReader GetReader(string resourceFileName)
			{
				if (this.GetBuffer() == null) {
					if (File.Exists(resourceFileName)) {
                        IsReadFromFile = true;
						return CreateResourceReader(resourceFileName);
					} else {
						return null;
					}
				} else {
					return CreateResourceReader(new MemoryStream(this.buffer, false));
				}
			}
			
			/// <summary>
			/// Returns a new resource writer for this resource.
			/// According to the SDK documentation of IResourceService.GetResourceWriter,
			/// a new writer needs to be returned every time one is requested, discarding any
			/// data written by previously returned writers.
			/// </summary>
			public IResourceWriter GetWriter()
			{
				this.stream = new MemoryStream();
				this.writer = CreateResourceWriter(this.stream);
				return this.writer;
			}

			public void Save(string fileName)
			{
				if (this.GetBuffer() != null) {
					File.WriteAllBytes(fileName, this.buffer);
				}
			}
		}
		#endregion
		
		
		// In ResourceMemoryStreams are stored:
		// Key: Culture name (empty string for invariant culture)
		// Value: ResourceStorage, where the resources are stored
		// Memory streams are cleared, when WriteSerialization will start
		// or File in the editor will be reloaded from the disc and of
		// course in Dispose of the service
		public Dictionary<string, ResourceStorage> Resources
		{
			get {
				return resources;
			}
			set {
				resources = value;
			}
		}
		public IDesignerHost Host {
			get {
				return host;
			}
			set {
				host = value;
			}
		}

		public DesignerResourceService(FormsDesignerViewContent formsDesigner)
		{
            this.formsDesignerViewContent = formsDesigner;
            this.host = formsDesigner.Host;
        }

		static IProject GetProject(string formFileName)
		{
			if (ProjectService.OpenSolution != null && formFileName != null)
				return ProjectService.OpenSolution.FindProjectContainingFile(formFileName);
			else
				return null;
		}
		
		#region System.ComponentModel.Design.IResourceService interface implementation
		public System.Resources.IResourceWriter GetResourceWriter(CultureInfo info)
		{
			try {
				LoggingService.Debug("ResourceWriter requested for culture: " + info.ToString());
				ResourceStorage resourceStorage;
				if (resources.ContainsKey(info.Name)) {
					resourceStorage = resources[info.Name];
				} else {
					resourceStorage = new ResourceStorage();
					resources[info.Name] = resourceStorage;
				}
				return resourceStorage.GetWriter();
			} catch (Exception e) {
				MessageService.ShowError(e.Message);
				return null;
			}
		}

		public System.Resources.IResourceReader GetResourceReader(System.Globalization.CultureInfo info)
		{
			try {
				LoggingService.Debug("ResourceReader requested for culture: "+info.ToString());
				ResourceStorage resourceStorage;
				if (resources != null && resources.ContainsKey(info.Name)) {
					resourceStorage = resources[info.Name];
				} else {
					resourceStorage = new ResourceStorage();
					resources[info.Name] = resourceStorage;
				}
				return resourceStorage.GetReader(CalcResourceFileName(FormFileName, info.Name));
			} catch (Exception e) {
				MessageService.ShowError(e.Message);
				return null;
			}
		}
		#endregion

		public void Save()
		{
			if (resources != null) {
				foreach (KeyValuePair<string, ResourceStorage> entry in resources) {
					string cultureName = entry.Key;
					string resourceFileName = CalcResourceFileName(FormFileName, cultureName);
					FileUtility.ObservedSave(new NamedFileOperationDelegate(entry.Value.Save), resourceFileName, FileErrorPolicy.Inform);
					
					IProject project = GetProject(FormFileName);
					
					// Add this resource file to the project
					if (entry.Value.ContainsData && project != null && !project.IsFileInProject(resourceFileName)) {
						FileProjectItem newFileProjectItem = new FileProjectItem(project, ItemType.EmbeddedResource);
						newFileProjectItem.DependentUpon = Path.GetFileName(FormFileName);
						newFileProjectItem.Include = FileUtility.GetRelativePath(project.Directory, resourceFileName);
						ProjectService.AddProjectItem(project, newFileProjectItem);
						
						PadDescriptor pd = WorkbenchSingleton.Workbench.GetPad(typeof(ProjectBrowserPad));
						FileNode formFileNode = ((ProjectBrowserPad)pd.PadContent).ProjectBrowserControl.FindFileNode(FormFileName);
						if (formFileNode != null) {
							LoggingService.Info("FormFileNode found, adding subitem");
							FileNode fileNode = new FileNode(resourceFileName, FileNodeStatus.BehindFile);
							fileNode.AddTo(formFileNode);
							fileNode.ProjectItem = newFileProjectItem;
						}
						project.Save();
					}
				}
			}
		}

		protected string CalcResourceFileName(string formFileName, string cultureName)
		{
			StringBuilder resourceFileName = null;
			IProject project = GetProject(formFileName);
			
			if (formFileName != null && formFileName != String.Empty) {
				resourceFileName = new StringBuilder(Path.GetDirectoryName(formFileName));
			} else if (project != null) {
				resourceFileName = new StringBuilder(project.Directory);
			} else {
				// required for untitled files. Untitled files should NOT save their resources.
				resourceFileName = new StringBuilder(Path.GetTempPath());
			}
			resourceFileName.Append(Path.DirectorySeparatorChar);
			string sourceFileName = null;
			if (project != null && formFileName != null) {
				// Try to find the source file name by using the project dependencies first.
				FileProjectItem sourceItem = project.FindFile(formFileName);
				if (sourceItem != null && sourceItem.DependentUpon != null && sourceItem.DependentUpon.Length > 0) {
					sourceFileName = Path.GetFileNameWithoutExtension(sourceItem.DependentUpon);
				}
			}
			if (sourceFileName == null) {
				// If the source file name cannot be found using the project dependencies,
				// assume the resource file name to be equal to the current source file name.
				// Remove the ".Designer" part if present.
				sourceFileName = Path.GetFileNameWithoutExtension(formFileName);
				if (sourceFileName != null && sourceFileName.ToLowerInvariant().EndsWith(".designer")) {
					sourceFileName = sourceFileName.Substring(0, sourceFileName.Length - 9);
				}
			}
			resourceFileName.Append(sourceFileName);
			
			if (!string.IsNullOrEmpty(cultureName)) {
				resourceFileName.Append('.');
				resourceFileName.Append(cultureName);
			}
			
            resourceFileName.Append('.');
            resourceFileName.Append(Host.RootComponent.Site.Name);//roman//
            resourceFileName.Append(".resources");
			
			return resourceFileName.ToString();
		}
		
		public void Dispose()
		{
			if (resources != null) {
				foreach (ResourceStorage storage in resources.Values) {
					storage.Dispose();
				}
				resources.Clear();
			}
		}
		
		
		static IResourceReader CreateResourceReader(string fileName)
		{
			return new ResourceReader(fileName);
		}
		
		static IResourceReader CreateResourceReader(Stream stream)
		{
            return new ResourceReader(stream);
		}
		
		static IResourceWriter CreateResourceWriter(Stream stream)
		{
            return new ResourceWriter(stream);
		}
	}
}
