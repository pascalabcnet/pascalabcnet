//
// PortablePdbData.cs
//
// Author:
//       David Karlaš <david.karlas@xamarin.com>
//
// Copyright (c) 2017 Xamarin, Inc (http://www.xamarin.com)
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
using Mono.Debugger.Soft;
using System.IO;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;

namespace Mono.Debugging.Soft
{
	class PortablePdbData
	{
		static readonly Guid AsyncMethodSteppingInformationBlob = new Guid ("54FD2AC5-E925-401A-9C2A-F94F171072F8");
		static readonly Guid StateMachineHoistedLocalScopes = new Guid ("6DA9A61E-F8C7-4874-BE62-68BC5630DF71");
		static readonly Guid DynamicLocalVariables = new Guid ("83C563C4-B4F3-47D5-B824-BA5441477EA8");
		static readonly Guid TupleElementNames = new Guid ("ED9FDF71-8879-4747-8ED3-FE5EDE3CE710");
		static readonly Guid DefaultNamespace = new Guid ("58b2eab6-209f-4e4e-a22c-b2d0f910c782");
		static readonly Guid EncLocalSlotMap = new Guid ("755F52A8-91C5-45BE-B4B8-209571E552BD");
		static readonly Guid EncLambdaAndClosureMap = new Guid ("A643004C-0240-496F-A783-30D64F4979DE");
		static readonly Guid SourceLinkGuid = new Guid ("CC110556-A091-4D38-9FEC-25AB9A351A6A");
		static readonly Guid EmbeddedSource = new Guid ("0E8A571B-6926-466E-B4AD-8AB04611F5FE");

		public static bool IsPortablePdb (string pdbFileName)
		{
			if (string.IsNullOrEmpty (pdbFileName) || !File.Exists (pdbFileName))
				return false;
			using (var file = new FileStream (pdbFileName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
				var data = new byte [4];
				int read = file.Read (data, 0, data.Length);
				return read == 4 && BitConverter.ToUInt32 (data, 0) == 0x424a5342;
			}
		}

		readonly byte [] pdbBytes;
		private readonly string pdbFileName;

		public PortablePdbData (string pdbFileName)
		{
			this.pdbFileName = Path.GetFullPath (pdbFileName);
		}

		public PortablePdbData (byte [] pdbBytes)
		{
			this.pdbBytes = pdbBytes ?? throw new ArgumentNullException (nameof(pdbBytes));
		}

		internal class SoftScope
		{
			public int LiveRangeStart;

			public int LiveRangeEnd;
		}

		Stream GetStream()
		{
			if (pdbBytes != null)
				return new MemoryStream (pdbBytes);
			return new FileStream (pdbFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		public string GetSourceLinkBlob ()
		{
			/*using (var provider = MetadataReaderProvider.FromPortablePdbStream (GetStream ())) {
				var pdbReader = provider.GetMetadataReader ();

				var jsonBlob =
					pdbReader.GetCustomDebugInformation (EntityHandle.ModuleDefinition)
					.Select (cdiHandle => pdbReader.GetCustomDebugInformation (cdiHandle))
					.Where (cdi => pdbReader.GetGuid (cdi.Kind) == SourceLinkGuid)
					.Select (cdi => pdbReader.GetBlobBytes (cdi.Value))
					.FirstOrDefault ();

				if (jsonBlob == null)
					return null;

				return System.Text.Encoding.UTF8.GetString (jsonBlob);
			}*/
			return null;
		}

		// We need proxy method to make sure VS2013/15 doesn't crash(this method won't be called if portable .pdb file doesn't exist, which means 2017+)
		[MethodImpl (MethodImplOptions.NoInlining)]
		internal SoftScope [] GetHoistedScopes (MethodMirror method) => GetHoistedScopesPrivate (method);

		internal SoftScope [] GetHoistedScopesPrivate (MethodMirror method)
		{
			/*using (var metadataReader = MetadataReaderProvider.FromPortablePdbStream (GetStream ())) {
				var reader = metadataReader.GetMetadataReader ();
				var methodHandle = MetadataTokens.MethodDefinitionHandle (method.MetadataToken);
				var customDebugInfos = reader.GetCustomDebugInformation (methodHandle);
				foreach (var item in customDebugInfos) {
					var debugInfo = reader.GetCustomDebugInformation (item);
					if (reader.GetGuid (debugInfo.Kind) == StateMachineHoistedLocalScopes) {
						var bytes = reader.GetBlobBytes (debugInfo.Value);
						var result = new SoftScope [bytes.Length / 8];
						for (int i = 0; i < bytes.Length; i += 8) {
							var offset = BitConverter.ToInt32 (bytes, i);
							var len = BitConverter.ToInt32 (bytes, i + 4);
							result [i / 8] = new SoftScope () {
								LiveRangeStart = offset,
								LiveRangeEnd = offset + len
							};
						}
						return result;
					}
				}
			}*/
			return null;
		}

		// We need proxy method to make sure VS2013/15 doesn't crash(this method won't be called if portable .pdb file doesn't exist, which means 2017+)
		[MethodImpl (MethodImplOptions.NoInlining)]
		internal string [] GetTupleElementNames (MethodMirror method, int localVariableIndex) => TupleElementNamesPrivate (method, localVariableIndex);

		/*private static string [] DecodeTupleElementNames (BlobReader reader)
		{
			var list = new List<string> ();
			while (reader.RemainingBytes > 0) {
				int byteCount = reader.IndexOf (0);
				string value = reader.ReadUTF8 (byteCount);
				byte terminator = reader.ReadByte ();
				list.Add (value.Length == 0 ? null : value);
			}
			return list.ToArray ();
		}*/

		internal string [] TupleElementNamesPrivate (MethodMirror method, int localVariableIndex)
		{
			/*using (var metadataReader = MetadataReaderProvider.FromPortablePdbStream (GetStream())) {
				var reader = metadataReader.GetMetadataReader ();
				var methodHandle = MetadataTokens.MethodDefinitionHandle (method.MetadataToken);
				var localScopes = reader.GetLocalScopes (methodHandle);
				// localVariableIndex is not really il_index, but sequential index when fetching locals
				// hence use Skip(index) instead of Index matching.
				var localVar = localScopes.Select (s => reader.GetLocalScope (s)).SelectMany (s => s.GetLocalVariables ()).Skip (localVariableIndex).First ();
				var customDebugInfos = reader.GetCustomDebugInformation (localVar);
				foreach (var item in customDebugInfos) {
					var debugInfo = reader.GetCustomDebugInformation (item);
					if (reader.GetGuid (debugInfo.Kind) == TupleElementNames) {
						return DecodeTupleElementNames (reader.GetBlobReader (debugInfo.Value));
					}
				}
			}*/
			return null;
		}
	}
}
