// 
// SoftDebuggerAdaptor.cs
//  
// Authors: Lluis Sanchez Gual <lluis@novell.com>
//          Jeffrey Stedfast <jeff@xamarin.com>
// 
// Copyright (c) 2009 Novell, Inc (http://www.novell.com)
// Copyright (c) 2011,2012 Xamain Inc. (http://www.xamarin.com)
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
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

using Mono.Debugger.Soft;
using Mono.Debugging.Backend;
using Mono.Debugging.Evaluation;
using Mono.Debugging.Client;


namespace Mono.Debugging.Soft
{
	public class SoftDebuggerAdaptor : ObjectValueAdaptor
	{
		static readonly Dictionary<Type, OpCode> convertOps = new Dictionary<Type, OpCode> ();
		delegate object TypeCastDelegate (object value);

		static SoftDebuggerAdaptor ()
		{
			convertOps.Add (typeof (double), OpCodes.Conv_R8);
			convertOps.Add (typeof (float), OpCodes.Conv_R4);
			convertOps.Add (typeof (ulong), OpCodes.Conv_U8);
			convertOps.Add (typeof (uint), OpCodes.Conv_U4);
			convertOps.Add (typeof (ushort), OpCodes.Conv_U2);
			convertOps.Add (typeof (char), OpCodes.Conv_U2);
			convertOps.Add (typeof (byte), OpCodes.Conv_U1);
			convertOps.Add (typeof (long), OpCodes.Conv_I8);
			convertOps.Add (typeof (int), OpCodes.Conv_I4);
			convertOps.Add (typeof (short), OpCodes.Conv_I2);
			convertOps.Add (typeof (sbyte), OpCodes.Conv_I1);
		}

		public SoftDebuggerAdaptor ()
		{
		}

		public SoftDebuggerSession Session {
			get { return (SoftDebuggerSession)DebuggerSession; }
			set { DebuggerSession = value; }
		}

		//static string GetPrettyMethodName (EvaluationContext ctx, MethodMirror method)
		//{
		//	var name = new System.Text.StringBuilder ();

		//	name.Append (ctx.Adapter.GetDisplayTypeName (method.ReturnType.FullName));
		//	name.Append (" ");
		//	name.Append (ctx.Adapter.GetDisplayTypeName (method.DeclaringType.FullName));
		//	name.Append (".");
		//	name.Append (method.Name);

		//	if (method.VirtualMachine.Version.AtLeast (2, 12)) {
		//		if (method.IsGenericMethodDefinition || method.IsGenericMethod) {
		//			name.Append ("<");
		//			if (method.VirtualMachine.Version.AtLeast (2, 15)) {
		//				var argTypes = method.GetGenericArguments ();
		//				for (int i = 0; i < argTypes.Length; i++) {
		//					if (i != 0)
		//						name.Append (", ");
		//					name.Append (ctx.Adapter.GetDisplayTypeName (argTypes[i].FullName));
		//				}
		//			}
		//			name.Append (">");
		//		}
		//	}

		//	name.Append (" (");
		//	var @params = method.GetParameters ();
		//	for (int i = 0; i < @params.Length; i++) {
		//		if (i != 0)
		//			name.Append (", ");
		//		if (@params[i].Attributes.HasFlag (ParameterAttributes.Out)) {
		//			if (@params[i].Attributes.HasFlag (ParameterAttributes.In))
		//				name.Append ("ref ");
		//			else
		//				name.Append ("out ");
		//		}
		//		name.Append (ctx.Adapter.GetDisplayTypeName (@params[i].ParameterType.FullName));
		//		name.Append (" ");
		//		name.Append (@params[i].Name);
		//	}
		//	name.Append (")");

		//	return name.ToString ();
		//}

		string InvokeToString (SoftEvaluationContext ctx, MethodMirror method, object obj)
		{
			try {
				var result = ctx.RuntimeInvoke (method, obj, new Value[0]);
				var str = result as StringMirror;
				if (str != null)
                {
					return MirrorStringToString(ctx, str);
				}
					

				return null;
			} catch {
				return GetDisplayTypeName (GetValueTypeName (ctx, obj));
			}
		}
		
		public override string CallToString (EvaluationContext ctx, object obj)
		{
			if (obj == null)
				return null;
			var str = obj as StringMirror;
			if (str != null)
            {
				return str.Value;
			}

			var em = obj as EnumMirror;
			if (em != null)
				return em.StringValue;

			var primitive = obj as PrimitiveValue;
			if (primitive != null)
				return primitive.Value.ToString ();
			var pointer = obj as PointerValue;
			if (pointer != null)
				return string.Format ("0x{0:x}", pointer.Address);

			var cx = (SoftEvaluationContext) ctx;
			var sm = obj as StructMirror;
			var om = obj as ObjectMirror;

			if (sm != null && sm.Type.IsPrimitive) {
				// Boxed primitive
				if (sm.Fields.Length > 0 && (sm.Fields[0] is PrimitiveValue))
					return ((PrimitiveValue) sm.Fields[0]).Value.ToString ();
			} else if (om != null && cx.Options.AllowTargetInvoke) {
				var method = OverloadResolve (cx, om.Type, "ToString", null, new ArgumentType[0], true, false, false);
				if (method != null && method.DeclaringType.FullName != "System.Object")
					return InvokeToString (cx, method, obj);
			} else if (sm != null && cx.Options.AllowTargetInvoke) {
				var method = OverloadResolve (cx, sm.Type, "ToString", null, new ArgumentType [0], true, false, false);
				if (method != null && method.DeclaringType.FullName != "System.ValueType")
					return InvokeToString (cx, method, obj);
			}
			
			return GetDisplayTypeName (GetValueTypeName (ctx, obj));
		}

		public override object TryConvert (EvaluationContext ctx, object obj, object targetType)
		{
			var res = TryCast (ctx, obj, targetType);
			
			if (res != null || obj == null)
				return res;
			
			var otype = GetValueType (ctx, obj) as Type;

			if (otype != null) {
				var tm = targetType as TypeMirror;
				if (tm != null)
					targetType = Type.GetType (tm.FullName, false);
				
				var tt = targetType as Type;
				if (tt != null) {
					try {
						var primitive = obj as PrimitiveValue;
						if (primitive != null)
							obj = primitive.Value;

						res = System.Convert.ChangeType (obj, tt);
						return CreateValue (ctx, res);
					} catch {
						return null;
					}
				}
			}

			return null;
		}

		static readonly Dictionary<string, TypeCastDelegate> typeCastDelegatesCache = new Dictionary<string, TypeCastDelegate> ();

		static TypeCastDelegate GenerateTypeCastDelegate (string methodName, Type fromType, Type toType)
		{
			lock(typeCastDelegatesCache) {
				if (typeCastDelegatesCache.TryGetValue (methodName, out var cached))
					return cached;

				var argTypes = new [] { typeof (object) };
				var method = new DynamicMethod (methodName, typeof (object), argTypes, true);
				var il = method.GetILGenerator ();
				ConstructorInfo ctorInfo;
				System.Reflection.MethodInfo methodInfo;

				il.Emit (OpCodes.Ldarg_0);
				il.Emit (OpCodes.Unbox_Any, fromType);

				if (fromType.IsSubclassOf (typeof (Nullable))) {
					var propInfo = fromType.GetProperty ("Value");
					methodInfo = propInfo.GetGetMethod ();

					il.Emit (OpCodes.Stloc_0);
					il.Emit (OpCodes.Ldloca_S);
					il.Emit (OpCodes.Call, methodInfo);

					fromType = methodInfo.ReturnType;
				}

				if (!convertOps.TryGetValue (toType, out var conv)) {
					argTypes = new [] { fromType };

					if (toType == typeof (string)) {
						methodInfo = fromType.GetMethod ("ToString", new Type [0]);
						il.Emit (OpCodes.Call, methodInfo);
					} else if ((methodInfo = toType.GetMethod ("op_Explicit", argTypes)) != null) {
						il.Emit (OpCodes.Call, methodInfo);
					} else if ((methodInfo = toType.GetMethod ("op_Implicit", argTypes)) != null) {
						il.Emit (OpCodes.Call, methodInfo);
					} else if ((ctorInfo = toType.GetConstructor (argTypes)) != null) {
						il.Emit (OpCodes.Call, ctorInfo);
					} else {
						// No idea what else to try...
						throw new InvalidCastException ();
					}
				} else {
					il.Emit (conv);
				}

				il.Emit (OpCodes.Box, toType);
				il.Emit (OpCodes.Ret);
				cached = (TypeCastDelegate)method.CreateDelegate (typeof (TypeCastDelegate));
				typeCastDelegatesCache [methodName] = cached;

				return cached;
			}
		}

		static object DynamicCast (object value, Type target)
		{
			var methodName = string.Format ("CastFrom{0}To{1}", value.GetType ().Name, target.Name);
			var method = GenerateTypeCastDelegate (methodName, value.GetType (), target);

			return method.Invoke (value);
		}

		static bool CanForceCast (EvaluationContext ctx, ArgumentType fromType, TypeMirror toType)
		{
			var cx = (SoftEvaluationContext) ctx;
			MethodMirror method;

			if (CanCast (ctx, fromType, toType))
				return true;

			// check for explicit cast operators in the target type
			method = OverloadResolve (cx, toType, "op_Explicit", null, new [] { fromType }, false, true, false, false);
			if (method != null)
				return true;

			// check for explicit cast operators on the source type
			method = OverloadResolve (cx, fromType.Type, "op_Explicit", null, toType, new [] { fromType }, false, true, false, false);
			if (method != null)
				return true;

			method = OverloadResolve (cx, toType, ".ctor", null, new [] { fromType }, true, false, false, false);
			if (method != null)
				return true;

			return false;
		}

		static bool CanCast (EvaluationContext ctx, ArgumentType fromType, TypeMirror toType)
		{
			var cx = (SoftEvaluationContext)ctx;
			MethodMirror method;

			// check for implicit cast operators in the target type
			method = OverloadResolve (cx, toType, "op_Implicit", null, new [] { fromType }, false, true, false, false);
			if (method != null)
				return true;

			// check for implicit cast operators on the source type
			method = OverloadResolve (cx, fromType.Type, "op_Implicit", null, toType, new [] { fromType }, false, true, false, false);
			if (method != null)
				return true;

			return false;
		}

		object TryForceCast (EvaluationContext ctx, Value value, TypeMirror fromType, TypeMirror toType)
		{
			var cx = (SoftEvaluationContext) ctx;
			MethodMirror method;

			// check for explicit and implicit cast operators in the target type
			method = OverloadResolve (cx, toType, "op_Explicit", null, new [] { new ArgumentType { Type = fromType } }, false, true, false, false);
			if (method != null)
				return cx.RuntimeInvoke (method, toType, new [] { value });

			method = OverloadResolve (cx, toType, "op_Implicit", null, new [] { new ArgumentType { Type = fromType } }, false, true, false, false);
			if (method != null)
				return cx.RuntimeInvoke (method, toType, new [] { value });

			// check for explicit and implicit cast operators on the source type
			method = OverloadResolve (cx, fromType, "op_Explicit", null, toType, new [] { new ArgumentType { Type = fromType } }, false, true, false, false);
			if (method != null)
				return cx.RuntimeInvoke (method, fromType, new [] { value });

			method = OverloadResolve (cx, fromType, "op_Implicit", null, toType, new [] { new ArgumentType { Type = fromType } }, false, true, false, false);
			if (method != null)
				return cx.RuntimeInvoke (method, fromType, new [] { value });

			// Finally, try a ctor...
			try {
				return CreateValue (ctx, toType, value);
			} catch {
				return null;
			}
		}

		public override object TryCast (EvaluationContext ctx, object val, object type)
		{
			var cx = (SoftEvaluationContext) ctx;
			var toType = type as TypeMirror;
			TypeMirror fromType;

			if (val == null)
				return null;

			if (val is DelayedLambdaValue)
				return null;

			var valueType = GetValueType (ctx, val);

			fromType = valueType as TypeMirror;

			// If we are trying to cast into non-primitive/enum value(e.g. System.nint)
			// that class might have implicit operator and this must be handled via TypeMirrors
			if (toType != null && !toType.IsPrimitive  && !toType.IsEnum)
				fromType = ToTypeMirror (ctx, valueType);

			if (fromType != null) {
				// Try casting the primitive type of the enum
				var em = val as EnumMirror;
				if (em != null)
					return TryCast (ctx, CreateValue (ctx, em.Value), type);

				if (toType != null && toType.IsAssignableFrom(fromType))
					return val;

				if (toType == null)
					return null;

				MethodMirror method;

				if (fromType.IsGenericType && fromType.FullName.StartsWith ("System.Nullable`1", StringComparison.Ordinal)) {
					method = OverloadResolve (cx, fromType, "get_Value", null, new ArgumentType[0], true, false, false);
					if (method != null) {
						val = cx.RuntimeInvoke (method, val, new Value[0]);
						return TryCast (ctx, val, type);
					}
				}


				if (toType.IsGenericType && toType.FullName.StartsWith ("System.Nullable`1", StringComparison.Ordinal)) 
				{
					var primitiveVal = val as PrimitiveValue;
					if (primitiveVal != null && primitiveVal.Value == null) {
						val = CreateValue (ctx, toType, new object [0]);
					} else {
						val = CreateValue (ctx, toType, val);
					}
					return val;
				}

				return TryForceCast (ctx, (Value) val, fromType, toType);
			}

			var vtype = valueType as Type;

			if (vtype != null) {
				if (toType != null) {
					if (toType.IsEnum) {
						var casted = TryCast (ctx, val, toType.EnumUnderlyingType) as PrimitiveValue;

						return casted != null ? cx.Session.VirtualMachine.CreateEnumMirror (toType, casted) : null;
					}

					type = Type.GetType (toType.FullName, false);
				}
				
				var tt = type as Type;
				if (tt != null) {
					if (tt.IsAssignableFrom (vtype))
						return val;

					try {
						if (tt.IsPrimitive || tt == typeof (string)) {
							var primitive = val as PrimitiveValue;
							if (primitive != null)
								val = primitive.Value;

							if (val == null)
								return null;

							return CreateValue (ctx, DynamicCast (val, tt));
						}

						fromType = (TypeMirror) ForceLoadType (ctx, ((Type) valueType).FullName);
						if (toType == null)
							toType = (TypeMirror) ForceLoadType (ctx, tt.FullName);

						return TryForceCast (ctx, (Value) val, fromType, toType);
					} catch {
						return null;
					}
				}
			}

			return null;
		}
		
		public override IStringAdaptor CreateStringAdaptor (EvaluationContext ctx, object str)
		{
			return new StringAdaptor ((StringMirror) str);
		}

		public override ICollectionAdaptor CreateArrayAdaptor (EvaluationContext ctx, object arr)
		{
			return new ArrayAdaptor ((ArrayMirror) arr);
		}

		public override object CreateNullValue (EvaluationContext ctx, object type)
		{
			var cx = (SoftEvaluationContext) ctx;

			return new PrimitiveValue (cx.Session.VirtualMachine, null);
		}

		public override object CreateTypeObject (EvaluationContext ctx, object type)
		{
			return ((TypeMirror) type).GetTypeObject ();
		}

		public override object CreateValue (EvaluationContext ctx, object type, params object[] argValues)
		{
			ctx.AssertTargetInvokeAllowed ();
			
			var cx = (SoftEvaluationContext) ctx;
			var tm = (TypeMirror) type;
			
			var types = new ArgumentType [argValues.Length];
			var values = new Value[argValues.Length];

			for (int n = 0; n < argValues.Length; n++) {
				types[n] = ToArgumentType (ctx, GetValueType (ctx, argValues[n]));
			}

			var method = OverloadResolve (cx, tm, ".ctor", null, types, true, false, false);

			if (method != null) {
				var mparams = method.GetParameters ();

				for (int n = 0; n < argValues.Length; n++) {
					var param_type = mparams [n].ParameterType;

					if (param_type.FullName != types [n].Type.FullName && !param_type.IsAssignableFrom (types [n].Type) && param_type.IsGenericType) {
						/* TODO: Add genericTypeArgs and handle this
						bool throwCastException = true;

						if (method.VirtualMachine.Version.AtLeast (2, 15)) {
							var args = param_type.GetGenericArguments ();

							if (args.Length == genericTypes.Length) {
								var real_type = soft.Adapter.GetType (soft, param_type.GetGenericTypeDefinition ().FullName, genericTypes);

								values [n] = (Value)TryCast (soft, (Value)argValues [n], real_type);
								if (!(values [n] == null && argValues [n] != null && !soft.Adapter.IsNull (soft, argValues [n])))
									throwCastException = false;
							}
						}

						if (throwCastException) {
							string fromType = !IsGeneratedType (types [n]) ? soft.Adapter.GetDisplayTypeName (soft, types [n]) : types [n].FullName;
							string toType = soft.Adapter.GetDisplayTypeName (soft, param_type);

							throw new EvaluatorException ("Argument {0}: Cannot implicitly convert `{1}' to `{2}'", n, fromType, toType);
						}*/
					} else if (param_type.FullName != types [n].Type.FullName && !param_type.IsAssignableFrom (types [n].Type) && CanForceCast (ctx, types [n], param_type)) {
						values [n] = (Value)TryCast (ctx, argValues [n], param_type);
					} else if (param_type.FullName != types [n].Type.FullName && !param_type.IsAssignableFrom (types [n].Type) && CanDoPrimaryCast (types [n], param_type)) {
						values [n] = (Value)TryConvert (ctx, argValues [n], param_type);
					} else {
						values [n] = (Value)argValues [n];
					}
				}

				lock(method.VirtualMachine) {
					return tm.NewInstance (cx.Thread, method, values);
				}
			}

			if (argValues.Length == 0 && tm.VirtualMachine.Version.AtLeast (2, 31))
				return tm.NewInstance ();

			var typeName = ctx.Adapter.GetDisplayTypeName (ctx, type);

			throw new EvaluatorException ("Constructor not found for type `{0}'.", typeName);
		}

		public override object CreateValue (EvaluationContext ctx, object value)
		{
			var cx = (SoftEvaluationContext) ctx;
			var str = value as string;
			if (str != null)
				return cx.Domain.CreateString (str);

			if (value is decimal) {
				var bits = decimal.GetBits ((decimal)value);

				return CreateValue (ctx, ToTypeMirror (ctx, typeof (decimal)), CreateValue (ctx, bits [0]), CreateValue (ctx, bits [1]), CreateValue (ctx, bits [2]), CreateValue (ctx, (bits [3] & unchecked((int)0x80000000)) != 0), CreateValue (ctx, (byte)(bits [3] >> 16)));
			}

			return cx.Session.VirtualMachine.CreateValue (value);
		}

		public object CreateByteArray (EvaluationContext ctx, byte [] bytes)
		{
			var cx = (SoftEvaluationContext) ctx;

			if (Session.VirtualMachine.Version.AtLeast (2, 52))
				return cx.Domain.CreateByteArray (bytes);

			var arrayType = GetType (ctx, "System.Array");
			var int32Type = GetType (ctx, "System.Int32");
			var typeType = GetType (ctx, "System.Type");
			var stringType = GetType (ctx, "System.String");
			var byteTypeValue = RuntimeInvoke (ctx, typeType, null, "GetType", new object [] { stringType }, new object [] { CreateValue (ctx, "System.Byte") });
			//var byteType = ctx.Adapter.GetType (ctx, "System.Byte");
			var n = CreateValue (ctx, bytes.Length);
			var args = new object [] { byteTypeValue, n };
			var arr = RuntimeInvoke (ctx, arrayType, null, "CreateInstance", new object [] { typeType, int32Type }, args);
			if (arr is ArrayMirror) {
				var arrm = arr as ArrayMirror;
				arrm.SetByteValues (bytes);
				return arr;
			}

			return null;
		}

		public override object GetBaseValue (EvaluationContext ctx, object val)
		{
			return val;
		}

		public override bool NullableHasValue (EvaluationContext ctx, object type, object obj)
		{
			var hasValue = GetMember (ctx, type, obj, "hasValue") ?? GetMember (ctx, type, obj, "has_value");

			return (bool) hasValue.ObjectValue;
		}

		public override ValueReference NullableGetValue (EvaluationContext ctx, object type, object obj)
		{
			return GetMember (ctx, type, obj, "value");
		}

		public override object GetEnclosingType (EvaluationContext ctx)
		{
			return ((SoftEvaluationContext) ctx).Frame.Method.DeclaringType;
		}

		public override string[] GetImportedNamespaces (EvaluationContext ctx)
		{
			var namespaces = new HashSet<string> ();
			var cx = (SoftEvaluationContext) ctx;

			foreach (var type in cx.Session.GetAllTypes ())
				namespaces.Add (type.Namespace);
			
			var nss = new string [namespaces.Count];
			namespaces.CopyTo (nss);

			return nss;
		}

		public override ValueReference GetIndexerReference (EvaluationContext ctx, object target, object type, object [] indices)
		{
			var values = new Value [indices.Length];
			var types = new ArgumentType [indices.Length];
			for (int n = 0; n < indices.Length; n++) {
				types[n] = ToArgumentType (ctx, GetValueType (ctx, indices[n]));
				values[n] = (Value) indices[n];
			}

			var candidates = new List<MethodMirror> ();
			var props = new List<PropertyInfoMirror> ();
			var mirType = type as TypeMirror;
			while (mirType != null) {
				foreach (var prop in mirType.GetProperties ()) {
					var met = prop.GetGetMethod (true);
					if (met != null &&
						!met.IsStatic &&
						met.GetParameters ().Length > 0 &&
						!(met.IsPrivate && met.IsVirtual)) {//Don't use explicit interface implementation
						candidates.Add (met);
						props.Add (prop);
					}
				}
				mirType = mirType.BaseType;
			}

			var idx = OverloadResolve ((SoftEvaluationContext) ctx, mirType, null, null, null, types, candidates, true);
			int i = candidates.IndexOf (idx);

			var getter = props[i].GetGetMethod (true);

			return getter != null ? new PropertyValueReference (ctx, props[i], target, null, getter, values) : null;
		}

		public override ValueReference GetIndexerReference (EvaluationContext ctx, object target, object[] indices)
		{
			object valueType = GetValueType (ctx, target);
			var targetType = valueType as TypeMirror;

			if (targetType == null) {
				var tt = valueType as Type;

				if (tt == null)
					return null;

				targetType = (TypeMirror) ForceLoadType (ctx, tt.FullName);
			}

			return GetIndexerReference (ctx, target, targetType, indices);
		}
		
		static bool InGeneratedClosureOrIteratorType (EvaluationContext ctx)
		{
			var cx = (SoftEvaluationContext) ctx;

			if (cx.Frame.Method.IsStatic)
				return false;

			var tm = cx.Frame.Method.DeclaringType;

			return IsGeneratedType (tm);
		}

		static bool IsLocalFunction(EvaluationContext ctx)
		{
			return ((SoftEvaluationContext)ctx).Frame.Method.Name.IndexOf (">g__", StringComparison.Ordinal) > 0;
		}
		
		internal static bool IsGeneratedType (TypeMirror tm)
		{
			//
			// This should cover all C# generated special containers
			// - anonymous methods
			// - lambdas
			// - iterators
			// - async methods
			//
			// which allow stepping into
			//

			// Note: mcs uses the form <${NAME}>c__${KIND}${NUMBER} where the leading '<' seems to have been dropped in 3.4.x
			//       csc uses the form <${NAME}>d__${NUMBER}
			//		 roslyn uses the form <${NAME}>d

			return tm.Name.IndexOf (">c__", StringComparison.Ordinal) > 0 || tm.Name.IndexOf (">d", StringComparison.Ordinal) > 0;
		}

		internal static string GetNameFromGeneratedType (TypeMirror tm)
		{
			return tm.Name.Substring (1, tm.Name.IndexOf ('>') - 1);
		}
		
		static bool IsHoistedThisReference (FieldInfoMirror field)
		{
			// mcs is "<>f__this" or "$this" (if in an async compiler generated type)
			// csc is "<>4__this"
			return field.Name == "$this" ||
				(field.Name.StartsWith ("<>", StringComparison.Ordinal) &&
				 field.Name.EndsWith ("__this", StringComparison.Ordinal));
		}
		
		static bool IsClosureReferenceField (FieldInfoMirror field)
		{
			// mcs is "$locvar"
			// old mcs is "<>f__ref"
			// csc is "CS$<>"
			// roslyn is "<>8__"
			return field.Name.StartsWith ("CS$<>", StringComparison.Ordinal) ||
				        field.Name.StartsWith ("<>f__ref", StringComparison.Ordinal) ||
				        field.Name.StartsWith ("$locvar", StringComparison.Ordinal) ||
				        field.Name.StartsWith ("<>8__", StringComparison.Ordinal);
		}
		
		static bool IsClosureReferenceLocal (LocalVariable local)
		{
			if (local.Name == null)
				return false;

			// mcs is "$locvar" or starts with '<'
			// csc is "CS$<>"
			return local.Name.Length == 0 || local.Name[0] == '<' || local.Name.StartsWith ("$locvar", StringComparison.Ordinal) ||
				local.Name.StartsWith ("CS$<>", StringComparison.Ordinal);
		}
		
		static bool IsGeneratedTemporaryLocal (LocalVariable local)
		{
			// csc uses CS$ prefix for temporary variables and <>t__ prefix for async task-related state variables
			return local.Name != null && (local.Name.StartsWith ("CS$", StringComparison.Ordinal) || local.Name.StartsWith ("<>t__", StringComparison.Ordinal));
		}

		Dictionary<MethodMirror, PortablePdbData.SoftScope []> methodScopeCache = new Dictionary<MethodMirror, PortablePdbData.SoftScope []> ();

		string GetHoistedIteratorLocalName (FieldInfoMirror field, SoftEvaluationContext cx)
		{
			//mcs captured args, of form <$>name
			if (field.Name.StartsWith ("<$>", StringComparison.Ordinal)) {
				return field.Name.Substring (3);
			}

			// csc, mcs locals of form <name>__#, where # represents index of scope
			// roslyn locals of form <name>5__#, where # represents index of scope
			if (field.Name [0] == '<') {
				int suffixLength = 3;
				var i = field.Name.IndexOf (">__", StringComparison.Ordinal);
				if (i == -1) {
					suffixLength = 4;
					i = field.Name.IndexOf (">5__", StringComparison.Ordinal);
				}

				if (i != -1 && field.VirtualMachine.Version.AtLeast (2, 43)) {
					if (int.TryParse (field.Name.Substring (i + suffixLength), out int scopeIndex) && scopeIndex > 0) {//0 means whole method scope
						PortablePdbData.SoftScope [] scopes = null;

						scopeIndex--;//Scope index is 1 based(not zero)

						if (cx.Frame.Method != null && !methodScopeCache.TryGetValue (cx.Frame.Method, out scopes)) {
							scopes = cx.Session.GetPdbData (cx.Frame.Method)?.GetHoistedScopes (cx.Frame.Method);
							if (scopes == null || scopes.Length == 0) {
								// If hoisted scopes are empty use normal scopes
								scopes = cx.Frame.Method.GetScopes ().Select (s => new PortablePdbData.SoftScope { LiveRangeStart = s.LiveRangeStart, LiveRangeEnd = s.LiveRangeEnd }).ToArray ();
								DebuggerLoggingService.LogMessage ("PDB data not found for frame: {0}", cx.Frame);
							}

							methodScopeCache [cx.Frame.Method] = scopes;
						}

						if (scopes != null && scopeIndex < scopes.Length) {
							var scope = scopes [scopeIndex];
							if (scope.LiveRangeStart > cx.Frame.Location.ILOffset || scope.LiveRangeEnd < cx.Frame.Location.ILOffset)
								return null;
						}
					}
				}

				i = field.Name.IndexOf ('>');
				if (i > 1) {
					return field.Name.Substring (1, i - 1);
				}
			}

			return null;
		}

		IEnumerable<ValueReference> GetHoistedLocalVariables (SoftEvaluationContext cx, ValueReference vthis, HashSet<FieldInfoMirror> alreadyVisited = null)
		{
			if (vthis == null)
				return new ValueReference [0];

			object val;
			try {
				val = vthis.Value;
			} catch (InvalidStackFrameException) {
 				return new ValueReference [0];
			} catch (AbsentInformationException) {
				return new ValueReference [0];
			} catch (EvaluatorException ex) when (ex.InnerException is AbsentInformationException) {
				return new ValueReference [0];
			}

			if (IsNull (cx, val))
				return new ValueReference [0];
			
			var tm = (TypeMirror) vthis.Type;
			var isIterator = IsGeneratedType (tm);
			
			var list = new List<ValueReference> ();
			var type = (TypeMirror) vthis.Type;

			foreach (var field in type.GetFields ()) {
				if (IsHoistedThisReference (field))
					continue;

				if (IsClosureReferenceField (field)) {
					alreadyVisited = alreadyVisited ?? new HashSet<FieldInfoMirror> ();
					if (alreadyVisited.Contains (field))
						continue;
					alreadyVisited.Add (field);
					list.AddRange (GetHoistedLocalVariables (cx, new FieldValueReference (cx, field, val, type), alreadyVisited));
					continue;
				}

				if (field.Name[0] == '<') {
					if (isIterator) {
						var name = GetHoistedIteratorLocalName (field, cx);

						if (!string.IsNullOrEmpty (name))
							list.Add (new FieldValueReference (cx, field, val, type, name, ObjectValueFlags.Variable) {
								ParentSource = vthis
							});
					}
				} else if (!field.Name.Contains ("$")) {
					list.Add (new FieldValueReference (cx, field, val, type, field.Name, ObjectValueFlags.Variable) {
						ParentSource = vthis
					});
				}
			}

			return list;
		}
		
		ValueReference GetHoistedThisReference (SoftEvaluationContext cx)
		{
			try {
				var val = cx.Frame.GetThis ();
				var valueType = GetValueType (cx, val);
				var type = valueType as TypeMirror;

				if (type == null) {
					var tt = valueType as Type;

					if (tt == null)
						return null;

					type = (TypeMirror) ForceLoadType (cx, tt.FullName);
				}

				return GetHoistedThisReference (cx, type, val);
			} catch (InvalidStackFrameException) {
			} catch (AbsentInformationException) {
			}
			return null;
		}
		
		ValueReference GetHoistedThisReference (SoftEvaluationContext cx, TypeMirror type, object val, HashSet<FieldInfoMirror> alreadyVisited = null)
		{
			foreach (var field in type.GetFields ()) {
				if (IsHoistedThisReference (field))
					return new FieldValueReference (cx, field, val, type, "this", ObjectValueFlags.Literal);

				if (IsClosureReferenceField (field)) {
					alreadyVisited = alreadyVisited ?? new HashSet<FieldInfoMirror> ();
					if (alreadyVisited.Contains (field))
						continue;
					alreadyVisited.Add (field);
					var fieldRef = new FieldValueReference (cx, field, val, type);
					var thisRef = GetHoistedThisReference (cx, field.FieldType, fieldRef.Value, alreadyVisited);
					if (thisRef != null)
						return thisRef;
				}
			}

			return null;
		}
		
		// if the local does not have a name, constructs one from the index
		static string GetLocalName (SoftEvaluationContext cx, LocalVariable local)
		{
			if (!string.IsNullOrEmpty (local.Name) || cx.SourceCodeAvailable)
				return local.Name;

			return "loc" + local.Index;
		}
		
		protected override ValueReference OnGetLocalVariable (EvaluationContext ctx, string name)
		{
			var cx = (SoftEvaluationContext) ctx;

			if (InGeneratedClosureOrIteratorType (cx) || IsLocalFunction(cx))
				return FindByName (OnGetLocalVariables (cx), v => v.Name, name, ctx.CaseSensitive);

			try {
				LocalVariable local = null;

				if (!cx.SourceCodeAvailable) {
					if (name.StartsWith ("loc", StringComparison.Ordinal)) {
						int idx;

						if (int.TryParse (name.Substring (3), out idx))
							local = cx.Frame.Method.GetLocals ().FirstOrDefault (loc => loc.Index == idx);
					}
				} else {
					local = ctx.CaseSensitive
						? cx.Frame.GetVisibleVariableByName (name)
						: FindByName (cx.Frame.GetVisibleVariables (), v => v.Name, name, false);
				}

				if (local != null)
					return new VariableValueReference (ctx, GetLocalName (cx, local), local);

				return FindByName (OnGetLocalVariables (ctx), v => v.Name, name, ctx.CaseSensitive);
			} catch (InvalidStackFrameException) {
				return null;
			} catch (AbsentInformationException) {
				return null;
			}
		}

		protected override IEnumerable<ValueReference> OnGetLocalVariables (EvaluationContext ctx)
		{
			var cx = (SoftEvaluationContext) ctx;

			if (InGeneratedClosureOrIteratorType (cx)) {
				var vthis = GetThisReference (cx);
				return GetHoistedLocalVariables (cx, vthis).Union (GetLocalVariables (cx));
			}

			if (IsLocalFunction (cx)) {
				var vthis = GetClosureReference (cx);
				// if there's no closure reference then it didn't capture anything
				if (vthis != null) {
					return GetHoistedLocalVariables (cx, vthis).Union (GetLocalVariables (cx));
				}
			}

			return GetLocalVariables (cx);
		}

		static VariableValueReference GetClosureReference (SoftEvaluationContext cx)
		{
			foreach (var local in cx.Frame.Method.GetLocals ()) {
				if (IsClosureReferenceLocal (local)) {
					return new VariableValueReference (cx, local.Name, local);
				}
			}
			return null;
		}
		
		IEnumerable<ValueReference> GetLocalVariables (SoftEvaluationContext cx)
		{
			LocalVariable[] locals;

			try {
				locals = cx.Frame.GetVisibleVariables ().Where (x => !x.IsArg && ((IsClosureReferenceLocal (x) && IsGeneratedType (x.Type)) || !IsGeneratedTemporaryLocal (x))).ToArray ();
			} catch (InvalidStackFrameException) {
				yield break;
			} catch (AbsentInformationException) {
				yield break;
			}

			if (locals.Length == 0)
				yield break;

			var batch = new LocalVariableBatch (locals);

			for (int i = 0; i < locals.Length; i++) {
				if (IsClosureReferenceLocal (locals[i]) && IsGeneratedType (locals[i].Type)) {
					foreach (var gv in GetHoistedLocalVariables (cx, new VariableValueReference (cx, locals[i].Name, locals[i], batch))) {
						yield return gv;
					}
				} else if (!IsGeneratedTemporaryLocal (locals[i])) {
					yield return new VariableValueReference (cx, GetLocalName (cx, locals[i]), locals[i], batch);
				}
			}
		}

		public override bool HasMember (EvaluationContext ctx, object type, string memberName, BindingFlags bindingFlags)
		{
			var tm = (TypeMirror) type;

			while (tm != null) {
				var field = FindByName (tm.GetFields (), f => f.Name, memberName, ctx.CaseSensitive);

				if (field != null)
					return true;

				var prop = FindByName (tm.GetProperties (), p => p.Name, memberName, ctx.CaseSensitive);

				if (prop != null) {
					var getter = prop.GetGetMethod (bindingFlags.HasFlag (BindingFlags.NonPublic));
					if (getter != null)
						return true;
				}

				if (bindingFlags.HasFlag (BindingFlags.DeclaredOnly))
					break;

				tm = tm.BaseType;
			}

			return false;
		}

		static bool IsAnonymousType (TypeMirror type)
		{
			return type.Name.StartsWith ("<>__AnonType", StringComparison.Ordinal);
		}

		protected override ValueReference GetMember (EvaluationContext ctx, object t, object co, string name)
		{
			return OnGetMember (ctx, null, t, co, name);
		}

		protected override ValueReference OnGetMember (EvaluationContext ctx, IObjectSource objectSource, object t, object co, string name)
		{
			var type = t as TypeMirror;
			var tupleNames = GetTupleElementNames (objectSource, ctx);
			while (type != null) {
				var field = FindByName (type.GetFields (), f => MapTupleName(f.Name, tupleNames), name, ctx.CaseSensitive);

				if (field != null && (field.IsStatic || co != null))
					return new FieldValueReference (ctx, field, co, type);

				var prop = FindByName (type.GetProperties (), p => p.Name, name, ctx.CaseSensitive);

				if (prop != null && (IsStatic (prop) || co != null)) {
					var getter = prop.GetGetMethod (true);
					// Optimization: if the property has a CompilerGenerated backing field, use that instead.
					// This way we avoid overhead of invoking methods on the debugee when the value is requested.
					//But also check that this method is not virtual, because in that case we need to call getter to invoke override
					if (!getter.IsVirtual) {
						var cgFieldName = string.Format ("<{0}>{1}", prop.Name, IsAnonymousType (type) ? "" : "k__BackingField");
						if ((field = FindByName (type.GetFields (), f => f.Name, cgFieldName, true)) != null && IsCompilerGenerated (field))
							return new FieldValueReference (ctx, field, co, type, prop.Name, ObjectValueFlags.Property);
					}
					// Backing field not available, so do things the old fashioned way.
					return getter != null ? new PropertyValueReference (ctx, prop, co, type, getter, null) : null;
				}
				if (type.IsInterface) {
					foreach (var inteface in type.GetInterfaces ()) {
						var result = GetMember (ctx, inteface, co, name);
						if (result != null)
							return result;
					}
					//foreach above recursively checked all "base" interfaces
					//nothing was found, quit, otherwise we would loop forever
					return null;
				}

				type = type.BaseType;
			}

			return null;
		}

		static string MapTupleName (string name, string [] tupleNames)
		{
			if (tupleNames != null &&
				name.Length > 4 &&
				name.StartsWith ("Item", StringComparison.Ordinal) &&
				int.TryParse (name.Substring (4), out var tupleIndex) &&
				tupleNames.Length >= tupleIndex &&
				tupleNames [tupleIndex - 1] != null)
				return tupleNames [tupleIndex - 1];

			return name;
		}

		static bool IsCompilerGenerated (FieldInfoMirror field)
		{
			var attrs = field.GetCustomAttributes (true);
			var generated = GetAttribute<CompilerGeneratedAttribute> (attrs);

			return generated != null;
		}

		static bool IsStatic (PropertyInfoMirror prop)
		{
			var met = prop.GetGetMethod (true) ?? prop.GetSetMethod (true);

			return met.IsStatic;
		}

		static T FindByName<T> (IEnumerable<T> items, Func<T,string> getName, string name, bool caseSensitive)
		{
			var best = default(T);

			foreach (var item in items) {
				var itemName = getName (item);

				if (itemName == name)
					return item;

				if (!caseSensitive && itemName.Equals (name, StringComparison.CurrentCultureIgnoreCase))
					best = item;
			}

			return best;
		}

		protected override IEnumerable<ValueReference> GetMembers (EvaluationContext ctx, object t, object co, BindingFlags bindingFlags)
		{
			return GetMembers (ctx, null, t, co, bindingFlags);
		}

		string[] GetTupleElementNames(IObjectSource source, EvaluationContext ctx)
		{
			if (source is FieldValueReference)
			{
				FieldValueReference field = source as FieldValueReference;
				if ((field.Type as TypeMirror)?.Name?.StartsWith("ValueTuple`", StringComparison.Ordinal) != true)
					return null;
				return field.GetTupleElementNames();
			}
			if (source is PropertyValueReference)
			{
				PropertyValueReference prop = source as PropertyValueReference;
				if ((prop.Type as TypeMirror)?.Name?.StartsWith("ValueTuple`", StringComparison.Ordinal) != true)
					return null;
				return prop.GetTupleElementNames();
			}
			if (source is VariableValueReference)
			{
				VariableValueReference variable = source as VariableValueReference;
				if ((variable.Type as TypeMirror)?.Name?.StartsWith("ValueTuple`", StringComparison.Ordinal) != true)
					return null;
				return variable.GetTupleElementNames((SoftEvaluationContext)ctx);
			}
			return null;

		}

		protected override IEnumerable<ValueReference> GetMembers (EvaluationContext ctx, IObjectSource objectSource, object t, object co, BindingFlags bindingFlags)
		{
			var subProps = new Dictionary<string, PropertyInfoMirror> ();
			var type = t as TypeMirror;
			TypeMirror realType = null;

			if (co != null && (bindingFlags & BindingFlags.Instance) != 0)
				realType = GetValueType (ctx, co) as TypeMirror;

			// First of all, get a list of properties overriden in sub-types
			while (realType != null && realType != type) {
				foreach (var prop in realType.GetProperties (bindingFlags | BindingFlags.DeclaredOnly)) {
					var met = prop.GetGetMethod (true);

					if (met == null || met.GetParameters ().Length != 0 || met.IsAbstract || !met.IsVirtual || met.IsStatic)
						continue;

					if (met.IsPublic && ((bindingFlags & BindingFlags.Public) == 0))
						continue;

					if (!met.IsPublic && ((bindingFlags & BindingFlags.NonPublic) == 0))
						continue;

					subProps [prop.Name] = prop;
				}

				realType = realType.BaseType;
			}

			var tupleNames = GetTupleElementNames (objectSource, ctx);
			bool hasExplicitInterface = false;
			while (type != null) {
				var fieldsBatch = new FieldReferenceBatch (co);
				foreach (var field in type.GetFields ()) {
					if (field.IsStatic && ((bindingFlags & BindingFlags.Static) == 0))
						continue;

					if (!field.IsStatic && ((bindingFlags & BindingFlags.Instance) == 0))
						continue;

					if (field.IsPublic && ((bindingFlags & BindingFlags.Public) == 0))
						continue;

					if (!field.IsPublic && ((bindingFlags & BindingFlags.NonPublic) == 0))
						continue;
					if (field.IsStatic) {
						yield return new FieldValueReference (ctx, field, co, type);
					} else {
						fieldsBatch.Add (field);
						yield return new FieldValueReference (ctx, field, co, type, MapTupleName (field.Name, tupleNames), ObjectValueFlags.Field, fieldsBatch);
					}
				}

				foreach (var prop in type.GetProperties (bindingFlags)) {
					var getter = prop.GetGetMethod (true);

					if (getter == null || getter.GetParameters ().Length != 0 || getter.IsAbstract)
						continue;

					if (getter.IsStatic && ((bindingFlags & BindingFlags.Static) == 0))
						continue;

					if (!getter.IsStatic && ((bindingFlags & BindingFlags.Instance) == 0))
						continue;
					
					if (getter.IsPublic && ((bindingFlags & BindingFlags.Public) == 0))
						continue;

					//This is only possible in case of explicitly implemented interface property, which we handle later
					if (getter.IsVirtual && getter.IsPrivate) {
						hasExplicitInterface = true;
						continue;
					}

					if (!getter.IsPublic && ((bindingFlags & BindingFlags.NonPublic) == 0))
						continue;

					// If a property is overriden, return the override instead of the base property
					PropertyInfoMirror overridden;
					if (getter.IsVirtual && subProps.TryGetValue (prop.Name, out overridden)) {
						getter = overridden.GetGetMethod (true);
						if (getter == null)
							continue;
						
						yield return new PropertyValueReference (ctx, overridden, co, overridden.DeclaringType, getter, null);
					} else {
						yield return new PropertyValueReference (ctx, prop, co, type, getter, null);
					}
				}

				if ((bindingFlags & BindingFlags.DeclaredOnly) != 0)
					break;

				type = type.BaseType;
			}

			type = t as TypeMirror;
			if (type == null ||
				!hasExplicitInterface ||
				(bindingFlags & BindingFlags.Instance) == 0 ||
				(bindingFlags & BindingFlags.Public) == 0) {
				yield break;
			}
			if (Session.VirtualMachine.Version.AtLeast (2, 11)) {
				var interfaces = type.GetInterfaces ();
				foreach (var intr in interfaces) {
					var map = type.GetInterfaceMap (intr);
					foreach (var prop in intr.GetProperties (bindingFlags)) {
						var getter = prop.GetGetMethod (true);
						if (getter == null || getter.GetParameters ().Length != 0)
							continue;
						var implementationGetter = map.TargetMethods [Array.IndexOf (map.InterfaceMethods, getter)];
						//We are only intersted into private(explicit) implementations because public ones are already handled before
						if (implementationGetter.IsPublic)
							continue;
						yield return new PropertyValueReference (ctx, prop, co, type, getter, null);
					}
				}
			}
		}

		static bool IsIEnumerable (TypeMirror type)
		{
			if (!type.IsInterface)
				return false;

			if (type.Namespace == "System.Collections" && type.Name == "IEnumerable")
				return true;

			if (type.Namespace == "System.Collections.Generic" && type.Name == "IEnumerable`1")
				return true;

			return false;
		}

		static ObjectValueFlags GetFlags (MethodMirror method)
		{
			var flags = ObjectValueFlags.Method;

			if (method.IsStatic)
				flags |= ObjectValueFlags.Global;

			if (method.IsPublic)
				flags |= ObjectValueFlags.Public;
			else if (method.IsPrivate)
				flags |= ObjectValueFlags.Private;
			else if (method.IsFamily)
				flags |= ObjectValueFlags.Protected;
			else if (method.IsFamilyAndAssembly)
				flags |= ObjectValueFlags.Internal;
			else if (method.IsFamilyOrAssembly)
				flags |= ObjectValueFlags.InternalProtected;

			return flags;
		}

		protected override CompletionData GetMemberCompletionData (EvaluationContext ctx, ValueReference vr)
		{
			var properties = new HashSet<string> ();
			var methods = new HashSet<string> ();
			var fields = new HashSet<string> ();
			var data = new CompletionData ();
			var type = vr.Type as TypeMirror;
			bool isEnumerable = false;

			while (type != null) {
				if (!isEnumerable && IsIEnumerable (type))
					isEnumerable = true;

				bool isExternal = Session.IsExternalCode (type);

				foreach (var field in type.GetFields ()) {
					if (field.IsStatic || field.IsSpecialName || (isExternal && !field.IsPublic) ||
						IsClosureReferenceField (field) || IsCompilerGenerated (field))
						continue;

					if (fields.Add (field.Name))
						data.Items.Add (new CompletionItem (field.Name, FieldValueReference.GetFlags (field)));
				}

				foreach (var property in type.GetProperties ()) {
					var getter = property.GetGetMethod (true);

					if (getter == null || getter.IsStatic || (isExternal && !getter.IsPublic))
						continue;

					if (properties.Add (property.Name))
						data.Items.Add (new CompletionItem (property.Name, PropertyValueReference.GetFlags (property, getter)));
				}

				foreach (var method in type.GetMethods ()) {
					if (method.IsStatic || method.IsConstructor || method.IsSpecialName || (isExternal && !method.IsPublic))
						continue;

					if (methods.Add (method.Name))
						data.Items.Add (new CompletionItem (method.Name, GetFlags (method)));
				}

				if (type.BaseType == null && type.FullName != "System.Object")
					type = ctx.Adapter.GetType (ctx, "System.Object") as TypeMirror;
				else
					type = type.BaseType;
			}

			type = (TypeMirror) vr.Type;
			foreach (var iface in type.GetInterfaces ()) {
				if (!isEnumerable && IsIEnumerable (iface))
					isEnumerable = true;
			}

			if (isEnumerable) {
				// Look for LINQ extension methods...
				var linq = ctx.Adapter.GetType (ctx, "System.Linq.Enumerable") as TypeMirror;
				if (linq != null) {
					foreach (var method in linq.GetMethods ()) {
						if (!method.IsStatic || method.IsConstructor || method.IsSpecialName || !method.IsPublic)
							continue;

						if (methods.Add (method.Name))
							data.Items.Add (new CompletionItem (method.Name, ObjectValueFlags.Method | ObjectValueFlags.Public));
					}
				}
			}

			data.ExpressionLength = 0;

			return data;
		}
		
		public override void GetNamespaceContents (EvaluationContext ctx, string namspace, out string[] childNamespaces, out string[] childTypes)
		{
			var soft = (SoftEvaluationContext) ctx;
			var types = new HashSet<string> ();
			var namespaces = new HashSet<string> ();
			var namspacePrefix = namspace.Length > 0 ? namspace + "." : "";

			foreach (var type in soft.Session.GetAllTypes ()) {
				if (type.Namespace == namspace || type.Namespace.StartsWith (namspacePrefix, StringComparison.InvariantCulture)) {
					namespaces.Add (type.Namespace);
					types.Add (type.FullName);
				}
			}

			childNamespaces = new string [namespaces.Count];
			namespaces.CopyTo (childNamespaces);
			
			childTypes = new string [types.Count];
			types.CopyTo (childTypes);
		}

		protected override ObjectValue CreateObjectValueImpl (EvaluationContext ctx, IObjectValueSource source, ObjectPath path, object obj, ObjectValueFlags flags)
		{
			try {
				return base.CreateObjectValueImpl (ctx, source, path, obj, flags);
			}
			catch (NotSupportedException e) {
				throw new EvaluatorException ("Evaluation failed: {0}", e.Message);
			}
		}

		protected override IEnumerable<ValueReference> OnGetParameters (EvaluationContext ctx)
		{
			var soft = (SoftEvaluationContext) ctx;
			LocalVariable[] locals;

			try {
				locals = soft.Frame.Method.GetLocals ().Where (x => x.IsArg && !IsClosureReferenceLocal (x)).ToArray ();
			} catch (InvalidStackFrameException) {
				yield break;
			} catch (AbsentInformationException) {
				yield break;
			}

			if (locals.Length == 0)
				yield break;

			var batch = new LocalVariableBatch (locals);
				
			for (int i = 0; i < locals.Length; i++) {
				var name = !string.IsNullOrEmpty (locals[i].Name) ? locals[i].Name : "arg" + locals[i].Index;
				yield return new VariableValueReference (ctx, name, locals[i], batch);
			}
		}

		protected override ValueReference OnGetThisReference (EvaluationContext ctx)
		{
			var cx = (SoftEvaluationContext) ctx;

			if (InGeneratedClosureOrIteratorType (cx))
				return GetHoistedThisReference (cx);

			return GetThisReference (cx);
		}
		
		static ValueReference GetThisReference (SoftEvaluationContext ctx)
		{
			return ctx.Frame.Method.IsStatic ? null : new ThisValueReference (ctx, ctx.Frame);
		}
		
		public override ValueReference GetCurrentException (EvaluationContext ctx)
		{
			try {
				var cx = (SoftEvaluationContext) ctx;
				var exc = cx.Session.GetExceptionObject (cx.Thread);

				return exc != null ? LiteralValueReference.CreateTargetObjectLiteral (ctx, ctx.Options.CurrentExceptionTag, exc) : null;
			} catch (AbsentInformationException) {
				return null;
			}
		}

		public override bool IsGenericType (EvaluationContext ctx, object type)
		{
			return type != null && ((TypeMirror) type).IsGenericType;
		}

		public override IEnumerable<object> GetGenericTypeArguments (EvaluationContext ctx, object type)
		{
			return ((TypeMirror)type).GetGenericArguments ();
		}

		public override object[] GetTypeArgs (EvaluationContext ctx, object type)
		{
			var tm = (TypeMirror) type;

			if (tm.VirtualMachine.Version.AtLeast (2, 15))
				return tm.GetGenericArguments ();

			// fall back to parsing them from the from the FullName
			var names = new List<string> ();
			var typeName = tm.FullName;
			int i = typeName.IndexOf ('`');

			if (i != -1) {
				i = typeName.IndexOf ('[', i);
				if (i == -1)
					return new object [0];
				int si = ++i;
				int nt = 0;
				for (; i < typeName.Length && (nt > 0 || typeName[i] != ']'); i++) {
					if (typeName[i] == '[')
						nt++;
					else if (typeName[i] == ']')
						nt--;
					else if (typeName[i] == ',' && nt == 0) {
						names.Add (typeName.Substring (si, i - si));
						si = i + 1;
					}
				}
				names.Add (typeName.Substring (si, i - si));
				var types = new object [names.Count];
				for (int n=0; n<names.Count; n++) {
					string tn = names [n];
					if (tn.StartsWith ("[", StringComparison.Ordinal))
						tn = tn.Substring (1, tn.Length - 2);
					types [n] = GetType (ctx, tn);
					if (types [n] == null)
						return new object [0];
				}
				return types;
			}

			return new object [0];
		}

		public override object GetType (EvaluationContext ctx, string name, object[] typeArgs)
		{
			var cx = (SoftEvaluationContext) ctx;

			int i = name.IndexOf (',');
			if (i != -1) {
				// Find first comma outside brackets
				int nest = 0;
				for (int n = 0; n < name.Length; n++) {
					char c = name [n];
					if (c == '[')
						nest++;
					else if (c == ']')
						nest--;
					else if (c == ',' && nest == 0) {
						name = name.Substring (0, n).Trim ();
						break;
					}
				}
			}
			
			if (typeArgs != null && typeArgs.Length > 0) {
				string args = "";

				foreach (var argType in typeArgs) {
					if (args.Length > 0)
						args += ",";

					string tn;
					var atm = argType as TypeMirror;
					if (atm == null) {
						var att = (Type) argType;

						tn = att.FullName + ", " + att.Assembly.GetName ();
					} else {
						tn = atm.FullName + ", " + atm.Assembly.GetName ();
					}

					if (tn.IndexOf (',') != -1)
						tn = "[" + tn + "]";

					args += tn;
				}

				name += "[" +args + "]";
			}
			
			var tm = cx.Session.GetType (name);
			if (tm != null)
				return tm;

			foreach (var asm in cx.Domain.GetAssemblies ()) {
				tm = asm.GetType (name, false, false);
				if (tm != null)
					return tm;
			}

			var method = cx.Frame.Method;
			if (method.IsGenericMethod && !method.IsGenericMethodDefinition) {
				var definition = method.GetGenericMethodDefinition ();
				var names = definition.GetGenericArguments ();
				var types = method.GetGenericArguments ();

				for (i = 0; i < names.Length; i++) {
					if (names[i].FullName == name)
						return types[i];
				}
			}

			var declaringType = method.DeclaringType;
			if (declaringType.IsGenericType && !declaringType.IsGenericTypeDefinition) {
				var definition = declaringType.GetGenericTypeDefinition ();
				var types = declaringType.GetGenericArguments ();
				var names = definition.GetGenericArguments ();

				for (i = 0; i < names.Length; i++) {
					if (names[i].FullName == name)
						return types[i];
				}
			}

			return null;
		}

		protected override object GetBaseTypeWithAttribute (EvaluationContext ctx, object type, object attrType)
		{
			var atm = attrType as TypeMirror;
			var tm = type as TypeMirror;

			while (tm != null && atm != null) {
				if (tm.GetCustomAttributes (atm, false).Any ())
					return tm;

				tm = tm.BaseType;
			}

			return null;
		}

		public override object GetParentType (EvaluationContext ctx, object type)
		{
			var tm = type as TypeMirror;
			if (tm != null) {
				int plus = tm.FullName.LastIndexOf ('+');

				return plus != -1 ? GetType (ctx, tm.FullName.Substring (0, plus)) : null;
			}

			return ((Type) type).DeclaringType;
		}

		public override IEnumerable<object> GetNestedTypes (EvaluationContext ctx, object type)
		{
			var tm = (TypeMirror) type;

			foreach (var nested in tm.GetNestedTypes ())
				if (!IsGeneratedType (nested))
					yield return nested;
		}

		public override IEnumerable<object> GetImplementedInterfaces (EvaluationContext ctx, object type)
		{
			var tm = (TypeMirror) type;

			foreach (var nested in tm.GetInterfaces ())
				yield return nested;
		}

		public override string GetTypeName (EvaluationContext ctx, object type)
		{
			var tm = type as TypeMirror ?? (type as ArgumentType)?.Type;

			if (tm != null) {
				if (IsGeneratedType (tm)) {
					// Return the name of the container-type.
					return tm.FullName.Substring (0, tm.FullName.LastIndexOf ('+'));
				}
				
				return tm.FullName;
			}

			return ((Type) type).FullName;
		}
		
		public override object GetValueType (EvaluationContext ctx, object val)
		{
			if (val == null)
				return typeof (object);
			if (val is ArrayMirror)
				return ((ArrayMirror) val).Type;
			if (val is ObjectMirror)
				return ((ObjectMirror) val).Type;
			if (val is EnumMirror)
				return ((EnumMirror) val).Type;
			if (val is StructMirror)
				return ((StructMirror) val).Type;
			if (val is PointerValue)
				return ((PointerValue) val).Type;
			if (val is DelayedLambdaValue)
				return ((DelayedLambdaValue)val).DelayedType;
			if (val is PrimitiveValue) {
				var pv = (PrimitiveValue) val;
				if (pv.Value == null)
					return typeof (object);

				return pv.Value.GetType ();
			}

			throw new NotSupportedException (val.GetType ().FullName);
		}
		
		public override object GetBaseType (EvaluationContext ctx, object type)
		{
			var tm = type as TypeMirror;
			return tm != null ? tm.BaseType : null;
		}

		public override bool HasMethodWithParamLength (EvaluationContext ctx, object targetType, string methodName, BindingFlags flags, int paramLength)
		{
			var soft = (SoftEvaluationContext)ctx;
			var currentType = ToTypeMirror (ctx, targetType);
			var allowInstance = (flags & BindingFlags.Instance) != 0;
			var allowStatic = (flags & BindingFlags.Static) != 0;
			var onlyPublic = (flags & BindingFlags.Public) != 0;

			while (currentType != null) {
				var methods = GetMethodsByName (soft, currentType, methodName);

				foreach (var method in methods) {
					var parms = method.GetParameters ();
					if (parms.Length == paramLength && ((method.IsStatic && allowStatic) || (!method.IsStatic && allowInstance))) {
						if (onlyPublic && !method.IsPublic)
							continue;
						return true;
					}
				}

				if (methodName == ".ctor")
					break;

				if (currentType.BaseType == null && currentType.FullName != "System.Object")
					currentType = soft.Adapter.GetType (soft, "System.Object") as TypeMirror;
				else
					currentType = currentType.BaseType;
			}

			return false;
		}

		public override bool HasMethod (EvaluationContext ctx, object targetType, string methodName, object [] genericTypeArgs, object [] argTypes, BindingFlags flags)
		{
			return HasMethod (ctx, targetType, methodName, genericTypeArgs, argTypes, flags, out _);
		}

		public override bool HasMethod (EvaluationContext ctx, object targetType, string methodName, object[] genericTypeArgs, object[] argTypes, BindingFlags flags, out Tuple<int, object>[] resolvedLambdaTypes)
		{
			var soft = (SoftEvaluationContext) ctx;
			var tm = ToTypeMirror (ctx, targetType);
			ArgumentType[] typeArgs = null;
			ArgumentType [] types = null;
			var hasDelayed = false;
			resolvedLambdaTypes = null;

			if (genericTypeArgs != null) {
				typeArgs = new ArgumentType [genericTypeArgs.Length];
				for (int n = 0; n < genericTypeArgs.Length; n++)
					typeArgs[n] = ToArgumentType (ctx, genericTypeArgs[n]);
			}

			if (argTypes != null) {
				types = new ArgumentType [argTypes.Length];
				for (int n = 0; n < argTypes.Length; n++) {
					var argType = ToArgumentType (ctx, argTypes[n]);
					types[n] = argType;
					hasDelayed |= argType.IsDelayed;
				}
			}

			var allowInstance = (flags & BindingFlags.Instance) != 0;
			var allowStatic = (flags & BindingFlags.Static) != 0;
			var throwIfNotFound = false;
			var tryCasting = true;
			MethodMirror method;

			if (hasDelayed) {
				var candidates = OverloadResolveMulti (soft, tm, methodName, typeArgs, null, types, allowInstance, allowStatic, throwIfNotFound, tryCasting);
				method = null;
			} else {
				method = OverloadResolve (soft, tm, methodName, typeArgs, types, allowInstance, allowStatic, throwIfNotFound, tryCasting);
			}

			return method != null;
		}

		// Returns the best method from `candidates' using lambda types in
		// `argTypes'. Non lambda types in argtypes will be ignored. We make sure
		// lamdas have its exected type by compiling them. If a matched method is
		// found, return an array of tuples of (argument index, resolved argument
		// type for lambda) through 'resolved'.
		
		public override bool IsExternalType (EvaluationContext ctx, object type)
		{
			var tm = type as TypeMirror;

			return tm == null || ((SoftEvaluationContext)ctx).Session.IsExternalCode (tm);
		}

		public override bool IsString (EvaluationContext ctx, object val)
		{
			return val is StringMirror;
		}
		
		public override bool IsArray (EvaluationContext ctx, object val)
		{
			return val is ArrayMirror;
		}

		public override bool IsValueType (object type)
		{
			var tm = type as TypeMirror;
			return tm != null && tm.IsValueType;
		}

		public override bool IsPrimitiveType (object type)
		{
			var tm = type as TypeMirror;
			if (tm == null)
				return false;

			if (tm.IsPrimitive)
				return true;

			if (tm.IsValueType && tm.Namespace == "System" && (tm.Name == "nfloat" || tm.Name == "nint"))
				return true;

			return false;
		}

		public override bool IsClass (EvaluationContext ctx, object type)
		{
			var tm = type as TypeMirror;
			return tm != null && (tm.IsClass || tm.IsValueType) && !tm.IsPrimitive;
		}

		public override bool IsNull (EvaluationContext ctx, object val)
		{
			return val == null || ((val is PrimitiveValue) && ((PrimitiveValue)val).Value == null) || ((val is PointerValue) && ((PointerValue)val).Address == 0);
		}

		public override bool IsPrimitive (EvaluationContext ctx, object val)
		{
			if (val is PrimitiveValue || val is StringMirror || val is PointerValue)
				return true;
			var sm = val as StructMirror;
			if (sm == null)
				return false;
			if (sm.Type.IsPrimitive)
				return true;
			if (sm.Type.Namespace == "System" && (sm.Type.Name == "nfloat" || sm.Type.Name == "nint"))
				return true;
			return false;
		}

		public override bool IsPointer (EvaluationContext ctx, object val)
		{
			return val is PointerValue;
		}

		public override bool IsEnum (EvaluationContext ctx, object val)
		{
			return val is EnumMirror;
		}

		public override bool IsDelayedType (EvaluationContext ctx, object type)
		{
			return type is DelayedLambdaType;
		}

		public override bool IsPublic (EvaluationContext ctx, object type)
		{
			var tm = ToTypeMirror (ctx, type);

			return tm != null && tm.IsPublic;
		}

		protected override TypeDisplayData OnGetTypeDisplayData (EvaluationContext ctx, object type)
		{
			Dictionary<string, DebuggerBrowsableState> memberData = null;
			var soft = (SoftEvaluationContext) ctx;
			bool isCompilerGenerated = false;
			string displayValue = null;
			string displayName = null;
			string displayType = null;
			string proxyType = null;

			try {
				var tm = (TypeMirror) type;

				foreach (var attr in tm.GetCustomAttributes (true)) {
					var attrName = attr.Constructor.DeclaringType.FullName;
					switch (attrName) {
					case "System.Diagnostics.DebuggerDisplayAttribute":
						var display = BuildAttribute<DebuggerDisplayAttribute> (attr);
						displayValue = display.Value;
						displayName = display.Name;
						displayType = display.Type;
						break;
					case "System.Diagnostics.DebuggerTypeProxyAttribute":
						var proxy = BuildAttribute<DebuggerTypeProxyAttribute> (attr);
						proxyType = proxy.ProxyTypeName;
						if (!string.IsNullOrEmpty (proxyType))
							ForceLoadType (soft, proxyType);
						break;
					case "System.Runtime.CompilerServices.CompilerGeneratedAttribute":
						isCompilerGenerated = true;
						break;
					}
				}

				foreach (var field in tm.GetFields ()) {
					var attrs = field.GetCustomAttributes (true);
					var browsable = GetAttribute <DebuggerBrowsableAttribute> (attrs);

					if (browsable == null) {
						var generated = GetAttribute<CompilerGeneratedAttribute> (attrs);
						if (generated != null)
							browsable = new DebuggerBrowsableAttribute (DebuggerBrowsableState.Never);
					}

					if (browsable != null) {
						if (memberData == null)
							memberData = new Dictionary<string, DebuggerBrowsableState> ();
						memberData [field.Name] = browsable.State;
					}
				}

				foreach (var property in tm.GetProperties ()) {
					var browsable = GetAttribute <DebuggerBrowsableAttribute> (property.GetCustomAttributes (true));
					if (browsable != null) {
						if (memberData == null)
							memberData = new Dictionary<string, DebuggerBrowsableState> ();
						memberData [property.Name] = browsable.State;
					}
				}
			} catch (Exception ex) {
				soft.Session.WriteDebuggerOutput (true, ex.ToString ());
			}

			return new TypeDisplayData (proxyType, displayValue, displayType, displayName, isCompilerGenerated, memberData);
		}
		
		static T GetAttribute<T> (CustomAttributeDataMirror[] attrs)
		{
			foreach (var attr in attrs) {
				if (attr.Constructor.DeclaringType.FullName == typeof (T).FullName)
					return BuildAttribute<T> (attr);
			}

			return default (T);
		}

		public override bool IsTypeLoaded (EvaluationContext ctx, string typeName)
		{
			var soft = (SoftEvaluationContext) ctx;
			
			return soft.Session.GetType (typeName) != null;
		}
		
		public override bool IsTypeLoaded (EvaluationContext ctx, object type)
		{
			var tm = (TypeMirror) type;

			if (tm.VirtualMachine.Version.AtLeast (2, 23))
				return tm.IsInitialized;

			return IsTypeLoaded (ctx, tm.FullName);
		}

		public override bool ForceLoadType (EvaluationContext ctx, object type)
		{
			var soft = (SoftEvaluationContext) ctx;
			var tm = (TypeMirror) type;

			if (!tm.VirtualMachine.Version.AtLeast (2, 23))
				return IsTypeLoaded (ctx, tm.FullName);

			if (tm.IsInitialized)
				return true;

			if (!tm.Attributes.HasFlag (TypeAttributes.BeforeFieldInit))
				return false;

			var cctor = OverloadResolve (soft, tm, ".cctor", null, new ArgumentType[0], false, true, false);
			if (cctor == null)
				return true;

			try {
				lock (cctor.VirtualMachine) {
					tm.InvokeMethod(soft.Thread, cctor, new Value [0], InvokeOptions.DisableBreakpoints | InvokeOptions.SingleThreaded);
				}
			} catch {
				return false;
			} finally {
				soft.Session.StackVersion++;
			}

			return true;
		}
		
		static T BuildAttribute<T> (CustomAttributeDataMirror attr)
		{
			var args = new List<object> ();
			var type = typeof (T);

			foreach (var arg in attr.ConstructorArguments) {
				object val = arg.Value;

				if (val is TypeMirror) {
					// The debugger attributes that take a type as parameter of the constructor have
					// a corresponding constructor overload that takes a type name. We'll use that
					// constructor because we can't load target types in the debugger process.
					// So what we do here is convert the Type to a String.
					var tm = (TypeMirror) val;
					// Workaround for older Mono runtime that doesn't support generics, simply use ICollection viewer instead generic version
					if (!tm.VirtualMachine.Version.AtLeast (2, 15) && tm.FullName.StartsWith ("System.Collections.Generic.CollectionDebuggerView`", StringComparison.Ordinal))
						val = "System.Collections.CollectionDebuggerView, " + tm.Assembly.ManifestModule.Name;
					else
						val = tm.FullName + ", " + tm.Assembly.ManifestModule.Name;
				} else if (val is EnumMirror) {
					var em = (EnumMirror) val;

					if (type == typeof (DebuggerBrowsableAttribute))
						val = (DebuggerBrowsableState) em.Value;
					else
						val = em.Value;
				}

				args.Add (val);
			}

			var attribute = Activator.CreateInstance (type, args.ToArray ());

			foreach (var arg in attr.NamedArguments) {
				object val = arg.TypedValue.Value;
				string postFix = "";

				if (arg.TypedValue.ArgumentType == typeof (Type))
					postFix = "TypeName";
				if (arg.Field != null)
					type.GetField (arg.Field.Name + postFix).SetValue (attribute, val);
				else if (arg.Property != null)
					type.GetProperty (arg.Property.Name + postFix).SetValue (attribute, val, null);
			}

			return (T) attribute;
		}

		public class ArgumentType
		{
			TypeMirror type;
			DelayedLambdaType delayedType;
			public bool RepresentsNull { get; internal set; }
			public bool IsDelayed { get; internal set; }
			public TypeMirror Type {
				get {
					if (IsDelayed)
						throw new NotSupportedException();
					return type;
				}
				internal set {
					type = value;
				}
			}
			public DelayedLambdaType DelayedType {
				get {
					if (!IsDelayed)
						throw new NotSupportedException();
					return delayedType;
				}
				internal set {
					delayedType = value;
				}
			}

			public static implicit operator TypeMirror (ArgumentType d)
			{
				return d.Type;
			}

			public static implicit operator ArgumentType (TypeMirror d)
			{
				return new ArgumentType () { Type = d };
			}
		}

		ArgumentType ToArgumentType (EvaluationContext ctx, object type)
		{
			var tm = type as TypeMirror;
			var isDelayed = ctx.Adapter.IsDelayedType (ctx, type);

			if (isDelayed) {
				return new ArgumentType {
					IsDelayed = true,
					DelayedType = type as DelayedLambdaType
				};
			}

			return new ArgumentType {
				Type = tm ?? (TypeMirror)GetType (ctx, ((Type)type).FullName),
				// GetValueType method always returns `typeof (object)` when value is `null`
				// We need to store this information because later in process this information is lost
				// when needed by OverloadResolve when trying to determine correct method overload
				RepresentsNull = (type as Type) == typeof (object)
			};
		}

		TypeMirror ToTypeMirror (EvaluationContext ctx, object type)
		{
			var tm = type as TypeMirror;

			return tm ?? (TypeMirror) GetType (ctx, ((Type) type).FullName);
		}

		public override object RuntimeInvoke (EvaluationContext ctx, object targetType, object target, string methodName, object[] genericTypeArgs, object[] argTypes, object[] argValues)
		{
			object[] outArgs;
			return RuntimeInvoke (ctx, targetType, target, methodName, genericTypeArgs, argTypes, argValues, false, out outArgs);
		}

		public override object RuntimeInvoke (EvaluationContext ctx, object targetType, object target, string methodName, object[] genericTypeArgs, object[] argTypes, object[] argValues, out object[] outArgs)
		{
			return RuntimeInvoke (ctx, targetType, target, methodName, genericTypeArgs, argTypes, argValues, true, out outArgs);
		}

		private object RuntimeInvoke (EvaluationContext ctx, object targetType, object target, string methodName, object[] genericTypeArgs, object[] argTypes, object[] argValues,bool enableOutArgs, out object[] outArgs)
		{
			var type = ToTypeMirror (ctx, targetType);
			var soft = (SoftEvaluationContext) ctx;

			soft.AssertTargetInvokeAllowed ();

			var genericTypes = new ArgumentType [genericTypeArgs != null ? genericTypeArgs.Length : 0];
			for (int n = 0; n < genericTypes.Length; n++)
				genericTypes[n] = ToArgumentType (soft, genericTypeArgs[n]);

			var types = new ArgumentType [argTypes.Length];
			for (int n = 0; n < argTypes.Length; n++)
				types[n] = ToArgumentType (soft, argTypes[n]);

			var method = OverloadResolve (soft, type, methodName, genericTypes, types, target != null, target == null, true);
			var mparams = method.GetParameters ();
			var values = new Value [argValues.Length];

			for (int n = 0; n < argValues.Length; n++) {
				var param_type = mparams [n].ParameterType;
				if (param_type.FullName != types [n].Type.FullName && !param_type.IsAssignableFrom (types [n].Type) && param_type.IsGenericType &&
				   !(param_type.IsGenericType && param_type.FullName.StartsWith ("System.Nullable`1", StringComparison.Ordinal))) {
					var throwCastException = true;

					if (method.VirtualMachine.Version.AtLeast (2, 15)) {
						var args = param_type.GetGenericArguments ();

						if (args.Length == genericTypes.Length) {
							var real_type = soft.Adapter.GetType (soft, param_type.GetGenericTypeDefinition ().FullName, genericTypes);

							values [n] = (Value)TryCast (soft, (Value)argValues [n], real_type);
							if (!(values [n] == null && argValues [n] != null && !soft.Adapter.IsNull (soft, argValues [n])))
								throwCastException = false;
						}
					}

					if (throwCastException) {
						var fromType = !IsGeneratedType (types [n].Type) ? soft.Adapter.GetDisplayTypeName (soft, types [n]) : types [n].Type.FullName;
						var toType = soft.Adapter.GetDisplayTypeName (soft, param_type);

						throw new EvaluatorException ("Argument {0}: Cannot implicitly convert `{1}' to `{2}'", n, fromType, toType);
					}
				} else if (argValues[n] is DelayedLambdaValue) {
					outArgs = null;
					return null;
					/*string error;
					var val = CompileAndLoadLambdaValue (soft, (DelayedLambdaValue) argValues[n], param_type, out error);
					values[n] = (Value) val;

					if (error != null)
						throw new EvaluatorException ("Invalid arguments for method `{0}': Argument {1}: {2}", methodName, n, error);*/
				} else if (param_type.FullName != types [n].Type.FullName && !param_type.IsAssignableFrom (types [n].Type) && !types[n].RepresentsNull && CanForceCast (ctx, types [n], param_type)) {
					values [n] = (Value)TryCast (ctx, argValues [n], param_type);
				} else {
					values [n] = (Value)argValues [n];
				}
			}
			if (enableOutArgs) {
				Value[] outArgsValue;
				var result = soft.RuntimeInvoke (method, target ?? targetType, values, out outArgsValue);
				outArgs = outArgsValue;
				return result;
			} else {
				outArgs = null;
				return soft.RuntimeInvoke (method, target ?? targetType, values);
			}
		}

		static ArgumentType[] ResolveGenericTypeArguments (MethodMirror method, ArgumentType[] argTypes)
		{
			var genericArgs = method.GetGenericArguments ();
			var types = new ArgumentType[genericArgs.Length];
			var parameters = method.GetParameters ();
			var names = new List<string> ();

			// map the generic argument type names
			foreach (var arg in genericArgs)
				names.Add (arg.Name);

			// map parameter types to generic argument types...
			for (int i = 0; i < argTypes.Length && i < parameters.Length; i++) {
				if (argTypes[i].IsDelayed)
					continue;

				var paramType = parameters[i].ParameterType;
				var isArray = paramType.IsArray;
				if (isArray)
					paramType = paramType.GetElementType ();

				int index = names.IndexOf (paramType.Name);

				if (index != -1 && types[index] == null) {
					var argType = argTypes[i].Type;
					if (isArray && argType.IsArray)
						argType = argType.GetElementType ();

					types[index] = argType;
				}
			}

			// make sure we have all the generic argument types...
			for (int i = 0; i < types.Length; i++) {
				if (types[i] == null)
					return null;
			}

			return types;
		}

		static MethodMirror PickFirstCandidate (MethodMirror [] methods)
		{
			if (methods == null || methods.Length == 0)
				return null;

			if (methods.Length == 1)
				return methods [0];

			// If there is an ambiguous match, just pick the first match. If the user was expecting
			// something else, he can provide more specific arguments

			//if (!throwIfNotFound)
			//	return null;
			//if (methodName != null)
			//	throw new EvaluatorException ("Ambiguous method `{0}'; need to use full name", methodName);
			//else
			//	throw new EvaluatorException ("Ambiguous arguments for indexer.", methodName);

			return methods [0];
		}

		public static MethodMirror OverloadResolve (SoftEvaluationContext ctx, TypeMirror type, string methodName, ArgumentType[] genericTypeArgs, ArgumentType[] argTypes, bool allowInstance, bool allowStatic, bool throwIfNotFound, bool tryCasting = true)
		{
			return OverloadResolve (ctx, type, methodName, genericTypeArgs, null, argTypes, allowInstance, allowStatic, throwIfNotFound, tryCasting);
		}

		public static MethodMirror OverloadResolve (SoftEvaluationContext ctx, TypeMirror type, string methodName, ArgumentType [] genericTypeArgs, TypeMirror returnType, ArgumentType [] argTypes, bool allowInstance, bool allowStatic, bool throwIfNotFound, bool tryCasting)
		{
			var results = OverloadResolveMulti (ctx, type, methodName, genericTypeArgs, returnType, argTypes, allowInstance, allowStatic, throwIfNotFound, tryCasting: tryCasting);
			return PickFirstCandidate (results);
		}

		public static MethodMirror[] GetMethodsByName (SoftEvaluationContext ctx, TypeMirror type, string methodName)
		{
			const BindingFlags flag = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
			MethodMirror[] methods = null;
			var cache = ctx.Session.OverloadResolveCache;

			if (ctx.CaseSensitive) {
				lock (cache) {
					cache.TryGetValue (Tuple.Create (type, methodName), out methods);
				}
			}

			if (methods == null) {
				if (type.VirtualMachine.Version.AtLeast (2, 7)) {
					methods = type.GetMethodsByNameFlags (methodName, flag, !ctx.CaseSensitive);
				} else {
					methods = type.GetMethods ();
				}

				if (ctx.CaseSensitive) {
					lock (cache) {
						cache [Tuple.Create (type, methodName)] = methods;
					}
				}
			}
			return methods;
		}


		public static MethodMirror[] OverloadResolveMulti (SoftEvaluationContext ctx, TypeMirror type, string methodName, ArgumentType [] genericTypeArgs, TypeMirror returnType, ArgumentType [] argTypes, bool allowInstance, bool allowStatic, bool throwIfNotFound, bool tryCasting)
		{
			var candidates = new List<MethodMirror> ();
			var currentType = type;

			while (currentType != null) {
				MethodMirror [] methods = GetMethodsByName(ctx, currentType, methodName);

				foreach (var method in methods) {
					if (method.Name == methodName || (!ctx.CaseSensitive && method.Name.Equals (methodName, StringComparison.CurrentCultureIgnoreCase))) {
						MethodMirror actualMethod;

						if (argTypes != null && method.VirtualMachine.Version.AtLeast (2, 24) && method.IsGenericMethod) {
							var generic = method.GetGenericMethodDefinition ();
							ArgumentType [] typeArgs;

							//Console.WriteLine ("Attempting to resolve generic type args for: {0}", GetPrettyMethodName (ctx, generic));

							if ((genericTypeArgs == null || genericTypeArgs.Length == 0))
								typeArgs = ResolveGenericTypeArguments (generic, argTypes);
							else
								typeArgs = genericTypeArgs;

							if (typeArgs == null || typeArgs.Length != generic.GetGenericArguments ().Length) {
								//Console.WriteLine ("Failed to resolve generic method argument types...");
								continue;
							}

							actualMethod = generic.MakeGenericMethod (typeArgs.Select (t => t.Type).ToArray ());
							//Console.WriteLine ("Resolve generic method to: {0}", GetPrettyMethodName (ctx, actualMethod));
						} else {
							actualMethod = method;
						}

						var parms = actualMethod.GetParameters ();
						if (argTypes == null || parms.Length == argTypes.Length && ((actualMethod.IsStatic && allowStatic) || (!actualMethod.IsStatic && allowInstance)))
							candidates.Add (actualMethod);
					}
				}

				if (argTypes == null && candidates.Count > 0)
					break; // when argTypes is null, we are just looking for *any* match (not a specific match)

				if (methodName == ".ctor")
					break; // Can't create objects using constructor from base classes

				// Make sure that we always pull in at least System.Object methods (this is mostly needed for cases where 'type' was an interface)
				if (currentType.BaseType == null && currentType.FullName != "System.Object")
					currentType = ctx.Adapter.GetType (ctx, "System.Object") as TypeMirror;
				else
					currentType = currentType.BaseType;
			}

			return OverloadResolveMulti (ctx, type, methodName, genericTypeArgs, returnType, argTypes, candidates, throwIfNotFound, tryCasting);
		}

		static bool IsApplicable (SoftEvaluationContext ctx, MethodMirror method, ArgumentType[] genericTypeArgs, TypeMirror returnType, ArgumentType [] types, out string error, out int matchCount, bool tryCasting = true)
		{
			var mparams = method.GetParameters ();
			matchCount = 0;

			for (int i = 0; i < types.Length; i++) {
				var param_type = mparams[i].ParameterType;

				if (types [i].RepresentsNull && !SoftEvaluationContext.IsValueTypeOrPrimitive (param_type))
					continue;

				if (types[i].IsDelayed) {
					var lambdaType = types[i].DelayedType;
					if (lambdaType.IsAcceptableType (param_type)) {
						matchCount++;
					}
					continue;
				}

				if (param_type.FullName == types[i].Type.FullName) {
					matchCount++;
					continue;
				}

				if (param_type.IsAssignableFrom (types[i].Type))
					continue;

				if (param_type.IsGenericType) {
					if (genericTypeArgs != null && method.VirtualMachine.Version.AtLeast (2, 12)) {
						// FIXME: how can we make this more definitive?
						if (param_type.GetGenericArguments ().Length == genericTypeArgs.Length)
							continue;
					} else {
						// no way to check... assume it'll work?
						continue;
					}
				}

				if (tryCasting && CanCast (ctx, types [i], param_type))
					continue;

				if (CanDoPrimaryCast(types[i].Type, param_type))
					continue;

				var fromType = !IsGeneratedType (types[i].Type) ? ctx.Adapter.GetDisplayTypeName (ctx, types[i]) : types[i].Type.FullName;
				var toType = ctx.Adapter.GetDisplayTypeName (ctx, param_type);

				error = string.Format ("Argument {0}: Cannot implicitly convert `{1}' to `{2}'", i, fromType, toType);

				return false;
			}

			if (returnType != null && returnType != method.ReturnType) {
				var actual = ctx.Adapter.GetDisplayTypeName (ctx, method.ReturnType);
				var expected = ctx.Adapter.GetDisplayTypeName (ctx, returnType);

				error = string.Format ("Return types do not match: `{0}' vs `{1}'", expected, actual);

				return false;
			}

			error = null;

			return true;
		}

		static bool CanDoPrimaryCast (TypeMirror fromType, TypeMirror toType)
		{
			var name = toType.CSharpName;
			switch (fromType.CSharpName) {
			case "sbyte": return name == "short" || name == "int" || name == "long" || name == "float" || name == "double" || name == "decimal";
			case "byte": return name == "short" || name == "ushort" || name == "int" || name == "uint" || name == "long" || name == "ulong" || name == "float" || name == "double" || name == "decimal";
			case "short": return name == "int" || name == "long" || name == "float" || name == "double" || name == "decimal";
			case "ushort": return name == "int" || name == "uint" || name == "long" || name == "ulong" || name == "float" || name == "double" || name == "decimal";
			case "int": return name == "long" || name == "float" || name == "double" || name == "decimal";
			case "uint": return name == "long" || name == "ulong" || name == "float" || name == "double" || name == "decimal";
			case "long": return name == "float" || name == "double" || name == "decimal";
			case "char": return name == "ushort" || name == "int" || name == "uint" || name == "long" || name == "ulong" || name == "float" || name == "double" || name == "decimal";
			case "float": return name == "double";
			case "ulong": return name == "float" || name == "double" || name == "decimal";
			}
			return false;
		}

		static MethodMirror OverloadResolve (SoftEvaluationContext ctx, TypeMirror type, string methodName, ArgumentType[] genericTypeArgs, TypeMirror returnType, ArgumentType[] argTypes, List<MethodMirror> candidates, bool throwIfNotFound, bool tryCasting = true)
		{
			var results = OverloadResolveMulti (ctx, type, methodName, genericTypeArgs, returnType, argTypes, candidates, throwIfNotFound, tryCasting: tryCasting);
			return PickFirstCandidate (results);
		}

		static MethodMirror[] OverloadResolveMulti (SoftEvaluationContext ctx, TypeMirror type, string methodName, ArgumentType[] genericTypeArgs, TypeMirror returnType, ArgumentType[] argTypes, List<MethodMirror> candidates, bool throwIfNotFound, bool tryCasting = true)
		{
			if (candidates.Count == 0) {
				if (throwIfNotFound) {
					var typeName = ctx.Adapter.GetDisplayTypeName (ctx, type);

					if (methodName == null)
						throw new EvaluatorException ("Indexer not found in type `{0}'.", typeName);

					if (genericTypeArgs != null && genericTypeArgs.Length > 0) {
						var types = string.Join (", ", genericTypeArgs.Select (t => ctx.Adapter.GetDisplayTypeName (ctx, t)));

						throw new EvaluatorException ("Method `{0}<{1}>' not found in type `{2}'.", methodName, types, typeName);
					}

					throw new EvaluatorException ("Method `{0}' not found in type `{1}'.", methodName, typeName);
				}

				return null;
			}

			if (argTypes == null) {
				// This is just a probe to see if the type contains *any* methods of the given name
				return new MethodMirror[] { candidates [0] };
			}

			if (candidates.Count == 1) {
				if (IsApplicable (ctx, candidates [0], genericTypeArgs, returnType, argTypes, out var error, out var matchCount, tryCasting))
					return new MethodMirror [] { candidates [0] };

				if (throwIfNotFound)
					throw new EvaluatorException ("Invalid arguments for method `{0}': {1}", methodName, error);

				return null;
			}
			
			// Ok, now we need to find exact matches.
			var bestCandidates = new List<MethodMirror>();
			int bestCount = -1;
			
			foreach (var method in candidates) {
				string error;
				int matchCount;
				
				if (!IsApplicable (ctx, method, genericTypeArgs, returnType, argTypes, out error, out matchCount, tryCasting))
					continue;

				if (matchCount == bestCount) {
					bestCandidates.Add (method);
				} else if (matchCount > bestCount) {
					bestCandidates = new List<MethodMirror> { method };
					bestCount = matchCount;
				}
			}

			if (bestCandidates.Count == 0) {
				if (!throwIfNotFound)
					return null;

				if (methodName != null)
					throw new EvaluatorException ("Invalid arguments for method `{0}'.", methodName);

				throw new EvaluatorException ("Invalid arguments for indexer.");
			}
			return bestCandidates.ToArray ();
		}		

		public override object TargetObjectToObject (EvaluationContext ctx, object obj)
		{
			if (obj is StringMirror) {
				return MirrorStringToString (ctx, (StringMirror)obj);
			}

			if (obj is PrimitiveValue)
				return ((PrimitiveValue)obj).Value;

			if (obj is PointerValue)
				return new EvaluationResult ("0x" + ((PointerValue)obj).Address.ToString ("x"));

			if (obj is StructMirror) {
				var sm = (StructMirror) obj;

				if (sm.Type.IsPrimitive) {
					// Boxed primitive
					if (sm.Type.FullName == "System.IntPtr")
						return new EvaluationResult ("0x" + ((long)((PointerValue)sm.Fields [0]).Address).ToString ("x"));
					if (sm.Fields.Length > 0 && (sm.Fields[0] is PrimitiveValue))
						return ((PrimitiveValue)sm.Fields[0]).Value;
				} else if (sm.Type.FullName == "System.Decimal") {
					var soft = (SoftEvaluationContext) ctx;

					var method = OverloadResolve (soft, sm.Type, "GetBits", null, new [] { new ArgumentType { Type = sm.Type, RepresentsNull = false } }, false, true, false);

					if (method != null) {
						ArrayMirror array;

						try {
							lock (method.VirtualMachine) {
								array =  sm.Type.InvokeMethod(soft.Thread, method, new [] { sm }, InvokeOptions.DisableBreakpoints | InvokeOptions.SingleThreaded) as ArrayMirror;
							}
						} catch {
							array = null;
						} finally {
							soft.Session.StackVersion++;
						}
						
						if (array != null) {
							var bits = new int[4];

							for (int i = 0; i < 4; i++)
								bits[i] = (int) TargetObjectToObject (ctx, array[i]);
							
							return new decimal (bits);
						}
					}
				}
			}

			return base.TargetObjectToObject (ctx, obj);
		}

		static string MirrorStringToString (EvaluationContext ctx, StringMirror mirror)
		{
			string str;

			if (ctx.Options.EllipsizeStrings) {
				if (mirror.VirtualMachine.Version.AtLeast (2, 10)) {
					int length = mirror.Length;

					if (length > ctx.Options.EllipsizedLength)
						str = new string (mirror.GetChars (0, ctx.Options.EllipsizedLength)) + EvaluationOptions.Ellipsis;
					else
						str = mirror.Value;
				} else {
					str = mirror.Value;
					if (str.Length > ctx.Options.EllipsizedLength)
						str = str.Substring (0, ctx.Options.EllipsizedLength) + EvaluationOptions.Ellipsis;
				}
			} else {
				str = mirror.Value;
			}

			return str;
		}
	}

	class MethodCall: AsyncOperation
	{
		readonly InvokeOptions options = InvokeOptions.DisableBreakpoints;

		readonly ManualResetEvent shutdownEvent = new ManualResetEvent (false);
		readonly MethodMirror function;
		readonly Value[] args;
		readonly object obj;
		IAsyncResult handle;
		Exception exception;
		InvokeResult result;
		
		public MethodCall (SoftEvaluationContext ctx, MethodMirror function, object obj, Value[] args, bool enableOutArgs) : base (ctx)
		{
			this.function = function;
			this.obj = obj;
			this.args = args;
			if (enableOutArgs) {
				this.options |= InvokeOptions.ReturnOutArgs;
			}
			if (function.VirtualMachine.Version.AtLeast (2, 40)) {
				this.options |= InvokeOptions.Virtual;
			}
		}
		
		public override string Description {
			get {
				return function.DeclaringType.FullName + "." + function.Name;
			}
		}

		public override void Invoke ()
		{
			var ctx = (SoftEvaluationContext) EvaluationContext;

			try {
				if (obj is ObjectMirror)
					handle = ((ObjectMirror)obj).BeginInvokeMethod (ctx.Thread, function, args, options, null, null);
				else if (obj is TypeMirror)
					handle = ((TypeMirror)obj).BeginInvokeMethod (ctx.Thread, function, args, options, null, null);
				else if (obj is StructMirror)
					handle = ((StructMirror)obj).BeginInvokeMethod (ctx.Thread, function, args, options | InvokeOptions.ReturnOutThis, null, null);
				else if (obj is PrimitiveValue)
					handle = ((PrimitiveValue)obj).BeginInvokeMethod (ctx.Thread, function, args, options, null, null);
				else
					throw new ArgumentException ("Soft debugger method calls cannot be invoked on objects of type " + obj.GetType ().Name);
			} catch (InvocationException ex) {
				ctx.Session.StackVersion++;
				exception = ex;
			} catch (Exception ex) {
				ctx.Session.StackVersion++;
				DebuggerLoggingService.LogError ("Error in soft debugger method call thread on " + GetInfo (), ex);
				exception = ex;
			}
		}

		public override void Abort ()
		{
			var ctx = (SoftEvaluationContext)EvaluationContext;
			if (handle is IInvokeAsyncResult) {
				var info = GetInfo ();
				DebuggerLoggingService.LogMessage ("Aborting invocation of " + info);
				((IInvokeAsyncResult) handle).Abort ();
				ctx.Session.StackVersion++;
				// Don't wait for the abort to finish. The engine will do it.
			} else {
				throw new NotSupportedException ();
			}
		}
		
		public override void Shutdown ()
		{
			shutdownEvent.Set ();
		}
		
		void EndInvoke ()
		{
			var ctx = (SoftEvaluationContext) EvaluationContext;

			try {
				if (obj is ObjectMirror)
					result = ((ObjectMirror)obj).EndInvokeMethodWithResult (handle);
				else if (obj is TypeMirror)
					result = ((TypeMirror)obj).EndInvokeMethodWithResult (handle);
				else if (obj is StructMirror)
					result = ((StructMirror)obj).EndInvokeMethodWithResult (handle);
				else
					result = ((PrimitiveValue)obj).EndInvokeMethodWithResult (handle);
			} catch (InvocationException ex) {
				if (!Aborting && ex.Exception != null) {
					string ename = ctx.Adapter.GetValueTypeName (ctx, ex.Exception);
					var vref = ctx.Adapter.GetMember (ctx, null, ex.Exception, "Message");

					exception = vref != null ? new Exception (ename + ": " + (string) vref.ObjectValue) : new Exception (ename);
					return;
				}
				exception = ex;
			} catch (Exception ex) {
				DebuggerLoggingService.LogError ("Error in soft debugger method call thread on " + GetInfo (), ex);
				exception = ex;
			} finally {
				ctx.Session.StackVersion++;
			}
		}
		
		string GetInfo ()
		{
			try {
				TypeMirror type = null;
				if (obj is ObjectMirror)
					type = ((ObjectMirror)obj).Type;
				else if (obj is TypeMirror)
					type = (TypeMirror)obj;
				else if (obj is StructMirror)
					type = ((StructMirror)obj).Type;
				return string.Format ("method {0} on object {1}",
				                      function == null? "[null]" : function.FullName,
				                      type == null? "[null]" : type.FullName);
			} catch (Exception ex) {
				DebuggerLoggingService.LogError ("Error getting info for SDB MethodCall", ex);
				return "";
			}
		}

		public override bool WaitForCompleted (int timeout)
		{
			if (handle == null)
				return true;
			int res = WaitHandle.WaitAny (new WaitHandle[] { handle.AsyncWaitHandle, shutdownEvent }, timeout); 
			if (res == 0) {
				EndInvoke ();
				return true;
			}
			// Return true if shut down.
			return res == 1;
		}

		public Value ReturnValue {
			get {
				if (exception != null)
					throw new EvaluatorException (exception.Message);
				return result.Result;
			}
		}

		public Value[] OutArgs {
			get {
				if (exception != null)
					throw new EvaluatorException (exception.Message);
				return result.OutArgs;
			}
		}
	}
}
