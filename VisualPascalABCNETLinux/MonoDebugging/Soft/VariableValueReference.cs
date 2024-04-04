// 
// VariableValueReference.cs
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
using Mono.Debugging.Client;
using Mono.Debugger.Soft;

namespace Mono.Debugging.Soft
{
	public class VariableValueReference : ValueReference
	{
		readonly LocalVariableBatch batch;
		readonly LocalVariable variable;
		readonly string name;
		Value value;
		int version;

		public VariableValueReference (EvaluationContext ctx, string name, LocalVariable variable, LocalVariableBatch batch) : base (ctx)
		{
			this.variable = variable;
			this.batch = batch;
			this.name = name;
		}

		public VariableValueReference (EvaluationContext ctx, string name, LocalVariable variable, Value value) : base (ctx)
		{
			version = ((SoftEvaluationContext)ctx).Session.StackVersion;
			this.variable = variable;
			this.value = value;
			this.name = name;
		}
		
		public VariableValueReference (EvaluationContext ctx, string name, LocalVariable variable) : base (ctx)
		{
			this.variable = variable;
			this.name = name;
		}
		
		public override ObjectValueFlags Flags {
			get {
				return ObjectValueFlags.Variable;
			}
		}

		public override string Name {
			get {
				return name;
			}
		}

		public override object Type {
			get {
				return variable.Type;
			}
		}

		Value NormalizeValue (EvaluationContext ctx, Value value)
		{
			if (variable.Type.IsPointer) {
				long addr = (long) ((PrimitiveValue) value).Value;

				return new PointerValue (value.VirtualMachine, variable.Type, addr);
			}

			return ctx.Adapter.IsNull (ctx, value) ? null : value;
		}

		object GetValue (SoftEvaluationContext ctx)
		{
			try {
				if (value == null || version != ctx.Session.StackVersion) {
					value = batch != null ? batch.GetValue (ctx, variable) : ctx.Frame.GetValue (variable);
					version = ctx.Session.StackVersion;
				}

				return NormalizeValue (ctx, value);
			} catch (AbsentInformationException ex) {
				throw new EvaluatorException (ex, "Value not available");
			} catch (Exception ex) {
				throw new EvaluatorException (ex.Message);
			}
		}

		void SetValue (SoftEvaluationContext ctx, object value)
		{
			if (batch != null) {
				batch.SetValue (ctx, variable, (Value) value);
			} else {
				ctx.Frame.SetValue (variable, (Value) value);
				ctx.Session.StackVersion++;
			}
			version = ctx.Session.StackVersion;
			this.value = (Value) value;
		}

		public override object GetValue (EvaluationContext ctx)
		{
			return GetValue ((SoftEvaluationContext) ctx);
		}

		public override void SetValue (EvaluationContext ctx, object value)
		{
			SetValue ((SoftEvaluationContext) ctx, value);
		}

		public override object Value {
			get {
				return GetValue ((SoftEvaluationContext) Context);
			}
			set {
				SetValue ((SoftEvaluationContext) Context, value);
			}
		}

		internal string [] GetTupleElementNames (SoftEvaluationContext ctx)
		{
			return ctx.Session.GetPdbData (variable.Method)?.GetTupleElementNames (variable.Method, variable.Index);
		}
	}
}
