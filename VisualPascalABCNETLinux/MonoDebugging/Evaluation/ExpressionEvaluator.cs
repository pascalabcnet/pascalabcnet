// IExpressionEvaluator.cs
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
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Mono.Debugging.Backend;
using Mono.Debugging.Client;

namespace Mono.Debugging.Evaluation
{
	public class ExpressionEvaluator
	{
		public ExpressionEvaluator()
        {

        }

		public ValueReference Evaluate (EvaluationContext ctx, string expression)
		{
			return Evaluate (ctx, expression, null);
		}
		
		public virtual ValueReference Evaluate (EvaluationContext ctx, string expression, object expectedType)
		{
			foreach (var variable in ctx.Adapter.GetLocalVariables (ctx))
				if (variable.Name == expression)
					return variable;

			foreach (var parameter in ctx.Adapter.GetParameters (ctx))
				if (parameter.Name == expression)
					return parameter;

			var thisVar = ctx.Adapter.GetThisReference (ctx);
			if (thisVar != null) {
				if (thisVar.Name == expression)
					return thisVar;
				foreach (var cv in thisVar.GetChildReferences (ctx.Options))
					if (cv.Name == expression)
						return cv;
			}
			throw new EvaluatorException ("Invalid Expression: '{0}'", expression);
		}
		
		public virtual ValidationResult ValidateExpression (EvaluationContext ctx, string expression)
		{
			return new ValidationResult (true, null);
		}

		public string TargetObjectToString (EvaluationContext ctx, object obj)
		{
			var res = ctx.Adapter.TargetObjectToObject (ctx, obj);

			if (res == null)
				return null;

			if (res is EvaluationResult)
				return ((EvaluationResult) res).DisplayValue ?? ((EvaluationResult) res).Value;

			return res.ToString ();
		}

		public EvaluationResult TargetObjectToExpression (EvaluationContext ctx, object obj)
		{
			return ToExpression (ctx, ctx.Adapter.TargetObjectToObject (ctx, obj));
		}
		
		public virtual EvaluationResult ToExpression (EvaluationContext ctx, object obj)
		{
			if (obj == null)
				return new EvaluationResult ("null");

			if (obj is IntPtr ptr)
				return new EvaluationResult ("0x" + ptr.ToInt64 ().ToString ("x"));

			if (obj is char c) {
				string str;

				if (c == '\'')
					str = @"'\''";
				else if (c == '"')
					str = "'\"'";
				else
					str = EscapeString ("'" + c + "'");
				return new EvaluationResult (str, ((int) c) + " " + str);
			}

			if (obj is string s)
				return new EvaluationResult ("\"" + EscapeString (s) + "\"");

			if (obj is bool b)
				return new EvaluationResult (b ? "true" : "false");

			if (obj is decimal d)
				return new EvaluationResult (d.ToString (CultureInfo.InvariantCulture));

			if (obj is EvaluationResult result)
				return result;
			
			if (ctx.Options.IntegerDisplayFormat == IntegerDisplayFormat.Hexadecimal) {
				string fval = null;

				if (obj is sbyte)
					fval = ((sbyte) obj).ToString ("x2");
				else if (obj is int)
					fval = ((int) obj).ToString ("x4");
				else if (obj is short)
					fval = ((short) obj).ToString ("x8");
				else if (obj is long)
					fval = ((long) obj).ToString ("x16");
				else if (obj is byte)
					fval = ((byte) obj).ToString ("x2");
				else if (obj is uint)
					fval = ((uint) obj).ToString ("x4");
				else if (obj is ushort)
					fval = ((ushort) obj).ToString ("x8");
				else if (obj is ulong)
					fval = ((ulong) obj).ToString ("x16");
				
				if (fval != null)
					return new EvaluationResult ("0x" + fval);
			}
			
			return new EvaluationResult (obj.ToString ());
		}

		public static string EscapeString (string text)
		{
			var sb = new StringBuilder ();

			for (int i = 0; i < text.Length; i++) {
				char c = text[i];
				string txt;

				switch (c) {
				case '"': txt = "\\\""; break;
				case '\0': txt = @"\0"; break;
				case '\\': txt = @"\\"; break;
				case '\a': txt = @"\a"; break;
				case '\b': txt = @"\b"; break;
				case '\f': txt = @"\f"; break;
				case '\v': txt = @"\v"; break;
				case '\n': txt = @"\n"; break;
				case '\r': txt = @"\r"; break;
				case '\t': txt = @"\t"; break;
				default:
					if (char.GetUnicodeCategory (c) == UnicodeCategory.OtherNotAssigned) {
						sb.AppendFormat ("\\u{0:x4}", (int) c);
					} else {
						sb.Append (c);
					}
					continue;
				}
				sb.Append (txt);
			}

			return sb.ToString ();
		}
		
		public virtual bool CaseSensitive {
			get { return true; }
		}

		public string Resolve (DebuggerSession session, SourceLocation location, string expression)
        {
			return expression;
        }

		public virtual IEnumerable<ValueReference> GetLocalVariables (EvaluationContext ctx)
		{
			return ctx.Adapter.GetLocalVariables (ctx);
		}

		public virtual ValueReference GetThisReference (EvaluationContext ctx)
		{
			return ctx.Adapter.GetThisReference (ctx);
		}

		public virtual IEnumerable<ValueReference> GetParameters (EvaluationContext ctx)
		{
			return ctx.Adapter.GetParameters (ctx);
		}

		public virtual ValueReference GetCurrentException (EvaluationContext ctx)
		{
			return ctx.Adapter.GetCurrentException (ctx);
		}
	}
	
	[Serializable]
	public class EvaluatorException: Exception
	{
		protected EvaluatorException (SerializationInfo info, StreamingContext context)
			: base (info, context)
		{
		}
		
		public EvaluatorException (string msg, params object[] args): base (string.Format (msg, args))
		{
		}

		public EvaluatorException (Exception innerException, string msg, params object [] args) : base (string.Format (msg, args), innerException)
		{
		}
	}

	[Serializable]
	public class EvaluatorAbortedException: EvaluatorException
	{
		protected EvaluatorAbortedException (SerializationInfo info, StreamingContext context)
			: base (info, context)
		{
		}

		public EvaluatorAbortedException ()
			: base ("Aborted.")
		{
		}
	}

	[Serializable]
	public class NotSupportedExpressionException: EvaluatorException
	{
		protected NotSupportedExpressionException (SerializationInfo info, StreamingContext context)
			: base (info, context)
		{
		}
		
		public NotSupportedExpressionException ()
			: base ("Expression not supported.")
		{
		}
	}

	[Serializable]
	public class ImplicitEvaluationDisabledException: EvaluatorException
	{
		protected ImplicitEvaluationDisabledException (SerializationInfo info, StreamingContext context)
			: base (info, context)
		{
		}
		
		public ImplicitEvaluationDisabledException ()
			: base ("Implicit property and method evaluation is disabled.")
		{
		}
	}
}
