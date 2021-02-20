﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision: 2185 $</version>
// </file>

#pragma warning disable 1591

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Debugger.Interop
{        
	public static class NativeMethods
	{
		[DllImport("kernel32.dll")]
		public static extern bool CloseHandle(IntPtr handle);
		
		[DllImport("mscoree.dll", CharSet=CharSet.Unicode, PreserveSig=false)]
		public static extern Debugger.Interop.CorDebug.ICorDebug CreateDebuggingInterfaceFromVersion(int debuggerVersion, string debuggeeVersion);
		
		[DllImport("mscoree.dll", CharSet=CharSet.Unicode)]
		public static extern int GetCORVersion([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder szName, Int32 cchBuffer, out Int32 dwLength);
		
		[DllImport("mscoree.dll", CharSet=CharSet.Unicode)]
		public static extern int GetRequestedRuntimeVersion(string exeFilename, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pVersion, Int32 cchBuffer, out Int32 dwLength);
	}
}

#pragma warning restore 1591
