// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision: 2285 $</version>
// </file>

using System;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using Debugger.Wrappers.CorDebug;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using System.Text;

namespace Debugger
{
			[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto, Pack=8), ComVisible(false)]
public class STARTUPINFO
{
  public int cb;
  public string lpReserved;
  public string lpDesktop;
  public string lpTitle;
  public int dwX;
  public int dwY;
  public int dwXSize;
  public int dwYSize;
  public int dwXCountChars;
  public int dwYCountChars;
  public int dwFillAttribute;
  public int dwFlags;
  public short wShowWindow;
  public short cbReserved2;
  public IntPtr lpReserved2;
  public SafeFileHandle hStdInput;
  public SafeFileHandle hStdOutput;
  public SafeFileHandle hStdError;
}

public delegate void DataReceivedEventHandler(object sender, DataReceivedEventArgs e);

	public partial class Process: DebuggerObject, IExpirable
	{
		NDebugger debugger;
		
		ICorDebugProcess corProcess;
		ManagedCallback callbackInterface;
		
		Thread selectedThread;
		PauseSession pauseSession;
		
		bool hasExpired = false;
		
		public event EventHandler Expired;
		public event DataReceivedEventHandler OutputDataReceived;
		public event DataReceivedEventHandler ErrorDataReceived;
		
		public bool HasExpired {
			get {
				return hasExpired;
			}
		}
		
		private StreamWriter standardInput;
		
		public StreamWriter StandardInput
		{
			get
			{
				return standardInput;
			}
			set
			{
				standardInput = value;
			}
		}
		
		private StreamReader standardOutput;
		public StreamReader StandardOutput
		{
			get
			{
				return standardOutput;
			}
			set
			{
				standardOutput = value;
			}
		}
		
		private StreamReader standardError;
		public StreamReader StandardError
		{
			get
			{
				return standardError;
			}
			set
			{
				standardError = value;
			}
		}
		
		internal void NotifyHasExpired()
		{
			if(!hasExpired) {
				hasExpired = true;
				if (Expired != null) {
					Expired(this, new ProcessEventArgs(this));
				}
//				if (PausedReason == PausedReason.Exception) {
//					ExceptionEventArgs args = new ExceptionEventArgs(this, SelectedThread.CurrentException);
//					OnExceptionThrown(args);
////					if (args.Continue) {
////						this.Continue();
////					}
//				}
				debugger.RemoveProcess(this);
			}
		}
		
		/// <summary>
		/// Indentification of the current debugger session. This value changes whenever debugger is continued
		/// </summary>
		public PauseSession PauseSession {
			get {
				return pauseSession;
			}
		}
		
		public void NotifyPaused(PauseSession pauseSession)
		{
			this.pauseSession = pauseSession;
		}
		
		public NDebugger Debugger {
			get {
				return debugger;
			}
		}
		
		internal ManagedCallback CallbackInterface {
			get {
				return callbackInterface;
			}
		}
		
		internal Process(NDebugger debugger, ICorDebugProcess corProcess)
		{
			this.debugger = debugger;
			this.corProcess = corProcess;
			
			this.callbackInterface = new ManagedCallback(this);
		}

		internal ICorDebugProcess CorProcess {
			get {
				return corProcess;
			}
		}
		
		public uint Id {
			get {
				return corProcess.ID;
			}
		}
		public Thread SelectedThread {
			get {
				return selectedThread;
			}
			set {
				selectedThread = value;
			}
		}
		
		static public Process CreateProcess(NDebugger debugger, string filename, string workingDirectory, string arguments)
		{
			return debugger.MTA2STA.Call<Process>(delegate{
			                                      	return StartInternal(debugger, filename, workingDirectory, arguments);
			                                      });
		}
		
		private static void CreatePipeWithSecurityAttributes(out SafeFileHandle hReadPipe, out SafeFileHandle hWritePipe, SECURITY_ATTRIBUTES lpPipeAttributes, int nSize)
{
  if ((!CreatePipe(out hReadPipe, out hWritePipe, lpPipeAttributes, nSize) || hReadPipe.IsInvalid) || hWritePipe.IsInvalid)
  {
    throw new Win32Exception();
  }
}

 		[DllImport("kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
private static extern bool CreatePipe(out SafeFileHandle hReadPipe, out SafeFileHandle hWritePipe, SECURITY_ATTRIBUTES lpPipeAttributes, int nSize);
 
 [DllImport("kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
private static extern int GetConsoleCP();

[DllImport("kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
private static extern int GetConsoleOutputCP();
 
 [DllImport("kernel32.dll", CharSet=CharSet.Ansi, SetLastError=true)]
private static extern IntPtr GetCurrentProcess();

[DllImport("kernel32.dll", CharSet=CharSet.Ansi, SetLastError=true)]
private static extern bool DuplicateHandle(HandleRef hSourceProcessHandle, SafeHandle hSourceHandle, HandleRef hTargetProcess, out SafeFileHandle targetHandle, int dwDesiredAccess, bool bInheritHandle, int dwOptions);
 

 [StructLayout(LayoutKind.Sequential)]
internal class SECURITY_ATTRIBUTES
{
  public int nLength;
  public int lpSecurityDescriptor;
  public bool bInheritHandle;
}


		private static void CreatePipe(out SafeFileHandle parentHandle, out SafeFileHandle childHandle, bool parentInputs)
		{
  			SECURITY_ATTRIBUTES lpPipeAttributes = new SECURITY_ATTRIBUTES();
  			lpPipeAttributes.bInheritHandle = true;
  			SafeFileHandle hWritePipe = null;
  			parentHandle = null;
  			try
  			{
    			if (parentInputs)
    			{
      				CreatePipeWithSecurityAttributes(out childHandle, out hWritePipe, lpPipeAttributes, 0);
    				parentHandle = hWritePipe;
    			}
    			else
    			{
      				CreatePipeWithSecurityAttributes(out hWritePipe, out childHandle, lpPipeAttributes, 0);
      				parentHandle = hWritePipe;
    			}
//    			if (!DuplicateHandle(new HandleRef(this, GetCurrentProcess()), hWritePipe, new HandleRef(this, GetCurrentProcess()), out parentHandle, 0, false, 2))
//    			{
//      				throw new Win32Exception();
//    			}
  			}
  			finally
 	 		{
//    			if ((hWritePipe != null) && !hWritePipe.IsInvalid)
//    			{
//      				hWritePipe.Close();
//    			}
  			}
		}
		



		static unsafe Process StartInternal(NDebugger debugger, string filename, string workingDirectory, string arguments)
		{
			debugger.TraceMessage("Executing " + filename);
			
			uint[] processStartupInfo = new uint[17];
			processStartupInfo[0] = sizeof(uint) * 17;
			uint[] processInfo = new uint[4];
			
			SafeFileHandle prnt_inp;
			SafeFileHandle prnt_out;
			SafeFileHandle prnt_err;
			SafeFileHandle input_handle;
			SafeFileHandle output_handle;
			SafeFileHandle err_handle;
//			CreatePipe(out prnt_out,out output_handle,false);
//			CreatePipe(out prnt_inp,out input_handle,true);
			STARTUPINFO startupInfo = new STARTUPINFO();
  			startupInfo.cb = Marshal.SizeOf(startupInfo);

			SECURITY_ATTRIBUTES lpPipeAttributes = new SECURITY_ATTRIBUTES();
  			lpPipeAttributes.bInheritHandle = true;
			//CreatePipe(out output_handle, out input_handle,lpPipeAttributes,0);
			CreatePipe(out prnt_inp, out input_handle, true);
			CreatePipe(out prnt_out, out output_handle, false);
			CreatePipe(out prnt_err, out err_handle, false);
			startupInfo.hStdInput = input_handle;
			startupInfo.hStdOutput = output_handle;
			startupInfo.hStdError = err_handle;
			ICorDebugProcess outProcess;
//			processStartupInfo[14] = (uint)GCHandle.Alloc(input_handle,GCHandleType.Pinned).AddrOfPinnedObject().ToInt32();
//			processStartupInfo[15] = (uint)GCHandle.Alloc(output_handle,GCHandleType.Pinned).AddrOfPinnedObject().ToInt32();
			StreamWriter standardInput = new StreamWriter(new FileStream(prnt_inp, FileAccess.Write, 0x1000, false), Encoding.GetEncoding(GetConsoleCP()), 0x1000);
    		standardInput.AutoFlush = true;
			Encoding encoding = Encoding.GetEncoding(GetConsoleOutputCP());
    		StreamReader standardOutput = new StreamReader(new FileStream(prnt_out, FileAccess.Read, 0x1000, false), encoding, true, 0x1000);
			StreamReader errorOutput = new StreamReader(new FileStream(prnt_err, FileAccess.Read, 0x1000, false), encoding, true, 0x1000);
			if (workingDirectory == null || workingDirectory == "") {
				workingDirectory = System.IO.Path.GetDirectoryName(filename);
			}
			
			fixed (uint* pprocessStartupInfo = processStartupInfo)
				fixed (uint* pprocessInfo = processInfo)
					outProcess =
						debugger.CorDebug.CreateProcess(
							filename,   // lpApplicationName
							  // If we do not prepend " ", the first argument migh just get lost
							" " + arguments,                       // lpCommandLine
							ref _SECURITY_ATTRIBUTES.Default,                       // lpProcessAttributes
							ref _SECURITY_ATTRIBUTES.Default,                      // lpThreadAttributes
							1,//TRUE                    // bInheritHandles
							//0x00000010 /*CREATE_NEW_CONSOLE*/,    // dwCreationFlags
							0x08000000,
							IntPtr.Zero,                       // lpEnvironment
							workingDirectory,                       // lpCurrentDirectory
							startupInfo,
							//(uint)pprocessStartupInfo,        // lpStartupInfo
							(uint)pprocessInfo,               // lpProcessInformation,
							CorDebugCreateProcessFlags.DEBUG_NO_SPECIAL_OPTIONS   // debuggingFlags
							);
			
			Process p = new Process(debugger, outProcess);
			p.StandardInput = standardInput;
			p.StandardOutput = standardOutput;
			p.StandardError = errorOutput;
			return p;
		}
		
		public void Break()
		{
			AssertRunning();
			
			corProcess.Stop(100); // TODO: Hardcoded value
			
			pauseSession = new PauseSession(PausedReason.ForcedBreak);

			// TODO: Code duplication from enter callback
			// Remove expired threads and functions
			foreach(Thread thread in this.Threads) {
				thread.CheckExpiration();
			}
			
			Pause(true);
		}
		
		public void Continue()
		{
			try
			{
			AssertPaused();
			
			
			pauseSession.NotifyHasExpired();
			pauseSession = null;
			OnDebuggingResumed();
			
			corProcess.Continue(0);
			}
			catch(System.Exception e)
			{
				
			}
		}
		
		public void Terminate()
		{
			// Resume stoped tread
			if (this.IsPaused) {
			// We might get more callbacks so we should maintain consistent sate
				this.Continue(); // TODO: Remove this...
			}
			
			// Expose race condition - drain callback queue
			System.Threading.Thread.Sleep(0);
			
			// Stop&terminate - both must be called
			corProcess.Stop(100); // TODO: ...and this
			corProcess.Terminate(0);
		}

		public bool IsRunning { 
			get {
				return pauseSession == null;
			}
		}
		
		public bool IsPaused {
			get {
				return !IsRunning;
			}
		}
		
		public void AssertPaused()
		{
			if (IsRunning) {
				throw new DebuggerException("Process is not paused.");
			}
		}
		
		public void AssertRunning()
		{
			if (IsPaused) {
				throw new DebuggerException("Process is not running.");
			}
		}
		
		
		public string DebuggeeVersion {
			get {
				return debugger.DebuggeeVersion;
			}
		}
		
		/// <summary>
		/// Fired when System.Diagnostics.Trace.WriteLine() is called in debuged process
		/// </summary>
		public event EventHandler<MessageEventArgs> LogMessage;
		
		protected internal virtual void OnLogMessage(MessageEventArgs arg)
		{
			TraceMessage ("Debugger event: OnLogMessage");
			if (LogMessage != null) {
				LogMessage(this, arg);
			}
		}
		
		public void TraceMessage(string message)
		{
			System.Diagnostics.Debug.WriteLine("Debugger:" + message);
			debugger.OnDebuggerTraceMessage(new MessageEventArgs(this, message));
		}
		
		public SourcecodeSegment NextStatement { 
			get {
				if (SelectedFunction == null || IsRunning) {
					return null;
				} else {
					return SelectedFunction.NextStatement;
				}
			}
		}
		
		public NamedValueCollection LocalVariables { 
			get {
				if (SelectedFunction == null || IsRunning) {
					return NamedValueCollection.Empty;
				} else {
					return SelectedFunction.Variables;
				}
			}
		}
		
		/// <summary> Gets value of given name which is accessible from selected function </summary>
		/// <returns> Null if not found </returns>
		public NamedValue GetValue(string name)
		{
			if (SelectedFunction == null || IsRunning) {
				return null;
			} else {
				return SelectedFunction.GetValue(name);
			}
		}
	}
}
