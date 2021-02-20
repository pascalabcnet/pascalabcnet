﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace ICSharpCode.Core
{
	/// <summary>
	/// Stores information about the manifest of an AddIn.
	/// </summary>
	public class AddInManifest
	{
		List<AddInReference> dependencies = new List<AddInReference>();
		List<AddInReference> conflicts = new List<AddInReference>();
		Dictionary<string, Version> identities = new Dictionary<string, Version>();
		Version primaryVersion;
		string primaryIdentity;
		
		public string PrimaryIdentity {
			get {
				return primaryIdentity;
			}
		}
		
		public Version PrimaryVersion {
			get {
				return primaryVersion;
			}
		}
		
		public Dictionary<string, Version> Identities {
			get {
				return identities;
			}
		}
		
		public ReadOnlyCollection<AddInReference> Dependencies {
			get {
				return dependencies.AsReadOnly();
			}
		}
		
		public ReadOnlyCollection<AddInReference> Conflicts {
			get {
				return conflicts.AsReadOnly();
			}
		}
		
		void AddIdentity(string name, string version, string hintPath)
		{
			if (name.Length == 0)
				throw new AddInLoadException("Identity needs a name");
			foreach (char c in name) {
				if (!char.IsLetterOrDigit(c) && c != '.' && c != '_') {
					throw new AddInLoadException("Identity name contains invalid character: '" + c + "'");
				}
			}
			Version v = AddInReference.ParseVersion(version, hintPath);
			if (primaryVersion == null) {
				primaryVersion = v;
			}
			if (primaryIdentity == null) {
				primaryIdentity = name;
			}
			identities.Add(name, v);
		}
		
		public void ReadManifestSection(XmlReader reader, string hintPath)
		{
			if (reader.AttributeCount != 0) {
				throw new AddInLoadException("Manifest node cannot have attributes.");
			}
			if (reader.IsEmptyElement) {
				throw new AddInLoadException("Manifest node cannot be empty.");
			}
			while (reader.Read()) {
				switch (reader.NodeType) {
					case XmlNodeType.EndElement:
						if (reader.LocalName == "Manifest") {
							return;
						}
						break;
					case XmlNodeType.Element:
						string nodeName = reader.LocalName;
						Properties properties = Properties.ReadFromAttributes(reader);
						switch (nodeName) {
							case "Identity":
								AddIdentity(properties["name"], properties["version"], hintPath);
								break;
							case "Dependency":
								dependencies.Add(AddInReference.Create(properties, hintPath));
								break;
							case "Conflict":
								conflicts.Add(AddInReference.Create(properties, hintPath));
								break;
							default:
								throw new AddInLoadException("Unknown node in Manifest section:" + nodeName);
						}
						break;
				}
			}
		}
	}
}
