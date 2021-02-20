﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Daniel Grunwald" email="daniel@danielgrunwald.de"/>
//     <version>$Revision: 1040 $</version>
// </file>

/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                         //
// DO NOT EDIT GlobalAssemblyInfo.cs, it is recreated using AssemblyInfo.template whenever //
// StartUp is compiled.                                                                    //
//                                                                                         //
/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////

using System.Reflection;

[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: AssemblyCompany("ic#code")]
[assembly: AssemblyProduct("SharpDevelop")]
[assembly: AssemblyCopyright("2000-2007 AlphaSierraPapa")]
[assembly: AssemblyVersion(RevisionClass.FullVersion)]

internal static class RevisionClass
{
	public const string Major = "3";
	public const string Minor = "0";
	public const string Build = "0";
	public const string Revision = "2649";
	
	public const string MainVersion = Major + "." + Minor;
	public const string FullVersion = Major + "." + Minor + "." + Build + "." + Revision;
}
	
