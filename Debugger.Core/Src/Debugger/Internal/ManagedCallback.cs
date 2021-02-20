﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision: 2274 $</version>
// </file>

// Regular expresion:
// ^{\t*}{(:Ll| )*{:i} *\(((.# {:i}, |\))|())^6\)*}\n\t*\{(.|\n)@\}
// Output: \1 - intention   \2 - declaration \3 - function name  \4-9 parameters

// Replace with:
// \1\2\n\1{\n\1\tEnterCallback(PausedReason.Other, "\3");\n\1\t\n\1\tExitCallback_Continue();\n\1}

using System;
using System.Runtime.InteropServices;
using Debugger.Wrappers.CorDebug;

namespace Debugger
{
	/// <summary>
	/// Handles all callbacks of a given process
	/// </summary>
	class ManagedCallback
	{
		Process process;
		bool pauseProcessInsteadOfContinue;
		
		[Debugger.Tests.Ignore]
		public Process Process {
			get {
				return process;
			}
		}
		
		public ManagedCallback(Process process)
		{
			this.process = process;
		}
		
		void EnterCallback(PausedReason pausedReason, string name, ICorDebugProcess pProcess)
		{
			process.TraceMessage("Callback: " + name);
			System.Diagnostics.Debug.Assert(process.CorProcess == pProcess);
			// Check state
			if (process.IsRunning ||
				// After break is pressed we may receive some messages that were already queued
				process.PauseSession.PausedReason == PausedReason.ForcedBreak ||
				// ExitProcess may be called at any time when debuggee is killed
				name == "ExitProcess") {
				
				if (process.IsPaused && process.PauseSession.PausedReason == PausedReason.ForcedBreak && name != "ExitProcess") {
					process.TraceMessage("Processing post-break callback");
					// Continue the break, process is still breaked because of the callback
					process.Continue();
					pauseProcessInsteadOfContinue = true;
				} else {
					pauseProcessInsteadOfContinue = false;
				}
				
				// Remove expired threads and functions
				foreach(Thread thread in process.Threads) {
					thread.CheckExpiration();
				}
				
				process.NotifyPaused(new PauseSession(pausedReason));
			} else {
				throw new DebuggerException("Invalid state at the start of callback");
			}
		}
		
		void EnterCallback(PausedReason pausedReason, string name, ICorDebugAppDomain pAppDomain)
		{
			EnterCallback(pausedReason, name, pAppDomain.Process);
		}
		
		void EnterCallback(PausedReason pausedReason, string name, ICorDebugThread pThread)
		{
			EnterCallback(pausedReason, name, pThread.Process);
			process.SelectedThread = process.GetThread(pThread);
		}
		
		void ExitCallback_Continue()
		{
			if (pauseProcessInsteadOfContinue) {
				ExitCallback_Paused();
			} else {
				process.Continue();
			}
		}
		
		void ExitCallback_Paused()
		{
			if (process.Evaluating) {
				// Ignore events during property evaluation
				ExitCallback_Continue();
			} else {
				process.Pause(process.PauseSession.PausedReason != PausedReason.EvalComplete);
			}
		}
		
		
		#region Program folow control
		
		public void StepComplete(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugStepper pStepper, CorDebugStepReason reason)
		{
			EnterCallback(PausedReason.StepComplete, "StepComplete (" + reason.ToString() + ")", pThread);
			
			Thread thread = process.GetThread(pThread);
			Stepper stepper = thread.GetStepper(pStepper);
			
			process.TraceMessage(" - stepper info: " + stepper.ToString());
			
			thread.Steppers.Remove(stepper);
			stepper.OnStepComplete();
			
			if (stepper.PauseWhenComplete) {
				if (process.SelectedThread.LastFunction.HasSymbols) {
					ExitCallback_Paused();
				} else {
					// This can only happen when JMC is disabled (ie NET1.1 or StepOut)
					if (stepper.Operation == Stepper.StepperOperation.StepOut) {
						// Create new stepper and keep going
						process.TraceMessage(" - stepping out of code without symbols at " + process.SelectedThread.LastFunction.ToString());
						new Stepper(process.SelectedThread.LastFunction, "Stepper out of code without symbols").StepOut();
						ExitCallback_Continue();
					} else {
						// NET1.1: There is extra step over stepper, just keep going
						process.TraceMessage(" - leaving code without symbols");
						ExitCallback_Continue();
					}
				}
			} else {
				ExitCallback_Continue();
			}
		}
		
		// Do not pass the pBreakpoint parameter as ICorDebugBreakpoint - marshaling of it fails in .NET 1.1
		public void Breakpoint(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, IntPtr pBreakpoint)
		{
			EnterCallback(PausedReason.Breakpoint, "Breakpoint", pThread);
			this.process.NotifyPaused(new PauseSession(PausedReason.Breakpoint));
			ExitCallback_Paused();
			
//			foreach (Breakpoint b in debugger.Breakpoints) {
//				if (b.Equals(pBreakpoint)) {
//					// TODO: Check that this works
//					b.OnHit();
//				}
//			}
		}
		
		public void BreakpointSetError(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugBreakpoint pBreakpoint, uint dwError)
		{
			EnterCallback(PausedReason.Other, "BreakpointSetError", pThread);
			
			ExitCallback_Continue();
		}
		
		public unsafe void Break(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread)
		{
			EnterCallback(PausedReason.Break, "Break", pThread);

			ExitCallback_Paused();
		}

		public void ControlCTrap(ICorDebugProcess pProcess)
		{
			EnterCallback(PausedReason.ControlCTrap, "ControlCTrap", pProcess);

			ExitCallback_Paused();
		}

		public unsafe void Exception(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, int unhandled)
		{
			// Exception2 is used in .NET Framework 2.0
			
			if (process.DebuggeeVersion.StartsWith("v1.")) {
				// Forward the call to Exception2, which handles EnterCallback and ExitCallback
				ExceptionType exceptionType = (unhandled != 0)?ExceptionType.DEBUG_EXCEPTION_UNHANDLED:ExceptionType.DEBUG_EXCEPTION_FIRST_CHANCE;
				Exception2(pAppDomain, pThread, null, 0, (CorDebugExceptionCallbackType)exceptionType, 0);
			} else {
				// This callback should be ignored in v2 applications
				EnterCallback(PausedReason.Other, "Exception", pThread);
	
				ExitCallback_Continue();	
			}
		}

		#endregion

		#region Various

		public void LogSwitch(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, int lLevel, uint ulReason, string pLogSwitchName, string pParentName)
		{
			EnterCallback(PausedReason.Other, "LogSwitch", pThread);

			ExitCallback_Continue();
		}
		
		public void LogMessage(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, int lLevel, string pLogSwitchName, string pMessage)
		{
			EnterCallback(PausedReason.Other, "LogMessage", pThread);

			process.OnLogMessage(new MessageEventArgs(process, lLevel, pMessage, pLogSwitchName));

			ExitCallback_Continue();
		}

		public void EditAndContinueRemap(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugFunction pFunction, int fAccurate)
		{
			EnterCallback(PausedReason.Other, "EditAndContinueRemap", pThread);

			ExitCallback_Continue();
		}
		
		public void EvalException(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugEval corEval)
		{
			EnterCallback(PausedReason.EvalComplete, "EvalException", pThread);
			
			HandleEvalComplete(pAppDomain, pThread, corEval, true);
		}
		
		public void EvalComplete(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugEval corEval)
		{
			EnterCallback(PausedReason.EvalComplete, "EvalComplete", pThread);
			
			HandleEvalComplete(pAppDomain, pThread, corEval, false);			
		}
		
		void HandleEvalComplete(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugEval corEval, bool exception)
		{
			// Let the eval know that the CorEval has finished
			Eval eval = process.GetEval(corEval);
			eval.NotifyEvaluationComplete(!exception);
			process.NotifyEvaluationComplete(eval);
			
			if (process.SetupNextEvaluation()) {
				ExitCallback_Continue();
			} else {
				ExitCallback_Paused();
			}
		}
		
		public void DebuggerError(ICorDebugProcess pProcess, int errorHR, uint errorCode)
		{
			EnterCallback(PausedReason.DebuggerError, "DebuggerError", pProcess);

			string errorText = String.Format("Debugger error: \nHR = 0x{0:X} \nCode = 0x{1:X}", errorHR, errorCode);
			
			if ((uint)errorHR == 0x80131C30) {
				errorText += "\n\nIf you are running a 64-bit system this setting might help:\nProject -> Project Options -> Compiling -> Target CPU = 32-bit Intel";
			}
			
			System.Windows.Forms.MessageBox.Show(errorText);

			ExitCallback_Paused();
		}

		public void UpdateModuleSymbols(ICorDebugAppDomain pAppDomain, ICorDebugModule pModule, IStream pSymbolStream)
		{
			EnterCallback(PausedReason.Other, "UpdateModuleSymbols", pAppDomain);

			ExitCallback_Continue();
		}

		#endregion

		#region Start of Application

		public void CreateProcess(ICorDebugProcess pProcess)
		{
			EnterCallback(PausedReason.Other, "CreateProcess", pProcess);

			// Process is added in NDebugger.Start

			ExitCallback_Continue();
		}

		public void CreateAppDomain(ICorDebugProcess pProcess, ICorDebugAppDomain pAppDomain)
		{
			EnterCallback(PausedReason.Other, "CreateAppDomain", pAppDomain);

			pAppDomain.Attach();

			ExitCallback_Continue();
		}

		public void LoadAssembly(ICorDebugAppDomain pAppDomain, ICorDebugAssembly pAssembly)
		{
			EnterCallback(PausedReason.Other, "LoadAssembly", pAppDomain);

			ExitCallback_Continue();
		}

		public unsafe void LoadModule(ICorDebugAppDomain pAppDomain, ICorDebugModule pModule)
		{
			EnterCallback(PausedReason.Other, "LoadModule", pAppDomain);
			
			process.AddModule(pModule);
			
			ExitCallback_Continue();
		}
		
		public void NameChange(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread)
		{
			if (pAppDomain != null) {
				
				EnterCallback(PausedReason.Other, "NameChange: pAppDomain", pAppDomain);
				
				ExitCallback_Continue();
				
			}
			if (pThread != null) {
				
				EnterCallback(PausedReason.Other, "NameChange: pThread", pThread);
				
				Thread thread = process.GetThread(pThread);
				thread.HasBeenLoaded = true;
				
				ExitCallback_Continue();
				
			}
		}
		
		public void CreateThread(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread)
		{
			// We can not use pThread since it has not been added yet
			// and we continue from this callback anyway
			EnterCallback(PausedReason.Other, "CreateThread " + pThread.ID, pAppDomain);
			
			process.AddThread(pThread);
			
			ExitCallback_Continue();
		}
		
		public void LoadClass(ICorDebugAppDomain pAppDomain, ICorDebugClass c)
		{
			EnterCallback(PausedReason.Other, "LoadClass", pAppDomain);
			
			ExitCallback_Continue();
		}
		
		#endregion
		
		#region Exit of Application
		
		public void UnloadClass(ICorDebugAppDomain pAppDomain, ICorDebugClass c)
		{
			EnterCallback(PausedReason.Other, "UnloadClass", pAppDomain);
			
			ExitCallback_Continue();
		}
		
		public void UnloadModule(ICorDebugAppDomain pAppDomain, ICorDebugModule pModule)
		{
			EnterCallback(PausedReason.Other, "UnloadModule", pAppDomain);
			
			process.RemoveModule(pModule);
			
			ExitCallback_Continue();
		}
		
		public void UnloadAssembly(ICorDebugAppDomain pAppDomain, ICorDebugAssembly pAssembly)
		{
			EnterCallback(PausedReason.Other, "UnloadAssembly", pAppDomain);
			
			ExitCallback_Continue();
		}
		
		public void ExitThread(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread)
		{
			// It seems that ICorDebugThread is still not dead and can be used
			EnterCallback(PausedReason.Other, "ExitThread " + pThread.ID, pThread);
			
			process.GetThread(pThread).NotifyNativeThreadExited();
			
			try {
				ExitCallback_Continue();
			} catch (COMException e) {
				// For some reason this sometimes happens in .NET 1.1
				process.TraceMessage("Continue failed in ExitThread callback: " + e.Message);
			}
		}
		
		public void ExitAppDomain(ICorDebugProcess pProcess, ICorDebugAppDomain pAppDomain)
		{
			EnterCallback(PausedReason.Other, "ExitAppDomain", pAppDomain);
			
			ExitCallback_Continue();
		}
		
		public void ExitProcess(ICorDebugProcess pProcess)
		{
			EnterCallback(PausedReason.Other, "ExitProcess", pProcess);
			process.NotifyHasExpired();
		}
		
		#endregion
		
		#region ICorDebugManagedCallback2 Members
		
		public void ChangeConnection(ICorDebugProcess pProcess, uint dwConnectionId)
		{
			EnterCallback(PausedReason.Other, "ChangeConnection", pProcess);
			
			ExitCallback_Continue();
		}

		public void CreateConnection(ICorDebugProcess pProcess, uint dwConnectionId, IntPtr pConnName)
		{
			EnterCallback(PausedReason.Other, "CreateConnection", pProcess);
			
			ExitCallback_Continue();
		}

		public void DestroyConnection(ICorDebugProcess pProcess, uint dwConnectionId)
		{
			EnterCallback(PausedReason.Other, "DestroyConnection", pProcess);
			
			ExitCallback_Continue();
		}

		public void Exception2(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugFrame pFrame, uint nOffset, CorDebugExceptionCallbackType exceptionType, uint dwFlags)
		{
			EnterCallback(PausedReason.Exception, "Exception2 (type=" + exceptionType.ToString() + ")", pThread);
			
			// This callback is also called from Exception(...)!!!! (the .NET 1.1 version)
			// Whatch out for the zeros and null!
			// Exception -> Exception2(pAppDomain, pThread, null, 0, exceptionType, 0);
			
			process.SelectedThread.CurrentExceptionType = (ExceptionType)exceptionType;
			
			if (ExceptionType.DEBUG_EXCEPTION_UNHANDLED != (ExceptionType)exceptionType) {
				// Handled exception
				if (process.PauseOnHandledException) {
					ExitCallback_Paused();
				} else {
					ExitCallback_Continue();					
				}
			} else {
				// Unhandled exception				
				ExitCallback_Paused();
			}
		}
		
//		public void Exception2(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugFrame pFrame, uint nOffset, CorDebugExceptionCallbackType exceptionType, uint dwFlags)
//		{
//			EnterCallback(PausedReason.Exception, "Exception2 (type=" + exceptionType.ToString() + ")", pThread);
//			
//			// This callback is also called from Exception(...)!!!! (the .NET 1.1 version)
//			// Watch out for the zeros and null!
//			// Exception -> Exception2(pAppDomain, pThread, null, 0, exceptionType, 0);
//			
//			process.SelectedThread.CurrentException = new Exception(new Value(process, new Expressions.CurrentExceptionExpression(), process.SelectedThread.CorThread.CurrentException));
//			process.SelectedThread.CurrentException_DebuggeeState = process.DebuggeeState;
//			process.SelectedThread.CurrentExceptionType = (ExceptionType)exceptionType;
//			process.SelectedThread.CurrentExceptionIsUnhandled = (ExceptionType)exceptionType == ExceptionType.Unhandled;
//			
//			if (process.SelectedThread.CurrentExceptionIsUnhandled ||
//			    process.PauseOnHandledException) 
//			{
//				pauseOnNextExit = true;
//			}
//			ExitCallback();
//		}
		
		public void ExceptionUnwind(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, CorDebugExceptionUnwindCallbackType dwEventType, uint dwFlags)
		{
			EnterCallback(PausedReason.ExceptionIntercepted, "ExceptionUnwind", pThread);
			
			if (dwEventType == CorDebugExceptionUnwindCallbackType.DEBUG_EXCEPTION_INTERCEPTED) {
				ExitCallback_Paused();
			} else {
				ExitCallback_Continue();
			}
		}

		public void FunctionRemapComplete(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugFunction pFunction)
		{
			EnterCallback(PausedReason.Other, "FunctionRemapComplete", pThread);
			
			ExitCallback_Continue();
		}

		public void FunctionRemapOpportunity(ICorDebugAppDomain pAppDomain, ICorDebugThread pThread, ICorDebugFunction pOldFunction, ICorDebugFunction pNewFunction, uint oldILOffset)
		{
			EnterCallback(PausedReason.Other, "FunctionRemapOpportunity", pThread);
			
			ExitCallback_Continue();
		}

		public void MDANotification(ICorDebugController c, ICorDebugThread t, ICorDebugMDA mda)
		{
			if (c.Is<ICorDebugAppDomain>()) {
				EnterCallback(PausedReason.Other, "MDANotification", c.CastTo<ICorDebugAppDomain>());
			} else if (c.Is<ICorDebugProcess>()){
				EnterCallback(PausedReason.Other, "MDANotification", c.CastTo<ICorDebugProcess>());
			} else {
				throw new System.Exception("Unknown callback argument");
			}
			
			ExitCallback_Continue();
		}

		#endregion
	}
}
