//
// IEnumerableSource.cs
//
// Author:
//       David Karlaš <david.karlas@xamarin.com>
//
// Copyright (c) 2014 Xamarin, Inc (http://www.xamarin.com)
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
using System.Linq;
using Mono.Debugging.Backend;
using Mono.Debugging.Client;
using System.Collections.Generic;

namespace Mono.Debugging.Evaluation
{
	class EnumerableSource : IObjectValueSource
	{
		object obj;
		object objType;
		EvaluationContext ctx;
		List<ObjectValue> elements;
		List<object> values;
		int currentIndex = 0;
		object enumerator;
		object enumeratorType;

		public EnumerableSource (object source, object type, EvaluationContext ctx)
		{
			this.obj = source;
			this.objType = type;
			this.ctx = ctx;
		}

		bool MoveNext ()
		{
			return (bool)ctx.Adapter.TargetObjectToObject (ctx, ctx.Adapter.RuntimeInvoke (ctx, enumeratorType, enumerator, "MoveNext", new object[0], new object[0]));
		}

		void Fetch (int maxIndex)
		{
			if (elements == null) {
				elements = new List<ObjectValue> ();
				values = new List<object> ();
				enumerator = ctx.Adapter.RuntimeInvoke (ctx, objType, obj, "GetEnumerator", new object[0], new object[0]);
				enumeratorType = ctx.Adapter.GetImplementedInterfaces (ctx, ctx.Adapter.GetValueType (ctx, enumerator)).First (f => ctx.Adapter.GetTypeName (ctx, f) == "System.Collections.IEnumerator");
			}
			while (maxIndex > elements.Count && MoveNext ()) {
				var valCurrent = ctx.Adapter.GetMember (ctx, null, enumeratorType, enumerator, "Current");
				var val = valCurrent.Value;
				values.Add (val);
				if (val != null) {
					elements.Add (ctx.Adapter.CreateObjectValue (ctx, valCurrent, new ObjectPath ("[" + currentIndex + "]"), val, ObjectValueFlags.ReadOnly));
				} else {
					elements.Add (Mono.Debugging.Client.ObjectValue.CreateNullObject (this, "[" + currentIndex + "]", ctx.Adapter.GetDisplayTypeName (ctx.Adapter.GetTypeName (ctx, valCurrent.Type)), ObjectValueFlags.ReadOnly));
				}
				currentIndex++;
			}
		}


		public object GetElement (int idx)
		{
			return values [idx];
		}

		public ObjectValue[] GetChildren (ObjectPath path, int index, int count, EvaluationOptions options)
		{
			int idx;
			if (int.TryParse (path.LastName.Replace ("[", "").Replace ("]", ""), out idx)) {
				return ctx.Adapter.GetObjectValueChildren (ctx, null, values [idx], -1, -1);
			}
			if (index < 0)
				index = 0;
			if (count == 0)
				return new ObjectValue[0];
			if (count == -1)
				count = int.MaxValue;
			Fetch (index + count);
			if (count < 0 || index + count > elements.Count) {
				return  elements.Skip (index).ToArray ();
			} else {
				if (index < elements.Count) {
					return  elements.Skip (index).Take (System.Math.Min (count, elements.Count - index)).ToArray ();
				} else {
					return new ObjectValue[0];
				}
			}
		}

		public EvaluationResult SetValue (ObjectPath path, string value, EvaluationOptions options)
		{
			throw new InvalidOperationException ("Elements of IEnumerable can not be set");
		}

		public ObjectValue GetValue (ObjectPath path, EvaluationOptions options)
		{
			int idx;
			if (int.TryParse (path.LastName.Replace ("[", "").Replace ("]", ""), out idx)) {
				var element = elements [idx];
				element.Refresh (options);
				return element;
			}
			return null;
		}

		public object GetRawValue (ObjectPath path, EvaluationOptions options)
		{
			int idx = int.Parse (path.LastName.Replace ("[", "").Replace ("]", ""));
			EvaluationContext cctx = ctx.WithOptions (options);
			return cctx.Adapter.ToRawValue (cctx, new EnumerableObjectSource (this, idx), GetElement (idx));
		}

		public void SetRawValue (ObjectPath path, object value, EvaluationOptions options)
		{
			throw new InvalidOperationException ("Elements of IEnumerable can not be set");
		}
	}

	class EnumerableObjectSource : IObjectSource
	{
		EnumerableSource enumerableSource;
		int idx;

		public EnumerableObjectSource (EnumerableSource enumerableSource, int idx)
		{
			this.enumerableSource = enumerableSource;
			this.idx = idx;

		}

		#region IObjectSource implementation

		public object Value {
			get {
				return enumerableSource.GetElement (idx);
			}
			set {
				throw new InvalidOperationException ("Elements of IEnumerable can not be set");
			}
		}

		#endregion
	}
}

