//
// NRefactoryExpressionEvaluatorVisitor.cs
//
// Author: Jeffrey Stedfast <jeff@xamarin.com>
//
// Copyright (c) 2013 Xamarin Inc.
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
using System.Reflection;
using System.Collections.Generic;

using Mono.Debugging.Client;


namespace Mono.Debugging.Evaluation
{
	/*
	public class NRefactoryExpressionEvaluatorVisitor : CSharpSyntaxVisitor<ValueReference>
	{
		readonly Dictionary<string, ValueReference> userVariables;
		readonly EvaluationOptions options;
		readonly EvaluationContext ctx;
		readonly object expectedType;
		readonly string expression;

		public NRefactoryExpressionEvaluatorVisitor (EvaluationContext ctx, string expression, object expectedType, Dictionary<string,ValueReference> userVariables)
		{
			this.ctx = ctx;
			this.expression = expression;
			this.expectedType = expectedType;
			this.userVariables = userVariables;
			this.options = ctx.Options;
		}

		static Exception ParseError (string message, params object[] args)
		{
			return new EvaluatorException (message, args);
		}

		static Exception NotSupported ()
		{
			return new NotSupportedExpressionException ();
		}

		static string ResolveTypeName (SyntaxNode type)
		{
			string name = type.ToString ();
			if (name.StartsWith ("global::", StringComparison.Ordinal))
				name = name.Substring ("global::".Length);

			var index = name.IndexOf('<');
			if (index > -1)
				name = name.Substring(0, index);
			return name;
		}

		static long GetInteger (object val)
		{
			try {
				return Convert.ToInt64 (val);
			} catch {
				throw ParseError ("Expected integer value.");
			}
		}

		long ConvertToInt64 (object val)
		{
			if (val is IntPtr)
				return ((IntPtr) val).ToInt64 ();

			if (ctx.Adapter.IsEnum (ctx, val)) {
				var type = ctx.Adapter.GetType (ctx, "System.Int64");
				var result = ctx.Adapter.Cast (ctx, val, type);

				return (long) ctx.Adapter.TargetObjectToObject (ctx, result);
			}

			return Convert.ToInt64 (val);
		}

		static Type GetCommonOperationType (object v1, object v2)
		{
			if (v1 is double || v2 is double)
				return typeof (double);

			if (v1 is float || v2 is float)
				return typeof (double);

			return typeof (long);
		}

		static Type GetCommonType (object v1, object v2)
		{
			var t1 = Type.GetTypeCode (v1.GetType ());
			var t2 = Type.GetTypeCode (v2.GetType ());
			if (t1 < TypeCode.Int32 && t2 < TypeCode.Int32)
				return typeof (int);

			switch ((TypeCode) Math.Max ((int) t1, (int) t2)) {
			case TypeCode.Byte: return typeof (byte);
			case TypeCode.Decimal: return typeof (decimal);
			case TypeCode.Double: return typeof (double);
			case TypeCode.Int16: return typeof (short);
			case TypeCode.Int32: return typeof (int);
			case TypeCode.Int64: return typeof (long);
			case TypeCode.SByte: return typeof (sbyte);
			case TypeCode.Single: return typeof (float);
			case TypeCode.UInt16: return typeof (ushort);
			case TypeCode.UInt32: return typeof (uint);
			case TypeCode.UInt64: return typeof (ulong);
			default: throw new Exception (((TypeCode) Math.Max ((int) t1, (int) t2)).ToString ());
			}
		}

		static object EvaluateOperation (SyntaxKind op, double v1, double v2)
		{
			switch (op) {
			case SyntaxKind.AddExpression: return v1 + v2;
			case SyntaxKind.DivideExpression: return v1 / v2;
			case SyntaxKind.MultiplyExpression: return v1 * v2;
			case SyntaxKind.SubtractExpression: return v1 - v2;
			case SyntaxKind.GreaterThanExpression: return v1 > v2;
			case SyntaxKind.GreaterThanOrEqualExpression: return v1 >= v2;
			case SyntaxKind.LessThanExpression: return v1 < v2;
			case SyntaxKind.LessThanOrEqualExpression: return v1 <= v2;
			case SyntaxKind.EqualsExpression: return v1 == v2;
			case SyntaxKind.NotEqualsExpression: return v1 != v2;
			default: throw ParseError ("Invalid binary operator.");
			}
		}

		static object EvaluateOperation (SyntaxKind op, long v1, long v2)
		{
			switch (op) {
			case SyntaxKind.AddExpression: return v1 + v2;
			case SyntaxKind.BitwiseAndExpression: return v1 & v2;
			case SyntaxKind.BitwiseOrExpression: return v1 | v2;
			case SyntaxKind.ExclusiveOrExpression: return v1 ^ v2;
			case SyntaxKind.DivideExpression: return v1 / v2;
			case SyntaxKind.ModuloExpression: return v1 % v2;
			case SyntaxKind.MultiplyExpression: return v1 * v2;
			case SyntaxKind.LeftShiftExpression: return v1 << (int) v2;
			case SyntaxKind.RightShiftExpression: return v1 >> (int) v2;
			case SyntaxKind.SubtractExpression: return v1 - v2;
			case SyntaxKind.GreaterThanExpression: return v1 > v2;
			case SyntaxKind.GreaterThanOrEqualExpression: return v1 >= v2;
			case SyntaxKind.LessThanExpression: return v1 < v2;
			case SyntaxKind.LessThanOrEqualExpression: return v1 <= v2;
			case SyntaxKind.EqualsExpression: return v1 == v2;
			case SyntaxKind.NotEqualsExpression: return v1 != v2;
			default: throw ParseError ("Invalid binary operator.");
			}
		}

		static bool CheckReferenceEquality (EvaluationContext ctx, object v1, object v2)
		{
			if (v1 == null && v2 == null)
				return true;

			if (v1 == null || v2 == null)
				return false;

			object objectType = ctx.Adapter.GetType (ctx, "System.Object");
			object[] argTypes = { objectType, objectType };
			object[] args = { v1, v2 };

			object result = ctx.Adapter.RuntimeInvoke (ctx, objectType, null, "ReferenceEquals", argTypes, args);
			var literal = LiteralValueReference.CreateTargetObjectLiteral (ctx, "result", result);

			return (bool) literal.ObjectValue;
		}

		static bool CheckEquality (EvaluationContext ctx, bool negate, object type1, object type2, object targetVal1, object targetVal2, object val1, object val2)
		{
			if (val1 == null && val2 == null)
				return !negate;

			if (val1 == null || val2 == null)
				return negate;

			string method = negate ? "op_Inequality" : "op_Equality";
			object[] argTypes = { type1, type2 };
			object target, targetType;
			object[] args;

			if (ctx.Adapter.HasMethod (ctx, type1, method, argTypes, BindingFlags.Public | BindingFlags.Static)) {
				args = new [] { targetVal1, targetVal2 };
				targetType = type1;
				target = null;
				negate = false;
			} else if (ctx.Adapter.HasMethod (ctx, type2, method, argTypes, BindingFlags.Public | BindingFlags.Static)) {
				args = new [] { targetVal1, targetVal2 };
				targetType = type2;
				target = null;
				negate = false;
			} else {
				method = ctx.Adapter.IsValueType (type1) ? "Equals" : "ReferenceEquals";
				targetType = ctx.Adapter.GetType (ctx, "System.Object");
				argTypes = new [] { targetType, targetType };
				args = new [] { targetVal1, targetVal2 };
				target = null;
			}

			object result = ctx.Adapter.RuntimeInvoke (ctx, targetType, target, method, argTypes, args);
			var literal = LiteralValueReference.CreateTargetObjectLiteral (ctx, "result", result);
			bool retval = (bool) literal.ObjectValue;

			return negate ? !retval : retval;
		}

		static ValueReference EvaluateOverloadedOperator (EvaluationContext ctx, string expression, SyntaxKind op, object type1, object type2, object targetVal1, object targetVal2, object val1, object val2)
		{
			object[] args = new [] { targetVal1, targetVal2 };
			object[] argTypes = { type1, type2 };
			object targetType = null;
			string methodName = null;

			switch (op) {
			case SyntaxKind.BitwiseAndExpression:         methodName = "op_BitwiseAnd"; break;
			case SyntaxKind.BitwiseOrExpression:          methodName = "op_BitwiseOr"; break;
			case SyntaxKind.ExclusiveOrExpression:        methodName = "op_ExclusiveOr"; break;
			case SyntaxKind.GreaterThanExpression:        methodName = "op_GreaterThan"; break;
			case SyntaxKind.GreaterThanOrEqualExpression: methodName = "op_GreaterThanOrEqual"; break;
			case SyntaxKind.EqualsExpression:             methodName = "op_Equality"; break;
			case SyntaxKind.NotEqualsExpression:          methodName = "op_Inequality"; break;
			case SyntaxKind.LessThanExpression:           methodName = "op_LessThan"; break;
			case SyntaxKind.LessThanOrEqualExpression:    methodName = "op_LessThanOrEqual"; break;
			case SyntaxKind.AddExpression:                methodName = "op_Addition"; break;
			case SyntaxKind.SubtractExpression:           methodName = "op_Subtraction"; break;
			case SyntaxKind.MultiplyExpression:           methodName = "op_Multiply"; break;
			case SyntaxKind.DivideExpression:             methodName = "op_Division"; break;
			case SyntaxKind.ModuloExpression:             methodName = "op_Modulus"; break;
			case SyntaxKind.LeftShiftExpression:          methodName = "op_LeftShift"; break;
			case SyntaxKind.RightShiftExpression:         methodName = "op_RightShift"; break;
			}

			if (methodName == null)
				throw ParseError ("Invalid operands in binary operator.");

			if (ctx.Adapter.HasMethod (ctx, type1, methodName, argTypes, BindingFlags.Public | BindingFlags.Static)) {
				targetType = type1;
			} else if (ctx.Adapter.HasMethod (ctx, type2, methodName, argTypes, BindingFlags.Public | BindingFlags.Static)) {
				targetType = type2;
			} else {
				throw ParseError ("Invalid operands in binary operator.");
			}

			object result = ctx.Adapter.RuntimeInvoke (ctx, targetType, null, methodName, argTypes, args);

			return LiteralValueReference.CreateTargetObjectLiteral (ctx, expression, result);
		}

		ValueReference EvaluateBinaryOperatorExpression (SyntaxKind op, ValueReference left, ExpressionSyntax rightExp)
		{
			if (op == SyntaxKind.LogicalAndExpression) {
				var val = left.ObjectValue;
				if (!(val is bool))
					throw ParseError ("Left operand of logical And must be a boolean.");

				if (!(bool) val)
					return LiteralValueReference.CreateObjectLiteral (ctx, expression, false);

				var vr = Visit (rightExp);
				if (vr == null || ctx.Adapter.GetTypeName (ctx, vr.Type) != "System.Boolean")
					throw ParseError ("Right operand of logical And must be a boolean.");

				return vr;
			}

			if (op == SyntaxKind.LogicalOrExpression) {
				var val = left.ObjectValue;
				if (!(val is bool))
					throw ParseError ("Left operand of logical Or must be a boolean.");

				if ((bool) val)
					return LiteralValueReference.CreateObjectLiteral (ctx, expression, true);

				var vr = Visit (rightExp);
				if (vr == null || ctx.Adapter.GetTypeName (ctx, vr.Type) != "System.Boolean")
					throw ParseError ("Right operand of logical Or must be a boolean.");

				return vr;
			}

			var right = Visit (rightExp);
			var targetVal1 = left.Value;
			var targetVal2 = right.Value;
			var type1 = ctx.Adapter.GetValueType (ctx, targetVal1);
			var type2 = ctx.Adapter.GetValueType (ctx, targetVal2);
			var val1 = left.ObjectValue;
			var val2 = right.ObjectValue;
			object res = null;

			if (ctx.Adapter.IsNullableType (ctx, type1) && ctx.Adapter.NullableHasValue (ctx, type1, val1)) {
				if (val2 == null) {
					if (op == SyntaxKind.EqualsExpression)
						return LiteralValueReference.CreateObjectLiteral (ctx, expression, false);
					if (op == SyntaxKind.NotEqualsExpression)
						return LiteralValueReference.CreateObjectLiteral (ctx, expression, true);
				}

				ValueReference nullable = ctx.Adapter.NullableGetValue (ctx, type1, val1);
				targetVal1 = nullable.Value;
				val1 = nullable.ObjectValue;
				type1 = nullable.Type;
			}

			if (ctx.Adapter.IsNullableType (ctx, type2) && ctx.Adapter.NullableHasValue (ctx, type2, val2)) {
				if (val1 == null) {
					if (op == SyntaxKind.EqualsExpression)
						return LiteralValueReference.CreateObjectLiteral (ctx, expression, false);
					if (op == SyntaxKind.NotEqualsExpression)
						return LiteralValueReference.CreateObjectLiteral (ctx, expression, true);
				}

				ValueReference nullable = ctx.Adapter.NullableGetValue (ctx, type2, val2);
				targetVal2 = nullable.Value;
				val2 = nullable.ObjectValue;
				type2 = nullable.Type;
			}

			if (val1 is string || val2 is string) {
				switch (op) {
				case SyntaxKind.AddExpression:
					if (val1 != null && val2 != null) {
						if (!(val1 is string))
							val1 = ctx.Adapter.CallToString (ctx, targetVal1);

						if (!(val2 is string))
							val2 = ctx.Adapter.CallToString (ctx, targetVal2);

						res = (string) val1 + (string) val2;
					} else if (val1 != null) {
						res = val1.ToString ();
					} else if (val2 != null) {
						res = val2.ToString ();
					}

					return LiteralValueReference.CreateObjectLiteral (ctx, expression, res);
				case SyntaxKind.EqualsExpression:
					if ((val1 == null || val1 is string) && (val2 == null || val2 is string))
						return LiteralValueReference.CreateObjectLiteral (ctx, expression, ((string) val1) == ((string) val2));
					break;
				case SyntaxKind.NotEqualsExpression:
					if ((val1 == null || val1 is string) && (val2 == null || val2 is string))
						return LiteralValueReference.CreateObjectLiteral (ctx, expression, ((string) val1) != ((string) val2));
					break;
				}
			}

			if (val1 == null || (!ctx.Adapter.IsPrimitive (ctx, targetVal1) && !ctx.Adapter.IsEnum (ctx, targetVal1))) {
				switch (op) {
				case SyntaxKind.EqualsExpression:
					return LiteralValueReference.CreateObjectLiteral (ctx, expression, CheckEquality (ctx, false, type1, type2, targetVal1, targetVal2, val1, val2));
				case SyntaxKind.NotEqualsExpression:
					return LiteralValueReference.CreateObjectLiteral (ctx, expression, CheckEquality (ctx, true, type1, type2, targetVal1, targetVal2, val1, val2));
				default:
					if (val1 != null && val2 != null)
						return EvaluateOverloadedOperator (ctx, expression, op, type1, type2, targetVal1, targetVal2, val1, val2);
					break;
				}
			}

			if ((val1 is bool) && (val2 is bool)) {
				switch (op) {
				case SyntaxKind.ExclusiveOrExpression:
					return LiteralValueReference.CreateObjectLiteral (ctx, expression, (bool) val1 ^ (bool) val2);
				case SyntaxKind.EqualsExpression:
					return LiteralValueReference.CreateObjectLiteral (ctx, expression, (bool) val1 == (bool) val2);
				case SyntaxKind.NotEqualsExpression:
					return LiteralValueReference.CreateObjectLiteral (ctx, expression, (bool) val1 != (bool) val2);
				}
			}

			if (val1 == null || val2 == null || (val1 is bool) || (val2 is bool))
				throw ParseError ("Invalid operands in binary operator.");

			var commonType = GetCommonOperationType (val1, val2);

			if (commonType == typeof (double)) {
				double v1, v2;

				try {
					v1 = Convert.ToDouble (val1);
					v2 = Convert.ToDouble (val2);
				} catch {
					throw ParseError ("Invalid operands in binary operator.");
				}

				res = EvaluateOperation (op, v1, v2);
			} else {
				var v1 = ConvertToInt64 (val1);
				var v2 = ConvertToInt64 (val2);

				res = EvaluateOperation (op, v1, v2);
			}

			if (!(res is bool) && !(res is string)) {
				if (ctx.Adapter.IsEnum (ctx, targetVal1)) {
					object tval = ctx.Adapter.Cast (ctx, ctx.Adapter.CreateValue (ctx, res), ctx.Adapter.GetValueType (ctx, targetVal1));
					return LiteralValueReference.CreateTargetObjectLiteral (ctx, expression, tval);
				}

				if (ctx.Adapter.IsEnum (ctx, targetVal2)) {
					object tval = ctx.Adapter.Cast (ctx, ctx.Adapter.CreateValue (ctx, res), ctx.Adapter.GetValueType (ctx, targetVal2));
					return LiteralValueReference.CreateTargetObjectLiteral (ctx, expression, tval);
				}

				var targetType = GetCommonType (val1, val2);

				if (targetType != typeof (IntPtr))
					res = Convert.ChangeType (res, targetType);
				else
					res = new IntPtr ((long) res);
			}

			return LiteralValueReference.CreateObjectLiteral (ctx, expression, res);
		}

		static object[] UpdateDelayedTypes (object[] types, Tuple<int, object>[] updates, ref bool alreadyUpdated)
		{
			if (alreadyUpdated || types == null || updates == null || types.Length < updates.Length || updates.Length == 0)
				return types;

			for (int x = 0; x < updates.Length; x++) {
				int index = updates[x].Item1;
				types[index] = updates[x].Item2;
			}
			alreadyUpdated = true;
			return types;
		}

		#region IAstVisitor implementation
		public override ValueReference VisitArrayCreationExpression (ArrayCreationExpressionSyntax node)
		{
			var type = Visit(node.Type.ElementType);
			if (type == null)
				throw ParseError ("Invalid type in array creation.");

			var lengths = Array.Empty<int>();
			if (node.Type is ArrayTypeSyntax ats) {
				lengths = new int[ats.RankSpecifiers[0].Sizes.Count];
				for (int i = 0; i < lengths.Length; i++) {
					var arsi = ats.RankSpecifiers[0].Sizes[i];
					lengths [i] = (int)Convert.ChangeType (Visit(arsi).ObjectValue, typeof (int));
				}
			}
			var array = ctx.Adapter.CreateArray (ctx, type.Type, lengths);
			if (node.Initializer?.Expressions.Count > 0) {
				var arrayAdaptor = ctx.Adapter.CreateArrayAdaptor (ctx, array);
				int index = 0;
				foreach (var el in LinearElements(node.Initializer.Expressions)) {
					arrayAdaptor.SetElement (new int [] { index++ },  Visit(el).Value);
				}
			}
			return LiteralValueReference.CreateTargetObjectLiteral (ctx, expression, array);
		}

		IEnumerable<ExpressionSyntax> LinearElements (SeparatedSyntaxList<ExpressionSyntax> elements)
		{
			foreach (var el in elements) {
				if (el is ArrayCreationExpressionSyntax arrCre) { 
					foreach (var el2 in LinearElements (arrCre.Initializer.Expressions)) {
						yield return el2;
					}
				} else if (el is InitializerExpressionSyntax ies) { 
					foreach (var el2 in LinearElements (ies.Expressions)) {
						yield return el2;
					}
				} else
					yield return el;
			}
		}

		public override ValueReference VisitAssignmentExpression (AssignmentExpressionSyntax node)
		{
			if (!options.AllowMethodEvaluation)
				throw new ImplicitEvaluationDisabledException ();

			var left = Visit (node.Left);

			if (node.Kind () == SyntaxKind.SimpleAssignmentExpression) {
				var right = Visit (node.Right);
				if (left is UserVariableReference) {
					left.Value = right.Value;
				} else {
					var castedValue = ctx.Adapter.TryCast (ctx, right.Value, left.Type);
					left.Value = castedValue;
				}
			} else {
				SyntaxKind op;

				switch (node.Kind ()) {
				case SyntaxKind.AddAssignmentExpression:         op = SyntaxKind.AddExpression; break;
				case SyntaxKind.SubtractAssignmentExpression:    op = SyntaxKind.SubtractExpression; break;
				case SyntaxKind.MultiplyAssignmentExpression:    op = SyntaxKind.MultiplyExpression; break;
				case SyntaxKind.DivideAssignmentExpression:      op = SyntaxKind.DivideExpression; break;
				case SyntaxKind.ModuloAssignmentExpression:      op = SyntaxKind.ModuloExpression; break;
				case SyntaxKind.LeftShiftAssignmentExpression:   op = SyntaxKind.LeftShiftExpression; break;
				case SyntaxKind.RightShiftAssignmentExpression:  op = SyntaxKind.RightShiftExpression; break;
				case SyntaxKind.AndAssignmentExpression:         op = SyntaxKind.BitwiseAndExpression; break;
				case SyntaxKind.OrAssignmentExpression:          op = SyntaxKind.BitwiseOrExpression; break;
				case SyntaxKind.ExclusiveOrAssignmentExpression: op = SyntaxKind.ExclusiveOrExpression; break;
				default: throw ParseError ("Invalid operator in assignment.");
				}

				var result = EvaluateBinaryOperatorExpression (op, left, node.Right);
				left.Value = result.Value;
			}

			return left;
		}

		public override ValueReference VisitBaseExpression (BaseExpressionSyntax node)
		{
			var self = ctx.Adapter.GetThisReference (ctx);

			if (self != null)
				return LiteralValueReference.CreateTargetBaseObjectLiteral (ctx, expression, self.Value);

			throw ParseError ("'base' reference not available in static methods.");
		}

		public override ValueReference VisitBinaryExpression (BinaryExpressionSyntax node)
		{
			if (node.IsKind (SyntaxKind.AsExpression)) {
				var type = Visit (node.Right) as TypeValueReference;
				if (type == null)
					throw ParseError ("Invalid type in cast.");

				var val = Visit (node.Left);
				var result = ctx.Adapter.TryCast (ctx, val.Value, type.Type);

				if (result == null)
					return new NullValueReference (ctx, type.Type);

				return LiteralValueReference.CreateTargetObjectLiteral (ctx, expression, result, type.Type);
			}
			if (node.IsKind (SyntaxKind.IsExpression)) {
				var type = (Visit (node.Right) as TypeValueReference)?.Type;
				if (type == null)
					throw ParseError ("Invalid type in 'is' expression.");
				if (ctx.Adapter.IsNullableType (ctx, type))
					type = ctx.Adapter.GetGenericTypeArguments (ctx, type).Single ();
				var val = Visit (node.Left).Value;
				if (ctx.Adapter.IsNull (ctx, val))
					return LiteralValueReference.CreateObjectLiteral (ctx, expression, false);
				var valueIsPrimitive = ctx.Adapter.IsPrimitive (ctx, val);
				var typeIsPrimitive = ctx.Adapter.IsPrimitiveType (type);
				if (valueIsPrimitive != typeIsPrimitive)
					return LiteralValueReference.CreateObjectLiteral (ctx, expression, false);
				if (typeIsPrimitive)
					return LiteralValueReference.CreateObjectLiteral (ctx, expression, ctx.Adapter.GetTypeName (ctx, type) == ctx.Adapter.GetValueTypeName (ctx, val));
				return LiteralValueReference.CreateObjectLiteral (ctx, expression, ctx.Adapter.TryCast (ctx, val, type) != null);
			}

			var left = this.Visit (node.Left);

			return EvaluateBinaryOperatorExpression (node.Kind (), left, node.Right);
		}

		public override ValueReference VisitCastExpression (CastExpressionSyntax node)
		{
			var type = Visit(node.Type) as TypeValueReference;
			if (type == null)
				throw ParseError ("Invalid type in cast.");

			var val = Visit(node.Expression);
			object result = ctx.Adapter.TryCast (ctx, val.Value, type.Type);
			if (result == null)
				throw ParseError ("Invalid cast.");

			return LiteralValueReference.CreateTargetObjectLiteral (ctx, expression, result, type.Type);
		}

		public override ValueReference VisitCheckedExpression (CheckedExpressionSyntax node)
		{
			throw NotSupported ();
		}

		public override ValueReference VisitConditionalExpression (ConditionalExpressionSyntax node)
		{
			ValueReference val = Visit(node.Condition);
			if (val is TypeValueReference)
				throw NotSupported ();

			if ((bool)val.ObjectValue)
				return Visit (node.WhenTrue);;

			return Visit (node.WhenFalse);
		}

		public override ValueReference VisitDefaultExpression (DefaultExpressionSyntax node)
		{
			var type = Visit(node.Type) as TypeValueReference;
			if (type == null)
				throw ParseError ("Invalid type in 'default' expression.");

			if (ctx.Adapter.IsClass (ctx, type.Type))
				return LiteralValueReference.CreateTargetObjectLiteral (ctx, expression, ctx.Adapter.CreateNullValue (ctx, type.Type), type.Type);

			if (ctx.Adapter.IsValueType (type.Type))
				return LiteralValueReference.CreateTargetObjectLiteral (ctx, expression, ctx.Adapter.CreateValue (ctx, type.Type, new object [0]), type.Type);

			switch (ctx.Adapter.GetTypeName (ctx, type.Type)) {
			case "System.Boolean": return LiteralValueReference.CreateObjectLiteral (ctx, expression, false);
			case "System.Char": return LiteralValueReference.CreateObjectLiteral (ctx, expression, '\0');
			case "System.Byte": return LiteralValueReference.CreateObjectLiteral (ctx, expression, (byte) 0);
			case "System.SByte": return LiteralValueReference.CreateObjectLiteral (ctx, expression, (sbyte) 0);
			case "System.Int16": return LiteralValueReference.CreateObjectLiteral (ctx, expression, (short) 0);
			case "System.UInt16": return LiteralValueReference.CreateObjectLiteral (ctx, expression, (ushort) 0);
			case "System.Int32": return LiteralValueReference.CreateObjectLiteral (ctx, expression, (int) 0);
			case "System.UInt32": return LiteralValueReference.CreateObjectLiteral (ctx, expression, (uint) 0);
			case "System.Int64": return LiteralValueReference.CreateObjectLiteral (ctx, expression, (long) 0);
			case "System.UInt64": return LiteralValueReference.CreateObjectLiteral (ctx, expression, (ulong) 0);
			case "System.Decimal": return LiteralValueReference.CreateObjectLiteral (ctx, expression, (decimal) 0);
			case "System.Single": return LiteralValueReference.CreateObjectLiteral (ctx, expression, (float) 0);
			case "System.Double": return LiteralValueReference.CreateObjectLiteral (ctx, expression, (double) 0);
			default: throw new Exception ($"Unexpected type {ctx.Adapter.GetTypeName (ctx, type.Type)}");
			}
		}

		public override ValueReference VisitIdentifierName (IdentifierNameSyntax node)
		{
			var name = node.Identifier.ValueText;

			if (name == "__EXCEPTION_OBJECT__")
				return ctx.Adapter.GetCurrentException (ctx);

			// Look in user defined variables

			ValueReference userVar;
			if (userVariables.TryGetValue (name, out userVar))
				return userVar;

			// Look in variables

			ValueReference var = ctx.Adapter.GetLocalVariable (ctx, name);
			if (var != null)
				return var;

			// Look in parameters

			var = ctx.Adapter.GetParameter (ctx, name);
			if (var != null)
				return var;

			// Look in instance fields and properties

			ValueReference self = ctx.Adapter.GetThisReference (ctx);

			if (self != null) {
				// check for fields and properties in this instance

				// first try if current type has field or property
				var = ctx.Adapter.GetMember (ctx, self, ctx.Adapter.GetEnclosingType (ctx), self.Value, name);
				if (var != null)
					return var;
				
				var = ctx.Adapter.GetMember (ctx, self, self.Type, self.Value, name);
				if (var != null)
					return var;
			}

			// Look in static fields & properties of the enclosing type and all parent types

			object type = ctx.Adapter.GetEnclosingType (ctx);
			object vtype = type;

			while (vtype != null) {
				// check for static fields and properties
				var = ctx.Adapter.GetMember (ctx, null, vtype, null, name);
				if (var != null)
					return var;

				vtype = ctx.Adapter.GetParentType (ctx, vtype);
			}

			// Look in types

			vtype = ctx.Adapter.GetType (ctx, name);
			if (vtype != null)
				return new TypeValueReference (ctx, vtype);

			if (self == null && ctx.Adapter.HasMember (ctx, type, name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
				string message = string.Format ("An object reference is required for the non-static field, method, or property '{0}.{1}'.",
				                                ctx.Adapter.GetDisplayTypeName (ctx, type), name);
				throw ParseError (message);
			}

			// Assume a namespace
			return new NamespaceValueReference(ctx, name);
		}

		public override ValueReference VisitElementAccessExpression (ElementAccessExpressionSyntax node)
		{
			int n = 0;

			var target = Visit(node.Expression);
			if (target is TypeValueReference)
				throw NotSupported ();

			if (ctx.Adapter.IsArray (ctx, target.Value)) {
				int[] indexes = new int [node.ArgumentList.Arguments.Count];

				foreach (var arg in node.ArgumentList.Arguments) {
					var index = Visit(arg);
					indexes[n++] = (int) Convert.ChangeType (index.ObjectValue, typeof (int));
				}

				return new ArrayValueReference (ctx, target.Value, indexes);
			}

			object[] args = new object [node.ArgumentList.Arguments.Count];
			foreach (var arg in node.ArgumentList.Arguments)
				args[n++] = Visit (arg).Value;

			var indexer = ctx.Adapter.GetIndexerReference (ctx, target.Value, target.Type, args);
			if (indexer == null)
				throw NotSupported ();

			return indexer;
		}

		string ResolveMethodName (SyntaxNode invocationExpression, out object[] typeArgs)
		{
			if (invocationExpression is IdentifierNameSyntax id) {
				typeArgs = null;
				return id.Identifier.ValueText;
			}
			if (invocationExpression is GenericNameSyntax gns) {
				if (gns.Arity > 0) {
					var args = new List<object> ();

					foreach (var arg in gns.TypeArgumentList.Arguments) {
						var type = Visit(arg);
						args.Add (type.Type);
					}

					typeArgs = args.ToArray ();
				} else {
					typeArgs = null;
				}
				return gns.Identifier.ValueText;
			}

			typeArgs = null;
			var fullName = invocationExpression.ToString();
			var lastIndexOfDot = fullName.LastIndexOf(".");
			return fullName.Substring(lastIndexOfDot + 1);
		}

		public override ValueReference VisitInvocationExpression (InvocationExpressionSyntax node)
		{
			if (!options.AllowMethodEvaluation)
				throw new ImplicitEvaluationDisabledException ();

			bool invokeBaseMethod = false;
			bool allArgTypesAreResolved = true;
			ValueReference target = null;
			string methodName;

			var types = new object [node.ArgumentList.Arguments.Count];
			var args = new object [node.ArgumentList.Arguments.Count];
			object[] typeArgs = null;
			int n = 0;

			foreach (var arg in node.ArgumentList.Arguments) {
				var vref = this.Visit (arg);
				args[n] = vref.Value;
				types[n] = ctx.Adapter.GetValueType (ctx, args[n]);

				if (ctx.Adapter.IsDelayedType (ctx, types[n]))
					allArgTypesAreResolved = false;
				n++;
			}
			object vtype = null;
			Tuple<int, object>[] resolvedLambdaTypes;

			if (node.Expression is MemberAccessExpressionSyntax field) {
				target = Visit (field.Expression);
				if (field.Expression is BaseExpressionSyntax)
					invokeBaseMethod = true;
				methodName = ResolveMethodName (field, out typeArgs);
			} else if (node.Expression is SyntaxNode method && (method is IdentifierNameSyntax || method is GenericNameSyntax)) {
				var vref = ctx.Adapter.GetThisReference (ctx);

				methodName = ResolveMethodName (method, out typeArgs);

				if (vref != null && ctx.Adapter.HasMethod (ctx, vref.Type, methodName, typeArgs, types, BindingFlags.Instance)) {
					vtype = ctx.Adapter.GetEnclosingType (ctx);
					// There is an instance method for 'this', although it may not have an exact signature match. Check it now.
					if (ctx.Adapter.HasMethod (ctx, vref.Type, methodName, typeArgs, types, BindingFlags.Instance)) {
						target = vref;
					} else {
						// There isn't an instance method with exact signature match.
						// If there isn't a static method, then use the instance method,
						// which will report the signature match error when invoked
						if (!ctx.Adapter.HasMethod (ctx, vtype, methodName, typeArgs, types, BindingFlags.Static))
							target = vref;
					}
				} else {
					if (ctx.Adapter.HasMethod (ctx, ctx.Adapter.GetEnclosingType (ctx), methodName, types, BindingFlags.Instance))
						throw new EvaluatorException ("Cannot invoke an instance method from a static method.");
					target = null;
				}
			} else {
				throw NotSupported ();
			}

			if (vtype == null)
				vtype = target != null ? target.Type : ctx.Adapter.GetEnclosingType (ctx);
			object vtarget = (target is TypeValueReference) || target == null ? null : target.Value;

			var hasMethod = ctx.Adapter.HasMethod (ctx, vtype, methodName, typeArgs, types, BindingFlags.Instance | BindingFlags.Static, out resolvedLambdaTypes);
			if (hasMethod)
				types = UpdateDelayedTypes (types, resolvedLambdaTypes, ref allArgTypesAreResolved);

			if (invokeBaseMethod) {
				vtype = ctx.Adapter.GetBaseType (ctx, vtype);
			} else if (target != null && !hasMethod) {
				// Look for LINQ extension methods...
				var linq = ctx.Adapter.GetType (ctx, "System.Linq.Enumerable");
				if (linq != null) {
					object[] xtypeArgs = typeArgs;

					if (xtypeArgs == null) {
						// try to infer the generic type arguments from the type of the object...
						object xtype = vtype;
						while (xtype != null && !ctx.Adapter.IsGenericType (ctx, xtype))
							xtype = ctx.Adapter.GetBaseType (ctx, xtype);

						if (xtype != null)
							xtypeArgs = ctx.Adapter.GetTypeArgs (ctx, xtype);
					}

					if (xtypeArgs == null && ctx.Adapter.IsArray (ctx, vtarget)) {
						xtypeArgs = new object [] { ctx.Adapter.CreateArrayAdaptor (ctx, vtarget).ElementType };
					}

					if (xtypeArgs != null) {
						var xtypes = new object[types.Length + 1];
						Array.Copy (types, 0, xtypes, 1, types.Length);
						xtypes[0] = vtype;

						var xargs = new object[args.Length + 1];
						Array.Copy (args, 0, xargs, 1, args.Length);
						xargs[0] = vtarget;

						if (ctx.Adapter.HasMethod (ctx, linq, methodName, xtypeArgs, xtypes, BindingFlags.Static, out resolvedLambdaTypes)) {
							vtarget = null;
							vtype = linq;

							typeArgs = xtypeArgs;
							types = UpdateDelayedTypes (xtypes, resolvedLambdaTypes, ref allArgTypesAreResolved);
							args = xargs;
						}
					}
				}
			}

			if (!allArgTypesAreResolved) {
				// TODO: Show detailed error message for why lambda types were not
				// resolved. Major causes are:
				// 1. there is no matched method
				// 2. matched method exists, but the lambda body has some invalid
				// expressions and does not compile
				throw NotSupported ();
			}

			object result = ctx.Adapter.RuntimeInvoke (ctx, vtype, vtarget, methodName, typeArgs, types, args);
			if (result != null)
				return LiteralValueReference.CreateTargetObjectLiteral (ctx, expression, result);

			return LiteralValueReference.CreateVoidReturnLiteral (ctx, expression);
		}


		public override ValueReference VisitParenthesizedLambdaExpression (ParenthesizedLambdaExpressionSyntax node)
		{
			return VisitLambdaExpression(node);
		}

		public override ValueReference VisitSimpleLambdaExpression (SimpleLambdaExpressionSyntax node)
		{
			return VisitLambdaExpression (node);
		}

		ValueReference VisitLambdaExpression (LambdaExpressionSyntax node)
		{
			if (node.AsyncKeyword != default)
				throw NotSupported ();

			var parent = node.Parent;
			while (parent != null && parent is ParenthesizedExpressionSyntax)
				parent = parent.Parent;

			if (parent is InvocationExpressionSyntax || parent is CastExpressionSyntax || parent is ArgumentSyntax) {
				var visitor = new LambdaBodyOutputVisitor (ctx, userVariables);
				visitor.Visit (node);
				var values = visitor.GetLocalValues ();
				object val = ctx.Adapter.CreateDelayedLambdaValue (ctx, node.ToString (), values);
				if (val != null)
					return LiteralValueReference.CreateTargetObjectLiteral (ctx, expression, val);
			}

			throw NotSupported ();
		}

		public override ValueReference VisitMemberAccessExpression (MemberAccessExpressionSyntax node)
		{
			if (node.Name is GenericNameSyntax gns) {
				var expr = Visit (node.Expression);

				if (expr is TypeValueReference tvr && node.Expression is GenericNameSyntax gnsOuter) {
					// Thing<string>.Done<int> is
					// Thing`1.Done`1 with type arguments System.String and System.Int32
					// Thing`1 is node.Expression
					// Done`1 is node.Name
					var outer = MakeGenericTypeName (gnsOuter, gnsOuter.Identifier.ValueText);

					var typeArgsOuter = GetGenericTypeArgs(gnsOuter);
					var inner = MakeGenericTypeName(gns, gns.Identifier.ValueText);
					var typeArgsInner = GetGenericTypeArgs(gns);

					string nestedTypeName = $"{outer}.{inner}";
					var nestedType = ctx.Adapter.GetType(ctx, nestedTypeName, typeArgsOuter.Concat(typeArgsInner).ToArray());
					return new TypeValueReference(ctx, nestedType);

				}

				var typeArgs = GetGenericTypeArgs (gns);
				string typeName = $"{ResolveTypeName (node)}`{gns.TypeArgumentList.Arguments.Count}";

				var type1 = ctx.Adapter.GetType (ctx, typeName, typeArgs);
				return new TypeValueReference (ctx, type1);
			}

			if (node.Name is IdentifierNameSyntax)
			{
				var name = ResolveTypeName(node);
				var type = ctx.Adapter.GetType(ctx, name);

				if (type != null)
					return new TypeValueReference(ctx, type);
			}
			var target = Visit (node.Expression);

			var member = target.GetChild(node.Name.Identifier.ValueText, ctx.Options);

			if (member != null)
				return member;

			if (!(target is TypeValueReference)) {
				if (ctx.Adapter.IsNull(ctx, target.Value))
					throw new EvaluatorException("{0} is null", target.Name);
			}
			throw new EvaluatorException("{0} is null", target.Name);

		}

		object [] GetGenericTypeArgs (GenericNameSyntax gns)
		{
			object [] typeArgs = new object [gns.TypeArgumentList.Arguments.Count];

			for (var i = 0; i < gns.TypeArgumentList.Arguments.Count; i++) {
				var typeArg = Visit (gns.TypeArgumentList.Arguments [i]);
				typeArgs [i] = typeArg.Type;
			}

			return typeArgs;
		}

		public override ValueReference VisitLiteralExpression (LiteralExpressionSyntax node)
		{
			if (node.Kind() == SyntaxKind.NullLiteralExpression)
				return new NullValueReference (ctx, ctx.Adapter.GetType (ctx, "System.Object"));
			return base.VisitLiteralExpression (node);
		}

		public override ValueReference VisitObjectCreationExpression (ObjectCreationExpressionSyntax node)
		{
			var type = Visit(node.Type) as TypeValueReference;
			var args = new List<object> ();

			foreach (var arg in node.ArgumentList.Arguments) {
				var val = Visit(arg);
				args.Add (val != null ? val.Value : null);
			}

			return LiteralValueReference.CreateTargetObjectLiteral (ctx, expression, ctx.Adapter.CreateValue (ctx, type.Type, args.ToArray ()));
		}

		public override ValueReference VisitParenthesizedExpression (ParenthesizedExpressionSyntax node)
		{
			return Visit (node.Expression);
		}

		public override ValueReference VisitPredefinedType (PredefinedTypeSyntax node)
		{
			var type = ctx.Adapter.GetType(ctx, node.Resolve());
			return new TypeValueReference (ctx, type);
		}

		public override ValueReference VisitThisExpression (ThisExpressionSyntax node)
		{
			var self = ctx.Adapter.GetThisReference (ctx);

			if (self == null)
				throw ParseError ("'this' reference not available in the current evaluation context.");

			return self;
		}

		public override ValueReference VisitTypeOfExpression (TypeOfExpressionSyntax node)
		{
			var name = ResolveTypeName (node.Type);

			var type = ctx.Adapter.GetType(ctx, name);
			if (type == null)
				throw ParseError ("Could not load type: {0}", name);

			object result = ctx.Adapter.CreateTypeObject (ctx, type);
			if (result == null)
				throw NotSupported ();

			return LiteralValueReference.CreateTargetObjectLiteral (ctx, name, result);
		}

		public override ValueReference VisitPostfixUnaryExpression (PostfixUnaryExpressionSyntax node)
		{
			var vref = Visit (node.Operand);
			var val = vref.ObjectValue;
			object newVal;
			long num;

			switch (node.Kind ()) {
			case SyntaxKind.PostDecrementExpression:
				if (val is decimal) {
					newVal = ((decimal)val) - 1;
				} else if (val is double) {
					newVal = ((double)val) - 1;
				} else if (val is float) {
					newVal = ((float)val) - 1;
				} else {
					num = GetInteger (val) - 1;
					newVal = Convert.ChangeType (num, val.GetType ());
				}
				vref.Value = ctx.Adapter.CreateValue (ctx, newVal);
				break;
			case SyntaxKind.PostIncrementExpression:
				if (val is decimal) {
					newVal = ((decimal)val) + 1;
				} else if (val is double) {
					newVal = ((double)val) + 1;
				} else if (val is float) {
					newVal = ((float)val) + 1;
				} else {
					num = GetInteger (val) + 1;
					newVal = Convert.ChangeType (num, val.GetType ());
				}
				vref.Value = ctx.Adapter.CreateValue (ctx, newVal);
				break;
			default:
				throw NotSupported ();
			}

			return LiteralValueReference.CreateObjectLiteral (ctx, expression, val);
		}

		public override ValueReference VisitPrefixUnaryExpression (PrefixUnaryExpressionSyntax node)
		{
			var vref = Visit (node.Operand);
			var val = vref.ObjectValue;
			object newVal;
			long num;

			switch (node.Kind ()) {
			case SyntaxKind.BitwiseNotExpression:
				num = ~GetInteger (val);
				val = Convert.ChangeType (num, val.GetType ());
				break;
			case SyntaxKind.UnaryMinusExpression:
				if (val is decimal) {
					val = -(decimal)val;
				} else if (val is double) {
					val = -(double)val;
				} else if (val is float) {
					val = -(float)val;
				} else {
					num = -GetInteger (val);
					val = Convert.ChangeType (num, val.GetType ());
				}
				break;
			case SyntaxKind.LogicalNotExpression:
				if (!(val is bool))
					throw ParseError ("Expected boolean type in Not operator.");

				val = !(bool)val;
				break;
			case SyntaxKind.PreDecrementExpression:
				if (val is decimal) {
					val = ((decimal)val) - 1;
				} else if (val is double) {
					val = ((double)val) - 1;
				} else if (val is float) {
					val = ((float)val) - 1;
				} else {
					num = GetInteger (val) - 1;
					val = Convert.ChangeType (num, val.GetType ());
				}
				vref.Value = ctx.Adapter.CreateValue (ctx, val);
				break;
			case SyntaxKind.PreIncrementExpression:
				if (val is decimal) {
					val = ((decimal)val) + 1;
				} else if (val is double) {
					val = ((double)val) + 1;
				} else if (val is float) {
					val = ((float)val) + 1;
				} else {
					num = GetInteger (val) + 1;
					val = Convert.ChangeType (num, val.GetType ());
				}
				vref.Value = ctx.Adapter.CreateValue (ctx, val);
				break;
			case SyntaxKind.UnaryPlusExpression:
				break;
			default:
				throw NotSupported ();
			}

			return LiteralValueReference.CreateObjectLiteral (ctx, expression, val);
		}

		public override ValueReference VisitArgument (ArgumentSyntax node)
		{
			return this.Visit(node.Expression);
		}
		public override ValueReference VisitAliasQualifiedName(AliasQualifiedNameSyntax node)
		{
			//return null;
			return this.Visit(node.Name);
		}

		public override ValueReference VisitNullableType (NullableTypeSyntax node)
		{
			var genericTypeArgument = new[] { Visit(node.ElementType).Type };
			var type1 = ctx.Adapter.GetType(ctx, "System.Nullable`1", genericTypeArgument);
			return new TypeValueReference(ctx, type1);
		}

		public override ValueReference VisitGenericName (GenericNameSyntax node)
		{
			object [] typeArgs = new object [node.TypeArgumentList.Arguments.Count];

			for (var i = 0; i < node.TypeArgumentList.Arguments.Count; i++) {
				var typeArg = Visit (node.TypeArgumentList.Arguments [i]);
				typeArgs [i] = typeArg.Type;
			}

			string typeName = node.Identifier.ValueText;

			typeName = MakeGenericTypeName(node, typeName);// $"{typeName}`{node.TypeArgumentList.Arguments.Count}";


			var type1 = ctx.Adapter.GetType (ctx, typeName, typeArgs);
			return new TypeValueReference (ctx, type1);
		}

		static string MakeGenericTypeName(GenericNameSyntax node, string typeName)
		{
			if (node.Parent is QualifiedNameSyntax qns && qns.Left is IdentifierNameSyntax ins)
				return $"{ins.Identifier.Value}.{typeName}`{node.TypeArgumentList.Arguments.Count}";

			if (node.Parent is QualifiedNameSyntax qns2 && qns2.Left is QualifiedNameSyntax left)
				return $"{left}.{typeName}`{node.TypeArgumentList.Arguments.Count}";
			return $"{typeName}`{node.TypeArgumentList.Arguments.Count}";
;
 		}

		public override ValueReference VisitQualifiedName(QualifiedNameSyntax node)
		{
			if(node.Right is GenericNameSyntax) {
				return Visit(node.Right);
			}
			var type1 = ctx.Adapter.GetType(ctx, node.ToString());
			return new TypeValueReference(ctx, type1);
		}

		public override ValueReference DefaultVisit (SyntaxNode node)
		{
			if (node is LiteralExpressionSyntax syntax)
			{
				return LiteralValueReference.CreateObjectLiteral(ctx, expression, syntax.Token.Value);
			}
			throw NotSupported();
		}
		#endregion
	}*/
}