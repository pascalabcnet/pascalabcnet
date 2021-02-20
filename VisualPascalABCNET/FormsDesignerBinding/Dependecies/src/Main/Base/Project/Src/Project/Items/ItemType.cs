﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;

namespace ICSharpCode.SharpDevelop.Project
{
	/// <summary>
	/// Struct for strongly-typed passing of item types
	/// - we don't want to use strings everywhere.
	/// Basically this is something like a typedef for C# (without implicit conversions).
	/// </summary>
	public struct ItemType : IEquatable<ItemType>, IComparable<ItemType>
	{
		// ReferenceProjectItem
		public static readonly ItemType Reference = new ItemType("Reference");
		public static readonly ItemType ProjectReference = new ItemType("ProjectReference");
		public static readonly ItemType COMReference = new ItemType("COMReference");
		
		public static readonly ReadOnlyCollectionWrapper<ItemType> ReferenceItemTypes
			= new ReadOnlyCollectionWrapper<ItemType>(new ItemType[] { Reference, ProjectReference, COMReference });
		
		/// <summary>
		/// Item type for imported VB namespaces
		/// </summary>
		public static readonly ItemType Import = new ItemType("Import");
		
		public static readonly ItemType WebReferenceUrl = new ItemType("WebReferenceUrl");
		
		// FileProjectItem
		public static readonly ItemType Compile = new ItemType("Compile");
		public static readonly ItemType EmbeddedResource = new ItemType("EmbeddedResource");
		public static readonly ItemType None = new ItemType("None");
		public static readonly ItemType Content = new ItemType("Content");
		public static readonly ItemType ApplicationDefinition = new ItemType("ApplicationDefinition");
		public static readonly ItemType Page = new ItemType("Page");
		public static readonly ItemType BootstrapperFile = new ItemType("BootstrapperFile");
		public static readonly ItemType Header = new ItemType("Header");
		
		// vcxproj-only (c++ project) items
		public static readonly ItemType ClCompile = new ItemType("ClCompile");
		public static readonly ItemType ClInclude = new ItemType("ClInclude");
		
		/// <summary>
		/// Gets a collection of item types that are used for files.
		/// </summary>
		public static readonly ReadOnlyCollectionWrapper<ItemType> DefaultFileItems
			= new ReadOnlyCollectionWrapper<ItemType>(new ItemType[] { Compile, EmbeddedResource, None, Content });
		
		public static readonly ItemType Resource = new ItemType("Resource");
		public static readonly ItemType Folder = new ItemType("Folder");
		public static readonly ItemType WebReferences = new ItemType("WebReferences");
		public static readonly ItemType ServiceReferences = new ItemType("WCFMetadata");
		public static readonly ItemType ServiceReference = new ItemType("WCFMetadataStorage");
		
		/// <summary>
		/// Gets a collection of item types that are known not to be used for files.
		/// </summary>
		public static readonly ReadOnlyCollectionWrapper<ItemType> NonFileItemTypes
			= new ReadOnlyCollectionWrapper<ItemType>(
				new List<ItemType>(ReferenceItemTypes) {
					Folder, WebReferences, Import 
				});
		
		readonly string itemName;
		
		public string ItemName {
			get { return itemName; }
		}
		
		public ItemType(string itemName)
		{
			if (itemName == null)
				throw new ArgumentNullException("itemName");
			this.itemName = itemName;
		}
		
		public override string ToString()
		{
			return itemName;
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			if (obj is ItemType)
				return Equals((ItemType)obj); // use Equals method below
			else
				return false;
		}
		
		public bool Equals(ItemType other)
		{
			return this.itemName == other.itemName;
		}
		
		public override int GetHashCode()
		{
			return itemName.GetHashCode();
		}
		
		public static bool operator ==(ItemType lhs, ItemType rhs)
		{
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(ItemType lhs, ItemType rhs)
		{
			return !(lhs.Equals(rhs)); // use operator == and negate result
		}
		#endregion
		
		public int CompareTo(ItemType other)
		{
			return itemName.CompareTo(other.itemName);
		}
		
		public bool IsFolder()
		{
			return
				(this == ItemType.Folder) ||
				(this == ItemType.WebReferences) ||
				(this == ItemType.ServiceReferences);
		}
	}
}
