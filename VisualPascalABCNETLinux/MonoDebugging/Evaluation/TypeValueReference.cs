// TypeValueReference.cs
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
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;

using Mono.Debugging.Client;

namespace Mono.Debugging.Evaluation
{
	public class TypeValueReference: ValueReference
	{
		readonly string fullName;
		readonly string name;
		readonly object type;

		public TypeValueReference (EvaluationContext ctx, object type) : base (ctx)
		{
			this.type = type;
			fullName = ctx.Adapter.GetDisplayTypeName (ctx, type);
			name = GetTypeName (fullName);
		}
		
		internal static string GetTypeName (string tname)
		{
			tname = tname.Replace ('+', '.');

			int sep1 = tname.IndexOf ('<');
			int sep2 = tname.IndexOf ('[');

			if (sep2 != -1 && (sep2 < sep1 || sep1 == -1))
				sep1 = sep2;

			if (sep1 == -1)
				sep1 = tname.Length - 1;

			int dot = tname.LastIndexOf ('.', sep1);

			return dot != -1 ? tname.Substring (dot + 1) : tname;
		}
		
		public override object Value {
			get {
				throw new NotSupportedException ();
			}
			set {
				throw new NotSupportedException();
			}
		}

		public override object Type {
			get {
				return type;
			}
		}
		
		public override object ObjectValue {
			get {
				throw new NotSupportedException ();
			}
		}

		public override string Name {
			get {
				return name;
			}
		}
		
		public override ObjectValueFlags Flags {
			get {
				return ObjectValueFlags.Type;
			}
		}

		protected override ObjectValue OnCreateObjectValue (EvaluationOptions options)
		{
			return Mono.Debugging.Client.ObjectValue.CreateObject (this, new ObjectPath (Name), "<type>", fullName, Flags, null);
		}

		public override ValueReference GetChild (string name, EvaluationOptions options)
		{
			var ctx = GetContext (options);

			foreach (var val in ctx.Adapter.GetMembers (ctx, this, type, null)) {
				if (val.Name == name)
					return val;
			}

			foreach (var nestedType in ctx.Adapter.GetNestedTypes (ctx, type)) {
				string typeName = ctx.Adapter.GetTypeName (ctx, nestedType);

				if (GetTypeName (typeName) == name)
					return new TypeValueReference (ctx, nestedType);
			}

			return null;
		}

		public override ObjectValue[] GetChildren (ObjectPath path, int index, int count, EvaluationOptions options)
		{
			var ctx = GetContext (options);

			try {
				BindingFlags flattenFlag = options.FlattenHierarchy ? (BindingFlags)0 : BindingFlags.DeclaredOnly;
				BindingFlags flags = BindingFlags.Static | BindingFlags.Public | flattenFlag;
				bool groupPrivateMembers = options.GroupPrivateMembers || ctx.Adapter.IsExternalType (ctx, type);
				var list = new List<ObjectValue> ();

				if (!groupPrivateMembers)
					flags |= BindingFlags.NonPublic;
				
				var tdata = ctx.Adapter.GetTypeDisplayData (ctx, type);
				var tdataType = type;

				foreach (var val in ctx.Adapter.GetMembersSorted (ctx, this, type, null, flags)) {
					var decType = val.DeclaringType;
					if (decType != null && decType != tdataType) {
						tdataType = decType;
						tdata = ctx.Adapter.GetTypeDisplayData (ctx, decType);
					}

					var state = tdata.GetMemberBrowsableState (val.Name);
					if (state == DebuggerBrowsableState.Never)
						continue;

					var oval = val.CreateObjectValue (options);
					list.Add (oval);
				}
				
				var nestedTypes = new List<ObjectValue> ();
				foreach (var nestedType in ctx.Adapter.GetNestedTypes (ctx, type))
					nestedTypes.Add (new TypeValueReference (ctx, nestedType).CreateObjectValue (options));
				
				nestedTypes.Sort ((v1, v2) => string.Compare (v1.Name, v2.Name, StringComparison.CurrentCulture));
				
				list.AddRange (nestedTypes);
				
				if (groupPrivateMembers)
					list.Add (FilteredMembersSource.CreateNonPublicsNode (ctx, this, type, null, BindingFlags.NonPublic | BindingFlags.Static | flattenFlag));
				
				if (!options.FlattenHierarchy) {
					object baseType = ctx.Adapter.GetBaseType (ctx, type, false);
					if (baseType != null) {
						var baseRef = new TypeValueReference (ctx, baseType);
						var baseVal = baseRef.CreateObjectValue (false);
						baseVal.Name = "base";
						list.Insert (0, baseVal);
					}
				}
				
				return list.ToArray ();
			} catch (Exception ex) {
				ctx.WriteDebuggerOutput (ex.Message);
				return new ObjectValue [0];
			}
		}

		public override IEnumerable<ValueReference> GetChildReferences (EvaluationOptions options)
		{
			var ctx = GetContext (options);

			try {
				var list = new List<ValueReference> ();

				list.AddRange (ctx.Adapter.GetMembersSorted (ctx, this, type, null, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static));
				
				var nestedTypes = new List<ValueReference> ();
				foreach (var nestedType in ctx.Adapter.GetNestedTypes (ctx, type))
					nestedTypes.Add (new TypeValueReference (ctx, nestedType));
				
				nestedTypes.Sort ((v1, v2) => string.Compare (v1.Name, v2.Name, StringComparison.CurrentCulture));
				list.AddRange (nestedTypes);

				return list;
			} catch (Exception ex) {
				ctx.WriteDebuggerOutput (ex.Message);
				return new ValueReference[0];
			}
		}
	}
}
