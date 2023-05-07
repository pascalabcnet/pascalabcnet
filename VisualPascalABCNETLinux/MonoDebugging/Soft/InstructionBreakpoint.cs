using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Debugging.Client;

namespace Mono.Debugging.Soft
{
	[Serializable]
	public class InstructionBreakpoint : Breakpoint
	{
		public string MethodName { get; set; }
		public long ILOffset { get; private set; }

		public InstructionBreakpoint (string fileName, int line, int column, long ilOffset) : base (fileName, line, column)
		{
			this.ILOffset = ilOffset;
		}

	}
}
