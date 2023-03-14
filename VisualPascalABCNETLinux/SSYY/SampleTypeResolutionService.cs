//------------------------------------------------------------------------------
/// <copyright from='1997' to='2002' company='Microsoft Corporation'>
///    Copyright (c) Microsoft Corporation. All Rights Reserved.
///
///    This source code is intended only as a supplement to Microsoft
///    Development Tools and/or on-line documentation.  See these other
///    materials for detailed information regarding Microsoft code samples.
///
/// </copyright>
//------------------------------------------------------------------------------
using System;
using System.ComponentModel.Design;
using System.Collections;
using System.Reflection;

namespace SampleDesignerHost
{

	/// The ITypeResolutionService is used to load types at design time. It keeps an internal
	/// set of assemblies that have been referenced thus far and can search them for types given
	/// unqualified names.
	public class SampleTypeResolutionService : System.ComponentModel.Design.ITypeResolutionService
	{
		private Hashtable assemblies;
		public SampleTypeResolutionService() { }

		/// We use this property to help us generate code and compile.
		public Assembly[] RefencedAssemblies
		{
			get
			{
				if (assemblies == null)
				{
					assemblies = new Hashtable();
				}
				Assembly[] ret = new Assembly[assemblies.Values.Count];
				assemblies.Values.CopyTo(ret, 0);
				return ret;
			}
		}

		#region Implementation of ITypeResolutionService

		/// Add an assembly to our internal set.
		public void ReferenceAssembly(System.Reflection.AssemblyName name)
		{
			if (assemblies == null)
			{
				assemblies = new Hashtable();
			}
			
			if (!assemblies.Contains(name))
			{
				assemblies.Add(name, Assembly.Load(name));
			}
		}

		/// Search our internal set of assemblies for one with this AssemblyName.
		/// If it cannot be located and throwOnError is true, throw an exception.
		public System.Reflection.Assembly GetAssembly(System.Reflection.AssemblyName name, bool throwOnError)
		{
			Assembly a = null;

			if (assemblies != null)
			{
				a = assemblies[name] as Assembly;
			}

			if ((a == null) && throwOnError)
			{
				throw new Exception("Assembly " + name.Name + " not found in referenced assemblies.");
			}
			return a;
		}

		/// Search our internal set of assemblies for one with this AssemblyName.
		System.Reflection.Assembly System.ComponentModel.Design.ITypeResolutionService.GetAssembly(System.Reflection.AssemblyName name)
		{
			return GetAssembly(name, false);
		}

		/// Find a type based on a name that may or may not be fully qualified.
		/// If the type cannot be found and throwOnError is true, throw an Exception.
		/// Searching should ignore case based on ignoreCase's value.
		public Type GetType(string name, bool throwOnError, bool ignoreCase)
		{
			// First we assume it is a fully qualified name, and that Type.GetType()
			// can find it.
			//
			Type t = Type.GetType(name, throwOnError, ignoreCase);

			// If that didn't work, then we check our referenced assemblies' types.
			if ((t == null) && (assemblies != null))
			{
				foreach (Assembly a in assemblies.Values)
				{
					t = a.GetType(name, throwOnError, ignoreCase);
					if (t != null)
					{
						break;
					}
				}
			}

			// No luck.
			if ((t == null) && throwOnError)
			{
				throw new Exception("The type " + name + " could not be found. " +
					"If it is an unqualified name, then its assembly has not been referenced.");
			}
			return t;
		}

		/// Find a type based on a name that may or may not be fully qualified.
		/// If the type cannot be found and throwOnError is true, throw an Exception.
		/// Do not ignore case while searching.
		Type System.ComponentModel.Design.ITypeResolutionService.GetType(string name, bool throwOnError)
		{
			return GetType(name, throwOnError, false);
		}

		/// Find a type based on a name that may or may not be fully qualified.
		/// Do not ignore case while searching.
		Type System.ComponentModel.Design.ITypeResolutionService.GetType(string name)
		{
			return GetType(name, false, false);
		}

		/// Return the path to the file which the given assembly was loaded.
		/// If that assembly has not been loaded, this returns null.
		public string GetPathOfAssembly(System.Reflection.AssemblyName name)
		{
			Assembly a = GetAssembly(name, false);
			if (a != null)
			{
				return a.Location;
			}
			return null;
		}

		#endregion
	}
}
