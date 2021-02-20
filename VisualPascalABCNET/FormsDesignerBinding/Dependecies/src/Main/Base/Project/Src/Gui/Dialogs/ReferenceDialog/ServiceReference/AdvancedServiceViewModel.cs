﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Imaging;

using ICSharpCode.Core.Presentation;
using ICSharpCode.SharpDevelop.Widgets;

namespace ICSharpCode.SharpDevelop.Gui.Dialogs.ReferenceDialog.ServiceReference
{
	public enum Modifiers
	{
		//[Description("${res:Dialog.ProjectOptions.RunPostBuildEvent.Always}")]
		Public,
		//[Description("${res:Dialog.ProjectOptions.RunPostBuildEvent.OnOutputUpdated}")]
		Internal
	}
	
	public enum CollectionTypes
	{
		[Description("System.Array")]
		Array,
		[Description("System.Collections.ArrayList")]
		ArrayList,
		[Description("System.Collections.Generic.LinkedList")]
		LinkedList,
		[Description("System.Collections.Generic.List")]
		List,
		[Description("System.Collections.ObjectModel.Collection")]
		Collection,
		[Description("System.Collections.ObjectModel.ObservableCollection")]
		ObservableCollection,
		[Description("System.ComponentModel.BindingList")]
		BindingList
	}
	
	public enum DictionaryCollectionTypes
	{
		[Description("System.Collections.Generic.Dictionary")]
		Dictionary,
		[Description("System.Collections.Generic.SortedList")]
		SortedList,
		[Description("System.Collections.Generic.SortedDictionary")]
		SortedDictionary,
		[Description("System.Collections.Hashtable")]
		HashTable,
		[Description("System.Collections.ObjectModel.KeyedCollection")]
		KeyedCollection,
		[Description("System.Collections.SortedList")]
		SortedList_2,
		[Description("System.Collections.Specialized.HybridDictionary")]
		HybridDictionary,
		[Description("System.Collections.Specialized.ListDictionary")]
		ListDictionary,
		[Description("System.Collections.Specialized.OrderedDictionary")]
		OrderedDictionary
	}	
	
	internal class AdvancedServiceViewModel : ViewModelBase
	{
		string accesslevel = "Access level for generated classes:";
		ServiceReferenceGeneratorOptions options;
		
		public AdvancedServiceViewModel(ServiceReferenceGeneratorOptions options)
		{
			this.options = options;
			UpdateSettingsFromOptions();
			Title = "Service Reference Settings";
			AssembliesToReference = new ObservableCollection<CheckableAssemblyReference>();
		}
		
		public ServiceReferenceGeneratorOptions Options {
			get { return options; }
		}
		
		void UpdateSettingsFromOptions()
		{
			UpdateSelectedModifier();
			UpdateReferencedTypes();
		}
		
		void UpdateReferencedTypes()
		{
			if (options.UseTypesInProjectReferences) {
				this.ReuseTypes = true;
				this.UseReferencedAssemblies = true;
			} else if (options.UseTypesInSpecifiedAssemblies) {
				this.ReuseReferencedTypes = true;
				this.UseReferencedAssemblies = true;
			} else {
				this.ReuseReferencedTypes = false;
				this.ReuseTypes = false;
				this.UseReferencedAssemblies = false;
			}
		}
		
		void UpdateSelectedModifier()
		{
			if (options.GenerateInternalClasses) {
				SelectedModifier = Modifiers.Internal;
			} else {
				SelectedModifier = Modifiers.Public;
			}
		}
		
		public string Title { get; set; }
		
		public string AccessLevel {
			get { return accesslevel; }
		}
		
		Modifiers selectedModifier;
		
		public Modifiers SelectedModifier {
			get { return selectedModifier; }
			set {
				selectedModifier = value;
				UpdateClassGenerationModifier();
				base.RaisePropertyChanged(() => SelectedModifier);
			}
		}
		
		void UpdateClassGenerationModifier()
		{
			options.GenerateInternalClasses = (selectedModifier == Modifiers.Internal);
		}
		
		public bool GenerateAsyncOperations {
			get { return options.GenerateAsyncOperations; }
			set {
				options.GenerateAsyncOperations = value;
				base.RaisePropertyChanged(() => GenerateAsyncOperations);
			}
		}
		
		public bool GenerateMessageContract {
			get { return options.GenerateMessageContract; }
			set {
				options.GenerateMessageContract = value;
				base.RaisePropertyChanged(() => GenerateMessageContract);
			}
		}
		
		public CollectionTypes CollectionType {
			get { return options.ArrayCollectionType; }
			set {
				options.ArrayCollectionType = value;
				base.RaisePropertyChanged(() => CollectionType);
			}
		}
		
		public DictionaryCollectionTypes DictionaryCollectionType {
			get { return options.DictionaryCollectionType; }
			set {
				options.DictionaryCollectionType = value;
				base.RaisePropertyChanged(() => DictionaryCollectionType);
			}
		}
		
		bool useReferencedAssemblies;
		
		public bool UseReferencedAssemblies {
			get { return useReferencedAssemblies; }
			set { 
				useReferencedAssemblies = value;
				ReuseTypes = useReferencedAssemblies;
				if (!useReferencedAssemblies)
					ReuseReferencedTypes = false;
				base.RaisePropertyChanged(() => UseReferencedAssemblies);
			}
		}
		
		public bool ReuseTypes {
			get { return options.UseTypesInProjectReferences; }
			set {
				options.UseTypesInProjectReferences = value;
				base.RaisePropertyChanged(() => ReuseTypes);
			}
		}
		
		public bool ReuseReferencedTypes {
			get { return options.UseTypesInSpecifiedAssemblies; }
			set { 
				options.UseTypesInSpecifiedAssemblies = value;
				ListViewEnable = value;
				base.RaisePropertyChanged(() => ReuseReferencedTypes);
			}
		}
		
		bool listViewEnable;
		
		public bool ListViewEnable {
			get { return listViewEnable; }
			set {
				listViewEnable = value;
				base.RaisePropertyChanged(() => ListViewEnable);
			}
		}
		
		public ObservableCollection <CheckableAssemblyReference> AssembliesToReference { get; private set; }	
	}
}
