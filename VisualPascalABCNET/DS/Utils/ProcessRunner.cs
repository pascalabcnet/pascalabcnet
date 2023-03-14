// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace VisualPascalABC.Utils
{
    public delegate void LineReceivedEventHandler(object sender, LineReceivedEventArgs e);

    /// <summary>
    /// The arguments for the <see cref="LineReceivedEventHandler"/> event.
    /// </summary>
    public class LineReceivedEventArgs : EventArgs
    {
        string line = String.Empty;

        public LineReceivedEventArgs(string line)
        {
            this.line = line;
        }

        public string Line
        {
            get
            {
                return line;
            }
        }
    }

	/// <summary>
	/// Runs a process that sends output to standard output and to
	/// standard error.
	/// </summary>
    [Serializable]
	public class ProcessRunner : IDisposable
	{
		public Process process;
		string standardOutput = String.Empty;
		string workingDirectory = String.Empty;
		OutputReader standardOutputReader;
		OutputReader standardErrorReader;
        bool RedirectIO;

        

        //AppDomain Domain;
        string DomainID = "PascalABCNETRunner";

		/// <summary>
		/// Triggered when the process has exited.
		/// </summary>
		public event EventHandler ProcessExited;
		
		/// <summary>
		/// Triggered when a line of text is read from the standard output.
		/// </summary>
		public event LineReceivedEventHandler OutputLineReceived;
		
		/// <summary>
		/// Triggered when a line of text is read from the standard error.
		/// </summary>
		public event LineReceivedEventHandler ErrorLineReceived;
		
		/// <summary>
		/// Creates a new instance of the <see cref="ProcessRunner"/>.
		/// </summary>
		public ProcessRunner()
		{
            //Domain = AppDomain.CreateDomain(DomainID);
            //Domain.UnhandledException += new UnhandledExceptionEventHandler(Domain_UnhandledException);
		}

        void Domain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("x");
        }
		
		/// <summary>
		/// Gets or sets the process's working directory.
		/// </summary>
		public string WorkingDirectory {
			get {
				return workingDirectory;
			}
			
			set {
				workingDirectory = value;
			}
		}

		/// <summary>
		/// Gets the standard output returned from the process.
		/// </summary>
		public string StandardOutput {
			get {
				string output = String.Empty;
				if (standardOutputReader != null) {
					output = standardOutputReader.Output;
				}
				return output;
			}
		}
		
		/// <summary>
		/// Gets the standard error output returned from the process.
		/// </summary>
		public string StandardError {
			get {
				string output = String.Empty;
				if (standardErrorReader != null) {
					output = standardErrorReader.Output;
				}
				return output;
			}
		}
		
		/// <summary>
		/// Releases resources held by the <see cref="ProcessRunner"/>
		/// </summary>
		public void Dispose()
		{
		}
		
		/// <summary>
		/// Gets the process exit code.
		/// </summary>
		public int ExitCode {
			get {	
				int exitCode = 0;
				if (process != null) {
					exitCode = process.ExitCode;
				}
				return exitCode;
			}
		}
		
		/// <summary>
		/// Waits for the process to exit.
		/// </summary>
		public void WaitForExit()
		{
			WaitForExit(Int32.MaxValue);
		}
		
		/// <summary>
		/// Waits for the process to exit.
		/// </summary>
		/// <param name="timeout">A timeout in milliseconds.</param>
		/// <returns><see langword="true"/> if the associated process has 
		/// exited; otherwise, <see langword="false"/></returns>
		public bool WaitForExit(int timeout)
		{
			if (process == null) {
				throw new Exception("NoProcessRunning");
			}
			
			bool exited = process.WaitForExit(timeout);
			
			if (exited) {
				standardOutputReader.WaitForFinish();
				standardErrorReader.WaitForFinish();
			}
			
			return exited;
		}
		
		public bool IsRunning {
			get {
				bool isRunning = false;
				
				if (process != null) {
					isRunning = !process.HasExited;
				}
				
				return isRunning;
			}
		}

        string GenerateBatFileForRunWithPause(string command, string arguments)
        {
            string AppPatch = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            string PauseExe = AppPatch + @"\Pause.exe";
            if (!File.Exists(PauseExe) || !Directory.Exists(AppPatch+@"\Temp"))
                return null;
            string BatNameTempl = AppPatch + @"\Temp\" + Path.ChangeExtension(Path.GetFileNameWithoutExtension(command)+"{0}", ".bat");
            string BatName = string.Format(BatNameTempl, "");
            int i=0;
            while (File.Exists(BatName))
                BatName = string.Format(BatNameTempl, i++);
            try
            {
                TextWriter batFile = File.CreateText(BatName);
                batFile.WriteLine("@echo off");
                batFile.Write("\"" + command + "\"");
                if (arguments != string.Empty)
                    batFile.Write(" " + arguments);
                batFile.WriteLine();
                batFile.WriteLine("\"" + PauseExe + "\"");
                batFile.Close();
                return BatName;
            }
            catch (Exception)
            {
                return null;
            }
            //return null;
        }
        public string TempBatFile = null;
        public void Start(string command, string arguments, bool redirectIO, bool redirectErrors, bool RunWithPause, bool attachDebugger, bool fictive_attach)
		{
            RedirectIO=redirectIO;
			process = new Process();
            string BatFile = null;
            process.StartInfo.ErrorDialog = false;
            if (RunWithPause && (BatFile = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath),"ProgrammRunner.exe")/*GenerateBatFileForRunWithPause(command, arguments)*/) != null)
            {
                TempBatFile = BatFile;
                //process.StartInfo.UseShellExecute = false;
                process.StartInfo.FileName = BatFile;
                process.StartInfo.WorkingDirectory = workingDirectory;
                //process.StartInfo.Arguments = command.Replace(" ","\" \"") + " " + PascalABCCompiler.StringResourcesLanguage.CurrentLCID + " " + arguments;
                if (command.Contains(" "))
                    command = "\"" + command + "\"";
                process.StartInfo.Arguments = command + " " + PascalABCCompiler.StringResourcesLanguage.CurrentLCID + " " + arguments;
            }
            else
            {
                process.StartInfo.UseShellExecute = false;
                #if (DEBUG)
                if (VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.StandartCompiler.InternalDebug.RunOnMono && !(attachDebugger && !fictive_attach))
                {
                	string mono_path = @"C:\Program Files\Mono-2.0.1\bin";//Environment.GetEnvironmentVariable("MONO_PATH", EnvironmentVariableTarget.Machine);
                	process.StartInfo.FileName = Path.Combine(mono_path,"mono.exe");
                	process.StartInfo.WorkingDirectory = workingDirectory;
               	 	process.StartInfo.Arguments = command+" "+arguments;
                }
                else
                #endif
                {
                	process.StartInfo.FileName = command;
                	process.StartInfo.WorkingDirectory = workingDirectory;
               	 	process.StartInfo.Arguments = arguments;
                }
            }
            //process.StartInfo.Domain = Domain.FriendlyName;
            if (RedirectIO)
            {
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
            }
            if (redirectErrors)
            {
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;
            }
            if (ProcessExited != null)
            {
                process.EnableRaisingEvents = true;
                process.Exited += new EventHandler(OnProcessExited);
			}
            process.Start();
            //ssyy
            process.PriorityClass = ProcessPriorityClass.BelowNormal;
            //\ssyy
            if (attachDebugger)
            {
                WorkbenchServiceFactory.DebuggerManager.Attach((uint)process.Id,command,!fictive_attach,false);
            }
            if (redirectIO)
            process.StandardInput.WriteLine("GO");
            
		}


        void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }
		
		/// <summary>
		/// Kills the running process.
		/// </summary>
        public void Kill()
        {
            if (process != null)
            {
                if (!process.HasExited)
                {
                    process.Kill();
                    process.Close();
                    process.Dispose();
                    process = null;
                    /*if (RedirectOutput)
                    {
                        standardOutputReader.WaitForFinish();
                        standardErrorReader.WaitForFinish();
                    }*/
                }
                else
                {
                    process = null;
                }
            }
            // Control-C does not seem to work.
            //GenerateConsoleCtrlEvent((int)ConsoleEvent.ControlC, 0);
        }		
		
		/// <summary>
		/// Raises the <see cref="ProcessExited"/> event.
		/// </summary>
		protected void OnProcessExited(object sender, EventArgs e)
		{
			if (ProcessExited != null) {

                if (RedirectIO)
                {
                    //standardOutputReader.WaitForFinish();
                    //standardErrorReader.WaitForFinish();
                }
				ProcessExited(this, e);
			}
		}
		
		/// <summary>
		/// Raises the <see cref="OutputLineReceived"/> event.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The line received event arguments.</param>
		protected void OnOutputLineReceived(object sender, LineReceivedEventArgs e)
		{
			if (OutputLineReceived != null) {
				OutputLineReceived(this, e);
			}
		}
		
		/// <summary>
		/// Raises the <see cref="ErrorLineReceived"/> event.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The line received event arguments.</param>
		protected void OnErrorLineReceived(object sender, LineReceivedEventArgs e)
		{
			if (ErrorLineReceived != null) {
				ErrorLineReceived(this, e);
			}
		}		

		enum ConsoleEvent
		{
			ControlC = 0,
			ControlBreak = 1
		};
		
		[DllImport("kernel32.dll", SetLastError=true)] 
		static extern int GenerateConsoleCtrlEvent(int dwCtrlEvent, int dwProcessGroupId);
	}
}
