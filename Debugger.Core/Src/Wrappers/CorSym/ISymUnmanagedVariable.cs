// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbeck�" email="dsrbecky@gmail.com"/>
//     <version>$Revision: 2077 $</version>
// </file>

#pragma warning disable 1591

namespace Debugger.Wrappers.CorSym
{
	using System;

	public partial class ISymUnmanagedVariable
	{
		public string Name {
			get {
				return Util.GetString(GetName);
			}
		}
	}
}

#pragma warning restore 1591
