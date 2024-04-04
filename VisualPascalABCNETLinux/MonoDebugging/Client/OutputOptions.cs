using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.Debugging.Client
{
	[Serializable]
	public class OutputOptions
	{
		public bool ModuleLoaded { get; set; }
		public bool ModuleUnoaded { get; set; }
		public bool ExceptionMessage { get; set; }
		public bool SymbolSearch { get; set; }
		public bool ThreadExited { get; set; }
		public bool ProcessExited { get; set; }

	}
}
