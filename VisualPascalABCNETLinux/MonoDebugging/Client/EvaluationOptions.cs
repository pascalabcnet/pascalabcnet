// 
// EvaluationOptions.cs
//  
// Author:
//       Lluis Sanchez Gual <lluis@novell.com>
// 
// Copyright (c) 2009 Novell, Inc (http://www.novell.com)
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
	[Serializable]
	public class EvaluationOptions
	{
		bool allowMethodEvaluation;
		bool allowToStringCalls;

		public const char Ellipsis = '…';

		public static EvaluationOptions DefaultOptions {
			get {
				EvaluationOptions ops = new EvaluationOptions ();
				ops.EvaluationTimeout = 1000;
				ops.MemberEvaluationTimeout = 5000;
				ops.AllowTargetInvoke = true;
				ops.AllowMethodEvaluation = true;
				ops.AllowToStringCalls = true;
				ops.FlattenHierarchy = true;
				ops.GroupPrivateMembers = true;
				ops.GroupStaticMembers = true;
				ops.UseExternalTypeResolver = true;
				ops.IntegerDisplayFormat = IntegerDisplayFormat.Decimal;
				ops.CurrentExceptionTag = "$exception";
				ops.EllipsizeStrings = true;
				ops.EllipsizedLength = 100;
				ops.ChunkRawStrings = false;
				ops.StackFrameFormat = new StackFrameFormat ();
				return ops;
			}
		}

		public EvaluationOptions Clone ()
		{
			var clone = (EvaluationOptions)MemberwiseClone ();
			clone.StackFrameFormat = new StackFrameFormat (StackFrameFormat);
			return clone;
		}

		public bool ChunkRawStrings { get; set; }

		public bool EllipsizeStrings { get; set; }
		public int EllipsizedLength { get; set; }

		public int EvaluationTimeout { get; set; }
		public int MemberEvaluationTimeout { get; set; }
		public bool AllowTargetInvoke { get; set; }

		public bool AllowMethodEvaluation {
			get { return allowMethodEvaluation && AllowTargetInvoke; }
			set { allowMethodEvaluation = value; }
		}

		public bool AllowToStringCalls {
			get { return allowToStringCalls && AllowTargetInvoke; }
			set { allowToStringCalls = value; }
		}

		public bool AllowDisplayStringEvaluation {
			get { return AllowTargetInvoke; }
		}

		public bool AllowDebuggerProxy {
			get { return AllowTargetInvoke; }
		}

		public bool FlattenHierarchy { get; set; }

		public bool GroupPrivateMembers { get; set; }

		public bool GroupStaticMembers { get; set; }

		public bool UseExternalTypeResolver { get; set; }

		public IntegerDisplayFormat IntegerDisplayFormat { get; set; }

		public string CurrentExceptionTag { get; set; }

		public bool IEnumerable { get; set; }

		public StackFrameFormat StackFrameFormat { get; set; }
	}

	public class StackFrameFormat
	{
		public bool Line { get; set; } = true;

		public bool Module { get; set; } = false;

		public bool ParameterNames { get; set; } = true;

		public bool ParameterTypes { get; set; } = true;

		public bool ParameterValues { get; set; } = false;

		/// <summary>
		/// Default is null. Which means do same as "ProjectAssembliesOnly" setting.
		/// </summary>
		public bool? ExternalCode { get; set; } = null;

		public StackFrameFormat ()
		{
		}

		public StackFrameFormat (StackFrameFormat copy)
		{
			Line = copy.Line;
			Module = copy.Module;
			ParameterNames = copy.ParameterNames;
			ParameterTypes = copy.ParameterTypes;
			ParameterValues = copy.ParameterValues;
			ExternalCode = copy.ExternalCode;
		}
	}

	public enum IntegerDisplayFormat
	{
		Decimal,
		Hexadecimal
	}
}
