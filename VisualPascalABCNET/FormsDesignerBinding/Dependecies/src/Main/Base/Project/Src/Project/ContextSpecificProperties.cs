﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using ICSharpCode.Core;
using System.ComponentModel;

namespace ICSharpCode.SharpDevelop.Project
{
	// The classes in this file provide support for context-specific custom properties
	// (pseudo-properties that are visible in the property grid and allow editing MSBuild meta data)
	
	/// <summary>
	/// Creates a custom property for project items.
	/// </summary>
	/// <attribute name="name" use="required">
	/// The name of the MSBuild meta data.
	/// </attribute>
	/// <attribute name="displayName" use="optional">
	/// The display name of the property.
	/// </attribute>
	/// <attribute name="description" use="optional">
	/// The description text for the property.
	/// </attribute>
	/// <attribute name="runCustomTool" use="optional">
	/// Boolean property specifying whether the custom tool should be run when the property value is changed
	/// by the user. Default: false.
	/// </attribute>
	/// <usage>In /SharpDevelop/Views/ProjectBrowser/ContextSpecificProperties</usage>
	/// <returns>
	/// A <see cref="PropertyDescriptor"/> object.
	/// </returns>
	public sealed class CustomPropertyDoozer : IDoozer
	{
		public bool HandleConditions {
			get {
				return false;
			}
		}
		
		public object BuildItem(BuildItemArgs args)
		{
			Codon codon = args.Codon;
			CustomProperty cp = new CustomProperty(codon.Properties["name"]) {
				displayName = codon.Properties["displayName"],
				description = codon.Properties["description"]
			};
			if (!string.IsNullOrEmpty(codon.Properties["runCustomTool"]))
				cp.runCustomTool = bool.Parse(codon.Properties["runCustomTool"]);
			return cp;
		}
		
		sealed class CustomProperty : PropertyDescriptor
		{
			public CustomProperty(string name) : base(name, null)
			{
			}
			
			internal string displayName, description;
			internal bool runCustomTool;
			
			public override string DisplayName {
				get {
					if (string.IsNullOrEmpty(displayName))
						return Name;
					else
						return StringParser.Parse(displayName);
				}
			}
			
			public override string Description {
				get { return StringParser.Parse(description); }
			}
			
			public override Type ComponentType {
				get {
					return typeof(ProjectItem);
				}
			}
			
			public override bool IsReadOnly {
				get {
					return false;
				}
			}
			
			public override Type PropertyType {
				get {
					return typeof(string);
				}
			}
			
			public override bool CanResetValue(object component)
			{
				return true;
			}
			
			public override object GetValue(object component)
			{
				return ((ProjectItem)component).GetEvaluatedMetadata(Name);
			}
			
			public override void ResetValue(object component)
			{
				SetValue(component, null);
			}
			
			public override void SetValue(object component, object value)
			{
				ProjectItem p = (ProjectItem)component;
				p.SetEvaluatedMetadata(Name, (string)value);
				p.InformSetValue(this, component, value);
				if (runCustomTool) {
					FileProjectItem fpi = p as FileProjectItem;
					if (fpi != null) {
						CustomToolsService.RunCustomTool(fpi, false);
					}
				}
			}
			
			public override bool ShouldSerializeValue(object component)
			{
				return !string.IsNullOrEmpty((string)GetValue(component));
			}
		}
	}
	
	/// <summary>
	/// Tests the item type of a project item and/or an MSBuild meta data value.
	/// </summary>
	/// <attribute name="itemType">
	/// The type the project item must have. If this attribute is not specified, the type is not tested.
	/// </attribute>
	/// <attribute name="property">
	/// The name of the MSBuild meta data to test. If this attribute is not specified, no property is tested.
	/// </attribute>
	/// <attribute name="value">
	/// The value that the MSBuild meta data must have.
	/// </attribute>
	/// <example title="Test if a ProjectItem is an embedded resource">
	/// &lt;Condition name = "ProjectItem" itemType = "EmbeddedResource"&gt;
	/// </example>
	public sealed class ProjectItemConditionEvaluator : IConditionEvaluator
	{
		public bool IsValid(object caller, Condition condition)
		{
			ProjectItem p = (ProjectItem)caller;
			
			string itemType = condition.Properties["itemType"];
			if (!string.IsNullOrEmpty(itemType)) {
				if (!string.Equals(p.ItemType.ItemName, itemType, StringComparison.OrdinalIgnoreCase))
					return false;
			}
			
			string propName = condition.Properties["property"];
			string value = condition.Properties["value"];
			if (!string.IsNullOrEmpty(propName)) {
				if (!string.Equals(p.GetEvaluatedMetadata(propName) ?? "", value ?? "", StringComparison.OrdinalIgnoreCase))
					return false;
			}
			return true;
		}
	}
}
