﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Description;

using ICSharpCode.SharpDevelop.Project;

namespace ICSharpCode.SharpDevelop.Gui.Dialogs.ReferenceDialog.ServiceReference
{
	public class ServiceReferenceGenerator
	{
		IProjectWithServiceReferences project;
		IServiceReferenceFileGenerator fileGenerator;
		IFileSystem fileSystem;
		IActiveTextEditors activeTextEditors;
		string tempAppConfigFileName;
		ServiceReferenceFileName referenceFileName;
		
		public ServiceReferenceGenerator(IProject project)
			: this(new ProjectWithServiceReferences(project))
		{
		}
		
		public ServiceReferenceGenerator(IProjectWithServiceReferences project)
			: this(
				project,
				new ServiceReferenceFileGenerator(),
				new ServiceReferenceFileSystem(),
				new ActiveTextEditors())
		{
		}
		
		public ServiceReferenceGenerator(
			IProjectWithServiceReferences project,
			IServiceReferenceFileGenerator fileGenerator,
			IFileSystem fileSystem,
			IActiveTextEditors activeTextEditors)
		{
			this.project = project;
			this.fileGenerator = fileGenerator;
			this.fileSystem = fileSystem;
			this.activeTextEditors = activeTextEditors;
		}
		
		public ServiceReferenceGeneratorOptions Options {
			get { return fileGenerator.Options; }
			set { fileGenerator.Options = value; }
		}
		
		public event EventHandler<GeneratorCompleteEventArgs> Complete;
		
		void OnComplete(GeneratorCompleteEventArgs e)
		{
			if (Complete != null) {
				Complete(this, e);
			}
		}
		
		public void AddServiceReference()
		{
			referenceFileName = StartProxyFileGeneration();
		}
		
		ServiceReferenceFileName StartProxyFileGeneration()
		{
			ServiceReferenceFileName referenceFileName = project.GetServiceReferenceFileName(fileGenerator.Options.ServiceName);
			CreateFolderForFileIfFolderMissing(referenceFileName.Path);
			
			CreateTempAppConfigFileIfOpenInTextEditor();
			
			Options.OutputFileName = referenceFileName.Path;
			Options.AppConfigFileName = GetAppConfigFileName();
			Options.NoAppConfig = false;
			Options.MergeAppConfig = project.HasAppConfigFile();
			Options.MapProjectLanguage(project.Language);
			Options.GenerateNamespace(project.RootNamespace);
			Options.AddProjectReferencesIfUsingTypesFromProjectReferences(project.GetReferences());
			
			fileGenerator.Complete += ProxyFileGenerationComplete;
			fileGenerator.GenerateProxyFile();
			
			return referenceFileName;
		}
		
		string GetAppConfigFileName()
		{
			if (tempAppConfigFileName != null) {
				return tempAppConfigFileName;
			}
			return project.GetAppConfigFileName();
		}
		
		void CreateTempAppConfigFileIfOpenInTextEditor()
		{
			string appConfigText = activeTextEditors.GetTextForOpenFile(project.GetAppConfigFileName());
			if (appConfigText != null) {
				tempAppConfigFileName = fileSystem.CreateTempFile(appConfigText);
			}
		}
		
		ServiceReferenceMapFileName CreateServiceReferenceMapFile()
		{
			ServiceReferenceMapFileName mapFileName = project.GetServiceReferenceMapFileName(fileGenerator.Options.ServiceName);
			var mapFile = new ServiceReferenceMapFile(mapFileName);
			fileGenerator.GenerateServiceReferenceMapFile(mapFile);
			return mapFileName;
		}
		
		void CreateFolderForFileIfFolderMissing(string fileName)
		{
			string folder = Path.GetDirectoryName(fileName);
			fileSystem.CreateDirectoryIfMissing(folder);
		}
		
		void ProxyFileGenerationComplete(object sender, GeneratorCompleteEventArgs e)
		{
			if (e.IsSuccess) {
				UpdateProjectWithGeneratedServiceReference();
			}
			
			if (tempAppConfigFileName != null) {
				if (e.IsSuccess) {
					UpdateAppConfigInTextEditor();
				}
				DeleteTempAppConfigFile();
			}
			OnComplete(e);
		}
		
		void UpdateProjectWithGeneratedServiceReference()
		{
			ServiceReferenceMapFileName mapFileName = CreateServiceReferenceMapFile();
			project.AddServiceReferenceProxyFile(referenceFileName);
			project.AddServiceReferenceMapFile(mapFileName);
			
			project.AddAssemblyReference("System.Runtime.Serialization");
			project.AddAssemblyReference("System.ServiceModel");
			
			if (!project.HasAppConfigFile()) {
				project.AddAppConfigFile();
			}
			
			project.Save();
		}
		
		void DeleteTempAppConfigFile()
		{
			fileSystem.DeleteFile(tempAppConfigFileName);
		}
		
		void UpdateAppConfigInTextEditor()
		{
			string text = fileSystem.ReadAllFileText(tempAppConfigFileName);
			if (activeTextEditors.IsFileOpen(project.GetAppConfigFileName())) {
				activeTextEditors.UpdateTextForOpenFile(project.GetAppConfigFileName(), text);
			} else {
				fileSystem.WriteAllText(project.GetAppConfigFileName(), text);
			}
		}
		
		public IEnumerable<CheckableAssemblyReference> GetCheckableAssemblyReferences()
		{
			return GetUnsortedCheckableAssemblyReferences()
				.OrderBy(reference => reference.Description)
				.ToArray();
		}
		
		IEnumerable<CheckableAssemblyReference> GetUnsortedCheckableAssemblyReferences()
		{
			foreach (ReferenceProjectItem item in project.GetReferences()) {
				yield return new CheckableAssemblyReference(item);
			}
		}
		
		public void UpdateAssemblyReferences(IEnumerable<CheckableAssemblyReference> references)
		{
			Options.Assemblies.Clear();
			foreach (CheckableAssemblyReference reference in references) {
				if (reference.ItemChecked) {
					Options.Assemblies.Add(reference.GetFileName());
				}
			}
		}
	}
}
