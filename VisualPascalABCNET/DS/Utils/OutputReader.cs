// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace VisualPascalABC.Utils
{
	/// <summary>
	/// A threaded <see cref="Process.StandardOutput"/> or
	/// <see cref="Process.StandardError"/> reader.
	/// </summary>
	public class OutputReader
	{
		StreamReader reader;
		string output = String.Empty;
		Thread thread;
		
		public event LineReceivedEventHandler LineReceived;
		
		public OutputReader(StreamReader reader)
		{
			this.reader = reader;
		}
		
		/// <summary>
		/// Starts reading the output stream.
		/// </summary>
		public void Start()
		{
			thread = new Thread(new ThreadStart(ReadOutput));
            thread.Start();
		}
		
		/// <summary>
		/// Gets the text output read from the reader.
		/// </summary>
		public string Output {
			get {
				return output;
			}
		}
		
		/// <summary>
		/// Waits for the reader to finish.
		/// </summary>
		public void WaitForFinish()
		{
			if (thread != null) {
				thread.Join();
			}
		}
		
		/// <summary>
		/// Raises the <see cref="LineReceived"/> event.
		/// </summary>
		/// <param name="line"></param>
		protected void OnLineReceived(string line)
		{
			if (LineReceived != null) {
				LineReceived(this, new LineReceivedEventArgs(line));
			}
		}
		
		/// <summary>
		/// Reads the output stream on a different thread.
		/// </summary>
		void ReadOutput()
		{
			output = String.Empty;
			StringBuilder outputBuilder = new StringBuilder();
			
			bool endOfStream = false;
			while(!endOfStream)
			{
                //reader.
				string line = reader.ReadLine();
				
				if (line != null) {
					outputBuilder.Append(line);
					outputBuilder.Append(Environment.NewLine);
					OnLineReceived(line);
				} else {
					endOfStream = true;
				}
			} 
			
			output = outputBuilder.ToString();
		}
	}
}
