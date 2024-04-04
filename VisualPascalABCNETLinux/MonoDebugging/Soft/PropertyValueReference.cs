// 
// PropertyValueReference.cs
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

using Mono.Debugging.Evaluation;
using Mono.Debugger.Soft;
using Mono.Debugging.Client;
using System;
using System.Collections.Generic;

namespace Mono.Debugging.Soft
{
	public class PropertyValueReference: SoftValueReference
	{
		PropertyInfoMirror property;
		TypeMirror declaringType;
		ObjectValueFlags flags;
		MethodMirror getter;
		Value[] indexerArgs;
		object obj, value;
		bool haveValue;
		bool safe;
		
		public PropertyValueReference (EvaluationContext ctx, PropertyInfoMirror property, object obj, TypeMirror declaringType, MethodMirror getter, Value[] indexerArgs): base (ctx)
		{
			this.safe = Context.Adapter.IsSafeToInvokeMethod (ctx, getter, obj ?? declaringType);
			this.declaringType = declaringType;
			this.indexerArgs = indexerArgs;
			this.property = property;
			this.getter = getter;
			this.obj = obj;

			if (getter.IsStatic)
				this.obj = null;
			
			var objectMirror = obj as ObjectMirror;
			if (objectMirror != null)
				EnsureContextHasDomain (objectMirror.Domain);

			flags = GetFlags (property, getter);
		}

		internal static ObjectValueFlags GetFlags (PropertyInfoMirror property, MethodMirror getter)
		{
			var flags = ObjectValueFlags.Property;

			if (property.GetSetMethod (true) == null)
				flags |= ObjectValueFlags.ReadOnly;

			if (getter.IsStatic)
				flags |= ObjectValueFlags.Global;

			if (getter.IsPublic)
				flags |= ObjectValueFlags.Public;
			else if (getter.IsPrivate)
				flags |= ObjectValueFlags.Private;
			else if (getter.IsFamily)
				flags |= ObjectValueFlags.Protected;
			else if (getter.IsFamilyAndAssembly)
				flags |= ObjectValueFlags.Internal;
			else if (getter.IsFamilyOrAssembly)
				flags |= ObjectValueFlags.InternalProtected;

			if (property.DeclaringType.IsValueType)
				flags |= ObjectValueFlags.ReadOnly; // Setting property values on structs is not supported by sdb

			return flags;
		}
		
		public override ObjectValueFlags Flags {
			get {
				return flags;
			}
		}

		public override string Name {
			get {
				return property.Name;
			}
		}

		public override object Type {
			get {
				return property.PropertyType;
			}
		}
		
		public override object DeclaringType {
			get {
				return property.DeclaringType;
			}
		}

		public override object Value {
			get {
				return GetValue (Context);
			}
			set {
				SetValue (Context, value);
			}
		}

		public override object GetValue (EvaluationContext ctx)
		{
			if (!haveValue) {
				if (!safe)
					throw new EvaluatorException ("This property is not safe to evaluate.");

				value = ((SoftEvaluationContext) ctx).RuntimeInvoke (getter, obj ?? declaringType, indexerArgs);
				haveValue = true;
			}

			return value;
		}

		public override void SetValue (EvaluationContext ctx, object value)
		{
			ctx.AssertTargetInvokeAllowed ();

			var args = new Value [indexerArgs != null ? indexerArgs.Length + 1 : 1];
			if (indexerArgs != null)
				indexerArgs.CopyTo (args, 0);

			args [args.Length - 1] = (Value) value;

			var setter = property.GetSetMethod (true);
			if (setter == null)
				throw new EvaluatorException ("Property is read-only");

			this.value = null;
			haveValue = false;

			((SoftEvaluationContext) ctx).RuntimeInvoke (setter, obj ?? declaringType, args);

			this.value = value;
			haveValue = true;
		}

		protected override bool CanEvaluate (EvaluationOptions options)
		{
			if (options.AllowTargetInvoke)
				return safe;

			if (!safe)
				return false;

			try {
				GetValue (Context.WithOptions (options));
				return true;
			} catch (ImplicitEvaluationDisabledException) {
				return false;
			}
		}

		internal string [] GetTupleElementNames ()
		{
			return FieldValueReference.GetTupleElementNames (property.GetCustomAttributes (true));
		}
	}
}
