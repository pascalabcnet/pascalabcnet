// ValueReference.cs
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
using System.Collections.Generic;
using Mono.Debugging.Client;
using Mono.Debugging.Backend;
using DC = Mono.Debugging.Client;

namespace Mono.Debugging.Evaluation
{
	public abstract class ValueReference: RemoteFrameObject, IObjectValueSource, IObjectSource
	{
		readonly EvaluationOptions originalOptions;

		protected ValueReference (EvaluationContext ctx)
		{
			originalOptions = ctx.Options;
			Context = ctx;
		}
		
		public virtual object ObjectValue {
			get {
				object ob = Value;
				if (Context.Adapter.IsNull (Context, ob))
					return null;

				if (Context.Adapter.IsPrimitive (Context, ob))
					return Context.Adapter.TargetObjectToObject (Context, ob);

				return ob;
			}
		}
		
		public abstract object Value { get; set; }
		public abstract string Name { get; }
		public abstract object Type { get; }
		public abstract ObjectValueFlags Flags { get; }
		
		// For class members, the type declaring the member (null otherwise)
		public virtual object DeclaringType {
			get { return null; }
		}

		public EvaluationContext Context {
			get; set;
		}

		public EvaluationContext GetContext (EvaluationOptions options)
		{
			return Context.WithOptions (options);
		}

		public ObjectValue CreateObjectValue (bool withTimeout)
		{
			return CreateObjectValue (withTimeout, Context.Options);
		}
		
		public ObjectValue CreateObjectValue (bool withTimeout, EvaluationOptions options)
		{
			if (!CanEvaluate (options))
				return DC.ObjectValue.CreateImplicitNotSupported (this, new ObjectPath (Name), Context.Adapter.GetDisplayTypeName (GetContext (options), Type), Flags);

			if (withTimeout) {
				return Context.Adapter.CreateObjectValueAsync (Name, Flags, delegate {
					return CreateObjectValue (options);
				});
			}

			return CreateObjectValue (options);
		}
		
		public ObjectValue CreateObjectValue (EvaluationOptions options)
		{
			if (!CanEvaluate (options)) {
				if (options.AllowTargetInvoke)//If it can't evaluate and target invoke is allowed, mark it as not supported.
					return DC.ObjectValue.CreateNotSupported (this, new ObjectPath (Name), Context.Adapter.GetDisplayTypeName (GetContext (options), Type), "Can not evaluate", Flags);
				return DC.ObjectValue.CreateImplicitNotSupported (this, new ObjectPath (Name), Context.Adapter.GetDisplayTypeName (GetContext (options), Type), Flags);
			}
			Connect ();
			try {
				return OnCreateObjectValue (options);
			} catch (ImplicitEvaluationDisabledException) {
				return DC.ObjectValue.CreateImplicitNotSupported (this, new ObjectPath (Name), Context.Adapter.GetDisplayTypeName (GetContext (options), Type), Flags);
			} catch (NotSupportedExpressionException ex) {
				return DC.ObjectValue.CreateNotSupported (this, new ObjectPath (Name), Context.Adapter.GetDisplayTypeName (GetContext (options), Type), ex.Message, Flags);
			} catch (EvaluatorException ex) {
				return DC.ObjectValue.CreateError (this, new ObjectPath (Name), Context.Adapter.GetDisplayTypeName (GetContext (options), Type), ex.Message, Flags);
			} catch (Exception ex) {
				Context.WriteDebuggerError (ex);
#if (DEBUG)
				Console.WriteLine(ex.Message + " " + ex.StackTrace);
#endif
				return DC.ObjectValue.CreateUnknown (Name);
			}
		}
		
		protected virtual bool CanEvaluate (EvaluationOptions options)
		{
			return true;
		}
		
		protected virtual ObjectValue OnCreateObjectValue (EvaluationOptions options)
		{
			string name = Name;
			if (string.IsNullOrEmpty (name))
				name = "?";
			
			var ctx = GetContext (options);
			object val = null;

			// Note: The Value property implementation may make use of the EvaluationOptions,
			// so we need to override our context temporarily to do the evaluation.
			val = GetValue (ctx);

			if (val != null && !ctx.Adapter.IsNull (ctx, val))
				return ctx.Adapter.CreateObjectValue (ctx, this, new ObjectPath (name), val, Flags);

			return DC.ObjectValue.CreateNullObject (this, name, ctx.Adapter.GetDisplayTypeName (ctx.Adapter.GetTypeName (ctx, Type)), Flags);
		}

		ObjectValue IObjectValueSource.GetValue (ObjectPath path, EvaluationOptions options)
		{
			return CreateObjectValue (true, options);
		}
		
		EvaluationResult IObjectValueSource.SetValue (ObjectPath path, string value, EvaluationOptions options)
		{
			try {
				Context.WaitRuntimeInvokes ();

				var ctx = GetContext (options);
				ctx.Options.AllowMethodEvaluation = true;
				ctx.Options.AllowTargetInvoke = true;

				var vref = ctx.Evaluator.Evaluate (ctx, value, Type);
				var newValue = ctx.Adapter.Convert (ctx, vref.Value, Type);
				SetValue (ctx, newValue);
			} catch (Exception ex) {
				Context.WriteDebuggerError (ex);
				Context.WriteDebuggerOutput ("Value assignment failed: {0}: {1}\n", ex.GetType (), ex.Message);
			}
			
			try {
				return Context.Evaluator.TargetObjectToExpression (Context, Value);
			} catch (Exception ex) {
				Context.WriteDebuggerError (ex);
				Context.WriteDebuggerOutput ("Value assignment failed: {0}: {1}\n", ex.GetType (), ex.Message);
			}
			
			return null;
		}
		
		object IObjectValueSource.GetRawValue (ObjectPath path, EvaluationOptions options)
		{
			var ctx = GetContext (options);

			return ctx.Adapter.ToRawValue (ctx, this, GetValue (ctx));
		}

		void IObjectValueSource.SetRawValue (ObjectPath path, object value, EvaluationOptions options)
		{
			SetRawValue (path, value, options);
		}

		public object GetRawValue (EvaluationOptions options)
		{
			var ctx = GetContext (options);
			return ctx.Adapter.ToRawValue (ctx, this, GetValue (ctx));
		}

		public void SetRawValue (object value, EvaluationOptions options)
		{
			SetRawValue (new ObjectPath (), value, options);
		}

		protected virtual void SetRawValue (ObjectPath path, object value, EvaluationOptions options)
		{
			var ctx = GetContext (options);

			SetValue (ctx, Context.Adapter.FromRawValue (ctx, value));
		}

		ObjectValue[] IObjectValueSource.GetChildren (ObjectPath path, int index, int count, EvaluationOptions options)
		{
			return GetChildren (path, index, count, options);
		}

		public virtual string CallToString ()
		{
			return Context.Adapter.CallToString (Context, Value);
		}

		public virtual object GetValue (EvaluationContext ctx)
		{
			return Value;
		}

		public virtual void SetValue (EvaluationContext ctx, object value)
		{
			Value = value;
		}

		[Obsolete ("Use GetValue(EvaluationContext) instead.")]
		protected virtual object GetValueExplicitly ()
		{
			var options = Context.Options.Clone ();
			options.AllowTargetInvoke = true;
			var ctx = GetContext (options);

			return GetValue (ctx);
		}

		public virtual ObjectValue[] GetChildren (ObjectPath path, int index, int count, EvaluationOptions options)
		{
			try {
				var ctx = GetChildrenContext (options);

				return ctx.Adapter.GetObjectValueChildren (ctx, this, GetValue (ctx), index, count);
			} catch (Exception ex) {
				return new [] { DC.ObjectValue.CreateFatalError ("", ex.Message, ObjectValueFlags.ReadOnly) };
			}
		}

		public virtual IEnumerable<ValueReference> GetChildReferences (EvaluationOptions options)
		{
			try {
				object val = Value;
				if (Context.Adapter.IsClassInstance (Context, val))
					return Context.Adapter.GetMembersSorted (GetChildrenContext (options), this, Type, val);
			} catch {
				// Ignore
			}

			return new ValueReference [0];
		}
		
		public IObjectSource ParentSource { get; set; }

		protected EvaluationContext GetChildrenContext (EvaluationOptions options)
		{
			var ctx = Context.Clone ();

			if (options != null)
				ctx.Options = options;

			ctx.Options.EvaluationTimeout = originalOptions.MemberEvaluationTimeout;

			return ctx;
		}

		public virtual ValueReference GetChild (ObjectPath vpath, EvaluationOptions options)
		{
			if (vpath.Length == 0)
				return this;

			var val = GetChild (vpath[0], options);

			return val != null ? val.GetChild (vpath.GetSubpath (1), options) : null;
		}

		public virtual ValueReference GetChild (string name, EvaluationOptions options)
		{
			object obj = Value;
			
			if (obj == null)
				return null;

			if (name[0] == '[' && Context.Adapter.IsArray (Context, obj)) {
				// Parse the array indices
				var tokens = name.Substring (1, name.Length - 2).Split (',');
				var indices = new int [tokens.Length];

				for (int n = 0; n < tokens.Length; n++)
					indices[n] = int.Parse (tokens[n]);

				return new ArrayValueReference (Context, obj, indices);
			}
			
			if (Context.Adapter.IsClassInstance (Context, obj))
				return Context.Adapter.GetMember (GetChildrenContext (options), this, Type, obj, name);

			return null;
		}
	}
}
