// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision: 2207 $</version>
// </file>

using System;
using System.Threading;

namespace Debugger
{
	public partial class Process
	{
		bool pauseOnHandledException = false;
		
		DebugeeState debugeeState;
		
		public event EventHandler<ExceptionEventArgs> ExceptionThrown;
		public event EventHandler<ProcessEventArgs> DebuggingResumed;
		public event EventHandler<ProcessEventArgs> DebuggingPaused;
		public event EventHandler<ProcessEventArgs> DebuggeeStateChanged;
		
		public bool PauseOnHandledException {
			get {
				return pauseOnHandledException;
			}
			set {
				pauseOnHandledException = value;
			}
		}
		
		protected virtual void OnExceptionThrown(ExceptionEventArgs e)
		{
			if (ExceptionThrown != null) {
				ExceptionThrown(this, e);
			}
		}
		
		internal virtual void OnDebuggingResumed()
		{
			TraceMessage ("Debugger event: OnDebuggingResumed()");
			if (DebuggingResumed != null) {
				DebuggingResumed(this, new ProcessEventArgs(this));
			}
		}
		
		protected virtual void OnDebuggingPaused()
		{
			TraceMessage ("Debugger event: OnDebuggingPaused (" + PausedReason.ToString() + ")");
			if (DebuggingPaused != null) {
				DebuggingPaused(this, new ProcessEventArgs(this));
			}
		}
		
		// HACK: should not be public
		public virtual void OnDebuggeeStateChanged()
		{
			TraceMessage ("Debugger event: OnDebuggeeStateChanged (" + PausedReason.ToString() + ")");
			if (DebuggeeStateChanged != null) {
				DebuggeeStateChanged(this, new ProcessEventArgs(this));
			}
		}
		
		public Function SelectedFunction {
			get {
				if (SelectedThread == null) {
					return null;
				} else {
					return SelectedThread.SelectedFunction;
				}
			}
		}
		
		/// <summary>
		/// Indentification of the state of the debugee. This value changes whenever the state of the debugee significatntly changes
		/// </summary>
		public DebugeeState DebugeeState {
			get {
				return debugeeState;
			}
		}
		
		/// <summary>
		/// The reason why the debugger is paused.
		/// Thows an DebuggerException if debugger is not paused.
		/// </summary>
		public PausedReason PausedReason {
			get {
				AssertPaused();
				return PauseSession.PausedReason;
			}
		}
		
		public void Pause(bool debuggeeStateChanged)
		{
			try
			{
			if (this.SelectedThread == null && this.Threads.Count > 0) {
				this.SelectedThread = this.Threads[0];
			}
			if (this.SelectedThread != null) {
				// Disable all steppers - do not Deactivate since function tracking still needs them
				foreach(Stepper s in this.SelectedThread.Steppers) {
					s.PauseWhenComplete = false;
				}
				
				this.SelectedThread.SelectedFunction = this.SelectedThread.LastFunctionWithLoadedSymbols;
			}
				
			if (debuggeeStateChanged) {
				DebugeeState oldDebugeeState = debugeeState;
				debugeeState = new DebugeeState(this);
				OnDebuggeeStateChanged();
				if (oldDebugeeState != null) {
					oldDebugeeState.NotifyHasExpired();
				}
			}
			OnDebuggingPaused();
			if (PausedReason == PausedReason.Exception) {
				ExceptionEventArgs args = new ExceptionEventArgs(this, SelectedThread.CurrentException);
				OnExceptionThrown(args);
				if (args.Continue) {
					this.Continue();
				}
			}
			}
			catch(System.Exception e)
			{
				
			}
		}
		
		bool hasExited = false;
		
		public event EventHandler Exited;
		
		public bool HasExited {
			get {
				return hasExited;
			}
		}
		
		
		public void WaitForPause(TimeSpan timeout)
		{
			DateTime endTime = Util.HighPrecisionTimer.Now + timeout;
			while(this.IsRunning) {
				TimeSpan timeLeft = endTime - Util.HighPrecisionTimer.Now;
				if (timeLeft <= TimeSpan.FromMilliseconds(10)) break;
				//this.TraceMessage("Time left: " + timeLeft.TotalMilliseconds);
				debugger.MTA2STA.WaitForCall(timeLeft);
				debugger.MTA2STA.PerformCall();
			}
			//if (this.HasExited) throw new ProcessExitedException();
		}
		
		/// <summary>
		/// Waits until the debugger pauses unless it is already paused.
		/// Use PausedReason to find out why it paused.
		/// </summary>
		public void WaitForPause(out bool not_eval)
		{
			int ticks = Environment.TickCount;
			not_eval = false;
			if (!this.IsRunning || this.HasExpired)
			{
				not_eval = true;
				return;
			}
			while(this.IsRunning && !this.HasExpired) {
				debugger.MTA2STA.WaitForCall();
				debugger.MTA2STA.PerformAllCalls();
				if (Environment.TickCount-ticks > 100)
				{
					not_eval = true;
					return;
				}
			}
			if (this.HasExpired) throw new DebuggerException("Process exited before pausing");
		}
		
		/// <summary>
		/// Waits until the precesses exits.
		/// </summary>
		public void WaitForExit()
		{
			while(!this.HasExpired) {
				debugger.MTA2STA.WaitForCall();
				debugger.MTA2STA.PerformAllCalls();
			}
		}
		
		public void StepInto()
		{
			SelectedFunction.StepInto();
		}
		
		public void StepOver()
		{
			SelectedFunction.StepOver();
		}
		
		public void StepOut()
		{
			SelectedFunction.StepOut();
		}
	}
}
