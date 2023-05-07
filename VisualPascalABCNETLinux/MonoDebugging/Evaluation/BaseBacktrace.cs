// 
// Backtrace.cs
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
using System.Collections.Generic;

using Mono.Debugging.Client;
using Mono.Debugging.Backend;

namespace Mono.Debugging.Evaluation
{
	public abstract class BaseBacktrace: RemoteFrameObject, IBacktrace
	{
		readonly Dictionary<int, FrameInfo> frameInfo = new Dictionary<int, FrameInfo> ();
		
		protected BaseBacktrace (ObjectValueAdaptor adaptor)
		{
			Adaptor = adaptor;
		}
		
		public abstract StackFrame[] GetStackFrames (int firstIndex, int lastIndex);
		
		public ObjectValueAdaptor Adaptor { get; set; }
		
		protected abstract EvaluationContext GetEvaluationContext (int frameIndex, EvaluationOptions options);
		
		public abstract int FrameCount { get; }
	
		public virtual ObjectValue[] GetLocalVariables (int frameIndex, EvaluationOptions options)
		{
			var frame = GetFrameInfo (frameIndex, options, false);
			var list = new List<ObjectValue> ();
			
			if (frame == null) {
				var val = Adaptor.CreateObjectValueAsync ("Local Variables", ObjectValueFlags.EvaluatingGroup, delegate {
					frame = GetFrameInfo (frameIndex, options, true);
					foreach (var local in frame.LocalVariables) {
						using (var timer = StartEvaluationTimer (local.Name)) {
							var localValue = local.CreateObjectValue (false, options);
							timer.Stop (localValue);
							list.Add (localValue);
						}
					}

					return ObjectValue.CreateArray (null, new ObjectPath ("Local Variables"), "", list.Count, ObjectValueFlags.EvaluatingGroup, list.ToArray ());
				});

				return new [] { val };
			}
			
			foreach (var local in frame.LocalVariables)
				list.Add (local.CreateObjectValue (true, options));

			return list.ToArray ();
		}
		
		public virtual ObjectValue[] GetParameters (int frameIndex, EvaluationOptions options)
		{
			var frame = GetFrameInfo (frameIndex, options, false);
			var values = new List<ObjectValue> ();

			if (frame == null) {
				var value = Adaptor.CreateObjectValueAsync ("Parameters", ObjectValueFlags.EvaluatingGroup, delegate {
					frame = GetFrameInfo (frameIndex, options, true);
					foreach (var param in frame.Parameters) {
						using (var timer = StartEvaluationTimer (param.Name)) {
							var paramValue = param.CreateObjectValue (false, options);
							timer.Stop (paramValue);
							values.Add (paramValue);
						}
					}

					return ObjectValue.CreateArray (null, new ObjectPath ("Parameters"), "", values.Count, ObjectValueFlags.EvaluatingGroup, values.ToArray ());
				});

				return new [] { value };
			}
			
			foreach (var param in frame.Parameters)
				values.Add (param.CreateObjectValue (true, options));

			return values.ToArray ();
		}
		
		public virtual ObjectValue GetThisReference (int frameIndex, EvaluationOptions options)
		{
			var frame = GetFrameInfo (frameIndex, options, false);

			if (frame == null) {
				return Adaptor.CreateObjectValueAsync ("this", ObjectValueFlags.EvaluatingGroup, delegate {
					frame = GetFrameInfo (frameIndex, options, true);
					ObjectValue[] values;

					if (frame.This != null) {
						using (var timer = StartEvaluationTimer ("this")) {
							var thisValue = frame.This.CreateObjectValue (false, options);
							timer.Stop (thisValue);
							values = new [] { thisValue };
						}
					} else
						values = new ObjectValue [0];

					return ObjectValue.CreateArray (null, new ObjectPath ("this"), "", values.Length, ObjectValueFlags.EvaluatingGroup, values);
				});
			}

			return frame.This != null ? frame.This.CreateObjectValue (true, options) : null;
		}
		
		public virtual ExceptionInfo GetException (int frameIndex, EvaluationOptions options)
		{
			var frame = GetFrameInfo (frameIndex, options, false);
			ObjectValue value;

			if (frame == null) {
				value = Adaptor.CreateObjectValueAsync (options.CurrentExceptionTag, ObjectValueFlags.EvaluatingGroup, delegate {
					frame = GetFrameInfo (frameIndex, options, true);
					ObjectValue[] values;

					if (frame.Exception != null) {
						using (var timer = StartEvaluationTimer ("Exception")) {
							var exceptionValue = frame.Exception.CreateObjectValue (false, options);
							timer.Stop (exceptionValue);
							values = new [] { exceptionValue };
						}
					} else
						values = new ObjectValue [0];

					return ObjectValue.CreateArray (null, new ObjectPath (options.CurrentExceptionTag), "", values.Length, ObjectValueFlags.EvaluatingGroup, values);
				});
			} else if (frame.Exception != null) {
				value = frame.Exception.CreateObjectValue (true, options);
			} else {
				return null;
			}

			return new ExceptionInfo (value);
		}
		
		public virtual ObjectValue GetExceptionInstance (int frameIndex, EvaluationOptions options)
		{
			var frame = GetFrameInfo (frameIndex, options, false);

			if (frame == null) {
				return Adaptor.CreateObjectValueAsync (options.CurrentExceptionTag, ObjectValueFlags.EvaluatingGroup, delegate {
					frame = GetFrameInfo (frameIndex, options, true);
					ObjectValue[] values;

					if (frame.Exception != null) {
						using (var timer = StartEvaluationTimer ("Exception")) {
							var exceptionValue = frame.Exception.Exception.CreateObjectValue (false, options);
							timer.Stop (exceptionValue);
							values = new [] { exceptionValue };
						}
					} else
						values = new ObjectValue [0];

					return ObjectValue.CreateArray (null, new ObjectPath (options.CurrentExceptionTag), "", values.Length, ObjectValueFlags.EvaluatingGroup, values);
				});
			}

			return frame.Exception != null ? frame.Exception.Exception.CreateObjectValue (true, options) : null;
		}
		
		public virtual ObjectValue[] GetAllLocals (int frameIndex, EvaluationOptions options)
		{
			var locals = new List<ObjectValue> ();
			
			var excObj = GetExceptionInstance (frameIndex, options);
			if (excObj != null)
				locals.Insert (0, excObj);
			
			locals.AddRange (GetLocalVariables (frameIndex, options));
			locals.AddRange (GetParameters (frameIndex, options));
			
			locals.Sort ((v1, v2) => StringComparer.InvariantCulture.Compare (v1.Name, v2.Name));

			var thisObj = GetThisReference (frameIndex, options);
			if (thisObj != null)
				locals.Insert (0, thisObj);
			
			return locals.ToArray ();
		}
		
		public virtual ObjectValue[] GetExpressionValues (int frameIndex, string[] expressions, EvaluationOptions options)
		{
			if (Adaptor.IsEvaluating) {
				var values = new List<ObjectValue> ();

				foreach (var expression in expressions) {
					string tmpExp = expression;

					var value = Adaptor.CreateObjectValueAsync (tmpExp, ObjectValueFlags.Field, delegate {
						var cctx = GetEvaluationContext (frameIndex, options);
						return Adaptor.GetExpressionValue (cctx, tmpExp);
					});
					value.Name = expression;

					values.Add (value);
				}

				return values.ToArray ();
			}

			var ctx = GetEvaluationContext (frameIndex, options);

			return ctx.Adapter.GetExpressionValuesAsync (ctx, expressions);
		}
		
		public virtual CompletionData GetExpressionCompletionData (int frameIndex, string exp)
		{
			var ctx = GetEvaluationContext (frameIndex, EvaluationOptions.DefaultOptions);
			return ctx.Adapter.GetExpressionCompletionData (ctx, exp);
		}
		
		public virtual AssemblyLine[] Disassemble (int frameIndex, int firstLine, int count)
		{
			throw new NotImplementedException ();
		}
		
		public virtual ValidationResult ValidateExpression (int frameIndex, string expression, EvaluationOptions options)
		{
			var ctx = GetEvaluationContext (frameIndex, options);
			return Adaptor.ValidateExpression (ctx, expression);
		}
		
		FrameInfo GetFrameInfo (int frameIndex, EvaluationOptions options, bool ignoreEvalStatus)
		{
			FrameInfo finfo;

			lock (frameInfo) {
				if (frameInfo.TryGetValue (frameIndex, out finfo))
					return finfo;

				if (!ignoreEvalStatus && Adaptor.IsEvaluating)
					return null;

				var ctx = GetEvaluationContext (frameIndex, options);
				if (ctx == null)
					return null;

				finfo = new FrameInfo ();
				finfo.Context = ctx;
				//Don't try to optimize lines below with lazy loading, you won't gain anything(in communication with runtime)
				finfo.LocalVariables.AddRange (ctx.Evaluator.GetLocalVariables (ctx));
				finfo.Parameters.AddRange (ctx.Evaluator.GetParameters (ctx));
				finfo.This = ctx.Evaluator.GetThisReference (ctx);

				var ex = ctx.Evaluator.GetCurrentException (ctx);
				if (ex != null)
					finfo.Exception = new ExceptionInfoSource (ctx, ex);

				frameInfo[frameIndex] = finfo;
			}

			return finfo;
		}

		DebuggerTimer StartEvaluationTimer (string name)
		{
			return new DebuggerTimer (Adaptor.DebuggerSession?.EvaluationStats, name);
		}
	}
	
	class FrameInfo
	{
		public EvaluationContext Context;
		public List<ValueReference> LocalVariables = new List<ValueReference> ();
		public List<ValueReference> Parameters = new List<ValueReference> ();
		public ValueReference This;
		public ExceptionInfoSource Exception;
	}
}
