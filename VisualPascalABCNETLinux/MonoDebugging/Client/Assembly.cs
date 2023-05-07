//
// Assembly.cs
//
// Author:
//       Jonathan Chang <t-jochang@microsoft.com>
//
// Copyright (c) 2022 
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
namespace Mono.Debugging.Client
{
	/// <summary>
	/// Represents the assembly loaded during the debugging session.
	/// </summary>
	public class Assembly
	{
		public Assembly (string name, string path, bool optimized, bool userCode, string symbolStatus, string symbolFile, int? order, string version, string timestamp, string address, string process, string appdomain, long? processId, bool hasSymbol = false, bool isDynamic = false)
		{
			Name = name;
			Path = path;
			Optimized = optimized;
			SymbolStatus = symbolStatus;
			SymbolFile = symbolFile;
			Order = order.GetValueOrDefault (-1);
			TimeStamp = timestamp;
			Address = address;
			Process = process;
			AppDomain = appdomain;
			Version = version;
			UserCode = userCode;
			ProcessId = processId;
			IsDynamic = isDynamic;
			HasSymbols = hasSymbol;
		}

		public Assembly (string path)
		{
			Path = path;
		}

		/// <summary>
		/// Represents the name of the assembly.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Represents the local path of the assembly is loaded from.
		/// </summary>
		public string Path { get; private set; }

		/// <summary>
		/// Shows if the assembly has been optimized, true if the assembly is optimized.
		/// </summary>
		public bool Optimized { get; private set; }

		/// <summary>
		/// Shows if the assembly is considered 'user code' by a debugger that supports 'Just My Code'.True if it's considered.
		/// </summary>
		public bool UserCode { get; private set; }

		/// <summary>
		/// Represents the Description on if symbols were found for the assembly (ex: 'Symbols Loaded', 'Symbols not found', etc.
		/// </summary>
		public string SymbolStatus { get; private set; }

		/// <summary>
		/// Represents the Logical full path to the symbol file. The exact definition is implementation defined.
		/// </summary>
		public string SymbolFile { get; private set; }

		/// <summary>
		/// Represents the order in which the assembly was loaded.
		/// </summary>
		public int Order { get; private set; } = -1;

		/// <summary>
		/// Represents the version of assembly.
		/// </summary>
		public string Version { get; private set; }

		/// <summary>
		/// Represents the time when the assembly was built in the units of UNIX timestamp formatted as a 64-bit unsigned decimal number in a string.
		/// </summary>
		public string TimeStamp { get; private set; }

		/// <summary>
		/// Represents the Address where the assembly was loaded as a 64-bit unsigned decimal number.
		/// </summary>
		public string Address { get; private set; }

		/// <summary>
		/// Represent the process name and process ID the assembly is loaded. 
		/// </summary>
		public string Process { get; private set; }

		/// <summary>
		/// Represent the name of the AppDomain where the assembly is loaded.
		/// </summary>
		public string AppDomain { get; private set; }

		/// <summary>
		/// Represent the process ID the assembly is loaded. 
		/// </summary>
		public long? ProcessId { get; private set; } = -1;

		/// <summary>
		/// Indicates if the assembly has symbol file. Mainly use for mono project.
		/// </summary>
		public bool HasSymbols { get; private set; }

		/// <summary>
		/// Indicate if the assembly is a dynamic. Mainly use for mono project. 
		/// </summary>
		public bool IsDynamic { get; private set; }
	}
}