// EvaluationContext.cs
//
// Author:
//   Lluis Sanchez Gual <lluis@novell.com>
//
// Copyright (c) 2008 Novell, Inc (http://www.novell.com)
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
//
//

using System;
using Mono.Debugging.Client;
using Mono.Debugging.Backend;

namespace Mono.Debugging.Evaluation
{
	public class EvaluationContext
	{
		private ExpressionEvaluator evaluator;

		public ExpressionEvaluator Evaluator { 
			get
            {
				if (evaluator == null)
					evaluator = new ExpressionEvaluator();
				return evaluator;
            }
			set
            {
				evaluator = value;
            }
		}
		public ObjectValueAdaptor Adapter { get; set; }
		
		public EvaluationOptions Options {
			get; set;
		}
		
		public bool CaseSensitive {
			get { return Evaluator.CaseSensitive; }
		}
		
		public virtual void WriteDebuggerError (Exception ex)
		{
		}

		public virtual void WriteDebuggerOutput (string message, params object[] values)
		{
		}

		public void WaitRuntimeInvokes ()
		{
		}
		
		public void AssertTargetInvokeAllowed ()
		{
			if (!Options.AllowTargetInvoke)
				throw new ImplicitEvaluationDisabledException ();
		}
		
		public EvaluationContext (EvaluationOptions options)
		{
			Options = options;
		}

		public EvaluationContext Clone ()
		{
			var clone = (EvaluationContext) MemberwiseClone ();
			clone.CopyFrom (this);
			return clone;
		}

		public EvaluationContext WithOptions (EvaluationOptions options)
		{
			if (options == null || Options == options)
				return this;

			var clone = Clone ();
			clone.Options = options;
			return clone;
		}

		public virtual void CopyFrom (EvaluationContext ctx)
		{
			Options = ctx.Options.Clone ();
			Evaluator = ctx.Evaluator;
			Adapter = ctx.Adapter;
		}
		
		ExpressionValueSource expressionValueSource;
		internal ExpressionValueSource ExpressionValueSource {
			get {
				if (expressionValueSource == null)
					expressionValueSource = new ExpressionValueSource (this);
				return expressionValueSource;
			}
		}

		public virtual bool SupportIEnumerable {
			get {
				return false;
			}
		}
	}
	
	class ExpressionValueSource: RemoteFrameObject, IObjectValueSource
	{
		readonly EvaluationContext ctx;
		
		public ExpressionValueSource (EvaluationContext ctx)
		{
			this.ctx = ctx;
			Connect ();
		}
		
		public ObjectValue[] GetChildren (ObjectPath path, int index, int count, EvaluationOptions options)
		{
			throw new NotImplementedException ();
		}
		
		public EvaluationResult SetValue (ObjectPath path, string value, EvaluationOptions options)
		{
			throw new NotImplementedException ();
		}
		
		public ObjectValue GetValue (ObjectPath path, EvaluationOptions options)
		{
			EvaluationContext c = ctx.WithOptions (options);
			var vals = c.Adapter.GetExpressionValuesAsync (c, new string[] { path.LastName });
			return vals[0];
		}
		
		public object GetRawValue (ObjectPath path, EvaluationOptions options)
		{
			throw new NotImplementedException ();
		}
		
		public void SetRawValue (ObjectPath path, object value, EvaluationOptions options)
		{
			throw new NotImplementedException ();
		}
	}
}
