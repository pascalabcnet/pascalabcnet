//
// NRefactoryExtensions.cs
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
using System.Collections.Generic;


namespace Mono.Debugging.Evaluation
{
	/*public static class NRefactoryExtensions
	{
		#region AstType

		public static object Resolve (this TypeSyntax type, EvaluationContext ctx)
		{
			var args = new List<object> ();
			var name = type.Resolve (ctx, args);

			//if (name.StartsWith ("global::", StringComparison.Ordinal))
			//	name = name.Substring ("global::".Length);

			if (string.IsNullOrEmpty (name))
				return null;

			if (args.Count > 0)
				return ctx.Adapter.GetType (ctx, name, args.ToArray ());

			return ctx.Adapter.GetType (ctx, name);
		}

		static string Resolve (this ExpressionSyntax type, EvaluationContext ctx, List<object> args)
		{
			if (type is PredefinedTypeSyntax)
				return Resolve ((PredefinedTypeSyntax) type, ctx, args);
			else if (type is PointerTypeSyntax)
				return Resolve ((PointerTypeSyntax)type, ctx, args);
			else if (type is NullableTypeSyntax)
				return Resolve ((NullableTypeSyntax)type, ctx, args);
			else if (type is RefTypeSyntax)
				return Resolve ((RefTypeSyntax)type, ctx, args);
			else if (type is QualifiedNameSyntax)
				return Resolve ((QualifiedNameSyntax) type, ctx, args);
			else if (type is IdentifierNameSyntax)
				return Resolve ((IdentifierNameSyntax)type, ctx, args);
			else if (type is GenericNameSyntax)
				return Resolve ((GenericNameSyntax)type, ctx, args);

			return null;
		}

		#endregion AstType

		#region ComposedType

		static string Resolve (this PointerTypeSyntax type, EvaluationContext ctx, List<object> args)
		{
			return type.ElementType.Resolve (ctx, args) + "*";
		}

		static string Resolve (this RefTypeSyntax type, EvaluationContext ctx, List<object> args)
		{
			return type.Type.Resolve (ctx, args);
		}

		static string Resolve (this NullableTypeSyntax type, EvaluationContext ctx, List<object> args)
		{
			args.Insert (0, type.ElementType.Resolve (ctx));
			return "System.Nullable`1";
		}

		#endregion ComposedType

		#region MemberType

		static string Resolve (this QualifiedNameSyntax type, EvaluationContext ctx, List<object> args)
		{
			string name;

			if (!(type.Left is AliasQualifiedNameSyntax)) {
				var parent = type.Left.Resolve (ctx, args);
				name = parent + "." + type.Right.Identifier.ValueText;
			} else {
				name = type.Right.Identifier.ValueText;
			}

			
			if (type.Right is GenericNameSyntax genericName) {
				name += "`" + genericName.TypeArgumentList.Arguments.Count;
				foreach (var arg in genericName.TypeArgumentList.Arguments) {
					object resolved;

					if ((resolved = arg.Resolve (ctx)) == null)
						return null;

					args.Add (resolved);
				}
			}

			return name;
		}

		#endregion MemberType

		#region PrimitiveType

		public static string Resolve (this PredefinedTypeSyntax type)
		{
			switch (type.Keyword.Kind()) {
			case SyntaxKind.BoolKeyword:    return "System.Boolean";
			case SyntaxKind.SByteKeyword:   return "System.SByte";
			case SyntaxKind.ByteKeyword:    return "System.Byte";
			case SyntaxKind.CharKeyword:    return "System.Char";
			case SyntaxKind.ShortKeyword:   return "System.Int16";
			case SyntaxKind.UShortKeyword:  return "System.UInt16";
			case SyntaxKind.IntKeyword:     return "System.Int32";
			case SyntaxKind.UIntKeyword:    return "System.UInt32";
			case SyntaxKind.LongKeyword:    return "System.Int64";
			case SyntaxKind.ULongKeyword:   return "System.UInt64";
			case SyntaxKind.FloatKeyword:   return "System.Single";
			case SyntaxKind.DoubleKeyword:  return "System.Double";
			case SyntaxKind.DecimalKeyword: return "System.Decimal";
			case SyntaxKind.StringKeyword:  return "System.String";
			case SyntaxKind.ObjectKeyword:  return "System.Object";
			case SyntaxKind.VoidKeyword:    return "System.Void";
			default: return null;
			}
		}

		public static string Resolve(string shortTypeName)
		{
			string longName;
			switch (shortTypeName) {
			case "bool": longName = "System.Boolean"; break;
			case "byte": longName = "System.Byte"; break;
			case "sbyte": longName = "System.SByte"; break;
			case "char": longName = "System.Char"; break;
			case "decimal": longName = "System.Decimal"; break;
			case "double": longName = "System.Double"; break;
			case "float": longName = "System.Single"; break;
			case "int": longName = "System.Int32"; break;
			case "uint": longName = "System.UInt32"; break;
			case "nint": longName = "System.IntPtr"; break;
			case "nuint": longName = "System.UIntPtr"; break;
			case "long": longName = "System.Int64"; break;
			case "ulong": longName = "System.UInt64"; break;
			case "short": longName = "System.Int16"; break;
			case "ushort": longName = "System.UInt16"; break;
			case "object": longName = "System.Object"; break;
			case "string": longName = "System.String"; break;
			case "dynamic": longName = "System.Object"; break;
			default: throw new ArgumentException($"Unknown type {shortTypeName}");
			}
			return longName;
 		}

		static string Resolve (this PredefinedTypeSyntax type, EvaluationContext ctx, List<object> args)
		{
			return Resolve (type);
		}

		#endregion PrimitiveType

		#region SimpleType

		static string Resolve (this GenericNameSyntax type, EvaluationContext ctx, List<object> args)
		{
			string name = type.Identifier.ValueText;

			if (type.TypeArgumentList.Arguments.Count > 0) {
				name += "`" + type.TypeArgumentList.Arguments.Count;
				foreach (var arg in type.TypeArgumentList.Arguments) {
					object resolved;

					if ((resolved = arg.Resolve (ctx)) == null)
						return null;

					args.Add (resolved);
				}
			}

			return type.Identifier.ValueText;
		}

		static string Resolve (this IdentifierNameSyntax type, EvaluationContext ctx, List<object> args)
		{
			return type.Identifier.ValueText;
		}

		#endregion SimpleType
	}*/
}
