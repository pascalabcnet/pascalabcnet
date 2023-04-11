// 
// ObjectValueAdaptor.cs
//  
// Authors: Lluis Sanchez Gual <lluis@novell.com>
//          Jeffrey Stedfast <jeff@xamarin.com>
// 
// Copyright (c) 2008 Novell, Inc (http://www.novell.com)
// Copyright (c) 2012 Xamarin Inc. (http://www.xamarin.com)
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
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;

using Mono.Debugging.Client;
using Mono.Debugging.Backend;

namespace Mono.Debugging.Evaluation
{
	public abstract class ObjectValueAdaptor: IDisposable
	{
		readonly Dictionary<string, TypeDisplayData> typeDisplayData = new Dictionary<string, TypeDisplayData> ();

		// Time to wait while evaluating before switching to async mode
		public int DefaultEvaluationWaitTime { get; set; }
		
		public event EventHandler<BusyStateEventArgs> BusyStateChanged;

		public DebuggerSession DebuggerSession {
			get { return asyncEvaluationTracker.Session; }
			set { asyncEvaluationTracker.Session = value; }
		}

		static readonly Dictionary<string, string> CSharpTypeNames = new Dictionary<string, string> ();
		readonly AsyncEvaluationTracker asyncEvaluationTracker = new AsyncEvaluationTracker ();
		readonly AsyncOperationManager asyncOperationManager = new AsyncOperationManager ();

		static ObjectValueAdaptor ()
		{
			CSharpTypeNames["System.Void"]    = "void";
			CSharpTypeNames["System.Object"]  = "object";
			CSharpTypeNames["System.Boolean"] = "bool";
			CSharpTypeNames["System.Byte"]    = "byte";
			CSharpTypeNames["System.SByte"]   = "sbyte";
			CSharpTypeNames["System.Char"]    = "char";
			CSharpTypeNames["System.Enum"]    = "enum";
			CSharpTypeNames["System.Int16"]   = "short";
			CSharpTypeNames["System.Int32"]   = "int";
			CSharpTypeNames["System.Int64"]   = "long";
			CSharpTypeNames["System.UInt16"]  = "ushort";
			CSharpTypeNames["System.UInt32"]  = "uint";
			CSharpTypeNames["System.UInt64"]  = "ulong";
			CSharpTypeNames["System.Single"]  = "float";
			CSharpTypeNames["System.Double"]  = "double";
			CSharpTypeNames["System.Decimal"] = "decimal";
			CSharpTypeNames["System.String"]  = "string";
		}
		
		protected ObjectValueAdaptor ()
		{
			DefaultEvaluationWaitTime = 100;
			
			asyncOperationManager.BusyStateChanged += (sender, e) => OnBusyStateChanged (e);
			asyncEvaluationTracker.WaitTime = DefaultEvaluationWaitTime;
		}
		
		public void Dispose ()
		{
			asyncEvaluationTracker.Dispose ();
			asyncOperationManager.Dispose ();
		}

		public ObjectValue CreateObjectValue (EvaluationContext ctx, IObjectValueSource source, ObjectPath path, object obj, ObjectValueFlags flags)
		{
			try {
				return CreateObjectValueImpl (ctx, source, path, obj, flags);
			} catch (EvaluatorAbortedException ex) {
				return ObjectValue.CreateFatalError (path.LastName, ex.Message, flags);
			} catch (EvaluatorException ex) {
				return ObjectValue.CreateFatalError (path.LastName, ex.Message, flags);
			} catch (Exception ex) {
				ctx.WriteDebuggerError (ex);
				return ObjectValue.CreateFatalError (path.LastName, ex.Message, flags);
			}
		}
		
		public virtual string GetDisplayTypeName (string typeName)
		{
			return GetDisplayTypeName (typeName.Replace ('+', '.'), 0, typeName.Length);
		}
		
		public string GetDisplayTypeName (EvaluationContext ctx, object type)
		{
			return GetDisplayTypeName (GetTypeName (ctx, type));
		}
		
		string GetDisplayTypeName (string typeName, int startIndex, int endIndex)
		{
			// Note: '[' denotes the start of an array
			//       '`' denotes a generic type
			//       ',' denotes the start of the assembly name
			int tokenIndex = typeName.IndexOfAny (new [] { '[', '`', ',' }, startIndex, endIndex - startIndex);
			List<string> genericArgs = null;
			string array = string.Empty;
			int genericEndIndex = -1;
			int typeEndIndex;
			
		retry:
			if (tokenIndex == -1) // Simple type
				return GetShortTypeName (typeName.Substring (startIndex, endIndex - startIndex));
			
			if (typeName[tokenIndex] == ',') // Simple type with an assembly name
				return GetShortTypeName (typeName.Substring (startIndex, tokenIndex - startIndex));
			
			// save the index of the end of the type name
			typeEndIndex = tokenIndex;
			
			// decode generic args first, if this is a generic type
			if (typeName[tokenIndex] == '`') {
				genericEndIndex = typeName.IndexOf ('[', tokenIndex, endIndex - tokenIndex);
				if (genericEndIndex == -1) {
					// Mono's compiler seems to generate non-generic types with '`'s in the name
					// e.g. __EventHandler`1_FileCopyEventArgs_DelegateFactory_2
					tokenIndex = typeName.IndexOfAny (new [] { '[', ',' }, tokenIndex, endIndex - tokenIndex);
					goto retry;
				}
				
				tokenIndex = genericEndIndex;
				genericArgs = GetGenericArguments (typeName, ref tokenIndex, endIndex);
			}
			
			// decode array rank info
			while (tokenIndex < endIndex && typeName[tokenIndex] == '[') {
				int arrayEndIndex = typeName.IndexOf (']', tokenIndex, endIndex - tokenIndex);
				if (arrayEndIndex == -1)
					break;
				arrayEndIndex++;
				array += typeName.Substring (tokenIndex, arrayEndIndex - tokenIndex);
				tokenIndex = arrayEndIndex;
			}
			
			string name = typeName.Substring (startIndex, typeEndIndex - startIndex);
			
			if (genericArgs == null)
				return GetShortTypeName (name) + array;
			
			// Use the prettier name for nullable types
			if (name == "System.Nullable" && genericArgs.Count == 1)
				return genericArgs[0] + "?" + array;
			
			// Insert the generic arguments next to each type.
			// for example: Foo`1+Bar`1[System.Int32,System.String]
			// is converted to: Foo<int>.Bar<string>
			var builder = new StringBuilder (name);
			int i = typeEndIndex + 1;
			int genericIndex = 0;
			int argCount, next;
			
			while (i < genericEndIndex) {
				// decode the argument count
				argCount = 0;
				while (i < genericEndIndex && char.IsDigit (typeName[i])) {
					argCount = (argCount * 10) + (typeName[i] - '0');
					i++;
				}
				
				// insert the argument types
				builder.Append ('<');
				while (argCount > 0 && genericIndex < genericArgs.Count) {
					builder.Append (genericArgs[genericIndex++]);
					if (--argCount > 0)
						builder.Append (',');
				}
				builder.Append ('>');
				
				// Find the end of the next generic type component
				if ((next = typeName.IndexOf ('`', i, genericEndIndex - i)) == -1)
					next = genericEndIndex;
				
				// Append the next generic type component
				builder.Append (typeName, i, next - i);
				
				i = next + 1;
			}
			
			return builder + array;
		}
		
		List<string> GetGenericArguments (string typeName, ref int i, int endIndex)
		{
			// Get a list of the generic arguments.
			// When returning, i points to the next char after the closing ']'
			var genericArgs = new List<string> ();

			i++;

			while (i < endIndex && typeName [i] != ']') {
				int pend = FindTypeEnd (typeName, i, endIndex);
				bool escaped = typeName [i] == '[';

				genericArgs.Add (GetDisplayTypeName (typeName, escaped ? i + 1 : i, escaped ? pend - 1 : pend));
				i = pend;

				if (i < endIndex && typeName[i] == ',')
					i++;
			}

			i++;

			return genericArgs;
		}
		
		static int FindTypeEnd (string typeName, int startIndex, int endIndex)
		{
			int i = startIndex;
			int brackets = 0;

			while (i < endIndex) {
				char c = typeName[i];

				if (c == '[') {
					brackets++;
				} else if (c == ']') {
					if (brackets <= 0)
						return i;

					brackets--;
				} else if (c == ',' && brackets == 0) {
					return i;
				}

				i++;
			}

			return i;
		}
		
		public static string GetCSharpTypeName (string typeName)
		{
			int star = typeName.IndexOf ('*');
			string name, ptr, csharp;

			if (star != -1) {
				name = typeName.Substring (0, star);
				ptr = typeName.Substring (star);
			} else {
				ptr = string.Empty;
				name = typeName;
			}

			if (CSharpTypeNames.TryGetValue (name, out csharp))
				return csharp + ptr;

			return typeName;
		}

		public virtual string GetShortTypeName (string typeName)
		{
			return GetCSharpTypeName (typeName);
		}

		public virtual void OnBusyStateChanged (BusyStateEventArgs e)
		{
			EventHandler<BusyStateEventArgs> evnt = BusyStateChanged;
			if (evnt != null)
				evnt (this, e);
		}

		public abstract ICollectionAdaptor CreateArrayAdaptor (EvaluationContext ctx, object arr);
		public abstract IStringAdaptor CreateStringAdaptor (EvaluationContext ctx, object str);

		public abstract bool IsNull (EvaluationContext ctx, object val);
		public abstract bool IsPrimitive (EvaluationContext ctx, object val);
		public abstract bool IsPointer (EvaluationContext ctx, object val);
		public abstract bool IsString (EvaluationContext ctx, object val);
		public abstract bool IsArray (EvaluationContext ctx, object val);
		public abstract bool IsEnum (EvaluationContext ctx, object val);
		public abstract bool IsValueType (object type);
		public virtual bool IsPrimitiveType (object type) { throw new NotImplementedException (); }
		public abstract bool IsClass (EvaluationContext ctx, object type);
		public abstract object TryCast (EvaluationContext ctx, object val, object type);

		public abstract object GetValueType (EvaluationContext ctx, object val);
		public abstract string GetTypeName (EvaluationContext ctx, object type);
		public abstract object[] GetTypeArgs (EvaluationContext ctx, object type);
		public abstract object GetBaseType (EvaluationContext ctx, object type);

		public virtual bool IsDelayedType (EvaluationContext ctx, object type)
		{
			return false;
		}

		public virtual bool IsGenericType (EvaluationContext ctx, object type)
		{
			return type != null && GetTypeName (ctx, type).IndexOf ('`') != -1;
		}

		public virtual IEnumerable<object> GetGenericTypeArguments (EvaluationContext ctx, object type)
		{
			yield break;
		}

		public virtual bool IsNullableType (EvaluationContext ctx, object type)
		{
			return type != null && GetTypeName (ctx, type).StartsWith ("System.Nullable`1", StringComparison.Ordinal);
		}

		public virtual bool NullableHasValue (EvaluationContext ctx, object type, object obj)
		{
			ValueReference hasValue = GetMember (ctx, type, obj, "HasValue");

			return (bool) hasValue.ObjectValue;
		}

		public virtual ValueReference NullableGetValue (EvaluationContext ctx, object type, object obj)
		{
			return GetMember (ctx, type, obj, "Value");
		}
		
		public virtual bool IsFlagsEnumType (EvaluationContext ctx, object type)
		{
			return true;
		}

		public virtual bool IsSafeToInvokeMethod (EvaluationContext ctx, object method, object obj)
		{
			return true;
		}
		
		public virtual IEnumerable<EnumMember> GetEnumMembers (EvaluationContext ctx, object type)
		{
			object longType = GetType (ctx, "System.Int64");
			var tref = new TypeValueReference (ctx, type);

			foreach (var cr in tref.GetChildReferences (ctx.Options)) {
				var c = TryCast (ctx, cr.Value, longType);
				if (c == null)
					continue;

				long val = (long) TargetObjectToObject (ctx, c);
				var em = new EnumMember { Name = cr.Name, Value = val };

				yield return em;
			}
		}
		
		public object GetBaseType (EvaluationContext ctx, object type, bool includeObjectClass)
		{
			object bt = GetBaseType (ctx, type);
			string tn = bt != null ? GetTypeName (ctx, bt) : null;

			if (!includeObjectClass && bt != null && (tn == "System.Object" || tn == "System.ValueType"))
				return null;
			if (tn == "System.Enum")
				return GetMembers (ctx, type, null, BindingFlags.GetField | BindingFlags.Instance | BindingFlags.Public).FirstOrDefault ()?.Type;

			return bt;
		}

		public virtual bool IsClassInstance (EvaluationContext ctx, object val)
		{
			return IsClass (ctx, GetValueType (ctx, val));
		}
		
		public virtual bool IsExternalType (EvaluationContext ctx, object type)
		{
			return false;
		}

		public virtual bool IsPublic (EvaluationContext ctx, object type)
		{
			return false;
		}
		
		public object GetType (EvaluationContext ctx, string name)
		{
			return GetType (ctx, name, null);
		}

		public abstract object GetType (EvaluationContext ctx, string name, object[] typeArgs);

		public virtual string GetValueTypeName (EvaluationContext ctx, object val)
		{
			return GetTypeName (ctx, GetValueType (ctx, val));
		}

		public virtual object CreateTypeObject (EvaluationContext ctx, object type)
		{
			return default (object);
		}

		public virtual bool IsTypeLoaded (EvaluationContext ctx, string typeName)
		{
			var type = GetType (ctx, typeName);

			return type != null && IsTypeLoaded (ctx, type);
		}

		public virtual bool IsTypeLoaded (EvaluationContext ctx, object type)
		{
			return true;
		}

		public virtual object ForceLoadType (EvaluationContext ctx, string typeName)
		{
			var type = GetType (ctx, typeName);

			if (type == null || IsTypeLoaded (ctx, type))
				return type;

			return ForceLoadType (ctx, type) ? type : null;
		}

		public virtual bool ForceLoadType (EvaluationContext ctx, object type)
		{
			return true;
		}

		public abstract object CreateValue (EvaluationContext ctx, object value);

		public abstract object CreateValue (EvaluationContext ctx, object type, params object[] args);

		public abstract object CreateNullValue (EvaluationContext ctx, object type);

		public virtual object GetBaseValue (EvaluationContext ctx, object val)
		{
			return val;
		}

		public virtual object CreateDelayedLambdaValue (EvaluationContext ctx, string expression, Tuple<string, object>[] localVariables)
		{
			return null;
		}

		public virtual string[] GetImportedNamespaces (EvaluationContext ctx)
		{
			return new string[0];
		}

		public virtual void GetNamespaceContents (EvaluationContext ctx, string namspace, out string[] childNamespaces, out string[] childTypes)
		{
			childTypes = childNamespaces = new string[0];
		}

		protected virtual ObjectValue CreateObjectValueImpl (EvaluationContext ctx, IObjectValueSource source, ObjectPath path, object obj, ObjectValueFlags flags)
		{
			object type = obj != null ? GetValueType (ctx, obj) : null;
			string typeName = type != null ? GetTypeName (ctx, type) : "";

			if (obj == null || IsNull (ctx, obj))
				return ObjectValue.CreateNullObject (source, path, GetDisplayTypeName (typeName), flags);

			if (IsPrimitive (ctx, obj) || IsEnum (ctx,obj))
				return ObjectValue.CreatePrimitive (source, path, GetDisplayTypeName (typeName), ctx.Evaluator.TargetObjectToExpression (ctx, obj), flags);

			if (IsArray (ctx, obj))
				return ObjectValue.CreateObject (source, path, GetDisplayTypeName (typeName), ctx.Evaluator.TargetObjectToExpression (ctx, obj), flags, null);

			EvaluationResult tvalue = null;
			TypeDisplayData tdata = null;
			string tname;

			if (IsNullableType (ctx, type)) {
				if (NullableHasValue (ctx, type, obj)) {
					ValueReference value = NullableGetValue (ctx, type, obj);

					tdata = GetTypeDisplayData (ctx, value.Type);
					obj = value.Value;
				} else {
					tdata = GetTypeDisplayData (ctx, type);
					tvalue = new EvaluationResult ("null");
				}

				tname = GetDisplayTypeName (typeName);
			} else {
				tdata = GetTypeDisplayData (ctx, type);

				if (!string.IsNullOrEmpty (tdata.TypeDisplayString) && ctx.Options.AllowDisplayStringEvaluation) {
					try {
						tname = EvaluateDisplayString (ctx, obj, tdata.TypeDisplayString);
					} catch (MissingMemberException) {
						// missing property or otherwise malformed DebuggerDisplay string
						tname = GetDisplayTypeName (typeName);
					}
				} else {
					tname = GetDisplayTypeName (typeName);
				}
			}

			if (tvalue == null) {
				if (!string.IsNullOrEmpty (tdata.ValueDisplayString) && ctx.Options.AllowDisplayStringEvaluation) {
					try {
						tvalue = new EvaluationResult (EvaluateDisplayString (ctx, obj, tdata.ValueDisplayString));
					} catch (MissingMemberException) {
						// missing property or otherwise malformed DebuggerDisplay string
						tvalue = ctx.Evaluator.TargetObjectToExpression (ctx, obj);
					}
				} else {
					tvalue = ctx.Evaluator.TargetObjectToExpression (ctx, obj);
				}
			}

			ObjectValue oval = ObjectValue.CreateObject (source, path, tname, tvalue, flags, null);
			if (!string.IsNullOrEmpty (tdata.NameDisplayString) && ctx.Options.AllowDisplayStringEvaluation) {
				try {
					oval.Name = EvaluateDisplayString (ctx, obj, tdata.NameDisplayString);
				} catch (MissingMemberException) {
					// missing property or otherwise malformed DebuggerDisplay string
				}
			}

			return oval;
		}
		
		public ObjectValue[] GetObjectValueChildren (EvaluationContext ctx, IObjectSource objectSource, object obj, int firstItemIndex, int count)
		{
			return GetObjectValueChildren (ctx, objectSource, GetValueType (ctx, obj), obj, firstItemIndex, count, true);
		}

		public virtual ObjectValue[] GetObjectValueChildren (EvaluationContext ctx, IObjectSource objectSource, object type, object obj, int firstItemIndex, int count, bool dereferenceProxy)
		{
			if (obj is EvaluationResult)
				return new ObjectValue[0];
			
			if (IsArray (ctx, obj)) {
				var agroup = new ArrayElementGroup (ctx, CreateArrayAdaptor (ctx, obj));
				return agroup.GetChildren (ctx.Options);
			}

			if (IsPrimitive (ctx, obj))
				return new ObjectValue[0];

			if (IsNullableType (ctx, type)) {
				if (NullableHasValue (ctx, type, obj)) {
					ValueReference value = NullableGetValue (ctx, type, obj);

					return GetObjectValueChildren (ctx, objectSource, value.Type, value.Value, firstItemIndex, count, dereferenceProxy);
				}

				return new ObjectValue[0];
			}

			bool showRawView = false;
			
			// If there is a proxy, it has to show the members of the proxy
			object proxy = obj;
			if (dereferenceProxy) {
				proxy = GetProxyObject (ctx, obj);
				if (proxy != obj) {
					type = GetValueType (ctx, proxy);
					showRawView = true;
				}
			}

			TypeDisplayData tdata = GetTypeDisplayData (ctx, type);
			bool groupPrivateMembers = ctx.Options.GroupPrivateMembers || IsExternalType (ctx, type);

			var values = new List<ObjectValue> ();
			BindingFlags flattenFlag = ctx.Options.FlattenHierarchy ? (BindingFlags)0 : BindingFlags.DeclaredOnly;
			BindingFlags nonPublicFlag = !(groupPrivateMembers || showRawView) ? BindingFlags.NonPublic : (BindingFlags) 0;
			BindingFlags staticFlag = ctx.Options.GroupStaticMembers ? (BindingFlags)0 : BindingFlags.Static;
			BindingFlags access = BindingFlags.Public | BindingFlags.Instance | flattenFlag | nonPublicFlag | staticFlag;
			
			// Load all members to a list before creating the object values,
			// to avoid problems with objects being invalidated due to evaluations in the target,
			var list = new List<ValueReference> ();
			list.AddRange (GetMembersSorted (ctx, objectSource, type, proxy, access));

			// Some implementations of DebuggerProxies(showRawView==true) only have private members
			if (showRawView && list.Count == 0) {
				list.AddRange (GetMembersSorted (ctx, objectSource, type, proxy, access | BindingFlags.NonPublic));
			}
			var names = new ObjectValueNameTracker (ctx);
			object tdataType = type;
			
			foreach (ValueReference val in list) {
				try {
					object decType = val.DeclaringType;
					if (decType != null && decType != tdataType) {
						tdataType = decType;
						tdata = GetTypeDisplayData (ctx, decType);
					}

					DebuggerBrowsableState state = tdata.GetMemberBrowsableState (val.Name);
					if (state == DebuggerBrowsableState.Never)
						continue;

					if (state == DebuggerBrowsableState.RootHidden && dereferenceProxy) {
						object ob = val.Value;
						if (ob != null) {
							values.Clear ();
							values.AddRange (GetObjectValueChildren (ctx, val, ob, -1, -1));
							showRawView = true;
							break;
						}
					} else {
						ObjectValue oval = val.CreateObjectValue (true);
						names.Disambiguate (val, oval);
						values.Add (oval);
					}
				} catch (Exception ex) {
					ctx.WriteDebuggerError (ex);
					values.Add (ObjectValue.CreateError (null, new ObjectPath (val.Name), GetDisplayTypeName (GetTypeName (ctx, val.Type)), ex.Message, val.Flags));
				}
			}

			if (showRawView) {
				values.Add (RawViewSource.CreateRawView (ctx, objectSource, obj));
			} else {
				if (IsArray (ctx, proxy)) {
					var col = CreateArrayAdaptor (ctx, proxy);
					var agroup = new ArrayElementGroup (ctx, col);
					var val = ObjectValue.CreateObject (null, new ObjectPath ("Raw View"), "", "", ObjectValueFlags.ReadOnly, values.ToArray ());

					values = new List<ObjectValue> ();
					values.Add (val);
					values.AddRange (agroup.GetChildren (ctx.Options));
				} else {
					if (ctx.Options.GroupStaticMembers && HasMembers (ctx, type, proxy, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | flattenFlag)) {
						access = BindingFlags.Static | BindingFlags.Public | flattenFlag | nonPublicFlag;
						values.Add (FilteredMembersSource.CreateStaticsNode (ctx, objectSource, type, proxy, access));
					}

					if (groupPrivateMembers && HasMembers (ctx, type, proxy, BindingFlags.Instance | BindingFlags.NonPublic | flattenFlag | staticFlag))
						values.Add (FilteredMembersSource.CreateNonPublicsNode (ctx, objectSource, type, proxy, BindingFlags.Instance | BindingFlags.NonPublic | flattenFlag | staticFlag));
					
					if (!ctx.Options.FlattenHierarchy) {
						object baseType = GetBaseType (ctx, type, false);
						if (baseType != null)
							values.Insert (0, BaseTypeViewSource.CreateBaseTypeView (ctx, objectSource, baseType, proxy));
					}

					if (ctx.SupportIEnumerable) {
						var iEnumerableType = GetImplementedInterfaces (ctx, type).FirstOrDefault ((interfaceType) => {
							string interfaceName = GetTypeName (ctx, interfaceType);
							if (interfaceName == "System.Collections.IEnumerable")
								return true;
							if (interfaceName == "System.Collections.Generic.IEnumerable`1")
								return true;
							return false;
						});
						if (iEnumerableType != null)
							values.Add (ObjectValue.CreatePrimitive (new EnumerableSource (proxy, iEnumerableType, ctx), new ObjectPath ("IEnumerator"), "", new EvaluationResult (""), ObjectValueFlags.ReadOnly | ObjectValueFlags.Object | ObjectValueFlags.Group | ObjectValueFlags.IEnumerable));
					}
				}
			}

			return values.ToArray ();
		}

		public ObjectValue[] GetExpressionValuesAsync (EvaluationContext ctx, string[] expressions)
		{
			var values = new ObjectValue[expressions.Length];

			for (int n = 0; n < values.Length; n++) {
				string exp = expressions[n];

				// This is a workaround to a bug in mono 2.0. That mono version fails to compile
				// an anonymous method here
				var edata = new ExpData (ctx, exp, this);
				values[n] = asyncEvaluationTracker.Run (exp, ObjectValueFlags.Literal, edata.Run);
			}

			return values;
		}
		
		class ExpData
		{
			readonly ObjectValueAdaptor adaptor;
			readonly EvaluationContext ctx;
			readonly string exp;
			
			public ExpData (EvaluationContext ctx, string exp, ObjectValueAdaptor adaptor)
			{
				this.ctx = ctx;
				this.exp = exp;
				this.adaptor = adaptor;
			}
			
			public ObjectValue Run ()
			{
				return adaptor.GetExpressionValue (ctx, exp);
			}
		}

		public virtual ValueReference GetIndexerReference (EvaluationContext ctx, object target, object[] indices)
		{
			return null;
		}

		public virtual ValueReference GetIndexerReference (EvaluationContext ctx, object target, object type, object[] indices)
		{
			return GetIndexerReference (ctx, target, indices);
		}

		public ValueReference GetLocalVariable (EvaluationContext ctx, string name)
		{
			return OnGetLocalVariable (ctx, name);
		}

		protected virtual ValueReference OnGetLocalVariable (EvaluationContext ctx, string name)
		{
			ValueReference best = null;
			foreach (ValueReference var in GetLocalVariables (ctx)) {
				if (var.Name == name)
					return var;
				if (!ctx.Evaluator.CaseSensitive && var.Name.Equals (name, StringComparison.CurrentCultureIgnoreCase))
					best = var;
			}
			return best;
		}

		public virtual ValueReference GetParameter (EvaluationContext ctx, string name)
		{
			return OnGetParameter (ctx, name);
		}

		protected virtual ValueReference OnGetParameter (EvaluationContext ctx, string name)
		{
			ValueReference best = null;
			foreach (ValueReference var in GetParameters (ctx)) {
				if (var.Name == name)
					return var;
				if (!ctx.Evaluator.CaseSensitive && var.Name.Equals (name, StringComparison.CurrentCultureIgnoreCase))
					best = var;
			}
			return best;
		}

		public IEnumerable<ValueReference> GetLocalVariables (EvaluationContext ctx)
		{
			return OnGetLocalVariables (ctx);
		}

		public ValueReference GetThisReference (EvaluationContext ctx)
		{
			return OnGetThisReference (ctx);
		}

		public IEnumerable<ValueReference> GetParameters (EvaluationContext ctx)
		{
			return OnGetParameters (ctx);
		}

		protected virtual IEnumerable<ValueReference> OnGetLocalVariables (EvaluationContext ctx)
		{
			yield break;
		}

		protected virtual IEnumerable<ValueReference> OnGetParameters (EvaluationContext ctx)
		{
			yield break;
		}

		protected virtual ValueReference OnGetThisReference (EvaluationContext ctx)
		{
			return null;
		}

		public virtual ValueReference GetCurrentException (EvaluationContext ctx)
		{
			return null;
		}

		public virtual object GetEnclosingType (EvaluationContext ctx)
		{
			return null;
		}

		protected virtual CompletionData GetMemberCompletionData (EvaluationContext ctx, ValueReference vr)
		{
			var data = new CompletionData ();

			foreach (var cv in vr.GetChildReferences (ctx.Options))
				data.Items.Add (new CompletionItem (cv.Name, cv.Flags));

			data.ExpressionLength = 0;

			return data;
		}

		public virtual CompletionData GetExpressionCompletionData (EvaluationContext ctx, string expr)
		{
			if (expr == null)
				return null;

			int dot = expr.LastIndexOf ('.');

			if (dot != -1) {
				try {
					var vr = ctx.Evaluator.Evaluate (ctx, expr.Substring (0, dot), null);
					if (vr != null) {
						var completionData = GetMemberCompletionData (ctx, vr);
						completionData.ExpressionLength = expr.Length - dot - 1;
						return completionData;
					}

					// FIXME: handle types and namespaces...
				} catch (EvaluatorException) {
				} catch (Exception ex) {
					ctx.WriteDebuggerError (ex);
				}

				return null;
			}

			bool lastWastLetter = false;
			int i = expr.Length - 1;

			while (i >= 0) {
				char c = expr[i--];
				if (!char.IsLetterOrDigit (c) && c != '_')
					break;

				lastWastLetter = !char.IsDigit (c);
			}

			if (lastWastLetter || expr.Length == 0) {
				var data = new CompletionData ();
				data.ExpressionLength = expr.Length - (i + 1);

				// Local variables
				
				foreach (var vc in GetLocalVariables (ctx)) {
					data.Items.Add (new CompletionItem (vc.Name, vc.Flags));
				}

				// Parameters
				
				foreach (var vc in GetParameters (ctx)) {
					data.Items.Add (new CompletionItem (vc.Name, vc.Flags));
				}

				// Members
				
				ValueReference thisobj = GetThisReference (ctx);
				
				if (thisobj != null)
					data.Items.Add (new CompletionItem ("this", ObjectValueFlags.Field | ObjectValueFlags.ReadOnly));

				object type = GetEnclosingType (ctx);
				
				foreach (var vc in GetMembers (ctx, null, type, thisobj != null ? thisobj.Value : null)) {
					data.Items.Add (new CompletionItem (vc.Name, vc.Flags));
				}
				
				if (data.Items.Count > 0)
					return data;
			}

			return null;
		}
		
		public IEnumerable<ValueReference> GetMembers (EvaluationContext ctx, IObjectSource objectSource, object t, object co)
		{
			foreach (ValueReference val in GetMembers (ctx, objectSource, t, co, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) {
				val.ParentSource = objectSource;
				yield return val;
			}
		}

		public ValueReference GetMember (EvaluationContext ctx, IObjectSource objectSource, object co, string name)
		{
			return GetMember (ctx, objectSource, GetValueType (ctx, co), co, name);
		}

		protected virtual ValueReference OnGetMember (EvaluationContext ctx, IObjectSource objectSource, object t, object co, string name)
		{
			return GetMember (ctx, t, co, name);
		}

		public ValueReference GetMember (EvaluationContext ctx, IObjectSource objectSource, object t, object co, string name)
		{
			ValueReference m = OnGetMember (ctx, objectSource, t, co, name);
			if (m != null)
				m.ParentSource = objectSource;
			return m;
		}
		
		protected virtual ValueReference GetMember (EvaluationContext ctx, object t, object co, string name)
		{
			ValueReference best = null;
			foreach (ValueReference var in GetMembers (ctx, t, co, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) {
				if (var.Name == name)
					return var;
				if (!ctx.Evaluator.CaseSensitive && var.Name.Equals (name, StringComparison.CurrentCultureIgnoreCase))
					best = var;
			}
			return best;
		}

		internal IEnumerable<ValueReference> GetMembersSorted (EvaluationContext ctx, IObjectSource objectSource, object t, object co)
		{
			return GetMembersSorted (ctx, objectSource, t, co, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		}
		
		internal IEnumerable<ValueReference> GetMembersSorted (EvaluationContext ctx, IObjectSource objectSource, object t, object co, BindingFlags bindingFlags)
		{
			var list = new List<ValueReference> ();

			foreach (var vr in GetMembers (ctx, objectSource, t, co, bindingFlags)) {
				vr.ParentSource = objectSource;
				list.Add (vr);
			}

			list.Sort ((v1, v2) => string.Compare (v1.Name, v2.Name, StringComparison.Ordinal));

			return list;
		}
		
		public bool HasMembers (EvaluationContext ctx, object t, object co, BindingFlags bindingFlags)
		{
			return GetMembers (ctx, t, co, bindingFlags).Any ();
		}

		public bool HasMember (EvaluationContext ctx, object type, string memberName)
		{
			return HasMember (ctx, type, memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		}

		public abstract bool HasMember (EvaluationContext ctx, object type, string memberName, BindingFlags bindingFlags);
		
		/// <summary>
		/// Returns all members of a type. The following binding flags have to be honored:
		/// BindingFlags.Static, BindingFlags.Instance, BindingFlags.Public, BindingFlags.NonPublic, BindingFlags.DeclareOnly
		/// </summary>
		protected abstract IEnumerable<ValueReference> GetMembers (EvaluationContext ctx, object t, object co, BindingFlags bindingFlags);

		/// <summary>
		/// Returns all members of a type. The following binding flags have to be honored:
		/// BindingFlags.Static, BindingFlags.Instance, BindingFlags.Public, BindingFlags.NonPublic, BindingFlags.DeclareOnly
		/// </summary>
		protected virtual IEnumerable<ValueReference> GetMembers (EvaluationContext ctx, IObjectSource objectSource, object t, object co, BindingFlags bindingFlags)
		{
			return GetMembers (ctx, t, co, bindingFlags);
		}

		public virtual IEnumerable<object> GetNestedTypes (EvaluationContext ctx, object type)
		{
			yield break;
		}

		public virtual IEnumerable<object> GetImplementedInterfaces (EvaluationContext ctx, object type)
		{
			yield break;
		}

		public virtual object GetParentType (EvaluationContext ctx, object type)
		{
			var tt = type as Type;

			if (tt != null)
				return tt.DeclaringType;

			var name = GetTypeName (ctx, type);
			int plus = name.LastIndexOf ('+');

			return plus != -1 ? GetType (ctx, name.Substring (0, plus)) : null;
		}
		
		public virtual object CreateArray (EvaluationContext ctx, object type, object[] values)
		{
			var arrType = GetType (ctx, "System.Collections.ArrayList");
			var arrayList = CreateValue (ctx, arrType, new object[0]);
			object[] objTypes = { GetType (ctx, "System.Object") };

			foreach (object value in values)
				RuntimeInvoke (ctx, arrType, arrayList, "Add", objTypes, new [] { value });
			
			var typof = CreateTypeObject (ctx, type);
			objTypes = new [] { GetType (ctx, "System.Type") };

			return RuntimeInvoke (ctx, arrType, arrayList, "ToArray", objTypes, new [] { typof });
		}

		public virtual object CreateArray (EvaluationContext ctx, object type, int [] lengths)
		{
			if (lengths.Length > 3) {
				throw new NotSupportedException ("Arrays with more than 3 demensions are not supported.");
			}
			var arrType = GetType (ctx, "System.Array");
			var intType = GetType (ctx, "System.Int32");
			var typeType = GetType (ctx, "System.Type");
			var arguments = new object [lengths.Length + 1];
			var argTypes = new object [lengths.Length + 1];
			arguments [0] = CreateTypeObject (ctx, type);
			argTypes [0] = typeType;
			for (int i = 0; i < lengths.Length; i++) {
				arguments [i + 1] = FromRawValue (ctx, lengths [i]);
				argTypes [i + 1] = intType;
			}

			return RuntimeInvoke (ctx, arrType, null, "CreateInstance", argTypes, arguments);
		}
		
		public virtual object ToRawValue (EvaluationContext ctx, IObjectSource source, object obj)
		{
			if (IsEnum (ctx, obj)) {
				var longType = GetType (ctx, "System.Int64");
				var c = Cast (ctx, obj, longType);

				return TargetObjectToObject (ctx, c);
			}
			
			if (ctx.Options.ChunkRawStrings && IsString (ctx, obj)) {
				var adaptor = CreateStringAdaptor (ctx, obj);
				return new RawValueString (new RemoteRawValueString (adaptor, obj));
			}
			
			if (IsPrimitive (ctx, obj))
				return TargetObjectToObject (ctx, obj);
				
			if (IsArray (ctx, obj)) {
				var adaptor = CreateArrayAdaptor (ctx, obj);
				return new RawValueArray (new RemoteRawValueArray (ctx, source, adaptor, obj));
			}
			
			return new RawValue (new RemoteRawValue (ctx, source, obj));
		}
		
		public virtual object FromRawValue (EvaluationContext ctx, object obj)
		{
			var rawValue = obj as RawValue;
			if (rawValue != null) {
				var val = rawValue.Source as RemoteRawValue;
				if (val == null)
					throw new InvalidOperationException ("Unknown RawValue source: " + rawValue.Source);

				return val.TargetObject;
			}

			var rawArray = obj as RawValueArray;
			if (rawArray != null) {
				var val = rawArray.Source as RemoteRawValueArray;
				if (val == null)
					throw new InvalidOperationException ("Unknown RawValueArray source: " + rawArray.Source);

				return val.TargetObject;
			}

			var rawString = obj as RawValueString;
			if (rawString != null) {
				var val = rawString.Source as RemoteRawValueString;
				if (val == null)
					throw new InvalidOperationException ("Unknown RawValueString source: " + rawString.Source);

				return val.TargetObject;
			}

			var array = obj as Array;
			if (array != null) {
				if (obj.GetType ().GetElementType () == typeof (RawValue))
					throw new NotSupportedException ();

				var elemType = GetType (ctx, obj.GetType ().GetElementType ().FullName);
				if (elemType == null)
					throw new EvaluatorException ("Unknown target type: {0}", obj.GetType ().GetElementType ().FullName);

				var values = new object [array.Length];
				for (int n = 0; n < values.Length; n++)
					values[n] = FromRawValue (ctx, array.GetValue (n));

				return CreateArray (ctx, elemType, values);
			}

			return CreateValue (ctx, obj);
		}
		
		public virtual object TargetObjectToObject (EvaluationContext ctx, object obj)
		{
			if (IsNull (ctx, obj))
				return null;

			if (IsArray (ctx, obj)) {
				ICollectionAdaptor adaptor = CreateArrayAdaptor (ctx, obj);
				string ename = GetDisplayTypeName (GetTypeName (ctx, adaptor.ElementType));
				int[] dims = adaptor.GetDimensions ();
				var tn = new StringBuilder ("[");

				for (int n = 0; n < dims.Length; n++) {
					if (n > 0)
						tn.Append (',');
					tn.Append (dims[n]);
				}

				tn.Append ("]");

				int i = ename.LastIndexOf ('>');
				if (i == -1)
					i = 0;

				i = ename.IndexOf ('[', i);

				if (i != -1)
					return new EvaluationResult ("{" + ename.Substring (0, i) + tn + ename.Substring (i) + "}");

				return new EvaluationResult ("{" + ename + tn + "}");
			}

			object type = GetValueType (ctx, obj);
			string typeName = GetTypeName (ctx, type);
			if (IsEnum (ctx, obj)) {
				object longType = GetType (ctx, "System.Int64");
				object c = Cast (ctx, obj, longType);
				long val = (long) TargetObjectToObject (ctx, c);
				long rest = val;
				string composed = string.Empty;
				string composedDisplay = string.Empty;

				foreach (var em in GetEnumMembers (ctx, type)) {
					if (em.Value == val)
						return new EvaluationResult (typeName + "." + em.Name, em.Name);

					if (em.Value != 0 && (rest & em.Value) == em.Value) {
						rest &= ~em.Value;
						if (composed.Length > 0) {
							composed += " | ";
							composedDisplay += " | ";
						}
						composed += typeName + "." + em.Name;
						composedDisplay += em.Name;
					}
				}

				if (IsFlagsEnumType (ctx, type) && rest == 0 && composed.Length > 0)
					return new EvaluationResult (composed, composedDisplay);

				return new EvaluationResult (val.ToString ());
			}

			if (typeName == "System.Decimal") {
				string res = CallToString (ctx, obj);
				// This returns the decimal formatted using the current culture. It has to be converted to invariant culture.
				decimal dec = decimal.Parse (res);
				res = dec.ToString (System.Globalization.CultureInfo.InvariantCulture);
				return new EvaluationResult (res);
			}

			if (typeName == "System.nfloat" || typeName == "System.nint") {
				return TargetObjectToObject (ctx, GetMembersSorted (ctx, null, type, obj, BindingFlags.Instance | BindingFlags.NonPublic).Single ().Value);
			}

			if (IsClassInstance (ctx, obj)) {
				TypeDisplayData tdata = GetTypeDisplayData (ctx, GetValueType (ctx, obj));
				if (!string.IsNullOrEmpty (tdata.ValueDisplayString) && ctx.Options.AllowDisplayStringEvaluation) {
					try {
						return new EvaluationResult (EvaluateDisplayString (ctx, obj, tdata.ValueDisplayString));
					} catch (MissingMemberException) {
						// missing property or otherwise malformed DebuggerDisplay string
					}
				}

				// Return the type name
				if (ctx.Options.AllowToStringCalls) {
					try {
						return new EvaluationResult ("{" + CallToString (ctx, obj) + "}");
					} catch (TimeOutException) {
						// ToString() timed out, fall back to default behavior.
					}
				}
				
				if (!string.IsNullOrEmpty (tdata.TypeDisplayString) && ctx.Options.AllowDisplayStringEvaluation) {
					try {
						return new EvaluationResult ("{" + EvaluateDisplayString (ctx, obj, tdata.TypeDisplayString) + "}");
					} catch (MissingMemberException) {
						// missing property or otherwise malformed DebuggerDisplay string
					}
				}
				
				return new EvaluationResult ("{" + GetDisplayTypeName (GetValueTypeName (ctx, obj)) + "}");
			}

			return new EvaluationResult ("{" + CallToString (ctx, obj) + "}");
		}

		public object Convert (EvaluationContext ctx, object obj, object targetType)
		{
			if (obj == null)
				return null;

			object res = TryConvert (ctx, obj, targetType);
			if (res != null)
				return res;

			throw new EvaluatorException ("Can't convert an object of type '{0}' to type '{1}'", GetValueTypeName (ctx, obj), GetTypeName (ctx, targetType));
		}

		public virtual object TryConvert (EvaluationContext ctx, object obj, object targetType)
		{
			return TryCast (ctx, obj, targetType);
		}

		public virtual object Cast (EvaluationContext ctx, object obj, object targetType)
		{
			if (obj == null)
				return null;

			object res = TryCast (ctx, obj, targetType);
			if (res != null)
				return res;

			throw new EvaluatorException ("Can't cast an object of type '{0}' to type '{1}'", GetValueTypeName (ctx, obj), GetTypeName (ctx, targetType));
		}

		public virtual string CallToString (EvaluationContext ctx, object obj)
		{
			return GetValueTypeName (ctx, obj);
		}

		// FIXME: next time we can break ABI/API, make this abstract
		protected virtual object GetBaseTypeWithAttribute (EvaluationContext ctx, object type, object attrType)
		{
			return null;
		}

		public object GetProxyObject (EvaluationContext ctx, object obj)
		{
			TypeDisplayData data = GetTypeDisplayData (ctx, GetValueType (ctx, obj));
			if (string.IsNullOrEmpty (data.ProxyType) || !ctx.Options.AllowDebuggerProxy)
				return obj;

			string proxyType = data.ProxyType;
			object[] typeArgs = null;

			int index = proxyType.IndexOf ('`');
			if (index != -1) {
				// The proxy type is an uninstantiated generic type.
				// The number of type args of the proxy must match the args of the target object
				int startIndex = index + 1;
				int endIndex = index + 1;

				while (endIndex < proxyType.Length && char.IsDigit (proxyType[endIndex]))
					endIndex++;

				var attrType = GetType (ctx, "System.Diagnostics.DebuggerTypeProxyAttribute");
				int num = int.Parse (proxyType.Substring (startIndex, endIndex - startIndex));
				var proxiedType = GetBaseTypeWithAttribute (ctx, GetValueType (ctx, obj), attrType);

				if (proxiedType == null || !IsGenericType (ctx, proxiedType))
					return obj;

				typeArgs = GetTypeArgs (ctx, proxiedType);
				if (typeArgs.Length != num)
					return obj;

				if (endIndex < proxyType.Length) {
					// chop off the []'d list of generic type arguments
					proxyType = proxyType.Substring (0, endIndex);
				}
			}
			
			object ttype = GetType (ctx, proxyType, typeArgs);
			if (ttype == null) {
				// the proxy type string might be in the form: "Namespace.TypeName, Assembly...", chop off the ", Assembly..." bit.
				if ((index = proxyType.IndexOf (',')) != -1)
					ttype = GetType (ctx, proxyType.Substring (0, index).Trim (), typeArgs);
			}
			if (ttype == null)
				throw new EvaluatorException ("Unknown type '{0}'", data.ProxyType);

			try {
				object val = CreateValue (ctx, ttype, obj);
				return val ?? obj;
			} catch (EvaluatorException) {
				// probably couldn't find the .ctor for the proxy type because the linker stripped it out
				return obj;
			} catch (Exception ex) {
				ctx.WriteDebuggerError (ex);
				return obj;
			}
		}

		public TypeDisplayData GetTypeDisplayData (EvaluationContext ctx, object type)
		{
			if (!IsClass (ctx, type))
				return TypeDisplayData.Default;

			TypeDisplayData td;
			string tname = GetTypeName (ctx, type);
			if (typeDisplayData.TryGetValue (tname, out td))
				return td;

			try {
				td = OnGetTypeDisplayData (ctx, type);
			} catch (Exception ex) {
				ctx.WriteDebuggerError (ex);
			}

			if (td == null)
				typeDisplayData[tname] = td = TypeDisplayData.Default;
			else
				typeDisplayData[tname] = td;

			return td;
		}

		protected virtual TypeDisplayData OnGetTypeDisplayData (EvaluationContext ctx, object type)
		{
			return null;
		}

		static bool IsQuoted (string str)
		{
			return str.Length >= 2 && str[0] == '"' && str[str.Length - 1] == '"';
		}

		public string EvaluateDisplayString (EvaluationContext ctx, object obj, string expr)
		{
			var display = new StringBuilder ();
			int i = expr.IndexOf ('{');
			int last = 0;

			while (i != -1 && i < expr.Length) {
				display.Append (expr, last, i - last);
				i++;

				int j = expr.IndexOf ('}', i);
				if (j == -1)
					return expr;

				string memberExpr = expr.Substring (i, j - i).Trim ();
				if (memberExpr.Length == 0)
					return expr;

				int comma = memberExpr.LastIndexOf (',');
				bool noquotes = false;
				if (comma != -1) {
					var option = memberExpr.Substring (comma + 1).Trim ();
					memberExpr = memberExpr.Substring (0, comma).Trim ();
					noquotes |= option == "nq";
				}
				
				var props = memberExpr.Split (new [] { '.' });
				object val = obj;
				
				for (int k = 0; k < props.Length; k++) {
					var member = GetMember (ctx, null, GetValueType (ctx, val), val, props [k]);
					if (member != null) {
						val = member.Value;
					} else {
						var methodName = props [k].TrimEnd ('(', ')', ' ');
						if (HasMethod (ctx, GetValueType (ctx, val), methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)) {
							val = RuntimeInvoke (ctx, GetValueType (ctx, val), val, methodName, new object[0], new object[0]);
						} else {
							val = null;
							break;
						}
					}
				}
				
				if (val != null) {
					var str = ctx.Evaluator.TargetObjectToString (ctx, val);
					if (str == null)
						display.Append ("null");
					else if (noquotes && IsQuoted (str))
						display.Append (str, 1, str.Length - 2);
					else
						display.Append (str);
				} else {
					throw new MissingMemberException (GetValueTypeName (ctx, obj), memberExpr);
				}

				last = j + 1;
				i = expr.IndexOf ('{', last);
			}

			display.Append (expr, last, expr.Length - last);

			return display.ToString ();
		}

		public void AsyncExecute (AsyncOperation operation, int timeout)
		{
			asyncOperationManager.Invoke (operation, timeout);
		}

		public ObjectValue CreateObjectValueAsync (string name, ObjectValueFlags flags, ObjectEvaluatorDelegate evaluator)
		{
			return asyncEvaluationTracker.Run (name, flags, evaluator);
		}
		
		public bool IsEvaluating {
			get { return asyncEvaluationTracker.IsEvaluating; }
		}

		public void CancelAsyncOperations ( )
		{
			asyncEvaluationTracker.Stop ();
			asyncOperationManager.AbortAll ();
			asyncEvaluationTracker.WaitForStopped ();
		}

		public ObjectValue GetExpressionValue (EvaluationContext ctx, string exp)
		{
			try {
				var var = ctx.Evaluator.Evaluate (ctx, exp);

				if (var == null)
					return ObjectValue.CreateUnknown (exp);

				var value = var.CreateObjectValue (ctx.Options);
				value.Name = exp;
				return value;
			} catch (ImplicitEvaluationDisabledException) {
				return ObjectValue.CreateImplicitNotSupported (ctx.ExpressionValueSource, new ObjectPath (exp), "", ObjectValueFlags.None);
			} catch (NotSupportedExpressionException ex) {
				return ObjectValue.CreateNotSupported (ctx.ExpressionValueSource, new ObjectPath (exp), "", ex.Message, ObjectValueFlags.None);
			} catch (EvaluatorException ex) {
				return ObjectValue.CreateError (ctx.ExpressionValueSource, new ObjectPath (exp), "", ex.Message, ObjectValueFlags.None);
			} catch (Exception ex) {
				ctx.WriteDebuggerError (ex);
				return ObjectValue.CreateUnknown (exp);
			}
		}

		public virtual bool HasMethodWithParamLength (EvaluationContext ctx, object targetType, string methodName, BindingFlags flags, int paramLength)
		{
			return false;
		}

		public bool HasMethod (EvaluationContext ctx, object targetType, string methodName)
		{
			BindingFlags flags = BindingFlags.Instance | BindingFlags.Static;

			if (!ctx.Evaluator.CaseSensitive)
				flags |= BindingFlags.IgnoreCase;

			return HasMethod (ctx, targetType, methodName, null, null, flags);
		}
		
		public bool HasMethod (EvaluationContext ctx, object targetType, string methodName, BindingFlags flags)
		{
			return HasMethod (ctx, targetType, methodName, null, null, flags);
		}
		
		// argTypes can be null, meaning that it has to return true if there is any method with that name
		// flags will only contain Static or Instance flags
		public bool HasMethod (EvaluationContext ctx, object targetType, string methodName, object[] argTypes, BindingFlags flags)
		{
			return HasMethod (ctx, targetType, methodName, null, argTypes, flags);
		}

		// argTypes can be null, meaning that it has to return true if there is any method with that name
		// flags will only contain Static or Instance flags
		public abstract bool HasMethod (EvaluationContext ctx, object targetType, string methodName, object[] genericTypeArgs, object[] argTypes, BindingFlags flags);

		// outarg `untyped lambda`
		// if one of argtypes is untyped lambda, this will resolve its type.
		public virtual bool HasMethod (EvaluationContext ctx, object targetType, string methodName, object[] genericTypeArgs, object[] argTypes, BindingFlags flags, out Tuple<int, object>[] resolvedLambdaTypes)
		{
			resolvedLambdaTypes = null;
			return HasMethod (ctx, targetType, methodName, genericTypeArgs, argTypes, flags);
		}

		public object RuntimeInvoke (EvaluationContext ctx, object targetType, object target, string methodName, object[] argTypes, object[] argValues)
		{
			return RuntimeInvoke (ctx, targetType, target, methodName, null, argTypes, argValues);
		}

		public virtual object RuntimeInvoke (EvaluationContext ctx, object targetType, object target, string methodName, object[] genericTypeArgs, object[] argTypes, object[] argValues, out object[] outArgs){
			outArgs = null;
			return RuntimeInvoke (ctx, targetType, target, methodName, genericTypeArgs, argTypes, argValues);
		}

		public abstract object RuntimeInvoke (EvaluationContext ctx, object targetType, object target, string methodName, object[] genericTypeArgs, object[] argTypes, object[] argValues);
		
		public virtual ValidationResult ValidateExpression (EvaluationContext ctx, string expression)
		{
			return ctx.Evaluator.ValidateExpression (ctx, expression);
		}
	}

	public class TypeDisplayData
	{
		public string ProxyType { get; internal set; }
		public string ValueDisplayString { get; internal set; }
		public string TypeDisplayString { get; internal set; }
		public string NameDisplayString { get; internal set; }
		public bool IsCompilerGenerated { get; internal set; }
		
		public bool IsProxyType {
			get { return ProxyType != null; }
		}

		public static readonly TypeDisplayData Default = new TypeDisplayData (null, null, null, null, false, null);

		public Dictionary<string, DebuggerBrowsableState> MemberData { get; internal set; }
		
		public TypeDisplayData (string proxyType, string valueDisplayString, string typeDisplayString,
			string nameDisplayString, bool isCompilerGenerated, Dictionary<string, DebuggerBrowsableState> memberData)
		{
			ProxyType = proxyType;
			ValueDisplayString = valueDisplayString;
			TypeDisplayString = typeDisplayString;
			NameDisplayString = nameDisplayString;
			IsCompilerGenerated = isCompilerGenerated;
			MemberData = memberData;
		}

		public DebuggerBrowsableState GetMemberBrowsableState (string name)
		{
			if (MemberData == null)
				return DebuggerBrowsableState.Collapsed;

			DebuggerBrowsableState state;
			if (!MemberData.TryGetValue (name, out state))
				state = DebuggerBrowsableState.Collapsed;

			return state;
		}
	}
	
	class ObjectValueNameTracker
	{
		readonly Dictionary<string,KeyValuePair<ObjectValue, ValueReference>> names = new Dictionary<string,KeyValuePair<ObjectValue, ValueReference>> ();
		readonly EvaluationContext ctx;
		
		public ObjectValueNameTracker (EvaluationContext ctx)
		{
			this.ctx = ctx;
		}
		
		/// <summary>
		/// Disambiguate the ObjectValue's name (in the case where the property name also exists in a base class).
		/// </summary>
		/// <param name='val'>
		/// The ValueReference.
		/// </param>
		/// <param name='oval'>
		/// The ObjectValue.
		/// </param>
		public void Disambiguate (ValueReference val, ObjectValue oval)
		{
			KeyValuePair<ObjectValue, ValueReference> other;

			if (names.TryGetValue (oval.Name, out other)) {
				object tn = val.DeclaringType;
				
				if (tn != null)
					oval.Name += " (" + ctx.Adapter.GetDisplayTypeName (ctx, tn) + ")";
				if (!other.Key.Name.EndsWith (")", StringComparison.Ordinal)) {
					tn = other.Value.DeclaringType;
					if (tn != null)
						other.Key.Name += " (" + ctx.Adapter.GetDisplayTypeName (ctx, tn) + ")";
				}
			}
			
			names [oval.Name] = new KeyValuePair<ObjectValue, ValueReference> (oval, val);
		}
	}
	
	public struct EnumMember
	{
		public string Name { get; set; }
		public long Value { get; set; }
	}
}
