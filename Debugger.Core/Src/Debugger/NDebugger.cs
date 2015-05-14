// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision: 2210 $</version>
// </file>

using System;
using System.Text;
using System.Threading;

using Debugger.Interop;
using Debugger.Wrappers.CorDebug;

namespace Debugger
{
	public partial class NDebugger: DebuggerObject
	{
		ICorDebug                  corDebug;
		ManagedCallbackSwitch      managedCallbackSwitch;
		ManagedCallbackProxy       managedCallbackProxy;
		
		MTA2STA mta2sta = new MTA2STA();
		
		string debuggeeVersion;
		
		public MTA2STA MTA2STA {
			get {
				return mta2sta;
			}
		}
		
		internal ICorDebug CorDebug {
			get {
				return corDebug;
			}
		}
		
		public string DebuggeeVersion {
			get {
				return debuggeeVersion;
			}
		}
		
		public NDebugger()
		{
			if (ApartmentState.STA == System.Threading.Thread.CurrentThread.GetApartmentState()) {
				mta2sta.CallMethod = CallMethod.HiddenFormWithTimeout;
			} else {
				mta2sta.CallMethod = CallMethod.DirectCall;
			}
			
			Wrappers.ResourceManager.TraceMessagesEnabled = false;
			Wrappers.ResourceManager.TraceMessage += delegate (object s, MessageEventArgs e) { 
				TraceMessage(e.Message);
			};
		}
		
		/// <summary>
		/// Get the .NET version of the process that called this function
		/// </summary>
		public string GetDebuggerVersion()
		{
			int size;
			NativeMethods.GetCORVersion(null, 0, out size);
			StringBuilder sb = new StringBuilder(size);
			int hr = NativeMethods.GetCORVersion(sb, sb.Capacity, out size);
			return sb.ToString();
		}
		
		/// <summary>
		/// Get the .NET version of a given program - eg. "v1.1.4322"
		/// </summary>
		public string GetProgramVersion(string exeFilename)
		{
			int size;
			NativeMethods.GetRequestedRuntimeVersion(exeFilename, null, 0, out size);
			StringBuilder sb = new StringBuilder(size);
			NativeMethods.GetRequestedRuntimeVersion(exeFilename, sb, sb.Capacity, out size);
			return sb.ToString();
		}
		
		public Breakpoint GetBreakpoint(string fileName, int line)
		{
			foreach (Breakpoint br in this.breakpointCollection)
				if (br.SourcecodeSegment.StartLine == line && string.Compare(br.SourcecodeSegment.SourceFullFilename,fileName,true)==0)
				return br;
			return null;
		}
		/// <summary>
		/// Prepares the debugger
		/// </summary>
		/// <param name="debuggeeVersion">Version of the program to debug - eg. "v1.1.4322"
		/// If null, the version of the executing process will be used</param>
		internal void InitDebugger(string debuggeeVersion)
		{
			/*if (debuggeeVersion != null && debuggeeVersion.Length > 1) {
				this.debuggeeVersion = debuggeeVersion;
			} else {
				this.debuggeeVersion = GetDebuggerVersion();
			}
			int debug_ver = 3;
		    if (debuggeeVersion.StartsWith("v1") || debuggeeVersion.StartsWith("v2")) {
				debug_ver = 3; // 2.0 CLR
				TraceMessage("Debugger interface version: v2.0");
			} else {
				debug_ver = 4; // 4.0 CLR
				TraceMessage("Debugger interface version: v4.0");
			}*/
			if (string.IsNullOrEmpty(debuggeeVersion)) {
				debuggeeVersion = GetDebuggerVersion();
				TraceMessage("Debuggee version: Unknown (assuming " + debuggeeVersion + ")");
			} else {
				TraceMessage("Debuggee version: " + debuggeeVersion);
			}
			this.debuggeeVersion = debuggeeVersion;
			
			int debuggerVersion;
			// The CLR does not provide 4.0 debugger interface for older versions
			if (debuggeeVersion.StartsWith("v1") || debuggeeVersion.StartsWith("v2")) {
				debuggerVersion = 3; // 2.0 CLR
				TraceMessage("Debugger interface version: v2.0");
			} else {
				debuggerVersion = 4; // 4.0 CLR
				TraceMessage("Debugger interface version: v4.0");
			}
			
			
			corDebug = new ICorDebug(NativeMethods.CreateDebuggingInterfaceFromVersion(debuggerVersion, debuggeeVersion));
			
			managedCallbackSwitch = new ManagedCallbackSwitch(this);
			managedCallbackProxy = new ManagedCallbackProxy(this, managedCallbackSwitch);
			
			corDebug.Initialize();
			corDebug.SetManagedHandler(new ICorDebugManagedCallback(managedCallbackProxy));
			
			TraceMessage("ICorDebug initialized, debugee version " + debuggeeVersion);
		}
		
		internal void TerminateDebugger()
		{
			ResetBreakpoints();
			
			TraceMessage("Reset done");
			
			corDebug.Terminate();
			
			TraceMessage("ICorDebug terminated");
			
			Wrappers.ResourceManager.TraceMessagesEnabled = true;
			Wrappers.ResourceManager.ReleaseAllTrackedCOMObjects();
			Wrappers.ResourceManager.TraceMessagesEnabled = false;
			
			TraceMessage("Tracked COM objects released");
		}
		
		/// <summary>
		/// Internal: Used to debug the debugger library.
		/// </summary>
		public event EventHandler<MessageEventArgs> DebuggerTraceMessage;
		
		protected internal virtual void OnDebuggerTraceMessage(MessageEventArgs e)
		{
			if (DebuggerTraceMessage != null) {
				DebuggerTraceMessage(this, e);
			}
		}
		
		internal void TraceMessage(string message)
		{
			System.Diagnostics.Debug.WriteLine("Debugger:" + message);
			OnDebuggerTraceMessage(new MessageEventArgs(null, message));
		}
		
		public void StartWithoutDebugging(System.Diagnostics.ProcessStartInfo psi)
		{
			System.Diagnostics.Process process;
			process = new System.Diagnostics.Process();
			process.StartInfo = psi;
			process.Start();
		}
		
		public Process Start(string filename, string workingDirectory, string arguments)		
		{
			InitDebugger(GetProgramVersion(filename));
			Process process = Process.CreateProcess(this, filename, workingDirectory, arguments);
			AddProcess(process);
			return process;
		}
	}
}
