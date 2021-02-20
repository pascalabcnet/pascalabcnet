// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbeck�" email="dsrbecky@gmail.com"/>
//     <version>$Revision: 2077 $</version>
// </file>

#pragma warning disable 1591

namespace Debugger.Wrappers.CorDebug
{
	using System;
	
	public interface ICorDebugManagedCallbacks: Debugger.Interop.CorDebug.ICorDebugManagedCallback, Debugger.Interop.CorDebug.ICorDebugManagedCallback2
	{
		
	}
}

#pragma warning restore 1591
