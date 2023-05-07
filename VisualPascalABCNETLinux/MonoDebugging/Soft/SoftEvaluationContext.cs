// 
// SoftEvaluationContext.cs
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
using Mono.Debugging.Evaluation;
using Mono.Debugger.Soft;
using DC = Mono.Debugging.Client;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Mono.Debugging.Soft
{
	public class SoftEvaluationContext: EvaluationContext
	{
		SoftDebuggerSession session;
		int stackVersion;
		StackFrame frame;
		bool sourceAvailable;

		public ThreadMirror Thread { get; set; }
		public AppDomainMirror Domain { get; set; }

		public SoftEvaluationContext (SoftDebuggerSession session, StackFrame frame, DC.EvaluationOptions options): base (options)
		{
			Frame = frame;
			Thread = frame.Thread;
			Domain = frame.Domain;

			string method = frame.Method.Name;
			if (frame.Method.DeclaringType != null)
				method = frame.Method.DeclaringType.FullName + "." + method;

			var sourceLink = session.GetSourceLink(frame.Method, frame.FileName);

			var location = new DC.SourceLocation (method, frame.FileName, frame.LineNumber, frame.ColumnNumber, frame.EndLineNumber, frame.EndColumnNumber, frame.Location.SourceFileHash, sourceLink);
			string language;

			if (frame.Method != null) {
				language = frame.IsNativeTransition ? "Transition" : "Managed";
			} else {
				language = "Native";
			}
			
			Evaluator = session.GetEvaluator (new DC.StackFrame (frame.ILOffset, location, language, session.IsExternalCode (frame), true));
			Adapter = session.Adaptor;
			this.session = session;
			stackVersion = session.StackVersion;
			sourceAvailable = !string.IsNullOrEmpty (frame.FileName) && System.IO.File.Exists (frame.FileName);
		}
		
		public StackFrame Frame {
			get {
				if (stackVersion != session.StackVersion)
					UpdateFrame ();
				return frame;
			}
			set {
				frame = value;
			}
		}
		
		public bool SourceCodeAvailable {
			get {
				if (stackVersion != session.StackVersion)
					sourceAvailable = !string.IsNullOrEmpty (Frame.FileName) && System.IO.File.Exists (Frame.FileName);
				return sourceAvailable;
			}
		}
		
		public SoftDebuggerSession Session {
			get { return session; }
		}
		
		public override void WriteDebuggerError (Exception ex)
		{
			session.WriteDebuggerOutput (true, ex.ToString ());
		}
		
		public override void WriteDebuggerOutput (string message, params object[] values)
		{
			session.WriteDebuggerOutput (false, string.Format (message, values));
		}

		public override void CopyFrom (EvaluationContext ctx)
		{
			base.CopyFrom (ctx);

			var other = (SoftEvaluationContext) ctx;
			frame = other.frame;
			stackVersion = other.stackVersion;
			Thread = other.Thread;
			session = other.session;
			Domain = other.Domain;
		}

		internal static bool IsValueTypeOrPrimitive (TypeMirror type)
		{
			return type != null && (type.IsValueType || type.IsPrimitive);
		}

		static bool IsValueTypeOrPrimitive (Type type)
		{
			return type != null && (type.IsValueType || type.IsPrimitive);
		}
		
		public Value RuntimeInvoke (MethodMirror method, object target, Value[] values)
		{
			Value[] outArgs;
			return RuntimeInvoke (method, target, values, false, out outArgs);
		}

		public Value RuntimeInvoke (MethodMirror method, object target, Value[] values, out Value[] outArgs)
		{
			return RuntimeInvoke (method, target, values, true, out outArgs);
		}
		
		Value RuntimeInvoke (MethodMirror method, object target, Value[] values, bool enableOutArgs, out Value[] outArgs)
		{
			outArgs = null;
			if (values != null) {
				// Some arguments may need to be boxed
				var mparams = method.GetParameters ();
				if (mparams.Length != values.Length)
					throw new EvaluatorException ("Invalid number of arguments when calling: " + method.Name);
				
				for (int n = 0; n < mparams.Length; n++) {
					var tm = mparams[n].ParameterType;
					if (tm.IsValueType || tm.IsPrimitive || tm.FullName.StartsWith ("System.Nullable`1", StringComparison.Ordinal))
						continue;

					var type = Adapter.GetValueType (this, values[n]);
					var argTypeMirror = type as TypeMirror;
					var argType = type as Type;

					if (IsValueTypeOrPrimitive (argTypeMirror) || IsValueTypeOrPrimitive (argType)) {
						// A value type being assigned to a parameter which is not a value type. The value has to be boxed.
						try {
							values[n] = Thread.Domain.CreateBoxedValue (values [n]);
						} catch (NotSupportedException) {
							// This runtime doesn't support creating boxed values
							throw new EvaluatorException ("This runtime does not support creating boxed values.");
						}
					}
				}
			}

			if (!method.IsStatic && method.DeclaringType.IsClass && !IsValueTypeOrPrimitive (method.DeclaringType)) {
				object type = Adapter.GetValueType (this, target);
				var targetTypeMirror = type as TypeMirror;
				var targetType = type as Type;

				if ((target is StructMirror && ((StructMirror) target).Type != method.DeclaringType) ||
				    (IsValueTypeOrPrimitive (targetTypeMirror) || IsValueTypeOrPrimitive (targetType))) {
					// A value type being assigned to a parameter which is not a value type. The value has to be boxed.
					try {
						target = Thread.Domain.CreateBoxedValue ((Value) target);
					} catch (NotSupportedException) {
						// This runtime doesn't support creating boxed values
						throw new EvaluatorException ("This runtime does not support creating boxed values.");
					}
				}
			}

			try {
				return method.Evaluate (target is TypeMirror ? null : (Value) target, values);
			} catch (NotSupportedException) {
				AssertTargetInvokeAllowed ();
				var threadState = Thread.ThreadState;
				if ((threadState & ThreadState.WaitSleepJoin) == ThreadState.WaitSleepJoin) {
					DC.DebuggerLoggingService.LogMessage ("Thread state before evaluation is {0}", threadState);
					throw new EvaluatorException ("Evaluation is not allowed when the thread is in 'Wait' state");
				}
				var mc = new MethodCall (this, method, target, values, enableOutArgs);
				//Since runtime is returning NOT_SUSPENDED error if two methods invokes are executed
				//at same time we have to lock invoking to prevent this...
				lock (method.VirtualMachine) {
					Adapter.AsyncExecute (mc, Options.EvaluationTimeout);
				}
				if (enableOutArgs) {
					outArgs = mc.OutArgs;
				}
				return mc.ReturnValue;
			}
		}

		void UpdateFrame ()
		{
			stackVersion = session.StackVersion;
			foreach (StackFrame f in Thread.GetFrames ()) {
				if (f.FileName == Frame.FileName && f.LineNumber == Frame.LineNumber && f.ILOffset == Frame.ILOffset) {
					Frame = f;
					break;
				}
			}
		}

		public override bool SupportIEnumerable {
			get {
				return session.VirtualMachine.Version.AtLeast (2, 35);
			}
		}
	}
}
