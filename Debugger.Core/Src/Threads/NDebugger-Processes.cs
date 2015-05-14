// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision: 2206 $</version>
// </file>

using System;
using System.Collections.Generic;
using System.Threading;

using Debugger.Wrappers.CorDebug;

namespace Debugger
{
	public partial class NDebugger
	{
		List<Process> processCollection = new List<Process>();
		
		public event EventHandler<ProcessEventArgs> ProcessStarted;
		public event EventHandler<ProcessEventArgs> ProcessExited;
		
		public IList<Process> Processes {
			get {
				return processCollection.AsReadOnly();
			}
		}
		
		public Process DebugActiveProcess(uint handle, string filename)
		{
			
			InitDebugger(GetProgramVersion(filename));
			Process p = new Process(this,this.CorDebug.DebugActiveProcess(handle,0));
			AddProcess(p);
			return p;
		}
		
		private Process debugActiveProcess(uint handle, string filename)
		{
			InitDebugger(GetProgramVersion(filename));
			Process p = new Process(this,this.CorDebug.DebugActiveProcess(handle,0));
			AddProcess(p);
			return p;
		}
		
		internal Process GetProcess(ICorDebugProcess corProcess)
		{
			foreach (Process process in Processes) {
				if (process.CorProcess == corProcess) {
					return process;
				}
			}
			return null;
			//throw new DebuggerException("Process is not in collection");
		}

		internal void AddProcess(Process process)
		{
			processCollection.Add(process);
			OnProcessStarted(process);
		}

		internal void RemoveProcess(Process process)
		{
			processCollection.Remove(process);
			OnProcessExited(process);
			if (processCollection.Count == 0) {
				// Exit callback and then terminate the debugger
				this.MTA2STA.AsyncCall( delegate { this.TerminateDebugger(); } );
			}
		}

		protected virtual void OnProcessStarted(Process process)
		{
			if (ProcessStarted != null) {
				ProcessStarted(this, new ProcessEventArgs(process));
			}
		}

		protected virtual void OnProcessExited(Process process)
		{
			if (ProcessExited != null) {
				ProcessExited(this, new ProcessEventArgs(process));
			}
		}
	}
}
