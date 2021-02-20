﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision: 2274 $</version>
// </file>

using System;

namespace Debugger
{
	[Serializable]
	public class ProcessEventArgs: DebuggerEventArgs
	{
		Process process;
		
		[Debugger.Tests.Ignore]
		public Process Process {
			get {
				return process;
			}
		}

		public ProcessEventArgs(Process process): base(process == null ? null : process.Debugger)
		{
			this.process = process;
		}
	}
}
