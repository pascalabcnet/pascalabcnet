//
// Subprocess.cs
//
// Author:
//       Lluis Sanchez <llsan@microsoft.com>
//
// Copyright (c) 2020 Microsoft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Mono.Debugger.Soft;
using Mono.Debugging.Backend;
using Mono.Debugging.Client;
using Mono.Debugging.Evaluation;

namespace Mono.Debugging.Soft
{
	class Subprocess
	{
		const string ArgStartMarker = "_ARG_START_";
		readonly SoftDebuggerSession softDebuggerSession;

		ManualResetEvent launchingEvent = new ManualResetEvent (false);
		ManualResetEvent startedEvent = new ManualResetEvent (false);

		string arguments;
		string exe;

		int processId;
		bool shutdown;

		ProcessStartInfo debuggerStartInfo;

		SoftDebuggerSession newSession;

		public SoftDebuggerSession SubprocessSession => newSession;

		public long ThreadId { get; private set; }

		public Subprocess (long threadId, SoftDebuggerSession softDebuggerSession)
		{
			ThreadId = threadId;
			this.softDebuggerSession = softDebuggerSession;
		}

		public static bool IsMonoLauncher (string filePath)
		{
			var exeName = Path.GetFileName (filePath);
			return exeName == "mono" || exeName == "mono64";
		}

		public bool CreateSession (ValueReference startInfo, EvaluationOptions ops)
		{
			// Get the exe and arguments from the start info and store them
			this.arguments = (string)startInfo.GetChild ("Arguments", ops).GetRawValue (ops);
			this.exe = (string)startInfo.GetChild ("FileName", ops).GetRawValue (ops);

			// Create the new session and custom start arguments.
			// The actual arguments don't matter much since we'll use a custom launcher for the process.
			newSession = new SoftDebuggerSession ();

			var startArgs = new SoftDebuggerLaunchArgs (null, new Dictionary<string, string> ());
			var dsi = new SoftDebuggerStartInfo (startArgs);

			// A mono process can be launched either by providing "mono" as launcher and then the assembly name as argument,
			// or by providing the assembly name directly as file to launch.

			bool explicitMonoLaunch = IsMonoLauncher (exe);
			if (explicitMonoLaunch) {
				// Try to get the assembly name from the command line. The debug session uses what's provided
				// in the Command property as process name. Without this, all mono processes launched in this
				// way would show up as "mono" in the IDE.
				var cmd = GetExeName (arguments);
				if (cmd != null)
					dsi.Command = cmd;

				// The new session will add additional runtime arguments to the start info.
				// We'll use the _ARG_START_ marker to know where those arguments end.
				dsi.RuntimeArguments = ArgStartMarker;
			} else {
				dsi.Command = exe;
				dsi.Arguments = arguments;
			}

			startArgs.CustomProcessLauncher = TargetProcessLauncher;

			// Start the session. This will run asynchronously, and will at some point call the custom launcher specified just above.
			newSession.Run (dsi, softDebuggerSession.Options);

			// Wait for the session to ask for the process to be launched
			launchingEvent.WaitOne ();
			if (shutdown)
				return false;

			// The custom launcher will store the debugger agent configuration args in debuggerStartInfo.
			// No the original startInfo object is patched to include the debugger agent args

			if (explicitMonoLaunch) {
				// Prepend the debugger args to the original arguments
				int i = debuggerStartInfo.Arguments.IndexOf (ArgStartMarker);
				var debuggerArgs = debuggerStartInfo.Arguments.Substring (0, i);
				startInfo.GetChild ("Arguments", ops).SetRawValue (debuggerArgs + " " + arguments, ops);
			} else {
				startInfo.GetChild ("Arguments", ops).SetRawValue (debuggerStartInfo.Arguments, ops);
				startInfo.GetChild ("FileName", ops).SetRawValue (debuggerStartInfo.FileName, ops);
			}
			return true;
		}

		Process TargetProcessLauncher (ProcessStartInfo info)
		{
			// This callback is called by the debug session to start the process.
			// We won't actually launch the process here since the current process is already doing it,
			// we just need to wait for the process to be launched by resuming execution, and then return
			// a reference to that process.

			// Store a reference to the start info provided to start the process. We need to retrieve the
			// debug agent configuration arguments from it.
			debuggerStartInfo = info;

			info.RedirectStandardError = false;
			info.RedirectStandardOutput = false;

			// Signal that the session asked the process to be started (this will cause execution to be resumed,
			// so the process will be launched)
			launchingEvent.Set ();

			// Wait for the process to start
			startedEvent.WaitOne ();

			// processId should be set now. We can now return the process that has launched.
			var process = Process.GetProcessById (processId);
			process.EnableRaisingEvents = true;
			return process;
		}

		public void SetStarted (ValueReference startInfo, int id, EvaluationContext ctx)
		{
			// This is called after the process is launched.

			// Store the process ID. It will be used by TargetProcessLauncher to get a reference to the process.
			processId = id;

			// Assign the original exe and arguments to the start info object
			startInfo.GetChild ("Arguments", ctx.Options).SetRawValue (arguments, ctx.Options);
			startInfo.GetChild ("FileName", ctx.Options).SetRawValue (exe, ctx.Options);

			// Signal TargetProcessLauncher that the process has started and the process ID is available.
			startedEvent.Set ();
		}

		public static string GetExeName (string monoCommandArgs)
		{
			foreach (var arg in GetArguments (monoCommandArgs)) {
				if (arg.StartsWith ("--"))
					continue;
				var ext = Path.GetExtension (arg).ToLower ();
				if ((ext == ".dll" || ext == ".exe") && File.Exists (arg))
					return arg;
			}
			return null;
		}

		static IEnumerable<string> GetArguments (string monoCommandArgs)
		{
			bool inQuotes = false;
			var currentString = new StringBuilder ();
			for (int n = 0; n < monoCommandArgs.Length; n++) {
				var c = monoCommandArgs[n];
				if (c == '\\') {
					if (n < monoCommandArgs.Length - 1 && (monoCommandArgs[n + 1] == '\\' || monoCommandArgs[n + 1] == '"')) {
						currentString.Append (monoCommandArgs[n + 1]);
						n++;
					}
				} else if (inQuotes) {
					if (c == '"')
						inQuotes = false;
					else
						currentString.Append (c);
				} else if (c == '"') {
					inQuotes = true;
				} else if (char.IsWhiteSpace (c)) {
					if (currentString.Length > 0) {
						yield return currentString.ToString ();
						currentString.Clear ();
					}
				} else
					currentString.Append (c);
			}
			if (currentString.Length > 0)
				yield return currentString.ToString ();
		}

		public void Shutdown ()
		{
			shutdown = true;
			launchingEvent.Set ();
			startedEvent.Set ();
		}
	}
}
