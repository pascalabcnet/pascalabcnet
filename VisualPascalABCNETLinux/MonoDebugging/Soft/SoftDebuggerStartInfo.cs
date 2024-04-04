// 
// SoftDebuggerStartInfo.cs
//  
// Author:
//       Michael Hutchinson <mhutchinson@novell.com>
// 
// Copyright (c) 2010 Novell, Inc. (http://www.novell.com)
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
using Mono.Debugging.Client;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Net;
using Mono.Debugger.Soft;

namespace Mono.Debugging.Soft
{
	public class SoftDebuggerStartInfo : DebuggerStartInfo
	{
		public SoftDebuggerStartInfo (string monoRuntimePrefix, Dictionary<string,string> monoRuntimeEnvironmentVariables)
			: this (new SoftDebuggerLaunchArgs (monoRuntimePrefix, monoRuntimeEnvironmentVariables))
		{
		}
		
		public SoftDebuggerStartInfo (SoftDebuggerStartArgs startArgs)
		{
			if (startArgs == null)
				throw new ArgumentNullException ("startArgs");
			this.StartArgs = startArgs;
		}
		
		/// <summary>
		/// Names of assemblies that are user code.
		/// </summary>
		public List<AssemblyName> UserAssemblyNames { get; set; }
		
		/// <summary>
		/// A mapping of AssemblyNames to their paths.
		/// </summary>
		public Dictionary<string, string> AssemblyPathMap { get; set; }

		/// <summary>
		/// A mapping of AssemblyNames to their symbol paths. If no symbols maps are provided, the symbols will be found next to the assemblies in AssemblyPathMap.
		/// </summary>
		public Dictionary<string, string> SymbolPathMap { get; set; }
		
		/// <summary>
		/// The session will output this to the debug log as soon as it starts. It can be used to log warnings from
		/// creating the SoftDebuggerStartInfo
		/// </summary>
		public string LogMessage { get; set; }
		
		/// <summary>
		/// Args for starting the debugger connection.
		/// </summary>
		public SoftDebuggerStartArgs StartArgs { get; set; }
	}
	
	public interface ISoftDebuggerConnectionProvider
	{
		IAsyncResult BeginConnect (DebuggerStartInfo dsi, AsyncCallback callback);
		void EndConnect (IAsyncResult result, out VirtualMachine vm, out string appName);
		void CancelConnect (IAsyncResult result);
		bool ShouldRetryConnection (Exception ex);
	}
	
	public abstract class SoftDebuggerStartArgs
	{
		protected SoftDebuggerStartArgs ()
		{
			MaxConnectionAttempts = 1;
			TimeBetweenConnectionAttempts = 500;
		}
		
		public abstract ISoftDebuggerConnectionProvider ConnectionProvider { get; }
		
		/// <summary>
		/// Maximum number of connection attempts. Zero or less means infinite attempts. Default is 1.
		/// </summary>
		public int MaxConnectionAttempts { get; set; }
		
		/// <summary>
		/// The time between connection attempts, in milliseconds. Default is 500.
		/// </summary>
		public int TimeBetweenConnectionAttempts { get; set; }
	}
	
	public abstract class SoftDebuggerRemoteArgs : SoftDebuggerStartArgs
	{
		protected SoftDebuggerRemoteArgs (string appName, IPAddress address, int debugPort, int outputPort)
		{
			if (address == null)
				throw new ArgumentNullException ("address");
			if (debugPort < 0)
				throw new ArgumentException ("Debug port cannot be less than zero", "debugPort");
			
			this.AppName = appName;
			this.Address = address;
			this.DebugPort = debugPort;
			this.OutputPort = outputPort;
		}
		
		/// <summary>
		/// The IP address for the connection.
		/// </summary>
		public IPAddress Address { get; private set; }
		
		/// <summary>
		/// Port for the debugger connection. Zero means random port.
		/// </summary>
		public int DebugPort { get; private set; }
		
		/// <summary>
		/// Port for the console connection. Zero means random port, less than zero means that output is not redirected.
		/// </summary>
		public int OutputPort { get; private set; }
		
		/// <summary>
		/// Application name that will be shown in the debugger.
		/// </summary>
		public string AppName { get; private set; }
		
		public bool RedirectOutput { get { return OutputPort >= 0; } }
	}
	
	/// <summary>
	/// Args for the debugger to listen for an incoming connection from a debuggee.
	/// </summary>
	public sealed class SoftDebuggerListenArgs : SoftDebuggerRemoteArgs
	{
		public SoftDebuggerListenArgs (string appName, IPAddress address, int debugPort)
			: this (appName, address, debugPort, -1) {}
		
		public SoftDebuggerListenArgs (string appName, IPAddress address, int debugPort, int outputPort)
			: base (appName, address, debugPort, outputPort)
		{
		}
		
		public override ISoftDebuggerConnectionProvider ConnectionProvider { get { return null; } }
	}
	
	/// <summary>
	/// Args for the debugger to connect to target that is listening.
	/// </summary>
	public sealed class SoftDebuggerConnectArgs : SoftDebuggerRemoteArgs
	{
		public SoftDebuggerConnectArgs (string appName, IPAddress address, int debugPort)
			: this (appName, address, debugPort, -1) {}
		
		public SoftDebuggerConnectArgs (string appName, IPAddress address, int debugPort, int outputPort)
			: base (appName, address, debugPort, outputPort)
		{
			if (debugPort == 0)
				throw new ArgumentException ("Debug port cannot be zero when connecting", "debugPort");
			if (outputPort == 0)
				throw new ArgumentException ("Output port cannot be zero when connecting", "outputPort");
		}
		
		public override ISoftDebuggerConnectionProvider ConnectionProvider { get { return null; } }
	}

	/// <summary>
	/// Options for the debugger to start a process directly.
	/// </summary>
	public sealed class SoftDebuggerLaunchArgs : SoftDebuggerStartArgs
	{
		public SoftDebuggerLaunchArgs (string monoRuntimePrefix, Dictionary<string,string> monoRuntimeEnvironmentVariables)
		{
			this.MonoRuntimePrefix = monoRuntimePrefix;
			this.MonoRuntimeEnvironmentVariables = monoRuntimeEnvironmentVariables;
		}
		
		/// <summary>
		/// Prefix into which the target Mono runtime is installed.
		/// </summary>
		public string MonoRuntimePrefix { get; private set; }
		
		/// <summary>
		/// Environment variables for the Mono runtime.
		/// </summary>
		public Dictionary<string,string> MonoRuntimeEnvironmentVariables { get; private set; }

		/// <summary>
		/// Launcher for the external console. May be null if the app does not run on an external console.
		/// </summary>
		public Mono.Debugger.Soft.LaunchOptions.TargetProcessLauncher ExternalConsoleLauncher { get; set; }

		/// <summary>
		/// Gets or sets the name of the mono executable file. e.g. "mono", "mono32", "mono64"...
		/// </summary>
		public string MonoExecutableFileName { get; set; }
		
		public override ISoftDebuggerConnectionProvider ConnectionProvider { get { return null; } }

		internal Mono.Debugger.Soft.LaunchOptions.ProcessLauncher CustomProcessLauncher { get; set; }
	}
}